using CalamityMod;
using CalamityMod.Tiles.FurnitureAshen;
using CalRemix.Content.Items.Critters;
using CalRemix.Content.NPCs.Subworlds;
using CalRemix.Content.NPCs.Subworlds.Nowhere;
using CalRemix.Content.Tiles;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using SubworldLibrary;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace CalRemix.Core.Subworlds
{
    public class PinnaclesSubworld : Subworld, IDisableOcean, ICustomSpawnSubworld
    {
        public override int Height => 800;
        public override int Width => 2000;
        public override List<GenPass> Tasks => new List<GenPass>()
        {
            new PinnaclesGeneration()
        };

        public List<(int, float, Predicate<NPCSpawnInfo>)> Spawns()
        {
            List<(int, float, Predicate<NPCSpawnInfo>)> list = [];
            list.Add(item: (ModContent.NPCType<Noone>(), 16f, (NPCSpawnInfo n) => Main.tile[n.SpawnTileX, n.SpawnTileY + 1].HasTile));
            return list;
        }

        int ICustomSpawnSubworld.MaxSpawns { get => 14; }
        float ICustomSpawnSubworld.SpawnMult { get => 0.3f; }

        bool ICustomSpawnSubworld.OverrideVanilla { get => true; }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void Update()
        {
            if (Main.LocalPlayer.selectedItem == 1 && Main.LocalPlayer.controlUseItem)
            {
                SubworldSystem.Enter<PinnaclesSubworld>();
            }
            base.Update();
            SubworldSystem.hideUnderworld = true;
            SkyManager.Instance["Ambience"].Deactivate();
            Main.dayTime = true;
            Main.time = Main.dayLength / 2;
        }

        public override bool GetLight(Tile tile, int x, int y, ref FastRandom rand, ref Vector3 color)
        {
            if (tile.HasTile)
                return false;
            color = new Vector3(1f, 1f, 1f);
            return false;
        }

        public override void DrawMenu(GameTime gameTime)
        {
            base.DrawMenu(gameTime);
            string str = CalRemixHelper.LocalText("StatusText.Ant").Value;
            Vector2 size = FontAssets.MouseText.Value.MeasureString(str) * 2;
            //Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, Vector2.Zero, new Rectangle(0, 0, Main.screenWidth * 2, Main.screenHeight * 2), Color.Black, 0, Vector2.Zero, 1, 0, 0);
            /*Utils.DrawBorderString(Main.spriteBatch,
                str,
                Main.ScreenSize.ToVector2() * 0.5f - size * 0.5f, Color.White, 2);*/

        }
    }
    public class PinnaclesGeneration : GenPass
    {
        public PinnaclesGeneration() : base("Terrain", 1) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Generating terrain"; // Sets the text displayed for this pass
            Main.worldSurface = Main.maxTilesY - 142; // Hides the underground layer just out of bounds
            Main.rockLayer = Main.maxTilesY; // Hides the cavern layer way out of bounds
            int concrete = 160;

            Main.spawnTileX = concrete;

            CalRemixHelper.PerlinSurface(new Rectangle(0, (int)(Main.maxTilesY * 0.8f), Main.maxTilesX, (int)(Main.maxTilesY * 0.2f)), (ushort)TileID.Obsidian, variance: 10);
            CalRemixHelper.PerlinSurface(new Rectangle(0, (int)(Main.maxTilesY * 0.8f) - 1, Main.maxTilesX, (int)(Main.maxTilesY * 0.02f)), (ushort)TileID.SnowBlock, variance: 10);

            int topY = (int)(Main.maxTilesY * 0.2f);
            int topWidth = (int)(Main.maxTilesX * 0.2f);
            int topHeight = (int)(Main.maxTilesY * 0.2f) * 2;
            Rectangle topElipse = new Rectangle(0, topY - topHeight / 2, topWidth, topHeight);

            for (int i = topElipse.Left; i < topElipse.Right; i++)
            {
                for (int j = topElipse.Center.Y; j < topElipse.Bottom; j++)
                {
                    if (CalRemixHelper.WithinElipse(i, j, 0, topY, topWidth, topHeight / 2))
                    {
                        Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                        t.ResetToType(TileID.Obsidian);
                        t.SetHighlight(true);
                    }
                }
            }
            CalRemixHelper.PerlinSurface(new Rectangle(0, topY - 15, topWidth, 20), (ushort)TileID.SnowBlock, variance: 10);

            for (int i = topWidth + 22; i > topWidth - 22; i--)
            {
                for (int j = 0; j < (int)(Main.maxTilesY * 0.3f); j++)
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                    if (t.TileType == TileID.SnowBlock && !CalamityUtils.ParanoidTileRetrieval(i + 1, j).HasTile)
                    {
                        if (WorldGen.genRand.NextBool())
                        {
                            Main.tile[i + 1, j].ResetToType(TileID.SnowBlock);
                        }
                    }
                }
            }

            Main.spawnTileY = topY - 10;

            int bottomY = (int)(Main.maxTilesY * 0.8f);
            int bottomWidth = (int)(Main.maxTilesX * 0.25f);
            int bottomHeight = (int)(Main.maxTilesY - bottomY) * 2;

            Rectangle bottomElipse = new Rectangle(0, bottomY - bottomHeight / 2, bottomWidth, bottomHeight / 2);
            for (int i = bottomElipse.Left; i < bottomElipse.Right; i++)
            {
                for (int j = bottomElipse.Top; j < bottomElipse.Bottom + 100; j++)
                {
                    if (CalRemixHelper.WithinElipse(i, j, 0, bottomY, bottomWidth, bottomHeight / 2))
                    {
                        Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                        t.ResetToType(TileID.Obsidian);
                        t.SetHighlight(true);
                    }
                }
            }


            #region Pinnacles

            // Point where the triangles focus on
            Point pinnacleAnchor = new Point((int)(Main.maxTilesX * 0.85f), (int)(Main.maxTilesY * 0.5f));
            Vector2 pinnacleAnchorVector = pinnacleAnchor.ToVector2();
            // Distance away from the point that the triangles begin
            float pinnacleRadius = (int)(Main.maxTilesY * 0.1f);

            float spikeCount = WorldGen.genRand.Next(9, 14);

            int skipTimer = 0;
            for (int z = 0; z < spikeCount; z++)
            {
                // Skip spikes occasionally after the first few
                if (skipTimer <= 0 && z > 2 && WorldGen.genRand.NextBool(3))
                {
                    skipTimer = WorldGen.genRand.Next(1, 3);
                    continue;
                }
                // The angle that this spike is facing away from the center
                float angle = MathHelper.Lerp(MathHelper.ToRadians(20), MathHelper.ToRadians(-200), z / (spikeCount - 1));
                // The vertex of the spike that touches the center
                Vector2 startPoint = pinnacleAnchorVector + new Vector2(0, pinnacleRadius).RotatedBy(angle);
                // The angle of the above vertex
                float angularSize = WorldGen.genRand.NextFloat(MathHelper.ToRadians(5), MathHelper.ToRadians(25));
                // Make the first triangle bigger
                if (z == 0)
                {
                    angularSize = WorldGen.genRand.NextFloat(MathHelper.ToRadians(15), MathHelper.ToRadians(25));
                }
                float halfSize = angularSize / 2f;

                // Create the three points of the triangle
                Point start = startPoint.ToPoint();
                Point tip = (startPoint + Vector2.UnitY.RotatedBy(halfSize + angle) * 1000).ToPoint();
                Point tip2 = (startPoint + Vector2.UnitY.RotatedBy(-halfSize + angle) * 1000).ToPoint();

                // I could absolutely optimize this to only iterate through neccessary bounds, but won't because I'm evil and it generates instantly anyways
                for (int i = 0; i < Main.maxTilesX; i++)
                {
                    for (int j = 0; j < Main.maxTilesY; j++)
                    {
                        Point current = new Point(i, j);
                        if (CalRemixHelper.WithinTriangle(start, tip, tip2, current))
                        {
                            Main.tile[i, j].ResetToType(TileID.Obsidian);
                            Main.tile[i, j].SetHighlight(true);
                        }
                    }
                }

                skipTimer--;
            }

            #endregion

            #region Noise and Smoothing

            // Rhombus noise
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    if (WorldGen.genRand.NextBool(22))
                        continue;

                    // If a tile is highlighted, spawn rhombuses 
                    if (CalamityUtils.ParanoidTileRetrieval(i, j).GetHighlight())
                    {
                        int xDim = WorldGen.genRand.Next(4, 10);
                        int yDim = WorldGen.genRand.Next(4, 10);
                        Rectangle diamondArea = new Rectangle(i - xDim, j - yDim, xDim * 2 + 1, yDim * 2 + 1);
                        for (int k = diamondArea.Left; k < diamondArea.Right; k++)
                        {
                            for (int l = diamondArea.Top; l < diamondArea.Bottom; l++)
                            {
                                Tile targ = CalamityUtils.ParanoidTileRetrieval(k, l);
                                if (targ.HasTile)
                                    continue;
                                if (CalRemixHelper.WithinRhombus(new Point(i, j), new Point(xDim, yDim), new Point(k, l)))
                                {
                                    targ.ResetToType(TileID.Obsidian);
                                }
                            }
                        }
                    }
                }
            }

            for (int k = 0; k < 3; k++)
            {
                for (int i = 0; i < Main.maxTilesX; i++)
                {
                    for (int j = 0; j < Main.maxTilesY; j++)
                    {
                        Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                        // Reset highlights
                        if (t.GetHighlight())
                        {
                            t.SetHighlight(false);
                        }
                        if (t.TileType != TileID.Obsidian)
                            continue;

                        // Check adjacent tile counts
                        int surroundingCounts = 0;
                        if (CalamityUtils.ParanoidTileRetrieval(i + 1, j).HasTile)
                        {
                            surroundingCounts++;
                        }
                        if (CalamityUtils.ParanoidTileRetrieval(i - 1, j).HasTile)
                        {
                            surroundingCounts++;
                        }
                        if (CalamityUtils.ParanoidTileRetrieval(i, j + 1).HasTile)
                        {
                            surroundingCounts++;
                        }
                        if (CalamityUtils.ParanoidTileRetrieval(i, j - 1).HasTile)
                        {
                            surroundingCounts++;
                        }

                        // Clear out orphaned tiles, and single tiles
                        if (surroundingCounts < 2)
                            t.HasTile = false;

                        // Single holes get filled
                        if (surroundingCounts == 3)
                            t.ResetToType(TileID.Obsidian);
                    }
                }
            }

            #endregion

            RandomSubworldDoors.GenerateDoorRandom(ModContent.TileType<NightlineDoor>());
        }
    }
}