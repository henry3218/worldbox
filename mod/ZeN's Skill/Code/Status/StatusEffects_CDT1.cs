using NeoModLoader.api.attributes;
using NeoModLoader.General;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Reflection;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using ReflectionUtility;
using HarmonyLib;
using ai;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.CanvasScaler;

namespace ZeN_01
{
	class StatusEffects_CDT1
	{	
		public static void init()
		{
			# region 淨空CDT
		//淨空型CDT
			var cdt_clear00 = new StatusAsset();
			cdt_clear00.id = "cdt_clear00";								 //ID
			cdt_clear00.duration = 60f;									 //倒數計時
			cdt_clear00.path_icon = "ui/icons/CDT/cdt_clear00";			 //效果圖標
			cdt_clear00.locale_id = $"status_title_{cdt_clear00.id}";	  //區域設定ID					非必要
			cdt_clear00.locale_description = $"status_description_{cdt_clear00.id}";		//區域設定描述非必要			非必要
			cdt_clear00.tier = StatusTier.None;
			// ======================================================================================================
			// 修正點：將所有要移除的狀態一次性放入一個字串陣列
			cdt_clear00.remove_status = new string[] 
			{ 
				"cdt_buff00", "cdt_give00",
				"cdt_atk00", "cdt_atk01", "cdt_atk02", "cdt_atk03", "cdt_atk04",
				"cdt_cure00", "cdt_cure01", "cdt_cure02", "cdt_cure03",
				"cdt_rain01", "cdt_rain02", "cdt_rain03", "cdt_rain04",
				"cdt_debuff00", "cdt_debuff01", "cdt_debuff02", "cdt_debuff03", "cdt_debuff04",
				"recovery_spell", "recovery_combat_action" 
			};
			// ======================================================================================================
			addToLocalizedLibrary("ch",cdt_clear00.id, "Time Reset", "不需要等待，即刻就能開始的超級時間");
			addToLocalizedLibrary("en",cdt_clear00.id, "Time Reset", "No need to wait, super time starts right away...");
			AssetManager.status.add(cdt_clear00);
			# endregion
			# region 干擾CDT
			var cdt_debuff00 = new StatusAsset();
			cdt_debuff00.id = "cdt_debuff00";								 //ID
			cdt_debuff00.duration = 60f;									 //倒數計時
			cdt_debuff00.path_icon = "ui/icons/CDT/cdt_debuff";				//效果圖標
			cdt_debuff00.locale_id = $"status_title_{cdt_debuff00.id}";		//區域設定ID					非必要
			cdt_debuff00.locale_description = $"status_description_{cdt_debuff00.id}";		//區域設定描述非必要			非必要
			cdt_debuff00.tier = StatusTier.None;
			cdt_debuff00.opposite_status = new string[] { "cdt_clear00" }; // 移除空字串 ""
			cdt_debuff00.remove_status = new string[] 
			{ 
				"cdt_atk00", "cdt_atk01", "cdt_atk02", "cdt_atk03", "cdt_atk04",
				"cdt_buff00", "cdt_give00", 
				"cdt_cure00", "cdt_cure01", "cdt_cure02", "cdt_cure03", 
				"cdt_rain01", "cdt_rain02", "cdt_rain03", "cdt_rain04",
				"cdt_nest01", "cdt_drop"
			};
			addToLocalizedLibrary("ch",cdt_debuff00.id, "封鎖", "只要它還在，部分特質效果就無法正常運作…");
			addToLocalizedLibrary("en",cdt_debuff00.id, "Blockade", "As long as it still has some trait effects, it will not work properly.");
			AssetManager.status.add(cdt_debuff00);

			var cdt_debuff01 = new StatusAsset();
			cdt_debuff01.id = "cdt_debuff01";								 //ID
			cdt_debuff01.duration = 60f;									 //倒數計時
			cdt_debuff01.path_icon = "ui/icons/CDT/cdt_debuff";				//效果圖標
			cdt_debuff01.locale_id = $"status_title_{cdt_debuff01.id}";		//區域設定ID					非必要
			cdt_debuff01.locale_description = $"status_description_{cdt_debuff01.id}";		//區域設定描述非必要			非必要
			cdt_debuff01.tier = StatusTier.None;
			cdt_debuff01.opposite_status = new string[] { "cdt_clear00" }; // 移除空字串 ""
			cdt_debuff01.remove_status = new string[] 
			{ 
				"cdt_atk00", "cdt_atk01", "cdt_atk02", "cdt_atk03", "cdt_atk04"
			};
			addToLocalizedLibrary("ch",cdt_debuff01.id, "封鎖", "只要它還在，攻擊特質效果就無法正常運作…");
			addToLocalizedLibrary("en",cdt_debuff01.id, "Blockade", "As long as it still has some trait effects, it will not work properly.");
			AssetManager.status.add(cdt_debuff01);

			var cdt_debuff02 = new StatusAsset();
			cdt_debuff02.id = "cdt_debuff02";								 //ID
			cdt_debuff02.duration = 60f;									 //倒數計時
			cdt_debuff02.path_icon = "ui/icons/CDT/cdt_debuff";				//效果圖標
			cdt_debuff02.locale_id = $"status_title_{cdt_debuff02.id}";		//區域設定ID					非必要
			cdt_debuff02.locale_description = $"status_description_{cdt_debuff02.id}";		//區域設定描述非必要			非必要
			cdt_debuff02.tier = StatusTier.None;
			cdt_debuff02.opposite_status = new string[] { "cdt_clear00" }; // 移除空字串 ""
			cdt_debuff02.remove_status = new string[] 
			{ 
				"cdt_buff00", "cdt_give00"
			};
			addToLocalizedLibrary("ch",cdt_debuff02.id, "封鎖", "只要它還在，強化特質效果就無法正常運作…");
			addToLocalizedLibrary("en",cdt_debuff02.id, "Blockade", "As long as it still has some trait effects, it will not work properly.");
			AssetManager.status.add(cdt_debuff02);

			var cdt_debuff03 = new StatusAsset();
			cdt_debuff03.id = "cdt_debuff03";								 //ID
			cdt_debuff03.duration = 60f;									 //倒數計時
			cdt_debuff03.path_icon = "ui/icons/CDT/cdt_debuff";				//效果圖標
			cdt_debuff03.locale_id = $"status_title_{cdt_debuff03.id}";		//區域設定ID					非必要
			cdt_debuff03.locale_description = $"status_description_{cdt_debuff03.id}";		//區域設定描述非必要			非必要
			cdt_debuff03.tier = StatusTier.None;
			cdt_debuff03.opposite_status = new string[] { "cdt_clear00" }; // 移除空字串 ""
			cdt_debuff03.remove_status = new string[] 
			{ 
				"cdt_cure00", "cdt_cure01", "cdt_cure02", "cdt_cure03", 
				"cdt_rain01", "cdt_rain02", "cdt_rain03", "cdt_rain04"
			};
			addToLocalizedLibrary("ch",cdt_debuff03.id, "封鎖", "只要它還在，部分恢復特質效果就無法正常運作…");
			addToLocalizedLibrary("en",cdt_debuff03.id, "Blockade", "As long as it still has some trait effects, it will not work properly.");
			AssetManager.status.add(cdt_debuff03);

			var cdt_debuff04 = new StatusAsset();
			cdt_debuff04.id = "cdt_debuff04";								 //ID
			cdt_debuff04.duration = 60f;									 //倒數計時
			cdt_debuff04.path_icon = "ui/icons/CDT/cdt_debuff";				//效果圖標
			cdt_debuff04.locale_id = $"status_title_{cdt_debuff04.id}";		//區域設定ID					非必要
			cdt_debuff04.locale_description = $"status_description_{cdt_debuff04.id}";		//區域設定描述非必要			非必要
			cdt_debuff04.tier = StatusTier.None;
			cdt_debuff04.opposite_status = new string[] { "cdt_clear00" }; // 移除空字串 ""
			cdt_debuff04.remove_status = new string[] 
			{ 
				"cdt_nest01", "cdt_drop"
			};
			addToLocalizedLibrary("ch",cdt_debuff04.id, "封鎖", "只要它還在，建築特質效果就無法正常運作…");
			addToLocalizedLibrary("en",cdt_debuff04.id, "Blockade", "As long as it still has some trait effects, it will not work properly.");
			AssetManager.status.add(cdt_debuff04);
			# endregion
			# region 攻擊CDT
		//攻擊狀態CDT
			var cdt_atk00 = new StatusAsset();
			cdt_atk00.id = "cdt_atk00";								 //ID
			cdt_atk00.duration = 60f;									 //倒數計時
			cdt_atk00.path_icon = "ui/icons/CDT/CDT000";				//效果圖標
			cdt_atk00.locale_id = $"status_title_{cdt_atk00.id}";		//區域設定ID					非必要
			cdt_atk00.locale_description = $"status_description_{cdt_atk00.id}";		//區域設定描述非必要			非必要
			cdt_atk00.tier = StatusTier.None;
			cdt_atk00.opposite_status = new string[] { "cdt_clear00", "cdt_debuff01" };								//移除,對下位狀態
			addToLocalizedLibrary("ch",cdt_atk00.id, "攻擊冷卻", "冷卻倒數,距離再次使用附加魔法還要…");
			addToLocalizedLibrary("en",cdt_atk00.id, "Atk CD", "Cooling down, how long will it take to use addmagic again...");
			AssetManager.status.add(cdt_atk00);

			var cdt_atk01 = new StatusAsset();
			cdt_atk01.id = "cdt_atk01";								 //ID
			cdt_atk01.duration = 60f;									 //倒數計時
			cdt_atk01.path_icon = "ui/icons/CDT/CDT000";				//效果圖標
			cdt_atk01.locale_id = $"status_title_{cdt_atk01.id}";		//區域設定ID					非必要
			cdt_atk01.locale_description = $"status_description_{cdt_atk01.id}";		//區域設定描述非必要			非必要
			cdt_atk01.tier = StatusTier.None;
			cdt_atk01.opposite_status = new string[] { "cdt_clear00", "cdt_debuff01" };								//移除,對下位狀態
			addToLocalizedLibrary("ch",cdt_atk01.id, "戰技冷卻", "冷卻倒數,距離再次發動戰技還要…");
			addToLocalizedLibrary("en",cdt_atk01.id, "Atk CD", "Cooling down, how long will it take to use addmagic again...");
			AssetManager.status.add(cdt_atk01);

			var cdt_atk02 = new StatusAsset();
			cdt_atk02.id = "cdt_atk02";								 //ID
			cdt_atk02.duration = 60f;									 //倒數計時
			cdt_atk02.path_icon = "ui/icons/CDT/CDT000";				//效果圖標
			cdt_atk02.locale_id = $"status_title_{cdt_atk02.id}";		//區域設定ID					非必要
			cdt_atk02.locale_description = $"status_description_{cdt_atk02.id}";		//區域設定描述非必要			非必要
			cdt_atk02.tier = StatusTier.None;
			cdt_atk02.opposite_status = new string[] { "cdt_clear00", "cdt_debuff01" };								//移除,對下位狀態
			addToLocalizedLibrary("ch",cdt_atk02.id, "戰技冷卻", "冷卻倒數,距離再次發動戰技還要…");
			addToLocalizedLibrary("en",cdt_atk02.id, "Atk CD", "Cooling down, how long will it take to use addmagic again...");
			AssetManager.status.add(cdt_atk02);

			var cdt_atk03 = new StatusAsset();
			cdt_atk03.id = "cdt_atk03";								 //ID
			cdt_atk03.duration = 60f;									 //倒數計時
			cdt_atk03.path_icon = "ui/icons/CDT/CDT000";				//效果圖標
			cdt_atk03.locale_id = $"status_title_{cdt_atk03.id}";		//區域設定ID					非必要
			cdt_atk03.locale_description = $"status_description_{cdt_atk03.id}";		//區域設定描述非必要			非必要
			cdt_atk03.tier = StatusTier.None;
			cdt_atk03.opposite_status = new string[] { "cdt_clear00", "cdt_debuff01" };								//移除,對下位狀態
			addToLocalizedLibrary("ch",cdt_atk03.id, "戰技冷卻", "冷卻倒數,距離再次發動戰技還要…");
			addToLocalizedLibrary("en",cdt_atk03.id, "Atk CD", "Cooling down, how long will it take to use addmagic again...");
			AssetManager.status.add(cdt_atk03);

			var cdt_atk04 = new StatusAsset();
			cdt_atk04.id = "cdt_atk04";								 //ID
			cdt_atk04.duration = 60f;									 //倒數計時
			cdt_atk04.path_icon = "ui/icons/CDT/CDT000";				//效果圖標
			cdt_atk04.locale_id = $"status_title_{cdt_atk04.id}";		//區域設定ID					非必要
			cdt_atk04.locale_description = $"status_description_{cdt_atk04.id}";		//區域設定描述非必要			非必要
			cdt_atk04.tier = StatusTier.None;
			cdt_atk04.opposite_status = new string[] { "cdt_clear00", "cdt_debuff01" };								//移除,對下位狀態
			addToLocalizedLibrary("ch",cdt_atk04.id, "射程冷卻", "冷卻倒數,距離再次擴張視野還要…");
			addToLocalizedLibrary("en",cdt_atk04.id, "Range Cooling", "Cooling down, It's still a long way to expand your horizon again...");
			AssetManager.status.add(cdt_atk04);

			var cdt_atk05 = new StatusAsset();
			cdt_atk05.id = "cdt_atk05";								 //ID
			cdt_atk05.duration = 60f;									 //倒數計時
			cdt_atk05.path_icon = "ui/icons/CDT/CDT000";				//效果圖標
			cdt_atk05.locale_id = $"status_title_{cdt_atk05.id}";		//區域設定ID					非必要
			cdt_atk05.locale_description = $"status_description_{cdt_atk05.id}";		//區域設定描述非必要			非必要
			cdt_atk05.tier = StatusTier.None;
			cdt_atk05.opposite_status = new string[] { "cdt_clear00", "cdt_debuff01" };								//移除,對下位狀態
			addToLocalizedLibrary("ch",cdt_atk05.id, "射擊冷卻", "冷卻倒數,距離再次射擊子彈還要…");
			addToLocalizedLibrary("en",cdt_atk05.id, "Atk CD", "Cooling down, It's still a while before we can fire the bullet again...");
			AssetManager.status.add(cdt_atk05);

			var cdt_call00 = new StatusAsset();
			cdt_call00.id = "cdt_call00";								 //ID
			cdt_call00.duration = 60f;									 //倒數計時
			cdt_call00.path_icon = "ui/icons/CDT/CDT000";				//效果圖標
			cdt_call00.locale_id = $"status_title_{cdt_call00.id}";		//區域設定ID					非必要
			cdt_call00.locale_description = $"status_description_{cdt_call00.id}";		//區域設定描述非必要			非必要
			cdt_call00.tier = StatusTier.None;
			//cdt_call00.opposite_status = new string[] { "cdt_clear00", "cdt_debuff01" };								//移除,對下位狀態
			addToLocalizedLibrary("ch",cdt_call00.id, "呼喚冷卻", "冷卻倒數,距離下次呼喚它們還需要…");
			addToLocalizedLibrary("en",cdt_call00.id, "Call CD", "Cooling down, It will take a while before we call them again....");
			AssetManager.status.add(cdt_call00);

			var cdt_call01 = new StatusAsset();
			cdt_call01.id = "cdt_call01";								 //ID
			cdt_call01.duration = 60f;									 //倒數計時
			cdt_call01.path_icon = "ui/icons/CDT/CDT000";				//效果圖標
			cdt_call01.locale_id = $"status_title_{cdt_call01.id}";		//區域設定ID					非必要
			cdt_call01.locale_description = $"status_description_{cdt_call01.id}";		//區域設定描述非必要			非必要
			cdt_call01.tier = StatusTier.None;
			//cdt_call01.opposite_status = new string[] { "cdt_clear00", "cdt_debuff01" };								//移除,對下位狀態
			addToLocalizedLibrary("ch",cdt_call01.id, "呼喚冷卻", "冷卻倒數,距離下次呼喚它們還需要…");
			addToLocalizedLibrary("en",cdt_call01.id, "Call CD", "Cooling down, It will take a while before we call them again....");
			AssetManager.status.add(cdt_call01);

			var Rebirth = new StatusAsset();
			Rebirth.id = "rebirth";								 //ID
			Rebirth.duration = 60f;									 //倒數計時
			Rebirth.path_icon = "ui/icons/CDT/CDT000";				//效果圖標
			Rebirth.locale_id = $"status_title_{Rebirth.id}";		//區域設定ID					非必要
			Rebirth.locale_description = $"status_description_{Rebirth.id}";		//區域設定描述非必要			非必要
			Rebirth.tier = StatusTier.None;
			//Rebirth.opposite_status = new string[] { "cdt_clear00", "cdt_debuff01" };								//移除,對下位狀態
			addToLocalizedLibrary("ch",Rebirth.id, "重生", "");
			addToLocalizedLibrary("en",Rebirth.id, "Rebirth", "");
			AssetManager.status.add(Rebirth);

			# endregion
			# region 強化CDT
		//強化狀態CDT
			var cdt_buff00 = new StatusAsset();
			cdt_buff00.id = "cdt_buff00";								 //ID
			cdt_buff00.duration = 60f;									 //倒數計時
			cdt_buff00.path_icon = "ui/icons/CDT/CDT000";				//效果圖標
			cdt_buff00.locale_id = $"status_title_{cdt_buff00.id}";		//區域設定ID					非必要
			cdt_buff00.locale_description = $"status_description_{cdt_buff00.id}";		//區域設定描述非必要			非必要
			cdt_buff00.tier = StatusTier.None;
			// 修正點：opposite_status 的設置也要確保包含所有對立狀態
			cdt_buff00.opposite_status = new string[] { "cdt_clear00", "cdt_debuff02" };
			addToLocalizedLibrary("ch",cdt_buff00.id, "強化冷卻", "冷卻倒數,距離下次執行還要…");
			addToLocalizedLibrary("en",cdt_buff00.id, "Buff CD", "Cooling down countdown, until next execution...");
			AssetManager.status.add(cdt_buff00);
			# endregion
			# region 給予CDT
		//給予狀態CDT
			var cdt_give00 = new StatusAsset();
			cdt_give00.id = "cdt_give00";								 //ID
			cdt_give00.duration = 60f;									 //倒數計時
			cdt_give00.path_icon = "ui/icons/CDT/CDT000";				//效果圖標
			cdt_give00.locale_id = $"status_title_{cdt_give00.id}";		//區域設定ID					非必要
			cdt_give00.locale_description = $"status_description_{cdt_give00.id}";		//區域設定描述非必要			非必要
			cdt_give00.tier = StatusTier.None;
			cdt_give00.opposite_status = new string[] { "cdt_clear00", "cdt_debuff02" };				//對立
			addToLocalizedLibrary("ch",cdt_give00.id, "贈與冷卻", "再次得到效果還需要…");
			addToLocalizedLibrary("en",cdt_give00.id, "Give CD", "To get the effect again, you need...");
			AssetManager.status.add(cdt_give00);
			# endregion
			# region 恢復CDT
		//恢復狀態CDT
			var cdt_cure00 = new StatusAsset();
			cdt_cure00.id = "cdt_cure00";								 //ID
			cdt_cure00.duration = 60f;									 //倒數計時
			cdt_cure00.path_icon = "ui/icons/CDT/CDT000";				//效果圖標
			cdt_cure00.locale_id = $"status_title_{cdt_cure00.id}";		//區域設定ID					非必要
			cdt_cure00.locale_description = $"status_description_{cdt_cure00.id}";		//區域設定描述非必要			非必要
			cdt_cure00.tier = StatusTier.None;
			cdt_cure00.opposite_status = new string[] { "cdt_clear00", "cdt_debuff03" };				//對立
			addToLocalizedLibrary("ch",cdt_cure00.id, "發動冷卻", "冷卻倒數,距離再次使用還要…");
			addToLocalizedLibrary("en",cdt_cure00.id, "Start Cooling", "Cooling down, it will take a while before it can be used again...");
			AssetManager.status.add(cdt_cure00);

			var cdt_cure01 = new StatusAsset();
			cdt_cure01.id = "cdt_cure01";								 //ID
			cdt_cure01.duration = 60f;									 //倒數計時
			cdt_cure01.path_icon = "ui/icons/CDT/CDT000";				//效果圖標
			cdt_cure01.locale_id = $"status_title_{cdt_cure01.id}";		//區域設定ID					非必要
			cdt_cure01.locale_description = $"status_description_{cdt_cure01.id}";		//區域設定描述非必要			非必要
			cdt_cure01.tier = StatusTier.None;
			cdt_cure01.opposite_status = new string[] { "cdt_clear00", "cdt_debuff03" };				//對立
			addToLocalizedLibrary("ch",cdt_cure01.id, "治療冷卻", "冷卻倒數,距離再次發動治療還要…");
			addToLocalizedLibrary("en",cdt_cure01.id, "Treatment CD", "Cooling down, it will take a while before the next treatment starts...");
			AssetManager.status.add(cdt_cure01);

			var cdt_cure02 = new StatusAsset();
			cdt_cure02.id = "cdt_cure02";								 //ID
			cdt_cure02.duration = 60f;									 //倒數計時
			cdt_cure02.path_icon = "ui/icons/CDT/CDT000";				//效果圖標
			cdt_cure02.locale_id = $"status_title_{cdt_cure02.id}";		//區域設定ID					非必要
			cdt_cure02.locale_description = $"status_description_{cdt_cure02.id}";		//區域設定描述非必要			非必要
			cdt_cure02.tier = StatusTier.None;
			cdt_cure02.opposite_status = new string[] { "cdt_clear00", "cdt_debuff03" };				//對立
			addToLocalizedLibrary("ch",cdt_cure02.id, "法術冷卻", "冷卻倒數,距離再次發動治癒魔法還要…");
			addToLocalizedLibrary("en",cdt_cure02.id, "Treatment CD", "Cooling down, it will take a while before the next treatment starts...");
			AssetManager.status.add(cdt_cure02);

			var cdt_cure03 = new StatusAsset();
			cdt_cure03.id = "cdt_cure03";								 //ID
			cdt_cure03.duration = 60f;									 //倒數計時
			cdt_cure03.path_icon = "ui/icons/CDT/CDT000";				//效果圖標
			cdt_cure03.locale_id = $"status_title_{cdt_cure03.id}";		//區域設定ID					非必要
			cdt_cure03.locale_description = $"status_description_{cdt_cure03.id}";		//區域設定描述非必要			非必要
			cdt_cure03.tier = StatusTier.None;
			cdt_cure03.opposite_status = new string[] { "cdt_clear00", "cdt_debuff03" };				//對立
			addToLocalizedLibrary("ch",cdt_cure03.id, "法術冷卻 ", "冷卻倒數,距離再次發動治癒魔法還要… ");
			addToLocalizedLibrary("en",cdt_cure03.id, "Treatment CD ", "Cooling down, it will take a while before the next treatment starts... ");
			AssetManager.status.add(cdt_cure03);

			var cdt_cure04 = new StatusAsset();
			cdt_cure04.id = "cdt_cure04";								 //ID
			cdt_cure04.duration = 60f;									 //倒數計時
			cdt_cure04.path_icon = "ui/icons/CDT/CDT000";				//效果圖標
			cdt_cure04.locale_id = $"status_title_{cdt_cure04.id}";		//區域設定ID					非必要
			cdt_cure04.locale_description = $"status_description_{cdt_cure04.id}";		//區域設定描述非必要			非必要
			cdt_cure04.tier = StatusTier.None;
			cdt_cure04.opposite_status = new string[] { "cdt_clear00", "cdt_debuff03" };				//對立
			addToLocalizedLibrary("ch",cdt_cure04.id, "法術冷卻 ", "冷卻倒數,距離再次發動聖光魔法還要… ");
			addToLocalizedLibrary("en",cdt_cure04.id, "Treatment CD ", "Cooling down, it will take a while before the next treatment starts... ");
			AssetManager.status.add(cdt_cure04);
			# endregion
			# region 降雨CDT
		//降雨
			var cdt_rain01 = new StatusAsset();
			cdt_rain01.id = "cdt_rain01";								 //ID
			cdt_rain01.duration = 60f;									 //倒數計時
			cdt_rain01.path_icon = "ui/icons/CDT/CDT000";			 //效果圖標
			cdt_rain01.locale_id = $"status_title_{cdt_rain01.id}";	  //區域設定ID					非必要
			cdt_rain01.locale_description = $"status_description_{cdt_rain01.id}";		//區域設定描述非必要			非必要
			cdt_rain01.tier = StatusTier.None;
			cdt_rain01.opposite_status = new string[] { "cdt_clear00", "cdt_debuff03" };
			addToLocalizedLibrary("ch",cdt_rain01.id, "灑水之後", "剛剛澆熄了被點燃的同胞");
			addToLocalizedLibrary("en",cdt_rain01.id, "After watering", "Just extinguished the burning compatriots.");
			AssetManager.status.add(cdt_rain01);

			var cdt_rain02 = new StatusAsset();
			cdt_rain02.id = "cdt_rain02";								 //ID
			cdt_rain02.duration = 60f;									 //倒數計時
			cdt_rain02.path_icon = "ui/icons/CDT/CDT000";			 //效果圖標
			cdt_rain02.locale_id = $"status_title_{cdt_rain02.id}";	  //區域設定ID					非必要
			cdt_rain02.locale_description = $"status_description_{cdt_rain02.id}";		//區域設定描述非必要			非必要
			cdt_rain02.tier = StatusTier.None;
			cdt_rain02.opposite_status = new string[] { "cdt_clear00", "cdt_debuff03" };
			addToLocalizedLibrary("ch",cdt_rain02.id, "灑水之後", "為了耕耘而降下雨水");
			addToLocalizedLibrary("en",cdt_rain02.id, "After watering", "Rain for farming.");
			AssetManager.status.add(cdt_rain02);

			var cdt_rain03 = new StatusAsset();
			cdt_rain03.id = "cdt_rain03";								 //ID
			cdt_rain03.duration = 60f;									 //倒數計時
			cdt_rain03.path_icon = "ui/icons/CDT/CDT000";			 //效果圖標
			cdt_rain03.locale_id = $"status_title_{cdt_rain03.id}";	  //區域設定ID					非必要
			cdt_rain03.locale_description = $"status_description_{cdt_rain03.id}";		//區域設定描述非必要			非必要
			cdt_rain03.tier = StatusTier.None;
			cdt_rain03.opposite_status = new string[] { "cdt_clear00", "cdt_debuff03" };
			addToLocalizedLibrary("ch",cdt_rain03.id, "灑水之後", "為了救火而降下雨水");
			addToLocalizedLibrary("en",cdt_rain03.id, "After watering", "Rain falls to put out fires.");
			AssetManager.status.add(cdt_rain03);

			var cdt_rain04 = new StatusAsset();
			cdt_rain04.id = "cdt_rain04";								 //ID
			cdt_rain04.duration = 60f;									 //倒數計時
			cdt_rain04.path_icon = "ui/icons/CDT/CDT000";			 //效果圖標
			cdt_rain04.locale_id = $"status_title_{cdt_rain04.id}";	  //區域設定ID					非必要
			cdt_rain04.locale_description = $"status_description_{cdt_rain04.id}";		//區域設定描述非必要			非必要
			cdt_rain04.tier = StatusTier.None;
			cdt_rain04.opposite_status = new string[] { "cdt_clear00", "cdt_debuff03" };
			addToLocalizedLibrary("ch",cdt_rain04.id, "灑水之後", "為了護身而降下雨水");
			addToLocalizedLibrary("en",cdt_rain04.id, "After watering", "Rain falls for protection.");
			AssetManager.status.add(cdt_rain04);
			# endregion
			# region 建築CDT
		//築巢
			var cdt_nest01 = new StatusAsset();
			cdt_nest01.id = "cdt_nest01";								 //ID
			cdt_nest01.duration = 60f;									 //倒數計時
			cdt_nest01.path_icon = "ui/icons/CDT/CDT000";			 //效果圖標
			cdt_nest01.locale_id = $"status_title_{cdt_nest01.id}";	  //區域設定ID					非必要
			cdt_nest01.locale_description = $"status_description_{cdt_nest01.id}";		//區域設定描述非必要			非必要
			cdt_nest01.tier = StatusTier.None;
			cdt_nest01.opposite_status = new string[] { "cdt_clear00", "cdt_debuff04" }; // 移除空字串 ""
			addToLocalizedLibrary("ch",cdt_nest01.id, "築巢之後", "要過一段時間才能再建立新巢穴");
			addToLocalizedLibrary("en",cdt_nest01.id, "After nesting", "It will take some time before a new nest can be established.");
			AssetManager.status.add(cdt_nest01);
		//掉落
			var cdt_drop = new StatusAsset();
			cdt_drop.id = "cdt_drop";								 //ID
			cdt_drop.duration = 60f;									 //倒數計時
			cdt_drop.path_icon = "ui/icons/CDT/CDT000";			 //效果圖標
			cdt_drop.locale_id = $"status_title_{cdt_drop.id}";	  //區域設定ID					非必要
			cdt_drop.locale_description = $"status_description_{cdt_drop.id}";		//區域設定描述非必要			非必要
			cdt_drop.tier = StatusTier.None;
			cdt_drop.opposite_status = new string[] { "cdt_clear00", "cdt_debuff04" }; // 移除空字串 ""
			addToLocalizedLibrary("ch",cdt_drop.id, "遠距建立", "剛剛在遠端建立了一些東西");
			addToLocalizedLibrary("en",cdt_drop.id, "Remote Construction", "Just built something remotely.");
			AssetManager.status.add(cdt_drop);
			# endregion
			# region 受胎效果專用
		//受胎
			var pregnancy = new StatusAsset();
			pregnancy.id = "pregnancy";								 //ID
			pregnancy.duration = 1200f;									 //倒數計時
			pregnancy.path_icon = "ui/icons/Effects1/pregnancy1";				//效果圖標
			pregnancy.locale_id = $"status_title_{pregnancy.id}";	//區域設定ID					非必要
			pregnancy.locale_description = $"status_description_{pregnancy.id}";	//區域設定描述非必要			非必要
			pregnancy.tier = StatusTier.Advanced;										//狀態層級						非必要
			pregnancy.opposite_status = new string[] { }; // 移除空字串 ""
			pregnancy.remove_status = new string[] { "pregnancy2" };// 移除
			// 如果這個狀態沒有任何對立或要移除的狀態，可以將陣列留空 `new string[] { }`
			addToLocalizedLibrary("ch",pregnancy.id, "聖力消退", "受孕之力以散去,再次開始還需要…");
			addToLocalizedLibrary("en",pregnancy.id, "Holy Power Fades", "The power of conception has dissipated, and starting over again requires...");
			AssetManager.status.add(pregnancy);

			var pregnancy2 = new StatusAsset();
			pregnancy2.id = "pregnancy2";								 //ID
			pregnancy2.duration = 1200f;									 //倒數計時
			pregnancy2.path_icon = "ui/icons/Effects1/pregnancy2";				//效果圖標
			pregnancy2.locale_id = $"status_title_{pregnancy2.id}";	//區域設定ID					非必要
			pregnancy2.locale_description = $"status_description_{pregnancy2.id}";	//區域設定描述非必要			非必要
			pregnancy2.tier = StatusTier.Advanced;										//狀態層級						非必要
			pregnancy2.opposite_status = new string[] { "pregnancy" }; // 對立
			pregnancy2.remove_status = new string[] { };	 // 移除空字串 ""
			// 如果這個狀態沒有任何對立或要移除的狀態，可以將陣列留空 `new string[] { }`
			addToLocalizedLibrary("ch",pregnancy2.id, "閉鎖", "孕育之力暫時無法進入。");
			addToLocalizedLibrary("en",pregnancy2.id, "Loop closed", "The power of fertility is temporarily unavailable.");
			AssetManager.status.add(pregnancy2);
			# endregion
			# region 武裝效果專用
		//武裝冷卻 item_cdt
			var item_cdt00 = new StatusAsset();
			item_cdt00.id = "item_cdt00";								 //ID
			item_cdt00.duration = 60f;									 //倒數計時
			item_cdt00.path_icon = "ui/icons/CDT/CDT000";				//效果圖標
			item_cdt00.locale_id = $"status_title_{item_cdt00.id}";		//區域設定ID					非必要
			item_cdt00.locale_description = $"status_description_{item_cdt00.id}";		//區域設定描述非必要			非必要
			item_cdt00.tier = StatusTier.None;
			item_cdt00.opposite_status = new string[] { "", "" };								//移除,對下位狀態
			addToLocalizedLibrary("ch",item_cdt00.id, "武器冷卻", "冷卻倒數,距離再次驅動武裝還要…");
			addToLocalizedLibrary("en",item_cdt00.id, "Item CD", "Cooling down, it takes more time to activate the weapon again...");
			AssetManager.status.add(item_cdt00);

			var item_cdt01 = new StatusAsset();
			item_cdt01.id = "item_cdt01";								 //ID
			item_cdt01.duration = 60f;									 //倒數計時
			item_cdt01.path_icon = "ui/icons/CDT/CDT000";				//效果圖標
			item_cdt01.locale_id = $"status_title_{item_cdt01.id}";		//區域設定ID					非必要
			item_cdt01.locale_description = $"status_description_{item_cdt01.id}";		//區域設定描述非必要			非必要
			item_cdt01.tier = StatusTier.None;
			item_cdt01.opposite_status = new string[] { "", "" };								//移除,對下位狀態
			addToLocalizedLibrary("ch",item_cdt01.id, "武器冷卻", "冷卻倒數,距離再次驅動武裝還要…");
			addToLocalizedLibrary("en",item_cdt01.id, "Item CD", "Cooling down, it takes more time to activate the weapon again...");
			AssetManager.status.add(item_cdt01);

			var item_cdt02 = new StatusAsset();
			item_cdt02.id = "item_cdt02";								 //ID
			item_cdt02.duration = 60f;									 //倒數計時
			item_cdt02.path_icon = "ui/icons/CDT/CDT000";				//效果圖標
			item_cdt02.locale_id = $"status_title_{item_cdt02.id}";		//區域設定ID					非必要
			item_cdt02.locale_description = $"status_description_{item_cdt02.id}";		//區域設定描述非必要			非必要
			item_cdt02.tier = StatusTier.None;
			item_cdt02.opposite_status = new string[] { "", "" };								//移除,對下位狀態
			addToLocalizedLibrary("ch",item_cdt02.id, "武器冷卻", "冷卻倒數,距離再次驅動武裝還要…");
			addToLocalizedLibrary("en",item_cdt02.id, "Item CD", "Cooling down, it takes more time to activate the weapon again...");
			AssetManager.status.add(item_cdt02);

			var item_cdt03 = new StatusAsset();
			item_cdt03.id = "item_cdt03";								 //ID
			item_cdt03.duration = 60f;									 //倒數計時
			item_cdt03.path_icon = "ui/icons/CDT/CDT000";				//效果圖標
			item_cdt03.locale_id = $"status_title_{item_cdt03.id}";		//區域設定ID					非必要
			item_cdt03.locale_description = $"status_description_{item_cdt03.id}";		//區域設定描述非必要			非必要
			item_cdt03.tier = StatusTier.None;
			item_cdt03.opposite_status = new string[] { "", "" };								//移除,對下位狀態
			addToLocalizedLibrary("ch",item_cdt03.id, "武器冷卻", "冷卻倒數,距離再次驅動武裝還要…");
			addToLocalizedLibrary("en",item_cdt03.id, "Item CD", "Cooling down, it takes more time to activate the weapon again...");
			AssetManager.status.add(item_cdt03);

			var item_cdt04 = new StatusAsset();
			item_cdt04.id = "item_cdt04";								 //ID
			item_cdt04.duration = 60f;									 //倒數計時
			item_cdt04.path_icon = "ui/icons/CDT/CDT000";				//效果圖標
			item_cdt04.locale_id = $"status_title_{item_cdt04.id}";		//區域設定ID					非必要
			item_cdt04.locale_description = $"status_description_{item_cdt04.id}";		//區域設定描述非必要			非必要
			item_cdt04.tier = StatusTier.None;
			item_cdt04.opposite_status = new string[] { "", "" };								//移除,對下位狀態
			addToLocalizedLibrary("ch",item_cdt04.id, "武器冷卻", "冷卻倒數,距離再次驅動武裝還要…");
			addToLocalizedLibrary("en",item_cdt04.id, "Item CD", "Cooling down, it takes more time to activate the weapon again...");
			AssetManager.status.add(item_cdt04);

			var item_cdt05 = new StatusAsset();
			item_cdt05.id = "item_cdt05";								 //ID
			item_cdt05.duration = 60f;									 //倒數計時
			item_cdt05.path_icon = "ui/icons/CDT/CDT000";				//效果圖標
			item_cdt05.locale_id = $"status_title_{item_cdt05.id}";		//區域設定ID					非必要
			item_cdt05.locale_description = $"status_description_{item_cdt05.id}";		//區域設定描述非必要			非必要
			item_cdt05.tier = StatusTier.None;
			item_cdt05.opposite_status = new string[] { "", "" };								//移除,對下位狀態
			addToLocalizedLibrary("ch",item_cdt05.id, "武器冷卻", "冷卻倒數,距離再次驅動武裝還要…");
			addToLocalizedLibrary("en",item_cdt05.id, "Item CD", "Cooling down, it takes more time to activate the weapon again...");
			AssetManager.status.add(item_cdt05);
			# endregion
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