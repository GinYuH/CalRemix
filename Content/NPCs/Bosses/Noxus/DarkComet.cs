using CalamityMod;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using CalRemix.Core.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static System.MathF;
using static Terraria.Utils;
using static Microsoft.Xna.Framework.MathHelper;

namespace CalRemix.Content.NPCs.Bosses.Noxus
{
    public class DarkComet : ModProjectile
    {
        public ref float Time => ref Projectile.ai[1];

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 38;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 120;
            CooldownSlot = ImmunityCooldownID.Bosses;
        }

        public override void AI()
        {
            // Spawn particles.
            for (int i = 0; i < 2; i++)
            {
                Color voidColor = Color.Lerp(Color.Purple, Color.Black, Main.rand.NextFloat(0.54f, 0.9f));
                voidColor = Color.Lerp(voidColor, Color.DarkBlue, Main.rand.NextFloat(0.4f));
                HeavySmokeParticle darkGas = new(Projectile.Center + Main.rand.NextVector2Circular(4f, 4f), Main.rand.NextVector2Circular(1f, 1f), voidColor, 11, Projectile.scale * 1.24f, Projectile.Opacity * 0.6f, Main.rand.NextFloat(0.02f), true);
                GeneralParticleHandler.SpawnParticle(darkGas);
            }

            // Add a mild amount of slithering movement.
            float slitherOffset = Sin(Time / 6.4f + Projectile.identity) * GetLerpValue(10f, 25f, Time, true) * 6.2f;
            Vector2 perpendicularDirection = Projectile.velocity.SafeNormalize(Vector2.Zero).RotatedBy(PiOver2);
            Projectile.Center += perpendicularDirection * slitherOffset;

            // Accelerate over time.
            if (Projectile.velocity.Length() < 26.25f)
                Projectile.velocity *= 1.0265f;

            // Decide the current rotation.
            Projectile.rotation = (Projectile.position - Projectile.oldPosition).ToRotation();
            Projectile.spriteDirection = (Cos(Projectile.rotation) > 0f).ToDirectionInt();
            if (Projectile.spriteDirection == -1)
                Projectile.rotation += Pi;

            // Create gas particles.
            if (Main.rand.NextBool(3))
            {
                float gasSize = GetLerpValue(-3f, 25f, Time, true) * Projectile.width * 0.68f;
                float angularOffset = Sin(Time / 5f) * 0.77f;
                NoxusGasMetaball.CreateParticle(Projectile.Center + Projectile.velocity * 2f, Main.rand.NextVector2Circular(2f, 2f) + Projectile.velocity.RotatedBy(angularOffset).RotatedByRandom(0.6f) * 0.26f, gasSize);
            }

            Time++;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<NoxusFumes>(), EntropicGod.DebuffDuration_RegularAttack);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Color drawColor = Color.White;
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], drawColor);
            return false;
        }
    }
}
