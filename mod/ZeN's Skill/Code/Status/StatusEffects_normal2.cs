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
    class StatusEffects_normal2
    { 
		private static readonly float maxDuration = 99999f;
        public static void init()
        {
/*			//樣本
			var Status_test01 = new StatusAsset();
			Status_test01.id = "Status_test01";												//ID
			Status_test01.duration = maxDuration;													//倒數計時
			Status_test01.path_icon = "ui/icons/Effects1/M000";								//效果圖標
			//動畫相關↓↓↓↓↓
			Status_test01.animated = false;													//是否有動畫	true / false	非必要
			Status_test01.is_animated_in_pause = false;										//能否暫停		true / false	非必要 遊戲暫停時，這個狀態效果的動畫是否繼續播放。
			Status_test01.can_be_flipped = false;											//可以翻轉		true / false	非必要
			Status_test01.use_parent_rotation = false;										//使用父級翻轉	true / false	非必要
			var test01Sprite = Resources.Load<Sprite>("effects/Status_test01"); 			//資料夾位置					非必要
			Status_test01.sprite_list = new Sprite[] { test01Sprite };						//動畫幀來源					非必要
			//動畫相關↑↑↑↑↑
			Status_test01.locale_id = $"status_title_{Status_test01.id}";					//區域設定ID					非必要
			Status_test01.locale_description = $"status_description_{Status_test01.id}";	//區域設定描述非必要			非必要
			Status_test01.tier = StatusTier.Advanced;										//狀態層級						非必要
										 //Advanced	高級
										 //Basic 	基礎
			Status_test01.removed_on_damage = false;										//損壞時移除	true / false	根據情況調整
			Status_test01.opposite_status = new string[] { "", "" };				//對立
			Status_test01.remove_status = new string[] { "", "" };					//移除,對下位狀態
			Status_test01.base_stats = new BaseStats();										//添加數值
			addToLocalizedLibrary("ch",Status_test01.id, "『中文名稱1』", "『中文描述1』");
			addToLocalizedLibrary("en",Status_test01.id, "『ENname』", "『ENdescription』");
			AssetManager.status.add(Status_test01);*/
			
			//ⅠⅡⅢⅣⅤⅥⅦⅧⅨⅩ

			//MP Ⅰ
			var mp01 = new StatusAsset();
			mp01.id = "mp01";												//ID
			mp01.duration = maxDuration;													//倒數計時
			mp01.path_icon = "ui/icons/Effects1/M000";								//效果圖標
			mp01.locale_id = $"status_title_{mp01.id}";					//區域設定ID					非必要
			mp01.locale_description = $"status_description_{mp01.id}";	//區域設定描述非必要			非必要
			mp01.tier = StatusTier.Advanced;
			mp01.opposite_status = new string[] { "", "mp02", "mp03", "mp04", "mp05", "mp06", "mp07", "mp08", "mp09", "mp10" };				//對立
			mp01.base_stats = new BaseStats();										//添加數值
			mp01.base_stats.set("multiplier_lifespan", 0.50f);					//壽命 %
			mp01.base_stats.set("multiplier_damage", 0.50f);					//傷害 %
			mp01.base_stats.set("damage_range", 0.50f);							//傷害區間 [未顯示]
			mp01.base_stats.set("armor", 10f);									//防禦
			mp01.base_stats.set("multiplier_attack_speed", 0.50f);				//攻擊速度 %
			mp01.base_stats.set("multiplier_speed", 0.50f);						//移動速度 %
			mp01.base_stats.set("critical_chance", 0.50f);						//爆擊機率 %
			mp01.base_stats.set("critical_damage_multiplier", 0.50f);			//重擊 %
			addToLocalizedLibrary("ch",mp01.id, "金錢之力", "Ⅰ\n 積蓄金額已超過550$");
			addToLocalizedLibrary("en",mp01.id, "Money Power", "Ⅰ\n The savings amount has exceeded $550");
			AssetManager.status.add(mp01);

			//MP Ⅱ
			var mp02 = new StatusAsset();
			mp02.id = "mp02";												//ID
			mp02.duration = maxDuration;													//倒數計時
			mp02.path_icon = "ui/icons/Effects1/M000";								//效果圖標
			mp02.locale_id = $"status_title_{mp02.id}";					//區域設定ID					非必要
			mp02.locale_description = $"status_description_{mp02.id}";	//區域設定描述非必要			非必要
			mp02.tier = StatusTier.Advanced;
			mp02.opposite_status = new string[] { "", "mp03", "mp04", "mp05", "mp06", "mp07", "mp08", "mp09", "mp10" };				//對立
			mp02.remove_status = new string[] { "mp01", "" };					//移除,對下位狀態
			mp02.base_stats = new BaseStats();										//添加數值
			mp02.base_stats.set("multiplier_lifespan", 0.60f);					//壽命 %
			mp02.base_stats.set("multiplier_damage", 0.60f);					//傷害 %
			mp02.base_stats.set("damage_range", 0.55f);							//傷害區間 [未顯示]
			mp02.base_stats.set("armor", 20f);									//防禦
			mp02.base_stats.set("multiplier_attack_speed", 0.60f);				//攻擊速度 %
			mp02.base_stats.set("multiplier_speed", 0.60f);						//移動速度 %
			mp02.base_stats.set("critical_chance", 0.60f);						//爆擊機率 %
			mp02.base_stats.set("critical_damage_multiplier", 0.60f);			//重擊 %
			addToLocalizedLibrary("ch",mp02.id, "金錢之力", "Ⅱ\n 積蓄金額已超過 1200$");
			addToLocalizedLibrary("en",mp02.id, "Money Power", "Ⅱ\n The savings amount has exceeded 1200$");
			AssetManager.status.add(mp02);

			//MP Ⅲ
			var mp03 = new StatusAsset();
			mp03.id = "mp03";												//ID
			mp03.duration = maxDuration;													//倒數計時
			mp03.path_icon = "ui/icons/Effects1/M000";								//效果圖標
			mp03.locale_id = $"status_title_{mp03.id}";					//區域設定ID					非必要
			mp03.locale_description = $"status_description_{mp03.id}";	//區域設定描述非必要			非必要
			mp03.tier = StatusTier.Advanced;
			mp03.opposite_status = new string[] { "", "mp04", "mp05", "mp06", "mp07", "mp08", "mp09", "mp10" };				//對立
			mp03.remove_status = new string[] { "mp01", "mp02", "", "", "", "", "", "", "", "" };					//移除,對下位狀態
			mp03.base_stats = new BaseStats();										//添加數值
			mp03.base_stats.set("multiplier_lifespan", 0.70f);					//壽命 %
			mp03.base_stats.set("multiplier_damage", 0.70f);					//傷害 %
			mp03.base_stats.set("damage_range", 0.60f);							//傷害區間 [未顯示]
			mp03.base_stats.set("armor", 30f);									//防禦
			mp03.base_stats.set("multiplier_attack_speed", 0.70f);				//攻擊速度 %
			mp03.base_stats.set("multiplier_speed", 0.70f);						//移動速度 %
			mp03.base_stats.set("critical_chance", 0.70f);						//爆擊機率 %
			mp03.base_stats.set("critical_damage_multiplier", 0.70f);			//重擊 %
			addToLocalizedLibrary("ch",mp03.id, "金錢之力", "Ⅲ\n 積蓄金額已超過 1950$");
			addToLocalizedLibrary("en",mp03.id, "Money Power", "Ⅲ\n The savings amount has exceeded 1950$");
			AssetManager.status.add(mp03);

			//MP Ⅳ
			var mp04 = new StatusAsset();
			mp04.id = "mp04";												//ID
			mp04.duration = maxDuration;													//倒數計時
			mp04.path_icon = "ui/icons/Effects1/M000";								//效果圖標
			mp04.locale_id = $"status_title_{mp04.id}";					//區域設定ID					非必要
			mp04.locale_description = $"status_description_{mp04.id}";	//區域設定描述非必要			非必要
			mp04.tier = StatusTier.Advanced;
			mp04.opposite_status = new string[] { "", "", "", "", "mp05", "mp06", "mp07", "mp08", "mp09", "mp10" };				//對立
			mp04.remove_status = new string[] { "mp01", "mp02", "mp03", "", "", "", "", "", "", "" };					//移除,對下位狀態
			mp04.base_stats = new BaseStats();										//添加數值
			mp04.base_stats.set("multiplier_lifespan", 0.80f);					//壽命 %
			mp04.base_stats.set("multiplier_damage", 0.80f);					//傷害 %
			mp04.base_stats.set("damage_range", 0.65f);							//傷害區間 [未顯示]
			mp04.base_stats.set("armor", 40f);									//防禦
			mp04.base_stats.set("multiplier_attack_speed", 0.80f);				//攻擊速度 %
			mp04.base_stats.set("multiplier_speed", 0.80f);						//移動速度 %
			mp04.base_stats.set("critical_chance", 0.80f);						//爆擊機率 %
			mp04.base_stats.set("critical_damage_multiplier", 0.80f);			//重擊 %
			addToLocalizedLibrary("ch",mp04.id, "金錢之力", "Ⅳ\n 積蓄金額已超過 2800$");
			addToLocalizedLibrary("en",mp04.id, "Money Power", "Ⅳ\n The savings amount has exceeded 2800$");
			AssetManager.status.add(mp04);

			//MP Ⅴ
			var mp05 = new StatusAsset();
			mp05.id = "mp05";												//ID
			mp05.duration = maxDuration;													//倒數計時
			mp05.path_icon = "ui/icons/Effects1/M000";								//效果圖標
			mp05.locale_id = $"status_title_{mp05.id}";					//區域設定ID					非必要
			mp05.locale_description = $"status_description_{mp05.id}";	//區域設定描述非必要			非必要
			mp05.tier = StatusTier.Advanced;
			mp05.opposite_status = new string[] { "", "", "", "", "", "mp06", "mp07", "mp08", "mp09", "mp10" };				//對立
			mp05.remove_status = new string[] { "mp01", "mp02", "mp03", "mp04", "", "", "", "", "", "" };					//移除,對下位狀態
			mp05.base_stats = new BaseStats();										//添加數值
			mp05.base_stats.set("multiplier_lifespan", 0.90f);					//壽命 %
			mp05.base_stats.set("multiplier_damage", 0.90f);					//傷害 %
			mp05.base_stats.set("damage_range", 0.70f);							//傷害區間 [未顯示]
			mp05.base_stats.set("armor", 50f);									//防禦
			mp05.base_stats.set("multiplier_attack_speed", 0.90f);				//攻擊速度 %
			mp05.base_stats.set("multiplier_speed", 0.90f);						//移動速度 %
			mp05.base_stats.set("critical_chance", 0.90f);						//爆擊機率 %
			mp05.base_stats.set("critical_damage_multiplier", 0.90f);			//重擊 %
			addToLocalizedLibrary("ch",mp05.id, "金錢之力", "Ⅴ\n 積蓄金額已超過 3750$");
			addToLocalizedLibrary("en",mp05.id, "Money Power", "Ⅴ\n 3750$");
			AssetManager.status.add(mp05);

			//MP Ⅵ
			var mp06 = new StatusAsset();
			mp06.id = "mp06";												//ID
			mp06.duration = maxDuration;													//倒數計時
			mp06.path_icon = "ui/icons/Effects1/M000";								//效果圖標
			mp06.locale_id = $"status_title_{mp06.id}";					//區域設定ID					非必要
			mp06.locale_description = $"status_description_{mp06.id}";	//區域設定描述非必要			非必要
			mp06.tier = StatusTier.Advanced;
			mp06.opposite_status = new string[] { "", "", "", "", "", "", "mp07", "mp08", "mp09", "mp10" };				//對立
			mp06.remove_status = new string[] { "mp01", "mp02", "mp03", "mp04", "mp05", "", "", "", "", "" };					//移除,對下位狀態
			mp06.base_stats = new BaseStats();										//添加數值
			mp06.base_stats.set("multiplier_lifespan", 1.00f);					//壽命 %
			mp06.base_stats.set("multiplier_damage", 1.00f);					//傷害 %
			mp06.base_stats.set("damage_range", 0.75f);							//傷害區間 [未顯示]
			mp06.base_stats.set("armor", 60f);									//防禦
			mp06.base_stats.set("multiplier_attack_speed", 1.00f);				//攻擊速度 %
			mp06.base_stats.set("multiplier_speed", 1.00f);						//移動速度 %
			mp06.base_stats.set("critical_chance", 1.00f);						//爆擊機率 %
			mp06.base_stats.set("critical_damage_multiplier", 1.00f);			//重擊 %
			addToLocalizedLibrary("ch",mp06.id, "金錢之力", "Ⅵ\n 積蓄金額已超過 4800$");
			addToLocalizedLibrary("en",mp06.id, "Money Power", "Ⅵ\n The savings amount has exceeded 4800$");
			AssetManager.status.add(mp06);

			//MP Ⅶ
			var mp07 = new StatusAsset();
			mp07.id = "mp07";												//ID
			mp07.duration = maxDuration;													//倒數計時
			mp07.path_icon = "ui/icons/Effects1/M000";								//效果圖標
			mp07.locale_id = $"status_title_{mp07.id}";					//區域設定ID					非必要
			mp07.locale_description = $"status_description_{mp07.id}";	//區域設定描述非必要			非必要
			mp07.tier = StatusTier.Advanced;
			mp07.opposite_status = new string[] { "", "", "", "", "", "", "", "mp08", "mp09", "mp10" };				//對立
			mp07.remove_status = new string[] { "mp01", "mp02", "mp03", "mp04", "mp05", "mp06", "", "", "", "" };					//移除,對下位狀態
			mp07.base_stats = new BaseStats();										//添加數值
			mp07.base_stats.set("multiplier_lifespan", 1.10f);					//壽命 %
			mp07.base_stats.set("multiplier_damage", 1.10f);					//傷害 %
			mp07.base_stats.set("damage_range", 0.80f);							//傷害區間 [未顯示]
			mp07.base_stats.set("armor", 70f);									//防禦
			mp07.base_stats.set("multiplier_attack_speed", 1.10f);				//攻擊速度 %
			mp07.base_stats.set("multiplier_speed", 1.10f);						//移動速度 %
			mp07.base_stats.set("critical_chance", 1.10f);						//爆擊機率 %
			mp07.base_stats.set("critical_damage_multiplier", 1.10f);			//重擊 %
			addToLocalizedLibrary("ch",mp07.id, "金錢之力", "Ⅶ\n 積蓄金額已超過 5950$");
			addToLocalizedLibrary("en",mp07.id, "Money Power", "Ⅶ\n The savings amount has exceeded 5950$");
			AssetManager.status.add(mp07);

			//MP Ⅷ
			var mp08 = new StatusAsset();
			mp08.id = "mp08";												//ID
			mp08.duration = maxDuration;													//倒數計時
			mp08.path_icon = "ui/icons/Effects1/M000";								//效果圖標
			mp08.locale_id = $"status_title_{mp08.id}";					//區域設定ID					非必要
			mp08.locale_description = $"status_description_{mp08.id}";	//區域設定描述非必要			非必要
			mp08.tier = StatusTier.Advanced;
			mp08.opposite_status = new string[] { "", "", "", "", "", "", "", "", "mp09", "mp10" };				//對立
			mp08.remove_status = new string[] { "mp01", "mp02", "mp03", "mp04", "mp05", "mp06", "mp07", "", "", "" };					//移除,對下位狀態
			mp08.base_stats = new BaseStats();										//添加數值
			mp08.base_stats.set("multiplier_lifespan", 1.20f);					//壽命 %
			mp08.base_stats.set("multiplier_damage", 1.20f);					//傷害 %
			mp08.base_stats.set("damage_range", 0.85f);							//傷害區間 [未顯示]
			mp08.base_stats.set("armor", 80f);									//防禦
			mp08.base_stats.set("multiplier_attack_speed", 1.20f);				//攻擊速度 %
			mp08.base_stats.set("multiplier_speed", 1.20f);						//移動速度 %
			mp08.base_stats.set("critical_chance", 1.20f);						//爆擊機率 %
			mp08.base_stats.set("critical_damage_multiplier", 1.20f);			//重擊 %
			addToLocalizedLibrary("ch",mp08.id, "金錢之力", "Ⅷ\n 積蓄金額已超過 7200$");
			addToLocalizedLibrary("en",mp08.id, "Money Power", "Ⅷ\n The savings amount has exceeded 7200$");
			AssetManager.status.add(mp08);

			//MP Ⅸ
			var mp09 = new StatusAsset();
			mp09.id = "mp09";												//ID
			mp09.duration = maxDuration;													//倒數計時
			mp09.path_icon = "ui/icons/Effects1/M000";								//效果圖標
			mp09.locale_id = $"status_title_{mp09.id}";					//區域設定ID					非必要
			mp09.locale_description = $"status_description_{mp09.id}";	//區域設定描述非必要			非必要
			mp09.tier = StatusTier.Advanced;
			mp09.opposite_status = new string[] { "", "", "", "", "", "", "", "", "", "mp10" };				//對立
			mp09.remove_status = new string[] { "mp01", "mp02", "mp03", "mp04", "mp05", "mp06", "mp07", "mp08", "", "" };
			mp09.base_stats = new BaseStats();										//添加數值
			mp09.base_stats.set("multiplier_lifespan", 1.30f);					//壽命 %
			mp09.base_stats.set("multiplier_damage", 1.30f);					//傷害 %
			mp09.base_stats.set("damage_range", 0.90f);							//傷害區間 [未顯示]
			mp09.base_stats.set("armor", 90f);									//防禦
			mp09.base_stats.set("multiplier_attack_speed", 1.30f);				//攻擊速度 %
			mp09.base_stats.set("multiplier_speed", 1.30f);						//移動速度 %
			mp09.base_stats.set("critical_chance", 1.30f);						//爆擊機率 %
			mp09.base_stats.set("critical_damage_multiplier", 1.30f);			//重擊 %
			addToLocalizedLibrary("ch",mp09.id, "金錢之力", "Ⅸ\n 積蓄金額已超過 8550$");
			addToLocalizedLibrary("en",mp09.id, "Money Power", "Ⅸ\n The savings amount has exceeded 8550$");
			AssetManager.status.add(mp09);

			//MP Ⅹ
			var mp10 = new StatusAsset();
			mp10.id = "mp10";												//ID
			mp10.duration = maxDuration;													//倒數計時
			mp10.path_icon = "ui/icons/Effects1/M000";								//效果圖標
			mp10.locale_id = $"status_title_{mp10.id}";					//區域設定ID					非必要
			mp10.locale_description = $"status_description_{mp10.id}";	//區域設定描述非必要			非必要
			mp10.tier = StatusTier.Advanced;
			mp10.opposite_status = new string[] { "", "" };				//對立
			mp10.remove_status = new string[] { "mp01", "mp02", "mp03", "mp04", "mp05", "mp06", "mp07", "mp08", "mp09", "" };
			mp10.base_stats = new BaseStats();										//添加數值
			mp10.base_stats.set("multiplier_lifespan", 1.40f);					//壽命 %
			mp10.base_stats.set("multiplier_damage", 1.40f);					//傷害 %
			mp10.base_stats.set("damage_range", 0.95f);							//傷害區間 [未顯示]
			mp10.base_stats.set("armor", 100f);									//防禦
			mp10.base_stats.set("multiplier_attack_speed", 1.40f);				//攻擊速度 %
			mp10.base_stats.set("multiplier_speed", 1.40f);						//移動速度 %
			mp10.base_stats.set("critical_chance", 1.40f);						//爆擊機率 %
			mp10.base_stats.set("critical_damage_multiplier", 1.40f);			//重擊 %
			addToLocalizedLibrary("ch",mp10.id, "金錢之力", "Ⅹ\n 積蓄金額已超過 10000$");
			addToLocalizedLibrary("en",mp10.id, "Money Power", "Ⅹ\n The savings amount has exceeded 10000$");
			AssetManager.status.add(mp10);

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