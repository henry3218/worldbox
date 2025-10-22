using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChivalryZhanXun.code
{
    internal class traitGroup
    {
        public static void Init()
        {
            ActorTraitGroupAsset ZhanXun = new ActorTraitGroupAsset();
            ZhanXun.id = "ZhanXun";
            ZhanXun.name = "战勋";
            ZhanXun.color = "#FFFF00";
            AssetManager.trait_groups.add(ZhanXun);

            ActorTraitGroupAsset Diversification = new ActorTraitGroupAsset();
            Diversification.id = "Diversification";
            Diversification.name = "trait_group_Diversification";
            Diversification.color = "#FFFF00";
            AssetManager.trait_groups.add(Diversification);
        }
    }
}
