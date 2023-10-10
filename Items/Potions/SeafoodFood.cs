using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using CalamityMod.Items.Potions.Alcohol;
using CalamityMod.Items.Materials;

namespace CalRemix.Items.Potions
{
    public class SeafoodFood : ModItem
    {
        public override string Texture => "CalamityMod/Items/SummonItems/Seafood";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Seafood");
            Tooltip.SetDefault("Minor improvements to all stats");
        }


        public override void SetDefaults()
        {
			Item.width = 40;
			Item.height = 26;
			Item.rare = ItemRarityID.Pink;
			Item.maxStack = 9999;
			Item.consumable = true;
			Item.useAnimation = 17;
			Item.useTime = 17;
			Item.UseSound = SoundID.Item2;
			Item.useStyle = ItemUseStyleID.EatFood;
			Item.useTurn = true;
			Item.buffType = BuffID.WellFed;
			Item.buffTime = 54000;
		}

        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup(RecipeGroupID.Sand, 20).
                AddIngredient(ItemID.Starfish, 10).
                AddIngredient(ItemID.WaterBucket, 5).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}