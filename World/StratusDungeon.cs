using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using CalamityMod;
using CalamityMod.Tiles.FurnitureStratus;
using CalRemix.Walls;

namespace CalRemix
{
    public class StratusDungeon : ModSystem
    {

        public static void ReplaceDungeon()
        {
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                    if (t != null && t.HasTile)
                    {
                        switch (t.TileType)
                        {
                            case TileID.BlueDungeonBrick:
                            case TileID.PinkDungeonBrick:
                            case TileID.GreenDungeonBrick:
                                {
                                    //if (!Main.tile[i, j - 1].HasTile)
                                    //continue;
                                    t.TileType = (ushort)TileType<StratusBricks>();
                                }
                                break;
                            case TileID.CrackedBlueDungeonBrick:
                            case TileID.CrackedPinkDungeonBrick:
                            case TileID.CrackedGreenDungeonBrick:
                                WorldGen.KillTile(i, j);
                                break;
                            case TileID.Chairs:
                                {
                                    ReplaceFurniture(ref t, TileType<StratusChair>(), new int[0], new int[4] { 520, 560, 600, 680 });
                                }
                                break;
                            case TileID.Toilets:
                                {
                                    ReplaceFurniture(ref t, TileType<StratusChair>(), new int[0], new int[3] { 440, 480, 520 });
                                }
                                break;
                            case TileID.Tables:
                                {
                                    ReplaceFurniture(ref t, TileType<StratusTable>(), new int[4] { 540, 594, 648, 756 }, new int[0], height: 2);
                                }
                                break;
                            case TileID.OpenDoor:
                                {
                                    ReplaceFurniture(ref t, TileType<StratusDoorOpen>(), new int[0], new int[4] { 702, 864, 972, 918 });
                                }
                                break;
                            case TileID.ClosedDoor:
                                {
                                    ReplaceFurniture(ref t, TileType<StratusDoorClosed>(), new int[0], new int[4] { 702, 864, 972, 918 });
                                }
                                break;
                            case TileID.Platforms:
                                if (t.TileFrameY > 90 && t.TileFrameY < 232)
                                {
                                    if (Main.rand.NextBool())
                                    {
                                        t.TileType = (ushort)TileType<StratusPlatform>();
                                    }
                                    else
                                    {
                                        t.TileType = (ushort)TileType<StratusStarPlatform>();
                                    }
                                    t.TileFrameY = 0;
                                }
                                break;
                            case TileID.Sinks:
                                {
                                    ReplaceFurniture(ref t, TileType<StratusSink>(), new int[0], new int[3] { 380, 418, 456 }, 2, 2);
                                }
                                break;
                            case TileID.Lamps:
                                {
                                    ReplaceFurniture(ref t, TileType<StratusLamp>(), new int[0], new int[3] { 1296, 1350, 1404 }, 1);
                                }
                                break;
                            case TileID.HangingLanterns:
                                {
                                    if (t.TileFrameY < 250)
                                    {
                                        ReplaceFurniture(ref t, TileType<StratusLantern>(), y: 2);
                                    }
                                }
                                break;
                            case TileID.Bookcases:
                                {
                                    ReplaceFurniture(ref t, TileType<StratusBookcase>(), new int[4] { 54, 108, 162, 270 }, new int[0]);
                                }
                                break;
                            case TileID.Candelabras:
                                {
                                    ReplaceFurniture(ref t, TileType<StratusCandelabra>(), new int[0], new int[3] { 792, 864, 828 }, 2, 2);
                                }
                                break;
                            case TileID.WaterCandle:
                                t.TileType = (ushort)TileType<StratusCandle>();
                                break;
                            case TileID.Candles:
                                if (t.TileFrameY >= 22 && t.TileFrameY <= 66)
                                {
                                    t.TileType = (ushort)TileType<StratusCandle>();
                                    t.TileFrameY = 0;
                                }
                                break;
                            case TileID.Pianos:
                                {
                                    ReplaceFurniture(ref t, TileType<StratusPiano>(), new int[3] { 594, 702, 648 }, new int[0], height: 2);
                                }
                                break;
                            case TileID.Dressers:
                                {
                                    ReplaceFurniture(ref t, TileType<StratusDresser>(), new int[3] { 378, 324, 270 }, new int[0], height: 2);
                                }
                                break;
                            case TileID.WorkBenches:
                                {
                                    ReplaceFurniture(ref t, TileType<StratusWorkBenchTile>(), new int[4] { 396, 432, 468, 540 }, new int[0], 2);
                                }
                                break;
                            case TileID.GrandfatherClocks:
                                {
                                    ReplaceFurniture(ref t, TileType<StratusClock>(), new int[3] { 1080, 1116, 1152 }, new int[0], 2);
                                }
                                break;
                            case TileID.Beds:
                                {
                                    ReplaceFurniture(ref t, TileType<StratusBed>(), new int[0], new int[3] { 180, 216, 252 }, height: 2);
                                }
                                break;
                            case TileID.Bathtubs:
                                {
                                    ReplaceFurniture(ref t, TileType<StratusBathtub>(), new int[0], new int[3] { 756, 792, 828 }, height: 2);
                                }
                                break;
                            case TileID.Containers:
                                {
                                    if (t.TileFrameX > 70 && t.TileFrameX < 107)
                                    {
                                        t.TileType = (ushort)TileType<StratusChest>();
                                        short frame = (short)(t.TileFrameX % 36 == 18 ? 19 : 0);
                                        t.TileFrameX = frame;
                                    }
                                }
                                break;
                            case TileID.Chandeliers:
                                {
                                    ReplaceFurniture(ref t, TileType<StratusChandelier>(), new int[0], new int[3] { 1458, 1512, 1566 });
                                }
                                break;
                            case TileID.Benches:
                                {
                                    ReplaceFurniture(ref t, TileType<StratusSofa>(), new int[3] { 324, 378, 432 }, new int[0], height: 2);
                                }
                                break;
                        }
                    }
                    //if (t != null)
                    {
                        switch (t.WallType)
                        {
                            case WallID.BlueDungeonUnsafe:
                            case WallID.PinkDungeonUnsafe:
                            case WallID.GreenDungeonUnsafe:
                            case WallID.BlueDungeonSlabUnsafe:
                            case WallID.PinkDungeonSlabUnsafe:
                            case WallID.GreenDungeonSlabUnsafe:
                            case WallID.BlueDungeonTileUnsafe:
                            case WallID.PinkDungeonTileUnsafe:
                            case WallID.GreenDungeonTileUnsafe:
                            case WallID.BlueDungeon:
                            case WallID.PinkDungeon:
                            case WallID.GreenDungeon:
                            case WallID.BlueDungeonSlab:
                            case WallID.PinkDungeonSlab:
                            case WallID.GreenDungeonSlab:
                            case WallID.BlueDungeonTile:
                            case WallID.PinkDungeonTile:
                            case WallID.GreenDungeonTile:
                                {
                                    int radii = WorldGen.GetWorldSize() + 2;
                                    if ((Main.tile[i + radii, j].WallType > 0 || Main.tile[i - radii, j].WallType > 0) || j > Main.worldSurface) // keep the surface pillars intact for the cultists. remove whenever we fuck up the cultist spawn
                                        t.WallType = (ushort)WallType<StratusWallRemix>();
                                }
                                break;
                        }
                    }
                }
            }
        }

        public static void ReplaceFurniture(ref Tile originalTile, int typer, int[] xIDs, int[] yIDs, int width = 3, int height = 3)
        {
            //72 126 180
            if (xIDs.Length > 0)
            {
                for (int i = 0; i < xIDs.Length; i++)
                {
                    int min = xIDs[i];
                    int frame2 = xIDs[i] + 18;
                    int frame3 = xIDs[i] + 36;
                    int frameMax = width == 3 ? frame3 : frame2;
                    if (originalTile.TileFrameX >= xIDs[i] && originalTile.TileFrameX <= frameMax)
                    {
                        int frameToUse = 0;
                        if (originalTile.TileFrameX == xIDs[i])
                        {
                            frameToUse = 0;
                        }
                        if (width >= 2)
                        {
                            if (originalTile.TileFrameX == frame2)
                            {
                                frameToUse = 18;
                            }
                        }
                        if (width >= 3)
                        {
                            if (originalTile.TileFrameX == frame3)
                            {
                                frameToUse = 36;
                            }
                        }
                        originalTile.TileType = (ushort)typer;
                        originalTile.TileFrameX = (short)(frameToUse);
                    }
                }
            }
            if (yIDs.Length > 0)
            {
                for (int i = 0; i < yIDs.Length; i++)
                {
                    int min = yIDs[i];
                    int frame2 = yIDs[i] + 18;
                    int frame3 = yIDs[i] + 36;
                    int frameMax = height == 3 ? frame3 : frame2;
                    if (originalTile.TileFrameY >= yIDs[i] && originalTile.TileFrameY <= frameMax)
                    {
                        int frameToUse = 0;
                        if (originalTile.TileFrameY == yIDs[i])
                        {
                            frameToUse = 0;
                        }
                        if (height >= 2)
                        {
                            if (originalTile.TileFrameY == frame2)
                            {
                                frameToUse = 18;
                            }
                        }
                        if (height >= 3)
                        {
                            if (originalTile.TileFrameY == frame3)
                            {
                                frameToUse = 36;
                            }
                        }
                        originalTile.TileType = (ushort)typer;
                        originalTile.TileFrameY = (short)(frameToUse);
                    }
                }
            }
        }

        public static void ReplaceFurniture(ref Tile originalTile, int type, int x = 1, int y = 1)
        {
            {
                originalTile.TileType = (ushort)type;
                short frameX = originalTile.TileFrameX;
                short frameY = originalTile.TileFrameY;
                if (x == 2)
                {
                    frameX = (short)(originalTile.TileFrameX % 36 == 0 ? 0 : 18);
                }
                if (x == 3)
                {
                    frameX = (short)(originalTile.TileFrameX % 54 == 0 ? 0 : originalTile.TileFrameX % 36 == 0 ? 18 : 36);
                }
                if (y == 2)
                {
                    frameY = (short)(originalTile.TileFrameY % 36 == 0 ? 0 : 18);
                }
                if (y == 3)
                {
                    frameY = (short)(originalTile.TileFrameY % 54 == 0 ? 0 : originalTile.TileFrameY % 36 == 0 ? 18 : 36);
                }
                originalTile.TileFrameX = frameX;
                originalTile.TileFrameY = frameY;
            }
        }

        public static void AddOriginalDungeonHoles()
        {
            int pocketCount = 0;
            for (int be = 0; be < 200; be++)
            {
                if (pocketCount >= 22)
                    break;
                for (int i = 0; i < Main.maxTilesX; i++)
                {
                    for (int j = (int)(Main.maxTilesY * 0.4f); j < Main.maxTilesY; j++)
                    {
                        if (Main.rand.NextBool(1000))
                        {
                            if (Main.tile[i, j].TileType == ModContent.TileType<StratusBricks>())
                            {
                                int planetradius = Main.rand.Next(11, 22);
                                int brick = Main.rand.Next(3);
                                int wall = Main.rand.Next(3);
                                switch (brick)
                                {
                                    case 0:
                                        brick = TileID.PinkDungeonBrick;
                                        switch (wall)
                                        {
                                            case 0:
                                                wall = WallID.PinkDungeonUnsafe;
                                                break;
                                            case 1:
                                                wall = WallID.PinkDungeonSlabUnsafe;
                                                break;
                                            case 2:
                                                wall = WallID.PinkDungeonTileUnsafe;
                                                break;
                                        }
                                        break;
                                    case 1:
                                        brick = TileID.GreenDungeonBrick;
                                        switch (wall)
                                        {
                                            case 0:
                                                wall = WallID.GreenDungeonUnsafe;
                                                break;
                                            case 1:
                                                wall = WallID.GreenDungeonSlabUnsafe;
                                                break;
                                            case 2:
                                                wall = WallID.GreenDungeonTileUnsafe;
                                                break;
                                        }
                                        break;
                                    case 2:
                                        brick = TileID.BlueDungeonBrick;
                                        switch (wall)
                                        {
                                            case 0:
                                                wall = WallID.BlueDungeonUnsafe;
                                                break;
                                            case 1:
                                                wall = WallID.BlueDungeonSlabUnsafe;
                                                break;
                                            case 2:
                                                wall = WallID.BlueDungeonTileUnsafe;
                                                break;
                                        }
                                        break;
                                }
                                for (int p = i - planetradius; p < i + planetradius; p++)
                                {
                                    for (int q = j - planetradius; q < j + planetradius; q++)
                                    {
                                        int dist = (p - i) * (p - i) + (q - j) * (q - j);
                                        if (dist > planetradius * planetradius)
                                            continue;

                                        if (WorldGen.InWorld(p, q, 1) && Main.tile[p, q].HasTile)
                                        {
                                            if (Main.tile[p, q].TileType == TileType<StratusBricks>())
                                            {
                                                Main.tile[p, q].TileType = (ushort)brick;
                                            }
                                        }
                                        if (Main.tile[p, q].WallType == WallType<StratusWallRemix>())
                                        {
                                            Main.tile[p, q].WallType = (ushort)wall;
                                        }
                                    }
                                }
                                pocketCount++;
                            }
                        }
                    }
                }
            }
        }
    }
}