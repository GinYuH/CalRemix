using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles
{
	public class TyrantBobber : ModProjectile
    {
        public override string Texture => "CalRemix/Content/Items/Weapons/TyrantShield";
        public override void SetDefaults() 
		{
			Projectile.CloneDefaults(ProjectileID.BobberWooden);
			Projectile.width = 50;
			Projectile.height = 50;
        }
    }
}