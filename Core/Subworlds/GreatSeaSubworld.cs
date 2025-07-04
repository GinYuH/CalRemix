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

namespace CalRemix.Core.Subworlds
{
    public class GreatSeaSubworld : Subworld, ICustomSpawnSubworld
    {
        public List<(int, float, Predicate<NPCSpawnInfo>)> Spawns()
        {
            List<(int, float, Predicate<NPCSpawnInfo>)> list = [];
            // Main Great Sea
            list.Add(item: (ModContent.NPCType<BullShark>(), 0.1f, (NPCSpawnInfo n) => n.Player.InModBiome<GreatSeaBiome>()));
            list.Add(item: (ModContent.NPCType<MicrobialClusterII>(), 2f, (NPCSpawnInfo n) => n.Player.InModBiome<GreatSeaBiome>()));
            list.Add(item: (ModContent.NPCType<Crustiment>(), 0.6f, (NPCSpawnInfo n) => n.Player.InModBiome<GreatSeaBiome>()));
            list.Add(item: (ModContent.NPCType<Stanchor>(), 16f, (NPCSpawnInfo n) => n.Player.InModBiome<GreatSeaBiome>() && Main.tile[n.SpawnTileX, n.SpawnTileY + 1].HasTile));
            list.Add(item: (ModContent.NPCType<KillerPolyp>(), 22f, (NPCSpawnInfo n) => n.Player.InModBiome<GreatSeaBiome>() && Main.tile[n.SpawnTileX, n.SpawnTileY + 1].HasTile));
            list.Add(item: (ModContent.NPCType<TempestKraken>(), 0.05f, (NPCSpawnInfo n) => n.Player.InModBiome<GreatSeaBiome>() && !NPC.AnyNPCs(ModContent.NPCType<TempestKraken>())));
            list.Add(item: (ModContent.NPCType<HellbenderBaby>(), 0.05f, (NPCSpawnInfo n) => n.Player.InModBiome<GreatSeaBiome>()));
            list.Add(item: (ModContent.NPCType<HellbenderHead>(), 0.01f, (NPCSpawnInfo n) => n.Player.InModBiome<GreatSeaBiome>() && !NPC.AnyNPCs(ModContent.NPCType<HellbenderHead>())));
            list.Add(item: (ModContent.NPCType<Livyatan>(), 0.01f, (NPCSpawnInfo n) => n.Player.InModBiome<GreatSeaBiome>() && !NPC.AnyNPCs(ModContent.NPCType<Livyatan>())));

            // Primordial Caves
            list.Add(item: (ModContent.NPCType<TheShoalless>(), 0.8f, (NPCSpawnInfo n) => n.Player.InModBiome<PrimordialCavesBiome>()));
            list.Add(item: (ModContent.NPCType<Zoaoa>(), 1f, (NPCSpawnInfo n) => n.Player.InModBiome<PrimordialCavesBiome>()));
            list.Add(item: (ModContent.NPCType<Xiphactinus>(), 0.4f, (NPCSpawnInfo n) => n.Player.InModBiome<PrimordialCavesBiome>()));
            list.Add(item: (ModContent.NPCType<TanyHead>(), 0.02f, (NPCSpawnInfo n) => n.Player.InModBiome<PrimordialCavesBiome>() && !NPC.AnyNPCs(ModContent.NPCType<TanyHead>())));
            list.Add(item: (ModContent.NPCType<Liopleurodon>(), 0.02f, (NPCSpawnInfo n) => n.Player.InModBiome<PrimordialCavesBiome>() && !NPC.AnyNPCs(ModContent.NPCType<Liopleurodon>())));
            list.Add(item: (ModContent.NPCType<DepthGlider>(), 0.02f, (NPCSpawnInfo n) => n.Player.InModBiome<PrimordialCavesBiome>()));
            list.Add(item: (ModContent.NPCType<AnomalyDisciple3>(), 3f, (NPCSpawnInfo n) => n.Player.InModBiome<PrimordialCavesBiome>() && n.Player.position.ToTileCoordinates().Y > (Main.maxTilesY * 0.9f)));
            return list;
        }

        public override int Height => 2000;
        public override int Width => 6400;
        public override List<GenPass> Tasks =>
        [
            new GrandSeaGeneration()
        ];

        int ICustomSpawnSubworld.MaxSpawns { get => 14; }
        float ICustomSpawnSubworld.SpawnMult { get => 0.1f; }

        bool ICustomSpawnSubworld.OverrideVanilla { get => true; }

        public override bool GetLight(Tile tile, int x, int y, ref FastRandom rand, ref Vector3 color)
        {
            if (tile.LiquidType == LiquidID.Water && tile.LiquidAmount > 0 && !tile.HasTile)
            {
                if (Main.LocalPlayer.InModBiome<PrimordialCavesBiome>())
                {
                    color.X = tile.LiquidAmount / 255f;
                    color.Y = tile.LiquidAmount / 255f;
                    color.Z = tile.LiquidAmount / 255f;
                }
                else
                {
                    color.Z = tile.LiquidAmount / 1055f;
                    color.Y = tile.LiquidAmount / 1255f;
                }
            }
            return false;
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void Update()
        {
            //Main.NewText(Main.LocalPlayer.position.Y / 16 / (float)Main.maxTilesY + " " + Main.LocalPlayer.position.Y / 16 + " " + Main.worldSurface);
            Main.LocalPlayer.ZoneBeach = false;
            //Main.LocalPlayer.ManageSpecialBiomeVisuals("CalRemix:ScreamingFaceSky", true);
            //SkyManager.Instance.Activate("CalRemix:ScreamingFaceSky", Main.LocalPlayer.position);
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
    public class GrandSeaGeneration : GenPass
    {
        public static float seaLevel = 0.1f;
        public static float groundTop = 0.6f;
        public static float groundBottom = 0.605f;
        public static float caveBottom = 0.7f;

        public GrandSeaGeneration() : base("Terrain", 1) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Generating terrain"; // Sets the text displayed for this pass
            Main.worldSurface = Main.maxTilesY - 42; // Hides the underground layer just out of bounds
            Main.rockLayer = Main.maxTilesY; // Hides the cavern layer way out of bounds

            progress.Message = "Generating bedrock";
            progress.Value = 0.05f;
            GenerateBottom();
            progress.Message = "Uncovering the lost depths";
            progress.Value = 0.1f;
            GeneratePrimordialDepths();
            progress.Message = "Caving";
            progress.Value = 0.4f;
            GenerateCaves();
            progress.Message = "Building the sea floor";
            progress.Value = 0.6f;
            GenerateSurface();
            progress.Message = "Littering the sea floor";
            progress.Value = 0.7f;
            GenerateDebris();
            progress.Message = "Generating islands";
            progress.Value = 0.8f;
            GenerateIslands();
            progress.Message = "Flooding";
            progress.Value = 0.9f;

            for (int i = 0; i < Main.maxTilesX; i++)
            {
                progress.Value = 0.9f + MathHelper.Lerp(0f, 0.1f, i / (float)Main.maxTilesX);
                for (int j = (int)(Main.maxTilesY * seaLevel); j < Main.maxTilesY; j++)
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                    t.LiquidAmount = 255;
                    t.LiquidType = LiquidID.Water;
                }
            }
            progress.Value = 1f;


            RandomSubworldDoors.GenerateDoorRandom(ModContent.TileType<GrandSeaDoor>());
        }

        public static void GenerateBottom()
        {
            int height = 80;
            int baseHeight = Main.maxTilesY - height;
            CalRemixHelper.PerlinSurface(new Rectangle(0, baseHeight, Main.maxTilesX, height), ModContent.TileType<DarkstonePlaced>(), 3);
            //Frame the tiles at the surface
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = (int)MathHelper.Max(2, baseHeight - 50); j < (int)MathHelper.Min(baseHeight + 150, Main.maxTilesX - 10); j++)
                {
                    WorldGen.TileFrame(i, j, true);

                    if (i > 2 && i < Main.maxTilesX - 3)
                        Tile.SmoothSlope(i, j, applyToNeighbors: false);
                }
            }
        }

        public static void GeneratePrimordialDepths()
        {
            int y = (int)(Main.maxTilesY * caveBottom);
            CalRemixHelper.PerlinGeneration(new Rectangle(0, y, Main.maxTilesX, Main.maxTilesY - y), noiseThreshold: 0.3f, noiseSize: new Vector2(400, 200), tileType: ModContent.TileType<ChertPlaced>(), ease: CalRemixHelper.PerlinEase.EaseInTop, topStop: 0.2f);
        }

        public static void GenerateCaves()
        {
            int y = (int)(Main.maxTilesY * groundBottom);
            CalRemixHelper.PerlinGeneration(new Rectangle(0, y, Main.maxTilesX, (int)(Main.maxTilesY * caveBottom) - y), noiseThreshold: 0.3f, noiseStrength: 0.2f, noiseSize: new Vector2(240, 180), tileType: ModContent.TileType<SchistPlaced>(), ease: CalRemixHelper.PerlinEase.EaseInOut, bottomStop: 0.8f);
            int padding = 10;
            CalRemixHelper.PerlinSurface(new Rectangle(0, (int)(Main.maxTilesY * caveBottom) - padding, Main.maxTilesX, padding * 2), ModContent.TileType<SchistPlaced>(), 5, 5, true);
        }

        public static void GenerateSurface()
        {
            CalRemixHelper.PerlinSurface(new Rectangle(0, (int)(Main.maxTilesY * groundTop), Main.maxTilesX, (int)(Main.maxTilesX * (groundBottom - groundTop))), ModContent.TileType<SyringodiumPlaced>(), 5, 10, true);
        }

        public static void GenerateDebris()
        {
            int grass = ModContent.TileType<SyringodiumPlaced>();
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < (int)(Main.maxTilesY * groundBottom); j++)
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                    if (!t.HasTile)
                    {
                        if (CalamityUtils.ParanoidTileRetrieval(i, j + 1).TileType == grass)
                        {
                            if (Main.rand.NextBool(60))
                            {
                                int type = Main.rand.NextBool() ? ModContent.TileType<SeaAnchorRubble>() : ModContent.TileType<SeaAnchorRubble2>();
                                WorldGen.PlaceTile(i, j, (ushort)type, true, false, -1);
                            }
                            else if (Main.rand.NextBool(10))
                            {
                                int type = Main.rand.NextBool() ? ModContent.TileType<SeaRockRubble>() : ModContent.TileType<SeaRockRubble2>();
                                WorldGen.PlaceTile(i, j, (ushort)type, true, false, -1);
                            }
                            else if (Main.rand.NextBool(20))
                            {
                                int type = Main.rand.NextBool() ? ModContent.TileType<SeaSpearRubble>() : ModContent.TileType<SeaSpearRubble2>();
                                WorldGen.PlaceTile(i, j, (ushort)type, true, false, -1);
                            }
                        }
                    }
                }
            }
        }

        public static void GenerateIslands()
        {
            int y = (int)(Main.maxTilesY * seaLevel);
            CalRemixHelper.PerlinGeneration(new Rectangle(0, y, Main.maxTilesX, (int)(Main.maxTilesY * groundTop) - y), noiseThreshold: 0.15f,  noiseSize: new Vector2(800, 800), tileType: ModContent.TileType<SyringodiumPlaced>(), ease: CalRemixHelper.PerlinEase.EaseOutTop, topStop: 0.025f);

            Main.spawnTileY = y - 2;

            int xAmt = 10;
            int yAmt = 8;
            int startFadingX = 6;
            int startFadingY = 1;

            for (int i = -xAmt + 1; i < xAmt; i++)
            {
                for (int j = 0; j < yAmt; j++)
                {
                    if (j > startFadingY && Main.rand.NextBool(yAmt - j))
                        continue;
                    if (j > startFadingY && !Main.tile[Main.spawnTileX + i, y + j - 1].HasTile)
                        continue;
                    if (Math.Abs(i) > startFadingX && j > startFadingY && Main.rand.NextBool(xAmt - Math.Abs(i)))
                        continue;
                    Main.tile[Main.spawnTileX + i, y + j].ResetToType((ushort)ModContent.TileType<SyringodiumPlaced>());
                }
            }
        }
    }
}