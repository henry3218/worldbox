namespace Chevalier.code
{
    internal class stats
    {
        public static BaseStatAsset Resist;
        public static BaseStatAsset Comprehension;

        public static void Init()
        {
            BaseStatAsset Chevalier = EnsureBaseStat("Chevalier", stat =>
            {
                stat.normalize = true;
                stat.normalize_min = -9999999;
                stat.normalize_max = 9999999;
                //Chevalier.multiplier = true;
                stat.used_only_for_civs = false;
            });

            // 定义 Resist 属性
            Resist = EnsureBaseStat("Resist", stat =>
            {
                stat.normalize = true;
                stat.normalize_min = 0;
                stat.normalize_max = 999999;
                stat.used_only_for_civs = false;
            });

            EnsureBaseStat("Dodge", stat =>
            {
                stat.normalize = true;
                stat.normalize_min = 0;
                stat.normalize_max = 99999;
                stat.used_only_for_civs = false;
            });

            EnsureBaseStat("Accuracy", stat =>
            {
                stat.normalize = true;
                stat.normalize_min = 0;
                stat.normalize_max = 99999;
                stat.used_only_for_civs = false;
            });

            // 定义领悟度属性
            Comprehension = EnsureBaseStat("Comprehension", stat =>
            {
                stat.normalize = true;
                stat.normalize_min = 0;
                stat.normalize_max = 99999;
                stat.used_only_for_civs = false;
            });
        }

        private static BaseStatAsset EnsureBaseStat(string id, System.Action<BaseStatAsset> initializer)
        {
            BaseStatAsset existing = AssetManager.base_stats_library.get(id);
            if (existing != null)
            {
                return existing;
            }

            BaseStatAsset stat = new BaseStatAsset();
            stat.id = id;
            initializer?.Invoke(stat);
            AssetManager.base_stats_library.add(stat);
            return stat;
        }
    }
}
