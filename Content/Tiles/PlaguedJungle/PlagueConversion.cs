using CalamityMod.Projectiles.Summon;
using CalamityMod.Tiles.Astral;
using CalamityMod.Tiles.AstralDesert;
using CalamityMod.Walls;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles.PlaguedJungle
{
    public class PlagueConversion : ModBiomeConversion
    {
        public override string Name => "Plague";

        public override void PostSetupContent()
        {
            TileLoader.RegisterSimpleConversion(TileID.Grass, Type, ModContent.TileType<PlaguedGrass>());
            TileLoader.RegisterSimpleConversion(TileID.JungleGrass, Type, ModContent.TileType<PlaguedGrass>());
            TileLoader.RegisterSimpleConversion(TileID.Dirt, Type, ModContent.TileType<PlaguedMud>());
            TileLoader.RegisterSimpleConversion(ModContent.TileType<AstralDirt>(), Type, ModContent.TileType<PlaguedClay>());
            TileLoader.RegisterSimpleConversion(TileID.Mud, Type, ModContent.TileType<PlaguedMud>());
            TileLoader.RegisterSimpleConversion(TileID.Stone, Type, ModContent.TileType<PlaguedStone>());
            TileLoader.RegisterSimpleConversion(TileID.Silt, Type, ModContent.TileType<PlaguedSilt>());
            TileLoader.RegisterSimpleConversion(ModContent.TileType<CelestialRemains>(), Type, ModContent.TileType<PlaguedClay>());
            TileLoader.RegisterSimpleConversion(TileID.Sand, Type, ModContent.TileType<PlaguedSand>());
            TileLoader.RegisterSimpleConversion(TileID.LivingWood, Type, ModContent.TileType<UndeadPlaguePipe>());
            TileLoader.RegisterSimpleConversion(TileID.LivingMahogany, Type, ModContent.TileType<UndeadPlaguePipe>());
            TileLoader.RegisterSimpleConversion(ModContent.TileType<AstralMonolith>(), Type, ModContent.TileType<PlaguedClay>());
            TileLoader.RegisterSimpleConversion(TileID.Hive, Type, ModContent.TileType<PlaguedHive>());
            TileLoader.RegisterSimpleConversion(TileID.Granite, Type, ModContent.TileType<PlaguedHive>());
            TileLoader.RegisterSimpleConversion(TileID.Marble, Type, ModContent.TileType<PlaguedHive>());
            TileLoader.RegisterSimpleConversion(TileID.ClayBlock, Type, ModContent.TileType<PlaguedClay>());
            TileLoader.RegisterSimpleConversion(ModContent.TileType<AstralClay>(), Type, ModContent.TileType<PlaguedClay>());

            TileLoader.RegisterSimpleConversion(TileID.Copper, Type, ModContent.TileType<Sporezol>());
            TileLoader.RegisterSimpleConversion(TileID.Iron, Type, ModContent.TileType<Sporezol>());
            TileLoader.RegisterSimpleConversion(TileID.Silver, Type, ModContent.TileType<Sporezol>());
            TileLoader.RegisterSimpleConversion(TileID.Gold, Type, ModContent.TileType<Sporezol>());
            TileLoader.RegisterSimpleConversion(TileID.Demonite, Type, ModContent.TileType<Sporezol>());
            TileLoader.RegisterSimpleConversion(TileID.Tin, Type, ModContent.TileType<Sporezol>());
            TileLoader.RegisterSimpleConversion(TileID.Lead, Type, ModContent.TileType<Sporezol>());
            TileLoader.RegisterSimpleConversion(TileID.Tungsten, Type, ModContent.TileType<Sporezol>());
            TileLoader.RegisterSimpleConversion(TileID.Platinum, Type, ModContent.TileType<Sporezol>());
            TileLoader.RegisterSimpleConversion(TileID.Crimtane, Type, ModContent.TileType<Sporezol>());

            WallLoader.RegisterSimpleConversion(WallID.DirtUnsafe, Type, ModContent.WallType<PlaguedMudWall>());
            WallLoader.RegisterSimpleConversion(WallID.MudUnsafe, Type, ModContent.WallType<PlaguedMudWall>());
            WallLoader.RegisterSimpleConversion(WallID.DirtUnsafe1, Type, ModContent.WallType<PlaguedMudWall>());
            WallLoader.RegisterSimpleConversion(WallID.DirtUnsafe2, Type, ModContent.WallType<PlaguedMudWall>());
            WallLoader.RegisterSimpleConversion(WallID.DirtUnsafe3, Type, ModContent.WallType<PlaguedMudWall>());
            WallLoader.RegisterSimpleConversion(WallID.DirtUnsafe4, Type, ModContent.WallType<PlaguedMudWall>());
            WallLoader.RegisterSimpleConversion(ModContent.WallType<AstralDirtWall>(), Type, ModContent.WallType<PlaguedVineWall>());
            WallLoader.RegisterSimpleConversion(WallID.Stone, Type, ModContent.WallType<PlaguedStoneWall>());
            WallLoader.RegisterSimpleConversion(WallID.CaveUnsafe, Type, ModContent.WallType<PlaguedStoneWall>());
            WallLoader.RegisterSimpleConversion(WallID.Cave2Unsafe, Type, ModContent.WallType<PlaguedStoneWall>());
            WallLoader.RegisterSimpleConversion(WallID.Cave3Unsafe, Type, ModContent.WallType<PlaguedStoneWall>());
            WallLoader.RegisterSimpleConversion(WallID.Cave4Unsafe, Type, ModContent.WallType<PlaguedStoneWall>());
            WallLoader.RegisterSimpleConversion(WallID.Cave5Unsafe, Type, ModContent.WallType<PlaguedStoneWall>());
            WallLoader.RegisterSimpleConversion(WallID.Cave6Unsafe, Type, ModContent.WallType<PlaguedStoneWall>());
            WallLoader.RegisterSimpleConversion(WallID.Cave7Unsafe, Type, ModContent.WallType<PlaguedStoneWall>());
            WallLoader.RegisterSimpleConversion(WallID.Cave8Unsafe, Type, ModContent.WallType<PlaguedStoneWall>());
            WallLoader.RegisterSimpleConversion(WallID.LivingWoodUnsafe, Type, ModContent.WallType<PlaguedPipeWall>());
            WallLoader.RegisterSimpleConversion(ModContent.WallType<AstralMonolithWall>(), Type, ModContent.WallType<PlaguedVineWall>());
            WallLoader.RegisterSimpleConversion(WallID.HiveUnsafe, Type, ModContent.WallType<PlaguedHiveWall>());
            WallLoader.RegisterSimpleConversion(WallID.GraniteUnsafe, Type, ModContent.WallType<PlaguedHiveWall>());
            WallLoader.RegisterSimpleConversion(WallID.MarbleUnsafe, Type, ModContent.WallType<PlaguedHiveWall>());
            WallLoader.RegisterSimpleConversion(WallID.JungleUnsafe, Type, ModContent.WallType<PlaguedVineWall>());
            WallLoader.RegisterSimpleConversion(WallID.GrassUnsafe, Type, ModContent.WallType<PlaguedVineWall>());
            WallLoader.RegisterSimpleConversion(WallID.JungleUnsafe1, Type, ModContent.WallType<PlaguedVineWall>());
            WallLoader.RegisterSimpleConversion(WallID.JungleUnsafe2, Type, ModContent.WallType<PlaguedVineWall>());
            WallLoader.RegisterSimpleConversion(WallID.JungleUnsafe3, Type, ModContent.WallType<PlaguedVineWall>());
            WallLoader.RegisterSimpleConversion(WallID.JungleUnsafe4, Type, ModContent.WallType<PlaguedVineWall>());
            WallLoader.RegisterSimpleConversion(WallID.FlowerUnsafe, Type, ModContent.WallType<PlaguedVineWall>());

            CommonTileConversion(ModContent.TileType<PlaguedClay>(), TileID.ClayBlock);
            CommonTileConversion(ModContent.TileType<PlaguedMud>(), TileID.Mud);
            CommonTileConversion(ModContent.TileType<PlaguedSilt>(), TileID.Silt);
            CommonTileConversion(ModContent.TileType<PlaguedHive>(), TileID.Hive);
            CommonTileConversion(ModContent.TileType<Sporezol>(), TileID.Copper);

            CommmonWallConversion(ModContent.WallType<PlaguedMudWall>(), WallID.MudUnsafe);
            CommmonWallConversion(ModContent.WallType<PlaguedPipeWall>(), WallID.RichMaogany);
            CommmonWallConversion(ModContent.WallType<PlaguedHiveWall>(), WallID.HiveUnsafe);
        }

        public static void CommonTileConversion(int infectedTileID, ushort cleanTileID)
        {
            TileLoader.RegisterConversion(infectedTileID, BiomeConversionID.Crimson, cleanTileID);
            TileLoader.RegisterConversion(infectedTileID, BiomeConversionID.Hallow, cleanTileID);
            TileLoader.RegisterConversion(infectedTileID, BiomeConversionID.Corruption, cleanTileID);
            TileLoader.RegisterConversion(infectedTileID, BiomeConversionID.Purity, cleanTileID);
        }

        public static void CommmonWallConversion(int infectedWallID, ushort cleanWallID)
        {
            WallLoader.RegisterConversion(infectedWallID, BiomeConversionID.Crimson, cleanWallID);
            WallLoader.RegisterConversion(infectedWallID, BiomeConversionID.Hallow, cleanWallID);
            WallLoader.RegisterConversion(infectedWallID, BiomeConversionID.Corruption, cleanWallID);
            WallLoader.RegisterConversion(infectedWallID, BiomeConversionID.Purity, cleanWallID);
        }
    }
}