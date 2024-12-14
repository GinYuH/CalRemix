using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Projectiles.BaseProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Items.Weapons;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class ChainsmokerHoldout : BaseGunHoldoutProjectile
    {
        public override int AssociatedItemID => ModContent.ItemType<Chainsmoker>();
        public override Vector2 GunTipPosition => base.GunTipPosition - Vector2.UnitX.RotatedBy(Projectile.rotation) - (Vector2.UnitY.RotatedBy(Projectile.rotation) * 7f * Projectile.spriteDirection * Owner.gravDir);
        public override float MaxOffsetLengthFromArm => 20f;
        public override float OffsetXUpwards => -5f;
        public override float BaseOffsetY => -5f;
        public override float OffsetYDownwards => 5f;
        public ref float ShootTimer => ref Projectile.ai[1];

        public override void HoldoutAI()
        {
            ShootTimer++;
            if (ShootTimer % HeldItem.useTime == 0)
            {
                SoundEngine.PlaySound(DragonsBreath.WeldingBurn with { Pitch = -0.4f }, Projectile.Center);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), GunTipPosition, (Projectile.velocity * 9).RotatedByRandom(0.1f), ModContent.ProjectileType<Chainsmog>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
            if (ShootTimer == 60)
            {
                SoundEngine.PlaySound(DragonsBreath.FireballSound with { Pitch = -0.4f }, Projectile.Center);
                for (int i = 0; i < 12; i++)
                {
                    Vector2 dustpos = Vector2.UnitX * (float)Projectile.width / 2f;
                    dustpos += -Vector2.UnitY.RotatedBy((double)((float)i * 3.14159274f / 6f), default) * new Vector2(8f, 16f);
                    dustpos = dustpos.RotatedBy((double)(Projectile.rotation), default);
                    int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.Torch, 0f, 0f, 100, Color.Red, 1f);
                    Main.dust[dust].scale = 1.1f;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].position = Projectile.Center + dustpos;
                    Main.dust[dust].velocity = Projectile.velocity * 0.1f;
                    Main.dust[dust].velocity = Vector2.Normalize(Projectile.Center + Projectile.velocity * 3f + Main.dust[dust].position) * 1.25f;
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            if (ShootTimer > 60)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), GunTipPosition, Projectile.velocity * 9, ModContent.ProjectileType<Chainfire>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                SoundEngine.PlaySound(DragonsBreath.WeldingStart, Projectile.Center);
            }
        }
    }
}
