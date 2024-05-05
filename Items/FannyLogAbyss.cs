using CalamityMod.Rarities;
using CalamityMod.UI;
using CalamityMod.UI.DraedonLogs;
using CalRemix.UI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items
{
    public class FannyLogAbyss : ModItem, ILocalizedModType
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Depths of the Abyss (Entry ???)");
            Tooltip.SetDefault("It has scribbles all over it, making it unusable");
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.rare = ItemRarityID.Gray;
            Item.useAnimation = Item.useTime = 20;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }

        public override bool? UseItem(Player player)
        {
            if (Main.myPlayer == player.whoAmI)
                PopupGUIManager.FlipActivityOfGUIWithType(typeof(FannyLog7));
            return true;
        }
    }
}
