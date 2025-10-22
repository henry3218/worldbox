using HarmonyLib;
using PeerlessOverpoweringWarrior.code;
using NeoModLoader.api;
using System.IO;
using PeerlessOverpoweringWarrior.code;

namespace PeerlessOverpoweringWarrior
{
    internal class PeerlessOverpoweringWarriorClass : BasicMod<PeerlessOverpoweringWarriorClass>
    {
        public static string id = "gemen.worldbox.mod.PeerlessOverpoweringWarrior";
        
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
                WarriorUIManager.Init();
                UnityEngine.Debug.Log("UI initialization completed.");
                
                // 初始化阵道法术效果
                UnityEngine.Debug.Log("Starting FormationSpellEffect initialization...");
                PeerlessOverpoweringWarrior.code.FormationSpellEffect.Init();
                UnityEngine.Debug.Log("FormationSpellEffect initialization completed.");
                
                // 注意：阵道法术系统不需要单独初始化，直接在ArmyManager_update_Postfix中被调用

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