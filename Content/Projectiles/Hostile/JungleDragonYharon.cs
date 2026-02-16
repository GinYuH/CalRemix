using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using CalamityMod.NPCs.Yharon;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class JungleDragonYharon : ModProjectile
    {
        public override string Texture => "CalamityMod/NPCs/Yharon/Yharon";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Yharon");
            Main.projFrames[Projectile.type] = 7;
        }
        public override void SetDefaults()
        {
            Projectile.width = 200;
            Projectile.height = 200;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 4)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= 6)
            {
                Projectile.frame = 0;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects fx = Projectile.velocity.X > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Asset<Texture2D> tex = TextureAssets.Projectile[Type];
            Vector2 centered = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            Main.EntitySpriteDraw(tex.Value, centered, tex.Frame(1, 7, 0, Projectile.frame), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(tex.Value.Width / 2, tex.Value.Height / 14), Projectile.scale, fx, 0);
            return false;
        }
        public override bool? CanDamage() => false;

        public override void OnKill(int timeLeft)
        {
            if (Main.zenithWorld) CalRemixHelper.SpawnNewNPC(base.Projectile.GetSource_FromThis(), (int)Projectile.Center.X, (int)Projectile.Center.Y, ModContent.NPCType<Yharon>());
        }
    }
}