using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.NPCs.SunkenSea;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.NPCs.Abyss;
using CalamityMod.Projectiles.Boss;
using CalamityMod.Projectiles.Summon;
using CalamityMod.Particles;
using CalRemix.Projectiles;
using CalRemix.Buffs;
using CalRemix.NPCs;
using CalRemix.NPCs.Bosses;
using Terraria.ModLoader.IO;
using System.Collections.Generic;
using CalamityMod.Items.PermanentBoosters;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.UI.CalamitasEnchants;
using CalamityMod.Items;

namespace CalRemix
{
	public class CalRemixPlayer : ModPlayer
	{
		public bool earthEnchant;
		public bool amongusEnchant;
		public bool brimPortal;
		public bool arcanumHands;
		public bool marnite;
		public bool roguebox;
		public int eclipseaura = -1;
		public int marnitetimer = 1200;
		public bool soldier;
		public bool astEffigy;
		public bool halEffigy;
		public bool nothing;
		public bool miragel;
		public bool godfather;
		public bool tvo;
		public bool nuclegel;
		public bool assortegel;
		public bool amalgel;
		public bool verboten;
		public bool cosmele;
		public bool earthele;
		public bool crystalconflict;
		public bool moonFist;
		public bool cursed;
		public bool cart;
		public bool tvohide;
		public Particle ring;
		public Particle ring2;
		public Particle aura;
		public bool ZoneLife;
		public float cosdam = 0;

		public int[] MinionList =
		{
			ModContent.ProjectileType<PlantSummon>(),
			ModContent.ProjectileType<AtlasSoldier>(),
			ModContent.ProjectileType<CosmilampMinion>(),
			ModContent.ProjectileType<FieryDraconid>(),
			ModContent.ProjectileType<SepulcherMinion>(),
			ModContent.ProjectileType<CosmicEnergySpiral>(),
			ModContent.ProjectileType<EndoCooperBody>(),
			ModContent.ProjectileType<MagicUmbrella>()
		};

		public int[] abnormalEnemyList = // immune to effects like Moon Fist's instant kill
		{
			ModContent.NPCType<SignalDrone>(),
			ModContent.NPCType<DerellectPlug>(),
			ModContent.NPCType<LifeSlime>()
		};

		public override void ProcessTriggers(TriggersSet triggersSet)
		{
			if (CalamityMod.CalamityKeybinds.SpectralVeilHotKey.JustPressed && roguebox)
			{
				if (!Player.HasCooldown(EclipseAuraCooldown.ID) && Player.GetModPlayer<CalRemixPlayer>().eclipseaura <= -1)
				{
					Player.GetModPlayer<CalRemixPlayer>().eclipseaura = 300;
				}
			}
		}
        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
        {
            if (eclipseaura > 0)
            {
				return false;
            }
			if (cursed)
			{ 
            damage = (int)((float)damage * 1.9f);
			}
			

			return true;
        }
        public override void PreUpdate()
        {
            SpawnPhantomHeart();
			/*
            if (Main.LocalPlayer.HeldItem.GetGlobalItem<CalamityGlobalItem>().AppliedEnchantment.Value.Equals(156))
            {
                amongusEnchant = true;
            }
            else
            {
                amongusEnchant = false;
            }
            if (Main.LocalPlayer.HeldItem.GetGlobalItem<CalamityGlobalItem>().AppliedEnchantment.Value.Equals(157))
            {
                earthEnchant = true;
            }
            else
            {
                earthEnchant = false;
            }*/
        }
        public override void PostUpdateMiscEffects()
		{
			CalamityPlayer calplayer = Main.LocalPlayer.GetModPlayer<CalamityPlayer>();
			if (cart)
			{
				for (int i = 0; i < MinionList.Length; i++)
				{
					if (Main.LocalPlayer.ownedProjectileCounts[MinionList[i]] > 0)
					{
						Main.LocalPlayer.maxMinions += Player.ownedProjectileCounts[MinionList[i]];

					}
				}
			}
			if (cosdam > 0.3f)
            {
				cosdam = 0.3f;
            }
			if (crystalconflict && cosdam > 0)
            {
				Main.LocalPlayer.GetDamage<GenericDamageClass>() += cosdam;
			}
			if (!crystalconflict)
            {
				cosdam = 0;
            }
			if (godfather)
            {
				calplayer.externalAbyssLight = 10;
				Main.LocalPlayer.breath = Main.LocalPlayer.breathMax;
            }
            #region Eclipse Aura
            if (ring2 != null)
			{
				ring2.Position = Player.Center;
				ring2.Velocity = Player.velocity;
				ring2.Scale *= 1.05f;
				ring2.Time += 1;
			}
			if (eclipseaura != -1)
            {
				Color outerring = tvo ? Color.LightPink : Color.Yellow;
				aura = new StrongBloom(Player.Center, Player.velocity, Color.Purple * 0.6f, 1f + Main.rand.NextFloat(0f, 1.5f) * 1.5f, 40);
				ring = new BloomRing(Player.Center, Player.velocity, outerring * 0.4f, 1.5f, 40);
				if (ring != null)
				{
					ring.Position = Player.Center;
					ring.Velocity = Player.velocity;
					ring.Time = 0;
				}
				if (aura != null)
				{
					aura.Position = Player.Center;
					aura.Velocity = Player.velocity;
					aura.Time = 0;
				}
				for (int i = 0; i < Main.maxNPCs; i++)
                {
					NPC target = Main.npc[i];
					if (Player.Center.Distance(target.Center) < 100 && !target.friendly)
                    {
						int dam = tvo ? 300 : 200;
						target.StrikeNPC(dam, 0, 0);
                    }
                }
				if (eclipseaura % 10 == 0)
				{
					GeneralParticleHandler.SpawnParticle(aura);
					GeneralParticleHandler.SpawnParticle(ring);
				}
				eclipseaura--;
            }
			else
			{
				if (ring != null)
				{
					ring.Kill();
				}
				if (aura != null)
				{
					aura.Kill();
				}
			}
			if (eclipseaura == 0)
			{
				int duration = tvo ? 30 : 20;
				Main.LocalPlayer.AddCooldown(EclipseAuraCooldown.ID, CalamityUtils.SecondsToFrames(duration));
			}
            #endregion
            if (halEffigy)
			{
				Player.moveSpeed += 0.25f;
				Player.GetCritChance<GenericDamageClass>() += 25;
				Player.GetDamage<GenericDamageClass>() += 0.25f;
				Player.statDefense += 20;
				Player.endurance += 0.08f;
				Player.AddCooldown(CalamityMod.Cooldowns.ChaosState.ID, 20);
			}
			if (astEffigy)
			{
				if (Player.InSpace())
				{
					Player.gravity = Player.defaultGravity;
					if (Player.wet)
					{
						if (Player.honeyWet)
							Player.gravity = 0.1f;
						else if (Player.merman)
							Player.gravity = 0.3f;
						else
							Player.gravity = 0.2f;
					}
				}
			}
			if (tvo && calplayer.adrenalineModeActive)
			{
				Main.LocalPlayer.GetDamage<GenericDamageClass>() *= 3;
			}
			if (tvo)
			{
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					NPC target = Main.npc[i];
					if (!target.active || !target.Hitbox.Intersects(Utils.CenteredRectangle(Main.MouseWorld, new Vector2(35f, 62f))) || target.immortal || target.dontTakeDamage || target.townNPC || NPCID.Sets.ActsLikeTownNPC[target.type] || NPCID.Sets.CountsAsCritter[target.type])
						continue;

					target.AddBuff(ModContent.BuffType<DemonFlames>(), 1200);
				}
			}
            #region stealth cuts
            if (tvo) //Verboten one
			{
				StealthCut(0.995f);
			}
			else if (godfather) //Godfather
			{
				StealthCut(0.01f);
			}
			else if (calplayer.sponge) //Sponge
			{
				StealthCut(0.02f);
			}
			else if (calplayer.absorber) //Absorber
			{
				StealthCut(0.03f);
			}
			else if (miragel) //Mirage Jelly & Grand Gelatin
            {
				StealthCut(0.05f);
            }
            #endregion
        }
        public override void ResetEffects()
		{

            earthEnchant = false;
			amongusEnchant = false;
			brimPortal = false;
			arcanumHands = false;
			marnite = false;
			roguebox = false;
			soldier = false;
			marnitetimer = 0;
			astEffigy = false;
			halEffigy = false;
			nothing = false;
			miragel = false;
			nuclegel = false;
			assortegel = false;
			amalgel = false;
			godfather = false;
			cosmele = false;
			earthele = false;
			crystalconflict = false;
			moonFist = false;
			cursed = false;
			tvo = false;
			cart = false;
			tvohide = false;
			if (astEffigy)
				Player.statLifeMax2 = (int)(Player.statLifeMax2 * 1.5);
		}
        public override void GetDyeTraderReward(List<int> rewardPool)
        {
			if (CalamityMod.DownedBossSystem.downedProvidence)
			{
				if (CalamityMod.DownedBossSystem.downedProvidence && !Player.Calamity().eBerry)
				{
					rewardPool.Clear();
				}
				rewardPool.Add(ModContent.ItemType<Elderberry>());
			}
        }
		public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
		{
			if (amongusEnchant && crit)
			{
                CombatText.NewText(Main.LocalPlayer.getRect(), Color.Red, damage/7);
                Main.LocalPlayer.statLife -= damage / 7;
                SoundEngine.PlaySound(new SoundStyle($"{nameof(CalRemix)}/Sounds/Stab"));
                damage = (int)((float)damage * 2.5f);
            }
            if (earthEnchant)
            {
            }
            if (moonFist && item.DamageType == DamageClass.Melee)
			{
				target.AddBuff(ModContent.BuffType<Nightwither>(), 300, false);
				if (target.boss == false && !CalamityLists.bossMinionList.Contains(target.type) && !abnormalEnemyList.Contains(target.type)) {				
					if(Main.rand.NextBool(10))
						{
						target.active = false;
						}		
					}
				}
			if (moonFist && item.DamageType == ModContent.GetInstance<TrueMeleeDamageClass>()); //simply uses another if statement to fix an error they don't give a shit about
				{
				Player.AddBuff(ModContent.BuffType<MoonfistBuff>(), 1000);
				target.AddBuff(ModContent.BuffType<Nightwither>(), 500, false);
				if (target.boss == false && !CalamityLists.bossMinionList.Contains(target.type) && !abnormalEnemyList.Contains(target.type)) {				
					if(Main.rand.NextBool(10))
						{
						target.active = false;
						}		
					}
				}
			}

		public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
		{
            if (amongusEnchant && crit)
            {
                CombatText.NewText(Main.LocalPlayer.getRect(), Color.Red, damage / 7);
                Main.LocalPlayer.statLife -= damage / 7;
                SoundEngine.PlaySound(new SoundStyle($"{nameof(CalRemix)}/Sounds/Stab"));
                damage = (int)((float)damage * 2.5f);
            }
			if (moonFist && proj.DamageType == DamageClass.Melee)
			{
					target.AddBuff(ModContent.BuffType<Nightwither>(), 300, false);
					if (target.boss == false && !CalamityLists.bossMinionList.Contains(target.type)) {				
					if(Main.rand.NextBool(10))
					{
					target.active = false;
					}		
				}
			}
        }

		public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
			CalamityPlayer calplayer = Main.LocalPlayer.GetModPlayer<CalamityPlayer>();
			if (godfather)
			{
				if (npc.type == NPCID.BlueJellyfish || npc.type == NPCID.PinkJellyfish || npc.type == NPCID.GreenJellyfish ||
					npc.type == NPCID.FungoFish || npc.type == NPCID.BloodJelly || npc.type == NPCID.AngryNimbus || npc.type == NPCID.GigaZapper ||
					npc.type == NPCID.MartianTurret || npc.type == ModContent.NPCType<Stormlion>() || npc.type == ModContent.NPCType<GhostBell>() || npc.type == ModContent.NPCType<BoxJellyfish>())
					calplayer.contactDamageReduction += 1;
				var source = Main.LocalPlayer.GetSource_OnHurt(npc);
				Vector2 playercenter = Main.LocalPlayer.Center;
				Vector2 spawnvector = new Vector2(playercenter.X - 4, playercenter.Y - 4);
				Projectile.NewProjectile(source, spawnvector, Vector2.Zero, ModContent.ProjectileType<CalamityMod.Projectiles.Melee.CosmicIceBurst>(), 33000, 0, Main.LocalPlayer.whoAmI);
			}
		}
		public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
		{
			CalamityPlayer calplayer = Main.LocalPlayer.GetModPlayer<CalamityPlayer>();
			if (godfather)
			{
				if (proj.type == ProjectileID.MartianTurretBolt || proj.type == ProjectileID.GigaZapperSpear || proj.type == ProjectileID.CultistBossLightningOrbArc || proj.type == ProjectileID.VortexLightning || proj.type == ModContent.ProjectileType<DestroyerElectricLaser>() ||
					proj.type == ProjectileID.BulletSnowman || proj.type == ProjectileID.BulletDeadeye || proj.type == ProjectileID.SniperBullet || proj.type == ProjectileID.VortexLaser)
					calplayer.projectileDamageReduction += 1;
			}
		}
        public void SpawnPhantomHeart()
        {
            if (Main.rand.NextBool(6000) && Player.ZoneDungeon && DownedBossSystem.downedPolterghast && !Player.GetModPlayer<CalamityPlayer>().pHeart)
            {
                Projectile.NewProjectile(Player.GetSource_FromThis(), new Vector2(Main.rand.Next((int)Player.Center.X - Main.screenWidth, (int)Player.Center.X + Main.screenWidth), Player.Center.Y + Main.screenHeight), new Vector2((float)Main.rand.Next(-400, 401) * 0.01f, (float)Main.rand.Next(-1000, -701) * 0.01f), ModContent.ProjectileType<FallingPhantomHeart>(), 0, 0, Player.whoAmI);
            }
            else if (Main.rand.NextBool(10800) && Player.ZoneDungeon && DownedBossSystem.downedPolterghast && Player.GetModPlayer<CalamityPlayer>().pHeart)
            {
                Projectile.NewProjectile(Player.GetSource_FromThis(), new Vector2(Main.rand.Next((int)Player.Center.X - Main.screenWidth, (int)Player.Center.X + Main.screenWidth), Player.Center.Y + Main.screenHeight), new Vector2((float)Main.rand.Next(-400, 401) * 0.01f, (float)Main.rand.Next(-1000, -701) * 0.01f), ModContent.ProjectileType<FallingPhantomHeart>(), 0, 0, Player.whoAmI);
            }
        }
        public void StealthCut(float amt)
        {
            CalamityPlayer calplayer = Main.LocalPlayer.GetModPlayer<CalamityPlayer>();
            int sstealth = (int)(calplayer.rogueStealthMax * amt);
            if (calplayer.rogueStealth == 0)
            {
                calplayer.rogueStealth = sstealth;
            }
        }
    }
}