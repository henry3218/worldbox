using NeoModLoader.api.attributes;
using NeoModLoader.General;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using UnityEngine;
using ReflectionUtility;
using HarmonyLib;
using ai;

namespace ZeN_01
{
    class StatusEffects_normal1
    { 
        public static void init()
        {
/*			//樣本
			var stabilize = new StatusAsset();
			stabilize.id = "stabilize";						//ID
			stabilize.duration = 60f;							//倒數計時
			stabilize.path_icon = "ui/icons/CDT/CDT000";					//效果圖標
			//動畫相關↓↓↓↓↓
			stabilize.animated = false;							//是否有動畫	true / false	非必要
			stabilize.is_animated_in_pause = false;							//能否暫停		true / false	非必要 遊戲暫停時，這個狀態效果的動畫是否繼續播放。
			stabilize.can_be_flipped = false;								//可以翻轉		true / false	非必要
			stabilize.use_parent_rotation = false;							//使用父級翻轉	true / false	非必要
			var test01Sprite = Resources.Load<Sprite>("effects/stabilize"); 			//資料夾位置					非必要
			stabilize.sprite_list = new Sprite[] { test01Sprite };			//動畫幀來源					非必要
			//動畫相關↑↑↑↑↑
			stabilize.locale_id = $"status_title_{stabilize.id}";					//區域設定ID					非必要
			stabilize.locale_description = $"status_description_{stabilize.id}";	//區域設定描述非必要			非必要
			stabilize.tier = StatusTier.Advanced;							//狀態層級			非必要
							 //Advanced	高級
							 //Basic 	基礎
			stabilize.removed_on_damage = false;							//損壞時移除	true / false	根據情況調整
			stabilize.opposite_status = new string[] { "", "" };				//對立
			stabilize.remove_status = new string[] { "", "" };					//移除,對下位狀態
			stabilize.base_stats = new BaseStats();							//添加數值
			addToLocalizedLibrary("ch",stabilize.id, "『中文名稱1』", "『中文描述1』");
			addToLocalizedLibrary("en",stabilize.id, "『ENname』", "『ENdescription』");
			AssetManager.status.add(stabilize);*/

		//職位狀態
			//國王特效1
			var king_effect1 = new StatusAsset();
			king_effect1.id = "king_effect1";						//ID
			king_effect1.duration = 3600f;							//倒數計時
			king_effect1.path_icon = "ui/icons/Effects1/001";						//效果圖標
			king_effect1.locale_id = $"status_title_{king_effect1.id}";			//區域設定ID					非必要
			king_effect1.locale_description = $"status_description_{king_effect1.id}";		//區域設定描述非必要			非必要
			king_effect1.tier = StatusTier.Advanced;							//狀態層級			非必要
			king_effect1.opposite_status = new string[] { "king_effect1", "king_effect2", "king_effect3", "king_effect4" };				//對立
			king_effect1.remove_status = new string[] { "leader_effect1", "leader_effect2", "leader_effect3", "leader_effect5" };
			king_effect1.base_stats = new BaseStats();							//添加數值
			king_effect1.base_stats.set("multiplier_lifespan", 0.50f);			//壽命 %
			king_effect1.base_stats.set("diplomacy", 799f);						//外交
			king_effect1.base_stats.set("warfare", 999f);						//軍事
			king_effect1.base_stats.set("stewardship", 799f);					//管理
			king_effect1.base_stats.set("intelligence", 799f);					//智力
			king_effect1.base_stats.set("birth_rate", 999f);					//生育機率
			king_effect1.base_stats.set("offspring", 100f);						//後代
			king_effect1.base_stats.set("multiplier_offspring", 0.20f);			//後代 %
			king_effect1.base_stats.set("cities", 100f);						//城市數量
			king_effect1.base_stats.set("personality_aggression", 9999f);					//攻擊型人格 [未顯示]
			addToLocalizedLibrary("ch",king_effect1.id, "國王軍事思維", "窮兵黷武的軍事之王");
			addToLocalizedLibrary("en",king_effect1.id, "Aggression", "The Warmongering Military King");
			AssetManager.status.add(king_effect1);

			//國王特效2
			var king_effect2 = new StatusAsset();
			king_effect2.id = "king_effect2";						//ID
			king_effect2.duration = 3600f;							//倒數計時
			king_effect2.path_icon = "ui/icons/Effects1/002";						//效果圖標
			king_effect2.locale_id = $"status_title_{king_effect2.id}";			//區域設定ID					非必要
			king_effect2.locale_description = $"status_description_{king_effect2.id}";		//區域設定描述非必要			非必要
			king_effect2.tier = StatusTier.Advanced;							//狀態層級			非必要
			king_effect2.opposite_status = new string[] { "king_effect1", "king_effect2", "king_effect3", "king_effect4" };//對立
			king_effect2.remove_status = new string[] { "leader_effect1", "leader_effect2", "leader_effect3", "leader_effect5" };
			king_effect2.base_stats = new BaseStats();							//添加數值
			king_effect2.base_stats.set("multiplier_lifespan", 0.50f);			//壽命 %
			king_effect2.base_stats.set("diplomacy", 799f);						//外交
			king_effect2.base_stats.set("warfare", 799f);						//軍事
			king_effect2.base_stats.set("stewardship", 999f);					//管理
			king_effect2.base_stats.set("intelligence", 799f);					//智力
			king_effect2.base_stats.set("birth_rate", 999f);					//生育機率
			king_effect2.base_stats.set("offspring", 100f);						//後代
			king_effect2.base_stats.set("multiplier_offspring", 0.20f);			//後代 %
			king_effect2.base_stats.set("cities", 100f);						//城市數量
			king_effect2.base_stats.set("personality_administration", 9999f);				//管理型人格 [未顯示]
			addToLocalizedLibrary("ch",king_effect2.id, "國王管理思維", "精於內政的管理之王");
			addToLocalizedLibrary("en",king_effect2.id, "Administration", "The king of management who is good at internal affairs");
			AssetManager.status.add(king_effect2);

			//國王特效3
			var king_effect3 = new StatusAsset();
			king_effect3.id = "king_effect3";						//ID
			king_effect3.duration = 3600f;							//倒數計時
			king_effect3.path_icon = "ui/icons/Effects1/003";						//效果圖標
			king_effect3.locale_id = $"status_title_{king_effect3.id}";			//區域設定ID					非必要
			king_effect3.locale_description = $"status_description_{king_effect3.id}";		//區域設定描述非必要			非必要
			king_effect3.tier = StatusTier.Advanced;							//狀態層級			非必要
			king_effect3.opposite_status = new string[] { "king_effect1", "king_effect2", "king_effect3", "king_effect4" };					//對立
			king_effect3.remove_status = new string[] { "leader_effect1", "leader_effect2", "leader_effect3", "leader_effect5" };				//對立
			king_effect3.base_stats = new BaseStats();							//添加數值
			king_effect3.base_stats.set("multiplier_lifespan", 0.50f);			//壽命 %
			king_effect3.base_stats.set("diplomacy", 999f);						//外交
			king_effect3.base_stats.set("warfare", 799f);						//軍事
			king_effect3.base_stats.set("stewardship", 799f);					//管理
			king_effect3.base_stats.set("intelligence", 799f);					//智力
			king_effect3.base_stats.set("birth_rate", 999f);					//生育機率
			king_effect3.base_stats.set("offspring", 100f);						//後代
			king_effect3.base_stats.set("multiplier_offspring", 0.20f);			//後代 %
			king_effect3.base_stats.set("cities", 100f);						//城市數量
			king_effect3.base_stats.set("personality_diplomatic", 9999f);					//外交型人格 [未顯示]
			addToLocalizedLibrary("ch",king_effect3.id, "國王外交思維", "能說會道的交際之王");
			addToLocalizedLibrary("en",king_effect3.id, "Diplomatic", "The eloquent king of communication.");
			AssetManager.status.add(king_effect3);

			//國王特效4
			var king_effect4 = new StatusAsset();
			king_effect4.id = "king_effect4";						//ID
			king_effect4.duration = 3600f;							//倒數計時
			king_effect4.path_icon = "ui/icons/Effects1/004";						//效果圖標
			king_effect4.locale_id = $"status_title_{king_effect4.id}";			//區域設定ID					非必要
			king_effect4.locale_description = $"status_description_{king_effect4.id}";		//區域設定描述非必要			非必要
			king_effect4.tier = StatusTier.Advanced;							//狀態層級			非必要
			king_effect4.opposite_status = new string[] { "king_effect1", "king_effect2", "king_effect3", "king_effect4" };				//對立
			king_effect4.remove_status = new string[] { "leader_effect1", "leader_effect2", "leader_effect3", "leader_effect5" };
			king_effect4.base_stats = new BaseStats();							//添加數值
			king_effect4.base_stats.set("multiplier_lifespan", 0.50f);			//壽命 %
			king_effect4.base_stats.set("diplomacy", 799f);						//外交
			king_effect4.base_stats.set("warfare", 799f);						//軍事
			king_effect4.base_stats.set("stewardship", 799f);					//管理
			king_effect4.base_stats.set("intelligence", 999f);					//智力
			king_effect4.base_stats.set("birth_rate", 999f);					//生育機率
			king_effect4.base_stats.set("offspring", 100f);						//後代
			king_effect4.base_stats.set("multiplier_offspring", 0.20f);			//後代 %
			king_effect4.base_stats.set("cities", 100f);						//城市數量
			king_effect4.base_stats.set("personality_rationality", 9999f);					//理性型人格 [未顯示]
			addToLocalizedLibrary("ch",king_effect4.id, "國王理性思維", "在理性思考後再採取行動的國王");
			addToLocalizedLibrary("en",king_effect4.id, "Rationality", "A king who thinks rationally before taking action");
			AssetManager.status.add(king_effect4);

			//國王特效5 正觀點
			var king_effect5 = new StatusAsset();
			king_effect5.id = "king_effect5";						//ID
			king_effect5.duration = 1800f;							//倒數計時
			king_effect5.path_icon = "ui/icons/Effects1/PositiveOpinion";						//效果圖標
			king_effect5.locale_id = $"status_title_{king_effect5.id}";			//區域設定ID					非必要
			king_effect5.locale_description = $"status_description_{king_effect5.id}";		//區域設定描述非必要			非必要
			king_effect5.tier = StatusTier.Advanced;							//狀態層級			非必要
			king_effect5.opposite_status = new string[] { "king_effect5", "king_effect6", "no_feeling01" };				//對立
			king_effect5.remove_status = new string[] { "leader_effect5", "leader_effect6", "no_feeling02" };	
			king_effect5.base_stats = new BaseStats();							//添加數值
			king_effect5.base_stats.set("opinion", 9999f);						//觀點
			addToLocalizedLibrary("ch",king_effect5.id, "正面意見", "對其他國家十分友好");
			addToLocalizedLibrary("en",king_effect5.id, "Positive Opinion", "Very friendly to other countries");
			AssetManager.status.add(king_effect5);

			//國王特效6 負觀點
			var king_effect6 = new StatusAsset();
			king_effect6.id = "king_effect6";						//ID
			king_effect6.duration = 1800f;							//倒數計時
			king_effect6.path_icon = "ui/icons/Effects1/NegativeOpinion";						//效果圖標
			king_effect6.locale_id = $"status_title_{king_effect6.id}";			//區域設定ID					非必要
			king_effect6.locale_description = $"status_description_{king_effect6.id}";		//區域設定描述非必要			非必要
			king_effect6.tier = StatusTier.Advanced;							//狀態層級			非必要
			king_effect6.opposite_status = new string[] { "king_effect5", "king_effect6", "no_feeling01" };				//對立
			king_effect6.remove_status = new string[] { "leader_effect5", "leader_effect6", "no_feeling02" };
			king_effect6.base_stats = new BaseStats();							//添加數值
			king_effect6.base_stats.set("opinion", -9999f);						//觀點
			addToLocalizedLibrary("ch",king_effect6.id, "負面意見", "討厭其他國家");
			addToLocalizedLibrary("en",king_effect6.id, "Negative Opinion", "Dislike for other countries");
			AssetManager.status.add(king_effect6);

			//無感
			var NoFeeling01 = new StatusAsset();
			NoFeeling01.id = "no_feeling01";						//ID
			NoFeeling01.duration = 1800f;							//倒數計時
			NoFeeling01.path_icon = "ui/icons/Effects1/NoFeeling";						//效果圖標
			NoFeeling01.locale_id = $"status_title_{NoFeeling01.id}";			//區域設定ID					非必要
			NoFeeling01.locale_description = $"status_description_{NoFeeling01.id}";		//區域設定描述非必要			非必要
			NoFeeling01.tier = StatusTier.Advanced;							//狀態層級			非必要
			NoFeeling01.opposite_status = new string[] { "king_effect5", "king_effect6", "no_feeling01" };				//對立
			NoFeeling01.remove_status = new string[] { "leader_effect5", "leader_effect6", "no_feeling02" };
			NoFeeling01.base_stats = new BaseStats();							//添加數值
			addToLocalizedLibrary("ch",NoFeeling01.id, "沒有意見", "");
			addToLocalizedLibrary("en",NoFeeling01.id, "No Feeling", "");
			AssetManager.status.add(NoFeeling01);

			//領主特效1
			var leader_effect1 = new StatusAsset();
			leader_effect1.id = "leader_effect1";						//ID
			leader_effect1.duration = 1800f;							//倒數計時
			leader_effect1.path_icon = "ui/icons/Effects1/001";						//效果圖標
			leader_effect1.locale_id = $"status_title_{leader_effect1.id}";			//區域設定ID					非必要
			leader_effect1.locale_description = $"status_description_{leader_effect1.id}";		//區域設定描述非必要			非必要
			leader_effect1.tier = StatusTier.Advanced;							//狀態層級			非必要
			leader_effect1.opposite_status = new string[] { "king_effect1", "king_effect2", "king_effect3", "king_effect4", "leader_effect1", "leader_effect2", "leader_effect3", "leader_effect4" };			//對立
			leader_effect1.base_stats = new BaseStats();							//添加數值
			leader_effect1.base_stats.set("multiplier_lifespan", 0.25f);			//壽命 %
			leader_effect1.base_stats.set("diplomacy", 300f);						//外交
			leader_effect1.base_stats.set("warfare", 350f);							//軍事
			leader_effect1.base_stats.set("stewardship", 300f);						//管理
			leader_effect1.base_stats.set("intelligence", 300f);					//智力
			leader_effect1.base_stats.set("birth_rate", 100f);						//生育機率
			leader_effect1.base_stats.set("offspring", 50f);						//後代
			leader_effect1.base_stats.set("multiplier_offspring", 0.10f);			//後代 %
			leader_effect1.base_stats.set("cities", 100f);							//城市數量
			leader_effect1.base_stats.set("bonus_towers", 100f);					//獎勵塔
			leader_effect1.base_stats.set("multiplier_supply_timer", -100f);		//乘法器供應定時器[未顯示]
			leader_effect1.base_stats.set("personality_aggression", 9999f);			//攻擊型人格 [未顯示]
			addToLocalizedLibrary("ch",leader_effect1.id, "領主軍政思維", "他現在重視村子的防衛能力");
			addToLocalizedLibrary("en",leader_effect1.id, "Aggression", "He now attaches importance to the village's defense capabilities.");
			AssetManager.status.add(leader_effect1);

			//領主特效2
			var leader_effect2 = new StatusAsset();
			leader_effect2.id = "leader_effect2";						//ID
			leader_effect2.duration = 1800f;							//倒數計時
			leader_effect2.path_icon = "ui/icons/Effects1/002";						//效果圖標
			leader_effect2.locale_id = $"status_title_{leader_effect2.id}";			//區域設定ID					非必要
			leader_effect2.locale_description = $"status_description_{leader_effect2.id}";		//區域設定描述非必要			非必要
			leader_effect2.tier = StatusTier.Advanced;							//狀態層級			非必要
			leader_effect2.opposite_status = new string[] { "king_effect1", "king_effect2", "king_effect3", "king_effect4", "leader_effect1", "leader_effect2", "leader_effect3", "leader_effect4" };			//對立
			leader_effect2.base_stats = new BaseStats();							//添加數值
			leader_effect2.base_stats.set("multiplier_lifespan", 0.25f);			//壽命 %
			leader_effect2.base_stats.set("diplomacy", 300f);						//外交
			leader_effect2.base_stats.set("warfare", 300f);							//軍事
			leader_effect2.base_stats.set("stewardship", 350f);						//管理
			leader_effect2.base_stats.set("intelligence", 300f);					//智力
			leader_effect2.base_stats.set("birth_rate", 100f);						//生育機率
			leader_effect2.base_stats.set("offspring", 50f);						//後代
			leader_effect2.base_stats.set("multiplier_offspring", 0.10f);			//後代 %
			leader_effect2.base_stats.set("cities", 100f);							//城市數量
			leader_effect2.base_stats.set("bonus_towers", 100f);					//獎勵塔
			leader_effect2.base_stats.set("multiplier_supply_timer", -100f);		//乘法器供應定時器[未顯示]
			leader_effect2.base_stats.set("personality_administration", 9999f);		//管理型人格 [未顯示]
			addToLocalizedLibrary("ch",leader_effect2.id, "領主管理思維", "他現在重視村莊管理");
			addToLocalizedLibrary("en",leader_effect2.id, "Administration", "He now pays attention to village management.");
			AssetManager.status.add(leader_effect2);

			//領主特效3
			var leader_effect3 = new StatusAsset();
			leader_effect3.id = "leader_effect3";						//ID
			leader_effect3.duration = 1800f;							//倒數計時
			leader_effect3.path_icon = "ui/icons/Effects1/003";						//效果圖標
			leader_effect3.locale_id = $"status_title_{leader_effect3.id}";			//區域設定ID					非必要
			leader_effect3.locale_description = $"status_description_{leader_effect3.id}";		//區域設定描述非必要			非必要
			leader_effect3.tier = StatusTier.Advanced;							//狀態層級			非必要
			leader_effect3.opposite_status = new string[] { "king_effect1", "king_effect2", "king_effect3", "king_effect4", "leader_effect1", "leader_effect2", "leader_effect3", "leader_effect4" };			//對立
			leader_effect3.base_stats = new BaseStats();							//添加數值
			leader_effect3.base_stats.set("multiplier_lifespan", 0.25f);			//壽命 %
			leader_effect3.base_stats.set("diplomacy", 350f);						//外交
			leader_effect3.base_stats.set("warfare", 300f);							//軍事
			leader_effect3.base_stats.set("stewardship", 300f);						//管理
			leader_effect3.base_stats.set("intelligence", 300f);					//智力
			leader_effect3.base_stats.set("birth_rate", 100f);						//生育機率
			leader_effect3.base_stats.set("offspring", 50f);						//後代
			leader_effect3.base_stats.set("multiplier_offspring", 0.10f);			//後代 %
			leader_effect3.base_stats.set("cities", 100f);							//城市數量
			leader_effect3.base_stats.set("bonus_towers", 100f);					//獎勵塔
			leader_effect3.base_stats.set("multiplier_supply_timer", -100f);		//乘法器供應定時器[未顯示]
			leader_effect3.base_stats.set("personality_diplomatic", 9999f);			//外交型人格 [未顯示]
			addToLocalizedLibrary("ch",leader_effect3.id, "領主外交思維", "他對其他村莊都很友善");
			addToLocalizedLibrary("en",leader_effect3.id, "Diplomatic", "He is very friendly to other countries.");
			AssetManager.status.add(leader_effect3);

			//領主特效4
			var leader_effect4 = new StatusAsset();
			leader_effect4.id = "leader_effect4";						//ID
			leader_effect4.duration = 1800f;							//倒數計時
			leader_effect4.path_icon = "ui/icons/Effects1/005";						//效果圖標
			leader_effect4.locale_id = $"status_title_{leader_effect4.id}";			//區域設定ID					非必要
			leader_effect4.locale_description = $"status_description_{leader_effect4.id}";		//區域設定描述非必要			非必要
			leader_effect4.tier = StatusTier.Advanced;							//狀態層級			非必要
			leader_effect4.opposite_status = new string[] { "king_effect1", "king_effect2", "king_effect3", "king_effect4", "leader_effect1", "leader_effect2", "leader_effect3", "leader_effect4" };			//對立
			leader_effect4.base_stats = new BaseStats();							//添加數值
			leader_effect4.base_stats.set("multiplier_lifespan", 0.25f);			//壽命 %
			leader_effect4.base_stats.set("diplomacy", 300f);						//外交
			leader_effect4.base_stats.set("warfare", 300f);							//軍事
			leader_effect4.base_stats.set("stewardship", 300f);						//管理
			leader_effect4.base_stats.set("intelligence", 350f);					//智力
			leader_effect4.base_stats.set("birth_rate", 100f);						//生育機率
			leader_effect4.base_stats.set("offspring", 50f);						//後代
			leader_effect4.base_stats.set("multiplier_offspring", 0.10f);			//後代 %
			leader_effect4.base_stats.set("cities", 100f);							//城市數量
			leader_effect4.base_stats.set("bonus_towers", 100f);					//獎勵塔
			leader_effect4.base_stats.set("multiplier_supply_timer", -100f);					//乘法器供應定時器[未顯示]
			leader_effect4.base_stats.set("personality_rationality", 9999f);					//理性型人格 [未顯示]
			addToLocalizedLibrary("ch",leader_effect4.id, "領主理性思維", "他總是理性的看待局勢");
			addToLocalizedLibrary("en",leader_effect4.id, "Rationality", "He always looks at the situation rationally.");
			AssetManager.status.add(leader_effect4);

			//領主特效5 忠誠
			var leader_effect5 = new StatusAsset();
			leader_effect5.id = "leader_effect5";						//ID
			leader_effect5.duration = 900f;							//倒數計時
			leader_effect5.path_icon = "ui/icons/Effects1/Loyalty";						//效果圖標
			leader_effect5.locale_id = $"status_title_{leader_effect5.id}";			//區域設定ID					非必要
			leader_effect5.locale_description = $"status_description_{leader_effect5.id}";		//區域設定描述非必要			非必要
			leader_effect5.tier = StatusTier.Advanced;							//狀態層級			非必要
			leader_effect5.opposite_status = new string[] { "king_effect5", "king_effect6", "no_feeling01", "leader_effect5", "leader_effect6", "no_feeling02" };	//對立
			leader_effect5.base_stats = new BaseStats();							//添加數值
			leader_effect5.base_stats.set("loyalty_traits", 4999.5f);							//忠誠特質
			leader_effect5.base_stats.set("loyalty_mood", 4999.5f);							//忠誠情緒
			addToLocalizedLibrary("ch",leader_effect5.id, "忠誠", "他對國家獻上了忠誠");
			addToLocalizedLibrary("en",leader_effect5.id, "Loyalty", "He was loyal to his country");
			AssetManager.status.add(leader_effect5);

			//領主特效5 不忠
			var leader_effect6 = new StatusAsset();
			leader_effect6.id = "leader_effect6";						//ID
			leader_effect6.duration = 900f;							//倒數計時
			leader_effect6.path_icon = "ui/icons/Effects1/LoyaltyAnit";						//效果圖標
			leader_effect6.locale_id = $"status_title_{leader_effect6.id}";			//區域設定ID					非必要
			leader_effect6.locale_description = $"status_description_{leader_effect6.id}";		//區域設定描述非必要			非必要
			leader_effect6.tier = StatusTier.Advanced;							//狀態層級			非必要
			leader_effect6.opposite_status = new string[] { "king_effect5", "king_effect6", "no_feeling01", "leader_effect5", "leader_effect6", "no_feeling02" };	//對立
			leader_effect6.base_stats = new BaseStats();							//添加數值
			leader_effect6.base_stats.set("loyalty_traits", -4999.5f);							//忠誠特質
			leader_effect6.base_stats.set("loyalty_mood", -4999.5f);							//忠誠情緒
			addToLocalizedLibrary("ch",leader_effect6.id, "不忠", "待時機一到他將與國家劃清界線自立門戶");
			addToLocalizedLibrary("en",leader_effect6.id, "Disloyalty", "When the time comes, he will draw a clear line with the country and establish his own");
			AssetManager.status.add(leader_effect6);

			//無感 02
			var NoFeeling02 = new StatusAsset();
			NoFeeling02.id = "no_feeling02";						//ID
			NoFeeling02.duration = 900f;							//倒數計時
			NoFeeling02.path_icon = "ui/icons/Effects1/NoFeeling";						//效果圖標
			NoFeeling02.locale_id = $"status_title_{NoFeeling02.id}";			//區域設定ID					非必要
			NoFeeling02.locale_description = $"status_description_{NoFeeling02.id}";		//區域設定描述非必要			非必要
			NoFeeling02.tier = StatusTier.Advanced;							//狀態層級			非必要
			NoFeeling02.opposite_status = new string[] { "king_effect5", "king_effect6", "no_feeling01", "leader_effect5", "leader_effect6", "no_feeling02" };	//對立
			NoFeeling02.base_stats = new BaseStats();							//添加數值
			addToLocalizedLibrary("ch",NoFeeling02.id, "無感", "");
			addToLocalizedLibrary("en",NoFeeling02.id, "No Feeling", "");
			AssetManager.status.add(NoFeeling02);



		//狀態抗性

			//抗體 對減益狀態
			var antibody = new StatusAsset();
			antibody.id = "antibody";						//ID
			antibody.duration = 60f;							//倒數計時
			antibody.path_icon = "ui/icons/Skill/Skill0301";				//效果圖標
/*			//動畫相關↓↓↓↓↓
			antibody.animated = true;							//是否有動畫	true / false	非必要
			antibody.is_animated_in_pause = false;							//能否暫停		true / false	非必要 遊戲暫停時，這個狀態效果的動畫是否繼續播放。
			antibody.can_be_flipped = false;								//可以翻轉		true / false	非必要
			antibody.use_parent_rotation = false;							//使用父級翻轉	true / false	非必要
			var test01Sprite = Resources.Load<Sprite>("effects/antibody"); 			//資料夾位置					非必要
			antibody.sprite_list = new Sprite[] { test01Sprite };			//動畫幀來源					非必要
			//動畫相關↑↑↑↑↑*/
			antibody.locale_id = $"status_title_{antibody.id}";					//區域設定ID					非必要
			antibody.locale_description = $"status_description_{antibody.id}";	//區域設定描述非必要			非必要
			antibody.tier = StatusTier.Basic;							//狀態層級
			antibody.opposite_status = new string[] { "weaken", "" };				//對立
			antibody.remove_status = new string[] { "slowness", "cough", "ash_fever", "frozen", "burning", "poisoned", "stunned", "sleeping", "spell_silence", "drowning", "confused" };
			antibody.base_stats = new BaseStats();							//添加數值
			addToLocalizedLibrary("ch",antibody.id, "抗體", "完全免疫負面狀態");
			addToLocalizedLibrary("en",antibody.id, "Antibody", "Completely immune to negative status");
			AssetManager.status.add(antibody);

			//寧靜 對減益狀態
			var serenity = new StatusAsset();
			serenity.id = "serenity";						//ID
			serenity.duration = 600f;							//倒數計時
			serenity.path_icon = "ui/icons/Skill/Skill0312";				//效果圖標
/*			//動畫相關↓↓↓↓↓
			serenity.animated = true;							//是否有動畫	true / false	非必要
			serenity.is_animated_in_pause = false;							//能否暫停		true / false	非必要 遊戲暫停時，這個狀態效果的動畫是否繼續播放。
			serenity.can_be_flipped = false;								//可以翻轉		true / false	非必要
			serenity.use_parent_rotation = false;							//使用父級翻轉	true / false	非必要
			var test01Sprite = Resources.Load<Sprite>("effects/serenity"); 			//資料夾位置					非必要
			serenity.sprite_list = new Sprite[] { test01Sprite };			//動畫幀來源					非必要
			//動畫相關↑↑↑↑↑*/
			serenity.locale_id = $"status_title_{serenity.id}";					//區域設定ID					非必要
			serenity.locale_description = $"status_description_{serenity.id}";	//區域設定描述非必要			非必要
			serenity.tier = StatusTier.Basic;							//狀態層級
			serenity.remove_status = new string[] { "tantrum", "angry" };
			serenity.base_stats = new BaseStats();							//添加數值
			addToLocalizedLibrary("ch",serenity.id, "寧靜", "暫時不會陷入忿怒之中");
			addToLocalizedLibrary("en",serenity.id, "serenity", "Don't fall into anger for the time being");
			AssetManager.status.add(serenity);

			//衰弱 對增益狀態
			var weaken = new StatusAsset();
			weaken.id = "weaken";						//ID
			weaken.duration = 60f;							//倒數計時
			weaken.path_icon = "ui/icons/Effects1/weaken";				//效果圖標↑↑↑↑↑*/
			weaken.locale_id = $"status_title_{weaken.id}";					//區域設定ID					非必要
			weaken.locale_description = $"status_description_{weaken.id}";	//區域設定描述非必要			非必要
			weaken.tier = StatusTier.Basic;							//狀態層級 
			weaken.opposite_status = new string[] { "antibody", "arrogant_demon_king", "greedy_demon_king", "lust_demon_king", "wrath_demon_king", "gluttony_demon_king", "sloth_demon_king", "envy_demon_king", "darkblessing", "ex_undead_emperor" };				//對立
			weaken.remove_status = new string[] { "shield", "powerup", "caffeinated", "enchanted", "rage", "invincible", "spell_boost", "motivated", "inspired" };
			weaken.base_stats = new BaseStats();
			weaken.base_stats.set("multiplier_damage", -0.50f);			//傷害 %
			weaken.base_stats.set("armor", -50f);						//防禦
			weaken.base_stats.set("multiplier_speed", -0.50f);			//移動速度 %
			weaken.base_stats.set("multiplier_attack_speed", -0.50f);				//攻擊速度
			weaken.base_stats.set("multiplier_mass", -0.50f);			//質量 %[未顯示]
			addToLocalizedLibrary("ch",weaken.id, "衰弱", "體況不佳");
			addToLocalizedLibrary("en",weaken.id, "Weaken", "Poor physical condition");
			AssetManager.status.add(weaken);

			//絕育
			var sterilization = new StatusAsset();
			sterilization.id = "sterilization";						//ID
			sterilization.duration = 600f;							//倒數計時
			sterilization.path_icon = "ui/icons/Skill/Skill0409";
			sterilization.locale_id = $"status_title_{sterilization.id}";					//區域設定ID					非必要
			sterilization.locale_description = $"status_description_{sterilization.id}";	//區域設定描述非必要			非必要
			sterilization.tier = StatusTier.Basic;							//狀態層級
			sterilization.base_stats = new BaseStats();
			sterilization.remove_status = new string[] { "pregnant", "budding", "taking_roots", "soul_harvested" };
			sterilization.opposite_status = new string[] { "egg", "uprooting" };
			sterilization.base_stats = new BaseStats();							//添加數值
			addToLocalizedLibrary("ch",sterilization.id, "絕育", "無法生育新的生命");
			addToLocalizedLibrary("en",sterilization.id, "sterilization", "Unable to have new children");
			AssetManager.status.add(sterilization);

		//戰技狀態
			//堅守狀態
			var stabilize = new StatusAsset();
			stabilize.id = "stabilize";						//ID
			stabilize.duration = 10f;							//倒數計時
			stabilize.path_icon = "ui/Icons/Skill/Skill0001";					//效果圖標
			stabilize.locale_id = $"status_title_{stabilize.id}";					//區域設定ID					非必要
			stabilize.locale_description = $"status_description_{stabilize.id}";	//區域設定描述非必要			非必要
			stabilize.tier = StatusTier.Advanced;							//狀態層級			非必要
			stabilize.removed_on_damage = false;							//損壞時移除	true / false	根據情況調整
			stabilize.base_stats = new BaseStats();	
			stabilize.base_stats.addTag("immovable");
			stabilize.base_stats.set("armor", 9999f);						//防禦
			stabilize.base_stats.set("mass", 999999999f);							//質量
			stabilize.base_stats.set("multiplier_mass", 999999999f);			//質量 %[未顯示]					//添加數值
			addToLocalizedLibrary("ch",stabilize.id, "堅若磐石", "堅守不退");
			addToLocalizedLibrary("en",stabilize.id, "stabilize", "Stand firm");
			AssetManager.status.add(stabilize);

			//猛擊狀態
			var fullpower = new StatusAsset();
			fullpower.id = "fullpower";						//ID
			fullpower.duration = 3f;							//倒數計時
			fullpower.path_icon = "ui/Icons/Skill/Skill0002";					//效果圖標
			fullpower.locale_id = $"status_title_{fullpower.id}";					//區域設定ID					非必要
			fullpower.locale_description = $"status_description_{fullpower.id}";	//區域設定描述非必要			非必要
			fullpower.tier = StatusTier.Advanced;							//狀態層級			非必要
			fullpower.removed_on_damage = false;							//損壞時移除	true / false	根據情況調整
			fullpower.base_stats = new BaseStats();	
			fullpower.base_stats.set("multiplier_damage", 100f);			//傷害 %
			fullpower.base_stats.set("multiplier_crit", 100f);			//爆擊機率 %
			fullpower.base_stats.set("critical_damage_multiplier", 100f);			//重擊 %	
			fullpower.base_stats.set("knockback", 1.00f);				//擊退 % [未顯示]
			addToLocalizedLibrary("ch",fullpower.id, "全力一擊", "這一下會很痛");
			addToLocalizedLibrary("en",fullpower.id, "Full Strength Attack", "This is gonna hurt.");
			AssetManager.status.add(fullpower);

			//彈雨狀態
			var bulletrain = new StatusAsset();
			bulletrain.id = "bulletrain";						//ID
			bulletrain.duration = 3f;							//倒數計時
			bulletrain.path_icon = "ui/Icons/Skill/Skill0003";					//效果圖標
			bulletrain.locale_id = $"status_title_{bulletrain.id}";					//區域設定ID					非必要
			bulletrain.locale_description = $"status_description_{bulletrain.id}";	//區域設定描述非必要			非必要
			bulletrain.tier = StatusTier.Advanced;							//狀態層級			非必要
			bulletrain.removed_on_damage = false;							//損壞時移除	true / false	根據情況調整
			bulletrain.base_stats = new BaseStats();				//添加數值
			bulletrain.base_stats.set("projectiles", 20f);					//子彈 [未顯示]
			addToLocalizedLibrary("ch",bulletrain.id, "彈雨", "如同潑出去的水一般");
			addToLocalizedLibrary("en",bulletrain.id, "Bullet Rain", "Like spilled water.");
			AssetManager.status.add(bulletrain);

			//狙擊狀態
			var Crosshair = new StatusAsset();
			Crosshair.id = "crosshair";						//ID
			Crosshair.duration = 3f;							//倒數計時
			Crosshair.path_icon = "ui/Icons/Effects1/Crosshair";					//效果圖標
			Crosshair.locale_id = $"status_title_{Crosshair.id}";					//區域設定ID
			Crosshair.locale_description = $"status_description_{Crosshair.id}";	//區域設定描述
			Crosshair.tier = StatusTier.Basic;							//狀態層級			非必要
			Crosshair.removed_on_damage = false;							//損壞時移除	true / false	根據情況調整
			Crosshair.base_stats = new BaseStats();					//添加數值
			Crosshair.base_stats.set("range", 20f);					//
			Crosshair.base_stats.set("throwing_range", 5f);									//精確度
			addToLocalizedLibrary("ch",Crosshair.id, "射程上升", "他可以攻擊更遠的地方");
			addToLocalizedLibrary("en",Crosshair.id, "Range Up", "He can attack farther away.");
			AssetManager.status.add(Crosshair);

			//戰鬥狀態
			var Fighting = new StatusAsset();
			Fighting.id = "fighting";						//ID
			Fighting.duration = 3f;							//倒數計時
			Fighting.path_icon = "ui/Icons/Effects1/Fighting";					//效果圖標
			Fighting.locale_id = $"status_title_{Fighting.id}";					//區域設定ID
			Fighting.locale_description = $"status_description_{Fighting.id}";	//區域設定描述
			Fighting.tier = StatusTier.Basic;							//狀態層級			非必要
			Fighting.removed_on_damage = false;							//損壞時移除	true / false	根據情況調整
			Fighting.base_stats = new BaseStats();					//添加數值
			addToLocalizedLibrary("ch",Fighting.id, "戰鬥狀態", "");
			addToLocalizedLibrary("en",Fighting.id, "fighting", "");
			AssetManager.status.add(Fighting);
		}
		public static void addToLocalizedLibrary(string planguage, string id, string name, string description)
		{// 新增到本地化資料庫 (language)
			string language = Reflection.GetField(LocalizedTextManager.instance.GetType(), LocalizedTextManager.instance, "language") as string;
			string templanguage;
			templanguage = language;
			if (templanguage != "ch" && templanguage != "en")
			{
				templanguage = "en";
			}
			if (planguage == templanguage)
			{
				LocalizedTextManager.add("status_title_" + id, name);
				LocalizedTextManager.add("status_description_" + id, description);
			}
		}
        public static Sprite[] getStatusSprites(string id)
        {//精靈獲取狀態精靈
			var sprite = Resources.Load<Sprite>("effects/" + id);
			if (sprite != null)
			    return new Sprite[] { sprite };
			else
			{
			    Debug.LogError($"ZeN_TEST: Can not find status sprite for ID: {id}. Please ensure the image is in GameResources/effects/ directory.");
			    return Array.Empty<Sprite>();
			}
        }
    }
}