using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Build.Tasks.Deployment.ManifestUtilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Hostile
{
    public class OxygenBubble : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/Typeless/CoralBubble";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bubble");
        }
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.hostile = true;
            Projectile.timeLeft = 180;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
        }
        public override void AI()
        {
            if (Projectile.scale < 2.01f)
            {
                Projectile.scale += 0.01f;
                Projectile.alpha -= 5;
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item54, Projectile.Center);
            int padding = 0;
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(Projectile.position - new Vector2(padding, padding), Projectile.width + padding, Projectile.height + padding, DustID.BubbleBlock, Scale: Main.rand.NextFloat(0.2f, 0.5f));
            }
        }
        public override bool? CanDamage()
        {
            return Projectile.scale > 2f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawProjectileWithBackglow(Projectile, Color.LightBlue * 0.4f, lightColor, 8);
            return false;
        }
    }
}