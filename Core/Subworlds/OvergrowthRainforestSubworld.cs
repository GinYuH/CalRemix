using CalamityMod;
using CalRemix.Content.Items.Misc;
using CalRemix.Content.NPCs;
using CalRemix.Content.NPCs.Subworlds.GreatSea;
using CalRemix.Content.NPCs.Subworlds.OvergrowthRainforest;
using CalRemix.Content.Tiles;
using CalRemix.Content.Tiles.Subworlds.GreatSea;
using CalRemix.Core.Biomes;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
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
            for (int j = (int)(Main.maxTilesY * 0.4f); j < Main.maxTilesY; j++)
            {
                if (CalamityUtils.ParanoidTileRetrieval(Main.spawnTileX, j).HasTile)
                {
                    Main.spawnTileY = j - 1;
                    break;
                }
            }

            RandomSubworldDoors.GenerateDoorRandom(ModContent.TileType<OvergrowthRainforestDoor>());
        }
    }
}