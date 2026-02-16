using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalRemix.Content.Items.Materials;

namespace CalRemix.Content.Items.Tools.Chlorium
{
    public class ChloriumWaraxe : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ChlorophyteGreataxe);
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ChloriumBar>(18).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
