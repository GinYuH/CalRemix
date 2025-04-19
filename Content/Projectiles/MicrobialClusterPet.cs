using Terraria;
using CalamityMod.Dusts;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework;
namespace CalRemix.Content.Projectiles
{
    public class MicrobialClusterPet : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Microbial Cluster");
            Main.projFrames[Projectile.type] = 1;
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft *= 5;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active)
            {
                Projectile.active = false;
                return;
            }
            CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();
            if (player.dead)
            {
                modPlayer.nothing = false;
            }
            if (modPlayer.nothing)
            {
                Projectile.timeLeft = 2;
            }
            Projectile.FloatingPetAI(true, 0.02f);
            Projectile.ai[0]++;
            if (Projectile.ai[0] % 32f == 31f)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, (int)CalamityDusts.SulphurousSeaAcid);
                dust.velocity = Vector2.One.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(1f, 2f);
                dust.noGravity = true;
                dust.scale = 1.6f;
            }
        }
    }
}
