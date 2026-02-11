namespace K4WeaponPurchase;

public sealed class PluginConfig
{
	public Dictionary<string, WeaponConfig> Weapons { get; set; } = new()
	{
		// Rifles
		["weapon_ak47"] = new() { Aliases = ["ak47", "ak"] },
		["weapon_m4a1_silencer"] = new() { Aliases = ["m4a1s", "m4s"] },
		["weapon_m4a1"] = new() { Aliases = ["m4a1", "m4", "m4a4"] },
		["weapon_awp"] = new() { Aliases = ["awp"] },
		["weapon_aug"] = new() { Aliases = ["aug"] },
		["weapon_famas"] = new() { Aliases = ["famas"] },
		["weapon_galilar"] = new() { Aliases = ["galilar", "galil"] },
		["weapon_sg556"] = new() { Aliases = ["sg556", "sg"] },
		["weapon_ssg08"] = new() { Aliases = ["ssg", "scout", "ssg08"] },
		["weapon_scar20"] = new() { Aliases = ["scar20", "scar"] },
		["weapon_g3sg1"] = new() { Aliases = ["g3sg1"] },
		["weapon_m249"] = new() { Aliases = ["m249"] },
		["weapon_negev"] = new() { Aliases = ["negev"] },

		// SMGs
		["weapon_mp9"] = new() { Aliases = ["mp9"] },
		["weapon_mp7"] = new() { Aliases = ["mp7"] },
		["weapon_mp5sd"] = new() { Aliases = ["mp5sd", "mp5"] },
		["weapon_ump45"] = new() { Aliases = ["ump45", "ump"] },
		["weapon_p90"] = new() { Aliases = ["p90"] },
		["weapon_bizon"] = new() { Aliases = ["bizon"] },
		["weapon_mac10"] = new() { Aliases = ["mac10", "mac"] },

		// Shotguns
		["weapon_nova"] = new() { Aliases = ["nova"] },
		["weapon_xm1014"] = new() { Aliases = ["xm1014", "xm"] },
		["weapon_sawedoff"] = new() { Aliases = ["sawedoff"] },
		["weapon_mag7"] = new() { Aliases = ["mag7", "mag"] },

		// Pistols
		["weapon_deagle"] = new() { Aliases = ["deagle"] },
		["weapon_elite"] = new() { Aliases = ["elite", "dualberettas"] },
		["weapon_fiveseven"] = new() { Aliases = ["fiveseven"] },
		["weapon_glock"] = new() { Aliases = ["glock", "glock18"] },
		["weapon_hkp2000"] = new() { Aliases = ["hkp2000", "hkp", "p2000"] },
		["weapon_p250"] = new() { Aliases = ["p250"] },
		["weapon_tec9"] = new() { Aliases = ["tec9", "tec"] },
		["weapon_usp_silencer"] = new() { Aliases = ["usp_silencer", "usp", "usp-s"] },
		["weapon_cz75a"] = new() { Aliases = ["cz75a", "cz", "cs75-auto"] },
		["weapon_revolver"] = new() { Aliases = ["revolver", "r8revolver", "r8"] },

		// Grenades
		["weapon_flashbang"] = new() { Aliases = ["flashbang", "flash"] },
		["weapon_smokegrenade"] = new() { Aliases = ["smokegrenade", "smoke"] },
		["weapon_hegrenade"] = new() { Aliases = ["hegrenade", "grenade", "he"] },
		["weapon_molotov"] = new() { Aliases = ["molotov"] },
		["weapon_incgrenade"] = new() { Aliases = ["incgrenade"] },
		["weapon_decoy"] = new() { Aliases = ["decoy"] },
		["weapon_tagrenade"] = new() { Aliases = ["tagrenade"] },

		// Utility
		["weapon_taser"] = new() { Aliases = ["taser"] },
		["weapon_healthshot"] = new() { Aliases = ["healthshot"] },
		["item_kevlar"] = new() { Aliases = ["kevlar"] },
		["weapon_shield"] = new() { Aliases = ["shield"] },
	};

	public bool CheckBuyZone { get; set; } = true;
	public bool CheckBuyTime { get; set; } = true;
}

public sealed class WeaponConfig
{
	public List<string> Aliases { get; set; } = [];
	public int? CustomPrice { get; set; }
	public List<string>? AllowedMaps { get; set; }
	public List<string>? DisabledMaps { get; set; }
	public List<string>? AllowedTeams { get; set; } // CT, T, or empty for both
}
