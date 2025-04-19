using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class PinkSquare : ModProjectile
	{
        public override void SetStaticDefaults() 
        {
			// DisplayName.SetDefault("Square");
		}
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 999;
            Projectile.timeLeft = 3000;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 40;
        }
        public override void AI()
        {
            if (Main.projectile[(int)Projectile.ai[0]] != null && Main.projectile[(int)Projectile.ai[0]].active && Main.projectile[(int)Projectile.ai[0]].type == ModContent.ProjectileType<UnsealedBlackhole>())
            {
                if (Projectile.Hitbox.Intersects(Main.projectile[(int)Projectile.ai[0]].Hitbox))
                    Projectile.Kill();
            }
            else
                Projectile.Kill();

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