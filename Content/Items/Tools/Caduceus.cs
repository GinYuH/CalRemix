using CalRemix.Core.Subworlds;
using SubworldLibrary;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Tools
{
    public class Caduceus : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("Caduceus");
        }
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.useStyle = ItemUseStyleID.Shoot;
        }
        /*
        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                if (SubworldSystem.Current != ModContent.GetInstance<ExosphereSubworld>())
                    SubworldSystem.Enter<ExosphereSubworld>();
                else
                    SubworldSystem.Exit();
                return true;
            }
            return false;
        }
         */
    }
}
