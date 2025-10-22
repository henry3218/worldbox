using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

namespace Chevalier.code
{
    public static class NotificationHelper
    {
        // 境界名称映射表：特质ID -> 中文境界名称
        private static readonly Dictionary<string, string> RealmNameMap = new Dictionary<string, string>
        {
            { "Chevalier1", "骑士侍从" },
            { "Chevalier2", "准骑士" },
            { "Chevalier3", "正式骑士" },
            { "Chevalier4", "守护骑士" },
            { "Chevalier5", "方旗骑士" },
            { "Chevalier6", "大骑士" },
            { "Chevalier7", "称号大骑士" },
            { "Chevalier8", "传奇大骑士" },
            { "Chevalier9", "神话大骑士" },
            { "Chevalier91", "烈日骑士" },
            { "Chevalier92", "永耀骑士" },
            { "Chevalier93", "不朽骑士" },
        };

        // 荒古武躯名称映射表
        private static readonly Dictionary<string, string> MartialBodyNameMap = new Dictionary<string, string>
        {
            {"GodlySigil1", "【时序之印】"},
            {"GodlySigil2", "【生命母印】"},
            {"GodlySigil3", "【终末之印】"},
            {"GodlySigil4", "【战旌之印】"},
            {"GodlySigil5", "【智识之印】"},
            {"GodlySigil6", "【歌律之印】"},
            {"GodlySigil7", "【锻炉之印】"},
            {"GodlySigil8", "【旅者之印】"},
            {"GodlySigil9", "【丰穰之印】"},
            {"GodlySigil91", "【医愈之印】"},
            {"GodlySigil92", "【浪涛之印】"},
            {"GodlySigil93", "【狩夜之印】"},
            {"GodlySigil94", "【契约之印】"},
            {"GodlySigil95", "【梦魇之印】"},
            {"GodlySigil96", "【烈阳之印】"},
            {"GodlySigil97", "【霜痕之印】"},
            {"GodlySigil98", "【谎言之印】"},
            {"GodlySigil99", "【寂默之印】"}
        };

        // 洞天名称映射表
        private static readonly Dictionary<string, string> GodKingdomNameMap = new Dictionary<string, string>
        {
            {"GodKingdom1", "罡煞破虚真武洞天"},
            {"GodKingdom2", "龙虎交泰阴阳洞天"},
            {"GodKingdom3", "玄霄天门玉京洞天"},
            {"GodKingdom4", "八极镇岳平海洞天"},
            {"GodKingdom5", "紫微斗转北辰洞天"},
            {"GodKingdom6", "九转还丹赤明洞天"},
            {"GodKingdom7", "玄溟寒渊龙象洞天"},
            {"GodKingdom8", "朱雀离明焚焰洞天"},
            {"GodKingdom9", "归墟溟渤潮生洞天"},
            {"GodKingdom10", "五炁炁朝元玄都洞天"},
            {"GodKingdom11", "岐黄内景炼魔洞天"},
            {"GodKingdom12", "涅槃无住寂照洞天"},
            {"GodKingdom13", "四象镇阙星寰洞天"},
            {"GodKingdom14", "太虚青冥扶摇洞天"},
            {"GodKingdom15", "风雷无相逍遥洞天"},
            {"GodKingdom16", "五岳真形镇煞洞天"},
            {"GodKingdom17", "大壑雷渊劫烬洞天"},
            {"GodKingdom18", "般若禅武明心洞天"}
        };

        // 灵植名称映射表
        private static readonly Dictionary<string, string> MysteriousConcoctionNameMap = new Dictionary<string, string>
        {
            {"MysteriousConcoction1+", "【角斗士亡魂酊】"},
            {"MysteriousConcoction2", "【雷击木果实】"},
            {"MysteriousConcoction3", "【狮心蔷薇】"},
            {"MysteriousConcoction4", "【蝎尾狮奶蜜】"},
            {"MysteriousConcoction5", "【巨人铁藓】"},
            {"MysteriousConcoction6", "【深渊蠕虫胆膏】"},
            {"MysteriousConcoction7", "【霜狼之息】"},
            {"MysteriousConcoction8", "【影月菇】"},
            {"MysteriousConcoction9", "【太阳井苔】"},
            {"MysteriousConcoction1", "【诸神宴羹】"},
            {"MysteriousConcoction91", "【凤凰硝石粉】"},
            {"MysteriousConcoction92", "【海德拉腺体萃取】"},
            {"MysteriousConcoction93", "【泰坦指骨粉】"},
            {"MysteriousConcoction94", "【龙血】"},
            {"MysteriousConcoction95", "【弑神者遗恨】"},
            {"MysteriousConcoction96", "【不朽圣徒遗血】"},
            {"MysteriousConcoction97", "【龙晶髓】"},
            {"MysteriousConcoction98", "【世界树叶】"},
            {"MysteriousConcoction99", "【永恒井水稀释液】"},
            {"MysteriousConcoction100", "【神血】"}
        };

        // 先贤知识名称映射表
        private static readonly Dictionary<string, string> AncientKnowledgeNameMap = new Dictionary<string, string>
        {
            {"AncientKnowledge01", "【羊皮残卷】"},
            {"AncientKnowledge02", "【青铜铭文片】"},
            {"AncientKnowledge03", "【贤者笔记】"},
            {"AncientKnowledge04", "【星象图谱】"},
            {"AncientKnowledge05", "【圣典残篇】"},
            {"AncientKnowledge06", "【炼金手册】"},
            {"AncientKnowledge07", "【龙语残章】"},
            {"AncientKnowledge08", "【元素论著】"},
            {"AncientKnowledge09", "【时空奥秘录】"},
            {"AncientKnowledge10", "【灵魂学概论】"},
            {"AncientKnowledge11", "【法则解析】"},
            {"AncientKnowledge12", "【创世纪残卷】"},
            {"AncientKnowledge13", "【神之语言】"},
            {"AncientKnowledge14", "【因果律论】"},
            {"AncientKnowledge15", "【真理残章】"},
            {"AncientKnowledge16", "【永恒之书】"},
            {"AncientKnowledge17", "【启源之书】"},
            {"AncientKnowledge18", "【先贤启示录】"}
        };

        // 金色颜色定义
        private static readonly Color GoldColor = new Color(1f, 0.843f, 0f, 1f);
        
        // 新增：通知队列和状态控制
        private static Queue<NotificationData> notificationQueue = new Queue<NotificationData>();
        private static bool isProcessingQueue = false;
        private static GameObject coroutineRunnerObject = null;
        private static Canvas notificationCanvas = null;
        
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

        public static void ShowBreakNotification(Actor actor, string oldTrait, string newTrait)
        {
            string oldRealmName = GetRealmName(oldTrait);
            string newRealmName = GetRealmName(newTrait);
            
            string message = $"{actor.getName()} 已从 {oldRealmName} 突破到 {newRealmName}！";
            Debug.Log(message);
            
            // 将通知加入队列而不是立即显示
            EnqueueNotification(message, "BreakNotification");
        }

        public static void ShowGodlySigilNotification(Actor actor, string traitId)
        {
            string bodyName = GetMartialBodyName(traitId);
            string message = $"{actor.getName()} 沐浴神恩，被赐予了 {bodyName}！";
            Debug.Log(message);
            
            // 将通知加入队列
            EnqueueNotification(message, "GodlySigilNotification");
        }

        public static void ShowGodKingdomNotification(Actor actor, string traitId)
        {
            string GodKingdomName = GetGodKingdomName(traitId);
            string message = $"{actor.getName()} 神国坠落，自冥府归来！";
            Debug.Log(message);
            
            // 将通知加入队列
            EnqueueNotification(message, "GodKingdomNotification");
        }

        // 前六个灵植宝药的特质ID列表
        private static readonly HashSet<string> NoNotificationConcoctions = new HashSet<string>
        {
            "MysteriousConcoction1+", "MysteriousConcoction2", "MysteriousConcoction3",
            "MysteriousConcoction4", "MysteriousConcoction5", "MysteriousConcoction6"
        };

        // 前六个先贤知识的特质ID列表
        private static readonly HashSet<string> NoNotificationKnowledge = new HashSet<string>
        {
            "AncientKnowledge01", "AncientKnowledge02", "AncientKnowledge03",
            "AncientKnowledge04", "AncientKnowledge05", "AncientKnowledge06"
        };

        // 新增：显示灵植使用提示
        public static void ShowMysteriousConcoctionNotification(Actor actor, string ConcoctionTraitId)
        {
            string message;
            
            // 特殊处理MysteriousConcoction100（神血）
            if (ConcoctionTraitId == "MysteriousConcoction100")
            {
                message = $"{actor.getName()} 服用了神血，悟性提高！";
            }
            else
            {
                string ConcoctionName = GetMysteriousConcoctionName(ConcoctionTraitId);
                message = $"{actor.getName()} 获得奇遇，服用了{ConcoctionName}！";
            }
            
            Debug.Log(message);
            // 检查是否属于不播报的灵植
            if (NoNotificationConcoctions.Contains(ConcoctionTraitId))
            {
                return; // 前六种灵植不显示通知
            }
            // 将通知加入队列
            EnqueueNotification(message, "MysteriousConcoctionNotification");
        }

        // 新增：显示先贤知识使用提示
        public static void ShowAncientKnowledgeNotification(Actor actor, string KnowledgeTraitId)
        {
            string KnowledgeName = GetAncientKnowledgeName(KnowledgeTraitId);
            string message = $"{actor.getName()} 被知识之神眷顾，研读了{KnowledgeName}！";
            
            Debug.Log(message);
            // 检查是否属于不播报的知识
            if (NoNotificationKnowledge.Contains(KnowledgeTraitId))
            {
                return; // 前六种知识不显示通知
            }
            // 将通知加入队列
            EnqueueNotification(message, "AncientKnowledgeNotification");
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
                    coroutineRunnerObject = new GameObject("ChevalierNotificationCoroutineRunner");
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
                BreakNotification component = notificationObj.GetComponent<BreakNotification>();
                
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
            text.fontSize = 24;
            text.color = GoldColor;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Truncate;
            text.resizeTextForBestFit = true;
            text.resizeTextMaxSize = 32;
            text.resizeTextMinSize = 16;
            
            // 添加外发光效果
            Shadow shadow = textObj.AddComponent<Shadow>();
            shadow.effectColor = new Color(0f, 0f, 0f, 0.7f);
            shadow.effectDistance = new Vector2(2f, -2f);
            
            Outline outline = textObj.AddComponent<Outline>();
            outline.effectColor = new Color(0f, 0f, 0f, 0.8f);
            outline.effectDistance = new Vector2(1.5f, 1.5f);
            
            RectTransform textRect = textObj.GetComponent<RectTransform>();
            textRect.anchorMin = new Vector2(1.8f, 4.2f);
            textRect.anchorMax = new Vector2(1.8f, 4.2f);
            textRect.pivot = new Vector2(0.5f, 0.5f);
            textRect.sizeDelta = new Vector2(1800, 60);
            
            // 确保文本组件正确设置为居中对齐
            text.alignment = TextAnchor.MiddleCenter;
            
            // 添加淡入淡出效果
            BreakNotification component = notificationObj.AddComponent<BreakNotification>();
            component.Init(text);
            
            return notificationObj;
        }
        
        private static Canvas GetCanvas()
        {
            // 确保始终使用我们自己创建的独立Canvas，而不是游戏中现有的Canvas
            if (notificationCanvas == null || notificationCanvas.gameObject == null)
            {
                GameObject canvasObj = new GameObject("ChevalierNotificationCanvas");
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

        private static string GetMartialBodyName(string traitId)
        {
            if (MartialBodyNameMap.TryGetValue(traitId, out string bodyName))
            {
                return bodyName;
            }
            return traitId;
        }

        private static string GetGodKingdomName(string traitId)
        {
            if (GodKingdomNameMap.TryGetValue(traitId, out string GodKingdomName))
            {
                return GodKingdomName;
            }
            return "洞天";
        }

        // 新增：获取灵植名称
        private static string GetMysteriousConcoctionName(string traitId)
        {
            if (MysteriousConcoctionNameMap.TryGetValue(traitId, out string ConcoctionName))
            {
                return ConcoctionName;
            }
            return traitId;
        }

        private static string GetAncientKnowledgeName(string traitId)
        {
            if (AncientKnowledgeNameMap.TryGetValue(traitId, out string KnowledgeName))
            {
                return KnowledgeName;
            }
            return "古籍";
        }

        private static Font TryLoadFont(string fontName)
        {
            try
            {
                Font font = Resources.GetBuiltinResource<Font>(fontName);
                if (font != null)
                {
                    Debug.Log($"成功加载字体: {fontName}");
                    return font;
                }
                Debug.LogWarning($"无法加载字体: {fontName}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"加载字体时出错: {fontName}, 错误: {e.Message}");
            }
            return null;
        }
        
        private static Font CreateDefaultFont()
        {
            Debug.LogWarning("使用空字体作为后备方案，文本可能不会显示");
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
    
    public class BreakNotification : MonoBehaviour
    {
        private Text textComponent;
        private float showDuration = 3f;
        private float fadeDuration = 0.5f;
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