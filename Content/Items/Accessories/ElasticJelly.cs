using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;

namespace CalRemix.Content.Items.Accessories
{
    public class ElasticJelly : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            // DisplayName.SetDefault("Elastic Jelly");
            // Tooltip.SetDefault("Increases wing flight time by 10%");
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 20;
            Item.value = CalamityGlobalItem.RarityBlueBuyPrice;
            Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalRemixPlayer modplayer = player.GetModPlayer<CalRemixPlayer>();
            modplayer.elastigel = true;
        }
    }
}
