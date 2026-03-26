using CalamityMod;
using CalamityMod.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Animations;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace CalRemix.Content.Tiles.Subworlds.OvergrowthRainforest
{
    public class Calamansi : ModTile
    {
        public override void SetStaticDefaults()
        {
            TileID.Sets.DisableSmartCursor[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.FramesOnKillWall[Type] = true; // Necessary since Style3x3Wall uses AnchorWall
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.Height = 1;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16 }; //
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(GetInstance<CalamansiTE>().Hook_AfterPlacement, -1, 0, false);
            TileObjectData.addTile(Type);
        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            GetInstance<CalamansiTE>().Kill(i, j);
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            GetTEFromCoords(i, j, out CalamansiTE cables);
            if (cables != null)
            {
                if (cables.Segments != null)
                {
                    Lighting.AddLight(cables.Segments[^1].position, 2, 2, 1);
                }
            }
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            //if (tile.TileFrameX == 0 && tile.TileFrameY == 0)
            {
                //Sproots

                Texture2D glow = Request<Texture2D>("CalRemix/Content/Tiles/Subworlds/OvergrowthRainforest/Calamansi_FruitGlow").Value;
                Texture2D fruit = Request<Texture2D>("CalRemix/Content/Tiles/Subworlds/OvergrowthRainforest/Calamansi_Fruit").Value;
                Texture2D Seg3 = TextureAssets.MagicPixel.Value;

                CalamansiTE cables = new CalamansiTE();
                GetTEFromCoords(i, j, out cables);
                Vector2 weirdOffset = new Vector2(224, 176);
                Color stringColor = WorldGen.paintColor(tile.TileColor);
                //if (tile.TileFrameX == 0 && tile.TileFrameY == 0)
                {
                    GetTEFromCoords(i, j, out cables);
                    if (cables != null)
                    {

                        if (cables.Segments != null)
                        {
                            for (int u = 0; u < cables.Segments.Count - 1; u++)
                            {
                                if (u == 0)
                                    continue;
                                VerletSimulatedSegment seg = cables.Segments[u];
                                float dist = u > 0 ? Vector2.Distance(seg.position, cables.Segments[u - 1].position) : 0;
                                if (dist <= 2)
                                    dist = 2;
                                dist += 2;
                                float rot = 0f;
                                if (u > 0)
                                    rot = seg.position.DirectionTo(cables.Segments[u - 1].position).ToRotation();
                                else
                                    rot = 0;
                                float scalee = (1 - (u / cables.Segments.Count));
                                Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, seg.position - Main.screenPosition + CalamityUtils.TileDrawOffset + Vector2.UnitX * 16, new Rectangle(0, 0, (int)dist, 2), Lighting.GetColor(seg.position.ToTileCoordinates()).MultiplyRGB(Color.ForestGreen), rot, TextureAssets.BlackTile.Size() / 2, scalee, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
                            }

                            float fruitRot = (cables.Segments[^1].position.DirectionTo(cables.Segments[^2].position)).ToRotation() + MathHelper.PiOver2;
                            Vector2 fruitPos = cables.Segments[^1].position - Main.screenPosition + CalamityUtils.TileDrawOffset + Vector2.UnitX * 10;
                            Main.spriteBatch.EnterShaderRegion(BlendState.Additive);
                            Main.EntitySpriteDraw(CalRemixAsset.BloomTexture.Value, fruitPos, null, Color.Yellow * 0.5f, 0, CalRemixAsset.BloomTexture.Size() / 2, 0.3f, 0);
                            Main.spriteBatch.ExitShaderRegion();
                            Main.EntitySpriteDraw(fruit, fruitPos, null, Lighting.GetColor(cables.Segments[^1].position.ToTileCoordinates()), fruitRot, fruit.Size() / 2, 1, 0);
                            Main.EntitySpriteDraw(fruit, fruitPos, null, Color.White, fruitRot, fruit.Size() / 2, 1, 0);
                        }
                    }
                    else
                    {
                    }
                }
            }
        }
        public bool GetTEFromCoords(int i, int j, out CalamansiTE cable)
        {
            cable = new CalamansiTE();
            //for (int k = 0; k <= TileEntity.ByPosition.Count; k++)
            {
                if (TileEntity.ByPosition.ContainsKey(new Point16(i, j)))
                {
                    if (TileEntity.ByPosition[new Point16(i, j)] == null)
                        return false;
                    if (TileEntity.ByPosition[new Point16(i, j)].type == TileEntityType<CalamansiTE>())
                    {
                        if (TileEntity.ByPosition[new Point16(i, j)].Position == new Point16(i, j))
                        {
                            cable = TileEntity.ByPosition[new Point16(i, j)] as CalamansiTE;
                            return true;
                        }
                    }
                }
            }
            cable = new CalamansiTE();
            return false;
        }
    }

    public class CalamansiTE : ModTileEntity
    {
        public List<VerletSimulatedSegment> Segments;
        public override bool IsTileValidForEntity(int x, int y)
        {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == TileType<Calamansi>();
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
            if (Segments is null || Segments.Count < 11)
            {
                Segments = new List<VerletSimulatedSegment>();
                for (int i = 0; i < 11; ++i)
                    Segments.Add(new VerletSimulatedSegment(new Vector2((int)Position.X * 16, (int)Position.Y * 16), false));
            }
            else
            {
                int gravity = 16;
                gravity += 8 * Position.X % 6;
                gravity += 6 * Position.Y % 6;
                Segments = VerletSimulatedSegment.SimpleSimulation(Segments, gravity, 10, 4);
                Segments[0].locked = true;
                Segments[0].oldPosition = Segments[0].position;
                Segments[0].position = new Vector2(Position.X * 16, Position.Y * 16);
                Segments[^1].position.X += MathF.Sin(Main.GlobalTimeWrappedHourly) * 0.5f;
                foreach (Player p in Main.ActivePlayers)
                {
                    Rectangle rect = Utils.CenteredRectangle(Segments[^1].position, Vector2.One * 16);
                    if (p.getRect().Intersects(rect))
                    {
                        Segments[^1].position += p.velocity.ClampMagnitude(0, 8) * 0.8f;
                    }
                    if (p.Distance(Segments[^1].position) < Main.screenWidth)
                    {
                        foreach (Projectile pr in Main.ActiveProjectiles)
                        {
                            if (pr.getRect().Intersects(rect))
                            {
                                Segments[^1].position += pr.velocity.ClampMagnitude(0, 22);
                            }
                        }
                    }
                }
            }
        }
    }
}