using CalamityMod;
using CalamityMod.Tiles.FurnitureAshen;
using CalRemix.Content.NPCs.Subworlds;
using CalRemix.Content.Projectiles.Hostile;
using CalRemix.Content.Tiles;
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
    public class BridgeofLostHopeSubworld : Subworld, IDisableOcean, IDisableFlight, IDisableSpawnsSubworld, ISingleColorSky, IDisableItems
    {
        public override int Height => 200;
        public override int Width => 2000;

        public Color SkyColor => Color.Black;

        public override List<GenPass> Tasks => new List<GenPass>()
        {
            new BridgeGeneration()
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


            int spawnY = (int)(Main.maxTilesY * 0.5f);

            if (Main.rand.NextBool(20))
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 spawnPos = new(Main.player[0].Center.X + Main.rand.Next(-1000, 1000) + (Main.player[0].velocity.X > 0).ToInt() * 500, spawnY * 16 + Main.rand.NextBool().ToDirectionInt() * Main.rand.Next(600, 800));
                    Projectile.NewProjectile(new EntitySource_WorldEvent(), spawnPos, spawnPos.DirectionTo(Main.player[0].Center).RotatedByRandom(MathHelper.PiOver4) * 5, ModContent.ProjectileType<Boomer>(), 200, 1);
                }
            }
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
    public class BridgeGeneration : GenPass
    {
        public BridgeGeneration() : base("Terrain", 1) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Generating terrain"; // Sets the text displayed for this pass
            Main.worldSurface = Main.maxTilesY - 1; // Hides the underground layer just out of bounds
            Main.rockLayer = Main.maxTilesY; // Hides the cavern layer way out of bounds
            int spawnX = 60;
            int spawnY = (int)(Main.maxTilesY * 0.5f);

            for (int i = 0; i < Main.maxTilesX; i++)
            {
                CalamityUtils.ParanoidTileRetrieval(i, spawnY).ResetToType(TileID.Stone);
                CalamityUtils.ParanoidTileRetrieval(i, spawnY - 5).ResetToType(TileID.Stone);
            }

            Main.spawnTileX = spawnX - 3;
            Main.spawnTileY = spawnY;

            WorldGen.PlaceObject(spawnX, spawnY - 1, ModContent.TileType<BridgeDoor>());
            WorldGen.PlaceObject(Main.maxTilesX - spawnX, spawnY - 1, ModContent.TileType<BridgeDoor>());
        }
    }
}