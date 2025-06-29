using CalamityMod;
using CalRemix.Content.DamageClasses;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class FearlessCrabWarrior : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.aiStyle = 1;
            Projectile.penetrate = 3;
            Projectile.DamageType = ModContent.GetInstance<StormbowDamageClass>();
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath1);
        }
    }
}