using CalamityMod;
using CalamityMod.NPCs.SupremeCalamitas;
using CalamityMod.Sounds;
using CalRemix.Content.Items.Weapons;
using Microsoft.Xna.Framework;
using Mono.Cecil;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons.Stormbow
{
    public class TerraHawk : ModProjectile
    {

        private const int TimeLeft = 180;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.timeLeft = TimeLeft;
            Projectile.aiStyle = -1;
            Projectile.width = 80;
            Projectile.height = 36;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.Pi;

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= Main.projFrames[Type])
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= Main.projFrames[Type])
            {
                Projectile.frame = 0;
            }

            Projectile.localAI[1] += 1;
            if (Projectile.localAI[1] >= 10)
            {
                int projToShoot = ProjectileID.TerraBeam;
                SoundStyle soundToPlay = SoundID.Item9;
                
                int projectile = Projectile.NewProjectile(Main.player[Projectile.owner].GetSource_FromThis(), Projectile.Center, new Vector2(0, 15), projToShoot, Projectile.damage, 0, Projectile.owner);
                SoundEngine.PlaySound(soundToPlay, Projectile.Center);
                Projectile.localAI[1] = 0;
            }

            int dust = Dust.NewDust(Projectile.oldPosition + Projectile.oldVelocity, Projectile.width, Projectile.height, DustID.Terra, 0f, 0f, 100, default, 1.25f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 0f;
            Main.dust[dust].noLightEmittence = true;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(CommonCalamitySounds.ExoDeathSound, Projectile.Center);
            for (int i = 4; i < 31; i++)
            {
                float projOldX = Projectile.oldVelocity.X * (30f / i);
                float projOldY = Projectile.oldVelocity.Y * (30f / i);
                int dust = Dust.NewDust(new Vector2(Projectile.oldPosition.X - projOldX, Projectile.oldPosition.Y - projOldY), 8, 8, DustID.Terra, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default, 1.8f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].noLightEmittence = true;

                dust = Dust.NewDust(new Vector2(Projectile.oldPosition.X - projOldX, Projectile.oldPosition.Y - projOldY), 8, 8, DustID.Terra, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default, 1.4f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.1f;
                Main.dust[dust].noLightEmittence = true;
            }
        }
    }
}
