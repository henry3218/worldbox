using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using ReflectionUtility;
using ai;
using System.Numerics;
using UnityEngine;

namespace ChivalryZhanXun.code
{
    internal class traits
    {
        public static void Init()
        {
            ActorTrait ZhanXun1 = new ActorTrait();
            ZhanXun1.id = "ZhanXun1";
            ZhanXun1.path_icon = "trait/ZhanXun1";
            ZhanXun1.needs_to_be_explored = false;
            ZhanXun1.group_id = "ZhanXun";
            ZhanXun1.base_stats = new BaseStats();
            ZhanXun1.base_stats["lifespan"] = 1f;
            ZhanXun1.base_stats["damage"] = 1f;
            ZhanXun1.base_stats["health"] = 10f;
            ZhanXun1.base_stats["targets"] = 1f;
            ZhanXun1.base_stats["attack_speed"] = 1f;
            ZhanXun1.base_stats["area_of_effect"] = 1f;
            ZhanXun1.base_stats["range"] = 1f;
            AssetManager.traits.add(ZhanXun1);

            ActorTrait ZhanXun2 = new ActorTrait();
            ZhanXun2.id = "ZhanXun2";
            ZhanXun2.path_icon = "trait/ZhanXun2";
            ZhanXun2.needs_to_be_explored = false;
            ZhanXun2.group_id = "ZhanXun";
            ZhanXun2.base_stats = new BaseStats();
            ZhanXun2.base_stats["lifespan"] = 10f;
            ZhanXun2.base_stats["damage"] = 10f;
            ZhanXun2.base_stats["health"] = 100f;
            ZhanXun2.base_stats["targets"] = 2f;
            ZhanXun2.base_stats["accuracy"] = 1f;
            ZhanXun2.base_stats["warfare"] = 1f;
            ZhanXun2.base_stats["stewardship"] = 1f;
            ZhanXun2.base_stats["armor"] = 1f;
            ZhanXun2.base_stats["attack_speed"] = 2f;
            ZhanXun2.base_stats["area_of_effect"] = 2f;
            ZhanXun2.base_stats["range"] = 2f;
            ZhanXun2.action_special_effect += (WorldAction)Delegate.Combine(ZhanXun2.action_special_effect,
                    new WorldAction(traitAction.ZhanXun2_effectAction)); 
            AssetManager.traits.add(ZhanXun2);

            ActorTrait ZhanXun3 = new ActorTrait();
            ZhanXun3.id = "ZhanXun3";
            ZhanXun3.path_icon = "trait/ZhanXun3";
            ZhanXun3.needs_to_be_explored = false;
            ZhanXun3.group_id = "ZhanXun";
            ZhanXun3.base_stats = new BaseStats();
            ZhanXun3.base_stats["lifespan"] = 20f;
            ZhanXun3.base_stats["damage"] = 20f;
            ZhanXun3.base_stats["health"] = 200f;
            ZhanXun3.base_stats["targets"] = 3f;
            ZhanXun3.base_stats["accuracy"] = 2f;
            ZhanXun3.base_stats["warfare"] = 2f;
            ZhanXun3.base_stats["stewardship"] = 2f;
            ZhanXun3.base_stats["armor"] = 2f;
            ZhanXun3.base_stats["attack_speed"] = 3f;
            ZhanXun3.base_stats["area_of_effect"] = 3f;
            ZhanXun3.base_stats["range"] = 3f;
            ZhanXun3.action_special_effect += (WorldAction)Delegate.Combine(ZhanXun3.action_special_effect,
                new WorldAction(traitAction.ZhanXun3_effectAction));
            AssetManager.traits.add(ZhanXun3);

            ActorTrait ZhanXun4 = new ActorTrait();
            ZhanXun4.id = "ZhanXun4";
            ZhanXun4.path_icon = "trait/ZhanXun4";
            ZhanXun4.needs_to_be_explored = false;
            ZhanXun4.group_id = "ZhanXun";
            ZhanXun4.base_stats = new BaseStats();
            ZhanXun4.base_stats["lifespan"] = 30f;
            ZhanXun4.base_stats["damage"] = 30f;
            ZhanXun4.base_stats["health"] = 300f;
            ZhanXun4.base_stats["targets"] = 4f;
            ZhanXun4.base_stats["accuracy"] = 3f;
            ZhanXun4.base_stats["warfare"] = 3f;
            ZhanXun4.base_stats["stewardship"] = 3f;
            ZhanXun4.base_stats["armor"] = 3f;
            ZhanXun4.base_stats["attack_speed"] = 4f;
            ZhanXun4.base_stats["area_of_effect"] = 4f;
            ZhanXun4.base_stats["range"] = 4f;
            ZhanXun4.action_special_effect += (WorldAction)Delegate.Combine(ZhanXun4.action_special_effect,
                new WorldAction(traitAction.ZhanXun4_effectAction));
            AssetManager.traits.add(ZhanXun4);

            ActorTrait ZhanXun5 = new ActorTrait();
            ZhanXun5.id = "ZhanXun5";
            ZhanXun5.path_icon = "trait/ZhanXun5";
            ZhanXun5.needs_to_be_explored = false;
            ZhanXun5.group_id = "ZhanXun";
            ZhanXun5.base_stats = new BaseStats();
            ZhanXun5.base_stats["lifespan"] = 50f;
            ZhanXun5.base_stats["damage"] = 50f;
            ZhanXun5.base_stats["health"] = 500f;
            ZhanXun5.base_stats["targets"] = 5f;
            ZhanXun5.base_stats["accuracy"] = 5f;
            ZhanXun5.base_stats["warfare"] = 5f;
            ZhanXun5.base_stats["stewardship"] = 5f;
            ZhanXun5.base_stats["armor"] = 5f;
            ZhanXun5.base_stats["attack_speed"] = 5f;
            ZhanXun5.base_stats["area_of_effect"] = 5f;
            ZhanXun5.base_stats["range"] = 5f;
            ZhanXun5.action_special_effect += (WorldAction)Delegate.Combine(ZhanXun5.action_special_effect,
                new WorldAction(traitAction.ZhanXun5_effectAction));
            AssetManager.traits.add(ZhanXun5);

            ActorTrait ZhanXun6 = new ActorTrait();
            ZhanXun6.id = "ZhanXun6";
            ZhanXun6.path_icon = "trait/ZhanXun6";
            ZhanXun6.needs_to_be_explored = false;
            ZhanXun6.group_id = "ZhanXun";
            ZhanXun6.base_stats = new BaseStats();
            ZhanXun6.base_stats["lifespan"] = 70f;
            ZhanXun6.base_stats["damage"] = 70f;
            ZhanXun6.base_stats["health"] = 700f;
            ZhanXun6.base_stats["targets"] = 6f;
            ZhanXun6.base_stats["accuracy"] = 7f;
            ZhanXun6.base_stats["warfare"] = 7f;
            ZhanXun6.base_stats["stewardship"] = 7f;
            ZhanXun6.base_stats["armor"] = 7f;
            ZhanXun6.base_stats["attack_speed"] = 6f;
            ZhanXun6.base_stats["area_of_effect"] = 6f;
            ZhanXun6.base_stats["range"] = 6f;
            ZhanXun6.action_special_effect += (WorldAction)Delegate.Combine(ZhanXun6.action_special_effect,
                new WorldAction(traitAction.ZhanXun6_effectAction));
            AssetManager.traits.add(ZhanXun6);

            ActorTrait ZhanXun7 = new ActorTrait();
            ZhanXun7.id = "ZhanXun7";
            ZhanXun7.path_icon = "trait/ZhanXun7";
            ZhanXun7.needs_to_be_explored = false;
            ZhanXun7.group_id = "ZhanXun";
            ZhanXun7.base_stats = new BaseStats();
            ZhanXun7.base_stats["lifespan"] = 100f;
            ZhanXun7.base_stats["damage"] = 100f;
            ZhanXun7.base_stats["health"] = 1000f;
            ZhanXun7.base_stats["targets"] = 7f;
            ZhanXun7.base_stats["accuracy"] = 10f;
            ZhanXun7.base_stats["warfare"] = 10f;
            ZhanXun7.base_stats["stewardship"] = 10f;
            ZhanXun7.base_stats["armor"] = 10f;
            ZhanXun7.base_stats["attack_speed"] = 7f;
            ZhanXun7.base_stats["multiplier_damage"] = 0.5f;
            ZhanXun7.base_stats["multiplier_health"] = 0.5f;
            ZhanXun7.base_stats["area_of_effect"] = 7f;
            ZhanXun7.base_stats["range"] = 7f;
            ZhanXun7.action_special_effect += (WorldAction)Delegate.Combine(ZhanXun7.action_special_effect,
                new WorldAction(traitAction.ZhanXun7_effectAction));
            AssetManager.traits.add(ZhanXun7);

            ActorTrait ZhanXun8 = new ActorTrait();
            ZhanXun8.id = "ZhanXun8";
            ZhanXun8.path_icon = "trait/ZhanXun8";
            ZhanXun8.needs_to_be_explored = false;
            ZhanXun8.group_id = "ZhanXun";
            ZhanXun8.base_stats = new BaseStats();
            ZhanXun8.base_stats["lifespan"] = 200f;
            ZhanXun8.base_stats["damage"] = 200f;
            ZhanXun8.base_stats["health"] = 2000f;
            ZhanXun8.base_stats["targets"] = 8f;
            ZhanXun8.base_stats["accuracy"] = 20f;
            ZhanXun8.base_stats["warfare"] = 20f;
            ZhanXun8.base_stats["stewardship"] = 20f;
            ZhanXun8.base_stats["armor"] = 20f;
            ZhanXun8.base_stats["attack_speed"] = 8f;
            ZhanXun8.base_stats["multiplier_damage"] = 0.7f;
            ZhanXun8.base_stats["multiplier_health"] = 0.7f;
            ZhanXun8.base_stats["area_of_effect"] = 8f;
            ZhanXun8.base_stats["range"] = 8f;
            ZhanXun8.action_special_effect += (WorldAction)Delegate.Combine(ZhanXun8.action_special_effect,
                new WorldAction(traitAction.ZhanXun8_effectAction));
            AssetManager.traits.add(ZhanXun8);

            ActorTrait ZhanXun9 = new ActorTrait();
            ZhanXun9.id = "ZhanXun9";
            ZhanXun9.path_icon = "trait/ZhanXun9";
            ZhanXun9.needs_to_be_explored = false;
            ZhanXun9.group_id = "ZhanXun";
            ZhanXun9.base_stats = new BaseStats();
            ZhanXun9.base_stats["lifespan"] = 300f;
            ZhanXun9.base_stats["damage"] = 300f;
            ZhanXun9.base_stats["health"] = 3000f;
            ZhanXun9.base_stats["targets"] = 9f;
            ZhanXun9.base_stats["accuracy"] = 30f;
            ZhanXun9.base_stats["warfare"] = 30f;
            ZhanXun9.base_stats["stewardship"] = 30f;
            ZhanXun9.base_stats["armor"] = 30f;
            ZhanXun9.base_stats["attack_speed"] = 9f;
            ZhanXun9.base_stats["multiplier_damage"] = 1f;
            ZhanXun9.base_stats["multiplier_health"] = 1f;
            ZhanXun9.base_stats["area_of_effect"] = 9f;
            ZhanXun9.base_stats["range"] = 9f;
            ZhanXun9.action_special_effect += (WorldAction)Delegate.Combine(ZhanXun9.action_special_effect,
                new WorldAction(traitAction.ZhanXun9_effectAction));
            AssetManager.traits.add(ZhanXun9);

            ActorTrait ZhanXun91 = new ActorTrait();
            ZhanXun91.id = "ZhanXun91";
            ZhanXun91.path_icon = "trait/ZhanXun91";
            ZhanXun91.needs_to_be_explored = false;
            ZhanXun91.group_id = "ZhanXun";
            ZhanXun91.base_stats = new BaseStats();
            ZhanXun91.base_stats["lifespan"] = 500f;
            ZhanXun91.base_stats["damage"] = 500f;
            ZhanXun91.base_stats["health"] = 5000f;
            ZhanXun91.base_stats["targets"] = 10f;
            ZhanXun91.base_stats["accuracy"] = 50f;
            ZhanXun91.base_stats["warfare"] = 50f;
            ZhanXun91.base_stats["stewardship"] = 50f;
            ZhanXun91.base_stats["armor"] = 50f;
            ZhanXun91.base_stats["attack_speed"] = 20f;
            ZhanXun91.base_stats["multiplier_attack_speed"] = 1f;
            ZhanXun91.base_stats["multiplier_damage"] = 1.5f;
            ZhanXun91.base_stats["multiplier_health"] = 1.5f;
            ZhanXun91.base_stats["area_of_effect"] = 10f;
            ZhanXun91.base_stats["range"] = 10f;
            ZhanXun91.action_special_effect += (WorldAction)Delegate.Combine(ZhanXun91.action_special_effect,
                new WorldAction(traitAction.ZhanXun91_effectAction));
            AssetManager.traits.add(ZhanXun91);

            ActorTrait ZhanXun92 = new ActorTrait();
            ZhanXun92.id = "ZhanXun92";
            ZhanXun92.path_icon = "trait/ZhanXun92";
            ZhanXun92.needs_to_be_explored = false;
            ZhanXun92.group_id = "ZhanXun";
            ZhanXun92.base_stats = new BaseStats();
            ZhanXun92.base_stats["lifespan"] = 700f;
            ZhanXun92.base_stats["damage"] = 700f;
            ZhanXun92.base_stats["health"] = 7000f;
            ZhanXun92.base_stats["targets"] = 11f;
            ZhanXun92.base_stats["accuracy"] = 70f;
            ZhanXun92.base_stats["warfare"] = 70f;
            ZhanXun92.base_stats["stewardship"] = 70f;
            ZhanXun92.base_stats["armor"] = 70f;
            ZhanXun92.base_stats["attack_speed"] = 20f;
            ZhanXun92.base_stats["multiplier_attack_speed"] = 1f;
            ZhanXun92.base_stats["multiplier_damage"] = 1.7f;
            ZhanXun92.base_stats["multiplier_health"] = 1.7f;
            ZhanXun92.base_stats["area_of_effect"] = 10f;
            ZhanXun92.base_stats["range"] = 10f;
            ZhanXun92.action_special_effect += (WorldAction)Delegate.Combine(ZhanXun92.action_special_effect,
                new WorldAction(traitAction.ZhanXun92_effectAction));
            AssetManager.traits.add(ZhanXun92);

            ActorTrait ZhanXun93 = new ActorTrait();
            ZhanXun93.id = "ZhanXun93";
            ZhanXun93.path_icon = "trait/ZhanXun93";
            ZhanXun93.needs_to_be_explored = false;
            ZhanXun93.group_id = "ZhanXun";
            ZhanXun93.base_stats = new BaseStats();
            ZhanXun93.base_stats["lifespan"] = 1000f;
            ZhanXun93.base_stats["damage"] = 1000f;
            ZhanXun93.base_stats["health"] = 10000f;
            ZhanXun93.base_stats["targets"] = 12f;
            ZhanXun93.base_stats["accuracy"] = 100f;
            ZhanXun93.base_stats["warfare"] = 100f;
            ZhanXun93.base_stats["stewardship"] = 100f;
            ZhanXun93.base_stats["armor"] = 100f;
            ZhanXun93.base_stats["attack_speed"] = 20f;
            ZhanXun93.base_stats["multiplier_attack_speed"] = 1f;
            ZhanXun93.base_stats["multiplier_damage"] = 2f;
            ZhanXun93.base_stats["multiplier_health"] = 2f;
            ZhanXun93.base_stats["area_of_effect"] = 10f;
            ZhanXun93.base_stats["range"] = 10f;
            ZhanXun93.action_special_effect += (WorldAction)Delegate.Combine(ZhanXun93.action_special_effect,
                new WorldAction(traitAction.ZhanXun93_effectAction));
            AssetManager.traits.add(ZhanXun93);

            ActorTrait ZhanXun94 = new ActorTrait();
            ZhanXun94.id = "ZhanXun94";
            ZhanXun94.path_icon = "trait/ZhanXun94";
            ZhanXun94.needs_to_be_explored = false;
            ZhanXun94.group_id = "ZhanXun";
            ZhanXun94.base_stats = new BaseStats();
            ZhanXun94.base_stats["lifespan"] = 2000f;
            ZhanXun94.base_stats["damage"] = 2000f;
            ZhanXun94.base_stats["health"] = 20000f;
            ZhanXun94.base_stats["targets"] = 13f;
            ZhanXun94.base_stats["accuracy"] = 200f;
            ZhanXun94.base_stats["warfare"] = 200f;
            ZhanXun94.base_stats["stewardship"] = 200f;
            ZhanXun94.base_stats["armor"] = 200f;
            ZhanXun94.base_stats["attack_speed"] = 20f;
            ZhanXun94.base_stats["multiplier_attack_speed"] = 1f;
            ZhanXun94.base_stats["multiplier_damage"] = 3f;
            ZhanXun94.base_stats["multiplier_health"] = 3f;
            ZhanXun94.base_stats["area_of_effect"] = 10f;
            ZhanXun94.base_stats["range"] = 10f;
            ZhanXun94.action_special_effect += (WorldAction)Delegate.Combine(ZhanXun94.action_special_effect,
                new WorldAction(traitAction.ZhanXun94_effectAction));
            AssetManager.traits.add(ZhanXun94);

            ActorTrait ZhanXun95 = new ActorTrait();
            ZhanXun95.id = "ZhanXun95";
            ZhanXun95.path_icon = "trait/ZhanXun95";
            ZhanXun95.needs_to_be_explored = false;
            ZhanXun95.group_id = "ZhanXun";
            ZhanXun95.base_stats = new BaseStats();
            ZhanXun95.base_stats["lifespan"] = 3000f;
            ZhanXun95.base_stats["damage"] = 3000f;
            ZhanXun95.base_stats["health"] = 30000f;
            ZhanXun95.base_stats["targets"] = 14f;
            ZhanXun95.base_stats["accuracy"] = 300f;
            ZhanXun95.base_stats["warfare"] = 300f;
            ZhanXun95.base_stats["stewardship"] = 300f;
            ZhanXun95.base_stats["armor"] = 300f;
            ZhanXun95.base_stats["attack_speed"] = 20f;
            ZhanXun95.base_stats["multiplier_attack_speed"] = 1f;
            ZhanXun95.base_stats["multiplier_damage"] = 4f;
            ZhanXun95.base_stats["multiplier_health"] = 4f;
            ZhanXun95.base_stats["area_of_effect"] = 10f;
            ZhanXun95.base_stats["range"] = 10f;
            ZhanXun95.action_special_effect += (WorldAction)Delegate.Combine(ZhanXun95.action_special_effect,
                new WorldAction(traitAction.ZhanXun95_effectAction));
            AssetManager.traits.add(ZhanXun95);

            ActorTrait ZhanXun96 = new ActorTrait();
            ZhanXun96.id = "ZhanXun96";
            ZhanXun96.path_icon = "trait/ZhanXun96";
            ZhanXun96.needs_to_be_explored = false;
            ZhanXun96.group_id = "ZhanXun";
            ZhanXun96.base_stats = new BaseStats();
            ZhanXun96.base_stats["lifespan"] = 5000f;
            ZhanXun96.base_stats["damage"] = 5000f;
            ZhanXun96.base_stats["health"] = 50000f;
            ZhanXun96.base_stats["targets"] = 15f;
            ZhanXun96.base_stats["accuracy"] = 500f;
            ZhanXun96.base_stats["warfare"] = 500f;
            ZhanXun96.base_stats["stewardship"] = 500f;
            ZhanXun96.base_stats["armor"] = 500f;
            ZhanXun96.base_stats["attack_speed"] = 20f;
            ZhanXun96.base_stats["multiplier_attack_speed"] = 1f;
            ZhanXun96.base_stats["multiplier_damage"] = 5f;
            ZhanXun96.base_stats["multiplier_health"] = 5f;
            ZhanXun96.base_stats["area_of_effect"] = 10f;
            ZhanXun96.base_stats["range"] = 10f;
            ZhanXun96.action_special_effect += (WorldAction)Delegate.Combine(ZhanXun96.action_special_effect,
                new WorldAction(traitAction.ZhanXun96_effectAction));
            AssetManager.traits.add(ZhanXun96);

            ActorTrait ZhanXun97 = new ActorTrait();
            ZhanXun97.id = "ZhanXun97";
            ZhanXun97.path_icon = "trait/ZhanXun97";
            ZhanXun97.needs_to_be_explored = false;
            ZhanXun97.group_id = "ZhanXun";
            ZhanXun97.base_stats = new BaseStats();
            ZhanXun97.base_stats["lifespan"] = 7000f;
            ZhanXun97.base_stats["damage"] = 7000f;
            ZhanXun97.base_stats["health"] = 70000f;
            ZhanXun97.base_stats["targets"] = 16f;
            ZhanXun97.base_stats["accuracy"] = 700f;
            ZhanXun97.base_stats["warfare"] = 700f;
            ZhanXun97.base_stats["stewardship"] = 700f;
            ZhanXun97.base_stats["armor"] = 700f;
            ZhanXun97.base_stats["attack_speed"] = 20f;
            ZhanXun97.base_stats["multiplier_attack_speed"] = 1f;
            ZhanXun97.base_stats["multiplier_damage"] = 7f;
            ZhanXun97.base_stats["multiplier_health"] = 7f;
            ZhanXun97.base_stats["area_of_effect"] = 7f;
            ZhanXun97.base_stats["range"] = 7f;
            ZhanXun97.action_special_effect += (WorldAction)Delegate.Combine(ZhanXun97.action_special_effect,
                new WorldAction(traitAction.ZhanXun97_effectAction));
            AssetManager.traits.add(ZhanXun97);

            ActorTrait ZhanXun98 = new ActorTrait();
            ZhanXun98.id = "ZhanXun98";
            ZhanXun98.path_icon = "trait/ZhanXun98";
            ZhanXun98.needs_to_be_explored = false;
            ZhanXun98.group_id = "ZhanXun";
            ZhanXun98.base_stats = new BaseStats();
            ZhanXun98.base_stats["lifespan"] = 10000f;
            ZhanXun98.base_stats["damage"] = 10000f;
            ZhanXun98.base_stats["health"] = 100000f;
            ZhanXun98.base_stats["targets"] = 20f;
            ZhanXun98.base_stats["accuracy"] = 1000f;
            ZhanXun98.base_stats["warfare"] = 1000f;
            ZhanXun98.base_stats["stewardship"] = 1000f;
            ZhanXun98.base_stats["armor"] = 1000f;
            ZhanXun98.base_stats["attack_speed"] = 20f;
            ZhanXun98.base_stats["multiplier_attack_speed"] = 1f;
            ZhanXun98.base_stats["multiplier_damage"] = 10f;
            ZhanXun98.base_stats["multiplier_health"] = 10f;
            ZhanXun98.base_stats["area_of_effect"] = 10f;
            ZhanXun98.base_stats["range"] = 10f;
            ZhanXun98.action_special_effect += (WorldAction)Delegate.Combine(ZhanXun98.action_special_effect,
                new WorldAction(traitAction.ZhanXun98_effectAction));
            AssetManager.traits.add(ZhanXun98);

            //猎王者
            ActorTrait HunterKing = new ActorTrait();
            HunterKing.id = "HunterKing";
            HunterKing.path_icon = "trait/HunterKing";
            HunterKing.needs_to_be_explored = false;
            HunterKing.group_id = "Diversification";
            HunterKing.base_stats = new BaseStats();
            HunterKing.base_stats["lifespan"] = 200f;
            HunterKing.base_stats["damage"] = 200f;
            HunterKing.base_stats["health"] = 2000f;
            HunterKing.base_stats["accuracy"] = 20f;
            HunterKing.base_stats["warfare"] = 20f;
            HunterKing.base_stats["stewardship"] = 20f;
            HunterKing.base_stats["targets"] = 8f;
            HunterKing.base_stats["armor"] = 20f;
            HunterKing.base_stats["attack_speed"] = 2f;
            HunterKing.base_stats["multiplier_damage"] = 2f;
            HunterKing.base_stats["multiplier_health"] = 2f;
            HunterKing.action_special_effect += (WorldAction)Delegate.Combine(HunterKing.action_special_effect,
                new WorldAction(traitAction.HunterKing_effectAction));
            AssetManager.traits.add(HunterKing);

            ActorTrait Diversification1 = new ActorTrait();
            Diversification1.id = "Diversification1";
            Diversification1.path_icon = "trait/Diversification1";
            Diversification1.needs_to_be_explored = false;
            Diversification1.group_id = "Diversification";
            Diversification1.base_stats = new BaseStats();
            Diversification1.rate_inherit = 100;
            Diversification1.base_stats["birth_rate"] = 100f;
            Diversification1.base_stats["offspring"] = 100f;
            AssetManager.traits.add(Diversification1);

        }
    }
}