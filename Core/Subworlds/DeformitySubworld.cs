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
        public override int Height => 900;
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

        public static float caveHeight => 0.6f;

        public static float hellHeight => 0.8f;

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


            Main.spawnTileY = surfaceTile;
            Main.spawnTileX = (int)(Main.maxTilesX * 0.5f);
        }

        public static void GenerateLunarRusts()
        {
            Rectangle rect = new Rectangle(0, surfaceTile + 10, Main.maxTilesX, hellTile - surfaceTile);
            Rectangle topRect = new Rectangle(0, surfaceTile, Main.maxTilesX, surfaceYArea);
            PerlinGeneration(rect, tileType: TileID.LunarRustBrick, wallType: WallID.LunarRustBrickWall, noiseStrength: 0.2f);
            for (int i = rect.X; i < rect.X + rect.Width; i++)
            {
                for (int j = rect.Y; j < rect.Y + rect.Height; j++)
                {
                    if (CalamityUtils.ParanoidTileRetrieval(i, j).WallType == WallID.None)
                    {
                        CalamityUtils.ParanoidTileRetrieval(i, j).WallType = WallID.LunarRustBrickWall;
                    }
                }
            }
            PerlinSurface(topRect, TileID.LunarRustBrick, perlinBottom: true);
        }

        public static void GenerateCryoLattices()
        {
            int width = (int)(Main.maxTilesX * 0.28f);
            for (int i = 0; i < width; i++)
            {
                for (int j = surfaceTile; j < caveTile + 30; j++)
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                    if (t.TileType == TileID.LunarRustBrick)
                    {
                        t.TileType = TileID.CryocoreBrick;
                    }
                    if (t.WallType == WallID.LunarRustBrickWall)
                    {
                        t.WallType = WallID.LunarRustBrickWall;
                    }
                }

                for (int j = 0; j < surfaceTile; j++)
                {
                    if (WorldGen.genRand.NextBool(20000))
                    {
                        Point spikeOrigin = new(i, j);
                        int spikeWidth = Main.rand.Next(4, 30);
                        int spikeHeight = (int)(spikeWidth * Main.rand.NextFloat(2, 5));

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

        }

        public static void GenerateAstraPlane()
        {
            int start = (int)(Main.maxTilesX * 0.72f);
            for (int i = start; i < Main.maxTilesX; i++)
            {
                for (int j = surfaceTile; j < caveTile + 30; j++)
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

        public static void GenerateHeavensForge()
        {
            Rectangle areaRect = new Rectangle((int)(Main.maxTilesX * 0.33f), (int)(Main.maxTilesY * 0.2f), (int)(Main.maxTilesX * 0.33f), (int)(Main.maxTilesY * 0.1f));
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
                    if (t.HasTile)
                    {
                        t.TileType = TileID.DarkCelestialBrick;
                    }
                    if (t.WallType != WallID.None)
                    {
                        t.WallType = WallID.DarkCelestialBrickWall;
                    }
                }
            }
        }

        public static void GemerateMercuryBasins()
        {

        }

        public static void GenerateCosmicEmberDepths()
        {

        }
    }
}