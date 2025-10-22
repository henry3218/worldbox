/* 
AUTHOR: MASON SCARBRO
VERSION: 1.0.0
*/
using System.Threading.Tasks;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NCMS;
using NCMS.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ReflectionUtility;
using HarmonyLib;

namespace ZeN_01
{
	
	class NewProjectilesActions : MonoBehaviour
	{
		public static bool Anti_OtherRaces(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 傲慢子彈特效 異族殺手
			// 1. 安全檢查
			if (pSelf?.a == null || pTarget?.a == null || !pSelf.a.isAlive() || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			Actor targetActor = pTarget.a;
			float totalBonusMultiplier = 0f;
			float SetValue = 0.50f;
			bool hasTriggered = false;
			
			// 2. 檢查排除特質
			if (targetActor.hasTrait("holyarts_justice"))
			{
				return false;
			}
			// 3. 檢查物種和亞種差異
			if (targetActor.asset.id != selfActor.asset.id)
			{//物種不同
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (targetActor.subspecies != selfActor.subspecies)
			{//亞種不同
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			// 4. 如果條件滿足，則計算並造成傷害
			if (hasTriggered)
			{
				float selfDamage = selfActor.stats["damage"];
				float finalDamage = selfDamage * totalBonusMultiplier;
				// 使用正確的 SpecialAttackDamage 函式，傳入攻擊者
				SpecialAttackDamage(pSelf, pTarget, finalDamage, pTile);
				return false;
			}
			return false;
		}
		public static bool Anti_Shield(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 傲慢子彈特效 清除防護照
			// 1. 基本安全檢查
			if (pSelf == null || pTarget == null)
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			Actor targetActor = pTarget.a;
			if (selfActor == null || targetActor == null || !selfActor.isAlive() || !targetActor.isAlive())
			{
				return false;
			}
			// 2. 檢查觸發條件
			// 自身必須持有 "wrath_demon_king" 狀態
			//bool selfHasWrathStatus = selfActor.hasStatus("arrogant_demon_king");
			// 目標必須持有 "invincible" 或 "shield" 狀態
			bool targetHasStatus1 = targetActor.hasStatus("shield");
			// 只有當兩個條件都滿足時才執行後續邏輯
			if (/*selfHasWrathStatus &&*/ targetHasStatus1)
			{
				targetActor.finishStatusEffect("shield");
				return false; // 效果成功發動
			}
			else
			{
				return false;
			}
		}
		public static bool Blade_Black_WhiteTerraform(WorldTile pTile)
		{// 傲慢子彈特效 武器子彈 地形改造效果
			if (pTile == null)
			{
				return false;
			}
			EffectsLibrary.spawn("fx_impact", pTile, "Blade_Black_White", null, 0f, -1f, -1f, null);
			return true;
		}
		public static bool Blade_WhiteTerraform(WorldTile pTile)
		{// 傲慢子彈特效 效果子彈 地形改造效果
			if (pTile == null)
			{
				return false;
			}
			EffectsLibrary.spawn("fx_impact", pTile, "Blade_White", null, 0f, -1f, -1f, null);
			return true;
		}
		public static bool Blade_BlackTerraform(WorldTile pTile)
		{// 傲慢子彈特效 效果子彈 地形改造效果
			if (pTile == null)
			{
				return false;
			}
			EffectsLibrary.spawn("fx_impact", pTile, "Blade_Black", null, 0f, -1f, -1f, null);
			return true;
		}
		public static bool Blade_White01Terraform(WorldTile pTile)
		{// 傲慢子彈特效 效果子彈 地形改造效果
			if (pTile == null)
			{
				return false;
			}
			EffectsLibrary.spawn("fx_impact", pTile, "Blade_White01", null, 0f, -1f, -1f, null);
			return true;
		}
		public static bool Blade_Black01Terraform(WorldTile pTile)
		{// 傲慢子彈特效 效果子彈 地形改造效果
			if (pTile == null)
			{
				return false;
			}
			EffectsLibrary.spawn("fx_impact", pTile, "Blade_Black01", null, 0f, -1f, -1f, null);
			return true;
		}
		public static bool Blade_White02Terraform(WorldTile pTile)
		{// 傲慢子彈特效 效果子彈 地形改造效果
			if (pTile == null)
			{
				return false;
			}
			EffectsLibrary.spawn("fx_impact", pTile, "Blade_White02", null, 0f, -1f, -1f, null);
			return true;
		}
		public static bool Blade_Black02Terraform(WorldTile pTile)
		{// 傲慢子彈特效 效果子彈 地形改造效果
			if (pTile == null)
			{
				return false;
			}
			EffectsLibrary.spawn("fx_impact", pTile, "Blade_Black02", null, 0f, -1f, -1f, null);
			return true;
		}
		public static bool Blade_White03Terraform(WorldTile pTile)
		{// 傲慢子彈特效 效果子彈 地形改造效果
			if (pTile == null)
			{
				return false;
			}
			EffectsLibrary.spawn("fx_impact", pTile, "Blade_White03", null, 0f, -1f, -1f, null);
			return true;
		}
		public static bool Blade_Black03Terraform(WorldTile pTile)
		{// 傲慢子彈特效 效果子彈 地形改造效果
			if (pTile == null)
			{
				return false;
			}
			EffectsLibrary.spawn("fx_impact", pTile, "Blade_Black03", null, 0f, -1f, -1f, null);
			return true;
		}

		public static bool Anti_Poverty(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 強慾子彈特效 貧者殺手
			// 安全檢查：確保施法者和目標都存在且有效
			if (pSelf?.a == null || pTarget?.a == null || !pSelf.a.isAlive() || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			Actor targetActor = pTarget.a;
			float totalBonusMultiplier = 0f;
			float SetValue = 0.50f;
			bool hasTriggered = false;
			// 檢查目標種族是否與施法者不同
			if (targetActor.hasTrait("holyarts_justice"))
			{// 如果目標持有 特質 則返回
				return false;
			}
			if (targetActor.data.money < selfActor.data.money || targetActor.data.loot < selfActor.data.loot)
			{
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			// 4. 如果條件滿足，則計算並造成傷害
			if (hasTriggered)
			{
				float selfDamage = selfActor.stats["damage"];
				float finalDamage = selfDamage * totalBonusMultiplier;
				// 使用正確的 SpecialAttackDamage 函式，傳入攻擊者
				SpecialAttackDamage(pSelf, pTarget, finalDamage, pTile);
				return true;
			}
			return false;
		}

		public static bool Anti_Angry(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 憤怒子彈特效 憤怒殺手
			// 安全檢查：確保施法者和目標都存在且有效
			if (pSelf?.a == null || pTarget?.a == null || !pSelf.a.isAlive() || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			Actor targetActor = pTarget.a;
			float totalBonusMultiplier = 0f;
			float SetValue = 1.50f;
			bool hasTriggered = false;
			// 檢查目標種族是否與施法者不同
			if (targetActor.hasTrait("holyarts_justice"))
			{// 如果目標持有 特質 則返回
				return false;
			}
			if (targetActor.hasStatus("angry"))
			{
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			// 4. 如果條件滿足，則計算並造成傷害
			if (hasTriggered)
			{
				float selfDamage = selfActor.stats["damage"];
				float finalDamage = selfDamage * totalBonusMultiplier;
				// 使用正確的 SpecialAttackDamage 函式，傳入攻擊者
				SpecialAttackDamage(pSelf, pTarget, finalDamage, pTile);
				return true;
			}
			return false;
		}
		public static bool Anti_Invincible(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 憤怒子彈特效 無敵粉碎
			// 1. 基本安全檢查
			if (pSelf == null || pTarget == null)
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			Actor targetActor = pTarget.a;
			if (selfActor == null || targetActor == null || !selfActor.isAlive() || !targetActor.isAlive())
			{
				return false;
			}
			// 2. 檢查觸發條件
			// 自身必須持有 "wrath_demon_king" 狀態
			bool selfHasWrathStatus = selfActor.hasStatus("wrath_demon_king");
			bool targetHasStatus1 = targetActor.hasStatus("invincible");
			bool targetHasStatus2 = targetActor.hasStatus("shield");
			// 只有當兩個條件都滿足時才執行後續邏輯
			selfActor.addStatusEffect("crosshair", 1f);
			if (selfHasWrathStatus &&(targetHasStatus1 || targetHasStatus2))
			{
				targetActor.finishStatusEffect("invincible");
				targetActor.finishStatusEffect("shield");
				return true; // 效果成功發動
			}
			else
			{
				return false;
			}
		}
		public static bool High_FireballTerraform(WorldTile pTile)
		{// 憤怒子彈特效 效果子彈 地形改造效果
			if (pTile == null)
			{
				return false;
			}
			EffectsLibrary.spawn("fx_impact", pTile, "High_Fireball", null, 0f, -1f, -1f, null);
			return true;
		}
		public static bool High_RedOrblTerraform(WorldTile pTile)
		{// 憤怒子彈特效 效果子彈 地形改造效果
			if (pTile == null)
			{
				return false;
			}
			EffectsLibrary.spawn("fx_impact", pTile, "High_RedOrbl", null, 0f, -1f, -1f, null);
			return true;
		}
		public static bool NuclearFusionTerraform(WorldTile pTile)
		{// 憤怒子彈特效 效果子彈 地形改造效果
			if (pTile == null)
			{
				return false;
			}
			EffectsLibrary.spawn("fx_impact", pTile, "NuclearFusion_to", null, 0f, -1f, -1f, null);
			return true;
		}
		public static bool High_Fireball_2Terraform(WorldTile pTile)
		{// 憤怒子彈特效 效果子彈 地形改造效果
			if (pTile == null)
			{
				return false;
			}
			EffectsLibrary.spawn("fx_impact", pTile, "High_Fireball_2", null, 0f, -1f, -1f, null);
			return true;
		}

		public static bool Anti_Hungry(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 暴食子彈特效 飢餓殺手
			// 安全檢查：確保施法者和目標都存在且有效
			if (pSelf?.a == null || pTarget?.a == null || !pSelf.a.isAlive() || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			Actor targetActor = pTarget.a;
			float totalBonusMultiplier = 0f;
			float SetValue = 0.25f;
			bool hasTriggered = false;
			// 檢查目標種族是否與施法者不同
			if (targetActor.hasTrait("holyarts_justice"))
			{// 如果目標持有 特質 則返回
				return false;
			}
			if (targetActor.data.nutrition < selfActor.data.nutrition || !targetActor.subspecies.hasTrait("stomach"))
			{
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			// 4. 如果條件滿足，則計算並造成傷害
			if (hasTriggered)
			{
				float selfDamage = selfActor.stats["damage"];
				float finalDamage = selfDamage * totalBonusMultiplier;
				// 使用正確的 SpecialAttackDamage 函式，傳入攻擊者
				SpecialAttackDamage(pSelf, pTarget, finalDamage, pTile);
				return true;
			}
			return false;
		}
		public static bool Tableware1Terraform(WorldTile pTile)
		{// 暴食子彈特效 效果子彈 地形改造效果
			if (pTile == null)
			{
				return false;
			}
			EffectsLibrary.spawn("fx_impact", pTile, "Tableware1", null, 0f, -1f, -1f, null);
			return true;
		}
		public static bool Tableware2Terraform(WorldTile pTile)
		{// 暴食子彈特效 效果子彈 地形改造效果
			if (pTile == null)
			{
				return false;
			}
			EffectsLibrary.spawn("fx_impact", pTile, "Tableware2", null, 0f, -1f, -1f, null);
			return true;
		}
		public static bool High_AcidBallTerraform(WorldTile pTile)
		{// 暴食子彈特效 效果子彈 地形改造效果
			if (pTile == null)
			{
				return false;
			}
			EffectsLibrary.spawn("fx_impact", pTile, "High_AcidBall", null, 0f, -1f, -1f, null);
			return true;
		}


		public static bool Anti_NoSleeping(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 怠惰子彈特效 不眠殺手
			// 安全檢查：確保施法者和目標都存在且有效
			if (pSelf?.a == null || pTarget?.a == null || !pSelf.a.isAlive() || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			Actor targetActor = pTarget.a;
			float totalBonusMultiplier = 0f;
			float SetValue = 1.50f;
			bool hasTriggered = false;
			// 檢查目標種族是否與施法者不同
			if (targetActor.hasTrait("holyarts_justice"))
			{// 如果目標持有 特質 則返回
				return false;
			}
			if (!targetActor.hasStatus("sleeping"))
			{
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			// 4. 如果條件滿足，則計算並造成傷害
			if (hasTriggered)
			{
				float selfDamage = selfActor.stats["damage"];
				float finalDamage = selfDamage * totalBonusMultiplier;
				// 使用正確的 SpecialAttackDamage 函式，傳入攻擊者
				SpecialAttackDamage(pSelf, pTarget, finalDamage, pTile);
				return true;
			}
			return false;
		}
		public static bool AvalancheTerraform(WorldTile pTile)
		{// 怠惰子彈特效 效果子彈 地形改造效果
			if (pTile == null)
			{
				return false;
			}
			EffectsLibrary.spawn("fx_impact", pTile, "Avalanche_to", null, 0f, -1f, -1f, null);
			return true;
		}
		public static bool IcePickTerraform(WorldTile pTile)
		{// 怠惰子彈特效 效果子彈 地形改造效果
			if (pTile == null)
			{
				return false;
			}
			EffectsLibrary.spawn("fx_impact", pTile, "IcePick_to", null, 0f, -1f, -1f, null);
			return true;
		}
		public static bool SnowflakeTerraform(WorldTile pTile)
		{// 怠惰子彈特效 效果子彈 地形改造效果
			if (pTile == null)
			{
				return false;
			}
			EffectsLibrary.spawn("fx_impact", pTile, "Snowflake_to", null, 0f, -1f, -1f, null);
			return true;
		}


		public static bool SpecialAttackDamage(BaseSimObject pAttacker, BaseSimObject pTarget, float pFinalDamage, WorldTile pTile = null)
		{// 特攻傷害執行
			if (pTarget == null || !pTarget.isAlive())
			{
				return false;
			}
			// 現在 pAttacker 變數被正確地傳入 getHit 函式
			pTarget.getHit(pFinalDamage, true, AttackType.None, pAttacker, false, false, false);
			return true;
		}
	}
}