using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Accessories
{
    public class AstralPearl : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("Astral Pearl");
        }

        public override void SetDefaults()
        {
            Item.value = Item.sellPrice(gold: 11, silver: 55);
            Item.rare = ItemRarityID.LightPurple;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<CalRemixPlayer>().pearl = true;
        }
    }
}
