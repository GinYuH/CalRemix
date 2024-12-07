using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
	public class RoseArrow : ModProjectile
	{
		public override void SetDefaults() 
        {
            Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
            AIType = ProjectileID.WoodenArrowFriendly;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
            Projectile.NewProjectile(Projectile.GetSource_OnHit(target), Projectile.position, Vector2.Zero, ModContent.ProjectileType<RoseSlash>(), Projectile.damage / 2, 0);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Color drawColor = Projectile.GetAlpha(new Color(255, 255, 255, 255));
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, drawColor, Projectile.rotation, texture.Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}