using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;

namespace CalRemix.Content.Items.Accessories
{
    public class InvigoratingJelly : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("Invigorating Jelly");
            Tooltip.SetDefault("Activating Adrenaline will defile your soul");
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            item.height = 20;
            Item.value = CalamityGlobalItem.RarityGreenBuyPrice;
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalRemixPlayer modplayer = player.GetModPlayer<CalRemixPlayer>();
            modplayer.invigel = true;
        }
    }
}
