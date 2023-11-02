using CalamityMod;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Weapons
{
    public class Frosting : ModProjectile
    {
        public ref float Alpha => ref Projectile.localAI[0];
        public ref float Timer => ref Projectile.localAI[1];
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frosting");
            Main.projFrames[Type] = 6;
        }
        public override void SetDefaults()
        {
            Projectile.width = 52;
            Projectile.height = 56;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.penetrate = -1;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Alpha);
            writer.Write(Timer);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Alpha = reader.ReadSingle();
            Timer = reader.ReadSingle();
        }
        public override void AI()
        {
            Timer += 1f;
            if (Timer > 600f)
            {
                Alpha += 10f;
                Projectile.damage = 0;
            }
            if (Alpha > 255f)
            {
                Projectile.Kill();
                Alpha = 255f;
            }
            Projectile.alpha = (int)(100.0 + (double)Alpha * 0.7);
            if (Projectile.velocity.Y != 0f)
            {
                Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) - MathF.PI / 2f;
                Projectile.frameCounter++;
                if (Projectile.frameCounter > 6)
                {
                    Projectile.frame++;
                    Projectile.frameCounter = 0;
                }
                if (Projectile.frame > 1)
                    Projectile.frame = 0;
            }
            else
            {
                Projectile.velocity.X = 0f;
                if (Projectile.frame < 2)
                {
                    Projectile.frame = 2;
                    Projectile.frameCounter = 0;
                    SoundEngine.PlaySound(in SoundID.NPCDeath21, Projectile.Center);
                }
                Projectile.rotation = 0f;
                Projectile.gfxOffY = 4f;
                Projectile.frameCounter++;
                if (Projectile.frameCounter > 6)
                {
                    Projectile.frame++;
                    Projectile.frameCounter = 0;
                }
                if (Projectile.frame > 5)
                    Projectile.frame = 5;
            }

            Projectile.velocity.X *= 0.995f;
            if (Projectile.wet || Projectile.lavaWet)
            {
                Projectile.velocity.Y = 0f;
                return;
            }
            Projectile.velocity.Y += 0.15f;
            if (Projectile.velocity.Y > 8f)
                Projectile.velocity.Y = 8f;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return CalamityUtils.CircularHitboxCollision(Projectile.Center, 16f, targetHitbox);
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
        public override Color? GetAlpha(Color lightColor)
        {
            if (Timer > 900f)
            {
                byte b = (byte)((26f - (Timer - 900f)) * 10f);
                byte alpha = (byte)((float)Projectile.alpha * ((float)(int)b / 255f));
                return new Color(b, b, b, alpha);
            }

            return new Color(255, 255, 255, Projectile.alpha);
        }
    }
}
