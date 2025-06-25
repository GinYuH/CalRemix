﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Tiles;

namespace CalRemix.Content.Items.Placeables
{
    public class TorrefiedTephra : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
            // DisplayName.SetDefault("Torrefied Tephra");
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
            Item.createTile = ModContent.TileType<TorrefiedTephraPlaced>();
            Item.width = 12;
            Item.height = 12;
        }
    }
}