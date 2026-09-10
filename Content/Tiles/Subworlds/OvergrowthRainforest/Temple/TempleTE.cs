using CalamityMod;
using CalRemix.Content.NPCs.Subworlds.GreatSea;
using CalRemix.Core.Subworlds;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CalRemix.Content.Tiles.Subworlds.OvergrowthRainforest.Temple
{
    public abstract class TempleTE : ModTileEntity
    {
        public Point designatedRoom = new Point(0, 0);

        public virtual int tileID { get; set; }


        public override bool IsTileValidForEntity(int x, int y)
        {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == tileID;
        }
        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
        {
            TileObjectData tileData = TileObjectData.GetTileData(type, style, alternate);

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                //Sync the entire multitile's area. 
                NetMessage.SendTileSquare(Main.myPlayer, i, j, tileData.Width, tileData.Height);

                //Sync the placement of the tile entity with other clients
                NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i, j, Type);

                return -1;
            }

            int placedEntity = Place(i, j);

            return placedEntity;
        }

        public override void OnNetPlace()
        {
            NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
        }

        public override void Update()
        {
            if (OvergrowthRainforestGeneration.Rooms == null)
                return;
            bool anyPlayers = false;
            foreach (Player p in Main.ActivePlayers)
            {
                if (p.Remix().currentTempleRoom == OvergrowthRainforestGeneration.SafeRoom(designatedRoom))
                {
                    anyPlayers = true;
                    break;
                }
            }

            if (!anyPlayers)
            {
                ResetObject();
                return;
            }

            ObjectBehaviour();
        }

        public virtual void ObjectBehaviour()
        {

        }

        public virtual void ResetObject()
        {

        }
    }
}
