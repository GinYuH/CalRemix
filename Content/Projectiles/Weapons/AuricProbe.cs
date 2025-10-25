using CalamityMod;
using CalamityMod.Dusts;
using CalamityMod.NPCs.ExoMechs.Ares;
using CalamityMod.Particles;
using CalamityMod.Sounds;
using Microsoft.Build.Evaluation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class AuricProbe : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.timeLeft = 1200;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.alpha = 255;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Projectile.velocity *= 0.98f;

            if (Projectile.velocity.Length() < 1)
            {
                if (Projectile.ai[0] == 0)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        Dust d = Dust.NewDustPerfect(Projectile.Center + Vector2.UnitY.RotatedBy(MathHelper.Lerp(0, MathHelper.TwoPi, i / 20f)) * 5, (int)CalamityDusts.ProfanedFire, Scale: 0.4f);
                        d.noGravity = true;
                        d.velocity = Vector2.Zero;
                    }
                    Projectile.ai[0] = 1;
                }
                foreach (Projectile p in Main.ActiveProjectiles)
                {
                    if (p.type == Type)
                        continue;
                    if (p.hostile || !p.friendly)
                        continue;
                    if (p.getRect().Intersects(Projectile.getRect()))
                    {
                        p.penetrate--;
                        Projectile.ai[1] = 1;
                        Projectile.Kill();
                        break;
                    }
                }
            }

            if (Projectile.timeLeft > 60)
                Projectile.alpha -= 30;
            else
            {
                Projectile.ai[0] = 2;
                Projectile.alpha += 50;
            }
        }

        public override void OnKill(int timeLeft)
        {
            if (Projectile.ai[1] == 1)
            {
                Projectile.position = Projectile.Center;
                Projectile.width = Projectile.height = 500;
                Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
                Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
                Projectile.maxPenetrate = -1;
                Projectile.penetrate = -1;
                Projectile.usesLocalNPCImmunity = true;
                Projectile.localNPCHitCooldown = 10;
                Projectile.Damage();
                SoundEngine.PlaySound(AresTeslaCannon.TeslaOrbShootSound with { Pitch = -0.5f }, Projectile.Center);
                SoundEngine.PlaySound(CommonCalamitySounds.LargeWeaponFireSound, Projectile.Center);
                CalRemixHelper.DustExplosionOutward(Projectile.Center, DustID.Electric, speedMin: 6, speedMax: 40, 100, default, 0, scale: Main.rand.NextFloat(1f, 2f));
                GeneralParticleHandler.SpawnParticle(new PlasmaExplosion(Projectile.Center, Vector2.Zero, Color.Cyan, Vector2.One, Main.rand.NextFloat(MathHelper.TwoPi), 0.1f, 0.4f, 20));
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;  
            for (int i = 0; i < 8; i++)
            {
                Main.EntitySpriteDraw(tex, Projectile.Center + new Vector2(0, Projectile.gfxOffY) - Main.screenPosition + Vector2.UnitY.RotatedBy(MathHelper.Lerp(0, MathHelper.TwoPi, i / 8f)) * 4, null, Color.CornflowerBlue * 0.6f, 0, tex.Size() / 2, Projectile.scale, 0);
            }
            Main.EntitySpriteDraw(tex, Projectile.Center + new Vector2(0, Projectile.gfxOffY) - Main.screenPosition, null, lightColor, 0, tex.Size() / 2, Projectile.scale, 0);
            return false;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return Projectile.ai[1] == 1;
        }
    }
}