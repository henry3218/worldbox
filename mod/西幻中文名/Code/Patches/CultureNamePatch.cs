using System.Collections.Generic;
using Chinese_Name.utils;
using HarmonyLib;
using NeoModLoader.General.Event.Handlers;
using NeoModLoader.General.Event.Listeners;

namespace Chinese_Name;

public class CultureNamePatch : IPatch
{
    public void Initialize()
    {
        //CultureCreateListener.RegisterHandler(new RenameCulture());
        new Harmony(nameof(set_culture_name)).Patch(AccessTools.Method(typeof(Culture), nameof(Culture.createCulture)),
            postfix: new HarmonyMethod(GetType(), nameof(set_culture_name)));
    }

    private static void set_culture_name(Culture __instance, Actor pActor)
    {
        string race_id = pActor.asset.id;
    string template_id = $"{race_id}_culture";
    
    // [新增回退机制] ↓↓↓
    var generator = CN_NameGeneratorLibrary.Instance.get(template_id);
    if (generator == null) 
    {
        generator = CN_NameGeneratorLibrary.Instance.get("human_culture");
        if(generator == null) return;
    }

        var para = new Dictionary<string, string>();

        ParameterGetters.GetCultureParameterGetter(generator.parameter_getter)(__instance, para);
        __instance.data.name = generator.GenerateName(para);
    }
}