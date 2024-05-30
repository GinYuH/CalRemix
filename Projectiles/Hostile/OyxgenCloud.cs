using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Hostile
{
    public class OxygenCloud : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cloud");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.hostile = true;
            Projectile.timeLeft = 480;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Cloud, 1);
        }

        public override void OnKill(int timeLeft)
        {
            int padding = 0;
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(Projectile.position - new Vector2(padding, padding), Projectile.width + padding, Projectile.height + padding, DustID.Cloud, Scale: Main.rand.NextFloat(2, 4));
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, 1, lightColor, 4, TextureAssets.Cloud[(int)Projectile.ai[0]].Value);
            return false; 
        }
    }
}