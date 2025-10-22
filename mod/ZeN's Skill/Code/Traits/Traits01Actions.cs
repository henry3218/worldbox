
using System;
using System.Threading;
using System.Linq;
using System.Text;
//using System.Numerics;
using System.Collections.Generic;
using System.Reflection;
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
using System.IO;
using System.Threading.Tasks;
using ai.behaviours;
using db;
//Alt+3
namespace ZeN_01
{
	class Traits01Actions
	{
		private static Dictionary<Actor, Actor> listOfTamedBeasts = new Dictionary<Actor, Actor>();
		private static Dictionary<ActorData, Actor> listOfTamedBeastsData = new Dictionary<ActorData, Actor>();
		// 建议：将 System.Random 实例声明为静态字段，避免频繁创建
		//private static System.Random _random = new System.Random();
		private static readonly System.Random _random = new System.Random();
			#region 測試項目
/*		public static bool DivineAuraAction(BaseSimObject pSelf, WorldTile pTile = null)
{// 聖光靈氣效果
	if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
		return false;
	Actor selfActor = pSelf.a;
	const int AURA_RADIUS = 20; // 靈氣半徑
	const int DAMAGE_PER_TICK = 100; // 基礎傷害
	bool effectApplied = false;
	WorldTile centerTile = pTile ?? pSelf.current_tile;
	// 核心邏輯：遍歷範圍內所有單位
	World.world.loopWithBrush(centerTile,
	Brush.get(AURA_RADIUS, "circ_"),
	delegate (WorldTile pTileInBrush) // Code\Traits\Traits01Actions.cs(47,5): error CS1593: Delegate 'PowerAction' does not take 1 arguments
	{
		pTileInBrush.doUnits(delegate (Actor pTarget)
		{
			if (pTarget == null || pTarget == selfActor || !pTarget.isAlive())
				return;
			
			// 條件檢查：目標是否為不死生物或惡魔
			bool isTargetUndeadOrDemon = pTarget.asset.id == "ghost" || 
										 pTarget.asset.id == "skeleton" ||
										 pTarget.asset.id == "demon" ||
										 pTarget.hasTrait("undead_servant") ||
										 pTarget.hasTrait("undead_servant2") ||
										 pTarget.hasTrait("evil"); // 廣泛識別邪惡單位

			if (isTargetUndeadOrDemon)
			{
				// 1. 【弱化效果】：清除黑暗賜福狀態 (針對不死族)
				pTarget.finishStatusEffect("darkblessing");
				// 2. 【淨化效果】：強制移除僕從特質
				//pTarget.removeTrait("undead_servant");
				//pTarget.removeTrait("undead_servant2");
				// 3. 【神聖傷害】：造成固定傷害 (無視護甲，類似 Divine Attack)
				// 使用 getHit 造成 Divine 攻擊
				pTarget.getHit(DAMAGE_PER_TICK, true, AttackType.Divine, selfActor, true, false, true);
				// 4. 【額外視覺和聽覺效果】
				pTarget.startColorEffect(ActorColorEffect.White);
				World.world.fx.add(pTarget.current_position, "fx_light_damage"); // 增加一個光效

				effectApplied = true;
			}
		});
		return true;
	},
	null);
	// 如果至少對一個單位造成了影響，觸發視覺效果
	if (effectApplied)
	{
		// 可以調用您複製的 divineLightFX 作為光環釋放的視覺回饋
		Traits01Actions.divineLightFX(centerTile, "DivineAura");
	}
	return effectApplied;
}
*/
		public static bool SpecialAttack(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 特別攻擊效果
			// 安全檢查：確保施法者和目標都存在且有效
			if (pSelf?.a == null || pTarget?.a == null || !pSelf.a.isAlive() || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			Actor targetActor = pTarget.a;
			// 檢查目標的幸福值是否比施法者高
			if (targetActor.hasTrait("evil"))
			{
				// 取得施法者的傷害值
				float selfDamage = selfActor.stats["damage"];
				// 施加額外傷害 (例如額外造成 50% 傷害)
				targetActor.getHit(selfDamage * 0.5f, true, AttackType.Weapon, selfActor, false, false);
				return true;
			}
			return false;
		}
		public static bool SpecialDefense(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 特別減傷效果
			// 1. 安全檢查：確保施法者 (防禦者) 和目標 (攻擊者) 都存在且有效
			if (pSelf?.a == null || pTarget?.a == null || !pSelf.a.isAlive() || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a; // 防禦者
			Actor targetActor = pTarget.a; // 攻擊者
			// 2. 檢查攻擊者是否擁有「邪惡」特質
			if (targetActor.hasTrait("evil"))
			{				
				// 取得攻擊者對防禦者造成的傷害值
				float damageDealt = targetActor.stats["damage"];
				// 你可以根據需要調整這個百分比
				float damageToHeal = damageDealt * 0.99f;
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
			#endregion
			#region 職位能力
	//國王效果 King
		public static bool KingEffect(BaseSimObject pTarget, WorldTile pTile)
		{// 帝王 King 就職 (強化版：確保國王身份，並處理潛在的 Leader 身份衝突)
			if (pTarget == null || pTarget.a == null)
			{
				// Debug.Log("KingEffect: 目標無效。");
				return false;
			}
			Actor actor = pTarget.a;
			Kingdom kingdom = actor.kingdom;
			if (actor.city == null)
			{
				return false;
			}
			// 1. 基礎檢查：確保單位持有 "pro_king" 特質才執行後續國王邏輯
			if (!actor.hasTrait("pro_king"))
			{
				// Debug.LogWarning($"KingEffect: Actor {actor.getName()} does not have 'pro_king' trait. Skipping.");
				return false;
			}
			// 2. 王國和國王空缺檢查：確保單位屬於一個王國，並且國王職位空缺 (無國王或國王已死亡)
			if (kingdom == null)
			{
				// Debug.LogWarning($"KingEffect: Actor {actor.getName()} is not part of a kingdom. Cannot assign king.");
				return false;
			}
			if (kingdom.king != null && kingdom.king.isAlive())
			{
				// Debug.Log($"KingEffect: Kingdom {kingdom.name} already has an active king ({kingdom.king.getName()}). Cannot assign new king.");
				return false; // 如果當前國王仍在世，則不執行指派
			}

			// 3. 處理潛在的舊國王身份 (如果當前王國有死亡的國王引用，先清理)
			// 雖然上面已檢查 alive，但此處可作為防禦性清理
			if (kingdom.king != null)
			{
				kingdom.king.setProfession(UnitProfession.Unit); // 重置舊國王的職業
				// Debug.Log($"KingEffect: Reset profession of old king {kingdom.king.getName()} to Unit.");
			}

			// 4. 處理單位潛在的 Leader 身份衝突
			// 如果這個單位目前是某個城市的領袖 (Leader)，則先將其從領袖職位移除
			if (actor.isCityLeader())
			{
				actor.city.removeLeader();
				// Debug.Log($"KingEffect: Actor {actor.getName()} was a city leader, removed from city {actor.city.name}.");
			}

			// 5. 將單位設定為國王
			kingdom.king = actor; // 直接設定王國的國王
			actor.setProfession(UnitProfession.King); // 設置角色的職業為國王
			WorldLog.logNewKing(kingdom);
			// 6. 視覺效果和反饋 (可選，從 KingAssignationAction 借鑒)
			actor.startShake();
			actor.startColorEffect();
			
			//Debug.Log($"KingEffect: 單位 {actor.getName()} 已成功就任 {kingdom.name} 的國王。");
			return true;
		}
		public static bool ContinuousKingBuffEffect1(BaseSimObject pTarget, WorldTile pTile)
		{// 帝王 King 思維輪轉 (理政能力)
			if (pTarget == null || pTarget.a == null)
				return false;
			Actor actor = pTarget.a;
			if (!actor.hasTrait("pro_king"))
			{
				// //Debug.LogWarning($"KingEffect called on actor {actor.getName()} without pro_king trait. Skipping.");
				return false; 
			}
			// 检查：只有当是国王时才继续添加效果
			if (actor.getProfession() != UnitProfession.King) // 使用 getProfession()
			{
				// 如果不再是国王，移除控制器状态，停止循环
				actor.finishStatusEffect("Status_KingBuffController1");
				return false; // 不再执行
			}
			// 定义国王状态效果ID列表
			List<string> kingStatusEffects = new List<string>
			{//人格狀態
				"king_effect1",		//軍事
				"king_effect2", 	//管理
				"king_effect3", 	//外交
				"king_effect4"  	//理性
			};
			if (kingStatusEffects.Count == 0)
			{
				//Debug.LogWarning("No king status effects defined for continuous buff.");
				return false;
			}
			// 随机选择一个状态效果ID
			int randomIndex = _random.Next(0, kingStatusEffects.Count); // 使用静态 _random 实例
			string selectedStatusId = kingStatusEffects[randomIndex];
			StatusAsset selectedStatus = AssetManager.status.get(selectedStatusId);
			if (selectedStatus != null)
			{
				// --- 新增检查：如果单位已经持有该状态，则不再添加 ---
				if (!actor.hasStatus(selectedStatus.id)) 
				{
					actor.addStatusEffect(selectedStatus.id);
					//Debug.LogWarning($"King {actor.getName()} received a new continuous buff: {selectedStatus.id}");
				}
				else
				{
					//Debug.LogWarning($"King {actor.getName()} already has {selectedStatus.id}. Not adding again.");
				}
			}
			else
			{
				 //Debug.LogError($"Failed to add continuous king status effect: {selectedStatusId}. StatusAsset not found.");
			}
			// 关键点：重新添加自身来形成循环
			// 确保 Status_KingBuffController 的 duration 设置合理，
			// 这样每次触发后，它会在短時間後再次觸發自身。
			actor.addStatusEffect("Status_KingBuffController1", 3600f); 
			return true;
		}
		public static bool ContinuousKingBuffEffect2(BaseSimObject pTarget, WorldTile pTile)
		{// 帝王 King 思維輪轉 (國際觀感)
			if (pTarget == null || pTarget.a == null)
				return false;
			Actor actor = pTarget.a;
			if (!actor.hasTrait("pro_king"))
			{
				//Debug.LogWarning($"KingEffect called on actor {actor.getName()} without pro_king trait. Skipping.");
				return false; 
			}
			// 检查：只有当是国王时才继续添加效果
			if (actor.getProfession() != UnitProfession.King) // 使用 getProfession()
			{
				// 如果不再是国王，移除控制器状态，停止循环
				actor.finishStatusEffect("Status_KingBuffController2");
				return false; // 不再执行
			}
			// 定义国王状态效果ID列表
			List<string> kingOpinionEffects = new List<string>
			{// 觀感狀態
				"king_effect5",		//正評價
				"king_effect6", 	//負評價
				"no_feeling01" 		//無感
			};
			if (kingOpinionEffects.Count == 0)
			{
				//Debug.LogWarning("No king status effects defined for continuous buff.");
				return false;
			}
			// 随机选择一个状态效果ID
			int randomIndex = _random.Next(0, kingOpinionEffects.Count); // 使用静态 _random 实例
			string selectedStatusId = kingOpinionEffects[randomIndex];
			StatusAsset selectedStatus = AssetManager.status.get(selectedStatusId);
			if (selectedStatus != null)
			{
				// --- 新增检查：如果单位已经持有该状态，则不再添加 ---
				if (!actor.hasStatus(selectedStatus.id)) 
				{
					actor.addStatusEffect(selectedStatus.id);
					//Debug.LogWarning($"King {actor.getName()} received a new continuous buff: {selectedStatus.id}");
				}
				else
				{
					//Debug.LogWarning($"King {actor.getName()} already has {selectedStatus.id}. Not adding again.");
				}
			}
			else
			{
				 //Debug.LogError($"Failed to add continuous king status effect: {selectedStatusId}. StatusAsset not found.");
			}
			actor.addStatusEffect("Status_KingBuffController2", 1800f); 
			return true;
		}
		public static bool ConferringLeader(BaseSimObject pTarget, WorldTile pTile)
		{//	帝王 授予特質
			// 1. 確保 pTarget 是有效的 Actor 並且存活
			if (pTarget == null || !(pTarget is Actor kingActor) || !kingActor.isAlive())
			{
				return false;
			}
			// 2. 檢查單位是否為國王
			if (!kingActor.isKing()) 
			// 如果是方法，請改為 if (!kingActor.isKing())
			{
				return false; // 如果不是國王，則不執行後續效果
			}
			// 3. 獲取國王所屬的王國
			Kingdom kingdom = kingActor.kingdom;
			if (kingdom == null)
			{
				return false; // 國王沒有所屬王國，不執行
			}
			// 4. 遍歷王國下的所有村莊 (City 類在 WorldBox 中通常代表村莊/城市)
			if (kingdom.cities != null)
			{
				foreach (City city in kingdom.cities)
				{
					if (city == null) continue;

					// 5. 獲取村莊的領袖
					Actor leader = city.leader; // 假設 leader 是一個 Actor 屬性
					// 如果是方法，請改為 Actor leader = city.getLeader();

					if (leader != null && leader.isAlive() && leader != kingActor) // 確保領袖存在、存活且不是國王本人
					{
						// 6. 給村莊領袖添加特質
						// !!! 重要：請將 "YourNewLeaderTraitID" 替換為您想要添加的實際特質 ID !!!
						string traitToAdd = "pro_leader"; 
						if (!leader.hasTrait(traitToAdd)) // 避免重複添加
						{
							leader.addTrait(traitToAdd);
							// 您可以在這裡添加一些調試輸出，例如：
							// Debug.Log($"Added trait '{traitToAdd}' to village leader: {leader.name}");
						}
					}
				}
			}
			return true; // 效果成功執行
		}
		public static bool addFavorite_K(BaseSimObject pTarget, WorldTile pTile)
		{// 當單位職業是國王時，將其添加到最愛；否則不添加。
			// 確保 pTarget 是有效的 Actor 並且存活
			if (pTarget == null || !(pTarget is Actor actor) || !actor.isAlive())
			{
				return false;
			}

			// --- 使用 getProfession() 進行國王判斷 ---
			// 獲取單位當前的職業
			UnitProfession currentProfession = actor.getProfession();

			// 判斷單位是否是國王
			if (currentProfession == UnitProfession.King)
			{
				// 如果單位是國王，則將其設置為最愛
				actor.data.favorite = true;
				//Debug.Log($"{actor.name} 的職業是國王，已添加到最愛。");
				return true; // 成功設置為最愛
			}
			else
			{
				// 如果單位不是國王，則不添加最愛
				actor.data.favorite = false;
				// 如果您希望非國王單位明確地從最愛中移除
			   // Debug.Log($"{actor.name} 的職業是 {currentProfession} (不是國王)，未添加到最愛。");
				return false; // 未設置為最愛
			}
			// --- 國王判斷邏輯結束 ---
		}
		public static bool removeTraitK(BaseSimObject pTarget, WorldTile pTile)
		{// 移除特質 模板
			if (pTarget.a != null)
			{
			pTarget.a.removeTrait("pro_leader");
			}
			return true;
		}
	//領主效果 Leader
		public static bool LeaderEffect(BaseSimObject pTarget, WorldTile pTile)
		{// 領主 Leader 就職 (強化版：避免與國王身份衝突)
			if (pTarget == null || pTarget.a == null) // 確保 pTarget 物件及其 Actor 組件存在
			{
				return false;
			}

			Actor actor = pTarget.a; // 確保 pTarget.a 是 Actor 類型

			// 1. 檢查是否持有 "pro_leader" 特質
			if (!actor.hasTrait("pro_leader"))
			{
				// 如果沒有 pro_leader 特質，則此效果不應繼續
				return false; 
			}

			// 2. **關鍵新增：如果單位已經是國王，則不再將其設為領主**
			//	這將阻止已晉升為國王的單位被拉回領主身份
			if (actor.isKing()) // WorldBox 內建方法，判斷是否為國王
			{
				Debug.Log($"[LeaderEffect] {actor.name} 已經是國王，不再將其設定為領主。");
				return false; // 單位已是國王，此效果不再需要將其設為領主
			}

			// 3. 確保單位有城市
			if (actor.city != null) 
			{
				// 4. 檢查城市目前的領導者是否持有 "pro_leader" 特質
				//	只有當城市沒有領袖，或者現有領袖不是 pro_leader 時，才嘗試設定
				if (actor.city.leader == null || !actor.city.leader.hasTrait("pro_leader"))
				{
					// 將 pTarget 角色設為城市的領導者
					actor.city.leader = actor;
					actor.setProfession(UnitProfession.Leader); // 設定角色的職業為領導者
					actor.city.data.leaderID = actor.data.id; // 更新城市數據中的領導者 ID
					//Debug.Log($"[LeaderEffect] {actor.name} 已被設定為城市 {actor.city.name} 的領主。");
				}
				return true; // 返回 true 表示操作成功
			}
			
			return false; // 返回 false 表示操作未成功 (無城市或不符合條件)
		}
		public static bool ContinuousLeaderBuffEffect1(BaseSimObject pTarget, WorldTile pTile)
		{// 領主 Leader 思維輪轉 (理政能力)
			if (pTarget == null || pTarget.a == null)
				return false;
			
			Actor actor = pTarget.a;

			// 檢查：如果沒有 "pro_leader" 特質，則停止
			if (!actor.hasTrait("pro_leader")) // 注意：這裡的特質 ID 有空格
			{
				// Debug.LogWarning($"LeaderEffect called on actor {actor.getName()} without Pro Leader trait. Skipping.");
				return false; 
			}

			// 檢查：只有當是領主时才繼續添加效果
			if (actor.getProfession() != UnitProfession.Leader) // 使用 getProfession()
			{
				// 如果不再是領主，移除控制器状态，停止循環
				actor.finishStatusEffect("Status_LeaderBuffController1"); // 注意：這裡的狀態 ID 有空格
				return false; // 不再執行
			}

			// 定義領主狀態效果ID列表
			List<string> LeaderStatusEffects = new List<string>
			{//人格狀態
				"leader_effect1",	//軍事
				"leader_effect2",	//管理
				"leader_effect3",	//外交
				"leader_effect4"	//理性
			};

			if (LeaderStatusEffects.Count == 0)
			{
				// Debug.LogWarning("No Leader status effects defined for continuous buff.");
				return false;
			}

			// 隨機選擇一個狀態效果ID
			// 使用靜態 _random 實例
			int randomIndex = _random.Next(0, LeaderStatusEffects.Count); 
			string selectedStatusId = LeaderStatusEffects[randomIndex];

			StatusAsset selectedStatus = AssetManager.status.get(selectedStatusId);

			if (selectedStatus != null)
			{
				// --- 新增檢查：如果單位已經持有該狀態，則不再添加 ---
				if (!actor.hasStatus(selectedStatus.id)) 
				{
					actor.addStatusEffect(selectedStatus.id);
					// Debug.LogInfo($"Leader {actor.getName()} received a new continuous buff: {selectedStatus.id}");
				}
				// else
				// {
				//	// Debug.LogInfo($"Leader {actor.getName()} already has {selectedStatus.id}. Not adding again.");
				// }
			}
			else
			{
				// Debug.LogError($"Failed to add continuous Leader status effect: {selectedStatusId}. StatusAsset not found.");
			}

			// 關鍵點：重新添加自身來形成循環
			// 確保 Status LeaderBuffController 的 duration 設置合理，
			// 這樣每次觸發後，它會在短時間後再次觸發自身。
			actor.addStatusEffect("Status_LeaderBuffController1", 1800f); 

			return true;
		}
		public static bool ContinuousLeaderBuffEffect2(BaseSimObject pTarget, WorldTile pTile)
		{// 領主 Leader 思維輪轉 (忠 誠 心)
			if (pTarget == null || pTarget.a == null)
				return false;
			
			Actor actor = pTarget.a;

			// 檢查：如果沒有 "pro_leader" 特質，則停止
			if (!actor.hasTrait("pro_leader"))
			{
				return false;
			}

			// 檢查：只有當是領主时才繼續添加效果
			if (actor.getProfession() != UnitProfession.Leader)
			{
				actor.finishStatusEffect("Status_LeaderBuffController2");
				return false;
			}

			// --- 新增邏輯：如果單位是首都的領主，則直接給予忠誠效果 ---
			// 檢查單位是否有城市，並且該城市是其王國的首都
			if (actor.city != null && actor.city == actor.city.kingdom.capital)
			{
				// 直接添加忠誠狀態，並確保沒有不忠狀態
				if (!actor.hasStatus("leader_effect5"))
				{
					actor.addStatusEffect("leader_effect5");
				}
				// 移除不忠狀態和無感狀態，因為他現在是首都領主了
				actor.finishStatusEffect("leader_effect6");
				actor.finishStatusEffect("no_feeling02");
			}
			else
			{
				// --- 舊的隨機選擇邏輯（只有在不是首都領主時才執行）---
				// 定義領主狀態效果ID列表
				List<string> LeaderLoyaltyEffects = new List<string>
				{//忠誠狀態
					"leader_effect5",	//忠誠
					"leader_effect6",	//不忠
					"no_feeling02"		//無感
				};

				if (LeaderLoyaltyEffects.Count == 0)
				{
					return false;
				}

				int randomIndex = _random.Next(0, LeaderLoyaltyEffects.Count); 
				string selectedStatusId = LeaderLoyaltyEffects[randomIndex];

				StatusAsset selectedStatus = AssetManager.status.get(selectedStatusId);

				if (selectedStatus != null)
				{
					if (!actor.hasStatus(selectedStatus.id)) 
					{
						actor.addStatusEffect(selectedStatus.id);
					}
				}
			}

			// 關鍵點：重新添加自身來形成循環
			actor.addStatusEffect("Status_LeaderBuffController2", 900f); 

			return true;
		}
		public static bool AscendTheThrone(BaseSimObject pTarget, WorldTile pTile)
		{// 領主 登基 (當單位成為國王時，添加 pro_king 特質並移除 pro_leader 特質)
			// 1. 確保 pTarget 是有效的 Actor 並且存活
			if (pTarget == null || !(pTarget is Actor actor) || !actor.isAlive())
			{
				//Debug.Log($"AscendTheThrone: 目標無效或已死亡，操作未執行。");
				return false;
			}
			// 2. 檢查該單位是否為國王
			// 使用 actor.isKing() 方法來判斷 (如果遊戲版本支持此便捷方法)
			// 如果不支持，則需要改回 actor.getProfession() == UnitProfession.King
			if (actor.isKing()) // 如果 actor 已經是國王
			{
				// 3. 為其添加 "pro_king" 特質
				string proKingTraitID = "pro_king"; // 您定義的國王特質 ID
				if (!actor.hasTrait(proKingTraitID)) // 避免重複添加
				{
					actor.addTrait(proKingTraitID);
					//Debug.Log($"AscendTheThrone: 單位 {actor.name} 登基成為國王，並獲得特質: {proKingTraitID}");
				}

				// 4. 追加功能：移除自身的 "pro_leader" 特質
				string proLeaderTraitID = "pro_leader"; // 領主特質 ID (即觸發此效果的特質)
				if (actor.hasTrait(proLeaderTraitID)) // 檢查單位是否確實持有該特質
				{
					actor.removeTrait(proLeaderTraitID);
					//Debug.Log($"AscendTheThrone: 單位 {actor.name} 移除特質: {proLeaderTraitID} (完成登基轉換)。");
				}
				
				return true; // 操作成功
			}

			//Debug.Log($"AscendTheThrone: 單位 {actor.name} 不是國王，操作未執行。");
			return false; // 如果不是國王，則不執行此效果
		}
		public static bool addFavorite_L(BaseSimObject pTarget, WorldTile pTile)
		{// 當單位職業是領主時，將其添加到最愛；否則不添加。
			// 確保 pTarget 是有效的 Actor 並且存活
			if (pTarget == null || !(pTarget is Actor actor) || !actor.isAlive())
			{
				return false;
			}

			// --- 使用 getProfession() 進行國王判斷 ---
			// 獲取單位當前的職業
			UnitProfession currentProfession = actor.getProfession();

			// 判斷單位是否是Leader
			if (currentProfession == UnitProfession.Leader)
			{
				// 如果單位是Leader，則將其設置為最愛
				actor.data.favorite = true;
				//Debug.Log($"{actor.name} 的職業是領主，已添加到最愛。");
				return true; // 成功設置為最愛
			}
			else
			{
				// 如果單位不是Leader，則不添加最愛
				actor.data.favorite = false;
				// 如果您希望非Leader單位明確地從最愛中移除
				//Debug.Log($"{actor.name} 的職業是 {currentProfession} (不是領主)，未添加到最愛。");
				return false; // 未設置為最愛
			}
			// --- Leader判斷邏輯結束 ---
		}
		public static bool Appointment(BaseSimObject pTarget, WorldTile pTile)
		{// 領主 任命隊長
			// 檢查目標是否為領主或國王，如果不是，就什麼都不做並結束
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive() || (pTarget.a.getProfession() != UnitProfession.Leader && pTarget.a.getProfession() != UnitProfession.King))
			{
				return false;
			}
			Actor leader = pTarget.a;
			City city = leader.city;
			// 領主必須屬於一個城市
			if (city == null)
			{
				return false;
			}
			Army leaderCityArmy = null;
			// 獲取城市的軍隊，需要遍歷所有軍隊來尋找
			foreach (Army army in World.world.armies.list)
			{
				if (army.getCity() == city)
				{
					leaderCityArmy = army;
					break;
				}
			}
			// 檢查領主城市的軍隊是否存在
			if (leaderCityArmy == null)
			{
				// 如果城市沒有軍隊，需要檢查並移除可能的特質
				foreach (Actor cityUnit in city.units)
				{
					if (cityUnit.hasTrait("pro_groupleader"))
					{
						cityUnit.removeTrait("pro_groupleader");
					}
				}
				return false;
			}
			// 檢查軍隊是否有隊長
			if (leaderCityArmy.hasCaptain())
			{
				Actor captain = leaderCityArmy._captain;
				// 確保隊長存活且有效
				if (captain != null && captain.isAlive())
				{
					// 如果隊長單位沒有 "pro_groupleader" 特質，則添加它
					if (!captain.hasTrait("pro_groupleader"))
					{
						captain.addTrait("pro_groupleader");
					}
					// 同時，確保移除任何其他單位身上可能有的這個特質（防止舊隊長特質殘留）
					foreach (Actor cityUnit in city.units)
					{
						if (cityUnit != captain && cityUnit.hasTrait("pro_groupleader"))
						{
							cityUnit.removeTrait("pro_groupleader");
						}
					}
				}
			}
			else
			{
				// 如果城市沒有隊長，則從城市內的每個單位身上移除其 "pro_groupleader" 特質
				foreach (Actor cityUnit in city.units)
				{
					if (cityUnit.hasTrait("pro_groupleader"))
					{
						cityUnit.removeTrait("pro_groupleader");
					}
				}
			}
			return true;
		}
	//隊長 Captain 
		public static bool GroupLeaderEffect(BaseSimObject pTarget, WorldTile pTile = null)
		{// 軍隊長 Captain 就職
			// 1. 基本安全检查：确保目标存在、是活着的Actor
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
				return false;
			Actor actor = pTarget.a;
			if (actor.isKing() || actor.getProfession() == UnitProfession.Leader) // WorldBox 內建方法，判斷是否為國王
			{
				return false; // 單位已是國王，此效果不再需要將其轉職
			}
			// 2. 确保指挥官是战士职业
			if (!actor.isProfession(UnitProfession.Warrior))
			{
				actor.setProfession(UnitProfession.Warrior); // 设置为战士
				// 尝试设置其为军队队长 (如果它不是且属于一个军队)
				if (!actor.is_army_captain && actor.army != null)
				{
					actor.army.setCaptain(actor);
				}
			}
			// 設定角色的 AI 工作為 "attacker"
			var pAI = (AiSystemActor)Reflection.GetField(typeof(Actor), actor, "ai");
			if (pAI != null) // 確保 AI 系統存在
			{
				pAI.setJob("attacker");
			}
			return true; // 效果成功执行
		}
		public static bool GroupLeaderEffect2(BaseSimObject pTarget, WorldTile pTile = null)
		{// 軍隊長 Captain 招募
			// 1. 基本安全检查：确保目标存在、是活着的Actor
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
				return false;
			Actor actor = pTarget.a;
			// 3. 检查城市军队容量
			var city = pTarget.getCity(); // 获取目标单位所在的城市
			if (city == null || city.isArmyFull() || city.isArmyOverLimit()) // 检查军队是否已满或超限
				return false; // 如果满了或超限，则不继续招募
			// 4. 尝试将附近的盟友转换为战士 (有随机概率)
			if (Randy.randomChance(0.8f)) // 80% 的概率触发此招募逻辑
			{
				var nearbyUnits = Finder.getUnitsFromChunk(pTile, 5);
				foreach (var unit in nearbyUnits)
				{
					// 确保是活着的Actor，且与指挥官是同一王国
					if (unit.a != null && unit.a.isAlive() && unit.a.kingdom == actor.kingdom)
					{
						// 不要改变国王或领主的职业
						if (!unit.a.isProfession(UnitProfession.King) && !unit.a.isProfession(UnitProfession.Leader))
						{
							unit.a.setProfession(UnitProfession.Warrior); // 将符合条件的单位设为战士
						}
					}
				}
			}
			return true; // 效果成功执行
		}

	//士兵 Warrior
		public static bool SoldierEffect(BaseSimObject pTarget, WorldTile pTile)
		{// 士兵 Soldier 就職
			if (pTarget != null && pTarget.a != null)
			{
				Actor actor = pTarget.a;
				if (actor.isKing() || actor.getProfession() == UnitProfession.Leader) // WorldBox 內建方法，判斷是否為國王
				{
					return false; // 單位已是國王，此效果不再需要將其轉職
				}
				// 确保目标对象拥有 Warrior 职业
				if (!actor.isProfession(UnitProfession.Warrior))
				{
					actor.setProfession(UnitProfession.Warrior);
				}
				// 設定角色的 AI 工作為 "attacker"
				var pAI = (AiSystemActor)Reflection.GetField(typeof(Actor), actor, "ai");
				if (pAI != null) // 確保 AI 系統存在
				{
					pAI.setJob("attacker");
				}
			}
			return true;
		}
			#endregion
			#region 戰技
		public static bool Skill0001_Effect(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 堅守 (被攻擊時檢查自身 cdt_atk 或 cdt_debuff01 並施加給自身)
			if (pSelf == null || pSelf.a == null) // 更簡潔的 null 檢查
			{
				return false; // 自身 Actor 無效
			}

			Actor selfActor = pSelf.a;
			
			string selfAtkStatus = "cdt_atk02";
			float selfAtkDuration = 30f;
			string selfDebuffStatus = "cdt_debuff01";
			string selfAddStatus = "stabilize";
			float selfAddDuration = 3f;

			// 檢查自身是否沒有 cdt_atk02 且沒有 cdt_debuff01 狀態
			if (!selfActor.hasStatus(selfAtkStatus) && !selfActor.hasStatus(selfDebuffStatus))
			{
				// 如果沒有冷卻狀態，則執行以下操作：

				// 1. 給予自身 cdt_atk02 狀態 (冷卻)
				selfActor.addStatusEffect(selfAtkStatus, selfAtkDuration);
				//Debug.Log($"{selfActor.name} 獲得了 {selfAtkStatus}，持續 {selfAtkDuration} 秒");

				// 2. 為自身施加 stabilize 狀態效果
				selfActor.addStatusEffect(selfAddStatus, selfAddDuration);
				//Debug.Log($"{selfActor.name} 獲得了 {selfAddStatus}，持續 {selfAddDuration} 秒");
				
				return true; // 效果成功執行
			}
			else
			{
				// 如果已經有冷卻狀態，則不執行任何效果，直接返回 false 或 true (取決於您希望的效果是否算"成功")
				// 如果您希望它在有冷卻時就"跳過"整個效果，則返回 false 即可
				// Debug.Log($"{selfActor.name} 處於冷卻狀態，未施加 {selfAtkStatus} 和 {selfAddStatus}。");
				return false; 
			}
		}
		private static readonly HashSet<string> rangedWeaponIDs = new HashSet<string>
		{// 遠程武器清單
			"white_staff",		//1>9
			"druid_staff",		// >9
			"necromancer_staff",// >6
			"evil_staff",		// >5
			"bow_wood",			//5>22
			"bow_copper",		//6
			"bow_bronze",		//7
			"bow_silver",		//8
			"bow_iron",			//9
			"bow_steel",		//8
			"bow_mythril",		//11
			"bow_adamantine",	//12
			"alien_blaster",	//13
			"shotgun"			//14
		};
		public static bool Skill0002_Effect(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 猛擊 (攻擊時檢查自身 cdt_atk 或 cdt_debuff01 並施加給自身，排除遠程武器)
			if (pSelf == null || pSelf.a == null) 
			{
				return false; // 自身 Actor 無效
			}
			Actor selfActor = pSelf.a;
			// === 新增：檢查單位是否持有遠程武器 ===
			var weaponSlot = selfActor.equipment.getSlot(EquipmentType.Weapon);
			if (weaponSlot != null && weaponSlot.getItem() != null)
			{
				Item equippedWeapon = weaponSlot.getItem();
				// === 修正: 新增對 equippedWeapon.asset 的空值檢查 ===
				if (equippedWeapon.asset != null && rangedWeaponIDs.Contains(equippedWeapon.asset.id))
				{
					return false;
				}
			}
			// === 遠程武器檢查結束 ===
			string selfAtkStatus = "cdt_atk01";
			float selfAtkDuration = 30f;
			string selfDebuffStatus = "cdt_debuff01"; 
			string selfAddStatus = "fullpower";
			float selfAddDuration = 2f;
			// 檢查自身是否沒有 cdt_atk01 且沒有 cdt_debuff01 狀態
			if (!selfActor.hasStatus(selfAtkStatus) && !selfActor.hasStatus(selfDebuffStatus))
			{
				// 如果沒有冷卻狀態，則執行以下操作：
				selfActor.addStatusEffect(selfAtkStatus, selfAtkDuration);
				selfActor.addStatusEffect(selfAddStatus, selfAddDuration);
				return true; // 效果成功執行
			}
			else
			{
				return false; 
			}
		}
		public static bool Skill0003_Effect(BaseSimObject pSelf, WorldTile pTile) 
		{// 彈雨 常態添加
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) // 添加 !pSelf.a.isAlive() 確保存活
			{
				return false; // 自身 Actor 無效或已死亡
			}

			Actor selfActor = pSelf.a;

			string selfAtkStatus = "cdt_atk03";	 // 攻擊冷卻狀態
			float selfAtkDuration = 60f;			// 攻擊冷卻持續時間
			string selfDebuffStatus = "cdt_debuff01"; // 另一個可能觸發冷卻的 Debuff 狀態
			string selfAddStatus = "bulletrain";	// 彈雨狀態
			float selfAddDuration = 50f;			// 彈雨狀態持續時間

			// 檢查自身是否沒有 cdt_atk03 且沒有 cdt_debuff01 狀態
			if (!selfActor.hasStatus(selfAtkStatus) && !selfActor.hasStatus(selfDebuffStatus))
			{
				// 如果沒有冷卻狀態，則執行以下操作：

				// 1. 給予自身 cdt_atk03 狀態 (冷卻)
				selfActor.addStatusEffect(selfAtkStatus, selfAtkDuration);
				// Debug.Log($"{selfActor.name} 獲得了 {selfAtkStatus}，持續 {selfAtkDuration} 秒");

				// 2. 為自身施加 bulletrain 狀態效果
				selfActor.addStatusEffect(selfAddStatus, selfAddDuration);
				// Debug.Log($"{selfActor.name} 獲得了 {selfAddStatus}，持續 {selfAddDuration} 秒");
				
				return true; // 效果成功執行
			}
			else
			{
				// 如果已經有冷卻狀態，則不執行任何效果
				// Debug.Log($"{selfActor.name} 處於冷卻狀態 (有 {selfAtkStatus} 或 {selfDebuffStatus})，未施加 {selfAddStatus}。");
				return false; // 效果未成功觸發
			}
		}
		public static bool Experience(BaseSimObject pTarget, WorldTile pTile = null)
		{// 經驗值增加
			if (pTarget is Actor actor) 
			{
				actor.addExperience(25); 
				actor.data.experience += (int)25f;
				//Debug.Log($"單位 {actor.name} 獲得了大量經驗值，準備升級！");
				return true; 
			}
			return false; 
		}
			#endregion
			#region 子彈
		public static bool Projectile_01(BaseSimObject pSelf, WorldTile pTile = null)
		{// 子彈攻擊 投石 常態被動
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a; // 攻擊發動者
			string attackCooldownStatus = "cdt_atk05";
			float attackCooldownDuration = 1.5f; // 冷卻時間調整為 1.5 秒 (您可以根據需要更改)
			string debuffStatus = "cdt_debuff01";
			if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus(debuffStatus))
			{
				return false;
			}
			float maxRange = 15f; // 索敵範圍
			Actor target = null;
			float closestDist = float.MaxValue;
			foreach (var other in World.world.units) // 遍歷所有單位
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist < maxRange && dist < closestDist)
				{
					closestDist = dist;
					target = other;
				}
			}
			if (target == null)
			{
				return false;
			}
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);

			UnityEngine.Vector3 selfPosition = selfActor.current_position;
			UnityEngine.Vector3 targetPosition = target.current_position;
			float distToTarget = Vector3.Distance(selfPosition, targetPosition);
			float targetSize = 1f; // 默認目標大小
			if (target != null && target.stats != null)
			{
				targetSize = target.stats["size"]; // 獲取目標的實際大小
			}
			UnityEngine.Vector3 basePoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, targetPosition.x, targetPosition.y, selfActor.stats["size"]);
			basePoint.y += 0.5f; // 發射點高度
			UnityEngine.Vector3 spreadCenterTarget = Toolbox.getNewPoint(targetPosition.x, targetPosition.y, selfPosition.x, selfPosition.y, targetSize, true);
			//子彈相關設定 發射多類型子彈由此開始複製
			int numberOfProjectiles_00 = 5; // 投射物數量
			float spreadAngle_00 = 2.5f; // 投射物散布角度
			for (int i = 0; i < numberOfProjectiles_00; i++)
			{	// 計算每個投射物的旋轉角度，以實現扇形散布
				UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, spreadAngle_00 * (i - (numberOfProjectiles_00 - 1f) / 2f));
				// 計算每個投射物的最終目標點
				UnityEngine.Vector3 spreadTarget = rotation * (targetPosition - basePoint) + basePoint; // 以 basePoint 為旋轉中心
				World.world.projectiles.spawn(
					pInitiator: selfActor,		 	// 發射者
					pTargetObject: target,		 	// 目標對象
					pAssetID: "rock",			  	// 投射物資產 ID
					pLaunchPosition: basePoint,		// 發射位置
					pTargetPosition: spreadTarget, 	// 目標位置
					pTargetZ: 0.0f				 	// 目標 Z 軸（通常為 0）
				);
			}
			//發射多類型子彈複製段落到此
			return true; // 表示能力成功觸發並執行了動作
		}
		public static bool Projectile_02(BaseSimObject pSelf, WorldTile pTile = null)
		{// 子彈攻擊 雪球 常態被動
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a; // 攻擊發動者
			string attackCooldownStatus = "cdt_atk05";
			float attackCooldownDuration = 1.5f; // 冷卻時間調整為 1.5 秒 (您可以根據需要更改)
			string debuffStatus = "cdt_debuff01";
			if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus(debuffStatus))
			{
				return false;
			}
			float maxRange = 15f; // 索敵範圍
			Actor target = null;
			float closestDist = float.MaxValue;
			foreach (var other in World.world.units) // 遍歷所有單位
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist < maxRange && dist < closestDist)
				{
					closestDist = dist;
					target = other;
				}
			}
			if (target == null)
			{
				return false;
			}
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);

			UnityEngine.Vector3 selfPosition = selfActor.current_position;
			UnityEngine.Vector3 targetPosition = target.current_position;
			float distToTarget = Vector3.Distance(selfPosition, targetPosition);
			float targetSize = 1f; // 默認目標大小
			if (target != null && target.stats != null)
			{
				targetSize = target.stats["size"]; // 獲取目標的實際大小
			}
			UnityEngine.Vector3 basePoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, targetPosition.x, targetPosition.y, selfActor.stats["size"]);
			basePoint.y += 0.5f; // 發射點高度
			UnityEngine.Vector3 spreadCenterTarget = Toolbox.getNewPoint(targetPosition.x, targetPosition.y, selfPosition.x, selfPosition.y, targetSize, true);
			//子彈相關設定 發射多類型子彈由此開始複製
			int numberOfProjectiles_00 = 5; // 投射物數量
			float spreadAngle_00 = 4.5f; // 投射物散布角度
			for (int i = 0; i < numberOfProjectiles_00; i++)
			{	// 計算每個投射物的旋轉角度，以實現扇形散布
				UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, spreadAngle_00 * (i - (numberOfProjectiles_00 - 1f) / 2f));
				// 計算每個投射物的最終目標點
				UnityEngine.Vector3 spreadTarget = rotation * (targetPosition - basePoint) + basePoint; // 以 basePoint 為旋轉中心
				World.world.projectiles.spawn(
					pInitiator: selfActor,		 	// 發射者
					pTargetObject: target,		 	// 目標對象
					pAssetID: "snowball",			  	// 投射物資產 ID
					pLaunchPosition: basePoint,		// 發射位置
					pTargetPosition: spreadTarget, 	// 目標位置
					pTargetZ: 0.0f				 	// 目標 Z 軸（通常為 0）
				);
			}
			//發射多類型子彈複製段落到此
			return true; // 表示能力成功觸發並執行了動作
		}
		public static bool Projectile_03(BaseSimObject pSelf, WorldTile pTile = null)
		{// 子彈攻擊 箭矢 常態被動
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a; // 攻擊發動者
			string attackCooldownStatus = "cdt_atk05";
			float attackCooldownDuration = 1.5f; // 冷卻時間調整為 1.5 秒 (您可以根據需要更改)
			string debuffStatus = "cdt_debuff01";
			if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus(debuffStatus))
			{
				return false;
			}
			float maxRange = 15f; // 索敵範圍
			Actor target = null;
			float closestDist = float.MaxValue;
			foreach (var other in World.world.units) // 遍歷所有單位
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist < maxRange && dist < closestDist)
				{
					closestDist = dist;
					target = other;
				}
			}
			if (target == null)
			{
				return false;
			}
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);

			UnityEngine.Vector3 selfPosition = selfActor.current_position;
			UnityEngine.Vector3 targetPosition = target.current_position;
			float distToTarget = Vector3.Distance(selfPosition, targetPosition);
			float targetSize = 1f; // 默認目標大小
			if (target != null && target.stats != null)
			{
				targetSize = target.stats["size"]; // 獲取目標的實際大小
			}
			UnityEngine.Vector3 basePoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, targetPosition.x, targetPosition.y, selfActor.stats["size"]);
			basePoint.y += 0.5f; // 發射點高度
			UnityEngine.Vector3 spreadCenterTarget = Toolbox.getNewPoint(targetPosition.x, targetPosition.y, selfPosition.x, selfPosition.y, targetSize, true);
			//子彈相關設定 發射多類型子彈由此開始複製
			int numberOfProjectiles_00 = 5; // 投射物數量
			float spreadAngle_00 = 5.0f; // 投射物散布角度
			for (int i = 0; i < numberOfProjectiles_00; i++)
			{	// 計算每個投射物的旋轉角度，以實現扇形散布
				UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, spreadAngle_00 * (i - (numberOfProjectiles_00 - 1f) / 2f));
				// 計算每個投射物的最終目標點
				UnityEngine.Vector3 spreadTarget = rotation * (targetPosition - basePoint) + basePoint; // 以 basePoint 為旋轉中心
				World.world.projectiles.spawn(
					pInitiator: selfActor,		 	// 發射者
					pTargetObject: target,		 	// 目標對象
					pAssetID: "arrow",			  	// 投射物資產 ID
					pLaunchPosition: basePoint,		// 發射位置
					pTargetPosition: spreadTarget, 	// 目標位置
					pTargetZ: 0.0f				 	// 目標 Z 軸（通常為 0）
				);
			}
			//發射多類型子彈複製段落到此
			return true; // 表示能力成功觸發並執行了動作
		}
		public static bool Projectile_04(BaseSimObject pSelf, WorldTile pTile = null)
		{// 子彈攻擊 火把 常態被動
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a; // 攻擊發動者
			string attackCooldownStatus = "cdt_atk05";
			float attackCooldownDuration = 1.5f; // 冷卻時間調整為 1.5 秒 (您可以根據需要更改)
			string debuffStatus = "cdt_debuff01";
			if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus(debuffStatus))
			{
				return false;
			}
			float maxRange = 15f; // 索敵範圍
			Actor target = null;
			float closestDist = float.MaxValue;
			foreach (var other in World.world.units) // 遍歷所有單位
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist < maxRange && dist < closestDist)
				{
					closestDist = dist;
					target = other;
				}
			}
			if (target == null)
			{
				return false;
			}
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);


			UnityEngine.Vector3 selfPosition = selfActor.current_position;
			UnityEngine.Vector3 targetPosition = target.current_position;
			float distToTarget = Vector3.Distance(selfPosition, targetPosition);
			float targetSize = 1f; // 默認目標大小
			if (target != null && target.stats != null)
			{
				targetSize = target.stats["size"]; // 獲取目標的實際大小
			}
			UnityEngine.Vector3 basePoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, targetPosition.x, targetPosition.y, selfActor.stats["size"]);
			basePoint.y += 0.5f; // 發射點高度
			UnityEngine.Vector3 spreadCenterTarget = Toolbox.getNewPoint(targetPosition.x, targetPosition.y, selfPosition.x, selfPosition.y, targetSize, true);
			//子彈相關設定 發射多類型子彈由此開始複製
			int numberOfProjectiles_00 = 3; // 投射物數量
			float spreadAngle_00 = 3.0f; // 投射物散布角度
			for (int i = 0; i < numberOfProjectiles_00; i++)
			{	// 計算每個投射物的旋轉角度，以實現扇形散布
				UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, spreadAngle_00 * (i - (numberOfProjectiles_00 - 1f) / 2f));
				// 計算每個投射物的最終目標點
				UnityEngine.Vector3 spreadTarget = rotation * (targetPosition - basePoint) + basePoint; // 以 basePoint 為旋轉中心
				World.world.projectiles.spawn(
					pInitiator: selfActor,		 	// 發射者
					pTargetObject: target,		 	// 目標對象
					pAssetID: "torch",			  	// 投射物資產 ID
					pLaunchPosition: basePoint,		// 發射位置
					pTargetPosition: spreadTarget, 	// 目標位置
					pTargetZ: 0.0f				 	// 目標 Z 軸（通常為 0）
				);
			}
			//發射多類型子彈複製段落到此
			return true; // 表示能力成功觸發並執行了動作
		}
		public static bool Projectile_05(BaseSimObject pSelf, WorldTile pTile = null)
		{// 子彈攻擊 火彈 常態被動
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a; // 攻擊發動者
			string attackCooldownStatus = "cdt_atk05";
			float attackCooldownDuration = 1.5f; // 冷卻時間調整為 1.5 秒 (您可以根據需要更改)
			string debuffStatus = "cdt_debuff01";
			if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus(debuffStatus))
			{
				return false;
			}
			float maxRange = 15f; // 索敵範圍
			Actor target = null;
			float closestDist = float.MaxValue;
			foreach (var other in World.world.units) // 遍歷所有單位
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist < maxRange && dist < closestDist)
				{
					closestDist = dist;
					target = other;
				}
			}
			if (target == null)
			{
				return false;
			}
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);


			UnityEngine.Vector3 selfPosition = selfActor.current_position;
			UnityEngine.Vector3 targetPosition = target.current_position;
			float distToTarget = Vector3.Distance(selfPosition, targetPosition);
			float targetSize = 1f; // 默認目標大小
			if (target != null && target.stats != null)
			{
				targetSize = target.stats["size"]; // 獲取目標的實際大小
			}
			UnityEngine.Vector3 basePoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, targetPosition.x, targetPosition.y, selfActor.stats["size"]);
			basePoint.y += 0.5f; // 發射點高度
			UnityEngine.Vector3 spreadCenterTarget = Toolbox.getNewPoint(targetPosition.x, targetPosition.y, selfPosition.x, selfPosition.y, targetSize, true);
			//子彈相關設定 發射多類型子彈由此開始複製
			int numberOfProjectiles_00 = 2; // 投射物數量
			float spreadAngle_00 = 5.0f; // 投射物散布角度
			for (int i = 0; i < numberOfProjectiles_00; i++)
			{	// 計算每個投射物的旋轉角度，以實現扇形散布
				UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, spreadAngle_00 * (i - (numberOfProjectiles_00 - 1f) / 2f));
				// 計算每個投射物的最終目標點
				UnityEngine.Vector3 spreadTarget = rotation * (targetPosition - basePoint) + basePoint; // 以 basePoint 為旋轉中心
				World.world.projectiles.spawn(
					pInitiator: selfActor,		 	// 發射者
					pTargetObject: target,		 	// 目標對象
					pAssetID: "firebomb",			  	// 投射物資產 ID
					pLaunchPosition: basePoint,		// 發射位置
					pTargetPosition: spreadTarget, 	// 目標位置
					pTargetZ: 0.0f				 	// 目標 Z 軸（通常為 0）
				);
			}
			//發射多類型子彈複製段落到此
			return true; // 表示能力成功觸發並執行了動作
		}
		public static bool Projectile_06(BaseSimObject pSelf, WorldTile pTile = null)
		{// 子彈攻擊 顱骨 常態被動
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a; // 攻擊發動者
			string attackCooldownStatus = "cdt_atk05";
			float attackCooldownDuration = 1.5f; // 冷卻時間調整為 1.5 秒 (您可以根據需要更改)
			string debuffStatus = "cdt_debuff01";
			if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus(debuffStatus))
			{
				return false;
			}
			float maxRange = 15f; // 索敵範圍
			Actor target = null;
			float closestDist = float.MaxValue;
			foreach (var other in World.world.units) // 遍歷所有單位
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist < maxRange && dist < closestDist)
				{
					closestDist = dist;
					target = other;
				}
			}
			if (target == null)
			{
				return false;
			}
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);

			UnityEngine.Vector3 selfPosition = selfActor.current_position;
			UnityEngine.Vector3 targetPosition = target.current_position;
			float distToTarget = Vector3.Distance(selfPosition, targetPosition);
			float targetSize = 1f; // 默認目標大小
			if (target != null && target.stats != null)
			{
				targetSize = target.stats["size"]; // 獲取目標的實際大小
			}
			UnityEngine.Vector3 basePoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, targetPosition.x, targetPosition.y, selfActor.stats["size"]);
			basePoint.y += 0.5f; // 發射點高度
			UnityEngine.Vector3 spreadCenterTarget = Toolbox.getNewPoint(targetPosition.x, targetPosition.y, selfPosition.x, selfPosition.y, targetSize, true);
			//子彈相關設定 發射多類型子彈由此開始複製
			int numberOfProjectiles_00 = 3; // 投射物數量
			float spreadAngle_00 = 3.0f; // 投射物散布角度
			for (int i = 0; i < numberOfProjectiles_00; i++)
			{	// 計算每個投射物的旋轉角度，以實現扇形散布
				UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, spreadAngle_00 * (i - (numberOfProjectiles_00 - 1f) / 2f));
				// 計算每個投射物的最終目標點
				UnityEngine.Vector3 spreadTarget = rotation * (targetPosition - basePoint) + basePoint; // 以 basePoint 為旋轉中心
				World.world.projectiles.spawn(
					pInitiator: selfActor,		 	// 發射者
					pTargetObject: target,		 	// 目標對象
					pAssetID: "skull",			  	// 投射物資產 ID
					pLaunchPosition: basePoint,		// 發射位置
					pTargetPosition: spreadTarget, 	// 目標位置
					pTargetZ: 0.0f				 	// 目標 Z 軸（通常為 0）
				);
			}
			int numberOfProjectiles_01 = 2; // 投射物數量
			float spreadAngle_01 = 3.0f; // 投射物散布角度
			for (int i = 0; i < numberOfProjectiles_01; i++)
			{	// 計算每個投射物的旋轉角度，以實現扇形散布
				UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, spreadAngle_01 * (i - (numberOfProjectiles_01 - 1f) / 2f));
				// 計算每個投射物的最終目標點
				UnityEngine.Vector3 spreadTarget = rotation * (targetPosition - basePoint) + basePoint; // 以 basePoint 為旋轉中心
				World.world.projectiles.spawn(
					pInitiator: selfActor,		 	// 發射者
					pTargetObject: target,		 	// 目標對象
					pAssetID: "bone",			  	// 投射物資產 ID
					pLaunchPosition: basePoint,		// 發射位置
					pTargetPosition: spreadTarget, 	// 目標位置
					pTargetZ: 0.0f				 	// 目標 Z 軸（通常為 0）
				);
			}
			//發射多類型子彈複製段落到此
			return true; // 表示能力成功觸發並執行了動作
		}
		public static bool Projectile_07(BaseSimObject pSelf, WorldTile pTile = null)
		{// 子彈攻擊 紅球 常態被動
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a; // 攻擊發動者
			string attackCooldownStatus = "cdt_atk05";
			float attackCooldownDuration = 1.5f; // 冷卻時間調整為 1.5 秒 (您可以根據需要更改)
			string debuffStatus = "cdt_debuff01";
			if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus(debuffStatus))
			{
				return false;
			}
			float maxRange = 15f; // 索敵範圍
			Actor target = null;
			float closestDist = float.MaxValue;
			foreach (var other in World.world.units) // 遍歷所有單位
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist < maxRange && dist < closestDist)
				{
					closestDist = dist;
					target = other;
				}
			}
			if (target == null)
			{
				return false;
			}
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);


			UnityEngine.Vector3 selfPosition = selfActor.current_position;
			UnityEngine.Vector3 targetPosition = target.current_position;
			float distToTarget = Vector3.Distance(selfPosition, targetPosition);
			float targetSize = 1f; // 默認目標大小
			if (target != null && target.stats != null)
			{
				targetSize = target.stats["size"]; // 獲取目標的實際大小
			}
			UnityEngine.Vector3 basePoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, targetPosition.x, targetPosition.y, selfActor.stats["size"]);
			basePoint.y += 0.5f; // 發射點高度
			UnityEngine.Vector3 spreadCenterTarget = Toolbox.getNewPoint(targetPosition.x, targetPosition.y, selfPosition.x, selfPosition.y, targetSize, true);
			//子彈相關設定 發射多類型子彈由此開始複製
			int numberOfProjectiles_00 = 3; // 投射物數量
			float spreadAngle_00 = 2.0f; // 投射物散布角度
			for (int i = 0; i < numberOfProjectiles_00; i++)
			{	// 計算每個投射物的旋轉角度，以實現扇形散布
				UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, spreadAngle_00 * (i - (numberOfProjectiles_00 - 1f) / 2f));
				// 計算每個投射物的最終目標點
				UnityEngine.Vector3 spreadTarget = rotation * (targetPosition - basePoint) + basePoint; // 以 basePoint 為旋轉中心
				World.world.projectiles.spawn(
					pInitiator: selfActor,		 	// 發射者
					pTargetObject: target,		 	// 目標對象
					pAssetID: "red_orb",			  	// 投射物資產 ID
					pLaunchPosition: basePoint,		// 發射位置
					pTargetPosition: spreadTarget, 	// 目標位置
					pTargetZ: 0.0f				 	// 目標 Z 軸（通常為 0）
				);
			}
			//發射多類型子彈複製段落到此
			return true; // 表示能力成功觸發並執行了動作
		}
		public static bool Projectile_08(BaseSimObject pSelf, WorldTile pTile = null)
		{// 子彈攻擊 冰球 常態被動
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a; // 攻擊發動者
			string attackCooldownStatus = "cdt_atk05";
			float attackCooldownDuration = 1.5f; // 冷卻時間調整為 1.5 秒 (您可以根據需要更改)
			string debuffStatus = "cdt_debuff01";
			if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus(debuffStatus))
			{
				return false;
			}
			float maxRange = 15f; // 索敵範圍
			Actor target = null;
			float closestDist = float.MaxValue;
			foreach (var other in World.world.units) // 遍歷所有單位
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist < maxRange && dist < closestDist)
				{
					closestDist = dist;
					target = other;
				}
			}
			if (target == null)
			{
				return false;
			}
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);

			UnityEngine.Vector3 selfPosition = selfActor.current_position;
			UnityEngine.Vector3 targetPosition = target.current_position;
			float distToTarget = Vector3.Distance(selfPosition, targetPosition);
			float targetSize = 1f; // 默認目標大小
			if (target != null && target.stats != null)
			{
				targetSize = target.stats["size"]; // 獲取目標的實際大小
			}
			UnityEngine.Vector3 basePoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, targetPosition.x, targetPosition.y, selfActor.stats["size"]);
			basePoint.y += 0.5f; // 發射點高度
			UnityEngine.Vector3 spreadCenterTarget = Toolbox.getNewPoint(targetPosition.x, targetPosition.y, selfPosition.x, selfPosition.y, targetSize, true);
			//子彈相關設定 發射多類型子彈由此開始複製
			int numberOfProjectiles_00 = 3; // 投射物數量
			float spreadAngle_00 = 2.0f; // 投射物散布角度
			for (int i = 0; i < numberOfProjectiles_00; i++)
			{	// 計算每個投射物的旋轉角度，以實現扇形散布
				UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, spreadAngle_00 * (i - (numberOfProjectiles_00 - 1f) / 2f));
				// 計算每個投射物的最終目標點
				UnityEngine.Vector3 spreadTarget = rotation * (targetPosition - basePoint) + basePoint; // 以 basePoint 為旋轉中心
				World.world.projectiles.spawn(
					pInitiator: selfActor,		 	// 發射者
					pTargetObject: target,		 	// 目標對象
					pAssetID: "freeze_orb",			  	// 投射物資產 ID
					pLaunchPosition: basePoint,		// 發射位置
					pTargetPosition: spreadTarget, 	// 目標位置
					pTargetZ: 0.0f				 	// 目標 Z 軸（通常為 0）
				);
			}
			//發射多類型子彈複製段落到此
			return true; // 表示能力成功觸發並執行了動作
		}
		public static bool Projectile_09(BaseSimObject pSelf, WorldTile pTile = null)
		{// 子彈攻擊 綠球 常態被動
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a; // 攻擊發動者
			string attackCooldownStatus = "cdt_atk05";
			float attackCooldownDuration = 1.5f; // 冷卻時間調整為 1.5 秒 (您可以根據需要更改)
			string debuffStatus = "cdt_debuff01";
			if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus(debuffStatus))
			{
				return false;
			}
			float maxRange = 15f; // 索敵範圍
			Actor target = null;
			float closestDist = float.MaxValue;
			foreach (var other in World.world.units) // 遍歷所有單位
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist < maxRange && dist < closestDist)
				{
					closestDist = dist;
					target = other;
				}
			}
			if (target == null)
			{
				return false;
			}
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);


			UnityEngine.Vector3 selfPosition = selfActor.current_position;
			UnityEngine.Vector3 targetPosition = target.current_position;
			float distToTarget = Vector3.Distance(selfPosition, targetPosition);
			float targetSize = 1f; // 默認目標大小
			if (target != null && target.stats != null)
			{
				targetSize = target.stats["size"]; // 獲取目標的實際大小
			}
			UnityEngine.Vector3 basePoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, targetPosition.x, targetPosition.y, selfActor.stats["size"]);
			basePoint.y += 0.5f; // 發射點高度
			UnityEngine.Vector3 spreadCenterTarget = Toolbox.getNewPoint(targetPosition.x, targetPosition.y, selfPosition.x, selfPosition.y, targetSize, true);
			//子彈相關設定 發射多類型子彈由此開始複製
			int numberOfProjectiles_00 = 3; // 投射物數量
			float spreadAngle_00 = 3.0f; // 投射物散布角度
			for (int i = 0; i < numberOfProjectiles_00; i++)
			{	// 計算每個投射物的旋轉角度，以實現扇形散布
				UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, spreadAngle_00 * (i - (numberOfProjectiles_00 - 1f) / 2f));
				// 計算每個投射物的最終目標點
				UnityEngine.Vector3 spreadTarget = rotation * (targetPosition - basePoint) + basePoint; // 以 basePoint 為旋轉中心
				World.world.projectiles.spawn(
					pInitiator: selfActor,		 	// 發射者
					pTargetObject: target,		 	// 目標對象
					pAssetID: "green_orb",			  	// 投射物資產 ID
					pLaunchPosition: basePoint,		// 發射位置
					pTargetPosition: spreadTarget, 	// 目標位置
					pTargetZ: 0.0f				 	// 目標 Z 軸（通常為 0）
				);
			}
			//發射多類型子彈複製段落到此
			return true; // 表示能力成功觸發並執行了動作
		}
		public static bool Projectile_10(BaseSimObject pSelf, WorldTile pTile = null)
		{// 子彈攻擊 酸炮 常態被動
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a; // 攻擊發動者

			// === 根據發動者類型修改參數 ===
			float maxRange = 15f; // 默認索敵範圍
			int numberOfProjectiles_00 = 1; // 默認投射物數量
			
			// 檢查發動者是否為龍
			if (selfActor.asset.id == "zombie_dragon")
			{
				maxRange = 25f; // 龍的索敵範圍增加
				numberOfProjectiles_00 = 3; // 龍的投射物數量增加
			}
			// =============================
			
			string attackCooldownStatus = "cdt_atk05";
			float attackCooldownDuration = 1.5f; // 冷卻時間調整為 1.5 秒 (您可以根據需要更改)
			string debuffStatus = "cdt_debuff01";
			if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus(debuffStatus))
			{
				return false;
			}
			float closestDist = float.MaxValue;
			Actor target = null;
			foreach (var other in World.world.units) // 遍歷所有單位
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist < maxRange && dist < closestDist)
				{
					closestDist = dist;
					target = other;
				}
			}
			if (target == null)
			{
				return false;
			}
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);


			UnityEngine.Vector3 selfPosition = selfActor.current_position;
			UnityEngine.Vector3 targetPosition = target.current_position;
			float distToTarget = Vector3.Distance(selfPosition, targetPosition);
			float targetSize = 1f; // 默認目標大小
			if (target != null && target.stats != null)
			{
				targetSize = target.stats["size"]; // 獲取目標的實際大小
			}
			UnityEngine.Vector3 basePoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, targetPosition.x, targetPosition.y, selfActor.stats["size"]);
			basePoint.y += 0.5f; // 發射點高度
			UnityEngine.Vector3 spreadCenterTarget = Toolbox.getNewPoint(targetPosition.x, targetPosition.y, selfPosition.x, selfPosition.y, targetSize, true);
			//子彈相關設定 發射多類型子彈由此開始複製
			float spreadAngle_00 = 1.0f; // 投射物散布角度
			for (int i = 0; i < numberOfProjectiles_00; i++)
			{	// 計算每個投射物的旋轉角度，以實現扇形散布
				UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, spreadAngle_00 * (i - (numberOfProjectiles_00 - 1f) / 2f));
				// 計算每個投射物的最終目標點
				UnityEngine.Vector3 spreadTarget = rotation * (targetPosition - basePoint) + basePoint; // 以 basePoint 為旋轉中心
				World.world.projectiles.spawn(
					pInitiator: selfActor,		 	// 發射者
					pTargetObject: target,		 	// 目標對象
					pAssetID: "acid_ball",			  	// 投射物資產 ID
					pLaunchPosition: basePoint,		// 發射位置
					pTargetPosition: spreadTarget, 	// 目標位置
					pTargetZ: 0.0f				 	// 目標 Z 軸（通常為 0）
				);
			}
			//發射多類型子彈複製段落到此
			return true; // 表示能力成功觸發並執行了動作
		}
		public static bool Projectile_11(BaseSimObject pSelf, WorldTile pTile = null)
		{// 子彈攻擊 炮彈 常態被動
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a; // 攻擊發動者
			string attackCooldownStatus = "cdt_atk05";
			float attackCooldownDuration = 1.5f; // 冷卻時間調整為 1.5 秒 (您可以根據需要更改)
			string debuffStatus = "cdt_debuff01";
			if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus(debuffStatus))
			{
				return false;
			}
			float maxRange = 15f; // 索敵範圍
			Actor target = null;
			float closestDist = float.MaxValue;
			foreach (var other in World.world.units) // 遍歷所有單位
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist < maxRange && dist < closestDist)
				{
					closestDist = dist;
					target = other;
				}
			}
			if (target == null)
			{
				return false;
			}
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);


			UnityEngine.Vector3 selfPosition = selfActor.current_position;
			UnityEngine.Vector3 targetPosition = target.current_position;
			float distToTarget = Vector3.Distance(selfPosition, targetPosition);
			float targetSize = 1f; // 默認目標大小
			if (target != null && target.stats != null)
			{
				targetSize = target.stats["size"]; // 獲取目標的實際大小
			}
			UnityEngine.Vector3 basePoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, targetPosition.x, targetPosition.y, selfActor.stats["size"]);
			basePoint.y += 0.5f; // 發射點高度
			UnityEngine.Vector3 spreadCenterTarget = Toolbox.getNewPoint(targetPosition.x, targetPosition.y, selfPosition.x, selfPosition.y, targetSize, true);
			//子彈相關設定 發射多類型子彈由此開始複製
			int numberOfProjectiles_00 = 1; // 投射物數量
			float spreadAngle_00 = 5.0f; // 投射物散布角度
			for (int i = 0; i < numberOfProjectiles_00; i++)
			{	// 計算每個投射物的旋轉角度，以實現扇形散布
				UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, spreadAngle_00 * (i - (numberOfProjectiles_00 - 1f) / 2f));
				// 計算每個投射物的最終目標點
				UnityEngine.Vector3 spreadTarget = rotation * (targetPosition - basePoint) + basePoint; // 以 basePoint 為旋轉中心
				World.world.projectiles.spawn(
					pInitiator: selfActor,		 	// 發射者
					pTargetObject: target,		 	// 目標對象
					pAssetID: "cannonball",			  	// 投射物資產 ID
					pLaunchPosition: basePoint,		// 發射位置
					pTargetPosition: spreadTarget, 	// 目標位置
					pTargetZ: 0.0f				 	// 目標 Z 軸（通常為 0）
				);
			}
			//發射多類型子彈複製段落到此
			return true; // 表示能力成功觸發並執行了動作
		}
		public static bool Projectile_12(BaseSimObject pSelf, WorldTile pTile = null)
		{// 子彈攻擊 電漿 常態被動
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a; // 攻擊發動者
			string attackCooldownStatus = "cdt_atk05";
			float attackCooldownDuration = 1.5f; // 冷卻時間調整為 1.5 秒 (您可以根據需要更改)
			string debuffStatus = "cdt_debuff01";
			if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus(debuffStatus))
			{
				return false;
			}
			float maxRange = 15f; // 索敵範圍
			Actor target = null;
			float closestDist = float.MaxValue;
			foreach (var other in World.world.units) // 遍歷所有單位
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist < maxRange && dist < closestDist)
				{
					closestDist = dist;
					target = other;
				}
			}
			if (target == null)
			{
				return false;
			}
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);


			UnityEngine.Vector3 selfPosition = selfActor.current_position;
			UnityEngine.Vector3 targetPosition = target.current_position;
			float distToTarget = Vector3.Distance(selfPosition, targetPosition);
			float targetSize = 1f; // 默認目標大小
			if (target != null && target.stats != null)
			{
				targetSize = target.stats["size"]; // 獲取目標的實際大小
			}
			UnityEngine.Vector3 basePoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, targetPosition.x, targetPosition.y, selfActor.stats["size"]);
			basePoint.y += 0.5f; // 發射點高度
			UnityEngine.Vector3 spreadCenterTarget = Toolbox.getNewPoint(targetPosition.x, targetPosition.y, selfPosition.x, selfPosition.y, targetSize, true);
			//子彈相關設定 發射多類型子彈由此開始複製
			int numberOfProjectiles_00 = 1; // 投射物數量
			float spreadAngle_00 = 3.0f; // 投射物散布角度
			for (int i = 0; i < numberOfProjectiles_00; i++)
			{	// 計算每個投射物的旋轉角度，以實現扇形散布
				UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, spreadAngle_00 * (i - (numberOfProjectiles_00 - 1f) / 2f));
				// 計算每個投射物的最終目標點
				UnityEngine.Vector3 spreadTarget = rotation * (targetPosition - basePoint) + basePoint; // 以 basePoint 為旋轉中心
				World.world.projectiles.spawn(
					pInitiator: selfActor,		 	// 發射者
					pTargetObject: target,		 	// 目標對象
					pAssetID: "plasma_ball",			  	// 投射物資產 ID
					pLaunchPosition: basePoint,		// 發射位置
					pTargetPosition: spreadTarget, 	// 目標位置
					pTargetZ: 0.0f				 	// 目標 Z 軸（通常為 0）
				);
			}
			//發射多類型子彈複製段落到此
			return true; // 表示能力成功觸發並執行了動作
		}
		public static bool Projectile_13(BaseSimObject pSelf, WorldTile pTile = null)
		{// 子彈攻擊 槍彈 常態被動
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a; // 攻擊發動者
			string attackCooldownStatus = "cdt_atk05";
			float attackCooldownDuration = 1.5f; // 冷卻時間調整為 1.5 秒 (您可以根據需要更改)
			string debuffStatus = "cdt_debuff01";
			if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus(debuffStatus))
			{
				return false;
			}
			float maxRange = 15f; // 索敵範圍
			Actor target = null;
			float closestDist = float.MaxValue;
			foreach (var other in World.world.units) // 遍歷所有單位
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist < maxRange && dist < closestDist)
				{
					closestDist = dist;
					target = other;
				}
			}
			if (target == null)
			{
				return false;
			}
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);

			UnityEngine.Vector3 selfPosition = selfActor.current_position;
			UnityEngine.Vector3 targetPosition = target.current_position;
			float distToTarget = Vector3.Distance(selfPosition, targetPosition);
			float targetSize = 1f; // 默認目標大小
			if (target != null && target.stats != null)
			{
				targetSize = target.stats["size"]; // 獲取目標的實際大小
			}
			UnityEngine.Vector3 basePoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, targetPosition.x, targetPosition.y, selfActor.stats["size"]);
			basePoint.y += 0.5f; // 發射點高度
			UnityEngine.Vector3 spreadCenterTarget = Toolbox.getNewPoint(targetPosition.x, targetPosition.y, selfPosition.x, selfPosition.y, targetSize, true);
			//子彈相關設定 發射多類型子彈由此開始複製
			int numberOfProjectiles_00 = 5; // 投射物數量
			float spreadAngle_00 = 0.9f; // 投射物散布角度
			for (int i = 0; i < numberOfProjectiles_00; i++)
			{	// 計算每個投射物的旋轉角度，以實現扇形散布
				UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, spreadAngle_00 * (i - (numberOfProjectiles_00 - 1f) / 2f));
				// 計算每個投射物的最終目標點
				UnityEngine.Vector3 spreadTarget = rotation * (targetPosition - basePoint) + basePoint; // 以 basePoint 為旋轉中心
				World.world.projectiles.spawn(
					pInitiator: selfActor,		 	// 發射者
					pTargetObject: target,		 	// 目標對象
					pAssetID: "shotgun_bullet",			  	// 投射物資產 ID
					pLaunchPosition: basePoint,		// 發射位置
					pTargetPosition: spreadTarget, 	// 目標位置
					pTargetZ: 0.0f				 	// 目標 Z 軸（通常為 0）
				);
			}
			//發射多類型子彈複製段落到此
			return true; // 表示能力成功觸發並執行了動作
		}
		public static bool Projectile_14(BaseSimObject pSelf, WorldTile pTile = null)
		{// 子彈攻擊 狂球 常態被動
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a; // 攻擊發動者
			string attackCooldownStatus = "cdt_atk05";
			float attackCooldownDuration = 1.5f; // 冷卻時間調整為 1.5 秒 (您可以根據需要更改)
			string debuffStatus = "cdt_debuff01";
			if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus(debuffStatus))
			{
				return false;
			}
			float maxRange = 15f; // 索敵範圍
			Actor target = null;
			float closestDist = float.MaxValue;
			foreach (var other in World.world.units) // 遍歷所有單位
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist < maxRange && dist < closestDist)
				{
					closestDist = dist;
					target = other;
				}
			}
			if (target == null)
			{
				return false;
			}
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);

			UnityEngine.Vector3 selfPosition = selfActor.current_position;
			UnityEngine.Vector3 targetPosition = target.current_position;
			float distToTarget = Vector3.Distance(selfPosition, targetPosition);
			float targetSize = 1f; // 默認目標大小
			if (target != null && target.stats != null)
			{
				targetSize = target.stats["size"]; // 獲取目標的實際大小
			}
			UnityEngine.Vector3 basePoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, targetPosition.x, targetPosition.y, selfActor.stats["size"]);
			basePoint.y += 0.5f; // 發射點高度
			UnityEngine.Vector3 spreadCenterTarget = Toolbox.getNewPoint(targetPosition.x, targetPosition.y, selfPosition.x, selfPosition.y, targetSize, true);
			//子彈相關設定 發射多類型子彈由此開始複製
			int numberOfProjectiles_00 = 1; // 投射物數量
			float spreadAngle_00 = 5.0f; // 投射物散布角度
			for (int i = 0; i < numberOfProjectiles_00; i++)
			{	// 計算每個投射物的旋轉角度，以實現扇形散布
				UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, spreadAngle_00 * (i - (numberOfProjectiles_00 - 1f) / 2f));
				// 計算每個投射物的最終目標點
				UnityEngine.Vector3 spreadTarget = rotation * (targetPosition - basePoint) + basePoint; // 以 basePoint 為旋轉中心
				World.world.projectiles.spawn(
					pInitiator: selfActor,		 	// 發射者
					pTargetObject: target,		 	// 目標對象
					pAssetID: "madness_ball",			  	// 投射物資產 ID
					pLaunchPosition: basePoint,		// 發射位置
					pTargetPosition: spreadTarget, 	// 目標位置
					pTargetZ: 0.0f				 	// 目標 Z 軸（通常為 0）
				);
			}
			//發射多類型子彈複製段落到此
			return true; // 表示能力成功觸發並執行了動作
		}
		public static bool Projectile_15(BaseSimObject pSelf, WorldTile pTile = null)
		{// 子彈攻擊 火球 常態被動
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a; // 攻擊發動者

			// === 根據發動者類型修改參數 ===
			float maxRange = 15f; // 默認索敵範圍
			int numberOfProjectiles_00 = 1; // 默認投射物數量
			
			// 檢查發動者是否為龍
			if (selfActor.asset.id == "dragon")
			{
				maxRange = 25f; // 龍的索敵範圍增加
				numberOfProjectiles_00 = 3; // 龍的投射物數量增加
			}
			// =============================
			
			string attackCooldownStatus = "cdt_atk05";
			float attackCooldownDuration = 1.5f; // 冷卻時間調整為 1.5 秒 (您可以根據需要更改)
			string debuffStatus = "cdt_debuff01";
			if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus(debuffStatus))
			{
				return false;
			}
			float closestDist = float.MaxValue;
			Actor target = null;
			foreach (var other in World.world.units) // 遍歷所有單位
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist < maxRange && dist < closestDist)
				{
					closestDist = dist;
					target = other;
				}
			}
			if (target == null)
			{
				return false;
			}
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);

			UnityEngine.Vector3 selfPosition = selfActor.current_position;
			UnityEngine.Vector3 targetPosition = target.current_position;
			float distToTarget = Vector3.Distance(selfPosition, targetPosition);
			float targetSize = 1f; // 默認目標大小
			if (target != null && target.stats != null)
			{
				targetSize = target.stats["size"]; // 獲取目標的實際大小
			}
			UnityEngine.Vector3 basePoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, targetPosition.x, targetPosition.y, selfActor.stats["size"]);
			basePoint.y += 0.5f; // 發射點高度
			UnityEngine.Vector3 spreadCenterTarget = Toolbox.getNewPoint(targetPosition.x, targetPosition.y, selfPosition.x, selfPosition.y, targetSize, true);
			//子彈相關設定 發射多類型子彈由此開始複製
			float spreadAngle_00 = 1.0f; // 投射物散布角度
			for (int i = 0; i < numberOfProjectiles_00; i++)
			{	// 計算每個投射物的旋轉角度，以實現扇形散布
				UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, spreadAngle_00 * (i - (numberOfProjectiles_00 - 1f) / 2f));
				// 計算每個投射物的最終目標點
				UnityEngine.Vector3 spreadTarget = rotation * (targetPosition - basePoint) + basePoint; // 以 basePoint 為旋轉中心
				World.world.projectiles.spawn(
					pInitiator: selfActor,		 	// 發射者
					pTargetObject: target,		 	// 目標對象
					pAssetID: "fireball",			  	// 投射物資產 ID
					pLaunchPosition: basePoint,		// 發射位置
					pTargetPosition: spreadTarget, 	// 目標位置
					pTargetZ: 0.0f				 	// 目標 Z 軸（通常為 0）
				);
			}
			//發射多類型子彈複製段落到此
			return true; // 表示能力成功觸發並執行了動作
		}
	//子彈攻擊 攻擊發動
/*		public static bool Projectile_O01(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 子彈攻擊 石頭
			if (pTarget != null)
			{
				Actor selfActor = pSelf as Actor;
				Actor targetActor = pTarget as Actor;
				if (selfActor == null)
				{
					return false;
				}
				// === 新增：冷卻狀態檢查邏輯 ===
				string attackCooldownStatus = "cdt_atk05";
				float attackCooldownDuration = 0.1f; // 調整這個值來設定冷卻時間，例如 1.5 秒
				string debuffStatus = "cdt_debuff01";

				// 檢查單位是否持有任何冷卻或負面狀態，如果有的話就不發動
				// 這裡的邏輯是：只有在『沒有』cdt_atk05 和 『沒有』cdt_debuff01 的情況下才發動
				if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus(debuffStatus))
				{
					// Debug.Log($"[Projectile_01] {selfActor.name} 處於冷卻或負面狀態，無法發動投射物效果。");
					return false;
				}
				// === 原始的發動條件檢查 ===
				string requiredStatus = "crosshair";
				if (selfActor.hasStatus(requiredStatus))
				{
					if (targetActor == null)
					{
						//Debug.LogWarning("[Projectile_01] 目標 Actor 為 null，無法執行投射物邏輯。");
						return false;
					}
					// === 冷卻狀態施加 ===
					// 在發動後，立即為單位施加冷卻狀態
					selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
					// 施加 debuff 狀態（如果需要的話，但根據需求描述，這裡不需要，只檢查它是否存在）
					// selfActor.addStatusEffect(debuffStatus, debuffDuration); 
					// 保持原本的無敵狀態，但時長可以縮短，避免影響冷卻
		
					// === 投射物發射邏輯（保持不變） ===
					UnityEngine.Vector2Int pos = pTile.pos;
					UnityEngine.Vector3 selfPosition = selfActor.current_position;
					UnityEngine.Vector3 targetPosition = targetActor.current_position;
					float pDist = UnityEngine.Vector3.Distance(targetPosition, new UnityEngine.Vector3(pos.x, pos.y, 0f));
					float targetSize = 1f;
					if (targetActor != null && targetActor.stats != null)
					{
						targetSize = targetActor.stats["size"];
					}
					UnityEngine.Vector3 basePoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, (float)pos.x, (float)pos.y, pDist, true);
					UnityEngine.Vector3 targetPoint = Toolbox.getNewPoint(targetPosition.x, targetPosition.y, (float)pos.x, (float)pos.y, targetSize, true);
					int numberOfProjectiles = 20;
					float spreadAngle = 0.25f;
					for (int i = 0; i < numberOfProjectiles; i++)
					{
						UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, spreadAngle * (i - (numberOfProjectiles - 1f) / 2f));
						UnityEngine.Vector3 spreadTarget = rotation * (targetPoint - basePoint) + basePoint;
						World.world.projectiles.spawn(pInitiator: selfActor, pTargetObject: targetActor, pAssetID: "rock", pLaunchPosition: basePoint, pTargetPosition: spreadTarget, pTargetZ: 0.0f);
					}
					return true;
				}
				return false;
			}
			return false;
		}
		public static bool Projectile_O02(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 子彈攻擊 雪球
			if (pTarget != null)
			{
				Actor selfActor = pSelf as Actor;
				Actor targetActor = pTarget as Actor;
				string requiredStatus = "crosshair"; // 新增：需要檢查的狀態
				if (selfActor == null)
				{
					return false;
				}
				// === 新增：冷卻狀態檢查邏輯 ===
				string attackCooldownStatus = "cdt_atk05";
				float attackCooldownDuration = 0.1f; // 可以根據遊戲平衡需求調整
				string debuffStatus = "cdt_debuff01";
				// 檢查單位是否持有任何冷卻或負面狀態，如果有的話就不發動
				if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus(debuffStatus))
				{
					return false;
				}
				// === 修改這裡的條件判斷 ===
				// 現在只檢查 selfActor 是否具有 "crosshair" 狀態
				if (selfActor.hasStatus(requiredStatus))
				{
					if (targetActor == null)
					{
						//Debug.LogWarning("[Projectile_10] 目標 Actor 為 null，無法執行投射物邏輯。");
						return false;
					}
					// === 冷卻狀態施加 ===
					// 在發動後，立即為單位施加冷卻狀態
					selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
		
					UnityEngine.Vector2Int pos = pTile.pos;
					UnityEngine.Vector3 selfPosition = selfActor.current_position;
					UnityEngine.Vector3 targetPosition = targetActor.current_position;
					float pDist = UnityEngine.Vector3.Distance(targetPosition, new UnityEngine.Vector3(pos.x, pos.y, 0f));
					float targetSize = 1f;
					if (targetActor != null && targetActor.stats != null)
					{
						targetSize = targetActor.stats["size"];
					}
					UnityEngine.Vector3 basePoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, (float)pos.x, (float)pos.y, pDist, true);
					UnityEngine.Vector3 targetPoint = Toolbox.getNewPoint(targetPosition.x, targetPosition.y, (float)pos.x, (float)pos.y, targetSize, true);
					int numberOfProjectiles = 10;
					float spreadAngle = 4.5f;
					for (int i = 0; i < numberOfProjectiles; i++)
					{
						UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, spreadAngle * (i - (numberOfProjectiles - 1f) / 2f));
						UnityEngine.Vector3 spreadTarget = rotation * (targetPoint - basePoint) + basePoint;
						//子彈設置
						World.world.projectiles.spawn(pInitiator: selfActor, pTargetObject: targetActor, pAssetID: "snowball", pLaunchPosition: basePoint, pTargetPosition: spreadTarget, pTargetZ: 0.0f);
					}
					return true;
				}
				return false;
			}
			return false;
		}
		public static bool Projectile_O03(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 子彈攻擊 箭矢
			if (pTarget != null)
			{
				Actor selfActor = pSelf as Actor;
				Actor targetActor = pTarget as Actor;
				string requiredStatus = "crosshair"; // 新增：需要檢查的狀態
				if (selfActor == null)
				{
					return false;
				}
				// === 新增：冷卻狀態檢查邏輯 ===
				string attackCooldownStatus = "cdt_atk05";
				float attackCooldownDuration = 0.1f; // 可以根據遊戲平衡需求調整
				string debuffStatus = "cdt_debuff01";
				// 檢查單位是否持有任何冷卻或負面狀態，如果有的話就不發動
				if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus(debuffStatus))
				{
					return false;
				}
				// === 修改這裡的條件判斷 ===
				// 現在只檢查 selfActor 是否具有 "crosshair" 狀態
				if (selfActor.hasStatus(requiredStatus))
				{
					if (targetActor == null)
					{
						//Debug.LogWarning("[Projectile_10] 目標 Actor 為 null，無法執行投射物邏輯。");
						return false;
					}
					// === 冷卻狀態施加 ===
					// 在發動後，立即為單位施加冷卻狀態
					selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
		
					UnityEngine.Vector2Int pos = pTile.pos;
					UnityEngine.Vector3 selfPosition = selfActor.current_position;
					UnityEngine.Vector3 targetPosition = targetActor.current_position;
					float pDist = UnityEngine.Vector3.Distance(targetPosition, new UnityEngine.Vector3(pos.x, pos.y, 0f));
					float targetSize = 1f;
					if (targetActor != null && targetActor.stats != null)
					{
						targetSize = targetActor.stats["size"];
					}
					UnityEngine.Vector3 basePoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, (float)pos.x, (float)pos.y, pDist, true);
					UnityEngine.Vector3 targetPoint = Toolbox.getNewPoint(targetPosition.x, targetPosition.y, (float)pos.x, (float)pos.y, targetSize, true);
					int numberOfProjectiles = 10;
					float spreadAngle = 4.5f;
					for (int i = 0; i < numberOfProjectiles; i++)
					{
						UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, spreadAngle * (i - (numberOfProjectiles - 1f) / 2f));
						UnityEngine.Vector3 spreadTarget = rotation * (targetPoint - basePoint) + basePoint;
						//子彈設置
						World.world.projectiles.spawn(pInitiator: selfActor, pTargetObject: targetActor, pAssetID: "arrow", pLaunchPosition: basePoint, pTargetPosition: spreadTarget, pTargetZ: 0.0f);
					}
					return true;
				}
				return false;
			}
			return false;
		}
		public static bool Projectile_O04(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 子彈攻擊 火把
			if (pTarget != null)
			{
				Actor selfActor = pSelf as Actor;
				Actor targetActor = pTarget as Actor;
				string requiredStatus = "crosshair"; // 新增：需要檢查的狀態
				if (selfActor == null)
				{
					return false;
				}
				// === 新增：冷卻狀態檢查邏輯 ===
				string attackCooldownStatus = "cdt_atk05";
				float attackCooldownDuration = 0.1f; // 可以根據遊戲平衡需求調整
				string debuffStatus = "cdt_debuff01";
				// 檢查單位是否持有任何冷卻或負面狀態，如果有的話就不發動
				if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus(debuffStatus))
				{
					return false;
				}
				// === 修改這裡的條件判斷 ===
				// 現在只檢查 selfActor 是否具有 "crosshair" 狀態
				if (selfActor.hasStatus(requiredStatus))
				{
					if (targetActor == null)
					{
						//Debug.LogWarning("[Projectile_10] 目標 Actor 為 null，無法執行投射物邏輯。");
						return false;
					}
					// === 冷卻狀態施加 ===
					// 在發動後，立即為單位施加冷卻狀態
					selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
		 // 保持為施加動作
		 // 保持為施加動作
					UnityEngine.Vector2Int pos = pTile.pos;
					UnityEngine.Vector3 selfPosition = selfActor.current_position;
					UnityEngine.Vector3 targetPosition = targetActor.current_position;
					float pDist = UnityEngine.Vector3.Distance(targetPosition, new UnityEngine.Vector3(pos.x, pos.y, 0f));
					float targetSize = 1f;
					if (targetActor != null && targetActor.stats != null)
					{
						targetSize = targetActor.stats["size"];
					}
					UnityEngine.Vector3 basePoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, (float)pos.x, (float)pos.y, pDist, true);
					UnityEngine.Vector3 targetPoint = Toolbox.getNewPoint(targetPosition.x, targetPosition.y, (float)pos.x, (float)pos.y, targetSize, true);
					int numberOfProjectiles = 5;
					float spreadAngle = 3f;
					for (int i = 0; i < numberOfProjectiles; i++)
					{
						UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, spreadAngle * (i - (numberOfProjectiles - 1f) / 2f));
						UnityEngine.Vector3 spreadTarget = rotation * (targetPoint - basePoint) + basePoint;
						//子彈設置
						World.world.projectiles.spawn(pInitiator: selfActor, pTargetObject: targetActor, pAssetID: "torch", pLaunchPosition: basePoint, pTargetPosition: spreadTarget, pTargetZ: 0.0f);
					}
					return true;
				}
				return false;
			}
			return false;
		}
		public static bool Projectile_O05(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 子彈攻擊 炸彈
			if (pTarget != null)
			{
				Actor selfActor = pSelf as Actor;
				Actor targetActor = pTarget as Actor;
				string requiredStatus = "crosshair"; // 新增：需要檢查的狀態
				if (selfActor == null)
				{
					return false;
				}
				// === 新增：冷卻狀態檢查邏輯 ===
				string attackCooldownStatus = "cdt_atk05";
				float attackCooldownDuration = 0.1f; // 可以根據遊戲平衡需求調整
				string debuffStatus = "cdt_debuff01";
				// 檢查單位是否持有任何冷卻或負面狀態，如果有的話就不發動
				if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus(debuffStatus))
				{
					return false;
				}
				// === 修改這裡的條件判斷 ===
				// 現在只檢查 selfActor 是否具有 "crosshair" 狀態
				if (selfActor.hasStatus(requiredStatus))
				{
					if (targetActor == null)
					{
						//Debug.LogWarning("[Projectile_10] 目標 Actor 為 null，無法執行投射物邏輯。");
						return false;
					}
					// === 冷卻狀態施加 ===
					// 在發動後，立即為單位施加冷卻狀態
					selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
		 // 保持為施加動作
		 // 保持為施加動作
					UnityEngine.Vector2Int pos = pTile.pos;
					UnityEngine.Vector3 selfPosition = selfActor.current_position;
					UnityEngine.Vector3 targetPosition = targetActor.current_position;
					float pDist = UnityEngine.Vector3.Distance(targetPosition, new UnityEngine.Vector3(pos.x, pos.y, 0f));
					float targetSize = 1f;
					if (targetActor != null && targetActor.stats != null)
					{
						targetSize = targetActor.stats["size"];
					}
					UnityEngine.Vector3 basePoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, (float)pos.x, (float)pos.y, pDist, true);
					UnityEngine.Vector3 targetPoint = Toolbox.getNewPoint(targetPosition.x, targetPosition.y, (float)pos.x, (float)pos.y, targetSize, true);
					int numberOfProjectiles = 5;
					float spreadAngle = 5f;
					for (int i = 0; i < numberOfProjectiles; i++)
					{
						UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, spreadAngle * (i - (numberOfProjectiles - 1f) / 2f));
						UnityEngine.Vector3 spreadTarget = rotation * (targetPoint - basePoint) + basePoint;
						//子彈設置
						World.world.projectiles.spawn(pInitiator: selfActor, pTargetObject: targetActor, pAssetID: "firebomb", pLaunchPosition: basePoint, pTargetPosition: spreadTarget, pTargetZ: 0.0f);
					}
					return true;
				}
				return false;
			}
			return false;
		}
		public static bool Projectile_O06(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 子彈攻擊 死靈骨與頭骨(雙類子彈)
			if (pTarget != null)
			{
				Actor selfActor = pSelf as Actor;
				Actor targetActor = pTarget as Actor;
				string requiredStatus = "crosshair";
				if (selfActor == null)
				{
					return false;
				}
				// === 新增：冷卻狀態檢查邏輯 ===
				string attackCooldownStatus = "cdt_atk05";
				float attackCooldownDuration = 0.1f; // 可以根據遊戲平衡需求調整
				string debuffStatus = "cdt_debuff01";
				// 檢查單位是否持有任何冷卻或負面狀態，如果有的話就不發動
				if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus(debuffStatus))
				{
					return false;
				}
				if (selfActor.hasStatus(requiredStatus))
				{
					if (targetActor == null)
					{
						//Debug.LogWarning("[Projectile_06] 目標 Actor 為 null，無法執行投射物邏輯。");
						return false;
					}
					// === 冷卻狀態施加 ===
					// 在發動後，立即為單位施加冷卻狀態
					selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
		
					UnityEngine.Vector2Int pos = pTile.pos;
					UnityEngine.Vector3 selfPosition = selfActor.current_position;
					UnityEngine.Vector3 targetPosition = targetActor.current_position;
					float pDist = UnityEngine.Vector3.Distance(targetPosition, new UnityEngine.Vector3(pos.x, pos.y, 0f));
					float targetSize = 1f;
					if (targetActor.stats != null)
					{
						targetSize = targetActor.stats["size"];
					}
					UnityEngine.Vector3 basePoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, (float)pos.x, (float)pos.y, pDist, true);
					UnityEngine.Vector3 targetPoint = Toolbox.getNewPoint(targetPosition.x, targetPosition.y, (float)pos.x, (float)pos.y, targetSize, true);
					// --- 為不同子彈類型定義獨立的設定 ---
					// 設定 死靈骨 (bone) 的參數
					int bone_numberOfProjectiles = 2;
					float bone_spreadAngle = 5f;
					// 設定 頭骨 (skull) 的參數
					int skull_numberOfProjectiles = 3;
					float skull_spreadAngle = 5f;
					// --- 發射 死靈骨 (bone) ---
					for (int i = 0; i < bone_numberOfProjectiles; i++)
					{
						UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, bone_spreadAngle * (i - (bone_numberOfProjectiles - 1f) / 2f));
						UnityEngine.Vector3 spreadTarget = rotation * (targetPoint - basePoint) + basePoint;
						World.world.projectiles.spawn(pInitiator: selfActor, pTargetObject: targetActor, pAssetID: "bone", pLaunchPosition: basePoint, pTargetPosition: spreadTarget, pTargetZ: 0.0f);
					}
					// --- 發射 頭骨 (skull) ---
					for (int i = 0; i < skull_numberOfProjectiles; i++)
					{
						UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, skull_spreadAngle * (i - (skull_numberOfProjectiles - 1f) / 2f));
						UnityEngine.Vector3 spreadTarget = rotation * (targetPoint - basePoint) + basePoint;
						World.world.projectiles.spawn(pInitiator: selfActor, pTargetObject: targetActor, pAssetID: "skull", pLaunchPosition: basePoint, pTargetPosition: spreadTarget, pTargetZ: 0.0f);
					}
					return true;
				}
			}
			return false;
		}
		public static bool Projectile_O07(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 子彈攻擊 邪法紅球
			if (pTarget != null)
			{
				Actor selfActor = pSelf as Actor;
				Actor targetActor = pTarget as Actor;
				string requiredStatus = "crosshair"; // 新增：需要檢查的狀態
				if (selfActor == null)
				{
					return false;
				}
				// === 新增：冷卻狀態檢查邏輯 ===
				string attackCooldownStatus = "cdt_atk05";
				float attackCooldownDuration = 0.1f; // 可以根據遊戲平衡需求調整
				string debuffStatus = "cdt_debuff01";
				// 檢查單位是否持有任何冷卻或負面狀態，如果有的話就不發動
				if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus(debuffStatus))
				{
					return false;
				}
				// === 修改這裡的條件判斷 ===
				// 現在只檢查 selfActor 是否具有 "crosshair" 狀態
				if (selfActor.hasStatus(requiredStatus))
				{
					if (targetActor == null)
					{
						//Debug.LogWarning("[Projectile_10] 目標 Actor 為 null，無法執行投射物邏輯。");
						return false; // 如果目標不是有效的 Actor，則直接返回
					}
					// === 冷卻狀態施加 ===
					// 在發動後，立即為單位施加冷卻狀態
					selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
		 // 保持為施加動作
		 // 保持為施加動作
					UnityEngine.Vector2Int pos = pTile.pos;
					UnityEngine.Vector3 selfPosition = selfActor.current_position;
					UnityEngine.Vector3 targetPosition = targetActor.current_position;
					float pDist = UnityEngine.Vector3.Distance(targetPosition, new UnityEngine.Vector3(pos.x, pos.y, 0f));
					float targetSize = 1f;
					if (targetActor != null && targetActor.stats != null)
					{
						targetSize = targetActor.stats["size"];
					}
					UnityEngine.Vector3 basePoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, (float)pos.x, (float)pos.y, pDist, true);
					UnityEngine.Vector3 targetPoint = Toolbox.getNewPoint(targetPosition.x, targetPosition.y, (float)pos.x, (float)pos.y, targetSize, true);
					int numberOfProjectiles = 5;
					float spreadAngle = 2f;
					for (int i = 0; i < numberOfProjectiles; i++)
					{
						UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, spreadAngle * (i - (numberOfProjectiles - 1f) / 2f));
						UnityEngine.Vector3 spreadTarget = rotation * (targetPoint - basePoint) + basePoint;
						//子彈設置
						World.world.projectiles.spawn(pInitiator: selfActor, pTargetObject: targetActor, pAssetID: "red_orb", pLaunchPosition: basePoint, pTargetPosition: spreadTarget, pTargetZ: 0.0f);
					}
					return true;
				}
			}
			return false;
		}
		public static bool Projectile_O08(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 子彈攻擊 白法冰球
			if (pTarget != null)
			{
				Actor selfActor = pSelf as Actor;
				Actor targetActor = pTarget as Actor;
				string requiredStatus = "crosshair"; // 新增：需要檢查的狀態
				if (selfActor == null)
				{
					return false;
				}
				// === 新增：冷卻狀態檢查邏輯 ===
				string attackCooldownStatus = "cdt_atk05";
				float attackCooldownDuration = 0.1f; // 可以根據遊戲平衡需求調整
				string debuffStatus = "cdt_debuff01";
				// 檢查單位是否持有任何冷卻或負面狀態，如果有的話就不發動
				if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus(debuffStatus))
				{
					return false;
				}
				// === 修改這裡的條件判斷 ===
				// 現在只檢查 selfActor 是否具有 "crosshair" 狀態
				if (selfActor.hasStatus(requiredStatus))
				{
					if (targetActor == null)
					{
						//Debug.LogWarning("[Projectile_10] 目標 Actor 為 null，無法執行投射物邏輯。");
						return false; // 如果目標不是有效的 Actor，則直接返回
					}
					// === 冷卻狀態施加 ===
					// 在發動後，立即為單位施加冷卻狀態
					selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
		 // 保持為施加動作
		 // 保持為施加動作
					UnityEngine.Vector2Int pos = pTile.pos;
					UnityEngine.Vector3 selfPosition = selfActor.current_position;
					UnityEngine.Vector3 targetPosition = targetActor.current_position;
					float pDist = UnityEngine.Vector3.Distance(targetPosition, new UnityEngine.Vector3(pos.x, pos.y, 0f));
					float targetSize = 1f;
					if (targetActor != null && targetActor.stats != null)
					{
						targetSize = targetActor.stats["size"];
					}
					UnityEngine.Vector3 basePoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, (float)pos.x, (float)pos.y, pDist, true);
					UnityEngine.Vector3 targetPoint = Toolbox.getNewPoint(targetPosition.x, targetPosition.y, (float)pos.x, (float)pos.y, targetSize, true);
					int numberOfProjectiles = 5;
					float spreadAngle = 1f;
					for (int i = 0; i < numberOfProjectiles; i++)
					{
						UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, spreadAngle * (i - (numberOfProjectiles - 1f) / 2f));
						UnityEngine.Vector3 spreadTarget = rotation * (targetPoint - basePoint) + basePoint;
						//子彈設置
						World.world.projectiles.spawn(pInitiator: selfActor, pTargetObject: targetActor, pAssetID: "freeze_orb", pLaunchPosition: basePoint, pTargetPosition: spreadTarget, pTargetZ: 0.0f);
					}
					return true;
				}
			}
			return false;
		}
		public static bool Projectile_O09(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 子彈攻擊 德魯綠球
			if (pTarget != null)
			{
				Actor selfActor = pSelf as Actor;
				Actor targetActor = pTarget as Actor;
				string requiredStatus = "crosshair"; // 新增：需要檢查的狀態
				if (selfActor == null)
				{
					return false;
				}
				// === 新增：冷卻狀態檢查邏輯 ===
				string attackCooldownStatus = "cdt_atk05";
				float attackCooldownDuration = 0.1f; // 可以根據遊戲平衡需求調整
				string debuffStatus = "cdt_debuff01";
				// 檢查單位是否持有任何冷卻或負面狀態，如果有的話就不發動
				if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus(debuffStatus))
				{
					return false;
				}
				// === 修改這裡的條件判斷 ===
				// 現在只檢查 selfActor 是否具有 "crosshair" 狀態
				if (selfActor.hasStatus(requiredStatus))
				{
					if (targetActor == null)
					{
						//Debug.LogWarning("[Projectile_10] 目標 Actor 為 null，無法執行投射物邏輯。");
						return false; // 如果目標不是有效的 Actor，則直接返回
					}
					// === 冷卻狀態施加 ===
					// 在發動後，立即為單位施加冷卻狀態
					selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
		 // 保持為施加動作
		 // 保持為施加動作
					UnityEngine.Vector2Int pos = pTile.pos;
					UnityEngine.Vector3 selfPosition = selfActor.current_position;
					UnityEngine.Vector3 targetPosition = targetActor.current_position;
					float pDist = UnityEngine.Vector3.Distance(targetPosition, new UnityEngine.Vector3(pos.x, pos.y, 0f));
					float targetSize = 1f;
					if (targetActor != null && targetActor.stats != null)
					{
						targetSize = targetActor.stats["size"];
					}
					UnityEngine.Vector3 basePoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, (float)pos.x, (float)pos.y, pDist, true);
					UnityEngine.Vector3 targetPoint = Toolbox.getNewPoint(targetPosition.x, targetPosition.y, (float)pos.x, (float)pos.y, targetSize, true);
					int numberOfProjectiles = 5;
					float spreadAngle = 1f;
					for (int i = 0; i < numberOfProjectiles; i++)
					{
						UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, spreadAngle * (i - (numberOfProjectiles - 1f) / 2f));
						UnityEngine.Vector3 spreadTarget = rotation * (targetPoint - basePoint) + basePoint;
						//子彈設置
						World.world.projectiles.spawn(pInitiator: selfActor, pTargetObject: targetActor, pAssetID: "green_orb", pLaunchPosition: basePoint, pTargetPosition: spreadTarget, pTargetZ: 0.0f);
					}
					return true;
				}
			}
			return false;
		}
		public static bool Projectile_O10(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 子彈攻擊 酸砲
			if (pTarget != null)
			{
				Actor selfActor = pSelf as Actor;
				Actor targetActor = pTarget as Actor;
				string requiredStatus = "crosshair"; // 新增：需要檢查的狀態
				if (selfActor == null)
				{
					return false;
				}
				// === 新增：冷卻狀態檢查邏輯 ===
				string attackCooldownStatus = "cdt_atk05";
				float attackCooldownDuration = 0.1f; // 可以根據遊戲平衡需求調整
				string debuffStatus = "cdt_debuff01";
				// 檢查單位是否持有任何冷卻或負面狀態，如果有的話就不發動
				if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus(debuffStatus))
				{
					return false;
				}
				// === 修改這裡的條件判斷 ===
				// 現在只檢查 selfActor 是否具有 "crosshair" 狀態
				if (selfActor.hasStatus(requiredStatus))
				{
					if (targetActor == null)
					{
						//Debug.LogWarning("[Projectile_10] 目標 Actor 為 null，無法執行投射物邏輯。");
						return false; // 如果目標不是有效的 Actor，則直接返回
					}
					// === 冷卻狀態施加 ===
					// 在發動後，立即為單位施加冷卻狀態
					selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
		 // 保持為施加動作
		 // 保持為施加動作
					UnityEngine.Vector2Int pos = pTile.pos;
					UnityEngine.Vector3 selfPosition = selfActor.current_position;
					UnityEngine.Vector3 targetPosition = targetActor.current_position;
					float pDist = UnityEngine.Vector3.Distance(targetPosition, new UnityEngine.Vector3(pos.x, pos.y, 0f));
					float targetSize = 1f;
					if (targetActor != null && targetActor.stats != null)
					{
						targetSize = targetActor.stats["size"];
					}
					UnityEngine.Vector3 basePoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, (float)pos.x, (float)pos.y, pDist, true);
					UnityEngine.Vector3 targetPoint = Toolbox.getNewPoint(targetPosition.x, targetPosition.y, (float)pos.x, (float)pos.y, targetSize, true);
					int numberOfProjectiles = 5;
					float spreadAngle = 5f;
					for (int i = 0; i < numberOfProjectiles; i++)
					{
						UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, spreadAngle * (i - (numberOfProjectiles - 1f) / 2f));
						UnityEngine.Vector3 spreadTarget = rotation * (targetPoint - basePoint) + basePoint;
						//子彈設置
						World.world.projectiles.spawn(pInitiator: selfActor, pTargetObject: targetActor, pAssetID: "acid_ball", pLaunchPosition: basePoint, pTargetPosition: spreadTarget, pTargetZ: 0.0f);
					}
					return true;
				}
			}
			return false;
		}
		public static bool Projectile_O11(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 子彈攻擊 砲彈
			if (pTarget != null)
			{
				Actor selfActor = pSelf as Actor;
				Actor targetActor = pTarget as Actor;
				string requiredStatus = "crosshair"; // 新增：需要檢查的狀態
				if (selfActor == null)
				{
					return false;
				}
				// === 新增：冷卻狀態檢查邏輯 ===
				string attackCooldownStatus = "cdt_atk05";
				float attackCooldownDuration = 0.1f; // 可以根據遊戲平衡需求調整
				string debuffStatus = "cdt_debuff01";
				// 檢查單位是否持有任何冷卻或負面狀態，如果有的話就不發動
				if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus(debuffStatus))
				{
					return false;
				}
				// === 修改這裡的條件判斷 ===
				// 現在只檢查 selfActor 是否具有 "crosshair" 狀態
				if (selfActor.hasStatus(requiredStatus))
				{
					if (targetActor == null)
					{
						//Debug.LogWarning("[Projectile_10] 目標 Actor 為 null，無法執行投射物邏輯。");
						return false; // 如果目標不是有效的 Actor，則直接返回
					}
					// === 冷卻狀態施加 ===
					// 在發動後，立即為單位施加冷卻狀態
					selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
		 // 保持為施加動作
		 // 保持為施加動作
					UnityEngine.Vector2Int pos = pTile.pos;
					UnityEngine.Vector3 selfPosition = selfActor.current_position;
					UnityEngine.Vector3 targetPosition = targetActor.current_position;
					float pDist = UnityEngine.Vector3.Distance(targetPosition, new UnityEngine.Vector3(pos.x, pos.y, 0f));
					float targetSize = 1f;
					if (targetActor != null && targetActor.stats != null)
					{
						targetSize = targetActor.stats["size"];
					}
					UnityEngine.Vector3 basePoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, (float)pos.x, (float)pos.y, pDist, true);
					UnityEngine.Vector3 targetPoint = Toolbox.getNewPoint(targetPosition.x, targetPosition.y, (float)pos.x, (float)pos.y, targetSize, true);
					int numberOfProjectiles = 5;
					float spreadAngle = 5f;
					for (int i = 0; i < numberOfProjectiles; i++)
					{
						UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, spreadAngle * (i - (numberOfProjectiles - 1f) / 2f));
						UnityEngine.Vector3 spreadTarget = rotation * (targetPoint - basePoint) + basePoint;
						//子彈設置
						World.world.projectiles.spawn(pInitiator: selfActor, pTargetObject: targetActor, pAssetID: "cannonball", pLaunchPosition: basePoint, pTargetPosition: spreadTarget, pTargetZ: 0.0f);
					}
					return true;
				}
			}
			return false;
		}
		public static bool Projectile_O12(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 子彈攻擊 外星鎗
			if (pTarget != null)
			{
				Actor selfActor = pSelf as Actor;
				Actor targetActor = pTarget as Actor;
				string requiredStatus = "crosshair"; // 新增：需要檢查的狀態
				if (selfActor == null)
				{
					return false;
				}
				// === 新增：冷卻狀態檢查邏輯 ===
				string attackCooldownStatus = "cdt_atk05";
				float attackCooldownDuration = 0.1f; // 可以根據遊戲平衡需求調整
				string debuffStatus = "cdt_debuff01";
				// 檢查單位是否持有任何冷卻或負面狀態，如果有的話就不發動
				if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus(debuffStatus))
				{
					return false;
				}
				// === 修改這裡的條件判斷 ===
				// 現在只檢查 selfActor 是否具有 "crosshair" 狀態
				if (selfActor.hasStatus(requiredStatus))
				{
					if (targetActor == null)
					{
						//Debug.LogWarning("[Projectile_10] 目標 Actor 為 null，無法執行投射物邏輯。");
						return false; // 如果目標不是有效的 Actor，則直接返回
					}
					// === 冷卻狀態施加 ===
					// 在發動後，立即為單位施加冷卻狀態
					selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
		 // 保持為施加動作
		 // 保持為施加動作
					UnityEngine.Vector2Int pos = pTile.pos;
					UnityEngine.Vector3 selfPosition = selfActor.current_position;
					UnityEngine.Vector3 targetPosition = targetActor.current_position;
					float pDist = UnityEngine.Vector3.Distance(targetPosition, new UnityEngine.Vector3(pos.x, pos.y, 0f));
					float targetSize = 1f;
					if (targetActor != null && targetActor.stats != null)
					{
						targetSize = targetActor.stats["size"];
					}
					UnityEngine.Vector3 basePoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, (float)pos.x, (float)pos.y, pDist, true);
					UnityEngine.Vector3 targetPoint = Toolbox.getNewPoint(targetPosition.x, targetPosition.y, (float)pos.x, (float)pos.y, targetSize, true);
					int numberOfProjectiles = 5;
					float spreadAngle = 2.5f;
					for (int i = 0; i < numberOfProjectiles; i++)
					{
						UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, spreadAngle * (i - (numberOfProjectiles - 1f) / 2f));
						UnityEngine.Vector3 spreadTarget = rotation * (targetPoint - basePoint) + basePoint;
						//子彈設置
						World.world.projectiles.spawn(pInitiator: selfActor, pTargetObject: targetActor, pAssetID: "plasma_ball", pLaunchPosition: basePoint, pTargetPosition: spreadTarget, pTargetZ: 0.0f);
					}
					return true;
				}
			}
			return false;
		}
		public static bool Projectile_O13(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 子彈攻擊 霰彈槍
			if (pTarget != null)
			{
				Actor selfActor = pSelf as Actor;
				Actor targetActor = pTarget as Actor;
				string requiredStatus = "crosshair"; // 新增：需要檢查的狀態
				if (selfActor == null)
				{
					return false;
				}
				// === 新增：冷卻狀態檢查邏輯 ===
				string attackCooldownStatus = "cdt_atk05";
				float attackCooldownDuration = 0.1f; // 可以根據遊戲平衡需求調整
				string debuffStatus = "cdt_debuff01";
				// 檢查單位是否持有任何冷卻或負面狀態，如果有的話就不發動
				if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus(debuffStatus))
				{
					return false;
				}
				// === 修改這裡的條件判斷 ===
				// 現在只檢查 selfActor 是否具有 "crosshair" 狀態
				if (selfActor.hasStatus(requiredStatus))
				{
					if (targetActor == null)
					{
						//Debug.LogWarning("[Projectile_10] 目標 Actor 為 null，無法執行投射物邏輯。");
						return false; // 如果目標不是有效的 Actor，則直接返回
					}
					// === 冷卻狀態施加 ===
					// 在發動後，立即為單位施加冷卻狀態
					selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
		 // 保持為施加動作
					UnityEngine.Vector2Int pos = pTile.pos;
					UnityEngine.Vector3 selfPosition = selfActor.current_position;
					UnityEngine.Vector3 targetPosition = targetActor.current_position;
					float pDist = UnityEngine.Vector3.Distance(targetPosition, new UnityEngine.Vector3(pos.x, pos.y, 0f));
					float targetSize = 1f;
					if (targetActor != null && targetActor.stats != null)
					{
						targetSize = targetActor.stats["size"];
					}
					UnityEngine.Vector3 basePoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, (float)pos.x, (float)pos.y, pDist, true);
					UnityEngine.Vector3 targetPoint = Toolbox.getNewPoint(targetPosition.x, targetPosition.y, (float)pos.x, (float)pos.y, targetSize, true);
					int numberOfProjectiles = 20;
					float spreadAngle = 0.75f;
					for (int i = 0; i < numberOfProjectiles; i++)
					{
						UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, spreadAngle * (i - (numberOfProjectiles - 1f) / 2f));
						UnityEngine.Vector3 spreadTarget = rotation * (targetPoint - basePoint) + basePoint;
						//子彈設置
						World.world.projectiles.spawn(pInitiator: selfActor, pTargetObject: targetActor, pAssetID: "shotgun_bullet", pLaunchPosition: basePoint, pTargetPosition: spreadTarget, pTargetZ: 0.0f);
					}
					return true;
				}
			}
			return false;
		}
		public static bool Projectile_O14(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 子彈攻擊 瘋狂之球
			if (pTarget != null)
			{
				Actor selfActor = pSelf as Actor;
				Actor targetActor = pTarget as Actor;
				string requiredStatus = "crosshair"; // 新增：需要檢查的狀態
				if (selfActor == null)
				{
					return false;
				}
				// === 新增：冷卻狀態檢查邏輯 ===
				string attackCooldownStatus = "cdt_atk05";
				float attackCooldownDuration = 0.1f; // 可以根據遊戲平衡需求調整
				string debuffStatus = "cdt_debuff01";
				// 檢查單位是否持有任何冷卻或負面狀態，如果有的話就不發動
				if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus(debuffStatus))
				{
					return false;
				}
				// === 修改這裡的條件判斷 ===
				// 現在只檢查 selfActor 是否具有 "crosshair" 狀態
				if (selfActor.hasStatus(requiredStatus))
				{
					if (targetActor == null)
					{
						//Debug.LogWarning("[Projectile_10] 目標 Actor 為 null，無法執行投射物邏輯。");
						return false; // 如果目標不是有效的 Actor，則直接返回
					}
					// === 冷卻狀態施加 ===
					// 在發動後，立即為單位施加冷卻狀態
					selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
		 // 保持為施加動作
					selfActor.addTrait("strong_minded"); // 保持為施加動作
					UnityEngine.Vector2Int pos = pTile.pos;
					UnityEngine.Vector3 selfPosition = selfActor.current_position;
					UnityEngine.Vector3 targetPosition = targetActor.current_position;
					float pDist = UnityEngine.Vector3.Distance(targetPosition, new UnityEngine.Vector3(pos.x, pos.y, 0f));
					float targetSize = 1f;
					if (targetActor != null && targetActor.stats != null)
					{
						targetSize = targetActor.stats["size"];
					}
					UnityEngine.Vector3 basePoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, (float)pos.x, (float)pos.y, pDist, true);
					UnityEngine.Vector3 targetPoint = Toolbox.getNewPoint(targetPosition.x, targetPosition.y, (float)pos.x, (float)pos.y, targetSize, true);
					int numberOfProjectiles = 5;
					float spreadAngle = 5f;
					for (int i = 0; i < numberOfProjectiles; i++)
					{
						UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, spreadAngle * (i - (numberOfProjectiles - 1f) / 2f));
						UnityEngine.Vector3 spreadTarget = rotation * (targetPoint - basePoint) + basePoint;
						//子彈設置
						World.world.projectiles.spawn(pInitiator: selfActor, pTargetObject: targetActor, pAssetID: "madness_ball", pLaunchPosition: basePoint, pTargetPosition: spreadTarget, pTargetZ: 0.0f);
					}
					return true;
				}
			}
			return false;
		}
		public static bool Projectile_O15(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 子彈攻擊 火塔火球
			if (pTarget != null)
			{
				Actor selfActor = pSelf as Actor;
				Actor targetActor = pTarget as Actor;
				string requiredStatus = "crosshair"; // 新增：需要檢查的狀態
				if (selfActor == null)
				{
					return false;
				}
				// === 新增：冷卻狀態檢查邏輯 ===
				string attackCooldownStatus = "cdt_atk05";
				float attackCooldownDuration = 0.1f; // 可以根據遊戲平衡需求調整
				string debuffStatus = "cdt_debuff01";
				// 檢查單位是否持有任何冷卻或負面狀態，如果有的話就不發動
				if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus(debuffStatus))
				{
					return false;
				}
				// === 修改這裡的條件判斷 ===
				// 現在只檢查 selfActor 是否具有 "crosshair" 狀態
				if (selfActor.hasStatus(requiredStatus))
				{
					if (targetActor == null)
					{
						//Debug.LogWarning("[Projectile_10] 目標 Actor 為 null，無法執行投射物邏輯。");
						return false; // 如果目標不是有效的 Actor，則直接返回
					}
					// === 冷卻狀態施加 ===
					// 在發動後，立即為單位施加冷卻狀態
					selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
		 // 保持為施加動作
		 // 保持為施加動作
					UnityEngine.Vector2Int pos = pTile.pos;
					UnityEngine.Vector3 selfPosition = selfActor.current_position;
					UnityEngine.Vector3 targetPosition = targetActor.current_position;
					float pDist = UnityEngine.Vector3.Distance(targetPosition, new UnityEngine.Vector3(pos.x, pos.y, 0f));
					float targetSize = 1f;
					if (targetActor != null && targetActor.stats != null)
					{
						targetSize = targetActor.stats["size"];
					}
					UnityEngine.Vector3 basePoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, (float)pos.x, (float)pos.y, pDist, true);
					UnityEngine.Vector3 targetPoint = Toolbox.getNewPoint(targetPosition.x, targetPosition.y, (float)pos.x, (float)pos.y, targetSize, true);
					int numberOfProjectiles = 5;
					float spreadAngle = 10f;
					for (int i = 0; i < numberOfProjectiles; i++)
					{
						UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0f, 0f, spreadAngle * (i - (numberOfProjectiles - 1f) / 2f));
						UnityEngine.Vector3 spreadTarget = rotation * (targetPoint - basePoint) + basePoint;
						//子彈設置
						World.world.projectiles.spawn(pInitiator: selfActor, pTargetObject: targetActor, pAssetID: "fireball", pLaunchPosition: basePoint, pTargetPosition: spreadTarget, pTargetZ: 0.0f);
					}
					return true;
				}
			}
			return false;
		}
*/
			#endregion
			#region 攻擊添加負面狀態
	//附加攻擊 cdt_atk00 共用
		public static bool Burning_Effect2(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 燃燒 攻擊用 新
			if (pSelf == null || pTarget == null)
			{
				return false; // 攻擊者或目標為空，不執行
			}

			Actor selfActor = pSelf.a;
			Actor targetActor = pTarget.a;

			if (selfActor == null || targetActor == null || !selfActor.isAlive() || !targetActor.isAlive())
			{
				return false; // 攻擊者或目標 Actor 無效或已死亡，不執行
			}

			// 定義冷卻狀態 ID 和持續時間，以及目標狀態 ID 和持續時間
			string selfAtkStatus = "cdt_atk00";	 // 攻擊冷卻狀態 ID
			float selfAtkDuration = 11f;			 // 自身冷卻時間
			string selfDebuffStatus = "cdt_debuff01"; // 另一個可能影響發動的自身debuff狀態 ID
			string targetAddStatus = "burning";	 // 目標將獲得的狀態 ID
			float targetAddDuration = 10f;		  // 目標狀態持續時間

			// **核心修改：將所有能力執行邏輯都包裹在冷卻檢查內部**
			// 檢查自身是否沒有 cdt_atk00 且沒有 cdt_debuff01 狀態
			if (!selfActor.hasStatus(selfAtkStatus) && !selfActor.hasStatus(selfDebuffStatus))
			{
				// --- 能力發動區塊 ---

				// 1. 給予自身 cdt_atk00 冷卻狀態
				// 這裡使用 addStatusEffect 也是可以的，因為它符合 "如果沒有就添加" 的邏輯
				selfActor.addStatusEffect(selfAtkStatus, selfAtkDuration);
				//Debug.Log($"{selfActor.name} 獲得了 {selfAtkStatus}，持續 {selfAtkDuration} 秒");

				// 2. 為目標施加 Burning 狀態效果
				targetActor.addStatusEffect(targetAddStatus, targetAddDuration);
				//Debug.Log($"{targetActor.name} 獲得了 {targetAddStatus}，持續 {targetAddDuration} 秒");

				return true; // 能力成功發動並進入冷卻
			}
			else
			{
				// 如果自身處於冷卻狀態 (有 cdt_atk00 或 cdt_debuff01)，則不發動能力
				return false; 
			}
		}
		public static bool Frozen_Effect2(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 冰凍 攻擊用 新
			if (pSelf == null || pTarget == null)
			{
				return false; // 攻擊者或目標為空，不執行
			}

			Actor selfActor = pSelf.a;
			Actor targetActor = pTarget.a;

			if (selfActor == null || targetActor == null || !selfActor.isAlive() || !targetActor.isAlive())
			{
				return false; // 攻擊者或目標 Actor 無效或已死亡，不執行
			}

			// 定義冷卻狀態 ID 和持續時間，以及目標狀態 ID 和持續時間
			string selfAtkStatus = "cdt_atk00";	 // 攻擊冷卻狀態 ID
			string selfDebuffStatus = "cdt_debuff01"; // 另一個可能影響發動的自身debuff狀態 ID
			float selfAtkDuration = 20f;			 // 自身冷卻時間
			string targetAddStatus = "frozen";	 // 目標將獲得的狀態 ID
			float targetAddDuration = 15f;		  // 目標狀態持續時間

			// **核心修改：將所有能力執行邏輯都包裹在冷卻檢查內部**
			// 檢查自身是否沒有 cdt_atk00 且沒有 cdt_debuff01 狀態
			if (!selfActor.hasStatus(selfAtkStatus) && !selfActor.hasStatus(selfDebuffStatus))
			{
				// --- 能力發動區塊 ---

				// 1. 給予自身 cdt_atk00 冷卻狀態
				// 這裡使用 addStatusEffect 也是可以的，因為它符合 "如果沒有就添加" 的邏輯
				selfActor.addStatusEffect(selfAtkStatus, selfAtkDuration);
				//Debug.Log($"{selfActor.name} 獲得了 {selfAtkStatus}，持續 {selfAtkDuration} 秒");

				// 2. 為目標施加 frozen 狀態效果
				targetActor.addStatusEffect(targetAddStatus, targetAddDuration);
				//Debug.Log($"{targetActor.name} 獲得了 {targetAddStatus}，持續 {targetAddDuration} 秒");

				return true; // 能力成功發動並進入冷卻
			}
			else
			{
				// 如果自身處於冷卻狀態 (有 cdt_atk00 或 cdt_debuff01)，則不發動能力
				return false; 
			}
		}
		public static bool Slow_Effect2(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 緩速 攻擊用 新
			if (pSelf == null || pTarget == null)
			{
				return false; // 攻擊者或目標為空，不執行
			}

			Actor selfActor = pSelf.a;
			Actor targetActor = pTarget.a;

			if (selfActor == null || targetActor == null || !selfActor.isAlive() || !targetActor.isAlive())
			{
				return false; // 攻擊者或目標 Actor 無效或已死亡，不執行
			}

			// 定義冷卻狀態 ID 和持續時間，以及目標狀態 ID 和持續時間
			string selfAtkStatus = "cdt_atk00";	 // 攻擊冷卻狀態 ID
			string selfDebuffStatus = "cdt_debuff01"; // 另一個可能影響發動的自身debuff狀態 ID
			float selfAtkDuration = 11f;			 // 自身冷卻時間
			string targetAddStatus = "slowness";	 // 目標將獲得的狀態 ID
			float targetAddDuration = 10f;		  // 目標狀態持續時間

			// **核心修改：將所有能力執行邏輯都包裹在冷卻檢查內部**
			// 檢查自身是否沒有 cdt_atk00 且沒有 cdt_debuff01 狀態
			if (!selfActor.hasStatus(selfAtkStatus) && !selfActor.hasStatus(selfDebuffStatus))
			{
				// --- 能力發動區塊 ---

				// 1. 給予自身 cdt_atk00 冷卻狀態
				// 這裡使用 addStatusEffect 也是可以的，因為它符合 "如果沒有就添加" 的邏輯
				selfActor.addStatusEffect(selfAtkStatus, selfAtkDuration);
				//Debug.Log($"{selfActor.name} 獲得了 {selfAtkStatus}，持續 {selfAtkDuration} 秒");

				// 2. 為目標施加 slowness 狀態效果
				targetActor.addStatusEffect(targetAddStatus, targetAddDuration);
				//Debug.Log($"{targetActor.name} 獲得了 {targetAddStatus}，持續 {targetAddDuration} 秒");

				return true; // 能力成功發動並進入冷卻
			}
			else
			{
				// 如果自身處於冷卻狀態 (有 cdt_atk00 或 cdt_debuff01)，則不發動能力
				return false; 
			}
		}
		public static bool Poisoned_Effect2(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 猛毒 攻擊用 新
			if (pSelf == null || pTarget == null)
			{
				return false; // 攻擊者或目標為空，不執行
			}
			Actor selfActor = pSelf.a;
			Actor targetActor = pTarget.a;
			if (selfActor == null || targetActor == null || !selfActor.isAlive() || !targetActor.isAlive())
			{
				return false; // 攻擊者或目標 Actor 無效或已死亡，不執行
			}

			// 定義冷卻狀態 ID 和持續時間，以及目標狀態 ID 和持續時間
			string selfAtkStatus = "cdt_atk00";	 // 攻擊冷卻狀態 ID
			string selfDebuffStatus = "cdt_debuff01"; // 另一個可能影響發動的自身debuff狀態 ID
			float selfAtkDuration = 16f;			 // 自身冷卻時間
			string targetAddStatus = "poisoned";	 // 目標將獲得的狀態 ID
			float targetAddDuration = 15f;		  // 目標狀態持續時間

			// **核心修改：將所有能力執行邏輯都包裹在冷卻檢查內部**
			// 檢查自身是否沒有 cdt_atk00 且沒有 cdt_debuff01 狀態
			if (!selfActor.hasStatus(selfAtkStatus) && !selfActor.hasStatus(selfDebuffStatus))
			{
				// --- 能力發動區塊 ---

				// 1. 給予自身 cdt_atk00 冷卻狀態
				// 這裡使用 addStatusEffect 也是可以的，因為它符合 "如果沒有就添加" 的邏輯
				selfActor.addStatusEffect(selfAtkStatus, selfAtkDuration);
				//Debug.Log($"{selfActor.name} 獲得了 {selfAtkStatus}，持續 {selfAtkDuration} 秒");

				// 2. 為目標施加 slowness 狀態效果
				targetActor.addStatusEffect(targetAddStatus, targetAddDuration);
				//Debug.Log($"{targetActor.name} 獲得了 {targetAddStatus}，持續 {targetAddDuration} 秒");

				return true; // 能力成功發動並進入冷卻
			}
			else
			{
				// 如果自身處於冷卻狀態 (有 cdt_atk00 或 cdt_debuff01)，則不發動能力
				return false; 
			}
		}
		public static bool AshFeverAndCoughingEffect2(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 灰病與咳嗽 新
			if (pSelf == null || pTarget == null)
			{
				return false; // 攻擊者或目標為空，不執行
			}

			Actor selfActor = pSelf.a;
			Actor targetActor = pTarget.a;

			if (selfActor == null || targetActor == null || !selfActor.isAlive() || !targetActor.isAlive())
			{
				return false; // 攻擊者或目標 Actor 無效或已死亡，不執行
			}
			// 定義狀態 ID 和持續時間
			string selfAtkStatus = "cdt_atk00";	 // 自身冷卻狀態 ID
			float selfAtkDuration = 16f;			 // 自身冷卻時間
			string selfDebuffStatus = "cdt_debuff01"; // 另一個可能影響發動的自身debuff狀態 ID
			string targetAddStatus1 = "ash_fever";  // 目標將獲得的第一個狀態 ID
			float targetAddDuration1 = 15f;		 // 第一個狀態持續時間
			string targetAddStatus2 = "cough";	  // 目標將獲得的第二個狀態 ID
			float targetAddDuration2 = 15f;		 // 第二個狀態持續時間
			// **核心修改：將所有能力執行邏輯都包裹在冷卻檢查內部**
			// 檢查自身是否沒有 cdt_atk00 且沒有 cdt_debuff01 狀態
			if (!selfActor.hasStatus(selfAtkStatus) && !selfActor.hasStatus(selfDebuffStatus))
			{
				// --- 能力發動區塊 ---
				// 1. 給予自身 cdt_atk00 冷卻狀態
				selfActor.addStatusEffect(selfAtkStatus, selfAtkDuration);
				//Debug.Log($"{selfActor.name} 獲得了 {selfAtkStatus}，持續 {selfAtkDuration} 秒");
				// 2. 為目標施加第一個狀態效果 (ash_fever)
				targetActor.addStatusEffect(targetAddStatus1, targetAddDuration1);
				//Debug.Log($"{targetActor.name} 獲得了 {targetAddStatus1}，持續 {targetAddDuration1} 秒");
				// 3. 為目標施加第二個狀態效果 (cough)
				targetActor.addStatusEffect(targetAddStatus2, targetAddDuration2);
				//Debug.Log($"{targetActor.name} 獲得了 {targetAddStatus2}，持續 {targetAddDuration2} 秒");

				return true; // 能力成功發動並進入冷卻
			}
			else
			{
				// 如果自身處於冷卻狀態 (有 cdt_atk00 或 cdt_debuff01)，則不發動能力
				return false; 
			}
		}
		public static bool Silenced_Effect(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 沉默 攻擊用 新
			if (pSelf == null || pTarget == null)
			{
				return false; // 攻擊者或目標為空，不執行
			}
			Actor selfActor = pSelf.a;
			Actor targetActor = pTarget.a;
			if (selfActor == null || targetActor == null || !selfActor.isAlive() || !targetActor.isAlive())
			{
				return false; // 攻擊者或目標 Actor 無效或已死亡，不執行
			}

			// 定義冷卻狀態 ID 和持續時間，以及目標狀態 ID 和持續時間
			string selfAtkStatus = "cdt_atk00";	 // 攻擊冷卻狀態 ID
			string selfDebuffStatus = "cdt_debuff01"; // 另一個可能影響發動的自身debuff狀態 ID
			float selfAtkDuration = 16f;			 // 自身冷卻時間
			string targetAddStatus = "spell_silence";	 // 目標將獲得的狀態 ID
			float targetAddDuration = 15f;		  // 目標狀態持續時間

			// **核心修改：將所有能力執行邏輯都包裹在冷卻檢查內部**
			// 檢查自身是否沒有 cdt_atk00 且沒有 cdt_debuff01 狀態
			if (!selfActor.hasStatus(selfAtkStatus) && !selfActor.hasStatus(selfDebuffStatus))
			{
				// --- 能力發動區塊 ---

				// 1. 給予自身 cdt_atk00 冷卻狀態
				// 這裡使用 addStatusEffect 也是可以的，因為它符合 "如果沒有就添加" 的邏輯
				selfActor.addStatusEffect(selfAtkStatus, selfAtkDuration);
				//Debug.Log($"{selfActor.name} 獲得了 {selfAtkStatus}，持續 {selfAtkDuration} 秒");

				// 2. 為目標施加 spell_silence 狀態效果
				targetActor.addStatusEffect(targetAddStatus, targetAddDuration);
				//Debug.Log($"{targetActor.name} 獲得了 {targetAddStatus}，持續 {targetAddDuration} 秒");

				return true; // 能力成功發動並進入冷卻
			}
			else
			{
				// 如果自身處於冷卻狀態 (有 cdt_atk00 或 cdt_debuff01)，則不發動能力
				return false; 
			}
		}
		public static bool Stunned_Effect(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 眩暈 攻擊用 新
			if (pSelf == null || pTarget == null)
			{
				return false; // 攻擊者或目標為空，不執行
			}
			Actor selfActor = pSelf.a;
			Actor targetActor = pTarget.a;
			if (selfActor == null || targetActor == null || !selfActor.isAlive() || !targetActor.isAlive())
			{
				return false; // 攻擊者或目標 Actor 無效或已死亡，不執行
			}
			// 定義冷卻狀態 ID 和持續時間，以及目標狀態 ID 和持續時間
			string selfAtkStatus = "cdt_atk00";	 // 攻擊冷卻狀態 ID
			string selfDebuffStatus = "cdt_debuff01"; // 另一個可能影響發動的自身debuff狀態 ID
			float selfAtkDuration = 12f;			 // 自身冷卻時間
			string targetAddStatus = "stunned";	 // 目標將獲得的狀態 ID
			float targetAddDuration = 10f;		  // 目標狀態持續時間

			// **核心修改：將所有能力執行邏輯都包裹在冷卻檢查內部**
			// 檢查自身是否沒有 cdt_atk00 且沒有 cdt_debuff01 狀態
			if (!selfActor.hasStatus(selfAtkStatus) && !selfActor.hasStatus(selfDebuffStatus))
			{
				// --- 能力發動區塊 ---

				// 1. 給予自身 cdt_atk00 冷卻狀態
				// 這裡使用 addStatusEffect 也是可以的，因為它符合 "如果沒有就添加" 的邏輯
				selfActor.addStatusEffect(selfAtkStatus, selfAtkDuration);
				//Debug.Log($"{selfActor.name} 獲得了 {selfAtkStatus}，持續 {selfAtkDuration} 秒");

				// 2. 為目標施加 stunned 狀態效果
				targetActor.addStatusEffect(targetAddStatus, targetAddDuration);
				//Debug.Log($"{targetActor.name} 獲得了 {targetAddStatus}，持續 {targetAddDuration} 秒");

				return true; // 能力成功發動並進入冷卻
			}
			else
			{
				// 如果自身處於冷卻狀態 (有 cdt_atk00 或 cdt_debuff01)，則不發動能力
				return false; 
			}
		}
		public static bool Drowning_Effect(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 溺水 攻擊用 新
			if (pSelf == null || pTarget == null)
			{
				return false; // 攻擊者或目標為空，不執行
			}
			Actor selfActor = pSelf.a;
			Actor targetActor = pTarget.a;
			if (selfActor == null || targetActor == null || !selfActor.isAlive() || !targetActor.isAlive())
			{
				return false; // 攻擊者或目標 Actor 無效或已死亡，不執行
			}
			// 定義冷卻狀態 ID 和持續時間，以及目標狀態 ID 和持續時間
			string selfAtkStatus = "cdt_atk00";			// 攻擊冷卻狀態 ID
			string selfDebuffStatus = "cdt_debuff01";	// 另一個可能影響發動的自身debuff狀態 ID
			float selfAtkDuration = 11f;			 	// 自身冷卻時間
			string targetAddStatus = "drowning";	 	// 目標將獲得的狀態 ID
			float targetAddDuration = 10f;				// 目標狀態持續時間
			if (!selfActor.hasStatus(selfAtkStatus) && !selfActor.hasStatus(selfDebuffStatus))
			{
				selfActor.addStatusEffect(selfAtkStatus, selfAtkDuration);
				targetActor.addStatusEffect(targetAddStatus, targetAddDuration);
				return true; // 能力成功發動並進入冷卻
			}
			else
			{
				// 如果自身處於冷卻狀態 (有 cdt_atk00 或 cdt_debuff01)，則不發動能力
				return false; 
			}
		}
		public static bool Confused_Effect(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 混亂 攻擊用 新
			if (pSelf == null || pTarget == null)
			{
				return false; // 攻擊者或目標為空，不執行
			}
			Actor selfActor = pSelf.a;
			Actor targetActor = pTarget.a;
			if (selfActor == null || targetActor == null || !selfActor.isAlive() || !targetActor.isAlive())
			{
				return false; // 攻擊者或目標 Actor 無效或已死亡，不執行
			}
			// 定義冷卻狀態 ID 和持續時間，以及目標狀態 ID 和持續時間
			string selfAtkStatus = "cdt_atk00";			// 攻擊冷卻狀態 ID
			string selfDebuffStatus = "cdt_debuff01";	// 另一個可能影響發動的自身debuff狀態 ID
			float selfAtkDuration = 10f;			 	// 自身冷卻時間
			string targetAddStatus = "confused";	 	// 目標將獲得的狀態 ID
			float targetAddDuration = 90f;				// 目標狀態持續時間
			if (!selfActor.hasStatus(selfAtkStatus) && !selfActor.hasStatus(selfDebuffStatus))
			{
				selfActor.addStatusEffect(selfAtkStatus, selfAtkDuration);
				targetActor.addStatusEffect(targetAddStatus, targetAddDuration);
				return true; // 能力成功發動並進入冷卻
			}
			else
			{
				// 如果自身處於冷卻狀態 (有 cdt_atk00 或 cdt_debuff01)，則不發動能力
				return false; 
			}
		}
		public static bool Random_Effect1(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 隨機 反擊用
			if (pTarget != null)
			{
				Actor targetActor = pTarget.a;
				if (targetActor != null)
				{
					// 定義一個包含所有可能效果的字串陣列
					string[] possibleEffects = { "slowness", "cough", "ash_fever", "frozen", "burning", "poisoned", "stunned", "sleeping", "spell_silence", "drowning", "confused" };
					// 隨機選擇一個效果的索引
					int randomIndex = UnityEngine.Random.Range(0, possibleEffects.Length);
					// 獲取隨機選擇的效果名稱
					string selectedEffect = possibleEffects[randomIndex];

					// 檢查目標是否已經有選定的狀態效果
					if (!targetActor.hasStatus(selectedEffect))
					{
						// 為目標施加隨機選擇的狀態效果
						targetActor.addStatusEffect(selectedEffect, 3f);
						// 你可以在這裡添加一些日誌或回饋，表明觸發了哪個效果
						// Debug.Log($"{pSelf.id} 的攻擊觸發了 {selectedEffect} 效果!");
					}
				}
			}
		return false;
		}
		public static bool Random_Effect2(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 隨機 攻擊用 新
			if (pSelf == null || pTarget == null)
			{
				return false; // 攻擊者或目標為空，不執行
			}

			Actor selfActor = pSelf.a;
			Actor targetActor = pTarget.a;

			if (selfActor == null || targetActor == null || !selfActor.isAlive() || !targetActor.isAlive())
			{
				return false; // 攻擊者或目標 Actor 無效或已死亡，不執行
			}

			// 定義自身冷卻狀態 ID 和持續時間
			string selfAtkStatus = "cdt_atk00";	 // 自身冷卻狀態 ID
			float selfAtkDuration = 5f;			 // 自身冷卻時間
			string selfDebuffStatus = "cdt_debuff01"; // 另一個可能影響發動的自身debuff狀態 ID
			// **核心修改：將所有能力執行邏輯都包裹在冷卻檢查內部**
			// 檢查自身是否沒有 cdt_atk00 且沒有 cdt_debuff01 狀態
			if (!selfActor.hasStatus(selfAtkStatus) && !selfActor.hasStatus(selfDebuffStatus))
			{
				// --- 能力發動區塊 ---

				// 1. 給予自身 cdt_atk00 冷卻狀態
				selfActor.addStatusEffect(selfAtkStatus, selfAtkDuration);
				// Debug.Log($"{selfActor.name} 獲得了 {selfAtkStatus}，持續 {selfAtkDuration} 秒");

				// 2. 定義一個包含所有可能效果的字串陣列
				string[] possibleEffects = { "slowness", "cough", "ash_fever", "frozen", "burning", "poisoned", "stunned", "sleeping", "spell_silence", "drowning", "confused" };

				// 3. 隨機選擇一個效果的索引
				int randomIndex = UnityEngine.Random.Range(0, possibleEffects.Length);

				// 4. 獲取隨機選擇的效果名稱
				string selectedEffect = possibleEffects[randomIndex];

				// 5. 檢查目標是否已經有選定的狀態效果 (可選，但保留以防止重複疊加)
				if (!targetActor.hasStatus(selectedEffect))
				{
					// 為目標施加隨機選擇的狀態效果，持續 5 秒
					targetActor.addStatusEffect(selectedEffect, 8f);
					// 你可以在這裡添加一些日誌或回饋，表明觸發了哪個效果
					// Debug.Log($"{pSelf.id} 的攻擊觸發了 {selectedEffect} 效果，施加給 {pTarget.id}！");
				}
				return true; // 能力成功發動並進入冷卻
			}
			else
			{
				// 如果自身處於冷卻狀態 (有 cdt_atk00 或 cdt_debuff01)，則不發動能力
				return false; 
			}
		}
		public static bool CursedAttack(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 詛咒 攻擊用 新
			if (pSelf == null || pTarget == null)
			{
				return false; // 攻擊者或目標為空，不執行
			}
			Actor selfActor = pSelf.a;
			Actor targetActor = pTarget.a;
			if (selfActor == null || targetActor == null || !selfActor.isAlive() || !targetActor.isAlive())
			{
				return false; // 攻擊者或目標 Actor 無效或已死亡，不執行
			}
			// 定義冷卻狀態 ID 和持續時間，以及目標狀態 ID 和持續時間
			string selfAtkStatus = "cdt_atk00";	 // 攻擊冷卻狀態 ID
			string selfDebuffStatus = "cdt_debuff01"; // 另一個可能影響發動的自身debuff狀態 ID
			float selfAtkDuration = 3f;			 // 自身冷卻時間
			string targetAddStatus = "cursed";	 // 目標將獲得的狀態 ID
			float targetAddDuration = 600f;		  // 目標狀態持續時間

			// **核心修改：將所有能力執行邏輯都包裹在冷卻檢查內部**
			// 檢查自身是否沒有 cdt_atk00 且沒有 cdt_debuff01 狀態
			if (!selfActor.hasStatus(selfAtkStatus) && !selfActor.hasStatus(selfDebuffStatus))
			{
				// --- 能力發動區塊 ---

				// 1. 給予自身 cdt_atk00 冷卻狀態
				// 這裡使用 addStatusEffect 也是可以的，因為它符合 "如果沒有就添加" 的邏輯
				selfActor.addStatusEffect(selfAtkStatus, selfAtkDuration);
				//Debug.Log($"{selfActor.name} 獲得了 {selfAtkStatus}，持續 {selfAtkDuration} 秒");

				// 2. 為目標施加 slowness 狀態效果
				targetActor.addStatusEffect(targetAddStatus, targetAddDuration);
				//Debug.Log($"{targetActor.name} 獲得了 {targetAddStatus}，持續 {targetAddDuration} 秒");

				return true; // 能力成功發動並進入冷卻
			}
			else
			{
				// 如果自身處於冷卻狀態 (有 cdt_atk00 或 cdt_debuff01)，則不發動能力
				return false; 
			}
		}
		public static bool DeathEffect0(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 死亡與消滅 新增區域設定
			// 1. 基本安全检查：确保施法者 pSelf 及其 Actor 组件存在且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a; // 获取施法者自身的 Actor 对象
			// 确保 pTarget (目标) 存在且有效
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor targetActor = pTarget.a; // 获取目标的 Actor 对象
			// 2. ====== 核心冷卻檢查：只有當施法者沒有冷卻狀態或妨礙狀態時才發動能力 ======
			string selfAtkStatus = "cdt_atk00";
			float selfAtkDuration = 10f; // 冷卻時間設定為 30 秒
			string selfDebuffStatus = "cdt_debuff01";

			if (!selfActor.hasStatus(selfAtkStatus) && !selfActor.hasStatus(selfDebuffStatus))
			{	// --- 能力發動區塊 ---
				// 2.1 施加自身冷卻狀態 (能力發動後立刻進入冷卻)
				selfActor.addStatusEffect(selfAtkStatus, selfAtkDuration); 
				// Debug.Log($"施法者 {selfActor.name} 發動死亡與消滅，並進入 {selfAtkDuration} 秒冷卻。");
				// 2.2 判斷是添加特質還是移除單位
				float addTraitChance = 0.9f; // 添加 death_mark 特質的總概率为 90%
				if (Randy.randomChance(addTraitChance)) // 90% 機率
				{
					// 嘗試為目標添加 death_mark 特質
					if (!targetActor.hasTrait("death_mark")) // 檢查目標是否尚未擁有該特質
					{
						targetActor.addTrait("death_mark");
						// Debug.Log($"成功為 {targetActor.name} 添加了 death_mark 特質。");
					}
				}
				else // 10% 機率
				{
					// 嘗試移除單位
					// 這裡不需要再次檢查 targetActor.isAlive()，因為我們在函數開頭已經檢查過了
					ActionLibrary.removeUnit(targetActor);
					// Debug.Log($"成功移除了單位：{targetActor.name}。");
				}
				
				return true; // 能力成功發動
			}
			else
			{
				// 施法者處於冷卻或妨礙中，不發動能力
				// Debug.Log($"施法者 {selfActor.name} 處於冷卻或妨礙狀態，無法發動死亡與消滅。");
				return false; 
			}
		}
			#endregion
			#region 單位添加強化狀態
	//強化狀態效果
		public static bool powerupEffect(BaseSimObject pSelf, WorldTile pTile = null)
		{// 巨人 cdt_buff00 新 powerup
			// 1. 基本安全檢查：確保 pSelf 及其 Actor 組件存在且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			// 檢查施法者自身是否有冷卻狀態
			if (selfActor.hasStatus("cdt_buff00") || selfActor.hasStatus("cdt_debuff02"))
			{
				return false; // 如果有冷卻狀態，則不執行任何效果
			}
			// 2. 對同勢力單位（包括自己）添加效果
			var allClosestUnits = Finder.getUnitsFromChunk(pTile, 1);
			if (allClosestUnits.Any())
			{
				foreach (var unit in allClosestUnits)
				{
					// 確保 unit 是有效的 Actor，存活，且與施法者屬於同一王國
					if (unit?.a != null && unit.a.isAlive() && unit.a.kingdom == selfActor.kingdom)
					{
						// 檢查同勢力單位是否沒有 "指定狀態" 且沒有 "cdt_debuff02"
						if (!unit.a.hasStatus("powerup"))
						{
							unit.a.addStatusEffect("powerup", 60f); // 对盟友施加指定狀態
						}
					}
				}
			}
			// 3. 在所有狀態添加完成後，對施法者自身添加冷卻狀態
			selfActor.addStatusEffect("cdt_buff00", 60f); // 施法者進入冷卻
			return false;
		}
		public static bool caffeinatedEffect(BaseSimObject pSelf, WorldTile pTile = null)
		{// 咖啡 cdt_buff00 新 caffeinated
			// 1. 基本安全檢查：確保 pSelf 及其 Actor 組件存在且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			// 檢查施法者自身是否有冷卻狀態
			if (selfActor.hasStatus("cdt_buff00") || selfActor.hasStatus("cdt_debuff02"))
			{
				return false; // 如果有冷卻狀態，則不執行任何效果
			}
			// 2. 對同勢力單位（包括自己）添加效果
			var allClosestUnits = Finder.getUnitsFromChunk(pTile, 1);
			if (allClosestUnits.Any())
			{
				foreach (var unit in allClosestUnits)
				{
					// 確保 unit 是有效的 Actor，存活，且與施法者屬於同一王國
					if (unit?.a != null && unit.a.isAlive() && unit.a.kingdom == selfActor.kingdom)
					{
						// 檢查同勢力單位是否沒有 "指定狀態" 且沒有 "cdt_debuff02"
						if (!unit.a.hasStatus("caffeinated"))
						{
							unit.a.addStatusEffect("caffeinated", 60f); // 对盟友施加指定狀態
						}
					}
				}
			}
			// 3. 在所有狀態添加完成後，對施法者自身添加冷卻狀態
			selfActor.addStatusEffect("cdt_buff00", 60f); // 施法者進入冷卻
			return false;
		}
		public static bool enchantedEffect(BaseSimObject pSelf, WorldTile pTile = null)
		{// 充能 cdt_buff00 新 enchanted
			// 1. 基本安全檢查：確保 pSelf 及其 Actor 組件存在且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			// 檢查施法者自身是否有冷卻狀態
			if (selfActor.hasStatus("cdt_buff00") || selfActor.hasStatus("cdt_debuff02"))
			{
				return false; // 如果有冷卻狀態，則不執行任何效果
			}
			// 2. 對同勢力單位（包括自己）添加效果
			var allClosestUnits = Finder.getUnitsFromChunk(pTile, 1);
			if (allClosestUnits.Any())
			{
				foreach (var unit in allClosestUnits)
				{
					// 確保 unit 是有效的 Actor，存活，且與施法者屬於同一王國
					if (unit?.a != null && unit.a.isAlive() && unit.a.kingdom == selfActor.kingdom)
					{
						// 檢查同勢力單位是否沒有 "指定狀態" 且沒有 "cdt_debuff02"
						if (!unit.a.hasStatus("enchanted"))
						{
							unit.a.addStatusEffect("enchanted", 60f); // 对盟友施加指定狀態
						}
					}
				}
			}
			// 3. 在所有狀態添加完成後，對施法者自身添加冷卻狀態
			selfActor.addStatusEffect("cdt_buff00", 60f); // 施法者進入冷卻
			return false;
		}
		public static bool rageEffect(BaseSimObject pSelf, WorldTile pTile = null)
		{// 狂暴 cdt_buff00 新 rage
			// 1. 基本安全檢查：確保 pSelf 及其 Actor 組件存在且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			// 檢查施法者自身是否有冷卻狀態
			if (selfActor.hasStatus("cdt_buff00") || selfActor.hasStatus("cdt_debuff02"))
			{
				return false; // 如果有冷卻狀態，則不執行任何效果
			}
			// 2. 對同勢力單位（包括自己）添加效果
			var allClosestUnits = Finder.getUnitsFromChunk(pTile, 1);
			if (allClosestUnits.Any())
			{
				foreach (var unit in allClosestUnits)
				{
					// 確保 unit 是有效的 Actor，存活，且與施法者屬於同一王國
					if (unit?.a != null && unit.a.isAlive() && unit.a.kingdom == selfActor.kingdom)
					{
						// 檢查同勢力單位是否沒有 "指定狀態" 且沒有 "cdt_debuff02"
						if (!unit.a.hasStatus("rage"))
						{
							unit.a.addStatusEffect("rage", 60f); // 对盟友施加指定狀態
						}
					}
				}
			}
			// 3. 在所有狀態添加完成後，對施法者自身添加冷卻狀態
			selfActor.addStatusEffect("cdt_buff00", 60f); // 施法者進入冷卻
			return false;
		}
		public static bool shieldEffect(BaseSimObject pSelf, WorldTile pTile = null)
		{// 護盾 cdt_buff00 新 shield
			// 1. 基本安全檢查：確保 pSelf 及其 Actor 組件存在且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			// 檢查施法者自身是否有冷卻狀態
			if (selfActor.hasStatus("cdt_buff00") || selfActor.hasStatus("cdt_debuff02"))
			{
				return false; // 如果有冷卻狀態，則不執行任何效果
			}
			// 2. 對同勢力單位（包括自己）添加效果
			var allClosestUnits = Finder.getUnitsFromChunk(pTile, 1);
			if (allClosestUnits.Any())
			{
				foreach (var unit in allClosestUnits)
				{
					// 確保 unit 是有效的 Actor，存活，且與施法者屬於同一王國
					if (unit?.a != null && unit.a.isAlive() && unit.a.kingdom == selfActor.kingdom)
					{
						// 檢查同勢力單位是否沒有 "指定狀態" 且沒有 "cdt_debuff02"
						if (!unit.a.hasStatus("shield"))
						{
							unit.a.addStatusEffect("shield", 60f); // 对盟友施加指定狀態
						}
					}
				}
			}
			// 3. 在所有狀態添加完成後，對施法者自身添加冷卻狀態
			selfActor.addStatusEffect("cdt_buff00", 60f); // 施法者進入冷卻
			return false;
		}
		public static bool InvincibleEffect(BaseSimObject pSelf, WorldTile pTile = null)
		{// 無敵 cdt_buff00 新 invincible
			// 1. 基本安全檢查：確保 pSelf 及其 Actor 組件存在且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			// 檢查施法者自身是否有冷卻狀態
			if (selfActor.hasStatus("cdt_buff00") || selfActor.hasStatus("cdt_debuff02"))
			{
				return false; // 如果有冷卻狀態，則不執行任何效果
			}
			// 2. 對同勢力單位（包括自己）添加效果
			var allClosestUnits = Finder.getUnitsFromChunk(pTile, 0);
			if (allClosestUnits.Any())
			{
				foreach (var unit in allClosestUnits)
				{
					// 確保 unit 是有效的 Actor，存活，且與施法者屬於同一王國
					if (unit?.a != null && unit.a.isAlive() && unit.a.kingdom == selfActor.kingdom)
					{
						// 檢查同勢力單位是否沒有 "指定狀態" 且沒有 "cdt_debuff02"
						if (!unit.a.hasStatus("invincible"))
						{
							unit.a.addStatusEffect("invincible", 60f); // 对盟友施加指定狀態
						}
					}
				}
			}
			// 3. 在所有狀態添加完成後，對施法者自身添加冷卻狀態
			selfActor.addStatusEffect("cdt_buff00", 55f); // 施法者進入冷卻
			return false;
		}
		public static bool InspiredEffect(BaseSimObject pSelf, WorldTile pTile = null)
		{// 動力 cdt_buff00 新 inspired
			// 1. 基本安全檢查：確保 pSelf 及其 Actor 組件存在且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			// 檢查施法者自身是否有冷卻狀態
			if (selfActor.hasStatus("cdt_buff00") || selfActor.hasStatus("cdt_debuff02"))
			{
				return false; // 如果有冷卻狀態，則不執行任何效果
			}
			// 2. 對同勢力單位（包括自己）添加效果
			var allClosestUnits = Finder.getUnitsFromChunk(pTile, 0);
			if (allClosestUnits.Any())
			{
				foreach (var unit in allClosestUnits)
				{
					// 確保 unit 是有效的 Actor，存活，且與施法者屬於同一王國
					if (unit?.a != null && unit.a.isAlive() && unit.a.kingdom == selfActor.kingdom)
					{
						// 檢查同勢力單位是否沒有 "指定狀態" 且沒有 "cdt_debuff02"
						if (!unit.a.hasStatus("inspired"))
						{
							unit.a.addStatusEffect("inspired", 60f); // 对盟友施加指定狀態
						}
					}
				}
			}
			// 3. 在所有狀態添加完成後，對施法者自身添加冷卻狀態
			selfActor.addStatusEffect("cdt_buff00", 60f); // 施法者進入冷卻
			return false;
		}
		public static bool spellboostEffect(BaseSimObject pSelf, WorldTile pTile = null)
		{// 法升 cdt_buff00 新 spell_boost
			// 1. 基本安全檢查：確保 pSelf 及其 Actor 組件存在且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			// 檢查施法者自身是否有冷卻狀態
			if (selfActor.hasStatus("cdt_buff00") || selfActor.hasStatus("cdt_debuff02"))
			{
				return false; // 如果有冷卻狀態，則不執行任何效果
			}
			// 2. 對同勢力單位（包括自己）添加效果
			var allClosestUnits = Finder.getUnitsFromChunk(pTile, 0);
			if (allClosestUnits.Any())
			{
				foreach (var unit in allClosestUnits)
				{
					// 確保 unit 是有效的 Actor，存活，且與施法者屬於同一王國
					if (unit?.a != null && unit.a.isAlive() && unit.a.kingdom == selfActor.kingdom)
					{
						// 檢查同勢力單位是否沒有 "指定狀態" 且沒有 "cdt_debuff02"
						if (!unit.a.hasStatus("spell_boost"))
						{
							unit.a.addStatusEffect("spell_boost", 60f); // 对盟友施加指定狀態
						}
					}
				}
			}
			// 3. 在所有狀態添加完成後，對施法者自身添加冷卻狀態
			selfActor.addStatusEffect("cdt_buff00", 60f); // 施法者進入冷卻
			return false;
		}
		public static bool motivatedEffect(BaseSimObject pSelf, WorldTile pTile = null)
		{// 熱情 cdt_buff00 新 motivated
			// 1. 基本安全檢查：確保 pSelf 及其 Actor 組件存在且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			// 檢查施法者自身是否有冷卻狀態
			if (selfActor.hasStatus("cdt_buff00") || selfActor.hasStatus("cdt_debuff02"))
			{
				return false; // 如果有冷卻狀態，則不執行任何效果
			}
			// 2. 對同勢力單位（包括自己）添加效果
			var allClosestUnits = Finder.getUnitsFromChunk(pTile, 0);
			if (allClosestUnits.Any())
			{
				foreach (var unit in allClosestUnits)
				{
					// 確保 unit 是有效的 Actor，存活，且與施法者屬於同一王國
					if (unit?.a != null && unit.a.isAlive() && unit.a.kingdom == selfActor.kingdom)
					{
						// 檢查同勢力單位是否沒有 "指定狀態" 且沒有 "cdt_debuff02"
						if (!unit.a.hasStatus("motivated"))
						{
							unit.a.addStatusEffect("motivated", 60f); // 对盟友施加指定狀態
						}
					}
				}
			}
			// 3. 在所有狀態添加完成後，對施法者自身添加冷卻狀態
			selfActor.addStatusEffect("cdt_buff00", 60f); // 施法者進入冷卻
			return false;
		}
		public static bool SuperEffect1(BaseSimObject pSelf, WorldTile pTile = null)
		{// ALL FOR ONE cdt_buff00
			// 1. 基本安全检查：确保 pSelf 及其 Actor 组件存在且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) 
				return false;
			Actor selfActor = pSelf.a;
			// 2. 上半部：特质持有者自身添加条件 (保留您的状态检查)
			// 确保自身没有 "cdt_buff00" (冷却状态) 且没有 "cdt_debuff02"
			if (!selfActor.hasStatus("cdt_buff00") && !selfActor.hasStatus("cdt_debuff02"))
			{
				selfActor.addStatusEffect("cdt_buff00", 55f);// 冷却状态
				selfActor.addStatusEffect("powerup", 60f);// 指定狀態
				selfActor.addStatusEffect("caffeinated", 60f);// 指定狀態
				selfActor.addStatusEffect("enchanted", 60f);// 指定狀態
				selfActor.addStatusEffect("rage", 60f);// 指定狀態
				selfActor.addStatusEffect("shield", 60f);// 指定狀態
				selfActor.addStatusEffect("spell_boost", 60f);// 指定狀態
				selfActor.addStatusEffect("motivated", 60f);// 指定狀態
				selfActor.addStatusEffect("inspired", 60f);// 指定狀態
				selfActor.addStatusEffect("antibody", 60f);// 指定狀態
			}
			return false; // 特殊效果通常返回 false，表示不阻止其他效果链
		}
		public static bool SuperEffect2(BaseSimObject pSelf, WorldTile pTile = null)
		{// ONE FOR ALL cdt_buff00
			// 1. 基本安全检查：确保 pSelf 及其 Actor 组件存在且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			// 2. 只有在施法者沒有冷卻或封禁狀態時，才執行後續邏輯
			if (selfActor.hasStatus("cdt_buff00") || selfActor.hasStatus("cdt_debuff02"))
			{
				return false; // 如果有冷卻狀態，則不執行任何效果
			}
			// 3. 對施法者自身添加狀態
			selfActor.addStatusEffect("cdt_buff00", 60f);
			selfActor.addStatusEffect("invincible", 60f);
			selfActor.addStatusEffect("antibody", 60f);
			// 4. 對同勢力單位添加狀態
			var allClosestUnits = Finder.getUnitsFromChunk(pTile, 1);
			if (allClosestUnits.Any())
			{
				foreach (var unit in allClosestUnits)
				{
					// 確保 unit 是有效的 Actor、存活、是同勢力單位且不是施法者自己
					if (unit?.a != null && unit.a.isAlive() && unit.a != selfActor && unit.a.kingdom == selfActor.kingdom)
					{
						// 對同伴直接添加所有增益狀態
						unit.a.addStatusEffect("powerup", 60f);
						unit.a.addStatusEffect("caffeinated", 60f);
						unit.a.addStatusEffect("enchanted", 60f);
						unit.a.addStatusEffect("rage", 60f);
						unit.a.addStatusEffect("shield", 60f);
						unit.a.addStatusEffect("spell_boost", 60f);
						unit.a.addStatusEffect("motivated", 60f);
						unit.a.addStatusEffect("inspired", 60f);
						unit.a.addStatusEffect("antibody", 60f);
					}
				}
			}
			return false; // 返回 false，表示不阻止其他效果鏈
		}
			#endregion
			#region 恢復 異常消除
	//恢復效果
		public static bool removeStatus(BaseSimObject pSelf, WorldTile pTile = null)
		{// 抗體 CDT標記 cdt_cure00
			// 1. 基本安全检查：确保 pSelf 及其 Actor 组件存在且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) 
				return false;
			Actor selfActor = pSelf.a;
			// 2. 上半部：特质持有者自身添加条件 (保留您的状态检查)
			// 确保自身没有 "cdt_cure00" (冷却状态) 且没有 "cdt_debuff02"
			if (!selfActor.hasStatus("cdt_cure00") && !selfActor.hasStatus("cdt_debuff03"))
			{
				selfActor.addStatusEffect("cdt_cure00", 60f);// 冷却状态
				selfActor.addStatusEffect("antibody", 50f);// 指定狀態
			}
			// 3. 下半部：对同势力单位的添加条件 (使用 0.50 API 并保留状态检查)
			var allClosestUnits = Finder.getUnitsFromChunk(pTile, 1); // 范围 15 保持一致
			if (allClosestUnits.Any()) // 使用 LINQ 的 Any() 检查是否有单位
			{
					foreach (var unit in allClosestUnits)
				{
					// 确保 unit 是有效的 Actor，存活，且不是 pSelf 本身
					if (unit.a != null && unit.a.isAlive() && unit.a != selfActor) 
					{
						// 检查是否属于同一王国
						if (unit.a.kingdom == selfActor.kingdom)
						{
							// 检查同势力单位是否没有 "指定狀態" 且没有 "cdt_debuff02"
							if (!unit.a.hasStatus("antibody") && !unit.a.hasStatus("cdt_debuff03"))
							{
								unit.a.addStatusEffect("antibody", 60f); // 对盟友施加指定狀態
							}
						}
					}
				}
			}	
			return false; // 特殊效果通常返回 false，表示不阻止其他效果链
		}
		public static bool removeTrait(BaseSimObject pSelf, WorldTile pTile = null)
		{// 淨化抗體 癒合 cdt_cure01
			// 1. 基本安全检查：确保 pSelf 及其 Actor 组件存在且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			bool effectTriggered = false; // 用於標記是否成功移除了任何特質
			// 2. ====== 檢查是否處於冷卻中，如果是，直接返回 ======
			if (selfActor.hasStatus("cdt_cure01") || selfActor.hasStatus("cdt_debuff03"))
			{
				// Debug.Log($"{selfActor.name} 處於冷卻中，無法再次使用淨化抗體。");
				return false; // 處於冷卻中，效果未執行
			}
			// 3. ====== 檢查並移除施法者自身的特定特質 ======
			string[] negativeTraitsToRemove = { "tumor_infection", "infected", "mush_spores", "plague", "crippled", "eyepatch", "skin_burns", "death_mark" };
			foreach (string trait in negativeTraitsToRemove)
			{
				if (selfActor.hasTrait(trait))
				{
					selfActor.removeTrait(trait);
					effectTriggered = true; // 自身特質被移除，標記為已觸發效果
					// Debug.Log($"施法者 {selfActor.name} 自身移除了特質：{trait}");
				}
			}
			// 4. 對同勢力單位的移除特質效果 (已移除随机性，必然触发)
			var allClosestUnits = Finder.getUnitsFromChunk(pTile, 1); // 範圍 15 保持一致
			if (allClosestUnits.Any()) // 使用 LINQ 的 Any() 檢查是否有單位
			{
				foreach (var unit in allClosestUnits)
				{
					// 確保 unit 是有效的 Actor，存活，且不是 pSelf 本身
					if (unit.a != null && unit.a.isAlive() && unit.a != selfActor)
					{
						// 檢查是否屬於同一王國
						if (unit.a.kingdom == selfActor.kingdom)
						{
							// 嘗試移除負面特質
							foreach (string traitToRemove in negativeTraitsToRemove)
							{
								if (unit.a.hasTrait(traitToRemove))
								{
									unit.a.removeTrait(traitToRemove);
									effectTriggered = true; // 隊友特質被移除，標記為已觸發效果
									// Debug.Log($"{selfActor.name} 移除了 {unit.a.name} 的 {traitToRemove} 特質");
								}
							}
						}
					}
				}
			}
			// 5. ====== 只有在確實移除了任何特質後才添加冷卻 ======
			if (effectTriggered)
			{
				selfActor.addStatusEffect("cdt_cure01", 5f); // 冷卻時間設定為 5 秒
				return true; // 成功執行了移除效果並進入冷卻
			}
			else
			{
				// 如果沒有任何特質被移除，則不進入冷卻
				// Debug.Log($"{selfActor.name} 沒有找到可移除的特質，未進入冷卻。");
				return false; // 沒有移除任何特質，效果未執行
			}
		}
		public static bool MadnessCure(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 狂化移除
			// 1. 基本安全检查：确保目标存在、是活着的Actor
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) 
				return false;
			// 确保施法者也存在且是活着的Actor (如果 pSelf 的状态很重要)
			// if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) return false; 
			// Actor selfActor = pSelf.a; // 如果需要用到施法者actor，可以解注释这行
			// 设定移除 madness 特质的概率
			float chance = 0.90f;
			// 2. 使用 Randy.randomChance 替换 Toolbox.randomChance
			if (Randy.randomChance(chance)) 
			{
				// 获取目标的 Actor 对象
				Actor targetActor = pTarget.a;
				// 检查目标是否存在 madness 特质，如果存在则移除
				// targetActor != null 的检查在函数开头 pTarget.a == null 已经做了，这里可以简化
				if (targetActor.hasTrait("madness"))
				{
					// 移除目标的 madness 特质
					targetActor.removeTrait("madness");
					// Debug.Log($"{pSelf?.a?.name ?? "Unknown"} 移除了 {targetActor.name} 的 madness 特质。"); // 使用 targetActor.name
					return true; // 成功移除特质
				}
				else
				{
					// 目标没有 madness 特质
					// Debug.Log($"{targetActor.name} 没有 madness 特质。");
					return false; // 没有移除任何特质
				}
			}
			// 如果随机概率未通过，则返回 false
			return false; 
		}
		public static bool Divine_Arts1(BaseSimObject pSelf, WorldTile pTile = null)
		{// 淨化之滴 cdt_cure02
			// 1. 基本安全檢查：確保 pSelf 及其 Actor 組件存在且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			// 2. 只有在施法者沒有冷卻或封禁狀態時，才執行後續邏輯
			if (selfActor.hasStatus("cdt_cure02") || selfActor.hasStatus("cdt_debuff03"))
			{
				return false; // 處於冷卻中，效果未執行
			}
			bool effectTriggered = false;
			string[] negativeTraitsToCure = { "tumor_infection", "infected", "mush_spores", "plague" };
			// 3. 對自身特定特質應用 castCure
			foreach (string trait in negativeTraitsToCure)
			{
				if (selfActor.hasTrait(trait))
				{
					ActionLibrary.castCure(selfActor, selfActor, null);
					effectTriggered = true; // 自身獲得治療，標記效果已觸發
					break; // 只要有一個特質被治療就夠了
				}
			}
			// 4. 搜尋並治療範圍內的友軍
			var allClosestUnits = Finder.getUnitsFromChunk(pTile, 1);
			if (allClosestUnits.Any())
			{
				foreach (var unit in allClosestUnits)
				{
					// 確保 unit 是有效的 Actor，存活，且是同勢力單位
					if (unit?.a != null && unit.a.isAlive() && unit.a.kingdom == selfActor.kingdom)
					{
						// 檢查這些友軍是否帶有需要治療的負面特質
						foreach (string traitToCure in negativeTraitsToCure)
						{
							if (unit.a.hasTrait(traitToCure))
							{
								// 對帶有指定特質的友軍應用 castCure
								ActionLibrary.castCure(selfActor, unit.a, null);
								effectTriggered = true; // 友軍獲得治療，標記效果已觸發
							}
						}
					}
				}
			}
			// 5. 如果效果有實際觸發，則添加冷卻狀態
			if (effectTriggered)
			{
				selfActor.addStatusEffect("cdt_cure02", 30f); // 冷卻時間設定為 30 秒
				return true; // 成功執行了效果
			}
			return false; // 沒有單位需要治療，效果未執行
		}
		public static bool Divine_Arts2(BaseSimObject pSelf, WorldTile pTile = null)
		{// 生命之血 cdt_cure03
			// 1. 基本安全检查：确保 pSelf 及其 Actor 组件存在且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive())
				return false;

			Actor selfActor = pSelf.a;
			bool effectTriggered = false; // 用於標記是否成功應用了 castBloodRain

			// 2. ====== 檢查是否處於冷卻中，如果是，直接返回 ======
			if (selfActor.hasStatus("cdt_cure03") || selfActor.hasStatus("cdt_debuff03"))
			{
				// Debug.Log($"{selfActor.name} 處於冷卻中，無法再次使用生命之血。");
				return false; // 處於冷卻中，效果未執行
			}

			// 定義觸發閾值：生命值低於此百分比時觸發效果 (例如 50%)
			float healthThresholdPercentage = 0.60f; //

			// 3. ====== 檢查施法者自身生命值百分比並應用 castBloodRain ======
			float selfHealthPercentage = (float)selfActor.data.health / selfActor.getMaxHealth();

			if (selfHealthPercentage < healthThresholdPercentage) // 使用百分比觸發條件
			{
				ActionLibrary.castBloodRain(selfActor, selfActor, null);
				ActionLibrary.castBloodRain(selfActor, selfActor, null);
				ActionLibrary.castBloodRain(selfActor, selfActor, null);
				ActionLibrary.castBloodRain(selfActor, selfActor, null);
				ActionLibrary.castBloodRain(selfActor, selfActor, null);
				effectTriggered = true; // 自身觸發，標記為已觸發效果
				// Debug.Log($"施法者 {selfActor.name} 因生命值過低({selfHealthPercentage:P0})而獲得了生命之血效果。");
			}
			// 4. 對範圍內同勢力單位的治療效果
			// 確保 pTile 不為 null
			//if (pTile == null)
			//{
			//	pTile = selfActor.currentTile;
			//	if (pTile == null) return false;
			//}
			var allClosestUnits = Finder.getUnitsFromChunk(pTile, 1); // 範圍 15 保持一致
			if (allClosestUnits.Any())
			{
				foreach (var unit in allClosestUnits)
				{
					// 確保 unit 是有效的 Actor，存活，且不是 pSelf 本身
					if (unit.a != null && unit.a.isAlive() && unit.a != selfActor)
					{
						// 檢查是否屬於同一王國
						if (unit.a.kingdom == selfActor.kingdom)
						{
							// 計算友軍的生命值百分比
							float unitHealthPercentage = (float)unit.a.data.health / unit.a.getMaxHealth();

							// 新的條件：檢查這些友軍的生命值百分比是否小於閾值
							if (unitHealthPercentage < healthThresholdPercentage) // 使用百分比觸發條件
							{
								// 對生命值低的友軍應用 castBloodRain
								ActionLibrary.castBloodRain(selfActor, unit.a, null);
								ActionLibrary.castBloodRain(selfActor, unit.a, null);
								ActionLibrary.castBloodRain(selfActor, unit.a, null);
								ActionLibrary.castBloodRain(selfActor, unit.a, null);
								ActionLibrary.castBloodRain(selfActor, unit.a, null);
								effectTriggered = true; // 友軍觸發，標記為已觸發效果
								// Debug.Log($"{selfActor.name} 對 {unit.a.name} 應用了生命之血效果，因為其生命值過低({unitHealthPercentage:P0})。");
							}
						}
					}
				}
			}
			// 5. ====== 只有在確實應用了 castBloodRain 後才添加冷卻 ======
			if (effectTriggered)
			{
				selfActor.addStatusEffect("cdt_cure03", 5f); // 冷卻時間設定為 5 秒
				return true; // 成功執行了效果並進入冷卻
			}
			else
			{
				return false; // 沒有應用效果，效果未執行
			}
		}
		public static bool Health_recovery(BaseSimObject pTarget, WorldTile pTile = null)
		{// 生命值回復
			if (pTarget is Actor actor) // 檢查 pTarget 是否為 Actor 類型，並將其轉換
			{
				// 設定觸發恢復的閾值 (例如：血量低於 90% 才恢復)
				float activationThreshold = 0.90f;

				// 設定每次恢復的百分比 (例如：恢復最大生命值的 10%)
				float recoveryPercentage = 0.05f;

				// 計算當前生命值百分比
				float currentHealthPercentage = (float)actor.data.health / actor.getMaxHealth();

				// 檢查目標的當前生命值百分比是否低於觸發閾值
				if (currentHealthPercentage < activationThreshold)
				{
					// 計算要恢復的實際點數
					// 確保結果是整數，因為生命值通常是整數
					int healthToRestore = Mathf.RoundToInt(actor.getMaxHealth() * recoveryPercentage);

					// 確保至少恢復 1 點血，避免因計算結果為 0 而沒有效果
					if (healthToRestore < 1)
					{
						healthToRestore = 1;
					}

					// 使用 actor.restoreHealth() 方法來恢復生命值
					actor.restoreHealth(healthToRestore); 

					// 如果您還想添加粒子特效：
					actor.spawnParticle(Toolbox.color_heal); 

					// Debug.Log($"{actor.name} 生命值 ({currentHealthPercentage*100:F0}%) 過低，恢復了 {healthToRestore} 點生命值。"); // 可選的日誌輸出
					return true; // 成功執行了生命值恢復
				}
				else
				{
					// 目標生命值高於或等於激活閾值，無需恢復
					// Debug.Log($"{actor.name} 生命值 ({currentHealthPercentage*100:F0}%) 充足，無需恢復。"); // 可選的日誌輸出
					return false; // 沒有恢復生命值
				}
			}
			return false; // pTarget 不是 Actor 類型，或為 null
		}
		public static bool Mana_recovery(BaseSimObject pTarget, WorldTile pTile = null)
		{// 魔力值回復 
			if (pTarget is Actor actor) // 檢查 pTarget 是否為 Actor 類型，並將其轉換
			{
				// 設定觸發恢復的閾值 (例如：魔力低於 90% 才恢復)
				float activationThreshold = 0.90f;

				// 設定每次恢復的百分比 (例如：恢復最大魔力值的 10%)
				float recoveryPercentage = 0.05f;

				// 計算當前魔力值百分比
				float currentManaPercentage = (float)actor.data.mana / actor.getMaxMana();

				// 檢查目標的當前魔力值百分比是否低於觸發閾值
				if (currentManaPercentage < activationThreshold)
				{
					// 計算要恢復的實際點數
					int manaToRestore = Mathf.RoundToInt(actor.getMaxMana() * recoveryPercentage);

					// 確保至少恢復 1 點魔力，避免因計算結果為 0 而沒有效果
					if (manaToRestore < 1)
					{
						manaToRestore = 1;
					}

					// *** 關鍵修正：直接修改 actor.data.mana，並確保不超過最大魔力值 ***
					actor.data.mana += manaToRestore;
					// 確保魔力值不會超過最大值
					if (actor.data.mana > actor.getMaxMana())
					{
						actor.data.mana = actor.getMaxMana();
					}
					
					// 如果您還想添加粒子特效（通常魔力恢復的特效顏色可能不同於生命恢復）：
					// 例如，如果 WorldBox 有 Toolbox.color_mana 或類似的顏色
					actor.spawnParticle(Toolbox.color_heal); // 您可以使用 Toolbox.color_heal 或查找魔力相關顏色

					// Debug.Log($"{actor.name} 魔力值 ({currentManaPercentage*100:F0}%) 過低，恢復了 {manaToRestore} 點魔力值。"); // 可選的日誌輸出
					return true; // 成功執行了魔力值恢復
				}
				else
				{
					// 目標魔力值高於或等於激活閾值，無需恢復
					// Debug.Log($"{actor.name} 魔力值 ({currentManaPercentage*100:F0}%) 充足，無需恢復。"); // 可選的日誌輸出
					return false; // 沒有恢復魔力值
				}
			}
			return false; // pTarget 不是 Actor 類型，或為 null
		}
		public static bool Stamina_recovery(BaseSimObject pTarget, WorldTile pTile = null)
		{// 耐力值回復 
			if (!(pTarget is Actor actor) || !actor.isAlive()) // 確保 pTarget 是有效的 Actor 且存活
			{
				return false;
			}

			// 設定觸發恢復的閾值 (例如：耐力低於 90% 才恢復)
			float activationThreshold = 0.90f;

			// 設定每次恢復的百分比 (例如：恢復最大耐力值的 10%)
			float recoveryPercentage = 0.05f;

			// 計算當前耐力值百分比
			float currentStaminaPercentage = (float)actor.data.stamina / actor.getMaxStamina();

			// 檢查目標的當前耐力值百分比是否低於觸發閾值
			if (currentStaminaPercentage < activationThreshold)
			{
				// 計算要恢復的實際點數
				int staminaToRestore = Mathf.RoundToInt(actor.getMaxStamina() * recoveryPercentage);

				// 確保至少恢復 1 點耐力，避免因計算結果為 0 而沒有效果
				if (staminaToRestore < 1)
				{
					staminaToRestore = 1;
				}

				// 直接修改 actor.data.stamina
				actor.data.stamina += staminaToRestore;
				
				// 確保耐力值不會超過最大值
				if (actor.data.stamina > actor.getMaxStamina())
				{
					actor.data.stamina = actor.getMaxStamina();
				}
				
				// 添加粒子特效 (可以根據需要替換為耐力相關的顏色，如果遊戲有提供)
				actor.spawnParticle(Toolbox.color_heal); // 使用 Toolbox.color_heal 或查找更合適的顏色

				// Debug.Log($"{actor.name} 耐力值 ({currentStaminaPercentage*100:F0}%) 過低，恢復了 {staminaToRestore} 點耐力值。"); // 可選的日誌輸出
				return true; // 成功執行了耐力值恢復
			}
			else
			{
				// 目標耐力值高於或等於激活閾值，無需恢復
				// Debug.Log($"{actor.name} 耐力值 ({currentStaminaPercentage*100:F0}%) 充足，無需恢復。"); // 可選的日誌輸出
				return false; // 沒有恢復耐力值
			}
		}
		public static bool Stamina_recoveryXX(BaseSimObject pTarget, WorldTile pTile = null)
		{// 耐力值回復
			if (pTarget is Actor actor)
			{
				actor.data.stamina += (int)100f;
				return true;
			}
			return false;
		}
		private static bool ProcessBlessingAndCurse(Actor pActor, string cursedStatusID, string blessedTraitID, string debugActorName)
		{// 私有輔助函數，用於處理單個 Actor 的祝福/詛咒邏輯
			bool changed = false;

			// 1. 處理詛咒狀態 (現在是狀態效果)
			if (pActor.hasStatus(cursedStatusID)) // 檢查是否有詛咒狀態
			{
				pActor.finishStatusEffect(cursedStatusID); // 結束詛咒狀態
				changed = true;
				// Debug.Log($"移除了 {debugActorName} 的 {cursedStatusID} 狀態效果.");
			}

			// 2. 處理祝福特質 (仍是特質)
			if (!pActor.hasTrait(blessedTraitID)) // 如果沒有祝福特質
			{
				pActor.addTrait(blessedTraitID); // 添加祝福特質
				changed = true;
				// Debug.Log($"為 {debugActorName} 添加了 {blessedTraitID} 特質.");
			}
			return changed;
		}
		public static bool Divine_Arts5(BaseSimObject pSelf, WorldTile pTile)
		{// 祝聖術 賦予祝福/移除詛咒
			if (pSelf == null || pSelf.a == null) // pTile 可能為 null，我們將在內部處理
			{
				return false; // 無效的施法者
			}
			Actor selfActor = pSelf.a;
			// ** 臨時安全措施：如果 pTile 為 null，則直接返回，避免崩潰 **
			// 這是您之前討論過的部分，在沒有可靠獲取 WorldTile 方法前採用
			if (pTile == null)
			{
				// Debug.Log("Divine_Arts5 Warning: pTile is null. Skipping effect to prevent crash.");
				return false;
			}
			int range = 10; // 設定影響範圍
			string cursedStatusID = "cursed"; // 詛咒的狀態ID
			string blessedTraitID = "blessed"; // 祝福的特質ID
			bool effectAppliedToAnyone = false; // 追蹤是否成功對至少一個單位應用了效果
			// 1. 處理施法者自身
			if (ProcessBlessingAndCurse(selfActor, cursedStatusID, blessedTraitID, selfActor.name))
			{
				effectAppliedToAnyone = true;
			}
			// 2. 獲取範圍內的 Actor
			// 替換 World.world.getObjectsInChunks 為 Finder.getUnitsFromChunk
			var allClosestUnits = Finder.getUnitsFromChunk(pTile, range);
			// 遍歷範圍內的 Actor
			if (allClosestUnits.Any())
			{
				foreach (var unit in allClosestUnits)
				{
					Actor targetActor = unit.a; // 從 BaseSimObject 獲取 Actor 組件

					// 檢查目標是否是有效的 Actor，存活，且不是施法者本身，且屬於同一王國
					if (targetActor != null && targetActor.isAlive() && targetActor != selfActor && targetActor.kingdom == selfActor.kingdom)
					{
						if (ProcessBlessingAndCurse(targetActor, cursedStatusID, blessedTraitID, targetActor.name))
						{
							effectAppliedToAnyone = true; // 只要有一個目標被應用效果，就設為 true
						}
					}
				}
			}
			// 由於 selfActor 已經在前面處理過，effectAppliedToAnyone 會根據自身和友軍的處理結果最終確定
			return effectAppliedToAnyone; // 返回是否成功對至少一個目標應用了效果
		}
		public static bool Annunciation(BaseSimObject pSelf, WorldTile pTile = null)
		{// 受胎術 針對亞種特質賦予狀態
			// 1. 基本安全檢查：確保 pSelf 及其 Actor 組件存在且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			// 定義狀態 ID 和持續時間
			string selfCooldownAndMarkStatus = "pregnancy"; 
			string targetMarkStatus = "pregnancy2";	  
			float cooldownDuration = 1200f;		 
			float targetMarkDuration = 1200f;		   
			float antibodyDuration = 6f;				
			// 生育狀態 ID (根據亞種特徵動態賦予)
			string pregnantStatus = "pregnant";
			string buddingStatus = "budding";
			string takingRootsStatus = "taking_roots";
			string soulHarvestedStatus = "soul_harvested";
			string justAteStatus = "just_ate";
			string pregnantParthenogenesisStatus = "pregnant_parthenogenesis"; // <-- 新增
			// 通用生育狀態的持續時間
			float effectDuration = 5f;				  
			// 魂魄收穫狀態的特殊持續時間
			float soulHarvestedDuration = 666f;		 
			float soulHarvestedAntibodyDuration = 30f;  
			// 新增：just_ate 狀態的持續時間
			float justAteDuration = 600f;
			// 營養值與生命值百分比閾值
			float nutritionThreshold = 90f;
			float healthPercentageThreshold = 0.8f;
			// 2. 在發動前檢查自身是否持有 "pregnancy" 冷卻/標記狀態
			if (selfActor.hasStatus(selfCooldownAndMarkStatus) || selfActor.hasStatus(targetMarkStatus))
			{
				return false;
			}
			// 3. 檢查施法者的物種是否具有 "stomach" 特質並執行相應的消耗檢查
			bool selfHasStomach = selfActor.hasSubspeciesTrait("stomach");
			if (selfHasStomach)
			{
				if (selfActor.data.nutrition < nutritionThreshold)
				{
					return false; // 營養不足，無法發動
				}
			}
			else
			{
				float currentHealthPercentage = (float)selfActor.data.health / selfActor.getMaxHealth();
				if (currentHealthPercentage < healthPercentageThreshold)
				{
					return false; // 生命值不足，無法發動
				}
			}
			// 4. 獲取所有目標單位 (包含自身及周圍同王國單位)
			var allUnitsInProximity = Finder.getUnitsFromChunk(pTile, 1);
			// 5. 對所有符合條件的目標單位施加狀態
			foreach (var unit in allUnitsInProximity)
			{
				Actor currentTargetActor = unit.a;
				// 確保 unit 是有效的 Actor，存活，且是同王國單位
				if (currentTargetActor == null || !currentTargetActor.isAlive() || currentTargetActor.kingdom != selfActor.kingdom)
				{
					continue;
				}
				// 檢查目標是否已經有任何一種受胎術相關的標記
				if (currentTargetActor.hasStatus(selfCooldownAndMarkStatus) || currentTargetActor.hasStatus(targetMarkStatus))
				{
					continue;
				}
				// 6. 檢查並扣除目標單位發動所需代價
				bool targetHasStomach = currentTargetActor.hasSubspeciesTrait("stomach");
				if (targetHasStomach)
				{
					if (currentTargetActor.data.nutrition < nutritionThreshold)
					{
						continue; // 目標營養不足，跳過
					}
					// 修正: 將浮點數轉為整數
					currentTargetActor.data.nutrition -= (int)nutritionThreshold; // 扣除營養值
				}
				else
				{
					float currentHealthPercentage = (float)currentTargetActor.data.health / currentTargetActor.getMaxHealth();
					if (currentHealthPercentage < healthPercentageThreshold)
					{
						continue; // 目標生命值不足，跳過
					}
					// 扣除生命值，這裡使用 20% 作為代價
					int healthToDeduct = Mathf.RoundToInt(currentTargetActor.getMaxHealth() * 0.2f);
					if (healthToDeduct < 1)
					{
						healthToDeduct = 1;
					}
					// 修正: 將 addDamage 替換為 getHit，並指定傷害類型
					currentTargetActor.getHit((float)healthToDeduct, true, AttackType.None, null, false, false, true);
				}
				// 7. 根據亞種特徵賦予相應的生育狀態
				bool statusApplied = false;
				float currentAntibodyDuration = antibodyDuration;
				// 【新增：孤雌繁殖檢查】
				if (currentTargetActor.hasSubspeciesTrait("reproduction_parthenogenesis"))
				{
					currentTargetActor.addStatusEffect(pregnantParthenogenesisStatus, effectDuration);
					statusApplied = true;
				}
				else if (currentTargetActor.hasSubspeciesTrait("reproduction_strategy_viviparity"))
				{
					currentTargetActor.addStatusEffect(pregnantStatus, effectDuration);
					statusApplied = true;
				}
				else if (currentTargetActor.hasSubspeciesTrait("reproduction_budding"))
				{
					currentTargetActor.addStatusEffect(buddingStatus, effectDuration);
					statusApplied = true;
				}
				else if (currentTargetActor.hasSubspeciesTrait("reproduction_vegetative"))
				{
					currentTargetActor.addStatusEffect(takingRootsStatus, effectDuration);
					statusApplied = true;
				}
				else if (currentTargetActor.hasSubspeciesTrait("reproduction_soulborne"))
				{
					// 修正: 將 actor.a 替換為 currentTargetActor
					currentTargetActor.addStatusEffect(soulHarvestedStatus, soulHarvestedDuration);
					currentAntibodyDuration = soulHarvestedAntibodyDuration;
					statusApplied = true;
				}
				else if (currentTargetActor.hasSubspeciesTrait("reproduction_spores"))
				{
					currentTargetActor.addStatusEffect(justAteStatus, justAteDuration);
					statusApplied = true;
				}
				// 8. 如果有任何生育狀態被賦予，則額外給予 'antibody' 和 'pregnancy2' 狀態
				if (statusApplied)
				{
					currentTargetActor.addStatusEffect("antibody", currentAntibodyDuration);
					currentTargetActor.addStatusEffect(targetMarkStatus, targetMarkDuration);
				}
			}
			// 9. 確保施法者自身獲得 'pregnancy' 冷卻/標記狀態
			selfActor.addStatusEffect(selfCooldownAndMarkStatus, cooldownDuration);
			return true;
		}
		private static bool ProcessRemoveStatus(Actor pActor, string statusID1, string statusID2, string actorName)
		{// 受胎術 孵化輔助方法：處理移除指定狀態的邏輯 (已修改，只處理移除)
			bool removed = false;
			if (pActor.hasStatus(statusID1))
			{
				pActor.finishStatusEffect(statusID1);
				// Debug.Log($"Incubation: {actorName} 的 '{statusID1}' 狀態已被移除。");
				removed = true;
			}
			if (pActor.hasStatus(statusID2))
			{
				pActor.finishStatusEffect(statusID2);
				// Debug.Log($"Incubation: {actorName} 的 '{statusID2}' 狀態已被移除。");
				removed = true;
			}
			return removed;
		}
		public static bool Incubation(BaseSimObject pSelf, WorldTile pTile)
		{// 受胎術 孵化
			if (pSelf == null || pSelf.a == null)
			{
				return false; // 無效的施法者
			}
			Actor selfActor = pSelf.a;

			// 如果 pTile 為 null，則直接返回，避免崩潰 (保留此安全措施)
			if (pTile == null)
			{
				// Debug.Log("Incubation Warning: pTile is null. Skipping effect to prevent crash.");
				return false;
			}

			int range = 10; // 設定影響範圍
			string uprootingStatusID = "uprooting"; // 要移除的狀態ID 1
			string eggStatusID = "egg";			 // 要移除的狀態ID 2
			bool effectAppliedToAnyone = false;	 // 追蹤是否成功對至少一個單位應用了效果

			// 1. 處理施法者自身
			if (ProcessRemoveStatus(selfActor, uprootingStatusID, eggStatusID, selfActor.name))
			{
				effectAppliedToAnyone = true;
			}

			// 2. 獲取範圍內的 Actor
			var allClosestUnits = Finder.getUnitsFromChunk(pTile, range);
			
			if (allClosestUnits.Any())
			{
				foreach (var unit in allClosestUnits)
				{
					Actor targetActor = unit.a; // 從 BaseSimObject 獲取 Actor 組件

					// 檢查目標是否是有效的 Actor，存活，且不是施法者本身，且屬於同一王國
					if (targetActor != null && targetActor.isAlive() && targetActor != selfActor && targetActor.kingdom == selfActor.kingdom)
					{
						if (ProcessRemoveStatus(targetActor, uprootingStatusID, eggStatusID, targetActor.name))
						{
							effectAppliedToAnyone = true; // 只要有一個目標被應用效果，就設為 true
						}
					}
				}
			}
			
			return effectAppliedToAnyone; // 返回是否成功對至少一個目標應用了效果
		}
		public static bool RestoreAllyHunger(BaseSimObject pSelf, WorldTile pTile)
		{// 聖餐 營養值 恢復
			// 1. 基本安全检查：确保 pSelf 及其 Actor 组件存在且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive())
				return false;
			
			Actor selfActor = pSelf.a;

			// 確保 pTile 不為 null
			if (pTile == null)
			{
				Debug.Log("Warning: pTile is null. Skipping RestoreAllyHunger effect to prevent crash.");
				return false;
			}

			// 定義營養值恢復量和範圍
			int nutritionRestoreAmount = 10; // 固定點數恢復，從 float 改為 int 更符合營養值整數性質
			int maxNutritionValue = 100; // 直接使用最大營養值 100，避免 getMaxNutrition() 的不確定性
			float activationThresholdPercentage = 0.99f; // 觸發恢復的閾值 (營養度百分比)
			int restoreRange = 5; // 恢復範圍
			// --- 恢復特質持有者自身的營養度 ---
			// 計算自身當前營養度百分比
			float selfNutritionPercentage = (float)selfActor.data.nutrition / maxNutritionValue; // 使用 100 作為最大值
			if (selfNutritionPercentage < activationThresholdPercentage) // 當營養度低於設定閾值時
			{
				// 確保營養度不會超過最大值，並加上恢復量
				selfActor.data.nutrition = Mathf.Min(maxNutritionValue, selfActor.data.nutrition + nutritionRestoreAmount);
				// Debug.Log($"{selfActor.name} 的營養度恢復了 {nutritionRestoreAmount} 點 (自身). 當前: {selfActor.data.nutrition}");
			}
			// --- 恢復範圍內同陣營單位的營養度 ---
			var allClosestUnits = Finder.getUnitsFromChunk(pTile, restoreRange);

			if (allClosestUnits.Any())
			{
				foreach (var unit in allClosestUnits)
				{
					Actor targetActor = unit.a; // 從 BaseSimObject 獲取 Actor 組件

					// 確保 unit 是有效的 Actor，存活，且不是 pSelf 本身，且屬於同一王國
					if (targetActor != null && targetActor.isAlive() && targetActor != selfActor && targetActor.kingdom == selfActor.kingdom)
					{
						// 計算友軍當前營養度百分比
						float otherActorNutritionPercentage = (float)targetActor.data.nutrition / maxNutritionValue; // 使用 100 作為最大值
						
						if (otherActorNutritionPercentage < activationThresholdPercentage) // 當友軍營養度低於設定閾值時
						{
							// 確保營養度不會超過最大值，並加上恢復量
							targetActor.data.nutrition = Mathf.Min(maxNutritionValue, targetActor.data.nutrition + nutritionRestoreAmount);
							// Debug.Log($"{targetActor.name} 的營養度恢復了 {nutritionRestoreAmount} 點 (友軍). 當前: {targetActor.data.nutrition}");
						}
					}
				}
			}
			return true; // 效果成功執行，無論是否有單位被恢復
		}
		public static bool HolyArts_Bond_effect(BaseSimObject pSelf, WorldTile pTile = null)
		{// 絆
			// 確保 pSelf 是一個有效的 Actor 且存活
			if (pSelf?.a == null || !pSelf.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			Family selfFamily = selfActor.family;
			// 傳播特質的目標清單
			HashSet<Actor> targets = new HashSet<Actor>();
			// 1. 將所有家庭成員加入目標清單
			if (selfFamily != null)
			{
				foreach (Actor familyMember in selfFamily.units)
				{
					if (familyMember != null && familyMember.isAlive() && familyMember != selfActor)
					{
						targets.Add(familyMember);
					}
				}
			}
			// 2. 將戀人加入目標清單
			if (selfActor.lover != null)
			{
				Actor lover = selfActor.lover;
				// 確保戀人有效、存活且不是自己
				if (lover != null && lover.isAlive() && lover != selfActor)
				{
					targets.Add(lover);
				}
			}
			// 3. 遍歷目標清單並傳播特質
			foreach (Actor target in targets)
			{
				// 檢查目標是否已經擁有 holyarts_bond 特質
				if (!target.hasTrait("holyarts_bond"))
				{
					// 如果沒有，就為他添加這個特質
					target.addTrait("holyarts_bond");
				}
			}
			return true;
		}
		public static bool Anti_tantrum_Effect(BaseSimObject pSelf, WorldTile pTile = null)
		{// 寧靜 息怒
			// 1. 基本安全检查：确保 pSelf 及其 Actor 组件存在且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) 
				return false;
			Actor selfActor = pSelf.a;
			// 2. 上半部：特质持有者自身添加条件 (保留您的状态检查)
			// 确保自身没有 "cdt_buff00" (冷却状态) 且没有 "cdt_debuff03"
			if (selfActor.hasStatus("angry") || selfActor.hasStatus("tantrum"))
			{
				selfActor.finishStatusEffect("angry"); // 冷却状态
				selfActor.finishStatusEffect("tantrum");	// 指定狀態
				selfActor.finishStatusEffect("antibody");	// 指定狀態
				selfActor.addStatusEffect("serenity", 60f);	// 指定狀態
				selfActor.addStatusEffect("stunned", 0.01f);	// 指定狀態
			}
			// 3. 下半部：对同势力单位的添加条件 (使用 0.50 API 并保留状态检查)
			var allClosestUnits = Finder.getUnitsFromChunk(pTile, 1); // 范围 5 保持一致
			if (allClosestUnits.Any()) // 使用 LINQ 的 Any() 检查是否有单位
			{
					foreach (var unit in allClosestUnits)
				{
					// 确保 unit 是有效的 Actor，存活，且不是 pSelf 本身
					if (unit.a != null && unit.a.isAlive() && unit.a != selfActor) 
					{
						// 检查是否属于同一王国
						if (unit.a.kingdom == selfActor.kingdom)
						{
							if (unit.a.hasTrait("evillaw_tantrum"))
							{
								continue; // 如果有這個特質，跳過本次迴圈，不執行後續程式碼
							}
							// 检查同势力单位是否没有 "指定狀態"
							if (unit.a.hasStatus("angry") || unit.a.hasStatus("tantrum"))
							{
								unit.a.finishStatusEffect("angry");
								unit.a.finishStatusEffect("tantrum"); 
								unit.a.finishStatusEffect("antibody");	// 指定狀態
								unit.a.addStatusEffect("serenity", 60f);	// 指定狀態
								unit.a.addStatusEffect("stunned", 2f);	// 指定狀態
							}
						}
					}
				}
			}	
			return false; // 特殊效果通常返回 false，表示不阻止其他效果链
		}
		public static bool Extinguished(BaseSimObject pTarget, WorldTile pTile = null)
		{// 寧靜 滅火
			if (pTarget == null || !pTarget.isActor() || !pTarget.a.isAlive())
				return false;
			Actor selfActor = pTarget.a;
			// === 狀態檢查：必須同時擁有兩個狀態才能發動 ===
			int Extinguished_RADIUS = 40;
			World.world.loopWithBrush(pTarget.current_tile,
			Brush.get(Extinguished_RADIUS, "circ_"),
			new PowerActionWithID(Traits01Actions.Extinguished_Assist),
			null);
			return false;
		}
		public static bool Extinguished_Assist(WorldTile pTile, string pPower)
		{// 寧靜 滅火輔助效果
			if (pTile == null)
			{
				return false;
			}
			// 檢查並冷卻岩漿
			if (pTile.Type.lava)
			{
				LavaHelper.coolDownLava(pTile);
			}
			// 檢查並停止燃燒
			if (pTile.isOnFire())
			{
				pTile.stopFire();
			}
			return true;
		}
		public static bool StopErupting2(BaseSimObject pSelf, WorldTile pTile)
		{// 火山酸泉非活性化 (快速)
			// 1. 安全檢查：確保發動者和地塊資訊存在。
			if (pSelf == null || pTile == null)
			{
				return false;
			}
			bool effectApplied = false;
			// 2. 獲取單位周圍的所有建築物，範圍為 1 (與 getUnitsFromChunk 的範圍相同)
			var allBuildings = Finder.getBuildingsFromChunk(pTile, 2);
			
			foreach (var building in allBuildings)
			{
				// 3. 檢查建築物是否為火山或酸間歇泉
				if (building.asset.id == "volcano" || building.asset.id == "geyser_acid")
				{
					// 4. 對符合條件的建築物添加 stop_spawn_drops 旗標
					building.data.addFlag("stop_spawn_drops");
					effectApplied = true;
				}
			}
			return effectApplied;
		}
	//雨水
		public static bool castRain(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 造雨 效果
			if (pTarget == null)
			{
				return false;
			}
			WorldTile targetTile = pTarget.current_tile;
			if (targetTile == null)
			{
				return false;
			}
			// 在目標瓦片上生成雨水，達到滅火效果
			World.world.drop_manager.spawn(targetTile, "rain", 1f, -1f);
			return true;
		}
		public static bool Rain00(BaseSimObject pSelf, WorldTile pTile = null)
		{// 雨水 對 單位燃燒狀態
			// 1. 基本安全檢查：確保 pSelf 及其 Actor 组件存在且存活
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf as Actor;
			// 2. 冷卻狀態檢查
			string cooldownStatus = "cdt_rain01";
			float cooldownDuration = 1f;
			if (selfActor.hasStatus(cooldownStatus) || selfActor.hasStatus("cdt_debuff03"))
			{
				return false;
			}
			if (selfActor.subspecies.hasTrait("hydrophobia"))
			{
				return false;
			}
			// 3. 核心邏輯：尋找並滅火
			WorldTile currentTile = selfActor.current_tile;
			if (currentTile == null)
			{
				return false;
			}
			// 尋找目標的範圍
			var allClosestUnits = Finder.getUnitsFromChunk(pTile, 1);
			// 準備一個清單來儲存所有需要滅火的目標
			List<Actor> targetsToRain = new List<Actor>();
			foreach (var unit in allClosestUnits)
			{
				// 確保 unit 是有效的 Actor，存活
				if (unit == null || !unit.isAlive())
				{
					continue;
				}
				// 檢查是否為友軍或自身，並帶有「燃燒」狀態
				if ((unit.kingdom == selfActor.kingdom || unit == selfActor) && unit.hasStatus("burning"))
				{
					targetsToRain.Add(unit);
				}
			}
			// 如果沒有任何目標需要滅火，則不發動能力
			if (targetsToRain.Count == 0)
			{
				return false;
			}
			// 4. 施加冷卻狀態
			selfActor.addStatusEffect(cooldownStatus, cooldownDuration);
			// 5. 對所有目標應用效果
			foreach (var target in targetsToRain)
			{
				castRain(selfActor, target, null);
			}
			return true; // 成功執行了效果
		}
		public static bool Rain01(BaseSimObject pSelf, WorldTile pTile = null)
		{// 雨水 對 小麥建築
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			WorldTile currentTile = pSelf.current_tile;
			if (currentTile == null)
			{
				return false;
			}
			// 2. 冷卻狀態檢查
			string cooldownStatus = "cdt_rain02";
			float cooldownDuration = 900f;
			Actor selfActor = pSelf as Actor;
			if (selfActor == null || !selfActor.isAlive())
			{
				return false;
			}
			if (selfActor.hasStatus(cooldownStatus) || selfActor.hasStatus("cdt_debuff03"))
			{
				return false;
			}
			if (selfActor.subspecies.hasTrait("hydrophobia"))
			{
				return false;
			}
			// === 檢查單位是否擁有 TargetTrait 清單中的任一特質 ===
			foreach (string traitId in TargetTrait)
			{
				if (selfActor.hasTrait(traitId))
				{
					return false;
				}
			}
			// === 核心邏輯：尋找並鎖定未成熟的小麥田 ===
			Building targetBuilding = null;
			float searchRadius = 25f;
			foreach (var building in World.world.buildings)
			{
				if (building == null || !building.isAlive())
				{
					continue;
				}
				if (Vector2.Distance(selfActor.current_position, building.current_position) > searchRadius)
				{
					continue;
				}
				// 如果找到了未成熟的小麥田，將它設定為目標並跳出迴圈
				if (building.asset.wheat && building.component_wheat._current_level < building.component_wheat._max_level)
				{
					targetBuilding = building;
					break; 
				}
			}
			// 如果沒有找到任何未成熟的小麥田，則不發動能力
			if (targetBuilding == null)
			{
				return false;
			}
			// === 檢查結束 ===
			// 3. 施加冷卻狀態
			pSelf.a.addStatusEffect(cooldownStatus, cooldownDuration);
			// 4. 設定掉落物 ID 和生成參數
			string dropId_01 = "rain";
			int numberOfDrops_01 = 550;
			float spreadRadius_01 = 15f;
			// 5. 確定生成中心瓦片，使用找到的目標建築物瓦片
			pTile = targetBuilding.current_tile;
			if (pTile == null)
			{
				return false;
			}
			// 6. 進行多回掉落
			for (int i = 0; i < numberOfDrops_01; i++)
			{
				UnityEngine.Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spreadRadius_01;
				int newTileX = (int)pTile.pos.x + (int)randomOffset.x;
				int newTileY = (int)pTile.pos.y + (int)randomOffset.y;
				if (newTileX >= 0 && newTileX < MapBox.width &&
					newTileY >= 0 && newTileY < MapBox.height)
				{
					WorldTile randomTile = World.world.GetTile(newTileX, newTileY);
					if (randomTile != null)
					{
						World.world.drop_manager.spawn(randomTile, dropId_01, 15f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_01, 17f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_01, 19f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_01, 21f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_01, 23f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_01, 25f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_01, 27f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_01, 29f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_01, 31f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_01, 33f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_01, 35f, -1f);
					}
				}
			}
			return true;
		}
		public static bool Rain02(BaseSimObject pSelf, WorldTile pTile = null)
		{// 雨水 對 恐水單位
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			WorldTile currentTile = pSelf.current_tile;
			if (currentTile == null)
			{
				return false;
			}
			// 2. 冷卻狀態檢查
			string cooldownStatus = "cdt_rain04"; // 獨立的冷卻狀態
			float cooldownDuration = 5f;
			Actor selfActor = pSelf as Actor;
			if (selfActor == null || !selfActor.isAlive())
			{
				return false;
			}
			if (selfActor.hasStatus(cooldownStatus) || selfActor.hasStatus("cdt_debuff03"))
			{
				return false;
			}
			if (selfActor.subspecies.hasTrait("hydrophobia"))
			{
				return false;
			}
			// === 核心邏輯：判斷是否需要發動能力 ===
			bool foundTarget = false;
			float searchRadius = 30f;
			// 遍歷範圍內的瓦片，尋找恐水症敵人
			World.world.loopWithBrush(currentTile, Brush.get((int)searchRadius, "circ_"), delegate(WorldTile tTile, string pID)
			{
				// 新增檢查：確保 tTile 不為 null
				if (tTile == null)
				{
					return true; // 繼續下一個瓦片
				}
				tTile.doUnits(delegate(Actor tUnit)
				{
					// 強化檢查：確保 tUnit.subspecies 不為 null
					if (tUnit != null && tUnit.isAlive() && tUnit.subspecies != null && isEnemy(selfActor, tUnit) && tUnit.subspecies.hasTrait("hydrophobia"))
					{
						foundTarget = true;
						return;
					}
				});
				if (foundTarget)
				{
					return false; // 找到目標，停止循環
				}
				return true;
			}, null);
			// 如果沒有找到恐水症敵人，則檢查是否有 cybercore 建築物
			if (!foundTarget)
			{
				foreach (var building in World.world.buildings)
				{
					if (building == null || !building.isAlive())
					{
						continue;
					}
					if (Vector2.Distance(selfActor.current_position, building.current_position) > searchRadius)
					{
						continue;
					}
					if (building.asset.id == "cybercore")
					{
						foundTarget = true;
						break;
					}
				}
			}
			// 如果沒有找到任何目標，則不發動能力
			if (!foundTarget)
			{
				return false;
			}
			// === 檢查結束 ===
			// 3. 施加冷卻狀態
			selfActor.addStatusEffect(cooldownStatus, cooldownDuration);
			// 4. 設定掉落物 ID 和生成參數
			string dropId_01 = "rain";
			int numberOfDrops_01 = 500;
			float spreadRadius_01 = 15f;
			// 5. 確定生成中心瓦片
			pTile = selfActor.current_tile;
			// 6. 進行多回掉落
			for (int i = 0; i < numberOfDrops_01; i++)
			{
				UnityEngine.Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spreadRadius_01;
				int newTileX = (int)pTile.pos.x + (int)randomOffset.x;
				int newTileY = (int)pTile.pos.y + (int)randomOffset.y;
				if (newTileX >= 0 && newTileX < MapBox.width &&
					newTileY >= 0 && newTileY < MapBox.height)
				{
					WorldTile randomTile = World.world.GetTile(newTileX, newTileY);
					if (randomTile != null)
					{
						World.world.drop_manager.spawn(randomTile, dropId_01, 15f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_01, 16f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_01, 17f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_01, 18f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_01, 19f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_01, 20f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_01, 21f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_01, 22f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_01, 23f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_01, 24f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_01, 25f, -1f);
					}
				}
			}
			return true;
		}
		public static bool Rain03(BaseSimObject pSelf, WorldTile pTile = null)
		{// 雨水 對 燃燒地塊 建築 岩漿
			// 1. 基本安全安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			WorldTile currentTile = pSelf.current_tile;
			if (currentTile == null)
			{
				return false;
			}
			// 2. 冷卻狀態檢查
			string cooldownStatus = "cdt_rain03";
			float cooldownDuration = 4f;
			Actor selfActor = pSelf as Actor;
			if (selfActor == null || !selfActor.isAlive())
			{
				return false;
			}
			if (selfActor.hasStatus(cooldownStatus) || selfActor.hasStatus("cdt_debuff03"))
			{
				return false;
			}
			if (selfActor.subspecies.hasTrait("hydrophobia"))
			{
				return false;
			}
			// === 檢查單位是否擁有 TargetTrait 清單中的任一特質 ===
			foreach (string traitId in TargetTrait)
			{
				if (selfActor.hasTrait(traitId))
				{
					return false;
				}
			}
			// === 核心邏輯：判斷是否需要發動能力 ===
			WorldTile targetTile = null;
			float searchRadius = 50f;
			// 遍歷所有瓦片，尋找第一個著火的瓦片
			for (int i = 0; i < MapBox.width; i++)
			{
				for (int j = 0; j < MapBox.height; j++)
				{
					WorldTile tTile = World.world.GetTile(i, j);
					if (Vector2.Distance(currentTile.pos, tTile.pos) > searchRadius)
					{
						continue;
					}
					if (tTile.isOnFire())
					{
						if (tTile.Type != null && UnusablePlots3.Contains(tTile.Type.id))
						{
							if (isTargetSurroundedBySoil(tTile, 8))
							{
								continue;
							}
						}
						targetTile = tTile;
						break;
					}
				}
				if (targetTile != null)
				{
					break;
				}
			}
			// 如果沒有找到著火瓦片，則檢查著火的建築物
			if (targetTile == null)
			{
				foreach (var building in World.world.buildings)
				{
					if (building == null || !building.isAlive())
					{
						continue;
					}
					if (Vector2.Distance(selfActor.current_position, building.current_position) > searchRadius)
					{
						continue;
					}
					if (building.hasStatus("burning"))
					{
						if (building.asset.id != null && BadBuilding.Contains(building.asset.id))
						{
							if (isTargetSurroundedBySoil(building.current_tile, 8))
							{
								continue;
							}
						}
						targetTile = building.current_tile;
						break;
					}
				}
			}
			// === 修正：如果沒有找到任何目標，則檢查範圍內的岩漿地塊 ===
			if (targetTile == null)
			{
				for (int i = 0; i < MapBox.width; i++)
				{
					for (int j = 0; j < MapBox.height; j++)
					{
						WorldTile tTile = World.world.GetTile(i, j);
						if (Vector2.Distance(currentTile.pos, tTile.pos) > searchRadius)
						{
							continue;
						}
						if (tTile.Type != null && Lava.Contains(tTile.Type.id))
						{
							targetTile = tTile;
							break;
						}
					}
					if (targetTile != null)
					{
						break;
					}
				}
			}
			// 如果沒有找到任何目標，則不發動能力
			if (targetTile == null)
			{
				return false;
			}
			// === 檢查結束 ===
			// 3. 施加冷卻狀態
			pSelf.a.addStatusEffect(cooldownStatus, cooldownDuration);
			// 4. 設定掉落物 ID 和生成參數
			string dropId_01 = "rain";
			int numberOfDrops_01 = 1000;
			float spreadRadius_01 = 4f; //降雨範圍(半徑)
			// 5. 確定生成中心瓦片，使用找到的目標瓦片
			pTile = targetTile;
			// 6. 進行多回掉落
			for (int i = 0; i < numberOfDrops_01; i++)
			{
				UnityEngine.Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spreadRadius_01;
				int newTileX = (int)pTile.pos.x + (int)randomOffset.x;
				int newTileY = (int)pTile.pos.y + (int)randomOffset.y;
				if (newTileX >= 0 && newTileX < MapBox.width &&
					newTileY >= 0 && newTileY < MapBox.height)
				{
					WorldTile randomTile = World.world.GetTile(newTileX, newTileY);
					if (randomTile != null)
					{
						World.world.drop_manager.spawn(randomTile, dropId_01, 15f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_01, 16f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_01, 17f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_01, 18f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_01, 19f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_01, 20f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_01, 21f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_01, 22f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_01, 23f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_01, 24f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_01, 25f, -1f);
					}
				}
			}
			return true;
		}
		private static bool isTargetSurroundedBySoil(WorldTile pTargetTile, int pRadius)
		{// 滅火雨 輔助 目標瓦片周圍是否有指定的土壤類型
		/// <summary>
		/// 檢查目標瓦片周圍是否有指定的土壤類型。
		/// </summary>
		// 檢查目標瓦片周圍是否有指定的安全地塊類型。
		if (pTargetTile == null)
		{
			return false;
		}
		// 使用你提供的 AllTile01 清單作為檢查依據
		bool foundSafePlot = false;
		World.world.loopWithBrush(pTargetTile, Brush.get(pRadius, "circ_"), delegate(WorldTile tTile, string pID)
		{
			if (tTile != null && tTile.Type != null && AllTile01.Contains(tTile.Type.id))
			{
				foundSafePlot = true;
				return false; // 找到後立即停止遍歷
			}
			return true; // 繼續遍歷
		}, null);
		return foundSafePlot;
		}

		public static bool JusticeAttack(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 裁決 特別攻擊
			// 安全檢查...
			if (pSelf?.a == null || pTarget?.a == null || !pSelf.a.isAlive() || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			Actor targetActor = pTarget.a;
			if (selfActor.hasStatus("evil"))
			{
				return false;
			}
			float totalBonusMultiplier = 0f;
			float SetValue1 = 1.00f;
			float SetValue2 = 6.00f;
			bool hasTriggered = false;
			// 檢查數值屬性
			if (targetActor.hasTrait("evil"))
			{//持有 evil 特質
				totalBonusMultiplier += SetValue1;
				hasTriggered = true;
			}
			if (targetActor.hasTrait("evillaw_tgc"))
			{//持有 evillaw_tgc 特質
				totalBonusMultiplier += SetValue1;
				hasTriggered = true;
			}
			if (targetActor.hasTrait("evillaw_devour"))
			{//持有 evillaw_devour 特質
				totalBonusMultiplier += SetValue1;
				hasTriggered = true;
			}
			if (targetActor.hasTrait("evillaw_tc"))
			{//持有 evillaw_tc 特質
				totalBonusMultiplier += SetValue1;
				hasTriggered = true;
			}
			if (targetActor.hasTrait("evillaw_starvation"))
			{//持有 evillaw_starvation 特質
				totalBonusMultiplier += SetValue1;
				hasTriggered = true;
			}
			if (targetActor.hasTrait("evillaw_disease"))
			{//持有 evillaw_disease 特質
				totalBonusMultiplier += SetValue1;
				hasTriggered = true;
			}
			if (targetActor.hasTrait("evillaw_moneylaw"))
			{//持有 evillaw_moneylaw 特質
				totalBonusMultiplier += SetValue1;
				hasTriggered = true;
			}
			if (targetActor.hasTrait("evillaw_ea"))
			{//持有 evillaw_ea 特質
				totalBonusMultiplier += SetValue1;
				hasTriggered = true;
			}
			if (targetActor.hasTrait("evillaw_sleeping"))
			{//持有 evillaw_sleeping 特質
				totalBonusMultiplier += SetValue1;
				hasTriggered = true;
			}
			if (targetActor.hasTrait("evillaw_sterilization"))
			{//持有 evillaw_sterilization 特質
				totalBonusMultiplier += SetValue1;
				hasTriggered = true;
			}
			if (targetActor.hasTrait("evillaw_tantrum"))
			{//持有 evillaw_tantrum 特質
				totalBonusMultiplier += SetValue1;
				hasTriggered = true;
			}
			if (targetActor.hasTrait("evillaw_seduction"))
			{//持有 evillaw_seduction 特質
				totalBonusMultiplier += SetValue1;
				hasTriggered = true;
			}
			if (targetActor.hasTrait("evillaw_ew"))
			{//持有 evillaw_ew 特質
				totalBonusMultiplier += SetValue1;
				hasTriggered = true;
			}
			if (targetActor.hasStatus("evil"))
			{//持有 evil 狀態
				totalBonusMultiplier += SetValue2;
				hasTriggered = true;
			}
			if (targetActor.hasStatus("arrogant_demon_king"))
			{//持有 arrogant_demon_king 狀態
				totalBonusMultiplier += SetValue2;
				hasTriggered = true;
			}
			if (targetActor.hasStatus("greedy_demon_king"))
			{//持有 greedy_demon_king 狀態
				totalBonusMultiplier += SetValue2;
				hasTriggered = true;
			}
			if (targetActor.hasStatus("lust_demon_king"))
			{//持有 lust_demon_king 狀態
				totalBonusMultiplier += SetValue2;
				hasTriggered = true;
			}
			if (targetActor.hasStatus("wrath_demon_king"))
			{//持有 wrath_demon_king 狀態
				totalBonusMultiplier += SetValue2;
				hasTriggered = true;
			}
			if (targetActor.hasStatus("gluttony_demon_king"))
			{//持有 gluttony_demon_king 狀態
				totalBonusMultiplier += SetValue2;
				hasTriggered = true;
			}
			if (targetActor.hasStatus("sloth_demon_king"))
			{//持有 sloth_demon_king 狀態
				totalBonusMultiplier += SetValue2;
				hasTriggered = true;
			}
			if (targetActor.hasStatus("envy_demon_king"))
			{//持有 envy_demon_king 狀態
				totalBonusMultiplier += SetValue2;
				hasTriggered = true;
			}
			if (targetActor.hasStatus("ex_undead_emperor"))
			{//持有 ex_undead_emperor 狀態
				totalBonusMultiplier += SetValue2;
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
		public static bool JusticeDefense1(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 裁決 特別防禦
			// 1. 安全檢查
			if (pSelf?.a == null || pTarget?.a == null || !pSelf.a.isAlive() || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a; // 防禦者
			Actor targetActor = pTarget.a; // 攻擊者
			if (selfActor.hasStatus("evil"))
			{
				return false;
			}
			int braveStatusCount = 0;
			int evilTraitCount = 0;
			int demonKingStatusCount = 0;
			// 2. 計算 邪惡特質 與 狀態 總數
			if (selfActor.hasStatus("brave"))
			{
				braveStatusCount++;
			}
			if (targetActor.hasStatus("evil"))
			{
				evilTraitCount++;
			}
			foreach (string traitID in EvilLawTraits)
			{
				if (targetActor.hasTrait(traitID))
				{
					evilTraitCount++;
				}
			}
			foreach (string statusID in SevenDemonKingStatus_DemonKing)
			{
				if (targetActor.hasStatus(statusID))
				{
					demonKingStatusCount++;
				}
			}
			// 3. 只有當目標持有邪惡特質或狀態時才執行
			if (braveStatusCount > 0 || evilTraitCount > 0 || demonKingStatusCount > 0)
			{
				// 配置強度和上限
				const float PerBraveDefense = 0.25f; 	// 每個特質提供 10% 減傷
				const float PerTraitDefense = 0.10f; 	// 每個特質提供 10% 減傷
				const float PerHighStatusDefense = 2.00f; // 每個魔王/evil狀態提供 80% 減傷 (修正名稱以區分)
				const float MaxDefense = 3.00f;	 		// 減傷上限為 300%
				// 修正：計算總減傷比例
				float defenseAmount = (braveStatusCount * PerBraveDefense) +  (evilTraitCount * PerTraitDefense) + (demonKingStatusCount * PerHighStatusDefense);
				// 限制總減傷乘數
				float defenseMultiplier = Mathf.Min(defenseAmount, MaxDefense);
				// 計算抵銷量：使用攻擊者「全部」的基礎傷害值
				// 假設 targetActor.stats["damage"] 是基礎傷害量
				float totalDamageToHeal = targetActor.stats["damage"] * defenseMultiplier; 
				// 4. 執行生命值恢復（傷害抵銷）
				// 將當前生命值加上抵銷傷害，並限制在最大生命值 (MaxHealth)
				selfActor.data.health = (int)Mathf.Min(selfActor.data.health + totalDamageToHeal, selfActor.getMaxHealth());
				return true; // 效果成功發動
			}
			return false; // 沒有邪惡特質，效果未發動
		}
		public static bool BraveEffect(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 裁決 對魔王專用
			// 1. 基本安全检查：确保 pSelf 及其 Actor 组件存在且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) 
				return false;
			Actor selfActor = pSelf.a;
			// 檢查目標是否有效 (防止目標為 null 導致崩潰)
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
				return false;
			Actor targetActor = pTarget.a;
			// --- 核心條件編寫：檢查目標是否持有任一魔王狀態 ---
			if (selfActor.hasStatus("evil"))
			{
				return false;
			}
			bool isDemonKing = false;
			foreach (string statusID in SevenDemonKingStatus_DemonKing)
			{
				if (targetActor.hasStatus(statusID))
				{
					isDemonKing = true;
					break; // 找到任一狀態即停止檢查
				}
			}
			// 2. 條件檢查與狀態賦予
			if (isDemonKing)
			{
				// 目標是魔王，為自己添加 "brave" 狀態
				selfActor.addStatusEffect("brave", 10f);
			}
			return false; // 特殊效果通常返回 false，表示不阻止其他效果鏈
		}
		public static bool Judgment(BaseSimObject pSelf, WorldTile pTile)
		{// 裁決 宣告邪惡
			// 1. 基本安全检查
			if (pSelf == null || pSelf.a == null || pTile == null)
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			// 檢查機制 是否有指定特質 或 狀態
			Kingdom currentKingdom = selfActor.kingdom;
			if (currentKingdom != null)
			{
				bool isDemonKingExists = false; // 設置一個旗標
				foreach (Actor kingdomUnit in currentKingdom.units)
				{
					// 跳過自己
					if (kingdomUnit == null || kingdomUnit == selfActor)
					{
						continue;
					}
					// 優先檢查是否有 'other666' 特質
					if (kingdomUnit.hasTrait("hope")||						//裁決專用標記特質
						kingdomUnit.hasTrait("other6661")||					//魔王專用標記特質
						kingdomUnit.hasTrait("other6662")||					//魔王專用標記特質
						kingdomUnit.hasTrait("other6663")||					//魔王專用標記特質
						kingdomUnit.hasTrait("other6664")||					//魔王專用標記特質
						kingdomUnit.hasTrait("other6665")||					//魔王專用標記特質
						kingdomUnit.hasTrait("other6666")||					//魔王專用標記特質
						kingdomUnit.hasTrait("other6667")||					//魔王專用標記特質
						kingdomUnit.hasTrait("other6668")||					//魔王專用標記特質
						kingdomUnit.hasTrait("other6669")||					//魔王專用標記特質
						kingdomUnit.hasTrait("extraordinary_authority")		//不死王帝標記特質
						)
					{
						isDemonKingExists = true;
						break;
					}
					// 檢查單位是否擁有任一魔王狀態
					foreach (string demonKingStatusID in SevenDemonKingStatus_Brave)
					{
						if (kingdomUnit.hasStatus(demonKingStatusID))
						{
							isDemonKingExists = true; // 發現魔王，設置旗標
							break; // 跳出內層迴圈
						}
					}
					if (isDemonKingExists)
					{
						break; // 發現魔王，跳出外層迴圈
					}
				}
				// 根據旗標結果判斷是否返回
				if (isDemonKingExists)
				{
					return false;
				}
			}
			int range = 900; 
			string EvilStatusID = "evil"; 
			float EvilDuration = 10f; 
			bool effectAppliedToAnyone = false; 
			// --- 輔助函式：檢查目標王國是否為魔王勢力 ---
			// (邏輯保持不變)
			bool isTargetKingdomDemonKingAffiliated(Kingdom pTargetKingdom)
			{
				if (pTargetKingdom == null) return false;
				foreach (var unit in pTargetKingdom.units)
				{
					foreach (string statusID in SevenDemonKingStatus_Brave)
					{
						if (unit.hasStatus(statusID))
						{
							return true; 
						}
					}
				}
				return false;
			}
			// 2. 獲取範圍內的 Actor
			var allClosestUnits = Finder.getUnitsFromChunk(pTile, range);

			// 新增旗標：追蹤是否找到了需要賦予「邪惡」狀態的有效目標
			bool foundThreat = false; 

			// 3. 遍歷範圍內的 Actor
			if (allClosestUnits.Any())
			{
				foreach (var unit in allClosestUnits)
				{
					Actor targetActor = unit.a; 
					
					// 基礎安全和王國檢查
					if (targetActor == null || !targetActor.isAlive() || targetActor == selfActor || targetActor.kingdom == selfActor.kingdom)
					{
						continue;
					}
					
					// 魔王勢力檢查
					if (!isTargetKingdomDemonKingAffiliated(targetActor.kingdom))
					{
						// ========== 清除邪惡狀態 (只對目標執行，不影響施法者) ==========
						// 如果目標王國不再是魔王勢力，則清除其身上的 "evil" 狀態。
						if (targetActor.hasStatus(EvilStatusID))
						{
							targetActor.finishStatusEffect(EvilStatusID);
						}
						continue; // 跳過施加狀態
					}
					
					// 4. 施加狀態邏輯 (只有在是魔王勢力時才會執行)
					else
					{
						// 施加 "evil" 狀態效果
						targetActor.addStatusEffect(EvilStatusID, EvilDuration); 
						
						// 標記：找到威脅目標
						foundThreat = true; 
						effectAppliedToAnyone = true;
					}
				}
			}

			// 5. 根據是否找到威脅 (foundThreat)，決定施法者的最終狀態
			if (foundThreat)
			{
				// 如果找到魔王勢力，賦予/刷新施法者狀態
				selfActor.addStatusEffect("brave", 30f);
				selfActor.addTrait("hope");
				BraveHelmetGet(pSelf, pTile);
			}
			else
			{
				// 如果範圍內沒有魔王勢力目標，或者所有魔王勢力目標已經被處理完畢，則移除施法者狀態
				//selfActor.finishStatusEffect("brave"); 
				//selfActor.removeTrait("hope");
			}

			return effectAppliedToAnyone; 
		}
		public static bool JusticeJavelin1(BaseSimObject pSelf, WorldTile pTile = null)
		{// 裁決 投槍 對單位
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			
			// 裁決之槍攻擊清單 (假設這個清單在類別層級已定義為 EvilLawTraitsATK)
			// private static readonly HashSet<string> EvilLawTraitsATK = ...
			
			// 冷卻設定
			string attackCooldownStatus = "item_cdt00"; 
			float attackCooldownDuration = 4f;
			// 偵測範圍
			float maxRange = 900f;		
			float minRange = 30f;		
			int requiredEnemies = 1;	
			// 投射物資產ID
			string headProjectileID = "Justice_03"; 
			string bodyProjectileID = "Justice_03"; 
			string tailProjectileID = "Justice_04"; 
			// 中間彈藥設定
			int numberOfProjectiles_Body = 198;	
			float separationDistance_Body = 0.1f;	
			// =======================================================
			
			// 檢查冷卻或施法者狀態
			if (selfActor.hasStatus(attackCooldownStatus) || !selfActor.hasStatus("brave"))
			{
				return false;
			}
			
			// === 核心邏輯：偵測敵人 ===
			BaseSimObject mainTarget = null;
			float closestDist = float.MaxValue;
			int enemyCount = 0;
			
			// --- 輔助函式：檢查目標是否持有邪惡特質 (使用 EvilLawTraitsATK) ---
			bool targetHasEvilTrait(Actor pTargetActor)
			{
				foreach (string traitID in EvilLawTraitsATK)
				{
					if (pTargetActor.hasTrait(traitID))
					{
						return true;	
					}
				}
				return false;
			}

			// *** 移除 targetIsDesignatedEvil 及其內容，因為不再使用生物 ID 清單 ***
			// *** 輔助函式將直接在主循環中被 targetHasEvilTrait 替代 ***
			
			// A. 遍歷所有單位，尋找符合條件的敵人
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;

				// 確保是敵人王國
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;

				// ** 核心 NEW LOGIC: 必須持有邪惡特質 **
				if (other.isActor())
				{
					// 如果目標不持有 EvilLawTraitsATK 中的任意特質，則跳過
					if (!targetHasEvilTrait(other.a))
					{
						continue;
					}
				}
				else
				{
					continue; // 跳過所有非 Actor 的單位（因為它們沒有特質）
				}
				
				// 距離檢查與鎖定最接近目標
				float dist = UnityEngine.Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist <= maxRange && dist > minRange)	
				{
					enemyCount++;
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

				// 檢查是否為有害建築 (BadBuilding)
				if (!BadBuilding.Contains(building.asset.id))
				{
					continue; 
				}

				// 確保是敵人王國的建築
				if (building.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(building.kingdom))
					continue;

				// 距離檢查與鎖定最接近目標
				float dist = UnityEngine.Vector2.Distance(selfActor.current_position, building.current_position);
				if (dist <= maxRange && dist > minRange)
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = building;
					}
				}
			}*/
			
			// 檢查是否滿足發動條件
			if (enemyCount < requiredEnemies || mainTarget == null)
			{
				return false;
			}
			
			// === 成功找到目標，施加冷卻並發射投射物 (一列式射擊) ===
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			JusticeJavelin2(pSelf, pTile);
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
			
			// --- 核心發射邏輯：尾部先發射，頭部最後發射確保渲染層次 ---
			// 1. 【A. 尾部彈 (Tail)】。
			float tailOffset = (numberOfProjectiles_Body + 1) * separationDistance_Body;	
			UnityEngine.Vector3 launchPoint_Tail = initialBasePoint + (normalizedDirection * tailOffset);
			World.world.projectiles.spawn(
				pInitiator: selfActor,
				pTargetObject: mainTarget,
				pAssetID: tailProjectileID,
				pLaunchPosition: launchPoint_Tail,
				pTargetPosition: targetPosition,
				pTargetZ: 0.0f
			);
			
			// 2. 【B. 中間彈 (Body)】。
			for (int i = 1; i <= numberOfProjectiles_Body; i++)
			{
				float currentOffset = i * separationDistance_Body;
				UnityEngine.Vector3 launchPoint_Body = initialBasePoint + (normalizedDirection * currentOffset);
				World.world.projectiles.spawn(
					pInitiator: selfActor,
					pTargetObject: mainTarget,
					pAssetID: bodyProjectileID,
					pLaunchPosition: launchPoint_Body,
					pTargetPosition: targetPosition,
					pTargetZ: 0.0f
				);
			}
			
			// 3. 【C. 頭部彈 (Head) - 最後發射】
			UnityEngine.Vector3 launchPoint_Head = initialBasePoint;
			World.world.projectiles.spawn(
				pInitiator: selfActor,
				pTargetObject: mainTarget,
				pAssetID: headProjectileID,
				pLaunchPosition: launchPoint_Head,
				pTargetPosition: targetPosition,
				pTargetZ: 0.0f
			);
			
			return true;
		}
		public static bool JusticeJavelin2(BaseSimObject pSelf, WorldTile pTile = null)
		{// 裁決 投槍 對建築
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			
			// 裁決之槍攻擊清單 (假設這個清單在類別層級已定義為 EvilLawTraitsATK)
			// private static readonly HashSet<string> EvilLawTraitsATK = ...
			
			// 冷卻設定
			string attackCooldownStatus = "item_cdt01"; 
			float attackCooldownDuration = 4f;
			// 偵測範圍
			float maxRange = 900f;		
			float minRange = 30f;		
			int requiredEnemies = 1;	
			// 投射物資產ID
			string headProjectileID = "Justice_03"; 
			string bodyProjectileID = "Justice_03"; 
			string tailProjectileID = "Justice_04"; 
			// 中間彈藥設定
			int numberOfProjectiles_Body = 198;	
			float separationDistance_Body = 0.1f;	
			// =======================================================
			
			// 檢查冷卻或施法者狀態
			if (selfActor.hasStatus(attackCooldownStatus) || !selfActor.hasStatus("brave"))
			{
				return false;
			}
			
			// === 核心邏輯：偵測敵人 ===
			BaseSimObject mainTarget = null;
			float closestDist = float.MaxValue;
			int enemyCount = 0;
			
			// --- 輔助函式：檢查目標是否持有邪惡特質 (使用 EvilLawTraitsATK) ---
			bool targetHasEvilTrait(Actor pTargetActor)
			{
				foreach (string traitID in EvilLawTraitsATK)
				{
					if (pTargetActor.hasTrait(traitID))
					{
						return true;	
					}
				}
				return false;
			}

/*			// A. 遍歷所有單位，尋找符合條件的敵人
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;

				// 確保是敵人王國
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;

				// ** 核心 NEW LOGIC: 必須持有邪惡特質 **
				if (other.isActor())
				{
					// 如果目標不持有 EvilLawTraitsATK 中的任意特質，則跳過
					if (!targetHasEvilTrait(other.a))
					{
						continue;
					}
				}
				else
				{
					continue; // 跳過所有非 Actor 的單位（因為它們沒有特質）
				}
				
				// 距離檢查與鎖定最接近目標
				float dist = UnityEngine.Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist <= maxRange && dist > minRange)	
				{
					enemyCount++;
					if (dist < closestDist)
					{
						closestDist = dist;
						mainTarget = other;
					}
				}
			}*/

			// B. 遍歷所有建築物，尋找符合條件的敵人
			foreach (var building in World.world.buildings)
			{
				if (building == null || !building.isAlive())
					continue;

				// 檢查是否為有害建築 (BadBuilding)
				if (!Traits01Actions.BadBuilding.Contains(building.asset.id))
				{
					continue; 
				}

				// 確保是敵人王國的建築
				if (building.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(building.kingdom))
					continue;

				// 距離檢查與鎖定最接近目標
				float dist = UnityEngine.Vector2.Distance(selfActor.current_position, building.current_position);
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
			
			// 檢查是否滿足發動條件
			if (enemyCount < requiredEnemies || mainTarget == null)
			{
				return false;
			}
			
			// === 成功找到目標，施加冷卻並發射投射物 (一列式射擊) ===
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			JusticeJavelin1(pSelf, pTile);
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
			
			// --- 核心發射邏輯：尾部先發射，頭部最後發射確保渲染層次 ---
			// 1. 【A. 尾部彈 (Tail)】。
			float tailOffset = (numberOfProjectiles_Body + 1) * separationDistance_Body;	
			UnityEngine.Vector3 launchPoint_Tail = initialBasePoint + (normalizedDirection * tailOffset);
			World.world.projectiles.spawn(
				pInitiator: selfActor,
				pTargetObject: mainTarget,
				pAssetID: tailProjectileID,
				pLaunchPosition: launchPoint_Tail,
				pTargetPosition: targetPosition,
				pTargetZ: 0.0f
			);
			
			// 2. 【B. 中間彈 (Body)】。
			for (int i = 1; i <= numberOfProjectiles_Body; i++)
			{
				float currentOffset = i * separationDistance_Body;
				UnityEngine.Vector3 launchPoint_Body = initialBasePoint + (normalizedDirection * currentOffset);
				World.world.projectiles.spawn(
					pInitiator: selfActor,
					pTargetObject: mainTarget,
					pAssetID: bodyProjectileID,
					pLaunchPosition: launchPoint_Body,
					pTargetPosition: targetPosition,
					pTargetZ: 0.0f
				);
			}
			
			// 3. 【C. 頭部彈 (Head) - 最後發射】
			UnityEngine.Vector3 launchPoint_Head = initialBasePoint;
			World.world.projectiles.spawn(
				pInitiator: selfActor,
				pTargetObject: mainTarget,
				pAssetID: headProjectileID,
				pLaunchPosition: launchPoint_Head,
				pTargetPosition: targetPosition,
				pTargetZ: 0.0f
			);
			
			return true;
		}
		public static bool JusticeBlade(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 裁決 劍峰
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive() || pTarget == null || !pTarget.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			BaseSimObject mainTarget = pTarget; // <--- 直接使用傳入的目標
			// 冷卻設定
			string attackCooldownStatus = "item_cdt03"; // 使用一個專用ID
			float attackCooldownDuration = 0.01f;
			// 偵測範圍
			float maxRange = 30f;	   // 最大有效範圍
			float minRange = 1f;		// 最小有效範圍
			// 投射物資產ID (已依施法者距離重新定義)
			string headProjectileID = "Justice_02"; // 【頭部】 離施法者最近的子彈
			string bodyProjectileID = "Justice_02"; // 【中間】 
			string tailProjectileID = "Justice_01"; // 【尾部】 離施法者最遠的子彈 (最接近目標)
			// 中間彈藥設定
			int numberOfProjectiles_Body = 198;	  // 中間彈藥數量
			float separationDistance_Body = 0.01f;  // 中間彈藥間距 (負數表示向施法者方向延伸)
			// =======================================================
			// 檢查冷卻
			if (selfActor.hasStatus(attackCooldownStatus) || !selfActor.hasStatus("brave"))
			{
				return false;
			}
			// === 核心邏輯：距離和敵對檢查 (取代目標尋找迴圈) ===
			// 1. 檢查敵對關係
			if (mainTarget.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(mainTarget.kingdom))
				return false;
			// 2. 檢查距離是否在有效範圍內
			float dist = Vector2.Distance(selfActor.current_position, mainTarget.current_position);
			if (dist > maxRange || dist < minRange)
			{
				return false;
			}
			// === NEW LOGIC: 檢查目標是否持有任一邪惡特質 (已修正) ===
			bool isEvilTarget = false;
			// 只有 Actor 才能持有特質，首先檢查並轉換類型
			Actor targetActor = mainTarget as Actor;
			if (targetActor != null) // 確保目標是個活著的單位 (Actor)
			{
				foreach (string traitID in EvilLawTraitsATK)
				{
					if (targetActor.hasTrait(traitID)) // 現在可以安全地呼叫 hasTrait()
					{
						isEvilTarget = true;
						break;
					}
				}
			}
			// 注意: 如果目標是建築物 (Building)，targetActor 會是 null，isEvilTarget 會保持 false。
			// 這確保了建築物不會因為沒有特質而錯誤地通過檢查。
			if (!isEvilTarget)
			{
				return false; // 如果目標不是 Actor 或沒有任何邪惡特質，則取消發動
			}
			// === 滿足所有條件，執行射擊 ===	
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			UnityEngine.Vector3 selfPosition = selfActor.current_position;
			UnityEngine.Vector3 targetPosition = mainTarget.current_position;
			// 計算發射者到目標的標準化方向向量
			UnityEngine.Vector3 directionVector = targetPosition - selfPosition;
			UnityEngine.Vector3 normalizedDirection = directionVector.normalized;
			// 計算第一個子彈的發射點 (BasePoint) - 零偏移量，是離施法者最遠的點 (尾部)
			UnityEngine.Vector3 initialBasePoint = Toolbox.getNewPoint(
				selfPosition.x, selfPosition.y, 
				targetPosition.x, targetPosition.y, 
				selfActor.stats["size"] + 0.1f 
			);
			// --- 核心發射邏輯： Head (最靠近施法者) 先發射， Tail (最遠) 最後發射確保渲染層次 ---
			// 1. 【A. 頭部彈 (Head: Justice_01)】：離施法者最近的投射物
			// 頭部位於 Body 的最大負偏移量處。先發射。
			float headOffset = (numberOfProjectiles_Body + 1) * separationDistance_Body; 
			UnityEngine.Vector3 launchPoint_Head = initialBasePoint + (normalizedDirection * headOffset);
			World.world.projectiles.spawn(
				pInitiator: selfActor,
				pTargetObject: mainTarget,
				pAssetID: headProjectileID,
				pLaunchPosition: launchPoint_Head,
				pTargetPosition: targetPosition,
				pTargetZ: 0.0f
			);
			// 2. 【B. 中間彈 (Body: Justice_02)】：填充 Head 和 Tail 之間的空間
			// 從 i=1 開始循環
			for (int i = 1; i <= numberOfProjectiles_Body; i++)
			{
				float currentOffset = i * separationDistance_Body;
				UnityEngine.Vector3 launchPoint_Body = initialBasePoint + (normalizedDirection * currentOffset);
				World.world.projectiles.spawn(
					pInitiator: selfActor,
					pTargetObject: mainTarget,
					pAssetID: bodyProjectileID,
					pLaunchPosition: launchPoint_Body,
					pTargetPosition: targetPosition,
					pTargetZ: 0.0f
				);
			}
			// 3. 【C. 尾部彈 (Tail: Justice_04) - 最後發射】：離施法者最遠的投射物 (最前端)
			// 尾部位於 BasePoint (零偏移量)。最後發射以確保它是視覺上最上層的子彈。
			UnityEngine.Vector3 launchPoint_Tail = initialBasePoint;
			World.world.projectiles.spawn(
				pInitiator: selfActor,
				pTargetObject: mainTarget,
				pAssetID: tailProjectileID,
				pLaunchPosition: launchPoint_Tail,
				pTargetPosition: targetPosition,
				pTargetZ: 0.0f
			);
			return true; // 成功發動攻擊效果
		}
		public static bool DefenseOff(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 裁決 特防解除
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
			if (!Randy.randomChance(0.005f))
			{
				return false;
			}
			// 2. 檢查觸發條件
			bool selfHasBraveStatus = selfActor.hasStatus("brave");
			// 目標必須持有 "defense_on" 狀態
			bool targetHasStatus1 = targetActor.hasStatus("defense_on");
			// 只有當兩個條件都滿足時才執行後續邏輯
			if (selfHasBraveStatus && targetHasStatus1)
			{
				//targetActor.finishStatusEffect("defense_on");
				targetActor.addStatusEffect("defense_off", 10f);
				return true; // 效果成功發動
			}
			else
			{
				return false;
			}
		}
		public static bool BraveHelmetGet(BaseSimObject pTarget, WorldTile pTile = null)
		{// 裁決 武器給予
			// 定義所需的狀態ID常量
			const string braveStatus = "brave"; //魔王狀態
			const string HelmetID = "brave_helmet"; // 物品ID
			// 1. 基本安全檢查：目標是否有效且存活
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor targetActor = pTarget.a;
			// ****** 新增檢查：單位是否擁有 '指定魔王狀態' 狀態 ******
			if (!targetActor.hasStatus(braveStatus))
			{
				return false; // 如果沒有此狀態，則不給予裝備
			}
			// ************************************************************
			// 獲取目標的武器槽位
			var weaponSlot = targetActor.equipment.getSlot(EquipmentType.Helmet);
			// 檢查武器槽位是否已被佔用，且其中持有的道具ID是否為 "指定武器"
			if (weaponSlot != null && weaponSlot.getItem() != null && weaponSlot.getItem().asset.id == HelmetID)
			{
				// 額外檢查：如果武器沒有修飾符，在這裡補上
				Item existingWeapon = weaponSlot.getItem();
				/*if (!existingWeapon.hasMod("eternal"))
				{
					existingWeapon.addMod("eternal");
					existingWeapon.addMod("cursed");
				}*/
				return true;
			}
			// 如果單位沒有持有 "指定武器" 且擁有指定狀態，則繼續進行裝備流程
			// 獲取 "指定武器" 物品資產
			var weaponAsset = AssetManager.items.get(HelmetID);
			if (weaponAsset == null)
			{
				return false; // 無法找到物品資產，返回失敗
			}
			// 生成 "指定武器" 物品實例
			var newItem = World.world.items.generateItem(pItemAsset: weaponAsset);
			// =======================================================
			// === 核心修正：強制添加修飾符 ===
			if (newItem != null)
			{
				//newItem.addMod("eternal");
				//newItem.addMod("cursed");
				//newItem.addMod("power5");
				//newItem.addMod("truth5");
				//newItem.addMod("protection5");
				//newItem.addMod("speed5");
				//newItem.addMod("balance5");
				//newItem.addMod("health5");
				//newItem.addMod("finesse5");
				//newItem.addMod("mastery5");
				//newItem.addMod("knowledge5");
				//newItem.addMod("sharpness5");
			}
			// =======================================================
			// 將 "指定武器" 裝備到目標的武器槽位
			weaponSlot.setItem(newItem, targetActor);
			// 更新目標的統計數據，以反映新裝備帶來的變化
			targetActor.setStatsDirty();
			targetActor.data.health += 99999;
			return true; // 表示效果成功施加
		}
		public static bool ReAddBrave(BaseSimObject pTarget, WorldTile pTile = null)
		{// 裁決 微小添加
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;
			Actor selfActor = pTarget.a;
			if (!selfActor.hasStatus("brave"))
			{
				selfActor.addStatusEffect("brave", 4f);
			}
			return true;
		}
		public static bool SpecialAttackDamage(BaseSimObject pAttacker, BaseSimObject pTarget, float pFinalDamage, WorldTile pTile = null)
		{// 特攻 傷害執行
			if (pTarget == null || !pTarget.isAlive())
			{
				return false;
			}
			// 現在 pAttacker 變數被正確地傳入 getHit 函式
			pTarget.getHit(pFinalDamage, true, AttackType.None, pAttacker, false, false, false);
			return true;
		}

		public static bool Miracle(BaseSimObject pSelf, WorldTile pTile = null)
		{// 聖光 聖光奇蹟 (Miracle) - 範圍內負面特質/狀態淨化
			// 1. 基本安全檢查：確保 pSelf 及其 Actor 組件存在且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;

			// 檢查施法者自身是否有冷卻狀態
			string cooldownStatus = "cdt_buff00"; // 建議將冷卻 ID 補回
			if (selfActor.hasStatus(cooldownStatus) || selfActor.hasStatus("cdt_debuff02"))
			{
				return false; // 如果有冷卻狀態，則不執行任何效果
			}

			// 2. 設定特質/狀態清單和搜索範圍
			string[] negativeTraits = new string[]
			{
				"infected", "plague", "mush_spores", "tumor_infection", "crippled", 
				"eyepatch", "skin_burns", "madness", "desire_alien_mold", 
				"desire_computer", "desire_golden_egg", "desire_harp"
			};
			// 【新增】：要檢查的負面狀態清單
			string[] negativeStatuses = new string[]
			{
				"ash_fever", "cough", "cursed", "tantrum", "angry"
			};

			const int SEARCH_RADIUS = 30; // 使用 DivineLight 的核心效果範圍作為搜索半徑
			
			bool foundNegativeTrait = false;
			WorldTile targetTile = null; // 儲存目標地塊
			WorldTile centerTile = pTile ?? pSelf.current_tile;

			// 3. 遍歷範圍內單位，檢查是否存在負面特質或狀態
			var allClosestUnits = Finder.getUnitsFromChunk(centerTile, SEARCH_RADIUS);
			
			// A. 首先檢查施法者自己
			// 3.1. 檢查自身特質
			foreach (string traitId in negativeTraits)
			{
				if (selfActor.hasTrait(traitId))
				{
					foundNegativeTrait = true;
					targetTile = selfActor.current_tile; // 記錄自己的位置
					break;
				}
			}
			// 3.2. 檢查自身狀態 (只有在尚未找到特質時才檢查狀態)
			if (!foundNegativeTrait)
			{
				foreach (string statusId in negativeStatuses)
				{
					if (selfActor.hasStatus(statusId))
					{
						foundNegativeTrait = true;
						targetTile = selfActor.current_tile; // 記錄自己的位置
						break;
					}
				}
			}


			// B. 如果自己沒有，則檢查周圍單位
			if (!foundNegativeTrait)
			{
				if (allClosestUnits.Any())
				{
					foreach (var unit in allClosestUnits)
					{
						if (unit?.a != null && unit.a.isAlive())
						{
							// 檢查周圍單位的特質
							foreach (string traitId in negativeTraits)
							{
								if (unit.a.hasTrait(traitId))
								{
									foundNegativeTrait = true;
									targetTile = unit.a.current_tile; // 記錄第一個目標的位置
									break; // 找到特質，跳出內層迴圈
								}
							}
							// 檢查周圍單位的狀態 (只有在尚未找到特質時才檢查狀態)
							if (!foundNegativeTrait)
							{
								foreach (string statusId in negativeStatuses)
								{
									if (unit.a.hasStatus(statusId))
									{
										foundNegativeTrait = true;
										targetTile = unit.a.current_tile; // 記錄第一個目標的位置
										break; // 找到狀態，跳出內層迴圈
									}
								}
							}
						}
						if (foundNegativeTrait)
						{
							break; // 找到一個帶有負面效果的單位，立即跳出所有迴圈
						}
					}
				}
			}
			
			// 4. 根據檢查結果執行效果
			if (foundNegativeTrait)
			{
				// 發現負面特質/狀態，觸發 DivineLight 淨化效果
				DivineLight(pSelf, targetTile); // 使用記錄的 targetTile 作為施法中心
				
				// 5. 施加冷卻狀態
				selfActor.addStatusEffect(cooldownStatus, 60f); // 施法者進入冷卻 60 秒
				return true; // 成功觸發效果
			}
			
			return false; // 沒有發現任何負面效果，不執行效果
		}
		public static bool DestroyEvil(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 聖光 毀滅邪惡 (DestroyEvil) - 攻擊觸發的聖光淨化
			// 1. 基本安全檢查：施法者和目標是否有效且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive())
				return false;
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
				return false;
			
			Actor selfActor = pSelf.a;
			Actor targetActor = pTarget.a;
			
			// 2. 設定目標物種 ID 和特質清單
			string[] targetSpeciesIDs = new string[]
			{
				"demon", "skeleton", "jumpy_skull", "ghost", "mush_unit", "mush_animal"
			};
			string[] targetTraits = new string[]
			{
				"zombie"
			};
			
			bool shouldTrigger = false;

			// 3. 檢查目標是否為指定的邪惡物種
			foreach (string speciesID in targetSpeciesIDs)
			{
				if (targetActor.asset.id == speciesID)
				{
					shouldTrigger = true;
					break;
				}
			}

			// 4. 如果目標不是指定物種，檢查是否持有指定特質
			if (!shouldTrigger)
			{
				foreach (string traitID in targetTraits)
				{
					if (targetActor.hasTrait(traitID))
					{
						shouldTrigger = true;
						break;
					}
				}
			}
			
			// 5. 執行效果
			if (shouldTrigger)
			{
				// 觸發 DivineLight 淨化效果
				// 【關鍵修正】：聖光以目標所在位置為中心釋放
				WorldTile targetCenterTile = pTarget.current_tile; // 使用 pTarget 的地塊
				DivineLight(selfActor, targetCenterTile); // 施法者仍然是 selfActor，中心地塊改為 targetCenterTile
				
				// 由於這是一個 Action_attack 觸發，通常不需要冷卻時間，
				// 除非您想限制它的發動頻率 (例如使用 Randy.randomChance 或 addStatusEffect)。
				
				return true; // 成功觸發效果
			}
			
			return false; // 目標不符合觸發條件
		}
		public static bool AnitUndeadDefense(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 聖光 特別防禦 - 傷害抵銷 (即時治療)
			// 1. 安全檢查
			if (pSelf?.a == null || pTarget?.a == null || !pSelf.a.isAlive() || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a; // 防禦者
			Actor targetActor = pTarget.a; // 攻擊者
			
			// 2. 檢查目標是否符合特防清單或持有 "zombie" 特質
			bool isTargetedEnemy = DivineLightTarget.Contains(targetActor.asset.id) || targetActor.hasTrait("zombie");

			// 3. 只有當目標是特防對象時才執行效果
			if (isTargetedEnemy)
			{
				// 配置強度和上限
				const float MaxDefenseMultiplier = 0.99f; // 減傷上限為 99% (即抵銷 99% 傷害)
				float baseDamage = targetActor.stats["damage"]; 
				float totalDamageToHeal = baseDamage * MaxDefenseMultiplier; 
				
				// 4. 執行生命值恢復（傷害抵銷）
				// 將當前生命值加上抵銷傷害，並限制在最大生命值 (MaxHealth)
				selfActor.data.health = (int)Mathf.Min(selfActor.data.health + totalDamageToHeal, selfActor.getMaxHealth());
				
				// 視覺效果：可以考慮在此處添加一個治癒或防禦的特效，讓玩家知道特防發動了
				
				return true; // 效果成功發動
			}
			return false; // 不是目標，效果未發動
		}
			#endregion
			#region 負面狀態 特殊強化
	//邪法效果
		public static bool Evil_Law1(BaseSimObject pSelf, WorldTile pTile)
		{// 大咒法 賦予詛咒/移除祝福/淨化己方詛咒
			// 1. 基本安全檢查：確保施法者 pSelf 及其 Actor 組件存在且存活
			if (pSelf == null || pSelf.a == null) 
			{
				return false; 
			}
			Actor selfActor = pSelf.a;

			// ** 臨時安全措施：如果 pTile 為 null，則直接返回，避免崩潰 **
			if (pTile == null)
			{
				// Debug.Log("EvilLaw Warning: pTile is null. Skipping effect to prevent crash.");
				return false;
			}
			
			int range = 5; // 設定影響範圍
			string cursedStatusID = "cursed"; // 詛咒的狀態ID
			float cursedDuration = 666f; // 詛咒狀態的持續時間
			string blessedTraitID = "blessed"; // 祝福的特質ID
			bool effectAppliedToAnyone = false; // 追蹤是否成功對至少一個外部目標應用了效果
			
			// 1. ====== 檢查並移除施法者自身的 "cursed" 狀態效果 (保持不變) ======
			if (selfActor.hasStatus(cursedStatusID))
			{
				selfActor.finishStatusEffect(cursedStatusID); 
			}
			
			// 2. 獲取範圍內的 Actor
			var allClosestUnits = Finder.getUnitsFromChunk(pTile, range);
			
			// 遍歷範圍內的 Actor
			if (allClosestUnits.Any())
			{
				foreach (var unit in allClosestUnits)
				{
					Actor targetActor = unit.a; 
					// 檢查目標是否是有效的 Actor，存活，且不是施法者本身
					if (targetActor == null || !targetActor.isAlive() || targetActor == selfActor)
					{
						continue; // 跳過無效單位和施法者本身
					}
					
					// 處理邏輯分為 敵方 (施加詛咒) 和 己方 (清除詛咒)
					
					if (targetActor.kingdom != selfActor.kingdom)
					{// 【A. 敵方單位】: 施加詛咒 / 移除祝福 (原邏輯)
						
						bool targetAffected = false; // 標記當前目標是否被影響
						
						// a. 施加 "cursed" 狀態效果
						if (!targetActor.hasStatus(cursedStatusID)) 
						{
							targetActor.addStatusEffect(cursedStatusID, cursedDuration); 
							targetAffected = true;
						}

						// b. 移除 "blessed" 特質
						if (targetActor.hasTrait(blessedTraitID))
						{
							targetActor.removeTrait(blessedTraitID);
							targetAffected = true;
						}

						if (targetAffected)
						{
							effectAppliedToAnyone = true; // 只要有一個目標被應用效果，就設為 true
						}
					}
					else // targetActor.kingdom == selfActor.kingdom
					{// 【B. 己方單位】: 清除詛咒 (新增邏輯)
						
						// ** 新增功能：如果己方單位處於詛咒狀態，則替他清除 **
						if (targetActor.hasStatus(cursedStatusID))
						{
							targetActor.finishStatusEffect(cursedStatusID); // 清除己方的詛咒狀態
							// 由於這是對己方的淨化行為，我們將其視為成功應用效果，但可能不計入 effectAppliedToAnyone 
							// 如果您希望這個淨化行為也計入 'effectAppliedToAnyone'，可以將 L103 設為 true
						}
					}
				}
			}
			
			// 返回是否成功對至少一個外部敵方目標應用了效果
			return effectAppliedToAnyone; 
		}
		public static bool Tamed_Undead_Effect(BaseSimObject pTarget, WorldTile pTile = null)
		{// 大咒法 不死者馴服 (骷髏 幽靈)
			// 1. 基本安全檢查
			if (pTarget == null || !pTarget.isActor() || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pTarget.a;
			// 2. 亞種檢查 (保持不變)
			if (selfActor.subspecies == null || selfActor.hasStatus("rebirth"))
			{
				return false;
			}
			// 3. 根據施法者類型設定參數 (範圍與特質)
			int maxRange = 0;
			string currentTraitID = "";  // 施法者自己擁有的馴服等級 ID
			string oppositeTraitID = ""; // 另一方持有的馴服等級 ID
			// 判斷施法者類型，並設定參數
			bool isNecromancer = selfActor.asset.id == "necromancer";
			if (isNecromancer)
			{
				// 死靈法師 (Necromancer) - 高階馴服
				maxRange = 5;
				currentTraitID = "undead_servant2";
				oppositeTraitID = "undead_servant";
			}
			else
			{
				// 非死靈法師 - 低階馴服
				maxRange = 2;
				currentTraitID = "undead_servant";
				oppositeTraitID = "undead_servant2";
			}
			if (maxRange == 0)
			{
				return false;
			}
			// 4. 獲取搜索地塊
			WorldTile tTile = pTile ?? pTarget.current_tile;
			// 5. 獲取範圍內的單位
			var allClosestUnits = Finder.getUnitsFromChunk(tTile, maxRange);
			bool effectApplied = false;
			if (allClosestUnits.Any())
			{
				int currentMasterID = 0; // 宣告整數變數，用於儲存僕從 Master ID
				foreach (var unit in allClosestUnits)
				{
					// 基礎安全檢查
					if (unit == null || !unit.isActor() || !unit.a.isAlive())
						continue;
					Actor targetActor = unit.a;
					
					// 排除自身
					if (targetActor == selfActor)
					{
						continue;
					}

					// 7. 物種檢查：目標必須是亡靈 (您精簡後的邏輯)
					string assetID = targetActor.asset.id;
					if (!(assetID == "ghost" || assetID == "skeleton" || assetID == "jumpy_skull"))
					{
						continue; // 不是目標亡靈單位，跳過
					}

					// ===== 核心互搶邏輯：從最高級別開始檢查 =====

					// A. 檢查是否已經是【高階僕從】 (undead_servant2)
					if (targetActor.hasTrait("undead_servant2"))
					{
						targetActor.data.get("master_id", out currentMasterID);
						
						// 條件 1: 如果施法者不是 Necromancer，無權處理，直接跳過。
						if (!isNecromancer)
						{
							continue;
						}
						
						// 條件 2: 施法者是 Necromancer，但 Master ID 屬於【別人】(非 0 且不等於自己 ID)，禁止搶奪。
						if (currentMasterID != 0 && currentMasterID != selfActor.data.id)
						{
							continue; // 它是別人的高級僕從，鎖定！
						}
					}
					// B. 檢查是否已經是【低階僕從】 (undead_servant)
					else if (targetActor.hasTrait("undead_servant"))
					{
						if (!isNecromancer)
						{
							continue;
						}
					}
					// C. 檢查目標是否已經持有「相同」等級的契約 (僅適用於野生的或沒有契約的僕從)
					if (targetActor.hasTrait(currentTraitID))
					{
						// 如果目標已經擁有當前施法者等級的契約，且不是高階僕從 (undead_servant2 已在 A 處理)
						// 則跳過，防止不必要的重複添加。
						continue;
					}
					
					// ===== 執行馴服 / 升級邏輯 =====

					// 8.1 移除較低階特質 (僅在死靈法師升級契約時需要)
					if (isNecromancer && targetActor.hasTrait(oppositeTraitID))
					{
						targetActor.removeTrait(oppositeTraitID); // 移除低階契約 (undead_servant)
					}
					// 8.2 變更歸屬
					targetActor.kingdom = selfActor.kingdom;
					// 移除軍籍
					if (targetActor.army != null)
					{
						targetActor.stopBeingWarrior(); 
					}
					if (selfActor.city != null) // 主人有城市
					{
						if (targetActor.city != null) // 目標有城市
						{
							// 情況 A: 目標城市友好 (保持原城市)
							if(targetActor.city.kingdom == selfActor.kingdom)
							{
								// 城市友好：保持原城市，但須確保它在清單中 (避免被系統清理)
								if (!targetActor.city.units.Contains(targetActor))
								{
									targetActor.city.units.Add(targetActor);
								}
							}
							else // 情況 B: 目標城市非己方 (遷入主人城市)
							{
								if (targetActor.isCityLeader() || targetActor.isKing())
								{
									TraitCityConversion02(targetActor, selfActor);
								}
								else
								{
									targetActor.city.units.Remove(targetActor);
									targetActor.city = selfActor.city;
									if (selfActor.city != null && !selfActor.city.units.Contains(targetActor))
									{
										selfActor.city.units.Add(targetActor);
									}
								}
							}
						}
						else // 情況 C: 目標無城市 (Nomad)，直接遷入主人城市
						{
							targetActor.city = selfActor.city;
							if (!selfActor.city.units.Contains(targetActor))
							{
								selfActor.city.units.Add(targetActor);
							}
						}
					}
					else // 情況 D: 如果主人沒有城市，僕從也必須清除城市歸屬
					{
						// 從舊城市清單移除 (重要)
						if (targetActor.city != null)
						{
							targetActor.city.units.Remove(targetActor);
						}
						targetActor.city = null;
					}
					// 8.3 添加特質與主人ID
					targetActor.setBestFriend(selfActor, false);
					targetActor.setLover(selfActor);
					if (selfActor.subspecies.hasTrait("prefrontal_cortex"))
					{// 施法者 有
						if (!targetActor.subspecies.hasTrait("prefrontal_cortex"))
						{// 且 目標 沒有
							targetActor.subspecies.addTrait("prefrontal_cortex");
						}
					}
					targetActor.removeTrait("slave");
					targetActor.finishStatusEffect("soul");
					targetActor.addTrait(currentTraitID); // 添加當前施法者等級的契約
					targetActor.data.health += 9999; // 恢復大量生命值
					targetActor.data.set("master_id", selfActor.data.id);		
					if (Randy.randomChance(0.50f)) 
					{
						if (!targetActor.hasTrait("pro_soldier")) 
						{
							targetActor.addTrait("pro_soldier");
						}
					}
					// 8.4 添加到清單
					if (!listOfTamedBeasts.ContainsKey(targetActor))
					{
						listOfTamedBeasts.Add(targetActor, selfActor);				
					} 
					// 如果已經在清單中，則更新其主人 (因為契約已經更新)
					else if (listOfTamedBeasts[targetActor] != selfActor)
					{
						listOfTamedBeasts[targetActor] = selfActor;
					}

					effectApplied = true;
				}
			}
			
			return effectApplied; // 只有當至少一個單位被馴服或更新時，才返回 true
		}
		public static bool Return(BaseSimObject pTarget, WorldTile pTile = null)
		{// 大咒法 歸籍
			// 1. 基本安全檢查與施法者定義
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pTarget.a; // 施法者
			
			// 2. 參數設定與施法者檢查
			int maxRange = 50;
			// 施法者必須有城市，才能提供歸屬地
			if (selfActor.subspecies == null || selfActor.city == null)
			{
				return false;
			}
			// 3. 獲取搜索地塊
			WorldTile tTile = pTile ?? pTarget.current_tile;
			// 4. 獲取範圍內的單位 (使用 maxRange)
			var allClosestUnits = Finder.getUnitsFromChunk(tTile, maxRange); // <-- 修正範圍
			bool effectApplied = false;
			if (allClosestUnits.Any())
			{
				foreach (var unit in allClosestUnits)
				{
					// 基礎安全檢查
					if (unit == null || !unit.isActor() || !unit.a.isAlive())
						continue;
					Actor targetActor = unit.a; // 正確宣告目標 Actor
					// 5. 條件檢查
					// 條件1: 雙方國家相同
					// 條件2: 目標沒有居住城市
					// 條件3: 目標種族是死靈法師
					if (targetActor.kingdom == selfActor.kingdom && 
						targetActor.asset.id == "necromancer" &&
						targetActor.city == null)
					{
						targetActor.city = selfActor.city;
						selfActor.city.units.Add(targetActor);
						
						if(selfActor.clan != null)
						{	// 只有當目標不在施法者氏族時才加入，避免重複
							if (targetActor.clan != selfActor.clan)
							{
								targetActor.clan = selfActor.clan;
								selfActor.clan.units.Add(targetActor);
							}
						}
						teleportToHomeCity(selfActor, targetActor, pTile);
						effectApplied = true;
					}
				}
			}
			return effectApplied; // 只有在至少一個單位被歸化時，才返回 true
		}
		public static bool ProtectOffspring(BaseSimObject pTarget, WorldTile pTile = null)
		{// 大咒法 搜蛋
			// 1. 基本安全檢查與施法者定義
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pTarget.a; // 施法者
			
			// 2. 參數設定與施法者檢查
			int maxRange = 20;
			if (selfActor.subspecies == null)
			{
				return false;
			}
			// 3. 獲取搜索地塊
			WorldTile tTile = pTile ?? pTarget.current_tile;
			// 4. 獲取範圍內的單位 (使用 maxRange)
			var allClosestUnits = Finder.getUnitsFromChunk(tTile, maxRange); // <-- 修正範圍
			bool effectApplied = false;
			if (allClosestUnits.Any())
			{
				foreach (var unit in allClosestUnits)
				{
					// 基礎安全檢查
					if (unit == null || !unit.isActor() || !unit.a.isAlive())
						continue;
					Actor targetActor = unit.a; // 正確宣告目標 Actor
					// 5. 條件檢查
					// 條件1: 雙方國家相同
					// 條件2: 目標沒有居住城市
					// 條件3: 目標種族是死靈法師
					if (targetActor.kingdom == selfActor.kingdom && 
						targetActor.asset.id == "necromancer" &&
						targetActor.city != null &&
						targetActor.hasStatus("egg"))
					{
						teleportToHomeCity(selfActor, targetActor, pTile);
						effectApplied = true;
					}
				}
			}
			return effectApplied; // 只有在至少一個單位被歸化時，才返回 true
		}
		public static bool addClan(BaseSimObject pTarget, WorldTile pTile = null)
		{// 大咒法 加入氏族
			// 1. 基本安全檢查與施法者定義
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pTarget.a; // 施法者
			
			// 2. 參數設定與施法者檢查
			int maxRange = 10;
			// 施法者必須有氏族，才能提供歸屬地
			if (selfActor.subspecies == null || selfActor.clan == null)
			{
				return false;
			}
			// 3. 獲取搜索地塊
			WorldTile tTile = pTile ?? pTarget.current_tile;
			// 4. 獲取範圍內的單位 (使用 maxRange)
			var allClosestUnits = Finder.getUnitsFromChunk(tTile, maxRange); // <-- 修正範圍
			bool effectApplied = false;
			if (allClosestUnits.Any())
			{
				foreach (var unit in allClosestUnits)
				{
					// 基礎安全檢查
					if (unit == null || !unit.isActor() || !unit.a.isAlive())
						continue;
					Actor targetActor = unit.a;
					if (targetActor.kingdom == selfActor.kingdom && 
						targetActor.clan == null && 
						targetActor.asset.id == "necromancer")
					{
						targetActor.clan = selfActor.clan;
						selfActor.clan.units.Add(targetActor);
						effectApplied = true;
					}
				}
			}
			return effectApplied; // 只有在至少一個單位被歸化時，才返回 true
		}
		public static bool UndeadCall(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 大咒法 不死者呼喚(攻擊)
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			if (pTarget == null || !pTarget.isActor() || !pTarget.a.isAlive())
			{
				// 如果 pTarget 無效，則使用 pSelf 作為召喚目標（例如：召喚在施法者腳下）
				pTarget = pSelf;
				return false;
			}
			
			Actor selfActor = pSelf.a; 
			Actor targetActor = pTarget.a; 
			
			// 確保召喚地塊 tTile 有效：優先使用傳入的 pTile，其次使用目標單位地塊
			WorldTile tTile = pTile ?? targetActor.current_tile; 
			
			// 2. 條件檢查
			if (selfActor.subspecies == null || selfActor.asset.id != "necromancer" || !selfActor.subspecies.hasTrait("prefrontal_cortex"))
			{
				return false; // 只有文明死靈法師才能發動
			}
			string attackCooldownStatus = "cdt_call00";
			float attackCooldownDuration = 180f;
			int iMaxSpawnCount = 5; // 每次施法召喚  個/組單位 
			if (selfActor.hasStatus("ex_undead_emperor"))
			{
				attackCooldownDuration = 30f;
				iMaxSpawnCount = 10;
			}
			if (selfActor.hasStatus(attackCooldownStatus))
			{
				Tamed_Undead_Effect(pSelf, tTile);
				return false; // 檢查冷卻時間
			}

			bool spawnedAny = false;

			// 4. 重複執行召喚動作
			for (int i = 0; i < iMaxSpawnCount; i++)
			{
				// 召喚骷髏
				if (ActionLibrary.castSpawnSkeleton(pSelf, pTarget, tTile)) 
				{
					spawnedAny = true;
				}
			}
			// 5. 馴服與冷卻
			// 嘗試馴服範圍內所有合適的亡靈（包括剛召喚出來的）
			if (spawnedAny || Tamed_Undead_Effect(pSelf, tTile))
			{
				// 如果有召喚成功，或馴服成功，則套用冷卻時間
				selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration); // 3 分鐘冷卻 (180秒)
				return true;
			}
			
			return false; // 召喚與馴服都失敗
		}
		public static bool UndeadCall2(BaseSimObject pTarget, WorldTile pTile = null)
		{// 大咒法 不死者呼喚
			// 1. 基本安全檢查：目標是否有效且存活
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor targetActor = pTarget.a;
			WorldTile tTile = pTile ?? targetActor.current_tile; 

			if (targetActor.subspecies == null || targetActor.asset.id != "necromancer")
			{
				return false;
			}
			string attackCooldownStatus = "cdt_call01";
			float attackCooldownDuration = 900f;
			int iMaxSpawnCount = 1; // 每次施法召喚  個/組單位 
			if (targetActor.hasStatus(attackCooldownStatus))
			{
				Tamed_Undead_Effect(pTarget, tTile);
				return false; // 檢查冷卻時間
			}
			bool spawnedAny = false;
			// 4. 重複執行召喚動作
			for (int i = 0; i < iMaxSpawnCount; i++)
			{
				// 召喚幽靈
				if (SpawnGhost(pTarget, pTarget, tTile))
				{
					spawnedAny = true;
				}
				// 召換骷髏
				if (SpawnSkeleton(pTarget, pTarget, tTile)) // 使用 pSelf 作為施法者，tTile 作為地塊
				{
					spawnedAny = true;
				}
			}
			// 5. 馴服與冷卻
			// 嘗試馴服範圍內所有合適的亡靈（包括剛召喚出來的）
			if (spawnedAny || Tamed_Undead_Effect(pTarget, tTile))
			{
				// 如果有召喚成功，或馴服成功，則套用冷卻時間
				targetActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration); // 3 分鐘冷卻 (180秒)
				return true;
			}
			return true; // 表示效果成功施加
		}
		public static bool SpawnGhost(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 大咒法 生成幽靈效果 代碼
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a;
///--複製起始端
			// 下文中的_XX可依照生成需求去進行修改
			// 設定生成生物
			string unitToSummon_XX = "ghost";
			// 呼喚與施法者同種生物的輸入法為 selfActor.asset.id;
			// 呼喚特定生物的方法為 "生物ID";
			int NumberOfMobsSpawn_XX = 1;
			// 設定每次觸發要生成的單位數量
			for (int i = 0; i < NumberOfMobsSpawn_XX; i++)
			{	// 生物_XX 生成迴圈
				var act_XX = World.world.units.createNewUnit(unitToSummon_XX, pTile);
				act_XX.setKingdom(selfActor.kingdom);									//加入施法者王國
				act_XX.setCity(selfActor.city);											//加入施法者城市
				act_XX.setBestFriend(selfActor, false);
				act_XX.setLover(selfActor);
				act_XX.addTrait("undead_servant2");									//加入施法者城市 
				act_XX.data.set("master_id", selfActor.a.data.id);						//紀錄主人ID
				act_XX.goTo(selfActor.current_tile);									//到主人身邊
				if (!listOfTamedBeasts.ContainsKey(act_XX))
				{listOfTamedBeasts.Add(act_XX, selfActor.a);}							//添加到清單
			}
///--複製末尾端
			return true; // 只要成功生成，就返回 true
		}
		public static bool SpawnSkeleton(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 大咒法 生成骷髏效果 代碼
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a;
///--複製起始端
			// 下文中的_XX可依照生成需求去進行修改
			// 設定生成生物
			string unitToSummon_XX = "skeleton";
			// 呼喚與施法者同種生物的輸入法為 selfActor.asset.id;
			// 呼喚特定生物的方法為 "生物ID";
			int NumberOfMobsSpawn_XX = 1;
			// 設定每次觸發要生成的單位數量
			for (int i = 0; i < NumberOfMobsSpawn_XX; i++)
			{	// 生物_XX 生成迴圈
				var act_XX = World.world.units.createNewUnit(unitToSummon_XX, pTile);
				act_XX.setKingdom(selfActor.kingdom);									//加入施法者王國
				act_XX.setCity(selfActor.city);											//加入施法者城市
				act_XX.setBestFriend(selfActor, false);
				act_XX.setLover(selfActor);
				act_XX.addTrait("undead_servant2");									//加入施法者城市 
				act_XX.data.set("master_id", selfActor.a.data.id);						//紀錄主人ID
				act_XX.goTo(selfActor.current_tile);									//到主人身邊
				if (!listOfTamedBeasts.ContainsKey(act_XX))
				{listOfTamedBeasts.Add(act_XX, selfActor.a);}							//添加到清單
			}
///--複製末尾端
			return true; // 只要成功生成，就返回 true
		}
		public static bool Transformation_Undead(BaseSimObject pTarget, WorldTile pTile = null)
		{// 大咒法 蘇生
			// 基礎安全檢查：確保目標單位及其 Actor 組件存在
			if (pTarget == null || pTarget.a == null)
			{
				return false;
			}
			Actor originalActor = pTarget.a; // 將原始 Actor 儲存起來以備後用

			if (originalActor.asset.id == "necromancer")
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
			originalActor.removeTrait("infected");
			originalActor.removeTrait("mush_spores");
			originalActor.removeTrait("tumor_infection");
			originalActor.removeTrait("plague");
			originalActor.removeTrait("death_mark");
			originalActor.removeTrait("skin_burns");
			originalActor.removeTrait("crippled");
			originalActor.removeTrait("eyepatch");
			originalActor.removeTrait("madness");
			originalActor.removeTrait("undead_servant");
			originalActor.removeTrait("undead_servant2");
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
			act.data.name = originalActor.getName();
			act.data.favorite = originalActor.data.favorite;
			act.data.health += 99999; 		// 恢復大量生命值
			act.data.age_overgrowth += 2 ; 	// 恢復大量生命值

			// 10. 為新單位施加臨時狀態效果
			act.addStatusEffect("invincible", 5); 
			act.addStatusEffect("antibody", 5); 
			act.addStatusEffect("rebirth", 5); 
			act.finishStatusEffect("egg");								//移除狀態
			act.finishStatusEffect("uprooting");						//移除狀態
			if (originalActor.subspecies.hasTrait("prefrontal_cortex"))
			{// 如果施法者有才添加
				act.subspecies.addTrait("prefrontal_cortex");
				act.subspecies.addTrait("advanced_hippocampus");
				act.subspecies.addTrait("wernicke_area");
				act.subspecies.addTrait("amygdala");
			}

			// 11. 更多酷炫的生成效果
			EffectsLibrary.spawnExplosionWave(pTile.posV3, 1f, 1f); 
			World.world.applyForceOnTile(pTile, 3, 1.5f, pForceOut: true, 0, null, pByWho: act); 
			return true; // 表示效果成功執行
		}
		public static bool TraittAddRemove1(BaseSimObject pTarget, WorldTile pTile)
		{// 大咒法 專用移除機制
			// 基礎安全檢查：確保目標單位及其 Actor 組件存在
			if (pTarget == null || pTarget.a == null)
			{
				return false; // 無效目標，直接返回
			}
			Actor targetActor = pTarget.a;
			// 定義要移除的一般特質列表
			if (targetActor.subspecies.hasTrait("prefrontal_cortex") || targetActor.asset.id == "necromancer")
			{
				return false; // 無效目標，直接返回
			}
			targetActor.removeTrait("evillaw_tgc");
			return true; // 函數執行完成
		}
		public static bool Blessing_of_the_Undead_King(BaseSimObject pTarget, WorldTile pTile)
		{// 大咒法 不死王的祝福
			// 1. 確保 pTarget 是有效的 Actor 並且存活
			if (pTarget == null || !(pTarget is Actor kingActor) || !kingActor.isAlive())
			{
				return false;
			}
			// 2. 檢查單位是否為國王
			if (!kingActor.isKing() || kingActor.asset.id != "necromancer") 
			{
				return false; // 如果不是國王，則不執行後續效果
			}
			// 3. 獲取國王所屬的王國
			Kingdom kingdom = kingActor.kingdom;
			if (kingdom == null)
			{
				return false; // 國王沒有所屬王國，不執行
			}
			// 4. 遍歷王國下的所有村莊 (City 類在 WorldBox 中通常代表村莊/城市)
			if (kingdom.cities != null)
			{
				foreach (City city in kingdom.cities)
				{
					if (city == null) continue;

					// 5. 獲取村莊的領袖
					Actor leader = city.leader; // 假設 leader 是一個 Actor 屬性

					if (leader != null && leader.isAlive() && leader != kingActor) // 確保領袖存在、存活且不是國王本人
					{
						// 6. 給村莊領袖添加特質
						// !!! 重要：請將 "YourNewLeaderTraitID" 替換為您想要添加的實際特質 ID !!!
						string traitToAdd = "evillaw_tgc"; 
						if (!leader.hasTrait(traitToAdd)) // 避免重複添加
						{
							leader.addTrait(traitToAdd);
							// 您可以在這裡添加一些調試輸出，例如：
							// Debug.Log($"Added trait '{traitToAdd}' to village leader: {leader.name}");
						}
					}
				}
			}
			return true; // 效果成功執行
		}
		public static bool UndeadEmperor(BaseSimObject pTarget, WorldTile pTile = null)
		{// 大咒法 (戴冠式)
			// 安全檢查：確保目標存在且是活著的 Actor
			if (pTarget?.a == null || !pTarget.isActor() || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor targetActor = pTarget.a;
			Kingdom kingdom = targetActor.kingdom;
			string EmperorStatusID = "ex_undead_emperor";
			float statusDuration = 3600f; // 60分鐘的狀態
			
			// === 修正：檢查【世界】上是否已存在不死帝王 ===
			bool isUndeadEmperorExists = false;
			
			// 遍歷【世界】上的所有單位：使用 getSimpleList()
			var allWorldUnits = World.world.units.getSimpleList(); 
			
			foreach (Actor worldUnit in allWorldUnits)
			{
				// 跳過無效單位和施法者自己
				if (worldUnit == null || worldUnit == targetActor)
				{
					continue;
				}
				
				// 檢查是否有「非自己」的單位擁有 "extraordinary_authority" 特質
				if (worldUnit.hasTrait("extraordinary_authority"))
				{
					isUndeadEmperorExists = true;
					break; // 找到一個，立即退出迴圈
				}
			}
			
			// 如果世界上已存在不死帝王，則不賦予新狀態
			if (isUndeadEmperorExists)
			{
				return false;
			}
			
			// 賦予 undead_emperor 狀態
			targetActor.addTrait("extraordinary_authority");
			kingdom.data.name = $"Undead Kingdom";
			kingdom.capital.data.name = $"Throne of the Undead";
			targetActor.data.name = $"Undead Emperor";
			WorldLog.logNewKing(kingdom);
			targetActor.addStatusEffect(EmperorStatusID, statusDuration);
			UndeadCrownGet(pTarget, pTile);
			return true;
		}
		public static bool UndeadCrownGet(BaseSimObject pTarget, WorldTile pTile = null)
		{// 大咒法 裝備獲得
			// 定義所需的狀態ID常量
			const string UndeadEmperorStatus = "ex_undead_emperor"; //魔王狀態
			const string HelmetID = "undead_crown"; // 物品ID
			// 1. 基本安全檢查：目標是否有效且存活
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor targetActor = pTarget.a;
			// ****** 新增檢查：單位是否擁有 '指定狀態' 狀態 ******
			if (!targetActor.hasStatus(UndeadEmperorStatus))
			{
				return false; // 如果沒有此狀態，則不給予裝備
			}
			// ************************************************************
			// 獲取目標的武器槽位
			var weaponSlot = targetActor.equipment.getSlot(EquipmentType.Helmet);
			// 檢查武器槽位是否已被佔用，且其中持有的道具ID是否為 "指定武器"
			if (weaponSlot != null && weaponSlot.getItem() != null && weaponSlot.getItem().asset.id == HelmetID)
			{
				// 額外檢查：如果武器沒有修飾符，在這裡補上
				Item existingWeapon = weaponSlot.getItem();
				if (!existingWeapon.hasMod("eternal") || !existingWeapon.hasMod("cursed"))
				{
					//existingWeapon.addMod("eternal");
					//existingWeapon.addMod("cursed");
				}
				return true;
			}
			// 如果單位沒有持有 "指定武器" 且擁有指定狀態，則繼續進行裝備流程
			// 獲取 "指定武器" 物品資產
			var weaponAsset = AssetManager.items.get(HelmetID);
			if (weaponAsset == null)
			{
				return false; // 無法找到物品資產，返回失敗
			}
			// 生成 "指定武器" 物品實例
			var newItem = World.world.items.generateItem(pItemAsset: weaponAsset);
			// =======================================================
			// === 核心修正：強制添加修飾符 ===
			if (newItem != null)
			{
				//newItem.addMod("eternal");
				//newItem.addMod("cursed");
				newItem.addMod("power5");
				newItem.addMod("truth5");
				newItem.addMod("protection5");
				newItem.addMod("speed5");
				newItem.addMod("balance5");
				newItem.addMod("health5");
				newItem.addMod("finesse5");
				newItem.addMod("mastery5");
				newItem.addMod("knowledge5");
				newItem.addMod("sharpness5");
			}
			// =======================================================
			// 將 "指定武器" 裝備到目標的武器槽位
			weaponSlot.setItem(newItem, targetActor);
			// 更新目標的統計數據，以反映新裝備帶來的變化
			targetActor.setStatsDirty();
			targetActor.data.health += 99999;
			return true; // 表示效果成功施加
		}
		public static bool UndeadEmperor2(BaseSimObject pSelf, WorldTile pTile = null)
		{// 大咒法 非常大權
			// 1. 基本安全检查：确保 pSelf 及其 Actor 组件存在且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			Kingdom kingdom = selfActor.kingdom;
			// 1. 對施法者自身添加狀態
			selfActor.addStatusEffect("ex_undead_emperor", 3600f);
			kingdom.data.name = $"Undead Kingdom";
			kingdom.capital.data.name = $"Throne of the Undead";
			// 2. 對同勢力單位添加狀態
			//var allClosestUnits = Finder.getUnitsFromChunk(pTile, 60);
			//if (allClosestUnits.Any())
			//{
			//	foreach (var unit in allClosestUnits)
			//	{
			//		// 確保 unit 是有效的 Actor、存活、是同勢力單位且不是施法者自己
			//		if (unit?.a != null && unit.a.isAlive() && unit.a != selfActor && unit.a.kingdom == selfActor.kingdom)
			//		{
			//			// 對同伴直接添加所有增益狀態
			//			//unit.a.addStatusEffect("powerup", 60f);
			//		}
			//	}
			//}
			return false; // 返回 false，表示不阻止其他效果鏈
		}

		public static bool UndeadServantAtk(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 大咒法 不死族從者 攻擊調用
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			UndeadServant(pSelf, pTile);
			return true;
		}
		public static bool UndeadServant(BaseSimObject pTarget, WorldTile pTile = null)
		{// 大咒法 不死族從者(其他類型)
			if (pTarget?.a == null || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor beast = pTarget.a;
			Actor master = null; // 【外部作用域宣告】

			//if (beast == master)
			//{
			//	beast.removeTrait("undead_servant");
			//	beast.removeTrait("undead_servant2");
			//}
			// 獲取野獸的種類 ID
			string beastTypeID = beast.asset.id;
			float followChance = 0.00f; // 默認跟隨機率
			
			// 根據生物種類 和特質 設定不同的跟隨機率
			if (beastTypeID == "ghost" || beastTypeID == "skeleton" || beastTypeID == "jumpy_skull")
			{
				followChance = 0.005f; //
			}
			else if (beast.hasTrait("pro_soldier"))
			{
				followChance = 0.95f;
			}
			if (listOfTamedBeasts.ContainsKey(beast))
			{// 如果野獸已經在清單中，則檢查主人並決定是否跟隨
				master = listOfTamedBeasts[beast]; 
				if (master != null && master.isAlive())
				{
					const float MasterRange1 = 10.0f; // 保護半徑
					const float MasterRange2 = 5.0f; // 保護半徑
					float masterHealthPercent = (float)master.getHealth() / master.getMaxHealth();
					 // 計算僕從與主人的距離
					float distance1 = UnityEngine.Vector2.Distance(beast.current_position, master.current_position);
					float distance2 = UnityEngine.Vector2.Distance(beast.current_position, master.current_position);
					if (masterHealthPercent < 0.05f)
					{// 當主人的血量低於 5% 就調用效果
						if (master.city != null)
						{
							// 【方案 A: 文明生物】傳送回城
							teleportToHomeCity(beast, master, pTile);
						}
						else if (distance1> MasterRange1)
						{
							// 【方案 B: 非文明/野人】僕從強制保護主人逃離
							// 1. 賦予主人極高速度狀態，使其能快速逃離戰場
							master.addStatusEffect("dash", 10f); 
							master.addStatusEffect("caffeinated", 10f); // 組合極速狀態
							// 2. 僕從也加速並追隨主人
							beast.addStatusEffect("dash", 5f);
							beast.addStatusEffect("inspired", 5f);
							beast.addStatusEffect("caffeinated", 5f);
							beast.goTo(master.current_tile);
						}
					}
					else if (masterHealthPercent < 0.25f)
					{// 當主人的血量低於 25%
						if (distance1> MasterRange1)
						{
							beast.addStatusEffect("dash", 5f);
							beast.addStatusEffect("caffeinated", 5f);
							beast.goTo(master.current_tile);
							return true;
						}
					}
					else if (masterHealthPercent < 0.50f)
					{// 當主人的血量低於 50%
						if (distance1> MasterRange1)
						{
							beast.addStatusEffect("dash", 5f);
							beast.addStatusEffect("inspired", 5f);
							beast.goTo(master.current_tile);
							return true; 
						}
					}
					else if (masterHealthPercent < 0.75f)
					{// 當主人的血量低於 75%
						if (distance1> MasterRange1)
						{
							beast.addStatusEffect("dash", 5f);
							beast.goTo(master.current_tile);
							return true;
						}
					}
					if (distance2 > MasterRange2)
					{//一般模式
						if (Randy.randomChance(followChance))
						{
							beast.goTo(master.current_tile);
							return true;
						}
					}
				}
			}
			else
			{// 否則，從自訂數據中重新建立主人關係
				if (beast.data?.custom_data_long != null &&
					beast.data.custom_data_long.TryGetValue("master_id", out long masterId))
				{
					master = World.world.units.get(masterId);
					if (master != null)
					{
						if (listOfTamedBeasts == null)
						{
							return false; // 安全檢查
						}
						// 成功從 ID 恢復，將其加入清單
						listOfTamedBeasts[beast] = master;
						// 將野獸的王國同步給主人
						if (master.kingdom != null)
							beast.kingdom = master.kingdom;
					}
				}
			}
			if (master == null || !master.isAlive())
			{// 主人死亡之後的後續處裡
				// 確保 master 已經被賦值為清單中的值，如果之前沒有成功從 custom_data 恢復
				if (listOfTamedBeasts.ContainsKey(beast))
				{
					master = listOfTamedBeasts[beast];
				}

				if (master == null || !master.isAlive())
				{
					// 清理奴隸關係
					beast.removeTrait("undead_servant");
					beast.removeTrait("undead_servant2");
					beast.finishStatusEffect("undead_lord01");
					beast.finishStatusEffect("undead_captain01");
					beast.finishStatusEffect("undead_warrior01");
					// 進行安全檢查，避免對不存在的 key 進行 Remove
					if (listOfTamedBeasts.ContainsKey(beast))
					{
						listOfTamedBeasts.Remove(beast);
					}
					return false;
				}
			}
			if (master.city != null)
			{// 恢復能力 與 武裝賦予
				const float HEALTH_TRIGGER_PERCENTAGE = 0.99f;
				const float HEALTH_RESTORE_PERCENTAGE = 0.01f;
				const float MIN_BEAST_HEALTH_PERCENTAGE = 0.05f;
				
				const float MANA_TRIGGER_PERCENTAGE = 0.99f;
				const float MANA_RESTORE_PERCENTAGE = 0.01f;
				
				const float STAMINA_TRIGGER_PERCENTAGE = 0.99f;
				const float STAMINA_RESTORE_PERCENTAGE = 0.01f;
				
				//const float NUTRITION_TRIGGER_PERCENTAGE = 0.99f;
				//const int NUTRITION_RESTORE_AMOUNT = 1;
				//const int MAX_NUTRITION_VALUE = 100;
				float masterMaxHealth = master.getMaxHealth();
				float beastMaxHealth = beast.getMaxHealth(); // 獲取僕從最大血量
				float healthToRestore = masterMaxHealth * HEALTH_RESTORE_PERCENTAGE;
				int healthCost = Mathf.RoundToInt(healthToRestore); // 治療所需的實際血量成本
				float minBeastHealthThreshold = beastMaxHealth * MIN_BEAST_HEALTH_PERCENTAGE;
				// 檢查 1: 主人是否需要治療
				// 檢查 2: 僕從支付成本後，是否仍高於最低血量閾值
				if (master.data.health / masterMaxHealth < HEALTH_TRIGGER_PERCENTAGE &&
					beast.data.health - healthCost >= minBeastHealthThreshold) // <-- 增加的關鍵檢查
				{
					master.restoreHealth(healthCost);
					beast.data.health -= healthCost;
					// 由於上面已經檢查過，這裡的 < 0 檢查現在僅作為最終安全保障
					if (beast.data.health < 0) beast.data.health = 0;
				}
				float masterMaxMana = master.getMaxMana();
				float manaToRestore = masterMaxMana * MANA_RESTORE_PERCENTAGE;
				if (master.data.mana / masterMaxMana < MANA_TRIGGER_PERCENTAGE)
				{
					//master.data.mana += Mathf.RoundToInt(manaToRestore);
					master.data.mana = (int)masterMaxMana; 
					if (master.data.mana > masterMaxMana)
					{
						master.data.mana += Mathf.RoundToInt(manaToRestore);
					}
					beast.data.mana -= Mathf.RoundToInt(manaToRestore);
					if (beast.data.mana < 0) beast.data.mana = 0;
				}
				float masterMaxStamina = master.getMaxStamina();
				float staminaToRestore = masterMaxStamina * STAMINA_RESTORE_PERCENTAGE;
				if (master.data.stamina / masterMaxStamina < STAMINA_TRIGGER_PERCENTAGE)
				{
					//master.data.stamina += Mathf.RoundToInt(staminaToRestore);
					master.data.stamina = (int)masterMaxStamina;
					if (master.data.stamina > masterMaxStamina)
					{
						master.data.stamina += Mathf.RoundToInt(staminaToRestore);
					}
					beast.data.stamina -= Mathf.RoundToInt(staminaToRestore);
					if (beast.data.stamina < 0) beast.data.stamina = 0;
				}
				
			/*	float nutritionMaster = master.data.nutrition;
				if (nutritionMaster / MAX_NUTRITION_VALUE < NUTRITION_TRIGGER_PERCENTAGE)
				{
					master.data.nutrition = Mathf.Min(MAX_NUTRITION_VALUE, master.data.nutrition + NUTRITION_RESTORE_AMOUNT);
					beast.data.nutrition -= NUTRITION_RESTORE_AMOUNT;
					if (beast.data.nutrition < 0) beast.data.nutrition = 0;
				}*/

				//武器賦予邏輯
				var weaponSlot = master.equipment.getSlot(EquipmentType.Weapon);
				if (weaponSlot != null)
				{
					var currentItem = weaponSlot.getItem();
					if (currentItem == null) 
					{
						// 創建並裝備法杖
						var weaponAsset = AssetManager.items.get("necromancer_staff");	
						if (weaponAsset != null)
						{
							var weaponInstance = World.world.items.generateItem(pItemAsset: weaponAsset);
							
							if (weaponInstance != null)
							{
								weaponInstance.addMod("power5");
								weaponInstance.addMod("truth5");
								weaponInstance.addMod("protection5");
								weaponInstance.addMod("speed5");
								weaponInstance.addMod("balance5");
								weaponInstance.addMod("health5");
								weaponInstance.addMod("finesse5");
								weaponInstance.addMod("mastery5");
								weaponInstance.addMod("knowledge5");
								weaponInstance.addMod("sharpness5");
								// 由於 currentItem == null，這裡可以直接設置
								weaponSlot.setItem(weaponInstance, master);
							}
						}
					}
					// 否則，如果主人已經持有武器 (currentItem != null)，則不做任何操作，保留該武器。
				}
			}
			// === 雙向同步：戶籍與關係 ===
			if (beast.city != master.city)
			{// 城市同步：確保奴隸和主人屬於同一個城市
				// 情況 1: 主人有城市 -> 奴隸跟隨
				if (beast.city == null)
				{
					if (master.city != null)
					{
						beast.city = master.city;
						master.city.units.Add(beast);
					}
				}
				// 情況 2: 逆向寄生 主人沒有城市但奴隸有
				else if (beast.city != null)
				{
					if (master.city == null)
					{
						master.city = beast.city;
						master.kingdom = beast.kingdom;
						beast.city.units.Add(master);
					}
				}
			}
			if (!master.hasTrait("madness") &&
			!master.hasTrait("desire_harp") &&
			!master.hasTrait("desire_alien_mold") &&
			!master.hasTrait("desire_computer") &&
			!master.hasTrait("desire_golden_egg"))
			{// 當主人持有異常精神時 不執行下方區塊
				if (beast.kingdom != master.kingdom)
				{// 國家同步：確保奴隸和主人屬於同一個國家
					if (beast.kingdom != null)
					{
						beast.kingdom.units.Remove(beast); // 從舊王國移除
					}
					beast.kingdom = master.kingdom;
					if (master.kingdom != null)
					{
						// 【致命錯誤修正點】: 必須檢查 Contains，否則會不斷 Add 導致人口狂飆
						if (!master.kingdom.units.Contains(beast))
						{
							master.kingdom.units.Add(beast); // 加入新王國清單
						}
					}
				}
				if (beast.city != null && beast.city.kingdom != master.kingdom)
				{// 當奴隸城市不為空,奴隸所屬城市王國不歸屬王國時
					if (master.isKing())
					{// 觸發城市移轉效果
						TraitCityConversion01(master, beast);//由主人方發動
						TraitCityConversion02(beast, master);//由奴隸方發動
					}
					else if (!master.isKing())
					{// 離開城市
						beast.city.units.Remove(beast);
					}
				}
			}
			if (beast.clan != master.clan)
			{// 氏族同步：確保奴隸和主人有條件的屬於同一個氏族
				// 情況 1: 逆向寄生 - 僕從是城市領袖，且主人沒有氏族但奴隸有
				if (master.clan == null && beast.clan != null && beast.isCityLeader())
				{
					// 只有僕從是領袖，才有權力將主人拉入自己的氏族
					master.clan = beast.clan;
					// 【安全檢查】：確保不重複 ADD
					if (!beast.clan.units.Contains(master))
					{
						beast.clan.units.Add(master);
					}
				}
				// 情況 2: 正常同步 - 奴隸是領袖，且奴隸沒有氏族，但主人有
				else if (beast.clan == null && master.clan != null && beast.isCityLeader())
				{
					// 只有僕從是領袖，才能進入主人的氏族
					beast.clan = master.clan;
					// 【安全檢查】：確保不重複 ADD
					if (!master.clan.units.Contains(beast))
					{
						master.clan.units.Add(beast);
					}
				}
				// 情況 3: 雙方都有不同的氏族 (執行傷害效果)
				else if (master.clan != null && beast.clan != null)
				{
					Clan currentClan = beast.clan;
					if(currentClan.getChief() == beast)
					{
						beast.addTrait("evillaw_tgc");
						TraitAddDamage(beast, pTile);
					}
					else
					{
						beast.clan.units.Remove(beast);
					}
				}
			}
			if (beast.isKing() && master.kingdom != null)
			{// 職位讓渡 (國王)
				// 1. 移除奴隸的國王身分
				Kingdom slaveKingdom = beast.kingdom;
				if (slaveKingdom != null && slaveKingdom.king == beast)
				{
					beast.setProfession(UnitProfession.Unit); // 將奴隸職業重置為普通單位
					beast.removeTrait("pro_king"); // 移除國王特質
					slaveKingdom.king = null; // 清空王國的國王引用
				}
				// 2. 將主人設為國王
				Kingdom masterKingdom = master.kingdom;
				if (masterKingdom != null)
				{
					if (!master.subspecies.hasTrait("advanced_hippocampus"))
					{
						master.subspecies.addTrait("advanced_hippocampus");
					}
					// 如果主人是城市的領袖，先移除領袖身分
					if (master.isCityLeader())
					{
						master.city.removeLeader();
					}
					masterKingdom.king = master; // 設定王國的國王
					master.setProfession(UnitProfession.King); // 設定職業為國王
					WorldLog.logNewKing(masterKingdom);
					// 可選: 觸發國王就職的視覺效果
					master.startShake();
					master.startColorEffect();
				}
			}
			if (beast.isCityLeader() && !master.isKing())
			{// 職位讓渡 (領主)
				City city = beast.city;
				
				// **核心修正點**：主人讓位邏輯
				// 只有當奴隸是當前城市領袖時，我們才介入職位變更
				if (city.leader == beast)
				{
					// 情況 A: 主人是普通單位 (平民/Unit)
					if (master.getProfession() == UnitProfession.Unit) 
					{
						if (!master.subspecies.hasTrait("advanced_hippocampus"))
						{
							master.subspecies.addTrait("advanced_hippocampus");
						}
						city.removeLeader(); 
						city.setLeader(master, true); 
						master.startShake();
						master.startColorEffect();
					}
					else 
					{
						
					}
				}
			}
			if (beast.isWarrior())
			{// 職位讓渡 (軍隊長)
				if(beast.is_army_captain && beast.army != null)
				{
					// 1. 安全檢查：確保奴隸和主人在同一個城市且城市存在
					if (beast.city == null || beast.city != master.city)
					{
						// 如果城市不同步，不進行軍隊長讓渡
						return true; 
					}
					City city = beast.city;
					Army cityArmy = city.army;
					// 2. 核心檢查：確認奴隸確實是該軍隊的隊長
					if (cityArmy.getCaptain() == beast)
					{	// 3. 讓渡邏輯
						if (master.getProfession() == UnitProfession.Unit && (!master.isKing() || !master.isCityLeader()))
						{	// 如果主人是普通單位或戰士，則可以升任隊長
							master.setProfession(UnitProfession.Warrior);
							beast.setProfession(UnitProfession.Unit); 
							cityArmy.setCaptain(master, true);
							// 視覺效果 (可選)
							master.startShake();
							master.startColorEffect();
						}
						else 
						{
							master.stopBeingWarrior();
						}
					}
				}
			}
			if (beast.clan == master.clan && beast.clan != null)
			{// 職位讓渡 (族長)
				Clan currentClan = beast.clan;
				// 檢查奴隸是否為該氏族的現任首領
				if (currentClan.getChief() == beast)
				{
					currentClan.setChief(master);
				}
			}
			if (beast.isCityLeader())
			{// 職位能力 (領主)
				City city = beast.city;
				if (city.leader == beast)
				{
					beast.addStatusEffect("undead_lord01", 600f);
				}
				else
				{
					beast.finishStatusEffect("undead_lord01");
				}
			}
			if (beast.isWarrior() && beast.city != null)
			{// 職位能力 (戰士)
				if(beast.is_army_captain && beast.army != null)
				{
					beast.addStatusEffect("undead_captain01", 600f);
					beast.finishStatusEffect("undead_warrior01");
				}
				else
				{
					beast.addStatusEffect("undead_warrior01", 600f);
					beast.finishStatusEffect("undead_captain01");
				}
			}
			// ---特質 與 狀態處理 ---
			if (master.subspecies.hasTrait("prefrontal_cortex"))
			{// 主人是文明生物 奴隸單位才會發揮以下效果
				if (!beast.subspecies.hasTrait("prefrontal_cortex"))
				{
					beast.subspecies.addTrait("prefrontal_cortex");
				}
				if (master.isKing())
				{// 特殊效果調用 戴冠式
					if(master.asset.id == "necromancer")
					{
						if(master.kingdom.cities.Count >= 5)
						{
							Kingdom kingdom = master.kingdom;
							if(!master.hasTrait("extraordinary_authority"))
							{	// 假設 UndeadEmperor 等函式存在
								UndeadEmperor(master, pTile);
								UndeadCrownGet(master, pTile);
								Items01Actions.UndeadCrownEffect(master, pTile);
								Items01Actions.addFavoriteWeaponUE(master, pTile);
								beast.addStatusEffect("festive_spirit", 60f);
								master.finishStatusEffect("sleeping");
							}
						}
					}
				}
				if (beast.isCityLeader()) 
				{// 職位能力 (領主)
					// 只有當它是 CityLeader 時，才賦予/維持狀態，否則清除。
					beast.addStatusEffect("undead_lord01", 600f);
				}
				else
				{// 如果不再是領主，清除相關狀態
					beast.finishStatusEffect("undead_lord01");
				}
				if (beast.isWarrior() && beast.city != null)
				{// 職位能力 (戰士/隊長) - 建議使用 else if 確保互斥
					if(beast.is_army_captain && beast.army != null)
					{
						beast.addStatusEffect("undead_captain01", 600f);
						beast.finishStatusEffect("undead_warrior01"); // 確保不是戰士
					}
					else if (!beast.is_army_captain) // 確保不是隊長
					{
						beast.addStatusEffect("undead_warrior01", 600f);
						beast.finishStatusEffect("undead_captain01"); // 確保不是隊長
					}
				}
				else
				{// 如果不再是戰士，清除相關狀態
					beast.finishStatusEffect("undead_warrior01");
					beast.finishStatusEffect("undead_captain01");
				}
				if (master.kingdom.cities.Count < 5)
				{// 國家城市低於 10 消除睡眠
					master.finishStatusEffect("sleeping");
				}
				if (!master.hasTrait("evillaw_tantrum"))
				{// 如果主人沒有 憤怒法 特質
					if (master.hasStatus("tantrum") || master.hasStatus("angry"))
					{// 清除主人負面情緒
						master.finishStatusEffect("tantrum");
						master.finishStatusEffect("angry");
					}
				}
				if (master.hasStatus("wrath_demon_king"))
				{// 如果主人持有 憤怒魔王 狀態
					beast.subspecies.addTrait("heat_resistance");
					master.finishStatusEffect("tantrum");
					master.finishStatusEffect("angry");
				}
				if (!master.hasTrait("evillaw_starvation"))
				{// 如果主人沒有 惡食法 特質
					if (master.data.nutrition < 99)
					{// 清除主人負面情緒
						master.data.nutrition += 1;
					}
				}
				if (master.hasStatus("gluttony_demon_king"))
				{// 如果主人持有 暴食魔王 狀態
					if (master.data.nutrition < 99)
					{// 清除主人負面情緒
						master.data.nutrition += 1;
					}
				}
				if (!master.hasTrait("evillaw_devour"))
				{// 如果主人沒有 吞噬法 特質
					if (master.data.happiness < 0)
					{// 幸福度維持
						master.data.happiness = 50; // 75%
					}
				}
				if (master.hasStatus("envy_demon_king"))
				{// 如果主人持有 嫉妒魔王 狀態
					if (master.data.happiness < 0)
					{// 增加主人幸福度
						master.data.happiness = 1; //50%
					}
				}
				if (master.hasStatus("sloth_demon_king"))
				{// 如果主人持有 怠惰魔王
					beast.subspecies.addTrait("cold_resistance");
				}
				if (master.hasStatus("ex_undead_emperor"))
				{// 如果主人持有 不死帝王
					beast.addStatusEffect("darkblessing", 120f);
					beast.removeTrait("infected");
					beast.removeTrait("mush_spores");
					beast.removeTrait("tumor_infection");
					beast.removeTrait("plague");
					beast.removeTrait("crippled");
					beast.removeTrait("skin_burns");
					beast.removeTrait("eyepatch");
					beast.removeTrait("death_mark");
				}
				if (master.hasStatus("darkblessing"))
				{// 如果主人持有 黑暗賜福
					beast.addStatusEffect("darkblessing", 120f);
					beast.removeTrait("infected");
					beast.removeTrait("mush_spores");
					beast.removeTrait("tumor_infection");
					beast.removeTrait("plague");
					beast.removeTrait("crippled");
					beast.removeTrait("skin_burns");
					beast.removeTrait("eyepatch");
					beast.removeTrait("death_mark");
				}
				if (master.hasStatus("stunned"))
				{// 清除主人負面狀態
					master.finishStatusEffect("stunned");
				}
				if (master.hasStatus("recovery_plot"))
				{// 清除主人負面狀態
					master.finishStatusEffect("recovery_plot");
				}
				if (master.hasStatus("recovery_spell"))
				{// 清除主人負面狀態
					master.finishStatusEffect("recovery_spell");
				}
				if (master.hasStatus("recovery_combat_action"))
				{// 清除主人負面狀態
					master.finishStatusEffect("recovery_combat_action");
				}
				if (master.hasStatus("recovery_social"))
				{// 清除主人負面狀態
					master.finishStatusEffect("recovery_social");
				}
				if (master.hasStatus("confused"))
				{// 清除主人負面狀態
					master.finishStatusEffect("confused");
				}
				if (master.hasStatus("slowness"))
				{// 清除主人負面狀態
					master.finishStatusEffect("slowness");
				}
				if (master.hasStatus("poisoned"))
				{// 清除主人負面狀態
					master.finishStatusEffect("poisoned");
				}
				if (master.hasStatus("cough"))
				{// 清除主人負面狀態
					master.finishStatusEffect("cough");
				}
				if (master.hasStatus("ash_fever"))
				{// 清除主人負面狀態
					master.finishStatusEffect("ash_fever");
				}
				if (master.hasStatus("burning"))
				{// 清除主人負面狀態
					master.finishStatusEffect("burning");
				}
				if (master.hasStatus("frozen"))
				{// 清除主人負面狀態
					master.finishStatusEffect("frozen");
				}
				if (beast.hasStatus("angry"))
				{// 自身憤怒情緒處理
					beast.finishStatusEffect("angry"); 
					beast.addStatusEffect("stunned", 0.001f);
				}
				if (beast.data.money > 0)
				{// 納貢 金錢
					master.data.money += 1; // 設定為 1
					beast.data.money -= 1; // 設定為 1
				}
				if (beast.data.loot > 0)
				{// 納貢 戰利品
					master.data.loot += 1; // 設定為 1
					beast.data.loot -= 1; // 設定為 1
				}
				if (beast.data.level > 0)
				{// 納貢 經驗值
					master.addExperience(1);
				}
				if(master.hasTrait("infected")){master.removeTrait("infected");}
				if(master.hasTrait("mush_spores")){master.removeTrait("mush_spores");}
				if(master.hasTrait("tumor_infection")){master.removeTrait("tumor_infection");}
				if(master.hasTrait("plague")){master.removeTrait("plague");}
				if(master.hasTrait("crippled")){master.removeTrait("crippled");}
				if(master.hasTrait("skin_burns")){master.removeTrait("skin_burns");}
				if(master.hasTrait("eyepatch")){master.removeTrait("eyepatch");}
				if(master.hasTrait("death_mark")){master.removeTrait("death_mark");}
				if(master.hasTrait("evillaw_tgc"))
				{// 精神異常特質處理
					beast.removeTrait("madness");
					beast.removeTrait("desire_harp");
					beast.removeTrait("desire_alien_mold");
					beast.removeTrait("desire_computer");
					beast.removeTrait("desire_golden_egg");
					master.removeTrait("madness");
					master.removeTrait("desire_harp");
					master.removeTrait("desire_alien_mold");
					master.removeTrait("desire_computer");
					master.removeTrait("desire_golden_egg");
				}
			}
			return true; // 如果執行到這裡，表示同步邏輯已完成
		}
		public static bool UndeadServant_NAtk(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 大咒法 不死族從者 攻擊調用
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			UndeadServant_N(pSelf, pTile);
			return true;
		}
		public static bool UndeadServant_N(BaseSimObject pTarget, WorldTile pTile = null)
		{// 大咒法 不死族從者(死靈法師)
			if (pTarget?.a == null || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor beast = pTarget.a;
			Actor master = null; // 【外部作用域宣告】

			// 獲取野獸的種類 ID
			string beastTypeID = beast.asset.id;
			float followChance = 0.00f; // 默認跟隨機率
			if (beast.hasTrait("pro_soldier"))
			{
				followChance = 0.10f;
			}
			if (listOfTamedBeasts.ContainsKey(beast))
			{// 如果野獸已經在清單中，則檢查主人並決定是否跟隨
				master = listOfTamedBeasts[beast]; 
				if (master != null && master.isAlive())
				{
					const float MasterRange1 = 10.0f; // 保護半徑
					const float MasterRange2 = 5.0f; // 保護半徑
					float masterHealthPercent = (float)master.getHealth() / master.getMaxHealth();
					 // 計算僕從與主人的距離
					float distance1 = UnityEngine.Vector2.Distance(beast.current_position, master.current_position);
					float distance2 = UnityEngine.Vector2.Distance(beast.current_position, master.current_position);
					if (masterHealthPercent < 0.05f)
					{// 當主人的血量低於 5% 就調用效果
						if (master.city != null)
						{
							// 【方案 A: 文明生物】傳送回城
							teleportToHomeCity(beast, master, pTile);
							return true;
						}
					}
					else if (masterHealthPercent < 0.25f)
					{// 當主人的血量低於 25%
						if (distance1> MasterRange1)
						{
							beast.addStatusEffect("dash", 5f);
							beast.addStatusEffect("caffeinated", 5f);
							beast.goTo(master.current_tile);
							return true;
						}
					}
					else if (masterHealthPercent < 0.50f)
					{// 當主人的血量低於 50%
						if (distance1> MasterRange1)
						{
							beast.addStatusEffect("dash", 5f);
							beast.addStatusEffect("inspired", 5f);
							beast.goTo(master.current_tile);
							return true; 
						}
					}
					else if (masterHealthPercent < 0.75f)
					{// 當主人的血量低於 75%
						if (distance1> MasterRange1)
						{
							beast.addStatusEffect("dash", 5f);
							beast.goTo(master.current_tile);
							return true;
						}
					}
					if (distance2 > MasterRange2)
					{//一般模式
						if (Randy.randomChance(followChance))
						{
							beast.goTo(master.current_tile);
							return true;
						}
					}
				}
			}
			else
			{// 否則，從自訂數據中重新建立主人關係
				if (beast.data?.custom_data_long != null &&
					beast.data.custom_data_long.TryGetValue("master_id", out long masterId))
				{
					master = World.world.units.get(masterId);
					if (master != null)
					{
						if (listOfTamedBeasts == null)
						{
							return false; // 安全檢查
						}
						// 成功從 ID 恢復，將其加入清單
						listOfTamedBeasts[beast] = master;
						// 將野獸的王國同步給主人
						if (master.kingdom != null)
							beast.kingdom = master.kingdom;
					}
				}
			}
			if (master == null || !master.isAlive())
			{// 主人死亡之後的後續處裡
				// 確保 master 已經被賦值為清單中的值，如果之前沒有成功從 custom_data 恢復
				if (listOfTamedBeasts.ContainsKey(beast))
				{
					master = listOfTamedBeasts[beast];
				}

				if (master == null || !master.isAlive())
				{
					// 清理奴隸關係
					beast.removeTrait("undead_servant3");
					beast.finishStatusEffect("undead_lord");
					beast.finishStatusEffect("undead_captain");
					beast.finishStatusEffect("undead_warrior");
					// 進行安全檢查，避免對不存在的 key 進行 Remove
					if (listOfTamedBeasts.ContainsKey(beast))
					{
						listOfTamedBeasts.Remove(beast);
					}
					return false;
				}
			}
			if (master.city != null)
			{// 恢復能力 與 武裝賦予
				const float HEALTH_TRIGGER_PERCENTAGE = 0.99f;
				const float HEALTH_RESTORE_PERCENTAGE = 0.01f;
				const float MIN_BEAST_HEALTH_PERCENTAGE = 0.05f;
				const float MANA_TRIGGER_PERCENTAGE = 0.99f;
				const float MANA_RESTORE_PERCENTAGE = 0.01f;
				const float STAMINA_TRIGGER_PERCENTAGE = 0.99f;
				const float STAMINA_RESTORE_PERCENTAGE = 0.01f;
			//	const float NUTRITION_TRIGGER_PERCENTAGE = 0.99f;
			//	const int NUTRITION_RESTORE_AMOUNT = 1;
			//	const int MAX_NUTRITION_VALUE = 100;
				float masterMaxHealth = master.getMaxHealth();
				float beastMaxHealth = beast.getMaxHealth(); // 獲取僕從最大血量
				float healthToRestore = masterMaxHealth * HEALTH_RESTORE_PERCENTAGE;
				int healthCost = Mathf.RoundToInt(healthToRestore); // 治療所需的實際血量成本
				// 計算僕從必須維持的最低血量 (1% 的 Max Health)
				float minBeastHealthThreshold = beastMaxHealth * MIN_BEAST_HEALTH_PERCENTAGE;
				// 檢查 1: 主人是否需要治療
				// 檢查 2: 僕從支付成本後，是否仍高於最低血量閾值
				if (master.data.health / masterMaxHealth < HEALTH_TRIGGER_PERCENTAGE &&
					beast.data.health - healthCost >= minBeastHealthThreshold) // <-- 增加的關鍵檢查
				{
					master.restoreHealth(healthCost);
					beast.data.health -= healthCost;
					// 由於上面已經檢查過，這裡的 < 0 檢查現在僅作為最終安全保障
					if (beast.data.health < 0) beast.data.health = 0;
				}
				float masterMaxMana = master.getMaxMana();
				float manaToRestore = masterMaxMana * MANA_RESTORE_PERCENTAGE;
				if (master.data.mana / masterMaxMana < MANA_TRIGGER_PERCENTAGE)
				{
					//master.data.mana += Mathf.RoundToInt(manaToRestore);
					master.data.mana = (int)masterMaxMana; 
					if (master.data.mana > masterMaxMana)
					{
						master.data.mana += Mathf.RoundToInt(manaToRestore);
					}
					beast.data.mana -= Mathf.RoundToInt(manaToRestore);
					if (beast.data.mana < 0) beast.data.mana = 0;
				}
				float masterMaxStamina = master.getMaxStamina();
				float staminaToRestore = masterMaxStamina * STAMINA_RESTORE_PERCENTAGE;
				if (master.data.stamina / masterMaxStamina < STAMINA_TRIGGER_PERCENTAGE)
				{
					//master.data.stamina += Mathf.RoundToInt(staminaToRestore);
					master.data.stamina = (int)masterMaxStamina;
					if (master.data.stamina > masterMaxStamina)
					{
						master.data.stamina += Mathf.RoundToInt(staminaToRestore);
					}
					beast.data.stamina -= Mathf.RoundToInt(staminaToRestore);
					if (beast.data.stamina < 0) beast.data.stamina = 0;
				}
			/*	float nutritionMaster = master.data.nutrition;
				if (nutritionMaster / MAX_NUTRITION_VALUE < NUTRITION_TRIGGER_PERCENTAGE)
				{
					master.data.nutrition = Mathf.Min(MAX_NUTRITION_VALUE, master.data.nutrition + NUTRITION_RESTORE_AMOUNT);
					beast.data.nutrition -= NUTRITION_RESTORE_AMOUNT;
					if (beast.data.nutrition < 0) beast.data.nutrition = 0;
				}*/

				//武器賦予邏輯
				var weaponSlot = master.equipment.getSlot(EquipmentType.Weapon);
				if (weaponSlot != null)
				{
					var currentItem = weaponSlot.getItem();
					if (currentItem == null) 
					{
						// 創建並裝備法杖
						var weaponAsset = AssetManager.items.get("necromancer_staff");	
						if (weaponAsset != null)
						{
							var weaponInstance = World.world.items.generateItem(pItemAsset: weaponAsset);
							
							if (weaponInstance != null)
							{
								weaponInstance.addMod("power5");
								weaponInstance.addMod("truth5");
								weaponInstance.addMod("protection5");
								weaponInstance.addMod("speed5");
								weaponInstance.addMod("balance5");
								weaponInstance.addMod("health5");
								weaponInstance.addMod("finesse5");
								weaponInstance.addMod("mastery5");
								weaponInstance.addMod("knowledge5");
								weaponInstance.addMod("sharpness5");
								// 由於 currentItem == null，這裡可以直接設置
								weaponSlot.setItem(weaponInstance, master);
							}
						}
					}
					// 否則，如果主人已經持有武器 (currentItem != null)，則不做任何操作，保留該武器。
				}
			}
			// === 雙向同步：戶籍與關係 ===
			if (beast.city != master.city)
			{// 城市同步：確保奴隸和主人屬於同一個城市
				// 情況 1: 主人有城市 -> 奴隸跟隨
				if (beast.city == null)
				{
					if (master.city != null)
					{
						beast.city = master.city;
						master.city.units.Add(beast);
					}
				}
				else if (beast.city != null)
				{	// 情況 2: 逆向寄生 主人沒有城市但奴隸有
					//(用於遊戲開始單位都沒有國家且奴隸先於主人創建國家)
					if (master.city == null)
					{
						master.city = beast.city;
						master.kingdom = beast.kingdom;
						beast.city.units.Add(master);
					}
				}
				else if (beast.city != null && master.city != null)
				{
				}
			}
			if (!master.hasTrait("madness") &&
			!master.hasTrait("desire_harp") &&
			!master.hasTrait("desire_alien_mold") &&
			!master.hasTrait("desire_computer") &&
			!master.hasTrait("desire_golden_egg"))
			{// 當主人持有異常精神時 不執行下方區塊
				if (beast.kingdom != master.kingdom)
				{// 國家同步：確保奴隸和主人屬於同一個國家
					if (beast.kingdom != null)
					{
						beast.kingdom.units.Remove(beast); // 從舊王國移除
					}
					beast.kingdom = master.kingdom;
					if (master.kingdom != null)
					{
						// 【致命錯誤修正點】: 必須檢查 Contains，否則會不斷 Add 導致人口狂飆
						if (!master.kingdom.units.Contains(beast))
						{
							master.kingdom.units.Add(beast); // 加入新王國清單
						}
					}
				}
				if (beast.city != null && beast.city.kingdom != master.kingdom)
				{// 當奴隸城市不為空,奴隸所屬城市王國不歸屬王國時
					if (master.isKing())
					{// 觸發城市移轉效果
						TraitCityConversion01(master, beast);//由主人方發動
						TraitCityConversion02(beast, master);//由奴隸方發動
					}
					else if (!master.isKing())
					{// 離開城市
						beast.city.units.Remove(beast);
					}
				}
			}
			if (beast.isKing() && master.kingdom != null)
			{// 職位讓渡 (國王)
				// 1. 移除奴隸的國王身分
				Kingdom slaveKingdom = beast.kingdom;
				if (slaveKingdom != null && slaveKingdom.king == beast)
				{
					beast.setProfession(UnitProfession.Unit); // 將奴隸職業重置為普通單位
					beast.removeTrait("pro_king"); // 移除國王特質
					slaveKingdom.king = null; // 清空王國的國王引用
				}
				// 2. 將主人設為國王
				Kingdom masterKingdom = master.kingdom;
				if (masterKingdom != null)
				{
					if (!master.subspecies.hasTrait("advanced_hippocampus"))
					{
						master.subspecies.addTrait("advanced_hippocampus");
					}
					// 如果主人是城市的領袖，先移除領袖身分
					if (master.isCityLeader())
					{
						master.city.removeLeader();
					}
					masterKingdom.king = master; // 設定王國的國王
					master.setProfession(UnitProfession.King); // 設定職業為國王
					WorldLog.logNewKing(masterKingdom);
					// 可選: 觸發國王就職的視覺效果
					master.startShake();
					master.startColorEffect();
				}
			}
			if (beast.clan == master.clan && beast.clan != null)
			{// 職位讓渡 (族長)
				Clan currentClan = beast.clan;
				// 檢查奴隸是否為該氏族的現任首領
				if (currentClan.getChief() == beast)
				{
					currentClan.setChief(master);
				}
			}
			if (master.subspecies.hasTrait("prefrontal_cortex"))
			{// 主人是文明生物 奴隸單位才會發揮以下效果
				if (!beast.subspecies.hasTrait("prefrontal_cortex"))
				{// 添加亞種特質
					beast.subspecies.addTrait("prefrontal_cortex");
				}
				//else 
				//{// 如果主人沒有此特質 無論僕從是否有，都將其移除。
				//	beast.subspecies.removeTrait("prefrontal_cortex");
				//}
				if (beast.isCityLeader()) 
				{// 職位能力 (領主)
					// 只有當它是 CityLeader 時，才賦予/維持狀態，否則清除。
					beast.addStatusEffect("undead_lord", 600f);
				}
				else
				{// 如果不再是領主，清除相關狀態
					beast.finishStatusEffect("undead_lord");
				}
				if (beast.isWarrior() && beast.city != null)
				{// 職位能力 (戰士/隊長) - 建議使用 else if 確保互斥
					if(beast.is_army_captain && beast.army != null)
					{
						beast.addStatusEffect("undead_captain", 600f);
						beast.finishStatusEffect("undead_warrior"); // 確保不是戰士
					}
					else if (!beast.is_army_captain) // 確保不是隊長
					{
						beast.addStatusEffect("undead_warrior", 600f);
						beast.finishStatusEffect("undead_captain"); // 確保不是隊長
					}
				}
				else
				{// 如果不再是戰士，清除相關狀態
					beast.finishStatusEffect("undead_warrior");
					beast.finishStatusEffect("undead_captain");
				}
				if (master.kingdom.cities.Count < 6)
				{// 國家城市低於 10 消除睡眠
					master.finishStatusEffect("sleeping");
				}
				if (!master.hasTrait("evillaw_tantrum"))
				{// 如果主人沒有 憤怒法 特質
					if (master.hasStatus("tantrum") || master.hasStatus("angry"))
					{// 清除主人負面情緒
						master.finishStatusEffect("tantrum");
						master.finishStatusEffect("angry");
					}
				}
				if (master.hasStatus("wrath_demon_king"))
				{// 如果主人持有 憤怒魔王 狀態
					beast.subspecies.addTrait("heat_resistance");
					master.finishStatusEffect("tantrum");
					master.finishStatusEffect("angry");
				}
				if (!master.hasTrait("evillaw_starvation"))
				{// 如果主人沒有 惡食法 特質
					if (master.data.nutrition < 99)
					{// 清除主人負面情緒
						master.data.nutrition += 1;
					}
				}
				if (master.hasStatus("gluttony_demon_king"))
				{// 如果主人持有 暴食魔王 狀態
					if (master.data.nutrition < 99)
					{// 清除主人負面情緒
						master.data.nutrition += 1;
					}
				}
				if (!master.hasTrait("evillaw_devour"))
				{// 如果主人沒有 吞噬法 特質
					if (master.data.happiness < 0)
					{// 幸福度維持
						master.data.happiness = 50; // 75%
					}
				}
				if (master.hasStatus("envy_demon_king"))
				{// 如果主人持有 嫉妒魔王 狀態
					if (master.data.happiness < 0)
					{// 增加主人幸福度
						master.data.happiness = 1; //50%
					}
				}
				if (master.hasStatus("sloth_demon_king"))
				{// 如果主人持有 怠惰魔王
					beast.subspecies.addTrait("cold_resistance");
				}
				if (master.hasStatus("ex_undead_emperor"))
				{// 如果主人持有 不死帝王
					beast.addStatusEffect("darkblessing", 120f);
					beast.removeTrait("infected");
					beast.removeTrait("mush_spores");
					beast.removeTrait("tumor_infection");
					beast.removeTrait("plague");
					beast.removeTrait("crippled");
					beast.removeTrait("skin_burns");
					beast.removeTrait("eyepatch");
					beast.removeTrait("death_mark");
				}
				if (master.hasStatus("stunned"))
				{// 清除主人負面狀態
					master.finishStatusEffect("stunned");
				}
				if (master.hasStatus("recovery_plot"))
				{// 清除主人負面狀態
					master.finishStatusEffect("recovery_plot");
				}
				if (master.hasStatus("recovery_spell"))
				{// 清除主人負面狀態
					master.finishStatusEffect("recovery_spell");
				}
				if (master.hasStatus("recovery_combat_action"))
				{// 清除主人負面狀態
					master.finishStatusEffect("recovery_combat_action");
				}
				if (master.hasStatus("recovery_social"))
				{// 清除主人負面狀態
					master.finishStatusEffect("recovery_social");
				}
				if (master.hasStatus("confused"))
				{// 清除主人負面狀態
					master.finishStatusEffect("confused");
				}
				if (master.hasStatus("slowness"))
				{// 清除主人負面狀態
					master.finishStatusEffect("slowness");
				}
				if (master.hasStatus("poisoned"))
				{// 清除主人負面狀態
					master.finishStatusEffect("poisoned");
				}
				if (master.hasStatus("cough"))
				{// 清除主人負面狀態
					master.finishStatusEffect("cough");
				}
				if (master.hasStatus("ash_fever"))
				{// 清除主人負面狀態
					master.finishStatusEffect("ash_fever");
				}
				if (master.hasStatus("burning"))
				{// 清除主人負面狀態
					master.finishStatusEffect("burning");
				}
				if (master.hasStatus("frozen"))
				{// 清除主人負面狀態
					master.finishStatusEffect("frozen");
				}
				if (beast.hasStatus("angry"))
				{// 自身憤怒情緒處理
					beast.finishStatusEffect("angry"); 
					beast.addStatusEffect("stunned", 0.001f);
				}
				if (beast.data.money > 0)
				{// 納貢 金錢
					master.data.money += 1; // 設定為 1
					beast.data.money -= 1; // 設定為 1
				}
				if (beast.data.loot > 0)
				{// 納貢 戰利品
					master.data.loot += 1; // 設定為 1
					beast.data.loot -= 1; // 設定為 1
				}
				if (beast.data.level > 0)
				{// 納貢 經驗值
					master.addExperience(1);
				}
				if(master.hasTrait("infected")){master.removeTrait("infected");}
				if(master.hasTrait("mush_spores")){master.removeTrait("mush_spores");}
				if(master.hasTrait("tumor_infection")){master.removeTrait("tumor_infection");}
				if(master.hasTrait("plague")){master.removeTrait("plague");}
				if(master.hasTrait("crippled")){master.removeTrait("crippled");}
				if(master.hasTrait("skin_burns")){master.removeTrait("skin_burns");}
				if(master.hasTrait("eyepatch")){master.removeTrait("eyepatch");}
				if(master.hasTrait("death_mark")){master.removeTrait("death_mark");}
				if(master.hasTrait("evillaw_tgc"))
				{// 精神異常特質處理
					beast.removeTrait("madness");
					beast.removeTrait("desire_harp");
					beast.removeTrait("desire_alien_mold");
					beast.removeTrait("desire_computer");
					beast.removeTrait("desire_golden_egg");
					master.removeTrait("madness");
					master.removeTrait("desire_harp");
					master.removeTrait("desire_alien_mold");
					master.removeTrait("desire_computer");
					master.removeTrait("desire_golden_egg");
				}
			}
			return true; // 如果執行到這裡，表示同步邏輯已完成
		}
		public static bool Recovery(BaseSimObject pSelf, WorldTile pTile = null)
		{// 大咒法 從屬單位自動恢復
			// 1. 基本安全检查：确保 pSelf 及其 Actor 组件存在且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			// 1. 對施法者自身添加狀態
			if (!selfActor.hasStatus("darkblessing"))
			{
				return false;
			}
			Traits01Actions.Health_recovery(pSelf, pTile);
			Traits01Actions.Mana_recovery(pSelf, pTile);
			Traits01Actions.Stamina_recovery(pSelf, pTile);
			return true;
		}

		public static bool Devour_Effect1(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 吞噬法 敵機Buff 攻擊 及 遭受傷害時
			// 確保施法者有效且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive())
			{
				return false;
			}

			Actor selfActor = pSelf.a; // 施法者

			// ** 新增檢查：如果施法者自身有 "eating" 狀態，則不觸發效果 **
			if (selfActor.hasStatus("eating"))
			{
				// Debug.Log($"Devour_Effect1: {selfActor.name} 擁有 'eating' 狀態，跳過對敵方狀態的清除。");
				return false; // 不執行後續邏輯
			}

			// 檢查目標是否有效且存活
			if (pTarget != null)
			{
				Actor targetActor = pTarget.a;
				if (targetActor != null && targetActor.isAlive())
				{
					// 需要清除的狀態效果列表
					string[] potentialEffectsToRemove = { "antibody", "shield", "powerup", "caffeinated", "enchanted", "rage", "motivated", "spell_boost", "inspired" };
					
					// 獲取目標當前擁有的且在 potentialEffectsToRemove 列表中的狀態
					List<string> currentEffects = potentialEffectsToRemove
						.Where(effectName => targetActor.hasStatus(effectName))
						.ToList();

					if (currentEffects.Count > 0)
					{
						// 隨機選擇一個要清除的狀態
						// 使用靜態的 _random 實例
						int randomIndex = _random.Next(currentEffects.Count);
						string effectToRemove = currentEffects[randomIndex];

						// 清除選定的狀態
						targetActor.finishStatusEffect(effectToRemove);
						// Debug.Log($"Devour_Effect1: 清除了 {targetActor.name} 的狀態: {effectToRemove}");

						// ** 只有在成功清除狀態後才恢復生命值 **
						float restorePercentage = 0.10f; // 3%
						int healthToRestore = Mathf.RoundToInt(selfActor.getMaxHealth() * restorePercentage);
						selfActor.data.health = Mathf.Min(selfActor.getMaxHealth(), selfActor.data.health + healthToRestore);
						// Debug.Log($"Devour_Effect1: 為 {selfActor.name} 恢復了 {healthToRestore} 點生命值 ({restorePercentage * 100}%). 當前生命值: {selfActor.data.health}");

						// ** 只有在成功清除狀態後才施加「Eating」狀態 **
						string eatingStatusID = "eating";
						float eatingStatusDuration = 2f; // 2 秒
						selfActor.addStatusEffect(eatingStatusID, eatingStatusDuration);
						// Debug.Log($"Devour_Effect1: {selfActor.name} 獲得了 '{eatingStatusID}' 狀態，持續 {eatingStatusDuration} 秒。");

						return true; // 表示成功清除了一個狀態並產生了效果
					}
				}
			}
			Devour_Effect2(pSelf, pTile);
			return false; // 目標無效或沒有可清除的狀態
		}
		public static bool RapidHappiness(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 吞噬法 幸福扣減 受到傷害時
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
			if (selfActor.hasStatus("envy_demon_king") || selfActor.data.happiness < -99)
			{
				// 如果 pSelf 不是一個 Actor，則無法繼續
				return false;
			}
			const int Using_happiness = 2;
			selfActor.data.happiness -= Using_happiness;
			return true;
		}
		public static bool RapidHappiness2(BaseSimObject pTarget, WorldTile pTile = null)
		{// 吞噬法 幸福扣減 (帶有冷卻狀態)
			if (pTarget is Actor actor)
			{
				// 設定幸福減少的量
				int Using_happiness = 4; // 減少的幸福值 2%
				// 設定檢查的狀態 ID 和其持續時間
				string cooldownStatusID = "happiness_leak"; // 用於幸福減少的冷卻狀態ID
				float cooldownDuration = 4f;			 // 冷卻時間 (秒), 例如 5 秒
				if (actor.hasStatus(cooldownStatusID) || actor.hasStatus("envy_demon_king") || actor.data.happiness < -99)
				{
					return false; // 不減少幸福
				}
				actor.data.happiness -= Using_happiness;
				actor.addStatusEffect(cooldownStatusID, cooldownDuration);
				// Debug.Log($"{actor.name} 獲得了 '{cooldownStatusID}' 狀態，持續 {cooldownDuration} 秒。");
				return true; // 幸福減少並賦予狀態
			}
			return false; // 目標不是 Actor
		}
		public static bool Devour_Effect2(BaseSimObject pSelf, WorldTile pTile)
		{// 吞噬法 自機Debuff
			// 注意：這個技能是清除自身負面狀態，所以 pTarget 在這裡不會被使用。
			// 為了符合 WorldAction 的簽名，我們保留了 pTile 參數。

			// 確保施法者有效且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a; // 施法者
			// ** 新增檢查：如果施法者自身有 "eating" 狀態，則不觸發效果 **
			if (selfActor.hasStatus("eating"))
			{
				// Debug.Log($"Devour_Effect2: {selfActor.name} 擁有 'eating' 狀態，跳過清除自身負面狀態。");
				return false; // 不執行後續邏輯
			}
			string[] negativeEffectsToRemove = { "slowness", "cough", "ash_fever", "frozen", "burning", "poisoned", "stunned", "sleeping", "spell_silence", "drowning", "confused" };
			int effectsRemovedCount = 0;
			// 遍歷並嘗試清除施法者自身的負面狀態
			foreach (string effectName in negativeEffectsToRemove)
			{
				if (selfActor.hasStatus(effectName))
				{
					selfActor.finishStatusEffect(effectName);
					effectsRemovedCount++;
					// Debug.Log($"Devour_Effect2: 清除了 {selfActor.name} 自身的狀態: {effectName}");
				}
			}
			// ** 只有在成功清除狀態後才恢復生命值並施加「Eating」狀態 **
			if (effectsRemovedCount > 0)
			{
				// 恢復施法者的生命值 (百分比恢復)
				float restorePercentagePerEffect = 0.10f; // 每消除一個狀態恢復 3%
				float totalRestorePercentage = restorePercentagePerEffect * effectsRemovedCount;
				
				int healthToRestore = Mathf.RoundToInt(selfActor.getMaxHealth() * totalRestorePercentage);
				selfActor.data.health = Mathf.Min(selfActor.getMaxHealth(), selfActor.data.health + healthToRestore);
				// Debug.Log($"Devour_Effect2: 為 {selfActor.name} 恢復了 {healthToRestore} 點生命值 ({totalRestorePercentage * 100}%). 當前生命值: {selfActor.data.health}");

				// 施法者獲得「Eating」狀態
				string eatingStatusID = "eating";
				float eatingStatusDuration = 2f; // 3 秒
				selfActor.addStatusEffect(eatingStatusID, eatingStatusDuration);
				// Debug.Log($"Devour_Effect2: {selfActor.name} 獲得了 '{eatingStatusID}' 狀態，持續 {eatingStatusDuration} 秒。");

				return true; // 表示成功清除了至少一個負面狀態並產生了效果
			}
			
			return false; // 持有者無效或沒有可清除的負面狀態
		}
		public static bool applyEnvyStatus(BaseSimObject pTarget, WorldTile pTile = null)
		{// 吞噬法 魔王狀態
			// 安全檢查：確保目標存在且是活著的 Actor
			if (pTarget?.a == null || !pTarget.isActor() || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor targetActor = pTarget.a;
			string envyStatusID = "envy_demon_king";
			float statusDuration = 3600f; // 60分鐘的狀態
			// === 新增：檢查王國內是否已存在其他魔王 ===
			Kingdom currentKingdom = targetActor.kingdom;
			if (currentKingdom != null)
			{
				bool isDemonKingExists = false;
				foreach (Actor kingdomUnit in currentKingdom.units)
				{
					// 跳過自己
					if (kingdomUnit == null || kingdomUnit == targetActor)
					{
						continue;
					}
					// 優先檢查是否有 'other666' 特質
					if (kingdomUnit.hasTrait("hope")||
						kingdomUnit.hasTrait("other6661")||
						kingdomUnit.hasTrait("other6662")||
						kingdomUnit.hasTrait("other6663")||
						kingdomUnit.hasTrait("other6664")||
						kingdomUnit.hasTrait("other6665")||
						kingdomUnit.hasTrait("other6666")||
						kingdomUnit.hasTrait("other6667")||
						kingdomUnit.hasTrait("other6668")||
						kingdomUnit.hasTrait("other6669"))
					{
						isDemonKingExists = true;
						break;
					}
					// 檢查單位是否擁有任一魔王狀態
					foreach (string demonKingStatusID in SevenDemonKingStatus_DemonKing)
					{
						if (kingdomUnit.hasStatus(demonKingStatusID))
						{
							isDemonKingExists = true;
							break;
						}
					}
					if (isDemonKingExists)
					{
						break;
					}
				}
				// 如果王國中已存在魔王，則不賦予新狀態
				if (isDemonKingExists)
				{
					return false;
				}
			}
			// === 檢查結束 ===
			// 檢查條件：當目標的幸福值小於或等於 -99 時
			if (targetActor.data.happiness <= -99)
			{
				// 賦予 envy_demon_king 狀態
				targetActor.addStatusEffect(envyStatusID, statusDuration);
				EvilPoniardGet(pTarget, pTile);
				return true;
			}
			return false;
		}
		public static bool EvilPoniardGet(BaseSimObject pTarget, WorldTile pTile = null)
		{// 吞噬法 武器給予
			// 定義所需的狀態ID常量
			const string DemonKingStatus = "envy_demon_king"; //魔王狀態
			const string WeapontID = "evil_poniard"; // 物品ID
			// 1. 基本安全檢查：目標是否有效且存活
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor targetActor = pTarget.a;
			// ****** 新增檢查：單位是否擁有 '指定魔王狀態' 狀態 ******
			if (!targetActor.hasStatus(DemonKingStatus))
			{
				return false; // 如果沒有此狀態，則不給予裝備
			}
			// ************************************************************
			// 獲取目標的武器槽位
			var weaponSlot = targetActor.equipment.getSlot(EquipmentType.Weapon);
			// 檢查武器槽位是否已被佔用，且其中持有的道具ID是否為 "指定武器"
			if (weaponSlot != null && weaponSlot.getItem() != null && weaponSlot.getItem().asset.id == WeapontID)
			{
				// 額外檢查：如果武器沒有修飾符，在這裡補上
				Item existingWeapon = weaponSlot.getItem();
				/*if (!existingWeapon.hasMod("eternal"))
				{
					existingWeapon.addMod("eternal");
					existingWeapon.addMod("cursed");
				}*/
				return true;
			}
			// 如果單位沒有持有 "指定武器" 且擁有指定狀態，則繼續進行裝備流程
			// 獲取 "指定武器" 物品資產
			var weaponAsset = AssetManager.items.get(WeapontID);
			if (weaponAsset == null)
			{
				return false; // 無法找到物品資產，返回失敗
			}
			// 生成 "指定武器" 物品實例
			var newItem = World.world.items.generateItem(pItemAsset: weaponAsset);
			// =======================================================
			// === 核心修正：強制添加修飾符 ===
			if (newItem != null)
			{
				//newItem.addMod("eternal");
				newItem.addMod("power5");
				newItem.addMod("truth5");
				newItem.addMod("protection5");
				newItem.addMod("speed5");
				newItem.addMod("balance5");
				newItem.addMod("health5");
				newItem.addMod("finesse5");
				newItem.addMod("mastery5");
				newItem.addMod("knowledge5");
				newItem.addMod("sharpness5");
			}
			// =======================================================
			// 將 "指定武器" 裝備到目標的武器槽位
			weaponSlot.setItem(newItem, targetActor);
			// 更新目標的統計數據，以反映新裝備帶來的變化
			targetActor.setStatsDirty();
			targetActor.data.health += 99999;
			return true; // 表示效果成功施加
		}

public static bool Extend_Effect0(BaseSimObject pSelf, WorldTile pTile)
{//御時法 封鎖0(原型)
	// 1. 基本安全檢查：確保施法者 pSelf 及其 Actor 組件存在且存活
	if (pSelf?.a == null || !pSelf.a.isAlive() || pTile == null)
	{
		return false;
	}
	Actor selfActor = pSelf.a;
	string StatusID_01 = "cdt_debuff00"; // 狀態ID
	float Duration_01 = 600f; // 狀態的持續時間 (可調整)
	// ====== 確保施法者不會被自己的法術影響 ======
	if (selfActor.hasStatus(StatusID_01))
	{
		selfActor.finishStatusEffect(StatusID_01);
	}
	// === 自動尋找目標 ===
	float maxRange = 20f;	// 最大範圍
	float minRange = 0f;	// 最小範圍
	Actor target = null;
	float closestDist = float.MaxValue;
	foreach (var other in World.world.units) // 遍歷所有單位
	{	// 檢查目標是否有效、存活，且不是施法者自己
		if (other == null || other == selfActor || !other.isAlive())
		{
			continue;
		}

		// 判斷是否為敵對單位
		bool isEnemy = false;
		if (selfActor.kingdom != null && other.kingdom != null)
		{
			// 情況一：施法者和目標都有王國，使用輔助方法判斷敵對關係
			isEnemy = Traits01Actions.isEnemy(selfActor, other);
		}
		else if (other.kingdom == null && selfActor.kingdom != null)
		{
			isEnemy = true;
		}

		// 如果不是敵人，則跳過
		if (!isEnemy)
		{
			continue;
		}

		// 計算距離，尋找最近的敵人
		float dist = Vector2.Distance(selfActor.current_position, other.current_position);
		if (dist < maxRange && dist > minRange && dist < closestDist)
		{
			closestDist = dist;
			target = other;
		}
	}

	// 如果沒有找到合適的敵方目標，則退出
	if (target == null)
	{
		return false;
	}

	// 檢查目標是否已經擁有負面狀態
	if (!target.hasStatus(StatusID_01))
	{
		target.addStatusEffect(StatusID_01, Duration_01);
		return true;
	}
	return false;
}
		public static bool Extend_Effect_Atk01(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 御時法 封鎖1 對攻擊類特質 Trait_Atk		(攻擊被攻擊觸發)
			// 攻擊者或目標為空，不執行
			if (pSelf == null || pTarget == null)
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			Actor targetActor = pTarget.a;
			// 攻擊者或目標 Actor 無效或已死亡，不執行
			if (selfActor == null || targetActor == null || !selfActor.isAlive() || !targetActor.isAlive())
			{
				return false;
			}
			// 檢查攻擊對象是否擁有清單中的任何特質
			bool targetHasRequiredTrait = false;
			foreach (var traitId in Trait_Atk)
			{
				if (targetActor.hasTrait(traitId))
				{
					targetHasRequiredTrait = true;
					break;
				}
			}
			// 如果目標沒有清單中的任何特質，則不執行後續效果
			if (!targetHasRequiredTrait)
			{
				return false;
			}
			// 定義冷卻狀態 ID 和持續時間
			string targetAddStatus = "cdt_debuff01";
			float targetAddDuration = 90f;
			// 檢查目標是否已擁有該狀態效果，如果沒有則添加
			if (!targetActor.hasStatus(targetAddStatus))
			{
				targetActor.addStatusEffect(targetAddStatus, targetAddDuration);
				return true;
			}
			return false;
		}
		public static bool Extend_Effect_Atk02(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 御時法 封鎖2 對強化類特質 Trait_Status	(攻擊被攻擊觸發)
			// 攻擊者或目標為空，不執行
			if (pSelf == null || pTarget == null)
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			Actor targetActor = pTarget.a;
			// 攻擊者或目標 Actor 無效或已死亡，不執行
			if (selfActor == null || targetActor == null || !selfActor.isAlive() || !targetActor.isAlive())
			{
				return false;
			}
			// 檢查攻擊對象是否擁有清單中的任何特質
			bool targetHasRequiredTrait = false;
			foreach (var traitId in Trait_Status)
			{
				if (targetActor.hasTrait(traitId))
				{
					targetHasRequiredTrait = true;
					break;
				}
			}
			// 如果目標沒有清單中的任何特質，則不執行後續效果
			if (!targetHasRequiredTrait)
			{
				return false;
			}
			// 定義冷卻狀態 ID 和持續時間
			string targetAddStatus = "cdt_debuff02";
			float targetAddDuration = 180f;
			// 檢查目標是否已擁有該狀態效果，如果沒有則添加
			if (!targetActor.hasStatus(targetAddStatus))
			{
				targetActor.addStatusEffect(targetAddStatus, targetAddDuration);
				return true;
			}
			return false;
		}
		public static bool Extend_Effect_Atk03(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 御時法 封鎖3 對恢復類特質 Trait_Holy	(攻擊被攻擊觸發)
			// 攻擊者或目標為空，不執行
			if (pSelf == null || pTarget == null)
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			Actor targetActor = pTarget.a;
			// 攻擊者或目標 Actor 無效或已死亡，不執行
			if (selfActor == null || targetActor == null || !selfActor.isAlive() || !targetActor.isAlive())
			{
				return false;
			}
			// 檢查攻擊對象是否擁有清單中的任何特質
			bool targetHasRequiredTrait = false;
			foreach (var traitId in Trait_Holy)
			{
				if (targetActor.hasTrait(traitId))
				{
					targetHasRequiredTrait = true;
					break;
				}
			}
			// 如果目標沒有清單中的任何特質，則不執行後續效果
			if (!targetHasRequiredTrait)
			{
				return false;
			}
			// 定義冷卻狀態 ID 和持續時間
			string targetAddStatus = "cdt_debuff03";
			float targetAddDuration = 180f;
			// 檢查目標是否已擁有該狀態效果，如果沒有則添加
			if (!targetActor.hasStatus(targetAddStatus))
			{
				targetActor.addStatusEffect(targetAddStatus, targetAddDuration);
				return true;
			}
			return false;
		}
		public static bool Extend_Effect_Atk04(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 御時法 封鎖4 對建築類特質 Trait_Const	(攻擊被攻擊觸發)
			// 攻擊者或目標為空，不執行
			if (pSelf == null || pTarget == null)
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			Actor targetActor = pTarget.a;
			// 攻擊者或目標 Actor 無效或已死亡，不執行
			if (selfActor == null || targetActor == null || !selfActor.isAlive() || !targetActor.isAlive())
			{
				return false;
			}
			// 檢查攻擊對象是否擁有清單中的任何特質
			bool targetHasRequiredTrait = false;
			foreach (var traitId in Trait_Const)
			{
				if (targetActor.hasTrait(traitId))
				{
					targetHasRequiredTrait = true;
					break;
				}
			}
			// 如果目標沒有清單中的任何特質，則不執行後續效果
			if (!targetHasRequiredTrait)
			{
				return false;
			}
			// 定義冷卻狀態 ID 和持續時間
			string targetAddStatus = "cdt_debuff04";
			float targetAddDuration = 3900f;
			// 檢查目標是否已擁有該狀態效果，如果沒有則添加
			if (!targetActor.hasStatus(targetAddStatus))
			{
				targetActor.addStatusEffect(targetAddStatus, targetAddDuration);
				return true;
			}
			return false;
		}
		public static bool Extend_Effect1(BaseSimObject pSelf, WorldTile pTile)
		{// 御時法 封鎖1 對攻擊類特質 Trait_Atk		(群體添加)
			// 1. 基本安全檢查：確保施法者 pSelf 及其 Actor 組件存在且存活
			if (pSelf?.a == null || !pSelf.a.isAlive() || pTile == null)
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			string StatusID_01 = "cdt_debuff01"; // 狀態ID
			float Duration_01 = 90f; // 狀態的持續時間
			// 確保施法者不會被自己的法術影響
			if (selfActor.hasStatus(StatusID_01))
			{
				selfActor.finishStatusEffect(StatusID_01);
			}
			// 定義範圍
			float maxRange = 20f;
			bool appliedToAnyTarget = false; // 用於追蹤是否有任何目標被施加了效果
			// 2. 遍歷所有單位，尋找符合條件的多個目標
			foreach (var other in World.world.units)
			{
				// 檢查目標是否有效、存活，且不是施法者自己
				if (other == null || other == selfActor || !other.isAlive())
				{
					continue;
				}
				// 判斷是否為敵對單位
				bool isEnemy = false;
				if (selfActor.kingdom != null && other.kingdom != null)
				{
					isEnemy = Traits01Actions.isEnemy(selfActor, other);
				}
				else if (other.kingdom == null && selfActor.kingdom != null)
				{
					isEnemy = true;
				}
				// 如果不是敵人，或距離超過範圍，則跳過
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (!isEnemy || dist > maxRange)
				{
					continue;
				}
				// 檢查目標是否擁有清單中的任何特質
				bool hasTargetTrait = false;
				foreach (string traitId in Trait_Atk)//特質清單
				{
					if (other.hasTrait(traitId))
					{
						hasTargetTrait = true;
						break;
					}
				}
				// 如果目標沒有任何指定特質，則跳過
				if (!hasTargetTrait)
				{
					continue;
				}
				// 3. 如果目標符合所有條件，則施加效果
				if (!other.hasStatus(StatusID_01))
				{
					other.addStatusEffect(StatusID_01, Duration_01);
					appliedToAnyTarget = true; // 標記為已成功施加效果
				}
			}
			// 4. 返回是否成功對任何一個目標施加了效果
			return appliedToAnyTarget;
		}
		public static bool Extend_Effect2(BaseSimObject pSelf, WorldTile pTile)
		{// 御時法 封鎖2 對強化類特質 Trait_Status	(群體添加)
			// 1. 基本安全檢查：確保施法者 pSelf 及其 Actor 組件存在且存活
			if (pSelf?.a == null || !pSelf.a.isAlive() || pTile == null)
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			string StatusID_01 = "cdt_debuff02"; // 狀態ID
			float Duration_01 = 120f; // 狀態的持續時間
			// 確保施法者不會被自己的法術影響
			if (selfActor.hasStatus(StatusID_01))
			{
				selfActor.finishStatusEffect(StatusID_01);
			}
			// 定義範圍
			float maxRange = 30f;
			bool appliedToAnyTarget = false; // 用於追蹤是否有任何目標被施加了效果
			// 2. 遍歷所有單位，尋找符合條件的多個目標
			foreach (var other in World.world.units)
			{
				// 檢查目標是否有效、存活，且不是施法者自己
				if (other == null || other == selfActor || !other.isAlive())
				{
					continue;
				}
				// 判斷是否為敵對單位
				bool isEnemy = false;
				if (selfActor.kingdom != null && other.kingdom != null)
				{
					isEnemy = Traits01Actions.isEnemy(selfActor, other);
				}
				else if (other.kingdom == null && selfActor.kingdom != null)
				{
					isEnemy = true;
				}
				// 如果不是敵人，或距離超過範圍，則跳過
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (!isEnemy || dist > maxRange)
				{
					continue;
				}
				// 檢查目標是否擁有清單中的任何特質
				bool hasTargetTrait = false;
				foreach (string traitId in Trait_Status)//特質清單
				{
					if (other.hasTrait(traitId))
					{
						hasTargetTrait = true;
						break;
					}
				}
				// 如果目標沒有任何指定特質，則跳過
				if (!hasTargetTrait)
				{
					continue;
				}
				// 3. 如果目標符合所有條件，則施加效果
				if (!other.hasStatus(StatusID_01))
				{
					other.addStatusEffect(StatusID_01, Duration_01);
					appliedToAnyTarget = true; // 標記為已成功施加效果
				}
			}
			// 4. 返回是否成功對任何一個目標施加了效果
			return appliedToAnyTarget;
		}
		public static bool Extend_Effect3(BaseSimObject pSelf, WorldTile pTile)
		{// 御時法 封鎖3 對恢復類特質 Trait_Holy	(群體添加)
			// 1. 基本安全檢查：確保施法者 pSelf 及其 Actor 組件存在且存活
			if (pSelf?.a == null || !pSelf.a.isAlive() || pTile == null)
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			string StatusID_01 = "cdt_debuff03"; // 狀態ID
			float Duration_01 = 180f; // 狀態的持續時間
			// 確保施法者不會被自己的法術影響
			if (selfActor.hasStatus(StatusID_01))
			{
				selfActor.finishStatusEffect(StatusID_01);
			}
			// 定義範圍
			float maxRange = 30f;
			bool appliedToAnyTarget = false; // 用於追蹤是否有任何目標被施加了效果
			// 2. 遍歷所有單位，尋找符合條件的多個目標
			foreach (var other in World.world.units)
			{
				// 檢查目標是否有效、存活，且不是施法者自己
				if (other == null || other == selfActor || !other.isAlive())
				{
					continue;
				}
				// 判斷是否為敵對單位
				bool isEnemy = false;
				if (selfActor.kingdom != null && other.kingdom != null)
				{
					isEnemy = Traits01Actions.isEnemy(selfActor, other);
				}
				else if (other.kingdom == null && selfActor.kingdom != null)
				{
					isEnemy = true;
				}
				// 如果不是敵人，或距離超過範圍，則跳過
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (!isEnemy || dist > maxRange)
				{
					continue;
				}
				// 檢查目標是否擁有清單中的任何特質
				bool hasTargetTrait = false;
				foreach (string traitId in Trait_Holy)//特質清單
				{
					if (other.hasTrait(traitId))
					{
						hasTargetTrait = true;
						break;
					}
				}
				// 如果目標沒有任何指定特質，則跳過
				if (!hasTargetTrait)
				{
					continue;
				}
				// 3. 如果目標符合所有條件，則施加效果
				if (!other.hasStatus(StatusID_01))
				{
					other.addStatusEffect(StatusID_01, Duration_01);
					appliedToAnyTarget = true; // 標記為已成功施加效果
				}
			}
			// 4. 返回是否成功對任何一個目標施加了效果
			return appliedToAnyTarget;
		}
		public static bool Extend_Effect4(BaseSimObject pSelf, WorldTile pTile)
		{// 御時法 封鎖4 對建築類特質 Trait_Const	(群體添加)
			// 1. 基本安全檢查：確保施法者 pSelf 及其 Actor 組件存在且存活
			if (pSelf?.a == null || !pSelf.a.isAlive() || pTile == null)
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			string StatusID_01 = "cdt_debuff04"; // 狀態ID
			float Duration_01 = 3900f; // 狀態的持續時間
			// 確保施法者不會被自己的法術影響
			if (selfActor.hasStatus(StatusID_01))
			{
				selfActor.finishStatusEffect(StatusID_01);
			}
			// 定義範圍
			float maxRange = 150f;
			bool appliedToAnyTarget = false; // 用於追蹤是否有任何目標被施加了效果
			// 2. 遍歷所有單位，尋找符合條件的多個目標
			foreach (var other in World.world.units)
			{
				// 檢查目標是否有效、存活，且不是施法者自己
				if (other == null || other == selfActor || !other.isAlive())
				{
					continue;
				}
				// 判斷是否為敵對單位
				bool isEnemy = false;
				if (selfActor.kingdom != null && other.kingdom != null)
				{
					isEnemy = Traits01Actions.isEnemy(selfActor, other);
				}
				else if (other.kingdom == null && selfActor.kingdom != null)
				{
					isEnemy = true;
				}
				// 如果不是敵人，或距離超過範圍，則跳過
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (!isEnemy || dist > maxRange)
				{
					continue;
				}
				// 檢查目標是否擁有清單中的任何特質
				bool hasTargetTrait = false;
				foreach (string traitId in Trait_Const)//特質清單
				{
					if (other.hasTrait(traitId))
					{
						hasTargetTrait = true;
						break;
					}
				}
				// 如果目標沒有任何指定特質，則跳過
				if (!hasTargetTrait)
				{
					continue;
				}
				// 3. 如果目標符合所有條件，則施加效果
				if (!other.hasStatus(StatusID_01))
				{
					other.addStatusEffect(StatusID_01, Duration_01);
					appliedToAnyTarget = true; // 標記為已成功施加效果
				}
			}
			// 4. 返回是否成功對任何一個目標施加了效果
			return appliedToAnyTarget;
		}
		public static bool ClearTime_Effect1(BaseSimObject pSelf, WorldTile pTile)
		{// 御時法 cdt_clear00狀態添加
			// 確保施法者有效且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a; // 施法者
			// ** 臨時安全措施：如果 pTile 為 null，則直接返回，避免崩潰 **
			if (pTile == null)
			{
				// Debug.Log("ClearTime_Effect1 Warning: pTile is null. Skipping effect to prevent crash.");
				return false;
			}
			string clearStatusID = "cdt_clear00";
			string buffStatusID = ""; // 新增：cdt_buff00 的 ID
			float statusDuration = 60f; // 60 秒持續時間 (您也可以為不同狀態設定不同持續時間)
			int range = 1; // 影響範圍
			// =========================================================================
			// 修正點：優先處理施法者自身，並檢查 cdt_buff00 冷卻
			// =========================================================================
			if (!selfActor.hasStatus(buffStatusID)) // 檢查施法者是否沒有 "cdt_buff00" 冷卻狀態
			{
				// 如果沒有 cdt_buff00，則發動能力：為自己添加 cdt_buff00 和 cdt_clear00
				selfActor.addStatusEffect(buffStatusID, statusDuration); // 添加 cdt_buff00 冷卻狀態
				selfActor.addStatusEffect(clearStatusID, statusDuration); // 添加 cdt_clear00 狀態
				//Debug.Log($"ClearTime_Effect1: {selfActor.name} **成功發動御時法**，獲得 '{buffStatusID}' 和 '{clearStatusID}' 狀態 (自身).");
				// 現在才處理範圍內其他單位
				var allClosestUnits = Finder.getUnitsFromChunk(pTile, range);
				if (allClosestUnits.Any())
				{
					foreach (var unit in allClosestUnits)
					{
						Actor targetActor = unit.a; // 從 BaseSimObject 獲取 Actor 組件
						// 檢查 targetActor 是否是有效的 Actor，存活，且不是施法者本身，且屬於同一王國
						// 並且目標單位自身沒有 clearStatusID
						if (targetActor != null && targetActor.isAlive() && targetActor != selfActor && targetActor.kingdom == selfActor.kingdom)
						{
							if (!targetActor.hasStatus(clearStatusID)) // 確保目標沒有此狀態才添加
							{
								targetActor.addStatusEffect(clearStatusID, statusDuration);
								//Debug.Log($"ClearTime_Effect1: {targetActor.name} 獲得了 '{clearStatusID}' 狀態 (友軍).");
							}
						}
					}
				}
			}
			else // 如果施法者有 "cdt_buff00" 冷卻狀態，則能力不發動
			{
				//Debug.Log($"ClearTime_Effect1: {selfActor.name} 擁有 '{buffStatusID}' 冷卻狀態，御時法無法發動。");
				return false; // 表示技能因冷卻條件不符而未發動
			}
			return true; // 表示技能已成功發動（施法者自身已處理，並可能影響了友軍）
		}
		public static bool ClearTime_Effect2(BaseSimObject pSelf, WorldTile pTile)
		{// 御時法 添加cdt_Clear01
			// 確保施法者有效且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a; // 施法者
			// ** 臨時安全措施：如果 pTile 為 null，則直接返回，避免崩潰 **
			// 此方法簽名已經提供了 pTile，所以我們應該直接使用它作為中心點。
			if (pTile == null)
			{
				// Debug.Log("ClearTime_Effect2 Warning: pTile is null. Skipping effect to prevent crash.");
				return false;
			}
			// --- 對特定特質 ("契約Zero") 的同陣營單位添加 cdt_Clear01 狀態 (240秒) ---
			int specificEffectRange = 10; // 指定的範圍，從 float 改為 int
			string specificTargetTraitID = "契約Zero"; // 目標特質ID (中文ID)
			string statusToAdd = "cdt_Clear01";
			float statusDuration = 300f; // 240秒改為300秒，因為您註釋中是300f，但原代碼是240f，以原代碼為準改為300f。
			// 獲取半徑範圍內的所有 Actor 對象
			// 替換 World.world.getObjectsInChunks 為 Finder.getUnitsFromChunk
			var allClosestUnits = Finder.getUnitsFromChunk(pTile, specificEffectRange);
			if (allClosestUnits.Any())
			{
				foreach (var unit in allClosestUnits)
				{
					Actor targetActor = unit.a; // 從 BaseSimObject 獲取 Actor 組件
					// 檢查 targetActor 是否是有效的 Actor，存活，且不是施法者本身，且屬於同一王國
					if (targetActor != null && targetActor.isAlive() && targetActor != selfActor && targetActor.kingdom == selfActor.kingdom)
					{
						// 檢查是否持有 "契約Zero" 特質
						// 注意：如果 "契約Zero" 未來會變成狀態，則需要改為 hasStatus()
						if (targetActor.hasTrait(specificTargetTraitID))
						{
							// 添加 cdt_Clear01 狀態
							if (!targetActor.hasStatus(statusToAdd)) // 避免重複添加相同狀態
							{
								targetActor.addStatusEffect(statusToAdd, statusDuration);
								// Debug.Log($"ClearTime_Effect2: 對 {targetActor.name} (持有 '{specificTargetTraitID}') 施加了 '{statusToAdd}' 狀態，持續 {statusDuration} 秒。");
							}
						}
					}
				}
			}
			return true; // 返回 true 表示技能嘗試執行了，即使沒有目標受影響
		}

		private static Dictionary<long, int> starvingCount = new Dictionary<long, int>();
		public static bool ManageGluttonyByNutrition(BaseSimObject pSelf, WorldTile pTile = null)
		{// 餓食法 魔王狀態 賦予 飢餓的次數
			// 1. 基本安全安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			long actorId = selfActor.data.id;
			const int STARVING_COUNT_TO_BECOME_DEMON_KING = 3;

			// 2. 如果單位有 gluttony_demon_king 狀態，則不執行此邏輯
			if (selfActor.hasStatus("gluttony_demon_king"))
			{
				return false;
			}

			// ====== 新增邏輯：檢查王國內是否已存在魔王 ======
			Kingdom currentKingdom = selfActor.kingdom;
			// 只有當單位屬於一個王國時才進行檢查
			if (currentKingdom != null)
			{
				foreach (Actor kingdomUnit in currentKingdom.units)
				{
					if (kingdomUnit == null || kingdomUnit == selfActor)
					{
						continue;
					}
					// 優先檢查是否有 'other666' 特質
					if (kingdomUnit.hasTrait("hope")||
						kingdomUnit.hasTrait("other6661")||
						kingdomUnit.hasTrait("other6662")||
						kingdomUnit.hasTrait("other6663")||
						kingdomUnit.hasTrait("other6664")||
						kingdomUnit.hasTrait("other6665")||
						kingdomUnit.hasTrait("other6666")||
						kingdomUnit.hasTrait("other6667")||
						kingdomUnit.hasTrait("other6668")||
						kingdomUnit.hasTrait("other6669"))
					{
						return false;
					}
					// 檢查單位是否擁有任一魔王狀態
					foreach (string demonKingStatusID in SevenDemonKingStatus_DemonKing)
					{
						if (kingdomUnit.hasStatus(demonKingStatusID))
						{
							// 如果找到魔王，直接返回 false，不進行後續操作
							return false;
						}
					}
				}
			}
			// ===================================
			// 3. 檢查單位是否有 starving 狀態
			if (selfActor.hasStatus("starving"))
			{
				// 增加計數
				if (starvingCount.ContainsKey(actorId))
				{
					starvingCount[actorId]++;
				}
				else
				{
					starvingCount[actorId] = 1;
				}
				// 檢查計數是否達到閾值
				if (starvingCount[actorId] >= STARVING_COUNT_TO_BECOME_DEMON_KING)
				{
					// 重置計數
					starvingCount.Remove(actorId);
					// 添加「暴食魔王」狀態
					selfActor.addStatusEffect("gluttony_demon_king", 3600f);
					selfActor.finishStatusEffect("starving");
					EvilSpearGet(pSelf, pTile);
					Items01Actions.EvilSpearAwakens(pSelf, pTile);
					return true;
				}
			}
			return false;
		}
		public static bool DrainEnemyHunger(BaseSimObject pSelf, WorldTile pTile)
		{// 餓食法 營養值 降低 (針對不同國家單位)
			// 確保施法者有效且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a; // 施法者
			// ** 臨時安全措施：如果 pTile 為 null，則直接返回，避免崩潰 **
			if (pTile == null)
			{
				// Debug.Log("DrainEnemyHunger Warning: pTile is null. Skipping effect to prevent crash.");
				return false;
			}
			float nutritionDrainAmount = 11f; // 可調控的營養值降低量
			int drainRange = 5; // 可調控的影響範圍
			// bool effectApplied = false; // 此標誌在此函數中不是必須的，因為我們總是返回 true
			// --- 搜尋範圍內不同陣營的單位並降低其營養值 ---
			// 替換 World.world.getObjectsInChunks 為 Finder.getUnitsFromChunk
			var allClosestUnits = Finder.getUnitsFromChunk(pTile, drainRange);
			if (allClosestUnits.Any())
			{
				foreach (var unit in allClosestUnits)
				{
					Actor otherActor = unit.a; // 從 BaseSimObject 獲取 Actor 組件
					//指定特質檢查
					if (otherActor  != null && otherActor.hasTrait("evillaw_starvation"))
					{
						continue; // 跳過此目標，繼續檢查下一個
					}
					// 檢查 otherActor 是否是有效的 Actor，存活，不是施法者本身，且屬於不同王國
					if (otherActor != null && otherActor.isAlive() && otherActor != selfActor && otherActor.kingdom != selfActor.kingdom)
					{
						// 確保營養值大於0才扣減
						// WorldBox 營養值範圍通常是 0-100。
						// 如果其他Actor的營養值高於0，則進行扣減
						if (otherActor.data.nutrition > 0) 
						{
							// 降低營養值，確保不低於 0
							otherActor.data.nutrition = Mathf.Max(0, otherActor.data.nutrition - Mathf.RoundToInt(nutritionDrainAmount));
							// effectApplied = true; // 如果需要精確追蹤是否實際應用，可取消註釋
							// Debug.Log($"{otherActor.name} 的營養值降低了 {nutritionDrainAmount}. 當前營養值: {otherActor.data.nutrition}");
						}
					}
				}
			}
			
			// 即使沒有單位滿足條件，技能邏輯也嘗試執行了，所以返回 true
			return true; // 效果成功執行
		}
		public static bool Devour_HungerHealth(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 餓食法 營養值 攻擊扣減
			// 確保施法者和目標有效且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive() ||
				pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false; 
			}
			Actor selfActor = pSelf.a;
			Actor targetActor = pTarget.a;
			// **修正：所有營養值相關的計算改回絕對值，HP 仍保留百分比**
			int maxNutritionValue = 100; // 遊戲中營養值的最大值，根據您的情報設定
			int minNutritionValue = 0;   // 遊戲中營養值的最小值
			// --- 對交戰對象進行營養值扣減 ---
			float nutritionDecreaseAmount = 1f; // 可調控的營養值扣減量 (每次降低 1 點)
			if (targetActor.data.nutrition > minNutritionValue) // 確保目標營養值大於最小值才扣減
			{
				targetActor.data.nutrition = Mathf.Max(minNutritionValue, targetActor.data.nutrition - Mathf.RoundToInt(nutritionDecreaseAmount));
				// Debug.Log($"{targetActor.name} 的營養值降低了 {nutritionDecreaseAmount}. 當前營養值: {targetActor.data.nutrition}");
			}
			// --- 持有特質的單位進行生命值百分比恢復 和 營養值絕對值恢復 ---
			float healthRestorePercentage = 0.01f; // 生命值恢復百分比 (%)
			// 恢復自身生命值 (百分比恢復) - 這部分保持不變
			int healthToRestore = Mathf.RoundToInt(selfActor.getMaxHealth() * healthRestorePercentage);
			selfActor.data.health = Mathf.Min(selfActor.getMaxHealth(), selfActor.data.health + healthToRestore);
			float selfNutritionRestoreAmount = 3f; // 恢復 3 點營養值
			selfActor.data.nutrition = Mathf.Min(maxNutritionValue, selfActor.data.nutrition + Mathf.RoundToInt(selfNutritionRestoreAmount));
			// Debug.Log($"{selfActor.name} 的營養值恢復了 {selfNutritionRestoreAmount} 點. 當前營養值: {selfActor.data.nutrition}");
			return true; // 效果成功執行
		}
		public static bool RapidHunger(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 餓食法 營養扣減
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
			if (selfActor.hasStatus("gluttony_demon_king"))
			{
				// 如果 pSelf 是吞食魔王，則不繼續
				return false;
			}
			const int Using_Nutrition = 7;
			// 確保營養值不會低於 0
			selfActor.data.nutrition = Mathf.Max(0, selfActor.data.nutrition - Using_Nutrition);
			if (selfActor.data.nutrition <= 1 && !selfActor.hasStatus("starving"))
			{
				selfActor.addStatusEffect("starving", 30);
				ManageGluttonyByNutrition(pSelf, pTile);
				return false;
			}
			return true;
		}
		public static bool EvilSpearGet(BaseSimObject pTarget, WorldTile pTile = null)
		{// 餓食法 武器給予
			// 定義所需的狀態ID常量
			const string DemonKingStatus = "gluttony_demon_king"; //魔王狀態
			const string WeapontID = "evil_spear"; // 物品ID
			// 1. 基本安全檢查：目標是否有效且存活
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor targetActor = pTarget.a;
			// ****** 新增檢查：單位是否擁有 '指定魔王狀態' 狀態 ******
			if (!targetActor.hasStatus(DemonKingStatus))
			{
				return false; // 如果沒有此狀態，則不給予裝備
			}
			// ************************************************************
			// 獲取目標的武器槽位
			var weaponSlot = targetActor.equipment.getSlot(EquipmentType.Weapon);
			// 檢查武器槽位是否已被佔用，且其中持有的道具ID是否為 "指定武器"
			if (weaponSlot != null && weaponSlot.getItem() != null && weaponSlot.getItem().asset.id == WeapontID)
			{
				// 額外檢查：如果武器沒有修飾符，在這裡補上
				Item existingWeapon = weaponSlot.getItem();
				/*if (!existingWeapon.hasMod("eternal"))
				{
					existingWeapon.addMod("eternal");
					existingWeapon.addMod("cursed");
				}*/
				return true;
			}
			// 如果單位沒有持有 "指定武器" 且擁有指定狀態，則繼續進行裝備流程
			// 獲取 "指定武器" 物品資產
			var weaponAsset = AssetManager.items.get(WeapontID);
			if (weaponAsset == null)
			{
				return false; // 無法找到物品資產，返回失敗
			}
			// 生成 "指定武器" 物品實例
			var newItem = World.world.items.generateItem(pItemAsset: weaponAsset);
			// =======================================================
			// === 核心修正：強制添加修飾符 ===
			if (newItem != null)
			{
				//newItem.addMod("eternal");
				newItem.addMod("power5");
				newItem.addMod("truth5");
				newItem.addMod("protection5");
				newItem.addMod("speed5");
				newItem.addMod("balance5");
				newItem.addMod("health5");
				newItem.addMod("finesse5");
				newItem.addMod("mastery5");
				newItem.addMod("knowledge5");
				newItem.addMod("sharpness5");
			}
			// =======================================================
			// 將 "指定武器" 裝備到目標的武器槽位
			weaponSlot.setItem(newItem, targetActor);
			// 更新目標的統計數據，以反映新裝備帶來的變化
			targetActor.setStatsDirty();
			targetActor.data.health += 99999;
			return true; // 表示效果成功施加
		}

		public static bool Skill0405addStatus(BaseSimObject pSelf, WorldTile pTile)
		{// 病災法 狀態添加
			// 確保施法者有效且存活，並且 pTile 不為 null
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive() || pTile == null)
			{
				return false; // 無效的施法者、目標地塊或施法者已死亡
			}
			
			Actor selfActor = pSelf.a;
			int range = 1; // 設定影響範圍，您可以根據需要調整
			bool statusAddedToTarget = false; // 追蹤是否成功對至少一個目標添加了狀態
			// --- 獲取範圍內的 Actor (替換 World.world.getObjectsInChunks 為 Finder.getUnitsFromChunk) ---
			var allClosestUnits = Finder.getUnitsFromChunk(pTile, range);
			if (allClosestUnits.Any()) // 使用 LINQ 的 Any() 檢查是否有單位
			{
				// 遍歷範圍內的 Actor
				foreach (var unit in allClosestUnits)
				{
					Actor targetActor = unit.a; // 從 BaseSimObject 獲取 Actor 組件
					// 檢查目標是否有效、存活、不是施法者本身，且不屬於同一王國 (敵方單位)
					if (targetActor != null && targetActor.isAlive() && targetActor != selfActor && targetActor.kingdom != selfActor.kingdom)
					{
						// 在這裡添加你希望賦予目標的狀態效果
						string statusToAdd = "weaken"; // 你要添加的狀態效果的 ID
						float statusDuration = 300f;	// 設定狀態效果的持續時間
						// 檢查目標是否沒有這個狀態效果，避免重複添加
						if (!targetActor.hasStatus(statusToAdd))
						{
							targetActor.addStatusEffect(statusToAdd, statusDuration);
							// Debug.Log($"{selfActor.name} 對 {targetActor.name} 施加了狀態：{statusToAdd}，持續 {statusDuration} 秒");
							statusAddedToTarget = true; // 標記為已成功添加狀態
						}
						else
						{
							// 可選：如果目標已經有這個狀態，可以添加 Log 訊息
							// Debug.Log($"{targetActor.name} 已經擁有狀態：{statusToAdd}");
						}
					}
				}
			}
			return statusAddedToTarget; // 返回是否成功對至少一個目標添加了狀態
		}
		public static bool Skill0405addTrait(BaseSimObject pSelf, WorldTile pTile)
		{// 病災法 特質添加
			// 確保施法者有效且存活，並且 pTile 不為 null
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive() || pTile == null)
			{
				return false; // 無效的施法者、目標地塊或施法者已死亡
			}
			Actor selfActor = pSelf.a;
			int range = 3; // 設定影響範圍，您可以根據需要調整
			// 定義要賦予的特質列表 (移到函數作用域)
			string[] traitsToGive = { "tumor_infection", "infected", "mush_spores", "plague" };
			// 檢查並移除施法者自身的特定特質 (確保施法者不會自帶這些負面特質)
			foreach (string trait in traitsToGive)
			{
				if (selfActor.hasTrait(trait))
				{
					selfActor.removeTrait(trait);
					//Debug.Log($"施法者 {selfActor.name} 自身移除了特質：{trait}");
				}
			}
			// --- 獲取範圍內的 Actor (替換 World.world.getObjectsInChunks 為 Finder.getUnitsFromChunk) ---
			var allClosestUnits = Finder.getUnitsFromChunk(pTile, range);
			List<Actor> targetsToAffect = new List<Actor>();
			if (allClosestUnits.Any()) // 使用 LINQ 的 Any() 檢查是否有單位
			{
				// 遍歷範圍內的 Actor
				foreach (var unit in allClosestUnits)
				{
					Actor targetActor = unit.a; // 從 BaseSimObject 獲取 Actor 組件
					// 檢查目標是否有效、存活、不是施法者本身，且不屬於同一王國 (敵方單位)
					if (targetActor != null && targetActor.isAlive() && targetActor != selfActor && targetActor.kingdom != selfActor.kingdom)
					{
						// 檢查目標是否已經擁有列表中的任何一個特質，只有沒有這些特質的才加入
						if (!traitsToGive.Any(trait => targetActor.hasTrait(trait)))
						{
							targetsToAffect.Add(targetActor); // 將符合條件的目標添加到列表中
						}
					}
				}
			}
			// 對符合條件的目標隨機賦予特質
			if (targetsToAffect.Count > 0)
			{
				// ====== 修改隨機賦予特質的邏輯以實現指定機率分配 ======
				// 創建一個包含權重分配的特質列表
				List<string> weightedTraits = new List<string>();
				// tumor_infection, infected, mush_spores 各占約 33.3%
				// 總共 3 * 333 = 999 份
				for (int i = 0; i < 333; i++) // 333 份 tumor_infection
				{
					weightedTraits.Add("tumor_infection");
				}
				for (int i = 0; i < 333; i++) // 333 份 infected
				{
					weightedTraits.Add("infected");
				}
				for (int i = 0; i < 333; i++) // 333 份 mush_spores
				{
					weightedTraits.Add("mush_spores");
				}
				// plague 占 0.1% (即 1/1000)
				weightedTraits.Add("plague"); // 1 份 plague

				// 現在 weightedTraits 總共有 333 + 333 + 333 + 1 = 1000 個元素
				// 這樣就實現了：
				// tumor_infection, infected, mush_spores 的機率約為 333/1000 = 33.3%
				// plague 的機率為 1/1000 = 0.1%
				// =========================================================
				foreach (Actor targetActor in targetsToAffect)
				{
					// 從帶權重的列表中隨機選擇一個特質
					// UnityEngine.Random.Range(minInclusive, maxExclusive)
					string randomTrait = weightedTraits[UnityEngine.Random.Range(0, weightedTraits.Count)];
					targetActor.addTrait(randomTrait);
					// Debug.Log($"{selfActor.name} 對 {targetActor.name} 施加了特質：{randomTrait}");
				}
				return true; // 成功對至少一個目標賦予特質
			}
			else
			{
				// 即使範圍內沒有符合條件的目標，技能本身也執行了檢查和潛在的自我淨化，
				// 所以從執行層面看，可以返回 true。如果希望只有實際賦予了特質才返回 true，則需要調整。
				return true; 
			}
		}

		public static bool CoinsIncrease(BaseSimObject pTarget, WorldTile pTile = null)
		{// 金錢法 金幣增加 (帶有冷卻狀態)
			if (pTarget is Actor actor)
			{
				// 設定金幣增加的量
				int goldAmount = 20; // 增加的金幣數量
				// 設定檢查的狀態 ID 和其持續時間
				string cooldownStatusID = "goldcooldown"; // 用於金幣增加的冷卻狀態ID
				float cooldownDuration = 2f;			 // 冷卻時間 (秒), 例如 5 秒
				// ** 新增檢查：如果單位已經有冷卻狀態，則不增加金幣 **
				if (actor.hasStatus(cooldownStatusID))
				{
					return false; // 不增加金幣
				}
				// ** 增加金幣 **
				actor.data.money += goldAmount;
				actor.data.loot += goldAmount;
				// ** 賦予冷卻狀態 **
				actor.addStatusEffect(cooldownStatusID, cooldownDuration);
				return true; // 金幣已增加並賦予狀態
			}
			return false; // 目標不是 Actor
		}
		public static bool MoneyTierEffects(BaseSimObject pTarget, WorldTile pTile = null)
		{// 金錢法 根據積蓄金額 獲得添加狀態
			if (pTarget is Actor actor)
			{
				// 定义金钱门槛和对应的状态 ID
				// 将 moneyTiers 的定义移到这里，使其在整个函数范围内都可访问
				var moneyTiers = new (int moneyAmount, string statusID)[]
				{
					(550, "mp01"),
					(1200, "mp02"),
					(1950, "mp03"),
					(2800, "mp04"),
					(3750, "mp05"),
					(4800, "mp06"),
					(5950, "mp07"),
					(7200, "mp08"),
					(8550, "mp09"),
					(10000, "mp10")
				};

				// === 新增檢查：當單位自身沒有 prefrontal_cortex 亞種特質時，能力不發動 ===
				if (actor.subspecies == null || !actor.subspecies.hasTrait("prefrontal_cortex"))
				{
					//Debug.Log($"[MoneyTierEffects] 單位 {actor.name} 沒有文明亞種特質 (prefrontal_cortex)，金錢階級效果不發動。");
					// 當單位失去文明特質時，確保移除所有 mpXX 狀態
					// 現在 moneyTiers 在這裡已經被宣告，所以不會報錯
					foreach (var tier in moneyTiers) 
					{
						if (actor.hasStatus(tier.statusID))
						{
							actor.finishStatusEffect(tier.statusID);
						}
					}
					return false; 
				}
				// === 新增自身文明特質檢查結束 ===
				
				string targetStatusToAdd = null; // 用來儲存應該添加的最高狀態ID
				// 從最高門檻開始向下檢查，找到第一個滿足條件的狀態
				for (int i = moneyTiers.Length - 1; i >= 0; i--)
				{
					var tier = moneyTiers[i];
					if (actor.data.money >= tier.moneyAmount)
					{
						targetStatusToAdd = tier.statusID; 
						break; 
					}
				}
				// 現在處理所有狀態的添加和移除
				foreach (var tier in moneyTiers)
				{
					if (tier.statusID == targetStatusToAdd)
					{
						if (!actor.hasStatus(tier.statusID))
						{
							actor.addStatusEffect(tier.statusID, 999999999f);
							// Console.WriteLine($"{actor.name} 金錢達到 {tier.moneyAmount}，獲得狀態: {tier.statusID}.");
						}
					}
					else
					{
						if (actor.hasStatus(tier.statusID))
						{
							actor.finishStatusEffect(tier.statusID);
							// Console.WriteLine($"{actor.name} 金錢不符 {tier.moneyAmount}，移除狀態: {tier.statusID}.");
						}
					}
				}
				return true;
			}
			return false;
		}
		public static bool CoinsReduction(BaseSimObject pSelf, WorldTile pTile)
		{// 金錢法 金錢掠奪效果
			// 確保施法者有效且存活，並且 pTile 不為 null
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive() || pTile == null)
			{
				return false; // 無效的施法者、目標地塊或施法者已死亡
			}
			Actor selfActor = pSelf.a;
			if (selfActor.subspecies == null ||!selfActor.subspecies.hasTrait("prefrontal_cortex"))
			{
				return false;
			}
			int range = 3; // 設定影響範圍
			bool effectApplied = false; // 追蹤是否成功對至少一個目標執行了效果
			// 獲取範圍內的 Actor
			var allClosestUnits = Finder.getUnitsFromChunk(pTile, range);
			if (allClosestUnits.Any())
			{
				// 遍歷範圍內的 Actor
				foreach (var unit in allClosestUnits)
				{
					Actor targetActor = unit.a; // 從 BaseSimObject 獲取 Actor 組件
					// 檢查目標是否有效、存活、不是施法者本身，且不屬於同一王國 (敵方單位)
					if (targetActor != null && targetActor.isAlive() && targetActor != selfActor && targetActor.kingdom != selfActor.kingdom)
					{
						// === 檢查目標的金錢是否大於 100 ===
						if (targetActor.data.money > 1)
						{
							int reductionAmount = 20; // 扣減金額
							// 執行金錢扣減效果
							targetActor.data.money -= reductionAmount;
							// === 新增邏輯：自身金錢增加 ===
							selfActor.data.money += reductionAmount;
							effectApplied = true; // 標記為已成功執行效果
						}
					}
				}
			}
			return effectApplied; // 返回是否成功對至少一個目標執行了效果
		}
		public static bool LootReduction(BaseSimObject pSelf, WorldTile pTile)
		{// 金錢法 掠奪品強奪效果
			// 確保施法者有效且存活，並且 pTile 不為 null
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive() || pTile == null)
			{
				return false; // 無效的施法者、目標地塊或施法者已死亡
			}
			Actor selfActor = pSelf.a;
			if (selfActor.subspecies == null ||!selfActor.subspecies.hasTrait("prefrontal_cortex"))
			{
				return false;
			}
			int range = 3; // 設定影響範圍
			bool effectApplied = false; // 追蹤是否成功對至少一個目標執行了效果
			// 獲取範圍內的 Actor
			var allClosestUnits = Finder.getUnitsFromChunk(pTile, range);
			if (allClosestUnits.Any())
			{
				// 遍歷範圍內的 Actor
				foreach (var unit in allClosestUnits)
				{
					Actor targetActor = unit.a; // 從 BaseSimObject 獲取 Actor 組件
					// 檢查目標是否有效、存活、不是施法者本身，且不屬於同一王國 (敵方單位)
					if (targetActor != null && targetActor.isAlive() && targetActor != selfActor && targetActor.kingdom != selfActor.kingdom)
					{
						// === 檢查目標的掠奪品是否大於 100 ===
						if (targetActor.data.loot > 1)
						{
							int reductionAmount = 20; // 扣減
							// 執行掠奪品扣減效果
							targetActor.data.loot -= reductionAmount;
							// === 新增邏輯：自身掠奪品增加 ===
							selfActor.data.loot += reductionAmount;
							effectApplied = true; // 標記為已成功執行效果
						}
					}
				}
			}
			return effectApplied; // 返回是否成功對至少一個目標執行了效果
		}
		public static bool Bribery(BaseSimObject pTarget, WorldTile pTile = null)
		{// 金錢法 蘇生
			// 基礎安全檢查：確保目標單位及其 Actor 組件存在
			if (pTarget == null || pTarget.a == null)
			{
				return false;
			}
			Actor originalActor = pTarget.a; // 將原始 Actor 儲存起來以備後用
			if (originalActor.hasTrait("extraordinary_authority"))
			{
				return false;
			}
			// === 新增檢查機制：單位持有的 金錢值 和 戰利品值 是否高於 100 ===
			if (originalActor.data.money < 100)
			{
				//Debug.Log($"{originalActor.name} 金錢不足，無法重生。所需100，現有{originalActor.data.money}。");
				return false; // 金錢不足，不發動重生效果
			}
			// === 新增安全檢查：只有文明生物才能被金錢法甦生 ===
				// 單位沒有 prefrontal_cortex 特質的話就不執行甦生
			if (originalActor.subspecies == null || !originalActor.subspecies.hasTrait("prefrontal_cortex"))
			{
				//Debug.Log($"[Bribery] 單位 {originalActor.name} (ID: {originalActor.asset.id}) 沒有文明亞種特質 (prefrontal_cortex)，無法執行金錢法甦生。");
				return false; // 非文明生物，不允許甦生
			}
			// === 安全檢查結束 ===
			// 1. 移除原始單位 (這裡移除的是舊單位，使其消失)
			ActionLibrary.removeUnit(originalActor); 
			// 2. 特質添加/移除：在複製前清除這些特質，確保新單位是「潔淨」的
			originalActor.removeTrait("infected");
			originalActor.removeTrait("mush_spores");
			originalActor.removeTrait("tumor_infection");
			originalActor.removeTrait("plague");
			originalActor.removeTrait("death_mark");
			originalActor.removeTrait("skin_burns");
			originalActor.removeTrait("crippled");
			originalActor.removeTrait("eyepatch");
			originalActor.removeTrait("madness");
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
			var act = World.world.units.createNewUnit(originalActor.asset.id, pTile);
			// 5. 複製原始單位數據到新單位
			// 注意：這裡複製的是 originalActor 在被移除前的數據，包括特質、金錢、統計等
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
			//6.2 設定新單位的氏族歸屬
			if (originalActor.clan != null && originalActor.clan.isAlive()) 
			{
				act.clan = originalActor.clan;
			}
			//6.3 設定新單位的家庭歸屬
			if (originalActor.family != null && originalActor.family.isAlive()) 
			{
				act.family = originalActor.family;
			}
			// 7. 設定新單位的名稱和收藏狀態
			act.data.name = originalActor.getName();
			act.data.favorite = originalActor.data.favorite;
			act.data.health += 9999; // 恢復大量生命值

			// 10. 為新單位施加臨時狀態效果
			act.addStatusEffect("invincible", 10); 
			act.addStatusEffect("antibody", 10); 

			// 11. 更多酷炫的生成效果
			EffectsLibrary.spawnExplosionWave(pTile.posV3, 1f, 1f); 
			World.world.applyForceOnTile(pTile, 3, 1.5f, pForceOut: true, 0, null, pByWho: act); 

			// === 完成後扣減單位當前金錢 與 戰利品 50% ===
			int moneyToDeduct = (int)(act.data.money * 0.50f);
			//int lootToDeduct = (int)(act.data.loot * 0.50f);
			act.data.money -= moneyToDeduct;
			//act.data.loot -= lootToDeduct;
			if (act.data.money < 0 || act.data.loot < 0 )
			{
				act.data.money = 0;
				act.data.loot = 0;
			}
			return true; // 表示效果成功執行
		}
		public static bool EvilLawMoney(BaseSimObject pTarget, WorldTile pTile = null)
		{// 金錢法 魔王狀態
			// 檢查目標是否有效且存活
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor targetActor = pTarget.a;
			// 定義狀態ID常量
			const string GreedyDemonKingStatus = "greedy_demon_king";
			const int RequiredMoney = 5950; //5950
			// 條件 1: 檢查王國內是否已存在魔王
			Kingdom currentKingdom = targetActor.kingdom;
			if (currentKingdom != null)
			{
				bool isDemonKingExists = false; // 設置一個旗標
				foreach (Actor kingdomUnit in currentKingdom.units)
				{
					// 跳過自己
					if (kingdomUnit == null || kingdomUnit == targetActor)
					{
						continue;
					}
					// 優先檢查是否有 'other666' 特質
					if (kingdomUnit.hasTrait("hope")||
						kingdomUnit.hasTrait("other6661")||
						kingdomUnit.hasTrait("other6662")||
						kingdomUnit.hasTrait("other6663")||
						kingdomUnit.hasTrait("other6664")||
						kingdomUnit.hasTrait("other6665")||
						kingdomUnit.hasTrait("other6666")||
						kingdomUnit.hasTrait("other6667")||
						kingdomUnit.hasTrait("other6668")||
						kingdomUnit.hasTrait("other6669"))
					{
						isDemonKingExists = true;
						break;
					}
					// 檢查單位是否擁有任一魔王狀態
					foreach (string demonKingStatusID in SevenDemonKingStatus_DemonKing)
					{
						if (kingdomUnit.hasStatus(demonKingStatusID))
						{
							isDemonKingExists = true; // 發現魔王，設置旗標
							break; // 跳出內層迴圈
						}
					}
					if (isDemonKingExists)
					{
						break; // 發現魔王，跳出外層迴圈
					}
				}
				// 根據旗標結果判斷是否返回
				if (isDemonKingExists)
				{
					return false;
				}
			}
			// 條件 2: 檢查單位所持金錢是否達到 RequiredMoney
			if (targetActor.data.money < RequiredMoney)
			{
				return false;
			}
			// 如果單位已經有此狀態，則無需重複添加
			if (targetActor.hasStatus(GreedyDemonKingStatus))
			{
				return true;
			}
			// 如果所有條件都滿足，添加 'greedy_demon_king' 狀態
			targetActor.addStatusEffect(GreedyDemonKingStatus, 3600f);
			EvilGunGet(pTarget, pTile);
			Items01Actions.EvilGunAwakens(pTarget, pTile);
			return true;
		}
		public static bool EvilGunGet(BaseSimObject pTarget, WorldTile pTile = null)
		{// 金錢法 武器給予
			// 定義所需的狀態ID常量
			const string DemonKingStatus = "greedy_demon_king"; //魔王狀態
			const string WeapontID = "evil_gun"; // 物品ID
			// 1. 基本安全檢查：目標是否有效且存活
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor targetActor = pTarget.a;
			// ****** 新增檢查：單位是否擁有 '指定魔王狀態' 狀態 ******
			if (!targetActor.hasStatus(DemonKingStatus))
			{
				return false; // 如果沒有此狀態，則不給予裝備
			}
			// ************************************************************
			// 獲取目標的武器槽位
			var weaponSlot = targetActor.equipment.getSlot(EquipmentType.Weapon);
			// 檢查武器槽位是否已被佔用，且其中持有的道具ID是否為 "指定武器"
			if (weaponSlot != null && weaponSlot.getItem() != null && weaponSlot.getItem().asset.id == WeapontID)
			{
				// 額外檢查：如果武器沒有修飾符，在這裡補上
				Item existingWeapon = weaponSlot.getItem();
				/*if (!existingWeapon.hasMod("eternal"))
				{
					existingWeapon.addMod("eternal");
					existingWeapon.addMod("cursed");
				}*/
				return true;
			}
			// 如果單位沒有持有 "指定武器" 且擁有指定狀態，則繼續進行裝備流程
			// 獲取 "指定武器" 物品資產
			var weaponAsset = AssetManager.items.get(WeapontID);
			if (weaponAsset == null)
			{
				return false; // 無法找到物品資產，返回失敗
			}
			// 生成 "指定武器" 物品實例
			var newItem = World.world.items.generateItem(pItemAsset: weaponAsset);
			// =======================================================
			// === 核心修正：強制添加修飾符 ===
			if (newItem != null)
			{
				//newItem.addMod("eternal");
				newItem.addMod("power5");
				newItem.addMod("truth5");
				newItem.addMod("protection5");
				newItem.addMod("speed5");
				newItem.addMod("balance5");
				newItem.addMod("health5");
				newItem.addMod("finesse5");
				newItem.addMod("mastery5");
				newItem.addMod("knowledge5");
				newItem.addMod("sharpness5");
			}
			// =======================================================
			// 將 "指定武器" 裝備到目標的武器槽位
			weaponSlot.setItem(newItem, targetActor);
			// 更新目標的統計數據，以反映新裝備帶來的變化
			targetActor.setStatsDirty();
			targetActor.data.health += 99999;
			return true; // 表示效果成功施加
		}

		public static bool DrainEnemyHMS(BaseSimObject pSelf, WorldTile pTile)
		{// 吸收法
			// 確保施法者有效且存活
			if (pSelf == null || !(pSelf is Actor selfActor) || !selfActor.isAlive())
			{
				return false;
			}
			// 安全措施：如果 pTile 為 null，則直接返回
			if (pTile == null)
			{
				return false;
			}
			float drainPercentage = 0.05f;	  // 敵人百分比扣減量 (5%)
			float thresholdPercentage = 0.20f;  // 敵人屬性閾值 (20%)
			int drainRange = 3;				 // 影響範圍
			// 新增：施法者恢復自身最大值的百分比
			float selfHealPercentage = 0.10f;   // 施法者恢復2%的最大生命值
			float selfManaRestorePercentage = 0.10f; // 施法者恢復2%的最大法力值
			float selfStaminaRestorePercentage = 0.10f; // 施法者恢復2%的最大耐力值
			bool effectAppliedToAnyEnemy = false; // 追蹤是否成功對至少一個敵人應用了扣減效果
			// --- 搜尋範圍內不同陣營的單位並扣減其 Health, Stamina, Mana ---
			var allClosestUnits = Finder.getUnitsFromChunk(pTile, drainRange);
			if (allClosestUnits.Any())
			{
				foreach (BaseSimObject unit in allClosestUnits)
				{
					if (!(unit is Actor otherActor) || !otherActor.isAlive()) 
					{
						continue; 
					}
					if (otherActor != selfActor && otherActor.kingdom != selfActor.kingdom)
					{
						// Health 扣減邏輯
						if (otherActor.data.health > otherActor.getMaxHealth() * thresholdPercentage)
						{
							float actualDrain = otherActor.getMaxHealth() * drainPercentage;
							actualDrain = Mathf.Min(actualDrain, otherActor.data.health - otherActor.getMaxHealth() * thresholdPercentage);
							actualDrain = Mathf.Max(0, actualDrain);
							otherActor.data.health -= Mathf.RoundToInt(actualDrain);
							effectAppliedToAnyEnemy = true;
						}
						// Stamina 扣減邏輯
						if (otherActor.data.stamina > otherActor.getMaxStamina() * thresholdPercentage)
						{
							float actualDrain = otherActor.getMaxStamina() * drainPercentage;
							actualDrain = Mathf.Min(actualDrain, otherActor.data.stamina - otherActor.getMaxStamina() * thresholdPercentage);
							actualDrain = Mathf.Max(0, actualDrain);
							otherActor.data.stamina -= Mathf.RoundToInt(actualDrain);
							effectAppliedToAnyEnemy = true;
						}
						// Mana 扣減邏輯
						if (otherActor.data.mana > otherActor.getMaxMana() * thresholdPercentage)
						{
							float actualDrain = otherActor.getMaxMana() * drainPercentage;
							actualDrain = Mathf.Min(actualDrain, otherActor.data.mana - otherActor.getMaxMana() * thresholdPercentage);
							actualDrain = Mathf.Max(0, actualDrain);
							otherActor.data.mana -= Mathf.RoundToInt(actualDrain);
							effectAppliedToAnyEnemy = true;
						}
					}
				}
			}
			// 施法者恢復相應屬性 - 改為百分比恢復自身屬性
			if (effectAppliedToAnyEnemy) // 只有當有敵人被成功吸取時才恢復
			{
				// 生命值恢復
				int healthToRestore = Mathf.RoundToInt(selfActor.getMaxHealth() * selfHealPercentage);
				if (healthToRestore > 0)
				{
					selfActor.restoreHealth(healthToRestore);
					// Debug.Log($"SelfActor recovered {healthToRestore} Health ({selfHealPercentage*100}%). Current: {selfActor.data.health}");
				}
				// 法力恢復
				int manaToRestore = Mathf.RoundToInt(selfActor.getMaxMana() * selfManaRestorePercentage);
				if (manaToRestore > 0)
				{
					selfActor.data.mana += manaToRestore;
					// Debug.Log($"SelfActor recovered {manaToRestore} Mana ({selfManaRestorePercentage*100}%). Current: {selfActor.data.mana}");
				}
				// 耐力恢復
				int staminaToRestore = Mathf.RoundToInt(selfActor.getMaxStamina() * selfStaminaRestorePercentage);
				if (staminaToRestore > 0)
				{
					selfActor.data.stamina += staminaToRestore;
					// Debug.Log($"SelfActor recovered {staminaToRestore} Stamina ({selfStaminaRestorePercentage*100}%). Current: {selfActor.data.stamina}");
				}
			}
			return effectAppliedToAnyEnemy; // 如果成功對至少一個敵人應用了效果，則返回 true
		}

		private static Dictionary<long, int> cdtSleepingCount = new Dictionary<long, int>();
		public static bool SleepingLaw(BaseSimObject pSelf, WorldTile pTile)
		{// 睡夢法 (新) 與 spawnSlothApostleEffect 效果連動
			// 1. 基本安全檢查
			if (pSelf?.a == null || pTile == null)
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			long actorId = selfActor.data.id;
			string sleepingStatusID = "sleeping";
			string cooldownStatusID = "cdt_sleeping";
			float sleepingDuration = 600f;
			float cooldownDuration = 300f;
			float maxRange = 160f;
			int SLEEPING_COUNT_TO_ACTIVATE_NEXT_STAGE = 3;
			bool effectAppliedToAnyone = false;
			// === 修正後的邏輯：如果施法者處於冷卻中，則直接返回，不執行後續施法 ===
			if (selfActor.hasStatus(cooldownStatusID))
			{
				return false;
			}
			if (selfActor.subspecies == null || !selfActor.subspecies.hasTrait("prefrontal_cortex"))
			{
				return false;
			}
			// === 邏輯：遍歷所有單位，鎖定敵對目標並施加效果 ===
			foreach (var other in World.world.units)
			{
				// 確保單位有效、不是自己、存活
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				// 檢查是否為敵對單位
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				// 檢查是否在射程內
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist < maxRange)
				{
					Actor targetActor = other;
					// 檢查目標是否已經擁有 "sleeping" 狀態效果
					if (!targetActor.hasStatus(sleepingStatusID))
					{
						targetActor.addStatusEffect(sleepingStatusID, sleepingDuration);
						effectAppliedToAnyone = true;
					}
				}
			}
			// === 修正後的邏輯：只有在成功施法後，才更新計數器並判斷是否觸發能力 ===
			if (effectAppliedToAnyone)
			{
				if (cdtSleepingCount.ContainsKey(actorId))
				{
					cdtSleepingCount[actorId]++;
				}
				else
				{
					cdtSleepingCount[actorId] = 1;
				}
				if (cdtSleepingCount[actorId] >= SLEEPING_COUNT_TO_ACTIVATE_NEXT_STAGE)
				{
					// === 新增：檢查王國是否已存在魔王或擁有 'other666' 特質的單位 ===
					bool isDemonKingExists = false;
					if (selfActor.kingdom != null)
					{
						foreach (Actor kingdomUnit in selfActor.kingdom.units)
						{
							if (kingdomUnit == null) continue;

							// 優先檢查是否有 'other666' 特質
							if (kingdomUnit.hasTrait("hope")||
								kingdomUnit.hasTrait("other6661")||
								kingdomUnit.hasTrait("other6662")||
								kingdomUnit.hasTrait("other6663")||
								kingdomUnit.hasTrait("other6664")||
								kingdomUnit.hasTrait("other6665")||
								kingdomUnit.hasTrait("other6666")||
								kingdomUnit.hasTrait("other6667")||
								kingdomUnit.hasTrait("other6668")||
								kingdomUnit.hasTrait("other6669"))
							{
								isDemonKingExists = true;
								break;
							}
							
							// 接著檢查是否有魔王狀態
							foreach (string demonKingStatusID in SevenDemonKingStatus_DemonKing)
							{
								if (kingdomUnit.hasStatus(demonKingStatusID))
								{
									isDemonKingExists = true;
									break;
								}
							}
							if (isDemonKingExists)
							{
								break;
							}
						}
					}
					// 只有在沒有魔王的情況下才執行生成,並對計數器執行重置
					if (!isDemonKingExists)
					{
						spawnSlothApostleEffect(pSelf, pTile);
						cdtSleepingCount.Remove(actorId);	
					}
				//selfActor.addStatusEffect(cooldownStatusID, cooldownDuration);
				}
				else
				{
					// 如果還沒達到閾值，則為施法者添加冷卻狀態
					selfActor.addStatusEffect(cooldownStatusID, cooldownDuration);
				}
			}
			return effectAppliedToAnyone;
		}
		private static readonly HashSet<string> AdamantineWeaponIDs = new HashSet<string>
		{// Adamantine武器清單
			"sword_adamantine",
			"axe_adamantine",
			"hammer_adamantine",
			"spear_adamantine",
			"bow_adamantine",
		};
		public static bool spawnSlothApostleEffect00(BaseSimObject pSelf/*, BaseSimObject pTarget*/, WorldTile pTile = null)
		{// 睡夢法 生成 怠惰使徒
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			const string slothDemonKingStatusID = "sloth_demon_king";
			//const string sleepingStatusID = "sleeping";
			//const float sleepDuration = 3600f;
			const float kingDuration = 3600f;
			if (selfActor.hasStatus(slothDemonKingStatusID))
			{
				// 如果已經有此狀態，則不執行生成，直接返回 false
				return false;
			}
			// 設定生成生物
			string unitToSummon01 = selfActor.asset.id;
			// 呼喚與施法者同種生物的輸入法為 selfActor.asset.id;
			// 呼喚特定生物的方法為 "生物ID";
			int NumberOfMobsSpawn01 = 40;
			// 設定每次觸發要生成的單位數量
			//const string Weapon_ID_01 = "AdamantineWeaponIDs";
			const string Helmet_ID_01 = "helmet_adamantine";
			const string Armor_ID_01 = "armor_adamantine";
			const string Boots_ID_01 = "boots_adamantine";
			const string Ring_ID_01 = "ring_adamantine";
			const string Amulet_ID_01 = "amulet_adamantine";
			for (int i = 0; i < NumberOfMobsSpawn01; i++)
			{
				var act01 = World.world.units.createNewUnit(unitToSummon01, pTile);		//生成的生物種類
				if (act01 == null)
				{
					continue;
				}
				act01.setKingdom(pSelf.kingdom);											//加入施法者王國
				act01.data.sex = selfActor.data.sex;											//性別與施法者同步
				act01.setLover(selfActor);														//將施法者設定為戀人
				act01.addTrait("apostle");													// 添加 / 移除 removeTrait
				act01.removeTrait("b0001");													// 添加 / 移除 removeTrait
				act01.addStatusEffect("apostle_se", 3600);									//添加狀態
				act01.finishStatusEffect("egg");											//添加狀態
				act01.finishStatusEffect("uprooting");										//添加狀態
				act01.a.data.age_overgrowth += 15;											//年齡過度生長
				act01.data.set("master_id", pSelf.a.data.id);								//紀錄主人ID
				act01.data.name = $"Apostle of {pSelf.a.getName()}";						//修改名稱
				act01.goTo(pSelf.current_tile);												//到主人身邊
				var weaponIDsList = new List<string>(AdamantineWeaponIDs);
				int randomIndex = _random.Next(0, weaponIDsList.Count);
				string randomWeaponId = weaponIDsList[randomIndex];
				var weaponAsset = AssetManager.items.get(randomWeaponId);					//武器
				if (weaponAsset != null)
				{
				var weaponInstance = World.world.items.generateItem(pItemAsset: weaponAsset);
				if (weaponInstance != null)
					{	// 在這裡添加修飾符
						weaponInstance.addMod("ice");
						weaponInstance.addMod("power5");
						weaponInstance.addMod("balance5");
						weaponInstance.addMod("sharpness5");
						act01.equipment.getSlot(EquipmentType.Weapon).setItem(weaponInstance, act01);
					}
				}
				var helmetAsset = AssetManager.items.get(Helmet_ID_01);						//頭盔
				if (helmetAsset != null)
				{
					var helmetInstance = World.world.items.generateItem(pItemAsset: helmetAsset);
					if (helmetInstance != null)
					{
						helmetInstance.addMod("truth5");
						helmetInstance.addMod("health5");
						helmetInstance.addMod("knowledge5");
						act01.equipment.getSlot(EquipmentType.Helmet).setItem(helmetInstance, act01);
					}
				}
				var armorAsset = AssetManager.items.get(Armor_ID_01);						//鎧甲
				if (armorAsset != null)
				{
					var armorInstance = World.world.items.generateItem(pItemAsset: armorAsset);
					if (armorInstance != null)
					{
						armorInstance.addMod("protection5");
						armorInstance.addMod("protection4");
						armorInstance.addMod("health5");
						act01.equipment.getSlot(EquipmentType.Armor).setItem(armorInstance, act01);
					}
				}
				var bootsAsset = AssetManager.items.get(Boots_ID_01);						//靴子
				if (bootsAsset != null)
				{
					var bootsInstance = World.world.items.generateItem(pItemAsset: bootsAsset);
					if (bootsInstance != null)
					{
						bootsInstance.addMod("speed5");
						bootsInstance.addMod("speed4");
						bootsInstance.addMod("speed3");
						act01.equipment.getSlot(EquipmentType.Boots).setItem(bootsInstance, act01);
					}
				}
				var ringAsset = AssetManager.items.get(Ring_ID_01);							//戒指
				if (ringAsset != null)
				{
					var ringInstance = World.world.items.generateItem(pItemAsset: ringAsset);
					if (ringInstance != null)
					{
						ringInstance.addMod("protection5");
						ringInstance.addMod("finesse5");
						ringInstance.addMod("mastery5");
						act01.equipment.getSlot(EquipmentType.Ring).setItem(ringInstance, act01);
					}
				}
				var amuletAsset = AssetManager.items.get(Amulet_ID_01);						//護符
				if (amuletAsset != null)
				{
					var amuletInstance = World.world.items.generateItem(pItemAsset: amuletAsset);
					if (amuletInstance != null)
					{
						amuletInstance.addMod("protection5");
						amuletInstance.addMod("finesse5");
						amuletInstance.addMod("mastery5");
						act01.equipment.getSlot(EquipmentType.Amulet).setItem(amuletInstance, act01);
					}
				}
				if (!listOfTamedBeasts.ContainsKey(act01))
				{listOfTamedBeasts.Add(act01, pSelf.a);}									//添加到清單
			}
			// 設定生成生物 02
			string unitToSummon02 = selfActor.asset.id;
			// 呼喚與施法者同種生物的輸入法為 selfActor.asset.id;
			// 呼喚特定生物的方法為 "生物ID";
			int NumberOfMobsSpawn02 = 10;
			// 設定每次觸發要生成的單位數量
			//const string Weapon_ID = "AdamantineWeaponIDs";
			const string Helmet_ID_02 = "helmet_adamantine";
			const string Armor_ID_02 = "armor_adamantine";
			const string Boots_ID_02 = "boots_adamantine";
			const string Ring_ID_02 = "ring_adamantine";
			const string Amulet_ID_02 = "amulet_adamantine";
			for (int i = 0; i < NumberOfMobsSpawn02; i++)
			{
				var act02 = World.world.units.createNewUnit(unitToSummon02, pTile);
				if (act02 == null)
				{
					continue;
				}
				act02.setKingdom(pSelf.kingdom);											//加入施法者王國
				act02.data.sex = selfActor.data.sex;											//性別與施法者同步
				act02.setLover(selfActor);														//將施法者設定為戀人
				act02.addTrait("apostle");													// 添加 / 移除 removeTrait
				act02.addTrait("pro_soldier");												// 添加 / 移除 removeTrait
				act02.removeTrait("b0001");													// 添加 / 移除 removeTrait
				act02.addStatusEffect("apostle_se", 3600);									//添加狀態
				act02.finishStatusEffect("egg");											//結束狀態
				act02.finishStatusEffect("uprooting");										//結束狀態
				act02.a.data.age_overgrowth += 15;											//年齡過度生長
				act02.data.set("master_id", pSelf.a.data.id);								//紀錄主人ID
				act02.data.name = $"Apostle of {pSelf.a.getName()}";						//修改名稱
				act02.goTo(pSelf.current_tile);												//到主人身邊
				var weaponIDsList = new List<string>(AdamantineWeaponIDs);
				int randomIndex = _random.Next(0, weaponIDsList.Count);
				string randomWeaponId = weaponIDsList[randomIndex];
				var weaponAsset = AssetManager.items.get(randomWeaponId);//武器
				if (weaponAsset != null)
				{
				var weaponInstance = World.world.items.generateItem(pItemAsset: weaponAsset);
				if (weaponInstance != null)
					{	// 在這裡添加修飾符
						weaponInstance.addMod("ice");
						weaponInstance.addMod("power5");
						weaponInstance.addMod("balance5");
						weaponInstance.addMod("sharpness5");
						act02.equipment.getSlot(EquipmentType.Weapon).setItem(weaponInstance, act02);
					}
				}
				var helmetAsset = AssetManager.items.get(Helmet_ID_01);						//頭盔
				if (helmetAsset != null)
				{
					var helmetInstance = World.world.items.generateItem(pItemAsset: helmetAsset);
					if (helmetInstance != null)
					{
						helmetInstance.addMod("truth5");
						helmetInstance.addMod("health5");
						helmetInstance.addMod("knowledge5");
						act02.equipment.getSlot(EquipmentType.Helmet).setItem(helmetInstance, act02);
					}
				}
				var armorAsset = AssetManager.items.get(Armor_ID_01);						//鎧甲
				if (armorAsset != null)
				{
					var armorInstance = World.world.items.generateItem(pItemAsset: armorAsset);
					if (armorInstance != null)
					{
						armorInstance.addMod("protection5");
						armorInstance.addMod("protection4");
						armorInstance.addMod("health5");
						act02.equipment.getSlot(EquipmentType.Armor).setItem(armorInstance, act02);
					}
				}
				var bootsAsset = AssetManager.items.get(Boots_ID_01);						//靴子
				if (bootsAsset != null)
				{
					var bootsInstance = World.world.items.generateItem(pItemAsset: bootsAsset);
					if (bootsInstance != null)
					{
						bootsInstance.addMod("speed5");
						bootsInstance.addMod("speed4");
						bootsInstance.addMod("speed3");
						act02.equipment.getSlot(EquipmentType.Boots).setItem(bootsInstance, act02);
					}
				}
				var ringAsset = AssetManager.items.get(Ring_ID_01);							//戒指
				if (ringAsset != null)
				{
					var ringInstance = World.world.items.generateItem(pItemAsset: ringAsset);
					if (ringInstance != null)
					{
						ringInstance.addMod("protection5");
						ringInstance.addMod("finesse5");
						ringInstance.addMod("mastery5");
						act02.equipment.getSlot(EquipmentType.Ring).setItem(ringInstance, act02);
					}
				}
				var amuletAsset = AssetManager.items.get(Amulet_ID_01);						//護符
				if (amuletAsset != null)
				{
					var amuletInstance = World.world.items.generateItem(pItemAsset: amuletAsset);
					if (amuletInstance != null)
					{
						amuletInstance.addMod("protection5");
						amuletInstance.addMod("finesse5");
						amuletInstance.addMod("mastery5");
						act02.equipment.getSlot(EquipmentType.Amulet).setItem(amuletInstance, act02);
					}
				}
				if (!listOfTamedBeasts.ContainsKey(act02))
				{listOfTamedBeasts.Add(act02, pSelf.a);}									//添加到清單
			}
			//selfActor.addStatusEffect(sleepingStatusID, sleepDuration);
			selfActor.addStatusEffect(slothDemonKingStatusID, kingDuration);
			EvilStickGet(pSelf, pTile);
			return true; // 只要成功生成，就返回 true
		}
		public static bool spawnSlothApostleEffect(BaseSimObject pSelf/*, BaseSimObject pTarget*/, WorldTile pTile = null)
		{// 睡夢法 生成 怠惰使徒 執行
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			const string slothDemonKingStatusID = "sloth_demon_king";
			const float kingDuration = 3600f;
			if (selfActor.hasStatus(slothDemonKingStatusID))
			{
				return false;
			}
			// 設定生成生物
			string unitToSummon = selfActor.asset.id;
			int numberOfCommonApostles = 80;	//一般使徒
			int numberOfProSoldiers = 20;		//士兵使徒
			// 呼叫輔助方法生成第一批使徒
			for (int i = 0; i < numberOfCommonApostles; i++)
			{
				createSlothApostle(selfActor, unitToSummon, pTile, false);
			}
			// 呼叫輔助方法生成第二批使徒（pro_soldier）
			for (int i = 0; i < numberOfProSoldiers; i++)
			{
				createSlothApostle(selfActor, unitToSummon, pTile, true);
			}
			selfActor.addStatusEffect(slothDemonKingStatusID, kingDuration);
			EvilStickGet(pSelf, pTile);
			Items01Actions.EvilStickAwakens(pSelf, pTile);
			return true;
		}
		private static void createSlothApostle(Actor pMaster, string pUnitId, WorldTile pTile, bool pIsProSoldier)
		{// 睡夢法 怠惰使徒主要內容
			var newApostle = World.world.units.createNewUnit(pUnitId, pTile);
			if (newApostle == null)
			{
				return;
			}
			newApostle.setKingdom(pMaster.kingdom);						//加入主人王國
			newApostle.data.sex = pMaster.data.sex;						//同步主人性別
			newApostle.setLover(pMaster);								//將主人設為戀人
			newApostle.addTrait("apostle");								//添加特質
			newApostle.removeTrait("b0001");							//移除特質
			newApostle.addStatusEffect("apostle_se", 3600);				//添加狀態 (存續倒計時)
			newApostle.finishStatusEffect("egg");						//移除狀態
			newApostle.finishStatusEffect("uprooting");					//移除狀態
			newApostle.a.data.age_overgrowth += 15;						//加速成長
			newApostle.data.set("master_id", pMaster.data.id);			//登陸主人ID
			newApostle.data.name = $"Apostle of {pMaster.getName()}";	//修改名稱
			newApostle.goTo(pMaster.current_tile);						//生成位置
			// 如果是 pro_soldier，則添加特質
			if (pIsProSoldier)
			{
				newApostle.addTrait("pro_soldier");
			}
			// --- 穿戴裝備 ---
			const string Helmet_ID_01 = "helmet_adamantine";
			const string Armor_ID_01 = "armor_adamantine";
			const string Boots_ID_01 = "boots_adamantine";
			const string Ring_ID_01 = "ring_adamantine";
			const string Amulet_ID_01 = "amulet_adamantine";
			var weaponIDsList = new List<string>(AdamantineWeaponIDs);
			int randomIndex = _random.Next(0, weaponIDsList.Count);
			string randomWeaponId = weaponIDsList[randomIndex];
			//武器 從 AdamantineWeaponIDs 中隨機選取
			var weaponAsset = AssetManager.items.get(randomWeaponId);
			if (weaponAsset != null)
			{
				var weaponInstance = World.world.items.generateItem(pItemAsset: weaponAsset);
				if (weaponInstance != null)
				{
					weaponInstance.addMod("ice");
					weaponInstance.addMod("power5");
					weaponInstance.addMod("balance5");
					weaponInstance.addMod("sharpness5");
					newApostle.equipment.getSlot(EquipmentType.Weapon).setItem(weaponInstance, newApostle);
				}
			}
			// 頭盔
			var helmetAsset = AssetManager.items.get(Helmet_ID_01);
			if (helmetAsset != null)
			{
				var helmetInstance = World.world.items.generateItem(pItemAsset: helmetAsset);
				if (helmetInstance != null)
				{
					helmetInstance.addMod("truth5");
					helmetInstance.addMod("health5");
					helmetInstance.addMod("knowledge5");
					newApostle.equipment.getSlot(EquipmentType.Helmet).setItem(helmetInstance, newApostle);
				}
			}
			// 鎧甲
			var armorAsset = AssetManager.items.get(Armor_ID_01);
			if (armorAsset != null)
			{
				var armorInstance = World.world.items.generateItem(pItemAsset: armorAsset);
				if (armorInstance != null)
				{
					armorInstance.addMod("protection5");
					armorInstance.addMod("protection4");
					armorInstance.addMod("health5");
					newApostle.equipment.getSlot(EquipmentType.Armor).setItem(armorInstance, newApostle);
				}
			}
			// 靴子
			var bootsAsset = AssetManager.items.get(Boots_ID_01);
			if (bootsAsset != null)
			{
				var bootsInstance = World.world.items.generateItem(pItemAsset: bootsAsset);
				if (bootsInstance != null)
				{
					bootsInstance.addMod("speed5");
					bootsInstance.addMod("speed4");
					bootsInstance.addMod("speed3");
					newApostle.equipment.getSlot(EquipmentType.Boots).setItem(bootsInstance, newApostle);
				}
			}
			// 戒指
			var ringAsset = AssetManager.items.get(Ring_ID_01);
			if (ringAsset != null)
			{
				var ringInstance = World.world.items.generateItem(pItemAsset: ringAsset);
				if (ringInstance != null)
				{
					ringInstance.addMod("protection5");
					ringInstance.addMod("finesse5");
					ringInstance.addMod("mastery5");
					newApostle.equipment.getSlot(EquipmentType.Ring).setItem(ringInstance, newApostle);
				}
			}
			// 護符
			var amuletAsset = AssetManager.items.get(Amulet_ID_01);
			if (amuletAsset != null)
			{
				var amuletInstance = World.world.items.generateItem(pItemAsset: amuletAsset);
				if (amuletInstance != null)
				{
					amuletInstance.addMod("protection5");
					amuletInstance.addMod("finesse5");
					amuletInstance.addMod("mastery5");
					newApostle.equipment.getSlot(EquipmentType.Amulet).setItem(amuletInstance, newApostle);
				}
			}
			// 添加到列表
			if (!listOfTamedBeasts.ContainsKey(newApostle))
			{listOfTamedBeasts.Add(newApostle, pMaster);}
		}
		public static bool LoverNull(BaseSimObject pTarget, WorldTile pTile = null)
		{// 睡夢法 清除與使徒的戀人關係
			// 1. 基本安全檢查
			if (pTarget?.a == null)
			{
				return false;
			}
			
			Actor targetActor = pTarget.a;
			
			// 2. 檢查目標單位是否有戀人
			if (targetActor.data.lover == 0)
			{
				// 如果沒有戀人，則沒有需要處理的邏輯
				return false;
			}
			
			// 3. 獲取戀人單位
			Actor lover = World.world.units.get(targetActor.data.lover);
			
			// 4. 檢查戀人單位是否存在且是否持有 "apostle" 特質
			if (lover != null && lover.hasTrait("apostle"))
			{
				// 如果戀人是使徒，則清除戀人關係
				targetActor.setLover(null);
				return true;
			}
			
			return false;
		}
		public static bool EvilStickGet(BaseSimObject pTarget, WorldTile pTile = null)
		{// 睡夢法 武器給予
			// 定義所需的狀態ID常量
			const string DemonKingStatus = "sloth_demon_king"; //魔王狀態
			const string WeapontID = "evil_stick"; // 物品ID
			// 1. 基本安全檢查：目標是否有效且存活
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor targetActor = pTarget.a;
			// ****** 新增檢查：單位是否擁有 '指定魔王狀態' 狀態 ******
			if (!targetActor.hasStatus(DemonKingStatus))
			{
				return false; // 如果沒有此狀態，則不給予裝備
			}
			// ************************************************************
			// 獲取目標的武器槽位
			var weaponSlot = targetActor.equipment.getSlot(EquipmentType.Weapon);
			// 檢查武器槽位是否已被佔用，且其中持有的道具ID是否為 "指定武器"
			if (weaponSlot != null && weaponSlot.getItem() != null && weaponSlot.getItem().asset.id == WeapontID)
			{
				// 額外檢查：如果武器沒有修飾符，在這裡補上
				Item existingWeapon = weaponSlot.getItem();
				/*if (!existingWeapon.hasMod("eternal"))
				{
					existingWeapon.addMod("eternal");
					existingWeapon.addMod("cursed");
				}*/
				return true;
			}
			// 如果單位沒有持有 "指定武器" 且擁有指定狀態，則繼續進行裝備流程
			// 獲取 "指定武器" 物品資產
			var weaponAsset = AssetManager.items.get(WeapontID);
			if (weaponAsset == null)
			{
				return false; // 無法找到物品資產，返回失敗
			}
			// 生成 "指定武器" 物品實例
			var newItem = World.world.items.generateItem(pItemAsset: weaponAsset);
			// =======================================================
			// === 核心修正：強制添加修飾符 ===
			if (newItem != null)
			{
				newItem.addMod("ice");
				newItem.addMod("power5");
				newItem.addMod("truth5");
				newItem.addMod("protection5");
				newItem.addMod("speed5");
				newItem.addMod("balance5");
				newItem.addMod("health5");
				newItem.addMod("finesse5");
				newItem.addMod("mastery5");
				newItem.addMod("knowledge5");
				newItem.addMod("sharpness5");
			}
			// =======================================================
			// 將 "指定武器" 裝備到目標的武器槽位
			weaponSlot.setItem(newItem, targetActor);
			// 更新目標的統計數據，以反映新裝備帶來的變化
			targetActor.setStatsDirty();
			targetActor.data.health += 99999;
			Items01Actions.addFavoriteWeapon1(pTarget, pTile);
			return true; // 表示效果成功施加
		}
	// 使徒
		public static bool ApostleUnit(BaseSimObject pTarget, WorldTile pTile = null)
		{// 使徒效果 (主要效果)
			// 1. 初始安全檢查
			if (pTarget?.a == null || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor apostle = pTarget.a;
			// 2. 核心邏輯：單次物種修正 (保持不變)
			if (apostle.hasTrait("apostle") && apostle.subspecies != null && (!apostle.hasTrait("battle_reflexes") || !apostle.hasTrait("arcane_reflexes")))
			{
				string childAssetId = apostle.asset.id;
				long childSubspeciesId = apostle.subspecies.id;
				
				if (childAssetId != childSubspeciesId.ToString())
				{
					Subspecies correctSubspecies = MapBox.instance.subspecies.get(childSubspeciesId);
					if (correctSubspecies != null)
					{
						apostle.setSubspecies(correctSubspecies);
					}
				}
				apostle.addTrait("arcane_reflexes");
				apostle.addTrait("battle_reflexes");
				apostle.addTrait("dash");
				apostle.addTrait("block");
				apostle.addTrait("backstep");
				apostle.addTrait("deflect_projectile");
				apostle.addTrait("mute");
			}
			// 3. 檢查並從 custom_data 恢復主人關係
			Actor master = null;
			if (Main.listOfTamedBeasts.ContainsKey(apostle))
			{
				master = Main.listOfTamedBeasts[apostle];
			}
			else if (apostle.data != null && apostle.data.custom_data_long != null &&
					 apostle.data.custom_data_long.TryGetValue("master_id", out long masterId))
			{
				master = World.world.units.get(masterId);
				if (master != null)
				{
					Main.listOfTamedBeasts[apostle] = master;
				}
			}
			if (master != null)
			{
				// 只有當 master 存在時，才執行這些檢查和狀態添加
				if (master.hasTrait("other6661") && !apostle.hasStatus("apostle_se"))
				{//遊戲重載後的狀態再添加
					apostle.addStatusEffect("apostle_se", 3600f);
				}
				if (master.hasTrait("other6662") && !apostle.hasStatus("apostle_se"))
				{//遊戲重載後的狀態再添加
					apostle.addStatusEffect("apostle_se", 3200f);
				}
				if (master.hasTrait("other6663") && !apostle.hasStatus("apostle_se"))
				{//遊戲重載後的狀態再添加
					apostle.addStatusEffect("apostle_se", 2800f);
				}
				if (master.hasTrait("other6664") && !apostle.hasStatus("apostle_se"))
				{//遊戲重載後的狀態再添加
					apostle.addStatusEffect("apostle_se", 2400f);
				}
				if (master.hasTrait("other6665") && !apostle.hasStatus("apostle_se"))
				{//遊戲重載後的狀態再添加
					apostle.addStatusEffect("apostle_se", 2000f);
				}
				if (master.hasTrait("other6666") && !apostle.hasStatus("apostle_se"))
				{//遊戲重載後的狀態再添加
					apostle.addStatusEffect("apostle_se", 1600f);
				}
				if (master.hasTrait("other6667") && !apostle.hasStatus("apostle_se"))
				{//遊戲重載後的狀態再添加
					apostle.addStatusEffect("apostle_se", 1200f);
				}
				if (master.hasTrait("other6668") && !apostle.hasStatus("apostle_se"))
				{//遊戲重載後的狀態再添加
					apostle.addStatusEffect("apostle_se", 800f);
				}
				if (master.hasTrait("other6669") && !apostle.hasStatus("apostle_se"))
				{//遊戲重載後的狀態再添加
					apostle.addStatusEffect("apostle_se", 400f);
				}
			}
			// 如果沒有主人或主人已死亡，移除使徒
			if (master == null || !master.isAlive())
			{
				ActionLibrary.removeUnit(apostle);
				return false;
			}
			// 4. 主人存活時的屬性恢復邏輯
			const float HEALTH_RESTORE_AMOUNT = 0.01f;
			const float STAMINA_RESTORE_AMOUNT = 0.01f;
			const int NUTRITION_RESTORE_AMOUNT = 1;
			const int MAX_NUTRITION_VALUE = 100;
			// 健康恢復
			// 使用臨時變數作為 ref 參數的橋樑
			float masterHealth = master.data.health;
			float apostleHealth = apostle.data.health;
			RestoreAndDrainStat(master, apostle, 99, (int)Mathf.Round(master.getMaxHealth() * HEALTH_RESTORE_AMOUNT), ref masterHealth, ref apostleHealth, master.getMaxHealth());
			master.data.health = (int)masterHealth;
			apostle.data.health = (int)apostleHealth;
			// 耐力恢復
			// 使用臨時變數作為 ref 參數的橋樑
			float masterStamina = master.data.stamina;
			float apostleStamina = apostle.data.stamina;
			RestoreAndDrainStat(master, apostle, 99, (int)Mathf.Round(master.getMaxStamina() * STAMINA_RESTORE_AMOUNT), ref masterStamina, ref apostleStamina, master.getMaxStamina());
			master.data.stamina = (int)masterStamina;
			apostle.data.stamina = (int)apostleStamina;
			// 營養值恢復
			if (master.data.nutrition < MAX_NUTRITION_VALUE * 0.99f)
			{
				master.data.nutrition = Mathf.Min(MAX_NUTRITION_VALUE, master.data.nutrition + NUTRITION_RESTORE_AMOUNT);
				apostle.data.nutrition = Mathf.Max(0, apostle.data.nutrition - NUTRITION_RESTORE_AMOUNT);
			}
			//讓主人入睡
			if (master.hasTrait("other6662") || master.hasTrait("other6664") || master.hasTrait("other6666") || master.hasTrait("other6668"))
			{
				master.addStatusEffect("sleeping", 10f);
			}
			if (master.hasTrait("other6661") || master.hasTrait("other6663") || master.hasTrait("other6665") || master.hasTrait("other6667"))
			{
				master.finishStatusEffect("sleeping");
			}
			if (master.hasStatus("sleeping"))
			{
				// 嘗試將主人傳送回城市
				bool teleportSuccess = teleportToHomeCity(apostle, master, pTile);
				// 如果傳送**失敗**，可能是因為城市被毀、地點無效等。
				// 在這種情況下，讓魔王留在戰區睡覺太危險，應強制喚醒並清除易怒狀態。
				//if (!teleportSuccess && master.hasStatus("sleeping")) 
				//{
				//	master.addStatusEffect("antibody", 400f);
				//	apostle.goTo(master.current_tile);
				//}
			}
			if (master.hasStatus("tantrum") || master.hasStatus("angry"))
			{
				master.finishStatusEffect("tantrum");
				master.finishStatusEffect("angry");
			}
			if (master.data.happiness < 0)
			{
				master.data.happiness += 10;
			}
			// 二次檢查，如果不是使徒了，就返回
			if (!apostle.hasTrait("apostle"))
			{
				return false;
			}
			// === 5. 同步設定區 (保持不變) ===
			if (apostle.city != master.city)
			{
				if (master.city != null)
				{
					if (apostle.city != null) apostle.city.units.Remove(apostle);
					apostle.city = master.city;
					master.city.units.Add(apostle);
				}
				else if (apostle.city != null)
				{
					if (master.city != null) master.city.units.Remove(master);
					master.city = apostle.city;
					master.kingdom = apostle.kingdom;
					apostle.city.units.Add(master);
				}
			}
			if (apostle.kingdom != master.kingdom)
			{
				if (apostle.kingdom != null) apostle.kingdom.units.Remove(apostle);
				apostle.kingdom = master.kingdom;
				if (master.kingdom != null) master.kingdom.units.Add(apostle);
			}
			if (master.data.lover != null && master.data.lover == apostle.data.id)
			{
				master.setLover(null);
				master.finishStatusEffect("fell_in_love");
			}
			if (apostle.family != null) apostle.setFamily(null);
			if (apostle.religion != null) apostle.setReligion(null);
			if (apostle.language != null) apostle.setLanguage(null);
			if (apostle.isKing() && master.kingdom != null)
			{
				Kingdom apostleKingdom = apostle.kingdom;
				if (apostleKingdom != null && apostleKingdom.king == apostle)
				{
					apostle.setProfession(UnitProfession.Unit);
					apostle.removeTrait("pro_king");
					apostleKingdom.king = null;
				}
				Kingdom masterKingdom = master.kingdom;
				if (masterKingdom != null)
				{
					if (master.isCityLeader()) master.city.removeLeader();
					masterKingdom.king = master;
					master.setProfession(UnitProfession.King);
					WorldLog.logNewKing(masterKingdom);
					master.startShake();
					master.startColorEffect();
				}
			}
			if (apostle.hasTrait("pro_soldier"))
			{
				if (Randy.randomChance(0.90f)) apostle.goTo(master.current_tile);
			}
			else
			{
				if (Randy.randomChance(0.01f)) apostle.goTo(master.current_tile);
			}

			return true;
		}
		private static void RestoreAndDrainStat(Actor pMaster, Actor pApostle, float pRestorePercentage, int pRestoreAmount, ref float pMasterStat, ref float pApostleStat, float pMasterMaxStat)
		{// 使徒效果 (主要) 恢復輔助
			float triggerThreshold = pMasterMaxStat * (pRestorePercentage / 100f);
			// 檢查主人屬性是否低於恢復閾值
			if (pMasterStat < triggerThreshold)
			{
				// 恢復主人屬性，同時確保不超過上限
				pMasterStat = Mathf.Min(pMasterStat + pRestoreAmount, pMasterMaxStat);
				// 消耗使徒屬性，同時確保不低於下限
				pApostleStat = Mathf.Max(0, pApostleStat - pRestoreAmount);
			}
		}
		public static bool ApostleUnit2(BaseSimObject pSelf, WorldTile pTile = null)
		{// 使徒效果 對使徒的支援效果
			if (pSelf?.a == null || !pSelf.a.isAlive())
				return false;
			
			Actor selfApostle = pSelf.a; // 發動效果的使徒

			// === 基本安全檢查：確保發動者是活著的使徒 ===
			if (!selfApostle.hasTrait("apostle"))
				return false;
			
			// === 獲取發動者的主人 ===
			Actor selfMaster = null;
			if (Main.listOfTamedBeasts.ContainsKey(selfApostle))
			{
				selfMaster = Main.listOfTamedBeasts[selfApostle];
			}
			
			// 如果找不到主人，則使徒無法發動此支援效果
			if (selfMaster == null || !selfMaster.isAlive())
				return false;

			// === 尋找同一個主人旗下的其他使徒 ===
			List<Actor> otherApostles = new List<Actor>();
			foreach (var entry in Main.listOfTamedBeasts)
			{
				Actor apostle = entry.Key;
				Actor master = entry.Value;
				
				// 如果找到另一個使徒，且它的主人與發動者的主人是同一個
				if (apostle != selfApostle && master == selfMaster)
				{
					// === 確保使徒是活著的，並加入清單 ===
					if (apostle != null && apostle.isAlive())
					{
						otherApostles.Add(apostle);
					}
				}
			}

			// 如果沒有找到其他使徒，則無法發動支援效果
			if (otherApostles.Count == 0)
				return false;
				
			// === 核心邏輯：施加恢復效果和情緒控制給全部友軍使徒 ===
			// 恢復效果的參數
			const float HEALTH_RESTORE_PERCENTAGE = 0.01f;
			const float STAMINA_RESTORE_PERCENTAGE = 0.01f;
			
			foreach (Actor targetApostle in otherApostles)
			{
				// === 優化：將檢查邏輯合併到迴圈開頭 ===
				if (targetApostle == null || !targetApostle.isAlive() || !targetApostle.hasTrait("apostle") || targetApostle.data.name != selfApostle.data.name)
				{
					continue; // 跳過不符合條件的目標
				}
					
				// === 將情緒控制代碼移到這裡 ===
				if (!targetApostle.hasStatus("apostle_se2"))
				{
					targetApostle.addStatusEffect("apostle_se2", 1800f);
				}		
				if (targetApostle.hasStatus("tantrum") || targetApostle.hasStatus("angry"))
				{
					targetApostle.finishStatusEffect("tantrum");
					targetApostle.finishStatusEffect("angry");
					targetApostle.finishStatusEffect("apostle_se2");
					targetApostle.addStatusEffect("stunned", 2f);
				}
				
				// 恢復目標使徒的生命值
				float targetMaxHealth = targetApostle.getMaxHealth();
				float healthToRestore = targetMaxHealth * HEALTH_RESTORE_PERCENTAGE;
				if (targetApostle.data.health < targetMaxHealth)
				{
					targetApostle.restoreHealth(Mathf.RoundToInt(healthToRestore));
				}

				// 恢復目標使徒的耐力
				float targetMaxStamina = targetApostle.getMaxStamina();
				float staminaToRestore = targetMaxStamina * STAMINA_RESTORE_PERCENTAGE;
				if (targetApostle.data.stamina < targetMaxStamina)
				{
					targetApostle.data.stamina += Mathf.RoundToInt(staminaToRestore);
					if (targetApostle.data.stamina > targetMaxStamina)
					{
						targetApostle.data.stamina = (int)targetMaxStamina; 
					}
				}
			}
			
			return true; // 表示效果成功執行
		}
		public static bool removefell_in_love(BaseSimObject pSelf, WorldTile pTile = null)
		{// 使徒效果 (移除狀態)
			// 1. 基本安全检查：确保 pSelf 及其 Actor 组件存在且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) 
				return false;
			Actor selfActor = pSelf.a;
			// 2. 特质持有者自身添加条件 
			selfActor.finishStatusEffect("fell_in_love");//
			selfActor.finishStatusEffect("tantrum");//
			selfActor.finishStatusEffect("angry");//
			return false; // 特殊效果通常返回 false，表示不阻止其他效果链
		}
		public static bool ApostleUnitEND(BaseSimObject pTarget, WorldTile pTile = null)
		{// 使徒效果 (生成終了)
			// 1. 基本安全檢查：確保目標存在且為有效的 Actor 實例
			//	使用 pTarget?.a 進行 null 傳播檢查，簡化了程式碼
			if (pTarget?.a == null)
			{
				return false;
			}
			
			Actor apostle = pTarget.a;
			
			// 2. 核心邏輯：檢查使徒是否擁有特定狀態
			//	如果使徒擁有 "apostle_se" 狀態，則不執行移除，直接返回 false
			if (apostle.hasStatus("apostle_se"))
			{
				return false;
			}
			
			// 3. 執行移除邏輯
			//	如果沒有上述狀態，則移除使徒單位
			ActionLibrary.removeUnit(apostle);
			
			return true;
		}

		public static bool Severe_WinterATK(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 睡夢法 嚴冬 (攻)
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			Severe_Winter10(pSelf, pTile);
			Severe_Winter20(pSelf, pTile);
			Severe_Winter30(pSelf, pTile);
			return true;
		}
		public static bool Severe_Winter10(BaseSimObject pTarget, WorldTile pTile = null)
		{// 睡夢法 怠惰使徒 嚴冬
			if (pTarget == null || !pTarget.isActor() || !pTarget.a.isAlive())
				return false;
			Actor selfActor = pTarget.a;
			
			// === 狀態檢查：必須擁有其中一個狀態才能發動 ===
			if (selfActor.hasStatus("sloth_demon_king") || selfActor.hasStatus("apostle_se"))
			{
				int COLD_AURA_RADIUS = 10;
				World.world.loopWithBrush(pTarget.current_tile,
				Brush.get(COLD_AURA_RADIUS, "circ_"),
				new PowerActionWithID(Traits01Actions.Severe_Winter_Assist),
				null);
				return true;
			}
			
			return false;
		}
		public static bool Severe_Winter20(BaseSimObject pTarget, WorldTile pTile = null)
		{// 睡夢法 怠惰使徒 嚴冬
			if (pTarget == null || !pTarget.isActor() || !pTarget.a.isAlive())
				return false;
			Actor selfActor = pTarget.a;
			
			// === 狀態檢查：必須擁有其中一個狀態才能發動 ===
			if (selfActor.hasStatus("sloth_demon_king") || selfActor.hasStatus("apostle_se"))
			{
				int COLD_AURA_RADIUS = 20;
				World.world.loopWithBrush(pTarget.current_tile,
				Brush.get(COLD_AURA_RADIUS, "circ_"),
				new PowerActionWithID(Traits01Actions.Severe_Winter_Assist),
				null);
				return true;
			}
			
			return false;
		}
		public static bool Severe_Winter30(BaseSimObject pTarget, WorldTile pTile = null)
		{// 睡夢法 怠惰使徒 嚴冬
			if (pTarget == null || !pTarget.isActor() || !pTarget.a.isAlive())
				return false;
			Actor selfActor = pTarget.a;
			
			// === 狀態檢查：必須擁有其中一個狀態才能發動 ===
			if (selfActor.hasStatus("sloth_demon_king") || selfActor.hasStatus("apostle_se"))
			{
				int COLD_AURA_RADIUS = 30;
				World.world.loopWithBrush(pTarget.current_tile,
				Brush.get(COLD_AURA_RADIUS, "circ_"),
				new PowerActionWithID(Traits01Actions.Severe_Winter_Assist),
				null);
				return true;
			}
			
			return false;
		}
		public static bool Severe_Winter_Assist(WorldTile pTile, string pPower)
		{// 睡夢法 怠惰使徒 嚴冬 輔助代碼
			int Freezing_degree = 999999999;
			int Temperature_control = -999999999;//溫度兼熱量控制
			if (pTile.Type.lava)
			{
				LavaHelper.coolDownLava(pTile);
			}
			if (pTile.isOnFire())
			{
				pTile.stopFire();
			}
			if (pTile.canBeFrozen() && Randy.randomBool())
			{
				if (pTile.health > 0)
				{
					pTile.health--;
				}
				else
				{
					pTile.freeze(Freezing_degree);
				}
			}
			WorldBehaviourUnitTemperatures.checkTile(pTile, Temperature_control);
			pTile.heat = Temperature_control;
			if (pTile.hasBuilding())
			{
				ActionLibrary.addFrozenEffectOnTarget(null, pTile.building, null);
			}
			if (pTile.hasBuilding() && pTile.building.asset.spawn_drops)
			{
				pTile.building.data.addFlag("stop_spawn_drops");
			}
			return true;
		}
		public static bool ApostleUnitATK(BaseSimObject pSelf, WorldTile pTile = null)
		{// 睡夢法 怠惰使徒 常態攻擊 (通常狀態 環狀集火)
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a; // 攻擊發動者}
			// === 新增檢查：必須擁有 'gluttony_demon_king' 狀態才能發動 ===
			if (!selfActor.hasStatus("apostle_se"))
			{
				return false;
			}
			// === 冷卻狀態檢查 ===
			string attackCooldownStatus = "item_cdt00";
			float attackCooldownDuration = 0.01f;
			if (selfActor.hasStatus(attackCooldownStatus))
			{
				return false;
			}
			// === 自動尋找目標 ===
			float maxRange = 15f;
			Actor target = null;
			float closestDist = float.MaxValue;
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist < maxRange && dist < closestDist)
				{
					closestDist = dist;
					target = other;
				}
			}
			if (target == null)
			{
				return false;
			}
			// === 成功找到目標，施加冷卻並發射投射物 ===
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			UnityEngine.Vector3 selfPosition = selfActor.current_position;
			UnityEngine.Vector3 targetPosition = target.current_position;
			// 計算發射物的基礎發射點，並向外擴展
			UnityEngine.Vector3 baseLaunchPoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, targetPosition.x, targetPosition.y, selfActor.stats["size"]);
			baseLaunchPoint.y += 0.5f;
			// 子彈相關設定 - 環狀攻擊
			int numberOfProjectiles_00 = 3;	//子彈數量
			float spreadDistance_00 = 5.0f; // 施法者和發射點的間距
			float spreadAngle_00 = 360.0f; // 投射物發射點的散布角度
			float totalSpread_00 = spreadAngle_00 / numberOfProjectiles_00; 
			for (int i = 0; i < numberOfProjectiles_00; i++)
			{	// 計算每個投射物的發射角度
				float angle = totalSpread_00 * i - spreadAngle_00 / 2f;
				// 根據角度和散布距離，計算每個投射物的發射位置
				UnityEngine.Vector3 launchPosition = baseLaunchPoint + Quaternion.Euler(0f, 0f, angle) * new Vector3(spreadDistance_00, 0, 0);
				World.world.projectiles.spawn(
					pInitiator: selfActor,
					pTargetObject: target,
					pAssetID: "Snowflake",
					pLaunchPosition: launchPosition,
					pTargetPosition: targetPosition, // 所有投射物都射向同一個目標點
					pTargetZ: 0.0f
				);
			}
			return true;
		}
		public static bool ApostleUnitCTK(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 睡夢法 怠惰使徒 子彈反擊
			if (pTarget != null)
			{
				Actor selfActor = pSelf as Actor;
				Actor targetActor = pTarget as Actor;
				if (selfActor == null)
				{
					return false;
				}
				// === 新增：冷卻狀態檢查邏輯 ===
				string attackCooldownStatus = "item_cdt01";//
				float attackCooldownDuration = 0.01f; // 調整這個值來設定冷卻時間，例如 1.5 秒
				// 檢查單位是否持有指定狀態
				if (!selfActor.hasStatus("apostle_se"))
				{
					return false;
				}
				if (selfActor.hasStatus(attackCooldownStatus))
				{
					return false;
				}
				if (targetActor == null)
				{
					return false;
				}
				// === 冷卻狀態施加 ===
				// 在發動後，立即為單位施加冷卻狀態 及其他狀態的添加
				selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
				// 保持原本的無敵狀態，但時長可以縮短，避免影響冷卻
				//selfActor.addStatusEffect("invincible", 0.05f);
				// === 投射物發射邏輯（環狀集火） ===
				UnityEngine.Vector3 selfPosition = selfActor.current_position;
				UnityEngine.Vector3 targetPosition = targetActor.current_position;
				// 計算發射物的基礎發射點，並向外擴展
				UnityEngine.Vector3 baseLaunchPoint = Toolbox.getNewPoint(selfPosition.x, selfPosition.y, targetPosition.x, targetPosition.y, selfActor.stats["size"]);
				baseLaunchPoint.y += 0.5f;

				// 子彈相關設定 - 環狀攻擊
				int numberOfProjectiles_00 = 3;	//子彈數量
				float spreadDistance_00 = 5.0f; // 施法者和發射點的間距
				float spreadAngle_00 = 360.0f; // 投射物發射點的散布角度
				float totalSpread_00 = spreadAngle_00 / numberOfProjectiles_00;
				
				for (int i = 0; i < numberOfProjectiles_00; i++)
				{
					// 計算每個投射物的發射角度
					float angle = totalSpread_00 * i - spreadAngle_00 / 2f;
					
					// 根據角度和散布距離，計算每個投射物的發射位置
					UnityEngine.Vector3 launchPosition = baseLaunchPoint + Quaternion.Euler(0f, 0f, angle) * new UnityEngine.Vector3(spreadDistance_00, 0, 0);

					World.world.projectiles.spawn(
						pInitiator: selfActor,
						pTargetObject: targetActor,
						pAssetID: "Snowflake",
						pLaunchPosition: launchPosition,
						pTargetPosition: targetPosition, // 所有投射物都射向同一個目標點
						pTargetZ: 0.0f
					);
				}
				return true;
			}
			return false;
		}
		public static bool SleepingLawO(BaseSimObject pSelf, WorldTile pTile)
		{// 睡夢法 (舊版)
			// 1. 基本安全檢查：確保施法者 pSelf 及其 Actor 組件存在且存活
			if (pSelf == null || pSelf.a == null)
			{
				// Debug.LogWarning("SleepingLaw: 施法者無效或不存在 Actor 組件.");
				return false; // 無效的施法者
			}
			Actor selfActor = pSelf.a;
			// ** 臨時安全措施：如果 pTile 為 null，則直接返回，避免崩潰 **
			// 這是在沒有可靠獲取 WorldTile 方法前採用的策略
			if (pTile == null)
			{
				// Debug.LogWarning("SleepingLaw Warning: pTile is null. Skipping effect to prevent crash.");
				return false;
			}
			int range = 1; // 設定影響範圍
			string sleepingStatusID = "sleeping"; // 睡眠的狀態ID
			float sleepingDuration = 600f; // 睡眠狀態的持續時間 (可調整)
			bool effectAppliedToAnyone = false; // 追蹤是否成功對至少一個目標應用了效果
			// ====== 邏輯：檢查並移除施法者自身的 "sleeping" 狀態效果 ======
			// 這是為了確保施法者不會被自己的法術影響而睡著
			if (selfActor.hasStatus(sleepingStatusID))
			{
				selfActor.finishStatusEffect(sleepingStatusID); // 結束睡眠狀態
				// Debug.Log($"施法者 {selfActor.name} 自身移除了睡眠狀態：{sleepingStatusID}");
			}
			// 2. 獲取範圍內的 Actor
			var allClosestUnits = Finder.getUnitsFromChunk(pTile, range);
			// 遍歷範圍內的 Actor
			if (allClosestUnits.Any())
			{
				foreach (var unit in allClosestUnits)
				{
					Actor targetActor = unit.a; // 從 BaseSimObject 獲取 Actor 組件
					// 檢查目標是否是有效的 Actor，存活，且不是施法者本身，且不屬於同一王國 (關鍵邏輯)
					if (targetActor != null && targetActor.isAlive() && targetActor != selfActor && targetActor.kingdom != selfActor.kingdom)
					{
						// 檢查目標是否已經擁有 "sleeping" 狀態效果
						if (!targetActor.hasStatus(sleepingStatusID)) // 如果目標沒有睡眠狀態，則添加
						{
							targetActor.addStatusEffect(sleepingStatusID, sleepingDuration); // 添加 "sleeping" 狀態效果
							//Debug.Log($"施法者 {selfActor.name} 對 {targetActor.name} 施加了睡眠狀態：{sleepingStatusID}");
							effectAppliedToAnyone = true; // 只要有一個目標被應用效果，就設為 true
						}
					}
				}
			}
			return effectAppliedToAnyone;
		}

		public static bool Sterilization(BaseSimObject pSelf, WorldTile pTile)
		{// 絕育法 狀態覆蓋
			// 1. 基本安全檢查：確保施法者 pSelf 及其 Actor 組件存在且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) 
			{
				return false; // 無效的施法者
			}
			Actor selfActor = pSelf.a;
			// 臨時安全措施：如果 pTile 為 null，則直接返回，避免崩潰
			if (pTile == null)
			{
				// Debug.Log("Sterilization Warning: pTile is null. Skipping effect to prevent crash.");
				return false;
			}
			if (!selfActor.subspecies.hasTrait("prefrontal_cortex"))
			{
				return false;
			}
			int range = 1; // 設定影響範圍
			string sterilizationStatusID = "sterilization"; // 絕育狀態的ID
			float sterilizationDuration = 1200f; // 絕育狀態的持續時間，例如 10 分鐘 (可調整)
			bool effectAppliedToAnyone = false; // 追蹤是否成功對至少一個目標應用了效果
			// 1.a 處理施法者自身：如果施法者有 "sterilization" 狀態，則移除它
			if (selfActor.hasStatus(sterilizationStatusID))
			{
				selfActor.finishStatusEffect(sterilizationStatusID); // 結束絕育狀態
				// Debug.Log($"施法者 {selfActor.name} 自身移除了絕育狀態：{sterilizationStatusID}");
				effectAppliedToAnyone = true; // 自身被影響也算效果應用
			}
			// 2. 獲取範圍內的 Actor
			var allClosestUnits = Finder.getUnitsFromChunk(pTile, range);
			// 遍歷範圍內的 Actor
			if (allClosestUnits.Any())
			{
				foreach (var unit in allClosestUnits)
				{
					Actor targetActor = unit.a; // 從 BaseSimObject 獲取 Actor 組件

					// 檢查目標是否是有效的 Actor，存活，且不是施法者本身
					if (targetActor != null && targetActor.isAlive() && targetActor != selfActor)
					{
						bool targetAffected = false; // 標記當前目標是否被影響

						// 判斷目標是友方還是敵方 (基於王國)
						if (targetActor.kingdom == selfActor.kingdom) // 同一王國 (友方)
						{
							// 對友方：如果持有 "sterilization" 狀態，則移除
							if (targetActor.hasStatus(sterilizationStatusID))
							{
								targetActor.finishStatusEffect(sterilizationStatusID);
								// Debug.Log($"{selfActor.name} 為友方 {targetActor.name} 移除了絕育狀態：{sterilizationStatusID}");
								targetAffected = true;
							}
						}
						else // 不同王國 (敵方)
						{
							// 對敵方：如果沒有 "sterilization" 狀態，則添加
							if (!targetActor.hasStatus(sterilizationStatusID))
							{
								targetActor.addStatusEffect(sterilizationStatusID, sterilizationDuration);
								// Debug.Log($"{selfActor.name} 對敵方 {targetActor.name} 施加了絕育狀態：{sterilizationStatusID}");
								targetAffected = true;
							}
						}

						if (targetAffected)
						{
							effectAppliedToAnyone = true; // 只要有一個目標被應用效果，就設為 true
						}
					}
				}
			}
			// 返回是否成功對至少一個目標應用了效果 (包括施法者自身)
			return effectAppliedToAnyone;
		}
		private static bool ProcessReapplyStatus(Actor pActor, string statusID1, string statusID2, float duration)
		{// 絕育法 延遲孵化 輔助方法
			bool reapplied = false;
			if (pActor.hasStatus(statusID1))
			{
				// 如果單位已經有狀態，則重新添加以延長時間
				pActor.addStatusEffect(statusID1, duration); 
				reapplied = true;
			}
			if (pActor.hasStatus(statusID2))
			{
				// 如果單位已經有狀態，則重新添加以延長時間
				pActor.addStatusEffect(statusID2, duration);
				reapplied = true;
			}
			return reapplied;
		}
		public static bool Delayed_hatching(BaseSimObject pSelf, WorldTile pTile)
		{// 絕育法 延遲孵化
			// 1. 基本安全檢查：確保施法者 pSelf 及其 Actor 組件存在且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive())
			{
				return false; // 無效的施法者
			}
			Actor selfActor = pSelf.a;

			// 臨時安全措施：如果 pTile 為 null，則直接返回，避免崩潰 (保留此安全措施)
			if (pTile == null)
			{
				// Debug.Log("Delayed_hatching Warning: pTile is null. Skipping effect to prevent crash.");
				return false;
			}
			if (!selfActor.subspecies.hasTrait("prefrontal_cortex"))
			{
				return false;
			}

			int range = 10; // 設定影響範圍
			string uprootingStatusID = "uprooting"; // 要延長的狀態ID 1
			string eggStatusID = "egg";			 // 要延長的狀態ID 2
			float extendDuration = 1800f;		   // 延長的持續時間 (30 分鐘)

			bool effectAppliedToAnyone = false;	 // 追蹤是否成功對至少一個單位應用了效果

			// 移除施法者自身處理的邏輯，因為這個能力不再影響自身，只影響敵方。
			// 1. 處理施法者自身 (此功能不再對施法者自身施加或移除效果)

			// 2. 獲取範圍內的 Actor
			var allClosestUnits = Finder.getUnitsFromChunk(pTile, range);
			
			if (allClosestUnits.Any())
			{
				foreach (var unit in allClosestUnits)
				{
					Actor targetActor = unit.a; // 從 BaseSimObject 獲取 Actor 組件

					// 檢查目標是否是有效的 Actor，存活，且不是施法者本身，**且不屬於同一王國 (核心邏輯：只影響敵方)**
					if (targetActor != null && targetActor.isAlive() && targetActor != selfActor && targetActor.kingdom != selfActor.kingdom)
					{
						// 調用輔助方法處理重新添加狀態以延長其持續時間
						if (ProcessReapplyStatus(targetActor, uprootingStatusID, eggStatusID, extendDuration))
						{
							effectAppliedToAnyone = true; // 只要有一個目標被影響，就設為 true
						}
					}
				}
			}
			
			return effectAppliedToAnyone; // 返回是否成功對至少一個目標應用了效果
		}

		public static bool ManageWrathEffect3(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 忿怒法 點火
			// 安全檢查：確保施法者和目標都存在且有效
			if (pSelf?.a == null || pTarget?.a == null || !pSelf.a.isAlive() || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			Actor targetActor = pTarget.a;
			if (selfActor.hasStatus("wrath_demon_king") && selfActor.hasStatus("wdk2"))
			{
				return false;
			}
			if (!selfActor.hasStatus("calm") && !selfActor.hasStatus("tantrum"))
			{				
				selfActor.addStatusEffect("tantrum", 5f);
				selfActor.removeTrait("strong_minded");
				if (UnityEngine.Random.value < 0.333f)
				{
					selfActor.addStatusEffect("angry", 5f); // 指定狀態
				}
			}
			if (selfActor.hasStatus("tantrum") && selfActor.hasStatus("angry"))
			{
				ManageWrathEffect1(pSelf, pTile);
			}
			if (selfActor.hasStatus("wrath_demon_king"))
			{
				EvilGlovesGet(pSelf, pTile);
				Items01Actions.EvilGlovesAwakens(pSelf, pTile);
			}
			return false; // 特殊效果通常返回 false，表示不阻止其他效果链
		}
		public static bool TantrumLaw(BaseSimObject pSelf, WorldTile pTile)
		{// 忿怒法 忿怒波動
			// 1. 基本安全檢查：確保施法者 pSelf 及其 Actor 組件存在且存活
			if (pSelf == null || pSelf.a == null)
			{
					// Debug.LogWarning("TantrumLaw: 施法者無效或不存在 Actor 組件.");
				return false; // 無效的施法者
			}
			Actor selfActor = pSelf.a;
			// ** 臨時安全措施：如果 pTile 為 null，則直接返回，避免崩潰 **
			if (pTile == null)
			{
					return false;
			}
			if (selfActor.subspecies == null || !selfActor.subspecies.hasTrait("prefrontal_cortex"))
			{
				return false;
			}
			int range = 4; // 設定影響範圍
			string tantrumStatusID = "tantrum"; // 忿怒的狀態ID
			float tantrumDuration = 600f; // 忿怒狀態的持續時間 (可調整)
			string angryStatusID = "angry"; // <--- 新增：憤怒的狀態ID
			float angryDuration = 600f;   // <--- 新增：憤怒狀態的持續時間 (可調整，可以與tantrumDuration不同)
			bool effectAppliedToAnyone = false; // 追蹤是否成功對至少一個目標應用了效果
			// ====== 邏輯：檢查並移除施法者自身的 "tantrum" 狀態效果 ======
			// 2. 獲取範圍內的 Actor
			var allClosestUnits = Finder.getUnitsFromChunk(pTile, range);
				// 遍歷範圍內的 Actor
				if (allClosestUnits.Any())
				{
					foreach (var unit in allClosestUnits)
					{
						Actor targetActor = unit.a; // 從 BaseSimObject 獲取 Actor 組件
						//特質檢查
						if (targetActor != null && targetActor.hasTrait("evillaw_tantrum") || targetActor.hasTrait("holyarts_serenity"))
						{
							continue; // 跳過此目標，繼續檢查下一個
						}
						// 檢查目標是否是有效的 Actor，存活，且不是施法者本身，且不屬於同一王國 (關鍵邏輯)
						if (targetActor != null && targetActor.isAlive() && targetActor != selfActor && targetActor.kingdom != selfActor.kingdom)
						{
							// ** 為目標添加 "tantrum" 狀態效果 **
							if (!targetActor.hasStatus(tantrumStatusID)) // 如果目標沒有忿怒狀態，則添加
							{
								targetActor.addStatusEffect(tantrumStatusID, tantrumDuration); 
								// Debug.Log($"施法者 {selfActor.name} 對 {targetActor.name} 施加了忿怒狀態：{tantrumStatusID}");
								effectAppliedToAnyone = true; // 只要有一個目標被應用效果，就設為 true
							}
							// ** 新增：為目標添加 "angry" 狀態效果 **
							// 這裡判斷是：如果目標沒有 angry 狀態，則添加
							if (!targetActor.hasStatus(angryStatusID)) 
							{
								targetActor.addStatusEffect(angryStatusID, angryDuration); 
								// Debug.Log($"施法者 {selfActor.name} 對 {targetActor.name} 施加了憤怒狀態：{angryStatusID}");
								effectAppliedToAnyone = true; // 只要有一個目標被應用效果，就設為 true (如果之前沒有，這裡也會設為true)
							}
						}
					}
				}
			return effectAppliedToAnyone;
			}
		public static bool ManageWrathEffect1(BaseSimObject pSelf, WorldTile pTile = null)
		{// 忿怒法 效果整合 (冷靜效果添加、魔王狀態觸發 與 瘋狂特質管理)
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			
			if (selfActor.subspecies == null)
			{
				return false;
			}
			//if (!selfActor.hasTrait("evillaw_tantrum"))
			//{
			//	return false;
			//}
			
			// ****** 第一部分：管理 'calm' (冷靜) 狀態 ******
			// 這部分邏輯不變，用來在特定情況下提供寧靜狀態
			if (!selfActor.hasStatus("cdt_calm"))
			{
				selfActor.addStatusEffect("cdt_calm", 300f);
				selfActor.addStatusEffect("calm");
			}
			
			// ****** 第二部分：觸發 'wrath_demon_king' (憤怒魔王) 狀態 ******
			// 如果已經處於魔王狀態，則直接跳到第三部分管理瘋狂特質
			if (selfActor.hasStatus("wrath_demon_king"))
			{
				// 直接跳到瘋狂特質管理部分
				goto manage_madness;
			}
			
			// 檢查是否滿足變身條件
			if (!selfActor.hasStatus("calm") && selfActor.hasStatus("tantrum") && selfActor.hasStatus("angry"))
			{
				// 檢查王國內是否已存在其他魔王
				bool hasOtherDemonKing = false;
				Kingdom currentKingdom = selfActor.kingdom;
				if (currentKingdom != null)
				{
					foreach (Actor kingdomUnit in currentKingdom.units)
					{
						if (kingdomUnit == null || kingdomUnit == selfActor)
						{
							continue;
						}
						if (kingdomUnit.hasTrait("hope")||
							kingdomUnit.hasTrait("other6661")||
							kingdomUnit.hasTrait("other6662")||
							kingdomUnit.hasTrait("other6663")||
							kingdomUnit.hasTrait("other6664")||
							kingdomUnit.hasTrait("other6665")||
							kingdomUnit.hasTrait("other6666")||
							kingdomUnit.hasTrait("other6667")||
							kingdomUnit.hasTrait("other6668")||
							kingdomUnit.hasTrait("other6669"))
						{
							hasOtherDemonKing = true;
							break;
						}
						foreach (string demonKingStatusID in SevenDemonKingStatus_DemonKing)
						{
							if (kingdomUnit.hasStatus(demonKingStatusID))
							{
								hasOtherDemonKing = true;
								break;
							}
						}
						if (hasOtherDemonKing)
						{
							break;
						}
					}
				}
				
				// 根據是否存在其他魔王來決定行為
				if (!hasOtherDemonKing)
				{
					selfActor.addStatusEffect("wrath_demon_king", 3600f);
					EvilGlovesGet(pSelf, pTile);
					Items01Actions.EvilGlovesAwakens(pSelf, pTile);
				}
				else
				{
					const int KILLS_TO_BECOME_DEMON_KING = 666;
					if (selfActor.data.kills >= KILLS_TO_BECOME_DEMON_KING)
					{
						selfActor.addStatusEffect("wrath_demon_king", 3600f);
						EvilGlovesGet(pSelf, pTile);
						Items01Actions.EvilGlovesAwakens(pSelf, pTile);
					}
				}
			}

			// ****** 第三部分：管理 'madness' (瘋狂) 特質 ******
			manage_madness:
			
			// 只有在 'wrath_demon_king' 狀態下才執行瘋狂特質的增減
			if (selfActor.hasStatus("wrath_demon_king"))
			{
				int currentKills = selfActor.data.kills;
				if (currentKills < 666)
				{
					if (!selfActor.hasTrait("madness"))
					{
						selfActor.addTrait("madness");
						return true;
					}
				}
				else
				{
					if (selfActor.hasTrait("madness"))
					{
						selfActor.removeTrait("madness");
						return true;
					}
				}
			}
			else
			{
				// 如果沒有魔王狀態，移除瘋狂特質以防萬一
				selfActor.removeTrait("madness");
			}
			
			return false; // 如果沒有狀態改變，返回 false
		}
		public static bool ManageWrathEffect2(BaseSimObject pSelf, WorldTile pTile = null)
		{// 忿怒法 給予tantrum
			// 1. 基本安全检查：确保 pSelf 及其 Actor 组件存在且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) 
				return false;
			Actor selfActor = pSelf.a;
			// 2. 上半部：特质持有者自身添加条件 (保留您的状态检查)
			// 确保自身没有 "cdt_buff00" (冷却状态) 且没有 "cdt_debuff00"
			if (!selfActor.hasStatus("calm") && !selfActor.hasStatus("wrath_demon_king") && !selfActor.hasStatus("tantrum"))
			{
				selfActor.addStatusEffect("tantrum", 4f);// 冷却状态
				selfActor.removeTrait("strong_minded");// 冷却状态
				if (UnityEngine.Random.value < 0.333f) // 使用 Random.value 來實現 50% 機率
				{
					selfActor.addStatusEffect("angry", 4f); // 指定狀態
				}

			}
			return false; // 特殊效果通常返回 false，表示不阻止其他效果链
		}
		public static bool EvilGlovesGet(BaseSimObject pTarget, WorldTile pTile = null)
		{// 忿怒法 武器給予
			// 定義所需的狀態ID常量
			const string DemonKingStatus = "wrath_demon_king"; //魔王狀態
			const string WeapontID = "evil_gloves"; // 物品ID
			// 1. 基本安全檢查：目標是否有效且存活
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor targetActor = pTarget.a;
			// ****** 新增檢查：單位是否擁有 '指定魔王狀態' 狀態 ******
			if (!targetActor.hasStatus(DemonKingStatus))
			{
				return false; // 如果沒有此狀態，則不給予裝備
			}
			// ************************************************************
			// 獲取目標的武器槽位
			var weaponSlot = targetActor.equipment.getSlot(EquipmentType.Weapon);
			// 檢查武器槽位是否已被佔用，且其中持有的道具ID是否為 "指定武器"
			if (weaponSlot != null && weaponSlot.getItem() != null && weaponSlot.getItem().asset.id == WeapontID)
			{
				// 額外檢查：如果武器沒有修飾符，在這裡補上
				Item existingWeapon = weaponSlot.getItem();
				/*if (!existingWeapon.hasMod("eternal"))
				{
					existingWeapon.addMod("eternal");
					existingWeapon.addMod("cursed");
				}*/
				return true;
			}
			// 如果單位沒有持有 "指定武器" 且擁有指定狀態，則繼續進行裝備流程
			// 獲取 "指定武器" 物品資產
			var weaponAsset = AssetManager.items.get(WeapontID);
			if (weaponAsset == null)
			{
				return false; // 無法找到物品資產，返回失敗
			}
			// 生成 "指定武器" 物品實例
			var newItem = World.world.items.generateItem(pItemAsset: weaponAsset);
			// =======================================================
			// === 核心修正：強制添加修飾符 ===
			if (newItem != null)
			{
				newItem.addMod("stun");
				newItem.addMod("power5");
				newItem.addMod("truth5");
				newItem.addMod("protection5");
				newItem.addMod("speed5");
				newItem.addMod("balance5");
				newItem.addMod("health5");
				newItem.addMod("finesse5");
				newItem.addMod("mastery5");
				newItem.addMod("knowledge5");
				newItem.addMod("sharpness5");
			}
			// =======================================================
			// 將 "指定武器" 裝備到目標的武器槽位
			weaponSlot.setItem(newItem, targetActor);
			// 更新目標的統計數據，以反映新裝備帶來的變化
			targetActor.setStatsDirty();
			targetActor.data.health += 99999;
			return true; // 表示效果成功施加
		}
		public static bool AngryAura_ATK(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
		{// 忿怒法 憤怒靈氣 (攻)
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			AngryAura(pSelf, pTile);
			TantrumLaw(pSelf, pTile);
			return true;
		}
		public static bool AngryAura(BaseSimObject pTarget, WorldTile pTile = null)
		{// 忿怒法 憤怒靈氣
			// 1. 基本安全檢查
			if (pTarget == null || !pTarget.isActor() || !pTarget.a.isAlive())
				return false;
			Actor selfActor = pTarget.a; // 獲取 Actor 實例
			// === 狀態檢查：必須擁有 'wrath_demon_king' 狀態才能發動 ===
			if (!selfActor.hasStatus("wrath_demon_king"))
			{
				return false;
			}
			WorldTile tCurTile = pTarget.current_tile;
			// 2. 檢查當前瓦片類型是否可以被點燃（這是針對單位腳下立即點燃的檢查）
			if (!tCurTile.Type.can_be_set_on_fire_by_burning_feet)
			{
				return false;
			}
			// 3. 執行範圍筆刷操作 1：用於融化冰凍區域和移除液體
			int igniteRadius1 = 20;
			World.world.loopWithBrush(tCurTile, Brush.get(igniteRadius1, "circ_"), new PowerActionWithID(Traits01Actions.burningFeetEffectTileDraw), null);
			int igniteRadius2 = 30;
			World.world.loopWithBrush(tCurTile, Brush.get(igniteRadius2, "circ_"), new PowerActionWithID(Traits01Actions.AngryAuraRemoveOcean), null);
			int igniteRadius3 = 8;
			World.world.loopWithBrush(tCurTile, Brush.get(igniteRadius3, "circ_"), new PowerActionWithID(Traits01Actions.AngryAuraRemoveLava1), null);
			int igniteRadius4 = 9;
			World.world.loopWithBrush(tCurTile, Brush.get(igniteRadius4, "circ_"), new PowerActionWithID(Traits01Actions.AngryAuraRemoveLava2), null);
			// 4. 如果單位不在液體中，則點燃腳下的瓦片（核心點，確保腳下有火）
			if (!pTarget.a.isInWater())//Liquid
			{
				tCurTile.startFire(true);
			}
			// === 修改點：擴大點燃相鄰瓦片的範圍 ===
			// 5. 執行範圍筆刷操作 2：用於大範圍點燃瓦片
			int igniteRadius5;
			if (selfActor.hasTrait("madness"))
			{
				igniteRadius5 = 15; 
			}
			else
			{
				igniteRadius5 = 0; 
			}
			World.world.loopWithBrush(tCurTile, Brush.get(igniteRadius5, "circ_"), new PowerActionWithID(Traits01Actions.AngryAuraIgniteTile), null);
			return true;
		}
		public static bool burningFeetEffectTileDraw(WorldTile pTile, string pPowerID)
		{// 忿怒法 憤怒靈氣 輔助函數1(解凍)
			if (pTile.isTemporaryFrozen() && Randy.randomBool())
			{
				pTile.unfreeze(99);
				//pTile.remove_water(true);
			}
			return true;
		}
		public static bool AngryAuraIgniteTile(WorldTile pTile, string pPowerID)
		{// 忿怒法 憤怒靈氣 輔助函數2(燃燒)
			// 確保瓦片有效
			if (pTile == null) return false;
			pTile.heat = 0; 
			int fireDuration = 30;
			if (pTile.Type.can_be_set_on_fire_by_burning_feet)
			{
				pTile.startFire(true);
				pTile.setBurned(fireDuration); // 永久燃燒或由事件控制
			}
			return true;
		}
		public static bool AngryAuraRemoveOcean(WorldTile pTile, string pPowerID)
		{// 忿怒法 憤怒靈氣 輔助函數3(移除海洋)
			if (pTile == null)
			{
				return false;
			}
			// 只有當瓦片是液體時才執行移除
			if (pTile.Type.ocean)
			{
				MapAction.decreaseTile(pTile, false, "flash");
			}
			return true;
		}
		public static bool AngryAuraRemoveLava1(WorldTile pTile, string pPowerID)
		{// 忿怒法 憤怒靈氣 輔助函數4(移除岩漿)
			if (pTile == null)
			{
				return false;
			}
			// 只有當瓦片是液體時才執行移除
			if (pTile.Type.lava)
			{
				MapAction.decreaseTile(pTile, false, "flash");
			}
			return true;
		}
		public static bool AngryAuraRemoveLava2(WorldTile pTile, string pPower)
		{// 忿怒法 憤怒靈氣 輔助函數5(冷卻岩漿)
			if (pTile == null)
			{
				return false;
			}
			// 檢查並冷卻岩漿
			if (pTile.Type.lava)
			{
				LavaHelper.coolDownLava(pTile);
			}
			return true;
		}


		private static readonly HashSet<string> unconvertibleTraits = new HashSet<string>
		{// 誘惑法 不可誘惑 的 特質清單
			"apostle",		 			// 使徒
			"slave",		 			// 奴隸
			"undead_servant",			// 不死族從者
			"undead_servant2",			// 不死族從者
			"undead_servant3",			// 不死族從者
			"extraordinary_authority",	// 不死族從者
			"evillaw_ew",				// 滅智 (傲慢)
			"evillaw_tantrum",			// 憤怒 (憤怒)
			"evillaw_seduction",		// 誘惑 (色慾)
			"evillaw_sleeping",			// 睡眠	(怠惰)
			"evillaw_moneylaw",			// 金錢 (強欲)
			"evillaw_starvation",		// 餓食 (暴食)
			"evillaw_devour",			// 吞噬 (嫉妒)
			"pro_king",					// 國王
			"pro_leader",				// 領主
			"holyarts_bond",			// 絆
			"holyarts_justice",			// 裁決
			"strong_minded",			// 原版特質
			"desire_alien_mold",		// 原版特質
			"desire_computer",			// 原版特質
			"desire_golden_egg",		// 原版特質
			"desire_harp",				// 原版特質
			"madness",					// 原版特質
			"psychopath",				// 原版特質
			// 如果有其他您不希望被轉化的特質，請添加在這裡
		};
		public static bool SeductionLaw(BaseSimObject pTarget, WorldTile pTile = null)
		{// 誘惑法 單位轉換
			// 確保 pTarget 是有效的 Actor 並且存活 (施法者)
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor seducerActor = pTarget.a; // 將施法者命名為 seducerActor 以提高可讀性
			if (seducerActor.subspecies == null || !seducerActor.subspecies.hasTrait("prefrontal_cortex"))
			{
				return false;
			}
			var allClosestUnits = Finder.getUnitsFromChunk(pTile, 5);
			if (allClosestUnits.Any())
			{
				foreach (var unit in allClosestUnits)
				{
					// 確保 unit 和 unit.a 有效且存活 (被轉化目標)
					if (unit == null || unit.a == null || !unit.a.isAlive())
					{
						continue;
					}
					// 不應對施法者自己執行效果，避免自轉化或自愛
					if (unit == seducerActor)
					{
						continue;
					}
					// === 新增功能：不轉化國王和領主 ===
					if (unit.getProfession() == UnitProfession.King)
					{
						continue; // 如果是國王，直接跳過
					}
					if (unit.getProfession() == UnitProfession.Leader)
					{
						continue; // 如果是領主，直接跳過
					}
					// 檢查單位是否帶有任何不可轉化的特質
					bool isUnconvertible = false;
					foreach (string traitId in unconvertibleTraits)
					{
						if (unit.hasTrait(traitId))
						{
							isUnconvertible = true;
							break;
						}
					}
					if (isUnconvertible)
					{
						continue; // 如果這個單位帶有不可轉化特質，跳過
					}
					// 檢查條件：
					// a. 單位不屬於施法者 (pTarget) 的王國 (即是敵對或中立單位)
					if (unit.kingdom != seducerActor.kingdom) // 使用 seducerActor
					{
						// 檢查單位是否擁有 "afterglow" 狀態
						if (unit.hasStatus("afterglow"))
						{
						// ====== 核心修改開始：轉化和 Lover 關係變更 ======
						// 移除軍籍 與 氏族
							if (unit.army != null)
							{
								unit.stopBeingWarrior(); 
							}
							if (seducerActor.kingdom != null)
							{
								unit.kingdom = seducerActor.kingdom;
							}
							if (unit.clan != null)
							{
								unit.clan.units.Remove(unit);
								unit.clan = null;
							}
						// 03.只有當施法者有城市時才設定
							if (seducerActor.city != null)
							{
								unit.city = seducerActor.city;
							}
						// 04. 如果單位有家庭，先將其從舊家庭的成員清單中移除
							if (unit.family != null)
							{
								unit.family.units.Remove(unit);
								unit.family = null;
							}
							if (seducerActor.family != null)
							{
								seducerActor.family.units.Add(unit);
								unit.family = seducerActor.family;
							}
							if (unit.subspecies != null)
							{// 如果奴隸單位亞種不為空
								if (unit.subspecies.hasTrait("advanced_hippocampus"))
								{// 持有主管文明宗教的advanced_hippocampus亞種特質為前提
									if (seducerActor.religion != null)
									{
										unit.religion = seducerActor.religion;
									}
									if (seducerActor.culture != null)
									{
										unit.culture = seducerActor.culture;
									}
								}
								if (unit.subspecies.hasTrait("wernicke_area"))
								{// 持有主管言語的wernicke_area亞種特質為前提
									if (seducerActor.language != null)
									{
										unit.language = seducerActor.language;
									}
								}
							}
							unit.addTrait("slave");
							unit.removeTrait("battle_reflexes");
							unit.removeTrait("arcane_reflexes");
							unit.data.health += 9999;
							unit.data.set("master_id", seducerActor.data.id);
							unit.addStatusEffect("fell_in_love", 180f, false); 
							unit.finishStatusEffect("angry"); 
							unit.setLover(seducerActor);
							if (!listOfTamedBeasts.ContainsKey(unit))
								listOfTamedBeasts.Add(unit, seducerActor); // 使用 seducerActor
							// Debug.Log($"單位 {unit.name.ToString()} (ID: {unit.asset.id}) 因誘惑效果被轉化並愛上 {seduce rActor.name.ToString()}！");
							// ====== 核心修改結束 ======
						}
					}
				}
			}
			return true;
		}
		public static bool SeductionLaw2(BaseSimObject pTarget, WorldTile pTile = null)
		{// 誘惑法 家族奴役
			// 確保 pTarget 是有效的 Actor 並且存活 (施法者)
			if (pTarget?.a == null || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor seducerActor = pTarget.a;
			// 發動條件：施法者是家庭創始人
			if (seducerActor.family == null || !seducerActor.family.isMainFounder(seducerActor))
			{
				return false;
			}
			if (!seducerActor.subspecies.hasTrait("prefrontal_cortex"))
			{
				return false;
			}
			// === 修正：創建家庭成員清單的副本 ===
			List<Actor> familyMembersToConvert = new List<Actor>(seducerActor.family.units);
			// ======================================
			foreach (var unit in familyMembersToConvert)
			{
				// 確保單位有效、存活，且不是施法者自己
				if (unit == null || !unit.isAlive() || unit == seducerActor)
				{
					continue;
				}
				// 檢查不可轉化的特質
				bool isUnconvertible = false;
				foreach (string traitId in unconvertibleTraits)
				{
					if (unit.hasTrait(traitId))
					{
						isUnconvertible = true;
						break;
					}
				}
				if (isUnconvertible)
				{
					continue;
				}
				// 如果單位還沒有奴隸特質，就進行轉化
				if (!unit.hasTrait("slave"))
				{
					unit.addTrait("slave");
					unit.removeTrait("battle_reflexes");
					unit.removeTrait("arcane_reflexes");
					unit.data.age_overgrowth += 15;
					unit.data.health += 9999;
					unit.data.set("master_id", seducerActor.data.id);
					unit.addStatusEffect("fell_in_love", 0f, false);
					unit.finishStatusEffect("angry");
					unit.setLover(seducerActor);
					if (!listOfTamedBeasts.ContainsKey(unit))
					{
						listOfTamedBeasts.Add(unit, seducerActor);
					}
				}
			}
			return true;
		}
		public static bool TransformGender(BaseSimObject pTarget, WorldTile pTile = null)
		{// 誘惑法 性別轉換 魔王狀態
			// 常量定義
			const string GenderChangeCooldownStatus = "change";
			const float GenderChangeCooldownDuration = 601.67f;//冷卻CD
			const string GenderChangeCountDataKey = "gender_change_count";
			const int TransformationThreshold = 7;//轉換次數 7
			const string LustDemonKingStatusID = "lust_demon_king";
			const float LustDemonKingStatusDuration = 3600f;//狀態持續
			if (pTarget == null || !(pTarget is Actor targetActor) || !targetActor.isAlive())
			{
				return false;
			}
			if (!targetActor.subspecies.hasTrait("prefrontal_cortex"))
			{
				return false;
			}
			if (targetActor.hasStatus(GenderChangeCooldownStatus))
			{
				return false;
			}
			ActorSex currentSex = targetActor.data.sex;
			ActorSex newSex;
			if (currentSex == ActorSex.Male)
			{
				newSex = ActorSex.Female;
			}
			else if (currentSex == ActorSex.Female)
			{
				newSex = ActorSex.Male;
			}
			else
			{
				return false;
			}
			targetActor.data.sex = newSex;
			targetActor.addStatusEffect(GenderChangeCooldownStatus, GenderChangeCooldownDuration);
			int currentTransformCount = 0;
			targetActor.data.get(GenderChangeCountDataKey, out currentTransformCount);
			currentTransformCount++;
			targetActor.data.set(GenderChangeCountDataKey, currentTransformCount);
			if (currentTransformCount > 0 && currentTransformCount % TransformationThreshold == 0)
			{
				// ====== 新增邏輯：檢查王國內是否已存在魔王 ======
				// 1. 獲取當前單位所屬的王國
				Kingdom currentKingdom = targetActor.kingdom;
				// 2. 只有當單位屬於一個王國時才進行檢查
				if (currentKingdom != null)
				{
					bool hasDemonKing = false;
					// 3. 遍歷王國中的所有單位
					foreach (Actor kingdomUnit in currentKingdom.units)
					{
						// 確保單位有效且不是自己
						if (kingdomUnit == null || kingdomUnit == targetActor)
						{
							continue;
						}
						if (kingdomUnit.hasTrait("hope")||
							kingdomUnit.hasTrait("other6661")||
							kingdomUnit.hasTrait("other6662")||
							kingdomUnit.hasTrait("other6663")||
							kingdomUnit.hasTrait("other6664")||
							kingdomUnit.hasTrait("other6665")||
							kingdomUnit.hasTrait("other6666")||
							kingdomUnit.hasTrait("other6667")||
							kingdomUnit.hasTrait("other6668")||
							kingdomUnit.hasTrait("other6669"))
						{
							hasDemonKing = true;
							break;
						}
						// 4. 檢查單位是否擁有任一魔王狀態
						foreach (string demonKingStatusID in SevenDemonKingStatus_DemonKing)
						{
							if (kingdomUnit.hasStatus(demonKingStatusID))
							{
								hasDemonKing = true;
								break; // 找到一個魔王，跳出內層迴圈
							}
						}
						if (hasDemonKing)
						{
							break; // 找到一個魔王，跳出外層迴圈
						}
					}
					// 5. 如果王國中沒有魔王，才賦予狀態
					if (!hasDemonKing)
					{
						targetActor.addStatusEffect(LustDemonKingStatusID, LustDemonKingStatusDuration);
						EvilBowGet(pTarget, pTile);
					}
				}
				else // 如果單位沒有王國，則直接賦予狀態（例如，流浪者）
				{
					targetActor.addStatusEffect(LustDemonKingStatusID, LustDemonKingStatusDuration);
					EvilBowGet(pTarget, pTile);
					Items01Actions.EvilBowAwakens(pTarget, pTile);
				}
				// ===================================
			}
			return true;
		}
		public static bool EvilBowGet(BaseSimObject pTarget, WorldTile pTile = null)
		{// 誘惑法 武器給予
			// 定義所需的狀態ID常量
			const string DemonKingStatus = "lust_demon_king";
			const string WeapontID = "evil_bow"; // 物品ID
			// 1. 基本安全檢查：目標是否有效且存活
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor targetActor = pTarget.a;
			// ****** 新增檢查：單位是否擁有 '指定魔王狀態' 狀態 ******
			if (!targetActor.hasStatus(DemonKingStatus))
			{
				return false; // 如果沒有此狀態，則不給予裝備
			}
			// ************************************************************
			// 獲取目標的武器槽位
			var weaponSlot = targetActor.equipment.getSlot(EquipmentType.Weapon);
			// 檢查武器槽位是否已被佔用，且其中持有的道具ID是否為 "指定武器"
			if (weaponSlot != null && weaponSlot.getItem() != null && weaponSlot.getItem().asset.id == WeapontID)
			{
				// 額外檢查：如果武器沒有修飾符，在這裡補上
				Item existingWeapon = weaponSlot.getItem();
				/*if (!existingWeapon.hasMod("eternal"))
				{
					existingWeapon.addMod("eternal");
					existingWeapon.addMod("cursed");
				}*/
				return true;
			}
			// 如果單位沒有持有 "指定武器" 且擁有指定狀態，則繼續進行裝備流程
			// 獲取 "指定武器" 物品資產
			var weaponAsset = AssetManager.items.get(WeapontID);
			if (weaponAsset == null)
			{
				return false; // 無法找到物品資產，返回失敗
			}
			// 生成 "指定武器" 物品實例
			var newItem = World.world.items.generateItem(pItemAsset: weaponAsset);
			// =======================================================
			// === 核心修正：強制添加修飾符 ===
			if (newItem != null)
			{
				//newItem.addMod("eternal");
				newItem.addMod("power5");
				newItem.addMod("truth5");
				newItem.addMod("protection5");
				newItem.addMod("speed5");
				newItem.addMod("balance5");
				newItem.addMod("health5");
				newItem.addMod("finesse5");
				newItem.addMod("mastery5");
				newItem.addMod("knowledge5");
				newItem.addMod("sharpness5");
			}
			// =======================================================
			// 將 "指定武器" 裝備到目標的武器槽位
			weaponSlot.setItem(newItem, targetActor);
			// 更新目標的統計數據，以反映新裝備帶來的變化
			targetActor.setStatsDirty();
			targetActor.data.health += 99999;
			return true; // 表示效果成功施加
		}
		public static bool Divorce(BaseSimObject pSelf, WorldTile pTile)
		{// 誘惑法 分別
			// 1. 基本安全檢查：確保施法者 pSelf 及其 Actor 組件存在且存活
			if (pSelf == null || pSelf.a == null)
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			if (pTile == null)
			{
				return false;
			}
			if (selfActor.hasTrait("evillaw_seduction") && selfActor.hasTrait("slave"))
			{
				return false;
			}
			// 為了專注於單目標，將範圍維持在 0
			int range = 0; 
			bool effectAppliedToAnyone = false; 
			// 2. 獲取範圍內的 Actor
			var allClosestUnits = Finder.getUnitsFromChunk(pTile, range);
			if (allClosestUnits.Any())
			{
				foreach (var unit in allClosestUnits)
				{
					Actor targetActor = unit.a;
					// 檢查目標：有效、存活、不是施法者、屬於敵對王國
					if (targetActor != null && targetActor.isAlive() && targetActor != selfActor && targetActor.kingdom != selfActor.kingdom)
					{
						// --- 3. 檢查目標是否擁有不可誘惑特質 (新條件) ---
						bool isUnconvertible = false;
						foreach (string trait in unconvertibleTraits)
						{
							if (targetActor.hasTrait(trait))
							{
								isUnconvertible = true;
								break;
							}
						}
						// --- 4. 核心效果觸發邏輯 ---
						// 只有當目標是 King/Leader 且滿足下列任一條件時，才執行效果：
						if (targetActor.getProfession() == UnitProfession.King || targetActor.getProfession() == UnitProfession.Leader)
						{
							if (targetActor.hasStatus("afterglow") || isUnconvertible)
							{
								targetActor.setLover(null);
								targetActor.finishStatusEffect("afterglow");
								effectAppliedToAnyone = true;
							}
						}
					}
				}
			}
			
			return effectAppliedToAnyone;
		}
		public static bool independence0(BaseSimObject pTarget, WorldTile pTile = null)
		{// 誘惑法 離家獨立
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor currentUnit = pTarget.a;
			// 檢查單位是否有家庭，如果沒有就不用執行了
			if (currentUnit.family == null)
			{
				return false;
			}
			// 檢查單位是否擁有 "evillaw_seduction" 特質
			if (currentUnit.hasTrait("evillaw_seduction"))
			{
				Family unitFamily = currentUnit.family;
				if (unitFamily.isMainFounder(currentUnit))
				{
					return false;
				}
				// 如果條件符合，執行自立門戶邏輯
				unitFamily.units.Remove(currentUnit);
				currentUnit.family = null;
				currentUnit.data.age_overgrowth += 15;
				//TraitChangeAsset(currentUnit, pTarget, pTile);
				// 可選：可以添加一個 WorldLog 訊息來記錄這個事件
				// WorldLog.logDetachFromFamily(currentUnit, unitFamily);
				return true;
			}
			return false;
		}
		public static bool independence(BaseSimObject pTarget, WorldTile pTile = null)
		{// 誘惑法 除籍
			// 1. 基本安全檢查與施法者定義
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pTarget.a; // 施法者
			//string selfAssetId = selfActor.asset.id;
			//ActorAsset selfAsset = World.world.unitAssets.get(selfAssetId);
			// 如果無法獲取到施法者的 Asset 物件，則直接返回
			//if (selfAsset == null)
			//{
			//	return false;
			//}
			// 2. 參數設定與施法者檢查
			int maxRange = 10;
			// 施法者必須有城市，才能提供歸屬地
			if (selfActor.subspecies == null || selfActor.family == null || !selfActor.family.isMainFounder(selfActor))
			{
				return false;
			}
			// 3. 獲取搜索地塊
			WorldTile tTile = pTile ?? pTarget.current_tile;
			var allClosestUnits = Finder.getUnitsFromChunk(tTile, maxRange); // <-- 修正範圍
			bool effectApplied = false;
			if (allClosestUnits.Any())
			{
				foreach (var unit in allClosestUnits)
				{
					// 基礎安全檢查
					if (unit == null || !unit.isActor() || !unit.a.isAlive())
						continue;
					Actor targetActor = unit.a; // 正確宣告目標 Actor
					if (targetActor.kingdom == selfActor.kingdom &&
						targetActor.family == selfActor.family &&
						!targetActor.family.isMainFounder(targetActor) &&
						targetActor.hasTrait("evillaw_seduction"))
					{
						selfActor.family.units.Remove(targetActor);
						targetActor.family = null;
						if(selfActor.clan != null)
						{	// 只有當目標不在施法者氏族時才加入，避免重複
							if (targetActor.clan != selfActor.clan)
							{
								targetActor.clan = selfActor.clan;
								selfActor.clan.units.Add(targetActor);
							}
						}
						//if (targetActor.asset.id != selfAssetId)
						//{// 使用正確的 API 函式來替換物種 Asset
						//	targetActor.setAsset(selfAsset); 
						//}
						targetActor.data.age_overgrowth += 15;
						// **D. 傳送回歸**
						teleportToHomeCity(selfActor, targetActor, pTile);
						effectApplied = true;
					}
				}
			}
			return effectApplied; // 只有在至少一個單位被歸化時，才返回 true
		}
		public static bool SmartKillerEffect(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 滅智法 智慧殺手 魔王狀態
			// 1. 基本安全檢查：確保自身和目標有效且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive() || pTarget == null || pTarget.a == null)
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			Actor targetActor = pTarget.a;
			// 2. 檢查自身是否已經處於傲慢魔王或其冷卻狀態
			if (selfActor.hasStatus("arrogant_demon_king") || selfActor.hasStatus("cdt_adk"))
			{
				return false;
			}
			// 3. 檢查自身是否擁有文明特質（這是發動能力的前提）
			if (selfActor.subspecies == null || !selfActor.subspecies.hasTrait("prefrontal_cortex"))
			{
				return false;
			}
			// 4. 如果攻擊對象與自己是同一亞種，則能力不發動
			if (selfActor.subspecies == targetActor.subspecies)
			{
				return false;
			}
			// 5. 檢查目標是否擁有文明特質
			if (targetActor.subspecies == null || !targetActor.subspecies.hasTrait("prefrontal_cortex"))
			{
				return false;
			}
			// ====== 檢查王國內是否已存在其他魔王 ======
			bool hasOtherDemonKing = false;
			Kingdom currentKingdom = selfActor.kingdom;
			if (currentKingdom != null)
			{
				foreach (Actor kingdomUnit in currentKingdom.units)
				{
					if (kingdomUnit == null || kingdomUnit == selfActor)
					{
						continue;
					}
					if (kingdomUnit.hasTrait("hope")||
						kingdomUnit.hasTrait("other6661")||
						kingdomUnit.hasTrait("other6662")||
						kingdomUnit.hasTrait("other6663")||
						kingdomUnit.hasTrait("other6664")||
						kingdomUnit.hasTrait("other6665")||
						kingdomUnit.hasTrait("other6666")||
						kingdomUnit.hasTrait("other6667")||
						kingdomUnit.hasTrait("other6668")||
						kingdomUnit.hasTrait("other6669"))
					{
						hasOtherDemonKing = true;
						break;
					}
					foreach (string demonKingStatusID in SevenDemonKingStatus_DemonKing)
					{
						if (kingdomUnit.hasStatus(demonKingStatusID))
						{
							hasOtherDemonKing = true;
							break;
						}
					}
					if (hasOtherDemonKing)
					{
						break;
					}
				}
			}
			// ===================================
			// 根據是否存在其他魔王來決定行為
			if (!hasOtherDemonKing)
			{
				selfActor.addStatusEffect("arrogant_demon_king", 3600f);
				EvilSwordGet(pSelf, pTile);
				Items01Actions.EvilSwordAwakens(pSelf, pTile);
				// 以下是原本的滅智法核心邏輯，現在被放在了這裡
				if (targetActor.subspecies != null)
				{
					targetActor.subspecies.removeTrait("prefrontal_cortex");
					targetActor.subspecies.removeTrait("advanced_hippocampus");
					targetActor.subspecies.removeTrait("wernicke_area");
					targetActor.subspecies.removeTrait("amygdala");
				}
				return true;
			}
			else
			{
				selfActor.addStatusEffect("cdt_adk", 3600f);
				if (targetActor.subspecies != null)
				{
					//targetActor.subspecies.removeTrait("prefrontal_cortex");
					targetActor.subspecies.removeTrait("advanced_hippocampus");
					targetActor.subspecies.removeTrait("wernicke_area");
					targetActor.subspecies.removeTrait("amygdala");
				}
				return true;
			}
		}
		public static bool Ready(BaseSimObject pSelf, WorldTile pTile = null)
		{// 傲慢法 等待狀態
			// 1. 基本安全檢查：確保 pSelf 及其 Actor 組件存在且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) 
				return false;
			
			Actor selfActor = pSelf.a;
			
			// === 檢查王國內是否已存在其他魔王 ===
			bool hasOtherDemonKing = false;
			Kingdom currentKingdom = selfActor.kingdom;
			if (currentKingdom != null)
			{
				foreach (Actor kingdomUnit in currentKingdom.units)
				{
					if (kingdomUnit == null || kingdomUnit == selfActor)
					{
						continue;
					}
					if (kingdomUnit.hasTrait("hope")||
						kingdomUnit.hasTrait("other6661")||
						kingdomUnit.hasTrait("other6662")||
						kingdomUnit.hasTrait("other6663")||
						kingdomUnit.hasTrait("other6664")||
						kingdomUnit.hasTrait("other6665")||
						kingdomUnit.hasTrait("other6666")||
						kingdomUnit.hasTrait("other6667")||
						kingdomUnit.hasTrait("other6668")||
						kingdomUnit.hasTrait("other6669"))
					{
						hasOtherDemonKing = true;
						break;
					}
					// 假設 SevenDemonKingStatus_DemonKing 已經設定為 public 或 internal
					foreach (string demonKingStatusID in Traits01Actions.SevenDemonKingStatus_DemonKing)
					{
						if (kingdomUnit.hasStatus(demonKingStatusID))
						{
							hasOtherDemonKing = true;
							break;
						}
					}
					if (hasOtherDemonKing)
					{
						break;
					}
				}
			}
			
			// === 根據是否存在其他魔王來決定 cdt_adk01 的持續時間 ===
			float cooldownDuration;
			if (!hasOtherDemonKing)
			{
				// 如果沒有其他魔王，則設置為較長的等待時間
				cooldownDuration = 3600f;
			}
			else
			{
				// 如果有其他魔王，則設置為較短的等待時間，以便快速重新嘗試
				cooldownDuration = 4.0f;
			}
			
			// === 只有在沒有 'arrogant_demon_king' 或 'cdt_adk01' 狀態時才添加冷卻 ===
			if (!selfActor.hasStatus("arrogant_demon_king") && !selfActor.hasStatus("cdt_adk01"))
			{
				selfActor.addStatusEffect("cdt_adk01", cooldownDuration);
			}
			
			return false; // 特殊效果通常返回 false，表示不阻止其他效果链
		}
		public static bool Ready2(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 傲慢法 被攻擊添加
			// 1. 基本安全檢查：確保 pSelf 及其 Actor 組件存在且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) 
				return false;
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) 
				return false;
			Actor selfActor = pSelf.a;
			Actor targetActor = pTarget.a;

			if (!selfActor.hasStatus("cdt_adk") && !selfActor.hasStatus("cdt_adk01"))
			{
				return false;
			}
			// === 檢查王國內是否已存在其他魔王 ===
			bool hasOtherDemonKing = false;
			Kingdom currentKingdom = selfActor.kingdom;
			if (currentKingdom != null)
			{
				foreach (Actor kingdomUnit in currentKingdom.units)
				{
					if (kingdomUnit == null || kingdomUnit == selfActor)
					{
						continue;
					}
					if (kingdomUnit.hasTrait("hope")||
						kingdomUnit.hasTrait("other6661")||
						kingdomUnit.hasTrait("other6662")||
						kingdomUnit.hasTrait("other6663")||
						kingdomUnit.hasTrait("other6664")||
						kingdomUnit.hasTrait("other6665")||
						kingdomUnit.hasTrait("other6666")||
						kingdomUnit.hasTrait("other6667")||
						kingdomUnit.hasTrait("other6668")||
						kingdomUnit.hasTrait("other6669"))
					{
						hasOtherDemonKing = true;
						break;
					}
					// 假設 SevenDemonKingStatus_DemonKing 已經設定為 public 或 internal
					foreach (string demonKingStatusID in Traits01Actions.SevenDemonKingStatus_DemonKing)
					{
						if (kingdomUnit.hasStatus(demonKingStatusID))
						{
							hasOtherDemonKing = true;
							break;
						}
					}
					if (hasOtherDemonKing)
					{
						break;
					}
				}
			}
			
			// === 根據是否存在其他魔王來決定 cdt_adk01 的持續時間 ===
			float cooldownDuration;
			if (!hasOtherDemonKing)
			{
				// 如果沒有其他魔王，則設置為較長的等待時間
				cooldownDuration = 3600f;
			}
			else
			{
				// 如果有其他魔王，返回 false
				return false;
			}
			if (!selfActor.hasStatus("arrogant_demon_king"))
			{
				selfActor.addStatusEffect("arrogant_demon_king", cooldownDuration);
				EvilSwordGet(pSelf, pTile);
				Items01Actions.EvilSwordAwakens(pSelf, pTile);
			}
			
			return false; // 特殊效果通常返回 false，表示不阻止其他效果链
		}
		public static bool EvilSwordGet(BaseSimObject pTarget, WorldTile pTile = null)
		{// 滅智法 武器給予
			// 定義所需的狀態ID常量
			const string DemonKingStatus = "arrogant_demon_king";
			const string WeapontID = "evil_sword"; // 物品ID
			// 1. 基本安全檢查：目標是否有效且存活
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor targetActor = pTarget.a;
			// ****** 新增檢查：單位是否擁有 '指定魔王狀態' 狀態 ******
			if (!targetActor.hasStatus(DemonKingStatus))
			{
				return false; // 如果沒有此狀態，則不給予裝備
			}
			// ************************************************************
			// 獲取目標的武器槽位
			var weaponSlot = targetActor.equipment.getSlot(EquipmentType.Weapon);
			// 檢查武器槽位是否已被佔用，且其中持有的道具ID是否為 "指定武器"
			if (weaponSlot != null && weaponSlot.getItem() != null && weaponSlot.getItem().asset.id == WeapontID)
			{
				// 額外檢查：如果武器沒有修飾符，在這裡補上
				Item existingWeapon = weaponSlot.getItem();
				/*if (!existingWeapon.hasMod("cursed"))
				{
					existingWeapon.addMod("eternal");
					existingWeapon.addMod("cursed");
				}*/
				return true;
			}
			// 如果單位沒有持有 "指定武器" 且擁有指定狀態，則繼續進行裝備流程
			// 獲取 "指定武器" 物品資產
			var weaponAsset = AssetManager.items.get(WeapontID);
			if (weaponAsset == null)
			{
				return false; // 無法找到物品資產，返回失敗
			}
			// 生成 "指定武器" 物品實例
			var newItem = World.world.items.generateItem(pItemAsset: weaponAsset);
			// =======================================================
			// === 核心修正：強制添加修飾符 ===
			if (newItem != null)
			{
				//newItem.addMod("eternal");
				newItem.addMod("power5");
				newItem.addMod("truth5");
				newItem.addMod("protection5");
				newItem.addMod("speed5");
				newItem.addMod("balance5");
				newItem.addMod("health5");
				newItem.addMod("finesse5");
				newItem.addMod("mastery5");
				newItem.addMod("knowledge5");
				newItem.addMod("sharpness5");
			}
			// =======================================================
			// 將 "指定武器" 裝備到目標的武器槽位
			weaponSlot.setItem(newItem, targetActor);
			// 更新目標的統計數據，以反映新裝備帶來的變化
			targetActor.setStatsDirty();
			targetActor.data.health += 99999;
			return true; // 表示效果成功施加
		}
	// 奴隸
		public static bool SlaveUnit(BaseSimObject pTarget, WorldTile pTile = null)
		{// 奴隸效果
			const string FellInLoveStatusID = "fell_in_love";
			const float HEALTH_TRIGGER_PERCENTAGE = 0.99f;
			const float HEALTH_RESTORE_PERCENTAGE = 0.01f;
			const float STAMINA_TRIGGER_PERCENTAGE = 0.99f;
			const float STAMINA_RESTORE_PERCENTAGE = 0.01f;
			const float NUTRITION_TRIGGER_PERCENTAGE = 0.99f;
			const int NUTRITION_RESTORE_AMOUNT = 1;
			const int MAX_NUTRITION_VALUE = 100;
			if (pTarget?.a == null || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor beast = pTarget.a;
			Actor master = null;
			// === 核心邏輯：單次物種修正 ===
			// 確保奴隸單位有特質，並且 subspecies 不為空 //species_fixed
			if (beast.hasTrait("slave") && beast.subspecies != null &&(!beast.hasTrait("battle_reflexes") || !beast.hasTrait("arcane_reflexes")))
			{
				string childAssetId = beast.asset.id;
				long childSubspeciesId = beast.subspecies.id;
				
				if (childAssetId != childSubspeciesId.ToString())
				{
					Subspecies correctSubspecies = MapBox.instance.subspecies.get(childSubspeciesId);
					if (correctSubspecies != null)
					{
						beast.setSubspecies(correctSubspecies);
					}
				}				//species_fixed
				beast.addTrait("arcane_reflexes");
				beast.addTrait("battle_reflexes");
				beast.addTrait("pro_soldier");
			}
			// === 檢查並從 custom_data 恢復主人關係 ===
			if (!Main.listOfTamedBeasts.ContainsKey(beast))
			{
				if (beast.data?.custom_data_long != null &&
					beast.data.custom_data_long.TryGetValue("master_id", out long masterId))
				{
					master = World.world.units.get(masterId);
					if (master != null)
					{
						// 恢復關係並加入到我們的追蹤列表
						Main.listOfTamedBeasts[beast] = master;
						return true;
					}
				}
				// 如果找不到主人，移除奴隸特質
				beast.removeTrait("slave");
				beast.removeTrait("arcane_reflexes");
				beast.removeTrait("battle_reflexes");
				beast.removeTrait("pro_soldier");
				return false;
			}
			// 從追蹤列表中取得主人
			master = Main.listOfTamedBeasts[beast];
			// 如果主人不存在或已死亡，清理奴隸關係
			if (master == null || !master.isAlive())
			{
				beast.removeTrait("slave");
				beast.removeTrait("arcane_reflexes");
				beast.removeTrait("battle_reflexes");
				beast.removeTrait("slave");
				Main.listOfTamedBeasts.Remove(beast);
				beast.setLover(null);
				if (beast.family != null)
				{
					beast.family.units.Remove(beast);
					beast.family = null;
				}
				return false;
			}
			// === 主人存活時的邏輯 ===
			// 奴隸給予主人恢復效果
			float masterMaxHealth = master.getMaxHealth();
			float healthToRestore = masterMaxHealth * HEALTH_RESTORE_PERCENTAGE;
			if (master.data.health / masterMaxHealth < HEALTH_TRIGGER_PERCENTAGE)
			{
				master.restoreHealth(Mathf.RoundToInt(healthToRestore));
				beast.data.health -= Mathf.RoundToInt(healthToRestore);
				if (beast.data.health < 0) beast.data.health = 0;
			}
			
			float masterMaxStamina = master.getMaxStamina();
			float staminaToRestore = masterMaxStamina * STAMINA_RESTORE_PERCENTAGE;
			if (master.data.stamina / masterMaxStamina < STAMINA_TRIGGER_PERCENTAGE)
			{
				master.data.stamina += Mathf.RoundToInt(staminaToRestore);
				if (master.data.stamina > masterMaxStamina)
				{
					master.data.stamina += Mathf.RoundToInt(staminaToRestore);
				}
				beast.data.stamina -= Mathf.RoundToInt(staminaToRestore);
				if (beast.data.stamina < 0) beast.data.stamina = 0;
			}
			
			float nutritionMaster = master.data.nutrition;
			if (nutritionMaster / MAX_NUTRITION_VALUE < NUTRITION_TRIGGER_PERCENTAGE)
			{
				master.data.nutrition = Mathf.Min(MAX_NUTRITION_VALUE, master.data.nutrition + NUTRITION_RESTORE_AMOUNT);
				beast.data.nutrition -= NUTRITION_RESTORE_AMOUNT;
				if (beast.data.nutrition < 0) beast.data.nutrition = 0;
			}
			// 同步設定區
			// === 雙向同步：戶籍與關係 ===
			if (beast.city != master.city)
			{// 城市同步：確保奴隸和主人屬於同一個城市
				// 情況 1: 主人有城市 -> 奴隸跟隨
				if (beast.city == null)
				{
					if (master.city != null)
					{
						beast.city = master.city;
						master.city.units.Add(beast);
					}
				}
				// 情況 2: 逆向寄生 主人沒有城市但奴隸有
				else if (beast.city != null)
				{
					if (master.city == null)
					{
						master.city = beast.city;
						master.kingdom = beast.kingdom;
						beast.city.units.Add(master);
					}
				}
			}
			if (beast.kingdom != master.kingdom)
			{// 國家同步：確保奴隸和主人屬於同一個國家
				if (beast.kingdom != null)
				{
					beast.kingdom.units.Remove(beast);
				}
				beast.kingdom = master.kingdom;
				if (master.kingdom != null)
				{
					master.kingdom.units.Add(beast);
				}
			}
			if (beast.city != null && beast.city.kingdom != master.kingdom)
			{// 當奴隸城市不為空,奴隸所屬城市王國不歸屬王國時
				beast.city.units.Remove(beast);
			}
			if (beast.family != master.family)
			{// 家庭同步：確保奴隸和主人屬於同一個家庭
				if (beast.family != null)
				{
					beast.family.units.Remove(beast);
				}
				beast.family = master.family;
				if (master.family != null)
				{
					master.family.units.Add(beast);
				}
			}
			if (beast.subspecies != null)
			{// 如果奴隸單位亞種不為空
				if (beast.subspecies.hasTrait("advanced_hippocampus"))
				{// 持有主管文明宗教的advanced_hippocampus亞種特質為前提
					if (beast.culture != master.culture)
					{// 文明同步：確保奴隸和主人屬於同一個文明
						if (beast.culture != null)
						{
							beast.culture.units.Remove(beast);
						}
						beast.culture = master.culture;
						if (master.culture != null)
						{
							master.culture.units.Add(beast);
						}
					}
					if (beast.religion != master.religion)
					{// 宗教同步：確保奴隸和主人屬於同一個宗教
						if (beast.religion != null)
						{
							beast.religion.units.Remove(beast);
						}
						beast.religion = master.religion;
						if (master.religion != null)
						{
							master.religion.units.Add(beast);
						}
					}
				}
				if (beast.subspecies.hasTrait("wernicke_area"))
				{// 持有主管言語的wernicke_area亞種特質為前提
					if (beast.language != master.language)
					{// 言語同步：確保奴隸和主人屬於同一個言語
						if (beast.language != null)
						{
							beast.language.units.Remove(beast);
						}
						beast.language = master.language;
						if (master.language != null)
						{
							master.language.units.Add(beast);
						}
					}
				}
			}
			if (beast.isKing() && master.kingdom != null)
			{// 職位讓渡 國王
				// 1. 移除奴隸的國王身分
				Kingdom slaveKingdom = beast.kingdom;
				if (slaveKingdom != null && slaveKingdom.king == beast)
				{
					beast.setProfession(UnitProfession.Unit); // 將奴隸職業重置為普通單位
					beast.removeTrait("pro_king"); // 移除國王特質
					slaveKingdom.king = null; // 清空王國的國王引用
				}
				// 2. 將主人設為國王
				Kingdom masterKingdom = master.kingdom;
				if (masterKingdom != null)
				{
					// 如果主人是城市的領袖，先移除領袖身分
					if (master.isCityLeader())
					{
						master.city.removeLeader();
					}
					masterKingdom.king = master; // 設定王國的國王
					master.setProfession(UnitProfession.King); // 設定職業為國王
					WorldLog.logNewKing(masterKingdom);
					// 可選: 觸發國王就職的視覺效果
					master.startShake();
					master.startColorEffect();
				}
			}
			if (beast.isCityLeader() && beast.city == master.city)
			{// 職位讓渡 領主
				City city = beast.city;
				
				// **核心修正點**：主人讓位邏輯
				// 只有當奴隸是當前城市領袖時，我們才介入職位變更
				if (city.leader == beast)
				{
					// 情況 A: 主人是普通單位 (平民/Unit)
					if (master.getProfession() == UnitProfession.Unit) 
					{
						city.removeLeader(); 
						city.setLeader(master, true); 
						master.startShake();
						master.startColorEffect();
					}
					else 
					{
						
					}
				}
			}
			if (beast.clan == master.clan && beast.clan != null)
			{// 職位讓渡 族長
				Clan currentClan = beast.clan;
				// 檢查奴隸是否為該氏族的現任首領
				if (currentClan.getChief() == beast)
				{
					// 核心邏輯：將首領職位轉讓給主人
					// setChief 會自動處理舊首領的歷史記錄
					currentClan.setChief(master);
					// 可選：如果你希望主人在接任後獲得視覺效果
					master.startShake();
					master.startColorEffect();
				}
				// 將奴隸從氏族中移除 (除籍)
				currentClan.units.Remove(beast);
				beast.clan = null; // 清除奴隸的氏族引用
			}
			if (beast.getProfession() == UnitProfession.Leader)
			{// 職位能力
				beast.addStatusEffect("slave00", 60f);
			}
			else
			{
				beast.finishStatusEffect("slave00");
			}
			if (master.data.sex != beast.data.sex && !master.hasStatus(FellInLoveStatusID))
			{// 關係狀態同步
				master.addStatusEffect(FellInLoveStatusID, 30f);
				master.setLover(beast);
			}
			// 清除主人負面情緒
			if (master.hasStatus("tantrum") || master.hasStatus("angry"))
			{
				master.finishStatusEffect("antibody");
				master.finishStatusEffect("tantrum");
				master.finishStatusEffect("angry");
				master.addStatusEffect("stunned", 4f);
			}
			// 自身憤怒情緒處理
			if (beast.hasStatus("angry"))
			{
				beast.finishStatusEffect("antibody");
				beast.finishStatusEffect("angry");
				beast.addStatusEffect("stunned", 4f);
			}
			// 幸福度維持
			if (master.a.data.happiness < 0)
			{
				master.a.data.happiness = +10;
			}
			// 隨機跟隨主人
			if (Randy.randomChance(0.01f))
			{
				beast.goTo(master.current_tile);
			}
			return true;
		}
		public static bool TransferUnitLeader(BaseSimObject pTarget, WorldTile pTile = null)
		{// 領主職位轉讓 (Transfer)：將王國內的指定單位提升為領袖
			// 1. 基本安全檢查
			if (pTarget?.a == null || !pTarget.a.isAlive())
			{
				return false;
			}
			
			Actor selfActor = pTarget.a;
			
			// 2. 條件檢查：執行者必須是領主 (Leader)
			if (selfActor.getProfession() != UnitProfession.Leader)
			{
				return false;
			}

			// 2.5 確定執行者目前擔任領袖的城市
			City selfCity = selfActor.city;
			if (selfCity == null || selfCity.leader != selfActor)
			{
				// 如果 selfActor 不是任何城市的領袖，或其城市是 null，則不能進行轉讓
				return false;
			}
			
			Kingdom selfKingdom = selfActor.kingdom;
			
			// 3. 條件檢查：執行者必須隸屬於一個存活的王國
			if (selfKingdom == null || !selfKingdom.isAlive())
			{
				return false;
			}
			
			// 4. 在王國內尋找合適的目標單位 (正統人選)
			Actor targetToPromote = null;
			
			// 遍歷王國中的所有單位
			foreach (Actor unit in selfKingdom.units)
			{
				// 檢查條件：
				// a) 單位必須存活、與執行者不同
				if (unit == null || !unit.isAlive() || unit == selfActor)
					continue;
					
				// b) 單位不能是國王或領主 (只能是平民 Unit)
				if (unit.getProfession() != UnitProfession.Unit)
					continue;
					
				// c) 單位必須攜帶 'evillaw_seduction' 特質
				if (unit.hasTrait("evillaw_seduction"))
				{
					// 找到第一個符合條件的單位
					targetToPromote = unit;
					break; 
				}
			}
			
			// 5. 執行晉升：如果找到目標
			if (targetToPromote != null)
			{
				// **核心修正 A: 讓 selfActor 讓位**
				selfCity.removeLeader(); 

				// **核心修正 B: 將目標單位強制移動到這個城市**
				// 這是確保 setLeader 成功和避免 NRE 的關鍵
				if (targetToPromote.city != selfCity)
				{
					// 從目標單位的舊城市移除
					if (targetToPromote.city != null)
					{
						targetToPromote.city.units.Remove(targetToPromote);
					}
					// 設置目標單位的城市為 selfCity
					targetToPromote.city = selfCity;
					selfCity.units.Add(targetToPromote);
				}

				// **核心修正 C: 進行晉升**
				
				// a) 將目標單位設為城市領袖 (selfCity.leader 已經是 null)
				selfCity.setLeader(targetToPromote, true); 
				
				// b) 確保目標單位的職業設為 Leader
				targetToPromote.setProfession(UnitProfession.Leader);
				
				// c) 觸發視覺效果
				targetToPromote.startShake();
				targetToPromote.startColorEffect();
				
				return true;
			}
			
			return false; // 沒有找到合適的目標單位
		}
		public static bool addfell_in_love(BaseSimObject pSelf, WorldTile pTile = null)
		{// 奴隸效果 愛河
			// 1. 基本安全检查：确保 pSelf 及其 Actor 组件存在且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) 
				return false;
			Actor selfActor = pSelf.a;
			// 2. 特质持有者自身添加条件 
			selfActor.addStatusEffect("fell_in_love", 10f);//
			selfActor.addStatusEffect("slave", 10f);//
			//selfActor.finishStatusEffect("angry");//
			selfActor.finishStatusEffect("tantrum");//
			return false; // 特殊效果通常返回 false，表示不阻止其他效果链
		}
		public static bool nutrition1(BaseSimObject pTarget, WorldTile pTile = null)
		{// 營養值
			// 檢查目標單位是否有效
			if (pTarget?.a == null)
			{
				return false;
			}
			Actor actor = pTarget.a;
			const int MAX_NUTRITION_VALUE = 100;
			const int NUTRITION_RESTORE_AMOUNT = 12;

			// 如果營養值小於最大值，就進行補充
			if (actor.data.nutrition < MAX_NUTRITION_VALUE)
			{
				// 使用 Mathf.Min 確保營養值不會超過上限
				actor.data.nutrition = Mathf.Min(MAX_NUTRITION_VALUE, actor.data.nutrition + NUTRITION_RESTORE_AMOUNT);
				return true;
			}
			return false;
		}
		public static bool SlaveUnitAI(BaseSimObject pTarget, WorldTile pTile)
		{// 奴隸效果 AI
			if (pTarget?.a == null || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor actor = pTarget.a;
			var pAI = (AiSystemActor)Reflection.GetField(typeof(Actor), actor, "ai");
			if (pAI != null) // 確保 AI 系統存在
			{
				pAI.setJob("attacker");
			}
			return true;
		}
		private static readonly Dictionary<string, (string traitId, string statusId)> TraitMap = new Dictionary<string, (string, string)>
		{// 特定奴隸物種 特質 狀態
			{ "demon", ("burning_feet", "fighting") },
			{ "snowman", ("cold_aura", "fighting") },
			{ "cold_one", ("cold_aura", "fighting") },
			{ "civ_acid_gentleman", ("acid_touch", "fighting") },
			{ "acid_blob", ("acid_touch", "fighting") }
		};
		public static bool SlaveTraitAdd(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 特定奴隸特質添加
			// 1. 基本安全檢查
			if (pSelf == null || pSelf.a == null)
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			string selfAssetId = selfActor.asset.id;
			// 2. 檢查字典是否包含當前物種ID
			if (TraitMap.ContainsKey(selfAssetId))
			{	// 3. 從字典中取出對應的特質和狀態ID
				var traitAndStatus = TraitMap[selfAssetId];
				string requiredTraitId = traitAndStatus.traitId;
				string statusEffectId = traitAndStatus.statusId;
				// 4. 檢查單位是否已擁有該特質，如果沒有，則添加
				if (!selfActor.hasTrait(requiredTraitId))
				{
					selfActor.addTrait(requiredTraitId);
					selfActor.addStatusEffect(statusEffectId, 5f);
				}
			}
			return false;
		}
		public static bool SlaveTraitRemove(BaseSimObject pTarget, WorldTile pTile)
		{// 特定奴隸特質移除
			if (pTarget == null || pTarget.a == null)
			{
				return false;
			}
			Actor actor = pTarget.a;
			// 檢查單位是否未持有 "fighting" 狀態
			if (!actor.hasStatus("fighting"))
			{
				actor.removeTrait("burning_feet");
				actor.removeTrait("cold_aura");
				actor.removeTrait("acid_touch");
			}
			return true;
		}
			#endregion
			#region 環境改造類
		public static bool AlteredSurface_01(BaseSimObject pSelf, WorldTile pTile = null)
		{// 01 草地種 seeds_grass		biome_grass
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			WorldTile currentTile = pSelf.current_tile;
			if (currentTile == null)
			{
				return false;
			}
			// 檢查 Biome ID
			BiomeAsset currentBiome = currentTile.getBiome();
			if (currentBiome != null && currentBiome.id == "biome_grass")
			{
				return false;
			}
			if (currentTile.Type != null && UnusablePlots.Contains(currentTile.Type.id))
			{
				return false;
			}
			// 2. 冷卻狀態檢查
			string cooldownStatus = "cdt_alter01";
			float cooldownDuration = 3600f; // 你可以根據需要調整冷卻時間，單位為秒
			// 如果單位已經有冷卻狀態，則直接返回，不執行能力
			if (pSelf.a.hasStatus(cooldownStatus))
			{
				return false;
			}
			// 3. 施加冷卻狀態
			pSelf.a.addStatusEffect(cooldownStatus, cooldownDuration);
			// 4. 設定掉落物 ID 和生成參數
			string dropId_01 = "seeds_grass";
			string dropId_02 = "fertilizer_plants";
			string dropId_03 = "fertilizer_trees";
			int numberOfDrops_01 = 1000;//數量
			float spreadRadius_01 = 20f;//半徑
			// 5. 確定生成中心瓦片
			if (pTile == null)
			{
				pTile = pSelf.current_tile;
			}
			if (pTile == null)
			{
				return false;
			}
			// 6. 進行多回掉落
			for (int i = 0; i < numberOfDrops_01; i++)
			{
				UnityEngine.Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spreadRadius_01;
				int newTileX = (int)pTile.pos.x + (int)randomOffset.x;
				int newTileY = (int)pTile.pos.y + (int)randomOffset.y;
				if (newTileX >= 0 && newTileX < MapBox.width &&
					newTileY >= 0 && newTileY < MapBox.height)
				{
					WorldTile randomTile = World.world.GetTile(newTileX, newTileY);
					if (randomTile != null)
					{
						World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_02, 1f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_03, 2f, -1f);
					}
				}
			}
			return true;
		}
		public static bool AlteredSurface_02(BaseSimObject pSelf, WorldTile pTile = null)
		{// 02 魔法種 seeds_enchanted	biome_enchanted
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			WorldTile currentTile = pSelf.current_tile;
			if (currentTile == null)
			{
				return false;
			}
			// 檢查 Biome ID
			BiomeAsset currentBiome = currentTile.getBiome();
			if (currentBiome != null && currentBiome.id == "biome_enchanted")
			{
				return false;
			}
			if (currentTile.Type != null && UnusablePlots.Contains(currentTile.Type.id))
			{
				return false;
			}
			// 2. 冷卻狀態檢查
			string cooldownStatus = "cdt_alter01";
			float cooldownDuration = 3600f; // 你可以根據需要調整冷卻時間，單位為秒
			// 如果單位已經有冷卻狀態，則直接返回，不執行能力
			if (pSelf.a.hasStatus(cooldownStatus))
			{
				return false;
			}
			// 3. 施加冷卻狀態
			pSelf.a.addStatusEffect(cooldownStatus, cooldownDuration);
			// 4. 設定掉落物 ID 和生成參數
			string dropId_01 = "seeds_enchanted";
			string dropId_02 = "fertilizer_plants";
			string dropId_03 = "fertilizer_trees";
			int numberOfDrops_01 = 1000;//數量
			float spreadRadius_01 = 20f;//半徑
			// 5. 確定生成中心瓦片
			if (pTile == null)
			{
				pTile = pSelf.current_tile;
			}
			if (pTile == null)
			{
				return false;
			}
			// 6. 進行多回掉落
			for (int i = 0; i < numberOfDrops_01; i++)
			{
				UnityEngine.Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spreadRadius_01;
				int newTileX = (int)pTile.pos.x + (int)randomOffset.x;
				int newTileY = (int)pTile.pos.y + (int)randomOffset.y;
				if (newTileX >= 0 && newTileX < MapBox.width &&
					newTileY >= 0 && newTileY < MapBox.height)
				{
					WorldTile randomTile = World.world.GetTile(newTileX, newTileY);
					if (randomTile != null)
					{
						World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_02, 1f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_03, 2f, -1f);
					}
				}
			}
			return true;
		}
		public static bool AlteredSurface_03(BaseSimObject pSelf, WorldTile pTile = null)
		{// 03 稀樹種 seeds_savanna		biome_savanna
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			WorldTile currentTile = pSelf.current_tile;
			if (currentTile == null)
			{
				return false;
			}
			// 檢查 Biome ID
			BiomeAsset currentBiome = currentTile.getBiome();
			if (currentBiome != null && currentBiome.id == "biome_savanna")
			{
				return false;
			}
			if (currentTile.Type != null && UnusablePlots.Contains(currentTile.Type.id))
			{
				return false;
			}
			// 2. 冷卻狀態檢查
			string cooldownStatus = "cdt_alter01";
			float cooldownDuration = 3600f; // 你可以根據需要調整冷卻時間，單位為秒
			// 如果單位已經有冷卻狀態，則直接返回，不執行能力
			if (pSelf.a.hasStatus(cooldownStatus))
			{
				return false;
			}
			// 3. 施加冷卻狀態
			pSelf.a.addStatusEffect(cooldownStatus, cooldownDuration);
			// 4. 設定掉落物 ID 和生成參數
			string dropId_01 = "seeds_savanna";
			string dropId_02 = "fertilizer_plants";
			string dropId_03 = "fertilizer_trees";
			int numberOfDrops_01 = 1000;//數量
			float spreadRadius_01 = 20f;//半徑
			// 5. 確定生成中心瓦片
			if (pTile == null)
			{
				pTile = pSelf.current_tile;
			}
			if (pTile == null)
			{
				return false;
			}
			// 6. 進行多回掉落
			for (int i = 0; i < numberOfDrops_01; i++)
			{
				UnityEngine.Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spreadRadius_01;
				int newTileX = (int)pTile.pos.x + (int)randomOffset.x;
				int newTileY = (int)pTile.pos.y + (int)randomOffset.y;
				if (newTileX >= 0 && newTileX < MapBox.width &&
					newTileY >= 0 && newTileY < MapBox.height)
				{
					WorldTile randomTile = World.world.GetTile(newTileX, newTileY);
					if (randomTile != null)
					{
						World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_02, 1f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_03, 2f, -1f);
					}
				}
			}
			return true;
		}
		public static bool AlteredSurface_04(BaseSimObject pSelf, WorldTile pTile = null)
		{// 04 腐朽種 seeds_corrupted	biome_corrupted
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			WorldTile currentTile = pSelf.current_tile;
			if (currentTile == null)
			{
				return false;
			}
			// 檢查 Biome ID
			BiomeAsset currentBiome = currentTile.getBiome();
			if (currentBiome != null && currentBiome.id == "biome_corrupted")
			{
				return false;
			}
			if (currentTile.Type != null && UnusablePlots.Contains(currentTile.Type.id))
			{
				return false;
			}
			// 2. 冷卻狀態檢查
			string cooldownStatus = "cdt_alter01";
			float cooldownDuration = 3600f; // 你可以根據需要調整冷卻時間，單位為秒
			// 如果單位已經有冷卻狀態，則直接返回，不執行能力
			if (pSelf.a.hasStatus(cooldownStatus))
			{
				return false;
			}
			// 3. 施加冷卻狀態
			pSelf.a.addStatusEffect(cooldownStatus, cooldownDuration);
			// 4. 設定掉落物 ID 和生成參數
			string dropId_01 = "seeds_corrupted";
			string dropId_02 = "fertilizer_plants";
			string dropId_03 = "fertilizer_trees";
			int numberOfDrops_01 = 1000;//數量
			float spreadRadius_01 = 20f;//半徑
			// 5. 確定生成中心瓦片
			if (pTile == null)
			{
				pTile = pSelf.current_tile;
			}
			if (pTile == null)
			{
				return false;
			}
			// 6. 進行多回掉落
			for (int i = 0; i < numberOfDrops_01; i++)
			{
				UnityEngine.Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spreadRadius_01;
				int newTileX = (int)pTile.pos.x + (int)randomOffset.x;
				int newTileY = (int)pTile.pos.y + (int)randomOffset.y;
				if (newTileX >= 0 && newTileX < MapBox.width &&
					newTileY >= 0 && newTileY < MapBox.height)
				{
					WorldTile randomTile = World.world.GetTile(newTileX, newTileY);
					if (randomTile != null)
					{
						World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_02, 1f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_03, 2f, -1f);
					}
				}
			}
			return true;
		}
		public static bool AlteredSurface_05(BaseSimObject pSelf, WorldTile pTile = null)
		{// 05 蘑菇種 seeds_mushroom	biome_mushroom
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			WorldTile currentTile = pSelf.current_tile;
			if (currentTile == null)
			{
				return false;
			}
			// 檢查 Biome ID
			BiomeAsset currentBiome = currentTile.getBiome();
			if (currentBiome != null && currentBiome.id == "biome_mushroom")
			{
				return false;
			}
			if (currentTile.Type != null && UnusablePlots.Contains(currentTile.Type.id))
			{
				return false;
			}
			// 2. 冷卻狀態檢查
			string cooldownStatus = "cdt_alter01";
			float cooldownDuration = 3600f; // 你可以根據需要調整冷卻時間，單位為秒
			// 如果單位已經有冷卻狀態，則直接返回，不執行能力
			if (pSelf.a.hasStatus(cooldownStatus))
			{
				return false;
			}
			// 3. 施加冷卻狀態
			pSelf.a.addStatusEffect(cooldownStatus, cooldownDuration);
			// 4. 設定掉落物 ID 和生成參數
			string dropId_01 = "seeds_mushroom";
			string dropId_02 = "fertilizer_plants";
			string dropId_03 = "fertilizer_trees";
			int numberOfDrops_01 = 1000;//數量
			float spreadRadius_01 = 20f;//半徑
			// 5. 確定生成中心瓦片
			if (pTile == null)
			{
				pTile = pSelf.current_tile;
			}
			if (pTile == null)
			{
				return false;
			}
			// 6. 進行多回掉落
			for (int i = 0; i < numberOfDrops_01; i++)
			{
				UnityEngine.Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spreadRadius_01;
				int newTileX = (int)pTile.pos.x + (int)randomOffset.x;
				int newTileY = (int)pTile.pos.y + (int)randomOffset.y;
				if (newTileX >= 0 && newTileX < MapBox.width &&
					newTileY >= 0 && newTileY < MapBox.height)
				{
					WorldTile randomTile = World.world.GetTile(newTileX, newTileY);
					if (randomTile != null)
					{
						World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_02, 1f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_03, 2f, -1f);
					}
				}
			}
			return true;
		}
		public static bool AlteredSurface_06(BaseSimObject pSelf, WorldTile pTile = null)
		{// 06 叢林種 seeds_jungle		biome_jungle
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			WorldTile currentTile = pSelf.current_tile;
			if (currentTile == null)
			{
				return false;
			}
			// 檢查 Biome ID
			BiomeAsset currentBiome = currentTile.getBiome();
			if (currentBiome != null && currentBiome.id == "biome_jungle")
			{
				return false;
			}
			if (currentTile.Type != null && UnusablePlots.Contains(currentTile.Type.id))
			{
				return false;
			}
			// 2. 冷卻狀態檢查
			string cooldownStatus = "cdt_alter01";
			float cooldownDuration = 3600f; // 你可以根據需要調整冷卻時間，單位為秒
			// 如果單位已經有冷卻狀態，則直接返回，不執行能力
			if (pSelf.a.hasStatus(cooldownStatus))
			{
				return false;
			}
			// 3. 施加冷卻狀態
			pSelf.a.addStatusEffect(cooldownStatus, cooldownDuration);
			// 4. 設定掉落物 ID 和生成參數
			string dropId_01 = "seeds_jungle";
			string dropId_02 = "fertilizer_plants";
			string dropId_03 = "fertilizer_trees";
			int numberOfDrops_01 = 1000;//數量
			float spreadRadius_01 = 20f;//半徑
			// 5. 確定生成中心瓦片
			if (pTile == null)
			{
				pTile = pSelf.current_tile;
			}
			if (pTile == null)
			{
				return false;
			}
			// 6. 進行多回掉落
			for (int i = 0; i < numberOfDrops_01; i++)
			{
				UnityEngine.Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spreadRadius_01;
				int newTileX = (int)pTile.pos.x + (int)randomOffset.x;
				int newTileY = (int)pTile.pos.y + (int)randomOffset.y;
				if (newTileX >= 0 && newTileX < MapBox.width &&
					newTileY >= 0 && newTileY < MapBox.height)
				{
					WorldTile randomTile = World.world.GetTile(newTileX, newTileY);
					if (randomTile != null)
					{
						World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_02, 1f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_03, 2f, -1f);
					}
				}
			}
			return true;
		}
		public static bool AlteredSurface_07(BaseSimObject pSelf, WorldTile pTile = null)
		{// 07 沙漠種 seeds_desert		biome_desert
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			WorldTile currentTile = pSelf.current_tile;
			if (currentTile == null)
			{
				return false;
			}
			// 檢查 Biome ID
			BiomeAsset currentBiome = currentTile.getBiome();
			if (currentBiome != null && currentBiome.id == "biome_desert")
			{
				return false;
			}
			if (currentTile.Type != null && UnusablePlots.Contains(currentTile.Type.id))
			{
				return false;
			}
			// 2. 冷卻狀態檢查
			string cooldownStatus = "cdt_alter01";
			float cooldownDuration = 3600f; // 你可以根據需要調整冷卻時間，單位為秒
			// 如果單位已經有冷卻狀態，則直接返回，不執行能力
			if (pSelf.a.hasStatus(cooldownStatus))
			{
				return false;
			}
			// 3. 施加冷卻狀態
			pSelf.a.addStatusEffect(cooldownStatus, cooldownDuration);
			// 4. 設定掉落物 ID 和生成參數
			string dropId_01 = "seeds_desert";
			string dropId_02 = "fertilizer_plants";
			string dropId_03 = "fertilizer_trees";
			int numberOfDrops_01 = 1000;//數量
			float spreadRadius_01 = 20f;//半徑
			// 5. 確定生成中心瓦片
			if (pTile == null)
			{
				pTile = pSelf.current_tile;
			}
			if (pTile == null)
			{
				return false;
			}
			// 6. 進行多回掉落
			for (int i = 0; i < numberOfDrops_01; i++)
			{
				UnityEngine.Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spreadRadius_01;
				int newTileX = (int)pTile.pos.x + (int)randomOffset.x;
				int newTileY = (int)pTile.pos.y + (int)randomOffset.y;
				if (newTileX >= 0 && newTileX < MapBox.width &&
					newTileY >= 0 && newTileY < MapBox.height)
				{
					WorldTile randomTile = World.world.GetTile(newTileX, newTileY);
					if (randomTile != null)
					{
						World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_02, 1f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_03, 2f, -1f);
					}
				}
			}
			return true;
		}
		public static bool AlteredSurface_08(BaseSimObject pSelf, WorldTile pTile = null)
		{// 08 檸檬種 seeds_lemon		biome_lemon
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			WorldTile currentTile = pSelf.current_tile;
			if (currentTile == null)
			{
				return false;
			}
			// 檢查 Biome ID
			BiomeAsset currentBiome = currentTile.getBiome();
			if (currentBiome != null && currentBiome.id == "biome_lemon")
			{
				return false;
			}
			if (currentTile.Type != null && UnusablePlots.Contains(currentTile.Type.id))
			{
				return false;
			}
			// 2. 冷卻狀態檢查
			string cooldownStatus = "cdt_alter01";
			float cooldownDuration = 3600f; // 你可以根據需要調整冷卻時間，單位為秒
			// 如果單位已經有冷卻狀態，則直接返回，不執行能力
			if (pSelf.a.hasStatus(cooldownStatus))
			{
				return false;
			}
			// 3. 施加冷卻狀態
			pSelf.a.addStatusEffect(cooldownStatus, cooldownDuration);
			// 4. 設定掉落物 ID 和生成參數
			string dropId_01 = "seeds_lemon";
			string dropId_02 = "fertilizer_plants";
			string dropId_03 = "fertilizer_trees";
			int numberOfDrops_01 = 1000;//數量
			float spreadRadius_01 = 20f;//半徑
			// 5. 確定生成中心瓦片
			if (pTile == null)
			{
				pTile = pSelf.current_tile;
			}
			if (pTile == null)
			{
				return false;
			}
			// 6. 進行多回掉落
			for (int i = 0; i < numberOfDrops_01; i++)
			{
				UnityEngine.Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spreadRadius_01;
				int newTileX = (int)pTile.pos.x + (int)randomOffset.x;
				int newTileY = (int)pTile.pos.y + (int)randomOffset.y;
				if (newTileX >= 0 && newTileX < MapBox.width &&
					newTileY >= 0 && newTileY < MapBox.height)
				{
					WorldTile randomTile = World.world.GetTile(newTileX, newTileY);
					if (randomTile != null)
					{
						World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_02, 1f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_03, 2f, -1f);
					}
				}
			}
			return true;
		}
		public static bool AlteredSurface_09(BaseSimObject pSelf, WorldTile pTile = null)
		{// 09 凍土種 seeds_permafrost	biome_permafrost
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			WorldTile currentTile = pSelf.current_tile;
			if (currentTile == null)
			{
				return false;
			}
			// 檢查 Biome ID
			BiomeAsset currentBiome = currentTile.getBiome();
			if (currentBiome != null && currentBiome.id == "biome_permafrost")
			{
				return false;
			}
			if (currentTile.Type != null && UnusablePlots.Contains(currentTile.Type.id))
			{
				return false;
			}
			// 2. 冷卻狀態檢查
			string cooldownStatus = "cdt_alter01";
			float cooldownDuration = 3600f; // 你可以根據需要調整冷卻時間，單位為秒
			// 如果單位已經有冷卻狀態，則直接返回，不執行能力
			if (pSelf.a.hasStatus(cooldownStatus))
			{
				return false;
			}
			// 3. 施加冷卻狀態
			pSelf.a.addStatusEffect(cooldownStatus, cooldownDuration);
			// 4. 設定掉落物 ID 和生成參數
			string dropId_01 = "seeds_permafrost";
			string dropId_02 = "fertilizer_plants";
			string dropId_03 = "fertilizer_trees";
			int numberOfDrops_01 = 1000;//數量
			float spreadRadius_01 = 20f;//半徑
			// 5. 確定生成中心瓦片
			if (pTile == null)
			{
				pTile = pSelf.current_tile;
			}
			if (pTile == null)
			{
				return false;
			}
			// 6. 進行多回掉落
			for (int i = 0; i < numberOfDrops_01; i++)
			{
				UnityEngine.Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spreadRadius_01;
				int newTileX = (int)pTile.pos.x + (int)randomOffset.x;
				int newTileY = (int)pTile.pos.y + (int)randomOffset.y;
				if (newTileX >= 0 && newTileX < MapBox.width &&
					newTileY >= 0 && newTileY < MapBox.height)
				{
					WorldTile randomTile = World.world.GetTile(newTileX, newTileY);
					if (randomTile != null)
					{
						World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_02, 1f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_03, 2f, -1f);
					}
				}
			}
			return true;
		}
		public static bool AlteredSurface_10(BaseSimObject pSelf, WorldTile pTile = null)
		{// 10 糖果種 seeds_candy		biome_candy
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			WorldTile currentTile = pSelf.current_tile;
			if (currentTile == null)
			{
				return false;
			}
			// 檢查 Biome ID
			BiomeAsset currentBiome = currentTile.getBiome();
			if (currentBiome != null && currentBiome.id == "biome_candy")
			{
				return false;
			}
			if (currentTile.Type != null && UnusablePlots.Contains(currentTile.Type.id))
			{
				return false;
			}
			// 2. 冷卻狀態檢查
			string cooldownStatus = "cdt_alter01";
			float cooldownDuration = 3600f; // 你可以根據需要調整冷卻時間，單位為秒
			// 如果單位已經有冷卻狀態，則直接返回，不執行能力
			if (pSelf.a.hasStatus(cooldownStatus))
			{
				return false;
			}
			// 3. 施加冷卻狀態
			pSelf.a.addStatusEffect(cooldownStatus, cooldownDuration);
			// 4. 設定掉落物 ID 和生成參數
			string dropId_01 = "seeds_candy";
			string dropId_02 = "fertilizer_plants";
			string dropId_03 = "fertilizer_trees";
			int numberOfDrops_01 = 1000;//數量
			float spreadRadius_01 = 20f;//半徑
			// 5. 確定生成中心瓦片
			if (pTile == null)
			{
				pTile = pSelf.current_tile;
			}
			if (pTile == null)
			{
				return false;
			}
			// 6. 進行多回掉落
			for (int i = 0; i < numberOfDrops_01; i++)
			{
				UnityEngine.Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spreadRadius_01;
				int newTileX = (int)pTile.pos.x + (int)randomOffset.x;
				int newTileY = (int)pTile.pos.y + (int)randomOffset.y;
				if (newTileX >= 0 && newTileX < MapBox.width &&
					newTileY >= 0 && newTileY < MapBox.height)
				{
					WorldTile randomTile = World.world.GetTile(newTileX, newTileY);
					if (randomTile != null)
					{
						World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_02, 15f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_03, 15f, -1f);
					}
				}
			}
			return true;
		}
		public static bool AlteredSurface_11(BaseSimObject pSelf, WorldTile pTile = null)
		{// 11 水晶種 seeds_crystal		biome_crystal
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			WorldTile currentTile = pSelf.current_tile;
			if (currentTile == null)
			{
				return false;
			}
			// 檢查 Biome ID
			BiomeAsset currentBiome = currentTile.getBiome();
			if (currentBiome != null && currentBiome.id == "biome_crystal")
			{
				return false;
			}
			if (currentTile.Type != null && UnusablePlots.Contains(currentTile.Type.id))
			{
				return false;
			}
			// 2. 冷卻狀態檢查
			string cooldownStatus = "cdt_alter01";
			float cooldownDuration = 3600f; // 你可以根據需要調整冷卻時間，單位為秒
			// 如果單位已經有冷卻狀態，則直接返回，不執行能力
			if (pSelf.a.hasStatus(cooldownStatus))
			{
				return false;
			}
			// 3. 施加冷卻狀態
			pSelf.a.addStatusEffect(cooldownStatus, cooldownDuration);
			// 4. 設定掉落物 ID 和生成參數
			string dropId_01 = "seeds_crystal";
			string dropId_02 = "fertilizer_plants";
			string dropId_03 = "fertilizer_trees";
			int numberOfDrops_01 = 1000;//數量
			float spreadRadius_01 = 20f;//半徑
			// 5. 確定生成中心瓦片
			if (pTile == null)
			{
				pTile = pSelf.current_tile;
			}
			if (pTile == null)
			{
				return false;
			}
			// 6. 進行多回掉落
			for (int i = 0; i < numberOfDrops_01; i++)
			{
				UnityEngine.Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spreadRadius_01;
				int newTileX = (int)pTile.pos.x + (int)randomOffset.x;
				int newTileY = (int)pTile.pos.y + (int)randomOffset.y;
				if (newTileX >= 0 && newTileX < MapBox.width &&
					newTileY >= 0 && newTileY < MapBox.height)
				{
					WorldTile randomTile = World.world.GetTile(newTileX, newTileY);
					if (randomTile != null)
					{
						World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_02, 1f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_03, 2f, -1f);
					}
				}
			}
			return true;
		}
		public static bool AlteredSurface_12(BaseSimObject pSelf, WorldTile pTile = null)
		{// 12 沼澤種 seeds_swamp		biome_swamp
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			WorldTile currentTile = pSelf.current_tile;
			if (currentTile == null)
			{
				return false;
			}
			// 檢查 Biome ID
			BiomeAsset currentBiome = currentTile.getBiome();
			if (currentBiome != null && currentBiome.id == "biome_swamp")
			{
				return false;
			}
			if (currentTile.Type != null && UnusablePlots.Contains(currentTile.Type.id))
			{
				return false;
			}
			// 2. 冷卻狀態檢查
			string cooldownStatus = "cdt_alter01";
			float cooldownDuration = 3600f; // 你可以根據需要調整冷卻時間，單位為秒
			// 如果單位已經有冷卻狀態，則直接返回，不執行能力
			if (pSelf.a.hasStatus(cooldownStatus))
			{
				return false;
			}
			// 3. 施加冷卻狀態
			pSelf.a.addStatusEffect(cooldownStatus, cooldownDuration);
			// 4. 設定掉落物 ID 和生成參數
			string dropId_01 = "seeds_swamp";
			string dropId_02 = "fertilizer_plants";
			string dropId_03 = "fertilizer_trees";
			int numberOfDrops_01 = 1000;//數量
			float spreadRadius_01 = 20f;//半徑
			// 5. 確定生成中心瓦片
			if (pTile == null)
			{
				pTile = pSelf.current_tile;
			}
			if (pTile == null)
			{
				return false;
			}
			// 6. 進行多回掉落
			for (int i = 0; i < numberOfDrops_01; i++)
			{
				UnityEngine.Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spreadRadius_01;
				int newTileX = (int)pTile.pos.x + (int)randomOffset.x;
				int newTileY = (int)pTile.pos.y + (int)randomOffset.y;
				if (newTileX >= 0 && newTileX < MapBox.width &&
					newTileY >= 0 && newTileY < MapBox.height)
				{
					WorldTile randomTile = World.world.GetTile(newTileX, newTileY);
					if (randomTile != null)
					{
						World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_02, 1f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_03, 2f, -1f);
					}
				}
			}
			return true;
		}
		public static bool AlteredSurface_13(BaseSimObject pSelf, WorldTile pTile = null)
		{// 13 地獄種 seeds_infernal	biome_infernal
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			WorldTile currentTile = pSelf.current_tile;
			if (currentTile == null)
			{
				return false;
			}
			// 檢查 Biome ID
			BiomeAsset currentBiome = currentTile.getBiome();
			if (currentBiome != null && currentBiome.id == "biome_infernal")
			{
				return false;
			}
			if (currentTile.Type != null && UnusablePlots.Contains(currentTile.Type.id))
			{
				return false;
			}
			// 2. 冷卻狀態檢查
			string cooldownStatus = "cdt_alter01";
			float cooldownDuration = 3600f; // 你可以根據需要調整冷卻時間，單位為秒
			// 如果單位已經有冷卻狀態，則直接返回，不執行能力
			if (pSelf.a.hasStatus(cooldownStatus))
			{
				return false;
			}
			// 3. 施加冷卻狀態
			pSelf.a.addStatusEffect(cooldownStatus, cooldownDuration);
			// 4. 設定掉落物 ID 和生成參數
			string dropId_01 = "seeds_infernal";
			string dropId_02 = "fertilizer_plants";
			string dropId_03 = "fertilizer_trees";
			int numberOfDrops_01 = 1000;//數量
			float spreadRadius_01 = 20f;//半徑
			// 5. 確定生成中心瓦片
			if (pTile == null)
			{
				pTile = pSelf.current_tile;
			}
			if (pTile == null)
			{
				return false;
			}
			// 6. 進行多回掉落
			for (int i = 0; i < numberOfDrops_01; i++)
			{
				UnityEngine.Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spreadRadius_01;
				int newTileX = (int)pTile.pos.x + (int)randomOffset.x;
				int newTileY = (int)pTile.pos.y + (int)randomOffset.y;
				if (newTileX >= 0 && newTileX < MapBox.width &&
					newTileY >= 0 && newTileY < MapBox.height)
				{
					WorldTile randomTile = World.world.GetTile(newTileX, newTileY);
					if (randomTile != null)
					{
						World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_02, 1f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_03, 2f, -1f);
					}
				}
			}
			return true;
		}
		public static bool AlteredSurface_14(BaseSimObject pSelf, WorldTile pTile = null)
		{// 14 樺木種 seeds_birch		biome_birch
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			WorldTile currentTile = pSelf.current_tile;
			if (currentTile == null)
			{
				return false;
			}
			// 檢查 Biome ID
			BiomeAsset currentBiome = currentTile.getBiome();
			if (currentBiome != null && currentBiome.id == "biome_birch")
			{
				return false;
			}
			if (currentTile.Type != null && UnusablePlots.Contains(currentTile.Type.id))
			{
				return false;
			}
			// 2. 冷卻狀態檢查
			string cooldownStatus = "cdt_alter01";
			float cooldownDuration = 3600f; // 你可以根據需要調整冷卻時間，單位為秒
			// 如果單位已經有冷卻狀態，則直接返回，不執行能力
			if (pSelf.a.hasStatus(cooldownStatus))
			{
				return false;
			}
			// 3. 施加冷卻狀態
			pSelf.a.addStatusEffect(cooldownStatus, cooldownDuration);
			// 4. 設定掉落物 ID 和生成參數
			string dropId_01 = "seeds_birch";
			string dropId_02 = "fertilizer_plants";
			string dropId_03 = "fertilizer_trees";
			int numberOfDrops_01 = 1000;//數量
			float spreadRadius_01 = 20f;//半徑
			// 5. 確定生成中心瓦片
			if (pTile == null)
			{
				pTile = pSelf.current_tile;
			}
			if (pTile == null)
			{
				return false;
			}
			// 6. 進行多回掉落
			for (int i = 0; i < numberOfDrops_01; i++)
			{
				UnityEngine.Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spreadRadius_01;
				int newTileX = (int)pTile.pos.x + (int)randomOffset.x;
				int newTileY = (int)pTile.pos.y + (int)randomOffset.y;
				if (newTileX >= 0 && newTileX < MapBox.width &&
					newTileY >= 0 && newTileY < MapBox.height)
				{
					WorldTile randomTile = World.world.GetTile(newTileX, newTileY);
					if (randomTile != null)
					{
						World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_02, 1f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_03, 2f, -1f);
					}
				}
			}
			return true;
		}
		public static bool AlteredSurface_15(BaseSimObject pSelf, WorldTile pTile = null)
		{// 15 楓樹種 seeds_maple		biome_maple
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			WorldTile currentTile = pSelf.current_tile;
			if (currentTile == null)
			{
				return false;
			}
			// 檢查 Biome ID
			BiomeAsset currentBiome = currentTile.getBiome();
			if (currentBiome != null && currentBiome.id == "biome_maple")
			{
				return false;
			}
			if (currentTile.Type != null && UnusablePlots.Contains(currentTile.Type.id))
			{
				return false;
			}
			// 2. 冷卻狀態檢查
			string cooldownStatus = "cdt_alter01";
			float cooldownDuration = 3600f; // 你可以根據需要調整冷卻時間，單位為秒
			// 如果單位已經有冷卻狀態，則直接返回，不執行能力
			if (pSelf.a.hasStatus(cooldownStatus))
			{
				return false;
			}
			// 3. 施加冷卻狀態
			pSelf.a.addStatusEffect(cooldownStatus, cooldownDuration);
			// 4. 設定掉落物 ID 和生成參數
			string dropId_01 = "seeds_maple";
			string dropId_02 = "fertilizer_plants";
			string dropId_03 = "fertilizer_trees";
			int numberOfDrops_01 = 1000;//數量
			float spreadRadius_01 = 20f;//半徑
			// 5. 確定生成中心瓦片
			if (pTile == null)
			{
				pTile = pSelf.current_tile;
			}
			if (pTile == null)
			{
				return false;
			}
			// 6. 進行多回掉落
			for (int i = 0; i < numberOfDrops_01; i++)
			{
				UnityEngine.Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spreadRadius_01;
				int newTileX = (int)pTile.pos.x + (int)randomOffset.x;
				int newTileY = (int)pTile.pos.y + (int)randomOffset.y;
				if (newTileX >= 0 && newTileX < MapBox.width &&
					newTileY >= 0 && newTileY < MapBox.height)
				{
					WorldTile randomTile = World.world.GetTile(newTileX, newTileY);
					if (randomTile != null)
					{
						World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_02, 1f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_03, 2f, -1f);
					}
				}
			}
			return true;
		}
		public static bool AlteredSurface_16(BaseSimObject pSelf, WorldTile pTile = null)
		{// 16 巨石種 seeds_rocklands	biome_rocklands
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			WorldTile currentTile = pSelf.current_tile;
			if (currentTile == null)
			{
				return false;
			}
			// 檢查 Biome ID
			BiomeAsset currentBiome = currentTile.getBiome();
			if (currentBiome != null && currentBiome.id == "biome_rocklands")
			{
				return false;
			}
			if (currentTile.Type != null && UnusablePlots.Contains(currentTile.Type.id))
			{
				return false;
			}
			// 2. 冷卻狀態檢查
			string cooldownStatus = "cdt_alter01";
			float cooldownDuration = 3600f; // 你可以根據需要調整冷卻時間，單位為秒
			// 如果單位已經有冷卻狀態，則直接返回，不執行能力
			if (pSelf.a.hasStatus(cooldownStatus))
			{
				return false;
			}
			// 3. 施加冷卻狀態
			pSelf.a.addStatusEffect(cooldownStatus, cooldownDuration);
			// 4. 設定掉落物 ID 和生成參數
			string dropId_01 = "seeds_rocklands";
			string dropId_02 = "fertilizer_plants";
			string dropId_03 = "fertilizer_trees";
			int numberOfDrops_01 = 1000;//數量
			float spreadRadius_01 = 20f;//半徑
			// 5. 確定生成中心瓦片
			if (pTile == null)
			{
				pTile = pSelf.current_tile;
			}
			if (pTile == null)
			{
				return false;
			}
			// 6. 進行多回掉落
			for (int i = 0; i < numberOfDrops_01; i++)
			{
				UnityEngine.Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spreadRadius_01;
				int newTileX = (int)pTile.pos.x + (int)randomOffset.x;
				int newTileY = (int)pTile.pos.y + (int)randomOffset.y;
				if (newTileX >= 0 && newTileX < MapBox.width &&
					newTileY >= 0 && newTileY < MapBox.height)
				{
					WorldTile randomTile = World.world.GetTile(newTileX, newTileY);
					if (randomTile != null)
					{
						World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_02, 1f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_03, 2f, -1f);
					}
				}
			}
			return true;
		}
		public static bool AlteredSurface_17(BaseSimObject pSelf, WorldTile pTile = null)
		{// 17 大蒜種 seeds_garlic		biome_garlic
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			WorldTile currentTile = pSelf.current_tile;
			if (currentTile == null)
			{
				return false;
			}
			// 檢查 Biome ID
			BiomeAsset currentBiome = currentTile.getBiome();
			if (currentBiome != null && currentBiome.id == "biome_garlic")
			{
				return false;
			}
			if (currentTile.Type != null && UnusablePlots.Contains(currentTile.Type.id))
			{
				return false;
			}
			// 2. 冷卻狀態檢查
			string cooldownStatus = "cdt_alter01";
			float cooldownDuration = 3600f; // 你可以根據需要調整冷卻時間，單位為秒
			// 如果單位已經有冷卻狀態，則直接返回，不執行能力
			if (pSelf.a.hasStatus(cooldownStatus))
			{
				return false;
			}
			// 3. 施加冷卻狀態
			pSelf.a.addStatusEffect(cooldownStatus, cooldownDuration);
			// 4. 設定掉落物 ID 和生成參數
			string dropId_01 = "seeds_garlic";
			string dropId_02 = "fertilizer_plants";
			string dropId_03 = "fertilizer_trees";
			int numberOfDrops_01 = 1000;//數量
			float spreadRadius_01 = 20f;//半徑
			// 5. 確定生成中心瓦片
			if (pTile == null)
			{
				pTile = pSelf.current_tile;
			}
			if (pTile == null)
			{
				return false;
			}
			// 6. 進行多回掉落
			for (int i = 0; i < numberOfDrops_01; i++)
			{
				UnityEngine.Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spreadRadius_01;
				int newTileX = (int)pTile.pos.x + (int)randomOffset.x;
				int newTileY = (int)pTile.pos.y + (int)randomOffset.y;
				if (newTileX >= 0 && newTileX < MapBox.width &&
					newTileY >= 0 && newTileY < MapBox.height)
				{
					WorldTile randomTile = World.world.GetTile(newTileX, newTileY);
					if (randomTile != null)
					{
						World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_02, 1f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_03, 2f, -1f);
					}
				}
			}
			return true;
		}
		public static bool AlteredSurface_18(BaseSimObject pSelf, WorldTile pTile = null)
		{// 18 花卉種 seeds_flower		biome_flower
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			WorldTile currentTile = pSelf.current_tile;
			if (currentTile == null)
			{
				return false;
			}
			// 檢查 Biome ID
			BiomeAsset currentBiome = currentTile.getBiome();
			if (currentBiome != null && currentBiome.id == "biome_flower")
			{
				return false;
			}
			if (currentTile.Type != null && UnusablePlots.Contains(currentTile.Type.id))
			{
				return false;
			}
			// 2. 冷卻狀態檢查
			string cooldownStatus = "cdt_alter01";
			float cooldownDuration = 3600f; // 你可以根據需要調整冷卻時間，單位為秒
			// 如果單位已經有冷卻狀態，則直接返回，不執行能力
			if (pSelf.a.hasStatus(cooldownStatus))
			{
				return false;
			}
			// 3. 施加冷卻狀態
			pSelf.a.addStatusEffect(cooldownStatus, cooldownDuration);
			// 4. 設定掉落物 ID 和生成參數
			string dropId_01 = "seeds_flower";
			string dropId_02 = "fertilizer_plants";
			string dropId_03 = "fertilizer_trees";
			int numberOfDrops_01 = 1000;//數量
			float spreadRadius_01 = 20f;//半徑
			// 5. 確定生成中心瓦片
			if (pTile == null)
			{
				pTile = pSelf.current_tile;
			}
			if (pTile == null)
			{
				return false;
			}
			// 6. 進行多回掉落
			for (int i = 0; i < numberOfDrops_01; i++)
			{
				UnityEngine.Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spreadRadius_01;
				int newTileX = (int)pTile.pos.x + (int)randomOffset.x;
				int newTileY = (int)pTile.pos.y + (int)randomOffset.y;
				if (newTileX >= 0 && newTileX < MapBox.width &&
					newTileY >= 0 && newTileY < MapBox.height)
				{
					WorldTile randomTile = World.world.GetTile(newTileX, newTileY);
					if (randomTile != null)
					{
						World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_02, 1f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_03, 2f, -1f);
					}
				}
			}
			return true;
		}
		public static bool AlteredSurface_19(BaseSimObject pSelf, WorldTile pTile = null)
		{// 19 天界種 seeds_celestial	biome_celestial
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			WorldTile currentTile = pSelf.current_tile;
			if (currentTile == null)
			{
				return false;
			}
			// 檢查 Biome ID
			BiomeAsset currentBiome = currentTile.getBiome();
			if (currentBiome != null && currentBiome.id == "biome_celestial")
			{
				return false;
			}
			if (currentTile.Type != null && UnusablePlots.Contains(currentTile.Type.id))
			{
				return false;
			}
			// 2. 冷卻狀態檢查
			string cooldownStatus = "cdt_alter01";
			float cooldownDuration = 3600f; // 你可以根據需要調整冷卻時間，單位為秒
			// 如果單位已經有冷卻狀態，則直接返回，不執行能力
			if (pSelf.a.hasStatus(cooldownStatus))
			{
				return false;
			}
			// 3. 施加冷卻狀態
			pSelf.a.addStatusEffect(cooldownStatus, cooldownDuration);
			// 4. 設定掉落物 ID 和生成參數
			string dropId_01 = "seeds_celestial";
			string dropId_02 = "fertilizer_plants";
			string dropId_03 = "fertilizer_trees";
			int numberOfDrops_01 = 1000;//數量
			float spreadRadius_01 = 20f;//半徑
			// 5. 確定生成中心瓦片
			if (pTile == null)
			{
				pTile = pSelf.current_tile;
			}
			if (pTile == null)
			{
				return false;
			}
			// 6. 進行多回掉落
			for (int i = 0; i < numberOfDrops_01; i++)
			{
				UnityEngine.Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spreadRadius_01;
				int newTileX = (int)pTile.pos.x + (int)randomOffset.x;
				int newTileY = (int)pTile.pos.y + (int)randomOffset.y;
				if (newTileX >= 0 && newTileX < MapBox.width &&
					newTileY >= 0 && newTileY < MapBox.height)
				{
					WorldTile randomTile = World.world.GetTile(newTileX, newTileY);
					if (randomTile != null)
					{
						World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_02, 1f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_03, 2f, -1f);
					}
				}
			}
			return true;
		}
		public static bool AlteredSurface_20(BaseSimObject pSelf, WorldTile pTile = null)
		{// 20 奇點種 seeds_singularity	biome_singularity
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			WorldTile currentTile = pSelf.current_tile;
			if (currentTile == null)
			{
				return false;
			}
			// 檢查 Biome ID
			BiomeAsset currentBiome = currentTile.getBiome();
			if (currentBiome != null && currentBiome.id == "biome_singularity")
			{
				return false;
			}
			if (currentTile.Type != null && UnusablePlots.Contains(currentTile.Type.id))
			{
				return false;
			}
			// 2. 冷卻狀態檢查
			string cooldownStatus = "cdt_alter01";
			float cooldownDuration = 3600f; // 你可以根據需要調整冷卻時間，單位為秒
			// 如果單位已經有冷卻狀態，則直接返回，不執行能力
			if (pSelf.a.hasStatus(cooldownStatus))
			{
				return false;
			}
			// 3. 施加冷卻狀態
			pSelf.a.addStatusEffect(cooldownStatus, cooldownDuration);
			// 4. 設定掉落物 ID 和生成參數
			string dropId_01 = "seeds_singularity";
			string dropId_02 = "fertilizer_plants";
			string dropId_03 = "fertilizer_trees";
			int numberOfDrops_01 = 1000;//數量
			float spreadRadius_01 = 20f;//半徑
			// 5. 確定生成中心瓦片
			if (pTile == null)
			{
				pTile = pSelf.current_tile;
			}
			if (pTile == null)
			{
				return false;
			}
			// 6. 進行多回掉落
			for (int i = 0; i < numberOfDrops_01; i++)
			{
				UnityEngine.Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spreadRadius_01;
				int newTileX = (int)pTile.pos.x + (int)randomOffset.x;
				int newTileY = (int)pTile.pos.y + (int)randomOffset.y;
				if (newTileX >= 0 && newTileX < MapBox.width &&
					newTileY >= 0 && newTileY < MapBox.height)
				{
					WorldTile randomTile = World.world.GetTile(newTileX, newTileY);
					if (randomTile != null)
					{
						World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_02, 1f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_03, 2f, -1f);
					}
				}
			}
			return true;
		}
		public static bool AlteredSurface_21(BaseSimObject pSelf, WorldTile pTile = null)
		{// 21 三葉種 seeds_clover		biome_clover
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			WorldTile currentTile = pSelf.current_tile;
			if (currentTile == null)
			{
				return false;
			}
			// 檢查 Biome ID
			BiomeAsset currentBiome = currentTile.getBiome();
			if (currentBiome != null && currentBiome.id == "biome_clover")
			{
				return false;
			}
			if (currentTile.Type != null && UnusablePlots.Contains(currentTile.Type.id))
			{
				return false;
			}
			// 2. 冷卻狀態檢查
			string cooldownStatus = "cdt_alter01";
			float cooldownDuration = 3600f; // 你可以根據需要調整冷卻時間，單位為秒
			// 如果單位已經有冷卻狀態，則直接返回，不執行能力
			if (pSelf.a.hasStatus(cooldownStatus))
			{
				return false;
			}
			// 3. 施加冷卻狀態
			pSelf.a.addStatusEffect(cooldownStatus, cooldownDuration);
			// 4. 設定掉落物 ID 和生成參數
			string dropId_01 = "seeds_clover";
			string dropId_02 = "fertilizer_plants";
			string dropId_03 = "fertilizer_trees";
			int numberOfDrops_01 = 1000;//數量
			float spreadRadius_01 = 20f;//半徑
			// 5. 確定生成中心瓦片
			if (pTile == null)
			{
				pTile = pSelf.current_tile;
			}
			if (pTile == null)
			{
				return false;
			}
			// 6. 進行多回掉落
			for (int i = 0; i < numberOfDrops_01; i++)
			{
				UnityEngine.Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spreadRadius_01;
				int newTileX = (int)pTile.pos.x + (int)randomOffset.x;
				int newTileY = (int)pTile.pos.y + (int)randomOffset.y;
				if (newTileX >= 0 && newTileX < MapBox.width &&
					newTileY >= 0 && newTileY < MapBox.height)
				{
					WorldTile randomTile = World.world.GetTile(newTileX, newTileY);
					if (randomTile != null)
					{
						World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_02, 1f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_03, 2f, -1f);
					}
				}
			}
			return true;
		}
		public static bool AlteredSurface_22(BaseSimObject pSelf, WorldTile pTile = null)
		{// 22 悖論種 seeds_paradox		biome_paradox
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			WorldTile currentTile = pSelf.current_tile;
			if (currentTile == null)
			{
				return false;
			}
			// 檢查 Biome ID
			BiomeAsset currentBiome = currentTile.getBiome();
			if (currentBiome != null && currentBiome.id == "biome_paradox")
			{
				return false;
			}
			if (currentTile.Type != null && UnusablePlots.Contains(currentTile.Type.id))
			{
				return false;
			}
			// 2. 冷卻狀態檢查
			string cooldownStatus = "cdt_alter01";
			float cooldownDuration = 3600f; // 你可以根據需要調整冷卻時間，單位為秒
			// 如果單位已經有冷卻狀態，則直接返回，不執行能力
			if (pSelf.a.hasStatus(cooldownStatus))
			{
				return false;
			}
			// 3. 施加冷卻狀態
			pSelf.a.addStatusEffect(cooldownStatus, cooldownDuration);
			// 4. 設定掉落物 ID 和生成參數
			string dropId_01 = "seeds_paradox";
			string dropId_02 = "fertilizer_plants";
			string dropId_03 = "fertilizer_trees";
			int numberOfDrops_01 = 1000;//數量
			float spreadRadius_01 = 20f;//半徑
			// 5. 確定生成中心瓦片
			if (pTile == null)
			{
				pTile = pSelf.current_tile;
			}
			if (pTile == null)
			{
				return false;
			}
			// 6. 進行多回掉落
			for (int i = 0; i < numberOfDrops_01; i++)
			{
				UnityEngine.Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spreadRadius_01;
				int newTileX = (int)pTile.pos.x + (int)randomOffset.x;
				int newTileY = (int)pTile.pos.y + (int)randomOffset.y;
				if (newTileX >= 0 && newTileX < MapBox.width &&
					newTileY >= 0 && newTileY < MapBox.height)
				{
					WorldTile randomTile = World.world.GetTile(newTileX, newTileY);
					if (randomTile != null)
					{
						World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_02, 1f, -1f);
						World.world.drop_manager.spawn(randomTile, dropId_03, 2f, -1f);
					}
				}
			}
			return true;
		}
	// 自然其他
		public static bool Planting(BaseSimObject pSelf, WorldTile pTile = null)
		{// 果樹 fruit_bush
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			WorldTile currentTile = pSelf.current_tile;
			if (currentTile == null)
			{
				return false;
			}
			// 檢查 Biome ID
			BiomeAsset currentBiome = currentTile.getBiome();
			if (currentBiome != null && UnplantableBiomes.Contains(currentBiome.id))
			{
				return false;
			}
			if (currentTile.Type != null && UnusablePlots.Contains(currentTile.Type.id))
			{
				return false;
			}
			// 2. 冷卻狀態檢查
			string cooldownStatus = "cdt_alter02";
			float cooldownDuration = 3600f; // 你可以根據需要調整冷卻時間，單位為秒
			// 如果單位已經有冷卻狀態，則直接返回，不執行能力
			if (pSelf.a.hasStatus(cooldownStatus))
			{
				return false;
			}
			// 3. 施加冷卻狀態
			pSelf.a.addStatusEffect(cooldownStatus, cooldownDuration);
			// 4. 設定掉落物 ID 和生成參數
			string dropId_01 = "fruit_bush";
			int numberOfDrops_01 = 7;//數量
			float spreadRadius_01 = 20f;//半徑
			// 5. 確定生成中心瓦片
			if (pTile == null)
			{
				pTile = pSelf.current_tile;
			}
			if (pTile == null)
			{
				return false;
			}
			// 6. 進行多回掉落
			for (int i = 0; i < numberOfDrops_01; i++)
			{
				UnityEngine.Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spreadRadius_01;
				int newTileX = (int)pTile.pos.x + (int)randomOffset.x;
				int newTileY = (int)pTile.pos.y + (int)randomOffset.y;
				if (newTileX >= 0 && newTileX < MapBox.width &&
					newTileY >= 0 && newTileY < MapBox.height)
				{
					WorldTile randomTile = World.world.GetTile(newTileX, newTileY);
					if (randomTile != null)
					{
						World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
					}
				}
			}
			return true;
		}
		public static bool DropOre(BaseSimObject pSelf, WorldTile pTile = null)
		{// 礦物 Ore
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			// 2. 冷卻狀態檢查
			string cooldownStatus = "cdt_alter03";
			float cooldownDuration = 3600f; // 你可以根據需要調整冷卻時間，單位為秒
			// 如果單位已經有冷卻狀態，則直接返回，不執行能力
			if (pSelf.a.hasStatus(cooldownStatus))
			{
				return false;
			}
			// 3. 施加冷卻狀態
			pSelf.a.addStatusEffect(cooldownStatus, cooldownDuration);
			// 4. 設定掉落物清單和生成參數
			string[] oreDrops = { "stone", "metals", "gold", "silver", "mythril", "adamantine" }; // 所有可能的掉落物
			int numberOfDrops = 7; // 數量
			float spreadRadius = 20f; // 半徑
			// 5. 確定生成中心瓦片
			if (pTile == null)
			{
				pTile = pSelf.current_tile;
			}
			if (pTile == null)
			{
				return false;
			}
			// 6. 進行多回掉落
			for (int i = 0; i < numberOfDrops; i++)
			{
				// 隨機選擇一個礦物 ID
				string randomOreId = oreDrops[UnityEngine.Random.Range(0, oreDrops.Length)];
				UnityEngine.Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spreadRadius;
				int newTileX = (int)pTile.pos.x + (int)randomOffset.x;
				int newTileY = (int)pTile.pos.y + (int)randomOffset.y;
				if (newTileX >= 0 && newTileX < MapBox.width &&
					newTileY >= 0 && newTileY < MapBox.height)
				{
					WorldTile randomTile = World.world.GetTile(newTileX, newTileY);
					if (randomTile != null)
					{
						// 在隨機瓦片上生成隨機選擇的礦物
						World.world.drop_manager.spawn(randomTile, randomOreId, 0f, -1f);
					}
				}
			}
			return true;
		}
			#endregion
			#region 築巢建築類
	//母巢建築
		public static bool Nest_000(BaseSimObject pSelf, WorldTile pTile = null)
		{// 蜂巢 beehive		物種亞種限制	bee
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			WorldTile currentTile = pSelf.current_tile;
			if (currentTile == null)
			{
				return false;
			}
			// 2. 冷卻狀態檢查
			string cooldownStatus = "cdt_nest01";
			float cooldownDuration = 1800f; // 你可以根據需要調整冷卻時間，單位為秒
			// 如果單位已經有冷卻狀態，則直接返回，不執行能力
			if (pSelf.a.hasStatus(cooldownStatus) || pSelf.a.hasStatus("cdt_debuff04"))
			{
				return false;
			}
			Actor selfActor = pSelf as Actor;
			if (selfActor == null || !selfActor.isAlive())
			{
				return false;
			}
			// 3. 檢查單位 ID
			if (selfActor.asset.id != "bee" || selfActor.subspecies.hasTrait("prefrontal_cortex"))
			{
				return false;
			}
			// === 檢查單位是否擁有 TargetTrait 清單中的任一特質 ===
			foreach (string traitId in TargetTrait)
			{
				if (selfActor.hasTrait(traitId))
				{
					// 如果單位擁有任何一個這些特質，就終止能力
					return false;
				}
			}
			if (selfActor.hasTrait("slave"))
			{
				return false;
			}
			
			// === 新增：檢查周圍是否有不適合的地塊 ===
			float unusablePlotCheckRadius = 3f; // 你可以調整這個半徑
			if (check_for_unusable_plots_in_radius(selfActor, unusablePlotCheckRadius))
			{
				// 如果在半徑內找到了不適合的地塊，則不發動能力
				return false;
			}

			// 檢查附近是否已存在母巢建築
			float buildingCheckRadius = 50f; // 檢查半徑
			if (check_for_buildings(selfActor, buildingCheckRadius, "beehive"))
			{// 如果在附近找到了 指定建築建築，則不築巢
				return false;
			}
			
			// 4. 施加冷卻狀態
			pSelf.a.addStatusEffect(cooldownStatus, cooldownDuration);
			
			// 5. 設定掉落物 ID 和生成參數
			string dropId_01 = "beehive";
			int numberOfDrops_01 = 1;//數量
			float spreadRadius_01 = 2f;//半徑
			
			// 6. 確定生成中心瓦片
			if (pTile == null)
			{
				pTile = pSelf.current_tile;
			}
			if (pTile == null)
			{
				return false;
			}
			// 7. 進行多回掉落
			for (int i = 0; i < numberOfDrops_01; i++)
			{
				UnityEngine.Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spreadRadius_01;
				int newTileX = (int)pTile.pos.x + (int)randomOffset.x;
				int newTileY = (int)pTile.pos.y + (int)randomOffset.y;
				if (newTileX >= 0 && newTileX < MapBox.width &&
					newTileY >= 0 && newTileY < MapBox.height)
				{
					WorldTile randomTile = World.world.GetTile(newTileX, newTileY);
					if (randomTile != null)
					{
						World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
					}
				}
			}
			return true;
		}
		public static bool Nest_000_2(BaseSimObject pSelf, WorldTile pTile = null)
		{// 蜂巢 beehive		物種亞種限制	no bee
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			WorldTile currentTile = pSelf.current_tile;
			if (currentTile == null)
			{
				return false;
			}
			// 2. 冷卻狀態檢查
			string cooldownStatus = "cdt_nest01";
			float cooldownDuration = 1800f; // 你可以根據需要調整冷卻時間，單位為秒
			// 如果單位已經有冷卻狀態，則直接返回，不執行能力
			if (pSelf.a.hasStatus(cooldownStatus) || pSelf.a.hasStatus("cdt_debuff04"))
			{
				return false;
			}
			Actor selfActor = pSelf as Actor;
			if (selfActor == null || !selfActor.isAlive())
			{
				return false;
			}
			// 3. 檢查單位 ID
			if (selfActor.asset.id == "bee")
			{
				return false;
			}
			// === 檢查單位是否擁有 TargetTrait 清單中的任一特質 ===
			foreach (string traitId in TargetTrait)
			{
				if (selfActor.hasTrait(traitId))
				{
					// 如果單位擁有任何一個這些特質，就終止能力
					return false;
				}
			}
			if (selfActor.hasTrait("slave"))
			{
				return false;
			}
			
			// === 新增：檢查周圍是否有不適合的地塊 ===
			float unusablePlotCheckRadius = 3f; // 你可以調整這個半徑
			if (check_for_unusable_plots_in_radius(selfActor, unusablePlotCheckRadius))
			{
				// 如果在半徑內找到了不適合的地塊，則不發動能力
				return false;
			}

			// 檢查附近是否已存在母巢建築
			float buildingCheckRadius = 50f; // 檢查半徑
			if (check_for_buildings(selfActor, buildingCheckRadius, "beehive"))
			{// 如果在附近找到了 指定建築建築，則不築巢
				return false;
			}
			
			// 4. 施加冷卻狀態
			pSelf.a.addStatusEffect(cooldownStatus, cooldownDuration);
			
			// 5. 設定掉落物 ID 和生成參數
			string dropId_01 = "beehive";
			int numberOfDrops_01 = 1;//數量
			float spreadRadius_01 = 2f;//半徑
			
			// 6. 確定生成中心瓦片
			if (pTile == null)
			{
				pTile = pSelf.current_tile;
			}
			if (pTile == null)
			{
				return false;
			}
			// 7. 進行多回掉落
			for (int i = 0; i < numberOfDrops_01; i++)
			{
				UnityEngine.Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spreadRadius_01;
				int newTileX = (int)pTile.pos.x + (int)randomOffset.x;
				int newTileY = (int)pTile.pos.y + (int)randomOffset.y;
				if (newTileX >= 0 && newTileX < MapBox.width &&
					newTileY >= 0 && newTileY < MapBox.height)
				{
					WorldTile randomTile = World.world.GetTile(newTileX, newTileY);
					if (randomTile != null)
					{
						World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
					}
				}
			}
			return true;
		}
		public static bool Nest_001(BaseSimObject pSelf, WorldTile pTile = null)
		{// 腫瘤 tumor			物種限制		tumor_monster_animal	tumor_monster_unit
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			WorldTile currentTile = pSelf.current_tile;
			if (currentTile == null)
			{
				return false;
			}
			// 2. 冷卻狀態檢查
			string cooldownStatus = "cdt_nest01";
			float cooldownDuration = 1800f; // 你可以根據需要調整冷卻時間，單位為秒
			// 如果單位已經有冷卻狀態，則直接返回，不執行能力
			if (pSelf.a.hasStatus(cooldownStatus) || pSelf.a.hasStatus("cdt_debuff04"))
			{
				return false;
			}
			Actor selfActor = pSelf as Actor;
			if (selfActor == null || !selfActor.isAlive())
			{
				return false;
			}
			// 3. 檢查單位 ID
			if (selfActor.asset.id != "tumor_monster_animal" || selfActor.asset.id != "tumor_monster_unit" || selfActor.city != null)
			{
				return false;
			}
			// === 檢查單位是否擁有 TargetTrait 清單中的任一特質 ===
			foreach (string traitId in TargetTrait)
			{
				if (selfActor.hasTrait(traitId))
				{
					// 如果單位擁有任何一個這些特質，就終止能力
					return false;
				}
			}
			if (selfActor.hasTrait("slave"))
			{
				return false;
			}
			
			// === 新增：檢查周圍是否有不適合的地塊 ===
			float unusablePlotCheckRadius = 3f; // 你可以調整這個半徑
			if (check_for_unusable_plots_in_radius(selfActor, unusablePlotCheckRadius))
			{
				// 如果在半徑內找到了不適合的地塊，則不發動能力
				return false;
			}

			// 檢查附近是否已存在母巢建築
			float buildingCheckRadius = 30f; // 檢查半徑
			if (check_for_buildings(selfActor, buildingCheckRadius, "tumor"))
			{// 如果在附近找到了 指定建築建築，則不築巢
				return false;
			}
			
			// 4. 施加冷卻狀態
			pSelf.a.addStatusEffect(cooldownStatus, cooldownDuration);
			
			// 5. 設定掉落物 ID 和生成參數
			string dropId_01 = "tumor";
			int numberOfDrops_01 = 1;//數量
			float spreadRadius_01 = 2f;//半徑
			
			// 6. 確定生成中心瓦片
			if (pTile == null)
			{
				pTile = pSelf.current_tile;
			}
			if (pTile == null)
			{
				return false;
			}
			// 7. 進行多回掉落
			for (int i = 0; i < numberOfDrops_01; i++)
			{
				UnityEngine.Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spreadRadius_01;
				int newTileX = (int)pTile.pos.x + (int)randomOffset.x;
				int newTileY = (int)pTile.pos.y + (int)randomOffset.y;
				if (newTileX >= 0 && newTileX < MapBox.width &&
					newTileY >= 0 && newTileY < MapBox.height)
				{
					WorldTile randomTile = World.world.GetTile(newTileX, newTileY);
					if (randomTile != null)
					{
						World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
					}
				}
			}
			return true;
		}
		public static bool Nest_002(BaseSimObject pSelf, WorldTile pTile = null)
		{// 核心 cybercore		物種限制		assimilator
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			WorldTile currentTile = pSelf.current_tile;
			if (currentTile == null)
			{
				return false;
			}
			// 2. 冷卻狀態檢查
			string cooldownStatus = "cdt_nest01";
			float cooldownDuration = 1800f; // 你可以根據需要調整冷卻時間，單位為秒
			// 如果單位已經有冷卻狀態，則直接返回，不執行能力
			if (pSelf.a.hasStatus(cooldownStatus) || pSelf.a.hasStatus("cdt_debuff04"))
			{
				return false;
			}
			Actor selfActor = pSelf as Actor;
			if (selfActor == null || !selfActor.isAlive())
			{
				return false;
			}
			// 3. 檢查單位 ID
			if (selfActor.asset.id != "assimilator" || selfActor.city != null)
			{
				return false;
			}
			// === 檢查單位是否擁有 TargetTrait 清單中的任一特質 ===
			foreach (string traitId in TargetTrait)
			{
				if (selfActor.hasTrait(traitId))
				{
					// 如果單位擁有任何一個這些特質，就終止能力
					return false;
				}
			}
			if (selfActor.hasTrait("slave"))
			{
				return false;
			}
			
			// === 新增：檢查周圍是否有不適合的地塊 ===
			float unusablePlotCheckRadius = 3f; // 你可以調整這個半徑
			if (check_for_unusable_plots_in_radius(selfActor, unusablePlotCheckRadius))
			{
				// 如果在半徑內找到了不適合的地塊，則不發動能力
				return false;
			}

			// 檢查附近是否已存在母巢建築
			float buildingCheckRadius = 45f; // 檢查半徑
			if (check_for_buildings(selfActor, buildingCheckRadius, "cybercore"))
			{// 如果在附近找到了 指定建築建築，則不築巢
				return false;
			}
			
			// 4. 施加冷卻狀態
			pSelf.a.addStatusEffect(cooldownStatus, cooldownDuration);
			
			// 5. 設定掉落物 ID 和生成參數
			string dropId_01 = "cybercore";
			int numberOfDrops_01 = 1;//數量
			float spreadRadius_01 = 2f;//半徑
			
			// 6. 確定生成中心瓦片
			if (pTile == null)
			{
				pTile = pSelf.current_tile;
			}
			if (pTile == null)
			{
				return false;
			}
			// 7. 進行多回掉落
			for (int i = 0; i < numberOfDrops_01; i++)
			{
				UnityEngine.Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spreadRadius_01;
				int newTileX = (int)pTile.pos.x + (int)randomOffset.x;
				int newTileY = (int)pTile.pos.y + (int)randomOffset.y;
				if (newTileX >= 0 && newTileX < MapBox.width &&
					newTileY >= 0 && newTileY < MapBox.height)
				{
					WorldTile randomTile = World.world.GetTile(newTileX, newTileY);
					if (randomTile != null)
					{
						World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
					}
				}
			}
			return true;
		}
		public static bool Nest_003(BaseSimObject pSelf, WorldTile pTile = null)
		{// 南瓜 super_pumpkin	物種限制		lil_pumpkin
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			WorldTile currentTile = pSelf.current_tile;
			if (currentTile == null)
			{
				return false;
			}
			// 2. 冷卻狀態檢查
			string cooldownStatus = "cdt_nest01";
			float cooldownDuration = 1800f; // 你可以根據需要調整冷卻時間，單位為秒
			// 如果單位已經有冷卻狀態，則直接返回，不執行能力
			if (pSelf.a.hasStatus(cooldownStatus) || pSelf.a.hasStatus("cdt_debuff04"))
			{
				return false;
			}
			Actor selfActor = pSelf as Actor;
			if (selfActor == null || !selfActor.isAlive())
			{
				return false;
			}
			// 3. 檢查單位 ID
			if (selfActor.asset.id != "lil_pumpkin" || selfActor.city != null)
			{
				return false;
			}
			// === 檢查單位是否擁有 TargetTrait 清單中的任一特質 ===
			foreach (string traitId in TargetTrait)
			{
				if (selfActor.hasTrait(traitId))
				{
					// 如果單位擁有任何一個這些特質，就終止能力
					return false;
				}
			}
			if (selfActor.hasTrait("slave"))
			{
				return false;
			}
			
			// === 新增：檢查周圍是否有不適合的地塊 ===
			float unusablePlotCheckRadius = 3f; // 你可以調整這個半徑
			if (check_for_unusable_plots_in_radius(selfActor, unusablePlotCheckRadius))
			{
				// 如果在半徑內找到了不適合的地塊，則不發動能力
				return false;
			}

			// 檢查附近是否已存在母巢建築
			float buildingCheckRadius = 30f; // 檢查半徑
			if (check_for_buildings(selfActor, buildingCheckRadius, "super_pumpkin"))
			{// 如果在附近找到了 指定建築建築，則不築巢
				return false;
			}
			
			// 4. 施加冷卻狀態
			pSelf.a.addStatusEffect(cooldownStatus, cooldownDuration);
			
			// 5. 設定掉落物 ID 和生成參數
			string dropId_01 = "super_pumpkin";
			int numberOfDrops_01 = 1;//數量
			float spreadRadius_01 = 2f;//半徑
			
			// 6. 確定生成中心瓦片
			if (pTile == null)
			{
				pTile = pSelf.current_tile;
			}
			if (pTile == null)
			{
				return false;
			}
			// 7. 進行多回掉落
			for (int i = 0; i < numberOfDrops_01; i++)
			{
				UnityEngine.Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spreadRadius_01;
				int newTileX = (int)pTile.pos.x + (int)randomOffset.x;
				int newTileY = (int)pTile.pos.y + (int)randomOffset.y;
				if (newTileX >= 0 && newTileX < MapBox.width &&
					newTileY >= 0 && newTileY < MapBox.height)
				{
					WorldTile randomTile = World.world.GetTile(newTileX, newTileY);
					if (randomTile != null)
					{
						World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
					}
				}
			}
			return true;
		}
		public static bool Nest_004(BaseSimObject pSelf, WorldTile pTile = null)
		{// 生物質 biomass		物種限制		bioblob
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			WorldTile currentTile = pSelf.current_tile;
			if (currentTile == null)
			{
				return false;
			}
			// 2. 冷卻狀態檢查
			string cooldownStatus = "cdt_nest01";
			float cooldownDuration = 1800f; // 你可以根據需要調整冷卻時間，單位為秒
			// 如果單位已經有冷卻狀態，則直接返回，不執行能力
			if (pSelf.a.hasStatus(cooldownStatus) || pSelf.a.hasStatus("cdt_debuff04"))
			{
				return false;
			}
			Actor selfActor = pSelf as Actor;
			if (selfActor == null || !selfActor.isAlive())
			{
				return false;
			}
			// 3. 檢查單位 ID
			if (selfActor.asset.id != "bioblob" || selfActor.city != null)
			{
				return false;
			}
			// === 檢查單位是否擁有 TargetTrait 清單中的任一特質 ===
			foreach (string traitId in TargetTrait)
			{
				if (selfActor.hasTrait(traitId))
				{
					// 如果單位擁有任何一個這些特質，就終止能力
					return false;
				}
			}
			if (selfActor.hasTrait("slave"))
			{
				return false;
			}
			
			// === 新增：檢查周圍是否有不適合的地塊 ===
			float unusablePlotCheckRadius = 3f; // 你可以調整這個半徑
			if (check_for_unusable_plots_in_radius(selfActor, unusablePlotCheckRadius))
			{
				// 如果在半徑內找到了不適合的地塊，則不發動能力
				return false;
			}

			// 檢查附近是否已存在母巢建築
			float buildingCheckRadius = 30f; // 檢查半徑
			if (check_for_buildings(selfActor, buildingCheckRadius, "biomass"))
			{// 如果在附近找到了 指定建築建築，則不築巢
				return false;
			}
			
			// 4. 施加冷卻狀態
			pSelf.a.addStatusEffect(cooldownStatus, cooldownDuration);
			
			// 5. 設定掉落物 ID 和生成參數
			string dropId_01 = "biomass";
			int numberOfDrops_01 = 1;//數量
			float spreadRadius_01 = 2f;//半徑
			
			// 6. 確定生成中心瓦片
			if (pTile == null)
			{
				pTile = pSelf.current_tile;
			}
			if (pTile == null)
			{
				return false;
			}
			// 7. 進行多回掉落
			for (int i = 0; i < numberOfDrops_01; i++)
			{
				UnityEngine.Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spreadRadius_01;
				int newTileX = (int)pTile.pos.x + (int)randomOffset.x;
				int newTileY = (int)pTile.pos.y + (int)randomOffset.y;
				if (newTileX >= 0 && newTileX < MapBox.width &&
					newTileY >= 0 && newTileY < MapBox.height)
				{
					WorldTile randomTile = World.world.GetTile(newTileX, newTileY);
					if (randomTile != null)
					{
						World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
					}
				}
			}
			return true;
		}
		public static bool Nest_005(BaseSimObject pSelf, WorldTile pTile = null)
		{// 冰塔 ice_tower		物種亞種限制	cold_one
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			WorldTile currentTile = pSelf.current_tile;
			if (currentTile == null)
			{
				return false;
			}
			// 2. 冷卻狀態檢查
			string cooldownStatus = "cdt_nest01";
			float cooldownDuration = 1800f; // 你可以根據需要調整冷卻時間，單位為秒
			// 如果單位已經有冷卻狀態，則直接返回，不執行能力
			if (pSelf.a.hasStatus(cooldownStatus) || pSelf.a.hasStatus("cdt_debuff04"))
			{
				return false;
			}
			Actor selfActor = pSelf as Actor;
			if (selfActor == null || !selfActor.isAlive())
			{
				return false;
			}
			// 3. 檢查單位 ID
			if (selfActor.asset.id != "cold_one" || selfActor.subspecies.hasTrait("prefrontal_cortex") || selfActor.city != null)
			{
				return false;
			}
			// === 檢查單位是否擁有 TargetTrait 清單中的任一特質 ===
			foreach (string traitId in TargetTrait)
			{
				if (selfActor.hasTrait(traitId))
				{
					// 如果單位擁有任何一個這些特質，就終止能力
					return false;
				}
			}
			if (selfActor.hasTrait("slave"))
			{
				return false;
			}
			
			// === 新增：檢查周圍是否有不適合的地塊 ===
			float unusablePlotCheckRadius = 3f; // 你可以調整這個半徑
			if (check_for_unusable_plots_in_radius(selfActor, unusablePlotCheckRadius))
			{
				// 如果在半徑內找到了不適合的地塊，則不發動能力
				return false;
			}

			// 檢查附近是否已存在母巢建築
			float buildingCheckRadius = 50f; // 檢查半徑
			if (check_for_buildings(selfActor, buildingCheckRadius, "ice_tower"))
			{// 如果在附近找到了 指定建築建築，則不築巢
				return false;
			}
			
			// 4. 施加冷卻狀態
			pSelf.a.addStatusEffect(cooldownStatus, cooldownDuration);
			
			// 5. 設定掉落物 ID 和生成參數
			string dropId_01 = "ice_tower";
			int numberOfDrops_01 = 1;//數量
			float spreadRadius_01 = 2f;//半徑
			
			// 6. 確定生成中心瓦片
			if (pTile == null)
			{
				pTile = pSelf.current_tile;
			}
			if (pTile == null)
			{
				return false;
			}
			// 7. 進行多回掉落
			for (int i = 0; i < numberOfDrops_01; i++)
			{
				UnityEngine.Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spreadRadius_01;
				int newTileX = (int)pTile.pos.x + (int)randomOffset.x;
				int newTileY = (int)pTile.pos.y + (int)randomOffset.y;
				if (newTileX >= 0 && newTileX < MapBox.width &&
					newTileY >= 0 && newTileY < MapBox.height)
				{
					WorldTile randomTile = World.world.GetTile(newTileX, newTileY);
					if (randomTile != null)
					{
						World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
					}
				}
			}
			return true;
		}
		public static bool Nest_006(BaseSimObject pSelf, WorldTile pTile = null)
		{// 火塔 flame_tower	物種亞種限制	demon
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			WorldTile currentTile = pSelf.current_tile;
			if (currentTile == null)
			{
				return false;
			}
			// 2. 冷卻狀態檢查
			string cooldownStatus = "cdt_nest01";
			float cooldownDuration = 1800f; // 你可以根據需要調整冷卻時間，單位為秒
			// 如果單位已經有冷卻狀態，則直接返回，不執行能力
			if (pSelf.a.hasStatus(cooldownStatus) || pSelf.a.hasStatus("cdt_debuff04"))
			{
				return false;
			}
			Actor selfActor = pSelf as Actor;
			if (selfActor == null || !selfActor.isAlive())
			{
				return false;
			}
			// 3. 檢查單位 ID
			if (selfActor.asset.id != "demon" || selfActor.subspecies.hasTrait("prefrontal_cortex") || selfActor.city != null)
			{
				return false;
			}
			// === 檢查單位是否擁有 TargetTrait 清單中的任一特質 ===
			foreach (string traitId in TargetTrait)
			{
				if (selfActor.hasTrait(traitId))
				{
					// 如果單位擁有任何一個這些特質，就終止能力
					return false;
				}
			}
			if (selfActor.hasTrait("slave"))
			{
				return false;
			}
			
			// === 新增：檢查周圍是否有不適合的地塊 ===
			float unusablePlotCheckRadius = 3f; // 你可以調整這個半徑
			if (check_for_unusable_plots_in_radius(selfActor, unusablePlotCheckRadius))
			{
				// 如果在半徑內找到了不適合的地塊，則不發動能力
				return false;
			}

			// 檢查附近是否已存在母巢建築
			float buildingCheckRadius = 50f; // 檢查半徑
			if (check_for_buildings(selfActor, buildingCheckRadius, "flame_tower"))
			{// 如果在附近找到了 指定建築建築，則不築巢
				return false;
			}
			
			// 4. 施加冷卻狀態
			pSelf.a.addStatusEffect(cooldownStatus, cooldownDuration);
			
			// 5. 設定掉落物 ID 和生成參數
			string dropId_01 = "flame_tower";
			int numberOfDrops_01 = 1;//數量
			float spreadRadius_01 = 2f;//半徑
			
			// 6. 確定生成中心瓦片
			if (pTile == null)
			{
				pTile = pSelf.current_tile;
			}
			if (pTile == null)
			{
				return false;
			}
			// 7. 進行多回掉落
			for (int i = 0; i < numberOfDrops_01; i++)
			{
				UnityEngine.Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spreadRadius_01;
				int newTileX = (int)pTile.pos.x + (int)randomOffset.x;
				int newTileY = (int)pTile.pos.y + (int)randomOffset.y;
				if (newTileX >= 0 && newTileX < MapBox.width &&
					newTileY >= 0 && newTileY < MapBox.height)
				{
					WorldTile randomTile = World.world.GetTile(newTileX, newTileY);
					if (randomTile != null)
					{
						World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
					}
				}
			}
			return true;
		}
		public static bool Nest_007(BaseSimObject pSelf, WorldTile pTile = null)
		{// 天塔 angle_tower	物種亞種限制	angle
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			WorldTile currentTile = pSelf.current_tile;
			if (currentTile == null)
			{
				return false;
			}
			// 2. 冷卻狀態檢查
			string cooldownStatus = "cdt_nest01";
			float cooldownDuration = 1800f; // 你可以根據需要調整冷卻時間，單位為秒
			// 如果單位已經有冷卻狀態，則直接返回，不執行能力
			if (pSelf.a.hasStatus(cooldownStatus) || pSelf.a.hasStatus("cdt_debuff04"))
			{
				return false;
			}
			Actor selfActor = pSelf as Actor;
			if (selfActor == null || !selfActor.isAlive())
			{
				return false;
			}
			// 3. 檢查單位 ID
			if (selfActor.asset.id != "angle" || selfActor.subspecies.hasTrait("prefrontal_cortex") || selfActor.city != null)
			{
				return false;
			}
			// === 檢查單位是否擁有 TargetTrait 清單中的任一特質 ===
			foreach (string traitId in TargetTrait)
			{
				if (selfActor.hasTrait(traitId))
				{
					// 如果單位擁有任何一個這些特質，就終止能力
					return false;
				}
			}
			if (selfActor.hasTrait("slave"))
			{
				return false;
			}
			
			// === 新增：檢查周圍是否有不適合的地塊 ===
			float unusablePlotCheckRadius = 3f; // 你可以調整這個半徑
			if (check_for_unusable_plots_in_radius(selfActor, unusablePlotCheckRadius))
			{
				// 如果在半徑內找到了不適合的地塊，則不發動能力
				return false;
			}

			// 檢查附近是否已存在母巢建築
			float buildingCheckRadius = 60f; // 檢查半徑
			if (check_for_buildings(selfActor, buildingCheckRadius, "angle_tower"))
			{// 如果在附近找到了 指定建築建築，則不築巢
				return false;
			}
			
			// 4. 施加冷卻狀態
			pSelf.a.addStatusEffect(cooldownStatus, cooldownDuration);
			
			// 5. 設定掉落物 ID 和生成參數
			string dropId_01 = "angle_tower";
			int numberOfDrops_01 = 1;//數量
			float spreadRadius_01 = 2f;//半徑
			
			// 6. 確定生成中心瓦片
			if (pTile == null)
			{
				pTile = pSelf.current_tile;
			}
			if (pTile == null)
			{
				return false;
			}
			// 7. 進行多回掉落
			for (int i = 0; i < numberOfDrops_01; i++)
			{
				UnityEngine.Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spreadRadius_01;
				int newTileX = (int)pTile.pos.x + (int)randomOffset.x;
				int newTileY = (int)pTile.pos.y + (int)randomOffset.y;
				if (newTileX >= 0 && newTileX < MapBox.width &&
					newTileY >= 0 && newTileY < MapBox.height)
				{
					WorldTile randomTile = World.world.GetTile(newTileX, newTileY);
					if (randomTile != null)
					{
						World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
					}
				}
			}
			return true;
		}
	//遠距生成(築巢建塔)
		public static bool DropMode_001(BaseSimObject pSelf, WorldTile pTile = null)
		{// 腫瘤 tumor
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;

			string attackCooldownStatus = "cdt_drop";
			float attackCooldownDuration = 1800.0f;
			if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus("cdt_debuff04"))
			{
				return false;
			}
			if (!selfActor.subspecies.hasTrait("prefrontal_cortex"))
			{
				return false;
			}
			// === 新增：檢查單位是否擁有 TargetTrait 清單中的任一特質 ===
			foreach (string traitId in TargetTrait)
			{
				if (selfActor.hasTrait(traitId))
				{
					// 如果單位擁有任何一個這些特質，就終止能力
					return false;
				}
			}
			// 尋找目標
			string dropId_01 = "tumor";
			float maxRange = 150f;
			float minRange = 100f; // === 新增：最小攻擊範圍 ===
			Actor target = null;
			float closestDist = float.MaxValue;
			// 遍歷所有單位，尋找最接近且符合條件的目標
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				if (other.asset.id == "tumor_monster_animal" || other.asset.id == "tumor_monster_unit")
				{
					continue;
				}
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				// === 新增：判斷距離是否在最大範圍和最小範圍之間 ===
				if (dist < maxRange && dist > minRange && dist < closestDist)
				{
					closestDist = dist;
					target = other;
				}
			}
			// 如果沒有找到目標，則返回
			if (target == null)
			{
				return false;
			}
			// === 新增：檢查目標周圍是否有已存在的相同建築物 ===
			float buildingCheckRadius = 50f; // 檢查半徑
			if (check_for_buildings(target, buildingCheckRadius, dropId_01))
			{// 如果在附近找到了 指定建築建築，則返回
				return false;
			}
			// === 檢查結束 ===
			// 成功找到目標，施加冷卻並發動攻擊
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			// 我們需要一個瓦片(tile)作為掉落物的目標，這裡直接使用目標單位的當前瓦片
			WorldTile targetTile = target.current_tile;
			if (targetTile != null)
			{// _00設定 
				float spreadRadius_00 = 25f;//隨機散佈半徑
				int numberOfLightning_00 = 12;//生成回數
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
							World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
						}
					}
				}
			}
			return true;
		}	
		public static bool DropMode_002(BaseSimObject pSelf, WorldTile pTile = null)
		{// 核心 cybercore
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			string attackCooldownStatus = "cdt_drop";
			float attackCooldownDuration = 1800.001f;
			if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus("cdt_debuff04"))
			{
				return false;
			}
			if (!selfActor.subspecies.hasTrait("prefrontal_cortex"))
			{
				return false;
			}
			// === 新增：檢查單位是否擁有 TargetTrait 清單中的任一特質 ===
			foreach (string traitId in TargetTrait)
			{
				if (selfActor.hasTrait(traitId))
				{
					// 如果單位擁有任何一個這些特質，就終止能力
					return false;
				}
			}
			// 尋找目標
			string dropId_01 = "cybercore";
			float maxRange = 150f;
			float minRange = 100f; // === 新增：最小攻擊範圍 ===
			Actor target = null;
			float closestDist = float.MaxValue;
			// 遍歷所有單位，尋找最接近且符合條件的目標
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				if (other.asset.id == "assimilator")
				{
					continue;
				}
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				// === 新增：判斷距離是否在最大範圍和最小範圍之間 ===
				if (dist < maxRange && dist > minRange && dist < closestDist)
				{
					closestDist = dist;
					target = other;
				}
			}
			// 如果沒有找到目標，則返回
			if (target == null)
			{
				return false;
			}
			// === 新增：檢查目標周圍是否有已存在的相同建築物 ===
			float buildingCheckRadius = 50f; // 檢查半徑
			if (check_for_buildings(target, buildingCheckRadius, dropId_01))
			{// 如果在附近找到了 指定建築建築，則返回
				return false;
			}
			// === 檢查結束 ===
			// 成功找到目標，施加冷卻並發動攻擊
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			// 我們需要一個瓦片(tile)作為掉落物的目標，這裡直接使用目標單位的當前瓦片
			WorldTile targetTile = target.current_tile;
			if (targetTile != null)
			{// _00設定 
				float spreadRadius_00 = 25f;//隨機散佈半徑
				int numberOfLightning_00 = 12;//生成回數
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
							World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
						}
					}
				}
			}
			return true;
		}	
		public static bool DropMode_003(BaseSimObject pSelf, WorldTile pTile = null)
		{// 南瓜 super_pumpkin
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			string attackCooldownStatus = "cdt_drop";
			float attackCooldownDuration = 1800.001f;
			if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus("cdt_debuff04"))
			{
				return false;
			}
			if (!selfActor.subspecies.hasTrait("prefrontal_cortex"))
			{
				return false;
			}
			// === 新增：檢查單位是否擁有 TargetTrait 清單中的任一特質 ===
			foreach (string traitId in TargetTrait)
			{
				if (selfActor.hasTrait(traitId))
				{
					// 如果單位擁有任何一個這些特質，就終止能力
					return false;
				}
			}
			// 尋找目標
			string dropId_01 = "super_pumpkin";
			float maxRange = 150f;
			float minRange = 100f; // === 新增：最小攻擊範圍 ===
			Actor target = null;
			float closestDist = float.MaxValue;
			// 遍歷所有單位，尋找最接近且符合條件的目標
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				if (other.asset.id == "lil_pumpkin")
				{
					continue;
				}
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				// === 新增：判斷距離是否在最大範圍和最小範圍之間 ===
				if (dist < maxRange && dist > minRange && dist < closestDist)
				{
					closestDist = dist;
					target = other;
				}
			}
			// 如果沒有找到目標，則返回
			if (target == null)
			{
				return false;
			}
			// === 新增：檢查目標周圍是否有已存在的相同建築物 ===
			float buildingCheckRadius = 50f; // 檢查半徑
			if (check_for_buildings(target, buildingCheckRadius, dropId_01))
			{// 如果在附近找到了 指定建築建築，則返回
				return false;
			}
			// === 檢查結束 ===
			// 成功找到目標，施加冷卻並發動攻擊
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			// 我們需要一個瓦片(tile)作為掉落物的目標，這裡直接使用目標單位的當前瓦片
			WorldTile targetTile = target.current_tile;
			if (targetTile != null)
			{// _00設定 
				float spreadRadius_00 = 25f;//隨機散佈半徑
				int numberOfLightning_00 = 12;//生成回數
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
							World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
						}
					}
				}
			}
			return true;
		}	
		public static bool DropMode_004(BaseSimObject pSelf, WorldTile pTile = null)
		{// 生物質 biomass
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;

			string attackCooldownStatus = "cdt_drop";
			float attackCooldownDuration = 1800.001f;
			if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus("cdt_debuff04"))
			{
				return false;
			}
			if (!selfActor.subspecies.hasTrait("prefrontal_cortex"))
			{
				return false;
			}
			// === 新增：檢查單位是否擁有 TargetTrait 清單中的任一特質 ===
			foreach (string traitId in TargetTrait)
			{
				if (selfActor.hasTrait(traitId))
				{
					// 如果單位擁有任何一個這些特質，就終止能力
					return false;
				}
			}
			// 尋找目標
			string dropId_01 = "biomass";
			float maxRange = 150f;
			float minRange = 100f; // === 新增：最小攻擊範圍 ===
			Actor target = null;
			float closestDist = float.MaxValue;
			
			// 遍歷所有單位，尋找最接近且符合條件的目標
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				if (other.asset.id == "bioblob")
				{
					continue;
				}
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				// === 新增：判斷距離是否在最大範圍和最小範圍之間 ===
				if (dist < maxRange && dist > minRange && dist < closestDist)
				{
					closestDist = dist;
					target = other;
				}
			}
			// 如果沒有找到目標，則返回
			if (target == null)
			{
				return false;
			}
			// === 新增：檢查目標周圍是否有已存在的相同建築物 ===
			float buildingCheckRadius = 50f; // 檢查半徑
			if (check_for_buildings(target, buildingCheckRadius, dropId_01))
			{// 如果在附近找到了 指定建築建築，則返回
				return false;
			}
			// === 檢查結束 ===
			
			// 成功找到目標，施加冷卻並發動攻擊
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			
			// 我們需要一個瓦片(tile)作為掉落物的目標，這裡直接使用目標單位的當前瓦片
			WorldTile targetTile = target.current_tile;
			
			if (targetTile != null)
			{// _00設定 
				float spreadRadius_00 = 25f;//隨機散佈半徑
				int numberOfLightning_00 = 12;//生成回數
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
							World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
						}
					}
				}
			}
			return true;
		}	
		public static bool DropMode_005(BaseSimObject pSelf, WorldTile pTile = null)
		{// 冰塔 ice_tower
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;

			string attackCooldownStatus = "cdt_drop";
			float attackCooldownDuration = 1800.001f;
			if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus("cdt_debuff04"))
			{
				return false;
			}
			if (!selfActor.subspecies.hasTrait("prefrontal_cortex"))
			{
				return false;
			}
			// === 新增：檢查單位是否擁有 TargetTrait 清單中的任一特質 ===
			foreach (string traitId in TargetTrait)
			{
				if (selfActor.hasTrait(traitId))
				{
					// 如果單位擁有任何一個這些特質，就終止能力
					return false;
				}
			}
			// 尋找目標
			string dropId_01 = "ice_tower";
			float maxRange = 150f;
			float minRange = 100f; // === 新增：最小攻擊範圍 ===
			Actor target = null;
			float closestDist = float.MaxValue;
			
			// 遍歷所有單位，尋找最接近且符合條件的目標
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				if (other.asset.id == "cold_one")
				{
					continue;
				}
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				// === 新增：判斷距離是否在最大範圍和最小範圍之間 ===
				if (dist < maxRange && dist > minRange && dist < closestDist)
				{
					closestDist = dist;
					target = other;
				}
			}
			// 如果沒有找到目標，則返回
			if (target == null)
			{
				return false;
			}
			// === 新增：檢查目標周圍是否有已存在的相同建築物 ===
			float buildingCheckRadius = 75f; // 檢查半徑
			if (check_for_buildings(target, buildingCheckRadius, dropId_01))
			{// 如果在附近找到了 指定建築建築，則返回
				return false;
			}
			// === 檢查結束 ===
			
			// 成功找到目標，施加冷卻並發動攻擊
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			
			// 我們需要一個瓦片(tile)作為掉落物的目標，這裡直接使用目標單位的當前瓦片
			WorldTile targetTile = target.current_tile;
			
			if (targetTile != null)
			{// _00設定 
				float spreadRadius_00 = 5f;//隨機散佈半徑
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
							World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
						}
					}
				}
			}
			return true;
		}	
		public static bool DropMode_006(BaseSimObject pSelf, WorldTile pTile = null)
		{// 火塔 flame_tower
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;

			string attackCooldownStatus = "cdt_drop";
			float attackCooldownDuration = 1800.001f;
			if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus("cdt_debuff04"))
			{
				return false;
			}
			if (!selfActor.subspecies.hasTrait("prefrontal_cortex"))
			{
				return false;
			}
			// === 新增：檢查單位是否擁有 TargetTrait 清單中的任一特質 ===
			foreach (string traitId in TargetTrait)
			{
				if (selfActor.hasTrait(traitId))
				{
					// 如果單位擁有任何一個這些特質，就終止能力
					return false;
				}
			}
			// 尋找目標
			string dropId_01 = "flame_tower";
			float maxRange = 150f;
			float minRange = 100f; // === 新增：最小攻擊範圍 ===
			Actor target = null;
			float closestDist = float.MaxValue;
			
			// 遍歷所有單位，尋找最接近且符合條件的目標
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				if (other.asset.id == "demon")
				{
					continue;
				}
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				// === 新增：判斷距離是否在最大範圍和最小範圍之間 ===
				if (dist < maxRange && dist > minRange && dist < closestDist)
				{
					closestDist = dist;
					target = other;
				}
			}
			// 如果沒有找到目標，則返回
			if (target == null)
			{
				return false;
			}
			// === 新增：檢查目標周圍是否有已存在的相同建築物 ===
			float buildingCheckRadius = 75f; // 檢查半徑
			if (check_for_buildings(target, buildingCheckRadius, dropId_01))
			{// 如果在附近找到了 指定建築建築，則返回
				return false;
			}
			// === 檢查結束 ===
			
			// 成功找到目標，施加冷卻並發動攻擊
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			
			// 我們需要一個瓦片(tile)作為掉落物的目標，這裡直接使用目標單位的當前瓦片
			WorldTile targetTile = target.current_tile;
			
			if (targetTile != null)
			{// _00設定  
				float spreadRadius_00 = 5f;//隨機散佈半徑
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
							World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
						}
					}
				}
			}
			return true;
		}	
		public static bool DropMode_007(BaseSimObject pSelf, WorldTile pTile = null)
		{// 天塔 angle_tower
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;

			string attackCooldownStatus = "cdt_drop";
			float attackCooldownDuration = 1800.001f;
			if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus("cdt_debuff04"))
			{
				return false;
			}
			if (!selfActor.subspecies.hasTrait("prefrontal_cortex"))
			{
				return false;
			}
			// === 新增：檢查單位是否擁有 TargetTrait 清單中的任一特質 ===
			foreach (string traitId in TargetTrait)
			{
				if (selfActor.hasTrait(traitId))
				{
					// 如果單位擁有任何一個這些特質，就終止能力
					return false;
				}
			}
			// 尋找目標
			string dropId_01 = "angle_tower";
			float maxRange = 150f;
			float minRange = 100f; // === 新增：最小攻擊範圍 ===
			Actor target = null;
			float closestDist = float.MaxValue;
			
			// 遍歷所有單位，尋找最接近且符合條件的目標
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				if (other.asset.id == "angle")
				{
					continue;
				}
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				// === 新增：判斷距離是否在最大範圍和最小範圍之間 ===
				if (dist < maxRange && dist > minRange && dist < closestDist)
				{
					closestDist = dist;
					target = other;
				}
			}
			// 如果沒有找到目標，則返回
			if (target == null)
			{
				return false;
			}
			// === 新增：檢查目標周圍是否有已存在的相同建築物 ===
			float buildingCheckRadius = 75f; // 檢查半徑
			if (check_for_buildings(target, buildingCheckRadius, dropId_01))
			{// 如果在附近找到了 指定建築建築，則返回
				return false;
			}
			// === 檢查結束 ===
			
			// 成功找到目標，施加冷卻並發動攻擊
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			
			// 我們需要一個瓦片(tile)作為掉落物的目標，這裡直接使用目標單位的當前瓦片
			WorldTile targetTile = target.current_tile;
			
			if (targetTile != null)
			{// _00設定  
				float spreadRadius_00 = 5f;//隨機散佈半徑
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
							World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
						}
					}
				}
			}
			return true;
		}	
		public static bool DropMode_0070(BaseSimObject pSelf, WorldTile pTile = null)
		{// 天塔 angle_tower
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			string attackCooldownStatus = "cdt_drop";
			float attackCooldownDuration = 1800.001f;
			if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus("cdt_debuff04"))
			{
				return false;
			}
			if (!selfActor.subspecies.hasTrait("prefrontal_cortex"))
			{
				return false;
			}
			// === 新增：檢查單位是否擁有 TargetTrait 清單中的任一特質 ===
			foreach (string traitId in TargetTrait)
			{
				if (selfActor.hasTrait(traitId))
				{
					// 如果單位擁有任何一個這些特質，就終止能力
					return false;
				}
			}
			// 尋找目標
			string dropId_01 = "angle_tower";
			float maxRange = 150f;
			float minRange = 50f; // === 新增：最小攻擊範圍 ===
			Actor target = null;
			float closestDist = float.MaxValue;
			// 遍歷所有單位，尋找最接近且符合條件的目標
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				// 檢查目標是否為敵人
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				// === 重新組織邏輯：首先判斷目標是否有效 ===
				// 條件一：目標是 AngelAttackTarget 清單中的物種，或擁有 TargetTrait 清單中的特質
				bool isTargetValid = AngelAttackTarget.Contains(other.asset.id);
				if (!isTargetValid)
				{
					foreach (string traitId in TargetTrait)
					{
						if (other.hasTrait(traitId))
						{
							isTargetValid = true;
							break;
						}
					}
				}
				if (other.city != null && !isTargetValid)
				{
					continue;
				}
				// 條件二：如果目標有效，但它的物種ID是"angle"，則不考慮它
				// 這是為了排除那些沒有「可攻擊特質」的普通天使
				if (other.asset.id == "angle" && !isTargetValid)
				{
					continue;
				}
				// 如果目標不屬於任何一個有效條件，則跳過
				if (!isTargetValid)
				{
					continue;
				}
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				if (dist < maxRange && dist > minRange && dist < closestDist)
				{
					closestDist = dist;
					target = other;
				}
			}
			// 如果沒有找到目標，則返回
			if (target == null)
			{
				return false;
			}
			// 檢查目標周圍是否有已存在的相同建築物
			// 將半徑調整為與生成範圍一致，以確保邏輯準確
			float buildingCheckRadius = 75f;
			if (check_for_buildings(target, buildingCheckRadius, dropId_01))
			{
				return false;
			}
			// 成功找到目標，施加冷卻並發動攻擊
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			WorldTile targetTile = target.current_tile;
			if (targetTile != null)
			{ 
				float spreadRadius_00 = 5f;//隨機散佈半徑
				int numberOfLightning_00 = 3;//生成回數
				for (int i = 0; i < numberOfLightning_00; i++)
				{
					float offsetX = UnityEngine.Random.Range(-spreadRadius_00, spreadRadius_00);
					float offsetY = UnityEngine.Random.Range(-spreadRadius_00, spreadRadius_00);
					int newTileX = (int)targetTile.pos.x + (int)offsetX;
					int newTileY = (int)targetTile.pos.y + (int)offsetY;
					if (newTileX >= 0 && newTileX < MapBox.width && 
						newTileY >= 0 && newTileY < MapBox.height)
					{
						WorldTile randomTile = World.world.GetTile(newTileX, newTileY);
						if (randomTile != null)
						{
							World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
						}
					}
				}
			}
			return true;
		}
		public static bool DropMode_008(BaseSimObject pSelf, WorldTile pTile = null)
		{// 墮落之腦 corrupted_brain
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;

			string attackCooldownStatus = "cdt_drop";
			float attackCooldownDuration = 1800.001f;
			if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus("cdt_debuff04"))
			{
				return false;
			}
			if (!selfActor.subspecies.hasTrait("prefrontal_cortex"))
			{
				return false;
			}
			// === 新增：檢查單位是否擁有 TargetTrait 清單中的任一特質 ===
			foreach (string traitId in TargetTrait)
			{
				if (selfActor.hasTrait(traitId))
				{
					// 如果單位擁有任何一個這些特質，就終止能力
					return false;
				}
			}
			// 尋找目標
			string dropId_01 = "corrupted_brain";
			float maxRange = 150f;
			float minRange = 100f; // === 新增：最小攻擊範圍 ===
			Actor target = null;
			float closestDist = float.MaxValue;
			
			// 遍歷所有單位，尋找最接近且符合條件的目標
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				if (other.hasTrait("madness"))
				{
					continue;
				}
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				// === 新增：判斷距離是否在最大範圍和最小範圍之間 ===
				if (dist < maxRange && dist > minRange && dist < closestDist)
				{
					closestDist = dist;
					target = other;
				}
			}
			// 如果沒有找到目標，則返回
			if (target == null)
			{
				return false;
			}
			// === 新增：檢查目標周圍是否有已存在的相同建築物 ===
			float buildingCheckRadius = 50f; // 檢查半徑
			if (check_for_buildings(target, buildingCheckRadius, dropId_01))
			{// 如果在附近找到了 指定建築建築，則返回
				return false;
			}
			// === 檢查結束 ===
			
			// 成功找到目標，施加冷卻並發動攻擊
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			
			// 我們需要一個瓦片(tile)作為掉落物的目標，這裡直接使用目標單位的當前瓦片
			WorldTile targetTile = target.current_tile;
			
			if (targetTile != null)
			{// _00設定 
				float spreadRadius_00 = 10f;//隨機散佈半徑
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
							World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
						}
					}
				}
			}
			return true;
		}	
		public static bool DropMode_009(BaseSimObject pSelf, WorldTile pTile = null)
		{// 慾望電腦 waypoint_computer
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;

			string attackCooldownStatus = "cdt_drop";
			float attackCooldownDuration = 1800.001f;
			if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus("cdt_debuff04"))
			{
				return false;
			}
			if (!selfActor.subspecies.hasTrait("prefrontal_cortex"))
			{
				return false;
			}
			// === 新增：檢查單位是否擁有 TargetTrait 清單中的任一特質 ===
			foreach (string traitId in TargetTrait)
			{
				if (selfActor.hasTrait(traitId))
				{
					// 如果單位擁有任何一個這些特質，就終止能力
					return false;
				}
			}
			// 尋找目標
			string dropId_01 = "waypoint_computer";
			float maxRange = 150f;
			float minRange = 100f; // === 新增：最小攻擊範圍 ===
			Actor target = null;
			float closestDist = float.MaxValue;
			
			// 遍歷所有單位，尋找最接近且符合條件的目標
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				if (other.hasTrait("desire_computer"))
				{
					continue;
				}
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				// === 新增：判斷距離是否在最大範圍和最小範圍之間 ===
				if (dist < maxRange && dist > minRange && dist < closestDist)
				{
					closestDist = dist;
					target = other;
				}
			}
			// 如果沒有找到目標，則返回
			if (target == null)
			{
				return false;
			}
			// === 新增：檢查目標周圍是否有已存在的相同建築物 ===
			float buildingCheckRadius = 50f; // 檢查半徑
			if (check_for_buildings(target, buildingCheckRadius, dropId_01))
			{// 如果在附近找到了 指定建築建築，則返回
				return false;
			}
			// === 檢查結束 ===
			
			// 成功找到目標，施加冷卻並發動攻擊
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			
			// 我們需要一個瓦片(tile)作為掉落物的目標，這裡直接使用目標單位的當前瓦片
			WorldTile targetTile = target.current_tile;
			
			if (targetTile != null)
			{// _00設定 
				float spreadRadius_00 = 10f;//隨機散佈半徑
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
							World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
						}
					}
				}
			}
			return true;
		}	
		public static bool DropMode_010(BaseSimObject pSelf, WorldTile pTile = null)
		{// 慾望金蛋 waypoint_golden_egg
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;

			string attackCooldownStatus = "cdt_drop";
			float attackCooldownDuration = 1800.001f;
			if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus("cdt_debuff04"))
			{
				return false;
			}
			if (!selfActor.subspecies.hasTrait("prefrontal_cortex"))
			{
				return false;
			}
			// === 新增：檢查單位是否擁有 TargetTrait 清單中的任一特質 ===
			foreach (string traitId in TargetTrait)
			{
				if (selfActor.hasTrait(traitId))
				{
					// 如果單位擁有任何一個這些特質，就終止能力
					return false;
				}
			}
			// 尋找目標
			string dropId_01 = "waypoint_golden_egg";
			float maxRange = 150f;
			float minRange = 100f; // === 新增：最小攻擊範圍 ===
			Actor target = null;
			float closestDist = float.MaxValue;
			
			// 遍歷所有單位，尋找最接近且符合條件的目標
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				if (other.hasTrait("desire_golden_egg"))
				{
					continue;
				}
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				// === 新增：判斷距離是否在最大範圍和最小範圍之間 ===
				if (dist < maxRange && dist > minRange && dist < closestDist)
				{
					closestDist = dist;
					target = other;
				}
			}
			// 如果沒有找到目標，則返回
			if (target == null)
			{
				return false;
			}
			// === 新增：檢查目標周圍是否有已存在的相同建築物 ===
			float buildingCheckRadius = 50f; // 檢查半徑
			if (check_for_buildings(target, buildingCheckRadius, dropId_01))
			{// 如果在附近找到了 指定建築建築，則返回
				return false;
			}
			// === 檢查結束 ===
			
			// 成功找到目標，施加冷卻並發動攻擊
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			
			// 我們需要一個瓦片(tile)作為掉落物的目標，這裡直接使用目標單位的當前瓦片
			WorldTile targetTile = target.current_tile;
			
			if (targetTile != null)
			{// _00設定 
				float spreadRadius_00 = 10f;//隨機散佈半徑
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
							World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
						}
					}
				}
			}
			return true;
		}	
		public static bool DropMode_011(BaseSimObject pSelf, WorldTile pTile = null)
		{// 慾望豎琴 waypoint_harp
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;

			string attackCooldownStatus = "cdt_drop";
			float attackCooldownDuration = 1800.001f;
			if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus("cdt_debuff04"))
			{
				return false;
			}
			if (!selfActor.subspecies.hasTrait("prefrontal_cortex"))
			{
				return false;
			}
			// === 新增：檢查單位是否擁有 TargetTrait 清單中的任一特質 ===
			foreach (string traitId in TargetTrait)
			{
				if (selfActor.hasTrait(traitId))
				{
					// 如果單位擁有任何一個這些特質，就終止能力
					return false;
				}
			}
			// 尋找目標
			string dropId_01 = "waypoint_harp";
			float maxRange = 150f;
			float minRange = 100f; // === 新增：最小攻擊範圍 ===
			Actor target = null;
			float closestDist = float.MaxValue;
			
			// 遍歷所有單位，尋找最接近且符合條件的目標
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				if (other.hasTrait("desire_harp"))
				{
					continue;
				}
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				// === 新增：判斷距離是否在最大範圍和最小範圍之間 ===
				if (dist < maxRange && dist > minRange && dist < closestDist)
				{
					closestDist = dist;
					target = other;
				}
			}
			// 如果沒有找到目標，則返回
			if (target == null)
			{
				return false;
			}
			// === 新增：檢查目標周圍是否有已存在的相同建築物 ===
			float buildingCheckRadius = 50f; // 檢查半徑
			if (check_for_buildings(target, buildingCheckRadius, dropId_01))
			{// 如果在附近找到了 指定建築建築，則返回
				return false;
			}
			// === 檢查結束 ===
			
			// 成功找到目標，施加冷卻並發動攻擊
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			
			// 我們需要一個瓦片(tile)作為掉落物的目標，這裡直接使用目標單位的當前瓦片
			WorldTile targetTile = target.current_tile;
			
			if (targetTile != null)
			{// _00設定 
				float spreadRadius_00 = 10f;//隨機散佈半徑
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
							World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
						}
					}
				}
			}
			return true;
		}	
		public static bool DropMode_012(BaseSimObject pSelf, WorldTile pTile = null)
		{// 異形黴菌 waypoint_alien_mold
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;

			string attackCooldownStatus = "cdt_drop";
			float attackCooldownDuration = 1800.001f;
			if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus("cdt_debuff04"))
			{
				return false;
			}
			if (!selfActor.subspecies.hasTrait("prefrontal_cortex"))
			{
				return false;
			}
			// === 新增：檢查單位是否擁有 TargetTrait 清單中的任一特質 ===
			foreach (string traitId in TargetTrait)
			{
				if (selfActor.hasTrait(traitId))
				{
					// 如果單位擁有任何一個這些特質，就終止能力
					return false;
				}
			}
			// 尋找目標
			string dropId_01 = "waypoint_alien_mold";
			float maxRange = 150f;
			float minRange = 100f; // === 新增：最小攻擊範圍 ===
			Actor target = null;
			float closestDist = float.MaxValue;
			
			// 遍歷所有單位，尋找最接近且符合條件的目標
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				if (other.hasTrait("desire_alien_mold"))
				{
					continue;
				}
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				// === 新增：判斷距離是否在最大範圍和最小範圍之間 ===
				if (dist < maxRange && dist > minRange && dist < closestDist)
				{
					closestDist = dist;
					target = other;
				}
			}
			// 如果沒有找到目標，則返回
			if (target == null)
			{
				return false;
			}
			// === 新增：檢查目標周圍是否有已存在的相同建築物 ===
			float buildingCheckRadius = 50f; // 檢查半徑
			if (check_for_buildings(target, buildingCheckRadius, dropId_01))
			{// 如果在附近找到了 指定建築建築，則返回
				return false;
			}
			// === 檢查結束 ===
			
			// 成功找到目標，施加冷卻並發動攻擊
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			
			// 我們需要一個瓦片(tile)作為掉落物的目標，這裡直接使用目標單位的當前瓦片
			WorldTile targetTile = target.current_tile;
			
			if (targetTile != null)
			{// _00設定 
				float spreadRadius_00 = 10f;//隨機散佈半徑
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
							World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
						}
					}
				}
			}
			return true;
		}	
		public static bool DropMode_013(BaseSimObject pSelf, WorldTile pTile = null)
		{// 酸泉 geyser_acid
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;

			string attackCooldownStatus = "cdt_drop";
			float attackCooldownDuration = 1800.001f;
			if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus("cdt_debuff04"))
			{
				return false;
			}
			if (!selfActor.subspecies.hasTrait("prefrontal_cortex"))
			{
				return false;
			}
			// === 新增：檢查單位是否擁有 TargetTrait 清單中的任一特質 ===
			foreach (string traitId in TargetTrait)
			{
				if (selfActor.hasTrait(traitId))
				{
					// 如果單位擁有任何一個這些特質，就終止能力
					return false;
				}
			}
			// 尋找目標
			string dropId_01 = "geyser_acid";
			float maxRange = 150f;
			float minRange = 100f; // === 新增：最小攻擊範圍 ===
			Actor target = null;
			float closestDist = float.MaxValue;
			
			// 遍歷所有單位，尋找最接近且符合條件的目標
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				//if (other.hasTrait(traitId))
				//{
				//	continue;
				//}
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				// === 新增：判斷距離是否在最大範圍和最小範圍之間 ===
				if (dist < maxRange && dist > minRange && dist < closestDist)
				{
					closestDist = dist;
					target = other;
				}
			}
			// 如果沒有找到目標，則返回
			if (target == null)
			{
				return false;
			}
			// === 新增：檢查目標周圍是否有已存在的相同建築物 ===
			float buildingCheckRadius = 50f; // 檢查半徑
			if (check_for_buildings(target, buildingCheckRadius, dropId_01))
			{// 如果在附近找到了 指定建築建築，則返回
				return false;
			}
			// === 檢查結束 ===
			
			// 成功找到目標，施加冷卻並發動攻擊
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			
			// 我們需要一個瓦片(tile)作為掉落物的目標，這裡直接使用目標單位的當前瓦片
			WorldTile targetTile = target.current_tile;
			
			if (targetTile != null)
			{// _00設定 
				float spreadRadius_00 = 10f;//隨機散佈半徑
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
							World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
						}
					}
				}
			}
			return true;
		}	
		public static bool DropMode_014(BaseSimObject pSelf, WorldTile pTile = null)
		{// 火山 volcano
			// 1. 基本安全檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;

			string attackCooldownStatus = "cdt_drop";
			float attackCooldownDuration = 1800.001f;
			if (selfActor.hasStatus(attackCooldownStatus) || selfActor.hasStatus("cdt_debuff04"))
			{
				return false;
			}
			if (!selfActor.subspecies.hasTrait("prefrontal_cortex"))
			{
				return false;
			}
			// === 新增：檢查單位是否擁有 TargetTrait 清單中的任一特質 ===
			foreach (string traitId in TargetTrait)
			{
				if (selfActor.hasTrait(traitId))
				{
					// 如果單位擁有任何一個這些特質，就終止能力
					return false;
				}
			}
			// 尋找目標
			string dropId_01 = "volcano";
			float maxRange = 150f;
			float minRange = 100f; // === 新增：最小攻擊範圍 ===
			Actor target = null;
			float closestDist = float.MaxValue;
			
			// 遍歷所有單位，尋找最接近且符合條件的目標
			foreach (var other in World.world.units)
			{
				if (other == null || other == selfActor || !other.isAlive())
					continue;
				if (other.kingdom == null || selfActor.kingdom == null || !selfActor.kingdom.isEnemy(other.kingdom))
					continue;
				//if (other.hasTrait(traitId))
				//{
				//	continue;
				//}
				float dist = Vector2.Distance(selfActor.current_position, other.current_position);
				// === 新增：判斷距離是否在最大範圍和最小範圍之間 ===
				if (dist < maxRange && dist > minRange && dist < closestDist)
				{
					closestDist = dist;
					target = other;
				}
			}
			// 如果沒有找到目標，則返回
			if (target == null)
			{
				return false;
			}
			// === 新增：檢查目標周圍是否有已存在的相同建築物 ===
			float buildingCheckRadius = 50f; // 檢查半徑
			if (check_for_buildings(target, buildingCheckRadius, dropId_01))
			{// 如果在附近找到了 指定建築建築，則返回
				return false;
			}
			// === 檢查結束 ===
			
			// 成功找到目標，施加冷卻並發動攻擊
			selfActor.addStatusEffect(attackCooldownStatus, attackCooldownDuration);
			
			// 我們需要一個瓦片(tile)作為掉落物的目標，這裡直接使用目標單位的當前瓦片
			WorldTile targetTile = target.current_tile;
			
			if (targetTile != null)
			{// _00設定 
				float spreadRadius_00 = 10f;//隨機散佈半徑
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
							World.world.drop_manager.spawn(randomTile, dropId_01, 0f, -1f);
						}
					}
				}
			}
			return true;
		}	

		public static bool Return_01(BaseSimObject pTarget, WorldTile pTile = null)
		{// 腫瘤 歸籍 tumor_monster_animal	tumor_monster_unit
			// 1. 基本安全檢查與施法者定義
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pTarget.a; // 施法者
			
			// 2. 參數設定與施法者檢查
			int maxRange = 1;
			// 施法者必須有城市，才能提供歸屬地
			if (selfActor.subspecies == null || selfActor.city == null)
			{
				return false;
			}
			// 3. 獲取搜索地塊
			WorldTile tTile = pTile ?? pTarget.current_tile;
			// 4. 獲取範圍內的單位 (使用 maxRange)
			var allClosestUnits = Finder.getUnitsFromChunk(tTile, maxRange); // <-- 修正範圍
			bool effectApplied = false;
			if (allClosestUnits.Any())
			{
				foreach (var unit in allClosestUnits)
				{
					// 基礎安全檢查
					if (unit == null || !unit.isActor() || !unit.a.isAlive())
						continue;
					Actor targetActor = unit.a; // 正確宣告目標 Actor
					// 5. 條件檢查
					// 條件1: 雙方國家相同
					// 條件2: 目標沒有居住城市
					// 條件3: 目標種族是 冰魔
					if (targetActor.kingdom != selfActor.kingdom &&
						(targetActor.asset.id == "tumor_monster_animal" || targetActor.asset.id == "tumor_monster_unit") &&
						(!targetActor.hasTrait("madness") || !targetActor.hasTrait("desire_harp") || !targetActor.hasTrait("desire_alien_mold") || !targetActor.hasTrait("desire_computer") || !targetActor.hasTrait("desire_golden_egg")) &&
						targetActor.city == null)
					{
						if (targetActor.kingdom != null)
						{
							targetActor.kingdom.units.Remove(targetActor);
						}

						// 步驟 2: 歸屬王國
						targetActor.kingdom = selfActor.kingdom;
						// 【安全檢查】：防止在週期運行時重複添加
						if (!selfActor.kingdom.units.Contains(targetActor))
						{
							selfActor.kingdom.units.Add(targetActor);
						}

						// 步驟 3: 歸屬城市
						targetActor.city = selfActor.city;
						// 【安全檢查】：防止在週期運行時重複添加
						if (!selfActor.city.units.Contains(targetActor))
						{
							selfActor.city.units.Add(targetActor);
						}
						effectApplied = true;
					}
				}
			}
			return effectApplied; // 只有在至少一個單位被歸化時，才返回 true
		}
		public static bool Return_02(BaseSimObject pTarget, WorldTile pTile = null)
		{// 核心 歸籍 assimilator
			// 1. 基本安全檢查與施法者定義
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pTarget.a; // 施法者
			
			// 2. 參數設定與施法者檢查
			int maxRange = 1;
			// 施法者必須有城市，才能提供歸屬地
			if (selfActor.subspecies == null || selfActor.city == null)
			{
				return false;
			}
			// 3. 獲取搜索地塊
			WorldTile tTile = pTile ?? pTarget.current_tile;
			// 4. 獲取範圍內的單位 (使用 maxRange)
			var allClosestUnits = Finder.getUnitsFromChunk(tTile, maxRange); // <-- 修正範圍
			bool effectApplied = false;
			if (allClosestUnits.Any())
			{
				foreach (var unit in allClosestUnits)
				{
					// 基礎安全檢查
					if (unit == null || !unit.isActor() || !unit.a.isAlive())
						continue;
					Actor targetActor = unit.a; // 正確宣告目標 Actor
					// 5. 條件檢查
					// 條件1: 雙方國家相同
					// 條件2: 目標沒有居住城市
					// 條件3: 目標種族是 冰魔
					if (targetActor.kingdom != selfActor.kingdom &&
						targetActor.asset.id == "assimilator" &&
						(!targetActor.hasTrait("madness") || !targetActor.hasTrait("desire_harp") || !targetActor.hasTrait("desire_alien_mold") || !targetActor.hasTrait("desire_computer") || !targetActor.hasTrait("desire_golden_egg")) &&
						targetActor.city == null)
					{
						if (targetActor.kingdom != null)
						{
							targetActor.kingdom.units.Remove(targetActor);
						}

						// 步驟 2: 歸屬王國
						targetActor.kingdom = selfActor.kingdom;
						// 【安全檢查】：防止在週期運行時重複添加
						if (!selfActor.kingdom.units.Contains(targetActor))
						{
							selfActor.kingdom.units.Add(targetActor);
						}

						// 步驟 3: 歸屬城市
						targetActor.city = selfActor.city;
						// 【安全檢查】：防止在週期運行時重複添加
						if (!selfActor.city.units.Contains(targetActor))
						{
							selfActor.city.units.Add(targetActor);
						}
						effectApplied = true;
					}
				}
			}
			return effectApplied; // 只有在至少一個單位被歸化時，才返回 true
		}
		public static bool Return_03(BaseSimObject pTarget, WorldTile pTile = null)
		{// 南瓜 歸籍 lil_pumpkin
			// 1. 基本安全檢查與施法者定義
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pTarget.a; // 施法者
			
			// 2. 參數設定與施法者檢查
			int maxRange = 1;
			// 施法者必須有城市，才能提供歸屬地
			if (selfActor.subspecies == null || selfActor.city == null)
			{
				return false;
			}
			// 3. 獲取搜索地塊
			WorldTile tTile = pTile ?? pTarget.current_tile;
			// 4. 獲取範圍內的單位 (使用 maxRange)
			var allClosestUnits = Finder.getUnitsFromChunk(tTile, maxRange); // <-- 修正範圍
			bool effectApplied = false;
			if (allClosestUnits.Any())
			{
				foreach (var unit in allClosestUnits)
				{
					// 基礎安全檢查
					if (unit == null || !unit.isActor() || !unit.a.isAlive())
						continue;
					Actor targetActor = unit.a; // 正確宣告目標 Actor
					// 5. 條件檢查
					// 條件1: 雙方國家相同
					// 條件2: 目標沒有居住城市
					// 條件3: 目標種族是 冰魔
					if (targetActor.kingdom != selfActor.kingdom &&
						targetActor.asset.id == "lil_pumpkin" &&
						(!targetActor.hasTrait("madness") || !targetActor.hasTrait("desire_harp") || !targetActor.hasTrait("desire_alien_mold") || !targetActor.hasTrait("desire_computer") || !targetActor.hasTrait("desire_golden_egg")) &&
						targetActor.city == null)
					{
						if (targetActor.kingdom != null)
						{
							targetActor.kingdom.units.Remove(targetActor);
						}

						// 步驟 2: 歸屬王國
						targetActor.kingdom = selfActor.kingdom;
						// 【安全檢查】：防止在週期運行時重複添加
						if (!selfActor.kingdom.units.Contains(targetActor))
						{
							selfActor.kingdom.units.Add(targetActor);
						}

						// 步驟 3: 歸屬城市
						targetActor.city = selfActor.city;
						// 【安全檢查】：防止在週期運行時重複添加
						if (!selfActor.city.units.Contains(targetActor))
						{
							selfActor.city.units.Add(targetActor);
						}
						effectApplied = true;
					}
				}
			}
			return effectApplied; // 只有在至少一個單位被歸化時，才返回 true
		}
		public static bool Return_04(BaseSimObject pTarget, WorldTile pTile = null)
		{// 生物質 歸籍 bioblob
			// 1. 基本安全檢查與施法者定義
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pTarget.a; // 施法者
			
			// 2. 參數設定與施法者檢查
			int maxRange = 1;
			// 施法者必須有城市，才能提供歸屬地
			if (selfActor.subspecies == null || selfActor.city == null)
			{
				return false;
			}
			// 3. 獲取搜索地塊
			WorldTile tTile = pTile ?? pTarget.current_tile;
			// 4. 獲取範圍內的單位 (使用 maxRange)
			var allClosestUnits = Finder.getUnitsFromChunk(tTile, maxRange); // <-- 修正範圍
			bool effectApplied = false;
			if (allClosestUnits.Any())
			{
				foreach (var unit in allClosestUnits)
				{
					// 基礎安全檢查
					if (unit == null || !unit.isActor() || !unit.a.isAlive())
						continue;
					Actor targetActor = unit.a; // 正確宣告目標 Actor
					// 5. 條件檢查
					// 條件1: 雙方國家相同
					// 條件2: 目標沒有居住城市
					// 條件3: 目標種族是 冰魔
					if (targetActor.kingdom != selfActor.kingdom &&
						targetActor.asset.id == "bioblob" &&
						(!targetActor.hasTrait("madness") || !targetActor.hasTrait("desire_harp") || !targetActor.hasTrait("desire_alien_mold") || !targetActor.hasTrait("desire_computer") || !targetActor.hasTrait("desire_golden_egg")) &&
						targetActor.city == null)
					{
						if (targetActor.kingdom != null)
						{
							targetActor.kingdom.units.Remove(targetActor);
						}

						// 步驟 2: 歸屬王國
						targetActor.kingdom = selfActor.kingdom;
						// 【安全檢查】：防止在週期運行時重複添加
						if (!selfActor.kingdom.units.Contains(targetActor))
						{
							selfActor.kingdom.units.Add(targetActor);
						}

						// 步驟 3: 歸屬城市
						targetActor.city = selfActor.city;
						// 【安全檢查】：防止在週期運行時重複添加
						if (!selfActor.city.units.Contains(targetActor))
						{
							selfActor.city.units.Add(targetActor);
						}
						effectApplied = true;
					}
				}
			}
			return effectApplied; // 只有在至少一個單位被歸化時，才返回 true
		}
		public static bool Return_05(BaseSimObject pTarget, WorldTile pTile = null)
		{// 冰塔 歸籍 cold_one
			// 1. 基本安全檢查與施法者定義
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pTarget.a; // 施法者
			
			// 2. 參數設定與施法者檢查
			int maxRange = 1;
			if(selfActor.asset.id == "cold_one" )
			{
				maxRange = 60;
			}
			// 施法者必須有城市，才能提供歸屬地
			if (selfActor.subspecies == null || selfActor.city == null /*|| selfActor.asset.id != "cold_one"*/)
			{
				return false;
			}
			// 3. 獲取搜索地塊
			WorldTile tTile = pTile ?? pTarget.current_tile;
			// 4. 獲取範圍內的單位 (使用 maxRange)
			var allClosestUnits = Finder.getUnitsFromChunk(tTile, maxRange); // <-- 修正範圍
			bool effectApplied = false;
			if (allClosestUnits.Any())
			{
				foreach (var unit in allClosestUnits)
				{
					// 基礎安全檢查
					if (unit == null || !unit.isActor() || !unit.a.isAlive())
						continue;
					Actor targetActor = unit.a; // 正確宣告目標 Actor
					// 5. 條件檢查
					// 條件1: 雙方國家相同
					// 條件2: 目標沒有居住城市
					// 條件3: 目標種族是 冰魔
					if (targetActor.kingdom != selfActor.kingdom &&
						targetActor.asset.id == "cold_one" &&
						(!targetActor.hasTrait("madness") || !targetActor.hasTrait("desire_harp") || !targetActor.hasTrait("desire_alien_mold") || !targetActor.hasTrait("desire_computer") || !targetActor.hasTrait("desire_golden_egg")) &&
						targetActor.city == null)
					{
						if (targetActor.kingdom != null)
						{
							targetActor.kingdom.units.Remove(targetActor);
						}

						// 步驟 2: 歸屬王國
						targetActor.kingdom = selfActor.kingdom;
						// 【安全檢查】：防止在週期運行時重複添加
						if (!selfActor.kingdom.units.Contains(targetActor))
						{
							selfActor.kingdom.units.Add(targetActor);
						}

						// 步驟 3: 歸屬城市
						targetActor.city = selfActor.city;
						// 【安全檢查】：防止在週期運行時重複添加
						if (!selfActor.city.units.Contains(targetActor))
						{
							selfActor.city.units.Add(targetActor);
						}
						if (selfActor.subspecies.hasTrait("prefrontal_cortex"))
						{// 施法者 有
							if (!targetActor.subspecies.hasTrait("prefrontal_cortex"))
							{// 且 目標 沒有
								targetActor.subspecies.addTrait("prefrontal_cortex");
							}
						}
						effectApplied = true;
					}
				}
			}
			return effectApplied; // 只有在至少一個單位被歸化時，才返回 true
		}
		public static bool Return_06(BaseSimObject pTarget, WorldTile pTile = null)
		{// 火塔 歸籍 demon
			// 1. 基本安全檢查與施法者定義
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pTarget.a; // 施法者
			
			// 2. 參數設定與施法者檢查
			int maxRange = 1;
			if(selfActor.asset.id == "demon" )
			{
				maxRange = 60;
			}
			// 施法者必須有城市，才能提供歸屬地
			if (selfActor.subspecies == null || selfActor.city == null/* || selfActor.asset.id != "demon"*/)
			{
				return false;
			}
			// 3. 獲取搜索地塊
			WorldTile tTile = pTile ?? pTarget.current_tile;
			// 4. 獲取範圍內的單位 (使用 maxRange)
			var allClosestUnits = Finder.getUnitsFromChunk(tTile, maxRange); // <-- 修正範圍
			bool effectApplied = false;
			if (allClosestUnits.Any())
			{
				foreach (var unit in allClosestUnits)
				{
					// 基礎安全檢查
					if (unit == null || !unit.isActor() || !unit.a.isAlive())
						continue;
					Actor targetActor = unit.a; // 正確宣告目標 Actor
					// 5. 條件檢查
					// 條件1: 雙方國家相同
					// 條件2: 目標沒有居住城市
					// 條件3: 目標種族是 惡魔
					// 條件4: 目標沒有精神異常特質
					if (targetActor.kingdom != selfActor.kingdom &&
						targetActor.asset.id == "demon" &&
						(!targetActor.hasTrait("madness") || !targetActor.hasTrait("desire_harp") || !targetActor.hasTrait("desire_alien_mold") || !targetActor.hasTrait("desire_computer") || !targetActor.hasTrait("desire_golden_egg")) &&
						targetActor.city == null)
					{
						if (targetActor.kingdom != null)
						{
							targetActor.kingdom.units.Remove(targetActor);
						}

						// 步驟 2: 歸屬王國
						targetActor.kingdom = selfActor.kingdom;
						// 【安全檢查】：防止在週期運行時重複添加
						if (!selfActor.kingdom.units.Contains(targetActor))
						{
							selfActor.kingdom.units.Add(targetActor);
						}

						// 步驟 3: 歸屬城市
						targetActor.city = selfActor.city;
						// 【安全檢查】：防止在週期運行時重複添加
						if (!selfActor.city.units.Contains(targetActor))
						{
							selfActor.city.units.Add(targetActor);
						}
						if (selfActor.subspecies.hasTrait("prefrontal_cortex"))
						{// 施法者 有
							if (!targetActor.subspecies.hasTrait("prefrontal_cortex"))
							{// 且 目標 沒有
								targetActor.subspecies.addTrait("prefrontal_cortex");
							}
						}
						effectApplied = true;
					}
				}
			}
			return effectApplied; // 只有在至少一個單位被歸化時，才返回 true
		}
		public static bool Return_07(BaseSimObject pTarget, WorldTile pTile = null)
		{// 天塔 歸籍 angle
			// 1. 基本安全檢查與施法者定義
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pTarget.a; // 施法者
			
			// 2. 參數設定與施法者檢查
			int maxRange = 60;
		//	if(selfActor.asset.id == "angle" )
		//	{
		//		maxRange = 60;
		//	}
			// 施法者必須有城市，才能提供歸屬地
			if (selfActor.subspecies == null || selfActor.city == null /*|| selfActor.asset.id != "angle"*/)
			{
				return false;
			}
			// 3. 獲取搜索地塊
			WorldTile tTile = pTile ?? pTarget.current_tile;
			// 4. 獲取範圍內的單位 (使用 maxRange)
			var allClosestUnits = Finder.getUnitsFromChunk(tTile, maxRange); // <-- 修正範圍
			bool effectApplied = false;
			if (allClosestUnits.Any())
			{
				foreach (var unit in allClosestUnits)
				{
					// 基礎安全檢查
					if (unit == null || !unit.isActor() || !unit.a.isAlive())
						continue;
					Actor targetActor = unit.a; // 正確宣告目標 Actor
					// 5. 條件檢查
					// 條件1: 雙方國家相同
					// 條件2: 目標沒有居住城市
					// 條件3: 目標種族是 天使
					if (targetActor.kingdom != selfActor.kingdom &&
						targetActor.asset.id == "angle" &&
						(!targetActor.hasTrait("madness") || !targetActor.hasTrait("desire_harp") || !targetActor.hasTrait("desire_alien_mold") || !targetActor.hasTrait("desire_computer") || !targetActor.hasTrait("desire_golden_egg")) &&
						targetActor.city == null)
					{
						if (targetActor.kingdom != null)
						{
							targetActor.kingdom.units.Remove(targetActor);
						}

						// 步驟 2: 歸屬王國
						targetActor.kingdom = selfActor.kingdom;
						// 【安全檢查】：防止在週期運行時重複添加
						if (!selfActor.kingdom.units.Contains(targetActor))
						{
							selfActor.kingdom.units.Add(targetActor);
						}

						// 步驟 3: 歸屬城市
						targetActor.city = selfActor.city;
						// 【安全檢查】：防止在週期運行時重複添加
						if (!selfActor.city.units.Contains(targetActor))
						{
							selfActor.city.units.Add(targetActor);
						}
						if (selfActor.subspecies.hasTrait("prefrontal_cortex"))
						{// 施法者 有
							if (!targetActor.subspecies.hasTrait("prefrontal_cortex"))
							{// 且 目標 沒有
								targetActor.subspecies.addTrait("prefrontal_cortex");
							}
						}
						effectApplied = true;
					}
				}
			}
			return effectApplied; // 只有在至少一個單位被歸化時，才返回 true
		}

		public static bool removeRuins(BaseSimObject pSelf, WorldTile pTile = null)
		{// 移除廢墟
			// 1. 基本安全檢查：確保呼叫者是個活著的單位，並且有作用的所在位置
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf as Actor;
			// 2. 設定作用範圍的中心點
			WorldTile centerTile = (pTile != null) ? pTile : selfActor.current_tile;
			if (centerTile == null)
			{
				return false;
			}
			// 3. 根據特質設定作用範圍半徑
			int removalRadius = 10;
			if (selfActor.hasTrait("talent_build"))
			{
				removalRadius = 20;
			}
			bool buildingRemoved = false;
			// 4. 遍歷範圍內的每個地塊
			for (int x = -removalRadius; x <= removalRadius; x++)
			{
				for (int y = -removalRadius; y <= removalRadius; y++)
				{
					// 修正: 使用 World.world.GetTile(x, y) 並傳入正確的絕對座標
					WorldTile targetTile = World.world.GetTile(centerTile.x + x, centerTile.y + y);
					// 5. 檢查地塊是否存在且有廢墟
					if (targetTile != null && targetTile.hasBuilding() && targetTile.building.isRuin())
					{
						// 6. 觸發拆除
						targetTile.building.startDestroyBuilding();
						buildingRemoved = true;
					}
				}
			}
			// 7. 返回是否成功移除過任何建築
			return buildingRemoved;
		}
		public static bool DemolishInactive(BaseSimObject pSelf, WorldTile pTile = null)
		{// 移除非活性火山 酸泉
			// 1. 安全檢查：確保 pSelf 及其 Actor 組件存在且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			// 2. 定義要影響的建築物 ID 和作用半徑
			List<string> buildingIDsToAffect = new List<string> { "volcano", "geyser_acid" };
			const float effectRadius = 10f; // 設定作用半徑，可根據需要調整
			bool effectTriggered = false; // 用來追蹤是否有建築物被移除
			// 3. 遍歷遊戲世界中所有的建築物
			foreach (var building in World.world.buildings)
			{
				// 4. 確保建築物是有效的
				if (building == null || !building.isAlive())
				{
					continue;
				}
				// 5. 檢查建築物是否為目標類型、在範圍內，且帶有指定的旗標
				if (buildingIDsToAffect.Contains(building.asset.id))
				{
					float distance = Vector2.Distance(selfActor.current_position, building.current_position);
					if (distance <= effectRadius && building.data.hasFlag("stop_spawn_drops"))
					{
						// 6. 如果所有條件都符合，則移除建築物
						building.startDestroyBuilding();
						effectTriggered = true;
					}
				}
			}
			// 7. 根據是否有建築物被移除來返回結果
			return effectTriggered;
		}
		public static bool StopErupting(BaseSimObject pSelf, WorldTile pTile = null)
		{// 火山酸泉非活性化 (慢速)
			// 1. 安全檢查：確保 pSelf 及其 Actor 組件存在且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive())
				return false;
			Actor selfActor = pSelf.a;
			// 2. 定義要影響的建築物 ID 和作用半徑
			List<string> buildingIDsToAffect = new List<string> { "volcano", "geyser_acid" };
			const float effectRadius = 15f; // 設定作用半徑
			bool effectTriggered = false; // 用來追蹤是否有建築物被影響
			// 3. 遍歷遊戲世界中所有的建築物
			foreach (var building in World.world.buildings)
			{
				// 確保建築物是有效的，並且在作用半徑內
				if (building != null && buildingIDsToAffect.Contains(building.asset.id))
				{
					float distance = Vector2.Distance(selfActor.current_position, building.current_position);
					if (distance <= effectRadius)
					{
						// 4. 如果建築物是目標且在範圍內，則為其添加旗標
						building.data.addFlag("stop_spawn_drops");
						effectTriggered = true;
					}
				}
			}
			// 5. 根據是否有建築物受到影響來返回結果
			return effectTriggered;
		}
		public static bool BuildingUp(BaseSimObject pSelf, WorldTile pTile = null)
		{// 建築物升級效果
			// 1. 基本安全檢查：確保 pSelf 是有效的 Actor
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive())
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			string cooldownStatus = "cdt_alter04";
			float cooldownDuration = 1200f;
			
			// 2. 冷卻狀態檢查
			if (selfActor.hasStatus(cooldownStatus))
			{
				return false;
			}
			
			// 3. 尋找範圍內所有可升級的建築物
			float maxRange = 10f;
			List<Building> buildingsToUpgrade = new List<Building>();
			
			foreach (var building in World.world.buildings)
			{
				if (building == null || building.kingdom == null || building.kingdom != selfActor.kingdom)
					continue; // 跳過無效或非己方建築物
				
				float dist = Vector2.Distance(selfActor.current_position, building.current_position);
				
				// 檢查建築物是否在範圍內且可以升級
				if (dist <= maxRange && building.canBeUpgraded() && !string.IsNullOrWhiteSpace(building.asset.upgrade_to))
				{
					buildingsToUpgrade.Add(building);
				}
			}

			// 4. 檢查是否找到任何建築物，並進行升級
			if (buildingsToUpgrade.Count > 0)
			{
				foreach (var targetBuilding in buildingsToUpgrade)
				{
					BuildingAsset pTemplate = AssetManager.buildings.get(targetBuilding.asset.upgrade_to);
					targetBuilding.city?.setBuildingDictID(targetBuilding);
					targetBuilding.setTemplate(pTemplate);
					targetBuilding.initAnimationData();
					targetBuilding.updateStats();
					targetBuilding.data.health = targetBuilding.getMaxHealth();
					targetBuilding.fillTiles();
				}
				
				// 在所有建築物升級後，施加冷卻
				selfActor.addStatusEffect(cooldownStatus, cooldownDuration);
				return true; // 成功升級，回傳 true
			}
			
			return false; // 沒有找到符合條件的建築物，回傳 false
		}
			#endregion
			#region 清單
		public static readonly HashSet<string> SevenDemonKingStatus_Brave = new HashSet<string>
		{// 魔王狀態 (清單) 裁決專用
			"arrogant_demon_king",
			"greedy_demon_king",
			"lust_demon_king",
			"wrath_demon_king",
			"gluttony_demon_king",
			"sloth_demon_king",
			"envy_demon_king",
			"ex_undead_emperor",
		};
		public static readonly HashSet<string> SevenDemonKingStatus_DemonKing = new HashSet<string>
		{// 七魔王狀態 (清單) 魔王專用
			"arrogant_demon_king",
			"greedy_demon_king",
			"lust_demon_king",
			"wrath_demon_king",
			"gluttony_demon_king",
			"sloth_demon_king",
			"envy_demon_king",
			"brave",
		};
		private static readonly HashSet<string> SevenDemonKingStatus2 = new HashSet<string>
		{// 武器覺醒 (清單)
			"adk2",
			"gdk3",
			"ldk2",
			"wdk2",
			"gldk2",
			"sdk2",
			"edk2",
		};
		private static readonly HashSet<string> UnplantableBiomes = new HashSet<string>
		{// 不可種植樹果的群系 (清單)
			"biome_wasteland",
			"biome_corrupted",
			"biome_infernal",
			"biome_swamp",
			"biome_desert",
			"biome_crystal",
			"biome_candy",
			"biome_permafrost",
			"biome_rocklands",
			"biome_celestial",
			"biome_singularity",
			"biome_paradox",
			"biome_hill",
			"biome_sand",
			"biome_tumor",
			"biome_biomass",
			"biome_pumpkin",
			"biome_cybertile"
		};
		private static readonly HashSet<string> AllTile01 = new HashSet<string>
		{// 一般全部地塊 (築巢專用清單)
			"pit_deep_ocean",		// 深海水坑
			"pit_close_ocean",		// 近海水坑
			"pit_shallow_waters",	// 淺灘水坑
			"ice",					// 冰面
			"deep_ocean",			// 深海
			"close_ocean",			// 近海
			"shallow_waters",		// 淺灘
			"sand",					// 沙
			"hills",				// 丘
			"mountains",			// 山
			"summit",				// 峰
			"snow_sand",			// 雪沙
			"snow_hills",			// 雪丘
			"snow_block",			// 雪山
			"soil_low",				// 00 泥地土
			"soil_high",			// 00 林地土
			"grass_low",			// 01 草 泥地
			"grass_high",			// 01 草 林地
			"savanna_low",			// 02 稀樹 泥地
			"savanna_high",			// 02 稀樹 林地
			"mushroom_low",			// 03 蘑菇 泥地
			"mushroom_high",		// 03 蘑菇 林地
			"enchanted_low",		// 04 魔法 泥地
			"enchanted_high",		// 04 魔法 林地
			"corrupted_low",		// 05 腐朽 泥地
			"corrupted_high",		// 05 腐朽 林地
			"swamp_low",			// 06 沼澤 泥地
			"swamp_high",			// 06 沼澤 林地
			"infernal_low",			// 07 地獄 泥地
			"infernal_high",		// 07 地獄 林地
			"jungle_low",			// 08 叢林 泥地
			"jungle_high",			// 08 叢林 林地
			"candy_low",			// 09 糖果 泥地
			"candy_high",			// 09 糖果 林地
			"desert_low",			// 10 沙漠 泥地
			"desert_high",			// 10 沙漠 林地
			"crystal_low",			// 11 水晶 泥地
			"crystal_high",			// 11 水晶 林地
			"lemon_low",			// 12 檸檬 泥地
			"lemon_high",			// 12 檸檬 林地
			"permafrost_low",		// 13 凍土 泥地
			"permafrost_high",		// 13 凍土 林地
			"birch_low",			// 14 樺木 泥地
			"birch_high",			// 14 樺木 林地
			"maple_low",			// 15 楓樹 泥地
			"maple_high",			// 15 楓樹 林地
			"rocklands_low",		// 16 巨石 泥地
			"rocklands_high",		// 16 巨石 林地
			"garlic_low",			// 17 大蒜 泥地
			"garlic_high",			// 17 大蒜 林地
			"flower_low",			// 18 花卉 泥地
			"flower_high",			// 18 花卉 林地
			"celestial_low",		// 19 天界 泥地
			"celestial_high",		// 19 天界 林地
			"singularity_low",		// 20 奇點 泥地
			"singularity_high",		// 20 奇點 林地
			"clover_low",			// 21 三葉 泥地
			"clover_high",			// 21 三葉 林地
			"paradox_low",			// 21 三葉 泥地
			"paradox_high",			// 21 三葉 林地
		};
		private static readonly HashSet<string> Lava = new HashSet<string>
		{// 岩漿
			"lava0",				//岩漿1
			"lava1",				//岩漿1
			"lava2",				//岩漿1
			"lava3",				//岩漿1
		};
		private static readonly HashSet<string> UnusablePlots = new HashSet<string>
		{// 效果不發動的地塊 (變遷專用清單)
			"road",					//道路
			"field",				//農田
			"pit_deep_ocean",		//深海水坑
			"pit_close_ocean",		//近海水坑
			"pit_shallow_waters",	//淺灘水坑
			"ice",					//冰面
			"deep_ocean",			//深海
			"close_ocean",			//近海
			"shallow_waters",		//淺灘
			"lava0",				//岩漿1
			"lava1",				//岩漿1
			"lava2",				//岩漿1
			"lava3",				//岩漿1
			"grey_goo",				//灰色黏液
			"snow_sand",			//雪沙
			"snow_hills",			//雪丘
			"snow_block",			//雪塊
			"frozen_low",			//雪泥地
			"frozen_high",			//雪森土
			"sand",					//沙
			"hills",				//丘
			"mountains",			//山
			"summit",				//峰
			"tumor_low",			//腫瘤泥土
			"tumor_high",			//腫瘤林地土
			"biomass_low",			//生物質泥土
			"biomass_high",			//生物質林地土
			"pumpkin_low",			//南瓜泥土
			"pumpkin_high",			//南瓜林地土
			"cybertile_low",		//核心泥土
			"cybertile_high",		//核心林地土
		};
		private static readonly HashSet<string> UnusablePlots2 = new HashSet<string>
		{// 地塊 (築巢專用清單)
			"pit_deep_ocean",		//深海水坑
			"pit_close_ocean",		//近海水坑
			"pit_shallow_waters",	//淺灘水坑
			"ice",					//冰面
			"deep_ocean",			//深海
			"close_ocean",			//近海
			"shallow_waters",		//淺灘
			"lava0",				//岩漿1
			"lava1",				//岩漿1
			"lava2",				//岩漿1
			"lava3",				//岩漿1
			"grey_goo",				//灰色黏液
			"mountains",			//山
			"summit",				//峰
			"snow_block",			//雪山
		};
		private static readonly HashSet<string> UnusablePlots3 = new HashSet<string>
		{// 地塊 (有害型清單)
			"tumor_low",			//腫瘤泥土
			"tumor_high",			//腫瘤林地土
			"biomass_low",			//生物質泥土
			"biomass_high",			//生物質林地土
			"pumpkin_low",			//南瓜泥土
			"pumpkin_high",			//南瓜林地土
		};
		public static readonly HashSet<string> BadBuilding = new HashSet<string>
		{// 建築 (有害型清單)
			"tumor",				//母巢建築
			"super_pumpkin",		//南瓜建築
			"biomass",				//生物質建築
			"ice_tower",			//冰塔
			"flame_tower",			//冰塔
			"corrupted_brain",		//墮落之腦
			"waypoint_computer",	//邪惡電腦
			"waypoint_golden_egg",	//金蛋
			"waypoint_harp",		//豎琴
			"waypoint_alien_mold",	//外星黴菌
		};
		private static readonly HashSet<string> Kara = new HashSet<string>
		{// 裁決 投槍 生物清單
		};
		private static readonly HashSet<string> AngelAttackTarget = new HashSet<string>
		{// 天使攻擊目標 (清單)
			"alien",
			"UFO",
			"bandit",
			"dragon",
			"demon",
			"fire_skull",
			"jumpy_skull",
			"ghost",
			"skeleton",
			"necromancer",
			"cold_one",
			"god_finger",
			"evil_mage",
			"living_house",
			"greg",
			"fire_elemental",
			"fire_elemental_blob",
			"fire_elemental_snake",
			"fire_elemental_horse",
			"fire_elemental_slug",
			"mush_animal",
			"mush_unit",
			"assimilator",
			"bioblob",
			"lil_pumpkin",
			"tumor_monster_animal",
			"tumor_monster_unit",
			"zombie_dragon"
		};
		private static readonly HashSet<string> DivineLightTarget = new HashSet<string>
		{// 不死生物 (清單)
			"jumpy_skull",
			"ghost",
			"skeleton",
			"demon",
			"mush_unit",
			"mush_animal",
		};
		private static readonly HashSet<string> TargetTrait = new HashSet<string>
		{// 天使攻擊目標 (清單)
			"zombie",
			"madness",
			"desire_alien_mold",
			"desire_computer",
			"desire_golden_egg",
			"desire_harp"
		};
		private static readonly HashSet<string> Trait_Atk = new HashSet<string>
		{// 御時法 負面效果添加對象 : 戰技、子彈、附魔 (60)
			"cb_holdfast",
			"cb_slam",
			"cb_bulletrain",
			"cb_experience",
			"projectile01",
			"projectile02",
			"projectile03",
			"projectile04",
			"projectile05",
			"projectile06",
			"projectile07",
			"projectile08",
			"projectile09",
			"projectile10",
			"projectile11",
			"projectile12",
			"projectile13",
			"projectile14",
			"projectile15",
			"add_burning",
			"add_slowdown",
			"add_frozen",
			"add_poisonous",
			"add_afc",
			"add_silenced",
			"add_stunned",
			"add_unknown",
			"add_cursed",
			"add_death",
		};
		private static readonly HashSet<string> Trait_Status = new HashSet<string>
		{// 御時法 負面效果添加對象 : 強化 (120)
			"status_powerup",
			"status_caffeinated",
			"status_enchanted",
			"status_rage",
			"status_spellboost",
			"status_motivated",
			"status_shield",
			"status_invincible",
			"status_inspired",
			"status_afo",
			"status_ofa",
		};
		private static readonly HashSet<string> Trait_Holy = new HashSet<string>
		{// 御時法 負面效果添加對象 : 恢復 (150)
			"holyarts_ha",
			"holyarts_cure",
			"holyarts_heal",
			"holyarts_healcure",
			"holyarts_rainfall",
		};
		private static readonly HashSet<string> Trait_Const = new HashSet<string>
		{// 御時法 負面效果添加對象 : 建造 (5400)
			"monste_nest000",
			"monste_nest001",
			"monste_nest002",
			"monste_nest003",
			"monste_nest004",
			"monste_nest005",
			"monste_nest006",
			"monste_nest007",
			"monste_nest008",
			"monste_nest009",
			"monste_nest010",
			"monste_nest011",
			"monste_nest012",
		};
		private static readonly HashSet<string> EvilLawTraits = new HashSet<string>
		{// 邪惡特質清單 (特防專用)
			"evillaw_tgc",
			"evillaw_tc",
			"evillaw_disease",
			"evillaw_ea",
			"evillaw_sterilization",
			"evillaw_tantrum",
			"evillaw_starvation",
			"evillaw_sleeping",
			"evillaw_devour",
			"evillaw_seduction",
			"evillaw_moneylaw",
			"evillaw_ew",
			"evil"
		};
		private static readonly HashSet<string> EvilLawTraitsATK = new HashSet<string>
		{// 邪惡特質清單 (裁決之槍攻擊清單)
			"evillaw_tgc",
			"evillaw_tc",
			"evillaw_disease",
			"evillaw_ea",
			"evillaw_sterilization",
			"evillaw_tantrum",
			"evillaw_starvation",
			"evillaw_sleeping",
			"evillaw_devour",
			"evillaw_seduction",
			"evillaw_moneylaw",
			"evillaw_ew",
			"evil",
			"zombie",
			"monster",
		};
			#endregion
			#region 輔助類程式碼 與 函數 其他
	//功能性效果
		public static bool removeDemonKingAwakening(BaseSimObject pSelf, WorldTile pTile = null)
		{// 移除魔王武器覺醒狀態
			// 1. 基本安全檢查：確保 pSelf 及其 Actor 組件存在且存活
			if (pSelf == null || pSelf.a == null || !pSelf.a.isAlive()) 
			{
				return false;
			}
			Actor selfActor = pSelf.a;
			
			// 2. 檢查單位是否持有任何一個七魔王狀態
			bool hasDemonKingStatus = false;
			foreach (string statusID in SevenDemonKingStatus_DemonKing)
			{
				if (selfActor.hasStatus(statusID))
				{
					hasDemonKingStatus = true;
					break; // 只要找到一個魔王狀態，就跳出迴圈
				}
			}
			// 3. 如果單位沒有任何魔王狀態，則移除所有武器覺醒狀態
			if (!hasDemonKingStatus)
			{
				foreach (string awakeningStatusID in SevenDemonKingStatus2)
				{
					if (selfActor.hasStatus(awakeningStatusID))
					{
						selfActor.finishStatusEffect(awakeningStatusID);
					}
				}
				return true; // 狀態被成功移除
			}
			
			return false; // 單位持有魔王狀態，無需操作
		}
		public static bool addFavorite1(BaseSimObject pTarget, WorldTile pTile = null)
		{// 添加最愛
			// 確保 pTarget 是有效的 Actor 並且存活
			if (pTarget == null || !(pTarget is Actor actor) || !actor.isAlive())
			{
				return false;
			}

			// 保留的邏輯：將單位設置為最愛
			actor.data.favorite = true; // for easier finding

			// 由於只保留這一行，其他所有複雜的判斷和效果都被移除了
			return true;
		}
		public static bool addFavorite2(BaseSimObject pTarget, WorldTile pTile)
		{// 添加最愛
			// 確保 pTarget 是有效的 Actor 並且存活
			// 在此版本中，pTile 被假設為已提供，但您仍然可以在此處添加 pTile == null 的檢查，如果這是必要的業務邏輯
			if (pTarget == null || !(pTarget is Actor actor) || !actor.isAlive())
			{
				return false;
			}

			// 保留的邏輯：將單位設置為最愛
			actor.data.favorite = true; // for easier finding

			// 由於只保留這一行，其他所有複雜的判斷和效果都被移除了
			return true;
		}
		public static bool TraittAddRemove0(BaseSimObject pTarget, WorldTile pTile)
		{// 指定移除
			// 基礎安全檢查：確保目標單位及其 Actor 組件存在
			if (pTarget == null || pTarget.a == null)
			{
				return false; // 無效目標，直接返回
			}
			Actor targetActor = pTarget.a;
			// 定義要移除的一般特質列表
			HashSet<string> traitsToRemove = new HashSet<string>()
			{
				"pro_king",
				"pro_leader",	
				"pro_warrior",
				"pro_groupleader",
				"status_invincible",
				"status_AFO",
				"status_OFA",
				"add_death",
				"holyarts_ha",
				"holyarts_annunciation",
				"holyarts_eucharist",
				"holyarts_justice",
				"evillaw_tgc",
				"evillaw_devour",
				"evillaw_tc",
				"evillaw_starvation",
				"evillaw_disease",
				"evillaw_moneylaw",
				"evillaw_ea",
				"evillaw_sleeping",
				"evillaw_sterilization",
				"evillaw_seduction",
				"evillaw_ew",
			};
			// 判斷是否滿足移除條件：單位沒有 "prefrontal_cortex" 亞種特質
			bool shouldRemoveGeneralTraits = false;
			// 首先，檢查目標是否有 subspecies 物件。如果沒有，那肯定就沒有 prefrontal_cortex
			if (targetActor.subspecies == null)
			{
				shouldRemoveGeneralTraits = true;
				//Debug.Log($"SubspeciesTraittAddRemoveXXX: 單位 {targetActor.name} 沒有亞種項目，判斷為不具備 prefrontal_cortex。");
			}
			// 如果有 subspecies 物件，則檢查它是否沒有 "prefrontal_cortex" 特質
			else if (!targetActor.subspecies.hasTrait("prefrontal_cortex"))
			{
				shouldRemoveGeneralTraits = true;
				//Debug.Log($"SubspeciesTraittAddRemoveXXX: 單位 {targetActor.name} 沒有 prefrontal_cortex 亞種特質。");
			}
			// 執行移除操作和設置 favorite
			if (shouldRemoveGeneralTraits)
			{
				// 移除指定的一般特質
				foreach (string traitID in traitsToRemove)
				{
					if (targetActor.hasTrait(traitID)) // 只有當單位確實擁有該特質時才移除
					{
						targetActor.removeTrait(traitID);
						//Debug.Log($"SubspeciesTraittAddRemoveXXX: 從 {targetActor.name} 移除了特質: {traitID}。");
					}
				}
				// 將 favorite 設值成 false
				targetActor.data.favorite = false;
				//Debug.Log($"SubspeciesTraittAddRemoveXXX: 將 {targetActor.name} 的收藏狀態設為 false。");
			}
			return true; // 函數執行完成
		}

		public static bool weapon_get(BaseSimObject pTarget, WorldTile pTile = null)
		{// 裝備賦予 持續添加型
			// 無條件檢查目標是否有效且存活
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
				return false;
			// 獲取 "evil_sword" 物品資產
			var weapon = AssetManager.items.get("evil_sword");
			// 生成 "evil_sword" 物品實例
			var pData = World.world.items.generateItem(pItemAsset: weapon);
			// 獲取目標的武器槽位
			var pSlot = pTarget.a.equipment.getSlot(EquipmentType.Weapon);
			// 將 "evil_sword" 裝備到目標的武器槽位
			pSlot.setItem(pData, pTarget.a);
			// 更新目標的統計數據，以反映新裝備帶來的變化
			pTarget.setStatsDirty();
			return true; // 表示效果成功施加
		}
		public static bool isEnemy(Actor pUnit1, Actor pUnit2)
		{// 輔助函式：敵對定義
		/// 檢查兩個單位是否為敵人。
		/// </summary>
		/// <param name="pUnit1">第一個單位</param>
		/// <param name="pUnit2">第二個單位</param>
		/// <returns>如果是敵人則返回 true，否則返回 false。</returns>
			if (pUnit1 == null || pUnit2 == null || !pUnit1.isAlive() || !pUnit2.isAlive())
			{
				return false;
			}
			// 檢查兩個單位是否有有效的王國，並直接使用 Kingdom 類別的 isEnemy 方法
			return pUnit1.kingdom != null && pUnit2.kingdom != null && pUnit1.kingdom.isEnemy(pUnit2.kingdom);
		}
		private static bool check_for_buildings(Actor pActor, float pRadius, string pBuildingID)
		{// 輔助函式：檢查特定範圍內是否有指定建築物
			// 遍歷遊戲世界中所有的建築物
			foreach (var building in World.world.buildings)
			{
				// 檢查建築物是否存在且 ID 相符
				if (building.asset.id == pBuildingID)
				{
					// 計算建築物與單位的距離
					float distance = Vector2.Distance(pActor.current_position, building.current_position);
					// 如果距離在指定半徑內，則表示找到，立即返回 true
					if (distance < pRadius)
					{
						return true;
					}
				}
			}
			// 如果遍歷完所有建築物都沒找到，則返回 false
			return false;
		}
		private static bool check_for_unusable_plots_in_radius(Actor pActor, float pRadius)
		{// 築巢 輔助方法：檢查指定半徑內是否有不適合的地塊
			if (pActor == null) return false;

			int centerTileX = (int)pActor.current_position.x;
			int centerTileY = (int)pActor.current_position.y;
			int radiusInTiles = Mathf.CeilToInt(pRadius);

			for (int x = centerTileX - radiusInTiles; x <= centerTileX + radiusInTiles; x++)
			{
				for (int y = centerTileY - radiusInTiles; y <= centerTileY + radiusInTiles; y++)
				{
					// 確保瓦片座標在地圖範圍內
					if (x >= 0 && x < MapBox.width && y >= 0 && y < MapBox.height)
					{
						WorldTile tile = World.world.GetTile(x, y);
						// 檢查瓦片是否存在且其類型ID是否在 UnusablePlots2 清單中
						if (tile != null && tile.Type != null && UnusablePlots2.Contains(tile.Type.id))
						{
							return true; // 找到了不適合的地塊，立即返回 true
						}
					}
				}
			}
			return false; // 沒有找到不適合的地塊
		}
		public static bool TransformMTF(BaseSimObject pTarget, WorldTile pTile = null)
		{// 性別轉換 Gender transition 男 >> 女 基本版
			// 1. 基本安全檢查：確保目標 pTarget 是有效的 Actor 並且存活
			if (pTarget == null || !(pTarget is Actor targetActor) || !targetActor.isAlive())
			{
				// Debug.LogWarning("TransformMaleToFemale: 目標無效或不存在 Actor 組件或已死亡.");
				return false; // 無效的目標
			}
			if (targetActor.data.sex == ActorSex.Male) // 直接訪問 data.sex
			{
				targetActor.data.sex = ActorSex.Female;
				//Debug.Log($"單位 {targetActor.name.ToString()} 已成功從男性轉變為女性！");
				return true; // 成功執行變性
			}
			else if (targetActor.data.sex == ActorSex.Female) // 直接訪問 data.sex
			{
				// Debug.Log($"單位 {targetActor.name.ToString()} 已經是女性，無需變性。");
				return false; // 目標已經是女性，無需變性
			}
			else
			{
				// Debug.LogWarning($"TransformMaleToFemale: 無法判斷單位 {targetActor.name.ToString()} 的性別或非男性。");
				return false; // 無法判斷性別或非男性
			}
		}
		public static bool TransformFTM(BaseSimObject pTarget, WorldTile pTile = null)
		{// 性別轉換 Gender transition 女 >> 男 基本版
			// 1. 基本安全檢查：確保目標 pTarget 是有效的 Actor 並且存活
			if (pTarget == null || !(pTarget is Actor targetActor) || !targetActor.isAlive())
			{
				return false; // 無效的目標
			}
			if (targetActor.data.sex == ActorSex.Female) // 直接訪問 data.sex
			{
				targetActor.data.sex = ActorSex.Male;
				return true;
			}
			else if (targetActor.data.sex == ActorSex.Male) // 直接訪問 data.sex
			{
				return false;
			}
			else
			{
				return false; // 無法判斷性別或非男性
			}
		}
		public static bool TraitAddDamage(BaseSimObject pTarget, WorldTile pTile = null)
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
		public static bool TraitCityConversion01(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 特質城市歸屬轉換 (主人方發動) pSelf 將 pTarget 的城市轉移給自身所屬的王國
			// 1. 基本安全與類型檢查
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive() || pSelf.a.kingdom == null || pSelf.a.city == null)
				return false; // 主人無效、未啟用或沒有王國/城市
			if (pTarget == null || !pTarget.isActor() || !pTarget.a.isAlive() || pTarget.a.kingdom == null || pTarget.a.city == null)
				return false; // 奴隸無效、未啟用或沒有王國/城市
			// 確保主人和奴隸不是同一個單位（理論上不可能，但作為安全檢查）
			if (pSelf == pTarget)
				return false;
			Actor master = pSelf.a;
			Actor slave = pTarget.a;
			// 2. 獲取核心對象
			City targetCity = slave.city;		   // 奴隸所屬的城市 (即將被轉換的城市)
			// 【關鍵修正】：使用城市本身的 Kingdom 屬性，無視奴隸單位的 Kingom 屬性
			Kingdom targetCityKingdom = targetCity.kingdom; // <--- 修正點
			Kingdom newKingdom = master.kingdom;	// 主人王國 (新的歸屬王國)
			// 3. 核心邏輯檢查 (避免不必要的轉換)
			// 檢查目標城市和新王國是否已經是同一歸屬
			if (targetCityKingdom == newKingdom) // <--- 檢查 targetCityKingdom
			{
				return true; 
			}
			// 4. 執行城市歸屬轉換
			targetCity.joinAnotherKingdom(newKingdom);
			// 5. 確保奴隸單位歸屬同步 (此步驟可選，因為 joinAnotherKingdom 應該會處理)
			slave.kingdom = newKingdom;
			slave.city = targetCity;
			return true;
		}
		public static bool TraitCityConversion02(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 特質城市歸屬轉移 (奴隸方發動) pSelf 將自身的城市轉移給 pTarget 所屬的王國
			// 1. 基本安全與類型檢查
			// pSelf 現在是奴隸（發動者）
			if (pSelf == null || !pSelf.isActor() || !pSelf.a.isAlive() || pSelf.a.kingdom == null || pSelf.a.city == null)
				return false; // 奴隸無效、未啟用或沒有王國/城市
			
			// pTarget 現在是主人
			if (pTarget == null || !pTarget.isActor() || !pTarget.a.isAlive() || pTarget.a.kingdom == null)
				return false; // 主人無效、未啟用或沒有王國
			
			// 確保主人和奴隸不是同一個單位
			if (pSelf == pTarget)
				return false;
			
			Actor slave = pSelf.a;
			Actor master = pTarget.a;
			
			// 2. 獲取核心對象
			City slaveCity = slave.city;		// 奴隸所屬的城市 (即將被轉換的城市)
			Kingdom slaveCityKingdom = slaveCity.kingdom; // 奴隸城市的原始王國
			Kingdom masterKingdom = master.kingdom;	// 主人王國 (新的歸屬王國)
			
			// 3. 核心邏輯檢查 (避免不必要的轉換)
			// 檢查目標城市和新王國是否已經是同一歸屬
			if (slaveCityKingdom == masterKingdom)
			{
				return true; // 城市已經在正確的王國下，不需轉換
			}
			
			// 4. 權限檢查 (可選但推薦)：確保奴隸有權力執行城市轉移
			// 這個Action通常應該綁定到奴隸擁有的特定特質上（例如：undead_servant3）
			// 這裡可以加入額外的檢查，例如：if (!slave.hasTrait("undead_servant_high_rank")) return false;
			
			// 5. 執行城市歸屬轉換
			// 將奴隸的城市轉移到主人的王國
			slaveCity.joinAnotherKingdom(masterKingdom);
			
			// 6. 確保奴隸單位歸屬同步 (joinAnotherKingdom 應會處理，但此處可加強確保)
			slave.kingdom = masterKingdom;
			
			// 播放效果（可選：模擬交接儀式）
			EffectsLibrary.spawnExplosionWave(slaveCity.getTile().posV3, 0.5f, 0.5f);
			
			return true;
		}
		public static bool teleportToHomeCity(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
		{// 轉移效果 返家
			// 1. 安全檢查：確保目標存在且存活
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
				return false;
			Actor targetActor = pTarget.a;
			// 2. 檢查目標是否有城市，並使用正確的方法檢查城市是否被摧毀
			if (!targetActor.hasCity() || targetActor.city == null) 
			{
				// 如果目標沒有城市或城市已被摧毀，則無法傳送回家
				return false;
			}
			// 3. 取得城市中心瓦片
			WorldTile tTile = targetActor.city.getTile(false);
			// 4. 再次進行瓦片安全檢查
			if (tTile == null)
			{
				return false; // 城市沒有有效的中心瓦片
			}
			if (targetActor.current_tile == tTile)
			{
				return false; // 目標已經在城市中心，不執行傳送
			}
			// 5. 執行傳送操作 
			ActionLibrary.teleportEffect(targetActor, tTile);
			targetActor.cancelAllBeh(); // 取消所有當前行為
			targetActor.spawnOn(tTile, 0f); // 傳送並生成
			return true; // 傳送成功
		}
		public static bool AddMoney(BaseSimObject pTarget, WorldTile pTile = null)
		{// 金錢增加
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor targetActor = pTarget.a;
			pTarget.a.data.money += 1000;
			pTarget.a.removeTrait("other001");
			return true;
		}
		public static bool AddLoot(BaseSimObject pTarget, WorldTile pTile = null)
		{// 掠奪品增加
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor targetActor = pTarget.a;
			pTarget.a.data.loot += 1000;
			pTarget.a.removeTrait("other002");
			return true;
		}
		public static bool addDemonKingStatus_Combined(BaseSimObject pTarget, float pDuration, string pMarkerID, WorldTile pTile = null)
		{// 魔神附體者 統一效果函式
			// 確保目標存在且是活著的角色
			if (pTarget?.a == null || !pTarget.a.isAlive())
			{
				return false;
			}
			Actor targetActor = pTarget.a;
			
			// 定義特質與魔王狀態的對應清單 (保持不變)
			var traits_to_add_status = new Dictionary<string, string>()
			{
				{"evillaw_devour", "envy_demon_king"},
				{"evillaw_starvation", "gluttony_demon_king"},
				{"evillaw_moneylaw", "greedy_demon_king"},
				{"evillaw_sleeping", "sloth_demon_king"},
				{"evillaw_tantrum", "wrath_demon_king"},
				{"evillaw_seduction", "lust_demon_king"},
				{"evillaw_ew", "arrogant_demon_king"}
			};
			
			bool statusAdded = false;
			
			// 遍歷所有對應關係
			foreach (var pair in traits_to_add_status)
			{
				string requiredTrait = pair.Key;
				string demonKingStatus = pair.Value;
				
				if (targetActor.hasTrait(requiredTrait) && !targetActor.hasStatus(demonKingStatus))
				{
					// 關鍵修正 1：使用傳入的 pDuration 作為持續時間
					targetActor.addStatusEffect(demonKingStatus, pDuration); 
					statusAdded = true;
					// Debug.Log($"[MyMod] Added status '{demonKingStatus}' to actor '{targetActor.data.name}' based on trait '{requiredTrait}'.");
				}
			}
			
			// 關鍵修正 2：使用傳入的 pMarkerID 作為標記 ID
			if (!targetActor.hasStatus(pMarkerID))
			{
				// 標記狀態的持續時間可以保持為 900f，或是也可以作為參數傳入
				targetActor.addStatusEffect(pMarkerID, 400f); 
			}
			
			return statusAdded;
		}

		public static bool Experience1(BaseSimObject pTarget, WorldTile pTile = null)
		{// 經驗值增加
			if (pTarget is Actor actor)
			{
				actor.data.experience += (int)999999999f;
				return true;
			}
			return false;
		}
		public static bool happiness00(BaseSimObject pTarget, WorldTile pTile = null)
		{// 幸福值孝正
		if (pTarget.a.data.happiness > 0)
			{
				pTarget.a.data.happiness = -100;
				return true;
				//總點數200點,最高100,最低-100
				//設定值 = 實際值
				//  100 = 100%
				//   50 =  75%
				//	1 =  50%
				//  -50 =  25%
				// -100 =   0%
			}
			return false;
		}
		public static bool RemoveMonster(BaseSimObject pTarget, WorldTile pTile)
		{// 移除特質模板，當目標擁有特定亞種特質且屬於一個城市時觸發
			// 1. 基本安全檢查：確保目標是有效的 Actor
			if (pTarget == null || pTarget.a == null || !pTarget.isActor())
			{
				return false;
			}
			Actor targetActor = pTarget.a;
			if (targetActor.subspecies == null)
			{
				return false;
			}
			// 2. 檢查觸發條件
			// 條件 A: 必須擁有指定的亞種特質 ("prefrontal_cortex")
			bool hasSubspeciesTrait = targetActor.subspecies.hasTrait("prefrontal_cortex");
			// 條件 B: 必須屬於一個城市 (city 不為 null)
			bool isAffiliatedWithCity = targetActor.city != null;
			// 3. 根據條件執行移除動作
			if (hasSubspeciesTrait && isAffiliatedWithCity)
			{
				// 只有在兩個條件都滿足時，才移除 "monster" 特質
				targetActor.removeTrait("monster");
				return true; // 成功觸發並移除
			}

			return false; // 不符合觸發條件，未執行移除
		}
		public static bool removeTraitXXX(BaseSimObject pTarget, WorldTile pTile)
		{// 添加 addTrait / 移除 removeTrait 特質 模板
			if (pTarget.a != null)
			{
			pTarget.a.removeTrait("crippled");
			pTarget.a.removeTrait("skin_burns");
			pTarget.a.removeTrait("eyepatch");
			pTarget.a.removeTrait("madness");
			}
			return true;
		}
		public static bool removeTraitXX(BaseSimObject pTarget, WorldTile pTile)
		{// 添加 / 移除特質 模板
			if (pTarget.a != null)
			{
			pTarget.a.removeTrait("crippled");
			pTarget.a.removeTrait("skin_burns");
			pTarget.a.removeTrait("eyepatch");
			}
			return true;
		}
		public static bool TraittAddRemoveXXX(BaseSimObject pTarget, WorldTile pTile)
		{// 亞種特質 添加 addTrait / 移除 removeTrait 模板
			if (pTarget.a != null)
			{
			pTarget.a.addTrait("");//一般特質
			pTarget.a.subspecies.addTrait("");//亞種特質
			pTarget.a.clan.addTrait("");//氏族特質
			pTarget.a.language.addTrait("");//言語特質
			pTarget.a.religion.addTrait("");//宗教特質
			pTarget.a.culture.addTrait("");//文明特質
			}
			return true;
		}
		public static bool DivineLight(BaseSimObject pTarget, WorldTile pTile = null)
		{// 聖光 調用效果
			if (pTarget == null || !pTarget.isActor() || !pTarget.a.isAlive())
				return false;
			Actor selfActor = pTarget.a;
			//if (selfActor.hasStatus("") || selfActor.hasStatus(""))
			//{
				int AURA_RADIUS1 = 0;	// 視覺效果大小,數字太高也不會變大,但會偏移
				int AURA_RADIUS2 = 30;	// 核心效果範圍,最高到30
				WorldTile centerTile = pTile ?? pTarget.current_tile;

				// 【靜態調用修正 1：視覺效果】
				World.world.loopWithBrush(centerTile,
				Brush.get(AURA_RADIUS1, "circ_"),
				new PowerActionWithID(Traits01Actions.divineLightFX), // 使用您的靜態類別名稱
				null);

				// 【靜態調用修正 2：核心效果】
				World.world.loopWithBrush(centerTile,
				Brush.get(AURA_RADIUS2, "circ_"),
				new PowerActionWithID(Traits01Actions.drawDivineLight), // 使用您的靜態類別名稱
				null);
				
				return true;
			//}
		}
		private static bool divineLightFX(WorldTile pCenterTile, string pPowerID)
		{// 聖光 特效函式
			World.world.fx_divine_light.playOn(pCenterTile);
			return true;
		}
		private static bool drawDivineLight(WorldTile pCenterTile, string pPowerID)
		{// 聖光 效果函式
			pCenterTile.doUnits(delegate(Actor pActor)
			{
				Traits01Actions.clearBadTraitsFrom(pActor);
				if (pActor.asset.can_be_killed_by_divine_light)
				{
					pActor.getHit((float)pActor.getMaxHealthPercent(0.5f), true, AttackType.Divine, null, true, false, true);
				}
				else
				{
					pActor.startColorEffect(ActorColorEffect.White);
				}
				pActor.finishStatusEffect("ash_fever");
				pActor.finishStatusEffect("cough");
				pActor.finishStatusEffect("cursed");
				pActor.finishAngryStatus();
				if (!pActor.isInLiquid())
				{
					pActor.cancelAllBeh();
				}
				if (pActor.hasPlot())
				{
					World.world.plots.cancelPlot(pActor.plot);
				}
			});
			return true;
		}
		private static void clearBadTraitsFrom(Actor pActor)
		{// 聖光 清理函式
			using (ListPool<ActorTrait> tTraitsToRemove = new ListPool<ActorTrait>())
			{
				foreach (ActorTrait tTrait in pActor.getTraits())
				{
					if (tTrait.can_be_removed_by_divine_light)
					{
						tTraitsToRemove.Add(tTrait);
					}
				}
				if (tTraitsToRemove.Count > 0)
				{
					pActor.removeTraits(tTraitsToRemove);
					pActor.setStatsDirty();
					pActor.changeHappiness("just_felt_the_divine", 0);
				}
			}
		}
			#endregion
	///////////////////////////////////////////////////////////////////////////////////////////

	}

}