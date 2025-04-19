using CalamityMod;
using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
	public class UnsealedBomb : ModProjectile
	{
        public override string Texture => "CalamityMod/Items/Weapons/Rogue/SealedSingularity";
        public override void SetStaticDefaults() 
        {
			// DisplayName.SetDefault("Unsealed Singularity");
		}
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ModContent.ProjectileType<SealedSingularityProj>());
            AIType = ModContent.ProjectileType<SealedSingularityProj>();
        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                SoundEngine.PlaySound(in SoundID.Shatter, Projectile.position);
                int num = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<UnsealedBlackhole>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.Calamity().stealthStrike ? (-180f) : 0f);
                if (num.WithinBounds(1000))
                    Main.projectile[num].Calamity().stealthStrike = Projectile.Calamity().stealthStrike;

                for (int i = 0; i < 3; i++)
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.One.RotatedByRandom(MathHelper.ToRadians(360)), ModContent.ProjectileType<SealedSingularityGore>(), (int)((double)Projectile.damage * 0.25), 0f, Projectile.owner, i);
            }
        }
    }
}