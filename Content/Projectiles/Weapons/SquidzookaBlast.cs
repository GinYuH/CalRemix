using CalamityMod;
using CalamityMod.Graphics.Primitives;
using CalRemix.Content.NPCs.Subworlds.GreatSea;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class SquidzookaBlast : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 11;
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 240;
            Projectile.tileCollide = true;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
        public override void OnKill(int timeLeft)
        {
            int p = Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<AttackSquid>(), Projectile.damage, Projectile.knockBack, Projectile.owner);

            SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Projectile.Center);
            SoundEngine.PlaySound(Stanchor.DeathSound, Projectile.Center);
            Projectile.ExpandHitboxBy(128);
            Projectile.maxPenetrate = -1;
            Projectile.penetrate = -1;
            Projectile.Damage();

            for (int i = 0; i < 80; i++)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Obsidian, new Vector2(Projectile.velocity.X, Projectile.velocity.Y), Alpha: 50, Color.Black, Scale: Main.rand.NextFloat(1, 2));
                dust.noGravity = true;
                dust.position += Main.rand.NextVector2Square(-5, 5);
                dust.velocity = Projectile.Center.DirectionTo(dust.position) * Main.rand.Next(3, 14);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 centered = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);

            Vector2 trailOffset = Projectile.Size * 0.5f;
            trailOffset += (Projectile.rotation + MathHelper.PiOver2).ToRotationVector2();
            PrimitiveRenderer.RenderTrail(Projectile.oldPos, new(FlameTrailWidthFunction, FlameTrailColorFunction, (_) => trailOffset), 61);
            Main.EntitySpriteDraw(texture, centered, null, Projectile.GetAlpha(lightColor), Projectile.rotation, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);

            return false;
        }
        public float FlameTrailWidthFunction(float completionRatio) => MathHelper.SmoothStep(12f * Projectile.scale, 6f * Projectile.scale, completionRatio);

        public Color FlameTrailColorFunction(float completionRatio)
        {
            float trailOpacity = Utils.GetLerpValue(0.8f, 0.27f, completionRatio, true) * Utils.GetLerpValue(0f, 0.067f, completionRatio, true);
            return Color.DarkBlue * trailOpacity;
        }
    }
}