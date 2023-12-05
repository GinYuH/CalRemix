using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bloopslite.Items;



public class MinnowsLargeShockwave : ModProjectile
{




    public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("Force");
    }

    public override void SetDefaults()
    {

        Projectile.aiStyle = -1; 
        Projectile.friendly = false;
        Projectile.hostile = true;
        Projectile.width = 500;
        Projectile.height = 500;
        Projectile.timeLeft = 60;
        Projectile.penetrate = -1;
        Projectile.tileCollide = false;
        Projectile.alpha = 100;
    }
  
    public override void AI()
    {
        if (Projectile.timeLeft == 59)
        {
            Particle wave = new PulseRing(Projectile.Center, Projectile.velocity *= 0, Color.Cyan, 0.2f, 6f, 60);
            GeneralParticleHandler.SpawnParticle(wave);
        }
        if (Projectile.timeLeft < 60)
        {
            Projectile.scale += 0.1f;
        }
        


    }
}

