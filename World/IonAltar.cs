using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityMod.World;
using CalRemix.Backgrounds.Plague;
using CalRemix.Projectiles.TileTypeless;
using System.Collections.Generic;
using CalamityMod.Schematics;
using CalamityMod;
using System.IO;
using System.Reflection;
using CalamityMod.Tiles.Abyss;
using System;
using CalRemix.Tiles;
using Terraria.DataStructures;

namespace CalRemix
{
    public class IonAltar : ModSystem
    {
        internal const string IonAltarName = "World/ionaltar.csch";

        internal static Dictionary<string, SchematicMetaTile[,]> TileMaps =>
            typeof(SchematicManager).GetField("TileMaps", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null) as Dictionary<string, SchematicMetaTile[,]>;

        internal static readonly MethodInfo ImportSchematicMethod = typeof(CalamitySchematicIO).GetMethod("ImportSchematic", BindingFlags.NonPublic | BindingFlags.Static);


        public static void GenerateIonAltar()
        {
            bool shouldBreak = false;
            for (int z = 0; z < 2222; z++)
            {
                if (shouldBreak)
                {
                    break;
                }
                for (int i = 100; i < Main.maxTilesX - 100; i++)
                {
                    if (shouldBreak)
                    {
                        break;
                    }
                    for (int j = 0; j < Main.maxTilesY / 2; j++)
                    {
                        Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                        Tile te = CalamityUtils.ParanoidTileRetrieval(i, j - 1);
                        if (shouldBreak)
                        {
                            break;
                        }
                        if (t != null)
                        {
                            if (t.TileType == ModContent.TileType<SulphurousSand>())
                            {
                                if (Main.rand.NextBool(64))
                                {
                                    // check for tiles above, this is ignored if we are on attempt 50
                                    for (int l = 1; l < 22; l++)
                                    {
                                        Tile above = CalamityUtils.ParanoidTileRetrieval(i, j - l);
                                        if (above.HasTile && z < 50)
                                            break;

                                        bool liquidCheck = above.LiquidAmount <= 0;
                                        // If there truly are no dry blocks, increasingly add more wet room
                                        if (z > 22)
                                        {
                                            liquidCheck = above.LiquidAmount <= (z * 5);
                                        }

                                        bool _ = false;
                                        SchematicManager.PlaceSchematic<Action<Chest>>("Ion Altar", new Point(i, j), SchematicAnchor.CenterLeft, ref _);
                                        Main.LocalPlayer.position = new Vector2(i * 16, j * 16);
                                        shouldBreak = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (!shouldBreak)
            {
                CalRemix.instance.Logger.Error("Ion Altar failed to generate!");
            }
            shouldBreak = false;
            for (int i = 100; i < Main.maxTilesX - 100; i++)
            {
                if (shouldBreak)
                {
                    break;
                }
                for (int j = 0; j < Main.maxTilesY - 80; j++)
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);

                    if (t.TileFrameX != 0 || t.TileFrameY != 0)
                        continue;
                    if (t.TileType == ModContent.TileType<IonCubePlaced>())
                    {
                        TileEntity.PlaceEntityNet(i, j, ModContent.TileEntityType<IonCubeTE>());
                        shouldBreak = true;
                        break;
                    }
                }
            }
            if (!shouldBreak)
            {
                CalRemix.instance.Logger.Error("Could not place Ion Cube!");
            }
        }

        public override void PostSetupContent()
        {
            TileMaps.Add("Ion Altar", LoadSchematic(IonAltarName).ShaveOffEdge());
        }


        public static SchematicMetaTile[,] LoadSchematic(string filename)
        {
            SchematicMetaTile[,] ret = null;
            using (Stream st = CalRemix.instance.GetFileStream(filename, true))
                ret = (SchematicMetaTile[,])ImportSchematicMethod.Invoke(null, [st]);

            return ret;
        }
    }
}