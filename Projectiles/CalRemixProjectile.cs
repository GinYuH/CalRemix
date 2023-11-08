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
using CalRemix.Retheme;
using CalRemix.Tiles.PlaguedJungle;
using System;
using Terraria.ID;
using CalRemix.Tiles;

namespace CalRemix
{
	public class CalRemixProjectile : GlobalProjectile
	{
		public bool nihilicArrow = false;
		public bool rogueclone = false;
		public bool tvoproj = false;
		public bool uniproj = false;
        public bool hyperCharged = false;
		public int eye = 0;
		public int bladetimer = 0;
        NPC exc;
        public override bool InstancePerEntity => true;
        public override void SetDefaults(Projectile projectile)
        {
			RethemeMaster.RethemeProjDefaults(projectile);
        }

        public override void AI(Projectile projectile)
        {
            Player player = Main.LocalPlayer;
            CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();
            bladetimer--;
            if (modPlayer.brimPortal && nihilicArrow && !projectile.minion && !projectile.sentry && projectile.velocity != Vector2.Zero)
                CalamityUtils.HomeInOnNPC(projectile, false, 2500, 10f, 1);
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
            if (projectile.type == ProjectileID.PureSpray)
            {
                PlagueToPureConvert((int)(projectile.position.X + projectile.width / 2) / 16, (int)(projectile.position.Y + projectile.height / 2) / 16, 2);
            }
            if (projectile.type == ProjectileID.CorruptSpray || projectile.type == ProjectileID.CrimsonSpray || projectile.type == ProjectileID.HallowSpray || projectile.type == ModContent.ProjectileType<AstralSpray>() || projectile.type == ProjectileID.MushroomSpray)
            {
                PlagueToNeutralConvert((int)(projectile.position.X + projectile.width / 2) / 16, (int)(projectile.position.Y + projectile.height / 2) / 16, 2);
            }
            if (ModLoader.TryGetMod("NoxusBoss", out Mod nox))
            {
                int noxType = nox.Find<ModProjectile>("NoxusSprayerGas").Type;
                int i = (int)(projectile.position.X + projectile.width / 2) / 16;
                int j = (int)(projectile.position.Y + projectile.height / 2) / 16;
                if (projectile.type == noxType)
                {
                    for (int k = i - 4; k <= i + 4; k++)
                    {
                        for (int l = j - 4; l <= j + 4; l++)
                        {
                            if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < Math.Sqrt(4 * 4 + 4 * 4))
                            {
                                int type = Main.tile[k, l].TileType;
                                if (type == ModContent.TileType<MeldGunkPlaced>())
                                {
                                    Main.tile[k, l].TileType = TileID.Stone;
                                    WorldGen.SquareTileFrame(k, l, true);
                                }
                            }
                        }
                    }
                }
            }
            if (projectile.minion || projectile.sentry || projectile.hostile || !projectile.friendly)
                return;
            if (modPlayer.pearl)
            {
                CalamityUtils.HomeInOnNPC(projectile, false, 320, projectile.velocity.Length(), 1);
            }
            eye++;
            if (modPlayer.astralEye && eye % 120 == 0 && eye > 0 && projectile.type != ProjectileType<HomingAstralFireball>())
            {
                Projectile.NewProjectile(projectile.GetSource_FromAI(), projectile.Center, projectile.velocity * 0.75f, ProjectileType<HomingAstralFireball>(), 10, 0, projectile.owner);
            }
        }

        public static void PlagueToPureConvert(int i, int j, int size = 4)
        {
            for (int k = i - size; k <= i + size; k++)
            {
                for (int l = j - size; l <= j + size; l++)
                {
                    if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < Math.Sqrt(size * size + size * size))
                    {
                        int type = Main.tile[k, l].TileType;
                        int wall = Main.tile[k, l].WallType;

                        if (type == ModContent.TileType<PlaguedGrass>())
                        {
                            Main.tile[k, l].TileType = TileID.JungleGrass;
                            WorldGen.SquareTileFrame(k, l, true);
                        }
                        if (type == ModContent.TileType<PlaguedMud>())
                        {
                            Main.tile[k, l].TileType = TileID.Mud;
                            WorldGen.SquareTileFrame(k, l, true);
                        }
                        if (type == ModContent.TileType<Sporezol>())
                        {
                            Main.tile[k, l].TileType = TileID.Copper;
                            WorldGen.SquareTileFrame(k, l, true);
                        }
                        if (type == ModContent.TileType<PlaguedStone>())
                        {
                            Main.tile[k, l].TileType = TileID.Stone;
                            WorldGen.SquareTileFrame(k, l, true);
                        }
                        if (wall == ModContent.WallType<PlaguedMudWall>() || wall == ModContent.WallType<PlaguedMudWallSafe>())
                        {
                            Main.tile[k, l].WallType = WallID.MudUnsafe;
                            WorldGen.SquareWallFrame(k, l, true);
                        }
                        if (type == ModContent.TileType<PlaguedHive>())
                        {
                            Main.tile[k, l].TileType = TileID.Hive;
                            WorldGen.SquareTileFrame(k, l, true);
                        }
                        if (type == ModContent.TileType<PlaguedSilt>())
                        {
                            Main.tile[k, l].TileType = TileID.Silt;
                            WorldGen.SquareTileFrame(k, l, true);
                        }
                        if (type == ModContent.TileType<PlaguedSand>())
                        {
                            Main.tile[k, l].TileType = TileID.Sand;
                            WorldGen.SquareTileFrame(k, l, true);
                        }
                        if (type == ModContent.TileType<PlaguedPipe>())
                        {
                            Main.tile[k, l].TileType = TileID.RichMahogany;
                            WorldGen.SquareTileFrame(k, l, true);
                        }
                        if (wall == ModContent.WallType<PlaguedPipeWall>())
                        {
                            Main.tile[k, l].WallType = WallID.RichMaogany;
                            WorldGen.SquareWallFrame(k, l, true);
                        }
                        if (wall == ModContent.WallType<PlaguedHiveWall>())
                        {
                            Main.tile[k, l].WallType = WallID.HiveUnsafe;
                            WorldGen.SquareWallFrame(k, l, true);
                        }
                        if (type == ModContent.TileType<PlaguedStone>())
                        {
                            Main.tile[k, l].TileType = TileID.Stone;
                            WorldGen.SquareTileFrame(k, l, true);
                        }
                        if (wall == ModContent.WallType<PlaguedVineWall>() || wall == ModContent.WallType<PlaguedVineWallSafe>())
                        {
                            Main.tile[k, l].WallType = WallID.GrassUnsafe;
                            WorldGen.SquareWallFrame(k, l, true);
                        }
                        if (wall == ModContent.WallType<PlaguedStoneWall>() || wall == ModContent.WallType<PlaguedStoneWallSafe>())
                        {
                            Main.tile[k, l].WallType = WallID.Stone;
                            WorldGen.SquareWallFrame(k, l, true);
                        }
                        if (type == ModContent.TileType<PlaguedClay>())
                        {
                            Main.tile[k, l].TileType = TileID.ClayBlock;
                            WorldGen.SquareTileFrame(k, l, true);
                        }
                    }
                }
            }
        }


        public static void PlagueToNeutralConvert(int i, int j, int size = 4)
        {
            for (int k = i - size; k <= i + size; k++)
            {
                for (int l = j - size; l <= j + size; l++)
                {
                    if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < Math.Sqrt(size * size + size * size))
                    {
                        int type = Main.tile[k, l].TileType;
                        int wall = Main.tile[k, l].WallType;

                        if (type == ModContent.TileType<PlaguedMud>())
                        {
                            Main.tile[k, l].TileType = TileID.Mud;
                            WorldGen.SquareTileFrame(k, l, true);
                        }
                        if (type == ModContent.TileType<Sporezol>())
                        {
                            Main.tile[k, l].TileType = TileID.Copper;
                            WorldGen.SquareTileFrame(k, l, true);
                        }
                        if (wall == ModContent.WallType<PlaguedMudWall>() || wall == ModContent.WallType<PlaguedMudWallSafe>())
                        {
                            Main.tile[k, l].WallType = WallID.MudUnsafe;
                            WorldGen.SquareWallFrame(k, l, true);
                        }
                        if (type == ModContent.TileType<PlaguedHive>())
                        {
                            Main.tile[k, l].TileType = TileID.Hive;
                            WorldGen.SquareTileFrame(k, l, true);
                        }
                        if (type == ModContent.TileType<PlaguedSilt>())
                        {
                            Main.tile[k, l].TileType = TileID.Silt;
                            WorldGen.SquareTileFrame(k, l, true);
                        }
                        if (wall == ModContent.WallType<PlaguedHiveWall>())
                        {
                            Main.tile[k, l].WallType = WallID.HiveUnsafe;
                            WorldGen.SquareWallFrame(k, l, true);
                        }
                        if (type == ModContent.TileType<PlaguedClay>())
                        {
                            Main.tile[k, l].TileType = TileID.ClayBlock;
                            WorldGen.SquareTileFrame(k, l, true);
                        }
                    }
                }
            }
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[projectile.owner];
			CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();
			var source = projectile.GetSource_FromThis();
			if (modPlayer.tvo && CalamityUtils.CountProjectiles(ProjectileType<PlagueSeeker>()) > 3 && projectile.type == ProjectileType<PlagueSeeker>())
			{
				projectile.active = false;
			}
			if (modPlayer.arcanumHands && projectile.type != ProjectileType<ArmofAgony>() && CalamityUtils.CountProjectiles(ProjectileType<ArmofAgony>()) < 8)
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
				if (CalamityUtils.CountProjectiles(ProjectileType<JewelSpike>()) < 3)
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
				int p = Projectile.NewProjectile(projectile.GetSource_FromThis(), new Vector2(projectile.Center.X - 10, projectile.Center.Y), Vector2.Zero, ProjectileType<DarksunTornado>(), 20000, 0, Main.LocalPlayer.whoAmI);
				if (p.WithinBounds(Main.maxProjectiles))
				{
					Main.projectile[p].originalDamage = 20000;
				}
			}
			if (modPlayer.tvo && projectile.type != ProjectileType<DarksunTornado>() && projectile.type != ProjectileType<NanoFlare>())
			{
				int dam = (int)(projectile.damage * 0.2f);
				if (CalamityUtils.CountProjectiles(ProjectileType<DarksunTornado>()) < 3)
				{
					int p = Projectile.NewProjectile(projectile.GetSource_FromThis(), new Vector2(projectile.Center.X - 10, projectile.Center.Y), Vector2.Zero, ProjectileType<DarksunTornado>(), dam, 0, projectile.owner);
					if (p.WithinBounds(Main.maxProjectiles))
					{
						Main.projectile[p].originalDamage = dam;
					}
				}
				if (CalamityUtils.CountProjectiles(ProjectileType<DarksunTornado>()) < 2)
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
            if (modPlayer.wormMeal && projectile.DamageType == DamageClass.Summon)
            {
                if (Main.rand.NextBool(20))
                {
                    target.AddBuff(BuffID.Confused, 480);
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
            return RethemeMaster.RethemeProjAlpha(projectile);
		}
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (NPC.AnyNPCs(NPCType<WulfwyrmHead>()))
            {
                NPC exc = Main.npc[CalRemixGlobalNPC.wulfyrm];
                if (projectile.type == ProjectileType<ExcavatorShot>() && exc.ModNPC<WulfwyrmHead>().DeathCharge) // not even gonna bother iterating through npcs since literally no other entity uses this projectile
                {
                    hyperCharged = true;
                }
            }
        }
        public override void OnHitPlayer(Projectile projectile, Player target, Player.HurtInfo info)
        {
            if (hyperCharged)
                target.AddBuff(BuffType<GlacialState>(), 50);
        }
    }
}