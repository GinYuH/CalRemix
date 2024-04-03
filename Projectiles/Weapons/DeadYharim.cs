using CalamityMod;
using CalRemix.Items.Weapons;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Weapons
{
    public class DeadYharim : ModProjectile
	{
        public override string Texture => "CalRemix/Items/Weapons/TyrantShield";
        public Player Owner => Main.player[Projectile.owner];
        public override void SetStaticDefaults() 
        {
			DisplayName.SetDefault("Tyrant Shield");
		}
        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 68;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Generic;
        }
        public override void AI()
        {
            if (Owner.HasCooldown(TyrantCooldown.ID) || Owner.HeldItem.type != ModContent.ItemType<TyrantShield>())
            {
                Projectile.Kill();
                return;
            }
            Projectile.timeLeft = 2;
            foreach (Projectile p in Main.projectile)
            {
                if (p.hostile && !p.friendly && p.Hitbox.Intersects(Projectile.Hitbox) && p.velocity.Y <= 10f && p.velocity.Y >= -10f && Projectile.spriteDirection != p.direction)
                {
                    p.velocity *= -1;
                    p.friendly = true;
                    p.hostile = false;
                    p.damage = Projectile.damage;
                    SoundEngine.PlaySound(SoundID.PlayerKilled);
                    if (!Owner.HasCooldown(TyrantCooldown.ID))
                        Owner.AddCooldown(TyrantCooldown.ID, CalamityUtils.SecondsToFrames(60));
                    Projectile.Kill();
                }
            }
            if (Main.myPlayer == Owner.whoAmI)
            {
                Projectile.Center = Owner.Center + Owner.DirectionTo(Main.MouseWorld) * 16f;
                Projectile.spriteDirection = (Main.MouseWorld.X < Owner.Center.X) ? -1 : 1;
                Owner.ChangeDir(Projectile.spriteDirection);
                Projectile.netUpdate = true;
            }
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (target.velocity.Y <= 10f && target.velocity.Y >= -10f && target.velocity.X != 0 && Projectile.spriteDirection != target.direction)
                return true;
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(SoundID.PlayerKilled);
            if (!Owner.HasCooldown(TyrantCooldown.ID))
                Owner.AddCooldown(TyrantCooldown.ID, CalamityUtils.SecondsToFrames(60));
            Projectile.Kill();
        }
    }
}