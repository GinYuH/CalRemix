using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Weapons
{
	public class ThunderBoltBolt : ModProjectile
	{
        public override void SetStaticDefaults() 
        {
			DisplayName.SetDefault("Lightning Ball");
        }
		public override void SetDefaults() 
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 4;
            Projectile.DamageType = DamageClass.Magic;
        }
        public override void AI()
        {
            Projectile.rotation += 0.1f;
            if (Main.rand.NextBool(6))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(10)).X / 2, Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(10)).Y / 2);
                Main.dust[dust].noGravity = true;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Electrified, 180);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X;
                Projectile.penetrate--;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
                Projectile.penetrate--;
            }
            if (Projectile.penetrate < 1)
                return true;
            return false;
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