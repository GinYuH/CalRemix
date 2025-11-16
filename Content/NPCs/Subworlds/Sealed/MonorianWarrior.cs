using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Terraria.Localization;
using CalRemix.Core.World;
using CalRemix.Content.Projectiles.Hostile;
using CalamityMod.World;
using CalamityMod.InverseKinematics;
using Terraria.GameContent.Bestiary;
using CalRemix.Core.Biomes;
using CalRemix.UI;
using CalamityMod.Sounds;

namespace CalRemix.Content.NPCs.Subworlds.Sealed
{
	[AutoloadBossHead]

	public class MonorianWarrior : ModNPC
	{
		public bool Phase2
		{ 
			get => NPC.Calamity().newAI[0] == 1;
			set => NPC.Calamity().newAI[0] = value.ToInt();
		}

		public ref float Phase => ref NPC.ai[0];
		public ref float Timer => ref NPC.ai[1];

		public enum PhaseType
        {
            SpawnAnim = 0,
            Idle = 11,
			Tank = 1,
			Hover = 2,
			Drones = 3,
			Teleport = 4,
			Jump = 5,
			Knives = 6,
			Bolt = 7,
			Catchup = 8,
			TankEX = 9,
			Supercharge = 10
		}

		public PhaseType CurrentPhase
		{
			get => (PhaseType)Phase;
			set => Phase = (float)value;
		}

		public override void SetDefaults()
		{
			NPC.damage = 10;
			NPC.npcSlots = 0f;
			NPC.width = 28;
			NPC.height = 48;
			NPC.defense = 26;
			NPC.lifeMax = 30000;
			NPC.aiStyle = -1;
			NPC.knockBackResist = 0f;
			NPC.value = Item.buyPrice(0, 0, 10, 0);
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath14;
			NPC.boss = true;
			NPC.DR_NERD(0.1f);
			SpawnModBiomes = [ModContent.GetInstance<VoidForestBiome>().Type];
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

		public override void AI()
		{
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead)
			{
				NPC.TargetClosest(true);
			}
			if (Phase2 && Main.rand.NextBool(5))
			{
				Dust dust = Main.dust[Terraria.Dust.NewDust(NPC.Center, 30, 30, DustID.TintableDustLighted, 0f, 0f, 0, new Color(0, 177, 255), 3.5f)];
				dust.noGravity = true;
				dust.noLight = false;
			}
			if (NPC.alpha > 0)
			{
				NPC.dontTakeDamage = true;
			}
			else
			{
				NPC.dontTakeDamage = false;
			}
			if (Main.player[NPC.target].dead)
			{
				NPC.active = false;
            }
            if (!Phase2 && NPC.life <= NPC.lifeMax * 0.5f && Main.expertMode && CurrentPhase != PhaseType.Supercharge)
            {
                Timer = 0;
                NPC.ai[2] = 0;
                NPC.ai[3] = 0;
                CurrentPhase = PhaseType.Supercharge;
            }
            switch (CurrentPhase)
			{
				case PhaseType.SpawnAnim:
					{
						int gorb = NPC.FindFirstNPC(ModContent.NPCType<BrightMind>());
						if (gorb != -1)
						{
							Vector2 startPos = new Vector2(NPC.Calamity().newAI[1], NPC.Calamity().newAI[2]);
							if (startPos == Vector2.Zero)
							{
								NPC.Calamity().newAI[1] = NPC.Center.X;
								NPC.Calamity().newAI[2] = NPC.Center.Y;
								NPC.netUpdate = true;
							}
							else if (Main.npc[gorb].velocity == Vector2.Zero)
							{
								NPC.Center = Vector2.Lerp(startPos, Main.npc[gorb].Left, Timer / 20f);
							}
						}
						else
						{
							Timer = 0;
							CurrentPhase = PhaseType.Idle;
							NPC.velocity = Vector2.Zero;
							NPC.netUpdate = true;
						}
					}
					break;
				case PhaseType.Idle:
					{
						float playerX = Main.player[NPC.target].position.X - (NPC.position.X + NPC.width);
						float playerY = Main.player[NPC.target].position.Y - (NPC.position.Y + NPC.height);
						if (Timer >= 160)
						{
							NPC.TargetClosest();
						}
						if (!RemixDowned.downedOneguy)
						{
							if (Timer == 20)
							{
								NPCDialogueUI.StartDialogue(NPC.whoAmI, "Intro");
							}
							if (Timer > 20 && Timer % 7 == 0 && NPCDialogueUI.NotFinishedTalking(NPC))
							{
								SoundEngine.PlaySound(BetterSoundID.ItemAerialBane with { Pitch = 3 }, NPC.Center);
							}
							if (Timer >= 22 && !NPCDialogueUI.IsBeingTalkedTo(NPC))
							{
								Timer = 0;
								CurrentPhase = PhaseType.Tank;
							}
						}
						if (Timer >= 50 && RemixDowned.downedOneguy)
						{
							Timer = 0;
							NPC.ai[3] = 0;
							CurrentPhase = PhaseType.Tank;
						}
						if (NPC.life < NPC.lifeMax * 0.99f || playerX > 600 | playerX < -600 || playerY > 600 || playerY < -600)
                        {
                            NPCDialogueUI.StartDialogue(NPC.whoAmI, "Enrage");
                            Timer = 0;
							NPC.ai[3] = 0;
							CurrentPhase = PhaseType.Tank;
						}
					}
				break;

				case PhaseType.Tank:
					{
						NPC.damage = 60;
						NPC.TargetClosest(false);
						NPC.ai[3]++;
						NPC.noGravity = true;
						NPC.noTileCollide = true;
						float chargeDuration = Phase2 ? 110 : 150;
						float localTimer = Timer % chargeDuration;
						float playerX = Main.player[NPC.target].position.X - (NPC.position.X + NPC.width);
						float playerY = Main.player[NPC.target].position.Y - (NPC.position.Y + NPC.height);
						if (Timer == 1)
						{
							SoundEngine.PlaySound(SoundID.Item23, NPC.position);
							NPC.direction = -NPC.DirectionTo(Main.player[NPC.target].Center).X.DirectionalSign();
						}
						int extraboost = Phase2 ? 2 : 0;
						float speed = CalamityMod.World.CalamityWorld.revenge ? (6f + extraboost + (Timer / 50)) : (4f + extraboost + (Timer / 50));
						if (Timer >= 1)
							NPC.velocity.X = NPC.direction * speed;
						int num858 = 80;
						int num859 = 20;
						Vector2 position4 = new Vector2(NPC.Center.X - (float)(num858 / 2), NPC.position.Y + (float)NPC.height - (float)num859);
						bool flag50 = false;
						if (NPC.position.X < Main.player[NPC.target].position.X && NPC.position.X + (float)NPC.width > Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width && NPC.position.Y + (float)NPC.height < Main.player[NPC.target].position.Y + (float)Main.player[NPC.target].height - 16f)
						{
							flag50 = true;
						}
						if (flag50)
						{
							NPC.velocity.Y += 0.5f;
						}
						if (Collision.SolidCollision(position4, num858, num859))
						{
							if (NPC.velocity.Y > 0f)
							{
								NPC.velocity.Y = 0f;
							}
							if ((double)NPC.velocity.Y > -0.2)
							{
								NPC.velocity.Y -= 0.025f;
							}
							else
							{
								NPC.velocity.Y -= 0.2f;
							}
							if (NPC.velocity.Y < -4f)
							{
								NPC.velocity.Y = -4f;
							}
						}
						else
						{
							if (NPC.velocity.Y < 0f)
							{
								NPC.velocity.Y = 0f;
							}
							if ((double)NPC.velocity.Y < 0.1)
							{
								NPC.velocity.Y += 0.025f;
							}
							else
							{
								NPC.velocity.Y += 0.5f;
							}
						}
						if (NPC.velocity.Y > 10f)
						{
							NPC.velocity.Y = 10f;
						}
						if (localTimer == 1 && Timer < 295)
						{
							SoundEngine.PlaySound(SoundID.Item23, NPC.position);
							NPC.direction *= -1;
						}
						/*if (((playerX > 1200 || playerY < -360 || playerX < -1200) && Main.expertMode) || ((playerX > 1200 || playerY < -260 || playerX < -1200) && CalamityMod.World.CalamityWorld.revenge))
						{
							NPC.velocity.Y -= 0.8f;
							CurrentPhase = PhaseType.TankEX;
							Timer = 0;
						}*/
						if (Timer >= 300 || Timer >= 900)
						{
							Timer = 0;
							CurrentPhase = PhaseType.Hover;
						}
					}
				break;

				case PhaseType.Hover:
					{
						NPC.damage = 0;
						NPC.TargetClosest();
						NPC.noGravity = true;
						NPC.noTileCollide = true;
						float speed = 10f;
						int phaseTime = 360;
						Vector2 playerPos = Main.player[NPC.target].Center - NPC.Center;
						if (playerPos.Y < 100f)
						{
							NPC.velocity.Y -= 0.1f;
						}
						if (playerPos.Y > 200f)
						{
							NPC.velocity.Y += 0.1f;
						}
						if (NPC.velocity.Y > 2f)
						{
							NPC.velocity.Y = 2f;
						}
						if (NPC.velocity.Y < -4f)
						{
							NPC.velocity.Y = -4f;
						}

						if (playerPos.X < 0f)
						{
							NPC.velocity.X -= 0.1f;
						}
						if (playerPos.X > 0f)
						{
							NPC.velocity.X += 0.1f;
						}
						if (NPC.velocity.X > 6f)
						{
							NPC.velocity.X = 6f + (playerPos.X * 0.001f);
						}
						if (NPC.velocity.X < -6f)
						{
							NPC.velocity.X = -6f + (playerPos.X * 0.001f);
						}
						if (CalamityMod.World.CalamityWorld.death)
						{
							Timer++;
						}
						if (Timer % (Phase2 ? 30 : 70) == 0)
						{
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionTo(Main.player[NPC.target].Center) * speed, ModContent.ProjectileType<MonorianBolt>(), CalRemixHelper.ProjectileDamage(210, 380), 0f);
						}
						if (Timer >= phaseTime)
						{
							Timer = 0;
								CurrentPhase = PhaseType.Drones;
							}
					}
				break;

				case PhaseType.Drones:
					{
						NPC.noGravity = true;
						NPC.noTileCollide = false;
						NPC.velocity.X = 0;
						NPC.velocity.Y = 0;

						int droneAmt = CalamityWorld.revenge ? 4 : Main.expertMode ? 2 : 1;
						if (Phase2)
							droneAmt *= 1;

						if (Timer == 40)
						{
							SoundEngine.PlaySound(SoundID.Item15, NPC.position);
							for (int i = 0; i < droneAmt; i++)
							{
								if (Main.netMode != NetmodeID.MultiplayerClient)
								{
									float ai = MathHelper.Lerp(MathHelper.PiOver4, MathHelper.TwoPi + MathHelper.PiOver4, i / (float)droneAmt);
									if (droneAmt == 1)
									{
										ai = Main.rand.Next(2, 4);
									}
									NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<MonorianGastropodMini>(), ai3: ai);
								}
							}
						}

						if (Timer >= 120) //Go invisible
						{
							NPC.alpha += 12;
						}
						if (NPC.alpha >= 255)
						{
							Timer = 0;
								CurrentPhase = PhaseType.Teleport;
							}
					}
				break;

				case PhaseType.Teleport:
					{
						NPC.TargetClosest();
						NPC.damage = 0;
						NPC.position.X = Main.player[NPC.target].Center.X - 120;
						NPC.position.Y = Main.player[NPC.target].Center.Y - 16;
						if ((Timer == 67 | Timer == 157) && (CalamityMod.World.CalamityWorld.revenge || (Main.expertMode && Phase2)))
						{
							SoundEngine.PlaySound(SoundID.Item43, Main.player[NPC.target].Center);
						}
						if (!NPC.AnyNPCs(ModContent.NPCType<MonorianGastropodMini>()) || CalamityMod.World.CalamityWorld.death ? Timer >= 240 : Timer >= 300)
						{
							NPC.alpha -= 2;
							if (NPC.alpha <= 0)
							{
								Timer = 0;
									CurrentPhase = PhaseType.Jump;
								for (int x = 0; x < 20; x++)
								{
									SoundEngine.PlaySound(SoundID.Item96, NPC.position);
									Dust dust;
									Vector2 position = NPC.Center;
									position.X = NPC.Center.X - 4;
									dust = Terraria.Dust.NewDustPerfect(position, 43, new Vector2(Main.rand.Next(-20, 20), Main.rand.Next(-20, 20)), 0, new Color(0, 177, 255), 1.718605f);
									dust.noGravity = true;
								}
							}
						}
					}
					break;
				case PhaseType.Jump:
					{
						NPC.noGravity = true;
						NPC.noTileCollide = true;
						if (Timer <= 2)
						{
							NPC.velocity.Y = -4;
							NPC.velocity.X = -6;
						}
						if (Timer >= 20)
						{
							Timer = 0;
								CurrentPhase = PhaseType.Knives;
							}
					}
					break;
				case PhaseType.Knives:
					{
						if (NPC.velocity.X < 0)
						{
							NPC.velocity.X += 0.1f;
						}
						if (NPC.velocity.Y < 0)
						{
							NPC.velocity.X += 0.1f;
						}
						NPC.alpha = 0;
						NPC.TargetClosest();
						NPC.noGravity = false;
						NPC.noTileCollide = false;
						int ranshotspeed = (CalamityMod.World.CalamityWorld.revenge ? 68 : 85);
						if (Timer % ranshotspeed == 0)
						{
							SoundEngine.PlaySound(CommonCalamitySounds.SwiftSliceSound, NPC.position);
							Vector2 position = NPC.Center;
							Vector2 targetPosition = Main.player[NPC.target].Center;
							Vector2 direction = targetPosition - position;
							direction.Normalize();
							int type = ModContent.ProjectileType<ArkhalisProj>();
							int spacer = CalamityMod.World.CalamityWorld.revenge || Phase2 ? Main.rand.Next(3, 10) * NPC.direction : 5 * NPC.direction;
							for (int spacing = 0; spacing < 10; spacing++)
							{
								Projectile.NewProjectile(NPC.GetSource_FromAI(), position.X, position.Y, (NPC.direction * spacing * (CalamityMod.World.CalamityWorld.revenge ? 2.5f : 3)) - spacer, -9, type, CalRemixHelper.ProjectileDamage(170, 310), 0f, Main.myPlayer);
							}
						}
						if (Timer >= 300)
						{
							Timer = 0;
							CurrentPhase = PhaseType.Bolt;
						}
					}
					break;
				case PhaseType.Bolt:
					{
						float cycleLength = 90;
						float localTimer = Timer % cycleLength;
						float totalTime = cycleLength * 4;
                        int projRate = 45;
                        if (localTimer == 1)
						{
							for (int dusttimer = 0; dusttimer < 10; dusttimer++)
							{
								int dusty = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.RedTorch, 0f, 0f, 100, default, 3f);
								Main.dust[dusty].noGravity = true;
							}
						}
						if (localTimer == 2)
						{
							SoundEngine.PlaySound(SoundID.Item8, NPC.position);
							NPC.position.X = Main.player[NPC.target].Center.X + 500 * NPC.DirectionTo(Main.player[NPC.target].Center).X.DirectionalSign();
							NPC.position.Y = Main.player[NPC.target].Center.Y - 80;
							NPC.direction = NPC.DirectionTo(Main.player[NPC.target].Center).X.DirectionalSign();
                            for (int dusttimer = 0; dusttimer < 10; dusttimer++)
							{
								int dusty = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.RedTorch, 0f, 0f, 100, default, 3f);
								Main.dust[dusty].noGravity = true;
                            }
                        }
						if (localTimer == 3)
                        {
                            for (int dusttimer = 0; dusttimer < 10; dusttimer++)
							{
								int dustf = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.RedTorch, 0f, 0f, 100, default, 3f);
								Main.dust[dustf].noGravity = true;
							}
						}
						NPC.velocity.X = 0;
						NPC.TargetClosest(false);
						if (localTimer >= 30 || Main.expertMode)
						{
							if (localTimer % projRate == 0)
							{
								SoundEngine.PlaySound(SoundID.Item43, NPC.position);
								int projCount = ((Main.expertMode && Main.rand.Next(4) == 0) || CalamityMod.World.CalamityWorld.revenge || Phase2) ? 5 : 3;

								for (int i = 0; i < projCount; i++)
								{
									if (Main.netMode != NetmodeID.MultiplayerClient)
									{
										Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(NPC.direction, NPC.DirectionTo(Main.player[NPC.target].Center).Y).RotatedBy(MathHelper.Lerp(-MathHelper.PiOver4, MathHelper.PiOver4, i / (float)(projCount - 1))) * 8, ModContent.ProjectileType<MonorianBolt>(), CalRemixHelper.ProjectileDamage(240, 410), 1);
									}
								}
							}
						}
						if (Timer >= totalTime - 2)
						{
							Timer = 0;
                            CurrentPhase = PhaseType.Tank;
                        }
					}
					break;
				case PhaseType.Catchup:
					{
						NPC.damage = 0;
						NPC.TargetClosest();
						NPC.noGravity = true;
						NPC.noTileCollide = true;
						float playerY = Main.player[NPC.target].position.Y - (NPC.position.Y + NPC.height);
						float playerX = Main.player[NPC.target].position.X - (NPC.position.X + NPC.width);
						if (playerY < 20)
						{
							NPC.velocity.Y -= 0.3f;
						}
						else
						{
							NPC.velocity.Y += 0.01f;
						}
						if (NPC.velocity.Y > 10f)
						{
							NPC.velocity.Y = 10f;
						}
						if (NPC.velocity.Y < -10f)
						{
							NPC.velocity.Y = -10f;
						}

						if (playerX < 0f)
						{
							NPC.velocity.X -= 0.2f;
						}
						if (playerX > 0f)
						{
							NPC.velocity.X += 0.2f;
						}
						if (NPC.velocity.X > 25f)
						{
							NPC.velocity.X = 25f + (playerX * 0.001f);
						}
						if (NPC.velocity.X < -25f)
						{
							NPC.velocity.X = -25f + (playerX * 0.001f);
						}
						if (playerX < 680 && playerY > 40 && playerX > -680)
						{
							SoundEngine.PlaySound(SoundID.Item23, NPC.position);
							NPC.velocity.Y = 0;
							Timer = 0;
                            CurrentPhase = PhaseType.Hover;
                        }
						if (Timer >= 360)
						{
							Timer = 0;
                            CurrentPhase = PhaseType.Hover;
                        }
					}
					break;
				case PhaseType.TankEX:
					{
						//Orbital strike!!!
						Vector2 position = NPC.Center;
						Vector2 targetPosition = Main.player[NPC.target].Center;
						Vector2 direction = targetPosition - position;
						direction.Normalize();
						NPC.damage = 60;
						NPC.direction = NPC.velocity.X.DirectionalSign();
						NPC.TargetClosest();
						NPC.noGravity = true;
						NPC.noTileCollide = true;
						float playerX = Main.player[NPC.target].position.X - (NPC.position.X + NPC.width);
						float playerY = Main.player[NPC.target].position.Y - (NPC.position.Y + NPC.height);
						if (Timer == 1)
						{
							SoundEngine.PlaySound(SoundID.Item23, NPC.position);
							float speed = CalamityMod.World.CalamityWorld.revenge ? 16f : 12f;
							NPC.velocity = NPC.DirectionTo(targetPosition) * speed;
						}
						if (playerX > 1600 || playerY < -360 || playerX < -1600 || Timer >= 70)
						{
							NPC.velocity.X = 0f;
							NPC.velocity.Y -= 0.8f;
							CurrentPhase = PhaseType.Catchup;
						}
					}
					break;
				case PhaseType.Supercharge:
					{
						NPC.noGravity = false;
						NPC.noTileCollide = false;
						NPC.dontTakeDamage = true;
						NPC.velocity.X = 0;
						NPC.velocity.Y += 0.1f;
						if (((NPC.collideX || NPC.collideY) && Timer < 180) || (Timer >= 180))
						{
							if (Timer <= 100)
							{
								if (Timer % 35 == 0)
								{
									SoundEngine.PlaySound(SoundID.Item43, NPC.position);
									for (int x = 0; x < 20; x++)
									{
										Dust dust;
										Vector2 position = NPC.Center;
										position.X = NPC.Center.X - 4;
										dust = Terraria.Dust.NewDustPerfect(position, 43, new Vector2(Main.rand.Next(-30, 30), Main.rand.Next(-30, 30)), 0, new Color(0, 177, 255), 1.718605f);
										dust.noGravity = true;
									}
								}
							}
							else if (Timer > 100)
							{
								if (Timer % 20 == 0)
								{
									SoundEngine.PlaySound(SoundID.Item43, NPC.position);
									for (int x = 0; x < 20; x++)
									{
										Dust dust;
										Vector2 position = NPC.Center;
										position.X = NPC.Center.X - 4;
										dust = Terraria.Dust.NewDustPerfect(position, 43, new Vector2(Main.rand.Next(-30, 30), Main.rand.Next(-30, 30)), 0, new Color(0, 177, 255), 1.718605f);
										dust.noGravity = true;
									}
								}
							}
							if (Timer < 160)
							{
								if (Timer % 5 == 0)
								{
									Dust dust;
									Vector2 position = NPC.Center;
									position.Y = NPC.Center.Y + (NPC.height / 2);
									position.X = NPC.Center.X - 4;
									for (int i = 0; i < 8; i++)
									{
										dust = Terraria.Dust.NewDustPerfect(position, DustID.GemRuby, Vector2.UnitY.RotatedBy(MathHelper.Lerp(0, MathHelper.TwoPi, i / 8f)) * 2.5f, 0, new Color(255, 255, 255), 1f);
										dust.noGravity = true;
									}
								}
							}
						}
						if (Timer >= 160)
						{
							for (int x = 0; x < 40; x++)
							{
								Dust dust;
								Vector2 position = NPC.Center;
								position.X = NPC.Center.X - 4;
								dust = Terraria.Dust.NewDustPerfect(position, 43, new Vector2(Main.rand.Next(-20, 20), Main.rand.Next(-20, 20)), 0, new Color(0, 177, 255), 4.718605f);
								dust.noGravity = true;
							}
							SoundEngine.PlaySound(SoundID.Item117, NPC.position);
							Phase2 = true;
							Timer = 0;
                            CurrentPhase = PhaseType.Tank;
                        }
					}
					break;
			}
			Timer++;
			NPC.spriteDirection = -NPC.direction;
		}

		public override void OnKill()
		{
			SoundEngine.PlaySound(BetterSoundID.ItemSentrySummonStrong with { Pitch = -0.4f });
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<MonorianSoul>());
			}
			CalRemixHelper.DustExplosionOutward(NPC.Center, DustID.TintableDustLighted, speedMax: 20, speedMin: 15, color: Color.Cyan, scaleMin: 1f, scaleMax: 1.1f);
		}

        public override bool CheckActive()
        {
			return !NPC.HasValidTarget;
        }

        public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Phase2);
			writer.Write(NPC.Calamity().newAI[0]);
            writer.Write(NPC.Calamity().newAI[1]);
            writer.Write(NPC.Calamity().newAI[2]);
        }
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Phase2 = reader.ReadBoolean();
			NPC.Calamity().newAI[0] = reader.ReadSingle();
            NPC.Calamity().newAI[1] = reader.ReadSingle();
            NPC.Calamity().newAI[2] = reader.ReadSingle();
        }
	}
}
