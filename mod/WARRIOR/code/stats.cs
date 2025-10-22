namespace PeerlessOverpoweringWarrior.code
{
    internal class stats
    {
        public static BaseStatAsset Resist;
        public static BaseStatAsset TrueGang;
        public static BaseStatAsset TrueGangTrueDamageMultiplier; // 真罡真伤倍数
        public static BaseStatAsset TrueGangHealMultiplier; // 真罡回血倍数
        public static BaseStatAsset TrueGangDamageReductionMultiplier; // 真罡伤害稀释倍数

        public static BaseStatAsset Pattern;

        public static void Init()
        {
            BaseStatAsset Warrior = EnsureBaseStat("Warrior", stat =>
            {
                stat.normalize = true;
                stat.normalize_min = -9999999;
                stat.normalize_max = 9999999;
                //Warrior.multiplier = true;
                stat.used_only_for_civs = false;
            });

            // 定义阵纹属性
            Pattern = EnsureBaseStat("Pattern", stat =>
            {
                stat.normalize = true;
                stat.normalize_min = -9999999;
                stat.normalize_max = 9999999;
                stat.used_only_for_civs = false;
            });

            // 定义真罡属性，与武道气血使用相同的上下限
            TrueGang = EnsureBaseStat("TrueGang", stat =>
            {
                stat.normalize = true;
                stat.normalize_min = -9999999;
                stat.normalize_max = 9999999;
                stat.used_only_for_civs = false;
            });

            // 定义真罡真伤倍数属性
            TrueGangTrueDamageMultiplier = EnsureBaseStat("TrueGangTrueDamageMultiplier", stat =>
            {
                stat.normalize = true;
                stat.normalize_min = 0;
                stat.normalize_max = 999999;
                stat.used_only_for_civs = false;
            });

            // 定义真罡回血倍数属性
            TrueGangHealMultiplier = EnsureBaseStat("TrueGangHealMultiplier", stat =>
            {
                stat.normalize = true;
                stat.normalize_min = 0;
                stat.normalize_max = 999999;
                stat.used_only_for_civs = false;
            });

            // 定义真罡伤害稀释倍数属性
            TrueGangDamageReductionMultiplier = EnsureBaseStat("TrueGangDamageReductionMultiplier", stat =>
            {
                stat.normalize = true;
                stat.normalize_min = 0;
                stat.normalize_max = 999999;
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
