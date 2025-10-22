//========= MODERNBOX 2.1.0.1 ============//
//
// Made by Tuxxego
//
//=============================================================================//
using System;
using System.IO;
using System.Linq;
using NCMS;
using tools;
using NCMS.Utils;
using UnityEngine;
using ReflectionUtility;
using HarmonyLib;
using System.Reflection;
using Newtonsoft.Json;
using ModernBox;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Threading.Tasks;
using static Config;
using System.Reflection.Emit;
using UnityEngine.Tilemaps;
using UnityEngine.Purchasing.MiniJSON;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using UnityEngine.CrashReportHandler;
using System.IO.Compression;
using System.Threading;
using System.Text;
using Beebyte.Obfuscator;
using ai;
using ai.behaviours;
using life.taxi;
using SleekRender;
using tools.debug;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using WorldBoxConsole;
using UnityEngine.UI;
using static TopTileLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions.Must;
using Random=UnityEngine.Random;
using NeoModLoader.api;
using NeoModLoader.api.attributes;
using NeoModLoader.General;
// using TuxModLoader.Builders;

namespace ModernBox{
    [ModEntry]
    class Main : MonoBehaviour{
        #region
        public static Main instance;
        #endregion
        internal const string id = "Tuxxego.mods.worldbox.M3";
        internal static Harmony harmony;
        internal static Dictionary<string, UnityEngine.Object> modsResources;
        public BuildingLibrary buildingLibrary = new BuildingLibrary();
        public const string mainPath = "Mods/M3";
        public Buttonz Buttonz = new Buttonz();
        public KaijuUI KaijuUI = new KaijuUI();
        public PlayWavDirectly PlayWavDirectly = new PlayWavDirectly();
        public SpaceManager SpaceManager = new SpaceManager();
        public PlanetGenerator PlanetGenerator = new PlanetGenerator();
        public PlanetManager PlanetManager = new PlanetManager();
        public AchievementManager AchievementManager = new AchievementManager();
        public LocalizationManager LocalizationManager = new LocalizationManager();
        public BombUtilities testBombDebugger = new BombUtilities();
        public Bombs Bombs = new Bombs();
        public StatManager StatManager = new StatManager();
        public UnitTracker UnitTracker = new UnitTracker();
        public FirstTimeSetup FirstTimeSetup = new FirstTimeSetup();
        private static string correctSettingsVersion = "3.8.0.0"; 
        public const string settingsKey = "MBoxSettings"; 
        public static bool isNewVersion;
        public static SavedSettings savedSettings = new SavedSettings();
        public static PizzaManager PizzaManager = new PizzaManager();

        internal static string ModName
        {
            get { return Mod.Info.Name; }
        }

        void Awake()
        {
            try
            {
	            loadSettings();

                var worldBoxConsole = FindObjectOfType<WorldBoxConsole.Console>();
                if (worldBoxConsole != null)
                {
                    worldBoxConsole.gameObject.SetActive(false);
                    ModernBoxLogger.Log("WorldBoxConsole script has been disabled.");
                }
                else
                {
                    ModernBoxLogger.Warning("No GameObject with WorldBoxConsole script found in the scene.");
                }

                NATOisStillNeededEspeciallyNow();

                harmony = new Harmony("com.Tuxxego.M3");

                Assembly targetAssembly = Assembly.GetExecutingAssembly();
                harmony.PatchAll(targetAssembly);

                new TabBuilder()
				.SetTabID("ModernBox")
				.SetName("ModernBox")
				.SetDescription("Eras, Space, Politics, and more.")
				.SetPosition(200)
				.SetIcon("ui/icon")
				.Build();

            //    PowersTab tab = TabManager.CreateTab("ModernBox", "ModernBox", "Best Mod Ever", Resources.Load<Sprite>("ui/icon"));

            new TabBuilder()
            .SetTabID("Tab_kaiju")
            .SetName("ModernBox KaijuBox")
            .SetDescription("Spawn kaiju directly in your world!")
            .SetPosition(200)
            .SetIcon("ui/icons/tabIconkaiju")
            .Build();

            new TabBuilder()
            .SetTabID("????")
            .SetName("????")
            .SetDescription("????????????????????????????")
            .SetPosition(200)
            .SetIcon("ui/icons/wat")
            .Build();

                  ModernBoxLogger.Log("[M2] Space Manager starting...");
                SpaceManager = gameObject.AddComponent<SpaceManager>();
                ModernBoxLogger.Log("[M2] That big manager has started.");
        		AchievementManager = gameObject.AddComponent<AchievementManager>();
        		StatManager = gameObject.AddComponent<StatManager>();
        		UnitTracker = gameObject.AddComponent<UnitTracker>();
        		testBombDebugger = gameObject.AddComponent<BombUtilities>();
                gameObject.AddComponent<PizzaManager>();
                ModernBoxLogger.Log("SpaceBoxModernBox: Pls no lag!");

                ModernBoxLogger.Log("[M3] Initializing Bombs...");
                Bombs.Init();
                ModernBoxLogger.Log("[M3] Bombs loaded!");

                ModernBoxLogger.Log("[M3] Loading AboutWindow...");
                AboutWindow.init();
                ModernBoxLogger.Log("[M3] AboutWindow loaded!");

                ModernBoxLogger.Log("[M3] Loading CreditsWindow...");
                CreditsWindow.init();
                ModernBoxLogger.Log("[M3] CreditsWindow loaded!");

                ModernBoxLogger.Log("[M3] Loading SpaceWindow...");
                SpaceWindow.init();
                ModernBoxLogger.Log("[M3] SpaceWindow loaded!");
                ModernBoxLogger.Log("[M3] Loading CustomGalaxiesWindow...");
                CustomGalaxiesWindow.init();
                ModernBoxLogger.Log("[M3] CustomGalaxiesWindow loaded!");

                              ModernBoxLogger.Log("[M3] Loading AchievementsWindow...");
                 AchievementsWindow.init();
                 ModernBoxLogger.Log("[M3] AchievementsWindow loaded!");
                 
                ModernBoxLogger.Log("[M3] Initializing Buttonz...");
                Buttonz.Init();
                ModernBoxLogger.Log("[M3] Buttonz loaded!");

                ModernBoxLogger.Log("[M3] Buildings initialized");

			    Windows.ShowWindow("AboutWindow");

                GameObject setupGO = new GameObject("FirstTimeSetupGO");
                setupGO.AddComponent<FirstTimeSetup>();

            }
            catch (Exception ex)
            {
                ModernBoxLogger.Error($"[M3] Exception in Awake: {ex.Message}");
                ModernBoxLogger.Error($"[M3] Stack Trace: {ex.StackTrace}");
            }
        }


        void Start()
        {
            InitializeComponents();

        }

        void InitializeComponents()
        {
            try
            {
                modsResources = Reflection.GetField(typeof(ResourcesPatch), null, "modsResources") as Dictionary<string, UnityEngine.Object>;
                ModernBoxLogger.Log("Meow meow meow, FEED ME YOU MOTHERF");
                ModernBoxLogger.Log("No thanks Morfos.");

                Vehicles.init();

                Traits.init();
               Itemz.init();

               WeaponsProjectilesEffects.init();
               WeaponsProjectilesEffects.FixAllWeapons();


                Buildings.init();

                                Kaiju.init();
                                KaijuTraits.init();
                                KaijuUI.init();


                instance = this;

                PizzaWindow.init();

                PreloadHelpers.preloadBuildingSprites();
                ModernBoxLogger.Log("[M3] All components initialized successfully");

            }
            catch (Exception ex)
            {
                ModernBoxLogger.Error($"Exception in InitializeComponents: {ex.Message}");
                ModernBoxLogger.Error($"Stack Trace: {ex.StackTrace}");
            }
        }

       public static void StupidStuffOne()
        {
            StupidStuffTwo();
        }

        public static void StupidStuffTwo()
        {
            StupidStuffThree();
        }

        public static void StupidStuffThree()
        {
            StupidStuff();
        }

        public static void StupidStuff()
        {
            WeaponsProjectilesEffects.FixAllWeapons();
            Type Stupid = typeof(DynamicSprites).Assembly.GetType("HandRendererTexturePreloader");
            MethodInfo EvenMoreStupid = Stupid.GetMethod("preloadItemsIntoAtlas", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

            EvenMoreStupid.Invoke(null, null);
        }


        // from ancient warfare mod
        public static void NATOisStillNeededEspeciallyNow()
        {
            string path = $"{Mod.Info.Path}/EmbededResources/LoadingScreenM3";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                ExportResources.init_LoadingScreen(path);
                string filePath = Path.Combine(path, "ass.txt");
                File.WriteAllText(filePath, "");
            }

            string[] files1 = Directory.GetDirectories(path);
            if (files1.Length == 0)
            {
                ExportResources.init_LoadingScreen(path);
            }
            LoadingScreen transitionScreen = World.world.transition_screen;
            transitionScreen.enabled = false;
            Image backgroundImage = transitionScreen.background;
            backgroundImage.type = Image.Type.Simple;
            RectTransform backgroundRect = backgroundImage.GetComponent<RectTransform>();
            backgroundRect.anchoredPosition = new Vector2(0, 0);
            backgroundRect.sizeDelta = new Vector2(Screen.width, Screen.height);
            var imageExtensions = new[] { ".png", ".jpg", ".jpeg", ".bmp", ".gif" };
            var files = Directory
                .GetFiles(path, "*.*")
                .Where(s => imageExtensions.Any(s.EndsWith))
                .ToList();
            var randomGenerator = new System.Random();

            var selectedFile = Path.GetFileName(files[randomGenerator.Next(files.Count)]);

            backgroundImage.sprite = Toolbox.LoadSprite($"{path}/{selectedFile}");
            transitionScreen.enabled = true;
            StupidStuffOne();
        }

        public static void resetToDefaults()
        {
            SavedSettings defaultSettings = new SavedSettings(); 
            Windows.ShowWindow("DefaultSettingsWindow");
            foreach (var option in defaultSettings.boolOptions)
            {
                savedSettings.boolOptions[option.Key] = option.Value;
            }

            saveSettings();
        }

        public static void saveSettings(SavedSettings previousSettings = null)
        {
            if (previousSettings != null && savedSettings.Equals(previousSettings))
            {
                ModernBoxLogger.Log("ModernBox: No changes to settings, skipping saving!");
                return; 
            }

            ModernBoxLogger.Log("===============================");
            ModernBoxLogger.Log("ModernBox 3.8.0.0");
            ModernBoxLogger.Log("Changes were made, saving!");
            ModernBoxLogger.Log("===============================");

            foreach (var option in savedSettings.boolOptions)
            {
                PlayerPrefs.SetInt(option.Key, option.Value ? 1 : 0);
            }

            PlayerPrefs.SetString("SettingVersion", correctSettingsVersion);
            PlayerPrefs.Save();
        }
        public static bool loadSettings()
        {
            isNewVersion = false;

            if (!PlayerPrefs.HasKey("SettingVersion"))
            {
                isNewVersion = true;
                saveSettings();
                return false;
            }

            string loadedVersion = PlayerPrefs.GetString("SettingVersion");
            
            if (loadedVersion != correctSettingsVersion)
            {
                isNewVersion = true;
                saveSettings();
                return false;
            }

            var keys = savedSettings.boolOptions.Keys.ToList();

            foreach (var key in keys)
            {
                savedSettings.boolOptions[key] = PlayerPrefs.GetInt(key) == 1;
            }

            return true;
        }
        public static void modifyBoolOption(string key, bool value, UnityAction call = null)
        {
            if (savedSettings.boolOptions.TryGetValue(key, out bool oldValue) && oldValue == value)
            {
                return; 
            }

            Main.savedSettings.boolOptions[key] = value;

            saveSettings();

            if (call != null)
            {
                call.Invoke();
            }

        }
    }





      [HarmonyPatch(typeof(ItemLibrary), "loadSprites")]
    public static class LoadSpritesPatch
    {

        static void Postfix(object __instance)
        {
            ModernBoxLogger.Log("[loadSprites] Method completed");
            Main.StupidStuffOne();
        }
    }
    [HarmonyPatch]
    public static class Patch_DisableSortButtons
    {
        static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(PowersTab), "sortButtons");
        }

        static bool Prefix(object __instance)
        {
            FieldInfo assetField = AccessTools.Field(__instance.GetType(), "_asset");
            if (assetField == null) return true;

            var asset = assetField.GetValue(__instance) as PowerTabAsset;
            if (asset == null) return true;

            if (asset.id == "ModernBox")
            {
                return false;
            }

            return true;
                }
    }

}


