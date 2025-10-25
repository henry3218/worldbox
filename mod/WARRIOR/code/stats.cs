namespace PeerlessOverpoweringWarrior.code
{
    internal static class stats
    {
        public static BaseStatAsset Resist;
        public static BaseStatAsset TrueGang;
        public static BaseStatAsset TrueGangTrueDamageMultiplier;
        public static BaseStatAsset TrueGangHealMultiplier;
        public static BaseStatAsset TrueGangDamageReductionMultiplier;
        public static BaseStatAsset Pattern;

        public static void Init()
        {
            EnsureStat("Warrior", -9999999f, 9999999f);
            Pattern = EnsureStat("Pattern", -9999999f, 9999999f);
            TrueGang = EnsureStat("TrueGang", -9999999f, 9999999f);
            TrueGangTrueDamageMultiplier = EnsureStat("TrueGangTrueDamageMultiplier", 0f, 999999f);
            TrueGangHealMultiplier = EnsureStat("TrueGangHealMultiplier", 0f, 999999f);
            TrueGangDamageReductionMultiplier = EnsureStat("TrueGangDamageReductionMultiplier", 0f, 999999f);
            Resist = EnsureStat("Resist", 0f, 999999f);
            EnsureStat("Dodge", 0f, 99999f);
            EnsureStat("Accuracy", 0f, 99999f);
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
