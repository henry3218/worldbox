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
using Chevalier.code.Config;

namespace Chevalier.code
{
    internal class patch
    {
        public static bool window_creature_info_initialized;
        private static bool GodSealAcquired = false; // 全局标记是否已被获取

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

        // 用于跟踪每个角色上次获得领悟度增长的年龄
        private static Dictionary<string, int> lastComprehensionGainAge = new Dictionary<string, int>();
        // 预定义的领悟度增长值，避免重复计算
        private static Dictionary<string, float> comprehensionGainValues = new Dictionary<string, float> {
            {"Comprehensiontrait1", 4f},
            {"Comprehensiontrait2", 5f},
            {"Comprehensiontrait3", 6f},
            {"Comprehensiontrait4", 10f}
        };

        [HarmonyPostfix, HarmonyPatch(typeof(Actor), "updateAge")]
        public static void updateWorldTime_ChevalierPostfix(Actor __instance)
        {

            if (__instance == null)
            {
                return;
            }

            float age = (float)__instance.getAge();
            bool hasGermofLife10 = __instance.hasTrait("GermofLife10");// 检查是否具有特质
            bool hasGermofLife9 = __instance.hasTrait("GermofLife9");
            bool hasDivineSeal = __instance.hasTrait("DivineSeal"); // 检查是否拥有“先天圆满”特质

            bool hasObtainedLegacyTechnique = false;

            // 检查是否拥有“武祖赐福”特质
            bool hasGermofLife8 = __instance.hasTrait("GermofLife8");
            if (hasGermofLife8)
            {
                if (!hasObtainedLegacyTechnique)
                {
                    string[] LegacyTechnique = {
                        "LegacyTechnique91"
                    };

                    int randomIndex = UnityEngine.Random.Range(0, LegacyTechnique.Length);
                    string selectedLegacyTechnique = LegacyTechnique[randomIndex];

                    if (!__instance.hasTrait(selectedLegacyTechnique))
                    {
                        __instance.addTrait(selectedLegacyTechnique);
                        hasObtainedLegacyTechnique = true;
                    }
                }

                __instance.data.favorite = true;

            }

            // 检查是否拥有混沌帝血特质
            bool hasKnightlyBloodline7 = __instance.hasTrait("KnightlyBloodline7");
            if (hasKnightlyBloodline7)
            {
                // 直接觉醒玄玉根骨
                string GermofLife4Trait = "GermofLife4";
                // 定义根骨特质数组
                string[] GermofLifeTraits = {
                    "GermofLife1",
                    "GermofLife2",
                    "GermofLife3",
                    "GermofLife4",
                    "GermofLife7",
                    "GermofLife8",
                    "GermofLife9",
                    "GermofLife10"
                };

                // 检查是否已拥有任何根骨特质
                bool hasAnyGermofLife = false;
                foreach (string trait in GermofLifeTraits)
                {
                    if (__instance.hasTrait(trait))
                    {
                        hasAnyGermofLife = true;
                        break;
                    }
                }

                // 只有在未拥有任何根骨特质时才觉醒玄玉根骨
                if (!hasAnyGermofLife && !__instance.hasTrait("GermofLife4"))
                {
                    __instance.addTrait("GermofLife4");
                }
            }

            // 出生时（年龄为1）有一定概率获得“先天圆满”特质
            if (Mathf.FloorToInt(age) == 1 && !hasDivineSeal && Randy.randomChance(0.03f)) // 3% 的概率
            {
                __instance.addTrait("DivineSeal", false);
            }

            // 定义特定种族数组
            string[] basicRaces = { "orc", "human", "elf", "dwarf" };

            // 4岁时获得GermofLife1 - GermofLife10的随机特质
            if (Mathf.FloorToInt(age) == 4 && !hasGermofLife9 && !hasGermofLife10 &&
                !HasAnyFlairTalen(__instance) &&
                (basicRaces.Contains(__instance.asset.id) || __instance.asset.id.StartsWith("civ_")))
            {

                // 如果已经拥有玄玉根骨（因混沌帝血获得），则跳过此次随机根骨觉醒
                if (__instance.hasTrait("GermofLife4"))
                {
                    return;
                }

                float bloodlineBonus = 0f;
                if (__instance.hasTrait("KnightlyBloodline1"))
                {
                    bloodlineBonus = 0.2f; // 一级血脉提高20%的概率
                }
                else if (__instance.hasTrait("KnightlyBloodline2"))
                {
                    bloodlineBonus = 0.3f; // 二级血脉提高30%的概率
                }
                else if (__instance.hasTrait("KnightlyBloodline3"))
                {
                    bloodlineBonus = 0.4f; // 三级血脉提高40%的概率
                }
                else if (__instance.hasTrait("KnightlyBloodline4"))
                {
                    bloodlineBonus = 0.5f; // 四级血脉提高50%的概率
                }
                else if (__instance.hasTrait("KnightlyBloodline5"))
                {
                    bloodlineBonus = 0.6f; // 五级血脉提高60%的概率
                }
                else if (__instance.hasTrait("KnightlyBloodline6"))
                {
                    bloodlineBonus = 0.7f; // 六级血脉提高70%的概率
                }

                if (Randy.randomChance(0.2f + bloodlineBonus))
                {
                    var GermofLifeWeights = new (string traitId, float weight)[]
                    {
                        ("GermofLife1", 64f),
                        ("GermofLife2", 31f),
                        ("GermofLife3", 4.58f),
                        ("GermofLife4", 0.33f),
                        ("GermofLife7", 0.076f),
                        ("GermofLife8", 0.014f),
                    };

                    // 计算总权重
                    float totalWeight = GermofLifeWeights.Sum(t => t.weight);

                    // 生成随机浮点数
                    float randomValue = UnityEngine.Random.Range(0f, totalWeight);

                    // 遍历权重选择特质
                    string selectedGermofLife = "GermofLife1"; // 默认值
                    foreach (var GermofLife in GermofLifeWeights)
                    {
                        if (randomValue < GermofLife.weight)
                        {
                            selectedGermofLife = GermofLife.traitId;
                            break;
                        }
                        randomValue -= GermofLife.weight;
                    }

                    __instance.addTrait(selectedGermofLife, false);

                    // 觉醒悟性特质 - 所有觉醒资质的角色必定觉醒悟性
                    // 定义悟性特质权重
                    var ComprehensionWeights = new (string traitId, float weight)[]
                    {
                        ("Comprehensiontrait1", 50f),    // 下等：50%
                        ("Comprehensiontrait2", 10f),    // 中等：10%
                        ("Comprehensiontrait3", 1f),    // 上等：1%
                        ("Comprehensiontrait4", 0.1f), // 超绝：0.1%
                    };

                    // 计算总权重
                    float totalComprehensionWeight = ComprehensionWeights.Sum(t => t.weight);
                    
                    // 生成随机浮点数
                    float comprehensionRandom = UnityEngine.Random.Range(0f, totalComprehensionWeight);
                    
                    // 选择悟性特质
                    string selectedComprehension = "Comprehensiontrait1"; // 默认值
                    float cumWeight = 0;
                    foreach (var comprehension in ComprehensionWeights)
                    {
                        if (comprehensionRandom < comprehension.weight)
                        {
                            selectedComprehension = comprehension.traitId;
                            break;
                        }
                        cumWeight += comprehension.weight;
                        if (comprehensionRandom < cumWeight)
                        {
                            selectedComprehension = comprehension.traitId;
                            break;
                        }
                    }
                    __instance.addTrait(selectedComprehension, false);
                }
            }

            var GodlySigilChevalierChanges = new Dictionary<string, float>
            {
                {"GodlySigil1", 3.0f},
                {"GodlySigil2", 0.5f},
                {"GodlySigil3", 0.2f},
                {"GodlySigil4", 2.0f},
                {"GodlySigil5", 0.8f},
                {"GodlySigil6", 1.0f},
                {"GodlySigil7", 1.5f},
                {"GodlySigil8", 0.9f},
                {"GodlySigil9", 2.4f},
                {"GodlySigil91", 2.0f},
                {"GodlySigil92", 0.5f},
                {"GodlySigil93", 1.2f},
                {"GodlySigil94", 0.9f},
                {"GodlySigil95", 1.5f},
                {"GodlySigil96", 1.8f},
                {"GodlySigil97", 0.6f},
                {"GodlySigil98", 0.4f},
                {"GodlySigil99", 0.6f}
            };

            foreach (var GodlySigil in GodlySigilChevalierChanges)
            {
                if (__instance.hasTrait(GodlySigil.Key))
                {
                    float GodlySigilBonus = GodlySigil.Value;
                    __instance.ChangeChevalier(GodlySigilBonus);
                }
            }

            var GodKingdomChevalierChanges = new Dictionary<string, float>
            {
                {"GodKingdom1", 2.0f},
                {"GodKingdom2", 3.0f},
                {"GodKingdom3", 4.0f},
                {"GodKingdom4", 2.0f},
                {"GodKingdom5", 4.0f},
                {"GodKingdom6", 1.5f},
                {"GodKingdom7", 4.0f},
                {"GodKingdom8", 3.0f},
                {"GodKingdom9", 5.0f},
                {"GodKingdom10", 2.0f},
                {"GodKingdom11", 1.5f},
                {"GodKingdom12", 2.0f},
                {"GodKingdom13", 3.0f},
                {"GodKingdom14", 1.5f},
                {"GodKingdom15", 1.5f},
                {"GodKingdom16", 1.5f},
                {"GodKingdom17", 1.5f},
                {"GodKingdom18", 3.0f}
            };

            foreach (var GodKingdom in GodKingdomChevalierChanges)
            {
                if (__instance.hasTrait(GodKingdom.Key))
                {
                    float GodKingdomBonus = GodKingdom.Value;
                    __instance.ChangeChevalier(GodKingdomBonus);
                }
            }

            var NineLawsofKnighthoodChevalierChanges = new Dictionary<string, float>
            {
                {"NineLawsofKnighthood1", 2.0f},
                {"NineLawsofKnighthood2", 1.0f},
                {"NineLawsofKnighthood3", 2.0f},
                {"NineLawsofKnighthood4", 4.0f},
                {"NineLawsofKnighthood5", 1.0f},
                {"NineLawsofKnighthood6", 5.0f},
                {"NineLawsofKnighthood7", 3.0f},
                {"NineLawsofKnighthood8", 2.0f},
                {"NineLawsofKnighthood9", 6.0f}
            };

            foreach (var NineLawsofKnighthood in NineLawsofKnighthoodChevalierChanges)
            {
                if (__instance.hasTrait(NineLawsofKnighthood.Key))
                {
                    float NineLawsofKnighthoodBonus = NineLawsofKnighthood.Value;
                    __instance.ChangeChevalier(NineLawsofKnighthoodBonus);
                }
            }

            // 基础功法增幅
            var midFightingTechniqueChevalierChanges = new Dictionary<string, float>
            {
                {"MidFightingTechniqueTrait14", 1.0f}, // 新增基础功法：斗气+1
                // 可继续添加其他基础功法
            };
            // 新增：基础功法增幅逻辑
            foreach (var midFightingTechnique in midFightingTechniqueChevalierChanges)
            {
                if (__instance.hasTrait(midFightingTechnique.Key))
                {
                    __instance.ChangeChevalier(midFightingTechnique.Value);
                }
            }

            var FightingTechniqueChevalierChanges = new Dictionary<string, float>
            {
                {"FightingTechniqueTrait14", 2.0f},
                // 可继续添加其他基础功法
            };
            foreach (var FightingTechnique in FightingTechniqueChevalierChanges)
            {
                if (__instance.hasTrait(FightingTechnique.Key))
                {
                    __instance.ChangeChevalier(FightingTechnique.Value);
                }
            }

            var LegacyTechniqueChevalierChanges = new Dictionary<string, float>
            {
                {"LegacyTechnique14", 4.0f},
                // 可继续添加其他基础功法
            };
            foreach (var LegacyTechnique in LegacyTechniqueChevalierChanges)
            {
                if (__instance.hasTrait(LegacyTechnique.Key))
                {
                    __instance.ChangeChevalier(LegacyTechnique.Value);
                }
            }

            var GodSealChevalierChanges = new Dictionary<string, float>
            {
                {"GodSeal", 500f},
            };
            foreach (var GodSeal in GodSealChevalierChanges)
            {
                if (__instance.hasTrait(GodSeal.Key))
                {
                    __instance.ChangeChevalier(GodSeal.Value);
                }
            }

            // 根据悟性特质增加领悟度（每年一次）
            int currentAge = Mathf.FloorToInt(age);
            // 将long类型的id转换为string
            string actorId = __instance.id.ToString();
            
            if (__instance.isAlive())
            {
                // 检查是否需要增加领悟度
                    // 检查是否启用性能优化
                    if (ChevalierConfig.UsePerformanceOptimizations && ChevalierConfig.OptimizeComprehensionCalculation)
                    {
                        // 优化版本：批量计算和更新
                        if (!lastComprehensionGainAge.ContainsKey(actorId))
                        {
                            lastComprehensionGainAge[actorId] = currentAge; // 初始化
                        }
                        
                        // 只有当年龄变化超过1年时才更新，减少计算频率
                        if (currentAge > lastComprehensionGainAge[actorId])
                        {
                            // 计算应该增加的领悟度总量
                            int yearsPassed = currentAge - lastComprehensionGainAge[actorId];
                            float totalComprehensionGain = 0f;
                            
                            // 快速查找预定义的领悟度增长值
                            foreach (var traitGainPair in comprehensionGainValues)
                            {
                                if (__instance.hasTrait(traitGainPair.Key))
                                {
                                    totalComprehensionGain = traitGainPair.Value * yearsPassed;
                                    break;
                                }
                            }
                            
                            if (totalComprehensionGain > 0)
                            {
                                __instance.ChangeComprehension(totalComprehensionGain);
                                lastComprehensionGainAge[actorId] = currentAge;
                                
                                // 只有在累积了足够的领悟度时才检查战技获取
                                float currentComprehension = __instance.GetComprehension();
                                float lastCheckedComprehension = currentComprehension - totalComprehensionGain;
                                
                                // 只在可能跨越某个战技门槛时才调用检查函数
                                if ((lastCheckedComprehension < 100 && currentComprehension >= 100) ||
                                    (lastCheckedComprehension < 500 && currentComprehension >= 500) ||
                                    (lastCheckedComprehension < 1000 && currentComprehension >= 1000) ||
                                    (lastCheckedComprehension < 3000 && currentComprehension >= 3000) ||
                                    (lastCheckedComprehension < 6000 && currentComprehension >= 6000))
                                {
                                    traitAction.CheckAndGainTechniquesByComprehension(__instance);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!lastComprehensionGainAge.ContainsKey(actorId) || lastComprehensionGainAge[actorId] < currentAge)
                        {
                            float comprehensionGain = 0f;
                            if (__instance.hasTrait("Comprehensiontrait1"))
                            {
                                comprehensionGain = 4f;
                            }
                            else if (__instance.hasTrait("Comprehensiontrait2"))
                            {
                                comprehensionGain = 5f;
                            }
                            else if (__instance.hasTrait("Comprehensiontrait3"))
                            {
                                comprehensionGain = 6f;
                            }
                            else if (__instance.hasTrait("Comprehensiontrait4"))
                            {
                                comprehensionGain = 10f;
                            }
                            
                            if (comprehensionGain > 0)
                            {
                                __instance.ChangeComprehension(comprehensionGain);
                                lastComprehensionGainAge[actorId] = currentAge;
                                traitAction.CheckAndGainTechniquesByComprehension(__instance);
                            }
                        }
                    }
            }
            else if (lastComprehensionGainAge.ContainsKey(actorId))
            {
                // 移除已死亡角色的记录，避免内存泄漏
                lastComprehensionGainAge.Remove(actorId);
            }

            // 特质增加武者斗气的处理
            var GermofLifeChevalierChanges = new Dictionary<string, (float, float)>
            {
                { "GermofLife1", (1.0f, 2.0f) },//下
                { "GermofLife2", (2.0f, 4.0f) },//中
                { "GermofLife3", (4.0f, 6.0f) },//上
                { "GermofLife4", (6.0f, 10.0f) },//龙筋虎骨
                { "GermofLife10", (-1.0f, -2.0f) },//英雄迟暮
                { "GermofLife7", (10.0f, 14.0f) },//真武转世
                { "GermofLife8", (14.0f, 18.0f) },//天生至尊
            };

            foreach (var change in GermofLifeChevalierChanges)
            {
                // 如果具有GermofLife10特质，并且当前特质是GermofLife1到GermofLife4，则跳过
                if ((hasGermofLife10 || hasGermofLife9) && (change.Key == "GermofLife1" || change.Key == "GermofLife2" || change.Key == "GermofLife3" || change.Key == "GermofLife4"))
                {
                    continue;
                }

                if (__instance.hasTrait(change.Key))
                {
                    float randomChevalierIncrease = UnityEngine.Random.Range(change.Value.Item1, change.Value.Item2);
                    __instance.ChangeChevalier(randomChevalierIncrease);
                }
            }

            // 年龄和概率条件增加特质的处理
            var ChevalierTraitThresholds = new Dictionary<string, float>
            {
                { "Chevalier1", 40f },
                { "Chevalier2", 60f },
                { "Chevalier3", 80f },
                { "Chevalier4", 100f },
                { "Chevalier5", 120f },
                { "Chevalier6", 180f },
                { "Chevalier7", 200f },
                { "Chevalier8", 240f },
                { "Chevalier9", 300f },
                { "Chevalier91", 400f },
                { "Chevalier92", 1000f },
                { "Chevalier93", 50000f }
            };
            const float GermofLife10Chance = 0.1f;
            foreach (var threshold in ChevalierTraitThresholds)
            {
                if (__instance.hasTrait(threshold.Key) && age > threshold.Value && Randy.randomChance(GermofLife10Chance) && !hasDivineSeal)
                {
                    __instance.addTrait("GermofLife10", false);
                }
            }

            var grades = new Dictionary<string, float>
            {
                { "Chevalier1", 10f },
                { "Chevalier2", 20f },
                { "Chevalier3", 40f },
                { "Chevalier4", 80f },
                { "Chevalier5", 160f },
                { "Chevalier6", 300f },
                { "Chevalier7", 500f },
                { "Chevalier8", 800f },
                { "Chevalier9", 1200f },
                { "Chevalier91", 3200f },
                { "Chevalier92", 10000f },
                { "Chevalier93", 9999999f },
            };
            foreach (var grade in grades)
            {
                UpdateChevalierBasedOnGrade(__instance, grade.Key, grade.Value);
            }

            // 检查是否已经有人获得帝印
            if (GodSealAcquired) return;
        
            // 检查斗气值是否达到100000
            if (__instance.GetChevalier() >= 100000f)
            {
                // 添加帝印特质
                __instance.addTrait("GodSeal", false);
                GodSealAcquired = true;
            }
        }

        private static void UpdateChevalierBasedOnGrade(Actor actor, string traitName, float maxChevalier)
        {
            if (actor.hasTrait(traitName))
            {
                float currentChevalier = actor.GetChevalier();
                float newValue = Mathf.Min(maxChevalier, currentChevalier);
                actor.SetChevalier(newValue);
            }
        }

        private static readonly string[] FlairGermofLifeTraits = new[] { "GermofLife1", "GermofLife2", "GermofLife3", "GermofLife4", "flair1", "flair2", "flair3", "flair4", "flair5", "flair6", "flair7" };
        private static bool HasAnyFlairTalen(Actor actor)
        {
            foreach (var trait in FlairGermofLifeTraits)
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
            "GodKingdom1", "GodKingdom2", "GodKingdom3", "GodKingdom4",
            "GodKingdom5", "GodKingdom6", "GodKingdom7", "GodKingdom8",
            "GodKingdom9", "GodKingdom10", "GodKingdom11", "GodKingdom12",
            "GodKingdom13", "GodKingdom14", "GodKingdom15", "GodKingdom16",
            "GodKingdom17", "GodKingdom18"
        };
        private static bool TheresurrectionoftheGodKingdom(Actor actor)
        {
            if (actor == null) return false;
            return Dongtian.Any(trait => actor.hasTrait(trait));
        }

        [HarmonyPrefix, HarmonyPatch(typeof(Actor), nameof(Actor.checkDeath))]
        private static bool Actor_CheckDeath_TheresurrectionoftheGodKingdom(Actor __instance)
        {
            if (!__instance.hasHealth() && __instance.isAlive())
            {
                if (TheresurrectionoftheGodKingdom(__instance))
                {
                    string GodKingdomTrait = Dongtian.FirstOrDefault(t => __instance.hasTrait(t));
                    NotificationHelper.ShowGodKingdomNotification(__instance, GodKingdomTrait);
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
    }
}