using CalamityMod;
using CalamityMod.Schematics;
using CalamityMod.Tiles.FurnitureAshen;
using CalRemix.Content.Items.Weapons;
using CalRemix.Content.NPCs.Bosses.Carcinogen;
using CalRemix.Content.NPCs.Subworlds.Nowhere;
using CalRemix.Content.NPCs.Subworlds.Sealed;
using CalRemix.Content.NPCs.Subworlds.TheGray;
using CalRemix.Content.Projectiles.Weapons;
using CalRemix.Content.Tiles;
using CalRemix.Content.Tiles.Subworlds.Horizon;
using CalRemix.Content.Tiles.Subworlds.TheGray;
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
using Terraria.Graphics.Light;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace CalRemix.Core.Subworlds
{
    public class TheGraySubworld : Subworld, ICustomSpawnSubworld, IDisableOcean, IDisableBuilding, IInfiniteFlight
    {
        public List<(int, float, Predicate<NPCSpawnInfo>)> Spawns()
        {
            List<(int, float, Predicate<NPCSpawnInfo>)> list = [];
            list.Add(item: (ModContent.NPCType<Underscore>(), 22f, (NPCSpawnInfo n) => NPC.CountNPCS(ModContent.NPCType<Underscore>()) < 24));
            return list;
        }

        int ICustomSpawnSubworld.MaxSpawns { get => 24; }
        float ICustomSpawnSubworld.SpawnMult { get => 0.2f; }

        public bool OverrideVanilla => false;
        public override int Height => 300;
        public override int Width => 2000;
        public override List<GenPass> Tasks => new List<GenPass>()
        {
            new GrayGeneration()
        };

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override bool GetLight(Tile tile, int x, int y, ref FastRandom rand, ref Vector3 color)
        {
            color = Vector3.Zero;
            return true;
        }

        public override void Update()
        {
            int light = ModContent.ProjectileType<LightOrbGuiding>();
            if (!CalamityUtils.AnyProjectiles(light) && !Main.LocalPlayer.HasItem(ModContent.ItemType<ParadiseInfusedMurasama>()) && !Main.LocalPlayer.HasItem(ModContent.ItemType<Combosama>()))
            {
                Lighting.Clear();
            }
            foreach (Projectile p in Main.ActiveProjectiles)
            {
                if (p.type == light)
                {
                    Lighting.AddLight(p.Center, 2, 2, 0);
                }
            }
            SubworldSystem.hideUnderworld = true;
            Main.LocalPlayer.ManageSpecialBiomeVisuals("CalRemix:TheGraySky", true);
            SkyManager.Instance.Activate("CalRemix:TheGraySky", Main.LocalPlayer.position);

            base.Update();
        }

        public override void DrawMenu(GameTime gameTime)
        {
            base.DrawMenu(gameTime);
            string str = CalRemixHelper.LocalText("StatusText.Ant").Value;
            Vector2 size = FontAssets.MouseText.Value.MeasureString(str) * 2;

        }
    }
    public class GrayGeneration : GenPass
    {
        public GrayGeneration() : base("Terrain", 1) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Generating terrain"; // Sets the text displayed for this pass
            Main.worldSurface = Main.maxTilesY - 42; // Hides the underground layer just out of bounds
            Main.rockLayer = Main.maxTilesY; // Hides the cavern layer way out of bounds

            int soil = ModContent.TileType<QuestionSoilPlaced>();

            float groundRatio = 0.6f;
            

            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    if (j > (int)(Main.maxTilesY * groundRatio))
                    {
                        WorldGen.PlaceTile(i, j, soil);
                    }
                }
            }

            CalRemixHelper.PerlinSurface(new Rectangle(0, (int)(Main.maxTilesY * groundRatio) - 2, Main.maxTilesX, (int)(Main.maxTilesY * (1f - groundRatio))), soil, variance: 13);

            Main.spawnTileX = (int)(Main.maxTilesX / 2f);
            for (int i = 0; i < Main.maxTilesY; i++)
            {
                if (CalamityUtils.ParanoidTileRetrieval(Main.spawnTileX, i).HasTile)
                {
                    Main.spawnTileY = i - 1;
                    break;
                }
            }

            for (int l = 0; l < 2; l++)
            {
                int spawnX = (int)MathHelper.Lerp((int)(Main.maxTilesX / 8f), (int)(Main.maxTilesX * 0.875f), l / 1f) + WorldGen.genRand.Next(-30, 30);
                int spawnY = 0;
                for (int i = 0; i < Main.maxTilesY; i++)
                {
                    if (CalamityUtils.ParanoidTileRetrieval(Main.spawnTileX, i).HasTile)
                    {
                        spawnY = i;
                        break;
                    }
                }
                if (spawnX % 2 != 0)
                    spawnX++;
                if (spawnY % 2 != 0)
                    spawnY--;
                bool _ = false;
                SchematicManager.PlaceSchematic<Action<Chest>>("Gray Temple", new Point(spawnX, spawnY), SchematicAnchor.BottomLeft, ref _);
            }

            int brick = ModContent.TileType<BlueMazeBrickPlaced>();
            int wall = ModContent.WallType<BlueMazeBrickWallPlaced>();
            int ybrick = ModContent.TileType<YellowMazeBrickPlaced>();
            int ywall = ModContent.WallType<YellowMazeBrickWallPlaced>();

            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    if ((i % 4 < 2 && j % 4 >= 2) || (j % 4 < 2 && i % 4 >= 2))
                    {
                        Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                        if (t.TileType == brick)
                        {
                            t.TileType = (ushort)ybrick;
                        }
                        if (t.WallType == wall)
                        {
                            t.WallType = (ushort)ywall;
                        }
                    }
                }
            }

            RandomSubworldDoors.GenerateDoorRandom(ModContent.TileType<TheGrayDoor>());
        }
    }
}