using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Mono.Cecil;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace CalRemix.Content.Projectiles.Weapons.Stormbow
{
    public class ChlorophyteYoyoProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // The following sets are only applicable to yoyo that use aiStyle 99.

            // YoyosLifeTimeMultiplier is how long in seconds the yoyo will stay out before automatically returning to the player. 
            // Vanilla values range from 3f (Wood) to 16f (Chik), and defaults to -1f. Leaving as -1 will make the time infinite.
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 3.5f;

            // YoyosMaximumRange is the maximum distance the yoyo sleep away from the player. 
            // Vanilla values range from 130f (Wood) to 400f (Terrarian), and defaults to 200f.
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 300f;

            // YoyosTopSpeed is top speed of the yoyo Projectile.
            // Vanilla values range from 9f (Wood) to 17.5f (Terrarian), and defaults to 10f.
            ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 13f;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16; // The width of the projectile's hitbox.
            Projectile.height = 16; // The height of the projectile's hitbox.

            Projectile.aiStyle = ProjAIStyleID.Yoyo; // The projectile's ai style. Yoyos use aiStyle 99 (ProjAIStyleID.Yoyo). A lot of yoyo code checks for this aiStyle to work properly.

            Projectile.friendly = true; // Player shot projectile. Does damage to enemies but not to friendly Town NPCs.
            Projectile.DamageType = DamageClass.MeleeNoSpeed; // Benefits from melee bonuses. MeleeNoSpeed means the item will not scale with attack speed.
            Projectile.penetrate = -1; // All vanilla yoyos have infinite penetration. The number of enemies the yoyo can hit before being pulled back in is based on YoyosLifeTimeMultiplier.
                                       // Projectile.scale = 1f; // The scale of the projectile. Most yoyos are 1f, but a few are larger. The Kraken is the largest at 1.2f
        }

        // notes for aiStyle 99: 
        // localAI[0] is used for timing up to YoyosLifeTimeMultiplier
        // localAI[1] can be used freely by specific types
        // ai[0] and ai[1] usually point towards the x and y world coordinate hover point
        // ai[0] is -1f once YoyosLifeTimeMultiplier is reached, when the player is stoned/frozen, when the yoyo is too far away, or the player is no longer clicking the shoot button.
        // ai[0] being negative makes the yoyo move back towards the player
        // Any AI method can be used for dust, spawning projectiles, etc specific to your yoyo.

        public override void AI()
        {
            Projectile.ai[2]++;
            if (Projectile.ai[2] >= 14)
            {
                SoundEngine.PlaySound(SoundID.Item5, Projectile.position);
                
                // big govt secret: this is actually just a really edited undines retribution. but dont tell anyone that
                // u can edit the i < whatever for extra arrows lool. lol. haha lol
                for (int i = 0; i < 5; i++)
                {
                    Vector2 cursorPos = Main.MouseWorld;
                    cursorPos.X = Projectile.Center.X;
                    cursorPos.Y = Projectile.Center.Y - 800 - (100 * (i * 0.75f));
                    float speedX = Main.rand.Next(-60, 91) * 0.02f;
                    float speedY = Main.rand.Next(-60, 91) * 0.02f;
                    speedY += 15;

                    // arrow position noise pass
                    cursorPos.X += Main.rand.Next(-60, 61);
                    cursorPos.Y += Main.rand.Next(-60, 61);

                    int projToShoot = ProjectileID.ChlorophyteArrow;
                    int awesomeRandomNumber = Main.rand.Next(0, 4);
                    // if 0, chloro arrow
                    if (awesomeRandomNumber == 1)
                    {
                        projToShoot = ProjectileID.ChlorophyteBullet;
                    }
                    else if (awesomeRandomNumber == 2)
                    {
                        projToShoot = ProjectileID.ChlorophyteOrb;
                    }
                    else if (awesomeRandomNumber == 3)
                    {
                        projToShoot = ProjectileID.SporeCloud;
                    }

                    int projectile = Projectile.NewProjectile(Projectile.GetSource_FromThis(), cursorPos, new Vector2(speedX, speedY), projToShoot, Projectile.damage, 0);
                    Projectile.ai[2] = 0;
                }
            }
        }
    }
}
