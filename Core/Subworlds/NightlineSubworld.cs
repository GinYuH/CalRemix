using System.Collections.Generic;
using Terraria;
using SubworldLibrary;
using Terraria.WorldBuilding;
using Terraria.IO;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Terraria.ID;
using CalRemix.Core.World;
using CalRemix.Content.Tiles;
using CalamityMod.Tiles.FurnitureAshen;
using CalRemix.Content.NPCs.Subworlds;
using Terraria.DataStructures;

namespace CalRemix.Core.Subworlds
{
    public class NightlineSubworld : Subworld, IDisableBuilding, IDisableOcean, IDisableFlight, IDisableSpawnsSubworld
    {
        public override int Height => 300;
        public override int Width => 2000;
        public override List<GenPass> Tasks => new List<GenPass>()
        {
            new NightlineGeneration()
        };

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void Update()
        {
            base.Update();
            Main.dayTime = false;
            Main.time = Main.nightLength / 2;
            if (!NPC.AnyNPCs(ModContent.NPCType<Car>()))
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.NewNPC(new EntitySource_WorldEvent(), (Main.spawnTileX + 22) * 16, (Main.spawnTileY - 1) * 16, ModContent.NPCType<Car>());
                }
            }
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
    public class NightlineGeneration : GenPass
    {
        public NightlineGeneration() : base("Terrain", 1) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Generating terrain"; // Sets the text displayed for this pass
            Main.worldSurface = Main.maxTilesY - 142; // Hides the underground layer just out of bounds
            Main.rockLayer = Main.maxTilesY; // Hides the cavern layer way out of bounds
            int concrete = 60;

            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = (int)Main.worldSurface; j < Main.maxTilesY; j++)
                {
                    if (i <= concrete && j < Main.worldSurface + 5)
                    {
                        Main.tile[i, j].ResetToType(TileID.StoneSlab);
                    }
                    else
                    {
                        Main.tile[i, j].ResetToType(TileID.Asphalt);
                    }
                }
            }

            Main.spawnTileX = concrete - 3;
            Main.spawnTileY = (int)Main.worldSurface - 1;

            WorldGen.PlaceObject((int)Main.spawnTileX - 10, (int)Main.spawnTileY, TileID.Lampposts);
            WorldGen.PlaceObject((int)Main.spawnTileX - 6, (int)Main.spawnTileY, TileID.Benches);

            RandomSubworldDoors.GenerateDoorRandom(ModContent.TileType<NightlineDoor>());
        }
    }
}