using CalamityMod.Projectiles.Environment;
using CalamityMod.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles
{
	public class LifeMeteor : ModProjectile
	{
        public override string Texture => "CalamityMod/Projectiles/Magic/AsteroidMolten";
        public override void SetStaticDefaults() 
        {
			DisplayName.SetDefault("Hydrothermic Meteor");
		}
		public override void SetDefaults() 
        {
            Projectile.CloneDefaults(ModContent.ProjectileType<AsteroidMolten>());
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Default;
            Projectile.tileCollide = true;
            AIType = ModContent.ProjectileType<AsteroidMolten>();
        }
		public override void AI()
		{
            if (!Main.dedServ)
            {
                if (Main.rand.NextBool(6))
                {
                    Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, Alpha: 128, Scale: 1f);
                }
            }
        }
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
            Projectile.Kill();
		}
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity != Projectile.oldVelocity)
            {
                for (int i = 0; i < 10; i++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Normalize(-oldVelocity).RotatedByRandom(MathHelper.ToRadians(180f)) * 6f, ModContent.ProjectileType<LavaChunk>(), Projectile.damage, 0);
                }
            }
            return true;
        }
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            if (Projectile.velocity == Projectile.oldVelocity)
            {
                for (int i = 0; i < 10; i++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Normalize(Vector2.One).RotatedByRandom(MathHelper.ToRadians(360f)) * 6f, ModContent.ProjectileType<LavaChunk>(), Projectile.damage, 0);
                }
            }
        }
    }
}