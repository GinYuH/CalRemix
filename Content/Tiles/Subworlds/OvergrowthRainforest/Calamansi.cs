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
using Terraria.GameContent;
using Terraria.GameContent.Animations;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace CalRemix.Content.Tiles.Subworlds.OvergrowthRainforest
{
    public class Calamansi : ModTile
    {
        public static Asset<Texture2D> GlowTexture;
        public static Asset<Texture2D> FruitTexture;
        public static Asset<Texture2D> LargeFruitTexture;
        public static Asset<Texture2D> LargeGlowTexture;
        public static Asset<Texture2D> Fruit2Texture;
        public static Asset<Texture2D> Glow2Texture;

        public override void Load()
        {
            GlowTexture = Request<Texture2D>("CalRemix/Content/Tiles/Subworlds/OvergrowthRainforest/Calamansi_FruitGlow");
            FruitTexture = Request<Texture2D>("CalRemix/Content/Tiles/Subworlds/OvergrowthRainforest/Calamansi_Fruit");
            LargeFruitTexture = Request<Texture2D>("CalRemix/Content/Tiles/Subworlds/OvergrowthRainforest/Calamansi_FruitLarge");
            LargeGlowTexture = Request<Texture2D>("CalRemix/Content/Tiles/Subworlds/OvergrowthRainforest/Calamansi_FruitLargeGlow");
            Glow2Texture = Request<Texture2D>("CalRemix/Content/Tiles/Subworlds/OvergrowthRainforest/Calamansi_Fruit2Glow");
            Fruit2Texture = Request<Texture2D>("CalRemix/Content/Tiles/Subworlds/OvergrowthRainforest/Calamansi_Fruit2");
        }

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
            GetTEFromCoords(i, j, out CalamansiTE cables);
            if (cables != null)
            {
                bool large = cables.variant == 2;
                bool yuh = cables.variant == 3;
                Texture2D glow = yuh ? Glow2Texture.Value : large ? LargeGlowTexture.Value : GlowTexture.Value;
                Texture2D fruit = yuh ? Fruit2Texture.Value : large ? LargeFruitTexture.Value : FruitTexture.Value;
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
                    SpriteEffects dir = i % 2 == 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                    Main.spriteBatch.EnterShaderRegion(BlendState.Additive);
                    float bloomSize = large ? 0.4f : 0.3f;
                    Main.EntitySpriteDraw(CalRemixAsset.BloomTexture.Value, fruitPos, null, Color.Yellow * 0.5f, 0, CalRemixAsset.BloomTexture.Size() / 2, bloomSize, 0);
                    Main.spriteBatch.ExitShaderRegion();
                    Main.EntitySpriteDraw(fruit, fruitPos, null, Lighting.GetColor(cables.Segments[^1].position.ToTileCoordinates()), fruitRot, fruit.Size() / 2, 1, dir);
                    Main.EntitySpriteDraw(fruit, fruitPos, null, Color.White, fruitRot, fruit.Size() / 2, 1, dir);
                }
            }
        }
        public static bool GetTEFromCoords(int i, int j, out CalamansiTE cable)
        {
            cable = new CalamansiTE();
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
            return false;
        }
    }

    public class CalamansiTE : ModTileEntity
    {
        public int variant;
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

        public override void SaveData(TagCompound tag)
        {
            tag["variant"] = variant;
        }

        public override void LoadData(TagCompound tag)
        {
            variant = tag.Get<int>("variant");
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(variant); ;
        }

        public override void NetReceive(BinaryReader reader)
        {
            variant = reader.ReadInt16();
        }

        public override void Update()
        {
            if (variant == 0)
            {
                if (WorldGen.genRand.NextBool(3))
                    variant = 2;
                else
                    variant = WorldGen.genRand.NextBool() ? 3 : 1;
            }
            if (Segments is null || Segments.Count < 11)
            {
                Segments = new List<VerletSimulatedSegment>();
                for (int i = 0; i < 11; ++i)
                    Segments.Add(new VerletSimulatedSegment(new Vector2((int)Position.X * 16, (int)Position.Y * 16), false));
            }
            else
            {
                float largeHalf = variant == 2 ? 0.5f : 1;
                float gravMod = variant == 2 ? 1.4f : 1;
                float gravity = 16;
                gravity += 8 * Position.X % 6 * gravMod;
                gravity += 6 * Position.Y % 6 * gravMod;
                Vector2 oldPos = Segments[^1].oldPosition;
                Vector2 headPos = Segments[^1].position;
                Segments = VerletSimulatedSegment.SimpleSimulation(Segments, gravity, 10, 4);
                Segments[0].locked = true;
                Segments[0].oldPosition = Segments[0].position;
                Segments[0].position = new Vector2(Position.X * 16, Position.Y * 16);
                Segments[^1].position.X += MathF.Sin(Main.GlobalTimeWrappedHourly) * 0.5f * largeHalf;
                foreach (Player p in Main.ActivePlayers)
                {
                    Rectangle rect = Utils.CenteredRectangle(Segments[^1].position, Vector2.One * 16);
                    Vector2 segMvt = Vector2.Zero;
                    if (p.getRect().Intersects(rect))
                    {
                        segMvt += p.velocity.ClampMagnitude(0, 8) * 0.8f * largeHalf;
                    }
                    if (p.Distance(Segments[^1].position + segMvt) < Main.screenWidth)
                    {
                        foreach (Projectile pr in Main.ActiveProjectiles)
                        {
                            if (pr.getRect().Intersects(rect))
                            {
                                segMvt += pr.velocity.ClampMagnitude(0, 22) * largeHalf;
                            }
                        }
                    }
                    if (segMvt != Vector2.Zero)
                        Segments[^1].oldPosition = Segments[^1].position;
                    Segments[^1].position += segMvt;
                }
                Point pt = Segments[^1].position.ToTileCoordinates();
                Tile t = CalamityUtils.ParanoidTileRetrieval(pt.X, pt.Y);
                if (t.IsTileSolid())
                    Segments[^1].position = Segments[^1].oldPosition;
            }
        }
    }
}