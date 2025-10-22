using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using NeoModLoader.General;
using UnityEngine;
using ai.behaviours;
using System.Reflection;
using Newtonsoft.Json;
using NeoModLoader.api.attributes;

namespace ZeN_01
{
	internal class patch
	{
		
		[Serializable]
		public class SavedStatusEffectData
		{
			public string asset_id;
			public string target_actor_id;
			public float remaining_time;
		}
		// 所有補丁和相關邏輯都放在這個類別裡面
		private static readonly Dictionary<int, string> KillMilestoneTraits = new()
		{
			{10, "veteran"}
		};

		// Harmony 后缀补丁：在 Actor 完成击杀动作后执行
		[HarmonyPostfix]
		[HarmonyPatch(typeof(Actor), "newKillAction")]
		public static void Actor_newKillAction_Postfix(Actor __instance)
		{
			if (__instance == null || !__instance.isAlive()) return;

			string targetAwardTraitID = null;
			int targetKills = 0;
			
			foreach (var milestone in KillMilestoneTraits.Reverse())
			{
				if (__instance.data.kills >= milestone.Key)
				{
					targetKills = milestone.Key;
					targetAwardTraitID = milestone.Value;
					break;
				}
			}

			if (targetAwardTraitID != null && !__instance.hasTrait(targetAwardTraitID))
			{
				__instance.addTrait(targetAwardTraitID, false);
				string lotteryTraitID = "b0002";
				if (!__instance.hasTrait(lotteryTraitID))
				{
					__instance.addTrait(lotteryTraitID, false);
				}
			}
		}

		// 修正 ItemCrafting.craftItem 補丁
		[HarmonyPrefix]
		[HarmonyPatch(typeof(ItemCrafting), "craftItem")]
		public static bool Prefix_ItemCrafting_craftItem(Actor pActor, string pCreatorName, EquipmentType pType, int pTries, City pCity)
		{
			if (pActor == null || pType == null || pCity == null)
			{
				return false;
			}
			return true;
		}

		// 修正 ItemCrafting.tryToCraftRandomWeapon 補丁
		[HarmonyPrefix]
		[HarmonyPatch(typeof(ItemCrafting), "tryToCraftRandomWeapon")]
		public static bool Prefix_ItemCrafting_tryToCraftRandomWeapon(Actor pActor, City pCity)
		{
			// 檢查 Actor 和 City 是否為 null (這是必需的輸入)
			if (pActor == null || pCity == null)
			{
				return false;
			}
			// 額外檢查：確保角色的數據結構完整
			if (pActor.data == null)
			{
				return false;
			}
			return true;
		}

		// 修正 BehMakeItem.execute 補丁
		[HarmonyPrefix]
		[HarmonyPatch(typeof(BehMakeItem), "execute")]
		public static bool Prefix_BehMakeItem_execute(Actor pActor)
		{
			if (pActor == null || pActor.city == null)
			{
				return false;
			}
			return true;
		}

		// 修復 City.updateCityStatus 補丁，防止 _cached_book_ids 為空時引發錯誤
		[HarmonyPrefix]
		[HarmonyPatch(typeof(City), "updateCityStatus")]
		public static bool Prefix_City_updateCityStatus(City __instance)
		{
			// 透過反射取得私有欄位 _cached_book_ids
			var field = typeof(City).GetField("_cached_book_ids", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			var cachedBookIds = field.GetValue(__instance) as List<long>;

			// 如果列表為空，則手動初始化它
			if (cachedBookIds == null)
			{
				field.SetValue(__instance, new List<long>());
			}

			// 繼續執行原始函式
			return true;
		}

		// 修復 CityBehBuild 補丁
		[HarmonyPrefix]
		[HarmonyPatch(typeof(CityBehBuild), "calcPossibleBuildings")]
		public static bool Prefix_CityBehBuild_calcPossibleBuildings(City pCity)
		{
			ActorAsset tActorAsset = pCity.getActorAsset();
			var buildOrders = AssetManager.city_build_orders.get(tActorAsset.build_order_template_id);
			if (buildOrders == null || buildOrders.list == null)
			{
				return false;
			}
			return true;
		}

		// 修復 Actor.calculateMainSprite 補丁
		[HarmonyPrefix]
		[HarmonyPatch(typeof(Actor), "calculateMainSprite")]
		public static bool Prefix_calculateMainSprite(Actor __instance)
		{
			if (__instance == null || __instance.asset == null)
			{
				// 這是您原本的檢查
				return false;
			}
			// *** 新增檢查：確保核心數據和種族存在 ***
			if (__instance.data == null)
			{
				return false; // 數據結構是空的，無法計算精靈圖
			}
			if (__instance.asset.id == null) 
			{
				// 如果種族是空的，通常角色不應該存在或無法被渲染
				return false; 
			}
			// 如果所有關鍵數據都存在，才繼續執行原版方法
			return true;
		}

		[HarmonyPrefix]
		[HarmonyPatch(typeof(StatsWindow), "showStatsMetaKingdom")]
		public static bool Prefix_StatsWindow_showStatsMetaKingdom(Kingdom pKingdom)
		{
			// 如果 pKingdom 是空值，則跳過原始函式，直接返回 false
			if (pKingdom == null)
			{
				return false;
			}
			// 否則，繼續執行原始函式
			return true;
		}
[HarmonyPrefix]
[HarmonyPatch(typeof(City), "update")]
public static bool Prefix_City_update_Safety(City __instance)
{
	// 1. 基本安全檢查：確保城市本身存活且有單位列表
	if (__instance == null || __instance.units == null)
	{
		return false;
	}

	// 2. 創建一個新的列表來存儲有效的單位
	List<Actor> validUnits = new List<Actor>();
	int originalCount = __instance.units.Count;

	for (int i = 0; i < originalCount; i++)
	{
		Actor unit = __instance.units[i];

		// 檢查條件：
		// a) 單位必須有效且存活
		// b) 單位的 city 屬性必須仍然指向當前城市 (__instance)
		if (unit != null && unit.isAlive() && unit.city == __instance)
		{
			validUnits.Add(unit);
		}
		// 如果 unit.city != __instance，表示這個單位已經被你的 Mod 邏輯搶走或城市關係被清除，
		// 它將不會被添加到 validUnits 中，從而達到清理的目的。
	}

	// 3. 【核心修正 CS0200】：使用 Clear() 和 AddRange() 來替換列表內容
	if (originalCount != validUnits.Count)
	{
		__instance.units.Clear();
		__instance.units.AddRange(validUnits);
		
		// 額外建議：如果你的環境允許，可以嘗試記錄日誌來確認清理動作
		// Debug.Log($"City {__instance.name} cleaned up {originalCount - validUnits.Count} invalid units.");
	}

	// 4. 允許原始 City.update 方法執行
	return true; 
}

[HarmonyPrefix]
[HarmonyPatch(typeof(UtilityBasedDecisionSystem), "useOn")]
public static bool Prefix_UtilityBasedDecisionSystem_useOn(Actor pActor)
{
	// 檢查 Actor 本身是否為 null (儘管不太可能)
	if (pActor == null)
	{
		return false;
	}

	// 檢查 Actor 的核心數據和資產是否有效
	// AI 決策系統高度依賴這些屬性
	if (pActor.data == null || pActor.asset == null)
	{
		// 如果核心數據或種族資產丟失，阻止 AI 決策更新，防止崩潰。
		return false;
	}

	// 檢查單位是否還存活 (雖然 updateDecisions 裡通常會檢查)
	if (!pActor.isAlive())
	{
		// 如果單位已死亡/正在移除，阻止決策
		return false;
	}

	// 允許原始方法執行
	return true;
}

[HarmonyPrefix]
[HarmonyPatch(typeof(Clan), "getChief")]
public static bool Prefix_Clan_getChief(Clan __instance, ref Actor __result)
{
	// 檢查 Clan 實例的核心数据 'data' 是否為 null
	if (__instance.data == null)
	{
		// 如果數據為空，則直接返回 null，防止原方法執行並避免 NullReferenceException
		__result = null; 
		
		// 返回 false，阻止執行原始的 Clan.getChief() 方法
		return false;
	}
	
	// 數據存在，允許原始方法執行
	return true;
}
[HarmonyPatch(typeof(UnitAvatarLoader), "load")]
public static class UnitAvatarLoaderSafetyPatch
{
    public static bool Prefix(Actor pActor)
    {
        // 核心檢查 1：確保 Actor 核心數據結構存在
        if (pActor == null || pActor.data == null)
        {
            return false; // 阻止加載
        }

        // 核心檢查 2：確保裝備容器存在 (最常見的崩潰源)
        if (pActor.equipment == null)
        {
             // 可以嘗試修復，但最安全是直接阻止加載
             return false; 
        }

        // 所有檢查通過，允許原版方法繼續執行
        return true;
    }
}
// ====================================================================
// 【核心防崩潰補丁】 阻止特殊單位執行文明決策
// 解決 DecisionsLibrary+<>c.<initDecisionsTraits>b__22_6 (pActor.clan / pActor.kingdom 為 null) 的崩潰問題
// ====================================================================

[HarmonyPatch(typeof(UtilityBasedDecisionSystem), "registerDecisionArrayGameplay")]
public static class DecisionSystemSafetyPatch
{
    // 檢查 pActor 是否為您的特殊單位（奴隸、使徒或亡靈僕從）
    public static bool Prefix(Actor pActor)
    {
        // 基礎安全檢查
        if (pActor == null || !pActor.isAlive() || pActor.data == null)
        {
            return true; 
        }

        // 檢查 pActor 是否帶有您 Mod 創建的、需要特別處理的特質
        // 這裡集合了所有可能導致核心 AI 決策崩潰的單位
        bool isSpecialUnit = pActor.hasTrait("slave") || 
                             pActor.hasTrait("apostle") ||
                             pActor.hasTrait("undead_servant") || 
                             pActor.hasTrait("undead_servant2");

        if (isSpecialUnit)
        {
            // 如果單位是特殊單位，且其核心文明歸屬已被移除 (例如：clan 或 kingdom 為 null)
            // 這些 null 屬性會導致核心遊戲在檢查相關決策時崩潰。
            if (pActor.clan == null || pActor.kingdom == null)
            {
                // 返回 false：阻止執行原始的 registerDecisionArrayGameplay 方法，
                // 成功繞過會導致 NullReferenceException 的 DecisionsLibrary 邏輯。
                return false; 
            }
        }
        
        // 返回 true：允許所有其他單位（包括狀態正常的特殊單位）或非特殊單位執行原版方法
        return true; 
    }
}



	}
}