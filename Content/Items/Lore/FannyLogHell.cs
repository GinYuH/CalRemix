using CalamityMod.Rarities;
using CalamityMod.UI;
using CalRemix.UI.Logs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Lore
{
    public class FannyLogHell : ModItem, ILocalizedModType
    {
        public override string Texture => "CalamityMod/Items/DraedonMisc/DraedonsLogJungle";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Fiery Finds in the Underworld Lab (Entry 4)");
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.rare = ModContent.RarityType<DarkOrange>();
            Item.useAnimation = Item.useTime = 20;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }

        public override bool? UseItem(Player player)
        {
            if (Main.myPlayer == player.whoAmI)
                PopupGUIManager.FlipActivityOfGUIWithType(typeof(FannyLog4));
            return true;
        }
    }
}
