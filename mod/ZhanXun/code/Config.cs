using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeoModLoader;
using NeoModLoader.api;
using NeoModLoader.api.attributes;

namespace ChivalryZhanXun.code
{
    internal static class ZhanXunConfig
    {
        public static float AutoCollectPlunder { get; set; } = 0f;
        public static void AutoCollectPlunderCallBack(float pCurrentValue)
        {
            AutoCollectPlunder = pCurrentValue;
        }
        public static bool AutoCollectZhanXun = true;  // 开关
        public static bool AutoCollectMilitaryteleportation = true;  // 开关
        public static bool AutoCollectMilitarystateteleportation = true;  // 开关
        public static bool AutoCollectMilitarymagic = true;     // 军队法术开关
        public static bool AutoCollectMilitarystatemagic = true;     // 军国法术开关
        public static bool AutoCollectCity = false;     // 城市占领开关
        public static bool AutoCollectEternalWar = false;  // 永恒战争开关
        public static bool AutoCollectAdulthood = false;  // 成年开关
        public static float AutoCollectUrbanpopulation { get; set; } = 0f;

        public static void AutoCollectZhanXunCallBack(bool newValue)
        {
            AutoCollectZhanXun = newValue;
        }

        public static void AutoCollectMilitaryteleportationCallBack(bool newValue)
        {
            AutoCollectMilitaryteleportation = newValue;
        }
        public static void AutoCollectMilitarystateteleportationCallBack(bool newValue)
        {
            AutoCollectMilitarystateteleportation = newValue;
        }

        public static void AutoCollectMilitarymagicCallBack(bool newValue)
        {
            AutoCollectMilitarymagic = newValue;
        }

        public static void AutoCollectMilitarystatemagicCallBack(bool newValue)
        {
            AutoCollectMilitarystatemagic = newValue;
        }

        public static void AutoCollectCityCallBack(bool newValue)
        {
            AutoCollectCity = newValue;
        }

        public static void AutoCollectEternalWarCallBack(bool newValue)
        {
            AutoCollectEternalWar = newValue;
        }

        public static void AutoCollectAdulthoodCallBack(bool newValue)
        {
            AutoCollectAdulthood = newValue;
        }
        
        public static void AutoCollectUrbanpopulationCallBack(string pCurrentValue)
        {
            if (string.IsNullOrEmpty(pCurrentValue))
            {
                // 如果输入为空，则使用默认值
                AutoCollectUrbanpopulation = 0f;
            }
            else if (float.TryParse(pCurrentValue, out float value))
            {
                AutoCollectUrbanpopulation = value;
            }
        }
    }
}