using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
	public class Cake : ModProjectile
	{
        public override string Texture => "Terraria/Images/Item_3750";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Cake");
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
            SoundEngine.PlaySound(SoundID.NPCDeath21 with { MaxInstances = 1 }, Projectile.Center);
            if (Main.player[Projectile.owner].ownedProjectileCounts[ModContent.ProjectileType<Frosting>()] <= 40)
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + new Vector2(0, (Projectile.oldVelocity.Y < 0) ? 8f : -8f), Vector2.Zero, ModContent.ProjectileType<Frosting>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            else
                return;
            for (int i = 0; i < 7; i++)
            {
                if (Main.player[Projectile.owner].ownedProjectileCounts[ModContent.ProjectileType<Frosting>()] <= 40)
                {
                    Vector2 speed = new(0, (Projectile.oldVelocity.Y < 0) ? 10 : -10);
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + new Vector2(0, (Projectile.oldVelocity.Y < 0) ? 8f : -8f), speed.RotatedByRandom(MathHelper.ToRadians(135f)), ModContent.ProjectileType<Frosting>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                }
                else
                    break;
            }
        }
    }
}