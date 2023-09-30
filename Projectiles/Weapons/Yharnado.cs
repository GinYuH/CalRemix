using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using CalamityMod;
using Terraria.Audio;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Projectiles.Magic;
using Terraria.ID;

namespace CalRemix.Projectiles.Weapons
{
    public class Yharnado : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/Boss/Flare";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.alpha = 255;
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 600;
            Projectile.extraUpdates = 1;
            Projectile.tileCollide = true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Dragonfire>(), 240);
            Projectile.Kill();
        }

        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 4)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 3)
            {
                Projectile.frame = 0;
            }

            CalamityUtils.HomeInOnNPC(Projectile, !Projectile.tileCollide, 3000f, 10f, 20f);

            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 10f)
            {
                Projectile.alpha -= 5;
                if (Projectile.alpha < 100)
                {
                    Projectile.alpha = 100;
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(CalamityMod.Projectiles.Boss.Flare.FlareSound, Projectile.Center);
            if (Projectile.owner == Main.myPlayer)
            {
                int num236 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<InfernadoFriendly>(), (int)(Projectile.damage * 1.33333f), Projectile.knockBack * 30f, Main.myPlayer, 16f, 16f);
                Main.projectile[num236].netUpdate = true;
}
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, Main.DiscoG, 53, Projectile.alpha);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}
