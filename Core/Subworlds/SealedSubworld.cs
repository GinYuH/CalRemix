using System.Collections.Generic;
using Terraria;
using SubworldLibrary;
using Terraria.WorldBuilding;
using Terraria.IO;
using Terraria.ModLoader;
using CalamityMod.Tiles.FurnitureExo;
using System;
using CalamityMod.Tiles.Ores;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Terraria.ID;
using CalRemix.Core.World;
using CalRemix.Content.Tiles;
using Terraria.Graphics.Effects;
using Microsoft.Build.Tasks.Deployment.ManifestUtilities;
using CalamityMod;
using Terraria.Audio;
using System.Configuration;
using Iced.Intel;
using CalRemix.Content.NPCs.Subworlds.GreatSea;
using CalRemix.Core.Biomes;
using CalamityMod.NPCs.SulphurousSea;
using CalRemix.Content.NPCs.PandemicPanic;
using CalRemix.Content.Tiles.Subworlds.GreatSea;
using Terraria.Utilities;
using CalamityMod.DataStructures;
using CalamityMod.Schematics;
using Humanizer;
using Microsoft.Build.Tasks;
using CalRemix.Content.Tiles.Subworlds.Sealed;
using CalRemix.Content.Walls;
using CalRemix.Content.Items.Potions.Tainted;
using Terraria.ModLoader.Default;
using CalRemix.Content.NPCs.Bosses.Origen;
using Terraria.GameContent.Biomes.CaveHouse;

namespace CalRemix.Core.Subworlds
{
    public class SealedSubworld : Subworld, IDisableOcean, IFixDrawBlack, ICustomSpawnSubworld
    {
        public List<(int, float, Predicate<NPCSpawnInfo>)> Spawns()
        {
            List<(int, float, Predicate<NPCSpawnInfo>)> list = [];
            //list.Add(item: (ModContent.NPCType<BullShark>(), 0.1f, (NPCSpawnInfo n) => n.Player.InModBiome<GreatSeaBiome>()));
            return list;
        }

        int ICustomSpawnSubworld.MaxSpawns { get => 8; }
        float ICustomSpawnSubworld.SpawnMult { get => 0.2f; }

        bool ICustomSpawnSubworld.OverrideVanilla { get => false; }

        public override int Height => 1000;
        public override int Width => 3000;
        public override List<GenPass> Tasks =>
        [
            new SealedGeneration()
        ];

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void Update()
        {
            Main.LocalPlayer.ManageSpecialBiomeVisuals("CalRemix:Sealed", true);
            SkyManager.Instance.Activate("CalRemix:Sealed", Main.LocalPlayer.position);
            Main.time = Main.dayLength * 0.5f;
            base.Update();
            Liquid.UpdateLiquid();
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

        public override bool GetLight(Tile tile, int x, int y, ref FastRandom rand, ref Vector3 color)
        {
            ushort carnWall = (ushort)ModContent.WallType<UnsafeCarnelianStoneWallPlaced>();
            ushort voidWall = (ushort)ModContent.WallType<VoidInfusedStoneWallPlaced>();
            ushort fabricWall = (ushort)ModContent.WallType<GreenFabricWallPlaced>();

            if (!tile.HasTile)
            {
                if (tile.WallType == voidWall)
                {
                    color.X = 1;
                    color.Y = 1;
                    color.Z = 1;
                }
                else if (tile.WallType == carnWall)
                {
                    color.X = 0.4f;
                    color.Y = 0;
                    color.Z = 0.05f;
                }
                else if (tile.WallType == fabricWall)
                {
                    float lightLevel = 0.6f;
                    color.X = lightLevel;
                    color.Y = lightLevel;
                    color.Z = lightLevel;
                }
            }
            return base.GetLight(tile, x, y, ref rand, ref color);
        }
    }

    public class SealedGeneration : GenPass
    {
        #region Surface Ratios
        public static float seaDist => 0.05f;

        public static float darnWoodWidth => 0.05f;

        public static float lavaWidth => 0.15f;

        public static float barrenWidth => 0.15f;

        public static float turnipWidth => 0.15f;

        public static float cragWidth => 0.15f;

        public static float fieldWidth => 1 - (seaDist * 2 + darnWoodWidth * 2 + lavaWidth + cragWidth + barrenWidth + turnipWidth);

        public static float villageWidth => fieldWidth * 0.6f;
        #endregion


        #region Surface positions

        public static int lavaPosition => (int)(Main.maxTilesX * (seaDist + darnWoodWidth));

        public static int barrenPosition => lavaPosition + (int)(Main.maxTilesX * lavaWidth);

        public static int cragPosition => barrenPosition + (int)(Main.maxTilesX * barrenWidth);

        public static int fieldPosition => cragPosition + (int)(Main.maxTilesX * cragWidth);

        public static int villagePosition => fieldPosition + (int)(Main.maxTilesX * fieldWidth) - (int)(Main.maxTilesX * villageWidth);

        public static int turnipPosition => villagePosition + (int)(Main.maxTilesX * villageWidth);

        #endregion

        #region Vertical surface ratios

        public static float surfaceHeight => 0.3f;

        public static float caveHeight => 0.45f;

        public static int surfaceTile => (int)(Main.maxTilesY * surfaceHeight);

        public static int caveTile => (int)(Main.maxTilesY * caveHeight);

        public static int surfaceYArea => caveTile - surfaceTile;
        #endregion


        public SealedGeneration() : base("Terrain", 1) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Generating terrain"; // Sets the text displayed for this pass
            Main.worldSurface = Main.maxTilesY - 42; // Hides the underground layer just out of bounds
            Main.rockLayer = Main.maxTilesY; // Hides the cavern layer way out of bounds

            progress.Message = "Erosion";
            GenerateBase();
            progress.Message = "Swamping";
            progress.Value = 0.1f;
            GenerateDarnwood();
            progress.Message = "Making the world more barren";
            progress.Value = 0.15f;
            GenerateBarrens();
            progress.Message = "Cragging things up";
            progress.Value = 0.25f;
            GenerateCrags();
            progress.Message = "Growing turnips";
            progress.Value = 0.35f;
            GenerateTurnips();
            progress.Message = "Erupting a volcano";
            progress.Value = 0.45f;
            GenerateVolcano();
            progress.Message = "Building a village";
            progress.Value = 0.55f;
            GenerateVillage();
            progress.Message = "Making things more red";
            progress.Value = 0.75f;
            GenerateCarnelian();
            progress.Message = "Void";
            progress.Value = 0.85f;
            GenerateVoid();
            progress.Message = "Laying out the bottom";
            progress.Value = 0.95f;
            GenerateBedrock();
            progress.Value = 1f;

            

            Main.spawnTileY = surfaceTile;
            Main.spawnTileX = fieldPosition + (int)(Main.maxTilesX * fieldWidth * 0.3f);
        }

        public static void GenerateBase()
        {
            ushort stone = (ushort)ModContent.TileType<SealedStonePlaced>();
            ushort dirt = (ushort)ModContent.TileType<SealedDirtPlaced>();
            ushort grass = (ushort)ModContent.TileType<SealedGrassPlaced>();
            ushort stoneWall = (ushort)ModContent.WallType<UnsafeSealedStoneWallPlaced>();
            ushort dirtWall = (ushort)ModContent.WallType<UnsafeSealedDirtWallPlaced>();
            int leftOceanStop = (int)(Main.maxTilesX * seaDist);
            int rightOceanStop = Main.maxTilesX - leftOceanStop;
            int variance = 50;
            int surface = (int)(Main.maxTilesY * surfaceHeight) + variance;
            int stretch = (int)(variance * 1f);
            Rectangle caveRect = new Rectangle(leftOceanStop, surface - stretch + variance, Main.maxTilesX - leftOceanStop * 2, Main.maxTilesY - surface + stretch - variance);
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = surface - variance; j < Main.maxTilesY; j++)
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                    if (i < leftOceanStop || i >= rightOceanStop)
                    {
                        t.LiquidAmount = 255;
                        t.LiquidType = LiquidID.Water;
                    }
                }
            }

            CalRemixHelper.PerlinGeneration(caveRect, noiseStrength: 0.45f, noiseThreshold: 0.8f, tileType: stone, wallType: stoneWall, ease: CalRemixHelper.PerlinEase.EaseInOut, topStop: 0.05f, bottomStop: 0.9f);
            CalRemixHelper.PerlinSurface(new Rectangle(leftOceanStop, surface - stretch, Main.maxTilesX - leftOceanStop * 2, variance + 1), dirt, variance: (int)(variance * 0.8f));

            bool placeGrass = true;
            for (int i = leftOceanStop; i <= rightOceanStop; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                    if (placeGrass && t.HasTile)
                    {
                        t.TileType = grass;
                        placeGrass = false;
                    }
                    else if(j < caveTile && t.HasTile && t.TileType != TileID.Trees)
                    {
                        t.TileType = dirt;
                    }
                    if (!placeGrass && t.TileType != grass)
                    {
                        t.WallType = (j > caveTile) ? stoneWall : dirtWall;
                    }
                }
                placeGrass = true;
            }
        }

        public static void GenerateDarnwood()
        {
            ushort dwood = (ushort)ModContent.TileType<RichMudPlaced>();
            int leftOceanStop = (int)(Main.maxTilesX * seaDist);
            int rightOceanStop = Main.maxTilesX - leftOceanStop;
            int surface = (int)(Main.maxTilesY * surfaceHeight);
            int width = (int)(Main.maxTilesX * darnWoodWidth);
            int height = (int)(Main.maxTilesY * (caveHeight - surfaceHeight));
            int padding = 60;
            Rectangle leftDarnwood = new Rectangle(leftOceanStop, surface, width + padding, height);
            Rectangle rightDarnwood = new Rectangle(rightOceanStop - width - padding, surface, width + padding, height);

            for (int i = leftDarnwood.X; i <= (leftDarnwood.X + leftDarnwood.Width); i++)
            {
                for (int j = leftDarnwood.Y; j <= (leftDarnwood.Y + leftDarnwood.Height); j++)
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                    if (t.HasTile)
                    {
                        t.ResetToType(dwood);
                    }
                    if (j > surfaceTile + 6 && i < leftDarnwood.X + leftDarnwood.Width - padding)
                    {
                        if (!t.HasTile)
                        {
                            t.LiquidAmount = 255;
                            t.LiquidType = LiquidID.Water;
                        }
                    }
                }
            }
            for (int i = rightDarnwood.X; i < (rightDarnwood.X + rightDarnwood.Width); i++)
            {
                for (int j = rightDarnwood.Y; j <= (rightDarnwood.Y + rightDarnwood.Height); j++)
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                    if (t.HasTile)
                    {
                        t.ResetToType(dwood);
                    }
                    if (j > surfaceTile + 6 && i > rightDarnwood.X + padding)
                    {
                        if (!t.HasTile)
                        {
                            t.LiquidAmount = 255;
                            t.LiquidType = LiquidID.Water;
                        }
                    }
                }
            }

            for (int i = leftDarnwood.X; i < leftDarnwood.X + leftDarnwood.Width + 1 - padding; i++)
            {
                for (int j = leftDarnwood.Y; j < leftDarnwood.Y + leftDarnwood.Height + 1; j++)
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                    Tile above = CalamityUtils.ParanoidTileRetrieval(i, j - 1);
                    if (!above.HasTile && t.TileType == dwood && t.HasTile)
                    {
                        if (Main.rand.NextBool(7))
                        {
                            CalRemixHelper.ForceGrowTree(i, j, Main.rand.Next(10, 20));
                        }
                        else if (Main.rand.NextBool(5))
                        {
                            int peatAmt = Main.rand.Next(2, 12);
                            for (int k = j - 1; k > j - peatAmt; k--)
                            {
                                if (!CalamityUtils.ParanoidTileRetrieval(i, k).HasTile)
                                    WorldGen.PlaceObject(i, k, ModContent.TileType<PeatSpirePlaced>(), true, Main.rand.Next(0, 3));
                            }
                        }
                    }
                }
            }
            for (int i = rightDarnwood.X + padding; i < rightDarnwood.X + rightDarnwood.Width + 1; i++)
            {
                for (int j = leftDarnwood.Y; j < leftDarnwood.Y + leftDarnwood.Height + 1; j++)
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                    Tile above = CalamityUtils.ParanoidTileRetrieval(i, j - 1);
                    if (!above.HasTile && t.TileType == dwood && t.HasTile)
                    {
                        if (Main.rand.NextBool(7))
                        {
                            CalRemixHelper.ForceGrowTree(i, j, Main.rand.Next(10, 20));
                        }
                        else if (Main.rand.NextBool(5))
                        {
                            int peatAmt = Main.rand.Next(2, 12);
                            for (int k = j - 1; k > j - peatAmt; k--)
                            {
                                if (!CalamityUtils.ParanoidTileRetrieval(i, k).HasTile)
                                    WorldGen.PlaceObject(i, k, ModContent.TileType<PeatSpirePlaced>(), true, Main.rand.Next(0, 3));
                            }
                        }
                    }
                }
            }
        }

        public static void GenerateBarrens()
        {
            ushort tType = (ushort)ModContent.TileType<DesoilitePlaced>();
            ushort bType = (ushort)ModContent.TileType<LightResiduePlaced>();
            int padding = 120;
            Rectangle barrenRect = new Rectangle(barrenPosition - padding, surfaceTile, (int)(Main.maxTilesX * barrenWidth) + padding * 2, surfaceYArea);
            bool top = false;

            WeightedRandom<int> blobSize = new();
            blobSize.Add(1, 1);
            blobSize.Add(2, 0.3f);
            blobSize.Add(3, 0.1f);

            int blobCooldown = 0;
            for (int i = barrenRect.X; i <= (barrenRect.X + barrenRect.Width); i++)
            {
                for (int j = barrenRect.Y; j <= (barrenRect.Y + barrenRect.Height); j++)
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                    if (t.HasTile)
                    {
                        if (t.TileType != bType)
                            t.ResetToType(tType);
                        if (!top)
                        {
                            if (blobCooldown <= 0 && Main.rand.NextBool(30) && t.TileType == tType)
                            {
                                int size = blobSize.Get();

                                for (int k = i - size; k < i + size; k++)
                                {
                                    for (int l = j - size; l < j + size; l++)
                                    {
                                        Tile blob = CalamityUtils.ParanoidTileRetrieval(k, l);
                                        blob.ResetToType(bType);
                                    }
                                }
                                blobCooldown = size * 2 + 3;
                            }
                            top = true;
                        }
                    }
                    if (j > surfaceTile + 40)
                    {
                        if (!t.HasTile)
                        {
                            t.LiquidAmount = 255;
                            t.LiquidType = LiquidID.Lava;
                        }
                    }
                }
                top = false;
                blobCooldown--;
            }
            Rectangle barrenBottom = new Rectangle(barrenPosition - padding, surfaceTile + surfaceYArea - 20, (int)(Main.maxTilesX * barrenWidth) + padding * 2, 30);
            CalRemixHelper.PerlinSurface(barrenBottom, tType, perlinBottom: true);
        }

        public static void GenerateCrags()
        {
            ushort tType = (ushort)ModContent.TileType<BadrockPlaced>();
            Point origin = new Point(cragPosition + (int)(Main.maxTilesX * 0.5f * cragWidth), surfaceTile);

            int width = (int)(Main.maxTilesX * cragWidth * 0.5f);
            int height = surfaceYArea;

            for (int i = origin.X - width; i < origin.X + width ; i++)
            {
                for (int j = origin.Y; j < origin.Y + height * 2; j++)
                {
                    if (CalRemixHelper.WithinRhombus(origin, new Point(width, height * 2), new Point(i, j)))
                    {
                        Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                        if (!t.HasTile || t.TileType == TileID.Trees)
                           continue;

                        t.ResetToType(tType);
                    }
                }
            }
            int spikeCooldown = 0;
            int spikeCount = 0;
            for (int i = origin.X - width; i < origin.X + width; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                    if (t.TileType == tType)
                    {
                        if (Main.rand.NextBool(14) && spikeCooldown <= 0)
                        {
                            int cd = 20;
                            if (spikeCount == 6)
                            {
                                bool _ = false;
                                SchematicManager.PlaceSchematic<Action<Chest>>("Bright Shrine", new Point(i, j + 4), SchematicAnchor.BottomCenter, ref _);
                                cd = 30;
                            }
                            else
                            {
                                int itMin = 4;
                                int itMax = 7;
                                int iterations = Main.rand.Next(itMin, itMax + 1);
                                int curHeight = 0;
                                for (int m = 0; m < iterations; m++)
                                {
                                    int spikeWidth = (int)(Main.rand.Next(1, 5) * MathHelper.Lerp(1, 0.2f, Utils.GetLerpValue(itMin, itMax, m, true)));
                                    int spikeHeight = (int)(spikeWidth * Main.rand.NextFloat(0.8f, 1.5f));
                                    curHeight += (int)(spikeHeight * 1.6f);
                                    Point spikeOrigin = new Point(i, j - curHeight);
                                    for (int k = spikeOrigin.X - spikeWidth * 2; k < spikeOrigin.X + spikeWidth * 2; k++)
                                    {
                                        for (int l = spikeOrigin.Y - spikeHeight * 2; l < spikeOrigin.Y + spikeHeight * 2; l++)
                                        {
                                            if (CalRemixHelper.WithinRhombus(spikeOrigin, new Point(spikeWidth * 2, spikeHeight * 2), new Point(k, l)))
                                            {
                                                Tile t2 = CalamityUtils.ParanoidTileRetrieval(k, l);
                                                t2.ResetToType(tType);
                                            }
                                        }
                                    }
                                }
                            }
                            spikeCount++;
                            spikeCooldown = cd;
                        }
                        break;
                    }
                }
                spikeCooldown--;
            }
        }

        public static void GenerateTurnips()
        {
            ushort tType = (ushort)ModContent.TileType<PorswineManurePlaced>();
            Point origin = new Point(turnipPosition + (int)(Main.maxTilesX * 0.5f * turnipWidth), surfaceTile);
            Vector2 vecOrigin = origin.ToVector2();
            int radius = (int)(Main.maxTilesX * turnipWidth * 0.5f);

            for (int i = origin.X - radius; i < origin.X + radius; i++)
            {
                for (int j = origin.Y; j < origin.Y + radius; j++)
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                    if (!t.HasTile || t.TileType == TileID.Trees)
                        continue;
                    Vector2 pt = new Vector2(i, j);
                    if (pt.Distance(vecOrigin) < radius)
                    {
                        t.ResetToType(tType);
                    }
                }
            }

            int turnipCooldown = 0;
            int turnipsPlaced = 0;
            for (int i = origin.X - radius; i < origin.X + radius; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                    if (t.TileType == tType)
                    {
                        if (Main.rand.NextBool(10) && turnipCooldown <= 0)
                        {
                            bool _ = false;
                            string chem = turnipsPlaced == 5 ? "Sealed Citadel" : "Turnip";
                            int offset = turnipsPlaced == 5 ? 3 : 2;
                            SchematicManager.PlaceSchematic<Action<Chest>>(chem, new Point(i, j + offset), SchematicAnchor.BottomCenter, ref _);
                            turnipCooldown = 30;
                            turnipsPlaced++;
                        }
                        break;
                    }
                }
                turnipCooldown--;
            }
        }

        public static void GenerateVolcano()
        {
            ushort tType = (ushort)ModContent.TileType<ActivePlumestonePlaced>();
            Point origin = new Point(lavaPosition + (int)(Main.maxTilesX * 0.5f * lavaWidth), surfaceTile);
            Vector2 vecOrigin = origin.ToVector2();
            int radius = (int)(Main.maxTilesX * lavaWidth * 0.5f);

            for (int i = origin.X - radius; i < origin.X + radius; i++)
            {
                for (int j = origin.Y; j < origin.Y + radius; j++)
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                    Vector2 pt = new Vector2(i, j);
                    if (pt.Distance(vecOrigin) < radius)
                    {
                        if (t.HasTile)
                        {
                            t.ResetToType(tType);
                        }
                        else if (j > surfaceTile + 40)
                        {
                            t.LiquidAmount = 255;
                            t.LiquidType = LiquidID.Lava;
                        }
                    }
                }
            }

            int turnipCooldown = 0;
            for (int i = origin.X - radius; i < origin.X + radius ; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                    if (t.TileType == tType)
                    {
                        if (Main.rand.NextBool(10) && turnipCooldown <= 0)
                        {
                            bool _ = false;
                            SchematicManager.PlaceSchematic<Action<Chest>>("Plumestone", new Point(i, j + 4), SchematicAnchor.BottomCenter, ref _);
                            turnipCooldown = 20;
                        }
                        break;
                    }
                }
                turnipCooldown--;
            }
        }

        public static void GenerateVillage()
        {
            int villageStart = villagePosition;
            int villageEnd = villagePosition + (int)(Main.maxTilesX * villageWidth);

            ushort tType = (ushort)ModContent.TileType<SealedGrassPlaced>();

            int hausCooldown = 0;

            WeightedRandom<string> houseTypes = new();
            string prefix = "Sealed House ";
            houseTypes.Add(prefix + "Small", 1);
            houseTypes.Add(prefix + "Large", 0.6f);
            houseTypes.Add(prefix + "Library", 0.4f);
            houseTypes.Add(prefix + "Church", 0.2f);

            bool generatedChurch = false;
            int housesGenerated = 0;
            for (int i = villageStart; i < villageEnd; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                    if (t.TileType == tType)
                    {
                        if (Main.rand.NextBool(10) && hausCooldown <= 0)
                        {
                            string houseType = houseTypes.Get();
                            // Guarantee a church on the fifth house if one hasn't generated yet
                            if ((!generatedChurch && housesGenerated >= 4) || houseType == prefix + "Church")
                            {
                                houseType = prefix + "Church";
                                generatedChurch = true;
                            }
                            bool _ = false;
                            SchematicManager.PlaceSchematic<Action<Chest>>(houseType, new Point(i, j + 1), SchematicAnchor.BottomCenter, ref _);
                            hausCooldown = (int)(RemixSchematics.TileMaps[prefix + "Library"].GetLength(0));
                            housesGenerated++;
                        }
                        else if (Main.rand.NextBool(25))
                        {
                            int peatAmt = Main.rand.Next(3, 8);
                            for (int k = j - 1; k > j - peatAmt; k--)
                            {
                                if (!CalamityUtils.ParanoidTileRetrieval(i, k).HasTile)
                                    WorldGen.PlaceObject(i, k, ModContent.TileType<NeoncanePlaced>(), true, Main.rand.Next(0, 3));
                            }
                        }
                        break;
                    }
                }
                hausCooldown--;
            }

            Point chamberPoint = new Point(villageStart + 100, caveTile);
            bool _2 = false;
            SchematicManager.PlaceSchematic<Action<Chest>>("Sealed Chamber", chamberPoint, SchematicAnchor.Center, ref _2);
        }

        public static void GenerateCarnelian()
        {
            ushort stone = (ushort)ModContent.TileType<CarnelianStonePlaced>();
            ushort dirt = (ushort)ModContent.TileType<CarnelianDirtPlaced>();
            ushort grass = (ushort)ModContent.TileType<CarnelianGrassPlaced>();
            ushort stoneWall = (ushort)ModContent.WallType<UnsafeCarnelianStoneWallPlaced>();

            Point origin = new Point(lavaPosition + (int)(Main.maxTilesX * 0.5f * lavaWidth), surfaceTile + caveTile);

            int width = (int)(Main.maxTilesX * lavaWidth);
            int height = (int)(Main.maxTilesY * 0.5f * 0.3f);

            Rectangle heartRect = new Rectangle(origin.X - width, origin.Y - height * 2, width * 2, height * 3);

            CalRemixHelper.PerlinGeneration(heartRect, noiseStrength: 0.3f, noiseThreshold: 0.9f, tileType: stone, wallType: stoneWall, ease: CalRemixHelper.PerlinEase.EaseInOut, topStop: 0.05f, bottomStop: 0.9f, tileCondition: (Point p) => CalRemixHelper.WithinHeart(origin, new Point(width, height * 2), p), overrideTiles: true, eraseWalls: false);

            for (int i = heartRect.X; i < heartRect.X + heartRect.Width + 1; i++)
            {
                for (int j = heartRect.Y; j < heartRect.Y + heartRect.Height + 1; j++)
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                    Tile above = CalamityUtils.ParanoidTileRetrieval(i, j - 1);
                    bool growOverride = above.WallType == stoneWall && t.TileType != dirt && t.TileType != stone && t.TileType != TileID.Trees;
                    if (!above.HasTile || growOverride)
                    {
                        int dirtDist = 3;
                        int oj = j;
                        for (int k = oj; k < oj + dirtDist; k++)
                        {
                            Tile t2 = CalamityUtils.ParanoidTileRetrieval(i, k);
                            if (t2.HasTile && (t2.TileType == stone || growOverride) && t2.TileType != TileID.Trees)
                            {
                                t2.TileType = dirt;
                            }
                        }
                        if (CalamityUtils.ParanoidTileRetrieval(i, j + 1).TileType == dirt)
                        {
                            t.TileType = grass;
                        }
                        if (t.TileType == grass && t.HasTile)
                        {
                            if (Main.rand.NextBool(5))
                            {
                                CalRemixHelper.ForceGrowTree(i, j);
                            }
                            else if (Main.rand.NextBool(15))
                            {
                                WorldGen.PlaceObject(i, j - 1, ModContent.TileType<CarnelianRosePlaced>(), true);
                            }
                        }
                    }
                }
            }
        }

        public static void GenerateVoid()
        {
            ushort tType = (ushort)ModContent.TileType<VoidInfusedStonePlaced>();
            ushort wType = (ushort)ModContent.WallType<VoidInfusedStoneWallPlaced>();
            int offset = (int)(Main.maxTilesX * turnipWidth * 0.5f);
            Point origin = new Point(turnipPosition + (int)(Main.maxTilesX * 0.5f * turnipWidth) - offset, caveTile);
            int size = (int)(Main.maxTilesX * turnipWidth * 0.5f);
            int floor = 10;

            Rectangle rect = new Rectangle(origin.X, origin.Y, size, size);
            Rectangle rect2 = new Rectangle(origin.X - offset, origin.Y + offset, size, size);

            CalRemixHelper.PerlinGeneration(rect, noiseThreshold: 0.05f, noiseStrength: 0.4f, noiseSize: new Vector2(500, 500), tileType: tType, wallType: wType, eraseWalls: false, overrideTiles: true);
            CalRemixHelper.PerlinGeneration(rect2, noiseThreshold: 0.05f, noiseStrength: 0.4f, noiseSize: new Vector2(500, 500), tileType: tType, wallType: wType, eraseWalls: false, overrideTiles: true);


            for (int i = rect.X; i < rect.X + rect.Width; i++)
            {
                for (int j = rect.Y; j < rect.Y + rect.Height; j++)
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                    if (j > rect.Y + rect.Height - floor)
                    {
                        t.ResetToType(tType);
                    }
                    Tile above = CalamityUtils.ParanoidTileRetrieval(i, j - 1);
                    if (!above.HasTile && t.TileType == tType && t.HasTile)
                    {
                        if (Main.rand.NextBool(5))
                        {
                            CalRemixHelper.ForceGrowTree(i, j, Main.rand.Next(10, 40));
                        }
                    }
                }
            }
            bool placedShrine = false;
            int treesPlaecd = 0;
            for (int i = rect2.X; i < rect2.X + rect2.Width; i++)
            {
                for (int j = rect2.Y; j < rect2.Y + rect2.Height; j++)
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                    if (j > rect2.Y + rect2.Height - floor)
                    {
                        t.ResetToType(tType);
                    }
                    Tile above = CalamityUtils.ParanoidTileRetrieval(i, j - 1);
                    if (!above.HasTile && t.TileType == tType && t.HasTile)
                    {
                        if (Main.rand.NextBool(5))
                        {
                            bool tree = CalRemixHelper.ForceGrowTree(i, j, Main.rand.Next(10, 40));
                            if (tree && treesPlaecd > 5 && !placedShrine)
                            {
                                placedShrine = true;

                                bool _ = false;
                                SchematicManager.PlaceSchematic<Action<Chest>>("Monorian Shrine", new Point(i, j + 1), SchematicAnchor.BottomCenter, ref _);
                            }
                            treesPlaecd++;
                        }
                    }
                }
            }
        }

        public static void GenerateBedrock()
        {
            int height = 80;
            int baseHeight = Main.maxTilesY - height;
            CalRemixHelper.PerlinSurface(new Rectangle(0, baseHeight, Main.maxTilesX, height), ModContent.TileType<DarkstonePlaced>(), 3);

            ushort grass = (ushort)ModContent.TileType<SealedGrassPlaced>();

            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                    if (t.TileType == grass)
                    {
                        if (Main.rand.NextBool(5))
                        {
                            CalRemixHelper.ForceGrowTree(i, j);
                        }
                    }
                }
            }

            CalamityUtils.SpawnOre(ModContent.TileType<FrozenSealedTearOrePlaced>(), 14E-05, 0.55f, 0.95f, 3, 8, ModContent.TileType<SealedStonePlaced>());
            CalamityUtils.SpawnOre(ModContent.TileType<MonoriumOrePlaced>(), 12E-05, 0.45f, 0.95f, 1, 4, ModContent.TileType<SealedStonePlaced>());
            CalamityUtils.SpawnOre(ModContent.TileType<CarnelianiteOrePlaced>(), 12E-06, 0.35f, 0.95f, 1, 4, ModContent.TileType<SealedStonePlaced>());
            CalamityUtils.SpawnOre(ModContent.TileType<CarnelianiteOrePlaced>(), 12E-04, 0.35f, 0.95f, 5, 10, ModContent.TileType<CarnelianStonePlaced>());
            CalamityUtils.SpawnOre(ModContent.TileType<PeatOrePlaced>(), 12E-05, 0.25f, 0.85f, 10, 20, ModContent.TileType<SealedStonePlaced>());
            int iron = Main.rand.NextBool() ? TileID.Iron : TileID.Lead;
            CalamityUtils.SpawnOre(iron, 12E-05, 0.25f, 0.85f, 8, 12, ModContent.TileType<SealedStonePlaced>());
        }
    }
}