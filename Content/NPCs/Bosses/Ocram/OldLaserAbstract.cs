using CalRemix.UI.ElementalSystem;
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
    public abstract class OldLaserAbstract : ModProjectile
    {
        public virtual Vector3 laserLight => new Vector3(0, 0, 0);

        public override void AI()
        {
            if (Projectile.ai[1] == 0)
            {
                Projectile.ai[1] = 1;
                SoundEngine.PlaySound(SoundID.Item33, Projectile.position);
            }

            if (Projectile.alpha > 15)
            {
                Projectile.alpha -= 15;
            }
            else
            {
                Projectile.alpha = 0;
            }

            Projectile.rotation = (float)(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57);

            if (Projectile.velocity.Y > 16f)
            {
                Projectile.velocity.Y = 16f;
            }

            Lighting.AddLight(new Vector2(Projectile.position.X + (Projectile.width / 2), Projectile.position.Y + (Projectile.height / 2)), laserLight);
        }

        public override void OnKill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.alpha < 200)
            {
                return new Color(255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha, 0);
            }
            return default;
        }
    }
}
