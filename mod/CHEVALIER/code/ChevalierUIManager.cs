using UnityEngine;
using UnityEngine.UI;
using NeoModLoader.General;
using NeoModLoader.General.UI.Tab;
using NeoModLoader.General.UI.Window;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace Chevalier.code
{
    // 主UI控制类
    public static class ChevalierUIManager
    {
        private static bool _inited = false;
        private static PowersTab _tab;

        // 初始化UI系统
        public static void Init()
        {
            if (_inited) return;
            _inited = true;

            UnityEngine.Debug.Log("[圣骑] Step1: UI initialization begin");

            // 创建标签页和按钮
            var tabIcon = TrySprite("ui/icons/iconTab1", "ui/icons/iconBook");
            _tab = TabManager.CreateTab("chevalier_mod_tab", "圣骑传说", "更快捷的模组设置入口", tabIcon);
            _tab.SetLayout(new List<string> { "tools" });

            // 添加设置按钮
            var settingsIcon = TrySprite("ui/icons/iconSetting2", "ui/icons/iconBook");
            var settingsBtn = PowerButtonCreator.CreateSimpleButton("模组设置", OpenNativeModSettings, settingsIcon);
            _tab.AddPowerButton("tools", settingsBtn);

            _tab.UpdateLayout();

            UnityEngine.Debug.Log("[圣骑] Step2: UI initialization done");
        }

        // 尝试加载精灵图
        private static Sprite TrySprite(params string[] keys)
        {
            foreach (var k in keys)
            {
                if (string.IsNullOrEmpty(k)) continue;
                var s = SpriteTextureLoader.getSprite(k);
                if (s != null) return s;
            }
            return null;
        }

        // 打开原生模组设置窗口
        private static void OpenNativeModSettings()
        {
            try
            {
                var cfg = TryGetMyModConfig();
                if (cfg == null)
                {
                    UnityEngine.Debug.LogError("[圣骑] 未找到 ModConfig（检查 default_config.json / BasicMod<T>.GetConfig() 暴露情况）");
                    return;
                }

                var wndType = FindTypeBySimpleNames(new[] {
                    "NeoModLoader.ui.ModConfigureWindow",
                    "NeoModLoader.General.ModConfigureWindow",
                    "NeoModLoader.General.UI.ModConfigureWindow",
                    "ModConfigureWindow"
                });
                if (wndType == null) { UnityEngine.Debug.LogError("[圣骑] 未找到 ModConfigureWindow 类型"); return; }

                var show = wndType.GetMethod("ShowWindow", BindingFlags.Public | BindingFlags.Static);
                if (show == null) { UnityEngine.Debug.LogError("[圣骑] 未找到 ModConfigureWindow.ShowWindow(...)"); return; }

                show.Invoke(null, new object[] { cfg });
            }
            catch (Exception ex) { UnityEngine.Debug.LogError($"[圣骑] 打开原生设置窗口失败: {ex}"); }
        }

        // 获取模组配置对象
        private static object TryGetMyModConfig()
        {
            var modType = Type.GetType("Chevalier.ChevalierClass")
                       ?? AppDomain.CurrentDomain.GetAssemblies()
                            .SelectMany(a => { try { return a.GetTypes(); } catch { return Array.Empty<Type>(); } })
                            .FirstOrDefault(t => t.Name == "ChevalierClass");
            if (modType == null) return null;

            // 尝试获取静态ModCfg属性
            var pModCfg = modType.GetProperty("ModCfg", BindingFlags.Public | BindingFlags.Static);
            if (pModCfg != null) { var v = pModCfg.GetValue(null, null); if (v != null) return v; }

            // 尝试获取实例
            object inst = null;
            var pI = modType.GetProperty("I", BindingFlags.Public | BindingFlags.Static);
            if (pI != null) inst = pI.GetValue(null, null);
            if (inst == null)
            {
                var fI = modType.GetField("I", BindingFlags.Public | BindingFlags.Static);
                if (fI != null) inst = fI.GetValue(null);
            }
            
            // 如果获取到实例，尝试从实例获取配置
            if (inst != null)
            {
                var mGetCfg = inst.GetType().GetMethod("GetConfig", BindingFlags.Public | BindingFlags.Instance);
                if (mGetCfg != null) { var v = mGetCfg.Invoke(inst, null); if (v != null) return v; }

                var fCfg2 = inst.GetType().GetField("config", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                         ?? inst.GetType().GetField("Config", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (fCfg2 != null) { var v = fCfg2.GetValue(inst); if (v != null) return v; }

                var pCfg2 = inst.GetType().GetProperty("config", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                         ?? inst.GetType().GetProperty("Config", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (pCfg2 != null) { var v = pCfg2.GetValue(inst, null); if (v != null) return v; }
            }

            // 尝试直接获取静态配置字段/属性
            var fCfg = modType.GetField("config", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                   ?? modType.GetField("Config", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            if (fCfg != null) { var v = fCfg.GetValue(null); if (v != null) return v; }
            
            var pCfg = modType.GetProperty("config", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                  ?? modType.GetProperty("Config", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            if (pCfg != null) { var v = pCfg.GetValue(null, null); if (v != null) return v; }

            return null;
        }

        // 查找类型
        private static Type FindTypeBySimpleNames(IEnumerable<string> candidates)
        {
            foreach (var name in candidates)
            {
                var t = Type.GetType(name);
                if (t != null) return t;

                t = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => { try { return a.GetTypes(); } catch { return Array.Empty<Type>(); } })
                    .FirstOrDefault(tt => tt.FullName == name || tt.Name == name);
                if (t != null) return t;
            }
            return null;
        }

        // 通过名称查找类型
        private static Type FindTypeByNames(string[] names)
        {
            foreach (string name in names)
            {
                Type type = Type.GetType(name);
                if (type != null) return type;

                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    try
                    {
                        foreach (Type t in assembly.GetTypes())
                        {
                            if (t.Name == name || t.FullName == name)
                            {
                                return t;
                            }
                        }
                    }
                    catch { }
                }
            }
            return null;
        }
    }
}