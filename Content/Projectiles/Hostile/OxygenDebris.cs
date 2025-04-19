using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class OxygenDebris : ModProjectile
    {
        public override string Texture => "CalRemix/Content/Projectiles/Hostile/OxygenDebris1";

        public Vector2 pivot = new Vector2(0, 0);
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Debris");
        }
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.hostile = true;
            Projectile.timeLeft = 240;
            Projectile.tileCollide = false;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
            writer.Write(Projectile.localAI[1]);
            writer.Write(Projectile.localAI[2]);
            writer.Write(pivot.X);
            writer.Write(pivot.Y);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[0] = reader.ReadSingle();
            Projectile.localAI[1] = reader.ReadSingle();
            Projectile.localAI[2] = reader.ReadSingle();
            pivot.X = reader.ReadSingle();
            pivot.Y = reader.ReadSingle();
        }

        public override void AI()
        {
            Projectile.rotation += Projectile.direction * 0.5f;
            if (pivot == Vector2.Zero)
            {
                pivot = Projectile.Center;
            }
            else
            {
                Projectile.localAI[2] += Projectile.localAI[1] == 1 ? -1f : 1f;
                float distance = Projectile.localAI[2] * 9;
                double deg = 360 / Projectile.ai[1] * Projectile.ai[0] + Projectile.localAI[2] * 0.5f;
                double rad = deg * (Math.PI / 180);
                float hyposx = pivot.X - (int)(Math.Cos(rad) * distance) - Projectile.width / 2;
                float hyposy = pivot.Y - (int)(Math.Sin(rad) * distance) - Projectile.height / 2;

                Projectile.position = new Microsoft.Xna.Framework.Vector2(hyposx, hyposy);
            }

        }

        public override void OnKill(int timeLeft)
        {
            int padding = 0;
            for (int i = 0; i < 20; i++)
            {
                Dust.NewDust(Projectile.position - new Vector2(padding, padding), Projectile.width + padding, Projectile.height + padding, DustID.Torch, Scale: Main.rand.NextFloat(2, 4));
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.localAI[0] == 0)
                return false;
            Texture2D texture = ModContent.Request<Texture2D>("CalRemix/Content/Projectiles/Hostile/OxygenDebris" + Projectile.localAI[0]).Value;
            Vector2 centered = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            //Main.EntitySpriteDraw(texture, centered, null, Projectile.GetAlpha(lightColor), Projectile.rotation, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            CalamityUtils.DrawProjectileWithBackglow(Projectile, Color.White * 0.4f, Color.White, 4, texture);
            return false;
        }
    }
}