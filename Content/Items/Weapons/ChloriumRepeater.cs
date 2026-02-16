using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Items.Materials;

namespace CalRemix.Content.Items.Weapons
{
    public class ChloriumRepeater : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.AdamantiteRepeater);
            Item.rare = ItemRarityID.Lime;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ChloriumBar>(12).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
