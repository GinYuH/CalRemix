using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class MutantSeeker : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Seeker");
        }
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 180;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Dust d = Dust.NewDustPerfect(Projectile.position, DustID.Venom);
            d.noGravity = true;
            d.velocity = Vector2.Zero;
            Dust de = Dust.NewDustPerfect(Projectile.position, DustID.Blood);
            de.noGravity = true;
            de.velocity = Vector2.Zero;
            CalamityUtils.HomeInOnNPC(Projectile, true, 800, 14, 12);
        }
    }
}