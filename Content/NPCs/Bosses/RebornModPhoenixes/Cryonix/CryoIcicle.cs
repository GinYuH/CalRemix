using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Bosses.RebornModPhoenixes.Cryonix
{
    public class CryoIcicle : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.aiStyle = 0;
            Projectile.hostile = true;
            Projectile.height = 64;
            Projectile.penetrate = 50;
            Projectile.tileCollide = false;
            Projectile.width = 64;
            Projectile.timeLeft = 480;
            //killPretendType=15   uhhhmmmm documentation says this controls what happens on death but im too lazy to copy it
        }

        public override void AI()
        {
            Projectile.velocity = Projectile.velocity.ClampMagnitude(-20, 20);
            Projectile.rotation += Projectile.velocity.X * 0.05f;
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.Pi / 2;
        }

        public override void OnKill(int timeLeft)
        {

            Projectile.active = false;
        }
    }
}
