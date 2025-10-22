using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using ReflectionUtility;
using ai;
using System.Numerics;
using strings;

namespace PeerlessOverpoweringWarrior.code
{
    internal class traits
    {
        private static ActorTrait CreateTrait(string id, string path_icon, string group_id)
        {
            ActorTrait trait = new ActorTrait();
            trait.id = id;
            trait.path_icon = path_icon;
            trait.needs_to_be_explored = false;
            trait.group_id = group_id;
            trait.base_stats = new BaseStats();

            return trait;
        }
        public static ActorTrait martialAptitude_AddActorTrait(string id, string pathIcon)
        {
            ActorTrait martialAptitude = new ActorTrait
            {
                id = id,
                path_icon = pathIcon,
                group_id = "MartialFoundations",
                needs_to_be_explored = false
            };
            martialAptitude.action_special_effect += traitAction.Warrior1_effectAction;
            AssetManager.traits.add(martialAptitude);
            return martialAptitude;
        }
        // 添加阵法天赋的方法
        public static ActorTrait Formation_AddActorTrait(string id, string pathIcon, Rarity rarity)
        {
            ActorTrait formation = new ActorTrait
            {
                id = id,
                path_icon = pathIcon,
                group_id = "FormationPatterns", // 新的特质组
                needs_to_be_explored = false,
                rarity = rarity
            };
            // 为阵法天赋添加阵道晋升触发事件，与武道晋升机制保持一致
            formation.action_special_effect += traitAction.FormationRealm1_effectAction;
            AssetManager.traits.add(formation);
            return formation;
        }

        // 创建阵法特质的辅助方法
        public static ActorTrait CreateFormationSkill(string id, string path_icon, Rarity rarity)
        {
            ActorTrait trait = new ActorTrait();
            trait.id = id;
            trait.path_icon = path_icon;
            trait.needs_to_be_explored = false;
            trait.group_id = "FormationSkills"; // 新的阵法特质组
            trait.rarity = rarity;
            trait.base_stats = new BaseStats();
            return trait;
        }

        public static void Init()
        {
            // 添加阵法天赋
            _=Formation_AddActorTrait("formation1", "trait/formation1", Rarity.R1_Rare); // 下等阵法天赋
            _=Formation_AddActorTrait("formation2", "trait/formation2", Rarity.R1_Rare); // 中等阵法天赋
            _=Formation_AddActorTrait("formation3", "trait/formation3", Rarity.R2_Epic);   // 上等阵法天赋
            _=Formation_AddActorTrait("formation4", "trait/formation4", Rarity.R3_Legendary); // 绝顶阵法天赋
            
            // 添加阵法特质 - 第一重：聚气阵
            ActorTrait FormationSkill1 = CreateFormationSkill("FormationSkill1", "trait/FormationSkill1", Rarity.R1_Rare);
            SafeSetStat(FormationSkill1.base_stats, strings.S.damage, 450f);
            SafeSetStat(FormationSkill1.base_stats, strings.S.health, 2000f);
            SafeSetStat(FormationSkill1.base_stats, strings.S.armor, 16f);
            SafeSetStat(FormationSkill1.base_stats, strings.S.speed, 50f);
            SafeSetStat(FormationSkill1.base_stats, strings.S.stamina, 200f);
            SafeSetStat(FormationSkill1.base_stats, strings.S.area_of_effect, 12f);
            SafeSetStat(FormationSkill1.base_stats, strings.S.targets, 16f);
            SafeSetStat(FormationSkill1.base_stats, strings.S.multiplier_damage, 0.24f);
            SafeSetStat(FormationSkill1.base_stats, strings.S.multiplier_health, 0.24f);
            SafeSetStat(FormationSkill1.base_stats, strings.S.multiplier_speed, 0.24f);
            SafeSetStat(FormationSkill1.base_stats, strings.S.attack_speed, 2.0f);
            FormationSkill1.action_attack_target += traitAction.FormationSkill1_AttackAction;
            AssetManager.traits.add(FormationSkill1);
            
            // 添加阵法特质 - 第二重：防御阵
            ActorTrait FormationSkill2 = CreateFormationSkill("FormationSkill2", "trait/FormationSkill2", Rarity.R1_Rare);
            SafeSetStat(FormationSkill2.base_stats, strings.S.damage, 500f);
            SafeSetStat(FormationSkill2.base_stats, strings.S.health, 4000f);
            SafeSetStat(FormationSkill2.base_stats, strings.S.armor, 18f);
            SafeSetStat(FormationSkill2.base_stats, strings.S.speed, 60f);
            SafeSetStat(FormationSkill2.base_stats, strings.S.stamina, 3000f);
            SafeSetStat(FormationSkill2.base_stats, strings.S.area_of_effect, 16f);
            SafeSetStat(FormationSkill2.base_stats, strings.S.targets, 18f);
            SafeSetStat(FormationSkill2.base_stats, strings.S.multiplier_damage, 0.28f);
            SafeSetStat(FormationSkill2.base_stats, strings.S.multiplier_health, 0.28f);
            SafeSetStat(FormationSkill2.base_stats, strings.S.multiplier_speed, 0.28f);
            SafeSetStat(FormationSkill2.base_stats, strings.S.attack_speed, 2.4f);
            FormationSkill2.action_attack_target += traitAction.FormationSkill2_AttackAction;
            AssetManager.traits.add(FormationSkill2);
            
            // 添加阵法特质 - 第三重：攻击阵
            ActorTrait FormationSkill3 = CreateFormationSkill("FormationSkill3", "trait/FormationSkill3", Rarity.R2_Epic);
            SafeSetStat(FormationSkill3.base_stats, strings.S.damage, 600f);
            SafeSetStat(FormationSkill3.base_stats, strings.S.health, 2800f);
            SafeSetStat(FormationSkill3.base_stats, strings.S.armor, 20f);
            SafeSetStat(FormationSkill3.base_stats, strings.S.speed, 80f);
            SafeSetStat(FormationSkill3.base_stats, strings.S.area_of_effect, 18f);
            SafeSetStat(FormationSkill3.base_stats, strings.S.targets, 20f);
            SafeSetStat(FormationSkill3.base_stats, strings.S.multiplier_damage, 0.3f);
            SafeSetStat(FormationSkill3.base_stats, strings.S.multiplier_health, 0.3f);
            SafeSetStat(FormationSkill3.base_stats, strings.S.multiplier_speed, 0.3f);
            SafeSetStat(FormationSkill3.base_stats, strings.S.attack_speed, 2.8f);
            FormationSkill3.action_attack_target += traitAction.FormationSkill3_AttackAction;
            FormationSkill3.action_attack_target += traitAction.Baiguang_AttackAction;
            AssetManager.traits.add(FormationSkill3);
            
            // 添加阵法特质 - 第四重：增幅阵
            ActorTrait FormationSkill4 = CreateFormationSkill("FormationSkill4", "trait/FormationSkill4", Rarity.R2_Epic);
            SafeSetStat(FormationSkill4.base_stats, strings.S.damage, 700f);
            SafeSetStat(FormationSkill4.base_stats, strings.S.health, 3000f);
            SafeSetStat(FormationSkill4.base_stats, strings.S.armor, 24f);
            SafeSetStat(FormationSkill4.base_stats, strings.S.speed, 90f);
            SafeSetStat(FormationSkill4.base_stats, strings.S.targets, 24f);
            SafeSetStat(FormationSkill4.base_stats, strings.S.area_of_effect, 20f);
            SafeSetStat(FormationSkill4.base_stats, strings.S.multiplier_damage, 0.32f);
            SafeSetStat(FormationSkill4.base_stats, strings.S.multiplier_health, 0.32f);
            SafeSetStat(FormationSkill4.base_stats, strings.S.multiplier_speed, 0.32f);
            SafeSetStat(FormationSkill4.base_stats, strings.S.attack_speed, 3.2f);
            FormationSkill4.action_attack_target += traitAction.liuxing_attackAction;
            FormationSkill4.action_attack_target += traitAction.FormationSkill4_AttackAction;
            FormationSkill4.action_attack_target += traitAction.XingdouJimie_AttackAction;
            AssetManager.traits.add(FormationSkill4);
            
            // 添加阵法特质 - 第五重：混元阵
            ActorTrait FormationSkill5 = CreateFormationSkill("FormationSkill5", "trait/FormationSkill5", Rarity.R3_Legendary);
            SafeSetStat(FormationSkill5.base_stats, strings.S.damage, 800f);
            SafeSetStat(FormationSkill5.base_stats, strings.S.health, 3200f);
            SafeSetStat(FormationSkill5.base_stats, strings.S.armor, 25f);
            SafeSetStat(FormationSkill5.base_stats, strings.S.speed, 100f);
            SafeSetStat(FormationSkill5.base_stats, strings.S.area_of_effect, 24f);
            SafeSetStat(FormationSkill5.base_stats, strings.S.targets, 30f);
            SafeSetStat(FormationSkill5.base_stats, strings.S.multiplier_damage, 0.4f);
            SafeSetStat(FormationSkill5.base_stats, strings.S.multiplier_health, 0.4f);
            SafeSetStat(FormationSkill5.base_stats, strings.S.multiplier_speed, 0.4f);
            SafeSetStat(FormationSkill5.base_stats, strings.S.critical_chance, 0.4f);
            SafeSetStat(FormationSkill5.base_stats, strings.S.attack_speed, 3.6f);
            FormationSkill5.action_attack_target += traitAction.FormationSkill5_AttackAction;
            FormationSkill5.action_attack_target += traitAction.WanJieLunHui_AttackAction;
            AssetManager.traits.add(FormationSkill5);

            // 添加阵道境界特质
            // 阵道第一重 - 对应武道基础境界
            ActorTrait FormationRealm1 = CreateTrait("FormationRealm1", "trait/FormationRealm1", "FormationRealms");
            FormationRealm1.rarity = Rarity.R1_Rare;
            SafeSetStat(FormationRealm1.base_stats, strings.S.damage, 200f);
            SafeSetStat(FormationRealm1.base_stats, strings.S.health, 1000f);
            SafeSetStat(FormationRealm1.base_stats, strings.S.armor, 5f);
            SafeSetStat(FormationRealm1.base_stats, strings.S.speed, 5f);
            SafeSetStat(FormationRealm1.base_stats, strings.S.accuracy, 10f);
            SafeSetStat(FormationRealm1.base_stats, strings.S.stamina, 30f);
            SafeSetStat(FormationRealm1.base_stats, strings.S.lifespan, 10f);
            SafeSetStat(FormationRealm1.base_stats, strings.S.multiplier_damage, 0.1f);
            SafeSetStat(FormationRealm1.base_stats, strings.S.multiplier_health, 0.1f);
            SafeSetStat(FormationRealm1.base_stats, strings.S.attack_speed, 1.2f);
            SafeSetStat(FormationRealm1.base_stats, strings.S.critical_chance, 0.1f);
            SafeSetStat(FormationRealm1.base_stats, strings.S.multiplier_stamina, 0.3f);
            SafeSetStat(FormationRealm1.base_stats, strings.S.area_of_effect, 5f);
            SafeSetStat(FormationRealm1.base_stats, strings.S.targets, 4f);
            SafeSetStat(FormationRealm1.base_stats, strings.S.warfare, 10f);
            SafeSetStat(FormationRealm1.base_stats, strings.S.diplomacy, 2f);
            SafeSetStat(FormationRealm1.base_stats, strings.S.stewardship, 6f);
            SafeSetStat(FormationRealm1.base_stats, strings.S.opinion, 8f);
            SafeSetStat(FormationRealm1.base_stats, strings.S.cities, 6f);
            SafeSetStat(FormationRealm1.base_stats, stats.Resist.id, 1.0f);
            SafeSetStat(FormationRealm1.base_stats, "Dodge", 120f);
            SafeSetStat(FormationRealm1.base_stats, "Accuracy", 100f);
            // 添加回血效果
            FormationRealm1.action_special_effect += traitAction.FormationRealm1_Regen;
            // 绑定基于阵纹的真伤攻击效果
            FormationRealm1.action_attack_target += traitAction.FormationPatternTrueDamage1_AttackAction;
            // 绑定基于攻击值的真伤攻击效果
            FormationRealm1.action_attack_target += traitAction.FormationDamageTrueDamage1_AttackAction;
            // 绑定第二重境界晋升效果
            FormationRealm1.action_special_effect += traitAction.FormationRealm2_effectAction;
            AssetManager.traits.add(FormationRealm1);

            // 阵道第二重 - 对应武道第四/第五重境界
            ActorTrait FormationRealm2 = CreateTrait("FormationRealm2", "trait/FormationRealm2", "FormationRealms");
            FormationRealm2.rarity = Rarity.R1_Rare;
            SafeSetStat(FormationRealm2.base_stats, strings.S.damage, 400f);
            SafeSetStat(FormationRealm2.base_stats, strings.S.health, 4000f);
            SafeSetStat(FormationRealm2.base_stats, strings.S.armor, 10f);
            SafeSetStat(FormationRealm2.base_stats, strings.S.speed, 10f);
            SafeSetStat(FormationRealm2.base_stats, strings.S.critical_chance, 0.2f);
            SafeSetStat(FormationRealm2.base_stats, strings.S.accuracy, 20f);
            SafeSetStat(FormationRealm2.base_stats, strings.S.multiplier_speed, 0.3f);
            SafeSetStat(FormationRealm2.base_stats, strings.S.stamina, 50f);
            SafeSetStat(FormationRealm2.base_stats, strings.S.lifespan, 30f);
            SafeSetStat(FormationRealm2.base_stats, strings.S.multiplier_damage, 0.2f);
            SafeSetStat(FormationRealm2.base_stats, strings.S.multiplier_health, 0.2f);
            SafeSetStat(FormationRealm2.base_stats, strings.S.attack_speed, 2.4f);
            SafeSetStat(FormationRealm2.base_stats, strings.S.multiplier_stamina, 0.4f);
            SafeSetStat(FormationRealm2.base_stats, strings.S.area_of_effect, 10f);
            SafeSetStat(FormationRealm2.base_stats, strings.S.targets, 5f);
            SafeSetStat(FormationRealm2.base_stats, strings.S.warfare, 20f);
            SafeSetStat(FormationRealm2.base_stats, strings.S.diplomacy, 4f);
            SafeSetStat(FormationRealm2.base_stats, strings.S.stewardship, 12f);
            SafeSetStat(FormationRealm2.base_stats, strings.S.opinion, 16f);
            SafeSetStat(FormationRealm2.base_stats, strings.S.cities, 12f);
            SafeSetStat(FormationRealm2.base_stats, stats.Resist.id, 4.0f);
            SafeSetStat(FormationRealm2.base_stats, "Dodge", 150f);
            SafeSetStat(FormationRealm2.base_stats, "Accuracy", 110f);
            // 添加回血效果
            FormationRealm2.action_special_effect += traitAction.FormationRealm2_Regen;
            // 绑定基于阵纹的真伤攻击效果
            FormationRealm2.action_attack_target += traitAction.FormationPatternTrueDamage2_AttackAction;
            // 绑定基于攻击值的真伤攻击效果
            FormationRealm2.action_attack_target += traitAction.FormationDamageTrueDamage2_AttackAction;
            // 绑定第三重境界晋升效果
            FormationRealm2.action_special_effect += traitAction.FormationRealm3_effectAction;
            AssetManager.traits.add(FormationRealm2);

            // 阵道第三重 - 对应武道第六重境界
            ActorTrait FormationRealm3 = CreateTrait("FormationRealm3", "trait/FormationRealm3", "FormationRealms");
            FormationRealm3.rarity = Rarity.R2_Epic;
            SafeSetStat(FormationRealm3.base_stats, strings.S.damage, 1000f);
            SafeSetStat(FormationRealm3.base_stats, strings.S.health, 10000f);
            SafeSetStat(FormationRealm3.base_stats, strings.S.armor, 25f);
            SafeSetStat(FormationRealm3.base_stats, strings.S.speed, 15f);
            SafeSetStat(FormationRealm3.base_stats, strings.S.area_of_effect, 10f);
            SafeSetStat(FormationRealm3.base_stats, strings.S.targets, 8f);
            SafeSetStat(FormationRealm3.base_stats, strings.S.critical_chance, 0.5f);
            SafeSetStat(FormationRealm3.base_stats, strings.S.accuracy, 30f);
            SafeSetStat(FormationRealm3.base_stats, strings.S.multiplier_speed, 0.6f);
            SafeSetStat(FormationRealm3.base_stats, strings.S.stamina, 70f);
            SafeSetStat(FormationRealm3.base_stats, strings.S.lifespan, 60f);
            SafeSetStat(FormationRealm3.base_stats, strings.S.multiplier_damage, 0.3f);
            SafeSetStat(FormationRealm3.base_stats, strings.S.multiplier_health, 0.3f);
            SafeSetStat(FormationRealm3.base_stats, strings.S.attack_speed, 4.8f);
            SafeSetStat(FormationRealm3.base_stats, strings.S.multiplier_stamina, 0.5f);
            SafeSetStat(FormationRealm3.base_stats, strings.S.warfare, 40f);
            SafeSetStat(FormationRealm3.base_stats, strings.S.diplomacy, 8f);
            SafeSetStat(FormationRealm3.base_stats, strings.S.stewardship, 24f);
            SafeSetStat(FormationRealm3.base_stats, strings.S.opinion, 32f);
            SafeSetStat(FormationRealm3.base_stats, strings.S.cities, 24f);
            SafeSetStat(FormationRealm3.base_stats, stats.Resist.id, 16.0f);
            SafeSetStat(FormationRealm3.base_stats, "Dodge", 200f);
            SafeSetStat(FormationRealm3.base_stats, "Accuracy", 160f);
            // 添加回血效果
            FormationRealm3.action_special_effect += traitAction.FormationRealm3_Regen;
            // 绑定基于阵纹的真伤攻击效果
            FormationRealm3.action_attack_target += traitAction.FormationPatternTrueDamage3_AttackAction;
            // 绑定基于攻击值的真伤攻击效果
            FormationRealm3.action_attack_target += traitAction.FormationDamageTrueDamage3_AttackAction;
            // 绑定第四重境界晋升效果
            FormationRealm3.action_special_effect += traitAction.FormationRealm4_effectAction;
            AssetManager.traits.add(FormationRealm3);

            // 阵道第四重 - 对应武道第七/第八重境界
            ActorTrait FormationRealm4 = CreateTrait("FormationRealm4", "trait/FormationRealm4", "FormationRealms");
            FormationRealm4.rarity = Rarity.R2_Epic;
            SafeSetStat(FormationRealm4.base_stats, strings.S.damage, 2400f);
            SafeSetStat(FormationRealm4.base_stats, strings.S.health, 30000f);
            SafeSetStat(FormationRealm4.base_stats, strings.S.armor, 45f);
            SafeSetStat(FormationRealm4.base_stats, strings.S.speed, 25f);
            SafeSetStat(FormationRealm4.base_stats, strings.S.area_of_effect, 30f);
            SafeSetStat(FormationRealm4.base_stats, strings.S.targets, 15f);
            SafeSetStat(FormationRealm4.base_stats, strings.S.critical_chance, 0.7f);
            SafeSetStat(FormationRealm4.base_stats, strings.S.accuracy, 50f);
            SafeSetStat(FormationRealm4.base_stats, strings.S.multiplier_speed, 1.0f);
            SafeSetStat(FormationRealm4.base_stats, strings.S.stamina, 240f);
            SafeSetStat(FormationRealm4.base_stats, strings.S.multiplier_health, 0.4f);
            SafeSetStat(FormationRealm4.base_stats, strings.S.multiplier_damage, 0.4f);
            SafeSetStat(FormationRealm4.base_stats, strings.S.lifespan, 120f);
            SafeSetStat(FormationRealm4.base_stats, strings.S.attack_speed, 10f);
            SafeSetStat(FormationRealm4.base_stats, strings.S.multiplier_stamina, 0.6f);
            SafeSetStat(FormationRealm4.base_stats, strings.S.warfare, 80f);
            SafeSetStat(FormationRealm4.base_stats, strings.S.diplomacy, 16f);
            SafeSetStat(FormationRealm4.base_stats, strings.S.stewardship, 48f);
            SafeSetStat(FormationRealm4.base_stats, strings.S.opinion, 64f);
            SafeSetStat(FormationRealm4.base_stats, strings.S.cities, 48f);
            SafeSetStat(FormationRealm4.base_stats, stats.Resist.id, 50.0f);
            SafeSetStat(FormationRealm4.base_stats, "Dodge", 220f);
            SafeSetStat(FormationRealm4.base_stats, "Accuracy", 180f);
            // 添加回血效果
            FormationRealm4.action_special_effect += traitAction.FormationRealm4_Regen;
            // 绑定基于阵纹的真伤攻击效果（最强级别）
            FormationRealm4.action_attack_target += traitAction.FormationPatternTrueDamage4_AttackAction;
            // 绑定基于攻击值的真伤攻击效果（最强级别）
            FormationRealm4.action_attack_target += traitAction.FormationDamageTrueDamage4_AttackAction;
            // 绑定第五重境界晋升效果
            FormationRealm4.action_special_effect += traitAction.FormationRealm5_effectAction;
            AssetManager.traits.add(FormationRealm4);

            // 阵道第五重 - 对应武道第九重境界
            ActorTrait FormationRealm5 = CreateTrait("FormationRealm5", "trait/FormationRealm5", "FormationRealms");
            FormationRealm5.rarity = Rarity.R2_Epic;
            SafeSetStat(FormationRealm5.base_stats, strings.S.damage, 12000f);
            SafeSetStat(FormationRealm5.base_stats, strings.S.health, 240000f);
            SafeSetStat(FormationRealm5.base_stats, strings.S.armor, 80f);
            SafeSetStat(FormationRealm5.base_stats, strings.S.speed, 100f);
            SafeSetStat(FormationRealm5.base_stats, strings.S.area_of_effect, 50f);
            SafeSetStat(FormationRealm5.base_stats, strings.S.targets, 30f);
            SafeSetStat(FormationRealm5.base_stats, strings.S.critical_chance, 1.8f);
            SafeSetStat(FormationRealm5.base_stats, strings.S.accuracy, 160f);
            SafeSetStat(FormationRealm5.base_stats, strings.S.multiplier_speed, 1.5f);
            SafeSetStat(FormationRealm5.base_stats, strings.S.stamina, 1200f);
            SafeSetStat(FormationRealm5.base_stats, strings.S.multiplier_health, 1.2f);
            SafeSetStat(FormationRealm5.base_stats, strings.S.multiplier_damage, 1.2f);
            SafeSetStat(FormationRealm5.base_stats, strings.S.lifespan, 200f);
            SafeSetStat(FormationRealm5.base_stats, strings.S.attack_speed, 24f);
            SafeSetStat(FormationRealm5.base_stats, strings.S.multiplier_stamina, 0.7f);
            SafeSetStat(FormationRealm5.base_stats, strings.S.warfare, 160f);
            SafeSetStat(FormationRealm5.base_stats, strings.S.diplomacy, 32f);
            SafeSetStat(FormationRealm5.base_stats, strings.S.stewardship, 96f);
            SafeSetStat(FormationRealm5.base_stats, strings.S.opinion, 128f);
            SafeSetStat(FormationRealm5.base_stats, strings.S.cities, 96f);
            SafeSetStat(FormationRealm5.base_stats, stats.Resist.id, 108.0f);
            SafeSetStat(FormationRealm5.base_stats, "Dodge", 320f);
            SafeSetStat(FormationRealm5.base_stats, "Accuracy", 280f);
            // 添加回血效果
            FormationRealm5.action_special_effect += traitAction.FormationRealm5_Regen;
            // 绑定基于阵纹的真伤攻击效果（最强级别）
            FormationRealm5.action_attack_target += traitAction.FormationPatternTrueDamage5_AttackAction;
            // 绑定基于攻击值的真伤攻击效果（最强级别）
            FormationRealm5.action_attack_target += traitAction.FormationDamageTrueDamage5_AttackAction;
            // 绑定第六重境界晋升效果
            FormationRealm5.action_special_effect += traitAction.FormationRealm6_effectAction;
            AssetManager.traits.add(FormationRealm5);

            // 阵道第六重 - 对应武道最后一重境界(Warrior93)
            ActorTrait FormationRealm6 = CreateTrait("FormationRealm6", "trait/FormationRealm6", "FormationRealms");
            FormationRealm6.rarity = Rarity.R3_Legendary;
            SafeSetStat(FormationRealm6.base_stats, strings.S.damage, 150000f);
            SafeSetStat(FormationRealm6.base_stats, strings.S.health, 2400000f);
            SafeSetStat(FormationRealm6.base_stats, strings.S.armor, 160f);
            SafeSetStat(FormationRealm6.base_stats, strings.S.speed, 300f);
            SafeSetStat(FormationRealm6.base_stats, strings.S.area_of_effect, 100f);
            SafeSetStat(FormationRealm6.base_stats, strings.S.targets, 50f);
            SafeSetStat(FormationRealm6.base_stats, strings.S.critical_chance, 3.0f);
            SafeSetStat(FormationRealm6.base_stats, strings.S.accuracy, 360f);
            SafeSetStat(FormationRealm6.base_stats, strings.S.multiplier_speed, 3.0f);
            SafeSetStat(FormationRealm6.base_stats, strings.S.stamina, 10000f);
            SafeSetStat(FormationRealm6.base_stats, strings.S.multiplier_health, 3f);
            SafeSetStat(FormationRealm6.base_stats, strings.S.multiplier_damage, 3f);
            SafeSetStat(FormationRealm6.base_stats, strings.S.lifespan, 300f);
            SafeSetStat(FormationRealm6.base_stats, strings.S.attack_speed, 48f);
            SafeSetStat(FormationRealm6.base_stats, strings.S.multiplier_stamina, 0.8f);
            SafeSetStat(FormationRealm6.base_stats, strings.S.warfare, 320f);
            SafeSetStat(FormationRealm6.base_stats, strings.S.diplomacy, 64f);
            SafeSetStat(FormationRealm6.base_stats, strings.S.stewardship, 192f);
            SafeSetStat(FormationRealm6.base_stats, strings.S.opinion, 256f);
            SafeSetStat(FormationRealm6.base_stats, strings.S.cities, 192f);
            SafeSetStat(FormationRealm6.base_stats, stats.Resist.id, 240.0f);
            SafeSetStat(FormationRealm6.base_stats, "Dodge", 500f);
            SafeSetStat(FormationRealm6.base_stats, "Accuracy", 480f);
            FormationRealm6.action_special_effect += traitAction.MaintainFullNutrition;
            // 添加回血效果
            FormationRealm6.action_special_effect += traitAction.FormationRealm6_Regen;
            // 绑定基于阵纹的真伤攻击效果（最强级别）
            FormationRealm6.action_attack_target += traitAction.FormationPatternTrueDamage6_AttackAction;
            // 绑定基于攻击值的真伤攻击效果（最强级别）
            FormationRealm6.action_attack_target += traitAction.FormationDamageTrueDamage6_AttackAction;
            AssetManager.traits.add(FormationRealm6);

            ActorTrait martialAptitude4 = martialAptitude_AddActorTrait("martialAptitude4", "trait/martialAptitude4");
            martialAptitude4.rarity = Rarity.R2_Epic;

            ActorTrait martialAptitude7 = martialAptitude_AddActorTrait("martialAptitude7", "trait/martialAptitude7");
            martialAptitude7.rarity = Rarity.R2_Epic;
        
            ActorTrait martialAptitude8 = martialAptitude_AddActorTrait("martialAptitude8", "trait/martialAptitude8");
            martialAptitude8.rarity = Rarity.R3_Legendary;

            _=martialAptitude_AddActorTrait("martialAptitude1", "trait/martialAptitude1");
            _=martialAptitude_AddActorTrait("martialAptitude2", "trait/martialAptitude2");
            _=martialAptitude_AddActorTrait("martialAptitude3", "trait/martialAptitude3");

            ActorTrait martialAptitude9 = CreateTrait("martialAptitude9", "trait/martialAptitude9", "MartialFoundations");
            martialAptitude9.rate_inherit = 100;
            AssetManager.traits.add(martialAptitude9);

            ActorTrait martialAptitude10 = CreateTrait("martialAptitude10", "trait/martialAptitude10", "MartialFoundations");
            SafeSetStat(martialAptitude10.base_stats, strings.S.multiplier_health, -0.15f);
            SafeSetStat(martialAptitude10.base_stats, strings.S.multiplier_damage, -0.2f);
            SafeSetStat(martialAptitude10.base_stats, strings.S.multiplier_speed, -0.25f);
            AssetManager.traits.add(martialAptitude10);

            ActorTrait ancientMartialBody1 = CreateTrait("ancientMartialBody1", "trait/ancientMartialBody1", "AncientMartialBodies");
            ancientMartialBody1.rarity = Rarity.R3_Legendary;
            SafeSetStat(ancientMartialBody1.base_stats, strings.S.speed, 50f);
            SafeSetStat(ancientMartialBody1.base_stats, strings.S.armor, 50f);
            SafeSetStat(ancientMartialBody1.base_stats, strings.S.health, 5000f);
            SafeSetStat(ancientMartialBody1.base_stats, strings.S.damage, 5000f); 
            SafeSetStat(ancientMartialBody1.base_stats, strings.S.lifespan, 1000f);
            SafeSetStat(ancientMartialBody1.base_stats, strings.S.stamina, 1000f);
            SafeSetStat(ancientMartialBody1.base_stats, strings.S.attack_speed, 200f);
            SafeSetStat(ancientMartialBody1.base_stats, strings.S.multiplier_health, 6f);
            SafeSetStat(ancientMartialBody1.base_stats, strings.S.multiplier_damage, 6f);
            SafeSetStat(ancientMartialBody1.base_stats, strings.S.multiplier_speed, 6f);
            ancientMartialBody1.action_attack_target += traitAction.MartialGodTrueDamage_AttackAction;
            AssetManager.traits.add(ancientMartialBody1);

            ActorTrait ancientMartialBody2 = CreateTrait("ancientMartialBody2", "trait/ancientMartialBody2", "AncientMartialBodies");
            ancientMartialBody2.rarity = Rarity.R2_Epic;
            SafeSetStat(ancientMartialBody2.base_stats, strings.S.speed, 10f);
            SafeSetStat(ancientMartialBody2.base_stats, strings.S.armor, 10f);
            SafeSetStat(ancientMartialBody2.base_stats, strings.S.health, 500f);
            SafeSetStat(ancientMartialBody2.base_stats, strings.S.damage, 600f); 
            SafeSetStat(ancientMartialBody2.base_stats, strings.S.lifespan, 20f);
            SafeSetStat(ancientMartialBody2.base_stats, strings.S.stamina, 200f);
            SafeSetStat(ancientMartialBody2.base_stats, strings.S.attack_speed, 20f);
            SafeSetStat(ancientMartialBody2.base_stats, strings.S.multiplier_health, 0.5f);
            SafeSetStat(ancientMartialBody2.base_stats, strings.S.multiplier_damage, 0.25f);
            SafeSetStat(ancientMartialBody2.base_stats, strings.S.multiplier_speed, 0.15f);
            ancientMartialBody2.action_attack_target += traitAction.XingChenTrueDamage_AttackAction;
            AssetManager.traits.add(ancientMartialBody2);

            ActorTrait ancientMartialBody3 = CreateTrait("ancientMartialBody3", "trait/ancientMartialBody3", "AncientMartialBodies");
            ancientMartialBody3.rarity = Rarity.R2_Epic;
            SafeSetStat(ancientMartialBody3.base_stats, strings.S.speed, 20f);
            SafeSetStat(ancientMartialBody3.base_stats, strings.S.armor, 15f);
            SafeSetStat(ancientMartialBody3.base_stats, strings.S.health, 800f);
            SafeSetStat(ancientMartialBody3.base_stats, strings.S.damage, 800f); 
            SafeSetStat(ancientMartialBody3.base_stats, strings.S.lifespan, 50f);
            SafeSetStat(ancientMartialBody3.base_stats, strings.S.stamina, 100f);
            SafeSetStat(ancientMartialBody3.base_stats, strings.S.attack_speed, 10f);
            SafeSetStat(ancientMartialBody3.base_stats, strings.S.multiplier_health, 0.15f);
            SafeSetStat(ancientMartialBody3.base_stats, strings.S.multiplier_damage, 0.25f);
            SafeSetStat(ancientMartialBody3.base_stats, strings.S.multiplier_speed, 0.45f);
            ancientMartialBody3.action_attack_target += traitAction.HunDunTrueDamage_AttackAction;
            AssetManager.traits.add(ancientMartialBody3);

            ActorTrait ancientMartialBody4 = CreateTrait("ancientMartialBody4", "trait/ancientMartialBody4", "AncientMartialBodies");
            ancientMartialBody4.rarity = Rarity.R3_Legendary;
            SafeSetStat(ancientMartialBody4.base_stats, strings.S.speed, 40f);
            SafeSetStat(ancientMartialBody4.base_stats, strings.S.armor, 40f);
            SafeSetStat(ancientMartialBody4.base_stats, strings.S.health, 4000f);
            SafeSetStat(ancientMartialBody4.base_stats, strings.S.damage, 6000f); 
            SafeSetStat(ancientMartialBody4.base_stats, strings.S.lifespan, 500f);
            SafeSetStat(ancientMartialBody4.base_stats, strings.S.stamina,500f);
            SafeSetStat(ancientMartialBody4.base_stats, strings.S.attack_speed, 100f);
            SafeSetStat(ancientMartialBody4.base_stats, strings.S.multiplier_health, 5f);
            SafeSetStat(ancientMartialBody4.base_stats, strings.S.multiplier_damage, 4f);
            SafeSetStat(ancientMartialBody4.base_stats, strings.S.multiplier_speed, 2f);
            ancientMartialBody4.action_attack_target += traitAction.ManLongTrueDamage_AttackAction;
            AssetManager.traits.add(ancientMartialBody4);

            ActorTrait ancientMartialBody5 = CreateTrait("ancientMartialBody5", "trait/ancientMartialBody5", "AncientMartialBodies");
            ancientMartialBody5.rarity = Rarity.R2_Epic;
            SafeSetStat(ancientMartialBody5.base_stats, strings.S.speed, 20f);
            SafeSetStat(ancientMartialBody5.base_stats, strings.S.armor, 30f);
            SafeSetStat(ancientMartialBody5.base_stats, strings.S.health, 1000f);
            SafeSetStat(ancientMartialBody5.base_stats, strings.S.damage, 1000f); 
            SafeSetStat(ancientMartialBody5.base_stats, strings.S.lifespan, 120f);
            SafeSetStat(ancientMartialBody5.base_stats, strings.S.stamina, 10f);
            SafeSetStat(ancientMartialBody5.base_stats, strings.S.attack_speed, 10f);
            SafeSetStat(ancientMartialBody5.base_stats, strings.S.multiplier_health, 0.2f);
            SafeSetStat(ancientMartialBody5.base_stats, strings.S.multiplier_damage, 0.3f);
            SafeSetStat(ancientMartialBody5.base_stats, strings.S.multiplier_speed, 0.56f);
            ancientMartialBody5.action_attack_target += traitAction.YunXingTrueDamage_AttackAction;
            AssetManager.traits.add(ancientMartialBody5);

            ActorTrait ancientMartialBody6 = CreateTrait("ancientMartialBody6", "trait/ancientMartialBody6", "AncientMartialBodies");
            ancientMartialBody6.rarity = Rarity.R2_Epic;
            SafeSetStat(ancientMartialBody6.base_stats, strings.S.speed, 10f);
            SafeSetStat(ancientMartialBody6.base_stats, strings.S.armor, 10f);
            SafeSetStat(ancientMartialBody6.base_stats, strings.S.health, 600f);
            SafeSetStat(ancientMartialBody6.base_stats, strings.S.damage, 600f); 
            SafeSetStat(ancientMartialBody6.base_stats, strings.S.lifespan, 80f);
            SafeSetStat(ancientMartialBody6.base_stats, strings.S.stamina, 100f);
            SafeSetStat(ancientMartialBody6.base_stats, strings.S.attack_speed, 20f);
            SafeSetStat(ancientMartialBody6.base_stats, strings.S.multiplier_health, 0.15f);
            SafeSetStat(ancientMartialBody6.base_stats, strings.S.multiplier_damage, 0.6f);
            SafeSetStat(ancientMartialBody6.base_stats, strings.S.multiplier_speed, 0.3f);
            ancientMartialBody6.action_attack_target += traitAction.WanZhanTrueDamage_AttackAction;
            AssetManager.traits.add(ancientMartialBody6);

            ActorTrait ancientMartialBody7 = CreateTrait("ancientMartialBody7", "trait/ancientMartialBody7", "AncientMartialBodies");
            ancientMartialBody7.rarity = Rarity.R2_Epic;
            SafeSetStat(ancientMartialBody7.base_stats, strings.S.speed, 20f);
            SafeSetStat(ancientMartialBody7.base_stats, strings.S.armor, 30f);
            SafeSetStat(ancientMartialBody7.base_stats, strings.S.health, 500f);
            SafeSetStat(ancientMartialBody7.base_stats, strings.S.damage, 2000f); 
            SafeSetStat(ancientMartialBody7.base_stats, strings.S.lifespan, 90f);
            SafeSetStat(ancientMartialBody7.base_stats, strings.S.stamina, 300f);
            SafeSetStat(ancientMartialBody7.base_stats, strings.S.attack_speed, 10f);
            SafeSetStat(ancientMartialBody7.base_stats, strings.S.multiplier_health, 0.5f);
            SafeSetStat(ancientMartialBody7.base_stats, strings.S.multiplier_damage, 0.3f);
            SafeSetStat(ancientMartialBody7.base_stats, strings.S.multiplier_speed, 0.3f);
            ancientMartialBody7.action_attack_target += traitAction.ArmorBasedTrueDamage_AttackAction;
            AssetManager.traits.add(ancientMartialBody7);

            ActorTrait ancientMartialBody8 = CreateTrait("ancientMartialBody8", "trait/ancientMartialBody8", "AncientMartialBodies");
            ancientMartialBody8.rarity = Rarity.R2_Epic;
            SafeSetStat(ancientMartialBody8.base_stats, strings.S.speed, 40f);
            SafeSetStat(ancientMartialBody8.base_stats, strings.S.armor, 10f);
            SafeSetStat(ancientMartialBody8.base_stats, strings.S.health, 1000f);
            SafeSetStat(ancientMartialBody8.base_stats, strings.S.damage, 800f); 
            SafeSetStat(ancientMartialBody8.base_stats, strings.S.lifespan, 60f);
            SafeSetStat(ancientMartialBody8.base_stats, strings.S.stamina, 240f);
            SafeSetStat(ancientMartialBody8.base_stats, strings.S.attack_speed, 80f);
            SafeSetStat(ancientMartialBody8.base_stats, strings.S.multiplier_health, 0.4f);
            SafeSetStat(ancientMartialBody8.base_stats, strings.S.multiplier_damage, 0.36f);
            SafeSetStat(ancientMartialBody8.base_stats, strings.S.multiplier_speed, 0.12f);
            ancientMartialBody8.action_attack_target += traitAction.BloodSeaTrueDamage_AttackAction;
            AssetManager.traits.add(ancientMartialBody8);

            ActorTrait ancientMartialBody9 = CreateTrait("ancientMartialBody9", "trait/ancientMartialBody9", "AncientMartialBodies");
            ancientMartialBody9.rarity = Rarity.R3_Legendary;
            SafeSetStat(ancientMartialBody9.base_stats, strings.S.speed, 50f);
            SafeSetStat(ancientMartialBody9.base_stats, strings.S.armor, 40f);
            SafeSetStat(ancientMartialBody9.base_stats, strings.S.health, 3600f);
            SafeSetStat(ancientMartialBody9.base_stats, strings.S.damage, 4800f); 
            SafeSetStat(ancientMartialBody9.base_stats, strings.S.lifespan, 800f);
            SafeSetStat(ancientMartialBody9.base_stats, strings.S.stamina, 600f);
            SafeSetStat(ancientMartialBody9.base_stats, strings.S.attack_speed, 120f);
            SafeSetStat(ancientMartialBody9.base_stats, strings.S.multiplier_health, 4f);
            SafeSetStat(ancientMartialBody9.base_stats, strings.S.multiplier_damage, 5f);
            SafeSetStat(ancientMartialBody9.base_stats, strings.S.multiplier_speed, 3f);
            ancientMartialBody9.action_attack_target += traitAction.TaiChuTrueDamage_AttackAction;
            AssetManager.traits.add(ancientMartialBody9);

            ActorTrait ancientMartialBody91 = CreateTrait("ancientMartialBody91", "trait/ancientMartialBody91", "AncientMartialBodies");
            ancientMartialBody91.rarity = Rarity.R3_Legendary;
            SafeSetStat(ancientMartialBody91.base_stats, strings.S.speed, 45f);
            SafeSetStat(ancientMartialBody91.base_stats, strings.S.armor, 45f);
            SafeSetStat(ancientMartialBody91.base_stats, strings.S.health, 4500f);
            SafeSetStat(ancientMartialBody91.base_stats, strings.S.damage, 4500f); 
            SafeSetStat(ancientMartialBody91.base_stats, strings.S.lifespan, 900f);
            SafeSetStat(ancientMartialBody91.base_stats, strings.S.stamina, 900f);
            SafeSetStat(ancientMartialBody91.base_stats, strings.S.attack_speed, 120f);
            SafeSetStat(ancientMartialBody91.base_stats, strings.S.multiplier_health, 5f);
            SafeSetStat(ancientMartialBody91.base_stats, strings.S.multiplier_damage, 8f);
            SafeSetStat(ancientMartialBody91.base_stats, strings.S.multiplier_speed, 5f);
            ancientMartialBody91.action_attack_target += traitAction.fire2_attackAction;
            ancientMartialBody91.action_attack_target += traitAction.DaSunTrueDamage_AttackAction;
            AssetManager.traits.add(ancientMartialBody91);

            ActorTrait ancientMartialBody92 = CreateTrait("ancientMartialBody92", "trait/ancientMartialBody92", "AncientMartialBodies");
            ancientMartialBody92.rarity = Rarity.R2_Epic;
            SafeSetStat(ancientMartialBody92.base_stats, strings.S.speed, 9f);
            SafeSetStat(ancientMartialBody92.base_stats, strings.S.armor, 10f);
            SafeSetStat(ancientMartialBody92.base_stats, strings.S.health, 550f);
            SafeSetStat(ancientMartialBody92.base_stats, strings.S.damage, 650f); 
            SafeSetStat(ancientMartialBody92.base_stats, strings.S.lifespan, 22f);
            SafeSetStat(ancientMartialBody92.base_stats, strings.S.stamina, 300f);
            SafeSetStat(ancientMartialBody92.base_stats, strings.S.attack_speed, 24f);
            SafeSetStat(ancientMartialBody92.base_stats, strings.S.multiplier_health, 0.4f);
            SafeSetStat(ancientMartialBody92.base_stats, strings.S.multiplier_damage, 0.35f);
            SafeSetStat(ancientMartialBody92.base_stats, strings.S.multiplier_speed, 0.15f);
            ancientMartialBody92.action_attack_target += traitAction.TaiYinTrueDamage_AttackAction;
            AssetManager.traits.add(ancientMartialBody92);

            ActorTrait ancientMartialBody93 = CreateTrait("ancientMartialBody93", "trait/ancientMartialBody93", "AncientMartialBodies");
            ancientMartialBody93.rarity = Rarity.R2_Epic;
            SafeSetStat(ancientMartialBody93.base_stats, strings.S.speed, 25f);
            SafeSetStat(ancientMartialBody93.base_stats, strings.S.armor, 20f);
            SafeSetStat(ancientMartialBody93.base_stats, strings.S.health, 600f);
            SafeSetStat(ancientMartialBody93.base_stats, strings.S.damage, 700f); 
            SafeSetStat(ancientMartialBody93.base_stats, strings.S.lifespan, 400f);
            SafeSetStat(ancientMartialBody93.base_stats, strings.S.stamina, 120f);
            SafeSetStat(ancientMartialBody93.base_stats, strings.S.attack_speed, 12f);
            SafeSetStat(ancientMartialBody93.base_stats, strings.S.multiplier_health, 0.16f);
            SafeSetStat(ancientMartialBody93.base_stats, strings.S.multiplier_damage, 0.24f);
            SafeSetStat(ancientMartialBody93.base_stats, strings.S.multiplier_speed, 0.55f);
            ancientMartialBody93.action_attack_target += traitAction.QingTianTrueDamage_AttackAction;
            AssetManager.traits.add(ancientMartialBody93);

            ActorTrait ancientMartialBody94 = CreateTrait("ancientMartialBody94", "trait/ancientMartialBody94", "AncientMartialBodies");
            ancientMartialBody94.rarity = Rarity.R2_Epic;
            SafeSetStat(ancientMartialBody94.base_stats, strings.S.speed, 20f);
            SafeSetStat(ancientMartialBody94.base_stats, strings.S.armor, 20f);
            SafeSetStat(ancientMartialBody94.base_stats, strings.S.health, 400f);
            SafeSetStat(ancientMartialBody94.base_stats, strings.S.damage, 600f); 
            SafeSetStat(ancientMartialBody94.base_stats, strings.S.lifespan, 50f);
            SafeSetStat(ancientMartialBody94.base_stats, strings.S.stamina,50f);
            SafeSetStat(ancientMartialBody94.base_stats, strings.S.attack_speed, 10f);
            SafeSetStat(ancientMartialBody94.base_stats, strings.S.multiplier_health, 0.3f);
            SafeSetStat(ancientMartialBody94.base_stats, strings.S.multiplier_damage, 0.4f);
            SafeSetStat(ancientMartialBody94.base_stats, strings.S.multiplier_speed, 0.2f);
            ancientMartialBody94.action_attack_target += traitAction.JieMieTrueDamage_AttackAction;
            AssetManager.traits.add(ancientMartialBody94);

            ActorTrait ancientMartialBody95 = CreateTrait("ancientMartialBody95", "trait/ancientMartialBody95", "AncientMartialBodies");
            ancientMartialBody95.rarity = Rarity.R2_Epic;
            SafeSetStat(ancientMartialBody95.base_stats, strings.S.speed, 30f);
            SafeSetStat(ancientMartialBody95.base_stats, strings.S.armor, 2f);
            SafeSetStat(ancientMartialBody95.base_stats, strings.S.health, 2000f);
            SafeSetStat(ancientMartialBody95.base_stats, strings.S.damage, 400f); 
            SafeSetStat(ancientMartialBody95.base_stats, strings.S.lifespan, 120f);
            SafeSetStat(ancientMartialBody95.base_stats, strings.S.stamina, 120f);
            SafeSetStat(ancientMartialBody95.base_stats, strings.S.attack_speed, 21f);
            SafeSetStat(ancientMartialBody95.base_stats, strings.S.multiplier_health, 0.8f);
            SafeSetStat(ancientMartialBody95.base_stats, strings.S.multiplier_damage, 0.45f);
            SafeSetStat(ancientMartialBody95.base_stats, strings.S.multiplier_speed, 0.48f);
            ancientMartialBody95.action_attack_target += traitAction.XuKongTrueDamage_AttackAction;
            AssetManager.traits.add(ancientMartialBody95);

            ActorTrait ancientMartialBody96 = CreateTrait("ancientMartialBody96", "trait/ancientMartialBody96", "AncientMartialBodies");
            ancientMartialBody96.rarity = Rarity.R3_Legendary;
            SafeSetStat(ancientMartialBody96.base_stats, strings.S.speed, 30f);
            SafeSetStat(ancientMartialBody96.base_stats, strings.S.armor, 30f);
            SafeSetStat(ancientMartialBody96.base_stats, strings.S.health, 6000f);
            SafeSetStat(ancientMartialBody96.base_stats, strings.S.damage, 1000f); 
            SafeSetStat(ancientMartialBody96.base_stats, strings.S.lifespan, 300f);
            SafeSetStat(ancientMartialBody96.base_stats, strings.S.stamina, 300f);
            SafeSetStat(ancientMartialBody96.base_stats, strings.S.attack_speed, 50f);
            SafeSetStat(ancientMartialBody96.base_stats, strings.S.multiplier_health, 4.5f);
            SafeSetStat(ancientMartialBody96.base_stats, strings.S.multiplier_damage, 3.6f);
            SafeSetStat(ancientMartialBody96.base_stats, strings.S.multiplier_speed, 2.3f);
            ancientMartialBody96.action_attack_target += traitAction.ZhuJieTrueDamage_AttackAction;
            AssetManager.traits.add(ancientMartialBody96);

            ActorTrait ancientMartialBody97 = CreateTrait("ancientMartialBody97", "trait/ancientMartialBody97", "AncientMartialBodies");
            ancientMartialBody97.rarity = Rarity.R2_Epic;
            SafeSetStat(ancientMartialBody97.base_stats, strings.S.speed, 25f);
            SafeSetStat(ancientMartialBody97.base_stats, strings.S.armor, 25f);
            SafeSetStat(ancientMartialBody97.base_stats, strings.S.health, 600f);
            SafeSetStat(ancientMartialBody97.base_stats, strings.S.damage, 1000f); 
            SafeSetStat(ancientMartialBody97.base_stats, strings.S.lifespan, 60f);
            SafeSetStat(ancientMartialBody97.base_stats, strings.S.stamina, 240f);
            SafeSetStat(ancientMartialBody97.base_stats, strings.S.attack_speed, 20f);
            SafeSetStat(ancientMartialBody97.base_stats, strings.S.multiplier_health, 0.55f);
            SafeSetStat(ancientMartialBody97.base_stats, strings.S.multiplier_damage, 0.25f);
            SafeSetStat(ancientMartialBody97.base_stats, strings.S.multiplier_speed, 0.35f);
            ancientMartialBody97.action_attack_target += traitAction.WanHeTrueDamage_AttackAction;
            AssetManager.traits.add(ancientMartialBody97);

            ActorTrait ancientMartialBody98 = CreateTrait("ancientMartialBody98", "trait/ancientMartialBody98", "AncientMartialBodies");
            ancientMartialBody98.rarity = Rarity.R2_Epic;
            SafeSetStat(ancientMartialBody98.base_stats, strings.S.speed, 48f);
            SafeSetStat(ancientMartialBody98.base_stats, strings.S.armor, 12f);
            SafeSetStat(ancientMartialBody98.base_stats, strings.S.health, 1600f);
            SafeSetStat(ancientMartialBody98.base_stats, strings.S.damage, 500f); 
            SafeSetStat(ancientMartialBody98.base_stats, strings.S.lifespan, 90f);
            SafeSetStat(ancientMartialBody98.base_stats, strings.S.stamina, 600f);
            SafeSetStat(ancientMartialBody98.base_stats, strings.S.attack_speed, 80f);
            SafeSetStat(ancientMartialBody98.base_stats, strings.S.multiplier_health, 0.5f);
            SafeSetStat(ancientMartialBody98.base_stats, strings.S.multiplier_damage, 0.56f);
            SafeSetStat(ancientMartialBody98.base_stats, strings.S.multiplier_speed, 0.32f);
            ancientMartialBody98.action_attack_target += traitAction.GengGuTrueDamage_AttackAction;
            AssetManager.traits.add(ancientMartialBody98);

            ActorTrait ancientMartialBody99 = CreateTrait("ancientMartialBody99", "trait/ancientMartialBody99", "AncientMartialBodies");
            ancientMartialBody99.rarity = Rarity.R2_Epic;
            SafeSetStat(ancientMartialBody99.base_stats, strings.S.speed, 60f);
            SafeSetStat(ancientMartialBody99.base_stats, strings.S.armor, 30f);
            SafeSetStat(ancientMartialBody99.base_stats, strings.S.health, 3600f);
            SafeSetStat(ancientMartialBody99.base_stats, strings.S.damage, 480f); 
            SafeSetStat(ancientMartialBody99.base_stats, strings.S.lifespan, 180f);
            SafeSetStat(ancientMartialBody99.base_stats, strings.S.stamina, 200f);
            SafeSetStat(ancientMartialBody99.base_stats, strings.S.attack_speed, 20f);
            SafeSetStat(ancientMartialBody99.base_stats, strings.S.multiplier_health, 0.4f);
            SafeSetStat(ancientMartialBody99.base_stats, strings.S.multiplier_damage, 0.5f);
            SafeSetStat(ancientMartialBody99.base_stats, strings.S.multiplier_speed, 0.3f);
            ancientMartialBody99.action_attack_target += traitAction.JiuYuanTrueDamage_AttackAction;
            AssetManager.traits.add(ancientMartialBody99);

            ActorTrait Warrior02 = CreateTrait("Warrior2+", "trait/Warrior2+", "Warrior");
            SafeSetStat(Warrior02.base_stats, strings.S.lifespan, 10f);
            SafeSetStat(Warrior02.base_stats, strings.S.skill_combat, 0.1f);
            SafeSetStat(Warrior02.base_stats, strings.S.loyalty_traits, -1f);
            AssetManager.traits.add(Warrior02);

            ActorTrait Warrior03 = CreateTrait("Warrior3+", "trait/Warrior3+", "Warrior");
            SafeSetStat(Warrior03.base_stats, strings.S.lifespan, 20f);
            SafeSetStat(Warrior03.base_stats, strings.S.skill_combat, 0.2f);
            SafeSetStat(Warrior03.base_stats, strings.S.loyalty_traits, -5f);
            AssetManager.traits.add(Warrior03);

            ActorTrait Warrior04 = CreateTrait("Warrior4+", "trait/Warrior4+", "Warrior");
            SafeSetStat(Warrior04.base_stats , strings.S.lifespan, 30f);
            SafeSetStat(Warrior04.base_stats , strings.S.skill_combat, 0.3f);
            SafeSetStat(Warrior04.base_stats, strings.S.loyalty_traits, -10f);
            AssetManager.traits.add(Warrior04);

            ActorTrait Warrior05 = CreateTrait("Warrior5+", "trait/Warrior5+", "Warrior");
            SafeSetStat(Warrior05.base_stats , strings.S.lifespan, 50f);
            SafeSetStat(Warrior05.base_stats , strings.S.skill_combat, 0.4f);
            SafeSetStat(Warrior05.base_stats, strings.S.loyalty_traits, -20f);
            AssetManager.traits.add(Warrior05);

            ActorTrait Warrior06 = CreateTrait("Warrior6+", "trait/Warrior6+", "Warrior");
            SafeSetStat(Warrior06.base_stats , strings.S.lifespan, 80f);
            SafeSetStat(Warrior06.base_stats , strings.S.skill_combat, 0.5f);
            SafeSetStat(Warrior06.base_stats, strings.S.loyalty_traits, -30f);
            AssetManager.traits.add(Warrior06);

            ActorTrait Warrior07 = CreateTrait("Warrior7+", "trait/Warrior7+", "Warrior");
            SafeSetStat(Warrior07.base_stats, strings.S.lifespan, 100f);
            SafeSetStat(Warrior07.base_stats, strings.S.skill_combat, 0.6f);
            SafeSetStat(Warrior07.base_stats, strings.S.loyalty_traits, -40f);
            AssetManager.traits.add(Warrior07);

            ActorTrait Warrior08 = CreateTrait("Warrior8+", "trait/Warrior8+", "Warrior");
            SafeSetStat(Warrior08.base_stats, strings.S.lifespan, 150f);
            SafeSetStat(Warrior08.base_stats, strings.S.skill_combat, 0.6f);
            SafeSetStat(Warrior08.base_stats, strings.S.loyalty_traits, -60f);
            AssetManager.traits.add(Warrior08);

            ActorTrait Warrior09 = CreateTrait("Warrior9+", "trait/Warrior9+", "Warrior");
            SafeSetStat(Warrior09.base_stats, strings.S.lifespan, 240f);
            SafeSetStat(Warrior09.base_stats, strings.S.skill_combat, 0.6f);
            SafeSetStat(Warrior09.base_stats, strings.S.loyalty_traits, -80f);
            AssetManager.traits.add(Warrior09);

            ActorTrait Warrior091 = CreateTrait("Warrior91+", "trait/Warrior91+", "Warrior");
            SafeSetStat(Warrior091.base_stats, strings.S.lifespan, 400f);
            SafeSetStat(Warrior091.base_stats, strings.S.skill_combat, 0.6f);
            SafeSetStat(Warrior091.base_stats, strings.S.loyalty_traits, -120f);
            AssetManager.traits.add(Warrior091);

            ActorTrait Warrior092 = CreateTrait("Warrior92+", "trait/Warrior92+", "Warrior");
            SafeSetStat(Warrior092.base_stats, strings.S.lifespan, 800f);
            SafeSetStat(Warrior092.base_stats, strings.S.skill_combat, 0.7f);
            SafeSetStat(Warrior092.base_stats, strings.S.loyalty_traits, -300f);
            AssetManager.traits.add(Warrior092);

            ActorTrait Warrior093 = CreateTrait("Warrior93+", "trait/Warrior93+", "Warrior");
            SafeSetStat(Warrior093.base_stats, strings.S.lifespan, 10000f);
            SafeSetStat(Warrior093.base_stats, strings.S.skill_combat, 0.8f);
            SafeSetStat(Warrior093.base_stats, strings.S.loyalty_traits, -500f);
            AssetManager.traits.add(Warrior093);

            ActorTrait Warrior1 = CreateTrait("Warrior1", "trait/Warrior1", "Warrior");
            SafeSetStat(Warrior1.base_stats, stats.Resist.id, 0.5f);
            SafeSetStat(Warrior1.base_stats, strings.S.damage, 60f);
            SafeSetStat(Warrior1.base_stats, strings.S.mass, 5.0f);
            SafeSetStat(Warrior1.base_stats, strings.S.health, 300f);
            SafeSetStat(Warrior1.base_stats, strings.S.stamina, 10f);
            SafeSetStat(Warrior1.base_stats, "Dodge", 100f);
            SafeSetStat(Warrior1.base_stats, "Accuracy", 80f);
            Warrior1.action_special_effect += traitAction.Warrior2_effectAction;
            Warrior1.action_special_effect += traitAction.Warrior1_Regen;
            AssetManager.traits.add(Warrior1);

            ActorTrait Warrior2 = CreateTrait("Warrior2", "trait/Warrior2", "Warrior");
            SafeSetStat(Warrior2.base_stats, stats.Resist.id, 1.0f);
            SafeSetStat(Warrior2.base_stats , strings.S.damage, 120f);
            SafeSetStat(Warrior2.base_stats, strings.S.mass, 10f);
            SafeSetStat(Warrior2.base_stats , strings.S.health, 500f);
            SafeSetStat(Warrior2.base_stats , strings.S.accuracy, 2f);
            SafeSetStat(Warrior2.base_stats , strings.S.multiplier_speed, 0.1f);
            SafeSetStat(Warrior2.base_stats , strings.S.stamina, 20f);
            SafeSetStat(Warrior2.base_stats, "Dodge", 130f);
            SafeSetStat(Warrior2.base_stats, "Accuracy", 110f);
            Warrior2.action_special_effect += traitAction.Warrior3_effectAction;
            Warrior2.action_special_effect += traitAction.Warrior2_Regen;
            AssetManager.traits.add(Warrior2);

            ActorTrait Warrior3 = CreateTrait("Warrior3", "trait/Warrior3", "Warrior");
            SafeSetStat(Warrior3.base_stats, stats.Resist.id, 2.0f);
            SafeSetStat(Warrior3.base_stats , strings.S.damage, 180f);
            SafeSetStat(Warrior3.base_stats, strings.S.mass, 10f);
            SafeSetStat(Warrior3.base_stats , strings.S.health, 800f);
            SafeSetStat(Warrior3.base_stats , strings.S.armor, 5f);
            SafeSetStat(Warrior3.base_stats , strings.S.targets, 0.1f);
            SafeSetStat(Warrior3.base_stats , strings.S.accuracy, 5f);
            SafeSetStat(Warrior3.base_stats , strings.S.multiplier_speed, 0.2f);
            SafeSetStat(Warrior3.base_stats , strings.S.stamina, 30f);
            SafeSetStat(Warrior3.base_stats, "Dodge", 140f);
            SafeSetStat(Warrior3.base_stats, "Accuracy", 110f);
            Warrior3.action_special_effect += traitAction.Warrior4_effectAction;
            Warrior3.action_special_effect += traitAction.Warrior3_Regen;
            AssetManager.traits.add(Warrior3);

            ActorTrait Warrior4 = CreateTrait("Warrior4", "trait/Warrior4", "Warrior");
            SafeSetStat(Warrior4.base_stats, stats.Resist.id, 4.0f);
            SafeSetStat(Warrior4.base_stats , strings.S.damage, 240f);
            SafeSetStat(Warrior4.base_stats, strings.S.mass, 10f);
            SafeSetStat(Warrior4.base_stats , strings.S.health, 1500f);
            SafeSetStat(Warrior4.base_stats , strings.S.speed, 5f);
            SafeSetStat(Warrior4.base_stats , strings.S.armor, 10f);
            SafeSetStat(Warrior4.base_stats , strings.S.targets, 1f);
            SafeSetStat(Warrior4.base_stats , strings.S.critical_chance, 0.2f);
            SafeSetStat(Warrior4.base_stats , strings.S.accuracy, 10f);
            SafeSetStat(Warrior4.base_stats , strings.S.multiplier_speed, 0.3f);
            SafeSetStat(Warrior4.base_stats , strings.S.stamina, 40f);
            SafeSetStat(Warrior4.base_stats, "Dodge", 150f);
            SafeSetStat(Warrior4.base_stats, "Accuracy", 110f);
            Warrior4.action_special_effect += traitAction.Warrior5_effectAction;
            Warrior4.action_special_effect += traitAction.Warrior4_Regen;
            AssetManager.traits.add(Warrior4);

            ActorTrait Warrior5 = CreateTrait("Warrior5", "trait/Warrior5", "Warrior");
            SafeSetStat(Warrior5.base_stats, stats.Resist.id, 8.0f);
            SafeSetStat(Warrior5.base_stats , strings.S.warfare, 10f);
            SafeSetStat(Warrior5.base_stats, strings.S.mass, 15f);
            SafeSetStat(Warrior5.base_stats , strings.S.damage, 360f);
            SafeSetStat(Warrior5.base_stats , strings.S.health, 3000f);
            SafeSetStat(Warrior5.base_stats , strings.S.speed, 10f);
            SafeSetStat(Warrior5.base_stats , strings.S.armor, 15f);
            SafeSetStat(Warrior5.base_stats , strings.S.targets, 2f);
            SafeSetStat(Warrior5.base_stats , strings.S.critical_chance, 0.3f);
            SafeSetStat(Warrior5.base_stats , strings.S.accuracy, 20f);
            SafeSetStat(Warrior5.base_stats , strings.S.multiplier_speed, 0.4f);
            SafeSetStat(Warrior5.base_stats , strings.S.stamina, 50f);
            SafeSetStat(Warrior5.base_stats, "Dodge", 180f);
            SafeSetStat(Warrior5.base_stats, "Accuracy", 140f);
            Warrior5.action_special_effect += traitAction.Warrior6_effectAction;
            Warrior5.action_special_effect += traitAction.Warrior5_Regen;
            AssetManager.traits.add(Warrior5);

            ActorTrait Warrior6 = CreateTrait("Warrior6", "trait/Warrior6", "Warrior");
            Warrior6.rarity = Rarity.R2_Epic;
            SafeSetStat(Warrior6.base_stats, stats.Resist.id, 16.0f);
            SafeSetStat(Warrior6.base_stats , strings.S.warfare, 20f);
            SafeSetStat(Warrior6.base_stats, strings.S.mass, 20f);
            SafeSetStat(Warrior6.base_stats , strings.S.damage, 600f);
            SafeSetStat(Warrior6.base_stats , strings.S.armor, 25f);
            SafeSetStat(Warrior6.base_stats , strings.S.health, 6000f);
            SafeSetStat(Warrior6.base_stats , strings.S.speed, 15f);
            SafeSetStat(Warrior6.base_stats , strings.S.area_of_effect, 10f);
            SafeSetStat(Warrior6.base_stats , strings.S.targets, 5f);
            SafeSetStat(Warrior6.base_stats , strings.S.critical_chance, 0.5f);
            SafeSetStat(Warrior6.base_stats , strings.S.accuracy, 30f);
            SafeSetStat(Warrior6.base_stats , strings.S.multiplier_speed, 0.6f);
            SafeSetStat(Warrior6.base_stats , strings.S.stamina, 70f);
            SafeSetStat(Warrior6.base_stats, "Dodge", 200f);
            SafeSetStat(Warrior6.base_stats, "Accuracy", 160f);
            Warrior6.action_special_effect += traitAction.Warrior7_effectAction;
            Warrior6.action_special_effect += traitAction.Warrior6_Regen;
            Warrior6.action_attack_target += traitAction.TrueDamage1_AttackAction;
            AssetManager.traits.add(Warrior6);

            ActorTrait Warrior7 = CreateTrait("Warrior7", "trait/Warrior7", "Warrior");
            Warrior7.rarity = Rarity.R2_Epic;
            SafeSetStat(Warrior7.base_stats, stats.Resist.id, 32.0f);
            SafeSetStat(Warrior7.base_stats, strings.S.warfare, 30f);
            SafeSetStat(Warrior7.base_stats, strings.S.damage, 1200f);
            SafeSetStat(Warrior7.base_stats, strings.S.mass, 25f);
            SafeSetStat(Warrior7.base_stats, strings.S.armor, 35f);
            SafeSetStat(Warrior7.base_stats, strings.S.health, 12000f);
            SafeSetStat(Warrior7.base_stats, strings.S.speed, 20f);
            SafeSetStat(Warrior7.base_stats, strings.S.area_of_effect, 15f);
            SafeSetStat(Warrior7.base_stats, strings.S.targets, 7f);
            SafeSetStat(Warrior7.base_stats, strings.S.critical_chance, 0.6f);
            SafeSetStat(Warrior7.base_stats, strings.S.accuracy, 40f);
            SafeSetStat(Warrior7.base_stats, strings.S.multiplier_speed, 0.8f);
            SafeSetStat(Warrior7.base_stats, strings.S.stamina, 140f);
            SafeSetStat(Warrior7.base_stats, strings.S.range, 2f);
            SafeSetStat(Warrior7.base_stats, strings.S.attack_speed, 2f);
            SafeSetStat(Warrior7.base_stats, strings.S.scale, 0.04f);
            SafeSetStat(Warrior7.base_stats, strings.S.multiplier_health, 0.2f);
            SafeSetStat(Warrior7.base_stats, strings.S.multiplier_damage, 0.2f);
            SafeSetStat(Warrior7.base_stats, "Dodge", 220f);
            SafeSetStat(Warrior7.base_stats, "Accuracy", 180f);
            Warrior7.action_attack_target += traitAction.TrueDamage2_AttackAction;
            Warrior7.action_special_effect += traitAction.Warrior8_effectAction;
            Warrior7.action_special_effect += traitAction.Warrior7_Regen;
            AssetManager.traits.add(Warrior7);

            ActorTrait Warrior8 = CreateTrait("Warrior8", "trait/Warrior8", "Warrior");
            Warrior8.rarity = Rarity.R2_Epic;
            SafeSetStat(Warrior8.base_stats, stats.Resist.id, 64.0f);
            SafeSetStat(Warrior8.base_stats, strings.S.warfare, 40f);
            SafeSetStat(Warrior8.base_stats, strings.S.damage, 2400f);
            SafeSetStat(Warrior8.base_stats, strings.S.mass, 60f);
            SafeSetStat(Warrior8.base_stats, strings.S.armor, 45f);
            SafeSetStat(Warrior8.base_stats, strings.S.health, 25000f);
            SafeSetStat(Warrior8.base_stats, strings.S.speed, 25f);
            SafeSetStat(Warrior8.base_stats, strings.S.area_of_effect, 20f);
            SafeSetStat(Warrior8.base_stats, strings.S.targets, 9f);
            SafeSetStat(Warrior8.base_stats, strings.S.critical_chance, 0.7f);
            SafeSetStat(Warrior8.base_stats, strings.S.accuracy, 50f);
            SafeSetStat(Warrior8.base_stats, strings.S.multiplier_speed, 1.0f);
            SafeSetStat(Warrior8.base_stats, strings.S.stamina, 240f);
            SafeSetStat(Warrior8.base_stats, strings.S.range, 4f);
            SafeSetStat(Warrior8.base_stats, strings.S.attack_speed, 4f);
            SafeSetStat(Warrior8.base_stats, strings.S.multiplier_health, 0.4f);
            SafeSetStat(Warrior8.base_stats, strings.S.multiplier_damage, 0.4f);
            SafeSetStat(Warrior8.base_stats, strings.S.scale, 0.06f);
            SafeSetStat(Warrior8.base_stats, "Dodge", 260f);
            SafeSetStat(Warrior8.base_stats, "Accuracy", 220f);
            Warrior8.action_attack_target += traitAction.TrueDamage3_AttackAction;
            Warrior8.action_special_effect += traitAction.Warrior9_effectAction;
            Warrior8.action_special_effect += traitAction.Warrior8_Regen;
            AssetManager.traits.add(Warrior8);

            ActorTrait Warrior9 = CreateTrait("Warrior9", "trait/Warrior9", "Warrior");
            Warrior9.rarity = Rarity.R2_Epic;
            SafeSetStat(Warrior9.base_stats, stats.Resist.id, 128.0f);
            SafeSetStat(Warrior9.base_stats, strings.S.warfare, 50f);
            SafeSetStat(Warrior9.base_stats, strings.S.damage, 5000f);
            SafeSetStat(Warrior9.base_stats, strings.S.mass, 140f);
            SafeSetStat(Warrior9.base_stats, strings.S.armor, 60f);
            SafeSetStat(Warrior9.base_stats, strings.S.health, 50000f);
            SafeSetStat(Warrior9.base_stats, strings.S.speed, 30f);
            SafeSetStat(Warrior9.base_stats, strings.S.area_of_effect, 30f);
            SafeSetStat(Warrior9.base_stats, strings.S.targets, 12f);
            SafeSetStat(Warrior9.base_stats, strings.S.critical_chance, 0.8f);
            SafeSetStat(Warrior9.base_stats, strings.S.accuracy, 60f);
            SafeSetStat(Warrior9.base_stats, strings.S.multiplier_speed, 1.2f);
            SafeSetStat(Warrior9.base_stats, strings.S.stamina, 400f);
            SafeSetStat(Warrior9.base_stats, strings.S.range, 6f);
            SafeSetStat(Warrior9.base_stats, strings.S.attack_speed, 6f);
            SafeSetStat(Warrior9.base_stats, strings.S.scale, 0.08f);
            SafeSetStat(Warrior9.base_stats, strings.S.multiplier_health, 0.6f);
            SafeSetStat(Warrior9.base_stats, strings.S.multiplier_damage, 0.6f);
            SafeSetStat(Warrior9.base_stats, "Dodge", 300f);
            SafeSetStat(Warrior9.base_stats, "Accuracy", 260f);
            Warrior9.action_attack_target += traitAction.TrueDamage4_AttackAction;
            Warrior9.action_special_effect += traitAction.Warrior91_effectAction;
            Warrior9.action_special_effect += traitAction.Warrior9_Regen;
            AssetManager.traits.add(Warrior9);

            ActorTrait Warrior91 = CreateTrait("Warrior91", "trait/Warrior91", "Warrior");
            Warrior91.rarity = Rarity.R3_Legendary;
            SafeSetStat(Warrior91.base_stats, stats.Resist.id, 160.0f);
            SafeSetStat(Warrior91.base_stats, strings.S.warfare, 100f);
            SafeSetStat(Warrior91.base_stats, strings.S.damage, 12000f);
            SafeSetStat(Warrior91.base_stats, strings.S.mass, 400f);
            SafeSetStat(Warrior91.base_stats, strings.S.armor, 80f);
            SafeSetStat(Warrior91.base_stats, strings.S.health, 200000f);
            SafeSetStat(Warrior91.base_stats, strings.S.speed, 100f);
            SafeSetStat(Warrior91.base_stats, strings.S.area_of_effect, 40f);
            SafeSetStat(Warrior91.base_stats, strings.S.targets, 20f);
            SafeSetStat(Warrior91.base_stats, strings.S.critical_chance, 1.8f);
            SafeSetStat(Warrior91.base_stats, strings.S.accuracy, 160f);
            SafeSetStat(Warrior91.base_stats, strings.S.multiplier_speed, 1.5f);
            SafeSetStat(Warrior91.base_stats, strings.S.stamina, 1200f);
            SafeSetStat(Warrior91.base_stats, strings.S.range, 24f);
            SafeSetStat(Warrior91.base_stats, strings.S.attack_speed, 24f);
            SafeSetStat(Warrior91.base_stats, strings.S.scale, 0.2f);
            SafeSetStat(Warrior91.base_stats, strings.S.multiplier_health, 1.2f);
            SafeSetStat(Warrior91.base_stats, strings.S.multiplier_damage, 1.2f);
            SafeSetStat(Warrior91.base_stats, "Dodge", 340f);
            SafeSetStat(Warrior91.base_stats, "Accuracy", 300f);
            Warrior91.action_special_effect += traitAction.Warrior92_effectAction;
            Warrior91.action_special_effect += traitAction.Warrior91_Regen;
            Warrior91.action_attack_target += traitAction.TrueDamage5_AttackAction;
            Warrior91.action_attack_target += traitAction.fire1_attackAction;
            Warrior91.action_attack_target += traitAction.TrueDamageByWarrior1_AttackAction;
            Warrior91.action_special_effect += traitAction.MaintainFullNutrition;
            AssetManager.traits.add(Warrior91);

            ActorTrait Warrior92 = CreateTrait("Warrior92", "trait/Warrior92", "Warrior");
            Warrior92.rarity = Rarity.R3_Legendary;
            SafeSetStat(Warrior92.base_stats, stats.Resist.id, 200.0f);
            SafeSetStat(Warrior92.base_stats, strings.S.warfare, 200f);
            SafeSetStat(Warrior92.base_stats, strings.S.damage, 45000f);
            SafeSetStat(Warrior92.base_stats, strings.S.mass, 1600f);
            SafeSetStat(Warrior92.base_stats, strings.S.armor, 100f);
            SafeSetStat(Warrior92.base_stats, strings.S.health, 600000f);
            SafeSetStat(Warrior92.base_stats, strings.S.speed, 200f);
            SafeSetStat(Warrior92.base_stats, strings.S.area_of_effect, 60f);
            SafeSetStat(Warrior92.base_stats, strings.S.targets, 30f);
            SafeSetStat(Warrior92.base_stats, strings.S.critical_chance, 2.0f);
            SafeSetStat(Warrior92.base_stats, strings.S.accuracy, 200f);
            SafeSetStat(Warrior92.base_stats, strings.S.multiplier_speed, 2.0f);
            SafeSetStat(Warrior92.base_stats, strings.S.stamina, 3000f);
            SafeSetStat(Warrior92.base_stats, strings.S.range, 40f);
            SafeSetStat(Warrior92.base_stats, strings.S.attack_speed, 40f);
            SafeSetStat(Warrior92.base_stats, strings.S.scale, 0.3f);
            SafeSetStat(Warrior92.base_stats, strings.S.multiplier_health, 2f);
            SafeSetStat(Warrior92.base_stats, strings.S.multiplier_damage, 2f);
            SafeSetStat(Warrior92.base_stats, "Dodge", 380f);
            SafeSetStat(Warrior92.base_stats, "Accuracy", 340f);
            Warrior92.action_special_effect += traitAction.Warrior93_effectAction;
            Warrior92.action_special_effect += traitAction.Warrior92_Regen;
            Warrior92.action_attack_target += traitAction.TrueDamage6_AttackAction;
            Warrior92.action_attack_target += traitAction.fire1_attackAction;
            Warrior92.action_attack_target += traitAction.TrueDamageByWarrior2_AttackAction;
            Warrior92.action_special_effect += traitAction.MaintainFullNutrition;
            AssetManager.traits.add(Warrior92);

            ActorTrait Warrior93 = CreateTrait("Warrior93", "trait/Warrior93", "Warrior");
            Warrior93.rarity = Rarity.R3_Legendary;
            SafeSetStat(Warrior93.base_stats, stats.Resist.id, 240.0f);
            SafeSetStat(Warrior93.base_stats, strings.S.warfare, 400f);
            SafeSetStat(Warrior93.base_stats, strings.S.damage, 100000f);
            SafeSetStat(Warrior93.base_stats, strings.S.mass, 2400f);
            SafeSetStat(Warrior93.base_stats, strings.S.armor, 160f);
            SafeSetStat(Warrior93.base_stats, strings.S.health, 2000000f);
            SafeSetStat(Warrior93.base_stats, strings.S.speed, 300f);
            SafeSetStat(Warrior93.base_stats, strings.S.area_of_effect, 80f);
            SafeSetStat(Warrior93.base_stats, strings.S.targets, 40f);
            SafeSetStat(Warrior93.base_stats, strings.S.critical_chance, 3.0f);
            SafeSetStat(Warrior93.base_stats, strings.S.accuracy, 360f);
            SafeSetStat(Warrior93.base_stats, strings.S.multiplier_speed, 3.0f);
            SafeSetStat(Warrior93.base_stats, strings.S.stamina, 10000f);
            SafeSetStat(Warrior93.base_stats, strings.S.range, 60f);
            SafeSetStat(Warrior93.base_stats, strings.S.attack_speed, 60f);
            SafeSetStat(Warrior93.base_stats, strings.S.scale, 0.4f);
            SafeSetStat(Warrior93.base_stats, strings.S.multiplier_health, 3f);
            SafeSetStat(Warrior93.base_stats, strings.S.multiplier_damage, 3f);    
            SafeSetStat(Warrior93.base_stats, "Dodge", 500f);
            SafeSetStat(Warrior93.base_stats, "Accuracy", 480f);
            Warrior93.action_special_effect += traitAction.Warrior93_Regen;
            Warrior93.action_attack_target += traitAction.TrueDamage7_AttackAction;
            Warrior93.action_attack_target += traitAction.fire2_attackAction;
            Warrior93.action_attack_target += traitAction.TrueDamageByWarrior3_AttackAction;
            Warrior93.action_special_effect += traitAction.MaintainFullNutrition;
            AssetManager.traits.add(Warrior93);

            ActorTrait Warrior94 = CreateTrait("Warrior94", "trait/Warrior94", "Warrior");
            SafeSetStat(Warrior94.base_stats, strings.S.multiplier_lifespan, -0.05f);
            SafeSetStat(Warrior94.base_stats, strings.S.multiplier_health, -0.05f);
            SafeSetStat(Warrior94.base_stats, strings.S.multiplier_damage, -0.05f);
            AssetManager.traits.add(Warrior94);

            ActorTrait Warrior95 = CreateTrait("Warrior95", "trait/Warrior95", "Warrior");
            SafeSetStat(Warrior95.base_stats, strings.S.multiplier_lifespan, -0.1f);
            SafeSetStat(Warrior95.base_stats, strings.S.multiplier_health, -0.1f);
            SafeSetStat(Warrior95.base_stats, strings.S.multiplier_damage, -0.1f);
            AssetManager.traits.add(Warrior95);

            ActorTrait Warrior96 = CreateTrait("Warrior96", "trait/Warrior96", "Warrior");
            Warrior96.rarity = Rarity.R2_Epic;
            SafeSetStat(Warrior96.base_stats, strings.S.multiplier_lifespan, -0.15f);
            SafeSetStat(Warrior96.base_stats, strings.S.multiplier_health, -0.15f);
            SafeSetStat(Warrior96.base_stats, strings.S.multiplier_damage, -0.15f);
            AssetManager.traits.add(Warrior96);

            ActorTrait Warrior97 = CreateTrait("Warrior97", "trait/Warrior97", "Warrior");
            Warrior97.rarity = Rarity.R2_Epic;
            SafeSetStat(Warrior97.base_stats, strings.S.multiplier_lifespan, -0.2f);
            SafeSetStat(Warrior97.base_stats, strings.S.multiplier_health, -0.2f);
            SafeSetStat(Warrior97.base_stats, strings.S.multiplier_damage, -0.2f);
            AssetManager.traits.add(Warrior97);

            ActorTrait Warrior98 = CreateTrait("Warrior98", "trait/Warrior98", "Warrior");
            Warrior98.rarity = Rarity.R3_Legendary;
            SafeSetStat(Warrior98.base_stats, strings.S.multiplier_lifespan, -0.25f);
            SafeSetStat(Warrior98.base_stats, strings.S.multiplier_health, -0.25f);
            SafeSetStat(Warrior98.base_stats, strings.S.multiplier_damage, -0.25f);
            AssetManager.traits.add(Warrior98);

            ActorTrait Warrior99 = CreateTrait("Warrior99", "trait/Warrior99", "Warrior");
            Warrior99.rarity = Rarity.R3_Legendary;
            SafeSetStat(Warrior99.base_stats, strings.S.multiplier_lifespan, -0.3f);
            SafeSetStat(Warrior99.base_stats, strings.S.multiplier_health, -0.3f);
            SafeSetStat(Warrior99.base_stats, strings.S.multiplier_damage, -0.3f);
            AssetManager.traits.add(Warrior99);

            ActorTrait arcaneTome1 = CreateTrait("arcaneTome1", "trait/arcaneTome1", "arcaneTome");
            SafeSetStat(arcaneTome1.base_stats, strings.S.damage, 500f);
            SafeSetStat(arcaneTome1.base_stats, strings.S.health, 500f);
            SafeSetStat(arcaneTome1.base_stats, strings.S.speed, 30f);
            SafeSetStat(arcaneTome1.base_stats, strings.S.armor, 15f);
            SafeSetStat(arcaneTome1.base_stats, strings.S.lifespan, 10f);
            SafeSetStat(arcaneTome1.base_stats, strings.S.stamina, 80f);
            SafeSetStat(arcaneTome1.base_stats, strings.S.attack_speed, 80f);
            SafeSetStat(arcaneTome1.base_stats, strings.S.multiplier_health, 0.1f);
            SafeSetStat(arcaneTome1.base_stats, strings.S.multiplier_damage, 0.1f);
            AssetManager.traits.add(arcaneTome1);

            ActorTrait arcaneTome2 = CreateTrait("arcaneTome2", "trait/arcaneTome2", "arcaneTome");
            SafeSetStat(arcaneTome2.base_stats, strings.S.speed, 80f);
            SafeSetStat(arcaneTome2.base_stats, strings.S.armor, 20f);
            SafeSetStat(arcaneTome2.base_stats, strings.S.damage, 800f);
            SafeSetStat(arcaneTome2.base_stats, strings.S.health, 400f);
            SafeSetStat(arcaneTome2.base_stats, strings.S.lifespan, 30f);
            SafeSetStat(arcaneTome2.base_stats, strings.S.stamina, 50f);
            SafeSetStat(arcaneTome2.base_stats, strings.S.attack_speed, 40f);
            SafeSetStat(arcaneTome2.base_stats, strings.S.multiplier_health, 0.1f);
            SafeSetStat(arcaneTome2.base_stats, strings.S.multiplier_damage, 0.2f);
            SafeSetStat(arcaneTome2.base_stats, strings.S.multiplier_speed, 0.2f);
            AssetManager.traits.add(arcaneTome2);

            ActorTrait arcaneTome3 = CreateTrait("arcaneTome3", "trait/arcaneTome3", "arcaneTome");
            SafeSetStat(arcaneTome3.base_stats, strings.S.damage, 200f);
            SafeSetStat(arcaneTome3.base_stats, strings.S.health, 1000f);
            SafeSetStat(arcaneTome3.base_stats, strings.S.speed, 10f);
            SafeSetStat(arcaneTome3.base_stats, strings.S.armor, 80f);
            SafeSetStat(arcaneTome3.base_stats, strings.S.lifespan, 50f);
            SafeSetStat(arcaneTome3.base_stats, strings.S.stamina, 200f);
            SafeSetStat(arcaneTome3.base_stats, strings.S.attack_speed, 20f);
            SafeSetStat(arcaneTome3.base_stats, strings.S.multiplier_health, 0.2f);
            SafeSetStat(arcaneTome3.base_stats, strings.S.multiplier_damage, 0.3f);
            SafeSetStat(arcaneTome3.base_stats, strings.S.multiplier_speed, 0.1f);
            AssetManager.traits.add(arcaneTome3);

            ActorTrait arcaneTome4 = CreateTrait("arcaneTome4", "trait/arcaneTome4", "arcaneTome");
            SafeSetStat(arcaneTome4.base_stats, strings.S.speed, 60f);
            SafeSetStat(arcaneTome4.base_stats, strings.S.armor, 50f);
            SafeSetStat(arcaneTome4.base_stats, strings.S.damage, 400f);
            SafeSetStat(arcaneTome4.base_stats, strings.S.health, 400f);
            SafeSetStat(arcaneTome4.base_stats, strings.S.lifespan, 100f);
            SafeSetStat(arcaneTome4.base_stats, strings.S.stamina, 120f);
            SafeSetStat(arcaneTome4.base_stats, strings.S.attack_speed, 10f);
            SafeSetStat(arcaneTome4.base_stats, strings.S.multiplier_health, 0.1f);
            SafeSetStat(arcaneTome4.base_stats, strings.S.multiplier_damage, 0.1f);
            SafeSetStat(arcaneTome4.base_stats, strings.S.multiplier_speed, 0.3f);
            AssetManager.traits.add(arcaneTome4);

            ActorTrait arcaneTome5 = CreateTrait("arcaneTome5", "trait/arcaneTome5", "arcaneTome");
            SafeSetStat(arcaneTome5.base_stats, strings.S.speed, 40f);
            SafeSetStat(arcaneTome5.base_stats, strings.S.armor, 60f);
            SafeSetStat(arcaneTome5.base_stats, strings.S.health, 200f);
            SafeSetStat(arcaneTome5.base_stats, strings.S.damage, 300f);
            SafeSetStat(arcaneTome5.base_stats, strings.S.lifespan, 300f);
            SafeSetStat(arcaneTome5.base_stats, strings.S.stamina, 30f);
            SafeSetStat(arcaneTome5.base_stats, strings.S.attack_speed, 40f);
            SafeSetStat(arcaneTome5.base_stats, strings.S.multiplier_health, 0.2f);
            SafeSetStat(arcaneTome5.base_stats, strings.S.multiplier_damage, 0.2f);
            SafeSetStat(arcaneTome5.base_stats, strings.S.multiplier_speed, 0.1f);
            AssetManager.traits.add(arcaneTome5);

            ActorTrait arcaneTome6 = CreateTrait("arcaneTome6", "trait/arcaneTome6", "arcaneTome");
            SafeSetStat(arcaneTome6.base_stats, strings.S.speed, 50f);
            SafeSetStat(arcaneTome6.base_stats, strings.S.armor, 50f);
            SafeSetStat(arcaneTome6.base_stats, strings.S.health, 100f);
            SafeSetStat(arcaneTome6.base_stats, strings.S.damage, 200f);
            SafeSetStat(arcaneTome6.base_stats, strings.S.lifespan, 30f);
            SafeSetStat(arcaneTome6.base_stats, strings.S.stamina, 90f);
            SafeSetStat(arcaneTome6.base_stats, strings.S.attack_speed, 70f);
            SafeSetStat(arcaneTome6.base_stats, strings.S.multiplier_health, 0.1f);
            SafeSetStat(arcaneTome6.base_stats, strings.S.multiplier_damage, 0.1f);
            SafeSetStat(arcaneTome6.base_stats, strings.S.multiplier_speed, 0.1f);
            AssetManager.traits.add(arcaneTome6);

            ActorTrait arcaneTome7 = CreateTrait("arcaneTome7", "trait/arcaneTome7", "arcaneTome");
            SafeSetStat(arcaneTome7.base_stats, strings.S.speed, 60f);
            SafeSetStat(arcaneTome7.base_stats, strings.S.armor, 20f);
            SafeSetStat(arcaneTome7.base_stats, strings.S.health, 180f);
            SafeSetStat(arcaneTome7.base_stats, strings.S.damage, 120f);
            SafeSetStat(arcaneTome7.base_stats, strings.S.lifespan, 50f);
            SafeSetStat(arcaneTome7.base_stats, strings.S.stamina, 40f);
            SafeSetStat(arcaneTome7.base_stats, strings.S.attack_speed, 40f);
            SafeSetStat(arcaneTome7.base_stats, strings.S.multiplier_health, 0.3f);
            SafeSetStat(arcaneTome7.base_stats, strings.S.multiplier_damage, 0.1f);
            SafeSetStat(arcaneTome7.base_stats, strings.S.multiplier_speed, 0.1f);
            AssetManager.traits.add(arcaneTome7);

            ActorTrait arcaneTome8 = CreateTrait("arcaneTome8", "trait/arcaneTome8", "arcaneTome");
            SafeSetStat(arcaneTome8.base_stats, strings.S.speed, 45f);
            SafeSetStat(arcaneTome8.base_stats, strings.S.armor, 55f);
            SafeSetStat(arcaneTome8.base_stats, strings.S.health, 600f);
            SafeSetStat(arcaneTome8.base_stats, strings.S.damage, 100f);
            SafeSetStat(arcaneTome8.base_stats, strings.S.lifespan, 40f);
            SafeSetStat(arcaneTome8.base_stats, strings.S.stamina, 70f);
            SafeSetStat(arcaneTome8.base_stats, strings.S.attack_speed, 30f);
            SafeSetStat(arcaneTome8.base_stats, strings.S.multiplier_health, 0.1f);
            SafeSetStat(arcaneTome8.base_stats, strings.S.multiplier_damage, 0.2f);
            SafeSetStat(arcaneTome8.base_stats, strings.S.multiplier_speed, 0.2f);
            AssetManager.traits.add(arcaneTome8);

            ActorTrait arcaneTome9 = CreateTrait("arcaneTome9", "trait/arcaneTome9", "arcaneTome");
            SafeSetStat(arcaneTome9.base_stats, strings.S.speed, 40f);
            SafeSetStat(arcaneTome9.base_stats, strings.S.armor, 60f);
            SafeSetStat(arcaneTome9.base_stats, strings.S.health, 800f);
            SafeSetStat(arcaneTome9.base_stats, strings.S.damage, 240f);
            SafeSetStat(arcaneTome9.base_stats, strings.S.lifespan, 30f);
            SafeSetStat(arcaneTome9.base_stats, strings.S.stamina, 90f);
            SafeSetStat(arcaneTome9.base_stats, strings.S.attack_speed, 60f);
            SafeSetStat(arcaneTome9.base_stats, strings.S.multiplier_health, 0.1f);
            SafeSetStat(arcaneTome9.base_stats, strings.S.multiplier_damage, 0.2f);
            SafeSetStat(arcaneTome9.base_stats, strings.S.multiplier_speed, 0.2f);
            AssetManager.traits.add(arcaneTome9);

            ActorTrait arcaneTome10 = CreateTrait("arcaneTome10", "trait/arcaneTome10", "arcaneTome");
            arcaneTome10.rarity = Rarity.R2_Epic;
            SafeSetStat(arcaneTome10.base_stats, strings.S.speed, 10f);
            SafeSetStat(arcaneTome10.base_stats, strings.S.armor, 10f);
            SafeSetStat(arcaneTome10.base_stats, strings.S.health, 2000f);
            SafeSetStat(arcaneTome10.base_stats, strings.S.damage, 90f);
            SafeSetStat(arcaneTome10.base_stats, strings.S.lifespan, 500f);
            SafeSetStat(arcaneTome10.base_stats, strings.S.stamina, 60f);
            SafeSetStat(arcaneTome10.base_stats, strings.S.attack_speed, 50f);
            SafeSetStat(arcaneTome10.base_stats, strings.S.multiplier_health, 0.3f);
            SafeSetStat(arcaneTome10.base_stats, strings.S.multiplier_damage, 0.3f);
            SafeSetStat(arcaneTome10.base_stats, strings.S.multiplier_speed, 0.3f);
            AssetManager.traits.add(arcaneTome10);

            ActorTrait arcaneTome11 = CreateTrait("arcaneTome11", "trait/arcaneTome11", "arcaneTome");
            SafeSetStat(arcaneTome11.base_stats, strings.S.speed, 10f);
            SafeSetStat(arcaneTome11.base_stats, strings.S.armor, -10f);
            SafeSetStat(arcaneTome11.base_stats, strings.S.health, -50f);
            SafeSetStat(arcaneTome11.base_stats, strings.S.damage, 5000f);
            SafeSetStat(arcaneTome11.base_stats, strings.S.lifespan, -10f);
            SafeSetStat(arcaneTome11.base_stats, strings.S.stamina, -30f);
            SafeSetStat(arcaneTome11.base_stats, strings.S.attack_speed, 100f);
            SafeSetStat(arcaneTome11.base_stats, strings.S.multiplier_health, 0.1f);
            SafeSetStat(arcaneTome11.base_stats, strings.S.multiplier_damage, 0.6f);
            SafeSetStat(arcaneTome11.base_stats, strings.S.multiplier_speed, 0.1f);
            AssetManager.traits.add(arcaneTome11);

            ActorTrait arcaneTome12 = CreateTrait("arcaneTome12", "trait/arcaneTome12", "arcaneTome");
            arcaneTome12.rarity = Rarity.R3_Legendary;
            SafeSetStat(arcaneTome12.base_stats, strings.S.speed, 40f);
            SafeSetStat(arcaneTome12.base_stats, strings.S.armor, 100f);
            SafeSetStat(arcaneTome12.base_stats, strings.S.health, 8000f);
            SafeSetStat(arcaneTome12.base_stats, strings.S.damage, 40f); 
            SafeSetStat(arcaneTome12.base_stats, strings.S.lifespan, 600f);
            SafeSetStat(arcaneTome12.base_stats, strings.S.stamina, 1000f);
            SafeSetStat(arcaneTome12.base_stats, strings.S.attack_speed, 20f);
            SafeSetStat(arcaneTome12.base_stats, strings.S.multiplier_health, 0.5f);
            SafeSetStat(arcaneTome12.base_stats, strings.S.multiplier_damage, 0.5f);
            SafeSetStat(arcaneTome12.base_stats, strings.S.multiplier_speed, 0.5f);
            AssetManager.traits.add(arcaneTome12);

            ActorTrait arcaneTome13 = CreateTrait("arcaneTome13", "trait/arcaneTome13", "arcaneTome");
            SafeSetStat(arcaneTome13.base_stats, strings.S.speed, 80f);
            SafeSetStat(arcaneTome13.base_stats, strings.S.armor, 80f);
            SafeSetStat(arcaneTome13.base_stats, strings.S.health, 800f);
            SafeSetStat(arcaneTome13.base_stats, strings.S.damage, 800f); 
            SafeSetStat(arcaneTome13.base_stats, strings.S.lifespan, 80f);
            SafeSetStat(arcaneTome13.base_stats, strings.S.stamina, 80f);
            SafeSetStat(arcaneTome13.base_stats, strings.S.attack_speed, 80f);
            SafeSetStat(arcaneTome13.base_stats, strings.S.multiplier_health, 0.2f);
            SafeSetStat(arcaneTome13.base_stats, strings.S.multiplier_damage, 0.2f);
            SafeSetStat(arcaneTome13.base_stats, strings.S.multiplier_speed, 0.1f);
            AssetManager.traits.add(arcaneTome13);

            ActorTrait arcaneTome14 = CreateTrait("arcaneTome14", "trait/arcaneTome14", "arcaneTome");
            SafeSetStat(arcaneTome14.base_stats, strings.S.speed, 40f);
            SafeSetStat(arcaneTome14.base_stats, strings.S.armor, 40f);
            SafeSetStat(arcaneTome14.base_stats, strings.S.health, -200f);
            SafeSetStat(arcaneTome14.base_stats, strings.S.damage, 400f); 
            SafeSetStat(arcaneTome14.base_stats, strings.S.multiplier_lifespan, -0.5f);
            SafeSetStat(arcaneTome14.base_stats, strings.S.stamina, 40f);
            SafeSetStat(arcaneTome14.base_stats, strings.S.attack_speed, 40f);
            SafeSetStat(arcaneTome14.base_stats, strings.S.multiplier_health, 0.1f);
            SafeSetStat(arcaneTome14.base_stats, strings.S.multiplier_damage, 0.1f);
            SafeSetStat(arcaneTome14.base_stats, strings.S.multiplier_speed, 0.1f);
            AssetManager.traits.add(arcaneTome14);

            ActorTrait arcaneTome91 = CreateTrait("arcaneTome91", "trait/arcaneTome91", "arcaneTome");
            arcaneTome91.rarity = Rarity.R3_Legendary;
            SafeSetStat(arcaneTome91.base_stats, strings.S.speed, 100f);
            SafeSetStat(arcaneTome91.base_stats, strings.S.armor, 100f);
            SafeSetStat(arcaneTome91.base_stats, strings.S.health, 6000f);
            SafeSetStat(arcaneTome91.base_stats, strings.S.damage, 6000f); 
            SafeSetStat(arcaneTome91.base_stats, strings.S.lifespan, 100f);
            SafeSetStat(arcaneTome91.base_stats, strings.S.stamina, 80f);
            SafeSetStat(arcaneTome91.base_stats, strings.S.attack_speed, 600f);
            SafeSetStat(arcaneTome91.base_stats, strings.S.multiplier_health, 9.9f);
            SafeSetStat(arcaneTome91.base_stats, strings.S.multiplier_damage, 9.9f);
            SafeSetStat(arcaneTome91.base_stats, strings.S.multiplier_speed, 9.9f);
            arcaneTome91.action_attack_target += traitAction.TrueDamage8_AttackAction;
            AssetManager.traits.add(arcaneTome91);

            // 邪功特质 - 奇门秘术组（30%气血值）
            ActorTrait EvilGongFa2 = CreateTrait("EvilGongFa2", "trait/EvilGongFa2", "MidGongFa");
            EvilGongFa2.rarity = Rarity.R3_Legendary;
            SafeSetStat(EvilGongFa2.base_stats, strings.S.speed, 10f);
            SafeSetStat(EvilGongFa2.base_stats, strings.S.armor, 5f);
            SafeSetStat(EvilGongFa2.base_stats, strings.S.health, 50f);
            SafeSetStat(EvilGongFa2.base_stats, strings.S.damage, 50f);
            SafeSetStat(EvilGongFa2.base_stats, strings.S.lifespan, -50f);
            SafeSetStat(EvilGongFa2.base_stats, strings.S.stamina, 10f);
            SafeSetStat(EvilGongFa2.base_stats, strings.S.multiplier_health, 0.05f);
            SafeSetStat(EvilGongFa2.base_stats, strings.S.multiplier_damage, 0.05f);
            AssetManager.traits.add(EvilGongFa2);

            ActorTrait NineCharacterSecret1 = CreateTrait("NineCharacterSecret1", "trait/NineCharacterSecret1", "NineCharacterSecrets");
            NineCharacterSecret1.rarity = Rarity.R3_Legendary;
            SafeSetStat(NineCharacterSecret1.base_stats, strings.S.multiplier_health, 3.0f);
            SafeSetStat(NineCharacterSecret1.base_stats, strings.S.intelligence, 10f);
            SafeSetStat(NineCharacterSecret1.base_stats, strings.S.knockback, 10f); 
            SafeSetStat(NineCharacterSecret1.base_stats, strings.S.area_of_effect, 4f);
            NineCharacterSecret1.action_attack_target += traitAction.NineCharacterSecret1TrueDamage_AttackAction;
            AssetManager.traits.add(NineCharacterSecret1);

            ActorTrait NineCharacterSecret2 = CreateTrait("NineCharacterSecret2", "trait/NineCharacterSecret2", "NineCharacterSecrets");
            NineCharacterSecret2.rarity = Rarity.R3_Legendary;
            SafeSetStat(NineCharacterSecret2.base_stats, strings.S.multiplier_damage, 1.8f);
            SafeSetStat(NineCharacterSecret2.base_stats, strings.S.multiplier_health, 1.8f);
            SafeSetStat(NineCharacterSecret2.base_stats, strings.S.multiplier_speed, 1.8f);
            SafeSetStat(NineCharacterSecret2.base_stats, strings.S.intelligence, 100f);
            SafeSetStat(NineCharacterSecret2.base_stats, strings.S.knockback, 20f); 
            SafeSetStat(NineCharacterSecret2.base_stats, strings.S.area_of_effect, 8f);
            NineCharacterSecret2.action_attack_target += traitAction.NineCharacterSecret2TrueDamage_AttackAction;
            AssetManager.traits.add(NineCharacterSecret2);

            ActorTrait NineCharacterSecret3 = CreateTrait("NineCharacterSecret3", "trait/NineCharacterSecret3", "NineCharacterSecrets");
            NineCharacterSecret3.rarity = Rarity.R3_Legendary;
            SafeSetStat(NineCharacterSecret3.base_stats, strings.S.multiplier_speed, 3.2f);
            SafeSetStat(NineCharacterSecret3.base_stats, strings.S.multiplier_damage, 1.0f);
            SafeSetStat(NineCharacterSecret3.base_stats, strings.S.intelligence, 50f);
            SafeSetStat(NineCharacterSecret3.base_stats, strings.S.knockback, 10f); 
            SafeSetStat(NineCharacterSecret3.base_stats, strings.S.area_of_effect, 6f);
            NineCharacterSecret3.action_special_effect += traitAction.NineCharacterSecret3_Regen;
            NineCharacterSecret3.action_attack_target += traitAction.NineCharacterSecret3TrueDamage_AttackAction;
            AssetManager.traits.add(NineCharacterSecret3);

            ActorTrait NineCharacterSecret4 = CreateTrait("NineCharacterSecret4", "trait/NineCharacterSecret4", "NineCharacterSecrets");
            NineCharacterSecret4.rarity = Rarity.R3_Legendary;
            SafeSetStat(NineCharacterSecret4.base_stats, strings.S.multiplier_speed, 1.2f);
            SafeSetStat(NineCharacterSecret4.base_stats, strings.S.multiplier_damage, 1.0f);
            SafeSetStat(NineCharacterSecret4.base_stats, strings.S.multiplier_health, 1.0f);
            SafeSetStat(NineCharacterSecret4.base_stats, strings.S.intelligence, 10f);
            SafeSetStat(NineCharacterSecret4.base_stats, strings.S.knockback, 5f); 
            SafeSetStat(NineCharacterSecret4.base_stats, strings.S.area_of_effect, 4f);
            NineCharacterSecret4.action_attack_target += traitAction.NineCharacterSecret4TrueDamage_AttackAction;
            AssetManager.traits.add(NineCharacterSecret4);

            ActorTrait NineCharacterSecret5 = CreateTrait("NineCharacterSecret5", "trait/NineCharacterSecret5", "NineCharacterSecrets");
            NineCharacterSecret5.rarity = Rarity.R3_Legendary;
            SafeSetStat(NineCharacterSecret5.base_stats, strings.S.multiplier_speed, 1.0f);
            SafeSetStat(NineCharacterSecret5.base_stats, strings.S.multiplier_damage, 0.8f);
            SafeSetStat(NineCharacterSecret5.base_stats, strings.S.multiplier_health, 2.0f);
            SafeSetStat(NineCharacterSecret5.base_stats, strings.S.intelligence, 60f);
            SafeSetStat(NineCharacterSecret5.base_stats, strings.S.knockback, 20f); 
            SafeSetStat(NineCharacterSecret5.base_stats, strings.S.area_of_effect, 8f);
            NineCharacterSecret5.action_attack_target += traitAction.NineCharacterSecret5TrueDamage_AttackAction;
            AssetManager.traits.add(NineCharacterSecret5);

            ActorTrait NineCharacterSecret6 = CreateTrait("NineCharacterSecret6", "trait/NineCharacterSecret6", "NineCharacterSecrets");
            NineCharacterSecret6.rarity = Rarity.R3_Legendary;
            SafeSetStat(NineCharacterSecret6.base_stats, strings.S.multiplier_speed, 0.8f);
            SafeSetStat(NineCharacterSecret6.base_stats, strings.S.multiplier_damage, 2.0f);
            SafeSetStat(NineCharacterSecret6.base_stats, strings.S.multiplier_health, 0.8f);
            SafeSetStat(NineCharacterSecret6.base_stats, strings.S.intelligence, 10f);
            SafeSetStat(NineCharacterSecret6.base_stats, strings.S.knockback, 10f); 
            SafeSetStat(NineCharacterSecret6.base_stats, strings.S.area_of_effect, 10f);
            NineCharacterSecret6.action_special_effect += traitAction.NineCharacterSecret6_Regen;
            NineCharacterSecret6.action_attack_target += traitAction.NineCharacterSecret6TrueDamage_AttackAction;
            AssetManager.traits.add(NineCharacterSecret6);

            ActorTrait NineCharacterSecret7 = CreateTrait("NineCharacterSecret7", "trait/NineCharacterSecret7", "NineCharacterSecrets");
            NineCharacterSecret7.rarity = Rarity.R3_Legendary;
            SafeSetStat(NineCharacterSecret7.base_stats, strings.S.multiplier_speed, 2.0f);
            SafeSetStat(NineCharacterSecret7.base_stats, strings.S.multiplier_damage, 0.8f);
            SafeSetStat(NineCharacterSecret7.base_stats, strings.S.multiplier_health, 0.8f);
            SafeSetStat(NineCharacterSecret7.base_stats, strings.S.intelligence, 20f);
            SafeSetStat(NineCharacterSecret7.base_stats, strings.S.knockback, 20f); 
            SafeSetStat(NineCharacterSecret7.base_stats, strings.S.area_of_effect, 6f);
            NineCharacterSecret7.action_attack_target += traitAction.NineCharacterSecret7TrueDamage_AttackAction;
            AssetManager.traits.add(NineCharacterSecret7);

            ActorTrait NineCharacterSecret8 = CreateTrait("NineCharacterSecret8", "trait/NineCharacterSecret8", "NineCharacterSecrets");
            NineCharacterSecret8.rarity = Rarity.R3_Legendary;
            SafeSetStat(NineCharacterSecret8.base_stats, strings.S.multiplier_speed, 0.9f);
            SafeSetStat(NineCharacterSecret8.base_stats, strings.S.multiplier_damage, 0.9f);
            SafeSetStat(NineCharacterSecret8.base_stats, strings.S.multiplier_health, 0.9f);
            SafeSetStat(NineCharacterSecret8.base_stats, strings.S.intelligence, 1000f);
            SafeSetStat(NineCharacterSecret8.base_stats, strings.S.knockback, 100f); 
            SafeSetStat(NineCharacterSecret8.base_stats, strings.S.area_of_effect, 100f);
            NineCharacterSecret8.action_attack_target += traitAction.NineCharacterSecret8TrueDamage_AttackAction;
            AssetManager.traits.add(NineCharacterSecret8);

            ActorTrait NineCharacterSecret9 = CreateTrait("NineCharacterSecret9", "trait/NineCharacterSecret9", "NineCharacterSecrets");
            NineCharacterSecret9.rarity = Rarity.R3_Legendary;
            SafeSetStat(NineCharacterSecret9.base_stats, strings.S.multiplier_speed, 2.0f);
            SafeSetStat(NineCharacterSecret9.base_stats, strings.S.multiplier_damage, 1.0f);
            SafeSetStat(NineCharacterSecret9.base_stats, strings.S.multiplier_health, 2.0f);
            SafeSetStat(NineCharacterSecret9.base_stats, strings.S.intelligence, 10f);
            SafeSetStat(NineCharacterSecret9.base_stats, strings.S.knockback, 10f); 
            SafeSetStat(NineCharacterSecret9.base_stats, strings.S.area_of_effect, 10f);
            NineCharacterSecret9.action_attack_target += traitAction.NineCharacterSecret9TrueDamage_AttackAction;
            AssetManager.traits.add(NineCharacterSecret9);

            // 邪功特质 - 武道秘典组（50%气血值）
            ActorTrait EvilGongFa3 = CreateTrait("EvilGongFa3", "trait/EvilGongFa3", "arcaneTome");
            EvilGongFa3.rarity = Rarity.R3_Legendary;
            SafeSetStat(EvilGongFa3.base_stats, strings.S.multiplier_speed, 0.1f);
            SafeSetStat(EvilGongFa3.base_stats, strings.S.multiplier_damage, 0.1f);
            SafeSetStat(EvilGongFa3.base_stats, strings.S.multiplier_health, 0.2f);
            SafeSetStat(EvilGongFa3.base_stats, strings.S.intelligence, 50f);
            SafeSetStat(EvilGongFa3.base_stats, strings.S.lifespan, -200f);
            SafeSetStat(EvilGongFa3.base_stats, strings.S.area_of_effect, 15f);
            AssetManager.traits.add(EvilGongFa3);

            ActorTrait LowGongFaTrait1 = CreateTrait("LowGongFaTrait1", "trait/LowGongFaTrait1", "LowGongFa");
            SafeSetStat(LowGongFaTrait1.base_stats, strings.S.multiplier_speed, 0.03f);
            SafeSetStat(LowGongFaTrait1.base_stats, strings.S.multiplier_damage, 0.07f);
            SafeSetStat(LowGongFaTrait1.base_stats, strings.S.multiplier_health, 0.04f);
            SafeSetStat(LowGongFaTrait1.base_stats, strings.S.lifespan, 5f);
            LowGongFaTrait1.rate_inherit = 1;
            AssetManager.traits.add(LowGongFaTrait1);

            ActorTrait LowGongFaTrait2 = CreateTrait("LowGongFaTrait2", "trait/LowGongFaTrait2", "LowGongFa");
            SafeSetStat(LowGongFaTrait2.base_stats, strings.S.multiplier_speed, 0.03f);
            SafeSetStat(LowGongFaTrait2.base_stats, strings.S.multiplier_damage, 0.07f);
            SafeSetStat(LowGongFaTrait2.base_stats, strings.S.multiplier_health, 0.04f);
            SafeSetStat(LowGongFaTrait2.base_stats, strings.S.lifespan, 8f);
            LowGongFaTrait2.rate_inherit = 1;
            AssetManager.traits.add(LowGongFaTrait2);

            ActorTrait LowGongFaTrait3 = CreateTrait("LowGongFaTrait3", "trait/LowGongFaTrait3", "LowGongFa");
            SafeSetStat(LowGongFaTrait3.base_stats, strings.S.multiplier_speed, 0.04f);
            SafeSetStat(LowGongFaTrait3.base_stats, strings.S.multiplier_damage, 0.06f);
            SafeSetStat(LowGongFaTrait3.base_stats, strings.S.multiplier_health, 0.09f);
            SafeSetStat(LowGongFaTrait3.base_stats, strings.S.lifespan, 9f);
            LowGongFaTrait3.rate_inherit = 1;
            AssetManager.traits.add(LowGongFaTrait3);

            ActorTrait LowGongFaTrait4 = CreateTrait("LowGongFaTrait4", "trait/LowGongFaTrait4", "LowGongFa");
            SafeSetStat(LowGongFaTrait4.base_stats, strings.S.multiplier_speed, 0.01f);
            SafeSetStat(LowGongFaTrait4.base_stats, strings.S.multiplier_damage, 0.08f);
            SafeSetStat(LowGongFaTrait4.base_stats, strings.S.multiplier_health, 0.06f);
            SafeSetStat(LowGongFaTrait4.base_stats, strings.S.lifespan, 6f);
            LowGongFaTrait4.rate_inherit = 1;
            AssetManager.traits.add(LowGongFaTrait4);

            ActorTrait LowGongFaTrait5 = CreateTrait("LowGongFaTrait5", "trait/LowGongFaTrait5", "LowGongFa");
            SafeSetStat(LowGongFaTrait5.base_stats, strings.S.multiplier_speed, 0.03f);
            SafeSetStat(LowGongFaTrait5.base_stats, strings.S.multiplier_damage, 0.09f);
            SafeSetStat(LowGongFaTrait5.base_stats, strings.S.multiplier_health, 0.09f);
            SafeSetStat(LowGongFaTrait5.base_stats, strings.S.lifespan, 16f);
            LowGongFaTrait5.rate_inherit = 1;
            AssetManager.traits.add(LowGongFaTrait5);

            ActorTrait LowGongFaTrait6 = CreateTrait("LowGongFaTrait6", "trait/LowGongFaTrait6", "LowGongFa");
            SafeSetStat(LowGongFaTrait6.base_stats, strings.S.multiplier_speed, 0.09f);
            SafeSetStat(LowGongFaTrait6.base_stats, strings.S.multiplier_damage, 0.09f);
            SafeSetStat(LowGongFaTrait6.base_stats, strings.S.multiplier_health, 0.09f);
            SafeSetStat(LowGongFaTrait6.base_stats, strings.S.lifespan, 10f);
            LowGongFaTrait6.rate_inherit = 1;
            AssetManager.traits.add(LowGongFaTrait6);

            ActorTrait LowGongFaTrait7 = CreateTrait("LowGongFaTrait7", "trait/LowGongFaTrait7", "LowGongFa");
            SafeSetStat(LowGongFaTrait7.base_stats, strings.S.multiplier_speed, 0.06f);
            SafeSetStat(LowGongFaTrait7.base_stats, strings.S.multiplier_damage, 0.03f);
            SafeSetStat(LowGongFaTrait7.base_stats, strings.S.multiplier_health, 0.07f);
            SafeSetStat(LowGongFaTrait7.base_stats, strings.S.lifespan, 5f);
            LowGongFaTrait7.rate_inherit = 1;
            AssetManager.traits.add(LowGongFaTrait7);

            ActorTrait LowGongFaTrait8 = CreateTrait("LowGongFaTrait8", "trait/LowGongFaTrait8", "LowGongFa");
            SafeSetStat(LowGongFaTrait8.base_stats, strings.S.multiplier_speed, 0.05f);
            SafeSetStat(LowGongFaTrait8.base_stats, strings.S.multiplier_damage, 0.05f);
            SafeSetStat(LowGongFaTrait8.base_stats, strings.S.multiplier_health, 0.05f);
            SafeSetStat(LowGongFaTrait8.base_stats, strings.S.lifespan, 12f);
            LowGongFaTrait8.rate_inherit = 1;
            AssetManager.traits.add(LowGongFaTrait8);

            ActorTrait LowGongFaTrait9 = CreateTrait("LowGongFaTrait9", "trait/LowGongFaTrait9", "LowGongFa");
            SafeSetStat(LowGongFaTrait9.base_stats, strings.S.multiplier_speed, 0.08f);
            SafeSetStat(LowGongFaTrait9.base_stats, strings.S.multiplier_damage, 0.07f);
            SafeSetStat(LowGongFaTrait9.base_stats, strings.S.multiplier_health, 0.09f);
            SafeSetStat(LowGongFaTrait9.base_stats, strings.S.lifespan, 7f);
            LowGongFaTrait9.rate_inherit = 1;
            AssetManager.traits.add(LowGongFaTrait9);

            ActorTrait GongFaTrait1 = CreateTrait("GongFaTrait1", "trait/GongFaTrait1", "GongFa");
            SafeSetStat(GongFaTrait1.base_stats, strings.S.multiplier_speed, 0.1f);
            SafeSetStat(GongFaTrait1.base_stats, strings.S.multiplier_damage, 0.2f);
            SafeSetStat(GongFaTrait1.base_stats, strings.S.multiplier_health, 0.1f);
            SafeSetStat(GongFaTrait1.base_stats, strings.S.lifespan, 10f);
            SafeSetStat(GongFaTrait1.base_stats, strings.S.stamina, 15f);
            SafeSetStat(GongFaTrait1.base_stats, strings.S.area_of_effect, 10f);
            GongFaTrait1.rate_inherit = 5;
            AssetManager.traits.add(GongFaTrait1);

            ActorTrait GongFaTrait12 = CreateTrait("GongFaTrait12", "trait/GongFaTrait12", "GongFa");
            SafeSetStat(GongFaTrait12.base_stats, strings.S.multiplier_speed, 0.2f);
            SafeSetStat(GongFaTrait12.base_stats, strings.S.multiplier_damage, 0.1f);
            SafeSetStat(GongFaTrait12.base_stats, strings.S.multiplier_health, 0.1f);
            SafeSetStat(GongFaTrait12.base_stats, strings.S.lifespan, 12f);
            SafeSetStat(GongFaTrait12.base_stats, strings.S.stamina, 10f);
            SafeSetStat(GongFaTrait12.base_stats, strings.S.area_of_effect, 12f);
            GongFaTrait12.rate_inherit = 5;
            AssetManager.traits.add(GongFaTrait12);

            ActorTrait GongFaTrait2 = CreateTrait("GongFaTrait2", "trait/GongFaTrait2", "GongFa");
            SafeSetStat(GongFaTrait2.base_stats, strings.S.multiplier_speed, 0.15f);
            SafeSetStat(GongFaTrait2.base_stats, strings.S.multiplier_damage, 0.1f);
            SafeSetStat(GongFaTrait2.base_stats, strings.S.multiplier_health, 0.2f);
            SafeSetStat(GongFaTrait2.base_stats, strings.S.lifespan, 15f);
            SafeSetStat(GongFaTrait2.base_stats, strings.S.stamina, 11f);
            SafeSetStat(GongFaTrait2.base_stats, strings.S.area_of_effect, 10f);
            GongFaTrait12.rate_inherit = 5;
            AssetManager.traits.add(GongFaTrait2);

            ActorTrait GongFaTrait3 = CreateTrait("GongFaTrait3", "trait/GongFaTrait3", "GongFa");
            SafeSetStat(GongFaTrait3.base_stats, strings.S.multiplier_speed, 0.11f);
            SafeSetStat(GongFaTrait3.base_stats, strings.S.multiplier_damage, 0.09f);
            SafeSetStat(GongFaTrait3.base_stats, strings.S.multiplier_health, 0.2f);
            SafeSetStat(GongFaTrait3.base_stats, strings.S.lifespan, 11f);
            SafeSetStat(GongFaTrait3.base_stats, strings.S.stamina, 19f);
            SafeSetStat(GongFaTrait3.base_stats, strings.S.area_of_effect, 11f);
            GongFaTrait3.rate_inherit = 5;
            AssetManager.traits.add(GongFaTrait3);

            ActorTrait GongFaTrait4 = CreateTrait("GongFaTrait4", "trait/GongFaTrait4", "GongFa");
            SafeSetStat(GongFaTrait4.base_stats, strings.S.multiplier_speed, 0.08f);
            SafeSetStat(GongFaTrait4.base_stats, strings.S.multiplier_damage, 0.09f);
            SafeSetStat(GongFaTrait4.base_stats, strings.S.multiplier_health, 0.19f);
            SafeSetStat(GongFaTrait4.base_stats, strings.S.lifespan, 10f);
            SafeSetStat(GongFaTrait4.base_stats, strings.S.stamina, 10f);
            SafeSetStat(GongFaTrait4.base_stats, strings.S.area_of_effect, 10f);
            GongFaTrait4.rate_inherit = 5;
            AssetManager.traits.add(GongFaTrait4);

            ActorTrait GongFaTrait5 = CreateTrait("GongFaTrait5", "trait/GongFaTrait5", "GongFa");
            SafeSetStat(GongFaTrait5.base_stats, strings.S.multiplier_speed, 0.07f);
            SafeSetStat(GongFaTrait5.base_stats, strings.S.multiplier_damage, 0.18f);
            SafeSetStat(GongFaTrait5.base_stats, strings.S.multiplier_health, 0.1f);
            SafeSetStat(GongFaTrait5.base_stats, strings.S.lifespan, 18f);
            SafeSetStat(GongFaTrait5.base_stats, strings.S.stamina, 12f);
            SafeSetStat(GongFaTrait5.base_stats, strings.S.area_of_effect, 11f);
            GongFaTrait5.rate_inherit = 5;
            AssetManager.traits.add(GongFaTrait5);

            ActorTrait GongFaTrait6 = CreateTrait("GongFaTrait6", "trait/GongFaTrait6", "GongFa");
            SafeSetStat(GongFaTrait6.base_stats, strings.S.multiplier_speed, 0.11f);
            SafeSetStat(GongFaTrait6.base_stats, strings.S.multiplier_damage, 0.11f);
            SafeSetStat(GongFaTrait6.base_stats, strings.S.multiplier_health, 0.15f);
            SafeSetStat(GongFaTrait6.base_stats, strings.S.lifespan, 15f);
            SafeSetStat(GongFaTrait6.base_stats, strings.S.stamina, 15f);
            SafeSetStat(GongFaTrait6.base_stats, strings.S.area_of_effect, 15f);
            GongFaTrait6.rate_inherit = 5;
            AssetManager.traits.add(GongFaTrait6);

            ActorTrait GongFaTrait7 = CreateTrait("GongFaTrait7", "trait/GongFaTrait7", "GongFa");
            GongFaTrait7.rarity = Rarity.R3_Legendary;
            SafeSetStat(GongFaTrait7.base_stats, strings.S.multiplier_speed, 0.19f);
            SafeSetStat(GongFaTrait7.base_stats, strings.S.multiplier_damage, 0.19f);
            SafeSetStat(GongFaTrait7.base_stats, strings.S.multiplier_health, 0.19f);
            SafeSetStat(GongFaTrait7.base_stats, strings.S.lifespan, 19f);
            SafeSetStat(GongFaTrait7.base_stats, strings.S.stamina, 19f);
            SafeSetStat(GongFaTrait7.base_stats, strings.S.area_of_effect, 19f);
            GongFaTrait7.rate_inherit = 5;
            AssetManager.traits.add(GongFaTrait7);

            ActorTrait GongFaTrait8 = CreateTrait("GongFaTrait8", "trait/GongFaTrait8", "GongFa");
            SafeSetStat(GongFaTrait8.base_stats, strings.S.multiplier_speed, 0.09f);
            SafeSetStat(GongFaTrait8.base_stats, strings.S.multiplier_damage, 0.13f);
            SafeSetStat(GongFaTrait8.base_stats, strings.S.multiplier_health, 0.17f);
            SafeSetStat(GongFaTrait8.base_stats, strings.S.lifespan, 14f);
            SafeSetStat(GongFaTrait8.base_stats, strings.S.stamina, 13f);
            SafeSetStat(GongFaTrait8.base_stats, strings.S.area_of_effect, 11f);
            GongFaTrait8.rate_inherit = 5;
            AssetManager.traits.add(GongFaTrait8);

            ActorTrait GongFaTrait9 = CreateTrait("GongFaTrait9", "trait/GongFaTrait9", "GongFa");
            SafeSetStat(GongFaTrait9.base_stats, strings.S.multiplier_speed, 0.16f);
            SafeSetStat(GongFaTrait9.base_stats, strings.S.multiplier_damage, 0.14f);
            SafeSetStat(GongFaTrait9.base_stats, strings.S.multiplier_health, 0.12f);
            SafeSetStat(GongFaTrait9.base_stats, strings.S.lifespan, 14f);
            SafeSetStat(GongFaTrait9.base_stats, strings.S.stamina, 16f);
            SafeSetStat(GongFaTrait9.base_stats, strings.S.area_of_effect, 12f);
            GongFaTrait9.rate_inherit = 5;
            AssetManager.traits.add(GongFaTrait9);

            ActorTrait GongFaTrait10 = CreateTrait("GongFaTrait10", "trait/GongFaTrait10", "GongFa");
            SafeSetStat(GongFaTrait10.base_stats, strings.S.multiplier_speed, 0.17f);
            SafeSetStat(GongFaTrait10.base_stats, strings.S.multiplier_damage, 0.13f);
            SafeSetStat(GongFaTrait10.base_stats, strings.S.multiplier_health, 0.11f);
            SafeSetStat(GongFaTrait10.base_stats, strings.S.lifespan, 14f);
            SafeSetStat(GongFaTrait10.base_stats, strings.S.stamina, 12f);
            SafeSetStat(GongFaTrait10.base_stats, strings.S.area_of_effect, 11f);
            GongFaTrait10.rate_inherit = 5;
            AssetManager.traits.add(GongFaTrait10);

            ActorTrait GongFaTrait11 = CreateTrait("GongFaTrait11", "trait/GongFaTrait11", "GongFa");
            SafeSetStat(GongFaTrait11.base_stats, strings.S.multiplier_speed, 0.09f);
            SafeSetStat(GongFaTrait11.base_stats, strings.S.multiplier_damage, 0.09f);
            SafeSetStat(GongFaTrait11.base_stats, strings.S.multiplier_health, 0.3f);
            SafeSetStat(GongFaTrait11.base_stats, strings.S.lifespan, 24f);
            SafeSetStat(GongFaTrait11.base_stats, strings.S.stamina, 40f);
            SafeSetStat(GongFaTrait11.base_stats, strings.S.area_of_effect, 20f);
            GongFaTrait11.rate_inherit = 5;
            AssetManager.traits.add(GongFaTrait11);

            ActorTrait GongFaTrait13 = CreateTrait("GongFaTrait13", "trait/GongFaTrait13", "GongFa");
            SafeSetStat(GongFaTrait13.base_stats, strings.S.multiplier_speed, 0.1f);
            SafeSetStat(GongFaTrait13.base_stats, strings.S.multiplier_damage, 0.3f);
            SafeSetStat(GongFaTrait13.base_stats, strings.S.multiplier_health, 0.1f);
            SafeSetStat(GongFaTrait13.base_stats, strings.S.lifespan, 15f);
            SafeSetStat(GongFaTrait13.base_stats, strings.S.stamina, 90f);
            SafeSetStat(GongFaTrait13.base_stats, strings.S.area_of_effect, 30f);
            GongFaTrait13.rate_inherit = 5;
            AssetManager.traits.add(GongFaTrait13);

            // 邪功特质 - 武者功法组（10%气血值）
            ActorTrait EvilGongFa1 = CreateTrait("EvilGongFa1", "trait/EvilGongFa1", "GongFa");
            EvilGongFa1.rarity = Rarity.R2_Epic;
            SafeSetStat(EvilGongFa1.base_stats, strings.S.multiplier_speed, 0.15f);
            SafeSetStat(EvilGongFa1.base_stats, strings.S.multiplier_damage, 0.1f);
            SafeSetStat(EvilGongFa1.base_stats, strings.S.multiplier_health, 0.05f);
            SafeSetStat(EvilGongFa1.base_stats, strings.S.lifespan, -100f);
            SafeSetStat(EvilGongFa1.base_stats, strings.S.stamina, 20f);
            AssetManager.traits.add(EvilGongFa1);

            ActorTrait GongFaTrait14 = CreateTrait("GongFaTrait14", "trait/GongFaTrait14", "GongFa");
            SafeSetStat(GongFaTrait14.base_stats, strings.S.multiplier_speed, 0.1f);
            SafeSetStat(GongFaTrait14.base_stats, strings.S.multiplier_damage, 0.1f);
            SafeSetStat(GongFaTrait14.base_stats, strings.S.multiplier_health, 0.1f);
            SafeSetStat(GongFaTrait14.base_stats, strings.S.multiplier_lifespan, -0.2f);
            SafeSetStat(GongFaTrait14.base_stats, strings.S.stamina, 45f);
            SafeSetStat(GongFaTrait14.base_stats, strings.S.area_of_effect, 15f);
            GongFaTrait14.rate_inherit = 5;
            AssetManager.traits.add(GongFaTrait14);

            ActorTrait congenitalPerfection = CreateTrait("congenitalPerfection", "trait/congenitalPerfection", "MartialFoundations");
            AssetManager.traits.add(congenitalPerfection);

            // 定义五个等级的血脉特质
            ActorTrait martialBloodline1 = CreateTrait("martialBloodline1", "trait/martialBloodline1", "MartialBloodline");
            martialBloodline1.rate_inherit = 10;
            SafeSetStat(martialBloodline1.base_stats, strings.S.multiplier_health, 0.01f);
            SafeSetStat(martialBloodline1.base_stats, strings.S.multiplier_damage, 0.01f);
            SafeSetStat(martialBloodline1.base_stats, strings.S.lifespan, 5f);
            AssetManager.traits.add(martialBloodline1);

            ActorTrait martialBloodline2 = CreateTrait("martialBloodline2", "trait/martialBloodline2", "MartialBloodline");
            martialBloodline2.rate_inherit = 9;
            SafeSetStat(martialBloodline2.base_stats, strings.S.multiplier_health, 0.05f);
            SafeSetStat(martialBloodline2.base_stats, strings.S.multiplier_damage, 0.05f);
            SafeSetStat(martialBloodline2.base_stats, strings.S.lifespan, 10f);
            AssetManager.traits.add(martialBloodline2);

            ActorTrait martialBloodline3 = CreateTrait("martialBloodline3", "trait/martialBloodline3", "MartialBloodline");
            martialBloodline3.rate_inherit = 8; 
            SafeSetStat(martialBloodline3.base_stats, strings.S.multiplier_health, 0.1f);
            SafeSetStat(martialBloodline3.base_stats, strings.S.multiplier_damage, 0.1f);
            SafeSetStat(martialBloodline3.base_stats, strings.S.lifespan, 15f);
            AssetManager.traits.add(martialBloodline3);

            ActorTrait martialBloodline4 = CreateTrait("martialBloodline4", "trait/martialBloodline4", "MartialBloodline");
            martialBloodline4.rarity = Rarity.R2_Epic;
            martialBloodline4.rate_inherit = 7;
            SafeSetStat(martialBloodline4.base_stats, strings.S.multiplier_health, 0.15f);
            SafeSetStat(martialBloodline4.base_stats, strings.S.multiplier_damage, 0.15f);
            SafeSetStat(martialBloodline4.base_stats, strings.S.lifespan, 20f);
            AssetManager.traits.add(martialBloodline4);

            ActorTrait martialBloodline5 = CreateTrait("martialBloodline5", "trait/martialBloodline5", "MartialBloodline");
            martialBloodline5.rarity = Rarity.R2_Epic;
            martialBloodline5.rate_inherit = 6; 
            SafeSetStat(martialBloodline5.base_stats, strings.S.multiplier_health, 0.2f);
            SafeSetStat(martialBloodline5.base_stats, strings.S.multiplier_damage, 0.2f);
            SafeSetStat(martialBloodline5.base_stats, strings.S.lifespan, 30f);
            AssetManager.traits.add(martialBloodline5);

            ActorTrait martialBloodline6 = CreateTrait("martialBloodline6", "trait/martialBloodline6", "MartialBloodline");
            martialBloodline6.rarity = Rarity.R3_Legendary;
            martialBloodline6.rate_inherit = 5; 
            SafeSetStat(martialBloodline6.base_stats, strings.S.multiplier_health, 0.25f);
            SafeSetStat(martialBloodline6.base_stats, strings.S.multiplier_damage, 0.25f);
            SafeSetStat(martialBloodline6.base_stats, strings.S.lifespan, 50f);
            AssetManager.traits.add(martialBloodline6);

            ActorTrait martialBloodline7 = CreateTrait("martialBloodline7", "trait/martialBloodline7", "MartialBloodline");
            martialBloodline7.rarity = Rarity.R3_Legendary;
            martialBloodline7.rate_inherit = 1; 
            SafeSetStat(martialBloodline7.base_stats, strings.S.multiplier_health, 0.3f);
            SafeSetStat(martialBloodline7.base_stats, strings.S.multiplier_damage, 0.3f);
            SafeSetStat(martialBloodline7.base_stats, strings.S.lifespan, 100f);
            AssetManager.traits.add(martialBloodline7);

            ActorTrait midGongFaTrait1 = CreateTrait("MidGongFaTrait1", "trait/MidGongFaTrait1", "MidGongFa");
            SafeSetStat(midGongFaTrait1.base_stats, strings.S.multiplier_speed, 0.04f);
            SafeSetStat(midGongFaTrait1.base_stats, strings.S.multiplier_damage, 0.05f);
            SafeSetStat(midGongFaTrait1.base_stats, strings.S.multiplier_health, 0.06f);
            SafeSetStat(midGongFaTrait1.base_stats, strings.S.lifespan, 8f);
            SafeSetStat(midGongFaTrait1.base_stats, strings.S.stamina, 10f);
            SafeSetStat(midGongFaTrait1.base_stats, strings.S.area_of_effect, 10f);
            SafeSetStat(midGongFaTrait1.base_stats, strings.S.skill_combat, 1f);
            SafeSetStat(midGongFaTrait1.base_stats, strings.S.warfare, 6f);
            SafeSetStat(midGongFaTrait1.base_stats, strings.S.diplomacy, 4f);
            SafeSetStat(midGongFaTrait1.base_stats, strings.S.stewardship, 4f);
            SafeSetStat(midGongFaTrait1.base_stats, strings.S.opinion, 4f);
            SafeSetStat(midGongFaTrait1.base_stats, strings.S.cities, 5f);
            midGongFaTrait1.rate_inherit = 5;
            AssetManager.traits.add(midGongFaTrait1);

            ActorTrait midGongFaTrait12 = CreateTrait("MidGongFaTrait12", "trait/MidGongFaTrait12", "MidGongFa");
            SafeSetStat(midGongFaTrait12.base_stats, strings.S.multiplier_speed, 0.01f);
            SafeSetStat(midGongFaTrait12.base_stats, strings.S.multiplier_damage, 0.09f);
            SafeSetStat(midGongFaTrait12.base_stats, strings.S.multiplier_health, 0.04f);
            SafeSetStat(midGongFaTrait12.base_stats, strings.S.lifespan, 5f);
            SafeSetStat(midGongFaTrait12.base_stats, strings.S.stamina, 12f);
            SafeSetStat(midGongFaTrait12.base_stats, strings.S.area_of_effect, 12f);
            SafeSetStat(midGongFaTrait12.base_stats, strings.S.skill_combat, 0.1f);
            SafeSetStat(midGongFaTrait12.base_stats, strings.S.warfare, 9f);
            SafeSetStat(midGongFaTrait12.base_stats, strings.S.diplomacy, 4f);
            SafeSetStat(midGongFaTrait12.base_stats, strings.S.stewardship, 8f);
            SafeSetStat(midGongFaTrait12.base_stats, strings.S.opinion, 7f);
            SafeSetStat(midGongFaTrait12.base_stats, strings.S.cities, 8f);
            midGongFaTrait12.rate_inherit = 5;
            AssetManager.traits.add(midGongFaTrait12);

            ActorTrait midGongFaTrait13 = CreateTrait("MidGongFaTrait13", "trait/MidGongFaTrait13", "MidGongFa");
            SafeSetStat(midGongFaTrait13.base_stats, strings.S.multiplier_speed, 0.06f);
            SafeSetStat(midGongFaTrait13.base_stats, strings.S.multiplier_damage, 0.06f);
            SafeSetStat(midGongFaTrait13.base_stats, strings.S.multiplier_health, 0.06f);
            SafeSetStat(midGongFaTrait13.base_stats, strings.S.lifespan, 6f);
            SafeSetStat(midGongFaTrait13.base_stats, strings.S.stamina, 60f);
            SafeSetStat(midGongFaTrait13.base_stats, strings.S.area_of_effect, 6f);
            SafeSetStat(midGongFaTrait13.base_stats, strings.S.skill_combat, 0.6f);
            SafeSetStat(midGongFaTrait13.base_stats, strings.S.warfare, 6f);
            SafeSetStat(midGongFaTrait13.base_stats, strings.S.diplomacy, 6f);
            SafeSetStat(midGongFaTrait13.base_stats, strings.S.stewardship, 6f);
            SafeSetStat(midGongFaTrait13.base_stats, strings.S.opinion, 6f);
            SafeSetStat(midGongFaTrait13.base_stats, strings.S.cities, 6f);
            midGongFaTrait13.rate_inherit = 5;
            AssetManager.traits.add(midGongFaTrait13);

            ActorTrait midGongFaTrait2 = CreateTrait("MidGongFaTrait2", "trait/MidGongFaTrait2", "MidGongFa");
            SafeSetStat(midGongFaTrait2.base_stats, strings.S.multiplier_speed, 0.08f);
            SafeSetStat(midGongFaTrait2.base_stats, strings.S.multiplier_damage, 0.1f);
            SafeSetStat(midGongFaTrait2.base_stats, strings.S.multiplier_health, 0.1f);
            SafeSetStat(midGongFaTrait2.base_stats, strings.S.lifespan, 15f);
            SafeSetStat(midGongFaTrait2.base_stats, strings.S.stamina, 20f);
            SafeSetStat(midGongFaTrait2.base_stats, strings.S.area_of_effect, 20f);
            SafeSetStat(midGongFaTrait2.base_stats, strings.S.skill_combat, 0.4f);
            SafeSetStat(midGongFaTrait2.base_stats, strings.S.warfare, 8f);
            SafeSetStat(midGongFaTrait2.base_stats, strings.S.diplomacy, 4f);
            SafeSetStat(midGongFaTrait2.base_stats, strings.S.stewardship, 2f);
            SafeSetStat(midGongFaTrait2.base_stats, strings.S.opinion, 4f);
            SafeSetStat(midGongFaTrait2.base_stats, strings.S.cities, 2f);
            midGongFaTrait2.rate_inherit = 5;
            AssetManager.traits.add(midGongFaTrait2);

            ActorTrait midGongFaTrait3 = CreateTrait("MidGongFaTrait3", "trait/MidGongFaTrait3", "MidGongFa");
            SafeSetStat(midGongFaTrait3.base_stats, strings.S.multiplier_speed, 0.08f);
            SafeSetStat(midGongFaTrait3.base_stats, strings.S.multiplier_damage, 0.05f);
            SafeSetStat(midGongFaTrait3.base_stats, strings.S.multiplier_health, 0.2f);
            SafeSetStat(midGongFaTrait3.base_stats, strings.S.lifespan, 16f);
            SafeSetStat(midGongFaTrait3.base_stats, strings.S.stamina, 12f);
            SafeSetStat(midGongFaTrait3.base_stats, strings.S.area_of_effect, 18f);
            SafeSetStat(midGongFaTrait3.base_stats, strings.S.skill_combat, 1f);
            SafeSetStat(midGongFaTrait3.base_stats, strings.S.warfare, 10f);
            SafeSetStat(midGongFaTrait3.base_stats, strings.S.diplomacy, 4f);
            SafeSetStat(midGongFaTrait3.base_stats, strings.S.stewardship, 2f);
            SafeSetStat(midGongFaTrait3.base_stats, strings.S.opinion, 8f);
            SafeSetStat(midGongFaTrait3.base_stats, strings.S.cities, 2f);
            midGongFaTrait3.rate_inherit = 5;
            AssetManager.traits.add(midGongFaTrait3);

            ActorTrait midGongFaTrait4 = CreateTrait("MidGongFaTrait4", "trait/MidGongFaTrait4", "MidGongFa");
            SafeSetStat(midGongFaTrait4.base_stats, strings.S.multiplier_speed, 0.3f);
            SafeSetStat(midGongFaTrait4.base_stats, strings.S.multiplier_damage, 0.04f);
            SafeSetStat(midGongFaTrait4.base_stats, strings.S.multiplier_health, 0.08f);
            SafeSetStat(midGongFaTrait4.base_stats, strings.S.lifespan, 6f);
            SafeSetStat(midGongFaTrait4.base_stats, strings.S.stamina, 10f);
            SafeSetStat(midGongFaTrait4.base_stats, strings.S.area_of_effect, 16f);
            SafeSetStat(midGongFaTrait4.base_stats, strings.S.skill_combat, 0.8f);
            SafeSetStat(midGongFaTrait4.base_stats, strings.S.warfare, 2f);
            SafeSetStat(midGongFaTrait4.base_stats, strings.S.diplomacy, 8f);
            SafeSetStat(midGongFaTrait4.base_stats, strings.S.stewardship, 4f);
            SafeSetStat(midGongFaTrait4.base_stats, strings.S.opinion, 8f);
            SafeSetStat(midGongFaTrait4.base_stats, strings.S.cities, 2f);
            midGongFaTrait4.rate_inherit = 5;
            AssetManager.traits.add(midGongFaTrait4);

            ActorTrait midGongFaTrait5 = CreateTrait("MidGongFaTrait5", "trait/MidGongFaTrait5", "MidGongFa");
            SafeSetStat(midGongFaTrait5.base_stats, strings.S.multiplier_speed, 0.03f);
            SafeSetStat(midGongFaTrait5.base_stats, strings.S.multiplier_damage, 0.3f);
            SafeSetStat(midGongFaTrait5.base_stats, strings.S.multiplier_health, 0.04f);
            SafeSetStat(midGongFaTrait5.base_stats, strings.S.lifespan, 10f);
            SafeSetStat(midGongFaTrait5.base_stats, strings.S.stamina, 50f);
            SafeSetStat(midGongFaTrait5.base_stats, strings.S.area_of_effect, 20f);
            SafeSetStat(midGongFaTrait5.base_stats, strings.S.skill_combat, 1f);
            SafeSetStat(midGongFaTrait5.base_stats, strings.S.warfare, 10f);
            SafeSetStat(midGongFaTrait5.base_stats, strings.S.diplomacy, 10f);
            SafeSetStat(midGongFaTrait5.base_stats, strings.S.stewardship, 2f);
            SafeSetStat(midGongFaTrait5.base_stats, strings.S.opinion, 2f);
            SafeSetStat(midGongFaTrait5.base_stats, strings.S.cities, 2f);
            midGongFaTrait5.rate_inherit = 5;
            AssetManager.traits.add(midGongFaTrait5);

            ActorTrait midGongFaTrait6 = CreateTrait("MidGongFaTrait6", "trait/MidGongFaTrait6", "MidGongFa");
            SafeSetStat(midGongFaTrait6.base_stats, strings.S.multiplier_speed, 0.1f);
            SafeSetStat(midGongFaTrait6.base_stats, strings.S.multiplier_damage, 0.06f);
            SafeSetStat(midGongFaTrait6.base_stats, strings.S.multiplier_health, 0.04f);
            SafeSetStat(midGongFaTrait6.base_stats, strings.S.lifespan, 20f);
            SafeSetStat(midGongFaTrait6.base_stats, strings.S.stamina, 110f);
            SafeSetStat(midGongFaTrait6.base_stats, strings.S.area_of_effect, 20f);
            SafeSetStat(midGongFaTrait6.base_stats, strings.S.skill_combat, 1f);
            SafeSetStat(midGongFaTrait6.base_stats, strings.S.warfare, 6f);
            SafeSetStat(midGongFaTrait6.base_stats, strings.S.diplomacy, 8f);
            SafeSetStat(midGongFaTrait6.base_stats, strings.S.stewardship, 2f);
            SafeSetStat(midGongFaTrait6.base_stats, strings.S.opinion, 8f);
            SafeSetStat(midGongFaTrait6.base_stats, strings.S.cities, 2f);
            midGongFaTrait6.rate_inherit = 5;
            AssetManager.traits.add(midGongFaTrait6);

            ActorTrait midGongFaTrait7 = CreateTrait("MidGongFaTrait7", "trait/MidGongFaTrait7", "MidGongFa");
            SafeSetStat(midGongFaTrait7.base_stats, strings.S.multiplier_speed, 0.09f);
            SafeSetStat(midGongFaTrait7.base_stats, strings.S.multiplier_damage, 0.09f);
            SafeSetStat(midGongFaTrait7.base_stats, strings.S.multiplier_health, 0.09f);
            SafeSetStat(midGongFaTrait7.base_stats, strings.S.lifespan, 4f);
            SafeSetStat(midGongFaTrait7.base_stats, strings.S.stamina, 60f);
            SafeSetStat(midGongFaTrait7.base_stats, strings.S.area_of_effect, 18f);
            SafeSetStat(midGongFaTrait7.base_stats, strings.S.skill_combat, 0.4f);
            SafeSetStat(midGongFaTrait7.base_stats, strings.S.warfare, 2f);
            SafeSetStat(midGongFaTrait7.base_stats, strings.S.diplomacy, 4f);
            SafeSetStat(midGongFaTrait7.base_stats, strings.S.stewardship, 4f);
            SafeSetStat(midGongFaTrait7.base_stats, strings.S.opinion, 8f);
            SafeSetStat(midGongFaTrait7.base_stats, strings.S.cities, 10f);
            midGongFaTrait7.rate_inherit = 5;
            AssetManager.traits.add(midGongFaTrait7);

            ActorTrait midGongFaTrait8 = CreateTrait("MidGongFaTrait8", "trait/MidGongFaTrait8", "MidGongFa");
            SafeSetStat(midGongFaTrait8.base_stats, strings.S.multiplier_speed, 0.1f);
            SafeSetStat(midGongFaTrait8.base_stats, strings.S.multiplier_damage, 0.2f);
            SafeSetStat(midGongFaTrait8.base_stats, strings.S.multiplier_health, 0.06f);
            SafeSetStat(midGongFaTrait8.base_stats, strings.S.lifespan, 20f);
            SafeSetStat(midGongFaTrait8.base_stats, strings.S.stamina, 40f);
            SafeSetStat(midGongFaTrait8.base_stats, strings.S.area_of_effect, 40f);
            SafeSetStat(midGongFaTrait8.base_stats, strings.S.skill_combat, 2f);
            SafeSetStat(midGongFaTrait8.base_stats, strings.S.warfare, 10f);
            SafeSetStat(midGongFaTrait8.base_stats, strings.S.diplomacy, 2f);
            SafeSetStat(midGongFaTrait8.base_stats, strings.S.stewardship, 6f);
            SafeSetStat(midGongFaTrait8.base_stats, strings.S.opinion, 8f);
            SafeSetStat(midGongFaTrait8.base_stats, strings.S.cities, 6f);
            midGongFaTrait8.rate_inherit = 5;
            AssetManager.traits.add(midGongFaTrait8);

            ActorTrait midGongFaTrait9 = CreateTrait("MidGongFaTrait9", "trait/MidGongFaTrait9", "MidGongFa");
            SafeSetStat(midGongFaTrait9.base_stats, strings.S.multiplier_speed, 0.01f);
            SafeSetStat(midGongFaTrait9.base_stats, strings.S.multiplier_damage, 0.25f);
            SafeSetStat(midGongFaTrait9.base_stats, strings.S.multiplier_health, 0.01f);
            SafeSetStat(midGongFaTrait9.base_stats, strings.S.lifespan, -10f);
            SafeSetStat(midGongFaTrait9.base_stats, strings.S.stamina, 10f);
            SafeSetStat(midGongFaTrait9.base_stats, strings.S.area_of_effect, 1f);
            SafeSetStat(midGongFaTrait9.base_stats, strings.S.skill_combat, 0.1f);
            SafeSetStat(midGongFaTrait9.base_stats, strings.S.warfare, 12f);
            SafeSetStat(midGongFaTrait9.base_stats, strings.S.diplomacy, -1f);
            SafeSetStat(midGongFaTrait9.base_stats, strings.S.stewardship, -1f);
            SafeSetStat(midGongFaTrait9.base_stats, strings.S.opinion, -1f);
            SafeSetStat(midGongFaTrait9.base_stats, strings.S.cities, -1f);
            midGongFaTrait9.rate_inherit = 5;
            AssetManager.traits.add(midGongFaTrait9);

            ActorTrait midGongFaTrait10 = CreateTrait("MidGongFaTrait10", "trait/MidGongFaTrait10", "MidGongFa");
            SafeSetStat(midGongFaTrait10.base_stats, strings.S.multiplier_speed, 0.01f);
            SafeSetStat(midGongFaTrait10.base_stats, strings.S.multiplier_damage, 0.01f);
            SafeSetStat(midGongFaTrait10.base_stats, strings.S.multiplier_health, 0.2f);
            SafeSetStat(midGongFaTrait10.base_stats, strings.S.lifespan, 80f);
            SafeSetStat(midGongFaTrait10.base_stats, strings.S.stamina, 20f);
            SafeSetStat(midGongFaTrait10.base_stats, strings.S.area_of_effect, 11f);
            SafeSetStat(midGongFaTrait10.base_stats, strings.S.skill_combat, 0.1f);
            SafeSetStat(midGongFaTrait10.base_stats, strings.S.warfare, 2f);
            SafeSetStat(midGongFaTrait10.base_stats, strings.S.diplomacy, 4f);
            SafeSetStat(midGongFaTrait10.base_stats, strings.S.stewardship, 2f);
            SafeSetStat(midGongFaTrait10.base_stats, strings.S.opinion, -2f);
            SafeSetStat(midGongFaTrait10.base_stats, strings.S.cities, -2f);
            midGongFaTrait10.rate_inherit = 5;
            AssetManager.traits.add(midGongFaTrait10);

            ActorTrait midGongFaTrait11 = CreateTrait("MidGongFaTrait11", "trait/MidGongFaTrait11", "MidGongFa");
            SafeSetStat(midGongFaTrait11.base_stats, strings.S.multiplier_speed, 0.1f);
            SafeSetStat(midGongFaTrait11.base_stats, strings.S.multiplier_damage, 0.1f);
            SafeSetStat(midGongFaTrait11.base_stats, strings.S.multiplier_health, 0.1f);
            SafeSetStat(midGongFaTrait11.base_stats, strings.S.lifespan, 8f);
            SafeSetStat(midGongFaTrait11.base_stats, strings.S.stamina, 30f);
            SafeSetStat(midGongFaTrait11.base_stats, strings.S.area_of_effect, 40f);
            SafeSetStat(midGongFaTrait11.base_stats, strings.S.skill_combat, 1f);
            SafeSetStat(midGongFaTrait11.base_stats, strings.S.warfare, 4f);
            SafeSetStat(midGongFaTrait11.base_stats, strings.S.diplomacy, 6f);
            SafeSetStat(midGongFaTrait11.base_stats, strings.S.stewardship, 8f);
            SafeSetStat(midGongFaTrait11.base_stats, strings.S.opinion, 2f);
            SafeSetStat(midGongFaTrait11.base_stats, strings.S.cities, 5f);
            midGongFaTrait11.rate_inherit = 5;
            AssetManager.traits.add(midGongFaTrait11);

            ActorTrait midGongFaTrait14 = CreateTrait("MidGongFaTrait14", "trait/MidGongFaTrait14", "MidGongFa");
            SafeSetStat(midGongFaTrait14.base_stats, strings.S.multiplier_speed, 0.1f);
            SafeSetStat(midGongFaTrait14.base_stats, strings.S.multiplier_damage, 0.1f);
            SafeSetStat(midGongFaTrait14.base_stats, strings.S.multiplier_health, 0.1f);
            SafeSetStat(midGongFaTrait14.base_stats, strings.S.multiplier_lifespan, -0.1f);
            SafeSetStat(midGongFaTrait14.base_stats, strings.S.stamina, 30f);
            SafeSetStat(midGongFaTrait14.base_stats, strings.S.area_of_effect, 40f);
            SafeSetStat(midGongFaTrait14.base_stats, strings.S.skill_combat, 1f);
            SafeSetStat(midGongFaTrait14.base_stats, strings.S.warfare, -2f);
            SafeSetStat(midGongFaTrait14.base_stats, strings.S.diplomacy, -3f);
            SafeSetStat(midGongFaTrait14.base_stats, strings.S.stewardship, -4f);
            SafeSetStat(midGongFaTrait14.base_stats, strings.S.opinion, -1f);
            SafeSetStat(midGongFaTrait14.base_stats, strings.S.cities, -2f);
            midGongFaTrait14.rate_inherit = 5;
            AssetManager.traits.add(midGongFaTrait14);

            ActorTrait celestialGrotto1 = CreateTrait("CelestialGrotto1", "trait/CelestialGrotto1", "CelestialGrotto");
            celestialGrotto1.rarity = Rarity.R3_Legendary;
            SafeSetStat(celestialGrotto1.base_stats, strings.S.multiplier_damage, 0.4f);
            SafeSetStat(celestialGrotto1.base_stats, strings.S.multiplier_health, 0.4f);
            SafeSetStat(celestialGrotto1.base_stats, strings.S.intelligence, 100f);
            SafeSetStat(celestialGrotto1.base_stats, strings.S.lifespan, 180f);
            SafeSetStat(celestialGrotto1.base_stats, strings.S.stamina, 300f);
            SafeSetStat(celestialGrotto1.base_stats, strings.S.area_of_effect, 8f);
            SafeSetStat(celestialGrotto1.base_stats, strings.S.critical_chance, 0.2f);
            SafeSetStat(celestialGrotto1.base_stats, strings.S.targets, 10f);
            SafeSetStat(celestialGrotto1.base_stats, strings.S.accuracy, 60f);
            SafeSetStat(celestialGrotto1.base_stats, "Accuracy", 50f);
            SafeSetStat(celestialGrotto1.base_stats, "Dodge", 50f);
            AssetManager.traits.add(celestialGrotto1);

            ActorTrait celestialGrotto2 = CreateTrait("CelestialGrotto2", "trait/CelestialGrotto2", "CelestialGrotto");
            celestialGrotto2.rarity = Rarity.R3_Legendary;
            SafeSetStat(celestialGrotto2.base_stats, strings.S.multiplier_damage, 0.35f);
            SafeSetStat(celestialGrotto2.base_stats, strings.S.multiplier_health, 0.45f);
            SafeSetStat(celestialGrotto2.base_stats, strings.S.intelligence, 40f);
            SafeSetStat(celestialGrotto2.base_stats, strings.S.lifespan, 120f);
            SafeSetStat(celestialGrotto2.base_stats, strings.S.stamina, 200f);
            SafeSetStat(celestialGrotto2.base_stats, strings.S.area_of_effect, 18f);
            SafeSetStat(celestialGrotto2.base_stats, strings.S.critical_chance, 0.8f);
            SafeSetStat(celestialGrotto2.base_stats, strings.S.targets, 15f);
            SafeSetStat(celestialGrotto2.base_stats, strings.S.accuracy, 40f);
            SafeSetStat(celestialGrotto2.base_stats, "Accuracy", 40f);
            SafeSetStat(celestialGrotto2.base_stats, "Dodge", 60f);
            AssetManager.traits.add(celestialGrotto2);

            ActorTrait celestialGrotto3 = CreateTrait("CelestialGrotto3", "trait/CelestialGrotto3", "CelestialGrotto");
            celestialGrotto3.rarity = Rarity.R3_Legendary;
            SafeSetStat(celestialGrotto3.base_stats, strings.S.multiplier_damage, 0.3f);
            SafeSetStat(celestialGrotto3.base_stats, strings.S.multiplier_health, 0.2f);
            SafeSetStat(celestialGrotto3.base_stats, strings.S.intelligence, 120f);
            SafeSetStat(celestialGrotto3.base_stats, strings.S.lifespan, 150f);
            SafeSetStat(celestialGrotto3.base_stats, strings.S.stamina, 240f);
            SafeSetStat(celestialGrotto3.base_stats, strings.S.area_of_effect, 6f);
            SafeSetStat(celestialGrotto3.base_stats, strings.S.critical_chance, 0.5f);
            SafeSetStat(celestialGrotto3.base_stats, strings.S.targets, 8f);
            SafeSetStat(celestialGrotto3.base_stats, strings.S.accuracy, 50f);
            SafeSetStat(celestialGrotto3.base_stats, "Accuracy", 60f);
            SafeSetStat(celestialGrotto3.base_stats, "Dodge", 40f);
            AssetManager.traits.add(celestialGrotto3);

            ActorTrait celestialGrotto4 = CreateTrait("CelestialGrotto4", "trait/CelestialGrotto4", "CelestialGrotto");
            celestialGrotto4.rarity = Rarity.R3_Legendary;
            SafeSetStat(celestialGrotto4.base_stats, strings.S.multiplier_damage, 0.36f);
            SafeSetStat(celestialGrotto4.base_stats, strings.S.multiplier_health, 0.18f);
            SafeSetStat(celestialGrotto4.base_stats, strings.S.intelligence, 60f);
            SafeSetStat(celestialGrotto4.base_stats, strings.S.lifespan, 140f);
            SafeSetStat(celestialGrotto4.base_stats, strings.S.stamina, 240f);
            SafeSetStat(celestialGrotto4.base_stats, strings.S.area_of_effect, 6f);
            SafeSetStat(celestialGrotto4.base_stats, strings.S.critical_chance, 0.5f);
            SafeSetStat(celestialGrotto4.base_stats, strings.S.targets, 15f);
            SafeSetStat(celestialGrotto4.base_stats, strings.S.accuracy, 60f);
            SafeSetStat(celestialGrotto4.base_stats, "Accuracy", 70f);
            SafeSetStat(celestialGrotto4.base_stats, "Dodge", 30f);
            AssetManager.traits.add(celestialGrotto4);

            ActorTrait celestialGrotto5 = CreateTrait("CelestialGrotto5", "trait/CelestialGrotto5", "CelestialGrotto");
            celestialGrotto5.rarity = Rarity.R3_Legendary;
            SafeSetStat(celestialGrotto5.base_stats, strings.S.multiplier_damage, 0.49f);
            SafeSetStat(celestialGrotto5.base_stats, strings.S.multiplier_health, 0.1f);
            SafeSetStat(celestialGrotto5.base_stats, strings.S.intelligence, 260f);
            SafeSetStat(celestialGrotto5.base_stats, strings.S.lifespan, 140f);
            SafeSetStat(celestialGrotto5.base_stats, strings.S.stamina, 40f);
            SafeSetStat(celestialGrotto5.base_stats, strings.S.area_of_effect, 2f);
            SafeSetStat(celestialGrotto5.base_stats, strings.S.critical_chance, 0.1f);
            SafeSetStat(celestialGrotto5.base_stats, strings.S.targets, 10f);
            SafeSetStat(celestialGrotto5.base_stats, strings.S.accuracy, 20f);
            SafeSetStat(celestialGrotto5.base_stats, strings.S.targets, 20f);
            SafeSetStat(celestialGrotto5.base_stats, strings.S.accuracy, 45f);
            SafeSetStat(celestialGrotto5.base_stats, "Accuracy", 80f);
            SafeSetStat(celestialGrotto5.base_stats, "Dodge", 20f);
            AssetManager.traits.add(celestialGrotto5);

            ActorTrait celestialGrotto6 = CreateTrait("CelestialGrotto6", "trait/CelestialGrotto6", "CelestialGrotto");
            celestialGrotto6.rarity = Rarity.R3_Legendary;
            SafeSetStat(celestialGrotto6.base_stats, strings.S.multiplier_health, 0.3f);
            SafeSetStat(celestialGrotto6.base_stats, strings.S.multiplier_damage, 0.1f);
            SafeSetStat(celestialGrotto6.base_stats, strings.S.intelligence, 20f);
            SafeSetStat(celestialGrotto6.base_stats, strings.S.lifespan, 200f);
            SafeSetStat(celestialGrotto6.base_stats, strings.S.stamina, 240f);
            SafeSetStat(celestialGrotto6.base_stats, strings.S.area_of_effect, 12f);
            SafeSetStat(celestialGrotto6.base_stats, strings.S.critical_chance, 0.4f);
            SafeSetStat(celestialGrotto6.base_stats, strings.S.targets, 15f);
            SafeSetStat(celestialGrotto6.base_stats, strings.S.accuracy, 60f);
            SafeSetStat(celestialGrotto6.base_stats, "Accuracy", 90f);
            SafeSetStat(celestialGrotto6.base_stats, "Dodge", 10f);
            AssetManager.traits.add(celestialGrotto6);

            ActorTrait celestialGrotto7 = CreateTrait("CelestialGrotto7", "trait/CelestialGrotto7", "CelestialGrotto");
            celestialGrotto7.rarity = Rarity.R3_Legendary;
            SafeSetStat(celestialGrotto7.base_stats, strings.S.multiplier_health, 0.18f);
            SafeSetStat(celestialGrotto7.base_stats, strings.S.multiplier_damage, 0.24f);
            SafeSetStat(celestialGrotto7.base_stats, strings.S.intelligence, 320f);
            SafeSetStat(celestialGrotto7.base_stats, strings.S.lifespan, 160f);
            SafeSetStat(celestialGrotto7.base_stats, strings.S.stamina, 500f);
            SafeSetStat(celestialGrotto7.base_stats, strings.S.area_of_effect, 20f);
            SafeSetStat(celestialGrotto7.base_stats, strings.S.critical_chance, 0.8f);
            SafeSetStat(celestialGrotto7.base_stats, strings.S.targets, 24f);
            SafeSetStat(celestialGrotto7.base_stats, strings.S.accuracy, 64f);
            SafeSetStat(celestialGrotto7.base_stats, "Accuracy", 40f);
            SafeSetStat(celestialGrotto7.base_stats, "Dodge", 60f);
            AssetManager.traits.add(celestialGrotto7);

            ActorTrait celestialGrotto8 = CreateTrait("CelestialGrotto8", "trait/CelestialGrotto8", "CelestialGrotto");
            celestialGrotto8.rarity = Rarity.R3_Legendary;
            SafeSetStat(celestialGrotto8.base_stats, strings.S.multiplier_health, 0.28f);
            SafeSetStat(celestialGrotto8.base_stats, strings.S.multiplier_damage, 0.25f);
            SafeSetStat(celestialGrotto8.base_stats, strings.S.intelligence, 120f);
            SafeSetStat(celestialGrotto8.base_stats, strings.S.lifespan, 200f);
            SafeSetStat(celestialGrotto8.base_stats, strings.S.stamina, 300f);
            SafeSetStat(celestialGrotto8.base_stats, strings.S.area_of_effect, 10f);
            SafeSetStat(celestialGrotto8.base_stats, strings.S.critical_chance, 0.6f);
            SafeSetStat(celestialGrotto8.base_stats, strings.S.targets, 25f);
            SafeSetStat(celestialGrotto8.base_stats, strings.S.accuracy, 70f);
            SafeSetStat(celestialGrotto8.base_stats, "Accuracy", 30f);
            SafeSetStat(celestialGrotto8.base_stats, "Dodge", 70f);
            AssetManager.traits.add(celestialGrotto8);

            ActorTrait celestialGrotto9 = CreateTrait("CelestialGrotto9", "trait/CelestialGrotto9", "CelestialGrotto");
            celestialGrotto9.rarity = Rarity.R3_Legendary;
            SafeSetStat(celestialGrotto9.base_stats, strings.S.multiplier_health, 0.25f);
            SafeSetStat(celestialGrotto9.base_stats, strings.S.multiplier_damage, 0.18f);
            SafeSetStat(celestialGrotto9.base_stats, strings.S.intelligence, 108f);
            SafeSetStat(celestialGrotto9.base_stats, strings.S.lifespan, 125f);
            SafeSetStat(celestialGrotto9.base_stats, strings.S.stamina, 225f);
            SafeSetStat(celestialGrotto9.base_stats, strings.S.area_of_effect, 22f);
            SafeSetStat(celestialGrotto9.base_stats, strings.S.critical_chance, 0.36f);
            SafeSetStat(celestialGrotto9.base_stats, strings.S.targets, 72f);
            SafeSetStat(celestialGrotto9.base_stats, strings.S.accuracy, 50f);
            SafeSetStat(celestialGrotto9.base_stats, "Accuracy", 20f);
            SafeSetStat(celestialGrotto9.base_stats, "Dodge", 80f);
            AssetManager.traits.add(celestialGrotto9);

            ActorTrait celestialGrotto10 = CreateTrait("CelestialGrotto10", "trait/CelestialGrotto10", "CelestialGrotto");
            celestialGrotto10.rarity = Rarity.R3_Legendary;
            SafeSetStat(celestialGrotto10.base_stats, strings.S.multiplier_damage, 0.4f);
            SafeSetStat(celestialGrotto10.base_stats, strings.S.multiplier_health, 0.35f);
            SafeSetStat(celestialGrotto10.base_stats, strings.S.intelligence, 130f);
            SafeSetStat(celestialGrotto10.base_stats, strings.S.lifespan, 160f);
            SafeSetStat(celestialGrotto10.base_stats, strings.S.stamina, 500f);
            SafeSetStat(celestialGrotto10.base_stats, strings.S.area_of_effect, 28f);
            SafeSetStat(celestialGrotto10.base_stats, strings.S.critical_chance, 0.24f);
            SafeSetStat(celestialGrotto10.base_stats, strings.S.targets, 49f);
            SafeSetStat(celestialGrotto10.base_stats, strings.S.accuracy, 50f);
            SafeSetStat(celestialGrotto10.base_stats, "Accuracy", 10f);
            SafeSetStat(celestialGrotto10.base_stats, "Dodge", 90f);
            AssetManager.traits.add(celestialGrotto10);

            ActorTrait celestialGrotto11 = CreateTrait("CelestialGrotto11", "trait/CelestialGrotto11", "CelestialGrotto");
            celestialGrotto11.rarity = Rarity.R3_Legendary;
            SafeSetStat(celestialGrotto11.base_stats, strings.S.multiplier_damage, 0.36f);
            SafeSetStat(celestialGrotto11.base_stats, strings.S.multiplier_health, 0.4f);
            SafeSetStat(celestialGrotto11.base_stats, strings.S.intelligence, 90f);
            SafeSetStat(celestialGrotto11.base_stats, strings.S.lifespan, 180f);
            SafeSetStat(celestialGrotto11.base_stats, strings.S.stamina, 160f);
            SafeSetStat(celestialGrotto11.base_stats, strings.S.area_of_effect, 19f);
            SafeSetStat(celestialGrotto11.base_stats, strings.S.critical_chance, 0.64f);
            SafeSetStat(celestialGrotto11.base_stats, strings.S.targets, 20f);
            SafeSetStat(celestialGrotto11.base_stats, strings.S.accuracy, 99f);
            SafeSetStat(celestialGrotto11.base_stats, "Accuracy", 55f);
            SafeSetStat(celestialGrotto11.base_stats, "Dodge", 45f);
            AssetManager.traits.add(celestialGrotto11);

            ActorTrait celestialGrotto12 = CreateTrait("CelestialGrotto12", "trait/CelestialGrotto12", "CelestialGrotto");
            celestialGrotto12.rarity = Rarity.R3_Legendary;
            SafeSetStat(celestialGrotto12.base_stats, strings.S.multiplier_damage, 0.32f);
            SafeSetStat(celestialGrotto12.base_stats, strings.S.multiplier_health, 0.32f);
            SafeSetStat(celestialGrotto12.base_stats, strings.S.intelligence, 121f);
            SafeSetStat(celestialGrotto12.base_stats, strings.S.lifespan, 199f);
            SafeSetStat(celestialGrotto12.base_stats, strings.S.stamina, 250f);
            SafeSetStat(celestialGrotto12.base_stats, strings.S.area_of_effect, 30f);
            SafeSetStat(celestialGrotto12.base_stats, strings.S.critical_chance, 0.11f);
            SafeSetStat(celestialGrotto12.base_stats, strings.S.targets, 100f);
            SafeSetStat(celestialGrotto12.base_stats, strings.S.accuracy, 99f);
            SafeSetStat(celestialGrotto12.base_stats, "Accuracy", 65f);
            SafeSetStat(celestialGrotto12.base_stats, "Dodge", 35f);
            AssetManager.traits.add(celestialGrotto12);

            ActorTrait celestialGrotto13 = CreateTrait("CelestialGrotto13", "trait/CelestialGrotto13", "CelestialGrotto");
            celestialGrotto13.rarity = Rarity.R3_Legendary;
            SafeSetStat(celestialGrotto13.base_stats, strings.S.multiplier_damage, 0.36f);
            SafeSetStat(celestialGrotto13.base_stats, strings.S.multiplier_health, 0.25f);
            SafeSetStat(celestialGrotto13.base_stats, strings.S.intelligence, 81f);
            SafeSetStat(celestialGrotto13.base_stats, strings.S.lifespan, 125f);
            SafeSetStat(celestialGrotto13.base_stats, strings.S.stamina, 144f);
            SafeSetStat(celestialGrotto13.base_stats, strings.S.area_of_effect, 16f);
            SafeSetStat(celestialGrotto13.base_stats, strings.S.critical_chance, 0.49f);
            SafeSetStat(celestialGrotto13.base_stats, strings.S.targets, 14f);
            SafeSetStat(celestialGrotto13.base_stats, strings.S.accuracy, 49f);
            SafeSetStat(celestialGrotto13.base_stats, "Accuracy", 75f);
            SafeSetStat(celestialGrotto13.base_stats, "Dodge", 25f);
            AssetManager.traits.add(celestialGrotto13);

            ActorTrait celestialGrotto14 = CreateTrait("CelestialGrotto14", "trait/CelestialGrotto14", "CelestialGrotto");
            celestialGrotto14.rarity = Rarity.R3_Legendary;
            SafeSetStat(celestialGrotto14.base_stats, strings.S.multiplier_damage, 0.44f);
            SafeSetStat(celestialGrotto14.base_stats, strings.S.multiplier_health, 0.44f);
            SafeSetStat(celestialGrotto14.base_stats, strings.S.intelligence, 188f);
            SafeSetStat(celestialGrotto14.base_stats, strings.S.lifespan, 188f);
            SafeSetStat(celestialGrotto14.base_stats, strings.S.stamina, 188f);
            SafeSetStat(celestialGrotto14.base_stats, strings.S.area_of_effect, 18f);
            SafeSetStat(celestialGrotto14.base_stats, strings.S.critical_chance, 0.18f);
            SafeSetStat(celestialGrotto14.base_stats, strings.S.targets, 18f);
            SafeSetStat(celestialGrotto14.base_stats, strings.S.accuracy, 88f);
            SafeSetStat(celestialGrotto14.base_stats, "Accuracy", 85f);
            SafeSetStat(celestialGrotto14.base_stats, "Dodge", 15f);
            AssetManager.traits.add(celestialGrotto14);

            ActorTrait celestialGrotto15 = CreateTrait("CelestialGrotto15", "trait/CelestialGrotto15", "CelestialGrotto");
            celestialGrotto15.rarity = Rarity.R3_Legendary;
            SafeSetStat(celestialGrotto15.base_stats, strings.S.multiplier_damage, 0.55f);
            SafeSetStat(celestialGrotto15.base_stats, strings.S.multiplier_health, 0.3f);
            SafeSetStat(celestialGrotto15.base_stats, strings.S.intelligence, 10f);
            SafeSetStat(celestialGrotto15.base_stats, strings.S.lifespan, 99f);
            SafeSetStat(celestialGrotto15.base_stats, strings.S.stamina, 399f);
            SafeSetStat(celestialGrotto15.base_stats, strings.S.area_of_effect, 9f);
            SafeSetStat(celestialGrotto15.base_stats, strings.S.critical_chance, 0.9f);
            SafeSetStat(celestialGrotto15.base_stats, strings.S.targets, 19f);
            SafeSetStat(celestialGrotto15.base_stats, strings.S.accuracy, 99f);
            SafeSetStat(celestialGrotto15.base_stats, "Accuracy", 45f);
            SafeSetStat(celestialGrotto15.base_stats, "Dodge", 55f);
            AssetManager.traits.add(celestialGrotto15);

            ActorTrait celestialGrotto16 = CreateTrait("CelestialGrotto16", "trait/CelestialGrotto16", "CelestialGrotto");
            celestialGrotto16.rarity = Rarity.R3_Legendary;
             SafeSetStat(celestialGrotto16.base_stats, strings.S.multiplier_damage, 0.36f);
            SafeSetStat(celestialGrotto16.base_stats, strings.S.multiplier_health, 0.34f);
            SafeSetStat(celestialGrotto16.base_stats, strings.S.intelligence, 88f);
            SafeSetStat(celestialGrotto16.base_stats, strings.S.lifespan, 175f);
            SafeSetStat(celestialGrotto16.base_stats, strings.S.stamina, 550f);
            SafeSetStat(celestialGrotto16.base_stats, strings.S.area_of_effect, 48f);
            SafeSetStat(celestialGrotto16.base_stats, strings.S.critical_chance, 0.64f);
            SafeSetStat(celestialGrotto16.base_stats, strings.S.targets, 49f);
            SafeSetStat(celestialGrotto16.base_stats, strings.S.accuracy, 36f);
            SafeSetStat(celestialGrotto16.base_stats, "Accuracy", 35f);
            SafeSetStat(celestialGrotto16.base_stats, "Dodge", 65f);
            AssetManager.traits.add(celestialGrotto16);

            ActorTrait celestialGrotto17 = CreateTrait("CelestialGrotto17", "trait/CelestialGrotto17", "CelestialGrotto");
            celestialGrotto17.rarity = Rarity.R3_Legendary;
            SafeSetStat(celestialGrotto17.base_stats, strings.S.multiplier_damage, 0.25f);
            SafeSetStat(celestialGrotto17.base_stats, strings.S.multiplier_health, 0.25f);
            SafeSetStat(celestialGrotto17.base_stats, strings.S.intelligence, 150f);
            SafeSetStat(celestialGrotto17.base_stats, strings.S.lifespan, 170f);
            SafeSetStat(celestialGrotto17.base_stats, strings.S.stamina, 1000f);
            SafeSetStat(celestialGrotto17.base_stats, strings.S.area_of_effect, 36f);
            SafeSetStat(celestialGrotto17.base_stats, strings.S.critical_chance, 0.5f);
            SafeSetStat(celestialGrotto17.base_stats, strings.S.targets, 20f);
            SafeSetStat(celestialGrotto17.base_stats, strings.S.accuracy, 64f);
            SafeSetStat(celestialGrotto17.base_stats, "Accuracy", 25f);
            SafeSetStat(celestialGrotto17.base_stats, "Dodge", 75f);
            AssetManager.traits.add(celestialGrotto17);

            ActorTrait celestialGrotto18 = CreateTrait("CelestialGrotto18", "trait/CelestialGrotto18", "CelestialGrotto");
            celestialGrotto18.rarity = Rarity.R3_Legendary;
            SafeSetStat(celestialGrotto18.base_stats, strings.S.multiplier_damage, 0.36f);
            SafeSetStat(celestialGrotto18.base_stats, strings.S.multiplier_health, 0.18f);
            SafeSetStat(celestialGrotto18.base_stats, strings.S.intelligence, 81f);
            SafeSetStat(celestialGrotto18.base_stats, strings.S.lifespan, 190f);
            SafeSetStat(celestialGrotto18.base_stats, strings.S.stamina, 1080f);
            SafeSetStat(celestialGrotto18.base_stats, strings.S.area_of_effect, 10f);
            SafeSetStat(celestialGrotto18.base_stats, strings.S.critical_chance, 0.45f);
            SafeSetStat(celestialGrotto18.base_stats, strings.S.targets, 99f);
            SafeSetStat(celestialGrotto18.base_stats, strings.S.accuracy, 56f);
            SafeSetStat(celestialGrotto18.base_stats, "Accuracy", 15f);
            SafeSetStat(celestialGrotto18.base_stats, "Dodge", 85f);
            AssetManager.traits.add(celestialGrotto18);

            ActorTrait EmperorSeal = CreateTrait("EmperorSeal", "trait/EmperorSeal", "EmperorSeal");
            EmperorSeal.rarity = Rarity.R3_Legendary;
            SafeSetStat(EmperorSeal.base_stats, strings.S.multiplier_health, 10f); // 10倍生命值
            SafeSetStat(EmperorSeal.base_stats, strings.S.multiplier_damage, 10f); // 10倍伤害
            SafeSetStat(EmperorSeal.base_stats, strings.S.multiplier_speed, 5f);    // 5倍速度
            SafeSetStat(EmperorSeal.base_stats, strings.S.lifespan, 5000f);       // 增加5000年寿命 
            EmperorSeal.action_attack_target += traitAction.DiYinTrueDamage_AttackAction;     
            AssetManager.traits.add(EmperorSeal);
            
            // 添加戮王印特质
            ActorTrait SlaughterKingSeal = CreateTrait("SlaughterKingSeal", "trait/SlaughterKingSeal", "EmperorSeal");
            SlaughterKingSeal.rarity = Rarity.R3_Legendary;
            SafeSetStat(SlaughterKingSeal.base_stats, strings.S.multiplier_health, 8f);  // 8倍生命值（帝印的五分之四）
            SafeSetStat(SlaughterKingSeal.base_stats, strings.S.multiplier_damage, 8f);  // 8倍伤害（帝印的五分之四）
            SafeSetStat(SlaughterKingSeal.base_stats, strings.S.multiplier_speed, 4f);   // 4倍速度（帝印的五分之四）
            SafeSetStat(SlaughterKingSeal.base_stats, strings.S.lifespan, 4000f);        // 增加4000年寿命（帝印的五分之四）
            SlaughterKingSeal.action_attack_target += traitAction.LuWangYinTrueDamage_AttackAction;  // 真伤是生命值的五倍
            AssetManager.traits.add(SlaughterKingSeal);

            ActorTrait SpiritualPlant01 = CreateTrait("SpiritualPlant1+", "trait/SpiritualPlant1+", "SpiritualPlants");
            SpiritualPlant01.rarity = Rarity.R1_Rare;
            SpiritualPlant01.action_special_effect += new WorldAction(traitAction.SpiritualPlant01_Regen);
            AssetManager.traits.add(SpiritualPlant01);

            ActorTrait SpiritualPlant2 = CreateTrait("SpiritualPlant2", "trait/SpiritualPlant2", "SpiritualPlants");
            SpiritualPlant2.rarity = Rarity.R1_Rare;
            SpiritualPlant2.action_special_effect += new WorldAction(traitAction.SpiritualPlant2_Regen);
            AssetManager.traits.add(SpiritualPlant2);

            ActorTrait SpiritualPlant3 = CreateTrait("SpiritualPlant3", "trait/SpiritualPlant3", "SpiritualPlants");
            SpiritualPlant3.rarity = Rarity.R1_Rare;
            SpiritualPlant3.action_special_effect += new WorldAction(traitAction.SpiritualPlant3_Regen);
            AssetManager.traits.add(SpiritualPlant3);

            ActorTrait SpiritualPlant4 = CreateTrait("SpiritualPlant4", "trait/SpiritualPlant4", "SpiritualPlants");
            SpiritualPlant4.rarity = Rarity.R1_Rare;
            SpiritualPlant4.action_special_effect += new WorldAction(traitAction.SpiritualPlant4_Regen);
            AssetManager.traits.add(SpiritualPlant4);

            ActorTrait SpiritualPlant5 = CreateTrait("SpiritualPlant5", "trait/SpiritualPlant5", "SpiritualPlants");
            SpiritualPlant5.rarity = Rarity.R1_Rare;
            SpiritualPlant5.action_special_effect += new WorldAction(traitAction.SpiritualPlant5_Regen);
            AssetManager.traits.add(SpiritualPlant5);

            ActorTrait SpiritualPlant6 = CreateTrait("SpiritualPlant6", "trait/SpiritualPlant6", "SpiritualPlants");
            SpiritualPlant6.rarity = Rarity.R1_Rare;
            SpiritualPlant6.action_special_effect += new WorldAction(traitAction.SpiritualPlant6_Regen);
            AssetManager.traits.add(SpiritualPlant6);

            ActorTrait SpiritualPlant7 = CreateTrait("SpiritualPlant7", "trait/SpiritualPlant7", "SpiritualPlants");
            SpiritualPlant7.rarity = Rarity.R2_Epic;
            SpiritualPlant7.action_special_effect += new WorldAction(traitAction.SpiritualPlant7_Regen);
            AssetManager.traits.add(SpiritualPlant7);

            ActorTrait SpiritualPlant8 = CreateTrait("SpiritualPlant8", "trait/SpiritualPlant8", "SpiritualPlants");
            SpiritualPlant8.rarity = Rarity.R2_Epic;
            SpiritualPlant8.action_special_effect += new WorldAction(traitAction.SpiritualPlant8_Regen);
            AssetManager.traits.add(SpiritualPlant8);

            ActorTrait SpiritualPlant9 = CreateTrait("SpiritualPlant9", "trait/SpiritualPlant9", "SpiritualPlants");
            SpiritualPlant9.rarity = Rarity.R2_Epic;
            SpiritualPlant9.action_special_effect += new WorldAction(traitAction.SpiritualPlant9_Regen);
            AssetManager.traits.add(SpiritualPlant9);

            ActorTrait SpiritualPlant91 = CreateTrait("SpiritualPlant91", "trait/SpiritualPlant91", "SpiritualPlants");
            SpiritualPlant91.rarity = Rarity.R2_Epic;
            SpiritualPlant91.action_special_effect += new WorldAction(traitAction.SpiritualPlant91_Regen);
            AssetManager.traits.add(SpiritualPlant91);

            ActorTrait SpiritualPlant92 = CreateTrait("SpiritualPlant92", "trait/SpiritualPlant92", "SpiritualPlants");
            SpiritualPlant92.rarity = Rarity.R2_Epic;
            SpiritualPlant92.action_special_effect += new WorldAction(traitAction.SpiritualPlant92_Regen);
            AssetManager.traits.add(SpiritualPlant92);

            ActorTrait SpiritualPlant93 = CreateTrait("SpiritualPlant93", "trait/SpiritualPlant93", "SpiritualPlants");
            SpiritualPlant93.rarity = Rarity.R2_Epic;
            SpiritualPlant93.action_special_effect += new WorldAction(traitAction.SpiritualPlant93_Regen);
            AssetManager.traits.add(SpiritualPlant93);

            ActorTrait SpiritualPlant94 = CreateTrait("SpiritualPlant94", "trait/SpiritualPlant94", "SpiritualPlants");
            SpiritualPlant94.rarity = Rarity.R3_Legendary;
            SpiritualPlant94.action_special_effect += new WorldAction(traitAction.SpiritualPlant94_Regen);
            AssetManager.traits.add(SpiritualPlant94);

            ActorTrait SpiritualPlant95 = CreateTrait("SpiritualPlant95", "trait/SpiritualPlant95", "SpiritualPlants");
            SpiritualPlant95.rarity = Rarity.R3_Legendary;
            SpiritualPlant95.action_special_effect += new WorldAction(traitAction.SpiritualPlant95_Regen);
            AssetManager.traits.add(SpiritualPlant95);

            ActorTrait SpiritualPlant96 = CreateTrait("SpiritualPlant96", "trait/SpiritualPlant96", "SpiritualPlants");
            SpiritualPlant96.rarity = Rarity.R3_Legendary;
            SpiritualPlant96.action_special_effect += new WorldAction(traitAction.SpiritualPlant96_Regen);
            AssetManager.traits.add(SpiritualPlant96);

            ActorTrait SpiritualPlant97 = CreateTrait("SpiritualPlant97", "trait/SpiritualPlant97", "SpiritualPlants");
            SpiritualPlant97.rarity = Rarity.R3_Legendary;
            SpiritualPlant97.action_special_effect += new WorldAction(traitAction.SpiritualPlant97_Regen);
            AssetManager.traits.add(SpiritualPlant97);

            ActorTrait SpiritualPlant98 = CreateTrait("SpiritualPlant98", "trait/SpiritualPlant98", "SpiritualPlants");
            SpiritualPlant98.rarity = Rarity.R3_Legendary;
            SpiritualPlant98.action_special_effect += new WorldAction(traitAction.SpiritualPlant98_Regen);
            AssetManager.traits.add(SpiritualPlant98);

            // 新增真罡灵植特质定义 - 低阶真罡灵植
            ActorTrait TrueGangPlant1 = CreateTrait("TrueGangPlant1", "trait/TrueGangPlant1", "SpiritualPlants");
            TrueGangPlant1.rarity = Rarity.R1_Rare;
            TrueGangPlant1.action_special_effect += new WorldAction(traitAction.TrueGangPlant1_Regen);
            AssetManager.traits.add(TrueGangPlant1);

            ActorTrait TrueGangPlant2 = CreateTrait("TrueGangPlant2", "trait/TrueGangPlant2", "SpiritualPlants");
            TrueGangPlant2.rarity = Rarity.R1_Rare;
            TrueGangPlant2.action_special_effect += new WorldAction(traitAction.TrueGangPlant2_Regen);
            AssetManager.traits.add(TrueGangPlant2);

            ActorTrait TrueGangPlant3 = CreateTrait("TrueGangPlant3", "trait/TrueGangPlant3", "SpiritualPlants");
            TrueGangPlant3.rarity = Rarity.R1_Rare;
            TrueGangPlant3.action_special_effect += new WorldAction(traitAction.TrueGangPlant3_Regen);
            AssetManager.traits.add(TrueGangPlant3);

            ActorTrait TrueGangPlant4 = CreateTrait("TrueGangPlant4", "trait/TrueGangPlant4", "SpiritualPlants");
            TrueGangPlant4.rarity = Rarity.R1_Rare;
            TrueGangPlant4.action_special_effect += new WorldAction(traitAction.TrueGangPlant4_Regen);
            AssetManager.traits.add(TrueGangPlant4);

            // 中阶真罡灵植
            ActorTrait TrueGangPlant5 = CreateTrait("TrueGangPlant5", "trait/TrueGangPlant5", "SpiritualPlants");
            TrueGangPlant5.rarity = Rarity.R2_Epic;
            TrueGangPlant5.action_special_effect += new WorldAction(traitAction.TrueGangPlant5_Regen);
            AssetManager.traits.add(TrueGangPlant5);

            ActorTrait TrueGangPlant6 = CreateTrait("TrueGangPlant6", "trait/TrueGangPlant6", "SpiritualPlants");
            TrueGangPlant6.rarity = Rarity.R2_Epic;
            TrueGangPlant6.action_special_effect += new WorldAction(traitAction.TrueGangPlant6_Regen);
            AssetManager.traits.add(TrueGangPlant6);

            // 高阶真罡灵植
            ActorTrait TrueGangPlant7 = CreateTrait("TrueGangPlant7", "trait/TrueGangPlant7", "SpiritualPlants");
            TrueGangPlant7.rarity = Rarity.R3_Legendary;
            TrueGangPlant7.action_special_effect += new WorldAction(traitAction.TrueGangPlant7_Regen);
            AssetManager.traits.add(TrueGangPlant7);

            ActorTrait TrueGangPlant8 = CreateTrait("TrueGangPlant8", "trait/TrueGangPlant8", "SpiritualPlants");
            TrueGangPlant8.rarity = Rarity.R3_Legendary;
            TrueGangPlant8.action_special_effect += new WorldAction(traitAction.TrueGangPlant8_Regen);
            AssetManager.traits.add(TrueGangPlant8);

            // 传说级真罡灵植
            ActorTrait TrueGangPlant9 = CreateTrait("TrueGangPlant9", "trait/TrueGangPlant9", "SpiritualPlants");
            TrueGangPlant9.rarity = Rarity.R3_Legendary;
            TrueGangPlant9.action_special_effect += new WorldAction(traitAction.TrueGangPlant9_Regen);
            AssetManager.traits.add(TrueGangPlant9);

            ActorTrait SpiritualPlant99 = CreateTrait("SpiritualPlant99", "trait/SpiritualPlant99", "SpiritualPlants");
            SpiritualPlant99.rarity = Rarity.R3_Legendary;
            SpiritualPlant99.action_special_effect += new WorldAction(traitAction.SpiritualPlant99_Regen);
            AssetManager.traits.add(SpiritualPlant99);

            ActorTrait SpiritualPlant1 = CreateTrait("SpiritualPlant1", "trait/SpiritualPlant1", "SpiritualPlants");
            SpiritualPlant1.rarity = Rarity.R3_Legendary;
            SpiritualPlant1.action_special_effect += new WorldAction(traitAction.SpiritualPlant1_Regen);
            AssetManager.traits.add(SpiritualPlant1);
        }

        private static void SafeSetStat(BaseStats baseStats, string statKey, float value)
        {
            baseStats[statKey]= value;
        }
    }
}