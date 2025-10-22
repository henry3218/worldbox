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
    class Traits01_symbolic
    {
		private static List<ActorTrait> myListTraits = new();
        // ** 修正 weightedRandom 的定義，移除多餘的大括號 **
		public static void init()
        {//－－－－－－－－－－－－－－－－－－－－－特質區－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－//
/*		//象徵性特質
            ActorTrait achievement01 = new ActorTrait();
            achievement01.id = "rich";														//ID
			achievement01.path_icon = "ui/icons/Merits/rich";							//圖片
			achievement01.group_id = "merits";										//群組,下面為遊戲內預設類別
            achievement01.can_be_given = true;												//可否給予 true/false
            achievement01.can_be_removed = true;											//可否移除 true/false
			AssetManager.traits.add(achievement01);
			//請將不使用的語言版本加上『//』
			addToLocale_CH(achievement01.id, "富豪", "他非常的有錢");//中文名稱與介紹
			//addToLocale_EN(achievement01.id, "Rich", "He is very rich.");//英文名稱與介紹
			achievement01.unlock(true);

            ActorTrait achievement02 = new ActorTrait();
            achievement02.id = "mentor";														//ID
			achievement02.path_icon = "ui/icons/Merits/mentor";							//圖片
			achievement02.group_id = "merits";										//群組,下面為遊戲內預設類別
            achievement02.can_be_given = true;												//可否給予 true/false
            achievement02.can_be_removed = true;											//可否移除 true/false
			AssetManager.traits.add(achievement02);
			//請將不使用的語言版本加上『//』
			addToLocale_CH(achievement02.id, "人生導師", "他的經驗非常的豐富");//中文名稱與介紹
			//addToLocale_EN(achievement02.id, "Life Coach", "He has a lot of experience.");//英文名稱與介紹
			achievement02.unlock(true);*/

		}

		private static void addToLocale_CH(string id, string name, string description)
		{// 新增區域設定 CH
			LM.AddToCurrentLocale($"trait_{id}", name);
			LM.AddToCurrentLocale($"trait_{id}_info", description);
		}
		private static void addToLocale_EN(string id, string name, string description)
		{// 新增區域設定 EN
			LM.AddToCurrentLocale($"trait_{id}", name);
			LM.AddToCurrentLocale($"trait_{id}_info", description);
		}
		private static void populateListOppositeTraits()
		{//填充列表相反的特徵
			if (myListTraits.Any())
			{
				foreach(var trait in myListTraits)
				{
					List<string>? curentTraitOppositeList = trait.opposite_list;
					if (curentTraitOppositeList != null && curentTraitOppositeList.Any()) // 增加空值檢查
					{
						// Ensure opposite_traits list exists
						if (trait.opposite_traits == null)
							trait.opposite_traits = new();
						foreach (var opposite in curentTraitOppositeList) // 使用 curentTraitOppositeList
						{
							var matchedTrait = myListTraits.FirstOrDefault(t => t.id == opposite);
							if (matchedTrait != null && !trait.opposite_traits.Contains(matchedTrait))
							{
								trait.opposite_traits.Add(matchedTrait);
							}
						}
					}
				}
			}
		}
        private static void applyTraitRates(ActorTrait trait)
        {
            if (trait == null)
                return;
            if (trait.rate_birth > 0)
            {
                AssetManager.traits.pot_traits_birth.AddRange(
                    Enumerable.Repeat(trait, trait.rate_birth)
                );
            }
        }
    }

}