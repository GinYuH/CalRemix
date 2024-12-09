using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class ObsidianFragment : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            if (!Main.dedServ)
                Main.instance.LoadProjectile(ProjectileID.DeerclopsRangedProjectile);
        }
        public override void SetDefaults()
        {
            Projectile.timeLeft = 300;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.width = Projectile.height = 30;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.X.DirectionalSign() * 0.6f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Main.zenithWorld ? TextureAssets.Projectile[ProjectileID.DeerclopsRangedProjectile].Value : TextureAssets.Projectile[Type].Value;
            
            if (Main.zenithWorld)
            {
                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, texture.Frame(3, 4, (int)MathHelper.Min(Projectile.ai[0], 3), 3), lightColor, Projectile.rotation, texture.Size() / 8, Projectile.scale * 2, 0, 0);
            }
            else
            {
                if (Projectile.ai[0] > 0)
                {
                    texture = ModContent.Request<Texture2D>(Texture + (Projectile.ai[0] + 1)).Value;
                }
                CalamityUtils.DrawAfterimagesCentered(Projectile, 0, Color.Purple, 3, texture);
                CalamityUtils.DrawProjectileWithBackglow(Projectile, Color.Violet, lightColor, 4, texture);
            }
            
            return false;
        }
    }
}