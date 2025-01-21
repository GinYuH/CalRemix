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

namespace CalRemix.Core.Subworlds
{
    public class ExosphereSubworld : Subworld
    {
        public override int Height => 2222;
        public override int Width => 8222;

        public int KickTimer = 0;
        public override List<GenPass> Tasks => new List<GenPass>()
        {
            new ExosphereGeneration()
        };

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void Update()
        {
            base.Update();
            SubworldSystem.Exit();
            KickTimer = 0;            
        }

        public override void DrawMenu(GameTime gameTime)
        {
            base.DrawMenu(gameTime);
            string str = CalRemixHelper.LocalText("StatusText.Exosphere").Value;
            Vector2 size = FontAssets.MouseText.Value.MeasureString(str) * 2;
            Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, Vector2.Zero, new Rectangle(0, 0, Main.screenWidth * 2, Main.screenHeight * 2), Color.Black, 0, Vector2.Zero, 1, 0, 0);
            Utils.DrawBorderString(Main.spriteBatch, 
                str, 
                Main.ScreenSize.ToVector2() * 0.5f - size * 0.5f, Color.White, 2);

        }
    }
    public class ExosphereGeneration : GenPass
    {
        public ExosphereGeneration() : base("Terrain", 1) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Generating terrain"; // Sets the text displayed for this pass
            Main.worldSurface = Main.maxTilesY - 42; // Hides the underground layer just out of bounds
            Main.rockLayer = Main.maxTilesY; // Hides the cavern layer way out of bounds
            for (int i = 0; i < 260; i++)
            {
                for (int j = 2140; j < Main.maxTilesY; j++)
                {
                    if (i > 20 && i < 500 && j > 20 && j < Main.maxTilesY - 20)
                    {
                        float y = Main.maxTilesY - (float)(((-Math.Pow(i, 2) + 24) / 2000) + 50) + Main.rand.Next(-3, 3);
                        if (j > y || Main.tile[i, Math.Max(1, j - 1)].TileType == ModContent.TileType<ScoriaOre>())
                        {
                            WorldGen.PlaceTile(i, j, ModContent.TileType<ScoriaOre>(), true, true);
                            WorldGen.SquareTileFrame(i, j, true);
                            NetMessage.SendTileSquare(-1, i, j, 1);
                        }
                    }
                }
            }
            // smoothening
            /*
            for (int i = 0; i < 260; i++)
            {
                for (int j = 2140; j < Main.maxTilesY; j++)
                {
                    if (i > 20 && i < 500 && j > 20 && j < Main.maxTilesY - 20)
                    {                        
                        if (Main.tile[i, Math.Max(1, j)].TileType == ModContent.TileType<ScoriaOre>())
                        {
                            if (!Main.tile[i + 1, j].HasTile && !Main.tile[i - 1, j].HasTile)
                            {
                                WorldGen.KillTile(i, j, noItem: true);
                                continue;
                            }
                            if (!Main.tile[i + 1, j].HasTile && Main.tile[i + 2, j].TileType == ModContent.TileType<ScoriaOre>())
                            {
                                WorldGen.PlaceTile(i + 1, j, ModContent.TileType<ScoriaOre>(), true, true);
                                WorldGen.SquareTileFrame(i + 1, j, true);
                                NetMessage.SendTileSquare(-1, i + 1, j, 1);
                            }
                        }
                    }
                }
            }*/
            for (int i = 0; i <  Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    if (j % 60 != 0)
                        continue;
                    if (Main.tile[i, j].TileType == ModContent.TileType<ScoriaOre>())
                        continue;
                    WorldGen.PlaceTile(i, j, ModContent.TileType<ExoPlatformTile>(), true, true);
                    WorldGen.SquareTileFrame(i, j, true);
                    NetMessage.SendTileSquare(-1, i, j, 1);
                }
            }
        }
    }
}