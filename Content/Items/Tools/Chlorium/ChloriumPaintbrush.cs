using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalRemix.Content.Projectiles;
using CalRemix.Content.Items.Materials;

namespace CalRemix.Content.Items.Tools.Chlorium
{
    public class ChloriumPaintbrush : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Paintbrush);
            Item.tileBoost = 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ChloriumBar>(8).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
