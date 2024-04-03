using CalamityMod.Projectiles.BaseProjectiles;

namespace CalRemix.Projectiles.Weapons
{
    public class GrarbleSpearProjectile : BaseSpearProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spear");
        }

        public override void SetDefaults()
        {
            Projectile.width = 50;  //The width of the .png file in pixels divided by 2.
            Projectile.DamageType = Terraria.ModLoader.DamageClass.Melee;
            Projectile.timeLeft = 90;
            Projectile.height = 50;  //The height of the .png file in pixels divided by 2.
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.ownerHitCheck = true;
        }

        public override float InitialSpeed => 3f;
        public override float ReelbackSpeed => 1.1f;
        public override float ForwardSpeed => 0.95f;
    }
}
