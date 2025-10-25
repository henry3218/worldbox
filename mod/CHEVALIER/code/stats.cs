namespace Chevalier.code
{
    internal static class stats
    {
        public static BaseStatAsset Resist;
        public static BaseStatAsset Comprehension;

        public static void Init()
        {
            EnsureStat("Chevalier", -9999999f, 9999999f);
            Resist = EnsureStat("Resist", 0f, 999999f);
            EnsureStat("Dodge", 0f, 99999f);
            EnsureStat("Accuracy", 0f, 99999f);
            Comprehension = EnsureStat("Comprehension", 0f, 99999f);
        }

        private static BaseStatAsset EnsureStat(string pId, float pMin, float pMax)
        {
            var existing = AssetManager.base_stats_library.get(pId);
            if (existing == null)
            {
                existing = new BaseStatAsset
                {
                    id = pId
                };
                AssetManager.base_stats_library.add(existing);
            }

            existing.normalize = true;
            existing.normalize_min = pMin;
            existing.normalize_max = pMax;
            existing.used_only_for_civs = false;
            return existing;
        }
    }
}
