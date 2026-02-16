using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace CalRemix.Content.NPCs.Bosses.BossChanges.SupremeCalamitas
{
    public class SupremeSkull : ModProjectile
    {
        public ref float IdealPosX => ref Projectile.ai[0];
        public ref float IdealPosY => ref Projectile.ai[1];
        public ref float Timer => ref Projectile.ai[2];
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 34;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 720;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();

            Vector2 projSpeedCompensation = Vector2.Normalize(Projectile.velocity) * 10;
            Vector2 dustVelocityBase = Vector2.Normalize(Projectile.velocity * -1);

            // matte layer
            for (int i = 0; i < 4; i++)
            {
                Vector2 dustVelocityNoisePass = new Vector2(Main.rand.NextFloat(-1, 2) + dustVelocityBase.X, Main.rand.NextFloat(-1, 2) + dustVelocityBase.Y);
                Dust.NewDustPerfect(Projectile.Center + projSpeedCompensation, ModContent.DustType<BrimstoneFireDustMatte>(), dustVelocityNoisePass);
            }

            // flare layer
            for (int i = 0; i < 2; i++)
            {
                Vector2 dustVelocityNoisePass2 = new Vector2(Main.rand.NextFloat(-1, 2) + dustVelocityBase.X, Main.rand.NextFloat(-1, 2) + dustVelocityBase.Y);
                Dust.NewDustPerfect(Projectile.Center + projSpeedCompensation, ModContent.DustType<BrimstoneFireDustFlare>(), dustVelocityNoisePass2);
            }

            // from brimstone hellblast
            Lighting.AddLight(Projectile.Center, 0.9f * Projectile.Opacity, 0f, 0f);

            Timer++;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<VulnerabilityHex>(), 120);
        }
        public override void OnKill(int timeLeft)
        {
            // mattesplosion
            for (int i = 0; i < 24; i++)
            {
                Vector2 dustVelocity = new Vector2(Main.rand.NextFloat(-5, 5), Main.rand.NextFloat(-5, 5));
                Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<BrimstoneFireDustMatte>(), dustVelocity);
            }
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            // yeahhhhhhhhhhhhhh
            overWiresUI.Add(index);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            SpriteEffects flip = SpriteEffects.None;
            if (Projectile.velocity.X < 0)
                flip = SpriteEffects.FlipVertically;
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Size() * 0.5f, Projectile.scale, flip);
            return false;
        }
    }
}
