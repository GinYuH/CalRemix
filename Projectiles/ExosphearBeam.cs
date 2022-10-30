using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles
{
	public class ExosphearBeam : ModProjectile
	{
		public override void SetStaticDefaults() 
        {
			DisplayName.SetDefault("Exo Pike Beam");
		}
		public override void SetDefaults() 
        {
            Projectile.width = 40;
			Projectile.height = 40;
            Projectile.aiStyle = 27;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.MeleeNoSpeed;
			Projectile.timeLeft = 120; 
			Projectile.light = 0.5f; 
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
        }
		public override void AI()
		{
            var source = Projectile.GetSource_FromThis();
            NPC target = Projectile.FindTargetWithinRange(360f);
            Projectile.scale = 1;
            Projectile.alpha = 0;

            if (Projectile.ai[1] < 15)
            {
                Projectile.ai[1]++;
            }
            if (!Main.dedServ)
            {
                if (Main.rand.NextBool(10))
                {
                    Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.TerraBlade, 0f, 0f, Alpha: 128, Scale: 1f);
                }
            }
            if (Projectile.ai[1] >= 15)
            {
                if (Projectile.ai[0] == 0)
                {
                    if (target != null)
                    {
                        Projectile.velocity = (target.Center - Projectile.Center).SafeNormalize(Vector2.One) * 12f;
                        Projectile.NewProjectile(source, Projectile.Center, Projectile.velocity.RotatedBy(MathHelper.ToRadians(25f)), ModContent.ProjectileType<ExosphearBeam>(), Projectile.damage / 2, 0, Main.LocalPlayer.whoAmI, 1);
                        Projectile.NewProjectile(source, Projectile.Center, Projectile.velocity.RotatedBy(MathHelper.ToRadians(-25f)), ModContent.ProjectileType<ExosphearBeam>(), Projectile.damage / 2, 0, Main.LocalPlayer.whoAmI, 1);
                        Projectile.ai[0] = 2;
                    }
                }
                else if (Projectile.ai[0] == 1)
                {
                    if (target != null)
                    {
                        Projectile.velocity = (target.Center - Projectile.Center).SafeNormalize(Vector2.One) * 12f;
                        Projectile.NewProjectile(source, Projectile.Center, Projectile.velocity.RotatedBy(MathHelper.ToRadians(25f)), ModContent.ProjectileType<ExosphearBeam>(), Projectile.damage / 2, 0, Main.LocalPlayer.whoAmI, 2);
                        Projectile.NewProjectile(source, Projectile.Center, Projectile.velocity.RotatedBy(MathHelper.ToRadians(-25f)), ModContent.ProjectileType<ExosphearBeam>(), Projectile.damage / 2, 0, Main.LocalPlayer.whoAmI, 2);
                        Projectile.Kill();
                    }
                }
                else if (Projectile.ai[0] == 2)
                {
                    if (Projectile.FindTargetWithinRange(480f) != null)
                    {
                        Projectile.velocity = (target.Center - Projectile.Center).SafeNormalize(Vector2.One) * 12f;
                    }
                }
            }
        }
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
            var source = Projectile.GetSource_FromThis();

            target.AddBuff(ModContent.BuffType<ExoFreeze>(), 30);
            target.AddBuff(ModContent.BuffType <HolyFlames>(), 120);
            target.AddBuff(BuffID.Frostburn, 150);
            target.AddBuff(BuffID.OnFire, 180);
            if (Projectile.ai[1] >= 5)
            {
                if (Projectile.ai[0] == 0)
                {
                    if (target != null)
                    {
                        Projectile.alpha = 0;
                        Projectile.velocity = (target.Center - Projectile.Center).SafeNormalize(Vector2.One) * 12f;
                        Projectile.NewProjectile(source, Projectile.Center, Projectile.velocity.RotatedBy(MathHelper.ToRadians(25f)), ModContent.ProjectileType<ExosphearBeam>(), Projectile.damage / 2, 0, Main.LocalPlayer.whoAmI, 1);
                        Projectile.NewProjectile(source, Projectile.Center, Projectile.velocity.RotatedBy(MathHelper.ToRadians(-25f)), ModContent.ProjectileType<ExosphearBeam>(), Projectile.damage / 2, 0, Main.LocalPlayer.whoAmI, 1);
                        Projectile.ai[0] = 1;
                    }
                }
                else if (Projectile.ai[0] == 1)
                {
                    if (target != null)
                    {
                        Projectile.alpha = 0;
                        Projectile.velocity = (target.Center - Projectile.Center).SafeNormalize(Vector2.One) * 12f;
                        Projectile.NewProjectile(source, Projectile.Center, Projectile.velocity.RotatedBy(MathHelper.ToRadians(25f)), ModContent.ProjectileType<ExosphearBeam>(), Projectile.damage / 2, 0, Main.LocalPlayer.whoAmI, 2);
                        Projectile.NewProjectile(source, Projectile.Center, Projectile.velocity.RotatedBy(MathHelper.ToRadians(-25f)), ModContent.ProjectileType<ExosphearBeam>(), Projectile.damage / 2, 0, Main.LocalPlayer.whoAmI, 2);
                        Projectile.Kill();
                    }
                }
            }
        }
	}
}