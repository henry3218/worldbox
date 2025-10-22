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
	class StatusEffects_CDT2
	{	
		public static void init()
		{
			# region 環境CDT
			var cdt_alter01 = new StatusAsset();
			cdt_alter01.id = "cdt_alter01";								 //ID
			cdt_alter01.duration = 60f;									 //倒數計時
			cdt_alter01.path_icon = "ui/icons/CDT/CDT001";			 //效果圖標
			cdt_alter01.locale_id = $"status_title_{cdt_alter01.id}";	  //區域設定ID					非必要
			cdt_alter01.locale_description = $"status_description_{cdt_alter01.id}";		//區域設定描述非必要			非必要
			cdt_alter01.tier = StatusTier.Basic;
			addToLocalizedLibrary("ch",cdt_alter01.id, "改變環境後", "要過一段時間才能繼續工作");
			addToLocalizedLibrary("en",cdt_alter01.id, "After changing the environment", "It will take some time before I can continue working.");
			AssetManager.status.add(cdt_alter01);

			var cdt_alter02 = new StatusAsset();
			cdt_alter02.id = "cdt_alter02";								 //ID
			cdt_alter02.duration = 60f;									 //倒數計時
			cdt_alter02.path_icon = "ui/icons/CDT/CDT002";			 //效果圖標
			cdt_alter02.locale_id = $"status_title_{cdt_alter02.id}";	  //區域設定ID					非必要
			cdt_alter02.locale_description = $"status_description_{cdt_alter02.id}";		//區域設定描述非必要			非必要
			cdt_alter02.tier = StatusTier.Basic;
			addToLocalizedLibrary("ch",cdt_alter02.id, "種植果樹後", "要過一段時間才能繼續工作");
			addToLocalizedLibrary("en",cdt_alter02.id, "After planting fruit trees", "It will take some time before I can continue working.");
			AssetManager.status.add(cdt_alter02);

			var cdt_alter03 = new StatusAsset();
			cdt_alter03.id = "cdt_alter03";								 //ID
			cdt_alter03.duration = 60f;									 //倒數計時
			cdt_alter03.path_icon = "ui/icons/CDT/CDT003";			 //效果圖標
			cdt_alter03.locale_id = $"status_title_{cdt_alter03.id}";	  //區域設定ID					非必要
			cdt_alter03.locale_description = $"status_description_{cdt_alter03.id}";		//區域設定描述非必要			非必要
			cdt_alter03.tier = StatusTier.Basic;
			addToLocalizedLibrary("ch",cdt_alter03.id, "挖掘礦物後", "要過一段時間才能繼續工作");
			addToLocalizedLibrary("en",cdt_alter03.id, "After mining the minerals", "It will take some time before I can continue working.");
			AssetManager.status.add(cdt_alter03);

			var cdt_alter04 = new StatusAsset();
			cdt_alter04.id = "cdt_alter04";								 //ID
			cdt_alter04.duration = 60f;									 //倒數計時
			cdt_alter04.path_icon = "ui/icons/CDT/CDT004";			 //效果圖標
			cdt_alter04.locale_id = $"status_title_{cdt_alter04.id}";	  //區域設定ID					非必要
			cdt_alter04.locale_description = $"status_description_{cdt_alter04.id}";		//區域設定描述非必要			非必要
			cdt_alter04.tier = StatusTier.Basic;
			addToLocalizedLibrary("ch",cdt_alter04.id, "建築升級後", "要過一段時間才能繼續工作");
			addToLocalizedLibrary("en",cdt_alter04.id, "After building upgrade", "It will take some time before I can continue working.");
			AssetManager.status.add(cdt_alter04);
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