using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Weapons
{
	public class TendonTidesDisc : ModProjectile
    {
        public override string Texture => "CalRemix/Items/Weapons/TendonTides";
        public Player Owner => Main.player[Projectile.owner];
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tendon Tides");
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.PaladinsHammerFriendly);
            Projectile.width = 110;
            Projectile.height = 110;
            Projectile.friendly = true;
            Projectile.penetrate = 10;
            Projectile.timeLeft = 9999;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.NextBool(10))
                target.AddBuff(ModContent.BuffType<Shred>(), 360);
        }
        public override void Kill(int timeLeft)
        {
            if (Projectile.penetrate <= 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    int gore = Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.One.RotatedByRandom(MathHelper.ToRadians(360)), 353);
                    Main.gore[gore].timeLeft = 80;
                }
            }
        }
    }
}


