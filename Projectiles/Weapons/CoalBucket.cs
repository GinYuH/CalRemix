using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Weapons
{
	public class CoalBucket : ModProjectile
	{
        public override string Texture => "CalRemix/Items/Weapons/BucketofCoal";
        public override void SetStaticDefaults() 
        {
			DisplayName.SetDefault("Bucket of Coal");
		}
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.rotation = Main.rand.Next(360);
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
        }
        public override void AI()
        {
            Projectile.velocity.Y += 0.2f;
            Projectile.spriteDirection = Projectile.direction;
            Projectile.rotation += 0.2f * Projectile.direction;
            if (Projectile.timeLeft % 20 == 0)
            {
                Vector2 vector = Vector2.One.RotatedBy(Projectile.rotation);
                for (int i = 0; i < 3; i++)
                    Dust.NewDust(Projectile.Center, 1, 1, DustID.Obsidian, vector.RotatedByRandom(2f).X * Main.rand.NextFloat(1f,1.5f), vector.RotatedByRandom(MathHelper.ToRadians(45f)).Y * Main.rand.NextFloat(1f, 1.5f));
                Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, vector.RotatedByRandom(MathHelper.ToRadians(45f)) * 6f, ModContent.ProjectileType<Coal>(), Projectile.damage, Projectile.knockBack / 2, Projectile.owner);
                proj.Calamity().stealthStrike = Projectile.Calamity().stealthStrike;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.Kill();
        }
        public override void OnKill(int timeLeft)
        {
            Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ProjectileID.BlackBolt, Projectile.damage, Projectile.knockBack / 2, Projectile.owner);
            proj.DamageType = ModContent.GetInstance<RogueDamageClass>();
            proj.Kill();
        }
    }
}