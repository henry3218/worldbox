using System.Collections.Generic;
using System.IO;
using System.Linq;
using Chinese_Name.utils;
using UnityEngine;

namespace Chinese_Name;

public class WordLibraryManager : AssetLibrary<WordLibraryAsset>
{
    internal static readonly WordLibraryManager Instance = new();
    private static readonly HashSet<string> submitted_dir = new();

    public override void init()
    {
        base.init();
        id = "WordLibraryManager";
        SubmitDirectoryToLoad(Path.Combine(ModClass.Instance.GetDeclaration().FolderPath, "word_libraries/default"));
    }

    internal void Reload()
    {
        var reloadDir = new HashSet<string>(submitted_dir);
        submitted_dir.Clear();
        foreach (var dir in reloadDir)
        {
            SubmitDirectoryToLoad(dir);
        }
    }

    public static string GetRandomWord(string pId)
    {
        if (Instance.dict.TryGetValue(pId, out var asset) && asset.words.Count > 0)
        {
            return asset.GetRandom();
        }

        return string.Empty;
    }

    public static void SubmitDirectoryToLoad(string pDirectory)
    {
        if (submitted_dir.Contains(pDirectory))
        {
            return;
        }

        var textAssets = GeneralUtils.LoadAllFrom(pDirectory);
        foreach (var textAsset in textAssets)
        {
            var words = NormalizeWords(textAsset.text.Replace("\r", string.Empty).Split('\n'));
            AddOrReplace(textAsset.name, words);
        }

        submitted_dir.Add(pDirectory);
    }

    public static void Submit(string pId, List<string> pWords)
    {
        AddOrReplace(pId, NormalizeWords(pWords));
    }

    private static void AddOrReplace(string pId, List<string> pWords)
    {
        if (Instance.dict.TryGetValue(pId, out var existingAsset))
        {
            existingAsset.words.Clear();
            existingAsset.words.AddRange(pWords);
        }
        else
        {
            Instance.add(new WordLibraryAsset(pId, pWords));
        }
    }

    public static void SubmitForPatch(string pId, List<string> pWords)
    {
        var sanitizedWords = NormalizeWords(pWords);
        if (Instance.dict.TryGetValue(pId, out var existingAsset))
        {
            foreach (var word in sanitizedWords)
            {
                if (!existingAsset.words.Contains(word))
                {
                    existingAsset.words.Add(word);
                }
            }
        }
        else
        {
            Instance.add(new WordLibraryAsset(pId, sanitizedWords));
        }
    }

    private static List<string> NormalizeWords(IEnumerable<string> pWords)
    {
        if (pWords == null)
        {
            return new List<string>();
        }

        return pWords
            .Select(word => word?.Trim())
            .Where(word => !string.IsNullOrEmpty(word))
            .Distinct()
            .ToList();
    }
}
