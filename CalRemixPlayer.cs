using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.CalPlayer;
using CalamityMod.Events;
using CalamityMod.Items.Accessories.Vanity;
using CalamityMod.Items.Dyes;
using CalamityMod.Items.PermanentBoosters;
using CalamityMod.Items.Placeables.Furniture;
using CalamityMod.NPCs.Abyss;
using CalamityMod.NPCs.DevourerofGods;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.NPCs.SunkenSea;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Boss;
using CalamityMod.Projectiles.Summon;
using CalamityMod.Projectiles.Summon.Umbrella;
using CalamityMod.Projectiles.Typeless;
using CalamityMod.Sounds;
using CalRemix.Content;
using CalRemix.Content.Buffs;
using CalRemix.Content.Cooldowns;
using CalRemix.Content.Items.Accessories;
using CalRemix.Content.Items.Armor;
using CalRemix.Content.Items.Bags;
using CalRemix.Content.Items.Critters;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Items.Misc;
using CalRemix.Content.Items.Potions.Recovery;
using CalRemix.Content.Items.Tools;
using CalRemix.Content.Items.Weapons;
using CalRemix.Content.NPCs.Bosses.BossScule;
using CalRemix.Content.NPCs.Bosses.Hydrogen;
using CalRemix.Content.NPCs.Bosses.Hypnos;
using CalRemix.Content.NPCs.Bosses.Phytogen;
using CalRemix.Content.NPCs.Minibosses;
using CalRemix.Content.Projectiles;
using CalRemix.Content.Projectiles.Accessories;
using CalRemix.Content.Projectiles.Hostile;
using CalRemix.Content.Projectiles.Weapons;
using CalRemix.Content.Walls;
using CalRemix.Core;
using CalRemix.Core.Biomes;
using CalRemix.Core.Subworlds;
using CalRemix.Core.World;
using CalRemix.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SubworldLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;
using CalamityMod.NPCs.ExoMechs.Ares;
using System.Threading.Tasks;
using CalRemix.Content.Items.Weapons.Stormbow;
using Mono.Cecil;
using CalamityMod.Items.VanillaArmorChanges;
using CalamityMod.Buffs.StatBuffs;

namespace CalRemix
{
    public struct DyeStats(int red = 0, int orange = 0, int yellow = 0, int lime = 0, int green = 0, int cyan = 0, int teal = 0, int skyblue = 0, int blue = 0, int purple = 0, int violet = 0, int pink = 0, int brown = 0, int black = 0, int silver = 0)
	{
		public int red = red;
        public int orange = orange;
        public int yellow = yellow;
        public int lime = lime;
        public int green = green;
        public int cyan = cyan;
        public int teal = teal;
        public int skyblue = skyblue;
        public int blue = blue;
        public int purple = purple;
        public int violet = violet;
        public int brown = brown;
        public int pink = pink;
        public int silver = silver;
        public int black = black;
	}

    public class CalRemixPlayer : ModPlayer
	{

        public static readonly SoundStyle glassBreakSound = new("CalRemix/Assets/Sounds/GlassBreak");

        // General
        public int commonItemHoldTimer;
        public int remixJumpCount;
        public int RecentChest = -1;
        public int onFandom;
        public int checkWarningDelay;
        public bool anomaly109UI;
        public bool fridge;

        public bool gottenCellPhone = false;
        public bool miracleUnlocked = false;
        public bool solynUnlocked = false;
        public int trapperFriendsLearned = 0;
        public int ionDialogue = -1;
        public int ionQuestLevel = -1;

        public CustomGen customGen = new(1, Color.White, false, true, 3, Color.White, false, true);
        public bool generatingGen = false;
        public bool genActive = false;
        public bool genMusic = true;

        // Biome
        public bool dungeon2;
        public bool ZoneLife;
        public bool ZonePlague;
        public bool ZonePlagueDesert;

        // Accessories
        public bool brimPortal;
        public bool arcanumHands;
        public bool marnite;
        public bool roguebox;
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
        public bool wormMeal;
        public bool invGar;
        public int VerbotenMode = 1;
        public bool retroman = false;
        public bool noTomorrow;
        public bool calamityRing;
        public bool fungiStone;
        public bool fungiStone2;

        public bool miragel;
        public bool elastigel;
        public bool invigel;
        public bool irategel;

        public int timeSmoked;
        public bool carcinogenSoul;
        public bool carcinogenSoulVanity;
        public bool hydrogenSoul;
        public bool ionogenSoul;
        public bool origenSoul;
        public bool pathogenSoul;
        public bool oxygenSoul;
        public bool phytogenSoul;
        public bool pyrogenSoul;

        public int blazeCount = 0;
        public bool blaze;
        public bool pearl;
        public bool astralEye;

        public bool polyShieldChargeEnabled;
        public int polyShieldCooldown;
        public bool polyShieldChargeKeypress;
        public int polyShieldChargeDuration;
        public int polyShieldChargeSpeed = 16;
        public int polyShieldChargeDurationMax = 25;
        public int dashBuffer = 5;
        public int dashBufferEarly = 5;
        public int dashBufferLate = 5;
        public Vector2 polyShieldChargeDir = new Vector2(0, 0);

        public bool baroclaw;
        public Vector2 clawPosition = Vector2.Zero;

        public bool infraredSights;
        public bool infraredSightsScanning;
        public int infraredSightsCounter;

        public int eclipseaura = -1;
        public Particle ring;
        public Particle ring2;
        public Particle aura;

        // Weapons
        public int chainSawCharge;
        public int chainSawHitCooldown = 0;
        public int chainSawLevel1 = 1;
        public int chainSawLevel2 = 30 * 5;
        public int chainSawLevel3 = 30 * 10;
        public int chainSawChargeCritMax = 30 * 15;
        public int chainSawChargeMax = 30 * 20;
        public int roxCooldown;

        // Tools
        public bool phd;

        // Armor
        public bool bananaClown;
        public bool twistedNetherite;
        public bool twistedNetheriteBoots;

        // Minions
        public bool soldier;
        public bool dreamingGhost;
        public bool statue;
        public bool mackerel;
        public bool neuron;
        public bool corrosiveEye;
        public bool sickcell;
        public bool moltool;
        public bool onyxFist;
        public bool fractalCrawler;
        public bool exolotl;
        public bool cSlime3;

        // Pets
        public bool nothing;

        // Buffs
        public bool hayFever;
        public int calamitizedCounter;
        public int calamitizedHitCooldown;
        public bool stratusBeverage;
        // Tainted
        public bool taintedAmmo;
        public bool taintedArchery;
        public bool taintedBattle;
        public bool taintedBuilder;
        public bool taintedCalm;
        public bool taintedCrate;
        public bool taintedDanger;
        public bool taintedEndurance;
        public bool taintedFeather;
        public bool taintedFish;
        public bool taintedGills;
        public bool taintedGravity;
        public bool taintedHeart;
        public bool taintedHunt;
        public bool taintedInferno;
        public bool taintedInvis;
        public bool taintedIron;
        public bool taintedLife;
        public bool taintedLove;
        public bool taintedLuck;
        public bool taintedMagic;
        public bool taintedMana;
        public bool taintedMining;
        public bool taintedOwl;
        public bool taintedObsidian;
        public bool taintedRage;
        public bool taintedRegen;
        public bool taintedShine;
        public bool taintedSonar;
        public bool taintedSpelunker;
        public bool taintedStink;
        public bool taintedSummoning;
        public bool taintedSwift;
        public bool taintedThorns;
        public bool taintedTitan;
        public bool taintedWarmth;
        public bool taintedWater;
        public bool taintedWrath;

        // Tiles
        public float cosdam = 0;
        public bool astEffigy;
        public bool halEffigy;

        // Enchantments
        public bool earthEnchant;
        public bool amongusEnchant;

        // Dyes
        public int dyesRed = 0;
        public int dyesOrange = 0;
        public int dyesYellow = 0;
        public int dyesLime = 0;
        public int dyesGreen = 0;
        public int dyesTeal = 0;
        public int dyesCyan = 0;
        public int dyesLightBlue = 0;
        public int dyesDarkBlue = 0;
        public int dyesPurple = 0;
        public int dyesViolet = 0;
        public int dyesPink = 0;
        public int dyesBlack = 0;
        public int dyesBrown = 0;
        public int dyesSilver = 0;

		public static Dictionary<int, DyeStats> dyeStats = new Dictionary<int, DyeStats>();

        private static readonly List<PlayerDrawLayer> HiddenGenLayers =
        [
            PlayerDrawLayers.Wings,
            PlayerDrawLayers.HeadBack,
            PlayerDrawLayers.Torso,
            PlayerDrawLayers.Skin,
            PlayerDrawLayers.Leggings,
            PlayerDrawLayers.Shoes,
            PlayerDrawLayers.Robe,
            PlayerDrawLayers.SkinLongCoat,
            PlayerDrawLayers.ArmorLongCoat,
            PlayerDrawLayers.Head,
            PlayerDrawLayers.FrontAccBack,
            PlayerDrawLayers.FrontAccFront,
            PlayerDrawLayers.Shield,
            PlayerDrawLayers.ArmOverItem,
            PlayerDrawLayers.HandOnAcc
        ];
        public int[] MinionList =
		{
			ProjectileType<PlantationStaffSummon>(),
			ProjectileType<AtlasSoldier>(),
			ProjectileType<CosmilampMinion>(),
			ProjectileType<FieryDraconid>(),
			ProjectileType<SepulcherMinion>(),
			ProjectileType<CosmicEnergySpiral>(),
			ProjectileType<EndoCooperBody>(),
			ProjectileType<MagicUmbrella>(),
			ProjectileType<SiriusMinion>(),
			ProjectileType<SarosAura>()

		};

		public int[] abnormalEnemyList = // immune to effects like Moon Fist's instant kill
		{
			/*NPCType<SignalDrone>(),
			NPCType<DerellectPlug>(),*/
			NPCType<LifeSlime>()
		};

        public override void Load()
        {
            LoadDyeStats();
        }

        public override void SaveData(TagCompound tag)
        {
            tag["GeneratorPlayerGen"] = customGen;
            tag["GeneratorPlayerMusic"] = genMusic;
            tag["CellPhone"] = gottenCellPhone;
            tag["TrappFriends"] = trapperFriendsLearned;
            tag["MiracleUnlocked"] = miracleUnlocked;
        }
        public override void LoadData(TagCompound tag)
        {
            customGen = tag.Get<CustomGen>("GeneratorPlayerGen");
            genMusic = tag.GetBool("GeneratorPlayerMusic");
            gottenCellPhone = tag.GetBool("CellPhone");
            trapperFriendsLearned = tag.GetInt("TrappFriends");
            miracleUnlocked = tag.GetBool("MiracleUnlocked");
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
		{
			if (CalamityKeybinds.SpectralVeilHotKey.JustPressed && roguebox)
			{
				if (!Player.HasCooldown(EclipseAuraCooldown.ID) && Player.GetModPlayer<CalRemixPlayer>().eclipseaura <= -1)
				{
					Player.GetModPlayer<CalRemixPlayer>().eclipseaura = 300;
				}
			}
			if (CalRemixKeybinds.BaroClawHotKey.JustPressed && baroclaw && CalamityUtils.CountProjectiles(ProjectileType<Claw>()) <= 0)
            {
				if (!Player.HasCooldown(BaroclawCooldown.ID))
				{
					float XDist = 120;
					clawPosition = Main.MouseWorld;
					Projectile.NewProjectile(Player.GetSource_FromThis(), Main.MouseWorld + Vector2.UnitX * XDist, Vector2.Zero, ProjectileType<Claw>(), 30, 0);
					int p = Projectile.NewProjectile(Player.GetSource_FromThis(), Main.MouseWorld - Vector2.UnitX * XDist, Vector2.Zero, ProjectileType<Claw>(), 30, 0);
					Main.projectile[p].spriteDirection *= -1;
				}
            }
            if (CalRemixKeybinds.StealthPotKeybind.JustPressed && Player.Calamity().rogueStealth < Player.Calamity().rogueStealthMax / 2 && Player.Calamity().wearingRogueArmor)
            {
				if (Player.HasItem(ItemType<SuperStealthPotion>()))
                {
                    CombatText.NewText(Player.getRect(), Color.MediumPurple, (int)(Player.Calamity().rogueStealthMax * 100f * 0.25f));
                    Player.Calamity().rogueStealth += Player.Calamity().rogueStealthMax * 0.25f;
                    Player.ConsumeItem(ItemType<LesserStealthPotion>());
                    Player.ConsumeItem(ItemType<SuperStealthPotion>());
                }
                else if (Player.HasItem(ItemType<GreaterStealthPotion>()))
                {
                    CombatText.NewText(Player.getRect(), Color.MediumPurple, (int)(Player.Calamity().rogueStealthMax * 100f * 0.2f));
                    Player.Calamity().rogueStealth += Player.Calamity().rogueStealthMax * 0.2f;
                    Player.ConsumeItem(ItemType<LesserStealthPotion>());
                    Player.ConsumeItem(ItemType<GreaterStealthPotion>());
                }
                else if (Player.HasItem(ItemType<StealthPotion>()))
                {
                    CombatText.NewText(Player.getRect(), Color.MediumPurple, (int)(Player.Calamity().rogueStealthMax * 100f * 0.15f));
                    Player.Calamity().rogueStealth += Player.Calamity().rogueStealthMax * 0.15f;
                    Player.ConsumeItem(ItemType<LesserStealthPotion>());
                    Player.ConsumeItem(ItemType<StealthPotion>());
                }
                else if (Player.HasItem(ItemType<LesserStealthPotion>()))
                {
                    CombatText.NewText(Player.getRect(), Color.MediumPurple, (int)(Player.Calamity().rogueStealthMax * 100f * 0.1f));
                    Player.Calamity().rogueStealth += Player.Calamity().rogueStealthMax * 0.1f;
                    Player.ConsumeItem(ItemType<LesserStealthPotion>());
                }
				if (Main.myPlayer == Player.whoAmI)
					SoundEngine.PlaySound(SoundID.Item3, Player.Center);
            }
            if (CalRemixKeybinds.IonoLightningKeybind.JustPressed && ionogenSoul)
            {
                if (!Player.HasCooldown(IonLightningCooldown.ID))
                {
                    int io = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center - Vector2.UnitY * 3200f, Vector2.UnitY, ProjectileType<IonogenLightning>(), 0, 0, Player.whoAmI, 0f, -1, 61);
					Main.projectile[io].timeLeft = 22;
					SoundEngine.PlaySound(CommonCalamitySounds.LightningSound, Player.Center);
					double dam = Player.Hurt(PlayerDeathReason.ByProjectile(Player.whoAmI, io), 1, 0, dodgeable: false);
					if (dam > 0)
					{
						Player.statLife += (int)dam;
						Player.RemoveAllIFrames();
					}
					Player.AddCooldown("Ionic", 300);
                }
            }
            if (CalamityKeybinds.ArmorSetBonusHotKey.JustPressed && twistedNetherite && Player.armor[0].type == ItemType<TwistedNetheriteHelmet>())
            {
				TwistedNetheriteHelmet helmet = Player.armor[0].ModItem as TwistedNetheriteHelmet;
				if (!Player.HasCooldown(SoulExplosionCooldown.ID) && helmet.souls > 0)
				{
                    SoundEngine.PlaySound(DevourerofGodsHead.AttackSound);
                    int maxSouls = (helmet.souls < 10) ? helmet.souls : 10;
                    for (int i = 0; i < maxSouls; i++)
                    {
                        Vector2 velocity = -Vector2.UnitY.RotatedByRandom(0.52999997138977051) * Main.rand.NextFloat(2.5f, 4f);
                        Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, velocity, ProjectileType<SepulcherSoul>(), 0, 0f);
                    }
                    Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ProjectileType<TwistedNetheriteSoulExplosion>(), (int)(Math.Log(helmet.souls) + 1) * 1000, 0f, Player.whoAmI, ai1: 1600);
					helmet.souls = 0;
                    Player.AddCooldown("SoulExplosion", 1800);
                }
            }
			if (CalRemixKeybinds.InfraredSightsKeybind.JustPressed && infraredSights)
			{
				if (!Player.HasCooldown(InfraredSightsCooldown.ID))
                {
					if (Main.myPlayer == Player.whoAmI)
						CombatText.NewText(Player.getRect(), Color.Red, CalRemixHelper.LocalText("StatusText.InfaredScan").Value, true);
                    infraredSightsScanning = true;
                    Player.AddCooldown("InfraredSights", 3600);
                }
			}
            if (CalamityKeybinds.RageHotKey.JustPressed && Player.Calamity().rage >= Player.Calamity().rageMax && irategel)
            {
                Player.Hurt(new PlayerDeathReason(), Player.statLifeMax2 / 2, 0);
            }
        }

        public override void ModifyHurt(ref Player.HurtModifiers modifiers)/* tModPorter Override ImmuneTo, FreeDodge or ConsumableDodge instead to prevent taking damage */
        {
            if (eclipseaura > 0)
            {
				modifiers.SourceDamage *= 0f;
            }
			if (fridge)
            {
                modifiers.SourceDamage *= 0f;
            }
			if (cursed)
			{ 
				modifiers.SourceDamage *= 1.9f;
			}
            if (taintedGills && Player.IsUnderwater() && Player.breath > 0)
            {
                modifiers.SourceDamage *= 0f;
            }			
        }

        private int stoolBoost;

        public override void PostUpdateEquips()
        {
	        base.PostUpdateEquips();
	        
	        if (!Player.portableStoolInfo.IsInUse)
	        {
		        stoolBoost = 0;
	        }
	        else
	        {
		        stoolBoost++;
	            
		        var boost = Player.portableStoolInfo.HeightBoost + stoolBoost;
		        Player.portableStoolInfo.SetStats(boost, boost, boost);
	        }
        }

        public override void UpdateEquips()
        {
			if (CalRemixWorld.remixJump)
				Player.GetJumpState<DefaultJump>().Enable();
			else
                Player.GetJumpState<DefaultJump>().Disable();

            if (CalRemixWorld.dyeStats)
            {
                for (int i = 0; i < 3; i++)
                    CalRemixHooks.CountDyes(Player, Player.dye[i].type);
                for (int i = 0; i < 5; i++)
                    CalRemixHooks.CountDyes(Player, Player.miscDyes[i].type);
            }
        }
		public override void PreUpdate()
        {
            if (Main.myPlayer == Player.whoAmI)
            {
                if (ExoMechWorld.AnyDraedonActive && SubworldSystem.Current == GetInstance<ExosphereSubworld>() || NPC.AnyNPCs(NPCType<Hypnos>()))
                    Player.Calamity().monolithExoShader = 30;
                if (Main.mouseItem.type == ItemType<CirrusCouch>() || Main.mouseItem.type == ItemType<CrystalHeartVodka>())
                    Main.mouseItem.stack = 0;
                if (checkWarningDelay <= 0)
                {
                    Task.Run(() => Warning.CheckOnFandom(Player));
                    checkWarningDelay = 30;
                }
                if (Player.miscCounter % 90 == 0 && onFandom > 0)
                    SoundEngine.PlaySound(AresBody.EnragedSound with { MaxInstances = 3, SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest });
                if (onFandom > 0)
                    onFandom--;
                checkWarningDelay--; 
                if (ScreenHelpersUIState.BizarroFanny != null)
                {
                    if (ScreenHelpersUIState.BizarroFanny.Speaking && ScreenHelpersUIState.BizarroFanny.UsedMessage.Portrait == ScreenHelperManager.Portraits["BizarroFannyGoner"])
                        Main.musicVolume = 0;
                }
            }
            if (CalRemixWorld.permanenthealth)
				SpawnPhantomHeart();

            if (calamitizedHitCooldown > 0)
                calamitizedHitCooldown--;

            if (infraredSightsScanning)
                InfraredLogic();

            if (roxCooldown > 0)
                roxCooldown--;

            if (VerbotenMode >= 5)
				VerbotenMode = 1;
            if (chainSawCharge >= chainSawLevel1)
            {
                Player.ClearBuff(BuffID.Darkness);
                Player.AddBuff(BuffID.Darkness, chainSawCharge);
            }
            if (chainSawCharge >= chainSawLevel2)
            {
                Player.ClearBuff(BuffID.Blackout);
                Player.AddBuff(BuffID.Blackout, chainSawCharge - (30 * 4));
            }
            if (chainSawCharge >= chainSawLevel3)
            {
                Player.ClearBuff(BuffID.Obstructed);
                Player.AddBuff(BuffID.Obstructed, chainSawCharge - (30 * 10));
            }

            chainSawHitCooldown--;
            if (chainSawHitCooldown < 0)
            {
                chainSawCharge--;
                if (chainSawCharge < 0)
                {
                    chainSawCharge = 0;
                }
                chainSawHitCooldown = 0;
            }

            if (taintedBuilder)
            {
                Player.blockRange = 0;
                Player.tileSpeed = 0;
                Player.wallSpeed = 0;
            }

            if (taintedDanger)
            {
                Point playerPos = Player.position.ToTileCoordinates();
                int range = Main.rand.Next(8, 64);
                Wiring.TripWire(playerPos.X - range, playerPos.Y - range, range * 2, range * 2);
            }

            if (taintedFish)
            {
                Player.fishingSkill -= 22;
            }

            if (taintedGills)
            {
                if (Player.IsUnderwater())
                {
                    Player.breath -= (int)(Player.breathMax * 0.02f);
                }
            }

            if (taintedGravity)
            {
                Player.velocity += new Vector2(Main.rand.Next(-10, 11), Main.rand.Next(-10, 31));
            }

            if (taintedHunt)
            {
                Player.aggro += 800;
            }

            if (taintedMana)
            {
                Player.manaCost *= 2;
            }

            if (taintedMining)
            {
                Player.pickSpeed *= 0.2f;
            }

            if (taintedRage)
            {
                Player.GetCritChance(DamageClass.Generic) *= 0;
                Player.GetDamage<GenericDamageClass>() *= 1.3f;
            }

            if (taintedSummoning)
            {
                Player.maxMinions = 222222;
            }

            if (taintedTitan)
            {
                Player.GetKnockback(DamageClass.Generic) *= 0;
            }

            if (taintedIron)
            {
                Player.statDefense -= (int)(Player.statDefense * 0.22f);
            }

			if (Player.ZoneRockLayerHeight)
			{
				if (!NPC.AnyNPCs(NPCType<Phytogen>()))
				{
					int plagueEnemies = 0;
					int plagueToSpawnPhytogen = 6;
					foreach (NPC n in Main.npc)
					{
						if (n.active && n.life > 0 && n != null && Phytogen.plagueEnemies.Contains(n.type) && n.Distance(Player.Center) < 600)
						{
							plagueEnemies++;
							if (plagueEnemies >= plagueToSpawnPhytogen)
							{
								NPC.SpawnOnPlayer(Player.whoAmI, NPCType<Phytogen>());
								break;
							}
						}
					}
				}
			}
            if (CalRemixWorld.hydrogenLocation != Vector2.Zero && !BossRushEvent.BossRushActive)
            {
                if (Player.Distance(CalRemixWorld.hydrogenLocation) < 2000)
                {
                    if (!NPC.AnyNPCs(NPCType<Hydrogen>()))
                        NPC.NewNPC(Player.GetSource_FromThis(), (int)CalRemixWorld.hydrogenLocation.X + 10, (int)CalRemixWorld.hydrogenLocation.Y + 40, NPCType<Hydrogen>());
                }
            }
        }

        public override bool PreItemCheck()
        {
            if (Main.myPlayer == Player.whoAmI)
                ManageItemsInUse(Player, Player.HeldItem, Main.mouseItem, ref commonItemHoldTimer);
            if (Player.HeldItem.type == ItemType<CirrusCouch>() || Player.HeldItem.type == ItemType<CrystalHeartVodka>())
                Player.HeldItem.stack = 0;
            if (Player.HeldItem.type == ItemType<TwistedNetheriteShovel>() && Player.itemAnimation == Player.itemAnimationMax && Player.IsTargetTileInItemRange(Player.HeldItem))
            {
                for (int i = Player.tileTargetX - 4; i <= Player.tileTargetX + 4; i++)
                {
                    for (int j = Player.tileTargetY - 4; j <= Player.tileTargetY + 4; j++)
                    {
                        if (TileID.Sets.CanBeDugByShovel[Main.tile[i, j].TileType])
                        {
                            Player.PickTile(i, j, 100);
                        }
                    }
                }
            }

            /*
            if (Player.HeldItem.type == ItemID.MechanicalWorm) // has to be here or else derellect spawns 5 times. blame vanilla jank for this, THEY had to work around this problem
			{ 
                if (NPC.CountNPCS(NPCType<DerellectBoss>()) >= 1)
				{
					Player.itemTime = 0;
					Player.itemAnimation = 0; // looks weird but has to be done or else you get stuck holding it forever
					return false;

				}
  				return true;                  
			}
			*/
            return true;
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {     
            if (Main.zenithWorld) 
                SoundEngine.PlaySound(glassBreakSound, Player.Center);
            return true;
        }
        public override void PostUpdateMiscEffects()
        {   if (Main.myPlayer == Player.whoAmI)
            {
                if (Main.mouseItem.type == ItemType<ShardofGlass>())
                {
                    if (((ShardofGlass)Main.mouseItem.ModItem).durability <= 0)
                    {
                        SoundEngine.PlaySound(SoundID.Shatter with { Volume = 2 }, Player.position);
                        for (int i = 0; i < 30; i++)
                        {
                            Gore.NewGore(Player.GetSource_FromThis(), Player.Center, Main.rand.NextVector2Circular(10, 10).SafeNormalize(Vector2.UnitY) * Main.rand.Next(4, 8), Mod.Find<ModGore>("GlassShard" + Main.rand.Next(1, 5)).Type);
                        }
                        Main.mouseItem.TurnToAir();
                    }
                }
                else if (Player.HeldItem.type == ItemType<ShardofGlass>())
                {
                    if (((ShardofGlass)Player.HeldItem.ModItem).durability <= 0)
                    {
                        SoundEngine.PlaySound(SoundID.Shatter with { Volume = 2 }, Player.position);
                        for (int i = 0; i < 30; i++)
                        {
                            Gore.NewGore(Player.GetSource_FromThis(), Player.Center, Main.rand.NextVector2Circular(10, 10).SafeNormalize(Vector2.UnitY) * Main.rand.Next(4, 8), Mod.Find<ModGore>("GlassShard" + Main.rand.Next(1, 5)).Type);
                        }
                        Player.HeldItem.TurnToAir();
                    }
                }
            }
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
				if (Player.ZoneSkyHeight)
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

					target.AddBuff(BuffType<BrimstoneFlames>(), 1200);
				}
            }

            if (Player.Calamity().fearmongerSet && CalRemixWorld.fearmonger)
            {
				if (Main.snowMoon || Main.pumpkinMoon)
				{
					Player.endurance *= 0.85f;
				}
                Player.Calamity().wearingRogueArmor = false;
                Player.Calamity().WearingPostMLSummonerSet = false;
                Player.maxMinions -= 1;
                if (Player.slotsMinions > 0)
                {
                    Player.GetDamage<MeleeDamageClass>() *= 0.75f;
                    Player.GetDamage<RangedDamageClass>() *= 0.75f;
                    Player.GetDamage<MagicDamageClass>() *= 0.75f;
                    Player.GetDamage<RogueDamageClass>() *= 0.75f;
                    Player.GetDamage<TrueMeleeDamageClass>() *= 0.75f;
                    Player.GetDamage<TrueMeleeNoSpeedDamageClass>() *= 0.75f;
                    Player.GetDamage<TrueMeleeDamageClass>() *= 0.75f;
                }
                Player.GetDamage<SummonDamageClass>() -= 0.2f;
                Player.lifeRegen = (int)MathHelper.Min(Player.lifeRegen, 2);
                int[] immuneDebuffs = {
                BuffID.OnFire,
                BuffID.Frostburn,
                BuffID.CursedInferno,
                BuffID.ShadowFlame, //doesn't do anything
                BuffID.Daybreak, //doesn't do anything
                BuffID.Burning,
                BuffType<Shadowflame>(),
                BuffType<BrimstoneFlames>(),
                BuffType<HolyFlames>(),
                BuffType<GodSlayerInferno>(),
                BuffID.Chilled,
                BuffID.Frozen,
                BuffType<GlacialState>(),
            };
                for (var i = 0; i < immuneDebuffs.Length; ++i)
                    Player.buffImmune[immuneDebuffs[i]] = false;
                if (Player.yoraiz0rEye == 0)
                    Player.yoraiz0rEye = 3;
            }
            if ((int)Player.position.X / 16 >= 0 && (int)Player.position.Y / 16 >= 0 && (int)Player.position.X / 16 < Main.maxTilesX && (int)Player.position.Y / 16 < Main.maxTilesY)
            {
                if (Main.tile[(int)Player.position.X / 16, (int)Player.position.Y / 16].WallType == WallType<StratusWallRemix>())
                    dungeon2 = true;
                else
                    dungeon2 = false;
            }
			if (!carcinogenSoul)
			{
                if (timeSmoked > 0)
                    timeSmoked--;
            }
			// Kick people from chests in pre hardmode
			if (Player.chest > -1)
			{
				if (Player.InModBiome<FrozenStrongholdBiome>() && !Main.hardMode)
				{
					Player.chest = -1;
				}
            }

            Player.GetDamage<GenericDamageClass>() += (float)(dyesRed * 0.01f);
            Player.GetKnockback<GenericDamageClass>() += (float)(dyesPurple * 0.01f);
            Player.GetAttackSpeed<GenericDamageClass>() += (float)(dyesOrange * 0.01f);
            Player.GetCritChance<GenericDamageClass>() += dyesCyan;
            Player.moveSpeed += (float)(dyesYellow * 0.01f);
            Player.luck += dyesLime;
            Player.jumpSpeed += dyesGreen;
            Player.endurance += (float)(dyesTeal * 0.01f);
            Player.statDefense += dyesDarkBlue;
            Player.aggro += dyesViolet;
            Player.tileRangeX += dyesBrown;
            Player.tileRangeY += dyesBrown;
            Player.wingTimeMax += dyesLightBlue * 10;
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

            if (elastigel)
            {
                Player.wingTimeMax = (int)(Player.wingTimeMax * 1.1);
            }

            if (invigel)
            {
                if (calplayer.adrenalineModeActive)
                {
                    Player.wingTime = 0;
                }
            }

            if (taintedLife)
            {
                Player.statLifeMax2 -= 100;
                if (Player.statLife > Player.statLifeMax2)
                    Player.statLife = Player.statLifeMax2;
                Player.statManaMax2 += 100;
            }

            if (taintedObsidian)
                if (Player.lavaWet)
                    Player.KillMe(PlayerDeathReason.ByCustomReason(Player.name + " thought that was orange juice"), Player.statLifeMax, 0);
        }
        public override void ResetEffects()
		{
			brimPortal = false;
			arcanumHands = false;
			marnite = false;
			roguebox = false;
            dreamingGhost = false;
			statue = false;
			mackerel = false;
            moltool = false;
            sickcell = false;
			neuron = false;
            soldier = false;
			corrosiveEye = false;
            onyxFist = false;
			astEffigy = false;
			halEffigy = false;
			nothing = false;
			miragel = false;
            elastigel = false;
            invigel = false;
            irategel = false;
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
			blaze = false;
			pearl = false;
			astralEye = false;
			bananaClown = false;
			twistedNetherite = false;
			twistedNetheriteBoots = false;
            wormMeal = false;
			invGar = false;
			hayFever = false;
            stratusBeverage = false;
            taintedAmmo= false;
            taintedArchery= false;
            taintedBattle= false;
            taintedBuilder= false;
            taintedCalm= false;
            taintedCrate= false;
            taintedDanger= false;
            taintedEndurance= false;
            taintedFeather= false;
            taintedFish = false;
            taintedGills= false;
            taintedGravity= false;
            taintedHeart= false;
            taintedHunt= false;
            taintedInferno= false;
            taintedInvis= false;
            taintedIron= false;
            taintedLife= false;
            taintedLove= false;
            taintedLuck= false;
            taintedMagic= false;
            taintedMana= false;
            taintedMining= false;
            taintedOwl= false;
            taintedObsidian= false;
            taintedRage= false;
            taintedRegen= false;
            taintedShine= false;
            taintedSonar= false;
            taintedSpelunker= false;
            taintedStink= false;
            taintedSummoning= false;
            taintedSwift= false;
            taintedThorns= false;
            taintedTitan= false;
            taintedWarmth= false;
            taintedWater= false;
            taintedWrath= false;
            phd = false;
			infraredSights = false;
            exolotl = false;
            fractalCrawler = false;
            carcinogenSoul = false;
			carcinogenSoulVanity = false;
			hydrogenSoul = false;
			origenSoul = false;
			ionogenSoul = false;
			phytogenSoul = false;
            pyrogenSoul = false;
			oxygenSoul = false;
			pathogenSoul = false;
            genActive = false;
			dyesRed = 0;
			dyesOrange = 0;
			dyesYellow = 0;
			dyesLime = 0;
			dyesGreen = 0;
			dyesCyan = 0;
			dyesTeal = 0;
			dyesLightBlue = 0;
			dyesDarkBlue = 0;
			dyesPurple = 0;
			dyesViolet = 0;
			dyesSilver = 0;
			dyesBlack = 0;
			dyesBrown = 0;
			dyesPink = 0;
            retroman = false;
            noTomorrow = false;
            calamityRing = false;
            fungiStone = false;
            fungiStone2 = false;

            if (!Player.HasBuff<Calamitized>() && !NPC.AnyNPCs(NPCType<TheCalamity>()))
            {
                calamitizedHitCooldown = 0;
                calamitizedCounter = 0;
            }
            if (!CalamityUtils.AnyProjectiles(ProjectileType<Fridge>()))
			{
                fridge = false;
            }
            if (astEffigy)
				Player.statLifeMax2 = (int)(Player.statLifeMax2 * 1.5);
			if (Player.HeldItem != null && Player.HeldItem.type != ItemID.None)
			{
				if (!Player.HeldItem.Calamity().AppliedEnchantment.HasValue)
				{
					amongusEnchant = false;
				}
			}
            if (Main.myPlayer == Player.whoAmI)
            {
                if (Filters.Scene["CalRemix:AcidSight"].Active)
                    Filters.Scene["CalRemix:AcidSight"].Deactivate();
                if (Filters.Scene["CalRemix:LeanVision"].Active)
                    Filters.Scene["CalRemix:LeanVision"].Deactivate();
            }
        }
        public override void GetDyeTraderReward(List<int> rewardPool)
        {
			if (CalamityMod.DownedBossSystem.downedProvidence && CalRemixWorld.permanenthealth)
			{
				if (CalamityMod.DownedBossSystem.downedProvidence && !Player.Calamity().eBerry)
				{
					rewardPool.Clear();
				}
				rewardPool.Add(ItemType<Elderberry>());
			}
        }
		public override void OnHitNPC(NPC npc, NPC.HitInfo hit, int damageDone)
		{
			if (npc.life <= 0 && npc.value > 0 && twistedNetherite && Player.armor[0].type == ItemType<TwistedNetheriteHelmet>())
            {
                TwistedNetheriteHelmet helmet = Player.armor[0].ModItem as TwistedNetheriteHelmet;
				helmet.souls++;
            }
            if (npc.life > 0)
            {
                if (taintedMana && Main.rand.NextBool(22) && hit.DamageType == DamageClass.Magic)
                {
                    Item.NewItem(npc.GetSource_OnHit(npc), npc.getRect(), ItemID.Star);
                }
                if (taintedRegen && Main.rand.NextBool(50))
                {
                    Item.NewItem(npc.GetSource_OnHit(npc), npc.getRect(), ItemID.Heart);
                }
                if (taintedOwl)
                {
                    npc.AddBuff(BuffID.Confused, 220);
                }
            }
            if (Main.LocalPlayer.GetModPlayer<CalamityPlayer>().amalgam);
            {
                if (Main.rand.NextBool(4))
                {
                    Main.LocalPlayer.AddBuff(BuffType<Mushy>(), 360, false);
                }
                else if (Main.rand.NextBool(2))
                {
                    Main.LocalPlayer.AddBuff(BuffType<Mushy>(), 240, false);
                }
                else
                {
                    Main.LocalPlayer.AddBuff(BuffType<Mushy>(), 120, false);
                }
            }

            if (Player.portableStoolInfo.IsInUse)
            {
	            npc.AddBuff(BuffID.Dazed, 5 * 60);
            }
        }
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Item, consider using OnHitNPC instead */
        {
            if (amongusEnchant && hit.Crit)
			{
				if (Main.myPlayer == Player.whoAmI)
                {
                    CombatText.NewText(Main.LocalPlayer.getRect(), Color.Red, damageDone / 7);
                    SoundEngine.PlaySound(SoundID.NPCHit1);
                }
                Main.LocalPlayer.statLife -= (int)MathHelper.Clamp(damageDone / 7, 0, Player.statLife - 1);
                hit.SourceDamage = (int)(hit.SourceDamage * 2.5f);
            }
            if (earthEnchant)
            {
            }
            if (moonFist && item.DamageType == DamageClass.Melee)
			{
				target.AddBuff(BuffType<Nightwither>(), 300, false);
				if (target.boss == false && !abnormalEnemyList.Contains(target.type)) 
				{				
					if (Main.rand.NextBool(10))
					{
						target.HitEffect(0, target.lifeMax * 22);
					}		
					
				}
			}
			if (twistedNetherite && Player.armor[0].type == ItemType<TwistedNetheriteHelmet>())
            {
				TwistedNetheriteHelmet helmet = Player.armor[0].ModItem as TwistedNetheriteHelmet;
                target.AddBuff(BuffType<Wither>(), 120);
				target.GetGlobalNPC<CalRemixNPC>().wither = helmet.souls;
                if (target.life <= 0 && target.value > 0 && item.type == ItemType<TwistedNetheriteSword>())
                    helmet.souls += 2;
            }
            if (fungiStone || fungiStone2)
            {
                if (Main.rand.NextBool(4))
                {
                    Main.LocalPlayer.AddBuff(BuffType<Mushy>(), 360, false);
                }
                else if (Main.rand.NextBool(2))
                {
                    Main.LocalPlayer.AddBuff(BuffType<Mushy>(), 240, false);
                }
                else
                {
                    Main.LocalPlayer.AddBuff(BuffType<Mushy>(), 120, false);
                }
            }
        }
		public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Projectile, consider using OnHitNPC instead */
		{
			if (amongusEnchant && hit.Crit)
			{
				CombatText.NewText(Main.LocalPlayer.getRect(), Color.Red, damageDone / 7);
				Main.LocalPlayer.statLife -= (int)MathHelper.Clamp(damageDone / 7, 0, Player.statLife - 1);
				SoundEngine.PlaySound(SoundID.NPCHit1, Player.Center);
				hit.SourceDamage = (int)(hit.SourceDamage * 2.5f);
			}
			if (moonFist && proj.DamageType == DamageClass.Melee)
			{
				target.AddBuff(BuffType<Nightwither>(), 300, false);
				if (target.boss == false) 
				{				
					if(Main.rand.NextBool(10))
					{
						target.HitEffect(0, target.lifeMax * 22);
					}		
				}
            }
            if (twistedNetherite && Player.armor[0].type == ItemType<TwistedNetheriteHelmet>())
            {
                TwistedNetheriteHelmet helmet = Player.armor[0].ModItem as TwistedNetheriteHelmet;
                target.AddBuff(BuffType<Wither>(), 120);
                target.GetGlobalNPC<CalRemixNPC>().wither = helmet.souls;
            }
        }

		public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
			CalamityPlayer calplayer = Main.LocalPlayer.GetModPlayer<CalamityPlayer>();
			if (godfather)
			{
				if (npc.type == NPCID.BlueJellyfish || npc.type == NPCID.PinkJellyfish || npc.type == NPCID.GreenJellyfish ||
					npc.type == NPCID.FungoFish || npc.type == NPCID.BloodJelly || npc.type == NPCID.AngryNimbus || npc.type == NPCID.GigaZapper ||
					npc.type == NPCID.MartianTurret || npc.type == NPCType<Stormlion>() || npc.type == NPCType<GhostBell>() || npc.type == NPCType<BoxJellyfish>())
					calplayer.contactDamageReduction += 1;
				var source = Main.LocalPlayer.GetSource_OnHurt(npc);
				Vector2 playercenter = Main.LocalPlayer.Center;
				Vector2 spawnvector = new Vector2(playercenter.X - 4, playercenter.Y - 4);
				Projectile.NewProjectile(source, spawnvector, Vector2.Zero, ProjectileType<CalamityMod.Projectiles.Melee.CosmicIceBurst>(), 33000, 0, Main.LocalPlayer.whoAmI);
			}
            if (calamityRing)
            {
                npc.damage *= 2;
            }
		}
		public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
		{
			CalamityPlayer calplayer = Main.LocalPlayer.GetModPlayer<CalamityPlayer>();
			if (godfather)
			{
				if (proj.type == ProjectileID.MartianTurretBolt || proj.type == ProjectileID.GigaZapperSpear || proj.type == ProjectileID.CultistBossLightningOrbArc || proj.type == ProjectileID.VortexLightning || proj.type == ProjectileType<DestroyerElectricLaser>() ||
					proj.type == ProjectileID.BulletSnowman || proj.type == ProjectileID.BulletDeadeye || proj.type == ProjectileID.SniperBullet || proj.type == ProjectileID.VortexLaser)
					calplayer.projectileDamageReduction += 1;
			}
            if (calamityRing)
            {
                proj.damage *= 2;
            }
            if (taintedWarmth)
            {
                if (proj.Name.Contains("fire", StringComparison.CurrentCultureIgnoreCase) || proj.Name.Contains("hell", StringComparison.CurrentCultureIgnoreCase) || proj.Name.Contains("infer", StringComparison.CurrentCultureIgnoreCase))
                {
                    modifiers.SourceDamage *= 0.5f;
                }
            }
        }
        public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
        {
            bool inWater = !attempt.inLava && !attempt.inHoney;

            if (taintedFish)
            {
                if (attempt.rolledItemDrop > 0)
                {
                    itemDrop = 0;
                    npcSpawn = Main.rand.NextBool(50) ? NPCType<Xeroc>() : Main.rand.NextBool(22) ? NPCID.BloodNautilus : Main.rand.NextBool(10) ? NPCID.BloodEelHead : Main.rand.NextBool(5) ? NPCID.GoblinShark : Main.rand.NextBool() ? NPCID.ZombieMerman : NPCID.EyeballFlyingFish;
                    return;
                }
            }

            int roll = Main.rand.Next(100);
            if (attempt.bobberType == ProjectileType<TyrantBobber>() && Player.ActiveItem().type == ItemType<TheCodseeker>() && Main.rand.NextBool(100))
            {
                if (Main.myPlayer == Player.whoAmI)
                    SoundEngine.PlaySound(SoundID.ScaryScream);
                if (Player.HeldItem.type == ItemType<TheCodseeker>())
                    Player.HeldItem.ChangeItemType(ItemID.WoodFishingPole);
                else if (Main.myPlayer == Player.whoAmI && Main.mouseItem.type == ItemType<TheCodseeker>())
                    Main.mouseItem.ChangeItemType(ItemID.WoodFishingPole);
                foreach (Projectile p in Main.projectile)
                {
                    if (p != null)
                        continue;
                    if (p.owner == Player.whoAmI && p.type == ProjectileType<TyrantBobber>())
                        p.Kill();
                }
            }

            if (inWater && Player.Calamity().ZoneSulphur && roll < 5)
            {
                itemDrop = ItemType<BlobfishPlushie>();
            }
            if (inWater && Player.ZoneJungle && Main.hardMode && roll >= 0 && roll <= 7)
            {
                itemDrop = ItemType<Babilfish>();
            }
            if (inWater && Main.bloodMoon && Main.rand.NextBool(6))
            {
                itemDrop = ItemType<GrandioseGland>();
            }
            if (inWater && Player.ZoneSkyHeight && NPC.downedMoonlord && Main.rand.NextBool(10) && CalRemixWorld.sidegar)
            {
                itemDrop = ItemType<SideGar>();
            }
            if (inWater && Player.ZoneJungle && DownedBossSystem.downedProvidence && Main.rand.NextBool(10) && CalRemixWorld.reargar)
            {
                itemDrop = ItemType<RearGar>();
            }
            if (inWater && Player.Calamity().ZoneSulphur && DownedBossSystem.downedPolterghast && Main.rand.NextBool(10) && CalRemixWorld.frontgar)
            {
                itemDrop = ItemType<FrontGar>();
            }
            if (inWater && (Player.ZoneCrimson || Player.ZoneCorrupt) && !Main.dayTime && attempt.common && Main.rand.NextBool(10))
            {
                itemDrop = ItemType<TarGar>();
            }
            if (inWater && Player.Calamity().ZoneAbyss && attempt.rare && Main.rand.NextBool(10))
            {
                itemDrop = ItemType<ShadowGar>();
            }
            if (inWater && Player.Calamity().ZoneAbyssLayer4 && attempt.legendary)
            {
                itemDrop = ItemType<RipperShark>();
            }
            if (attempt.playerFishingConditions.BaitItemType == ItemType<LabRoach>())
            {
                if (CalRemixWorld.roachDuration <= 0)
                {
                    CalRemixWorld.RoachCountdown = -1;
                    CalRemixWorld.UnleashRoaches();
                }
                itemDrop = ItemID.None;
            }
            // Tainted Crate is double or nothing for crates
            if (taintedCrate)
            {
                if (attempt.crate)
                {
                    if (Main.rand.NextBool())
                    {
                        attempt.crate = false;
                    }
                    else
                    {
                        Item.NewItem(Player.GetSource_FromThis(), Player.getRect(), attempt.rolledItemDrop);
                    }
                }
            }

            if (taintedSonar)
            {
                itemDrop = Main.rand.Next(0, ContentSamples.ItemsByType.Count - 1);
            }
        }
        public void SpawnPhantomHeart()
        {
            if (Main.rand.NextBool(6000) && Player.ZoneDungeon && DownedBossSystem.downedPolterghast && !Player.GetModPlayer<CalamityPlayer>().pHeart)
            {
                Projectile.NewProjectile(Player.GetSource_FromThis(), new Vector2(Main.rand.Next((int)Player.Center.X - Main.screenWidth, (int)Player.Center.X + Main.screenWidth), Player.Center.Y + Main.screenHeight), new Vector2((float)Main.rand.Next(-400, 401) * 0.01f, (float)Main.rand.Next(-1000, -701) * 0.01f), ProjectileType<FallingPhantomHeart>(), 0, 0, Player.whoAmI);
            }
            else if (Main.rand.NextBool(10800) && Player.ZoneDungeon && DownedBossSystem.downedPolterghast && Player.GetModPlayer<CalamityPlayer>().pHeart)
            {
                Projectile.NewProjectile(Player.GetSource_FromThis(), new Vector2(Main.rand.Next((int)Player.Center.X - Main.screenWidth, (int)Player.Center.X + Main.screenWidth), Player.Center.Y + Main.screenHeight), new Vector2((float)Main.rand.Next(-400, 401) * 0.01f, (float)Main.rand.Next(-1000, -701) * 0.01f), ProjectileType<FallingPhantomHeart>(), 0, 0, Player.whoAmI);
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
        public override void UpdateAutopause()
        {
            // Kick people from chests in pre hardmode
            if (Player.chest > -1)
            {
                if (Player.InModBiome<FrozenStrongholdBiome>() && !Main.hardMode)
                {
                    Player.chest = -1;
                }
            }
            RecentChest = Player.chest;
        }

        public override void PreUpdateBuffs() 
        {
        }
		public override void UpdateBadLifeRegen()
        {
            if (Held(Player, ItemType<FlamingIceBow>()))
            {
                if (Player.lifeRegen > 0)
                    Player.lifeRegen = 0;
                Player.lifeRegenTime = 0;
                Player.lifeRegen -= 12;
            }
            if (hayFever)
            {
                if (Player.lifeRegen > 0)
                    Player.lifeRegen = 0;
                Player.lifeRegenTime = 0;
                Player.lifeRegen -= 240;
            }
            if (taintedInferno)
            {
                if (Player.lifeRegen > 0)
                    Player.lifeRegen = 0;
                Player.lifeRegenTime = 0;
                Player.lifeRegen -= 22;
            }
            if (taintedRegen)
            {
                if (Player.lifeRegen > 0)
                    Player.lifeRegen = 0;
            }
            if (stratusBeverage) Main.LocalPlayer.Calamity().alcoholPoisonLevel += 2;
    }

    public override void UpdateLifeRegen()
        {
            Player.lifeRegen += (int)MathHelper.Min(dyesPink, 0);
        }

        public override void FrameEffects()
		{
			if (Player.GetJumpState<DefaultJump>().Active)
				Player.armorEffectDrawShadow = true;

            if (retroman)
                GetInstance<RetromansPowerStar>().SetSet(Player);
		}
		public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            bool deicide = Held(Player, ItemType<Deicide>());
            bool flamingIce = Held(Player, ItemType<FlamingIceBow>());

            if (flamingIce && Main.rand.NextBool(10))
            {
                int index = Dust.NewDust(Player.position, Player.width, Player.height, (Main.rand.NextBool()) ? DustID.IceTorch : DustID.Torch, Main.rand.Next(-2, 3), -5f);
                drawInfo.DustCache.Add(index);
            }
            if (deicide && commonItemHoldTimer > 0)
            {
                Color c = Color.Lerp(Color.Red, Color.White, Utils.GetLerpValue(-10f, 3000f, commonItemHoldTimer, true));
                g = c.G;
                b = c.B;
            }
            if (drawInfo.shadow != 0)
                return;
            if (deicide)
            {
                if (commonItemHoldTimer > 1800)
                {
                    Texture2D texture = Request<Texture2D>("CalRemix/Assets/ExtraTextures/DarkWreath").Value;
                    Vector2 position = Player.Center - new Vector2(texture.Width / 2, texture.Height / 2) - Main.screenPosition + Vector2.UnitY * Player.gfxOffY;
                    position = new Vector2((int)position.X, (int)position.Y);
                    Main.spriteBatch.Draw(texture, position, Color.White);
                }
                if (commonItemHoldTimer > 3000 && Main.rand.NextBool(5))
                {
                    int index = Dust.NewDust(Player.position, Player.width, Player.height, DustID.Smoke, SpeedY: -4f, newColor: Color.DarkGray);
                    drawInfo.DustCache.Add(index);
                }
            }
            if (hayFever)
            {
                int index = Dust.NewDust(Player.position, Player.width, Player.height, DustID.JungleSpore, Scale: Main.rand.NextFloat(1f, 2f));
                drawInfo.DustCache.Add(index);
            }
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            if (invGar)
            {
                anomaly109UI = false;
                SoundEngine.PlaySound(CalamityMod.NPCs.NormalNPCs.ScornEater.HitSound, Player.Center);
				Player.AddBuff(BuffType<GarBoost>(), 60);
			}
            if (taintedIron && npc.Calamity().canBreakPlayerDefense)
            {
                Player.Calamity().defenseDamageRatio *= 0.5f;
            }
        }

        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            if (invGar)
            {
                anomaly109UI = false;
                SoundEngine.PlaySound(CalamityMod.NPCs.NormalNPCs.ScornEater.HitSound, Player.Center);
                Player.AddBuff(BuffType<GarBoost>(), 60);
            }
        }
        public override void HideDrawLayers(PlayerDrawSet drawInfo)
        {
            if (genActive && customGen.CoreVisible)
            {
                foreach (PlayerDrawLayer layer in PlayerDrawLayerLoader.Layers)
                {
                    if (layer == null)
                        continue;
                    if (HiddenGenLayers.Contains(layer))
                        layer.Hide();
                }
            }
        }
        public void InfraredLogic()
        {
            if (infraredSightsCounter >= 300)
            {
                infraredSightsCounter = 0;
                infraredSightsScanning = false;
                NPC npc = null;
                try
                {
                    foreach (NPC n in Main.npc)
                    {
                        if (n is null)
                            continue;
                        if (!n.whoAmI.WithinBounds(Main.maxNPCs))
                            continue;
                        if (!n.CanBeChasedBy())
                            continue;
                        if (npc != null)
                        {
                            if (npc.lifeMax > n.lifeMax)
                                continue;
                        }
                        npc = n;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                if (npc is null)
                    CalamityUtils.DisplayLocalizedText("Mods.CalRemix.StatusText.InfaredNoData");
                else
                {
                    string f = CalRemixHelper.LocalText("StatusText.InfaredData").Format(npc.TypeName, npc.damage, npc.defDamage);

                    if (Main.netMode == NetmodeID.SinglePlayer)
                    {
                        Main.NewText(f);
                    }
                    else
                    {
                        ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(f), Color.White);
                    }
                }
            }
            infraredSightsCounter++;
        }
		public static void LoadDyeStats()
		{
			// Basics
			dyeStats.Add(ItemID.RedDye, new DyeStats(red: 1));
            dyeStats.Add(ItemID.OrangeDye, new DyeStats(orange: 1));
            dyeStats.Add(ItemID.YellowDye, new DyeStats(yellow: 1));
            dyeStats.Add(ItemID.LimeDye, new DyeStats(lime: 1));
            dyeStats.Add(ItemID.GreenDye, new DyeStats(green: 1));
            dyeStats.Add(ItemID.CyanDye, new DyeStats(cyan: 1));
			dyeStats.Add(ItemID.TealDye, new DyeStats(teal: 1));
            dyeStats.Add(ItemID.SkyBlueDye, new DyeStats(skyblue: 1));
            dyeStats.Add(ItemID.BlueDye, new DyeStats(blue: 1));
            dyeStats.Add(ItemID.PurpleDye, new DyeStats(purple: 1));
            dyeStats.Add(ItemID.VioletDye, new DyeStats(violet: 1));
            dyeStats.Add(ItemID.PinkDye, new DyeStats(pink: 1));
            dyeStats.Add(ItemID.BrownDye, new DyeStats(brown: 1));
            dyeStats.Add(ItemID.SilverDye, new DyeStats(silver: 1));
            dyeStats.Add(ItemID.BlackDye, new DyeStats(black: 1));
			// Bright
            dyeStats.Add(ItemID.BrightRedDye, new DyeStats(red: 2));
            dyeStats.Add(ItemID.BrightOrangeDye, new DyeStats(orange: 2));
            dyeStats.Add(ItemID.BrightYellowDye, new DyeStats(yellow: 2));
            dyeStats.Add(ItemID.BrightLimeDye, new DyeStats(lime: 2));
            dyeStats.Add(ItemID.BrightGreenDye, new DyeStats(green: 2));
            dyeStats.Add(ItemID.BrightCyanDye, new DyeStats(cyan: 2));
            dyeStats.Add(ItemID.BrightTealDye, new DyeStats(teal: 2));
            dyeStats.Add(ItemID.BrightSkyBlueDye, new DyeStats(skyblue: 2));
            dyeStats.Add(ItemID.BrightBlueDye, new DyeStats(blue: 2));
            dyeStats.Add(ItemID.BrightPurpleDye, new DyeStats(purple: 2));
            dyeStats.Add(ItemID.BrightVioletDye, new DyeStats(violet: 2));
            dyeStats.Add(ItemID.BrightPinkDye, new DyeStats(pink: 2));
            dyeStats.Add(ItemID.BrightBrownDye, new DyeStats(brown: 2));
            dyeStats.Add(ItemID.BrightSilverDye, new DyeStats(silver: 2));
			// Fancy crafts
            dyeStats.Add(ItemID.FlameDye, new DyeStats(red: 1, orange : 1, yellow: 1));
            dyeStats.Add(ItemID.GreenFlameDye, new DyeStats(green: 1, lime: 1, yellow: 1));
            dyeStats.Add(ItemID.BlueFlameDye, new DyeStats(blue: 1, cyan: 1, skyblue: 1));
            dyeStats.Add(ItemID.YellowGradientDye, new DyeStats(lime: 1, orange: 1, yellow: 1));
            dyeStats.Add(ItemID.CyanGradientDye, new DyeStats(blue: 1, cyan: 1, skyblue: 1));
            dyeStats.Add(ItemID.VioletGradientDye, new DyeStats(purple: 1, violet: 1, pink: 1));
            dyeStats.Add(ItemID.RainbowDye, new DyeStats(lime: 1, orange: 1, yellow: 1, blue: 1, cyan: 1, skyblue: 1, purple: 1, violet: 1, pink: 1));
            dyeStats.Add(ItemID.IntenseFlameDye, new DyeStats(red: 2, orange: 2, yellow: 2));
            dyeStats.Add(ItemID.IntenseGreenFlameDye, new DyeStats(green: 2, lime: 2, yellow: 2));
            dyeStats.Add(ItemID.IntenseBlueFlameDye, new DyeStats(blue: 2, cyan: 2, skyblue: 2));
            dyeStats.Add(ItemID.IntenseRainbowDye, new DyeStats(lime: 2, orange: 2, yellow: 2, blue: 2, cyan: 2, skyblue: 2, purple: 2, violet: 2, pink: 2));
            // Black combos
			dyeStats.Add(ItemID.RedandBlackDye, new DyeStats(red: -1));
            dyeStats.Add(ItemID.OrangeandBlackDye, new DyeStats(orange: -1));
            dyeStats.Add(ItemID.YellowandBlackDye, new DyeStats(yellow: -1));
            dyeStats.Add(ItemID.LimeandBlackDye, new DyeStats(lime: -1));
            dyeStats.Add(ItemID.GreenandBlackDye, new DyeStats(green: -1));
            dyeStats.Add(ItemID.CyanandBlackDye, new DyeStats(cyan: -1));
            dyeStats.Add(ItemID.TealandBlackDye, new DyeStats(teal: -1));
            dyeStats.Add(ItemID.SkyBlueandBlackDye, new DyeStats(skyblue: -1));
            dyeStats.Add(ItemID.BlueandBlackDye, new DyeStats(blue: -1));
            dyeStats.Add(ItemID.PurpleandBlackDye, new DyeStats(purple: -1));
            dyeStats.Add(ItemID.VioletandBlackDye, new DyeStats(violet: -1));
            dyeStats.Add(ItemID.PinkandBlackDye, new DyeStats(pink: -1));
            dyeStats.Add(ItemID.BrownAndBlackDye, new DyeStats(brown: -1));
            dyeStats.Add(ItemID.SilverAndBlackDye, new DyeStats(silver: -1));
            dyeStats.Add(ItemID.FlameAndBlackDye, new DyeStats(red: -1, orange: -1, yellow: -1));
            dyeStats.Add(ItemID.GreenFlameAndBlackDye, new DyeStats(green: -1, lime: -1, yellow: -1));
            dyeStats.Add(ItemID.BlueFlameAndBlackDye, new DyeStats(blue: -1, cyan: -1, skyblue: -1));
            // Silver combos
            dyeStats.Add(ItemID.RedandSilverDye, new DyeStats(red: 5, cyan: -5));
            dyeStats.Add(ItemID.OrangeandSilverDye, new DyeStats(orange: 5, skyblue: -5));
            dyeStats.Add(ItemID.YellowandSilverDye, new DyeStats(yellow: 5, blue: -5));
            dyeStats.Add(ItemID.LimeandSilverDye, new DyeStats(lime: 5, purple: -5));
            dyeStats.Add(ItemID.GreenandSilverDye, new DyeStats(green: 5, violet: -5));
            dyeStats.Add(ItemID.CyanandSilverDye, new DyeStats(cyan: 5, pink: -5));
            dyeStats.Add(ItemID.TealandSilverDye, new DyeStats(teal: 5, red: -5));
            dyeStats.Add(ItemID.SkyBlueandSilverDye, new DyeStats(skyblue: 5, orange: -5));
            dyeStats.Add(ItemID.BlueandSilverDye, new DyeStats(blue: 5, yellow: -5));
            dyeStats.Add(ItemID.PurpleandSilverDye, new DyeStats(purple: 5, lime: -5));
            dyeStats.Add(ItemID.VioletandSilverDye, new DyeStats(violet: 5, green: -5));
            dyeStats.Add(ItemID.PinkandSilverDye, new DyeStats(pink: 5, cyan: -5));
            dyeStats.Add(ItemID.BrownAndSilverDye, new DyeStats(brown: 5, teal: -5));
            dyeStats.Add(ItemID.BlackAndWhiteDye, new DyeStats(silver: 5, black: 5));
            dyeStats.Add(ItemID.FlameAndSilverDye, new DyeStats(red: 5, orange: 5, yellow: 5, cyan: -5, skyblue: -5, blue: -5));
            dyeStats.Add(ItemID.GreenFlameAndSilverDye, new DyeStats(green: 5, lime: 5, yellow: 5, violet: -5, purple: -5, blue: -5));
            dyeStats.Add(ItemID.BlueFlameAndSilverDye, new DyeStats(blue: 5, cyan: 5, skyblue: 5, yellow: -5, pink: -5, orange: -5));
            // Strange
            dyeStats.Add(ItemID.AcidDye, new DyeStats(green: 3, lime: 2));
            dyeStats.Add(ItemID.BlueAcidDye, new DyeStats(blue: 3, skyblue: 2));
            dyeStats.Add(ItemID.RedAcidDye, new DyeStats(red: 3, orange: 2));
            dyeStats.Add(ItemID.ChlorophyteDye, new DyeStats(green: 4));
            dyeStats.Add(ItemID.GelDye, new DyeStats(blue: 3));
            dyeStats.Add(ItemID.MushroomDye, new DyeStats(blue: 3, teal: 2));
            dyeStats.Add(ItemID.GrimDye, new DyeStats(red: 3, brown: 4));
            dyeStats.Add(ItemID.HadesDye, new DyeStats(skyblue: 4, cyan: 3));
            dyeStats.Add(ItemID.BurningHadesDye, new DyeStats(orange: 4, yellow: 3));
            dyeStats.Add(ItemID.ShadowflameHadesDye, new DyeStats(purple: 4, violet: 3));
            dyeStats.Add(ItemID.LivingOceanDye, new DyeStats(skyblue: 1, blue: 4));
            dyeStats.Add(ItemID.LivingFlameDye, new DyeStats(red: 4, orange: 1));
            dyeStats.Add(ItemID.LivingRainbowDye, new DyeStats(lime: 3, orange: 3, yellow: 3, blue: 3, cyan: 3, skyblue: 3, purple: 3, violet: 3, pink: 3));
            dyeStats.Add(ItemID.MartianArmorDye, new DyeStats(teal: 4, cyan: 2));
            dyeStats.Add(ItemID.MidnightRainbowDye, new DyeStats(lime: -1, orange: -1, yellow: -1, blue: -1, cyan: -1, skyblue: -1, purple: -1, violet: -1, pink: -1));
            dyeStats.Add(ItemID.MirageDye, new DyeStats(red: 2, blue: 2, green: 2));
            dyeStats.Add(ItemID.NegativeDye, new DyeStats(lime: -3, orange: -3, yellow: -3, blue: -3, cyan: -3, skyblue: -3, purple: -3, violet: -3, pink: -3));
            dyeStats.Add(ItemID.PixieDye, new DyeStats(yellow: 5));
            dyeStats.Add(ItemID.PhaseDye, new DyeStats(purple: -4));
            dyeStats.Add(ItemID.PurpleOozeDye, new DyeStats(purple: 5));
            dyeStats.Add(ItemID.ReflectiveDye, new DyeStats(silver: 2));
            dyeStats.Add(ItemID.ReflectiveCopperDye, new DyeStats(orange: 4, brown: 4));
            dyeStats.Add(ItemID.ReflectiveGoldDye, new DyeStats(yellow: 4, brown: 4));
            dyeStats.Add(ItemID.ReflectiveObsidianDye, new DyeStats(blue: -4));
            dyeStats.Add(ItemID.ReflectiveMetalDye, new DyeStats(silver: -4));
            dyeStats.Add(ItemID.ShadowDye, new DyeStats(black: 22));
            dyeStats.Add(ItemID.ShiftingSandsDye, new DyeStats(orange: 3, yellow: 3, brown: 3));
            dyeStats.Add(3024, new DyeStats(purple: 6));
            dyeStats.Add(ItemID.TwilightDye, new DyeStats(purple: -5));
            dyeStats.Add(ItemID.WispDye, new DyeStats(silver: 5));
            dyeStats.Add(ItemID.InfernalWispDye, new DyeStats(red: 3, yellow: 4));
            dyeStats.Add(ItemID.UnicornWispDye, new DyeStats(pink: 3, violet: 4));
            // Crafted
            dyeStats.Add(ItemID.PinkGelDye, new DyeStats(pink: 3, blue: 1));
            dyeStats.Add(ItemID.ShiftingPearlSandsDye, new DyeStats(pink: 3, orange: 3, yellow: 3, brown: 3));
            dyeStats.Add(ItemID.NebulaDye, new DyeStats(pink: 5, purple: 5, violet: 5));
            dyeStats.Add(ItemID.SolarDye, new DyeStats(red: 5, orange: 5, yellow: 5));
            dyeStats.Add(ItemID.VortexDye, new DyeStats(green: 5, teal: 5, lime: 5));
            dyeStats.Add(ItemID.StardustDye, new DyeStats(blue: 5, skyblue: 5, cyan: 5));
            dyeStats.Add(ItemID.VoidDye, new DyeStats(green: -20));
            // Other
            dyeStats.Add(ItemID.LokisDye, new DyeStats(brown: 5));
            dyeStats.Add(ItemID.TeamDye, new DyeStats(red: -22, pink: -22, orange: -22, blue: -22, green: -22, silver: -22));
            dyeStats.Add(ItemID.BloodbathDye, new DyeStats(red: 6));
            dyeStats.Add(ItemID.FogboundDye, new DyeStats(silver: 6));
            dyeStats.Add(4778, new DyeStats(blue: 4, yellow: 4));
            // Calamity
            dyeStats.Add(ItemType<AerialiteDye>(), new DyeStats(skyblue: 2, cyan: 2));
            dyeStats.Add(ItemType<AstralBlueDye>(), new DyeStats(blue: 3, orange: 2));
            dyeStats.Add(ItemType<AstralOrangeDye>(), new DyeStats(blue: 2, orange: 3));
            dyeStats.Add(ItemType<AstralSwirlDye>(), new DyeStats(blue: 3, orange: 3));
            dyeStats.Add(ItemType<AstralDye>(), new DyeStats(blue: 2, orange: 2));
            dyeStats.Add(ItemType<AuricDye>(), new DyeStats(yellow: 7));
            dyeStats.Add(ItemType<BloodflareDye>(), new DyeStats(red: 4, orange: 2, violet: 2));
            dyeStats.Add(ItemType<BlueCosmicFlameDye>(), new DyeStats(blue: 6, pink: 2));
            dyeStats.Add(ItemType<PinkCosmicFlameDye>(), new DyeStats(pink: 6, blue: 2));
            dyeStats.Add(ItemType<SwirlingCosmicFlameDye>(), new DyeStats(pink: 6, blue: 6));
            dyeStats.Add(ItemType<BlueStatigelDye>(), new DyeStats(blue: 3, pink: 1));
            dyeStats.Add(ItemType<PinkStatigelDye>(), new DyeStats(pink: 3, blue: 1));
            dyeStats.Add(ItemType<SlimeGodDye>(), new DyeStats(blue: 2, pink: 2));
            dyeStats.Add(ItemType<BrimflameDye>(), new DyeStats(red: 2, pink: 2));
            dyeStats.Add(ItemType<CalamitousDye>(), new DyeStats(red: 7, pink: 1));
            dyeStats.Add(ItemType<CeaselessDye>(), new DyeStats(black: 69));
            dyeStats.Add(ItemType<CosmiliteDye>(), new DyeStats(blue: 5, pink: 5));
            dyeStats.Add(ItemType<CryonicDye>(), new DyeStats(skyblue: 3, teal: 3));
            dyeStats.Add(ItemType<DefiledFlameDye>(), new DyeStats(green: 5, skyblue: -5555));
            dyeStats.Add(ItemType<DragonSoulDye>(), new DyeStats(orange: 7, yellow: 6));
            dyeStats.Add(ItemType<ElementalDye>(), new DyeStats(blue: 5, orange: 5, green: 5, purple: 5));
            dyeStats.Add(ItemType<EndothermicDye>(), new DyeStats(teal: 6, cyan: 6));
            dyeStats.Add(ItemType<ExoDye>(), new DyeStats(blue: 7, yellow: 7, red: 3, green: 7, orange: -7, violet: -7, cyan: -3, skyblue: -7));
            dyeStats.Add(ItemType<NecroplasmicDye>(), new DyeStats(pink: 6, violet: 4));
            dyeStats.Add(ItemType<NightmareDye>(), new DyeStats(orange: 5, yellow: 5));
            dyeStats.Add(ItemType<ProfanedFlameDye>(), new DyeStats(orange: 6, yellow: 4));
            dyeStats.Add(ItemType<ProfanedMoonlightDye>(), new DyeStats(teal: 5, green: 5, orange: 5, yellow: 5));
            dyeStats.Add(ItemType<ReaverDye>(), new DyeStats(green: 4, lime: 3));
            dyeStats.Add(ItemType<ShadowspecDye>(), new DyeStats(purple: 10, red: -10));
            dyeStats.Add(ItemType<StratusDye>(), new DyeStats(blue: 5, skyblue: -5));
            // Remix
            dyeStats.Add(ItemType<LucreciaDye>(), new DyeStats(purple: 10, pink: 10));
        }
        private static void ManageItemsInUse(Player player, Item h, Item m, ref int c)
        {
            if (Held(player, ItemType<FiberBaby>()))
            {
                Item item = (h.type == ItemType<FiberBaby>()) ? h : m;
                if (player.ownedProjectileCounts[ProjectileType<FiberBabyHoldout>()] < 1)
                    Projectile.NewProjectile(player.GetSource_FromThis(), player.Center + player.DirectionTo(Main.MouseWorld) * 16f, Vector2.Zero, ModContent.ProjectileType<FiberBabyHoldout>(), item.damage, 0, player.whoAmI);
            }
            if (Held(player, ItemType<TheSimpstring>()))
            {
                Item item = (h.type == ItemType<TheSimpstring>()) ? h : m;
                if (player.ownedProjectileCounts[ProjectileType<TheSimpstringHoldout>()] < 1)
                    Projectile.NewProjectile(player.GetSource_FromThis(), player.Center + player.DirectionTo(Main.MouseWorld) * 16f, Vector2.Zero, ModContent.ProjectileType<TheSimpstringHoldout>(), item.damage, 0, player.whoAmI);
            }
            if (!player.channel)
                c = 0;
            else if (Held(player, ItemType<Deicide>()))
            {
                if (c < 3601)
                    c++;
            }
            else if (Held(player, ItemType<DualCane>()))
            {
                if (c < 40)
                {
                    if (c == 0)
                        SoundEngine.PlaySound(SoundID.DD2_DarkMageHealImpact);
                    c++;
                }
            }
            else if (Held(player, ItemType<SDOMG>()))
            {
                if (c < 600 && player.altFunctionUse != 2)
                    c++;
            }
        }
        private static bool Held(Player player, int id) => id == player.HeldItem.type || id == Main.mouseItem.type;
    }
}