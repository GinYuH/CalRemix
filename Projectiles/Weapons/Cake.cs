using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Weapons
{
	public class Cake : ModProjectile
	{
        public override string Texture => "Terraria/Images/Item_3750";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cake");
        }
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = ProjAIStyleID.Arrow;
            Projectile.DamageType = DamageClass.Ranged;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 5; i++)
                if (Main.player[Projectile.owner].ownedProjectileCounts[ModContent.ProjectileType<Frosting>()] <= 40)
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, -Vector2.Normalize(Projectile.oldVelocity).RotatedByRandom(MathHelper.ToRadians(180)) * 4f, ModContent.ProjectileType<Frosting>(), Projectile.damage, Projectile.knockBack, Projectile.owner);  
        }
    }
}