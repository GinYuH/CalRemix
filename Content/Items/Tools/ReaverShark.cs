using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;
using CalamityMod.Rarities;
using CalRemix.Content.Items.Materials;
using CalamityMod.Items.Materials;

namespace CalRemix.Content.Items.Tools
{
    public class ReaverSharkWaitNVM : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ReaverShark);
            Item.pick = 65;
            Item.SetNameOverride("Reaver Shark");
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.ReaverShark).
                AddIngredient<PerennialBar>(26).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
