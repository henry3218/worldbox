using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeerlessOverpoweringWarrior.code
{
    internal class traitGroup
    {
        public static void Init()
        {
            ActorTraitGroupAsset Warrior = new ActorTraitGroupAsset();
            Warrior.id = "Warrior";
            Warrior.name = "trait_group_Warrior";
            Warrior.color = "#FFFF00";
            AssetManager.trait_groups.add(Warrior);

            ActorTraitGroupAsset MartialFoundations = new ActorTraitGroupAsset();
            MartialFoundations.id = "MartialFoundations";
            MartialFoundations.name = "trait_group_MartialFoundations";
            MartialFoundations.color = "#00FF00";
            AssetManager.trait_groups.add(MartialFoundations);

            ActorTraitGroupAsset LowGongFa = new ActorTraitGroupAsset();
            LowGongFa.id = "LowGongFa";
            LowGongFa.name = "trait_group_low_gong_fa";
            LowGongFa.color = "#00FFFF";
            AssetManager.trait_groups.add(LowGongFa);

            ActorTraitGroupAsset MidGongFa = new ActorTraitGroupAsset();
            MidGongFa.id = "MidGongFa";
            MidGongFa.name = "trait_group_mid_gong_fa";
            MidGongFa.color = "#FF8C00";
            AssetManager.trait_groups.add(MidGongFa);

            ActorTraitGroupAsset GongFa = new ActorTraitGroupAsset();
            GongFa.id = "GongFa";
            GongFa.name = "trait_group_GongFa";
            GongFa.color = "#FF0000";
            AssetManager.trait_groups.add(GongFa);

            ActorTraitGroupAsset arcaneTome = new ActorTraitGroupAsset();
            arcaneTome.id = "arcaneTome";
            arcaneTome.name = "trait_group_arcaneTome";
            arcaneTome.color = "#FF00FF";
            AssetManager.trait_groups.add(arcaneTome);

            ActorTraitGroupAsset NineCharacterSecrets = new ActorTraitGroupAsset();
            NineCharacterSecrets.id = "NineCharacterSecrets";
            NineCharacterSecrets.name = "trait_group_NineCharacterSecrets";
            NineCharacterSecrets.color = "#FFA500";
            AssetManager.trait_groups.add(NineCharacterSecrets);

            ActorTraitGroupAsset MartialBloodline = new ActorTraitGroupAsset();
            MartialBloodline.id = "MartialBloodline";
            MartialBloodline.name = "trait_group_MartialBloodline";
            MartialBloodline.color = "#0000FF";
            AssetManager.trait_groups.add(MartialBloodline);

            ActorTraitGroupAsset EmperorSealGroup = new ActorTraitGroupAsset();
            EmperorSealGroup.id = "EmperorSeal";
            EmperorSealGroup.name = "trait_group_EmperorSeal";
            EmperorSealGroup.color = "#FFD700"; // 金色
            AssetManager.trait_groups.add(EmperorSealGroup);

            ActorTraitGroupAsset SpiritualPlants = new ActorTraitGroupAsset();
            SpiritualPlants.id = "SpiritualPlants";
            SpiritualPlants.name = "trait_group_SpiritualPlants";
            SpiritualPlants.color = "#32CD32"; // 鲜绿色
            AssetManager.trait_groups.add(SpiritualPlants);

            ActorTraitGroupAsset celestialGrottoGroup = new ActorTraitGroupAsset
            {
                id = "CelestialGrotto",
                name = "trait_group_CelestialGrotto",
                color = "#8A2BE2" // 紫罗兰色
            };
            AssetManager.trait_groups.add(celestialGrottoGroup);

            ActorTraitGroupAsset AncientMartialBodies = new ActorTraitGroupAsset
            {
                id = "AncientMartialBodies",
                name = "trait_group_AncientMartialBodies",
                color = "#8B4513" // 深棕色
            };
            AssetManager.trait_groups.add(AncientMartialBodies);

            // 添加阵法天赋特质组
            ActorTraitGroupAsset FormationPatterns = new ActorTraitGroupAsset
            {
                id = "FormationPatterns",
                name = "trait_group_FormationPatterns",
                color = "#4B0082" // 靛蓝色
            };
            AssetManager.trait_groups.add(FormationPatterns);

            // 添加阵道境界特质组
            ActorTraitGroupAsset FormationRealms = new ActorTraitGroupAsset
            {
                id = "FormationRealms",
                name = "trait_group_FormationRealms",
                color = "#008080" // 青色
            };
            AssetManager.trait_groups.add(FormationRealms);

            // 添加阵法特质组
            ActorTraitGroupAsset FormationSkills = new ActorTraitGroupAsset
            {
                id = "FormationSkills",
                name = "trait_group_FormationSkills",
                color = "#9370DB" // 中紫色
            };
            AssetManager.trait_groups.add(FormationSkills);
        }
    }
}