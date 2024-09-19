using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
	public class Coal : ModProjectile
	{
        public override string Texture => "Terraria/Images/Item_1922";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Coal");
        }
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.timeLeft = 210;
            Projectile.aiStyle = ProjAIStyleID.GroundProjectile;
        }
        public override void AI()
        {
            Projectile.ai[0]++;
            if (Projectile.ai[0] % 60 * (Projectile.velocity.Length() / 16) == 0 && Projectile.Calamity().stealthStrike)
            {
                Projectile.damage++;
                Projectile.scale *= 1.05f;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return true;
        }
        public override void OnKill(int timeLeft)
        {
            Dust dust = Dust.NewDustDirect(Projectile.Center, 1, 1, DustID.Obsidian);
            dust.velocity = Vector2.One.RotatedByRandom(MathHelper.ToRadians(360f)) * Main.rand.NextFloat(1f, 1.5f);
        }
    }
}