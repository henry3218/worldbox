using HarmonyLib;
using NCMS;
using NCMS.Utils;
using NeoModLoader.api.attributes;
using NeoModLoader.General;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using UnityEngine;
using ReflectionUtility;

namespace ZeN_01
{
	public class Items_original
	{
		[Hotfixable]
		public static void init()
		{
			loadCustomItems();
		}
		private static void loadCustomItems()
		{
			ItemAsset white_staff = AssetManager.items.get("white_staff"); //白法杖
			white_staff.base_stats.set("projectiles", 4);

			ItemAsset druid_staff = AssetManager.items.get("druid_staff"); //德魯杖
			druid_staff.base_stats.set("projectiles", 20f);
			druid_staff.durability = 300;

			ItemAsset necromancer_staff = AssetManager.items.get("necromancer_staff"); //死靈杖
			necromancer_staff.base_stats.set("projectiles", 4f);
			necromancer_staff.durability = 300;

			ItemAsset bow_wood = AssetManager.items.get("bow_wood"); // 木弓
			bow_wood.base_stats.set("recoil", -1f);

			ItemAsset bow_copper = AssetManager.items.get("bow_copper"); // 銅弓
			bow_copper.base_stats.set("recoil", -1f);

			ItemAsset bow_bronze = AssetManager.items.get("bow_bronze"); // 青銅弓
			bow_bronze.base_stats.set("recoil", -1f);

			ItemAsset bow_silver = AssetManager.items.get("bow_silver"); // 銀弓
			bow_silver.base_stats.set("recoil", -1f);

			ItemAsset bow_iron = AssetManager.items.get("bow_iron"); // 鐵弓
			bow_iron.base_stats.set("recoil", -1f);

			ItemAsset bow_steel = AssetManager.items.get("bow_steel"); // 鋼弓
			bow_steel.base_stats.set("recoil", -1f);

			ItemAsset bow_mythril = AssetManager.items.get("bow_mythril"); // 秘銀弓
			bow_mythril.base_stats.set("recoil", -1f);

			ItemAsset bow_adamantine = AssetManager.items.get("bow_adamantine"); // 精金弓
			bow_adamantine.base_stats.set("recoil", -1f);

			//addWeaponsToWorld();
		}
/*		public static void addWeaponsToWorld()
		{// 增加武器到世界
			//now walker can spawn with ice sword
			var walker = AssetManager.actor_library.get("cold_one");
			if (walker != null)
			{
				walker.use_items = true;
				walker.default_weapons = AssetLibrary<ActorAsset>.a<string>(new string[]
				{
					"ice_hammer", "ice_sword"
				});
				walker.take_items = false;
			}
		}*/
		public static void addToLocalizedLibrary(string planguage, string id, string name, string description)
		{// 新增到本地化資料庫
			string language = Reflection.GetField(LocalizedTextManager.instance.GetType(), LocalizedTextManager.instance, "language") as string;
			string templanguage;
			templanguage = language;
			if (templanguage != "ch" && templanguage != "en")
			{
				templanguage = "en";
			}
			if (planguage == templanguage)
			{
				//Dictionary<string, string> localizedText = Reflection.GetField(LocalizedTextManager.instance.GetType(), LocalizedTextManager.instance, "localizedText") as Dictionary<string, string>;
				LocalizedTextManager.add("item_" + id, name);
				LocalizedTextManager.add(id + "_description", description);
			}
		}
		public static Sprite[] getWeaponSprites(string id)
		{
			var sprite = Resources.Load<Sprite>("weapons/" + id);
			if (sprite != null)
				return new Sprite[] { sprite };
			else
			{
				//DarkieTraitsMain.LogError("Can not find weapon sprite for weapon with this id: " + id);
				return Array.Empty<Sprite>();
			}
		}
	}
}
