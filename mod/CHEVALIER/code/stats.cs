diff --git a/mod/CHEVALIER/code/stats.cs b/mod/CHEVALIER/code/stats.cs
index aecccbebd38802857f546b393ba30e2c699fa30e..0690b3affa9b9e0570f24298379603fda4d2cf7a 100644
--- a/mod/CHEVALIER/code/stats.cs
+++ b/mod/CHEVALIER/code/stats.cs
@@ -1,54 +1,41 @@
-﻿namespace Chevalier.code
+namespace Chevalier.code
 {
     internal class stats
     {
         public static BaseStatAsset Resist;
         public static BaseStatAsset Comprehension;
 
         public static void Init()
         {
-            BaseStatAsset Chevalier = new BaseStatAsset();
-            Chevalier.id = "Chevalier";
-            Chevalier.normalize = true;
-            Chevalier.normalize_min = -9999999;
-            Chevalier.normalize_max = 9999999;
-            //Chevalier.multiplier = true;
-            Chevalier.used_only_for_civs = false;
-            AssetManager.base_stats_library.add(Chevalier);
+            EnsureStat("Chevalier", -9999999f, 9999999f);
 
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
-
-            BaseStatAsset Accuracy = new BaseStatAsset();
-            Accuracy.id = "Accuracy";// 命中率
-            Accuracy.normalize = true;
-            Accuracy.normalize_min = 0;
-            Accuracy.normalize_max = 99999;
-            Accuracy.used_only_for_civs = false;
-            AssetManager.base_stats_library.add(Accuracy);
+            EnsureStat("Dodge", 0f, 99999f);
+            EnsureStat("Accuracy", 0f, 99999f);
 
             // 定义领悟度属性
-            Comprehension = new BaseStatAsset();
-            Comprehension.id = "Comprehension";
-            Comprehension.normalize = true;
-            Comprehension.normalize_min = 0;
-            Comprehension.normalize_max = 99999;
-            Comprehension.used_only_for_civs = false;
-            AssetManager.base_stats_library.add(Comprehension);
+            Comprehension = EnsureStat("Comprehension", 0f, 99999f);
+        }
+
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
