using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using CalamityMod;
using Terraria.Audio;
using CalamityMod.Buffs.DamageOverTime;
namespace CalRemix.Content.Projectiles.Weapons
{
    public class GodSlayerFireball : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/Boss/DoGFire";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
        }
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 90;
        }

        public override bool? CanHitNPC(NPC target) => Projectile.timeLeft < 60 && target.CanBeChasedBy(Projectile);

        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 4)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= Main.projFrames[Projectile.type])
            {
                Projectile.frame = 0;
            }

            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.ToRadians(90);
            if (Main.rand.NextBool(20))
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, (int)CalamityMod.Dusts.CalamityDusts.BlueCosmilite, 0f, 0f, 100, default, 0.8f);

            if (Projectile.timeLeft < 60)
                CalamityUtils.HomeInOnNPC(Projectile, !Projectile.tileCollide, 1000f, 20f, 20f);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(CalamityMod.Projectiles.Boss.HolyBlast.ImpactSound, Projectile.Center);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 120);
        }
    }
}
