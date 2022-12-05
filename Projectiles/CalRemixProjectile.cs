using Terraria.ModLoader;
using CalamityMod;
using Terraria;
using Terraria.DataStructures; 
using Microsoft.Xna.Framework;
using CalRemix.Projectiles;
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
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using CalamityMod.Projectiles.Melee.Spears;
using Terraria.GameContent;
using System;
using Terraria.Graphics.Shaders;
using System.IO;

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
        private int frameX;
        private int frameY;
        NPC exc;


        private int CurrentFrame
        {
            get
            {
                return frameX * 7 + frameY;
            }
            set
            {
                frameX = value / 7;
                frameY = value % 7;
            }
        }

        internal PrimitiveTrail StreakDrawer;
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
            else if (projectile.type == ProjectileType<NadirSpear>())
            {
                TextureAssets.Projectile[projectile.type] = ModContent.Request<Texture2D>("CalRemix/Resprites/NadirSpear");
            }
            else if (projectile.type == ProjectileType<VoidEssence>())
            {
				TextureAssets.Projectile[projectile.type] = ModContent.Request<Texture2D>("CalRemix/Resprites/VoidEssence");
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
            if (projectile.type == ProjectileType<MurasamaSlash>())
            {
                if (projectile.frameCounter % 3 == 0)
                {
                    CurrentFrame++;
                    if (frameX >= 2)
                    {
                        CurrentFrame = 0;
                    }
                }
            }
        }

        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
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
				target.AddBuff(BuffType<DemonFlames>(), 180);
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
				target.AddBuff(BuffType<DemonFlames>(), 180);
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
			if (modPlayer.tvo && projectile.type != ProjectileType<DarksunTornado>() && projectile.type != ProjectileType<NanoFlare>() && projectile.type != ProjectileType<UnstableSpark>())
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
				if (CalamityUtils.CountProjectiles(ModContent.ProjectileType<UnstableSpark>()) < 3)
				{
					int e = Projectile.NewProjectile(projectile.GetSource_FromAI(), projectile.Center, new Vector2(Main.rand.Next(-20, 20), Main.rand.Next(5, 10)), ProjectileType<UnstableSpark>(), dam, 0, projectile.owner);
					if (e.WithinBounds(Main.maxProjectiles))
					{
						Main.projectile[e].originalDamage = dam;
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
					target.AddBuff(BuffType<ExoFreeze>(), 60);
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
		public override void PostDraw(Projectile projectile, Color lightColor)
		{
            if (projectile.type == ProjectileType<MurasamaSlash>())
            {
                Texture2D value = ModContent.Request<Texture2D>("CalRemix/Resprites/MurasamaSlash").Value;
                Vector2 origin = value.Size() / new Vector2(2f, 7f) * 0.5f;
                Rectangle value2 = value.Frame(2, 7, frameX, frameY);
                Main.EntitySpriteDraw(effects: (projectile.spriteDirection != 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, texture: value, position: projectile.Center - Main.screenPosition, sourceRectangle: value2, color: Color.White, rotation: projectile.rotation, origin: origin, scale: projectile.scale, worthless: 0);
            }
			if (projectile.type == ProjectileType<ViolenceThrownProjectile>())
            {
                if (StreakDrawer == null)
                {
                    StreakDrawer = new PrimitiveTrail(PrimitiveWidthFunction, PrimitiveColorFunction, null, GameShaders.Misc["CalamityMod:TrailStreak"]);
                }

                GameShaders.Misc["CalamityMod:TrailStreak"].SetShaderTexture(ModContent.Request<Texture2D>("CalRemix/ExtraTextures/FabstaffStreak"));
                Texture2D value = ModContent.Request<Texture2D>("CalRemix/Resprites/Violence").Value;
                Vector2[] array = (Vector2[])projectile.oldPos.Clone();
                Vector2 vector = (projectile.rotation - MathF.PI / 2f).ToRotationVector2();
                if (Main.player[projectile.owner].channel)
                {
                    array[0] += vector * -12f;
                    array[1] = array[0] - (projectile.rotation + MathF.PI / 4f).ToRotationVector2() * Vector2.Distance(array[0], array[1]);
                }

                for (int i = 0; i < array.Length; i++)
                {
                    array[i] -= (projectile.oldRot[i] + MathF.PI / 4f).ToRotationVector2() * projectile.height * 0.5f;
                }

                if (projectile.ai[0] > (float)projectile.oldPos.Length)
                {
                    StreakDrawer.Draw(array, projectile.Size * 0.5f - Main.screenPosition, 88);
                }

                Vector2 position = projectile.Center - Main.screenPosition;
                for (int j = 0; j < 6; j++)
                {
                    float num = projectile.oldRot[j] - MathF.PI / 2f;
                    if (Main.player[projectile.owner].channel)
                    {
                        num += 0.2f;
                    }

                    Color color = Color.Lerp(lightColor, Color.Transparent, 1f - (float)Math.Pow(Utils.GetLerpValue(0f, 6f, j), 1.4)) * projectile.Opacity;
                    Main.EntitySpriteDraw(value, position, null, color, num, value.Size() * 0.5f,projectile.scale, SpriteEffects.None, 0);
                }
            }
        }


        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            exc = Main.npc[1];
            if (projectile.type == ModContent.ProjectileType<ExcavatorShot>() && exc.ModNPC<WulfrumExcavatorHead>().DeathCharge) // not even gonna bother iterating through npcs since literally no other entity uses this projectile
            {
                hyperCharged = true;
            }
        }

        public override void OnHitPlayer(Projectile projectile, Player target, int damage, bool crit)
        {
            if (hyperCharged)
                target.AddBuff(ModContent.BuffType<ExoFreeze>(), 50);
        }

        internal float PrimitiveWidthFunction(float completionRatio)
        {
            float num = MathHelper.SmoothStep(0f, 1f, Utils.GetLerpValue(0.01f, 0.04f, completionRatio));
            float num2 = (float)Math.Pow(Utils.GetLerpValue(1f, 0.04f, completionRatio), 0.9);
            return (float)Math.Pow(num * num2, 0.1) * 30f;
        }
        internal Color PrimitiveColorFunction(float completionRatio)
        {
            float amount = (float)Math.Cos(Main.GlobalTimeWrappedHourly * -9f + completionRatio * 6f + 2f) * 0.5f + 0.5f;
            amount = MathHelper.Lerp(0.15f, 0.75f, amount);
            Color value = Color.Lerp(Color.Lerp(new Color(255, 145, 115), new Color(113, 0, 159), amount), Color.DarkRed, 0.5f);
            Color value2 = new Color(255, 145, 115);
            return Color.Lerp(value, value2, (float)Math.Pow(completionRatio, 1.2)) * (float)Math.Pow(1f - completionRatio, 1.1);
        }
    }
}