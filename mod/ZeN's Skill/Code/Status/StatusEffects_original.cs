using NeoModLoader.api.attributes;
using NeoModLoader.General;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using ReflectionUtility;
using HarmonyLib;
using ai;

namespace ZeN_01
{
    class StatusEffects_original
    { 
        public static void init()
        {
		//戰鬥狀態 
			var recovery_combat_action = AssetManager.status.get("recovery_combat_action");
			recovery_combat_action.opposite_status = new string[] { "cdt_clear00" };

			var recovery_spell = AssetManager.status.get("recovery_spell");
			recovery_spell.opposite_status = new string[] { "cdt_clear00" };

		//生育狀態
			var pregnant = AssetManager.status.get("pregnant"); // 懷孕
			pregnant.opposite_status = new string[] { "sterilization" };

			var budding = AssetManager.status.get("budding"); // 萌芽
			budding.opposite_status = new string[] { "sterilization" };

			var taking_roots = AssetManager.status.get("taking_roots"); //營養繁殖
			taking_roots.opposite_status = new string[] { "sterilization" };

			var soul_harvested = AssetManager.status.get("soul_harvested"); // 靈魂掠奪
			soul_harvested.opposite_status = new string[] { "sterilization" };

			var egg = AssetManager.status.get("egg"); // 蛋
			egg.remove_status = new string[] { "sterilization" };

			var uprooting = AssetManager.status.get("uprooting"); // 除根
			uprooting.remove_status = new string[] { "sterilization" };

		//情緒狀態
			var tantrum = AssetManager.status.get("tantrum"); //忿怒
			tantrum.opposite_status = new string[] { "calm", "serenity", "apostle_se2", "brave", "darkblessing" };
			tantrum.opposite_traits = AssetLibrary<StatusAsset>.a<string>(new string[]
			{
				 "apostle"
			});

			var angry = AssetManager.status.get("angry"); // 忿怒
			angry.opposite_status = new string[] { "calm", "serenity", "slave", "apostle_se2", "brave", "darkblessing" };
			angry.opposite_traits = AssetLibrary<StatusAsset>.a<string>(new string[]
			{
				 "apostle"
			});

			var on_guard = AssetManager.status.get("on_guard"); // 警戒
			on_guard.opposite_status = new string[] { "darkblessing" };
			on_guard.opposite_traits = AssetLibrary<StatusAsset>.a<string>(new string[]
			{
				 "other001", "other002"
			});

			var surprised = AssetManager.status.get("surprised"); // 驚訝
			surprised.opposite_status = new string[] { "darkblessing" };
			surprised.opposite_traits = AssetLibrary<StatusAsset>.a<string>(new string[]
			{
				 "other001", "other002"
			});

			var confused = AssetManager.status.get("confused"); // 混亂
			confused.opposite_status = new string[] { "antibody",
			"arrogant_demon_king", "greedy_demon_king", "wrath_demon_king", "gluttony_demon_king", "lust_demon_king", "sloth_demon_king", "envy_demon_king", 
			"brave", "darkblessing"/*, "ex_undead_emperor"*/ };
			confused.opposite_traits = AssetLibrary<StatusAsset>.a<string>(new string[]
			{
				 "add_confused", "other001", "other002"
			});

			var fell_in_love = AssetManager.status.get("fell_in_love"); // 愛
			fell_in_love.opposite_status = new string[] { "apostle_se2" };
		//減益狀態
			var cursed = AssetManager.status.get("cursed");//詛咒
			cursed.opposite_status = new string[] { "slave", "apostle_se",
			"arrogant_demon_king", "greedy_demon_king", "gluttony_demon_king", "lust_demon_king", "wrath_demon_king", "sloth_demon_king", "envy_demon_king", "brave" };
			cursed.opposite_traits = AssetLibrary<StatusAsset>.a<string>(new string[]
			{
				"evil", "holyarts_consecration", "holyarts_justice", "holyarts_divinelight", "add_cursed", "evillaw_tgc"
			});

			var burning = AssetManager.status.get("burning");//燃燒
			burning.opposite_status = new string[] { "antibody",
			"arrogant_demon_king", "greedy_demon_king", "wrath_demon_king", "lust_demon_king", "sloth_demon_king",
			"brave", "darkblessing", "ex_undead_emperor" };

			var frozen = AssetManager.status.get("frozen");//凍結
			frozen.opposite_status = new string[] { "antibody", "apostle_se",
			"arrogant_demon_king", "greedy_demon_king", "wrath_demon_king", "gluttony_demon_king", "lust_demon_king", "sloth_demon_king", "envy_demon_king",
			"brave", "darkblessing", "ex_undead_emperor" };

			var slowness = AssetManager.status.get("slowness");//遲緩
			slowness.opposite_status = new string[] { "antibody", "arrogant_demon_king", "brave", "darkblessing", "ex_undead_emperor" };
			slowness.opposite_traits = AssetLibrary<StatusAsset>.a<string>(new string[]
			{
				"add_slowdown", "add_unknown"
			});

			var ash_fever = AssetManager.status.get("ash_fever");//灰病
			ash_fever.id = "ash_fever";
			ash_fever.opposite_status = new string[] { "antibody", "brave", "darkblessing" };
			ash_fever.opposite_traits = AssetLibrary<StatusAsset>.a<string>(new string[]
			{
				"add_afc", "add_unknown"
			});

			var cough = AssetManager.status.get("cough");//咳嗽
			cough.opposite_status = new string[] { "antibody", "brave", "darkblessing" };
			cough.opposite_traits = AssetLibrary<StatusAsset>.a<string>(new string[]
			{
				"add_afc", "add_unknown"
			});


			var stunned = AssetManager.status.get("stunned");//眩暈
			stunned.opposite_status = new string[] { "cdt_calm", "antibody", "apostle_se2",
			"arrogant_demon_king", "greedy_demon_king", "wrath_demon_king", "gluttony_demon_king", "lust_demon_king", "sloth_demon_king", "envy_demon_king", "brave", "darkblessing", "ex_undead_emperor" };
			stunned.opposite_traits = AssetLibrary<StatusAsset>.a<string>(new string[]
			{
				"add_stunned", "add_unknown", "other001", "other002"
			});

			var poisoned = AssetManager.status.get("poisoned");//毒
			poisoned.opposite_status = new string[] { "antibody", "brave", "darkblessing", "ex_undead_emperor" };
			poisoned.opposite_traits = AssetLibrary<StatusAsset>.a<string>(new string[]
			{
				"add_poisonous", "add_unknown"
			});

			var silenced = AssetManager.status.get("spell_silence");//沉默
			silenced.opposite_status = new string[] { "antibody", "brave", "darkblessing", "ex_undead_emperor" };
			silenced.opposite_traits = AssetLibrary<StatusAsset>.a<string>(new string[]
			{
				"add_silenced", "add_unknown"
			});

			var drowning = AssetManager.status.get("drowning");//溺水
			drowning.opposite_status = new string[] { "antibody", "brave", "darkblessing", "ex_undead_emperor" };
			drowning.opposite_traits = AssetLibrary<StatusAsset>.a<string>(new string[]
			{
				"add_drowning", "add_unknown"
			});
			
			var sleeping = AssetManager.status.get("sleeping");//睡眠
			sleeping.opposite_status = new string[] { "antibody", "apostle_se",
			"greedy_demon_king", "wrath_demon_king", "gluttony_demon_king", "lust_demon_king", "envy_demon_king", "brave", "darkblessing" };
			sleeping.opposite_traits = AssetLibrary<StatusAsset>.a<string>(new string[]
			{
				"apostle", "undead_servant", "undead_servant2"
			});

		//增益狀態
			var shield = AssetManager.status.get("shield");//護盾
			shield.opposite_status = new string[] { "weaken"};

			var powerup = AssetManager.status.get("powerup");//超頻
			powerup.opposite_status = new string[] { "weaken"};

			var caffeinated = AssetManager.status.get("caffeinated");//咖啡
			caffeinated.opposite_status = new string[] { "weaken"};

			var enchanted = AssetManager.status.get("enchanted");//充能
			enchanted.opposite_status = new string[] { "weaken"};

			var rage = AssetManager.status.get("rage");//狂暴
			rage.opposite_status = new string[] { "weaken"};

			var spell_boost = AssetManager.status.get("spell_boost");//法術上升
			spell_boost.opposite_status = new string[] { "weaken"};

			var motivated = AssetManager.status.get("motivated");//有動力
			motivated.opposite_status = new string[] { "weaken"};

			var invincible = AssetManager.status.get("invincible");//無敵
			invincible.opposite_status = new string[] { "weaken"};
			
			var inspired = AssetManager.status.get("inspired");//激勵
			inspired.opposite_status = new string[] { "weaken"};

        }
        private static void addToLocale_ch(string id, string name, string description)
        {//新增到本地化ch
            LM.AddToCurrentLocale($"status_title_{id}", name);
            LM.AddToCurrentLocale($"status_description_{id}", description);
        }
        private static void addToLocale_en(string id, string name, string description)
        {//新增到本地化en
            LM.AddToCurrentLocale($"status_title_{id}", name);
            LM.AddToCurrentLocale($"status_description_{id}", description);
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