using CalamityMod.Projectiles.BaseProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using CalRemix.Content.Items.Weapons;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class XiphactinusGunHoldout : BaseGunHoldoutProjectile
    {
        public override string Texture => "CalRemix/Content/Projectiles/Weapons/XiphactinusGunHoldout";
        public override int AssociatedItemID => ModContent.ItemType<XiphactinusGun>();
        public override Vector2 GunTipPosition => base.GunTipPosition - Vector2.UnitX.RotatedBy(Projectile.rotation) - (Vector2.UnitY.RotatedBy(Projectile.rotation) * 7f * Projectile.spriteDirection * Owner.gravDir);
        public override float MaxOffsetLengthFromArm => 0;
        public override float OffsetXUpwards => 0;
        public override float BaseOffsetY => 0;
        public override float OffsetYDownwards => 0;
        public ref float ShootTimer => ref Projectile.ai[1];

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void HoldoutAI()
        {
            if (Projectile.ai[2] == 0)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.DirectionTo(Main.MouseWorld) * 20, ModContent.ProjectileType<XiphactinusGunProj>(), Projectile.damage, Projectile.knockBack, Projectile.owner, ai2: Projectile.whoAmI);
                Projectile.ai[2] = 1;
            }
            else
            {
                if (Owner.ownedProjectileCounts[ModContent.ProjectileType<XiphactinusGunProj>()] <= 0)
                    Projectile.Kill();
            }
        }
    }
}
