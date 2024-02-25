using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Chat;
using Terraria.Localization;
using CalamityMod.NPCs;
using CalamityMod;

namespace CalRemix.NPCs.Bosses.Poly
{
	// The main part of the boss, usually refered to as "body"
	[AutoloadBossHead] // This attribute looks for a texture called "ClassName_Head_Boss" and automatically registers it as the NPC boss head icon
	public class Polyphemalus : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Polyphemalus");

			// Add this in for bosses that have a summon item, requires corresponding code in the item (See MinionBossSummonItem.cs)
			NPCID.Sets.MPAllowedEnemies[Type] = true;
			// Automatically group with other bosses
			NPCID.Sets.BossBestiaryPriority.Add(Type);

            // Specify the debuffs it is immune to
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
        }

		public override void SetDefaults()
		{
			NPC.width = 302;
			NPC.height = 318;
			NPC.damage = 0;
			NPC.defense = 10;
			NPC.lifeMax = 50000;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0f;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.value = 0;
			NPC.SpawnWithHigherTime(30);
			NPC.boss = true;
			NPC.aiStyle = -1;
            NPC.Calamity().canBreakPlayerDefense = true;
            if (!Main.dedServ)
            {
				if (Main.zenithWorld)
					Music = MusicLoader.GetMusicSlot("CalRemix/Sounds/Music/TheEyesareAnger");
                else
                    Music = MusicLoader.GetMusicSlot("CalRemix/Sounds/Music/EyesofFlame");
            }
        }

		public override bool CanHitPlayer(Player target, ref int cooldownSlot)
		{
			cooldownSlot = ImmunityCooldownID.Bosses; // use the boss immunity cooldown counter, to prevent ignoring boss attacks by taking damage from other sources
			return true;
        }

		private int timerMax;
		public ref float timer => ref NPC.ai[1];
		public ref float phase => ref NPC.ai[2];

		public ref float hpThreshold => ref NPC.ai[3];

		private bool SoC = false;

		private bool OnlySoC = false;

		private bool enraged = false;

		private int contactDmg = 100;
        private int subphase = 0;
        private float greenRotate = (float)Math.PI;
        private float blueRotate = -MathHelper.PiOver2;
		private float grayRotate = MathHelper.PiOver2;

		private int RedLaser = ProjectileID.DeathLaser;
		private int BlueIce = ProjectileID.FrostWave;
		private int GrayShadow = ProjectileID.CultistBossFireBallClone;
		private int GreenFireball = ProjectileID.WoodenArrowHostile;
		private int GreenFlamethrower = ProjectileID.EyeFire;

		private int bhType = 0;
		private int phaseTransitionTime = 0;

		public override void OnSpawn(IEntitySource source)
		{
			NPC.TargetClosest();
			Player player = Main.player[NPC.target];
			phase = -1;
            /*
			if (player.dash == 2)
			{
				string text = "Showing disrespect, the corpse of their brother enraged them even further!";
                if (Main.netMode == NetmodeID.Server)
                    ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(text), new Color(175, 75, 255));
                else if (Main.netMode == NetmodeID.SinglePlayer)
                    Main.NewText(text, new Color(175, 75, 255));

                SoC = true;
			}
			 */
        }
        public override void AI()
		{
			// This should almost always be the first code in AI() as it is responsible for finding the proper player target
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
			{
				NPC.TargetClosest();
			}

			Player player = Main.player[NPC.target];

			if (player.dead)
			{
				// If the targeted player is dead, flee
				NPC.velocity.Y -= 0.04f;
				// This method makes it so when the boss is in "despawn range" (outside of the screen), it despawns in 10 ticks
				NPC.EncourageDespawn(10);
				return;
			}
			bool enrage = CheckEnrage(player);
			if (enrage)
				NPC.GetGlobalNPC<CalamityGlobalNPC>().CurrentlyEnraged = true;
			else
				NPC.GetGlobalNPC<CalamityGlobalNPC>().CurrentlyEnraged = false;
            Vector2 abovePlayer = (player.Center + new Vector2(0, -500));
            if (phase == -1)
            {
                timer++;
                MoveTowards(abovePlayer, 20f, 40f);
                TurnTowards(PredictiveMotion(player, 20f));
                if (Vector2.Distance(NPC.Center, abovePlayer) <= 48)
                {
                    NPC.Center = (NPC.Center * 3 + abovePlayer) / 4;
                }
                if (timer > 239)
                {
                    timer = 0;
                    phase = 0;
                }
            }
			NPC.damage = 0;
            if (phaseTransitionTime == 0)
			{
				
				if (hpThreshold == 0) { 
					if (phase == 0)
					{ 
						AbovePlayerAI(player);
					}
					if (phase == 1 )
					{
                        NPC.damage = (int)(contactDmg * 1.5f);
                        Dash(player);
					}
					if (phase == 2)
					{
						BulletHell(player);
					}
					if (phase  > 2)
					{
						phase = 0;
					}
				}
            } else
			{
                if (phase == 0)
                {
                    NPC.damage = contactDmg;
                    MoveTowards((player.Center + new Vector2(0, -500)), 20, 40);
                }
                if (phase == 2 && bhType == 0)
				{
					if (Vector2.Distance(NPC.Center, abovePlayer) <= 48 * 4)
					{
						NPC.Center = abovePlayer;
					}else {
                        NPC.damage = 0;
                        MoveTowards(abovePlayer, 80, 10);
					}
					}
					phaseTransitionTime--;
			}
        }

		private bool CheckEnrage(Player target) 
		{ 
			if (Main.dayTime)
			{
				return true;
			}
            /*
			if (SoC && target.dash != 2)
			{
				SoC = false;
				OnlySoC = false;
				string text = "Showing disrespect the corpse of their brother enraged them even further!";
                if (Main.netMode == NetmodeID.Server)
                    ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(text), new Color(175, 75, 255));
                else if (Main.netMode == NetmodeID.SinglePlayer)
                    Main.NewText(text, new Color(175, 75, 255));
                return true;
			}
			 */
            return false;

        }
        private void AbovePlayerAI(Player player)
        {

			Vector2 abovePlayer = (player.Center + new Vector2(0, -500));
            NPC.localAI[0] = 120;

			if (subphase == 0)
			{
				timer++;
				MoveTowards(abovePlayer, 20f, 40f);
				TurnTowards(PredictiveMotion(player, 20f));
				if (Vector2.Distance(NPC.Center, abovePlayer) <= 48)
                {
                    NPC.Center = (NPC.Center * 3 + abovePlayer) / 4;
                }
                if (NPC.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					List<int> ShotTimes = new List<int>() { 30, 60, 90, 120, 140, 160, 180, 190, 200, 210, 220, 230, 240 };

					if (ShotTimes.Contains((int)timer - 1))
					{
						ShootRed(RedLaser, 6, 100);
					}
					if (timer >= 270)
					{
						timer = 0;
                        subphase = 1;
                    }
				}

			}
			if (subphase == 1)
			{
				timer++;
                MoveTowards(abovePlayer, 30f, 40f);
                TurnTowards(PredictiveMotion(player, 0f), blueRotate);
                if (Vector2.Distance(NPC.Center, abovePlayer) <= 48)
				{
					NPC.Center = (NPC.Center + abovePlayer) / 2;
				}
                if (NPC.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    List<int> ShotTimes = new List<int>() { 30,60,90,120,150,180,210 };

                    if (ShotTimes.Contains((int)timer - 1))
					{
                        ShootBlue(BlueIce, 3, 100);
						ShootBlue(BlueIce, 3, 100, 22.5f);
						ShootBlue(BlueIce, 6, 75, 45);
                        ShootBlue(BlueIce, 6, 75, 67.5f);
                        ShootBlue(BlueIce, 3, 100, -22.5f);
						ShootBlue(BlueIce, 6, 75, -45);
                        ShootBlue(BlueIce, 6, 75, -67.5f);
                    }
					if (timer >= 240)
					{
						timer = 0;
						subphase = 2;
					}
				}
            }
			if (subphase == 2)
			{
				timer++;
                MoveTowards(abovePlayer, 20f, 40f);
				
				if (Vector2.Distance(NPC.Center, abovePlayer) <= 48)
                {
                    NPC.Center = (NPC.Center * 3 + abovePlayer) / 4;
                }
                if (timer >= 30 && timer < 46) {
					NPC.rotation += (float)(Math.PI / 180 * 11.25);
					if (timer % 2 == 0)
					{
						ShootGreen(GreenFireball, 5, 100);
                        ShootGreen(GreenFireball, 2, 100);
                        ShootGreen(GreenFireball, 7, 100);
                    }
				} else
				{
                    if (timer >= 15 && timer < 30)
                    {
                        NPC.rotation += (float)(Math.PI / 360 * 11.25);
                    } else {
						if (timer < 15)
						{

                            TurnTowards(player.Center, greenRotate);
                        }
						if (timer > 15)
						{
							TurnTowards(player.Center, grayRotate);
						}
                    }
                }
				if (timer >= 60)
				{
					timer = 0;
					subphase = 3;
				}
			}
            if (subphase == 3)
            {
                timer++;
                MoveTowards(abovePlayer, 20f, 40f);
				TurnTowards(player.Center, grayRotate);
                if (Vector2.Distance(NPC.Center, abovePlayer) <= 48)
                {
                    NPC.Center = (NPC.Center * 3 + abovePlayer) / 4;
                }
                if (timer == 30)
                {
                    ShootGray(GrayShadow, 5, 75, 20);
                    ShootGray(GrayShadow, 5, 75, -20);
                    ShootGray(GrayShadow, 5, 75, -60);
                    ShootGray(GrayShadow, 5, 75, 60);
                }

                if (timer >= 60)
                {
                    timer = 0;
                    subphase = 0;
					phase++;
                    phaseTransitionTime = 30;
                }
            }
        }

		private void Dash(Player player)
		{
			int dashcount = 4;
			timer++;
			NPC.rotation += (float)(22.5*Math.PI/180);
			if (timer >= 30)
			{
				NPC.velocity *= 0.9f;
				if (subphase > dashcount && timer == 40)
				{
					NPC.velocity *= 0.25f;
                    ShootCenter(RedLaser, 5, 100);
                    ShootCenter(RedLaser, 5, 100, 22.5f*1);
                    ShootCenter(RedLaser, 5, 100, 45);
                    ShootCenter(RedLaser, 5, 100, 22.5f *3);
                    ShootCenter(RedLaser, 5, 100, 90);
                    ShootCenter(RedLaser, 5, 100, 22.5f * 5);
                    ShootCenter(RedLaser, 5, 100, 45*3);
                    ShootCenter(RedLaser, 5, 100, 22.5f * 7);
                    ShootCenter(RedLaser, 5, 100, 180);
                    ShootCenter(RedLaser, 5, 100, 22.5f * 9);
                    ShootCenter(RedLaser, 5, 100, 45*5);
                    ShootCenter(RedLaser, 5, 100, 22.5f * 11);
                    ShootCenter(RedLaser, 5, 100, 270);
                    ShootCenter(RedLaser, 5, 100, 22.5f * 13);
                    ShootCenter(RedLaser, 5, 100, 45* 7);
                    ShootCenter(RedLaser, 5, 100, 22.5f * 15);
                    ShootCenter(BlueIce, 5, 100);
                    ShootCenter(BlueIce, 5, 100, 45);
                    ShootCenter(BlueIce, 5, 100, 90);
                    ShootCenter(BlueIce, 5, 100, 45 * 3);
                    ShootCenter(BlueIce, 5, 100, 180);
                    ShootCenter(BlueIce, 5, 100, 45 * 5);
                    ShootCenter(BlueIce, 5, 100, 270);
                    ShootCenter(BlueIce, 5, 100, 45 * 7);
                }
			}
			if (timer >= 60 && subphase < dashcount)
			{
                MoveTowards(player.Center, 70, 1);
                    timer = 0;
				subphase++;
            }
            if (timer >= 75 && subphase == dashcount)
            {
				MoveTowards(PredictiveMotion(player, 50), 70, 1);
				timer = 0;
				subphase++;
            }
			if (timer >= 75 && subphase > dashcount)
			{
                {
                    timer = 0;
                    subphase = 0;
                    phase++;
                    phaseTransitionTime = 30;
                }
            }

        }
		private void BulletHell(Player player)
		{
			timer++;
			if (bhType == 0)
			{
				var circlePos = CirclePos(player, (float)(((timer % 90) * 4 - 90) * Math.PI / 180), 500f);
                MoveTowards(circlePos, 80, 10);
                if (Vector2.Distance(NPC.Center, circlePos) <= 48*4)
				{
					NPC.Center = circlePos;
					NPC.velocity = Vector2.Zero;
                }
				if (timer < 180)
				{
					TurnTowards(player.Center, blueRotate);
					if (timer % 8 == 0)
					{
						ShootBlue(BlueIce, 0.5f, 100);
					}
					if (timer % 4 == 0)
					{
						ShootGray(GrayShadow, 5, 100);
					}
				} else
				{
					TurnTowards(player.Center);
                    if (timer % 32 == 0)
                    {
                        ShootRed(RedLaser, 3f, 100);
                    }
                    if (timer % 4 == 0)
                    {
                        ShootGreen(GreenFireball, 5, 100);
                    }
                }
                if (timer > 360)
                {
                    NPC.velocity = new Vector2(50, 0);
                    timer = 0;
                    subphase = 0;
                    phase++;
                    bhType = 1;
					phaseTransitionTime = 30;
                }
            }
			if (bhType == 1) { 
				NPC.rotation = (timer * (float)(Math.PI / 180) * (11.25f / 2));
				NPC.velocity = NPC.velocity * 0.99f;
				if (timer % 4 == 0)
				{
					ShootBlue(ProjectileID.FrostWave, 1, 100);
				}
				if (timer % 2 == 1)
				{
					ShootRed(ProjectileID.DeathLaser, 8, 35);
				}
				if (timer % 8 == 3)
				{
					ShootGray(ProjectileID.CultistBossFireBallClone, 6, 50);
				}
				if (timer % 4 == 2)
				{
					ShootGreen(96, 5, 50);
				}
                if (timer >= 180)
                {
                    timer = 0;
                    subphase = 0;
                    phase++;
                    bhType = 0;
                    phaseTransitionTime = 30;
                }
            }
            
		}

        public override void OnKill()
        {
			NPC.boss = false;
			NPC.NewNPC(base.NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<Astigmageddon>());
            NPC.NewNPC(base.NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<Conjunctivirus>());
            NPC.NewNPC(base.NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<Exotrexia>());
            NPC.NewNPC(base.NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<Cataractacomb>());
        }
        private void TurnTowards(Vector2 goal, float offset = 0, float maxSpeed = 1)
        {
			float goal2 = (goal-NPC.Center).ToRotation() + offset;
            maxSpeed *= (float)Math.PI / 180f;
			float rad360 = (360 * (float)Math.PI / 180f);
			if (goal2 % rad360 + rad360 > NPC.rotation + rad360)
            {
                NPC.rotation += Math.Min((goal2 % rad360 + rad360) - NPC.rotation, maxSpeed+ rad360);
            }
			if (goal2 % rad360 + rad360 < NPC.rotation + rad360)
			{
				NPC.rotation += Math.Min((goal2 % rad360 + rad360) - NPC.rotation, maxSpeed+ rad360);
			}
		}

		private Vector2 CirclePos(Player player, float rotation, float distance)
		{
			return player.position + (rotation).ToRotationVector2() * distance;
		}
		private void MoveTowards(Vector2 goal, float speed, float inertia)
		{
            Vector2 moveTo = (goal-NPC.Center).SafeNormalize(Vector2.UnitY) * speed / 1.5f;
            NPC.velocity = (NPC.velocity * (inertia - 1) + moveTo) / inertia;
		}

		private Vector2 RadToVector(float input)
		{
			return new Vector2((float)Math.Cos(input), (float)Math.Sin(input));
		}
		private Vector2 DegToVector(float input)
		{
            return new Vector2((float)Math.Cos(input* Math.PI / 180), (float)Math.Sin(input* Math.PI / 180));
        }
		private void ShootBlue(int type, float velocityMod, int damage, float spread = 0)
        {
            if (Main.masterMode) damage /= 4;
            else if (Main.expertMode) damage /= 4;
            else damage /= 2;
            Vector2 position = NPC.Center;
			Vector2 Velocity = RadToVector(NPC.rotation - blueRotate) * velocityMod;
            Vector2 offset = Vector2.Normalize(Velocity) * 150f;
            Projectile.NewProjectile(NPC.GetSource_FromAI(), position + offset, Velocity.RotatedBy(spread * Math.PI / 180), type, damage, 0f, Main.myPlayer);
		}
		private void ShootGray(int type, float velocityMod, int damage, float spread = 0)
		{
            if (Main.masterMode) damage /= 4;
            else if (Main.expertMode) damage /= 4;
            else damage /= 2;
            Vector2 position = NPC.Center;
			Vector2 Velocity = RadToVector(NPC.rotation - grayRotate) * velocityMod;
			Vector2 offset = Vector2.Normalize(Velocity) * 150f;
			Projectile.NewProjectile(NPC.GetSource_FromAI(), position + offset, Velocity.RotatedBy(spread * Math.PI / 180), type, damage, 0f, Main.myPlayer);
		}

		private void ShootGreen(int type, float velocityMod, int damage, float spread = 0)
		{
            if (Main.masterMode) damage /= 4;
            else if (Main.expertMode) damage /= 4;
            else damage /= 2;
            Vector2 position = NPC.Center;
			Vector2 Velocity = RadToVector(NPC.rotation + greenRotate) * velocityMod;
            Vector2 offset = Vector2.Normalize(Velocity) * 150f;
            Projectile.NewProjectile(NPC.GetSource_FromAI(), position + offset, Velocity.RotatedBy(spread * Math.PI / 180), type, damage, 0f, Main.myPlayer);
        }

		private void ShootRed(int type, float velocityMod, int damage, float spread = 0)
		{
            if (Main.masterMode) damage /= 4;
            else if (Main.expertMode) damage /= 4;
            else damage /= 2;
            Vector2 position = NPC.Center;
			Vector2 Velocity = RadToVector(NPC.rotation) * velocityMod;
            Vector2 offset = Vector2.Normalize(Velocity) * 150f;
            Projectile.NewProjectile(NPC.GetSource_FromAI(), position + offset, Velocity.RotatedBy(spread * Math.PI / 180), type, damage, 0f, Main.myPlayer);
		}

		private void ShootCenter(int type, float velocityMod, int damage, float spread = 0)
		{
			if (Main.masterMode) damage /= 4;
			else if (Main.expertMode) damage /= 4;
			else damage /= 2;
            Vector2 position = NPC.Center;
            Vector2 Velocity = RadToVector(NPC.rotation) * velocityMod;
            Projectile.NewProjectile(NPC.GetSource_FromAI(), position, Velocity.RotatedBy(spread * Math.PI / 180), type, damage, 0f, Main.myPlayer);
        }

		private Vector2 PredictiveMotion(Player player, float strength)
		{

            return (player.Center + player.velocity * strength);
        }
    }
    public class LastPolyBeaten : IItemDropRuleCondition, IProvideItemConditionDescription
    {
        public bool CanDrop(DropAttemptInfo info)
        {

            List<int> type = new List<int>() {
                ModContent.NPCType<Cataractacomb>(),
                ModContent.NPCType<Exotrexia>(),
                ModContent.NPCType<Conjunctivirus>(),
                ModContent.NPCType<Astigmageddon>()
            };

            type.Remove(info.npc.type);
            foreach (var item in type)
            {
                if (NPC.AnyNPCs(item))
                {
                    return false;
                }
            }
            return true;
        }
        public bool CanShowItemDropInUI() => true;
        public string GetConditionDescription() => null;
    }
}