using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using CalamityMod;
using Terraria.Audio;
using Terraria.ID;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
namespace CalRemix.Projectiles.Weapons
{
    public class RockOrb : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 180;
            Projectile.tileCollide = false;
        }

        public override bool? CanHitNPC(NPC target) => Projectile.timeLeft < 60 && target.CanBeChasedBy(Projectile);

        public override void AI()
        {
            if (Main.rand.NextBool(22))
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, (int)CalamityMod.Dusts.CalamityDusts.Brimstone, 0f, 0f, 100, default, 0.75f);

            if (Projectile.timeLeft < 120)
                CalamityUtils.HomeInOnNPC(Projectile, !Projectile.tileCollide, 4800f, 30f, 20f);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<WhisperingDeath>(), 120);
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 120);
            target.AddBuff(ModContent.BuffType<VulnerabilityHex>(), 120);
        }
    }
}
