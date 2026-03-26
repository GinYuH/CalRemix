using CalamityMod;
using CalamityMod.Tiles.FurnitureAshen;
using CalRemix.Content.NPCs.Subworlds;
using CalRemix.Content.NPCs.Subworlds.Pinnacles;
using CalRemix.Content.Tiles;
using CalRemix.Content.Tiles.Subworlds.Wolf;
using CalRemix.Content.Walls;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using SubworldLibrary;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace CalRemix.Core.Subworlds
{
    public class WolfForestSubworld : Subworld, ICustomSpawnSubworld, IDisableOcean
    {
        public List<(int, float, Predicate<NPCSpawnInfo>)> Spawns()
        {
            List<(int, float, Predicate<NPCSpawnInfo>)> list = [];
            list.Add(item: (ModContent.NPCType<DireWolf>(), 1, n => CalamityUtils.ParanoidTileRetrieval(n.SpawnTileX, n.SpawnTileY + 1).HasTile));
            return list;
        }

        int ICustomSpawnSubworld.MaxSpawns { get => 4; }
        float ICustomSpawnSubworld.SpawnMult { get => 0.3f; }

        bool ICustomSpawnSubworld.OverrideVanilla { get => true; }

        public override int Height => 400;
        public override int Width => 400;
        public override List<GenPass> Tasks => new List<GenPass>()
        {
            new WolfGeneration()
        };

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void Update()
        {
            SkyManager.Instance["Ambience"].Deactivate();
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
    public class WolfGeneration : GenPass
    {
        public WolfGeneration() : base("Terrain", 1) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Generating terrain"; // Sets the text displayed for this pass
            Main.worldSurface = Main.maxTilesY - 42; // Hides the underground layer just out of bounds
            Main.rockLayer = Main.maxTilesY; // Hides the cavern layer way out of bounds

            int surface = (int)(Main.maxTilesY * 0.2f);

            ushort earth = (ushort)ModContent.TileType<WolfEarthPlaced>();
            ushort snow = (ushort)ModContent.TileType<WolfSnowPlaced>();
            ushort tree = (ushort)ModContent.WallType<WolfTreePlaced>();


            CalRemixHelper.PerlinSurface(new Rectangle(0, surface, Main.maxTilesX, Main.maxTilesY - surface), earth, variance: 60);
            int treeCd = 0;
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    bool validSnow = false;
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                    if (t.TileType == earth && t.HasTile)
                    {
                        bool hasAir = false;
                        for (int k = i - 1; k < i + 2; k++)
                        {
                            if (hasAir)
                                break;
                            for (int l = j - 1; l < j + 2; l++)
                            {
                                if (l == j && i == k)
                                {
                                    continue;
                                }
                                if (!CalamityUtils.ParanoidTileRetrieval(k, l).HasTile)
                                {
                                    hasAir = true;
                                    break;
                                }
                            }
                        }
                        if (hasAir)
                        {
                            CalamityUtils.ParanoidTileRetrieval(i, j).ResetToType(snow);
                        }

                        if (!CalamityUtils.ParanoidTileRetrieval(i, j - 1).HasTile && treeCd <= 0)
                        {
                            if (WorldGen.genRand.NextBool(5))
                            {
                                int treeHeight = WorldGen.genRand.Next(3, 13);
                                for (int k = j - 1; k > j - treeHeight; k--)
                                {
                                    CalamityUtils.ParanoidTileRetrieval(i, k).WallType = tree;
                                }
                                treeCd = 2;
                            }
                        }
                    }
                }
                treeCd--;
            }

            Main.spawnTileX = (int)(Main.maxTilesX * 0.5f);
            for (int i = 0; i < Main.maxTilesY; i++)
            {
                if (CalamityUtils.ParanoidTileRetrieval(Main.spawnTileX, i).HasTile)
                {
                    Main.spawnTileY = i;
                    break;
                }
            }            

            RandomSubworldDoors.GenerateDoorRandom(ModContent.TileType<WolfDoor>());
        }
    }
}