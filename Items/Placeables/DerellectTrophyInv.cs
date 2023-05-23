using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CalRemix.Tiles;

namespace CalRemix.Items.Placeables
{
    public class DerellectTrophyInv : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Derellect Trophy");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.useStyle = 1;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.maxStack = 99;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<DerellectTrophyPlaced>();
            Item.width = 12;
            Item.height = 12;
            Item.rare = 6;
        }
    }
}