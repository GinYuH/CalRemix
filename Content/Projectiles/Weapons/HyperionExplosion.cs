using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.Audio;

namespace CalRemix.Content.Projectiles.Weapons
{
	public class HyperionExplosion : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public Player Owner => Main.player[Projectile.owner];

        public override void SetDefaults()
        {
            Projectile.width = 120;
            Projectile.height = 120;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        public override void OnSpawn(IEntitySource source)
        {
            SoundEngine.PlaySound(SoundID.Item62);
        }
        public override void AI()
        {
            for (int i = 0; i < 25; i++)
            {
                Vector2 v = new(Main.rand.Next(-30, 31), Main.rand.Next(-30, 31)); 
                float v3 = Main.rand.Next(9, 27);
                float m = v.Length();
                m = v3 / m;
                v *= m;
                Vector2 pos = Projectile.Center + Vector2.UnitX.RotatedByRandom(MathHelper.ToRadians(360)) * Main.rand.Next(-40, 40);
                Dust dust = Dust.NewDustDirect(pos, 1, 1, DustID.Pixie, 0f, 0f, 100, default, 2.5f);
                dust.noGravity = true;
                dust.velocity = v;
            }
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => CalamityUtils.CircularHitboxCollision(Projectile.Center, 160, targetHitbox);
    }
}