using CalamityMod;
using CalamityMod.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Animations;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace CalRemix.Content.Tiles.Subworlds.OvergrowthRainforest
{
    public class RicketyBridge : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = false;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.CoordinateHeights = new[] { 16 };
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(GetInstance<RicketyBridgeTE>().Hook_AfterPlacement, -1, 0, false);
            TileObjectData.addTile(Type);
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            GetInstance<RicketyBridgeTE>().Kill(i, j);
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            return false;
        }
        public static bool GetTEFromCoords(int i, int j, out RicketyBridgeTE cable)
        {
            cable = new RicketyBridgeTE();
            if (TileEntity.ByPosition.ContainsKey(new Point16(i, j)))
            {
                if (TileEntity.ByPosition[new Point16(i, j)] == null)
                    return false;
                if (TileEntity.ByPosition[new Point16(i, j)].type == TileEntityType<RicketyBridgeTE>())
                {
                    if (TileEntity.ByPosition[new Point16(i, j)].Position == new Point16(i, j))
                    {
                        cable = TileEntity.ByPosition[new Point16(i, j)] as RicketyBridgeTE;
                        return true;
                    }
                }
            }
            return false;
        }
    }

    public class RicketyBridgeTE : ModTileEntity
    {
        public Point anchorPoint;
        public List<VerletSimulatedSegment> Segments;
        public override bool IsTileValidForEntity(int x, int y)
        {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == TileType<RicketyBridge>();
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

        public override void SaveData(TagCompound tag)
        {
            tag["anchorX"] = anchorPoint.X;
            tag["anchorY"] = anchorPoint.Y;
        }

        public override void LoadData(TagCompound tag)
        {
            anchorPoint = new Point(0, 0);
            anchorPoint.X = tag.Get<int>("anchorX");
            anchorPoint.Y = tag.Get<int>("anchorY");
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(anchorPoint.X);
            writer.Write(anchorPoint.Y);
        }

        public override void NetReceive(BinaryReader reader)
        {
            anchorPoint = new Point(0, 0);
            anchorPoint.X = reader.ReadInt32();
            anchorPoint.Y = reader.ReadInt32();
        }

        public override void Update()
        {
            //Segments.Clear();
            if (Segments is null || Segments.Count <= 0)
            {
                Segments = new List<VerletSimulatedSegment>();

                int segCount =( anchorPoint.X - Position.X );

                for (int i = 0; i < segCount; ++i)
                    Segments.Add(new VerletSimulatedSegment(new Vector2((int)(Position.X + i) * 16, (int)Position.Y * 16), false));
            }
            else
            {
                Segments = VerletSimulatedSegment.SimpleSimulation(Segments, 2, 30, 0.3f);
                Segments[0].locked = true;
                Segments[0].oldPosition = Segments[0].position;
                Segments[0].position = new Vector2(Position.X * 16, Position.Y * 16);
                Segments[^1].locked = true;
                Segments[^1].oldPosition = Segments[^1].position;
                Segments[^1].position = anchorPoint.ToVector2() * 16;
            }
        }
    }
}