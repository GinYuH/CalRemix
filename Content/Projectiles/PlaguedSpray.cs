using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles
{
    public class PlaguedSpray : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 2;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if (Projectile.owner == Main.myPlayer)
                Convert((int)(Projectile.position.X + Projectile.width / 2) / 16, (int)(Projectile.position.Y + Projectile.height / 2) / 16, 2);

            if (Projectile.timeLeft > 133)
                Projectile.timeLeft = 133;

            if (Projectile.ai[0] > 7f)
            {
                Projectile.ai[0] += 1f;

                for (int i = 0; i < 1; i++)
                {
                    int dustType = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.GemEmerald, 0f, 0f, 100, default, 2f);
                    Dust obj = Main.dust[dustType];
                    obj.velocity *= 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[dustType].scale = 0.5f;
                        Main.dust[dustType].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                    }
                    Main.dust[dustType].noGravity = true;
                }
            }
            else
                Projectile.ai[0] += 1f;

            Projectile.rotation += 0.3f * Projectile.direction;
        }

        public static void Convert(int i, int j, int size = 4)
        {
            for (int k = i - size; k <= i + size; k++)
            {
                for (int l = j - size; l <= j + size; l++)
                {
                    if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < Math.Sqrt(size * size + size * size))
                    {
                        int type = Main.tile[k, l].TileType;
                        int wall = Main.tile[k, l].WallType;
                        int dist = (k - i) * (k - i) + (l - j) * (l - j);
                        if (dist > size * size)
                            continue;
                        if (Main.tile[k, l] == null)
                            continue;
                        if (type == TileID.Tin || type == TileID.Copper || type == TileID.Silver || type == TileID.Tungsten || type == TileID.Gold || type == TileID.Platinum || type == TileID.Iron || type == TileID.Lead || type == TileID.Demonite || type == TileID.Crimtane)
                        {
                            Main.tile[k, l].TileType = (ushort)ModContent.TileType<Tiles.PlaguedJungle.Sporezol>();
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        }
                        if (TileID.Sets.GrassSpecial[type] || type == TileID.Grass || type == TileID.CorruptGrass || type == TileID.CrimsonGrass)
                        {
                            Main.tile[k, l].TileType = (ushort)ModContent.TileType<Tiles.PlaguedJungle.PlaguedGrass>();
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        }
                        if (TileID.Sets.Mud[type])
                        {
                            Main.tile[k, l].TileType = (ushort)ModContent.TileType<Tiles.PlaguedJungle.PlaguedMud>();
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        }
                        if (TileID.Sets.Conversion.Stone[type])
                        {
                            Main.tile[k, l].TileType = (ushort)ModContent.TileType<Tiles.PlaguedJungle.PlaguedStone>();
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        }
                        if (wall == WallID.MudUnsafe || wall == WallID.DirtUnsafe || wall == WallID.DirtUnsafe2 || wall == WallID.DirtUnsafe3 || wall == WallID.DirtUnsafe4 || wall == WallID.Cave6Unsafe || wall == WallID.Cave5Unsafe || wall == WallID.Cave4Unsafe || wall == WallID.Cave3Unsafe || wall == WallID.Cave2Unsafe || wall == WallID.CaveUnsafe)
                        {
                            Main.tile[k, l].WallType = (ushort)ModContent.WallType<Tiles.PlaguedJungle.PlaguedMudWall>();
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        }
                        if (type == TileID.Hive)
                        {
                            Main.tile[k, l].TileType = (ushort)ModContent.TileType<Tiles.PlaguedJungle.PlaguedHive>();
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        }
                        if (type == TileID.Dirt)
                        {
                            Main.tile[k, l].TileType = (ushort)ModContent.TileType<Tiles.PlaguedJungle.PlaguedMud>();
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        }
                        if (type == TileID.RichMahogany)
                        {
                            Main.tile[k, l].TileType = (ushort)ModContent.TileType<Tiles.PlaguedJungle.PlaguedPipe>();
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        }
                        if (wall == WallID.RichMaogany)
                        {
                            Main.tile[k, l].WallType = (ushort)ModContent.WallType<Tiles.PlaguedJungle.PlaguedPipeWall>();
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        }
                        if (type == TileID.LivingMahogany || type == TileID.LivingWood)
                        {
                            Main.tile[k, l].TileType = (ushort)ModContent.TileType<Tiles.PlaguedJungle.UndeadPlaguePipe>();
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        }
                        if (wall == WallID.HiveUnsafe || wall == WallID.Hive || wall == WallID.GraniteUnsafe || wall == WallID.MarbleUnsafe || wall == WallID.Granite || wall == WallID.Marble)
                        {
                            Main.tile[k, l].WallType = (ushort)ModContent.WallType<Tiles.PlaguedJungle.PlaguedHiveWall>();
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        }
                        if (wall == WallID.Stone || wall == WallID.EbonstoneUnsafe || wall == WallID.CrimstoneUnsafe)
                        {
                            Main.tile[k, l].WallType = (ushort)ModContent.WallType<Tiles.PlaguedJungle.PlaguedStoneWall>();
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        }
                        if (wall == WallID.Grass || wall == WallID.GrassUnsafe || wall == WallID.CorruptGrassUnsafe || wall == WallID.CrimsonGrassUnsafe || wall == WallID.HallowedGrassUnsafe || wall == WallID.Jungle || wall == WallID.JungleUnsafe || wall == WallID.JungleUnsafe1 || wall == WallID.JungleUnsafe2 || wall == WallID.JungleUnsafe3 || wall == WallID.JungleUnsafe4 || wall == WallID.Flower || wall == WallID.FlowerUnsafe)
                        {
                            Main.tile[k, l].WallType = (ushort)ModContent.WallType<Tiles.PlaguedJungle.PlaguedVineWall>();
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        }
                        if (type == TileID.ClayBlock)
                        {
                            Main.tile[k, l].TileType = (ushort)ModContent.TileType<Tiles.PlaguedJungle.PlaguedClay>();
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        }
                        if (type == TileID.Silt)
                        {
                            Main.tile[k, l].TileType = (ushort)ModContent.TileType<Tiles.PlaguedJungle.PlaguedSilt>();
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        }
                        if (type == TileID.Sand)
                        {
                            Main.tile[k, l].TileType = (ushort)ModContent.TileType<Tiles.PlaguedJungle.PlaguedSand>();
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        }
                    }
                }
            }
        }
    }
}