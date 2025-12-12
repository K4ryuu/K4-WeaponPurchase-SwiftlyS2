using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Commands;
using SwiftlyS2.Shared.Players;
using SwiftlyS2.Shared.Plugins;
using SwiftlyS2.Shared.SchemaDefinitions;

namespace K4WeaponPurchase;

[PluginMetadata(
	Id = "k4.weaponpurchase",
	Version = "1.0.1",
	Name = "K4 - Weapon Purchase",
	Author = "K4ryuu",
	Description = "Purchase weapons, grenades and more through commands"
)]
public sealed partial class Plugin(ISwiftlyCore core) : BasePlugin(core)
{
	private const string ConfigFileName = "k4-weaponpurchase.jsonc";
	private const string ConfigSection = "K4WeaponPurchase";

	public static IOptionsMonitor<PluginConfig> Config { get; private set; } = null!;

	public override void Load(bool hotReload)
	{
		Core.Configuration
			.InitializeJsonWithModel<PluginConfig>(ConfigFileName, ConfigSection)
			.Configure(builder =>
			{
				builder.AddJsonFile(ConfigFileName, optional: false, reloadOnChange: true);
			});

		ServiceCollection services = new();
		services.AddSwiftly(Core)
			.AddOptionsWithValidateOnStart<PluginConfig>()
			.BindConfiguration(ConfigSection);

		var provider = services.BuildServiceProvider();
		Config = provider.GetRequiredService<IOptionsMonitor<PluginConfig>>();

		foreach (var (className, weaponConfig) in Config.CurrentValue.Weapons)
		{
			foreach (var alias in weaponConfig.Aliases)
			{
				Core.Command.RegisterCommand(alias, ctx => OnPurchaseCommand(ctx, className));
			}
		}
	}

	public override void Unload()
	{
		// Nothing to clean up
	}

	private void OnPurchaseCommand(ICommandContext ctx, string className)
	{
		var player = ctx.Sender;
		if (player == null || !player.IsValid)
			return;

		var controller = player.Controller;
		var pawn = player.PlayerPawn;

		if (controller == null || pawn == null)
			return;

		var localizer = Core.Translation.GetPlayerLocalizer(player);

		// Check if player is alive
		if (pawn.Health <= 0 || controller.Team <= Team.Spectator)
		{
			player.SendChat($" {localizer["k4.general.prefix"]} {localizer["k4.weaponpurchase.dead"]}");
			return;
		}

		// Check buy zone
		if (Config.CurrentValue.CheckBuyZone && !pawn.InBuyZone)
		{
			player.SendChat($" {localizer["k4.general.prefix"]} {localizer["k4.weaponpurchase.not_in_buyzone"]}");
			return;
		}

		// Check buy time
		if (Config.CurrentValue.CheckBuyTime && Core.EntitySystem.GetGameRules()?.BuyTimeEnded == true)
		{
			player.SendChat($" {localizer["k4.general.prefix"]} {localizer["k4.weaponpurchase.buytime_ended"]}");
			return;
		}

		// Get weapon data
		var weaponIndex = Core.Helpers.GetDefinitionIndexByClassname(className);
		if (!weaponIndex.HasValue)
		{
			Core.Logger.LogWarning("Weapon not found: {ClassName}", className);
			return;
		}

		var weaponData = Core.Helpers.GetWeaponCSDataFromKey(weaponIndex.Value);
		if (weaponData == null)
		{
			Core.Logger.LogWarning("Weapon data not found for: {ClassName}", className);
			return;
		}

		// Get price (custom or default from game data)
		var weaponConfig = Config.CurrentValue.Weapons[className];
		var price = weaponConfig.CustomPrice ?? weaponData.Price;

		// Check money
		var moneyServices = controller.InGameMoneyServices;
		if (moneyServices == null || moneyServices.Account < price)
		{
			player.SendChat($" {localizer["k4.general.prefix"]} {localizer["k4.weaponpurchase.not_enough_money", price]}");
			return;
		}

		// Check grenade limits
		if (weaponData.GearSlot == gear_slot_t.GEAR_SLOT_GRENADES)
		{
			var grenadeLimit = Core.ConVar.Find<int>("ammo_grenade_limit_total")?.Value ?? 4;
			var grenadeCount = CountItemsInSlot(pawn, gear_slot_t.GEAR_SLOT_GRENADES);

			if (grenadeCount >= grenadeLimit)
			{
				player.SendChat($" {localizer["k4.general.prefix"]} {localizer["k4.weaponpurchase.grenade_limit", grenadeLimit]}");
				return;
			}

			if (className == "weapon_flashbang")
			{
				var flashLimit = Core.ConVar.Find<int>("ammo_grenade_limit_flashbang")?.Value ?? 2;
				var flashCount = CountWeaponByClassName(pawn, "weapon_flashbang");

				if (flashCount >= flashLimit)
				{
					player.SendChat($" {localizer["k4.general.prefix"]} {localizer["k4.weaponpurchase.flashbang_limit", flashLimit]}");
					return;
				}
			}
			else
			{
				var defaultLimit = Core.ConVar.Find<int>("ammo_grenade_limit_default")?.Value ?? 1;
				var weaponCount = CountWeaponByClassName(pawn, className);

				if (weaponCount >= defaultLimit)
				{
					player.SendChat($" {localizer["k4.general.prefix"]} {localizer["k4.weaponpurchase.default_limit", defaultLimit]}");
					return;
				}
			}
		}
		// Check healthshot limits
		else if (className == "weapon_healthshot")
		{
			var healthshotLimit = Core.ConVar.Find<int>("ammo_item_limit_healthshot")?.Value ?? 4;
			var healthshotCount = CountWeaponByClassName(pawn, "weapon_healthshot");

			if (healthshotCount >= healthshotLimit)
			{
				player.SendChat($" {localizer["k4.general.prefix"]} {localizer["k4.weaponpurchase.healthshot_limit", healthshotLimit]}");
				return;
			}
		}
		// Handle weapon slots (drop existing)
		else if (weaponData.GearSlot is gear_slot_t.GEAR_SLOT_RIFLE or gear_slot_t.GEAR_SLOT_PISTOL)
		{
			pawn.WeaponServices?.DropWeaponBySlot(weaponData.GearSlot);
		}

		// Deduct money
		moneyServices.Account -= price;
		moneyServices.AccountUpdated();

		// Give weapon
		pawn.ItemServices?.GiveItem(className);

		// Success message - display name from translation
		var displayNameKey = $"k4.weapon.{className.Replace("weapon_", "").Replace("item_", "")}";
		var displayName = localizer[displayNameKey];
		player.SendChat($" {localizer["k4.general.prefix"]} {localizer["k4.weaponpurchase.purchase_success", displayName, price]}");
	}

	private int CountItemsInSlot(CCSPlayerPawn pawn, gear_slot_t slot)
	{
		var count = 0;
		var weaponServices = pawn.WeaponServices;
		if (weaponServices == null)
			return count;

		foreach (var weapon in weaponServices.MyValidWeapons)
		{
			var vdata = Core.Helpers.GetWeaponCSDataFromKey(weapon.AttributeManager.Item.ItemDefinitionIndex);
			if (vdata?.GearSlot == slot)
				count++;
		}

		return count;
	}

	private static int CountWeaponByClassName(CCSPlayerPawn pawn, string className)
	{
		var count = 0;
		var weaponServices = pawn.WeaponServices;
		if (weaponServices == null)
			return count;

		foreach (var weapon in weaponServices.MyValidWeapons)
		{
			if (weapon.DesignerName?.Equals(className, StringComparison.OrdinalIgnoreCase) == true)
				count++;
		}

		return count;
	}
}
