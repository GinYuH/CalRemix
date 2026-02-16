using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Accessories
{
    public class BlazingPowder : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            // DisplayName.SetDefault("Blazing Powder");
        }

        public override void SetDefaults()
        {
            Item.damage = 25;
            Item.value = Item.sellPrice(gold: 3, silver: 32);
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<CalRemixPlayer>().blaze = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AshesofCalamity>(10).
                AddIngredient<StarblightSoot>(5).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
