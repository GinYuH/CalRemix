using CalamityMod;
using CalamityMod.Items.Weapons.Melee;
using CalRemix.Content.NPCs.Subworlds.Sealed;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class PinkSquareArea : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 3000;
        }
        public override void AI()
        {
            Projectile.width = Projectile.height = (int)Projectile.ai[0];
            if (VoidBoss.VoidIDX <= -1)
            {
                Projectile.Kill();
            }
            else
            {
                NPC n = Main.npc[VoidBoss.VoidIDX];
                if (!n.active || n.life <= 0)
                {
                    Projectile.Kill();
                }
            }
            if (Projectile.ai[1] > VoidBoss.AreaChargeUp + VoidBoss.AreaDuration)
            {
                Projectile.alpha = (int)MathHelper.Lerp(0, 255, Utils.GetLerpValue(VoidBoss.AreaChargeUp + VoidBoss.AreaDuration, VoidBoss.FullAreaTime, Projectile.ai[1]));
            }
            if (Projectile.ai[1] >= 310)
            {
                Projectile.Kill();
            }
            Projectile.ai[1]++;
            if (Projectile.ai[2] == 1 && Projectile.ai[1] == VoidBoss.AreaChargeUp)
            {
                SoundEngine.PlaySound(BetterSoundID.ItemTerraBeam with { Pitch = 2 }, Projectile.Center);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.MagicPixel.Value;

            int size = (int)Projectile.ai[0];
            int lineWidth = 2;
            int halfsize = (int)(size / 2f) - lineWidth;
            // Vertical lines
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition - Vector2.One * (halfsize + (lineWidth / 2)), new Rectangle(0, 0, lineWidth, size), Color.White * Projectile.Opacity, 0, new Vector2(lineWidth / 2, 0), 1, 0, 0);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + Vector2.One * (halfsize + (lineWidth / 2)), new Rectangle(0, 0, lineWidth, size), Color.White * Projectile.Opacity, 0, new Vector2(lineWidth / 2, size), 1, 0, 0);
            // Horizontal lines
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition - Vector2.One * (halfsize + (lineWidth / 2)), new Rectangle(0, 0, size, lineWidth), Color.White * Projectile.Opacity, 0, new Vector2(0, lineWidth / 2), 1, 0, 0);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + Vector2.One * (halfsize + (lineWidth / 2)), new Rectangle(0, 0, size, lineWidth), Color.White * Projectile.Opacity, 0, new Vector2(size, lineWidth / 2), 1, 0, 0);

            Texture2D square = TextureAssets.Projectile[ModContent.ProjectileType<PinkSquareHostile>()].Value;

            Main.spriteBatch.Draw(square, Projectile.Center - Main.screenPosition, null, Color.White * Projectile.Opacity, 0, square.Size() / 2, MathHelper.Lerp(square.Width, size, CalamityUtils.SineInEasing(Utils.GetLerpValue(VoidBoss.AreaChargeUp - 5, VoidBoss.AreaChargeUp, Projectile.ai[1], true), 1)) / square.Width, 0, 0);

            return false;
        }

        public override bool? CanDamage()
        {
            if (Projectile.ai[1] >= VoidBoss.AreaChargeUp && Projectile.alpha == 0)
            {
                return true;
            }
            return false;
        }
    }
}