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
    class TraitsEx
    {
       public static void init()
        {
			addTraitToWorld();
		}
		public static void addTraitToWorld()
		{// 增加特質到世界

		//龍
			var dragon = AssetManager.actor_library.get("dragon");
			if (dragon != null)
			{
				dragon.addTrait("projectile15");
				dragon.addTrait("monster");
			}
		//屍龍
			var zombie_dragon = AssetManager.actor_library.get("zombie_dragon");
			if (zombie_dragon != null)
			{
				zombie_dragon.removeTrait("projectile15");
				zombie_dragon.addTrait("projectile10");
				zombie_dragon.addTrait("monster");
			}
		//腫瘤動物
			var tumor_monster_animal = AssetManager.actor_library.get("tumor_monster_animal");
			if (tumor_monster_animal != null)
			{
				tumor_monster_animal.addTrait("monste_nest001");
				tumor_monster_animal.addTrait("monster");
			}
		//腫瘤單位
			var tumor_monster_unit = AssetManager.actor_library.get("tumor_monster_unit");
			if (tumor_monster_unit != null)
			{
				tumor_monster_unit.addTrait("monste_nest001");
				tumor_monster_unit.addTrait("monster");
			}
		//核心機器人
			var assimilator = AssetManager.actor_library.get("assimilator");
			if (assimilator != null)
			{
				assimilator.addTrait("monste_nest002");
				assimilator.addTrait("monster");
			}
		//小南瓜
			var lil_pumpkin = AssetManager.actor_library.get("lil_pumpkin");
			if (lil_pumpkin != null)
			{
				lil_pumpkin.addTrait("monste_nest003");
				lil_pumpkin.addTrait("monster");
			}
		//生物質
			var bioblob = AssetManager.actor_library.get("bioblob");
			if (bioblob != null)
			{
				bioblob.addTrait("monste_nest004");
				bioblob.addTrait("monster");
			}
		//冰魔
			var cold_one = AssetManager.actor_library.get("cold_one");
			if (cold_one != null)
			{
				cold_one.addTrait("monste_nest005");
				cold_one.addTrait("monster");
			}
		//惡魔
			var demon = AssetManager.actor_library.get("demon");
			if (demon != null)
			{
				demon.addTrait("monste_nest006");
				demon.addTrait("monster");
			}
		//天使
			var angle = AssetManager.actor_library.get("angle");
			if (angle != null)
			{
				angle.addTrait("monste_nest007");
			}
		//蜜蜂
			var bee = AssetManager.actor_library.get("bee");
			if (bee != null)
			{
				bee.addTrait("monste_nest000");
			}
		//幽靈
			var ghost = AssetManager.actor_library.get("ghost");
			if (ghost != null)
			{
				ghost.addTrait("add_cursed");
				ghost.addTrait("monster");
			}
		//骷髏
			var skeleton = AssetManager.actor_library.get("skeleton");
			if (skeleton != null)
			{
				skeleton.addTrait("add_cursed");
				skeleton.addTrait("monster");
			}
		//外星人
			var alien = AssetManager.actor_library.get("alien");
			if (alien != null)
			{
				alien.addTrait("monster");
			}
		//UFO
			var UFO = AssetManager.actor_library.get("UFO");
			if (UFO != null)
			{
				UFO.addTrait("monster");
			}
		//暴徒
			var bandit = AssetManager.actor_library.get("bandit");
			if (bandit != null)
			{
				bandit.addTrait("monster");
			}
		//火骷髏
			var fire_skull = AssetManager.actor_library.get("fire_skull");
			if (fire_skull != null)
			{
				fire_skull.addTrait("monster");
			}
		//跳骷髏
			var jumpy_skull = AssetManager.actor_library.get("jumpy_skull");
			if (jumpy_skull != null)
			{
				jumpy_skull.addTrait("add_cursed");
				jumpy_skull.addTrait("monster");
			}
		//死靈術師
			var necromancer = AssetManager.actor_library.get("necromancer");
			if (necromancer != null)
			{
				necromancer.addTrait("evillaw_tgc");
				necromancer.addTrait("monster");
			}
		//神指
			var god_finger = AssetManager.actor_library.get("god_finger");
			if (god_finger != null)
			{
				god_finger.addTrait("monster");
			}
		//怪屋
			var living_house = AssetManager.actor_library.get("living_house");
			if (living_house != null)
			{
				living_house.addTrait("monster");
			}
		//格雷
			var greg = AssetManager.actor_library.get("greg");
			if (greg != null)
			{
				greg.addTrait("monster");
			}
		//火元素
			var fire_elemental = AssetManager.actor_library.get("fire_elemental");
			if (fire_elemental != null)
			{
				fire_elemental.addTrait("monster");
			}
		//火元素
			var fire_elemental_blob = AssetManager.actor_library.get("fire_elemental_blob");
			if (fire_elemental_blob != null)
			{
				fire_elemental_blob.addTrait("monster");
			}
		//火元素
			var fire_elemental_snake = AssetManager.actor_library.get("fire_elemental_snake");
			if (fire_elemental_snake != null)
			{
				fire_elemental_snake.addTrait("monster");
			}
		//火元素
			var fire_elemental_horse = AssetManager.actor_library.get("fire_elemental_horse");
			if (fire_elemental_horse != null)
			{
				fire_elemental_horse.addTrait("monster");
			}
		//火元素
			var fire_elemental_slug = AssetManager.actor_library.get("fire_elemental_slug");
			if (fire_elemental_slug != null)
			{
				fire_elemental_slug.addTrait("monster");
			}
		//蘑菇單位
			var mush_unit = AssetManager.actor_library.get("mush_unit");
			if (mush_unit != null)
			{
				mush_unit.addTrait("monster");
				mush_unit.removeTrait("holyarts_divinelight");
			}
		//蘑菇動物
			var mush_animal = AssetManager.actor_library.get("mush_animal");
			if (mush_animal != null)
			{
				mush_animal.addTrait("monster");
				mush_animal.removeTrait("holyarts_divinelight");
			}
		//雪人
			var snowman = AssetManager.actor_library.get("snowman");
			if (snowman != null)
			{
				snowman.addTrait("monster");
			}
		}
    }
}