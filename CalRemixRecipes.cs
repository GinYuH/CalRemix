using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using CalamityMod.Items.Potions.Alcohol;
using CalamityMod.Items.Materials;

namespace CalRemix
{
    public class ExampleRecipes : ModSystem
    {
        public override void PostAddRecipes()
        {
            for (int Fab = 0; Fab < Recipe.numRecipes; Fab++)
            {
                Recipe recipe = Main.recipe[Fab];
                if (recipe.HasResult(ModContent.ItemType<FabsolsVodka>()))
                {
                    recipe.DisableRecipe();
                }
            }
        }
    }
}
