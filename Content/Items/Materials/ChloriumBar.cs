﻿using CalRemix.Content.Items.Placeables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Materials
{
    public class ChloriumBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
        }
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Lime;
            Item.value = Item.sellPrice(silver: 90);
            Item.maxStack = 9999;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ChloriumOre>(6).
                AddTile(TileID.AdamantiteForge).
                Register();
        }
    }
}
