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
    public class VirisiteDrop : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.hostile = true;
            Projectile.timeLeft = 1200;
            Projectile.Opacity = 0;
            Projectile.ignoreWater = true;
        }
        public override void AI()
        {
            if (Projectile.ai[1] == 0)
            {
                Projectile.frame = Main.rand.Next(0, 2);
            }
            if (Projectile.ai[1] > 60 && Projectile.velocity.Y == 0 && Projectile.frame <= 1)
            {
                SoundEngine.PlaySound(SoundID.NPCDeath21 with { Pitch = -0.3f, Volume = 0.8f, MaxInstances = 8 }, Projectile.position);
                Projectile.frame += 2;
            }
            Projectile.ai[1]++;
            if (Projectile.ai[1] > 30)
            {
                Projectile.velocity.Y += 0.1f;
                if (Projectile.velocity.Y > 10f)
                {
                    Projectile.velocity.Y = 10f;
                }
            }
            Projectile.Opacity += 0.05f;
            if (Projectile.velocity.Y < 0 && Projectile.Opacity > 0.4f)
                Projectile.Opacity = 0.8f;

            if (Projectile.frame >= 2)
            {
                Projectile.ai[2]++;
                if (Projectile.ai[2] > 60)
                {
                    Projectile.Kill();
                }
                Projectile.rotation = 0;
                Projectile.velocity.X = 0;
            }
            else
            {
                Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
            }
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = false;
            return true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void OnKill(int timeLeft)
        {
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Main.spriteBatch.EnterShaderRegion(BlendState.Additive);
            Vector2 scale = new Vector2(Projectile.scale);
            if (Projectile.frame >= 2)
            {
                scale += new Vector2(MathF.Sin(4f * Main.GlobalTimeWrappedHourly + Projectile.whoAmI) * 0.2f, MathF.Cos(4f * Main.GlobalTimeWrappedHourly + Projectile.whoAmI) * 0.2f);
            }
            Main.EntitySpriteDraw(tex, Projectile.Bottom - Main.screenPosition + Vector2.UnitY * 8, tex.Frame(4, 1, Projectile.frame, 0), Color.White * Projectile.Opacity, Projectile.rotation, new Vector2(tex.Width / 8, tex.Height), scale, 0);
            Main.spriteBatch.ExitShaderRegion();
            return false;
        }

        public override bool CanHitPlayer(Player target)
        {
            return Projectile.velocity.Y >= 0;
        }
    }
}