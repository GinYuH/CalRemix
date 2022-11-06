using CalamityMod.CalPlayer;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Particles;

namespace CalRemix.Projectiles
{
    public class SlimeCore : ModProjectile
    {
        public Particle ring2;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Slime Core");
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.minionSlots = 1;
            Projectile.timeLeft = 18000;
            Projectile.penetrate = -1;
            Projectile.timeLeft *= 5;
            Projectile.minion = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Summon;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();
            bool flag64 = Projectile.type == ModContent.ProjectileType<SlimeCore>();
            if (flag64)
            {
                if (modPlayer.amalgel)
                {
                    Projectile.timeLeft = 2;
                }
            }
            Projectile.ai[0]++;
            if (Projectile.ai[0] > 120)
            {
                if (Projectile.ai[0] % 30 == 0) // Bolts
                {

                }
                if (Projectile.ai[0] % 600 == 0) // Deathray
                {

                }
                if (Projectile.ai[0] % 300 == 0) // Pulse
                {
                    ring2 = new BloomRing(Projectile.Center, Projectile.velocity, Color.Purple * 0.4f, 1.5f, 40);
                    GeneralParticleHandler.SpawnParticle(ring2);
                }
            }
            Projectile.rotation += Projectile.velocity.X * 0.04f;
            Projectile.ChargingMinionAI(2000f, 2400f, 3000f, 300f, 0, 30f, 12f, 8f, new Vector2(0f, -60f), 300f, 12.5f, true, true);
            if (ring2 != null)
            {
                ring2.Position = Projectile.Center;
                ring2.Velocity = Projectile.velocity;
                ring2.Scale *= 1.05f;
                ring2.Time += 1;
            }
        }
    }
}
