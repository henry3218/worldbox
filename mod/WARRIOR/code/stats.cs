diff --git a/mod/WARRIOR/code/stats.cs b/mod/WARRIOR/code/stats.cs
index e192474c0fe97cb50b4b4d9b63bafcea8a275aba..5fe7ca1cca2184649467eff5671ebd2e88009673 100644
--- a/mod/WARRIOR/code/stats.cs
+++ b/mod/WARRIOR/code/stats.cs
@@ -1,95 +1,58 @@
-﻿namespace PeerlessOverpoweringWarrior.code
+namespace PeerlessOverpoweringWarrior.code
 {
     internal class stats
     {
         public static BaseStatAsset Resist;
         public static BaseStatAsset TrueGang;
         public static BaseStatAsset TrueGangTrueDamageMultiplier; // 真罡真伤倍数
         public static BaseStatAsset TrueGangHealMultiplier; // 真罡回血倍数
         public static BaseStatAsset TrueGangDamageReductionMultiplier; // 真罡伤害稀释倍数
 
         public static BaseStatAsset Pattern;
 
         public static void Init()
         {
-            BaseStatAsset Warrior = new BaseStatAsset();
-            Warrior.id = "Warrior";
-            Warrior.normalize = true;
-            Warrior.normalize_min = -9999999;
-            Warrior.normalize_max = 9999999;
-            //Warrior.multiplier = true;
-            Warrior.used_only_for_civs = false;
-            AssetManager.base_stats_library.add(Warrior);
+            EnsureStat("Warrior", -9999999f, 9999999f);
 
             // 定义阵纹属性
-            Pattern = new BaseStatAsset();
-            Pattern.id = "Pattern";
-            Pattern.normalize = true;
-            Pattern.normalize_min = -9999999;
-            Pattern.normalize_max = 9999999;
-            Pattern.used_only_for_civs = false;
-            AssetManager.base_stats_library.add(Pattern);
+            Pattern = EnsureStat("Pattern", -9999999f, 9999999f);
 
             // 定义真罡属性，与武道气血使用相同的上下限
-            TrueGang = new BaseStatAsset();
-            TrueGang.id = "TrueGang";
-            TrueGang.normalize = true;
-            TrueGang.normalize_min = -9999999;
-            TrueGang.normalize_max = 9999999;
-            TrueGang.used_only_for_civs = false;
-            AssetManager.base_stats_library.add(TrueGang);
+            TrueGang = EnsureStat("TrueGang", -9999999f, 9999999f);
 
             // 定义真罡真伤倍数属性
-            TrueGangTrueDamageMultiplier = new BaseStatAsset();
-            TrueGangTrueDamageMultiplier.id = "TrueGangTrueDamageMultiplier";
-            TrueGangTrueDamageMultiplier.normalize = true;
-            TrueGangTrueDamageMultiplier.normalize_min = 0;
-            TrueGangTrueDamageMultiplier.normalize_max = 999999;
-            TrueGangTrueDamageMultiplier.used_only_for_civs = false;
-            AssetManager.base_stats_library.add(TrueGangTrueDamageMultiplier);
+            TrueGangTrueDamageMultiplier = EnsureStat("TrueGangTrueDamageMultiplier", 0f, 999999f);
 
             // 定义真罡回血倍数属性
-            TrueGangHealMultiplier = new BaseStatAsset();
-            TrueGangHealMultiplier.id = "TrueGangHealMultiplier";
-            TrueGangHealMultiplier.normalize = true;
-            TrueGangHealMultiplier.normalize_min = 0;
-            TrueGangHealMultiplier.normalize_max = 999999;
-            TrueGangHealMultiplier.used_only_for_civs = false;
-            AssetManager.base_stats_library.add(TrueGangHealMultiplier);
+            TrueGangHealMultiplier = EnsureStat("TrueGangHealMultiplier", 0f, 999999f);
 
             // 定义真罡伤害稀释倍数属性
-            TrueGangDamageReductionMultiplier = new BaseStatAsset();
-            TrueGangDamageReductionMultiplier.id = "TrueGangDamageReductionMultiplier";
-            TrueGangDamageReductionMultiplier.normalize = true;
-            TrueGangDamageReductionMultiplier.normalize_min = 0;
-            TrueGangDamageReductionMultiplier.normalize_max = 999999;
-            TrueGangDamageReductionMultiplier.used_only_for_civs = false;
-            AssetManager.base_stats_library.add(TrueGangDamageReductionMultiplier);
+            TrueGangDamageReductionMultiplier = EnsureStat("TrueGangDamageReductionMultiplier", 0f, 999999f);
 
             // 定义 Resist 属性
-            Resist = new BaseStatAsset();
-            Resist.id = "Resist";
-            Resist.normalize = true;
-            Resist.normalize_min = 0;
-            Resist.normalize_max = 999999;
-            Resist.used_only_for_civs = false;
-            AssetManager.base_stats_library.add(Resist);
+            Resist = EnsureStat("Resist", 0f, 999999f);
 
-            BaseStatAsset Dodge = new BaseStatAsset();
-            Dodge.id = "Dodge";// 闪避率
-            Dodge.normalize = true;
-            Dodge.normalize_min = 0;
-            Dodge.normalize_max = 99999;
-            Dodge.used_only_for_civs = false;
-            AssetManager.base_stats_library.add(Dodge);
+            EnsureStat("Dodge", 0f, 99999f);
+            EnsureStat("Accuracy", 0f, 99999f);
+        }
 
-            BaseStatAsset Accuracy = new BaseStatAsset();
-            Accuracy.id = "Accuracy";// 命中率
-            Accuracy.normalize = true;
-            Accuracy.normalize_min = 0;
-            Accuracy.normalize_max = 99999;
-            Accuracy.used_only_for_civs = false;
-            AssetManager.base_stats_library.add(Accuracy);
+        private static BaseStatAsset EnsureStat(string pId, float pMin, float pMax)
+        {
+            var existing = AssetManager.base_stats_library.get(pId);
+            if (existing == null)
+            {
+                existing = new BaseStatAsset
+                {
+                    id = pId
+                };
+                AssetManager.base_stats_library.add(existing);
+            }
+
+            existing.normalize = true;
+            existing.normalize_min = pMin;
+            existing.normalize_max = pMax;
+            existing.used_only_for_civs = false;
+            return existing;
         }
     }
-}
\ No newline at end of file
+}
