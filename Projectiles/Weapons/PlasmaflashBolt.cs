using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Weapons
{
	public class PlasmaflashBolt : ModProjectile
	{
        public override void SetStaticDefaults() 
        {
			DisplayName.SetDefault("Plasmaflash Bolt");
        }
		public override void SetDefaults() 
        {
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 5;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.extraUpdates = 10;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Main.rand.NextBool(20))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(10)).X / 2, Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(10)).Y / 2);
                Main.dust[dust].noGravity = true;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Electrified, 600);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, new Color(255, 255, 255, 255), Projectile.rotation, texture.Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}