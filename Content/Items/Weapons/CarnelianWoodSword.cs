using CalRemix.Content.Items.Placeables.Subworlds.Sealed;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons
{
    public class CarnelianWoodSword : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.WoodenSword);
            Item.damage += 2;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<CarnelianWood>(), 10)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}