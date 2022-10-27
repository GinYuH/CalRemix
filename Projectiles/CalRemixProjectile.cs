using Terraria.ModLoader;
using CalamityMod;
using Terraria;
using Microsoft.Xna.Framework;
using CalRemix.Projectiles;
using CalamityMod.Buffs.DamageOverTime;
using static Terraria.ModLoader.ModContent;

namespace CalRemix
{
	public class CalRemixProjectile : GlobalProjectile
	{
		public bool nihilicArrow = false;
		public override bool InstancePerEntity
		{
			get
			{
				return true;
			}
		}

		public override void AI(Projectile projectile)
        {
			Player player = Main.LocalPlayer;
			CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();
			if (modPlayer.brimPortal && nihilicArrow)
				CalamityMod.CalamityUtils.HomeInOnNPC(projectile, false, 500, 10f, 1);
		}

        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
		{
			Player player = Main.LocalPlayer;
			var source = projectile.GetSource_FromThis();
			if (player.GetModPlayer<CalRemixPlayer>().arcanumHands && projectile.type != ProjectileType<ArmofAgony>())
			{
				target.AddBuff(BuffType<DemonFlames>(), 180);
				int apparatusDamage = (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(260);
				int proj = Projectile.NewProjectile(source, projectile.Center, Vector2.Zero, ProjectileType<ArmofAgony>(), apparatusDamage, 4f, projectile.owner);
				if (proj.WithinBounds(Main.maxProjectiles))
					Main.projectile[proj].DamageType = DamageClass.Summon;
			}
		}

    }
}
