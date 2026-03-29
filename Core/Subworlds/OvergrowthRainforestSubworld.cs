using CalamityMod;
using CalamityMod.DataStructures;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Schematics;
using CalRemix.Content.Items.Placeables.Subworlds.OvergrowthRainforest;
using CalRemix.Content.Items.Potions;
using CalRemix.Content.NPCs;
using CalRemix.Content.NPCs.Subworlds.OvergrowthRainforest;
using CalRemix.Content.Tiles;
using CalRemix.Content.Tiles.Subworlds.GreatSea;
using CalRemix.Content.Tiles.Subworlds.OvergrowthRainforest;
using CalRemix.Content.Walls;
using CalRemix.Core.World;
using CalRemix.UI.ElementalSystem;
using Microsoft.Xna.Framework;
using rail;
using SubworldLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using Terraria.Utilities;
using Terraria.WorldBuilding;
using static Terraria.GameContent.Bestiary.IL_BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions;

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
            Point p = Main.MouseWorld.ToTileCoordinates();
            if (Main.mouseLeft && Main.LocalPlayer.selectedItem == 2)
            {
                int k = p.X;
                int j = p.Y;
                WorldGen.PlaceTile(k, j, ModContent.TileType<RicketyBridge>(), true);
                TileEntity.PlaceEntityNet(k, j, ModContent.TileEntityType<RicketyBridgeTE>());
                if (RicketyBridge.GetTEFromCoords(k, j, out RicketyBridgeTE te))
                {
                    te.anchorPoint = new Point(k + 22, j);
                }
            }
            base.Update();
            SubworldUpdateMethods.UpdateTileEntities();
            SubworldUpdateMethods.UpdateTiles();
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

            //WorldGen.genRand.SetSeed(Main.rand.Next(0, 10000) + (int)(Main.GlobalTimeWrappedHourly * 2222));

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

            progress.Set(0.6f);

            MakeBridges();

            progress.Set(0.75f);

            Moss();
            TreeHouse();
            FinalizeGen();

            progress.Set(1f);
        }

        public static void MakeBridges()
        {
            ushort woodBlock = (ushort)ModContent.TileType<TitanodendronWoodPlaced>();
            ushort leafBlock = (ushort)ModContent.TileType<TitanodendronLeafBlockPlaced>();

            int top = (int)(Main.maxTilesY * 0.4f);
            int dungeon = (int)(Main.maxTilesX * templePosition);
            int bottom = (int)(Main.maxTilesY * 0.6f);

            Rectangle toCheck = new Rectangle(dungeon, top, Main.maxTilesX - dungeon, bottom - top);

            List<int> bridgePoses = new();

            int bridgeTries = 0;
            int bridgesPlaced = 0;
            // Place bridges
            while (bridgeTries < 100000 && bridgesPlaced < 5)
            {
                Point pt = WorldGen.genRand.NextVector2FromRectangle(toCheck).ToPoint();
                int i = pt.X;
                int j = pt.Y;
                Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                bool sb = false;
                // Don't place bridges within a vertical distance of each other
                for (int e = 0; e < bridgePoses.Count; e++)
                {
                    if (Math.Abs(bridgePoses[e] - j) < 20)
                    {
                        sb = true;
                        break;
                    }
                }
                if (sb)
                    continue;
                if (!t.HasTile || (t.TileType != woodBlock && t.TileType != leafBlock))
                    continue;
                // Don't bother if the tile is surrounded by two other tiles
                if (CalamityUtils.ParanoidTileRetrieval(i - 1, j).HasTile && CalamityUtils.ParanoidTileRetrieval(i + 1, j).HasTile)
                {
                    continue;
                }
                int checkDistance = 100;

                int iters = 0;
                bool found = false;
                // Check for a solid tile to the right
                for (int k = i + 1; k < i + 1000; k++)
                {
                    Tile tileCheck = CalamityUtils.ParanoidTileRetrieval(k, j);
                    iters++;
                    if (tileCheck.HasTile || tileCheck.WallType > WallID.None)
                    {
                        found = true;
                        break;
                    }
                }

                Tile finalTile = CalamityUtils.ParanoidTileRetrieval(i + iters, j);

                if (finalTile.TileType != t.TileType)
                    continue;

                if (iters > checkDistance && found)
                {
                    for (int k = i + 1; k < i + iters; k++)
                    {
                        int platWidth = 15;
                        if (k == i + platWidth || k == i + iters - platWidth)
                        {
                            for (int m = j - 1; m > j - 8; m--)
                            {
                                CalamityUtils.ParanoidTileRetrieval(k, m).WallType = WallID.RichMahoganyFence;
                            }
                        }

                        // Platform ledges
                        if (k == i + 1 || k == i + iters - 1)
                        {
                            int halfHeight = 5;
                            int halfWidth = platWidth;
                            Rectangle island = Utils.CenteredRectangle(new Vector2(k, j), new Vector2(halfWidth * 2, halfHeight * 2));

                            for (int l = island.Left; l < island.Right; l++)
                            {
                                for (int m = island.Center.Y; m < island.Bottom; m++)
                                {
                                    Tile querie = CalamityUtils.ParanoidTileRetrieval(l, m);
                                    if (querie.TileType != woodBlock && querie.HasTile)
                                        continue;
                                    if (CalRemixHelper.WithinElipse(l, m, island.Center.X, island.Center.Y, halfWidth, halfHeight))
                                    {
                                        querie.ResetToType(woodBlock);
                                        querie.SetHighlight(true);
                                    }
                                }
                            }
                        }
                        if (k == i + platWidth)
                        {
                            CalamityUtils.ParanoidTileRetrieval(k, j - 1).TileType = (ushort)ModContent.TileType<RicketyBridge>();
                            CalamityUtils.ParanoidTileRetrieval(k, j - 1).HasTile = true;
                            TileEntity.PlaceEntityNet(k, j - 1, ModContent.TileEntityType<RicketyBridgeTE>());
                            if (RicketyBridge.GetTEFromCoords(k, j - 1, out RicketyBridgeTE te))
                            {
                                te.anchorPoint = new Point(i + iters - 1 - platWidth, j);
                            }
                        }
                    }
                    bridgesPlaced++;
                    bridgePoses.Add(j);
                }
                bridgeTries++;
            }
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

        public static void Moss()
        {
            ushort woodBlock = (ushort)ModContent.TileType<TitanodendronWoodPlaced>();
            ushort leafBlock = (ushort)ModContent.TileType<TitanodendronLeafBlockPlaced>();
            ushort mos = (ushort)ModContent.TileType<MossyTitanodendronWoodPlaced>();

            int top = (int)(Main.maxTilesY * 0.4f);
            int dungeon = (int)(Main.maxTilesX * templePosition);
            int bottom = (int)(Main.maxTilesY * 0.6f);

            int mossesPlaced = 0;
            int tries = 0;

            while (tries < 10000 && mossesPlaced < 40)
            {
                int x = WorldGen.genRand.Next(dungeon, Main.maxTilesX);
                int y = WorldGen.genRand.Next(top, bottom);
                Tile t = CalamityUtils.ParanoidTileRetrieval(x, y);
                if (!t.HasTile || t.TileType != woodBlock || t.WallType == WallID.None)
                {
                    continue;
                }
                int mossRad = WorldGen.genRand.Next(8, 24);
                Rectangle rect = Utils.CenteredRectangle(new Vector2(x, y), Vector2.One * (mossRad * 2 + 1));
                bool aMossPlaced = false;
                for (int i = rect.Left; i < rect.Right; i++)
                {
                    for (int j = rect.Top; j < rect.Bottom; j++)
                    {
                        Tile quer = CalamityUtils.ParanoidTileRetrieval(i, j);
                        Tile left = CalamityUtils.ParanoidTileRetrieval(i - 1, j);
                        Tile right = CalamityUtils.ParanoidTileRetrieval(i + 1, j);
                        Tile up = CalamityUtils.ParanoidTileRetrieval(i, j - 1);
                        Tile down = CalamityUtils.ParanoidTileRetrieval(i, j + 1);

                        if (!(up.HasTile && up.WallType == WallID.None
                            || left.HasTile && left.WallType == WallID.None
                            || right.HasTile && right.WallType == WallID.None
                            ||  down.HasTile && down.WallType == WallID.None))
                        {
                            if (!up.HasTile || !left.HasTile || !right.HasTile || !down.HasTile)
                            { 
                                if (CalRemixHelper.WithinElipse(i, j, x, y, mossRad, mossRad))
                                {
                                    quer.TileType = mos;
                                    aMossPlaced = true;
                                }
                            }
                        }
                    }
                }
                if (aMossPlaced)
                    mossesPlaced++;
                tries++;
            }
        }

        public static void TreeHouse()
        {
            ushort woodBlock = (ushort)ModContent.TileType<TitanodendronWoodPlaced>();
            ushort leafBlock = (ushort)ModContent.TileType<TitanodendronLeafBlockPlaced>();

            int top = (int)(Main.maxTilesY * 0.4f);
            int dungeon = (int)(Main.maxTilesX * templePosition);
            int bottom = (int)(Main.maxTilesY * 0.6f);

            bool hausPlaced = false;
            int tries = 0;

            while (tries < 10000 && !hausPlaced)
            {
                int x = WorldGen.genRand.Next(dungeon, Main.maxTilesX - dungeon);
                for (int j = 50; j < 300; j++)
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval(x, j);
                    Tile above = CalamityUtils.ParanoidTileRetrieval(x, j - 1);
                    if (t.HasTile && t.TileType == leafBlock && !above.HasTile)
                    {
                        bool _ = false;
                        SchematicManager.PlaceSchematic("Tree House", new Point(x, j + 5), SchematicAnchor.BottomCenter, ref _, new Action<Chest, int, bool>(FillTreeHouseChest));
                        hausPlaced = true;
                        break;
                    }
                }
                tries++;
            }
        }

        public static void FillTreeHouseChest(Chest c, int Type, bool place)
        {
            List<(int, int, int)> items = new List<(int, int, int)>();
            items.Add((ItemID.JungleSpores, 89, 120));
            items.Add((ItemID.Stinger, 89, 120));
            items.Add((ItemID.Vine, 89, 120));
            items.Add((ItemID.JungleKey, 1, 2));
            items.Add((ModContent.ItemType<Needler>(), 1, 2));
            items.Add((ModContent.ItemType<TrueCausticEdge>(), 1, 2));
            items.Add((ModContent.ItemType<UelibloomOre>(), 12, 34));
            items.Add((ModContent.ItemType<CrabLeaves>(), 23, 34));
            items.Add((ItemID.JungleYoyo, 1, 2));
            items.Add((ItemID.TempleKey, 1, 2));
            items.Add((ItemID.JungleRose, 1, 2));
            items.Add((ItemID.NaturesGift, 1, 2));
            items.Add((ItemID.Uzi, 1, 2));

            items = CalamityUtils.ShuffleArray(items.ToArray()).ToList();

            for (int i = 0; i < WorldGen.genRand.Next(6, 11); i++)
            {
                (int, int, int) choice = items[i];
                Item item = c.item[i];
                item.SetDefaults(choice.Item1);
                item.stack = WorldGen.genRand.Next(choice.Item2, choice.Item3);
            }
        }

        public static void FinalizeGen()
        {
            ushort woodBlock = (ushort)ModContent.TileType<TitanodendronWoodPlaced>();
            ushort leafBlock = (ushort)ModContent.TileType<TitanodendronLeafBlockPlaced>();
            ushort woodWall = (ushort)ModContent.WallType<UnsafeTitanodendronWoodWallPlaced>();
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
                    Tile below = CalamityUtils.ParanoidTileRetrieval(i, j + 1);
                    if (!above.HasTile && t.TileType == TileID.JungleGrass && t.HasTile)
                    {
                        if (WorldGen.genRand.NextBool(5))
                        {
                            CalRemixHelper.ForceGrowTree(i, j, WorldGen.genRand.Next(20, 53));
                        }
                    }
                    if (WorldGen.genRand.NextBool(2000) && t.WallType == woodWall && !t.HasTile)
                    {
                        WorldGen.PlaceTile(i, j, ModContent.TileType<CitrusPeelFungus>(), true, true);
                    }
                    int tableRate = 50;
                    if (WorldGen.genRand.NextBool(tableRate) && t.WallType == woodWall && !t.HasTile)
                    {
                        WorldGen.PlaceTile(i, j, ModContent.TileType<TableFungus>(), true, true, style: WorldGen.genRand.Next(3));
                    }
                    if (WorldGen.genRand.NextBool(tableRate) && t.WallType == woodWall && !t.HasTile)
                    {
                        WorldGen.PlaceTile(i, j, ModContent.TileType<TableFungusAlt>(), true, true, style: WorldGen.genRand.Next(3));
                    }
                    if (WorldGen.genRand.NextBool(30) && above.WallType > WallID.None && !above.HasTile && t.HasTile)
                    {
                        WorldGen.PlaceTile(i, j - 1, ModContent.TileType<Chimpnip>(), true, true);
                    }
                    if (WorldGen.genRand.NextBool(30))
                    {
                        if (t.HasTile && !below.HasTile && (t.TileType == woodBlock || t.TileType == leafBlock) && below.WallType > WallID.None)
                        {
                            bool interrupt = false;
                            for (int k = j + 2; k < j + 16; k++)
                            {
                                if (CalamityUtils.ParanoidTileRetrieval(i, k).HasTile)
                                {
                                    interrupt = true;
                                    break;
                                }
                            }
                            if (!interrupt)
                            {
                                t.TileType = (ushort)ModContent.TileType<Calamansi>();
                                TileEntity.PlaceEntityNet(i, j, ModContent.TileEntityType<CalamansiTE>());
                            }
                        }
                    }
                }
            }

            RandomSubworldDoors.GenerateDoorRandom(ModContent.TileType<OvergrowthRainforestDoor>());
        }

        public static void PlaceTreeDungeons(GenerationProgress prog)
        {
            ushort woodBlock = (ushort)ModContent.TileType<TitanodendronWoodPlaced>();
            ushort leafBlock = (ushort)ModContent.TileType<TitanodendronLeafBlockPlaced>();
            ushort woodWall = (ushort)ModContent.WallType<UnsafeTitanodendronWoodWallPlaced>();
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
                                    log.ResetToType(woodBlock);
                                    if (dist < pointRad - 3)
                                    {
                                        log.WallType = woodWall;
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
                            int rootCount = WorldGen.genRand.Next(14, 27);
                            for (int k = 0; k < rootCount; k++)
                            {
                                float rootSpread = MathHelper.ToRadians(75);
                                Vector2 rootDir = Vector2.UnitY.RotatedBy(MathHelper.Lerp(-rootSpread, rootSpread, k / (float)(rootCount - 1)) + WorldGen.genRand.NextFloat(MathHelper.ToRadians(5)));

                                Vector2 orig = treePoints[p];
                                bool bigRoot = k % 3 == 0;
                                bool gigaRoot = k % 9 == 0;
                                int rootLength = gigaRoot ? WorldGen.genRand.Next(600, 1400) : bigRoot ? WorldGen.genRand.Next(200, 400) : WorldGen.genRand.Next(150, 300);
                                Vector2 end = (treePoints[p] + rootDir * rootLength);

                                // Make a list of points for the roots
                                int rootPointCount = WorldGen.genRand.Next(14, 27);
                                List<Vector2> rootPoints = new();
                                rootPoints.Add(orig);
                                for (int r = 0; r < rootPointCount; r++)
                                {
                                    int bezierIntensity = gigaRoot ? 50 : 100;
                                    rootPoints.Add(Vector2.Lerp(orig, end, r / (float)(rootPointCount - 1)) + new Vector2(WorldGen.genRand.Next(-bezierIntensity, bezierIntensity), WorldGen.genRand.Next(-bezierIntensity, bezierIntensity)));
                                }

                                // Bezierify the points
                                rootPoints = new BezierCurve(rootPoints.ToArray()).GetPoints(280);

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
                                            Tile possibleRoot = CalamityUtils.ParanoidTileRetrieval(q, s);
                                            if (rootWidth == 2 && !possibleRoot.HasTile)
                                                continue;
                                            if (q < dungeon || q >= Main.maxTilesX || s < 0 || s >= Main.maxTilesY)
                                                continue;
                                            float dist = Vector2.Distance(new Vector2(q, s), rootPoint.ToVector2());
                                            if (dist < rootWidth)
                                            {
                                                possibleRoot.ResetToType(woodBlock);
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
                    bool placedACave = false;
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

                        // Entrances/exits
                        if (p < (int)(treePoints.Count * 0.8f))
                        {
                            int iterations = 0;
                            bool validCave = true;

                            // Holes are placed either by chance or if enough iterations have passed without a single one being placed
                            if (WorldGen.genRand.NextBool(50) || (iterations > WorldGen.genRand.Next(5, 12) && !placedACave))
                            {
                                // Check if any branches are nearby
                                // It's not fool-proof, but it greatly reduces the chances
                                for (int b = 0; b < branchIndices.Count; b++)
                                {
                                    if (branchIndices[b] - p > 15 && branchIndices[b] - p < 30)
                                    {
                                        validCave = false;
                                        break;
                                    }
                                }
                                if (validCave)
                                {
                                    int dir = WorldGen.genRand.NextBool().ToDirectionInt();
                                    // The first cave will always be facing right
                                    if (!placedACave)
                                        dir = 1;
                                    int holePoints = 30;
                                    Vector2 holeDest = tp.ToVector2() + new Vector2(dir * 200, WorldGen.genRand.Next(-20, 0));

                                    for (int b = 0; b < holePoints; b++)
                                    {
                                        int holeRadius = WorldGen.genRand.Next(4, 8);
                                        Point cavePos = (Vector2.Lerp(tp.ToVector2(), holeDest, b / (float)(holePoints - 1))).ToPoint();

                                        for (int k = cavePos.X - holeRadius; k < cavePos.X + holeRadius; k++)
                                        {
                                            for (int l = cavePos.Y - holeRadius; l < cavePos.Y + holeRadius; l++)
                                            {
                                                if (k < 0 || k >= Main.maxTilesX || l < 0 || l >= Main.maxTilesY)
                                                    continue;
                                                Tile toHollow = CalamityUtils.ParanoidTileRetrieval(k, l);
                                                if (toHollow.GetHighlight())
                                                    continue;
                                                float dist = Vector2.Distance(new Vector2(k, l), cavePos.ToVector2());
                                                if (dist < holeRadius && toHollow.TileType == woodBlock)
                                                {
                                                    if (toHollow.HasTile)
                                                        toHollow.WallType = woodWall;
                                                    toHollow.HasTile = false;
                                                }
                                            }
                                        }
                                    }
                                    placedACave = true;
                                    if (iterations > 22)
                                        iterations = 0;
                                }
                                iterations++;
                            }
                        }


                        if (p > (int)(treePoints.Count * 0.8f))
                            continue;

                        // Inner islands
                        if (WorldGen.genRand.NextBool(22) && CalamityUtils.ParanoidTileRetrieval(tp.X, tp.Y).TileType != leafBlock && islandCD <= 0)
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
                                        if (platformTile.WallType == WallID.None)
                                            continue;
                                        platformTile.TileType = woodBlock;
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
            ushort woodBlock = (ushort)ModContent.TileType<TitanodendronWoodPlaced>();
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
                        branchTile.ResetToType(woodBlock);
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
            ushort leafBlock = (ushort)ModContent.TileType<TitanodendronLeafBlockPlaced>();
            ushort leafWall = (ushort)ModContent.WallType<UnsafeTitanodendronLeafBlockWallPlaced>();
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
                        CalamityUtils.ParanoidTileRetrieval(k, l).ResetToType(leafBlock);

                        if (WorldGen.genRand.NextBool(WorldGen.genRand.Next((int)(sizeX * 0.8f), (int)(sizeX * 1.2f))))
                        {
                            int leafBallRad = (int)(WorldGen.genRand.NextFloat(sizeX * 0.05f, sizeX * 0.1f));
                            for (int m = k - leafBallRad; m < k + leafBallRad; m++)
                            {
                                for (int n = l - leafBallRad; n < l + leafBallRad; n++)
                                {
                                    if (new Vector2(k, l).Distance(new Vector2(m, n)) < leafBallRad)
                                    {
                                        CalamityUtils.ParanoidTileRetrieval(m, n).ResetToType(leafBlock);
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
                CalRemixHelper.PerlinGeneration(foliage, noiseSize: new Vector2(300, 300), noiseStrength: 0.2f, noiseThreshold: 0.7f, overrideTiles: true, tileType: leafBlock, wallType: leafWall, tileCondition: (Point p) => CalRemixHelper.WithinElipse(p.X, p.Y, foliage.Center.X, foliage.Center.Y, foliage.Width / 2, foliage.Height / 2));

                // Add leaf walls
                for (int k = foliage.Left; k < foliage.Right; k++)
                {
                    for (int l = foliage.Top; l < foliage.Bottom; l++)
                    {
                        if (CalRemixHelper.WithinElipse(k, l, foliage.Center.X, foliage.Center.Y, foliage.Width / 2, foliage.Height / 2))
                        {
                            CalamityUtils.ParanoidTileRetrieval(k, l).WallType = leafWall;
                        }
                    }
                }
            }
        }
    }
}