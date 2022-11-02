using Terraria.ModLoader;
using CalamityMod;
using Terraria;
using Microsoft.Xna.Framework;
using CalRemix.Projectiles;
using CalamityMod.Projectiles.Rogue;
using CalamityMod.Projectiles.Typeless;
using CalamityMod.Buffs.DamageOverTime;
using static Terraria.ModLoader.ModContent;
using CalamityMod.Projectiles.Boss;

namespace CalRemix
{
	public class CalRemixProjectile : GlobalProjectile
	{
		public bool nihilicArrow = false;
		public bool rogueclone = false;
		public override bool InstancePerEntity
		{
			get
			{
				return true;
			}
		}
        public override void SetDefaults(Projectile projectile)
        {
            if (projectile.type == ModContent.ProjectileType<BrimstoneBall>())
            {
                projectile.Name = "Calamity Fireball";
            }
            else if (projectile.type == ModContent.ProjectileType<BrimstoneBarrage>())
            {
                projectile.Name = "Calamity Barrage";
            }
            else if (projectile.type == ModContent.ProjectileType<BrimstoneFire>())
            {
                projectile.Name = "Calamity Fire";
            }
            else if (projectile.type == ModContent.ProjectileType<BrimstoneHellblast>())
            {
                projectile.Name = "Calamity Hellblast";
            }
            else if (projectile.type == ModContent.ProjectileType<BrimstoneHellblast2>())
            {
                projectile.Name = "Calamity Hellblast";
            }
            else if (projectile.type == ModContent.ProjectileType<BrimstoneHellfireball>())
            {
                projectile.Name = "Calamity Hellfireball";
            }
            else if (projectile.type == ModContent.ProjectileType<BrimstoneMonster>())
            {
                projectile.Name = "Calamity Monster";
            }
            else if (projectile.type == ModContent.ProjectileType<BrimstoneRay>())
            {
                projectile.Name = "Calamity Ray";
            }
            else if (projectile.type == ModContent.ProjectileType<BrimstoneTargetRay>())
            {
                projectile.Name = "Calamity Ray";
            }
            else if (projectile.type == ModContent.ProjectileType<BrimstoneWave>())
            {
                projectile.Name = "Calamity Flame Skull";
            }
        }

        public override void AI(Projectile projectile)
        {
			Player player = Main.LocalPlayer;
			CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();
			if (modPlayer.brimPortal && nihilicArrow)
				CalamityMod.CalamityUtils.HomeInOnNPC(projectile, false, 2500, 10f, 1);
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
			if (Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().roguebox && projectile.Calamity().stealthStrike && Main.LocalPlayer.ownedProjectileCounts[ProjectileType<DarksunTornado>()] <= 1)
			{
				int p = Projectile.NewProjectile(projectile.GetSource_FromThis(), new Vector2(projectile.Center.X - 10, projectile.Center.Y), Vector2.Zero, ModContent.ProjectileType<DarksunTornado>(), 20000, 0, Main.LocalPlayer.whoAmI);
				if (p.WithinBounds(Main.maxProjectiles))
				{
					Main.projectile[p].originalDamage = 20000;
				}
			}
		}

		public override void Kill(Projectile projectile, int timeLeft)
		{
			if (rogueclone)
			{
				int type = Main.rand.Next(0, 3);
				switch (type)
				{
					case 0:
						type = ProjectileType<JewelSpike>();
						break;
					case 1:
						type = ProjectileType<LostSoulFriendly>();
						break;
					case 2:
						type = ProjectileType<DragonShit>();
						break;
				}
				for (int i = 0; i < 5; i++)
                {
					Projectile p = Main.projectile[Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, new Vector2(Main.rand.Next(-10, 10), Main.rand.Next(-10, 10)), type, 20, 0, projectile.owner)];
					p.DamageType = ModContent.GetInstance<RogueDamageClass>();
					if (p.whoAmI.WithinBounds(Main.maxProjectiles))
					{
						p.originalDamage = 20;
					}

				}
			}
        }
    }
}

