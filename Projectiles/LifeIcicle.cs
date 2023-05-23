using CalamityMod.Projectiles.Environment;
using CalamityMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles
{
	public class LifeIcicle : ModProjectile
	{
        public override string Texture => "CalamityMod/Projectiles/Magic/IcicleStaffProj";
        public override void SetStaticDefaults() 
        {
			DisplayName.SetDefault("Cryonic Icicle");
		}
		public override void SetDefaults() 
        {
            Projectile.CloneDefaults(ModContent.ProjectileType<TridentIcicle>());
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.DamageType = DamageClass.Default;
            AIType = ModContent.ProjectileType<TridentIcicle>();
        }
		public override void AI()
		{
            if (!Main.dedServ)
            {
                if (Main.rand.NextBool(10))
                {
                    Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.IceTorch, 0f, 0f, Alpha: 128, Scale: 1f);
                }
            }
        }
	}
}