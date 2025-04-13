using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod;
using Terraria.Audio;
using System;

namespace CalRemix.Content.NPCs.Bosses.BossChanges.SupremeCalamitas
{
    public class SupremeBone : ModProjectile
    {
        public Player Target => Main.player[(int)Projectile.ai[0]];

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 36;
            Projectile.hostile = true;
            Projectile.aiStyle = -1;
        }
        public override void AI()
        {
            float rotationValue = 0.15f + Math.Abs(Projectile.velocity.Y / 15);
            if (Projectile.velocity.X < 0)
                rotationValue *= -1;
            Projectile.rotation += rotationValue;
            Projectile.velocity.Y += 0.25f;
            Projectile.velocity.ClampMagnitude(-10, 10);

            if (Projectile.Center.Y < Target.Center.Y + 60)
            {
                Projectile.tileCollide = false;
            }
            else
            {
                Projectile.tileCollide = true;
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            //target.AddBuff(ModContent.BuffType<VulnerabilityHex>(), 120);
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            for (int i = 0; i < 10; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Crimslime, 0f, 0f);
                d.velocity = new Vector2(Main.rand.Next(-3, 4), Main.rand.Next(-3, 4));
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return true;
        }
    }
}
