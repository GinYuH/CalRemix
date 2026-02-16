using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalRemix.Content.Buffs;

namespace CalRemix.Content.NPCs.Bosses.RajahBoss
{
    public class BaneR: ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Bane of the Bunny");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.hostile = true;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 1;
        }

        public bool StuckInEnemy = false;
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            Rectangle myRect = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
            bool flag3 = Projectile.Colliding(myRect, target.getRect());
            target.AddBuff(ModContent.BuffType<SpearStuck>(), 2);
            if (flag3 && !StuckInEnemy)
            {
                StuckInEnemy = true;
                Projectile.ai[0] = 1f;
                Projectile.ai[1] = target.whoAmI;
                Projectile.velocity = (target.Center - Projectile.Center) * 0.75f;
                Projectile.netUpdate = true;
            }
        }

        public override void AI()
        {
            int num972 = 25;
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= num972;
            }
            if (Projectile.alpha < 0)
            {
                Projectile.alpha = 0;
            }
            if (Projectile.ai[0] == 0f)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + 1.57079637f;
            }
            if (Projectile.ai[0] == 1f)
            {
                Projectile.damage = 0;
                Projectile.ignoreWater = true;
                Projectile.tileCollide = false;
                int num977 = 15;
                bool flag53 = false;
                Projectile.localAI[0] += 1f;
                int num978 = (int)Projectile.ai[1];
                if (Projectile.localAI[0] >= 60 * num977)
                {
                    flag53 = true;
                }
                else if (num978 < 0 || num978 >= 200)
                {
                    flag53 = true;
                }
                else if (Main.player[num978].active && !Main.player[num978].dead)
                {
                    Projectile.Center = Main.player[num978].Center - Projectile.velocity * 2f;
                    Projectile.gfxOffY = Main.player[num978].gfxOffY;
                    Main.player[num978].AddBuff(BuffID.Bleeding, 2);
                }
                else
                {
                    flag53 = true;
                }
                if (flag53)
                {
                    Projectile.Kill();
                }
            }
        }
    }
}
