using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons;

// Shortsword projectiles are handled in a special way with how they draw and damage things
// The "hitbox" itself is closer to the player, the sprite is centered on it
// However the interactions with the world will occur offset from this hitbox, closer to the sword's tip (CutTiles, Colliding)
// Values chosen mostly correspond to Iron Shortword
public class ExoPike : ModProjectile
{
    public const int FadeInDuration = 7;
    public const int FadeOutDuration = 4;

    public const int TotalDuration = 16;

    // The "width" of the blade
    public float CollisionWidth => 10f * Projectile.scale;

    public int Timer
    {
        get => (int)Projectile.ai[0];
        set => Projectile.ai[0] = value;
    }

    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("Gravitonomy Beam");
      
    }

    public override void SetDefaults()
    {
        Projectile.width = 20;
        Projectile.height = 20;
       
        Projectile.friendly = true;
        Projectile.penetrate = 1;
        Projectile.tileCollide = false;


      
        Projectile.timeLeft = 360; // This value does not matter since we manually kill it earlier, it just has to be higher than the duration we use in AI
      
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 0;
        Projectile.aiStyle = 0;
        Projectile.DamageType = DamageClass.Melee;
    }
    public override bool PreDraw(ref Color lightColor)
    {
        Mod calamityMod = CalRemix.CalMod;
        GameShaders.Armor.GetShaderFromItemId(calamityMod.Find<ModItem>("ExoDye").Type);
        return true;
    }
    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {


        SoundEngine.PlaySound(SoundID.Item109, Projectile.Center);
        Mod calamityMod = CalRemix.CalMod;
        SoundEngine.PlaySound(SoundID.NPCHit53);
        float spread = 44 * 3.14f;

        float baseSpeed = (float)Math.Sqrt(Projectile.velocity.X * Projectile.velocity.X + Projectile.velocity.Y * Projectile.velocity.Y);

        double startAngle = Math.Atan2(Projectile.velocity.X, Projectile.velocity.Y) - spread / 0.01f;

        double deltaAngle = spread / 40.1f;

        double offsetAngle;

        int i;

        for (i = 0; i < 11; i++)

        {

            offsetAngle = startAngle + deltaAngle * i;

            int a = Terraria.Projectile.NewProjectile(spawnSource: null, Projectile.Center.X, Projectile.Center.Y, 20 * (float)Math.Sin(offsetAngle), 20 * (float)Math.Cos(offsetAngle), Mod.Find<ModProjectile>("ExoPikePhantom").Type, Projectile.damage *= 1, 0, Projectile.owner);

        }


    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        Mod calamityMod = CalRemix.CalMod;
        target.AddBuff(calamityMod.Find<ModBuff>("HolyFlames").Type, 150);
        target.AddBuff(BuffID.Frostburn, 150);
        target.AddBuff(BuffID.OnFire, 150);
    }
   


     
    public override void AI()
    {

        Player player = Main.player[Projectile.owner];
        Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        Lighting.AddLight(Projectile.Center, TorchID.Rainbow);
        for (int i = 0; i < 1; i++)
        {
            int dust = Dust.NewDust(Projectile.position, Projectile.width + 2, Projectile.height + 2, DustID.TintableDustLighted, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 255, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].scale *= 1.00f;
            Main.dust[dust].noLight = false;
        }
        if (Projectile.timeLeft < 340)
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
                        Mod calamityMod = CalRemix.CalMod;
                        if (distance < 500f && !target.friendly && !target.dontTakeDamage && target.type != NPCID.TargetDummy && target.type != calamityMod.Find<ModNPC>("SepulcherTail").Type && target.type != calamityMod.Find<ModNPC>("SepulcherBody").Type && target.type != calamityMod.Find<ModNPC>("SepulcherHead").Type && target.type != calamityMod.Find<ModNPC>("SepulcherBodyEnergyBall").Type)
                        {
                            //Divide the factor, 3f, which is the desired velocity
                            distance = 3.5f / distance;

                            //Multiply the distance by a multiplier if you wish the projectile to have go faster
                            shootToX *= distance * 3.5f;
                            shootToY *= distance * 3.5f;

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

