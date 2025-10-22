using System;
using System.Collections.Generic;
using NCMS;
using UnityEngine;
using ReflectionUtility;

namespace PeerlessOverpoweringWarrior.code
{
    /// <summary>
    /// 阵道法术系统 - 负责管理和释放阵道法术
    /// </summary>
    internal class FormationSpellSystem
    {
        // 法术检查间隔（秒）
        private const float FORMATION_SPELL_CHECK_INTERVAL = 2f;
        private static float lastFormationSpellCheck = 0f;
        
        // 阵道法术列表
        public static List<FormationSpell> FormationSpells = new List<FormationSpell>()
        {
            // 阵道第四重境界（通地境）可施展的基础法术
            new FormationSpell("Formation_Jinshen", "阵道·金身阵", "Formation_Jinshen", 60f, 120f, 4, false),
            new FormationSpell("Formation_JinFeng", "阵道·劲风阵", "Formation_JinFeng", 60f, 120f, 4, false),
            new FormationSpell("Formation_Tieshen", "阵道·罡盾阵", "Formation_Tieshen", 60f, 120f, 4, true),
            
            // 阵道第五重境界（蕴枢境）可施展的高级法术
            new FormationSpell("Formation_Gangjia", "阵道·规元阵", "Formation_Gangjia", 75f, 180f, 5, true),
            new FormationSpell("Formation_QIxue", "阵道·气血燃烧", "Formation_QIxue", 50f, 120f, 5, true),
            new FormationSpell("Formation_Shenxing", "阵道·神行阵", "Formation_Shenxing", 60f, 120f, 5, false),
            new FormationSpell("Formation_Dalishu", "阵道·金刚阵", "Formation_Dalishu", 60f, 140f, 5, true),
            
            // 阵道第六重境界（玄真）可施展的终极法术
            new FormationSpell("Formation_Tianmo", "阵道·天神阵", "Formation_Tianmo", 30f, 240f, 6, true),
            
            // 阵道传送法术（拥有FormationSkill2特质的阵师可施展）
            new FormationSpell("Formation_Teleport", "阵道·挪移阵", "Formation_Teleport", 60f, 240f, 1, true) // minFormationLevel设为1，将在ProcessCasterSpells中特殊处理
        };
        
        // 冷却时间字典 - 使用弱引用避免内存泄漏
        private static readonly Dictionary<Actor, Dictionary<string, float>> formationSpellCooldowns = new Dictionary<Actor, Dictionary<string, float>>();
        
        /// <summary>
        /// 处理阵道法术释放逻辑
        /// </summary>
        /// <param name="pElapsed">经过的时间</param>
        public static void ProcessFormationSpells(float pElapsed)
        {
            // 累积时间并检查是否需要执行
            lastFormationSpellCheck += pElapsed;
            if (lastFormationSpellCheck < FORMATION_SPELL_CHECK_INTERVAL) 
                return;
            
            // 重置计时器
            lastFormationSpellCheck = 0f;
            
            try
            {
                // 按军队为单位处理，而不是遍历所有单位
                foreach (var army in World.world.armies)
                {
                    ProcessArmyFormationSpells(army);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("[武极] 处理阵道法术时出错: " + ex.Message);
            }
        }
        
        /// <summary>
        /// 处理单个军队的阵道法术逻辑
        /// </summary>
        /// <param name="army">军队对象</param>
        private static void ProcessArmyFormationSpells(Army army)
        {
            if (army == null || army.units.Count == 0)
                return;
            
            // 找出军队中的阵道施法者（境界4以上）
            List<Actor> formationCasters = new List<Actor>();
            foreach (var actor in army.units)
            {
                if (actor != null && actor.isAlive() && GetFormationLevel(actor) >= 4)
                {
                    formationCasters.Add(actor);
                }
            }
            
            // 如果没有施法者，直接返回
            if (formationCasters.Count == 0)
                return;
            
            // 检查军队中是否有战斗中的单位
            bool isArmyInCombat = IsArmyInCombat(army);
            
            // 为每个施法者处理法术
            foreach (var caster in formationCasters)
            {
                ProcessCasterSpells(caster, army.units, isArmyInCombat);
            }
        }
        
        /// <summary>
        /// 处理单个施法者的法术释放
        /// </summary>
        private static void ProcessCasterSpells(Actor caster, List<Actor> armyMembers, bool isArmyInCombat)
        {
            int formationLevel = GetFormationLevel(caster);
            
            // 获取或创建冷却时间字典
            if (!formationSpellCooldowns.ContainsKey(caster))
            {
                formationSpellCooldowns[caster] = new Dictionary<string, float>();
            }
            var cooldownDict = formationSpellCooldowns[caster];
            
            // 遍历可用法术
            foreach (var spell in FormationSpells)
            {
                // 特殊处理传送法术 - 检查是否拥有FormationSkill2特质
                if (spell.id == "Formation_Teleport")
                {
                    // 传送法术特殊要求：必须拥有FormationSkill2特质
                    if (!caster.hasTrait("FormationSkill2"))
                        continue;
                }
                else
                {
                    // 普通法术检查境界要求
                    if (formationLevel < spell.minFormationLevel)
                        continue;
                }
                
                // 检查冷却时间
                cooldownDict.TryGetValue(spell.id, out float cd);
                cd = Mathf.Max(0f, cd - FORMATION_SPELL_CHECK_INTERVAL);
                cooldownDict[spell.id] = cd;
                
                if (cd > 0f)
                    continue;
                
                // 检查战斗状态要求
                if (spell.combatOnly && !isArmyInCombat)
                    continue;
                
                // 随机决定是否释放（增加随机性）
                // 传送法术概率稍低，避免过于频繁
                float chance = (spell.id == "Formation_Teleport") ? 0.3f : 0.7f;
                if (Randy.randomChance(chance))
                {
                    ApplyFormationSpellEffect(caster, spell, armyMembers);
                    cooldownDict[spell.id] = spell.cooldown;
                }
            }
        }
        
        /// <summary>
        /// 应用阵道法术效果
        /// </summary>
        private static void ApplyFormationSpellEffect(Actor caster, FormationSpell spell, List<Actor> targets)
        {
            // 特殊处理传送法术
            if (spell.id == "Formation_Teleport")
            {
                ApplyTeleportSpell(caster, targets);
                return;
            }
            
            // 普通法术应用状态效果
            foreach (var target in targets)
            {
                if (target != null && target.isAlive() && IsAlly(caster, target))
                {
                    // 应用状态效果
                    target.addStatusEffect(spell.statusId, spell.duration, false);
                }
            }
        }
        
        /// <summary>
        /// 应用传送法术效果
        /// </summary>
        private static void ApplyTeleportSpell(Actor caster, List<Actor> armyMembers)
        {
            if (caster == null || !caster.hasCity()) return;
            
            // 寻找最近的敌方城市
            City nearestEnemyCity = FindNearestEnemyCity(caster);
            if (nearestEnemyCity == null) return;
            
            // 选择目标城市的一个随机区域中心
            var zone = nearestEnemyCity.zones.GetRandom<TileZone>();
            if (zone == null || zone.centerTile == null) return;
            
            var targetTile = zone.centerTile;
            
            // 传送所有军队成员
            foreach (var member in armyMembers)
            {
                if (member != null && member.isAlive() && IsAlly(caster, member))
                {
                    // 播放传送效果
                    ActionLibrary.teleportEffect(member, targetTile);
                    // 取消所有当前行为
                    member.cancelAllBeh();
                    // 传送到目标位置
                    member.spawnOn(targetTile, 0f);
                    // 传送后获得攻速加成（60秒）
                    member.addStatusEffect("Formation_Teleport", 60f, false);
                }
            }
            
            Debug.Log($"[武极] 阵师 {caster.name} 施展了阵道·乾坤变，天将雄师！");
        }
        
        /// <summary>
        /// 寻找最近的敌方城市
        /// </summary>
        private static City FindNearestEnemyCity(Actor caster)
        {
            if (caster == null || !caster.hasCity()) return null;
            
            City myCity = caster.getCity();
            Kingdom myKingdom = myCity?.kingdom;
            if (myCity == null || myKingdom == null) return null;
            
            City nearest = null;
            float bestDist = float.MaxValue;
            
            foreach (var city in World.world.cities)
            {
                if (city == null || city == myCity) continue;
                
                var enemyKingdom = city.kingdom;
                if (enemyKingdom != null && myKingdom.isEnemy(enemyKingdom))
                {
                    var zone = (city.zones != null && city.zones.Count > 0) ? city.zones[0] : null;
                    var tile = zone?.centerTile;
                    if (tile == null) continue;
                    
                    float dist = Toolbox.SquaredDistTile(caster.current_tile, tile);
                    if (dist < bestDist) 
                    {
                        bestDist = dist;
                        nearest = city;
                    }
                }
            }
            
            return nearest;
        }
        
        /// <summary>
        /// 检查两个单位是否是友方
        /// </summary>
        private static bool IsAlly(Actor actor1, Actor actor2)
        {
            if (actor1 == null || actor2 == null)
                return false;
                
            // 首先检查是否是同一个单位
            if (actor1 == actor2)
                return true;
                
            // 同一军队的视为友方
            if (actor1.army != null && actor1.army == actor2.army)
                return true;
                
            // 检查王国关系 - 如果双方都有王国，则确保不是敌人
            Kingdom kingdom1 = actor1.kingdom;
            Kingdom kingdom2 = actor2.kingdom;
            
            if (kingdom1 != null && kingdom2 != null)
            {
                // 如果双方属于同一王国，是友方
                if (kingdom1 == kingdom2)
                    return true;
                    
                // 如果双方是敌人，不是友方
                if (kingdom1.isEnemy(kingdom2))
                    return false;
            }
            
            // 默认情况下，如果没有明确的敌对关系，并且在同一军队，则视为友方
            return false;
        }
        
        /// <summary>
        /// 检查军队是否处于战斗状态
        /// </summary>
        private static bool IsArmyInCombat(Army army)
        {
            if (army == null)
                return false;
                
            foreach (var actor in army.units)
            {
                if (actor != null && actor.isAlive() && actor.isFighting())
                {
                    return true;
                }
            }
            
            return false;
        }
        
        /// <summary>
        /// 获取单位的阵道境界等级
        /// </summary>
        private static int GetFormationLevel(Actor actor)
        {
            if (actor == null || !actor.isAlive())
                return 0;
                
            // 检查是否有阵道境界特质
            if (actor.hasTrait("FormationRealm6")) return 6;
            if (actor.hasTrait("FormationRealm5")) return 5;
            if (actor.hasTrait("FormationRealm4")) return 4;
            if (actor.hasTrait("FormationRealm3")) return 3;
            if (actor.hasTrait("FormationRealm2")) return 2;
            if (actor.hasTrait("FormationRealm1")) return 1;
            
            return 0;
        }
    }
    
    /// <summary>
    /// 阵道法术定义类
    /// </summary>
    internal class FormationSpell
    {
        public string id;           // 法术ID
        public string name;         // 法术名称
        public string statusId;     // 对应状态效果ID
        public float duration;      // 持续时间
        public float cooldown;      // 冷却时间
        public int minFormationLevel; // 最低阵道境界要求（1-6）
        public bool combatOnly;     // 是否仅在战斗中释放
        
        public FormationSpell(string id, string name, string statusId, float duration, float cooldown, int minFormationLevel, bool combatOnly)
        {
            this.id = id;
            this.name = name;
            this.statusId = statusId;
            this.duration = duration;
            this.cooldown = cooldown;
            this.minFormationLevel = minFormationLevel;
            this.combatOnly = combatOnly;
        }
    }
}