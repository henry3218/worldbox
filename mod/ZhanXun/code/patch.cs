using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;
using ai;
using ai.behaviours;
using EpPathFinding.cs;
using life.taxi;
using NeoModLoader.api.attributes;
using NeoModLoader.General;

namespace ChivalryZhanXun.code
{
    internal static class patch
    {
        // =============================
        // 基础映射与常量
        // =============================

        // 按阈值从小到大有序，使用时从后往前扫描，避免 Dictionary.Reverse 的不确定性
        private static readonly (int threshold, string traitId)[] KillRewardThresholdsAsc =
        {
            (1, "ZhanXun1"),
            (10, "ZhanXun2"),
            (20, "ZhanXun3"),
            (30, "ZhanXun4"),
            (50, "ZhanXun5"),
            (70, "ZhanXun6"),
            (100, "ZhanXun7"),
            (200, "ZhanXun8"),
            (300, "ZhanXun9"),
            (500, "ZhanXun91"),
            (700, "ZhanXun92"),
            (1000, "ZhanXun93"),
            (2000, "ZhanXun94"),
            (3000, "ZhanXun95"),
            (5000, "ZhanXun96"),
            (7000, "ZhanXun97"),
            (10000, "ZhanXun98")
        };

        private static string GetKillRewardTrait(int kills)
        {
            for (int i = KillRewardThresholdsAsc.Length - 1; i >= 0; --i)
            {
                if (kills >= KillRewardThresholdsAsc[i].threshold)
                    return KillRewardThresholdsAsc[i].traitId;
            }
            return null;
        }

        private static readonly Dictionary<string, int> ZhanXunLevels = new()
        {
            {"ZhanXun1", 1}, {"ZhanXun2", 2}, {"ZhanXun3", 3}, {"ZhanXun4", 4},
            {"ZhanXun5", 5}, {"ZhanXun6", 6}, {"ZhanXun7", 7}, {"ZhanXun8", 8},
            {"ZhanXun9", 9}, {"ZhanXun91", 10}, {"ZhanXun92", 11}, {"ZhanXun93", 12},
            {"ZhanXun94", 13}, {"ZhanXun95", 14}, {"ZhanXun96", 15}, {"ZhanXun97", 16},
            {"ZhanXun98", 17}
        };

        // >= 7 即原「受保护」段
        private static bool HasProtectedTrait(Actor actor) => GetZhanXunLevel(actor) >= 7 || actor.hasStatus("TheUnmovingWiseKing");

        private static readonly Dictionary<long, int> KingKillCounts = new();

        private static readonly Dictionary<long, Dictionary<string, float>> armySpellCooldowns = new();
        private static readonly Dictionary<long, Dictionary<string, float>> countrySpellCooldowns = new();

        private static readonly HashSet<string> MilitarismTraits = new()
        {
            "ZhanXun93", "ZhanXun94", "ZhanXun95", "ZhanXun96", "ZhanXun97", "ZhanXun98"
        };

        private const string MilitarismKingdomTrait = "militarism";

        // 供 City.setCitizenJob 反射用：缓存字段信息，避免每次反射
        private static readonly System.Reflection.FieldInfo TimerWarriorField =
            AccessTools.Field(typeof(City), "_timer_warrior");
        private static readonly System.Reflection.FieldInfo LastCheckedJobIdField =
            AccessTools.Field(typeof(City), "_last_checked_job_id");

        // Army 队长反射缓存
        private static readonly System.Reflection.MethodInfo ArmyGetCaptainMI =
            AccessTools.Method(typeof(Army), "getCaptain");
        private static readonly System.Reflection.FieldInfo ArmyCaptainFI =
            AccessTools.Field(typeof(Army), "captain")
            ?? AccessTools.Field(typeof(Army), "leader")
            ?? AccessTools.Field(typeof(Army), "_captain");

        // 反射：行为目标 actor（供 BehGoToActorTarget / city_actor_attack_* 使用）
        private static readonly System.Reflection.FieldInfo BehActorTargetField =
            AccessTools.Field(typeof(Actor), "beh_actor_target");

        // 允许的 AI 任务白名单，静态缓存
        private static readonly HashSet<string> AllowedTasks = new()
        {
            "fighting",
            "warrior_army_follow_leader",
            "force_into_a_boat",
            "embark_into_boat",
            "sit_inside_boat",
            "taxi_check",
            "taxi_embark",
            "taxi_sit_inside",
            "taxi_find_ship_tile",
            "city_actor_warrior_taxi_check",
            "city_actor_attack_zone",
            "city_actor_attack_target",
            "city_actor_attack_building",
            "city_actor_attack_enemy_unit",
            "city_actor_attack_enemy_army",
            "city_actor_attack_enemy_city",
            "city_actor_attack_enemy_king",
            "city_actor_attack_enemy_leader",
            "check_warrior_transport",
            "boat_transport_check_taxi",
            "boat_transport_check",
            "boat_transport_go_load",
            "boat_transport_go_unload",
            "try_to_return_to_home_city",
            "warrior_try_join_army_group",
            "BehGoToActorTarget",
            "BehFightCheckEnemyIsOk"
        };

        private static readonly Dictionary<KingdomData, bool> MilitarismField = new();
        public static bool GetMilitarism(KingdomData data) => MilitarismField.TryGetValue(data, out var v) && v;
        public static void SetMilitarism(KingdomData data, bool value) => MilitarismField[data] = value;

        private static float lastArmyHeavyUpdate = 0f;
        private const float ARMY_HEAVY_UPDATE_INTERVAL = 0.25f; // 军队重逻辑节流（秒）

        // shareResource 调用保护，避免 StackTrace
        private static bool _inShareResource = false;

        // 主动寻敌节流
        private const float ARMY_SEEK_INTERVAL = 1.5f; // 每支军队寻敌节流秒数
        private const int ARMY_SEEK_RADIUS = 40;       // 寻敌半径（格子）
        private static readonly Dictionary<long, float> _armySeekCD = new();

        // =============================
        // 工具方法
        // =============================
        private static int GetZhanXunLevel(Actor actor)
        {
            if (actor == null) return 0;
            int highestLevel = 0;
            foreach (var trait in actor.getTraits())
            {
                if (trait.group_id == "ZhanXun" && ZhanXunLevels.TryGetValue(trait.id, out int level))
                    if (level > highestLevel) highestLevel = level;
            }
            return highestLevel;
        }

        private static int GetKingKillCount(Actor actor)
        {
            if (actor == null) return 0;
            return KingKillCounts.TryGetValue(actor.getID(), out int count) ? count : 0;
        }

        private static void IncrementKingKillCount(Actor actor)
        {
            if (actor == null) return;
            long id = actor.getID();
            if (KingKillCounts.ContainsKey(id)) KingKillCounts[id]++;
            else KingKillCounts[id] = 1;
        }

        private static Actor FindHighestZhanXunActor(IEnumerable<Actor> actors)
        {
            Actor best = null;
            int bestLevel = -1;
            foreach (var actor in actors)
            {
                if (actor == null || !actor.isAlive()) continue;
                int lv = GetZhanXunLevel(actor);
                if (lv > bestLevel) { bestLevel = lv; best = actor; }
            }
            return best;
        }

        private static bool IsPersonalArmy(Army army) => true;

        // 安全获取队长：先尝试 getCaptain()，再尝试常见私有字段名（已缓存）
        private static Actor CaptainOf(Army army)
        {
            if (army == null) return null;

            if (ArmyGetCaptainMI != null)
            {
                try { return (Actor)ArmyGetCaptainMI.Invoke(army, null); } catch { /* ignore */ }
            }

            if (ArmyCaptainFI != null)
            {
                try { return (Actor)ArmyCaptainFI.GetValue(army); } catch { /* ignore */ }
            }

            return null;
        }

        private static City FindNearestEnemyCity(Actor captain)
        {
            if (captain == null || !captain.hasCity()) return null;
            City myCity = captain.getCity();
            Kingdom myKingdom = myCity?.kingdom;
            if (myCity == null || myKingdom == null) return null;

            City nearest = null;
            float bestDist = float.MaxValue;
            foreach (var city in World.world.cities)
            {
                if (city == null || city == myCity) continue;
                if (city.kingdom != null && myKingdom.isEnemy(city.kingdom))
                {
                    var zone = (city.zones != null && city.zones.Count > 0) ? city.zones[0] : null;
                    var tile = zone?.centerTile;
                    if (tile == null) continue;
                    float d = Toolbox.SquaredDistTile(captain.current_tile, tile);
                    if (d < bestDist) { bestDist = d; nearest = city; }
                }
            }
            return nearest;
        }

        private static City FindEnemyCapital(Kingdom kingdom)
        {
            if (kingdom == null) return null;
            City myCapital = kingdom.capital;
            var myZone = myCapital?.zones != null && myCapital.zones.Count > 0 ? myCapital.zones[0] : null;
            var myTile = myZone?.centerTile;
            if (myTile == null) return null;

            City nearest = null;
            float bestDist = float.MaxValue;
            foreach (var city in World.world.cities)
            {
                if (city?.kingdom == null) continue;
                if (kingdom.isEnemy(city.kingdom) && city.kingdom.capital == city)
                {
                    var zone = (city.zones != null && city.zones.Count > 0) ? city.zones[0] : null;
                    var tile = zone?.centerTile;
                    if (tile == null) continue;
                    float d = Toolbox.SquaredDistTile(myTile, tile);
                    if (d < bestDist) { bestDist = d; nearest = city; }
                }
            }
            return nearest;
        }

        // 新增：仅挑选非军国主义敌国城市/首都作为传送目标
        private static City FindNearestEnemyCityNonMilitaristic(Actor captain)
        {
            if (captain == null || !captain.hasCity()) return null;
            City myCity = captain.getCity();
            Kingdom myKingdom = myCity?.kingdom;
            if (myCity == null || myKingdom == null) return null;

            City nearest = null;
            float bestDist = float.MaxValue;
            foreach (var city in World.world.cities)
            {
                if (city == null || city == myCity) continue;
                var enemyK = city.kingdom;
                if (enemyK != null && myKingdom.isEnemy(enemyK) && !GetMilitarism(enemyK.data))
                {
                    var zone = (city.zones != null && city.zones.Count > 0) ? city.zones[0] : null;
                    var tile = zone?.centerTile;
                    if (tile == null) continue;
                    float d = Toolbox.SquaredDistTile(captain.current_tile, tile);
                    if (d < bestDist) { bestDist = d; nearest = city; }
                }
            }
            return nearest;
        }

        private static City FindEnemyCapitalNonMilitaristic(Kingdom kingdom)
        {
            if (kingdom == null) return null;
            City myCapital = kingdom.capital;
            var myZone = myCapital?.zones != null && myCapital.zones.Count > 0 ? myCapital.zones[0] : null;
            var myTile = myZone?.centerTile;
            if (myTile == null) return null;

            City nearest = null;
            float bestDist = float.MaxValue;
            foreach (var city in World.world.cities)
            {
                var enemyK = city?.kingdom;
                if (enemyK == null) continue;
                if (kingdom.isEnemy(enemyK) && enemyK.capital == city && !GetMilitarism(enemyK.data))
                {
                    var zone = (city.zones != null && city.zones.Count > 0) ? city.zones[0] : null;
                    var tile = zone?.centerTile;
                    if (tile == null) continue;
                    float d = Toolbox.SquaredDistTile(myTile, tile);
                    if (d < bestDist) { bestDist = d; nearest = city; }
                }
            }
            return nearest;
        }

        private static void SyncMilitarismLeaders(Kingdom kingdom)
        {
            if (!GetMilitarism(kingdom.data)) return;

            var allUnits = kingdom.getUnits();
            var best = FindHighestZhanXunActor(allUnits);
            var oldKing = kingdom.king;

            if (best != null && best != oldKing &&
                (oldKing == null || GetZhanXunLevel(best) > GetZhanXunLevel(oldKing)))
            {
                if (best.city != null && best.city.leader == best)
                {
                    best.city.removeLeader();
                    best.setProfession(UnitProfession.Unit, true);
                }
                if (best.isWarrior()) best.stopBeingWarrior();
                best.setProfession(UnitProfession.Unit, true);

                if (oldKing != null)
                {
                    int year = Date.getYear(World.world.getCurWorldTime());
                    new WorldLogMessage(
                        WorldLogLibrary.king_new,
                        kingdom.name,
                        $"{kingdom.name}：{oldKing.getName()}于{year}年将王位传位于{best.getName()}",
                        year.ToString()
                    ).add();

                    kingdom.removeKing();

                    if (oldKing.city != null && oldKing.city.leader == oldKing)
                    {
                        oldKing.city.removeLeader();
                        oldKing.setProfession(UnitProfession.Unit, true);
                    }
                    if (oldKing.isWarrior()) oldKing.stopBeingWarrior();
                    if (oldKing.isKing()) oldKing.setProfession(UnitProfession.Unit, true);
                }

                kingdom.setKing(best, true);
                best.startShake();
                best.startColorEffect();
            }
        }

        private static Actor SelectBestCaptainFromArmy(Army army, City armyCity, WorldTile prevCaptainPos)
        {
            Actor best = null;
            int bestLevel = -1;
            int bestDist = int.MaxValue;

            // 仅在军队成员中择优，不按 city 过滤
            foreach (var u in army.units)
            {
                if (u == null || !u.isAlive() || u.army != army) continue;

                int level = GetZhanXunLevel(u);
                if (level <= 0) continue;

                int dist = int.MaxValue;
                if (prevCaptainPos != null) dist = Toolbox.SquaredDistTile(u.current_tile, prevCaptainPos);

                if (level > bestLevel || (level == bestLevel && dist < bestDist))
                {
                    best = u;
                    bestLevel = level;
                    bestDist = dist;
                }
            }

            // 没有战勋则选战士（不按 city 过滤）
            if (best == null)
            {
                Actor nearestWarrior = null;
                int nearestDist = int.MaxValue;
                foreach (var u in army.units)
                {
                    if (u == null || !u.isAlive() || u.army != army || !u.isWarrior()) continue;
                    int dist = prevCaptainPos == null ? 0 : Toolbox.SquaredDistTile(u.current_tile, prevCaptainPos);
                    if (nearestWarrior == null || dist < nearestDist)
                    {
                        nearestWarrior = u;
                        nearestDist = dist;
                    }
                }
                best = nearestWarrior;
            }

            // 仍没有则找最近的任意单位（不按 city 过滤）
            if (best == null)
            {
                Actor nearest = null;
                int nearestDist = int.MaxValue;
                foreach (var u in army.units)
                {
                    if (u == null || !u.isAlive() || u.army != army) continue;
                    int dist = prevCaptainPos == null ? 0 : Toolbox.SquaredDistTile(u.current_tile, prevCaptainPos);
                    if (nearest == null || dist < nearestDist)
                    {
                        nearest = u;
                        nearestDist = dist;
                    }
                }
                best = nearest;
            }

            // 兜底
            if (best == null)
            {
                foreach (var u in army.units)
                {
                    if (u != null && u.isAlive()) { best = u; break; }
                }
            }

            return best;
        }

        private static bool IsKingdomInWar(Kingdom kingdom)
        {
            if (kingdom == null) return false;

            foreach (var war in World.world.wars.getWars(kingdom))
                if (war != null && !war.hasEnded()) return true;

            foreach (var other in World.world.kingdoms)
                if (other != null && other != kingdom && kingdom.isEnemy(other)) return true;

            return false;
        }

        private static bool IsSpellRequireWar(string spellId)
        {
            // 战时限定的法术
            return spellId is "Johnson" or "Selfde" or "Ironarmor" or "TheUnmovingWiseKing" or
                   "Accelerate" or "Strongwind" or "Yufeng" or "enhancement" or "andblood" or "Overload" or "evilenergy_aura" or "teleport";
        }

        // 新增：统一判断是否应屏蔽「占领」
        private static bool ShouldBlockCapture(City city)
        {
            if (city == null) return false;
            // 全局开关 或 军国化国家
            if (ZhanXunConfig.AutoCollectCity) return true;
            if (city.kingdom == null) return false;
            return GetMilitarism(city.kingdom.data);
        }

        private static void CheckAndUpdateCaptain(Army army)
        {
            if (army == null || army.units.Count == 0) return;
            City city = army.getCity();
            var best = SelectBestCaptainFromArmy(army, city, CaptainOf(army)?.current_tile);

            var current = CaptainOf(army);
            if (best != null && (current == null || best != current))
            {
                if (current == null || GetZhanXunLevel(best) > GetZhanXunLevel(current))
                    army.setCaptain(best);
            }
        }

        // City 私有字段访问封装
        private static float GetTimerWarrior(City c)
        {
            if (TimerWarriorField == null) return 0f;
            var v = TimerWarriorField.GetValue(c);
            return v is float f ? f : 0f;
        }
        private static void SetTimerWarrior(City c, float v)
        {
            TimerWarriorField?.SetValue(c, v);
        }
        private static int IncAndGetLastCheckedJobId(City c, int max)
        {
            if (LastCheckedJobIdField == null || max <= 0) return 0;
            int cur = 0;
            var v = LastCheckedJobIdField.GetValue(c);
            if (v is int i) cur = i;
            cur++;
            if (cur > max - 1) cur = 0;
            LastCheckedJobIdField.SetValue(c, cur);
            return cur;
        }

        // =============================
        // 法术定义
        // =============================
        private class ArmySpell
        {
            public string id;
            public string statusId;
            public float duration;
            public float cooldown;
            public int minZhanXunLevel;
            public int maxZhanXunLevel;
            public string displayName;
        }

        private static readonly ArmySpell[] ArmySpells =
        {
            new() { id = "Johnson", statusId = "Johnson", duration = 30f, cooldown = 60f, minZhanXunLevel = 1, maxZhanXunLevel = 4, displayName = "强身术" },
            new() { id = "Selfde", statusId = "Selfde", duration = 30f, cooldown = 60f, minZhanXunLevel = 5, maxZhanXunLevel = 6, displayName = "护身术" },
            new() { id = "Ironarmor", statusId = "Ironarmor", duration = 30f, cooldown = 60f, minZhanXunLevel = 7, maxZhanXunLevel = 9, displayName = "铁甲术" },
            new() { id = "TheUnmovingWiseKing", statusId = "TheUnmovingWiseKing", duration = 60f, cooldown = 120f, minZhanXunLevel = 10, maxZhanXunLevel = 17, displayName = "不动明王" },
            new() { id = "heal_small", statusId = "heal", duration = 0f, cooldown = 60f, minZhanXunLevel = 1, maxZhanXunLevel = 4, displayName = "小治疗术" },
            new() { id = "heal_medium", statusId = "heal", duration = 0f, cooldown = 60f, minZhanXunLevel = 5, maxZhanXunLevel = 6, displayName = "中治疗术" },
            new() { id = "heal_large", statusId = "heal", duration = 0f, cooldown = 60f, minZhanXunLevel = 7, maxZhanXunLevel = 9, displayName = "大治疗术" },
            new() { id = "heal_Super", statusId = "heal", duration = 0f, cooldown = 60f, minZhanXunLevel = 10, maxZhanXunLevel = 11, displayName = "超级治疗术" },
            new() { id = "heal_Ultimate", statusId = "heal", duration = 0f, cooldown = 60f, minZhanXunLevel = 12, maxZhanXunLevel = 17, displayName = "究极治疗术" },
            new() { id = "Accelerate", statusId = "Accelerate", duration = 30f, cooldown = 60f, minZhanXunLevel = 1, maxZhanXunLevel = 4, displayName = "加速术" },
            new() { id = "Strongwind", statusId = "Strongwind", duration = 30f, cooldown = 60f, minZhanXunLevel = 5, maxZhanXunLevel = 6, displayName = "疾风术" },
            new() { id = "Yufeng", statusId = "Yufeng", duration = 30f, cooldown = 60f, minZhanXunLevel = 7, maxZhanXunLevel = 17, displayName = "御风术" },
            new() { id = "teleport", statusId = "teleport", duration = 60f, cooldown = 240f, minZhanXunLevel = 10, maxZhanXunLevel = 17, displayName = "传送术" },
            new() { id = "enhancement", statusId = "enhancement", duration = 30f, cooldown = 60f, minZhanXunLevel = 1, maxZhanXunLevel = 4, displayName = "力量强化" },
            new() { id = "andblood", statusId = "andblood", duration = 30f, cooldown = 60f, minZhanXunLevel = 5, maxZhanXunLevel = 6, displayName = "气血澎湃" },
            new() { id = "Overload", statusId = "Overload", duration = 30f, cooldown = 60f, minZhanXunLevel = 7, maxZhanXunLevel = 9, displayName = "超载爆发" },
            new() { id = "evilenergy_aura", statusId = "evilenergy_aura", duration = 60f, cooldown = 120f, minZhanXunLevel = 10, maxZhanXunLevel = 17, displayName = "煞气" }
        };

        private class CountrySpell
        {
            public string id;
            public string statusId;
            public float duration;
            public float cooldown;
            public int minZhanXunLevel;
            public int maxZhanXunLevel;
            public string displayName;
        }

        private static readonly CountrySpell[] CountrySpells =
        {
            new() { id = "Johnson", statusId = "Johnson", duration = 30f, cooldown = 60f, minZhanXunLevel = 1, maxZhanXunLevel = 4, displayName = "强身术" },
            new() { id = "Selfde", statusId = "Selfde", duration = 30f, cooldown = 60f, minZhanXunLevel = 5, maxZhanXunLevel = 6, displayName = "护身术" },
            new() { id = "Ironarmor", statusId = "Ironarmor", duration = 30f, cooldown = 60f, minZhanXunLevel = 7, maxZhanXunLevel = 9, displayName = "铁甲术" },
            new() { id = "TheUnmovingWiseKing", statusId = "TheUnmovingWiseKing", duration = 60f, cooldown = 120f, minZhanXunLevel = 10, maxZhanXunLevel = 17, displayName = "不动明王" },
            new() { id = "heal_small", statusId = "heal", duration = 0f, cooldown = 60f, minZhanXunLevel = 1, maxZhanXunLevel = 4, displayName = "小治疗术" },
            new() { id = "heal_medium", statusId = "heal", duration = 0f, cooldown = 60f, minZhanXunLevel = 5, maxZhanXunLevel = 6, displayName = "中治疗术" },
            new() { id = "heal_large", statusId = "heal", duration = 0f, cooldown = 60f, minZhanXunLevel = 7, maxZhanXunLevel = 9, displayName = "大治疗术" },
            new() { id = "heal_Super", statusId = "heal", duration = 0f, cooldown = 60f, minZhanXunLevel = 10, maxZhanXunLevel = 11, displayName = "超级治疗术" },
            new() { id = "heal_Ultimate", statusId = "heal", duration = 0f, cooldown = 60f, minZhanXunLevel = 12, maxZhanXunLevel = 17, displayName = "究极治疗术" },
            new() { id = "Accelerate", statusId = "Accelerate", duration = 30f, cooldown = 60f, minZhanXunLevel = 1, maxZhanXunLevel = 4, displayName = "加速术" },
            new() { id = "Strongwind", statusId = "Strongwind", duration = 30f, cooldown = 60f, minZhanXunLevel = 5, maxZhanXunLevel = 6, displayName = "疾风术" },
            new() { id = "Yufeng", statusId = "Yufeng", duration = 30f, cooldown = 60f, minZhanXunLevel = 7, maxZhanXunLevel = 17, displayName = "御风术" },
            new() { id = "teleport", statusId = "teleport", duration = 60f, cooldown = 240f, minZhanXunLevel = 10, maxZhanXunLevel = 17, displayName = "传送术" },
            new() { id = "enhancement", statusId = "enhancement", duration = 30f, cooldown = 60f, minZhanXunLevel = 1, maxZhanXunLevel = 4, displayName = "力量强化" },
            new() { id = "andblood", statusId = "andblood", duration = 30f, cooldown = 60f, minZhanXunLevel = 5, maxZhanXunLevel = 6, displayName = "气血澎湃" },
            new() { id = "Overload", statusId = "Overload", duration = 30f, cooldown = 60f, minZhanXunLevel = 7, maxZhanXunLevel = 9, displayName = "超载爆发" },
            new() { id = "evilenergy_aura", statusId = "evilenergy_aura", duration = 60f, cooldown = 120f, minZhanXunLevel = 10, maxZhanXunLevel = 17, displayName = "煞气" }
        };

        // =============================
        // 新增：战勋首达世界日志（功能函数 + 工具）
        // =============================
        private static readonly HashSet<string> ZhanXunFirstAnnounced = new();

        private static string GetTraitIconPathSafe(string traitId)
        {
            try
            {
                var traitAsset = AssetManager.traits.get(traitId);
                if (traitAsset != null)
                {
                    var fi = AccessTools.Field(traitAsset.GetType(), "path_icon");
                    if (fi != null)
                    {
                        var v = fi.GetValue(traitAsset) as string;
                        if (!string.IsNullOrEmpty(v)) return v;
                    }
                    var pi = AccessTools.Property(traitAsset.GetType(), "path_icon");
                    if (pi != null)
                    {
                        var v = pi.GetValue(traitAsset) as string;
                        if (!string.IsNullOrEmpty(v)) return v;
                    }
                }
            }
            catch { /* ignore */ }
            return "trait/Grade7"; // 兜底
        }

        private static void AddZhanXunBreakthroughLog(string assetId, string iconPath, string breakthroughText, Actor actor, Color color)
        {
            bool shouldShowLog = true; // 如需配置开关，可在此处做 switch(assetId) 判断

            // 首达去重：同一个 assetId 只播报一次
            if (!ZhanXunFirstAnnounced.Add(assetId))
                return;

            if (shouldShowLog)
            {
                WorldLogAsset asset = new WorldLogAsset
                {
                    id = assetId,
                    group = "zhanxun",
                    path_icon = iconPath,
                    color = Toolbox.color_log_neutral,
                    text_replacer = delegate (WorldLogMessage pMessage, ref string pText)
                    {
                        pText = "$special1$" + breakthroughText;
                        string tSpecialText = pMessage.getSpecial(1);
                        pText = pText.Replace("$special1$", tSpecialText);
                    }
                };

                if (!AssetManager.world_log_library.has(asset.id))
                {
                    AssetManager.world_log_library.add(asset);
                }

                new WorldLogMessage(AssetManager.world_log_library.get(asset.id), actor.getName(), null, null)
                {
                    unit = actor,
                    location = actor.current_position,
                    color_special1 = color
                }.add();
            }
        }

        // 战勋突破文案：1、7、12、17级使用特别文案，其它用默认
        private static string GetZhanXunBreakthroughText(int lv)
        {
            switch (lv)
            {
                case 1: return $"世界史上第1位杀人者 Lv.{lv}!";
                case 7: return $"第1位拥有左右战局能力的存在 Lv.{lv}!";
                case 12: return $"创国者，创立最初的军国 Lv.{lv}!";
                case 17: return $"灭世魔神，歼灭世界之人 Lv.{lv}!";
                default: return $"杀至战勋 Lv.{lv}!";
            }
        }

        // =============================
        // Patch：击杀/战勋/王杀
        // =============================
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Actor), "newKillAction")]
        public static void Actor_newKillAction_Postfix(Actor __instance, Actor pDeadUnit)
        {
            if (__instance == null || !__instance.isAlive()) return;

            // 1) 额外掠夺击杀（保留原逻辑）
            float ratio = Mathf.Clamp01(ZhanXunConfig.AutoCollectPlunder);
            if (pDeadUnit != null && pDeadUnit.data != null && ratio > 0f)
            {
                int victimKills = pDeadUnit.data.kills;
                if (victimKills > 0)
                {
                    long add = (long)Mathf.Round(victimKills * ratio);
                    if (add > 0)
                    {
                        long total = (long)__instance.data.kills + add; // 原方法已 +1
                        if (total > int.MaxValue) total = int.MaxValue;
                        __instance.data.kills = (int)total;
                    }
                }
            }

            // 2) 王杀统计（保留原逻辑）
            if (pDeadUnit != null && pDeadUnit.isKing())
            {
                IncrementKingKillCount(__instance);
                if (GetKingKillCount(__instance) >= 10 && !__instance.hasTrait("HunterKing"))
                    __instance.addTrait("HunterKing", false);
            }

            // 3) 新增：战勋开关。关闭后，不再授予战勋，但击杀数仍累积
            if (!ZhanXunConfig.AutoCollectZhanXun)
                return;

            // 4) 战勋奖励（原逻辑）
            string highestTraitId = GetKillRewardTrait(__instance.data.kills);
            if (highestTraitId != null && !__instance.hasTrait(highestTraitId))
            {
                var toRemove = new List<ActorTrait>();
                foreach (var t in __instance.getTraits())
                    if (t.group_id == "ZhanXun") toRemove.Add(t);
                if (toRemove.Count > 0) __instance.removeTraits(toRemove);

                __instance.addTrait(highestTraitId, false);

                // 新增：战勋“首达”世界日志（一个负责功能，一个负责调用）
                {
                    ZhanXunLevels.TryGetValue(highestTraitId, out int lv);
                    string assetId = $"zhanxun_{highestTraitId}_breakthrough";
                    string iconPath = GetTraitIconPathSafe(highestTraitId);
                    string text = GetZhanXunBreakthroughText(lv); // 按等级选择文案
                    AddZhanXunBreakthroughLog(assetId, iconPath, text, __instance, Color.yellow);
                }

                if (__instance.data.kills >= 1 && !__instance.hasTrait("fire_proof"))
                    __instance.addTrait("fire_proof", false);
                if (__instance.data.kills >= 100 && !__instance.hasTrait("freeze_proof"))
                    __instance.addTrait("freeze_proof", false);
                if (__instance.data.kills >= 100 && !__instance.hasTrait("immune"))
                    __instance.addTrait("immune", false);
                if (__instance.data.kills >= 100 && !__instance.hasTrait("strong_minded"))
                    __instance.addTrait("strong_minded", false);
            }
        }

        // =============================
        // Patch：市民分工/招募（只在接管时拦截）
        // =============================
        [HarmonyPrefix]
        [HarmonyPatch(typeof(City), "setCitizenJob")]
        public static bool City_setCitizenJob_Prefix(City __instance, Actor pActor)
        {
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

            bool handled = false;
            bool hasZhanXun = pActor.getTraits().Any(trait => trait.group_id == "ZhanXun");
            if (hasZhanXun && __instance.checkCanMakeWarrior(pActor))
            {
                __instance.makeWarrior(pActor);
                SetTimerWarrior(__instance, 15f);
                handled = true;
            }
            else if (!__instance.isGettingCaptured() &&
                     GetTimerWarrior(__instance) <= 0f &&
                     pActor.isProfession(UnitProfession.Unit) &&
                     __instance.getResourcesAmount("gold") > 10 &&
                     __instance.hasEnoughFoodForArmy() &&
                     __instance.tryToMakeWarrior(pActor))
            {
                handled = true;
            }
            else if (__instance.checkCitizenJobList(AssetManager.citizen_job_library.list_priority_high, pActor))
            {
                handled = true;
            }
            else if (!__instance.hasAnyFood() &&
                     __instance.checkCitizenJobList(AssetManager.citizen_job_library.list_priority_high_food, pActor))
            {
                handled = true;
            }
            else
            {
                List<CitizenJobAsset> tJobList = AssetManager.citizen_job_library.list_priority_normal;
                for (int i = 0; i < tJobList.Count; i++)
                {
                    int idx = IncAndGetLastCheckedJobId(__instance, tJobList.Count);
                    var job = tJobList[idx];
                    if ((job.ok_for_king || !pActor.isKing()) && (job.ok_for_leader || !pActor.isCityLeader()) &&
                        __instance.checkCitizenJob(job, __instance, pActor))
                    {
                        handled = true;
                        break;
                    }
                }
            }
            return handled ? false : true;
        }

        // =============================
        // Patch：军队-选择队长（改良，无城市限制 + 空队伍清空队长）
        // =============================
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Army), "findCaptain")]
        public static bool Army_findCaptain_Prefix(Army __instance)
        {
            if (__instance == null) return true;

            var curCap = CaptainOf(__instance);
            if (__instance.hasCaptain() && (curCap == null || curCap.isRekt() || !curCap.isAlive()))
                __instance.setCaptain(null);

            // 方案A：当军队无任何成员时，先清空队长，然后交回原版（由原版决定是否解散）
            if (__instance.units.Count == 0)
            {
                if (__instance.hasCaptain()) __instance.setCaptain(null);
                return true;
            }

            curCap = CaptainOf(__instance);
            if (__instance.hasCaptain() && curCap != null && curCap.isKingdomCiv())
                return true;

            if (__instance.hasCaptain()) __instance.setCaptain(null);

            City armyCity = __instance.getCity();
            var best = SelectBestCaptainFromArmy(__instance, armyCity, curCap?.current_tile);
            if (best != null) __instance.setCaptain(best);
            return false;
        }

        // =============================
        // Patch：控制免击飞/免控
        // =============================
        [HarmonyPrefix, HarmonyPatch(typeof(ActorTool), nameof(ActorTool.applyForceToUnit))]
        public static bool applyForceToUnit_Protection(AttackData pData, BaseSimObject pTargetToCheck, float pMod = 1f, bool pCheckCancelJobOnLand = false)
            => !(pTargetToCheck.isActor() && HasProtectedTrait(pTargetToCheck.a));

        [HarmonyPrefix, HarmonyPatch(typeof(Actor), nameof(Actor.calculateForce))]
        public static bool CalculateForce_Protection(Actor __instance, float pStartX, float pStartY, float pTargetX, float pTargetY, float pForceAmountDirection, float pForceHeight = 0f, bool pCheckCancelJobOnLand = false)
            => !HasProtectedTrait(__instance);

        [HarmonyPrefix, HarmonyPatch(typeof(Actor), nameof(Actor.applyRandomForce))]
        public static bool ApplyRandomForce_Protection(Actor __instance, float pMinHeight = 1.5f, float pMaxHeight = 2f)
            => !HasProtectedTrait(__instance);

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Actor), nameof(Actor.makeStunned))]
        public static bool MakeStunned_Protection(Actor __instance, float pTime = 5f)
            => !HasProtectedTrait(__instance);

        [HarmonyPrefix, HarmonyPatch(typeof(Actor), nameof(Actor.addStatusEffect), typeof(StatusAsset), typeof(float), typeof(bool))]
        public static bool AddStatusEffect_Protection(Actor __instance, StatusAsset pStatusAsset, float pOverrideTimer = 0f, bool pColorEffect = true)
        {
            if (pStatusAsset.id == "surprised" || pStatusAsset.id == "stunned" || pStatusAsset.id == "sleeping")
                return !HasProtectedTrait(__instance);
            return true;
        }

        // =============================
        // 主动寻敌工具
        // =============================
        private static Actor FindNearestEnemyUnit(WorldTile center, Kingdom myKingdom, int radius)
        {
            if (center == null || myKingdom == null) return null;
            float radius2 = radius * radius;
            Actor best = null;
            float bestDist = float.MaxValue;

            foreach (var k in World.world.kingdoms)
            {
                if (k == null || !myKingdom.isEnemy(k)) continue;

                foreach (var u in k.getUnits())
                {
                    if (u == null || !u.isAlive()) continue;
                    if (u.current_tile == null || !u.current_tile.isSameIsland(center)) continue;

                    float d = Toolbox.SquaredDistTile(center, u.current_tile);
                    if (d > radius2) continue;

                    int prio = (u.isKing() ? 3 : (u.isCityLeader() ? 2 : (u.isWarrior() ? 1 : 0)));
                    int bestPrio = (best == null ? -1 : (best.isKing() ? 3 : (best.isCityLeader() ? 2 : (best.isWarrior() ? 1 : 0))));

                    if (prio > bestPrio || (prio == bestPrio && d < bestDist))
                    {
                        best = u;
                        bestDist = d;
                    }
                }
            }
            return best;
        }

        // 安全封装，避免 AiSystem.setTask 引发 NRE
        private static bool SafeSetTask(Actor actor, string taskId, bool clean = true, bool cleanJob = false, bool forceAction = true)
        {
            if (actor == null || !actor.isAlive() || actor.ai == null) return false;
            try
            {
                actor.setTask(taskId, clean, cleanJob, forceAction);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void OrderArmyAttackTarget(List<Actor> units, Actor target)
        {
            if (target == null || !target.isAlive()) return;

            foreach (var m in units)
            {
                if (m == null || !m.isAlive() || m.ai == null) continue;

                // 尝试设置行为目标引用（非必须）
                try { BehActorTargetField?.SetValue(m, target); } catch { /* ignore */ }

                bool assigned = false;

                // 优先使用已有的“城市攻击单位/目标”任务；切换 job 以保证可用
                if (m.city != null)
                {
                    assigned = SafeSetTask(m, "city_actor_attack_enemy_unit", clean: true, cleanJob: true, forceAction: true)
                            || SafeSetTask(m, "city_actor_attack_target", clean: true, cleanJob: true, forceAction: true);
                }

                // 回退：直接战斗
                if (!assigned)
                {
                    assigned = SafeSetTask(m, "fighting", clean: true, cleanJob: false, forceAction: true);
                }
                // 再回退：跟随队长
                if (!assigned)
                {
                    SafeSetTask(m, "warrior_army_follow_leader", clean: true, cleanJob: false, forceAction: true);
                }
            }
        }

        // =============================
        // Patch：军队 AI + 军队法术（合并并节流 + 主动寻敌）
        // =============================
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ArmyManager), nameof(ArmyManager.update))]
        public static void ArmyManager_update_Postfix(ArmyManager __instance, float pElapsed)
        {
            lastArmyHeavyUpdate += pElapsed;
            if (lastArmyHeavyUpdate < ARMY_HEAVY_UPDATE_INTERVAL) return;
            float dt = lastArmyHeavyUpdate;
            lastArmyHeavyUpdate = 0f;

            foreach (var army in World.world.armies)
            {
                var armyCity = army.getCity();

                // 直接获取队长（移除城市一致性检查）
                var captain = CaptainOf(army);

                if (!IsPersonalArmy(army)) continue;
                if (captain == null || !captain.isAlive()) { CheckAndUpdateCaptain(army); captain = CaptainOf(army); }
                if (captain == null || !captain.isAlive() || captain.current_tile == null) continue;

                // 军队成员集合（不再做去重/单独处理队长）
                var allMembers = army.units;

                // 清理携带资源、情感、饱食、心情、去负面等（保留原有玩法）
                foreach (var member in allMembers)
                {
                    if (member == null || !member.isAlive()) continue;

                    if (member.isCarryingResources()) member.giveInventoryResourcesToCity();
                    if (member.loot > 0) member.takeAllOwnLoot();

                    var lover = member.lover;
                    if (lover != null && lover != member && !member.isClonedFrom(lover) && !member.isChildOf(lover) && !member.isParentOf(lover))
                    {
                        if (!(member.hasLover() && member.lover == lover)) member.becomeLoversWith(lover);
                    }

                    if (lover != null &&
                        member.isSexFemale() && member.canBreed() && lover.canBreed() &&
                        member.isAdult() && lover.isAdult() &&
                        !member.hasReachedOffspringLimit() &&
                        !member.hasStatus("afterglow") && !lover.hasStatus("afterglow") &&
                        !member.hasStatus("pregnant") &&
                        member.city != null &&
                        (
                            // 0（或以下）= 不限制；>0 = 按配置上限
                            ZhanXunConfig.AutoCollectUrbanpopulation <= 0f ||
                            member.city.getPopulationPeople() < ZhanXunConfig.AutoCollectUrbanpopulation
                        ))
                    {
                        float maturationTime = member.getMaturationTimeSeconds();
                        member.addStatusEffect("pregnant", maturationTime, true);
                        float afterglowTime = maturationTime + 30f;
                        member.addStatusEffect("afterglow", afterglowTime, true);
                        lover.addStatusEffect("afterglow", afterglowTime, true);
                    }

                    if (member.isHungry())
                    {
                        member.addNutritionFromEating(member.getMaxNutrition(), false, true);
                        member.countConsumed();
                    }

                    if (member.isWarrior() && member.getHappiness() < 50)
                    {
                        int newHappy = Mathf.Min(member.getHappiness() + 20, member.getMaxHappiness());
                        member.setHappiness(newHappy, true);
                    }

                    if (member.hasStatus("burning")) member.finishStatusEffect("burning");
                    if (member.hasTrait("crippled")) member.removeTrait("crippled");
                    if (member.hasTrait("eyepatch")) member.removeTrait("eyepatch");
                    if (member.hasTrait("skin_burns")) member.removeTrait("skin_burns");

                    if (member.city != null && member.city.hasAttackZoneOrder())
                    {
                        var targetTile = member.city.target_attack_zone.centerTile;
                        if (targetTile != null && member.current_tile != null && !targetTile.isSameIsland(member.current_tile))
                            TaxiManager.newRequest(member, targetTile);
                    }

                    // 非队长才做跟随与脱困
                    if (member == captain) continue;

                    var curTask = member.ai?.task?.id;
                    if (string.IsNullOrEmpty(curTask) || !AllowedTasks.Contains(curTask))
                    {
                        member.cancelAllBeh();
                        SafeSetTask(member, "warrior_army_follow_leader", true, false, true);
                    }

                    // 只有在不同岛屿的情况下才会传送（同岛不再救援传送）
                    if (member.ai?.task?.id == "warrior_army_follow_leader")
                    {
                        var mTile = member.current_tile;
                        var cTile = captain.current_tile;
                        if (mTile != null && cTile != null && !mTile.isSameIsland(cTile))
                        {
                            WorldTile targetTile = cTile;
                            var nearbyTiles = new List<WorldTile>();
                            for (int dx = -3; dx <= 3; dx++)
                            {
                                for (int dy = -3; dy <= 3; dy++)
                                {
                                    var tile = World.world.GetTile(targetTile.pos.x + dx, targetTile.pos.y + dy);
                                    if (tile != null && !tile.Type.block && !tile.Type.lava &&
                                        (member.isWaterCreature() ? tile.Type.ocean : tile.Type.ground))
                                    {
                                        nearbyTiles.Add(tile);
                                    }
                                }
                            }

                            if (nearbyTiles.Count > 0)
                            {
                                WorldTile bestTile = nearbyTiles[0];
                                float bestScore = float.MaxValue;
                                foreach (var tile in nearbyTiles)
                                {
                                    float d = Toolbox.SquaredDistTile(tile, targetTile);
                                    if (d >= 4f && d <= 16f && d < bestScore)
                                    {
                                        bestScore = d;
                                        bestTile = tile;
                                    }
                                }

                                if (bestScore == float.MaxValue)
                                {
                                    float bestDist = float.MaxValue;
                                    foreach (var tile in nearbyTiles)
                                    {
                                        float d = Toolbox.SquaredDistTile(tile, targetTile);
                                        if (d < bestDist)
                                        {
                                            bestDist = d;
                                            bestTile = tile;
                                        }
                                    }
                                }

                                member.cancelAllBeh();
                                // 如需视觉效果，可取消注释下一行
                                // ActionLibrary.teleportEffect(member, bestTile);
                                member.spawnOn(bestTile, 0f);
                                SafeSetTask(member, "warrior_army_follow_leader", true, false, true);
                            }
                        }
                    }
                }

                // 判断是否有人在战斗
                bool anyFighting = false;
                foreach (var m in allMembers) { if (m != null && m.isAlive() && m.isFighting()) { anyFighting = true; break; } }

                // 主动寻敌：当没人战斗时，按节流寻找附近敌方单位并下达攻击
                long armyId = army.data.id;
                if (!anyFighting)
                {
                    float seekCd = 0f;
                    _armySeekCD.TryGetValue(armyId, out seekCd);
                    seekCd -= dt;

                    if (seekCd <= 0f)
                    {
                        _armySeekCD[armyId] = ARMY_SEEK_INTERVAL;

                        var centerTile = captain.current_tile;
                        var myKingdom = armyCity?.kingdom ?? captain.kingdom;
                        if (centerTile != null && myKingdom != null)
                        {
                            var enemy = FindNearestEnemyUnit(centerTile, myKingdom, ARMY_SEEK_RADIUS);
                            if (enemy != null)
                            {
                                OrderArmyAttackTarget(allMembers, enemy);
                                // 可选：提前认为进入战斗态，从而触发“战斗限定法术”
                                // anyFighting = true;
                            }
                        }
                    }
                    else
                    {
                        _armySeekCD[armyId] = seekCd;
                    }
                }

                // 军队法术（以队长战勋为门槛）
                int captainZhanXunLevel = GetZhanXunLevel(captain);
                if (!armySpellCooldowns.ContainsKey(armyId))
                    armySpellCooldowns[armyId] = new Dictionary<string, float>();
                var cooldownDict = armySpellCooldowns[armyId];

                if (ZhanXunConfig.AutoCollectMilitarymagic)
                {
                    foreach (var spell in ArmySpells)
                    {
                        if (captainZhanXunLevel < spell.minZhanXunLevel || captainZhanXunLevel > spell.maxZhanXunLevel)
                            continue;

                        cooldownDict.TryGetValue(spell.id, out float cd);
                        cd = Mathf.Max(0f, cd - dt);
                        cooldownDict[spell.id] = cd;
                        if (cd > 0f) continue;

                        // 部分法术仅在战斗时释放
                        if (spell.id is "Johnson" or "Selfde" or "Ironarmor" or "TheUnmovingWiseKing" or
                            "enhancement" or "andblood" or "Overload" or "evilenergy_aura")
                        {
                            if (!anyFighting) continue;
                        }

                        bool ok = false;
                        if (spell.id == "teleport")
                        {
                            if (!ZhanXunConfig.AutoCollectMilitaryteleportation)
                            {
                                continue;
                            }
                            // 修改：只传送到非军国主义的敌国城市
                            City targetCity = FindNearestEnemyCityNonMilitaristic(captain);
                            if (targetCity != null)
                            {
                                var zone = targetCity.zones.GetRandom<TileZone>();
                                var tile = zone?.centerTile;
                                if (tile != null)
                                {
                                    foreach (var m in allMembers)
                                    {
                                        if (m != null && m.isAlive())
                                        {
                                            ActionLibrary.teleportEffect(m, tile);
                                            m.cancelAllBeh();
                                            m.spawnOn(tile, 0f);
                                        }
                                    }
                                    ok = true;
                                }
                            }
                        }
                        else
                        {
                            foreach (var m in allMembers)
                            {
                                if (m == null || !m.isAlive()) continue;

                                switch (spell.id)
                                {
                                    case "heal_small":
                                        m.restoreHealth(m.getMaxHealthPercent(0.1f)); ok = true; break;
                                    case "heal_medium":
                                        m.restoreHealth(m.getMaxHealthPercent(0.2f)); ok = true; break;
                                    case "heal_large":
                                        m.restoreHealth(m.getMaxHealthPercent(0.5f)); ok = true; break;
                                    case "heal_Super":
                                        m.restoreHealth(m.getMaxHealthPercent(0.7f)); ok = true; break;
                                    case "heal_Ultimate":
                                        m.restoreHealth(m.getMaxHealthPercent(1f)); ok = true; break;
                                    default:
                                        m.addStatusEffect(spell.statusId, spell.duration, true); ok = true; break;
                                }
                            }
                        }

                        if (ok) cooldownDict[spell.id] = spell.cooldown;
                    }
                }
            }
        }

        // =============================
        // Patch：城市状态（军国）
        // =============================
        [HarmonyPostfix]
        [HarmonyPatch(typeof(City), "updateCityStatus")]
        public static void City_updateCityStatus_MilitarismPatch(City __instance)
        {
            if (__instance.kingdom == null) return;
            var kingdom = __instance.kingdom;

            bool shouldBeMilitarism = GetMilitarism(kingdom.data);
            if (!shouldBeMilitarism)
            {
                foreach (var actor in kingdom.getUnits())
                {
                    if (actor == null) continue;
                    foreach (var trait in MilitarismTraits)
                    {
                        if (actor.hasTrait(trait)) { shouldBeMilitarism = true; break; }
                    }
                    if (shouldBeMilitarism) break;
                }
            }

            if (shouldBeMilitarism && !GetMilitarism(kingdom.data))
            {
                SetMilitarism(kingdom.data, true);
                foreach (var city in kingdom.cities)
                    city?.updateCityStatus();
            }

            if (GetMilitarism(kingdom.data))
            {
                SyncMilitarismLeaders(kingdom);
                int adultPop = 0;
                foreach (var actor in __instance.units)
                    if (actor != null && actor.isAlive() && actor.isAdult()) adultPop++;
                __instance.status.warrior_slots = (int)(adultPop * 0.9f);
            }
        }

        // =============================
        // Patch：国家法术（首都城市更新处，修正传送只执行一次）
        // =============================
        [HarmonyPostfix]
        [HarmonyPatch(typeof(City), "update")]
        public static void City_update_CountrySpell_Postfix(City __instance, float pElapsed)
        {
            if (__instance == null || __instance.kingdom == null) return;

            if (GetMilitarism(__instance.kingdom.data))
            {
                int currentWarriors = __instance.countWarriors();
                int maxWarriors = __instance.getMaxWarriors();
                if (currentWarriors < maxWarriors)
                {
                    int need = maxWarriors - currentWarriors;
                    foreach (var a in __instance.units)
                    {
                        if (need <= 0) break;
                        if (a == null || !a.isAlive() || !a.isAdult() || a.city != __instance) continue;
                        if (a.city.kingdom != __instance.kingdom || a.isWarrior() || a.isCityLeader() || a.isKing()) continue;
                        if (__instance.checkCanMakeWarrior(a))
                        {
                            __instance.makeWarrior(a);
                            need--;
                        }
                    }
                }
            }

            var kingdom = __instance.kingdom;
            if (kingdom.capital != __instance) return;

            var king = kingdom.king;
            if (king == null || !king.isAlive()) return;

            // 仅军国可用国家法术
            if (!GetMilitarism(kingdom.data)) return;

            int kingLv = GetZhanXunLevel(king);
            long kingdomId = kingdom.data.id;
            if (!countrySpellCooldowns.ContainsKey(kingdomId))
                countrySpellCooldowns[kingdomId] = new Dictionary<string, float>();
            var cdDict = countrySpellCooldowns[kingdomId];

            if (!ZhanXunConfig.AutoCollectMilitarystatemagic) return;

            foreach (var spell in CountrySpells)
            {
                if (kingLv < spell.minZhanXunLevel || kingLv > spell.maxZhanXunLevel) continue;
                if (IsSpellRequireWar(spell.id) && !IsKingdomInWar(kingdom)) continue;

                cdDict.TryGetValue(spell.id, out float cd);
                cd = Mathf.Max(0f, cd - pElapsed);
                cdDict[spell.id] = cd;
                if (cd > 0f) continue;

                bool ok = false;

                if (spell.id == "teleport")
                {
                    if (!ZhanXunConfig.AutoCollectMilitarystateteleportation)
                    {
                        continue;
                    }
                    City targetCapital = FindEnemyCapital(kingdom);
                    if (targetCapital != null)
                    {
                        var zone = targetCapital.zones.GetRandom<TileZone>();
                        var tile = zone?.centerTile;
                        if (tile != null)
                        {
                            var teleported = new HashSet<Actor>();

                            if (king != null && king.isAlive())
                            {
                                ActionLibrary.teleportEffect(king, tile);
                                king.cancelAllBeh();
                                king.spawnOn(tile, 0f);
                                teleported.Add(king);
                            }

                            foreach (var army in World.world.armies)
                            {
                                if (army?.getCity()?.kingdom != kingdom) continue;

                                var ac = CaptainOf(army);
                                if (ac != null && ac.isAlive() && teleported.Add(ac))
                                {
                                    ActionLibrary.teleportEffect(ac, tile);
                                    ac.cancelAllBeh();
                                    ac.spawnOn(tile, 0f);
                                }

                                foreach (var u in army.units)
                                {
                                    if (u != null && u.isAlive() && teleported.Add(u))
                                    {
                                        ActionLibrary.teleportEffect(u, tile);
                                        u.cancelAllBeh();
                                        u.spawnOn(tile, 0f);
                                    }
                                }
                            }

                            ok = teleported.Count > 0;
                        }
                    }
                }
                else
                {
                    foreach (var u in kingdom.getUnits())
                    {
                        if (u == null || !u.isAlive()) continue;
                        switch (spell.id)
                        {
                            case "heal_small": u.restoreHealth(u.getMaxHealthPercent(0.1f)); ok = true; break;
                            case "heal_medium": u.restoreHealth(u.getMaxHealthPercent(0.2f)); ok = true; break;
                            case "heal_large": u.restoreHealth(u.getMaxHealthPercent(0.5f)); ok = true; break;
                            case "heal_Super": u.restoreHealth(u.getMaxHealthPercent(0.7f)); ok = true; break;
                            case "heal_Ultimate": u.restoreHealth(u.getMaxHealthPercent(1f)); ok = true; break;
                            default: u.addStatusEffect(spell.statusId, spell.duration, true); ok = true; break;
                        }
                    }
                }

                if (ok) cdDict[spell.id] = spell.cooldown;
            }
        }

        // =============================
        // Patch：城市扩张/建筑/补给/产出
        // =============================
        [HarmonyPostfix]
        [HarmonyPatch(typeof(City), nameof(City.addZone))]
        public static void MilitarismExpandNeighboursPatch(City __instance, TileZone pZone)
        {
            if (__instance.kingdom != null && GetMilitarism(__instance.kingdom.data))
            {
                bool changed = false;
                foreach (var neighbour in pZone.neighbours_all)
                {
                    if (neighbour != null && neighbour.city == null)
                    {
                        neighbour.setCity(__instance);
                        __instance.zones.Add(neighbour);
                        changed = true;
                    }
                }
                if (changed)
                {
                    __instance.updateCityCenter();
                    __instance.setStatusDirty();
                }
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Actor), nameof(Actor.getConstructionSpeed))]
        public static void GetConstructionSpeed_MilitarismPatch(Actor __instance, ref int __result)
        {
            if (__instance.city?.kingdom != null && GetMilitarism(__instance.city.kingdom.data))
                __result *= 10;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(CityBehBuild), "execute")]
        public static void CityBehBuild_execute_MilitarismPatch(City pCity)
        {
            if (pCity.kingdom != null && GetMilitarism(pCity.kingdom.data))
                pCity.timer_build = 0.5f;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(CityBehSupplyKingdomCities), "updateSupplyTimer")]
        public static void CityBehSupplyKingdomCities_updateSupplyTimer_Postfix(City pCity)
        {
            if (pCity.kingdom != null && GetMilitarism(pCity.kingdom.data))
                pCity.data.timer_supply = 0f;
        }

        // 性能优化：通过 shareResource 打补丁的标志避免 StackTrace
        [HarmonyPrefix]
        [HarmonyPatch(typeof(CityBehSupplyKingdomCities), "shareResource")]
        public static void CityBehSupplyKingdomCities_shareResource_Prefix() => _inShareResource = true;

        [HarmonyFinalizer]
        [HarmonyPatch(typeof(CityBehSupplyKingdomCities), "shareResource")]
        public static void CityBehSupplyKingdomCities_shareResource_Finalizer() => _inShareResource = false;

        [HarmonyPrefix]
        [HarmonyPatch(typeof(City), "addResourcesToRandomStockpile")]
        public static void City_addResourcesToRandomStockpile_Prefix(City __instance, string pResourceID, ref int pAmount)
        {
            if (__instance.kingdom != null && GetMilitarism(__instance.kingdom.data))
            {
                if (_inShareResource) return; // 城际分享时不加速
                long v = (long)pAmount * 999L; // 可改为配置
                pAmount = v >= int.MaxValue ? int.MaxValue : (int)v;
            }
        }

        // =============================
        // Patch：占领流程屏蔽（军国 + 全局开关）
        // =============================
        [HarmonyPrefix]
        [HarmonyPatch(typeof(City), "addCapturePoints", new Type[] { typeof(Kingdom), typeof(int) })]
        public static bool City_addCapturePoints_Kingdom_Prefix(City __instance, Kingdom pKingdom, int pValue)
            => !ShouldBlockCapture(__instance);

        [HarmonyPrefix]
        [HarmonyPatch(typeof(City), "addCapturePoints", new Type[] { typeof(BaseSimObject), typeof(int) })]
        public static bool City_addCapturePoints_BaseSimObject_Prefix(City __instance, BaseSimObject pObject, int pValue)
            => !ShouldBlockCapture(__instance);

        [HarmonyPrefix]
        [HarmonyPatch(typeof(City), "finishCapture")]
        public static bool City_finishCapture_Prefix(City __instance, Kingdom pNewKingdom)
            => !ShouldBlockCapture(__instance);

        [HarmonyPostfix]
        [HarmonyPatch(typeof(City), "updateCapture", new Type[] { typeof(float) })]
        public static void City_updateCapture_Postfix(City __instance, float pElapsed)
        {
            if (ShouldBlockCapture(__instance))
                __instance.clearCapture();
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(City), "isGettingCaptured")]
        public static bool City_isGettingCaptured_Prefix(City __instance, ref bool __result)
        {
            if (ShouldBlockCapture(__instance))
            {
                __result = false;
                return false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(City), "isGettingCapturedBy")]
        public static bool City_isGettingCapturedBy_Prefix(City __instance, Kingdom pKingdom, ref bool __result)
        {
            if (ShouldBlockCapture(__instance))
            {
                __result = false;
                return false;
            }
            return true;
        }

        // =============================
        // Patch：装备耐久免损
        // =============================
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Item), "getDamaged")]
        public static bool Item_getDamaged_Prefix(Item __instance, int pDamage) => false;

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Actor), "getHit")]
        [HarmonyPriority(Priority.First)]
        public static bool Actor_getHit_AbsoluteHitPatch(
            Actor __instance,
            ref float pDamage,
            bool pFlash,
            AttackType pAttackType,
            BaseSimObject pAttacker,
            bool pSkipIfShake,
            bool pMetallicWeapon,
            bool pCheckDamageReduction = true)
        {
            // 状态触发：仅当攻击者拥有这些状态之一时启用“绝对伤害”
            // 可按需改成你自己的状态ID
            Actor attackerActor = (pAttacker != null && pAttacker.isActor()) ? pAttacker.a : null;
            bool triggerAbsoluteHit =
                attackerActor != null &&
                (attackerActor.hasStatus("evilenergy_aura"));

            // 未触发状态 -> 完全走原版
            if (!triggerAbsoluteHit)
                return true;

            __instance._last_attack_type = pAttackType;

            __instance.attackedBy = null;
            if (pAttacker != null && !pAttacker.isRekt() && pAttacker != __instance)
                __instance.attackedBy = pAttacker;

            if (!__instance.hasHealth()) return false;

            // 在这里设置伤害倍数（仅改伤害部分）
            const float DamageMultiplier = 1f; // TODO: 自行调整倍数
            float finalDamageF = pDamage * DamageMultiplier;
            int finalDamage = Mathf.Max(1, Mathf.RoundToInt(finalDamageF)); // 下限1，避免0伤害

            __instance.changeHealth(-finalDamage);
            __instance.timer_action = 0.002f;

            if (pFlash) __instance.startColorEffect(ActorColorEffect.Red);

            GetHitAction getHitAction = __instance.s_get_hit_action;
            getHitAction?.Invoke(__instance, pAttacker, __instance.current_tile);

            bool isDead = !__instance.hasHealth();
            if (isDead) __instance.batch.c_check_deaths.Add(__instance);

            if (!isDead && pAttackType == AttackType.Weapon && !__instance.asset.immune_to_injuries)
            {
                if (Randy.randomChance(0.02f)) __instance.addInjuryTrait("crippled");
                if (Randy.randomChance(0.02f)) __instance.addInjuryTrait("eyepatch");
            }

            __instance.startShake(0.3f, 0.1f, true, true);

            if (!isDead && !__instance.has_attack_target)
            {
                if (__instance.attackedBy != null && !__instance.shouldIgnoreTarget(__instance.attackedBy) && __instance.canAttackTarget(__instance.attackedBy, false))
                    __instance.setAttackTarget(__instance.attackedBy);
            }

            if (!isDead && __instance.hasAnyStatusEffect())
            {
                foreach (Status status in __instance.getStatuses())
                    status.asset.action_get_hit?.Invoke(__instance, pAttacker, __instance.current_tile);
            }

            __instance.asset.action_get_hit?.Invoke(__instance, pAttacker, __instance.current_tile);

            if (isDead) __instance.checkCallbacksOnDeath();

            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(BabyHelper), nameof(BabyHelper.isMetaLimitsReached))]
        public static bool BabyHelper_isMetaLimitsReached_Postfix(Actor pActor, ref bool __result)
        {
            // 重要人物（本人或其恋人）不受任何限制
            Actor tLover = pActor.lover;
            if (pActor.isImportantPerson() || (tLover != null && tLover.isImportantPerson()))
            {
                __result = false;
                return false;
            }

            if (pActor.subspecies.hasReachedPopulationLimit())
            {
                __result = true;
                return false;
            }

            if (pActor.hasCity())
            {
                var city = pActor.city;

                // 0 为无限制；>0 时启用配置上限
                float limit = ZhanXunConfig.AutoCollectUrbanpopulation;
                if (limit > 0f && city.getPopulationPeople() >= limit)
                {
                    __result = true;
                    return false;
                }

                if (pActor.subspecies.isReproductionSexual() && pActor.current_children_count == 0)
                {
                    __result = false;
                    return false;
                }

                if (!city.hasFreeHouseSlots())
                {
                    __result = true;
                    return false;
                }
            }

            __result = false;
            return false; // 始终跳过原方法
        }

        [HarmonyPostfix, HarmonyPatch(typeof(MapBox), "updateObjectAge")]
        public static void updateWorldTime_Postfix(MapBox __instance)
        {
            // 检查并触发全球战争
            CheckAndTriggerGlobalWar();
        }

        private static void CheckAndTriggerGlobalWar()
        {
            if (!ZhanXunConfig.AutoCollectEternalWar)
                return;

            bool hasTriggerTrait = true;
            if (hasTriggerTrait)
            {
                // 获取所有王国
                List<Kingdom> allKingdoms = World.world.kingdoms.list;

                // 让每个王国对其他所有王国宣战
                foreach (Kingdom attacker in allKingdoms)
                {
                    foreach (Kingdom defender in allKingdoms)
                    {
                        // 确保不是同一个王国
                        if (attacker != defender)
                        {
                            // 检查是否已经在战争中
                            if (!attacker.isInWarWith(defender))
                            {
                                // 使用普通战争类型
                                WarTypeAsset warType = AssetManager.war_types_library.get("normal");
                                if (warType != null)
                                {
                                    // 开始战争
                                    World.world.diplomacy.startWar(attacker, defender, warType);
                                }
                            }
                        }
                    }
                }
            }

            // 检查单国家情况并触发城市叛乱
            CheckAndTriggerCityRebellion();
        }

        // 检查并触发城市叛乱
        private static void CheckAndTriggerCityRebellion()
        {
            // 检查是否有任何单位具有特定特质
            bool hasTriggerTrait = true;

            if (!hasTriggerTrait)
            {
                return;
            }

            // 获取所有王国
            List<Kingdom> allKingdoms = World.world.kingdoms.list;

            // 如果只有一个王国，触发叛乱
            if (allKingdoms.Count == 1)
            {
                Kingdom kingdom = allKingdoms[0];
                List<City> cities = kingdom.cities;

                // 遍历所有城市
                foreach (City city in cities)
                {
                    // 跳过首都
                    if (city == kingdom.capital)
                    {
                        continue;
                    }

                    // 从城市中随机选择一个活着的非国王单位作为新国王
                    Actor newKing = null;
                    foreach (Actor actor in city.units)
                    {
                        if (actor.isAlive() && !actor.isKing())
                        {
                            newKing = actor;
                            break;
                        }
                    }

                    if (newKing != null)
                    {
                        // 创建新的王国
                        Kingdom newKingdom = World.world.kingdoms.makeNewCivKingdom(newKing, city.data.name);
                        if (newKingdom != null)
                        {
                            // 设置新王国的属性
                            newKingdom.data.name = city.data.name + " Kingdom";
                            newKingdom.capital = city;

                            // 将城市从原王国转移到新王国
                            city.setKingdom(newKingdom);

                            // 转移城市中的所有单位到新王国
                            foreach (Actor unit in city.units.ToList())
                            {
                                unit.setKingdom(newKingdom);
                            }

                            // 为新王国添加随机特性
                            MetaHelper.addRandomTrait<KingdomTrait>(newKingdom, AssetManager.kingdoms_traits);
                        }
                    }
                }
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Actor), "calcIsBaby")]
        public static bool calcIsBaby()
        {
            return !ZhanXunConfig.AutoCollectAdulthood;
        }
    }
}