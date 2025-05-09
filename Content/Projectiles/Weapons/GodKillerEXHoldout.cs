using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Projectiles.BaseProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Items.Weapons;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;
using System;
using CalamityMod.Particles;
using CalamityMod;
using System.Threading;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class GodKillerEXHoldout : BaseGunHoldoutProjectile
    {
        public override int AssociatedItemID => ModContent.ItemType<GodKillerEX>();
        public override Vector2 GunTipPosition => base.GunTipPosition - Vector2.UnitX.RotatedBy(Projectile.rotation) - (Vector2.UnitY.RotatedBy(Projectile.rotation) * 7f * Projectile.spriteDirection * Owner.gravDir);
        public override float MaxOffsetLengthFromArm => 0;
        public override float OffsetXUpwards => 0;
        public override float BaseOffsetY => 0;
        public override float OffsetYDownwards => 0;
        public ref float ShootTimer => ref Projectile.ai[1];

        public override void KillHoldoutLogic()
        {
            if (Owner.CantUseHoldout(false))
            {
                Projectile.Kill();
            }
            else if ((Projectile.frame <= 0 && ShootTimer > 77) || (!Owner.channel && ShootTimer < 77))
            {
                Projectile.Kill();
            }
        }

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.projFrames[Type] = 8;
        }

        public override void HoldoutAI()
        {
            ShootTimer++;
            Projectile.frameCounter++;
            int animSpeed = Owner.channel ? 11 : 5;
            int startFire = 7 * animSpeed;
            int increment = Owner.channel ? 1 : -1;
            if (Projectile.frameCounter > animSpeed)
            {
                Projectile.frame += increment;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 7)
            {
                Projectile.frame = 6;
            }

            if (ShootTimer == 1)
            {
                SoundEngine.PlaySound(new SoundStyle("CalRemix/Assets/Sounds/GodKillerBeam"), Projectile.Center);
            }
            if (ShootTimer == startFire)
            {
                for (int i = 0; i < 24; i++)
                {
                    Vector2 dustpos = Vector2.UnitX * (float)-(float)Projectile.width / 2f;
                    Vector2 ogPos = dustpos;
                    dustpos += -Vector2.UnitY.RotatedBy((double)((float)i * 3.14159274f / 12f), default) * new Vector2(8f, 2f);
                    dustpos = dustpos.RotatedBy(Projectile.rotation, ogPos);
                    Vector2 spawnPos = Projectile.Center - dustpos;

                    Dust d = Dust.NewDustPerfect(Projectile.Center + Projectile.rotation.ToRotationVector2() * 10, DustID.Electric, spawnPos.DirectionTo(Projectile.Center - ogPos) * Main.rand.NextFloat(3f, 7f));
                    d.noGravity = true;

                    /*Particle p = new HeavySmokeParticle(Projectile.Center + Projectile.DirectionTo(Main.MouseWorld) * 10, spawnPos.DirectionTo(Projectile.Center - ogPos) * Main.rand.NextFloat(3f, 7f), Color.Cyan, 20, Main.rand.NextFloat(0.1f, 0.3f), 180f, 0.2f, true);
                    GeneralParticleHandler.SpawnParticle(p);*/
                }
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.rotation.ToRotationVector2() * 4, ModContent.ProjectileType<GodKillerBeam>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D value = ModContent.Request<Texture2D>("CalRemix/Content/Projectiles/Weapons/GodKillerEXHoldout").Value;
            Main.EntitySpriteDraw(value, Projectile.Center - Main.screenPosition + Projectile.rotation.ToRotationVector2() * 20, value.Frame(1, 8, 0, Projectile.frame), rotation: Projectile.rotation + ((Projectile.spriteDirection == -1) ? MathF.PI : 0f), origin: new Vector2(value.Width / 2, value.Height / 16), effects: ((float)Projectile.spriteDirection * Owner.gravDir == -1f) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, color: Projectile.GetAlpha(lightColor), scale: Projectile.scale * Owner.gravDir);
            return false;
        }
    }
}
