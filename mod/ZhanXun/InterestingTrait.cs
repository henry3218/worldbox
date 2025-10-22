using HarmonyLib;

using ChivalryZhanXun.code;

using NeoModLoader.api;

namespace ChivalryZhanXun
{
    internal class ChivalryZhanXunClass : BasicMod<ChivalryZhanXunClass>
    {
        public static ModDeclare modDeclare;
        public static ModConfig config;
        public static string id = "shiyue.worldbox.mod.ChivalryZhanXun";
        protected override void OnModLoad()
        {
            traitGroup.Init();
            traits.Init();
            Droppeditems.Init();
            SorceryEffect.Init();
            ZhanXunUIManager.Init();
            new Harmony(id).PatchAll(typeof(patch));
            new Harmony(id).PatchAll(typeof(ZhanXunConfig));

            modDeclare = GetDeclaration();
            config = GetConfig();
        }

    }

}
