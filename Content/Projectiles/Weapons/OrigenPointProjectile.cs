using CalamityMod;
using CalRemix.Content.Items.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class OrigenPointProjectile : ModProjectile
    {
        public override string Texture => "CalRemix/Content/Items/Weapons/OrigenPoint";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Point");
        }
        public override void SetDefaults()
        {
            Projectile.width = 44;
            Projectile.height = 44;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.scale = 0.05f;
            Projectile.penetrate = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 60;
        }


        public override void AI()
        {
            CalamityUtils.HomeInOnNPC(Projectile, true, 1200, 0.2f, 0);
            Color color = OrigenPoint.origenPalette[(int)Main.rand.Next(0, OrigenPoint.origenPalette.Count)];
            Lighting.AddLight(Projectile.Center, color.R * 0.00392f, color.G * 0.00392f, color.B * 0.00392f);
            if (Projectile.timeLeft > 90)
            {
                if (Projectile.scale < 1)
                {
                    Projectile.scale += 0.05f;
                }
                else
                {
                    Projectile.scale = 1;
                }
            }
            else
            {
                if (Projectile.timeLeft == 90)
                {
                    SoundEngine.PlaySound(CalamityMod.CalPlayer.CalamityPlayer.DrownSound, Projectile.Center);
                }
                if (Projectile.scale >= 0)
                {
                    Projectile.scale -= 0.05f;
                }
                if (Projectile.scale <= 0)
                {
                    Projectile.Kill();
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Color color = OrigenPoint.origenPalette[(int)Main.rand.Next(0, OrigenPoint.origenPalette.Count)];
            Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition, null, color, 0, TextureAssets.Projectile[Type].Value.Size() / 2, Projectile.scale, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
            return false;
        }
    }
}