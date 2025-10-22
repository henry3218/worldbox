using System;
using System.Collections.Generic;
using NCMS;
using UnityEngine;

namespace PeerlessOverpoweringWarrior.code
{
    /// <summary>
    /// 阵道法术效果类 - 负责注册所有阵道法术状态效果
    /// </summary>
    internal class FormationSpellEffect
    {
        /// <summary>
        /// 初始化所有阵道法术状态效果
        /// </summary>
        public static void Init()
        {
            try
            {
                // 金身阵
                StatusAsset Jinshen = new StatusAsset();
                Jinshen.id = "Formation_Jinshen";
                Jinshen.locale_id = "status_title_Formation_Jinshen";
                Jinshen.locale_description = "status_desc_Formation_Jinshen";
                Jinshen.path_icon = "trait/Formation_Jinshen";
                Jinshen.base_stats["armor"] = 15f;
                AssetManager.status.add(pAsset: Jinshen);

                // 罡盾阵
                StatusAsset Tieshen = new StatusAsset();
                Tieshen.id = "Formation_Tieshen";
                Tieshen.locale_id = "status_title_Formation_Tieshen";
                Tieshen.locale_description = "status_desc_Formation_Tieshen";
                Tieshen.path_icon = "trait/Formation_Tieshen";
                Tieshen.base_stats["armor"] = 30f;
                AssetManager.status.add(pAsset: Tieshen);

                // 规元阵
                StatusAsset Gangjia = new StatusAsset();
                Gangjia.id = "Formation_Gangjia";
                Gangjia.locale_id = "status_title_Formation_Gangjia";
                Gangjia.locale_description = "status_desc_Formation_Gangjia";
                Gangjia.path_icon = "trait/Formation_Gangjia";
                Gangjia.base_stats["armor"] = 60f;
                AssetManager.status.add(pAsset: Gangjia);

                // 劲风阵
                StatusAsset JinFeng = new StatusAsset();
                JinFeng.id = "Formation_JinFeng";
                JinFeng.locale_id = "status_title_Formation_JinFeng";
                JinFeng.locale_description = "status_desc_Formation_JinFeng";
                JinFeng.path_icon = "trait/Formation_JinFeng";
                JinFeng.base_stats["speed"] = 40f;
                JinFeng.base_stats["attack_speed"] = 4f;
                AssetManager.status.add(pAsset: JinFeng);

                // 神行阵
                StatusAsset Shenxing = new StatusAsset();
                Shenxing.id = "Formation_Shenxing";
                Shenxing.locale_id = "status_title_Formation_Shenxing";
                Shenxing.locale_description = "status_desc_Formation_Shenxing";
                Shenxing.path_icon = "trait/Formation_Shenxing";
                Shenxing.base_stats["speed"] = 100f;
                Shenxing.base_stats["attack_speed"] = 10f;
                AssetManager.status.add(pAsset: Shenxing);

                // 燃烧气血
                StatusAsset QIxue = new StatusAsset();
                QIxue.id = "Formation_QIxue";
                QIxue.locale_id = "status_title_Formation_QIxue";
                QIxue.locale_description = "status_desc_Formation_QIxue";
                QIxue.path_icon = "trait/Formation_QIxue";
                QIxue.base_stats["multiplier_damage"] = 0.3f;
                AssetManager.status.add(pAsset: QIxue);

                // 金刚护法身
                StatusAsset Dalishu = new StatusAsset();
                Dalishu.id = "Formation_Dalishu";
                Dalishu.locale_id = "status_title_Formation_Dalishu";
                Dalishu.locale_description = "status_desc_Formation_Dalishu";
                Dalishu.path_icon = "trait/Formation_Dalishu";
                Dalishu.base_stats["multiplier_damage"] = 0.5f;
                AssetManager.status.add(pAsset: Dalishu);

                // 天魔解体
                StatusAsset Tianmo = new StatusAsset();
                Tianmo.id = "Formation_Tianmo";
                Tianmo.locale_id = "status_title_Formation_Tianmo";
                Tianmo.locale_description = "status_desc_Formation_Tianmo";
                Tianmo.path_icon = "trait/Formation_Tianmo";
                Tianmo.base_stats["multiplier_damage"] = 1f;
                Tianmo.base_stats["armor"] = 80f;
                Tianmo.base_stats["speed"] = 160f;
                Tianmo.base_stats["attack_speed"] = 16f;
                AssetManager.status.add(pAsset: Tianmo);
                
                // 阵道·挪移阵（传送后增益效果）
                StatusAsset Teleport = new StatusAsset();
                Teleport.id = "Formation_Teleport";
                Teleport.locale_id = "status_title_Formation_Teleport";
                Teleport.locale_description = "status_desc_Formation_Teleport";
                Teleport.path_icon = "trait/Formation_Teleport"; // 使用FormationSkill2的图标
                Teleport.base_stats["attack_speed"] = 10f; // 传送后增加攻速
                Teleport.base_stats["speed"] = 50f; // 传送后增加移速
                AssetManager.status.add(pAsset: Teleport);

                Debug.Log("[武极] 阵道法术状态效果初始化完成");
            }
            catch (Exception ex)
            {
                Debug.LogError("[武极] 阵道法术状态效果初始化失败: " + ex.Message);
            }
        }
    }
}