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

namespace Chevalier.code
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
        public static ActorTrait GermofLife_AddActorTrait(string id, string pathIcon)
        {
            ActorTrait GermofLife = new ActorTrait
            {
                id = id,
                path_icon = pathIcon,
                group_id = "ChivalricFoundations",
                needs_to_be_explored = false
            };
            GermofLife.action_special_effect += traitAction.Chevalier1_effectAction;
            AssetManager.traits.add(GermofLife);
            return GermofLife;
        }
        public static void Init()
        {
            // 添加悟性特质
            // 下等悟性：不影响境界突破成功率
            ActorTrait Comprehensiontrait1 = CreateTrait("Comprehensiontrait1", "trait/Comprehensiontrait1", "ChivalricFoundations");
            AssetManager.traits.add(Comprehensiontrait1);

            // 中等悟性：突破成功率增加一倍（乘以2）
            ActorTrait Comprehensiontrait2 = CreateTrait("Comprehensiontrait2", "trait/Comprehensiontrait2", "ChivalricFoundations");
            Comprehensiontrait2.rarity = Rarity.R2_Epic;
            AssetManager.traits.add(Comprehensiontrait2);

            // 上等悟性：突破成功率增加两倍（乘以3）
            ActorTrait Comprehensiontrait3 = CreateTrait("Comprehensiontrait3", "trait/Comprehensiontrait3", "ChivalricFoundations");
            Comprehensiontrait3.rarity = Rarity.R3_Legendary;
            AssetManager.traits.add(Comprehensiontrait3);

            // 超绝悟性：突破成功率增加三倍（乘以4）
            ActorTrait Comprehensiontrait4 = CreateTrait("Comprehensiontrait4", "trait/Comprehensiontrait4", "ChivalricFoundations");
            Comprehensiontrait4.rarity = Rarity.R3_Legendary;
            AssetManager.traits.add(Comprehensiontrait4);

            ActorTrait GermofLife4 = GermofLife_AddActorTrait("GermofLife4", "trait/GermofLife4");
            GermofLife4.rarity = Rarity.R2_Epic;

            ActorTrait GermofLife7 = GermofLife_AddActorTrait("GermofLife7", "trait/GermofLife7");
            GermofLife7.rarity = Rarity.R2_Epic;
        
            ActorTrait GermofLife8 = GermofLife_AddActorTrait("GermofLife8", "trait/GermofLife8");
            GermofLife8.rarity = Rarity.R3_Legendary;

            _=GermofLife_AddActorTrait("GermofLife1", "trait/GermofLife1");
            _=GermofLife_AddActorTrait("GermofLife2", "trait/GermofLife2");
            _=GermofLife_AddActorTrait("GermofLife3", "trait/GermofLife3");

            ActorTrait GermofLife9 = CreateTrait("GermofLife9", "trait/GermofLife9", "ChivalricFoundations");
            GermofLife9.rate_inherit = 100;
            AssetManager.traits.add(GermofLife9);

            ActorTrait GermofLife10 = CreateTrait("GermofLife10", "trait/GermofLife10", "ChivalricFoundations");
            AssetManager.traits.add(GermofLife10);

            ActorTrait GodlySigil1 = CreateTrait("GodlySigil1", "trait/GodlySigil1", "GodlySigil");
            GodlySigil1.rarity = Rarity.R3_Legendary;
            SafeSetStat(GodlySigil1.base_stats, strings.S.speed, 50f);
            SafeSetStat(GodlySigil1.base_stats, strings.S.armor, 50f);
            SafeSetStat(GodlySigil1.base_stats, strings.S.health, 5000f);
            SafeSetStat(GodlySigil1.base_stats, strings.S.damage, 5000f); 
            SafeSetStat(GodlySigil1.base_stats, strings.S.lifespan, 1000f);
            SafeSetStat(GodlySigil1.base_stats, strings.S.stamina, 1000f);
            SafeSetStat(GodlySigil1.base_stats, strings.S.attack_speed, 200f);
            SafeSetStat(GodlySigil1.base_stats, strings.S.multiplier_health, 6f);
            SafeSetStat(GodlySigil1.base_stats, strings.S.multiplier_damage, 6f);
            SafeSetStat(GodlySigil1.base_stats, strings.S.multiplier_speed, 6f);
            GodlySigil1.action_attack_target += traitAction.ZhanZhengTrueDamage_AttackAction;
            AssetManager.traits.add(GodlySigil1);

            ActorTrait GodlySigil2 = CreateTrait("GodlySigil2", "trait/GodlySigil2", "GodlySigil");
            GodlySigil2.rarity = Rarity.R2_Epic;
            SafeSetStat(GodlySigil2.base_stats, strings.S.speed, 10f);
            SafeSetStat(GodlySigil2.base_stats, strings.S.armor, 10f);
            SafeSetStat(GodlySigil2.base_stats, strings.S.health, 500f);
            SafeSetStat(GodlySigil2.base_stats, strings.S.damage, 600f); 
            SafeSetStat(GodlySigil2.base_stats, strings.S.lifespan, 20f);
            SafeSetStat(GodlySigil2.base_stats, strings.S.stamina, 200f);
            SafeSetStat(GodlySigil2.base_stats, strings.S.attack_speed, 20f);
            SafeSetStat(GodlySigil2.base_stats, strings.S.multiplier_health, 0.5f);
            SafeSetStat(GodlySigil2.base_stats, strings.S.multiplier_damage, 0.25f);
            SafeSetStat(GodlySigil2.base_stats, strings.S.multiplier_speed, 0.15f);
            GodlySigil2.action_attack_target += traitAction.HaiYangTrueDamage_AttackAction;
            AssetManager.traits.add(GodlySigil2);

            ActorTrait GodlySigil3 = CreateTrait("GodlySigil3", "trait/GodlySigil3", "GodlySigil");
            GodlySigil3.rarity = Rarity.R2_Epic;
            SafeSetStat(GodlySigil3.base_stats, strings.S.speed, 20f);
            SafeSetStat(GodlySigil3.base_stats, strings.S.armor, 15f);
            SafeSetStat(GodlySigil3.base_stats, strings.S.health, 800f);
            SafeSetStat(GodlySigil3.base_stats, strings.S.damage, 800f); 
            SafeSetStat(GodlySigil3.base_stats, strings.S.lifespan, 50f);
            SafeSetStat(GodlySigil3.base_stats, strings.S.stamina, 100f);
            SafeSetStat(GodlySigil3.base_stats, strings.S.attack_speed, 10f);
            SafeSetStat(GodlySigil3.base_stats, strings.S.multiplier_health, 0.15f);
            SafeSetStat(GodlySigil3.base_stats, strings.S.multiplier_damage, 0.25f);
            SafeSetStat(GodlySigil3.base_stats, strings.S.multiplier_speed, 0.45f);
            GodlySigil3.action_attack_target += traitAction.MingYunTrueDamage_AttackAction;
            AssetManager.traits.add(GodlySigil3);

            ActorTrait GodlySigil4 = CreateTrait("GodlySigil4", "trait/GodlySigil4", "GodlySigil");
            GodlySigil4.rarity = Rarity.R3_Legendary;
            SafeSetStat(GodlySigil4.base_stats, strings.S.speed, 40f);
            SafeSetStat(GodlySigil4.base_stats, strings.S.armor, 40f);
            SafeSetStat(GodlySigil4.base_stats, strings.S.health, 4000f);
            SafeSetStat(GodlySigil4.base_stats, strings.S.damage, 6000f); 
            SafeSetStat(GodlySigil4.base_stats, strings.S.lifespan, 500f);
            SafeSetStat(GodlySigil4.base_stats, strings.S.stamina,500f);
            SafeSetStat(GodlySigil4.base_stats, strings.S.attack_speed, 100f);
            SafeSetStat(GodlySigil4.base_stats, strings.S.multiplier_health, 5f);
            SafeSetStat(GodlySigil4.base_stats, strings.S.multiplier_damage, 4f);
            SafeSetStat(GodlySigil4.base_stats, strings.S.multiplier_speed, 2f);
            GodlySigil4.action_attack_target += traitAction.FengShouTrueDamage_AttackAction;
            AssetManager.traits.add(GodlySigil4);

            ActorTrait GodlySigil5 = CreateTrait("GodlySigil5", "trait/GodlySigil5", "GodlySigil");
            GodlySigil5.rarity = Rarity.R2_Epic;
            SafeSetStat(GodlySigil5.base_stats, strings.S.speed, 20f);
            SafeSetStat(GodlySigil5.base_stats, strings.S.armor, 30f);
            SafeSetStat(GodlySigil5.base_stats, strings.S.health, 1000f);
            SafeSetStat(GodlySigil5.base_stats, strings.S.damage, 1000f); 
            SafeSetStat(GodlySigil5.base_stats, strings.S.lifespan, 120f);
            SafeSetStat(GodlySigil5.base_stats, strings.S.stamina, 10f);
            SafeSetStat(GodlySigil5.base_stats, strings.S.attack_speed, 10f);
            SafeSetStat(GodlySigil5.base_stats, strings.S.multiplier_health, 0.2f);
            SafeSetStat(GodlySigil5.base_stats, strings.S.multiplier_damage, 0.3f);
            SafeSetStat(GodlySigil5.base_stats, strings.S.multiplier_speed, 0.56f);
            GodlySigil5.action_attack_target += traitAction.LeiTingTrueDamage_AttackAction;
            AssetManager.traits.add(GodlySigil5);

            ActorTrait GodlySigil6 = CreateTrait("GodlySigil6", "trait/GodlySigil6", "GodlySigil");
            GodlySigil6.rarity = Rarity.R2_Epic;
            SafeSetStat(GodlySigil6.base_stats, strings.S.speed, 10f);
            SafeSetStat(GodlySigil6.base_stats, strings.S.armor, 10f);
            SafeSetStat(GodlySigil6.base_stats, strings.S.health, 600f);
            SafeSetStat(GodlySigil6.base_stats, strings.S.damage, 600f); 
            SafeSetStat(GodlySigil6.base_stats, strings.S.lifespan, 80f);
            SafeSetStat(GodlySigil6.base_stats, strings.S.stamina, 100f);
            SafeSetStat(GodlySigil6.base_stats, strings.S.attack_speed, 20f);
            SafeSetStat(GodlySigil6.base_stats, strings.S.multiplier_health, 0.15f);
            SafeSetStat(GodlySigil6.base_stats, strings.S.multiplier_damage, 0.6f);
            SafeSetStat(GodlySigil6.base_stats, strings.S.multiplier_speed, 0.3f);
            GodlySigil6.action_attack_target += traitAction.CaiJueTrueDamage_AttackAction;
            AssetManager.traits.add(GodlySigil6);

            ActorTrait GodlySigil7 = CreateTrait("GodlySigil7", "trait/GodlySigil7", "GodlySigil");
            GodlySigil7.rarity = Rarity.R2_Epic;
            SafeSetStat(GodlySigil7.base_stats, strings.S.speed, 20f);
            SafeSetStat(GodlySigil7.base_stats, strings.S.armor, 30f);
            SafeSetStat(GodlySigil7.base_stats, strings.S.health, 500f);
            SafeSetStat(GodlySigil7.base_stats, strings.S.damage, 2000f); 
            SafeSetStat(GodlySigil7.base_stats, strings.S.lifespan, 90f);
            SafeSetStat(GodlySigil7.base_stats, strings.S.stamina, 300f);
            SafeSetStat(GodlySigil7.base_stats, strings.S.attack_speed, 10f);
            SafeSetStat(GodlySigil7.base_stats, strings.S.multiplier_health, 0.5f);
            SafeSetStat(GodlySigil7.base_stats, strings.S.multiplier_damage, 0.3f);
            SafeSetStat(GodlySigil7.base_stats, strings.S.multiplier_speed, 0.3f);
            GodlySigil7.action_attack_target += traitAction.SiWangTrueDamage_AttackAction;
            AssetManager.traits.add(GodlySigil7);

            ActorTrait GodlySigil8 = CreateTrait("GodlySigil8", "trait/GodlySigil8", "GodlySigil");
            GodlySigil8.rarity = Rarity.R2_Epic;
            SafeSetStat(GodlySigil8.base_stats, strings.S.speed, 40f);
            SafeSetStat(GodlySigil8.base_stats, strings.S.armor, 10f);
            SafeSetStat(GodlySigil8.base_stats, strings.S.health, 1000f);
            SafeSetStat(GodlySigil8.base_stats, strings.S.damage, 800f); 
            SafeSetStat(GodlySigil8.base_stats, strings.S.lifespan, 60f);
            SafeSetStat(GodlySigil8.base_stats, strings.S.stamina, 240f);
            SafeSetStat(GodlySigil8.base_stats, strings.S.attack_speed, 80f);
            SafeSetStat(GodlySigil8.base_stats, strings.S.multiplier_health, 0.4f);
            SafeSetStat(GodlySigil8.base_stats, strings.S.multiplier_damage, 0.36f);
            SafeSetStat(GodlySigil8.base_stats, strings.S.multiplier_speed, 0.12f);
            GodlySigil8.action_attack_target += traitAction.ShiJianTrueDamage_AttackAction;
            AssetManager.traits.add(GodlySigil8);

            ActorTrait GodlySigil9 = CreateTrait("GodlySigil9", "trait/GodlySigil9", "GodlySigil");
            GodlySigil9.rarity = Rarity.R3_Legendary;
            SafeSetStat(GodlySigil9.base_stats, strings.S.speed, 50f);
            SafeSetStat(GodlySigil9.base_stats, strings.S.armor, 40f);
            SafeSetStat(GodlySigil9.base_stats, strings.S.health, 3600f);
            SafeSetStat(GodlySigil9.base_stats, strings.S.damage, 4800f); 
            SafeSetStat(GodlySigil9.base_stats, strings.S.lifespan, 800f);
            SafeSetStat(GodlySigil9.base_stats, strings.S.stamina, 600f);
            SafeSetStat(GodlySigil9.base_stats, strings.S.attack_speed, 120f);
            SafeSetStat(GodlySigil9.base_stats, strings.S.multiplier_health, 4f);
            SafeSetStat(GodlySigil9.base_stats, strings.S.multiplier_damage, 5f);
            SafeSetStat(GodlySigil9.base_stats, strings.S.multiplier_speed, 3f);
            GodlySigil9.action_attack_target += traitAction.HanDongTrueDamage_AttackAction;
            AssetManager.traits.add(GodlySigil9);

            ActorTrait GodlySigil91 = CreateTrait("GodlySigil91", "trait/GodlySigil91", "GodlySigil");
            GodlySigil91.rarity = Rarity.R3_Legendary;
            SafeSetStat(GodlySigil91.base_stats, strings.S.speed, 45f);
            SafeSetStat(GodlySigil91.base_stats, strings.S.armor, 45f);
            SafeSetStat(GodlySigil91.base_stats, strings.S.health, 4500f);
            SafeSetStat(GodlySigil91.base_stats, strings.S.damage, 4500f); 
            SafeSetStat(GodlySigil91.base_stats, strings.S.lifespan, 900f);
            SafeSetStat(GodlySigil91.base_stats, strings.S.stamina, 900f);
            SafeSetStat(GodlySigil91.base_stats, strings.S.attack_speed, 120f);
            SafeSetStat(GodlySigil91.base_stats, strings.S.multiplier_health, 5f);
            SafeSetStat(GodlySigil91.base_stats, strings.S.multiplier_damage, 8f);
            SafeSetStat(GodlySigil91.base_stats, strings.S.multiplier_speed, 5f);
            GodlySigil91.action_attack_target += traitAction.fire2_attackAction;
            GodlySigil91.action_attack_target += traitAction.AnYeTrueDamage_AttackAction;
            AssetManager.traits.add(GodlySigil91);

            ActorTrait GodlySigil92 = CreateTrait("GodlySigil92", "trait/GodlySigil92", "GodlySigil");
            GodlySigil92.rarity = Rarity.R2_Epic;
            SafeSetStat(GodlySigil92.base_stats, strings.S.speed, 9f);
            SafeSetStat(GodlySigil92.base_stats, strings.S.armor, 10f);
            SafeSetStat(GodlySigil92.base_stats, strings.S.health, 550f);
            SafeSetStat(GodlySigil92.base_stats, strings.S.damage, 650f); 
            SafeSetStat(GodlySigil92.base_stats, strings.S.lifespan, 22f);
            SafeSetStat(GodlySigil92.base_stats, strings.S.stamina, 300f);
            SafeSetStat(GodlySigil92.base_stats, strings.S.attack_speed, 24f);
            SafeSetStat(GodlySigil92.base_stats, strings.S.multiplier_health, 0.4f);
            SafeSetStat(GodlySigil92.base_stats, strings.S.multiplier_damage, 0.35f);
            SafeSetStat(GodlySigil92.base_stats, strings.S.multiplier_speed, 0.15f);
            GodlySigil92.action_attack_target += traitAction.ZhiHuiTrueDamage_AttackAction;
            AssetManager.traits.add(GodlySigil92);

            ActorTrait GodlySigil93 = CreateTrait("GodlySigil93", "trait/GodlySigil93", "GodlySigil");
            GodlySigil93.rarity = Rarity.R2_Epic;
            SafeSetStat(GodlySigil93.base_stats, strings.S.speed, 25f);
            SafeSetStat(GodlySigil93.base_stats, strings.S.armor, 20f);
            SafeSetStat(GodlySigil93.base_stats, strings.S.health, 600f);
            SafeSetStat(GodlySigil93.base_stats, strings.S.damage, 700f); 
            SafeSetStat(GodlySigil93.base_stats, strings.S.lifespan, 400f);
            SafeSetStat(GodlySigil93.base_stats, strings.S.stamina, 120f);
            SafeSetStat(GodlySigil93.base_stats, strings.S.attack_speed, 12f);
            SafeSetStat(GodlySigil93.base_stats, strings.S.multiplier_health, 0.16f);
            SafeSetStat(GodlySigil93.base_stats, strings.S.multiplier_damage, 0.24f);
            SafeSetStat(GodlySigil93.base_stats, strings.S.multiplier_speed, 0.55f);
            GodlySigil93.action_attack_target += traitAction.GuangTrueDamage_AttackAction;
            AssetManager.traits.add(GodlySigil93);

            ActorTrait GodlySigil94 = CreateTrait("GodlySigil94", "trait/GodlySigil94", "GodlySigil");
            GodlySigil94.rarity = Rarity.R2_Epic;
            SafeSetStat(GodlySigil94.base_stats, strings.S.speed, 20f);
            SafeSetStat(GodlySigil94.base_stats, strings.S.armor, 20f);
            SafeSetStat(GodlySigil94.base_stats, strings.S.health, 400f);
            SafeSetStat(GodlySigil94.base_stats, strings.S.damage, 600f); 
            SafeSetStat(GodlySigil94.base_stats, strings.S.lifespan, 50f);
            SafeSetStat(GodlySigil94.base_stats, strings.S.stamina,50f);
            SafeSetStat(GodlySigil94.base_stats, strings.S.attack_speed, 10f);
            SafeSetStat(GodlySigil94.base_stats, strings.S.multiplier_health, 0.3f);
            SafeSetStat(GodlySigil94.base_stats, strings.S.multiplier_damage, 0.4f);
            SafeSetStat(GodlySigil94.base_stats, strings.S.multiplier_speed, 0.2f);
            GodlySigil94.action_attack_target += traitAction.ZiYouTrueDamage_AttackAction;
            AssetManager.traits.add(GodlySigil94);

            ActorTrait GodlySigil95 = CreateTrait("GodlySigil95", "trait/GodlySigil95", "GodlySigil");
            GodlySigil95.rarity = Rarity.R2_Epic;
            SafeSetStat(GodlySigil95.base_stats, strings.S.speed, 30f);
            SafeSetStat(GodlySigil95.base_stats, strings.S.armor, 2f);
            SafeSetStat(GodlySigil95.base_stats, strings.S.health, 2000f);
            SafeSetStat(GodlySigil95.base_stats, strings.S.damage, 400f); 
            SafeSetStat(GodlySigil95.base_stats, strings.S.lifespan, 120f);
            SafeSetStat(GodlySigil95.base_stats, strings.S.stamina, 120f);
            SafeSetStat(GodlySigil95.base_stats, strings.S.attack_speed, 21f);
            SafeSetStat(GodlySigil95.base_stats, strings.S.multiplier_health, 0.8f);
            SafeSetStat(GodlySigil95.base_stats, strings.S.multiplier_damage, 0.45f);
            SafeSetStat(GodlySigil95.base_stats, strings.S.multiplier_speed, 0.48f);
            GodlySigil95.action_attack_target += traitAction.YinMouDamage_AttackAction;
            AssetManager.traits.add(GodlySigil95);

            ActorTrait GodlySigil96 = CreateTrait("GodlySigil96", "trait/GodlySigil96", "GodlySigil");
            GodlySigil96.rarity = Rarity.R3_Legendary;
            SafeSetStat(GodlySigil96.base_stats, strings.S.speed, 30f);
            SafeSetStat(GodlySigil96.base_stats, strings.S.armor, 30f);
            SafeSetStat(GodlySigil96.base_stats, strings.S.health, 6000f);
            SafeSetStat(GodlySigil96.base_stats, strings.S.damage, 1000f); 
            SafeSetStat(GodlySigil96.base_stats, strings.S.lifespan, 300f);
            SafeSetStat(GodlySigil96.base_stats, strings.S.stamina, 300f);
            SafeSetStat(GodlySigil96.base_stats, strings.S.attack_speed, 50f);
            SafeSetStat(GodlySigil96.base_stats, strings.S.multiplier_health, 4.5f);
            SafeSetStat(GodlySigil96.base_stats, strings.S.multiplier_damage, 3.6f);
            SafeSetStat(GodlySigil96.base_stats, strings.S.multiplier_speed, 2.3f);
            GodlySigil96.action_attack_target += traitAction.GongJiangTrueDamage_AttackAction;
            AssetManager.traits.add(GodlySigil96);

            ActorTrait GodlySigil97 = CreateTrait("GodlySigil97", "trait/GodlySigil97", "GodlySigil");
            GodlySigil97.rarity = Rarity.R2_Epic;
            SafeSetStat(GodlySigil97.base_stats, strings.S.speed, 25f);
            SafeSetStat(GodlySigil97.base_stats, strings.S.armor, 25f);
            SafeSetStat(GodlySigil97.base_stats, strings.S.health, 600f);
            SafeSetStat(GodlySigil97.base_stats, strings.S.damage, 1000f); 
            SafeSetStat(GodlySigil97.base_stats, strings.S.lifespan, 60f);
            SafeSetStat(GodlySigil97.base_stats, strings.S.stamina, 240f);
            SafeSetStat(GodlySigil97.base_stats, strings.S.attack_speed, 20f);
            SafeSetStat(GodlySigil97.base_stats, strings.S.multiplier_health, 0.55f);
            SafeSetStat(GodlySigil97.base_stats, strings.S.multiplier_damage, 0.25f);
            SafeSetStat(GodlySigil97.base_stats, strings.S.multiplier_speed, 0.35f);
            GodlySigil97.action_attack_target += traitAction.ShengMingDamage_AttackAction;
            AssetManager.traits.add(GodlySigil97);

            ActorTrait GodlySigil98 = CreateTrait("GodlySigil98", "trait/GodlySigil98", "GodlySigil");
            GodlySigil98.rarity = Rarity.R2_Epic;
            SafeSetStat(GodlySigil98.base_stats, strings.S.speed, 48f);
            SafeSetStat(GodlySigil98.base_stats, strings.S.armor, 12f);
            SafeSetStat(GodlySigil98.base_stats, strings.S.health, 1600f);
            SafeSetStat(GodlySigil98.base_stats, strings.S.damage, 500f); 
            SafeSetStat(GodlySigil98.base_stats, strings.S.lifespan, 90f);
            SafeSetStat(GodlySigil98.base_stats, strings.S.stamina, 600f);
            SafeSetStat(GodlySigil98.base_stats, strings.S.attack_speed, 80f);
            SafeSetStat(GodlySigil98.base_stats, strings.S.multiplier_health, 0.5f);
            SafeSetStat(GodlySigil98.base_stats, strings.S.multiplier_damage, 0.56f);
            SafeSetStat(GodlySigil98.base_stats, strings.S.multiplier_speed, 0.32f);
            GodlySigil98.action_attack_target += traitAction.LongShenTrueDamage_AttackAction;
            AssetManager.traits.add(GodlySigil98);

            ActorTrait GodlySigil99 = CreateTrait("GodlySigil99", "trait/GodlySigil99", "GodlySigil");
            GodlySigil99.rarity = Rarity.R2_Epic;
            SafeSetStat(GodlySigil99.base_stats, strings.S.speed, 60f);
            SafeSetStat(GodlySigil99.base_stats, strings.S.armor, 30f);
            SafeSetStat(GodlySigil99.base_stats, strings.S.health, 3600f);
            SafeSetStat(GodlySigil99.base_stats, strings.S.damage, 480f); 
            SafeSetStat(GodlySigil99.base_stats, strings.S.lifespan, 180f);
            SafeSetStat(GodlySigil99.base_stats, strings.S.stamina, 200f);
            SafeSetStat(GodlySigil99.base_stats, strings.S.attack_speed, 20f);
            SafeSetStat(GodlySigil99.base_stats, strings.S.multiplier_health, 0.4f);
            SafeSetStat(GodlySigil99.base_stats, strings.S.multiplier_damage, 0.5f);
            SafeSetStat(GodlySigil99.base_stats, strings.S.multiplier_speed, 0.3f);
            GodlySigil99.action_attack_target += traitAction.XiShengTrueDamage_AttackAction;
            AssetManager.traits.add(GodlySigil99);

            ActorTrait Chevalier02 = CreateTrait("Chevalier2+", "trait/Chevalier2+", "Chevalier");
            SafeSetStat(Chevalier02.base_stats, strings.S.lifespan, 10f);
            SafeSetStat(Chevalier02.base_stats, strings.S.skill_combat, 0.1f);
            SafeSetStat(Chevalier02.base_stats, strings.S.loyalty_traits, -1f);
            AssetManager.traits.add(Chevalier02);

            ActorTrait Chevalier03 = CreateTrait("Chevalier3+", "trait/Chevalier3+", "Chevalier");
            SafeSetStat(Chevalier03.base_stats, strings.S.lifespan, 20f);
            SafeSetStat(Chevalier03.base_stats, strings.S.skill_combat, 0.2f);
            SafeSetStat(Chevalier03.base_stats, strings.S.loyalty_traits, -5f);
            AssetManager.traits.add(Chevalier03);

            ActorTrait Chevalier04 = CreateTrait("Chevalier4+", "trait/Chevalier4+", "Chevalier");
            SafeSetStat(Chevalier04.base_stats , strings.S.lifespan, 30f);
            SafeSetStat(Chevalier04.base_stats , strings.S.skill_combat, 0.3f);
            SafeSetStat(Chevalier04.base_stats, strings.S.loyalty_traits, -10f);
            AssetManager.traits.add(Chevalier04);

            ActorTrait Chevalier05 = CreateTrait("Chevalier5+", "trait/Chevalier5+", "Chevalier");
            SafeSetStat(Chevalier05.base_stats , strings.S.lifespan, 50f);
            SafeSetStat(Chevalier05.base_stats , strings.S.skill_combat, 0.4f);
            SafeSetStat(Chevalier05.base_stats, strings.S.loyalty_traits, -20f);
            AssetManager.traits.add(Chevalier05);

            ActorTrait Chevalier06 = CreateTrait("Chevalier6+", "trait/Chevalier6+", "Chevalier");
            SafeSetStat(Chevalier06.base_stats , strings.S.lifespan, 80f);
            SafeSetStat(Chevalier06.base_stats , strings.S.skill_combat, 0.5f);
            SafeSetStat(Chevalier06.base_stats, strings.S.loyalty_traits, -30f);
            AssetManager.traits.add(Chevalier06);

            ActorTrait Chevalier07 = CreateTrait("Chevalier7+", "trait/Chevalier7+", "Chevalier");
            SafeSetStat(Chevalier07.base_stats, strings.S.lifespan, 100f);
            SafeSetStat(Chevalier07.base_stats, strings.S.skill_combat, 0.6f);
            SafeSetStat(Chevalier07.base_stats, strings.S.loyalty_traits, -40f);
            AssetManager.traits.add(Chevalier07);

            ActorTrait Chevalier08 = CreateTrait("Chevalier8+", "trait/Chevalier8+", "Chevalier");
            SafeSetStat(Chevalier08.base_stats, strings.S.lifespan, 150f);
            SafeSetStat(Chevalier08.base_stats, strings.S.skill_combat, 0.6f);
            SafeSetStat(Chevalier08.base_stats, strings.S.loyalty_traits, -60f);
            AssetManager.traits.add(Chevalier08);

            ActorTrait Chevalier09 = CreateTrait("Chevalier9+", "trait/Chevalier9+", "Chevalier");
            SafeSetStat(Chevalier09.base_stats, strings.S.lifespan, 240f);
            SafeSetStat(Chevalier09.base_stats, strings.S.skill_combat, 0.6f);
            SafeSetStat(Chevalier09.base_stats, strings.S.loyalty_traits, -80f);
            AssetManager.traits.add(Chevalier09);

            ActorTrait Chevalier091 = CreateTrait("Chevalier91+", "trait/Chevalier91+", "Chevalier");
            SafeSetStat(Chevalier091.base_stats, strings.S.lifespan, 400f);
            SafeSetStat(Chevalier091.base_stats, strings.S.skill_combat, 0.6f);
            SafeSetStat(Chevalier091.base_stats, strings.S.loyalty_traits, -120f);
            AssetManager.traits.add(Chevalier091);

            ActorTrait Chevalier092 = CreateTrait("Chevalier92+", "trait/Chevalier92+", "Chevalier");
            SafeSetStat(Chevalier092.base_stats, strings.S.lifespan, 800f);
            SafeSetStat(Chevalier092.base_stats, strings.S.skill_combat, 0.7f);
            SafeSetStat(Chevalier092.base_stats, strings.S.loyalty_traits, -300f);
            AssetManager.traits.add(Chevalier092);

            ActorTrait Chevalier093 = CreateTrait("Chevalier93+", "trait/Chevalier93+", "Chevalier");
            SafeSetStat(Chevalier093.base_stats, strings.S.lifespan, 100000f);
            SafeSetStat(Chevalier093.base_stats, strings.S.skill_combat, 0.8f);
            SafeSetStat(Chevalier093.base_stats, strings.S.loyalty_traits, -500f);
            AssetManager.traits.add(Chevalier093);

            ActorTrait Chevalier1 = CreateTrait("Chevalier1", "trait/Chevalier1", "Chevalier");
            SafeSetStat(Chevalier1.base_stats, stats.Resist.id, 0.5f);
            SafeSetStat(Chevalier1.base_stats, strings.S.damage, 60f);
            SafeSetStat(Chevalier1.base_stats, strings.S.mass, 5.0f);
            SafeSetStat(Chevalier1.base_stats, strings.S.health, 300f);
            SafeSetStat(Chevalier1.base_stats, strings.S.stamina, 10f);
            SafeSetStat(Chevalier1.base_stats, "Dodge", 120f);
            SafeSetStat(Chevalier1.base_stats, "Accuracy", 110f);
            Chevalier1.action_special_effect += traitAction.Chevalier2_effectAction;
            Chevalier1.action_special_effect += traitAction.Chevalier1_Regen;
            AssetManager.traits.add(Chevalier1);

            ActorTrait Chevalier2 = CreateTrait("Chevalier2", "trait/Chevalier2", "Chevalier");
            SafeSetStat(Chevalier2.base_stats, stats.Resist.id, 1.0f);
            SafeSetStat(Chevalier2.base_stats , strings.S.damage, 120f);
            SafeSetStat(Chevalier2.base_stats, strings.S.mass, 10f);
            SafeSetStat(Chevalier2.base_stats , strings.S.health, 500f);
            SafeSetStat(Chevalier2.base_stats , strings.S.accuracy, 2f);
            SafeSetStat(Chevalier2.base_stats , strings.S.multiplier_speed, 0.1f);
            SafeSetStat(Chevalier2.base_stats , strings.S.stamina, 20f);
            SafeSetStat(Chevalier2.base_stats, "Dodge", 130f);
            SafeSetStat(Chevalier2.base_stats, "Accuracy", 110f);
            Chevalier2.action_special_effect += traitAction.Chevalier3_effectAction;
            Chevalier2.action_special_effect += traitAction.Chevalier2_Regen;
            AssetManager.traits.add(Chevalier2);

            ActorTrait Chevalier3 = CreateTrait("Chevalier3", "trait/Chevalier3", "Chevalier");
            SafeSetStat(Chevalier3.base_stats, stats.Resist.id, 2.0f);
            SafeSetStat(Chevalier3.base_stats , strings.S.damage, 240f);
            SafeSetStat(Chevalier3.base_stats, strings.S.mass, 10f);
            SafeSetStat(Chevalier3.base_stats , strings.S.health, 1000f);
            SafeSetStat(Chevalier3.base_stats , strings.S.armor, 5f);
            SafeSetStat(Chevalier3.base_stats , strings.S.targets, 0.1f);
            SafeSetStat(Chevalier3.base_stats , strings.S.accuracy, 5f);
            SafeSetStat(Chevalier3.base_stats , strings.S.multiplier_speed, 0.2f);
            SafeSetStat(Chevalier3.base_stats , strings.S.stamina, 30f);
            SafeSetStat(Chevalier3.base_stats, "Dodge", 140f);
            SafeSetStat(Chevalier3.base_stats, "Accuracy", 110f);
            Chevalier3.action_special_effect += traitAction.Chevalier4_effectAction;
            Chevalier3.action_special_effect += traitAction.Chevalier3_Regen;
            AssetManager.traits.add(Chevalier3);

            ActorTrait Chevalier4 = CreateTrait("Chevalier4", "trait/Chevalier4", "Chevalier");
            SafeSetStat(Chevalier4.base_stats, stats.Resist.id, 4.0f);
            SafeSetStat(Chevalier4.base_stats , strings.S.damage, 300f);
            SafeSetStat(Chevalier4.base_stats, strings.S.mass, 10f);
            SafeSetStat(Chevalier4.base_stats , strings.S.health, 2000f);
            SafeSetStat(Chevalier4.base_stats , strings.S.speed, 5f);
            SafeSetStat(Chevalier4.base_stats , strings.S.armor, 10f);
            SafeSetStat(Chevalier4.base_stats , strings.S.targets, 1f);
            SafeSetStat(Chevalier4.base_stats , strings.S.critical_chance, 0.2f);
            SafeSetStat(Chevalier4.base_stats , strings.S.accuracy, 10f);
            SafeSetStat(Chevalier4.base_stats , strings.S.multiplier_speed, 0.3f);
            SafeSetStat(Chevalier4.base_stats , strings.S.stamina, 40f);
            SafeSetStat(Chevalier4.base_stats, "Dodge", 150f);
            SafeSetStat(Chevalier4.base_stats, "Accuracy", 110f);
            Chevalier4.action_special_effect += traitAction.Chevalier5_effectAction;
            Chevalier4.action_special_effect += traitAction.Chevalier4_Regen;
            AssetManager.traits.add(Chevalier4);

            ActorTrait Chevalier5 = CreateTrait("Chevalier5", "trait/Chevalier5", "Chevalier");
            SafeSetStat(Chevalier5.base_stats, stats.Resist.id, 8.0f);
            SafeSetStat(Chevalier5.base_stats , strings.S.warfare, 10f);
            SafeSetStat(Chevalier5.base_stats, strings.S.mass, 15f);
            SafeSetStat(Chevalier5.base_stats , strings.S.damage, 600f);
            SafeSetStat(Chevalier5.base_stats , strings.S.health, 4000f);
            SafeSetStat(Chevalier5.base_stats , strings.S.speed, 10f);
            SafeSetStat(Chevalier5.base_stats , strings.S.armor, 15f);
            SafeSetStat(Chevalier5.base_stats , strings.S.targets, 2f);
            SafeSetStat(Chevalier5.base_stats , strings.S.critical_chance, 0.3f);
            SafeSetStat(Chevalier5.base_stats , strings.S.accuracy, 20f);
            SafeSetStat(Chevalier5.base_stats , strings.S.multiplier_speed, 0.4f);
            SafeSetStat(Chevalier5.base_stats , strings.S.stamina, 50f);
            SafeSetStat(Chevalier5.base_stats, "Dodge", 180f);
            SafeSetStat(Chevalier5.base_stats, "Accuracy", 140f);
            Chevalier5.action_special_effect += traitAction.Chevalier6_effectAction;
            Chevalier5.action_special_effect += traitAction.Chevalier5_Regen;
            AssetManager.traits.add(Chevalier5);

            ActorTrait Chevalier6 = CreateTrait("Chevalier6", "trait/Chevalier6", "Chevalier");
            Chevalier6.rarity = Rarity.R2_Epic;
            SafeSetStat(Chevalier6.base_stats, stats.Resist.id, 16.0f);
            SafeSetStat(Chevalier6.base_stats , strings.S.warfare, 20f);
            SafeSetStat(Chevalier6.base_stats, strings.S.mass, 20f);
            SafeSetStat(Chevalier6.base_stats , strings.S.damage, 1200f);
            SafeSetStat(Chevalier6.base_stats , strings.S.armor, 25f);
            SafeSetStat(Chevalier6.base_stats , strings.S.health, 8000f);
            SafeSetStat(Chevalier6.base_stats , strings.S.speed, 15f);
            SafeSetStat(Chevalier6.base_stats , strings.S.area_of_effect, 10f);
            SafeSetStat(Chevalier6.base_stats , strings.S.targets, 5f);
            SafeSetStat(Chevalier6.base_stats , strings.S.critical_chance, 0.5f);
            SafeSetStat(Chevalier6.base_stats , strings.S.accuracy, 30f);
            SafeSetStat(Chevalier6.base_stats , strings.S.multiplier_speed, 0.6f);
            SafeSetStat(Chevalier6.base_stats , strings.S.stamina, 70f);
            SafeSetStat(Chevalier6.base_stats, "Dodge", 200f);
            SafeSetStat(Chevalier6.base_stats, "Accuracy", 160f);
            Chevalier6.action_special_effect += traitAction.Chevalier7_effectAction;
            Chevalier6.action_special_effect += traitAction.Chevalier6_Regen;
            Chevalier6.action_attack_target += traitAction.TrueDamage1_AttackAction;
            AssetManager.traits.add(Chevalier6);

            ActorTrait Chevalier7 = CreateTrait("Chevalier7", "trait/Chevalier7", "Chevalier");
            Chevalier7.rarity = Rarity.R2_Epic;
            SafeSetStat(Chevalier7.base_stats, stats.Resist.id, 32.0f);
            SafeSetStat(Chevalier7.base_stats, strings.S.warfare, 30f);
            SafeSetStat(Chevalier7.base_stats, strings.S.damage, 2000f);
            SafeSetStat(Chevalier7.base_stats, strings.S.mass, 25f);
            SafeSetStat(Chevalier7.base_stats, strings.S.armor, 35f);
            SafeSetStat(Chevalier7.base_stats, strings.S.health, 12000f);
            SafeSetStat(Chevalier7.base_stats, strings.S.speed, 20f);
            SafeSetStat(Chevalier7.base_stats, strings.S.area_of_effect, 20f);
            SafeSetStat(Chevalier7.base_stats, strings.S.targets, 7f);
            SafeSetStat(Chevalier7.base_stats, strings.S.critical_chance, 0.6f);
            SafeSetStat(Chevalier7.base_stats, strings.S.accuracy, 40f);
            SafeSetStat(Chevalier7.base_stats, strings.S.multiplier_speed, 0.8f);
            SafeSetStat(Chevalier7.base_stats, strings.S.stamina, 140f);
            SafeSetStat(Chevalier7.base_stats, strings.S.range, 2f);
            SafeSetStat(Chevalier7.base_stats, strings.S.attack_speed, 2f);
            SafeSetStat(Chevalier7.base_stats, strings.S.scale, 0.04f);
            SafeSetStat(Chevalier7.base_stats, strings.S.multiplier_health, 0.2f);
            SafeSetStat(Chevalier7.base_stats, strings.S.multiplier_damage, 0.2f);
            SafeSetStat(Chevalier7.base_stats, "Dodge", 220f);
            SafeSetStat(Chevalier7.base_stats, "Accuracy", 180f);
            Chevalier7.action_attack_target += traitAction.TrueDamage2_AttackAction;
            Chevalier7.action_special_effect += traitAction.Chevalier8_effectAction;
            Chevalier7.action_special_effect += traitAction.Chevalier7_Regen;
            AssetManager.traits.add(Chevalier7);

            ActorTrait Chevalier8 = CreateTrait("Chevalier8", "trait/Chevalier8", "Chevalier");
            Chevalier8.rarity = Rarity.R2_Epic;
            SafeSetStat(Chevalier8.base_stats, stats.Resist.id, 64.0f);
            SafeSetStat(Chevalier8.base_stats, strings.S.warfare, 40f);
            SafeSetStat(Chevalier8.base_stats, strings.S.damage, 4000f);
            SafeSetStat(Chevalier8.base_stats, strings.S.mass, 60f);
            SafeSetStat(Chevalier8.base_stats, strings.S.armor, 45f);
            SafeSetStat(Chevalier8.base_stats, strings.S.health, 30000f);
            SafeSetStat(Chevalier8.base_stats, strings.S.speed, 25f);
            SafeSetStat(Chevalier8.base_stats, strings.S.area_of_effect, 30f);
            SafeSetStat(Chevalier8.base_stats, strings.S.targets, 9f);
            SafeSetStat(Chevalier8.base_stats, strings.S.critical_chance, 0.7f);
            SafeSetStat(Chevalier8.base_stats, strings.S.accuracy, 50f);
            SafeSetStat(Chevalier8.base_stats, strings.S.multiplier_speed, 1.0f);
            SafeSetStat(Chevalier8.base_stats, strings.S.stamina, 240f);
            SafeSetStat(Chevalier8.base_stats, strings.S.range, 4f);
            SafeSetStat(Chevalier8.base_stats, strings.S.attack_speed, 4f);
            SafeSetStat(Chevalier8.base_stats, strings.S.multiplier_health, 0.4f);
            SafeSetStat(Chevalier8.base_stats, strings.S.multiplier_damage, 0.4f);
            SafeSetStat(Chevalier8.base_stats, strings.S.scale, 0.06f);
            SafeSetStat(Chevalier8.base_stats, "Dodge", 260f);
            SafeSetStat(Chevalier8.base_stats, "Accuracy", 220f);
            Chevalier8.action_attack_target += traitAction.TrueDamage3_AttackAction;
            Chevalier8.action_special_effect += traitAction.Chevalier9_effectAction;
            Chevalier8.action_special_effect += traitAction.Chevalier8_Regen;
            AssetManager.traits.add(Chevalier8);

            ActorTrait Chevalier9 = CreateTrait("Chevalier9", "trait/Chevalier9", "Chevalier");
            Chevalier9.rarity = Rarity.R2_Epic;
            SafeSetStat(Chevalier9.base_stats, stats.Resist.id, 128.0f);
            SafeSetStat(Chevalier9.base_stats, strings.S.warfare, 50f);
            SafeSetStat(Chevalier9.base_stats, strings.S.damage, 9000f);
            SafeSetStat(Chevalier9.base_stats, strings.S.mass, 140f);
            SafeSetStat(Chevalier9.base_stats, strings.S.armor, 60f);
            SafeSetStat(Chevalier9.base_stats, strings.S.health, 60000f);
            SafeSetStat(Chevalier9.base_stats, strings.S.speed, 30f);
            SafeSetStat(Chevalier9.base_stats, strings.S.area_of_effect, 40f);
            SafeSetStat(Chevalier9.base_stats, strings.S.targets, 12f);
            SafeSetStat(Chevalier9.base_stats, strings.S.critical_chance, 0.8f);
            SafeSetStat(Chevalier9.base_stats, strings.S.accuracy, 60f);
            SafeSetStat(Chevalier9.base_stats, strings.S.multiplier_speed, 1.2f);
            SafeSetStat(Chevalier9.base_stats, strings.S.stamina, 400f);
            SafeSetStat(Chevalier9.base_stats, strings.S.range, 6f);
            SafeSetStat(Chevalier9.base_stats, strings.S.attack_speed, 6f);
            SafeSetStat(Chevalier9.base_stats, strings.S.scale, 0.08f);
            SafeSetStat(Chevalier9.base_stats, strings.S.multiplier_health, 0.6f);
            SafeSetStat(Chevalier9.base_stats, strings.S.multiplier_damage, 0.6f);
            SafeSetStat(Chevalier9.base_stats, "Dodge", 300f);
            SafeSetStat(Chevalier9.base_stats, "Accuracy", 260f);
            Chevalier9.action_attack_target += traitAction.TrueDamage4_AttackAction;
            Chevalier9.action_special_effect += traitAction.Chevalier91_effectAction;
            Chevalier9.action_special_effect += traitAction.Chevalier9_Regen;
            AssetManager.traits.add(Chevalier9);

            ActorTrait Chevalier91 = CreateTrait("Chevalier91", "trait/Chevalier91", "Chevalier");
            Chevalier91.rarity = Rarity.R3_Legendary;
            SafeSetStat(Chevalier91.base_stats, stats.Resist.id, 160.0f);
            SafeSetStat(Chevalier91.base_stats, strings.S.warfare, 100f);
            SafeSetStat(Chevalier91.base_stats, strings.S.damage, 20000f);
            SafeSetStat(Chevalier91.base_stats, strings.S.mass, 400f);
            SafeSetStat(Chevalier91.base_stats, strings.S.armor, 80f);
            SafeSetStat(Chevalier91.base_stats, strings.S.health, 200000f);
            SafeSetStat(Chevalier91.base_stats, strings.S.speed, 100f);
            SafeSetStat(Chevalier91.base_stats, strings.S.area_of_effect, 50f);
            SafeSetStat(Chevalier91.base_stats, strings.S.targets, 30f);
            SafeSetStat(Chevalier91.base_stats, strings.S.critical_chance, 1.8f);
            SafeSetStat(Chevalier91.base_stats, strings.S.accuracy, 160f);
            SafeSetStat(Chevalier91.base_stats, strings.S.multiplier_speed, 1.5f);
            SafeSetStat(Chevalier91.base_stats, strings.S.stamina, 1200f);
            SafeSetStat(Chevalier91.base_stats, strings.S.range, 24f);
            SafeSetStat(Chevalier91.base_stats, strings.S.attack_speed, 24f);
            SafeSetStat(Chevalier91.base_stats, strings.S.scale, 0.2f);
            SafeSetStat(Chevalier91.base_stats, strings.S.multiplier_health, 1.2f);
            SafeSetStat(Chevalier91.base_stats, strings.S.multiplier_damage, 1.2f);
            SafeSetStat(Chevalier91.base_stats, "Dodge", 340f);
            SafeSetStat(Chevalier91.base_stats, "Accuracy", 300f);
            Chevalier91.action_special_effect += traitAction.Chevalier92_effectAction;
            Chevalier91.action_special_effect += traitAction.Chevalier91_Regen;
            Chevalier91.action_attack_target += traitAction.TrueDamage5_AttackAction;
            Chevalier91.action_attack_target += traitAction.fire1_attackAction;
            Chevalier91.action_attack_target += traitAction.TrueDamageByChevalier1_AttackAction;
            Chevalier91.action_special_effect += traitAction.MaintainFullNutrition;
            AssetManager.traits.add(Chevalier91);

            ActorTrait Chevalier92 = CreateTrait("Chevalier92", "trait/Chevalier92", "Chevalier");
            Chevalier92.rarity = Rarity.R3_Legendary;
            SafeSetStat(Chevalier92.base_stats, stats.Resist.id, 200.0f);
            SafeSetStat(Chevalier92.base_stats, strings.S.warfare, 200f);
            SafeSetStat(Chevalier92.base_stats, strings.S.damage, 40000f);
            SafeSetStat(Chevalier92.base_stats, strings.S.mass, 1600f);
            SafeSetStat(Chevalier92.base_stats, strings.S.armor, 100f);
            SafeSetStat(Chevalier92.base_stats, strings.S.health, 500000f);
            SafeSetStat(Chevalier92.base_stats, strings.S.speed, 200f);
            SafeSetStat(Chevalier92.base_stats, strings.S.area_of_effect, 70f);
            SafeSetStat(Chevalier92.base_stats, strings.S.targets, 40f);
            SafeSetStat(Chevalier92.base_stats, strings.S.critical_chance, 2.0f);
            SafeSetStat(Chevalier92.base_stats, strings.S.accuracy, 200f);
            SafeSetStat(Chevalier92.base_stats, strings.S.multiplier_speed, 2.0f);
            SafeSetStat(Chevalier92.base_stats, strings.S.stamina, 3000f);
            SafeSetStat(Chevalier92.base_stats, strings.S.range, 40f);
            SafeSetStat(Chevalier92.base_stats, strings.S.attack_speed, 40f);
            SafeSetStat(Chevalier92.base_stats, strings.S.scale, 0.3f);
            SafeSetStat(Chevalier92.base_stats, strings.S.multiplier_health, 2f);
            SafeSetStat(Chevalier92.base_stats, strings.S.multiplier_damage, 2f);
            SafeSetStat(Chevalier92.base_stats, "Dodge", 380f);
            SafeSetStat(Chevalier92.base_stats, "Accuracy", 340f);
            Chevalier92.action_special_effect += traitAction.Chevalier93_effectAction;
            Chevalier92.action_special_effect += traitAction.Chevalier92_Regen;
            Chevalier92.action_attack_target += traitAction.TrueDamage6_AttackAction;
            Chevalier92.action_attack_target += traitAction.fire1_attackAction;
            Chevalier92.action_attack_target += traitAction.TrueDamageByChevalier2_AttackAction;
            Chevalier92.action_special_effect += traitAction.MaintainFullNutrition;
            AssetManager.traits.add(Chevalier92);

            ActorTrait Chevalier93 = CreateTrait("Chevalier93", "trait/Chevalier93", "Chevalier");
            Chevalier93.rarity = Rarity.R3_Legendary;
            SafeSetStat(Chevalier93.base_stats, stats.Resist.id, 240.0f);
            SafeSetStat(Chevalier93.base_stats, strings.S.warfare, 400f);
            SafeSetStat(Chevalier93.base_stats, strings.S.damage, 100000f);
            SafeSetStat(Chevalier93.base_stats, strings.S.mass, 2400f);
            SafeSetStat(Chevalier93.base_stats, strings.S.armor, 160f);
            SafeSetStat(Chevalier93.base_stats, strings.S.health, 2000000f);
            SafeSetStat(Chevalier93.base_stats, strings.S.speed, 300f);
            SafeSetStat(Chevalier93.base_stats, strings.S.area_of_effect, 100f);
            SafeSetStat(Chevalier93.base_stats, strings.S.targets, 50f);
            SafeSetStat(Chevalier93.base_stats, strings.S.critical_chance, 3.0f);
            SafeSetStat(Chevalier93.base_stats, strings.S.accuracy, 360f);
            SafeSetStat(Chevalier93.base_stats, strings.S.multiplier_speed, 3.0f);
            SafeSetStat(Chevalier93.base_stats, strings.S.stamina, 10000f);
            SafeSetStat(Chevalier93.base_stats, strings.S.range, 60f);
            SafeSetStat(Chevalier93.base_stats, strings.S.attack_speed, 60f);
            SafeSetStat(Chevalier93.base_stats, strings.S.scale, 0.4f);
            SafeSetStat(Chevalier93.base_stats, strings.S.multiplier_health, 3f);
            SafeSetStat(Chevalier93.base_stats, strings.S.multiplier_damage, 3f);    
            SafeSetStat(Chevalier93.base_stats, "Dodge", 500f);
            SafeSetStat(Chevalier93.base_stats, "Accuracy", 480f);
            Chevalier93.action_special_effect += traitAction.Chevalier93_Regen;
            Chevalier93.action_attack_target += traitAction.TrueDamage7_AttackAction;
            Chevalier93.action_attack_target += traitAction.fire2_attackAction;
            Chevalier93.action_attack_target += traitAction.TrueDamageByChevalier3_AttackAction;
            Chevalier93.action_special_effect += traitAction.MaintainFullNutrition;
            AssetManager.traits.add(Chevalier93);

            ActorTrait Chevalier94 = CreateTrait("Chevalier94", "trait/Chevalier94", "Chevalier");
            SafeSetStat(Chevalier94.base_stats, strings.S.multiplier_lifespan, -0.05f);
            SafeSetStat(Chevalier94.base_stats, strings.S.multiplier_health, -0.05f);
            SafeSetStat(Chevalier94.base_stats, strings.S.multiplier_damage, -0.05f);
            AssetManager.traits.add(Chevalier94);

            ActorTrait Chevalier95 = CreateTrait("Chevalier95", "trait/Chevalier95", "Chevalier");
            SafeSetStat(Chevalier95.base_stats, strings.S.multiplier_lifespan, -0.1f);
            SafeSetStat(Chevalier95.base_stats, strings.S.multiplier_health, -0.1f);
            SafeSetStat(Chevalier95.base_stats, strings.S.multiplier_damage, -0.1f);
            AssetManager.traits.add(Chevalier95);

            ActorTrait Chevalier96 = CreateTrait("Chevalier96", "trait/Chevalier96", "Chevalier");
            Chevalier96.rarity = Rarity.R2_Epic;
            SafeSetStat(Chevalier96.base_stats, strings.S.multiplier_lifespan, -0.15f);
            SafeSetStat(Chevalier96.base_stats, strings.S.multiplier_health, -0.15f);
            SafeSetStat(Chevalier96.base_stats, strings.S.multiplier_damage, -0.15f);
            AssetManager.traits.add(Chevalier96);

            ActorTrait Chevalier97 = CreateTrait("Chevalier97", "trait/Chevalier97", "Chevalier");
            Chevalier97.rarity = Rarity.R2_Epic;
            SafeSetStat(Chevalier97.base_stats, strings.S.multiplier_lifespan, -0.2f);
            SafeSetStat(Chevalier97.base_stats, strings.S.multiplier_health, -0.2f);
            SafeSetStat(Chevalier97.base_stats, strings.S.multiplier_damage, -0.2f);
            AssetManager.traits.add(Chevalier97);

            ActorTrait Chevalier98 = CreateTrait("Chevalier98", "trait/Chevalier98", "Chevalier");
            Chevalier98.rarity = Rarity.R3_Legendary;
            SafeSetStat(Chevalier98.base_stats, strings.S.multiplier_lifespan, -0.25f);
            SafeSetStat(Chevalier98.base_stats, strings.S.multiplier_health, -0.25f);
            SafeSetStat(Chevalier98.base_stats, strings.S.multiplier_damage, -0.25f);
            AssetManager.traits.add(Chevalier98);

            ActorTrait Chevalier99 = CreateTrait("Chevalier99", "trait/Chevalier99", "Chevalier");
            Chevalier99.rarity = Rarity.R3_Legendary;
            SafeSetStat(Chevalier99.base_stats, strings.S.multiplier_lifespan, -0.3f);
            SafeSetStat(Chevalier99.base_stats, strings.S.multiplier_health, -0.3f);
            SafeSetStat(Chevalier99.base_stats, strings.S.multiplier_damage, -0.3f);
            AssetManager.traits.add(Chevalier99);

            ActorTrait LegacyTechnique1 = CreateTrait("LegacyTechnique1", "trait/LegacyTechnique1", "LegacyTechnique");
            SafeSetStat(LegacyTechnique1.base_stats, strings.S.damage, 500f);
            SafeSetStat(LegacyTechnique1.base_stats, strings.S.health, 500f);
            SafeSetStat(LegacyTechnique1.base_stats, strings.S.speed, 30f);
            SafeSetStat(LegacyTechnique1.base_stats, strings.S.armor, 15f);
            SafeSetStat(LegacyTechnique1.base_stats, strings.S.lifespan, 10f);
            SafeSetStat(LegacyTechnique1.base_stats, strings.S.stamina, 80f);
            SafeSetStat(LegacyTechnique1.base_stats, strings.S.attack_speed, 80f);
            SafeSetStat(LegacyTechnique1.base_stats, strings.S.multiplier_health, 0.1f);
            SafeSetStat(LegacyTechnique1.base_stats, strings.S.multiplier_damage, 0.1f);
            AssetManager.traits.add(LegacyTechnique1);

            ActorTrait LegacyTechnique2 = CreateTrait("LegacyTechnique2", "trait/LegacyTechnique2", "LegacyTechnique");
            SafeSetStat(LegacyTechnique2.base_stats, strings.S.speed, 80f);
            SafeSetStat(LegacyTechnique2.base_stats, strings.S.armor, 20f);
            SafeSetStat(LegacyTechnique2.base_stats, strings.S.damage, 800f);
            SafeSetStat(LegacyTechnique2.base_stats, strings.S.health, 400f);
            SafeSetStat(LegacyTechnique2.base_stats, strings.S.lifespan, 30f);
            SafeSetStat(LegacyTechnique2.base_stats, strings.S.stamina, 50f);
            SafeSetStat(LegacyTechnique2.base_stats, strings.S.attack_speed, 40f);
            SafeSetStat(LegacyTechnique2.base_stats, strings.S.multiplier_health, 0.1f);
            SafeSetStat(LegacyTechnique2.base_stats, strings.S.multiplier_damage, 0.2f);
            SafeSetStat(LegacyTechnique2.base_stats, strings.S.multiplier_speed, 0.2f);
            AssetManager.traits.add(LegacyTechnique2);

            ActorTrait LegacyTechnique3 = CreateTrait("LegacyTechnique3", "trait/LegacyTechnique3", "LegacyTechnique");
            SafeSetStat(LegacyTechnique3.base_stats, strings.S.damage, 200f);
            SafeSetStat(LegacyTechnique3.base_stats, strings.S.health, 1000f);
            SafeSetStat(LegacyTechnique3.base_stats, strings.S.speed, 10f);
            SafeSetStat(LegacyTechnique3.base_stats, strings.S.armor, 80f);
            SafeSetStat(LegacyTechnique3.base_stats, strings.S.lifespan, 50f);
            SafeSetStat(LegacyTechnique3.base_stats, strings.S.stamina, 200f);
            SafeSetStat(LegacyTechnique3.base_stats, strings.S.attack_speed, 20f);
            SafeSetStat(LegacyTechnique3.base_stats, strings.S.multiplier_health, 0.2f);
            SafeSetStat(LegacyTechnique3.base_stats, strings.S.multiplier_damage, 0.3f);
            SafeSetStat(LegacyTechnique3.base_stats, strings.S.multiplier_speed, 0.1f);
            AssetManager.traits.add(LegacyTechnique3);

            ActorTrait LegacyTechnique4 = CreateTrait("LegacyTechnique4", "trait/LegacyTechnique4", "LegacyTechnique");
            SafeSetStat(LegacyTechnique4.base_stats, strings.S.speed, 60f);
            SafeSetStat(LegacyTechnique4.base_stats, strings.S.armor, 50f);
            SafeSetStat(LegacyTechnique4.base_stats, strings.S.damage, 400f);
            SafeSetStat(LegacyTechnique4.base_stats, strings.S.health, 400f);
            SafeSetStat(LegacyTechnique4.base_stats, strings.S.lifespan, 100f);
            SafeSetStat(LegacyTechnique4.base_stats, strings.S.stamina, 120f);
            SafeSetStat(LegacyTechnique4.base_stats, strings.S.attack_speed, 10f);
            SafeSetStat(LegacyTechnique4.base_stats, strings.S.multiplier_health, 0.1f);
            SafeSetStat(LegacyTechnique4.base_stats, strings.S.multiplier_damage, 0.1f);
            SafeSetStat(LegacyTechnique4.base_stats, strings.S.multiplier_speed, 0.3f);
            AssetManager.traits.add(LegacyTechnique4);

            ActorTrait LegacyTechnique5 = CreateTrait("LegacyTechnique5", "trait/LegacyTechnique5", "LegacyTechnique");
            SafeSetStat(LegacyTechnique5.base_stats, strings.S.speed, 40f);
            SafeSetStat(LegacyTechnique5.base_stats, strings.S.armor, 60f);
            SafeSetStat(LegacyTechnique5.base_stats, strings.S.health, 200f);
            SafeSetStat(LegacyTechnique5.base_stats, strings.S.damage, 300f);
            SafeSetStat(LegacyTechnique5.base_stats, strings.S.lifespan, 300f);
            SafeSetStat(LegacyTechnique5.base_stats, strings.S.stamina, 30f);
            SafeSetStat(LegacyTechnique5.base_stats, strings.S.attack_speed, 40f);
            SafeSetStat(LegacyTechnique5.base_stats, strings.S.multiplier_health, 0.2f);
            SafeSetStat(LegacyTechnique5.base_stats, strings.S.multiplier_damage, 0.2f);
            SafeSetStat(LegacyTechnique5.base_stats, strings.S.multiplier_speed, 0.1f);
            AssetManager.traits.add(LegacyTechnique5);

            ActorTrait LegacyTechnique6 = CreateTrait("LegacyTechnique6", "trait/LegacyTechnique6", "LegacyTechnique");
            SafeSetStat(LegacyTechnique6.base_stats, strings.S.speed, 50f);
            SafeSetStat(LegacyTechnique6.base_stats, strings.S.armor, 50f);
            SafeSetStat(LegacyTechnique6.base_stats, strings.S.health, 100f);
            SafeSetStat(LegacyTechnique6.base_stats, strings.S.damage, 200f);
            SafeSetStat(LegacyTechnique6.base_stats, strings.S.lifespan, 30f);
            SafeSetStat(LegacyTechnique6.base_stats, strings.S.stamina, 90f);
            SafeSetStat(LegacyTechnique6.base_stats, strings.S.attack_speed, 70f);
            SafeSetStat(LegacyTechnique6.base_stats, strings.S.multiplier_health, 0.1f);
            SafeSetStat(LegacyTechnique6.base_stats, strings.S.multiplier_damage, 0.1f);
            SafeSetStat(LegacyTechnique6.base_stats, strings.S.multiplier_speed, 0.1f);
            AssetManager.traits.add(LegacyTechnique6);

            ActorTrait LegacyTechnique7 = CreateTrait("LegacyTechnique7", "trait/LegacyTechnique7", "LegacyTechnique");
            SafeSetStat(LegacyTechnique7.base_stats, strings.S.speed, 60f);
            SafeSetStat(LegacyTechnique7.base_stats, strings.S.armor, 20f);
            SafeSetStat(LegacyTechnique7.base_stats, strings.S.health, 180f);
            SafeSetStat(LegacyTechnique7.base_stats, strings.S.damage, 120f);
            SafeSetStat(LegacyTechnique7.base_stats, strings.S.lifespan, 50f);
            SafeSetStat(LegacyTechnique7.base_stats, strings.S.stamina, 40f);
            SafeSetStat(LegacyTechnique7.base_stats, strings.S.attack_speed, 40f);
            SafeSetStat(LegacyTechnique7.base_stats, strings.S.multiplier_health, 0.3f);
            SafeSetStat(LegacyTechnique7.base_stats, strings.S.multiplier_damage, 0.1f);
            SafeSetStat(LegacyTechnique7.base_stats, strings.S.multiplier_speed, 0.1f);
            AssetManager.traits.add(LegacyTechnique7);

            ActorTrait LegacyTechnique8 = CreateTrait("LegacyTechnique8", "trait/LegacyTechnique8", "LegacyTechnique");
            SafeSetStat(LegacyTechnique8.base_stats, strings.S.speed, 45f);
            SafeSetStat(LegacyTechnique8.base_stats, strings.S.armor, 55f);
            SafeSetStat(LegacyTechnique8.base_stats, strings.S.health, 600f);
            SafeSetStat(LegacyTechnique8.base_stats, strings.S.damage, 100f);
            SafeSetStat(LegacyTechnique8.base_stats, strings.S.lifespan, 40f);
            SafeSetStat(LegacyTechnique8.base_stats, strings.S.stamina, 70f);
            SafeSetStat(LegacyTechnique8.base_stats, strings.S.attack_speed, 30f);
            SafeSetStat(LegacyTechnique8.base_stats, strings.S.multiplier_health, 0.1f);
            SafeSetStat(LegacyTechnique8.base_stats, strings.S.multiplier_damage, 0.2f);
            SafeSetStat(LegacyTechnique8.base_stats, strings.S.multiplier_speed, 0.2f);
            AssetManager.traits.add(LegacyTechnique8);

            ActorTrait LegacyTechnique9 = CreateTrait("LegacyTechnique9", "trait/LegacyTechnique9", "LegacyTechnique");
            SafeSetStat(LegacyTechnique9.base_stats, strings.S.speed, 40f);
            SafeSetStat(LegacyTechnique9.base_stats, strings.S.armor, 60f);
            SafeSetStat(LegacyTechnique9.base_stats, strings.S.health, 800f);
            SafeSetStat(LegacyTechnique9.base_stats, strings.S.damage, 240f);
            SafeSetStat(LegacyTechnique9.base_stats, strings.S.lifespan, 30f);
            SafeSetStat(LegacyTechnique9.base_stats, strings.S.stamina, 90f);
            SafeSetStat(LegacyTechnique9.base_stats, strings.S.attack_speed, 60f);
            SafeSetStat(LegacyTechnique9.base_stats, strings.S.multiplier_health, 0.1f);
            SafeSetStat(LegacyTechnique9.base_stats, strings.S.multiplier_damage, 0.2f);
            SafeSetStat(LegacyTechnique9.base_stats, strings.S.multiplier_speed, 0.2f);
            AssetManager.traits.add(LegacyTechnique9);

            ActorTrait LegacyTechnique10 = CreateTrait("LegacyTechnique10", "trait/LegacyTechnique10", "LegacyTechnique");
            LegacyTechnique10.rarity = Rarity.R2_Epic;
            SafeSetStat(LegacyTechnique10.base_stats, strings.S.speed, 10f);
            SafeSetStat(LegacyTechnique10.base_stats, strings.S.armor, 10f);
            SafeSetStat(LegacyTechnique10.base_stats, strings.S.health, 2000f);
            SafeSetStat(LegacyTechnique10.base_stats, strings.S.damage, 90f);
            SafeSetStat(LegacyTechnique10.base_stats, strings.S.lifespan, 500f);
            SafeSetStat(LegacyTechnique10.base_stats, strings.S.stamina, 60f);
            SafeSetStat(LegacyTechnique10.base_stats, strings.S.attack_speed, 50f);
            SafeSetStat(LegacyTechnique10.base_stats, strings.S.multiplier_health, 0.3f);
            SafeSetStat(LegacyTechnique10.base_stats, strings.S.multiplier_damage, 0.3f);
            SafeSetStat(LegacyTechnique10.base_stats, strings.S.multiplier_speed, 0.3f);
            AssetManager.traits.add(LegacyTechnique10);

            ActorTrait LegacyTechnique11 = CreateTrait("LegacyTechnique11", "trait/LegacyTechnique11", "LegacyTechnique");
            SafeSetStat(LegacyTechnique11.base_stats, strings.S.speed, 10f);
            SafeSetStat(LegacyTechnique11.base_stats, strings.S.armor, -10f);
            SafeSetStat(LegacyTechnique11.base_stats, strings.S.health, -50f);
            SafeSetStat(LegacyTechnique11.base_stats, strings.S.damage, 5000f);
            SafeSetStat(LegacyTechnique11.base_stats, strings.S.lifespan, -10f);
            SafeSetStat(LegacyTechnique11.base_stats, strings.S.stamina, -30f);
            SafeSetStat(LegacyTechnique11.base_stats, strings.S.attack_speed, 100f);
            SafeSetStat(LegacyTechnique11.base_stats, strings.S.multiplier_health, 0.1f);
            SafeSetStat(LegacyTechnique11.base_stats, strings.S.multiplier_damage, 0.6f);
            SafeSetStat(LegacyTechnique11.base_stats, strings.S.multiplier_speed, 0.1f);
            AssetManager.traits.add(LegacyTechnique11);

            ActorTrait LegacyTechnique12 = CreateTrait("LegacyTechnique12", "trait/LegacyTechnique12", "LegacyTechnique");
            LegacyTechnique12.rarity = Rarity.R3_Legendary;
            SafeSetStat(LegacyTechnique12.base_stats, strings.S.speed, 40f);
            SafeSetStat(LegacyTechnique12.base_stats, strings.S.armor, 100f);
            SafeSetStat(LegacyTechnique12.base_stats, strings.S.health, 8000f);
            SafeSetStat(LegacyTechnique12.base_stats, strings.S.damage, 40f); 
            SafeSetStat(LegacyTechnique12.base_stats, strings.S.lifespan, 600f);
            SafeSetStat(LegacyTechnique12.base_stats, strings.S.stamina, 1000f);
            SafeSetStat(LegacyTechnique12.base_stats, strings.S.attack_speed, 20f);
            SafeSetStat(LegacyTechnique12.base_stats, strings.S.multiplier_health, 0.5f);
            SafeSetStat(LegacyTechnique12.base_stats, strings.S.multiplier_damage, 0.5f);
            SafeSetStat(LegacyTechnique12.base_stats, strings.S.multiplier_speed, 0.5f);
            AssetManager.traits.add(LegacyTechnique12);

            ActorTrait LegacyTechnique13 = CreateTrait("LegacyTechnique13", "trait/LegacyTechnique13", "LegacyTechnique");
            SafeSetStat(LegacyTechnique13.base_stats, strings.S.speed, 80f);
            SafeSetStat(LegacyTechnique13.base_stats, strings.S.armor, 80f);
            SafeSetStat(LegacyTechnique13.base_stats, strings.S.health, 800f);
            SafeSetStat(LegacyTechnique13.base_stats, strings.S.damage, 800f); 
            SafeSetStat(LegacyTechnique13.base_stats, strings.S.lifespan, 80f);
            SafeSetStat(LegacyTechnique13.base_stats, strings.S.stamina, 80f);
            SafeSetStat(LegacyTechnique13.base_stats, strings.S.attack_speed, 80f);
            SafeSetStat(LegacyTechnique13.base_stats, strings.S.multiplier_health, 0.2f);
            SafeSetStat(LegacyTechnique13.base_stats, strings.S.multiplier_damage, 0.2f);
            SafeSetStat(LegacyTechnique13.base_stats, strings.S.multiplier_speed, 0.1f);
            AssetManager.traits.add(LegacyTechnique13);

            ActorTrait LegacyTechnique14 = CreateTrait("LegacyTechnique14", "trait/LegacyTechnique14", "LegacyTechnique");
            SafeSetStat(LegacyTechnique14.base_stats, strings.S.speed, 40f);
            SafeSetStat(LegacyTechnique14.base_stats, strings.S.armor, 40f);
            SafeSetStat(LegacyTechnique14.base_stats, strings.S.health, -200f);
            SafeSetStat(LegacyTechnique14.base_stats, strings.S.damage, 400f); 
            SafeSetStat(LegacyTechnique14.base_stats, strings.S.multiplier_lifespan, -0.5f);
            SafeSetStat(LegacyTechnique14.base_stats, strings.S.stamina, 40f);
            SafeSetStat(LegacyTechnique14.base_stats, strings.S.attack_speed, 40f);
            SafeSetStat(LegacyTechnique14.base_stats, strings.S.multiplier_health, 0.1f);
            SafeSetStat(LegacyTechnique14.base_stats, strings.S.multiplier_damage, 0.1f);
            SafeSetStat(LegacyTechnique14.base_stats, strings.S.multiplier_speed, 0.1f);
            AssetManager.traits.add(LegacyTechnique14);

            ActorTrait LegacyTechnique91 = CreateTrait("LegacyTechnique91", "trait/LegacyTechnique91", "LegacyTechnique");
            LegacyTechnique91.rarity = Rarity.R3_Legendary;
            SafeSetStat(LegacyTechnique91.base_stats, strings.S.speed, 100f);
            SafeSetStat(LegacyTechnique91.base_stats, strings.S.armor, 100f);
            SafeSetStat(LegacyTechnique91.base_stats, strings.S.health, 6000f);
            SafeSetStat(LegacyTechnique91.base_stats, strings.S.damage, 6000f); 
            SafeSetStat(LegacyTechnique91.base_stats, strings.S.lifespan, 100f);
            SafeSetStat(LegacyTechnique91.base_stats, strings.S.stamina, 80f);
            SafeSetStat(LegacyTechnique91.base_stats, strings.S.attack_speed, 600f);
            SafeSetStat(LegacyTechnique91.base_stats, strings.S.multiplier_health, 9.9f);
            SafeSetStat(LegacyTechnique91.base_stats, strings.S.multiplier_damage, 9.9f);
            SafeSetStat(LegacyTechnique91.base_stats, strings.S.multiplier_speed, 9.9f);
            LegacyTechnique91.action_attack_target += traitAction.TrueDamage8_AttackAction;
            AssetManager.traits.add(LegacyTechnique91);

            ActorTrait NineLawsofKnighthood1 = CreateTrait("NineLawsofKnighthood1", "trait/NineLawsofKnighthood1", "NineLawsofKnighthood");
            NineLawsofKnighthood1.rarity = Rarity.R3_Legendary;
            SafeSetStat(NineLawsofKnighthood1.base_stats, strings.S.multiplier_health, 3.0f);
            SafeSetStat(NineLawsofKnighthood1.base_stats, strings.S.intelligence, 10f);
            SafeSetStat(NineLawsofKnighthood1.base_stats, strings.S.knockback, 10f); 
            SafeSetStat(NineLawsofKnighthood1.base_stats, strings.S.area_of_effect, 4f);
            NineLawsofKnighthood1.action_attack_target += traitAction.NineLawsofKnighthood1TrueDamage_AttackAction;
            AssetManager.traits.add(NineLawsofKnighthood1);

            ActorTrait NineLawsofKnighthood2 = CreateTrait("NineLawsofKnighthood2", "trait/NineLawsofKnighthood2", "NineLawsofKnighthood");
            NineLawsofKnighthood2.rarity = Rarity.R3_Legendary;
            SafeSetStat(NineLawsofKnighthood2.base_stats, strings.S.multiplier_damage, 1.8f);
            SafeSetStat(NineLawsofKnighthood2.base_stats, strings.S.multiplier_health, 1.8f);
            SafeSetStat(NineLawsofKnighthood2.base_stats, strings.S.multiplier_speed, 1.8f);
            SafeSetStat(NineLawsofKnighthood2.base_stats, strings.S.intelligence, 100f);
            SafeSetStat(NineLawsofKnighthood2.base_stats, strings.S.knockback, 20f); 
            SafeSetStat(NineLawsofKnighthood2.base_stats, strings.S.area_of_effect, 8f);
            NineLawsofKnighthood2.action_attack_target += traitAction.NineLawsofKnighthood2TrueDamage_AttackAction;
            AssetManager.traits.add(NineLawsofKnighthood2);

            ActorTrait NineLawsofKnighthood3 = CreateTrait("NineLawsofKnighthood3", "trait/NineLawsofKnighthood3", "NineLawsofKnighthood");
            NineLawsofKnighthood3.rarity = Rarity.R3_Legendary;
            SafeSetStat(NineLawsofKnighthood3.base_stats, strings.S.multiplier_speed, 3.2f);
            SafeSetStat(NineLawsofKnighthood3.base_stats, strings.S.multiplier_damage, 1.0f);
            SafeSetStat(NineLawsofKnighthood3.base_stats, strings.S.intelligence, 50f);
            SafeSetStat(NineLawsofKnighthood3.base_stats, strings.S.knockback, 10f); 
            SafeSetStat(NineLawsofKnighthood3.base_stats, strings.S.area_of_effect, 6f);
            NineLawsofKnighthood3.action_special_effect += traitAction.NineLawsofKnighthood3_Regen;
            NineLawsofKnighthood3.action_attack_target += traitAction.NineLawsofKnighthood3TrueDamage_AttackAction;
            AssetManager.traits.add(NineLawsofKnighthood3);

            ActorTrait NineLawsofKnighthood4 = CreateTrait("NineLawsofKnighthood4", "trait/NineLawsofKnighthood4", "NineLawsofKnighthood");
            NineLawsofKnighthood4.rarity = Rarity.R3_Legendary;
            SafeSetStat(NineLawsofKnighthood4.base_stats, strings.S.multiplier_speed, 1.2f);
            SafeSetStat(NineLawsofKnighthood4.base_stats, strings.S.multiplier_damage, 1.0f);
            SafeSetStat(NineLawsofKnighthood4.base_stats, strings.S.multiplier_health, 1.0f);
            SafeSetStat(NineLawsofKnighthood4.base_stats, strings.S.intelligence, 10f);
            SafeSetStat(NineLawsofKnighthood4.base_stats, strings.S.knockback, 5f); 
            SafeSetStat(NineLawsofKnighthood4.base_stats, strings.S.area_of_effect, 4f);
            NineLawsofKnighthood4.action_attack_target += traitAction.NineLawsofKnighthood4TrueDamage_AttackAction;
            AssetManager.traits.add(NineLawsofKnighthood4);

            ActorTrait NineLawsofKnighthood5 = CreateTrait("NineLawsofKnighthood5", "trait/NineLawsofKnighthood5", "NineLawsofKnighthood");
            NineLawsofKnighthood5.rarity = Rarity.R3_Legendary;
            SafeSetStat(NineLawsofKnighthood5.base_stats, strings.S.multiplier_speed, 1.0f);
            SafeSetStat(NineLawsofKnighthood5.base_stats, strings.S.multiplier_damage, 0.8f);
            SafeSetStat(NineLawsofKnighthood5.base_stats, strings.S.multiplier_health, 2.0f);
            SafeSetStat(NineLawsofKnighthood5.base_stats, strings.S.intelligence, 60f);
            SafeSetStat(NineLawsofKnighthood5.base_stats, strings.S.knockback, 20f); 
            SafeSetStat(NineLawsofKnighthood5.base_stats, strings.S.area_of_effect, 8f);
            NineLawsofKnighthood5.action_attack_target += traitAction.NineLawsofKnighthood5TrueDamage_AttackAction;
            AssetManager.traits.add(NineLawsofKnighthood5);

            ActorTrait NineLawsofKnighthood6 = CreateTrait("NineLawsofKnighthood6", "trait/NineLawsofKnighthood6", "NineLawsofKnighthood");
            NineLawsofKnighthood6.rarity = Rarity.R3_Legendary;
            SafeSetStat(NineLawsofKnighthood6.base_stats, strings.S.multiplier_speed, 0.8f);
            SafeSetStat(NineLawsofKnighthood6.base_stats, strings.S.multiplier_damage, 2.0f);
            SafeSetStat(NineLawsofKnighthood6.base_stats, strings.S.multiplier_health, 0.8f);
            SafeSetStat(NineLawsofKnighthood6.base_stats, strings.S.intelligence, 10f);
            SafeSetStat(NineLawsofKnighthood6.base_stats, strings.S.knockback, 10f); 
            SafeSetStat(NineLawsofKnighthood6.base_stats, strings.S.area_of_effect, 10f);
            NineLawsofKnighthood6.action_special_effect += traitAction.NineLawsofKnighthood6_Regen;
            NineLawsofKnighthood6.action_attack_target += traitAction.NineLawsofKnighthood6TrueDamage_AttackAction;
            AssetManager.traits.add(NineLawsofKnighthood6);

            ActorTrait NineLawsofKnighthood7 = CreateTrait("NineLawsofKnighthood7", "trait/NineLawsofKnighthood7", "NineLawsofKnighthood");
            NineLawsofKnighthood7.rarity = Rarity.R3_Legendary;
            SafeSetStat(NineLawsofKnighthood7.base_stats, strings.S.multiplier_speed, 2.0f);
            SafeSetStat(NineLawsofKnighthood7.base_stats, strings.S.multiplier_damage, 0.8f);
            SafeSetStat(NineLawsofKnighthood7.base_stats, strings.S.multiplier_health, 0.8f);
            SafeSetStat(NineLawsofKnighthood7.base_stats, strings.S.intelligence, 20f);
            SafeSetStat(NineLawsofKnighthood7.base_stats, strings.S.knockback, 20f); 
            SafeSetStat(NineLawsofKnighthood7.base_stats, strings.S.area_of_effect, 6f);
            NineLawsofKnighthood7.action_attack_target += traitAction.NineLawsofKnighthood7TrueDamage_AttackAction;
            AssetManager.traits.add(NineLawsofKnighthood7);

            ActorTrait NineLawsofKnighthood8 = CreateTrait("NineLawsofKnighthood8", "trait/NineLawsofKnighthood8", "NineLawsofKnighthood");
            NineLawsofKnighthood8.rarity = Rarity.R3_Legendary;
            SafeSetStat(NineLawsofKnighthood8.base_stats, strings.S.multiplier_speed, 0.9f);
            SafeSetStat(NineLawsofKnighthood8.base_stats, strings.S.multiplier_damage, 0.9f);
            SafeSetStat(NineLawsofKnighthood8.base_stats, strings.S.multiplier_health, 0.9f);
            SafeSetStat(NineLawsofKnighthood8.base_stats, strings.S.intelligence, 1000f);
            SafeSetStat(NineLawsofKnighthood8.base_stats, strings.S.knockback, 100f); 
            SafeSetStat(NineLawsofKnighthood8.base_stats, strings.S.area_of_effect, 100f);
            NineLawsofKnighthood8.action_attack_target += traitAction.NineLawsofKnighthood8TrueDamage_AttackAction;
            AssetManager.traits.add(NineLawsofKnighthood8);

            ActorTrait NineLawsofKnighthood9 = CreateTrait("NineLawsofKnighthood9", "trait/NineLawsofKnighthood9", "NineLawsofKnighthood");
            NineLawsofKnighthood9.rarity = Rarity.R3_Legendary;
            SafeSetStat(NineLawsofKnighthood9.base_stats, strings.S.multiplier_speed, 2.0f);
            SafeSetStat(NineLawsofKnighthood9.base_stats, strings.S.multiplier_damage, 1.0f);
            SafeSetStat(NineLawsofKnighthood9.base_stats, strings.S.multiplier_health, 2.0f);
            SafeSetStat(NineLawsofKnighthood9.base_stats, strings.S.intelligence, 10f);
            SafeSetStat(NineLawsofKnighthood9.base_stats, strings.S.knockback, 10f); 
            SafeSetStat(NineLawsofKnighthood9.base_stats, strings.S.area_of_effect, 10f);
            NineLawsofKnighthood9.action_attack_target += traitAction.NineLawsofKnighthood9TrueDamage_AttackAction;
            AssetManager.traits.add(NineLawsofKnighthood9);

            ActorTrait LowFightingTechniqueTrait1 = CreateTrait("LowFightingTechniqueTrait1", "trait/LowFightingTechniqueTrait1", "LowFightingTechnique");
            SafeSetStat(LowFightingTechniqueTrait1.base_stats, strings.S.multiplier_speed, 0.03f);
            SafeSetStat(LowFightingTechniqueTrait1.base_stats, strings.S.multiplier_damage, 0.07f);
            SafeSetStat(LowFightingTechniqueTrait1.base_stats, strings.S.multiplier_health, 0.04f);
            SafeSetStat(LowFightingTechniqueTrait1.base_stats, strings.S.lifespan, 5f);
            LowFightingTechniqueTrait1.rate_inherit = 10;
            AssetManager.traits.add(LowFightingTechniqueTrait1);

            ActorTrait LowFightingTechniqueTrait2 = CreateTrait("LowFightingTechniqueTrait2", "trait/LowFightingTechniqueTrait2", "LowFightingTechnique");
            SafeSetStat(LowFightingTechniqueTrait2.base_stats, strings.S.multiplier_speed, 0.03f);
            SafeSetStat(LowFightingTechniqueTrait2.base_stats, strings.S.multiplier_damage, 0.07f);
            SafeSetStat(LowFightingTechniqueTrait2.base_stats, strings.S.multiplier_health, 0.04f);
            SafeSetStat(LowFightingTechniqueTrait2.base_stats, strings.S.lifespan, 8f);
            LowFightingTechniqueTrait2.rate_inherit = 10;
            AssetManager.traits.add(LowFightingTechniqueTrait2);

            ActorTrait LowFightingTechniqueTrait3 = CreateTrait("LowFightingTechniqueTrait3", "trait/LowFightingTechniqueTrait3", "LowFightingTechnique");
            SafeSetStat(LowFightingTechniqueTrait3.base_stats, strings.S.multiplier_speed, 0.04f);
            SafeSetStat(LowFightingTechniqueTrait3.base_stats, strings.S.multiplier_damage, 0.06f);
            SafeSetStat(LowFightingTechniqueTrait3.base_stats, strings.S.multiplier_health, 0.09f);
            SafeSetStat(LowFightingTechniqueTrait3.base_stats, strings.S.lifespan, 9f);
            LowFightingTechniqueTrait3.rate_inherit = 10;
            AssetManager.traits.add(LowFightingTechniqueTrait3);

            ActorTrait LowFightingTechniqueTrait4 = CreateTrait("LowFightingTechniqueTrait4", "trait/LowFightingTechniqueTrait4", "LowFightingTechnique");
            SafeSetStat(LowFightingTechniqueTrait4.base_stats, strings.S.multiplier_speed, 0.01f);
            SafeSetStat(LowFightingTechniqueTrait4.base_stats, strings.S.multiplier_damage, 0.08f);
            SafeSetStat(LowFightingTechniqueTrait4.base_stats, strings.S.multiplier_health, 0.06f);
            SafeSetStat(LowFightingTechniqueTrait4.base_stats, strings.S.lifespan, 6f);
            LowFightingTechniqueTrait4.rate_inherit = 10;
            AssetManager.traits.add(LowFightingTechniqueTrait4);

            ActorTrait LowFightingTechniqueTrait5 = CreateTrait("LowFightingTechniqueTrait5", "trait/LowFightingTechniqueTrait5", "LowFightingTechnique");
            SafeSetStat(LowFightingTechniqueTrait5.base_stats, strings.S.multiplier_speed, 0.03f);
            SafeSetStat(LowFightingTechniqueTrait5.base_stats, strings.S.multiplier_damage, 0.09f);
            SafeSetStat(LowFightingTechniqueTrait5.base_stats, strings.S.multiplier_health, 0.09f);
            SafeSetStat(LowFightingTechniqueTrait5.base_stats, strings.S.lifespan, 16f);
            LowFightingTechniqueTrait5.rate_inherit = 10;
            AssetManager.traits.add(LowFightingTechniqueTrait5);

            ActorTrait LowFightingTechniqueTrait6 = CreateTrait("LowFightingTechniqueTrait6", "trait/LowFightingTechniqueTrait6", "LowFightingTechnique");
            SafeSetStat(LowFightingTechniqueTrait6.base_stats, strings.S.multiplier_speed, 0.09f);
            SafeSetStat(LowFightingTechniqueTrait6.base_stats, strings.S.multiplier_damage, 0.09f);
            SafeSetStat(LowFightingTechniqueTrait6.base_stats, strings.S.multiplier_health, 0.09f);
            SafeSetStat(LowFightingTechniqueTrait6.base_stats, strings.S.lifespan, 10f);
            LowFightingTechniqueTrait6.rate_inherit = 10;
            AssetManager.traits.add(LowFightingTechniqueTrait6);

            ActorTrait LowFightingTechniqueTrait7 = CreateTrait("LowFightingTechniqueTrait7", "trait/LowFightingTechniqueTrait7", "LowFightingTechnique");
            SafeSetStat(LowFightingTechniqueTrait7.base_stats, strings.S.multiplier_speed, 0.06f);
            SafeSetStat(LowFightingTechniqueTrait7.base_stats, strings.S.multiplier_damage, 0.03f);
            SafeSetStat(LowFightingTechniqueTrait7.base_stats, strings.S.multiplier_health, 0.07f);
            SafeSetStat(LowFightingTechniqueTrait7.base_stats, strings.S.lifespan, 5f);
            LowFightingTechniqueTrait7.rate_inherit = 10;
            AssetManager.traits.add(LowFightingTechniqueTrait7);

            ActorTrait LowFightingTechniqueTrait8 = CreateTrait("LowFightingTechniqueTrait8", "trait/LowFightingTechniqueTrait8", "LowFightingTechnique");
            SafeSetStat(LowFightingTechniqueTrait8.base_stats, strings.S.multiplier_speed, 0.05f);
            SafeSetStat(LowFightingTechniqueTrait8.base_stats, strings.S.multiplier_damage, 0.05f);
            SafeSetStat(LowFightingTechniqueTrait8.base_stats, strings.S.multiplier_health, 0.05f);
            SafeSetStat(LowFightingTechniqueTrait8.base_stats, strings.S.lifespan, 12f);
            LowFightingTechniqueTrait8.rate_inherit = 10;
            AssetManager.traits.add(LowFightingTechniqueTrait8);

            ActorTrait LowFightingTechniqueTrait9 = CreateTrait("LowFightingTechniqueTrait9", "trait/LowFightingTechniqueTrait9", "LowFightingTechnique");
            SafeSetStat(LowFightingTechniqueTrait9.base_stats, strings.S.multiplier_speed, 0.08f);
            SafeSetStat(LowFightingTechniqueTrait9.base_stats, strings.S.multiplier_damage, 0.07f);
            SafeSetStat(LowFightingTechniqueTrait9.base_stats, strings.S.multiplier_health, 0.09f);
            SafeSetStat(LowFightingTechniqueTrait9.base_stats, strings.S.lifespan, 7f);
            LowFightingTechniqueTrait9.rate_inherit = 10;
            AssetManager.traits.add(LowFightingTechniqueTrait9);

            ActorTrait FightingTechniqueTrait1 = CreateTrait("FightingTechniqueTrait1", "trait/FightingTechniqueTrait1", "FightingTechnique");
            SafeSetStat(FightingTechniqueTrait1.base_stats, strings.S.multiplier_speed, 0.1f);
            SafeSetStat(FightingTechniqueTrait1.base_stats, strings.S.multiplier_damage, 0.2f);
            SafeSetStat(FightingTechniqueTrait1.base_stats, strings.S.multiplier_health, 0.1f);
            SafeSetStat(FightingTechniqueTrait1.base_stats, strings.S.lifespan, 10f);
            SafeSetStat(FightingTechniqueTrait1.base_stats, strings.S.stamina, 15f);
            SafeSetStat(FightingTechniqueTrait1.base_stats, strings.S.area_of_effect, 10f);
            FightingTechniqueTrait1.rate_inherit = 5;
            AssetManager.traits.add(FightingTechniqueTrait1);

            ActorTrait FightingTechniqueTrait12 = CreateTrait("FightingTechniqueTrait12", "trait/FightingTechniqueTrait12", "FightingTechnique");
            SafeSetStat(FightingTechniqueTrait12.base_stats, strings.S.multiplier_speed, 0.2f);
            SafeSetStat(FightingTechniqueTrait12.base_stats, strings.S.multiplier_damage, 0.1f);
            SafeSetStat(FightingTechniqueTrait12.base_stats, strings.S.multiplier_health, 0.1f);
            SafeSetStat(FightingTechniqueTrait12.base_stats, strings.S.lifespan, 12f);
            SafeSetStat(FightingTechniqueTrait12.base_stats, strings.S.stamina, 10f);
            SafeSetStat(FightingTechniqueTrait12.base_stats, strings.S.area_of_effect, 12f);
            FightingTechniqueTrait12.rate_inherit = 5;
            AssetManager.traits.add(FightingTechniqueTrait12);

            ActorTrait FightingTechniqueTrait2 = CreateTrait("FightingTechniqueTrait2", "trait/FightingTechniqueTrait2", "FightingTechnique");
            SafeSetStat(FightingTechniqueTrait2.base_stats, strings.S.multiplier_speed, 0.15f);
            SafeSetStat(FightingTechniqueTrait2.base_stats, strings.S.multiplier_damage, 0.1f);
            SafeSetStat(FightingTechniqueTrait2.base_stats, strings.S.multiplier_health, 0.2f);
            SafeSetStat(FightingTechniqueTrait2.base_stats, strings.S.lifespan, 15f);
            SafeSetStat(FightingTechniqueTrait2.base_stats, strings.S.stamina, 11f);
            SafeSetStat(FightingTechniqueTrait2.base_stats, strings.S.area_of_effect, 10f);
            FightingTechniqueTrait12.rate_inherit = 5;
            AssetManager.traits.add(FightingTechniqueTrait2);

            ActorTrait FightingTechniqueTrait3 = CreateTrait("FightingTechniqueTrait3", "trait/FightingTechniqueTrait3", "FightingTechnique");
            SafeSetStat(FightingTechniqueTrait3.base_stats, strings.S.multiplier_speed, 0.11f);
            SafeSetStat(FightingTechniqueTrait3.base_stats, strings.S.multiplier_damage, 0.09f);
            SafeSetStat(FightingTechniqueTrait3.base_stats, strings.S.multiplier_health, 0.2f);
            SafeSetStat(FightingTechniqueTrait3.base_stats, strings.S.lifespan, 11f);
            SafeSetStat(FightingTechniqueTrait3.base_stats, strings.S.stamina, 19f);
            SafeSetStat(FightingTechniqueTrait3.base_stats, strings.S.area_of_effect, 11f);
            FightingTechniqueTrait3.rate_inherit = 5;
            AssetManager.traits.add(FightingTechniqueTrait3);

            ActorTrait FightingTechniqueTrait4 = CreateTrait("FightingTechniqueTrait4", "trait/FightingTechniqueTrait4", "FightingTechnique");
            SafeSetStat(FightingTechniqueTrait4.base_stats, strings.S.multiplier_speed, 0.08f);
            SafeSetStat(FightingTechniqueTrait4.base_stats, strings.S.multiplier_damage, 0.09f);
            SafeSetStat(FightingTechniqueTrait4.base_stats, strings.S.multiplier_health, 0.19f);
            SafeSetStat(FightingTechniqueTrait4.base_stats, strings.S.lifespan, 10f);
            SafeSetStat(FightingTechniqueTrait4.base_stats, strings.S.stamina, 10f);
            SafeSetStat(FightingTechniqueTrait4.base_stats, strings.S.area_of_effect, 10f);
            FightingTechniqueTrait4.rate_inherit = 5;
            AssetManager.traits.add(FightingTechniqueTrait4);

            ActorTrait FightingTechniqueTrait5 = CreateTrait("FightingTechniqueTrait5", "trait/FightingTechniqueTrait5", "FightingTechnique");
            SafeSetStat(FightingTechniqueTrait5.base_stats, strings.S.multiplier_speed, 0.07f);
            SafeSetStat(FightingTechniqueTrait5.base_stats, strings.S.multiplier_damage, 0.18f);
            SafeSetStat(FightingTechniqueTrait5.base_stats, strings.S.multiplier_health, 0.1f);
            SafeSetStat(FightingTechniqueTrait5.base_stats, strings.S.lifespan, 18f);
            SafeSetStat(FightingTechniqueTrait5.base_stats, strings.S.stamina, 12f);
            SafeSetStat(FightingTechniqueTrait5.base_stats, strings.S.area_of_effect, 11f);
            FightingTechniqueTrait5.rate_inherit = 5;
            AssetManager.traits.add(FightingTechniqueTrait5);

            ActorTrait FightingTechniqueTrait6 = CreateTrait("FightingTechniqueTrait6", "trait/FightingTechniqueTrait6", "FightingTechnique");
            SafeSetStat(FightingTechniqueTrait6.base_stats, strings.S.multiplier_speed, 0.11f);
            SafeSetStat(FightingTechniqueTrait6.base_stats, strings.S.multiplier_damage, 0.11f);
            SafeSetStat(FightingTechniqueTrait6.base_stats, strings.S.multiplier_health, 0.15f);
            SafeSetStat(FightingTechniqueTrait6.base_stats, strings.S.lifespan, 15f);
            SafeSetStat(FightingTechniqueTrait6.base_stats, strings.S.stamina, 15f);
            SafeSetStat(FightingTechniqueTrait6.base_stats, strings.S.area_of_effect, 15f);
            FightingTechniqueTrait6.rate_inherit = 5;
            AssetManager.traits.add(FightingTechniqueTrait6);

            ActorTrait FightingTechniqueTrait7 = CreateTrait("FightingTechniqueTrait7", "trait/FightingTechniqueTrait7", "FightingTechnique");
            FightingTechniqueTrait7.rarity = Rarity.R3_Legendary;
            SafeSetStat(FightingTechniqueTrait7.base_stats, strings.S.multiplier_speed, 0.19f);
            SafeSetStat(FightingTechniqueTrait7.base_stats, strings.S.multiplier_damage, 0.19f);
            SafeSetStat(FightingTechniqueTrait7.base_stats, strings.S.multiplier_health, 0.19f);
            SafeSetStat(FightingTechniqueTrait7.base_stats, strings.S.lifespan, 19f);
            SafeSetStat(FightingTechniqueTrait7.base_stats, strings.S.stamina, 19f);
            SafeSetStat(FightingTechniqueTrait7.base_stats, strings.S.area_of_effect, 19f);
            FightingTechniqueTrait7.rate_inherit = 5;
            AssetManager.traits.add(FightingTechniqueTrait7);

            ActorTrait FightingTechniqueTrait8 = CreateTrait("FightingTechniqueTrait8", "trait/FightingTechniqueTrait8", "FightingTechnique");
            SafeSetStat(FightingTechniqueTrait8.base_stats, strings.S.multiplier_speed, 0.09f);
            SafeSetStat(FightingTechniqueTrait8.base_stats, strings.S.multiplier_damage, 0.13f);
            SafeSetStat(FightingTechniqueTrait8.base_stats, strings.S.multiplier_health, 0.17f);
            SafeSetStat(FightingTechniqueTrait8.base_stats, strings.S.lifespan, 14f);
            SafeSetStat(FightingTechniqueTrait8.base_stats, strings.S.stamina, 13f);
            SafeSetStat(FightingTechniqueTrait8.base_stats, strings.S.area_of_effect, 11f);
            FightingTechniqueTrait8.rate_inherit = 5;
            AssetManager.traits.add(FightingTechniqueTrait8);

            ActorTrait FightingTechniqueTrait9 = CreateTrait("FightingTechniqueTrait9", "trait/FightingTechniqueTrait9", "FightingTechnique");
            SafeSetStat(FightingTechniqueTrait9.base_stats, strings.S.multiplier_speed, 0.16f);
            SafeSetStat(FightingTechniqueTrait9.base_stats, strings.S.multiplier_damage, 0.14f);
            SafeSetStat(FightingTechniqueTrait9.base_stats, strings.S.multiplier_health, 0.12f);
            SafeSetStat(FightingTechniqueTrait9.base_stats, strings.S.lifespan, 14f);
            SafeSetStat(FightingTechniqueTrait9.base_stats, strings.S.stamina, 16f);
            SafeSetStat(FightingTechniqueTrait9.base_stats, strings.S.area_of_effect, 12f);
            FightingTechniqueTrait9.rate_inherit = 5;
            AssetManager.traits.add(FightingTechniqueTrait9);

            ActorTrait FightingTechniqueTrait10 = CreateTrait("FightingTechniqueTrait10", "trait/FightingTechniqueTrait10", "FightingTechnique");
            SafeSetStat(FightingTechniqueTrait10.base_stats, strings.S.multiplier_speed, 0.17f);
            SafeSetStat(FightingTechniqueTrait10.base_stats, strings.S.multiplier_damage, 0.13f);
            SafeSetStat(FightingTechniqueTrait10.base_stats, strings.S.multiplier_health, 0.11f);
            SafeSetStat(FightingTechniqueTrait10.base_stats, strings.S.lifespan, 14f);
            SafeSetStat(FightingTechniqueTrait10.base_stats, strings.S.stamina, 12f);
            SafeSetStat(FightingTechniqueTrait10.base_stats, strings.S.area_of_effect, 11f);
            FightingTechniqueTrait10.rate_inherit = 5;
            AssetManager.traits.add(FightingTechniqueTrait10);

            ActorTrait FightingTechniqueTrait11 = CreateTrait("FightingTechniqueTrait11", "trait/FightingTechniqueTrait11", "FightingTechnique");
            SafeSetStat(FightingTechniqueTrait11.base_stats, strings.S.multiplier_speed, 0.09f);
            SafeSetStat(FightingTechniqueTrait11.base_stats, strings.S.multiplier_damage, 0.09f);
            SafeSetStat(FightingTechniqueTrait11.base_stats, strings.S.multiplier_health, 0.3f);
            SafeSetStat(FightingTechniqueTrait11.base_stats, strings.S.lifespan, 24f);
            SafeSetStat(FightingTechniqueTrait11.base_stats, strings.S.stamina, 40f);
            SafeSetStat(FightingTechniqueTrait11.base_stats, strings.S.area_of_effect, 20f);
            FightingTechniqueTrait11.rate_inherit = 5;
            AssetManager.traits.add(FightingTechniqueTrait11);

            ActorTrait FightingTechniqueTrait13 = CreateTrait("FightingTechniqueTrait13", "trait/FightingTechniqueTrait13", "FightingTechnique");
            SafeSetStat(FightingTechniqueTrait13.base_stats, strings.S.multiplier_speed, 0.1f);
            SafeSetStat(FightingTechniqueTrait13.base_stats, strings.S.multiplier_damage, 0.3f);
            SafeSetStat(FightingTechniqueTrait13.base_stats, strings.S.multiplier_health, 0.1f);
            SafeSetStat(FightingTechniqueTrait13.base_stats, strings.S.lifespan, 15f);
            SafeSetStat(FightingTechniqueTrait13.base_stats, strings.S.stamina, 90f);
            SafeSetStat(FightingTechniqueTrait13.base_stats, strings.S.area_of_effect, 30f);
            FightingTechniqueTrait13.rate_inherit = 5;
            AssetManager.traits.add(FightingTechniqueTrait13);

            ActorTrait FightingTechniqueTrait14 = CreateTrait("FightingTechniqueTrait14", "trait/FightingTechniqueTrait14", "FightingTechnique");
            SafeSetStat(FightingTechniqueTrait14.base_stats, strings.S.multiplier_speed, 0.1f);
            SafeSetStat(FightingTechniqueTrait14.base_stats, strings.S.multiplier_damage, 0.1f);
            SafeSetStat(FightingTechniqueTrait14.base_stats, strings.S.multiplier_health, 0.1f);
            SafeSetStat(FightingTechniqueTrait14.base_stats, strings.S.multiplier_lifespan, -0.2f);
            SafeSetStat(FightingTechniqueTrait14.base_stats, strings.S.stamina, 45f);
            SafeSetStat(FightingTechniqueTrait14.base_stats, strings.S.area_of_effect, 15f);
            FightingTechniqueTrait14.rate_inherit = 5;
            AssetManager.traits.add(FightingTechniqueTrait14);

            ActorTrait DivineSeal = CreateTrait("DivineSeal", "trait/DivineSeal", "ChivalricFoundations");
            AssetManager.traits.add(DivineSeal);

            // 定义五个等级的血脉特质
            ActorTrait KnightlyBloodline1 = CreateTrait("KnightlyBloodline1", "trait/KnightlyBloodline1", "KnightlyBloodline");
            KnightlyBloodline1.rate_inherit = 10;
            SafeSetStat(KnightlyBloodline1.base_stats, strings.S.multiplier_health, 0.01f);
            SafeSetStat(KnightlyBloodline1.base_stats, strings.S.multiplier_damage, 0.01f);
            SafeSetStat(KnightlyBloodline1.base_stats, strings.S.lifespan, 5f);
            AssetManager.traits.add(KnightlyBloodline1);

            ActorTrait KnightlyBloodline2 = CreateTrait("KnightlyBloodline2", "trait/KnightlyBloodline2", "KnightlyBloodline");
            KnightlyBloodline2.rate_inherit = 9;
            SafeSetStat(KnightlyBloodline2.base_stats, strings.S.multiplier_health, 0.05f);
            SafeSetStat(KnightlyBloodline2.base_stats, strings.S.multiplier_damage, 0.05f);
            SafeSetStat(KnightlyBloodline2.base_stats, strings.S.lifespan, 10f);
            AssetManager.traits.add(KnightlyBloodline2);

            ActorTrait KnightlyBloodline3 = CreateTrait("KnightlyBloodline3", "trait/KnightlyBloodline3", "KnightlyBloodline");
            KnightlyBloodline3.rate_inherit = 8; 
            SafeSetStat(KnightlyBloodline3.base_stats, strings.S.multiplier_health, 0.1f);
            SafeSetStat(KnightlyBloodline3.base_stats, strings.S.multiplier_damage, 0.1f);
            SafeSetStat(KnightlyBloodline3.base_stats, strings.S.lifespan, 15f);
            AssetManager.traits.add(KnightlyBloodline3);

            ActorTrait KnightlyBloodline4 = CreateTrait("KnightlyBloodline4", "trait/KnightlyBloodline4", "KnightlyBloodline");
            KnightlyBloodline4.rarity = Rarity.R2_Epic;
            KnightlyBloodline4.rate_inherit = 7;
            SafeSetStat(KnightlyBloodline4.base_stats, strings.S.multiplier_health, 0.15f);
            SafeSetStat(KnightlyBloodline4.base_stats, strings.S.multiplier_damage, 0.15f);
            SafeSetStat(KnightlyBloodline4.base_stats, strings.S.lifespan, 20f);
            AssetManager.traits.add(KnightlyBloodline4);

            ActorTrait KnightlyBloodline5 = CreateTrait("KnightlyBloodline5", "trait/KnightlyBloodline5", "KnightlyBloodline");
            KnightlyBloodline5.rarity = Rarity.R2_Epic;
            KnightlyBloodline5.rate_inherit = 6; 
            SafeSetStat(KnightlyBloodline5.base_stats, strings.S.multiplier_health, 0.2f);
            SafeSetStat(KnightlyBloodline5.base_stats, strings.S.multiplier_damage, 0.2f);
            SafeSetStat(KnightlyBloodline5.base_stats, strings.S.lifespan, 30f);
            AssetManager.traits.add(KnightlyBloodline5);

            ActorTrait KnightlyBloodline6 = CreateTrait("KnightlyBloodline6", "trait/KnightlyBloodline6", "KnightlyBloodline");
            KnightlyBloodline6.rarity = Rarity.R3_Legendary;
            KnightlyBloodline6.rate_inherit = 5; 
            SafeSetStat(KnightlyBloodline6.base_stats, strings.S.multiplier_health, 0.25f);
            SafeSetStat(KnightlyBloodline6.base_stats, strings.S.multiplier_damage, 0.25f);
            SafeSetStat(KnightlyBloodline6.base_stats, strings.S.lifespan, 50f);
            AssetManager.traits.add(KnightlyBloodline6);

            ActorTrait KnightlyBloodline7 = CreateTrait("KnightlyBloodline7", "trait/KnightlyBloodline7", "KnightlyBloodline");
            KnightlyBloodline7.rarity = Rarity.R3_Legendary;
            KnightlyBloodline7.rate_inherit = 1; 
            SafeSetStat(KnightlyBloodline7.base_stats, strings.S.multiplier_health, 0.3f);
            SafeSetStat(KnightlyBloodline7.base_stats, strings.S.multiplier_damage, 0.3f);
            SafeSetStat(KnightlyBloodline7.base_stats, strings.S.lifespan, 100f);
            AssetManager.traits.add(KnightlyBloodline7);

            ActorTrait midFightingTechniqueTrait1 = CreateTrait("MidFightingTechniqueTrait1", "trait/MidFightingTechniqueTrait1", "MidFightingTechnique");
            SafeSetStat(midFightingTechniqueTrait1.base_stats, strings.S.multiplier_speed, 0.04f);
            SafeSetStat(midFightingTechniqueTrait1.base_stats, strings.S.multiplier_damage, 0.05f);
            SafeSetStat(midFightingTechniqueTrait1.base_stats, strings.S.multiplier_health, 0.06f);
            SafeSetStat(midFightingTechniqueTrait1.base_stats, strings.S.lifespan, 8f);
            SafeSetStat(midFightingTechniqueTrait1.base_stats, strings.S.stamina, 10f);
            SafeSetStat(midFightingTechniqueTrait1.base_stats, strings.S.area_of_effect, 10f);
            SafeSetStat(midFightingTechniqueTrait1.base_stats, strings.S.skill_combat, 1f);
            SafeSetStat(midFightingTechniqueTrait1.base_stats, strings.S.warfare, 6f);
            SafeSetStat(midFightingTechniqueTrait1.base_stats, strings.S.diplomacy, 4f);
            SafeSetStat(midFightingTechniqueTrait1.base_stats, strings.S.stewardship, 4f);
            SafeSetStat(midFightingTechniqueTrait1.base_stats, strings.S.opinion, 4f);
            SafeSetStat(midFightingTechniqueTrait1.base_stats, strings.S.cities, 5f);
            midFightingTechniqueTrait1.rate_inherit = 5;
            AssetManager.traits.add(midFightingTechniqueTrait1);

            ActorTrait midFightingTechniqueTrait12 = CreateTrait("MidFightingTechniqueTrait12", "trait/MidFightingTechniqueTrait12", "MidFightingTechnique");
            SafeSetStat(midFightingTechniqueTrait12.base_stats, strings.S.multiplier_speed, 0.01f);
            SafeSetStat(midFightingTechniqueTrait12.base_stats, strings.S.multiplier_damage, 0.09f);
            SafeSetStat(midFightingTechniqueTrait12.base_stats, strings.S.multiplier_health, 0.04f);
            SafeSetStat(midFightingTechniqueTrait12.base_stats, strings.S.lifespan, 5f);
            SafeSetStat(midFightingTechniqueTrait12.base_stats, strings.S.stamina, 12f);
            SafeSetStat(midFightingTechniqueTrait12.base_stats, strings.S.area_of_effect, 12f);
            SafeSetStat(midFightingTechniqueTrait12.base_stats, strings.S.skill_combat, 0.1f);
            SafeSetStat(midFightingTechniqueTrait12.base_stats, strings.S.warfare, 9f);
            SafeSetStat(midFightingTechniqueTrait12.base_stats, strings.S.diplomacy, 4f);
            SafeSetStat(midFightingTechniqueTrait12.base_stats, strings.S.stewardship, 8f);
            SafeSetStat(midFightingTechniqueTrait12.base_stats, strings.S.opinion, 7f);
            SafeSetStat(midFightingTechniqueTrait12.base_stats, strings.S.cities, 8f);
            midFightingTechniqueTrait12.rate_inherit = 5;
            AssetManager.traits.add(midFightingTechniqueTrait12);

            ActorTrait midFightingTechniqueTrait13 = CreateTrait("MidFightingTechniqueTrait13", "trait/MidFightingTechniqueTrait13", "MidFightingTechnique");
            SafeSetStat(midFightingTechniqueTrait13.base_stats, strings.S.multiplier_speed, 0.06f);
            SafeSetStat(midFightingTechniqueTrait13.base_stats, strings.S.multiplier_damage, 0.06f);
            SafeSetStat(midFightingTechniqueTrait13.base_stats, strings.S.multiplier_health, 0.06f);
            SafeSetStat(midFightingTechniqueTrait13.base_stats, strings.S.lifespan, 6f);
            SafeSetStat(midFightingTechniqueTrait13.base_stats, strings.S.stamina, 60f);
            SafeSetStat(midFightingTechniqueTrait13.base_stats, strings.S.area_of_effect, 6f);
            SafeSetStat(midFightingTechniqueTrait13.base_stats, strings.S.skill_combat, 0.6f);
            SafeSetStat(midFightingTechniqueTrait13.base_stats, strings.S.warfare, 6f);
            SafeSetStat(midFightingTechniqueTrait13.base_stats, strings.S.diplomacy, 6f);
            SafeSetStat(midFightingTechniqueTrait13.base_stats, strings.S.stewardship, 6f);
            SafeSetStat(midFightingTechniqueTrait13.base_stats, strings.S.opinion, 6f);
            SafeSetStat(midFightingTechniqueTrait13.base_stats, strings.S.cities, 6f);
            midFightingTechniqueTrait13.rate_inherit = 5;
            AssetManager.traits.add(midFightingTechniqueTrait13);

            ActorTrait midFightingTechniqueTrait2 = CreateTrait("MidFightingTechniqueTrait2", "trait/MidFightingTechniqueTrait2", "MidFightingTechnique");
            SafeSetStat(midFightingTechniqueTrait2.base_stats, strings.S.multiplier_speed, 0.08f);
            SafeSetStat(midFightingTechniqueTrait2.base_stats, strings.S.multiplier_damage, 0.1f);
            SafeSetStat(midFightingTechniqueTrait2.base_stats, strings.S.multiplier_health, 0.1f);
            SafeSetStat(midFightingTechniqueTrait2.base_stats, strings.S.lifespan, 15f);
            SafeSetStat(midFightingTechniqueTrait2.base_stats, strings.S.stamina, 20f);
            SafeSetStat(midFightingTechniqueTrait2.base_stats, strings.S.area_of_effect, 20f);
            SafeSetStat(midFightingTechniqueTrait2.base_stats, strings.S.skill_combat, 0.4f);
            SafeSetStat(midFightingTechniqueTrait2.base_stats, strings.S.warfare, 8f);
            SafeSetStat(midFightingTechniqueTrait2.base_stats, strings.S.diplomacy, 4f);
            SafeSetStat(midFightingTechniqueTrait2.base_stats, strings.S.stewardship, 2f);
            SafeSetStat(midFightingTechniqueTrait2.base_stats, strings.S.opinion, 4f);
            SafeSetStat(midFightingTechniqueTrait2.base_stats, strings.S.cities, 2f);
            midFightingTechniqueTrait2.rate_inherit = 5;
            AssetManager.traits.add(midFightingTechniqueTrait2);

            ActorTrait midFightingTechniqueTrait3 = CreateTrait("MidFightingTechniqueTrait3", "trait/MidFightingTechniqueTrait3", "MidFightingTechnique");
            SafeSetStat(midFightingTechniqueTrait3.base_stats, strings.S.multiplier_speed, 0.08f);
            SafeSetStat(midFightingTechniqueTrait3.base_stats, strings.S.multiplier_damage, 0.05f);
            SafeSetStat(midFightingTechniqueTrait3.base_stats, strings.S.multiplier_health, 0.2f);
            SafeSetStat(midFightingTechniqueTrait3.base_stats, strings.S.lifespan, 16f);
            SafeSetStat(midFightingTechniqueTrait3.base_stats, strings.S.stamina, 12f);
            SafeSetStat(midFightingTechniqueTrait3.base_stats, strings.S.area_of_effect, 18f);
            SafeSetStat(midFightingTechniqueTrait3.base_stats, strings.S.skill_combat, 1f);
            SafeSetStat(midFightingTechniqueTrait3.base_stats, strings.S.warfare, 10f);
            SafeSetStat(midFightingTechniqueTrait3.base_stats, strings.S.diplomacy, 4f);
            SafeSetStat(midFightingTechniqueTrait3.base_stats, strings.S.stewardship, 2f);
            SafeSetStat(midFightingTechniqueTrait3.base_stats, strings.S.opinion, 8f);
            SafeSetStat(midFightingTechniqueTrait3.base_stats, strings.S.cities, 2f);
            midFightingTechniqueTrait3.rate_inherit = 5;
            AssetManager.traits.add(midFightingTechniqueTrait3);

            ActorTrait midFightingTechniqueTrait4 = CreateTrait("MidFightingTechniqueTrait4", "trait/MidFightingTechniqueTrait4", "MidFightingTechnique");
            SafeSetStat(midFightingTechniqueTrait4.base_stats, strings.S.multiplier_speed, 0.3f);
            SafeSetStat(midFightingTechniqueTrait4.base_stats, strings.S.multiplier_damage, 0.04f);
            SafeSetStat(midFightingTechniqueTrait4.base_stats, strings.S.multiplier_health, 0.08f);
            SafeSetStat(midFightingTechniqueTrait4.base_stats, strings.S.lifespan, 6f);
            SafeSetStat(midFightingTechniqueTrait4.base_stats, strings.S.stamina, 10f);
            SafeSetStat(midFightingTechniqueTrait4.base_stats, strings.S.area_of_effect, 16f);
            SafeSetStat(midFightingTechniqueTrait4.base_stats, strings.S.skill_combat, 0.8f);
            SafeSetStat(midFightingTechniqueTrait4.base_stats, strings.S.warfare, 2f);
            SafeSetStat(midFightingTechniqueTrait4.base_stats, strings.S.diplomacy, 8f);
            SafeSetStat(midFightingTechniqueTrait4.base_stats, strings.S.stewardship, 4f);
            SafeSetStat(midFightingTechniqueTrait4.base_stats, strings.S.opinion, 8f);
            SafeSetStat(midFightingTechniqueTrait4.base_stats, strings.S.cities, 2f);
            midFightingTechniqueTrait4.rate_inherit = 5;
            AssetManager.traits.add(midFightingTechniqueTrait4);

            ActorTrait midFightingTechniqueTrait5 = CreateTrait("MidFightingTechniqueTrait5", "trait/MidFightingTechniqueTrait5", "MidFightingTechnique");
            SafeSetStat(midFightingTechniqueTrait5.base_stats, strings.S.multiplier_speed, 0.03f);
            SafeSetStat(midFightingTechniqueTrait5.base_stats, strings.S.multiplier_damage, 0.3f);
            SafeSetStat(midFightingTechniqueTrait5.base_stats, strings.S.multiplier_health, 0.04f);
            SafeSetStat(midFightingTechniqueTrait5.base_stats, strings.S.lifespan, 10f);
            SafeSetStat(midFightingTechniqueTrait5.base_stats, strings.S.stamina, 50f);
            SafeSetStat(midFightingTechniqueTrait5.base_stats, strings.S.area_of_effect, 20f);
            SafeSetStat(midFightingTechniqueTrait5.base_stats, strings.S.skill_combat, 1f);
            SafeSetStat(midFightingTechniqueTrait5.base_stats, strings.S.warfare, 10f);
            SafeSetStat(midFightingTechniqueTrait5.base_stats, strings.S.diplomacy, 10f);
            SafeSetStat(midFightingTechniqueTrait5.base_stats, strings.S.stewardship, 2f);
            SafeSetStat(midFightingTechniqueTrait5.base_stats, strings.S.opinion, 2f);
            SafeSetStat(midFightingTechniqueTrait5.base_stats, strings.S.cities, 2f);
            midFightingTechniqueTrait5.rate_inherit = 5;
            AssetManager.traits.add(midFightingTechniqueTrait5);

            ActorTrait midFightingTechniqueTrait6 = CreateTrait("MidFightingTechniqueTrait6", "trait/MidFightingTechniqueTrait6", "MidFightingTechnique");
            SafeSetStat(midFightingTechniqueTrait6.base_stats, strings.S.multiplier_speed, 0.1f);
            SafeSetStat(midFightingTechniqueTrait6.base_stats, strings.S.multiplier_damage, 0.06f);
            SafeSetStat(midFightingTechniqueTrait6.base_stats, strings.S.multiplier_health, 0.04f);
            SafeSetStat(midFightingTechniqueTrait6.base_stats, strings.S.lifespan, 20f);
            SafeSetStat(midFightingTechniqueTrait6.base_stats, strings.S.stamina, 110f);
            SafeSetStat(midFightingTechniqueTrait6.base_stats, strings.S.area_of_effect, 20f);
            SafeSetStat(midFightingTechniqueTrait6.base_stats, strings.S.skill_combat, 1f);
            SafeSetStat(midFightingTechniqueTrait6.base_stats, strings.S.warfare, 6f);
            SafeSetStat(midFightingTechniqueTrait6.base_stats, strings.S.diplomacy, 8f);
            SafeSetStat(midFightingTechniqueTrait6.base_stats, strings.S.stewardship, 2f);
            SafeSetStat(midFightingTechniqueTrait6.base_stats, strings.S.opinion, 8f);
            SafeSetStat(midFightingTechniqueTrait6.base_stats, strings.S.cities, 2f);
            midFightingTechniqueTrait6.rate_inherit = 5;
            AssetManager.traits.add(midFightingTechniqueTrait6);

            ActorTrait midFightingTechniqueTrait7 = CreateTrait("MidFightingTechniqueTrait7", "trait/MidFightingTechniqueTrait7", "MidFightingTechnique");
            SafeSetStat(midFightingTechniqueTrait7.base_stats, strings.S.multiplier_speed, 0.09f);
            SafeSetStat(midFightingTechniqueTrait7.base_stats, strings.S.multiplier_damage, 0.09f);
            SafeSetStat(midFightingTechniqueTrait7.base_stats, strings.S.multiplier_health, 0.09f);
            SafeSetStat(midFightingTechniqueTrait7.base_stats, strings.S.lifespan, 4f);
            SafeSetStat(midFightingTechniqueTrait7.base_stats, strings.S.stamina, 60f);
            SafeSetStat(midFightingTechniqueTrait7.base_stats, strings.S.area_of_effect, 18f);
            SafeSetStat(midFightingTechniqueTrait7.base_stats, strings.S.skill_combat, 0.4f);
            SafeSetStat(midFightingTechniqueTrait7.base_stats, strings.S.warfare, 2f);
            SafeSetStat(midFightingTechniqueTrait7.base_stats, strings.S.diplomacy, 4f);
            SafeSetStat(midFightingTechniqueTrait7.base_stats, strings.S.stewardship, 4f);
            SafeSetStat(midFightingTechniqueTrait7.base_stats, strings.S.opinion, 8f);
            SafeSetStat(midFightingTechniqueTrait7.base_stats, strings.S.cities, 10f);
            midFightingTechniqueTrait7.rate_inherit = 5;
            AssetManager.traits.add(midFightingTechniqueTrait7);

            ActorTrait midFightingTechniqueTrait8 = CreateTrait("MidFightingTechniqueTrait8", "trait/MidFightingTechniqueTrait8", "MidFightingTechnique");
            SafeSetStat(midFightingTechniqueTrait8.base_stats, strings.S.multiplier_speed, 0.1f);
            SafeSetStat(midFightingTechniqueTrait8.base_stats, strings.S.multiplier_damage, 0.2f);
            SafeSetStat(midFightingTechniqueTrait8.base_stats, strings.S.multiplier_health, 0.06f);
            SafeSetStat(midFightingTechniqueTrait8.base_stats, strings.S.lifespan, 20f);
            SafeSetStat(midFightingTechniqueTrait8.base_stats, strings.S.stamina, 40f);
            SafeSetStat(midFightingTechniqueTrait8.base_stats, strings.S.area_of_effect, 40f);
            SafeSetStat(midFightingTechniqueTrait8.base_stats, strings.S.skill_combat, 2f);
            SafeSetStat(midFightingTechniqueTrait8.base_stats, strings.S.warfare, 10f);
            SafeSetStat(midFightingTechniqueTrait8.base_stats, strings.S.diplomacy, 2f);
            SafeSetStat(midFightingTechniqueTrait8.base_stats, strings.S.stewardship, 6f);
            SafeSetStat(midFightingTechniqueTrait8.base_stats, strings.S.opinion, 8f);
            SafeSetStat(midFightingTechniqueTrait8.base_stats, strings.S.cities, 6f);
            midFightingTechniqueTrait8.rate_inherit = 5;
            AssetManager.traits.add(midFightingTechniqueTrait8);

            ActorTrait midFightingTechniqueTrait9 = CreateTrait("MidFightingTechniqueTrait9", "trait/MidFightingTechniqueTrait9", "MidFightingTechnique");
            SafeSetStat(midFightingTechniqueTrait9.base_stats, strings.S.multiplier_speed, 0.01f);
            SafeSetStat(midFightingTechniqueTrait9.base_stats, strings.S.multiplier_damage, 0.25f);
            SafeSetStat(midFightingTechniqueTrait9.base_stats, strings.S.multiplier_health, 0.01f);
            SafeSetStat(midFightingTechniqueTrait9.base_stats, strings.S.lifespan, -10f);
            SafeSetStat(midFightingTechniqueTrait9.base_stats, strings.S.stamina, 10f);
            SafeSetStat(midFightingTechniqueTrait9.base_stats, strings.S.area_of_effect, 1f);
            SafeSetStat(midFightingTechniqueTrait9.base_stats, strings.S.skill_combat, 0.1f);
            SafeSetStat(midFightingTechniqueTrait9.base_stats, strings.S.warfare, 12f);
            SafeSetStat(midFightingTechniqueTrait9.base_stats, strings.S.diplomacy, -1f);
            SafeSetStat(midFightingTechniqueTrait9.base_stats, strings.S.stewardship, -1f);
            SafeSetStat(midFightingTechniqueTrait9.base_stats, strings.S.opinion, -1f);
            SafeSetStat(midFightingTechniqueTrait9.base_stats, strings.S.cities, -1f);
            midFightingTechniqueTrait9.rate_inherit = 5;
            AssetManager.traits.add(midFightingTechniqueTrait9);

            ActorTrait midFightingTechniqueTrait10 = CreateTrait("MidFightingTechniqueTrait10", "trait/MidFightingTechniqueTrait10", "MidFightingTechnique");
            SafeSetStat(midFightingTechniqueTrait10.base_stats, strings.S.multiplier_speed, 0.01f);
            SafeSetStat(midFightingTechniqueTrait10.base_stats, strings.S.multiplier_damage, 0.01f);
            SafeSetStat(midFightingTechniqueTrait10.base_stats, strings.S.multiplier_health, 0.2f);
            SafeSetStat(midFightingTechniqueTrait10.base_stats, strings.S.lifespan, 80f);
            SafeSetStat(midFightingTechniqueTrait10.base_stats, strings.S.stamina, 20f);
            SafeSetStat(midFightingTechniqueTrait10.base_stats, strings.S.area_of_effect, 11f);
            SafeSetStat(midFightingTechniqueTrait10.base_stats, strings.S.skill_combat, 0.1f);
            SafeSetStat(midFightingTechniqueTrait10.base_stats, strings.S.warfare, 2f);
            SafeSetStat(midFightingTechniqueTrait10.base_stats, strings.S.diplomacy, 4f);
            SafeSetStat(midFightingTechniqueTrait10.base_stats, strings.S.stewardship, 2f);
            SafeSetStat(midFightingTechniqueTrait10.base_stats, strings.S.opinion, -2f);
            SafeSetStat(midFightingTechniqueTrait10.base_stats, strings.S.cities, -2f);
            midFightingTechniqueTrait10.rate_inherit = 5;
            AssetManager.traits.add(midFightingTechniqueTrait10);

            ActorTrait midFightingTechniqueTrait11 = CreateTrait("MidFightingTechniqueTrait11", "trait/MidFightingTechniqueTrait11", "MidFightingTechnique");
            SafeSetStat(midFightingTechniqueTrait11.base_stats, strings.S.multiplier_speed, 0.1f);
            SafeSetStat(midFightingTechniqueTrait11.base_stats, strings.S.multiplier_damage, 0.1f);
            SafeSetStat(midFightingTechniqueTrait11.base_stats, strings.S.multiplier_health, 0.1f);
            SafeSetStat(midFightingTechniqueTrait11.base_stats, strings.S.lifespan, 8f);
            SafeSetStat(midFightingTechniqueTrait11.base_stats, strings.S.stamina, 30f);
            SafeSetStat(midFightingTechniqueTrait11.base_stats, strings.S.area_of_effect, 40f);
            SafeSetStat(midFightingTechniqueTrait11.base_stats, strings.S.skill_combat, 1f);
            SafeSetStat(midFightingTechniqueTrait11.base_stats, strings.S.warfare, 4f);
            SafeSetStat(midFightingTechniqueTrait11.base_stats, strings.S.diplomacy, 6f);
            SafeSetStat(midFightingTechniqueTrait11.base_stats, strings.S.stewardship, 8f);
            SafeSetStat(midFightingTechniqueTrait11.base_stats, strings.S.opinion, 2f);
            SafeSetStat(midFightingTechniqueTrait11.base_stats, strings.S.cities, 5f);
            midFightingTechniqueTrait11.rate_inherit = 5;
            AssetManager.traits.add(midFightingTechniqueTrait11);

            ActorTrait midFightingTechniqueTrait14 = CreateTrait("MidFightingTechniqueTrait14", "trait/MidFightingTechniqueTrait14", "MidFightingTechnique");
            SafeSetStat(midFightingTechniqueTrait14.base_stats, strings.S.multiplier_speed, 0.1f);
            SafeSetStat(midFightingTechniqueTrait14.base_stats, strings.S.multiplier_damage, 0.1f);
            SafeSetStat(midFightingTechniqueTrait14.base_stats, strings.S.multiplier_health, 0.1f);
            SafeSetStat(midFightingTechniqueTrait14.base_stats, strings.S.multiplier_lifespan, -0.1f);
            SafeSetStat(midFightingTechniqueTrait14.base_stats, strings.S.stamina, 30f);
            SafeSetStat(midFightingTechniqueTrait14.base_stats, strings.S.area_of_effect, 40f);
            SafeSetStat(midFightingTechniqueTrait14.base_stats, strings.S.skill_combat, 1f);
            SafeSetStat(midFightingTechniqueTrait14.base_stats, strings.S.warfare, -2f);
            SafeSetStat(midFightingTechniqueTrait14.base_stats, strings.S.diplomacy, -3f);
            SafeSetStat(midFightingTechniqueTrait14.base_stats, strings.S.stewardship, -4f);
            SafeSetStat(midFightingTechniqueTrait14.base_stats, strings.S.opinion, -1f);
            SafeSetStat(midFightingTechniqueTrait14.base_stats, strings.S.cities, -2f);
            midFightingTechniqueTrait14.rate_inherit = 5;
            AssetManager.traits.add(midFightingTechniqueTrait14);

            ActorTrait GodKingdom1 = CreateTrait("GodKingdom1", "trait/GodKingdom1", "GodKingdom");
            GodKingdom1.rarity = Rarity.R3_Legendary;
            SafeSetStat(GodKingdom1.base_stats, strings.S.multiplier_damage, 0.4f);
            SafeSetStat(GodKingdom1.base_stats, strings.S.multiplier_health, 0.4f);
            SafeSetStat(GodKingdom1.base_stats, strings.S.intelligence, 100f);
            SafeSetStat(GodKingdom1.base_stats, strings.S.lifespan, 180f);
            SafeSetStat(GodKingdom1.base_stats, strings.S.stamina, 300f);
            SafeSetStat(GodKingdom1.base_stats, strings.S.area_of_effect, 8f);
            SafeSetStat(GodKingdom1.base_stats, strings.S.critical_chance, 0.2f);
            SafeSetStat(GodKingdom1.base_stats, strings.S.targets, 10f);
            SafeSetStat(GodKingdom1.base_stats, strings.S.accuracy, 60f);
            SafeSetStat(GodKingdom1.base_stats, "Accuracy", 50f);
            SafeSetStat(GodKingdom1.base_stats, "Dodge", 50f);
            AssetManager.traits.add(GodKingdom1);

            ActorTrait GodKingdom2 = CreateTrait("GodKingdom2", "trait/GodKingdom2", "GodKingdom");
            GodKingdom2.rarity = Rarity.R3_Legendary;
            SafeSetStat(GodKingdom2.base_stats, strings.S.multiplier_damage, 0.35f);
            SafeSetStat(GodKingdom2.base_stats, strings.S.multiplier_health, 0.45f);
            SafeSetStat(GodKingdom2.base_stats, strings.S.intelligence, 40f);
            SafeSetStat(GodKingdom2.base_stats, strings.S.lifespan, 120f);
            SafeSetStat(GodKingdom2.base_stats, strings.S.stamina, 200f);
            SafeSetStat(GodKingdom2.base_stats, strings.S.area_of_effect, 18f);
            SafeSetStat(GodKingdom2.base_stats, strings.S.critical_chance, 0.8f);
            SafeSetStat(GodKingdom2.base_stats, strings.S.targets, 15f);
            SafeSetStat(GodKingdom2.base_stats, strings.S.accuracy, 40f);
            SafeSetStat(GodKingdom2.base_stats, "Accuracy", 40f);
            SafeSetStat(GodKingdom2.base_stats, "Dodge", 60f);
            AssetManager.traits.add(GodKingdom2);

            ActorTrait GodKingdom3 = CreateTrait("GodKingdom3", "trait/GodKingdom3", "GodKingdom");
            GodKingdom3.rarity = Rarity.R3_Legendary;
            SafeSetStat(GodKingdom3.base_stats, strings.S.multiplier_damage, 0.3f);
            SafeSetStat(GodKingdom3.base_stats, strings.S.multiplier_health, 0.2f);
            SafeSetStat(GodKingdom3.base_stats, strings.S.intelligence, 120f);
            SafeSetStat(GodKingdom3.base_stats, strings.S.lifespan, 150f);
            SafeSetStat(GodKingdom3.base_stats, strings.S.stamina, 240f);
            SafeSetStat(GodKingdom3.base_stats, strings.S.area_of_effect, 6f);
            SafeSetStat(GodKingdom3.base_stats, strings.S.critical_chance, 0.5f);
            SafeSetStat(GodKingdom3.base_stats, strings.S.targets, 8f);
            SafeSetStat(GodKingdom3.base_stats, strings.S.accuracy, 50f);
            SafeSetStat(GodKingdom3.base_stats, "Accuracy", 60f);
            SafeSetStat(GodKingdom3.base_stats, "Dodge", 40f);
            AssetManager.traits.add(GodKingdom3);

            ActorTrait GodKingdom4 = CreateTrait("GodKingdom4", "trait/GodKingdom4", "GodKingdom");
            GodKingdom4.rarity = Rarity.R3_Legendary;
            SafeSetStat(GodKingdom4.base_stats, strings.S.multiplier_damage, 0.36f);
            SafeSetStat(GodKingdom4.base_stats, strings.S.multiplier_health, 0.18f);
            SafeSetStat(GodKingdom4.base_stats, strings.S.intelligence, 60f);
            SafeSetStat(GodKingdom4.base_stats, strings.S.lifespan, 140f);
            SafeSetStat(GodKingdom4.base_stats, strings.S.stamina, 240f);
            SafeSetStat(GodKingdom4.base_stats, strings.S.area_of_effect, 6f);
            SafeSetStat(GodKingdom4.base_stats, strings.S.critical_chance, 0.5f);
            SafeSetStat(GodKingdom4.base_stats, strings.S.targets, 15f);
            SafeSetStat(GodKingdom4.base_stats, strings.S.accuracy, 60f);
            SafeSetStat(GodKingdom4.base_stats, "Accuracy", 70f);
            SafeSetStat(GodKingdom4.base_stats, "Dodge", 30f);
            AssetManager.traits.add(GodKingdom4);

            ActorTrait GodKingdom5 = CreateTrait("GodKingdom5", "trait/GodKingdom5", "GodKingdom");
            GodKingdom5.rarity = Rarity.R3_Legendary;
            SafeSetStat(GodKingdom5.base_stats, strings.S.multiplier_damage, 0.49f);
            SafeSetStat(GodKingdom5.base_stats, strings.S.multiplier_health, 0.1f);
            SafeSetStat(GodKingdom5.base_stats, strings.S.intelligence, 260f);
            SafeSetStat(GodKingdom5.base_stats, strings.S.lifespan, 140f);
            SafeSetStat(GodKingdom5.base_stats, strings.S.stamina, 40f);
            SafeSetStat(GodKingdom5.base_stats, strings.S.area_of_effect, 2f);
            SafeSetStat(GodKingdom5.base_stats, strings.S.critical_chance, 0.1f);
            SafeSetStat(GodKingdom5.base_stats, strings.S.targets, 10f);
            SafeSetStat(GodKingdom5.base_stats, strings.S.accuracy, 20f);
            SafeSetStat(GodKingdom5.base_stats, strings.S.targets, 20f);
            SafeSetStat(GodKingdom5.base_stats, strings.S.accuracy, 45f);
            SafeSetStat(GodKingdom5.base_stats, "Accuracy", 80f);
            SafeSetStat(GodKingdom5.base_stats, "Dodge", 20f);
            AssetManager.traits.add(GodKingdom5);

            ActorTrait GodKingdom6 = CreateTrait("GodKingdom6", "trait/GodKingdom6", "GodKingdom");
            GodKingdom6.rarity = Rarity.R3_Legendary;
            SafeSetStat(GodKingdom6.base_stats, strings.S.multiplier_health, 0.3f);
            SafeSetStat(GodKingdom6.base_stats, strings.S.multiplier_damage, 0.1f);
            SafeSetStat(GodKingdom6.base_stats, strings.S.intelligence, 20f);
            SafeSetStat(GodKingdom6.base_stats, strings.S.lifespan, 200f);
            SafeSetStat(GodKingdom6.base_stats, strings.S.stamina, 240f);
            SafeSetStat(GodKingdom6.base_stats, strings.S.area_of_effect, 12f);
            SafeSetStat(GodKingdom6.base_stats, strings.S.critical_chance, 0.4f);
            SafeSetStat(GodKingdom6.base_stats, strings.S.targets, 15f);
            SafeSetStat(GodKingdom6.base_stats, strings.S.accuracy, 60f);
            SafeSetStat(GodKingdom6.base_stats, "Accuracy", 90f);
            SafeSetStat(GodKingdom6.base_stats, "Dodge", 10f);
            AssetManager.traits.add(GodKingdom6);

            ActorTrait GodKingdom7 = CreateTrait("GodKingdom7", "trait/GodKingdom7", "GodKingdom");
            GodKingdom7.rarity = Rarity.R3_Legendary;
            SafeSetStat(GodKingdom7.base_stats, strings.S.multiplier_health, 0.18f);
            SafeSetStat(GodKingdom7.base_stats, strings.S.multiplier_damage, 0.24f);
            SafeSetStat(GodKingdom7.base_stats, strings.S.intelligence, 320f);
            SafeSetStat(GodKingdom7.base_stats, strings.S.lifespan, 160f);
            SafeSetStat(GodKingdom7.base_stats, strings.S.stamina, 500f);
            SafeSetStat(GodKingdom7.base_stats, strings.S.area_of_effect, 20f);
            SafeSetStat(GodKingdom7.base_stats, strings.S.critical_chance, 0.8f);
            SafeSetStat(GodKingdom7.base_stats, strings.S.targets, 24f);
            SafeSetStat(GodKingdom7.base_stats, strings.S.accuracy, 64f);
            SafeSetStat(GodKingdom7.base_stats, "Accuracy", 40f);
            SafeSetStat(GodKingdom7.base_stats, "Dodge", 60f);
            AssetManager.traits.add(GodKingdom7);

            ActorTrait GodKingdom8 = CreateTrait("GodKingdom8", "trait/GodKingdom8", "GodKingdom");
            GodKingdom8.rarity = Rarity.R3_Legendary;
            SafeSetStat(GodKingdom8.base_stats, strings.S.multiplier_health, 0.28f);
            SafeSetStat(GodKingdom8.base_stats, strings.S.multiplier_damage, 0.25f);
            SafeSetStat(GodKingdom8.base_stats, strings.S.intelligence, 120f);
            SafeSetStat(GodKingdom8.base_stats, strings.S.lifespan, 200f);
            SafeSetStat(GodKingdom8.base_stats, strings.S.stamina, 300f);
            SafeSetStat(GodKingdom8.base_stats, strings.S.area_of_effect, 10f);
            SafeSetStat(GodKingdom8.base_stats, strings.S.critical_chance, 0.6f);
            SafeSetStat(GodKingdom8.base_stats, strings.S.targets, 25f);
            SafeSetStat(GodKingdom8.base_stats, strings.S.accuracy, 70f);
            SafeSetStat(GodKingdom8.base_stats, "Accuracy", 30f);
            SafeSetStat(GodKingdom8.base_stats, "Dodge", 70f);
            AssetManager.traits.add(GodKingdom8);

            ActorTrait GodKingdom9 = CreateTrait("GodKingdom9", "trait/GodKingdom9", "GodKingdom");
            GodKingdom9.rarity = Rarity.R3_Legendary;
            SafeSetStat(GodKingdom9.base_stats, strings.S.multiplier_health, 0.25f);
            SafeSetStat(GodKingdom9.base_stats, strings.S.multiplier_damage, 0.18f);
            SafeSetStat(GodKingdom9.base_stats, strings.S.intelligence, 108f);
            SafeSetStat(GodKingdom9.base_stats, strings.S.lifespan, 125f);
            SafeSetStat(GodKingdom9.base_stats, strings.S.stamina, 225f);
            SafeSetStat(GodKingdom9.base_stats, strings.S.area_of_effect, 22f);
            SafeSetStat(GodKingdom9.base_stats, strings.S.critical_chance, 0.36f);
            SafeSetStat(GodKingdom9.base_stats, strings.S.targets, 72f);
            SafeSetStat(GodKingdom9.base_stats, strings.S.accuracy, 50f);
            SafeSetStat(GodKingdom9.base_stats, "Accuracy", 20f);
            SafeSetStat(GodKingdom9.base_stats, "Dodge", 80f);
            AssetManager.traits.add(GodKingdom9);

            ActorTrait GodKingdom10 = CreateTrait("GodKingdom10", "trait/GodKingdom10", "GodKingdom");
            GodKingdom10.rarity = Rarity.R3_Legendary;
            SafeSetStat(GodKingdom10.base_stats, strings.S.multiplier_damage, 0.4f);
            SafeSetStat(GodKingdom10.base_stats, strings.S.multiplier_health, 0.35f);
            SafeSetStat(GodKingdom10.base_stats, strings.S.intelligence, 130f);
            SafeSetStat(GodKingdom10.base_stats, strings.S.lifespan, 160f);
            SafeSetStat(GodKingdom10.base_stats, strings.S.stamina, 500f);
            SafeSetStat(GodKingdom10.base_stats, strings.S.area_of_effect, 28f);
            SafeSetStat(GodKingdom10.base_stats, strings.S.critical_chance, 0.24f);
            SafeSetStat(GodKingdom10.base_stats, strings.S.targets, 49f);
            SafeSetStat(GodKingdom10.base_stats, strings.S.accuracy, 50f);
            SafeSetStat(GodKingdom10.base_stats, "Accuracy", 10f);
            SafeSetStat(GodKingdom10.base_stats, "Dodge", 90f);
            AssetManager.traits.add(GodKingdom10);

            ActorTrait GodKingdom11 = CreateTrait("GodKingdom11", "trait/GodKingdom11", "GodKingdom");
            GodKingdom11.rarity = Rarity.R3_Legendary;
            SafeSetStat(GodKingdom11.base_stats, strings.S.multiplier_damage, 0.36f);
            SafeSetStat(GodKingdom11.base_stats, strings.S.multiplier_health, 0.4f);
            SafeSetStat(GodKingdom11.base_stats, strings.S.intelligence, 90f);
            SafeSetStat(GodKingdom11.base_stats, strings.S.lifespan, 180f);
            SafeSetStat(GodKingdom11.base_stats, strings.S.stamina, 160f);
            SafeSetStat(GodKingdom11.base_stats, strings.S.area_of_effect, 19f);
            SafeSetStat(GodKingdom11.base_stats, strings.S.critical_chance, 0.64f);
            SafeSetStat(GodKingdom11.base_stats, strings.S.targets, 20f);
            SafeSetStat(GodKingdom11.base_stats, strings.S.accuracy, 99f);
            SafeSetStat(GodKingdom11.base_stats, "Accuracy", 55f);
            SafeSetStat(GodKingdom11.base_stats, "Dodge", 45f);
            AssetManager.traits.add(GodKingdom11);

            ActorTrait GodKingdom12 = CreateTrait("GodKingdom12", "trait/GodKingdom12", "GodKingdom");
            GodKingdom12.rarity = Rarity.R3_Legendary;
            SafeSetStat(GodKingdom12.base_stats, strings.S.multiplier_damage, 0.32f);
            SafeSetStat(GodKingdom12.base_stats, strings.S.multiplier_health, 0.32f);
            SafeSetStat(GodKingdom12.base_stats, strings.S.intelligence, 121f);
            SafeSetStat(GodKingdom12.base_stats, strings.S.lifespan, 199f);
            SafeSetStat(GodKingdom12.base_stats, strings.S.stamina, 250f);
            SafeSetStat(GodKingdom12.base_stats, strings.S.area_of_effect, 30f);
            SafeSetStat(GodKingdom12.base_stats, strings.S.critical_chance, 0.11f);
            SafeSetStat(GodKingdom12.base_stats, strings.S.targets, 100f);
            SafeSetStat(GodKingdom12.base_stats, strings.S.accuracy, 99f);
            SafeSetStat(GodKingdom12.base_stats, "Accuracy", 65f);
            SafeSetStat(GodKingdom12.base_stats, "Dodge", 35f);
            AssetManager.traits.add(GodKingdom12);

            ActorTrait GodKingdom13 = CreateTrait("GodKingdom13", "trait/GodKingdom13", "GodKingdom");
            GodKingdom13.rarity = Rarity.R3_Legendary;
            SafeSetStat(GodKingdom13.base_stats, strings.S.multiplier_damage, 0.36f);
            SafeSetStat(GodKingdom13.base_stats, strings.S.multiplier_health, 0.25f);
            SafeSetStat(GodKingdom13.base_stats, strings.S.intelligence, 81f);
            SafeSetStat(GodKingdom13.base_stats, strings.S.lifespan, 125f);
            SafeSetStat(GodKingdom13.base_stats, strings.S.stamina, 144f);
            SafeSetStat(GodKingdom13.base_stats, strings.S.area_of_effect, 16f);
            SafeSetStat(GodKingdom13.base_stats, strings.S.critical_chance, 0.49f);
            SafeSetStat(GodKingdom13.base_stats, strings.S.targets, 14f);
            SafeSetStat(GodKingdom13.base_stats, strings.S.accuracy, 49f);
            SafeSetStat(GodKingdom13.base_stats, "Accuracy", 75f);
            SafeSetStat(GodKingdom13.base_stats, "Dodge", 25f);
            AssetManager.traits.add(GodKingdom13);

            ActorTrait GodKingdom14 = CreateTrait("GodKingdom14", "trait/GodKingdom14", "GodKingdom");
            GodKingdom14.rarity = Rarity.R3_Legendary;
            SafeSetStat(GodKingdom14.base_stats, strings.S.multiplier_damage, 0.44f);
            SafeSetStat(GodKingdom14.base_stats, strings.S.multiplier_health, 0.44f);
            SafeSetStat(GodKingdom14.base_stats, strings.S.intelligence, 188f);
            SafeSetStat(GodKingdom14.base_stats, strings.S.lifespan, 188f);
            SafeSetStat(GodKingdom14.base_stats, strings.S.stamina, 188f);
            SafeSetStat(GodKingdom14.base_stats, strings.S.area_of_effect, 18f);
            SafeSetStat(GodKingdom14.base_stats, strings.S.critical_chance, 0.18f);
            SafeSetStat(GodKingdom14.base_stats, strings.S.targets, 18f);
            SafeSetStat(GodKingdom14.base_stats, strings.S.accuracy, 88f);
            SafeSetStat(GodKingdom14.base_stats, "Accuracy", 85f);
            SafeSetStat(GodKingdom14.base_stats, "Dodge", 15f);
            AssetManager.traits.add(GodKingdom14);

            ActorTrait GodKingdom15 = CreateTrait("GodKingdom15", "trait/GodKingdom15", "GodKingdom");
            GodKingdom15.rarity = Rarity.R3_Legendary;
            SafeSetStat(GodKingdom15.base_stats, strings.S.multiplier_damage, 0.55f);
            SafeSetStat(GodKingdom15.base_stats, strings.S.multiplier_health, 0.3f);
            SafeSetStat(GodKingdom15.base_stats, strings.S.intelligence, 10f);
            SafeSetStat(GodKingdom15.base_stats, strings.S.lifespan, 99f);
            SafeSetStat(GodKingdom15.base_stats, strings.S.stamina, 399f);
            SafeSetStat(GodKingdom15.base_stats, strings.S.area_of_effect, 9f);
            SafeSetStat(GodKingdom15.base_stats, strings.S.critical_chance, 0.9f);
            SafeSetStat(GodKingdom15.base_stats, strings.S.targets, 19f);
            SafeSetStat(GodKingdom15.base_stats, strings.S.accuracy, 99f);
            SafeSetStat(GodKingdom15.base_stats, "Accuracy", 45f);
            SafeSetStat(GodKingdom15.base_stats, "Dodge", 55f);
            AssetManager.traits.add(GodKingdom15);

            ActorTrait GodKingdom16 = CreateTrait("GodKingdom16", "trait/GodKingdom16", "GodKingdom");
            GodKingdom16.rarity = Rarity.R3_Legendary;
             SafeSetStat(GodKingdom16.base_stats, strings.S.multiplier_damage, 0.36f);
            SafeSetStat(GodKingdom16.base_stats, strings.S.multiplier_health, 0.34f);
            SafeSetStat(GodKingdom16.base_stats, strings.S.intelligence, 88f);
            SafeSetStat(GodKingdom16.base_stats, strings.S.lifespan, 175f);
            SafeSetStat(GodKingdom16.base_stats, strings.S.stamina, 550f);
            SafeSetStat(GodKingdom16.base_stats, strings.S.area_of_effect, 48f);
            SafeSetStat(GodKingdom16.base_stats, strings.S.critical_chance, 0.64f);
            SafeSetStat(GodKingdom16.base_stats, strings.S.targets, 49f);
            SafeSetStat(GodKingdom16.base_stats, strings.S.accuracy, 36f);
            SafeSetStat(GodKingdom16.base_stats, "Accuracy", 35f);
            SafeSetStat(GodKingdom16.base_stats, "Dodge", 65f);
            AssetManager.traits.add(GodKingdom16);

            ActorTrait GodKingdom17 = CreateTrait("GodKingdom17", "trait/GodKingdom17", "GodKingdom");
            GodKingdom17.rarity = Rarity.R3_Legendary;
            SafeSetStat(GodKingdom17.base_stats, strings.S.multiplier_damage, 0.25f);
            SafeSetStat(GodKingdom17.base_stats, strings.S.multiplier_health, 0.25f);
            SafeSetStat(GodKingdom17.base_stats, strings.S.intelligence, 150f);
            SafeSetStat(GodKingdom17.base_stats, strings.S.lifespan, 170f);
            SafeSetStat(GodKingdom17.base_stats, strings.S.stamina, 1000f);
            SafeSetStat(GodKingdom17.base_stats, strings.S.area_of_effect, 36f);
            SafeSetStat(GodKingdom17.base_stats, strings.S.critical_chance, 0.5f);
            SafeSetStat(GodKingdom17.base_stats, strings.S.targets, 20f);
            SafeSetStat(GodKingdom17.base_stats, strings.S.accuracy, 64f);
            SafeSetStat(GodKingdom17.base_stats, "Accuracy", 25f);
            SafeSetStat(GodKingdom17.base_stats, "Dodge", 75f);
            AssetManager.traits.add(GodKingdom17);

            ActorTrait GodKingdom18 = CreateTrait("GodKingdom18", "trait/GodKingdom18", "GodKingdom");
            GodKingdom18.rarity = Rarity.R3_Legendary;
            SafeSetStat(GodKingdom18.base_stats, strings.S.multiplier_damage, 0.36f);
            SafeSetStat(GodKingdom18.base_stats, strings.S.multiplier_health, 0.18f);
            SafeSetStat(GodKingdom18.base_stats, strings.S.intelligence, 81f);
            SafeSetStat(GodKingdom18.base_stats, strings.S.lifespan, 190f);
            SafeSetStat(GodKingdom18.base_stats, strings.S.stamina, 1080f);
            SafeSetStat(GodKingdom18.base_stats, strings.S.area_of_effect, 10f);
            SafeSetStat(GodKingdom18.base_stats, strings.S.critical_chance, 0.45f);
            SafeSetStat(GodKingdom18.base_stats, strings.S.targets, 99f);
            SafeSetStat(GodKingdom18.base_stats, strings.S.accuracy, 56f);
            SafeSetStat(GodKingdom18.base_stats, "Accuracy", 15f);
            SafeSetStat(GodKingdom18.base_stats, "Dodge", 85f);
            AssetManager.traits.add(GodKingdom18);

            ActorTrait GodSeal = CreateTrait("GodSeal", "trait/GodSeal", "GodSeal");
            GodSeal.rarity = Rarity.R3_Legendary;
            SafeSetStat(GodSeal.base_stats, strings.S.multiplier_health, 10f); // 10倍生命值
            SafeSetStat(GodSeal.base_stats, strings.S.multiplier_damage, 10f); // 10倍伤害
            SafeSetStat(GodSeal.base_stats, strings.S.multiplier_speed, 5f);    // 5倍速度
            SafeSetStat(GodSeal.base_stats, strings.S.lifespan, 5000f);       // 增加5000年寿命 
            GodSeal.action_attack_target += traitAction.ZhiGaoTrueDamage_AttackAction;
            GodSeal.action_special_effect += new WorldAction(traitAction.GodSealSpecialEffect);
            AssetManager.traits.add(GodSeal);

            ActorTrait MysteriousConcoction01 = CreateTrait("MysteriousConcoction1+", "trait/MysteriousConcoction1+", "MysteriousConcoction");
            MysteriousConcoction01.rarity = Rarity.R1_Rare;
            MysteriousConcoction01.action_special_effect += new WorldAction(traitAction.MysteriousConcoction01_Regen);
            AssetManager.traits.add(MysteriousConcoction01);

            ActorTrait MysteriousConcoction2 = CreateTrait("MysteriousConcoction2", "trait/MysteriousConcoction2", "MysteriousConcoction");
            MysteriousConcoction2.rarity = Rarity.R1_Rare;
            MysteriousConcoction2.action_special_effect += new WorldAction(traitAction.MysteriousConcoction2_Regen);
            AssetManager.traits.add(MysteriousConcoction2);

            ActorTrait MysteriousConcoction3 = CreateTrait("MysteriousConcoction3", "trait/MysteriousConcoction3", "MysteriousConcoction");
            MysteriousConcoction3.rarity = Rarity.R1_Rare;
            MysteriousConcoction3.action_special_effect += new WorldAction(traitAction.MysteriousConcoction3_Regen);
            AssetManager.traits.add(MysteriousConcoction3);

            ActorTrait MysteriousConcoction4 = CreateTrait("MysteriousConcoction4", "trait/MysteriousConcoction4", "MysteriousConcoction");
            MysteriousConcoction4.rarity = Rarity.R1_Rare;
            MysteriousConcoction4.action_special_effect += new WorldAction(traitAction.MysteriousConcoction4_Regen);
            AssetManager.traits.add(MysteriousConcoction4);

            ActorTrait MysteriousConcoction5 = CreateTrait("MysteriousConcoction5", "trait/MysteriousConcoction5", "MysteriousConcoction");
            MysteriousConcoction5.rarity = Rarity.R1_Rare;
            MysteriousConcoction5.action_special_effect += new WorldAction(traitAction.MysteriousConcoction5_Regen);
            AssetManager.traits.add(MysteriousConcoction5);

            ActorTrait MysteriousConcoction6 = CreateTrait("MysteriousConcoction6", "trait/MysteriousConcoction6", "MysteriousConcoction");
            MysteriousConcoction6.rarity = Rarity.R1_Rare;
            MysteriousConcoction6.action_special_effect += new WorldAction(traitAction.MysteriousConcoction6_Regen);
            AssetManager.traits.add(MysteriousConcoction6);

            ActorTrait MysteriousConcoction7 = CreateTrait("MysteriousConcoction7", "trait/MysteriousConcoction7", "MysteriousConcoction");
            MysteriousConcoction7.rarity = Rarity.R2_Epic;
            MysteriousConcoction7.action_special_effect += new WorldAction(traitAction.MysteriousConcoction7_Regen);
            AssetManager.traits.add(MysteriousConcoction7);

            ActorTrait MysteriousConcoction8 = CreateTrait("MysteriousConcoction8", "trait/MysteriousConcoction8", "MysteriousConcoction");
            MysteriousConcoction8.rarity = Rarity.R2_Epic;
            MysteriousConcoction8.action_special_effect += new WorldAction(traitAction.MysteriousConcoction8_Regen);
            AssetManager.traits.add(MysteriousConcoction8);

            ActorTrait MysteriousConcoction9 = CreateTrait("MysteriousConcoction9", "trait/MysteriousConcoction9", "MysteriousConcoction");
            MysteriousConcoction9.rarity = Rarity.R2_Epic;
            MysteriousConcoction9.action_special_effect += new WorldAction(traitAction.MysteriousConcoction9_Regen);
            AssetManager.traits.add(MysteriousConcoction9);

            ActorTrait MysteriousConcoction91 = CreateTrait("MysteriousConcoction91", "trait/MysteriousConcoction91", "MysteriousConcoction");
            MysteriousConcoction91.rarity = Rarity.R2_Epic;
            MysteriousConcoction91.action_special_effect += new WorldAction(traitAction.MysteriousConcoction91_Regen);
            AssetManager.traits.add(MysteriousConcoction91);

            ActorTrait MysteriousConcoction92 = CreateTrait("MysteriousConcoction92", "trait/MysteriousConcoction92", "MysteriousConcoction");
            MysteriousConcoction92.rarity = Rarity.R2_Epic;
            MysteriousConcoction92.action_special_effect += new WorldAction(traitAction.MysteriousConcoction92_Regen);
            AssetManager.traits.add(MysteriousConcoction92);

            ActorTrait MysteriousConcoction93 = CreateTrait("MysteriousConcoction93", "trait/MysteriousConcoction93", "MysteriousConcoction");
            MysteriousConcoction93.rarity = Rarity.R2_Epic;
            MysteriousConcoction93.action_special_effect += new WorldAction(traitAction.MysteriousConcoction93_Regen);
            AssetManager.traits.add(MysteriousConcoction93);

            ActorTrait MysteriousConcoction94 = CreateTrait("MysteriousConcoction94", "trait/MysteriousConcoction94", "MysteriousConcoction");
            MysteriousConcoction94.rarity = Rarity.R3_Legendary;
            MysteriousConcoction94.action_special_effect += new WorldAction(traitAction.MysteriousConcoction94_Regen);
            AssetManager.traits.add(MysteriousConcoction94);

            ActorTrait MysteriousConcoction95 = CreateTrait("MysteriousConcoction95", "trait/MysteriousConcoction95", "MysteriousConcoction");
            MysteriousConcoction95.rarity = Rarity.R3_Legendary;
            MysteriousConcoction95.action_special_effect += new WorldAction(traitAction.MysteriousConcoction95_Regen);
            AssetManager.traits.add(MysteriousConcoction95);

            ActorTrait MysteriousConcoction96 = CreateTrait("MysteriousConcoction96", "trait/MysteriousConcoction96", "MysteriousConcoction");
            MysteriousConcoction96.rarity = Rarity.R3_Legendary;
            MysteriousConcoction96.action_special_effect += new WorldAction(traitAction.MysteriousConcoction96_Regen);
            AssetManager.traits.add(MysteriousConcoction96);

            ActorTrait MysteriousConcoction97 = CreateTrait("MysteriousConcoction97", "trait/MysteriousConcoction97", "MysteriousConcoction");
            MysteriousConcoction97.rarity = Rarity.R3_Legendary;
            MysteriousConcoction97.action_special_effect += new WorldAction(traitAction.MysteriousConcoction97_Regen);
            AssetManager.traits.add(MysteriousConcoction97);

            ActorTrait MysteriousConcoction98 = CreateTrait("MysteriousConcoction98", "trait/MysteriousConcoction98", "MysteriousConcoction");
            MysteriousConcoction98.rarity = Rarity.R3_Legendary;
            MysteriousConcoction98.action_special_effect += new WorldAction(traitAction.MysteriousConcoction98_Regen);
            AssetManager.traits.add(MysteriousConcoction98);

            ActorTrait MysteriousConcoction99 = CreateTrait("MysteriousConcoction99", "trait/MysteriousConcoction99", "MysteriousConcoction");
            MysteriousConcoction99.rarity = Rarity.R3_Legendary;
            MysteriousConcoction99.action_special_effect += new WorldAction(traitAction.MysteriousConcoction99_Regen);
            AssetManager.traits.add(MysteriousConcoction99);
            
            // 新增：提高悟性的炼金秘药特质
            ActorTrait MysteriousConcoction100 = CreateTrait("MysteriousConcoction100", "trait/MysteriousConcoction100", "MysteriousConcoction");
            MysteriousConcoction100.rarity = Rarity.R3_Legendary;
            MysteriousConcoction100.action_special_effect += new WorldAction(traitAction.MysteriousConcoction100_Regen);
            AssetManager.traits.add(MysteriousConcoction100);

            ActorTrait MysteriousConcoction1 = CreateTrait("MysteriousConcoction1", "trait/MysteriousConcoction1", "MysteriousConcoction");
        MysteriousConcoction1.rarity = Rarity.R3_Legendary;
        MysteriousConcoction1.action_special_effect += new WorldAction(traitAction.MysteriousConcoction1_Regen);
        AssetManager.traits.add(MysteriousConcoction1);

        // 先贤知识特质定义 - 低级（R1_Rare）
        ActorTrait AncientKnowledge01 = CreateTrait("AncientKnowledge01", "trait/AncientKnowledge01", "AncientKnowledge");
        AncientKnowledge01.rarity = Rarity.R1_Rare;
        AncientKnowledge01.action_special_effect += new WorldAction(traitAction.AncientKnowledge01_Regen);
        AssetManager.traits.add(AncientKnowledge01);

        ActorTrait AncientKnowledge02 = CreateTrait("AncientKnowledge02", "trait/AncientKnowledge02", "AncientKnowledge");
        AncientKnowledge02.rarity = Rarity.R1_Rare;
        AncientKnowledge02.action_special_effect += new WorldAction(traitAction.AncientKnowledge02_Regen);
        AssetManager.traits.add(AncientKnowledge02);

        ActorTrait AncientKnowledge03 = CreateTrait("AncientKnowledge03", "trait/AncientKnowledge03", "AncientKnowledge");
        AncientKnowledge03.rarity = Rarity.R1_Rare;
        AncientKnowledge03.action_special_effect += new WorldAction(traitAction.AncientKnowledge03_Regen);
        AssetManager.traits.add(AncientKnowledge03);

        ActorTrait AncientKnowledge04 = CreateTrait("AncientKnowledge04", "trait/AncientKnowledge04", "AncientKnowledge");
        AncientKnowledge04.rarity = Rarity.R1_Rare;
        AncientKnowledge04.action_special_effect += new WorldAction(traitAction.AncientKnowledge04_Regen);
        AssetManager.traits.add(AncientKnowledge04);

        ActorTrait AncientKnowledge05 = CreateTrait("AncientKnowledge05", "trait/AncientKnowledge05", "AncientKnowledge");
        AncientKnowledge05.rarity = Rarity.R1_Rare;
        AncientKnowledge05.action_special_effect += new WorldAction(traitAction.AncientKnowledge05_Regen);
        AssetManager.traits.add(AncientKnowledge05);

        ActorTrait AncientKnowledge06 = CreateTrait("AncientKnowledge06", "trait/AncientKnowledge06", "AncientKnowledge");
        AncientKnowledge06.rarity = Rarity.R1_Rare;
        AncientKnowledge06.action_special_effect += new WorldAction(traitAction.AncientKnowledge06_Regen);
        AssetManager.traits.add(AncientKnowledge06);

        // 先贤知识特质定义 - 中级（R2_Epic）
        ActorTrait AncientKnowledge07 = CreateTrait("AncientKnowledge07", "trait/AncientKnowledge07", "AncientKnowledge");
        AncientKnowledge07.rarity = Rarity.R2_Epic;
        AncientKnowledge07.action_special_effect += new WorldAction(traitAction.AncientKnowledge07_Regen);
        AssetManager.traits.add(AncientKnowledge07);

        ActorTrait AncientKnowledge08 = CreateTrait("AncientKnowledge08", "trait/AncientKnowledge08", "AncientKnowledge");
        AncientKnowledge08.rarity = Rarity.R2_Epic;
        AncientKnowledge08.action_special_effect += new WorldAction(traitAction.AncientKnowledge08_Regen);
        AssetManager.traits.add(AncientKnowledge08);

        ActorTrait AncientKnowledge09 = CreateTrait("AncientKnowledge09", "trait/AncientKnowledge09", "AncientKnowledge");
        AncientKnowledge09.rarity = Rarity.R2_Epic;
        AncientKnowledge09.action_special_effect += new WorldAction(traitAction.AncientKnowledge09_Regen);
        AssetManager.traits.add(AncientKnowledge09);

        ActorTrait AncientKnowledge10 = CreateTrait("AncientKnowledge10", "trait/AncientKnowledge10", "AncientKnowledge");
        AncientKnowledge10.rarity = Rarity.R2_Epic;
        AncientKnowledge10.action_special_effect += new WorldAction(traitAction.AncientKnowledge10_Regen);
        AssetManager.traits.add(AncientKnowledge10);

        ActorTrait AncientKnowledge11 = CreateTrait("AncientKnowledge11", "trait/AncientKnowledge11", "AncientKnowledge");
        AncientKnowledge11.rarity = Rarity.R2_Epic;
        AncientKnowledge11.action_special_effect += new WorldAction(traitAction.AncientKnowledge11_Regen);
        AssetManager.traits.add(AncientKnowledge11);

        ActorTrait AncientKnowledge12 = CreateTrait("AncientKnowledge12", "trait/AncientKnowledge12", "AncientKnowledge");
        AncientKnowledge12.rarity = Rarity.R2_Epic;
        AncientKnowledge12.action_special_effect += new WorldAction(traitAction.AncientKnowledge12_Regen);
        AssetManager.traits.add(AncientKnowledge12);

        // 先贤知识特质定义 - 高级（R3_Legendary）
        ActorTrait AncientKnowledge13 = CreateTrait("AncientKnowledge13", "trait/AncientKnowledge13", "AncientKnowledge");
        AncientKnowledge13.rarity = Rarity.R3_Legendary;
        AncientKnowledge13.action_special_effect += new WorldAction(traitAction.AncientKnowledge13_Regen);
        AssetManager.traits.add(AncientKnowledge13);

        ActorTrait AncientKnowledge14 = CreateTrait("AncientKnowledge14", "trait/AncientKnowledge14", "AncientKnowledge");
        AncientKnowledge14.rarity = Rarity.R3_Legendary;
        AncientKnowledge14.action_special_effect += new WorldAction(traitAction.AncientKnowledge14_Regen);
        AssetManager.traits.add(AncientKnowledge14);

        ActorTrait AncientKnowledge15 = CreateTrait("AncientKnowledge15", "trait/AncientKnowledge15", "AncientKnowledge");
        AncientKnowledge15.rarity = Rarity.R3_Legendary;
        AncientKnowledge15.action_special_effect += new WorldAction(traitAction.AncientKnowledge15_Regen);
        AssetManager.traits.add(AncientKnowledge15);

        ActorTrait AncientKnowledge16 = CreateTrait("AncientKnowledge16", "trait/AncientKnowledge16", "AncientKnowledge");
        AncientKnowledge16.rarity = Rarity.R3_Legendary;
        AncientKnowledge16.action_special_effect += new WorldAction(traitAction.AncientKnowledge16_Regen);
        AssetManager.traits.add(AncientKnowledge16);

        ActorTrait AncientKnowledge17 = CreateTrait("AncientKnowledge17", "trait/AncientKnowledge17", "AncientKnowledge");
        AncientKnowledge17.rarity = Rarity.R3_Legendary;
        AncientKnowledge17.action_special_effect += new WorldAction(traitAction.AncientKnowledge17_Regen);
        AssetManager.traits.add(AncientKnowledge17);

        ActorTrait AncientKnowledge18 = CreateTrait("AncientKnowledge18", "trait/AncientKnowledge18", "AncientKnowledge");
        AncientKnowledge18.rarity = Rarity.R3_Legendary;
        AncientKnowledge18.action_special_effect += new WorldAction(traitAction.AncientKnowledge18_Regen);
        AssetManager.traits.add(AncientKnowledge18);
        }

        private static void SafeSetStat(BaseStats baseStats, string statKey, float value)
        {
            baseStats[statKey]= value;
        }
    }
}