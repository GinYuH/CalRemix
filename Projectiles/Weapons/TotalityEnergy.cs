using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using CalamityMod;
using Terraria.DataStructures;

namespace CalRemix.Projectiles.Weapons
{
	public class TotalityEnergy : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/Summon/CosmicEnergySpiral";
        public ref float Timer => ref Projectile.ai[0];
        public Player Owner => Main.player[Projectile.owner];
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Exo Rammer");
        }

        public override void SetDefaults()
        {
            Projectile.width = 78;
            Projectile.height = 78;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.ai[0] = 100f;
        }
        public override void AI()
        {
            Lighting.AddLight((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16, Main.DiscoR / 255f, Main.DiscoG / 255f, Main.DiscoB / 255f);
            bool projType = Projectile.type == ModContent.ProjectileType<TotalityEnergy>();
            if (!projType || Owner.dead)
                Projectile.Kill();

            Projectile.rotation += Projectile.velocity.X * 0.1f;
            bool flag = false;

            if (Owner.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[Owner.MinionAttackTargetNPC];
                flag = targetNPC(npc, flag);

            }
            else
            {
                for (int i = 0; i < 200; i++)
                {
                    NPC npc = Main.npc[i];
                    flag = targetNPC(npc, flag);
                }
            }
            if (Vector2.Distance(Owner.Center, Projectile.Center) > 1600f)
                flag = false;
            if (flag)
                Projectile.ChargingMinionAI(1200f, 1500f, 2400f, 150f, 0, 30f, 18f, 9f, new Vector2(0f, -60f), 30f, 12f, tileVision: true, ignoreTilesWhenCharging: true);
            else
            {
                Vector2 targetVect = Owner.Center - Projectile.Center + new Vector2(0f, -60f);
                float targetDist = targetVect.Length();
                if (targetDist < 800f && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
                {
                    Projectile.netUpdate = true;
                }

                if (targetDist > 2000f)
                {
                    Projectile.position.X = Owner.Center.X - (Projectile.width / 2);
                    Projectile.position.Y = Owner.Center.Y - (Projectile.height / 2);
                    Projectile.netUpdate = true;
                }
                if (targetDist > 70f)
                {
                    targetVect.Normalize();
                    targetVect *= 6;
                    Projectile.velocity = (Projectile.velocity * 40f + targetVect) / 41f;
                }
                if (Projectile.velocity.X == 0f && Projectile.velocity.Y == 0f)
                {
                    Projectile.velocity.X = -0.15f;
                    Projectile.velocity.Y = -0.05f;
                }
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 255);
        }
        private bool targetNPC(NPC npc, bool flag)
        {
            if (npc.CanBeChasedBy(Projectile))
            {
                float targetDistance = Vector2.Distance(npc.Center, Projectile.Center);
                if (!flag && targetDistance < 1400f)
                {
                    return true;
                }
            }
            return false;
        }
    }
}


