using CalamityMod.Items.Placeables.FurnitureStatigel;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using static Terraria.WorldGen;

namespace CalRemix.Core.Subworlds
{
    public static class SubworldUpdateMethods
    {
        public static void UpdateLiquids()
        {
            Liquid.skipCount++;
            if (Liquid.skipCount > 1)
            {
                Liquid.UpdateLiquid();
                Liquid.skipCount = 0;
            }
        }

        public static void UpdateTileEntities()
        {
            TileEntity.UpdateStart();
            foreach (TileEntity value in TileEntity.ByID.Values)
            {
                value.Update();
            }
            TileEntity.UpdateEnd();
        }

        public static void UpdateWires()
        {
            Wiring.UpdateMech();
        }


        public static void UpdateWalls() => UpdateTiles();

        public static void UpdateTiles(float multiplier = 1)
        {
            double worldUpdateRate = WorldGen.GetWorldUpdateRate();
            if (worldUpdateRate == 0.0)
            {
                return;
            }
            worldUpdateRate *= multiplier;
            int wallDist = 3;
            double updateRate = 3E-05f * (float)worldUpdateRate;
            double ugUpdateRate = 1.5E-05f * (float)worldUpdateRate;
            double remixUpdateRate = 2.5E-05f * (float)worldUpdateRate;
            bool checkNPCSpawns = false;
            double tileAmt = (double)(Main.maxTilesX * Main.maxTilesY) * updateRate;

            MethodInfo updateInfo = typeof(Terraria.WorldGen).GetMethod("UpdateWorld_OvergroundTile", BindingFlags.Static | BindingFlags.NonPublic);
            MethodInfo updateInfo2 = typeof(Terraria.WorldGen).GetMethod("UpdateWorld_UndergroundTile", BindingFlags.Static | BindingFlags.NonPublic);
            FieldInfo ugGrassInfo = typeof(Terraria.WorldGen).GetField("growGrassUnderground", BindingFlags.Static | BindingFlags.NonPublic);

            for (int j = 0; (double)j < tileAmt; j++)
            {
                int cordX = WorldGen.genRand.Next(10, Main.maxTilesX - 10);
                int cordY = WorldGen.genRand.Next(10, (int)Main.worldSurface - 1);
                updateInfo.Invoke(null, new object[] { cordX, cordY, checkNPCSpawns, wallDist });
            }
            if (Main.remixWorld)
            {
                for (int k = 0; (double)k < (double)(Main.maxTilesX * Main.maxTilesY) * remixUpdateRate; k++)
                {
                    int i3 = WorldGen.genRand.Next(10, Main.maxTilesX - 10);
                    int j3 = WorldGen.genRand.Next((int)Main.worldSurface - 1, Main.maxTilesY - 20);
                    ugGrassInfo.SetValue(null, true);
                    updateInfo2.Invoke(null, new object[] { i3, j3, checkNPCSpawns, wallDist });
                    updateInfo.Invoke(null, new object[] { i3, j3, checkNPCSpawns, wallDist });
                    ugGrassInfo.SetValue(null, false);
                }
            }
            else
            {
                for (int l = 0; (double)l < (double)(Main.maxTilesX * Main.maxTilesY) * ugUpdateRate; l++)
                {
                    int i4 = WorldGen.genRand.Next(10, Main.maxTilesX - 10);
                    int j4 = WorldGen.genRand.Next((int)Main.worldSurface - 1, Main.maxTilesY - 20);
                    updateInfo2.Invoke(null, new object[] { i4, j4, checkNPCSpawns, wallDist });
                }
            }
        }
    }
}