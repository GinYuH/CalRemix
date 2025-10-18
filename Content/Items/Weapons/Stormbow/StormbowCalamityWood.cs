using CalRemix.Content.Items.Placeables.Subworlds.Sealed;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons.Stormbow
{
    #region Acidwood
    // Acidwood
    #endregion
    #region Monolith

    #endregion

    #region Carnelian
    public class CarnelianWoodStormbow : StormbowAbstract
    {
        public override int damage => 10;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<CarnelianWood>(), 10).
                AddIngredient(ItemID.Cobweb, 15).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
    #endregion
}
