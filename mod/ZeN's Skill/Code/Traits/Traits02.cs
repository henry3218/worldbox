
using System;
using System.IO;
using System.Threading;
using System.Linq;
using System.Text;
//using System.Numerics;
using System.Reflection;
using System.Collections;
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
	class Traits02
	{
		private static List<ActorTrait> myListTraits = new();
		
		public static void init()
		{//－－－－－－－－－－－－－－－－－－－－－特質區－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－//
		//亞種特質添加
			ActorTrait Mutation00 = new ActorTrait();
			Mutation00.id = "mutation";
			Mutation00.path_icon = "ui/icons/Mutation00/Mutation00";
			Mutation00.group_id = "special";
			Mutation00.rarity = Rarity.R0_Normal;
			Mutation00.can_be_given = true;												//可否給予 true/false
			Mutation00.rate_birth = 1;													//誕生機率
			Mutation00.action_special_effect = new WorldAction(Traits02Actions.Mutation);
			AssetManager.traits.add(Mutation00);
			applyTraitRates(Mutation00); 
			addToLocalizedLibrary("ch",Mutation00.id, "智域開拓", "動物將不再是動物");//中文名稱與介紹
			addToLocalizedLibrary("en",Mutation00.id, "Smart Development", "Animals will no longer be animals");//英文名稱與介紹
			Mutation00.unlock(true);

		//抽選類特質
			ActorTrait birth00 = new ActorTrait();
			birth00.id = "b0000";														//ID
			birth00.path_icon = "ui/icons/Lottery/Nothing";							//圖片
			birth00.group_id = "special";										//群組,下面為遊戲內預設類別
			birth00.can_be_given = false;												//可否給予 true/false
			birth00.action_special_effect = new WorldAction(Traits02Actions.AutoremoveTrait);
			AssetManager.traits.add(birth00); 
			addToLocalizedLibrary("ch",birth00.id, "『銘謝惠顧』", "『甚麼都沒有』");//中文名稱與介紹
			addToLocalizedLibrary("en",birth00.id, "『Thank you for your patronage』", "『Nothing.』");//英文名稱與介紹
			birth00.unlock(true);

			ActorTrait birth01 = new ActorTrait();
			birth01.id = "b0001";														//ID
			birth01.path_icon = "ui/icons/Lottery/Lottery";							//圖片
			birth01.group_id = "special";										//群組,下面為遊戲內預設類別
			birth01.can_be_given = false;												//可否給予 true/false
			birth01.rate_birth = 999;													//誕生機率
			WorldAction combinedActionbirth01 = (WorldAction)Delegate.Combine(/*new WorldAction(Traits02Actions.AutoremoveTraitTT), */new WorldAction(Traits02Actions.Randomness1));
			birth01.action_special_effect = combinedActionbirth01;
			//birth01.action_special_effect = new WorldAction(Traits02Actions.Randomness1);
			AssetManager.traits.add(birth01);
			applyTraitRates(birth01); 
			addToLocalizedLibrary("ch",birth01.id, "『誕生抽選』", "『誕生於此世之人即將獲得力量，但也有可能空手而歸』");//中文名稱與介紹
			addToLocalizedLibrary("en",birth01.id, "『Birth Lottery』", "『Those born into this world will gain power, but they may also return empty-handed..』");//英文名稱與介紹
			birth01.unlock(true);

			ActorTrait birth02 = new ActorTrait();
			birth02.id = "b0002";														//ID
			birth02.path_icon = "ui/icons/Lottery/Lottery";							//圖片
			birth02.group_id = "special";											//群組,下面為遊戲內預設類別
			birth02.can_be_given = false;											//可否給予 true/false
			birth02.rate_birth = 0;													//誕生機率
			WorldAction combinedActionbirth02 = (WorldAction)Delegate.Combine(
				new WorldAction(Traits02Actions.Randomness2_1),
				new WorldAction(Traits02Actions.Randomness2_2),
				new WorldAction(Traits02Actions.Randomness2_3),
				new WorldAction(Traits02Actions.Randomness2_4),
				new WorldAction(Traits02Actions.Randomness2_5),
				new WorldAction(Traits02Actions.Randomness2_6),
				new WorldAction(Traits02Actions.Randomness2_7),
				// *** 新增: 在所有子抽選都执行完毕后，移除 b0002 特质 ***
				new WorldAction((pTarget, pTile) => {
					Actor actor = pTarget as Actor;
					if (actor != null && actor.hasTrait("b0002"))
					{
						actor.removeTrait("b0002");
						//Debug.Log($"單位 {actor.name} 的抽獎主特質 b0002 已在所有子抽選完成後移除。");
					}
					return true;	// 返回 true 表示动作成功
				})
			);
			birth02.action_special_effect = combinedActionbirth02;
			AssetManager.traits.add(birth02);
			applyTraitRates(birth02); 
			addToLocalizedLibrary("ch",birth02.id, "『成就獎勵』", "『成就偉業之人將獲得力量,但也有可能甚麼都得不到』");//中文名稱與介紹
			addToLocalizedLibrary("en",birth02.id, "『Achievement Rewards』", "『Those who achieve great things will gain power, but they may also gain nothing.』");//英文名稱與介紹
			birth02.unlock(true);


			//智慧 >< 失智
			HashSet<ActorTrait> oppositeTraitsSet_WD2 = new HashSet<ActorTrait>();
			string[] traitIDsToOppose_WD2 = { "Mutation00", "evillaw_ew",}; 
			foreach (string traitID in traitIDsToOppose_WD2)
			{
				if (!string.IsNullOrEmpty(traitID)) 
				{
					ActorTrait traitAsset = AssetManager.traits.get(traitID);
					if (traitAsset != null)
					{
						oppositeTraitsSet_WD2.Add(traitAsset);
					}
				}
			};
			Mutation00.opposite_traits = oppositeTraitsSet_WD2;
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
			    LocalizedTextManager.add("trait_" + id, name);
			    LocalizedTextManager.add("trait_" + id + "_info", description);
			}
        }
		private static void populateListOppositeTraits()
		{//填充列表相反的特徵
			if (myListTraits.Any())
			{
				foreach(var trait in myListTraits)
				{
					List<string>? curentTraitOppositeList = trait.opposite_list;
					if (curentTraitOppositeList != null && curentTraitOppositeList.Any())	// 增加空值檢查
					{
						// Ensure opposite_traits list exists
						if (trait.opposite_traits == null)
							trait.opposite_traits = new();
						foreach (var opposite in curentTraitOppositeList)	// 使用 curentTraitOppositeList
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
		{// 特質誕生
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