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
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using UnityEngine;
using ReflectionUtility;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.CanvasScaler;
using ai;
using ai.behaviours;
using db;

namespace ZeN_01
{
	public class Items01Actions
	{
		private static Dictionary<Actor, Actor> listOfTamedBeasts = new Dictionary<Actor, Actor>();
		private static Dictionary<ActorData, Actor> listOfTamedBeastsData = new Dictionary<ActorData, Actor>();
		// detectionRange	檢測範圍
		// requiredEnemies	目標人數
		// maxRange			最大範圍
		// minRange			最小範圍

		public static bool Defense(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 全魔王通用 特防啟動狀態
			// 1. 基本安全检查：确保 pSelf 及其 Actor 组件存在且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) 
				return false;
			Actor selfActor = pSelf.a;
			Actor targetActor = pTarget?.a;
			// 2. 上半部：特质持有者自身添加条件 (保留您的状态检查)
			bool hasDemonKingStatus = false;
			foreach (string statusID in SevenDemonKingStatus1)
			{
				if (selfActor.hasStatus(statusID))
				{
					hasDemonKingStatus = true;
					break; // 找到任一狀態即跳出迴圈
				}
			}
			// 如果單位沒有任何魔王狀態，則直接返回 false，阻止特防發動
			if (!hasDemonKingStatus)
			{
				return false;
			}
			// 确保自身没有 "defense"
			if (!selfActor.hasStatus("defense_on"))
			{
				if (Randy.randomChance(0.10f)) // 0.5f = 50% 機率
				{
					selfActor.addStatusEffect("defense_on", 9f);// 冷却状态
				}
			}
			return false; // 特殊效果通常返回 false，表示不阻止其他效果链
		}
		
			#region 傲慢之劍
		public static bool Anti_Shield(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 魔劍 破防
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
			bool selfHasWrathStatus = selfActor.hasStatus("arrogant_demon_king");
			// 目標必須持有 "invincible" 或 "shield" 狀態
			bool targetHasStatus1 = targetActor.hasStatus("shield");
			// 只有當兩個條件都滿足時才執行後續邏輯
			if (selfHasWrathStatus && targetHasStatus1)
			{
				targetActor.finishStatusEffect("shield");
				return true; // 效果成功發動
			}
			else
			{
				return false;
			}
		}
		public static bool Anti_OtherRaces(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 魔劍 異族殺手
			// 安全檢查...
			if (pSelf?.a == null || pTarget?.a == null || !pSelf.a.isAlive() || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			Actor targetActor = pTarget.a;
			float totalBonusMultiplier = 0f;
			float SetValue = 2.50f;
			bool hasTriggered = false;
			if (targetActor.hasStatus("brave"))
			{// 如果目標持有 狀態 則返回
				SetValue = 1.25f;
			}
			if (targetActor.asset.id != selfActor.asset.id)
			{//物種不同的話
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (targetActor.subspecies != selfActor.subspecies)
			{//亞種不同的話
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			// 如果至少有一個屬性觸發了效果，就計算並造成傷害
			if (hasTriggered)
			{
				float selfDamage = selfActor.stats["damage"];
				float finalDamage = selfDamage * totalBonusMultiplier;
				SpecialAttackDamage(pSelf, pTarget, finalDamage, pTile);
				return true;
			}
			return false;
		}
		public static bool Anti_OtherRaces_Defense(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 魔劍 異族特防
			// 1. 安全檢查：確保施法者 (防禦者) 和目標 (攻擊者) 都存在且有效
			if (pSelf?.a == null || pTarget?.a == null || !pSelf.a.isAlive() || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a; // 防禦者
			Actor targetActor = pTarget.a; // 攻擊者
			if (!selfActor.hasStatus("arrogant_demon_king") || !selfActor.hasStatus("defense_on"))
			{
				return false;
			}
			// 取得攻擊者對防禦者造成的基礎傷害值
			float damageDealt = targetActor.stats["damage"];
			float totalDamageToHeal = 0f; // 用一個變數來累計總減免傷害
			// 檢查第一個條件：種族是否不同
			if (targetActor.asset.id != selfActor.asset.id)
			{				
				totalDamageToHeal += damageDealt * 0.45f;
			}
			// 檢查第二個條件：亞種是否不同
			if (targetActor.subspecies != selfActor.subspecies)
			{				
				totalDamageToHeal += damageDealt * 0.45f;
			}
			// 如果任何一個條件滿足（總減免傷害大於0），才執行生命值恢復
			if (totalDamageToHeal > 0)
			{
				// 直接增加生命值，以抵銷受到的傷害
				selfActor.data.health += (int)totalDamageToHeal;
				// 確保生命值不會超過最大值
				if (selfActor.data.health > selfActor.getMaxHealth())
				{
					selfActor.data.health = selfActor.getMaxHealth();
				}
				return true; // 效果成功發動
			}
			return false; // 沒有條件滿足，效果未發動
		}
		public static bool EvilSwordSlashX1(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 魔劍 轉接 (EvilSwordSlash 01 02)
			// 1. 基礎安全檢查
			if (pSelf == null || pTarget == null)
			{
				return false;
			}
			
			// 2. 隨機分配攻擊
			// 隨機選擇要觸發的攻擊效果
			float randomValue = UnityEngine.Random.value; // 獲取一個 0 到 1 的隨機值
			if (randomValue <= 0.50f)
			{
				// 50% 機率觸發 EvilSwordSlash01
				return EvilSwordSlash01(pSelf, pTarget, pTile);
			}
			else
			{
				// 另外 50% 機率觸發 EvilSwordSlash02
				return EvilSwordSlash02(pSelf, pTarget, pTile);
			}
		}
		public static bool EvilSwordSlash01(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 魔劍 攻擊發動 扇狀霰射 item_cdt00 氣刃斬
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			string requiredStatus = "arrogant_demon_king";
			
			// 檢查自身狀態和冷卻
			string attackCooldownStatus = "item_cdt00";
			float attackCooldownDuration = 0.01f;
			if (selfActor.hasStatus(attackCooldownStatus) || !selfActor.hasStatus(requiredStatus))
			{
				return false;
			}
			
			// === 核心邏輯：根據周圍敵人的數量決定是否發動 ===
			float detectionRange = 30f; // 設定一個檢測敵人的範圍
			int requiredEnemies = 3;	// 設定發動所需的最少敵人數
			int enemyCount = 0;
			BaseSimObject mainTarget = null; // 儲存最接近的敵方目標
			float closestDist = float.MaxValue;

			// A. 遍歷所有單位，尋找符合條件的敵人
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
					
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist <= detectionRange)
				{
					enemyCount++;
					// 同時尋找最接近的目標，作為攻擊的中心點
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = other;
					}
				}
			}

			// B. 遍歷所有建築物，尋找符合條件的敵人
			foreach (var building in World.world.buildings)
			{
				if (building == null || !building.isAlive())
					continue;
				if (building.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(building.kingdom))
					continue;
					
				float dist = Vector2.Distance(selfActor.current_position, building.current_position);
				if (dist <= detectionRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = building;
					}
				}
			}
			
			// 只有當敵人數達標且找到主目標時，才發動攻擊
			if (enemyCount >= requiredEnemies && mainTarget != null)
			{
				// === 滿足條件，施加冷卻並發射投射物 ===
				selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
				mainTarget.finishStatusEffect("invincible");
				mainTarget.finishStatusEffect("shield");

				// --- 特攻效果 ---

				// 使用最接近的目標作為扇形攻擊的中心點
				UnityEngine.Vector3 selfPosition = selfActor.current_position;
				UnityEngine.Vector3 targetPosition = mainTarget.current_position;
				
				// 調整發射點
				UnityEngine.Vector3 basePoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, targetPosition.x, targetPosition.y, selfActor.stats["size"]);
				UnityEngine.Vector3 targetPoint = targetPosition; // 直接使用目標位置作為扇形中心
				
				// --- 為不同子彈類型定義獨立的設定 ---
				int Projectiles_001 = 12;
				float spreadAngle_001 = 10f;
				int Projectiles_002 = 12;
				float spreadAngle_002 = 10f;
				
				// --- 發射 001 ---
				for (int i = 0; i < Projectiles_001; i++)
				{
					UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, spreadAngle_001 * (i - (Projectiles_001 - 1f) / 2f));
					UnityEngine.Vector3 spreadTarget = rotation * (targetPoint - basePoint) + basePoint;
					World.world.projectiles.spawn(pInitiator: selfActor, pTargetObject: mainTarget, pAssetID: "BladeWhite", pLaunchPosition: basePoint, pTargetPosition: spreadTarget, pTargetZ: 0.0f);
				}
				// --- 發射 002 ---
				for (int i = 0; i < Projectiles_002; i++)
				{
					UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, spreadAngle_002 * (i - (Projectiles_002 - 1f) / 2f));
					UnityEngine.Vector3 spreadTarget = rotation * (targetPoint - basePoint) + basePoint;
					World.world.projectiles.spawn(pInitiator: selfActor, pTargetObject: mainTarget, pAssetID: "BladeBlack", pLaunchPosition: basePoint, pTargetPosition: spreadTarget, pTargetZ: 0.0f);
				}
				return true;
			}
			return false;
		}
		public static bool EvilSwordSlash02(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 魔劍 攻擊發動 環狀集火 item_cdt01 飛劍
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			string requiredStatus = "arrogant_demon_king";
			
			if (!selfActor.hasStatus(requiredStatus))
			{
				return false;
			}
			
			// === 冷卻狀態檢查邏輯 ===
			string attackCooldownStatus = "item_cdt01";
			float attackCooldownDuration = 0.01f;
			if (selfActor.hasStatus(attackCooldownStatus))
			{
				return false;
			}
			
			// === 核心邏輯：根據周圍敵人的數量決定是否發動 ===
			float detectionRange = 100f; // 設定一個檢測敵人的範圍
			int requiredEnemies = 1;	// 設定發動所需的最少敵人數
			int enemyCount = 0;
			BaseSimObject mainTarget = null;
			float closestDist = float.MaxValue;
			
			// 遍歷所有單位和建築物來計數和尋找主目標
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist <= detectionRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = other;
					}
				}
			}
			foreach (var building in World.world.buildings)
			{
				if (building == null || !building.isAlive())
					continue;
				if (building.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(building.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, building.current_position);
				if (dist <= detectionRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = building;
					}
				}
			}
			
			// 只有當敵人數達標且找到主目標時，才發動攻擊
			if (enemyCount >= requiredEnemies && mainTarget != null)
			{
				// === 滿足條件，施加冷卻並發射投射物 ===
				selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
				//selfActor.addStatusEffect("invincible", 0.05f);
				mainTarget.finishStatusEffect("invincible");
				mainTarget.finishStatusEffect("shield");



				UnityEngine.Vector3 selfPosition = selfActor.current_position;
				UnityEngine.Vector3 targetPosition = mainTarget.current_position;
				
				UnityEngine.Vector3 baseLaunchPoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, targetPosition.x, targetPosition.y, selfActor.stats["size"]);
				baseLaunchPoint.y += 0.5f;

				// 第一層環狀攻擊
				int numberOfProjectiles_01 = 12;
				float spreadDistance_01 = 10.0f;
				float spreadAngle_01 = 360.0f;
				float totalSpread_01 = spreadAngle_01 / numberOfProjectiles_01;
				for (int i = 0; i < numberOfProjectiles_01; i++)
				{
					float angle = totalSpread_01 * i - spreadAngle_01 / 2f;
					UnityEngine.Vector3 launchPosition_01 = baseLaunchPoint + Quaternion.Euler(0f, 0f, angle) * new UnityEngine.Vector3(spreadDistance_01, 0, 0);
					World.world.projectiles.spawn(
						pInitiator: selfActor,
						pTargetObject: mainTarget,
						pAssetID: "BladeWhite01",
						pLaunchPosition: launchPosition_01,
						pTargetPosition: targetPosition,
						pTargetZ: 0.0f
					);
				}

				// 第二層環狀攻擊
				int numberOfProjectiles_00 = 12;
				float spreadDistance_00 = 15.0f;
				float spreadAngle_00 = 360.0f;
				float totalSpread_00 = spreadAngle_00 / numberOfProjectiles_00;
				for (int i = 0; i < numberOfProjectiles_00; i++)
				{
					float angle = totalSpread_00 * i - spreadAngle_00 / 2f;
					UnityEngine.Vector3 launchPosition_00 = baseLaunchPoint + Quaternion.Euler(0f, 0f, angle) * new UnityEngine.Vector3(spreadDistance_00, 0, 0);
					World.world.projectiles.spawn(
						pInitiator: selfActor,
						pTargetObject: mainTarget,
						pAssetID: "BladeBlack01",
						pLaunchPosition: launchPosition_00,
						pTargetPosition: targetPosition,
						pTargetZ: 0.0f
					);
				}
				return true;
			}
			return false;
		}
		public static bool EvilSwordSlash03(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 魔劍 攻擊發動 收縮包圍 item_cdt02 第一終結 The End of the World
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			string requiredStatus = "arrogant_demon_king";

			if (!selfActor.hasStatus(requiredStatus))
			{
				return false;
			}

			// 檢查機率，如果失敗則不發動
			if (!Randy.randomChance(0.001f))
			{
				return false;
			}
			
			// === 冷卻狀態檢查邏輯 ===
			string attackCooldownStatus = "item_cdt02";
			float attackCooldownDuration = 1800.1f;
			if (selfActor.hasStatus(attackCooldownStatus))
			{
				return false;
			}

			// === 核心邏輯：根據周圍敵人的數量決定是否發動 ===
			float detectionRange = 900f; // 設定一個檢測敵人的範圍
			int requiredEnemies = 3;	// 設定發動所需的最少敵人數
			int enemyCount = 0;
			BaseSimObject mainTarget = null;
			float closestDist = float.MaxValue;
			
			// 遍歷所有單位和建築物來計數和尋找主目標
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist <= detectionRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = other;
					}
				}
			}
			foreach (var building in World.world.buildings)
			{
				if (building == null || !building.isAlive())
					continue;
				if (building.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(building.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, building.current_position);
				if (dist <= detectionRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = building;
					}
				}
			}
			
			// 只有當敵人數達標且找到主目標時，才發動攻擊
			if (enemyCount >= requiredEnemies && mainTarget != null)
			{
				// === 滿足條件，施加冷卻並發射投射物 ===
				selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
				//selfActor.addStatusEffect("invincible", 5.00f);
				mainTarget.finishStatusEffect("invincible");
				mainTarget.finishStatusEffect("shield");



				UnityEngine.Vector3 targetPosition = mainTarget.current_position;
				
				// 多層次包圍攻擊
				int numberOfProjectiles_602 = 360;
				float circleRadius_602 = 50.0f;
				float totalAngle_602 = 360.0f;
				float totalAnglePerProjectile_602 = totalAngle_602 / numberOfProjectiles_602;
				for (int i = 0; i < numberOfProjectiles_602; i++)
				{
					float launchAngle = totalAnglePerProjectile_602 * i;
					float launchAngleRad = launchAngle * Mathf.Deg2Rad;
					UnityEngine.Vector3 launchPosition_602 = new UnityEngine.Vector3(
						targetPosition.x + circleRadius_602 * Mathf.Cos(launchAngleRad),
						targetPosition.y + circleRadius_602 * Mathf.Sin(launchAngleRad),
						targetPosition.z
					);
					World.world.projectiles.spawn(pInitiator: selfActor, pTargetObject: mainTarget, pAssetID: "BladeWhite02", pLaunchPosition: launchPosition_602, pTargetPosition: targetPosition, pTargetZ: 0.0f);
				}
				
				int numberOfProjectiles_601 = 360;
				float circleRadius_601 = 50.5f;
				float totalAngle_601 = 360.0f;
				float totalAnglePerProjectile_601 = totalAngle_601 / numberOfProjectiles_601;
				for (int i = 0; i < numberOfProjectiles_601; i++)
				{
					float launchAngle = totalAnglePerProjectile_601 * i;
					float launchAngleRad = launchAngle * Mathf.Deg2Rad;
					UnityEngine.Vector3 launchPosition_601 = new UnityEngine.Vector3(
						targetPosition.x + circleRadius_601 * Mathf.Cos(launchAngleRad),
						targetPosition.y + circleRadius_601 * Mathf.Sin(launchAngleRad),
						targetPosition.z
					);
					World.world.projectiles.spawn(pInitiator: selfActor, pTargetObject: mainTarget, pAssetID: "BladeBlack02", pLaunchPosition: launchPosition_601, pTargetPosition: targetPosition, pTargetZ: 0.0f);
				}

				int numberOfProjectiles_502 = 360;
				float circleRadius_502 = 51.0f;
				float totalAngle_502 = 360.0f;
				float totalAnglePerProjectile_502 = totalAngle_502 / numberOfProjectiles_502;
				for (int i = 0; i < numberOfProjectiles_502; i++)
				{
					float launchAngle = totalAnglePerProjectile_502 * i;
					float launchAngleRad = launchAngle * Mathf.Deg2Rad;
					UnityEngine.Vector3 launchPosition_502 = new UnityEngine.Vector3(
						targetPosition.x + circleRadius_502 * Mathf.Cos(launchAngleRad),
						targetPosition.y + circleRadius_502 * Mathf.Sin(launchAngleRad),
						targetPosition.z
					);
					World.world.projectiles.spawn(pInitiator: selfActor, pTargetObject: mainTarget, pAssetID: "BladeWhite02", pLaunchPosition: launchPosition_502, pTargetPosition: targetPosition, pTargetZ: 0.0f);
				}
				
				int numberOfProjectiles_501 = 360;
				float circleRadius_501 = 51.5f;
				float totalAngle_501 = 360.0f;
				float totalAnglePerProjectile_501 = totalAngle_501 / numberOfProjectiles_501;
				for (int i = 0; i < numberOfProjectiles_501; i++)
				{
					float launchAngle = totalAnglePerProjectile_501 * i;
					float launchAngleRad = launchAngle * Mathf.Deg2Rad;
					UnityEngine.Vector3 launchPosition_501 = new UnityEngine.Vector3(
						targetPosition.x + circleRadius_501 * Mathf.Cos(launchAngleRad),
						targetPosition.y + circleRadius_501 * Mathf.Sin(launchAngleRad),
						targetPosition.z
					);
					World.world.projectiles.spawn(pInitiator: selfActor, pTargetObject: mainTarget, pAssetID: "BladeBlack02", pLaunchPosition: launchPosition_501, pTargetPosition: targetPosition, pTargetZ: 0.0f);
				}

				int numberOfProjectiles_402 = 360;
				float circleRadius_402 = 52.0f;
				float totalAngle_402 = 360.0f;
				float totalAnglePerProjectile_402 = totalAngle_402 / numberOfProjectiles_402;
				for (int i = 0; i < numberOfProjectiles_402; i++)
				{
					float launchAngle = totalAnglePerProjectile_402 * i;
					float launchAngleRad = launchAngle * Mathf.Deg2Rad;
					UnityEngine.Vector3 launchPosition_402 = new UnityEngine.Vector3(
						targetPosition.x + circleRadius_402 * Mathf.Cos(launchAngleRad),
						targetPosition.y + circleRadius_402 * Mathf.Sin(launchAngleRad),
						targetPosition.z
					);
					World.world.projectiles.spawn(pInitiator: selfActor, pTargetObject: mainTarget, pAssetID: "BladeWhite02", pLaunchPosition: launchPosition_402, pTargetPosition: targetPosition, pTargetZ: 0.0f);
				}
				
				int numberOfProjectiles_401 = 360;
				float circleRadius_401 = 52.5f;
				float totalAngle_401 = 360.0f;
				float totalAnglePerProjectile_401 = totalAngle_401 / numberOfProjectiles_401;
				for (int i = 0; i < numberOfProjectiles_401; i++)
				{
					float launchAngle = totalAnglePerProjectile_401 * i;
					float launchAngleRad = launchAngle * Mathf.Deg2Rad;
					UnityEngine.Vector3 launchPosition_401 = new UnityEngine.Vector3(
						targetPosition.x + circleRadius_401 * Mathf.Cos(launchAngleRad),
						targetPosition.y + circleRadius_401 * Mathf.Sin(launchAngleRad),
						targetPosition.z
					);
					World.world.projectiles.spawn(pInitiator: selfActor, pTargetObject: mainTarget, pAssetID: "BladeBlack02", pLaunchPosition: launchPosition_401, pTargetPosition: targetPosition, pTargetZ: 0.0f);
				}

				int numberOfProjectiles_302 = 360;
				float circleRadius_302 = 53.0f;
				float totalAngle_302 = 360.0f;
				float totalAnglePerProjectile_302 = totalAngle_302 / numberOfProjectiles_302;
				for (int i = 0; i < numberOfProjectiles_302; i++)
				{
					float launchAngle = totalAnglePerProjectile_302 * i;
					float launchAngleRad = launchAngle * Mathf.Deg2Rad;
					UnityEngine.Vector3 launchPosition_302 = new UnityEngine.Vector3(
						targetPosition.x + circleRadius_302 * Mathf.Cos(launchAngleRad),
						targetPosition.y + circleRadius_302 * Mathf.Sin(launchAngleRad),
						targetPosition.z
					);
					World.world.projectiles.spawn(pInitiator: selfActor, pTargetObject: mainTarget, pAssetID: "BladeWhite02", pLaunchPosition: launchPosition_302, pTargetPosition: targetPosition, pTargetZ: 0.0f);
				}
				
				int numberOfProjectiles_301 = 360;
				float circleRadius_301 = 53.5f;
				float totalAngle_301 = 360.0f;
				float totalAnglePerProjectile_301 = totalAngle_301 / numberOfProjectiles_301;
				for (int i = 0; i < numberOfProjectiles_301; i++)
				{
					float launchAngle = totalAnglePerProjectile_301 * i;
					float launchAngleRad = launchAngle * Mathf.Deg2Rad;
					UnityEngine.Vector3 launchPosition_301 = new UnityEngine.Vector3(
						targetPosition.x + circleRadius_301 * Mathf.Cos(launchAngleRad),
						targetPosition.y + circleRadius_301 * Mathf.Sin(launchAngleRad),
						targetPosition.z
					);
					World.world.projectiles.spawn(pInitiator: selfActor, pTargetObject: mainTarget, pAssetID: "BladeBlack02", pLaunchPosition: launchPosition_301, pTargetPosition: targetPosition, pTargetZ: 0.0f);
				}

				int numberOfProjectiles_202 = 360;
				float circleRadius_202 = 54.0f;
				float totalAngle_202 = 360.0f;
				float totalAnglePerProjectile_202 = totalAngle_202 / numberOfProjectiles_202;
				for (int i = 0; i < numberOfProjectiles_202; i++)
				{
					float launchAngle = totalAnglePerProjectile_202 * i;
					float launchAngleRad = launchAngle * Mathf.Deg2Rad;
					UnityEngine.Vector3 launchPosition_202 = new UnityEngine.Vector3(
						targetPosition.x + circleRadius_202 * Mathf.Cos(launchAngleRad),
						targetPosition.y + circleRadius_202 * Mathf.Sin(launchAngleRad),
						targetPosition.z
					);
					World.world.projectiles.spawn(pInitiator: selfActor, pTargetObject: mainTarget, pAssetID: "BladeWhite02", pLaunchPosition: launchPosition_202, pTargetPosition: targetPosition, pTargetZ: 0.0f);
				}

				int numberOfProjectiles_201 = 360;
				float circleRadius_201 = 54.5f;
				float totalAngle_201 = 360.0f;
				float totalAnglePerProjectile_201 = totalAngle_201 / numberOfProjectiles_201;
				for (int i = 0; i < numberOfProjectiles_201; i++)
				{
					float launchAngle = totalAnglePerProjectile_201 * i;
					float launchAngleRad = launchAngle * Mathf.Deg2Rad;
					UnityEngine.Vector3 launchPosition_201 = new UnityEngine.Vector3(
						targetPosition.x + circleRadius_201 * Mathf.Cos(launchAngleRad),
						targetPosition.y + circleRadius_201 * Mathf.Sin(launchAngleRad),
						targetPosition.z
					);
					World.world.projectiles.spawn(pInitiator: selfActor, pTargetObject: mainTarget, pAssetID: "BladeBlack02", pLaunchPosition: launchPosition_201, pTargetPosition: targetPosition, pTargetZ: 0.0f);
				}

				int numberOfProjectiles_102 = 360;
				float circleRadius_102 = 55.0f;
				float totalAngle_102 = 360.0f;
				float totalAnglePerProjectile_102 = totalAngle_102 / numberOfProjectiles_102;
				for (int i = 0; i < numberOfProjectiles_102; i++)
				{
					float launchAngle = totalAnglePerProjectile_102 * i;
					float launchAngleRad = launchAngle * Mathf.Deg2Rad;
					UnityEngine.Vector3 launchPosition_102 = new UnityEngine.Vector3(
						targetPosition.x + circleRadius_102 * Mathf.Cos(launchAngleRad),
						targetPosition.y + circleRadius_102 * Mathf.Sin(launchAngleRad),
						targetPosition.z
					);
					World.world.projectiles.spawn(pInitiator: selfActor, pTargetObject: mainTarget, pAssetID: "BladeWhite02", pLaunchPosition: launchPosition_102, pTargetPosition: targetPosition, pTargetZ: 0.0f);
				}
				
				int numberOfProjectiles_101 = 360;
				float circleRadius_101 = 55.5f;
				float totalAngle_101 = 360.0f;
				float totalAnglePerProjectile_101 = totalAngle_101 / numberOfProjectiles_101;
				for (int i = 0; i < numberOfProjectiles_101; i++)
				{
					float launchAngle = totalAnglePerProjectile_101 * i;
					float launchAngleRad = launchAngle * Mathf.Deg2Rad;
					UnityEngine.Vector3 launchPosition_101 = new UnityEngine.Vector3(
						targetPosition.x + circleRadius_101 * Mathf.Cos(launchAngleRad),
						targetPosition.y + circleRadius_101 * Mathf.Sin(launchAngleRad),
						targetPosition.z
					);
					World.world.projectiles.spawn(pInitiator: selfActor, pTargetObject: mainTarget, pAssetID: "BladeBlack02", pLaunchPosition: launchPosition_101, pTargetPosition: targetPosition, pTargetZ: 0.0f);
				}
				return true;
			}
			return false;
		}
		public static bool EvilSwordSlashX2(BaseSimObject pSelf, WorldTile pTile = null)
		{// 魔劍 轉接 (EvilSwordSlash 04 05)
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			if (!selfActor.hasStatus("arrogant_demon_king"))
			{
				return false;
			}
			// 隨機選擇要觸發的攻擊效果
			float randomValue = UnityEngine.Random.value; // 獲取一個 0 到 1 的隨機值
			if (randomValue <= 0.50f)
			{
				return EvilSwordSlash04(pSelf, pTile);
			}
			else
			{
				return EvilSwordSlash05(pSelf, pTile);
			}
		}
		public static bool EvilSwordSlash04(BaseSimObject pSelf, WorldTile pTile = null)
		{// 魔劍 常態攻擊 扇狀霰射 item_cdt03 飛劍
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			if (!selfActor.hasStatus("arrogant_demon_king"))
			{
				return false;
			}
			// === 冷卻狀態檢查 ===
			string attackCooldownStatus = "item_cdt03";
			float attackCooldownDuration = 0.03f;
			if (selfActor.hasStatus(attackCooldownStatus))
			{
				return false;
			}
			
			// === 新增：根據周圍敵人的數量決定是否發動 ===
			float detectionRange = 40f;		//	範圍
			int requiredEnemies = 3;		//	設定發動所需的最少敵人數
			int enemyCount = 0;
			BaseSimObject mainTarget = null;
			float closestDist = float.MaxValue;
			
			// 遍歷所有單位和建築物來計數和尋找主目標
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive() || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist <= detectionRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = other;
					}
				}
			}
			foreach (var building in World.world.buildings)
			{
				if (building == null || !building.isAlive() || !selfActor.kingdom.isEnemy(building.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, building.current_position);
				if (dist <= detectionRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = building;
					}
				}
			}
			
			// 如果沒有找到足夠的目標，則退出
			if (enemyCount < requiredEnemies || mainTarget == null)
			{
				return false;
			}
			
			// === 成功找到目標，施加冷卻並發射投射物 ===
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			//selfActor.addStatusEffect("invincible", 0.05f);
			mainTarget.finishStatusEffect("invincible");
			mainTarget.finishStatusEffect("shield");

			UnityEngine.Vector3 selfPosition = selfActor.current_position;
			UnityEngine.Vector3 targetPosition = mainTarget.current_position;
			
			float targetSize = 1f;
			if (mainTarget.isActor() && mainTarget.a.stats != null)
			{
				targetSize = mainTarget.a.stats["size"];
			}
			
			UnityEngine.Vector3 basePoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, targetPosition.x, targetPosition.y, selfActor.stats["size"]);
			basePoint.y += 0.5f;

			// 子彈相關設定
			int numberOfProjectiles_00 = 12;
			float spreadAngle_00 = 10.0f;
			for (int i = 0; i < numberOfProjectiles_00; i++)
			{
				UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, spreadAngle_00 * (i - (numberOfProjectiles_00 - 1f) / 2f));
				UnityEngine.Vector3 spreadTarget = rotation * (targetPosition - basePoint) + basePoint;
				World.world.projectiles.spawn(
					pInitiator: selfActor,
					pTargetObject: mainTarget,
					pAssetID: "BladeWhite01",
					pLaunchPosition: basePoint,
					pTargetPosition: spreadTarget,
					pTargetZ: 0.0f
				);
			}
			int numberOfProjectiles_01 = 13;
			float spreadAngle_01 = 10.0f;
			for (int i = 0; i < numberOfProjectiles_01; i++)
			{
				UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, spreadAngle_01 * (i - (numberOfProjectiles_01 - 1f) / 2f));
				UnityEngine.Vector3 spreadTarget = rotation * (targetPosition - basePoint) + basePoint;
				World.world.projectiles.spawn(
					pInitiator: selfActor,
					pTargetObject: mainTarget,
					pAssetID: "BladeBlack01",
					pLaunchPosition: basePoint,
					pTargetPosition: spreadTarget,
					pTargetZ: 0.0f
				);
			}
			return true;
		}
		public static bool EvilSwordSlash05(BaseSimObject pSelf, WorldTile pTile = null)
		{// 魔劍 常態攻擊 環狀集火 item_cdt04 氣刃斬
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			if (!selfActor.hasStatus("arrogant_demon_king"))
			{
				return false;
			}
			// === 冷卻狀態檢查 ===
			string attackCooldownStatus = "item_cdt04";
			float attackCooldownDuration = 0.01f;
			if (selfActor.hasStatus(attackCooldownStatus))
			{
				return false;
			}
			
			// === 新增：根據周圍敵人的數量決定是否發動 ===
			float detectionRange = 60f;
			int requiredEnemies = 1;
			int enemyCount = 0;
			BaseSimObject mainTarget = null;
			float closestDist = float.MaxValue;
			
			// 遍歷所有單位和建築物來計數和尋找主目標
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive() || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist <= detectionRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = other;
					}
				}
			}
			foreach (var building in World.world.buildings)
			{
				if (building == null || !building.isAlive() || !selfActor.kingdom.isEnemy(building.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, building.current_position);
				if (dist <= detectionRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = building;
					}
				}
			}
			
			// 如果沒有找到足夠的目標，則退出
			if (enemyCount < requiredEnemies || mainTarget == null)
			{
				return false;
			}
			
			// === 成功找到目標，施加冷卻並發射投射物 ===
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			//selfActor.addStatusEffect("invincible", 0.05f);
			mainTarget.finishStatusEffect("invincible");
			mainTarget.finishStatusEffect("shield");

			UnityEngine.Vector3 selfPosition = selfActor.current_position;
			UnityEngine.Vector3 targetPosition = mainTarget.current_position;
			
			UnityEngine.Vector3 baseLaunchPoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, targetPosition.x, targetPosition.y, selfActor.stats["size"]);
			baseLaunchPoint.y += 0.5f;

			// 子彈相關設定 - 環狀攻擊
			int numberOfProjectiles_00 = 12;
			float spreadDistance_00 = 5.0f;
			float spreadAngle_00 = 360.0f;
			float totalSpread = spreadAngle_00 / numberOfProjectiles_00;
			for (int i = 0; i < numberOfProjectiles_00; i++)
			{
				float angle = totalSpread * i - spreadAngle_00 / 2f;
				UnityEngine.Vector3 launchPosition_00 = baseLaunchPoint + Quaternion.Euler(0f, 0f, angle) * new Vector3(spreadDistance_00, 0, 0);
				World.world.projectiles.spawn(
					pInitiator: selfActor,
					pTargetObject: mainTarget,
					pAssetID: "BladeWhite",
					pLaunchPosition: launchPosition_00,
					pTargetPosition: targetPosition,
					pTargetZ: 0.0f
				);
				World.world.projectiles.spawn(
					pInitiator: selfActor,
					pTargetObject: mainTarget,
					pAssetID: "BladeBlack",
					pLaunchPosition: launchPosition_00,
					pTargetPosition: targetPosition,
					pTargetZ: 0.0f
				);
			}
			return true;
		}
		public static bool EvilSwordSlash06(BaseSimObject pSelf, WorldTile pTile = null)
		{// 魔劍 常態攻擊 收縮包圍 item_cdt05 第二終結 The End of the World
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			if (!selfActor.hasStatus("arrogant_demon_king"))
			{
				return false;
			}
			// 機率檢查
			if (!Randy.randomChance(0.025f))
			{
				return false;
			}
			// === 冷卻狀態檢查 ===
			string attackCooldownStatus = "item_cdt05";
			float attackCooldownDuration = 3600.00f;
			if (selfActor.hasStatus(attackCooldownStatus))
			{
				return false;
			}
			// === 新增：根據周圍敵人的數量決定是否發動 ===
			float detectionRange = 900f;
			int requiredEnemies = 3;
			int enemyCount = 0;
			BaseSimObject mainTarget = null;
			float closestDist = float.MaxValue;
			foreach (var other in World.world.units)
			{//對單位
				if (other == null || other == selfActor || !other.isAlive() || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist <= detectionRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = other;
					}
				}
			}
			foreach (var building in World.world.buildings)
			{//對建築
				if (building == null || !building.isAlive() || !selfActor.kingdom.isEnemy(building.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, building.current_position);
				if (dist <= detectionRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = building;
					}
				}
			}
			if (enemyCount < requiredEnemies || mainTarget == null)
			{
				return false;
			}
			// === 成功找到目標，施加冷卻並發射投射物 ===
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			//selfActor.addStatusEffect("invincible", 0.05f);
			mainTarget.finishStatusEffect("invincible");
			mainTarget.finishStatusEffect("shield");

			UnityEngine.Vector3 targetPosition = mainTarget.current_position;
			var projectilesToSpawn = new (string id, float radius)[]
			{
				("BladeWhite03", 45.0f), ("BladeBlack03", 46.0f),
				("BladeWhite03", 47.0f), ("BladeBlack03", 48.0f),
				("BladeWhite03", 49.0f), ("BladeBlack03", 50.0f),
				("BladeWhite03", 51.0f), ("BladeBlack03", 52.0f),
				("BladeWhite03", 53.0f), ("BladeBlack03", 54.0f),
				("BladeWhite03", 55.0f), ("BladeBlack03", 56.0f)
			};
			foreach (var projectile in projectilesToSpawn)
			{
				int numberOfProjectiles = 360;
				float totalAngle = 360.0f;
				float totalAnglePerProjectile = totalAngle / numberOfProjectiles;
				for (int i = 0; i < numberOfProjectiles; i++)
				{
					float launchAngle = totalAnglePerProjectile * i;
					float launchAngleRad = launchAngle * Mathf.Deg2Rad;
					UnityEngine.Vector3 launchPosition = new UnityEngine.Vector3(
						targetPosition.x + projectile.radius * Mathf.Cos(launchAngleRad),
						targetPosition.y + projectile.radius * Mathf.Sin(launchAngleRad),
						targetPosition.z
					);
					World.world.projectiles.spawn(
						pInitiator: selfActor,
						pTargetObject: mainTarget,
						pAssetID: projectile.id,
						pLaunchPosition: launchPosition,
						pTargetPosition: targetPosition,
						pTargetZ: 0.0f
					);
				}
			}
			return true;
		}
		public static bool EvilSwordAwakens(BaseSimObject pSelf, WorldTile pTile = null)
		{// 魔劍 武裝復甦
			// 1. 基本安全檢查：確保 pSelf 及其 Actor 組件存在且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			// 如果單位持有 "arrogant_demon_king" 狀態
			if (selfActor.hasStatus("arrogant_demon_king"))
			{
				// 則為其持續添加 "adk2" 狀態，每次添加時設定持續時間為 10 秒
				// 如果 adk2 已經存在，此操作會刷新其持續時間為 10 秒
				selfActor.addStatusEffect("adk2", 3600f); // 將時間改為 10 秒
			}
			return false; // 特殊效果通常返回 false，表示不阻止其他效果鏈
		}
		public static bool Extinguished01(BaseSimObject pTarget, WorldTile pTile = null)
		{// 魔劍 滅火
			if (pTarget == null || !pTarget.isActor() || !pTarget.a.isAlive())
				return false;
			Actor selfActor = pTarget.a;
			int Extinguished_RADIUS = 20;
			World.world.loopWithBrush(pTarget.current_tile,
			Brush.get(Extinguished_RADIUS, "circ_"),
			new PowerActionWithID(Traits01Actions.Extinguished_Assist),
			null);
			return false;
		}
		public static bool EvilLawGet01(BaseSimObject pTarget, WorldTile pTile)
		{// 魔劍 給予拾獲者給予拾獲者認證狀態
			// 1. 安全檢查：確保 pTarget 存在且有 Actor 組件
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor targetActor = pTarget.a;
			// =======================================================
			// === 新增/修改邏輯：檢查王國內是否已存在特定魔王 ===
			const string DemonKingStatusID = "arrogant_demon_king";
			Kingdom currentKingdom = targetActor.kingdom;
			// 檢查目標單位是否屬於一個國家
			if (currentKingdom != null)
			{
				// 遍歷王國中的所有單位
				foreach (Actor kingdomUnit in currentKingdom.units)
				{
					// 跳過自己，只檢查其他單位
					if (kingdomUnit == null || kingdomUnit == targetActor)
					{
						continue;
					}
					// 檢查其他單位是否擁有 "???_demon_king" 狀態
					if (kingdomUnit.hasStatus(DemonKingStatusID))
					{
						// 如果找到，則直接返回 false，不執行後續操作
						return false;
					}
				}
			}
			// =======================================================
			// 2. 如果通過了上面的檢查，繼續執行後續邏輯
			// 檢查是否持有 特質
			if (targetActor.hasTrait("evillaw_ew"))
			{
				// 如果單位已經有 特質，就不進行任何動作
				return true; 
			}
			// 3. 如果沒有 特質，則檢查是否持有 狀態效果
			else if (!targetActor.hasStatus("adk3"))
			{
				// 如果都沒有，就添加 狀態效果
				targetActor.addStatusEffect("adk3", 600f);
			}
			return true; // 表示操作成功執行
		}
			#endregion
			#region 強欲之銃
	//銃
		public static bool Anti_Poverty(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 魔銃 貧者殺手
			// 安全檢查：確保施法者和目標都存在且有效
			if (pSelf?.a == null || pTarget?.a == null || !pSelf.a.isAlive() || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			Actor targetActor = pTarget.a;
			// 檢查目標種族是否與施法者不同
/*			if (targetActor.hasTrait("holyarts_justice"))
			{// 如果目標持有 特質 則返回
				return false;
			}*/
			if (targetActor.data.money < selfActor.data.money || targetActor.data.loot < selfActor.data.loot)
			{
				// 取得施法者的傷害值
				float selfDamage = selfActor.stats["damage"];
				// 施加額外傷害 (例如額外造成 50% 傷害)
				targetActor.getHit(selfDamage * 0.50f, true, AttackType.Weapon, selfActor, false, true, false);
				return true;
			}
			return false;
		}
		public static bool Anti_Poverty_Defense(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 魔銃 貧者特防
			// 1. 安全檢查：確保施法者 (防禦者) 和目標 (攻擊者) 都存在且有效
			if (pSelf?.a == null || pTarget?.a == null || !pSelf.a.isAlive() || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a; // 防禦者
			Actor targetActor = pTarget.a; // 攻擊者
			if (!selfActor.hasStatus("greedy_demon_king") || !selfActor.hasStatus("defense_on"))
			{
				return false;
			}
			if (targetActor.data.money < selfActor.data.money || targetActor.data.loot < selfActor.data.loot)
			{				
				// 取得攻擊者對防禦者造成的傷害值
				float damageDealt = targetActor.stats["damage"] ;
				// 你可以根據需要調整這個百分比
				float damageToHeal = damageDealt * 0.75f;
				// 直接增加生命值，以抵銷受到的傷害
				selfActor.data.health += (int)damageToHeal;
				// 確保生命值不會超過最大值
				if (selfActor.data.health > selfActor.getMaxHealth())
				{
					selfActor.data.health = selfActor.getMaxHealth();
				}
				return true;
			}
			return false;
		}
		public static bool ForcedBorrowing1(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 魔銃 強制借貸
			// 安全檢查：確保施法者和目標都存在且有效
			if (pSelf?.a == null || pTarget?.a == null || !pSelf.a.isAlive() || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			Actor targetActor = pTarget.a;
			targetActor.data.money -= 100;
			selfActor.data.money +=100;
			return true;
			
		}
		public static bool ForcedBorrowing2(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 魔銃 強制借貸
			// 安全檢查：確保施法者和目標都存在且有效
			if (pSelf?.a == null || pTarget?.a == null || !pSelf.a.isAlive() || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			Actor targetActor = pTarget.a;
			targetActor.data.loot -= 100;
			selfActor.data.loot +=300;
			return true;
		}
		public static bool EvilGunShooting00_1(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 魔銃 追加攻擊 霰射 不消費
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			string requiredStatus = "greedy_demon_king";
			
			// 檢查攻擊者是否擁有必要狀態
			if (!selfActor.hasStatus(requiredStatus))
			{
				return false;
			}
			
			// === 冷卻狀態檢查邏輯 ===
			string attackCooldownStatus = "item_cdt00";
			float attackCooldownDuration = 0.01f;
			if (selfActor.hasStatus(attackCooldownStatus))
			{
				return false;
			}
			
			// === 核心邏輯：根據周圍敵人的數量決定是否發動 ===
			float detectionRange = 15f; // 偵測敵人的範圍
			int requiredEnemies = 3;	// 發動所需的最少敵人數
			int enemyCount = 0;
			BaseSimObject mainTarget = null; // 儲存最接近的敵方目標
			float closestDist = float.MaxValue;

			// 遍歷所有單位和建築物來計數和尋找主目標
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist <= detectionRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = other;
					}
				}
			}
			foreach (var building in World.world.buildings)
			{
				if (building == null || !building.isAlive())
					continue;
				if (building.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(building.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, building.current_position);
				if (dist <= detectionRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = building;
					}
				}
			}
			
			// 只有當敵人數達標且找到主目標時，才發動攻擊
			if (enemyCount >= requiredEnemies && mainTarget != null)
			{
				// === 滿足條件，施加冷卻並發射投射物 ===
				selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
				//selfActor.addStatusEffect("invincible", 0.05f);



				UnityEngine.Vector3 selfPosition = selfActor.current_position;
				UnityEngine.Vector3 targetPosition = mainTarget.current_position;
				
				float targetSize = 1f;
				if (mainTarget.isActor() && mainTarget.a.stats != null)
				{
					targetSize = mainTarget.a.stats["size"];
				}
				
				UnityEngine.Vector3 basePoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, targetPosition.x, targetPosition.y, selfActor.stats["size"]);
				UnityEngine.Vector3 targetPoint = Toolbox.getNewPoint(targetPosition.x, targetPosition.y, targetPosition.x, targetPosition.y, targetSize, true);
				
				// --- 為不同子彈類型定義獨立的設定 ---
				int Projectiles_001 = 10;
				float spreadAngle_001 = 0.25f;
				
				// --- 發射 001 ---
				for (int i = 0; i < Projectiles_001; i++)
				{
					UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, spreadAngle_001 * (i - (Projectiles_001 - 1f) / 2f));
					UnityEngine.Vector3 spreadTarget = rotation * (targetPoint - basePoint) + basePoint;
					World.world.projectiles.spawn(
					pInitiator: selfActor,
					pTargetObject: mainTarget,
					pAssetID: "HighSpeedBullet",
					pLaunchPosition: basePoint,
					pTargetPosition: spreadTarget,
					pTargetZ: 0.0f);
				}
				
				return true;
			}
			
			return false;
		}
		public static bool EvilGunShooting00_2(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 魔銃 追加攻擊 列射 不消費
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive() || pTarget == null || !pTarget.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			BaseSimObject mainTarget = pTarget;
			// 冷卻機制
			string requiredStatus = "greedy_demon_king";
			string attackCooldownStatus = "item_cdt00";
			float attackCooldownDuration = 0.01f; 
			if (selfActor.hasStatus(attackCooldownStatus) || !selfActor.hasStatus(requiredStatus))
			{
				return false;
			}
			// === 核心邏輯：距離和敵對檢查 (取代目標尋找迴圈) ===
			float maxRange = 30f;	   // 最大有效範圍
			float minRange = 3f;		// 最小有效範圍
			// 1. 檢查敵對關係
			if (mainTarget.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(mainTarget.kingdom))
				return false;
			// 2. 檢查距離是否在有效範圍內
			float dist = Vector2.Distance(selfActor.current_position, mainTarget.current_position);
			if (dist > maxRange || dist < minRange)
			{
				return false;
			}
			// === 滿足所有條件，執行射擊 ===
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			UnityEngine.Vector3 selfPosition = selfActor.current_position;
			UnityEngine.Vector3 targetPosition = mainTarget.current_position;
			// 計算發射者到目標的標準化方向向量
			UnityEngine.Vector3 directionVector = targetPosition - selfPosition;
			UnityEngine.Vector3 normalizedDirection = directionVector.normalized;
			// 計算第一個子彈的發射點 (BasePoint)，確保在單位碰撞體之外
			UnityEngine.Vector3 initialBasePoint = Toolbox.getNewPoint(
				selfPosition.x, selfPosition.y, 
				targetPosition.x, targetPosition.y, 
				selfActor.stats["size"] + 0.1f 
			);
			// === 核心發射邏輯：分開處理 ===
			// --- 【A. 先導彈 (槍頭)】---
			string projectileAssetID_01 = "HighSpeedBullet"; 
			// 計算第一顆子彈的發射點 (i=0)
			UnityEngine.Vector3 launchPoint_01 = initialBasePoint; // separationDistance_01 = 0f
			World.world.projectiles.spawn(
				pInitiator: selfActor,
				pTargetObject: mainTarget,
				pAssetID: projectileAssetID_01,
				pLaunchPosition: launchPoint_01,
				pTargetPosition: targetPosition,
				pTargetZ: 0.0f
			);
			// --- 【B. 後續彈 (槍身)】---
			int numberOfProjectiles_00 = 9;
			float separationDistance_00 = 0.1f;
			string projectileAssetID_00 = "HighSpeedBullet";
			// 從 i=1 開始循環 (因為 i=0 已經發射過了)
			for (int i = 1; i <= numberOfProjectiles_00; i++)
			{
				float currentOffset = i * separationDistance_00;
				// 計算當前子彈的發射點
				UnityEngine.Vector3 launchPoint_00 = initialBasePoint + (normalizedDirection * currentOffset);
				
				World.world.projectiles.spawn(
					pInitiator: selfActor,
					pTargetObject: mainTarget,
					pAssetID: projectileAssetID_00,
					pLaunchPosition: launchPoint_00,
					pTargetPosition: targetPosition,
					pTargetZ: 0.0f
				);
			}
			return true; // 成功發動攻擊效果
		}
		public static bool EvilGunShooting01(BaseSimObject pSelf, WorldTile pTile = null)
		{// 魔銃 常態攻擊 霰射 消費 0
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			if (!selfActor.hasStatus("greedy_demon_king"))
			{
				return false;
			}
			// === 冷卻狀態檢查 ===
			string attackCooldownStatus = "item_cdt01";
			float attackCooldownDuration = 0.01f;
			if (selfActor.hasStatus(attackCooldownStatus))
			{
				return false;
			}
			
			// === 核心邏輯：根據周圍敵人的數量決定是否發動 (預設至少1個敵人) ===
			float detectionRange = 15f;
			int requiredEnemies = 1;
			int enemyCount = 0;
			BaseSimObject mainTarget = null;
			float closestDist = float.MaxValue;
			
			// 遍歷所有單位和建築物來計數和尋找主目標
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive() || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist <= detectionRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = other;
					}
				}
			}
			foreach (var building in World.world.buildings)
			{
				if (building == null || !building.isAlive() || !selfActor.kingdom.isEnemy(building.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, building.current_position);
				if (dist <= detectionRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = building;
					}
				}
			}
			
			if (enemyCount < requiredEnemies || mainTarget == null)
			{
				return false;
			}

			// === 成功找到目標，施加冷卻並發射投射物 ===
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			//selfActor.addStatusEffect("invincible", 0.05f);



			UnityEngine.Vector3 selfPosition = selfActor.current_position;
			UnityEngine.Vector3 targetPosition = mainTarget.current_position;
			
			UnityEngine.Vector3 basePoint = selfPosition;
			basePoint.y += 0.5f;

			int numberOfProjectiles_00 = 10;
			float spreadAngle_00 = 0.50f;
			
			for (int i = 0; i < numberOfProjectiles_00; i++)
			{
				UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, spreadAngle_00 * (i - (numberOfProjectiles_00 - 1f) / 2f));
				UnityEngine.Vector3 spreadTarget = rotation * (targetPosition - basePoint) + basePoint;
				
				World.world.projectiles.spawn(
					pInitiator: selfActor,
					pTargetObject: mainTarget,
					pAssetID: "HighSpeedBullet",
					pLaunchPosition: basePoint,
					pTargetPosition: spreadTarget,
					pTargetZ: 0.0f
				);
			}
			return true;
		}
		public static bool EvilGunShooting02(BaseSimObject pSelf, WorldTile pTile = null)
		{// 魔銃 常態攻擊 環帶 消費 100
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;

			if (!selfActor.hasStatus("greedy_demon_king"))
			{
				return false;
			}
			
			string attackCooldownStatus = "item_cdt02";
			float attackCooldownDuration = 5.00f;
			if (selfActor.hasStatus(attackCooldownStatus))
			{
				return false;
			}
			
			// === 核心邏輯：根據周圍敵人的數量決定是否發動 (預設至少1個敵人) ===
			float maxRange = 30f;
			int requiredEnemies = 1;
			int enemyCount = 0;
			BaseSimObject mainTarget = null;
			float closestDist = float.MaxValue;
			
			// 遍歷所有單位和建築物來計數和尋找主目標
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive() || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist <= maxRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = other;
					}
				}
			}
			foreach (var building in World.world.buildings)
			{
				if (building == null || !building.isAlive() || !selfActor.kingdom.isEnemy(building.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, building.current_position);
				if (dist <= maxRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = building;
					}
				}
			}

			if (enemyCount < requiredEnemies || mainTarget == null)
			{
				return false;
			}

			// === 金錢與施法後續 ===
			if (selfActor.data.money < 6000 /*&& selfActor.data.loot < 6000 */) // 調整金錢
			{
				return false;
			}
			selfActor.data.money -= 50;
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);



			UnityEngine.Vector3 selfPosition = selfActor.current_position;
			UnityEngine.Vector3 targetPosition = mainTarget.current_position;
			
			UnityEngine.Vector3 baseLaunchPoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, targetPosition.x, targetPosition.y, selfActor.stats["size"]);
			baseLaunchPoint.y += 0.5f;

			int numberOfProjectiles_00 = 24;
			float spreadDistance_00 = 5.0f;
			float spreadAngle_00 = 360.0f;
			float totalSpread = spreadAngle_00 / numberOfProjectiles_00;
			for (int i = 0; i < numberOfProjectiles_00; i++)
			{
				float angle = totalSpread * i - spreadAngle_00 / 2f;
				UnityEngine.Vector3 launchPosition_00 = baseLaunchPoint + Quaternion.Euler(0f, 0f, angle) * new Vector3(spreadDistance_00, 0, 0);

				World.world.projectiles.spawn(
					pInitiator: selfActor,
					pTargetObject: mainTarget,
					pAssetID: "HighSpeedBullet",
					pLaunchPosition: launchPosition_00,
					pTargetPosition: targetPosition,
					pTargetZ: 0.0f
				);
			}
			return true;
		}
		public static bool EvilGunShooting03(BaseSimObject pSelf, WorldTile pTile = null)
		{// 魔銃 常態攻擊 狙擊 消費 10000
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			if (!selfActor.hasStatus("greedy_demon_king"))
			{
				return false;
			}
			
			string attackCooldownStatus = "item_cdt03";
			float attackCooldownDuration = 10.00f;
			if (selfActor.hasStatus(attackCooldownStatus))
			{
				return false;
			}
			
			// === 核心邏輯：根據周圍敵人的數量決定是否發動 (預設至少1個敵人) ===
			float maxRange = 60f;
			int requiredEnemies = 1;
			int enemyCount = 0;
			BaseSimObject mainTarget = null;
			float closestDist = float.MaxValue;
			
			// 遍歷所有單位和建築物來計數和尋找主目標
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive() || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist <= maxRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = other;
					}
				}
			}
			foreach (var building in World.world.buildings)
			{
				if (building == null || !building.isAlive() || !selfActor.kingdom.isEnemy(building.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, building.current_position);
				if (dist <= maxRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = building;
					}
				}
			}
			
			if (enemyCount < requiredEnemies || mainTarget == null)
			{
				return false;
			}

			// === 金錢與施法後續 ===
			if (selfActor.data.money < 10000 )
			{
				return false;
			}
			selfActor.data.money -= 10000;
			//selfActor.data.loot -= 500;
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			//selfActor.addStatusEffect("invincible", 0.05f);
			mainTarget.finishStatusEffect("invincible");



			UnityEngine.Vector3 targetPosition = mainTarget.current_position;

			int numberOfProjectiles_00 = 360;
			float circleRadius_00 = 0.50f;
			float totalAngle_00 = 360.0f;
			float totalAnglePerProjectile_00 = totalAngle_00 / numberOfProjectiles_00;
			
			for (int i = 0; i < numberOfProjectiles_00; i++)
			{
				float launchAngle = totalAnglePerProjectile_00 * i;
				float launchAngleRad = launchAngle * Mathf.Deg2Rad;
				UnityEngine.Vector3 launchPosition_00 = new UnityEngine.Vector3(
					targetPosition.x + circleRadius_00 * Mathf.Cos(launchAngleRad),
					targetPosition.y + circleRadius_00 * Mathf.Sin(launchAngleRad),
					targetPosition.z
				);
				World.world.projectiles.spawn(
					pInitiator: selfActor,
					pTargetObject: mainTarget,
					pAssetID: "HighSpeedBullet",
					pLaunchPosition: launchPosition_00,
					pTargetPosition: targetPosition,
					pTargetZ: 0.0f
				);
			}
			return true;
		}
		public static bool EvilGunShooting04(BaseSimObject pSelf, WorldTile pTile = null)
		{// 魔銃 常態攻擊 直列狙擊 消費 50000
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			// === 冷卻參數設定 ===)
			// 冷卻機制
			string attackCooldownStatus = "item_cdt04";
			float attackCooldownDuration = 1800f; 
			if (selfActor.hasStatus(attackCooldownStatus) || !selfActor.hasStatus("greedy_demon_king"))
			{
				return false;
			}
			// === 核心邏輯：偵測敵人 (已整合 minRange) ===
			float maxRange = 500f;	   // 最大有效範圍
			float minRange = 100f;		// 最小有效範圍
			int requiredEnemies = 1;	// 最低目標數
			int enemyCount = 0;
			BaseSimObject mainTarget = null;
			float closestDist = float.MaxValue;
			// A. 遍歷所有單位，尋找符合條件的敵人
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				// 距離必須大於 minRange 且小於等於 maxRange
				if (dist <= maxRange && dist > minRange) 
				{
					enemyCount++;
					// 尋找最接近的目標作為攻擊中心
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = other;
					}
				}
			}
/*			// B. 遍歷所有建築物，尋找符合條件的敵人
			foreach (var building in World.world.buildings)
			{
				if (building == null || !building.isAlive())
					continue;
				if (building.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(building.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, building.current_position);
				// 距離必須大於 minRange 且小於等於 maxRange
				if (dist <= maxRange && dist > minRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = building;
					}
				}
			}
*/
			// 檢查是否滿足發動條件
			if (enemyCount < requiredEnemies || mainTarget == null)
			{
				return false;
			}
			// === 成功找到目標，施加冷卻並發射投射物 (一列式射擊) ===
			
			// === 金錢與施法後續 ===
			if (selfActor.data.money < 50000 )
			{
				return false;
			}
			selfActor.data.money -= 50000;
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			UnityEngine.Vector3 selfPosition = selfActor.current_position;
			UnityEngine.Vector3 targetPosition = mainTarget.current_position;
			// 計算發射者到目標的標準化方向向量
			UnityEngine.Vector3 directionVector = targetPosition - selfPosition;
			UnityEngine.Vector3 normalizedDirection = directionVector.normalized;
			// 計算第一個子彈的發射點 (BasePoint)，確保在單位碰撞體之外
			UnityEngine.Vector3 initialBasePoint = Toolbox.getNewPoint(
				selfPosition.x, selfPosition.y, 
				targetPosition.x, targetPosition.y, 
				selfActor.stats["size"] + 0.1f 
			);
			// === 核心發射邏輯：分開處理，交換順序 ===
			// --- 【A. 後續彈 (槍身) - 現在先發射】---
			int numberOfProjectiles_00 = 100;
			float separationDistance_00 = -0.01f;
			string projectileAssetID_00 = "HighSpeedBullet2";
			// 從 i=1 開始循環，先發射所有槍身彈藥
			for (int i = 1; i <= numberOfProjectiles_00; i++)
			{
				float currentOffset = i * separationDistance_00;
				// 計算當前子彈的發射點
				UnityEngine.Vector3 launchPoint_00 = initialBasePoint + (normalizedDirection * currentOffset);
				
				World.world.projectiles.spawn(
					pInitiator: selfActor,
					pTargetObject: mainTarget,
					pAssetID: projectileAssetID_00,
					pLaunchPosition: launchPoint_00,
					pTargetPosition: targetPosition,
					pTargetZ: 0.0f
				);
			}
			// --- 【B. 先導彈 】---
			string projectileAssetID_01 = "HighSpeedBullet2"; 
			// 計算第一顆子彈的發射點 (i=0)
			UnityEngine.Vector3 launchPoint_01 = initialBasePoint; // separationDistance_01 = 0f。
			World.world.projectiles.spawn(
				pInitiator: selfActor,
				pTargetObject: mainTarget,
				pAssetID: projectileAssetID_01,
				pLaunchPosition: launchPoint_01,
				pTargetPosition: targetPosition,
				pTargetZ: 0.0f
			);
			return true;
		}
		public static bool EvilGunAwakens(BaseSimObject pSelf, WorldTile pTile = null)
		{// 魔銃 武器復甦
			// 1. 基本安全檢查：確保 pSelf 及其 Actor 組件存在且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			// 如果單位持有 "arrogant_demon_king" 狀態
			if (selfActor.hasStatus("greedy_demon_king"))
			{
				// 則為其持續添加 "adk2" 狀態，每次添加時設定持續時間為 10 秒
				// 如果 adk2 已經存在，此操作會刷新其持續時間為 10 秒
				selfActor.addStatusEffect("gdk3", 3600f); // 將時間改為 10 秒
			}
			return false; // 特殊效果通常返回 false，表示不阻止其他效果鏈
		}
		public static bool EvilLawGet02(BaseSimObject pTarget, WorldTile pTile)
		{// 魔銃 給予拾獲者認證狀態
			// 1. 安全檢查：確保 pTarget 存在且有 Actor 組件
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor targetActor = pTarget.a;
			// =======================================================
			// === 新增/修改邏輯：檢查王國內是否已存在特定魔王 ===
			const string DemonKingStatusID = "greedy_demon_king";
			Kingdom currentKingdom = targetActor.kingdom;
			// 檢查目標單位是否屬於一個國家
			if (currentKingdom != null)
			{
				// 遍歷王國中的所有單位
				foreach (Actor kingdomUnit in currentKingdom.units)
				{
					// 跳過自己，只檢查其他單位
					if (kingdomUnit == null || kingdomUnit == targetActor)
					{
						continue;
					}
					// 檢查其他單位是否擁有 "???_demon_king" 狀態
					if (kingdomUnit.hasStatus(DemonKingStatusID))
					{
						// 如果找到，則直接返回 false，不執行後續操作
						return false;
					}
				}
			}
			// =======================================================
			// 2. 如果通過了上面的檢查，繼續執行後續邏輯
			// 檢查是否持有 特質
			if (targetActor.hasTrait("evillaw_moneylaw"))
			{
				// 如果單位已經有 特質，就不進行任何動作
				return true; 
			}
			// 3. 如果沒有 特質，則檢查是否持有 狀態效果
			else if (!targetActor.hasStatus("gdk2"))
			{
				// 如果都沒有，就添加 狀態效果
				targetActor.addStatusEffect("gdk2", 600f);
			}
			return true; // 表示操作成功執行
		}
			#endregion
			#region 色慾之弓
	//弓
		public static bool Shock_protectionX(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 魔弓 震盪防護 5%
			// 1. 安全檢查：確保發動者、目標和地塊資訊都存在。
			if (pSelf == null || pTarget == null || pTile == null)
			{
				return false;
			}
			if (!Randy.randomChance(0.05f)) // 0.05f 代表 5% 的機率
			{
				return false; // 如果機率檢查失敗，則直接退出，不發動效果
			}
			// 將 pSelf 轉換為 Actor 類型，並進行安全檢查
			Actor selfActor = pSelf.a;
			if (selfActor == null)
			{
				// 如果 pSelf 不是一個 Actor，則無法繼續
				return false;
			}
			if (!selfActor.hasStatus("lust_demon_king"))
			{
				return false;
			}
			string attackCooldownStatus = "item_cdt00";
			float attackCooldownDuration = 0.01f;
			if (selfActor.hasStatus(attackCooldownStatus))
			{
				return false;
			}
			// 2. 施加爆炸波紋與震盪效果。
			// 參數：中心地塊, 震盪半徑(3), 力量大小(0.3f), 影響地面(true), 其他參數
			World.world.applyForceOnTile(pTile, 50, 2.50f, true, 0, null, pTarget, null, false);
			
			// 參數：位置, 視覺半徑(3f), 持續時間(5.0f)
			EffectsLibrary.spawnExplosionWave(pTile.posV3, 0f, 5.00f);
			EffectsLibrary.spawnExplosionWave(pTile.posV3, 3f, 2.50f);
			EffectsLibrary.spawnExplosionWave(pTile.posV3, 6f, 0.99f);
			// 3. 回傳 true 表示攻擊成功發動。
			return true;
		}
		public static bool Anti_Eex_Defense(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 魔弓 性別特防
			// 1. 安全檢查：確保施法者 (防禦者) 和目標 (攻擊者) 都存在且有效
			if (pSelf?.a == null || pTarget?.a == null || !pSelf.a.isAlive() || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a; // 防禦者
			Actor targetActor = pTarget.a; // 攻擊者
			if (!selfActor.hasStatus("lust_demon_king") || !selfActor.hasStatus("defense_on"))
			{
				return false;
			}
			// 取得攻擊者對防禦者造成的傷害值
			float damageDealt = targetActor.stats["damage"];
			float damageToHeal = 0f;
			// 根據性別差異來決定傷害減免百分比
			if (targetActor.data.sex != selfActor.data.sex)
			{
				// 異性，高額減免
				damageToHeal = damageDealt * 0.75f;
			}
			else
			{
				// 同性，較低減免
				damageToHeal = damageDealt * 0.25f;
			}
			// 如果有任何傷害減免（理論上總是會），則執行生命值恢復邏輯
			if (damageToHeal > 0)
			{
				// 直接增加生命值，以抵銷受到的傷害
				selfActor.data.health += (int)damageToHeal;
				// 確保生命值不會超過最大值
				if (selfActor.data.health > selfActor.getMaxHealth())
				{
					selfActor.data.health = selfActor.getMaxHealth();
				}
				return true; // 效果成功發動
			}
			return false;
		}
		public static bool EvilBowShootingCounterattack(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 魔弓 反擊射擊 item_cdt04
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			// 檢查自身狀態和冷卻
			string requiredStatus = "lust_demon_king";			//特定狀態
			string attackCooldownStatus = "item_cdt04";	//冷卻狀態
			float attackCooldownDuration = 0.01f;		//冷卻狀態時長
			// 持有 冷卻狀態 和 未持有 特定狀態 的檢查
			if (selfActor.hasStatus(attackCooldownStatus) || !selfActor.hasStatus(requiredStatus))
			{
				return false;
			}
			// === 核心邏輯：根據周圍敵人的數量決定是否發動 ===
			float maxRange = 900f;   	// 最大有效範圍
			float minRange = 30f;		// 最小有效範圍
			int requiredEnemies = 1;	// 有效目標數量
			int enemyCount = 0;
			BaseSimObject mainTarget = null;
			float closestDist = float.MaxValue;
			// A. 遍歷所有單位，尋找符合條件的敵人
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				
				bool isAttackable = false;
				foreach (string traitID in unconvertibleTraits00)
				{
					if (other.hasTrait(traitID))
					{
						isAttackable = true;
						break;
					}
				}
				if (other.getProfession() == UnitProfession.King || other.getProfession() == UnitProfession.Leader)
				{
					isAttackable = true;
				}
				if (!isAttackable)
				{
					continue;
				}
				
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist < maxRange && dist > minRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = other;
					}
				}
			}
			// B. 遍歷所有建築物，尋找符合條件的敵人
			foreach (var building in World.world.buildings)
			{
				if (building == null || !building.isAlive())
					continue;
				if (building.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(building.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, building.current_position);
				// 【關鍵修正點】：距離必須大於 minRange 且小於等於 maxRange
				if (dist <= maxRange && dist > minRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = building;
					}
				}
			}
			// 只有當敵人數達標且找到主目標時，才發動攻擊
			if (enemyCount >= requiredEnemies && mainTarget != null)
			{
				// === 滿足條件，施加狀態並發射投射物 === selfActor 單位自身 , mainTarget 攻擊對象
				selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
				mainTarget.finishStatusEffect("shield");

				UnityEngine.Vector3 selfPosition = selfActor.current_position;
				UnityEngine.Vector3 targetPosition = mainTarget.current_position;
				
				UnityEngine.Vector3 baseLaunchPoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, targetPosition.x, targetPosition.y, selfActor.stats["size"]);
				baseLaunchPoint.y += 0.5f;

				float[] spreadDistances = { 5.0f, 5.2f, 5.4f, 5.6f, 5.8f };
				int projectileCountPerLayer = 36;
				float totalSpreadAngle = 360.0f;
				float angleStep = totalSpreadAngle / projectileCountPerLayer;
				// 外層迴圈：遍歷每一層半徑
				for (int j = 0; j < spreadDistances.Length; j++)
				{
					float currentSpreadDistance = spreadDistances[j];
					// 內層迴圈：發射該層的所有子彈
					for (int i = 0; i < projectileCountPerLayer; i++)
					{
						// 計算角度：確保子彈在圓周上平均分佈
						float angle = angleStep * i - totalSpreadAngle / 2f;
						// 計算發射點
						UnityEngine.Vector3 launchPosition = baseLaunchPoint + Quaternion.Euler(0f, 0f, angle) * new UnityEngine.Vector3(currentSpreadDistance, 0, 0);
						World.world.projectiles.spawn(
							pInitiator: selfActor,
							pTargetObject: mainTarget,
							pAssetID: "HighSpeedArrow2",
							pLaunchPosition: launchPosition,
							pTargetPosition: targetPosition,
							pTargetZ: 0.0f
						);
					}
				}
				return true;
			}
			return false;
		}
		public static bool EvilBowMainEffect(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 魔弓 攻擊
			// 1. 基本安全檢查：攻擊者或目標為空，或 Actor 無效/已死亡，不執行
			if (pSelf == null || pTarget == null || !pSelf.isActor() || !pSelf.a.isAlive() || !pTarget.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			// 常量定義
			const string LustDemonKingStatus = "lust_demon_king";
			const string AfterglowStatus = "afterglow";
			const string StunnedStatus = "stunned";
			const float Duration = 5f;
			HashSet<string> unconvertibleTraits = new HashSet<string>
			{
				"apostle",
				"slave",
				"undead_servant",			// 不死族從者
				"undead_servant2",			// 不死族從者
				"undead_servant3",			// 不死族從者
				"extraordinary_authority",	// 不死族從者
				"evillaw_ew",
				"evillaw_tantrum",
				"evillaw_seduction",
				"evillaw_sleeping",
				"evillaw_moneylaw",
				"evillaw_starvation",
				"evillaw_devour",
				"pro_king",				// 國王
				"pro_leader",			// 領主
				"holyarts_bond",		// 絆
				"holyarts_justice",		// 絆
				"strong_minded",		// 原版特質
				"desire_alien_mold",	// 原版特質
				"desire_computer",		// 原版特質
				"desire_golden_egg",	// 原版特質
				"desire_harp",			// 原版特質
				"madness",				// 原版特質
				"psychopath",			// 原版特質
			};
			// ******************************************************************
			if (selfActor.hasStatus(LustDemonKingStatus))
			{
				// --- 能力發動區塊 ---
				bool isUnconvertible = false;
				
				// 檢查目標是否為建築物，如果是，則視為不可轉化
				if (pTarget is Building)
				{
					isUnconvertible = true;
				}
				// 如果不是建築物（即為單位），則進行額外的不可轉化檢查
				else if (pTarget is Actor targetActor)
				{
					// 檢查目標職業是否為國王或領主
					if (targetActor.getProfession() == UnitProfession.King || targetActor.getProfession() == UnitProfession.Leader)
					{
						isUnconvertible = true;
					}
					// 檢查目標特質
					if (!isUnconvertible)
					{
						foreach (string traitID in unconvertibleTraits)
						{
							if (targetActor.hasTrait(traitID))
							{
								isUnconvertible = true;
								break;
							}
						}
					}
				}
				
				// 根據是否為不可轉化對象來執行不同效果
				if (isUnconvertible)
				{
					// 對於建築物和不可轉化的單位
					Items01Actions.EvilBowDamage(pTarget, pTile);
					// 只有單位才能被暈眩，因此進行類型檢查
					if (pTarget is Actor targetActor)
					{
						targetActor.addStatusEffect(StunnedStatus, Duration);
					}
				}
				else
				{
					// 對於可轉化的單位，施加 afterglow、stunned
					Actor targetActor = pTarget.a;
					targetActor.addStatusEffect(AfterglowStatus, Duration);
					targetActor.addStatusEffect(StunnedStatus, Duration);
					targetActor.finishStatusEffect("angry");
					targetActor.data.health += 9999;
					Traits01Actions.SeductionLaw(selfActor, pTile);
				}
				
				return true;
			}
			else
			{
				return false;
			}
		}
		public static bool EvilBowShooting01(BaseSimObject pSelf, WorldTile pTile = null)
		{// 魔弓 高速箭矢 常態攻擊 (零距九箭) item_cdt00 對不可轉化對象外
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			if (!selfActor.hasStatus("lust_demon_king"))
			{
				return false;
			}
			string attackCooldownStatus = "item_cdt00";
			float attackCooldownDuration = 0.01f;
			if (selfActor.hasStatus(attackCooldownStatus))
			{
				return false;
			}

			// === 核心邏輯：根據周圍敵人的數量決定是否發動 ===
			float maxRange = 60f;
			int requiredEnemies = 1;
			int enemyCount = 0;
			BaseSimObject mainTarget = null;
			float closestDist = float.MaxValue;
			
			// A. 優先尋找敵對單位（保留排除邏輯）
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				
				bool canAttack = true;
				foreach (string traitID in unconvertibleTraits00)
				{
					if (other.hasTrait(traitID))
					{
						canAttack = false;
						break;
					}
				}
				if (other.getProfession() == UnitProfession.King || other.getProfession() == UnitProfession.Leader)
				{
					canAttack = false;
				}
				if (!canAttack)
				{
					continue;
				}
				
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist < maxRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = other;
					}
				}
			}
			
			// B. 如果沒有找到單位，則尋找敵對建築
			foreach (var building in World.world.buildings)
			{
				if (building == null || !building.isAlive())
					continue;
				if (building.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(building.kingdom))
					continue;
				
				float dist = Vector2.Distance(selfActor.current_position, building.current_position);
				if (dist < maxRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = building;
					}
				}
			}

			if (enemyCount < requiredEnemies || mainTarget == null)
			{
				return false;
			}
			
			// === 成功找到目標，施加冷卻並發射投射物 ===
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			UnityEngine.Vector3 targetPosition = mainTarget.current_position;
			
			int numberOfProjectiles_00 = 9;
			float circleRadius_00 = 3.00f;
			float totalAngle_00 = 360.0f;
			float totalAnglePerProjectile_00 = totalAngle_00 / numberOfProjectiles_00;

			for (int i = 0; i < numberOfProjectiles_00; i++)
			{
				float launchAngle = totalAnglePerProjectile_00 * i;
				float launchAngleRad = launchAngle * Mathf.Deg2Rad;
				UnityEngine.Vector3 launchPosition_00 = new UnityEngine.Vector3(
					targetPosition.x + circleRadius_00 * Mathf.Cos(launchAngleRad),
					targetPosition.y + circleRadius_00 * Mathf.Sin(launchAngleRad),
					targetPosition.z
				);

				World.world.projectiles.spawn(
					pInitiator: selfActor,
					pTargetObject: mainTarget,
					pAssetID: "HighSpeedArrow",
					pLaunchPosition: launchPosition_00,
					pTargetPosition: targetPosition,
					pTargetZ: 0.0f
				);
			}
			return true;
		}
		public static bool EvilBowShooting02(BaseSimObject pSelf, WorldTile pTile = null)
		{// 魔弓 高速箭矢 常態攻擊 (三六連射) item_cdt01 對不可轉化對象
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			if (!selfActor.hasStatus("lust_demon_king"))
			{
				return false;
			}
			string attackCooldownStatus = "item_cdt01";
			float attackCooldownDuration = 0.01f;
			if (selfActor.hasStatus(attackCooldownStatus))
			{
				return false;
			}

			// === 核心邏輯：根據周圍敵人的數量決定是否發動 ===
			float maxRange = 99f;
			float minRange = 0f;
			int requiredEnemies = 1;
			int enemyCount = 0;
			BaseSimObject mainTarget = null;
			float closestDist = float.MaxValue;
			
			// A. 優先尋找敵對單位
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				
				bool isAttackable = false;
				foreach (string traitID in unconvertibleTraits00)
				{
					if (other.hasTrait(traitID))
					{
						isAttackable = true;
						break;
					}
				}
				if (other.getProfession() == UnitProfession.King || other.getProfession() == UnitProfession.Leader)
				{
					isAttackable = true;
				}
				if (!isAttackable)
				{
					continue;
				}
				
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist < maxRange && dist > minRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = other;
					}
				}
			}
			
			// B. 如果沒有找到單位，則尋找敵對建築
			foreach (var building in World.world.buildings)
			{
				if (building == null || !building.isAlive())
					continue;

				// 檢查是否為有害建築 (BadBuilding)
				if (!Traits01Actions.BadBuilding.Contains(building.asset.id))
				{
					continue; 
				}
				if (building.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(building.kingdom))
					continue;
				
				float dist = Vector2.Distance(selfActor.current_position, building.current_position);
				if (dist < maxRange && dist > minRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = building;
					}
				}
			}

			if (enemyCount < requiredEnemies || mainTarget == null)
			{
				return false;
			}
			
			// === 成功找到目標，施加冷卻並發射投射物 ===
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			mainTarget.finishStatusEffect("invincible");
			
			UnityEngine.Vector3 selfPosition = selfActor.current_position;
			UnityEngine.Vector3 targetPosition = mainTarget.current_position;
			UnityEngine.Vector3 baseLaunchPoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, targetPosition.x, targetPosition.y, selfActor.stats["size"]);
			baseLaunchPoint.y += 0.5f;

			int numberOfProjectiles_02 = 12;	
			float spreadDistance_02 = 5.4f;
			float spreadAngle_02 = 360.0f;
			float totalSpread_02 = spreadAngle_02 / numberOfProjectiles_02;
			for (int i = 0; i < numberOfProjectiles_02; i++)
			{
				float angle = totalSpread_02 * i - spreadAngle_02 / 2f;
				UnityEngine.Vector3 launchPosition_02 = baseLaunchPoint + Quaternion.Euler(0f, 0f, angle) * new Vector3(spreadDistance_02, 0, 0);
				World.world.projectiles.spawn(pInitiator: selfActor, pTargetObject: mainTarget, pAssetID: "HighSpeedArrow1", pLaunchPosition: launchPosition_02, pTargetPosition: targetPosition, pTargetZ: 0.0f);
			}
			int numberOfProjectiles_01 = 12;
			float spreadDistance_01 = 5.2f;
			float spreadAngle_01 = 360.0f;
			float totalSpread_01 = spreadAngle_01 / numberOfProjectiles_01;
			for (int i = 0; i < numberOfProjectiles_01; i++)
			{
				float angle = totalSpread_01 * i - spreadAngle_01 / 2f;
				UnityEngine.Vector3 launchPosition_01 = baseLaunchPoint + Quaternion.Euler(0f, 0f, angle) * new Vector3(spreadDistance_01, 0, 0);
				World.world.projectiles.spawn(pInitiator: selfActor, pTargetObject: mainTarget, pAssetID: "HighSpeedArrow1", pLaunchPosition: launchPosition_01, pTargetPosition: targetPosition, pTargetZ: 0.0f);
			}
			int numberOfProjectiles_00 = 12;
			float spreadDistance_00 = 5.0f;
			float spreadAngle_00 = 360.0f;
			float totalSpread_00 = spreadAngle_00 / numberOfProjectiles_00;
			for (int i = 0; i < numberOfProjectiles_00; i++)
			{
				float angle = totalSpread_00 * i - spreadAngle_00 / 2f;
				UnityEngine.Vector3 launchPosition_00 = baseLaunchPoint + Quaternion.Euler(0f, 0f, angle) * new Vector3(spreadDistance_00, 0, 0);
				World.world.projectiles.spawn(pInitiator: selfActor, pTargetObject: mainTarget, pAssetID: "HighSpeedArrow1", pLaunchPosition: launchPosition_00, pTargetPosition: targetPosition, pTargetZ: 0.0f);
			}
			return true;
		}
		public static bool EvilBowShooting03(BaseSimObject pSelf, WorldTile pTile = null)
		{// 魔弓 高速箭矢 常態攻擊 (獵殺射法) item_cdt02 對不可轉化對象
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			if (!selfActor.hasStatus("lust_demon_king"))
			{
				return false;
			}
			string attackCooldownStatus = "item_cdt02";
			float attackCooldownDuration = 60.00f;
			if (selfActor.hasStatus(attackCooldownStatus))
			{
				return false;
			}
			
			// === 核心邏輯：根據周圍敵人的數量決定是否發動 ===
			float maxRange = 200f;
			float minRange = 60f;
			int requiredEnemies = 1;
			int enemyCount = 0;
			BaseSimObject mainTarget = null;
			float closestDist = float.MaxValue;
			
			// A. 優先尋找敵對單位
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				
				bool isAttackable = false;
				foreach (string traitID in unconvertibleTraits00)
				{
					if (other.hasTrait(traitID))
					{
						isAttackable = true;
						break;
					}
				}
				if (other.getProfession() == UnitProfession.King || other.getProfession() == UnitProfession.Leader)
				{
					isAttackable = true;
				}
				if (!isAttackable)
				{
					continue;
				}
				
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist < maxRange && dist > minRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = other;
					}
				}
			}
			
			// B. 如果沒有找到單位，則尋找敵對建築
			foreach (var building in World.world.buildings)
			{
				if (building == null || !building.isAlive())
					continue;

				// 檢查是否為有害建築 (BadBuilding)
				if (!Traits01Actions.BadBuilding.Contains(building.asset.id))
				{
					continue; 
				}

				if (building.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(building.kingdom))
					continue;
				
				float dist = Vector2.Distance(selfActor.current_position, building.current_position);
				if (dist < maxRange && dist > minRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = building;
					}
				}
			}

			if (enemyCount < requiredEnemies || mainTarget == null)
			{
				return false;
			}
			
			// === 成功找到目標，施加冷卻並發射投射物 ===
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			mainTarget.finishStatusEffect("invincible");
			
			UnityEngine.Vector3 targetPosition = mainTarget.current_position;
			
			int numberOfProjectiles_02 = 360;
			float circleRadius_02 = 30f;
			float totalAngle_02 = 360.0f;
			float totalAnglePerProjectile_02 = totalAngle_02 / numberOfProjectiles_02;
			for (int i = 0; i < numberOfProjectiles_02; i++)
			{
				float launchAngle = totalAnglePerProjectile_02 * i;
				float launchAngleRad = launchAngle * Mathf.Deg2Rad;
				UnityEngine.Vector3 launchPosition_02 = new UnityEngine.Vector3(
					targetPosition.x + circleRadius_02 * Mathf.Cos(launchAngleRad),
					targetPosition.y + circleRadius_02 * Mathf.Sin(launchAngleRad),
					targetPosition.z
				);
				World.world.projectiles.spawn(pInitiator: selfActor, pTargetObject: mainTarget, pAssetID: "HighSpeedArrow1", pLaunchPosition: launchPosition_02, pTargetPosition: targetPosition, pTargetZ: 0.0f);
			}
			int numberOfProjectiles_01 = 360;
			float circleRadius_01 = 25f;
			float totalAngle_01 = 360.0f;
			float totalAnglePerProjectile_01 = totalAngle_01 / numberOfProjectiles_01;
			for (int i = 0; i < numberOfProjectiles_01; i++)
			{
				float launchAngle = totalAnglePerProjectile_01 * i;
				float launchAngleRad = launchAngle * Mathf.Deg2Rad;
				UnityEngine.Vector3 launchPosition_01 = new UnityEngine.Vector3(
					targetPosition.x + circleRadius_01 * Mathf.Cos(launchAngleRad),
					targetPosition.y + circleRadius_01 * Mathf.Sin(launchAngleRad),
					targetPosition.z
				);
				World.world.projectiles.spawn(pInitiator: selfActor, pTargetObject: mainTarget, pAssetID: "HighSpeedArrow1", pLaunchPosition: launchPosition_01, pTargetPosition: targetPosition, pTargetZ: 0.0f);
			}
			int numberOfProjectiles_00 = 360;
			float circleRadius_00 = 20f;
			float totalAngle_00 = 360.0f;
			float totalAnglePerProjectile_00 = totalAngle_00 / numberOfProjectiles_00;
			for (int i = 0; i < numberOfProjectiles_00; i++)
			{
				float launchAngle = totalAnglePerProjectile_00 * i;
				float launchAngleRad = launchAngle * Mathf.Deg2Rad;
				UnityEngine.Vector3 launchPosition_00 = new UnityEngine.Vector3(
					targetPosition.x + circleRadius_00 * Mathf.Cos(launchAngleRad),
					targetPosition.y + circleRadius_00 * Mathf.Sin(launchAngleRad),
					targetPosition.z
				);
				World.world.projectiles.spawn(pInitiator: selfActor, pTargetObject: mainTarget, pAssetID: "HighSpeedArrow1", pLaunchPosition: launchPosition_00, pTargetPosition: targetPosition, pTargetZ: 0.0f);
			}
			return true;
		}
		public static bool EvilBowShooting04(BaseSimObject pSelf, WorldTile pTile = null)
		{// 魔弓 隕石攻擊 常態攻擊 (星殞之彈) item_cdt03 對不可轉化對象
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			if (!selfActor.hasStatus("lust_demon_king"))
			{
				return false;
			}
			string attackCooldownStatus = "item_cdt03";
			float attackCooldownDuration = 2700.00f;
			if (selfActor.hasStatus(attackCooldownStatus))
			{
				return false;
			}
			
			// === 核心邏輯：根據周圍敵人的數量決定是否發動 ===
			float maxRange = 250f;
			float minRange = 100f;
			int requiredEnemies = 1;
			int enemyCount = 0;
			BaseSimObject mainTarget = null;
			float closestDist = float.MaxValue;
			
			// A. 優先遍歷所有單位，尋找最接近且符合條件的目標
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				
				bool isAttackable = false;
				foreach (string traitID in unconvertibleTraits00)
				{
					if (other.hasTrait(traitID))
					{
						isAttackable = true;
						break;
					}
				}
				if (other.getProfession() == UnitProfession.King || other.getProfession() == UnitProfession.Leader)
				{
					isAttackable = true;
				}
				if (!isAttackable)
				{
					continue;
				}
				
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist < maxRange && dist > minRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = other;
					}
				}
			}
			
			// B. 如果沒有找到單位，則尋找敵對建築
			foreach (var building in World.world.buildings)
			{
				if (building == null || !building.isAlive())
					continue;

				// 檢查是否為有害建築 (BadBuilding)
				if (!Traits01Actions.BadBuilding.Contains(building.asset.id))
				{
					continue; 
				}

				if (building.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(building.kingdom))
					continue;
				
				float dist = Vector2.Distance(selfActor.current_position, building.current_position);
				if (dist < maxRange && dist > minRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = building;
					}
				}
			}
			
			if (enemyCount < requiredEnemies || mainTarget == null)
			{
				return false;
			}
			
			// 成功找到目標，施加冷卻並發動攻擊
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			
			WorldTile targetTile = mainTarget.current_tile;
			
			if (targetTile != null)
			{
				float spreadRadius = 15f;

				int numberOfMeteors = 12;
				for (int i = 0; i < numberOfMeteors; i++)
				{
					float offsetX = UnityEngine.Random.Range(-spreadRadius, spreadRadius);
					float offsetY = UnityEngine.Random.Range(-spreadRadius, spreadRadius);
					
					int newTileX = (int)targetTile.pos.x + (int)offsetX;
					int newTileY = (int)targetTile.pos.y + (int)offsetY;

					if (newTileX >= 0 && newTileX < MapBox.width &&
						newTileY >= 0 && newTileY < MapBox.height)
					{
						WorldTile randomTile = World.world.GetTile(newTileX, newTileY);
						if (randomTile != null)
						{
							EffectsLibrary.spawn("fx_meteorite", randomTile, "meteorite_ex", null, 0f, -1f, -1f, selfActor);
						}
					}
				}
			}
			return true;
		}
		public static bool EvilBowAwakens(BaseSimObject pSelf, WorldTile pTile = null)
		{// 魔弓 復甦
			// 1. 基本安全檢查：確保 pSelf 及其 Actor 組件存在且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			// 如果單位持有 "arrogant_demon_king" 狀態
			if (selfActor.hasStatus("lust_demon_king"))
			{
				// 則為其持續添加 "adk2" 狀態，每次添加時設定持續時間為 10 秒
				// 如果 adk2 已經存在，此操作會刷新其持續時間為 10 秒
				selfActor.addStatusEffect("ldk2", 3600f); // 將時間改為 10 秒
			}
			return false; // 特殊效果通常返回 false，表示不阻止其他效果鏈
		}
		public static bool EvilBowDamage(BaseSimObject pTarget, WorldTile pTile = null)
		{// 魔弓 傷害
			// 1. 安全檢查：確保目標存在且存活
			if (pTarget == null || !pTarget.isAlive())
			{
				return false;
			}
			// --- 傷害計算與處理 ---
			int finalDamageAmount = 0;
			const int minDamage = 1;
			// A. 處理單位 (Actor)
			if (pTarget.isActor())
			{
				Actor targetActor = pTarget.a;
				float damagePercentage = 0.0333f;
				bool isSpecialTarget = false;
					// 檢查 holyarts_justice 特質
					if (targetActor.hasTrait("holyarts_justice"))
					{
						isSpecialTarget = true;
					}
					// 檢查是否持有任一魔王狀態
					if (!isSpecialTarget) // 如果還不是特殊目標，才繼續檢查
					{
						foreach (string statusID in SevenDemonKingStatus1)
						{
							if (targetActor.hasStatus(statusID))
							{
								isSpecialTarget = true;
								break; // 一旦找到，就跳出迴圈
							}
						}
					}
				if (isSpecialTarget)
				{
					damagePercentage = 0.02f; // 對魔王/正義單位調低傷害
				}
				// 單位傷害計算是基於 Health
				int calculatedDamage = (int)(targetActor.getHealth() * damagePercentage);
				finalDamageAmount = Mathf.Max(calculatedDamage, minDamage);
			}
			// B. 處理建築物 (Building)
			else if (pTarget is Building targetBuilding)
			{
				// 建築物通常沒有複雜的特質，直接使用基礎傷害比例
				float damagePercentage = 0.20f; // 建議對建築使用較低的百分比，防止過快摧毀城市
				// 建築傷害計算是基於 Building Health
				int calculatedDamage = (int)(targetBuilding.getHealth() * damagePercentage);
				finalDamageAmount = Mathf.Max(calculatedDamage, minDamage);
			}
			else
			{
				// 如果不是 Actor 也不是 Building，則返回
				return false;
			}
			// 2. 對目標造成傷害 (無論是單位還是建築，都使用 getHit)
			if (finalDamageAmount > 0)
			{
				// 保持您的核心機制：內部傷害 (pIsInternal = true)
				pTarget.getHit((float)finalDamageAmount, true, AttackType.None, null, false, false, true);
			}
			return true;
		}
		public static bool EvilLawGet03(BaseSimObject pTarget, WorldTile pTile)
		{// 魔弓 給予拾獲者認證狀態
			// 1. 安全檢查：確保 pTarget 存在且有 Actor 組件
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor targetActor = pTarget.a;
			// =======================================================
			// === 新增/修改邏輯：檢查王國內是否已存在特定魔王 ===
			const string DemonKingStatusID = "lust_demon_king";
			Kingdom currentKingdom = targetActor.kingdom;
			// 檢查目標單位是否屬於一個國家
			if (currentKingdom != null)
			{
				// 遍歷王國中的所有單位
				foreach (Actor kingdomUnit in currentKingdom.units)
				{
					// 跳過自己，只檢查其他單位
					if (kingdomUnit == null || kingdomUnit == targetActor)
					{
						continue;
					}
					// 檢查其他單位是否擁有 "???_demon_king" 狀態
					if (kingdomUnit.hasStatus(DemonKingStatusID))
					{
						// 如果找到，則直接返回 false，不執行後續操作
						return false;
					}
				}
			}
			// =======================================================
			// 2. 如果通過了上面的檢查，繼續執行後續邏輯
			// 檢查是否持有 特質
			if (targetActor.hasTrait("evillaw_seduction"))
			{
				// 如果單位已經有 特質，就不進行任何動作
				return true; 
			}
			// 3. 如果沒有 特質，則檢查是否持有 狀態效果
			else if (!targetActor.hasStatus("ldk3"))
			{
				// 如果都沒有，就添加 狀態效果
				targetActor.addStatusEffect("ldk3", 600f);
			}
			return true; // 表示操作成功執行
		}
			#endregion
			#region 憤怒拳甲
	//手
		public static bool Devour_HungerHealth(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 魔拳 營養值恢復
			// 確保施法者和目標有效且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive() ||
				pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false; 
			}
			Actor selfActor = pSelf.a;
			Actor targetActor = pTarget.a;
			int maxNutritionValue = 100; //營養值的最大值
			float selfNutritionRestoreAmount = 1f; // 恢復 3 點營養值
			// --- 持有特質的單位進行生命值百分比恢復 和 營養值絕對值恢復 ---
			selfActor.data.nutrition = Mathf.Min(maxNutritionValue, selfActor.data.nutrition + Mathf.RoundToInt(selfNutritionRestoreAmount));
			// Debug.Log($"{selfActor.name} 的營養值恢復了 {selfNutritionRestoreAmount} 點. 當前營養值: {selfActor.data.nutrition}");
			return true; // 效果成功執行
		}
		public static bool Anti_Invincible(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 魔拳 無敵粉碎
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
			// 目標必須持有 "invincible" 或 "shield" 狀態
			bool targetHasStatus1 = targetActor.hasStatus("invincible");
			bool targetHasStatus2 = targetActor.hasStatus("shield");
			// 只有當兩個條件都滿足時才執行後續邏輯
			selfActor.addStatusEffect("crosshair", 1f);
			selfActor.addStatusEffect("tantrum", 10f);
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
		public static bool Anti_Angry(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 魔拳 怒者殺手
			// 安全檢查：確保施法者和目標都存在且有效
			if (pSelf?.a == null || pTarget?.a == null || !pSelf.a.isAlive() || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			Actor targetActor = pTarget.a;
			float SetValue = 5.50f;
			if (targetActor.hasStatus("brave"))
			{// 如果目標持有 狀態 則返回
				SetValue = 2.75f;
			}
			if (targetActor.hasStatus("angry"))
			{
				// 取得施法者的傷害值
				float selfDamage = selfActor.stats["damage"];
				// 施加額外傷害 (例如額外造成 50% 傷害)
				targetActor.getHit(selfDamage * SetValue, true, AttackType.Weapon, selfActor, false, true, false);
				return true;
			}
			return false;
		}
		public static bool Anti_Angry_Defense(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 魔拳 怒者特防
			// 1. 安全檢查：確保施法者 (防禦者) 和目標 (攻擊者) 都存在且有效
			if (pSelf?.a == null || pTarget?.a == null || !pSelf.a.isAlive() || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a; // 防禦者
			Actor targetActor = pTarget.a; // 攻擊者
			if (!selfActor.hasStatus("wrath_demon_king") || !selfActor.hasStatus("defense_on"))
			{
				return false;
			}
			if (targetActor.hasStatus("angry") && selfActor.hasStatus("tantrum"))
			{				
				// 取得攻擊者對防禦者造成的傷害值
				float damageDealt = targetActor.stats["damage"];
				// 你可以根據需要調整這個百分比
				float damageToHeal = damageDealt * 0.75f;
				// 直接增加生命值，以抵銷受到的傷害
				selfActor.data.health += (int)damageToHeal;
				// 確保生命值不會超過最大值
				if (selfActor.data.health > selfActor.getMaxHealth())
				{
					selfActor.data.health = selfActor.getMaxHealth();
				}
				return true;
			}
			return false;
		}
		public static bool EvilGlovesStrike_Attack01(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 魔拳 子彈攻擊 item_cdt00 (瘋狂/通常)
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive() || pTarget == null || !pTarget.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			
			// 檢查目標是否為敵對單位或建築
			if (pTarget.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(pTarget.kingdom))
			{
				return false;
			}
			
			// === 狀態檢查 ===
			if (!selfActor.hasStatus("wrath_demon_king"))
			{
				return false;
			}
			string attackCooldownStatus = "item_cdt00";
			float attackCooldownDuration = 0.01f;
			if (selfActor.hasStatus(attackCooldownStatus))
			{
				return false;
			}
			
			// === 根據 'madness' 特質設定子彈數量 ===
			int numberOfProjectiles;
			int numberOfProjectiles_00;
			if (selfActor.hasTrait("madness"))
			{
				numberOfProjectiles = 6;
				numberOfProjectiles_00 = 30;
			}
			else
			{
				numberOfProjectiles = 12;
				numberOfProjectiles_00 = 60;
			}
			
			// === 成功找到目標，施加冷卻並發射投射物 ===
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			//selfActor.addStatusEffect("invincible", 0.05f);
			selfActor.addStatusEffect("crosshair", 1.00f);
			pTarget.addStatusEffect("angry", 300f);
			pTarget.finishStatusEffect("invincible");



			UnityEngine.Vector3 selfPosition = selfActor.current_position;
			UnityEngine.Vector3 targetPosition = pTarget.current_position;
			UnityEngine.Vector3 baseLaunchPoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, targetPosition.x, targetPosition.y, selfActor.stats["size"]);
			baseLaunchPoint.y += 0.5f;

			float spreadDistance = 5.0f;
			float spreadAngle = 360.0f;
			float totalSpread = numberOfProjectiles > 0 ? (spreadAngle / numberOfProjectiles) : 0f;
			for (int i = 0; i < numberOfProjectiles; i++)
			{
				float angle = totalSpread * i - spreadAngle / 2f;
				UnityEngine.Vector3 launchPosition = baseLaunchPoint + Quaternion.Euler(0f, 0f, angle) * new UnityEngine.Vector3(spreadDistance, 0, 0);

				World.world.projectiles.spawn(
					pInitiator: selfActor,
					pTargetObject: pTarget,
					pAssetID: "HighFireball",
					pLaunchPosition: launchPosition,
					pTargetPosition: targetPosition,
					pTargetZ: 0.0f
				);
				Traits01Actions.AngryAura(pTarget, pTile);
			}
			float spreadDistance_00 = 5.0f;
			float spreadAngle_00 = 360.0f;
			float totalSpread_00 = numberOfProjectiles_00 > 0 ? (spreadAngle_00 / numberOfProjectiles_00) : 0f;
			for (int i = 0; i < numberOfProjectiles_00; i++)
			{
				float angle = totalSpread_00 * i - spreadAngle_00 / 2f;
				UnityEngine.Vector3 launchPosition = baseLaunchPoint + Quaternion.Euler(0f, 0f, angle) * new UnityEngine.Vector3(spreadDistance_00, 0, 0);

				World.world.projectiles.spawn(
					pInitiator: selfActor,
					pTargetObject: pTarget,
					pAssetID: "HighRedOrbl",
					pLaunchPosition: launchPosition,
					pTargetPosition: targetPosition,
					pTargetZ: 0.0f
				);
				Traits01Actions.AngryAura(pTarget, pTile);
			}
			return true;
		}
		public static bool EvilGlovesStrike_Counterattack01(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 魔拳 子彈反擊 item_cdt01 (瘋狂/通常)
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive() || pTarget == null || !pTarget.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			
			// 檢查目標是否為敵對單位或建築
			if (pTarget.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(pTarget.kingdom))
			{
				return false;
			}
			
			// === 狀態檢查 ===
			if (!selfActor.hasStatus("wrath_demon_king"))
			{
				return false;
			}
			string attackCooldownStatus = "item_cdt01";
			float attackCooldownDuration = 0.01f;
			if (selfActor.hasStatus(attackCooldownStatus))
			{
				return false;
			}
			
			// === 根據 'madness' 特質設定子彈數量 ===
			int numberOfProjectiles;
			int numberOfProjectiles_00;
			if (selfActor.hasTrait("madness"))
			{
				numberOfProjectiles = 3;
				numberOfProjectiles_00 = 15;
			}
			else
			{
				numberOfProjectiles = 6;
				numberOfProjectiles_00 = 30;
			}
			
			// === 成功找到目標，施加冷卻並發射投射物 ===
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			//selfActor.addStatusEffect("invincible", 0.05f);
			selfActor.addStatusEffect("crosshair", 1.05f);
			pTarget.addStatusEffect("angry", 300f);
			pTarget.finishStatusEffect("invincible");

			UnityEngine.Vector3 selfPosition = selfActor.current_position;
			UnityEngine.Vector3 targetPosition = pTarget.current_position;
			UnityEngine.Vector3 baseLaunchPoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, targetPosition.x, targetPosition.y, selfActor.stats["size"]);
			baseLaunchPoint.y += 0.5f;

			float spreadDistance = 5.0f;
			float spreadAngle = 360.0f;
			float totalSpread = numberOfProjectiles > 0 ? (spreadAngle / numberOfProjectiles) : 0f;
			for (int i = 0; i < numberOfProjectiles; i++)
			{
				float angle = totalSpread * i - spreadAngle / 2f;
				UnityEngine.Vector3 launchPosition = baseLaunchPoint + Quaternion.Euler(0f, 0f, angle) * new UnityEngine.Vector3(spreadDistance, 0, 0);

				World.world.projectiles.spawn(
					pInitiator: selfActor,
					pTargetObject: pTarget,
					pAssetID: "HighFireball",
					pLaunchPosition: launchPosition,
					pTargetPosition: targetPosition,
					pTargetZ: 0.0f
				);
				Traits01Actions.AngryAura(pTarget, pTile);
			}
			float spreadDistance_00 = 5.0f;
			float spreadAngle_00 = 360.0f;
			float totalSpread_00 = numberOfProjectiles_00 > 0 ? (spreadAngle_00 / numberOfProjectiles_00) : 0f;
			for (int i = 0; i < numberOfProjectiles_00; i++)
			{
				float angle = totalSpread_00 * i - spreadAngle_00 / 2f;
				UnityEngine.Vector3 launchPosition = baseLaunchPoint + Quaternion.Euler(0f, 0f, angle) * new UnityEngine.Vector3(spreadDistance_00, 0, 0);

				World.world.projectiles.spawn(
					pInitiator: selfActor,
					pTargetObject: pTarget,
					pAssetID: "HighFireball",
					pLaunchPosition: launchPosition,
					pTargetPosition: targetPosition,
					pTargetZ: 0.0f
				);
				Traits01Actions.AngryAura(pTarget, pTile);
			}
			return true;
		}
		public static bool EvilGlovesStrike_Counterattack02(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 魔拳 子彈反擊 item_cdt02 第一終結 The Last Sun
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive() || pTarget == null || !pTarget.isAlive())
				return false;
			Actor selfActor = pSelf.a;

			// 檢查目標是否為敵對單位或建築
			if (pTarget.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(pTarget.kingdom))
			{
				return false;
			}
			
			// === 狀態與冷卻檢查 ===
			string attackCooldownStatus = "item_cdt02";
			float attackCooldownDuration = 3600.0f;
			if (!selfActor.hasStatus("wrath_demon_king"))
			{
				return false;
			}
			if (selfActor.hasStatus(attackCooldownStatus))
			{
				return false;
			}
			
			// === 額外的發動機率檢查 ===
			if (!Randy.randomChance(0.01f))
			{
				return false;
			}
			
			// === 成功找到目標，施加冷卻並發射投射物 ===
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			//selfActor.addStatusEffect("invincible", 3.05f);
			pTarget.addStatusEffect("angry", 300f);
			pTarget.finishStatusEffect("invincible");


			
			UnityEngine.Vector3 selfPosition = selfActor.current_position;
			UnityEngine.Vector3 targetPosition = pTarget.current_position;
			float targetSize = 1f;
			// 由於 pTarget 現在可以是 BaseSimObject，需要判斷其類型來獲取大小
			if (pTarget.isActor() && pTarget.a.stats != null)
			{
				targetSize = pTarget.a.stats["size"];
			}
			
			UnityEngine.Vector3 basePoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, targetPosition.x, targetPosition.y, selfActor.stats["size"]);
			UnityEngine.Vector3 targetPoint = Toolbox.getNewPoint(targetPosition.x, targetPosition.y, targetPosition.x, targetPosition.y, targetSize, true);

			int numberOfProjectiles_01 = 20;
			float spreadAngle_01 = 0.0f;
			for (int i = 0; i < numberOfProjectiles_01; i++)
			{
				UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, spreadAngle_01 * (i - (numberOfProjectiles_01 - 1f) / 2f));
				UnityEngine.Vector3 spreadTarget = rotation * (targetPoint - basePoint) + basePoint;
				World.world.projectiles.spawn(
					pInitiator: selfActor,
					pTargetObject: pTarget,
					pAssetID: "NuclearFusion1",
					pLaunchPosition: basePoint,
					pTargetPosition: spreadTarget,
					pTargetZ: 0.0f
				);
			}
			int numberOfProjectiles_02 = 20;
			float spreadAngle_02 = 0.0f;
			for (int i = 0; i < numberOfProjectiles_02; i++)
			{
				UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, spreadAngle_02 * (i - (numberOfProjectiles_02 - 1f) / 2f));
				UnityEngine.Vector3 spreadTarget = rotation * (targetPoint - basePoint) + basePoint;
				World.world.projectiles.spawn(
					pInitiator: selfActor,
					pTargetObject: pTarget,
					pAssetID: "NuclearFusion2",
					pLaunchPosition: basePoint,
					pTargetPosition: spreadTarget,
					pTargetZ: 0.0f
				);
			}
			int numberOfProjectiles_03 = 20;
			float spreadAngle_03 = 0.0f;
			for (int i = 0; i < numberOfProjectiles_03; i++)
			{
				UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, spreadAngle_03 * (i - (numberOfProjectiles_03 - 1f) / 2f));
				UnityEngine.Vector3 spreadTarget = rotation * (targetPoint - basePoint) + basePoint;
				World.world.projectiles.spawn(
					pInitiator: selfActor,
					pTargetObject: pTarget,
					pAssetID: "NuclearFusion3",
					pLaunchPosition: basePoint,
					pTargetPosition: spreadTarget,
					pTargetZ: 0.0f
				);
			}
			return true;
		}
		public static bool EvilGlovesStrike_ArmedAttack01(BaseSimObject pSelf, WorldTile pTile = null)
		{// 魔拳 常態攻擊 item_cdt03 (瘋狂/通常)
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			if (!selfActor.hasStatus("wrath_demon_king"))
			{
				return false;
			}
			string attackCooldownStatus = "item_cdt03";
			float attackCooldownDuration = 0.01f;
			if (selfActor.hasStatus(attackCooldownStatus))
			{
				return false;
			}
			
			// === 核心邏輯：根據周圍敵人的數量決定是否發動 (預設至少1個敵人) ===
			float maxRange = 30f;
			int requiredEnemies = 1;
			int enemyCount = 0;
			BaseSimObject mainTarget = null;
			float closestDist = float.MaxValue;
			
			// 遍歷所有單位和建築物來計數和尋找主目標
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive() || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist <= maxRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = other;
					}
				}
			}
			foreach (var building in World.world.buildings)
			{
				if (building == null || !building.isAlive() || !selfActor.kingdom.isEnemy(building.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, building.current_position);
				if (dist <= maxRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = building;
					}
				}
			}

			if (enemyCount < requiredEnemies || mainTarget == null)
			{
				return false;
			}
			
			// === 成功找到目標，施加冷卻並發射投射物 ===
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			//selfActor.addStatusEffect("invincible", 0.05f);
			mainTarget.finishStatusEffect("invincible");
			mainTarget.addStatusEffect("angry", 300f);
			selfActor.addStatusEffect("crosshair", 1.05f);

			UnityEngine.Vector3 selfPosition = selfActor.current_position;
			UnityEngine.Vector3 targetPosition = mainTarget.current_position;
			UnityEngine.Vector3 baseLaunchPoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, targetPosition.x, targetPosition.y, selfActor.stats["size"]);
			baseLaunchPoint.y += 0.5f;

			if (selfActor.hasTrait("madness"))
			{
				int numberOfProjectiles = 6;
				float spreadDistance = 5.0f;
				float spreadAngle = 360.0f;
				float totalSpread = spreadAngle / numberOfProjectiles;
				for (int i = 0; i < numberOfProjectiles; i++)
				{
					float angle = totalSpread * i - spreadAngle / 2f;
					UnityEngine.Vector3 launchPosition = baseLaunchPoint + Quaternion.Euler(0f, 0f, angle) * new Vector3(spreadDistance, 0, 0);
					World.world.projectiles.spawn(
						pInitiator: selfActor,
						pTargetObject: mainTarget,
						pAssetID: "HighFireball",
						pLaunchPosition: launchPosition,
						pTargetPosition: targetPosition,
						pTargetZ: 0.0f
					);
				}
				int numberOfProjectiles_00 = 30;
				float spreadDistance_00 = 5.0f;
				float spreadAngle_00 = 360.0f;
				float totalSpread_00 = spreadAngle_00 / numberOfProjectiles_00;
				for (int i = 0; i < numberOfProjectiles_00; i++)
				{
					float angle = totalSpread_00 * i - spreadAngle_00 / 2f;
					UnityEngine.Vector3 launchPosition = baseLaunchPoint + Quaternion.Euler(0f, 0f, angle) * new UnityEngine.Vector3(spreadDistance_00, 0, 0);
					World.world.projectiles.spawn(
						pInitiator: selfActor,
						pTargetObject: mainTarget,
						pAssetID: "HighRedOrbl",
						pLaunchPosition: launchPosition,
						pTargetPosition: targetPosition,
						pTargetZ: 0.0f
					);
				}
			}
			else
			{
				int numberOfProjectiles_00 = 12;
				float spreadDistance_00 = 5.0f;
				float spreadAngle_00 = 360.0f;
				float totalSpread_00 = spreadAngle_00 / numberOfProjectiles_00;
				for (int i = 0; i < numberOfProjectiles_00; i++)
				{
					float angle = totalSpread_00 * i - spreadAngle_00 / 2f;
					UnityEngine.Vector3 launchPosition = baseLaunchPoint + Quaternion.Euler(0f, 0f, angle) * new Vector3(spreadDistance_00, 0, 0);
					World.world.projectiles.spawn(
						pInitiator: selfActor,
						pTargetObject: mainTarget,
						pAssetID: "HighFireball",
						pLaunchPosition: launchPosition,
						pTargetPosition: targetPosition,
						pTargetZ: 0.0f
					);
				}
				int numberOfProjectiles_01 = 60;
				float spreadDistance_01 = 5.5f;
				float spreadAngle_01 = 360.0f;
				float totalSpread_01 = spreadAngle_01 / numberOfProjectiles_01;
				for (int i = 0; i < numberOfProjectiles_01; i++)
				{
					float angle = totalSpread_01 * i - spreadAngle_01 / 2f;
					UnityEngine.Vector3 launchPosition = baseLaunchPoint + Quaternion.Euler(0f, 0f, angle) * new UnityEngine.Vector3(spreadDistance_01, 0, 0);
					World.world.projectiles.spawn(
						pInitiator: selfActor,
						pTargetObject: mainTarget,
						pAssetID: "HighRedOrbl",
						pLaunchPosition: launchPosition,
						pTargetPosition: targetPosition,
						pTargetZ: 0.0f
					);
				}
			}
			return true;
		}
		public static bool EvilGlovesStrike_ArmedAttack02(BaseSimObject pSelf, WorldTile pTile = null)
		{// 魔拳 常態攻擊 item_cdt02 第一終結 The Last Sun
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			
			// === 檢查：必須擁有 'wrath_demon_king' 狀態才能發動 ===
			if (!selfActor.hasStatus("wrath_demon_king"))
			{
				return false;
			}
			// === 冷卻狀態檢查 ===
			string attackCooldownStatus = "item_cdt02";
			float attackCooldownDuration = 3600f;
			float randomChance = 0.01f;
			if (selfActor.hasStatus(attackCooldownStatus))
			{
				return false;
			}

			// === 核心邏輯：根據周圍敵人的數量決定是否發動 (預設至少1個敵人) ===
			float maxRange = 25f;
			int requiredEnemies = 1;
			int enemyCount = 0;
			BaseSimObject mainTarget = null;
			float closestDist = float.MaxValue;
			
			// 遍歷所有單位和建築物來計數和尋找主目標
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive() || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist <= maxRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = other;
					}
				}
			}
			foreach (var building in World.world.buildings)
			{
				if (building == null || !building.isAlive() || !selfActor.kingdom.isEnemy(building.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, building.current_position);
				if (dist <= maxRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = building;
					}
				}
			}
			
			// 在檢查發動機率前，先檢查是否有足夠的敵人
			if (enemyCount < requiredEnemies || mainTarget == null)
			{
				return false;
			}
			
			// === 發動機率檢查 ===
			if (!Randy.randomChance(randomChance))
			{
				return false;
			}

			// === 成功找到目標，施加冷卻並發射投射物 ===
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			//selfActor.addStatusEffect("invincible", 3.00f);
			mainTarget.finishStatusEffect("invincible");
			mainTarget.addStatusEffect("angry", 300f);

			UnityEngine.Vector3 selfPosition = selfActor.current_position;
			UnityEngine.Vector3 targetPosition = mainTarget.current_position;
			float targetSize = 1.0f;
			if (mainTarget.isActor() && mainTarget.a.stats != null)
			{
				targetSize = mainTarget.a.stats["size"];
			}
			
			UnityEngine.Vector3 basePoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, targetPosition.x, targetPosition.y, selfActor.stats["size"]);
			basePoint.y += 5.5f;
			
			int totalProjectiles = 60;
			float randomSpread = 1.0f;
			for (int i = 0; i < totalProjectiles; i++)
			{
				string projectileAssetID;
				if (i < 20)
				{
					projectileAssetID = "NuclearFusion1";
				}
				else if (i < 40)
				{
					projectileAssetID = "NuclearFusion2";
				}
				else
				{
					projectileAssetID = "NuclearFusion3";
				}
				float offsetX = (float)Randy.rnd.NextDouble() * randomSpread - randomSpread / 2f;
				float offsetY = (float)Randy.rnd.NextDouble() * randomSpread - randomSpread / 2f;
				UnityEngine.Vector3 adjustedBasePoint = basePoint + new UnityEngine.Vector3(offsetX, offsetY, 0f);
				UnityEngine.Vector3 spreadTarget = targetPosition;
			
				World.world.projectiles.spawn(
					pInitiator: selfActor,
					pTargetObject: mainTarget,
					pAssetID: projectileAssetID,
					pLaunchPosition: adjustedBasePoint,
					pTargetPosition: spreadTarget,
					pTargetZ: 0.0f
				);
			}
			return true;
		}
		public static bool EvilGlovesStrike_FinishingAttack(BaseSimObject pSelf, WorldTile pTile = null)
		{// 魔拳 常態攻擊 item_cdt04 第二終結 Angry Eclipse
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			if (!selfActor.hasStatus("wrath_demon_king"))
			{
				return false;
			}
			if (selfActor.hasTrait("madness"))
			{
				return false;
			}
			// 機率檢查
			if (!Randy.randomChance(0.05f))
			{
				return false;
			}
			// === 冷卻狀態檢查 ===
			string attackCooldownStatus = "item_cdt04";
			float attackCooldownDuration = 3600.00f;
			if (selfActor.hasStatus(attackCooldownStatus))
			{
				return false;
			}
			// === 新增：根據周圍敵人的數量決定是否發動 ===
			float detectionRange = 35f;
			int requiredEnemies = 3;
			int enemyCount = 0;
			BaseSimObject mainTarget = null;
			float closestDist = float.MaxValue;
			foreach (var other in World.world.units)
			{//對單位
				if (other == null || other == selfActor || !other.isAlive() || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist <= detectionRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = other;
					}
				}
			}
			foreach (var building in World.world.buildings)
			{//對建築
				if (building == null || !building.isAlive() || !selfActor.kingdom.isEnemy(building.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, building.current_position);
				if (dist <= detectionRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = building;
					}
				}
			}
			if (enemyCount < requiredEnemies || mainTarget == null)
			{
				return false;
			}
			// === 成功找到目標，施加冷卻並發射投射物 ===
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			//selfActor.addStatusEffect("invincible", 0.05f);
			mainTarget.finishStatusEffect("invincible");
			mainTarget.addStatusEffect("angry", 300f);

			UnityEngine.Vector3 targetPosition = mainTarget.current_position;
			var projectilesToSpawn = new (string id, float radius)[]
			{
				("HighFireball_2", 45.0f), ("HighFireball_2", 45.2f),
				("HighFireball_2", 45.4f), ("HighFireball_2", 45.6f),
				("HighFireball_2", 45.8f), ("HighFireball_2", 46.0f)
			};
			foreach (var projectile in projectilesToSpawn)
			{
				int numberOfProjectiles = 360;
				float totalAngle = 360.0f;
				float totalAnglePerProjectile = totalAngle / numberOfProjectiles;
				for (int i = 0; i < numberOfProjectiles; i++)
				{
					float launchAngle = totalAnglePerProjectile * i;
					float launchAngleRad = launchAngle * Mathf.Deg2Rad;
					UnityEngine.Vector3 launchPosition = new UnityEngine.Vector3(
						targetPosition.x + projectile.radius * Mathf.Cos(launchAngleRad),
						targetPosition.y + projectile.radius * Mathf.Sin(launchAngleRad),
						targetPosition.z
					);
					World.world.projectiles.spawn(
						pInitiator: selfActor,
						pTargetObject: mainTarget,
						pAssetID: projectile.id,
						pLaunchPosition: launchPosition,
						pTargetPosition: targetPosition,
						pTargetZ: 0.0f
					);
				}
			}
			return true;
		}
		public static bool EvilGlovesAwakens(BaseSimObject pSelf, WorldTile pTile = null)
		{// 魔拳 復甦
			// 1. 基本安全檢查：確保 pSelf 及其 Actor 組件存在且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			// 如果單位持有 "arrogant_demon_king" 狀態
			if (selfActor.hasStatus("wrath_demon_king"))
			{
				// 則為其持續添加 "adk2" 狀態，每次添加時設定持續時間為 10 秒
				// 如果 adk2 已經存在，此操作會刷新其持續時間為 10 秒
				selfActor.addStatusEffect("wdk2", 3600f); // 將時間改為 10 秒
			}
			return false; // 特殊效果通常返回 false，表示不阻止其他效果鏈
		}
		public static bool EvilLawGet04(BaseSimObject pTarget, WorldTile pTile)
		{// 魔拳 給予拾獲者認證狀態
			// 1. 安全檢查：確保 pTarget 存在且有 Actor 組件
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor targetActor = pTarget.a;
			// =======================================================
			// === 新增/修改邏輯：檢查王國內是否已存在特定魔王 ===
			const string DemonKingStatusID = "wrath_demon_king";
			Kingdom currentKingdom = targetActor.kingdom;
			// 檢查目標單位是否屬於一個國家
			if (currentKingdom != null)
			{
				// 遍歷王國中的所有單位
				foreach (Actor kingdomUnit in currentKingdom.units)
				{
					// 跳過自己，只檢查其他單位
					if (kingdomUnit == null || kingdomUnit == targetActor)
					{
						continue;
					}
					// 檢查其他單位是否擁有 "???_demon_king" 狀態
					if (kingdomUnit.hasStatus(DemonKingStatusID))
					{
						// 如果找到，則直接返回 false，不執行後續操作
						return false;
					}
				}
			}
			// =======================================================
			// 2. 如果通過了上面的檢查，繼續執行後續邏輯
			// 檢查是否持有 特質
			if (targetActor.hasTrait("evillaw_tantrum"))
			{
				// 如果單位已經有 特質，就不進行任何動作
				return true; 
			}
			// 3. 如果沒有 特質，則檢查是否持有 狀態效果
			else if (!targetActor.hasStatus("wdk3"))
			{
				// 如果都沒有，就添加 狀態效果
				targetActor.addStatusEffect("wdk3", 600f);
			}
			return true; // 表示操作成功執行
		}
			#endregion
			#region 暴食餐具
	//槍
		public static bool Anti_Hungry(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 魔槍 餓者殺手
			// 安全檢查：確保施法者和目標都存在且有效
			if (pSelf?.a == null || pTarget?.a == null || !pSelf.a.isAlive() || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			Actor targetActor = pTarget.a;
/*			if (targetActor.hasTrait("holyarts_justice"))
			{// 如果目標持有 特質 則返回
				return false;
			}*/
			// 檢查目標種族是否與施法者不同
			if (targetActor.data.nutrition < selfActor.data.nutrition || !targetActor.subspecies.hasTrait("stomach"))
			{
				// 取得施法者的傷害值
				float selfDamage = selfActor.stats["damage"];
				// 施加額外傷害 (例如額外造成 50% 傷害)
				targetActor.getHit(selfDamage * 0.50f, true, AttackType.Weapon, selfActor, false, true, false);
				return true;
			}
			return false;
		}
		public static bool Anti_Hungry_Defense(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 魔槍 餓者特防
			// 1. 安全檢查：確保施法者 (防禦者) 和目標 (攻擊者) 都存在且有效
			if (pSelf?.a == null || pTarget?.a == null || !pSelf.a.isAlive() || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a; // 防禦者
			Actor targetActor = pTarget.a; // 攻擊者
			if (!selfActor.hasStatus("gluttony_demon_king") || !selfActor.hasStatus("defense_on"))
			{
				return false;
			}
			if (targetActor.data.nutrition < selfActor.data.nutrition || !targetActor.subspecies.hasTrait("stomach"))
			{				
				// 取得攻擊者對防禦者造成的傷害值
				float damageDealt = targetActor.stats["damage"];
				// 你可以根據需要調整這個百分比
				float damageToHeal = damageDealt * 0.75f;
				// 直接增加生命值，以抵銷受到的傷害
				selfActor.data.health += (int)damageToHeal;
				// 確保生命值不會超過最大值
				if (selfActor.data.health > selfActor.getMaxHealth())
				{
					selfActor.data.health = selfActor.getMaxHealth();
				}
				return true;
			}
			return false;
		}
		public static bool EvilSpearCounterattack(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 魔槍 轉接 反擊技
			// 1. 基礎安全檢查
			if (pSelf == null || pTarget == null)
			{
				return false;
			}
			
			// 2. 隨機分配攻擊
			// 隨機選擇要觸發的攻擊效果
			float randomValue = UnityEngine.Random.value; // 獲取一個 0 到 1 的隨機值
			if (randomValue <= 0.95f)
			{
				// 95% 機率觸發 Fork_Throwing
				return Fork_Throwing(pSelf, pTarget, pTile);
			}
			else
			{
				// 另外 5% 機率觸發 Shock_protection
				return Shock_protection(pSelf, pTarget, pTile);
			}
		}
		public static bool Fork_Throwing(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 魔槍 餐叉投擲(反擊) 消費 3 營養
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			string requiredStatus = "gluttony_demon_king";
			const int Using_Nutrition = 3;

			// 檢查攻擊者是否擁有必要狀態和足夠營養
			if (!selfActor.hasStatus(requiredStatus) || selfActor.data.nutrition < Using_Nutrition)
			{
				return false;
			}

			// === 冷卻狀態檢查邏輯 ===
			string attackCooldownStatus = "item_cdt00";
			float attackCooldownDuration = 0.01f;
			if (selfActor.hasStatus(attackCooldownStatus))
			{
				return false;
			}

			// === 核心邏輯：根據周圍敵人的數量決定是否發動 ===
			float detectionRange = 25f; // 你可以根據需要調整這個範圍
			int requiredEnemies = 1;
			int enemyCount = 0;
			BaseSimObject mainTarget = null;
			float closestDist = float.MaxValue;
			
			// 遍歷所有單位和建築物來計數和尋找主目標
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive() || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist <= detectionRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = other;
					}
				}
			}
			foreach (var building in World.world.buildings)
			{
				if (building == null || !building.isAlive() || !selfActor.kingdom.isEnemy(building.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, building.current_position);
				if (dist <= detectionRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = building;
					}
				}
			}

			// 只有當敵人數達標且找到主目標時，才發動攻擊
			if (enemyCount < requiredEnemies || mainTarget == null)
			{
				return false;
			}

			// === 滿足條件，扣除營養並發射投射物 ===
			selfActor.data.nutrition -= Using_Nutrition;
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			//selfActor.addStatusEffect("invincible", 0.05f);

			UnityEngine.Vector3 selfPosition = selfActor.current_position;
			UnityEngine.Vector3 targetPosition = mainTarget.current_position;
			float targetSize = 1f;
			if (mainTarget.isActor() && mainTarget.a.stats != null)
			{
				targetSize = mainTarget.a.stats["size"];
			}
			
			UnityEngine.Vector3 basePoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, targetPosition.x, targetPosition.y, selfActor.stats["size"]);
			UnityEngine.Vector3 targetPoint = Toolbox.getNewPoint(targetPosition.x, targetPosition.y, targetPosition.x, targetPosition.y, targetSize, true);

			int numberOfProjectiles = 6;
			float spreadAngle = 1.0f;
			for (int i = 0; i < numberOfProjectiles; i++)
			{
				UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, spreadAngle * (i - (numberOfProjectiles - 1f) / 2f));
				UnityEngine.Vector3 spreadTarget = rotation * (targetPoint - basePoint) + basePoint;
				
				World.world.projectiles.spawn(
					pInitiator: selfActor,
					pTargetObject: mainTarget,
					pAssetID: "Fork",
					pLaunchPosition: basePoint,
					pTargetPosition: spreadTarget,
					pTargetZ: 0.0f
				);
			}

			return true;
		}
		public static bool Shock_protection(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 魔槍 震盪防護(反擊) 消費 5 營養
			// 1. 安全檢查：確保發動者、目標和地塊資訊都存在。
			if (pSelf == null || pTarget == null || pTile == null)
			{
				return false;
			}
			// 將 pSelf 轉換為 Actor 類型，並進行安全檢查
			Actor selfActor = pSelf.a;
			if (selfActor == null)
			{
				// 如果 pSelf 不是一個 Actor，則無法繼續
				return false;
			}
			if (!selfActor.hasStatus("gluttony_demon_king"))
			{
				return false;
			}
			string attackCooldownStatus = "item_cdt00";
			float attackCooldownDuration = 0.01f;
			const int Using_Nutrition = 5;
			// 現在可以使用 selfActor.data 來訪問屬性
			if (selfActor.hasStatus(attackCooldownStatus))
			{
				return false;
			}
			if (selfActor.data.nutrition < Using_Nutrition)
			{
				return false;
			}
			selfActor.data.nutrition -= Using_Nutrition;
			// 2. 施加爆炸波紋與震盪效果。
			// 參數：中心地塊, 震盪半徑(3), 力量大小(0.3f), 影響地面(true), 其他參數
			World.world.applyForceOnTile(pTile, 100, 2.50f, true, 0, null, pTarget, null, false);
			
			// 參數：位置, 視覺半徑(3f), 持續時間(5.0f)
			EffectsLibrary.spawnExplosionWave(pTile.posV3, 3f, 5.00f);
			EffectsLibrary.spawnExplosionWave(pTile.posV3, 6f, 2.50f);
			EffectsLibrary.spawnExplosionWave(pTile.posV3, 9f, 0.99f);
			// 3. 回傳 true 表示攻擊成功發動。
			return true;
		}
		public static bool EvilSpearThrowing01(BaseSimObject pSelf, WorldTile pTile = null)
		{// 魔槍 高速酸彈 常態攻擊 消費 3 營養
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			
			// === 狀態與資源檢查 ===
			if (!selfActor.hasStatus("gluttony_demon_king"))
			{
				return false;
			}
			const int Using_Nutrition = 2;
			if (selfActor.data.nutrition < Using_Nutrition)
			{
				return false;
			}
			
			// === 冷卻狀態檢查 ===
			string attackCooldownStatus = "item_cdt01";
			float attackCooldownDuration = 0.1f;
			if (selfActor.hasStatus(attackCooldownStatus))
			{
				return false;
			}
			
			// === 核心邏輯：根據周圍敵人的數量決定是否發動 (預設至少1個敵人) ===
			float maxRange = 35f;
			int requiredEnemies = 1;
			int enemyCount = 0;
			BaseSimObject mainTarget = null;
			float closestDist = float.MaxValue;
			
			// 遍歷所有單位和建築物來計數和尋找主目標
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive() || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist <= maxRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = other;
					}
				}
			}
			foreach (var building in World.world.buildings)
			{
				if (building == null || !building.isAlive() || !selfActor.kingdom.isEnemy(building.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, building.current_position);
				if (dist <= maxRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = building;
					}
				}
			}
			
			if (enemyCount < requiredEnemies || mainTarget == null)
			{
				return false;
			}

			// === 成功找到目標，扣除營養並施加冷卻 ===
			selfActor.data.nutrition -= Using_Nutrition;
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			//selfActor.addStatusEffect("invincible", 0.05f);

			// === 發射投射物 ===
			UnityEngine.Vector3 selfPosition = selfActor.current_position;
			UnityEngine.Vector3 targetPosition = mainTarget.current_position;
			UnityEngine.Vector3 basePoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, targetPosition.x, targetPosition.y, selfActor.stats["size"]);
			basePoint.y += 0.5f;
			
			int numberOfProjectiles_00 = 1;
			float spreadAngle_00 = 0.0f;
			for (int i = 0; i < numberOfProjectiles_00; i++)
			{
				UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, spreadAngle_00 * (i - (numberOfProjectiles_00 - 1f) / 2f));
				UnityEngine.Vector3 spreadTarget = rotation * (targetPosition - basePoint) + basePoint;
				
				World.world.projectiles.spawn(
					pInitiator: selfActor,
					pTargetObject: mainTarget,
					pAssetID: "HighAcidBall",
					pLaunchPosition: basePoint,
					pTargetPosition: spreadTarget,
					pTargetZ: 0.0f
				);
			}
			
			return true;
		}
		public static bool EvilSpearThrowingXX(BaseSimObject pSelf, WorldTile pTile = null)
		{// 魔槍 中轉連接 The Demon King's Dining Table (EvilSpearThrowing 02 03 合集)
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			if (!selfActor.hasStatus("gluttony_demon_king"))
			{
				return false;
			}
			// 隨機選擇要觸發的攻擊效果
			float randomValue = UnityEngine.Random.value; // 獲取一個 0 到 1 的隨機值
			// 40% 的機率觸發 EvilSpearThrowing02 (扇狀霰射)
			if (randomValue <= 0.50f)
			{
				return EvilSpearThrowing02(pSelf, pTile);
			}
			// 30% 的機率觸發 EvilSpearThrowing03 (環狀集火)
			else
			{
				return EvilSpearThrowing03(pSelf, pTile);
			}
		}
		public static bool EvilSpearThrowing02(BaseSimObject pSelf, WorldTile pTile = null)
		{// 魔槍 高速餐刀 常態攻擊 (扇狀霰射) 消費 20 營養
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			
			// === 狀態與資源檢查 ===
			if (!selfActor.hasStatus("gluttony_demon_king"))
			{
				return false;
			}
			const int Using_Nutrition = 20;
			if (selfActor.data.nutrition < Using_Nutrition)
			{
				return false;
			}
			
			// === 冷卻狀態檢查 ===
			string attackCooldownStatus = "item_cdt02";
			float attackCooldownDuration = 0.01f;
			if (selfActor.hasStatus(attackCooldownStatus))
			{
				return false;
			}
			
			// === 核心邏輯：根據周圍敵人的數量決定是否發動 (預設至少3個敵人) ===
			float maxRange = 15f;
			int requiredEnemies = 3;
			int enemyCount = 0;
			BaseSimObject mainTarget = null;
			float closestDist = float.MaxValue;
			
			// 遍歷所有單位和建築物來計數和尋找主目標
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive() || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist <= maxRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = other;
					}
				}
			}
			foreach (var building in World.world.buildings)
			{
				if (building == null || !building.isAlive() || !selfActor.kingdom.isEnemy(building.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, building.current_position);
				if (dist <= maxRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = building;
					}
				}
			}
			
			if (enemyCount < requiredEnemies || mainTarget == null)
			{
				return false;
			}
			
			// === 成功找到目標，扣除營養並施加冷卻 ===
			selfActor.data.nutrition -= Using_Nutrition;
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			//selfActor.addStatusEffect("invincible", 0.05f);
			
			UnityEngine.Vector3 selfPosition = selfActor.current_position;
			UnityEngine.Vector3 targetPosition = mainTarget.current_position;
			UnityEngine.Vector3 basePoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, targetPosition.x, targetPosition.y, selfActor.stats["size"]);
			basePoint.y += 0.5f;
			
			int numberOfProjectiles_00 = 90;
			float spreadAngle_00 = 1.0f;
			for (int i = 0; i < numberOfProjectiles_00; i++)
			{
				UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, spreadAngle_00 * (i - (numberOfProjectiles_00 - 1f) / 2f));
				UnityEngine.Vector3 spreadTarget = rotation * (targetPosition - basePoint) + basePoint;
				
				World.world.projectiles.spawn(
					pInitiator: selfActor,
					pTargetObject: mainTarget,
					pAssetID: "Knife",
					pLaunchPosition: basePoint,
					pTargetPosition: spreadTarget,
					pTargetZ: 0.0f
				);
			}
			return true;
		}
		public static bool EvilSpearThrowing03(BaseSimObject pSelf, WorldTile pTile = null)
		{// 魔槍 高速餐叉 常態攻擊 (環狀集火) 消費 20 營養
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			
			// === 狀態與資源檢查 ===
			if (!selfActor.hasStatus("gluttony_demon_king"))
			{
				return false;
			}
			const int Using_Nutrition = 20;
			if (selfActor.data.nutrition < Using_Nutrition)
			{
				return false;
			}
			
			// === 冷卻狀態檢查 ===
			string attackCooldownStatus = "item_cdt02";
			float attackCooldownDuration = 0.01f;
			if (selfActor.hasStatus(attackCooldownStatus))
			{
				return false;
			}
			
			// === 核心邏輯：根據周圍敵人的數量決定是否發動 (預設至少3個敵人) ===
			float maxRange = 15f;
			int requiredEnemies = 1;
			int enemyCount = 0;
			BaseSimObject mainTarget = null;
			float closestDist = float.MaxValue;
			
			// 遍歷所有單位和建築物來計數和尋找主目標
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive() || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist <= maxRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = other;
					}
				}
			}
			foreach (var building in World.world.buildings)
			{
				if (building == null || !building.isAlive() || !selfActor.kingdom.isEnemy(building.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, building.current_position);
				if (dist <= maxRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = building;
					}
				}
			}
			
			if (enemyCount < requiredEnemies || mainTarget == null)
			{
				return false;
			}
			
			// === 成功找到目標，扣除營養並施加冷卻 ===
			selfActor.data.nutrition -= Using_Nutrition;
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			//selfActor.addStatusEffect("invincible", 0.05f);
			
			UnityEngine.Vector3 selfPosition = selfActor.current_position;
			UnityEngine.Vector3 targetPosition = mainTarget.current_position;
			
			UnityEngine.Vector3 baseLaunchPoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, targetPosition.x, targetPosition.y, selfActor.stats["size"]);
			baseLaunchPoint.y += 0.5f;
			
			int numberOfProjectiles_00 = 180;
			float spreadDistance_00 = 5.0f;
			float spreadAngle_00 = 360.0f;
			float totalSpread = spreadAngle_00 / numberOfProjectiles_00;
			for (int i = 0; i < numberOfProjectiles_00; i++)
			{
				float angle = totalSpread * i - spreadAngle_00 / 2f;
				UnityEngine.Vector3 launchPosition = baseLaunchPoint + Quaternion.Euler(0f, 0f, angle) * new UnityEngine.Vector3(spreadDistance_00, 0, 0);
				World.world.projectiles.spawn(
					pInitiator: selfActor,
					pTargetObject: mainTarget,
					pAssetID: "Fork",
					pLaunchPosition: launchPosition,
					pTargetPosition: targetPosition,
					pTargetZ: 0.0f
				);
			}
			return true;
		}
		public static bool EvilSpearThrowing04(BaseSimObject pSelf, WorldTile pTile = null)
		{// 魔槍 高速刀叉 常態攻擊 (包圍收縮) 消費 60 營養
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			
			// === 狀態與資源檢查 ===
			if (!selfActor.hasStatus("gluttony_demon_king"))
			{
				return false;
			}
			const int Using_Nutrition = 60;
			if (selfActor.data.nutrition < Using_Nutrition)
			{
				return false;
			}
			
			// === 冷卻狀態檢查 ===
			string attackCooldownStatus = "item_cdt03";
			float attackCooldownDuration = 600.00f;
			if (selfActor.hasStatus(attackCooldownStatus))
			{
				return false;
			}
			
			// === 核心邏輯：根據周圍敵人的數量決定是否發動 (預設至少3個敵人) ===
			float maxRange = 15f;
			int requiredEnemies = 3;
			int enemyCount = 0;
			BaseSimObject mainTarget = null;
			float closestDist = float.MaxValue;
			
			// 遍歷所有單位和建築物來計數和尋找主目標
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive() || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist <= maxRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = other;
					}
				}
			}
			foreach (var building in World.world.buildings)
			{
				if (building == null || !building.isAlive() || !selfActor.kingdom.isEnemy(building.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, building.current_position);
				if (dist <= maxRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = building;
					}
				}
			}

			// 在檢查發動機率前，先檢查是否有足夠的敵人
			if (enemyCount < requiredEnemies || mainTarget == null)
			{
				return false;
			}

			// === 額外的發動機率檢查 ===
			if (!Randy.randomChance(0.025f))
			{
				return false;
			}

			// === 成功找到目標，扣除營養並施加冷卻 ===
			selfActor.data.nutrition -= Using_Nutrition;
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			//selfActor.addStatusEffect("invincible", 0.05f);

			UnityEngine.Vector3 targetPosition = mainTarget.current_position;
			
			// === 子彈相關設定 - 360 度包圍攻擊 (Knife) ===
			int numberOfProjectiles_00 = 360;
			float circleRadius_00 = 30.0f;
			float totalAngle_00 = 360.0f;
			float totalAnglePerProjectile_00 = totalAngle_00 / numberOfProjectiles_00;
			for (int i = 0; i < numberOfProjectiles_00; i++)
			{
				float launchAngle = totalAnglePerProjectile_00 * i;
				float launchAngleRad = launchAngle * Mathf.Deg2Rad;
				UnityEngine.Vector3 launchPosition = new UnityEngine.Vector3(
					targetPosition.x + circleRadius_00 * Mathf.Cos(launchAngleRad),
					targetPosition.y + circleRadius_00 * Mathf.Sin(launchAngleRad),
					targetPosition.z
				);
				World.world.projectiles.spawn(
					pInitiator: selfActor,
					pTargetObject: mainTarget,
					pAssetID: "Knife",
					pLaunchPosition: launchPosition,
					pTargetPosition: targetPosition,
					pTargetZ: 0.0f
				);
			}
			
			// === 子彈相關設定 - 360 度包圍攻擊 (Fork) ===
			int numberOfProjectiles_01 = 360;
			float circleRadius_01 = 15.0f;
			float totalAngle_01 = 360.0f;
			float totalAnglePerProjectile_01 = totalAngle_01 / numberOfProjectiles_01;
			for (int i = 0; i < numberOfProjectiles_01; i++)
			{
				float launchAngle = totalAnglePerProjectile_01 * i;
				float launchAngleRad = launchAngle * Mathf.Deg2Rad;
				UnityEngine.Vector3 launchPosition = new UnityEngine.Vector3(
					targetPosition.x + circleRadius_01 * Mathf.Cos(launchAngleRad),
					targetPosition.y + circleRadius_01 * Mathf.Sin(launchAngleRad),
					targetPosition.z
				);
				World.world.projectiles.spawn(
					pInitiator: selfActor,
					pTargetObject: mainTarget,
					pAssetID: "Fork",
					pLaunchPosition: launchPosition,
					pTargetPosition: targetPosition,
					pTargetZ: 0.0f
				);
			}
			return true;
		}
		public static bool EvilSpearAwakens(BaseSimObject pSelf, WorldTile pTile = null)
		{// 魔槍 武器復甦
			// 1. 基本安全檢查：確保 pSelf 及其 Actor 組件存在且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			// 如果單位持有 "gluttony_demon_king" 狀態
			if (selfActor.hasStatus("gluttony_demon_king"))
			{
				// 則為其持續添加 "gldk2" 狀態，每次添加時設定持續時間為 10 秒
				// 如果 gldk2 已經存在，此操作會刷新其持續時間為 10 秒
				selfActor.addStatusEffect("gldk2", 3600f); // 將時間改為 10 秒
			}
			return false; // 特殊效果通常返回 false，表示不阻止其他效果鏈
		}
		public static bool EvilLawGet05(BaseSimObject pTarget, WorldTile pTile)
		{// 魔槍 給予拾獲者認證狀態
			// 1. 安全檢查：確保 pTarget 存在且有 Actor 組件
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor targetActor = pTarget.a;
			// =======================================================
			// === 新增/修改邏輯：檢查王國內是否已存在特定魔王 ===
			const string DemonKingStatusID = "gluttony_demon_king";
			Kingdom currentKingdom = targetActor.kingdom;
			// 檢查目標單位是否屬於一個國家
			if (currentKingdom != null)
			{
				// 遍歷王國中的所有單位
				foreach (Actor kingdomUnit in currentKingdom.units)
				{
					// 跳過自己，只檢查其他單位
					if (kingdomUnit == null || kingdomUnit == targetActor)
					{
						continue;
					}
					// 檢查其他單位是否擁有 "???_demon_king" 狀態
					if (kingdomUnit.hasStatus(DemonKingStatusID))
					{
						// 如果找到，則直接返回 false，不執行後續操作
						return false;
					}
				}
			}
			// =======================================================
			// 2. 如果通過了上面的檢查，繼續執行後續邏輯
			// 檢查是否持有 特質
			if (targetActor.hasTrait("evillaw_starvation"))
			{
				// 如果單位已經有 特質，就不進行任何動作
				return true; 
			}
			// 3. 如果沒有 特質，則檢查是否持有 狀態效果
			else if (!targetActor.hasStatus("gldk3"))
			{
				// 如果都沒有，就添加 狀態效果
				targetActor.addStatusEffect("gldk3", 600f);
			}
			return true; // 表示操作成功執行
		}
		public static bool Acid_Domain(BaseSimObject pTarget, WorldTile pTile = null)
		{// 魔槍 酸域
			if (pTarget == null || !pTarget.isActor() || !pTarget.a.isAlive())
			{
				return false;
			}
			if (!pTarget.hasStatus("gluttony_demon_king"))
			{
				return false;
			}
			// 獲取角色的營養值
			int currentNutrition = pTarget.a.data.nutrition;
			int radius = 0; // 初始化半徑為 0
			// 根據營養值來設定半徑
			if (currentNutrition < 25)
			{
				radius = 30;
			}
			else if (currentNutrition >= 25 && currentNutrition < 50)
			{
				radius = 25;
			}
			else if (currentNutrition >= 50 && currentNutrition < 75)
			{
				radius = 20;
			}
			else if (currentNutrition >= 75 && currentNutrition < 85)
			{
				radius = 15;
			}
			else if (currentNutrition >= 85 && currentNutrition < 90)
			{
				radius = 10;
			}
			else if (currentNutrition >= 90 && currentNutrition < 95)
			{
				radius = 5;
			}
			else if (currentNutrition >= 95 && currentNutrition < 100)
			{
				radius = 1;
			}
			else if (currentNutrition >= 100) // 營養值為 100 或更高
			{
				radius = 0;
			}
			// 如果半徑為0，則技能不會產生任何效果，直接返回
			if (radius <= 0)
			{
				return false;
			}
			WorldTile targetTile = pTarget.a.current_tile;
			// 將動態設定的半徑傳入 loopWithBrush
			World.world.loopWithBrush(
				targetTile,
				Brush.get(radius, "circ_"),
				new PowerActionWithID(Items01Actions.checkAcidTerraform_AoE),
				pTarget.a.data.id.ToString()
			);
			return true;
		}
		private static Dictionary<long, float> _cumulativeNutrition = new Dictionary<long, float>();
		public static bool checkAcidTerraform_AoE(WorldTile pTile, string pPowerID)
		{// 魔槍 酸域 輔助函數
			if (pTile == null) return false;
			// 1. 遍歷所有單位，找到施放技能的單位
			Actor selfActor = null;
			long parsedID;
			if (long.TryParse(pPowerID, out parsedID))
			{
				foreach (Actor unit in World.world.units)
				{
					if (unit.data.id == parsedID)
					{
						selfActor = unit;
						break;
					}
				}
			}
			if (selfActor == null)
			{
				return false;
			}
			// 獲取角色的最大營養值
			int maxNutrition = 100;
			// 檢查字典中是否已有該角色的累積值，若無則初始化
			if (!_cumulativeNutrition.ContainsKey(selfActor.data.id))
			{
				_cumulativeNutrition[selfActor.data.id] = 0f;
			}
			// 2. 如果地格是暫時凍結的
			if (pTile.isTemporaryFrozen())
			{
				pTile.unfreeze(99);
				_cumulativeNutrition[selfActor.data.id] += 0.1f;
				
				// 如果累積值超過或等於 1，則將其添加到營養值中
				if (_cumulativeNutrition[selfActor.data.id] >= 1f)
				{
					int nutritionToAdd = (int)Mathf.Floor(_cumulativeNutrition[selfActor.data.id]);
					selfActor.data.nutrition = Mathf.Min(selfActor.data.nutrition + nutritionToAdd, maxNutrition);
					_cumulativeNutrition[selfActor.data.id] -= nutritionToAdd;
				}
				return true;
			}
			// 3. 如果地格已經是「荒地」類型
			if (pTile.top_type != null && pTile.top_type.wasteland)
			{
				return false;
			}
			// 4. 如果地格有頂層類型
			if (pTile.top_type != null)
			{
				MapAction.decreaseTile(pTile, true, "flash");
				_cumulativeNutrition[selfActor.data.id] += 0.1f;
				if (_cumulativeNutrition[selfActor.data.id] >= 1f)
				{
					int nutritionToAdd = (int)Mathf.Floor(_cumulativeNutrition[selfActor.data.id]);
					selfActor.data.nutrition = Mathf.Min(selfActor.data.nutrition + nutritionToAdd, maxNutrition);
					_cumulativeNutrition[selfActor.data.id] -= nutritionToAdd;
				}
				return true;
			}
			// 5. 如果地格是地面類型
			if (pTile.Type.ground)
			{
				if (pTile.isTileRank(TileRank.Low))
				{
					MapAction.terraformTop(pTile, TopTileLibrary.wasteland_low);
				}
				else if (pTile.isTileRank(TileRank.High))
				{
					MapAction.terraformTop(pTile, TopTileLibrary.wasteland_high);
				}
				AchievementLibrary.lets_not.check(null);
				_cumulativeNutrition[selfActor.data.id] += 0.1f;
				if (_cumulativeNutrition[selfActor.data.id] >= 1f)
				{
					int nutritionToAdd = (int)Mathf.Floor(_cumulativeNutrition[selfActor.data.id]);
					selfActor.data.nutrition = Mathf.Min(selfActor.data.nutrition + nutritionToAdd, maxNutrition);
					_cumulativeNutrition[selfActor.data.id] -= nutritionToAdd;
				}
				return true;
			}
			return false;
		}
			#endregion
			#region 怠惰之杖
	//杖		
		public static bool Anti_NoSleeping(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 魔槍 不眠者殺手
			// 安全檢查：確保施法者和目標都存在且有效
			if (pSelf?.a == null || pTarget?.a == null || !pSelf.a.isAlive() || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			Actor targetActor = pTarget.a;
/*			if (targetActor.hasTrait("holyarts_justice"))
			{// 如果目標持有 特質 則返回
				return false;
			}*/
			// 檢查目標種族是否與施法者不同
			if (!targetActor.hasStatus("sleeping"))
			{
				// 取得施法者的傷害值
				float selfDamage = selfActor.stats["damage"];
				// 施加額外傷害 (例如額外造成 50% 傷害)
				targetActor.getHit(selfDamage * 1.50f, true, AttackType.Weapon, selfActor, false, true, false);
				return true;
			}
			return false;
		}
		public static bool Anti_NoSleeping_Defense(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 魔槍 不眠者特防
			// 1. 安全檢查：確保施法者 (防禦者) 和目標 (攻擊者) 都存在且有效
			if (pSelf?.a == null || pTarget?.a == null || !pSelf.a.isAlive() || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a; // 防禦者
			Actor targetActor = pTarget.a; // 攻擊者
			if (!selfActor.hasStatus("sloth_demon_king") || !selfActor.hasStatus("defense_on"))
			{
				return false;
			}
			if (!targetActor.hasStatus("sleeping"))
			{				
				// 取得攻擊者對防禦者造成的傷害值
				float damageDealt = targetActor.stats["damage"];
				// 你可以根據需要調整這個百分比
				float damageToHeal = damageDealt * 0.90f;
				// 直接增加生命值，以抵銷受到的傷害
				selfActor.data.health += (int)damageToHeal;
				// 確保生命值不會超過最大值
				if (selfActor.data.health > selfActor.getMaxHealth())
				{
					selfActor.data.health = selfActor.getMaxHealth();
				}
				return true;
			}
			return false;
		}
		public static bool EvilStickThrowing00(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 魔杖 冰槍 環狀集火
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive() || pTarget == null || !pTarget.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			
			// 檢查目標是否為敵對單位或建築
			if (pTarget.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(pTarget.kingdom))
			{
				return false;
			}
			
			// === 狀態檢查 ===
			if (!selfActor.hasStatus("sloth_demon_king"))
			{
				return false;
			}
			string attackCooldownStatus = "item_cdt00";
			float attackCooldownDuration = 0.01f;
			if (selfActor.hasStatus(attackCooldownStatus))
			{
				return false;
			}
			
			// === 成功找到目標，施加冷卻並發射投射物 ===
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			//selfActor.addStatusEffect("invincible", 0.05f);

			UnityEngine.Vector3 selfPosition = selfActor.current_position;
			UnityEngine.Vector3 targetPosition = pTarget.current_position;
			
			UnityEngine.Vector3 baseLaunchPoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, targetPosition.x, targetPosition.y, selfActor.stats["size"]);
			baseLaunchPoint.y += 0.5f;

			// 子彈相關設定 - 環狀攻擊 (第一層)
			int numberOfProjectiles_01 = 6;
			float spreadDistance_01 = 5.0f;
			float spreadAngle_01 = 360.0f;
			float totalSpread_01 = numberOfProjectiles_01 > 0 ? (spreadAngle_01 / numberOfProjectiles_01) : 0f;
			for (int i = 0; i < numberOfProjectiles_01; i++)
			{
				float angle = totalSpread_01 * i - spreadAngle_01 / 2f;
				UnityEngine.Vector3 launchPosition_01 = baseLaunchPoint + Quaternion.Euler(0f, 0f, angle) * new UnityEngine.Vector3(spreadDistance_01, 0, 0);
				World.world.projectiles.spawn(
					pInitiator: selfActor,
					pTargetObject: pTarget,
					pAssetID: "IcePick",
					pLaunchPosition: launchPosition_01,
					pTargetPosition: targetPosition,
					pTargetZ: 0.0f
				);
			}
			
			// 子彈相關設定 - 環狀攻擊 (第二層)
			int numberOfProjectiles_00 = 6;
			float spreadDistance_00 = 7.50f;
			float spreadAngle_00 = 360.0f;
			float totalSpread_00 = numberOfProjectiles_00 > 0 ? (spreadAngle_00 / numberOfProjectiles_00) : 0f;
			for (int i = 0; i < numberOfProjectiles_00; i++)
			{
				float angle = totalSpread_00 * i - spreadAngle_00 / 2f;
				UnityEngine.Vector3 launchPosition_00 = baseLaunchPoint + Quaternion.Euler(0f, 0f, angle) * new UnityEngine.Vector3(spreadDistance_00, 0, 0);
				World.world.projectiles.spawn(
					pInitiator: selfActor,
					pTargetObject: pTarget,
					pAssetID: "IcePick",
					pLaunchPosition: launchPosition_00,
					pTargetPosition: targetPosition,
					pTargetZ: 0.0f
				);
			}
			
			// 子彈相關設定 - 環狀攻擊 (第三層)
			int numberOfProjectiles_02 = 12;
			float spreadDistance_02 = 10.00f;
			float spreadAngle_02 = 360.0f;
			float totalSpread_02 = numberOfProjectiles_02 > 0 ? (spreadAngle_02 / numberOfProjectiles_02) : 0f;
			for (int i = 0; i < numberOfProjectiles_02; i++)
			{
				float angle = totalSpread_02 * i - spreadAngle_02 / 2f;
				UnityEngine.Vector3 launchPosition_02 = baseLaunchPoint + Quaternion.Euler(0f, 0f, angle) * new UnityEngine.Vector3(spreadDistance_02, 0, 0);
				World.world.projectiles.spawn(
					pInitiator: selfActor,
					pTargetObject: pTarget,
					pAssetID: "Snowflake",
					pLaunchPosition: launchPosition_02,
					pTargetPosition: targetPosition,
					pTargetZ: 0.0f
				);
			}
			return true;
		}
		public static bool EvilStickThrowing01(BaseSimObject pSelf, WorldTile pTile = null)
		{// 魔杖 冰槍 常態攻擊
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			
			// === 狀態檢查 ===
			if (!selfActor.hasStatus("sloth_demon_king"))
			{
				return false;
			}
			
			// === 冷卻狀態檢查 ===
			string attackCooldownStatus = "item_cdt01";
			float attackCooldownDuration = 0.01f;
			if (selfActor.hasStatus(attackCooldownStatus))
			{
				return false;
			}
			
			// === 核心邏輯：根據周圍敵人的數量決定是否發動 (預設至少3個敵人) ===
			float maxRange = 20f;
			int requiredEnemies = 1;
			int enemyCount = 0;
			BaseSimObject mainTarget = null;
			float closestDist = float.MaxValue;
			
			// 遍歷所有單位和建築物來計數和尋找主目標
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive() || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist <= maxRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = other;
					}
				}
			}
			foreach (var building in World.world.buildings)
			{
				if (building == null || !building.isAlive() || !selfActor.kingdom.isEnemy(building.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, building.current_position);
				if (dist <= maxRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = building;
					}
				}
			}
			
			if (enemyCount < requiredEnemies || mainTarget == null)
			{
				return false;
			}

			// === 成功找到目標，施加冷卻並發射投射物 ===
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			//selfActor.addStatusEffect("invincible", 0.05f);

			UnityEngine.Vector3 selfPosition = selfActor.current_position;
			UnityEngine.Vector3 targetPosition = mainTarget.current_position;
			UnityEngine.Vector3 baseLaunchPoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, targetPosition.x, targetPosition.y, selfActor.stats["size"]);
			baseLaunchPoint.y += 0.5f;

			int numberOfProjectiles_00 = 12;
			float spreadDistance_00 = 5.0f;
			float spreadAngle_00 = 360.0f;
			float totalSpread = spreadAngle_00 / numberOfProjectiles_00;
			for (int i = 0; i < numberOfProjectiles_00; i++)
			{
				float angle = totalSpread * i - spreadAngle_00 / 2f;
				UnityEngine.Vector3 launchPosition = baseLaunchPoint + Quaternion.Euler(0f, 0f, angle) * new UnityEngine.Vector3(spreadDistance_00, 0, 0);

				World.world.projectiles.spawn(
					pInitiator: selfActor,
					pTargetObject: mainTarget,
					pAssetID: "IcePick",
					pLaunchPosition: launchPosition,
					pTargetPosition: targetPosition,
					pTargetZ: 0.0f
				);
			}
			return true;
		}
		public static bool EvilStickThrowing02(BaseSimObject pSelf, WorldTile pTile = null)
		{// 魔杖 冰槍群 常態攻擊
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			
			// === 狀態檢查 ===
			if (!selfActor.hasStatus("sloth_demon_king"))
			{
				return false;
			}
			
			// === 冷卻狀態檢查 ===
			string attackCooldownStatus = "item_cdt02";
			float attackCooldownDuration = 300.0f;
			if (selfActor.hasStatus(attackCooldownStatus))
			{
				return false;
			}
			
			// === 核心邏輯：根據周圍敵人的數量決定是否發動 (預設至少3個敵人) ===
			float maxRange = 35f;
			int requiredEnemies = 3;
			int enemyCount = 0;
			BaseSimObject mainTarget = null;
			float closestDist = float.MaxValue;
			
			// 遍歷所有單位和建築物來計數和尋找主目標
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive() || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist <= maxRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = other;
					}
				}
			}
			foreach (var building in World.world.buildings)
			{
				if (building == null || !building.isAlive() || !selfActor.kingdom.isEnemy(building.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, building.current_position);
				if (dist <= maxRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = building;
					}
				}
			}
			
			if (enemyCount < requiredEnemies || mainTarget == null)
			{
				return false;
			}
			
			// === 成功找到目標，施加冷卻並發射投射物 ===
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			//selfActor.addStatusEffect("invincible", 0.05f);

			
			UnityEngine.Vector3 selfPosition = selfActor.current_position;
			UnityEngine.Vector3 targetPosition = mainTarget.current_position;
			
			UnityEngine.Vector3 baseLaunchPoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, targetPosition.x, targetPosition.y, selfActor.stats["size"]);
			baseLaunchPoint.y += 0.5f;

			int numberOfProjectiles_0001 = 360;
			float spreadDistance_0001 = 5.0f;
			float spreadAngle_0001 = 360.0f;
			float totalSpread = spreadAngle_0001 / numberOfProjectiles_0001;
			for (int i = 0; i < numberOfProjectiles_0001; i++)
			{
				float angle = totalSpread * i - spreadAngle_0001 / 2f;
				UnityEngine.Vector3 launchPosition_0001 = baseLaunchPoint + Quaternion.Euler(0f, 0f, angle) * new UnityEngine.Vector3(spreadDistance_0001, 0, 0);
				World.world.projectiles.spawn(
					pInitiator: selfActor,
					pTargetObject: mainTarget,
					pAssetID: "IcePick",
					pLaunchPosition: launchPosition_0001,
					pTargetPosition: targetPosition,
					pTargetZ: 0.0f
				);
			}
			
			int numberOfProjectiles_0101 = 360;
			float circleRadius_0101 = 15.0f;
			float totalAngle_0101 = 360.0f;
			float totalAnglePerProjectile_0101 = totalAngle_0101 / numberOfProjectiles_0101;
			for (int i = 0; i < numberOfProjectiles_0101; i++)
			{
				float launchAngle = totalAnglePerProjectile_0101 * i;
				float launchAngleRad = launchAngle * Mathf.Deg2Rad;
				UnityEngine.Vector3 launchPosition_0101 = new UnityEngine.Vector3(
					targetPosition.x + circleRadius_0101 * Mathf.Cos(launchAngleRad),
					targetPosition.y + circleRadius_0101 * Mathf.Sin(launchAngleRad),
					targetPosition.z
				);
				World.world.projectiles.spawn(
					pInitiator: selfActor,
					pTargetObject: mainTarget,
					pAssetID: "IcePick",
					pLaunchPosition: launchPosition_0101,
					pTargetPosition: targetPosition,
					pTargetZ: 0.0f
				);
			}
			
			int numberOfProjectiles_0102 = 360;
			float circleRadius_0102 = 10.0f;
			float totalAngle_0102 = 360.0f;
			float totalAnglePerProjectile_0102 = totalAngle_0102 / numberOfProjectiles_0102;
			for (int i = 0; i < numberOfProjectiles_0102; i++)
			{
				float launchAngle = totalAnglePerProjectile_0102 * i;
				float launchAngleRad = launchAngle * Mathf.Deg2Rad;
				UnityEngine.Vector3 launchPosition_0102 = new UnityEngine.Vector3(
					targetPosition.x + circleRadius_0102 * Mathf.Cos(launchAngleRad),
					targetPosition.y + circleRadius_0102 * Mathf.Sin(launchAngleRad),
					targetPosition.z
				);
				World.world.projectiles.spawn(
					pInitiator: selfActor,
					pTargetObject: mainTarget,
					pAssetID: "IcePick",
					pLaunchPosition: launchPosition_0102,
					pTargetPosition: targetPosition,
					pTargetZ: 0.0f
				);
			}
			return true;
		}
		public static bool EvilStickThrowing03(BaseSimObject pSelf, WorldTile pTile = null)
		{// 魔杖 大雪崩彈 常態攻擊 Avalanche
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			
			// === 狀態檢查 ===
			if (!selfActor.hasStatus("sloth_demon_king"))
			{
				return false;
			}
			
			// === 冷卻狀態檢查 ===
			string attackCooldownStatus = "item_cdt03";
			float attackCooldownDuration = 3600.0f;
			if (selfActor.hasStatus(attackCooldownStatus))
			{
				return false;
			}
			
			// === 核心邏輯：根據周圍敵人的數量決定是否發動 (預設至少3個敵人) ===
			float maxRange = 18f;
			int requiredEnemies = 1;
			int enemyCount = 0;
			BaseSimObject mainTarget = null;
			float closestDist = float.MaxValue;
			
			// 遍歷所有單位和建築物來計數和尋找主目標
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive() || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist <= maxRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = other;
					}
				}
			}
			foreach (var building in World.world.buildings)
			{
				if (building == null || !building.isAlive() || !selfActor.kingdom.isEnemy(building.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, building.current_position);
				if (dist <= maxRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = building;
					}
				}
			}
			
			// 在檢查發動機率前，先檢查是否有足夠的敵人
			if (enemyCount < requiredEnemies || mainTarget == null)
			{
				return false;
			}

			// === 額外的發動機率檢查 ===
			if (!Randy.randomChance(0.025f))
			{
				return false;
			}
			
			// === 成功找到目標，施加冷卻並發射投射物 ===
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			//selfActor.addStatusEffect("invincible", 0.05f);

			
			UnityEngine.Vector3 selfPosition = selfActor.current_position;
			UnityEngine.Vector3 targetPosition = mainTarget.current_position;
			
			UnityEngine.Vector3 basePoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, targetPosition.x, targetPosition.y, selfActor.stats["size"]);
			basePoint.y += 0.5f;
			
			int numberOfProjectiles_00 = 10;
			float spreadAngle_00 = 1.0f;
			for (int i = 0; i < numberOfProjectiles_00; i++)
			{
				UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, spreadAngle_00 * (i - (numberOfProjectiles_00 - 1f) / 2f));
				UnityEngine.Vector3 spreadTarget = rotation * (targetPosition - basePoint) + basePoint;
				
				World.world.projectiles.spawn(
					pInitiator: selfActor,
					pTargetObject: mainTarget,
					pAssetID: "Avalanche",
					pLaunchPosition: basePoint,
					pTargetPosition: spreadTarget,
					pTargetZ: 0.0f
				);
			}
			return true;
		}
		public static bool EvilStickAwakens(BaseSimObject pSelf, WorldTile pTile = null)
		{// 魔杖 復甦
			// 1. 基本安全檢查：確保 pSelf 及其 Actor 組件存在且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			// 如果單位持有 "sloth_demon_king" 狀態
			if (selfActor.hasStatus("sloth_demon_king"))
			{
				// 則為其持續添加 "sdk2" 狀態，每次添加時設定持續時間為 10 秒
				// 如果 sdk2 已經存在，此操作會刷新其持續時間為 10 秒
				selfActor.addStatusEffect("sdk2", 3600f); // 將時間改為 10 秒
			}
			return false; // 特殊效果通常返回 false，表示不阻止其他效果鏈
		}
		public static bool EvilLawGet06(BaseSimObject pTarget, WorldTile pTile)
		{// 魔杖 給予拾獲者認證狀態
			// 1. 安全檢查：確保 pTarget 存在且有 Actor 組件
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor targetActor = pTarget.a;
			// =======================================================
			// === 新增/修改邏輯：檢查王國內是否已存在特定魔王 ===
			const string DemonKingStatusID = "sloth_demon_king";
			Kingdom currentKingdom = targetActor.kingdom;
			// 檢查目標單位是否屬於一個國家
			if (currentKingdom != null)
			{
				// 遍歷王國中的所有單位
				foreach (Actor kingdomUnit in currentKingdom.units)
				{
					// 跳過自己，只檢查其他單位
					if (kingdomUnit == null || kingdomUnit == targetActor)
					{
						continue;
					}
					// 檢查其他單位是否擁有 "???_demon_king" 狀態
					if (kingdomUnit.hasStatus(DemonKingStatusID))
					{
						// 如果找到，則直接返回 false，不執行後續操作
						return false;
					}
				}
			}
			// =======================================================
			// 2. 如果通過了上面的檢查，繼續執行後續邏輯
			// 檢查是否持有 特質
			if (targetActor.hasTrait("evillaw_sleeping"))
			{
				// 如果單位已經有 特質，就不進行任何動作
				return true; 
			}
			// 3. 如果沒有 特質，則檢查是否持有 狀態效果
			else if (!targetActor.hasStatus("sdk3"))
			{
				// 如果都沒有，就添加 狀態效果
				targetActor.addStatusEffect("sdk3", 600f);
			}
			return true; // 表示操作成功執行
		}
			#endregion
			#region	嫉妒之刃
	//刀
		public static bool Shock_protectionX2(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 魔刃 震盪防護 5%
			// 1. 安全檢查：確保發動者、目標和地塊資訊都存在。
			if (pSelf == null || pTarget == null || pTile == null)
			{
				return false;
			}
			if (!Randy.randomChance(0.05f)) // 0.05f 代表 5% 的機率
			{
				return false; // 如果機率檢查失敗，則直接退出，不發動效果
			}
			// 將 pSelf 轉換為 Actor 類型，並進行安全檢查
			Actor selfActor = pSelf.a;
			if (selfActor == null)
			{
				// 如果 pSelf 不是一個 Actor，則無法繼續
				return false;
			}
			if (!selfActor.hasStatus("envy_demon_king"))
			{
				return false;
			}
			string attackCooldownStatus = "item_cdt04";
			float attackCooldownDuration = 0.01f;
			if (selfActor.hasStatus(attackCooldownStatus))
			{
				return false;
			}
			// 2. 施加爆炸波紋與震盪效果。
			// 參數：中心地塊, 震盪半徑(3), 力量大小(0.3f), 影響地面(true), 其他參數
			World.world.applyForceOnTile(pTile, 25, 2.50f, true, 0, null, pTarget, null, false);
			
			// 參數：位置, 視覺半徑(3f), 持續時間(5.0f)
			EffectsLibrary.spawnExplosionWave(pTile.posV3, 0f, 5.00f);
			EffectsLibrary.spawnExplosionWave(pTile.posV3, 3f, 2.50f);
			EffectsLibrary.spawnExplosionWave(pTile.posV3, 6f, 0.99f);
			// 3. 回傳 true 表示攻擊成功發動。
			return true;
		}
		public static bool Compare(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 魔刃 比較特攻 效果 55項
			// 安全檢查...
			if (pSelf?.a == null || pTarget?.a == null || !pSelf.a.isAlive() || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			Actor targetActor = pTarget.a;
			float totalBonusMultiplier = 0f;
			float SetValue = 3.0f;
			bool hasTriggered = false;
			//目前比較的 數值 和 特質 總計 55
			// 檢查數值屬性
			if (targetActor.data.happiness > selfActor.data.happiness)
			{//01 幸福值比較
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (targetActor.data.nutrition > selfActor.data.nutrition)
			{//02 營養值比較
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (targetActor.data.money > selfActor.data.money)
			{//03 金錢值比較
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (targetActor.data.level > selfActor.data.level)
			{//04 等級值比較
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (targetActor.data.renown > selfActor.data.renown)
			{//05 聲望值比較
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (targetActor.data.lover != null && selfActor.data.lover == null)
			{//06 戀人有無
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (targetActor.data.best_friend_id != null && selfActor.data.best_friend_id == null)
			{//07 摯友有無
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (targetActor.data.clan != null && selfActor.data.clan == null)
			{//08 氏族有無
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (selfActor.getProfession() == UnitProfession.Leader && targetActor.getProfession() == UnitProfession.King)
			{//09.1 施法者是領主 目標是國王
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			else if (selfActor.getProfession() != UnitProfession.King && selfActor.getProfession() != UnitProfession.Leader && targetActor.getProfession() == UnitProfession.Leader)
			{//09.2 施法者不是國王也不是領主，但目標是領主
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			else if (selfActor.getProfession() != UnitProfession.King && targetActor.getProfession() == UnitProfession.King)
			{//09.3 施法者不是國王，但目標是國王
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (targetActor.hasTrait("golden_tooth") && !selfActor.hasTrait("golden_tooth"))
			{//10 特質 金牙
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (targetActor.hasTrait("attractive") && !selfActor.hasTrait("attractive"))
			{//11 特質 魅力
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (targetActor.hasTrait("strong") && !selfActor.hasTrait("strong"))
			{//12 特質 強壯
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (targetActor.hasTrait("genius") && !selfActor.hasTrait("genius"))
			{//13 特質 天才
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (targetActor.hasTrait("lucky") && !selfActor.hasTrait("lucky"))
			{//14 特質 幸運
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (targetActor.hasTrait("fast") && !selfActor.hasTrait("fast"))
			{//15 特質 快速
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (targetActor.hasTrait("agile") && !selfActor.hasTrait("agile"))
			{//16 特質 敏捷
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (targetActor.hasTrait("strong_minded") && !selfActor.hasTrait("strong_minded"))
			{//17 特質 堅忍
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (targetActor.hasTrait("eagle_eyed") && !selfActor.hasTrait("eagle_eyed"))
			{//18 特質 鷹眼
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (targetActor.hasTrait("fertile") && !selfActor.hasTrait("fertile"))
			{//19 特質 興旺
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (targetActor.hasTrait("blessed") && !selfActor.hasTrait("blessed"))
			{//20 特質 祝福
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (targetActor.hasTrait("moonchild") && !selfActor.hasTrait("moonchild"))
			{//21 特質 月之子
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (targetActor.hasTrait("nightchild") && !selfActor.hasTrait("nightchild"))
			{//22 特質 夜之子
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (targetActor.hasTrait("immortal") && !selfActor.hasTrait("immortal"))
			{//23 特質 不朽(永生)
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (targetActor.hasTrait("immune") && !selfActor.hasTrait("immune"))
			{//24 特質 免疫
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (targetActor.hasTrait("healing_aura") && !selfActor.hasTrait("healing_aura"))
			{//25 特質 治療光環
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (targetActor.hasTrait("bubble_defense") && !selfActor.hasTrait("bubble_defense"))
			{//26 特質 防護罩
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (targetActor.hasTrait("acid_proof") && !selfActor.hasTrait("acid_proof"))
			{//27 特質 防酸
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (targetActor.hasTrait("fire_proof") && !selfActor.hasTrait("fire_proof"))
			{//28 特質 防火
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (targetActor.hasTrait("freeze_proof") && !selfActor.hasTrait("freeze_proof"))
			{//29特質 防凍
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (targetActor.hasTrait("poison_immune") && !selfActor.hasTrait("poison_immune"))
			{//30 特質 抗毒
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (targetActor.hasTrait("tough") && !selfActor.hasTrait("tough"))
			{//31 特質 強硬
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (targetActor.hasTrait("weightless") && !selfActor.hasTrait("weightless"))
			{//32 特質 輕盈
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (targetActor.hasTrait("thorns") && !selfActor.hasTrait("thorns"))
			{//33 特質 荊棘
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (targetActor.hasTrait("boosted_vitality") && !selfActor.hasTrait("boosted_vitality"))
			{//34 特質 活力
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (selfActor.hasTrait("ugly") && !targetActor.hasTrait("ugly"))
			{//35 特質 醜陋
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (selfActor.hasTrait("weak") && !targetActor.hasTrait("weak"))
			{//36 特質 嬴弱
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (selfActor.hasTrait("slow") && !targetActor.hasTrait("slow"))
			{//37 特質 緩慢
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (selfActor.hasTrait("fat") && !targetActor.hasTrait("fat"))
			{//38 特質 肥胖
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (selfActor.hasTrait("unlucky") && !targetActor.hasTrait("unlucky"))
			{//39 特質 倒楣
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (selfActor.hasTrait("stupid") && !targetActor.hasTrait("stupid"))
			{//40 特質 愚蠢
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (selfActor.hasTrait("short_sighted") && !targetActor.hasTrait("short_sighted"))
			{//41 特質 近視
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (selfActor.hasTrait("fragile_health") && !targetActor.hasTrait("fragile_health"))
			{//42 特質 脆弱
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (selfActor.hasStatus("burning") && !targetActor.hasStatus("burning"))
			{//43 狀態 燃燒
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (selfActor.hasStatus("slowness") && !targetActor.hasStatus("slowness"))
			{//44 狀態 緩速
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (selfActor.hasStatus("poisoned") && !targetActor.hasStatus("poisoned"))
			{//45 狀態 毒
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (selfActor.hasStatus("ash_fever") && !targetActor.hasStatus("ash_fever"))
			{//46 狀態 灰熱病
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (selfActor.hasStatus("cough") && !targetActor.hasStatus("cough"))
			{//47 狀態 咳嗽
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (selfActor.hasStatus("spell_silence") && !targetActor.hasStatus("spell_silence"))
			{//47 狀態 沉默
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (targetActor.hasStatus("powerup") && !selfActor.hasStatus("powerup"))
			{//48 狀態 巨大
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (targetActor.hasStatus("caffeinated") && !selfActor.hasStatus("caffeinated"))
			{//49 狀態 咖啡因
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (targetActor.hasStatus("enchanted") && !selfActor.hasStatus("enchanted"))
			{//50 狀態 充能
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (targetActor.hasStatus("rage") && !selfActor.hasStatus("rage"))
			{//51 狀態 狂暴
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (targetActor.hasStatus("shield") && !selfActor.hasStatus("shield"))
			{//52 狀態 護盾
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (targetActor.hasStatus("inspired") && !selfActor.hasStatus("inspired"))
			{//53 狀態 動力
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (targetActor.hasStatus("spell_boost") && !selfActor.hasStatus("spell_boost"))
			{//54 狀態 法術提升
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (targetActor.hasStatus("motivated") && !selfActor.hasStatus("motivated"))
			{//54 狀態 熱情
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			if (targetActor.hasStatus("antibody") && !selfActor.hasStatus("antibody"))
			{//55 狀態 抗體
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			// 如果至少有一個屬性觸發了效果，就計算並造成傷害
			if (hasTriggered)
			{
				float selfDamage = selfActor.stats["damage"];
				float finalDamage = selfDamage * totalBonusMultiplier;
				SpecialAttackDamage(pSelf, pTarget, finalDamage, pTile);
				return true;
			}
			return false;
		}
		public static bool Compare_Defense(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 魔刃 比較特防 效果
			// 安全檢查
			if (pSelf?.a == null || pTarget?.a == null || !pSelf.a.isAlive() || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			Actor targetActor = pTarget.a;
			float totalDefenseMultiplier = 0f;
			float SetValue = 0.75f;
			bool hasTriggered = false;

			if (!selfActor.hasStatus("envy_demon_king") || !selfActor.hasStatus("defense_on"))
			{
				return false;
			}
			// 這裡的邏輯與 Compare 函式類似，但方向相反
			// 我們比較「攻擊者」相對於「防禦者」的優勢
			
		// 檢查數值屬性
			//幸福 營養 金錢 等級 威望 是否高於師法者
			if (targetActor.data.happiness > selfActor.data.happiness) totalDefenseMultiplier += SetValue;
			if (targetActor.data.nutrition > selfActor.data.nutrition) totalDefenseMultiplier += SetValue;
			if (targetActor.data.money > selfActor.data.money) totalDefenseMultiplier += SetValue;
			if (targetActor.data.level > selfActor.data.level) totalDefenseMultiplier += SetValue;
			if (targetActor.data.renown > selfActor.data.renown) totalDefenseMultiplier += SetValue;
			//戀人 摯友 氏族有無
			if (targetActor.data.lover != null && selfActor.data.lover == null) totalDefenseMultiplier += SetValue;
			if (targetActor.data.best_friend_id != null && selfActor.data.best_friend_id == null) totalDefenseMultiplier += SetValue;
			if (targetActor.data.clan != null && selfActor.data.clan == null) totalDefenseMultiplier += SetValue;
			//職業身分
			if (targetActor.getProfession() == UnitProfession.King && selfActor.getProfession() != UnitProfession.King) totalDefenseMultiplier += SetValue;
			if (targetActor.getProfession() == UnitProfession.Leader && selfActor.getProfession() != UnitProfession.King && selfActor.getProfession() != UnitProfession.Leader) totalDefenseMultiplier += SetValue;
			
		// 檢查特質
			//優勢特質
			if (targetActor.hasTrait("golden_tooth") && !selfActor.hasTrait("golden_tooth")) totalDefenseMultiplier += SetValue;
			if (targetActor.hasTrait("attractive") && !selfActor.hasTrait("attractive")) totalDefenseMultiplier += SetValue;
			if (targetActor.hasTrait("strong") && !selfActor.hasTrait("strong")) totalDefenseMultiplier += SetValue;
			if (targetActor.hasTrait("genius") && !selfActor.hasTrait("genius")) totalDefenseMultiplier += SetValue;
			if (targetActor.hasTrait("lucky") && !selfActor.hasTrait("lucky")) totalDefenseMultiplier += SetValue;
			if (targetActor.hasTrait("fast") && !selfActor.hasTrait("fast")) totalDefenseMultiplier += SetValue;
			if (targetActor.hasTrait("agile") && !selfActor.hasTrait("agile")) totalDefenseMultiplier += SetValue;
			if (targetActor.hasTrait("strong_minded") && !selfActor.hasTrait("strong_minded")) totalDefenseMultiplier += SetValue;
			if (targetActor.hasTrait("eagle_eyed") && !selfActor.hasTrait("eagle_eyed")) totalDefenseMultiplier += SetValue;
			if (targetActor.hasTrait("fertile") && !selfActor.hasTrait("fertile")) totalDefenseMultiplier += SetValue;
			if (targetActor.hasTrait("blessed") && !selfActor.hasTrait("blessed")) totalDefenseMultiplier += SetValue;
			if (targetActor.hasTrait("moonchild") && !selfActor.hasTrait("moonchild")) totalDefenseMultiplier += SetValue;
			if (targetActor.hasTrait("nightchild") && !selfActor.hasTrait("nightchild")) totalDefenseMultiplier += SetValue;
			if (targetActor.hasTrait("immortal") && !selfActor.hasTrait("immortal")) totalDefenseMultiplier += SetValue;
			if (targetActor.hasTrait("immune") && !selfActor.hasTrait("immune")) totalDefenseMultiplier += SetValue;
			if (targetActor.hasTrait("healing_aura") && !selfActor.hasTrait("healing_aura")) totalDefenseMultiplier += SetValue;
			if (targetActor.hasTrait("bubble_defense") && !selfActor.hasTrait("bubble_defense")) totalDefenseMultiplier += SetValue;
			if (targetActor.hasTrait("acid_proof") && !selfActor.hasTrait("acid_proof")) totalDefenseMultiplier += SetValue;
			if (targetActor.hasTrait("fire_proof") && !selfActor.hasTrait("fire_proof")) totalDefenseMultiplier += SetValue;
			if (targetActor.hasTrait("freeze_proof") && !selfActor.hasTrait("freeze_proof")) totalDefenseMultiplier += SetValue;
			if (targetActor.hasTrait("poison_immune") && !selfActor.hasTrait("poison_immune")) totalDefenseMultiplier += SetValue;
			if (targetActor.hasTrait("tough") && !selfActor.hasTrait("tough")) totalDefenseMultiplier += SetValue;
			if (targetActor.hasTrait("weightless") && !selfActor.hasTrait("weightless")) totalDefenseMultiplier += SetValue;
			if (targetActor.hasTrait("thorns") && !selfActor.hasTrait("thorns")) totalDefenseMultiplier += SetValue;
			if (targetActor.hasTrait("boosted_vitality") && !selfActor.hasTrait("boosted_vitality")) totalDefenseMultiplier += SetValue;
			//劣勢特質
			if (selfActor.hasTrait("ugly") && !targetActor.hasTrait("ugly")) totalDefenseMultiplier += SetValue;
			if (selfActor.hasTrait("weak") && !targetActor.hasTrait("weak")) totalDefenseMultiplier += SetValue;
			if (selfActor.hasTrait("slow") && !targetActor.hasTrait("slow")) totalDefenseMultiplier += SetValue;
			if (selfActor.hasTrait("fat") && !targetActor.hasTrait("fat")) totalDefenseMultiplier += SetValue;
			if (selfActor.hasTrait("unlucky") && !targetActor.hasTrait("unlucky")) totalDefenseMultiplier += SetValue;
			if (selfActor.hasTrait("stupid") && !targetActor.hasTrait("stupid")) totalDefenseMultiplier += SetValue;
			if (selfActor.hasTrait("short_sighted") && !targetActor.hasTrait("short_sighted")) totalDefenseMultiplier += SetValue;
			if (selfActor.hasTrait("fragile_health") && !targetActor.hasTrait("fragile_health")) totalDefenseMultiplier += SetValue;
			
		// 檢查狀態
			//劣勢狀態
			if (selfActor.hasStatus("burning") && !targetActor.hasStatus("burning")) totalDefenseMultiplier += SetValue;
			if (selfActor.hasStatus("slowness") && !targetActor.hasStatus("slowness")) totalDefenseMultiplier += SetValue;
			if (selfActor.hasStatus("poisoned") && !targetActor.hasStatus("poisoned")) totalDefenseMultiplier += SetValue;
			if (selfActor.hasStatus("ash_fever") && !targetActor.hasStatus("ash_fever")) totalDefenseMultiplier += SetValue;
			if (selfActor.hasStatus("cough") && !targetActor.hasStatus("cough")) totalDefenseMultiplier += SetValue;
			if (selfActor.hasStatus("spell_silence") && !targetActor.hasStatus("spell_silence")) totalDefenseMultiplier += SetValue;
			//優勢狀態
			if (targetActor.hasStatus("powerup") && !selfActor.hasStatus("powerup")) totalDefenseMultiplier += SetValue;
			if (targetActor.hasStatus("caffeinated") && !selfActor.hasStatus("caffeinated")) totalDefenseMultiplier += SetValue;
			if (targetActor.hasStatus("enchanted") && !selfActor.hasStatus("enchanted")) totalDefenseMultiplier += SetValue;
			if (targetActor.hasStatus("rage") && !selfActor.hasStatus("rage")) totalDefenseMultiplier += SetValue;
			if (targetActor.hasStatus("shield") && !selfActor.hasStatus("shield")) totalDefenseMultiplier += SetValue;
			if (targetActor.hasStatus("inspired") && !selfActor.hasStatus("inspired")) totalDefenseMultiplier += SetValue;
			if (targetActor.hasStatus("spell_boost") && !selfActor.hasStatus("spell_boost")) totalDefenseMultiplier += SetValue;
			if (targetActor.hasStatus("motivated") && !selfActor.hasStatus("motivated")) totalDefenseMultiplier += SetValue;
			if (targetActor.hasStatus("antibody") && !selfActor.hasStatus("antibody")) totalDefenseMultiplier += SetValue;
			
			// 如果至少有一個屬性觸發了效果，就計算並抵銷傷害
			if (totalDefenseMultiplier > 0)
			{
				float damageDealt = targetActor.stats["damage"];
				float totalHeal = damageDealt * totalDefenseMultiplier;
				
				selfActor.data.health += (int)totalHeal;
				
				if (selfActor.data.health > selfActor.getMaxHealth())
				{
					selfActor.data.health = selfActor.getMaxHealth();
				}
				
				return true;
			}
			
			return false;
		}
		public static bool EnvyPoniardHit01(BaseSimObject pSelf, WorldTile pTile = null)
		{// 魔刃 雷擊 近  5 ~ 15
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;

			// 必須擁有 'envy_demon_king' 狀態才能發動
			if (!selfActor.hasStatus("envy_demon_king"))
			{
				return false;
			}
			// 冷卻狀態檢查
			string attackCooldownStatus = "item_cdt00";
			float attackCooldownDuration = 0.001f;
			if (selfActor.hasStatus(attackCooldownStatus))
			{
				return false;
			}
			// 尋找目標
			float maxRange = 15f;
			float minRange = 5f;
			int enemyCount = 0;
			int requiredEnemies = 1;
			BaseSimObject target = null;
			float closestDist = float.MaxValue;
			
			// 遍歷所有單位，尋找最接近且符合條件的目標
			// A. 優先遍歷所有單位，尋找最接近且符合條件的目標
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				
				bool isAttackable = false;
				foreach (string traitID in unconvertibleTraits00)
				{
					if (other.hasTrait(traitID))
					{
						isAttackable = true;
						break;
					}
				}
				if (other.getProfession() == UnitProfession.King || other.getProfession() == UnitProfession.Leader)
				{
					isAttackable = true;
				}
				if (!isAttackable)
				{
					continue;
				}
				
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist < maxRange && dist > minRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						target = other;
					}
				}
			}
			
			// B. 如果沒有找到單位，則尋找敵對建築
			foreach (var building in World.world.buildings)
			{
				if (building == null || !building.isAlive())
					continue;
				if (building.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(building.kingdom))
					continue;
				
				float dist = Vector2.Distance(selfActor.current_position, building.current_position);
				if (dist < maxRange && dist > minRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						target = building;
					}
				}
			}
			
			if (enemyCount < requiredEnemies || target == null)
			{
				return false;
			}
			
			// 成功找到目標，施加冷卻並發動攻擊
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			
			// 我們需要一個瓦片(tile)作為流星雨的目標，這裡直接使用目標單位的當前瓦片
			WorldTile targetTile = target.current_tile;
			
			if (targetTile != null)
			{// _00設定 
				float spreadRadius_00 = 1f;//隨機散佈半徑
				int numberOfLightning_00 = 1;//生成回數
				for (int i = 0; i < numberOfLightning_00; i++)
				{	// 自己計算隨機的 X 和 Y 座標偏移量
					float offsetX = UnityEngine.Random.Range(-spreadRadius_00, spreadRadius_00);
					float offsetY = UnityEngine.Random.Range(-spreadRadius_00, spreadRadius_00);
					
					// 計算新的隨機位置
					int newTileX = (int)targetTile.pos.x + (int)offsetX;
					int newTileY = (int)targetTile.pos.y + (int)offsetY;

					// === 修正：直接從 World.world 獲取地圖寬高 ===
					// 檢查新位置是否在地圖範圍內
					if (newTileX >= 0 && newTileX < MapBox.width && 
						newTileY >= 0 && newTileY < MapBox.height)
					{	// 根據新的座標獲取瓦片
						WorldTile randomTile = World.world.GetTile(newTileX, newTileY);

						if (randomTile != null)
						{
							// 使用新的隨機瓦片作為雷電的目標
							spawnLightning00(randomTile, 0.05f, selfActor);
						}
					}
				}
			}
			return true;
		}		
		public static bool EnvyPoniardHit02(BaseSimObject pSelf, WorldTile pTile = null)
		{// 魔刃 雷擊 中 14 ~ 30
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;

			// 必須擁有 'envy_demon_king' 狀態才能發動
			if (!selfActor.hasStatus("envy_demon_king"))
			{
				return false;
			}
			// 冷卻狀態檢查
			string attackCooldownStatus = "item_cdt01";
			float attackCooldownDuration = 0.001f;
			if (selfActor.hasStatus(attackCooldownStatus))
			{
				return false;
			}
			// 尋找目標
			float maxRange = 30f;
			float minRange = 14f; 
			int enemyCount = 0;
			int requiredEnemies = 1;
			BaseSimObject target = null;
			float closestDist = float.MaxValue;
			
			// 遍歷所有單位，尋找最接近且符合條件的目標
			// A. 優先遍歷所有單位，尋找最接近且符合條件的目標
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				
				bool isAttackable = false;
				foreach (string traitID in unconvertibleTraits00)
				{
					if (other.hasTrait(traitID))
					{
						isAttackable = true;
						break;
					}
				}
				if (other.getProfession() == UnitProfession.King || other.getProfession() == UnitProfession.Leader)
				{
					isAttackable = true;
				}
				if (!isAttackable)
				{
					continue;
				}
				
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist < maxRange && dist > minRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						target = other;
					}
				}
			}
			
			// B. 如果沒有找到單位，則尋找敵對建築
			foreach (var building in World.world.buildings)
			{
				if (building == null || !building.isAlive())
					continue;
				if (building.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(building.kingdom))
					continue;
				
				float dist = Vector2.Distance(selfActor.current_position, building.current_position);
				if (dist < maxRange && dist > minRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						target = building;
					}
				}
			}
			
			if (enemyCount < requiredEnemies || target == null)
			{
				return false;
			}
			
			// 成功找到目標，施加冷卻並發動攻擊
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			
			// 我們需要一個瓦片(tile)作為流星雨的目標，這裡直接使用目標單位的當前瓦片
			WorldTile targetTile = target.current_tile;
			
			if (targetTile != null)
			{// _00設定 
				float spreadRadius_00 = 2.5f;//隨機散佈半徑
				int numberOfLightning_00 = 3;//生成回數
				for (int i = 0; i < numberOfLightning_00; i++)
				{	// 自己計算隨機的 X 和 Y 座標偏移量
					float offsetX = UnityEngine.Random.Range(-spreadRadius_00, spreadRadius_00);
					float offsetY = UnityEngine.Random.Range(-spreadRadius_00, spreadRadius_00);
					
					// 計算新的隨機位置
					int newTileX = (int)targetTile.pos.x + (int)offsetX;
					int newTileY = (int)targetTile.pos.y + (int)offsetY;

					// === 修正：直接從 World.world 獲取地圖寬高 ===
					// 檢查新位置是否在地圖範圍內
					if (newTileX >= 0 && newTileX < MapBox.width && 
						newTileY >= 0 && newTileY < MapBox.height)
					{	// 根據新的座標獲取瓦片
						WorldTile randomTile = World.world.GetTile(newTileX, newTileY);

						if (randomTile != null)
						{
							// 使用新的隨機瓦片作為雷電的目標
							spawnLightning00(randomTile, 0.25f, selfActor);
						}
					}
				}
			}
			return true;
		}		
		public static bool EnvyPoniardHit03(BaseSimObject pSelf, WorldTile pTile = null)
		{// 魔刃 雷擊 遠 29 ~ 59
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;

			// 必須擁有 'envy_demon_king' 狀態才能發動
			if (!selfActor.hasStatus("envy_demon_king"))
			{
				return false;
			}
			// 冷卻狀態檢查
			string attackCooldownStatus = "item_cdt02";
			float attackCooldownDuration = 0.001f;
			if (selfActor.hasStatus(attackCooldownStatus))
			{
				return false;
			}
			// 尋找目標
			float maxRange = 59f;
			float minRange = 29f;
			int enemyCount = 0;
			int requiredEnemies = 1;
			BaseSimObject target = null;
			float closestDist = float.MaxValue;
			
			// 遍歷所有單位，尋找最接近且符合條件的目標
			// A. 優先遍歷所有單位，尋找最接近且符合條件的目標
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				
				bool isAttackable = false;
				foreach (string traitID in unconvertibleTraits00)
				{
					if (other.hasTrait(traitID))
					{
						isAttackable = true;
						break;
					}
				}
				if (other.getProfession() == UnitProfession.King || other.getProfession() == UnitProfession.Leader)
				{
					isAttackable = true;
				}
				if (!isAttackable)
				{
					continue;
				}
				
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist < maxRange && dist > minRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						target = other;
					}
				}
			}
			
			// B. 如果沒有找到單位，則尋找敵對建築
			foreach (var building in World.world.buildings)
			{
				if (building == null || !building.isAlive())
					continue;
				if (building.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(building.kingdom))
					continue;
				
				float dist = Vector2.Distance(selfActor.current_position, building.current_position);
				if (dist < maxRange && dist > minRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						target = building;
					}
				}
			}
			
			if (enemyCount < requiredEnemies || target == null)
			{
				return false;
			}
			
			// 成功找到目標，施加冷卻並發動攻擊
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			
			// 我們需要一個瓦片(tile)作為雷擊的目標，這裡直接使用目標單位的當前瓦片
			WorldTile targetTile = target.current_tile;
			
			if (targetTile != null)
			{// _00設定 
				float spreadRadius_00 = 5.0f;//隨機散佈半徑
				int numberOfLightning_00 = 9;//生成回數
				for (int i = 0; i < numberOfLightning_00; i++)
				{	// 自己計算隨機的 X 和 Y 座標偏移量
					float offsetX = UnityEngine.Random.Range(-spreadRadius_00, spreadRadius_00);
					float offsetY = UnityEngine.Random.Range(-spreadRadius_00, spreadRadius_00);
					
					// 計算新的隨機位置
					int newTileX = (int)targetTile.pos.x + (int)offsetX;
					int newTileY = (int)targetTile.pos.y + (int)offsetY;

					// === 修正：直接從 World.world 獲取地圖寬高 ===
					// 檢查新位置是否在地圖範圍內
					if (newTileX >= 0 && newTileX < MapBox.width && 
						newTileY >= 0 && newTileY < MapBox.height)
					{	// 根據新的座標獲取瓦片
						WorldTile randomTile = World.world.GetTile(newTileX, newTileY);

						if (randomTile != null)
						{
							// 使用新的隨機瓦片作為雷電的目標
							spawnLightning00(randomTile, 0.50f, selfActor);
						}
					}
				}
			}
			return true;
		}		
		public static void spawnLightning00(WorldTile pTile, float pScale = 0.25f, Actor pActor = null)
		{// 魔刃 雷擊效果 自訂版
			BaseEffect tEffect = EffectsLibrary.spawnAtTile("fx_lightning_big", pTile, pScale);//效果圖層
			if (tEffect == null)
			{
				return;
			}
			int tRadius = (int)(pScale * 25f);//影響範圍
			MapAction.checkLightningAction(pTile.pos, tRadius);
			MapAction.damageWorld(pTile, tRadius, AssetManager.terraform.get("lightning_ex"), pActor);
			tEffect.sprite_renderer.flipX = Randy.randomBool();
			MapAction.checkSantaHit(pTile.pos, tRadius);
			MapAction.checkUFOHit(pTile.pos, tRadius, pActor);
			MapAction.checkTornadoHit(pTile.pos, tRadius);
		}
		public static bool EnvyPoniardAwakens(BaseSimObject pSelf, WorldTile pTile = null)
		{// 魔刃 武器復甦狀態
			// 1. 基本安全檢查：確保 pSelf 及其 Actor 組件存在且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			// 如果單位持有 "envy_demon_king" 狀態
			if (selfActor.hasStatus("envy_demon_king"))
			{
				// 則為其持續添加 "sdk2" 狀態，每次添加時設定持續時間為 10 秒
				// 如果 sdk2 已經存在，此操作會刷新其持續時間為 10 秒
				selfActor.addStatusEffect("edk2", 3600f); // 將時間改為 10 秒
			}
			return false; // 特殊效果通常返回 false，表示不阻止其他效果鏈
		}
		public static bool EvilLawGet07(BaseSimObject pTarget, WorldTile pTile)
		{// 魔刃 給予拾獲者認證狀態
			// 1. 安全檢查：確保 pTarget 存在且有 Actor 組件
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor targetActor = pTarget.a;
			// =======================================================
			// === 新增/修改邏輯：檢查王國內是否已存在特定魔王 ===
			const string DemonKingStatusID = "envy_demon_king";
			Kingdom currentKingdom = targetActor.kingdom;
			// 檢查目標單位是否屬於一個國家
			if (currentKingdom != null)
			{
				// 遍歷王國中的所有單位
				foreach (Actor kingdomUnit in currentKingdom.units)
				{
					// 跳過自己，只檢查其他單位
					if (kingdomUnit == null || kingdomUnit == targetActor)
					{
						continue;
					}
					// 檢查其他單位是否擁有 "???_demon_king" 狀態
					if (kingdomUnit.hasStatus(DemonKingStatusID))
					{
						// 如果找到，則直接返回 false，不執行後續操作
						return false;
					}
				}
			}
			// =======================================================
			// 2. 如果通過了上面的檢查，繼續執行後續邏輯
			// 檢查是否持有 特質
			if (targetActor.hasTrait("evillaw_devour"))
			{
				// 如果單位已經有 特質，就不進行任何動作
				return true; 
			}
			// 3. 如果沒有 特質，則檢查是否持有 狀態效果
			else if (!targetActor.hasStatus("edk3"))
			{
				// 如果都沒有，就添加 狀態效果
				targetActor.addStatusEffect("edk3", 600f);
			}
			return true; // 表示操作成功執行
		}
			#endregion
			#region	不死之冠
		public static bool CrownATK(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 不死之冠 攻擊調用
			// 安全檢查...
			if (pSelf?.a == null || pTarget?.a == null || !pSelf.a.isAlive() || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			const string UndeadEmperorStatus = "ex_undead_emperor";
			if (!selfActor.hasStatus(UndeadEmperorStatus))
			{
				return false;
			}
			GiveSoul(pSelf, pTile);
			return true;
		}
		public static bool UndeadCrownEffect(BaseSimObject pTarget, WorldTile pTile = null)
		{// 不死之冠 征服
			// 確保 pTarget 是有效的 Actor 並且存活 (施法者)
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pTarget.a;
			if (selfActor.subspecies == null || !selfActor.subspecies.hasTrait("prefrontal_cortex") || selfActor.asset.id != "necromancer")
			{
				return false;
			}
			// 檢查施法者是否擁有皇冠狀態，只有擁有狀態才執行轉化
			const string UndeadEmperorStatus = "ex_undead_emperor";
			if (!selfActor.hasStatus(UndeadEmperorStatus))
			{
				return false;
			}
			
			var allClosestUnits = Finder.getUnitsFromChunk(pTile, 60);//60
			if (allClosestUnits.Any())
			{
				foreach (var unit in allClosestUnits)
				{
					// 確保 unit 和 unit.a 有效且存活 (被轉化目標)
					if (unit == null || unit.a == null || !unit.a.isAlive())
					{
						continue;
					}
					Actor targetActor = unit.a; // 使用 targetActor 變數，清晰且安全
					
					// 不應對施法者自己執行效果
					if (targetActor == selfActor)
					{
						continue;
					}
					
					// 核心檢查：目標必須是死靈法師 (修正 unitr 錯誤)
					if (targetActor.asset.id == "necromancer")
					{
						
						if (targetActor.hasTrait("undead_servant3") || targetActor.hasTrait("extraordinary_authority"))
						{// 目標持有指定特質
							continue;
						}
						if (targetActor.army != null)
						{
							targetActor.stopBeingWarrior(); 
						}
                        
						// 王國同步
						if (selfActor.kingdom != null)
						{
							if (targetActor.kingdom != null)
							{
								targetActor.kingdom.units.Remove(targetActor); // 確保從舊王國移除
							}
							targetActor.kingdom = selfActor.kingdom;
							selfActor.kingdom.units.Add(targetActor);
						}
						// 城市同步
						if (selfActor.city != null)
						{
							if (targetActor.city != null)
							{
								if (targetActor.city.kingdom != selfActor.kingdom)
								{
									if(targetActor.isCityLeader())
									{
										Traits01Actions.TraitCityConversion01(selfActor, targetActor);
									}
									else
									{
										targetActor.city = selfActor.city;
									}
								}
								else
								{
									targetActor.city = targetActor.city;
								}
							}
						}
						else // 如果主人沒有城市，僕從也必須清除城市歸屬
						{
							targetActor.city = null;
						}
						// 亞種特質同步 (保持您原有的安全檢查邏輯)
						if (targetActor.subspecies != null)
						{
							if (targetActor.subspecies.hasTrait("advanced_hippocampus"))
							{
								if (selfActor.religion != null)
								{
									targetActor.religion = selfActor.religion;
								}
								if (selfActor.culture != null)
								{
									targetActor.culture = selfActor.culture;
								}
							}
							if (targetActor.subspecies.hasTrait("wernicke_area"))
							{
								if (selfActor.language != null)
								{
									targetActor.language = selfActor.language;
								}
							}
						}
                        
                        // 最終賦值
						targetActor.addTrait("undead_servant3");
						targetActor.removeTrait("slave");
						targetActor.data.health += 9999;
						targetActor.data.set("master_id", selfActor.data.id);
						targetActor.setLover(selfActor); // 轉化為 Lover 關係
                        
						// 更新僕從追隨清單
						if (!listOfTamedBeasts.ContainsKey(targetActor))
							listOfTamedBeasts.Add(targetActor, selfActor);
						else if (listOfTamedBeasts[targetActor] != selfActor)
						{listOfTamedBeasts[targetActor] = selfActor;}
							
					// ====== 核心修改結束 ======
					}
				}
			}
			return true;
		}
		public static bool Anti_Soul(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 不死之冠 持魂殺手
			// 安全檢查...
			if (pSelf?.a == null || pTarget?.a == null || !pSelf.a.isAlive() || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			Actor targetActor = pTarget.a;
			float totalBonusMultiplier = 0f;
			float SetValue = 1.50f;
			bool hasTriggered = false;
			if (targetActor.asset.has_soul || targetActor.hasStatus("soul"))
			{//物種不同的話
				totalBonusMultiplier += SetValue;
				hasTriggered = true;
			}
			// 如果至少有一個屬性觸發了效果，就計算並造成傷害
			if (hasTriggered)
			{
				float selfDamage = selfActor.stats["damage"];
				float finalDamage = selfDamage * totalBonusMultiplier;
				SpecialAttackDamage(pSelf, pTarget, finalDamage, pTile);
				return true;
			}
			return false;
		}
		public static bool Anti_Soul_Defense(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 不死之冠 持魂特防
			// 1. 安全檢查：確保施法者 (防禦者) 和目標 (攻擊者) 都存在且有效
			if (pSelf?.a == null || pTarget?.a == null || !pSelf.a.isAlive() || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a; // 防禦者
			Actor targetActor = pTarget.a; // 攻擊者
			if (!selfActor.hasStatus("ex_undead_emperor") || !selfActor.hasStatus("defense_on"))
			{
				return false;
			}
			// 取得攻擊者對防禦者造成的基礎傷害值
			float damageDealt = targetActor.stats["damage"];
			float totalDamageToHeal = 0f; // 用一個變數來累計總減免傷害
			// 檢查第一個條件：種族是否不同
			if (targetActor.asset.has_soul || targetActor.hasStatus("soul"))
			{				
				totalDamageToHeal += damageDealt * 0.75f;
			}
			// 如果任何一個條件滿足（總減免傷害大於0），才執行生命值恢復
			if (totalDamageToHeal > 0)
			{
				// 直接增加生命值，以抵銷受到的傷害
				selfActor.data.health += (int)totalDamageToHeal;
				// 確保生命值不會超過最大值
				if (selfActor.data.health > selfActor.getMaxHealth())
				{
					selfActor.data.health = selfActor.getMaxHealth();
				}
				return true; // 效果成功發動
			}
			return false; // 沒有條件滿足，效果未發動
		}
		public static bool UndeadRebirth(BaseSimObject pTarget, WorldTile pTile = null)
		{// 不死之冠 蘇生
			// 基礎安全檢查：確保目標單位及其 Actor 組件存在
			if (pTarget == null || pTarget.a == null)
			{
				return false;
			}
			Actor originalActor = pTarget.a; // 將原始 Actor 儲存起來以備後用
			var weaponSlot = originalActor.equipment.getSlot(EquipmentType.Helmet);
			Item equippedItem = weaponSlot.getItem();
			if (!originalActor.hasTrait("extraordinary_authority"))
			{
				return false;
			}
			if (originalActor.subspecies == null || !originalActor.subspecies.hasTrait("prefrontal_cortex"))
			{
				return false; 
			}
			// === 安全檢查結束 ===
			// 1. 移除原始單位 (這裡移除的是舊單位，使其消失)
			ActionLibrary.removeUnit(originalActor); 
			// 2. 特質添加/移除：在複製前清除這些特質，確保新單位是「潔淨」的
			equippedItem.data.favorite = false;
			originalActor.removeTrait("infected");
			originalActor.removeTrait("mush_spores");
			originalActor.removeTrait("tumor_infection");
			originalActor.removeTrait("plague");
			originalActor.removeTrait("death_mark");
			originalActor.removeTrait("skin_burns");
			originalActor.removeTrait("crippled");
			originalActor.removeTrait("eyepatch");
			originalActor.removeTrait("madness");
			originalActor.removeTrait("extraordinary_authority");
			originalActor.removeTrait("other6661");
			originalActor.removeTrait("other6662");
			originalActor.removeTrait("other6663");
			originalActor.removeTrait("other6664");
			originalActor.removeTrait("other6665");
			originalActor.removeTrait("other6666");
			originalActor.removeTrait("other6667");
			originalActor.removeTrait("other6668");
			originalActor.removeTrait("other6669");
			// 3. 播放視覺效果：原始單位位置的生成特效 (此部分根據要求跳過)
			EffectsLibrary.spawn("fx_spawn", originalActor.current_tile, null, null, 0f, -1f, -1f);
			// 4. 創建新的單位 (Reborn!)
			var act = World.world.units.createNewUnit("necromancer", pTile);
			// 5. 複製原始單位數據到新單位
			ActorTool.copyUnitToOtherUnit(originalActor, act);
			// 5.1 移除抽獎特質 (在複製後立即移除，防止新單位觸發不想要的抽選效果)
			if (act.hasTrait("b0001"))
			{
				act.removeTrait("b0001");
				//Debug.Log($"[Bribery] 重生單位 {act.name} 上的誕生抽選特質 B0001 已被立即移除，以防止意外抽獎。");
			}
			// 6.1 設定新單位的王國歸屬
			if (originalActor.kingdom != null && originalActor.kingdom.isAlive()) 
			{
				act.kingdom = originalActor.kingdom;
			}
			// 7. 設定新單位的名稱和收藏狀態
			act.data.name = $"Loser Emperor";
			act.data.favorite = originalActor.data.favorite;
			act.data.health += 99999; 		// 恢復大量生命值
			act.data.age_overgrowth += 10 ; 	// 恢復大量生命值

			// 10. 為新單位施加臨時狀態效果
			act.addStatusEffect("invincible", 5); 
			act.addStatusEffect("antibody", 5);  
			act.addStatusEffect("rebirth", 5);
			act.finishStatusEffect("egg");								//移除狀態
			act.finishStatusEffect("uprooting");						//移除狀態
			if (originalActor.subspecies.hasTrait("prefrontal_cortex"))
			{// 如果施法者有才添加
				act.subspecies.addTrait("prefrontal_cortex");
			}
			if (originalActor.subspecies.hasTrait("advanced_hippocampus"))
			{// 如果施法者有才添加
				act.subspecies.addTrait("advanced_hippocampus");
			}
			if (originalActor.subspecies.hasTrait("wernicke_area"))
			{// 如果施法者有才添加
				act.subspecies.addTrait("wernicke_area");
			}
			if (originalActor.subspecies.hasTrait("amygdala"))
			{// 如果施法者有才添加
				act.subspecies.addTrait("amygdala");
			}

			// 11. 更多酷炫的生成效果
			EffectsLibrary.spawnExplosionWave(pTile.posV3, 1f, 1f); 
			World.world.applyForceOnTile(pTile, 3, 1.5f, pForceOut: true, 0, null, pByWho: act); 
			return true; // 表示效果成功執行
		}
		public static bool GiveSoul(BaseSimObject pSelf, WorldTile pTile)
		{// 不死之冠 授予靈魂
			// 1. 基本安全检查：确保施法者 pSelf 及其 Actor 组件存在且存活
			if (pSelf == null || pSelf.a == null) // pTile 可能為 null，我們將在內部處理
			{
				return false; // 無效的施法者
			}
			Actor selfActor = pSelf.a;
			if (pTile == null)
			{
				return false;
			}
			int range = 20; // 設定影響範圍
			string SoulStatusID = "soul"; // 詛咒的狀態ID
			float SoulDuration = 666f; // 詛咒狀態的持續時間，例如 60 秒 (可調整)
			bool effectAppliedToAnyone = false; // 追蹤是否成功對至少一個目標應用了效果
			// 1. ====== 新增邏輯：檢查並移除施法者自身的 "soul" 狀態效果 ======
			var allClosestUnits = Finder.getUnitsFromChunk(pTile, range);
			// 遍歷範圍內的 Actor
			if (allClosestUnits.Any())
			{
				foreach (var unit in allClosestUnits)
				{
					Actor targetActor = unit.a; // 從 BaseSimObject 獲取 Actor 組件
					// 檢查目標是否是有效的 Actor，存活，且不是施法者本身，**且不屬於同一王國** (關鍵邏輯)
					if (targetActor != null && targetActor.isAlive() && targetActor != selfActor && targetActor.kingdom != selfActor.kingdom)
					{
						if (targetActor.asset.has_soul)
						{				
							continue;
						}
						bool targetAffected = false; // 標記當前目標是否被影響
						// a. 檢查目標是否已經擁有 "soul" 狀態效果
						if (targetActor.hasStatus(SoulStatusID))
						{
							 continue; // 如果不希望刷新持續時間，可以直接 continue
						}
						else 
						{
							targetActor.addStatusEffect(SoulStatusID, SoulDuration); 
							targetAffected = true;
						}

						if (targetAffected)
						{
							effectAppliedToAnyone = true; // 只要有一個目標被應用效果，就設為 true
						}
					}
				}
			}
			return effectAppliedToAnyone; 
		}

			#endregion
		public static bool braveATK(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 勇者戶額
			// 安全檢查...
			if (pSelf?.a == null || pTarget?.a == null || !pSelf.a.isAlive() || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			const string UndeadEmperorStatus = "brave";
			if (!selfActor.hasStatus(UndeadEmperorStatus))
			{
				return false;
			}
			Traits01Actions.Health_recovery(pSelf, pTile);
			Traits01Actions.Mana_recovery(pSelf, pTile);
			Traits01Actions.Stamina_recovery(pSelf, pTile);
			return true;
		}

			#region 其他功能
	//功能性代碼
		public static bool addFavoriteWeaponO(BaseSimObject pTarget, WorldTile pTile)
		{// 自動收藏武器 一般
			// 1. 確保 pTarget 是有效的 Actor 並且存活
			if (pTarget == null || !(pTarget is Actor actor) || !actor.isAlive())
			{
				return false;
			}
			// 2. 獲取單位目前裝備的武器槽位
			// 這是假設您要將主要武器設為最愛，如果目標是其他槽位（例如護甲），則需要修改 EquipmentType
			var weaponSlot = actor.equipment.getSlot(EquipmentType.Weapon);
			// 3. 檢查武器槽位是否被佔用，並且其中有物品
			if (weaponSlot != null && weaponSlot.getItem() != null)
			{	// 4. 獲取槽位中的 Item 實例
				Item equippedItem = weaponSlot.getItem();
				// 5. 將該 Item 實例設置為最愛
				equippedItem.data.favorite = true; // 假設 Item 也有類似 Actor 的 data.favorite 屬性
				return true; // 表示操作成功執行
			}
			// 如果單位沒有裝備武器，則返回 false
			return false;
		}
		public static bool addFavoriteWeapon0(BaseSimObject pTarget, WorldTile pTile)
		{// 自動收藏 篩選 二分
			// 1. 確保 pTarget 是有效的 Actor 並且存活
			if (pTarget == null || !(pTarget is Actor actor) || !actor.isAlive())
			{
				return false;
			}
			// 2. 獲取單位目前裝備的武器槽位
			var weaponSlot = actor.equipment.getSlot(EquipmentType.Weapon);
			// 3. 檢查武器槽位是否被佔用，並且其中有物品
			if (weaponSlot != null && weaponSlot.getItem() != null)
			{
				// 4. 獲取槽位中的 Item 實例
				Item equippedItem = weaponSlot.getItem();
				if (!actor.hasStatus(SevenDemonKingStatus1)) // 如果單位「沒有」該系列狀態
				{
					equippedItem.data.favorite = false;
					weaponSlot.takeAwayItem();
					actor.setStatsDirty();
					return false; 
				}
				else // 不滿足任一條件的話
				{
					equippedItem.data.favorite = true;
					return true;
				}
			}
			// 如果單位沒有裝備武器，則返回 false
			return false;
		}
		public static bool addFavoriteWeaponB(BaseSimObject pTarget, WorldTile pTile)
		{// 自動收藏 篩選 二分
			// 1. 確保 pTarget 是有效的 Actor 並且存活
			if (pTarget == null || !(pTarget is Actor actor) || !actor.isAlive())
			{
				return false;
			}
			// 2. 獲取單位目前裝備的武器槽位
			var weaponSlot = actor.equipment.getSlot(EquipmentType.Helmet);
			// 3. 檢查武器槽位是否被佔用，並且其中有物品
			if (weaponSlot != null && weaponSlot.getItem() != null)
			{
				// 4. 獲取槽位中的 Item 實例
				Item equippedItem = weaponSlot.getItem();
				if (!actor.hasStatus("brave")) // 如果單位「沒有」該狀態
				{
					equippedItem.data.favorite = false;
					actor.data.favorite = false;
					weaponSlot.takeAwayItem();
					actor.setStatsDirty();
					return false; 
				}
				else // 不滿足任一條件的話
				{
					equippedItem.data.favorite = true;
					actor.data.favorite = true;
					return true;
				}
			}
			// 如果單位沒有裝備武器，則返回 false
			return false;
		}
		public static bool addFavoriteWeaponUE(BaseSimObject pTarget, WorldTile pTile)
		{// 自動收藏 篩選 二分
			// 1. 確保 pTarget 是有效的 Actor 並且存活
			if (pTarget == null || !(pTarget is Actor actor) || !actor.isAlive())
			{
				return false;
			}
			// 2. 獲取單位目前裝備的武器槽位
			var weaponSlot = actor.equipment.getSlot(EquipmentType.Helmet);
			// 3. 檢查武器槽位是否被佔用，並且其中有物品
			if (weaponSlot != null && weaponSlot.getItem() != null)
			{
				// 4. 獲取槽位中的 Item 實例
				Item equippedItem = weaponSlot.getItem();
				if (!actor.hasStatus("ex_undead_emperor")) // 如果單位「沒有」該狀態
				{
					equippedItem.data.favorite = false;
					actor.data.favorite = false;
					weaponSlot.takeAwayItem();
					actor.setStatsDirty();
					return false; 
				}
				else // 不滿足任一條件的話
				{
					equippedItem.data.favorite = true;
					actor.data.favorite = true;
					return true;
				}
			}
			// 如果單位沒有裝備武器，則返回 false
			return false;
		}
		public static bool addFavoriteWeapon1(BaseSimObject pTarget, WorldTile pTile)
		{// 自動收藏 篩選 四分
			// 1. 確保 pTarget 是有效的 Actor 並且存活
			if (pTarget == null || !(pTarget is Actor actor) || !actor.isAlive())
			{
				return false;
			}
			// 2. 獲取單位目前裝備的武器槽位
			var weaponSlot = actor.equipment.getSlot(EquipmentType.Weapon);
			// 3. 檢查武器槽位是否被佔用，並且其中有物品
			if (weaponSlot != null && weaponSlot.getItem() != null)
			{
				// 4. 獲取槽位中的 Item 實例
				Item equippedItem = weaponSlot.getItem();
				if (!actor.subspecies.hasTrait("prefrontal_cortex") || actor.hasTrait("slave") || actor.hasTrait("undead_servant") || actor.hasTrait("undead_servant2"))
				{
					equippedItem.data.favorite = false;
					weaponSlot.takeAwayItem();
					actor.setStatsDirty();
					WeaponAddDamage(pTarget, pTile); // 對持有武器的單位造成傷害
					// 返回 false 表示這次「收藏」操作最終沒有成功（因為被取消了），且觸發了懲罰 SevenDemonKingTrait
					return false; 
				}
				else if (!actor.subspecies.hasTrait("prefrontal_cortex")  && actor.hasStatus(SevenDemonKingStatus1))
				{
					equippedItem.data.favorite = false;
					weaponSlot.takeAwayItem();
					actor.setStatsDirty();
					WeaponAddDamage(pTarget, pTile); // 對持有武器的單位造成傷害
					return false; 
				}
				else if (!actor.hasStatus(SevenDemonKingStatus1))
				{
					equippedItem.data.favorite = false;
					weaponSlot.takeAwayItem();
					actor.setStatsDirty();
					return false; 
				}
				else // 不滿足任一條件的話
				{
					equippedItem.data.favorite = true;
					return true;
				}
			}
			// 如果單位沒有裝備武器，則返回 false
			return false;
		}
/*		public static bool addFavoriteWeapon1(BaseSimObject pTarget, WorldTile pTile)
		{// 自動收藏 篩選 四分
			// 1. 確保 pTarget 是有效的 Actor 並且存活
			if (pTarget == null || !(pTarget is Actor actor) || !actor.isAlive())
			{
				return false;
			}
			// 2. 獲取單位目前裝備的武器槽位
			var weaponSlot = actor.equipment.getSlot(EquipmentType.Weapon);
			// 3. 檢查武器槽位是否被佔用，並且其中有物品
			if (weaponSlot != null && weaponSlot.getItem() != null)
			{
				// 4. 獲取槽位中的 Item 實例
				Item equippedItem = weaponSlot.getItem();
				
				// --- 邏輯判斷開始 ---
				// 處理非魔王且無心智或為奴隸的情況
				if ((!actor.subspecies.hasTrait("prefrontal_cortex") || actor.hasTrait("slave"))
					&& !SevenDemonKingTrait.Any(t => actor.hasTrait(t))) // 排除魔王特質
				{
					equippedItem.data.favorite = false;
					weaponSlot.takeAwayItem();
					actor.setStatsDirty();
					WeaponAddDamage(pTarget, pTile); // 對持有武器的單位造成傷害
					return false;
				}
				// 處理淪為野獸的魔王
				else if (!actor.subspecies.hasTrait("prefrontal_cortex") && actor.hasStatus(SevenDemonKingStatus1))
				{
					equippedItem.data.favorite = false;
					weaponSlot.takeAwayItem();
					actor.setStatsDirty();
					WeaponAddDamage(pTarget, pTile); // 對持有武器的單位造成傷害
					return false;
				}
				// 處理非魔王狀態下持有魔王武器的情況
				else if (!actor.hasStatus(SevenDemonKingStatus1))
				{
					equippedItem.data.favorite = false;
					weaponSlot.takeAwayItem();
					actor.setStatsDirty();
					return false;
				}
				// 處理真正被認證的魔王
				else
				{
					equippedItem.data.favorite = true;
					return true;
				}
			}
			// 如果單位沒有裝備武器，則返回 false
			return false;
		}
*/
		private static readonly HashSet<string> SevenDemonKingStatus1 = new HashSet<string>
		{// 七魔王狀態
			"arrogant_demon_king",
			"greedy_demon_king",
			"lust_demon_king",
			"wrath_demon_king",
			"gluttony_demon_king",
			"sloth_demon_king",
			"envy_demon_king",
			"ex_undead_emperor",
		};
		private static readonly HashSet<string> SevenDemonKingTrait = new HashSet<string>
		{// 七魔王候補(特質)
			"evillaw_ew",
			"evillaw_tantrum",
			"evillaw_seduction",
			"evillaw_sleeping",
			"evillaw_moneylaw",
			"evillaw_starvation",
			"evillaw_devour",
		};
		private static readonly HashSet<string> SevenDemonKingStatus2 = new HashSet<string>
		{// 武器覺醒
			"adk2",
			"gdk3",
			"ldk2",
			"wdk2",
			"gldk2",
			"sdk2",
			"edk2",
		};
		private static readonly HashSet<string> unconvertibleTraits00 = new HashSet<string>
		{// 誘惑法 不可誘惑 的 特質清單
			"apostle",		 		// 使徒
			"slave",		 		// 奴隸
			"undead_servant2",		// 奴隸
			"undead_servant",		// 奴隸
			"evillaw_ew",			// 滅智 (傲慢)
			"evillaw_tantrum",		// 忿怒 (忿怒)
			"evillaw_seduction",	// 誘惑 (色慾)
			"evillaw_sleeping",		// 睡眠 (怠惰)
			"evillaw_moneylaw",		// 金錢 (強欲)
			"evillaw_starvation",	// 餓食 (暴食)
			"evillaw_devour",		// 吞噬 (嫉妒)
			"pro_king",				// 國王
			"pro_leader",			// 領主
			"holyarts_bond",		// 絆
			"holyarts_justice",		// 裁決
			"strong_minded",		// 原版特質
			"desire_alien_mold",	// 原版特質
			"desire_computer",		// 原版特質
			"desire_golden_egg",	// 原版特質
			"desire_harp",			// 原版特質
			"madness",				// 原版特質
			"psychopath",			// 原版特質
			// 如果有其他您不希望被轉化的特質，請添加在這裡
		};
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
		public static bool WeaponAddDamage(BaseSimObject pTarget, WorldTile pTile = null)
		{// 傷害給予
			if (pTarget == null || pTarget.a == null)
			{
				return false; // 如果目標無效，直接返回
			}
			Actor targetActor = pTarget.a;
			int damageAmount = targetActor.getHealth(); 
			if (damageAmount < 10)
			{
				damageAmount = 10;
			}
			targetActor.getHit((float)damageAmount, true, AttackType.None, null, false, false, true);
			return true; // 表示操作成功執行
		}
			#endregion
	}
}
