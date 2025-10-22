using System;
using System.Collections.Generic;
using System.Linq;
using Chinese_Name.constants;
using NeoModLoader.api.attributes;
using NeoModLoader.General;

namespace Chinese_Name;

public static class ParameterGetters
{
    private static readonly Dictionary<string, Action<Actor, Dictionary<string, string>>> actor_parameter_getters =
        new()
        {
            {
                "default", default_actor_parameter_getter
            }
        };

    private static readonly Dictionary<string, Action<Book, Dictionary<string, string>>> book_parameter_getters = new()
    {
        {
            "default", default_book_parameter_getter
        }
    };

    private static void default_book_parameter_getter(Book pBook, Dictionary<string, string> pParameters)
    {
    }

    private static readonly Dictionary<string, Action<City, Dictionary<string, string>>> city_parameter_getters = new()
    {
        {
            "default", default_city_parameter_getter
        }
    };

    private static readonly Dictionary<string, Action<Kingdom, Dictionary<string, string>>> kingdom_parameter_getters =
        new()
        {
            {
                "default", default_kingdom_parameter_getter
            }
        };

    private static readonly Dictionary<string, Action<Culture, Dictionary<string, string>>> culture_parameter_getters =
        new()
        {
            {
                "default", default_culture_parameter_getter
            }
        };

    private static readonly Dictionary<string, Action<Language, Dictionary<string, string>>> language_parameter_getters = new()
    {
        {
            "default", default_language_parameter_getter
        }
    };
    private static readonly Dictionary<string, Action<Subspecies, Dictionary<string, string>>> subspecies_parameter_getters = new()
    {
        {
            "default", default_subspecies_parameter_getter
        }
    };

    private static readonly Dictionary<string, Action<Religion, Dictionary<string, string>>>
        religion_parameter_getters = new()
        {
            {
                "default", default_religion_parameter_getter
            }
        };

    private static readonly Dictionary<string, Action<Clan, Actor, Dictionary<string, string>>> clan_parameter_getters =
        new()
        {
            {
                "default", default_clan_parameter_getter
            }
        };

    private static readonly Dictionary<string, Action<Alliance, Dictionary<string, string>>>
        alliance_parameter_getters = new()
        {
            {
                "default", default_alliance_parameter_getter
            }
        };

    private static readonly Dictionary<string, Action<War, Dictionary<string, string>>> war_parameter_getters = new()
    {
        {
            "default", default_war_parameter_getter
        }
    };

    private static readonly Dictionary<string, Action<ItemData, ItemAsset, Actor, Dictionary<string, string>>>
        item_parameter_getters = new()
        {
            {
                "default", default_item_parameter_getter
            }
        };

    internal static readonly List<Action<Dictionary<string, string>>> global_parameter_getters = new()
    {
        default_global_parameter_getter
    };

    private static readonly Dictionary<Type, Dictionary<string, Delegate>> custom_parameter_getters = new();

    [Hotfixable]
    private static void default_actor_parameter_getter(Actor pActor, Dictionary<string, string> pParameters)
    {
        pParameters["id"] = pActor.asset.id;
        if (!string.IsNullOrEmpty(pActor.asset.name_locale))
            pParameters["locale"] = LocalizedTextManager.stringExists(pActor.asset.name_locale)
                ? LM.Get(pActor.asset.name_locale)
                : pActor.asset.name_locale;
    }
    [Hotfixable]
    private static void default_subspecies_parameter_getter(Subspecies pSubspecies, Dictionary<string, string> pParameters)
    {
        pParameters["id"] = pSubspecies.species_id;
        pParameters["locale"] = AssetManager.actor_library.get(pSubspecies.species_id).getTranslatedName();
    }

    private static void default_religion_parameter_getter(Religion pReligion, Dictionary<string, string> pParameters)
    {
        
    }

    [Hotfixable]
    private static void default_city_parameter_getter(City pCity, Dictionary<string, string> pParameters)
    {
        pParameters["race"] = pCity.data.original_actor_asset;
    }

    [Hotfixable]
    private static void default_kingdom_parameter_getter(Kingdom pKingdom, Dictionary<string, string> pParameters)
    {
        pParameters["race"] = pKingdom.data.original_actor_asset;
    }

    [Hotfixable]
    private static void default_culture_parameter_getter(Culture pCulture, Dictionary<string, string> pParameters)
    {
        pParameters["race"] = pCulture.data.original_actor_asset;
    }

    private static void default_language_parameter_getter(Language pLang, Dictionary<string, string> pParameters)
    {
        
    }

    [Hotfixable]
    private static void default_clan_parameter_getter(Clan pClan, Actor pActor, Dictionary<string, string> pParameters)
    {
        pParameters["race"] = pClan.data.original_actor_asset;
        pParameters["founder_home"] = string.IsNullOrEmpty(pClan.data.founder_city_name)
            ? pClan.data.founder_kingdom_name
            : pClan.data.founder_city_name;

        if (pActor == null)
        {
            foreach (var unit in pClan.units)
            {
                unit.data.get(DataS.family_name, out var family_name, "");
                if (!string.IsNullOrEmpty(family_name))
                {
                    pParameters["founder_family_name"] = family_name;
                    return;
                }
            }
        }
        else
        {
            pActor.data.get(DataS.family_name, out var family_name, "无名");
            pParameters["founder_family_name"] = family_name;
        }
    }

    [Hotfixable]
    private static void default_alliance_parameter_getter(Alliance pAlliance, Dictionary<string, string> pParameters)
    {
        var list = new List<Kingdom>(pAlliance.kingdoms_hashset);
        pParameters["k1_short"] = list[0].data.name;
        pParameters["k2_short"] = list[1].data.name;
        pParameters["k1_capital"] = list[0].capital?.name;
        if (list[0].cities?.Count > 0)
            if (string.IsNullOrEmpty(pParameters["k1_capital"]))
                pParameters["k1_capital"] = list[0].cities[0].name;
        pParameters["k2_capital"] = list[1].capital?.name;
        if (list[1].cities?.Count > 0)
            if (string.IsNullOrEmpty(pParameters["k2_capital"]))
                pParameters["k2_capital"] = list[1].cities[0].name;
    }

    [Hotfixable]
    private static void default_war_parameter_getter(War pWar, Dictionary<string, string> pParameters)
    {
        pParameters["attacker"] = pWar.main_attacker.data.name;
        pParameters["defender"] = pWar.main_defender?.data.name;
        pParameters["attacker_leader"] = pWar.data.started_by_actor_name;
        pParameters["defender_leader"] = pWar.main_defender?.capital?.leader?.getName();
        pParameters["attacker_short"] = pWar.main_attacker.data.name[0].ToString();
        pParameters["defender_short"] = pWar.main_defender?.data.name[0].ToString();
        pParameters["defender_capital"] = pWar.main_defender?.capital?.data.name;
        if (pWar.main_defender?.cities?.Count > 0)
        {
            if (string.IsNullOrEmpty(pParameters["defender_capital"]))
            {
                pParameters["defender_capital"] = pWar.main_defender.cities[0].name;
            }

            if (string.IsNullOrEmpty(pParameters["defender_leader"]))
            {
                pParameters["defender_leader"] = pWar.main_defender.cities[0].leader?.getName();
            }
        }
    }

    [Hotfixable]
    private static void default_item_parameter_getter(ItemData pItemData, ItemAsset pItemAsset, Actor pActor,
        Dictionary<string, string> pParameters)
    {
        // 材质
        pParameters["material"] = pItemData.material;
        // 类型/对于weapon_name_city这些玩意来说可能会有用
        pParameters["type"] = pItemData.asset_id;
        // 本地名
        var locale = pItemAsset.getLocaleID();
        if (LocalizedTextManager.stringExists(locale)) locale = LM.Get(locale);
        pParameters["locale"] = locale;
        // 装备类别
        locale = pItemAsset.name_class;
        if (LocalizedTextManager.stringExists(locale)) locale = LM.Get(locale);
        pParameters["class"] = locale;
        if (pActor == null) return;
        // 制作者的城市名
        pParameters["city"] = pActor.city?.name;
        // 制作者的文化名
        pParameters["culture"] = pActor.culture?.data.name;

        if (pActor.kingdom == null) return;
        // 制作者的国家名
        pParameters["kingdom"] = pActor.kingdom.data.name;
        if (pActor.kingdom.king != null)
        {
            // 制作者的君主名
            pParameters["king"] = pActor.kingdom.king.getName();
        }

        if (!pActor.kingdom.hasEnemies()) return;

        foreach (Kingdom kingdom in pActor.kingdom.getEnemiesKingdoms().Where(kingdom => kingdom.king != null))
        {
            // 敌国名
            pParameters["enemy_kingdom"] = kingdom.data.name;
            // 敌国君主名
            pParameters["enemy_king"] = kingdom.king.getName();
        }
    }

    [Hotfixable]
    private static void default_global_parameter_getter(Dictionary<string, string> pParameters)
    {
        pParameters["month"] = AssetManager.months.getMonth(Date.getCurrentMonth()).english_name;
        pParameters["year"] = Date.getCurrentYear().ToString();
        World.world.era_manager.prepare();
        pParameters["era"] = World.world.era_manager.getCurrentAge().id;
        pParameters["天干地支纪年"] = LM.Get($"天干地支-{Date.getCurrentYear() % 60}");
    }

    public static Action<Actor, Dictionary<string, string>> GetActorParameterGetter(string pName)
    {
        if (actor_parameter_getters.TryGetValue(pName, out var getter)) return getter;
        return actor_parameter_getters["default"];
    }

    public static Action<Subspecies, Dictionary<string, string>> GetSubspeciesParameterGetter(string pName)
    {
        if (subspecies_parameter_getters.TryGetValue(pName, out var getter)) return getter;
        return subspecies_parameter_getters["default"];
    }

    public static Action<City, Dictionary<string, string>> GetCityParameterGetter(string pName)
    {
        if (city_parameter_getters.TryGetValue(pName, out var getter)) return getter;
        return city_parameter_getters["default"];
    }

    public static Action<Kingdom, Dictionary<string, string>> GetKingdomParameterGetter(string pName)
    {
        if (kingdom_parameter_getters.TryGetValue(pName, out var getter)) return getter;
        return kingdom_parameter_getters["default"];
    }

    public static Action<Culture, Dictionary<string, string>> GetCultureParameterGetter(string pName)
    {
        if (culture_parameter_getters.TryGetValue(pName, out var getter)) return getter;
        return culture_parameter_getters["default"];
    }

    public static Action<Language, Dictionary<string, string>> GetLanguageParameterGetter(string pName)
    {
        if (language_parameter_getters.TryGetValue(pName, out var getter)) return getter;
        return language_parameter_getters["default"];
    }
    public static Action<Religion, Dictionary<string, string>> GetReligionParameterGetter(string pName)
    {
        if (religion_parameter_getters.TryGetValue(pName, out var getter)) return getter;
        return religion_parameter_getters["default"];
    }
    public static Action<Clan, Actor, Dictionary<string, string>> GetClanParameterGetter(string pName)
    {
        if (clan_parameter_getters.TryGetValue(pName, out var getter)) return getter;
        return clan_parameter_getters["default"];
    }

    public static Action<Alliance, Dictionary<string, string>> GetAllianceParameterGetter(string pName)
    {
        if (alliance_parameter_getters.TryGetValue(pName, out var getter)) return getter;
        return alliance_parameter_getters["default"];
    }

    public static Action<Book, Dictionary<string, string>> GetBookParameterGetter(string pName)
    {
        if (book_parameter_getters.TryGetValue(pName, out var getter)) return getter;
        return book_parameter_getters["default"];
    }

    public static Action<War, Dictionary<string, string>> GetWarParameterGetter(string pName)
    {
        if (war_parameter_getters.TryGetValue(pName, out var getter)) return getter;
        return war_parameter_getters["default"];
    }

    public static Action<ItemData, ItemAsset, Actor, Dictionary<string, string>> GetItemParameterGetter(string pName)
    {
        if (item_parameter_getters.TryGetValue(pName, out var getter)) return getter;
        return item_parameter_getters["default"];
    }

    public static T GetCustomParameterGetter<T>(string pName) where T : Delegate
    {
        if (custom_parameter_getters.TryGetValue(typeof(T), out var getters))
        {
            if (getters.TryGetValue(pName, out var getter)) return (T)getter;
            return getters["default"] as T;
        }

        return null;
    }

    public static void PutActorParameterGetter(string pName, Action<Actor, Dictionary<string, string>> pGetter)
    {
        actor_parameter_getters[pName] = pGetter;
    }

    public static void PutCityParameterGetter(string pName, Action<City, Dictionary<string, string>> pGetter)
    {
        city_parameter_getters[pName] = pGetter;
    }

    public static void PutKingdomParameterGetter(string pName, Action<Kingdom, Dictionary<string, string>> pGetter)
    {
        kingdom_parameter_getters[pName] = pGetter;
    }

    public static void PutCultureParameterGetter(string pName, Action<Culture, Dictionary<string, string>> pGetter)
    {
        culture_parameter_getters[pName] = pGetter;
    }

    public static void PutClanParameterGetter(string pName, Action<Clan, Actor, Dictionary<string, string>> pGetter)
    {
        clan_parameter_getters[pName] = pGetter;
    }

    public static void PutAllianceParameterGetter(string pName, Action<Alliance, Dictionary<string, string>> pGetter)
    {
        alliance_parameter_getters[pName] = pGetter;
    }

    public static void PutWarParameterGetter(string pName, Action<War, Dictionary<string, string>> pGetter)
    {
        war_parameter_getters[pName] = pGetter;
    }

    public static void PutItemParameterGetter(string pName,
        Action<ItemData, ItemAsset, Actor, Dictionary<string, string>> pGetter)
    {
        item_parameter_getters[pName] = pGetter;
    }

    public static void PutCustomParameterGetter<T>(string pName, T pGetter) where T : Delegate
    {
        if (!custom_parameter_getters.TryGetValue(typeof(T), out var getters))
        {
            getters = new Dictionary<string, Delegate>();
            custom_parameter_getters[typeof(T)] = getters;
        }

        getters[pName] = pGetter;
    }

    public static void PutGlobalParameterGetter(Action<Dictionary<string, string>> pGetter)
    {
        global_parameter_getters.Add(pGetter);
    }
}