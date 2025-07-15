using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class TentDebris : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 6;
        }
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
        }
        public override void AI()
        {
            if (Projectile.ai[0] == 0)
            {
                Projectile.frame = Main.rand.Next(0, 6);
                Projectile.ai[0] = Main.rand.Next(1, 3);
            }

            Projectile.rotation += (Projectile.ai[0] == 2).ToDirectionInt() * 0.2f;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<ArmorCrunch>(), 120);
        }

        public override void OnKill(int timeLeft)
        {
            CalRemixHelper.DustExplosionOutward(Projectile.Center, dustID: DustID.DungeonGreen, speed: Main.rand.NextFloat(4, 10), amount: 30, alpha: 0, scale: 1);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 centered = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            Main.spriteBatch.Draw(texture, centered, texture.Frame(1, 6, 0, Projectile.frame), lightColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 12), Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}