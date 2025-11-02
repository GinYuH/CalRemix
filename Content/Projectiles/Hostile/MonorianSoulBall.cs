using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Graphics.Primitives;
using CalamityMod.Particles;
using CalamityMod.Sounds;
using CalRemix.Content.NPCs.Subworlds.Sealed;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.Graphics.Effects.Filters;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class MonorianSoulBall : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 11;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
        }

        public override void AI()
        {
            int gem = ModContent.NPCType<MonorianGemBoss>();
            foreach (NPC n in Main.ActiveNPCs)
            {
                if (n.type == gem)
                {
                    if (n.Hitbox.Intersects(Projectile.Hitbox))
                    {
                        SoundEngine.PlaySound(CommonCalamitySounds.ExoPlasmaExplosionSound, Projectile.Center);
                        Projectile.Kill();
                    }
                }
            }
            GeneralParticleHandler.SpawnParticle(new SparkleParticle(Main.rand.NextVector2FromRectangle(Projectile.Hitbox), Main.rand.NextVector2Circular(4, 4), Color.White, Color.Cyan, 0.2f, 30, Main.rand.NextFloat(-1, 1)));
        }

        public override void OnKill(int timeLeft)
        {
            float projCount = 60;
            for (int i = 0; i < projCount; i++)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 velocity = Vector2.UnitY.RotatedBy(MathHelper.Lerp(0, MathHelper.TwoPi, i / projCount));

                    float toRot = velocity.ToRotation();
                    float dirToP = Projectile.DirectionTo(Main.npc[(int)Projectile.ai[0]].Center).ToRotation();
                    float spread = MathHelper.ToRadians(10);
                    if (Math.Abs(MathHelper.WrapAngle(toRot - (spread * 0.5f)) - MathHelper.WrapAngle(dirToP)) < MathHelper.ToRadians(10))
                        continue;

                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.UnitY.RotatedBy(MathHelper.Lerp(0, MathHelper.TwoPi, i / projCount)) * 20f, ModContent.ProjectileType<MonorianSoulBolt>(), Projectile.damage, 1);
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D spark = ModContent.Request<Texture2D>("CalamityMod/Particles/BloomRing").Value;
            Texture2D star = ModContent.Request<Texture2D>("CalamityMod/Particles/Sparkle").Value;
            Texture2D bloom = ModContent.Request<Texture2D>("CalamityMod/Particles/LargeBloom").Value;
            Main.spriteBatch.EnterShaderRegion(BlendState.Additive);

            Main.spriteBatch.Draw(spark, Projectile.Center - Main.screenPosition, null, Color.Cyan, Main.GlobalTimeWrappedHourly, spark.Size() / 2, Projectile.scale * 0.6f, SpriteEffects.FlipHorizontally, 0);
            Main.spriteBatch.Draw(bloom, Projectile.Center - Main.screenPosition, null, Color.Cyan, Main.GlobalTimeWrappedHourly, bloom.Size() / 2, Projectile.scale * 0.2f, SpriteEffects.FlipHorizontally, 0);
            Main.spriteBatch.Draw(star, Projectile.Center - Main.screenPosition, null, Color.White, 0, star.Size() / 2, Projectile.scale + (1 + 0.5f * MathF.Sin(Main.GlobalTimeWrappedHourly * 2f)), SpriteEffects.FlipHorizontally, 0);
            Main.spriteBatch.ExitShaderRegion();
            return false;
        }
    }
}