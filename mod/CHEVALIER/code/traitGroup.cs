using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chevalier.code
{
    internal class traitGroup
    {
        public static void Init()
        {
            ActorTraitGroupAsset Chevalier = new ActorTraitGroupAsset();
            Chevalier.id = "Chevalier";
            Chevalier.name = "trait_group_Chevalier";
            Chevalier.color = "#FFFF00";
            AssetManager.trait_groups.add(Chevalier);

            ActorTraitGroupAsset ChivalricFoundations = new ActorTraitGroupAsset();
            ChivalricFoundations.id = "ChivalricFoundations";
            ChivalricFoundations.name = "trait_group_ChivalricFoundations";
            ChivalricFoundations.color = "#00FF00";
            AssetManager.trait_groups.add(ChivalricFoundations);

            ActorTraitGroupAsset LowFightingTechnique = new ActorTraitGroupAsset();
            LowFightingTechnique.id = "LowFightingTechnique";
            LowFightingTechnique.name = "trait_group_LowFightingTechnique";
            LowFightingTechnique.color = "#00FFFF";
            AssetManager.trait_groups.add(LowFightingTechnique);

            ActorTraitGroupAsset MidFightingTechnique = new ActorTraitGroupAsset();
            MidFightingTechnique.id = "MidFightingTechnique";
            MidFightingTechnique.name = "trait_group_MidFightingTechnique";
            MidFightingTechnique.color = "#FF8C00";
            AssetManager.trait_groups.add(MidFightingTechnique);

            ActorTraitGroupAsset FightingTechnique = new ActorTraitGroupAsset();
            FightingTechnique.id = "FightingTechnique";
            FightingTechnique.name = "trait_group_FightingTechnique";
            FightingTechnique.color = "#FF0000";
            AssetManager.trait_groups.add(FightingTechnique);

            ActorTraitGroupAsset LegacyTechnique = new ActorTraitGroupAsset();
            LegacyTechnique.id = "LegacyTechnique";
            LegacyTechnique.name = "trait_group_LegacyTechnique";
            LegacyTechnique.color = "#FF00FF";
            AssetManager.trait_groups.add(LegacyTechnique);

            ActorTraitGroupAsset NineLawsofKnighthood = new ActorTraitGroupAsset();
            NineLawsofKnighthood.id = "NineLawsofKnighthood";
            NineLawsofKnighthood.name = "trait_group_NineLawsofKnighthood";
            NineLawsofKnighthood.color = "#FFA500";
            AssetManager.trait_groups.add(NineLawsofKnighthood);

            ActorTraitGroupAsset KnightlyBloodline = new ActorTraitGroupAsset();
            KnightlyBloodline.id = "KnightlyBloodline";
            KnightlyBloodline.name = "trait_group_KnightlyBloodline";
            KnightlyBloodline.color = "#0000FF";
            AssetManager.trait_groups.add(KnightlyBloodline);

            ActorTraitGroupAsset GodSealGroup = new ActorTraitGroupAsset();
            GodSealGroup.id = "GodSeal";
            GodSealGroup.name = "trait_group_GodSeal";
            GodSealGroup.color = "#FFD700"; // 金色
            AssetManager.trait_groups.add(GodSealGroup);

            ActorTraitGroupAsset MysteriousConcoction = new ActorTraitGroupAsset();
            MysteriousConcoction.id = "MysteriousConcoction";
            MysteriousConcoction.name = "trait_group_MysteriousConcoction";
            MysteriousConcoction.color = "#32CD32"; // 鲜绿色
            AssetManager.trait_groups.add(MysteriousConcoction);

            ActorTraitGroupAsset AncientKnowledge = new ActorTraitGroupAsset();
            AncientKnowledge.id = "AncientKnowledge";
            AncientKnowledge.name = "trait_group_AncientKnowledge";
            AncientKnowledge.color = "#4169E1"; // 皇家蓝色
            AssetManager.trait_groups.add(AncientKnowledge);

            ActorTraitGroupAsset GodKingdomGroup = new ActorTraitGroupAsset
            {
                id = "GodKingdom",
                name = "trait_group_GodKingdom",
                color = "#8A2BE2" // 紫罗兰色
            };
            AssetManager.trait_groups.add(GodKingdomGroup);

            ActorTraitGroupAsset GodlySigil = new ActorTraitGroupAsset
            {
                id = "GodlySigil",
                name = "trait_group_GodlySigil",
                color = "#8B4513" // 深棕色
            };
            AssetManager.trait_groups.add(GodlySigil);
        }
    }
}