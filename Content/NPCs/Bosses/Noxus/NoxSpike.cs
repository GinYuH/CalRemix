using CalamityMod;
using CalamityMod.DataStructures;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

using static System.MathF;
using static Terraria.Utils;
using static Microsoft.Xna.Framework.MathHelper;
using static CalRemix.CalRemixHelper;

namespace CalRemix.Content.NPCs.Bosses.Noxus
{
    public class NoxSpike : ModProjectile, IAdditiveDrawer
    {
        public ref float Index => ref Projectile.ai[0];

        public ref float Time => ref Projectile.ai[1];

        public static int TelegraphTime
        {
            get
            {
                if (CalamityWorld.revenge)
                    return 30;
                if (Main.expertMode)
                    return 33;

                return 36;
            }
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.MaxUpdates = 5;
            Projectile.timeLeft = Projectile.MaxUpdates * 300;
            CooldownSlot = ImmunityCooldownID.Bosses;
        }

        public override void AI()
        {
            // Accelerate over time.
            Projectile.velocity *= 1.012f;

            // Decide the current rotation.
            Projectile.rotation = Projectile.velocity.ToRotation() + PiOver2;

            if (Projectile.FinalExtraUpdate())
            {
                Time++;

                // Fire once the telegraphing is done.
                if (Time == TelegraphTime)
                {
                    Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.UnitY) * 4f;
                    Projectile.netUpdate = true;
                }
            }
        }

        public override bool? CanDamage() => Projectile.velocity.Length() >= 1f;

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<NoxusFumes>(), EntropicGod.DebuffDuration_RegularAttack);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Color drawColor = Color.White;
            drawColor.A = (byte)Remap(Projectile.velocity.Length(), 3f, 13f, 255f, 0f);
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], drawColor);

            // Draw a pulsating overlay if moving very slowly.
            float overlayOpacity = GetLerpValue(2.4f, 0.01f, Projectile.velocity.Length(), true) * 0.3f;
            float pulsationInterpolant = Cos(TwoPi * Index / 12f + Main.GlobalTimeWrappedHourly * 15f) * 0.5f + 0.5f;
            float scalePulsation = Lerp(1f, 1.3f, pulsationInterpolant);
            if (overlayOpacity > 0f)
            {
                Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
                Vector2 drawPosition = Projectile.Center - Main.screenPosition;
                Main.EntitySpriteDraw(texture, drawPosition, null, Projectile.GetAlpha(drawColor) * overlayOpacity, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale * scalePulsation, 0, 0);
            }

            return false;
        }

        public void AdditiveDraw(SpriteBatch spriteBatch)
        {
            // Draw a telegraph line.
            float bloomLineIntensity = Pow(Sin(Pi * GetLerpValue(0f, TelegraphTime, Time, true)), 0.4f);
            spriteBatch.DrawBloomLine(Projectile.Center, Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.UnitY) * 4000f, Color.HotPink * bloomLineIntensity, 18f);
        }
    }
}
