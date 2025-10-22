
using System;
using System.IO;
using System.Threading;
using System.Linq;
using System.Text;
//using System.Numerics;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReflectionUtility;
using NeoModLoader.api.attributes;
using NeoModLoader.General;
using ai;
using HarmonyLib;
using NCMS;
using NCMS.Utils;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.CanvasScaler;

//Alt+3
namespace ZeN_01
{
	class Traits03
	{
		public static void init()
		{// 遊戲載入時調用這個函式
            // 呼叫自動化註冊函式
            registerOther666Traits(); 
		}

    // --- 步驟一：將自動化註冊邏輯貼在這裡 ---
    
    private static void registerOther666Traits()
    {
        // [持續時間, 標記ID, 圖標名稱]
        var traitConfigs = new (float duration, string markerId, string iconName)[]
        {
            (3600f, "dm6661", "Other6661"),
            (3200f, "dm6662", "Other6662"),
            (2800f, "dm6663", "Other6663"),
            (2400f, "dm6664", "Other6664"),
            (2000f, "dm6665", "Other6665"),
            (1600f, "dm6666", "Other6666"),
            (1200f, "dm6667", "Other6667"),
            ( 800f, "dm6668", "Other6668"),
            ( 400f, "dm6669", "Other6669"),
        };

        int index = 1;

        foreach (var config in traitConfigs)
        {
            string traitId = $"other666{index}";
            
            ActorTrait newTrait = new ActorTrait();
            newTrait.id = traitId;
            // 注意：請確保 Assets/ui/icons/Other/ 資料夾下有這些圖標
            newTrait.path_icon = $"ui/icons/Other/{config.iconName}"; 
            newTrait.group_id = "auxiliary_traits";
            newTrait.rarity = Rarity.R2_Epic;
            newTrait.can_be_given = false;
            // 使用 Lambda 捕獲當前迴圈的變數，並傳遞給 Traits01Actions 中的合併函式
            newTrait.action_special_effect = new WorldAction(delegate(BaseSimObject pTarget, WorldTile pTile)
            {
                // **請確認您合併後的函式在 Traits01Actions 類別中**
                return Traits01Actions.addDemonKingStatus_Combined(pTarget, config.duration, config.markerId, pTile);
            });
            
            AssetManager.traits.add(newTrait);
            
            // 本地化
            addToLocalizedLibrary("ch", traitId, "魔神附體者", "在世界重啟時他將獲得相應的魔王之力");
            addToLocalizedLibrary("en", traitId, "Demon Possessed", "When the world restarts, he will gain the corresponding power of the Demon King.");
            
            newTrait.unlock(true);
            index++;
        }
    }
	///////////////////////////////////////////////////////////////////////////////////////////
		public static void addToLocalizedLibrary(string planguage, string id, string name, string description)
		{
			// ... (這個函式保持不變)
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
				LocalizedTextManager.add("trait_" + id, name);
				LocalizedTextManager.add("trait_" + id + "_info", description);
			}
		}
	}
}