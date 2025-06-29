using CalamityMod.Items;
using CalamityMod.Rarities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Materials
{
    public class KrakenTooth : ModItem
    {
        public override void SetDefaults()
        {
            Item.rare = ModContent.RarityType<Turquoise>();
            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
            Item.maxStack = 9999;
            Item.width = 20;
            Item.height = 20;
        }
    }
}
