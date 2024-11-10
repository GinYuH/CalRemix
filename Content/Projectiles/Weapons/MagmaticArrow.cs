using CalamityMod.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class MagmaticArrow : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/Ranged/DrataliornusFlame";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magmatic Arrow");
        }
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.aiStyle = ProjAIStyleID.Arrow;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 5;
        }
        public override void AI()
        {
            Projectile.ai[2]++;
            if (Main.rand.NextBool(10))
            {
                int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<RancorFog>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                Main.projectile[p].DamageType = DamageClass.Ranged;
                Main.projectile[p].penetrate = -1;
            }
        }

        public override void PostAI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
    }
}