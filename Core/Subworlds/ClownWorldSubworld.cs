using CalRemix.Content.Tiles.Subworlds.ClownWorld;
using Microsoft.Xna.Framework;
using SubworldLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace CalRemix.Core.Subworlds
{
    public class ClownWorldSubworld : Subworld
    {
        public override int Height => 2000;
        public override int Width => 2000;
        public override List<GenPass> Tasks => new List<GenPass>()
        {
            new ClownWorldGeneration()
        };

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void Update()
        {
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

    public class ClownWorldGeneration : GenPass
    {
        public ClownWorldGeneration() : base("Terrain", 1) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            Main.worldSurface = Main.maxTilesY - 42; // Hides the underground layer just out of bounds
            Main.rockLayer = Main.maxTilesY; // Hides the cavern layer way out of bounds

            CalRemixHelper.PerlinGeneration(new Rectangle(0, 0, Main.maxTilesX, Main.maxTilesY), noiseThreshold: 0.3f, noiseSize: new Vector2(600, 600), tileType: ModContent.TileType<FunnyBalloonTile>());
            //PlantingHappyTrees();
            WorldGen.AddTrees();
        }

        //TODO: turn this into a more global place trees method for any subworld?
        public static void PlantingHappyTrees()
        {
            for (int num263 = 0; (double)num263 < (double)Main.maxTilesX; num263++)
            {
                int num264 = WorldGen.genRand.Next(50, Main.maxTilesX - 50);
                int num265 = WorldGen.genRand.Next(25, 50);
                for (int num266 = num264 - num265; num266 < num264 + num265; num266++)
                {
                    for (int num267 = 20; (double)num267 < Main.maxTilesY; num267++)
                    {
                        WorldGen.GrowEpicTree(num266, num267);
                    }
                }
            }
            WorldGen.AddTrees();
        }
    }
}
