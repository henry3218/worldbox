using System;
using System.Threading;
using NCMS;
using UnityEngine;
using ReflectionUtility;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;

namespace ChivalryZhanXun.code
{
    internal class SorceryEffect
    {
        public static void Init()
        {
            // 军队法术状态注册
            // 强身术
            StatusAsset Johnson = new StatusAsset();
            Johnson.id = "Johnson";
            Johnson.locale_id = "status_title_Johnson";
            Johnson.locale_description = "status_desc_Johnson";
            Johnson.path_icon = "Ring/Johnson";
            Johnson.base_stats["armor"] = 10f;
            AssetManager.status.add(pAsset:Johnson);

            // 护身术
            StatusAsset Selfde = new StatusAsset();
            Selfde.id = "Selfde";
            Selfde.locale_id = "status_title_Selfde";
            Selfde.locale_description = "status_desc_Selfde";
            Selfde.path_icon = "Ring/Selfde";
            Selfde.base_stats["armor"] = 20f;
            AssetManager.status.add(pAsset:Selfde);

            // 铁甲术
            StatusAsset Ironarmor = new StatusAsset();
            Ironarmor.id = "Ironarmor";
            Ironarmor.locale_id = "status_title_Ironarmor";
            Ironarmor.locale_description = "status_desc_Ironarmor";
            Ironarmor.path_icon = "Ring/Ironarmor";
            Ironarmor.base_stats["armor"] = 50f;
            AssetManager.status.add(pAsset:Ironarmor);

            // 不动明王
            StatusAsset TheUnmovingWiseKing = new StatusAsset();
            TheUnmovingWiseKing.id = "TheUnmovingWiseKing";
            TheUnmovingWiseKing.locale_id = "status_title_TheUnmovingWiseKing";
            TheUnmovingWiseKing.locale_description = "status_desc_TheUnmovingWiseKing";
            TheUnmovingWiseKing.path_icon = "Ring/TheUnmovingWiseKing";
            TheUnmovingWiseKing.base_stats["armor"] = 100f;
            AssetManager.status.add(pAsset:TheUnmovingWiseKing);

            // 治疗术（通用）
            StatusAsset heal = new StatusAsset();
            heal.id = "heal";
            heal.locale_id = "status_title_heal";
            heal.locale_description = "status_desc_heal";
            heal.path_icon = "Ring/heal";
            AssetManager.status.add(pAsset:heal);

            // 加速术
            StatusAsset Accelerate = new StatusAsset();
            Accelerate.id = "Accelerate";
            Accelerate.locale_id = "status_title_Accelerate";
            Accelerate.locale_description = "status_desc_Accelerate";
            Accelerate.path_icon = "Ring/Accelerate";
            Accelerate.base_stats["speed"] = 20f;
            Accelerate.base_stats["attack_speed"] = 1f;
            AssetManager.status.add(pAsset:Accelerate);

            // 疾风术
            StatusAsset Strongwind = new StatusAsset();
            Strongwind.id = "Strongwind";
            Strongwind.locale_id = "status_title_Strongwind";
            Strongwind.locale_description = "status_desc_Strongwind";
            Strongwind.path_icon = "Ring/Strongwind";
            Strongwind.base_stats["speed"] = 40f;
            Strongwind.base_stats["attack_speed"] = 2f;
            AssetManager.status.add(pAsset:Strongwind);

            // 御风术
            StatusAsset Yufeng = new StatusAsset();
            Yufeng.id = "Yufeng";
            Yufeng.locale_id = "status_title_Yufeng";
            Yufeng.locale_description = "status_desc_Yufeng";
            Yufeng.path_icon = "Ring/Yufeng";
            Yufeng.base_stats["speed"] = 100f;
            Yufeng.base_stats["attack_speed"] = 5f;
            AssetManager.status.add(pAsset:Yufeng);

            // 传送术
            StatusAsset teleport = new StatusAsset();
            teleport.id = "teleport";
            teleport.locale_id = "status_title_teleport";
            teleport.locale_description = "status_desc_teleport";
            teleport.path_icon = "Ring/teleport";
            teleport.base_stats["attack_speed"] = 10f;
            AssetManager.status.add(pAsset:teleport);

            // 力量强化
            StatusAsset enhancement = new StatusAsset();
            enhancement.id = "enhancement";
            enhancement.locale_id = "status_title_enhancement";
            enhancement.locale_description = "status_desc_enhancement";
            enhancement.path_icon = "Ring/enhancement";
            enhancement.base_stats["multiplier_damage"] = 0.1f;
            AssetManager.status.add(pAsset:enhancement);

            // 气血澎湃
            StatusAsset andblood = new StatusAsset();
            andblood.id = "andblood";
            andblood.locale_id = "status_title_andblood";
            andblood.locale_description = "status_desc_andblood";
            andblood.path_icon = "Ring/andblood";
            andblood.base_stats["multiplier_damage"] = 0.3f;
            AssetManager.status.add(pAsset:andblood);

            // 超载爆发
            StatusAsset Overload = new StatusAsset();
            Overload.id = "Overload";
            Overload.locale_id = "status_title_Overload";
            Overload.locale_description = "status_desc_Overload";
            Overload.path_icon = "Ring/Overload";
            Overload.base_stats["multiplier_damage"] = 1f;
            AssetManager.status.add(pAsset:Overload);

            // 煞气
            StatusAsset evilenergy_aura = new StatusAsset();
            evilenergy_aura.id = "evilenergy_aura";
            evilenergy_aura.locale_id = "status_title_evilenergy_aura";
            evilenergy_aura.locale_description = "status_desc_evilenergy_aura";
            evilenergy_aura.path_icon = "Ring/evilenergy_aura";
            evilenergy_aura.base_stats["multiplier_damage"] = 1f;
            AssetManager.status.add(pAsset:evilenergy_aura);
        }
    }
}