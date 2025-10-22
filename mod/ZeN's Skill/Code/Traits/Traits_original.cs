
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


namespace ZeN_01
{
    class Traits01_original
    {
        public static void init()
        {//－－－－－－－－－－－－－－－－－－－－－特質區－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－//
			HashSet<ActorTrait> oppositeTraitsSet_XXX = new HashSet<ActorTrait>();
            string[] traitIDsToOppose_XXX = { "holyarts_ha", "evillaw_disease", }; 
            foreach (string traitID in traitIDsToOppose_XXX)
            {//對立特質
                if (!string.IsNullOrEmpty(traitID)) 
                {
                    ActorTrait traitAsset = AssetManager.traits.get(traitID);
                    if (traitAsset != null)
                    {
                        oppositeTraitsSet_XXX.Add(traitAsset);
                    }
                }
            };
			
			HashSet<ActorTrait> oppositeTraitsSet_XXX2 = new HashSet<ActorTrait>();
            string[] traitIDsToOppose_XXX2 = { "evillaw_tgc", "add_cursed", }; 
            foreach (string traitID in traitIDsToOppose_XXX2)
            {//對立特質
                if (!string.IsNullOrEmpty(traitID)) 
                {
                    ActorTrait traitAsset = AssetManager.traits.get(traitID);
                    if (traitAsset != null)
                    {
                        oppositeTraitsSet_XXX2.Add(traitAsset);
                    }
                }
            };

			HashSet<ActorTrait> oppositeTraitsSet_XXX3 = new HashSet<ActorTrait>();
            string[] traitIDsToOppose_XXX3 = { "evillaw_tantrum",}; 
            foreach (string traitID in traitIDsToOppose_XXX3)
            {//對立特質
                if (!string.IsNullOrEmpty(traitID)) 
                {
                    ActorTrait traitAsset = AssetManager.traits.get(traitID);
                    if (traitAsset != null)
                    {
                        oppositeTraitsSet_XXX3.Add(traitAsset);
                    }
                }
            };

			//原版特質 殘疾
			ActorTrait crippled = AssetManager.traits.get("crippled");
			crippled.id = "crippled";
            crippled.opposite_traits = oppositeTraitsSet_XXX;
			crippled.unlock(true);
			
			//原版特質 獨眼
			ActorTrait eyepatch = AssetManager.traits.get("eyepatch");
			eyepatch.id = "eyepatch";
            eyepatch.opposite_traits = oppositeTraitsSet_XXX;
			eyepatch.unlock(true);
			
			//原版特質 燒傷
			ActorTrait skin_burns = AssetManager.traits.get("skin_burns");
			skin_burns.id = "skin_burns";
            skin_burns.opposite_traits = oppositeTraitsSet_XXX;
			skin_burns.unlock(true);

			//原版特質 祝福
			ActorTrait blessed = AssetManager.traits.get("blessed");
			blessed.id = "blessed";
            blessed.opposite_traits = oppositeTraitsSet_XXX2;
			blessed.unlock(true);

			//原版特質 瘋狂
			ActorTrait madness = AssetManager.traits.get("madness");
			madness.id = "madness";
            //madness.opposite_traits = oppositeTraitsSet_XXX2;
			madness.unlock(true);

			//原版特質 堅忍
			ActorTrait strong_minded = AssetManager.traits.get("strong_minded");
			strong_minded.id = "strong_minded";
            //strong_minded.opposite_traits = oppositeTraitsSet_XXX3;
			strong_minded.unlock(true);

			//原版特質 聖痕
			ActorTrait scar_of_divinity = AssetManager.traits.get("scar_of_divinity");
			scar_of_divinity.id = "scar_of_divinity";
			scar_of_divinity.action_special_effect = new WorldAction(scar_of_divinity_AutoTrait);
			scar_of_divinity.unlock(true);

		}
		public static bool scar_of_divinity_AutoTrait(BaseSimObject pTarget, WorldTile pTile = null)
		{//scar_of_divinity_AutoTrait
            if (pTarget is Actor actor)
			{// 增加生命值
                //actor.data.health += 40000000;
				//pTarget.a.removeTrait("gmo");
				pTarget.a.removeTrait("scar_of_divinity");
			}
			return true;
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
    }

}