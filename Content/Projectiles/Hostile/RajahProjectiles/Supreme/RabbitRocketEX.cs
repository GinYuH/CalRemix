using Terraria.Audio;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using CalRemix.Content.Projectiles.Hostile.RajahProjectiles;

namespace CalRemix.Content.Projectiles.Hostile.RajahProjectiles.Supreme

{
    public class RabbitRocketEX : RabbitRocket3
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Rajah Rocket");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(0, 0), Terraria.ModLoader.ModContent.ProjectileType<RabbitBoomEX>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            Main.projectile[p].DamageType = DamageClass.Ranged;
            Main.projectile[p].Center = Projectile.Center;
            float spread = 12f * 0.0174f;
            double startAngle = Math.Atan2(Projectile.velocity.X, Projectile.velocity.Y) - spread / 2;
            double deltaAngle = spread / 6;
            for (int i = 0; i < 3; i++)
            {
                double offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(Math.Sin(offsetAngle) * 3f) * 5, (float)(Math.Cos(offsetAngle) * 3f) * 5, Mod.Find<ModProjectile>("CarrotEX").Type, Projectile.damage / 6, Projectile.knockBack, Projectile.owner, 0f, 0f);
                Main.projectile[proj].DamageType = DamageClass.Ranged;
                 proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(-Math.Sin(offsetAngle) * 3f) * 5, (float)(-Math.Cos(offsetAngle) * 3f) * 5, Mod.Find<ModProjectile>("CarrotEX").Type, Projectile.damage / 6, Projectile.knockBack, Projectile.owner, 0f, 0f);
                Main.projectile[proj].DamageType = DamageClass.Ranged;
            }
        }
    }
}
