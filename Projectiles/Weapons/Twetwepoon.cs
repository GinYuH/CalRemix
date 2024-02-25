using CalamityMod;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Weapons
{
	public class Twetwepoon : ModProjectile
    {
        public ref float State => ref Projectile.ai[0];
        public Player Owner => Main.player[Projectile.owner];
        public override void SetStaticDefaults() 
        {
			DisplayName.SetDefault("Twetwepoon");
		}
		public override void SetDefaults() 
        {
            Projectile.width = 14;
			Projectile.height = 14;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 22;
        }
		public override void AI()
		{
			if (Owner is null)
				return;
            Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI / 2 * ((State == 0) ? 1 : -1);
            if (State < 1 && Projectile.Distance(Owner.Center) > 352)
                State = 1;
            if (State > 0)
            {
                Owner.SetDummyItemTime(5);
                Owner.ChangeDir((Projectile.Center.X > Owner.Center.X) ? 1 : -1);
                Projectile.tileCollide = false;
                Projectile.velocity = ((Owner.Center - Projectile.Center) / 3.5f).ClampMagnitude(22, 22);
            }
            if (State > 0 && Projectile.Distance(Owner.Center - new Vector2(0, Owner.width / 2)) < 50f)
                Projectile.Kill();
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (State > 0)
                return false;
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            Projectile.velocity = -oldVelocity;
            State = 1;
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (State > 0)
                return;
            State = 1;
        }
    }
}