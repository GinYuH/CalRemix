using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using System;

namespace CalRemix.Content.Projectiles.Weapons
{
	public class TotalityScythe : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/Rogue/CelestusMiniScythe";
        public ref float Timer => ref Projectile.ai[0];
        public Player Owner => Main.player[Projectile.owner];
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Exo Scythe");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 4;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 40;
            Projectile.timeLeft = 120;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
        }
        public override void AI()
        {
            Projectile.rotation += 2f;
            Lighting.AddLight(Projectile.Center, Main.DiscoR * 0.5f / 255f, Main.DiscoG * 0.5f / 255f, Main.DiscoB * 0.5f / 255f);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                double num = Math.Atan2(Projectile.velocity.X, Projectile.velocity.Y) - (double)(0.783f / 2f);
                for (int i = 0; i < 4; i++)
                {
                    double vel = num + 0.783f / 8f * (i + i * i) / 2.0 + (32f * i);
                    int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(Math.Sin(vel) * 2.0), (float)(Math.Cos(vel) * 2.0), ModContent.ProjectileType<CelestusMiniScythe>(), (int)(Projectile.damage * 0.7), Projectile.knockBack, Projectile.owner);
                    int proj2 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)((0.0 - Math.Sin(vel)) * 2.0), (float)((0.0 - Math.Cos(vel)) * 2.0), ModContent.ProjectileType<CelestusMiniScythe>(), (int)(Projectile.damage * 0.7), Projectile.knockBack, Projectile.owner);
                    Main.projectile[proj].DamageType = DamageClass.MeleeNoSpeed;
                    Main.projectile[proj2].DamageType = DamageClass.MeleeNoSpeed;
                }
            }
            SoundEngine.PlaySound(SoundID.Item122, Projectile.Center);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.timeLeft < 85)
            {
                byte b = (byte)(Projectile.timeLeft * 3);
                byte alpha = (byte)(100f * b / 255f);
                return new Color(b, b, b, alpha);
            }
            return new Color(255, 255, 255, 100);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor);
            return false;
        }
    }
}


