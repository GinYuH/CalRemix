using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader;
using System.IO;
using Microsoft.Build.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using CalamityMod.Graphics.Primitives;
using Terraria.Audio;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Projectiles.Ranged;
using CalamityMod;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class FallingChandelier : ModProjectile
    {
        public override string Texture => $"Terraria/Images/Tiles_{TileID.Chandeliers}";


        public override void SetDefaults()
        {
            Projectile.width = 54;
            Projectile.height = 20;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.hostile = true;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 300;
        }
        public override void AI()
        {
            Projectile.ai[1]++;
            if (Projectile.ai[1] > 30)
            {
                Projectile.velocity.Y += 0.2f;
                if (Projectile.velocity.Y > 14)
                    Projectile.velocity.Y = 14;
            }
        }

        public override bool CanHitPlayer(Player target)
        {
            return Projectile.velocity.Y > 0;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;

            Vector2 offset = Projectile.ai[1] < 30 ? Main.rand.NextVector2Circular(4, 4) : Vector2.Zero;
            for (int i = -1; i <= 1; i++)
            {
                for (int  j = -1; j <= 1; j++)
                {
                    Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition + new Vector2(i, j) * 16 + offset, new Rectangle(18 + 18 * i, (int)Projectile.ai[0] + 18 * j + 18, 16, 16), lightColor, Projectile.rotation, new Vector2(8, 8), Projectile.scale, 0);
                }
            }

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(BuzzkillSaw.TileCollideGFB, Projectile.Center);
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldCoin);
            }
            Main.LocalPlayer.Calamity().GeneralScreenShakePower = 10;
        }
    }
}
