using CalamityMod;
using CalRemix.Content.NPCs.Subworlds.Sealed;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class PinkSquareHoming : ModProjectile
    {
        public override string Texture => "CalRemix/Content/Projectiles/Weapons/PinkSquare";

        public override void SetDefaults()
        {
            Projectile.width = 400;
            Projectile.height = 400;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 3000;
            Projectile.scale = 0.02f;
        }
        public override void AI()
        {
            if (VoidBoss.VoidIDX <= -1)
            {
                Projectile.Kill();
            }
            else
            {
                NPC n = Main.npc[VoidBoss.VoidIDX];
                if (!n.active || n.life <= 0)
                {
                    Projectile.Kill();
                }
            }
            Player p = Main.player[(int)Projectile.ai[0]];
            if (!p.active || p.dead)
            {
                Projectile.Kill();
            }
            else
            {
                Projectile.velocity = Projectile.DirectionTo(p.Center) * 4;
            }

            Projectile.ai[1]++;
            if (Projectile.ai[1] > 300)
            {
                Projectile.scale -= 0.05f;
                if (Projectile.scale <= 0.01f)
                {
                    Projectile.Kill();
                }
            }
            else
            {
                if (Projectile.scale < 1f)
                {
                    Projectile.scale += 0.1f;
                    if (Projectile.scale > 1)
                    {
                        Projectile.scale = 1;
                        Projectile.width = Projectile.height = 400;
                    }
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Rectangle sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            Vector2 origin = sourceRectangle.Size() / 2f;
            Color drawColor = Projectile.GetAlpha(new Color(255, 255, 255, 255));
            Main.spriteBatch.Draw(texture,
                Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                sourceRectangle, drawColor, Projectile.rotation, origin, 400 / (float)texture.Width * Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public override bool CanHitPlayer(Player target)
        {
            return Projectile.scale >= 1;
        }
    }
}