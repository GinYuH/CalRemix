using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class OnyxFragment : ModProjectile
    {
        public ref float Timer => ref Projectile.ai[0];
        public ref float State => ref Projectile.ai[1];
        public Player Owner => Main.player[Projectile.owner];
        private Vector2 InitVelocity;
        public override void SetStaticDefaults() 
        {
			DisplayName.SetDefault("Onyx Fragment");
		}
		public override void SetDefaults() 
        {
            Projectile.aiStyle = ProjAIStyleID.ThrownProjectile;
            Projectile.width = 46;
			Projectile.height = 46;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
        }
        public override bool? CanHitNPC(NPC target) => false;
        public override void OnKill(int timeLeft)
        {
            Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ProjectileID.BlackBolt, Projectile.damage, Projectile.knockBack);
            proj.DamageType = DamageClass.Magic;
            proj.Kill();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Texture2D texture2 = ModContent.Request<Texture2D>("CalRemix/Content/Projectiles/Weapons/OnyxCore").Value;
            Vector2 centered = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            Main.EntitySpriteDraw(texture2, centered, null, new Color(255, 255, 255, 255), 0, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(texture, centered, null, lightColor, Projectile.rotation, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}