using HarmonyLib;
using PeerlessOverpoweringWarrior;
using NCMS.Utils;
using NeoModLoader.api.attributes;
using NeoModLoader.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PeerlessOverpoweringWarrior.code
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
            #region 焚天剑
            ItemAsset fenTianJian = AssetManager.items.clone("fen_tian_jian", "$weapon");
            fenTianJian.id = "fen_tian_jian";
            fenTianJian.material = "adamantine"; //神品玄铁铸造
            fenTianJian.translation_key = "fen_tian_jian";
            fenTianJian.equipment_subtype = "fen_tian_jian";
            fenTianJian.group_id = "sword";
            fenTianJian.animated = false;
            fenTianJian.is_pool_weapon = false;
            fenTianJian.unlock(true);

            fenTianJian.base_stats = new();
            fenTianJian.base_stats.set(CustomBaseStatsConstant.Damage, 5000f);
            fenTianJian.base_stats.set(CustomBaseStatsConstant.MultiplierSpeed, 1.0f);
            fenTianJian.base_stats.set(CustomBaseStatsConstant.MultiplierAttackSpeed, 1.0f);
            fenTianJian.base_stats.set(CustomBaseStatsConstant.MultiplierHealth, 0.25f);
            fenTianJian.base_stats.set(CustomBaseStatsConstant.ConstructionSpeed, 50f);
            fenTianJian.base_stats.set(CustomBaseStatsConstant.MultiplierMana, 0.5f);
            fenTianJian.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.5f);
            fenTianJian.base_stats.set(CustomBaseStatsConstant.CriticalDamageMultiplier, 0.8f);
            fenTianJian.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.25f);
            fenTianJian.base_stats[strings.S.damage_range] = 0.6f;//伤害范围

            fenTianJian.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            fenTianJian.equipment_value = 5000;
            fenTianJian.special_effect_interval = 0.4f;
            fenTianJian.quality = Rarity.R3_Legendary;
            fenTianJian.equipment_type = EquipmentType.Weapon;
            fenTianJian.name_class = "item_class_weapon";

            fenTianJian.item_modifier_ids = AssetLibrary<EquipmentAsset>.a(new string[]
             {
                   "stunned"
             });

            fenTianJian.path_slash_animation = "effects/slashes/slash_sword";
            fenTianJian.path_icon = $"{PathIcon}/icon_fen_tian_jian";
            fenTianJian.path_gameplay_sprite = $"weapons/fen_tian_jian"; //使用现有图片资源
            fenTianJian.gameplay_sprites = getWeaponSprites("fen_tian_jian"); //复用现有图片资源

            fenTianJian.action_attack_target = new AttackAction(CustomItemActions.fenTianJianAttackEffect);        //特殊攻击效果
            AssetManager.items.list.AddItem(fenTianJian);
            addToLocale(fenTianJian.id, fenTianJian.translation_key, "上古火神祝融所铸神剑，剑出焚尽苍穹！");
            #endregion

            #region 新增武器1-14
            // 1. 青冥剑
            ItemAsset qingMingJian = AssetManager.items.clone("qing_ming_jian", "$weapon");
            qingMingJian.id = "qing_ming_jian";
            qingMingJian.material = "adamantine";
            qingMingJian.translation_key = "qing_ming_jian";
            qingMingJian.equipment_subtype = "qing_ming_jian";
            qingMingJian.group_id = "sword";
            qingMingJian.animated = false;
            qingMingJian.is_pool_weapon = false;
            qingMingJian.unlock(true);
            qingMingJian.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            qingMingJian.base_stats = new();
            qingMingJian.base_stats.set(CustomBaseStatsConstant.Damage, 1500f);
            qingMingJian.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 10f);
            qingMingJian.base_stats.set(CustomBaseStatsConstant.Speed, 5f);
            qingMingJian.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.15f);
            qingMingJian.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.1f);
            qingMingJian.base_stats.set(CustomBaseStatsConstant.Stamina, 1500f);
            qingMingJian.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            qingMingJian.equipment_value = 1500;
            qingMingJian.quality = Rarity.R3_Legendary;
            qingMingJian.equipment_type = EquipmentType.Weapon;
            qingMingJian.name_class = "item_class_weapon";
            qingMingJian.path_slash_animation = "effects/slashes/slash_sword";
            qingMingJian.path_icon = $"{PathIcon}/icon_qing_ming_jian";
            qingMingJian.path_gameplay_sprite = $"weapons/qing_ming_jian";
            qingMingJian.gameplay_sprites = getWeaponSprites("qing_ming_jian");
            AssetManager.items.list.AddItem(qingMingJian);
            addToLocale(qingMingJian.id, qingMingJian.translation_key, "青松岭上玄铁所铸，剑出如青冥飞鸿！");

            // 2. 玄铁刀
            ItemAsset xuanTieDao = AssetManager.items.clone("xuan_tie_dao", "$weapon");
            xuanTieDao.id = "xuan_tie_dao";
            xuanTieDao.material = "adamantine";
            xuanTieDao.translation_key = "xuan_tie_dao";
            xuanTieDao.equipment_subtype = "xuan_tie_dao";
            xuanTieDao.group_id = "sword";
            xuanTieDao.animated = false;
            xuanTieDao.is_pool_weapon = false;
            xuanTieDao.unlock(true);
            xuanTieDao.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            xuanTieDao.base_stats = new();
            xuanTieDao.base_stats.set(CustomBaseStatsConstant.Damage, 2000f);
            xuanTieDao.base_stats.set(CustomBaseStatsConstant.Armor, 5f);
            xuanTieDao.base_stats.set(CustomBaseStatsConstant.Health, 1000f);
            xuanTieDao.base_stats.set(CustomBaseStatsConstant.Stamina, 200f);
            xuanTieDao.base_stats.set(CustomBaseStatsConstant.Knockback, 0.5f);
            xuanTieDao.base_stats.set(CustomBaseStatsConstant.Lifespan, 150f);
            xuanTieDao.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            xuanTieDao.equipment_value = 1200;
            xuanTieDao.quality = Rarity.R3_Legendary;
            xuanTieDao.equipment_type = EquipmentType.Weapon;
            xuanTieDao.name_class = "item_class_weapon";
            xuanTieDao.path_slash_animation = "effects/slashes/slash_sword";
            xuanTieDao.path_icon = $"{PathIcon}/icon_xuan_tie_dao";
            xuanTieDao.path_gameplay_sprite = $"weapons/xuan_tie_dao";
            xuanTieDao.gameplay_sprites = getWeaponSprites("xuan_tie_dao");
            AssetManager.items.list.AddItem(xuanTieDao);
            addToLocale(xuanTieDao.id, xuanTieDao.translation_key, "黑风山玄铁打造，厚重沉稳，刀劈金石！");

            // 3. 银蛇枪
            ItemAsset yinSheQiang = AssetManager.items.clone("yin_she_qiang", "$weapon");
            yinSheQiang.id = "yin_she_qiang";
            yinSheQiang.material = "adamantine";
            yinSheQiang.translation_key = "yin_she_qiang";
            yinSheQiang.equipment_subtype = "yin_she_qiang";
            yinSheQiang.group_id = "sword";
            yinSheQiang.animated = false;
            yinSheQiang.is_pool_weapon = false;
            yinSheQiang.unlock(true);
            yinSheQiang.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            yinSheQiang.base_stats = new();
            yinSheQiang.base_stats.set(CustomBaseStatsConstant.Damage, 1800f);
            yinSheQiang.base_stats.set(CustomBaseStatsConstant.Speed, 8f);
            yinSheQiang.base_stats.set(CustomBaseStatsConstant.Knockback, 0.2f);
            yinSheQiang.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.2f);
            yinSheQiang.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.15f);
            yinSheQiang.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 25f);
            yinSheQiang.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            yinSheQiang.equipment_value = 1100;
            yinSheQiang.quality = Rarity.R3_Legendary;
            yinSheQiang.equipment_type = EquipmentType.Weapon;
            yinSheQiang.name_class = "item_class_weapon";
            yinSheQiang.path_slash_animation = "effects/slashes/slash_sword";
            yinSheQiang.path_icon = $"{PathIcon}/icon_yin_she_qiang";
            yinSheQiang.path_gameplay_sprite = $"weapons/yin_she_qiang";
            yinSheQiang.gameplay_sprites = getWeaponSprites("yin_she_qiang");
            AssetManager.items.list.AddItem(yinSheQiang);
            addToLocale(yinSheQiang.id, yinSheQiang.translation_key, "岭南毒蛇山特产寒铁所铸，枪身如银蛇灵动！");

            // 4. 赤练剑
            ItemAsset chiLianJian = AssetManager.items.clone("chi_lian_jian", "$weapon");
            chiLianJian.id = "chi_lian_jian";
            chiLianJian.material = "adamantine";
            chiLianJian.translation_key = "chi_lian_jian";
            chiLianJian.equipment_subtype = "chi_lian_jian";
            chiLianJian.group_id = "sword";
            chiLianJian.animated = false;
            chiLianJian.is_pool_weapon = false;
            chiLianJian.unlock(true);
            chiLianJian.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            chiLianJian.base_stats = new();
            chiLianJian.base_stats.set(CustomBaseStatsConstant.Damage, 1600f);
            chiLianJian.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 12f);
            chiLianJian.base_stats.set(CustomBaseStatsConstant.MultiplierMana, 0.2f);
            chiLianJian.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.2f);
            chiLianJian.base_stats.set(CustomBaseStatsConstant.CriticalDamageMultiplier, 0.4f);
            chiLianJian.base_stats.set(CustomBaseStatsConstant.Speed, 15f);
            chiLianJian.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            chiLianJian.equipment_value = 1300;
            chiLianJian.quality = Rarity.R3_Legendary;
            chiLianJian.equipment_type = EquipmentType.Weapon;
            chiLianJian.name_class = "item_class_weapon";
            chiLianJian.path_slash_animation = "effects/slashes/slash_sword";
            chiLianJian.path_icon = $"{PathIcon}/icon_chi_lian_jian";
            chiLianJian.path_gameplay_sprite = $"weapons/chi_lian_jian";
            chiLianJian.gameplay_sprites = getWeaponSprites("chi_lian_jian");
            AssetManager.items.list.AddItem(chiLianJian);
            addToLocale(chiLianJian.id, chiLianJian.translation_key, "火焰山火铜锻造，剑身上有赤练纹路！");

            // 5. 玄冰刀
            ItemAsset xuanBingDao = AssetManager.items.clone("xuan_bing_dao", "$weapon");
            xuanBingDao.id = "xuan_bing_dao";
            xuanBingDao.material = "adamantine";
            xuanBingDao.translation_key = "xuan_bing_dao";
            xuanBingDao.equipment_subtype = "xuan_bing_dao";
            xuanBingDao.group_id = "sword";
            xuanBingDao.animated = false;
            xuanBingDao.is_pool_weapon = false;
            xuanBingDao.unlock(true);
            xuanBingDao.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            xuanBingDao.base_stats = new();
            xuanBingDao.base_stats.set(CustomBaseStatsConstant.Damage, 1000f);
            xuanBingDao.base_stats.set(CustomBaseStatsConstant.Speed, 6f);
            xuanBingDao.base_stats.set(CustomBaseStatsConstant.Stamina, 500f);
            xuanBingDao.base_stats.set(CustomBaseStatsConstant.Armor, 15f);
            xuanBingDao.base_stats.set(CustomBaseStatsConstant.MultiplierHealth, 0.25f);
            xuanBingDao.base_stats.set(CustomBaseStatsConstant.Knockback, 0.75f);
            xuanBingDao.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            xuanBingDao.equipment_value = 1400;
            xuanBingDao.quality = Rarity.R3_Legendary;
            xuanBingDao.equipment_type = EquipmentType.Weapon;
            xuanBingDao.name_class = "item_class_weapon";
            xuanBingDao.path_slash_animation = "effects/slashes/xuan_bing_dao";
            xuanBingDao.path_icon = $"{PathIcon}/icon_xuan_bing_dao";
            xuanBingDao.path_gameplay_sprite = $"weapons/xuan_bing_dao";
            xuanBingDao.gameplay_sprites = getWeaponSprites("xuan_bing_dao");
            AssetManager.items.list.AddItem(xuanBingDao);
            addToLocale(xuanBingDao.id, xuanBingDao.translation_key, "极北之地玄冰寒铁所铸，刀身散发刺骨寒气！");

            // 6. 金龙枪
            ItemAsset jinLongQiang = AssetManager.items.clone("jin_long_qiang", "$weapon");
            jinLongQiang.id = "jin_long_qiang";
            jinLongQiang.material = "adamantine";
            jinLongQiang.translation_key = "jin_long_qiang";
            jinLongQiang.equipment_subtype = "jin_long_qiang";
            jinLongQiang.group_id = "sword";
            jinLongQiang.animated = false;
            jinLongQiang.is_pool_weapon = false;
            jinLongQiang.unlock(true);
            jinLongQiang.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            jinLongQiang.base_stats = new();
            jinLongQiang.base_stats.set(CustomBaseStatsConstant.Damage, 2100f);
            jinLongQiang.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 8f);
            jinLongQiang.base_stats.set(CustomBaseStatsConstant.Lifespan, 50f);
            jinLongQiang.base_stats.set(CustomBaseStatsConstant.Health, 4000f);
            jinLongQiang.base_stats.set(CustomBaseStatsConstant.MultiplierMana, 0.4f);
            jinLongQiang.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.15f);
            jinLongQiang.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            jinLongQiang.equipment_value = 1500;
            jinLongQiang.quality = Rarity.R3_Legendary;
            jinLongQiang.equipment_type = EquipmentType.Weapon;
            jinLongQiang.name_class = "item_class_weapon";
            jinLongQiang.path_slash_animation = "effects/slashes/slash_sword";
            jinLongQiang.path_icon = $"{PathIcon}/icon_jin_long_qiang";
            jinLongQiang.path_gameplay_sprite = $"weapons/jin_long_qiang";
            jinLongQiang.gameplay_sprites = getWeaponSprites("jin_long_qiang");
            AssetManager.items.list.AddItem(jinLongQiang);
            addToLocale(jinLongQiang.id, jinLongQiang.translation_key, "东海龙族栖息地所产，枪头雕有金龙图案！");

            // 7. 紫竹杖
            ItemAsset ziZhuZhang = AssetManager.items.clone("zi_zhu_zhang", "$weapon");
            ziZhuZhang.id = "zi_zhu_zhang";
            ziZhuZhang.material = "adamantine";
            ziZhuZhang.translation_key = "zi_zhu_zhang";
            ziZhuZhang.equipment_subtype = "zi_zhu_zhang";
            ziZhuZhang.group_id = "sword";
            ziZhuZhang.animated = false;
            ziZhuZhang.is_pool_weapon = false;
            ziZhuZhang.unlock(true);
            ziZhuZhang.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            ziZhuZhang.base_stats = new();
            ziZhuZhang.base_stats.set(CustomBaseStatsConstant.Damage, 1400f);
            ziZhuZhang.base_stats.set(CustomBaseStatsConstant.Health, 1500f);
            ziZhuZhang.base_stats.set(CustomBaseStatsConstant.MultiplierHealth, 0.15f);
            ziZhuZhang.base_stats.set(CustomBaseStatsConstant.Stamina, 3000f);
            ziZhuZhang.base_stats.set(CustomBaseStatsConstant.Speed, 200f);
            ziZhuZhang.base_stats.set(CustomBaseStatsConstant.Lifespan, 40f);
            ziZhuZhang.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            ziZhuZhang.equipment_value = 900;
            ziZhuZhang.quality = Rarity.R3_Legendary;
            ziZhuZhang.equipment_type = EquipmentType.Weapon;
            ziZhuZhang.name_class = "item_class_weapon";
            ziZhuZhang.path_slash_animation = "effects/slashes/slash_sword";
            ziZhuZhang.path_icon = $"{PathIcon}/icon_zi_zhu_zhang";
            ziZhuZhang.path_gameplay_sprite = $"weapons/zi_zhu_zhang";
            ziZhuZhang.gameplay_sprites = getWeaponSprites("zi_zhu_zhang");
            AssetManager.items.list.AddItem(ziZhuZhang);
            addToLocale(ziZhuZhang.id, ziZhuZhang.translation_key, "南海紫竹林千年紫竹所制，韧性十足！");

            // 8. 青钢剑
            ItemAsset qingGangJian = AssetManager.items.clone("qing_gang_jian", "$weapon");
            qingGangJian.id = "qing_gang_jian";
            qingGangJian.material = "adamantine";
            qingGangJian.translation_key = "qing_gang_jian";
            qingGangJian.equipment_subtype = "qing_gang_jian";
            qingGangJian.group_id = "sword";
            qingGangJian.animated = false;
            qingGangJian.is_pool_weapon = false;
            qingGangJian.unlock(true);
            qingGangJian.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            qingGangJian.base_stats = new();
            qingGangJian.base_stats.set(CustomBaseStatsConstant.Damage, 1700f);
            qingGangJian.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 11f);
            qingGangJian.base_stats.set(CustomBaseStatsConstant.Speed, 400f);
            qingGangJian.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.25f);
            qingGangJian.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.15f);
            qingGangJian.base_stats.set(CustomBaseStatsConstant.Stamina, 1750f);
            qingGangJian.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            qingGangJian.equipment_value = 1200;
            qingGangJian.quality = Rarity.R3_Legendary;
            qingGangJian.equipment_type = EquipmentType.Weapon;
            qingGangJian.name_class = "item_class_weapon";
            qingGangJian.path_slash_animation = "effects/slashes/slash_sword";
            qingGangJian.path_icon = $"{PathIcon}/icon_qing_gang_jian";
            qingGangJian.path_gameplay_sprite = $"weapons/qing_gang_jian";
            qingGangJian.gameplay_sprites = getWeaponSprites("qing_gang_jian");
            AssetManager.items.list.AddItem(qingGangJian);
            addToLocale(qingGangJian.id, qingGangJian.translation_key, "西域青钢石提炼而成，锋利无比！");

            // 9. 铁骨扇
            ItemAsset tieGuShan = AssetManager.items.clone("tie_gu_shan", "$weapon");
            tieGuShan.id = "tie_gu_shan";
            tieGuShan.material = "adamantine";
            tieGuShan.translation_key = "tie_gu_shan";
            tieGuShan.equipment_subtype = "tie_gu_shan";
            tieGuShan.group_id = "sword";
            tieGuShan.animated = false;
            tieGuShan.is_pool_weapon = false;
            tieGuShan.unlock(true);
            tieGuShan.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            tieGuShan.base_stats = new();
            tieGuShan.base_stats.set(CustomBaseStatsConstant.Damage, 1600f);
            tieGuShan.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 15f);
            tieGuShan.base_stats.set(CustomBaseStatsConstant.MultiplierAttackSpeed, 0.1f);
            tieGuShan.base_stats.set(CustomBaseStatsConstant.Speed, 300f);
            tieGuShan.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.2f);
            tieGuShan.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.2f);
            tieGuShan.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            tieGuShan.equipment_value = 1500;
            tieGuShan.quality = Rarity.R3_Legendary;
            tieGuShan.equipment_type = EquipmentType.Weapon;
            tieGuShan.name_class = "item_class_weapon";
            tieGuShan.path_slash_animation = "effects/slashes/slash_sword";
            tieGuShan.path_icon = $"{PathIcon}/icon_tie_gu_shan";
            tieGuShan.path_gameplay_sprite = $"weapons/tie_gu_shan";
            tieGuShan.gameplay_sprites = getWeaponSprites("tie_gu_shan");
            AssetManager.items.list.AddItem(tieGuShan);
            addToLocale(tieGuShan.id, tieGuShan.translation_key, "北方铁骨木制作扇骨，轻巧锋利！");

            // 10. 断云刀
            ItemAsset duanYunDao = AssetManager.items.clone("duan_yun_dao", "$weapon");
            duanYunDao.id = "duan_yun_dao";
            duanYunDao.material = "adamantine";
            duanYunDao.translation_key = "duan_yun_dao";
            duanYunDao.equipment_subtype = "duan_yun_dao";
            duanYunDao.group_id = "sword";
            duanYunDao.animated = false;
            duanYunDao.is_pool_weapon = false;
            duanYunDao.unlock(true);
            duanYunDao.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            duanYunDao.base_stats = new();
            duanYunDao.base_stats.set(CustomBaseStatsConstant.Damage, 2200f);
            duanYunDao.base_stats.set(CustomBaseStatsConstant.Knockback, 0.3f);
            duanYunDao.base_stats.set(CustomBaseStatsConstant.MultiplierDamage, 0.2f);
            duanYunDao.base_stats.set(CustomBaseStatsConstant.Armor, 20f);
            duanYunDao.base_stats.set(CustomBaseStatsConstant.Speed, 250f);
            duanYunDao.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 30f);
            duanYunDao.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            duanYunDao.equipment_value = 1600;
            duanYunDao.quality = Rarity.R3_Legendary;
            duanYunDao.equipment_type = EquipmentType.Weapon;
            duanYunDao.name_class = "item_class_weapon";
            duanYunDao.path_slash_animation = "effects/slashes/slash_sword";
            duanYunDao.path_icon = $"{PathIcon}/icon_duan_yun_dao";
            duanYunDao.path_gameplay_sprite = $"weapons/duan_yun_dao";
            duanYunDao.gameplay_sprites = getWeaponSprites("duan_yun_dao");
            AssetManager.items.list.AddItem(duanYunDao);
            addToLocale(duanYunDao.id, duanYunDao.translation_key, "西蜀断云峡特产精铁打造，刀气可断云！");

            // 11. 穿云枪
            ItemAsset chuanYunQiang = AssetManager.items.clone("chuan_yun_qiang", "$weapon");
            chuanYunQiang.id = "chuan_yun_qiang";
            chuanYunQiang.material = "adamantine";
            chuanYunQiang.translation_key = "chuan_yun_qiang";
            chuanYunQiang.equipment_subtype = "chuan_yun_qiang";
            chuanYunQiang.group_id = "sword";
            chuanYunQiang.animated = false;
            chuanYunQiang.is_pool_weapon = false;
            chuanYunQiang.unlock(true);
            chuanYunQiang.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            chuanYunQiang.base_stats = new();
            chuanYunQiang.base_stats.set(CustomBaseStatsConstant.Damage, 2000f);
            chuanYunQiang.base_stats.set(CustomBaseStatsConstant.Speed, 70f);
            chuanYunQiang.base_stats.set(CustomBaseStatsConstant.ConstructionSpeed, 20f);
            chuanYunQiang.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 40f);
            chuanYunQiang.base_stats.set(CustomBaseStatsConstant.Knockback, 0.75f);
            chuanYunQiang.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.15f);
            chuanYunQiang.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            chuanYunQiang.equipment_value = 1400;
            chuanYunQiang.quality = Rarity.R3_Legendary;
            chuanYunQiang.equipment_type = EquipmentType.Weapon;
            chuanYunQiang.name_class = "item_class_weapon";
            chuanYunQiang.path_slash_animation = "effects/slashes/slash_sword";
            chuanYunQiang.path_icon = $"{PathIcon}/icon_chuan_yun_qiang";
            chuanYunQiang.path_gameplay_sprite = $"weapons/chuan_yun_qiang";
            chuanYunQiang.gameplay_sprites = getWeaponSprites("chuan_yun_qiang");
            AssetManager.items.list.AddItem(chuanYunQiang);
            addToLocale(chuanYunQiang.id, chuanYunQiang.translation_key, "北地穿云岭玄铁锻造，枪出如龙穿云！");

            // 12. 玄铁重剑
            ItemAsset xuanTieZhongJian = AssetManager.items.clone("xuan_tie_zhong_jian", "$weapon");
            xuanTieZhongJian.id = "xuan_tie_zhong_jian";
            xuanTieZhongJian.material = "adamantine";
            xuanTieZhongJian.translation_key = "xuan_tie_zhong_jian";
            xuanTieZhongJian.equipment_subtype = "xuan_tie_zhong_jian";
            xuanTieZhongJian.group_id = "sword";
            xuanTieZhongJian.animated = false;
            xuanTieZhongJian.is_pool_weapon = false;
            xuanTieZhongJian.unlock(true);
            xuanTieZhongJian.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            xuanTieZhongJian.base_stats = new();
            xuanTieZhongJian.base_stats.set(CustomBaseStatsConstant.Damage, 2500f);
            xuanTieZhongJian.base_stats.set(CustomBaseStatsConstant.Armor, 8f);
            xuanTieZhongJian.base_stats.set(CustomBaseStatsConstant.Health, 2000f);
            xuanTieZhongJian.base_stats.set(CustomBaseStatsConstant.Stamina, 3500f);
            xuanTieZhongJian.base_stats.set(CustomBaseStatsConstant.Knockback, 1.25f);
            xuanTieZhongJian.base_stats.set(CustomBaseStatsConstant.MultiplierHealth, 0.5f);
            xuanTieZhongJian.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            xuanTieZhongJian.equipment_value = 1700;
            xuanTieZhongJian.quality = Rarity.R3_Legendary;
            xuanTieZhongJian.equipment_type = EquipmentType.Weapon;
            xuanTieZhongJian.name_class = "item_class_weapon";
            xuanTieZhongJian.path_slash_animation = "effects/slashes/slash_sword";
            xuanTieZhongJian.path_icon = $"{PathIcon}/icon_xuan_tie_zhong_jian";
            xuanTieZhongJian.path_gameplay_sprite = $"weapons/xuan_tie_zhong_jian";
            xuanTieZhongJian.gameplay_sprites = getWeaponSprites("xuan_tie_zhong_jian");
            AssetManager.items.list.AddItem(xuanTieZhongJian);
            addToLocale(xuanTieZhongJian.id, xuanTieZhongJian.translation_key, "昆仑山脉玄铁打造，重剑无锋大巧不工！");

            // 13. 追风刀
            ItemAsset zhuiFengDao = AssetManager.items.clone("zhui_feng_dao", "$weapon");
            zhuiFengDao.id = "zhui_feng_dao";
            zhuiFengDao.material = "adamantine";
            zhuiFengDao.translation_key = "zhui_feng_dao";
            zhuiFengDao.equipment_subtype = "zhui_feng_dao";
            zhuiFengDao.group_id = "sword";
            zhuiFengDao.animated = false;
            zhuiFengDao.is_pool_weapon = false;
            zhuiFengDao.unlock(true);
            zhuiFengDao.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            zhuiFengDao.base_stats = new();
            zhuiFengDao.base_stats.set(CustomBaseStatsConstant.Damage, 1700f);
            zhuiFengDao.base_stats.set(CustomBaseStatsConstant.Speed, 100f);
            zhuiFengDao.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 13f);
            zhuiFengDao.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.25f);
            zhuiFengDao.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.2f);
            zhuiFengDao.base_stats.set(CustomBaseStatsConstant.Stamina, 2000f);
            zhuiFengDao.equipment_value = 1500;
            zhuiFengDao.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            zhuiFengDao.quality = Rarity.R3_Legendary;
            zhuiFengDao.equipment_type = EquipmentType.Weapon;
            zhuiFengDao.name_class = "item_class_weapon";
            zhuiFengDao.path_slash_animation = "effects/slashes/slash_sword";
            zhuiFengDao.path_icon = $"{PathIcon}/icon_zhui_feng_dao";
            zhuiFengDao.path_gameplay_sprite = $"weapons/zhui_feng_dao";
            zhuiFengDao.gameplay_sprites = getWeaponSprites("zhui_feng_dao");
            AssetManager.items.list.AddItem(zhuiFengDao);
            addToLocale(zhuiFengDao.id, zhuiFengDao.translation_key, "东方青丘山风属性材料所制，快如疾风！");

            // 14. 伏虎棍
            ItemAsset fuHuGun = AssetManager.items.clone("fu_hu_gun", "$weapon");
            fuHuGun.id = "fu_hu_gun";
            fuHuGun.material = "adamantine";
            fuHuGun.translation_key = "fu_hu_gun";
            fuHuGun.equipment_subtype = "fu_hu_gun";
            fuHuGun.group_id = "sword";
            fuHuGun.animated = false;
            fuHuGun.is_pool_weapon = false;
            fuHuGun.unlock(true);
            fuHuGun.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            fuHuGun.base_stats = new();
            fuHuGun.base_stats.set(CustomBaseStatsConstant.Damage, 1900f);
            fuHuGun.base_stats.set(CustomBaseStatsConstant.Health, 1800f);
            fuHuGun.base_stats.set(CustomBaseStatsConstant.Stamina, 700f);
            fuHuGun.base_stats.set(CustomBaseStatsConstant.Armor, 25f);
            fuHuGun.base_stats.set(CustomBaseStatsConstant.Knockback, 1.0f);
            fuHuGun.base_stats.set(CustomBaseStatsConstant.Lifespan, 75f);
            fuHuGun.equipment_value = 1600;
            fuHuGun.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            fuHuGun.quality = Rarity.R3_Legendary;
            fuHuGun.equipment_type = EquipmentType.Weapon;
            fuHuGun.name_class = "item_class_weapon";
            fuHuGun.path_slash_animation = "effects/slashes/slash_sword";
            fuHuGun.path_icon = $"{PathIcon}/icon_fu_hu_gun";
            fuHuGun.path_gameplay_sprite = $"weapons/fu_hu_gun";
            fuHuGun.gameplay_sprites = getWeaponSprites("fu_hu_gun");
            AssetManager.items.list.AddItem(fuHuGun);
            addToLocale(fuHuGun.id, fuHuGun.translation_key, "南岳衡山百年硬木所制，棍身刻伏虎图案！");
            #endregion

            #region 新增武器15-22
            // 15. 青霜剑
            ItemAsset qingShuangJian = AssetManager.items.clone("qing_shuang_jian", "$weapon");
            qingShuangJian.id = "qing_shuang_jian";
            qingShuangJian.material = "adamantine";
            qingShuangJian.translation_key = "qing_shuang_jian";
            qingShuangJian.equipment_subtype = "qing_shuang_jian";
            qingShuangJian.group_id = "sword";
            qingShuangJian.animated = false;
            qingShuangJian.is_pool_weapon = false;
            qingShuangJian.unlock(true);
            qingShuangJian.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            qingShuangJian.base_stats = new();
            qingShuangJian.base_stats.set(CustomBaseStatsConstant.Damage, 1800f);
            qingShuangJian.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 12f);
            qingShuangJian.base_stats.set(CustomBaseStatsConstant.Speed, 60f);
            qingShuangJian.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.2f);
            qingShuangJian.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.15f);
            qingShuangJian.base_stats.set(CustomBaseStatsConstant.MultiplierMana, 0.6f);
            qingShuangJian.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            qingShuangJian.equipment_value = 1400;
            qingShuangJian.quality = Rarity.R3_Legendary;
            qingShuangJian.equipment_type = EquipmentType.Weapon;
            qingShuangJian.name_class = "item_class_weapon";
            qingShuangJian.path_slash_animation = "effects/slashes/slash_sword";
            qingShuangJian.path_icon = $"{PathIcon}/icon_qing_shuang_jian";
            qingShuangJian.path_gameplay_sprite = $"weapons/qing_shuang_jian";
            qingShuangJian.gameplay_sprites = getWeaponSprites("qing_shuang_jian");
            AssetManager.items.list.AddItem(qingShuangJian);
            addToLocale(qingShuangJian.id, qingShuangJian.translation_key, "极北冰原青霜寒铁所铸，剑身上凝有永恒霜华！");

            // 16. 赤焰刀
            ItemAsset chiYanDao = AssetManager.items.clone("chi_yan_dao", "$weapon");
            chiYanDao.id = "chi_yan_dao";
            chiYanDao.material = "adamantine";
            chiYanDao.translation_key = "chi_yan_dao";
            chiYanDao.equipment_subtype = "chi_yan_dao";
            chiYanDao.group_id = "sword";
            chiYanDao.animated = false;
            chiYanDao.is_pool_weapon = false;
            chiYanDao.unlock(true);
            chiYanDao.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            chiYanDao.base_stats = new();
            chiYanDao.base_stats.set(CustomBaseStatsConstant.Damage, 2100f);
            chiYanDao.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 9f);
            chiYanDao.base_stats.set(CustomBaseStatsConstant.MultiplierDamage, 0.3f);
            chiYanDao.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.25f);
            chiYanDao.base_stats.set(CustomBaseStatsConstant.CriticalDamageMultiplier, 0.6f);
            chiYanDao.base_stats.set(CustomBaseStatsConstant.Speed, 20f);
            chiYanDao.equipment_value = 1600;
            chiYanDao.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            chiYanDao.quality = Rarity.R3_Legendary;
            chiYanDao.equipment_type = EquipmentType.Weapon;
            chiYanDao.name_class = "item_class_weapon";
            chiYanDao.path_slash_animation = "effects/slashes/slash_sword";
            chiYanDao.path_icon = $"{PathIcon}/icon_chi_yan_dao";
            chiYanDao.path_gameplay_sprite = $"weapons/chi_yan_dao";
            chiYanDao.gameplay_sprites = getWeaponSprites("chi_yan_dao");
            AssetManager.items.list.AddItem(chiYanDao);
            addToLocale(chiYanDao.id, chiYanDao.translation_key, "火山熔岩中赤焰精钢打造，刀身燃烧不灭圣火！");

            // 17. 玄影枪
            ItemAsset xuanYingQiang = AssetManager.items.clone("xuan_ying_qiang", "$weapon");
            xuanYingQiang.id = "xuan_ying_qiang";
            xuanYingQiang.material = "adamantine";
            xuanYingQiang.translation_key = "xuan_ying_qiang";
            xuanYingQiang.equipment_subtype = "xuan_ying_qiang";
            xuanYingQiang.group_id = "sword";
            xuanYingQiang.animated = false;
            xuanYingQiang.is_pool_weapon = false;
            xuanYingQiang.unlock(true);
            xuanYingQiang.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            xuanYingQiang.base_stats = new();
            xuanYingQiang.base_stats.set(CustomBaseStatsConstant.Damage, 1900f);
            xuanYingQiang.base_stats.set(CustomBaseStatsConstant.Speed, 80f);
            xuanYingQiang.base_stats.set(CustomBaseStatsConstant.Knockback, 0.25f);
            xuanYingQiang.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.2f);
            xuanYingQiang.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.15f);
            xuanYingQiang.base_stats.set(CustomBaseStatsConstant.Stamina, 225f);
            xuanYingQiang.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            xuanYingQiang.equipment_value = 1500;
            xuanYingQiang.quality = Rarity.R3_Legendary;
            xuanYingQiang.equipment_type = EquipmentType.Weapon;
            xuanYingQiang.name_class = "item_class_weapon";
            xuanYingQiang.path_slash_animation = "effects/slashes/slash_sword";
            xuanYingQiang.path_icon = $"{PathIcon}/icon_xuan_ying_qiang";
            xuanYingQiang.path_gameplay_sprite = $"weapons/xuan_ying_qiang";
            xuanYingQiang.gameplay_sprites = getWeaponSprites("xuan_ying_qiang");
            AssetManager.items.list.AddItem(xuanYingQiang);
            addToLocale(xuanYingQiang.id, xuanYingQiang.translation_key, "暗影森林玄铁矿石锻造，枪身可隐入阴影！");

            // 18. 金鳞剑
            ItemAsset jinLinJian = AssetManager.items.clone("jin_lin_jian", "$weapon");
            jinLinJian.id = "jin_lin_jian";
            jinLinJian.material = "adamantine";
            jinLinJian.translation_key = "jin_lin_jian";
            jinLinJian.equipment_subtype = "jin_lin_jian";
            jinLinJian.group_id = "sword";
            jinLinJian.animated = false;
            jinLinJian.is_pool_weapon = false;
            jinLinJian.unlock(true);
            jinLinJian.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            jinLinJian.base_stats = new();
            jinLinJian.base_stats.set(CustomBaseStatsConstant.Damage, 1700f);
            jinLinJian.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 13f);
            jinLinJian.base_stats.set(CustomBaseStatsConstant.Lifespan, 15f);
            jinLinJian.base_stats.set(CustomBaseStatsConstant.Health, 3000f);
            jinLinJian.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.2f);
            jinLinJian.base_stats.set(CustomBaseStatsConstant.MultiplierHealth, 0.2f);
            jinLinJian.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            jinLinJian.equipment_value = 1300;
            jinLinJian.quality = Rarity.R3_Legendary;
            jinLinJian.equipment_type = EquipmentType.Weapon;
            jinLinJian.name_class = "item_class_weapon";
            jinLinJian.path_slash_animation = "effects/slashes/slash_sword";
            jinLinJian.path_icon = $"{PathIcon}/icon_jin_lin_jian";
            jinLinJian.path_gameplay_sprite = $"weapons/jin_lin_jian";
            jinLinJian.gameplay_sprites = getWeaponSprites("jin_lin_jian");
            AssetManager.items.list.AddItem(jinLinJian);
            addToLocale(jinLinJian.id, jinLinJian.translation_key, "东海金鳞鱼鳞片融入精钢，剑身金光流转！");

            // 19. 银月刀
            ItemAsset yinYueDao = AssetManager.items.clone("yin_yue_dao", "$weapon");
            yinYueDao.id = "yin_yue_dao";
            yinYueDao.material = "adamantine";
            yinYueDao.translation_key = "yin_yue_dao";
            yinYueDao.equipment_subtype = "yin_yue_dao";
            yinYueDao.group_id = "sword";
            yinYueDao.animated = false;
            yinYueDao.is_pool_weapon = false;
            yinYueDao.unlock(true);
            yinYueDao.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            yinYueDao.base_stats = new();
            yinYueDao.base_stats.set(CustomBaseStatsConstant.Damage, 2000f);
            yinYueDao.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 11f);
            yinYueDao.base_stats.set(CustomBaseStatsConstant.Speed, 70f);
            yinYueDao.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.2f);
            yinYueDao.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.15f);
            yinYueDao.base_stats.set(CustomBaseStatsConstant.Stamina, 2500f);
            yinYueDao.equipment_value = 1400;
            yinYueDao.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            yinYueDao.quality = Rarity.R3_Legendary;
            yinYueDao.equipment_type = EquipmentType.Weapon;
            yinYueDao.name_class = "item_class_weapon";
            yinYueDao.path_slash_animation = "effects/slashes/slash_sword";
            yinYueDao.path_icon = $"{PathIcon}/icon_yin_yue_dao";
            yinYueDao.path_gameplay_sprite = $"weapons/yin_yue_dao";
            yinYueDao.gameplay_sprites = getWeaponSprites("yin_yue_dao");
            AssetManager.items.list.AddItem(yinYueDao);
            addToLocale(yinYueDao.id, yinYueDao.translation_key, "月光谷银月石提炼而成，刀身映月生辉！");

            // 20. 紫电枪
            ItemAsset ziDianQiang = AssetManager.items.clone("zi_dian_qiang", "$weapon");
            ziDianQiang.id = "zi_dian_qiang";
            ziDianQiang.material = "adamantine";
            ziDianQiang.translation_key = "zi_dian_qiang";
            ziDianQiang.equipment_subtype = "zi_dian_qiang";
            ziDianQiang.group_id = "sword";
            ziDianQiang.animated = false;
            ziDianQiang.is_pool_weapon = false;
            ziDianQiang.unlock(true);
            ziDianQiang.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            ziDianQiang.base_stats = new();
            ziDianQiang.base_stats.set(CustomBaseStatsConstant.Damage, 2200f);
            ziDianQiang.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 10f);
            ziDianQiang.base_stats.set(CustomBaseStatsConstant.MultiplierAttackSpeed, 0.1f);
            ziDianQiang.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.25f);
            ziDianQiang.base_stats.set(CustomBaseStatsConstant.CriticalDamageMultiplier, 0.4f);
            ziDianQiang.base_stats.set(CustomBaseStatsConstant.Knockback, 1.0f);
            ziDianQiang.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            ziDianQiang.equipment_value = 1700;
            ziDianQiang.quality = Rarity.R3_Legendary;
            ziDianQiang.equipment_type = EquipmentType.Weapon;
            ziDianQiang.name_class = "item_class_weapon";
            ziDianQiang.path_slash_animation = "effects/slashes/slash_sword";
            ziDianQiang.path_icon = $"{PathIcon}/icon_zi_dian_qiang";
            ziDianQiang.path_gameplay_sprite = $"weapons/zi_dian_qiang";
            ziDianQiang.gameplay_sprites = getWeaponSprites("zi_dian_qiang");
            AssetManager.items.list.AddItem(ziDianQiang);
            addToLocale(ziDianQiang.id, ziDianQiang.translation_key, "雷泽紫电石融入寒铁，枪身缠绕紫色电光！");

            // 21. 青竹剑
            ItemAsset qingZhuJian = AssetManager.items.clone("qing_zhu_jian", "$weapon");
            qingZhuJian.id = "qing_zhu_jian";
            qingZhuJian.material = "adamantine";
            qingZhuJian.translation_key = "qing_zhu_jian";
            qingZhuJian.equipment_subtype = "qing_zhu_jian";
            qingZhuJian.group_id = "sword";
            qingZhuJian.animated = false;
            qingZhuJian.is_pool_weapon = false;
            qingZhuJian.unlock(true);
            qingZhuJian.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            qingZhuJian.base_stats = new();
            qingZhuJian.base_stats.set(CustomBaseStatsConstant.Damage, 1500f);
            qingZhuJian.base_stats.set(CustomBaseStatsConstant.Health, 1200f);
            qingZhuJian.base_stats.set(CustomBaseStatsConstant.Stamina, 600f);
            qingZhuJian.base_stats.set(CustomBaseStatsConstant.Speed, 25f);
            qingZhuJian.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 40f);
            qingZhuJian.base_stats.set(CustomBaseStatsConstant.Lifespan, 30f);
            qingZhuJian.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            qingZhuJian.equipment_value = 1100;
            qingZhuJian.quality = Rarity.R3_Legendary;
            qingZhuJian.equipment_type = EquipmentType.Weapon;
            qingZhuJian.name_class = "item_class_weapon";
            qingZhuJian.path_slash_animation = "effects/slashes/slash_sword";
            qingZhuJian.path_icon = $"{PathIcon}/icon_qing_zhu_jian";
            qingZhuJian.path_gameplay_sprite = $"weapons/qing_zhu_jian";
            qingZhuJian.gameplay_sprites = getWeaponSprites("qing_zhu_jian");
            AssetManager.items.list.AddItem(qingZhuJian);
            addToLocale(qingZhuJian.id, qingZhuJian.translation_key, "青竹峰千年青竹制成，剑出带有竹香！");

            // 22. 赤血刀
            ItemAsset chiXueDao = AssetManager.items.clone("chi_xue_dao", "$weapon");
            chiXueDao.id = "chi_xue_dao";
            chiXueDao.material = "adamantine";
            chiXueDao.translation_key = "chi_xue_dao";
            chiXueDao.equipment_subtype = "chi_xue_dao";
            chiXueDao.group_id = "sword";
            chiXueDao.animated = false;
            chiXueDao.is_pool_weapon = false;
            chiXueDao.unlock(true);
            chiXueDao.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            chiXueDao.base_stats = new();
            chiXueDao.base_stats.set(CustomBaseStatsConstant.Damage, 2300f);
            chiXueDao.base_stats.set(CustomBaseStatsConstant.Knockback, 0.35f);
            chiXueDao.base_stats.set(CustomBaseStatsConstant.MultiplierHealth, 0.1f);
            chiXueDao.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.3f);
            chiXueDao.base_stats.set(CustomBaseStatsConstant.CriticalDamageMultiplier, 0.6f);
            chiXueDao.base_stats.set(CustomBaseStatsConstant.Armor, 20f);
            chiXueDao.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            chiXueDao.equipment_value = 1800;
            chiXueDao.quality = Rarity.R3_Legendary;
            chiXueDao.equipment_type = EquipmentType.Weapon;
            chiXueDao.name_class = "item_class_weapon";
            chiXueDao.path_slash_animation = "effects/slashes/slash_sword";
            chiXueDao.path_icon = $"{PathIcon}/icon_chi_xue_dao";
            chiXueDao.path_gameplay_sprite = $"weapons/chi_xue_dao";
            chiXueDao.gameplay_sprites = getWeaponSprites("chi_xue_dao");
            AssetManager.items.list.AddItem(chiXueDao);
            addToLocale(chiXueDao.id, chiXueDao.translation_key, "血池之地寒铁锻造，刀身饮血变红！");
            #endregion

            #region 裂地刀
            ItemAsset lieDiDao = AssetManager.items.clone("lie_di_dao", "$weapon");
            lieDiDao.id = "lie_di_dao";
            lieDiDao.material = "adamantine";
            lieDiDao.translation_key = "lie_di_dao";
            lieDiDao.equipment_subtype = "lie_di_dao";
            lieDiDao.group_id = "sword";
            lieDiDao.animated = false;
            lieDiDao.is_pool_weapon = false;
            lieDiDao.unlock(true);
            lieDiDao.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            lieDiDao.base_stats = new();
            lieDiDao.base_stats.set(CustomBaseStatsConstant.Damage, 900f);
            lieDiDao.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 15f);
            lieDiDao.base_stats.set(CustomBaseStatsConstant.Speed, 10f);
            lieDiDao.base_stats.set(CustomBaseStatsConstant.MultiplierMana, 0.5f);
            lieDiDao.base_stats.set(CustomBaseStatsConstant.Knockback, 0.5f);
            lieDiDao.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.75f);
            lieDiDao.base_stats.set(CustomBaseStatsConstant.CriticalDamageMultiplier, 1.2f);
            lieDiDao.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.5f);
            lieDiDao.base_stats.set(CustomBaseStatsConstant.Armor, 50f);
            lieDiDao.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            lieDiDao.equipment_value = 4000;
            lieDiDao.special_effect_interval = 0.4f;
            lieDiDao.quality = Rarity.R3_Legendary;
            lieDiDao.equipment_type = EquipmentType.Weapon;
            lieDiDao.name_class = "item_class_weapon";

            lieDiDao.path_slash_animation = "effects/slashes/slash_sword";
            lieDiDao.path_icon = $"{PathIcon}/icon_lie_di_dao";
            lieDiDao.path_gameplay_sprite = $"weapons/lie_di_dao"; //使用现有图片资源
            lieDiDao.gameplay_sprites = getWeaponSprites("lie_di_dao"); //复用现有图片资源

            lieDiDao.action_attack_target = new AttackAction(CustomItemActions.lieDiDaoAttackEffect);        //特殊攻击效果
            AssetManager.items.list.AddItem(lieDiDao);
            addToLocale(lieDiDao.id, lieDiDao.translation_key, "不周山底神铁锻造，一刀可裂大地山川！");
            #endregion

            #region 吞海枪
            ItemAsset tunHaiQiang = AssetManager.items.clone("tun_hai_qiang", "$weapon");
            tunHaiQiang.id = "tun_hai_qiang";
            tunHaiQiang.material = "adamantine";
            tunHaiQiang.translation_key = "tun_hai_qiang";
            tunHaiQiang.equipment_subtype = "tun_hai_qiang";
            tunHaiQiang.group_id = "sword";
            tunHaiQiang.animated = false;
            tunHaiQiang.is_pool_weapon = false;
            tunHaiQiang.unlock(true);
            tunHaiQiang.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            tunHaiQiang.base_stats = new();
            tunHaiQiang.base_stats.set(CustomBaseStatsConstant.Damage, 5000f);
            tunHaiQiang.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 10f);
            tunHaiQiang.base_stats.set(CustomBaseStatsConstant.Speed, 50f);
            tunHaiQiang.base_stats.set(CustomBaseStatsConstant.MultiplierMana, 0.7f);
            tunHaiQiang.base_stats.set(CustomBaseStatsConstant.Knockback, 0.5f);
            tunHaiQiang.base_stats.set(CustomBaseStatsConstant.Health, 7500f);
            tunHaiQiang.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.4f);
            tunHaiQiang.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.5f);
            tunHaiQiang.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            tunHaiQiang.equipment_value = 5000;
            tunHaiQiang.quality = Rarity.R3_Legendary;
            tunHaiQiang.equipment_type = EquipmentType.Weapon;
            tunHaiQiang.name_class = "item_class_weapon";

            tunHaiQiang.path_slash_animation = "effects/slashes/slash_sword";
            tunHaiQiang.path_icon = $"{PathIcon}/icon_tun_hai_qiang";
            tunHaiQiang.path_gameplay_sprite = $"weapons/tun_hai_qiang"; //使用现有图片资源
            tunHaiQiang.gameplay_sprites = getWeaponSprites("tun_hai_qiang"); //复用现有图片资源

            tunHaiQiang.action_attack_target = new AttackAction(CustomItemActions.tunHaiQiangAttackEffect);        //特殊攻击效果
            AssetManager.items.list.AddItem(tunHaiQiang);
            addToLocale(tunHaiQiang.id, tunHaiQiang.translation_key, "东海龙宫镇宫之宝，枪出可吞江噬海！");
            #endregion

            #region 轩辕剑
            ItemAsset xuanYuanJian = AssetManager.items.clone("xuan_yuan_jian", "$weapon");
            xuanYuanJian.id = "xuan_yuan_jian";
            xuanYuanJian.material = "adamantine";
            xuanYuanJian.translation_key = "xuan_yuan_jian";
            xuanYuanJian.equipment_subtype = "xuan_yuan_jian";
            xuanYuanJian.group_id = "sword";
            xuanYuanJian.animated = false;
            xuanYuanJian.is_pool_weapon = false;
            xuanYuanJian.unlock(true);
            xuanYuanJian.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            xuanYuanJian.base_stats = new();
            xuanYuanJian.base_stats.set(CustomBaseStatsConstant.Damage, 3000f);
            xuanYuanJian.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 50f);
            xuanYuanJian.base_stats.set(CustomBaseStatsConstant.MultiplierMana, 0.7f);
            xuanYuanJian.base_stats.set(CustomBaseStatsConstant.CriticalChance, 1.0f);
            xuanYuanJian.base_stats.set(CustomBaseStatsConstant.CriticalDamageMultiplier, 1.6f);
            xuanYuanJian.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.75f);
            xuanYuanJian.base_stats.set(CustomBaseStatsConstant.Speed, 50f);
            xuanYuanJian.base_stats.set(CustomBaseStatsConstant.MultiplierHealth, 1.2f);
            xuanYuanJian.base_stats[strings.S.damage_range] = 0.6f;//伤害范围
            xuanYuanJian.equipment_value = 5000;
            xuanYuanJian.quality = Rarity.R3_Legendary;
            xuanYuanJian.equipment_type = EquipmentType.Weapon;
            xuanYuanJian.name_class = "item_class_weapon";

            xuanYuanJian.path_slash_animation = "effects/slashes/slash_sword";
            xuanYuanJian.path_icon = $"{PathIcon}/icon_xuan_yuan_jian";
            xuanYuanJian.path_gameplay_sprite = $"weapons/xuan_yuan_jian"; //使用现有图片资源
            xuanYuanJian.gameplay_sprites = getWeaponSprites("xuan_yuan_jian"); //复用现有图片资源

            xuanYuanJian.action_attack_target = new AttackAction(CustomItemActions.xuanYuanJianAttackEffect);        //特殊攻击效果
            AssetManager.items.list.AddItem(xuanYuanJian);
            addToLocale(xuanYuanJian.id, xuanYuanJian.translation_key, "人文始祖轩辕黄帝所铸圣道之剑，剑出可断万物法则！");
            #endregion
            
            #region 新增武器25-48
            // weapon_0 - 鸿蒙剑
            ItemAsset weapon_0 = AssetManager.items.clone("weapon_0", "$weapon");
            weapon_0.id = "weapon_0";
            weapon_0.material = "adamantine";
            weapon_0.translation_key = "weapon_0";
            weapon_0.equipment_subtype = "weapon_0";
            weapon_0.group_id = "sword";
            weapon_0.animated = false;
            weapon_0.is_pool_weapon = false;
            weapon_0.unlock(true);
            weapon_0.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            weapon_0.base_stats = new();
            weapon_0.base_stats.set(CustomBaseStatsConstant.Damage, 1500f);
            weapon_0.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 40f);
            weapon_0.base_stats.set(CustomBaseStatsConstant.Speed, 30f);
            weapon_0.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.6f);
            weapon_0.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.1f);
            weapon_0.base_stats.set(CustomBaseStatsConstant.CriticalDamageMultiplier, 0.2f);
            weapon_0.base_stats.set(CustomBaseStatsConstant.MultiplierHealth, 0.2f);
            weapon_0.base_stats[strings.S.damage_range] = 0.6f;
            weapon_0.equipment_value = 1500;
            weapon_0.quality = Rarity.R3_Legendary;
            weapon_0.equipment_type = EquipmentType.Weapon;
            weapon_0.name_class = "item_class_weapon";
            weapon_0.path_slash_animation = "effects/slashes/slash_sword";
            weapon_0.path_icon = $"{PathIcon}/icon_weapon_0";
            weapon_0.path_gameplay_sprite = $"weapons/weapon_0";
            weapon_0.gameplay_sprites = getWeaponSprites("weapon_0");
            AssetManager.items.list.AddItem(weapon_0);
            addToLocale(weapon_0.id, weapon_0.translation_key, "混沌初开时诞生的圣剑，蕴含鸿蒙之气！");
            
            ItemAsset weapon_1 = AssetManager.items.clone("weapon_1", "$weapon");
            weapon_1.id = "weapon_1";
            weapon_1.material = "adamantine";
            weapon_1.translation_key = "weapon_1";
            weapon_1.equipment_subtype = "weapon_1";
            weapon_1.group_id = "sword";
            weapon_1.animated = false;
            weapon_1.is_pool_weapon = false;
            weapon_1.unlock(true);
            weapon_1.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            weapon_1.base_stats = new();
            weapon_1.base_stats.set(CustomBaseStatsConstant.Damage, 2200f);
            weapon_1.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 25f);
            weapon_1.base_stats.set(CustomBaseStatsConstant.Speed, 20f);
            weapon_1.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.5f);
            weapon_1.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.7f);
            weapon_1.base_stats[strings.S.damage_range] = 0.6f;
            weapon_1.equipment_value = 1200;
            weapon_1.quality = Rarity.R3_Legendary;
            weapon_1.equipment_type = EquipmentType.Weapon;
            weapon_1.name_class = "item_class_weapon";
            weapon_1.path_slash_animation = "effects/slashes/slash_sword";
            weapon_1.path_icon = $"{PathIcon}/icon_weapon_1";
            weapon_1.path_gameplay_sprite = $"weapons/weapon_1";
            weapon_1.gameplay_sprites = getWeaponSprites("weapon_1");
            AssetManager.items.list.AddItem(weapon_1);
            addToLocale(weapon_1.id, weapon_1.translation_key, "天外陨星所铸，刀身流转星光！");
            
            ItemAsset weapon_2 = AssetManager.items.clone("weapon_2", "$weapon");
            weapon_2.id = "weapon_2";
            weapon_2.material = "adamantine";
            weapon_2.translation_key = "weapon_2";
            weapon_2.equipment_subtype = "weapon_2";
            weapon_2.group_id = "sword";
            weapon_2.animated = false;
            weapon_2.is_pool_weapon = false;
            weapon_2.unlock(true);
            weapon_2.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            weapon_2.base_stats = new();
            weapon_2.base_stats.set(CustomBaseStatsConstant.Damage, 1800f);
            weapon_2.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 35f);
            weapon_2.base_stats.set(CustomBaseStatsConstant.Speed, 25f);
            weapon_2.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.55f);
            weapon_2.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.6f);
            weapon_2.base_stats[strings.S.damage_range] = 0.6f;
            weapon_2.equipment_value = 1800;
            weapon_2.quality = Rarity.R3_Legendary;
            weapon_2.equipment_type = EquipmentType.Weapon;
            weapon_2.name_class = "item_class_weapon";
            weapon_2.path_slash_animation = "effects/slashes/slash_sword";
            weapon_2.path_icon = $"{PathIcon}/icon_weapon_2";
            weapon_2.path_gameplay_sprite = $"weapons/weapon_2";
            weapon_2.gameplay_sprites = getWeaponSprites("weapon_2");
            AssetManager.items.list.AddItem(weapon_2);
            addToLocale(weapon_2.id, weapon_2.translation_key, "紫霄宫珍藏，枪出如雷贯日！");
            
            ItemAsset weapon_3 = AssetManager.items.clone("weapon_3", "$weapon");
            weapon_3.id = "weapon_3";
            weapon_3.material = "adamantine";
            weapon_3.translation_key = "weapon_3";
            weapon_3.equipment_subtype = "weapon_3";
            weapon_3.group_id = "sword";
            weapon_3.animated = false;
            weapon_3.is_pool_weapon = false;
            weapon_3.unlock(true);
            weapon_3.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            weapon_3.base_stats = new();
            weapon_3.base_stats.set(CustomBaseStatsConstant.Damage, 1200f);
            weapon_3.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 20f);
            weapon_3.base_stats.set(CustomBaseStatsConstant.Speed, 15f);
            weapon_3.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.4f);
            weapon_3.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.5f);
            weapon_3.base_stats[strings.S.damage_range] = 0.6f;
            weapon_3.equipment_value = 1200;
            weapon_3.quality = Rarity.R3_Legendary;
            weapon_3.equipment_type = EquipmentType.Weapon;
            weapon_3.name_class = "item_class_weapon";
            weapon_3.path_slash_animation = "effects/slashes/slash_sword";
            weapon_3.path_icon = $"{PathIcon}/icon_weapon_3";
            weapon_3.path_gameplay_sprite = $"weapons/weapon_3";
            weapon_3.gameplay_sprites = getWeaponSprites("weapon_3");
            AssetManager.items.list.AddItem(weapon_3);
            addToLocale(weapon_3.id, weapon_3.translation_key, "玄黄之气淬炼，棍法刚猛无匹！");
            
            ItemAsset weapon_4 = AssetManager.items.clone("weapon_4", "$weapon");
            weapon_4.id = "weapon_4";
            weapon_4.material = "adamantine";
            weapon_4.translation_key = "weapon_4";
            weapon_4.equipment_subtype = "weapon_4";
            weapon_4.group_id = "sword";
            weapon_4.animated = false;
            weapon_4.is_pool_weapon = false;
            weapon_4.unlock(true);
            weapon_4.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            weapon_4.base_stats = new();
            weapon_4.base_stats.set(CustomBaseStatsConstant.Damage, 1800f);
            weapon_4.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 45f);
            weapon_4.base_stats.set(CustomBaseStatsConstant.Speed, 35f);
            weapon_4.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.5f);
            weapon_4.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.6f);
            weapon_4.base_stats[strings.S.damage_range] = 0.6f;
            weapon_4.equipment_value = 1800;
            weapon_4.quality = Rarity.R3_Legendary;
            weapon_4.equipment_type = EquipmentType.Weapon;
            weapon_4.name_class = "item_class_weapon";
            weapon_4.path_slash_animation = "effects/slashes/slash_sword";
            weapon_4.path_icon = $"{PathIcon}/icon_weapon_4";
            weapon_4.path_gameplay_sprite = $"weapons/weapon_4";
            weapon_4.gameplay_sprites = getWeaponSprites("weapon_4");
            AssetManager.items.list.AddItem(weapon_4);
            addToLocale(weapon_4.id, weapon_4.translation_key, "青鸾鸟骨所铸，剑鸣如鸾啼！");
            
            ItemAsset weapon_5 = AssetManager.items.clone("weapon_5", "$weapon");
            weapon_5.id = "weapon_5";
            weapon_5.material = "adamantine";
            weapon_5.translation_key = "weapon_5";
            weapon_5.equipment_subtype = "weapon_5";
            weapon_5.group_id = "sword";
            weapon_5.animated = false;
            weapon_5.is_pool_weapon = false;
            weapon_5.unlock(true);
            weapon_5.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            weapon_5.base_stats = new();
            weapon_5.base_stats.set(CustomBaseStatsConstant.Damage, 2800f);
            weapon_5.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 15f);
            weapon_5.base_stats.set(CustomBaseStatsConstant.Speed, 10f);
            weapon_5.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.45f);
            weapon_5.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.6f);
            weapon_5.base_stats[strings.S.damage_range] = 0.6f;
            weapon_5.equipment_value = 1800;
            weapon_5.quality = Rarity.R3_Legendary;
            weapon_5.equipment_type = EquipmentType.Weapon;
            weapon_5.name_class = "item_class_weapon";
            weapon_5.path_slash_animation = "effects/slashes/slash_sword";
            weapon_5.path_icon = $"{PathIcon}/icon_weapon_5";
            weapon_5.path_gameplay_sprite = $"weapons/weapon_5";
            weapon_5.gameplay_sprites = getWeaponSprites("weapon_5");
            AssetManager.items.list.AddItem(weapon_5);
            addToLocale(weapon_5.id, weapon_5.translation_key, "熔岩核心铸就，锤落熔岩四溅！");
            
            ItemAsset weapon_6 = AssetManager.items.clone("weapon_6", "$weapon");
            weapon_6.id = "weapon_6";
            weapon_6.material = "adamantine";
            weapon_6.translation_key = "weapon_6";
            weapon_6.equipment_subtype = "weapon_6";
            weapon_6.group_id = "sword";
            weapon_6.animated = false;
            weapon_6.is_pool_weapon = false;
            weapon_6.unlock(true);
            weapon_6.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            weapon_6.base_stats = new();
            weapon_6.base_stats.set(CustomBaseStatsConstant.Damage, 1500f);
            weapon_6.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 30f);
            weapon_6.base_stats.set(CustomBaseStatsConstant.Speed, 25f);
            weapon_6.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.45f);
            weapon_6.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.6f);
            weapon_6.base_stats[strings.S.damage_range] = 0.6f;
            weapon_6.equipment_value = 1500;
            weapon_6.quality = Rarity.R3_Legendary;
            weapon_6.equipment_type = EquipmentType.Weapon;
            weapon_6.name_class = "item_class_weapon";
            weapon_6.path_slash_animation = "effects/slashes/slash_sword";
            weapon_6.path_icon = $"{PathIcon}/icon_weapon_6";
            weapon_6.path_gameplay_sprite = $"weapons/weapon_6";
            weapon_6.gameplay_sprites = getWeaponSprites("weapon_6");
            AssetManager.items.list.AddItem(weapon_6);
            addToLocale(weapon_6.id, weapon_6.translation_key, "冥河之水淬炼，剑出阴寒彻骨！");
            
            ItemAsset weapon_7 = AssetManager.items.clone("weapon_7", "$weapon");
            weapon_7.id = "weapon_7";
            weapon_7.material = "adamantine";
            weapon_7.translation_key = "weapon_7";
            weapon_7.equipment_subtype = "weapon_7";
            weapon_7.group_id = "sword";
            weapon_7.animated = false;
            weapon_7.is_pool_weapon = false;
            weapon_7.unlock(true);
            weapon_7.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            weapon_7.base_stats = new();
            weapon_7.base_stats.set(CustomBaseStatsConstant.Damage, 1600f);
            weapon_7.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 25f);
            weapon_7.base_stats.set(CustomBaseStatsConstant.Speed, 20f);
            weapon_7.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.5f);
            weapon_7.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.55f);
            weapon_7.base_stats[strings.S.damage_range] = 0.6f;
            weapon_7.equipment_value = 1600;
            weapon_7.quality = Rarity.R3_Legendary;
            weapon_7.equipment_type = EquipmentType.Weapon;
            weapon_7.name_class = "item_class_weapon";
            weapon_7.path_slash_animation = "effects/slashes/slash_sword";
            weapon_7.path_icon = $"{PathIcon}/icon_weapon_7";
            weapon_7.path_gameplay_sprite = $"weapons/weapon_7";
            weapon_7.gameplay_sprites = getWeaponSprites("weapon_7");
            AssetManager.items.list.AddItem(weapon_7);
            addToLocale(weapon_7.id, weapon_7.translation_key, "金蛟之骨所制，剪物如切豆腐！");
            
            ItemAsset weapon_8 = AssetManager.items.clone("weapon_8", "$weapon");
            weapon_8.id = "weapon_8";
            weapon_8.material = "adamantine";
            weapon_8.translation_key = "weapon_8";
            weapon_8.equipment_subtype = "weapon_8";
            weapon_8.group_id = "sword";
            weapon_8.animated = false;
            weapon_8.is_pool_weapon = false;
            weapon_8.unlock(true);
            weapon_8.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            weapon_8.base_stats = new();
            weapon_8.base_stats.set(CustomBaseStatsConstant.Damage, 2400f);
            weapon_8.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 35f);
            weapon_8.base_stats.set(CustomBaseStatsConstant.Speed, 30f);
            weapon_8.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.45f);
            weapon_8.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.5f);
            weapon_8.base_stats[strings.S.damage_range] = 0.6f;
            weapon_8.equipment_value = 1400;
            weapon_8.quality = Rarity.R3_Legendary;
            weapon_8.equipment_type = EquipmentType.Weapon;
            weapon_8.name_class = "item_class_weapon";
            weapon_8.path_slash_animation = "effects/slashes/slash_sword";
            weapon_8.path_icon = $"{PathIcon}/icon_weapon_8";
            weapon_8.path_gameplay_sprite = $"weapons/weapon_8";
            weapon_8.gameplay_sprites = getWeaponSprites("weapon_8");
            AssetManager.items.list.AddItem(weapon_8);
            addToLocale(weapon_8.id, weapon_8.translation_key, "饮尽万敌之血，枪身碧绿如翡翠！");
            
            ItemAsset weapon_9 = AssetManager.items.clone("weapon_9", "$weapon");
            weapon_9.id = "weapon_9";
            weapon_9.material = "adamantine";
            weapon_9.translation_key = "weapon_9";
            weapon_9.equipment_subtype = "weapon_9";
            weapon_9.group_id = "sword";
            weapon_9.animated = false;
            weapon_9.is_pool_weapon = false;
            weapon_9.unlock(true);
            weapon_9.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            weapon_9.base_stats = new();
            weapon_9.base_stats.set(CustomBaseStatsConstant.Damage, 2300f);
            weapon_9.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 20f);
            weapon_9.base_stats.set(CustomBaseStatsConstant.Speed, 15f);
            weapon_9.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.5f);
            weapon_9.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.65f);
            weapon_9.base_stats[strings.S.damage_range] = 0.6f;
            weapon_9.equipment_value = 1300;
            weapon_9.quality = Rarity.R3_Legendary;
            weapon_9.equipment_type = EquipmentType.Weapon;
            weapon_9.name_class = "item_class_weapon";
            weapon_9.path_slash_animation = "effects/slashes/slash_sword";
            weapon_9.path_icon = $"{PathIcon}/icon_weapon_9";
            weapon_9.path_gameplay_sprite = $"weapons/weapon_9";
            weapon_9.gameplay_sprites = getWeaponSprites("weapon_9");
            AssetManager.items.list.AddItem(weapon_9);
            addToLocale(weapon_9.id, weapon_9.translation_key, "戟出如雷震天，万军辟易！");
            
            ItemAsset weapon_10 = AssetManager.items.clone("weapon_10", "$weapon");
            weapon_10.id = "weapon_10";
            weapon_10.material = "adamantine";
            weapon_10.translation_key = "weapon_10";
            weapon_10.equipment_subtype = "weapon_10";
            weapon_10.group_id = "sword";
            weapon_10.animated = false;
            weapon_10.is_pool_weapon = false;
            weapon_10.unlock(true);
            weapon_10.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            weapon_10.base_stats = new();
            weapon_10.base_stats.set(CustomBaseStatsConstant.Damage, 1900f);
            weapon_10.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 40f);
            weapon_10.base_stats.set(CustomBaseStatsConstant.Speed, 40f);
            weapon_10.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.55f);
            weapon_10.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.7f);
            weapon_10.base_stats[strings.S.damage_range] = 0.6f;
            weapon_10.equipment_value = 1900;
            weapon_10.quality = Rarity.R3_Legendary;
            weapon_10.equipment_type = EquipmentType.Weapon;
            weapon_10.name_class = "item_class_weapon";
            weapon_10.path_slash_animation = "effects/slashes/slash_sword";
            weapon_10.path_icon = $"{PathIcon}/icon_weapon_10";
            weapon_10.path_gameplay_sprite = $"weapons/weapon_10";
            weapon_10.gameplay_sprites = getWeaponSprites("weapon_10");
            AssetManager.items.list.AddItem(weapon_10);
            addToLocale(weapon_10.id, weapon_10.translation_key, "雷神之怒所化，剑快如闪电！");
            
            ItemAsset weapon_12 = AssetManager.items.clone("weapon_12", "$weapon");
            weapon_12.id = "weapon_12";
            weapon_12.material = "adamantine";
            weapon_12.translation_key = "weapon_12";
            weapon_12.equipment_subtype = "weapon_12";
            weapon_12.group_id = "sword";
            weapon_12.animated = false;
            weapon_12.is_pool_weapon = false;
            weapon_12.unlock(true);
            weapon_12.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            weapon_12.base_stats = new();
            weapon_12.base_stats.set(CustomBaseStatsConstant.Damage, 1800f);
            weapon_12.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 15f);
            weapon_12.base_stats.set(CustomBaseStatsConstant.Speed, 10f);
            weapon_12.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.3f);
            weapon_12.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.2f);
            weapon_12.base_stats[strings.S.damage_range] = 0.6f;
            weapon_12.equipment_value = 1800;
            weapon_12.quality = Rarity.R3_Legendary;
            weapon_12.equipment_type = EquipmentType.Weapon;
            weapon_12.name_class = "item_class_weapon";
            weapon_12.path_slash_animation = "effects/slashes/slash_sword";
            weapon_12.path_icon = $"{PathIcon}/icon_weapon_12";
            weapon_12.path_gameplay_sprite = $"weapons/weapon_12";
            weapon_12.gameplay_sprites = getWeaponSprites("weapon_12");
            AssetManager.items.list.AddItem(weapon_12);
            addToLocale(weapon_12.id, weapon_12.translation_key, "普通玄铁所铸，坚韧耐用！");
            
            ItemAsset weapon_13 = AssetManager.items.clone("weapon_13", "$weapon");
            weapon_13.id = "weapon_13";
            weapon_13.material = "adamantine";
            weapon_13.translation_key = "weapon_13";
            weapon_13.equipment_subtype = "weapon_13";
            weapon_13.group_id = "sword";
            weapon_13.animated = false;
            weapon_13.is_pool_weapon = false;
            weapon_13.unlock(true);
            weapon_13.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            weapon_13.base_stats = new();
            weapon_13.base_stats.set(CustomBaseStatsConstant.Damage, 1600f);
            weapon_13.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 20f);
            weapon_13.base_stats.set(CustomBaseStatsConstant.Speed, 15f);
            weapon_13.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.25f);
            weapon_13.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.15f);
            weapon_13.base_stats[strings.S.damage_range] = 0.6f;
            weapon_13.equipment_value = 1600;
            weapon_13.quality = Rarity.R0_Normal;
            weapon_13.equipment_type = EquipmentType.Weapon;
            weapon_13.name_class = "item_class_weapon";
            weapon_13.path_slash_animation = "effects/slashes/slash_sword";
            weapon_13.path_icon = $"{PathIcon}/icon_weapon_13";
            weapon_13.path_gameplay_sprite = $"weapons/weapon_13";
            weapon_13.gameplay_sprites = getWeaponSprites("weapon_13");
            AssetManager.items.list.AddItem(weapon_13);
            addToLocale(weapon_13.id, weapon_13.translation_key, "青钢打造，锋利坚韧！");
            
            ItemAsset weapon_14 = AssetManager.items.clone("weapon_14", "$weapon");
            weapon_14.id = "weapon_14";
            weapon_14.material = "copper";
            weapon_14.translation_key = "weapon_14";
            weapon_14.equipment_subtype = "weapon_14";
            weapon_14.group_id = "sword";
            weapon_14.animated = false;
            weapon_14.is_pool_weapon = false;
            weapon_14.unlock(true);
            weapon_14.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            weapon_14.base_stats = new();
            weapon_14.base_stats.set(CustomBaseStatsConstant.Damage, 1400f);
            weapon_14.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 25f);
            weapon_14.base_stats.set(CustomBaseStatsConstant.Speed, 20f);
            weapon_14.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.2f);
            weapon_14.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.1f);
            weapon_14.base_stats[strings.S.damage_range] = 0.6f;
            weapon_14.equipment_value = 1400;
            weapon_14.quality = Rarity.R0_Normal;
            weapon_14.equipment_type = EquipmentType.Weapon;
            weapon_14.name_class = "item_class_weapon";
            weapon_14.path_slash_animation = "effects/slashes/slash_sword";
            weapon_14.path_icon = $"{PathIcon}/icon_weapon_14";
            weapon_14.path_gameplay_sprite = $"weapons/weapon_14";
            weapon_14.gameplay_sprites = getWeaponSprites("weapon_14");
            AssetManager.items.list.AddItem(weapon_14);
            addToLocale(weapon_14.id, weapon_14.translation_key, "紫铜铸就，剑身紫红如霞！");
            
            ItemAsset weapon_15 = AssetManager.items.clone("weapon_15", "$weapon");
            weapon_15.id = "weapon_15";
            weapon_15.material = "adamantine";
            weapon_15.translation_key = "weapon_15";
            weapon_15.equipment_subtype = "weapon_15";
            weapon_15.group_id = "sword";
            weapon_15.animated = false;
            weapon_15.is_pool_weapon = false;
            weapon_15.unlock(true);
            weapon_15.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            weapon_15.base_stats = new();
            weapon_15.base_stats.set(CustomBaseStatsConstant.Damage, 1700f);
            weapon_15.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 15f);
            weapon_15.base_stats.set(CustomBaseStatsConstant.Speed, 10f);
            weapon_15.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.25f);
            weapon_15.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.15f);
            weapon_15.base_stats[strings.S.damage_range] = 0.6f;
            weapon_15.equipment_value = 1700;
            weapon_15.quality = Rarity.R0_Normal;
            weapon_15.equipment_type = EquipmentType.Weapon;
            weapon_15.name_class = "item_class_weapon";
            weapon_15.path_slash_animation = "effects/slashes/slash_sword";
            weapon_15.path_icon = $"{PathIcon}/icon_weapon_15";
            weapon_15.path_gameplay_sprite = $"weapons/weapon_15";
            weapon_15.gameplay_sprites = getWeaponSprites("weapon_15");
            AssetManager.items.list.AddItem(weapon_15);
            addToLocale(weapon_15.id, weapon_15.translation_key, "赤铁锻造，枪身赤红如火！");
            
            ItemAsset weapon_16 = AssetManager.items.clone("weapon_16", "$weapon");
            weapon_16.id = "weapon_16";
            weapon_16.material = "adamantine";
            weapon_16.translation_key = "weapon_16";
            weapon_16.equipment_subtype = "weapon_16";
            weapon_16.group_id = "sword";
            weapon_16.animated = false;
            weapon_16.is_pool_weapon = false;
            weapon_16.unlock(true);
            weapon_16.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            weapon_16.base_stats = new();
            weapon_16.base_stats.set(CustomBaseStatsConstant.Damage, 1400f);
            weapon_16.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 30f);
            weapon_16.base_stats.set(CustomBaseStatsConstant.Speed, 25f);
            weapon_16.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.4f);
            weapon_16.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.5f);
            weapon_16.base_stats[strings.S.damage_range] = 0.6f;
            weapon_16.equipment_value = 1200;
            weapon_16.quality = Rarity.R3_Legendary;
            weapon_16.equipment_type = EquipmentType.Weapon;
            weapon_16.name_class = "item_class_weapon";
            weapon_16.path_slash_animation = "effects/slashes/slash_sword";
            weapon_16.path_icon = $"{PathIcon}/icon_weapon_16";
            weapon_16.path_gameplay_sprite = $"weapons/weapon_16";
            weapon_16.gameplay_sprites = getWeaponSprites("weapon_16");
            AssetManager.items.list.AddItem(weapon_16);
            addToLocale(weapon_16.id, weapon_16.translation_key, "玄冰寒铁所铸，剑出冻结万物！");
            
            ItemAsset weapon_17 = AssetManager.items.clone("weapon_17", "$weapon");
            weapon_17.id = "weapon_17";
            weapon_17.material = "adamantine";
            weapon_17.translation_key = "weapon_17";
            weapon_17.equipment_subtype = "weapon_17";
            weapon_17.group_id = "sword";
            weapon_17.animated = false;
            weapon_17.is_pool_weapon = false;
            weapon_17.unlock(true);
            weapon_17.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            weapon_17.base_stats = new();
            weapon_17.base_stats.set(CustomBaseStatsConstant.Damage, 1700f);
            weapon_17.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 20f);
            weapon_17.base_stats.set(CustomBaseStatsConstant.Speed, 15f);
            weapon_17.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.45f);
            weapon_17.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.5f);
            weapon_17.base_stats[strings.S.damage_range] = 0.6f;
            weapon_17.equipment_value = 1700;
            weapon_17.quality = Rarity.R3_Legendary;
            weapon_17.equipment_type = EquipmentType.Weapon;
            weapon_17.name_class = "item_class_weapon";
            weapon_17.path_slash_animation = "effects/slashes/slash_sword";
            weapon_17.path_icon = $"{PathIcon}/icon_weapon_17";
            weapon_17.path_gameplay_sprite = $"weapons/weapon_17";
            weapon_17.gameplay_sprites = getWeaponSprites("weapon_17");
            AssetManager.items.list.AddItem(weapon_17);
            addToLocale(weapon_17.id, weapon_17.translation_key, "金刚不坏之身，刀劈山岳！");
            
            ItemAsset weapon_91 = AssetManager.items.clone("weapon_91", "$weapon");
            weapon_91.id = "weapon_91";
            weapon_91.material = "adamantine";
            weapon_91.translation_key = "weapon_91";
            weapon_91.equipment_subtype = "weapon_91";
            weapon_91.group_id = "sword";
            weapon_91.animated = false;
            weapon_91.is_pool_weapon = false;
            weapon_91.unlock(true);
            weapon_91.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            weapon_91.base_stats = new();
            weapon_91.base_stats.set(CustomBaseStatsConstant.Damage, 1500f);
            weapon_91.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 50f);
            weapon_91.base_stats.set(CustomBaseStatsConstant.Speed, 50f);
            weapon_91.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.7f);
            weapon_91.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.9f);
            weapon_91.base_stats.set(CustomBaseStatsConstant.CriticalDamageMultiplier, 0.4f);
            weapon_91.base_stats.set(CustomBaseStatsConstant.MultiplierHealth, 1.0f);
            weapon_91.base_stats[strings.S.damage_range] = 0.6f;
            weapon_91.equipment_value = 1500;
            weapon_91.quality = Rarity.R3_Legendary;
            weapon_91.equipment_type = EquipmentType.Weapon;
            weapon_91.name_class = "item_class_weapon";
            weapon_91.path_slash_animation = "effects/slashes/slash_sword";
            weapon_91.path_icon = $"{PathIcon}/icon_weapon_91";
            weapon_91.path_gameplay_sprite = $"weapons/weapon_91";
            weapon_91.gameplay_sprites = getWeaponSprites("weapon_91");
            AssetManager.items.list.AddItem(weapon_91);
            addToLocale(weapon_91.id, weapon_91.translation_key, "诛仙四剑之一，剑出诛仙灭魔！");
            
            ItemAsset weapon_92 = AssetManager.items.clone("weapon_92", "$weapon");
            weapon_92.id = "weapon_92";
            weapon_92.material = "adamantine";
            weapon_92.translation_key = "weapon_92";
            weapon_92.equipment_subtype = "weapon_92";
            weapon_92.group_id = "sword";
            weapon_92.animated = false;
            weapon_92.is_pool_weapon = false;
            weapon_92.unlock(true);
            weapon_92.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            weapon_92.base_stats = new();
            weapon_92.base_stats.set(CustomBaseStatsConstant.Damage, 1800f);
            weapon_92.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 45f);
            weapon_92.base_stats.set(CustomBaseStatsConstant.Speed, 45f);
            weapon_92.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.75f);
            weapon_92.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.95f);
            weapon_92.base_stats.set(CustomBaseStatsConstant.CriticalDamageMultiplier, 0.5f);
            weapon_92.base_stats.set(CustomBaseStatsConstant.MultiplierHealth, 0.15f);
            weapon_92.base_stats[strings.S.damage_range] = 0.6f;
            weapon_92.equipment_value = 1800;
            weapon_92.quality = Rarity.R3_Legendary;
            weapon_92.equipment_type = EquipmentType.Weapon;
            weapon_92.name_class = "item_class_weapon";
            weapon_92.path_slash_animation = "effects/slashes/slash_sword";
            weapon_92.path_icon = $"{PathIcon}/icon_weapon_92";
            weapon_92.path_gameplay_sprite = $"weapons/weapon_92";
            weapon_92.gameplay_sprites = getWeaponSprites("weapon_92");
            AssetManager.items.list.AddItem(weapon_92);
            addToLocale(weapon_92.id, weapon_92.translation_key, "诛仙四剑之一，剑出戮尽天下！");
            
            ItemAsset weapon_93 = AssetManager.items.clone("weapon_93", "$weapon");
            weapon_93.id = "weapon_93";
            weapon_93.material = "adamantine";
            weapon_93.translation_key = "weapon_93";
            weapon_93.equipment_subtype = "weapon_93";
            weapon_93.group_id = "sword";
            weapon_93.animated = false;
            weapon_93.is_pool_weapon = false;
            weapon_93.unlock(true);
            weapon_93.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            weapon_93.base_stats = new();
            weapon_93.base_stats.set(CustomBaseStatsConstant.Damage, 2200f);
            weapon_93.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 40f);
            weapon_93.base_stats.set(CustomBaseStatsConstant.Speed, 40f);
            weapon_93.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.8f);
            weapon_93.base_stats.set(CustomBaseStatsConstant.CriticalChance, 1.0f);
            weapon_93.base_stats.set(CustomBaseStatsConstant.CriticalDamageMultiplier, 0.6f);
            weapon_93.base_stats.set(CustomBaseStatsConstant.MultiplierHealth, 0.2f);
            weapon_93.base_stats[strings.S.damage_range] = 0.6f;
            weapon_93.equipment_value = 1200;
            weapon_93.quality = Rarity.R3_Legendary;
            weapon_93.equipment_type = EquipmentType.Weapon;
            weapon_93.name_class = "item_class_weapon";
            weapon_93.path_slash_animation = "effects/slashes/slash_sword";
            weapon_93.path_icon = $"{PathIcon}/icon_weapon_93";
            weapon_93.path_gameplay_sprite = $"weapons/weapon_93";
            weapon_93.gameplay_sprites = getWeaponSprites("weapon_93");
            AssetManager.items.list.AddItem(weapon_93);
            addToLocale(weapon_93.id, weapon_93.translation_key, "诛仙四剑之一，剑出陷入黄泉！");
            
            ItemAsset weapon_94 = AssetManager.items.clone("weapon_94", "$weapon");
            weapon_94.id = "weapon_94";
            weapon_94.material = "adamantine";
            weapon_94.translation_key = "weapon_94";
            weapon_94.equipment_subtype = "weapon_94";
            weapon_94.group_id = "sword";
            weapon_94.animated = false;
            weapon_94.is_pool_weapon = false;
            weapon_94.unlock(true);
            weapon_94.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            weapon_94.base_stats = new();
            weapon_94.base_stats.set(CustomBaseStatsConstant.Damage, 1500f);
            weapon_94.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 35f);
            weapon_94.base_stats.set(CustomBaseStatsConstant.Speed, 35f);
            weapon_94.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.85f);
            weapon_94.base_stats.set(CustomBaseStatsConstant.CriticalChance, 1.0f);
            weapon_94.base_stats.set(CustomBaseStatsConstant.CriticalDamageMultiplier, 0.7f);
            weapon_94.base_stats.set(CustomBaseStatsConstant.MultiplierHealth, 0.3f);
            weapon_94.base_stats[strings.S.damage_range] = 0.6f;
            weapon_94.equipment_value = 1500;
            weapon_94.quality = Rarity.R3_Legendary;
            weapon_94.equipment_type = EquipmentType.Weapon;
            weapon_94.name_class = "item_class_weapon";
            weapon_94.path_slash_animation = "effects/slashes/slash_sword";
            weapon_94.path_icon = $"{PathIcon}/icon_weapon_94";
            weapon_94.path_gameplay_sprite = $"weapons/weapon_94";
            weapon_94.gameplay_sprites = getWeaponSprites("weapon_94");
            AssetManager.items.list.AddItem(weapon_94);
            addToLocale(weapon_94.id, weapon_94.translation_key, "诛仙四剑之一，剑出绝灭生机！");
            
            ItemAsset weapon_95 = AssetManager.items.clone("weapon_95", "$weapon");
            weapon_95.id = "weapon_95";
            weapon_95.material = "adamantine";
            weapon_95.translation_key = "weapon_95";
            weapon_95.equipment_subtype = "weapon_95";
            weapon_95.group_id = "sword";
            weapon_95.animated = false;
            weapon_95.is_pool_weapon = false;
            weapon_95.unlock(true);
            weapon_95.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            weapon_95.base_stats = new();
            weapon_95.base_stats.set(CustomBaseStatsConstant.Damage, 2000f);
            weapon_95.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 30f);
            weapon_95.base_stats.set(CustomBaseStatsConstant.Speed, 30f);
            weapon_95.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.9f);
            weapon_95.base_stats.set(CustomBaseStatsConstant.CriticalChance, 1.0f);
            weapon_95.base_stats.set(CustomBaseStatsConstant.CriticalDamageMultiplier, 0.8f);
            weapon_95.base_stats.set(CustomBaseStatsConstant.MultiplierHealth, 0.3f);
            weapon_95.base_stats[strings.S.damage_range] = 0.6f;
            weapon_95.equipment_value = 1700;
            weapon_95.quality = Rarity.R3_Legendary;
            weapon_95.equipment_type = EquipmentType.Weapon;
            weapon_95.name_class = "item_class_weapon";
            weapon_95.path_slash_animation = "effects/slashes/slash_sword";
            weapon_95.path_icon = $"{PathIcon}/icon_weapon_95";
            weapon_95.path_gameplay_sprite = $"weapons/weapon_95";
            weapon_95.gameplay_sprites = getWeaponSprites("weapon_95");
            AssetManager.items.list.AddItem(weapon_95);
            addToLocale(weapon_95.id, weapon_95.translation_key, "盘古开天辟地之斧，斧劈混沌！");
            
            ItemAsset weapon_96 = AssetManager.items.clone("weapon_96", "$weapon");
            weapon_96.id = "weapon_96";
            weapon_96.material = "adamantine";
            weapon_96.translation_key = "weapon_96";
            weapon_96.equipment_subtype = "weapon_96";
            weapon_96.group_id = "sword";
            weapon_96.animated = false;
            weapon_96.is_pool_weapon = false;
            weapon_96.unlock(true);
            weapon_96.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            weapon_96.base_stats = new();
            weapon_96.base_stats.set(CustomBaseStatsConstant.Damage, 1800f);
            weapon_96.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 60f);
            weapon_96.base_stats.set(CustomBaseStatsConstant.Speed, 60f);
            weapon_96.base_stats.set(CustomBaseStatsConstant.Accuracy, 1.0f);
            weapon_96.base_stats.set(CustomBaseStatsConstant.CriticalChance, 1.0f);
            weapon_96.base_stats.set(CustomBaseStatsConstant.CriticalDamageMultiplier, 0.5f);
            weapon_96.base_stats.set(CustomBaseStatsConstant.MultiplierHealth, 0.4f);
            weapon_96.base_stats[strings.S.damage_range] = 0.6f;
            weapon_96.equipment_value = 1800;
            weapon_96.quality = Rarity.R3_Legendary;
            weapon_96.equipment_type = EquipmentType.Weapon;
            weapon_96.name_class = "item_class_weapon";
            weapon_96.path_slash_animation = "effects/slashes/slash_sword";
            weapon_96.path_icon = $"{PathIcon}/icon_weapon_96";
            weapon_96.path_gameplay_sprite = $"weapons/weapon_96";
            weapon_96.gameplay_sprites = getWeaponSprites("weapon_96");
            AssetManager.items.list.AddItem(weapon_96);
            addToLocale(weapon_96.id, weapon_96.translation_key, "黄帝轩辕氏之剑，斩妖除魔，威慑天下！");
            
            ItemAsset weapon_99 = AssetManager.items.clone("weapon_99", "$weapon");
            weapon_99.id = "weapon_99";
            weapon_99.material = "adamantine";
            weapon_99.translation_key = "weapon_99";
            weapon_99.equipment_subtype = "weapon_99";
            weapon_99.group_id = "sword";
            weapon_99.animated = false;
            weapon_99.is_pool_weapon = false;
            weapon_99.unlock(true);
            weapon_99.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            weapon_99.base_stats = new();
            weapon_99.base_stats.set(CustomBaseStatsConstant.Damage, 1600f);
            weapon_99.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 80f);
            weapon_99.base_stats.set(CustomBaseStatsConstant.Speed, 80f);
            weapon_99.base_stats.set(CustomBaseStatsConstant.Accuracy, 1.5f);
            weapon_99.base_stats.set(CustomBaseStatsConstant.CriticalChance, 1.5f);
            weapon_99.base_stats.set(CustomBaseStatsConstant.CriticalDamageMultiplier, 0.5f);
            weapon_99.base_stats.set(CustomBaseStatsConstant.MultiplierHealth, 0.3f);
            weapon_99.base_stats[strings.S.damage_range] = 0.6f;
            weapon_99.equipment_value = 1600;
            weapon_99.quality = Rarity.R3_Legendary;
            weapon_99.equipment_type = EquipmentType.Weapon;
            weapon_99.name_class = "item_class_weapon";
            weapon_99.path_slash_animation = "effects/slashes/slash_sword";
            weapon_99.path_icon = $"{PathIcon}/icon_weapon_99";
            weapon_99.path_gameplay_sprite = $"weapons/weapon_99";
            weapon_99.gameplay_sprites = getWeaponSprites("weapon_99");
            AssetManager.items.list.AddItem(weapon_99);
            addToLocale(weapon_99.id, weapon_99.translation_key, "开天辟地第一剑，蕴含宇宙本源之力！");
            #endregion
            
            #region 新增刀剑类武器49-64
            // custom_sword_1 - 青冥剑
            ItemAsset custom_sword_1 = AssetManager.items.clone("custom_sword_1", "$weapon");
            custom_sword_1.id = "custom_sword_1";
            custom_sword_1.material = "adamantine";
            custom_sword_1.translation_key = "custom_sword_1";
            custom_sword_1.equipment_subtype = "custom_sword_1";
            custom_sword_1.group_id = "sword";
            custom_sword_1.animated = false;
            custom_sword_1.is_pool_weapon = false;
            custom_sword_1.unlock(true);
            custom_sword_1.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            custom_sword_1.base_stats = new();
            custom_sword_1.base_stats.set(CustomBaseStatsConstant.Damage, 1800f);
            custom_sword_1.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 60f);
            custom_sword_1.base_stats.set(CustomBaseStatsConstant.Speed, 40f);
            custom_sword_1.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.7f);
            custom_sword_1.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.8f);
            custom_sword_1.base_stats[strings.S.damage_range] = 0.6f;
            custom_sword_1.equipment_value = 1800;
            custom_sword_1.quality = Rarity.R3_Legendary;
            custom_sword_1.equipment_type = EquipmentType.Weapon;
            custom_sword_1.name_class = "item_class_weapon";
            custom_sword_1.path_slash_animation = "effects/slashes/slash_sword";
            custom_sword_1.path_icon = $"{PathIcon}/icon_custom_sword_1";
            custom_sword_1.path_gameplay_sprite = $"weapons/custom_sword_1";
            custom_sword_1.gameplay_sprites = getWeaponSprites("custom_sword_1");
            AssetManager.items.list.AddItem(custom_sword_1);
            addToLocale(custom_sword_1.id, custom_sword_1.translation_key, "青冥山千年寒铁所铸，剑出鞘青光四射！");
            
            // custom_sword_2 - 玄铁剑
            ItemAsset custom_sword_2 = AssetManager.items.clone("custom_sword_2", "$weapon");
            custom_sword_2.id = "custom_sword_2";
            custom_sword_2.material = "adamantine";
            custom_sword_2.translation_key = "custom_sword_2";
            custom_sword_2.equipment_subtype = "custom_sword_2";
            custom_sword_2.group_id = "sword";
            custom_sword_2.animated = false;
            custom_sword_2.is_pool_weapon = false;
            custom_sword_2.unlock(true);
            custom_sword_2.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            custom_sword_2.base_stats = new();
            custom_sword_2.base_stats.set(CustomBaseStatsConstant.Damage, 1500f);
            custom_sword_2.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 50f);
            custom_sword_2.base_stats.set(CustomBaseStatsConstant.Speed, 30f);
            custom_sword_2.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.6f);
            custom_sword_2.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.6f);
            custom_sword_2.base_stats[strings.S.damage_range] = 0.6f;
            custom_sword_2.equipment_value = 1500;
            custom_sword_2.quality = Rarity.R3_Legendary;
            custom_sword_2.equipment_type = EquipmentType.Weapon;
            custom_sword_2.name_class = "item_class_weapon";
            custom_sword_2.path_slash_animation = "effects/slashes/slash_sword";
            custom_sword_2.path_icon = $"{PathIcon}/icon_custom_sword_2";
            custom_sword_2.path_gameplay_sprite = $"weapons/custom_sword_2";
            custom_sword_2.gameplay_sprites = getWeaponSprites("custom_sword_2");
            AssetManager.items.list.AddItem(custom_sword_2);
            addToLocale(custom_sword_2.id, custom_sword_2.translation_key, "黑风山玄铁打造，剑身厚重无锋！");
            
            // custom_sword_3 - 赤焰刀
            ItemAsset custom_sword_3 = AssetManager.items.clone("custom_sword_3", "$weapon");
            custom_sword_3.id = "custom_sword_3";
            custom_sword_3.material = "adamantine";
            custom_sword_3.translation_key = "custom_sword_3";
            custom_sword_3.equipment_subtype = "custom_sword_3";
            custom_sword_3.group_id = "sword";
            custom_sword_3.animated = false;
            custom_sword_3.is_pool_weapon = false;
            custom_sword_3.unlock(true);
            custom_sword_3.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            custom_sword_3.base_stats = new();
            custom_sword_3.base_stats.set(CustomBaseStatsConstant.Damage, 2000f);
            custom_sword_3.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 45f);
            custom_sword_3.base_stats.set(CustomBaseStatsConstant.Speed, 35f);
            custom_sword_3.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.65f);
            custom_sword_3.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.7f);
            custom_sword_3.base_stats[strings.S.damage_range] = 0.6f;
            custom_sword_3.equipment_value = 2000;
            custom_sword_3.quality = Rarity.R3_Legendary;
            custom_sword_3.equipment_type = EquipmentType.Weapon;
            custom_sword_3.name_class = "item_class_weapon";
            custom_sword_3.path_slash_animation = "effects/slashes/slash_sword";
            custom_sword_3.path_icon = $"{PathIcon}/icon_custom_sword_3";
            custom_sword_3.path_gameplay_sprite = $"weapons/custom_sword_3";
            custom_sword_3.gameplay_sprites = getWeaponSprites("custom_sword_3");
            AssetManager.items.list.AddItem(custom_sword_3);
            addToLocale(custom_sword_3.id, custom_sword_3.translation_key, "火山熔岩淬炼，刀身赤焰缭绕！");
            
            // custom_sword_4 - 紫电剑
            ItemAsset custom_sword_4 = AssetManager.items.clone("custom_sword_4", "$weapon");
            custom_sword_4.id = "custom_sword_4";
            custom_sword_4.material = "adamantine";
            custom_sword_4.translation_key = "custom_sword_4";
            custom_sword_4.equipment_subtype = "custom_sword_4";
            custom_sword_4.group_id = "sword";
            custom_sword_4.animated = false;
            custom_sword_4.is_pool_weapon = false;
            custom_sword_4.unlock(true);
            custom_sword_4.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            custom_sword_4.base_stats = new();
            custom_sword_4.base_stats.set(CustomBaseStatsConstant.Damage, 1900f);
            custom_sword_4.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 55f);
            custom_sword_4.base_stats.set(CustomBaseStatsConstant.Speed, 45f);
            custom_sword_4.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.75f);
            custom_sword_4.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.9f);
            custom_sword_4.base_stats[strings.S.damage_range] = 0.6f;
            custom_sword_4.equipment_value = 1900;
            custom_sword_4.quality = Rarity.R3_Legendary;
            custom_sword_4.equipment_type = EquipmentType.Weapon;
            custom_sword_4.name_class = "item_class_weapon";
            custom_sword_4.path_slash_animation = "effects/slashes/slash_sword";
            custom_sword_4.path_icon = $"{PathIcon}/icon_custom_sword_4";
            custom_sword_4.path_gameplay_sprite = $"weapons/custom_sword_4";
            custom_sword_4.gameplay_sprites = getWeaponSprites("custom_sword_4");
            AssetManager.items.list.AddItem(custom_sword_4);
            addToLocale(custom_sword_4.id, custom_sword_4.translation_key, "雷池之水淬火，剑动紫电闪烁！");
            
            // custom_sword_5 - 金麟剑
            ItemAsset custom_sword_5 = AssetManager.items.clone("custom_sword_5", "$weapon");
            custom_sword_5.id = "custom_sword_5";
            custom_sword_5.material = "adamantine";
            custom_sword_5.translation_key = "custom_sword_5";
            custom_sword_5.equipment_subtype = "custom_sword_5";
            custom_sword_5.group_id = "sword";
            custom_sword_5.animated = false;
            custom_sword_5.is_pool_weapon = false;
            custom_sword_5.unlock(true);
            custom_sword_5.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            custom_sword_5.base_stats = new();
            custom_sword_5.base_stats.set(CustomBaseStatsConstant.Damage, 1600f);
            custom_sword_5.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 50f);
            custom_sword_5.base_stats.set(CustomBaseStatsConstant.Speed, 35f);
            custom_sword_5.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.65f);
            custom_sword_5.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.7f);
            custom_sword_5.base_stats[strings.S.damage_range] = 0.6f;
            custom_sword_5.equipment_value = 1600;
            custom_sword_5.quality = Rarity.R3_Legendary;
            custom_sword_5.equipment_type = EquipmentType.Weapon;
            custom_sword_5.name_class = "item_class_weapon";
            custom_sword_5.path_slash_animation = "effects/slashes/slash_sword";
            custom_sword_5.path_icon = $"{PathIcon}/icon_custom_sword_5";
            custom_sword_5.path_gameplay_sprite = $"weapons/custom_sword_5";
            custom_sword_5.gameplay_sprites = getWeaponSprites("custom_sword_5");
            AssetManager.items.list.AddItem(custom_sword_5);
            addToLocale(custom_sword_5.id, custom_sword_5.translation_key, "南海金鳞鱼精之骨所铸，剑身金光流转！");
            
            // custom_sword_6 - 青霜剑
            ItemAsset custom_sword_6 = AssetManager.items.clone("custom_sword_6", "$weapon");
            custom_sword_6.id = "custom_sword_6";
            custom_sword_6.material = "adamantine";
            custom_sword_6.translation_key = "custom_sword_6";
            custom_sword_6.equipment_subtype = "custom_sword_6";
            custom_sword_6.group_id = "sword";
            custom_sword_6.animated = false;
            custom_sword_6.is_pool_weapon = false;
            custom_sword_6.unlock(true);
            custom_sword_6.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            custom_sword_6.base_stats = new();
            custom_sword_6.base_stats.set(CustomBaseStatsConstant.Damage, 1700f);
            custom_sword_6.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 55f);
            custom_sword_6.base_stats.set(CustomBaseStatsConstant.Speed, 40f);
            custom_sword_6.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.7f);
            custom_sword_6.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.8f);
            custom_sword_6.base_stats[strings.S.damage_range] = 0.6f;
            custom_sword_6.equipment_value = 1700;
            custom_sword_6.quality = Rarity.R3_Legendary;
            custom_sword_6.equipment_type = EquipmentType.Weapon;
            custom_sword_6.name_class = "item_class_weapon";
            custom_sword_6.path_slash_animation = "effects/slashes/slash_sword";
            custom_sword_6.path_icon = $"{PathIcon}/icon_custom_sword_6";
            custom_sword_6.path_gameplay_sprite = $"weapons/custom_sword_6";
            custom_sword_6.gameplay_sprites = getWeaponSprites("custom_sword_6");
            AssetManager.items.list.AddItem(custom_sword_6);
            addToLocale(custom_sword_6.id, custom_sword_6.translation_key, "昆仑山寒潭之水淬成，剑出霜气逼人！");
            
            // custom_sword_7 - 赤血刀
            ItemAsset custom_sword_7 = AssetManager.items.clone("custom_sword_7", "$weapon");
            custom_sword_7.id = "custom_sword_7";
            custom_sword_7.material = "adamantine";
            custom_sword_7.translation_key = "custom_sword_7";
            custom_sword_7.equipment_subtype = "custom_sword_7";
            custom_sword_7.group_id = "sword";
            custom_sword_7.animated = false;
            custom_sword_7.is_pool_weapon = false;
            custom_sword_7.unlock(true);
            custom_sword_7.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            custom_sword_7.base_stats = new();
            custom_sword_7.base_stats.set(CustomBaseStatsConstant.Damage, 1800f);
            custom_sword_7.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 45f);
            custom_sword_7.base_stats.set(CustomBaseStatsConstant.Speed, 30f);
            custom_sword_7.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.6f);
            custom_sword_7.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.7f);
            custom_sword_7.base_stats[strings.S.damage_range] = 0.6f;
            custom_sword_7.equipment_value = 1800;
            custom_sword_7.quality = Rarity.R3_Legendary;
            custom_sword_7.equipment_type = EquipmentType.Weapon;
            custom_sword_7.name_class = "item_class_weapon";
            custom_sword_7.path_slash_animation = "effects/slashes/slash_sword";
            custom_sword_7.path_icon = $"{PathIcon}/icon_custom_sword_7";
            custom_sword_7.path_gameplay_sprite = $"weapons/custom_sword_7";
            custom_sword_7.gameplay_sprites = getWeaponSprites("custom_sword_7");
            AssetManager.items.list.AddItem(custom_sword_7);
            addToLocale(custom_sword_7.id, custom_sword_7.translation_key, "血池之地寒铁锻造，刀身饮血变红！");
            
            // custom_sword_8 - 玄冰剑
            ItemAsset custom_sword_8 = AssetManager.items.clone("custom_sword_8", "$weapon");
            custom_sword_8.id = "custom_sword_8";
            custom_sword_8.material = "adamantine";
            custom_sword_8.translation_key = "custom_sword_8";
            custom_sword_8.equipment_subtype = "custom_sword_8";
            custom_sword_8.group_id = "sword";
            custom_sword_8.animated = false;
            custom_sword_8.is_pool_weapon = false;
            custom_sword_8.unlock(true);
            custom_sword_8.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            custom_sword_8.base_stats = new();
            custom_sword_8.base_stats.set(CustomBaseStatsConstant.Damage, 1900f);
            custom_sword_8.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 50f);
            custom_sword_8.base_stats.set(CustomBaseStatsConstant.Speed, 35f);
            custom_sword_8.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.7f);
            custom_sword_8.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.8f);
            custom_sword_8.base_stats[strings.S.damage_range] = 0.6f;
            custom_sword_8.equipment_value = 1900;
            custom_sword_8.quality = Rarity.R3_Legendary;
            custom_sword_8.equipment_type = EquipmentType.Weapon;
            custom_sword_8.name_class = "item_class_weapon";
            custom_sword_8.path_slash_animation = "effects/slashes/slash_sword";
            custom_sword_8.path_icon = $"{PathIcon}/icon_custom_sword_8";
            custom_sword_8.path_gameplay_sprite = $"weapons/custom_sword_8";
            custom_sword_8.gameplay_sprites = getWeaponSprites("custom_sword_8");
            AssetManager.items.list.AddItem(custom_sword_8);
            addToLocale(custom_sword_8.id, custom_sword_8.translation_key, "极北之地玄冰所化，剑寒彻骨！");
            
            // custom_sword_9 - 金龙剑
            ItemAsset custom_sword_9 = AssetManager.items.clone("custom_sword_9", "$weapon");
            custom_sword_9.id = "custom_sword_9";
            custom_sword_9.material = "adamantine";
            custom_sword_9.translation_key = "custom_sword_9";
            custom_sword_9.equipment_subtype = "custom_sword_9";
            custom_sword_9.group_id = "sword";
            custom_sword_9.animated = false;
            custom_sword_9.is_pool_weapon = false;
            custom_sword_9.unlock(true);
            custom_sword_9.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            custom_sword_9.base_stats = new();
            custom_sword_9.base_stats.set(CustomBaseStatsConstant.Damage, 2000f);
            custom_sword_9.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 55f);
            custom_sword_9.base_stats.set(CustomBaseStatsConstant.Speed, 45f);
            custom_sword_9.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.8f);
            custom_sword_9.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.9f);
            custom_sword_9.base_stats[strings.S.damage_range] = 0.6f;
            custom_sword_9.equipment_value = 2000;
            custom_sword_9.quality = Rarity.R3_Legendary;
            custom_sword_9.equipment_type = EquipmentType.Weapon;
            custom_sword_9.name_class = "item_class_weapon";
            custom_sword_9.path_slash_animation = "effects/slashes/slash_sword";
            custom_sword_9.path_icon = $"{PathIcon}/icon_custom_sword_9";
            custom_sword_9.path_gameplay_sprite = $"weapons/custom_sword_9";
            custom_sword_9.gameplay_sprites = getWeaponSprites("custom_sword_9");
            AssetManager.items.list.AddItem(custom_sword_9);
            addToLocale(custom_sword_9.id, custom_sword_9.translation_key, "东海龙宫珍藏，剑身上嵌有金龙！");
            
            // custom_sword_10 - 紫竹剑
            ItemAsset custom_sword_10 = AssetManager.items.clone("custom_sword_10", "$weapon");
            custom_sword_10.id = "custom_sword_10";
            custom_sword_10.material = "adamantine";
            custom_sword_10.translation_key = "custom_sword_10";
            custom_sword_10.equipment_subtype = "custom_sword_10";
            custom_sword_10.group_id = "sword";
            custom_sword_10.animated = false;
            custom_sword_10.is_pool_weapon = false;
            custom_sword_10.unlock(true);
            custom_sword_10.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            custom_sword_10.base_stats = new();
            custom_sword_10.base_stats.set(CustomBaseStatsConstant.Damage, 1600f);
            custom_sword_10.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 60f);
            custom_sword_10.base_stats.set(CustomBaseStatsConstant.Speed, 40f);
            custom_sword_10.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.7f);
            custom_sword_10.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.8f);
            custom_sword_10.base_stats[strings.S.damage_range] = 0.6f;
            custom_sword_10.equipment_value = 1600;
            custom_sword_10.quality = Rarity.R3_Legendary;
            custom_sword_10.equipment_type = EquipmentType.Weapon;
            custom_sword_10.name_class = "item_class_weapon";
            custom_sword_10.path_slash_animation = "effects/slashes/slash_sword";
            custom_sword_10.path_icon = $"{PathIcon}/icon_custom_sword_10";
            custom_sword_10.path_gameplay_sprite = $"weapons/custom_sword_10";
            custom_sword_10.gameplay_sprites = getWeaponSprites("custom_sword_10");
            AssetManager.items.list.AddItem(custom_sword_10);
            addToLocale(custom_sword_10.id, custom_sword_10.translation_key, "南海紫竹林千年紫竹所制，剑风清冽！");
            
            // custom_sword_11 - 青钢剑
            ItemAsset custom_sword_11 = AssetManager.items.clone("custom_sword_11", "$weapon");
            custom_sword_11.id = "custom_sword_11";
            custom_sword_11.material = "adamantine";
            custom_sword_11.translation_key = "custom_sword_11";
            custom_sword_11.equipment_subtype = "custom_sword_11";
            custom_sword_11.group_id = "sword";
            custom_sword_11.animated = false;
            custom_sword_11.is_pool_weapon = false;
            custom_sword_11.unlock(true);
            custom_sword_11.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            custom_sword_11.base_stats = new();
            custom_sword_11.base_stats.set(CustomBaseStatsConstant.Damage, 1700f);
            custom_sword_11.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 55f);
            custom_sword_11.base_stats.set(CustomBaseStatsConstant.Speed, 35f);
            custom_sword_11.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.65f);
            custom_sword_11.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.7f);
            custom_sword_11.base_stats[strings.S.damage_range] = 0.6f;
            custom_sword_11.equipment_value = 1700;
            custom_sword_11.quality = Rarity.R3_Legendary;
            custom_sword_11.equipment_type = EquipmentType.Weapon;
            custom_sword_11.name_class = "item_class_weapon";
            custom_sword_11.path_slash_animation = "effects/slashes/slash_sword";
            custom_sword_11.path_icon = $"{PathIcon}/icon_custom_sword_11";
            custom_sword_11.path_gameplay_sprite = $"weapons/custom_sword_11";
            custom_sword_11.gameplay_sprites = getWeaponSprites("custom_sword_11");
            AssetManager.items.list.AddItem(custom_sword_11);
            addToLocale(custom_sword_11.id, custom_sword_11.translation_key, "庐山青钢石锻造，剑身坚韧无比！");
            
            // custom_sword_12 - 铁骨刀
            ItemAsset custom_sword_12 = AssetManager.items.clone("custom_sword_12", "$weapon");
            custom_sword_12.id = "custom_sword_12";
            custom_sword_12.material = "adamantine";
            custom_sword_12.translation_key = "custom_sword_12";
            custom_sword_12.equipment_subtype = "custom_sword_12";
            custom_sword_12.group_id = "sword";
            custom_sword_12.animated = false;
            custom_sword_12.is_pool_weapon = false;
            custom_sword_12.unlock(true);
            custom_sword_12.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            custom_sword_12.base_stats = new();
            custom_sword_12.base_stats.set(CustomBaseStatsConstant.Damage, 1800f);
            custom_sword_12.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 40f);
            custom_sword_12.base_stats.set(CustomBaseStatsConstant.Speed, 30f);
            custom_sword_12.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.6f);
            custom_sword_12.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.6f);
            custom_sword_12.base_stats[strings.S.damage_range] = 0.6f;
            custom_sword_12.equipment_value = 1800;
            custom_sword_12.quality = Rarity.R3_Legendary;
            custom_sword_12.equipment_type = EquipmentType.Weapon;
            custom_sword_12.name_class = "item_class_weapon";
            custom_sword_12.path_slash_animation = "effects/slashes/slash_sword";
            custom_sword_12.path_icon = $"{PathIcon}/icon_custom_sword_12";
            custom_sword_12.path_gameplay_sprite = $"weapons/custom_sword_12";
            custom_sword_12.gameplay_sprites = getWeaponSprites("custom_sword_12");
            AssetManager.items.list.AddItem(custom_sword_12);
            addToLocale(custom_sword_12.id, custom_sword_12.translation_key, "黑铁精钢打造，刀身厚重，力大无穷！");
            
            // custom_sword_13 - 断云剑
            ItemAsset custom_sword_13 = AssetManager.items.clone("custom_sword_13", "$weapon");
            custom_sword_13.id = "custom_sword_13";
            custom_sword_13.material = "adamantine";
            custom_sword_13.translation_key = "custom_sword_13";
            custom_sword_13.equipment_subtype = "custom_sword_13";
            custom_sword_13.group_id = "sword";
            custom_sword_13.animated = false;
            custom_sword_13.is_pool_weapon = false;
            custom_sword_13.unlock(true);
            custom_sword_13.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            custom_sword_13.base_stats = new();
            custom_sword_13.base_stats.set(CustomBaseStatsConstant.Damage, 2100f);
            custom_sword_13.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 60f);
            custom_sword_13.base_stats.set(CustomBaseStatsConstant.Speed, 50f);
            custom_sword_13.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.85f);
            custom_sword_13.base_stats.set(CustomBaseStatsConstant.CriticalChance, 1.0f);
            custom_sword_13.base_stats[strings.S.damage_range] = 0.6f;
            custom_sword_13.equipment_value = 2100;
            custom_sword_13.quality = Rarity.R3_Legendary;
            custom_sword_13.equipment_type = EquipmentType.Weapon;
            custom_sword_13.name_class = "item_class_weapon";
            custom_sword_13.path_slash_animation = "effects/slashes/slash_sword";
            custom_sword_13.path_icon = $"{PathIcon}/icon_custom_sword_13";
            custom_sword_13.path_gameplay_sprite = $"weapons/custom_sword_13";
            custom_sword_13.gameplay_sprites = getWeaponSprites("custom_sword_13");
            AssetManager.items.list.AddItem(custom_sword_13);
            addToLocale(custom_sword_13.id, custom_sword_13.translation_key, "剑出可断云，威力无穷！");
            
            // custom_sword_14 - 穿云剑
            ItemAsset custom_sword_14 = AssetManager.items.clone("custom_sword_14", "$weapon");
            custom_sword_14.id = "custom_sword_14";
            custom_sword_14.material = "adamantine";
            custom_sword_14.translation_key = "custom_sword_14";
            custom_sword_14.equipment_subtype = "custom_sword_14";
            custom_sword_14.group_id = "sword";
            custom_sword_14.animated = false;
            custom_sword_14.is_pool_weapon = false;
            custom_sword_14.unlock(true);
            custom_sword_14.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            custom_sword_14.base_stats = new();
            custom_sword_14.base_stats.set(CustomBaseStatsConstant.Damage, 2200f);
            custom_sword_14.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 65f);
            custom_sword_14.base_stats.set(CustomBaseStatsConstant.Speed, 55f);
            custom_sword_14.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.9f);
            custom_sword_14.base_stats.set(CustomBaseStatsConstant.CriticalChance, 1.1f);
            custom_sword_14.base_stats[strings.S.damage_range] = 0.6f;
            custom_sword_14.equipment_value = 2200;
            custom_sword_14.quality = Rarity.R3_Legendary;
            custom_sword_14.equipment_type = EquipmentType.Weapon;
            custom_sword_14.name_class = "item_class_weapon";
            custom_sword_14.path_slash_animation = "effects/slashes/slash_sword";
            custom_sword_14.path_icon = $"{PathIcon}/icon_custom_sword_14";
            custom_sword_14.path_gameplay_sprite = $"weapons/custom_sword_14";
            custom_sword_14.gameplay_sprites = getWeaponSprites("custom_sword_14");
            AssetManager.items.list.AddItem(custom_sword_14);
            addToLocale(custom_sword_14.id, custom_sword_14.translation_key, "剑势如穿云，快如闪电！");
            
            // custom_sword_15 - 玄铁重剑
            ItemAsset custom_sword_15 = AssetManager.items.clone("custom_sword_15", "$weapon");
            custom_sword_15.id = "custom_sword_15";
            custom_sword_15.material = "adamantine";
            custom_sword_15.translation_key = "custom_sword_15";
            custom_sword_15.equipment_subtype = "custom_sword_15";
            custom_sword_15.group_id = "sword";
            custom_sword_15.animated = false;
            custom_sword_15.is_pool_weapon = false;
            custom_sword_15.unlock(true);
            custom_sword_15.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            custom_sword_15.base_stats = new();
            custom_sword_15.base_stats.set(CustomBaseStatsConstant.Damage, 2300f);
            custom_sword_15.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 35f);
            custom_sword_15.base_stats.set(CustomBaseStatsConstant.Speed, 25f);
            custom_sword_15.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.55f);
            custom_sword_15.base_stats.set(CustomBaseStatsConstant.CriticalChance, 0.8f);
            custom_sword_15.base_stats[strings.S.damage_range] = 0.6f;
            custom_sword_15.equipment_value = 2300;
            custom_sword_15.quality = Rarity.R3_Legendary;
            custom_sword_15.equipment_type = EquipmentType.Weapon;
            custom_sword_15.name_class = "item_class_weapon";
            custom_sword_15.path_slash_animation = "effects/slashes/slash_sword";
            custom_sword_15.path_icon = $"{PathIcon}/icon_custom_sword_15";
            custom_sword_15.path_gameplay_sprite = $"weapons/custom_sword_15";
            custom_sword_15.gameplay_sprites = getWeaponSprites("custom_sword_15");
            AssetManager.items.list.AddItem(custom_sword_15);
            addToLocale(custom_sword_15.id, custom_sword_15.translation_key, "玄铁铸造，重若泰山，力大无穷！");
            
            // custom_sword_16 - 追风剑
            ItemAsset custom_sword_16 = AssetManager.items.clone("custom_sword_16", "$weapon");
            custom_sword_16.id = "custom_sword_16";
            custom_sword_16.material = "adamantine";
            custom_sword_16.translation_key = "custom_sword_16";
            custom_sword_16.equipment_subtype = "custom_sword_16";
            custom_sword_16.group_id = "sword";
            custom_sword_16.animated = false;
            custom_sword_16.is_pool_weapon = false;
            custom_sword_16.unlock(true);
            custom_sword_16.name_templates = AssetLibrary<EquipmentAsset>.l<string>("flame_sword_name");
            custom_sword_16.base_stats = new();
            custom_sword_16.base_stats.set(CustomBaseStatsConstant.Damage, 1900f);
            custom_sword_16.base_stats.set(CustomBaseStatsConstant.AttackSpeed, 75f);
            custom_sword_16.base_stats.set(CustomBaseStatsConstant.Speed, 65f);
            custom_sword_16.base_stats.set(CustomBaseStatsConstant.Accuracy, 0.95f);
            custom_sword_16.base_stats.set(CustomBaseStatsConstant.CriticalChance, 1.2f);
            custom_sword_16.base_stats[strings.S.damage_range] = 0.6f;
            custom_sword_16.equipment_value = 1900;
            custom_sword_16.quality = Rarity.R3_Legendary;
            custom_sword_16.equipment_type = EquipmentType.Weapon;
            custom_sword_16.name_class = "item_class_weapon";
            custom_sword_16.path_slash_animation = "effects/slashes/slash_sword";
            custom_sword_16.path_icon = $"{PathIcon}/icon_custom_sword_16";
            custom_sword_16.path_gameplay_sprite = $"weapons/custom_sword_16";
            custom_sword_16.gameplay_sprites = getWeaponSprites("custom_sword_16");
            AssetManager.items.list.AddItem(custom_sword_16);
            addToLocale(custom_sword_16.id, custom_sword_16.translation_key, "剑如追风，快若闪电！");
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
                PeerlessOverpoweringWarriorClass.LogError("Can not find weapon sprite for weapon with this id: " + id);
                return Array.Empty<Sprite>();
            }
        }
    }
}