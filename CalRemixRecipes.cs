using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using CalRemix.Items;
using CalRemix.Items.Materials;
using CalamityMod.Items.Potions.Alcohol;
using CalamityMod.Items.Materials;
using CalamityMod.Items.PermanentBoosters;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Ammo;

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
            Recipe coin = Recipe.Create(ItemID.PlatinumCoin, 100);
                 coin.AddIngredient<CosmiliteCoin>(1);
                 coin.Register();
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
                if (recipe.HasResult(ModContent.ItemType<Elderberry>()))
                {
                    recipe.DisableRecipe();
                }
                if (recipe.HasResult(ModContent.ItemType<MiracleFruit>()))
                {
                    recipe.DisableRecipe();
                }
                if (recipe.HasResult(ModContent.ItemType<Dragonfruit>()))
                {
                    recipe.DisableRecipe();
                }
                if (recipe.HasResult(ModContent.ItemType<BloodOrange>()))
                {
                    recipe.DisableRecipe();
                }
                if (recipe.HasResult(ModContent.ItemType<CometShard>()))
                {
                    recipe.DisableRecipe();
                }
                if (recipe.HasResult(ModContent.ItemType<EtherealCore>()))
                {
                    recipe.DisableRecipe();
                }
                if (recipe.HasResult(ModContent.ItemType<PhantomHeart>()))
                {
                    recipe.DisableRecipe();
                }
            }
        }
    }
}
