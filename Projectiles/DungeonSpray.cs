using CalamityMod.Tiles.FurnitureStratus;
using CalRemix.Walls;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles
{
    public abstract class DungeonSpray : ModProjectile
	{
        internal static readonly int[] StratusTiles = 
        {
            ModContent.TileType<StratusBricks>(),
            ModContent.TileType<StratusBathtub>(),
            ModContent.TileType<StratusBed>(),
            ModContent.TileType<StratusBookcase>(),
            ModContent.TileType<StratusCandelabra>(),
            ModContent.TileType<StratusCandle>(),
            ModContent.TileType<StratusChair>(),
            ModContent.TileType<StratusChandelier>(),
            ModContent.TileType<StratusClock>(),
            ModContent.TileType<StratusDoorClosed>(),
            ModContent.TileType<StratusDoorOpen>(),
            ModContent.TileType<StratusDresser>(),
            ModContent.TileType<StratusLamp>(),
            ModContent.TileType<StratusLantern>(),
            ModContent.TileType<StratusPiano>(),
            ModContent.TileType<StratusPlatform>(),
            ModContent.TileType<StratusSofa>(),
            ModContent.TileType<StratusTable>()
        };
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
        public static void Convert(ushort dungeonBrick, ushort dungeonWall, int i, int j, int size = 4)
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
                        if (type == ModContent.TileType<StratusBricks>())
                        {
                            Main.tile[k, l].TileType = dungeonBrick;
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        }
                        if (wall == ModContent.WallType<StratusWallRemix>())
                        {
                            Main.tile[k, l].WallType = dungeonWall;
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        }
                    }
                }
            }
        }
    }
    public class PinkGreySpray : DungeonSpray
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetDefaults()
        {
			base.SetDefaults();
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, new Vector3(100 / 255f, 10 / 255f, 20 / 255f));
            if (Projectile.owner == Main.myPlayer)
                DungeonSpray.Convert((ushort)TileID.PinkDungeonBrick, (ushort)WallID.PinkDungeonUnsafe, (int)(Projectile.position.X + Projectile.width / 2) / 16, (int)(Projectile.position.Y + Projectile.height / 2) / 16, 2);
            if (Projectile.timeLeft > 133)
                Projectile.timeLeft = 133;

            if (Projectile.ai[0] > 7f)
            {
                Projectile.ai[0] += 1f;

                for (int i = 0; i < 1; i++)
                {
                    int dustType = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.DungeonPink, 0f, 0f, 100, Color.White, 2f);
                    Dust obj = Main.dust[dustType];
                    obj.velocity *= 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[dustType].scale = 0.5f;
                        Main.dust[dustType].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                    }
                    Main.dust[dustType].noGravity = true;
                }
            }
            else
                Projectile.ai[0] += 1f;

            Projectile.rotation += 0.3f * Projectile.direction;
        }
    }
    public class GreenGreySpray : DungeonSpray
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetDefaults()
        {
            base.SetDefaults();
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, new Vector3(10 / 255f, 100 / 255f, 10 / 255f));
            if (Projectile.owner == Main.myPlayer)
                DungeonSpray.Convert((ushort)TileID.GreenDungeonBrick, (ushort)WallID.GreenDungeonUnsafe, (int)(Projectile.position.X + Projectile.width / 2) / 16, (int)(Projectile.position.Y + Projectile.height / 2) / 16, 2);
            if (Projectile.timeLeft > 133)
                Projectile.timeLeft = 133;

            if (Projectile.ai[0] > 7f)
            {
                Projectile.ai[0] += 1f;

                for (int i = 0; i < 1; i++)
                {
                    int dustType = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.DungeonGreen, 0f, 0f, 100, Color.White, 2f);
                    Dust obj = Main.dust[dustType];
                    obj.velocity *= 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[dustType].scale = 0.5f;
                        Main.dust[dustType].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                    }
                    Main.dust[dustType].noGravity = true;
                }
            }
            else
                Projectile.ai[0] += 1f;

            Projectile.rotation += 0.3f * Projectile.direction;
        }
    }
    public class BlueGreySpray : DungeonSpray
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetDefaults()
        {
            base.SetDefaults();
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, new Vector3(10 /  255f, 10 / 255f, 100 / 255f));
            if (Projectile.owner == Main.myPlayer)
                DungeonSpray.Convert((ushort)TileID.BlueDungeonBrick, (ushort)WallID.BlueDungeonUnsafe, (int)(Projectile.position.X + Projectile.width / 2) / 16, (int)(Projectile.position.Y + Projectile.height / 2) / 16, 2);
            if (Projectile.timeLeft > 133)
                Projectile.timeLeft = 133;

            if (Projectile.ai[0] > 7f)
            {
                Projectile.ai[0] += 1f;

                for (int i = 0; i < 1; i++)
                {
                    int dustType = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.DungeonBlue, 0f, 0f, 100, Color.White, 2f);
                    Dust obj = Main.dust[dustType];
                    obj.velocity *= 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[dustType].scale = 0.5f;
                        Main.dust[dustType].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                    }
                    Main.dust[dustType].noGravity = true;
                }
            }
            else
                Projectile.ai[0] += 1f;

            Projectile.rotation += 0.3f * Projectile.direction;
        }
    }
}