using CalRemix.Content.Tiles.Subworlds.ClownWorld;
using CalRemix.Content.Tiles.Subworlds.MoonGraveyard;
using Microsoft.Xna.Framework;
using SubworldLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using static CalRemix.Core.Subworlds.SubworldHelpers;

namespace CalRemix.Core.Subworlds
{
    #region subworld and worldgen
    public class MoonGraveyardSubworld : Subworld
    {
        public override int Height => 500;
        public override int Width => 1000;
        public override List<GenPass> Tasks => new List<GenPass>()
        {
            new MoonGraveyardGeneration()
        };

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void Update()
        {
            Main.LocalPlayer.ZoneBeach = false;
            // make it night. eternal torment... the torcher never stops...
            Main.dayTime = false;
            Main.time = Main.nightLength / 2;
            // make the moon invis... unless u use a texture pack that makes the new moon have a texture.
            // but texture pack users are subhuman anyways sooo
            Main.moonPhase = (int)MoonPhase.Empty;
            Main.moonType = 0;
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

    public class MoonGraveyardGeneration : GenPass
    {
        public MoonGraveyardGeneration() : base("Terrain", 1) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            GenerateBasicTerrain_PlateausAndPlateausOnly();
            ReplaceTilesWithMoonstone();
            SmoothWorld();
            SpawnPoint();
            PlaceGravestones();

            Main.worldSurface = Main.maxTilesY - 42; // Hides the underground layer just out of bounds
            Main.rockLayer = Main.maxTilesY; // Hides the cavern layer way out of bounds
        }

        public static void GenerateBasicTerrain_PlateausAndPlateausOnly()
        {
            TerrainFeatureType terrainFeatureType = TerrainFeatureType.Plateau;
            int terrainFeatureChangeTimer = 0;

            // making a bunch of vassal variables to contain temp stuff and be modified
            double surfaceVassal = (double)Main.maxTilesY * 0.5;
            double rockVassal = surfaceVassal;
            double surfaceLowVassal = surfaceVassal;
            double surfaceHighVassal = surfaceVassal;
            double rockLowVassal = rockVassal;
            double rockHighVassal = rockVassal;
            SurfaceHistory surfaceHistory = new SurfaceHistory(500);

            for (int i = 0; i < Main.maxTilesX; i++)
            {
                // each loop, choose the lower/higher value of the two
                surfaceLowVassal = Math.Min(surfaceVassal, surfaceLowVassal);
                surfaceHighVassal = Math.Max(surfaceVassal, surfaceHighVassal);
                rockLowVassal = Math.Min(rockVassal, rockLowVassal);
                rockHighVassal = Math.Max(rockVassal, rockHighVassal);

                // normally theres code here to do sutff.. but i got rid of it!! only plateaus here..

                // add the world surface offset to the surface vassal value
                // without doing this, you get no surface!! just a flat!!!! nothing!!!!! flat earth!!!!!!!!!
                surfaceVassal += GenerateWorldSurfaceOffset(terrainFeatureType);



                // record surface stuff and gen the surface
                surfaceHistory.Record(surfaceVassal);
                FillColumn(i, surfaceVassal, rockVassal);
            }

            // set a bunch of variables that help the rest of world generation
            // mainly determining where layers for things start
            Main.worldSurface = (int)(surfaceHighVassal + 25.0);
            Main.rockLayer = rockHighVassal;
            double num4 = (int)((Main.rockLayer - Main.worldSurface) / 6.0) * 6;
            Main.rockLayer = (int)(Main.worldSurface + num4);
            int waterLine = (int)(Main.rockLayer + (double)Main.maxTilesY) / 2 + WorldGen.genRand.Next(-100, 20);
            int lavaLine = waterLine + WorldGen.genRand.Next(50, 80);
            int num5 = 20;
            if (rockLowVassal < surfaceHighVassal + (double)num5)
            {
                double num16 = (rockLowVassal + surfaceHighVassal) / 2.0;
                double num6 = Math.Abs(rockLowVassal - surfaceHighVassal);
                if (num6 < (double)num5)
                {
                    num6 = num5;
                }
                rockLowVassal = num16 + num6 / 2.0;
                surfaceHighVassal = num16 - num6 / 2.0;
            }

            GenVars.rockLayer = rockVassal;
            GenVars.rockLayerHigh = rockHighVassal;
            GenVars.rockLayerLow = rockLowVassal;
            GenVars.worldSurface = surfaceVassal;
            GenVars.worldSurfaceHigh = surfaceHighVassal;
            GenVars.worldSurfaceLow = surfaceLowVassal;
            GenVars.waterLine = waterLine;
            GenVars.lavaLine = lavaLine;
        }
        private static void ReplaceTilesWithMoonstone()
        {
            for (int y = 0; y <= Main.maxTilesY; y++)
            {
                for (int x = 0; x <= Main.maxTilesX; x++)
                {
                    Tile currentTile;
                    currentTile = Main.tile[x, y];
                    if (currentTile.WallType == WallID.DirtUnsafe)
                    {
                        currentTile = Main.tile[x, y];
                        currentTile.WallType = WallID.SnowWallUnsafe;
                    }
                    currentTile = Main.tile[x, y];
                    switch (currentTile.TileType)
                    {
                        case TileID.Dirt:
                        case TileID.Grass:
                        case TileID.CorruptGrass:
                        case TileID.ClayBlock:
                        case TileID.Stone:
                            currentTile = Main.tile[x, y];
                            currentTile.TileType = (ushort)ModContent.TileType<MoonstoneTile>();
                            break;
                    }

                }
            }
        }
        private static void PlaceGravestones()
        {
            for (int x = 1; x < Main.maxTilesX; x++)
            {
                for (int y = 1; y < Main.maxTilesY; y++)
                {
                    if (WorldGen.genRand.NextBool(10))
                    {
                        int graveStyle = WorldGen.genRand.Next(6);
                        if (TileObject.CanPlace(x, y, TileID.Tombstones, graveStyle, 0, out _))
                        {
                            WorldGen.PlaceObject(x, y, TileID.Tombstones, true, graveStyle);

                            // this is for adding text to graves
                            int signID = Sign.ReadSign(x, y);

                            Sign.TextSign(signID, "");
                            /*if (WorldGen.genRand.NextBool(100))
                            {
                                Sign.TextSign(signID, "cool");
                            }*/
                        }
                    }
                }

            }
        }
    }
    #endregion

    #region biome and bg visuals

    #endregion

    public class MoonGraveyardMakeGraveyardBiomeLeaveMeAlone : ModSystem
    {
        private int OG_GraveyardTileMax;
        private int OG_GraveyardTileMin;
        private int OG_GraveyardTileThreshold;

        public override void Load()
        {
            base.Load();
            OG_GraveyardTileMax = SceneMetrics.GraveyardTileMax;
            OG_GraveyardTileMin = SceneMetrics.GraveyardTileMin;
            OG_GraveyardTileThreshold = SceneMetrics.GraveyardTileThreshold;
        }

        public override void OnWorldLoad()
        {
            base.OnWorldLoad();
            if (SubworldSystem.IsActive<MoonGraveyardSubworld>())
            {
                // the numbers have to be like this
                SceneMetrics.GraveyardTileMax = 501;
                SceneMetrics.GraveyardTileMin = 500;
                SceneMetrics.GraveyardTileThreshold = 499;
            }
            else
            {
                SceneMetrics.GraveyardTileMax = OG_GraveyardTileMax;
                SceneMetrics.GraveyardTileMin = OG_GraveyardTileMin;
                SceneMetrics.GraveyardTileThreshold = OG_GraveyardTileThreshold;
            }
        }
    }
}
