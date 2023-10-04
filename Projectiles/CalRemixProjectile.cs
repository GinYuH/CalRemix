using Terraria.ModLoader;
using CalamityMod;
using Terraria;
using Terraria.DataStructures; 
using Microsoft.Xna.Framework;
using CalRemix.Projectiles.Accessories;
using CalRemix.Projectiles.WulfrumExcavator;
using CalRemix.NPCs.Bosses;
using CalamityMod.Projectiles.Rogue;
using CalamityMod.Projectiles.Typeless;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Projectiles.Summon;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using static Terraria.ModLoader.ModContent;
using CalamityMod.Projectiles.Boss;
using Microsoft.Xna.Framework.Graphics;
using CalamityMod.Projectiles.Melee.Spears;
using Terraria.GameContent;
using System;
using Terraria.Graphics.Shaders;
using CalamityMod.Events;

namespace CalRemix
{
	public class CalRemixProjectile : GlobalProjectile
	{
		public bool nihilicArrow = false;
		public bool rogueclone = false;
		public bool tvoproj = false;
		public bool uniproj = false;
        public bool hyperCharged = false;
		public int bladetimer = 0;
        NPC exc;
        public override bool InstancePerEntity => true;
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
			bladetimer--;
			if (modPlayer.brimPortal && nihilicArrow && !projectile.minion && !projectile.sentry && projectile.velocity != Vector2.Zero)
				CalamityMod.CalamityUtils.HomeInOnNPC(projectile, false, 2500, 10f, 1);
			if (modPlayer.tvo && tvoproj)
            {
				if (projectile.type == ProjectileType<RainbowComet>())
                {
					CalamityUtils.HomeInOnNPC(projectile, true, 1200, 20, 1);
				}
            }
			if (modPlayer.tvo && projectile.DamageType == GetInstance<RogueDamageClass>() && bladetimer <= 0 && projectile.type != ProjectileType<Nanotech>() && player.ownedProjectileCounts[ProjectileType<Nanotech>()] < 8)
            {
				Projectile.NewProjectile(projectile.GetSource_FromAI(), projectile.Center, Vector2.Zero, ProjectileType<Nanotech>(), (int)(projectile.damage * 0.2f), 0, projectile.owner);
				bladetimer = 30;
            }
			if (modPlayer.tvo && projectile.type == ProjectileType<SandElementalHealer>() && player.statLife < player.statLifeMax && player.ownedProjectileCounts[ProjectileType<CalamityMod.Projectiles.Healing.CactusHealOrb>()] < 2)
            {
				Projectile.NewProjectile(projectile.GetSource_FromAI(), projectile.Center, Vector2.Zero, ProjectileType<CalamityMod.Projectiles.Healing.CactusHealOrb>(), 0, 0, projectile.owner);
            }
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[projectile.owner];
			CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();
			var source = projectile.GetSource_FromThis();
			if (modPlayer.tvo && CalamityUtils.CountProjectiles(ModContent.ProjectileType<PlagueSeeker>()) > 3 && projectile.type == ProjectileType<PlagueSeeker>())
			{
				projectile.active = false;
			}
			if (modPlayer.arcanumHands && projectile.type != ProjectileType<ArmofAgony>() && CalamityUtils.CountProjectiles(ModContent.ProjectileType<ArmofAgony>()) < 8)
			{
				target.AddBuff(BuffType<BrimstoneFlames>(), 180);
				int apparatusDamage = (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(260);
				int proj = Projectile.NewProjectile(source, projectile.Center, Vector2.Zero, ProjectileType<ArmofAgony>(), apparatusDamage, 4f, projectile.owner);
				if (proj.WithinBounds(Main.maxProjectiles))
                {
					Main.projectile[proj].originalDamage = apparatusDamage;
                }
				Main.projectile[proj].DamageType = DamageClass.Summon;
			}
			if (modPlayer.tvo && projectile.type != ProjectileType<JewelSpike>())
			{
				target.AddBuff(BuffType<BrimstoneFlames>(), 180);
				int apparatusDamage = (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(1060);
				if (CalamityUtils.CountProjectiles(ModContent.ProjectileType<JewelSpike>()) < 3)
				{
					int proj = Projectile.NewProjectile(source, projectile.Center, Vector2.Zero, ProjectileType<JewelSpike>(), apparatusDamage, 4f, projectile.owner);
					if (proj.WithinBounds(Main.maxProjectiles))
					{
						Main.projectile[proj].originalDamage = apparatusDamage;
					}
					Main.projectile[proj].DamageType = DamageClass.Summon;
					Main.projectile[proj].GetGlobalProjectile<CalRemixProjectile>().tvoproj = true;
				}
			}
			if (modPlayer.roguebox && projectile.Calamity().stealthStrike && player.ownedProjectileCounts[ProjectileType<DarksunTornado>()] <= 1)
			{
				int p = Projectile.NewProjectile(projectile.GetSource_FromThis(), new Vector2(projectile.Center.X - 10, projectile.Center.Y), Vector2.Zero, ModContent.ProjectileType<DarksunTornado>(), 20000, 0, Main.LocalPlayer.whoAmI);
				if (p.WithinBounds(Main.maxProjectiles))
				{
					Main.projectile[p].originalDamage = 20000;
				}
			}
			if (modPlayer.tvo && projectile.type != ProjectileType<DarksunTornado>() && projectile.type != ProjectileType<NanoFlare>())
			{
				int dam = (int)(projectile.damage * 0.2f);
				if (CalamityUtils.CountProjectiles(ModContent.ProjectileType<DarksunTornado>()) < 3)
				{
					int p = Projectile.NewProjectile(projectile.GetSource_FromThis(), new Vector2(projectile.Center.X - 10, projectile.Center.Y), Vector2.Zero, ModContent.ProjectileType<DarksunTornado>(), dam, 0, projectile.owner);
					if (p.WithinBounds(Main.maxProjectiles))
					{
						Main.projectile[p].originalDamage = dam;
					}
				}
				if (CalamityUtils.CountProjectiles(ModContent.ProjectileType<DarksunTornado>()) < 2)
				CalamityUtils.ProjectileRain(projectile.GetSource_FromAI(), target.Center, 300, 20, -500, -800, 10, ProjectileType<NanoFlare>(), dam, 0, projectile.owner);
			}
			if (modPlayer.godfather && projectile.type == ProjectileType<CosmicBlast>())
            {
				Main.player[projectile.owner].Heal(5);
			}
			if (modPlayer.crystalconflict && projectile.type == ProjectileType<CryonicShield>())
			{
				target.AddBuff(BuffType<GodSlayerInferno>(), 60);
				if (modPlayer.tvo)
				{
					target.AddBuff(BuffType<GlacialState>(), 60);
				}
			}
		}

		public override void OnKill(Projectile projectile, int timeLeft)
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
					p.DamageType = GetInstance<RogueDamageClass>();
					if (p.whoAmI.WithinBounds(Main.maxProjectiles))
					{
						p.originalDamage = 20;
					}

				}
			}
        }

        public override Color? GetAlpha(Projectile projectile, Color lightColor)
        {
			Player player = Main.player[projectile.owner];
			CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();
			if (modPlayer.crystalconflict)
			{
				if (projectile.type == ProjectileType<CryonicShield>())
				return Color.Magenta;
				if (uniproj && (projectile.type == ProjectileType<GalileosPlanet>() 
					|| projectile.type == ProjectileType<CosmicBlast>()
					|| projectile.type == ProjectileType<EndoIceShard>()))
				return Color.HotPink;
				return null;
			}
			if (modPlayer.tvo && tvoproj)
            {
				if (projectile.type == ProjectileType<JewelSpike>())
                {
					return Color.Tan;
                }
				return null;
            }
			if (modPlayer.tvo && projectile.type == ProjectileType<EclipseMirrorBurst>())
            {
				projectile.damage = 1000000;
				return Color.LightBlue;
            }
            if ((!Main.dayTime || BossRushEvent.BossRushActive) && (projectile.type == ProjectileType<HolyBlast>() || projectile.type == ProjectileType<HolyBomb>() || projectile.type == ProjectileType<HolyFire>() || projectile.type == ProjectileType<HolyFire2>() || projectile.type == ProjectileType<HolyFlare>() || projectile.type == ProjectileType<MoltenBlob>() || projectile.type == ProjectileType<MoltenBlast>()))
            {
                if (projectile.type == ProjectileType<HolyBlast>())
                {
                    return Color.DarkSlateBlue;
                }
                return Color.MediumPurple;
            }
            else
			{
				return null;
			}

		}
		public override bool PreDraw(Projectile projectile, ref Color lightColor)
		{
            if (projectile.type == ProjectileType<ViolenceThrownProjectile>())
            {
				return false;
			}
            if (projectile.type == ProjectileType<MurasamaSlash>())
            {
                return false;
            }
            return true;
		}
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<WulfwyrmHead>()))
            {
                NPC exc = Main.npc[CalRemixGlobalNPC.wulfyrm];
                if (projectile.type == ModContent.ProjectileType<ExcavatorShot>() && exc.ModNPC<WulfwyrmHead>().DeathCharge) // not even gonna bother iterating through npcs since literally no other entity uses this projectile
                {
                    hyperCharged = true;
                }
            }
        }

        public override void OnHitPlayer(Projectile projectile, Player target, Player.HurtInfo info)
        {
            if (hyperCharged)
                target.AddBuff(ModContent.BuffType<GlacialState>(), 50);
        }
    }
}