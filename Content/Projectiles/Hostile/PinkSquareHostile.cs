using CalamityMod;
using CalRemix.Content.NPCs.Subworlds.Sealed;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class PinkSquareHostile : ModProjectile
    {
        public override string Texture => "CalRemix/Content/Projectiles/Weapons/PinkSquare";
        
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
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
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Rectangle sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            Vector2 origin = sourceRectangle.Size() / 2f;
            Color drawColor = Projectile.GetAlpha(new Color(255, 255, 255, 255));
            Main.spriteBatch.Draw(texture,
                Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}