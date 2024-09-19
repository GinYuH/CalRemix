using CalamityMod.Items.Materials;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile;



public class MinnowsDeathAnimation : ModProjectile
{



    int repeat = 0;
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
        Projectile.width = 100;
        Projectile.height = 100;
        Projectile.timeLeft = 500;
        Projectile.penetrate = -1;
        Projectile.tileCollide = false;
        Projectile.alpha = 100;
    }
    public override bool PreKill(int timeLeft)
    {
        Item.NewItem(source: null, Projectile.Center, ModContent.ItemType<Necroplasm>(), Main.rand.Next(20, 30));
        return true;

    }
    public override void AI()
    {
        repeat++;
        Particle DeathBloomlong = new StrongBloom(Projectile.Center, new Vector2(0, 0), Color.Cyan, 2f, 600);
        if (Projectile.timeLeft == 499)
        {
            GeneralParticleHandler.SpawnParticle(DeathBloomlong);
        }
       
        if (repeat == 1)
        {
            Particle DeathBloom = new StrongBloom(Projectile.Center, new Vector2(0, 0), Color.Cyan, 2f, 10);
            GeneralParticleHandler.SpawnParticle(DeathBloom);
        }
        if (repeat == 5)
        {
            repeat = 0;
        }



    }
}

