
using NCMS;
using NCMS.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using ReflectionUtility;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Reflection;
using System.Net.Http;
using NeoModLoader.api;
using NeoModLoader.api.attributes;
using NeoModLoader.General;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ZeN_01
{
	[ModEntry]
	class Main : MonoBehaviour
	{
		public static Dictionary<Actor, Actor> listOfTamedBeasts = new Dictionary<Actor, Actor>();
		public static string id = "ZeN_01";
		void Awake()
		{
			Traits01.init();						//特質
			Traits02.init();						//特質
			Traits03.init();						//特質
			TraitsEx.init();						//特質(添加到世界)
			TraitsGroup.init();						//特質群組
			TraitsOpposite.init();					//對立設定
			Traits01_symbolic.init();				//象徵性特質
			Traits01_original.init();				//遊戲原生特質

			Items01.init();							//道具
			ItemsGroup.init();						//道具群組
			Items_original.init();					//遊戲原生道具

			StatusEffects_normal1.init();			//通常狀態
			StatusEffects_normal2.init();			//通常狀態
			StatusEffects_normal3.init();			//通常狀態 (魔王狀態管理)
			StatusEffects_CDT1.init();				//冷卻狀態
			StatusEffects_CDT2.init();				//冷卻狀態
			StatusEffects_original.init();			//遊戲原生狀態

			NewProjectiles.init();					//子彈
			NewTerraformOptions.init();				//地形改造
			new Harmony(id).PatchAll(typeof(patch));//補丁

		}
		public void OnUpdate()
		{
			// 呼叫 SlaveSyncManager 中的同步方法
		}
	}
}