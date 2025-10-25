
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

//Alt+4
namespace ZeN_01
{
	class Traits01
	{

		// 建议：将 System.Random 实例声明为静态字段，避免频繁创建
		//private static System.Random _random = new System.Random();
		private static List<ActorTrait> myListTraits = new();
		//public List<string> combat_actions_ids;//戰技相關
		
		public static void init()
		{//－－－－－－－－－－－－－－－－－－－－－特質區－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－//
		//測試用位置
		/*	ActorTrait test0000 = new ActorTrait();
			test0000.id = "test0000";
			test0000.path_icon = "ui/icons/Skill/test0000";
			test0000.group_id = "cognitive";
			test0000.rarity = Rarity.R2_Epic;
			test0000.rate_birth = 0;
			test0000.rate_inherit = 1;
			test0000.action_special_effect = new WorldAction(Items01Actions.GiveSoul);
			test0000.action_attack_target = new AttackAction(Traits01Actions.DestroyEvil);
			test0000.action_get_hit = new GetHitAction(Traits01Actions.DestroyEvil);
			AssetManager.traits.add(test0000);
			addToLocalizedLibrary("ch",test0000.id, "test000C", "test000C");
			addToLocalizedLibrary("en",test0000.id, "test000E", "test000E");
			test0000.unlock(true);*/

			#region 職務類別	Profession
			//帝王 King ★ 2
			ActorTrait ProK= new ActorTrait();
			ProK.id = "pro_king";//King
			ProK.path_icon = "ui/Icons/Pro/Pro01-King";
			ProK.group_id = "profession";											//群組
			ProK.rarity = Rarity.R3_Legendary;										//稀有度
			ProK.can_be_given = true;
			ProK.can_be_removed = true;
			ProK.rate_birth = 0;													//誕生機率
			ProK.rate_inherit = 1;													//繼承機率
			List<string> opposite_ProK = new () { "pro_king", "pro_leader", "pro_groupleader", "pro_soldier", "pro_warrior" };
			ProK.addOpposites(opposite_ProK);
			ProK.base_stats = new BaseStats();										//添加數值
			WorldAction combinedActionProK = (WorldAction)Delegate.Combine(
			new WorldAction(Traits01Actions.KingEffect),				//職業設定
			new WorldAction(Traits01Actions.ContinuousKingBuffEffect1),	//思維輪轉
			new WorldAction(Traits01Actions.ContinuousKingBuffEffect2),	//思維輪轉
			new WorldAction(Traits01Actions.ConferringLeader),			//特質授予
			new WorldAction(Traits01Actions.removeTraitK),				//特質移除
			new WorldAction(Traits01Actions.addFavorite_K),				//條件添加最愛
			new WorldAction(Traits01Actions.TraittAddRemove0));			//條件移除
			ProK.action_special_effect = combinedActionProK;
			AssetManager.traits.add(ProK);
			addToLocalizedLibrary("ch",ProK.id, "職階:帝王", "他將成為國王");
			addToLocalizedLibrary("en",ProK.id, "Pro:King", "He will be king.");
			ProK.unlock(true);

			//領主 Leader ★ 2
			ActorTrait ProL= new ActorTrait();
			ProL.id = "pro_leader";
			ProL.path_icon = "ui/Icons/Pro/Pro02-Leader";
			ProL.group_id = "profession";											//群組
			ProL.rarity = Rarity.R3_Legendary;										//稀有度
			ProL.can_be_given = true;
			ProL.can_be_removed = true;
			ProL.rate_birth = 0;													//誕生機率
			ProL.rate_inherit = 1;													//繼承機率
			ProL.base_stats = new BaseStats();										//添加數值
			ProL.base_stats.set("army", 1000f);									//軍隊數量
			WorldAction combinedActionProL = (WorldAction)Delegate.Combine(
			new WorldAction(Traits01Actions.LeaderEffect),					//職業設定
			new WorldAction(Traits01Actions.ContinuousLeaderBuffEffect1),	//思維輪轉
			new WorldAction(Traits01Actions.ContinuousLeaderBuffEffect2),	//思維輪轉
			new WorldAction(Traits01Actions.AscendTheThrone),				//登基(特質添加)
			new WorldAction(Traits01Actions.Appointment),					//任命
			new WorldAction(Traits01Actions.addFavorite_L),					//條件添加最愛
			new WorldAction(Traits01Actions.TraittAddRemove0));				//條件移除
			ProL.action_special_effect = combinedActionProL;
			AssetManager.traits.add(ProL);
			addToLocalizedLibrary("ch",ProL.id, "職階:領主", "他將成為領地的治理者");
			addToLocalizedLibrary("en",ProL.id, "Pro:Leader", "He will become the ruler of the territory.");
			ProL.unlock(true);

			//軍團長 GroupLeader
			ActorTrait ProGL= new ActorTrait();
			ProGL.id = "pro_groupleader";
			ProGL.path_icon = "ui/Icons/Pro/Pro03-GroupLeader";
			ProGL.group_id = "profession";											//群組
			ProGL.rarity = Rarity.R2_Epic;											//稀有度
			ProGL.can_be_given = true;
			ProGL.can_be_removed = true;
			ProGL.rate_birth = 0;													//誕生機率
			ProGL.rate_inherit = 1;													//繼承機率
			ProGL.base_stats = new BaseStats();										//添加數值
			ProGL.base_stats.set("multiplier_damage", 0.10f);						//傷害 %
			ProGL.base_stats.set("damage_range", 50f);								//傷害區間 [未顯示]
			ProGL.base_stats.set("armor", 10f);										//防禦
			ProGL.base_stats.set("critical_chance", 0.10f);							//爆擊機率 %
			ProGL.base_stats.set("multiplier_speed", 0.10f);						//速度 %
			ProGL.base_stats.set("multiplier_attack_speed", 0.10f);					//攻擊速度 %
			ProGL.base_stats.set("warfare", 200f);									//軍事
			WorldAction combinedActionProGL = (WorldAction)Delegate.Combine(
			new WorldAction(Traits01Actions.GroupLeaderEffect),
			new WorldAction(Traits01Actions.GroupLeaderEffect2),
			new WorldAction(Traits01Actions.TraittAddRemove0));
			ProGL.action_special_effect = combinedActionProGL;
			//ProGL.action_special_effect = new WorldAction(Traits01Actions.GroupLeaderEffect);
			AssetManager.traits.add(ProGL);
			addToLocalizedLibrary("ch",ProGL.id, "職階:軍團長", "他將成為領導軍隊之人");
			addToLocalizedLibrary("en",ProGL.id, "Pro:GroupLeader", "He will be the one leading the army.");
			ProGL.unlock(true);

			//士兵 Soldier
			ActorTrait ProS= new ActorTrait();
			ProS.id = "pro_soldier";
			ProS.path_icon = "ui/Icons/Pro/Pro04-Soldier";
			ProS.group_id = "profession";											//群組
			ProS.rarity = Rarity.R2_Epic;											//稀有度
			ProS.can_be_given = true;
			ProS.can_be_removed = true;
			ProS.rate_birth = 0;													//誕生機率
			ProS.rate_inherit = 1;													//繼承機率
			ProS.base_stats = new BaseStats();										//添加數值
			ProS.base_stats.set("multiplier_damage", 0.20f);						//傷害 %
			ProS.base_stats.set("damage_range", 05f);								//傷害區間 [未顯示]
			ProS.base_stats.set("armor", 20f);										//防禦
			ProS.base_stats.set("critical_chance", 0.20f);							//爆擊機率 %
			ProS.base_stats.set("multiplier_speed", 0.20f);							//速度 %
			ProS.base_stats.set("multiplier_attack_speed", 0.20f);					//攻擊速度 %
			WorldAction combinedActionProS = (WorldAction)Delegate.Combine(
			new WorldAction(Traits01Actions.SoldierEffect),
			new WorldAction(Traits01Actions.TraittAddRemove0));
			ProS.action_special_effect = combinedActionProS;
			AssetManager.traits.add(ProS);
			addToLocalizedLibrary("ch",ProS.id, "職階:士兵", "服役終身");
			addToLocalizedLibrary("en",ProS.id, "Pro:Soldier", "Lifetime service.");
			ProS.unlock(true);

			//特種兵 Warrior  ★ 2
			ActorTrait ProW= new ActorTrait();
			ProW.id = "pro_warrior";
			ProW.path_icon = "ui/Icons/Pro/Pro05-Warrior";
			ProW.group_id = "profession";											//群組
			ProW.rarity = Rarity.R3_Legendary;											//稀有度
			ProW.can_be_given = true;
			ProW.can_be_removed = true;
			ProW.rate_birth = 0;													//誕生機率
			ProW.rate_inherit = 1;													//繼承機率;
			ProW.base_stats = new BaseStats();										//添加數值
			ProW.base_stats.set("multiplier_lifespan", -0.50f);						//壽命 %
			ProW.base_stats.set("multiplier_damage", 0.60f);						//傷害 %
			ProW.base_stats.set("damage_range", 10f);								//傷害區間 [未顯示]
			ProW.base_stats.set("armor", 80f);										//防禦
			ProW.base_stats.set("critical_chance", 1.0f);							//爆擊機率 %
			ProW.base_stats.set("multiplier_speed", 1.00f);							//速度 %
			ProW.base_stats.set("multiplier_attack_speed", 10.00f);					//攻擊速度 %
			WorldAction combinedActionProW = (WorldAction)Delegate.Combine(
			new WorldAction(Traits01Actions.SoldierEffect),
			//new WorldAction(Traits01Actions.addFavorite1),
			new WorldAction(Traits01Actions.TraittAddRemove0));
			ProW.action_special_effect = combinedActionProW;
			AssetManager.traits.add(ProW);
			addToLocalizedLibrary("ch",ProW.id, "職階:特種兵", "以生命換取力量");
			addToLocalizedLibrary("en",ProW.id, "Pro:Warrior", "Trading life for power.");
			ProW.unlock(true);
				#endregion
			#region 天賦才能	Talent
			//外交 Diplomacy
			ActorTrait Talent01 = new ActorTrait();
			Talent01.id = "talent_diplomacy";
			Talent01.path_icon = "ui/icons/Talent/01";
			Talent01.group_id = "talent";
			Talent01.rarity = Rarity.R1_Rare;											//稀有度
			Talent01.can_be_given = true;
			Talent01.can_be_removed = true;
			Talent01.rate_birth = 5;
			Talent01.rate_inherit = 10;
			Talent01.base_stats = new BaseStats();
			Talent01.base_stats.set("diplomacy", 100f);					//外交
			AssetManager.traits.add(Talent01);
			addToLocalizedLibrary("ch",Talent01.id, "天賦:外交", "善於交際");
			addToLocalizedLibrary("en",Talent01.id, "Talent:Diplomacy", "Sociable.");
			Talent01.unlock(true);

			//軍事 Warfare
			ActorTrait Talent02 = new ActorTrait();
			Talent02.id = "talent_warfare";
			Talent02.path_icon = "ui/icons/Talent/02";
			Talent02.group_id = "talent";
			Talent02.rarity = Rarity.R1_Rare;											//稀有度
			Talent02.can_be_given = true;
			Talent02.can_be_removed = true;
			Talent02.rate_birth = 5;
			Talent02.rate_inherit = 10;
			Talent02.base_stats = new BaseStats();
			Talent02.base_stats.set("warfare", 100f);					//軍事
			AssetManager.traits.add(Talent02);
			addToLocalizedLibrary("ch",Talent02.id, "天賦:軍事", "善於軍政");
			addToLocalizedLibrary("en",Talent02.id, "Talent:Warfare", "Good at military and political affairs.");

			//管理 Stewardship
			ActorTrait Talent03 = new ActorTrait();
			Talent03.id = "talent_stewardship";
			Talent03.path_icon = "ui/icons/Talent/03";
			Talent03.group_id = "talent";
			Talent03.rarity = Rarity.R1_Rare;											//稀有度
			Talent03.can_be_given = true;
			Talent03.can_be_removed = true;
			Talent03.rate_birth = 5;
			Talent03.rate_inherit = 10;
			Talent03.base_stats = new BaseStats();
			Talent03.base_stats.set("stewardship", 100f);					//管理
			AssetManager.traits.add(Talent03);
			addToLocalizedLibrary("ch",Talent03.id, "天賦:管理", "善於管理");
			addToLocalizedLibrary("en",Talent03.id, "Talent:Stewardship", "Good at management.");
			Talent03.unlock(true);

			//智力 Intelligence
			ActorTrait Talent04 = new ActorTrait();
			Talent04.id = "talent_intelligence";
			Talent04.path_icon = "ui/icons/Talent/04";
			Talent04.group_id = "talent";
			Talent04.rarity = Rarity.R1_Rare;											//稀有度
			Talent04.can_be_given = true;
			Talent04.can_be_removed = true;
			Talent04.rate_birth = 5;
			Talent04.rate_inherit = 10;
			Talent04.base_stats = new BaseStats();
			Talent04.base_stats.set("intelligence", 100f);					//智力
			AssetManager.traits.add(Talent04);
			addToLocalizedLibrary("ch",Talent04.id, "天賦:智力", "天資聰穎");
			addToLocalizedLibrary("en",Talent04.id, "Talent:Intelligence", "Talented.");
			Talent04.unlock(true);

			//繁衍 Coition
			ActorTrait Talent05 = new ActorTrait();
			Talent05.id = "talent_coition";
			Talent05.path_icon = "ui/icons/Talent/05";
			Talent05.group_id = "talent";
			Talent05.rarity = Rarity.R1_Rare;											//稀有度
			Talent05.can_be_given = true;
			Talent05.can_be_removed = true;
			Talent05.rate_birth = 5;
			Talent05.rate_inherit = 10;
			Talent05.base_stats = new BaseStats();
			Talent05.base_stats.set("birth_rate", 999999f);									//生育機率
			Talent05.base_stats.set("offspring", 80f);										//後代
			Talent05.base_stats.set("maturation", -100f);									//成熟
			Talent05.base_stats.set("age_breeding", -99f);									//繁殖年齡[未顯示]
			Talent05.base_stats.set("age_adult", -99f);										//成人年齡[未顯示]
			AssetManager.traits.add(Talent05);
			addToLocalizedLibrary("ch",Talent05.id, "天賦:繁衍", "瘋狂捉愛");
			addToLocalizedLibrary("en",Talent05.id, "Talent:Coition", "Crazy sex.");
			Talent05.unlock(true);
			
			//建造 Build
			ActorTrait Talent06 = new ActorTrait();
			Talent06.id = "talent_build";
			Talent06.path_icon = "ui/icons/Talent/06";
			Talent06.group_id = "talent";
			Talent06.rarity = Rarity.R1_Rare;											//稀有度
			Talent06.can_be_given = true;
			Talent06.can_be_removed = true;
			Talent06.rate_birth = 0;
			Talent06.rate_inherit = 25;
			Talent06.base_stats = new BaseStats();
			Talent06.base_stats.set("construction_speed", 999f);						//建造速度
			WorldAction combinedAction_Talent06 = (WorldAction)Delegate.Combine(
			new WorldAction(Traits01Actions.BuildingUp),
			new WorldAction(Traits01Actions.StopErupting),
			new WorldAction(Traits01Actions.DemolishInactive),
			new WorldAction(Traits01Actions.removeRuins));
			Talent06.action_special_effect = combinedAction_Talent06;
			AssetManager.traits.add(Talent06);
			addToLocalizedLibrary("ch",Talent06.id, "特長:建造", "建築專長，也擅長拆除廢墟");
			addToLocalizedLibrary("en",Talent06.id, "Talent:Demolition", "Expertise in construction, also good at demolishing ruins.");
			Talent06.unlock(true);
			# endregion
			#region 戰技		Combat Skills
			// 堅守 Hold Fast
			ActorTrait Skill0001 = new ActorTrait();
			Skill0001.id = "cb_holdfast";
			Skill0001.path_icon = "ui/icons/Skill/Skill0001";
			Skill0001.group_id = "combatskill";
			Skill0001.rarity = Rarity.R2_Epic;
			Skill0001.rate_birth = 0;
			Skill0001.rate_inherit = 1;
			Skill0001.action_get_hit = new GetHitAction(Traits01Actions.Skill0001_Effect);
			AssetManager.traits.add(Skill0001);
			addToLocalizedLibrary("ch",Skill0001.id, "戰技:堅守", "堅持住!");
			addToLocalizedLibrary("en",Skill0001.id, "CombatSkills:Hold Fast", "Hang in there.");
			Skill0001.unlock(true);

			// 猛擊 Slam
			ActorTrait Skill0002 = new ActorTrait();
			Skill0002.id = "cb_slam";
			Skill0002.path_icon = "ui/icons/Skill/Skill0002";
			Skill0002.group_id = "combatskill";
			Skill0002.rarity = Rarity.R2_Epic;
			Skill0002.rate_birth = 0;
			Skill0002.rate_inherit = 1;
			Skill0002.action_attack_target = new AttackAction(Traits01Actions.Skill0002_Effect);
			AssetManager.traits.add(Skill0002);
			addToLocalizedLibrary("ch",Skill0002.id, "戰技:猛擊", "接下來才是致命一擊!");
			addToLocalizedLibrary("en",Skill0002.id, "CombatSkills:Slam", "The next step is the fatal blow.");
			Skill0002.unlock(true);

			// 彈雨 Bullet Rain
			ActorTrait Skill0003 = new ActorTrait();
			Skill0003.id = "cb_bulletrain";
			Skill0003.path_icon = "ui/icons/Skill/Skill0003";
			Skill0003.group_id = "combatskill";
			Skill0003.rarity = Rarity.R2_Epic;
			Skill0003.rate_birth = 0;
			Skill0003.rate_inherit = 1;
			Skill0003.base_stats = new BaseStats();							//添加數值
			//Skill0003.base_stats.set("projectiles", 80f);
			Skill0003.base_stats.set("recoil", -10f);
			Skill0003.action_special_effect = new WorldAction(Traits01Actions.Skill0003_Effect);
			AssetManager.traits.add(Skill0003);
			addToLocalizedLibrary("ch",Skill0003.id, "戰技:彈雨", "彈藥彷彿是無限!(遠程武器專用)");
			addToLocalizedLibrary("en",Skill0003.id, "CombatSkills:Bullet Rain", "Ammunition seems to be unlimited.(For long-range weapons only).");
			Skill0003.unlock(true);

			// 增長心得 Quick Learning
			ActorTrait Skill0004 = new ActorTrait();
			Skill0004.id = "cb_experience";
			Skill0004.path_icon = "ui/icons/Skill/Skill0004";
			Skill0004.group_id = "combatskill";
			Skill0004.rarity = Rarity.R2_Epic;
			Skill0004.rate_birth = 0;
			Skill0004.rate_inherit = 1;
			Skill0004.base_stats = new BaseStats();							//添加數值
			Skill0004.action_special_effect = new WorldAction(Traits01Actions.Experience);
			AssetManager.traits.add(Skill0004);
			addToLocalizedLibrary("ch",Skill0004.id, "戰技:增長心得", "他可以快速成長");
			addToLocalizedLibrary("en",Skill0004.id, "CombatSkills:Bullet Rain", "He can grow quickly.");
			Skill0004.unlock(true);
			#endregion
			#region 魔彈		Magic Bullet
			// 投石 Rock
			ActorTrait Projectile01 = new ActorTrait();
			Projectile01.id = "projectile01";
			Projectile01.path_icon = "ui/icons/Projectile/Projectile01";
			Projectile01.group_id = "magic_bullet";
			Projectile01.rarity = Rarity.R2_Epic;
			Projectile01.rate_birth = 0;
			Projectile01.rate_inherit = 1;
			Projectile01.base_stats = new BaseStats();									//添加數值
			Projectile01.action_special_effect = new WorldAction(Traits01Actions.Projectile_01);
//			Projectile01.action_attack_target = new AttackAction(Traits01Actions.Projectile_O01);
//			Projectile01.action_get_hit = new GetHitAction(Traits01Actions.Projectile_O01);
			AssetManager.traits.add(Projectile01);
			addToLocalizedLibrary("ch",Projectile01.id, "魔彈:投石", "投擲石塊的魔法");
			addToLocalizedLibrary("en",Projectile01.id, "Magic Bullet:Rock", "Stone throwing magic.");
			Projectile01.unlock(true);

			// 雪球 Snowball
			ActorTrait Projectile02 = new ActorTrait();
			Projectile02.id = "projectile02";
			Projectile02.path_icon = "ui/icons/Projectile/Projectile02";
			Projectile02.group_id = "magic_bullet";
			Projectile02.rarity = Rarity.R2_Epic;
			Projectile02.rate_birth = 0;
			Projectile02.rate_inherit = 1;
			Projectile02.base_stats = new BaseStats();
			Projectile02.base_stats.addTag("immunity_cold");							//添加數值
			Projectile02.action_special_effect = new WorldAction(Traits01Actions.Projectile_02);
//			Projectile02.action_attack_target = new AttackAction(Traits01Actions.Projectile_O02);
//			Projectile02.action_get_hit = new GetHitAction(Traits01Actions.Projectile_O02);
			AssetManager.traits.add(Projectile02);
			addToLocalizedLibrary("ch",Projectile02.id, "魔彈:雪球", "投擲雪球的魔法");
			addToLocalizedLibrary("en",Projectile02.id, "Magic Bullet:Snowball", "Snowball throwing magic.");
			Projectile02.unlock(true);

			// 箭矢 Arrow
			ActorTrait Projectile03 = new ActorTrait();
			Projectile03.id = "projectile03";
			Projectile03.path_icon = "ui/icons/Projectile/Projectile03";
			Projectile03.group_id = "magic_bullet";
			Projectile03.rarity = Rarity.R2_Epic;
			Projectile03.rate_birth = 0;
			Projectile03.rate_inherit = 1;
			Projectile03.base_stats = new BaseStats();									//添加數值
			Projectile03.action_special_effect = new WorldAction(Traits01Actions.Projectile_03);
//			Projectile03.action_attack_target = new AttackAction(Traits01Actions.Projectile_O03);
//			Projectile03.action_get_hit = new GetHitAction(Traits01Actions.Projectile_O03);
			AssetManager.traits.add(Projectile03);
			addToLocalizedLibrary("ch",Projectile03.id, "魔彈:箭矢", "射出箭矢的魔法");
			addToLocalizedLibrary("en",Projectile03.id, "Magic Bullet:Arrow", "Magic that shoots arrows.");
			Projectile03.unlock(true);

			// 火把 Torch
			ActorTrait Projectile04 = new ActorTrait();
			Projectile04.id = "projectile04";
			Projectile04.path_icon = "ui/icons/Projectile/Projectile04";
			Projectile04.group_id = "magic_bullet";
			Projectile04.rarity = Rarity.R2_Epic;
			Projectile04.rate_birth = 0;
			Projectile04.rate_inherit = 1;
			Projectile04.base_stats = new BaseStats();									//添加數值
			Projectile04.base_stats.addTag("immunity_fire");
			Projectile04.action_special_effect = new WorldAction(Traits01Actions.Projectile_04);
//			Projectile04.action_attack_target = new AttackAction(Traits01Actions.Projectile_O04);
//			Projectile04.action_get_hit = new GetHitAction(Traits01Actions.Projectile_O04);
			AssetManager.traits.add(Projectile04);
			addToLocalizedLibrary("ch",Projectile04.id, "魔彈:火把", "投擲大量火把的魔法");
			addToLocalizedLibrary("en",Projectile04.id, "Magic Bullet:Torch", "A magic that throws a lot of torches.");
			Projectile04.unlock(true);

			// 火彈 Fire bomb
			ActorTrait Projectile05 = new ActorTrait();
			Projectile05.id = "projectile05";
			Projectile05.path_icon = "ui/icons/Projectile/Projectile05";
			Projectile05.group_id = "magic_bullet";
			Projectile05.rarity = Rarity.R2_Epic;
			Projectile05.rate_birth = 0;
			Projectile05.rate_inherit = 1;
			Projectile05.base_stats = new BaseStats();									//添加數值
			Projectile05.base_stats.addTag("immunity_fire");
			Projectile05.action_special_effect = new WorldAction(Traits01Actions.Projectile_05);
//			Projectile05.action_attack_target = new AttackAction(Traits01Actions.Projectile_O05);
//			Projectile05.action_get_hit = new GetHitAction(Traits01Actions.Projectile_O05);
			AssetManager.traits.add(Projectile05);
			addToLocalizedLibrary("ch",Projectile05.id, "魔彈:火彈", "投擲大量火彈的魔法");
			addToLocalizedLibrary("en",Projectile05.id, "Magic Bullet:Fire bomb", "A magic that throws a large number of fire bombs.");
			Projectile05.unlock(true);

			// 死靈彈 Necromancer Bullet
			ActorTrait Projectile06 = new ActorTrait();
			Projectile06.id = "projectile06";
			Projectile06.path_icon = "ui/icons/Projectile/Projectile06";
			Projectile06.group_id = "magic_bullet";
			Projectile06.rarity = Rarity.R2_Epic;
			Projectile06.rate_birth = 0;
			Projectile06.rate_inherit = 1;
			Projectile06.base_stats = new BaseStats();									//添加數值
			Projectile06.action_special_effect = new WorldAction(Traits01Actions.Projectile_06);
//			Projectile06.action_attack_target = new AttackAction(Traits01Actions.Projectile_O06);
//			Projectile06.action_get_hit = new GetHitAction(Traits01Actions.Projectile_O06);
			AssetManager.traits.add(Projectile06);
			addToLocalizedLibrary("ch",Projectile06.id, "魔彈:死靈彈", "射出顱骨的魔法");
			addToLocalizedLibrary("en",Projectile06.id, "Magic Bullet:Necromancer Bullet", "Magic that shoots out skulls and bones.");
			Projectile06.unlock(true);

			// 赤火彈 Red Orb
			ActorTrait Projectile07 = new ActorTrait();
			Projectile07.id = "projectile07";
			Projectile07.path_icon = "ui/icons/Projectile/Projectile07";
			Projectile07.group_id = "magic_bullet";
			Projectile07.rarity = Rarity.R2_Epic;
			Projectile07.rate_birth = 0;
			Projectile07.rate_inherit = 1;
			Projectile07.base_stats = new BaseStats();									//添加數值
			Projectile07.base_stats.addTag("immunity_fire");
			Projectile07.action_special_effect = new WorldAction(Traits01Actions.Projectile_07);
//			Projectile07.action_attack_target = new AttackAction(Traits01Actions.Projectile_O07);
//			Projectile07.action_get_hit = new GetHitAction(Traits01Actions.Projectile_O07);
			AssetManager.traits.add(Projectile07);
			addToLocalizedLibrary("ch",Projectile07.id, "魔彈:赤火彈", "射出帶有火焰魔力的魔法");
			addToLocalizedLibrary("en",Projectile07.id, "Magic Bullet:Red Orb", "Shoots a projectile with fire magic.");
			Projectile07.unlock(true);

			// 藍冰彈 Freeze Orb
			ActorTrait Projectile08 = new ActorTrait();
			Projectile08.id = "projectile08";
			Projectile08.path_icon = "ui/icons/Projectile/Projectile08";
			Projectile08.group_id = "magic_bullet";
			Projectile08.rarity = Rarity.R2_Epic;
			Projectile08.rate_birth = 0;
			Projectile08.rate_inherit = 1;
			Projectile08.base_stats = new BaseStats();									//添加數值
			Projectile08.base_stats.addTag("immunity_cold");
			Projectile08.action_special_effect = new WorldAction(Traits01Actions.Projectile_08);
//			Projectile08.action_attack_target = new AttackAction(Traits01Actions.Projectile_O08);
//			Projectile08.action_get_hit = new GetHitAction(Traits01Actions.Projectile_O08);
			AssetManager.traits.add(Projectile08);
			addToLocalizedLibrary("ch",Projectile08.id, "魔彈:藍冰彈", "射出帶有冰魔力的魔法");
			addToLocalizedLibrary("en",Projectile08.id, "Magic Bullet:Freeze Orb", "hoots a projectile with freezing magic.");
			Projectile08.unlock(true);

			// 綠藤彈 Green Orb
			ActorTrait Projectile09 = new ActorTrait();
			Projectile09.id = "projectile09";
			Projectile09.path_icon = "ui/icons/Projectile/Projectile09";
			Projectile09.group_id = "magic_bullet";
			Projectile09.rarity = Rarity.R2_Epic;
			Projectile09.rate_birth = 0;
			Projectile09.rate_inherit = 1;
			Projectile09.base_stats = new BaseStats();									//添加數值
			Projectile09.action_special_effect = new WorldAction(Traits01Actions.Projectile_09);
//			Projectile09.action_attack_target = new AttackAction(Traits01Actions.Projectile_O09);
//			Projectile09.action_get_hit = new GetHitAction(Traits01Actions.Projectile_O09);
			AssetManager.traits.add(Projectile09);
			addToLocalizedLibrary("ch",Projectile09.id, "魔彈:綠藤彈", "射出帶有藤蔓魔力的魔法");
			addToLocalizedLibrary("en",Projectile09.id, "Magic Bullet:Green Orb", "Shoots a projectile with vine magic.");
			Projectile09.unlock(true);

			// 酸彈 Acid Ball
			ActorTrait Projectile10 = new ActorTrait();
			Projectile10.id = "projectile10";
			Projectile10.path_icon = "ui/icons/Projectile/Projectile10";
			Projectile10.group_id = "magic_bullet";
			Projectile10.rarity = Rarity.R2_Epic;
			Projectile10.rate_birth = 0;
			Projectile10.rate_inherit = 1;
			Projectile10.base_stats = new BaseStats();									//添加數值
			Projectile10.action_special_effect = new WorldAction(Traits01Actions.Projectile_10);
//			Projectile10.action_attack_target = new AttackAction(Traits01Actions.Projectile_O10);
//			Projectile10.action_get_hit = new GetHitAction(Traits01Actions.Projectile_O10);
			AssetManager.traits.add(Projectile10);
			addToLocalizedLibrary("ch",Projectile10.id, "魔彈:酸彈", "帶有酸蝕之力的魔法彈");
			addToLocalizedLibrary("en",Projectile10.id, "Magic Bullet:Acid Ball", "Magical projectile with acidic power.");
			Projectile10.unlock(true);

			// 砲彈 Cannon Ball 
			ActorTrait Projectile11 = new ActorTrait();
			Projectile11.id = "projectile11";
			Projectile11.path_icon = "ui/icons/Projectile/Projectile11";
			Projectile11.group_id = "magic_bullet";
			Projectile11.rarity = Rarity.R2_Epic;
			Projectile11.rate_birth = 0;
			Projectile11.rate_inherit = 1;
			Projectile11.base_stats = new BaseStats();									//添加數值
			Projectile11.action_special_effect = new WorldAction(Traits01Actions.Projectile_11);
//			Projectile11.action_attack_target = new AttackAction(Traits01Actions.Projectile_O11);
//			Projectile11.action_get_hit = new GetHitAction(Traits01Actions.Projectile_O11);
			AssetManager.traits.add(Projectile11);
			addToLocalizedLibrary("ch",Projectile11.id, "魔彈:砲彈", "可以與大砲匹敵的魔法彈");
			addToLocalizedLibrary("en",Projectile11.id, "Magic Bullet:Cannon Ball", "Magic bullets that can rival cannons.");
			Projectile11.unlock(true);

			// 等離子 Plasma Ball 
			ActorTrait Projectile12 = new ActorTrait();
			Projectile12.id = "projectile12";
			Projectile12.path_icon = "ui/icons/Projectile/Projectile12";
			Projectile12.group_id = "magic_bullet";
			Projectile12.rarity = Rarity.R2_Epic;
			Projectile12.rate_birth = 0;
			Projectile12.rate_inherit = 1;
			Projectile12.base_stats = new BaseStats();									//添加數值
			Projectile12.action_special_effect = new WorldAction(Traits01Actions.Projectile_12);
//			Projectile12.action_attack_target = new AttackAction(Traits01Actions.Projectile_O12);
//			Projectile12.action_get_hit = new GetHitAction(Traits01Actions.Projectile_O12);
			AssetManager.traits.add(Projectile12);
			addToLocalizedLibrary("ch",Projectile12.id, "魔彈:等離子", "射出等離子的魔法");
			addToLocalizedLibrary("en",Projectile12.id, "Magic Bullet:Plasma Ball", "Magic that shoots plasma.");
			Projectile12.unlock(true);

			// 鎗彈 Shotgun Bullet  
			ActorTrait Projectile13 = new ActorTrait();
			Projectile13.id = "projectile13";
			Projectile13.path_icon = "ui/icons/Projectile/Projectile13";
			Projectile13.group_id = "magic_bullet";
			Projectile13.rarity = Rarity.R2_Epic;
			Projectile13.rate_birth = 0;
			Projectile13.rate_inherit = 1;
			Projectile13.base_stats = new BaseStats();									//添加數值
			Projectile13.action_special_effect = new WorldAction(Traits01Actions.Projectile_13);
//			Projectile13.action_attack_target = new AttackAction(Traits01Actions.Projectile_O13);
//			Projectile13.action_get_hit = new GetHitAction(Traits01Actions.Projectile_O13);
			AssetManager.traits.add(Projectile13);
			addToLocalizedLibrary("ch",Projectile13.id, "魔彈:鎗彈", "進行火器射擊的魔法");
			addToLocalizedLibrary("en",Projectile13.id, "Magic Bullet:Bullets", "Magic of performing firearm shooting.");
			Projectile13.unlock(true);

			// 狂彈 Madness Ball  
			ActorTrait Projectile14 = new ActorTrait();
			Projectile14.id = "projectile14";
			Projectile14.path_icon = "ui/icons/Projectile/Projectile14";
			Projectile14.group_id = "magic_bullet";
			Projectile14.rarity = Rarity.R2_Epic;
			Projectile14.rate_birth = 0;
			Projectile14.rate_inherit = 1;
			Projectile14.base_stats = new BaseStats();									//添加數值
			Projectile14.action_special_effect = new WorldAction(Traits01Actions.Projectile_14);
//			Projectile14.action_attack_target = new AttackAction(Traits01Actions.Projectile_O14);
//			Projectile14.action_get_hit = new GetHitAction(Traits01Actions.Projectile_O14);
			AssetManager.traits.add(Projectile14);
			addToLocalizedLibrary("ch",Projectile14.id, "魔彈:狂彈", "射出帶有發狂力量的魔法彈");
			addToLocalizedLibrary("en",Projectile14.id, "Magic Bullet:Madness Ball", "Shoots a magic bullet with Madness power.");
			Projectile14.unlock(true);

			// 火球彈 Fire Ball  
			ActorTrait Projectile15 = new ActorTrait();
			Projectile15.id = "projectile15";
			Projectile15.path_icon = "ui/icons/Projectile/Projectile15";
			Projectile15.group_id = "magic_bullet";
			Projectile15.rarity = Rarity.R2_Epic;
			Projectile15.rate_birth = 0;
			Projectile15.rate_inherit = 1;
			Projectile15.base_stats = new BaseStats();									//添加數值
			Projectile15.base_stats.addTag("immunity_fire");
			Projectile15.action_special_effect = new WorldAction(Traits01Actions.Projectile_15);
//			Projectile15.action_attack_target = new AttackAction(Traits01Actions.Projectile_O15);
//			Projectile15.action_get_hit = new GetHitAction(Traits01Actions.Projectile_O15);
			AssetManager.traits.add(Projectile15);
			addToLocalizedLibrary("ch",Projectile15.id, "魔彈:火球彈", "射出帶有龐大火焰之力的魔法彈");
			addToLocalizedLibrary("en",Projectile15.id, "Magic Bullet:Fire Ball", "Shoots a magic bullet with huge fire power.");
			Projectile15.unlock(true);

			#endregion
			#region 強化		Status Up
			//巨人化 Giantization
			ActorTrait Skill0101 = new ActorTrait();
			Skill0101.id = "status_powerup";
			Skill0101.path_icon = "ui/icons/Skill/Skill0101";
			Skill0101.group_id = "statuseup";
			Skill0101.rarity = Rarity.R1_Rare;
			Skill0101.rate_birth = 0;
			Skill0101.rate_inherit = 1;
			Skill0101.action_special_effect = new WorldAction(Traits01Actions.powerupEffect);
			AssetManager.traits.add(Skill0101);
			addToLocalizedLibrary("ch",Skill0101.id, "強化:巨人化", "讓我們一起變成巨人");
			addToLocalizedLibrary("en",Skill0101.id, "StatusUp:Giantization", "Let's become giants together.");
			Skill0101.unlock(true);

			//加速化 Acceleration
			ActorTrait Skill0102 = new ActorTrait();
			Skill0102.id = "status_caffeinated";
			Skill0102.path_icon = "ui/icons/Skill/Skill0102";
			Skill0102.group_id = "statuseup";
			Skill0102.rarity = Rarity.R1_Rare;
			Skill0102.rate_birth = 0;
			Skill0102.rate_inherit = 1;
			Skill0102.action_special_effect = new WorldAction(Traits01Actions.caffeinatedEffect);
			AssetManager.traits.add(Skill0102);
			addToLocalizedLibrary("ch",Skill0102.id, "強化:加速化", "隨著咖啡因的亢奮一同,思考與身體一同加速");
			addToLocalizedLibrary("en",Skill0102.id, "StatusUp:Acceleration", "With the excitement of caffeine, thinking and body speed up.");
			Skill0102.unlock(true);

			//充能化 Energized
			ActorTrait Skill0103 = new ActorTrait();
			Skill0103.id = "status_enchanted";
			Skill0103.path_icon = "ui/icons/Skill/Skill0103";
			Skill0103.group_id = "statuseup";
			Skill0103.rarity = Rarity.R1_Rare;
			Skill0103.rate_birth = 0;
			Skill0103.rate_inherit = 1;
			Skill0103.action_special_effect = new WorldAction(Traits01Actions.enchantedEffect);
			AssetManager.traits.add(Skill0103);
			addToLocalizedLibrary("ch",Skill0103.id, "強化:充能化", "他感覺狀態絕佳,你也是如此嗎?");
			addToLocalizedLibrary("en",Skill0103.id, "StatusUp:Energized", "He feels great, do you feel the same?");
			Skill0103.unlock(true);

			//狂暴化 Rage
			ActorTrait Skill0104 = new ActorTrait();
			Skill0104.id = "status_rage";
			Skill0104.path_icon = "ui/icons/Skill/Skill0104";
			Skill0104.group_id = "statuseup";
			Skill0104.rarity = Rarity.R1_Rare;
			Skill0104.rate_birth = 0;
			Skill0104.rate_inherit = 1;
			Skill0104.action_special_effect = new WorldAction(Traits01Actions.rageEffect);
			AssetManager.traits.add(Skill0104);
			addToLocalizedLibrary("ch",Skill0104.id, "強化:狂暴化", "十分的暴躁!彷彿像是要變成惡魔一般");
			addToLocalizedLibrary("en",Skill0104.id, "StatusUp:Rage", "Very irritable! As if turning into a demon.");
			Skill0104.unlock(true);

			//法力化 Spell boost
			ActorTrait Skill0105 = new ActorTrait();
			Skill0105.id = "status_spellboost";
			Skill0105.path_icon = "ui/icons/Skill/Skill0105";
			Skill0105.group_id = "statuseup";
			Skill0105.rarity = Rarity.R1_Rare;
			Skill0105.rate_birth = 0;
			Skill0105.rate_inherit = 1;
			Skill0105.action_special_effect = new WorldAction(Traits01Actions.spellboostEffect);
			AssetManager.traits.add(Skill0105);
			addToLocalizedLibrary("ch",Skill0105.id, "強化:法力化", "現在真正的樂趣開始了");
			addToLocalizedLibrary("en",Skill0105.id, "StatusUp:Spell boost", "Now the real fun begins.");
			Skill0105.unlock(true);

			//動力化 Motivated
			ActorTrait Skill0106 = new ActorTrait();
			Skill0106.id = "status_motivated";
			Skill0106.path_icon = "ui/icons/Skill/Skill0106";
			Skill0106.group_id = "statuseup";
			Skill0106.rarity = Rarity.R1_Rare;
			Skill0106.rate_birth = 0;
			Skill0106.rate_inherit = 1;
			Skill0106.action_special_effect = new WorldAction(Traits01Actions.motivatedEffect);
			AssetManager.traits.add(Skill0106);
			addToLocalizedLibrary("ch",Skill0106.id, "強化:動力化", "他正在盡力！");//中文名稱與介紹
			addToLocalizedLibrary("en",Skill0106.id, "StatusUp:Motivated", "He is trying his best!");//英文名稱與介紹
			Skill0106.unlock(true);

			//防護化 Shield
			ActorTrait Skill0107 = new ActorTrait();
			Skill0107.id = "status_shield";
			Skill0107.path_icon = "ui/icons/Skill/Skill0107";
			Skill0107.group_id = "statuseup";
			Skill0107.rarity = Rarity.R1_Rare;
			Skill0107.rate_birth = 0;
			Skill0107.rate_inherit = 1;
			Skill0107.action_special_effect = new WorldAction(Traits01Actions.shieldEffect);
			AssetManager.traits.add(Skill0107);
			addToLocalizedLibrary("ch",Skill0107.id, "強化:防護化", "免受損壞");//中文名稱與介紹
			addToLocalizedLibrary("en",Skill0107.id, "StatusUp:Shield", "Protected from damage.");//英文名稱與介紹
			Skill0107.unlock(true);

			//無敵化 Invincible ★ 1
			ActorTrait Skill0108 = new ActorTrait();
			Skill0108.id = "status_invincible";
			Skill0108.path_icon = "ui/icons/Skill/Skill0108";
			Skill0108.group_id = "statuseup";
			Skill0108.rarity = Rarity.R3_Legendary;
			Skill0108.rate_birth = 0;
			Skill0108.rate_inherit = 1;
			WorldAction combinedAction0108 = (WorldAction)Delegate.Combine(
			new WorldAction(Traits01Actions.InvincibleEffect),
			//new WorldAction(Traits01Actions.addFavorite1),
			new WorldAction(Traits01Actions.TraittAddRemove0));
			Skill0108.action_special_effect = combinedAction0108;
			//Skill0108.action_special_effect = new WorldAction(Traits01Actions.InvincibleEffect);
			AssetManager.traits.add(Skill0108);
			addToLocalizedLibrary("ch",Skill0108.id, "強化:無敵化", "他只是…");//中文名稱與介紹
			addToLocalizedLibrary("en",Skill0108.id, "StatusUp:Invincible", "He just...");//英文名稱與介紹
			Skill0108.unlock(true);

			//激勵化 Inspired
			ActorTrait Skill0109 = new ActorTrait();
			Skill0109.id = "status_inspired";
			Skill0109.path_icon = "ui/icons/Skill/Skill0109";
			Skill0109.group_id = "statuseup";
			Skill0109.rarity = Rarity.R2_Epic;
			Skill0109.rate_birth = 0;
			Skill0109.rate_inherit = 1;
			WorldAction combinedAction0109 = (WorldAction)Delegate.Combine(
			new WorldAction(Traits01Actions.InspiredEffect));
			Skill0109.action_special_effect = combinedAction0109;
			AssetManager.traits.add(Skill0109);
			addToLocalizedLibrary("ch",Skill0109.id, "強化:激勵化", "未來一片光明");//中文名稱與介紹
			addToLocalizedLibrary("en",Skill0109.id, "StatusUp:Inspired", "The future looks bright");//英文名稱與介紹
			Skill0109.unlock(true);

			//ALL FOR ONE ★ 1
			ActorTrait Skill0198 = new ActorTrait();
			Skill0198.id = "status_afo";
			Skill0198.path_icon = "ui/icons/Skill/Skill0198";
			Skill0198.group_id = "statuseup";
			Skill0198.rarity = Rarity.R3_Legendary;
			Skill0198.rate_birth = 0;
			Skill0198.rate_inherit = 1;
			WorldAction combinedAction0198 = (WorldAction)Delegate.Combine(
			new WorldAction(Traits01Actions.SuperEffect1),
			//new WorldAction(Traits01Actions.addFavorite1),
			new WorldAction(Traits01Actions.TraittAddRemove0));
			Skill0198.action_special_effect = combinedAction0198;
			//Skill0198.action_special_effect = new WorldAction(Traits01Actions.SuperEffect1);
			AssetManager.traits.add(Skill0198);
			addToLocalizedLibrary("ch",Skill0198.id, "強化:ALL FOR ONE", "成為英雄的人");//中文名稱與介紹
			addToLocalizedLibrary("en",Skill0198.id, "StatusUp:ALL FOR ONE", "The one who becomes a hero");//英文名稱與介紹
			Skill0198.unlock(true);

			//ONE FOR ALL ★ 1
			ActorTrait Skill0199 = new ActorTrait();
			Skill0199.id = "status_ofa";
			Skill0199.path_icon = "ui/icons/Skill/Skill0199";
			Skill0199.group_id = "statuseup";
			Skill0199.rarity = Rarity.R3_Legendary;
			Skill0199.rate_birth = 0;
			Skill0199.rate_inherit = 1;
			WorldAction combinedAction0199 = (WorldAction)Delegate.Combine(
			new WorldAction(Traits01Actions.SuperEffect2),
			//new WorldAction(Traits01Actions.addFavorite1),
			new WorldAction(Traits01Actions.TraittAddRemove0));
			Skill0199.action_special_effect = combinedAction0199;
			//Skill0199.action_special_effect = new WorldAction(Traits01Actions.SuperEffect2);
			AssetManager.traits.add(Skill0199);
			addToLocalizedLibrary("ch",Skill0199.id, "強化:ONE FOR ALL", "創造英雄的人");//中文名稱與介紹
			addToLocalizedLibrary("en",Skill0199.id, "StatusUp:ONE FOR ALL", "The man who created heroes");//英文名稱與介紹
			Skill0199.unlock(true);
			#endregion
			#region 附加魔法	Add Magic
			//燃燒 Burning
			ActorTrait Skill0201 = new ActorTrait();
			Skill0201.id = "add_burning";
			Skill0201.path_icon = "ui/icons/Skill/Skill0201";
			Skill0201.group_id = "addmagic";
			Skill0201.rarity = Rarity.R2_Epic;
			Skill0201.rate_birth = 0;
			Skill0201.rate_inherit = 1;
			Skill0201.base_stats = new BaseStats();	
			Skill0201.base_stats.addTag("immunity_fire");
			Skill0201.action_attack_target = new AttackAction(Traits01Actions.Burning_Effect2);
			AssetManager.traits.add(Skill0201);
			addToLocalizedLibrary("ch",Skill0201.id, "附魔:燃燒", "焚燒的魔力!");//中文名稱與介紹
			addToLocalizedLibrary("en",Skill0201.id, "Add:Burning", "The Magic of Burning");//英文名稱與介紹
			Skill0201.unlock(true);

			//緩速 Slowdown
			ActorTrait Skill0202 = new ActorTrait();
			Skill0202.id = "add_slowdown";
			Skill0202.path_icon = "ui/icons/Skill/Skill0202";
			Skill0202.group_id = "addmagic";
			Skill0202.rarity = Rarity.R2_Epic;
			Skill0202.rate_birth = 0;
			Skill0202.rate_inherit = 1;
			Skill0202.action_attack_target = new AttackAction(Traits01Actions.Slow_Effect2);
			AssetManager.traits.add(Skill0202);
			addToLocalizedLibrary("ch",Skill0202.id, "附魔:緩速", "緩慢的魔力!");//中文名稱與介紹
			addToLocalizedLibrary("en",Skill0202.id, "Add:Slowdown", "The Magic of Slowdown");//英文名稱與介紹
			Skill0202.unlock(true);

			//冰結 Freeze
			ActorTrait Skill0203 = new ActorTrait();
			Skill0203.id = "add_frozen";
			Skill0203.path_icon = "ui/icons/Skill/Skill0203";
			Skill0203.group_id = "addmagic";
			Skill0203.rarity = Rarity.R2_Epic;
			Skill0203.rate_birth = 0;
			Skill0203.rate_inherit = 1;
			Skill0203.base_stats = new BaseStats();	
			Skill0203.base_stats.addTag("immunity_cold");
			Skill0203.action_attack_target = new AttackAction(Traits01Actions.Frozen_Effect2);
			AssetManager.traits.add(Skill0203);
			addToLocalizedLibrary("ch",Skill0203.id, "附魔:冰結", "凍結的魔力!");//中文名稱與介紹
			addToLocalizedLibrary("en",Skill0203.id, "Add:Freeze", "The Magic of Freeze");//英文名稱與介紹
			Skill0203.unlock(true);

			//猛毒 Poisonous
			ActorTrait Skill0204 = new ActorTrait();
			Skill0204.id = "add_poisonous";
			Skill0204.path_icon = "ui/icons/Skill/Skill0204";
			Skill0204.group_id = "addmagic";
			Skill0204.rarity = Rarity.R2_Epic;
			Skill0204.rate_birth = 0;
			Skill0204.rate_inherit = 1;
			Skill0204.action_attack_target = new AttackAction(Traits01Actions.Poisoned_Effect2);
			AssetManager.traits.add(Skill0204);
			addToLocalizedLibrary("ch",Skill0204.id, "附魔:猛毒", "毒素的魔力!");//中文名稱與介紹
			addToLocalizedLibrary("en",Skill0204.id, "Add:Poisonous", "The Magic of Poisonous");//英文名稱與介紹
			Skill0204.unlock(true);

			//病疫 Ash Fever Coughing
			ActorTrait Skill0205 = new ActorTrait();
			Skill0205.id = "add_afc";
			Skill0205.path_icon = "ui/icons/Skill/Skill0205";
			Skill0205.group_id = "addmagic";
			Skill0205.rarity = Rarity.R2_Epic;
			Skill0205.rate_birth = 0;
			Skill0205.rate_inherit = 1;
			Skill0205.action_attack_target = new AttackAction(Traits01Actions.AshFeverAndCoughingEffect2);
			AssetManager.traits.add(Skill0205);
			addToLocalizedLibrary("ch",Skill0205.id, "附魔:病疫", "病疫的魔力!");//中文名稱與介紹
			addToLocalizedLibrary("en",Skill0205.id, "Add:Ash Fever Coughing", "The Magic of Ash Fever Coughing");//英文名稱與介紹
			Skill0205.unlock(true);

			//沉默 silenced
			ActorTrait Skill0206 = new ActorTrait();
			Skill0206.id = "add_silenced";
			Skill0206.path_icon = "ui/icons/Skill/Skill0206";
			Skill0206.group_id = "addmagic";
			Skill0206.rarity = Rarity.R2_Epic;
			Skill0206.rate_birth = 0;
			Skill0206.rate_inherit = 1;
			Skill0206.action_attack_target = new AttackAction(Traits01Actions.Silenced_Effect);
			AssetManager.traits.add(Skill0206);
			addToLocalizedLibrary("ch",Skill0206.id, "附魔:沉默", "病疫的魔力!");//中文名稱與介紹
			addToLocalizedLibrary("en",Skill0206.id, "Add:Silenced", "The Magic of Silenced");//英文名稱與介紹
			Skill0206.unlock(true);

			//眩暈 Stunned  
			ActorTrait Skill0207 = new ActorTrait();
			Skill0207.id = "add_stunned";
			Skill0207.path_icon = "ui/icons/Skill/Skill0207";
			Skill0207.group_id = "addmagic";
			Skill0207.rarity = Rarity.R2_Epic;
			Skill0207.rate_birth = 0;
			Skill0207.rate_inherit = 1;
			Skill0207.action_attack_target = new AttackAction(Traits01Actions.Stunned_Effect);
			AssetManager.traits.add(Skill0207);
			addToLocalizedLibrary("ch",Skill0207.id, "附魔:眩暈", "眩暈的魔力!");//中文名稱與介紹
			addToLocalizedLibrary("en",Skill0207.id, "Add:Stunned", "The Magic of Stunned");//英文名稱與介紹
			Skill0207.unlock(true);

			//溺水 Drowning  
			ActorTrait Skill0208 = new ActorTrait();
			Skill0208.id = "add_drowning";
			Skill0208.path_icon = "ui/icons/Skill/Skill0208";
			Skill0208.group_id = "addmagic";
			Skill0208.rarity = Rarity.R0_Normal;
			Skill0208.rate_birth = 0;
			Skill0208.rate_inherit = 1;
			Skill0208.action_attack_target = new AttackAction(Traits01Actions.Drowning_Effect);
			AssetManager.traits.add(Skill0208);
			addToLocalizedLibrary("ch",Skill0208.id, "附魔:溺水", "溺水的魔力!");//中文名稱與介紹
			addToLocalizedLibrary("en",Skill0208.id, "Add:Drowning", "The Magic of Drowning");//英文名稱與介紹
			Skill0208.unlock(true);

			//混亂 Confused  
			ActorTrait Skill0209 = new ActorTrait();
			Skill0209.id = "add_confused";
			Skill0209.path_icon = "ui/icons/Skill/Skill0209";
			Skill0209.group_id = "addmagic";
			Skill0209.rarity = Rarity.R0_Normal;
			Skill0209.rate_birth = 0;
			Skill0209.rate_inherit = 1;
			Skill0209.action_attack_target = new AttackAction(Traits01Actions.Confused_Effect);
			AssetManager.traits.add(Skill0209);
			addToLocalizedLibrary("ch",Skill0209.id, "附魔:混亂", "混亂的魔力!");//中文名稱與介紹
			addToLocalizedLibrary("en",Skill0209.id, "Add:Confused", "The Magic of Confused");//英文名稱與介紹
			Skill0209.unlock(true);

			//?? Unknown
			ActorTrait Skill0297 = new ActorTrait();
			Skill0297.id = "add_unknown";
			Skill0297.path_icon = "ui/icons/Skill/Skill0297";
			Skill0297.group_id = "addmagic";
			Skill0297.rarity = Rarity.R3_Legendary;
			Skill0297.rate_birth = 0;
			Skill0297.rate_inherit = 1;
			Skill0297.base_stats = new BaseStats();	
			Skill0297.base_stats.addTag("immunity_fire");
			Skill0297.base_stats.addTag("immunity_cold");
			Skill0297.action_attack_target = new AttackAction(Traits01Actions.Random_Effect2);
			Skill0297.action_get_hit = new GetHitAction(Traits01Actions.Random_Effect1);
			AssetManager.traits.add(Skill0297);
			addToLocalizedLibrary("ch",Skill0297.id, "附魔:？？", "？？的魔力!");//中文名稱與介紹
			addToLocalizedLibrary("en",Skill0297.id, "Add:Unknown", "The Magic of Unknown");//英文名稱與介紹
			Skill0297.unlock(true);

			//詛咒 Cursed
			ActorTrait Skill0298 = new ActorTrait();
			Skill0298.id = "add_cursed";
			Skill0298.path_icon = "ui/icons/Skill/Skill0298";
			Skill0298.group_id = "addmagic";
			Skill0298.rarity = Rarity.R3_Legendary;
			Skill0298.rate_birth = 0;
			Skill0298.rate_inherit = 1;
			Skill0298.action_attack_target = new AttackAction(Traits01Actions.CursedAttack);
			AssetManager.traits.add(Skill0298);
			addToLocalizedLibrary("ch",Skill0298.id, "附魔:詛咒", "詛咒的魔力!");//中文名稱與介紹
			addToLocalizedLibrary("en",Skill0298.id, "Add:Cursed", "The Magic of Cursed");//英文名稱與介紹
			Skill0298.unlock(true);

			//死亡 Death ★ 1
			ActorTrait Skill0299 = new ActorTrait();
			Skill0299.id = "add_death";
			Skill0299.path_icon = "ui/icons/Skill/Skill0299";
			Skill0299.group_id = "addmagic";
			Skill0299.rarity = Rarity.R3_Legendary;
			Skill0299.rate_birth = 0;
			Skill0299.rate_inherit = 1;
			WorldAction combinedAction0299 = (WorldAction)Delegate.Combine(
			//new WorldAction(Traits01Actions.addFavorite1),
			new WorldAction(Traits01Actions.TraittAddRemove0));
			Skill0299.action_special_effect = combinedAction0299;
			Skill0299.action_attack_target = new AttackAction(Traits01Actions.DeathEffect0);
			AssetManager.traits.add(Skill0299);
			addToLocalizedLibrary("ch",Skill0299.id, "附魔:死亡", "死亡的魔力!");//中文名稱與介紹
			addToLocalizedLibrary("en",Skill0299.id, "Add:Death", "The Magic of Death");//英文名稱與介紹
			Skill0299.unlock(true);
			#endregion
			#region 神聖術		Holy Arts
			//淨化抗體 Healing Antibodies
			ActorTrait Skill0301 = new ActorTrait();
			Skill0301.id = "holyarts_ha";
			Skill0301.path_icon = "ui/icons/Skill/Skill0301";
			Skill0301.group_id = "holyarts";
			Skill0301.rarity = Rarity.R3_Legendary;
			Skill0301.rate_birth = 0;
			Skill0301.rate_inherit = 1;
			WorldAction combinedAction_0301 = (WorldAction)Delegate.Combine(
			new WorldAction(Traits01Actions.removeStatus),
			new WorldAction(Traits01Actions.removeTrait),
			new WorldAction(Traits01Actions.TraittAddRemove0));
			Skill0301.action_special_effect = combinedAction_0301;
			Skill0301.action_attack_target = new AttackAction(Traits01Actions.MadnessCure);
			AssetManager.traits.add(Skill0301);
			addToLocalizedLibrary("ch",Skill0301.id, "神聖術:淨化抗體", "身體不會產生異常，還能夠消除他人的異常");//
			addToLocalizedLibrary("en",Skill0301.id, "Holy Arts:Healing Antibodies", "The body will not produce abnormalities, and can also eliminate the abnormalities of others.");//
			Skill0301.unlock(true);

			//淨化之滴 Cure
			ActorTrait Skill0302 = new ActorTrait();
			Skill0302.id = "holyarts_cure";
			Skill0302.path_icon = "ui/icons/Skill/Skill0302";
			Skill0302.group_id = "holyarts";
			Skill0302.rarity = Rarity.R2_Epic;
			Skill0302.rate_birth = 0;
			Skill0302.rate_inherit = 1;
			Skill0302.action_special_effect = new WorldAction(Traits01Actions.Divine_Arts1);
			AssetManager.traits.add(Skill0302);
			addToLocalizedLibrary("ch",Skill0302.id, "神聖術:淨化之滴", "淨化異常特質");//中文名稱與介紹
			addToLocalizedLibrary("en",Skill0302.id, "Holy Arts:Cure", "Purify abnormal traits.");//英文名稱與介紹
			Skill0302.unlock(true);

			//生命之血 Heal
			ActorTrait Skill0303 = new ActorTrait();
			Skill0303.id = "holyarts_heal";
			Skill0303.path_icon = "ui/icons/Skill/Skill0303";
			Skill0303.group_id = "holyarts";
			Skill0303.rarity = Rarity.R2_Epic;
			Skill0303.rate_birth = 0;
			Skill0303.rate_inherit = 1;
			Skill0303.action_special_effect = new WorldAction(Traits01Actions.Divine_Arts2);
			AssetManager.traits.add(Skill0303);
			addToLocalizedLibrary("ch",Skill0303.id, "神聖術:生命之血", "恢復生命的紅色水滴");//中文名稱與介紹
			addToLocalizedLibrary("en",Skill0303.id, "Holy Arts:Heal", "Red drops of water that restore life.");//英文名稱與介紹
			Skill0303.unlock(true);

			//聖雫 HealCure
			ActorTrait Skill0304 = new ActorTrait();
			Skill0304.id = "holyarts_healcure";
			Skill0304.path_icon = "ui/icons/Skill/Skill0304";
			Skill0304.group_id = "holyarts";
			Skill0304.rarity = Rarity.R3_Legendary;
			Skill0304.rate_birth = 0;
			Skill0304.rate_inherit = 1;
			WorldAction combinedAction_0304 = (WorldAction)Delegate.Combine(
			new WorldAction(Traits01Actions.Divine_Arts1),
			new WorldAction(Traits01Actions.Divine_Arts2));
			Skill0304.action_special_effect = combinedAction_0304;
			AssetManager.traits.add(Skill0304);
			addToLocalizedLibrary("ch",Skill0304.id, "神聖術:聖雫", "『淨化之滴』與『生命之血』的融合");//中文名稱與介紹
			addToLocalizedLibrary("en",Skill0304.id, "Holy Arts:HealCure", "Combination of Heal and Cure.");//英文名稱與介紹
			Skill0304.unlock(true);

			//自動恢復 HP
			ActorTrait Skill0305 = new ActorTrait();
			Skill0305.id = "holyarts_health";
			Skill0305.path_icon = "ui/icons/Skill/Skill0305";
			Skill0305.group_id = "holyarts";
			Skill0305.rarity = Rarity.R1_Rare;
			Skill0305.rate_birth = 0;
			Skill0305.rate_inherit = 1;
			Skill0305.action_special_effect = new WorldAction(Traits01Actions.Health_recovery);
			AssetManager.traits.add(Skill0305);
			addToLocalizedLibrary("ch",Skill0305.id, "神聖術:自動恢復(HP)", "自動恢復生命值的能力");//中文名稱與介紹
			addToLocalizedLibrary("en",Skill0305.id, "Holy Arts:Automatic recovery(HP)", "The ability to automatically restore health.");//英文名稱與介紹
			Skill0305.unlock(true);
			
			//自動恢復 MP
			ActorTrait Skill0306 = new ActorTrait();
			Skill0306.id = "holyarts_mana";
			Skill0306.path_icon = "ui/icons/Skill/Skill0306";
			Skill0306.group_id = "holyarts";
			Skill0306.rarity = Rarity.R1_Rare;
			Skill0306.rate_birth = 0;
			Skill0306.rate_inherit = 1;
			Skill0306.action_special_effect = new WorldAction(Traits01Actions.Mana_recovery);
			AssetManager.traits.add(Skill0306);
			addToLocalizedLibrary("ch",Skill0306.id, "神聖術:自動恢復(MP)", "自動恢復魔力值的能力");//中文名稱與介紹
			addToLocalizedLibrary("en",Skill0306.id, "Holy Arts:Automatic recovery(MP)", "The ability to automatically restore mana.");//英文名稱與介紹
			Skill0306.unlock(true);

			//自動恢復 SP
			ActorTrait Skill0307 = new ActorTrait();
			Skill0307.id = "holyarts_stamina";
			Skill0307.path_icon = "ui/icons/Skill/Skill0307";
			Skill0307.group_id = "holyarts";
			Skill0307.rarity = Rarity.R1_Rare;
			Skill0307.rate_birth = 0;
			Skill0307.rate_inherit = 1;
			Skill0307.action_special_effect = new WorldAction(Traits01Actions.Stamina_recovery);
			AssetManager.traits.add(Skill0307);
			addToLocalizedLibrary("ch",Skill0307.id, "神聖術:自動恢復(SP)", "自動恢復耐力值的能力");//中文名稱與介紹
			addToLocalizedLibrary("en",Skill0307.id, "Holy Arts:Automatic recovery(SP)", "The ability to automatically restore stamina.");//英文名稱與介紹
			Skill0307.unlock(true);

			//受胎 Annunciation ★ 2
			ActorTrait Skill0308 = new ActorTrait();
			Skill0308.id = "holyarts_annunciation";
			Skill0308.path_icon = "ui/icons/Skill/Skill0308";
			Skill0308.group_id = "holyarts";
			Skill0308.rarity = Rarity.R3_Legendary;
			Skill0308.rate_birth = 0;
			Skill0308.rate_inherit = 0;
			WorldAction combinedAction_0308 = (WorldAction)Delegate.Combine(
			new WorldAction(Traits01Actions.Annunciation),
			new WorldAction(Traits01Actions.Incubation),
			//new WorldAction(Traits01Actions.addFavorite1),
			new WorldAction(Traits01Actions.TraittAddRemove0));
			Skill0308.action_special_effect = combinedAction_0308;
			//Skill0308.action_special_effect = new WorldAction(Traits01Actions.Divine_Arts5);
			AssetManager.traits.add(Skill0308);
			addToLocalizedLibrary("ch",Skill0308.id, "神聖術:受胎術", "以神聖之力孕育生命,蛋會立刻孵化,生命會以驚人的速度來到這世界");//中文名稱與介紹
			addToLocalizedLibrary("en",Skill0308.id, "Holy Arts:Annunciation", "Giving birth to life with divine power.");//英文名稱與介紹
			Skill0308.unlock(true);

			//祝聖 Consecration
			ActorTrait Skill0309 = new ActorTrait();
			Skill0309.id = "holyarts_consecration";
			Skill0309.path_icon = "ui/icons/Skill/Skill0309";
			Skill0309.group_id = "holyarts";
			Skill0309.rarity = Rarity.R2_Epic;
			Skill0309.rate_birth = 0;
			Skill0309.rate_inherit = 1;
			Skill0309.action_special_effect = new WorldAction(Traits01Actions.Divine_Arts5);
			AssetManager.traits.add(Skill0309);
			addToLocalizedLibrary("ch",Skill0309.id, "神聖術:祝聖術", "傳播祝福的人");//中文名稱與介紹
			addToLocalizedLibrary("en",Skill0309.id, "Holy Arts:Consecration", "People who spread blessings.");//英文名稱與介紹
			Skill0309.unlock(true);

			//聖餐 Eucharist ★ 2
			ActorTrait Skill0310 = new ActorTrait();
			Skill0310.id = "holyarts_eucharist";
			Skill0310.path_icon = "ui/icons/Skill/Skill0310";
			Skill0310.group_id = "holyarts";
			Skill0310.rarity = Rarity.R3_Legendary;
			Skill0310.rate_birth = 0;
			Skill0310.rate_inherit = 1;
			WorldAction combinedAction_0310 = (WorldAction)Delegate.Combine(
			new WorldAction(Traits01Actions.RestoreAllyHunger),
			//new WorldAction(Traits01Actions.addFavorite1),
			new WorldAction(Traits01Actions.TraittAddRemove0));
			Skill0310.action_special_effect = combinedAction_0310;
			//Skill0310.action_special_effect = new WorldAction(Traits01Actions.RestoreAllyHunger);
			AssetManager.traits.add(Skill0310);
			addToLocalizedLibrary("ch",Skill0310.id, "神聖術:聖餐術", "願飢餓不存於世");//中文名稱與介紹
			addToLocalizedLibrary("en",Skill0310.id, "Holy Arts:Eucharist", "May hunger no longer exist in the world.");//英文名稱與介紹
			Skill0310.unlock(true);

			//絆 Bond
			ActorTrait Skill0311 = new ActorTrait();
			Skill0311.id = "holyarts_bond";
			Skill0311.path_icon = "ui/icons/Skill/Skill0311";
			Skill0311.group_id = "holyarts";
			Skill0311.rarity = Rarity.R2_Epic;
			Skill0311.rate_birth = 0;
			Skill0311.rate_inherit = 1;
			Skill0311.action_special_effect = new WorldAction(Traits01Actions.HolyArts_Bond_effect);
			AssetManager.traits.add(Skill0311);
			addToLocalizedLibrary("ch",Skill0311.id, "神聖術:絆", "聯繫彼此，免受誘惑的紐帶");//中文名稱與介紹
			addToLocalizedLibrary("en",Skill0311.id, "Holy Arts:Bond", "A bond that connects us and protects us from temptation.");//英文名稱與介紹
			Skill0311.unlock(true);

			//寧靜 Serenity
			ActorTrait Skill0312 = new ActorTrait();
			Skill0312.id = "holyarts_serenity";
			Skill0312.path_icon = "ui/icons/Skill/Skill0312";
			Skill0312.group_id = "holyarts";
			Skill0312.rarity = Rarity.R2_Epic;
			Skill0312.rate_birth = 0;
			Skill0312.rate_inherit = 1;
			WorldAction combinedAction_0312 = (WorldAction)Delegate.Combine(
			new WorldAction(Traits01Actions.StopErupting2),
			new WorldAction(Traits01Actions.Anti_tantrum_Effect),
			new WorldAction(Traits01Actions.Extinguished),
			//new WorldAction(Traits01Actions.addFavorite1), 
			new WorldAction(Traits01Actions.TraittAddRemove0));
			Skill0312.action_special_effect = combinedAction_0312;
			AssetManager.traits.add(Skill0312);
			addToLocalizedLibrary("ch",Skill0312.id, "神聖術:寧靜術", "所有的忿怒都會被平息");//中文名稱與介紹
			addToLocalizedLibrary("en",Skill0312.id, "Holy Arts:Serenity", "All anger will be appeased.");//英文名稱與介紹
			Skill0312.unlock(true);

			//降雨 Rainfall
			ActorTrait Skill0313 = new ActorTrait();
			Skill0313.id = "holyarts_rainfall";
			Skill0313.path_icon = "ui/icons/Skill/Skill0313";
			Skill0313.group_id = "holyarts";
			Skill0313.rarity = Rarity.R2_Epic;
			Skill0313.rate_birth = 0;
			Skill0313.rate_inherit = 1;
			WorldAction combinedAction_0313 = (WorldAction)Delegate.Combine(
			//new WorldAction(Traits01Actions.addFavorite1),
			new WorldAction(Traits01Actions.Rain00),
			new WorldAction(Traits01Actions.Rain01),
			new WorldAction(Traits01Actions.Rain02),
			new WorldAction(Traits01Actions.Rain03));
			Skill0313.action_special_effect = combinedAction_0313;
			AssetManager.traits.add(Skill0313);
			addToLocalizedLibrary("ch",Skill0313.id, "神聖術:降雨術", "能夠在適當的時機降下雨水");//中文名稱與介紹
			addToLocalizedLibrary("en",Skill0313.id, "Holy Arts:Rainfall", "Able to bring rain at the right time.");//英文名稱與介紹
			Skill0313.unlock(true);

			//懲戒 Punish
			ActorTrait Skill0314 = new ActorTrait();
			Skill0314.id = "holyarts_justice";
			Skill0314.path_icon = "ui/icons/Skill/Skill0314";
			Skill0314.group_id = "holyarts";
			Skill0314.rarity = Rarity.R3_Legendary;
			Skill0314.rate_birth = 0;
			Skill0314.rate_inherit = 1;

			WorldAction combinedAction_0314A = (WorldAction)Delegate.Combine(//被動能力
			new WorldAction(Traits01Actions.Judgment),
			new WorldAction(Traits01Actions.JusticeJavelin1),
			new WorldAction(Traits01Actions.BraveHelmetGet),
			new WorldAction(Traits01Actions.TraittAddRemove0));
			Skill0314.action_special_effect = combinedAction_0314A;

			AttackAction combinedAction_0314B = (AttackAction)Delegate.Combine(//攻擊能力
			new AttackAction(Traits01Actions.DefenseOff),
			new AttackAction(Traits01Actions.JusticeAttack),
			new AttackAction(Traits01Actions.JusticeBlade),
			new AttackAction(Traits01Actions.BraveEffect));
			Skill0314.action_attack_target = combinedAction_0314B;

			GetHitAction combinedAction_0314C = (GetHitAction)Delegate.Combine(//被傷能力
			new GetHitAction(Traits01Actions.JusticeDefense1),
			new GetHitAction(Traits01Actions.JusticeBlade),
			new GetHitAction(Items01Actions.braveATK),
			new GetHitAction(Traits01Actions.BraveEffect));
			Skill0314.action_get_hit = combinedAction_0314C;

			AssetManager.traits.add(Skill0314);
			addToLocalizedLibrary("ch",Skill0314.id, "神聖術:懲戒", "正義執行，懲戒邪惡");//中文名稱與介紹
			addToLocalizedLibrary("en",Skill0314.id, "Holy Arts:Punishing", "Execute justice and punish evil.");//英文名稱與介紹
			Skill0314.unlock(true);

				ActorTrait Skill0314_2 = new ActorTrait();
				Skill0314_2.id = "hope";
				Skill0314_2.path_icon = "ui/icons/Skill/Skill0314_2";
				Skill0314_2.group_id = "auxiliary_traits2";
				Skill0314_2.rarity = Rarity.R3_Legendary;
				Skill0314_2.rate_birth = 0;
				Skill0314_2.rate_inherit = 0;
				Skill0314_2.can_be_given = false;

				WorldAction combinedAction_0314_2A = (WorldAction)Delegate.Combine(//被動能力
				new WorldAction(Traits01Actions.ReAddBrave));
				Skill0314_2.action_special_effect = combinedAction_0314_2A;

				AssetManager.traits.add(Skill0314_2);
				addToLocalizedLibrary("ch",Skill0314_2.id, "希望之星", "世界重啟之時，他將再次成為勇者。");//中文名稱與介紹
				addToLocalizedLibrary("en",Skill0314_2.id, "Star of Hope", "When the world restarts, he will become a hero again.");//英文名稱與介紹
				Skill0314_2.unlock(true);

				ActorTrait Skill0314_3 = new ActorTrait();
				Skill0314_3.id = "monster";
				Skill0314_3.path_icon = "ui/icons/Skill/Skill0314_3";
				Skill0314_3.group_id = "auxiliary_traits2";
				Skill0314_3.rarity = Rarity.R2_Epic;
				Skill0314_3.rate_birth = 0;
				Skill0314_3.rate_inherit = 1;
				Skill0314_3.can_be_given = false;
				Skill0314_3.action_special_effect = new WorldAction(Traits01Actions.RemoveMonster);
				AssetManager.traits.add(Skill0314_3);
				addToLocalizedLibrary("ch",Skill0314_3.id, "牠是怪物", "");//中文名稱與介紹
				addToLocalizedLibrary("en",Skill0314_3.id, "He is monster", "");//英文名稱與介紹
				Skill0314_3.unlock(true);

			ActorTrait Skill0315 = new ActorTrait();
			Skill0315.id = "holyarts_divinelight";
			Skill0315.path_icon = "ui/icons/Skill/Skill0315";
			Skill0315.group_id = "holyarts";
			Skill0315.rarity = Rarity.R3_Legendary;
			Skill0315.rate_birth = 0;
			Skill0315.rate_inherit = 1;
			Skill0315.action_special_effect = new WorldAction(Traits01Actions.Miracle);

			Skill0315.action_attack_target = new AttackAction(Traits01Actions.DestroyEvil);

			GetHitAction combinedAction_0351C = (GetHitAction)Delegate.Combine(//被傷能力
			new GetHitAction(Traits01Actions.DestroyEvil),
			new GetHitAction(Traits01Actions.AnitUndeadDefense));
			Skill0315.action_get_hit = combinedAction_0351C;

			AssetManager.traits.add(Skill0315);
			addToLocalizedLibrary("ch",Skill0315.id, "神聖術:聖光", "淨化一切不祥的神聖光輝");
			addToLocalizedLibrary("en",Skill0315.id, "Holy Arts:Divine Light", "Purify all ominous divine light.");
			Skill0315.unlock(true);

			#endregion
			#region 邪咒法		Evil law
			//大咒法 The Great Curse ★ 2
			ActorTrait Skill0401 = new ActorTrait();
			Skill0401.id = "evillaw_tgc";
			Skill0401.path_icon = "ui/icons/Skill/Skill0401";
			Skill0401.group_id = "evil_law";
			Skill0401.rarity = Rarity.R3_Legendary;
			Skill0401.rate_birth = 0;
			Skill0401.rate_inherit = 1;
			Skill0401.base_stats = new BaseStats();									//添加數值
			Skill0401.base_stats.set("lifespan", 5f);									//壽命
			Skill0401.base_stats.set("multiplier_health", 0.50f);						//生命 %
			Skill0401.base_stats.set("multiplier_damage", 0.50f);						//傷害 %
			Skill0401.base_stats.set("multiplier_speed", 0.50f);						//移動速度 %
			Skill0401.base_stats.set("critical_chance", 0.10f);						//爆擊機率 %
			Skill0401.base_stats.set("multiplier_diplomacy", 0.20f);					//外交 %
			WorldAction combinedAction0401 = (WorldAction)Delegate.Combine(
			new WorldAction(Traits01Actions.Evil_Law1),
			new WorldAction(Traits01Actions.Tamed_Undead_Effect),
			new WorldAction(Traits01Actions.UndeadCall2),
			new WorldAction(Traits01Actions.Return),
			new WorldAction(Traits01Actions.ProtectOffspring),
			new WorldAction(Traits01Actions.addClan),
			new WorldAction(Traits01Actions.Blessing_of_the_Undead_King),
			//new WorldAction(Traits01Actions.addFavorite1),
			new WorldAction(Traits01Actions.TraittAddRemove1));
			Skill0401.action_special_effect = combinedAction0401;

			Skill0401.action_attack_target = new AttackAction(Traits01Actions.UndeadCall);

			GetHitAction combinedAction_0401C = (GetHitAction)Delegate.Combine(//被傷能力
			new GetHitAction(Items01Actions.CrownATK),
			new GetHitAction(Items01Actions.Anti_Soul_Defense),
			new GetHitAction(Items01Actions.Defense),
			new GetHitAction(Traits01Actions.UndeadCall));
			Skill0401.action_get_hit = combinedAction_0401C;

			Skill0401.action_death = (WorldAction)Delegate.Combine(Skill0401.action_death,
			new WorldAction(Traits01Actions.Transformation_Undead),
			new WorldAction(Items01Actions.UndeadRebirth));
			
			AssetManager.traits.add(Skill0401);
			addToLocalizedLibrary("ch",Skill0401.id, "邪咒法:大咒法", "傳播詛咒的邪咒");//中文名稱與介紹
			addToLocalizedLibrary("en",Skill0401.id, "Evil Law:The Great Curse", "Spread the curse.");//英文名稱與介紹
			Skill0401.unlock(true);

			//不死族的僕從 undead_servant
				ActorTrait Skill0401_2 = new ActorTrait();
				Skill0401_2.id = "undead_servant";
				Skill0401_2.path_icon = "ui/icons/Skill/Skill0401_2";
				Skill0401_2.group_id = "auxiliary_traits2";
				Skill0401_2.rarity = Rarity.R0_Normal;
				Skill0401_2.rate_birth = 0;
				Skill0401_2.rate_inherit = 0;
				Skill0401_2.can_be_given = false;
				Skill0401_2.base_stats = new BaseStats();								//添加數值
				Skill0401_2.base_stats.set("lifespan", 5f);									//壽命
				Skill0401_2.base_stats.set("multiplier_health", 0.25f);						//生命 %
				Skill0401_2.base_stats.set("multiplier_damage", 0.25f);						//傷害 %
				Skill0401_2.base_stats.set("multiplier_speed", 0.25f);						//移動速度 %
				WorldAction combinedAction0401_2 = (WorldAction)Delegate.Combine(
				new WorldAction(Traits01Actions.UndeadServant),
				new WorldAction(Traits01Actions.Return),
				new WorldAction(Traits01Actions.ProtectOffspring));
				Skill0401_2.action_special_effect = combinedAction0401_2;
				AssetManager.traits.add(Skill0401_2);
				addToLocalizedLibrary("ch",Skill0401_2.id, "契約", "");//中文名稱與介紹
				addToLocalizedLibrary("en",Skill0401_2.id, "Contract", "");//英文名稱與介紹
				Skill0401_2.unlock(true);

				ActorTrait Skill0401_3 = new ActorTrait();
				Skill0401_3.id = "undead_servant2";
				Skill0401_3.path_icon = "ui/icons/Skill/Skill0401_3";
				Skill0401_3.group_id = "auxiliary_traits2";
				Skill0401_3.rarity = Rarity.R1_Rare;
				Skill0401_3.rate_birth = 0;
				Skill0401_3.rate_inherit = 0;
				Skill0401_3.can_be_given = false;
				Skill0401_3.base_stats = new BaseStats();
				Skill0401_3.base_stats.set("lifespan", 7f);									//壽命
				Skill0401_3.base_stats.set("multiplier_health", 0.35f);						//生命 %
				Skill0401_3.base_stats.set("multiplier_damage", 0.35f);						//傷害 %
				Skill0401_3.base_stats.set("multiplier_speed", 0.35f);						//移動速度 %
				WorldAction combinedAction0401_3 = (WorldAction)Delegate.Combine(
				new WorldAction(Traits01Actions.UndeadServant),
				new WorldAction(Traits01Actions.Return),
				new WorldAction(Traits01Actions.Recovery),
				new WorldAction(Traits01Actions.ProtectOffspring));
				Skill0401_3.action_special_effect = combinedAction0401_3;
				

				Skill0401_3.action_attack_target = new AttackAction(Traits01Actions.UndeadServantAtk);
				Skill0401_3.action_get_hit = new GetHitAction(Traits01Actions.UndeadServantAtk);

				AssetManager.traits.add(Skill0401_3);
				addToLocalizedLibrary("ch",Skill0401_3.id, "不死王的契約", "");//中文名稱與介紹
				addToLocalizedLibrary("en",Skill0401_3.id, "Undead King's Contract", "");//英文名稱與介紹
				Skill0401_3.unlock(true);

				ActorTrait Skill0401_4 = new ActorTrait();
				Skill0401_4.id = "undead_servant3";
				Skill0401_4.path_icon = "ui/icons/Skill/Skill0401_4";
				Skill0401_4.group_id = "auxiliary_traits2";
				Skill0401_4.rarity = Rarity.R2_Epic;
				Skill0401_4.rate_birth = 0;
				Skill0401_4.rate_inherit = 0;
				Skill0401_4.can_be_given = false;
				Skill0401_4.base_stats = new BaseStats();
				WorldAction combinedAction0401_4 = (WorldAction)Delegate.Combine(
				new WorldAction(Traits01Actions.UndeadServant_N),
				new WorldAction(Traits01Actions.Recovery));
				Skill0401_4.action_special_effect = combinedAction0401_4;
				

				Skill0401_4.action_attack_target = new AttackAction(Traits01Actions.UndeadServant_NAtk);
				Skill0401_4.action_get_hit = new GetHitAction(Traits01Actions.UndeadServant_NAtk);
				
				AssetManager.traits.add(Skill0401_4);
				addToLocalizedLibrary("ch",Skill0401_4.id, "不死帝的契約", "");//中文名稱與介紹
				addToLocalizedLibrary("en",Skill0401_4.id, "Undead Emperor's Contract", "");//英文名稱與介紹
				Skill0401_4.unlock(true);

				//不死帝王 undead_emperor
				ActorTrait Skill0401_5 = new ActorTrait();
				Skill0401_5.id = "extraordinary_authority";
				Skill0401_5.path_icon = "ui/icons/Effects1/undead/UndeadEmperor";
				Skill0401_5.group_id = "auxiliary_traits2";
				Skill0401_5.rarity = Rarity.R3_Legendary;
				Skill0401_5.rate_birth = 0;
				Skill0401_5.rate_inherit = 0;

				GetHitAction combinedAction_0401_5B = (GetHitAction)Delegate.Combine(
				new GetHitAction(Items01Actions.Anti_Soul_Defense),
				new GetHitAction(Items01Actions.Defense));
				Skill0401_5.action_get_hit = combinedAction_0401_5B;
				
				WorldAction combinedAction0401_5A = (WorldAction)Delegate.Combine(
				new WorldAction(Traits01Actions.UndeadEmperor2));
				Skill0401_5.action_special_effect = combinedAction0401_5A;

				AssetManager.traits.add(Skill0401_5);
				addToLocalizedLibrary("ch",Skill0401_5.id, "非常大權", "統帥此世不死者族群的證明");
				addToLocalizedLibrary("en",Skill0401_5.id, "Extraordinary Authority", "Proof of commanding the undead race of this world");
				Skill0401_5.unlock(true);


			//吞噬法 Devour ★ 2 魔王候補特質
			ActorTrait Skill0402 = new ActorTrait();
			Skill0402.id = "evillaw_devour";
			Skill0402.path_icon = "ui/icons/Skill/Skill0402";
			Skill0402.group_id = "evil_law";
			Skill0402.rarity = Rarity.R3_Legendary;
			Skill0402.rate_birth = 0;
			Skill0402.rate_inherit = 1;
			Skill0402.base_stats = new BaseStats();	
			Skill0402.base_stats.set("multiplier_health", 25.0f);						//生命 %
			Skill0402.action_special_effect = (WorldAction)Delegate.Combine(Skill0402.action_special_effect,
			new WorldAction(Traits01Actions.Devour_Effect2),
			new WorldAction(Traits01Actions.applyEnvyStatus),
			new WorldAction(Traits01Actions.RapidHappiness2),
			new WorldAction(Traits01Actions.EvilPoniardGet),
			new WorldAction(Traits01Actions.addFavorite1), 
			new WorldAction(Traits01Actions.TraittAddRemove0));

			Skill0402.action_attack_target = new AttackAction(Traits01Actions.Devour_Effect1);

			GetHitAction combinedAction_0402B = (GetHitAction)Delegate.Combine(
			new GetHitAction(Traits01Actions.Devour_Effect1),
			new GetHitAction(Items01Actions.Shock_protectionX2),
			new GetHitAction(Items01Actions.Compare_Defense),
			new GetHitAction(Items01Actions.Defense),
			new GetHitAction(Traits01Actions.RapidHappiness));
			Skill0402.action_get_hit = combinedAction_0402B;

			AssetManager.traits.add(Skill0402);
			addToLocalizedLibrary("ch",Skill0402.id, "邪咒法:吞噬法", "吞噬狀態的邪咒");//中文名稱與介紹
			addToLocalizedLibrary("en",Skill0402.id, "Evil Law:Devour", "The evil Law that devours the 『Status』.");//英文名稱與介紹
			Skill0402.unlock(true);

			//御時法 Time Control ★ 2
			ActorTrait Skill0403 = new ActorTrait();
			Skill0403.id = "evillaw_tc";
			Skill0403.path_icon = "ui/icons/Skill/Skill0403";
			Skill0403.group_id = "evil_law";
			Skill0403.rarity = Rarity.R3_Legendary;
			Skill0403.rate_birth = 0;
			Skill0403.rate_inherit = 1;
			Skill0403.base_stats = new BaseStats();	
			Skill0403.base_stats.set("multiplier_health", 2.50f);						//生命 %

			WorldAction combinedAction0403 = (WorldAction)Delegate.Combine(
			new WorldAction(Traits01Actions.Extend_Effect1), 
			new WorldAction(Traits01Actions.Extend_Effect2), 
			new WorldAction(Traits01Actions.Extend_Effect3), 
			new WorldAction(Traits01Actions.Extend_Effect4), 
			new WorldAction(Traits01Actions.ClearTime_Effect1), 
			new WorldAction(Traits01Actions.ClearTime_Effect2), 
			//new WorldAction(Traits01Actions.addFavorite1), 
			new WorldAction(Traits01Actions.TraittAddRemove0));
			Skill0403.action_special_effect = combinedAction0403;

			GetHitAction combinedAction_0403B = (GetHitAction)Delegate.Combine(
			new GetHitAction(Traits01Actions.Extend_Effect_Atk01),
			new GetHitAction(Traits01Actions.Extend_Effect_Atk03),
			new GetHitAction(Traits01Actions.Extend_Effect_Atk04),
			new GetHitAction(Traits01Actions.Extend_Effect_Atk04));
			Skill0403.action_get_hit = combinedAction_0403B;

			AttackAction combinedAction_0403A = (AttackAction)Delegate.Combine(
			new AttackAction(Traits01Actions.Extend_Effect_Atk01),
			new AttackAction(Traits01Actions.Extend_Effect_Atk02),
			new AttackAction(Traits01Actions.Extend_Effect_Atk03),
			new AttackAction(Traits01Actions.Extend_Effect_Atk04));
			Skill0403.action_attack_target = combinedAction_0403A;

			AssetManager.traits.add(Skill0403);
			addToLocalizedLibrary("ch",Skill0403.id, "邪咒法:御時法", "干擾時間的邪咒");//中文名稱與介紹
			addToLocalizedLibrary("en",Skill0403.id, "Evil Law:Time Control", "The evil law that interferes with time.");//英文名稱與介紹
			Skill0403.unlock(true);

			//餓食法 Starvation ★ 2 魔王候補特質
			ActorTrait Skill0404 = new ActorTrait();
			Skill0404.id = "evillaw_starvation";
			Skill0404.path_icon = "ui/icons/Skill/Skill0404";
			Skill0404.group_id = "evil_law";
			Skill0404.rarity = Rarity.R3_Legendary;
			Skill0404.rate_birth = 0;
			Skill0404.rate_inherit = 1;
			Skill0404.base_stats = new BaseStats();	
			Skill0404.base_stats.set("multiplier_health", 25.00f);
			Skill0404.base_stats.set("max_nutrition", +1000f);							//最大營養
			WorldAction combinedAction_ase0404 = (WorldAction)Delegate.Combine(//被動
			new WorldAction(Traits01Actions.DrainEnemyHunger), 			//飢餓波動
			new WorldAction(Traits01Actions.ManageGluttonyByNutrition), //魔王狀態
			new WorldAction(Traits01Actions.EvilSpearGet),				//武器獲得
			new WorldAction(Traits01Actions.removeDemonKingAwakening),	//武器覺醒移除
			new WorldAction(Traits01Actions.addFavorite1),				//自動加入最愛
			new WorldAction(Traits01Actions.TraittAddRemove0));			//條件移除
			Skill0404.action_special_effect = combinedAction_ase0404;
			
			Skill0404.action_attack_target = new AttackAction(Traits01Actions.Devour_HungerHealth);//恢復飢餓與生命
			
			GetHitAction combinedAction_ogh0404 = (GetHitAction)Delegate.Combine(
			new GetHitAction(Traits01Actions.Devour_HungerHealth),		//恢復飢餓與生命
			new GetHitAction(Traits01Actions.RapidHunger),				//加速飢餓
			new GetHitAction(Items01Actions.Anti_Hungry_Defense),		//減傷
			new GetHitAction(Items01Actions.Defense),					//減傷
			new GetHitAction(Items01Actions.EvilSpearCounterattack));	//反擊組合
			Skill0404.action_get_hit = combinedAction_ogh0404;
			
			AssetManager.traits.add(Skill0404);
			addToLocalizedLibrary("ch",Skill0404.id, "邪咒法:餓食法", "製造飢荒的邪咒");//中文名稱與介紹
			addToLocalizedLibrary("en",Skill0404.id, "Evil Law:Starvation", "The evil law that creates famine.");//英文名稱與介紹
			Skill0404.unlock(true);

			//病災法 Disease ★
			ActorTrait Skill0405 = new ActorTrait();
			Skill0405.id = "evillaw_disease";
			Skill0405.path_icon = "ui/icons/Skill/Skill0405";
			Skill0405.group_id = "evil_law";
			Skill0405.rarity = Rarity.R3_Legendary;
			Skill0405.rate_birth = 0;
			Skill0405.rate_inherit = 1;
			Skill0403.base_stats = new BaseStats();	
			Skill0403.base_stats.set("multiplier_health", 3.00f);
			WorldAction combinedAction_0405 = (WorldAction)Delegate.Combine(
			new WorldAction(Traits01Actions.Skill0405addStatus), 
			new WorldAction(Traits01Actions.Skill0405addTrait), 
			//new WorldAction(Traits01Actions.addFavorite1), 
			new WorldAction(Traits01Actions.TraittAddRemove0));
			Skill0405.action_special_effect = combinedAction_0405;
			AssetManager.traits.add(Skill0405);
			addToLocalizedLibrary("ch",Skill0405.id, "邪咒法:病災法", "散播疾病與異常的邪咒");//中文名稱與介紹
			addToLocalizedLibrary("en",Skill0405.id, "Evil Law:Disease", "The evil law that creates famine.");//英文名稱與介紹
			Skill0405.unlock(true);

			//金錢法 Money Law ★ 1 魔王候補特質
			ActorTrait Skill0406 = new ActorTrait();
			Skill0406.id = "evillaw_moneylaw";
			Skill0406.path_icon = "ui/icons/Skill/Skill0406";
			Skill0406.group_id = "evil_law";
			Skill0406.rarity = Rarity.R3_Legendary;
			Skill0406.rate_birth = 0;
			Skill0406.rate_inherit = 1;
			Skill0403.base_stats = new BaseStats();	
			Skill0403.base_stats.set("multiplier_health", 30.00f);
			WorldAction combinedAction_0406 = (WorldAction)Delegate.Combine(
			new WorldAction(Traits01Actions.CoinsIncrease),					//金錢增加
			new WorldAction(Traits01Actions.CoinsReduction),				//金錢掠奪
			new WorldAction(Traits01Actions.LootReduction),					//戰利品掠奪
			new WorldAction(Traits01Actions.MoneyTierEffects),				//金錢層級
			new WorldAction(Traits01Actions.EvilLawMoney),					//魔王狀態
			new WorldAction(Traits01Actions.EvilGunGet),					//武器獲得
			new WorldAction(Traits01Actions.removeDemonKingAwakening),		//武器覺醒移除
			new WorldAction(Traits01Actions.addFavorite1), 
			new WorldAction(Traits01Actions.TraittAddRemove0));
			Skill0406.action_special_effect = combinedAction_0406;

			GetHitAction combinedAction_ogh0406 = (GetHitAction)Delegate.Combine(
			new GetHitAction(Items01Actions.Anti_Poverty_Defense),		//減傷
			new GetHitAction(Items01Actions.Defense),					//減傷
			new GetHitAction(Items01Actions.EvilGunShooting00_2),		//反擊
			new GetHitAction(Items01Actions.EvilGunShooting00_1));		//反擊
			Skill0406.action_get_hit = combinedAction_ogh0406;
			
			Skill0406.action_death = (WorldAction)Delegate.Combine(Skill0406.action_death, new WorldAction(Traits01Actions.Bribery));

			AssetManager.traits.add(Skill0406);
			addToLocalizedLibrary("ch",Skill0406.id, "邪咒法:金錢法", "透過所持財富獲得力量的邪咒");//中文名稱與介紹
			addToLocalizedLibrary("en",Skill0406.id, "Evil Law:Money Law", "The evil law of accumulating power through the accumulation of money.");//英文名稱與介紹
			Skill0406.unlock(true);

			//吸收法 Essence Absorption ★ 2
			ActorTrait Skill0407 = new ActorTrait();
			Skill0407.id = "evillaw_ea";
			Skill0407.path_icon = "ui/icons/Skill/Skill0407";
			Skill0407.group_id = "evil_law";
			Skill0407.rarity = Rarity.R3_Legendary;
			Skill0407.rate_birth = 0;
			Skill0407.rate_inherit = 1;
			Skill0407.base_stats = new BaseStats();									//添加數值
			Skill0407.base_stats.set("multiplier_health", 6.00f);						//生命 %
			Skill0407.base_stats.set("multiplier_mana", 6.00f);						//魔力 %
			Skill0407.base_stats.set("multiplier_stamina", 6.00f);						//耐力 %
			WorldAction combinedAction_0407 = (WorldAction)Delegate.Combine(
			new WorldAction(Traits01Actions.DrainEnemyHMS),
			//new WorldAction(Traits01Actions.addFavorite1),
			new WorldAction(Traits01Actions.TraittAddRemove0));
			Skill0407.action_special_effect = combinedAction_0407;
			//Skill0407.action_special_effect = new WorldAction(Traits01Actions.DrainEnemyHMS);
			AssetManager.traits.add(Skill0407);
			addToLocalizedLibrary("ch",Skill0407.id, "邪咒法:吸收法", "吸取生命，吸收魔力與耐力的邪咒");//中文名稱與介紹
			addToLocalizedLibrary("en",Skill0407.id, "Evil Law:Essence Absorption", "An evil law that drains health, mana, and stamina.");//英文名稱與介紹
			Skill0407.unlock(true);

			//睡夢法 Sleeping Law ★ 2 魔王候補特質
			ActorTrait Skill0408 = new ActorTrait();
			Skill0408.id = "evillaw_sleeping";
			Skill0408.path_icon = "ui/icons/Skill/Skill0408";
			Skill0408.group_id = "evil_law";
			Skill0408.rarity = Rarity.R3_Legendary;
			Skill0408.rate_birth = 0;
			Skill0408.rate_inherit = 1;
			Skill0408.base_stats = new BaseStats();	
			Skill0408.base_stats.set("multiplier_health", 25.00f);
			Skill0408.base_stats.addTag("immunity_cold");
			WorldAction combinedAction_0408 = (WorldAction)Delegate.Combine(
			new WorldAction(Traits01Actions.SleepingLaw),		//眠之波動 魔王狀態
			new WorldAction(Traits01Actions.Severe_Winter30),	//嚴冬
			new WorldAction(Traits01Actions.Severe_Winter20),
			new WorldAction(Traits01Actions.Severe_Winter10),
			new WorldAction(Traits01Actions.LoverNull),
			new WorldAction(Traits01Actions.EvilStickGet),
			new WorldAction(Traits01Actions.removeDemonKingAwakening),
			new WorldAction(Traits01Actions.addFavorite1), 
			new WorldAction(Traits01Actions.TraittAddRemove0));
			Skill0408.action_special_effect = combinedAction_0408;

			Skill0408.action_attack_target = new AttackAction(Traits01Actions.Severe_WinterATK);

			GetHitAction combinedAction_0408GHA = (GetHitAction)Delegate.Combine(//被傷發動
			new GetHitAction(Items01Actions.EvilStickThrowing00),
			new GetHitAction(Traits01Actions.Severe_WinterATK),
			new GetHitAction(Items01Actions.Defense),
			new GetHitAction(Items01Actions.Anti_NoSleeping_Defense));
			Skill0408.action_get_hit = combinedAction_0408GHA;
			
			AssetManager.traits.add(Skill0408);
			addToLocalizedLibrary("ch",Skill0408.id, "邪咒法:睡夢法", "令人陷入漫長睡眠的邪咒");//中文名稱與介紹
			addToLocalizedLibrary("en",Skill0408.id, "Evil Law:Sleeping Law", "The evil law that causes a long sleep.");//英文名稱與介紹
			Skill0408.unlock(true);
			// 使徒 Apostle
				ActorTrait Skill0408_2 = new ActorTrait();
				Skill0408_2.id = "apostle";
				Skill0408_2.path_icon = "ui/icons/Skill/Skill0408_2";
				Skill0408_2.group_id = "auxiliary_traits2";
				Skill0408_2.rarity = Rarity.R2_Epic;
				Skill0408_2.can_be_given = false; //false true
				Skill0408_2.rate_birth = 0;
				Skill0408_2.rate_inherit = 0;
				Skill0408_2.base_stats = new BaseStats();
				Skill0408_2.base_stats.set("multiplier_lifespan", 1.00f);
				Skill0408_2.base_stats.set("multiplier_health", 2.00f);
				Skill0408_2.base_stats.set("multiplier_damage", 2.50f);
				Skill0408_2.base_stats.set("armor", 25f);
				Skill0408_2.base_stats.set("multiplier_speed", 2.50f);
				Skill0408_2.base_stats.set("multiplier_offspring", -9999.99f);
				Skill0408_2.base_stats.set("birth_rate", -9999.99f);
				Skill0408_2.base_stats.addTag("immunity_cold");
				
				WorldAction combinedAction_0408_2 = (WorldAction)Delegate.Combine(
				new WorldAction(Traits01Actions.ApostleUnit),		//跟隨+補給
				new WorldAction(Traits01Actions.ApostleUnit2),		//跟隨+補給
				new WorldAction(Traits01Actions.removefell_in_love),//移除狀態
			//	new WorldAction(Traits01Actions.ApostleUnitEND),	//移除
				new WorldAction(Traits01Actions.Severe_Winter20),		//嚴冬 20
				new WorldAction(Traits01Actions.Severe_Winter10),		//嚴冬 30
				new WorldAction(Traits01Actions.ApostleUnitATK),		
				new WorldAction(Traits01Actions.Health_recovery),
				new WorldAction(Traits01Actions.Mana_recovery),
				new WorldAction(Traits01Actions.Stamina_recovery),
				new WorldAction(Traits01Actions.nutrition1),
				new WorldAction(Traits01Actions.removeTraitXXX));
				Skill0408_2.action_special_effect = combinedAction_0408_2;

				GetHitAction combinedAction_0408_2GHA = (GetHitAction)Delegate.Combine(//被傷發動
				//new GetHitAction(Traits01Actions.ApostleUnitCTK),
				new GetHitAction(Traits01Actions.Severe_WinterATK));
				Skill0408_2.action_get_hit = combinedAction_0408_2GHA;
				
				Skill0408_2.action_attack_target = new AttackAction(Traits01Actions.Severe_WinterATK);

				AssetManager.traits.add(Skill0408_2);
				addToLocalizedLibrary("ch",Skill0408_2.id, "怠惰的使徒", "為世界帶來嚴冬,為他們的王帶來安眠");//中文名稱與介紹
				addToLocalizedLibrary("en",Skill0408_2.id, "Apostle of Sloth", "Bringing winter to the world and sleep to their king.");//英文名稱與介紹
				Skill0408_2.unlock(true);

			//絕育法 Sterilization Law ★ 2
			ActorTrait Skill0409 = new ActorTrait();
			Skill0409.id = "evillaw_sterilization";
			Skill0409.path_icon = "ui/icons/Skill/Skill0409";
			Skill0409.group_id = "evil_law";
			Skill0409.rarity = Rarity.R3_Legendary;
			Skill0409.rate_birth = 0;
			Skill0409.rate_inherit = 1;
			Skill0409.base_stats = new BaseStats();	
			Skill0409.base_stats.set("multiplier_health", 4.00f);
			Skill0409.base_stats.set("multiplier_speed", 3.00f);						//移動速度 %
			WorldAction combinedAction_0409 = (WorldAction)Delegate.Combine(
			new WorldAction(Traits01Actions.Sterilization),
			new WorldAction(Traits01Actions.Delayed_hatching), 
			//new WorldAction(Traits01Actions.addFavorite1), 
			new WorldAction(Traits01Actions.TraittAddRemove0));
			Skill0409.action_special_effect = combinedAction_0409;
			AssetManager.traits.add(Skill0409);
			addToLocalizedLibrary("ch",Skill0409.id, "邪咒法:絕育法", "令生命無法降生的邪咒");//中文名稱與介紹
			addToLocalizedLibrary("en",Skill0409.id, "Evil Law:Sterilization", "The evil law that prevents the birth of life.");//英文名稱與介紹
			Skill0409.unlock(true);

			//忿怒法 Wrath ★ 2 魔王候補特質
			ActorTrait Skill0410 = new ActorTrait();
			Skill0410.id = "evillaw_tantrum";
			Skill0410.path_icon = "ui/icons/Skill/Skill0410";
			Skill0410.group_id = "evil_law";
			Skill0410.rarity = Rarity.R3_Legendary;
			Skill0410.rate_birth = 0;
			Skill0410.rate_inherit = 1;
			Skill0410.base_stats = new BaseStats();
			Skill0410.base_stats.addTag("immunity_fire");
			Skill0410.base_stats.set("multiplier_health", 25.000f);

			WorldAction combinedAction_0410 = (WorldAction)Delegate.Combine(
			new WorldAction(Traits01Actions.addFavorite1),
			new WorldAction(Traits01Actions.TantrumLaw),			//憤怒波動
			new WorldAction(Traits01Actions.ManageWrathEffect1),	//寧靜與魔王狀態
			new WorldAction(Traits01Actions.ManageWrathEffect2),	//寧靜與魔王狀態
			new WorldAction(Traits01Actions.AngryAura),				//憤怒靈氣
			new WorldAction(Traits01Actions.EvilGlovesGet), 		//武器獲得
			new WorldAction(Traits01Actions.removeDemonKingAwakening));
			Skill0410.action_special_effect = combinedAction_0410;

			GetHitAction combinedAction_0410GHA = (GetHitAction)Delegate.Combine(//被傷發動
			new GetHitAction(Traits01Actions.ManageWrathEffect3),
			new GetHitAction(Traits01Actions.AngryAura_ATK),
			new GetHitAction(Items01Actions.Anti_Angry_Defense),
			new GetHitAction(Items01Actions.Defense),
			new GetHitAction(Items01Actions.EvilGlovesStrike_Counterattack01),
			new GetHitAction(Items01Actions.EvilGlovesStrike_Counterattack02));
			Skill0410.action_get_hit = combinedAction_0410GHA;
			
			AttackAction combinedAction_0410A = (AttackAction)Delegate.Combine(//攻擊發動
			new AttackAction(Traits01Actions.ManageWrathEffect3),
			new AttackAction(Traits01Actions.AngryAura_ATK));
			Skill0410.action_attack_target = combinedAction_0410A;

			AssetManager.traits.add(Skill0410);
			addToLocalizedLibrary("ch",Skill0410.id, "邪咒法:忿怒法", "令人們陷入忿怒與鬥爭的邪咒");//中文名稱與介紹
			addToLocalizedLibrary("en",Skill0410.id, "Evil Law:Wrath", "The evil law that plunges people into anger and strife.");//英文名稱與介紹
			Skill0410.unlock(true);

			//誘惑法 Seduction ★ 2 魔王候補特質
			ActorTrait Skill0411 = new ActorTrait();
			Skill0411.id = "evillaw_seduction";
			Skill0411.path_icon = "ui/icons/Skill/Skill0411";
			Skill0411.group_id = "evil_law";
			Skill0411.rarity = Rarity.R3_Legendary;
			Skill0411.rate_birth = 0;
			Skill0411.rate_inherit = 1;
			Skill0411.base_stats = new BaseStats();	
			Skill0411.base_stats.set("multiplier_health", 30.00f);
			Skill0411.base_stats.set("multiplier_lifespan", 3.00f);
			Skill0411.base_stats.set("maturation", -999999f);
			Skill0411.base_stats.set("birth_rate", 999999f);
			WorldAction combinedAction_0411 = (WorldAction)Delegate.Combine(
			new WorldAction(Traits01Actions.SeductionLaw), 			//轉化馴服 狀態轉化
			new WorldAction(Traits01Actions.SeductionLaw2), 		//轉化馴服 家庭成員
			new WorldAction(Traits01Actions.TransformGender), 		//性別轉換+魔王狀態
			new WorldAction(Traits01Actions.EvilBowGet), 			//武器獲得
			new WorldAction(Traits01Actions.Divorce), 			//死之代價
			new WorldAction(Traits01Actions.independence), 			//獨立
			new WorldAction(Traits01Actions.removeDemonKingAwakening), 	//武器獲得
			new WorldAction(Traits01Actions.addFavorite1),
			new WorldAction(Traits01Actions.TraittAddRemove0));
			Skill0411.action_special_effect = combinedAction_0411; //Shock_protectionX

			GetHitAction combinedAction_0411GHA = (GetHitAction)Delegate.Combine(//被傷發動
			new GetHitAction(Items01Actions.EvilBowShootingCounterattack),
			new GetHitAction(Items01Actions.Anti_Eex_Defense),
			new GetHitAction(Items01Actions.Defense),
			new GetHitAction(Items01Actions.Shock_protectionX));
			Skill0411.action_get_hit = combinedAction_0411GHA;
			
			AssetManager.traits.add(Skill0411);
			addToLocalizedLibrary("ch",Skill0411.id, "邪咒法:誘惑法", "令人們陷入愛與情慾中的邪咒");//中文名稱與介紹
			addToLocalizedLibrary("en",Skill0411.id, "Evil Law:Seduction", "The evil law that plunges people into anger and strife.");//英文名稱與介紹
			Skill0411.unlock(true);

			//奴隸 slave
				ActorTrait Skill0411_2 = new ActorTrait();
				Skill0411_2.id = "slave";
				Skill0411_2.path_icon = "ui/icons/Skill/Skill0411_2";
				Skill0411_2.group_id = "auxiliary_traits2";
				Skill0411_2.rarity = Rarity.R2_Epic;
				Skill0411_2.can_be_given = false; //false true
				Skill0411_2.rate_birth = 0;
				Skill0411_2.rate_inherit = 0;
				Skill0411_2.base_stats = new BaseStats();	
				Skill0411_2.base_stats.set("multiplier_health", 6.00f);
				Skill0411_2.base_stats.set("maturation", -999999f);
				Skill0411_2.base_stats.set("birth_rate", 999999f);
				Skill0411_2.base_stats.set("diplomacy", -999f);
				Skill0411_2.base_stats.set("warfare", -999f);
				Skill0411_2.base_stats.set("stewardship", -999f);
				Skill0411_2.base_stats.set("intelligence", -999f);
				WorldAction combinedAction_0411_2 = (WorldAction)Delegate.Combine(
				new WorldAction(Traits01Actions.SlaveUnit),//跟隨+補給
				new WorldAction(Traits01Actions.TransferUnitLeader),//跟隨+補給
				new WorldAction(Traits01Actions.SlaveUnitAI),//跟隨+補給
				new WorldAction(Traits01Actions.addfell_in_love),//持續添加狀態
				new WorldAction(Traits01Actions.Health_recovery),
				new WorldAction(Traits01Actions.Mana_recovery),
				new WorldAction(Traits01Actions.Stamina_recovery),
				new WorldAction(Traits01Actions.SlaveTraitRemove),
				new WorldAction(Traits01Actions.nutrition1),
				new WorldAction(Traits01Actions.Divorce),
				new WorldAction(Traits01Actions.removeTraitXXX));
				Skill0411_2.action_special_effect = combinedAction_0411_2;
				Skill0411_2.action_attack_target = new AttackAction(Traits01Actions.SlaveTraitAdd);
				Skill0411_2.action_get_hit = new GetHitAction(Traits01Actions.SlaveTraitAdd);
				AssetManager.traits.add(Skill0411_2);
				addToLocalizedLibrary("ch",Skill0411_2.id, "色慾的奴隸", "深陷色慾之中難以自拔");//中文名稱與介紹
				addToLocalizedLibrary("en",Skill0411_2.id, "Slave of lust", "He is deeply trapped in lust and cannot extricate himself.");//英文名稱與介紹
				Skill0411_2.unlock(true);

			//滅智法 Eliminate wisdom ★ 2 魔王候補特質
			ActorTrait Skill0499 = new ActorTrait();
			Skill0499.id = "evillaw_ew";
			Skill0499.path_icon = "ui/icons/Skill/Skill0499";
			Skill0499.group_id = "evil_law";
			Skill0499.rarity = Rarity.R3_Legendary;
			Skill0499.rate_birth = 0;
			Skill0499.rate_inherit = 1;
			Skill0499.base_stats = new BaseStats();
			Skill0499.base_stats.set("multiplier_health", 31.00f);
			Skill0499.base_stats.set("armor", 75f);										//防禦
			Skill0499.base_stats.set("multiplier_speed", 0.75f);						//移動速度 %
			Skill0499.base_stats.set("multiplier_attack_speed", 0.75f);					//攻擊速度 %
			Skill0499.base_stats.set("critical_chance", 0.75f);							//爆擊機率 %
			Skill0499.base_stats.set("critical_damage_multiplier", 0.75f);				//重擊 %
			WorldAction combinedAction_0499A = (WorldAction)Delegate.Combine(
			new WorldAction(Traits01Actions.addFavorite1), 
			new WorldAction(Traits01Actions.EvilSwordGet),
			new WorldAction(Traits01Actions.Ready),
			new WorldAction(Traits01Actions.removeDemonKingAwakening),
			new WorldAction(Traits01Actions.TraittAddRemove0));
			Skill0499.action_special_effect = combinedAction_0499A;
			
			Skill0499.action_attack_target = new AttackAction(Traits01Actions.SmartKillerEffect);

			GetHitAction combinedAction_0499B = (GetHitAction)Delegate.Combine(
			new GetHitAction(Traits01Actions.Ready2),
			new GetHitAction(Items01Actions.EvilSwordSlash03),
			new GetHitAction(Items01Actions.Anti_OtherRaces_Defense),
			new GetHitAction(Items01Actions.Defense),
			new GetHitAction(Items01Actions.EvilSwordSlashX1));
			Skill0499.action_get_hit = combinedAction_0499B;

			AssetManager.traits.add(Skill0499);
			addToLocalizedLibrary("ch",Skill0499.id, "邪咒法:滅智法", "消去智慧,將人變成野獸的邪咒");//中文名稱與介紹
			addToLocalizedLibrary("en",Skill0499.id, "Evil Law:Eliminate wisdom", "The evil law that eliminates wisdom and turns people into beasts.");//英文名稱與介紹
			Skill0499.unlock(true);
			#endregion
			#region 群系變遷	Biome Magic
			ActorTrait AlteredSurface01 = new ActorTrait();
			AlteredSurface01.id = "altered_surface01";
			AlteredSurface01.path_icon = "ui/icons/Skill/AlteredSurface/AlteredSurface01";
			AlteredSurface01.group_id = "biome_magic";
			AlteredSurface01.rarity = Rarity.R2_Epic;
			AlteredSurface01.rate_birth = 0;
			AlteredSurface01.rate_inherit = 100;
			AlteredSurface01.action_special_effect = new WorldAction(Traits01Actions.AlteredSurface_01);
			AssetManager.traits.add(AlteredSurface01);
			addToLocalizedLibrary("ch",AlteredSurface01.id, "群系：草地群系", "創造出青青草地！綠色！");
			addToLocalizedLibrary("en",AlteredSurface01.id, "Biome: Grass", "Create green grass! Green!");
			AlteredSurface01.unlock(true);

			ActorTrait AlteredSurface02 = new ActorTrait();
			AlteredSurface02.id = "altered_surface02";
			AlteredSurface02.path_icon = "ui/icons/Skill/AlteredSurface/AlteredSurface02";
			AlteredSurface02.group_id = "biome_magic";
			AlteredSurface02.rarity = Rarity.R2_Epic;
			AlteredSurface02.rate_birth = 0;
			AlteredSurface02.rate_inherit = 100;
			AlteredSurface02.action_special_effect = new WorldAction(Traits01Actions.AlteredSurface_02);
			AssetManager.traits.add(AlteredSurface02);
			addToLocalizedLibrary("ch",AlteredSurface02.id, "群系：魔法群系", "創造出魔法之地！發光！");
			addToLocalizedLibrary("en",AlteredSurface02.id, "Biome:Enchanted", "Create a magical place! Glow!");
			AlteredSurface02.unlock(true);

			ActorTrait AlteredSurface03 = new ActorTrait();
			AlteredSurface03.id = "altered_surface03";
			AlteredSurface03.path_icon = "ui/icons/Skill/AlteredSurface/AlteredSurface03";
			AlteredSurface03.group_id = "biome_magic";
			AlteredSurface03.rarity = Rarity.R2_Epic;
			AlteredSurface03.rate_birth = 0;
			AlteredSurface03.rate_inherit = 100;
			AlteredSurface03.action_special_effect = new WorldAction(Traits01Actions.AlteredSurface_03);
			AssetManager.traits.add(AlteredSurface03);
			addToLocalizedLibrary("ch",AlteredSurface03.id, "群系：稀樹草原", "創造熱帶草原！炎熱！");
			addToLocalizedLibrary("en",AlteredSurface03.id, "Biome:Savanna", "Create a savanna! Hot!");
			AlteredSurface03.unlock(true);

			ActorTrait AlteredSurface04 = new ActorTrait();
			AlteredSurface04.id = "altered_surface04";
			AlteredSurface04.path_icon = "ui/icons/Skill/AlteredSurface/AlteredSurface04";
			AlteredSurface04.group_id = "biome_magic";
			AlteredSurface04.rarity = Rarity.R2_Epic;
			AlteredSurface04.rate_birth = 0;
			AlteredSurface04.rate_inherit = 100;
			AlteredSurface04.action_special_effect = new WorldAction(Traits01Actions.AlteredSurface_04);
			AssetManager.traits.add(AlteredSurface04);
			addToLocalizedLibrary("ch",AlteredSurface04.id, "群系：腐朽群系", "創造腐朽之地！幽靈！");
			addToLocalizedLibrary("en",AlteredSurface04.id, "Biome:Corrupted", "Create a corrupted land! Ghost!");
			AlteredSurface04.unlock(true);

			ActorTrait AlteredSurface05 = new ActorTrait();
			AlteredSurface05.id = "altered_surface05";
			AlteredSurface05.path_icon = "ui/icons/Skill/AlteredSurface/AlteredSurface05";
			AlteredSurface05.group_id = "biome_magic";
			AlteredSurface05.rarity = Rarity.R2_Epic;
			AlteredSurface05.rate_birth = 0;
			AlteredSurface05.rate_inherit = 100;
			AlteredSurface05.action_special_effect = new WorldAction(Traits01Actions.AlteredSurface_05);
			AssetManager.traits.add(AlteredSurface05);
			addToLocalizedLibrary("ch",AlteredSurface05.id, "群系：蘑菇群系", "創造出蘑菇之地！孢子！");
			addToLocalizedLibrary("en",AlteredSurface05.id, "Biome:Mushroom", "Create the mushroom biome! It has mushrooms!");
			AlteredSurface05.unlock(true);

			ActorTrait AlteredSurface06 = new ActorTrait();
			AlteredSurface06.id = "altered_surface06";
			AlteredSurface06.path_icon = "ui/icons/Skill/AlteredSurface/AlteredSurface06";
			AlteredSurface06.group_id = "biome_magic";
			AlteredSurface06.rarity = Rarity.R2_Epic;
			AlteredSurface06.rate_birth = 0;
			AlteredSurface06.rate_inherit = 100;
			AlteredSurface06.action_special_effect = new WorldAction(Traits01Actions.AlteredSurface_06);
			AssetManager.traits.add(AlteredSurface06);
			addToLocalizedLibrary("ch",AlteredSurface06.id, "群系：叢林群系", "創造出叢林生態！多彩！");
			addToLocalizedLibrary("en",AlteredSurface06.id, "Biome:Jungle", "Create the jungle biome! It's colorful!");
			AlteredSurface06.unlock(true);

			ActorTrait AlteredSurface07 = new ActorTrait();
			AlteredSurface07.id = "altered_surface07";
			AlteredSurface07.path_icon = "ui/icons/Skill/AlteredSurface/AlteredSurface07";
			AlteredSurface07.group_id = "biome_magic";
			AlteredSurface07.rarity = Rarity.R2_Epic;
			AlteredSurface07.rate_birth = 0;
			AlteredSurface07.rate_inherit = 100;
			AlteredSurface07.action_special_effect = new WorldAction(Traits01Actions.AlteredSurface_07);
			AssetManager.traits.add(AlteredSurface07);
			addToLocalizedLibrary("ch",AlteredSurface07.id, "群系：魔法沙漠", "魔法種子，不斷改造土地為光滑的金沙，並將整片大地填滿，最終變成魔法沙漠");
			addToLocalizedLibrary("en",AlteredSurface07.id, "Biome:Desert", "Magic seeds, that keep turning into smooth golden sand, and eventually");
			AlteredSurface07.unlock(true);

			ActorTrait AlteredSurface08 = new ActorTrait();
			AlteredSurface08.id = "altered_surface08";
			AlteredSurface08.path_icon = "ui/icons/Skill/AlteredSurface/AlteredSurface08";
			AlteredSurface08.group_id = "biome_magic";
			AlteredSurface08.rarity = Rarity.R2_Epic;
			AlteredSurface08.rate_birth = 0;
			AlteredSurface08.rate_inherit = 100;
			AlteredSurface08.action_special_effect = new WorldAction(Traits01Actions.AlteredSurface_08);
			AssetManager.traits.add(AlteredSurface08);
			addToLocalizedLibrary("ch",AlteredSurface08.id, "群系：檸檬群系", "創造一個檸檬地吧…相比糖果地更容易接受!");
			addToLocalizedLibrary("en",AlteredSurface08.id, "Biome:Lemon", "Create the lemon biome! It's a pretty acceptable biome!");
			AlteredSurface08.unlock(true);

			ActorTrait AlteredSurface09 = new ActorTrait();
			AlteredSurface09.id = "altered_surface09";
			AlteredSurface09.path_icon = "ui/icons/Skill/AlteredSurface/AlteredSurface09";
			AlteredSurface09.group_id = "biome_magic";
			AlteredSurface09.rarity = Rarity.R2_Epic;
			AlteredSurface09.rate_birth = 0;
			AlteredSurface09.rate_inherit = 100;
			AlteredSurface09.action_special_effect = new WorldAction(Traits01Actions.AlteredSurface_09);
			AssetManager.traits.add(AlteredSurface09);
			addToLocalizedLibrary("ch",AlteredSurface09.id, "群系：凍土群系", "這裡草地冷如冰塊，陽光也無計可施");
			addToLocalizedLibrary("en",AlteredSurface09.id, "Biome:Permafrost", "Grass as cold as ice. Even in the sun");
			AlteredSurface09.unlock(true);

			ActorTrait AlteredSurface10 = new ActorTrait();
			AlteredSurface10.id = "altered_surface10";
			AlteredSurface10.path_icon = "ui/icons/Skill/AlteredSurface/AlteredSurface10";
			AlteredSurface10.group_id = "biome_magic";
			AlteredSurface10.rarity = Rarity.R2_Epic;
			AlteredSurface10.rate_birth = 0;
			AlteredSurface10.rate_inherit = 100;
			AlteredSurface10.action_special_effect = new WorldAction(Traits01Actions.AlteredSurface_10);
			AssetManager.traits.add(AlteredSurface10);
			addToLocalizedLibrary("ch",AlteredSurface10.id, "群系：糖果群系", "創造一個糖果田吧…這應該是最邪惡的一種");
			addToLocalizedLibrary("en",AlteredSurface10.id, "Biome:Candy", "GrassCreate the candy biome! The most evil one!");
			AlteredSurface10.unlock(true);

			ActorTrait AlteredSurface11 = new ActorTrait();
			AlteredSurface11.id = "altered_surface11";
			AlteredSurface11.path_icon = "ui/icons/Skill/AlteredSurface/AlteredSurface11";
			AlteredSurface11.group_id = "biome_magic";
			AlteredSurface11.rarity = Rarity.R2_Epic;
			AlteredSurface11.rate_birth = 0;
			AlteredSurface11.rate_inherit = 100;
			AlteredSurface11.action_special_effect = new WorldAction(Traits01Actions.AlteredSurface_11);
			AssetManager.traits.add(AlteredSurface11);
			addToLocalizedLibrary("ch",AlteredSurface11.id, "群系：水晶群系", "創造出水晶土地！礦物！");
			addToLocalizedLibrary("en",AlteredSurface11.id, "Biome:Crystal", "Create the crystal biome! It's full of minerals!");
			AlteredSurface11.unlock(true);

			ActorTrait AlteredSurface12 = new ActorTrait();
			AlteredSurface12.id = "altered_surface12";
			AlteredSurface12.path_icon = "ui/icons/Skill/AlteredSurface/AlteredSurface12";
			AlteredSurface12.group_id = "biome_magic";
			AlteredSurface12.rarity = Rarity.R2_Epic;
			AlteredSurface12.rate_birth = 0;
			AlteredSurface12.rate_inherit = 100;
			AlteredSurface12.action_special_effect = new WorldAction(Traits01Actions.AlteredSurface_12);
			AssetManager.traits.add(AlteredSurface12);
			addToLocalizedLibrary("ch",AlteredSurface12.id, "群系：沼澤群系", "創造出沼澤濕地！黏稠！");
			addToLocalizedLibrary("en",AlteredSurface12.id, "Biome:Swamp", "Create the swamp biome! It's sticky!");
			AlteredSurface12.unlock(true);

			ActorTrait AlteredSurface13 = new ActorTrait();
			AlteredSurface13.id = "altered_surface13";
			AlteredSurface13.path_icon = "ui/icons/Skill/AlteredSurface/AlteredSurface13";
			AlteredSurface13.group_id = "biome_magic";
			AlteredSurface13.rarity = Rarity.R2_Epic;
			AlteredSurface13.rate_birth = 0;
			AlteredSurface13.rate_inherit = 100;
			AlteredSurface13.action_special_effect = new WorldAction(Traits01Actions.AlteredSurface_13);
			AssetManager.traits.add(AlteredSurface13);
			addToLocalizedLibrary("ch",AlteredSurface13.id, "群系：地獄群系", "創造出焚燒之地！烈焰！");
			addToLocalizedLibrary("en",AlteredSurface13.id, "Biome:Infernal", "Create the infernal biome! It burns!");
			AlteredSurface13.unlock(true);

			ActorTrait AlteredSurface14 = new ActorTrait();
			AlteredSurface14.id = "altered_surface14";
			AlteredSurface14.path_icon = "ui/icons/Skill/AlteredSurface/AlteredSurface14";
			AlteredSurface14.group_id = "biome_magic";
			AlteredSurface14.rarity = Rarity.R2_Epic;
			AlteredSurface14.rate_birth = 0;
			AlteredSurface14.rate_inherit = 100;
			AlteredSurface14.action_special_effect = new WorldAction(Traits01Actions.AlteredSurface_14);
			AssetManager.traits.add(AlteredSurface14);
			addToLocalizedLibrary("ch",AlteredSurface14.id, "群系：樺木群系", "創造樺樹林生物群系！它像漣漪一樣！");
			addToLocalizedLibrary("en",AlteredSurface14.id, "Biome:Birch", "Create the birch grove biome! It's ripples!");
			AlteredSurface14.unlock(true);

			ActorTrait AlteredSurface15 = new ActorTrait();
			AlteredSurface15.id = "altered_surface15";
			AlteredSurface15.path_icon = "ui/icons/Skill/AlteredSurface/AlteredSurface15";
			AlteredSurface15.group_id = "biome_magic";
			AlteredSurface15.rarity = Rarity.R2_Epic;
			AlteredSurface15.rate_birth = 0;
			AlteredSurface15.rate_inherit = 100;
			AlteredSurface15.action_special_effect = new WorldAction(Traits01Actions.AlteredSurface_15);
			AssetManager.traits.add(AlteredSurface15);
			addToLocalizedLibrary("ch",AlteredSurface15.id, "群系：楓樹群系", "創造楓樹林生物群系！加拿大特色！");
			addToLocalizedLibrary("en",AlteredSurface15.id, "Biome:Maple", "Create the maple grove biome! It's Canadian!");
			AlteredSurface15.unlock(true);

			ActorTrait AlteredSurface16 = new ActorTrait();
			AlteredSurface16.id = "altered_surface16";
			AlteredSurface16.path_icon = "ui/icons/Skill/AlteredSurface/AlteredSurface16";
			AlteredSurface16.group_id = "biome_magic";
			AlteredSurface16.rarity = Rarity.R2_Epic;
			AlteredSurface16.rate_birth = 0;
			AlteredSurface16.rate_inherit = 100;
			AlteredSurface16.action_special_effect = new WorldAction(Traits01Actions.AlteredSurface_16);
			AssetManager.traits.add(AlteredSurface16);
			addToLocalizedLibrary("ch",AlteredSurface16.id, "群系：巨石群系", "創造巨石地生物群系！這太牛了！");
			addToLocalizedLibrary("en",AlteredSurface16.id, "Biome:Rocklands", "Create the rocklands biome! It's wtf!");
			AlteredSurface16.unlock(true);

			ActorTrait AlteredSurface17 = new ActorTrait();
			AlteredSurface17.id = "altered_surface17";
			AlteredSurface17.path_icon = "ui/icons/Skill/AlteredSurface/AlteredSurface17";
			AlteredSurface17.group_id = "biome_magic";
			AlteredSurface17.rarity = Rarity.R2_Epic;
			AlteredSurface17.rate_birth = 0;
			AlteredSurface17.rate_inherit = 100;
			AlteredSurface17.action_special_effect = new WorldAction(Traits01Actions.AlteredSurface_17);
			AssetManager.traits.add(AlteredSurface17);
			addToLocalizedLibrary("ch",AlteredSurface17.id, "群系：大蒜群系", "創造大蒜生物群系！它很臭！");
			addToLocalizedLibrary("en",AlteredSurface17.id, "Biome:Garlic", "Create the garlic biome! It's smelly!");
			AlteredSurface17.unlock(true);

			ActorTrait AlteredSurface18 = new ActorTrait();
			AlteredSurface18.id = "altered_surface18";
			AlteredSurface18.path_icon = "ui/icons/Skill/AlteredSurface/AlteredSurface18";
			AlteredSurface18.group_id = "biome_magic";
			AlteredSurface18.rarity = Rarity.R2_Epic;
			AlteredSurface18.rate_birth = 0;
			AlteredSurface18.rate_inherit = 100;
			AlteredSurface18.action_special_effect = new WorldAction(Traits01Actions.AlteredSurface_18);
			AssetManager.traits.add(AlteredSurface18);
			addToLocalizedLibrary("ch",AlteredSurface18.id, "群系：花卉群系", "創造花草甸生物群系！色彩繽紛！");
			addToLocalizedLibrary("en",AlteredSurface18.id, "Biome:Flower", "Create the flower meadow biome! It's colorful!");
			AlteredSurface18.unlock(true);

			ActorTrait AlteredSurface19 = new ActorTrait();
			AlteredSurface19.id = "altered_surface19";
			AlteredSurface19.path_icon = "ui/icons/Skill/AlteredSurface/AlteredSurface19";
			AlteredSurface19.group_id = "biome_magic";
			AlteredSurface19.rarity = Rarity.R2_Epic;
			AlteredSurface19.rate_birth = 0;
			AlteredSurface19.rate_inherit = 100;
			AlteredSurface19.action_special_effect = new WorldAction(Traits01Actions.AlteredSurface_19);
			AssetManager.traits.add(AlteredSurface19);
			addToLocalizedLibrary("ch",AlteredSurface19.id, "群系：天界群系", "創造天界生物群系！簡直就是天堂！");
			addToLocalizedLibrary("en",AlteredSurface19.id, "Biome:Celestial", "Create the celestial biome! It's literally heaven!");
			AlteredSurface19.unlock(true);

			ActorTrait AlteredSurface20 = new ActorTrait();
			AlteredSurface20.id = "altered_surface20";
			AlteredSurface20.path_icon = "ui/icons/Skill/AlteredSurface/AlteredSurface20";
			AlteredSurface20.group_id = "biome_magic";
			AlteredSurface20.rarity = Rarity.R2_Epic;
			AlteredSurface20.rate_birth = 0;
			AlteredSurface20.rate_inherit = 100;
			AlteredSurface20.action_special_effect = new WorldAction(Traits01Actions.AlteredSurface_20);
			AssetManager.traits.add(AlteredSurface20);
			addToLocalizedLibrary("ch",AlteredSurface20.id, "群系：奇點沼澤", "創造奇點沼澤生物群系！它有故障（或說）漏洞百出！");
			addToLocalizedLibrary("en",AlteredSurface20.id, "Biome:Singularity", "Create the singularity swamp biome! It's glitchy (or) It's buggy!");
			AlteredSurface20.unlock(true);

			ActorTrait AlteredSurface21 = new ActorTrait();
			AlteredSurface21.id = "altered_surface21";
			AlteredSurface21.path_icon = "ui/icons/Skill/AlteredSurface/AlteredSurface21";
			AlteredSurface21.group_id = "biome_magic";
			AlteredSurface21.rarity = Rarity.R2_Epic;
			AlteredSurface21.rate_birth = 0;
			AlteredSurface21.rate_inherit = 100;
			AlteredSurface21.action_special_effect = new WorldAction(Traits01Actions.AlteredSurface_21);
			AssetManager.traits.add(AlteredSurface21);
			addToLocalizedLibrary("ch",AlteredSurface21.id, "群系：三葉草群系", "創造三葉草草甸生物群系！太幸運了！");
			addToLocalizedLibrary("en",AlteredSurface21.id, "Biome:Clove", "Create the clover meadow biome! It's lucky!");
			AlteredSurface21.unlock(true);

			ActorTrait AlteredSurface22 = new ActorTrait();
			AlteredSurface22.id = "altered_surface22";
			AlteredSurface22.path_icon = "ui/icons/Skill/AlteredSurface/AlteredSurface22";
			AlteredSurface22.group_id = "biome_magic";
			AlteredSurface22.rarity = Rarity.R2_Epic;
			AlteredSurface22.rate_birth = 0;
			AlteredSurface22.rate_inherit = 100;
			AlteredSurface22.action_special_effect = new WorldAction(Traits01Actions.AlteredSurface_22);
			AssetManager.traits.add(AlteredSurface22);
			addToLocalizedLibrary("ch",AlteredSurface22.id, "群系：悖論群系", "創造融化時鐘生物群系！太夢幻了！");
			addToLocalizedLibrary("en",AlteredSurface22.id, "Biome:Paradox", "Create the melting clocks biome! It's surreal!");
			AlteredSurface22.unlock(true);

			ActorTrait AlteredSurfaceEX1 = new ActorTrait();
			AlteredSurfaceEX1.id = "altered_surface_ex1";
			AlteredSurfaceEX1.path_icon = "ui/icons/Skill/AlteredSurface/AlteredSurfaceEX1";
			AlteredSurfaceEX1.group_id = "biome_magic";
			AlteredSurfaceEX1.rarity = Rarity.R2_Epic;
			AlteredSurfaceEX1.rate_birth = 0;
			AlteredSurfaceEX1.rate_inherit = 10;
			AlteredSurfaceEX1.action_special_effect = new WorldAction(Traits01Actions.Planting);
			AssetManager.traits.add(AlteredSurfaceEX1);
			addToLocalizedLibrary("ch",AlteredSurfaceEX1.id, "果樹叢", "能夠變出果樹，供人食用，/n 在某些環境下無法種植。");
			addToLocalizedLibrary("en",AlteredSurfaceEX1.id, "Fruit Bush", "Able to create fruit trees for people to eat. /n Cannot be used in certain environments.");
			AlteredSurfaceEX1.unlock(true);

			ActorTrait AlteredSurfaceEX2 = new ActorTrait();
			AlteredSurfaceEX2.id = "altered_surface_ex2";
			AlteredSurfaceEX2.path_icon = "ui/icons/Skill/AlteredSurface/AlteredSurfaceEX2";
			AlteredSurfaceEX2.group_id = "biome_magic";
			AlteredSurfaceEX2.rarity = Rarity.R2_Epic;
			AlteredSurfaceEX2.rate_birth = 0;
			AlteredSurfaceEX2.rate_inherit = 10;
			AlteredSurfaceEX2.action_special_effect = new WorldAction(Traits01Actions.DropOre);
			AssetManager.traits.add(AlteredSurfaceEX2);
			addToLocalizedLibrary("ch",AlteredSurfaceEX2.id, "礦物", "能夠變出礦物，供人採集");
			addToLocalizedLibrary("en",AlteredSurfaceEX2.id, "Mineral", "Able to produce minerals for people to collect");
			AlteredSurfaceEX2.unlock(true);
			#endregion
			#region 建造魔法	Construction Magic
			ActorTrait MonsterNest000 = new ActorTrait();
			MonsterNest000.id = "monste_nest000";
			MonsterNest000.path_icon = "ui/icons/Skill/MonsterNest/MonsterNest000";
			MonsterNest000.group_id = "biome_magic";
			MonsterNest000.rarity = Rarity.R2_Epic;
			MonsterNest000.rate_birth = 0;
			MonsterNest000.rate_inherit = 1;
			WorldAction combinedAction_MN000 = (WorldAction)Delegate.Combine(
			new WorldAction(Traits01Actions.removeRuins),
			new WorldAction(Traits01Actions.Nest_000),
			new WorldAction(Traits01Actions.Nest_000_2));
			MonsterNest000.action_special_effect = combinedAction_MN000;
			AssetManager.traits.add(MonsterNest000);
			addToLocalizedLibrary("ch",MonsterNest000.id, "蜂巢", "產生蜜蜂。蜜蜂為花朵授粉");
			addToLocalizedLibrary("en",MonsterNest000.id, "Beehive", "Spawns bees. Bees pollinate flowers.");
			MonsterNest000.unlock(true);

			ActorTrait MonsterNest001 = new ActorTrait();
			MonsterNest001.id = "monste_nest001";
			MonsterNest001.path_icon = "ui/icons/Skill/MonsterNest/MonsterNest001";
			MonsterNest001.group_id = "mn_magic";
			MonsterNest001.rarity = Rarity.R2_Epic;
			MonsterNest001.rate_birth = 0;
			MonsterNest001.rate_inherit = 1;
			WorldAction combinedAction_MN001 = (WorldAction)Delegate.Combine(
			new WorldAction(Traits01Actions.Return_01),
			new WorldAction(Traits01Actions.removeRuins),
			new WorldAction(Traits01Actions.Nest_001),
			new WorldAction(Traits01Actions.DropMode_001));
			MonsterNest001.action_special_effect = combinedAction_MN001;
			AssetManager.traits.add(MonsterNest001);
			addToLocalizedLibrary("ch",MonsterNest001.id, "築巢:腫瘤", "建造巨大腫瘤的魔法");
			addToLocalizedLibrary("en",MonsterNest001.id, "Nest:Tumor", "The magic of building giant Tumors.");
			MonsterNest001.unlock(true);

			ActorTrait MonsterNest002 = new ActorTrait();
			MonsterNest002.id = "monste_nest002";
			MonsterNest002.path_icon = "ui/icons/Skill/MonsterNest/MonsterNest002";
			MonsterNest002.group_id = "mn_magic";
			MonsterNest002.rarity = Rarity.R2_Epic;
			MonsterNest002.rate_birth = 0;
			MonsterNest002.rate_inherit = 1;
			WorldAction combinedAction_MN002 = (WorldAction)Delegate.Combine(
			new WorldAction(Traits01Actions.Return_02),
			new WorldAction(Traits01Actions.removeRuins),
			new WorldAction(Traits01Actions.Nest_002), 
			new WorldAction(Traits01Actions.DropMode_002));
			MonsterNest002.action_special_effect = combinedAction_MN002;
			AssetManager.traits.add(MonsterNest002);
			addToLocalizedLibrary("ch",MonsterNest002.id, "築巢:核心", "建造機械核心的魔法");
			addToLocalizedLibrary("en",MonsterNest002.id, "Nest:Cybercore", "The Magic of Building Cybercore");
			MonsterNest002.unlock(true);

			ActorTrait MonsterNest003 = new ActorTrait();
			MonsterNest003.id = "monste_nest003";
			MonsterNest003.path_icon = "ui/icons/Skill/MonsterNest/MonsterNest003";
			MonsterNest003.group_id = "mn_magic";
			MonsterNest003.rarity = Rarity.R2_Epic;
			MonsterNest003.rate_birth = 0;
			MonsterNest003.rate_inherit = 1;
			WorldAction combinedAction_MN003 = (WorldAction)Delegate.Combine(
			new WorldAction(Traits01Actions.Return_03),
			new WorldAction(Traits01Actions.removeRuins),
			new WorldAction(Traits01Actions.Nest_003), 
			new WorldAction(Traits01Actions.DropMode_003));
			MonsterNest003.action_special_effect = combinedAction_MN003;
			AssetManager.traits.add(MonsterNest003);
			addToLocalizedLibrary("ch",MonsterNest003.id, "築巢:超級南瓜", "建造超級南瓜的魔法");
			addToLocalizedLibrary("en",MonsterNest003.id, "Nest:Super pumpkin", "The magic of building a Super Pumpkin");
			MonsterNest003.unlock(true);

			ActorTrait MonsterNest004 = new ActorTrait();
			MonsterNest004.id = "monste_nest004";
			MonsterNest004.path_icon = "ui/icons/Skill/MonsterNest/MonsterNest004";
			MonsterNest004.group_id = "mn_magic";
			MonsterNest004.rarity = Rarity.R2_Epic;
			MonsterNest004.rate_birth = 0;
			MonsterNest004.rate_inherit = 1;
			WorldAction combinedAction_MN004 = (WorldAction)Delegate.Combine(
			new WorldAction(Traits01Actions.Return_04),
			new WorldAction(Traits01Actions.removeRuins),
			new WorldAction(Traits01Actions.Nest_004),
			new WorldAction(Traits01Actions.DropMode_004));
			MonsterNest004.action_special_effect = combinedAction_MN004;
			AssetManager.traits.add(MonsterNest004);
			addToLocalizedLibrary("ch",MonsterNest004.id, "築巢:大生物質", "建造大生物質的魔法");
			addToLocalizedLibrary("en",MonsterNest004.id, "Nest:Biomass", "The magic of building large Biomass");
			MonsterNest004.unlock(true);

			ActorTrait MonsterNest005 = new ActorTrait();
			MonsterNest005.id = "monste_nest005";
			MonsterNest005.path_icon = "ui/icons/Skill/MonsterNest/MonsterNest005";
			MonsterNest005.group_id = "mn_magic";
			MonsterNest005.rarity = Rarity.R2_Epic;
			MonsterNest005.rate_birth = 0;
			MonsterNest005.rate_inherit = 1;
			WorldAction combinedAction_MN005 = (WorldAction)Delegate.Combine(
			new WorldAction(Traits01Actions.Return_05),
			new WorldAction(Traits01Actions.removeRuins),
			new WorldAction(Traits01Actions.Nest_005),
			new WorldAction(Traits01Actions.DropMode_005));
			MonsterNest005.action_special_effect = combinedAction_MN005;
			AssetManager.traits.add(MonsterNest005);
			addToLocalizedLibrary("ch",MonsterNest005.id, "築巢:冰魔塔", "建造冰魔塔的魔法");
			addToLocalizedLibrary("en",MonsterNest005.id, "Nest:Ice Tower", "The magic of building Ice Towers.");
			MonsterNest005.unlock(true);

			ActorTrait MonsterNest006 = new ActorTrait();
			MonsterNest006.id = "monste_nest006";
			MonsterNest006.path_icon = "ui/icons/Skill/MonsterNest/MonsterNest006";
			MonsterNest006.group_id = "mn_magic";
			MonsterNest006.rarity = Rarity.R2_Epic;
			MonsterNest006.rate_birth = 0;
			MonsterNest006.rate_inherit = 1;
			WorldAction combinedAction_MN006 = (WorldAction)Delegate.Combine(
			new WorldAction(Traits01Actions.Return_06),
			new WorldAction(Traits01Actions.removeRuins),
			new WorldAction(Traits01Actions.Nest_006),
			new WorldAction(Traits01Actions.DropMode_006));
			MonsterNest006.action_special_effect = combinedAction_MN006;
			AssetManager.traits.add(MonsterNest006);
			addToLocalizedLibrary("ch",MonsterNest006.id, "築巢:惡魔塔", "建造惡魔塔的魔法");
			addToLocalizedLibrary("en",MonsterNest006.id, "Nest:Flame Tower", "The magic of building a Flame Tower.");
			MonsterNest006.unlock(true);

			ActorTrait MonsterNest007 = new ActorTrait();
			MonsterNest007.id = "monste_nest007";
			MonsterNest007.path_icon = "ui/icons/Skill/MonsterNest/MonsterNest007";
			MonsterNest007.group_id = "mn_magic";
			MonsterNest007.rarity = Rarity.R2_Epic;
			MonsterNest007.rate_birth = 0;
			MonsterNest007.rate_inherit = 1;
			WorldAction combinedAction_MN007 = (WorldAction)Delegate.Combine(
			new WorldAction(Traits01Actions.Return_07),
			new WorldAction(Traits01Actions.removeRuins),
			new WorldAction(Traits01Actions.Nest_007),
			new WorldAction(Traits01Actions.DropMode_007));
			MonsterNest007.action_special_effect = combinedAction_MN007;
			AssetManager.traits.add(MonsterNest007);
			addToLocalizedLibrary("ch",MonsterNest007.id, "築巢:天使塔", "建造天使塔的魔法，\n此魔法有著許多的攻擊限制， \n攻擊對象如果不正確將無法發動建造。");
			addToLocalizedLibrary("en",MonsterNest007.id, "Nest:Angle Tower", "This spell builds an angel tower.\n This spell has many attack restrictions.\n If the target is incorrect, the tower construction will fail.");
			MonsterNest007.unlock(true);

			ActorTrait MonsterNest008 = new ActorTrait();
			MonsterNest008.id = "monste_nest008";
			MonsterNest008.path_icon = "ui/icons/Skill/MonsterNest/MonsterNest008";
			MonsterNest008.group_id = "mn_magic";
			MonsterNest008.rarity = Rarity.R2_Epic;
			MonsterNest008.rate_birth = 0;
			MonsterNest008.rate_inherit = 1;
			WorldAction combinedAction_MN008 = (WorldAction)Delegate.Combine(
			new WorldAction(Traits01Actions.removeRuins),
			new WorldAction(Traits01Actions.DropMode_008));
			MonsterNest008.action_special_effect = combinedAction_MN008;
			AssetManager.traits.add(MonsterNest008);
			addToLocalizedLibrary("ch",MonsterNest008.id, "建塔:墮落之腦", "建造墮落之腦的魔法");
			addToLocalizedLibrary("en",MonsterNest008.id, "Tower:Corrupted Brain", "The magic of building a Corrupted Brain");
			MonsterNest008.unlock(true);

			ActorTrait MonsterNest009 = new ActorTrait();
			MonsterNest009.id = "monste_nest009";
			MonsterNest009.path_icon = "ui/icons/Skill/MonsterNest/MonsterNest009";
			MonsterNest009.group_id = "mn_magic";
			MonsterNest009.rarity = Rarity.R2_Epic;
			MonsterNest009.rate_birth = 0;
			MonsterNest009.rate_inherit = 1;
			WorldAction combinedAction_MN009 = (WorldAction)Delegate.Combine(
			new WorldAction(Traits01Actions.removeRuins),
			new WorldAction(Traits01Actions.DropMode_009));
			MonsterNest009.action_special_effect = combinedAction_MN009;
			AssetManager.traits.add(MonsterNest009);
			addToLocalizedLibrary("ch",MonsterNest009.id, "建塔:邪惡電腦", "建造邪惡電腦的魔法");
			addToLocalizedLibrary("en",MonsterNest009.id, "Tower:Evil Computer", "The magic of building Evil Computer");
			MonsterNest009.unlock(true);

			ActorTrait MonsterNest010 = new ActorTrait();
			MonsterNest010.id = "monste_nest010";
			MonsterNest010.path_icon = "ui/icons/Skill/MonsterNest/MonsterNest010";
			MonsterNest010.group_id = "mn_magic";
			MonsterNest010.rarity = Rarity.R2_Epic;
			MonsterNest010.rate_birth = 0;
			MonsterNest010.rate_inherit = 1;
			WorldAction combinedAction_MN010 = (WorldAction)Delegate.Combine(
			new WorldAction(Traits01Actions.removeRuins),
			new WorldAction(Traits01Actions.DropMode_010));
			MonsterNest010.action_special_effect = combinedAction_MN010;
			AssetManager.traits.add(MonsterNest010);
			addToLocalizedLibrary("ch",MonsterNest010.id, "建塔:金蛋", "建造慾望金蛋的魔法");
			addToLocalizedLibrary("en",MonsterNest010.id, "Tower:Golden Egg", "The magic of building the Golden Egg");
			MonsterNest010.unlock(true);

			ActorTrait MonsterNest011 = new ActorTrait();
			MonsterNest011.id = "monste_nest011";
			MonsterNest011.path_icon = "ui/icons/Skill/MonsterNest/MonsterNest011";
			MonsterNest011.group_id = "mn_magic";
			MonsterNest011.rarity = Rarity.R2_Epic;
			MonsterNest011.rate_birth = 0;
			MonsterNest011.rate_inherit = 1;
			WorldAction combinedAction_MN011 = (WorldAction)Delegate.Combine(
			new WorldAction(Traits01Actions.removeRuins),
			new WorldAction(Traits01Actions.DropMode_011));
			MonsterNest011.action_special_effect = combinedAction_MN011;
			AssetManager.traits.add(MonsterNest011);
			addToLocalizedLibrary("ch",MonsterNest011.id, "建塔:空靈豎琴", "建造空靈豎琴的魔法");
			addToLocalizedLibrary("en",MonsterNest011.id, "Tower:Ethereal Harp", "The magic of building an Ethereal Harp");
			MonsterNest011.unlock(true);

			ActorTrait MonsterNest012 = new ActorTrait();
			MonsterNest012.id = "monste_nest012";
			MonsterNest012.path_icon = "ui/icons/Skill/MonsterNest/MonsterNest012";
			MonsterNest012.group_id = "mn_magic";
			MonsterNest012.rarity = Rarity.R2_Epic;
			MonsterNest012.rate_birth = 0;
			MonsterNest012.rate_inherit = 1;
			WorldAction combinedAction_MN012 = (WorldAction)Delegate.Combine(
			new WorldAction(Traits01Actions.removeRuins),
			new WorldAction(Traits01Actions.DropMode_012));
			MonsterNest012.action_special_effect = combinedAction_MN012;
			AssetManager.traits.add(MonsterNest012);
			addToLocalizedLibrary("ch",MonsterNest012.id, "建塔:外星黴菌", "建造外星黴菌的魔法");
			addToLocalizedLibrary("en",MonsterNest012.id, "Tower:Alien Mold", "The magic of building Alien Mold");
			MonsterNest012.unlock(true);

			ActorTrait MonsterNest013 = new ActorTrait();
			MonsterNest013.id = "monste_nest013";
			MonsterNest013.path_icon = "ui/icons/Skill/MonsterNest/MonsterNest013";
			MonsterNest013.group_id = "mn_magic";
			MonsterNest013.rarity = Rarity.R2_Epic;
			MonsterNest013.rate_birth = 0;
			MonsterNest013.rate_inherit = 1;
			WorldAction combinedAction_MN013 = (WorldAction)Delegate.Combine(
			new WorldAction(Traits01Actions.removeRuins),
			new WorldAction(Traits01Actions.DropMode_013));
			MonsterNest013.action_special_effect = combinedAction_MN013;
			AssetManager.traits.add(MonsterNest013);
			addToLocalizedLibrary("ch",MonsterNest013.id, "建塔:硫酸噴泉", "建造硫酸噴泉的魔法");
			addToLocalizedLibrary("en",MonsterNest013.id, "Tower:Acid Geyser", "The magic of building Acid Geyser");
			MonsterNest013.unlock(true);

			ActorTrait MonsterNest014 = new ActorTrait();
			MonsterNest014.id = "monste_nest014";
			MonsterNest014.path_icon = "ui/icons/Skill/MonsterNest/MonsterNest014";
			MonsterNest014.group_id = "mn_magic";
			MonsterNest014.rarity = Rarity.R2_Epic;
			MonsterNest014.rate_birth = 0;
			MonsterNest014.rate_inherit = 1;
			WorldAction combinedAction_MN014 = (WorldAction)Delegate.Combine(
			new WorldAction(Traits01Actions.removeRuins),
			new WorldAction(Traits01Actions.DropMode_014));
			MonsterNest014.action_special_effect = combinedAction_MN014;
			AssetManager.traits.add(MonsterNest014);
			addToLocalizedLibrary("ch",MonsterNest014.id, "建塔:火山", "建造火山的魔法");
			addToLocalizedLibrary("en",MonsterNest014.id, "Tower:Volcano", "The magic of building Volcano");
			MonsterNest014.unlock(true);
			#endregion
			#region 其他
			ActorTrait Other001 = new ActorTrait();
			Other001.id = "other001";
			Other001.path_icon = "ui/icons/Other/Other001";
			Other001.group_id = "special";
			Other001.rarity = Rarity.R2_Epic;
			Other001.rate_birth = 0;
			Other001.rate_inherit = 0;
			WorldAction combinedAction_Other001 = (WorldAction)Delegate.Combine(
			new WorldAction(Traits01Actions.AddMoney));
			Other001.action_special_effect = combinedAction_Other001;
			AssetManager.traits.add(Other001);
			addToLocalizedLibrary("ch",Other001.id, "金錢獎勵", "得到了一筆鉅款");
			addToLocalizedLibrary("en",Other001.id, "Money Reward", "Got a huge sum of money");
			Other001.unlock(true);

			ActorTrait Other002 = new ActorTrait();
			Other002.id = "other002";
			Other002.path_icon = "ui/icons/Other/Other002";
			Other002.group_id = "special";
			Other002.rarity = Rarity.R2_Epic;
			Other002.rate_birth = 0;
			Other002.rate_inherit = 0;
			WorldAction combinedAction_Other002 = (WorldAction)Delegate.Combine(
			new WorldAction(Traits01Actions.AddLoot));
			Other002.action_special_effect = combinedAction_Other002;
			AssetManager.traits.add(Other002);
			addToLocalizedLibrary("ch",Other002.id, "掠奪獎勵", "得到了掠奪品");
			addToLocalizedLibrary("en",Other002.id, "Loot Reward", "Got a huge sum of loot");
			Other002.unlock(true);
			#endregion
/*		#region 對立設置
		//職位特質清單
			HashSet<ActorTrait> oppositeTraitsSet_Pro01 = new HashSet<ActorTrait>();
			string[] traitIDsToOppose_Pro01 = { "pro_king", "pro_groupleader", "pro_soldier", "pro_warrior" };
			foreach (string traitID in traitIDsToOppose_Pro01)
			{
				if (!string.IsNullOrEmpty(traitID)) 
				{
					ActorTrait traitAsset = AssetManager.traits.get(traitID);
					if (traitAsset != null)
					{
						oppositeTraitsSet_Pro01.Add(traitAsset);
					}
				}
			};
			ProK.opposite_traits = oppositeTraitsSet_Pro01;

			HashSet<ActorTrait> oppositeTraitsSet_Pro02 = new HashSet<ActorTrait>();
			string[] traitIDsToOppose_Pro02 = { "pro_leader", "pro_groupleader", "pro_soldier", "pro_warrior" };
			foreach (string traitID in traitIDsToOppose_Pro02)
			{
				if (!string.IsNullOrEmpty(traitID)) 
				{
					ActorTrait traitAsset = AssetManager.traits.get(traitID);
					if (traitAsset != null)
					{
						oppositeTraitsSet_Pro02.Add(traitAsset);
					}
				}
			};
			ProL.opposite_traits = oppositeTraitsSet_Pro02;

			HashSet<ActorTrait> oppositeTraitsSet_Pro03 = new HashSet<ActorTrait>();
			string[] traitIDsToOppose_Pro03 = { "pro_king", "pro_leader", "pro_groupleader", "pro_soldier", "pro_warrior" };
			foreach (string traitID in traitIDsToOppose_Pro03)
			{
				if (!string.IsNullOrEmpty(traitID)) 
				{
					ActorTrait traitAsset = AssetManager.traits.get(traitID);
					if (traitAsset != null)
					{
						oppositeTraitsSet_Pro03.Add(traitAsset);
					}
				}
			};
			ProGL.opposite_traits = oppositeTraitsSet_Pro03;
			ProS.opposite_traits = oppositeTraitsSet_Pro03;
			ProW.opposite_traits = oppositeTraitsSet_Pro03;

		//狀態添加
			HashSet<ActorTrait> oppositeTraitsSet_StatusUp = new HashSet<ActorTrait>();
			string[] traitIDsToOppose_StatusUp = { "status_powerup", "status_caffeinated", "status_enchanted", "status_rage", "status_spellboost", "status_motivated", "status_shield", "status_invincible", "status_AFO", "status_OFA", "status_inspired" };
			foreach (string traitID in traitIDsToOppose_StatusUp)
			{
				if (!string.IsNullOrEmpty(traitID)) 
				{
					ActorTrait traitAsset = AssetManager.traits.get(traitID);
					if (traitAsset != null)
					{
						oppositeTraitsSet_StatusUp.Add(traitAsset);
					}
				}
			}
			Skill0101.opposite_traits = oppositeTraitsSet_StatusUp;
			Skill0102.opposite_traits = oppositeTraitsSet_StatusUp;
			Skill0103.opposite_traits = oppositeTraitsSet_StatusUp;
			Skill0104.opposite_traits = oppositeTraitsSet_StatusUp;
			Skill0105.opposite_traits = oppositeTraitsSet_StatusUp;
			Skill0106.opposite_traits = oppositeTraitsSet_StatusUp;
			Skill0107.opposite_traits = oppositeTraitsSet_StatusUp;
			Skill0108.opposite_traits = oppositeTraitsSet_StatusUp;
			Skill0109.opposite_traits = oppositeTraitsSet_StatusUp;
			Skill0198.opposite_traits = oppositeTraitsSet_StatusUp;
			Skill0199.opposite_traits = oppositeTraitsSet_StatusUp;


		//子彈特質
			HashSet<ActorTrait> oppositeTraitsSet_MB = new HashSet<ActorTrait>();
			string[] traitIDsToOppose_MB = { "Projectile01", "Projectile02", "Projectile03", "Projectile04", "Projectile05", "Projectile06", "Projectile07", "Projectile08", "Projectile09", "Projectile10", "Projectile11", "Projectile12", "Projectile13", "Projectile14", "Projectile15", "cb_bulletrain" };
			foreach (string traitID in traitIDsToOppose_MB)
			{
				if (!string.IsNullOrEmpty(traitID)) 
				{
					ActorTrait traitAsset = AssetManager.traits.get(traitID);
					if (traitAsset != null)
					{
						oppositeTraitsSet_MB.Add(traitAsset);
					}
				}
			}
			Projectile01.opposite_traits = oppositeTraitsSet_MB;
			Projectile02.opposite_traits = oppositeTraitsSet_MB;
			Projectile03.opposite_traits = oppositeTraitsSet_MB;
			Projectile04.opposite_traits = oppositeTraitsSet_MB;
			Projectile05.opposite_traits = oppositeTraitsSet_MB;
			Projectile06.opposite_traits = oppositeTraitsSet_MB;
			Projectile07.opposite_traits = oppositeTraitsSet_MB;
			Projectile08.opposite_traits = oppositeTraitsSet_MB;
			Projectile09.opposite_traits = oppositeTraitsSet_MB;
			Projectile10.opposite_traits = oppositeTraitsSet_MB;
			Projectile11.opposite_traits = oppositeTraitsSet_MB;
			Projectile12.opposite_traits = oppositeTraitsSet_MB;
			Projectile13.opposite_traits = oppositeTraitsSet_MB;
			Projectile14.opposite_traits = oppositeTraitsSet_MB;
			Projectile15.opposite_traits = oppositeTraitsSet_MB;
			Skill0003.opposite_traits = oppositeTraitsSet_MB;
			
			
		//附魔攻擊
			HashSet<ActorTrait> oppositeTraitsSet_Add = new HashSet<ActorTrait>();
			string[] traitIDsToOppose_Add = { "add_burning", "add_slowdown", "add_frozen", "add_poisonous", "add_afc", "add_silenced", "add_stunned", "add_unknown", "add_cursed", "add_death", "", "" };
			foreach (string traitID in traitIDsToOppose_Add)
			{
				if (!string.IsNullOrEmpty(traitID)) 
				{
					ActorTrait traitAsset = AssetManager.traits.get(traitID);
					if (traitAsset != null)
					{
						oppositeTraitsSet_Add.Add(traitAsset);
					}
				}
			}
			Skill0201.opposite_traits = oppositeTraitsSet_Add;
			Skill0202.opposite_traits = oppositeTraitsSet_Add;
			Skill0203.opposite_traits = oppositeTraitsSet_Add;
			Skill0204.opposite_traits = oppositeTraitsSet_Add;
			Skill0205.opposite_traits = oppositeTraitsSet_Add;
			Skill0206.opposite_traits = oppositeTraitsSet_Add;
			Skill0207.opposite_traits = oppositeTraitsSet_Add;
			Skill0297.opposite_traits = oppositeTraitsSet_Add;
			Skill0298.opposite_traits = oppositeTraitsSet_Add;
			Skill0299.opposite_traits = oppositeTraitsSet_Add;

			// 淨化聖體 >< 病災法
			HashSet<ActorTrait> oppositeTraitsSet_HolyArts01 = new HashSet<ActorTrait>();
			string[] traitIDsToOppose_HolyArts01 = { "holyarts_ha", "evillaw_disease", "eyepatch", "crippled", "skin_burns" }; 
			foreach (string traitID in traitIDsToOppose_HolyArts01)
			{
				if (!string.IsNullOrEmpty(traitID)) 
				{
					ActorTrait traitAsset = AssetManager.traits.get(traitID);
					if (traitAsset != null)
					{
							oppositeTraitsSet_HolyArts01.Add(traitAsset);
					}
				}
			};
			Skill0301.opposite_traits = oppositeTraitsSet_HolyArts01;
			Skill0405.opposite_traits = oppositeTraitsSet_HolyArts01;
			
			//			  聖 雫
			// 淨化之滴 	▲	 生命之血
			HashSet<ActorTrait> oppositeTraitsSet_HolyArts02 = new HashSet<ActorTrait>();
			string[] traitIDsToOppose_HolyArts02 = { "holyarts_heal", "holyarts_cure", "holyarts_healcure", "" }; 
			foreach (string traitID in traitIDsToOppose_HolyArts02)
			{
				if (!string.IsNullOrEmpty(traitID)) 
				{
					ActorTrait traitAsset = AssetManager.traits.get(traitID);
					if (traitAsset != null)
					{
						oppositeTraitsSet_HolyArts02.Add(traitAsset);
					}
				}
			};
			Skill0302.opposite_traits = oppositeTraitsSet_HolyArts02;
			Skill0303.opposite_traits = oppositeTraitsSet_HolyArts02;
			Skill0304.opposite_traits = oppositeTraitsSet_HolyArts02;
			
			//聖餐	><	餓食法
			HashSet<ActorTrait> oppositeTraitsSet_ES = new HashSet<ActorTrait>();
			string[] traitIDsToOppose_ES = { "holyarts_eucharist", "evillaw_starvation", "", "" }; 
			foreach (string traitID in traitIDsToOppose_ES)
			{
				if (!string.IsNullOrEmpty(traitID)) 
				{
					ActorTrait traitAsset = AssetManager.traits.get(traitID);
					if (traitAsset != null)
					{
						oppositeTraitsSet_ES.Add(traitAsset);
					}
				}
			};
			Skill0310.opposite_traits = oppositeTraitsSet_ES;
			Skill0404.opposite_traits = oppositeTraitsSet_ES;

			//詛咒能力 >< 祝福能力
			HashSet<ActorTrait> oppositeTraitsSet_Curse = new HashSet<ActorTrait>();
			string[] traitIDsToOppose_Curse = { "holyarts_consecration", "blessed", "", "" }; 
			foreach (string traitID in traitIDsToOppose_Curse)
			{
				if (!string.IsNullOrEmpty(traitID)) 
				{
					ActorTrait traitAsset = AssetManager.traits.get(traitID);
					if (traitAsset != null)
					{
						oppositeTraitsSet_Curse.Add(traitAsset);
					}
				}
			};
			Skill0298.opposite_traits = oppositeTraitsSet_Curse;
			Skill0401.opposite_traits = oppositeTraitsSet_Curse;

			//祝福能力 >< 詛咒能力
			HashSet<ActorTrait> oppositeTraitsSet_Bless = new HashSet<ActorTrait>();
			string[] traitIDsToOppose_Bless = { "evillaw_tgc", "add_cursed", "", "" }; 
			foreach (string traitID in traitIDsToOppose_Bless)
			{
				if (!string.IsNullOrEmpty(traitID)) 
				{
					ActorTrait traitAsset = AssetManager.traits.get(traitID);
					if (traitAsset != null)
					{
						oppositeTraitsSet_Bless.Add(traitAsset);
					}
				}
			};
			Skill0309.opposite_traits = oppositeTraitsSet_Bless;

			//自動恢復 >< 吸收能力
			HashSet<ActorTrait> oppositeTraitsSet_Recovery = new HashSet<ActorTrait>();
			string[] traitIDsToOppose_Recovery = { "evillaw_ea", "", "", "" }; 
			foreach (string traitID in traitIDsToOppose_Recovery)
			{
				if (!string.IsNullOrEmpty(traitID)) 
				{
					ActorTrait traitAsset = AssetManager.traits.get(traitID);
					if (traitAsset != null)
					{
						oppositeTraitsSet_Recovery.Add(traitAsset);
					}
				}
			};
			Skill0305.opposite_traits = oppositeTraitsSet_Recovery;
			Skill0306.opposite_traits = oppositeTraitsSet_Recovery;
			Skill0307.opposite_traits = oppositeTraitsSet_Recovery;

			//吸收能力 >< 自動恢復
			HashSet<ActorTrait> oppositeTraitsSet_Absorb = new HashSet<ActorTrait>();
			string[] traitIDsToOppose_Absorb = { "holyarts_health", "holyarts_mana", "holyarts_stamina", "" }; 
			foreach (string traitID in traitIDsToOppose_Absorb)
			{
				if (!string.IsNullOrEmpty(traitID)) 
				{
					ActorTrait traitAsset = AssetManager.traits.get(traitID);
					if (traitAsset != null)
					{
						oppositeTraitsSet_Absorb.Add(traitAsset);
					}
				}
			};
			Skill0407.opposite_traits = oppositeTraitsSet_Absorb;

			//誕生 >< 絕育
			HashSet<ActorTrait> oppositeTraitsSet_BandS = new HashSet<ActorTrait>();
			string[] traitIDsToOppose_BandS = { "holyarts_annunciation", "evillaw_sterilization", "", "" }; 
			foreach (string traitID in traitIDsToOppose_BandS)
			{
				if (!string.IsNullOrEmpty(traitID)) 
				{
					ActorTrait traitAsset = AssetManager.traits.get(traitID);
					if (traitAsset != null)
					{
						oppositeTraitsSet_BandS.Add(traitAsset);
					}
				}
			};
			Skill0308.opposite_traits = oppositeTraitsSet_BandS;
			Skill0409.opposite_traits = oppositeTraitsSet_BandS;

			//智慧 >< 失智
			HashSet<ActorTrait> oppositeTraitsSet_WD = new HashSet<ActorTrait>();
			string[] traitIDsToOppose_WD = { "Mutation00", "evillaw_ew",}; 
			foreach (string traitID in traitIDsToOppose_WD)
			{
				if (!string.IsNullOrEmpty(traitID)) 
				{
					ActorTrait traitAsset = AssetManager.traits.get(traitID);
					if (traitAsset != null)
					{
						oppositeTraitsSet_WD.Add(traitAsset);
					}
				}
			};
			Skill0410.opposite_traits = oppositeTraitsSet_WD;
			#endregion
*/		}
	///////////////////////////////////////////////////////////////////////////////////////////
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
		{// 填充列表相反的特徵
			if (myListTraits.Any())
			{
				foreach(var trait in myListTraits)
				{
					List<string>? curentTraitOppositeList = trait.opposite_list;
					if (curentTraitOppositeList.Any())
					{
						// Ensure opposite_traits list exists
						if (trait.opposite_traits == null)
							trait.opposite_traits = new();
						foreach (var opposite in trait.opposite_list)
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

	}

}