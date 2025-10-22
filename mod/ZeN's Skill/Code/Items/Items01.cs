using HarmonyLib;
using NCMS;
using NCMS.Utils;
using NeoModLoader.api.attributes;
using NeoModLoader.General;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using UnityEngine;
using ReflectionUtility;

namespace ZeN_01
{
	public class Items01
	{
		private const string PathIcon = "ui/icons/items";
		[Hotfixable]
		public static void init()
		{
			loadCustomItems();
		}
		private static void loadCustomItems()
		{

			#region evil sword
			ItemAsset evilsword = AssetManager.items.clone("evil_sword", "$range");
			evilsword.id = "evil_sword";
			evilsword.equipment_subtype = "evil_sword";
			evilsword.group_id = "armament_group01";
			evilsword.material = "basic";
			evilsword.path_icon = $"{PathIcon}/evilsword";
			evilsword.path_gameplay_sprite = $"weapons/{evilsword.id}";
			evilsword.gameplay_sprites = getWeaponSprites(evilsword.id);
			evilsword.path_slash_animation = "effects/slashes/slash_punch";
			evilsword.quality = Rarity.R3_Legendary;
			evilsword.equipment_type = EquipmentType.Weapon;
			evilsword.name_class = "item_class_weapon";
			evilsword.animated = false;										//動畫 true / false
			evilsword.is_pool_weapon = false;								//生成掉落設定 true / false
			evilsword.pool_rate = 10;										//生成掉落機率
			evilsword.durability = 99999;										//物品耐久值
			evilsword.equipment_value = 300000000;								//價值
			evilsword.rigidity_rating = 7;									//韌性評價
			evilsword.special_effect_interval = 0.4f;						//效果間隔
			evilsword.projectile = "BladeBlackWhite";						//子彈種類設置(遠程武器專用,需搭配 projectiles 參數)
			evilsword.base_stats = new();									//參數設置
			evilsword.base_stats.set("projectiles", 1f);					//子彈 [未顯示]
			evilsword.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");//名稱模板
			evilsword.item_modifier_ids = AssetLibrary<EquipmentAsset>.a<string>(new string[]
				{
					"eternal", "cursed"
				});

			//evilsword.action_attack_target = new AttackAction(Items01Actions.EvilSwordSlash);

			AttackAction combinedAction__evilsword01 = (AttackAction)Delegate.Combine(
			new AttackAction(Items01Actions.Anti_OtherRaces),
			new AttackAction(Items01Actions.Anti_Shield),
			new AttackAction(Items01Actions.EvilSwordSlashX1), 
			new AttackAction(Items01Actions.EvilSwordSlash03));
			evilsword.action_attack_target = combinedAction__evilsword01;
			
			WorldAction combinedAction_evilsword00 = (WorldAction)Delegate.Combine(
			new WorldAction(Items01Actions.EvilSwordSlashX2),
			new WorldAction(Items01Actions.EvilSwordSlash06), 
			new WorldAction(Items01Actions.EvilSwordAwakens), 
			new WorldAction(Items01Actions.EvilLawGet01),
			new WorldAction(Items01Actions.Extinguished01), 
			new WorldAction(Traits01Actions.Health_recovery), 
			new WorldAction(Traits01Actions.Stamina_recovery), 
			new WorldAction(Traits01Actions.Mana_recovery), 
			new WorldAction(Traits01Actions.removeTraitXXX), 
			new WorldAction(Items01Actions.addFavoriteWeapon1));
			evilsword.action_special_effect = combinedAction_evilsword00;


			addToLocalizedLibrary("ch",evilsword.id, "傲慢之劍","神授兵裝\n在特定條件下可發揮龐大的力量。");
			addToLocalizedLibrary("en",evilsword.id, "Pride Sword","God's Armor \n It can exert enormous power under certain conditions.");
			AssetManager.items.list.AddItem(evilsword);
			evilsword.unlock(true);
			#endregion
			#region evil gun
			ItemAsset evilgun = AssetManager.items.clone("evil_gun", "$range");
			evilgun.id = "evil_gun";
			evilgun.equipment_subtype = "evil_gun";
			evilgun.group_id = "armament_group01";
			evilgun.material = "basic";
			evilgun.path_icon = $"{PathIcon}/evilgun";
			evilgun.path_gameplay_sprite = $"weapons/{evilgun.id}";
			evilgun.gameplay_sprites = getWeaponSprites(evilgun.id);
			evilgun.path_slash_animation = "effects/slashes/slash_punch";
			evilgun.quality = Rarity.R3_Legendary;
			evilgun.equipment_type = EquipmentType.Weapon;
			evilgun.name_class = "item_class_weapon";
			evilgun.animated = false;										//動畫 true / false
			evilgun.is_pool_weapon = false;								//生成掉落設定 true / false
			evilgun.pool_rate = 10;										//生成掉落機率
			evilgun.durability = 99999;										//物品耐久值
			evilgun.equipment_value = 300000000;								//價值
			evilgun.rigidity_rating = 7;									//韌性評價
			evilgun.special_effect_interval = 0.4f;						//效果間隔
			evilgun.projectile = "HighSpeedBullet";								//子彈種類設置(遠程武器專用,需搭配 projectiles 參數)
			evilgun.base_stats = new();									//參數設置
			evilgun.base_stats.set("projectiles", 1f);					//子彈 [未顯示]
			evilgun.name_templates = AssetLibrary<EquipmentAsset>.l<string>("shotgun_name");//名稱模板
			evilgun.item_modifier_ids = AssetLibrary<EquipmentAsset>.a<string>(new string[]
				{
					"eternal", "cursed"
				});//Anti_Poverty

			//evilgun.action_attack_target = new AttackAction(Items01Actions.EvilGunShooting00);
			
			WorldAction combinedAction_evilgun = (WorldAction)Delegate.Combine(
			new WorldAction(Items01Actions.EvilGunShooting01), 
			new WorldAction(Items01Actions.EvilGunShooting02), 
			new WorldAction(Items01Actions.EvilGunShooting03), 
			new WorldAction(Items01Actions.EvilGunAwakens), 
			new WorldAction(Items01Actions.EvilLawGet02),
			new WorldAction(Traits01Actions.Health_recovery), 
			new WorldAction(Traits01Actions.Stamina_recovery), 
			new WorldAction(Traits01Actions.Mana_recovery), 
			new WorldAction(Traits01Actions.removeTraitXXX), 
			new WorldAction(Items01Actions.addFavoriteWeapon1));
			evilgun.action_special_effect = combinedAction_evilgun;

			AttackAction combinedAction_evilgun02 = (AttackAction)Delegate.Combine(//攻擊發動
			new AttackAction(Items01Actions.EvilGunShooting00_1),
			new AttackAction(Items01Actions.EvilGunShooting00_2),
			new AttackAction(Items01Actions.ForcedBorrowing1),
			new AttackAction(Items01Actions.ForcedBorrowing2),
			new AttackAction(Items01Actions.Anti_Poverty));
			evilgun.action_attack_target = combinedAction_evilgun02;

			addToLocalizedLibrary("ch",evilgun.id, "強欲之銃","神授兵裝\n在特定條件下可發揮龐大的力量。");
			addToLocalizedLibrary("en",evilgun.id, "Greed Gun","God's Armor \n It can exert enormous power under certain conditions.");
			AssetManager.items.list.AddItem(evilgun);
			evilgun.unlock(true);
			#endregion
			#region evil bow
			ItemAsset evilbow = AssetManager.items.clone("evil_bow", "$bow");
			evilbow.id = "evil_bow";
			evilbow.equipment_subtype = "evil_bow";
			evilbow.group_id = "armament_group01";
			evilbow.material = "basic";
			evilbow.path_icon = $"{PathIcon}/evilbow";
			evilbow.path_gameplay_sprite = $"weapons/{evilbow.id}";
			evilbow.gameplay_sprites = getWeaponSprites(evilbow.id);
			evilbow.path_slash_animation = "effects/slashes/slash_bow";
			evilbow.quality = Rarity.R3_Legendary;
			evilbow.equipment_type = EquipmentType.Weapon;
			evilbow.name_class = "item_class_weapon";
			evilbow.animated = false;										//動畫 true / false
			evilbow.is_pool_weapon = false;								//生成掉落設定 true / false
			evilbow.pool_rate = 10;										//生成掉落機率
			evilbow.durability = 99999;										//物品耐久值
			evilbow.equipment_value = 300000000;								//價值
			evilbow.rigidity_rating = 7;									//韌性評價
			evilbow.special_effect_interval = 0.4f;						//效果間隔
			evilbow.projectile = "HighSpeedArrow";						//子彈種類設置(遠程武器專用,需搭配 projectiles 參數)
			evilbow.base_stats = new();									//參數設置
			evilbow.base_stats.set("projectiles", 1f);					//子彈 [未顯示]
			evilbow.name_templates = AssetLibrary<EquipmentAsset>.l<string>("shotgun_name");//名稱模板
			evilbow.item_modifier_ids = AssetLibrary<EquipmentAsset>.a<string>(new string[]
				{
					"eternal", "cursed"
				});

			evilbow.action_attack_target = new AttackAction(Items01Actions.EvilBowMainEffect);
			
			WorldAction combinedAction_evilbow = (WorldAction)Delegate.Combine(
			new WorldAction(Items01Actions.EvilBowShooting01),
			new WorldAction(Items01Actions.EvilBowShooting02),
			new WorldAction(Items01Actions.EvilBowShooting03),
			new WorldAction(Items01Actions.EvilBowShooting04),
			new WorldAction(Items01Actions.EvilBowAwakens),  
			new WorldAction(Items01Actions.EvilLawGet03),
			new WorldAction(Traits01Actions.Health_recovery), 
			new WorldAction(Traits01Actions.Stamina_recovery), 
			new WorldAction(Traits01Actions.Mana_recovery),
			new WorldAction(Traits01Actions.removeTraitXXX),
			new WorldAction(Items01Actions.addFavoriteWeapon1));
			evilbow.action_special_effect = combinedAction_evilbow;


			addToLocalizedLibrary("ch",evilbow.id, "色慾之弓","神授兵裝\n在特定條件下可發揮龐大的力量。");
			addToLocalizedLibrary("en",evilbow.id, "Lust Bow","God's Armor \n It can exert enormous power under certain conditions.");
			AssetManager.items.list.AddItem(evilbow);
			evilbow.unlock(true);
			#endregion
			#region evil gloves
			ItemAsset evilgloves = AssetManager.items.clone("evil_gloves", "$hammer");
			evilgloves.id = "evil_gloves";
			evilgloves.equipment_subtype = "evil_gloves";
			evilgloves.group_id = "armament_group01";
			evilgloves.material = "basic";
			evilgloves.path_icon = $"{PathIcon}/evilgloves";
			evilgloves.path_gameplay_sprite = $"weapons/{evilgloves.id}";
			evilgloves.gameplay_sprites = getWeaponSprites(evilgloves.id);
			evilgloves.path_slash_animation = "effects/slashes/slash_hammer";
			evilgloves.quality = Rarity.R3_Legendary;
			evilgloves.equipment_type = EquipmentType.Weapon;
			evilgloves.name_class = "item_class_weapon";
			evilgloves.animated = false;										//動畫 true / false
			evilgloves.is_pool_weapon = false;								//生成掉落設定 true / false
			evilgloves.pool_rate = 10;										//生成掉落機率
			evilgloves.durability = 99999;										//物品耐久值
			evilgloves.equipment_value = 300000000;								//價值
			evilgloves.rigidity_rating = 7;									//韌性評價
			evilgloves.special_effect_interval = 0.4f;						//效果間隔
			evilgloves.projectile = "HighSpeedArrow";						//子彈種類設置(遠程武器專用,需搭配 projectiles 參數)
			evilgloves.base_stats = new();									//參數設置
			evilgloves.base_stats.set("projectiles", 1f);					//子彈 [未顯示]
			evilgloves.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_hammer_name");//名稱模板
			evilgloves.item_modifier_ids = AssetLibrary<EquipmentAsset>.a<string>(new string[]
				{
					"eternal", "cursed", "stunned"
				});

			WorldAction combinedAction_evilgloves01 = (WorldAction)Delegate.Combine(
			new WorldAction(Items01Actions.EvilGlovesStrike_ArmedAttack01),
			new WorldAction(Items01Actions.EvilGlovesStrike_ArmedAttack02),
			new WorldAction(Items01Actions.EvilGlovesStrike_FinishingAttack),
			new WorldAction(Items01Actions.EvilGlovesAwakens),  
			new WorldAction(Items01Actions.EvilLawGet04),
			new WorldAction(Traits01Actions.Health_recovery), 
			new WorldAction(Traits01Actions.Stamina_recovery), 
			new WorldAction(Traits01Actions.Mana_recovery),
			new WorldAction(Traits01Actions.removeTraitXX),
			new WorldAction(Items01Actions.addFavoriteWeapon0));
			evilgloves.action_special_effect = combinedAction_evilgloves01;

			AttackAction combinedAction_evilgloves02 = (AttackAction)Delegate.Combine(//攻擊發動
			new AttackAction(Items01Actions.Anti_Invincible),
			new AttackAction(Items01Actions.Anti_Angry),
			new AttackAction(Items01Actions.Devour_HungerHealth),
			new AttackAction(Items01Actions.EvilGlovesStrike_Attack01));
			evilgloves.action_attack_target = combinedAction_evilgloves02;

			addToLocalizedLibrary("ch",evilgloves.id, "憤怒手甲","神授兵裝\n在特定條件下可發揮龐大的力量。");
			addToLocalizedLibrary("en",evilgloves.id, "Wrath Gauntlets","God's Armor \n It can exert enormous power under certain conditions.");
			AssetManager.items.list.AddItem(evilgloves);
			evilgloves.unlock(true);
			#endregion
			#region evil spear
			ItemAsset evilspear = AssetManager.items.clone("evil_spear", "$spear");
			evilspear.id = "evil_spear";
			evilspear.equipment_subtype = "evil_spear";
			evilspear.group_id = "armament_group01";
			evilspear.material = "basic";
			evilspear.path_icon = $"{PathIcon}/evilspear";
			evilspear.path_gameplay_sprite = $"weapons/{evilspear.id}";
			evilspear.gameplay_sprites = getWeaponSprites(evilspear.id);
			evilspear.path_slash_animation = "effects/slashes/slash_spear";
			evilspear.quality = Rarity.R3_Legendary;
			evilspear.equipment_type = EquipmentType.Weapon;
			evilspear.name_class = "item_class_weapon";
			evilspear.animated = false;										//動畫 true / false
			evilspear.is_pool_weapon = false;								//生成掉落設定 true / false
			evilspear.pool_rate = 10;										//生成掉落機率
			evilspear.durability = 99999;										//物品耐久值
			evilspear.equipment_value = 300000000;								//價值
			evilspear.rigidity_rating = 7;									//韌性評價
			evilspear.special_effect_interval = 0.4f;						//效果間隔
			evilspear.projectile = "HighSpeedArrow";						//子彈種類設置(遠程武器專用,需搭配 projectiles 參數)
			evilspear.base_stats = new();									//參數設置
			evilspear.base_stats.set("projectiles", 1f);					//子彈 [未顯示]
			evilspear.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_hammer_name");//名稱模板
			evilspear.item_modifier_ids = AssetLibrary<EquipmentAsset>.a<string>(new string[]
				{
					"eternal", "cursed", ""
				});

			evilspear.action_attack_target = new AttackAction(Items01Actions.Anti_Hungry);
			
			WorldAction combinedAction_evilspear = (WorldAction)Delegate.Combine(
			new WorldAction(Items01Actions.EvilSpearThrowing01),	//魔王的唾液
			new WorldAction(Items01Actions.EvilSpearThrowingXX),	//魔王的餐桌(中轉用,2種射擊模射
			new WorldAction(Items01Actions.EvilSpearThrowing04),	//魔王的餐桌
			new WorldAction(Items01Actions.Acid_Domain),			//酸性領域
			new WorldAction(Items01Actions.EvilSpearAwakens),  		//武器覺醒
			new WorldAction(Items01Actions.EvilLawGet05),			//邪法傳承狀態給予
			//new WorldAction(Traits01Actions.Health_recovery), 
			new WorldAction(Traits01Actions.Stamina_recovery), 
			new WorldAction(Traits01Actions.Mana_recovery),
			new WorldAction(Traits01Actions.removeTraitXXX),
			new WorldAction(Items01Actions.addFavoriteWeapon1));
			evilspear.action_special_effect = combinedAction_evilspear;


			addToLocalizedLibrary("ch",evilspear.id, "暴食餐具","神授兵裝\n在特定條件下可發揮龐大的力量。");
			addToLocalizedLibrary("en",evilspear.id, "Gluttony Tableware","God's Armor \n It can exert enormous power under certain conditions.");
			AssetManager.items.list.AddItem(evilspear);
			evilspear.unlock(true);
			#endregion
			#region evil stick
			ItemAsset evilstick = AssetManager.items.clone("evil_stick", "$weapon");
			evilstick.id = "evil_stick";
			evilstick.equipment_subtype = "evil_stick";
			evilstick.group_id = "armament_group01";
			evilstick.material = "basic";
			evilstick.path_icon = $"{PathIcon}/evilstick";
			evilstick.path_gameplay_sprite = $"weapons/{evilstick.id}";
			evilstick.gameplay_sprites = getWeaponSprites(evilstick.id);
			evilstick.path_slash_animation = "effects/slashes/slash_base";
			evilstick.quality = Rarity.R3_Legendary;
			evilstick.equipment_type = EquipmentType.Weapon;
			evilstick.name_class = "item_class_weapon";
			evilstick.animated = false;										//動畫 true / false
			evilstick.is_pool_weapon = false;								//生成掉落設定 true / false
			evilstick.pool_rate = 10;										//生成掉落機率
			evilstick.durability = 99999;									//物品耐久值
			evilstick.equipment_value = 300000000;							//價值
			evilstick.rigidity_rating = 7;									//韌性評價
			evilstick.special_effect_interval = 0.4f;						//效果間隔
			evilstick.projectile = "HighSpeedArrow";						//子彈種類設置(遠程武器專用,需搭配 projectiles 參數)
			evilstick.base_stats = new();									//參數設置
			evilstick.base_stats.set("projectiles", 1f);					//子彈 [未顯示]
			evilstick.name_templates = AssetLibrary<EquipmentAsset>.l<string>("ice_hammer_name");//名稱模板
			evilstick.item_modifier_ids = AssetLibrary<EquipmentAsset>.a<string>(new string[]
				{
					"eternal", "cursed", ""
				});
			AttackAction combinedAction_evilstickATK = (AttackAction)Delegate.Combine(//攻擊發動
			new AttackAction(Items01Actions.Anti_NoSleeping),
			new AttackAction(ActionLibrary.addFrozenEffectOnTarget));
			evilstick.action_attack_target = combinedAction_evilstickATK;
			
			WorldAction combinedAction_evilstick = (WorldAction)Delegate.Combine(
			new WorldAction(Items01Actions.EvilStickThrowing01),
			new WorldAction(Items01Actions.EvilStickThrowing02),
			new WorldAction(Items01Actions.EvilStickThrowing03),
			new WorldAction(Items01Actions.EvilStickAwakens),  
			new WorldAction(Items01Actions.EvilLawGet06),
			new WorldAction(Traits01Actions.Health_recovery), 
			new WorldAction(Traits01Actions.Stamina_recovery), 
			new WorldAction(Traits01Actions.Mana_recovery),
			new WorldAction(Traits01Actions.removeTraitXXX),
			new WorldAction(Items01Actions.addFavoriteWeapon1));
			evilstick.action_special_effect = combinedAction_evilstick;


			addToLocalizedLibrary("ch",evilstick.id, "怠惰之杖","神授兵裝\n在特定條件下可發揮龐大的力量。");
			addToLocalizedLibrary("en",evilstick.id, "Sloth Stick","God's Armor \n It can exert enormous power under certain conditions.");
			AssetManager.items.list.AddItem(evilstick);
			evilstick.unlock(true);
			#endregion
			#region evil poniard
			ItemAsset evilponiard = AssetManager.items.clone("evil_poniard", "$weapon");
			evilponiard.id = "evil_poniard";
			evilponiard.equipment_subtype = "evil_poniard";
			evilponiard.group_id = "armament_group01";
			evilponiard.material = "basic";
			evilponiard.path_icon = $"{PathIcon}/evilponiard";
			evilponiard.path_gameplay_sprite = $"weapons/{evilponiard.id}";
			evilponiard.gameplay_sprites = getWeaponSprites(evilponiard.id);
			evilponiard.path_slash_animation = "effects/slashes/slash_base";
			evilponiard.quality = Rarity.R3_Legendary;
			evilponiard.equipment_type = EquipmentType.Weapon;
			evilponiard.name_class = "item_class_weapon";
			evilponiard.animated = false;										//動畫 true / false
			evilponiard.is_pool_weapon = false;								//生成掉落設定 true / false
			evilponiard.pool_rate = 10;										//生成掉落機率
			evilponiard.durability = 99999;									//物品耐久值
			evilponiard.equipment_value = 300000000;							//價值
			evilponiard.rigidity_rating = 7;									//韌性評價
			evilponiard.special_effect_interval = 0.4f;						//效果間隔
			evilponiard.projectile = "HighSpeedArrow";						//子彈種類設置(遠程武器專用,需搭配 projectiles 參數)
			evilponiard.base_stats = new();									//參數設置
			evilponiard.base_stats.set("projectiles", 1f);					//子彈 [未顯示]
			evilponiard.name_templates = AssetLibrary<EquipmentAsset>.l<string>("ice_hammer_name");//名稱模板
			evilponiard.item_modifier_ids = AssetLibrary<EquipmentAsset>.a<string>(new string[]
				{
					"eternal", "cursed", ""
				});
			AttackAction combinedAction_evilponiardATK = (AttackAction)Delegate.Combine(//攻擊發動
			new AttackAction(Items01Actions.Compare));
			evilponiard.action_attack_target = combinedAction_evilponiardATK;
			
			WorldAction combinedAction_evilponiard = (WorldAction)Delegate.Combine(
			new WorldAction(Items01Actions.EnvyPoniardHit01),
			new WorldAction(Items01Actions.EnvyPoniardHit02),
			new WorldAction(Items01Actions.EnvyPoniardHit03),
			new WorldAction(Items01Actions.EnvyPoniardAwakens),  
			new WorldAction(Items01Actions.EvilLawGet07),
			new WorldAction(Traits01Actions.Health_recovery), 
			new WorldAction(Traits01Actions.Stamina_recovery), 
			new WorldAction(Traits01Actions.Mana_recovery),
			new WorldAction(Traits01Actions.removeTraitXXX),
			new WorldAction(Items01Actions.addFavoriteWeapon1)
			);
			evilponiard.action_special_effect = combinedAction_evilponiard;


			addToLocalizedLibrary("ch",evilponiard.id, "忌妒短刃","神授兵裝\n在特定條件下可發揮龐大的力量。");
			addToLocalizedLibrary("en",evilponiard.id, "Envy Poniard","God's Armor \n It can exert enormous power under certain conditions.");
			AssetManager.items.list.AddItem(evilponiard);
			evilponiard.unlock(true);
			#endregion
			#region brave_helmet
			ItemAsset bravehelmet = AssetManager.items.clone("brave_helmet", "$helmet");
			bravehelmet.id = "brave_helmet";
			bravehelmet.equipment_subtype = "brave_helmet";
			bravehelmet.group_id = "armament_group01";
			bravehelmet.material = "basic";
			bravehelmet.path_icon = $"{PathIcon}/bravehelmet";
			//bravehelmet.path_gameplay_sprite = $"weapons/{bravehelmet.id}";
			//bravehelmet.gameplay_sprites = getWeaponSprites(bravehelmet.id);
			//bravehelmet.path_slash_animation = "effects/slashes/slash_base";
			bravehelmet.quality = Rarity.R3_Legendary;
			bravehelmet.equipment_type = EquipmentType.Helmet;
			bravehelmet.name_class = "item_class_armor";
			bravehelmet.animated = false;										//動畫 true / false
			bravehelmet.is_pool_weapon = false;								//生成掉落設定 true / false
			bravehelmet.pool_rate = 10;										//生成掉落機率
			bravehelmet.durability = 99999;									//物品耐久值
			bravehelmet.equipment_value = 300000000;							//價值
			bravehelmet.rigidity_rating = 7;									//韌性評價
			bravehelmet.special_effect_interval = 0.4f;						//效果間隔
			//bravehelmet.projectile = "HighSpeedArrow";						//子彈種類設置(遠程武器專用,需搭配 projectiles 參數)
			bravehelmet.base_stats = new();									//參數設置
			//bravehelmet.base_stats.set("projectiles", 1f);					//子彈 [未顯示]
			bravehelmet.name_templates = AssetLibrary<EquipmentAsset>.l<string>("helmet_name");//名稱模板
			bravehelmet.item_modifier_ids = AssetLibrary<EquipmentAsset>.a<string>(new string[]
				{
					"eternal", "cursed", ""
				});

			AttackAction combinedAction_bravehelmetATK = (AttackAction)Delegate.Combine(//攻擊發動
			new AttackAction(Items01Actions.braveATK));
			bravehelmet.action_attack_target = combinedAction_bravehelmetATK;

			WorldAction combinedAction_bravehelmet = (WorldAction)Delegate.Combine(
			new WorldAction(Traits01Actions.JusticeJavelin2), 
			new WorldAction(Traits01Actions.Health_recovery), 
			new WorldAction(Traits01Actions.Stamina_recovery), 
			new WorldAction(Traits01Actions.Mana_recovery),
			new WorldAction(Traits01Actions.removeTraitXXX),
			new WorldAction(Items01Actions.addFavoriteWeaponB)
			);
			bravehelmet.action_special_effect = combinedAction_bravehelmet;


			addToLocalizedLibrary("ch",bravehelmet.id, "勇者護額","作為勇者的象徵物");
			addToLocalizedLibrary("en",bravehelmet.id, "Brave Headband","As a symbol of bravery");
			AssetManager.items.list.AddItem(bravehelmet);
			bravehelmet.unlock(true);
			#endregion
			#region undead_crown
			ItemAsset undeadcrown = AssetManager.items.clone("undead_crown", "$helmet");
			undeadcrown.id = "undead_crown";
			undeadcrown.equipment_subtype = "undead_crown";
			undeadcrown.group_id = "armament_group01";
			undeadcrown.material = "basic";
			undeadcrown.path_icon = $"{PathIcon}/undeadcrown";
			//undeadcrown.path_gameplay_sprite = $"weapons/{undeadcrown.id}";
			//undeadcrown.gameplay_sprites = getWeaponSprites(undeadcrown.id);
			//undeadcrown.path_slash_animation = "effects/slashes/slash_base";
			undeadcrown.quality = Rarity.R3_Legendary;
			undeadcrown.equipment_type = EquipmentType.Helmet;
			undeadcrown.name_class = "item_class_armor";
			undeadcrown.animated = false;										//動畫 true / false
			undeadcrown.is_pool_weapon = false;								//生成掉落設定 true / false
			undeadcrown.pool_rate = 10;										//生成掉落機率
			undeadcrown.durability = 99999;									//物品耐久值
			undeadcrown.equipment_value = 300000000;							//價值
			undeadcrown.rigidity_rating = 7;									//韌性評價
			undeadcrown.special_effect_interval = 0.4f;						//效果間隔
			//undeadcrown.projectile = "HighSpeedArrow";						//子彈種類設置(遠程武器專用,需搭配 projectiles 參數)
			undeadcrown.base_stats = new();									//參數設置
			undeadcrown.base_stats.set("lifespan", 666f);					//子彈 [未顯示]
			undeadcrown.base_stats.set("multiplier_lifespan", 666.66f);		//子彈 [未顯示]
			undeadcrown.base_stats.set("throwing_range", 18f);				//子彈 [未顯示]
			undeadcrown.name_templates = AssetLibrary<EquipmentAsset>.l<string>("helmet_name");//名稱模板
			undeadcrown.item_modifier_ids = AssetLibrary<EquipmentAsset>.a<string>(new string[]
				{
					"eternal", "cursed", ""
				});

			AttackAction combinedAction__undeadcrown01 = (AttackAction)Delegate.Combine(
			new AttackAction(Items01Actions.CrownATK),
			new AttackAction(Items01Actions.Anti_Soul));
			undeadcrown.action_attack_target = combinedAction__undeadcrown01;

			WorldAction combinedAction_undeadcrown = (WorldAction)Delegate.Combine( 
			new WorldAction(Items01Actions.UndeadCrownEffect), 
			new WorldAction(Items01Actions.GiveSoul), 
			new WorldAction(Traits01Actions.Health_recovery), 
			new WorldAction(Traits01Actions.Stamina_recovery), 
			new WorldAction(Traits01Actions.Mana_recovery),
			new WorldAction(Traits01Actions.removeTraitXXX),
			new WorldAction(Items01Actions.addFavoriteWeaponUE)
			);
			undeadcrown.action_special_effect = combinedAction_undeadcrown;

			//undeadcrown.action_death = (WorldAction)Delegate.Combine(undeadcrown.action_death, new WorldAction(Items01Actions.UndeadRebirth));

			addToLocalizedLibrary("ch",undeadcrown.id, "不死者之冠","不死者族群的王冠，擁有統帥不死者的絕對權力");
			addToLocalizedLibrary("en",undeadcrown.id, "Brave Headband","As a symbol of bravery");
			AssetManager.items.list.AddItem(undeadcrown);
			undeadcrown.unlock(true);
			#endregion


			//addWeaponsToWorld();
		}
/*		public static void addWeaponsToWorld()
		{// 增加武器到世界
			//now walker can spawn with ice sword
			var walker = AssetManager.actor_library.get("cold_one");
			if (walker != null)
			{
				walker.use_items = true;
				walker.default_weapons = AssetLibrary<ActorAsset>.a<string>(new string[]
				{
					"ice_hammer", "ice_sword"
				});
				walker.take_items = false;
			}
		}*/
		public static void addToLocalizedLibrary(string planguage, string id, string name, string description)
		{// 新增到本地化資料庫
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
				LocalizedTextManager.add("item_" + id, name);
				LocalizedTextManager.add(id + "_description", description);
			}
		}
		public static Sprite[] getWeaponSprites(string id)
		{
			var sprite = Resources.Load<Sprite>("weapons/" + id);
			if (sprite != null)
				return new Sprite[] { sprite };
			else
			{
				//DarkieTraitsMain.LogError("Can not find weapon sprite for weapon with this id: " + id);
				return Array.Empty<Sprite>();
			}
		}
	}
}
			#region
			#endregion