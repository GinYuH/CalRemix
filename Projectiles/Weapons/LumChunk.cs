using Terraria;
using Terraria.ModLoader;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Projectiles.Rogue;
using CalamityMod.Items.Materials;

namespace CalRemix.Projectiles.Weapons
{
    public class LumChunk : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/Rogue/AbyssalMirrorProjectile";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lumenyl Shard");
            Main.projFrames[Projectile.type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ModContent.ProjectileType<AbyssalMirrorProjectile>());
            Projectile.tileCollide = true;
            Projectile.timeLeft = 600;
            AIType = ModContent.ProjectileType<AbyssalMirrorProjectile>();
            Projectile.frame = Main.rand.Next(3);
        }
    }
}
