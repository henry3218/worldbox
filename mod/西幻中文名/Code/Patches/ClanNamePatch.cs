using System.Collections.Generic;
using Chinese_Name.utils;
using HarmonyLib;
using NeoModLoader.General.Event.Handlers;
using NeoModLoader.General.Event.Listeners;

namespace Chinese_Name;

public class ClanNamePatch : IPatch
{
    public void Initialize()
    {
       // ClanCreateListener.RegisterHandler(new RenameClan());
       new Harmony(nameof(set_clan_name)).Patch(AccessTools.Method(typeof(Clan), nameof(Clan.newClan)),
           postfix: new HarmonyMethod(AccessTools.Method(GetType(), nameof(set_clan_name))));
       new Harmony(nameof(set_clan_motto)).Patch(AccessTools.Method(typeof(Clan), nameof(Clan.getMotto)),
           prefix: new HarmonyMethod(AccessTools.Method(GetType(), nameof(set_clan_motto))));
    }

    private static bool set_clan_motto(Clan __instance)
    {
        if (!string.IsNullOrWhiteSpace(__instance.data.motto)) return true;
        var generator = CN_NameGeneratorLibrary.Instance.get("clan_mottos");
        if (generator == null) return true;

        var para = new Dictionary<string, string>();

        ParameterGetters.GetClanParameterGetter(generator.parameter_getter)(__instance, null, para);

        __instance.data.motto = generator.GenerateName(para);
        return true;
    }

    private static void set_clan_name(Clan __instance, Actor pFounder)
    {
        string race_id = pFounder.asset.id;
    string template_id = $"{race_id}_clan";
    
    // [新增回退机制] ↓↓↓
    var generator = CN_NameGeneratorLibrary.Instance.get(template_id);
    if (generator == null) 
    {
        generator = CN_NameGeneratorLibrary.Instance.get("human_clan");
        if(generator == null) return;
    }

        var para = new Dictionary<string, string>();

        ParameterGetters.GetClanParameterGetter(generator.parameter_getter)(__instance, pFounder, para);

        __instance.data.name = generator.GenerateName(para);
    }
/*
    class RenameClan : ClanCreateHandler
    {
        private static readonly HashSet<string> vanilla_postfix = new()
        {
            "ak", "an", "ok", "on", "uk", "un"
        };

        public override void Handle(Clan pClan, Actor pFounder)
        {
            if (!string.IsNullOrWhiteSpace(pClan.data.name) &&
                !vanilla_postfix.Contains(pClan.data.name.Trim())) return;
            if (pFounder == null) return;

            var asset = CN_NameGeneratorLibrary.Instance.get(pFounder.race.name_template_clan);
            if (asset == null) return;

            var para = new Dictionary<string, string>();

            ParameterGetters.GetClanParameterGetter(asset.parameter_getter)(pClan, pFounder, para);

            pClan.data.name = asset.GenerateName(para);
        }
    }*/
}