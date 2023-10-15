using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.NPCs.SunkenSea;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.NPCs.Abyss;
using CalamityMod.Projectiles.Boss;
using CalamityMod.Projectiles.Summon;
using CalamityMod.Projectiles.Summon.Umbrella;
using CalamityMod.Projectiles.Summon.SmallAresArms;
using CalamityMod.Projectiles.Ranged;
using CalamityMod.Projectiles.Typeless;
using CalamityMod.Projectiles.Boss;
using CalamityMod.Projectiles.Pets;
using CalamityMod.Particles;
using CalRemix.Projectiles;
using CalRemix.Projectiles.Weapons;
using CalRemix.Projectiles.Accessories;
using CalRemix.NPCs;
using CalRemix.NPCs.Bosses;
using System.Collections.Generic;
using CalamityMod.Items.PermanentBoosters;
using CalamityMod.Buffs.DamageOverTime;
using CalRemix.Items;
using CalamityMod.BiomeManagers;
using CalRemix.UI;
using CalamityMod.Items.Placeables.Furniture.CraftingStations;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Potions;
using CalRemix.Items.Materials;

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
        public bool dreamingGhost;
        public Particle ring;
		public Particle ring2;
		public Particle aura;
		public bool ZoneLife;
		public float cosdam = 0;
		public int VerbotenMode = 1;
		public int RecentChest = -1;
		public bool baroclaw;
		public bool ZonePlague;
        public bool ZonePlagueDesert;
        public Vector2 clawPosition = Vector2.Zero;
		public int[] MinionList =
		{
			ModContent.ProjectileType<PlantationStaffSummon>(),
			ModContent.ProjectileType<AtlasSoldier>(),
			ModContent.ProjectileType<CosmilampMinion>(),
			ModContent.ProjectileType<FieryDraconid>(),
			ModContent.ProjectileType<SepulcherMinion>(),
			ModContent.ProjectileType<CosmicEnergySpiral>(),
			ModContent.ProjectileType<EndoCooperBody>(),
			ModContent.ProjectileType<MagicUmbrella>(),
			ModContent.ProjectileType<SiriusMinion>(),
			ModContent.ProjectileType<SarosAura>()

		};

		public int[] abnormalEnemyList = // immune to effects like Moon Fist's instant kill
		{
			/*ModContent.NPCType<SignalDrone>(),
			ModContent.NPCType<DerellectPlug>(),*/
			ModContent.NPCType<LifeSlime>()
		};

		public int[] tvgNoFireList = // can't be fired by The Verboten Gun. extremely long because it has to block boss summons, pets, and world destroying projectiles
		{							// remind me to add a check for Fargo's that adds Grave Buster, City Buster, Galactic Reformer, and Universal Collapse to this list
			ModContent.ProjectileType<LeviathanSpawner>(),
            ModContent.ProjectileType<DeusRitualDrama>(),
            ModContent.ProjectileType<OverlyDramaticDukeSummoner>(),
            ModContent.ProjectileType<SCalRitualDrama>(),
            ModContent.ProjectileType<TerminusHoldout>(),
            ModContent.ProjectileType<GiantIbanRobotOfDoom>(),
            ModContent.ProjectileType<HolyBlast>(),
            ModContent.ProjectileType<Akato>(),
            ModContent.ProjectileType<Astrophage>(),
            ModContent.ProjectileType<Bear>(),
            ModContent.ProjectileType<BendyPet>(),
            ModContent.ProjectileType<BrimlingPet>(),
            ModContent.ProjectileType<ChibiiDoggo>(),
            ModContent.ProjectileType<DaawnlightSpiritOriginMinion>(),
            ModContent.ProjectileType<DannyDevitoPet>(),
            ModContent.ProjectileType<FlakPet>(),
            ModContent.ProjectileType<FoxPet>(),
            ModContent.ProjectileType<KendraPet>(),
            ModContent.ProjectileType<LadShark>(),
            ModContent.ProjectileType<LeviPet>(),
            ModContent.ProjectileType<LittleLightProj>(),
            ModContent.ProjectileType<MiniHiveMind>(),
            //ModContent.ProjectileType<PerforaMini>(), // bloody vein is too important to blacklist
            ModContent.ProjectileType<PineapplePetProj>(),
            ModContent.ProjectileType<PlaguebringerBab>(),
            ModContent.ProjectileType<RadiatorPet>(),
            ModContent.ProjectileType<RotomPet>(),
            ModContent.ProjectileType<SCalPet>(),
            ModContent.ProjectileType<Sparks>(),
            ModContent.ProjectileType<YharonSonPet>(),
            ModContent.ProjectileType<BobbitHead>(),
            ModContent.ProjectileType<SerpentsBiteHook>(),
            //ModContent.ProjectileType<AstralFallingSand>(),
            ModContent.ProjectileType<WulfrumHook>(),
            ModContent.ProjectileType<AstralSandgun>(),
            ModContent.ProjectileType<AeroExplosive>(),
            ModContent.ProjectileType<ExoskeletonPanel>(),
            ModContent.ProjectileType<AresGaussNukeProjectile>(), //firing the gauss nuke causes the explosion to become hostile, which is way too much. the explosion can still be fired though!
            ModContent.ProjectileType<SlimeCore>(),
            ModContent.ProjectileType<CriticalSlimeCore>(),
            ModContent.ProjectileType<CosmicConflict>(),
            ModContent.ProjectileType<EarthElementalMinion>(),
            ModContent.ProjectileType<AtlasSoldier>(),
            ProjectileID.Bomb,
			ProjectileID.StickyBomb,
			ProjectileID.BouncyBomb,
			ProjectileID.BombFish,
			ProjectileID.ScarabBomb,
			ProjectileID.DryBomb,
			ProjectileID.WetBomb,
			ProjectileID.HoneyBomb,
			ProjectileID.DirtBomb,
			ProjectileID.DirtStickyBomb,
			ProjectileID.BombSkeletronPrime,
			ProjectileID.Dynamite,
			ProjectileID.StickyDynamite,
			ProjectileID.BouncyDynamite,
			ProjectileID.Celeb2RocketExplosive,
			ProjectileID.Celeb2RocketExplosiveLarge,
			ProjectileID.ClusterRocketII,
			ProjectileID.ClusterGrenadeII,
			ProjectileID.ClusterFragmentsII,
			ProjectileID.WetRocket,
			ProjectileID.HoneyRocket,
			ProjectileID.LavaRocket,
			ProjectileID.WetGrenade,
			ProjectileID.WetMine,
			ProjectileID.LavaGrenade,
			ProjectileID.LavaMine,
			ProjectileID.HoneyGrenade,
			ProjectileID.HoneyMine,
			ProjectileID.RocketII,
			ProjectileID.RocketIV,
			ProjectileID.GrenadeII,
			ProjectileID.GrenadeIV,
			ProjectileID.MiniNukeRocketII,
			ProjectileID.MiniNukeGrenadeII,
			ProjectileID.MiniNukeMineII,
			ProjectileID.DryRocket,
			ProjectileID.DryGrenade,
			ProjectileID.DryMine,
			ProjectileID.ClusterSnowmanRocketII,
			ProjectileID.WetSnowmanRocket,
			ProjectileID.DrySnowmanRocket,
			ProjectileID.LavaSnowmanRocket,
			ProjectileID.HoneySnowmanRocket,
			ProjectileID.Explosives,
			ProjectileID.TreeGlobe,
			ProjectileID.WorldGlobe,
			ProjectileID.PortalGun,
			ProjectileID.PortalGunBolt,
			ProjectileID.PortalGunGate,
			ProjectileID.Gravestone,
			ProjectileID.Obelisk,
            ProjectileID.Headstone,
            ProjectileID.GraveMarker,
            ProjectileID.CrossGraveMarker,
			ProjectileID.RichGravestone1,
			ProjectileID.RichGravestone2,
			ProjectileID.RichGravestone3,
			ProjectileID.RichGravestone4,
			ProjectileID.RichGravestone5,
			ProjectileID.DirtBall,
			ProjectileID.SandBallFalling,
            ProjectileID.EbonsandBallFalling,
            ProjectileID.CrimsandBallFalling,
            ProjectileID.PearlSandBallFalling,
            ProjectileID.CopperCoinsFalling,
            ProjectileID.SilverCoinsFalling,
            ProjectileID.GoldCoinsFalling,
            ProjectileID.PlatinumCoinsFalling,
            ProjectileID.SandBallGun,
			ProjectileID.PearlSandBallGun,
            ProjectileID.CrimsandBallGun,
            ProjectileID.EbonsandBallGun,
            ProjectileID.HolyWater,
			ProjectileID.UnholyWater,
			ProjectileID.BloodWater,
			ProjectileID.Hook,
			ProjectileID.FlyingPiggyBank,
            ProjectileID.BlueFairy,
            ProjectileID.PinkFairy,
            ProjectileID.GreenFairy,
            ProjectileID.DualHookBlue,
            ProjectileID.DualHookRed,
            ProjectileID.PureSpray,
            ProjectileID.HallowSpray,
            ProjectileID.CorruptSpray,
            ProjectileID.MushroomSpray,
            ProjectileID.CrimsonSpray,
            ProjectileID.BabyEater,
            ProjectileID.BabySkeletronHead,
            ProjectileID.BabyHornet,
            ProjectileID.TikiSpirit,
            ProjectileID.PetLizard,
            ProjectileID.Parrot,
            ProjectileID.Truffle,
            ProjectileID.Sapling,
            ProjectileID.Wisp,
            ProjectileID.BabyDino,
            ProjectileID.EyeSpring,
            ProjectileID.BabySnowman,
            ProjectileID.Spider,
            ProjectileID.Squashling,
            ProjectileID.BatHook,
            ProjectileID.Bat,
            ProjectileID.Raven,
            ProjectileID.BlackCat,
            ProjectileID.WoodHook,
            ProjectileID.CursedSapling,
            ProjectileID.Puppy,
            ProjectileID.BabyGrinch,
            ProjectileID.ZephyrFish,
            ProjectileID.MiniMinotaur,
            ProjectileID.ViciousPowder,
            ProjectileID.VilePowder,
            ProjectileID.BabySnowman,
            ProjectileID.TendonHook,
            ProjectileID.ThornHook,
            ProjectileID.IlluminantHook,
            ProjectileID.WormHook,
            ProjectileID.BabyFaceMonster,
            ProjectileID.CrimsonHeart,
            ProjectileID.RopeCoil,
            ProjectileID.SilkRopeCoil,
            ProjectileID.VineRopeCoil,
            ProjectileID.WebRopeCoil,
            ProjectileID.BabySnowman,
            ProjectileID.LunarHookSolar,
            ProjectileID.LunarHookVortex,
            ProjectileID.LunarHookNebula,
            ProjectileID.LunarHookStardust,
            ProjectileID.WireKite,
            ProjectileID.ShellPileFalling,
            ProjectileID.LilHarpy,
            ProjectileID.FennecFox,
            ProjectileID.GlitteryButterfly,
            ProjectileID.BabyImp,
            ProjectileID.KingSlimePet,
            ProjectileID.EyeOfCthulhuPet,
            ProjectileID.EaterOfWorldsPet,
            ProjectileID.BrainOfCthulhuPet,
            ProjectileID.SkeletronPet,
            ProjectileID.QueenBeePet,
            ProjectileID.DestroyerPet,
            ProjectileID.TwinsPet,
            ProjectileID.SkeletronPrimePet,
            ProjectileID.PlanteraPet,
            ProjectileID.GolemFist,
            ProjectileID.DukeFishronPet,
            ProjectileID.LunaticCultistPet,
            ProjectileID.MoonLordPet,
            ProjectileID.FairyQueenPet,
            ProjectileID.PumpkingPet,
            ProjectileID.EverscreamPet,
            ProjectileID.IceQueenPet,
            ProjectileID.MartianPet,
            ProjectileID.DD2OgrePet,
            ProjectileID.DD2BetsyPet,
            ProjectileID.QueenSlimePet,
            ProjectileID.SquirrelHook,
            ProjectileID.QueenSlimeHook,
            ProjectileID.GemHookAmethyst,
            ProjectileID.GemHookTopaz,
            ProjectileID.GemHookSapphire,
            ProjectileID.GemHookEmerald,
            ProjectileID.GemHookRuby,
			ProjectileID.GemHookDiamond,
            ProjectileID.SkeletronHand,
            ProjectileID.CandyCaneHook,
            ProjectileID.ChristmasHook,
            ProjectileID.FishHook,
            ProjectileID.SlimeHook,
            ProjectileID.TrackHook,
            ProjectileID.AntiGravityHook,
            ProjectileID.StaticHook,
            ProjectileID.AmberHook
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
			if (CalRemixKeybinds.BaroClawHotKey.JustPressed && baroclaw && CalamityUtils.CountProjectiles(ModContent.ProjectileType<Claw>()) <= 0)
			{
				float XDist = 120;
				clawPosition = Main.MouseWorld;
				Projectile.NewProjectile(Player.GetSource_FromThis(), Main.MouseWorld + Vector2.UnitX * XDist, Vector2.Zero, ModContent.ProjectileType<Claw>(), 30, 0);
				int p = Projectile.NewProjectile(Player.GetSource_FromThis(), Main.MouseWorld - Vector2.UnitX * XDist, Vector2.Zero, ModContent.ProjectileType<Claw>(), 30, 0);
				Main.projectile[p].spriteDirection *= -1;
			}
		}
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)/* tModPorter Override ImmuneTo, FreeDodge or ConsumableDodge instead to prevent taking damage */
        {
            if (eclipseaura > 0)
            {
				modifiers.SourceDamage *= 0f;
            }
			if (cursed)
			{ 
				modifiers.SourceDamage *= 1.9f;
			}
			
        }

        public override void PreUpdate()
        {
            SpawnPhantomHeart();

			if (VerbotenMode >= 5)
			{
				VerbotenMode = 1;
			}

            /*if (Main.LocalPlayer.HeldItem.GetGlobalItem<CalamityMod.Items.CalamityGlobalItem>().AppliedEnchantment.Value.Equals(156))
            {
				Main.NewText("Polterc");
                amongusEnchant = true;
            }
            else
            {
                amongusEnchant = false;
            }
            /*if (Main.LocalPlayer.HeldItem.GetGlobalItem<CalamityGlobalItem>().AppliedEnchantment.Value.Equals(157))
            {
                earthEnchant = true;
            }
            else
            {
                earthEnchant = false;
            }*/
        }

        public override bool PreItemCheck()
        {
            /*if (Player.HeldItem.type == ItemID.MechanicalWorm) // has to be here or else derellect spawns 5 times. blame vanilla jank for this, THEY had to work around this problem
			{ 
                if (NPC.CountNPCS(ModContent.NPCType<DerellectBoss>()) >= 1)
				{
					Player.itemTime = 0;
					Player.itemAnimation = 0; // looks weird but has to be done or else you get stuck holding it forever
					return false;

				}
  				return true;                  
			}*/
			return true;
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
						target.StrikeNPC(target.CalculateHitInfo(dam, 0, knockBack: 0));
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

					target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 1200);
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
			//amongusEnchant = false;
			brimPortal = false;
			arcanumHands = false;
			marnite = false;
			roguebox = false;
            dreamingGhost = false;
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
			baroclaw = false;
			if (astEffigy)
				Player.statLifeMax2 = (int)(Player.statLifeMax2 * 1.5);
			if (Player.HeldItem != null && Player.HeldItem.type != ItemID.None)
			{
				if (!Player.HeldItem.Calamity().AppliedEnchantment.HasValue)
				{
					amongusEnchant = false;
				}
			}
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
		public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Item, consider using OnHitNPC instead */
		{
			if (amongusEnchant && hit.Crit)
			{
                CombatText.NewText(Main.LocalPlayer.getRect(), Color.Red, damageDone / 7);
                Main.LocalPlayer.statLife -= (int)MathHelper.Clamp(damageDone / 7, 0, Player.statLife - 1);
                SoundEngine.PlaySound(new SoundStyle($"{nameof(CalRemix)}/Sounds/Stab"));
				hit.SourceDamage = (int)(hit.SourceDamage * 2.5f);
            }
            if (earthEnchant)
            {
            }
            if (moonFist && item.DamageType == DamageClass.Melee)
			{
				target.AddBuff(ModContent.BuffType<Nightwither>(), 300, false);
				if (target.boss == false && !CalamityLists.bossMinionList.Contains(target.type) && !abnormalEnemyList.Contains(target.type)) 
				{				
					if (Main.rand.NextBool(10))
						{
							target.HitEffect(0, target.lifeMax * 22);
						}		
					}
				}
			}

		public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Projectile, consider using OnHitNPC instead */
		{
			if (amongusEnchant && hit.Crit)
			{
				CombatText.NewText(Main.LocalPlayer.getRect(), Color.Red, damageDone / 7);
				Main.LocalPlayer.statLife -= (int)MathHelper.Clamp(damageDone / 7, 0, Player.statLife - 1);
				SoundEngine.PlaySound(new SoundStyle($"{nameof(CalRemix)}/Sounds/Stab"));
				hit.SourceDamage = (int)(hit.SourceDamage * 2.5f);
			}
			if (moonFist && proj.DamageType == DamageClass.Melee)
			{
				target.AddBuff(ModContent.BuffType<Nightwither>(), 300, false);
				if (target.boss == false && !CalamityLists.bossMinionList.Contains(target.type)) 
				{				
					if(Main.rand.NextBool(10))
					{
						target.HitEffect(0, target.lifeMax * 22);
					}		
				}		
			}
        }

		public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
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
		public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
		{
			CalamityPlayer calplayer = Main.LocalPlayer.GetModPlayer<CalamityPlayer>();
			if (godfather)
			{
				if (proj.type == ProjectileID.MartianTurretBolt || proj.type == ProjectileID.GigaZapperSpear || proj.type == ProjectileID.CultistBossLightningOrbArc || proj.type == ProjectileID.VortexLightning || proj.type == ModContent.ProjectileType<DestroyerElectricLaser>() ||
					proj.type == ProjectileID.BulletSnowman || proj.type == ProjectileID.BulletDeadeye || proj.type == ProjectileID.SniperBullet || proj.type == ProjectileID.VortexLaser)
					calplayer.projectileDamageReduction += 1;
			}
		}
        public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
        {
            bool inWater = !attempt.inLava && !attempt.inHoney;
            int roll = Main.rand.Next(100);
            if (inWater && Player.ZoneJungle && Main.hardMode && roll >= 0 && roll <= 7)
            {
                itemDrop = ModContent.ItemType<Babilfish>();
            }
            if (inWater && Main.bloodMoon && Main.rand.NextBool(6))
            {
                itemDrop = ModContent.ItemType<GrandioseGland>();
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

		// excavator summon, some code adapted from thorium mimic summoning
		public override void UpdateAutopause() { RecentChest = Player.chest; }
        public override void PreUpdateBuffs() 
        {
			if(Main.netMode != NetmodeID.MultiplayerClient)
            {
				if (Player.chest == -1 && RecentChest >= 0 && Main.chest[RecentChest] != null)
				{
					int i = Main.chest[RecentChest].x;
					int j = Main.chest[RecentChest].y;
					Chest cheste = Main.chest[RecentChest];
					if (Main.tile[cheste.x, cheste.y].TileType == TileID.Containers && (Main.tile[i, j].TileFrameX == 432 || Main.tile[i, j].TileFrameX == 450))
					{
						for (int slot = 0; slot < Chest.maxItems; slot++)
						{
							if (!NPC.AnyNPCs(ModContent.NPCType<WulfwyrmHead>()) && cheste.item[slot].type == ModContent.ItemType<CalamityMod.Items.Materials.EnergyCore>() && cheste.item[slot].stack == 1)
							{
								// is the rest of the chest empty
								int ok = 0;
								for (int q = 0; q < Chest.maxItems; q++) ok += cheste.item[q].stack;
								if (ok == 1)
								{
									cheste.item[slot].stack = 0;
									cheste.item[slot].type = ItemID.None;
									NPC guy = CalamityUtils.SpawnBossBetter(new Vector2(i * 16, j * 16 + 1200), ModContent.NPCType<WulfwyrmHead>());

									SoundEngine.PlaySound(SoundID.Roar, new Vector2(i * 16, j * 16));
									if (guy.whoAmI.WithinBounds(Main.maxNPCs))
									{
										guy.velocity.Y = -20;
									}
									break;
								}
								Main.NewText(ok);
							}
						}
					}
				}
				RecentChest = Player.chest;
			}
		}
    }
}