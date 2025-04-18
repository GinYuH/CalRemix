using CalamityMod;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
	public class ShadowCloud : ModProjectile
	{
        public override void SetStaticDefaults() 
        {
			// DisplayName.SetDefault("Demon Core");
		}
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.penetrate = -1;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
        }
        public override void AI()
        {
            Projectile.ai[1] += 1f;
            if (Projectile.ai[1] > 30f)
                Projectile.ai[0] += 10f;

            if (Projectile.ai[0] > 255f)
            {
                Projectile.Kill();
                Projectile.ai[0] = 255f;
            }
            Projectile.alpha = (int)(100.0 + (double)Projectile.ai[0]);
            Projectile.rotation += Projectile.velocity.X * 0.1f;
            Projectile.rotation += (float)Projectile.direction * 0.003f;
            Projectile.velocity *= 0.96f;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.ShadowFlame, 120);
        }
    }
}