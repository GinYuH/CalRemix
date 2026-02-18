using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using CalRemix.Content.Items.Weapons;
using CalRemix.Content.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using static CalamityMod.CalamityUtils;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class ShadesBaneHoldout : ModProjectile
    {

        public Player Owner => Main.player[Projectile.owner];
        public ref float Time => ref Projectile.ai[0];
        public int Direction => Projectile.velocity.X.DirectionalSign();

        public static float Life => 80f;

        public float SwingCompletion => MathHelper.Clamp(Time / Life, 0f, 1f);
        public override string Texture => "CalRemix/Content/Items/Weapons/ShadesBane";
        public float SwordRotation
        {
            get
            {
                float swordRotation = InitialRotation + (!SwingingUp ? SwingDown(SwingCompletion, Vector2.Zero) : SwingUp(SwingCompletion, Vector2.Zero)) * Projectile.spriteDirection + MathHelper.PiOver4;
                if (Projectile.spriteDirection == 1)
                    swordRotation += MathHelper.PiOver2;
                if (SwingingUp)
                    swordRotation -= MathHelper.PiOver2 * Projectile.velocity.X.DirectionalSign();

                return swordRotation;
            }
        }

        // Easings for things such as rotation.
        public static float SwingCompletionRatio => 0.37f;

        public static float RecoveryCompletionRatio => 0.84f;

        public Vector2 SwordDirection => SwordRotation.ToRotationVector2() * Direction;
        public ref float InitialRotation => ref Projectile.ai[1];

        public static CurveSegment Piece1 => new(EasingType.CircIn, 0.4f, MathHelper.ToRadians(180), -MathHelper.ToRadians(80));
        public static CurveSegment Piece2 => new(EasingType.Linear, 0.45f, Piece1.EndingHeight, MathHelper.ToRadians(180));
        public static CurveSegment Piece3 => new(EasingType.CircOut, 0.5f, Piece2.EndingHeight, MathHelper.ToRadians(60));
        public static CurveSegment Piece1Up => new(EasingType.CircIn, 0.4f, -MathHelper.ToRadians(180), MathHelper.ToRadians(80));
        public static CurveSegment Piece2Up => new(EasingType.Linear, 0.45f, Piece1Up.EndingHeight, -MathHelper.ToRadians(180));
        public static CurveSegment Piece3Up => new(EasingType.CircOut, 0.5f, Piece2Up.EndingHeight, -MathHelper.ToRadians(60));


        public static CurveSegment Piece1Big => new(EasingType.PolyIn, 0.22f, 2, 1f, 3);
        public static CurveSegment Piece2Big => new(EasingType.PolyIn, 0.7f, Piece1Big.EndingHeight, 2f, 1);
        public static CurveSegment Piece3Big => new(EasingType.PolyOut, 0.8f, Piece2Big.EndingHeight, 0.97f, 3);

        public static float SwingDown(float completion, Vector2 v) => PiecewiseAnimation(completion, Piece1, Piece2, Piece3);
        public static float SwingUp(float completion, Vector2 v) => PiecewiseAnimation(completion, Piece1Up, Piece2Up, Piece3Up);
        public static float SwingBing(float completion, Vector2 v) => PiecewiseAnimation(completion, Piece1Big, Piece2Big, Piece3Big);

        public bool SwingingUp => Projectile.ai[2] == 2;
        public bool Spinning => false;// Projectile.ai[2] == 3;

        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 250;
            Projectile.height = 250;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.timeLeft = (int)Life;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.MaxUpdates = 2;
            Projectile.localNPCHitCooldown = Projectile.MaxUpdates * 22;
            Projectile.noEnchantmentVisuals = true;
        }

        public override void AI()
        {
            // Initialize the initial rotation if necessary.
            if (InitialRotation == 0f)
            {
                InitialRotation = Projectile.velocity.X > 0 ? 0 : MathHelper.Pi;
                if (SwingingUp)
                {
                    InitialRotation += MathHelper.ToRadians(250) * Projectile.velocity.X.DirectionalSign();
                }
                if (Spinning && Projectile.rotation == 0)
                {
                    Projectile.rotation = -1.24f;
                }
                Projectile.netUpdate = true;
            }

            AdjustPlayerValues();
            StickToOwner();

            // Determine rotation.
            if (!Spinning)
            {
                Projectile.rotation = SwordRotation;
                if (SwingingUp)
                {
                    Projectile.rotation += MathHelper.PiOver2 * Projectile.velocity.X.DirectionalSign();
                }

                if (Projectile.ai[2] == 3)
                {
                    if (SwingCompletion > 0.4f && SwingCompletion < 0.45f)
                    {
                        Projectile.scale = MathHelper.Lerp(1f, 2f, Utils.GetLerpValue(0.4f, 0.45f, SwingCompletion, true));
                    }
                    else if (SwingCompletion > 0.45f)
                    {
                        Projectile.scale = MathHelper.Lerp(2f, 1f, Utils.GetLerpValue(0.45f, 0.7f, SwingCompletion, true));
                    }
                }

            }
            else
            {
                Projectile.rotation += 0.2f * Projectile.velocity.X.DirectionalSign();
            }
            if (Time == 30)
            {
                SoundEngine.PlaySound(Projectile.ai[2] == 3 ? ShadesBane.WetSlashBig with { Pitch = -0.4f } : ShadesBane.WetSlash, Owner.Center);

                if (Projectile.ai[2] != 3)
                {
                    SoundEngine.PlaySound(BetterSoundID.ItemShadowflameHexDoll2 with { Pitch = 1.5f }, Owner.Center);
                    for (int i = 0; i < 5; i++)
                    {
                        float aivar = Projectile.ai[2] == 1 ? 0 : 1;
                        float spread = 0.5f * (Projectile.ai[2] == 1 ? MathHelper.PiOver4 : MathHelper.ToRadians(15));
                        float damageMod = Projectile.ai[2] == 1 ? 0.2f : 0.5f;
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + Projectile.Center.DirectionTo(Main.MouseWorld) * 80, Projectile.Center.DirectionTo(Main.MouseWorld).RotatedBy(MathHelper.Lerp(-spread, spread, i / 4f)) * Owner.HeldItem.shootSpeed * 0.3f, ModContent.ProjectileType<ShadeBolt>(), (int)(Projectile.damage * damageMod), 1, Projectile.owner, Main.rand.Next(0, 4), aivar);
                    }
                }
            }
            Time++;
        }

        public void AdjustPlayerValues()
        {
            Projectile.spriteDirection = Projectile.direction = Direction;
            Owner.heldProj = Projectile.whoAmI;
            Owner.itemTime = 2;
            Owner.itemAnimation = 2;
            Owner.itemRotation = (Projectile.direction * Projectile.velocity).ToRotation();

            // Decide the arm rotation for the owner.
            float armRotation = SwordRotation - Direction * 1.67f;
            Owner.SetCompositeArmFront(Math.Abs(armRotation) > 0.01f, Player.CompositeArmStretchAmount.Full, armRotation);
        }

        public void StickToOwner()
        {
            // Glue the sword to its owner. This applies a handful of offsets to make the blade look like it's roughly inside of the owner's hand.
            Projectile.Center = Owner.RotatedRelativePoint(Owner.MountedCenter, true) + SwordDirection * new Vector2(7f, 16f) * Projectile.scale;
            Projectile.Center -= Projectile.velocity.SafeNormalize(Vector2.UnitY) * new Vector2(66f, 54f + Projectile.scale * 8f);

            // Set the owner's held projectile to this and register a false item time calculation.
            Owner.heldProj = Projectile.whoAmI;
            Owner.SetDummyItemTime(2);

            // Make the owner turn in the direction of the blade.
            Owner.ChangeDir(Direction);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            Vector2 origin = texture.Size() * Vector2.UnitY;
            bool flip = (SwingingUp && Projectile.spriteDirection == 1) || (!SwingingUp & Projectile.spriteDirection == -1);
            if (flip)
            {
                origin.X += texture.Width;
            }

            SpriteEffects direction = !flip ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            if (SwingCompletion > 0.4f && SwingCompletion < 0.45f)
            {
                Main.spriteBatch.EnterShaderRegion(BlendState.Additive);
                Texture2D swing = ModContent.Request<Texture2D>("CalamityMod/Particles/TrientCircularSmear").Value;
                Main.spriteBatch.Draw(swing, drawPosition, null, Color.Magenta * MathHelper.Lerp(1, 0, Utils.GetLerpValue(0.4f, 0.45f, SwingCompletion, true)) * (Projectile.ai[2] == 3 ? 1f : 0.8f), Projectile.rotation - MathHelper.PiOver2 + (flip ? MathHelper.Pi : 0), swing.Size() / 2, Projectile.scale * (Projectile.ai[2] == 3 ? 3f : 2.4f), direction, 0f);
                Main.spriteBatch.ExitShaderRegion();
            }

            Main.spriteBatch.Draw(texture, drawPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, origin, Projectile.scale, direction, 0f);
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<WhisperingDeath>(), 300);
            if (target.Remix().shadeStacks < 4 && Projectile.ai[2] != 3 && Projectile.localAI[2] == 0)
            {
                target.Remix().shadeStacks++;
                Projectile.localAI[2] = 1;
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (target.Remix().shadeStacks >= 4 && Projectile.ai[2] == 3)
            {
                Main.LocalPlayer.Calamity().GeneralScreenShakePower += 10;
                modifiers.SourceDamage *= 5;
                target.Remix().shadeStacks = 0;

                for (int i = 0; i < 10; i++)
                {
                    VoidMetaball.SpawnParticle(Main.rand.NextVector2FromRectangle(target.Hitbox), Main.rand.NextVector2Circular(4, 4), Main.rand.NextFloat(120, 300));
                }
                for (int i = 0; i < 30; i++)
                {
                    int choice = Main.rand.Next(0, 3);
                    float speed = Main.rand.NextFloat(10, 30);
                    float size = Main.rand.NextFloat(80, 160);
                    switch (choice)
                    {
                        case 0:
                        VoidMetaballBlue.SpawnParticle(target.Center, Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi) * speed, size);
                            break;
                        case 1:
                            VoidMetaballGreen.SpawnParticle(target.Center, Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi) * speed, size);
                            break;
                        case 2:
                            VoidMetaballYellow.SpawnParticle(target.Center, Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi) * speed, size);
                            break;
                    }
                }

            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<WhisperingDeath>(), 300);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (SwingCompletion < 0.4f)
                return false;
            float point = 0f;

            Vector2 attackRotation = (InitialRotation + SwingDown(SwingCompletion, Vector2.Zero)).ToRotationVector2();
            if (Projectile.ai[2] == 2)
            {
                attackRotation = (InitialRotation + SwingUp(SwingCompletion, Vector2.Zero)).ToRotationVector2();
            }
            else if (Projectile.ai[2] == 3)
            {
                attackRotation = (InitialRotation + SwingBing(SwingCompletion, Vector2.Zero)).ToRotationVector2();
            }

            Vector2 direction = attackRotation * new Vector2(Projectile.spriteDirection, 1f);
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + direction * Projectile.height * Projectile.scale, Projectile.width * 0.25f, ref point);
        }
    }
}