using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class PungentBomb : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.timeLeft = 240;
            Projectile.alpha = 220;
            Projectile.DamageType = DamageClass.Melee;
        }
        public override void AI()
        {
            Projectile.velocity *= 0.997f;
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.scale += 0.02f;
                if (Projectile.scale >= 1.2f)
                    Projectile.localAI[0] = 1f;
            }
            else if (Projectile.localAI[0] == 1f)
            {
                Projectile.scale -= 0.02f;
                if (Projectile.scale <= 0.8f)
                    Projectile.localAI[0] = 0f;
            }
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item112, Projectile.Center);
            for (int a = 0; a < 4; a++)
                Projectile.NewProjectileDirect(Projectile.GetSource_Death(), Projectile.Center, (-Vector2.UnitY).RotatedByRandom(MathHelper.ToRadians(45f)) * 6f, ModContent.ProjectileType<PungentShot>(), Projectile.damage, Projectile.knockBack);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Irradiated>(), 120);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 centered = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            Main.EntitySpriteDraw(texture, centered, null, new Color(255, 255, 255, Projectile.alpha), 0, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
