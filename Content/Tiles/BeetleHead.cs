using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.GameContent.ItemDropRules;
using System.Linq;
using Terraria.GameContent.Animations;
using CalamityMod.DataStructures;
using Terraria.GameContent;
using Steamworks;
using System.Runtime.ConstrainedExecution;

namespace CalRemix.Content.Tiles
{
    public class BeetleHeadPlaced : ModTile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        int rotationbottom;
        int negavar;
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
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(GetInstance<BeetleTE>().Hook_AfterPlacement, -1, 0, false);
            TileObjectData.addTile(Type);
        }

        private bool shittypostimer = true;
        private bool shittynegatimer = false;
        public override void PlaceInWorld(int i, int j, Item item)
        {
            rotationbottom = -35;
        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            GetInstance<BeetleTE>().Kill(i, j);
        }

        /*public override void RandomUpdate(int i, int j)
        {
            if (rotationbottom <= -30f)
            {
                rotationbottom++;
            }
            if (rotationbottom >= 30f)
            {
                rotationbottom--;
            }
        }*/

        float count;
        float abso;
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            //if (tile.TileFrameX == 0 && tile.TileFrameY == 0)
            {
                //Sproots

                Texture2D Seg1 = Request<Texture2D>("CalRemix/Content/Tiles/BeetleHead").Value;
                Texture2D Seg2 = Request<Texture2D>("CalRemix/Content/Tiles/BeetleBody").Value;
                Texture2D Seg3 = TextureAssets.MagicPixel.Value;

                BeetleTE cables = new BeetleTE();
                GetTEFromCoords(i, j, out cables);
                Vector2 weirdOffset = new Vector2(224, 176);
                //if (tile.TileFrameX == 0 && tile.TileFrameY == 0)
                {
                    GetTEFromCoords(i, j, out cables);
                    if (cables != null)
                    {

                        if (cables.Segments != null)
                        {
                            int gravity = 16;
                            gravity += 8 * i % 6;
                            gravity += 6 * j % 6;
                            cables.Segments = VerletSimulatedSegment.SimpleSimulation(cables.Segments, gravity, 22, 16);
                            cables.Segments[0].locked = true;
                            cables.Segments[0].oldPosition = cables.Segments[0].position;
                            cables.Segments[0].position = new Vector2(i * 16, j * 16);

                            cables.Segments[^2].position.X += MathF.Sin(Main.GlobalTimeWrappedHourly * 2) * 2;
                            for (int u = 0; u < cables.Segments.Count; u++)
                            {
                                VerletSimulatedSegment v = cables.Segments[u];
                                int ct = cables.Segments.Count;
                                //Main.NewText(v.position);
                                Texture2D s2u = Seg1;
                                Texture2D prev2u = Seg1;
                                switch (u)
                                {
                                    case 10:
                                        s2u = Seg1;
                                        prev2u = Seg2;
                                        break;
                                    case 9:
                                        s2u = Seg2;
                                        prev2u = Seg3;
                                        break;
                                    default:
                                        s2u = Seg3;
                                        prev2u = Seg3;
                                        break;

                                }
                                Vector2 origine = new(s2u.Width / 2, s2u.Height);
                                Rectangle rect = new(0, 0, s2u.Width, s2u.Height);
                                if (u > 0)
                                {
                                    float dist = u > 0 ? Vector2.Distance(v.position, cables.Segments[u - 1].position) : 0;
                                    if (dist <= 2)
                                        dist = 2;
                                    dist += 2;
                                    float rote = 0f;
                                    Rectangle final = u < 9 ? new Rectangle(0, 0, (int)dist, 2) : rect;
                                    Vector2 fingalorigin = u < 9 ? TextureAssets.BlackTile.Size() / 2 : origine;
                                    if (u > 0)
                                        rote = v.position.DirectionTo(cables.Segments[u - 1].position).ToRotation();
                                    else
                                        rote = 0;
                                    float scalee = (1 - (u / cables.Segments.Count)) * 1f;
                                    float finalRot = u < 9 ? rote : 0;
                                    Vector2 posAdd = u == 10 ? new Vector2(i % 2 == 0 ? 10 : 10, 10) : u == 9 ? Vector2.UnitY * 74 : new Vector2(-16, 0);
                                    SpriteEffects fx = i % 2 == 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                                    Vector2 pos = v.position - Vector2.UnitX * s2u.Width / 2 + weirdOffset + posAdd;
                                    Color baseColor = Lighting.GetColor((int)(v.position.X / 16f), (int)(v.position.Y / 16f));
                                    Color c = u < 9 ? Utils.MultiplyRGB(baseColor, Color.LightGray) : baseColor;
                                    Main.EntitySpriteDraw(s2u, pos - Main.screenPosition, final, c, finalRot, fingalorigin, scalee, fx, 0);
                                }
                                if (s2u == Seg3)
                                    continue;
                                float rot = u == 0 ? 0f : new Vector2(v.position.X + origine.X, v.position.Y + origine.Y).DirectionTo(cables.Segments[u - 1].position + new Vector2(prev2u.Width / 2, prev2u.Height)).ToRotation() + MathHelper.PiOver2;
                                //spriteBatch.Draw(s2u, v.position - Main.screenPosition - Vector2.UnitX * s2u.Width / 2 + weirdOffset, rect, Color.White, rot, origine, 0.5f, SpriteEffects.None, 0f);
                            }
                        }
                    }
                    else
                    {
                        Main.NewText("Cables was null");
                    }
                }
            }
        }
        public bool GetTEFromCoords(int i, int j, out BeetleTE cable)
        {
            cable = new BeetleTE();
            //for (int k = 0; k <= TileEntity.ByPosition.Count; k++)
            {
                if (TileEntity.ByPosition.ContainsKey(new Point16(i, j)))
                {
                    if (TileEntity.ByPosition[new Point16(i, j)] == null)
                        return false;
                    if (TileEntity.ByPosition[new Point16(i, j)].type == TileEntityType<BeetleTE>())
                    {
                        if (TileEntity.ByPosition[new Point16(i, j)].Position == new Point16(i, j))
                        {
                            cable = TileEntity.ByPosition[new Point16(i, j)] as BeetleTE;
                            return true;
                        }
                    }
                }
            }
            cable = new BeetleTE();
            return false;
        }
    }

    public class BeetleTE : ModTileEntity
    {
        public List<VerletSimulatedSegment> Segments;
        public override bool IsTileValidForEntity(int x, int y)
        {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == TileType<BeetleHeadPlaced>();
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

            char numbo = Position.X.ToString()[(short)(Position.X.ToString().Length - 1)];
            double num = char.GetNumericValue(numbo);
            if (num < 4)
            {
                num += 4;
            }

            //Segments[Segments.Count - 7].oldPosition = Segments[Segments.Count - 7].position;
            //Segments[Segments.Count - 7].position = new Vector2(Position.X * 16, Position.Y * 16) + Vector2.UnitX * (float)Math.Sin(Main.GlobalTimeWrappedHourly * num) * 92 + Main.windSpeedCurrent * 100 * Vector2.UnitX - Vector2.UnitY * 140 + Vector2.UnitY * Math.Abs((float)Math.Cos(Main.GlobalTimeWrappedHourly * 6)) * 42;
        }
    }
}