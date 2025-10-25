using HarmonyLib;
using Chevalier.code;
using NeoModLoader.api;

namespace Chevalier
{
    internal class ChevalierClass : BasicMod<ChevalierClass>
    {
        public static string id = "gemen.worldbox.mod.Chevalier";
        
        // 静态配置引用
        public static object ModCfg { get { return I?.GetConfig(); } }
        protected override void OnModLoad()
        {
            try
            {
                UnityEngine.Debug.Log("Starting stats initialization...");
                stats.Init();
                UnityEngine.Debug.Log("Stats initialization completed.");

                UnityEngine.Debug.Log("Starting traitGroup initialization...");
                traitGroup.Init();
                UnityEngine.Debug.Log("traitGroup initialization completed.");

                UnityEngine.Debug.Log("Starting traits initialization...");
                traits.Init();
                UnityEngine.Debug.Log("traits initialization completed.");

                // 初始化武器系统
                UnityEngine.Debug.Log("Starting weapons initialization...");
                CustomItems.Init();
                UnityEngine.Debug.Log("weapons initialization completed.");

                // 初始化UI管理器
                UnityEngine.Debug.Log("Starting UI initialization...");
                ChevalierUIManager.Init();
                UnityEngine.Debug.Log("UI initialization completed.");

                UnityEngine.Debug.Log("Applying Harmony patches...");
                new Harmony(id).PatchAll(typeof(patch));
                UnityEngine.Debug.Log("Harmony patches applied successfully.");
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogError($"Error during mod loading: {ex.Message}\n{ex.StackTrace}");
            }
        }
    }
}
