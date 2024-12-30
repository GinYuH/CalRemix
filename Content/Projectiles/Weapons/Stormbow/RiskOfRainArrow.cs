using CalRemix.Content.DamageClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons.Stormbow
{
    public class RiskOfRainArrow : ModProjectile
    {
        public ref float Mode => ref Projectile.ai[0];
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.BoneArrow);

            Projectile.width = 18;
            Projectile.height = 52;

            Projectile.arrow = true;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<StormbowDamageClass>();
            Projectile.timeLeft = 220;
            Projectile.aiStyle = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.penetrate = 1;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (Mode == 1)
            {
                for (int i = 0; i < 3; i++)
                {
                    Vector2 cursorPos = Main.MouseWorld;
                    cursorPos.X = Projectile.Center.X;
                    cursorPos.Y = Projectile.Center.Y - 800 - (100 * (i * 0.75f));
                    float speedX = Main.rand.Next(-60, 91) * 0.02f;
                    float speedY = Main.rand.Next(-60, 91) * 0.02f;
                    speedY += 15;

                    // arrow position noise pass
                    cursorPos.X += Main.rand.Next(-60, 61);
                    cursorPos.Y += Main.rand.Next(-60, 61);

                    int projectile = Projectile.NewProjectile(Projectile.GetSource_FromThis(), cursorPos, new Vector2(speedX, speedY), ModContent.ProjectileType<RiskOfRainArrow>(), Projectile.damage, 0);

                    for (int ii = 0; ii < 5; ii++)
                    {
                        Dust dust = Dust.NewDustDirect(cursorPos, Projectile.width, Projectile.height, DustID.Electric);
                        dust.noGravity = true;
                        dust.velocity *= 1.5f;
                        dust.scale *= 0.9f;
                    }
                }
            }
        }
        public override void OnKill(int timeLeft)
        {
            if (Mode != 1)
            {
                SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
                for (int i = 0; i < 3; i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Electric);
                    dust.noGravity = true;
                    dust.velocity *= 1.5f;
                    dust.scale *= 0.9f;
                }
            }
        }
        public override bool? CanDamage()
        {
            if (Mode == 1) 
                return false;

            return null;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Mode != 1)
            {
                Texture2D arrow = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
                Main.EntitySpriteDraw(arrow, Projectile.position - Main.screenPosition, null, Color.AliceBlue, Projectile.rotation, Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Size() * 0.5f, Projectile.scale, SpriteEffects.None);
            }
            
            return false;
        }
    }
}