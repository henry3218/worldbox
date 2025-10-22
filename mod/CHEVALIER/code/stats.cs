namespace Chevalier.code
{
    internal class stats
    {
        public static BaseStatAsset Resist;
        public static BaseStatAsset Comprehension;

        public static void Init()
        {
            BaseStatAsset Chevalier = new BaseStatAsset();
            Chevalier.id = "Chevalier";
            Chevalier.normalize = true;
            Chevalier.normalize_min = -9999999;
            Chevalier.normalize_max = 9999999;
            //Chevalier.multiplier = true;
            Chevalier.used_only_for_civs = false;
            AssetManager.base_stats_library.add(Chevalier);

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

            // 定义领悟度属性
            Comprehension = new BaseStatAsset();
            Comprehension.id = "Comprehension";
            Comprehension.normalize = true;
            Comprehension.normalize_min = 0;
            Comprehension.normalize_max = 99999;
            Comprehension.used_only_for_civs = false;
            AssetManager.base_stats_library.add(Comprehension);
        }
    }
}