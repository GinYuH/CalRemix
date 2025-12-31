using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class CaneOrb : ModProjectile
    {
        public ref float Time => ref Projectile.ai[0];
        public ref float Image => ref Projectile.ai[1];
        public override string Texture => "Terraria/Images/Item_" + ItemID.DarkShard;
        public override void SetStaticDefaults() 
        {
			// DisplayName.SetDefault("CaneOrb");
		}
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 10;
            Projectile.timeLeft = 300;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 6;
        }
        public override void AI()
        {
            Time += 0.5f;
            float wave = (float)Math.Sin((Time/3.25)+ Math.PI/2) * 15f;
            Vector2 direction = Projectile.velocity.SafeNormalize(Vector2.Zero).RotatedBy(Math.PI/2);
            Projectile.Center += (Image == 0) ? direction * wave : -direction * wave;

        }
        public override bool PreDraw(ref Color lightColor)
        {
            string pick = (Image == 0) ? "Terraria/Images/Item_" + ItemID.DarkShard : "Terraria/Images/Item_" + ItemID.LightShard;
            Texture2D texture = ModContent.Request<Texture2D>(pick).Value;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, Vector2.Zero, Projectile.scale, (Projectile.spriteDirection != 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
            return false;
        }
    }
}