using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CalRemix.Content.Items.Armor.RajahChampion.Drone
{
    public class RajahDrone : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Legendary Rainbow Cat");
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = 248;
            Projectile.height = 142;
            Projectile.penetrate = -1;
            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.alpha = 255;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 480;
        }

        public Entity target = null;

        public override void AI()
        {
            if (++Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 4)
                {
                    Projectile.frame = 0;
                }
            }

            Projectile.ai[0]++;

            if (Projectile.ai[0] < 450)
            {
                if (Projectile.alpha > 0)
                {
                    Projectile.alpha -= 4;
                }
                else
                {
                    Projectile.alpha = 0;
                }
                Projectile.velocity = default;

                Target();

                if (target != null && Main.netMode != 2 && Projectile.owner == Main.myPlayer)
                {
                    Projectile.localAI[0]--;
                    if (Projectile.localAI[0] <= 0)
                    {
                        Projectile.localAI[0] = 90f;
                        Vector2 velocity = Projectile.DirectionTo(target.Center) * 10f;
                        int projID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y - 4f, 0f, 0f, ModContent.ProjectileType<RajahDroneShot>(), Projectile.damage, 0f, Projectile.owner);
                        Main.projectile[projID].velocity = velocity;
                        Main.projectile[projID].netUpdate = true;
                    }
                }
            }
            else
            {
                if (Projectile.alpha < 255)
                {
                    Projectile.alpha += 4;
                }

                if (Projectile.ai[0] >= 480)
                {
                    Main.player[Projectile.owner].AddBuff(Mod.Find<ModBuff>("DroneCool").Type, 900);
                    Projectile.Kill();
                }
            }
        }

        public bool CanTarget(Entity codable, Vector2 startPos)
        {
            if (codable is NPC npc)
            {
                return npc.active && npc.life > 0 && !npc.friendly && !npc.dontTakeDamage && npc.lifeMax > 5 && Vector2.Distance(startPos, npc.Center) < 800f;
            }
            return false;
        }

        public void Target()
        {
            Vector2 startPos = Projectile.Center;
            if (target != null && !CanTarget(target, startPos)) target = null;
            if (target == null)
            {
                target = Projectile.FindTargetWithinRange(800f, true);
            }
        }
    }
}
