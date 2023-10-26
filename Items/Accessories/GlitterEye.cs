using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Accessories
{
    public class GlitterEye : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("Glittering Astral Eye");
        }

        public override void SetDefaults()
        {
            Item.value = Item.sellPrice(gold: 7, silver: 87);
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<CalRemixPlayer>().pearl = true;
            player.GetModPlayer<CalRemixPlayer>().astralEye = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<BlazingPowder>(1).
                AddIngredient<AstralPearl>(1).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
