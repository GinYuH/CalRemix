using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles
{
	public class OnyxBullet : ModProjectile
	{
        public override string Texture => "Terraria/Images/Projectile_" + 661;
        public override void SetDefaults() 
        {
            AIType = 661;
            Projectile.CloneDefaults(661);
            Projectile.timeLeft = 300;
            Projectile.friendly = false;
            Projectile.hostile = true;
		}
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.Slow, 180);
        }
        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            modifiers.SourceDamage *= 0f;
        }
    }
}