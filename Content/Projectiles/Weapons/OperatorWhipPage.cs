using CalamityMod.Projectiles.BaseProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using CalRemix.Content.Items.Weapons;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class OperatorWhipPage : BaseGunHoldoutProjectile
    {
        public override int AssociatedItemID => ModContent.ItemType<SignoftheOperator>();
        public override Vector2 GunTipPosition => base.GunTipPosition - Vector2.UnitX.RotatedBy(Projectile.rotation) - (Vector2.UnitY.RotatedBy(Projectile.rotation) * 7f * Projectile.spriteDirection * Owner.gravDir);
        public override float MaxOffsetLengthFromArm => 0;
        public override float OffsetXUpwards => 0;
        public override float BaseOffsetY => 0;
        public override float OffsetYDownwards => 0;
        public ref float ShootTimer => ref Projectile.ai[1];

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.projFrames[Type] = 4;
        }

        public override void HoldoutAI()
        {
            ShootTimer++;
            Projectile.frameCounter++;
            int animSpeed = 5;
            if (Projectile.frameCounter > animSpeed)
            {
                Projectile.frame += 1;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 3)
            {
                Projectile.frame = 0;
            }
            if (ShootTimer % 5 == 0)
            {
                Vector2 velocity = Projectile.Center.DirectionTo(Main.MouseWorld) * 5;
                for (int i = 0; i < Main.rand.Next(1, 3); i++)
                {
                    velocity += Main.rand.NextVector2Circular(5, 5);
                    int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<OperatorWhipEnd>(), Projectile.damage, Projectile.knockBack, Owner.whoAmI);
                    Main.projectile[p].localAI[1] = Main.rand.NextBool().ToDirectionInt();
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D value = ModContent.Request<Texture2D>("CalRemix/Content/Projectiles/Weapons/OperatorWhipPage").Value;
            Main.EntitySpriteDraw(value, Projectile.Center - Main.screenPosition + Projectile.rotation.ToRotationVector2() * 20, value.Frame(1, 4, 0, Projectile.frame), rotation: Projectile.rotation + ((Projectile.spriteDirection == -1) ? MathF.PI : 0f), origin: new Vector2(value.Width / 2, value.Height / 8), effects: ((float)Projectile.spriteDirection * Owner.gravDir == -1f) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, color: Projectile.GetAlpha(lightColor), scale: Projectile.scale * Owner.gravDir);
            return false;
        }
    }
}
