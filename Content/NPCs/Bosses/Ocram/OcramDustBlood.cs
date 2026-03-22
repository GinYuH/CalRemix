using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Bosses.Ocram
{
    public class OcramDustBlood : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.active = true;
            dust.noGravity = false;
            dust.noLight = false;
            dust.fadeIn = 0f;
            dust.rotation = 0f;
            dust.scale = (float)(dust.scale + Main.rand.Next(-20, 21) * 0.01 * dust.scale);
            dust.frame.Y = 10 * Main.rand.Next(3);
            dust.frame.Width = 8;
            dust.frame.Height = 8;
        }

        public override bool Update(Dust dust)
        {
            #region top of the method for all dust
            dust.position += dust.velocity;
            #endregion

            if (!dust.noGravity)
            {
                dust.velocity.Y += 0.1f;
            }

            dust.velocity.X *= 0.99f;

            #region "after y change"
            if (dust.fadeIn > 0f)
            {
                dust.scale += 0.03f;

                if (dust.scale > dust.fadeIn)
                {
                    dust.fadeIn = 0f;
                }
            }
            else
            {
                dust.scale -= 0.01f;
            }

            if (dust.noGravity)
            {
                dust.velocity *= 0.92f;
                if (dust.fadeIn == 0f)
                {
                    dust.scale -= 0.04f;
                }
            }

            if (dust.scale < 0.1f)
            {
                dust.active = false;
            }
            else
            {
                dust.rotation += dust.velocity.X * 0.5f;
            }
            #endregion

            return false;
        }
    }
}
