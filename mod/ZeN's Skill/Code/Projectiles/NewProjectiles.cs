/* 
AUTHOR: MASON SCARBRO
VERSION: 1.0.0
*/
using System;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NCMS;
using NCMS.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ReflectionUtility;
using HarmonyLib;

namespace ZeN_01
{
	
	class NewProjectiles : MonoBehaviour
	{	//dnSpy 搜尋 ProjectileLibrary "類"
		private static List<BaseSimObject> enemyObjectsList = new List<BaseSimObject>();
		public static void init()
		{
			loadProjectiles();
		}

		private static void loadProjectiles()
		{
			#region 傲慢
			//子彈設定 傲慢 黑白月牙	BladeBlackWhite
			AssetManager.projectiles.add(new ProjectileAsset{// true / false
				id = "BladeBlackWhite",									//子彈ID
				speed = 60f,												//子彈速度
				texture = "BladeBlackWhite",								//子彈材質 資料夾位置
				trail_effect_enabled = false,								//尾跡效果
				terraform_option = "Blade_Black_White",						//地形改造設定 銜接到 NewTerraformOptions 文件中的 ID
				terraform_range = 2,										//地形範圍
				draw_light_area = true,										//繪製光區
				draw_light_size = 0.25f,									//繪製燈光大小
				trigger_on_collision = true,								//觸發碰撞
				look_at_target = true,										//鎖定目標
				hit_shake = true,											//擊中震動
				shake_duration = 0.01f,										//震動持續時間
				shake_interval = 0.01f,										//震動間隔
				shake_intensity = 0.25f,									//震動強度
				can_be_blocked = false,										//阻擋效果
				scale_start = 0.005f,										//開始尺寸
				scale_target = 0.050f,										//最終尺寸
				trail_effect_scale = 0.1f,									//尾跡效應大小
				trail_effect_timer = 0.1f,									//尾跡效應時間
				trail_effect_id = "fx_plasma_trail",						//尾跡效應ID
				end_effect = string.Empty,
				sound_launch = "event:/SFX/WEAPONS/WeaponFireballStart",	//聲音發射
				sound_impact = "event:/SFX/WEAPONS/WeaponFireballLand",		//聲音影響
			//	impact_actions = new AttackAction(delegate(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
			//	{
			//		bool result1 = NewProjectilesActions.Anti_OtherRaces(pSelf, pTarget, pTile);
			//		bool result2 = NewProjectilesActions.Anti_Shield(pSelf, pTarget, pTile);
			//		bool result3 = NewProjectilesActions.Blade_Black_WhiteTerraform(pTile);
			//		return result1 || result2 || result3;
			//	})
			});
			//子彈設定 傲慢 白牙		BladeWhite
			AssetManager.projectiles.add(new ProjectileAsset{// true / false
				id = "BladeWhite",									//子彈ID
				speed = 100f,												//子彈速度
				texture = "BladeWhite",								//子彈材質 資料夾位置
				trail_effect_enabled = false,								//尾跡效果
				terraform_option = "Blade_White",						//地形改造設定 銜接到 NewTerraformOptions 文件中的 ID
				terraform_range = 3,										//地形範圍
				draw_light_area = true,										//繪製光區
				draw_light_size = 0.25f,									//繪製燈光大小
				trigger_on_collision = true,								//觸發碰撞
				look_at_target = true,										//鎖定目標
				hit_shake = true,											//擊中震動
				shake_duration = 0.01f,										//震動持續時間
				shake_interval = 0.01f,										//震動間隔
				shake_intensity = 0.25f,									//震動強度
				can_be_blocked = false,										//阻擋效果
				scale_start = 0.005f,										//開始尺寸
				scale_target = 0.025f,										//最終尺寸
				trail_effect_scale = 0.1f,									//尾跡效應大小
				trail_effect_timer = 0.1f,									//尾跡效應時間
				trail_effect_id = "fx_plasma_trail",						//尾跡效應ID
				end_effect = string.Empty,
				sound_launch = "event:/SFX/WEAPONS/WeaponFireballStart",	//聲音發射
				sound_impact = "event:/SFX/WEAPONS/WeaponFireballLand",		//聲音影響
			//	impact_actions = new AttackAction(delegate(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
			//	{
			//		bool result1 = NewProjectilesActions.Anti_OtherRaces(pSelf, pTarget, pTile);
			//		bool result2 = NewProjectilesActions.Anti_Shield(pSelf, pTarget, pTile);
			//		bool result3 = NewProjectilesActions.Blade_WhiteTerraform(pTile);
			//		return result1 || result2 || result3;
			//	})
			});
			//子彈設定 傲慢 黑牙		BladeBlack
			AssetManager.projectiles.add(new ProjectileAsset{// true / false
				id = "BladeBlack",											//子彈ID
				speed = 100f,												//子彈速度
				texture = "BladeBlack",										//子彈材質 資料夾位置
				trail_effect_enabled = false,								//尾跡效果
				terraform_option = "Blade_Black",							//地形改造設定 銜接到 NewTerraformOptions 文件中的 ID
				terraform_range = 3,										//地形範圍
				draw_light_area = true,										//繪製光區
				draw_light_size = 0.25f,									//繪製燈光大小
				trigger_on_collision = true,								//觸發碰撞
				look_at_target = true,										//鎖定目標
				hit_shake = false,											//擊中震動
				shake_duration = 0.01f,										//震動持續時間
				shake_interval = 0.01f,										//震動間隔
				shake_intensity = 0.25f,									//震動強度
				can_be_blocked = false,										//阻擋效果
				scale_start = 0.005f,										//開始尺寸
				scale_target = 0.025f,										//最終尺寸
				trail_effect_scale = 0.1f,									//尾跡效應大小
				trail_effect_timer = 0.1f,									//尾跡效應時間
				trail_effect_id = "fx_plasma_trail",						//尾跡效應ID
				end_effect = string.Empty,
				sound_launch = "event:/SFX/WEAPONS/WeaponFireballStart",	//聲音發射
				sound_impact = "event:/SFX/WEAPONS/WeaponFireballLand",		//聲音影響
			//	impact_actions = new AttackAction(delegate(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
			//	{
			//		bool result1 = NewProjectilesActions.Anti_OtherRaces(pSelf, pTarget, pTile);
			//		bool result2 = NewProjectilesActions.Anti_Shield(pSelf, pTarget, pTile);
			//		bool result3 = NewProjectilesActions.Blade_BlackTerraform(pTile);
			//		return result1 || result2 || result3;
			//	})
			});
			//子彈設定 傲慢 白劍		BladeWhite01
			AssetManager.projectiles.add(new ProjectileAsset{// true / false
				id = "BladeWhite01",									//子彈ID
				speed = 120f,												//子彈速度
				texture = "BladeWhite01",								//子彈材質 資料夾位置
				trail_effect_enabled = false,								//尾跡效果
				terraform_option = "Blade_White01",						//地形改造設定 銜接到 NewTerraformOptions 文件中的 ID
				terraform_range = 3,										//地形範圍
				draw_light_area = true,										//繪製光區
				draw_light_size = 0.25f,									//繪製燈光大小
				trigger_on_collision = false,								//觸發碰撞
				look_at_target = true,										//鎖定目標
				can_be_left_on_ground = true,								//可留存於地板
				hit_shake = true,											//擊中震動
				shake_duration = 0.01f,										//震動持續時間
				shake_interval = 0.01f,										//震動間隔
				shake_intensity = 0.25f,									//震動強度
				can_be_blocked = true,										//阻擋效果
				scale_start = 0.005f,										//開始尺寸
				scale_target = 0.005f,										//最終尺寸
				trail_effect_scale = 0.1f,									//尾跡效應大小
				trail_effect_timer = 0.1f,									//尾跡效應時間
				trail_effect_id = "fx_plasma_trail",						//尾跡效應ID
				end_effect = string.Empty,
				sound_launch = "event:/SFX/HIT/HitSwordSword",		//聲音-發射
				sound_impact = "event:/SFX/HIT/HitSwordSword",		//聲音-碰撞
			//	impact_actions = new AttackAction(delegate(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
			//	{
			//		bool result1 = NewProjectilesActions.Anti_OtherRaces(pSelf, pTarget, pTile);
			//		bool result2 = NewProjectilesActions.Anti_Shield(pSelf, pTarget, pTile);
			//		bool result3 = NewProjectilesActions.Blade_White01Terraform(pTile);
			//		return result1 || result2 || result3;
			//	})
			});
			//子彈設定 傲慢 黑劍		BladeBlack01
			AssetManager.projectiles.add(new ProjectileAsset{// true / false
				id = "BladeBlack01",									//子彈ID
				speed = 120f,												//子彈速度
				texture = "BladeBlack01",								//子彈材質 資料夾位置
				trail_effect_enabled = false,								//尾跡效果
				terraform_option = "Blade_Black01",						//地形改造設定 銜接到 NewTerraformOptions 文件中的 ID
				terraform_range = 3,										//地形範圍
				draw_light_area = true,										//繪製光區
				draw_light_size = 0.25f,									//繪製燈光大小
				trigger_on_collision = false,								//觸發碰撞
				can_be_left_on_ground = true,								//可留存於地板
				look_at_target = true,										//鎖定目標
				hit_shake = false,											//擊中震動
				shake_duration = 0.01f,										//震動持續時間
				shake_interval = 0.01f,										//震動間隔
				shake_intensity = 0.25f,									//震動強度
				can_be_blocked = true,										//阻擋效果
				scale_start = 0.005f,										//開始尺寸
				scale_target = 0.005f,										//最終尺寸
				trail_effect_scale = 0.1f,									//尾跡效應大小
				trail_effect_timer = 0.1f,									//尾跡效應時間
				trail_effect_id = "fx_plasma_trail",						//尾跡效應ID
				end_effect = string.Empty,
				sound_launch = "event:/SFX/HIT/HitSwordSword",	//聲音發射
				sound_impact = "event:/SFX/HIT/HitSwordSword",		//聲音影響
			//	impact_actions = new AttackAction(delegate(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
			//	{
			//		bool result1 = NewProjectilesActions.Anti_OtherRaces(pSelf, pTarget, pTile);
			//		bool result2 = NewProjectilesActions.Anti_Shield(pSelf, pTarget, pTile);
			//		bool result3 = NewProjectilesActions.Blade_Black01Terraform(pTile);
			//		return result1 || result2 || result3;
			//	})
			});
			//子彈設定 傲慢 白球		BladeWhite02
			AssetManager.projectiles.add(new ProjectileAsset{// true / false
				id = "BladeWhite02",									//子彈ID
				speed = 80f,												//子彈速度
				texture = "BladeWhite02",								//子彈材質 資料夾位置
				trail_effect_enabled = false,								//尾跡效果
				terraform_option = "Blade_White02",						//地形改造設定 銜接到 NewTerraformOptions 文件中的 ID
				terraform_range = 5,										//地形範圍
				draw_light_area = true,										//繪製光區
				draw_light_size = 0.25f,									//繪製燈光大小
				trigger_on_collision = true,								//觸發碰撞
				look_at_target = true,										//鎖定目標
				hit_shake = true,											//擊中震動
				shake_duration = 0.01f,										//震動持續時間
				shake_interval = 0.01f,										//震動間隔
				shake_intensity = 0.25f,									//震動強度
				can_be_blocked = false,										//阻擋效果
				scale_start = 0.005f,										//開始尺寸
				scale_target = 0.025f,										//最終尺寸
				trail_effect_scale = 0.1f,									//尾跡效應大小
				trail_effect_timer = 0.1f,									//尾跡效應時間
				trail_effect_id = "fx_plasma_trail",						//尾跡效應ID
				end_effect = string.Empty,
				sound_launch = "event:/SFX/WEAPONS/WeaponFireballStart",	//聲音發射
				sound_impact = "event:/SFX/WEAPONS/WeaponFireballLand",		//聲音影響
			//	impact_actions = new AttackAction(delegate(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
			//	{
			//		bool result1 = NewProjectilesActions.Anti_OtherRaces(pSelf, pTarget, pTile);
			//		bool result2 = NewProjectilesActions.Anti_Shield(pSelf, pTarget, pTile);
			//		bool result3 = NewProjectilesActions.Blade_White02Terraform(pTile);
			//		return result1 || result2 || result3;
			//	})
			});
			//子彈設定 傲慢 黑球		BladeBlack02
			AssetManager.projectiles.add(new ProjectileAsset{// true / false
				id = "BladeBlack02",									//子彈ID
				speed = 80f,												//子彈速度
				texture = "BladeBlack02",								//子彈材質 資料夾位置
				trail_effect_enabled = false,								//尾跡效果
				terraform_option = "Blade_Black02",						//地形改造設定 銜接到 NewTerraformOptions 文件中的 ID
				terraform_range = 10,										//地形範圍
				draw_light_area = true,										//繪製光區
				draw_light_size = 0.25f,									//繪製燈光大小
				trigger_on_collision = true,								//觸發碰撞
				look_at_target = true,										//鎖定目標
				hit_shake = false,											//擊中震動
				shake_duration = 0.01f,										//震動持續時間
				shake_interval = 0.01f,										//震動間隔
				shake_intensity = 0.25f,									//震動強度
				can_be_blocked = false,										//阻擋效果
				scale_start = 0.005f,										//開始尺寸
				scale_target = 0.025f,										//最終尺寸
				trail_effect_scale = 0.1f,									//尾跡效應大小
				trail_effect_timer = 0.1f,									//尾跡效應時間
				trail_effect_id = "fx_plasma_trail",						//尾跡效應ID
				end_effect = string.Empty,
				sound_launch = "event:/SFX/WEAPONS/WeaponFireballStart",			//聲音發射
				sound_impact = "event:/SFX/WEAPONS/WeaponFireballLand",		//聲音影響
			//	impact_actions = new AttackAction(delegate(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
			//	{
			//		bool result1 = NewProjectilesActions.Anti_OtherRaces(pSelf, pTarget, pTile);
			//		bool result2 = NewProjectilesActions.Anti_Shield(pSelf, pTarget, pTile);
			//		bool result3 = NewProjectilesActions.Blade_Black02Terraform(pTile);
			//		return result1 || result2 || result3;
			//	})
			});
			//子彈設定 傲慢 白劍		BladeWhite03
			AssetManager.projectiles.add(new ProjectileAsset{// true / false
				id = "BladeWhite03",									//子彈ID
				speed = 80f,												//子彈速度
				texture = "BladeWhite01",								//子彈材質 資料夾位置
				trail_effect_enabled = false,								//尾跡效果
				terraform_option = "Blade_White03",						//地形改造設定 銜接到 NewTerraformOptions 文件中的 ID
				terraform_range = 5,										//地形範圍
				draw_light_area = true,										//繪製光區
				draw_light_size = 0.25f,									//繪製燈光大小
				trigger_on_collision = false,								//觸發碰撞
				look_at_target = true,										//鎖定目標
				can_be_left_on_ground = true,								//可留存於地板
				hit_shake = true,											//擊中震動
				shake_duration = 0.01f,										//震動持續時間
				shake_interval = 0.01f,										//震動間隔
				shake_intensity = 0.25f,									//震動強度
				can_be_blocked = true,										//阻擋效果
				scale_start = 0.005f,										//開始尺寸
				scale_target = 0.005f,										//最終尺寸
				trail_effect_scale = 0.1f,									//尾跡效應大小
				trail_effect_timer = 0.1f,									//尾跡效應時間
				trail_effect_id = "fx_plasma_trail",						//尾跡效應ID
				end_effect = string.Empty,
				sound_launch = "event:/SFX/HIT/HitSwordSword",		//聲音-發射
				sound_impact = "event:/SFX/HIT/HitSwordSword",		//聲音-碰撞
			//	impact_actions = new AttackAction(delegate(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
			//	{
			//		bool result1 = NewProjectilesActions.Anti_OtherRaces(pSelf, pTarget, pTile);
			//		bool result2 = NewProjectilesActions.Anti_Shield(pSelf, pTarget, pTile);
			//		bool result3 = NewProjectilesActions.Blade_White03Terraform(pTile);
			//		return result1 || result2 || result3;
			//	})
			});
			//子彈設定 傲慢 黑劍		BladeBlack03
			AssetManager.projectiles.add(new ProjectileAsset{// true / false
				id = "BladeBlack03",									//子彈ID
				speed = 80f,												//子彈速度
				texture = "BladeBlack01",								//子彈材質 資料夾位置
				trail_effect_enabled = false,								//尾跡效果
				terraform_option = "Blade_Black03",						//地形改造設定 銜接到 NewTerraformOptions 文件中的 ID
				terraform_range = 5,										//地形範圍
				draw_light_area = true,										//繪製光區
				draw_light_size = 0.25f,									//繪製燈光大小
				trigger_on_collision = false,								//觸發碰撞
				can_be_left_on_ground = true,								//可留存於地板
				look_at_target = true,										//鎖定目標
				hit_shake = false,											//擊中震動
				shake_duration = 0.01f,										//震動持續時間
				shake_interval = 0.01f,										//震動間隔
				shake_intensity = 0.25f,									//震動強度
				can_be_blocked = true,										//阻擋效果
				scale_start = 0.005f,										//開始尺寸
				scale_target = 0.005f,										//最終尺寸
				trail_effect_scale = 0.1f,									//尾跡效應大小
				trail_effect_timer = 0.1f,									//尾跡效應時間
				trail_effect_id = "fx_plasma_trail",						//尾跡效應ID
				end_effect = string.Empty,
				sound_launch = "event:/SFX/HIT/HitSwordSword",	//聲音發射
				sound_impact = "event:/SFX/HIT/HitSwordSword",		//聲音影響
			//	impact_actions = new AttackAction(delegate(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
			//	{
			//		bool result1 = NewProjectilesActions.Anti_OtherRaces(pSelf, pTarget, pTile);
			//		bool result2 = NewProjectilesActions.Anti_Shield(pSelf, pTarget, pTile);
			//		bool result3 = NewProjectilesActions.Blade_Black03Terraform(pTile);
			//		return result1 || result2 || result3;
			//	})
			});
			#endregion
			#region 強欲
			//子彈設定 強欲 高速彈
			AssetManager.projectiles.add(new ProjectileAsset{// true / false
				id = "HighSpeedBullet",									//子彈ID
				speed = 120f,												//子彈速度
				texture = "shotgun_bullet",								//子彈材質 資料夾位置
				trail_effect_enabled = false,								//尾跡效果
				draw_light_area = true,										//繪製光區
				draw_light_size = 0.05f,									//繪製燈光大小
				trigger_on_collision = false,								//觸發碰撞
				look_at_target = true,										//鎖定目標
				can_be_blocked = true,										//阻擋效果
				scale_start = 0.035f,										//開始尺寸
				scale_target = 0.08f,										//最終尺寸
				sound_launch = "event:/SFX/WEAPONS/WeaponShotgunStart",		//聲音發射
				sound_impact = "event:/SFX/WEAPONS/WeaponShotgunLand",		//聲音影響
				texture_shadow = "shadows/projectiles/shadow_arrow",
			//	impact_actions = new AttackAction(delegate(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
			//	{//單一效果
			//		return NewProjectilesActions.Anti_Poverty(pSelf, pTarget, pTile);
			//	})
			});
			//子彈設定 強欲 高速彈 千速
			AssetManager.projectiles.add(new ProjectileAsset{// true / false
				id = "HighSpeedBullet2",									//子彈ID
				speed = 1000f,												//子彈速度
				texture = "shotgun_bullet",								//子彈材質 資料夾位置
				trail_effect_enabled = false,								//尾跡效果
				draw_light_area = true,										//繪製光區
				draw_light_size = 0.05f,									//繪製燈光大小
				trigger_on_collision = false,								//觸發碰撞
				look_at_target = true,										//鎖定目標
				can_be_blocked = true,										//阻擋效果
				scale_start = 0.035f,										//開始尺寸
				scale_target = 0.08f,										//最終尺寸
				sound_launch = "event:/SFX/WEAPONS/WeaponShotgunStart",		//聲音發射
				sound_impact = "event:/SFX/WEAPONS/WeaponShotgunLand",		//聲音影響
				texture_shadow = "shadows/projectiles/shadow_arrow",
			//	impact_actions = new AttackAction(delegate(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
			//	{//單一效果
			//		return NewProjectilesActions.Anti_Poverty(pSelf, pTarget, pTile);
			//	})
			});
			#endregion
			#region 色慾
			//子彈設定 色欲 高速箭矢
			AssetManager.projectiles.add(new ProjectileAsset{// true / false
				id = "HighSpeedArrow",										//子彈ID
				speed = 120f,												//子彈速度
				texture = "arrow",									//子彈材質 資料夾位置
				draw_light_area = true,										//繪製光區
				draw_light_size = 0.05f,									//繪製燈光大小
				trigger_on_collision = true,								//觸發碰撞
				look_at_target = true,										//鎖定目標
				can_be_blocked = true,										//阻擋效果
				can_be_left_on_ground = true,								//可留存於地板
				sound_launch = "event:/SFX/WEAPONS/WeaponStartArrow",		//聲音發射
				sound_impact = "event:/SFX/HIT/HitGeneric",		//聲音影響
				texture_shadow = "shadows/projectiles/shadow_arrow",
			});
			//子彈設定 色欲 高速箭矢
			AssetManager.projectiles.add(new ProjectileAsset{// true / false
				id = "HighSpeedArrow1",										//子彈ID
				speed = 250f,												//子彈速度
				texture = "arrow",									//子彈材質 資料夾位置
				draw_light_area = true,										//繪製光區
				draw_light_size = 0.05f,									//繪製燈光大小
				trigger_on_collision = true,								//觸發碰撞
				look_at_target = true,										//鎖定目標
				can_be_blocked = true,										//阻擋效果
				can_be_left_on_ground = true,								//可留存於地板
				sound_launch = "event:/SFX/WEAPONS/WeaponStartArrow",		//聲音發射
				sound_impact = "event:/SFX/HIT/HitGeneric",		//聲音影響
				texture_shadow = "shadows/projectiles/shadow_arrow",
			});
			//子彈設定 色欲 高速箭矢2
			AssetManager.projectiles.add(new ProjectileAsset{// true / false
				id = "HighSpeedArrow2",										//子彈ID
				speed = 500f,												//子彈速度
				texture = "arrow",									//子彈材質 資料夾位置
				draw_light_area = true,										//繪製光區
				draw_light_size = 0.05f,									//繪製燈光大小
				trigger_on_collision = true,								//觸發碰撞
				look_at_target = true,										//鎖定目標
				can_be_blocked = true,										//阻擋效果
				can_be_left_on_ground = true,								//可留存於地板
				sound_launch = "event:/SFX/WEAPONS/WeaponStartArrow",		//聲音發射
				sound_impact = "event:/SFX/HIT/HitGeneric",		//聲音影響
				texture_shadow = "shadows/projectiles/shadow_arrow",
			});
			#endregion
			#region 憤怒
			//子彈設定 憤怒 高速火球
			AssetManager.projectiles.add(new ProjectileAsset{// true / false
				id = "HighFireball",										//子彈ID
				speed = 120f,												//子彈速度
				texture = "fireball",									//子彈材質 資料夾位置
				trail_effect_enabled = true,								//尾跡效果
				terraform_option = "High_Fireball",
				terraform_range = 2,										//地形範圍
				trigger_on_collision = true,
				draw_light_area = true,										//繪製光區
				draw_light_size = 0.1f,									//繪製燈光大小
				scale_start = 0.035f,										//開始尺寸
				scale_target = 0.100f,										//最終尺寸
				look_at_target = true,										//鎖定目標
				can_be_blocked = true,										//阻擋效果
				can_be_left_on_ground = false,								//阻擋效果
				sound_launch = "event:/SFX/WEAPONS/WeaponFireballStart",		//聲音發射
				sound_impact = "event:/SFX/WEAPONS/WeaponFireballLand",		//聲音影響
				texture_shadow = "shadows/projectiles/shadow_ball",
				end_effect = "fx_fireball_explosion",
			//	impact_actions = new AttackAction(delegate(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
			//	{//多重效果
			//		bool result1 = NewProjectilesActions.Anti_Angry(pSelf, pTarget, pTile);
			//		bool result2 = NewProjectilesActions.Anti_Invincible(pSelf, pTarget, pTile);
			//		bool result3 = NewProjectilesActions.High_FireballTerraform(pTile);
			//		return result1 || result2 || result3;
			//	})
			});
			//子彈設定 憤怒 高速小火球
			AssetManager.projectiles.add(new ProjectileAsset{// true / false
				id = "HighRedOrbl",										//子彈ID
				speed = 120f,												//子彈速度
				texture = "pr_red_orb",									//子彈材質 資料夾位置
				terraform_option = "High_RedOrbl",
				terraform_range = 2,										//地形範圍
				trigger_on_collision = true,
				draw_light_area = true,										//繪製光區
				draw_light_size = 0.1f,									//繪製燈光大小
				scale_start = 0.035f,										//開始尺寸
				scale_target = 0.070f,										//最終尺寸
				look_at_target = true,										//鎖定目標
				can_be_blocked = true,										//阻擋效果
				can_be_left_on_ground = false,								//阻擋效果
				sound_launch = "event:/SFX/WEAPONS/WeaponRedOrbStart",		//聲音發射
				sound_impact = "event:/SFX/WEAPONS/WeaponRedOrbLand",		//聲音影響
				texture_shadow = "shadows/projectiles/shadow_ball",
			//	impact_actions = new AttackAction(delegate(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
			//	{//多重效果
			//		bool result1 = NewProjectilesActions.Anti_Angry(pSelf, pTarget, pTile);
			//		bool result2 = NewProjectilesActions.Anti_Invincible(pSelf, pTarget, pTile);
			//		bool result3 = NewProjectilesActions.High_RedOrblTerraform(pTile);
			//		return result1 || result2 || result3;
			//	})
			});
			//子彈設定 憤怒 太陽1
			AssetManager.projectiles.add(new ProjectileAsset{// true / false
				id = "NuclearFusion1",										//子彈ID
				speed = 5f,												//子彈速度
				texture = "NuclearFusion",									//子彈材質 資料夾位置
				trail_effect_enabled = true,								//尾跡效果
				terraform_option = "NuclearFusion_to",
				terraform_range = 5,										//地形範圍
				trigger_on_collision = true,
				draw_light_area = true,										//繪製光區
				draw_light_size = 0.1f,									//繪製燈光大小
				scale_start = 0.035f,										//開始尺寸
				scale_target = 3.50f,										//最終尺寸
				look_at_target = true,										//鎖定目標
				hit_shake = true,											//擊中震動
				shake_duration = 1.00f,										//震動持續時間
				shake_interval = 5.00f,										//震動間隔
				shake_intensity = 5.25f,									//震動強度
				can_be_blocked = true,										//阻擋效果
				can_be_left_on_ground = false,								//阻擋效果
				sound_launch = "event:/SFX/WEAPONS/WeaponFireballStart",		//聲音發射
				sound_impact = "event:/SFX/WEAPONS/WeaponFireballLand",		//聲音影響
				texture_shadow = "shadows/projectiles/shadow_ball",
				end_effect = "fx_fireball_explosion",
			//	impact_actions = new AttackAction(delegate(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
			//	{//多重效果
			//		bool result1 = NewProjectilesActions.Anti_Angry(pSelf, pTarget, pTile);
			//		bool result2 = NewProjectilesActions.Anti_Invincible(pSelf, pTarget, pTile);
			//		bool result3 = NewProjectilesActions.NuclearFusionTerraform(pTile);
			//		return result1 || result2 || result3;
			//	})
			});
			//子彈設定 憤怒 太陽2
			AssetManager.projectiles.add(new ProjectileAsset{// true / false
				id = "NuclearFusion2",										//子彈ID
				speed = 4f,												//子彈速度
				texture = "NuclearFusion",									//子彈材質 資料夾位置
				trail_effect_enabled = true,								//尾跡效果
				terraform_option = "NuclearFusion_to",
				terraform_range = 10,										//地形範圍
				trigger_on_collision = true,
				draw_light_area = true,										//繪製光區
				draw_light_size = 0.1f,									//繪製燈光大小
				scale_start = 0.035f,										//開始尺寸
				scale_target = 3.50f,										//最終尺寸
				look_at_target = true,										//鎖定目標
				hit_shake = true,											//擊中震動
				shake_duration = 1.00f,										//震動持續時間
				shake_interval = 5.00f,										//震動間隔
				shake_intensity = 5.25f,									//震動強度
				can_be_blocked = true,										//阻擋效果
				can_be_left_on_ground = false,								//阻擋效果
				sound_launch = "event:/SFX/WEAPONS/WeaponFireballStart",		//聲音發射
				sound_impact = "event:/SFX/WEAPONS/WeaponFireballLand",		//聲音影響
				texture_shadow = "shadows/projectiles/shadow_ball",
				end_effect = "fx_fireball_explosion",
			//	impact_actions = new AttackAction(delegate(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
			//	{//多重效果
			//		bool result1 = NewProjectilesActions.Anti_Angry(pSelf, pTarget, pTile);
			//		bool result2 = NewProjectilesActions.Anti_Invincible(pSelf, pTarget, pTile);
			//		bool result3 = NewProjectilesActions.NuclearFusionTerraform(pTile);
			//		return result1 || result2 || result3;
			//	})
			});
			//子彈設定 憤怒 太陽3
			AssetManager.projectiles.add(new ProjectileAsset{// true / false
				id = "NuclearFusion3",										//子彈ID
				speed = 3f,												//子彈速度
				texture = "NuclearFusion",									//子彈材質 資料夾位置
				trail_effect_enabled = true,								//尾跡效果
				terraform_option = "NuclearFusion_to",
				terraform_range = 15,										//地形範圍
				trigger_on_collision = true,
				draw_light_area = true,										//繪製光區
				draw_light_size = 0.1f,									//繪製燈光大小
				scale_start = 0.035f,										//開始尺寸
				scale_target = 3.50f,										//最終尺寸
				look_at_target = true,										//鎖定目標
				hit_shake = true,											//擊中震動
				shake_duration = 1.00f,										//震動持續時間
				shake_interval = 5.00f,										//震動間隔
				shake_intensity = 5.25f,									//震動強度
				can_be_blocked = true,										//阻擋效果
				can_be_left_on_ground = false,								//阻擋效果
				sound_launch = "event:/SFX/WEAPONS/WeaponFireballStart",		//聲音發射
				sound_impact = "event:/SFX/WEAPONS/WeaponFireballLand",		//聲音影響
				texture_shadow = "shadows/projectiles/shadow_ball",
				end_effect = "fx_fireball_explosion",
			//	impact_actions = new AttackAction(delegate(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
			//	{//多重效果
			//		bool result1 = NewProjectilesActions.Anti_Angry(pSelf, pTarget, pTile);
			//		bool result2 = NewProjectilesActions.Anti_Invincible(pSelf, pTarget, pTile);
			//		bool result3 = NewProjectilesActions.NuclearFusionTerraform(pTile);
			//		return result1 || result2 || result3;
			//	})
			});
			//子彈設定 憤怒 高速火球
			AssetManager.projectiles.add(new ProjectileAsset{// true / false
				id = "HighFireball_2",										//子彈ID
				speed = 120f,												//子彈速度
				texture = "fireball",									//子彈材質 資料夾位置
				trail_effect_enabled = true,								//尾跡效果
				terraform_option = "High_Fireball_2",
				terraform_range = 5,										//地形範圍
				trigger_on_collision = true,
				draw_light_area = true,										//繪製光區
				draw_light_size = 0.1f,									//繪製燈光大小
				scale_start = 0.035f,										//開始尺寸
				scale_target = 0.070f,										//最終尺寸
				look_at_target = true,										//鎖定目標
				can_be_blocked = true,										//阻擋效果
				can_be_left_on_ground = false,								//阻擋效果
				sound_launch = "event:/SFX/WEAPONS/WeaponFireballStart",		//聲音發射
				sound_impact = "event:/SFX/WEAPONS/WeaponFireballLand",		//聲音影響
				texture_shadow = "shadows/projectiles/shadow_ball",
				end_effect = "fx_fireball_explosion",
			//	impact_actions = new AttackAction(delegate(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
			//	{//多重效果
			//		bool result1 = NewProjectilesActions.Anti_Angry(pSelf, pTarget, pTile);
			//		bool result2 = NewProjectilesActions.Anti_Invincible(pSelf, pTarget, pTile);
			//		bool result3 = NewProjectilesActions.High_Fireball_2Terraform(pTile);
			//		return result1 || result2 || result3;
			//	})
			});
			#endregion
			#region 暴食
			//子彈設定 暴食 高速餐叉
			AssetManager.projectiles.add(new ProjectileAsset{// true / false
				id = "Fork",												//子彈ID
				speed = 100f,												//子彈速度
				texture = "GluttonyFork",											//子彈材質 資料夾位置
				terraform_option = "Tableware1",						//地形改造設定 銜接到 NewTerraformOptions 文件中的 ID
				terraform_range = 2,										//地形範圍
				trail_effect_enabled = false,								//尾跡效果
				draw_light_area = true,										//繪製光區
				draw_light_size = 0.25f,									//繪製燈光大小
				scale_start = 0.005f,										//開始尺寸
				scale_target = 0.015f,										//最終尺寸
				trigger_on_collision = false,								//觸發碰撞
				look_at_target = true,										//鎖定目標
				can_be_blocked = true,										//阻擋效果
				can_be_left_on_ground = true,								//阻擋效果
				sound_launch = "event:/SFX/HIT/HitSwordSword",		//聲音發射
				sound_impact = "event:/SFX/HIT/HitSwordSword",		//聲音影響
				texture_shadow = "shadows/projectiles/shadow_arrow",
			//	impact_actions = new AttackAction(delegate(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
			//	{//多重效果
			//		bool result1 = NewProjectilesActions.Anti_Hungry(pSelf, pTarget, pTile);
			//		bool result2 = NewProjectilesActions.Tableware1Terraform(pTile);
			//		return result1 || result2;
			//	})
			});
			//子彈設定 暴食 高速餐刀
			AssetManager.projectiles.add(new ProjectileAsset{// true / false
				id = "Knife",												//子彈ID
				speed = 95f,												//子彈速度
				texture = "GluttonyKnife",									//子彈材質 資料夾位置
				terraform_option = "Tableware2",						//地形改造設定 銜接到 NewTerraformOptions 文件中的 ID
				terraform_range = 2,										//地形範圍
				trail_effect_enabled = false,								//尾跡效果
				draw_light_area = true,										//繪製光區
				draw_light_size = 0.25f,									//繪製燈光大小
				scale_start = 0.005f,										//開始尺寸
				scale_target = 0.015f,										//最終尺寸
				trigger_on_collision = false,								//觸發碰撞
				look_at_target = true,										//鎖定目標
				can_be_blocked = true,										//阻擋效果
				can_be_left_on_ground = true,								//阻擋效果
				sound_launch = "event:/SFX/HIT/HitSwordSword",		//聲音發射
				sound_impact = "event:/SFX/HIT/HitSwordSword",		//聲音影響
				texture_shadow = "shadows/projectiles/shadow_arrow",
			//	impact_actions = new AttackAction(delegate(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
			//	{//多重效果
			//		bool result1 = NewProjectilesActions.Anti_Hungry(pSelf, pTarget, pTile);
			//		bool result2 = NewProjectilesActions.Tableware2Terraform(pTile);
			//		return result1 || result2;
			//	})
			});
			//子彈設定 暴食 高速酸彈
			AssetManager.projectiles.add(new ProjectileAsset{// true / false
				id = "HighAcidBall",										//子彈ID
				speed = 120f,												//子彈速度
				texture = "acid_ball",									//子彈材質 資料夾位置
				trail_effect_enabled = true,								//尾跡效果
				terraform_option = "High_AcidBall",
				terraform_range = 4,										//地形範圍
				trigger_on_collision = true,								//觸發碰撞
				scale_start = 0.1f,											//開始尺寸
				scale_target = 0.5f,										//最終尺寸
				sound_launch =  "event:/SFX/DROPS/DropAcid",				//聲音發射
				sound_impact = "event:/SFX/WEAPONS/WeaponFireballLand",		//聲音影響
				texture_shadow = "shadows/projectiles/shadow_ball",
				end_effect = "fx_cast_top_green",
			//	impact_actions = new AttackAction(delegate(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
			//	{//多重效果
			//		bool result1 = NewProjectilesActions.Anti_Hungry(pSelf, pTarget, pTile);
			//		bool result2 = NewProjectilesActions.High_AcidBallTerraform(pTile);
			//		return result1 || result2;
			//	})
			});
			#endregion
			#region 怠惰
			//子彈設定 怠惰 雪崩彈
			AssetManager.projectiles.add(new ProjectileAsset{// true / false
				id = "Avalanche",										//子彈ID
				speed = 3f,												//子彈速度
				texture = "Avalanche",									//子彈材質 資料夾位置
				trail_effect_enabled = false,								//尾跡效果
				terraform_option = "Avalanche_to",
				terraform_range = 15,										//地形範圍
				trigger_on_collision = true,
                hit_freeze = true,											//擊中凍結
				draw_light_area = true,										//繪製光區
				draw_light_size = 0.1f,									//繪製燈光大小
				scale_start = 0.035f,										//開始尺寸
				scale_target = 3.50f,										//最終尺寸
				look_at_target = true,										//鎖定目標
				hit_shake = true,											//擊中震動
				shake_duration = 1.00f,										//震動持續時間
				shake_interval = 5.00f,										//震動間隔
				shake_intensity = 5.25f,									//震動強度
				can_be_blocked = true,										//阻擋效果
				can_be_left_on_ground = false,								//阻擋效果
				sound_launch = "event:/SFX/WEAPONS/WeaponFireballStart",		//聲音發射
				sound_impact = "event:/SFX/WEAPONS/WeaponFireballLand",		//聲音影響
				texture_shadow = "shadows/projectiles/shadow_ball",
				end_effect = string.Empty,
			//	impact_actions = new AttackAction(delegate(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
			//	{//多重效果
			//		bool result1 = NewProjectilesActions.Anti_NoSleeping(pSelf, pTarget, pTile);
			//		bool result2 = NewProjectilesActions.AvalancheTerraform(pTile);
			//		return result1 || result2;
			//	})
			});
			//子彈設定 怠惰 冰槍
			AssetManager.projectiles.add(new ProjectileAsset{// true / false
				id = "IcePick",										//子彈ID
				speed = 110f,												//子彈速度
				texture = "IcePick",										//子彈材質 資料夾位置
				trail_effect_enabled = false,								//尾跡效果
				terraform_option = "IcePick_to",
				terraform_range = 0,										//地形範圍
				trigger_on_collision = true,
                hit_freeze = true,											//擊中凍結
				draw_light_area = true,										//繪製光區
				draw_light_size = 0.01f,									//繪製燈光大小
				scale_start = 0.003f,										//開始尺寸
				scale_target = 0.035f,										//最終尺寸
				look_at_target = true,										//鎖定目標
				hit_shake = false,											//擊中震動
				shake_duration = 1.00f,										//震動持續時間
				shake_interval = 5.00f,										//震動間隔
				shake_intensity = 5.25f,									//震動強度
				can_be_blocked = true,										//阻擋效果
				can_be_left_on_ground = false,								//阻擋效果
				sound_launch = "event:/SFX/WEAPONS/WeaponFreezeOrbStart",		//聲音發射
				sound_impact = "event:/SFX/WEAPONS/WeaponFreezeOrbLand",		//聲音影響
				end_effect = string.Empty,
			//	impact_actions = new AttackAction(delegate(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
			//	{//多重效果
			//		bool result1 = NewProjectilesActions.Anti_NoSleeping(pSelf, pTarget, pTile);
			//		bool result2 = NewProjectilesActions.IcePickTerraform(pTile);
			//		return result1 || result2;
			//	})
			});
			//子彈設定 怠惰 雪花彈
			AssetManager.projectiles.add(new ProjectileAsset{// true / false
				id = "Snowflake",										//子彈ID
				speed = 90f,												//子彈速度
				texture = "Snowflake",									//子彈材質 資料夾位置
				trail_effect_enabled = false,								//尾跡效果
				terraform_option = "Snowflake_to",
				terraform_range = 0,										//地形範圍
				trigger_on_collision = true,
                hit_freeze = true,											//擊中凍結
				draw_light_area = true,										//繪製光區
				draw_light_size = 0.01f,									//繪製燈光大小
				scale_start = 0.003f,										//開始尺寸
				scale_target = 0.035f,										//最終尺寸
				look_at_target = true,										//鎖定目標
				hit_shake = false,											//擊中震動
				shake_duration = 1.00f,										//震動持續時間
				shake_interval = 5.00f,										//震動間隔
				shake_intensity = 5.25f,									//震動強度
				can_be_blocked = true,										//阻擋效果
				can_be_left_on_ground = false,								//阻擋效果
				sound_launch = "event:/SFX/WEAPONS/WeaponFreezeOrbStart",		//聲音發射
				sound_impact = "event:/SFX/WEAPONS/WeaponFreezeOrbLand",		//聲音影響
				end_effect = string.Empty,
			//	impact_actions = new AttackAction(delegate(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
			//	{//多重效果
			//		bool result1 = NewProjectilesActions.Anti_NoSleeping(pSelf, pTarget, pTile);
			//		bool result2 = NewProjectilesActions.SnowflakeTerraform(pTile);
			//		return result1 || result2;
			//	})
			});
			#endregion
			#region 裁決
			//子彈設定 藍劍 柄部
			AssetManager.projectiles.add(new ProjectileAsset{// true / false
				id = "Justice_01",										//子彈ID
				speed = 120f,												//子彈速度
				texture = "Justice01",										//子彈材質 資料夾位置
				trail_effect_enabled = true,								//尾跡效果
				terraform_option = "Justice_01to",
				terraform_range = 1,										//地形範圍
				trigger_on_collision = true,								//觸發碰撞
				draw_light_area = true,										//繪製光區
				draw_light_size = 0.1f,										//繪製燈光大小
				scale_start = 0.030f,										//開始尺寸
				scale_target = 0.030f,										//最終尺寸
				look_at_target = true,										//鎖定目標
				hit_shake = false,											//擊中震動
				shake_duration = 1.00f,										//震動持續時間
				shake_interval = 5.00f,										//震動間隔
				shake_intensity = 5.25f,									//震動強度
				can_be_blocked = true,										//阻擋效果
				can_be_left_on_ground = false,								//可留存於地板
				sound_launch = "event:/SFX/HIT/HitSwordSword",				//聲音發射
				sound_impact = "event:/SFX/HIT/HitSwordSword",				//聲音影響
				end_effect = "fx_fireball_explosion",
			//	impact_actions = new AttackAction(delegate(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
			//	{//多重效果
			//		bool result1 = NewProjectilesActions.Anti_NoSleeping(pSelf, pTarget, pTile);
			//		bool result2 = NewProjectilesActions.AvalancheTerraform(pTile);
			//		return result1 || result2;
			//	})
			});
			//子彈設定 藍劍 刃部
			AssetManager.projectiles.add(new ProjectileAsset{// true / false
				id = "Justice_02",										//子彈ID
				speed = 120f,												//子彈速度
				texture = "Justice02",										//子彈材質 資料夾位置
				trail_effect_enabled = false,								//尾跡效果
				terraform_option = "Justice_02to",
				terraform_range = 1,										//地形範圍
				trigger_on_collision = true,								//觸發碰撞
				draw_light_area = true,										//繪製光區
				draw_light_size = 0.1f,										//繪製燈光大小
				scale_start = 0.030f,										//開始尺寸
				scale_target = 0.030f,										//最終尺寸
				look_at_target = true,										//鎖定目標
				hit_shake = false,											//擊中震動
				shake_duration = 1.00f,										//震動持續時間
				shake_interval = 5.00f,										//震動間隔
				shake_intensity = 5.25f,									//震動強度
				can_be_blocked = true,										//阻擋效果
				can_be_left_on_ground = false,								//可留存於地板
				sound_launch = "event:/SFX/HIT/HitSwordSword",				//聲音發射
				sound_impact = "event:/SFX/HIT/HitSwordSword",				//聲音影響
				end_effect = string.Empty,
			//	impact_actions = new AttackAction(delegate(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
			//	{//多重效果
			//		bool result1 = NewProjectilesActions.Anti_NoSleeping(pSelf, pTarget, pTile);
			//		bool result2 = NewProjectilesActions.AvalancheTerraform(pTile);
			//		return result1 || result2;
			//	})
			});
			//子彈設定 白槍 柄部
			AssetManager.projectiles.add(new ProjectileAsset{// true / false
				id = "Justice_03",										//子彈ID
				speed = 500f,												//子彈速度
				texture = "Justice03",										//子彈材質 資料夾位置
				trail_effect_enabled = false,								//尾跡效果
				terraform_option = "Justice_01to",
				terraform_range = 1,										//地形範圍
				trigger_on_collision = true,								//觸發碰撞
				draw_light_area = true,										//繪製光區
				draw_light_size = 0.1f,										//繪製燈光大小
				scale_start = 0.030f,										//開始尺寸
				scale_target = 0.030f,										//最終尺寸
				look_at_target = true,										//鎖定目標
				hit_shake = false,											//擊中震動
				shake_duration = 1.00f,										//震動持續時間
				shake_interval = 5.00f,										//震動間隔
				shake_intensity = 5.25f,									//震動強度
				can_be_blocked = true,										//阻擋效果
				can_be_left_on_ground = false,								//可留存於地板
				sound_launch = "event:/SFX/HIT/HitSwordSword",				//聲音發射
				sound_impact = "event:/SFX/WEAPONS/WeaponFireballLand",				//聲音影響
				end_effect = string.Empty,
			//	impact_actions = new AttackAction(delegate(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
			//	{//多重效果
			//		bool result1 = NewProjectilesActions.Anti_NoSleeping(pSelf, pTarget, pTile);
			//		bool result2 = NewProjectilesActions.AvalancheTerraform(pTile);
			//		return result1 || result2;
			//	})
			});
			//子彈設定 白槍 刃部
			AssetManager.projectiles.add(new ProjectileAsset{// true / false
				id = "Justice_04",										//子彈ID
				speed = 500f,												//子彈速度
				texture = "Justice04",										//子彈材質 資料夾位置
				trail_effect_enabled = true,								//尾跡效果
				terraform_option = "Justice_02to",
				terraform_range = 1,										//地形範圍
				trigger_on_collision = true,								//觸發碰撞
				draw_light_area = true,										//繪製光區
				draw_light_size = 0.1f,										//繪製燈光大小
				scale_start = 0.030f,										//開始尺寸
				scale_target = 0.030f,										//最終尺寸
				look_at_target = true,										//鎖定目標
				hit_shake = false,											//擊中震動
				shake_duration = 1.00f,										//震動持續時間
				shake_interval = 5.00f,										//震動間隔
				shake_intensity = 5.25f,									//震動強度
				can_be_blocked = true,										//阻擋效果
				can_be_left_on_ground = false,								//可留存於地板
				sound_launch = "event:/SFX/HIT/HitSwordSword",				//聲音發射
				sound_impact = "event:/SFX/WEAPONS/WeaponFireballLand",		//聲音影響
				end_effect = "fx_fireball_explosion",
			//	impact_actions = new AttackAction(delegate(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
			//	{//多重效果
			//		bool result1 = NewProjectilesActions.Anti_NoSleeping(pSelf, pTarget, pTile);
			//		bool result2 = NewProjectilesActions.AvalancheTerraform(pTile);
			//		return result1 || result2;
			//	})
			});
			#endregion
		}
	}
}