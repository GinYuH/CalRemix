using CalamityMod;
using CalamityMod.DataStructures;
using CalRemix.Content.Items.Misc;
using CalRemix.Content.NPCs;
using CalRemix.Content.NPCs.Subworlds.GreatSea;
using CalRemix.Content.NPCs.Subworlds.OvergrowthRainforest;
using CalRemix.Content.Tiles;
using CalRemix.Content.Tiles.Subworlds.GreatSea;
using CalRemix.Core.Biomes;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using Steamworks;
using SubworldLibrary;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace CalRemix.Core.Subworlds
{
    public class OvergrowthRainforestSubworld : Subworld, ICustomSpawnSubworld, IDisableOcean, IFixDrawBlack
    {
        public List<(int, float, Predicate<NPCSpawnInfo>)> Spawns()
        {
            List<(int, float, Predicate<NPCSpawnInfo>)> list = [];
            // Main Jungle
            list.Add(item: (ModContent.NPCType<LionDogMoth>(), 0.6f, (NPCSpawnInfo n) => true));
            list.Add(item: (ModContent.NPCType<LargeStinkbug>(), 16f, (NPCSpawnInfo n) => Main.tile[n.SpawnTileX, n.SpawnTileY + 1].HasTile));
            list.Add(item: (ModContent.NPCType<Chimp>(), 16f, (NPCSpawnInfo n) => Main.tile[n.SpawnTileX, n.SpawnTileY + 1].HasTile));
            list.Add(item: (NPCID.GreenDragonfly, 0.4f, (NPCSpawnInfo n) => true));
            list.Add(item: (NPCID.Stinkbug, 5f, (NPCSpawnInfo n) => true));

            // Temple
            return list;
        }

        public override int Height => 1300;
        public override int Width => 4400;
        public override List<GenPass> Tasks =>
        [
            new OvergrowthRainforestGeneration()
        ];

        int ICustomSpawnSubworld.MaxSpawns { get => 14; }
        float ICustomSpawnSubworld.SpawnMult { get => 0.1f; }

        bool ICustomSpawnSubworld.OverrideVanilla { get => true; }

        public static void SaveData(string world)
        {
            TagCompound savedWorldData = CalRemixHelper.SaveCommonSubworldBools();
            CalRemixHelper.MakeTag(ref savedWorldData, "Yahora", RemixDowned.downedYahora);
            CalRemixHelper.MakeTag(ref savedWorldData, "Greed", RemixDowned.downedGreed);

            SubworldSystem.CopyWorldData("RemixCommonBools_" + world, savedWorldData);
        }

        public static void LoadData(string world)
        {
            TagCompound savedWorldData = CalRemixHelper.LoadCommonSubworldBools(world);
            RemixDowned.downedYahora = savedWorldData.GetBool("Yahora");
            RemixDowned.downedGreed = savedWorldData.GetBool("Greed");
        }

        public override void CopyMainWorldData() => SaveData("Main");

        public override void ReadCopiedMainWorldData() => LoadData("Main");

        public override void CopySubworldData() => SaveData("OvergrowthRainforest");

        public override void ReadCopiedSubworldData() => LoadData("OvergrowthRainforest");

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void DrawMenu(GameTime gameTime)
        {
            base.DrawMenu(gameTime);
            if (WorldGenerator.CurrentGenerationProgress == null)
                return;
            string str = "Progress: " + WorldGenerator.CurrentGenerationProgress.Message + " " + Math.Round(WorldGenerator.CurrentGenerationProgress.Value * 100, 2) + "%";
            Vector2 size = FontAssets.MouseText.Value.MeasureString(str) * 2;
            Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, Vector2.Zero, new Rectangle(0, 0, Main.screenWidth * 2, Main.screenHeight * 2), Color.ForestGreen, 0, Vector2.Zero, 1, 0, 0);
            Utils.DrawBorderString(Main.spriteBatch,
                str,
                Main.ScreenSize.ToVector2() * 0.5f - size * 0.5f, Color.White, 2);

        }
    }
    public class OvergrowthRainforestGeneration : GenPass
    {
        public static float groundLevel = 0.7f;
        public static float treeTopLevel = 0.05f;
        public static float templePosition = 0.22f;

        public OvergrowthRainforestGeneration() : base("Terrain", 1) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Generating terrain"; // Sets the text displayed for this pass
            Main.worldSurface = Main.maxTilesY - 42; // Hides the underground layer just out of bounds
            Main.rockLayer = Main.maxTilesY; // Hides the cavern layer way out of bounds

            int spawnX = (int)(Main.maxTilesX * 0.95f);
            Main.spawnTileX = spawnX;

            // Basic tiles
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    if (i < Main.maxTilesX * templePosition)
                    {
                        if (j > Main.maxTilesY * treeTopLevel)
                        {
                            {
                                CalamityUtils.ParanoidTileRetrieval(i, j).ResetToType(TileID.StoneSlab);
                            }
                        }
                    }
                }
            }

            progress.Set(0.25f);

            CalRemixHelper.PerlinSurface(new Rectangle((int)(Main.maxTilesX * templePosition), (int)(Main.maxTilesY * groundLevel), Main.maxTilesX, (int)(Main.maxTilesY * (1 - groundLevel))), TileID.Mud, variance: 30);

            progress.Set(0.5f);

            PlaceTreeDungeon();

            SpreadGrass();

            progress.Set(0.75f);

            FinalizeGen();

            progress.Set(1f);
        }

        public static void SpreadGrass()
        {
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                    if (t.TileType == TileID.Mud && t.HasTile)
                    {
                        bool hasAir = false;
                        for (int k = i - 1; k < i + 2; k++)
                        {
                            if (hasAir)
                                break;
                            for (int l = j - 1; l < j + 2; l++)
                            {
                                if (l == j && i == k)
                                {
                                    continue;
                                }
                                if (!CalamityUtils.ParanoidTileRetrieval(k, l).HasTile)
                                {
                                    hasAir = true;
                                    break;
                                }
                            }
                        }
                        if (hasAir)
                        {
                            CalamityUtils.ParanoidTileRetrieval(i, j).ResetToType(TileID.JungleGrass);
                        }
                    }
                }
            }
        }

        public static void FinalizeGen()
        {
            // Spawn position
            for (int j = (int)(Main.maxTilesY * 0.6f); j < Main.maxTilesY; j++)
            {
                if (CalamityUtils.ParanoidTileRetrieval(Main.spawnTileX, j).HasTile)
                {
                    Main.spawnTileY = j - 1;
                    break;
                }
            }

            RandomSubworldDoors.GenerateDoorRandom(ModContent.TileType<OvergrowthRainforestDoor>());
        }

        public static void PlaceTreeDungeon()
        {
            int treeCd = 0;
            int padding = 250;
            int dungeon = (int)(Main.maxTilesX * templePosition);
            int top = (int)(Main.maxTilesY * treeTopLevel);
            int stuffInGround = 50;
            for (int i = dungeon + padding * 2; i < Main.maxTilesX - padding; i++)
            {
                if (treeCd <= 0 && WorldGen.genRand.NextBool(40))
                {
                    CalRemixHelper.FindTopTile(i, out int y, TileID.Mud, top + 100);

                    int treeDir = WorldGen.genRand.NextBool().ToDirectionInt();

                    int endY = top + (int)(Main.maxTilesX * WorldGen.genRand.NextFloat(0.03f, 0.1f));
                    int endX = i + WorldGen.genRand.Next(40, 100) * treeDir;

                    int treeHeight = y - endY;

                    int topCtrlX = i + WorldGen.genRand.Next(100, 160) * -treeDir;
                    int topCtrlY = endY + (int)(WorldGen.genRand.NextFloat(0.3f, 0.4f) * treeHeight);

                    int bottomCtrlX = i + WorldGen.genRand.Next(180, 250) * treeDir;
                    int bottomCtrlY = endY + (int)(WorldGen.genRand.NextFloat(0.7f, 0.8f) * treeHeight);

                    BezierCurve curve = new(new Vector2(i, y), new Vector2(bottomCtrlX, bottomCtrlY), new Vector2(topCtrlX, topCtrlY), new Vector2(endX, endY));

                    List<Vector2> treePoints = curve.GetPoints(400);
                    int branchCount = WorldGen.genRand.Next(1, 4);
                    List<int> branchIndices = new();
                    int branchSmall = (int)(treePoints.Count * 0.3f);
                    int branchTall = (int)(treePoints.Count * 0.6f);
                    for (int k = 1; k <= branchCount; k++)
                    {
                        branchIndices.Add((int)MathHelper.Lerp(branchSmall, branchTall, k / branchCount) + WorldGen.genRand.Next(-25, 25));
                    }

                    for (int p = 0; p < treePoints.Count; p++)
                    {
                        Point tp = treePoints[p].ToPoint();

                        int treeRadMax = WorldGen.genRand.Next(60, 100);
                        int treeRadMin = (int)(treeRadMax * WorldGen.genRand.NextFloat(0.2f, 0.4f));

                        int pointRad = (int)MathHelper.Lerp(treeRadMax, treeRadMin, p / (float)treePoints.Count);

                        for (int k = tp.X - pointRad; k < tp.X + pointRad; k++)
                        {
                            for (int l = tp.Y - pointRad; l < tp.Y + pointRad; l++)
                            {
                                if (k < 0 || k >= Main.maxTilesX || l < 0 || l >= Main.maxTilesY)
                                    continue;
                                float dist = Vector2.Distance(new Vector2(k, l), treePoints[p]);
                                if (dist < pointRad)
                                {
                                    Tile log = CalamityUtils.ParanoidTileRetrieval(k, l + stuffInGround);
                                    log.ResetToType(TileID.LivingMahogany);
                                    if (dist < pointRad - 3)
                                    {
                                        log.WallType = WallID.LivingWoodUnsafe;
                                    }
                                }
                            }
                        }

                        if (p == treePoints.Count - 1)
                        {
                            int bushWidth = WorldGen.genRand.Next(500, 800);
                            int bushHeight = (int)(bushWidth * WorldGen.genRand.NextFloat(0.2f, 0.4f));
                            MakeBush(treePoints[p].X, treePoints[p].Y, bushWidth, bushHeight, true);
                        }

                        if (branchIndices.Contains(p))
                        {
                            int sign = WorldGen.genRand.NextBool().ToDirectionInt();
                            Point startPoint = treePoints[p].ToPoint();
                            Point extraPoint = treePoints[p].ToPoint() - new Point(0, WorldGen.genRand.Next(60, 80));
                            Point endPoint = ((-Vector2.UnitY.RotatedBy(-sign * MathHelper.ToRadians(Main.rand.NextFloat(60, 80)))) * WorldGen.genRand.Next(250, 300)).ToPoint() + startPoint;

                            Rectangle branchBounds = Utils.BoundingRectangle([startPoint, startPoint, endPoint, extraPoint]);

                            for (int k = branchBounds.Left; k < branchBounds.Right; k++)
                            {
                                for (int l = branchBounds.Top; l < branchBounds.Bottom; l++)
                                {
                                    Tile branchTile = CalamityUtils.ParanoidTileRetrieval(k, l);
                                    if (branchTile.HasTile)
                                        continue;
                                    if (CalRemixHelper.WithinTriangle(endPoint, startPoint, extraPoint, new Point(k, l)))
                                    {
                                        branchTile.ResetToType(TileID.LivingMahogany);
                                    }
                                }
                            }
                            MakeBush(endPoint.X, endPoint.Y, WorldGen.genRand.Next(250, 300), WorldGen.genRand.Next(100, 120));
                        }
                    }

                    for (int p = 0; p < treePoints.Count; p++)
                    {
                        Point tp = treePoints[p].ToPoint();

                        int treeRadMax = WorldGen.genRand.Next(40, 50);
                        int treeRadMin = (int)(treeRadMax * WorldGen.genRand.NextFloat(0.2f, 0.4f));

                        int pointRad = (int)MathHelper.Lerp(treeRadMax, treeRadMin, p / (float)treePoints.Count);

                        for (int k = tp.X - pointRad; k < tp.X + pointRad; k++)
                        {
                            for (int l = tp.Y - pointRad; l < tp.Y + pointRad; l++)
                            {
                                if (k < 0 || k >= Main.maxTilesX || l < 0 || l >= Main.maxTilesY)
                                    continue;
                                float dist = Vector2.Distance(new Vector2(k, l), treePoints[p]);
                                if (dist < pointRad)
                                {
                                    if (l < y - 40)
                                        CalamityUtils.ParanoidTileRetrieval(k, l + stuffInGround).HasTile = false;
                                }
                            }
                        }
                    }
                    treeCd = padding * 2;
                }
                treeCd--;
            }
        }

        public static void MakeBush(float i, float j, int sizeX, int sizeY, bool overrideWalls = false)
        {
            Rectangle foliage = Utils.CenteredRectangle(new Vector2(i, j), new Vector2(sizeX, sizeY));

            for (int k = foliage.Left; k < foliage.Right; k++)
            {
                for (int l = foliage.Top; l < foliage.Bottom; l++)
                {
                    if (CalamityUtils.ParanoidTileRetrieval(k, l).WallType > WallID.None && !overrideWalls)
                    {
                        continue;
                    }
                    if (CalRemixHelper.WithinElipse(k, l, foliage.Center.X, foliage.Center.Y, foliage.Width / 2, foliage.Height / 2))
                    {
                        CalamityUtils.ParanoidTileRetrieval(k, l).ResetToType(TileID.LivingMahoganyLeaves);

                        if (WorldGen.genRand.NextBool(WorldGen.genRand.Next(200, 300)))
                        {
                            int leafBallRad = (int)(WorldGen.genRand.NextFloat(sizeX * 0.05f, sizeX * 0.1f));
                            for (int m = k - leafBallRad; m < k + leafBallRad; m++)
                            {
                                for (int n = l - leafBallRad; n < l + leafBallRad; n++)
                                {
                                    if (new Vector2(k, l).Distance(new Vector2(m, n)) < leafBallRad)
                                    {
                                        CalamityUtils.ParanoidTileRetrieval(m, n).ResetToType(TileID.LivingMahoganyLeaves);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}