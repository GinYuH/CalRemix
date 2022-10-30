using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles
{
	public class ExosphearSpear : ModProjectile
	{
        protected virtual float HoldoutRangeMin => 120f;
        protected virtual float HoldoutRangeMax => 260f;

        public override void SetStaticDefaults() 
        {
			DisplayName.SetDefault("Gravitonomy Pike");
		}
		public override void SetDefaults() 
        {
            Projectile.CloneDefaults(ProjectileID.Spear);
            Projectile.width = 80;
			Projectile.height = 80;
			Projectile.light = 0.5f; 
		}
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            int duration = player.itemAnimationMax;

            player.heldProj = Projectile.whoAmI;
            if (Projectile.timeLeft > duration)
            {
                Projectile.timeLeft = duration;
            }

            Projectile.velocity = Vector2.Normalize(Projectile.velocity);

            float halfDuration = duration * 0.5f;
            float progress;
            if (Projectile.timeLeft < halfDuration)
            {
                progress = Projectile.timeLeft / halfDuration;
            }
            else
            {
                progress = (duration - Projectile.timeLeft) / halfDuration;
            }
            Projectile.Center = player.MountedCenter + Vector2.SmoothStep(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax, progress);
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.ToRadians(45f);
            }
            else
            {
                Projectile.rotation += MathHelper.ToRadians(135f);
            }

            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModContent.BuffType<ExoFreeze>(), 30);
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 120);
            target.AddBuff(BuffID.Frostburn, 150);
            target.AddBuff(BuffID.OnFire, 180);
            target.immune[Projectile.owner] = 0;
            for (int k = 0; k < Main.maxNPCs; k++)
            {
                NPC npc = Main.npc[k];
                if (target.CanBeChasedBy())
                {
                    float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, npc.Center);
                    if (sqrDistanceToTarget < 360)
                    {
                        Vector2 pullVelocity = Vector2.Normalize(Projectile.Center - npc.Center);
                        npc.velocity += pullVelocity;
                        if (!Main.dedServ)
                        {
                            Dust.NewDust(npc.Center, 0, 0, DustID.TerraBlade, pullVelocity.X / 2f, pullVelocity.Y / 2f, default, Color.White, 1.5f);
                        }
                    }
                }
            }
        }
    }
}