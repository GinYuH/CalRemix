using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.ID;
using CalRemix.Content.Walls;
using CalRemix.Content.Tiles.Subworlds.OvergrowthRainforest;

namespace CalRemix.Content.Items.Placeables.Subworlds.OvergrowthRainforest
{
    public class MacroMoss : ModItem
    {
        public override void SetDefaults()
        {
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.width = Item.height = 16;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.maxStack = 9999;
        }

        public override bool? UseItem(Player player)
        {
            Tile tile = Framing.GetTileSafely(Player.tileTargetX, Player.tileTargetY);

            if (tile.HasTile && tile.TileType == ModContent.TileType<TitanodendronWoodPlaced>() && player.IsInTileInteractionRange(Player.tileTargetX, Player.tileTargetY, Terraria.DataStructures.TileReachCheckSettings.Simple))
            {
                Main.tile[Player.tileTargetX, Player.tileTargetY].TileType = (ushort)ModContent.TileType<MossyTitanodendronWoodPlaced>();

                SoundEngine.PlaySound(SoundID.Dig, player.Center);

                return true;
            }

            return false;
        }
    }
}