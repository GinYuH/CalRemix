using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Bosses.SealedOne
{
    public class HereticSpear : ModProjectile
    {
        public ref float Timer => ref Projectile.ai[0];
        public ref float Flip => ref Projectile.ai[1];
        public ref float VassalDamage => ref Projectile.ai[2];

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.CultistBossLightningOrb);
            Projectile.aiStyle = -1;
            Projectile.width = 25;
            Projectile.height = 25;
        }

        public override void AI()
        {
            int timerStartMovingFast = 10;
            int timerFullyFadeIn = 240;
            int timerEndMovingFast = 100;

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            int maxTransparency = 200;
            Projectile.alpha = (int)(-(Utils.GetLerpValue(0, timerFullyFadeIn, Timer, true) * maxTransparency) + 255);

            if (Projectile.alpha < maxTransparency)
                Projectile.damage = 0;
            else
                Projectile.damage = (int)VassalDamage;

            float velx = CalamityUtils.ExpInEasing(Utils.GetLerpValue(timerStartMovingFast, timerEndMovingFast, Timer, true), 1) * 50;
            Projectile.velocity.X = velx;
            Projectile.velocity.X += 1;
            if (Flip == 1)
                Projectile.velocity.X *= -1;

            if (Timer > 200)
                Projectile.Kill();

            Timer++;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawProjectileWithBackglow(Projectile, Color.OrangeRed, lightColor, 1);
            return false;
        }
    }
}
