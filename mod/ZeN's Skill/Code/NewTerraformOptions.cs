/* 
AUTHOR: MASON SCARBRO
VERSION: 1.0.0
*/
using System.Reflection;
using NCMS;
using NCMS.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ReflectionUtility;

namespace ZeN_01
{
	class NewTerraformOptions : MonoBehaviour
	{	//dnSpy 搜尋 TerraformLibrary "類"
		public static void init()
		{
			loadTerraformOptions();
		}
		private static void loadTerraformOptions()
		{
			#region 傲慢
			//地形改造設定 傲慢 黑白刃
			AssetManager.terraform.add(new TerraformOptions{// true / false
					id = "Blade_Black_White",			//ID
					flash = true,						//閃光?動畫?
					explode_tile = true,				//爆炸
					apply_force = true,					//震飛
					remove_top_tile = true,				//移除頂部圖塊
					remove_lava = true,					//移除岩漿
					remove_fire = true,					//移除火
					remove_frozen = true,				//移除冰
					remove_water = true,				//移除水
					remove_tornado = true,				//移除旋風
					set_fire = false,					//設置火焰
					add_heat = 0,						//添加熱量
					add_burned = true,					//添加燒毀
					transform_to_wasteland = false,		//廢土化
					applies_to_high_flyers = true,		//適用於高飛者
					destroy_buildings = true,			//摧毀房屋
					damage_buildings = true,			//傷害房屋
					make_ruins = true,					//製造廢墟
					force_power = 0.10f,				//震飛強度
					explode_strength = 1,				//爆炸強度
					explode_and_set_random_fire = false,//爆炸火焰
					lightning_effect = true,			//雷電效果
					shake = true,						//震動
					shake_intensity = 0.5f,				//震動強度
					attack_type = AttackType.None,		//攻擊類型
					damage = 100,						//傷害值
			});
			//地形改造設定 傲慢 白氣刃1
			AssetManager.terraform.add(new TerraformOptions{// true / false
					id = "Blade_White",					//ID
					flash = true,						//閃光?動畫?
					explode_tile = true,				//爆炸
					apply_force = true,					//震飛
					remove_top_tile = true,				//移除頂部圖塊
					remove_lava = false,					//移除岩漿
					remove_fire = false,					//移除火
					remove_frozen = true,				//移除冰
					remove_water = true,				//移除水
					remove_tornado = true,				//移除旋風
					set_fire = true,					//設置火焰
					add_heat = 0,						//添加熱量
					add_burned = true,					//添加燒毀
					transform_to_wasteland = false,		//廢土化
					applies_to_high_flyers = true,		//適用於高飛者
					destroy_buildings = true,			//摧毀房屋
					damage_buildings = true,			//傷害房屋
					make_ruins = true,					//製造廢墟
					force_power = 0.10f,				//震飛強度
					explode_strength = 1,				//爆炸強度
					explode_and_set_random_fire = true,	//爆炸火焰
					lightning_effect = true,			//雷電效果
					shake = true,						//震動
					shake_intensity = 0.5f,				//震動強度
					attack_type = AttackType.Divine,	//攻擊類型
					damage = 50,						//傷害值
			});
			//地形改造設定 傲慢 黑氣刃1
			AssetManager.terraform.add(new TerraformOptions{// true / false
					id = "Blade_Black",					//ID
					flash = true,						//閃光?動畫?
					explode_tile = true,				//爆炸
					apply_force = true,					//震飛
					remove_top_tile = true,				//移除頂部圖塊
					remove_lava = true,					//移除岩漿
					remove_fire = true,					//移除火
					remove_frozen = true,				//移除冰
					remove_water = true,				//移除水
					remove_tornado = true,				//移除旋風
					set_fire = false,					//設置火焰
					add_heat = 0,						//添加熱量
					add_burned = false,					//添加燒毀
					transform_to_wasteland = false,		//廢土化
					applies_to_high_flyers = true,		//適用於高飛者
					destroy_buildings = true,			//摧毀房屋
					damage_buildings = true,			//傷害房屋
					make_ruins = true,					//製造廢墟
					force_power = 0.01f,				//震飛強度
					explode_strength = 1,				//爆炸強度
					explode_and_set_random_fire = false,//爆炸火焰
					lightning_effect = true,			//雷電效果
					shake = true,						//震動
					shake_intensity = 0.5f,				//震動強度
					attack_type = AttackType.Gravity,	//攻擊類型
					damage = 50,						//傷害值
			});
			//地形改造設定 傲慢 白球2
			AssetManager.terraform.add(new TerraformOptions{// true / false
					id = "Blade_White02",					//ID
					flash = true,						//閃光?動畫?
					explode_tile = true,				//爆炸
					apply_force = true,					//震飛
					remove_top_tile = true,				//移除頂部圖塊
					remove_lava = false,					//移除岩漿
					remove_fire = false,					//移除火
					remove_frozen = true,				//移除冰
					remove_water = true,				//移除水
					remove_tornado = true,				//移除旋風
					set_fire = true,					//設置火焰
					add_heat = 0,						//添加熱量
					add_burned = true,					//添加燒毀
					transform_to_wasteland = false,		//廢土化
					applies_to_high_flyers = true,		//適用於高飛者
					destroy_buildings = true,			//摧毀房屋
					damage_buildings = true,			//傷害房屋
					make_ruins = true,					//製造廢墟
					force_power = 0.10f,				//震飛強度
					explode_strength = 1,				//爆炸強度
					explode_and_set_random_fire = true,	//爆炸火焰
					lightning_effect = true,			//雷電效果
					shake = true,						//震動
					shake_intensity = 0.5f,				//震動強度
					attack_type = AttackType.Divine,	//攻擊類型
					damage = 500,						//傷害值
			});
			//地形改造設定 傲慢 黑球2
			AssetManager.terraform.add(new TerraformOptions{// true / false
					id = "Blade_Black02",					//ID
					flash = true,						//閃光?動畫?
					explode_tile = true,				//爆炸
					apply_force = true,					//震飛
					remove_top_tile = true,				//移除頂部圖塊
					remove_lava = true,					//移除岩漿
					remove_fire = true,					//移除火
					remove_frozen = true,				//移除冰
					remove_water = true,				//移除水
					remove_tornado = true,				//移除旋風
					set_fire = false,					//設置火焰
					add_heat = 0,						//添加熱量
					add_burned = false,					//添加燒毀
					transform_to_wasteland = false,		//廢土化
					applies_to_high_flyers = true,		//適用於高飛者
					destroy_buildings = true,			//摧毀房屋
					damage_buildings = true,			//傷害房屋
					make_ruins = true,					//製造廢墟
					force_power = 0.01f,				//震飛強度
					explode_strength = 1,				//爆炸強度
					explode_and_set_random_fire = false,//爆炸火焰
					lightning_effect = true,			//雷電效果
					shake = true,						//震動
					shake_intensity = 0.5f,				//震動強度
					attack_type = AttackType.Gravity,	//攻擊類型
					damage = 500,						//傷害值
			});
			//地形改造設定 傲慢 白劍1
			AssetManager.terraform.add(new TerraformOptions{// true / false
					id = "Blade_White01",					//ID
					flash = true,						//閃光?動畫?
					explode_tile = true,				//爆炸
					apply_force = true,					//震飛
					remove_top_tile = true,				//移除頂部圖塊
					remove_lava = false,				//移除岩漿
					remove_fire = false,				//移除火
					remove_frozen = true,				//移除冰
					remove_water = true,				//移除水
					remove_tornado = true,				//移除旋風
					set_fire = true,					//設置火焰
					add_heat = 0,						//添加熱量
					add_burned = true,					//添加燒毀
					transform_to_wasteland = false,		//廢土化
					applies_to_high_flyers = true,		//適用於高飛者
					destroy_buildings = true,			//摧毀房屋
					damage_buildings = true,			//傷害房屋
					make_ruins = true,					//製造廢墟
					force_power = 0.10f,				//震飛強度
					explode_strength = 0,				//爆炸強度
					explode_and_set_random_fire = true,	//爆炸火焰
					lightning_effect = true,			//雷電效果
					shake = true,						//震動
					shake_intensity = 0.05f,			//震動強度
					attack_type = AttackType.Divine,	//攻擊類型
					damage = 50,						//傷害值
			});
			//地形改造設定 傲慢 黑劍1
			AssetManager.terraform.add(new TerraformOptions{// true / false
					id = "Blade_Black01",					//ID
					flash = true,						//閃光?動畫?
					explode_tile = true,				//爆炸
					apply_force = true,					//震飛
					remove_top_tile = true,				//移除頂部圖塊
					remove_lava = true,					//移除岩漿
					remove_fire = true,					//移除火
					remove_frozen = true,				//移除冰
					remove_water = true,				//移除水
					remove_tornado = true,				//移除旋風
					set_fire = false,					//設置火焰
					add_heat = 0,						//添加熱量
					add_burned = false,					//添加燒毀
					transform_to_wasteland = false,		//廢土化
					applies_to_high_flyers = true,		//適用於高飛者
					destroy_buildings = true,			//摧毀房屋
					damage_buildings = true,			//傷害房屋
					make_ruins = true,					//製造廢墟
					force_power = 0.01f,				//震飛強度
					explode_strength = 0,				//爆炸強度
					explode_and_set_random_fire = false,//爆炸火焰
					lightning_effect = true,			//雷電效果
					shake = true,						//震動
					shake_intensity = 0.05f,				//震動強度
					attack_type = AttackType.Gravity,	//攻擊類型
					damage = 50,						//傷害值
			});
			//地形改造設定 傲慢 白劍2
			AssetManager.terraform.add(new TerraformOptions{// true / false
					id = "Blade_White03",					//ID
					flash = true,						//閃光?動畫?
					explode_tile = true,				//爆炸
					apply_force = true,					//震飛
					remove_top_tile = true,				//移除頂部圖塊
					remove_lava = false,				//移除岩漿
					remove_fire = false,				//移除火
					remove_frozen = true,				//移除冰
					remove_water = true,				//移除水
					remove_tornado = true,				//移除旋風
					set_fire = true,					//設置火焰
					add_heat = 0,						//添加熱量
					add_burned = true,					//添加燒毀
					transform_to_wasteland = false,		//廢土化
					applies_to_high_flyers = true,		//適用於高飛者
					destroy_buildings = true,			//摧毀房屋
					damage_buildings = true,			//傷害房屋
					make_ruins = true,					//製造廢墟
					force_power = 0.10f,				//震飛強度
					explode_strength = 0,				//爆炸強度
					explode_and_set_random_fire = true,	//爆炸火焰
					lightning_effect = true,			//雷電效果
					shake = true,						//震動
					shake_intensity = 0.05f,			//震動強度
					attack_type = AttackType.Divine,	//攻擊類型
					damage = 500,						//傷害值
			});
			//地形改造設定 傲慢 黑劍2
			AssetManager.terraform.add(new TerraformOptions{// true / false
					id = "Blade_Black03",					//ID
					flash = true,						//閃光?動畫?
					explode_tile = true,				//爆炸
					apply_force = true,					//震飛
					remove_top_tile = true,				//移除頂部圖塊
					remove_lava = true,					//移除岩漿
					remove_fire = true,					//移除火
					remove_frozen = true,				//移除冰
					remove_water = true,				//移除水
					remove_tornado = true,				//移除旋風
					set_fire = false,					//設置火焰
					add_heat = 0,						//添加熱量
					add_burned = false,					//添加燒毀
					transform_to_wasteland = false,		//廢土化
					applies_to_high_flyers = true,		//適用於高飛者
					destroy_buildings = true,			//摧毀房屋
					damage_buildings = true,			//傷害房屋
					make_ruins = true,					//製造廢墟
					force_power = 0.01f,				//震飛強度
					explode_strength = 0,				//爆炸強度
					explode_and_set_random_fire = false,//爆炸火焰
					lightning_effect = true,			//雷電效果
					shake = true,						//震動
					shake_intensity = 0.05f,				//震動強度
					attack_type = AttackType.Gravity,	//攻擊類型
					damage = 500,						//傷害值
			});
			#endregion
			#region 憤怒
			//地形改造設定 憤怒 火球
			AssetManager.terraform.add(new TerraformOptions{// true / false
					id = "High_Fireball",					//ID
					flash = true,						//閃光?動畫?
					explode_tile = true,				//爆炸
					apply_force = true,					//震飛
					remove_top_tile = true,				//移除頂部圖塊
					remove_lava = false,					//移除岩漿
					remove_fire = false,					//移除火
					remove_frozen = true,				//移除冰
					remove_water = true,				//移除水
					remove_tornado = true,				//移除旋風
					set_fire = true,					//設置火焰
					add_heat = 0,						//添加熱量
					add_burned = true,					//添加燒毀
					transform_to_wasteland = false,		//廢土化
					applies_to_high_flyers = true,		//適用於高飛者
					destroy_buildings = true,			//摧毀房屋
					damage_buildings = true,			//傷害房屋
					make_ruins = true,					//製造廢墟
					force_power = 2.00f,				//震飛強度
					explode_strength = 1,				//爆炸強度
					explode_and_set_random_fire = true,	//爆炸火焰
					lightning_effect = true,			//雷電效果
					shake = true,						//震動
					shake_intensity = 0.0f,				//震動強度
					attack_type = AttackType.Explosion,	//攻擊類型
					damage = 500,						//傷害值
			});
			//地形改造設定 憤怒 火球2
			AssetManager.terraform.add(new TerraformOptions{// true / false
					id = "High_Fireball_2",					//ID
					flash = true,						//閃光?動畫?
					explode_tile = true,				//爆炸
					apply_force = true,					//震飛
					remove_top_tile = true,				//移除頂部圖塊
					remove_lava = false,					//移除岩漿
					remove_fire = false,					//移除火
					remove_frozen = true,				//移除冰
					remove_water = true,				//移除水
					remove_tornado = true,				//移除旋風
					set_fire = true,					//設置火焰
					add_heat = 0,						//添加熱量
					add_burned = true,					//添加燒毀
					transform_to_wasteland = false,		//廢土化
					applies_to_high_flyers = true,		//適用於高飛者
					destroy_buildings = true,			//摧毀房屋
					damage_buildings = true,			//傷害房屋
					make_ruins = true,					//製造廢墟
					force_power = 3.00f,				//震飛強度
					explode_strength = 2,				//爆炸強度
					explode_and_set_random_fire = true,	//爆炸火焰
					lightning_effect = true,			//雷電效果
					shake = true,						//震動
					shake_intensity = 0.0f,				//震動強度
					attack_type = AttackType.Explosion,	//攻擊類型
					damage = 1000,						//傷害值
			});
			//地形改造設定 憤怒 小火球
			AssetManager.terraform.add(new TerraformOptions{// true / false
					id = "High_RedOrbl",					//ID
					flash = true,						//閃光?動畫?
					explode_tile = false,				//爆炸
					apply_force = false,					//震飛
					remove_top_tile = false,				//移除頂部圖塊
					remove_lava = false,					//移除岩漿
					remove_fire = false,					//移除火
					remove_frozen = true,				//移除冰
					remove_water = true,				//移除水
					remove_tornado = true,				//移除旋風
					set_fire = true,					//設置火焰
					add_heat = 0,						//添加熱量
					add_burned = true,					//添加燒毀
					transform_to_wasteland = false,		//廢土化
					applies_to_high_flyers = true,		//適用於高飛者
					destroy_buildings = true,			//摧毀房屋
					damage_buildings = true,			//傷害房屋
					make_ruins = true,					//製造廢墟
					force_power = 1.50f,				//震飛強度
					explode_strength = 0,				//爆炸強度
					explode_and_set_random_fire = true,	//爆炸火焰
					lightning_effect = false,			//雷電效果
					shake = false,						//震動
					shake_intensity = 0.0f,				//震動強度
					attack_type = AttackType.Explosion,	//攻擊類型
					damage = 10,						//傷害值
			});
			//地形改造設定 憤怒 超火球
			AssetManager.terraform.add(new TerraformOptions{// true / false
					id = "NuclearFusion_to",					//ID
					flash = true,						//閃光?動畫?
					explode_tile = true,				//爆炸
					apply_force = true,					//震飛
					remove_top_tile = true,				//移除頂部圖塊
					remove_lava = false,				//移除岩漿
					remove_fire = false,				//移除火
					remove_frozen = true,				//移除冰
					remove_water = true,				//移除水
					remove_tornado = true,				//移除旋風
					set_fire = true,					//設置火焰
					add_heat = 404,						//添加熱量
					add_burned = true,					//添加燒毀
					transform_to_wasteland = false,		//廢土化
					applies_to_high_flyers = true,		//適用於高飛者
					destroy_buildings = true,			//摧毀房屋
					damage_buildings = true,			//傷害房屋
					make_ruins = true,					//製造廢墟
					force_power = 4.50f,				//震飛強度
					explode_strength = 1,				//爆炸強度
					explode_and_set_random_fire = true,	//爆炸火焰
					lightning_effect = true,			//雷電效果
					shake = true,						//震動
					shake_intensity = 10.0f,			//震動強度
					attack_type = AttackType.Explosion,	//攻擊類型
					damage = 1000,						//傷害值
			});
			#endregion
			#region 暴食
			//地形改造設定 暴食 餐叉
			AssetManager.terraform.add(new TerraformOptions{// true / false
					id = "Tableware1",						//ID
					flash = true,						//閃光?動畫?
					explode_tile = false,				//爆炸
					apply_force = true,					//震飛
					remove_top_tile = false,				//移除頂部圖塊
					remove_lava = false,					//移除岩漿
					remove_fire = false,					//移除火
					remove_frozen = false,				//移除冰
					remove_water = false,				//移除水
					remove_tornado = false,				//移除旋風
					set_fire = false,					//設置火焰
					add_heat = 0,						//添加熱量
					add_burned = false,					//添加燒毀
					transform_to_wasteland = false,		//廢土化
					applies_to_high_flyers = false,		//適用於高飛者
					destroy_buildings = true,			//摧毀房屋
					damage_buildings = true,			//傷害房屋
					make_ruins = true,					//製造廢墟
					force_power = 1.50f,					//震飛強度
					explode_strength = 0,				//爆炸強度
					explode_and_set_random_fire = false,//爆炸火焰
					lightning_effect = false,			//雷電效果
					shake = false,						//震動
					shake_intensity = 0.5f,				//震動強度
					attack_type = AttackType.Starvation,//攻擊類型
					damage = 25,						//傷害值
			});
			//地形改造設定 暴食 餐刀
			AssetManager.terraform.add(new TerraformOptions{// true / false
					id = "Tableware2",						//ID
					flash = true,						//閃光?動畫?
					explode_tile = false,				//爆炸
					apply_force = true,					//震飛
					remove_top_tile = false,				//移除頂部圖塊
					remove_lava = false,					//移除岩漿
					remove_fire = false,					//移除火
					remove_frozen = false,				//移除冰
					remove_water = false,				//移除水
					remove_tornado = false,				//移除旋風
					set_fire = false,					//設置火焰
					add_heat = 0,						//添加熱量
					add_burned = false,					//添加燒毀
					transform_to_wasteland = false,		//廢土化
					applies_to_high_flyers = false,		//適用於高飛者
					destroy_buildings = true,			//摧毀房屋
					damage_buildings = true,			//傷害房屋
					make_ruins = true,					//製造廢墟
					force_power = 1.50f,				//震飛強度
					explode_strength = 0,				//爆炸強度
					explode_and_set_random_fire = false,//爆炸火焰
					lightning_effect = false,			//雷電效果
					shake = false,						//震動
					shake_intensity = 0.5f,				//震動強度
					attack_type = AttackType.Eaten,		//攻擊類型
					damage = 25,						//傷害值
			});
			//地形改造設定 暴食 酸彈
			AssetManager.terraform.add(new TerraformOptions{// true / false
					id = "High_AcidBall",						//ID
					flash = true,						//閃光?動畫?
					explode_tile = true,				//爆炸
					apply_force = true,					//震飛
					remove_top_tile = true,				//移除頂部圖塊
					remove_lava = true,					//移除岩漿
					remove_fire = true,					//移除火
					remove_frozen = true,				//移除冰
					remove_water = true,				//移除水
					remove_tornado = false,				//移除旋風
					set_fire = false,					//設置火焰
					add_heat = 0,						//添加熱量
					add_burned = false,					//添加燒毀
					transform_to_wasteland = true,		//廢土化
					applies_to_high_flyers = true,		//適用於高飛者
					destroy_buildings = true,			//摧毀房屋
					damage_buildings = true,			//傷害房屋
					make_ruins = true,					//製造廢墟
					force_power = 3.00f,				//震飛強度
					explode_strength = 2,				//爆炸強度
					explode_and_set_random_fire = false,//爆炸火焰
					lightning_effect = false,			//雷電效果
					shake = false,						//震動
					shake_intensity = 0.0f,				//震動強度
					attack_type = AttackType.Acid,		//攻擊類型
					damage = 100,						//傷害值
			});
			#endregion
			#region 怠惰
			//地形改造設定 怠惰 大雪彈
			AssetManager.terraform.add(new TerraformOptions{// true / false
					id = "Avalanche_to",				//ID
					flash = true,						//閃光?動畫?
					explode_tile = true,				//爆炸
					apply_force = true,					//震飛
					remove_top_tile = true,				//移除頂部圖塊
					remove_lava = true,					//移除岩漿
					remove_fire = true,					//移除火
					remove_frozen = false,				//移除冰
					remove_water = false,				//移除水
					remove_tornado = true,				//移除旋風
					set_fire = false,					//設置火焰
					add_heat = -0,						//添加熱量
					add_burned = false,					//添加燒毀
					transform_to_wasteland = false,		//廢土化
					applies_to_high_flyers = false,		//適用於高飛者
					destroy_buildings = true,			//摧毀房屋
					damage_buildings = true,			//傷害房屋
					make_ruins = true,					//製造廢墟
					force_power = 4.50f,				//震飛強度
					explode_strength = 0,				//爆炸強度
					explode_and_set_random_fire = false,//爆炸火焰
					lightning_effect = false,			//雷電效果
					shake = true,						//震動
					shake_intensity = 10.0f,			//震動強度
					attack_type = AttackType.Other,		//攻擊類型
					damage = 1000,						//傷害值
			});
			//地形改造設定 怠惰 冰槍
			AssetManager.terraform.add(new TerraformOptions{// true / false
					id = "IcePick_to",					//ID
					flash = true,						//閃光?動畫?
					explode_tile = false,				//爆炸
					apply_force = false,				//震飛
					remove_top_tile = false,			//移除頂部圖塊
					remove_lava = true,					//移除岩漿
					remove_fire = true,					//移除火
					remove_frozen = false,				//移除冰
					remove_water = false,				//移除水
					remove_tornado = false,				//移除旋風
					set_fire = false,					//設置火焰
					add_heat = -0,						//添加熱量
					add_burned = false,					//添加燒毀
					transform_to_wasteland = false,		//廢土化
					applies_to_high_flyers = false,		//適用於高飛者
					destroy_buildings = true,			//摧毀房屋
					damage_buildings = true,			//傷害房屋
					make_ruins = true,					//製造廢墟
					force_power = 0.70f,				//震飛強度
					explode_strength = 0,				//爆炸強度
					explode_and_set_random_fire = false,//爆炸火焰
					lightning_effect = false,			//雷電效果
					shake = false,						//震動
					shake_intensity = 10.0f,			//震動強度
					attack_type = AttackType.Other,		//攻擊類型
					damage = 100,						//傷害值
			});
			//地形改造設定 怠惰 雪花彈
			AssetManager.terraform.add(new TerraformOptions{// true / false
					id = "Snowflake_to",				//ID
					flash = true,						//閃光?動畫?
					explode_tile = false,				//爆炸
					apply_force = true,					//震飛
					remove_top_tile = false,			//移除頂部圖塊
					remove_lava = true,					//移除岩漿
					remove_fire = true,					//移除火
					remove_frozen = false,				//移除冰
					remove_water = false,				//移除水
					remove_tornado = false,				//移除旋風
					set_fire = false,					//設置火焰
					add_heat = -0,						//添加熱量
					add_burned = false,					//添加燒毀
					transform_to_wasteland = false,		//廢土化
					applies_to_high_flyers = false,		//適用於高飛者
					destroy_buildings = true,			//摧毀房屋
					damage_buildings = true,			//傷害房屋
					make_ruins = true,					//製造廢墟
					force_power = 0.70f,				//震飛強度
					explode_strength = 0,				//爆炸強度
					explode_and_set_random_fire = false,//爆炸火焰
					lightning_effect = false,			//雷電效果
					shake = false,						//震動
					shake_intensity = 10.0f,			//震動強度
					attack_type = AttackType.Other,		//攻擊類型
					damage = 10,						//傷害值
			});
			#endregion
			#region 色慾
			//地形改造設定 色慾 流星
			AssetManager.terraform.add(new TerraformOptions{// true / false
					id = "meteorite_ex",				//ID
					flash = true,						//閃光
					explode_tile = true,				//爆炸
					apply_force = true,					//震飛
					remove_top_tile = true,				//移除頂部圖塊
					remove_lava = true,					//移除岩漿
					remove_fire = false,				//移除火
					remove_frozen = true,				//移除冰
					remove_water = true,				//移除水
					remove_tornado = true,				//移除旋風
					set_fire = false,					//設置火焰
					add_heat = 0,						//添加熱量
					add_burned = true,					//添加燒毀
					transform_to_wasteland = false,		//廢土化
					applies_to_high_flyers = true,		//適用於高飛者
					destroy_buildings = true,			//摧毀房屋
					damage_buildings = true,			//傷害房屋
					make_ruins = true,					//製造廢墟
					force_power = 0.70f,				//震飛強度
					explode_strength = 1,				//爆炸強度
					explode_and_set_random_fire = true,//爆炸火焰
					lightning_effect = false,			//雷電效果
					shake = true,						//震動
					shake_intensity = 10.0f,			//震動強度
					attack_type = AttackType.Other,		//攻擊類型
					damage = 3000,						//傷害值
			});
			#endregion
			#region 嫉妒
			//地形改造設定 雷擊
			AssetManager.terraform.add(new TerraformOptions{// true / false
					id = "lightning_ex",				//ID
					flash = true,						//閃光
					explode_tile = true,				//爆炸
					apply_force = true,					//震飛
					remove_top_tile = true,				//移除頂部圖塊
					remove_lava = true,					//移除岩漿
					remove_fire = true,					//移除火
					remove_frozen = true,				//移除冰
					remove_water = true,				//移除水
					remove_tornado = true,				//移除旋風
					set_fire = false,					//設置火焰
					add_heat = 0,						//添加熱量
					add_burned = true,					//添加燒毀
					transform_to_wasteland = false,		//廢土化
					applies_to_high_flyers = true,		//適用於高飛者
					destroy_buildings = true,			//摧毀房屋
					damage_buildings = true,			//傷害房屋
					make_ruins = true,					//製造廢墟
					force_power = 1.70f,				//震飛強度
					explode_strength = 1,				//爆炸強度
					explode_and_set_random_fire = true,	//爆炸火焰
					lightning_effect = false,			//雷電效果
					shake = true,						//震動
					shake_intensity = 5.0f,				//震動強度
					attack_type = AttackType.Poison,	//攻擊類型
					damage = 500,						//傷害值
			});
			#endregion
			#region 裁決
			//地形改造設定 十字
			AssetManager.terraform.add(new TerraformOptions{// true / false
					id = "Justice_01to",				//ID
					flash = true,						//閃光
					explode_tile = true,				//爆炸
					apply_force = true,					//震飛
					remove_top_tile = true,				//移除頂部圖塊
					remove_lava = true,					//移除岩漿
					remove_fire = true,					//移除火
					remove_frozen = true,				//移除冰
					remove_water = true,				//移除水
					remove_tornado = true,				//移除旋風
					set_fire = false,					//設置火焰
					add_heat = 0,						//添加熱量
					add_burned = true,					//添加燒毀
					transform_to_wasteland = false,		//廢土化
					applies_to_high_flyers = true,		//適用於高飛者
					destroy_buildings = true,			//摧毀房屋
					damage_buildings = true,			//傷害房屋
					make_ruins = false,					//製造廢墟
					force_power = 0.50f,				//震飛強度
					explode_strength = 1,				//爆炸強度
					explode_and_set_random_fire = false,//爆炸火焰
					lightning_effect = false,			//雷電效果
					shake = false,						//震動
					shake_intensity = 5.0f,				//震動強度
					attack_type = AttackType.Divine,	//攻擊類型
					//damage = 1500,						//傷害值
			});
			//地形改造設定 一字
			AssetManager.terraform.add(new TerraformOptions{// true / false
					id = "Justice_02to",				//ID
					flash = true,						//閃光
					explode_tile = true,				//爆炸
					apply_force = true,					//震飛
					remove_top_tile = true,				//移除頂部圖塊
					remove_lava = true,					//移除岩漿
					remove_fire = true,					//移除火
					remove_frozen = true,				//移除冰
					remove_water = true,				//移除水
					remove_tornado = true,				//移除旋風
					set_fire = false,					//設置火焰
					add_heat = 0,						//添加熱量
					add_burned = true,					//添加燒毀
					transform_to_wasteland = false,		//廢土化
					applies_to_high_flyers = true,		//適用於高飛者
					destroy_buildings = true,			//摧毀房屋
					damage_buildings = true,			//傷害房屋
					make_ruins = false,					//製造廢墟
					force_power = 0.50f,				//震飛強度
					explode_strength = 1,				//爆炸強度
					explode_and_set_random_fire = false,	//爆炸火焰
					lightning_effect = false,			//雷電效果
					shake = false,						//震動
					shake_intensity = 5.0f,				//震動強度
					attack_type = AttackType.Divine,	//攻擊類型
					//damage = 2500,						//傷害值
			});
			#endregion

		}
	}

}