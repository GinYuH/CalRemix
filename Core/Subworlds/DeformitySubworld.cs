using System.Collections.Generic;
using Terraria;
using SubworldLibrary;
using Terraria.WorldBuilding;
using Terraria.IO;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Terraria.ID;
using CalRemix.Core.World;
using Terraria.Graphics.Effects;
using CalamityMod;
using CalRemix.Core.Biomes;
using CalRemix.Content.Tiles.Subworlds.GreatSea;
using Terraria.Utilities;
using CalamityMod.Schematics;
using CalRemix.Content.Tiles.Subworlds.Sealed;
using CalRemix.Content.Walls;
using CalRemix.Content.NPCs.Subworlds.Sealed;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using CalRemix.Content.Items.Placeables.Subworlds.Sealed;
using CalRemix.Content.Items.SummonItems;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Items.Potions;
using CalamityMod.World;
using static CalRemix.CalRemixHelper;
using CalRemix.Content.NPCs;
using CalRemix.Content.Items.Weapons;
using Microsoft.Build.Tasks;
using System.Security.Cryptography.X509Certificates;
using Steamworks;
using CalRemix.Content.NPCs.Bosses.Origen;

namespace CalRemix.Core.Subworlds
{
    public class DeformitySubworld : Subworld, IDisableOcean, IFixDrawBlack
    {
        public override int Height => 2000;
        public override int Width => 3000;
        public override List<GenPass> Tasks =>
        [
            new DeformityGeneration()
        ];
        public override void Update()
        {
            SubworldUpdateMethods.UpdateLiquids();
            SubworldUpdateMethods.UpdateTiles();
            SubworldUpdateMethods.UpdateTileEntities();
            Main.time = Main.dayLength * 0.5f;
            base.Update();
        }

        public override void DrawMenu(GameTime gameTime)
        {
            base.DrawMenu(gameTime);
            if (WorldGenerator.CurrentGenerationProgress == null)
                return;
            string str = "Progress: " + WorldGenerator.CurrentGenerationProgress.Message + " " + Math.Round(WorldGenerator.CurrentGenerationProgress.Value * 100, 2) + "%";
            Vector2 size = FontAssets.MouseText.Value.MeasureString(str) * 2;
            Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, Vector2.Zero, new Rectangle(0, 0, Main.screenWidth * 2, Main.screenHeight * 2), Color.Cyan, 0, Vector2.Zero, 1, 0, 0);
            Utils.DrawBorderString(Main.spriteBatch,
                str,
                Main.ScreenSize.ToVector2() * 0.5f - size * 0.5f, Color.White, 2);

        }
    }

    public class DeformityGeneration : GenPass
    {

        public static float surfaceHeight => 0.5f;

        public static float caveHeight => 0.55f;

        public static float hellHeight => 0.9f;

        public static int surfaceTile => (int)(Main.maxTilesY * surfaceHeight);

        public static int caveTile => (int)(Main.maxTilesY * caveHeight);

        public static int hellTile => (int)(Main.maxTilesY * hellHeight);

        public static int surfaceYArea => caveTile - surfaceTile;

        public static int caveYArea => hellTile - caveTile;

        public DeformityGeneration() : base("Terrain", 1) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Generating terrain"; // Sets the text displayed for this pass
            Main.worldSurface = surfaceTile + 40;
            Main.rockLayer = caveTile;

            progress.Message = "Rust";
            GenerateLunarRusts();
            progress.Value = 0.25f;

            progress.Message = "Slicing lattices";
            GenerateCryoLattices();
            progress.Value = 0.35f;

            progress.Message = "Creating a plane";
            GenerateAstraPlane();
            progress.Value = 0.4f;

            progress.Message = "Forging the heavens";
            GenerateHeavensForge();
            progress.Value = 0.65f;

            progress.Message = "Dark tower";
            GenerateDarkCelestial();
            progress.Value = 0.75f;

            progress.Message = "Mining mercury";
            GemerateMercuryBasins();
            progress.Value = 0.85f;

            progress.Message = "Melting the depths";
            GenerateCosmicEmberDepths();
            progress.Value = 1f;


            Main.spawnTileY = surfaceTile - 10;
            Main.spawnTileX = (int)(Main.maxTilesX * 0.5f);
        }

        public static void GenerateLunarRusts()
        {
            Rectangle rect = new Rectangle(0, surfaceTile + 10, Main.maxTilesX, hellTile - surfaceTile);
            Rectangle topRect = new Rectangle(0, surfaceTile, Main.maxTilesX, surfaceYArea);
            PerlinGeneration(rect, tileType: TileID.LunarRustBrick, wallType: WallID.LunarRustBrickWall, noiseStrength: 0.2f, ease: PerlinEase.EaseOutBottom);
            PerlinSurface(topRect, TileID.LunarRustBrick, perlinBottom: true);
            int lavaHeight = rect.Y + (int)(rect.Height * 0.9f);
            int ditherAmt = 30;
            bool placeGrass = true;
            for (int i = 0; i <= Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                    if (j < lavaHeight - ditherAmt)
                    {
                        Tile left = CalamityUtils.ParanoidTileRetrieval(i - 1, j);
                        Tile right = CalamityUtils.ParanoidTileRetrieval(i + 1, j);
                        Tile top = CalamityUtils.ParanoidTileRetrieval(i, j - 1);
                        if (placeGrass && t.HasTile)
                        {
                            t.TileType = TileID.LunarRustBrick;
                            placeGrass = false;
                        }
                        if (!placeGrass && ((left.HasTile && right.HasTile && top.HasTile) || (j > surfaceTile + topRect.Height + 1)))
                        {
                            t.WallType = WallID.LunarRustBrickWall;
                        }
                    }
                    else if (j > lavaHeight + ditherAmt)
                    {
                        t.WallType = WallID.CosmicEmberBrickWall;
                    }
                    else
                    {
                        float emberChance = MathHelper.Lerp(0, 100, Utils.GetLerpValue(lavaHeight - ditherAmt, lavaHeight + ditherAmt, j, true));
                        ushort type = (WorldGen.genRand.Next(100) < emberChance) ? WallID.CosmicEmberBrickWall : WallID.LunarRustBrickWall;
                        t.WallType = type;
                    }
                }
                placeGrass = true;
            }
        }

        public static void GenerateCryoLattices()
        {
            int latticeWidth = (int)(Main.maxTilesX * 0.28f);
            int biomeWidth = (int)(Main.maxTilesX * 0.4f);
            int biomeHeight = caveTile + (caveTile - surfaceTile);
            Point point1 = new Point(latticeWidth, surfaceTile);
            Point point2 = new Point(latticeWidth, biomeHeight);
            Point point3 = new Point(biomeWidth, biomeHeight);
            for (int i = 0; i < biomeWidth; i++)
            {
                if (i < latticeWidth)
                {
                    for (int j = surfaceTile; j < biomeHeight; j++)
                    {
                        Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                        if (t.TileType == TileID.LunarRustBrick)
                        {
                            t.TileType = TileID.CryocoreBrick;
                        }
                        if (t.WallType == WallID.LunarRustBrickWall)
                        {
                            t.WallType = WallID.CryocoreBrickWall;
                        }
                    }
                    for (int j = 0; j < surfaceTile; j++)
                    {
                        if (WorldGen.genRand.NextBool(10000))
                        {
                            Point spikeOrigin = new(i, j);
                            int spikeWidth = WorldGen.genRand.NextBool(5) ? WorldGen.genRand.Next(10, 30) : WorldGen.genRand.Next(4, 10);
                            int spikeHeight = (int)(spikeWidth * WorldGen.genRand.NextFloat(2, 5));

                            for (int k = spikeOrigin.X - spikeWidth * 2; k < spikeOrigin.X + spikeWidth * 2; k++)
                            {
                                for (int l = spikeOrigin.Y - spikeHeight * 2; l < spikeOrigin.Y + spikeHeight * 2; l++)
                                {
                                    if (WithinRhombus(spikeOrigin, new Point(spikeWidth, spikeHeight * 2), new Point(k, l)))
                                    {
                                        Tile t = CalamityUtils.ParanoidTileRetrieval(k, l);
                                        t.ResetToType(TileID.CryocoreBrick);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < biomeHeight; j++)
                    {
                        if (WithinTriangle(point1, point2, point3, new Point(i, j)))
                        {
                            Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                            if (t.TileType == TileID.LunarRustBrick)
                            {
                                t.TileType = TileID.CryocoreBrick;
                            }
                            if (t.WallType == WallID.LunarRustBrickWall)
                            {
                                t.WallType = WallID.CryocoreBrickWall;
                            }
                        }
                    }
                }
            }

        }

        public static void GenerateAstraPlane()
        {
            int start = (int)(Main.maxTilesX * 0.72f);
            int height = (int)(0.5f * (Main.maxTilesX - start));
            Rectangle area = new Rectangle(start, surfaceTile - height / 2, Main.maxTilesX - start, height);
            Point anchor = area.Center;
            for (int i = start; i < Main.maxTilesX; i++)
            {
                for (int j = area.Top; j < area.Bottom; j++)
                {
                    if (WithinElipse(i, j, anchor.X, anchor.Y, area.Width / 2, area.Height / 2))
                    {
                        Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                        if (t.TileType == TileID.LunarRustBrick)
                        {
                            t.TileType = TileID.AstraBrick;
                        }
                        if (t.WallType == WallID.LunarRustBrickWall)
                        {
                            t.WallType = WallID.AstraBrickWall;
                        }
                    }
                }
            }
        }

        public static void GenerateHeavensForge()
        {
            Rectangle areaRect = new Rectangle((int)(Main.maxTilesX * 0.33f), (int)(Main.maxTilesY * 0.1f), (int)(Main.maxTilesX * 0.33f), (int)(Main.maxTilesY * 0.066f));
            Point anchor = new Point(areaRect.Center.X, areaRect.Center.Y);
            int radius = areaRect.Height / 2;
            for (int i = anchor.X - radius; i < anchor.X + radius; i++)
            {
                for (int j = anchor.Y - radius;  j < anchor.Y + radius; j++)
                {
                    Point cur = new Point(i, j);
                    if (cur.ToVector2().Distance(anchor.ToVector2()) < radius)
                    {
                        CalamityUtils.ParanoidTileRetrieval(i, j).ResetToType(TileID.HeavenforgeBrick);

                    }
                }
            }
            float puffAmt = 100;
            for (int k = 0; k < puffAmt; k++)
            {
                Point pos = new Point((int)MathHelper.Lerp(areaRect.X, areaRect.X + areaRect.Width, k / (float)(puffAmt - 1)), areaRect.Center.Y + (int)WorldGen.genRand.NextFloat(-radius * 0.8f, radius * 0.8f));
                int smallRadius = (int)MathHelper.Lerp((int)(radius * 0.2f), radius, Utils.PingPongFrom01To010(k / (float)(puffAmt - 1)));
                for (int i = pos.X - smallRadius; i < pos.X + smallRadius; i++)
                {
                    for (int j = pos.Y - smallRadius; j < pos.Y + smallRadius; j++)
                    {
                        if (CalamityUtils.ParanoidTileRetrieval(i, j).HasTile)
                            continue;
                        Point cur = new Point(i, j);
                        if (cur.ToVector2().Distance(pos.ToVector2()) < smallRadius)
                        {
                            CalamityUtils.ParanoidTileRetrieval(i, j).ResetToType(TileID.HeavenforgeBrick);
                        }
                    }
                }
            }
        }

        public static void GenerateDarkCelestial()
        {
            int start = (int)(Main.maxTilesX * 0.28f);
            for (int i = start; i < Main.maxTilesX - start; i++)
            {
                for (int j = surfaceTile; j < caveTile + 30; j++)
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                    if (t.HasTile && t.TileType != TileID.CryocoreBrick)
                    {
                        t.TileType = TileID.DarkCelestialBrick;
                    }
                    if (t.WallType != WallID.None && t.WallType != WallID.CryocoreBrickWall)
                    {
                        t.WallType = WallID.DarkCelestialBrickWall;
                    }
                }
            }
        }

        public static void GemerateMercuryBasins()
        {
            for (int l = 0; l < 3; l++)
            {
                int mercMin = (int)(Main.maxTilesX * MathHelper.Lerp(0.1f, 0.7f, l / 2f));
                int mercMax = (int)(Main.maxTilesX * MathHelper.Lerp(0.3f, 0.9f, l / 2f));
                int mercX = WorldGen.genRand.Next(mercMin, mercMax);
                int mercY = (int)((hellTile - caveTile) * WorldGen.genRand.NextFloat(0.4f, 0.6f) + caveTile);
                int primeMercRad = (int)((hellTile - caveTile) * WorldGen.genRand.NextFloat(0.1f, 0.2f));
                Rectangle mercRect = new Rectangle(mercX - primeMercRad, mercY - primeMercRad, primeMercRad * 2, primeMercRad * 2);
                for (int i = mercRect.X; i < mercRect.Right; i++)
                {
                    for (int j = mercRect.Y; j < mercRect.Bottom; j++)
                    {
                        Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                        if (WithinElipse(i, j, mercX, mercY, primeMercRad, primeMercRad))
                        {
                            t.TileType = TileID.MercuryBrick;
                            t.WallType = WallID.MercuryBrickWall;
                        }
                    }
                }
                mercRect.Inflate(mercRect.Width / 2, mercRect.Height / 2);
                for (int k = 0; k < 100; k++)
                {
                    Point newPos = (mercRect.Center() + WorldGen.genRand.NextVector2Circular(mercRect.Width / 3, mercRect.Height / 3)).ToPoint();
                    int radius = (int)MathHelper.Lerp((int)(primeMercRad * 0.05f), (int)(primeMercRad * 0.5f), Utils.GetLerpValue(mercRect.Width / 2, 0, newPos.ToVector2().Distance(mercRect.Center()), true));


                    PerlinGeneration(area: new Rectangle(newPos.X - radius, newPos.Y - radius, radius * 2, radius * 2), noiseSize: Vector2.One * 400, overrideTiles: true, eraseWalls: false, tileType: TileID.MercuryBrick, wallType: WallID.MercuryBrickWall, tileCondition: (Point p) => WithinElipse(p.X, p.Y, newPos.X, newPos.Y, radius, radius));

                    /*for (int i = newPos.X - radius; i < newPos.X + radius; i++)
                    {
                        for (int j = newPos.Y - radius; j < newPos.Y + radius; j++)
                        {
                            Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                            if (WithinElipse(i, j, newPos.X, newPos.Y, radius, radius))
                            {
                                t.TileType = TileID.MercuryBrick;
                                t.WallType = WallID.MercuryBrickWall;
                            }
                        }
                    }*/
                }
            }
        }

        public static void GenerateCosmicEmberDepths()
        {
            int halfWidth = (int)(Main.maxTilesX * 0.3f);
            int start = Main.maxTilesX / 2 - halfWidth;
            int end = Main.maxTilesX / 2 + halfWidth;
            Point top = new Point(Main.maxTilesX / 2, hellTile);
            Point left = new Point(start, Main.maxTilesY);
            Point right = new Point(end, Main.maxTilesY);
            int cut = hellTile + (int)((Main.maxTilesY - hellTile) * 0.4f);
            for (int i = start; i < end; i++)
            {
                for (int j = hellTile; j < Main.maxTilesY; j++)
                {
                    if (j > cut)
                    {
                        if (WithinTriangle(top, left, right, new Point(i, j)))
                        {
                            Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                            t.ResetToType(TileID.CosmicEmberBrick);
                        }
                    }
                }
            }
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = cut + 2; j < Main.maxTilesY; j++)
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                    t.LiquidAmount = 255;
                    t.LiquidType = LiquidID.Lava;
                }
            }
        }
    }
}