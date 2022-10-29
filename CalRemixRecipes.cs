using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using CalamityMod.Items.Potions.Alcohol;
using CalamityMod.Items.Materials;
using CalRemix.Items.Materials;

namespace CalRemix
{
    public class Recipes : ModSystem
    {
        public override void AddRecipes() 
        {
		Recipe recipe = Recipe.Create(ModContent.ItemType<EffulgentFeather>(), 3);
			recipe.AddIngredient<DesertFeather>(3)
			.AddIngredient<LifeAlloy>()
                	.AddTile(TileID.LunarCraftingStation)
			.Register();
        }
        public override void PostAddRecipes()
        {
            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe recipe = Main.recipe[i];
                
                if (recipe.HasResult(ModContent.ItemType<FabsolsVodka>()))
                {
                    recipe.DisableRecipe();
                }
                if (recipe.HasResult(ModContent.ItemType<GrandScale>()))
                {
                    recipe.DisableRecipe();
                }
                if (recipe.TryGetIngredient(ModContent.ItemType<PearlShard>(), out Item shard) && recipe.HasResult(ModContent.ItemType<SeaRemains>()) || recipe.HasResult(ModContent.ItemType<MonstrousKnives>()) || recipe.HasResult(ModContent.ItemType<FirestormCannon>()) || recipe.HasResult(ModContent.ItemType<SuperballBullet>()))
		{
			shard.type = ModContent.ItemType<ParchedScale>();
		}
            }
        }
    }
}
