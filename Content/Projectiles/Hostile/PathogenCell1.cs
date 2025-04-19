﻿using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class PathogenCell1 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Hostile Blood Cell");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.hostile = true;
            Projectile.timeLeft = 50;
            Projectile.tileCollide = false;
        }


        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Projectile.ai[1] == 0 ? ModContent.Request<Texture2D>("CalRemix/Content/Projectiles/Hostile/PathogenCell1").Value : ModContent.Request<Texture2D>("CalRemix/Content/Projectiles/Hostile/PathogenCell2").Value;
            CalamityUtils.DrawAfterimagesCentered(Projectile, 0, lightColor, 3, tex);
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            int projAmt = 3;
            int maxSplits = 3;
            if (Projectile.ai[0] < maxSplits)
            {
                SoundEngine.PlaySound(SoundID.NPCHit20, Projectile.Center);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                    for (int i = 0; i < projAmt; i++)
                {
                    int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.SafeNormalize(Vector2.UnitY).RotatedBy(MathHelper.Lerp(-MathHelper.PiOver4, MathHelper.PiOver4, (i + 1) / (float)projAmt)) * Projectile.velocity.Length() * 1.5f, Type, (int)MathHelper.Max((int)(Projectile.damage * 0.66f), 20), 0, ai0: Projectile.ai[0] + 1, ai1: Projectile.ai[1]);
                    Main.projectile[p].scale = Projectile.scale * 0.75f;
                }
            }
            for (int i = 0; i < 4; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, 0f, 0f, 100, default, 2f);
                Main.dust[dust].velocity *= 3f;
                if (Main.rand.NextBool())
                {
                    Main.dust[dust].scale = 0.5f;
                    Main.dust[dust].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
            }
        }
    }
}