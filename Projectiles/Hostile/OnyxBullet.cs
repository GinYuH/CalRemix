using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Hostile
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
        public override void AI()
        {
            foreach (Player p in Main.player)
            {
                if (p.Hitbox.Intersects(Projectile.Hitbox))
                {
                    if (Main.myPlayer == p.whoAmI)
                        SoundEngine.PlaySound(SoundID.NPCDeath14, p.Center);
                    p.AddBuff(BuffID.Slow, 120);
                    Projectile.Kill();
                }
            }
        }
        public override bool CanHitPlayer(Player target)
        {
            return false;
        }
    }
}