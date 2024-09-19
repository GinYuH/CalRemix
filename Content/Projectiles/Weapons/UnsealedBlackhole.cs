using CalamityMod;
using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
	public class UnsealedBlackhole : ModProjectile
	{
        public override string Texture => "CalamityMod/Projectiles/Rogue/SealedSingularityBlackhole";
        public override void SetStaticDefaults() 
        {
			DisplayName.SetDefault("Unsealed Singularity");
            Main.projFrames[Projectile.type] = 7;
        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ModContent.ProjectileType<SealedSingularityBlackhole>());
            AIType = ModContent.ProjectileType<SealedSingularityBlackhole>();
            Projectile.timeLeft = 300;
        }
        public override void OnSpawn(IEntitySource source)
        {
            if (Projectile.Calamity().stealthStrike)
            {
                Projectile.scale = 5f;
                Projectile.width = (int)(Projectile.width * Projectile.scale);
                Projectile.height = (int)(Projectile.height * Projectile.scale);
            }
        }

        public override void AI()
        {
            int frameGate = 6;

            Projectile.frameCounter++;
            if (Projectile.frameCounter > frameGate)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= 7)
            {
                Projectile.frame = 0;
            }

            Projectile.localAI[1]--;
            if (Projectile.Calamity().stealthStrike)
            {
                if (Projectile.localAI[1] <= 0)
                {
                    float startDist = Main.rand.NextFloat(2000f, 2257f);
                    Vector2 startDir = Main.rand.NextVector2Unit();
                    Vector2 startPoint = Projectile.Center + (startDir * startDist);

                    float randSpeed = Main.rand.NextFloat(32f, 65f);
                    Vector2 velocity = startDir * (-randSpeed);
                    if (Projectile.owner == Main.myPlayer)
                    {
                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), startPoint, velocity, ModContent.ProjectileType<PinkSquare>(), Projectile.damage / 4, 0, Projectile.owner, Projectile.whoAmI);
                    }
                    Projectile.localAI[1] = 10;
                }
            }
            else
            {
                if (Projectile.localAI[1] <= 0)
                {
                    float startDist = Main.rand.NextFloat(2000f, 2257f);
                    Vector2 startDir = Main.rand.NextVector2Unit();
                    Vector2 startPoint = Projectile.Center + (startDir * startDist);

                    float randSpeed = Main.rand.NextFloat(32f, 65f);
                    Vector2 velocity = startDir * (-randSpeed);
                    if (Projectile.owner == Main.myPlayer)
                    {
                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), startPoint, velocity, ModContent.ProjectileType<PinkSquare>(), Projectile.damage / 4, 0, Projectile.owner, Projectile.whoAmI);
                    }
                    Projectile.localAI[1] = 40;
                }
            }
        }
    }
}