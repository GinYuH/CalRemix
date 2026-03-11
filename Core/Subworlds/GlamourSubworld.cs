using CalamityMod.Tiles.FurnitureAshen;
using CalRemix.Content.NPCs.Subworlds;
using CalRemix.Content.Tiles;
using CalRemix.Content.Walls;
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
    public class GlamourSubworld : Subworld, IDisableBuilding, IDisableOcean, IDisableFlight, IDisableSpawnsSubworld
    {
        public override int Height => 300;
        public override int Width => 2000;
        public override List<GenPass> Tasks => new List<GenPass>()
        {
            new GlamourGeneration()
        };

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void Update()
        {
            if (Main.LocalPlayer.selectedItem == 1 && Main.LocalPlayer.controlUseItem)
            {
                SubworldSystem.Enter<GlamourSubworld>();
            }
            base.Update();
            SkyManager.Instance["Ambience"].Deactivate();
            Main.dayTime = true;
            Main.time = Main.dayLength / 2;
        }

        public override bool GetLight(Tile tile, int x, int y, ref FastRandom rand, ref Vector3 color)
        {
            if (tile.HasTile)
                return false;
            color = new Vector3(0.22f, 0.24f, 0.27f);
            return false;
        }

        public override void DrawMenu(GameTime gameTime)
        {
            base.DrawMenu(gameTime);
            string str = CalRemixHelper.LocalText("StatusText.Glamour").Value;
            Vector2 size = FontAssets.MouseText.Value.MeasureString(str) * 2;
            //Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, Vector2.Zero, new Rectangle(0, 0, Main.screenWidth * 2, Main.screenHeight * 2), Color.Black, 0, Vector2.Zero, 1, 0, 0);
            /*Utils.DrawBorderString(Main.spriteBatch,
                str,
                Main.ScreenSize.ToVector2() * 0.5f - size * 0.5f, Color.White, 2);*/

        }
    }
    public class GlamourGeneration : GenPass
    {
        public GlamourGeneration() : base("Terrain", 1) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Generating terrain"; // Sets the text displayed for this pass
            Main.worldSurface = Main.maxTilesY - 142; // Hides the underground layer just out of bounds
            Main.rockLayer = Main.maxTilesY; // Hides the cavern layer way out of bounds
            int spawnTile = 60;

            int wallCooldown = 0;
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = (int)Main.worldSurface; j < Main.maxTilesY; j++)
                {
                    Main.tile[i, j].ResetToType(TileID.Asphalt);

                    if (j == (int)Main.worldSurface && WorldGen.genRand.NextBool(20) && wallCooldown <= 0)
                    {
                        bool right = WorldGen.genRand.NextBool();
                        int dir = right.ToDirectionInt();
                        int xOff = WorldGen.genRand.Next(10, 20) * dir;
                        Rectangle bounds = new Rectangle(i - Math.Abs(xOff) * 2, 0, Math.Abs(xOff) * 4, j);
                        for (int k = bounds.X; k < bounds.Right; k++)
                        {
                            for (int l = bounds.Y; l < bounds.Bottom; l++)
                            {
                                if (CalRemixHelper.WithinTriangle(new Point(i, j), new Point(i - xOff, j), new Point(i + xOff * 2, -100), new Point(k, l)))
                                {
                                    Main.tile[k, l].WallType = (ushort)ModContent.WallType<GlamorousGemWallPlaced>();
                                }
                            }
                        }
                        wallCooldown = Math.Abs(xOff) * 4;
                    }
                }
                wallCooldown--;
            }

            Main.spawnTileX = spawnTile - 3;
            Main.spawnTileY = (int)Main.worldSurface - 1;

            RandomSubworldDoors.GenerateDoorRandom(ModContent.TileType<GlamourDoor>());
        }
    }
}