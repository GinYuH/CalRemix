﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Tiles;

namespace CalRemix.Items.Placeables
{
    public class IonCube : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("Ion Cube");
        }
        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 14;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<IonCubePlaced>();
            Item.width = 12;
            Item.height = 12;
        }
    }
}