using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Bosses.Ocram
{
    public class OcramDustShadowflame : ModDust
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
            float DustScale = dust.scale;
            dust.position += dust.velocity;
            #endregion

            #region shadowflame exclusive code
            dust.velocity *= 0.94f;
            dust.scale += 0.002f;

            if (dust.noLight)
            {
                DustScale *= 0.1f;
                dust.scale -= 0.06f;
                if (dust.scale < 1f)
                {
                    dust.scale -= 0.06f;
                }

                if (Main.player[Main.myPlayer].wet)
                {
                    dust.position += Main.player[Main.myPlayer].velocity * 0.5f;
                }
                else
                {
                    dust.position += Main.player[Main.myPlayer].velocity;
                }
            }

            if (DustScale > 1f)
            {
                DustScale = 1f;
            }

            Lighting.AddLight(dust.position, new Vector3(DustScale * 0.6f, DustScale * 0.2f, DustScale));
            #endregion

            dust.velocity.X *= 0.99f;

            #region after y change
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

        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            float AlphaMultiplier = (255 - dust.color.A) / 255f;
            AlphaMultiplier = (AlphaMultiplier + 3f) * 0.25f;

            lightColor.R = (byte)(lightColor.R * AlphaMultiplier);
            lightColor.G = (byte)(lightColor.G * AlphaMultiplier);
            lightColor.B = (byte)(lightColor.B * AlphaMultiplier);
            lightColor.A += (byte)dust.color.A;

            return lightColor;
        }
    }
}
