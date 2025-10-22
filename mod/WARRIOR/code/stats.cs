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
            BaseStatAsset Warrior = new BaseStatAsset();
            Warrior.id = "Warrior";
            Warrior.normalize = true;
            Warrior.normalize_min = -9999999;
            Warrior.normalize_max = 9999999;
            //Warrior.multiplier = true;
            Warrior.used_only_for_civs = false;
            AssetManager.base_stats_library.add(Warrior);

            // 定义阵纹属性
            Pattern = new BaseStatAsset();
            Pattern.id = "Pattern";
            Pattern.normalize = true;
            Pattern.normalize_min = -9999999;
            Pattern.normalize_max = 9999999;
            Pattern.used_only_for_civs = false;
            AssetManager.base_stats_library.add(Pattern);

            // 定义真罡属性，与武道气血使用相同的上下限
            TrueGang = new BaseStatAsset();
            TrueGang.id = "TrueGang";
            TrueGang.normalize = true;
            TrueGang.normalize_min = -9999999;
            TrueGang.normalize_max = 9999999;
            TrueGang.used_only_for_civs = false;
            AssetManager.base_stats_library.add(TrueGang);

            // 定义真罡真伤倍数属性
            TrueGangTrueDamageMultiplier = new BaseStatAsset();
            TrueGangTrueDamageMultiplier.id = "TrueGangTrueDamageMultiplier";
            TrueGangTrueDamageMultiplier.normalize = true;
            TrueGangTrueDamageMultiplier.normalize_min = 0;
            TrueGangTrueDamageMultiplier.normalize_max = 999999;
            TrueGangTrueDamageMultiplier.used_only_for_civs = false;
            AssetManager.base_stats_library.add(TrueGangTrueDamageMultiplier);

            // 定义真罡回血倍数属性
            TrueGangHealMultiplier = new BaseStatAsset();
            TrueGangHealMultiplier.id = "TrueGangHealMultiplier";
            TrueGangHealMultiplier.normalize = true;
            TrueGangHealMultiplier.normalize_min = 0;
            TrueGangHealMultiplier.normalize_max = 999999;
            TrueGangHealMultiplier.used_only_for_civs = false;
            AssetManager.base_stats_library.add(TrueGangHealMultiplier);

            // 定义真罡伤害稀释倍数属性
            TrueGangDamageReductionMultiplier = new BaseStatAsset();
            TrueGangDamageReductionMultiplier.id = "TrueGangDamageReductionMultiplier";
            TrueGangDamageReductionMultiplier.normalize = true;
            TrueGangDamageReductionMultiplier.normalize_min = 0;
            TrueGangDamageReductionMultiplier.normalize_max = 999999;
            TrueGangDamageReductionMultiplier.used_only_for_civs = false;
            AssetManager.base_stats_library.add(TrueGangDamageReductionMultiplier);

            // 定义 Resist 属性
            Resist = new BaseStatAsset();
            Resist.id = "Resist";
            Resist.normalize = true;
            Resist.normalize_min = 0;
            Resist.normalize_max = 999999;
            Resist.used_only_for_civs = false;
            AssetManager.base_stats_library.add(Resist);

            BaseStatAsset Dodge = new BaseStatAsset();
            Dodge.id = "Dodge";// 闪避率
            Dodge.normalize = true;
            Dodge.normalize_min = 0;
            Dodge.normalize_max = 99999;
            Dodge.used_only_for_civs = false;
            AssetManager.base_stats_library.add(Dodge);

            BaseStatAsset Accuracy = new BaseStatAsset();
            Accuracy.id = "Accuracy";// 命中率
            Accuracy.normalize = true;
            Accuracy.normalize_min = 0;
            Accuracy.normalize_max = 99999;
            Accuracy.used_only_for_civs = false;
            AssetManager.base_stats_library.add(Accuracy);
        }
    }
}