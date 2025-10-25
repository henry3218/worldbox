using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using NCMS.Utils;

namespace ZeN_01
{
	class TraitsOpposite
	{
		public static void init()
		{
			InitializeOppositeTraits();
		}

		public static void InitializeOppositeTraits()
		{
			#region 對立設置
/*
			//一對一對立設定
			SetTraitPairOpposites("trait01Id", "trait02Id");
			
			//清單對立設定
			string[] XXX_Traits = { "trait01Id", "trait02Id", "trait03Id" };
			SetTraitOpposites("trait04Id", XXX_Traits);
			SetTraitOpposites("trait05Id", XXX_Traits);

			//清單對立設定 群組內特質相互對立
			string[] XXX_Traits = {
				"trait01Id", "trait02Id", "trait03Id", "trait04Id",
			};
			foreach (string traitID in XXX_Traits)
			{
				// 對立關係在同一個群組內
				SetTraitOpposites(traitID, XXX_Traits.Except(new[] { traitID }).ToArray());
			}
*/
			// 職位特質清單 正常
			string[] professionTraits1 = { "pro_soldier", "pro_warrior" };
			SetTraitOpposites("pro_soldier", professionTraits1);
			SetTraitOpposites("pro_warrior", professionTraits1);


			string[] professionTraits2 = { "pro_king", "pro_leader" };
			foreach (string traitID in professionTraits2)
			{
				SetTraitPairOpposites(traitID, "slave");
				SetTraitPairOpposites(traitID, "apostle");
				SetTraitPairOpposites(traitID, "undead_servant");
				SetTraitPairOpposites(traitID, "undead_servant2");
			}

			// 狀態添加 (Skill01xx 系列) 正常
			string[] statusUpTraits = {
				"status_powerup", "status_caffeinated", "status_enchanted", "status_rage",
				"status_spellboost", "status_motivated", "status_shield", "status_invincible",
				"status_afo", "status_ofa", "status_inspired"
			};
			foreach (string traitID in statusUpTraits)
			{
				// 對立關係在同一個群組內
				SetTraitOpposites(traitID, statusUpTraits.Except(new[] { traitID }).ToArray());
			}

			// 子彈特質 (projectilexx 系列) 正常
			string[] projectileTraits = {
				"projectile01", "projectile02", "projectile03", "projectile04", "projectile05",
				"projectile06", "projectile07", "projectile08", "projectile09", "projectile10",
				"projectile11", "projectile12", "projectile13", "projectile14", "projectile15"
			};
			foreach (string traitID in projectileTraits)
			{
				SetTraitOpposites(traitID, projectileTraits.Except(new[] { traitID }).ToArray());
			}
			// 狂彈 >< 瘋狂 
			SetTraitPairOpposites("projectile14", "madness");

			// 附魔攻擊 (Skill02xx 系列) 正常
			string[] addEffectTraits = {
				"add_burning", "add_slowdown", "add_frozen", "add_poisonous", "add_afc",
				"add_silenced", "add_stunned", "add_drowning", "add_confused", "add_unknown", "add_cursed",
				"add_death"
			};
			foreach (string traitID in addEffectTraits)
			{
				SetTraitOpposites(traitID, addEffectTraits.Except(new[] { traitID }).ToArray());
			}

			// 環境魔法 (altered_surface 系列)
			string[] altered_surfaceTraits = {
				"altered_surface01", "altered_surface02", "altered_surface03", "altered_surface04",
				"altered_surface05", "altered_surface06", "altered_surface07", "altered_surface08",
				"altered_surface09", "altered_surface10", "altered_surface11", "altered_surface12",
				"altered_surface13", "altered_surface14", "altered_surface15", "altered_surface16",
				"altered_surface17", "altered_surface18", "altered_surface19", "altered_surface20",
				"altered_surface21", "altered_surface22"
			};
			foreach (string traitID in altered_surfaceTraits)
			{
				// 對立關係在同一個群組內
				SetTraitOpposites(traitID, altered_surfaceTraits.Except(new[] { traitID }).ToArray());
			}

			// 建造魔法 (MonsterNest 系列)
			string[] MonsterNestTraits = {
				"monste_nest001", "monste_nest002", "monste_nest003", "monste_nest004",
				"monste_nest005", "monste_nest006", "monste_nest007", "monste_nest008",
				"monste_nest009", "monste_nest010", "monste_nest011", "monste_nest012",
				"monste_nest013", "monste_nest014"
			};
			foreach (string traitID in MonsterNestTraits)
			{
				// 對立關係在同一個群組內
				SetTraitOpposites(traitID, MonsterNestTraits.Except(new[] { traitID }).ToArray());
			}
			
			SetTraitPairOpposites("madness", "monste_nest008");
			SetTraitPairOpposites("desire_golden_egg", "monste_nest010");
			SetTraitPairOpposites("desire_harp", "monste_nest011");
			SetTraitPairOpposites("desire_alien_mold", "monste_nest012");
			SetTraitPairOpposites("desire_computer", "monste_nest009");

			// 淨化聖體 >< 病災法 (Skill0301, Skill0405)
			SetTraitPairOpposites("holyarts_ha", "evillaw_disease");
			
			string[] holyArts01Opposites = { "eyepatch", "crippled", "skin_burns" };
			SetTraitOpposites("holyarts_ha", holyArts01Opposites);
			SetTraitOpposites("evillaw_disease", holyArts01Opposites);

			// 聖雫 (Skill0302, Skill0303, Skill0304) 正常
			string[] holyArts02Opposites = { "holyarts_heal", "holyarts_cure", "holyarts_healcure" };
			foreach (string traitID in holyArts02Opposites)
			{
				SetTraitOpposites(traitID, holyArts02Opposites.Except(new[] { traitID }).ToArray());
			}

			// 聖餐 >< 餓食法 (Skill0310, Skill0404)
			SetTraitPairOpposites("holyarts_eucharist", "evillaw_starvation");

			// 祝福與聖光特質 >< 詛咒能力 (通用)
			string[] BlessCurses = { "evillaw_tgc", "add_cursed" };
			foreach (string traitID in BlessCurses)
			{
				SetTraitPairOpposites(traitID, "holyarts_consecration");
				SetTraitPairOpposites(traitID, "blessed");
				SetTraitPairOpposites(traitID, "holyarts_divinelight");
				SetTraitPairOpposites(traitID, "monste_nest007");
			}
			// 聖光特質 對立設定
			string[] DivineLight = { "monste_nest006", "zombie", "mush_spores" };
			foreach (string traitID in DivineLight)
			{
				SetTraitPairOpposites(traitID, "holyarts_divinelight");
			}

			// 自動恢復 >< 吸收能力
			string[] holyRecovery = { "holyarts_health", "holyarts_mana", "holyarts_stamina" };
			foreach (string traitID in holyRecovery)
			{
				SetTraitPairOpposites(traitID, "evillaw_ea");
			}

			// 誕生 >< 絕育 
			SetTraitPairOpposites("holyarts_annunciation", "evillaw_sterilization");

			// 忿怒 >< 寧靜
			SetTraitPairOpposites("holyarts_serenity", "evillaw_tantrum");

			// 吞噬 >< ??
			SetTraitPairOpposites("evillaw_devour", "add_unknown");

			// 智慧 >< 失智
			SetTraitPairOpposites("evillaw_ew", "mutation");

			// 色慾 >< 絆
			string[] lustBondTraits = { "slave", "evillaw_seduction", "holyarts_bond" };
			foreach (string traitID in lustBondTraits)
			{
				SetTraitOpposites(traitID, lustBondTraits.Except(new[] { traitID }).ToArray());
			}

			// 大罪特質 奴隸 使徒  >< 精神異常特質
			string[] sin_Traits = { "slave", "apostle", "evillaw_ew", "evillaw_seduction", "evillaw_moneylaw", "evillaw_starvation", "evillaw_sleeping", "evillaw_devour", "extraordinary_authority","holyarts_divinelight" };
			foreach (string traitID in sin_Traits)
			{
				SetTraitPairOpposites(traitID, "madness");
				SetTraitPairOpposites(traitID, "desire_harp");
				SetTraitPairOpposites(traitID, "desire_alien_mold");
				SetTraitPairOpposites(traitID, "desire_computer");
				SetTraitPairOpposites(traitID, "desire_golden_egg");
			}


			// 大罪特質 憤怒 >< 精神異常特質
			string[] evillaw_tantrum = { "desire_harp", "desire_alien_mold", "desire_computer", "desire_golden_egg" };
			foreach (string traitID in evillaw_tantrum)
			{
				SetTraitPairOpposites(traitID, "evillaw_tantrum");
			}

			// 邪咒法系列 >< 懲戒
			string[] evillaw_Traits = { 
			"evillaw_tgc", "evillaw_devour", "evillaw_tc", "evillaw_starvation", "evillaw_disease", "evillaw_moneylaw",
			"evillaw_ea", "evillaw_sleeping", "evillaw_sterilization", "evillaw_tantrum", "evillaw_seduction",
			"evillaw_ew", "slave", "apostle", "undead_servant", "undead_servant2", "evil", "monste_nest001", "monste_nest002",
			"monste_nest003", "monste_nest004", "monste_nest005", "monste_nest006", "monste_nest008",
			"monste_nest009", "monste_nest010", "monste_nest011", "monste_nest012", "monste_nest013",
			"monste_nest014", "add_cursed"};
			foreach (string traitID in evillaw_Traits)
			{
				SetTraitPairOpposites(traitID, "holyarts_justice");
			}

			// 7大罪
			string[] sin70_Traits = { "evillaw_ew", "evillaw_seduction", "evillaw_moneylaw", "evillaw_starvation", "evillaw_sleeping", "evillaw_tantrum", "evillaw_devour" };
			foreach (string traitID in sin70_Traits)
			{
				SetTraitOpposites(traitID, sin70_Traits.Except(new[] { traitID }).ToArray());
			}


			string[] sin71_Traits = { "evillaw_ew", "evillaw_seduction", "evillaw_moneylaw", "evillaw_starvation", "evillaw_sleeping", "evillaw_devour" };
			foreach (string traitID in sin71_Traits)
			{
				SetTraitPairOpposites(traitID, "slave");
			}

			string[] sin72_Traits = { "evillaw_ew", "evillaw_seduction", "evillaw_moneylaw", "evillaw_starvation", "evillaw_sleeping", "evillaw_tantrum", "evillaw_devour" };
			foreach (string traitID in sin72_Traits)
			{
				SetTraitPairOpposites(traitID, "apostle");
				//SetTraitPairOpposites(traitID, "undead_servant");
				//SetTraitPairOpposites(traitID, "undead_servant2");
			}

			SetTraitPairOpposites("apostle", "b0002");
			#endregion
		}

		private static void SetTraitOpposites(string targetTraitId, params string[] opposingTraitIds)
		{
			ActorTrait targetTrait = AssetManager.traits.get(targetTraitId);
			if (targetTrait == null)
			{
				Debug.LogError($"[TraitsOpposite] 無法獲取目標特質 '{targetTraitId}' 實例，無法設置對立特質。");
				return;
			}
			if (targetTrait.opposite_traits == null)
			{
				targetTrait.opposite_traits = new HashSet<ActorTrait>();
			}
			foreach (string opposingTraitId in opposingTraitIds)
			{
				if (string.IsNullOrEmpty(opposingTraitId))
				{
					continue;
				}
				ActorTrait opposingTrait = AssetManager.traits.get(opposingTraitId);
				if (opposingTrait != null)
				{
					targetTrait.opposite_traits.Add(opposingTrait);
				}
				else
				{
					Debug.LogWarning($"[TraitsOpposite] 為 '{targetTraitId}' 設置對立特質時，未能找到對立特質 '{opposingTraitId}'。");
				}
			}
		}

		/// <summary>
		/// 輔助方法：為兩個特質建立雙向對立關係。
		/// </summary>
		private static void SetTraitPairOpposites(string trait1Id, string trait2Id)
		{
			SetTraitOpposites(trait1Id, trait2Id);
			SetTraitOpposites(trait2Id, trait1Id);
		}
	}
}