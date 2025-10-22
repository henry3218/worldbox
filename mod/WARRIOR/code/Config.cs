using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeoModLoader;
using NeoModLoader.api;
using NeoModLoader.api.attributes;

namespace PeerlessOverpoweringWarrior.code.Config
{
    internal static class WarriorConfig
    {
        public static bool AutoCollectHarmony = true;  // 合道境收藏开关
        public static bool AutoCollectZhanWo = true;  // 斩我境收藏开关
        public static bool AutoCollectWuJi = true;     // 武极境收藏开关
        public static bool AutoCollectXuanZhen = true;  // 玄真境收藏开关
        
        public static bool NotifyHarmonyBreakthrough = true;  // 合道境突破通知开关
        public static bool NotifyZhanWoBreakthrough = true;  // 斩我境突破通知开关
        public static bool NotifyWuJiBreakthrough = true;     // 武极境突破通知开关
        public static bool NotifyXuanZhenBreakthrough = true;  // 玄真境突破通知开关
        // 武极境限制开关（默认关闭）
        public static bool LimitWuJi = false;
        // 斩我境限制开关（默认关闭）
        public static bool LimitZhanWo = false;
        // 合道境限制开关（默认关闭）       
        public static bool BreakthroughLimit = false;
        public static bool LimitWarrior9 = false;
        public static bool LimitWarrior8 = false;
        public static bool LimitWarrior7 = false;
        public static bool LimitWarrior6 = false;
        public static bool LimitWarrior5 = false;
        public static bool LimitWarrior4 = false;
        public static bool LimitWarrior3 = false;
        public static bool LimitWarrior2 = false;
        // 武道根骨觉醒开关（默认开启）
        public static bool AllowWarriorRootAwakening = true;
        // 阵道天赋觉醒开关（默认开启）
        public static bool AllowFormationTalentAwakening = true;
        // 斩我境突破获得自定义武器开关（默认开启）
        public static bool AllowZhanWoWeaponAward = true;
        // 武极境限制回调
        public static void LimitWuJiCallBack(bool newValue)
        {
            LimitWuJi = newValue;
        } 
        // 斩我境限制回调
        public static void LimitZhanWoCallBack(bool newValue)
        {
            LimitZhanWo = newValue;
        }
        // 合道境限制回调    
        public static void BreakthroughLimitCallBack(bool newValue)
        {
            BreakthroughLimit = newValue;
        }
        // 新增回调方法
        public static void LimitWarrior9CallBack(bool newValue)
        {
            LimitWarrior9 = newValue;
        }
        
        public static void LimitWarrior8CallBack(bool newValue)
        {
            LimitWarrior8 = newValue;
        }
        
        public static void LimitWarrior7CallBack(bool newValue)
        {
            LimitWarrior7 = newValue;
        }
        
        public static void LimitWarrior6CallBack(bool newValue)
        {
            LimitWarrior6 = newValue;
        }
        
        public static void LimitWarrior5CallBack(bool newValue)
        {
            LimitWarrior5 = newValue;
        }
        
        public static void LimitWarrior4CallBack(bool newValue)
        {
            LimitWarrior4 = newValue;
        }
        
        public static void LimitWarrior3CallBack(bool newValue)
        {
            LimitWarrior3 = newValue;
        }
        
        public static void LimitWarrior2CallBack(bool newValue)
        {
            LimitWarrior2 = newValue;
        } 
        // 合道境回调
        public static void AutoCollectHarmonyCallBack(bool newValue)
        {
            AutoCollectHarmony = newValue;
        }
        // 斩我境回调
        public static void AutoCollectZhanWoCallBack(bool newValue)
        {
            AutoCollectZhanWo = newValue;
        }
        
        // 武极境回调
        public static void AutoCollectWuJiCallBack(bool newValue)
        {
            AutoCollectWuJi = newValue;
        }
        
        // 玄真境回调
        public static void AutoCollectXuanZhenCallBack(bool newValue)
        {
            AutoCollectXuanZhen = newValue;
        }
        
        // 武道根骨觉醒回调
        public static void AllowWarriorRootAwakeningCallBack(bool newValue)
        {
            AllowWarriorRootAwakening = newValue;
        }
        
        // 阵道天赋觉醒回调
        public static void AllowFormationTalentAwakeningCallBack(bool newValue)
        {
            AllowFormationTalentAwakening = newValue;
        }
        
        // 斩我境突破获得自定义武器回调
        public static void AllowZhanWoWeaponAwardCallBack(bool newValue)
        {
            AllowZhanWoWeaponAward = newValue;
        }
        
        // 合道境突破通知回调
        public static void NotifyHarmonyBreakthroughCallBack(bool newValue)
        {
            NotifyHarmonyBreakthrough = newValue;
        }
        
        // 斩我境突破通知回调
        public static void NotifyZhanWoBreakthroughCallBack(bool newValue)
        {
            NotifyZhanWoBreakthrough = newValue;
        }
        
        // 武极境突破通知回调
        public static void NotifyWuJiBreakthroughCallBack(bool newValue)
        {
            NotifyWuJiBreakthrough = newValue;
        }
        
        // 玄真境突破通知回调
        public static void NotifyXuanZhenBreakthroughCallBack(bool newValue)
        {
            NotifyXuanZhenBreakthrough = newValue;
        }
    }
}