using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalRemix.Content.Projectiles.Hostile;

namespace CalRemix.Content.NPCs.Subworlds.Sealed
{
	public class MonorianGastropodMini : ModNPC

	{
		public override string Texture => $"Terraria/Images/NPC_{NPCID.Gastropod}";
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Gastropod];
		}
		public override void SetDefaults()
		{
			NPC.damage = 250;
			NPC.npcSlots = 0f;
			NPC.width = 26; //324
			NPC.height = 24; //216
			NPC.defense = 10;
			NPC.lifeMax = 10000;
			NPC.aiStyle = -1; //new\
			AIType = -1; //new
			NPC.knockBackResist = 0.6f;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.DR_NERD(0.1f);
		}

		public override void AI()
		{
			Player target = Main.player[NPC.target];
			NPC.spriteDirection = -NPC.direction;
			NPC.TargetClosest();
			if (NPC.ai[0] == 0)
			{
				float fireRate = 90;
				NPC.ai[1]++;
				CalamityUtils.SmoothMovement(NPC, 5, target.Center + Vector2.UnitY.RotatedBy(NPC.ai[3]) * 300 - NPC.Center, 14, 0.1f, true);
				
				if (NPC.ai[1] % fireRate == 0 && NPC.ai[1] > 1)
				{
					if (NPC.CountNPCS(ModContent.NPCType<MonorianGastropodMini>()) < 3)
					{
						SoundEngine.PlaySound(SoundID.Item43, NPC.position);
					}
					Vector2 targetPosition = Main.player[NPC.target].Center;
					float mainSpeed = 10f;
					float borderSpeed = 6f;
					int damage = CalRemixHelper.ProjectileDamage(250, 430);
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionTo(targetPosition) * mainSpeed, ModContent.ProjectileType<MonorianBolt>(), damage, 1f, Main.myPlayer);
					if (Main.expertMode)
					{
						for (int i = 0; i < 2; i++)
						{
							if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, borderSpeed * NPC.DirectionTo(targetPosition).RotatedBy(MathHelper.Lerp(-MathHelper.PiOver2 * 0.5f, MathHelper.PiOver2 * 0.5f, i / 1f)), ModContent.ProjectileType<MonorianBolt>(), damage, 1f, Main.myPlayer);
                            }
						}
					}
                }
				if (NPC.ai[1] >= 330)
				{
					NPC.ai[1] = 0;
					NPC.ai[0] = 1;
				}
			}
			if (NPC.ai[0] == 1)
            {
				NPC.ai[1]++;
				NPC.velocity.Y = -7;
				if (NPC.ai[1] == 90)
				{
					SoundEngine.PlaySound(SoundID.Item43, NPC.position);
					float speed = 10f;
					if (Main.netMode != NetmodeID.MultiplayerClient)
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionTo(target.Center) * speed, ModContent.ProjectileType<MonorianBolt>(), CalRemixHelper.ProjectileDamage(250, 430), 1f, Main.myPlayer);
				}
			}
		}
		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 1;
			if (NPC.frameCounter > 6)
			{
				NPC.frameCounter = 0.0;
				NPC.frame.Y += frameHeight;
			}
			if (NPC.frame.Y > frameHeight * 5)
			{
				NPC.frame.Y = 0;
			}
		}

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
			return false;
        }
    }
}