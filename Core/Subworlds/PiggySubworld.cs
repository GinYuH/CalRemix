using System.Collections.Generic;
using Terraria;
using SubworldLibrary;
using Terraria.WorldBuilding;
using Terraria.IO;
using System;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Terraria.ID;
using CalamityMod;
using CalamityMod.Schematics;

namespace CalRemix.Core.Subworlds
{
    public class PiggySubworld : Subworld, IDisableSpawnsSubworld, IFixDrawBlack, IDisableOcean
    {

        public override int Height => 200;
        public override int Width => 600;
        public override List<GenPass> Tasks =>
        [
            new PiggyGeneration() //
        ];

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void Update()
        {
            Main.LocalPlayer.noBuilding = true;
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

    public class PiggyGeneration : GenPass
    {
        public static float groundHeight => 0.7f;

        public static float treeStart => 0.1f;

        public static float mountainStart => 0.85f;

        public PiggyGeneration() : base("Terrain", 1) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Generating terrain"; // Sets the text displayed for this pass
            Main.worldSurface = Main.maxTilesY - 42; // Hides the underground layer just out of bounds
            Main.rockLayer = Main.maxTilesY; // Hides the cavern layer way out of bounds

            progress.Message = "Growing a very big tree";
            progress.Value = 0.1f;
            GenerateTree();
            progress.Message = "Forming a mountain";
            progress.Value = 0.4f;
            GenerateMountain();
            progress.Message = "Laying out the land";
            progress.Value = 0.7f;
            GenerateLand();
            progress.Message = "Building houses";
            progress.Value = 0.8f;
            GenerateHouses();
            progress.Value = 1f;
        }

        public static void GenerateTree()
        {
            int guaranteedLeaf1 = (int)(Main.maxTilesY * 0.05f);
            int guaranteedLeaf2 = (int)(Main.maxTilesY * groundHeight) - 10;
            int xMax = (int)(Main.maxTilesX * treeStart);
            for (int i = 0; i < xMax; i++)
            {
                for (int j = guaranteedLeaf1; j < Main.maxTilesY; j++)
                {
                    if (i == xMax - 1 && Main.rand.NextBool(22))
                        continue;
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                    if (t.TileType != TileID.LeafBlock)
                        t.ResetToType(TileID.LivingWood);

                    if (j < guaranteedLeaf2)
                    {
                        if (Main.rand.NextBool(40) || j == guaranteedLeaf1 || j == guaranteedLeaf2)
                        {
                            if (i == (int)(Main.maxTilesX * treeStart) - 1)
                            {
                                int width = j == guaranteedLeaf2 ? 10 : j == guaranteedLeaf1 ? 64 : ((int)(Main.rand.Next(4, 12) * MathHelper.Lerp(1, 4, Utils.GetLerpValue(Main.maxTilesY, 0, j, true))));
                                int height = Math.Min((int)(width * Main.rand.NextFloat(0.4f, 0.8f)), 32);


                                for (int k = (int)(i - width); k < (int)(i + width); k++)
                                {
                                    for (int l = (int)(j - height); l < (int)(j + height); l++)
                                    {
                                        if (CalRemixHelper.WithinElipse(k, l, i, j, width, height))
                                        {
                                            Tile te = CalamityUtils.ParanoidTileRetrieval(k, l);
                                            te.ResetToType(TileID.LeafBlock);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void GenerateMountain()
        {
            for (int i = (int)(Main.maxTilesX * mountainStart); i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    bool hasblockAlready = false;
                    if (CalamityUtils.ParanoidTileRetrieval(i - 1, j).HasTile)
                    {
                        hasblockAlready = true;
                    }
                    int chanceToFail = (int)MathHelper.Lerp(2, 5, j / (float)Main.maxTilesY);
                    if (!Main.rand.NextBool(chanceToFail) || hasblockAlready)
                    {
                        Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                        t.ResetToType(TileID.Stone);
                    }
                }
            }
        }

        public static void GenerateLand()
        {
            int top = (int)(Main.maxTilesY * groundHeight);
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = top; j < Main.maxTilesY; j++)
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                    int type = (j == top) ? TileID.Grass : TileID.Dirt;
                    t.ResetToType((ushort)type);
                }
            }
            Main.spawnTileY = top;
            Main.spawnTileX = (int)(Main.maxTilesX * treeStart) + 20;
        }

        public static void GenerateHouses()
        {
            int ground = (int)(Main.maxTilesY * groundHeight);
            int house1 = (int)(Main.maxTilesX * 0.33f);
            int house2 = (int)(Main.maxTilesX * 0.5f);
            int house3 = (int)(Main.maxTilesX * 0.65f);
            bool _ = false;
            SchematicManager.PlaceSchematic<Action<Chest>>("Piggy Straw", new Point(house1, ground + 1), SchematicAnchor.BottomCenter, ref _);
            SchematicManager.PlaceSchematic<Action<Chest>>("Piggy Stick", new Point(house2, ground + 1), SchematicAnchor.BottomCenter, ref _);
            SchematicManager.PlaceSchematic<Action<Chest>>("Piggy Brick", new Point(house3, ground + 1), SchematicAnchor.BottomCenter, ref _);
        }
    }
}