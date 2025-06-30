using CalamityMod;
using CalamityMod.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using System;
using Terraria.ModLoader;
using CalamityMod.Projectiles.Magic;
using CalRemix.Content.Projectiles.Hostile;
using Humanizer;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class PliobrineProj : ModProjectile
    {
        public override string Texture => "CalRemix/Content/Items/Weapons/Pliobrine";

        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] > 3)
            {
                Projectile.tileCollide = true;
            }
            if (Projectile.ai[0] > 10f)
            {
                Projectile.ai[0] = 10f;
                if (Projectile.velocity.Y == 0f && Projectile.velocity.X != 0f)
                {
                    Projectile.velocity.X = Projectile.velocity.X * 0.97f;
                    if (Projectile.velocity.X > -0.01f && Projectile.velocity.X < 0.01f)
                    {
                        Projectile.velocity.X = 0f;
                        Projectile.netUpdate = true;
                    }
                }
                Projectile.velocity.Y += 0.2f;
            }
            Projectile.rotation += Projectile.velocity.X * 0.05f;
        }

        public override void OnKill(int timeLeft)
        {
            CalRemixHelper.DustExplosionOutward(Projectile.Center, DustID.Obsidian, 4f, 10f, alpha: 50, scaleMin: 0.8f, scaleMax: 1.2f);
            int projAmt = Projectile.Calamity().stealthStrike ? 15 : 5;
            for (int i = 0; i < projAmt; i++)
            {
                int p = Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.UnitY.RotatedBy(MathHelper.Lerp(0, MathHelper.TwoPi, i / (float)projAmt)) * Main.rand.NextFloat(8f, 12f), ModContent.ProjectileType<LioBubble>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                Projectile proj = Main.projectile[p];
                proj.hostile = false;
                proj.friendly = true;
                proj.DamageType = ModContent.GetInstance<RogueDamageClass>();
                proj.scale = Projectile.Calamity().stealthStrike ? Main.rand.NextFloat(1.5f, 2.5f) : 1f;
            }
        }
    }
}