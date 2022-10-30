using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Items.PermanentBoosters;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Projectiles.Ranged;
using CsvHelper.TypeConversion;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles
{
	public class FallingPhantomHeart : ModProjectile
	{
		public override void SetStaticDefaults() 
        {
			DisplayName.SetDefault("Phantom Heart");
		}
		public override void SetDefaults() 
        {
            Projectile.width = 36;
			Projectile.height = 36;
			Projectile.aiStyle = ProjAIStyleID.FallingStar;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
		}
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Player player = Main.LocalPlayer;
            if (Projectile.Center.Y == player.Center.Y && !Projectile.tileCollide)
            {
                Projectile.tileCollide = true;
                return true;
            }
            if (Projectile.tileCollide)
            {
                Projectile.Kill();
                return true;
            }
            return false;
        }
        public override void Kill(int timeLeft)
        {
            Item.NewItem(Projectile.GetSource_DropAsItem(), Projectile.getRect(), ModContent.ItemType<PhantomHeart>());
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