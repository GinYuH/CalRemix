using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Bosses.RajahBoss.Supreme
{
    public class BaneTEXR : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Bane of the Bunny");
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 1;
        }

        public bool StuckInEnemy = false;
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            Rectangle myRect = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
            bool flag3 = Projectile.Colliding(myRect, target.getRect());
            target.AddBuff(ModContent.BuffType<Buffs.SpearStuck>(), 2);
            if (flag3 && !StuckInEnemy)
            {
                StuckInEnemy = true;
                Projectile.ai[0] = 1f;
                Projectile.ai[1] = target.whoAmI;
                Projectile.velocity = (target.Center - Projectile.Center) * 0.75f;
                Projectile.netUpdate = true;
            }
        }

        public override void OnKill(int timeLeft)
        {
            if (Projectile.ai[0] == 1f)
            {
               ;
            }
        }

        public override void AI()
        {
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 25;
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
                else if (Main.player[num978].active || !Main.player[num978].dead)
                {
                    Projectile.Center = Main.player[num978].Center - Projectile.velocity * 2f;
                    Projectile.gfxOffY = Main.player[num978].gfxOffY;
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
        public override bool PreDraw(ref Color lightColor)
        {
            Rectangle frame = BaseDrawing.GetFrame(Projectile.frame, TextureAssets.Projectile[Projectile.type].Value.Width, TextureAssets.Projectile[Projectile.type].Value.Height, 0, 2);
            BaseDrawing.DrawTexture(Main.spriteBatch, TextureAssets.Projectile[Projectile.type].Value, 0, Projectile.position, Projectile.width, Projectile.height, Projectile.scale, Projectile.rotation, 0, 1, frame, lightColor, true);
            return false;
        }
    }
}
