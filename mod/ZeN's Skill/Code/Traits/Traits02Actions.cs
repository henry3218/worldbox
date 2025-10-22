using System;
using System.Threading;
using System.Linq;
using System.Text;
using System.Numerics;
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

// 修正 CS0138: 'MapBox' is a type not a namespace.
// 通常在 WorldBox modding 中，直接使用 'MapBox.instance.world' 而不是通过 MapBox 作为 using 语句。

namespace ZeN_01
{
	class Traits02Actions
	{
		public static string weightedRandom(string[] options, float[] weights)
		{// 加權隨機
			if (options == null || weights == null || options.Length != weights.Length || options.Length == 0)
			{
				return null;
			}
			float totalWeight = weights.Sum();
			float randomValue = UnityEngine.Random.Range(0f, totalWeight);
			float cumulativeWeight = 0f;
			for (int i = 0; i < options.Length; i++)
			{
				cumulativeWeight += weights[i];
				if (randomValue < cumulativeWeight)
				{
					return options[i];
				}
			}
			return options.Last();	// 避免浮點數精度問題
		}
		public static bool randomChance(float probability)
		{// 隨機機會 修正 randomChance 的定義，確保它在 類別的頂層
			return UnityEngine.Random.Range(0f, 1f) < probability;
		}
		public static bool AutoremoveTrait00(BaseSimObject pTarget, WorldTile pTile = null)
		{// 亞種特質測試
			if (pTarget is Actor actor)
			{
				string prefrontalCortexTraitID = "prefrontal_cortex";	// 亞種特質 ID
				string removeTraitID = "b0001";	// 要移除的特質 ID

				// 判斷條件：
				// 如果單位 **沒有** "prefrontal_cortex" 特質
				// 並且單位 **擁有** "b0001" 特質
				// 才執行移除操作
				
				// 注意：這裡假設 actor.hasTrait() 可以檢查亞種特質。
				// 如果不行，您可能需要找到檢查 Actor.race.subspeciesTraits 的方法。
				// 例如：if (!actor.race.hasSubspeciesTrait(prefrontalCortexTraitID) && actor.hasTrait(removeTraitID))
				// 或者更直接的：if (!actor.hasTrait(prefrontalCortexTraitID) && actor.hasTrait(removeTraitID))
				
				// 目前暫時使用 actor.hasTrait() 來嘗試檢查 prefrontal_cortex
				if (!actor.hasSubspeciesTrait(prefrontalCortexTraitID) && actor.hasTrait(removeTraitID))
				{
					actor.removeTrait(removeTraitID);
					//Debug.Log($"單位 {actor.name} 沒有 {prefrontalCortexTraitID} 特質，已移除其 {removeTraitID} 特質。");
				}
				else if (actor.hasTrait(prefrontalCortexTraitID))
				{
					// 如果單位有 prefrontal_cortex，則不移除 b0001 特質
					// Debug.Log($"單位 {actor.name} 擁有 {prefrontalCortexTraitID} 特質，不移除 {removeTraitID} 特質。");
				}
				// 如果單位本來就沒有 b0001 特質，也不做任何事

				return true;	// 表示方法執行完畢
			}
			return false;	// pTarget 不是 Actor 類型，或為 null
		}
		public static bool AutoremoveTraitTT(BaseSimObject pTarget, WorldTile pTile = null)
		{// 亞種特質測試
			if (pTarget is not Actor actor)
			{
				return false;
			}

			string prefrontalCortexTraitID = "prefrontal_cortex";
			string removeTraitID = "b0001";	// 或者您想在這裡移除其他特定特質

			if (!actor.hasSubspeciesTrait(prefrontalCortexTraitID) && actor.hasTrait(removeTraitID))
			{
				actor.removeTrait(removeTraitID);
				//Debug.Log($"[AutoremoveTraitTT] 單位 {actor.name} 沒有文明亞種特質 ({prefrontalCortexTraitID})，已移除其 {removeTraitID} 特質。");
				return true;
			}
			return false;
		}
public static bool Randomness1(BaseSimObject pTarget, WorldTile pTile)
{// 總和抽選
	// 總和抽選
	Actor actor = pTarget as Actor;
	// 檢查 actor 是否有效且存活
	if (actor == null || !actor.isAlive())
	{
		return false;
	}
	string prefrontalCortexTraitID = "prefrontal_cortex";//指定亞種特質
	if (!actor.hasSubspeciesTrait(prefrontalCortexTraitID))
	{
		// 如果單位沒有 prefrontal_cortex 亞種特質，則阻止其進行抽獎
		//Debug.Log($"[Randomness1] 單位 {actor.name} 沒有文明亞種特質 ({prefrontalCortexTraitID})，跳過抽獎。");

		// 由於此單位不應獲得 b0001 特質，如果它意外地有了，在這裡移除它。
		// 這是一個額外的安全措施，以防其他地方將 b0001 賦予給了非文明生物。
		if (actor.hasTrait("b0001"))
		{
			actor.removeTrait("b0001");
			//Debug.Log($"[Randomness1] 單位 {actor.name} 為非文明生物，已移除其抽獎特質b0001。");
		}
		return false;	// 返回 false 表示沒有賦予任何抽獎特質
	}
	// =========================================================================
	// 定義特質清單和其對應的機率
	string[] possibleTraits = {		//預設分配,不等於實際分配
		"other001",					// 20000
		"other002",					// 20000
		"pro_king",					// 1
		"pro_warrior",			 	// 1
		"Pro_Leader",			  	// 10
		"pro_groupleader",		 	// 100
		"pro_soldier",			 	// 900
		"talent_diplomacy",			// 3899f
		"talent_warfare",		  	// 3899f
		"talent_stewardship",	  	// 3899f
		"talent_intelligence",	 	// 3899f
		"talent_coition",		  	// 4999
		"talent_build",				// 4999
		"status_powerup",			// 999
		"status_caffeinated",	  	// 504
		"status_enchanted",			// 999
		"status_rage",			 	// 999
		"status_spellboost",		// 499
		"status_motivated",			// 950
		"status_shield",			// 500
		"status_invincible",		// 10
		"status_inspired",		 	// 500
		"status_AFO",			  	// 10
		"status_OFA",			  	// 10
		"cb_slam",				 	// 3500
		"cb_bulletrain",			// 3992
		"cb_holdfast",			 	// 3992
		"cb_experience",			// 3992
		"projectile01",				// 1000 
		"projectile02",				// 1000 
		"projectile03",				// 1000 
		"projectile04",				// 900  
		"projectile05",				// 900  
		"projectile06",				// 500  
		"projectile07",				// 500  
		"projectile08",				// 500  
		"projectile09",				// 500  
		"projectile10",				// 30   
		"projectile11",				// 30   
		"projectile12",				// 100  
		"projectile13",				// 100  
		"projectile14",				// 10   
		"projectile15",				// 10   
		"add_burning",			 	// 850
		"add_slowdown",				// 850
		"add_frozen",			  	// 850
		"add_poisonous",			// 850
		"add_afc",				 	// 850
		"add_drowning",			 	// 700
		"add_confused",			 	// 700
		"add_silenced",				// 700
		"add_stunned",			 	// 700
		"add_unknown",			 	// 10
		"add_cursed",			  	// 10
		"add_death",				// 10
		"holyarts_ha",			 	// 100
		"holyarts_heal",			// 2000
		"holyarts_cure",			// 1000
		"holyarts_healcure",		// 100
		"holyarts_health",		 	// 4500
		"holyarts_mana",			// 4500
		"holyarts_stamina",			// 4500
		"holyarts_annunciation",	// 10
		"holyarts_consecration",	// 100
		"holyarts_eucharist",	  	// 100
		"holyarts_bond",	  		// 100
		"holyarts_serenity",	  	// 100
		"holyarts_rainfall",	  	// 100
		"holyarts_justice",	  		// 12
		"holyarts_divinelight",	  	// 12
		"evillaw_tgc",			 	// 100
		"evillaw_tc",			  	// 100
		"evillaw_disease",		 	// 50
		"evillaw_ea",			  	// 100
		"evillaw_sterilization",	// 50
		"evillaw_devour",		  	// 2
		"evillaw_starvation",	  	// 2
		"evillaw_moneylaw",			// 2
		"evillaw_sleeping",			// 2
		"evillaw_tantrum",			// 1
		"evillaw_seduction",		// 2
		"evillaw_ew"				// 1
	};

	float[] probabilities = {
		10000f,	 	// other001
		10000f,	 	// other002
		1f,		 	// pro_king
		1f,		 	// pro_warrior
		10f,		// Pro_Leader
		100f,	 	// pro_groupleader
		900f,		// pro_soldier
		3899f,		// talent_diplomacy
		3899f,	  	// talent_warfare
		3899f,	  	// talent_stewardship
		3899f,	 	// talent_intelligence
		4999f,	 	// talent_coition
		4999f,	 	// talent_build
		999f,	  	// status_powerup
		504f,	  	// status_caffeinated
		999f,	  	// status_enchanted
		999f,	  	// status_rage
		505f,	  	// status_spellboost
		999f,	  	// status_motivated
		505f,	  	// status_shield
		20f,		// status_invincible
		504f,	  	// status_inspired
		15f,		// status_AFO
		15f,		// status_OFA
		3500f,	 	// cb_slam
		3997f,	 	// cb_bulletrain
		3997f,	 	// cb_holdfast
		3997f,	 	// cb_experience
		1000f,	 	// projectile01
		1000f,	 	// projectile02
		1000f,	 	// projectile03
		900f,	  	// projectile04
		900f,	  	// projectile05
		500f,	  	// projectile06
		500f,	  	// projectile07
		500f,	  	// projectile08
		500f,	  	// projectile09
		30f,		// projectile10
		30f,		// projectile11
		100f,	  	// projectile12
		100f,	  	// projectile13
		15f,		// projectile14
		15f,		// projectile15
		813f,	  	// add_burning
		813f,	  	// add_slowdown
		813f,	  	// add_frozen
		813f,	  	// add_poisonous
		813f,	  	// add_afc
		813f,	  	// add_drowning
		830f,	  	// add_confused
		703.5f,	  	// add_silenced
		703.5f,	  	// add_stunned
		10f,		// add_unknown
		10f,		// add_cursed
		10f,		// add_death
		100f,	  	// holyarts_ha
		2000f,	 	// holyarts_heal
		1000f,	 	// holyarts_cure
		100f,	  	// holyarts_healcure
		4500f,	 	// holyarts_health
		4500f,	 	// holyarts_mana
		4500f,	 	// holyarts_stamina
		10f,		// holyarts_annunciation
		100f,	  	// holyarts_consecration
		100f,	  	// holyarts_eucharist
		100f,	  	// holyarts_bond
		100f,	  	// holyarts_serenity
		100f,	  	// holyarts_rainfall
		12f,	  	// holyarts_justice
		10f,	  	// holyarts_divinelight
		10f,	  	// evillaw_tgc
		100f,	  	// evillaw_tc
		100f,		// evillaw_disease
		100f,	  	// evillaw_ea
		50f,		// evillaw_sterilization
		2f,			// evillaw_devour
		2f,			// evillaw_starvation
		2f,			// evillaw_moneylaw
		2f,			// evillaw_sleeping
		1f,			// evillaw_tantrum
		2f,			// evillaw_seduction
		1f		 	// evillaw_ew
};
	// 確保權重總和為 100000
	float expectedTotalWeight = 100000f;
	// 使用 Linq 的 Sum() 方法計算總和
	float currentTotalWeight = probabilities.Sum();

	if (Math.Abs(currentTotalWeight - expectedTotalWeight) > 0.001f || probabilities.Length != possibleTraits.Length)	// 使用 Math.Abs 處理浮點數比較
	{
		Debug.LogError($"特質賦予的機率總和不等於 {expectedTotalWeight} 或特質數量不匹配！請檢查 probabilities 陣列。當前總和: {currentTotalWeight}，期望總和: {expectedTotalWeight}。");
		return false;
	}

	// 設定抽選次數
	int numRolls = 3;
	bool anyTraitGiven = false;

	for (int i = 0; i < numRolls; i++)
	{
		// 假設 weightedRandom 函數已經定義在其他地方
		// 例如：
		// private static string weightedRandom(string[] items, float[] weights) { ... }
		string traitToGive = weightedRandom(possibleTraits, probabilities);

		if (!string.IsNullOrEmpty(traitToGive))
		{
			if (!actor.hasTrait(traitToGive))
			{
				actor.addTrait(traitToGive);
				anyTraitGiven = true;
				//Debug.Log($"[Randomness1] 單位 {actor.name} 獲得特質：{traitToGive}");
			}
		}
	}

	// 抽獎特質 b0001 應該在抽獎後立即移除
	if (actor.hasTrait("b0001"))
	{
		actor.removeTrait("b0001");
		//Debug.Log($"[Randomness1] 單位 {actor.name} 的抽獎特質 b0001 已移除。");
	}

	return anyTraitGiven;
}
		public static bool Randomness2_1(BaseSimObject pTarget, WorldTile pTile)
		{// 第一抽取(抽選型,可調控次數次) 職業特長
			Actor actor = pTarget as Actor;	// 將 a 重新命名為 actor，與其他方法保持一致
			if (actor == null || !actor.isAlive())	// 檢查 actor 是否有效且存活
			{
				return false;
			}
			// 定義特質清單和其對應的機率
			// 請確保這些特質 ID 是在 WorldBox 中真實存在的
			string[] possibleTraits = {//包含銘謝惠顧在內總計12個特質
			"other001",				//01 銘謝惠顧 1,923
			"pro_king",				//02	1
			"pro_leader",			//03	1
			"pro_warrior",			//04	1
			"pro_groupleader",		//05	1
			"pro_soldier",			//06	20
			"talent_diplomacy",		//07	800
			"talent_warfare",		//08	800
			"talent_stewardship",	//09	800
			"talent_intelligence",	//10	800
			"talent_coition",		//1		800
			"talent_build",			//12	800
			};
			float[] probabilities = { 
			34f, 	//b0000
			1f, 	//pro_king
			2f, 	//pro_leader
			3f, 	//pro_warrior
			4f, 	//pro_groupleader
			8f, 	//pro_soldier
			8f, 	//talent_diplomacy
			8f, 	//talent_warfare
			8f, 	//talent_intelligence
			8f, 	//talent_coition
			8f, 	//talent_coition
			8f, 	//talent_build
			}; 
			if (probabilities.Sum() != 100f || probabilities.Length != possibleTraits.Length) 
			{
				Debug.LogError("第一抽取的機率總和不等於 100 或特質數量不匹配！請檢查 probabilities 陣列。");
				return false;
			}
			// 設定抽選次數
			int numRolls = 1; 
			bool anyTraitGiven = false; 
			for (int i = 0; i < numRolls; i++)
			{
				string traitToGive = weightedRandom(possibleTraits, probabilities); 
				
				if (!string.IsNullOrEmpty(traitToGive))
				{
					if (!actor.hasTrait(traitToGive)) 
					{
						actor.addTrait(traitToGive);
						anyTraitGiven = true;
					}
				}
			}
			//if (actor.hasTrait("b0002"))
			//{
			//	actor.removeTrait("b0002");
			//}
			return anyTraitGiven; 
		}
		public static bool Randomness2_2(BaseSimObject pTarget, WorldTile pTile)
		{// 第二抽取(抽選型,可調控次數次) 強化狀態
			Actor actor = pTarget as Actor;	// 將 a 重新命名為 actor，與其他方法保持一致
			if (actor == null || !actor.isAlive())	// 檢查 actor 是否有效且存活
			{
				return false;
			}
			// 定義特質清單和其對應的機率
			// 請確保這些特質 ID 是在 WorldBox 中真實存在的
			string[] possibleTraits = {//包含銘謝惠顧在內總計11個特質
			"other002",				//01 銘謝惠顧
			"status_powerup",		//02	10
			"status_caffeinated",	//03	10
			"status_enchanted",		//04	10
			"status_rage",			//05	10
			"status_spellboost",	//06	10
			"status_motivated",		//07	10
			"status_shield",		//08	10
			"status_inspired",		//09	10
			"status_invincible",	//10	1
			"status_AFO",			//1		1
			"status_OFA",			//12	1
			};
			float[] probabilities = { 
			33f, 	//b0000
			8f, 	//status_powerup
			8f, 	//status_caffeinated
			8f, 	//status_enchanted
			8f, 	//status_rage
			8f, 	//status_spellboost
			8f, 	//status_motivated
			8f, 	//status_shield
			8f, 	//status_inspired
			1f, 	//status_invincible
			1f, 	//status_AFO
			1f, 	//status_OFA
			}; 
			if (probabilities.Sum() != 100f || probabilities.Length != possibleTraits.Length) 
			{
				Debug.LogError("第二抽取的機率總和不等於 100 或特質數量不匹配！請檢查 probabilities 陣列。");
				return false;
			}
			// 設定抽選次數
			int numRolls = 1; 
			bool anyTraitGiven = false; 
			for (int i = 0; i < numRolls; i++)
			{
				string traitToGive = weightedRandom(possibleTraits, probabilities); 
				
				if (!string.IsNullOrEmpty(traitToGive))
				{
					if (!actor.hasTrait(traitToGive)) 
					{
						actor.addTrait(traitToGive);
						anyTraitGiven = true;
					}
				}
			}
			//if (actor.hasTrait("b0002"))
			//{
			//	actor.removeTrait("b0002");
			//}
			return anyTraitGiven; 
		}
		public static bool Randomness2_3(BaseSimObject pTarget, WorldTile pTile)
		{// 第三抽取(抽選型,可調控次數次) 添加攻擊
			Actor actor = pTarget as Actor;	// 將 a 重新命名為 actor，與其他方法保持一致
			if (actor == null || !actor.isAlive())	// 檢查 actor 是否有效且存活
			{
				return false;
			}
			// 定義特質清單和其對應的機率
			// 請確保這些特質 ID 是在 WorldBox 中真實存在的
			string[] possibleTraits = {//包含銘謝惠顧在內總計14個特質
			"b0000",				//01 	34
			"add_burning",			//02	9
			"add_slowdown",			//03	9
			"add_frozen",			//04	9
			"add_poisonous",		//05	9
			"add_afc",				//06	5
			"add_drowning",			//07	5
			"add_confused",			//08	5
			"add_silenced",			//09	5
			"add_stunned",			//10	5
			"add_unknown",			//11	1
			"add_cursed",			//12	1
			"add_death",			//13	1
			"cb_slam",				//14	3
			"cb_holdfast",			//15	3
			"cb_bulletrain",		//16	3
			"cb_experience",		//17	3
			
			};
			float[] probabilities = {
			30f, 	//b0000
			8f, 	//add_burning
			8f, 	//add_slowdown
			8f, 	//add_frozen
			8f, 	//add_poisonous
			5f, 	//add_afc
			7f, 	//add_drowning
			7f, 	//add_confused
			2f, 	//add_silenced
			2f, 	//add_stunned
			1f, 	//add_unknown
			1f, 	//add_cursed
			1f, 	//add_death
			3f, 	//cb_slam
			3f, 	//cb_holdfast
			3f, 	//cb_bulletrain
			3f, 	//cb_experience
			}; 
			if (probabilities.Sum() != 100f || probabilities.Length != possibleTraits.Length) 
			{
				Debug.LogError("第三抽取的機率總和不等於 100 或特質數量不匹配！請檢查 probabilities 陣列。");
				return false;
			}
			// 設定抽選次數
			int numRolls = 1; 
			bool anyTraitGiven = false; 
			for (int i = 0; i < numRolls; i++)
			{
				string traitToGive = weightedRandom(possibleTraits, probabilities); 
				
				if (!string.IsNullOrEmpty(traitToGive))
				{
					if (!actor.hasTrait(traitToGive)) 
					{
						actor.addTrait(traitToGive);
						anyTraitGiven = true;
					}
				}
			}
			//if (actor.hasTrait("b0002"))
			//{
			//	actor.removeTrait("b0002");
			//}
			return anyTraitGiven; 
		}
		public static bool Randomness2_4(BaseSimObject pTarget, WorldTile pTile)
		{// 第四抽取(抽選型,可調控次數次) 恢復能力
			Actor actor = pTarget as Actor;	// 將 a 重新命名為 actor，與其他方法保持一致
			if (actor == null || !actor.isAlive())	// 檢查 actor 是否有效且存活
			{
				return false;
			}
			// 定義特質清單和其對應的機率
			// 請確保這些特質 ID 是在 WorldBox 中真實存在的
			string[] possibleTraits = {//包含銘謝惠顧在內總計10個特質
			"other001",					//01 銘謝惠顧
			"holyarts_ha",				//02
			"holyarts_heal",			//03
			"holyarts_cure",			//04
			"holyarts_healcure",		//05
			"holyarts_health",			//06
			"holyarts_mana",			//07
			"holyarts_stamina",			//08
			"holyarts_annunciation",	//09
			"holyarts_consecration",	//10
			"holyarts_eucharist",		//11
			"holyarts_bond",	  		//12
			"holyarts_serenity",	  	//13
			"holyarts_rainfall",	  	//14
			"holyarts_justice",	  		//15
			"holyarts_divinelight",	  	//16
			};
			float[] probabilities = {
			20f,	//b0000
			5f, 	//holyarts_ha
			4f, 	//holyarts_heal
			3f, 	//holyarts_cure
			2f, 	//holyarts_healcure
			8.5f, 	//holyarts_health
			8.5f, 	//holyarts_mana
			8.5f, 	//holyarts_stamina
			3.5f, 	//holyarts_annunciation
			4.5f, 	//holyarts_consecration
			4.5f, 	//holyarts_eucharist
			8f, 	//holyarts_bond
			8f, 	//holyarts_serenity
			8f, 	//holyarts_rainfall
			1f, 	//holyarts_justice
			3f, 	//holyarts_divinelight
			}; 
			if (probabilities.Sum() != 100f || probabilities.Length != possibleTraits.Length) 
			{
				Debug.LogError("第四抽取的機率總和不等於 100 或特質數量不匹配！請檢查 probabilities 陣列。");
				return false;
			}
			// 設定抽選次數
			int numRolls = 1; 
			bool anyTraitGiven = false; 
			for (int i = 0; i < numRolls; i++)
			{
				string traitToGive = weightedRandom(possibleTraits, probabilities); 
				
				if (!string.IsNullOrEmpty(traitToGive))
				{
					if (!actor.hasTrait(traitToGive)) 
					{
						actor.addTrait(traitToGive);
						anyTraitGiven = true;
					}
				}
			}
			//if (actor.hasTrait("b0002"))
			//{
			//	actor.removeTrait("b0002");
			//}
			return anyTraitGiven; 
		}
		public static bool Randomness2_5(BaseSimObject pTarget, WorldTile pTile)
		{// 第五抽取(抽選型,可調控次數次) 特殊能力
			Actor actor = pTarget as Actor;	// 將 a 重新命名為 actor，與其他方法保持一致
			if (actor == null || !actor.isAlive())	// 檢查 actor 是否有效且存活
			{
				return false;
			}
			// 定義特質清單和其對應的機率
			// 請確保這些特質 ID 是在 WorldBox 中真實存在的
			string[] possibleTraits = {//包含銘謝惠顧在內總計9個特質
			"other002",					//01 銘謝惠顧 46
			"evillaw_tgc",				//02
			"evillaw_tc",				//03
			"evillaw_disease",			//04
			"evillaw_ea",				//05
			"evillaw_sterilization",	//06
			"evillaw_moneylaw",			//07 大罪
			"evillaw_sleeping",			//08 大罪
			"evillaw_starvation",		//09 大罪
			"evillaw_devour",			//10 大罪
			"evillaw_tantrum",			//11 大罪
			"evillaw_seduction",		//12 大罪
			"evillaw_ew",				//13 大罪
			};
			float[] probabilities = 
			{
			51f, 	//b0000
			8f, 	//evillaw_tgc
			8f, 	//evillaw_tc
			8f, 	//evillaw_disease
			8f, 	//evillaw_ea
			5f, 	//evillaw_sterilization
			2f, 	//evillaw_moneylaw
			2f, 	//evillaw_sleeping
			2f, 	//evillaw_starvation
			2f, 	//evillaw_devour
			1f, 	//evillaw_tantrum
			2f, 	//evillaw_seduction
			1f, 	//evillaw_ew
			}; 
			if (probabilities.Sum() != 100f || probabilities.Length != possibleTraits.Length) 
			{
				Debug.LogError("第五抽取的機率總和不等於 100 或特質數量不匹配！請檢查 probabilities 陣列。");
				return false;
			}
			// 設定抽選次數
			int numRolls = 1; 
			bool anyTraitGiven = false; 
			for (int i = 0; i < numRolls; i++)
			{
				string traitToGive = weightedRandom(possibleTraits, probabilities); 
				
				if (!string.IsNullOrEmpty(traitToGive))
				{
					if (!actor.hasTrait(traitToGive)) 
					{
						actor.addTrait(traitToGive);
						anyTraitGiven = true;
					}
				}
			}
			//if (actor.hasTrait("b0002"))
			//{
			//	actor.removeTrait("b0002");
			//}
			return anyTraitGiven; 
		}
		public static bool Randomness2_6(BaseSimObject pTarget, WorldTile pTile)
		{// 第六抽取(抽選型,可調控次數次) 子彈能力
			Actor actor = pTarget as Actor;	// 將 a 重新命名為 actor，與其他方法保持一致
			if (actor == null || !actor.isAlive())	// 檢查 actor 是否有效且存活
			{
				return false;
			}
			// 定義特質清單和其對應的機率
			// 請確保這些特質 ID 是在 WorldBox 中真實存在的
			string[] possibleTraits = {//包含銘謝惠顧在內總計9個特質
			"b0000",					//01 銘謝惠顧 46
			"projectile01",			//16
			"projectile02",			//17
			"projectile03",			//18
			"projectile04",			//19
			"projectile05",			//20
			"projectile06",			//21
			"projectile07",			//22
			"projectile08",			//23
			"projectile09",			//24
			"projectile10",			//25
			"projectile11",			//26
			"projectile12",			//27
			"projectile13",			//28
			"projectile14",			//29
			"projectile15",			//30
			};
			float[] probabilities = 
			{
			75f, 	//b0000
			3f, 	//01 石頭
			3f, 	//02 雪球
			3f, 	//03 箭矢
			3f, 	//04 火把
			3f, 	//05 火彈
			1f, 	//06 骨彈
			1f, 	//07 紅彈
			1f, 	//08 冰彈
			1f, 	//09 綠彈
			1f, 	//10 酸砲
			1f, 	//11 砲彈
			1f, 	//12 電漿
			1f, 	//13 霰彈
			1f, 	//14 狂彈
			1f, 	//15 火球
			}; 
			if (probabilities.Sum() != 100f || probabilities.Length != possibleTraits.Length) 
			{
				Debug.LogError("第六抽取的機率總和不等於 100 或特質數量不匹配！請檢查 probabilities 陣列。");
				return false;
			}
			// 設定抽選次數
			int numRolls = 1; 
			bool anyTraitGiven = false; 
			for (int i = 0; i < numRolls; i++)
			{
				string traitToGive = weightedRandom(possibleTraits, probabilities); 
				
				if (!string.IsNullOrEmpty(traitToGive))
				{
					if (!actor.hasTrait(traitToGive)) 
					{
						actor.addTrait(traitToGive);
						anyTraitGiven = true;
					}
				}
			}
			//if (actor.hasTrait("b0002"))
			//{
			//	actor.removeTrait("b0002");
			//}
			return anyTraitGiven; 
		}
		public static bool Randomness2_7(BaseSimObject pTarget, WorldTile pTile)
		{// 第七抽取(抽選型,可調控次數次) 建造魔法
			Actor actor = pTarget as Actor;	// 將 a 重新命名為 actor，與其他方法保持一致
			if (actor == null || !actor.isAlive())	// 檢查 actor 是否有效且存活
			{
				return false;
			}
			// 定義特質清單和其對應的機率
			// 請確保這些特質 ID 是在 WorldBox 中真實存在的
			string[] possibleTraits = {//包含銘謝惠顧在內總計9個特質
			"other001",					//01 銘謝惠顧 46
			"monste_nest001",			//02
			"monste_nest002",			//03
			"monste_nest003",			//04
			"monste_nest004",			//05
			"monste_nest005",			//06
			"monste_nest006",			//07
			"monste_nest007",			//08
			"monste_nest008",			//09
			"monste_nest009",			//10
			"monste_nest010",			//11
			"monste_nest011",			//12
			"monste_nest012",			//13
			"monste_nest013",			//14
			"monste_nest014",			//15
			};
			float[] probabilities = 
			{
			90f, 	//b0000
			0.25f, 	//腫瘤
			0.25f, 	//核心
			0.25f, 	//南瓜
			0.25f, 	//生物質
			0.25f, 	//冰塔
			0.25f, 	//火塔
			0.25f, 	//天塔
			1.55f, 	//腐腦
			1.55f, 	//邪電腦
			1.55f, 	//金蛋
			1.55f, 	//豎琴
			1.55f, 	//外星黴菌
			0.25f, 	//酸泉
			0.25f, 	//火山
			}; 
			if (probabilities.Sum() != 100f || probabilities.Length != possibleTraits.Length) 
			{
				Debug.LogError("第六抽取的機率總和不等於 100 或特質數量不匹配！請檢查 probabilities 陣列。");
				return false;
			}
			// 設定抽選次數
			int numRolls = 1; 
			bool anyTraitGiven = false; 
			for (int i = 0; i < numRolls; i++)
			{
				string traitToGive = weightedRandom(possibleTraits, probabilities); 
				
				if (!string.IsNullOrEmpty(traitToGive))
				{
					if (!actor.hasTrait(traitToGive)) 
					{
						actor.addTrait(traitToGive);
						anyTraitGiven = true;
					}
				}
			}
			//if (actor.hasTrait("b0002"))
			//{
			//	actor.removeTrait("b0002");
			//}
			return anyTraitGiven; 
		}
		public static bool AutoremoveTrait(BaseSimObject pTarget, WorldTile pTile = null)
		{// 自我移除
			if (pTarget is Actor actor)
			{
				actor.removeTrait("b0000"); 
			}
			return true;
		}
		public static bool Mutation(BaseSimObject pTarget, WorldTile pTile)
		{// 亞種特質添加 改良式
			if (pTarget == null || pTarget.a == null)
			{
				return false;	// 無效目標，直接返回
			}
			Actor targetActor = pTarget.a;
			// 定義不需要添加非智慧特質的生物ID列表
			HashSet<string> excludedSpeciesIDs = new HashSet<string>()
			{
				"dragon",
				"ghost",
				"lil_pumpkin",
				"bioblob",
				"tumor_monster_unit",
				"tumor_monster_animal",
				"assimilator",
				"mush_unit",
				"mush_animal",
				"fire_elemental",
				"fire_elemental_horse",
				"fire_elemental_blob",
				"fire_elemental_snake",
				"fire_elemental_slug",
				"ant_black",
				"ant_red",
				"ant_blue",
				"ant_green",
				"printer",
				"sand_spider",
				"worm",
				"UFO",
				"living_house",
				"living_plants",
				"god_finger"
			};
			// 定義不需要添加特質的特質ID列表 (例如，用於殭屍或船隻)
			HashSet<string> excludedTraits = new HashSet<string>()
			{
				"boat",  	// 船特質
				"zombie" 	// 殭屍特質
			};
			// 判斷是否應該跳過添加亞種特質
			bool skipAddingSubspeciesTraits = false;
			// 1. 檢查生物ID是否在排除列表中
			if (targetActor.asset != null && excludedSpeciesIDs.Contains(targetActor.asset.id))
			{
				//Debug.Log($"Mutation: 單位 {targetActor.name} (ID: {targetActor.asset.id}) 屬於排除生物列表，跳過添加亞種特質。");
				skipAddingSubspeciesTraits = true;
			}
			// 2. 檢查單位是否擁有排除特質列表中的任何一個特質
			else
			{
				foreach (string traitID in excludedTraits)
				{
					if (targetActor.hasTrait(traitID))
					{
						//Debug.Log($"Mutation: 單位 {targetActor.name} 擁有特質 '{traitID}'，跳過添加亞種特質。");
						skipAddingSubspeciesTraits = true;
						break;	// 找到一個特質就夠了，無需檢查其他
					}
				}
			}
			// 3. 檢查單位是否有 subspecies 物件 (這是最基本的要求)
			// 如果單位根本沒有亞種項目 (subspecies 為 null)，那麼也無法添加亞種特質
			if (targetActor.subspecies == null)
			{
				//Debug.Log($"Mutation: 單位 {targetActor.name} 沒有亞種項目 (subspecies為null)，跳過添加亞種特質。");
				skipAddingSubspeciesTraits = true;
			}
			// 根據判斷結果執行操作
			if (!skipAddingSubspeciesTraits)
			{
				// 如果不跳過，則添加亞種特質
				// 這些特質是為 'subspecies' 添加的，不是為 Actor 直接添加的
				targetActor.subspecies.addTrait("prefrontal_cortex");
				targetActor.subspecies.addTrait("advanced_hippocampus");
				targetActor.subspecies.addTrait("wernicke_area");
				targetActor.subspecies.addTrait("amygdala");
				//Debug.Log($"Mutation: 成功為 {targetActor.name} 添加了智慧亞種特質。");
			}
			// 無論是否添加了亞種特質，都移除 'Mutation00' 特質
			// 這個特質通常是用來標記單位已經進行過一次突變嘗試
			if (targetActor.hasTrait("mutation"))	// 只有當它存在時才嘗試移除
			{
				targetActor.removeTrait("mutation");
				//Debug.Log($"Mutation: 從 {targetActor.name} 移除了 'Mutation00' 特質。");
			}
			
			return true;	// 函數執行完成
		}
	}
	
}