using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ai;
using ai.behaviours;
using UnityEngine;
using ChivalryZhanXun.code;

namespace ChivalryZhanXun.code
{
    internal class traitAction
    {
        public static bool ZhanXun2_effectAction(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget.a.data.health < pTarget.a.getMaxHealth())
            {
                pTarget.a.restoreHealth(1);
                pTarget.a.spawnParticle(Toolbox.color_heal);
            }

            return true;
        }

        public static bool ZhanXun3_effectAction(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget.a.data.health < pTarget.a.getMaxHealth())
            {
                pTarget.a.restoreHealth(2);
                pTarget.a.spawnParticle(Toolbox.color_heal);
            }
            return true;
        }

        public static bool ZhanXun4_effectAction(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget.a.data.health < pTarget.a.getMaxHealth())
            {
                pTarget.a.restoreHealth(3);
                pTarget.a.spawnParticle(Toolbox.color_heal);
            }
            return true;
        }

        public static bool ZhanXun5_effectAction(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget.a.data.health < pTarget.a.getMaxHealth())
            {
                pTarget.a.restoreHealth(5);
                pTarget.a.spawnParticle(Toolbox.color_heal);
            }
            return true;
        }

        public static bool ZhanXun6_effectAction(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget.a.data.health < pTarget.a.getMaxHealth())
            {
                pTarget.a.restoreHealth(7);
                pTarget.a.spawnParticle(Toolbox.color_heal);
            }
            return true;
        }

        public static bool ZhanXun7_effectAction(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget.a.data.health < pTarget.a.getMaxHealth())
            {
                pTarget.a.restoreHealth(15);
                pTarget.a.spawnParticle(Toolbox.color_heal);
            }
            return true;
        }

        public static bool ZhanXun8_effectAction(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget.a.data.health < pTarget.a.getMaxHealth())
            {
                pTarget.a.restoreHealth(34);
                pTarget.a.spawnParticle(Toolbox.color_heal);
            }
            return true;
        }

        public static bool ZhanXun9_effectAction(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget.a.data.health < pTarget.a.getMaxHealth())
            {
                pTarget.a.restoreHealth(60);
                pTarget.a.spawnParticle(Toolbox.color_heal);
            }
            return true;
        }

        public static bool ZhanXun91_effectAction(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget.a.data.health < pTarget.a.getMaxHealth())
            {
                pTarget.a.restoreHealth(125);
                pTarget.a.spawnParticle(Toolbox.color_heal);
            }
            return true;
        }

        public static bool ZhanXun92_effectAction(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget.a.data.health < pTarget.a.getMaxHealth())
            {
                pTarget.a.restoreHealth(189);
                pTarget.a.spawnParticle(Toolbox.color_heal);
            }
            return true;
        }

        public static bool ZhanXun93_effectAction(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget.a.data.health < pTarget.a.getMaxHealth())
            {
                pTarget.a.restoreHealth(300);
                pTarget.a.spawnParticle(Toolbox.color_heal);
            }
            return true;
        }

        public static bool ZhanXun94_effectAction(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget.a.data.health < pTarget.a.getMaxHealth())
            {
                pTarget.a.restoreHealth(800);
                pTarget.a.spawnParticle(Toolbox.color_heal);
            }
            return true;
        }

        public static bool ZhanXun95_effectAction(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget.a.data.health < pTarget.a.getMaxHealth())
            {
                pTarget.a.restoreHealth(1500);
                pTarget.a.spawnParticle(Toolbox.color_heal);
            }
            return true;
        }

        public static bool ZhanXun96_effectAction(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget.a.data.health < pTarget.a.getMaxHealth())
            {
                pTarget.a.restoreHealth(3000);
                pTarget.a.spawnParticle(Toolbox.color_heal);
            }
            return true;
        }

        public static bool ZhanXun97_effectAction(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget.a.data.health < pTarget.a.getMaxHealth())
            {
                pTarget.a.restoreHealth(5600);
                pTarget.a.spawnParticle(Toolbox.color_heal);
            }
            return true;
        }

        public static bool ZhanXun98_effectAction(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget.a.data.health < pTarget.a.getMaxHealth())
            {
                pTarget.a.restoreHealth(11000);
                pTarget.a.spawnParticle(Toolbox.color_heal);
            }

            return true;
        }

        public static bool HunterKing_effectAction(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget == null || !pTarget.isActor()) return true;

            Actor actor = pTarget.a;
            if (actor == null || !actor.isAlive()) return true;

            int lastTeleportTime;
            actor.data.get("kingSlayerMasterTeleportTimer", out lastTeleportTime, 0);
            int currentTime = (int)BehaviourActionBase<Actor>.world.getCurWorldTime();

            if (currentTime - lastTeleportTime < 120)
            {
                return true;
            }

            foreach (var otherActor in World.world.units)
            {
                if (otherActor == null || !otherActor.isAlive() || otherActor == actor) continue;

                if (otherActor.isKing())
                {
                    if (actor.areFoes(otherActor))
                    {
                        if (otherActor.current_tile != null)
                        {
                            EffectsLibrary.spawnAt("fx_teleport_red", actor.current_position, actor.stats["scale"]);
                            BaseEffect tEffect = EffectsLibrary.spawnAt("fx_teleport_red", otherActor.current_tile.posV3, actor.stats["scale"]);
                            if (tEffect != null)
                            {
                                tEffect.sprite_animation.setFrameIndex(9);
                            }

                            actor.cancelAllBeh();
                            actor.spawnOn(otherActor.current_tile, 0f);

                            actor.spawnParticle(Toolbox.color_red);

                            actor.data.set("kingSlayerMasterTeleportTimer", currentTime);

                            break;
                        }
                    }
                }
            }

            return true;
        }
    }
}