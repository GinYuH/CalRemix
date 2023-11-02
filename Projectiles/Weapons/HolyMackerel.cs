using CalamityMod;
using CalRemix.Buffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Humanizer.In;

namespace CalRemix.Projectiles.Weapons
{
	public class HolyMackerel : ModProjectile
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Holy Mackerel");
			Main.projFrames[Projectile.type] = 1;
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
			Main.projPet[Projectile.type] = true;

			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		}

		public override void SetDefaults() 
		{
			Projectile.width = 60;
			Projectile.height = 26;
			Projectile.tileCollide = true;
			Projectile.friendly = true;
			Projectile.minion = true;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.minionSlots = 0.5f;
			Projectile.penetrate = -1;
			Projectile.aiStyle = 54;
			AIType = 317;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 6;
        }

		public override bool? CanCutTiles() 
		{
			return false;
		}
		public override bool MinionContactDamage() 
		{
			return true;
		}
		public override void AI() 
		{
			Player owner = Main.player[Projectile.owner];
			CheckActive(owner);
			if (Projectile.ai[0] == 1 && Main.rand.NextBool(10))
            {
				int num357 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), 1, 1, DustID.AncientLight);
				Dust dust = Main.dust[num357];
				dust.scale = Main.rand.Next(60, 90) * 0.013f;
				dust.velocity = new Vector2(-Projectile.velocity.X + Main.rand.Next(-5, 6), Main.rand.Next(-5, 6));
				dust.alpha = 50;
				dust.noGravity = false;
			}
			Projectile.MinionAntiClump();
			Projectile.rotation = 0;
		}
		private void CheckActive(Player owner)
		{
            owner.AddBuff(ModContent.BuffType<MackerelBuff>(), 3600);
            if (Projectile.type != ModContent.ProjectileType<HolyMackerel>())
				return;
            if (owner.dead)
                owner.GetModPlayer<CalRemixPlayer>().mackerel = false;
            if (owner.GetModPlayer<CalRemixPlayer>().mackerel)
                Projectile.timeLeft = 2;
        }
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.velocity = Vector2.Normalize(Projectile.oldVelocity) * -12f;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = true;
            return true;
        }
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(255, 255, 255, 255);
		}
    }
}
