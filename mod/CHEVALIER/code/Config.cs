using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeoModLoader;
using NeoModLoader.api;
using NeoModLoader.api.attributes;

namespace Chevalier.code.Config
{
    internal static class ChevalierConfig
    {
        public static bool AutoCollectLieri = true;  // 合道境收藏开关
        public static bool AutoCollectYongyao = true;  // 斩我境收藏开关
        public static bool AutoCollectBuxiu = true;     // 武极境收藏开关
        public static bool ShowRealmSuffix = true;     // 显示境界后缀（默认开启）
        public static bool ShowTitlePrefix = false;    // 显示尊号前缀（默认关闭）
        // 合道境限制开关（默认关闭）       
        public static bool LieRiLimit = false;
        // 斩我境限制开关（默认关闭）
        public static bool LimitYongYao = false;
        // 武极境限制开关（默认关闭）
        public static bool LimitBuXiu = false;
        public static bool LimitChevalier9 = false;
        public static bool LimitChevalier8 = false;
        public static bool LimitChevalier7 = false;
        public static bool LimitChevalier6 = false;
        public static bool LimitChevalier5 = false;
        public static bool LimitChevalier4 = false;
        public static bool LimitChevalier3 = false;
        public static bool LimitChevalier2 = false;
        // 性能优化开关
        public static bool UsePerformanceOptimizations = true;
        public static bool OptimizeComprehensionCalculation = true;
        // 合道境限制回调    
        public static void LieRiLimitCallBack(bool newValue)
        {
            LieRiLimit = newValue;
        }
        // 斩我境限制回调
        public static void LimitYongYaoCallBack(bool newValue)
        {
            LimitYongYao = newValue;
        }
        // 武极境限制回调
        public static void LimitBuXiuCallBack(bool newValue)
        {
            LimitBuXiu = newValue;
        } 
        // 新增回调方法
        public static void LimitChevalier9CallBack(bool newValue)
        {
            LimitChevalier9 = newValue;
        }
        
        public static void LimitChevalier8CallBack(bool newValue)
        {
            LimitChevalier8 = newValue;
        }
        
        public static void LimitChevalier7CallBack(bool newValue)
        {
            LimitChevalier7 = newValue;
        }
        
        public static void LimitChevalier6CallBack(bool newValue)
        {
            LimitChevalier6 = newValue;
        }
        
        public static void LimitChevalier5CallBack(bool newValue)
        {
            LimitChevalier5 = newValue;
        }
        
        public static void LimitChevalier4CallBack(bool newValue)
        {
            LimitChevalier4 = newValue;
        }
        
        public static void LimitChevalier3CallBack(bool newValue)
        {
            LimitChevalier3 = newValue;
        }
        
        public static void LimitChevalier2CallBack(bool newValue)
        {
            LimitChevalier2 = newValue;
        }
        
        // 性能优化回调
        public static void UsePerformanceOptimizationsCallBack(bool newValue)
        {
            UsePerformanceOptimizations = newValue;
        }
        
        public static void OptimizeComprehensionCalculationCallBack(bool newValue)
        {
            OptimizeComprehensionCalculation = newValue;
        }  
        // 合道境回调
        public static void AutoCollectLieriCallBack(bool newValue)
        {
            AutoCollectLieri = newValue;
        }
        // 斩我境回调
        public static void AutoCollectYongyaoCallBack(bool newValue)
        {
            AutoCollectYongyao = newValue;
        }
        
        // 武极境回调
        public static void AutoCollectBuxiuCallBack(bool newValue)
        {
            AutoCollectBuxiu = newValue;
        }
        
        // 显示境界后缀回调
        public static void ShowRealmSuffixCallBack(bool newValue)
        {
            ShowRealmSuffix = newValue;
        }
        
        // 显示尊号前缀回调
        public static void ShowTitlePrefixCallBack(bool newValue)
        {
            ShowTitlePrefix = newValue;
        }
    }
}