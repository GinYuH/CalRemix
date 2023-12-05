using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.NPCs.Providence;
using CalamityMod.Particles;
using CalRemix.Gores;
using CalRemix.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Weapons
{
    public class ColdheartIcicleProjectile : ModProjectile
    {
        public static readonly SoundStyle ColdheartIcicleStrike = SoundID.DeerclopsIceAttack;

        public const int FadeInDuration = 12;
        public const int FadeOutDuration = 6;

        public const int TotalDuration = 24;

        public float CollisionWidth => 10f * Projectile.scale;

        public int Timer
        {
            get => (int)Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(18);
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ownerHitCheck = true;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 720;
            Projectile.hide = true;
        }


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            ref Player player = ref Main.player[Projectile.owner];
            NPC.HitInfo hitInfo = new()
            {
                Damage = (int)Math.Ceiling(target.lifeMax * 0.02),
                DamageType = DamageClass.MeleeNoSpeed
            };


            if (!(target.type == NPCID.TargetDummy
                || target.type == ModContent.GetInstance<Providence>().Type)
                )
            {
                for(int i = 0; i < 6; i++)
                {
                    Vector2 velocity = Vector2.Zero;
                    velocity.X = i switch
                    {
                        0 or 3 => -2.5f,
                        1 or 4 => 0,
                        2 or 5 => 2.5f,
                        _ => 0,
                    };
                    velocity.Y = i > 2 ? 2.5f : -2.5f;
                    int redness = Main.rand.Next(200, 256);
                    Color color = new(redness, 255, 255, 255);

                    Particle particle = new Snowflake(target.Center, velocity, 30, 1f, color)
                    {
                        VelocityDiminish = 0.05f,
                        SpinAmount = 1.2f
                    };
                    GeneralParticleHandler.SpawnParticle(particle);
                }

                player.StrikeNPCDirect(target, hitInfo);

                SoundEngine.PlaySound(ColdheartIcicleStrike, target.Center);
            }
        }



        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Timer += 1;
            if (Timer >= TotalDuration)
            {
                Projectile.Kill();
                return;
            }
            else
                player.heldProj = Projectile.whoAmI;

            Projectile.Opacity = Utils.GetLerpValue(0f, FadeInDuration, Timer, clamped: true) * Utils.GetLerpValue(TotalDuration, TotalDuration - FadeOutDuration, Timer, clamped: true);

            Vector2 playerCenter = player.RotatedRelativePoint(player.MountedCenter, reverseRotation: false, addGfxOffY: false);
            Projectile.Center = playerCenter + Projectile.velocity * (Timer - 1f);

            Projectile.spriteDirection = (Vector2.Dot(Projectile.velocity, Vector2.UnitX) >= 0f).ToDirectionInt();

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 - MathHelper.PiOver4 * Projectile.spriteDirection;

            SetVisualOffsets();
        }

        private void SetVisualOffsets()
        {
            const int HalfSpriteWidth = 24 / 2;
            const int HalfSpriteHeight = 24 / 2;

            int HalfProjWidth = Projectile.width / 2;
            int HalfProjHeight = Projectile.height / 2;

            DrawOriginOffsetX = 0;
            DrawOffsetX = -(HalfSpriteWidth - HalfProjWidth);
            DrawOriginOffsetY = -(HalfSpriteHeight - HalfProjHeight);
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Vector2 start = Projectile.Center;
            Vector2 end = start + Projectile.velocity.SafeNormalize(-Vector2.UnitY) * 10f;
            Utils.PlotTileLine(start, end, CollisionWidth, DelegateMethods.CutTiles);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 start = Projectile.Center;
            Vector2 end = start + Projectile.velocity * 6f;
            float collisionPoint = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, CollisionWidth, ref collisionPoint);
        }
    }
}