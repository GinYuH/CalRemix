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

namespace CalRemix.Core.Subworlds
{
    public class SealedSubworld : Subworld, IDisableOcean, IFixDrawBlack, ICustomSpawnSubworld
    {
        public List<(int, float, Predicate<NPCSpawnInfo>)> Spawns()
        {
            List<(int, float, Predicate<NPCSpawnInfo>)> list = [];
            list.Add(item: (ModContent.NPCType<StonePriest>(), 0.2f, (NPCSpawnInfo n) => n.Player.InModBiome<VolcanicFieldBiome>()&& Main.tile[n.SpawnTileX, n.SpawnTileY].HasTile));
            list.Add(item: (ModContent.NPCType<DisilUnit>(), 0.1f, (NPCSpawnInfo n) => n.Player.InModBiome<VolcanicFieldBiome>() && Main.tile[n.SpawnTileX, n.SpawnTileY].HasTile));
            list.Add(item: (ModContent.NPCType<SealedPorswine>(), 0.2f, (NPCSpawnInfo n) => (n.Player.InModBiome<SealedFieldsBiome>() || n.Player.InModBiome<TurnipBiome>()) && Main.tile[n.SpawnTileX, n.SpawnTileY].HasTile));
            list.Add(item: (ModContent.NPCType<DoUHead>(), 0.01f, (NPCSpawnInfo n) => n.Player.InModBiome<BadlandsBiome>() && Main.tile[n.SpawnTileX, n.SpawnTileY].HasTile && !NPC.AnyNPCs(ModContent.NPCType<DoUHead>())));
            list.Add(item: (ModContent.NPCType<ParadiseCreeper>(), 1f, (NPCSpawnInfo n) => n.Player.InModBiome<CarnelianForestBiome>()));
            list.Add(item: (ModContent.NPCType<WinterWitch>(), 0.05f, (NPCSpawnInfo n) => n.Player.InModBiome<CarnelianForestBiome>() && !NPC.AnyNPCs(ModContent.NPCType<WinterWitch>())));
            list.Add(item: (ModContent.NPCType<TheBealed>(), 0.1f, (NPCSpawnInfo n) => (n.Player.InModBiome<BadlandsBiome>() || n.Player.InModBiome<TurnipBiome>() || n.Player.InModBiome<SealedFieldsBiome>() || n.Player.InModBiome<DarnwoodSwampBiome>() || n.Player.InModBiome<BarrensBiome>()) && Main.tile[n.SpawnTileX, n.SpawnTileY].HasTile));
            list.Add(item: (ModContent.NPCType<EvilSealedPuppet>(), 2f, (NPCSpawnInfo n) => n.Player.InModBiome<BadlandsBiome>() && Main.tile[n.SpawnTileX, n.SpawnTileY].HasTile && Main.tile[n.SpawnTileX, n.SpawnTileY].TileType == ModContent.TileType<BadrockPlaced>()));
            list.Add(item: (ModContent.NPCType<SealedPuppet>(), 2f, (NPCSpawnInfo n) => n.Player.InModBiome<SealedFieldsBiome>() && Main.tile[n.SpawnTileX, n.SpawnTileY].HasTile));
            list.Add(item: (ModContent.NPCType<SealedCitizen>(), 2f, (NPCSpawnInfo n) => n.Player.InModBiome<TurnipBiome>() && Main.tile[n.SpawnTileX, n.SpawnTileY].HasTile));
            list.Add(item: (ModContent.NPCType<Observer>(), 0.05f, (NPCSpawnInfo n) => n.Player.InModBiome<TurnipBiome>() && Main.tile[n.SpawnTileX, n.SpawnTileY].HasTile));
            list.Add(item: (ModContent.NPCType<YellowEcho>(), 0.05f, (NPCSpawnInfo n) => n.Player.InModBiome<BarrensBiome>() && !NPC.AnyNPCs(ModContent.NPCType<YellowEcho>())));
            list.Add(item: (ModContent.NPCType<MonorianGastropod>(), 0.05f, (NPCSpawnInfo n) => n.Player.InModBiome<VoidForestBiome>() && !NPC.AnyNPCs(ModContent.NPCType<MonorianGastropod>())));
            return list;
        }

        int ICustomSpawnSubworld.MaxSpawns { get => 8; }
        float ICustomSpawnSubworld.SpawnMult { get => 0.2f; }

        bool ICustomSpawnSubworld.OverrideVanilla { get => false; }

        public override int Height => 900;
        public override int Width => 3000;
        public override List<GenPass> Tasks =>
        [
            new SealedGeneration()
        ];

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public static void SaveData(string world)
        {
            TagCompound savedWorldData = SaveCommonSubworldBools();
            MakeTag(ref savedWorldData, "ShadeLevel", CalRemixWorld.shadeQuestLevel);
            MakeTag(ref savedWorldData, "CultistLevel", ItemQuestSystem.cultistLevel);
            MakeTag(ref savedWorldData, "BrainLevel", ItemQuestSystem.brainLevel);
            MakeTag(ref savedWorldData, "RubyLevel", ItemQuestSystem.rubyLevel);
            MakeTag(ref savedWorldData, "DraedonLevel", ItemQuestSystem.draedonLevel);
            MakeTag(ref savedWorldData, "Draedon", RemixDowned.downedDraedon);
            MakeTag(ref savedWorldData, "Oneguy", RemixDowned.downedOneguy);
            MakeTag(ref savedWorldData, "Gastropod", RemixDowned.downedGastropod);
            MakeTag(ref savedWorldData, "Crevi", RemixDowned.downedCrevi);
            MakeTag(ref savedWorldData, "Disilphia", RemixDowned.downedDisil);
            MakeTag(ref savedWorldData, "Void", RemixDowned.downedVoid);

            SubworldSystem.CopyWorldData("RemixCommonBools_" + world, savedWorldData);
        }

        public static void LoadData(string world)
        {
            TagCompound savedWorldData = LoadCommonSubworldBools(world);
            CalRemixWorld.shadeQuestLevel = savedWorldData.GetInt("ShadeLevel");
            RemixDowned.downedDraedon = savedWorldData.GetBool("Draedon");
            RemixDowned.downedOneguy = savedWorldData.GetBool("Oneguy");
            RemixDowned.downedGastropod = savedWorldData.GetBool("Gastropod");
            RemixDowned.downedCrevi = savedWorldData.GetBool("Crevi");
            RemixDowned.downedDisil = savedWorldData.GetBool("Disilphia");
            RemixDowned.downedVoid = savedWorldData.GetBool("Void");
            ItemQuestSystem.cultistLevel = savedWorldData.GetInt("CultistLevel");
            ItemQuestSystem.brainLevel = savedWorldData.GetInt("BrainLevel");
            ItemQuestSystem.rubyLevel = savedWorldData.GetInt("RubyLevel");
            ItemQuestSystem.draedonLevel = savedWorldData.GetInt("DraedonLevel");
        }

        public override void CopyMainWorldData() => SaveData("Main");

        public override void ReadCopiedMainWorldData() => LoadData("Main");

        public override void CopySubworldData() => SaveData("Sealed");

        public override void ReadCopiedSubworldData() => LoadData("Sealed");

        public override void Update()
        {
            SubworldUpdateMethods.UpdateLiquids();
            SubworldUpdateMethods.UpdateTiles();
            SubworldUpdateMethods.UpdateTileEntities();

            if (!NPC.AnyNPCs(ModContent.NPCType<Disilphia>()))
            {
                Main.LocalPlayer.ManageSpecialBiomeVisuals("CalRemix:Sealed", true);
                SkyManager.Instance.Activate("CalRemix:Sealed", Main.LocalPlayer.position);
            }
            Main.time = Main.dayLength * 0.5f;
            base.Update();

            //Point pe = SealedSubworldData.cultPos.ToPoint();
            //Dust.DrawDebugBox(new Rectangle(pe.X, pe.Y, 16, 16));

            foreach (Player p in Main.ActivePlayers)
            {
                if (p.Distance(SealedSubworldData.horizonPos) < 1000)
                {
                    if (!NPC.AnyNPCs(ModContent.NPCType<VigorCloak>()))
                        NPC.NewNPC(new EntitySource_WorldEvent(), (int)SealedSubworldData.horizonPos.X, (int)SealedSubworldData.horizonPos.Y, ModContent.NPCType<VigorCloak>());
                }
                if (p.Distance(SealedSubworldData.warriorPos) < 1000)
                {
                    if (!NPC.AnyNPCs(ModContent.NPCType<RubyWarrior>()))
                        NPC.NewNPC(new EntitySource_WorldEvent(), (int)SealedSubworldData.warriorPos.X, (int)SealedSubworldData.warriorPos.Y, ModContent.NPCType<RubyWarrior>());
                }
                if (p.Distance(SealedSubworldData.brightShrinePos) < 1000)
                {
                    if (!NPC.AnyNPCs(ModContent.NPCType<BrightMind>()))
                        NPC.NewNPC(new EntitySource_WorldEvent(), (int)SealedSubworldData.brightShrinePos.X, (int)SealedSubworldData.brightShrinePos.Y, ModContent.NPCType<BrightMind>());
                }
                if (p.Distance(SealedSubworldData.tentPos) < 1000 && !RemixDowned.downedDraedon)
                {
                    if (!NPC.AnyNPCs(ModContent.NPCType<DreadonFriendly>()))
                        NPC.NewNPC(new EntitySource_WorldEvent(), (int)SealedSubworldData.tentPos.X, (int)SealedSubworldData.tentPos.Y, ModContent.NPCType<DreadonFriendly>());
                }
                if (RemixDowned.downedVoid)
                {
                    if (!NPC.AnyNPCs(ModContent.NPCType<ShadeGreen>()))
                        NPC.NewNPC(new EntitySource_WorldEvent(), (int)SealedSubworldData.citadelPos.X, (int)SealedSubworldData.citadelPos.Y, ModContent.NPCType<ShadeGreen>());
                }
            }
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

    public class SealedSubworldData : ModSystem
    {
        public static Vector2 brightShrinePos = Vector2.Zero;
        public static Vector2 monorianShrinePos = Vector2.Zero;
        public static Vector2 cultPos = Vector2.Zero;
        public static Vector2 tentPos = Vector2.Zero;
        public static Vector2 horizonPos = Vector2.Zero;
        public static Vector2 warriorPos = Vector2.Zero;
        public static Vector2 citadelPos = Vector2.Zero;

        public static float TentRight => SealedSubworldData.tentPos.X + 50 * 16;

        public static float TentLeft => SealedSubworldData.tentPos.X - 74 * 16;

        public static float TentTop => SealedSubworldData.tentPos.Y - 44 * 16;

        public static float TentBottom => SealedSubworldData.tentPos.Y + 4 * 16;
        public static Vector2 TentCenter => new(SealedSubworldData.tentPos.X, SealedSubworldData.tentPos.Y - 300);

        public override void SaveWorldData(TagCompound tag)
        {
            tag["brightShrinePosition"] = brightShrinePos;
            tag["monorianShrinePosition"] = monorianShrinePos;
            tag["cultPosition"] = cultPos;
            tag["tentPosition"] = tentPos;
            tag["horizonPosition"] = horizonPos;
            tag["warriorPosition"] = warriorPos;
            tag["citadelPosition"] = citadelPos;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            brightShrinePos = tag.Get<Vector2>("brightShrinePosition");
            monorianShrinePos = tag.Get<Vector2>("monorianShrinePosition");
            cultPos = tag.Get<Vector2>("cultPosition");
            tentPos = tag.Get<Vector2>("tentPosition");
            horizonPos = tag.Get<Vector2>("horizonPosition");
            warriorPos = tag.Get<Vector2>("warriorPosition");
            citadelPos = tag.Get<Vector2>("citadelPosition");
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
            Main.worldSurface = surfaceTile + 40; 
            Main.rockLayer = caveTile;

            progress.Message = "Erosion";
            GenerateBase(ref progress);
            progress.Message = "Swamping";
            progress.Value = 0.25f;
            GenerateDarnwood();
            progress.Message = "Making the world more barren";
            progress.Value = 0.3f;
            GenerateBarrens();
            progress.Message = "Cragging things up";
            progress.Value = 0.4f;
            GenerateCrags();
            progress.Message = "Growing turnips";
            progress.Value = 0.5f;
            GenerateTurnips();
            progress.Message = "Erupting a volcano";
            progress.Value = 0.6f;
            GenerateVolcano();
            progress.Message = "Building a village";
            progress.Value = 0.7f;
            GenerateVillage();
            progress.Message = "Making things more red";
            progress.Value = 0.8f;
            GenerateCarnelian();
            progress.Message = "Laying out the bottom";
            progress.Value = 0.9f;
            GenerateBedrock();
            progress.Message = "Void";
            progress.Value = 0.95f;
            GenerateVoid();
            progress.Value = 1f;            

            Main.spawnTileY = surfaceTile;
            Main.spawnTileX = fieldPosition + (int)(Main.maxTilesX * fieldWidth * 0.3f);
        }

        public static void GenerateBase(ref GenerationProgress prog)
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
                prog.Value = MathHelper.Lerp(0f, 0.05f, Utils.GetLerpValue(0, Main.maxTilesX, i, true));
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

            prog.Value = 0.05f;

            CalRemixHelper.PerlinGeneration(caveRect, noiseStrength: 0.45f, noiseThreshold: 0.8f, tileType: stone, wallType: stoneWall, ease: CalRemixHelper.PerlinEase.EaseInOut, topStop: 0.05f, bottomStop: 0.9f);

            prog.Value = 0.1f;
            
            CalRemixHelper.PerlinSurface(new Rectangle(leftOceanStop, surface - stretch, Main.maxTilesX - leftOceanStop * 2, variance + 1), dirt, variance: (int)(variance * 0.8f));

            prog.Value = 0.15f;

            bool placeGrass = true;
            for (int i = leftOceanStop; i <= rightOceanStop; i++)
            {
                prog.Value = MathHelper.Lerp(0.15f, 0.2f, Utils.GetLerpValue(leftOceanStop, rightOceanStop, i, true));
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                    Tile left = CalamityUtils.ParanoidTileRetrieval(i - 1, j);
                    Tile right = CalamityUtils.ParanoidTileRetrieval(i + 1, j);
                    Tile top = CalamityUtils.ParanoidTileRetrieval(i, j - 1);
                    if (placeGrass && t.HasTile)
                    {
                        t.TileType = grass;
                        placeGrass = false;
                    }
                    else if(j < caveTile && t.HasTile && t.TileType != TileID.Trees)
                    {
                        t.TileType = dirt;
                    }
                    if (!placeGrass && t.TileType != grass && ((left.HasTile && right.HasTile && top.HasTile) || (j > surfaceTile + variance)))
                    {
                        t.WallType = (j > caveTile) ? stoneWall : dirtWall;
                    }
                }
                placeGrass = true;
            }

            prog.Value = 0.2f;
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

            int elemTries = 0;
            for (int i = leftDarnwood.X; i < leftDarnwood.X + leftDarnwood.Width + 1 - padding; i++)
            {
                for (int j = leftDarnwood.Y; j < leftDarnwood.Y + leftDarnwood.Height + 1; j++)
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                    Tile above = CalamityUtils.ParanoidTileRetrieval(i, j - 1);
                    if (!above.HasTile && t.TileType == dwood && t.HasTile)
                    {
                        if (WorldGen.genRand.NextBool(7))
                        {                                
                            if (CalRemixHelper.ForceGrowTree(i, j, WorldGen.genRand.Next(10, 20)))
                            {
                                if (WorldGen.genRand.NextBool(30) || elemTries == 7)
                                    t.ResetToType((ushort)ModContent.TileType<ElementalWoodPlaced>());
                                elemTries++;
                            }
                        }
                        else if (WorldGen.genRand.NextBool(5))
                        {
                            int peatAmt = WorldGen.genRand.Next(2, 12);
                            for (int k = j - 1; k > j - peatAmt; k--)
                            {
                                if (!CalamityUtils.ParanoidTileRetrieval(i, k).HasTile)
                                    WorldGen.PlaceObject(i, k, ModContent.TileType<PeatSpirePlaced>(), true, WorldGen.genRand.Next(0, 3));
                            }
                        }
                    }
                }
            }
            elemTries = 0;
            for (int i = rightDarnwood.X + padding; i < rightDarnwood.X + rightDarnwood.Width + 1; i++)
            {
                for (int j = leftDarnwood.Y; j < leftDarnwood.Y + leftDarnwood.Height + 1; j++)
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                    Tile above = CalamityUtils.ParanoidTileRetrieval(i, j - 1);
                    if (!above.HasTile && t.TileType == dwood && t.HasTile)
                    {
                        if (WorldGen.genRand.NextBool(7))
                        {
                            if (CalRemixHelper.ForceGrowTree(i, j, WorldGen.genRand.Next(10, 20)))
                            {
                                if (WorldGen.genRand.NextBool(30) || elemTries == 7)
                                    t.ResetToType((ushort)ModContent.TileType<ElementalWoodPlaced>());
                                elemTries++;
                            }
                        }
                        else if (WorldGen.genRand.NextBool(5))
                        {
                            int peatAmt = WorldGen.genRand.Next(2, 12);
                            for (int k = j - 1; k > j - peatAmt; k--)
                            {
                                if (!CalamityUtils.ParanoidTileRetrieval(i, k).HasTile)
                                    WorldGen.PlaceObject(i, k, ModContent.TileType<PeatSpirePlaced>(), true, WorldGen.genRand.Next(0, 3));
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
                            if (blobCooldown <= 0 && WorldGen.genRand.NextBool(30) && t.TileType == tType)
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
                        if (WorldGen.genRand.NextBool(14) && spikeCooldown <= 0)
                        {
                            int cd = 20;
                            if (spikeCount == 6)
                            {
                                bool _ = false;
                                SchematicManager.PlaceSchematic<Action<Chest>>("Bright Shrine", new Point(i, j + 4), SchematicAnchor.BottomCenter, ref _);
                                SealedSubworldData.brightShrinePos = new Vector2(i, j - 4) * 16;
                                cd = 30;
                            }
                            else
                            {
                                int itMin = 4;
                                int itMax = 7;
                                int iterations = WorldGen.genRand.Next(itMin, itMax + 1);
                                int curHeight = 0;
                                for (int m = 0; m < iterations; m++)
                                {
                                    int spikeWidth = (int)(WorldGen.genRand.Next(1, 5) * MathHelper.Lerp(1, 0.2f, Utils.GetLerpValue(itMin, itMax, m, true)));
                                    int spikeHeight = (int)(spikeWidth * WorldGen.genRand.NextFloat(0.8f, 1.5f));
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
                        if (WorldGen.genRand.NextBool(10) && turnipCooldown <= 0)
                        {
                            bool _ = false;
                            string chem = turnipsPlaced == 5 ? "Sealed Citadel" : "Turnip";
                            int offset = turnipsPlaced == 5 ? 3 : 2;
                            if (chem == "Turnip")
                                SchematicManager.PlaceSchematic<Action<Chest>>(chem, new Point(i, j + offset), SchematicAnchor.BottomCenter, ref _);
                            else
                            {
                                SchematicManager.PlaceSchematic(chem, new Point(i, j + offset), SchematicAnchor.BottomCenter, ref _, new Action<Chest, int, bool>(FillCitadelChest));

                                SealedSubworldData.citadelPos = new Vector2(i, j - RemixSchematics.TileMaps[chem].GetLength(1)) * 16;
                            }
                            turnipCooldown = 30;
                            turnipsPlaced++;
                        }
                        break;
                    }
                }
                turnipCooldown--;
            }
        }


        public static void FillCitadelChest(Chest c, int Type, bool place)
        {
            List<(int, int, int)> lootfr = new()
            {
                { (ModContent.ItemType<ExtremelyStrangePuppet>(), 1, 1) },
                { (ModContent.ItemType<StrangePuppet>(), 3, 9) },
                { (ModContent.ItemType<FrozenSealedTear>(), 1, 4) },
                { (ModContent.ItemType<SealToken>(), 6, 14) },
            };
            (int, int, int)[] loot = CalamityUtils.ShuffleArray(lootfr.ToArray());
            for (int i = 0; i < loot.Length; i++)
            {
                Item item = c.item[i];
                item.SetDefaults(loot[i].Item1);
                item.stack = WorldGen.genRand.Next(loot[i].Item2, loot[i].Item3);
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

            int volcanoCooldown = 0;
            int volcanosPlaced = 0;
            for (int i = origin.X - radius; i < origin.X + radius ; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                    if (t.TileType == tType)
                    {
                        if (WorldGen.genRand.NextBool(10) && volcanoCooldown <= 0)
                        {
                            bool _ = false;
                            SchematicManager.PlaceSchematic<Action<Chest>>("Plumestone", new Point(i, j + 4), SchematicAnchor.BottomCenter, ref _);
                            volcanoCooldown = 20;
                            volcanosPlaced++;
                            if (volcanosPlaced == 5)
                            {
                                SealedSubworldData.horizonPos = new Vector2(i, j - 22) * 16;
                            }
                        }
                        break;
                    }
                }
                volcanoCooldown--;
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
            houseTypes.Add(prefix + "Small", 0.65f);
            houseTypes.Add(prefix + "SmallBaby", 0.65f);
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
                        if (WorldGen.genRand.NextBool(10) && hausCooldown <= 0)
                        {
                            string houseType = houseTypes.Get();
                            // Guarantee a church on the fifth house if one hasn't generated yet
                            if ((!generatedChurch && housesGenerated >= 4) || houseType == prefix + "Church")
                            {
                                houseType = prefix + "Church";
                                generatedChurch = true;
                            }
                            bool _ = false;
                            if (houseType.Contains("Church"))
                                SchematicManager.PlaceSchematic(houseType, new Point(i, j + 1), SchematicAnchor.BottomCenter, ref _, new Action<Chest, int, bool>(FillChurchChest));
                            else if (houseType.Contains("Library"))
                                SchematicManager.PlaceSchematic(houseType, new Point(i, j + 1), SchematicAnchor.BottomCenter, ref _, new Action<Chest, int, bool>(FillLibraryChest));
                            else if (houseType.Contains("Large"))
                                SchematicManager.PlaceSchematic(houseType, new Point(i, j + 1), SchematicAnchor.BottomCenter, ref _, new Action<Chest, int, bool>(FillLargeChest));
                            else
                                SchematicManager.PlaceSchematic<Action<Chest>>(houseType, new Point(i, j + 1), SchematicAnchor.BottomCenter, ref _);

                            hausCooldown = (int)(RemixSchematics.TileMaps[prefix + "Library"].GetLength(0));
                            housesGenerated++;
                        }
                        else if (WorldGen.genRand.NextBool(25))
                        {
                            int peatAmt = WorldGen.genRand.Next(3, 8);
                            for (int k = j - 1; k > j - peatAmt; k--)
                            {
                                if (!CalamityUtils.ParanoidTileRetrieval(i, k).HasTile)
                                    WorldGen.PlaceObject(i, k, ModContent.TileType<NeoncanePlaced>(), true, WorldGen.genRand.Next(0, 3));
                            }
                        }
                        break;
                    }
                }
                hausCooldown--;
            }

            Point chamberPoint = new Point(villageStart + 100, caveTile);
            bool _2 = false;
            SchematicManager.PlaceSchematic("Sealed Chamber", chamberPoint, SchematicAnchor.Center, ref _2, new Action<Chest, int, bool>(FillCultChest));
            Vector2 strucSize = new((int)(RemixSchematics.TileMaps["Sealed Chamber"].GetLength(1)), (int)(RemixSchematics.TileMaps["Sealed Chamber"].GetLength(0)));
            SealedSubworldData.tentPos = new Vector2(chamberPoint.X - 38, chamberPoint.Y + 32) * 16;
            SealedSubworldData.cultPos = new Vector2(chamberPoint.X + 85, chamberPoint.Y + 15) * 16;
        }

        public static void FillChurchChest(Chest c, int Type, bool place)
        {
            List<(int, int, int)> lootfr = new()
            {
                { (ModContent.ItemType<NullOrb>(), 1, 1) },
                { (ModContent.ItemType<BabySealedPuppet>(), 10, 15) },
                { (ModContent.ItemType<RotPearl>(), 3, 6) },
                { (ModContent.ItemType<SealToken>(), 3, 7) },
                { (ModContent.ItemType<MysteriousGraySlab>(), 5, 11) },
            };
            (int, int, int)[] loot = CalamityUtils.ShuffleArray(lootfr.ToArray());
            for (int i = 0; i < loot.Length; i++)
            {
                Item item = c.item[i];
                item.SetDefaults(loot[i].Item1);
                item.stack = WorldGen.genRand.Next(loot[i].Item2, loot[i].Item3);
            }
        }
        public static void FillLibraryChest(Chest c, int Type, bool place)
        {
            List<(int, int, int)> lootfr = new()
            {
                { (ModContent.ItemType<SealToken>(), 7, 18) },
                { (ModContent.ItemType<SealedFruit>(), 4, 10) },
                { (ModContent.ItemType<Coke>(), 1, 5) },
                { (ModContent.ItemType<CarnelianRose>(), 1, 3) },
                { (ModContent.ItemType<CarnelianiteOre>(), 1, 2) },
                { (ModContent.ItemType<CarnelianWood>(), 20, 40) },
            };
            (int, int, int)[] loot = CalamityUtils.ShuffleArray(lootfr.ToArray());
            for (int i = 0; i < loot.Length; i++)
            {
                Item item = c.item[i];
                item.SetDefaults(loot[i].Item1);
                item.stack = WorldGen.genRand.Next(loot[i].Item2, loot[i].Item3);
            }
        }
        public static void FillLargeChest(Chest c, int Type, bool place)
        {
            List<(int, int, int)> lootfr = new()
            {
                { (ModContent.ItemType<SealloyBar>(), 1, 4) },
                { (ModContent.ItemType<SealedFruit>(), 4, 8) },
                { (ModContent.ItemType<SealToken>(), 2, 6) },
                { (ModContent.ItemType<Forknife>(), 1, 1) },
                { (ModContent.ItemType<MysteriousGraySlab>(), 3, 7) },
            };
            (int, int, int)[] loot = CalamityUtils.ShuffleArray(lootfr.ToArray());
            for (int i = 0; i < loot.Length; i++)
            {
                Item item = c.item[i];
                item.SetDefaults(loot[i].Item1);
                int stak = (loot[i].Item2 == loot[i].Item3) ? loot[i].Item2 : WorldGen.genRand.Next(loot[i].Item2, loot[i].Item3);
                item.stack = stak;
            }
        }
        public static void FillCultChest(Chest c, int Type, bool place)
        {
            List<(int, int, int)> lootfr = new()
            {
                { (ModContent.ItemType<Veinroot>(), 20, 40) },
                { (ModContent.ItemType<NauseatingPowder>(), 5, 10) },
                { (ModContent.ItemType<RotPearl>(), 7, 14) },
                { (ModContent.ItemType<RottedTendril>(), 3, 9) },
                { (ModContent.ItemType<SealToken>(), 10, 15) },
                { (ModContent.ItemType<FrozenSealedTear>(), 3, 8) },
                { (ModContent.ItemType<AbnormalEye>(), 1, 1) },
            };
            (int, int, int)[] loot = CalamityUtils.ShuffleArray(lootfr.ToArray());
            for (int i = 0; i < loot.Length; i++)
            {
                Item item = c.item[i];
                item.SetDefaults(loot[i].Item1);
                item.stack = WorldGen.genRand.Next(loot[i].Item2, loot[i].Item3);
            }
        }

        public static void GenerateCarnelian()
        {
            ushort stone = (ushort)ModContent.TileType<CarnelianStonePlaced>();
            ushort dirt = (ushort)ModContent.TileType<CarnelianDirtPlaced>();
            ushort grass = (ushort)ModContent.TileType<CarnelianGrassPlaced>();
            ushort stoneWall = (ushort)ModContent.WallType<UnsafeCarnelianStoneWallPlaced>();
            ushort vine = (ushort)ModContent.TileType<CarnelianVines>();
            ushort cVine = (ushort)ModContent.TileType<CookieVines>();

            Point origin = new Point(lavaPosition + (int)(Main.maxTilesX * 0.5f * lavaWidth), surfaceTile + caveTile);

            int width = (int)(Main.maxTilesX * lavaWidth);
            int height = (int)(Main.maxTilesY * 0.5f * 0.3f);

            Rectangle heartRect = new Rectangle(origin.X - width, origin.Y - height * 2, width * 2, height * 3);

            CalRemixHelper.PerlinGeneration(heartRect, noiseStrength: 0.3f, noiseThreshold: 0.9f, tileType: stone, wallType: stoneWall, ease: CalRemixHelper.PerlinEase.EaseInOut, topStop: 0.05f, bottomStop: 0.9f, tileCondition: (Point p) => CalRemixHelper.WithinHeart(origin, new Point(width, height * 2), p), overrideTiles: true, eraseWalls: false);

            int roseAttempts = 0;
            for (int i = heartRect.X; i < heartRect.X + heartRect.Width + 1; i++)
            {
                for (int j = heartRect.Y; j < heartRect.Y + heartRect.Height + 1; j++)
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                    Tile above = CalamityUtils.ParanoidTileRetrieval(i, j - 1);
                    Tile below = CalamityUtils.ParanoidTileRetrieval(i, j + 1);
                    bool growOverride = above.WallType == stoneWall && t.TileType != dirt && t.TileType != stone && t.TileType != TileID.Trees && t.TileType != vine && t.TileType != cVine;
                    if (!above.HasTile || growOverride)
                    {
                        int dirtDist = 3;
                        int oj = j;
                        for (int k = oj; k < oj + dirtDist; k++)
                        {
                            Tile t2 = CalamityUtils.ParanoidTileRetrieval(i, k);
                            if (t2.HasTile && (t2.TileType == stone || growOverride) && t2.TileType != TileID.Trees && t2.TileType != vine && t2.TileType != cVine)
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
                            if (WorldGen.genRand.NextBool(5))
                            {
                                CalRemixHelper.ForceGrowTree(i, j);
                            }
                            else if (WorldGen.genRand.NextBool(15))
                            {
                                WorldGen.PlaceObject(i, j - 1, ModContent.TileType<CarnelianRosePlaced>(), true);
                                roseAttempts++;

                                if (roseAttempts == 15)
                                {
                                    SealedSubworldData.warriorPos = new Vector2(i, j - 1) * 16;
                                }
                            }
                        }
                    }
                    if (WorldGen.genRand.NextBool(60))
                    {
                        int xRange = WorldGen.genRand.Next(3, 7);
                        int yCheck = 5;

                        for (int l = i; l < i + xRange; l++)
                        {
                            for (int m = j - yCheck; m < j + yCheck; m++)
                            {
                                Tile baseTile = CalamityUtils.ParanoidTileRetrieval(l, m);
                                Tile aboveVine = CalamityUtils.ParanoidTileRetrieval(l, m - 1);
                                if (aboveVine.HasTile && Main.tileSolid[aboveVine.TileType] && !baseTile.HasTile && !WorldGen.genRand.NextBool(5) && (aboveVine.TileType == dirt || aboveVine.TileType == stone))
                                {
                                    int vineCheck = 5;
                                    bool enoughRoom = true;
                                    for (int k = m; k < m + vineCheck; k++)
                                    {
                                        Tile vineCheckTile = CalamityUtils.ParanoidTileRetrieval(i, k);
                                        if (vineCheckTile.HasTile)
                                        {
                                            enoughRoom = false;
                                            break;
                                        }
                                    }
                                    if (enoughRoom)
                                    {
                                        int vineHeight = WorldGen.genRand.Next(3, 11);

                                        for (int k = m; k < m + vineHeight; k++)
                                        {
                                            Tile vineCheckTile = CalamityUtils.ParanoidTileRetrieval(l, k);
                                            if (!vineCheckTile.HasTile)
                                            {
                                                ushort vineType = WorldGen.genRand.NextBool(50) ? cVine : vine;
                                                WorldGen.PlaceTile(l, k, vineType, true);
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    continue;
                                }
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
            int offset = (int)(Main.maxTilesX * turnipWidth * 0.4f);
            Point origin = new Point(turnipPosition + (int)(Main.maxTilesX * 0.5f * turnipWidth) - offset, caveTile);
            int size = (int)(Main.maxTilesX * turnipWidth * 0.4f);
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
                        if (WorldGen.genRand.NextBool(5))
                        {
                            CalRemixHelper.ForceGrowTree(i, j, WorldGen.genRand.Next(10, 40));
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
                        if (WorldGen.genRand.NextBool(5))
                        {
                            bool tree = CalRemixHelper.ForceGrowTree(i, j, WorldGen.genRand.Next(10, 40));
                            if (tree && treesPlaecd > 5 && !placedShrine)
                            {
                                placedShrine = true;

                                bool _ = false;
                                SchematicManager.PlaceSchematic("Monorian Shrine", new Point(i, j + 1), SchematicAnchor.BottomCenter, ref _, new Action<Chest, int, bool>(FillMonoriumChest));
                                SealedSubworldData.monorianShrinePos = new Vector2(i, j - 4) * 16;
                            }
                            treesPlaecd++;
                        }
                    }
                }
            }
        }

        public static void FillMonoriumChest(Chest c, int Type, bool place)
        {
            List<(int, int, int)> lootfr = new()
            {
                { (ModContent.ItemType<MonoriumOre>(), 7, 12) },
            };
            (int, int, int)[] loot = CalamityUtils.ShuffleArray(lootfr.ToArray());
            for (int i = 0; i < loot.Length; i++)
            {
                Item item = c.item[i];
                item.SetDefaults(loot[i].Item1);
                item.stack = WorldGen.genRand.Next(loot[i].Item2, loot[i].Item3);
            }
        }

        public static void GenerateBedrock()
        {
            int height = 80;
            int baseHeight = Main.maxTilesY - height;
            CalRemixHelper.PerlinSurface(new Rectangle(0, baseHeight, Main.maxTilesX, height), ModContent.TileType<DarkstonePlaced>(), 3);

            ushort grass = (ushort)ModContent.TileType<SealedGrassPlaced>();
            ushort stone = (ushort)ModContent.TileType<SealedStonePlaced>();
            ushort dirt = (ushort)ModContent.TileType<SealedDirtPlaced>();
            ushort manure = (ushort)ModContent.TileType<PorswineManurePlaced>();

            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                    if (t.TileType == grass)
                    {
                        if (WorldGen.genRand.NextBool(5))
                        {
                            CalRemixHelper.ForceGrowTree(i, j);

                            for (int k = i - 1; k < i + 2; k++)
                            {
                                for (int l = j; l > j - 20; l--)
                                {
                                    if (!WorldGen.genRand.NextBool(10))
                                        continue;
                                    Tile treeTileB = CalamityUtils.ParanoidTileRetrieval(k, l + 1);
                                    Tile treeTileL = CalamityUtils.ParanoidTileRetrieval(k - 1, l);
                                    Tile treeTileR = CalamityUtils.ParanoidTileRetrieval(k + 1, l);
                                    if (!treeTileB.HasTile)
                                    {
                                        if (treeTileL.TileType == TileID.Trees || treeTileR.TileType == TileID.Trees)
                                        {
                                            WorldGen.PlaceTile(k, l, ModContent.TileType<LargeSealedFruitPlaced>(), mute: true, forced: true);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (WorldGen.genRand.NextBool(200))
                    {
                        int xRange = WorldGen.genRand.Next(3, 7);
                        int yCheck = 5;

                        for (int l = i; l < i + xRange; l++)
                        {
                            for (int m = j - yCheck; m < j + yCheck; m++)
                            {
                                Tile baseTile = CalamityUtils.ParanoidTileRetrieval(l, m);
                                Tile aboveVine = CalamityUtils.ParanoidTileRetrieval(l, m - 1);
                                if (aboveVine.HasTile && Main.tileSolid[aboveVine.TileType] && !baseTile.HasTile && !WorldGen.genRand.NextBool(3) && (aboveVine.TileType == dirt || aboveVine.TileType == stone || aboveVine.TileType == manure))
                                {
                                    int vineCheck = 5;
                                    bool enoughRoom = true;
                                    for (int k = m; k < m + vineCheck; k++)
                                    {
                                        Tile vineCheckTile = CalamityUtils.ParanoidTileRetrieval(i, k);
                                        if (vineCheckTile.HasTile)
                                        {
                                            enoughRoom = false;
                                            break;
                                        }
                                    }
                                    if (enoughRoom)
                                    {
                                        int vineHeight = WorldGen.genRand.Next(3, 11);

                                        for (int k = m; k < m + vineHeight; k++)
                                        {
                                            Tile vineCheckTile = CalamityUtils.ParanoidTileRetrieval(l, k);
                                            if (!vineCheckTile.HasTile)
                                            {
                                                WorldGen.PlaceTile(l, k, ModContent.TileType<SealedVines>(), true);
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            CalamityUtils.SpawnOre(ModContent.TileType<FrozenSealedTearOrePlaced>(), 14E-05, 0.55f, 0.95f, 3, 8, ModContent.TileType<SealedStonePlaced>());
            CalamityUtils.SpawnOre(ModContent.TileType<MonoriumOrePlaced>(), 12E-05, 0.45f, 0.95f, 1, 4, ModContent.TileType<SealedStonePlaced>());
            CalamityUtils.SpawnOre(ModContent.TileType<CarnelianiteOrePlaced>(), 12E-06, 0.35f, 0.95f, 1, 4, ModContent.TileType<SealedStonePlaced>());
            CalamityUtils.SpawnOre(ModContent.TileType<CarnelianiteOrePlaced>(), 12E-04, 0.35f, 0.95f, 5, 10, ModContent.TileType<CarnelianStonePlaced>());
            CalamityUtils.SpawnOre(ModContent.TileType<PeatOrePlaced>(), 12E-05, 0.25f, 0.85f, 10, 20, ModContent.TileType<SealedStonePlaced>());
            int iron = WorldGen.genRand.NextBool() ? TileID.Iron : TileID.Lead;
            CalamityUtils.SpawnOre(iron, 12E-05, 0.25f, 0.85f, 8, 12, ModContent.TileType<SealedStonePlaced>());
            int cobalt = WorldGen.genRand.NextBool() ? TileID.Cobalt : TileID.Palladium;
            CalamityUtils.SpawnOre(cobalt, 11E-05, 0.4f, 0.9f, 8, 12, ModContent.TileType<SealedStonePlaced>());
            int mythril = WorldGen.genRand.NextBool() ? TileID.Mythril : TileID.Orichalcum;
            CalamityUtils.SpawnOre(mythril, 10E-05, 0.5f, 0.9f, 8, 12, ModContent.TileType<SealedStonePlaced>());
            int titanium = WorldGen.genRand.NextBool() ? TileID.Titanium : TileID.Adamantite;
            CalamityUtils.SpawnOre(titanium, 9E-05, 0.5f, 0.95f, 8, 12, ModContent.TileType<SealedStonePlaced>());
        }
    }
}