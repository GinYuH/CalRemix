using Terraria;
using Terraria.ModLoader;
using CalamityMod.Tiles.Astral;
using CalamityMod.Tiles.AstralSnow;
using CalamityMod.Walls;
using static Terraria.ModLoader.ModContent;
using CalamityMod.Tiles.AstralDesert;
using Microsoft.Xna.Framework;

namespace CalRemix
{
    public class AstralBlightBiome : ModSystem
    {

        public static int astralGrass = 0;
        public static int astralDirt = 0;
        public static int astralSand = 0;
        public static int astralStone = 0;
        public static int astralSandstone = 0;
        public static int astralHardenedSand = 0;
        public static int astralSnow = 0;
        public static int astralIce = 0;
        public static int astralClay = 0;
        public static int astralWood = 0;
        public static int astralGrassShort = 0;
        public static int astralGrassTall = 0;
        public static int astralDirtWall = 0;
        public static int astralSandstoneWall = 0;
        public static int astralHardenedSandWall = 0;
        public static int astralIceWall = 0;
        public static int astralClayWall = 0;
        public static int astralWoodWall = 0;
        public static int astralStoneWall = 0;
        public static int astralGrassWall = 0;

        public override void PostSetupContent()
        {
            if (ModLoader.HasMod("CalValEX"))
            {
                Mod CalVal = ModLoader.GetMod("CalValEX");
                astralWood = CalVal.Find<ModTile>("AstralTreeWoodPlaced").Type;
                astralStone = CalVal.Find<ModTile>("XenostonePlaced").Type;
                astralGrass = CalVal.Find<ModTile>("AstralGrassPlaced").Type;
                astralSand = CalVal.Find<ModTile>("AstralSandPlaced").Type;
                astralHardenedSand = CalVal.Find<ModTile>("AstralHardenedSandPlaced").Type;
                astralSandstone = CalVal.Find<ModTile>("AstralSandstonePlaced").Type;
                astralClay = CalVal.Find<ModTile>("AstralClayPlaced").Type;
                astralIce = CalVal.Find<ModTile>("AstralIcePlaced").Type;
                astralSnow = CalVal.Find<ModTile>("AstralSnowPlaced").Type;
                astralGrassShort = CalVal.Find<ModTile>("AstralShortGrass").Type;
                astralGrassTall = CalVal.Find<ModTile>("AstralTallGrass").Type;
                astralDirt = CalVal.Find<ModTile>("AstralDirtPlaced").Type;
                astralDirtWall = CalVal.Find<ModWall>("AstralDirtWallPlaced").Type;
                astralStoneWall = CalVal.Find<ModWall>("XenostoneWallPlaced").Type;
                astralIceWall = CalVal.Find<ModWall>("AstralIceWallPlaced").Type;
                astralHardenedSandWall = CalVal.Find<ModWall>("AstralHardenedSandWallPlaced").Type;
                astralSandstoneWall = CalVal.Find<ModWall>("AstralSandstoneWallPlaced").Type;
                astralGrassWall = CalVal.Find<ModWall>("AstralGrassWallPlaced").Type;
                astralWoodWall = CalVal.Find<ModWall>("XenomonolithWallPlaced").Type;
            }
        }

        public static void BlightConvert(int i, int j)
        {
            int k = i;
            int l = j;
            if (WorldGen.InWorld(k, l))
            {
                int type = Main.tile[k, l].TileType;
                int wall = Main.tile[k, l].WallType;

                //Stone
                if (type == TileType<AstralStone>())
                {
                    Main.tile[k, l].TileType = (ushort)astralStone;
                }
                else if (type == TileType<AstralSand>())
                {
                    Main.tile[k, l].TileType = (ushort)astralSand;
                }
                else if (type == TileType<AstralGrass>())
                {
                    Main.tile[k, l].TileType = (ushort)astralGrass;
                }
                else if (type == TileType<AstralDirt>())
                {
                    Main.tile[k, l].TileType = (ushort)astralDirt;
                }
                else if (type == TileType<AstralClay>())
                {
                    Main.tile[k, l].TileType = (ushort)astralClay;
                }
                else if (type == TileType<AstralMonolith>())
                {
                    Main.tile[k, l].TileType = (ushort)astralWood;
                }
                else if (type == TileType<AstralDirt>())
                {
                    Main.tile[k, l].TileType = (ushort)astralDirt;
                }
                else if (type == TileType<AstralMonolith>())
                {
                    Main.tile[k, l].TileType = (ushort)astralWood;
                }
                else if (type == TileType<AstralClay>())
                {
                    Main.tile[k, l].TileType = (ushort)astralClay;
                }
                else if (type == TileType<AstralSnow>())
                {
                    Main.tile[k, l].TileType = (ushort)astralSnow;
                }
                else if (type == TileType<HardenedAstralSand>())
                {
                    Main.tile[k, l].TileType = (ushort)astralHardenedSand;
                }
                else if (type == TileType<AstralShortPlants>())
                {
                    Main.tile[k, l].TileType = (ushort)astralGrassShort;
                }
                else if (type == TileType<AstralTallPlants>())
                {
                    Main.tile[k, l].TileType = (ushort)astralGrassTall;
                    Main.tile[k, l - 1].TileType = (ushort)astralGrassTall;
                }
                else if (type == TileType<AstralSnow>())
                {
                    Main.tile[k, l].TileType = (ushort)astralSnow;
                }
                else if (type == TileType<AstralSandstone>())
                {
                    Main.tile[k, l].TileType = (ushort)astralSandstone;
                }
                else if (type == TileType<AstralIce>())
                {
                    Main.tile[k, l].TileType = (ushort)astralIce;
                }
                if (wall == WallType<AstralSandstoneWall>())
                {
                    Main.tile[k, l].WallType = (ushort)astralSandstoneWall;
                }
                else if (wall == WallType<HardenedAstralSandWall>())
                {
                    Main.tile[k, l].WallType = (ushort)astralHardenedSandWall;
                }
                else if (wall == WallType<AstralGrassWall>())
                {
                    Main.tile[k, l].WallType = (ushort)astralGrassWall;
                }
                else if (wall == WallType<AstralStoneWall>())
                {
                    Main.tile[k, l].WallType = (ushort)astralStoneWall;
                }
                else if (wall == WallType<AstralIceWall>())
                {
                    Main.tile[k, l].WallType = (ushort)astralIceWall;
                }
                else if (type == WallType<AstralDirtWall>())
                {
                    Main.tile[k, l].WallType = (ushort)astralDirtWall;
                    return;
                }
                else if (wall == WallType<AstralMonolithWall>())
                {
                    Main.tile[k, l].WallType = (ushort)astralDirtWall;
                    return;
                }
            }
        }
        public static void GenerateBlight()
        {
            Vector2 pos = Vector2.Zero;
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                if (pos != Vector2.Zero)
                {
                    break;
                }
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    if (Main.tile[i, j].TileType == TileType<AstralBeacon>())
                    {
                        pos = new Vector2(i, j);
                        break;
                    }
                }
            }
            if (pos != Vector2.Zero)
            {
                for (int i = (int)pos.X; i < pos.X + 444; i++)
                {
                    for (int j = (int)pos.Y - 222; j < Main.maxTilesY; j++)
                    {
                        BlightConvert(i, j);
                    }
                }
            }
        }
    }
}