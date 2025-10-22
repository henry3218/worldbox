using HarmonyLib;
using UnityEngine;
using NeoModLoader.api.attributes;
using ai;
using ReflectionUtility;
using System.Collections.Generic;

namespace ZeN_01
{
    [HarmonyPatch(typeof(CombatActionLibrary), "init")]
    public class CombatAction_Patch
    {
        private static void Postfix(CombatActionLibrary __instance)
        {   // 在這裡添加 Debug.Log 來確認程式碼有執行
            Debug.Log("ZeN_01 Mod: 正在註冊戰技！");
            CombatActionAsset myNewDeflectAction = new CombatActionAsset
            {
                id = "combat_deflect_test0001",
                cost_stamina = 1,
                chance = 0.99f,
                
                // === 关键修正：确保 pools 被正确初始化并赋值 ===
                pools = new CombatActionPool[] { CombatActionPool.BEFORE_HIT },
                // ===================================

                action_actor = new CombatActionActor(doDeflect_new)
            };

            __instance.add(myNewDeflectAction);
        }

        private static bool doDeflect_new(Actor pActor, AttackData pData, float pTargetX = 0f, float pTargetY = 0f)
        {
            // === 修正：加入日志输出，以便调试 ===
            Debug.Log("偏轉戰技已發動！");
            // ===================================
            
            // 以下是原版逻辑
            Vector2 tOldStartPos = pData.initiator_position;
            pActor.spawnSlashPunch(tOldStartPos);
            pActor.stopMovement();
            pActor.punchTargetAnimation(tOldStartPos, true, pActor.hasRangeAttack(), 40f);
            pActor.startAttackCooldown();
            
            // 确保投射物被取消
            //if (pData.type == AttackType.Projectile)
            //{
            //    pData.set_cancelled();
            //}
            return true;
        }
    }
}