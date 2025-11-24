using CalamityMod;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class NowhereAura : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 200;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 420;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Summon;
        }

        public override void AI()
        {
            for (int i = 0; i < 5; i++)
            { 
                Vector2 spawnPos = Projectile.Center + Main.rand.NextVector2CircularEdge(Projectile.width / 2, Projectile.height / 2);
                GeneralParticleHandler.SpawnParticle(new SquishyLightParticle(spawnPos, spawnPos.DirectionTo(Projectile.Center) * Main.rand.NextFloat(4, 6), Main.rand.NextFloat(0.1f, 0.4f), Color.Magenta, 25));
            }
        }

        public override void OnKill(int timeLeft)
        {
            Main.player[Projectile.owner].AddCooldown("NowhereAura", CalamityUtils.SecondsToFrames(25));
        }
    }
}
