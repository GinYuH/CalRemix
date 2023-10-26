using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Weapons
{
	public class DemonCoreProj : ModProjectile
	{
        public override string Texture => "CalRemix/Items/Weapons/DemonCore";
        public override void SetStaticDefaults() 
        {
			DisplayName.SetDefault("Demon Core");
		}
        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 42;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
        }
        public override void OnKill(int timeLeft)
        {
            Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ProjectileID.BlackBolt, Projectile.damage, Projectile.knockBack / 2, Projectile.owner);
            proj.DamageType = ModContent.GetInstance<RogueDamageClass>();
            proj.Kill();
            if (!Projectile.Calamity().stealthStrike)
                return;
            for (int i = 0; i < 5; i++)
                Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.One.RotatedByRandom(MathHelper.ToRadians(360f)) * 3f, ModContent.ProjectileType<ShadowCloud>(), Projectile.damage / 10, Projectile.knockBack / 2, Projectile.owner);
        }
    }
}