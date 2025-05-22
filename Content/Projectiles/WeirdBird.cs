using Terraria;
using CalamityMod.Dusts;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework;
using CalRemix.Content.Buffs;
namespace CalRemix.Content.Projectiles
{
    public class WeirdBird : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Microbial Cluster");
            Main.projPet[Projectile.type] = true;
            Main.projFrames[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 90000;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active)
            {
                Projectile.active = false;
                return;
            }
            if (!Main.player[Projectile.owner].dead && Main.player[Projectile.owner].HasBuff(ModContent.BuffType<WeirdBirdBuff>()))
                Projectile.timeLeft = 2;

            Projectile.FloatingPetAI(true, 0.02f);
            Projectile.spriteDirection = -Projectile.velocity.X.DirectionalSign();
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 4)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 3)
                Projectile.frame = 0;
        }
    }
}
