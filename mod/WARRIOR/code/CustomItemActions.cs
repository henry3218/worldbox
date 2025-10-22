using HarmonyLib;
using NCMS.Utils;
using NeoModLoader.api.attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PeerlessOverpoweringWarrior.code
{
    internal class CustomItemActions
    {
        public static bool fenTianJianAttackEffect(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
                return false;
            if (Randy.randomChance(0.1f))
            {
                //火神之力，焚尽万物
                ActionLibrary.castFire(pSelf, pTarget, pTile);
                return true;
            }
            return false;
        }

        public static bool lieDiDaoAttackEffect(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
                return false;
            if (Randy.randomChance(0.1f))
            {
                //裂地之力，重如泰山
                ActionLibrary.addSlowEffectOnTarget(pSelf, pTarget, pTile);
                return true;
            }
            return false;
        }

        internal static bool tunHaiQiangAttackEffect(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
                return false;
            if (Randy.randomChance(0.1f))
            {
                //吞海之力，束缚敌人
                ActionLibrary.addSlowEffectOnTarget(pSelf, pTarget, pTile);
                ActionLibrary.addStunnedEffectOnTarget(pSelf, pTarget, pTile);
                return true;
            }
            return false;
        }

        internal static bool xuanYuanJianAttackEffect(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
                return false;
            if (Randy.randomChance(0.1f))
            {
                //圣道之剑，断筋裂骨
                ActionLibrary.breakBones(pSelf, pTarget, pTile);
                return true;
            }
            return false;
        }
    }
}