using CalamityMod;
using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles
{
	public class LumenousMine : ModProjectile
	{
        public override void SetStaticDefaults() 
        {
			DisplayName.SetDefault("Lumenous Mine");
		}
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 9999;
            Projectile.timeLeft = 1200;
            Projectile.usesLocalNPCImmunity = true;
        }
        public override void AI()
        {
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] >= 70f && Projectile.velocity != Vector2.Zero)
            {
                Projectile.velocity.X *= 0.96f;
                Projectile.velocity.Y *= 0.96f;
            }
            if (Projectile.ai[0] == 300f && !Projectile.Calamity().stealthStrike)
            {
                Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ShockTeslaAura>(), Projectile.damage / 4, Projectile.knockBack, Projectile.owner);
                proj.timeLeft = 15*60;
                SoundEngine.PlaySound(SoundID.Item94);
            }
        }
        public override void Kill(int timeLeft)
        {
            if (Projectile.Calamity().stealthStrike)
            {
                for (int i = 0; i < Main.rand.Next(20, 31); i++)
                {
                    Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.One.RotatedByRandom(MathHelper.ToRadians(360)), ModContent.ProjectileType<LumChunk>(), Projectile.damage / 4, Projectile.knockBack, Projectile.owner);
                }
            }
            else
            {
                for (int i = 0; i < Main.rand.Next(8, 11); i++)
                {
                    Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.One.RotatedByRandom(MathHelper.ToRadians(360)), ModContent.ProjectileType<LumChunk>(), Projectile.damage / 4, Projectile.knockBack, Projectile.owner);
                }
            }
        }
    }
}