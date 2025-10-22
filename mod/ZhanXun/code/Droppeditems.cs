using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using ReflectionUtility;
using ai;
using UnityEngine;
using NeoModLoader.General;

namespace ChivalryZhanXun.code
{
    internal class Droppeditems
    {
        public static void Init()
        {

            DropAsset warriorDrop = AssetManager.drops.get("warrior_drop");
            warriorDrop = new DropAsset();
            warriorDrop.id = "warrior_drop";
            warriorDrop.path_texture = "ui/drops/peachpunch";
            warriorDrop.random_frame = true;
            warriorDrop.default_scale = 0.2f;
            warriorDrop.falling_height = new Vector2(30f, 45f);
            warriorDrop.sound_drop = "event:/SFX/DROPS/DropRain";
            warriorDrop.type = DropType.DropGeneric;
            warriorDrop.surprises_units = false;
            warriorDrop.action_landed_drop = ActionGiveWarriorProfession;
            AssetManager.drops.add(warriorDrop);


            // 创建士兵神力
            GodPower warriorPower = new GodPower();
            warriorPower.id = "zhanxun_warrior_rain";
            warriorPower.name = "士兵";
            warriorPower.type = PowerActionType.PowerSpawnDrops;
            warriorPower.click_power_action = SpawnWarriorDrops;
            warriorPower.show_close_actor = true;
            warriorPower.unselect_when_window = true;
            warriorPower.path_icon = "ui/Icons/god_powers/PeachPunch";
            warriorPower.can_drag_map = true;
            warriorPower.hold_action = true;
            warriorPower.mouse_hold_animation = MouseHoldAnimation.Draw;
            warriorPower.falling_chance = 0.02f;
            warriorPower.show_tool_sizes = true;
            warriorPower.cached_drop_asset = AssetManager.drops.get("warrior_drop");
            warriorPower.click_power_action = (PowerAction)Delegate.Combine(
                warriorPower.click_power_action,
                new PowerAction(FlashPixelEffect)
            );
            warriorPower.click_power_brush_action = new PowerAction((pTile, pPower) =>
            {
                return (bool)AssetManager.powers.CallMethod("loopWithCurrentBrushPowerForDropsFull", pTile, pPower);
            });
            AssetManager.powers.add(warriorPower);
        }

        // 将单位转变为战士的方法
        public static void ActionGiveWarriorProfession(Drop pDrop, WorldTile pTile, string pDropID)
        {
            if (pTile == null) return;

            foreach (Actor actor in pTile._units)
            {
                if (actor == null) continue;
                if (actor.city == null) continue;

                actor.city.makeWarrior(actor);
            }
        }

        // 生成战士掉落物的方法
        private static bool SpawnWarriorDrops(WorldTile pTile, GodPower pPower)
        {
            return (bool)AssetManager.powers.CallMethod("spawnDrops", pTile, pPower);
        }

        // 像素闪烁效果
        private static bool FlashPixelEffect(WorldTile pTile, GodPower pPower)
        {
            // 这里可以添加闪烁效果的代码
            return true;
        }
    }
}