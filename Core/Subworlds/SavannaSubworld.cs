using CalamityMod;
using CalamityMod.Tiles.FurnitureAshen;
using CalRemix.Content.NPCs.Subworlds;
using CalRemix.Content.Projectiles.Hostile;
using CalRemix.Content.Tiles;
using CalRemix.Core.Graphics;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using SubworldLibrary;
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
    public class SavannaSubworld : Subworld, IDisableOcean, IDisableSpawnsSubworld, IDisableBuilding
    {
        public override int Height => 200;
        public override int Width => 200;

        public Color SkyColor => Color.Black;

        public override List<GenPass> Tasks => new List<GenPass>()
        {
            new SavannaGeneration()
        };

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void Update()
        {
            base.Update();
            SkyManager.Instance["Ambience"].Deactivate();
            Main.dayTime = true;
            Main.time = Main.dayLength / 2;
            if (Main.LocalPlayer.selectedItem == 2 && Main.LocalPlayer.controlUseItem)
            {
                SubworldSystem.Enter<SavannaSubworld>();
            }
            CameraPanSystem.CameraFocusPoint = new Vector2(Main.maxTilesX, Main.maxTilesY) * 8;
            CameraPanSystem.CameraPanInterpolant = 1;
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
    public class SavannaGeneration : GenPass
    {
        public SavannaGeneration() : base("Terrain", 1) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Generating terrain"; // Sets the text displayed for this pass
            Main.worldSurface = Main.maxTilesY - 1; // Hides the underground layer just out of bounds
            Main.rockLayer = Main.maxTilesY; // Hides the cavern layer way out of bounds
            int spawnX = (int)(Main.maxTilesX * 0.5f);
            int spawnY = (int)(Main.maxTilesY * 0.5f);
            int roof = spawnY - 30;

            for (int i = 0; i < Main.maxTilesX; i++)
            {
                CalamityUtils.ParanoidTileRetrieval(i, spawnY).ResetToType(TileID.Stone);
                CalamityUtils.ParanoidTileRetrieval(i, spawnY).Get<TileWallBrightnessInvisibilityData>().IsTileInvisible = true;
                CalamityUtils.ParanoidTileRetrieval(i, roof).ResetToType(TileID.Stone);
                CalamityUtils.ParanoidTileRetrieval(i, roof).Get<TileWallBrightnessInvisibilityData>().IsTileInvisible = true;
            }

            Main.spawnTileX = spawnX - 3;
            Main.spawnTileY = spawnY;

            WorldGen.PlaceObject(spawnX, spawnY - 1, ModContent.TileType<BridgeDoor>());
            WorldGen.PlaceObject(Main.maxTilesX - spawnX, spawnY - 1, ModContent.TileType<BridgeDoor>());
        }
    }
}