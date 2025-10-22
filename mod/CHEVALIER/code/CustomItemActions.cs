using HarmonyLib;
using NCMS.Utils;
using NeoModLoader.api.attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Chevalier.code
{
    internal class CustomItemActions
    {
        public static bool holyLightSwordAttackEffect(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
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

        public static bool shadowBladeAttackEffect(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
                return false;
            if (Randy.randomChance(0.15f))
            {
                //暗影之力，削弱敌人
                ActionLibrary.addSlowEffectOnTarget(pSelf, pTarget, pTile);
                return true;
            }
            return false;
        }

        internal static bool frostSwordAttackEffect(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
                return false;
            if (Randy.randomChance(0.1f))
            {
                //冰霜之力，冻结敌人
                ActionLibrary.addSlowEffectOnTarget(pSelf, pTarget, pTile);
                ActionLibrary.addStunnedEffectOnTarget(pSelf, pTarget, pTile);
                return true;
            }
            return false;
        }

        internal static bool fireScytheAttackEffect(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
                return false;
            if (Randy.randomChance(0.12f))
            {
                //火焰之力，焚烧敌人
                ActionLibrary.castFire(pSelf, pTarget, pTile);
                return true;
            }
            return false;
        }

        internal static bool lifeStaffAttackEffect(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
                return false;
            if (Randy.randomChance(0.08f))
            {
                //生命之力，治愈自身
                ActionLibrary.breakBones(pSelf, pTarget, pTile);
                return true;
            }
            return false;
        }

        internal static bool chaosAxeAttackEffect(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
                return false;
            if (Randy.randomChance(0.07f))
            {
                //混沌之力，混乱敌人
                ActionLibrary.addSlowEffectOnTarget(pSelf, pTarget, pTile);
                return true;
            }
            return false;
        }

        internal static bool lightHammerAttackEffect(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
                return false;
            if (Randy.randomChance(0.1f))
            {
                //光明之力，驱散黑暗
                ActionLibrary.addStunnedEffectOnTarget(pSelf, pTarget, pTile);
                return true;
            }
            return false;
        }

        internal static bool venomDaggerAttackEffect(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
        {
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive())
                return false;
            if (Randy.randomChance(0.15f))
            {
                //剧毒之力，削弱敌人
                ActionLibrary.addSlowEffectOnTarget(pSelf, pTarget, pTile);
                ActionLibrary.addStunnedEffectOnTarget(pSelf, pTarget, pTile);
                return true;
            }
            return false;
        }
    }
}