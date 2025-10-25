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
	class StatusEffects_normal3
	{ 
		private static readonly float maxDuration = 99999f;
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
			stabilize.tier = StatusTier.Basic;							//狀態層級			非必要
							 //Basic	高級
							 //Basic 	基礎
			stabilize.removed_on_damage = false;							//損壞時移除	true / false	根據情況調整
			stabilize.opposite_status = new string[] { "", "" };				//對立
			stabilize.remove_status = new string[] { "", "" };					//移除,對下位狀態
			stabilize.base_stats = new BaseStats();							//添加數值
			addToLocalizedLibrary("ch",stabilize.id, "『中文名稱1』", "『中文描述1』");
			addToLocalizedLibrary("en",stabilize.id, "『ENname』", "『ENdescription』");
			AssetManager.status.add(stabilize);*/
			#region 傲慢 arrogant_demon_king
		//滅智法專用 Arrogant Demon King
			var ADK = new StatusAsset();
			ADK.id = "arrogant_demon_king";								 //ID
			ADK.duration = 3600f;									 //倒數計時
			ADK.path_icon = "ui/icons/Effects1/demon_king/arrogant_demon_king";				//效果圖標
			ADK.locale_id = $"status_title_{ADK.id}";		//區域設定ID					非必要
			ADK.locale_description = $"status_description_{ADK.id}";		//區域設定描述非必要			非必要
			ADK.tier = StatusTier.Basic;
			ADK.base_stats = new BaseStats();
			ADK.base_stats.addTag("immunity_fire");
			ADK.base_stats.set("personality_aggression", 9999f);					//攻擊型人格 [未顯示]
			ADK.remove_status = new string[] { "cursed", "weaken", "frozen", "slowness", "burning", "stunned", "cdt_adk", "cdt_adk01" };								//移除,對下位狀態
			ADK.action_on_receive = (WorldAction)Delegate.Combine(ADK.action_on_receive, new WorldAction(addOther6661));
			ADK.action_finish = (WorldAction)Delegate.Combine(ADK.action_finish, new WorldAction(removeOther6669));
			addToLocalizedLibrary("ch",ADK.id, "傲慢魔王", "他的敵人已經淪為野獸。");
			addToLocalizedLibrary("en",ADK.id, "Arrogant Demon King", "Those who are his enemies have been reduced to the status of wild beasts.");
			AssetManager.status.add(ADK);

			var ADK2 = new StatusAsset();
			ADK2.id = "adk2";								 //ID
			ADK2.duration = 60f;									 //倒數計時
			ADK2.path_icon = "ui/icons/items/evilsword";				//效果圖標
			ADK2.locale_id = $"status_title_{ADK2.id}";		//區域設定ID					非必要
			ADK2.locale_description = $"status_description_{ADK2.id}";		//區域設定描述非必要
			ADK2.tier = StatusTier.Basic;
			ADK2.base_stats = new BaseStats();
			ADK2.base_stats.set("damage_range", 50f);								//傷害區間 [未顯示]
			ADK2.base_stats.set("armor", 25f);										//防禦
			ADK2.base_stats.set("multiplier_speed", 0.75f);							//移動速度 %
			ADK2.base_stats.set("multiplier_attack_speed", 0.75f);					//攻擊速度 %
			ADK2.base_stats.set("critical_chance", 0.75f);							//爆擊機率 %
			ADK2.base_stats.set("critical_damage_multiplier", 0.75f);				//重擊 %
			ADK2.base_stats.set("range", 18f);										//範圍 %
			ADK2.base_stats.set("projectiles", 15f);								//子彈 [未顯示]
			addToLocalizedLibrary("ch",ADK2.id, "魔劍甦醒", "魔劍之力開始甦醒");
			addToLocalizedLibrary("en",ADK2.id, "Evil Sword Awakens", "The power of the evil sword begins to awaken.");
			AssetManager.status.add(ADK2);

			var ADK3 = new StatusAsset();
			ADK3.id = "adk3";								 //ID
			ADK3.duration = 60f;									 //倒數計時
			ADK3.path_icon = "ui/icons/Effects1/eye";				//效果圖標
			ADK3.locale_id = $"status_title_{ADK3.id}";		//區域設定ID					非必要
			ADK3.locale_description = $"status_description_{ADK3.id}";		//區域設定描述非必要
			ADK3.tier = StatusTier.Basic;
			ADK3.opposite_traits = AssetLibrary<StatusAsset>.a<string>(new string[]
			{
				"evillaw_ew", "evillaw_seduction", "evillaw_moneylaw", "evillaw_starvation", "evillaw_sleeping", "evillaw_tantrum", "evillaw_devour", "holyarts_justice"
			});
			ADK3.action_on_receive = (WorldAction)Delegate.Combine(ADK3.action_on_receive, new WorldAction(addFavorite1));
			ADK3.action_finish = (WorldAction)Delegate.Combine(ADK3.action_finish, new WorldAction(EvilLawGet_StatusEffects01));
			addToLocalizedLibrary("ch",ADK3.id, "魔神凝視", "他正在審視你是否夠資格…");
			addToLocalizedLibrary("en",ADK3.id, "Evil God Gaze", "He is examining whether you are qualified...");
			AssetManager.status.add(ADK3);

			var cdt_adk = new StatusAsset();
			cdt_adk.id = "cdt_adk";								 //ID
			cdt_adk.duration = 3600f;									 //倒數計時
			cdt_adk.path_icon = "ui/icons/CDT/CDT000";				//效果圖標
			cdt_adk.locale_id = $"status_title_{cdt_adk.id}";		//區域設定ID					非必要
			cdt_adk.locale_description = $"status_description_{cdt_adk.id}";		//區域設定描述非必要			非必要
			cdt_adk.tier = StatusTier.Basic;
			cdt_adk.opposite_status = new string[] { "arrogant_demon_king", "" };				//對立
			cdt_adk.remove_status = new string[] { "" };
			addToLocalizedLibrary("ch",cdt_adk.id, "滅智冷卻", "因為有其他魔王，因此無法獲得魔王狀態，距離次回發動效果還需要…");
			addToLocalizedLibrary("en",cdt_adk.id, "Atk CD", "Because of the other demon kings, I cannot obtain the demon king status. It will take more time to activate the effect again...");
			AssetManager.status.add(cdt_adk);

			var cdt_adk01 = new StatusAsset();
			cdt_adk01.id = "cdt_adk01";								 //ID
			cdt_adk01.duration = 3600f;									 //倒數計時
			cdt_adk01.path_icon = "ui/icons/CDT/CDT000";				//效果圖標
			cdt_adk01.locale_id = $"status_title_{cdt_adk01.id}";		//區域設定ID					非必要
			cdt_adk01.locale_description = $"status_description_{cdt_adk01.id}";		//區域設定描述非必要			非必要
			cdt_adk01.tier = StatusTier.Basic;
			cdt_adk01.opposite_status = new string[] { "arrogant_demon_king", "" };				//對立
			cdt_adk01.remove_status = new string[] { "" };
			cdt_adk01.action_finish = (WorldAction)Delegate.Combine(ADK3.action_finish, new WorldAction(cdt_adk01Finish));
			addToLocalizedLibrary("ch",cdt_adk01.id, "靜待時機", "距離為時機成熟還需要…");
			addToLocalizedLibrary("en",cdt_adk01.id, "Waiting for the right time", "It will take some time until the time comes......");
			AssetManager.status.add(cdt_adk01);
			#endregion
			#region 強慾 greedy_demon_king
		//金錢法專用 Greedy Demon King
			var GDK = new StatusAsset();
			GDK.id = "greedy_demon_king";												//ID
			GDK.duration = 3600f;													//倒數計時
			GDK.path_icon = "ui/icons/Effects1/demon_king/greedy_demon_king";								//效果圖標
			GDK.locale_id = $"status_title_{GDK.id}";					//區域設定ID					非必要
			GDK.locale_description = $"status_description_{GDK.id}";	//區域設定描述非必要			非必要
			GDK.tier = StatusTier.Basic;
			GDK.opposite_status = new string[] { "", "" };				//對立
			GDK.remove_status = new string[] { "cursed", "weaken", "sleeping", "stunned", "burning", "frozen", };
			GDK.base_stats = new BaseStats();
			GDK.base_stats.set("personality_aggression", 9999f);
			GDK.action_on_receive = (WorldAction)Delegate.Combine(GDK.action_on_receive, new WorldAction(addOther6661));
			GDK.action_finish = (WorldAction)Delegate.Combine(GDK.action_finish, new WorldAction(removeOther6669));
			addToLocalizedLibrary("ch",GDK.id, "強欲魔王", "財富的積累得到了魔神認可");
			addToLocalizedLibrary("en",GDK.id, "Greedy Demon King", "The accumulation of wealth was recognized by the demon god");
			AssetManager.status.add(GDK);

			var GDK2 = new StatusAsset();
			GDK2.id = "gdk2";								 //ID
			GDK2.duration = 60f;									 //倒數計時
			GDK2.path_icon = "ui/icons/Effects1/eye";				//效果圖標
			GDK2.locale_id = $"status_title_{GDK2.id}";		//區域設定ID					非必要
			GDK2.locale_description = $"status_description_{GDK2.id}";		//區域設定描述非必要
			GDK2.tier = StatusTier.Basic;
			GDK2.opposite_traits = AssetLibrary<StatusAsset>.a<string>(new string[]
			{
				"evillaw_ew", "evillaw_seduction", "evillaw_moneylaw", "evillaw_starvation", "evillaw_sleeping", "evillaw_tantrum", "evillaw_devour", "holyarts_justice"
			});
			GDK2.action_on_receive = (WorldAction)Delegate.Combine(GDK2.action_on_receive, new WorldAction(addFavorite1));
			GDK2.action_finish = (WorldAction)Delegate.Combine(GDK2.action_finish, new WorldAction(EvilLawGet_StatusEffects02));
			addToLocalizedLibrary("ch",GDK2.id, "魔神凝視", "他正在審視你是否夠資格…");
			addToLocalizedLibrary("en",GDK2.id, "Evil God Gaze", "He is examining whether you are qualified...");
			AssetManager.status.add(GDK2);

			var GDK3 = new StatusAsset();
			GDK3.id = "gdk3";								 //ID
			GDK3.duration = 60f;									 //倒數計時
			GDK3.path_icon = "ui/icons/items/evilgun";				//效果圖標
			GDK3.locale_id = $"status_title_{GDK3.id}";		//區域設定ID					非必要
			GDK3.locale_description = $"status_description_{GDK3.id}";		//區域設定描述非必要
			GDK3.tier = StatusTier.Basic;
			GDK3.base_stats = new BaseStats();
			GDK3.base_stats.set("damage_range", 10f);
			GDK3.base_stats.set("multiplier_attack_speed", 999f);
			GDK3.base_stats.set("range", 30f);
			GDK3.base_stats.set("projectiles", 10f);
			GDK3.base_stats.set("targets", 99f);									//目標 [未顯示]
			addToLocalizedLibrary("ch",GDK3.id, "魔銃甦醒", "魔銃之力開始運作");
			addToLocalizedLibrary("en",GDK3.id, "Evil Gun Awakens", "The power of the evil gun begins to awaken");
			AssetManager.status.add(GDK3);

			var goldcooldown = new StatusAsset();
			goldcooldown.id = "goldcooldown";								 //ID
			goldcooldown.duration = 60f;									 //倒數計時
			goldcooldown.path_icon = "ui/icons/Effects1/coin";				//效果圖標
			goldcooldown.locale_id = $"status_title_{goldcooldown.id}";		//區域設定ID					非必要
			goldcooldown.locale_description = $"status_description_{goldcooldown.id}";		//區域設定描述非必要			非必要
			goldcooldown.tier = StatusTier.Basic;
			goldcooldown.action_finish = (WorldAction)Delegate.Combine(goldcooldown.action_finish, new WorldAction(goldcooldown01));
			addToLocalizedLibrary("ch",goldcooldown.id, "天降財富", "剛剛獲得了邪神的贊助金");
			addToLocalizedLibrary("en",goldcooldown.id, "Restoration interference", "");
			AssetManager.status.add(goldcooldown);

			#endregion
			#region 色慾 lust_demon_king
	//誘惑法專用 lust demon king
			var LDK1 = new StatusAsset();
			LDK1.id = "lust_demon_king";								 //ID
			LDK1.duration = 3600f;									 //倒數計時
			LDK1.path_icon = "ui/icons/Effects1/demon_king/lust_demon_king";				//效果圖標
			LDK1.locale_id = $"status_title_{LDK1.id}";		//區域設定ID					非必要
			LDK1.locale_description = $"status_description_{LDK1.id}";		//區域設定描述非必要
			LDK1.tier = StatusTier.Basic;
			LDK1.remove_status = new string[] { "cursed", "weaken", "sleeping", "stunned", "burning", "frozen", };
			LDK1.base_stats = new BaseStats();
			LDK1.base_stats.set("personality_aggression", 9999f);
			LDK1.action_on_receive = (WorldAction)Delegate.Combine(LDK1.action_on_receive, new WorldAction(addOther6661));
			LDK1.action_finish = (WorldAction)Delegate.Combine(LDK1.action_finish, new WorldAction(removeOther6669));
			addToLocalizedLibrary("ch",LDK1.id, "色慾魔王", "美色與情慾的化身");
			addToLocalizedLibrary("en",LDK1.id, "Lust Demon King", "The embodiment of beauty and lust.");
			AssetManager.status.add(LDK1);

			var LDK2 = new StatusAsset();//武器機能
			LDK2.id = "ldk2";								 //ID
			LDK2.duration = 60f;									 //倒數計時
			LDK2.path_icon = "ui/icons/items/evilbow";				//效果圖標
			LDK2.locale_id = $"status_title_{LDK2.id}";		//區域設定ID					非必要
			LDK2.locale_description = $"status_description_{LDK2.id}";		//區域設定描述非必要
			LDK2.tier = StatusTier.Basic;
			LDK2.base_stats = new BaseStats();	
			LDK2.base_stats.set("multiplier_damage", -999999999.99f);
			LDK2.base_stats.set("damage", -999999999.99f);
			LDK2.base_stats.set("armor", 99f);
			LDK2.base_stats.set("range", 60f);
			LDK2.base_stats.set("accuracy", 10f);									//添加數值
			addToLocalizedLibrary("ch",LDK2.id, "魔弓甦醒", "魔弓之力開始甦醒");
			addToLocalizedLibrary("en",LDK2.id, "Evil Bow Awakens", "The power of the evil bow begins to awaken.");
			AssetManager.status.add(LDK2);

			var LDK3 = new StatusAsset();
			LDK3.id = "ldk3";								 //ID
			LDK3.duration = 60f;									 //倒數計時
			LDK3.path_icon = "ui/icons/Effects1/eye";				//效果圖標
			LDK3.locale_id = $"status_title_{LDK3.id}";		//區域設定ID					非必要
			LDK3.locale_description = $"status_description_{LDK3.id}";		//區域設定描述非必要
			LDK3.tier = StatusTier.Basic;
			LDK3.opposite_traits = AssetLibrary<StatusAsset>.a<string>(new string[]
			{
				"evillaw_ew", "evillaw_seduction", "evillaw_moneylaw", "evillaw_starvation", "evillaw_sleeping", "evillaw_tantrum", "evillaw_devour", "holyarts_justice"
			});
			LDK3.action_on_receive = (WorldAction)Delegate.Combine(LDK3.action_on_receive, new WorldAction(addFavorite1));
			LDK3.action_finish = (WorldAction)Delegate.Combine(LDK3.action_finish, new WorldAction(EvilLawGet_StatusEffects03));
			addToLocalizedLibrary("ch",LDK3.id, "魔神凝視", "他正在審視你是否夠資格…");
			addToLocalizedLibrary("en",LDK3.id, "Evil God Gaze", "He is examining whether you are qualified...");
			AssetManager.status.add(LDK3);

			var Change = new StatusAsset();
			Change.id = "change";								 //ID
			Change.duration = 60f;									 //倒數計時
			Change.path_icon = "ui/icons/CDT/change";				//效果圖標
			Change.locale_id = $"status_title_{Change.id}";		//區域設定ID					非必要
			Change.locale_description = $"status_description_{Change.id}";		//區域設定描述非必要
			Change.tier = StatusTier.Basic; 
			Change.base_stats = new BaseStats();
			Change.action_finish = (WorldAction)Delegate.Combine(Change.action_finish, new WorldAction(Change01));
			addToLocalizedLibrary("ch",Change.id, "轉換", "性別轉換了");
			addToLocalizedLibrary("en",Change.id, "Change", "Gender changed");
			AssetManager.status.add(Change);

			var Slave = new StatusAsset();
			Slave.id = "slave";								 //ID
			Slave.duration = 60f;									 //倒數計時
			Slave.path_icon = "ui/icons/Skill/Skill0411_2";				//效果圖標
			Slave.locale_id = $"status_title_{Slave.id}";		//區域設定ID					非必要
			Slave.locale_description = $"status_description_{Slave.id}";		//區域設定描述非必要
			Slave.tier = StatusTier.Basic; 
			Slave.remove_status = new string[] { "cursed", "angry" };
			Slave.base_stats = new BaseStats();										//添加數值
			addToLocalizedLibrary("ch",Slave.id, "奴隸", "");
			addToLocalizedLibrary("en",Slave.id, "Slave", "");
			AssetManager.status.add(Slave);

			var Slave01 = new StatusAsset();
			Slave01.id = "slave00";								 //ID
			Slave01.duration = 60f;									 //倒數計時
			Slave01.path_icon = "ui/icons/Effects1/HighRankingSlaves";				//效果圖標
			Slave01.locale_id = $"status_title_{Slave01.id}";		//區域設定ID					非必要
			Slave01.locale_description = $"status_description_{Slave01.id}";		//區域設定描述非必要
			Slave01.tier = StatusTier.Basic; 
			Slave01.remove_status = new string[] { "" };
			Slave01.base_stats = new BaseStats();											//添加數值
			Slave01.base_stats.set("diplomacy", 1009f);										//添加數值
			Slave01.base_stats.set("warfare", 1009f);										//添加數值
			Slave01.base_stats.set("stewardship", 1009f);									//添加數值
			Slave01.base_stats.set("intelligence", 1009f);									//添加數值
			Slave01.base_stats.set("multiplier_supply_timer", -999f);						//添加數值
			addToLocalizedLibrary("ch",Slave01.id, "高級奴隸", "");
			addToLocalizedLibrary("en",Slave01.id, "High Ranking Slave", "");
			AssetManager.status.add(Slave01);
			#endregion
			#region 憤怒 wrath_demon_king
		//忿怒法專用
			var WDK1 = new StatusAsset();
			WDK1.id = "wrath_demon_king";								 //ID
			WDK1.duration = 3600f;									 //倒數計時
			WDK1.path_icon = "ui/icons/Effects1/demon_king/wrath_demon_king";				//效果圖標
			WDK1.locale_id = $"status_title_{WDK1.id}";		//區域設定ID					非必要
			WDK1.locale_description = $"status_description_{WDK1.id}";		//區域設定描述非必要
			WDK1.tier = StatusTier.Basic;
			WDK1.remove_status = new string[] { "calm", "cdt_calm", "cursed", "sleeping", "stunned", "burning", "frozen", };
			WDK1.base_stats = new BaseStats();
			WDK1.base_stats.set("personality_aggression", 9999f);
			WDK1.action_on_receive = (WorldAction)Delegate.Combine(WDK1.action_on_receive, new WorldAction(addOther6661));
			WDK1.action_finish = (WorldAction)Delegate.Combine(WDK1.action_finish, new WorldAction(removeOther6669));
			addToLocalizedLibrary("ch",WDK1.id, "憤怒魔王", "火焰與毀滅的具象化");
			addToLocalizedLibrary("en",WDK1.id, "Wrath Demon King", "The embodiment of fire and destruction.");
			AssetManager.status.add(WDK1);

			var WDK2 = new StatusAsset();//武器機能
			WDK2.id = "wdk2";								 //ID
			WDK2.duration = 60f;									 //倒數計時
			WDK2.path_icon = "ui/icons/items/evilgloves";				//效果圖標
			WDK2.locale_id = $"status_title_{WDK2.id}";		//區域設定ID					非必要
			WDK2.locale_description = $"status_description_{WDK2.id}";		//區域設定描述非必要
			WDK2.tier = StatusTier.Basic;
			WDK2.tier = StatusTier.Basic;
			WDK2.base_stats = new BaseStats();	
			WDK2.base_stats.set("multiplier_damage", 5.0f);
			WDK2.base_stats.set("damage_range", 55f);								//傷害區間 [未顯示]
			WDK2.base_stats.set("armor", 99f);
			WDK2.base_stats.set("multiplier_speed", 3.0f);
			WDK2.base_stats.set("multiplier_attack_speed", 5.0f);
			addToLocalizedLibrary("ch",WDK2.id, "魔拳甦醒", "魔拳之力開始甦醒");
			addToLocalizedLibrary("en",WDK2.id, "Evil Gloves Awakens", "The power of the evil gloves begins to awaken.");
			AssetManager.status.add(WDK2);

			var WDK3 = new StatusAsset();
			WDK3.id = "wdk3";								 //ID
			WDK3.duration = 60f;									 //倒數計時
			WDK3.path_icon = "ui/icons/Effects1/eye";				//效果圖標
			WDK3.locale_id = $"status_title_{WDK3.id}";		//區域設定ID					非必要
			WDK3.locale_description = $"status_description_{WDK3.id}";		//區域設定描述非必要
			WDK3.tier = StatusTier.Basic;
			WDK3.opposite_traits = AssetLibrary<StatusAsset>.a<string>(new string[]
			{
				"evillaw_ew", "evillaw_seduction", "evillaw_moneylaw", "evillaw_starvation", "evillaw_sleeping", "evillaw_tantrum", "evillaw_devour", "holyarts_justice"
			});
			WDK3.action_on_receive = (WorldAction)Delegate.Combine(WDK3.action_on_receive, new WorldAction(addFavorite1));
			WDK3.action_finish = (WorldAction)Delegate.Combine(WDK3.action_finish, new WorldAction(EvilLawGet_StatusEffects04));
			addToLocalizedLibrary("ch",WDK3.id, "魔神凝視", "他正在審視你是否夠資格…");
			addToLocalizedLibrary("en",WDK3.id, "Evil God Gaze", "He is examining whether you are qualified...");
			AssetManager.status.add(WDK3);

			var Calm = new StatusAsset();
			Calm.id = "calm";								 //ID
			Calm.duration = maxDuration;									 //倒數計時
			Calm.path_icon = "ui/icons/Effects1/Calm";				//效果圖標
			Calm.locale_id = $"status_title_{Calm.id}";		//區域設定ID					非必要
			Calm.locale_description = $"status_description_{Calm.id}";		//區域設定描述非必要
			Calm.removed_on_damage = true;										//損壞時移除
			Calm.opposite_status = new string[] { "wrath_demon_king", "" };
			Calm.remove_status = new string[] { "tantrum", "angry" };
			Calm.tier = StatusTier.Basic; 
			Calm.base_stats = new BaseStats();										//添加數值
			addToLocalizedLibrary("ch",Calm.id, "寧靜", "他正在尋求寧靜");
			addToLocalizedLibrary("en",Calm.id, "Calm", "He is seeking peace");
			AssetManager.status.add(Calm);

			var CDT_calm = new StatusAsset();
			CDT_calm.id = "cdt_calm";								 //ID
			CDT_calm.duration = maxDuration;									 //倒數計時
			CDT_calm.path_icon = "ui/icons/CDT/CDT000";				//效果圖標
			CDT_calm.locale_id = $"status_title_{CDT_calm.id}";		//區域設定ID					非必要
			CDT_calm.locale_description = $"status_description_{CDT_calm.id}";		//區域設定描述非必要
			CDT_calm.opposite_status = new string[] { "wrath_demon_king", "" };
			CDT_calm.remove_status = new string[] { "stunned" };
			CDT_calm.tier = StatusTier.Basic; 
			CDT_calm.base_stats = new BaseStats();										//添加數值
			addToLocalizedLibrary("ch",CDT_calm.id, "寧靜冷卻", "");
			addToLocalizedLibrary("en",CDT_calm.id, "Calm CD", "");
			AssetManager.status.add(CDT_calm);
			#endregion
			#region 暴食 gluttony_demon_king
		//餓食法專用
			var GlDK1 = new StatusAsset();
			GlDK1.id = "gluttony_demon_king";								 //ID
			GlDK1.duration = 3600f;									 //倒數計時
			GlDK1.path_icon = "ui/icons/Effects1/demon_king/gluttony_demon_king";				//效果圖標
			GlDK1.locale_id = $"status_title_{GlDK1.id}";		//區域設定ID					非必要
			GlDK1.locale_description = $"status_description_{GlDK1.id}";		//區域設定描述非必要
			GlDK1.tier = StatusTier.Basic;
			GlDK1.remove_status = new string[] { "cursed", "weaken", "sleeping", "stunned", "burning", "frozen", };
			GlDK1.base_stats = new BaseStats();
			GlDK1.base_stats.set("personality_aggression", 9999f);
			GlDK1.action_finish = (WorldAction)Delegate.Combine(GlDK1.action_finish, new WorldAction(removeOther6669));
			GlDK1.action_on_receive = (WorldAction)Delegate.Combine(GlDK1.action_on_receive, new WorldAction(GluttonyStart), new WorldAction(addOther6661));
			addToLocalizedLibrary("ch",GlDK1.id, "暴食魔王", "飢餓與貪食的具象化");
			addToLocalizedLibrary("en",GlDK1.id, "Gluttony Demon King", "The embodiment of hunger and gluttony.");
			AssetManager.status.add(GlDK1);

			var GlDK2 = new StatusAsset();//武器機能
			GlDK2.id = "gldk2";								 //ID
			GlDK2.duration = 60f;									 //倒數計時
			GlDK2.path_icon = "ui/icons/items/evilspear";				//效果圖標
			GlDK2.locale_id = $"status_title_{GlDK2.id}";		//區域設定ID					非必要
			GlDK2.locale_description = $"status_description_{GlDK2.id}";		//區域設定描述非必要
			GlDK2.tier = StatusTier.Basic;
			GlDK2.base_stats = new BaseStats();	
			// GlDK2.base_stats.set("multiplier_damage", 2.50f);
			GlDK2.base_stats.set("damage_range", 25f);								//傷害區間 [未顯示]
			GlDK2.base_stats.set("range", 1f);
			GlDK2.base_stats.set("armor", 99f);
			GlDK2.base_stats.set("multiplier_speed", 3.0f);
			GlDK2.base_stats.set("multiplier_attack_speed", 5.0f);
			addToLocalizedLibrary("ch",GlDK2.id, "魔槍甦醒", "魔槍之力開始甦醒");
			addToLocalizedLibrary("en",GlDK2.id, "Evil Spear Awakens", "The power of the evil spear begins to awaken.");
			AssetManager.status.add(GlDK2);

			var GlDK3 = new StatusAsset();
			GlDK3.id = "gldk3";								 //ID
			GlDK3.duration = 60f;									 //倒數計時
			GlDK3.path_icon = "ui/icons/Effects1/eye";				//效果圖標
			GlDK3.locale_id = $"status_title_{GlDK3.id}";		//區域設定ID					非必要
			GlDK3.locale_description = $"status_description_{GlDK3.id}";		//區域設定描述非必要
			GlDK3.tier = StatusTier.Basic;
			GlDK3.opposite_traits = AssetLibrary<StatusAsset>.a<string>(new string[]
			{
				"evillaw_ew", "evillaw_seduction", "evillaw_moneylaw", "evillaw_starvation", "evillaw_sleeping", "evillaw_tantrum", "evillaw_devour", "holyarts_justice"
			});
			GlDK3.action_on_receive = (WorldAction)Delegate.Combine(GlDK3.action_on_receive, new WorldAction(addFavorite1));
			GlDK3.action_finish = (WorldAction)Delegate.Combine(GlDK3.action_finish, new WorldAction(EvilLawGet_StatusEffects05));
			addToLocalizedLibrary("ch",GlDK3.id, "魔神凝視", "他正在審視你是否夠資格…");
			addToLocalizedLibrary("en",GlDK3.id, "Evil God Gaze", "He is examining whether you are qualified...");
			AssetManager.status.add(GlDK3);
			#endregion
			#region 怠惰 sloth_demon_king
		//睡夢法專用
			var SDK1 = new StatusAsset();
			SDK1.id = "sloth_demon_king";								 //ID
			SDK1.duration = 3600f;									 //倒數計時
			SDK1.path_icon = "ui/icons/Effects1/demon_king/sloth_demon_king";				//效果圖標
			SDK1.locale_id = $"status_title_{SDK1.id}";		//區域設定ID					非必要
			SDK1.locale_description = $"status_description_{SDK1.id}";		//區域設定描述非必要
			SDK1.tier = StatusTier.Basic;
			SDK1.remove_status = new string[] { "cursed", "weaken", "stunned", "burning", "frozen" };
			SDK1.base_stats = new BaseStats();
			SDK1.base_stats.set("personality_aggression", 9999f);
			SDK1.action_on_receive = (WorldAction)Delegate.Combine(SDK1.action_on_receive, new WorldAction(addOther6661));
			SDK1.action_finish = (WorldAction)Delegate.Combine(SDK1.action_finish, new WorldAction(removeOther6669));
			addToLocalizedLibrary("ch",SDK1.id, "怠惰魔王", "嚴冬與安眠的的主宰");
			addToLocalizedLibrary("en",SDK1.id, "Sloth Demon King", "The ruler of winter and sleep.");
			AssetManager.status.add(SDK1);

			var SDK2 = new StatusAsset();//武器機能
			SDK2.id = "sdk2";								 //ID
			SDK2.duration = 60f;									 //倒數計時
			SDK2.path_icon = "ui/icons/items/evilstick";				//效果圖標
			SDK2.locale_id = $"status_title_{SDK2.id}";		//區域設定ID					非必要
			SDK2.locale_description = $"status_description_{SDK2.id}";		//區域設定描述非必要
			SDK2.tier = StatusTier.Basic;
			SDK2.base_stats = new BaseStats();	
			//SDK2.base_stats.set("multiplier_damage", 2.50f);
			SDK2.base_stats.set("damage_range", 40f);								//傷害區間 [未顯示]
			SDK2.base_stats.set("range", 1f);
			SDK2.base_stats.set("armor", 99f);
			SDK2.base_stats.set("multiplier_speed", 3.0f);
			SDK2.base_stats.set("multiplier_attack_speed", 5.0f);
			addToLocalizedLibrary("ch",SDK2.id, "魔杖甦醒", "魔杖之力開始甦醒");
			addToLocalizedLibrary("en",SDK2.id, "Evil Stick Awakens", "The power of the evil stick begins to awaken.");
			AssetManager.status.add(SDK2);

			var SDK3 = new StatusAsset();
			SDK3.id = "sdk3";								 //ID
			SDK3.duration = 60f;									 //倒數計時
			SDK3.path_icon = "ui/icons/Effects1/eye";				//效果圖標
			SDK3.locale_id = $"status_title_{SDK3.id}";		//區域設定ID					非必要
			SDK3.locale_description = $"status_description_{SDK3.id}";		//區域設定描述非必要
			SDK3.tier = StatusTier.Basic;
			SDK3.opposite_traits = AssetLibrary<StatusAsset>.a<string>(new string[]
			{
				"evillaw_ew", "evillaw_seduction", "evillaw_moneylaw", "evillaw_starvation", "evillaw_sleeping", "evillaw_tantrum", "evillaw_devour", "holyarts_justice"
			});
			SDK3.action_on_receive = (WorldAction)Delegate.Combine(SDK3.action_on_receive, new WorldAction(addFavorite1));
			SDK3.action_finish = (WorldAction)Delegate.Combine(SDK3.action_finish, new WorldAction(EvilLawGet_StatusEffects06));
			addToLocalizedLibrary("ch",SDK3.id, "魔神凝視", "他正在審視你是否夠資格…");
			addToLocalizedLibrary("en",SDK3.id, "Evil God Gaze", "He is examining whether you are qualified...");
			AssetManager.status.add(SDK3);

			var cdt_Sleeping = new StatusAsset();
			cdt_Sleeping.id = "cdt_sleeping";								 //ID
			cdt_Sleeping.duration = 60f;									 //倒數計時
			cdt_Sleeping.path_icon = "ui/icons/Effects1/cdt_sleeping";				//效果圖標
			cdt_Sleeping.locale_id = $"status_title_{cdt_Sleeping.id}";		//區域設定ID					非必要
			cdt_Sleeping.locale_description = $"status_description_{cdt_Sleeping.id}";		//區域設定描述非必要			非必要
			cdt_Sleeping.tier = StatusTier.Basic;
			addToLocalizedLibrary("ch",cdt_Sleeping.id, "發動冷卻", "冷卻倒數,距離再次使用還要…");
			addToLocalizedLibrary("en",cdt_Sleeping.id, "Start Cooling", "Cooling down, it will take a while before it can be used again...");
			AssetManager.status.add(cdt_Sleeping);

			var Apostle = new StatusAsset();
			Apostle.id = "apostle_se";								 //ID
			Apostle.duration = 60f;									 //倒數計時
			Apostle.path_icon = "ui/icons/Skill/Skill0408_2";		//效果圖標
			Apostle.locale_id = $"status_title_{Apostle.id}";		//區域設定ID					非必要
			Apostle.locale_description = $"status_description_{Apostle.id}";		//區域設定描述非必要			非必要
			Apostle.tier = StatusTier.Basic;
			Apostle.remove_status = new string[] { "cursed", "frozen", "sleeping" };
			Apostle.action_finish = (WorldAction)Delegate.Combine(Apostle.action_finish, new WorldAction(ApostleUnitEND2));
			addToLocalizedLibrary("ch",Apostle.id, "使徒Ⅰ", "");
			addToLocalizedLibrary("en",Apostle.id, "ApostleⅠ", "");
			AssetManager.status.add(Apostle);

			var Ruthless = new StatusAsset();
			Ruthless.id = "apostle_se2";								 //ID
			Ruthless.duration = 60f;									 //倒數計時
			Ruthless.path_icon = "ui/icons/Skill/Skill0408_2";		//效果圖標
			Ruthless.locale_id = $"status_title_{Ruthless.id}";		//區域設定ID					非必要
			Ruthless.locale_description = $"status_description_{Ruthless.id}";		//區域設定描述非必要			非必要
			Ruthless.tier = StatusTier.Basic;
			Ruthless.remove_status = new string[] { "stunned", "tantrum", "angry", "fell_in_love" };
			addToLocalizedLibrary("ch",Ruthless.id, "使徒Ⅱ", "");
			addToLocalizedLibrary("en",Ruthless.id, "RuthlessⅡ", "");
			AssetManager.status.add(Ruthless);
			#endregion
			#region 嫉妒 envy_demon_king
		//吞噬法專用
			var EDK1 = new StatusAsset();
			EDK1.id = "envy_demon_king";								 //ID
			EDK1.duration = 3600f;									 //倒數計時
			EDK1.path_icon = "ui/icons/Effects1/demon_king/envy_demon_king";				//效果圖標
			EDK1.locale_id = $"status_title_{EDK1.id}";		//區域設定ID					非必要
			EDK1.locale_description = $"status_description_{EDK1.id}";		//區域設定描述非必要
			EDK1.tier = StatusTier.Basic;
			EDK1.remove_status = new string[] { "cursed", "weaken", "weaken", "stunned", "frozen", "sleeping" };
			EDK1.base_stats = new BaseStats();
			EDK1.base_stats.set("personality_aggression", 9999f);
			EDK1.action_on_receive = (WorldAction)Delegate.Combine(EDK1.action_on_receive, new WorldAction(addOther6661));
			EDK1.action_finish = (WorldAction)Delegate.Combine(EDK1.action_finish, new WorldAction(removeOther6669));
			addToLocalizedLibrary("ch",EDK1.id, "嫉妒魔王", "司掌『比較』與『競爭』者");
			addToLocalizedLibrary("en",EDK1.id, "Envy Demon King", "The demon of comparison and competition.");
			AssetManager.status.add(EDK1);

			var EDK2 = new StatusAsset();//武器機能
			EDK2.id = "edk2";								 //ID
			EDK2.duration = 60f;									 //倒數計時
			EDK2.path_icon = "ui/icons/items/evilponiard";				//效果圖標
			EDK2.locale_id = $"status_title_{EDK2.id}";		//區域設定ID					非必要
			EDK2.locale_description = $"status_description_{EDK2.id}";		//區域設定描述非必要
			EDK2.tier = StatusTier.Basic;
			EDK2.base_stats = new BaseStats();	
			EDK2.base_stats.set("multiplier_damage", 0.50f);
			EDK2.base_stats.set("range", 1f);
			EDK2.base_stats.set("armor", 99f);
			EDK2.base_stats.set("multiplier_speed", 10.0f);
			EDK2.base_stats.set("multiplier_attack_speed", 3.0f);
			addToLocalizedLibrary("ch",EDK2.id, "魔刃甦醒", "魔刃之力開始甦醒");
			addToLocalizedLibrary("en",EDK2.id, "Evil Poniard Awakens", "The power of the evil poniard begins to awaken.");
			AssetManager.status.add(EDK2);

			var EDK3 = new StatusAsset();
			EDK3.id = "edk3";								 //ID
			EDK3.duration = 60f;									 //倒數計時
			EDK3.path_icon = "ui/icons/Effects1/eye";				//效果圖標
			EDK3.locale_id = $"status_title_{EDK3.id}";		//區域設定ID					非必要
			EDK3.locale_description = $"status_description_{EDK3.id}";		//區域設定描述非必要
			EDK3.tier = StatusTier.Basic;
			EDK3.opposite_traits = AssetLibrary<StatusAsset>.a<string>(new string[]
			{
				"evillaw_ew", "evillaw_seduction", "evillaw_moneylaw", "evillaw_starvation", "evillaw_sleeping", "evillaw_tantrum", "evillaw_devour", "holyarts_justice"
			});
			EDK3.action_on_receive = (WorldAction)Delegate.Combine(EDK3.action_on_receive, new WorldAction(addFavorite1));
			EDK3.action_finish = (WorldAction)Delegate.Combine(EDK3.action_finish, new WorldAction(EvilLawGet_StatusEffects07));
			addToLocalizedLibrary("ch",EDK3.id, "魔神凝視", "他正在審視你是否夠格…");
			addToLocalizedLibrary("en",EDK3.id, "Evil God Gaze", "He is examining whether you are qualified...");
			AssetManager.status.add(EDK3);

			var eating = new StatusAsset();//進食
			eating.id = "eating";								 //ID
			eating.duration = 60f;									 //倒數計時
			eating.path_icon = "ui/icons/CDT/CDT000";				//效果圖標
			eating.locale_id = $"status_title_{eating.id}";	//區域設定ID					非必要
			eating.locale_description = $"status_description_{eating.id}";	//區域設定描述非必要			非必要
			eating.tier = StatusTier.Basic;										//狀態層級						非必要
			eating.opposite_status = new string[] { }; // 移除空字串 ""
			eating.remove_status = new string[] { };	 // 移除空字串 ""
			// 如果這個狀態沒有任何對立或要移除的狀態，可以將陣列留空 `new string[] { }`
			addToLocalizedLibrary("ch",eating.id, "進食中", "");
			addToLocalizedLibrary("en",eating.id, "Eating", "");
			AssetManager.status.add(eating);

			var Happiness_Leak = new StatusAsset();//進食
			Happiness_Leak.id = "happiness_leak";								 //ID
			Happiness_Leak.duration = 60f;									 //倒數計時
			Happiness_Leak.path_icon = "ui/icons/CDT/CDT000";				//效果圖標
			Happiness_Leak.locale_id = $"status_title_{Happiness_Leak.id}";	//區域設定ID					非必要
			Happiness_Leak.locale_description = $"status_description_{Happiness_Leak.id}";	//區域設定描述非必要			非必要
			Happiness_Leak.tier = StatusTier.Basic;										//狀態層級						非必要
			Happiness_Leak.opposite_status = new string[] { }; // 移除空字串 ""
			Happiness_Leak.remove_status = new string[] { };	 // 移除空字串 ""
			// 如果這個狀態沒有任何對立或要移除的狀態，可以將陣列留空 `new string[] { }`
			addToLocalizedLibrary("ch",Happiness_Leak.id, "幸福洩漏", "");
			addToLocalizedLibrary("en",Happiness_Leak.id, "Happiness Leak", "");
			AssetManager.status.add(Happiness_Leak);
			#endregion
			#region 模神附體者 Demon Possessed
			var dm6661 = new StatusAsset();
			dm6661.id = "dm6661";								 //ID
			dm6661.duration = 400f;									 //倒數計時
			dm6661.path_icon = "ui/icons/Other/Other6661";				//效果圖標
			dm6661.locale_id = $"status_title_{dm6661.id}";	//區域設定ID					非必要
			dm6661.locale_description = $"status_description_{dm6661.id}";	//區域設定描述非必要			非必要
			dm6661.tier = StatusTier.Basic;
			dm6661.action_finish = (WorldAction)Delegate.Combine(dm6661.action_finish, new WorldAction(dm6661_Manage));
			// 如果這個狀態沒有任何對立或要移除的狀態，可以將陣列留空 `new string[] { }`
			addToLocalizedLibrary("ch",dm6661.id, "魔神附體者", "");
			addToLocalizedLibrary("en",dm6661.id, "Demon Possessed", "");
			AssetManager.status.add(dm6661);

			var dm6662 = new StatusAsset();
			dm6662.id = "dm6662";								 //ID
			dm6662.duration = 400f;									 //倒數計時
			dm6662.path_icon = "ui/icons/Other/Other6662";				//效果圖標
			dm6662.locale_id = $"status_title_{dm6662.id}";	//區域設定ID					非必要
			dm6662.locale_description = $"status_description_{dm6662.id}";	//區域設定描述非必要			非必要
			dm6662.tier = StatusTier.Basic;
			dm6662.action_finish = (WorldAction)Delegate.Combine(dm6662.action_finish, new WorldAction(dm6662_Manage));
			// 如果這個狀態沒有任何對立或要移除的狀態，可以將陣列留空 `new string[] { }`
			addToLocalizedLibrary("ch",dm6662.id, "魔神附體者", "");
			addToLocalizedLibrary("en",dm6662.id, "Demon Possessed", "");
			AssetManager.status.add(dm6662);

			var dm6663 = new StatusAsset();
			dm6663.id = "dm6663";								 //ID
			dm6663.duration = 400f;									 //倒數計時
			dm6663.path_icon = "ui/icons/Other/Other6663";				//效果圖標
			dm6663.locale_id = $"status_title_{dm6663.id}";	//區域設定ID					非必要
			dm6663.locale_description = $"status_description_{dm6663.id}";	//區域設定描述非必要			非必要
			dm6663.tier = StatusTier.Basic;
			dm6663.action_finish = (WorldAction)Delegate.Combine(dm6663.action_finish, new WorldAction(dm6663_Manage));
			// 如果這個狀態沒有任何對立或要移除的狀態，可以將陣列留空 `new string[] { }`
			addToLocalizedLibrary("ch",dm6663.id, "魔神附體者", "");
			addToLocalizedLibrary("en",dm6663.id, "Demon Possessed", "");
			AssetManager.status.add(dm6663);

			var dm6664 = new StatusAsset();
			dm6664.id = "dm6664";								 //ID
			dm6664.duration = 400f;									 //倒數計時
			dm6664.path_icon = "ui/icons/Other/Other6664";				//效果圖標
			dm6664.locale_id = $"status_title_{dm6664.id}";	//區域設定ID					非必要
			dm6664.locale_description = $"status_description_{dm6664.id}";	//區域設定描述非必要			非必要
			dm6664.tier = StatusTier.Basic;
			dm6664.action_finish = (WorldAction)Delegate.Combine(dm6664.action_finish, new WorldAction(dm6664_Manage));
			// 如果這個狀態沒有任何對立或要移除的狀態，可以將陣列留空 `new string[] { }`
			addToLocalizedLibrary("ch",dm6664.id, "魔神附體者", "");
			addToLocalizedLibrary("en",dm6664.id, "Demon Possessed", "");
			AssetManager.status.add(dm6664);

			var dm6665 = new StatusAsset();
			dm6665.id = "dm6665";								 //ID
			dm6665.duration = 400f;									 //倒數計時
			dm6665.path_icon = "ui/icons/Other/Other6665";				//效果圖標
			dm6665.locale_id = $"status_title_{dm6665.id}";	//區域設定ID					非必要
			dm6665.locale_description = $"status_description_{dm6665.id}";	//區域設定描述非必要			非必要
			dm6665.tier = StatusTier.Basic;
			dm6665.action_finish = (WorldAction)Delegate.Combine(dm6665.action_finish, new WorldAction(dm6665_Manage));
			// 如果這個狀態沒有任何對立或要移除的狀態，可以將陣列留空 `new string[] { }`
			addToLocalizedLibrary("ch",dm6665.id, "魔神附體者", "");
			addToLocalizedLibrary("en",dm6665.id, "Demon Possessed", "");
			AssetManager.status.add(dm6665);

			var dm6666 = new StatusAsset();
			dm6666.id = "dm6666";								 //ID
			dm6666.duration = 400f;									 //倒數計時
			dm6666.path_icon = "ui/icons/Other/Other6666";				//效果圖標
			dm6666.locale_id = $"status_title_{dm6666.id}";	//區域設定ID					非必要
			dm6666.locale_description = $"status_description_{dm6666.id}";	//區域設定描述非必要			非必要
			dm6666.tier = StatusTier.Basic;
			dm6666.action_finish = (WorldAction)Delegate.Combine(dm6666.action_finish, new WorldAction(dm6666_Manage));
			// 如果這個狀態沒有任何對立或要移除的狀態，可以將陣列留空 `new string[] { }`
			addToLocalizedLibrary("ch",dm6666.id, "魔神附體者", "");
			addToLocalizedLibrary("en",dm6666.id, "Demon Possessed", "");
			AssetManager.status.add(dm6666);

			var dm6667 = new StatusAsset();
			dm6667.id = "dm6667";								 //ID
			dm6667.duration = 400f;									 //倒數計時
			dm6667.path_icon = "ui/icons/Other/Other6667";				//效果圖標
			dm6667.locale_id = $"status_title_{dm6667.id}";	//區域設定ID					非必要
			dm6667.locale_description = $"status_description_{dm6667.id}";	//區域設定描述非必要			非必要
			dm6667.tier = StatusTier.Basic;
			dm6667.action_finish = (WorldAction)Delegate.Combine(dm6667.action_finish, new WorldAction(dm6667_Manage));
			// 如果這個狀態沒有任何對立或要移除的狀態，可以將陣列留空 `new string[] { }`
			addToLocalizedLibrary("ch",dm6667.id, "魔神附體者", "");
			addToLocalizedLibrary("en",dm6667.id, "Demon Possessed", "");
			AssetManager.status.add(dm6667);

			var dm6668 = new StatusAsset();
			dm6668.id = "dm6668";								 //ID
			dm6668.duration = 400f;									 //倒數計時
			dm6668.path_icon = "ui/icons/Other/Other6668";				//效果圖標
			dm6668.locale_id = $"status_title_{dm6668.id}";	//區域設定ID					非必要
			dm6668.locale_description = $"status_description_{dm6668.id}";	//區域設定描述非必要			非必要
			dm6668.tier = StatusTier.Basic;
			dm6668.action_finish = (WorldAction)Delegate.Combine(dm6668.action_finish, new WorldAction(dm6668_Manage));
			// 如果這個狀態沒有任何對立或要移除的狀態，可以將陣列留空 `new string[] { }`
			addToLocalizedLibrary("ch",dm6668.id, "魔神附體者", "");
			addToLocalizedLibrary("en",dm6668.id, "Demon Possessed", "");
			AssetManager.status.add(dm6668);

			var dm6669 = new StatusAsset();
			dm6669.id = "dm6669";								 //ID
			dm6669.duration = 400f;									 //倒數計時
			dm6669.path_icon = "ui/icons/Other/Other6669";				//效果圖標
			dm6669.locale_id = $"status_title_{dm6669.id}";	//區域設定ID					非必要
			dm6669.locale_description = $"status_description_{dm6669.id}";	//區域設定描述非必要			非必要
			dm6669.tier = StatusTier.Basic;
			dm6669.action_finish = (WorldAction)Delegate.Combine(dm6669.action_finish, new WorldAction(dm6669_Manage));
			// 如果這個狀態沒有任何對立或要移除的狀態，可以將陣列留空 `new string[] { }`
			addToLocalizedLibrary("ch",dm6669.id, "魔神附體者", "");
			addToLocalizedLibrary("en",dm6669.id, "Demon Possessed", "");
			AssetManager.status.add(dm6669);

			var DefenseOn = new StatusAsset();
			DefenseOn.id = "defense_on";								 //ID
			DefenseOn.duration = 30f;									 //倒數計時
			DefenseOn.path_icon = "ui/icons/Effects1/defense_on";				//效果圖標
			DefenseOn.locale_id = $"status_title_{DefenseOn.id}";	//區域設定ID					非必要
			DefenseOn.locale_description = $"status_description_{DefenseOn.id}";	//區域設定描述非必要			非必要
			DefenseOn.tier = StatusTier.Basic;
			DefenseOn.opposite_status = new string[] { "defense_off" };				//對立
			DefenseOn.remove_status = new string[] { "" };
			// 如果這個狀態沒有任何對立或要移除的狀態，可以將陣列留空 `new string[] { }`
			addToLocalizedLibrary("ch",DefenseOn.id, "特防啟動", "");
			addToLocalizedLibrary("en",DefenseOn.id, "Special Defense startup", "");
			AssetManager.status.add(DefenseOn);

			var DefenseOff = new StatusAsset();
			DefenseOff.id = "defense_off";								 //ID
			DefenseOff.duration = 30f;									 //倒數計時
			DefenseOff.path_icon = "ui/icons/Effects1/defense_off";				//效果圖標
			DefenseOff.locale_id = $"status_title_{DefenseOff.id}";	//區域設定ID					非必要
			DefenseOff.locale_description = $"status_description_{DefenseOff.id}";	//區域設定描述非必要			非必要
			DefenseOff.tier = StatusTier.Basic;
			DefenseOff.opposite_status = new string[] { "" };				//對立
			DefenseOff.remove_status = new string[] { "defense_on" };
			// 如果這個狀態沒有任何對立或要移除的狀態，可以將陣列留空 `new string[] { }`
			addToLocalizedLibrary("ch",DefenseOff.id, "特防破碎", "");
			addToLocalizedLibrary("en",DefenseOff.id, "Special Defense broken", "");
			AssetManager.status.add(DefenseOff);

			#endregion
			#region 不死帝 ex_undead_emperor
			var UndeadEmperor = new StatusAsset();
			UndeadEmperor.id = "ex_undead_emperor";								 //ID
			UndeadEmperor.duration = 60f;									 //倒數計時
			UndeadEmperor.path_icon = "ui/icons/Effects1/undead/UndeadEmperor";				//效果圖標
			UndeadEmperor.locale_id = $"status_title_{UndeadEmperor.id}";	//區域設定ID					非必要
			UndeadEmperor.locale_description = $"status_description_{UndeadEmperor.id}";	//區域設定描述非必要			非必要
			UndeadEmperor.tier = StatusTier.None;//Advanced None Basic
			//UndeadEmperor.remove_status = new string[] { "weaken", "stunned", "confused", "burning", "frozen", "slowness" };
			//UndeadEmperor.base_stats = new BaseStats();
			addToLocalizedLibrary("ch",UndeadEmperor.id, "不死大帝", "屹立於眾多不死者之上的不死王者。");
			addToLocalizedLibrary("en",UndeadEmperor.id, "Undead Emperor", "The undead king who stands tall above the multitude of undead.");
			AssetManager.status.add(UndeadEmperor);

			var UndeadLord = new StatusAsset();
			UndeadLord.id = "undead_lord";								 //ID
			UndeadLord.duration = 10f;									 //倒數計時
			UndeadLord.path_icon = "ui/icons/Effects1/undead/UndeadLord";				//效果圖標
			UndeadLord.locale_id = $"status_title_{UndeadLord.id}";	//區域設定ID					非必要
			UndeadLord.locale_description = $"status_description_{UndeadLord.id}";	//區域設定描述非必要			非必要
			UndeadLord.tier = StatusTier.Basic;
			UndeadLord.base_stats = new BaseStats();
			UndeadLord.base_stats.set("diplomacy", 150f);
			UndeadLord.base_stats.set("warfare", 150f);
			UndeadLord.base_stats.set("stewardship", 150f);
			UndeadLord.base_stats.set("intelligence", 150f);
			UndeadLord.base_stats.set("loyalty_traits", 1300f);
			UndeadLord.base_stats.set("multiplier_supply_timer", -10f);
			addToLocalizedLibrary("ch",UndeadLord.id, "不死領主", "不死國度的領地管理者");
			addToLocalizedLibrary("en",UndeadLord.id, "Undead Lord", "Territory Manager of the Undead Kingdom.");
			AssetManager.status.add(UndeadLord);

			var UndeadLord01 = new StatusAsset();
			UndeadLord01.id = "undead_lord01";								 //ID
			UndeadLord01.duration = 10f;									 //倒數計時
			UndeadLord01.path_icon = "ui/icons/Effects1/undead/UndeadLord01";				//效果圖標
			UndeadLord01.locale_id = $"status_title_{UndeadLord01.id}";	//區域設定ID					非必要
			UndeadLord01.locale_description = $"status_description_{UndeadLord01.id}";	//區域設定描述非必要			非必要
			UndeadLord01.tier = StatusTier.Basic;
			UndeadLord01.base_stats = new BaseStats();
			UndeadLord01.base_stats.set("diplomacy", 50f);
			UndeadLord01.base_stats.set("warfare", 50f);
			UndeadLord01.base_stats.set("stewardship", 50f);
			UndeadLord01.base_stats.set("intelligence", 50f);
			UndeadLord01.base_stats.set("loyalty_traits", 650f);
			UndeadLord01.base_stats.set("multiplier_supply_timer", -10f);
			addToLocalizedLibrary("ch",UndeadLord01.id, "不死領主", "不死國度的領地管理者");
			addToLocalizedLibrary("en",UndeadLord01.id, "Undead Lord", "Territory Manager of the Undead Kingdom.");
			AssetManager.status.add(UndeadLord01);

			var UndeadCaptain = new StatusAsset();
			UndeadCaptain.id = "undead_captain";								 //ID
			UndeadCaptain.duration = 10f;									 //倒數計時
			UndeadCaptain.path_icon = "ui/icons/Effects1/undead/UndeadCaptain";				//效果圖標
			UndeadCaptain.locale_id = $"status_title_{UndeadCaptain.id}";	//區域設定ID					非必要
			UndeadCaptain.locale_description = $"status_description_{UndeadCaptain.id}";	//區域設定描述非必要			非必要
			UndeadCaptain.tier = StatusTier.Basic;
			UndeadCaptain.base_stats = new BaseStats();
			UndeadCaptain.base_stats.set("multiplier_damage", 0.25f);
			UndeadCaptain.base_stats.set("armor", 40f);
			UndeadCaptain.base_stats.set("multiplier_speed", 0.25f);
			UndeadCaptain.base_stats.set("projectiles", 5f);
			addToLocalizedLibrary("ch",UndeadCaptain.id, "不死騎士", "不死者軍團的統率者");
			addToLocalizedLibrary("en",UndeadCaptain.id, "Undead Knight", "Commander of the Undead Legion");
			AssetManager.status.add(UndeadCaptain);

			var UndeadCaptain01 = new StatusAsset();
			UndeadCaptain01.id = "undead_captain01";								 //ID
			UndeadCaptain01.duration = 10f;									 //倒數計時
			UndeadCaptain01.path_icon = "ui/icons/Effects1/undead/UndeadCaptain01";				//效果圖標
			UndeadCaptain01.locale_id = $"status_title_{UndeadCaptain01.id}";	//區域設定ID					非必要
			UndeadCaptain01.locale_description = $"status_description_{UndeadCaptain01.id}";	//區域設定描述非必要			非必要
			UndeadCaptain01.tier = StatusTier.Basic;
			UndeadCaptain01.base_stats = new BaseStats();
			UndeadCaptain01.base_stats.set("multiplier_damage", 0.10f);
			UndeadCaptain01.base_stats.set("armor", 10f);
			UndeadCaptain01.base_stats.set("multiplier_speed", 0.10f);
			UndeadCaptain01.base_stats.set("projectiles", 3f);
			addToLocalizedLibrary("ch",UndeadCaptain01.id, "不死騎士", "不死者軍團的統率者");
			addToLocalizedLibrary("en",UndeadCaptain01.id, "Undead Knight", "Commander of the Undead Legion");
			AssetManager.status.add(UndeadCaptain01);

			var UndeadWarrior = new StatusAsset();
			UndeadWarrior.id = "undead_warrior";								 //ID
			UndeadWarrior.duration = 10f;									 //倒數計時
			UndeadWarrior.path_icon = "ui/icons/Effects1/undead/UndeadWarrior";				//效果圖標
			UndeadWarrior.locale_id = $"status_title_{UndeadWarrior.id}";	//區域設定ID					非必要
			UndeadWarrior.locale_description = $"status_description_{UndeadWarrior.id}";	//區域設定描述非必要			非必要
			UndeadWarrior.tier = StatusTier.Basic;
			UndeadWarrior.base_stats = new BaseStats();
			UndeadWarrior.base_stats.set("multiplier_damage", 0.10f);
			UndeadWarrior.base_stats.set("armor", 20f);
			UndeadWarrior.base_stats.set("multiplier_speed", 0.10f);
			UndeadWarrior.base_stats.set("projectiles", 2f);
			addToLocalizedLibrary("ch",UndeadWarrior.id, "不死戰士", "不死者軍團的兵卒");
			addToLocalizedLibrary("en",UndeadWarrior.id, "Undead Knight", "Soldiers of the Undead Legion");
			AssetManager.status.add(UndeadWarrior);

			var UndeadWarrior01 = new StatusAsset();
			UndeadWarrior01.id = "undead_warrior01";								 //ID
			UndeadWarrior01.duration = 10f;									 //倒數計時
			UndeadWarrior01.path_icon = "ui/icons/Effects1/undead/UndeadWarrior01";				//效果圖標
			UndeadWarrior01.locale_id = $"status_title_{UndeadWarrior01.id}";	//區域設定ID					非必要
			UndeadWarrior01.locale_description = $"status_description_{UndeadWarrior01.id}";	//區域設定描述非必要			非必要
			UndeadWarrior01.tier = StatusTier.Basic;
			UndeadWarrior01.base_stats = new BaseStats();
			UndeadWarrior01.base_stats.set("multiplier_damage", 0.05f);
			UndeadWarrior01.base_stats.set("armor", 10f);
			UndeadWarrior01.base_stats.set("multiplier_speed", 0.05f);
			UndeadWarrior01.base_stats.set("projectiles", 1f);
			addToLocalizedLibrary("ch",UndeadWarrior01.id, "不死戰士", "不死者軍團的兵卒");
			addToLocalizedLibrary("en",UndeadWarrior01.id, "Undead Knight", "Soldiers of the Undead Legion");
			AssetManager.status.add(UndeadWarrior01);

			var DarkBlessing = new StatusAsset();
			DarkBlessing.id = "darkblessing";								 //ID
			DarkBlessing.duration = 60f;									 //倒數計時
			DarkBlessing.path_icon = "ui/icons/Effects1/undead/DarkBlessing";				//效果圖標
			DarkBlessing.locale_id = $"status_title_{DarkBlessing.id}";	//區域設定ID					非必要
			DarkBlessing.locale_description = $"status_description_{DarkBlessing.id}";	//區域設定描述非必要			非必要
			DarkBlessing.tier = StatusTier.None;
			DarkBlessing.remove_status = new string[] { "weaken", "slowness", "cough", "ash_fever", "frozen", "burning", "poisoned", "stunned", "sleeping", "spell_silence", "drowning", "confused" };
			DarkBlessing.base_stats = new BaseStats();
			DarkBlessing.base_stats.addTag("immunity_fire");
			DarkBlessing.base_stats.addTag("immunity_cold");
			addToLocalizedLibrary("ch",DarkBlessing.id, "黑暗賜福", "來自不死帝王的賜福，得以抵禦一切負面影響");
			addToLocalizedLibrary("en",DarkBlessing.id, "Dark Blessing", "The blessing from the Immortal Emperor can resist all negative influences");
			AssetManager.status.add(DarkBlessing);

			var Soul = new StatusAsset();
			Soul.id = "soul";								 //ID
			Soul.duration = 10f;									 //倒數計時
			Soul.path_icon = "ui/icons/Effects1/undead/Soul";				//效果圖標
			Soul.locale_id = $"status_title_{Soul.id}";	//區域設定ID					非必要
			Soul.locale_description = $"status_description_{Soul.id}";	//區域設定描述非必要			非必要
			Soul.tier = StatusTier.Basic;
			Soul.base_stats = new BaseStats();
			Soul.action_death = (WorldAction)Delegate.Combine(Soul.action_death, new WorldAction(spawnGhost));
			addToLocalizedLibrary("ch",Soul.id, "靈魂", "現在是有魂之人");
			addToLocalizedLibrary("en",Soul.id, "Soul", "Now I am a person with soul");
			AssetManager.status.add(Soul);
			#endregion

			#region
			var Brave_00 = new StatusAsset();
			Brave_00.id = "brave";								 //ID
			Brave_00.duration = 30f;									 //倒數計時
			Brave_00.path_icon = "ui/icons/Effects1/brave";				//效果圖標
			Brave_00.locale_id = $"status_title_{Brave_00.id}";		//區域設定ID					非必要
			Brave_00.locale_description = $"status_description_{Brave_00.id}";		//區域設定描述非必要
			Brave_00.tier = StatusTier.Basic;
			//Brave_00.opposite_status = new string[] { "evil" };
			Brave_00.remove_status = new string[] { "cursed", "weaken", "slowness", "cough", "ash_fever", "frozen", "burning", "poisoned", "stunned", "sleeping", "silenced", "drowning", "tantrum", "angry" };
			Brave_00.base_stats = new BaseStats();
			Brave_00.base_stats.addTag("immunity_fire");
			Brave_00.base_stats.addTag("immunity_cold");
			Brave_00.action_finish = (WorldAction)Delegate.Combine(Brave_00.action_finish, new WorldAction(Brave_00_Manage));
			addToLocalizedLibrary("ch",Brave_00.id, "勇者", "與魔王對峙的勇敢之人。");
			addToLocalizedLibrary("en",Brave_00.id, "Brave", "A brave man who faced off against the Demon King.");
			AssetManager.status.add(Brave_00);

			var Evil_00 = new StatusAsset();
			Evil_00.id = "evil";								 //ID
			Evil_00.duration = 30f;									 //倒數計時
			Evil_00.path_icon = "ui/Icons/actor_traits/iconEvil";				//效果圖標
			Evil_00.locale_id = $"status_title_{Evil_00.id}";		//區域設定ID					非必要
			Evil_00.locale_description = $"status_description_{Evil_00.id}";		//區域設定描述非必要
			Evil_00.tier = StatusTier.Basic;
			//Evil_00.remove_status = new string[] { "brave" };
			Evil_00.opposite_traits = AssetLibrary<StatusAsset>.a<string>(new string[]
			{
				"evillaw_tgc", "evillaw_devour", "evillaw_tc", "evillaw_starvation", "evillaw_disease", "evillaw_moneylaw", "evillaw_ea", "evillaw_sleeping", "evillaw_sterilization", "evillaw_tantrum", "evillaw_seduction", "evillaw_ew", "evil"
			});
			Evil_00.base_stats = new BaseStats();
			addToLocalizedLibrary("ch",Evil_00.id, "邪惡", "");
			addToLocalizedLibrary("en",Evil_00.id, "Evil", "");
			AssetManager.status.add(Evil_00);
			#endregion
		}
		public static bool EvilLawGet_StatusEffects01(BaseSimObject pTarget, WorldTile pTile = null)
		{// 劍 認證
			// 1. 基本安全檢查：確保 pTarget 存在且有 Actor 組件且存活
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false; 
			}
			Actor targetActor = pTarget.a;
			// 2. 主要邏輯判斷：如果單位還沒有 特質
			if (!targetActor.hasTrait("evillaw_ew"))
			{
				// 給予特質 (因為沒有這個特質，所以直接給予)
				targetActor.addTrait("evillaw_ew");
				targetActor.data.health += 99999;
			}
			// 如果單位已經有特質，或者剛被賦予特質，都直接返回 true
			return true;
		}
		public static bool EvilLawGet_StatusEffects02(BaseSimObject pTarget, WorldTile pTile = null)
		{// 銃 認證
			// 1. 基本安全檢查：確保 pTarget 存在且有 Actor 組件且存活
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false; 
			}
			Actor targetActor = pTarget.a;
			// 2. 主要邏輯判斷：如果單位還沒有  特質
			if (!targetActor.hasTrait("evillaw_moneylaw"))
			{
				// 給予特質 (因為沒有這個特質，所以直接給予)
				targetActor.addTrait("evillaw_moneylaw");
				targetActor.data.health += 99999;
				targetActor.data.money += 4760;
			}
			// 如果單位已經有特質，或者剛被賦予特質，都直接返回 true
			return true;
		}
		public static bool EvilLawGet_StatusEffects03(BaseSimObject pTarget, WorldTile pTile = null)
		{// 弓 認證
			// 1. 基本安全檢查：確保 pTarget 存在且有 Actor 組件且存活
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false; 
			}
			Actor targetActor = pTarget.a;
			// 2. 主要邏輯判斷：如果單位還沒有  特質
			if (!targetActor.hasTrait("evillaw_seduction"))
			{
				// 給予特質 (因為沒有這個特質，所以直接給予)
				targetActor.addTrait("evillaw_seduction");
				targetActor.data.health += 99999;
			}
			// 如果單位已經有特質，或者剛被賦予特質，都直接返回 true
			return true;
		}
		public static bool EvilLawGet_StatusEffects04(BaseSimObject pTarget, WorldTile pTile = null)
		{// 拳 認證
			// 1. 基本安全檢查：確保 pTarget 存在且有 Actor 組件且存活
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false; 
			}
			Actor targetActor = pTarget.a;
			// 2. 主要邏輯判斷：如果單位還沒有  特質
			if (!targetActor.hasTrait("evillaw_tantrum"))
			{
				// 給予特質 (因為沒有這個特質，所以直接給予)
				targetActor.addTrait("evillaw_tantrum");
				targetActor.data.health += 99999;
			}
			// 如果單位已經有特質，或者剛被賦予特質，都直接返回 true
			return true;
		}
		public static bool EvilLawGet_StatusEffects05(BaseSimObject pTarget, WorldTile pTile = null)
		{// 槍 認證
			// 1. 基本安全檢查：確保 pTarget 存在且有 Actor 組件且存活
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false; 
			}
			Actor targetActor = pTarget.a;
			// 2. 主要邏輯判斷：如果單位還沒有  特質
			if (!targetActor.hasTrait("evillaw_starvation"))
			{
				// 給予特質 (因為沒有這個特質，所以直接給予)
				targetActor.addTrait("evillaw_starvation");
				targetActor.data.health += 99999;
			}
			// 如果單位已經有特質，或者剛被賦予特質，都直接返回 true
			return true;
		}
		public static bool EvilLawGet_StatusEffects06(BaseSimObject pTarget, WorldTile pTile = null)
		{// 杖 認證
			// 1. 基本安全檢查：確保 pTarget 存在且有 Actor 組件且存活
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false; 
			}
			Actor targetActor = pTarget.a;
			// 2. 主要邏輯判斷：如果單位還沒有  特質
			if (!targetActor.hasTrait("evillaw_sleeping"))
			{
				// 給予特質 (因為沒有這個特質，所以直接給予)
				targetActor.addTrait("evillaw_sleeping");
				targetActor.data.health += 99999;
			}
			// 如果單位已經有特質，或者剛被賦予特質，都直接返回 true
			return true;
		}
		public static bool EvilLawGet_StatusEffects07(BaseSimObject pTarget, WorldTile pTile = null)
		{// 刃 認證
			// 1. 基本安全檢查：確保 pTarget 存在且有 Actor 組件且存活
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
			{
				return false; 
			}
			Actor targetActor = pTarget.a;
			// 2. 主要邏輯判斷：如果單位還沒有  特質
			if (!targetActor.hasTrait("evillaw_devour"))
			{
				// 給予特質 (因為沒有這個特質，所以直接給予)
				targetActor.addTrait("evillaw_devour");
				targetActor.data.health += 99999;
			}
			// 如果單位已經有特質，或者剛被賦予特質，都直接返回 true
			return true;
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
		public static bool spawnGhost(BaseSimObject pTarget, WorldTile pTile = null)
		{// 靈魂 效果
			// 確保 pTarget 是有效的 Actor
			if (pTarget == null || !pTarget.isActor())
			{
				return false;
			}
			Actor targetActor = pTarget.a;
			if (!targetActor.asset.has_soul)
			{
				Actor tGhost = World.world.units.createNewUnit("ghost", pTile, false, 0f, null, null, true, false, false, false);
				ActorTool.copyUnitToOtherUnit(pTarget.a, tGhost, true);
				tGhost.removeTrait("blessed");
				tGhost.removeTrait("holyarts_consecration");
				tGhost.removeTrait("monste_nest001");
				tGhost.removeTrait("monste_nest002");
				tGhost.removeTrait("monste_nest003");
				tGhost.removeTrait("monste_nest004");
				tGhost.removeTrait("monste_nest005");
				tGhost.removeTrait("monste_nest006");
				tGhost.removeTrait("monste_nest007");
				return true;
			}
			return false;
		}


	//魔王狀態特殊添加
		public static bool cdt_adk01Finish(BaseSimObject pTarget, WorldTile pTile = null)
		{// 傲慢 cdt_adk01 結束效果
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;
			Actor selfActor = pTarget.a;
			string actorID = selfActor.asset.id;			
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
			// ===================================
			// 根據是否存在其他魔王來決定行為
			if (!hasOtherDemonKing)
			{
				selfActor.addStatusEffect("arrogant_demon_king", 3600f);
				Traits01Actions.EvilSwordGet(pTarget, pTile);
				Items01Actions.EvilSwordAwakens(pTarget, pTile);
				return true;
			}
			return false;
		}
		public static bool WrathStart(BaseSimObject pTarget, WorldTile pTile = null)
		{// 憤怒開始
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;
			Actor selfActor = pTarget.a;
			string actorID = selfActor.asset.id;
			selfActor.removeTrait("slave");
			return true;
		}
		public static bool WrathFinish(BaseSimObject pTarget, WorldTile pTile = null)
		{// 憤怒結束
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;
			Actor selfActor = pTarget.a;
			string actorID = selfActor.asset.id;
			// 移除 madness 特質
			selfActor.removeTrait("madness");
			selfActor.finishStatusEffect("tantrum");
			selfActor.finishStatusEffect("angry");
			selfActor.finishStatusEffect("item_cdt01");
			selfActor.addStatusEffect("calm", 60f);
			return true;
		}
		public static bool ApostleUnitEND2(BaseSimObject pTarget, WorldTile pTile = null)
		{// 使徒效果 (生成終了)
			// 1. 基本安全檢查：確保目標存在且為有效的 Actor 實例
			//    使用 pTarget?.a 進行 null 傳播檢查，簡化了程式碼
			if (pTarget?.a == null)
			{
				return false;
			}
			
			Actor apostle = pTarget.a;
			ActionLibrary.removeUnit(apostle);
			
			return true;
		}
		public static bool GluttonyStart(BaseSimObject pTarget, WorldTile pTile = null)
		{// 暴食開始
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;
			Actor selfActor = pTarget.a;
			string actorID = selfActor.asset.id; // 或者使用 selfActor.uid.ToString() 如果你想基於實例ID
			selfActor.addTrait("acid_blood");
			selfActor.addTrait("acid_proof");
			return true;
		}
		public static bool addOther6661(BaseSimObject pTarget, WorldTile pTile = null)
		{// 用於開始
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;
			Actor selfActor = pTarget.a;
			if (selfActor.hasTrait("other6662") || selfActor.hasTrait("other6663") || selfActor.hasTrait("other6664") ||
				selfActor.hasTrait("other6665") || selfActor.hasTrait("other6666") || selfActor.hasTrait("other6667") ||
				selfActor.hasTrait("other6668") || selfActor.hasTrait("other6669"))
			{

				return false;
			}
			selfActor.addTrait("other6661");
			selfActor.addStatusEffect("dm6661", 400);
			return true;
		}
		public static bool dm6661_Manage(BaseSimObject pTarget, WorldTile pTile = null)
		{//
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;
			Actor selfActor = pTarget.a;
			selfActor.removeTrait("other6661");
			selfActor.addTrait("other6662");
			selfActor.addStatusEffect("dm6662", 400);
			return true;
		}
		public static bool dm6662_Manage(BaseSimObject pTarget, WorldTile pTile = null)
		{//
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;
			Actor selfActor = pTarget.a;
			selfActor.removeTrait("other6662");
			selfActor.addTrait("other6663");
			selfActor.addStatusEffect("dm6663", 400);
			return true;
		}
		public static bool dm6663_Manage(BaseSimObject pTarget, WorldTile pTile = null)
		{//
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;
			Actor selfActor = pTarget.a;
			selfActor.removeTrait("other6663");
			selfActor.addTrait("other6664");
			selfActor.addStatusEffect("dm6664", 400);
			return true;
		}
		public static bool dm6664_Manage(BaseSimObject pTarget, WorldTile pTile = null)
		{//
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;
			Actor selfActor = pTarget.a;
			selfActor.removeTrait("other6664");
			selfActor.addTrait("other6665");
			selfActor.addStatusEffect("dm6665", 400);
			return true;
		}
		public static bool dm6665_Manage(BaseSimObject pTarget, WorldTile pTile = null)
		{//
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;
			Actor selfActor = pTarget.a;
			selfActor.removeTrait("other6665");
			selfActor.addTrait("other6666");
			selfActor.addStatusEffect("dm6666", 400);
			return true;
		}
		public static bool dm6666_Manage(BaseSimObject pTarget, WorldTile pTile = null)
		{//
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;
			Actor selfActor = pTarget.a;
			selfActor.removeTrait("other6666");
			selfActor.addTrait("other6667");
			selfActor.addStatusEffect("dm6667", 400);
			return true;
		}
		public static bool dm6667_Manage(BaseSimObject pTarget, WorldTile pTile = null)
		{//
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;
			Actor selfActor = pTarget.a;
			selfActor.removeTrait("other6667");
			selfActor.addTrait("other6668");
			selfActor.addStatusEffect("dm6668", 400);
			return true;
		}
		public static bool dm6668_Manage(BaseSimObject pTarget, WorldTile pTile = null)
		{//
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;
			Actor selfActor = pTarget.a;
			selfActor.removeTrait("other6668");
			selfActor.addTrait("other6669");
			selfActor.addStatusEffect("dm6669", 400);
			return true;
		}
		public static bool dm6669_Manage(BaseSimObject pTarget, WorldTile pTile = null)
		{//
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;
			Actor selfActor = pTarget.a;
			selfActor.removeTrait("other6669");
			return true;
		}
		public static bool removeOther6669(BaseSimObject pTarget, WorldTile pTile = null)
		{// 用於結束
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;
			Actor selfActor = pTarget.a;
			selfActor.removeTrait("other6669");
			selfActor.finishStatusEffect("item_cdt00");
			selfActor.finishStatusEffect("item_cdt01");
			selfActor.finishStatusEffect("item_cdt02");
			selfActor.finishStatusEffect("item_cdt03");
			selfActor.finishStatusEffect("item_cdt04");
			selfActor.finishStatusEffect("item_cdt05");
			return true;
		}
		public static bool Brave_00_Manage(BaseSimObject pTarget, WorldTile pTile = null)
		{// 用於結束
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;
			Actor selfActor = pTarget.a;
			selfActor.removeTrait("hope");
			return true;
		}
		public static bool Change01(BaseSimObject pTarget, WorldTile pTile = null)
		{//
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;
			Actor selfActor = pTarget.a;
			Traits01Actions.TransformGender(pTarget, pTile);
			return true;
		}
		public static bool goldcooldown01(BaseSimObject pTarget, WorldTile pTile = null)
		{//
			if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;
			Actor selfActor = pTarget.a;
			Traits01Actions.CoinsIncrease(pTarget, pTile);
			return true;
		}
		public static bool addDamageStatusEffects(BaseSimObject pTarget, WorldTile pTile = null)
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
				//Dictionary<string, string> localizedText = Reflection.GetField(LocalizedTextManager.instance.GetType(), LocalizedTextManager.instance, "localizedText") as Dictionary<string, string>;
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