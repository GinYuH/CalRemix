using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalRemix.Content.DamageClasses;
using CalamityMod.Sounds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CalRemix.Content.Projectiles.Weapons.Stormbow
{
    public class NanomachinesSon : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nanomachine's Son");
            Main.projFrames[Type] = 5;
        }
        public override void SetDefaults()
        {
            Projectile.width = 86;
            Projectile.height = 1080;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 18000;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 1800;
            Projectile.DamageType = ModContent.GetInstance<StormbowDamageClass>();
            Projectile.aiStyle = -1;
        }

        public override void AI()
        {
            
            if (Projectile.frameCounter > Main.projFrames[Type])
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= Main.projFrames[Type])
            {
                Projectile.Kill();
            }
            Projectile.frameCounter++;

            if (Projectile.frame == 1)
            {
                SoundEngine.PlaySound(CommonCalamitySounds.ExoDeathSound, Projectile.Center);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(tex, Projectile.position - Main.screenPosition, tex.Frame(1, Main.projFrames[Type], 0, Projectile.frame), Color.White, Projectile.rotation, new Vector2(tex.Width / 2, tex.Height / 2 / Main.projFrames[Type]), Projectile.scale, SpriteEffects.None, 0);

            return false;
        }

        //public override Color? GetAlpha(Color lightColor) => new Color(100, 0, 0, 0);
    }
}
