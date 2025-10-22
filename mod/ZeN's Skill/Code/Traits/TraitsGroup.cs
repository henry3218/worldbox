using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using ReflectionUtility;
using UnityEngine;
using NeoModLoader.api.attributes;
using NeoModLoader.General;
using ai;
using HarmonyLib;
using NCMS;
using NCMS.Utils;

namespace ZeN_01
{
    class TraitsGroup
    {
        // 将 ID 定义为游戏查找的格式：全部小写，下划线分隔
        public static string TEST_GROUP_01_ID = "profession"; 		// 專業
        public static string TEST_GROUP_02_ID = "talent"; 			// 天賦特長
        public static string TEST_GROUP_03_ID = "combatskill"; 		// 戰技
        public static string TEST_GROUP_03_2_ID = "magic_bullet"; 	// 戰技
        public static string TEST_GROUP_04_ID = "addmagic"; 		// 添加魔法
        public static string TEST_GROUP_05_ID = "statuseup"; 		// 強化
        public static string TEST_GROUP_06_ID = "holyarts"; 		// 神聖術
        public static string TEST_GROUP_07_ID = "evil_law"; 		// 邪法
        public static string TEST_GROUP_08_ID = "biome_magic"; 		// 群系魔法
        public static string TEST_GROUP_09_ID = "mn_magic"; 		// 建築魔法
        public static string TEST_GROUP_XX_ID = "auxiliary_traits"; // 輔助用
        public static string TEST_GROUP_X2_ID = "auxiliary_traits2"; // 輔助用

        public static void init()
        {
            // 第1群組
            ActorTraitGroupAsset ZeNGroup01 = new ActorTraitGroupAsset
            {
                id = TEST_GROUP_01_ID,
                name = $"trait_group_{TEST_GROUP_01_ID}", // 這裡會變成 "trait_group_test_group_01"
                color = "#ff0000" // 紅
            };
            AssetManager.trait_groups.add(ZeNGroup01);
            addToLocalizedLibrary("ch", ZeNGroup01.id, "專職特質");
            addToLocalizedLibrary("en", ZeNGroup01.id, "Profession");

            // 第2群組
            ActorTraitGroupAsset ZeNGroup02 = new ActorTraitGroupAsset
            {
                id = TEST_GROUP_02_ID,
                name = $"trait_group_{TEST_GROUP_02_ID}",
                color = "#ffff00" //澄
            };
            AssetManager.trait_groups.add(ZeNGroup02);
            addToLocalizedLibrary("ch", ZeNGroup02.id, "天賦特長");
            addToLocalizedLibrary("en", ZeNGroup02.id, "Talents and Specialties");

            // 第3群組
            ActorTraitGroupAsset ZeNGroup03 = new ActorTraitGroupAsset
            {
                id = TEST_GROUP_03_ID,
                name = $"trait_group_{TEST_GROUP_03_ID}",
                color = "#ffff00" //黃
            };
            AssetManager.trait_groups.add(ZeNGroup03);
            addToLocalizedLibrary("ch", ZeNGroup03.id, "戰鬥技");
            addToLocalizedLibrary("en", ZeNGroup03.id, "Battle Skills");

            // 第3-2群組
            ActorTraitGroupAsset ZeNGroup032 = new ActorTraitGroupAsset
            {
                id = TEST_GROUP_03_2_ID,
                name = $"trait_group_{TEST_GROUP_03_2_ID}",
                color = "#ffff00" //黃
            };
            AssetManager.trait_groups.add(ZeNGroup032);
            addToLocalizedLibrary("ch", ZeNGroup032.id, "魔法彈");
            addToLocalizedLibrary("en", ZeNGroup032.id, "Magic Bullet");

            // 第4群組
            ActorTraitGroupAsset ZeNGroup04 = new ActorTraitGroupAsset
            {
                id = TEST_GROUP_04_ID,
                name = $"trait_group_{TEST_GROUP_04_ID}",
                color = "#00ff00" //綠
            };
            AssetManager.trait_groups.add(ZeNGroup04);
            addToLocalizedLibrary("ch", ZeNGroup04.id, "附加魔法");
            addToLocalizedLibrary("en", ZeNGroup04.id, "Add Magic");

            // 第5群組
            ActorTraitGroupAsset ZeNGroup05 = new ActorTraitGroupAsset
            {
                id = TEST_GROUP_05_ID,
                name = $"trait_group_{TEST_GROUP_05_ID}",
                color = "#00ffff" //青
            };
            AssetManager.trait_groups.add(ZeNGroup05);
            addToLocalizedLibrary("ch", ZeNGroup05.id, "強化術");
            addToLocalizedLibrary("en", ZeNGroup05.id, "Status Up");

            // 第6群組
            ActorTraitGroupAsset ZeNGroup06 = new ActorTraitGroupAsset
            {
                id = TEST_GROUP_06_ID,
                name = $"trait_group_{TEST_GROUP_06_ID}",
                color = "#0000ff" //藍
            };
            AssetManager.trait_groups.add(ZeNGroup06);
            addToLocalizedLibrary("ch", ZeNGroup06.id, "神聖術");
            addToLocalizedLibrary("en", ZeNGroup06.id, "Holy Arts");

            // 第7群組
            ActorTraitGroupAsset ZeNGroup07 = new ActorTraitGroupAsset
            {
                id = TEST_GROUP_07_ID,
                name = $"trait_group_{TEST_GROUP_07_ID}",
                color = "#ff00ff" //紫
            };
            AssetManager.trait_groups.add(ZeNGroup07);
            addToLocalizedLibrary("ch", ZeNGroup07.id, "邪咒法");
            addToLocalizedLibrary("en", ZeNGroup07.id, "Evil Law");

            // 第8群組
            ActorTraitGroupAsset ZeNGroup08 = new ActorTraitGroupAsset
            {
                id = TEST_GROUP_08_ID,
                name = $"trait_group_{TEST_GROUP_08_ID}",
                color = "#ff00ff" //紫
            };
            AssetManager.trait_groups.add(ZeNGroup08);
            addToLocalizedLibrary("ch", ZeNGroup08.id, "群系變遷魔法");
            addToLocalizedLibrary("en", ZeNGroup08.id, "Biome Change");

            // 第9群組
            ActorTraitGroupAsset ZeNGroup09 = new ActorTraitGroupAsset
            {
                id = TEST_GROUP_09_ID,
                name = $"trait_group_{TEST_GROUP_09_ID}",
                color = "#ff00ff" //紫
            };
            AssetManager.trait_groups.add(ZeNGroup09);
            addToLocalizedLibrary("ch", ZeNGroup09.id, "建造魔法");
            addToLocalizedLibrary("en", ZeNGroup09.id, "Construction Magic");

            // 第X1群組
            ActorTraitGroupAsset ZeNGroupXX = new ActorTraitGroupAsset
            {
                id = TEST_GROUP_XX_ID,
                name = $"trait_group_{TEST_GROUP_XX_ID}",
                color = "#ff00ff" //紫
            };
            AssetManager.trait_groups.add(ZeNGroupXX);
            addToLocalizedLibrary("ch", ZeNGroupXX.id, "輔助特質");
            addToLocalizedLibrary("en", ZeNGroupXX.id, "Auxiliary Traits");

            // 第X2群組
            ActorTraitGroupAsset ZeNGroupX2 = new ActorTraitGroupAsset
            {
                id = TEST_GROUP_X2_ID,
                name = $"trait_group_{TEST_GROUP_X2_ID}",
                color = "#ff00ff" //紫
            };
            AssetManager.trait_groups.add(ZeNGroupX2);
            addToLocalizedLibrary("ch", ZeNGroupX2.id, "輔助特質");
            addToLocalizedLibrary("en", ZeNGroupX2.id, "Auxiliary Traits");
        }
        public static void addToLocalizedLibrary(string planguage, string id, string name)
        {// 新增到本地化資料庫
            string language = Reflection.GetField(LocalizedTextManager.instance.GetType(), LocalizedTextManager.instance, "language") as string;
            string templanguage = language;
            
            // 由於您確認遊戲內部中文代稱是 "ch"，所以這裡保持 "ch"
            if (templanguage != "ch" && templanguage != "en") // 假設遊戲還有俄語，可以包含進去
            {
                templanguage = "en"; // 如果不是中文、英文、俄文，則回歸英文
            }

            if (planguage == templanguage)
            {
                // 在這裡拼接 "trait_group_" 前綴，因為這是 LocalizedTextManager 真正查找的鍵
                // 這裡的 id 參數應該是 "testgroup01" 或 "testgroup02"
                LocalizedTextManager.add("trait_group_" + id, name);
            }
        }
    }
}