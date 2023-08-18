using CalamityMod;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Weapons.Rogue;
namespace CalRemix.Projectiles.Weapons;


public class ExodimusPROJ : ModProjectile
{




    public bool stick;
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Exodiumus dagger");
    }

    public override void SetDefaults()
    {

        Projectile.width = 182;
        Projectile.height = 182;
        Projectile.friendly = true;
        Projectile.penetrate = 5;
        Projectile.tileCollide = false;
        Projectile.timeLeft = 660;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 10;
        Projectile.aiStyle = 0;
        Projectile.DamageType = RogueDamageClass.Default;
    }

  

    public override bool PreKill(int timeLeft)
    {
        SoundEngine.PlaySound(SoundID.Item96, Projectile.Center);
        for (int num163 = 0; num163 < 20; num163++)
        {
            float x2 = Projectile.Center.X - Projectile.velocity.X / 10f * (float)num163;
            float y2 = Projectile.Center.Y - Projectile.velocity.Y / 10f * (float)num163;
            int num165 = Dust.NewDust(new Vector2(x2, y2), 10, 10, 43, 0f, 0f, 255, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1f);


            Main.dust[num165].position.X = x2;
            Main.dust[num165].position.Y = y2;
            Main.dust[num165].velocity *= Main.rand.Next(0, 15);
            Main.dust[num165].noGravity = true;
            Main.dust[num165].fadeIn *= 1f;
            Main.dust[num165].scale = Main.rand.Next(2, 4);


            
        }
        return false;
    }
    // Are we sticking to a target?


    // Index of the current target
    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers fuckyou)
    {
        SparkParticle stab = new SparkParticle(Projectile.Center, Projectile.velocity, false, 5, 5f, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB));
        GeneralParticleHandler.SpawnParticle(stab);
        Player player = Main.player[Projectile.owner];

        SoundEngine.PlaySound(SoundID.Item109, Projectile.Center);
        Projectile.penetrate--;

        if (Projectile.penetrate <= 1)
        {
            Vector2 launchVelocity = new Vector2(-1, 1); // Create a velocity moving the left.
          
            if (stick)
            {
                for (int i = 0; i < 8; i++)
                {
                    // Every iteration, rotate the newly spawned projectile by the equivalent 1/4th of a circle (MathHelper.PiOver4)
                    // (Remember that all rotation in Terraria is based on Radians, NOT Degrees!)
                   
                    launchVelocity = launchVelocity.RotatedBy(MathHelper.PiOver4);

                    // Spawn a new projectile with the newly rotated velocity, belonging to the original projectile owner. The new projectile will inherit the spawning source of this projectile.
                    Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, launchVelocity, ModContent.ProjectileType<dimusBEAM>(), (int)(Projectile.damage * 0.50f), Projectile.knockBack, Projectile.owner);
                }
            }

            {
                for (int i = 0; i < 4; i++)
                {
                    // Every iteration, rotate the newly spawned projectile by the equivalent 1/4th of a circle (MathHelper.PiOver4)
                    // (Remember that all rotation in Terraria is based on Radians, NOT Degrees!)
                
                    launchVelocity = launchVelocity.RotatedBy(MathHelper.PiOver2);

                    // Spawn a new projectile with the newly rotated velocity, belonging to the original projectile owner. The new projectile will inherit the spawning source of this projectile.
                  int a =  Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, launchVelocity, ModContent.ProjectileType<dimusBEAM>(), (int)(Projectile.damage * 2f), Projectile.knockBack, Projectile.owner);
                    Main.projectile[a].localNPCHitCooldown = 10;
                }
            }

        }
        Mod calamityMod = ModLoader.GetMod("CalamityMod");
        target.AddBuff(calamityMod.Find<ModBuff>("HolyFlames").Type, 150);
        target.AddBuff(BuffID.Frostburn, 150);
        target.AddBuff(BuffID.OnFire, 150);
        for (int num163 = 0; num163 < 10; num163++)
        {
            float x2 = Projectile.Center.X - Projectile.velocity.X / 10f * (float)num163;
            float y2 = Projectile.Center.Y - Projectile.velocity.Y / 10f * (float)num163;
            int num165 = Dust.NewDust(new Vector2(x2, y2), 10, 10, 43, 0f, 0f, 255, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 5f);


            Main.dust[num165].position.X = x2;
            Main.dust[num165].position.Y = y2;
            Main.dust[num165].velocity *= Main.rand.NextFloat(0, 8);
            Main.dust[num165].noGravity = true;
            Main.dust[num165].fadeIn *= 0f;
            Main.dust[num165].scale = Main.rand.NextFloat(1f, 1.50f);



        }
      

    }
   
    public override void AI()
    {
        Player player = Main.player[Projectile.owner];

        if (player.Calamity().StealthStrikeAvailable())
        {
            stick = true;
        }
            Lighting.AddLight(Projectile.position, 10, 10, 10);
        Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        for (int num163 = 0; num163 < 1; num163++)
        {
            float x2 = Projectile.Center.X - Projectile.velocity.X / 10f * (float)num163;
            float y2 = Projectile.Center.Y - Projectile.velocity.Y / 10f * (float)num163;
            int num165 = Dust.NewDust(new Vector2(x2, y2), 10, 10, 43, 0f, 0f, 255, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 5f);


            Main.dust[num165].position.X = x2;
            Main.dust[num165].position.Y = y2;
            Main.dust[num165].velocity *= Main.rand.NextFloat(0, 8);
            Main.dust[num165].noGravity = true;
            Main.dust[num165].fadeIn *= 0f;
            Main.dust[num165].scale = Main.rand.NextFloat(1f, 1.50f);



        }
     

        if (Projectile.timeLeft < 655)

        {
            for (int i = 0; i < 200; i++)

            {
                NPC target = Main.npc[i];
                //If the npc is hostile
                if (target.active)
                {

                    {
                        //Get the shoot trajectory from the projectile and target
                        float shootToX = target.position.X + (float)target.width * 0.5f - Projectile.Center.X;
                        float shootToY = target.position.Y - Projectile.Center.Y;
                        float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

                        //If the distance between the live targeted npc and the projectile is less than 480 pixels
                        Mod calamityMod = ModLoader.GetMod("CalamityMod");
                       
                        if (distance < 700f && !target.friendly && !target.dontTakeDamage && target.type != NPCID.TargetDummy && target.type != calamityMod.Find<ModNPC>("SepulcherTail").Type && target.type != calamityMod.Find<ModNPC>("SepulcherBody").Type && target.type != calamityMod.Find<ModNPC>("SepulcherHead").Type && target.type != calamityMod.Find<ModNPC>("SepulcherBodyEnergyBall").Type)
                        {
                            //Divide the factor, 3f, which is the desired velocity
                            distance = 8f / distance;

                            //Multiply the distance by a multiplier if you wish the projectile to have go faster
                            shootToX *= distance * 8f;
                            shootToY *= distance * 8f;

                            //Set the velocities to the shoot values
                            Projectile.velocity.X = shootToX;
                            Projectile.velocity.Y = shootToY;
                        }
                       

                    }
                }

            }

        }
    }
}

