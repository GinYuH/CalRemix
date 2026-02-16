using CalamityMod.Projectiles.BaseProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Items.Weapons;
using CalamityMod.Sounds;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class SquidzookaHoldout : BaseGunHoldoutProjectile
    {
        public override string Texture => "CalRemix/Content/Items/Weapons/Squidzooka";
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
            Squidzooka.FireRate = 34;
            Projectile.ai[2]++;
            if (Projectile.ai[2] % Squidzooka.FireRate == 0)
            {
                SoundEngine.PlaySound(CommonCalamitySounds.LargeWeaponFireSound, Projectile.Center);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), GunTipPosition, GunTipPosition.DirectionTo(Main.MouseWorld) * 30, ModContent.ProjectileType<SquidzookaBlast>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                for (int i = 0; i < 20; i++)
                {
                    Dust.NewDustPerfect(GunTipPosition, DustID.Obsidian, GunTipPosition.DirectionTo(Main.MouseWorld).RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(4, 8), 60, Scale: Main.rand.NextFloat(0.8f, 1.2f));
                }
            }
        }
    }
}
