using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
	public class Bone : ModProjectile
	{
        public override string Texture => "Terraria/Images/Item_154";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bone");
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = ProjAIStyleID.GroundProjectile;
        }
        public override void OnKill(int timeLeft)
        {
            Dust dust = Dust.NewDustDirect(Projectile.Center, 1, 1, DustID.Bone);
            dust.velocity = Vector2.One.RotatedByRandom(MathHelper.ToRadians(360f)) * Main.rand.NextFloat(1f, 1.5f);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.ai[1] == 1)
                Projectile.Kill();
            return true;
        }
    }
}