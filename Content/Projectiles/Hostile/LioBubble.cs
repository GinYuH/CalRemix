using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class LioBubble : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/Magic/BlackAnurianBubble";
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.hostile = true;
            Projectile.timeLeft = 180;
            Projectile.alpha = 220;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Player p = Main.player[(int)Projectile.ai[0]];
            if (Projectile.velocity.Y < 0 && Projectile.position.Y < (p.Top.Y - 100))
            {
                Projectile.tileCollide = true;
            }
            Projectile.velocity.Y -= 0.1f;
            Projectile.velocity.X *= 0.997f;
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.scale += 0.02f;
                if (Projectile.scale >= 1.2f)
                    Projectile.localAI[0] = 1f;
            }
            else if (Projectile.localAI[0] == 1f)
            {
                Projectile.scale -= 0.02f;
                if (Projectile.scale <= 0.8f)
                    Projectile.localAI[0] = 0f;
            }
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(BetterSoundID.ItemBubblePop, Projectile.Center);
            GeneralParticleHandler.SpawnParticle(new PulseRing(Projectile.Center, Vector2.Zero, Color.Turquoise, 0.22f, 0.5f, 20));
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<CrushDepth>(), 120);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawProjectileWithBackglow(Projectile, Color.Cyan, lightColor, 4);
            return false;
        }
    }
}
