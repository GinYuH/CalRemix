using CalamityMod.CalPlayer;
using CalamityMod.Items;
using CalamityMod.Items.DraedonMisc;
using CalamityMod.Tiles.DraedonStructures;
using Microsoft.Xna.Framework;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CalRemix.Tiles
{
    public class Anomaly109TileEntity : ModTileEntity
    {
        public override bool IsTileValidForEntity(int x, int y)
        {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == ModContent.TileType<Anomaly109Placed>() && tile.TileFrameX == 0 && tile.TileFrameY == 0;
        }
        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                NetMessage.SendTileSquare(Main.myPlayer, i, j, Anomaly109Placed.Width, Anomaly109Placed.Height);
                NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i, j, Type);
                return -1;
            }
            return Place(i, j);
        }
        public override void OnNetPlace() => NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
    }
}