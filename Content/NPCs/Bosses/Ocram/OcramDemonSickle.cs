using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Bosses.Ocram
{
    public class OcramDemonSickle : ModProjectile
    {
        private const float LIGHT_LEVEL = 0.2f;
        private Vector3 laserLight => new Vector3(1, LIGHT_LEVEL * 0.6f, LIGHT_LEVEL * 0.1f);

        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.alpha = 100;
            //Projectile.light = 0.2f;
            //Projectile.aiStyle = 18;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.scale = 0.9f;
            Projectile.friendly = false;
        }

        public override void AI()
        {
            if (Projectile.ai[1] == 0)
            {
                Projectile.ai[1] = 1;
                SoundEngine.PlaySound(SoundID.Item8, Projectile.position);
            }

            Projectile.rotation += Projectile.direction * 0.8f;

            if (!((Projectile.ai[0] += 1f) < 30f))
            {
                if (Projectile.ai[0] < 100f)
                {
                    Projectile.velocity.X *= 1.06f;
                    Projectile.velocity.Y *= 1.06f;
                }
                else
                {
                    Projectile.ai[0] = 200f;
                }
            }

            for (int i = 0; i < 2; i++)
            {
                Dust ptr = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<OcramDustShadowflame>(), 0, 0, 100);
                if (ptr == null)
                {
                    break;
                }
                ptr.noGravity = true;
            }

            Lighting.AddLight(new Vector2(Projectile.position.X + (Projectile.width / 2), Projectile.position.Y + (Projectile.height / 2)), laserLight);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            for (int num42 = 0; num42 < 18; num42++)
            {
                Dust ptr17 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<OcramDustShadowflame>(), Projectile.velocity.X, Projectile.velocity.Y, 100, default, 1.7f);
                if (ptr17 == null)
                {
                    break;
                }
                ptr17.noGravity = true;

                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<OcramDustShadowflame>(), Projectile.velocity.X, Projectile.velocity.Y, 100);
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}
