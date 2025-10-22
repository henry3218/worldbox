using UnityEngine;
using System;
using System.Collections.Generic;
using VideoCopilot.code;

namespace VideoCopilot.code.utils
{
    public static class ActorExtensions
    {
        private const string warrior_key = "wushu.warriorNum";
        private const string true_gang_key = "wushu.trueGangNum";
        private const string lifespan_key = "strings.S.lifespan";
        private const string true_gang_true_damage_multiplier_key = "wushu.trueGangTrueDamageMultiplier";
        private const string true_gang_heal_multiplier_key = "wushu.trueGangHealMultiplier";
        private const string true_gang_damage_reduction_multiplier_key = "wushu.trueGangDamageReductionMultiplier";
        private const string pattern_key = "wushu.patternNum";
        
        // 静态缓存，避免重复计算
        private static readonly float[] trueGangLimits = new float[]
        {
            0f,      // 占位
            50f,     // 1: 锻体境
            100f,    // 2: 炼骨境
            200f,    // 3: 通脉境
            400f,    // 4: 气海境(低)
            800f,    // 5: 气海境(高)
            1600f,   // 6: 化劲境
            3200f,   // 7: 凝罡境
            6400f,   // 8: 洞虚境
            12800f,  // 9: 劫身境
            25600f,  // 10: 武域境
            51200f,  // 11: 合道境
            102400f, // 12: 斩我境
            float.MaxValue // 13: 武极境 - 无上限
        };

        public static float GetWarrior(this Actor actor)
        {
            actor.data.get(warrior_key, out float val, 0);
            return val;
        }

        public static void SetWarrior(this Actor actor, float val)
        {
            actor.data.set(warrior_key, val);
        }

        public static void ChangeWarrior(this Actor actor, float delta)
        {
            actor.data.get(warrior_key, out float val, 0);
            val += delta;
            actor.data.set(warrior_key, Mathf.Max(0, val));
        }

        // 获取角色当前境界
        public static int GetWarriorLevel(this Actor actor)
        {
            // 从高境界到低境界检查，确保返回最高境界
            if (actor.hasTrait("Warrior93") || actor.hasTrait("Warrior93+")) // 武极境
                return 13;
            if (actor.hasTrait("Warrior92") || actor.hasTrait("Warrior92+")) // 斩我境
                return 12;
            if (actor.hasTrait("Warrior91") || actor.hasTrait("Warrior91+")) // 合道境
                return 11;
            if (actor.hasTrait("Warrior9") || actor.hasTrait("Warrior9+")) // 武域境
                return 10;
            if (actor.hasTrait("Warrior8") || actor.hasTrait("Warrior8+")) // 劫身境
                return 9;
            if (actor.hasTrait("Warrior7") || actor.hasTrait("Warrior7+")) // 洞虚境
                return 8;
            if (actor.hasTrait("Warrior6") || actor.hasTrait("Warrior6+")) // 凝罡境
                return 7;
            if (actor.hasTrait("Warrior5") || actor.hasTrait("Warrior5+")) // 化劲境
                return 6;
            if (actor.hasTrait("Warrior4") || actor.hasTrait("Warrior4+")) // 气海境(高)
                return 5;
            if (actor.hasTrait("Warrior3") || actor.hasTrait("Warrior3+")) // 气海境(低)
                return 4;
            if (actor.hasTrait("Warrior2") || actor.hasTrait("Warrior2+")) // 通脉境
                return 3;
            if (actor.hasTrait("Warrior1") || actor.hasTrait("Warrior1+")) // 炼骨境
                return 2;
            
            return 1; // 默认锻体境
        }

        // 获取对应境界的真罡上限（使用静态数组提高性能）
        public static float GetTrueGangLimitByLevel(int level)
        {
            if (level >= 1 && level < trueGangLimits.Length)
            {
                return trueGangLimits[level];
            }
            return 100f; // 默认锻体境上限
        }

        // 获取角色当前阵道境界
        public static int GetFormationLevel(this Actor actor)
        {
            // 从高境界到低境界检查，确保返回最高境界
            if (actor.hasTrait("FormationRealm6"))
                return 6;
            if (actor.hasTrait("FormationRealm5"))
                return 5;
            if (actor.hasTrait("FormationRealm4"))
                return 4;
            if (actor.hasTrait("FormationRealm3"))
                return 3;
            if (actor.hasTrait("FormationRealm2"))
                return 2;
            if (actor.hasTrait("FormationRealm1"))
                return 1;
            
            return 0; // 默认无阵道境界
        }

        // 真罡相关的扩展方法
        public static float GetTrueGang(this Actor actor)
        {
            actor.data.get(true_gang_key, out float val, 0);
            return val;
        }

        public static void SetTrueGang(this Actor actor, float val)
        {
            int level = actor.GetWarriorLevel();
            float limit = GetTrueGangLimitByLevel(level);
            val = Mathf.Max(0, Mathf.Min(val, limit));
            
            actor.data.set(true_gang_key, val);
            
            // 真罡值设置后，更新静态属性
            actor.CalculateTrueGangStaticProperties();
        }

        public static void ChangeTrueGang(this Actor actor, float delta)
        {
            actor.data.get(true_gang_key, out float val, 0);
            val += delta;
            
            int level = actor.GetWarriorLevel();
            float limit = GetTrueGangLimitByLevel(level);
            val = Mathf.Max(0, Mathf.Min(val, limit));
            
            actor.data.set(true_gang_key, val);
            
            // 真罡值变化后，更新静态属性
            actor.CalculateTrueGangStaticProperties();
        }

        // 真罡真伤倍数相关的扩展方法
        public static float GetTrueGangTrueDamageMultiplier(this Actor actor)
        {
            actor.data.get(true_gang_true_damage_multiplier_key, out float val, 0.5f); // 默认0.5倍
            return val;
        }

        public static void SetTrueGangTrueDamageMultiplier(this Actor actor, float val)
        {
            actor.data.set(true_gang_true_damage_multiplier_key, val);
        }

        // 真罡回血倍数相关的扩展方法
        public static float GetTrueGangHealMultiplier(this Actor actor)
        {
            actor.data.get(true_gang_heal_multiplier_key, out float val, 0.5f); // 默认0.5倍
            return val;
        }

        public static void SetTrueGangHealMultiplier(this Actor actor, float val)
        {
            actor.data.set(true_gang_heal_multiplier_key, val);
        }

        // 真罡伤害稀释倍数相关的扩展方法
        public static float GetTrueGangDamageReductionMultiplier(this Actor actor)
        {
            actor.data.get(true_gang_damage_reduction_multiplier_key, out float val, 1.0f); // 默认1倍（无稀释）
            return val;
        }

        public static void SetTrueGangDamageReductionMultiplier(this Actor actor, float val)
        {
            actor.data.set(true_gang_damage_reduction_multiplier_key, val);
        }

        // 计算并更新真罡相关静态属性
        public static void CalculateTrueGangStaticProperties(this Actor actor)
        {
            float trueGangValue = actor.GetTrueGang();
            
            // 设置默认倍数
            float trueDamageMultiplier = 0.5f;
            float healMultiplier = 0.5f;
            float damageReductionMultiplier = 1.0f;
            
            // 可以根据真罡值调整倍数（如果需要的话）
            // 这里保持与原逻辑一致
            
            actor.SetTrueGangTrueDamageMultiplier(trueDamageMultiplier);
            actor.SetTrueGangHealMultiplier(healMultiplier);
            actor.SetTrueGangDamageReductionMultiplier(damageReductionMultiplier);
        }

        // 阵纹相关的扩展方法
        public static float GetPattern(this Actor actor)
        {
            actor.data.get(pattern_key, out float val, 0);
            return val;
        }

        public static void SetPattern(this Actor actor, float val)
        {
            // 确保不会因为境界问题导致阵纹值被限制为0
            // 先获取当前阵道境界
            int level = actor.GetFormationLevel();
            
            // 如果没有阵道境界，默认使用第一境界的上限
            if (level == 0)
            {
                level = 1; // 至少使用第一境界的上限
            }
            
            float limit = GetPatternLimitByLevel(level);
            // 确保值在有效范围内
            val = Mathf.Max(0, Mathf.Min(val, limit));
            // 直接存储阵纹值
            actor.data.set(pattern_key, val);
        }

        public static void ChangePattern(this Actor actor, float delta)
        {
            // 获取当前阵纹值
            actor.data.get(pattern_key, out float val, 0);
            // 应用增量
            val += delta;
            
            // 确保不会因为境界问题导致阵纹值被限制为0
            int level = actor.GetFormationLevel();
            
            // 如果没有阵道境界，默认使用第一境界的上限
            if (level == 0)
            {
                level = 1; // 至少使用第一境界的上限
            }
            
            float limit = GetPatternLimitByLevel(level);
            // 确保值在有效范围内
            val = Mathf.Max(0, Mathf.Min(val, limit));
            
            // 存储更新后的阵纹值
            actor.data.set(pattern_key, val);
        }

        // 获取对应阵道境界的阵纹上限
        public static float GetPatternLimitByLevel(int level)
        {
            switch (level)
            {
                case 1: return 100f;      // 第一境界最多100
                case 2: return 500f;      // 第二境界最多500
                case 3: return 1000f;     // 第三境界最多1000
                case 4: return 3000f;     // 第四境界最多3000
                case 5: return 10000f;     // 第五境界最多1000
                case 6: return 9999999f;  // 最后一个境界最多9999999
                default: return 0f;
            }
        }

        // 添加物品到角色背包
        /// <summary>
        /// 给角色添加指定物品（兼容原版方法名）
        /// </summary>
        public static bool addItem(this Actor actor, string itemId, int count = 1)
        {
            if (actor == null || string.IsNullOrEmpty(itemId) || count <= 0)
                return false;

            try
            {
                // 首先尝试获取物品资源
                ItemAsset itemAsset = AssetManager.items.get(itemId);
                if (itemAsset == null)
                {
                    Debug.LogError($"物品未找到: {itemId}");
                    return false;
                }

                // 检查是否是装备类型物品
                EquipmentAsset equipmentAsset = itemAsset as EquipmentAsset;
                if (equipmentAsset != null && equipmentAsset.equipment_type == EquipmentType.Weapon)
                {
                    // 如果是武器，尝试装备到武器槽
                    if (actor.canEditEquipment() && actor.equipment != null)
                    {
                        ActorEquipmentSlot slot = actor.equipment.getSlot(equipmentAsset.equipment_type);
                        if (slot != null && slot.canChangeSlot())
                        {
                            // 生成物品实例并装备
                            Item item = World.world.items.generateItem(equipmentAsset, actor.kingdom, World.world.map_stats.player_name, count, actor, 0, true);
                            actor.equipment.setItem(item, actor);
                            Debug.Log($"成功给{actor.getName()}装备了武器: {itemId}");
                            return true;
                        }
                    }
                }

                // 对于非装备物品或无法装备的情况，使用字典存储方式
                string itemCountKey = $"item_count_{itemId}";
                actor.data.get(itemCountKey, out int currentCount, 0);
                actor.data.set(itemCountKey, currentCount + count);
                Debug.Log($"成功给{actor.getName()}添加了{count}个{itemId}");
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"添加物品失败: {ex.Message}");
                return false;
            }
        }
    }
}