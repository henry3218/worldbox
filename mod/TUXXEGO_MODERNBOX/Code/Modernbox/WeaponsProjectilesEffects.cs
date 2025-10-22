//========= MODERNBOX 2.1.0.1 ============//
// Made by Tuxxego
//========================================//

using System;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;
using NCMS.Utils;
using NCMS;
using System.Linq;

namespace ModernBox
{
    public static class CustomItemsList
    {
        public static List<EquipmentAsset> CustomWeapons = new List<EquipmentAsset>();
        public static bool GunsAllowed;
        public static bool MirvsAllowed = true;


        public static readonly HashSet<string> Kys = new HashSet<string>
        {
            "BudgetMIRV",
            "DecentMIRV",
            "MIRV",
            "MIRVBomb",
            "STRONGMIRV"
        };

        public static void InitCustomItems()
        {
            if (!AssetManager.items.dict.ContainsKey("Glock17"))
                return;

            CustomWeapons.Clear();
            CustomWeapons.Add(AssetManager.items.get("Glock17"));
            CustomWeapons.Add(AssetManager.items.get("AK"));
            CustomWeapons.Add(AssetManager.items.get("RPG"));
            CustomWeapons.Add(AssetManager.items.get("Minigun"));
            CustomWeapons.Add(AssetManager.items.get("Sniper"));
            CustomWeapons.Add(AssetManager.items.get("FAMAS"));
            CustomWeapons.Add(AssetManager.items.get("M4A1"));
            CustomWeapons.Add(AssetManager.items.get("ThompsonM1A1"));
            CustomWeapons.Add(AssetManager.items.get("SGT44"));
            CustomWeapons.Add(AssetManager.items.get("XM8"));
            CustomWeapons.Add(AssetManager.items.get("AK103"));
            CustomWeapons.Add(AssetManager.items.get("Uzi"));
            CustomWeapons.Add(AssetManager.items.get("Malorian"));
            CustomWeapons.Add(AssetManager.items.get("DesertEagle"));
            CustomWeapons.Add(AssetManager.items.get("M16"));
            CustomWeapons.Add(AssetManager.items.get("HK416"));
            CustomWeapons.Add(AssetManager.items.get("MP7"));
            CustomWeapons.Add(AssetManager.items.get("M32"));
            CustomWeapons.Add(AssetManager.items.get("Sluggershotgun"));
            CustomWeapons.Add(AssetManager.items.get("Americanshotgun"));
            CustomWeapons.Add(AssetManager.items.get("Flamethrower"));
            CustomWeapons.Add(AssetManager.items.get("vrifle"));
            CustomWeapons.Add(AssetManager.items.get("bigboy"));
            CustomWeapons.Add(AssetManager.items.get("grifle"));
            CustomWeapons.Add(AssetManager.items.get("MGL"));
            CustomWeapons.Add(AssetManager.items.get("BudgetMIRV"));
            CustomWeapons.Add(AssetManager.items.get("DecentMIRV"));
            CustomWeapons.Add(AssetManager.items.get("MIRV"));
            CustomWeapons.Add(AssetManager.items.get("MIRVBomb"));
            CustomWeapons.Add(AssetManager.items.get("STRONGMIRV"));
            CustomWeapons.Add(AssetManager.items.get("BathSalts"));
            CustomWeapons.Add(AssetManager.items.get("Fentanyl"));
            CustomWeapons.Add(AssetManager.items.get("Morphine"));
            CustomWeapons.Add(AssetManager.items.get("Oxycodone"));
            CustomWeapons.Add(AssetManager.items.get("Ritalin"));
        }

        public static void turnOnGuns() => GunsAllowed = true;
        public static void turnOffGuns() => GunsAllowed = false;

        public static void turnOnMIRVs() => MirvsAllowed = true;
        public static void turnOffMIRVs() => MirvsAllowed = false;

public static void toggleGuns()
        {
            Main.modifyBoolOption("GunOption", PowerButtons.GetToggleValue("gun_toggle"));
            if (PowerButtons.GetToggleValue("gun_toggle"))
            {
                turnOnGuns();
                return;
            }
            turnOffGuns();
        }
    

public static void toggleMIRVs()
        {
            Main.modifyBoolOption("MIRVOption", PowerButtons.GetToggleValue("mirv_toggle"));
            if (PowerButtons.GetToggleValue("mirv_toggle"))
            {
                turnOnMIRVs();
                return;
            }
            turnOffMIRVs();
        }
    }

    [HarmonyPatch(typeof(Culture), "getPreferredWeaponSubtypeIDs")]
    public class Patch_Culture_PreferredWeaponSubtypes
    {
        static bool Prefix(Culture __instance, ref string __result)
        {
            if (!CustomItemsList.GunsAllowed || CustomItemsList.CustomWeapons.Count == 0)
                return true;

            if (!CustomItemsList.MirvsAllowed && CustomItemsList.Kys.Contains(__result))
                return false;

            __result = "firearm";
            return false;
        }
    }

    [HarmonyPatch(typeof(Culture), "getPreferredWeaponAssets")]
    public class Patch_Culture_PreferredWeaponAssets
    {
        static bool Prefix(Culture __instance, ref List<EquipmentAsset> __result)
        {
            if (!CustomItemsList.GunsAllowed || CustomItemsList.CustomWeapons.Count == 0)
                return true;

            if (!CustomItemsList.MirvsAllowed)
            {
                __result = CustomItemsList.CustomWeapons
                    .Where(w => !CustomItemsList.Kys.Contains(w.id))
                    .ToList();
            }
            else
            {
                __result = CustomItemsList.CustomWeapons;
            }

            return false;
        }
    }

    [HarmonyPatch(typeof(Culture), "hasPreferredWeaponsToCraft")]
    public class Patch_Culture_HasPreferredWeaponsToCraft
    {
        static bool Prefix(Culture __instance, ref bool __result)
        {
            if (!CustomItemsList.GunsAllowed || CustomItemsList.CustomWeapons.Count == 0)
                return true;

            if (!CustomItemsList.MirvsAllowed)
            {
                bool hasAllowed = CustomItemsList.CustomWeapons
                    .Any(w => !CustomItemsList.Kys.Contains(w.id));

                __result = hasAllowed;
                return false;
            }

            __result = true;
            return false;
        }
    }

    public class WeaponsProjectilesEffects : MonoBehaviour
    {
        public static void init()
        {
            FixAllWeapons();
        }

        public static void FixAllWeapons()
        {
            ModernBoxLogger.Log("[FixAllWeapons] Starting weapon sprite fix pass...");

            int totalChecked = 0;
            int totalFixed = 0;
            int totalSkipped = 0;

            foreach (var kvp in AssetManager.items.list)
            {
                string id = kvp.id;
                EquipmentAsset asset = kvp;

                if (asset == null)
                {
                    totalSkipped++;
                    continue;
                }

                totalChecked++;

                if (asset.gameplay_sprites == null || asset.gameplay_sprites.Length == 0)
                {
                    var sprites = FetchSprites(id);
                    asset.gameplay_sprites = sprites;

                    if (sprites != null && sprites.Length > 0)
                        totalFixed++;
                }
                else
                {
                    totalSkipped++;
                }
            }

            ModernBoxLogger.Log($"[FixAllWeapons] Done. Checked: {totalChecked}, Fixed: {totalFixed}, Skipped: {totalSkipped}");
        }

        public static Sprite[] FetchSprites(string id)
        {
            EquipmentAsset item = AssetManager.items.get(id);
            if (item == null)
                return Array.Empty<Sprite>();

            if (item.animated)
            {
                List<Sprite> spriteList = new List<Sprite>();
                int frameIndex = 0;
                bool foundFrames = false;

                while (true)
                {
                    string[] paths = new[]
                    {
                        $"weapons/{id}_{frameIndex}",
                        $"weapons/{id}{frameIndex}",
                        $"weapons/{id}/main_0_{frameIndex}"
                    };

                    bool frameFound = false;
                    foreach (string path in paths)
                    {
                        Sprite sprite = Resources.Load<Sprite>(path);
                        if (sprite != null)
                        {
                            spriteList.Add(sprite);
                            foundFrames = true;
                            frameIndex++;
                            frameFound = true;
                            break;
                        }
                    }

                    if (!frameFound) break;
                    if (frameIndex > 20) break;
                }

                if (!foundFrames)
                {
                    var fallback = Resources.LoadAll<Sprite>("weapons/" + id);
                    if (fallback != null && fallback.Length > 0)
                        return fallback;
                    else
                        return Array.Empty<Sprite>();
                }

                return spriteList.ToArray();
            }
            else
            {
                var sprite = Resources.Load<Sprite>("weapons/" + id);
                return sprite != null ? new Sprite[] { sprite } : Array.Empty<Sprite>();
            }
        }
    }
}

