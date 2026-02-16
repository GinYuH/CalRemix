using CalRemix.Content.DamageClasses;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class UFOLarge : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 46;
            Projectile.height = 70;
            Projectile.damage = 320;
            Projectile.friendly = true;
            Projectile.aiStyle = ProjAIStyleID.Arrow;
            Projectile.DamageType = ModContent.GetInstance<StormbowDamageClass>();
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath44);
        }
        public override void AI()
        {
            if (Main.rand.NextBool(2))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
        }
    }
}