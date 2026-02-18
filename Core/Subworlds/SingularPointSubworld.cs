using CalamityMod;
using CalRemix.Content.NPCs;
using CalRemix.Content.NPCs.Subworlds.Nowhere;
using CalRemix.Content.NPCs.Subworlds.SingularPoint;
using CalRemix.Content.Tiles;
using CalRemix.Content.Tiles.Subworlds.Nowhere;
using CalRemix.Content.Tiles.Subworlds.SingularPoint;
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
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace CalRemix.Core.Subworlds
{
    public class SingularPointSubworld : Subworld, IDisableSpawnsSubworld, IDisableOcean, IInfiniteFlight, IDisableBuilding
    {
        public override int Height => 300;
        public override int Width => 1400;
        public override List<GenPass> Tasks => new List<GenPass>()
        {
            new SingularPointGeneration()
        };

        public override bool GetLight(Tile tile, int x, int y, ref FastRandom rand, ref Vector3 color)
        {
            if (tile.HasTile)
                color = Color.White.ToVector3() * 0.01f;
            else if (tile.LiquidAmount > 0)
                color = Color.LightSeaGreen.ToVector3();
            else
                color = Color.DarkSeaGreen.ToVector3();
            return false;
        }
        public static void SaveData(string world)
        {
            TagCompound savedWorldData = CalRemixHelper.SaveCommonSubworldBools();
            CalRemixHelper.MakeTag(ref savedWorldData, "Anomaly", RemixDowned.downedAnomaly);

            SubworldSystem.CopyWorldData("RemixCommonBools_" + world, savedWorldData);
        }

        public static void LoadData(string world)
        {
            TagCompound savedWorldData = CalRemixHelper.LoadCommonSubworldBools(world);
            RemixDowned.downedAnomaly = savedWorldData.GetBool("Anomaly");
        }

        public override void CopyMainWorldData() => SaveData("Main");

        public override void ReadCopiedMainWorldData() => LoadData("Main");

        public override void CopySubworldData() => SaveData("SP");

        public override void ReadCopiedSubworldData() => LoadData("SP");

        public static bool dontSpawnAnomaly = false;

        public override void OnEnter()
        {
            base.OnEnter();
            dontSpawnAnomaly = false;
        }

        public override void Update()
        {
            Main.LocalPlayer.ManageSpecialBiomeVisuals("CalRemix:SPSky", true);
            SkyManager.Instance.Activate("CalRemix:SPSky", Main.LocalPlayer.position);
            if (!dontSpawnAnomaly)
            {
                foreach (Player p in Main.ActivePlayers)
                {
                    if (Math.Abs(p.Center.X - Main.maxTilesX * 16 * 0.5f) < 50)
                    {
                        if (!NPC.AnyNPCs(ModContent.NPCType<AnomalyTwo>()))
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                NPC.NewNPC(new EntitySource_WorldEvent(), Main.maxTilesX * 8, Main.maxTilesY * 8 + 1000, ModContent.NPCType<AnomalyTwo>());
                            }
                        }
                    }
                }
            }
            base.Update();
        }

        public override void DrawMenu(GameTime gameTime)
        {
            base.DrawMenu(gameTime);
            string str = CalRemixHelper.LocalText("StatusText.Screaming").Value;
            Vector2 size = FontAssets.MouseText.Value.MeasureString(str) * 2;
            Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, Vector2.Zero, new Rectangle(0, 0, Main.screenWidth * 2, Main.screenHeight * 2), Color.Black, 0, Vector2.Zero, 1, 0, 0);
            Utils.DrawBorderString(Main.spriteBatch,
                str,
                Main.ScreenSize.ToVector2() * 0.5f - size * 0.5f, Color.White, 2);

        }
    }
    public class SingularPointGeneration : GenPass
    {
        public SingularPointGeneration() : base("Terrain", 1) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Generating terrain"; // Sets the text displayed for this pass
            Main.worldSurface = Main.maxTilesY - 42; // Hides the underground layer just out of bounds
            Main.rockLayer = Main.maxTilesY; // Hides the cavern layer way out of bounds


            float surfaceLevel = 0.6f;
            int surfaceHeight = (int)(Main.maxTilesY * surfaceLevel);
            CalRemixHelper.PerlinSurface(new Rectangle(0, surfaceHeight, Main.maxTilesX, Main.maxTilesY - surfaceHeight), ModContent.TileType<VirisitePlaced>());

            float arenaRatio = 0.7f;
            float halfArenaRatio = arenaRatio * 0.5f;
            int arenaStart = (int)(Main.maxTilesX * 0.5f) - (int)(Main.maxTilesX * halfArenaRatio);

            Rectangle arenaArea = new Rectangle(arenaStart, surfaceHeight - 10, (int)(Main.maxTilesX * arenaRatio), Main.maxTilesY - surfaceHeight + 20);

            for (int i = arenaArea.X; i < arenaArea.Right; i++)
            {
                for (int j = arenaArea.Y; j < arenaArea.Bottom; j++)
                {
                    if (CalRemixHelper.WithinElipse(i, j, arenaArea.Center.X, arenaArea.Center.Y, arenaArea.Width / 2, arenaArea.Height / 2))
                    {
                        Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                        t.ClearEverything();
                    }
                }
            }

            float platformRatio = 0.8f;
            arenaArea.Inflate(-(int)(arenaArea.Width * (1 - platformRatio)) - 20, 0);

            for (int i = arenaArea.X; i < arenaArea.Right; i++)
            {
                for (int j = arenaArea.Y; j < arenaArea.Bottom; j++)
                {
                    if (CalRemixHelper.WithinElipse(i, j, arenaArea.Center.X, arenaArea.Center.Y, arenaArea.Width / 2, arenaArea.Height / 2))
                    {
                        Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                        t.ResetToType((ushort)ModContent.TileType<VirisitePlaced>());
                    }
                }
            }

            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = surfaceHeight + 10; j < Main.maxTilesY; j++)
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                    if (!t.HasTile)
                        t.LiquidAmount = 255;
                }
            }

            Main.spawnTileY = (int)(surfaceLevel * Main.maxTilesY);
            Main.spawnTileX = (int)(0.05f * Main.maxTilesX);
        }
    }
}