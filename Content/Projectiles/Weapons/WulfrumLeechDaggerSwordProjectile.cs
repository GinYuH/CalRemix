using CalamityMod.Particles;
using CalRemix.Assets.Gores;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class WulfrumLeechDaggerSwordProjectile : ModProjectile
    {
        public static readonly SoundStyle LeechDaggerBreak = new("CalRemix/Assets/Sounds/LeechDaggerBreak")
        {
            PitchVariance = 0.3f,
            Pitch = -0.5f,
            Volume = 0.4f
        };

        public const int FadeInDuration = 8;
        public const int FadeOutDuration = 6;

        public const int TotalDuration = 24;

        public float CollisionWidth => 10f * Projectile.scale;

        public int Timer
        {
            get => (int)Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(26);
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
                Damage = (int)Math.Ceiling(target.lifeMax * 0.005),
                DamageType = DamageClass.MeleeNoSpeed
            };


            if (!(target.type == NPCID.TargetDummy
                // || target.type == ModContent.NPCType<SuperDummyNPC>()
                ))
            {
                for(int i = 0; i < Main.rand.Next(4, 7); i++)
                {
                    Vector2 velocity = Utils.RotatedByRandom(Utils.SafeNormalize(Projectile.velocity, Vector2.UnitY), Main.rand.NextFloat(0.6f, 0.9f));
                    GeneralParticleHandler.SpawnParticle(new TechyHoloysquareParticle(Projectile.Center, velocity * 7f, Main.rand.NextFloat(2.5f, 4.5f), Utils.NextBool(Main.rand) ? new Color(99, 255, 229) : new Color(25, 132, 247), 25));
                }

                player.StrikeNPCDirect(target, hitInfo);
            }

            

            if (player.name != "John Wulfrum")
            {
                player.HeldItem.stack--;

                int daggerGore = Gore.NewGore(Main.player[Projectile.owner].GetSource_FromThis(), Projectile.Center, Projectile.velocity, ModContent.GoreType<WulfrumLeechDaggerGore0>());
                Main.gore[daggerGore].rotation = Projectile.rotation;
                Main.gore[daggerGore].timeLeft = 30;

                SoundEngine.PlaySound(LeechDaggerBreak, Projectile.Center);

                Projectile.damage = 0;
                Projectile.frame = 1;
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
            const int HalfSpriteWidth = 34 / 2;
            const int HalfSpriteHeight = 34 / 2;

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