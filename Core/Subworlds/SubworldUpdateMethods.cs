using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalRemix.Core.Subworlds
{
    public class SubworldUpdateMethods : ModSystem
    {
        public static MethodInfo updateOvergroundInfo;
        public static MethodInfo updateUndergroundInfo;
        public static FieldInfo ugGrassFieldButNotLikeAnActualGrassFieldTheGrassIsntGrowingOnTheFieldButInIt;

        public override void Load()
        {
            updateOvergroundInfo = typeof(WorldGen).GetMethod("UpdateWorld_OvergroundTile", BindingFlags.Static | BindingFlags.NonPublic);
            updateUndergroundInfo = typeof(WorldGen).GetMethod("UpdateWorld_UndergroundTile", BindingFlags.Static | BindingFlags.NonPublic);
            ugGrassFieldButNotLikeAnActualGrassFieldTheGrassIsntGrowingOnTheFieldButInIt = typeof(WorldGen).GetField("growGrassUnderground", BindingFlags.Static | BindingFlags.NonPublic);
        }

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

            for (int j = 0; (double)j < tileAmt; j++)
            {
                int cordX = WorldGen.genRand.Next(10, Main.maxTilesX - 10);
                int cordY = WorldGen.genRand.Next(10, (int)Main.worldSurface - 1);
                updateOvergroundInfo.Invoke(null, new object[] { cordX, cordY, checkNPCSpawns, wallDist });
            }
            if (Main.remixWorld)
            {
                for (int k = 0; (double)k < (double)(Main.maxTilesX * Main.maxTilesY) * remixUpdateRate; k++)
                {
                    int i3 = WorldGen.genRand.Next(10, Main.maxTilesX - 10);
                    int j3 = WorldGen.genRand.Next((int)Main.worldSurface - 1, Main.maxTilesY - 20);
                    ugGrassFieldButNotLikeAnActualGrassFieldTheGrassIsntGrowingOnTheFieldButInIt.SetValue(null, true);
                    updateUndergroundInfo.Invoke(null, new object[] { i3, j3, checkNPCSpawns, wallDist });
                    updateOvergroundInfo.Invoke(null, new object[] { i3, j3, checkNPCSpawns, wallDist });
                    ugGrassFieldButNotLikeAnActualGrassFieldTheGrassIsntGrowingOnTheFieldButInIt.SetValue(null, false);
                }
            }
            else
            {
                for (int l = 0; (double)l < (double)(Main.maxTilesX * Main.maxTilesY) * ugUpdateRate; l++)
                {
                    int i4 = WorldGen.genRand.Next(10, Main.maxTilesX - 10);
                    int j4 = WorldGen.genRand.Next((int)Main.worldSurface - 1, Main.maxTilesY - 20);
                    updateUndergroundInfo.Invoke(null, new object[] { i4, j4, checkNPCSpawns, wallDist });
                }
            }
        }
    }
}