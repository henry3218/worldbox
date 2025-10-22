using HarmonyLib;
using NCMS.Utils;
using NeoModLoader.api.attributes;
using NeoModLoader.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Chevalier.code
{
    internal class CustomItems
    {
        private const string PathIcon = "ui/Icons/items";
        private const string PathSlash = "ui/effects/slashes";

        [Hotfixable]
        public static void Init()
        {
            loadCustomItems();
        }

        private static void loadCustomItems()
        {
            #region 神圣武器
            // 光明使者 - 圣骑传说中的传奇武器
            ItemAsset lightbringer = AssetManager.items.clone("lightbringer", "$weapon");
            lightbringer.id = "lightbringer";
            lightbringer.material = "adamantine";
            lightbringer.translation_key = "lightbringer";
            lightbringer.equipment_subtype = "lightbringer";
            lightbringer.group_id = "sword";
            lightbringer.animated = false;
            lightbringer.is_pool_weapon = false;
            lightbringer.unlock(true);

            lightbringer.base_stats = new();
            lightbringer.base_stats.set(CustomBaseStatsConstant.Damage, 2500f);
            lightbringer.base_stats.set(CustomBaseStatsConstant.MultiplierSpeed, 1.0f);
            lightbringer.base_stats.set(CustomBaseStatsConstant.MultiplierAttackSpeed, 1.0f);
            lightbringer.base_stats.set(CustomBaseStatsConstant.MultiplierHealth, 0.2f);
            lightbringer.base_stats.set(CustomBaseStatsConstant.ConstructionSpeed, 50f);
            lightbringer.base_stats.set(CustomBaseStatsConstant.MultiplierMana, 0.5f);

            lightbringer.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            lightbringer.equipment_value = 5000;
            lightbringer.special_effect_interval = 0.4f;
            lightbringer.quality = Rarity.R3_Legendary;
            lightbringer.equipment_type = EquipmentType.Weapon;
            lightbringer.name_class = "item_class_weapon";

            lightbringer.item_modifier_ids = AssetLibrary<EquipmentAsset>.a(new string[]
             {
                   "stunned"
             });

            lightbringer.path_slash_animation = "effects/slashes/slash_sword";
            lightbringer.path_icon = $"{PathIcon}/icon_lightbringer";
            lightbringer.path_gameplay_sprite = $"weapons/lightbringer"; //使用现有图片资源
            lightbringer.gameplay_sprites = getWeaponSprites("lightbringer"); //复用现有图片资源
            lightbringer.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            lightbringer.action_attack_target = new AttackAction(CustomItemActions.holyLightSwordAttackEffect);        //特殊攻击效果
            AssetManager.items.list.AddItem(lightbringer);
            addToLocale(lightbringer.id, lightbringer.translation_key, "光明女神赐予的圣剑，能净化世间一切黑暗！");
            #endregion

            #region 西幻风格武器
            // 1. 圣骑士之剑
            ItemAsset paladin_sword = AssetManager.items.clone("paladin_sword", "$weapon");
            paladin_sword.id = "paladin_sword";
            paladin_sword.material = "steel";
            paladin_sword.translation_key = "paladin_sword";
            paladin_sword.equipment_subtype = "paladin_sword";
            paladin_sword.group_id = "sword";
            paladin_sword.animated = false;
            paladin_sword.is_pool_weapon = false;
            paladin_sword.unlock(true);
            paladin_sword.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            paladin_sword.base_stats = new();
            paladin_sword.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            paladin_sword.base_stats.set(CustomBaseStatsConstant.Damage, 1500f);
            paladin_sword.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 10f);
            paladin_sword.base_stats.set(CustomBaseStatsConstant.Speed, 5f);
            paladin_sword.equipment_value = 1000;
            paladin_sword.quality = Rarity.R1_Rare;
            paladin_sword.equipment_type = EquipmentType.Weapon;
            paladin_sword.name_class = "item_class_weapon";
            paladin_sword.path_slash_animation = "effects/slashes/slash_sword";
            paladin_sword.path_icon = $"{PathIcon}/icon_paladin_sword";
            paladin_sword.path_gameplay_sprite = $"weapons/paladin_sword";
            paladin_sword.gameplay_sprites = getWeaponSprites("paladin_sword");
            AssetManager.items.list.AddItem(paladin_sword);
            addToLocale(paladin_sword.id, paladin_sword.translation_key, "圣骑士团的标准配备，剑身上刻有神圣符文！");

            // 2. 精灵长剑
            ItemAsset elven_sword = AssetManager.items.clone("elven_sword", "$weapon");
            elven_sword.id = "elven_sword";
            elven_sword.material = "steel";
            elven_sword.translation_key = "elven_sword";
            elven_sword.equipment_subtype = "elven_sword";
            elven_sword.group_id = "sword";
            elven_sword.animated = false;
            elven_sword.is_pool_weapon = false;
            elven_sword.unlock(true);
            elven_sword.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            elven_sword.base_stats = new();
            elven_sword.base_stats.set(CustomBaseStatsConstant.Damage, 200f);
            elven_sword.base_stats.set(CustomBaseStatsConstant.Armor, 5f);
            elven_sword.base_stats.set(CustomBaseStatsConstant.Health, 1000f);
            elven_sword.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            elven_sword.equipment_value = 1200;
            elven_sword.quality = Rarity.R1_Rare;
            elven_sword.equipment_type = EquipmentType.Weapon;
            elven_sword.name_class = "item_class_weapon";
            elven_sword.path_slash_animation = "effects/slashes/slash_sword";
            elven_sword.path_icon = $"{PathIcon}/icon_elven_sword";
            elven_sword.path_gameplay_sprite = $"weapons/elven_sword";
            elven_sword.gameplay_sprites = getWeaponSprites("elven_sword");
            AssetManager.items.list.AddItem(elven_sword);
            addToLocale(elven_sword.id, elven_sword.translation_key, "精灵王国的传统武器，轻巧锋利，平衡性极佳！");

            // 3. 暗影匕首
            ItemAsset shadow_dagger = AssetManager.items.clone("shadow_dagger", "$weapon");
            shadow_dagger.id = "shadow_dagger";
            shadow_dagger.material = "steel";
            shadow_dagger.translation_key = "shadow_dagger";
            shadow_dagger.equipment_subtype = "shadow_dagger";
            shadow_dagger.group_id = "sword";
            shadow_dagger.animated = false;
            shadow_dagger.is_pool_weapon = false;
            shadow_dagger.unlock(true);
            shadow_dagger.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            shadow_dagger.base_stats = new();
            shadow_dagger.base_stats.set(CustomBaseStatsConstant.Damage, 180f);
            shadow_dagger.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 30f);
            shadow_dagger.base_stats.set(CustomBaseStatsConstant.Speed, 8f);
            shadow_dagger.equipment_value = 1500;
            shadow_dagger.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            shadow_dagger.quality = Rarity.R2_Epic;
            shadow_dagger.equipment_type = EquipmentType.Weapon;
            shadow_dagger.name_class = "item_class_weapon";
            shadow_dagger.path_slash_animation = "effects/slashes/slash_dagger";
            shadow_dagger.path_icon = $"{PathIcon}/icon_shadow_dagger";
            shadow_dagger.path_gameplay_sprite = $"weapons/shadow_dagger";
            shadow_dagger.gameplay_sprites = getWeaponSprites("shadow_dagger");
            shadow_dagger.action_attack_target = new AttackAction(CustomItemActions.shadowBladeAttackEffect);
            AssetManager.items.list.AddItem(shadow_dagger);
            addToLocale(shadow_dagger.id, shadow_dagger.translation_key, "暗影刺客的专用武器，在黑暗中无声杀敌！");

            // 4. 冰霜之剑
            ItemAsset frost_sword = AssetManager.items.clone("frost_sword", "$weapon");
            frost_sword.id = "frost_sword";
            frost_sword.material = "steel";
            frost_sword.translation_key = "frost_sword";
            frost_sword.equipment_subtype = "frost_sword";
            frost_sword.group_id = "sword";
            frost_sword.animated = false;
            frost_sword.is_pool_weapon = false;
            frost_sword.unlock(true);
            frost_sword.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            frost_sword.base_stats = new();
            frost_sword.base_stats.set(CustomBaseStatsConstant.Damage, 250f);
            frost_sword.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 20f);
            frost_sword.base_stats.set(CustomBaseStatsConstant.MultiplierMana, 0.3f);
            frost_sword.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            frost_sword.equipment_value = 2500;
            frost_sword.quality = Rarity.R2_Epic;
            frost_sword.equipment_type = EquipmentType.Weapon;
            frost_sword.name_class = "item_class_weapon";
            frost_sword.path_slash_animation = "effects/slashes/slash_sword";
            frost_sword.path_icon = $"{PathIcon}/icon_frost_sword";
            frost_sword.path_gameplay_sprite = $"weapons/frost_sword";
            frost_sword.gameplay_sprites = getWeaponSprites("frost_sword");
            frost_sword.action_attack_target = new AttackAction(CustomItemActions.frostSwordAttackEffect);
            AssetManager.items.list.AddItem(frost_sword);
            addToLocale(frost_sword.id, frost_sword.translation_key, "以永冻之地的冰晶石打造，剑身上凝聚着永恒的寒气！");

            // 5. 火焰镰刀
            ItemAsset fire_scythe = AssetManager.items.clone("fire_scythe", "$weapon");
            fire_scythe.id = "fire_scythe";
            fire_scythe.material = "adamantine";
            fire_scythe.translation_key = "fire_scythe";
            fire_scythe.equipment_subtype = "fire_scythe";
            fire_scythe.group_id = "sword";
            fire_scythe.animated = false;
            fire_scythe.is_pool_weapon = false;
            fire_scythe.unlock(true);
            fire_scythe.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            fire_scythe.base_stats = new();
            fire_scythe.base_stats.set(CustomBaseStatsConstant.Damage, 300f);
            fire_scythe.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 15f);
            fire_scythe.base_stats.set(CustomBaseStatsConstant.MultiplierMana, 0.4f);
            fire_scythe.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            fire_scythe.equipment_value = 3000;
            fire_scythe.quality = Rarity.R2_Epic;
            fire_scythe.equipment_type = EquipmentType.Weapon;
            fire_scythe.name_class = "item_class_weapon";
            fire_scythe.path_slash_animation = "effects/slashes/slash_scythe";
            fire_scythe.path_icon = $"{PathIcon}/icon_fire_scythe";
            fire_scythe.path_gameplay_sprite = $"weapons/fire_scythe";
            fire_scythe.gameplay_sprites = getWeaponSprites("fire_scythe");
            fire_scythe.action_attack_target = new AttackAction(CustomItemActions.fireScytheAttackEffect);
            AssetManager.items.list.AddItem(fire_scythe);
            addToLocale(fire_scythe.id, fire_scythe.translation_key, "燃烧着地狱烈焰的镰刀，每一次挥舞都能收割敌人的生命！");

            // 6. 生命法杖
            ItemAsset life_staff = AssetManager.items.clone("life_staff", "$weapon");
            life_staff.id = "life_staff";
            life_staff.material = "adamantine";
            life_staff.translation_key = "life_staff";
            life_staff.equipment_subtype = "life_staff";
            life_staff.group_id = "sword";
            life_staff.animated = false;
            life_staff.is_pool_weapon = false;
            life_staff.unlock(true);
            life_staff.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            life_staff.base_stats = new();
            life_staff.base_stats.set(CustomBaseStatsConstant.Damage, 150f);
            life_staff.base_stats.set(CustomBaseStatsConstant.MultiplierMana, 1.0f);
            life_staff.base_stats.set(CustomBaseStatsConstant.Health, 2000f);
            life_staff.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            life_staff.equipment_value = 2000;
            life_staff.quality = Rarity.R2_Epic;
            life_staff.equipment_type = EquipmentType.Weapon;
            life_staff.name_class = "item_class_weapon";
            life_staff.path_slash_animation = "effects/slashes/slash_staff";
            life_staff.path_icon = $"{PathIcon}/icon_life_staff";
            life_staff.path_gameplay_sprite = $"weapons/life_staff";
            life_staff.gameplay_sprites = getWeaponSprites("life_staff");
            life_staff.action_attack_target = new AttackAction(CustomItemActions.lifeStaffAttackEffect);
            AssetManager.items.list.AddItem(life_staff);
            addToLocale(life_staff.id, life_staff.translation_key, "掠夺生命之力的魔剑，能在战斗中吸收敌人伤口里的生命力！");

            // 7. 光明锤
            ItemAsset light_hammer = AssetManager.items.clone("light_hammer", "$weapon");
            light_hammer.id = "light_hammer";
            light_hammer.material = "adamantine";
            light_hammer.translation_key = "light_hammer";
            light_hammer.equipment_subtype = "light_hammer";
            light_hammer.group_id = "sword";
            light_hammer.animated = false;
            light_hammer.is_pool_weapon = false;
            light_hammer.unlock(true);
            light_hammer.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            light_hammer.base_stats = new();
            light_hammer.base_stats.set(CustomBaseStatsConstant.Damage, 350f);
            light_hammer.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 5f);
            light_hammer.base_stats.set(CustomBaseStatsConstant.Knockback, 0.4f);
            light_hammer.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            light_hammer.equipment_value = 2500;
            light_hammer.quality = Rarity.R2_Epic;
            light_hammer.equipment_type = EquipmentType.Weapon;
            light_hammer.name_class = "item_class_weapon";
            light_hammer.path_slash_animation = "effects/slashes/slash_hammer";
            light_hammer.path_icon = $"{PathIcon}/icon_light_hammer";
            light_hammer.path_gameplay_sprite = $"weapons/light_hammer";
            light_hammer.gameplay_sprites = getWeaponSprites("light_hammer");
            light_hammer.action_attack_target = new AttackAction(CustomItemActions.lightHammerAttackEffect);
            AssetManager.items.list.AddItem(light_hammer);
            addToLocale(light_hammer.id, light_hammer.translation_key, "散发着神圣光芒的战锤，能驱散黑暗并净化邪恶！");

            // 8. 毒牙匕首
            ItemAsset venom_dagger = AssetManager.items.clone("venom_dagger", "$weapon");
            venom_dagger.id = "venom_dagger";
            venom_dagger.material = "steel";
            venom_dagger.translation_key = "venom_dagger";
            venom_dagger.equipment_subtype = "venom_dagger";
            venom_dagger.group_id = "sword";
            venom_dagger.animated = false;
            venom_dagger.is_pool_weapon = false;
            venom_dagger.unlock(true);
            venom_dagger.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            venom_dagger.base_stats = new();
            venom_dagger.base_stats.set(CustomBaseStatsConstant.Damage, 200f);
            venom_dagger.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 25f);
            venom_dagger.base_stats.set(CustomBaseStatsConstant.Speed, 6f);
            venom_dagger.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            venom_dagger.equipment_value = 1800;
            venom_dagger.quality = Rarity.R2_Epic;
            venom_dagger.equipment_type = EquipmentType.Weapon;
            venom_dagger.name_class = "item_class_weapon";
            venom_dagger.path_slash_animation = "effects/slashes/slash_dagger";
            venom_dagger.path_icon = $"{PathIcon}/icon_venom_dagger";
            venom_dagger.path_gameplay_sprite = $"weapons/venom_dagger";
            venom_dagger.gameplay_sprites = getWeaponSprites("venom_dagger");
            venom_dagger.action_attack_target = new AttackAction(CustomItemActions.venomDaggerAttackEffect);
            AssetManager.items.list.AddItem(venom_dagger);
            addToLocale(venom_dagger.id, venom_dagger.translation_key, "涂满致命毒药的匕首，被刺中的敌人会慢慢痛苦地死去！");
            #endregion // 西幻风格武器

            ItemAsset light_shield = AssetManager.items.clone("light_shield", "$weapon");
            light_shield.id = "light_shield";
            light_shield.material = "steel";
            light_shield.translation_key = "light_shield";
            light_shield.equipment_subtype = "light_shield";
            light_shield.group_id = "sword";
            light_shield.animated = false;
            light_shield.is_pool_weapon = false;
            light_shield.unlock(true);
            light_shield.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            light_shield.base_stats = new();
            light_shield.base_stats.set(CustomBaseStatsConstant.Damage, 210f);
            light_shield.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 8f);
            light_shield.base_stats.set(CustomBaseStatsConstant.Lifespan, 50f);
            light_shield.equipment_value = 1500;
            light_shield.quality = Rarity.R1_Rare;
            light_shield.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            light_shield.equipment_type = EquipmentType.Weapon;
            light_shield.name_class = "item_class_weapon";
            light_shield.path_slash_animation = "effects/slashes/slash_sword";
            light_shield.path_icon = $"{PathIcon}/icon_light_shield";
            light_shield.path_gameplay_sprite = $"weapons/light_shield";
            light_shield.gameplay_sprites = getWeaponSprites("light_shield");
            AssetManager.items.list.AddItem(light_shield);
            addToLocale(light_shield.id, light_shield.translation_key, "东海龙族栖息地所产，枪头雕有金龙图案！");

            // 7. 紫竹杖
            ItemAsset soul_reaper = AssetManager.items.clone("soul_reaper", "$weapon");
            soul_reaper.id = "soul_reaper";
            soul_reaper.material = "steel";
            soul_reaper.translation_key = "soul_reaper";
            soul_reaper.equipment_subtype = "soul_reaper";
            soul_reaper.group_id = "sword";
            soul_reaper.animated = false;
            soul_reaper.is_pool_weapon = false;
            soul_reaper.unlock(true);
            soul_reaper.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            soul_reaper.base_stats = new();
            soul_reaper.base_stats.set(CustomBaseStatsConstant.Damage, 140f);
            soul_reaper.base_stats.set(CustomBaseStatsConstant.Health, 1500f);
            soul_reaper.base_stats.set(CustomBaseStatsConstant.MultiplierHealth, 0.1f);
            soul_reaper.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            soul_reaper.equipment_value = 900;
            soul_reaper.quality = Rarity.R1_Rare;
            soul_reaper.equipment_type = EquipmentType.Weapon;
            soul_reaper.name_class = "item_class_weapon";
            soul_reaper.path_slash_animation = "effects/slashes/slash_sword";
            soul_reaper.path_icon = $"{PathIcon}/icon_soul_reaper";
            soul_reaper.path_gameplay_sprite = $"weapons/soul_reaper";
            soul_reaper.gameplay_sprites = getWeaponSprites("soul_reaper");
            AssetManager.items.list.AddItem(soul_reaper);
            addToLocale(soul_reaper.id, soul_reaper.translation_key, "南海紫竹林千年紫竹所制，韧性十足！");

            // 8. 青钢剑
            ItemAsset storm_bow = AssetManager.items.clone("storm_bow", "$weapon");
            storm_bow.id = "storm_bow";
            storm_bow.material = "steel";
            storm_bow.translation_key = "storm_bow";
            storm_bow.equipment_subtype = "storm_bow";
            storm_bow.group_id = "sword";
            storm_bow.animated = false;
            storm_bow.is_pool_weapon = false;
            storm_bow.unlock(true);
            storm_bow.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            storm_bow.base_stats = new();
            storm_bow.base_stats.set(CustomBaseStatsConstant.Damage, 170f);
            storm_bow.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 11f);
            storm_bow.base_stats.set(CustomBaseStatsConstant.Speed, 4f);
            storm_bow.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            storm_bow.equipment_value = 1200;
            storm_bow.quality = Rarity.R1_Rare;
            storm_bow.equipment_type = EquipmentType.Weapon;
            storm_bow.name_class = "item_class_weapon";
            storm_bow.path_slash_animation = "effects/slashes/slash_sword";
            storm_bow.path_icon = $"{PathIcon}/icon_storm_bow";
            storm_bow.path_gameplay_sprite = $"weapons/storm_bow";
            storm_bow.gameplay_sprites = getWeaponSprites("storm_bow");
            AssetManager.items.list.AddItem(storm_bow);
            addToLocale(storm_bow.id, storm_bow.translation_key, "西域青钢石提炼而成，锋利无比！");

            // 9. 铁骨扇
            ItemAsset hellfire_sword = AssetManager.items.clone("hellfire_sword", "$weapon");
            hellfire_sword.id = "hellfire_sword";
            hellfire_sword.material = "steel";
            hellfire_sword.translation_key = "hellfire_sword";
            hellfire_sword.equipment_subtype = "hellfire_sword";
            hellfire_sword.group_id = "sword";
            hellfire_sword.animated = false;
            hellfire_sword.is_pool_weapon = false;
            hellfire_sword.unlock(true);
            hellfire_sword.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            hellfire_sword.base_stats = new();
            hellfire_sword.base_stats.set(CustomBaseStatsConstant.Damage, 160f);
            hellfire_sword.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 15f);
            hellfire_sword.base_stats.set(CustomBaseStatsConstant.MultiplierAttackSpeed, 0.1f);
            hellfire_sword.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            hellfire_sword.equipment_value = 1300;
            hellfire_sword.quality = Rarity.R1_Rare;
            hellfire_sword.equipment_type = EquipmentType.Weapon;
            hellfire_sword.name_class = "item_class_weapon";
            hellfire_sword.path_slash_animation = "effects/slashes/slash_sword";
            hellfire_sword.path_icon = $"{PathIcon}/icon_hellfire_sword";
            hellfire_sword.path_gameplay_sprite = $"weapons/hellfire_sword";
            hellfire_sword.gameplay_sprites = getWeaponSprites("hellfire_sword");
            AssetManager.items.list.AddItem(hellfire_sword);
            addToLocale(hellfire_sword.id, hellfire_sword.translation_key, "北方铁骨木制作扇骨，轻巧锋利！");

            // 10. 断云刀
            ItemAsset earth_breaker = AssetManager.items.clone("earth_breaker", "$weapon");
            earth_breaker.id = "earth_breaker";
            earth_breaker.material = "steel";
            earth_breaker.translation_key = "earth_breaker";
            earth_breaker.equipment_subtype = "earth_breaker";
            earth_breaker.group_id = "sword";
            earth_breaker.animated = false;
            earth_breaker.is_pool_weapon = false;
            earth_breaker.unlock(true);
            earth_breaker.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            earth_breaker.base_stats = new();
            earth_breaker.base_stats.set(CustomBaseStatsConstant.Damage, 220f);
            earth_breaker.base_stats.set(CustomBaseStatsConstant.Knockback, 0.3f);
            earth_breaker.base_stats.set(CustomBaseStatsConstant.MultiplierDamage, 0.1f);
            earth_breaker.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            earth_breaker.equipment_value = 1600;
            earth_breaker.quality = Rarity.R1_Rare;
            earth_breaker.equipment_type = EquipmentType.Weapon;
            earth_breaker.name_class = "item_class_weapon";
            earth_breaker.path_slash_animation = "effects/slashes/slash_sword";
            earth_breaker.path_icon = $"{PathIcon}/icon_earth_breaker";
            earth_breaker.path_gameplay_sprite = $"weapons/earth_breaker";
            earth_breaker.gameplay_sprites = getWeaponSprites("earth_breaker");
            AssetManager.items.list.AddItem(earth_breaker);
            addToLocale(earth_breaker.id, earth_breaker.translation_key, "西蜀断云峡特产精铁打造，刀气可断云！");

            // 11. 穿云枪
            ItemAsset nature_staff = AssetManager.items.clone("nature_staff", "$weapon");
            nature_staff.id = "nature_staff";
            nature_staff.material = "steel";
            nature_staff.translation_key = "nature_staff";
            nature_staff.equipment_subtype = "nature_staff";
            nature_staff.group_id = "sword";
            nature_staff.animated = false;
            nature_staff.is_pool_weapon = false;
            nature_staff.unlock(true);
            nature_staff.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            nature_staff.base_stats = new();
            nature_staff.base_stats.set(CustomBaseStatsConstant.Damage, 200f);
            nature_staff.base_stats.set(CustomBaseStatsConstant.Speed, 7f);
            nature_staff.base_stats.set(CustomBaseStatsConstant.ConstructionSpeed, 20f);
            nature_staff.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            nature_staff.equipment_value = 1400;
            nature_staff.quality = Rarity.R1_Rare;
            nature_staff.equipment_type = EquipmentType.Weapon;
            nature_staff.name_class = "item_class_weapon";
            nature_staff.path_slash_animation = "effects/slashes/slash_sword";
            nature_staff.path_icon = $"{PathIcon}/icon_nature_staff";
            nature_staff.path_gameplay_sprite = $"weapons/nature_staff";
            nature_staff.gameplay_sprites = getWeaponSprites("nature_staff");
            AssetManager.items.list.AddItem(nature_staff);
            addToLocale(nature_staff.id, nature_staff.translation_key, "北地穿云岭玄铁锻造，枪出如龙穿云！");

            // 12. 玄铁重剑
            ItemAsset moonlight_dagger = AssetManager.items.clone("moonlight_dagger", "$weapon");
            moonlight_dagger.id = "moonlight_dagger";
            moonlight_dagger.material = "steel";
            moonlight_dagger.translation_key = "moonlight_dagger";
            moonlight_dagger.equipment_subtype = "moonlight_dagger";
            moonlight_dagger.group_id = "sword";
            moonlight_dagger.animated = false;
            moonlight_dagger.is_pool_weapon = false;
            moonlight_dagger.unlock(true);
            moonlight_dagger.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            moonlight_dagger.base_stats = new();
            moonlight_dagger.base_stats.set(CustomBaseStatsConstant.Damage, 250f);
            moonlight_dagger.base_stats.set(CustomBaseStatsConstant.Armor, 8f);
            moonlight_dagger.base_stats.set(CustomBaseStatsConstant.Health, 2000f);
            moonlight_dagger.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            moonlight_dagger.equipment_value = 1700;
            moonlight_dagger.quality = Rarity.R1_Rare;
            moonlight_dagger.equipment_type = EquipmentType.Weapon;
            moonlight_dagger.name_class = "item_class_weapon";
            moonlight_dagger.path_slash_animation = "effects/slashes/slash_sword";
            moonlight_dagger.path_icon = $"{PathIcon}/icon_moonlight_dagger";
            moonlight_dagger.path_gameplay_sprite = $"weapons/moonlight_dagger";
            moonlight_dagger.gameplay_sprites = getWeaponSprites("moonlight_dagger");
            AssetManager.items.list.AddItem(moonlight_dagger);
            addToLocale(moonlight_dagger.id, moonlight_dagger.translation_key, "昆仑山脉玄铁打造，重剑无锋大巧不工！");

            // 13. 追风刀
            ItemAsset zhuiFengDao = AssetManager.items.clone("thunder_mace", "$weapon");
            zhuiFengDao.id = "thunder_mace";
            zhuiFengDao.material = "steel";
            zhuiFengDao.translation_key = "thunder_mace";
            zhuiFengDao.equipment_subtype = "thunder_mace";
            zhuiFengDao.group_id = "sword";
            zhuiFengDao.animated = false;
            zhuiFengDao.is_pool_weapon = false;
            zhuiFengDao.unlock(true);
            zhuiFengDao.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            zhuiFengDao.base_stats = new();
            zhuiFengDao.base_stats.set(CustomBaseStatsConstant.Damage, 170f);
            zhuiFengDao.base_stats.set(CustomBaseStatsConstant.Speed, 10f);
            zhuiFengDao.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 13f);
            zhuiFengDao.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            zhuiFengDao.equipment_value = 1500;
            zhuiFengDao.quality = Rarity.R1_Rare;
            zhuiFengDao.equipment_type = EquipmentType.Weapon;
            zhuiFengDao.name_class = "item_class_weapon";
            zhuiFengDao.path_slash_animation = "effects/slashes/slash_sword";
            zhuiFengDao.path_icon = $"{PathIcon}/icon_thunder_mace";
            zhuiFengDao.path_gameplay_sprite = $"weapons/thunder_mace";
            zhuiFengDao.gameplay_sprites = getWeaponSprites("thunder_mace");
            AssetManager.items.list.AddItem(zhuiFengDao);
            addToLocale(zhuiFengDao.id, zhuiFengDao.translation_key, "东方青丘山风属性材料所制，快如疾风！");

            // 14. 伏虎棍
            ItemAsset bright_sword = AssetManager.items.clone("bright_sword", "$weapon");
            bright_sword.id = "bright_sword";
            bright_sword.material = "steel";
            bright_sword.translation_key = "bright_sword";
            bright_sword.equipment_subtype = "bright_sword";
            bright_sword.group_id = "sword";
            bright_sword.animated = false;
            bright_sword.is_pool_weapon = false;
            bright_sword.unlock(true);
            bright_sword.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            bright_sword.base_stats = new();
            bright_sword.base_stats.set(CustomBaseStatsConstant.Damage, 190f);
            bright_sword.base_stats.set(CustomBaseStatsConstant.Health, 180f);
            bright_sword.base_stats.set(CustomBaseStatsConstant.Stamina, 70f);
            bright_sword.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            bright_sword.equipment_value = 1300;
            bright_sword.quality = Rarity.R1_Rare;
            bright_sword.equipment_type = EquipmentType.Weapon;
            bright_sword.name_class = "item_class_weapon";
            bright_sword.path_slash_animation = "effects/slashes/slash_sword";
            bright_sword.path_icon = $"{PathIcon}/icon_bright_sword";
            bright_sword.path_gameplay_sprite = $"weapons/bright_sword";
            bright_sword.gameplay_sprites = getWeaponSprites("bright_sword");
            AssetManager.items.list.AddItem(bright_sword);
            addToLocale(bright_sword.id, bright_sword.translation_key, "南岳衡山百年硬木所制，棍身刻伏虎图案！");

            #region 新增武器15-22
            // 15. 青霜剑
            ItemAsset shadow_staff = AssetManager.items.clone("shadow_staff", "$weapon");
            shadow_staff.id = "shadow_staff";
            shadow_staff.material = "steel";
            shadow_staff.translation_key = "shadow_staff";
            shadow_staff.equipment_subtype = "shadow_staff";
            shadow_staff.group_id = "sword";
            shadow_staff.animated = false;
            shadow_staff.is_pool_weapon = false;
            shadow_staff.unlock(true);
            shadow_staff.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            shadow_staff.base_stats = new();
            shadow_staff.base_stats.set(CustomBaseStatsConstant.Damage, 180f);
            shadow_staff.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 12f);
            shadow_staff.base_stats.set(CustomBaseStatsConstant.Speed, 6f);
            shadow_staff.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            shadow_staff.equipment_value = 1400;
            shadow_staff.quality = Rarity.R1_Rare;
            shadow_staff.equipment_type = EquipmentType.Weapon;
            shadow_staff.name_class = "item_class_weapon";
            shadow_staff.path_slash_animation = "effects/slashes/slash_sword";
            shadow_staff.path_icon = $"{PathIcon}/icon_shadow_staff";
            shadow_staff.path_gameplay_sprite = $"weapons/shadow_staff";
            shadow_staff.gameplay_sprites = getWeaponSprites("shadow_staff");
            AssetManager.items.list.AddItem(shadow_staff);
            addToLocale(shadow_staff.id, shadow_staff.translation_key, "极北冰原青霜寒铁所铸，剑身上凝有永恒霜华！");

            // 16. 赤焰刀
            ItemAsset dwarf_axe = AssetManager.items.clone("dwarf_axe", "$weapon");
            dwarf_axe.id = "dwarf_axe";
            dwarf_axe.material = "steel";
            dwarf_axe.translation_key = "dwarf_axe";
            dwarf_axe.equipment_subtype = "dwarf_axe";
            dwarf_axe.group_id = "sword";
            dwarf_axe.animated = false;
            dwarf_axe.is_pool_weapon = false;
            dwarf_axe.unlock(true);
            dwarf_axe.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            dwarf_axe.base_stats = new();
            dwarf_axe.base_stats.set(CustomBaseStatsConstant.Damage, 210f);
            dwarf_axe.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 9f);
            dwarf_axe.base_stats.set(CustomBaseStatsConstant.MultiplierDamage, 0.1f);
            dwarf_axe.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            dwarf_axe.equipment_value = 1600;
            dwarf_axe.quality = Rarity.R1_Rare;
            dwarf_axe.equipment_type = EquipmentType.Weapon;
            dwarf_axe.name_class = "item_class_weapon";
            dwarf_axe.path_slash_animation = "effects/slashes/slash_sword";
            dwarf_axe.path_icon = $"{PathIcon}/icon_dwarf_axe";
            dwarf_axe.path_gameplay_sprite = $"weapons/dwarf_axe";
            dwarf_axe.gameplay_sprites = getWeaponSprites("dwarf_axe");
            AssetManager.items.list.AddItem(dwarf_axe);
            addToLocale(dwarf_axe.id, dwarf_axe.translation_key, "火山熔岩中赤焰精钢打造，刀身燃烧不灭圣火！");

            // 17. 玄影枪
            ItemAsset mage_staff = AssetManager.items.clone("mage_staff", "$weapon");
            mage_staff.id = "mage_staff";
            mage_staff.material = "steel";
            mage_staff.translation_key = "mage_staff";
            mage_staff.equipment_subtype = "mage_staff";
            mage_staff.group_id = "sword";
            mage_staff.animated = false;
            mage_staff.is_pool_weapon = false;
            mage_staff.unlock(true);
            mage_staff.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            mage_staff.base_stats = new();
            mage_staff.base_stats.set(CustomBaseStatsConstant.Damage, 190f);
            mage_staff.base_stats.set(CustomBaseStatsConstant.Speed, 8f);
            mage_staff.base_stats.set(CustomBaseStatsConstant.Knockback, 0.25f);
            mage_staff.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            mage_staff.equipment_value = 1500;
            mage_staff.quality = Rarity.R1_Rare;
            mage_staff.equipment_type = EquipmentType.Weapon;
            mage_staff.name_class = "item_class_weapon";
            mage_staff.path_slash_animation = "effects/slashes/slash_sword";
            mage_staff.path_icon = $"{PathIcon}/icon_mage_staff";
            mage_staff.path_gameplay_sprite = $"weapons/mage_staff";
            mage_staff.gameplay_sprites = getWeaponSprites("mage_staff");
            AssetManager.items.list.AddItem(mage_staff);
            addToLocale(mage_staff.id, mage_staff.translation_key, "暗影森林玄铁矿石锻造，枪身可隐入阴影！");

            // 18. 金鳞剑
            ItemAsset thunder_hammer = AssetManager.items.clone("thunder_hammer", "$weapon");
            thunder_hammer.id = "thunder_hammer";
            thunder_hammer.material = "steel";
            thunder_hammer.translation_key = "thunder_hammer";
            thunder_hammer.equipment_subtype = "thunder_hammer";
            thunder_hammer.group_id = "sword";
            thunder_hammer.animated = false;
            thunder_hammer.is_pool_weapon = false;
            thunder_hammer.unlock(true);
            thunder_hammer.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            thunder_hammer.base_stats = new();
            thunder_hammer.base_stats.set(CustomBaseStatsConstant.Damage, 170f);
            thunder_hammer.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 13f);
            thunder_hammer.base_stats.set(CustomBaseStatsConstant.Lifespan, 15f);
            thunder_hammer.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            thunder_hammer.equipment_value = 1300;
            thunder_hammer.quality = Rarity.R1_Rare;
            thunder_hammer.equipment_type = EquipmentType.Weapon;
            thunder_hammer.name_class = "item_class_weapon";
            thunder_hammer.path_slash_animation = "effects/slashes/slash_sword";
            thunder_hammer.path_icon = $"{PathIcon}/icon_thunder_hammer";
            thunder_hammer.path_gameplay_sprite = $"weapons/thunder_hammer";
            thunder_hammer.gameplay_sprites = getWeaponSprites("thunder_hammer");
            AssetManager.items.list.AddItem(thunder_hammer);
            addToLocale(thunder_hammer.id, thunder_hammer.translation_key, "东海金鳞鱼鳞片融入精钢，剑身金光流转！");

            // 19. 银月刀
            ItemAsset fire_staff = AssetManager.items.clone("fire_staff", "$weapon");
            fire_staff.id = "fire_staff";
            fire_staff.material = "steel";
            fire_staff.translation_key = "fire_staff";
            fire_staff.equipment_subtype = "fire_staff";
            fire_staff.group_id = "sword";
            fire_staff.animated = false;
            fire_staff.is_pool_weapon = false;
            fire_staff.unlock(true);
            fire_staff.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            fire_staff.base_stats = new();
            fire_staff.base_stats.set(CustomBaseStatsConstant.Damage, 2000f);
            fire_staff.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 11f);
            fire_staff.base_stats.set(CustomBaseStatsConstant.Speed, 7f);
            fire_staff.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            fire_staff.equipment_value = 1400;
            fire_staff.quality = Rarity.R1_Rare;
            fire_staff.equipment_type = EquipmentType.Weapon;
            fire_staff.name_class = "item_class_weapon";
            fire_staff.path_slash_animation = "effects/slashes/slash_sword";
            fire_staff.path_icon = $"{PathIcon}/icon_fire_staff";
            fire_staff.path_gameplay_sprite = $"weapons/fire_staff";
            fire_staff.gameplay_sprites = getWeaponSprites("fire_staff");
            AssetManager.items.list.AddItem(fire_staff);
            addToLocale(fire_staff.id, fire_staff.translation_key, "月光谷银月石提炼而成，刀身映月生辉！");

            // 20. 紫电枪
            ItemAsset chaos_axe = AssetManager.items.clone("chaos_axe", "$weapon");
            chaos_axe.id = "chaos_axe";
            chaos_axe.material = "steel";
            chaos_axe.translation_key = "chaos_axe";
            chaos_axe.equipment_subtype = "chaos_axe";
            chaos_axe.group_id = "sword";
            chaos_axe.animated = false;
            chaos_axe.is_pool_weapon = false;
            chaos_axe.unlock(true);
            chaos_axe.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            chaos_axe.base_stats = new();
            chaos_axe.base_stats.set(CustomBaseStatsConstant.Damage, 2200f);
            chaos_axe.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 10f);
            chaos_axe.base_stats.set(CustomBaseStatsConstant.MultiplierAttackSpeed, 0.1f);
            chaos_axe.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            chaos_axe.equipment_value = 1700;
            chaos_axe.quality = Rarity.R1_Rare;
            chaos_axe.equipment_type = EquipmentType.Weapon;
            chaos_axe.name_class = "item_class_weapon";
            chaos_axe.path_slash_animation = "effects/slashes/slash_sword";
            chaos_axe.path_icon = $"{PathIcon}/icon_chaos_axe";
            chaos_axe.path_gameplay_sprite = $"weapons/chaos_axe";
            chaos_axe.gameplay_sprites = getWeaponSprites("chaos_axe");
            AssetManager.items.list.AddItem(chaos_axe);
            addToLocale(chaos_axe.id, chaos_axe.translation_key, "雷泽紫电石融入寒铁，枪身缠绕紫色电光！");

            // 21. 青竹剑
            ItemAsset wind_bow = AssetManager.items.clone("wind_bow", "$weapon");
            wind_bow.id = "wind_bow";
            wind_bow.material = "steel";
            wind_bow.translation_key = "wind_bow";
            wind_bow.equipment_subtype = "wind_bow";
            wind_bow.group_id = "sword";
            wind_bow.animated = false;
            wind_bow.is_pool_weapon = false;
            wind_bow.unlock(true);
            wind_bow.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            wind_bow.base_stats = new();
            wind_bow.base_stats.set(CustomBaseStatsConstant.Damage, 1500f);
            wind_bow.base_stats.set(CustomBaseStatsConstant.Health, 120f);
            wind_bow.base_stats.set(CustomBaseStatsConstant.Stamina, 60f);
            wind_bow.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            wind_bow.equipment_value = 1100;
            wind_bow.quality = Rarity.R1_Rare;
            wind_bow.equipment_type = EquipmentType.Weapon;
            wind_bow.name_class = "item_class_weapon";
            wind_bow.path_slash_animation = "effects/slashes/slash_sword";
            wind_bow.path_icon = $"{PathIcon}/icon_wind_bow";
            wind_bow.path_gameplay_sprite = $"weapons/wind_bow";
            wind_bow.gameplay_sprites = getWeaponSprites("wind_bow");
            AssetManager.items.list.AddItem(wind_bow);
            addToLocale(wind_bow.id, wind_bow.translation_key, "青竹峰千年青竹制成，剑出带有竹香！");

            #endregion // 神圣武器

            #region 新增剑类武器23-38
            // 23. 枫叶剑
            ItemAsset maple_sword = AssetManager.items.clone("maple_sword", "$weapon");
            maple_sword.id = "maple_sword";
            maple_sword.material = "steel";
            maple_sword.translation_key = "maple_sword";
            maple_sword.equipment_subtype = "maple_sword";
            maple_sword.group_id = "sword";
            maple_sword.animated = false;
            maple_sword.is_pool_weapon = false;
            maple_sword.unlock(true);
            maple_sword.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            maple_sword.base_stats = new();
            maple_sword.base_stats.set(CustomBaseStatsConstant.Damage, 1600f);
            maple_sword.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 15f);
            maple_sword.base_stats.set(CustomBaseStatsConstant.Speed, 5f);
            maple_sword.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            maple_sword.equipment_value = 1200;
            maple_sword.quality = Rarity.R1_Rare;
            maple_sword.equipment_type = EquipmentType.Weapon;
            maple_sword.name_class = "item_class_weapon";
            maple_sword.path_slash_animation = "effects/slashes/slash_sword";
            maple_sword.path_icon = $"{PathIcon}/icon_maple_sword";
            maple_sword.path_gameplay_sprite = "weapons/maple_sword";
            maple_sword.gameplay_sprites = getWeaponSprites("maple_sword");
            AssetManager.items.list.AddItem(maple_sword);
            
            // 24. 寒铁剑
            ItemAsset cold_iron_sword = AssetManager.items.clone("cold_iron_sword", "$weapon");
            cold_iron_sword.id = "cold_iron_sword";
            cold_iron_sword.material = "steel";
            cold_iron_sword.translation_key = "cold_iron_sword";
            cold_iron_sword.equipment_subtype = "cold_iron_sword";
            cold_iron_sword.group_id = "sword";
            cold_iron_sword.animated = false;
            cold_iron_sword.is_pool_weapon = false;
            cold_iron_sword.unlock(true);
            cold_iron_sword.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            cold_iron_sword.base_stats = new();
            cold_iron_sword.base_stats.set(CustomBaseStatsConstant.Damage, 200f);
            cold_iron_sword.base_stats.set(CustomBaseStatsConstant.Knockback, 0.2f);
            cold_iron_sword.base_stats.set(CustomBaseStatsConstant.MultiplierDamage, 0.05f);
            cold_iron_sword.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            cold_iron_sword.equipment_value = 1400;
            cold_iron_sword.quality = Rarity.R1_Rare;
            cold_iron_sword.equipment_type = EquipmentType.Weapon;
            cold_iron_sword.name_class = "item_class_weapon";
            cold_iron_sword.path_slash_animation = "effects/slashes/slash_sword";
            cold_iron_sword.path_icon = $"{PathIcon}/icon_cold_iron_sword";
            cold_iron_sword.path_gameplay_sprite = "weapons/cold_iron_sword";
            cold_iron_sword.gameplay_sprites = getWeaponSprites("cold_iron_sword");
            AssetManager.items.list.AddItem(cold_iron_sword);
            
            // 25. 松木剑
            ItemAsset pine_sword = AssetManager.items.clone("pine_sword", "$weapon");
            pine_sword.id = "pine_sword";
            pine_sword.material = "wood";
            pine_sword.translation_key = "pine_sword";
            pine_sword.equipment_subtype = "pine_sword";
            pine_sword.group_id = "sword";
            pine_sword.animated = false;
            pine_sword.is_pool_weapon = false;
            pine_sword.unlock(true);
            pine_sword.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            pine_sword.base_stats = new();
            pine_sword.base_stats.set(CustomBaseStatsConstant.Damage, 140f);
            pine_sword.base_stats.set(CustomBaseStatsConstant.Health, 1000f);
            pine_sword.base_stats.set(CustomBaseStatsConstant.ConstructionSpeed, 15f);
            pine_sword.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            pine_sword.equipment_value = 1000;
            pine_sword.quality = Rarity.R1_Rare;
            pine_sword.equipment_type = EquipmentType.Weapon;
            pine_sword.name_class = "item_class_weapon";
            pine_sword.path_slash_animation = "effects/slashes/slash_sword";
            pine_sword.path_icon = $"{PathIcon}/icon_pine_sword";
            pine_sword.path_gameplay_sprite = "weapons/pine_sword";
            pine_sword.gameplay_sprites = getWeaponSprites("pine_sword");
            AssetManager.items.list.AddItem(pine_sword);
            
            // 26. 青铜剑
            ItemAsset bronze_sword = AssetManager.items.clone("bronze_sword", "$weapon");
            bronze_sword.id = "bronze_sword";
            bronze_sword.material = "steel";
            bronze_sword.translation_key = "bronze_sword";
            bronze_sword.equipment_subtype = "bronze_sword";
            bronze_sword.group_id = "sword";
            bronze_sword.animated = false;
            bronze_sword.is_pool_weapon = false;
            bronze_sword.unlock(true);
            bronze_sword.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            bronze_sword.base_stats = new();
            bronze_sword.base_stats.set(CustomBaseStatsConstant.Damage, 170f);
            bronze_sword.base_stats.set(CustomBaseStatsConstant.Armor, 5f);
            bronze_sword.base_stats.set(CustomBaseStatsConstant.Stamina, 50f);
            bronze_sword.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            bronze_sword.equipment_value = 1100;
            bronze_sword.quality = Rarity.R1_Rare;
            bronze_sword.equipment_type = EquipmentType.Weapon;
            bronze_sword.name_class = "item_class_weapon";
            bronze_sword.path_slash_animation = "effects/slashes/slash_sword";
            bronze_sword.path_icon = $"{PathIcon}/icon_bronze_sword";
            bronze_sword.path_gameplay_sprite = "weapons/bronze_sword";
            bronze_sword.gameplay_sprites = getWeaponSprites("bronze_sword");
            AssetManager.items.list.AddItem(bronze_sword);
            
            // 27. 水晶剑
            ItemAsset crystal_sword = AssetManager.items.clone("crystal_sword", "$weapon");
            crystal_sword.id = "crystal_sword";
            crystal_sword.material = "steel";
            crystal_sword.translation_key = "crystal_sword";
            crystal_sword.equipment_subtype = "crystal_sword";
            crystal_sword.group_id = "sword";
            crystal_sword.animated = false;
            crystal_sword.is_pool_weapon = false;
            crystal_sword.unlock(true);
            crystal_sword.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            crystal_sword.base_stats = new();
            crystal_sword.base_stats.set(CustomBaseStatsConstant.Damage, 190f);
            crystal_sword.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 8f);
            crystal_sword.base_stats.set(CustomBaseStatsConstant.MultiplierMana, 0.3f);
            crystal_sword.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            crystal_sword.equipment_value = 1300;
            crystal_sword.quality = Rarity.R1_Rare;
            crystal_sword.equipment_type = EquipmentType.Weapon;
            crystal_sword.name_class = "item_class_weapon";
            crystal_sword.path_slash_animation = "effects/slashes/slash_sword";
            crystal_sword.path_icon = $"{PathIcon}/icon_crystal_sword";
            crystal_sword.path_gameplay_sprite = "weapons/crystal_sword";
            crystal_sword.gameplay_sprites = getWeaponSprites("crystal_sword");
            AssetManager.items.list.AddItem(crystal_sword);
            
            // 28. 竹剑
            ItemAsset bamboo_sword = AssetManager.items.clone("bamboo_sword", "$weapon");
            bamboo_sword.id = "bamboo_sword";
            bamboo_sword.material = "steel";
            bamboo_sword.translation_key = "bamboo_sword";
            bamboo_sword.equipment_subtype = "bamboo_sword";
            bamboo_sword.group_id = "sword";
            bamboo_sword.animated = false;
            bamboo_sword.is_pool_weapon = false;
            bamboo_sword.unlock(true);
            bamboo_sword.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            bamboo_sword.base_stats = new();
            bamboo_sword.base_stats.set(CustomBaseStatsConstant.Damage, 130f);
            bamboo_sword.base_stats.set(CustomBaseStatsConstant.Speed, 10f);
            bamboo_sword.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 20f);
            bamboo_sword.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            bamboo_sword.equipment_value = 900;
            bamboo_sword.quality = Rarity.R1_Rare;
            bamboo_sword.equipment_type = EquipmentType.Weapon;
            bamboo_sword.name_class = "item_class_weapon";
            bamboo_sword.path_slash_animation = "effects/slashes/slash_sword";
            bamboo_sword.path_icon = $"{PathIcon}/icon_bamboo_sword";
            bamboo_sword.path_gameplay_sprite = "weapons/bamboo_sword";
            bamboo_sword.gameplay_sprites = getWeaponSprites("bamboo_sword");
            AssetManager.items.list.AddItem(bamboo_sword);
            
            // 29. 黑曜石剑
            ItemAsset obsidian_sword = AssetManager.items.clone("obsidian_sword", "$weapon");
            obsidian_sword.id = "obsidian_sword";
            obsidian_sword.material = "steel";
            obsidian_sword.translation_key = "obsidian_sword";
            obsidian_sword.equipment_subtype = "obsidian_sword";
            obsidian_sword.group_id = "sword";
            obsidian_sword.animated = false;
            obsidian_sword.is_pool_weapon = false;
            obsidian_sword.unlock(true);
            obsidian_sword.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            obsidian_sword.base_stats = new();
            obsidian_sword.base_stats.set(CustomBaseStatsConstant.Damage, 210f);
            obsidian_sword.base_stats.set(CustomBaseStatsConstant.MultiplierDamage, 0.08f);
            obsidian_sword.base_stats.set(CustomBaseStatsConstant.Lifespan, 5f);
            obsidian_sword.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            obsidian_sword.equipment_value = 1500;
            obsidian_sword.quality = Rarity.R1_Rare;
            obsidian_sword.equipment_type = EquipmentType.Weapon;
            obsidian_sword.name_class = "item_class_weapon";
            obsidian_sword.path_slash_animation = "effects/slashes/slash_sword";
            obsidian_sword.path_icon = $"{PathIcon}/icon_obsidian_sword";
            obsidian_sword.path_gameplay_sprite = "weapons/obsidian_sword";
            obsidian_sword.gameplay_sprites = getWeaponSprites("obsidian_sword");
            AssetManager.items.list.AddItem(obsidian_sword);
            
            // 30. 珊瑚剑
            ItemAsset coral_sword = AssetManager.items.clone("coral_sword", "$weapon");
            coral_sword.id = "coral_sword";
            coral_sword.material = "steel";
            coral_sword.translation_key = "coral_sword";
            coral_sword.equipment_subtype = "coral_sword";
            coral_sword.group_id = "sword";
            coral_sword.animated = false;
            coral_sword.is_pool_weapon = false;
            coral_sword.unlock(true);
            coral_sword.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            coral_sword.base_stats = new();
            coral_sword.base_stats.set(CustomBaseStatsConstant.Damage, 150f);
            coral_sword.base_stats.set(CustomBaseStatsConstant.Health, 150f);
            coral_sword.base_stats.set(CustomBaseStatsConstant.Lifespan, 10f);
            coral_sword.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            coral_sword.equipment_value = 1200;
            coral_sword.quality = Rarity.R1_Rare;
            coral_sword.equipment_type = EquipmentType.Weapon;
            coral_sword.name_class = "item_class_weapon";
            coral_sword.path_slash_animation = "effects/slashes/slash_sword";
            coral_sword.path_icon = $"{PathIcon}/icon_coral_sword";
            coral_sword.path_gameplay_sprite = "weapons/coral_sword";
            coral_sword.gameplay_sprites = getWeaponSprites("coral_sword");
            AssetManager.items.list.AddItem(coral_sword);
            
            // 31. 象牙剑
            ItemAsset ivory_sword = AssetManager.items.clone("ivory_sword", "$weapon");
            ivory_sword.id = "ivory_sword";
            ivory_sword.material = "steel";
            ivory_sword.translation_key = "ivory_sword";
            ivory_sword.equipment_subtype = "ivory_sword";
            ivory_sword.group_id = "sword";
            ivory_sword.animated = false;
            ivory_sword.is_pool_weapon = false;
            ivory_sword.unlock(true);
            ivory_sword.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            ivory_sword.base_stats = new();
            ivory_sword.base_stats.set(CustomBaseStatsConstant.Damage, 180f);
            ivory_sword.base_stats.set(CustomBaseStatsConstant.Armor, 7f);
            ivory_sword.base_stats.set(CustomBaseStatsConstant.Lifespan, 80f);
            ivory_sword.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            ivory_sword.equipment_value = 1400;
            ivory_sword.quality = Rarity.R1_Rare;
            ivory_sword.equipment_type = EquipmentType.Weapon;
            ivory_sword.name_class = "item_class_weapon";
            ivory_sword.path_slash_animation = "effects/slashes/slash_sword";
            ivory_sword.path_icon = $"{PathIcon}/icon_ivory_sword";
            ivory_sword.path_gameplay_sprite = "weapons/ivory_sword";
            ivory_sword.gameplay_sprites = getWeaponSprites("ivory_sword");
            AssetManager.items.list.AddItem(ivory_sword);
            
            // 32. 骨剑
            ItemAsset bone_sword = AssetManager.items.clone("bone_sword", "$weapon");
            bone_sword.id = "bone_sword";
            bone_sword.material = "steel";
            bone_sword.translation_key = "bone_sword";
            bone_sword.equipment_subtype = "bone_sword";
            bone_sword.group_id = "sword";
            bone_sword.animated = false;
            bone_sword.is_pool_weapon = false;
            bone_sword.unlock(true);
            bone_sword.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            bone_sword.base_stats = new();
            bone_sword.base_stats.set(CustomBaseStatsConstant.Damage, 170f);
            bone_sword.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 12f);
            bone_sword.base_stats.set(CustomBaseStatsConstant.MultiplierHealth, 0.1f);
            bone_sword.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            bone_sword.equipment_value = 1100;
            bone_sword.quality = Rarity.R1_Rare;
            bone_sword.equipment_type = EquipmentType.Weapon;
            bone_sword.name_class = "item_class_weapon";
            bone_sword.path_slash_animation = "effects/slashes/slash_sword";
            bone_sword.path_icon = $"{PathIcon}/icon_bone_sword";
            bone_sword.path_gameplay_sprite = "weapons/bone_sword";
            bone_sword.gameplay_sprites = getWeaponSprites("bone_sword");
            AssetManager.items.list.AddItem(bone_sword);
            
            // 33. 银剑
            ItemAsset silver_sword = AssetManager.items.clone("silver_sword", "$weapon");
            silver_sword.id = "silver_sword";
            silver_sword.material = "steel";
            silver_sword.translation_key = "silver_sword";
            silver_sword.equipment_subtype = "silver_sword";
            silver_sword.group_id = "sword";
            silver_sword.animated = false;
            silver_sword.is_pool_weapon = false;
            silver_sword.unlock(true);
            silver_sword.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            silver_sword.base_stats = new();
            silver_sword.base_stats.set(CustomBaseStatsConstant.Damage, 200f);
            silver_sword.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 6f);
            silver_sword.base_stats.set(CustomBaseStatsConstant.MultiplierAttackSpeed, 0.05f);
            silver_sword.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            silver_sword.equipment_value = 1600;
            silver_sword.quality = Rarity.R1_Rare;
            silver_sword.equipment_type = EquipmentType.Weapon;
            silver_sword.name_class = "item_class_weapon";
            silver_sword.path_slash_animation = "effects/slashes/slash_sword";
            silver_sword.path_icon = $"{PathIcon}/icon_silver_sword";
            silver_sword.path_gameplay_sprite = "weapons/silver_sword";
            silver_sword.gameplay_sprites = getWeaponSprites("silver_sword");
            AssetManager.items.list.AddItem(silver_sword);
            
            // 34. 赤铜剑
            ItemAsset red_copper_sword = AssetManager.items.clone("red_copper_sword", "$weapon");
            red_copper_sword.id = "red_copper_sword";
            red_copper_sword.material = "steel";
            red_copper_sword.translation_key = "red_copper_sword";
            red_copper_sword.equipment_subtype = "red_copper_sword";
            red_copper_sword.group_id = "sword";
            red_copper_sword.animated = false;
            red_copper_sword.is_pool_weapon = false;
            red_copper_sword.unlock(true);
            red_copper_sword.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            red_copper_sword.base_stats = new();
            red_copper_sword.base_stats.set(CustomBaseStatsConstant.Damage, 160f);
            red_copper_sword.base_stats.set(CustomBaseStatsConstant.Health, 1300f);
            red_copper_sword.base_stats.set(CustomBaseStatsConstant.Stamina, 60f);
            red_copper_sword.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            red_copper_sword.equipment_value = 1100;
            red_copper_sword.quality = Rarity.R1_Rare;
            red_copper_sword.equipment_type = EquipmentType.Weapon;
            red_copper_sword.name_class = "item_class_weapon";
            red_copper_sword.path_slash_animation = "effects/slashes/slash_sword";
            red_copper_sword.path_icon = $"{PathIcon}/icon_red_copper_sword";
            red_copper_sword.path_gameplay_sprite = "weapons/red_copper_sword";
            red_copper_sword.gameplay_sprites = getWeaponSprites("red_copper_sword");
            AssetManager.items.list.AddItem(red_copper_sword);
            
            // 35. 玻璃剑
            ItemAsset glass_sword = AssetManager.items.clone("glass_sword", "$weapon");
            glass_sword.id = "glass_sword";
            glass_sword.material = "steel";
            glass_sword.translation_key = "glass_sword";
            glass_sword.equipment_subtype = "glass_sword";
            glass_sword.group_id = "sword";
            glass_sword.animated = false;
            glass_sword.is_pool_weapon = false;
            glass_sword.unlock(true);
            glass_sword.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            glass_sword.base_stats = new();
            glass_sword.base_stats.set(CustomBaseStatsConstant.Damage, 19f);
            glass_sword.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 18f);
            glass_sword.base_stats.set(CustomBaseStatsConstant.Speed, 7f);
            glass_sword.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            glass_sword.equipment_value = 1200;
            glass_sword.quality = Rarity.R1_Rare;
            glass_sword.equipment_type = EquipmentType.Weapon;
            glass_sword.name_class = "item_class_weapon";
            glass_sword.path_slash_animation = "effects/slashes/slash_sword";
            glass_sword.path_icon = $"{PathIcon}/icon_glass_sword";
            glass_sword.path_gameplay_sprite = "weapons/glass_sword";
            glass_sword.gameplay_sprites = getWeaponSprites("glass_sword");
            AssetManager.items.list.AddItem(glass_sword);
            
            // 36. 符文剑
            ItemAsset rune_sword = AssetManager.items.clone("rune_sword", "$weapon");
            rune_sword.id = "rune_sword";
            rune_sword.material = "steel";
            rune_sword.translation_key = "rune_sword";
            rune_sword.equipment_subtype = "rune_sword";
            rune_sword.group_id = "sword";
            rune_sword.animated = false;
            rune_sword.is_pool_weapon = false;
            rune_sword.unlock(true);
            rune_sword.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            rune_sword.base_stats = new();
            rune_sword.base_stats.set(CustomBaseStatsConstant.Damage, 220f);
            rune_sword.base_stats.set(CustomBaseStatsConstant.MultiplierMana, 0.4f);
            rune_sword.base_stats.set(CustomBaseStatsConstant.MultiplierDamage, 0.7f);
            rune_sword.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            rune_sword.equipment_value = 1700;
            rune_sword.quality = Rarity.R1_Rare;
            rune_sword.equipment_type = EquipmentType.Weapon;
            rune_sword.name_class = "item_class_weapon";
            rune_sword.path_slash_animation = "effects/slashes/slash_sword";
            rune_sword.path_icon = $"{PathIcon}/icon_rune_sword";
            rune_sword.path_gameplay_sprite = "weapons/rune_sword";
            rune_sword.gameplay_sprites = getWeaponSprites("rune_sword");
            AssetManager.items.list.AddItem(rune_sword);
            
            // 37. 精钢剑
            ItemAsset refined_steel_sword = AssetManager.items.clone("refined_steel_sword", "$weapon");
            refined_steel_sword.id = "refined_steel_sword";
            refined_steel_sword.material = "steel";
            refined_steel_sword.translation_key = "refined_steel_sword";
            refined_steel_sword.equipment_subtype = "refined_steel_sword";
            refined_steel_sword.group_id = "sword";
            refined_steel_sword.animated = false;
            refined_steel_sword.is_pool_weapon = false;
            refined_steel_sword.unlock(true);
            refined_steel_sword.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            refined_steel_sword.base_stats = new();
            refined_steel_sword.base_stats.set(CustomBaseStatsConstant.Damage, 210f);
            refined_steel_sword.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 7f);
            refined_steel_sword.base_stats.set(CustomBaseStatsConstant.Knockback, 0.2f);
            refined_steel_sword.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            refined_steel_sword.equipment_value = 1500;
            refined_steel_sword.quality = Rarity.R1_Rare;
            refined_steel_sword.equipment_type = EquipmentType.Weapon;
            refined_steel_sword.name_class = "item_class_weapon";
            refined_steel_sword.path_slash_animation = "effects/slashes/slash_sword";
            refined_steel_sword.path_icon = $"{PathIcon}/icon_refined_steel_sword";
            refined_steel_sword.path_gameplay_sprite = "weapons/refined_steel_sword";
            refined_steel_sword.gameplay_sprites = getWeaponSprites("refined_steel_sword");
            AssetManager.items.list.AddItem(refined_steel_sword);
            
            // 38. 镀金剑
            ItemAsset gold_plated_sword = AssetManager.items.clone("gold_plated_sword", "$weapon");
            gold_plated_sword.id = "gold_plated_sword";
            gold_plated_sword.material = "steel";
            gold_plated_sword.translation_key = "gold_plated_sword";
            gold_plated_sword.equipment_subtype = "gold_plated_sword";
            gold_plated_sword.group_id = "sword";
            gold_plated_sword.animated = false;
            gold_plated_sword.is_pool_weapon = false;
            gold_plated_sword.unlock(true);
            gold_plated_sword.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            gold_plated_sword.base_stats = new();
            gold_plated_sword.base_stats.set(CustomBaseStatsConstant.Damage, 170f);
            gold_plated_sword.base_stats.set(CustomBaseStatsConstant.Health, 16000f);
            gold_plated_sword.base_stats.set(CustomBaseStatsConstant.ConstructionSpeed, 10f);
            gold_plated_sword.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            gold_plated_sword.equipment_value = 2000;
            gold_plated_sword.quality = Rarity.R1_Rare;
            gold_plated_sword.equipment_type = EquipmentType.Weapon;
            gold_plated_sword.name_class = "item_class_weapon";
            gold_plated_sword.path_slash_animation = "effects/slashes/slash_sword";
            gold_plated_sword.path_icon = $"{PathIcon}/icon_gold_plated_sword";
            gold_plated_sword.path_gameplay_sprite = "weapons/gold_plated_sword";
            gold_plated_sword.gameplay_sprites = getWeaponSprites("gold_plated_sword");
            AssetManager.items.list.AddItem(gold_plated_sword);
            #endregion
        }

        private static void addToLocale(string id, string translation_key, string description)
        {
            //This is no longer needed since I have locales folder
            //LM.AddToCurrentLocale(translation_key, translation_key);
            //LM.AddToCurrentLocale($"{id}_description", description);
        }

        public static Sprite[] getWeaponSprites(string id)
        {
            var sprite = Resources.Load<Sprite>("weapons/" + id);
            if (sprite != null)
                return new Sprite[] { sprite };
            else
            {
                UnityEngine.Debug.LogError("Can not find weapon sprite for weapon with this id: " + id);
                return Array.Empty<Sprite>();
            }
        }
    }
}