using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using static CalamityMod.CalamityUtils;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class WaraxeReloadedHoldout : ModProjectile
    {

        public Player Owner => Main.player[Projectile.owner];
        public ref float Time => ref Projectile.ai[0];
        public int Direction => Projectile.velocity.X.DirectionalSign();

        public float SwingCompletion => MathHelper.Clamp(Time / 60, 0f, 1f);
        public override string Texture => "CalRemix/Content/Items/Weapons/WaraxeReloaded";
        public float SwordRotation
        {
            get
            {
                float swordRotation = InitialRotation + (Projectile.ai[2] == 1 ? SwingDown(SwingCompletion, Vector2.Zero) : SwingUp(SwingCompletion, Vector2.Zero)) * Projectile.spriteDirection + MathHelper.PiOver4;
                if (Projectile.spriteDirection == 1)
                    swordRotation += MathHelper.PiOver2;
                if (Projectile.ai[2] == 2)
                    swordRotation -= MathHelper.PiOver2 * Projectile.velocity.X.DirectionalSign();

                if (Projectile.ai[2] == 3)
                    return Projectile.rotation;
                return swordRotation;
            }
        }

        // Easings for things such as rotation.
        public static float SwingCompletionRatio => 0.37f;

        public static float RecoveryCompletionRatio => 0.84f;

        public Vector2 SwordDirection => SwordRotation.ToRotationVector2() * Direction;
        public ref float InitialRotation => ref Projectile.ai[1];

        public static CurveSegment Piece1 => new(EasingType.PolyIn, 0.22f, 2, 1f, 3);
        public static CurveSegment Piece2 => new(EasingType.PolyIn, 0.7f, Piece1.EndingHeight, 2f, 1);
        public static CurveSegment Piece3 => new(EasingType.PolyOut, 0.8f, Piece2.EndingHeight, 0.97f, 3);

        public static CurveSegment Piece1Up => new(EasingType.PolyIn, 0.22f, -1.2f, -1f, 3);
        public static CurveSegment Piece2Up => new(EasingType.PolyIn, 0.7f, Piece1Up.EndingHeight, -2f, 1);
        public static CurveSegment Piece3Up => new(EasingType.PolyOut, 0.8f, Piece2Up.EndingHeight, -0.97f, 3);

        public static float SwingDown(float completion, Vector2 v) => PiecewiseAnimation(completion, Piece1, Piece2, Piece3);
        public static float SwingUp(float completion, Vector2 v) => PiecewiseAnimation(completion, Piece1Up, Piece2Up, Piece3Up);
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Waraxe Reloaded");
        }
        public override void SetDefaults()
        {
            Projectile.width = 90;
            Projectile.height = 86;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.timeLeft = 60;
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
                if (Projectile.ai[2] == 2)
                {
                    InitialRotation += MathHelper.Pi * Projectile.velocity.X.DirectionalSign();
                }
                if (Projectile.ai[2] == 3 && Projectile.rotation == 0)
                {
                    Projectile.rotation = -1.24f;
                }
                Projectile.netUpdate = true;
            }

            AdjustPlayerValues();
            StickToOwner();

            // Determine rotation.
            if (Projectile.ai[2] < 3)
            {
                Projectile.rotation = SwordRotation;
                if (Projectile.ai[2] == 2)
                {
                    Projectile.rotation += MathHelper.PiOver2 * Projectile.velocity.X.DirectionalSign();
                }
            }
            else
            {
                Projectile.rotation += 0.2f * Projectile.velocity.X.DirectionalSign();
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
            bool flip = (Projectile.ai[2] == 2 && Projectile.spriteDirection == 1) || (Projectile.ai[2] != 2 & Projectile.spriteDirection == -1);
            if (flip)
            {
                origin.X += texture.Width;
            }

            SpriteEffects direction = !flip ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Main.spriteBatch.Draw(texture, drawPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, origin, Projectile.scale, direction, 0f);
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<ArmorCrunch>(), 300);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<ArmorCrunch>(), 300);
        }
    }
}