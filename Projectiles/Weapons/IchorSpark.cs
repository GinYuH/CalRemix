using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Weapons
{
	public class IchorSpark : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ichor Spark");
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.aiStyle = ProjAIStyleID.GroundProjectile;
        }
        public override void AI()
        {
            Projectile.rotation = 0;
            if (Projectile.timeLeft % 12 == 0)
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center + Projectile.velocity, Projectile.width, Projectile.height, DustID.GoldCoin, Projectile.oldVelocity.X, Projectile.oldVelocity.Y);
                dust.noGravity = true;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 position = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            Rectangle sourceRectangle = new(0, 0, texture.Width, texture.Height);
            Vector2 origin = sourceRectangle.Size() / 2f;
            Color drawColor = Projectile.GetAlpha(new Color(255, 255, 255, 255));
            Main.spriteBatch.Draw(texture, position, sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}