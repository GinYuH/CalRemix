using CalamityMod;
using CalamityMod.Projectiles.Melee;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
	public class RoxProj : ModProjectile
	{
        public override string Texture => "CalRemix/Content/Items/Weapons/Rox";
        public Player Owner => Main.player[Projectile.owner];
        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 15;
            Projectile.DamageType = DamageClass.Melee;
        }
        public override void AI()
        {
            Owner.mount?.Dismount(Owner);
            Owner.RemoveAllGrapplingHooks();
            Owner.heldProj = Projectile.whoAmI;
            Owner.itemTime = Owner.itemAnimation;
            Owner.SetDummyItemTime(2);
            Owner.Center = Projectile.Top;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            KillRox();
            return true;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            KillRox();
        }
        private void KillRox()
        {
            SoundEngine.PlaySound(BetterSoundID.ItemExplosion);
            Owner.velocity = -Projectile.velocity;
            Owner.Remix().roxCooldown = 60;
            for (int i = 0; i < 24; i++)
            {
                Vector2 vector = Vector2.UnitY.RotatedBy(MathF.PI * 2f * (float)i / 24f);
                int dust = Dust.NewDust(Projectile.Center + vector * 20f * Projectile.scale, 0, 0, 1, vector.X * 12f * Projectile.scale, vector.Y * 12f * Projectile.scale, 0, Color.Black, Projectile.scale);
                if (dust.WithinBounds(Main.maxDust))
                {
                    Main.dust[dust].noGravity = true;
                }
            }
            for (int i = 0; i < 9; i++)
            {
                Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Normalize(Projectile.velocity).RotatedByRandom(MathHelper.PiOver2) * -10f, ModContent.ProjectileType<Rox1>(), Projectile.damage / 10, 0);
            }
        }
    }
}