using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class ObsidianBomb : ModProjectile
    {
        public ref float Count => ref Projectile.ai[0];
        public override void SetDefaults()
        {
            Projectile.width = 172;
            Projectile.height = 172;
            Projectile.hostile = true;
            Projectile.timeLeft = 180;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.X.DirectionalSign() * 0.3f;
            Projectile.velocity *= 0.99f;
        }
        public override void OnKill(int timeLeft)
        {
            Main.LocalPlayer.Calamity().GeneralScreenShakePower = 100;
            SoundEngine.PlaySound(BetterSoundID.ItemExplosiveShotgun, Projectile.Center);
            SoundEngine.PlaySound(BetterSoundID.ItemMeteorImpact, Projectile.Center);
            ProjCircle(18, 0);
            if (Main.expertMode)
                ProjCircle(14, 0.67f * Count);
            if (CalamityWorld.revenge)
                ProjCircle(10, 0.33f * Count);
            for (int i = 0; i < 300; i++)
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Obsidian, Scale: Main.rand.NextFloat(1, 5));
                Main.dust[d].velocity = (Main.dust[d].position - Projectile.Center).SafeNormalize(Vector2.UnitY) * Main.rand.NextFloat(4, 22);
            }
        }

        public void ProjCircle(float speed, float rotoff)
        {
            for (int a = 0; a < Count; a++)
                Projectile.NewProjectileDirect(Projectile.GetSource_Death(), Projectile.Center, Vector2.UnitY.RotatedBy(MathHelper.Lerp(0, MathHelper.TwoPi, a / (float)Count)).RotatedBy(rotoff) * speed, ModContent.ProjectileType<ObsidianFragment>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, Main.rand.Next(0, 6), 1);
        }


        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 centered = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            Main.EntitySpriteDraw(texture, centered, null, lightColor, 0, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
