using System.Collections;
using HarmonyLib;
using NeoModLoader.api;
using NeoModLoader.api.attributes;
using NeoModLoader.General;
using UnityEngine;
using UnityEngine.UI;
using VideoCopilot.code.utils;

namespace PeerlessOverpoweringWarrior.code
{
    public class UnitWindowStatsIcon
    {
        /*-----------------------------------------------------------------------------------------------------------------------------------------------*/
        //在这写数值设置(显示时要给对应名字的icon赋值)
        //Write numerical settings here(When displayed, assign a value to the icon corresponding to the name)

        public static void OnEnable(UnitWindow window, Actor actor) {            
            // 添加UI元素存在性检查
            if (window.transform.Find("Background/Scroll View/Viewport/Content/content_more_icons") == null) {                
                return; // UI未初始化完成时跳过            
            }        
            
            window.setIconValue("Warrior", actor.GetWarrior());
            window.setIconValue("TrueGang", actor.GetTrueGang());
            window.setIconValue("Pattern", actor.GetPattern());
            window.showInfo();
        }

        /*-----------------------------------------------------------------------------------------------------------------------------------------------*/
        //在这写文本和图标,是否显示
        //Write text and icon, whether to display
        //最多5个(为了美观,做了5个的限制)
        //Maximum of 5 (limited to 5 for aesthetics)
        public static StatsIconData[] StatsIconDatas = {
            new ("Warrior", "Warrior.png",true),
            new ("TrueGang", "TrueGang.png",true),
            new ("Pattern", "Pattern.png",true), // 使用Warrior.png作为临时图标，后续可以替换为专用图标
        };
        /*重要提示---------本地化文本中,请使用"statsIcon_Name"来设置文本,例如"statsIcon_test"---------重要提示*/
        /*Important---------In localized text, use "statsIcon_Name" to set the text, e.g."statsIcon_test"------Important*/
        /*重要提示---------本地化文本中,请使用"statsIcon_Name"来设置文本,例如"statsIcon_test"---------重要提示*/
        /*Important---------In localized text, use "statsIcon_Name" to set the text, e.g."statsIcon_test"------Important*/


        /*-----------------------------------------------------------------------------------------------------------------------------------------------*/
        //写给新手(特别是十月白!)
        //For beginners
        //以下代码请勿乱动!!!!!!!!!!!!!!!!!!!!
        //Do not move the following code!!!!!!!!!!!!!!!!!!!!
        //以下代码请勿乱动!!!!!!!!!!!!!!!!!!!!
        //Do not move the following code!!!!!!!!!!!!!!!!!!!!
        //以下代码请勿乱动!!!!!!!!!!!!!!!!!!!!
        //Do not move the following code!!!!!!!!!!!!!!!!!!!!
        //以下代码请勿乱动!!!!!!!!!!!!!!!!!!!!
        //Do not move the following code!!!!!!!!!!!!!!!!!!!!
        //以下代码请勿乱动!!!!!!!!!!!!!!!!!!!!
        //Do not move the following code!!!!!!!!!!!!!!!!!!!!
        //以下代码请勿乱动!!!!!!!!!!!!!!!!!!!!
        //Do not move the following code!!!!!!!!!!!!!!!!!!!!
        /*-----------------------------------------------------------------------------------------------------------------------------------------------*/
        /*-----------------------------------------------------------------------------------------------------------------------------------------------*/
        /*-----------------------------------------------------------------------------------------------------------------------------------------------*/
        /*-----------------------------------------------------------------------------------------------------------------------------------------------*/
        /*-----------------------------------------------------------------------------------------------------------------------------------------------*/
        /*-----------------------------------------------------------------------------------------------------------------------------------------------*/
        /*-----------------------------------------------------------------------------------------------------------------------------------------------*/
        /*-----------------------------------------------------------------------------------------------------------------------------------------------*/
        /*-----------------------------------------------------------------------------------------------------------------------------------------------*/
        /*-----------------------------------------------------------------------------------------------------------------------------------------------*/
        /*-----------------------------------------------------------------------------------------------------------------------------------------------*/
        /*-----------------------------------------------------------------------------------------------------------------------------------------------*/
        /*-----------------------------------------------------------------------------------------------------------------------------------------------*/
        /*-----------------------------------------------------------------------------------------------------------------------------------------------*/
        /*-----------------------------------------------------------------------------------------------------------------------------------------------*/
        /*-----------------------------------------------------------------------------------------------------------------------------------------------*/
        /*-----------------------------------------------------------------------------------------------------------------------------------------------*/
        /*-----------------------------------------------------------------------------------------------------------------------------------------------*/
        /*-----------------------------------------------------------------------------------------------------------------------------------------------*/
        /*-----------------------------------------------------------------------------------------------------------------------------------------------*/

        public static StatsIcon icon;
        public static Transform content_transform;


        public static void Initialize(UnitWindow window)
        {
            // 1. 添加窗口对象空引用检查
            if (window == null || window.gameObject == null) 
            {
                Debug.LogError("UnitWindow or its GameObject is null!");
                return;
            }

            // 2. 添加滚动视图组件安全检查
            Transform scrollViewTransform = window.gameObject.transform.Find("Background/Scroll View");
            if (scrollViewTransform == null) 
            {
                Debug.LogError("Scroll View transform not found!");
                return;
            }
    
            ScrollRect scrollRect = scrollViewTransform.GetComponent<ScrollRect>();
            if (scrollRect != null) 
            {
                scrollRect.enabled = true;
            }

            // 3. 添加视口组件安全检查
            Transform viewportTransform = window.gameObject.transform.Find("Background/Scroll View/Viewport");
            if (viewportTransform == null) 
            {
                Debug.LogError("Viewport transform not found!");
                return;
            }
    
            Mask viewportMask = viewportTransform.GetComponent<Mask>();
            if (viewportMask != null) 
            {
                viewportMask.enabled = true;
            }
    
            Image viewportImage = viewportTransform.GetComponent<Image>();
            if (viewportImage != null) 
            {
                viewportImage.enabled = true;
            }

            // 4. 添加内容区域安全检查和空引用保护
            content_transform = window.gameObject.transform.Find(
                "Background/Scroll View/Viewport/Content/content_more_icons"
            );
    
            if (content_transform == null) 
            {
                Debug.LogError("content_more_icons transform not found!");
                return;
            }
    
            // 5. 添加组件获取安全检查
            var container = content_transform.gameObject.AddOrGetComponent<StatsIconContainer>();
            if (container == null) 
            {
                Debug.LogError("Failed to add StatsIconContainer component!");
                return;
            }

            // 6. 添加原始父对象安全检查
            if (content_transform.childCount < 5) 
            {
                Debug.LogError("content_more_icons doesn't have enough children!");
                return;
            }
    
            var originalParent = content_transform.GetChild(4);
            if (originalParent == null) 
            {
                Debug.LogError("Original parent transform not found!");
                return;
            }

            // 7. 添加实例化对象空引用检查
            var newStatsIconGroup = GameObject.Instantiate(originalParent);
            if (newStatsIconGroup == null) 
            {
                Debug.LogError("Failed to instantiate newStatsIconGroup!");
                return;
            }

            // 8. 安全清理子对象
            for (int i = newStatsIconGroup.transform.childCount - 1; i >= 0; i--)
            {
                Transform child = newStatsIconGroup.transform.GetChild(i);
                if (child != null && child.name != "i_kills") 
                {
                    GameObject.Destroy(child.gameObject);
                }
            }

            // 9. 添加关键对象查找安全检查
            var i_kills = newStatsIconGroup.Find("i_kills");
            if (i_kills == null) 
            {
                Debug.LogError("i_kills transform not found in newStatsIconGroup!");
                GameObject.Destroy(newStatsIconGroup); // 清理无效对象
                return;
            }

            int index = 0;
            foreach (var iconData in StatsIconDatas)
            {
                if (!iconData.is_show || index >= 5) 
                {
                    continue;
                }

                // 10. 添加实例化对象空引用检查
                var base_icon = GameObject.Instantiate(i_kills, originalParent);
                if (base_icon == null) 
                {
                    Debug.LogError($"Failed to instantiate icon for {iconData.name}");
                    continue;
                }

                // 11. 添加组件获取安全检查
                icon = base_icon.GetComponent<StatsIcon>();
                if (icon == null) 
                {
                    Debug.LogError($"StatsIcon component missing for {iconData.name}");
                    GameObject.Destroy(base_icon.gameObject);
                   continue;
                }

                var icon_text = base_icon.GetComponent<TipButton>();
                if (icon_text == null) 
                {
                    Debug.LogError($"TipButton component missing for {iconData.name}");
                    GameObject.Destroy(base_icon.gameObject);
                    continue;
                }

                // 12. 添加资源加载空引用检查
                string localizedText = LM.Get("statsIcon_" + iconData.name);
                if (!string.IsNullOrEmpty(localizedText)) 
                {
                    icon_text.textOnClick = localizedText;
                }
        
                icon.name = iconData.name;
        
                Sprite iconSprite = Resources.Load<Sprite>(iconData.iconPath);
                if (iconSprite != null) 
                {
                    icon.getIcon().sprite = iconSprite;
                }
                else 
                {
                    Debug.LogError($"Failed to load sprite: {iconData.iconPath}");
                }
        
                index++;
            }

            // 13. 安全销毁关键对象
            if (i_kills != null) 
            {
                GameObject.DestroyImmediate(i_kills.gameObject);
            }

            newStatsIconGroup.name = "test_new_icon";
    
            // 14. 添加父对象设置安全检查
            if (content_transform != null) 
            {
                newStatsIconGroup.SetParent(content_transform);
                newStatsIconGroup.transform.localScale = Vector3.one;
            }
            else 
            {
                Debug.LogError("Content transform is null, can't set parent!");
                GameObject.Destroy(newStatsIconGroup);
            }
        }
    }
    public class StatsIconData
    {
        public string name;
        public string iconPath;
        public bool is_show = false;

        public StatsIconData(string name, string iconPath, bool is_show = false)
        {
            this.name = name;
            this.iconPath = iconPath;
            this.is_show = is_show;
        }
    }
}
