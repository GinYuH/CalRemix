using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Armor.RajahChampion.Drone
{
    public class RajahDroneShot : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Carrot");
            Main.projFrames[Projectile.type] = 4;
		}

		public override void SetDefaults()
		{
            Projectile.DamageType = DamageClass.Ranged;
			Projectile.width = 10; 
			Projectile.height = 10; 
			Projectile.friendly = true; 
			Projectile.hostile = false;  
			Projectile.penetrate = 1;  
			Projectile.timeLeft = 600;  
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 0;
		}

        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
            const int aislotHomingCooldown = 0;
            const int homingDelay = 15;
            const float desiredFlySpeedInPixelsPerFrame = 10;
            const float amountOfFramesToLerpBy = 40; 

            Projectile.ai[aislotHomingCooldown]++;
            if (Projectile.ai[aislotHomingCooldown] > homingDelay)
            {
                Projectile.ai[aislotHomingCooldown] = homingDelay;

                int foundTarget = HomeOnTarget();
                if (foundTarget != -1)
                {
                    NPC n = Main.npc[foundTarget];
                    Vector2 desiredVelocity = Projectile.DirectionTo(n.Center) * desiredFlySpeedInPixelsPerFrame;
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, desiredVelocity, 1f / amountOfFramesToLerpBy);
                }
            }
        }

        private int HomeOnTarget()
        {
            const bool homingCanAimAtWetEnemies = true;
            const float homingMaximumRangeInPixels = 400;

            int selectedTarget = -1;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC n = Main.npc[i];
                if (n.CanBeChasedBy(Projectile) && (!n.wet || homingCanAimAtWetEnemies))
                {
                    float distance = Projectile.Distance(n.Center);
                    if (distance <= homingMaximumRangeInPixels &&
                        (
                            selectedTarget == -1 || //there is no selected target
                            Projectile.Distance(Main.npc[selectedTarget].Center) > distance)
                    )
                        selectedTarget = i;
                }
            }

            return selectedTarget;
        }

        public override void OnKill(int timeleft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

            int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), (int)Projectile.Center.X, (int)Projectile.Center.Y, 0, 0, ModContent.ProjectileType<DroneBoom>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
            Main.projectile[p].Center = Projectile.Center;
            Main.projectile[p].netUpdate = true;

            for (int num468 = 0; num468 < 5; num468++)
            {
                int num469 = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Torch, -Projectile.velocity.X * 0.2f,
                    -Projectile.velocity.Y * 0.2f, 100);
                Main.dust[num469].velocity *= 2f;
            }
        }
    }
}
