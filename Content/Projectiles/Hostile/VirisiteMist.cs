using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class VirisiteMist : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 400;
            Projectile.hostile = true;
            Projectile.timeLeft = 240;
            Projectile.scale = 0.2f;
            Projectile.Opacity = 0;
        }
        public override void AI()
        {
            if (Projectile.ai[1] == 0)
            {
                Projectile.frame = Main.rand.Next(0, 3);
                Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
            }
            if (Projectile.ai[0] == 2)
            {
                Projectile.timeLeft -= 3;
                Projectile.velocity *= 0.97f;
            }
            Projectile.ai[1]++;
            Projectile.rotation += 0.02f;
            if (!Main.dedServ)
            {
                if (Main.rand.NextBool(10))
                {
                    //Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GreenMoss, 0f, 0f);
                }
            }
            if (Projectile.timeLeft > 60)
            {
                if (Projectile.scale < 2f)
                {
                    Projectile.scale += Projectile.ai[0] == 1 ? 0.1f : 0.05f;
                }
                else
                {
                    Projectile.scale = 2f;
                }
                if (Projectile.Opacity < 1)
                {
                    Projectile.Opacity += Projectile.ai[0] == 1 ? 0.4f : 0.2f;
                }
                else
                {
                    Projectile.Opacity = 1;
                }
            }
            else
            {
                Projectile.scale -= 0.05f;
                Projectile.Opacity -= 0.05f;
            }
        }
        public override void OnKill(int timeLeft)
        {
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;

            var shader = GameShaders.Misc["CalRemix:VirisiteMist"];
            shader.UseColor(new Color(0, 255, 123));
            shader.SetShaderTexture(ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/GreyscaleGradients/Perlin"));
            shader.Shader.Parameters["opacity"].SetValue(Projectile.Opacity);
            //shader.Apply();
            Main.spriteBatch.EnterShaderRegion(BlendState.Additive/*, shader.Shader*/);

            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Color.SeaGreen * Projectile.Opacity, Projectile.rotation, new Vector2(tex.Width / 2, tex.Height / 2), Projectile.scale + MathF.Sin(4f * Main.GlobalTimeWrappedHourly + Projectile.whoAmI) * 0.05f, 0);
            
            Main.spriteBatch.ExitShaderRegion();
            return false;
        }

        public override bool CanHitPlayer(Player target)
        {
            return Projectile.scale >= 1f || Projectile.ai[0] == 1;
        }
    }
}