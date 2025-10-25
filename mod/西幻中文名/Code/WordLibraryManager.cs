diff --git a/mod/西幻中文名/Code/WordLibraryManager.cs b/mod/西幻中文名/Code/WordLibraryManager.cs
index fba866e3a12b88bb16ea25feea74f79fec4c516e..ccc45008fc39988fbc5ca6553acb8a12aec06075 100644
--- a/mod/西幻中文名/Code/WordLibraryManager.cs
+++ b/mod/西幻中文名/Code/WordLibraryManager.cs
@@ -4,65 +4,99 @@ using System.Linq;
 using Chinese_Name.utils;
 using UnityEngine;
 
 namespace Chinese_Name;
 
 public class WordLibraryManager : AssetLibrary<WordLibraryAsset>
 {
     internal static readonly WordLibraryManager Instance = new();
     private static HashSet<string> submitted_dir = new HashSet<string>();
     public override void init()
     {
         base.init();
         id = "WordLibraryManager";
         SubmitDirectoryToLoad(Path.Combine(ModClass.Instance.GetDeclaration().FolderPath, "word_libraries/default"));
     }
     internal void Reload()
     {
         HashSet<string> reload_dir = new HashSet<string>(submitted_dir);
         submitted_dir.Clear();
         foreach (var dir in reload_dir)
         {
             SubmitDirectoryToLoad(dir);
         }
     }
     /// <summary>
-    /// ָĴʿȡһ
+    /// ָĴʿȡһ
     /// </summary>
-    /// <param name="pId">ָʿid, ΪӦļļȥ׺</param>
-    /// <returns>ָʿһ, ʿⲻ򷵻ؿմ</returns>
+    /// <param name="pId">ָʿid, ΪӦļļȥ׺</param>
+    /// <returns>ָʿһ, ʿⲻ򷵻ؿմ</returns>
     public static string GetRandomWord(string pId)
     {
         if (Instance.dict.TryGetValue(pId, out WordLibraryAsset asset) && asset.words.Count > 0)
         {
             return Instance.dict[pId].GetRandom();
         }
         return "";
     }
     public static void SubmitDirectoryToLoad(string pDirectory)
     {
         if (submitted_dir.Contains(pDirectory)) return;
         TextAsset[] text_assets = GeneralUtils.LoadAllFrom(pDirectory);
         foreach (TextAsset text_asset in text_assets)
         {
-            Instance.add(new WordLibraryAsset(text_asset.name, text_asset.text.Replace("\r", "").Split('\n').ToList()));
+            var words = NormalizeWords(text_asset.text.Replace("\r", "").Split('\n'));
+            AddOrReplace(text_asset.name, words);
         }
         submitted_dir.Add(pDirectory);
     }
 
     public static void Submit(string pId, List<string> pWords)
     {
+        AddOrReplace(pId, NormalizeWords(pWords));
+    }
+
+    private static void AddOrReplace(string pId, List<string> pWords)
+    {
+        if (Instance.dict.TryGetValue(pId, out var existingAsset))
+        {
+            existingAsset.words.Clear();
+            existingAsset.words.AddRange(pWords);
+            return;
+        }
+
         Instance.add(new WordLibraryAsset(pId, pWords));
     }
 
     public static void SubmitForPatch(string pId, List<string> pWords)
     {
-        if (Instance.dict.ContainsKey(pId))
+        var sanitizedWords = NormalizeWords(pWords);
+        if (Instance.dict.TryGetValue(pId, out var existingAsset))
         {
-            Instance.dict[pId].words.AddRange(pWords);
+            foreach (var word in sanitizedWords)
+            {
+                if (!existingAsset.words.Contains(word))
+                {
+                    existingAsset.words.Add(word);
+                }
+            }
         }
         else
         {
-            Instance.add(new WordLibraryAsset(pId, pWords));
+            Instance.add(new WordLibraryAsset(pId, sanitizedWords));
         }
     }
-}
\ No newline at end of file
+
+    private static List<string> NormalizeWords(IEnumerable<string> pWords)
+    {
+        if (pWords == null)
+        {
+            return new List<string>();
+        }
+
+        return pWords
+            .Select(word => word?.Trim())
+            .Where(word => !string.IsNullOrEmpty(word))
+            .Distinct()
+            .ToList();
+    }
+}
