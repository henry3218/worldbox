using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

namespace PeerlessOverpoweringWarrior.code
{
    public static class NotificationHelper
    {
        // 境界名称映射表：特质ID -> 中文境界名称
        private static readonly Dictionary<string, string> RealmNameMap = new Dictionary<string, string>
        {
            // 武道境界
            { "Warrior1", "锻体境" },
            { "Warrior2", "炼骨境" },
            { "Warrior3", "通脉境" },
            { "Warrior4", "气海境" },
            { "Warrior5", "化劲境" },
            { "Warrior6", "凝罡境" },
            { "Warrior7", "洞虚境" },
            { "Warrior8", "劫身境" },
            { "Warrior9", "武域境" },
            { "Warrior91", "合道境" },
            { "Warrior92", "斩我境" },
            { "Warrior93", "武极境" },
            { "FormationRealm1", "引纹境" },
            { "FormationRealm2", "灵纹境" },
            { "FormationRealm3", "通地境" },
            { "FormationRealm4", "蕴枢境" },
            { "FormationRealm5", "规元境" },
            { "FormationRealm6", "玄真境" },
        };

        // 荒古武躯名称映射表
        private static readonly Dictionary<string, string> MartialBodyNameMap = new Dictionary<string, string>
        {
            {"ancientMartialBody1", "八荒武神躯"},
            {"ancientMartialBody2", "不灭星辰体"},
            {"ancientMartialBody3", "混沌武魔体"},
            {"ancientMartialBody4", "九狱蛮龙体"},
            {"ancientMartialBody5", "陨星霸烈体"},
            {"ancientMartialBody6", "万劫战佛身"},
            {"ancientMartialBody7", "天罡斗战体"},
            {"ancientMartialBody8", "血海修罗体"},
            {"ancientMartialBody9", "太初武源体"},
            {"ancientMartialBody91", "大日焚宇体"},
            {"ancientMartialBody92", "太阴转轮身"},
            {"ancientMartialBody93", "万古青天身"},
            {"ancientMartialBody94", "劫灭雷尊体"},
            {"ancientMartialBody95", "虚空鲸吞体"},
            {"ancientMartialBody96", "诸劫归墟体"},
            {"ancientMartialBody97", "万壑熔炉身"},
            {"ancientMartialBody98", "亘古磐岳躯"},
            {"ancientMartialBody99", "九渊噬道体"}
        };

        // 洞天名称映射表
        private static readonly Dictionary<string, string> GrottoNameMap = new Dictionary<string, string>
        {
            {"CelestialGrotto1", "罡煞破虚真武洞天"},
            {"CelestialGrotto2", "龙虎交泰阴阳洞天"},
            {"CelestialGrotto3", "玄霄天门玉京洞天"},
            {"CelestialGrotto4", "八极镇岳平海洞天"},
            {"CelestialGrotto5", "紫微斗转北辰洞天"},
            {"CelestialGrotto6", "九转还丹赤明洞天"},
            {"CelestialGrotto7", "玄溟寒渊龙象洞天"},
            {"CelestialGrotto8", "朱雀离明焚焰洞天"},
            {"CelestialGrotto9", "归墟溟渤潮生洞天"},
            {"CelestialGrotto10", "五炁炁朝元玄都洞天"},
            {"CelestialGrotto11", "岐黄内景炼魔洞天"},
            {"CelestialGrotto12", "涅槃无住寂照洞天"},
            {"CelestialGrotto13", "四象镇阙星寰洞天"},
            {"CelestialGrotto14", "太虚青冥扶摇洞天"},
            {"CelestialGrotto15", "风雷无相逍遥洞天"},
            {"CelestialGrotto16", "五岳真形镇煞洞天"},
            {"CelestialGrotto17", "大壑雷渊劫烬洞天"},
            {"CelestialGrotto18", "般若禅武明心洞天"}
        };

        // 灵植名称映射表
        private static readonly Dictionary<string, string> SpiritualPlantNameMap = new Dictionary<string, string>
        {
            {"SpiritualPlant1+", "赤鳞草"},
            {"SpiritualPlant2", "铁骨参"},
            {"SpiritualPlant3", "活血藤"},
            {"SpiritualPlant4", "地脉黄精"},
            {"SpiritualPlant5", "百炼果"},
            {"SpiritualPlant6", "养脉花"},
            {"SpiritualPlant7", "石乳菌"},
            {"SpiritualPlant8", "狼血棘"},
            {"SpiritualPlant9", "白玉穗"},
            {"SpiritualPlant1", "夺天丹"},
            {"SpiritualPlant91", "血龙圣花"},
            {"SpiritualPlant92", "九转金参"},
            {"SpiritualPlant93", "幽冥噬魂藤"},
            {"SpiritualPlant94", "千劫雷音竹"},
            {"SpiritualPlant95", "涅槃火莲"},
            {"SpiritualPlant96", "玄冰玉髓芝"},
            {"SpiritualPlant97", "万兽血菩提"},
            {"SpiritualPlant98", "星辰锻骨草"},
            {"SpiritualPlant99", "阴阳混沌果"},
            // 真罡灵植名称映射
            {"TrueGangPlant1", "玄罡草"},
            {"TrueGangPlant2", "天罡叶"},
            {"TrueGangPlant3", "真罡花"},
            {"TrueGangPlant4", "地罡根"},
            {"TrueGangPlant5", "雷罡笋"},
            {"TrueGangPlant6", "九霄罡果"},
            {"TrueGangPlant7", "混沌罡莲"},
            {"TrueGangPlant8", "太初罡晶"},
            {"TrueGangPlant9", "武祖罡灵"}
        };

        // 橘黄色颜色定义
        private static readonly Color GoldColor = new Color(1f, 0.647f, 0f, 0.6f);
        
        // 新增：通知队列和状态控制
        private static Queue<NotificationData> notificationQueue = new Queue<NotificationData>();
        private static bool isProcessingQueue = false;
        private static GameObject coroutineRunnerObject = null;
        
        // 通知数据结构
        private class NotificationData
        {
            public string message;
            public string objectName;
            
            public NotificationData(string msg, string objName)
            {
                message = msg;
                objectName = objName;
            }
        }

        public static void ShowBreakthroughNotification(Actor actor, string oldTrait, string newTrait)
        {
            // 检查是否需要显示通知
            if (!ShouldShowNotification(newTrait))
            {
                return; // 根据配置跳过通知
            }
            
            string oldRealmName = GetRealmName(oldTrait);
            string newRealmName = GetRealmName(newTrait);
            
            string message = $"{actor.getName()} 已从 {oldRealmName} 突破到 {newRealmName}！";
            Debug.Log(message);
            
            // 将通知加入队列而不是立即显示
            EnqueueNotification(message, "BreakthroughNotification");
        }
        
        // 根据配置决定是否显示通知
        private static bool ShouldShowNotification(string newTrait)
        {
            // 检查是否是特定的高级境界突破
            if (newTrait == "Warrior91" || newTrait == "Warrior91+") // 合道境
            {
                return PeerlessOverpoweringWarrior.code.Config.WarriorConfig.NotifyHarmonyBreakthrough;
            }
            else if (newTrait == "Warrior92" || newTrait == "Warrior92+") // 斩我境
            {
                return PeerlessOverpoweringWarrior.code.Config.WarriorConfig.NotifyZhanWoBreakthrough;
            }
            else if (newTrait == "Warrior93" || newTrait == "Warrior93+") // 武极境
            {
                return PeerlessOverpoweringWarrior.code.Config.WarriorConfig.NotifyWuJiBreakthrough;
            }
            else if (newTrait == "FormationRealm6") // 玄真境
            {
                return PeerlessOverpoweringWarrior.code.Config.WarriorConfig.NotifyXuanZhenBreakthrough;
            }
            
            // 其他境界突破默认显示通知
            return true;
        }

        public static void ShowMartialBodyNotification(Actor actor, string traitId)
        {
            string bodyName = GetMartialBodyName(traitId);
            string message = $"{actor.getName()} 觉醒了 {bodyName}！";
            Debug.Log(message);
            
            // 将通知加入队列
            EnqueueNotification(message, "MartialBodyNotification");
        }

        public static void ShowGrottoNotification(Actor actor, string traitId)
        {
            string grottoName = GetGrottoName(traitId);
            string message = $"{actor.getName()} 焚烧洞天，极尽升华！";
            Debug.Log(message);
            
            // 将通知加入队列
            EnqueueNotification(message, "GrottoNotification");
        }
        
        public static void ShowFormationRealm6Notification(Actor actor)
        {
            // 检查是否需要显示通知
            if (!PeerlessOverpoweringWarrior.code.Config.WarriorConfig.NotifyXuanZhenBreakthrough)
            {
                return; // 根据配置跳过通知
            }
            
            string message = $"{actor.getName()} 阵道大成，成为当世天师！";
            Debug.Log(message);
            
            // 将通知加入队列
            EnqueueNotification(message, "FormationRealm6Notification");
        }

        // 前六个灵植宝药的特质ID列表
        private static readonly HashSet<string> NoNotificationPlants = new HashSet<string>
        {
            "SpiritualPlant1+", "SpiritualPlant2", "SpiritualPlant3",
            "SpiritualPlant4", "SpiritualPlant5", "SpiritualPlant6"
        };

        // 新增：显示灵植使用提示
        public static void ShowSpiritualPlantNotification(Actor actor, string plantTraitId)
        {
            string plantName = GetSpiritualPlantName(plantTraitId);
            string message = $"{actor.getName()} 服用了 {plantName}，气血增长！";
            Debug.Log(message);
            // 检查是否属于不播报的灵植
            if (NoNotificationPlants.Contains(plantTraitId))
            {
                return; // 前六种灵植不显示通知
            }
            // 将通知加入队列
            EnqueueNotification(message, "SpiritualPlantNotification");
        }

        // 新增：显示真罡灵植使用提示
        public static void ShowTrueGangPlantNotification(Actor actor, string plantTraitId)
        {
            string plantName = GetSpiritualPlantName(plantTraitId);
            string message = $"{actor.getName()} 服用了 {plantName}，真罡更加凝练！";
            Debug.Log(message);
            // 检查是否属于不播报的灵植
            if (plantTraitId.Contains("TrueGangPlant"))
            {
                // 将通知加入队列
                EnqueueNotification(message, "TrueGangPlantNotification");
            }
        }
        public static void ShowFormationSkill5ResurrectionNotification(Actor actor)
        {
            string message = $"{actor.getName()}：阵纹不灭，吾身不死！万劫轮回，因果灭度！";
            Debug.Log(message);
            
            // 将通知加入队列
            EnqueueNotification(message, "FormationSkill5ResurrectionNotification");
        }

        // 新增：将通知加入队列
        private static void EnqueueNotification(string message, string objectName)
        {
            notificationQueue.Enqueue(new NotificationData(message, objectName));
            
            // 如果当前没有处理队列，则开始处理
            if (!isProcessingQueue)
            {
                // 确保只创建一个协程运行器对象
                if (coroutineRunnerObject == null)
                {
                    coroutineRunnerObject = new GameObject("NotificationCoroutineRunner");
                    // 确保对象不会在场景切换时被销毁
                    GameObject.DontDestroyOnLoad(coroutineRunnerObject);
                }
                
                // 添加协程运行器组件并启动协程
                NotificationCoroutineRunner runner = coroutineRunnerObject.GetComponent<NotificationCoroutineRunner>();
                if (runner == null)
                {
                    runner = coroutineRunnerObject.AddComponent<NotificationCoroutineRunner>();
                }
                
                // 确保协程正在运行
                if (!runner.isRunning)
                {
                    runner.StartCoroutine(ProcessNotificationQueue());
                }
            }
        }
        
        // 新增：处理通知队列的方法
        public static IEnumerator ProcessNotificationQueue()
        {
            isProcessingQueue = true;
            
            while (notificationQueue.Count > 0)
            {
                NotificationData notification = notificationQueue.Dequeue();
                
                // 创建提示UI
                GameObject notificationObj = CreateNotificationUI(notification.message, notification.objectName);
                BreakthroughNotification component = notificationObj.GetComponent<BreakthroughNotification>();
                
                // 设置回调，当通知显示完成后继续处理下一个
                component.onNotificationComplete = null; // 重置回调
                bool notificationComplete = false;
                component.onNotificationComplete = () => notificationComplete = true;
                
                // 等待通知显示完成
                while (!notificationComplete)
                {
                    yield return null;
                }
                
                // 可选：在通知之间添加短暂的间隔
                yield return new WaitForSeconds(0.5f);
            }
            
            isProcessingQueue = false;
            
            // 清理协程运行器对象
            if (coroutineRunnerObject != null)
            {
                // 移除组件但保留对象，以便下次使用
                NotificationCoroutineRunner runner = coroutineRunnerObject.GetComponent<NotificationCoroutineRunner>();
                if (runner != null)
                {
                    runner.isRunning = false;
                }
            }
        }

        private static GameObject CreateNotificationUI(string message, string objectName)
        {
            // 创建提示UI
            GameObject notificationObj = new GameObject(objectName);
            notificationObj.transform.SetParent(GetCanvas().transform, false);
            
            // 设置全局缩放
            CanvasScaler scaler = notificationObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;
            
            // 文本组件
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(notificationObj.transform, false);
            Text text = textObj.AddComponent<Text>();
            
            // 尝试加载Arial字体，如果失败则使用备用方案
            Font font = TryLoadFont("Arial.ttf") ?? TryLoadFont("Arial") ?? CreateDefaultFont();
            
            text.font = font;
            text.text = message;
            text.alignment = TextAnchor.MiddleCenter;
            text.fontSize = 36;
            text.color = GoldColor;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Truncate;
            text.resizeTextForBestFit = true;
            text.resizeTextMaxSize = 45;
            text.resizeTextMinSize = 24;
            
            // 添加外发光效果
            Shadow shadow = textObj.AddComponent<Shadow>();
            shadow.effectColor = new Color(0f, 0f, 0f, 0.4f);
            shadow.effectDistance = new Vector2(2f, -2f);
            
            Outline outline = textObj.AddComponent<Outline>();
            outline.effectColor = new Color(0f, 0f, 0f, 0.5f);
            outline.effectDistance = new Vector2(1.5f, 1.5f);
            
            RectTransform textRect = textObj.GetComponent<RectTransform>();
            textRect.anchorMin = new Vector2(0.5f, 0.85f);
            textRect.anchorMax = new Vector2(0.5f, 0.85f);
            textRect.pivot = new Vector2(0.5f, 0.5f);
            textRect.sizeDelta = new Vector2(1800, 60);
            textRect.anchoredPosition = Vector2.zero;
            
            // 确保文本组件正确设置为居中对齐
            text.alignment = TextAnchor.MiddleCenter;
            
            // 添加淡入淡出效果
            BreakthroughNotification component = notificationObj.AddComponent<BreakthroughNotification>();
            component.Init(text);
            
            return notificationObj;
        }

        private static string GetMartialBodyName(string traitId)
        {
            if (MartialBodyNameMap.TryGetValue(traitId, out string bodyName))
            {
                return bodyName;
            }
            return traitId;
        }

        private static string GetGrottoName(string traitId)
        {
            if (GrottoNameMap.TryGetValue(traitId, out string grottoName))
            {
                return grottoName;
            }
            return "洞天";
        }

        // 新增：获取灵植名称
        private static string GetSpiritualPlantName(string traitId)
        {
            if (SpiritualPlantNameMap.TryGetValue(traitId, out string plantName))
            {
                return plantName;
            }
            return "灵植";
        }

        private static Font TryLoadFont(string fontName)
        {
            try
            {
                Font font = Resources.GetBuiltinResource<Font>(fontName);
                if (font != null)
                {
                    return font;
                }
                // 移除日志输出，避免影响观感
            }
            catch (System.Exception)
            {
                // 移除错误日志输出
            }
            return null;
        }
        
        private static Font CreateDefaultFont()
        {
            // 移除警告日志，避免影响观感
            return new Font("Arial");
        }
        
        private static string GetRealmName(string traitId)
        {
            if (RealmNameMap.TryGetValue(traitId, out string realmName))
            {
                return realmName;
            }
            return traitId;
        }
        
        private static Canvas notificationCanvas = null;
        
        private static Canvas GetCanvas()
        {
            // 确保始终使用我们自己创建的独立Canvas，而不是游戏中现有的Canvas
            if (notificationCanvas == null || notificationCanvas.gameObject == null)
            {
                GameObject canvasObj = new GameObject("NotificationCanvas");
                // 确保Canvas对象不会在场景切换时被销毁
                GameObject.DontDestroyOnLoad(canvasObj);
                
                notificationCanvas = canvasObj.AddComponent<Canvas>();
                notificationCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                notificationCanvas.sortingOrder = 1000;
                
                CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(1920, 1080);
                scaler.matchWidthOrHeight = 0.5f;
                
                canvasObj.AddComponent<GraphicRaycaster>();
            }
            return notificationCanvas;
        }
    }
    
    // 新增：协程运行器类
    public class NotificationCoroutineRunner : MonoBehaviour
    {
        public bool isRunning = false;
        
        public Coroutine StartCoroutine(IEnumerator routine)
        {
            isRunning = true;
            return base.StartCoroutine(routine);
        }
    }
    
    public class BreakthroughNotification : MonoBehaviour
    {
        public Text textComponent;
        public float showDuration = 3f;
        public float fadeDuration = 0.5f;
        public System.Action onNotificationComplete;
        
        public void Init(Text text)
        {
            textComponent = text;
            
            // 初始透明度为0
            textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, 0);
            
            // 强制更新布局
            Canvas.ForceUpdateCanvases();
            
            // 启动淡入动画
            StartCoroutine(FadeIn());
        }
        
        IEnumerator FadeIn()
        {
            float elapsed = 0;
            while (elapsed < fadeDuration)
            {
                float alpha = Mathf.Lerp(0, 1, elapsed / fadeDuration);
                textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, alpha);
                
                // 同时淡入阴影和描边
                foreach (var effect in textComponent.GetComponents<BaseMeshEffect>())
                {
                    if (effect is Shadow shadow)
                    {
                        shadow.effectColor = new Color(shadow.effectColor.r, shadow.effectColor.g, shadow.effectColor.b, alpha * 0.7f);
                    }
                    else if (effect is Outline outline)
                    {
                        outline.effectColor = new Color(outline.effectColor.r, outline.effectColor.g, outline.effectColor.b, alpha * 0.8f);
                    }
                }
                
                elapsed += Time.deltaTime;
                yield return null;
            }
            
            // 保持显示
            yield return new WaitForSeconds(showDuration);
            
            // 启动淡出动画
            StartCoroutine(FadeOut());
        }
        
        IEnumerator FadeOut()
        {
            float elapsed = 0;
            while (elapsed < fadeDuration)
            {
                float alpha = Mathf.Lerp(1, 0, elapsed / fadeDuration);
                textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, alpha);
                
                // 同时淡出阴影和描边
                foreach (var effect in textComponent.GetComponents<BaseMeshEffect>())
                {
                    if (effect is Shadow shadow)
                    {
                        shadow.effectColor = new Color(shadow.effectColor.r, shadow.effectColor.g, shadow.effectColor.b, alpha * 0.7f);
                    }
                    else if (effect is Outline outline)
                    {
                        outline.effectColor = new Color(outline.effectColor.r, outline.effectColor.g, outline.effectColor.b, alpha * 0.8f);
                    }
                }
                
                elapsed += Time.deltaTime;
                yield return null;
            }
            
            // 触发完成回调
            if (onNotificationComplete != null)
            {
                onNotificationComplete();
            }
            
            // 销毁对象
            Destroy(gameObject);
        }
    }
}