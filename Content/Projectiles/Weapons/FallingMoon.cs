using CalamityMod;
using CalRemix.Content.DamageClasses;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    //This one looks kinda bad. Am probably gonna improve the visuals as I learn more
    public class FallingMoon : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 480;
            Projectile.height = 480;
            Projectile.friendly = true;
            Projectile.aiStyle = 2;
            Projectile.penetrate = 10;
            Projectile.ignoreWater = true;
            Projectile.DamageType = ModContent.GetInstance<StormbowDamageClass>();
            Main.NewText(Projectile.ai[0]);
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath14);
        }
    }
}