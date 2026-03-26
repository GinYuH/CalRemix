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
using JetBrains.Annotations;
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

            // Enemies are forced to spawn inside the tree if the player is inside as well
            Point playerPoint = Main.LocalPlayer.Center.ToTileCoordinates();
            bool playerWall = CalamityUtils.ParanoidTileRetrieval(playerPoint.X, playerPoint.Y).WallType > WallID.None;
            Predicate<NPCSpawnInfo> wallFunc = new Predicate<NPCSpawnInfo>(t => (CalamityUtils.ParanoidTileRetrieval(t.SpawnTileX, t.SpawnTileY).WallType > WallID.None && playerWall) || !playerWall);

            // Main Jungle
            list.Add(item: (ModContent.NPCType<LionDogMoth>(), 0.6f, wallFunc));
            list.Add(item: (ModContent.NPCType<LargeStinkbug>(), 16f, (NPCSpawnInfo n) => CalamityUtils.ParanoidTileRetrieval(n.SpawnTileX, n.SpawnTileY + 1).HasTile && wallFunc.Invoke(n)));
            list.Add(item: (ModContent.NPCType<Chimp>(), 16f, (NPCSpawnInfo n) => CalamityUtils.ParanoidTileRetrieval(n.SpawnTileX, n.SpawnTileY + 1).HasTile && wallFunc.Invoke(n)));
            list.Add(item: (NPCID.GreenDragonfly, 0.04f, wallFunc));
            list.Add(item: (NPCID.Stinkbug, 0.05f, wallFunc));

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
        float ICustomSpawnSubworld.SpawnMult { get => 0.05f; }

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

            PlaceTreeDungeons(progress);

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
            for (int j = Main.maxTilesY; j > 0; j--)
            {
                if (!CalamityUtils.ParanoidTileRetrieval(Main.spawnTileX, j - 1).HasTile)
                {
                    Main.spawnTileY = j;
                    break;
                }
            }

            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                    t.SetHighlight(false);

                    Tile above = CalamityUtils.ParanoidTileRetrieval(i, j - 1);
                    if (!above.HasTile && t.TileType == TileID.JungleGrass && t.HasTile)
                    {
                        if (WorldGen.genRand.NextBool(5))
                        {
                            CalRemixHelper.ForceGrowTree(i, j, WorldGen.genRand.Next(20, 53));
                        }
                    }
                }
            }

            RandomSubworldDoors.GenerateDoorRandom(ModContent.TileType<OvergrowthRainforestDoor>());
        }

        public static void PlaceTreeDungeons(GenerationProgress prog)
        {
            int treeCd = 0;
            int padding = 250;
            int dungeon = (int)(Main.maxTilesX * templePosition);
            int top = (int)(Main.maxTilesY * treeTopLevel);
            int stuffInGround = 50;
            int treeNum = 0;
            for (int i = dungeon + padding * 2; i < Main.maxTilesX - padding * 2; i++)
            {
                // Update gen progress each time a tree is made
                prog.Set(MathHelper.Lerp(0.25f, 0.5f, i / (float)Main.maxTilesX));
                if (treeCd <= 0 && WorldGen.genRand.NextBool(40))
                {
                    CalRemixHelper.FindTopTile(i, out int y, TileID.Mud, top + 100);

                    int treeDir = WorldGen.genRand.NextBool().ToDirectionInt();

                    float treeHeightMod = WorldGen.genRand.NextFloat(0.03f, 0.1f);
                    if (treeNum % 2 == 0 && treeHeightMod < 0.7f)
                        treeHeightMod = WorldGen.genRand.NextFloat(0.03f, 0.04f);
                    // Where the tree ends
                    int endY = top + (int)(Main.maxTilesX * treeHeightMod);
                    int endX = i + WorldGen.genRand.Next(40, 100) * treeDir;

                    int treeHeight = y - endY;

                    // Lower control point
                    int topCtrlX = i + WorldGen.genRand.Next(100, 160) * -treeDir;
                    int topCtrlY = endY + (int)(WorldGen.genRand.NextFloat(0.3f, 0.4f) * treeHeight);

                    // Upper control point
                    int bottomCtrlX = i + WorldGen.genRand.Next(180, 250) * treeDir;
                    int bottomCtrlY = endY + (int)(WorldGen.genRand.NextFloat(0.7f, 0.8f) * treeHeight);

                    BezierCurve curve = new(new Vector2(i, y), new Vector2(bottomCtrlX, bottomCtrlY), new Vector2(topCtrlX, topCtrlY), new Vector2(endX, endY));

                    // Make the list for the branches
                    // The list contains indices for which trunk segments will grow branches
                    List<Vector2> treePoints = curve.GetPoints(400);
                    int branchCount = WorldGen.genRand.Next(5, 12);
                    List<int> branchIndices = new();
                    int branchSmall = (int)(treePoints.Count * 0.3f);
                    int branchTall = (int)(treePoints.Count * 0.6f);
                    for (int k = 1; k <= branchCount; k++)
                    {
                        branchIndices.Add((int)MathHelper.Lerp(branchSmall, branchTall, k / branchCount) + WorldGen.genRand.Next(-25, 25));
                    }

                    int makeBig = 3;
                    // Main tree gen code
                    for (int p = 0; p < treePoints.Count; p++)
                    {
                        Point tp = treePoints[p].ToPoint();

                        int treeRadMax = WorldGen.genRand.Next(60, 100);
                        int treeRadMin = (int)(treeRadMax * WorldGen.genRand.NextFloat(0.2f, 0.4f));

                        int pointRad = (int)MathHelper.Lerp(treeRadMax, treeRadMin, p / (float)treePoints.Count);

                        // Trunk
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

                        // Main canopy
                        if (p == treePoints.Count - 1)
                        {
                            int bushWidth = WorldGen.genRand.Next(500, 800);
                            int bushHeight = (int)(bushWidth * WorldGen.genRand.NextFloat(0.2f, 0.4f));
                            MakeBush(treePoints[p].X, treePoints[p].Y, bushWidth, bushHeight, true, true);
                        }
                        // Roots
                        else if (p == 0)
                        {
                            int rootCount = WorldGen.genRand.Next(7, 13);
                            for (int k = 0; k < rootCount; k++)
                            {
                                Vector2 rootDir = Vector2.UnitY.RotatedBy(MathHelper.Lerp(-MathHelper.PiOver4, MathHelper.PiOver4, k / (float)(rootCount - 1)) + WorldGen.genRand.NextFloat(MathHelper.ToRadians(5)));

                                Vector2 orig = treePoints[p];
                                bool bigRoot = k % 3 == 0;
                                int rootLength = bigRoot ? WorldGen.genRand.Next(200, 400) : WorldGen.genRand.Next(150, 300);
                                Vector2 end = (treePoints[p] + rootDir * rootLength);

                                // Make a list of points for the roots
                                int rootPointCount = WorldGen.genRand.Next(4, 7);
                                List<Vector2> rootPoints = new();
                                rootPoints.Add(orig);
                                for (int r = 0; r < rootPointCount; r++)
                                {
                                    int bezierIntensity = 100;
                                    rootPoints.Add(Vector2.Lerp(orig, end, r / (float)(rootPointCount - 1)) + new Vector2(WorldGen.genRand.Next(-bezierIntensity, bezierIntensity), WorldGen.genRand.Next(-bezierIntensity, bezierIntensity)));
                                }

                                // Bezierify the points
                                rootPoints = new BezierCurve(rootPoints.ToArray()).GetPoints(80);

                                // Go through the points and blotch in mahogany circles
                                for (int r = 0; r < rootPoints.Count; r++)
                                {
                                    float comp = (float)(r / (float)(rootPoints.Count - 1));
                                    // Circles scale in size based on index
                                    int rootWidthMin = bigRoot ? 4 : 2;
                                    int roodWidthMax = bigRoot ? 30 : 16;
                                    int rootWidth = (int)(MathHelper.Lerp(roodWidthMax, rootWidthMin, comp));
                                    Point rootPoint = rootPoints[r].ToPoint();
                                    for (int q = rootPoint.X - rootWidth; q < rootPoint.X + rootWidth; q++)
                                    {
                                        for (int s = rootPoint.Y - rootWidth; s < rootPoint.Y + rootWidth; s++)
                                        {
                                            if (q < 0 || q >= Main.maxTilesX || s < 0 || s >= Main.maxTilesY)
                                                continue;
                                            float dist = Vector2.Distance(new Vector2(q, s), rootPoint.ToVector2());
                                            if (dist < rootWidth)
                                            {
                                                CalamityUtils.ParanoidTileRetrieval(q, s).ResetToType(TileID.LivingMahogany);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        // Branches
                        if (branchIndices.Contains(p))
                        {
                            int branchSize = WorldGen.genRand.Next(140, 200);
                            int branchHeight = WorldGen.genRand.Next(30, 50);
                            float bushSize = 0.4f;
                            if ((makeBig == 0 && WorldGen.genRand.NextBool()) || makeBig <= -1)
                            {
                                branchSize = WorldGen.genRand.Next(250, 300);
                                branchHeight = WorldGen.genRand.Next(60, 80);
                                bushSize = 1;
                                makeBig = 3;
                            }
                            MakeBranches(p, treePoints, branchSize, branchHeight, bushSize);
                            makeBig--;
                        }
                    }

                    int islandCD = 0;
                    // Hollow out the trunk using the same point list used to make it
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
                                Tile toHollow = CalamityUtils.ParanoidTileRetrieval(k, l + stuffInGround);
                                if (toHollow.GetHighlight())
                                    continue;
                                float dist = Vector2.Distance(new Vector2(k, l), treePoints[p]);
                                if (dist < pointRad)
                                {
                                    if (l < y - 40)
                                        toHollow.HasTile = false;
                                }
                            }
                        }

                        if (p > (int)(treePoints.Count * 0.8f))
                            continue;

                        // Inner islands
                        if (WorldGen.genRand.NextBool(22) && CalamityUtils.ParanoidTileRetrieval(tp.X, tp.Y).TileType != TileID.LivingMahoganyLeaves && islandCD <= 0)
                        {
                            Vector2 platformPoint = tp.ToVector2() + WorldGen.genRand.NextVector2Circular(pointRad / 2, pointRad / 2);
                            Point platformPointPoint = platformPoint.ToPoint();

                            int islandWidth = WorldGen.genRand.Next(20, 40);
                            int islandHeight = WorldGen.genRand.Next(10, 22); // This is divided by 2

                            int dir = 0;
                            if (WorldGen.genRand.NextBool())
                                dir = WorldGen.genRand.NextBool().ToDirectionInt();

                            if (dir != 0)
                            {
                                int searchStart = dir == -1 ? platformPointPoint.X - 30 : platformPointPoint.X;
                                int searchEnd = dir == -1 ? platformPointPoint.X : platformPointPoint.X + 30;
                                bool validLedge = false;

                                for (int d = searchStart; d < searchEnd; d++)
                                {
                                    if (CalamityUtils.ParanoidTileRetrieval(d, platformPointPoint.Y).HasTile)
                                    {
                                        platformPoint.X = d;
                                        validLedge = true;
                                        break;
                                    }
                                }
                                if (validLedge)
                                    islandWidth = (int)(islandWidth * 1.4f);
                            }

                            Rectangle platformRect = Utils.CenteredRectangle(platformPoint, new Vector2(islandWidth, islandHeight));
                            for (int k = platformRect.Left; k < platformRect.Right; k++)
                            {
                                for (int l = platformRect.Center.Y; l < platformRect.Bottom; l++)
                                {
                                    if (CalRemixHelper.WithinElipse(k, l, platformRect.Center.X, platformRect.Center.Y, platformRect.Width / 2, platformRect.Height / 2))
                                    {
                                        Tile platformTile = CalamityUtils.ParanoidTileRetrieval(k, l);
                                        platformTile.TileType = TileID.LivingMahogany;
                                        platformTile.HasTile = true;
                                        platformTile.SetHighlight(true);
                                    }
                                }
                            }
                            islandCD = 10;
                        }

                        islandCD--;
                    }
                    treeNum++;
                    treeCd = padding * 2;
                }
                treeCd--;
            }
        }

        public static void MakeBranches(int idx, List<Vector2> treePoints, int branchLength, int branchHeight, float bushSizeMult = 1f)
        {
            int sign = WorldGen.genRand.NextBool().ToDirectionInt();
            Point startPoint = treePoints[idx].ToPoint();
            Point extraPoint = treePoints[idx].ToPoint() - new Point(0, branchHeight);
            Point endPoint = ((-Vector2.UnitY.RotatedBy(-sign * MathHelper.ToRadians(WorldGen.genRand.NextFloat(60, 80)))) * branchLength).ToPoint() + startPoint;

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
            // Smaller canopy for each branch
            MakeBush(endPoint.X, endPoint.Y, (int)(WorldGen.genRand.Next(250, 300) * bushSizeMult), (int)(WorldGen.genRand.Next(100, 120) * bushSizeMult));
        }

        /// <summary>
        /// Generates a bush
        /// </summary>
        /// <param name="i">The horizontal center</param>
        /// <param name="j">The vertical center</param>
        /// <param name="sizeX">Half of its width</param>
        /// <param name="sizeY">Half of its height</param>
        /// <param name="overrideWalls">If this is set to false, existing walls will block leaves from being placed</param>
        public static void MakeBush(float i, float j, int sizeX, int sizeY, bool overrideWalls = false, bool big = false)
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

                        if (WorldGen.genRand.NextBool(WorldGen.genRand.Next((int)(sizeX * 0.8f), (int)(sizeX * 1.2f))))
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
            // Make caves in the canopies
            if (big)
            {
                foliage.Inflate(-10, -10);
                CalRemixHelper.PerlinGeneration(foliage, noiseSize: new Vector2(300, 300), noiseStrength: 0.2f, noiseThreshold: 0.7f, overrideTiles: true, tileType: TileID.LivingMahoganyLeaves, wallType: WallID.LivingLeaf, tileCondition: (Point p) => CalRemixHelper.WithinElipse(p.X, p.Y, foliage.Center.X, foliage.Center.Y, foliage.Width / 2, foliage.Height / 2));

                // Add leaf walls
                for (int k = foliage.Left; k < foliage.Right; k++)
                {
                    for (int l = foliage.Top; l < foliage.Bottom; l++)
                    {
                        if (CalRemixHelper.WithinElipse(k, l, foliage.Center.X, foliage.Center.Y, foliage.Width / 2, foliage.Height / 2))
                        {
                            CalamityUtils.ParanoidTileRetrieval(k, l).WallType = WallID.LivingLeaf;
                        }
                    }
                }
            }
        }
    }
}