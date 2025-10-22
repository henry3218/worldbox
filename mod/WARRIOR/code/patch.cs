using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using ai.behaviours;
using HarmonyLib;
using NeoModLoader.api.attributes;
using NeoModLoader.General;
using UnityEngine;
using ReflectionUtility;
using UnityEngine.UI;
using ai;
using System.Reflection;
using System.Reflection.Emit;
using VideoCopilot.code.utils;
using PeerlessOverpoweringWarrior.code.Config;
using life;

namespace PeerlessOverpoweringWarrior.code
{
    internal class patch
    {
        public static bool window_creature_info_initialized;
        private static bool emperorSealAcquired = false; // 全局标记是否已被获取
        private static bool slaughterKingSealAcquired = false; // 全局标记戮王印是否已被获取
        private static Dictionary<Actor, Actor> lastAttackers = new Dictionary<Actor, Actor>(); // 记录最后攻击者
        
        // 基础功法真罡增长范围静态映射表
        private static Dictionary<string, (int min, int max)> lowGongFaTrueGangRanges = new Dictionary<string, (int min, int max)>
        {
            {"LowGongFaTrait1", (1, 5)},    // 铁砂裂石手：较低增长
            {"LowGongFaTrait2", (2, 5)},    // 晨露清风诀：较低增长
            {"LowGongFaTrait3", (3, 6)},    // 铁布衫十三桩：中等增长
            {"LowGongFaTrait4", (2, 5)},    // 落叶步法：中等增长
            {"LowGongFaTrait5", (2, 7)},    // 赤阳燃木掌：较高增长
            {"LowGongFaTrait6", (3, 7)},    // 基础功法6：较高增长
            {"LowGongFaTrait7", (1, 5)},    // 基础功法7：较低增长
            {"LowGongFaTrait8", (3, 6)},    // 基础功法8：中等增长
            {"LowGongFaTrait9", (4, 8)}     // 基础功法9：较高增长
        };
        
        // 远古武体对武者属性的影响静态映射表
        private static readonly Dictionary<string, float> ancientMartialBodyWarriorChanges = new Dictionary<string, float>
        {
            {"ancientMartialBody1", 3.0f},
            {"ancientMartialBody2", 0.5f},
            {"ancientMartialBody3", 0.2f},
            {"ancientMartialBody4", 2.0f},
            {"ancientMartialBody5", 0.8f},
            {"ancientMartialBody6", 1.0f},
            {"ancientMartialBody7", 1.5f},
            {"ancientMartialBody8", 0.9f},
            {"ancientMartialBody9", 2.4f},
            {"ancientMartialBody91", 2.0f},
            {"ancientMartialBody92", 0.5f},
            {"ancientMartialBody93", 1.2f},
            {"ancientMartialBody94", 0.9f},
            {"ancientMartialBody95", 1.5f},
            {"ancientMartialBody96", 1.8f},
            {"ancientMartialBody97", 0.6f},
            {"ancientMartialBody98", 0.4f},
            {"ancientMartialBody99", 0.6f}
        };
        
        // 洞天对武者属性的影响静态映射表
        private static readonly Dictionary<string, float> celestialGrottoWarriorChanges = new Dictionary<string, float>
        {
            {"CelestialGrotto1", 2.0f},
            {"CelestialGrotto2", 3.0f},
            {"CelestialGrotto3", 4.0f},
            {"CelestialGrotto4", 2.0f},
            {"CelestialGrotto5", 4.0f},
            {"CelestialGrotto6", 1.5f},
            {"CelestialGrotto7", 4.0f},
            {"CelestialGrotto8", 3.0f},
            {"CelestialGrotto9", 5.0f},
            {"CelestialGrotto10", 2.0f},
            {"CelestialGrotto11", 1.5f},
            {"CelestialGrotto12", 2.0f},
            {"CelestialGrotto13", 3.0f},
            {"CelestialGrotto14", 1.5f},
            {"CelestialGrotto15", 1.5f},
            {"CelestialGrotto16", 1.5f},
            {"CelestialGrotto17", 1.5f},
            {"CelestialGrotto18", 3.0f}
        };
        
        // 九宫秘录对武者属性的影响静态映射表
        private static readonly Dictionary<string, float> NineCharacterSecretWarriorChanges = new Dictionary<string, float>
        {
            {"NineCharacterSecret1", 2.0f},
            {"NineCharacterSecret2", 1.0f},
            {"NineCharacterSecret3", 2.0f},
            {"NineCharacterSecret4", 4.0f},
            {"NineCharacterSecret5", 1.0f},
            {"NineCharacterSecret6", 5.0f},
            {"NineCharacterSecret7", 3.0f},
            {"NineCharacterSecret8", 2.0f},
            {"NineCharacterSecret9", 6.0f}
        };
        
        // 中级功法对武者属性的影响静态映射表
        private static readonly Dictionary<string, float> midGongFaWarriorChanges = new Dictionary<string, float>
        {
            {"MidGongFaTrait14", 1.0f} // 新增基础功法：气血+1
        };
        
        // 功法对武者属性的影响静态映射表
        private static readonly Dictionary<string, float> GongFaWarriorChanges = new Dictionary<string, float>
        {
            {"GongFaTrait14", 2.0f}
        };
        
        // 秘典对武者属性的影响静态映射表
        private static readonly Dictionary<string, float> arcaneTomeWarriorChanges = new Dictionary<string, float>
        {
            {"arcaneTome14", 4.0f}
        };
        
        // 帝印对武者属性的影响静态映射表
        private static readonly Dictionary<string, float> EmperorSealWarriorChanges = new Dictionary<string, float>
        {
            {"EmperorSeal", 500f}
        };
        
        // 根骨特质对武者气血的影响静态映射表
        private static readonly Dictionary<string, (float, float)> martialAptitudeWarriorChanges = new Dictionary<string, (float, float)>
        {
            { "martialAptitude1", (1.0f, 2.0f) },//下
            { "martialAptitude2", (2.0f, 4.0f) },//中
            { "martialAptitude3", (4.0f, 6.0f) },//上
            { "martialAptitude4", (6.0f, 9.0f) },//龙筋虎骨
            { "martialAptitude10", (-1.0f, -2.0f) },//英雄迟暮
            { "martialAptitude7", (9.0f, 12.0f) },//真武转世
            { "martialAptitude8", (12.0f, 16.0f) },//天生至尊
        };

        // 阵法天赋对阵纹属性的影响静态映射表
        private static readonly Dictionary<string, (int, int)> formationPatternChanges = new Dictionary<string, (int, int)>
        {
            { "formation1", (1, 3) },    // 下等：每年1-3点
            { "formation2", (3, 6) },    // 中等：每年3-6点
            { "formation3", (6, 9) },    // 上等：每年6-9点
            { "formation4", (9, 12) }    // 绝顶：每年9-12点
        };
        
        // 气血阈值与特质获取的静态映射表
        private static readonly Dictionary<string, float> WarriorTraitThresholds = new Dictionary<string, float>
        {
            { "Warrior1", 60f },
            { "Warrior2", 70f },
            { "Warrior3", 80f },
            { "Warrior4", 100f },
            { "Warrior5", 120f },
            { "Warrior6", 160f },
            { "Warrior7", 200f },
            { "Warrior8", 260f },
            { "Warrior9", 380f },
            { "Warrior91", 440f },
            { "Warrior92", 1200f },
            { "Warrior93", 8000f }
        };

        // 阵道各境界阵纹上限的静态映射表
        private static readonly Dictionary<string, float> FormationThresholds = new Dictionary<string, float>
        {
            { "FormationRealm1", 100f },
            { "FormationRealm2", 500f },
            { "FormationRealm3", 1000f },
            { "FormationRealm4", 3000f },
            { "FormationRealm5", 10000f },
            { "FormationRealm6", 9999999f }
        };
        
        // 境界对应的气血上限静态映射表
        private static readonly Dictionary<string, float> grades = new Dictionary<string, float>
        {
            { "Warrior1", 10f },
            { "Warrior2", 20f },
            { "Warrior3", 40f },
            { "Warrior4", 80f },
            { "Warrior5", 160f },
            { "Warrior6", 300f },
            { "Warrior7", 500f },
            { "Warrior8", 800f },
            { "Warrior9", 1200f },
            { "Warrior91", 3600f },
            { "Warrior92", 12000f },
            { "Warrior93", 9999999f },
        };

        // 在军队更新循环中调用阵道法术释放逻辑
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ArmyManager), nameof(ArmyManager.update))]
        public static void ArmyManager_update_Postfix(float pElapsed)
        {
            FormationSpellSystem.ProcessFormationSpells(pElapsed);
        }
        
        // =============================
        // Patch：让阵道第四境界以上的阵师直接入伍
        // =============================
        [HarmonyPrefix]
        [HarmonyPatch(typeof(City), "setCitizenJob")]
        public static bool City_setCitizenJob_FormationMaster_Prefix(City __instance, Actor pActor)
        {
            // 前置条件检查
            if (pActor == null || pActor.city != __instance || pActor.city == null || pActor.city.kingdom != __instance.kingdom)
                return true;
            if (pActor.isKing() || pActor.isCityLeader())
                return true;
            if (pActor.getCity() != __instance)
                return true;
            if (pActor.army != null && pActor.army.getCity() != __instance)
                return true;
            if (pActor.city != null && pActor.city.kingdom != __instance.kingdom)
                return true;
            
            // 检查是否是阵道第四境界以上的阵师
            bool isFormationMaster = pActor.hasTrait("FormationRealm4") || 
                                    pActor.hasTrait("FormationRealm5") || 
                                    pActor.hasTrait("FormationRealm6");
            
            // 如果是阵道大师且可以成为战士，则直接让其入伍
            if (isFormationMaster && __instance.checkCanMakeWarrior(pActor))
            {
                __instance.makeWarrior(pActor);
                // 设置计时器以避免频繁尝试
                SetTimerWarrior(__instance, 30f);
                return false; // 阻止默认的工作分配逻辑
            }
            
            // 允许其他单位的默认工作分配逻辑
            return true;
        }
        
        // 计时器相关辅助方法 - 复用现有逻辑
        private static Dictionary<City, float> warriorTimers = new Dictionary<City, float>();
        
        private static void SetTimerWarrior(City city, float time)
        {
            if (warriorTimers.ContainsKey(city))
                warriorTimers[city] = time;
            else
                warriorTimers.Add(city, time);
        }
        
        private static float GetTimerWarrior(City city)
        {
            if (warriorTimers.ContainsKey(city))
                return warriorTimers[city];
            return 0f;
        }
        
        private static int IncAndGetLastCheckedJobId(City city, int maxJobs)
        {
            // 简单实现，返回0作为默认值
            // 实际项目中可能需要更复杂的逻辑来跟踪每个城市的最后检查的工作ID
            return 0;
        }
        
        [HarmonyPostfix]
        [HarmonyPatch(typeof(UnitWindow), nameof(UnitWindow.OnEnable))]
        private static void WindowCreatureInfo_OnEnable_postfix(UnitWindow __instance)
        {
            if (__instance.actor == null || !__instance.actor.isAlive())
                return;
            // 添加延迟初始化
            __instance.StartCoroutine(DelayedInit(__instance));
        }

        // 修改协程声明为正确的非泛型IEnumerator
        private static IEnumerator DelayedInit(UnitWindow window)
        {
            // 等待一帧确保原生UI完成初始化
            yield return null;

            if (!window_creature_info_initialized)
            {
                UnitWindowStatsIcon.Initialize(window);
                window_creature_info_initialized = true;
            }

            UnitWindowStatsIcon.OnEnable(window, window.actor);
        }

        [HarmonyPrefix, HarmonyPatch(typeof(MapAction), "checkLightningAction")]
        public static bool checkLightningAction(Vector2Int pPos, int pRad)
        {
            bool flag = false;
            List<Actor> simpleList = World.world.units.getSimpleList();
            for (int i = 0; i < simpleList.Count; i++)
            {
                Actor actor = simpleList[i];
                if (Toolbox.DistVec2(actor.current_tile.pos, pPos) <= (float)pRad)
                {
                    if (actor.asset.flag_finger)
                    {
                        actor.getActorComponent<GodFinger>().lightAction();
                    }
                    else
                    {
                        if (!flag && !actor.hasTrait("immortal") && Randy.randomChance(0.2f))
                        {
                            flag = true;
                        }

                        SignalAsset check_achievement_may_i_interrupt = AchievementLibrary.may_i_interrupt.getSignal();
                        BehaviourTaskActor task = actor.ai.task;
                        SignalManager.add(check_achievement_may_i_interrupt, (task != null) ? task.id : null);
                    }
                }
            }
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(Actor), "makeStunned")]
        public static bool makeStunned(Actor __instance, float pTime = 5f)
        {
            pTime += Randy.randomFloat(0f, pTime * 0.1f);
            __instance.cancelAllBeh();
            __instance.makeWait(pTime);
            // 如果没有防火特质，则添加眩晕状态效果，否则不添加眩晕状态效果
            if (!__instance.hasTrait("fire_proof"))
            {
                if (__instance.addStatusEffect("stunned", pTime, true))
                {
                    __instance.finishAngryStatus();
                }
            }
            return false;
        }

        // 单独的真罡效果应用方法，提高代码复用性和可读性
        private static void ApplyTrueGangEffects(Actor attackerActor, Actor targetActor, ref float damage)
        {
            // 避免在短时间内重复计算静态属性
            // 注意：Actor.data可能不直接支持存储lastTrueGangCalcTime，
            // 这里保持简单的实现，移除了时间缓存机制以避免兼容性问题
            attackerActor.CalculateTrueGangStaticProperties();
            targetActor.CalculateTrueGangStaticProperties();
            
            float attackerTrueGang = attackerActor.GetTrueGang();
            float targetTrueGang = targetActor.GetTrueGang();
            
            // 如果双方都没有真罡，则不进行真罡相关计算
            if (attackerTrueGang <= 0 && targetTrueGang <= 0)
                return;
            
            float trueGangDiff = attackerTrueGang - targetTrueGang;

            // 1. 真罡差值产生的效果：真伤和回血
            if (trueGangDiff > 0)
            {
                // 使用静态属性获取真伤和回血倍数
                float trueDamageMultiplier = attackerActor.GetTrueGangTrueDamageMultiplier();
                float healMultiplier = attackerActor.GetTrueGangHealMultiplier();
                
                // 差值乘以静态倍数转化为真伤
                float trueDamage = trueGangDiff * trueDamageMultiplier;
                // 差值乘以静态倍数转化为回血
                float healAmount = trueGangDiff * healMultiplier;

                // 施加真伤（无视防御）
                if (targetActor.data.health > trueDamage)
                {
                    targetActor.restoreHealth(-Mathf.RoundToInt(trueDamage));
                }
                else
                {
                    // 如果伤害超过目标血量，直接扣除剩余血量
                    targetActor.restoreHealth(-targetActor.data.health);
                }

                // 为攻击者回血
                float maxHealth = attackerActor.stats[strings.S.health];
                float currentHealth = attackerActor.data.health;
                float healthDiff = maxHealth - currentHealth;
                float actualHeal = Mathf.Min(healAmount, healthDiff);
                attackerActor.restoreHealth(Mathf.RoundToInt(actualHeal));
            }

            // 2. 真罡倍数影响伤害稀释 - 使用静态属性
            float damageReductionMultiplier = 1.0f;
            
            // 如果目标没有真罡，攻击者伤害自动稀释十倍
            if (targetTrueGang <= 0)
            {
                damageReductionMultiplier = 10f;
            }
            // 如果目标有真罡且比攻击者高
            else if (targetTrueGang > attackerTrueGang && attackerTrueGang > 0)
            {
                // 计算目标真罡比攻击者高的倍数
                damageReductionMultiplier = targetTrueGang / attackerTrueGang;
            }
            
            // 应用伤害稀释倍数
            damage = damage / damageReductionMultiplier;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(ActionLibrary), "addStunnedEffectOnTarget20")]
        // 添加眩晕效果到目标（20%概率）
        public static bool addStunnedEffectOnTarget20_Patch(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            // 检查目标是否有效（未死亡、非建筑）
            if (pTarget.isRekt() || pTarget.isBuilding())
            {
                return false;
            }

            // 检查随机概率（20%）
            if (!Randy.randomChance(0.2f))
            {
                return false;
            }

            // 检查目标是否具有防火特质
            if (pTarget.isActor() && pTarget.a.hasTrait("fire_proof"))
            {
                return false;
            }

            // 调用眩晕方法
            return ActionLibrary.addStunnedEffectOnTarget(pSelf, pTarget, pTile);
        }

        [HarmonyPrefix, HarmonyPatch(typeof(Actor), "getHit")]
        public static bool actorGetHit_prefix(
            Actor __instance,
            ref float pDamage,
            bool pFlash,
            AttackType pAttackType,
            BaseSimObject pAttacker,
            bool pSkipIfShake,
            bool pMetallicWeapon,
            bool pCheckDamageReduction = true)
        {
            __instance._last_attack_type = pAttackType;
            // 闪避机制
            if (pAttacker is Actor attackerActor && __instance.isAlive())
            {
                float attackerAccuracy = attackerActor.stats["Accuracy"];
                float targetDodge = __instance.stats["Dodge"];
                float effectiveDodge = Mathf.Clamp(targetDodge - attackerAccuracy, 0f, 100f);

                if (Randy.randomChance(effectiveDodge / 100f))
                {
                    __instance.startColorEffect(ActorColorEffect.White);
                    return false;
                }

                // 应用真罡效果
                ApplyTrueGangEffects(attackerActor, __instance, ref pDamage);
            }

            // 记录最后攻击者
            if (pAttacker is Actor attacker && __instance.isActor())
            {
                if (lastAttackers.ContainsKey(__instance))
                {
                    lastAttackers[__instance] = attacker;
                }
                else
                {
                    lastAttackers.Add(__instance, attacker);
                }
            }

            // 死亡处理
            if (!__instance.hasHealth())
            {
                __instance.batch.c_check_deaths.Add(__instance);
            }

            return true;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(ActorTool), "applyForceToUnit")]
        public static bool ApplyForceToUnit_Postfix(Actor __instance, AttackData pData, BaseSimObject pTargetToCheck, float pMod = 1f, bool pCheckCancelJobOnLand = false)
        {
            float tForce = pData.knockback * pMod;

            if (tForce > 0f && pTargetToCheck.isActor())
            {
                float resistValue = pTargetToCheck.a.stats["Resist"];
                tForce = Mathf.Max(tForce - resistValue, 0);

                if (tForce > 0f)
                {
                    Vector2 tPosStart = pTargetToCheck.cur_transform_position;
                    Vector2 tAttackVec = pData.hit_position;
                    pTargetToCheck.a.calculateForce(
                        tPosStart.x, tPosStart.y,
                        tAttackVec.x, tAttackVec.y,
                        tForce,
                        0f,
                        pCheckCancelJobOnLand
                    );
                }
            }
            return false;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(Actor), "updateAge")]
        public static void updateAge_Postfix(Actor __instance)
        {

            if (__instance == null)
            {
                return;
            }

            float age = (float)__instance.getAge();

            // 真罡增长机制：从凝罡境(Warrior6)开始，根据基础功法类型每年获得不同范围的真罡
            bool hasTrueGang = false;
            // 检查是否拥有凝罡境或更高级别的特质
            for (int i = 6; i <= 9; i++)
            {
                if (__instance.hasTrait($"Warrior{i}") || __instance.hasTrait($"Warrior{i}+"))
                {
                    hasTrueGang = true;
                    break;
                }
            }
            
            // 检查高级境界(Warrior91-93)
            if (!hasTrueGang)
            {
                for (int i = 91; i <= 93; i++)
                {
                    if (__instance.hasTrait($"Warrior{i}") || __instance.hasTrait($"Warrior{i}+"))
                    {
                        hasTrueGang = true;
                        break;
                    }
                }
            }
            
            // 如果是整数年龄（代表过了完整的一年）且拥有凝罡境或更高级别特质
            if (hasTrueGang && Mathf.FloorToInt(age) == age)
            {
                // 使用静态的基础功法真罡增长范围映射表（已在类初始化时创建一次）
                
                int trueGangGain = 1; // 默认最小值
                bool hasLowGongFa = false;
                
                // 检查是否拥有基础功法，并根据功法类型决定真罡增长
                foreach (var (trait, range) in lowGongFaTrueGangRanges)
                {
                    if (__instance.hasTrait(trait))
                    {
                        // 在范围内随机取一个值
                        trueGangGain = UnityEngine.Random.Range(range.min, range.max + 1);
                        hasLowGongFa = true;
                        break;
                    }
                }
                
                // 如果没有任何基础功法，使用最低增长范围
                if (!hasLowGongFa)
                {
                    trueGangGain = UnityEngine.Random.Range(1, 3);
                }
                
                __instance.ChangeTrueGang(trueGangGain); // 使用ChangeTrueGang方法自动更新静态属性
            }
            
            // 戮王印每年增加300真罡
            if (Mathf.FloorToInt(age) == age && __instance.hasTrait("SlaughterKingSeal"))
            {
                __instance.ChangeTrueGang(+300); // 使用ChangeTrueGang方法增加真罡
            }
            
            bool hasmartialAptitude10 = __instance.hasTrait("martialAptitude10");// 检查是否具有特质
            bool hasmartialAptitude9 = __instance.hasTrait("martialAptitude9");
            bool hasCongenitalPerfection = __instance.hasTrait("congenitalPerfection"); // 检查是否拥有"先天圆满"特质

            bool hasObtainedarcaneTome = false;

            // 定义基本种族集合
            var basicRaces = new HashSet<string> { "human", "elf", "dwarf", "orc"};

            // 检查是否已拥有任何阵法天赋
            bool hasAnyFormationTrait = __instance.hasTrait("formation1") || 
                                      __instance.hasTrait("formation2") || 
                                      __instance.hasTrait("formation3") || 
                                      __instance.hasTrait("formation4");

            // 5岁时觉醒阵法天赋
            if (Mathf.FloorToInt(age) == 5 && !hasAnyFormationTrait &&
                (basicRaces.Contains(__instance.asset.id) || __instance.asset.id.StartsWith("civ_")) &&
                WarriorConfig.AllowFormationTalentAwakening) // 检查是否允许阵道天赋觉醒
            {
                // 检查是否拥有武道根骨
                bool hasMartialAptitude = __instance.hasTrait("martialAptitude1") ||
                                         __instance.hasTrait("martialAptitude2") ||
                                         __instance.hasTrait("martialAptitude3") ||
                                         __instance.hasTrait("martialAptitude4") ||
                                         __instance.hasTrait("martialAptitude7") ||
                                         __instance.hasTrait("martialAptitude8");

                // 根据是否拥有武道根骨决定觉醒概率
                float awakenChance = hasMartialAptitude ? 0.001f : 0.01f; // 有根骨：千分之一，无根骨：百分之一

                if (Randy.randomChance(awakenChance))
                {
                    var formationWeights = new (string traitId, float weight)[]
                    {
                        ("formation1", 70f),    // 下等
                        ("formation2", 25f),    // 中等
                        ("formation3", 4.5f),   // 上等
                        ("formation4", 0.5f)    // 绝顶
                    };

                    // 计算总权重
                    float totalWeight = formationWeights.Sum(t => t.weight);

                    // 生成随机浮点数
                    float randomValue = UnityEngine.Random.Range(0f, totalWeight);

                    // 遍历权重选择特质
                    string selectedFormation = "formation1"; // 默认值
                    foreach (var formation in formationWeights)
                    {
                        if (randomValue < formation.weight)
                        {
                            selectedFormation = formation.traitId;
                            break;
                        }
                        randomValue -= formation.weight;
                    }

                    __instance.addTrait(selectedFormation, false);
                }
            }

            // 阵纹属性增长逻辑（每年增长）
            if (Mathf.FloorToInt(age) == age)
            {
                foreach (var change in formationPatternChanges)
                {
                    if (__instance.hasTrait(change.Key))
                    {
                        int randomPatternIncrease = UnityEngine.Random.Range(change.Value.Item1, change.Value.Item2 + 1);
                        __instance.ChangePattern(randomPatternIncrease);
                        break; // 只应用一个阵法天赋的效果
                    }
                }
            }

            // 检查是否拥有“武祖赐福”特质
            bool hasMartialAptitude8 = __instance.hasTrait("martialAptitude8");
            if (hasMartialAptitude8)
            {
                if (!hasObtainedarcaneTome)
                {
                    string[] arcaneTome = {
                        "arcaneTome91"
                    };

                    int randomIndex = UnityEngine.Random.Range(0, arcaneTome.Length);
                    string selectedarcaneTome = arcaneTome[randomIndex];

                    if (!__instance.hasTrait(selectedarcaneTome))
                    {
                        __instance.addTrait(selectedarcaneTome);
                        hasObtainedarcaneTome = true;
                    }
                }

                __instance.data.favorite = true;
            }
            
            // 检查是否拥有“周天阵域”特质（阵道高级天赋），同样设置为自动收藏
            bool hasFormation4 = __instance.hasTrait("formation4");
            if (hasFormation4)
            {
                __instance.data.favorite = true;
            }

            // 检查是否拥有混沌帝血特质
            bool hasmartialBloodline7 = __instance.hasTrait("martialBloodline7");
            if (hasmartialBloodline7)
            {
                // 直接觉醒玄玉根骨
                string martialAptitude4Trait = "martialAptitude4";
                // 定义根骨特质数组
                string[] martialAptitudeTraits = {
                    "martialAptitude1",
                    "martialAptitude2",
                    "martialAptitude3",
                    "martialAptitude4",
                    "martialAptitude7",
                    "martialAptitude8",
                    "martialAptitude9",
                    "martialAptitude10"
                };

                // 检查是否已拥有任何根骨特质
                bool hasAnyMartialAptitude = false;
                foreach (string trait in martialAptitudeTraits)
                {
                    if (__instance.hasTrait(trait))
                    {
                        hasAnyMartialAptitude = true;
                        break;
                    }
                }

                // 只有在未拥有任何根骨特质时才觉醒玄玉根骨
            if (!hasAnyMartialAptitude && !__instance.hasTrait("martialAptitude4") &&
                WarriorConfig.AllowWarriorRootAwakening) // 检查是否允许武道根骨觉醒
                {
                    __instance.addTrait("martialAptitude4");
                }
            }

            // 出生时（年龄为1）有一定概率获得"先天圆满"特质
            if (Mathf.FloorToInt(age) == 1 && !hasCongenitalPerfection && Randy.randomChance(0.03f)) // 3% 的概率
            {
                __instance.addTrait("congenitalPerfection", false);
            }

            // 4岁时获得martialAptitude1 - martialAptitude10的随机特质
            if (Mathf.FloorToInt(age) == 4 && !hasmartialAptitude9 && !hasmartialAptitude10 &&
                !HasAnyFlairTalen(__instance) &&
                (basicRaces.Contains(__instance.asset.id) || __instance.asset.id.StartsWith("civ_")) &&
                WarriorConfig.AllowWarriorRootAwakening) // 检查是否允许武道根骨觉醒
            {

                // 如果已经拥有玄玉根骨（因混沌帝血获得），则跳过此次随机根骨觉醒
                if (__instance.hasTrait("martialAptitude4"))
                {
                    return;
                }

                float bloodlineBonus = 0f;
                if (__instance.hasTrait("martialBloodline1"))
                {
                    bloodlineBonus = 0.2f; // 一级血脉提高20%的概率
                }
                else if (__instance.hasTrait("martialBloodline2"))
                {
                    bloodlineBonus = 0.3f; // 二级血脉提高30%的概率
                }
                else if (__instance.hasTrait("martialBloodline3"))
                {
                    bloodlineBonus = 0.4f; // 三级血脉提高40%的概率
                }
                else if (__instance.hasTrait("martialBloodline4"))
                {
                    bloodlineBonus = 0.5f; // 四级血脉提高50%的概率
                }
                else if (__instance.hasTrait("martialBloodline5"))
                {
                    bloodlineBonus = 0.6f; // 五级血脉提高60%的概率
                }
                else if (__instance.hasTrait("martialBloodline6"))
                {
                    bloodlineBonus = 0.7f; // 六级血脉提高70%的概率
                }

                if (Randy.randomChance(0.2f + bloodlineBonus))
                {
                    var martialAptitudeWeights = new (string traitId, float weight)[]
                    {
                        ("martialAptitude1", 64f),
                        ("martialAptitude2", 31f),
                        ("martialAptitude3", 4.58f),
                        ("martialAptitude4", 0.33f),
                        ("martialAptitude7", 0.076f),
                        ("martialAptitude8", 0.014f),
                    };

                    // 计算总权重
                    float totalWeight = martialAptitudeWeights.Sum(t => t.weight);

                    // 生成随机浮点数
                    float randomValue = UnityEngine.Random.Range(0f, totalWeight);

                    // 遍历权重选择特质
                    string selectedmartialAptitude = "martialAptitude1"; // 默认值
                    foreach (var martialAptitude in martialAptitudeWeights)
                    {
                        if (randomValue < martialAptitude.weight)
                        {
                            selectedmartialAptitude = martialAptitude.traitId;
                            break;
                        }
                        randomValue -= martialAptitude.weight;
                    }

                    __instance.addTrait(selectedmartialAptitude, false);
                }
            }

            // 使用静态的远古武体对武者属性的影响映射表

            foreach (var ancientMartialBody in ancientMartialBodyWarriorChanges)
            {
                if (__instance.hasTrait(ancientMartialBody.Key))
                {
                    float ancientMartialBodyBonus = ancientMartialBody.Value;
                    __instance.ChangeWarrior(ancientMartialBodyBonus);
                }
            }

            // 使用静态的洞天对武者属性的影响映射表

            foreach (var grotto in celestialGrottoWarriorChanges)
            {
                if (__instance.hasTrait(grotto.Key))
                {
                    float grottoBonus = grotto.Value;
                    __instance.ChangeWarrior(grottoBonus);
                }
            }

            // 使用静态的九宫秘录对武者属性的影响映射表

            foreach (var NineCharacterSecret in NineCharacterSecretWarriorChanges)
            {
                if (__instance.hasTrait(NineCharacterSecret.Key))
                {
                    float NineCharacterSecretBonus = NineCharacterSecret.Value;
                    __instance.ChangeWarrior(NineCharacterSecretBonus);
                }
            }

            // 使用静态的中级功法对武者属性的影响映射表
            // 新增：基础功法增幅逻辑
            foreach (var midGongFa in midGongFaWarriorChanges)
            {
                if (__instance.hasTrait(midGongFa.Key))
                {
                    __instance.ChangeWarrior(midGongFa.Value);
                }
            }

            // 使用静态的功法对武者属性的影响映射表
            foreach (var GongFa in GongFaWarriorChanges)
            {
                if (__instance.hasTrait(GongFa.Key))
                {
                    __instance.ChangeWarrior(GongFa.Value);
                }
            }

            // 使用静态的秘典对武者属性的影响映射表
            foreach (var arcaneTome in arcaneTomeWarriorChanges)
            {
                if (__instance.hasTrait(arcaneTome.Key))
                {
                    __instance.ChangeWarrior(arcaneTome.Value);
                }
            }

            // 使用静态的帝印对武者属性的影响映射表
            foreach (var EmperorSeal in EmperorSealWarriorChanges)
            {
                if (__instance.hasTrait(EmperorSeal.Key))
                {
                    __instance.ChangeWarrior(EmperorSeal.Value);
                }
            }

            // 使用静态的根骨特质对武者气血的影响映射表

            foreach (var change in martialAptitudeWarriorChanges)
            {
                // 如果具有martialAptitude10特质，并且当前特质是martialAptitude1到martialAptitude4，则跳过
                if ((hasmartialAptitude10 || hasmartialAptitude9) && (change.Key == "martialAptitude1" || change.Key == "martialAptitude2" || change.Key == "martialAptitude3" || change.Key == "martialAptitude4"))
                {
                    continue;
                }

                if (__instance.hasTrait(change.Key))
                {
                    float randomWarriorIncrease = UnityEngine.Random.Range(change.Value.Item1, change.Value.Item2);
                    __instance.ChangeWarrior(randomWarriorIncrease);
                }
            }

            // 使用静态的气血阈值与特质获取的映射表
            const float martialAptitude10Chance = 0.1f;
            foreach (var threshold in WarriorTraitThresholds)
            {
                if (__instance.hasTrait(threshold.Key) && age > threshold.Value && Randy.randomChance(martialAptitude10Chance) && !hasCongenitalPerfection)
                {
                    __instance.addTrait("martialAptitude10", false);
                }
            }

            // 使用静态的境界对应的气血上限映射表
            foreach (var grade in grades)
            {
                UpdateWarriorBasedOnGrade(__instance, grade.Key, grade.Value);
            }

            // 阵道境界晋升检查已移至traitAction中实现，并通过特质的action_special_effect触发
            // CheckFormationBreakthrough(__instance);

            // 检查是否已经有人获得帝印
            if (!emperorSealAcquired)
            {
                // 检查气血值是否达到120000
                if (__instance.GetWarrior() >= 120000f)
                {
                    // 添加帝印特质
                    __instance.addTrait("EmperorSeal", false);
                    emperorSealAcquired = true;
                }
                return;
            }
            
            // 检查是否已经有人获得戮王印
            if (!slaughterKingSealAcquired)
            {
                // 检查击杀数是否达到60000
                if (__instance.data.kills >= 60000f)
                {
                    // 添加戮王印特质
                    __instance.addTrait("SlaughterKingSeal", false);
                    slaughterKingSealAcquired = true;
                }
            }
        }

        private static void UpdateWarriorBasedOnGrade(Actor actor, string traitName, float maxWarrior)
        {
            if (actor.hasTrait(traitName))
            {
                float currentWarrior = actor.GetWarrior();
                float newValue = Mathf.Min(maxWarrior, currentWarrior);
                actor.SetWarrior(newValue);
            }
        }

        private static readonly string[] FlairmartialAptitudeTraits = new[] { "martialAptitude1", "martialAptitude2", "martialAptitude3", "martialAptitude4", "flair1", "flair2", "flair3", "flair4", "flair5", "flair6", "flair7" };
        private static bool HasAnyFlairTalen(Actor actor)
        {
            foreach (var trait in FlairmartialAptitudeTraits)
            {
                if (actor.hasTrait(trait))
                {
                    return true;
                }
            }
            return false;
        }
        private static readonly HashSet<string> Dongtian = new()
        {
            "CelestialGrotto1", "CelestialGrotto2", "CelestialGrotto3", "CelestialGrotto4",
            "CelestialGrotto5", "CelestialGrotto6", "CelestialGrotto7", "CelestialGrotto8",
            "CelestialGrotto9", "CelestialGrotto10", "CelestialGrotto11", "CelestialGrotto12",
            "CelestialGrotto13", "CelestialGrotto14", "CelestialGrotto15", "CelestialGrotto16",
            "CelestialGrotto17", "CelestialGrotto18"
        };
        private static bool TheresurrectionoftheCaveHeaven(Actor actor)
        {
            if (actor == null) return false;
            return Dongtian.Any(trait => actor.hasTrait(trait));
        }

        [HarmonyPrefix, HarmonyPatch(typeof(Actor), nameof(Actor.checkDeath))]
        private static bool Actor_CheckDeath_TheresurrectionoftheCaveHeaven(Actor __instance)
        {
            if (!__instance.hasHealth() && __instance.isAlive())
            {
                if (TheresurrectionoftheCaveHeaven(__instance))
                {
                    string grottoTrait = Dongtian.FirstOrDefault(t => __instance.hasTrait(t));
                    NotificationHelper.ShowGrottoNotification(__instance, grottoTrait);
                    RemoveAllDongtian(__instance);
                    __instance.setHealth(__instance.getMaxHealth(), true);
                    Vector3 actorPos = new Vector3(__instance.current_position.x, __instance.current_position.y, 0f);
                    EffectsLibrary.spawnExplosionWave(actorPos, 0.05f, 6f);
                    MusicBox.playSound("event:/SFX/EXPLOSIONS/ExplosionSmall", __instance.current_position.x, __instance.current_position.y, false, false);
                    return false;
                }
            }
            return true;
        }
        
        private static void RemoveAllDongtian(Actor actor)
        {
            if (actor == null)
                return;
            foreach (var trait in Dongtian)
            {
                if (actor.hasTrait(trait))
                {
                    actor.removeTrait(trait);
                }
            }
            actor.setStatsDirty();
        }

        // FormationSkill5特质的阵纹免死机制
        [HarmonyPrefix, HarmonyPatch(typeof(Actor), nameof(Actor.checkDeath))]
        public static bool Actor_CheckDeath_FormationSkill5Resurrection(Actor __instance)
        {
            if (!__instance.hasHealth() && __instance.isAlive() && __instance.hasTrait("FormationSkill5"))
            {
                float currentPattern = __instance.GetPattern();
                if (currentPattern >= 500f)
                {
                    // 消耗500点阵纹
                    __instance.ChangePattern(-500f);
                    // 回满血
                    __instance.setHealth(__instance.getMaxHealth(), true);
                    Vector3 actorPos = new Vector3(__instance.current_position.x, __instance.current_position.y, 0f);
                    EffectsLibrary.spawnExplosionWave(actorPos, 0.05f, 6f);
                    // 显示通知
                    NotificationHelper.ShowFormationSkill5ResurrectionNotification(__instance);
                    return false;
                }
            }
            return true;
        }

        // 邪功特质 - 击杀后获取气血值效果
        [HarmonyPrefix, HarmonyPatch(typeof(Actor), nameof(Actor.checkDeath))]
        public static bool Actor_CheckDeath_EvilGongfaTrait(Actor __instance)
        {
            // 检查目标是否死亡
            if (!__instance.hasHealth() && __instance.isAlive())
            {
                // 检查是否有最后攻击者记录
                if (lastAttackers.ContainsKey(__instance))
                {
                    Actor attacker = lastAttackers[__instance];
                    
                    // 检查攻击者是否还活着
                    if (attacker != null && attacker.isAlive())
                    {
                        // 获取目标的武道气血值
                        float targetWarrior = __instance.GetWarrior();
                        
                        // 检查攻击者是否具有邪功特质，并根据特质获取气血值
                        if (attacker.hasTrait("EvilGongFa1")) // 武者功法 - 10%
                        {
                            float warriorToSteal = targetWarrior * 0.1f;
                            attacker.ChangeWarrior(warriorToSteal);
                            // 添加视觉效果
                            EffectsLibrary.spawnExplosionWave(new Vector3(attacker.current_position.x, attacker.current_position.y, 0f), 0.02f, 3f);
                        }
                        else if (attacker.hasTrait("EvilGongFa2")) // 奇门秘术(MidGongFa) - 30%
                        {
                            float warriorToSteal = targetWarrior * 0.1f;
                            attacker.ChangeWarrior(warriorToSteal);
                            // 添加视觉效果
                            EffectsLibrary.spawnExplosionWave(new Vector3(attacker.current_position.x, attacker.current_position.y, 0f), 0.03f, 4f);
                        }
                        else if (attacker.hasTrait("EvilGongFa3")) // 武道秘典(arcaneTome) - 50%
                        {
                            float warriorToSteal = targetWarrior * 0.3f;
                            attacker.ChangeWarrior(warriorToSteal);
                            // 添加视觉效果
                            EffectsLibrary.spawnExplosionWave(new Vector3(attacker.current_position.x, attacker.current_position.y, 0f), 0.05f, 5f);
                        }
                    }
                    
                    // 移除记录
                    lastAttackers.Remove(__instance);
                }
            }
            return true;
        }
    }
}