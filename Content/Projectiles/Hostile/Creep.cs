using CalamityMod;
using CalamityMod.Graphics.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class Creep : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Creep");
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.GreekFire1);
            Projectile.aiStyle = -1;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            if (Projectile.ai[1] == 0)
            {
                Projectile.ai[0] += 1f;
                if (Projectile.ai[0] > 5f)
                {
                    Projectile.ai[0] = 5f;
                    if (Projectile.velocity.Y == 0f && Projectile.velocity.X != 0f)
                    {
                        Projectile.velocity.X *= 0.97f;
                        if (Projectile.velocity.X > -0.01 && Projectile.velocity.X < 0.01)
                        {
                            Projectile.velocity.X = 0f;
                            Projectile.netUpdate = true;
                        }
                    }
                    Projectile.velocity.Y += 0.2f;
                }
                Projectile.rotation += Projectile.velocity.X * 0.1f;
                if ((double)Projectile.velocity.Y < 0.25 && (double)Projectile.velocity.Y > 0.15)
                {
                    Projectile.velocity.X *= 0.8f;
                }
                Projectile.rotation = (0f - Projectile.velocity.X) * 0.05f;
                if (Projectile.velocity.Y > 16f)
                {
                    Projectile.velocity.Y = 16f;
                }
            }
            Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GreenBlood, 0f, 0f, Scale: 2);
            d.velocity = new Vector2(Main.rand.Next(-1, 2), Main.rand.Next(-1, 2));
            d.noGravity = true;
        }

        public override void OnKill(int timeLeft)
        {
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = Vector2.Zero;
            return false;
        }
    }
}