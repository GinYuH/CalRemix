using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Graphics.Primitives;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles
{
    public class ZoaoaLight : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/Ranged/OpalStrike";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40;
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 22222;
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 3;
        }

        public override void AI()
        {
            Projectile.ai[2]++;
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
            if ((int)Projectile.ai[0] > 0)
            {
                NPC targ = Main.npc[(int)Projectile.ai[0] - 1];
                if (!targ.active || targ.life <= 0 || targ.friendly)
                {
                    Projectile.Kill();
                    return;
                }
                Projectile.velocity = Projectile.SuperhomeTowardsTarget(targ, MathHelper.Lerp(24, 30, Utils.GetLerpValue(0, 180, Projectile.ai[2], true)), 2f);
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.SourceDamage.Flat = target.lifeMax * 0.05f;
            GeneralParticleHandler.SpawnParticle(new PulseRing(Projectile.Center, Vector2.Zero, Color.Gold, 0.22f, 0.5f, 20));
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawProjectileWithBackglow(Color.Gold, Color.White, 4);
            PrimitiveRenderer.RenderTrail(Projectile.oldPos, new PrimitiveSettings(new PrimitiveSettings.VertexWidthFunction((float f) => (1 - f) * 6), new PrimitiveSettings.VertexColorFunction((float f) => Color.Gold)));
            return false;
        }
    }
}