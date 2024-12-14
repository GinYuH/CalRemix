using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using CalamityMod.Buffs.StatDebuffs;

namespace CalRemix.Content.Projectiles.Weapons
{
	public class CrescentSlash : ModProjectile
	{
        public override void SetStaticDefaults() 
        {
			DisplayName.SetDefault("Crescent Slash");
		}
		public override void SetDefaults() 
        {
            Projectile.CloneDefaults(ProjectileID.FrostWave);
            Projectile.friendly = true;
			Projectile.hostile = false;
            Projectile.timeLeft = 80;
			Projectile.DamageType = DamageClass.Summon;
            Projectile.aiStyle = 1;
        }
        public override void AI()
        {
            if (Main.rand.NextBool(5))
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Flare, -Projectile.velocity.X, -Projectile.velocity.Y, 255, default, 1f);
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
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<WhisperingDeath>(), 120);
            target.AddBuff(BuffID.OnFire3, 120);
        }
    }
}