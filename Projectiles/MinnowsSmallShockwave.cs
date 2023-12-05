using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bloopslite.Items;



public class MinnowsSmallShockwave: ModProjectile
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
        Projectile.width = 100;
        Projectile.hostile = true;
        Projectile.height = 100;
        Projectile.timeLeft = 10;
        Projectile.penetrate = -1;
        Projectile.tileCollide = false;
        Projectile.alpha = 100;
      
    }
  
    public override void AI()
    {
        if (Projectile.timeLeft == 9)
        {
            Particle wave = new PulseRing(Projectile.Center, Projectile.velocity *= 0, Color.Cyan, 0.2f, 1f, 9);
            GeneralParticleHandler.SpawnParticle(wave);
        }
        if (Projectile.timeLeft < 10)
        {
            Projectile.scale += 0.5f;
        }
        


    }
}

