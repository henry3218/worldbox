using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ReflectionUtility;
using UnityEngine;
using NeoModLoader.api.attributes;
using NeoModLoader.General;
using ai;
using HarmonyLib;
using NCMS;
using NCMS.Utils;
using System.Reflection;

namespace ZeN_01
{
	class ItemsGroup
	{
		public static string equipment_GROUP_00_ID = "armament_group01";

		public static void init()
		{
			// --- 第1群組 ---
			ItemGroupAsset ZeNequipmentGroup00 = new ItemGroupAsset // 使用正確的 ItemGroupAsset
			{
				id = equipment_GROUP_00_ID, // ID 保持為 "testgroup03"
				name = "equipment_group_" + equipment_GROUP_00_ID,
				color = "#00f7ff" // TEAL
			};
			AssetManager.item_groups.add(ZeNequipmentGroup00);
			addToLocalizedLibrary("ch", ZeNequipmentGroup00.id, "ZeN的武裝");
			addToLocalizedLibrary("en", ZeNequipmentGroup00.id, "ZeN's Armament");

		}
	
		// 修改 addToLocalizedLibrary 以匹配 ItemGroupAsset 的本地化鍵模式
		public static void addToLocalizedLibrary(string planguage, string id, string name)
		{// 新增到本地化資料庫
			string language = Reflection.GetField(LocalizedTextManager.instance.GetType(), LocalizedTextManager.instance, "language") as string;
			string templanguage = language;
			
			if (templanguage != "ch" && templanguage != "en")
			{
				templanguage = "en";
			}

			if (planguage == templanguage)
			{
				LocalizedTextManager.add("equipment_group_" + id, name);
			}
		}
	}
}