using CalamityMod;
using CalRemix.Content.NPCs.Bosses.RebornModPhoenixes.Vernix;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Bosses.RebornModPhoenixes.Chaotrix
{
    public class FireWave : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 2;
        }
        
        public override void SetDefaults()
        {
            Projectile.aiStyle = 0;
            Projectile.hostile = true;
            Projectile.height = 66;
            Projectile.penetrate = 50;
            Projectile.tileCollide = false;
            Projectile.width = 98;
            Projectile.timeLeft = 200;
            //killPretendType=15   uhhhmmmm documentation says this controls what happens on death but im too lazy to copy it

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 6;
        }

        public override void AI()
        {
            Projectile.velocity = Projectile.velocity.ClampMagnitude(-22, 22);

            Projectile.frameCounter++;
            if (Projectile.frameCounter > 5)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 1)
                Projectile.frame = 0;
        }

        public override void OnKill(int timeLeft)
        {
            Projectile.active = false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            SpriteEffects flip = SpriteEffects.None;
            if (Projectile.velocity.X > 0)
                flip = SpriteEffects.FlipHorizontally;
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, tex.Frame(1, 2, 0, Projectile.frame), lightColor, Projectile.rotation, Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Size() * 0.5f, Projectile.scale, flip);
            return false;
        }
    }
}
