using System.Collections.Generic;
using Terraria;
using SubworldLibrary;
using Terraria.WorldBuilding;
using Terraria.IO;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using CalRemix.Core.World;
using CalRemix.Content.Tiles;
using Terraria.Graphics.Effects;
using CalRemix.Content.Tiles.Subworlds.Nowhere;
using System;
using CalRemix.Content.NPCs.Subworlds.Nowhere;
using Terraria.Utilities;
using CalRemix.Core.Graphics;

namespace CalRemix.Core.Subworlds
{
    public class NowhereSubworld : Subworld, ICustomSpawnSubworld, IDisableOcean
    {
        public List<(int, float, Predicate<NPCSpawnInfo>)> Spawns()
        {
            List<(int, float, Predicate<NPCSpawnInfo>)> list = [];
            list.Add(item: (ModContent.NPCType<Noone>(), 1f, (NPCSpawnInfo n) => n.Player.Center.Y < (16 * Main.maxTilesY * 0.35f)));
            list.Add(item: (ModContent.NPCType<Nothing>(), 1f, (NPCSpawnInfo n) => n.Player.Center.Y > (16 * Main.maxTilesY * 0.35f) && n.Player.Center.Y < (16 * Main.maxTilesY * 0.6f)));
            return list;
        }

        int ICustomSpawnSubworld.MaxSpawns { get => 12; }
        float ICustomSpawnSubworld.SpawnMult { get => 0.1f; }

        public bool OverrideVanilla => true;        

        public override int Height => 600;
        public override int Width => 800;
        public override List<GenPass> Tasks => new List<GenPass>()
        {
            new NowhereGeneration()
        };

        public override bool GetLight(Tile tile, int x, int y, ref FastRandom rand, ref Vector3 color)
        {
            color = Color.White.ToVector3();
            return false;
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public static Vector2 CameraSavePoint = new();

        public override void Update()
        {
            Main.LocalPlayer.ManageSpecialBiomeVisuals("CalRemix:NowhereSky", true);
            SkyManager.Instance.Activate("CalRemix:NowhereSky", Main.LocalPlayer.position);

            if (Main.LocalPlayer.Center.Y > Main.maxTilesY * 16 * 0.8f)
            {
                if (CameraSavePoint == default)
                {
                    CameraSavePoint = Main.LocalPlayer.Center;
                }
                //Main.blockInput = true;
                //Main.LocalPlayer.mount.Dismount(Main.LocalPlayer);
                CameraPanSystem.CameraFocusPoint = CameraSavePoint;
                CameraPanSystem.CameraPanInterpolant = 1;
                if (Main.LocalPlayer.Center.Y > Main.maxTilesY * 16 * 0.9f)
                {
                    Main.blockInput = false;
                    CameraPanSystem.CameraPanInterpolant = 0;
                    SubworldSystem.Enter<SingularPointSubworld>();
                }
            }
            else
            {
                Main.blockInput = false;
                CameraPanSystem.CameraPanInterpolant = 0;
                CameraSavePoint = Vector2.Zero;
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
    public class NowhereGeneration : GenPass
    {
        public NowhereGeneration() : base("Terrain", 1) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Generating terrain"; // Sets the text displayed for this pass
            Main.worldSurface = Main.maxTilesY - 42; // Hides the underground layer just out of bounds
            Main.rockLayer = Main.maxTilesY; // Hides the cavern layer way out of bounds

            float surfaceLevel = 0.3f;
            float caveStart = 0.4f;
            float emptyLevel = 0.6f;

            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = (int)(Main.maxTilesY * surfaceLevel); j < Main.maxTilesY; j++)
                {
                    if (j < (int)(Main.maxTilesY * caveStart))
                    {
                        WorldGen.PlaceTile(i, j, ModContent.TileType<NowhereBlock>(), true, true);
                    }
                    else if (j < (int)(Main.maxTilesY * emptyLevel))
                    {
                        float comp = Utils.GetLerpValue((int)(Main.maxTilesY * caveStart), (int)(Main.maxTilesY * emptyLevel), j, true);
                        if (WorldGen.genRand.Next((int)MathHelper.Lerp(0, 100, comp)) < MathHelper.Lerp(20, 1, comp))
                        {
                            WorldGen.PlaceTile(i, j, ModContent.TileType<NowhereBlock>(), true, true);
                        }
                    }
                    if (j == (int)Main.maxTilesY * surfaceLevel)
                    {
                        WorldGen.PlaceObject(i, j - 1, ModContent.TileType<NowhereBacking>(), true, i % 12);
                    }
                }
            }
            Main.spawnTileY = (int)(surfaceLevel * Main.maxTilesY);
        }
    }
}