using CalamityMod.Projectiles.BaseProjectiles;
using Terraria;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class GuardiansWrathStab : BaseSpearProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 37;  //The width of the .png file in pixels divided by 2.
            Projectile.DamageType = Terraria.ModLoader.DamageClass.Melee;
            Projectile.timeLeft = 60;
            Projectile.height = 37;  //The height of the .png file in pixels divided by 2.
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.ownerHitCheck = true;
        }

        public override float InitialSpeed => 1.4f;
        public override float ReelbackSpeed => 0.9f;
        public override float ForwardSpeed => 0.95f;
    }
}
