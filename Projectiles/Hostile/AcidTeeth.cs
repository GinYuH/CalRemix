using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Dusts;
using CalamityMod.NPCs.OldDuke;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Hostile
{
    public class AcidTeeth : ModProjectile
    {
        public ref float Count => ref Projectile.ai[0];
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.hostile = true;
            Projectile.timeLeft = 240;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Projectile.velocity *= 0.997f;
            Projectile.rotation += MathHelper.ToRadians(12) * (Projectile.velocity.X > 0 ? 1 : -1);
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            for (int k = 0; k < 10; k++)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, (int)CalamityDusts.SulfurousSeaAcid, Scale: 1f + Main.rand.NextFloat());
                dust.noGravity = true;
                dust.velocity = new Vector2(Main.rand.Next(-20, 21), Main.rand.Next(-20, 21));
            }
            if (Count <= 0)
                Count = 3;
            for (int a = 0; a < Count; a++)
                Projectile.NewProjectileDirect(Projectile.GetSource_Death(), Projectile.Center, Vector2.One.RotatedByRandom(MathHelper.ToRadians(360f)) * 6f, ModContent.ProjectileType<AcidTooth>(), Projectile.damage, Projectile.knockBack);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<Irradiated>(), 120);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 centered = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            Main.EntitySpriteDraw(texture, centered, null, new Color(255, 255, 255), Projectile.rotation, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
