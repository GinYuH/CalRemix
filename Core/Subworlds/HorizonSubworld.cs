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
using CalRemix.Content.Tiles.Subworlds.Horizon;
using Terraria.Graphics.Effects;
using Terraria.DataStructures;
using CalRemix.Content.NPCs.Subworlds.Sealed;
using CalRemix.Content.NPCs.Bosses.Carcinogen;

namespace CalRemix.Core.Subworlds
{
    public class HorizonSubworld : Subworld, IDisableSpawnsSubworld, IDisableOcean
    {
        public override int Height => 700;
        public override int Width => 2000;
        public override List<GenPass> Tasks => new List<GenPass>()
        {
            new HorizonGeneration()
        };

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void Update()
        {
            SubworldSystem.hideUnderworld = true;
            Main.LocalPlayer.ManageSpecialBiomeVisuals("CalRemix:HorizonSky", true);
            SkyManager.Instance.Activate("CalRemix:HorizonSky", Main.LocalPlayer.position);

            foreach (Player p in Main.ActivePlayers)
            {
                if (p.Distance(new Vector2(Main.spawnTileX, Main.spawnTileY) * 16) < 1000)
                {
                    if (!NPC.AnyNPCs(ModContent.NPCType<VigorCloak>()) && !NPC.AnyNPCs(ModContent.NPCType<Crevivence>()))
                        NPC.NewNPC(new EntitySource_WorldEvent(), Main.spawnTileX * 16, Main.spawnTileY * 16, ModContent.NPCType<VigorCloak>());
                }
            }

            base.Update();
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
    public class HorizonGeneration : GenPass
    {
        public HorizonGeneration() : base("Terrain", 1) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Generating terrain"; // Sets the text displayed for this pass
            Main.worldSurface = Main.maxTilesY - 42; // Hides the underground layer just out of bounds
            Main.rockLayer = Main.maxTilesY; // Hides the cavern layer way out of bounds

            int grass = ModContent.TileType<HorizonGrass>();

            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    if (j > (int)(Main.maxTilesY * 0.9f))
                    {
                        WorldGen.PlaceTile(i, j, grass);
                    }
                }
            }

            Main.spawnTileX = (int)(Main.maxTilesX / 2f);
            Main.spawnTileY = (int)(Main.maxTilesY * 0.9f - 1);

            RandomSubworldDoors.GenerateDoorRandom(ModContent.TileType<HorizonDoor>());
        }
    }
}