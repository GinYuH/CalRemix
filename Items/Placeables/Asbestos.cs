﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Tiles;
using CalamityMod.Items;
using CalamityMod.Rarities;

namespace CalRemix.Items.Placeables
{
    public class Asbestos : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
            DisplayName.SetDefault("Asbestos Block");
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
            Item.createTile = ModContent.TileType<AsbestosPlaced>();
            Item.width = 12;
            Item.height = 12;
        }
    }
}