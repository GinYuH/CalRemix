using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Projectiles.Environment;
using CalamityMod.Projectiles.Magic;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles
{
    public class BananaBomb : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Banana Bomb");
        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Bomb);
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 300;
        }
        public override void OnKill(int timeLeft)
        {
            CalamityUtils.ExplodeandDestroyTiles(Projectile, 3, true, new List<int>(), new List<int>());
            Projectile.ExplodeTiles(Projectile.Center, 2, 1, 1, 3, 3, false);
            for (int num502 = 0; num502 < 36; num502++)
            {
                int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y + 16f), Projectile.width, Projectile.height - 16, DustID.Confetti_Yellow, 0f, 0f, 0, default, 1f);
                Main.dust[dust].velocity *= 3f;
                Main.dust[dust].scale *= 1.15f;
            }
            int num226 = 36;
            for (int num227 = 0; num227 < num226; num227++)
            {
                Vector2 vector6 = Vector2.Normalize(Projectile.velocity) * new Vector2((float)Projectile.width / 2f, (float)Projectile.height) * 0.75f;
                vector6 = vector6.RotatedBy((double)((float)(num227 - (num226 / 2 - 1)) * MathHelper.TwoPi / (float)num226), default) + Projectile.Center;
                Vector2 vector7 = vector6 - Projectile.Center;
                int num228 = Dust.NewDust(vector6 + vector7, 0, 0, DustID.Confetti_Yellow, vector7.X * 1.5f, vector7.Y * 1.5f, 100, default, 1.4f);
                Main.dust[num228].noGravity = true;
                Main.dust[num228].noLight = true;
                Main.dust[num228].velocity = vector7;
            }
            for (int i = 0; i < Main.rand.Next(2, 6); i++)
                SoundEngine.PlaySound(CalamityMod.Projectiles.Magic.AcidicReed.SaxSound with { MaxInstances = 0 });
        }
    }
}