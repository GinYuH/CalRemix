using CalRemix.Content.Tiles.PlaguedJungle;
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
                        int dist = (k - i) * (k - i) + (l - j) * (l - j);
                        if (dist > size * size)
                            continue;
                        if (Main.tile[k, l] == null)
                            continue;

                        WorldGen.Convert(k, l, ModContent.GetInstance<PlagueConversion>().Type, size);
                    }
                }
            }
        }
    }
}