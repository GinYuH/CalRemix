using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using CalRemix.Biomes;
using CalRemix.Items.Placeables;
using CalRemix.Projectiles;
using CalRemix.UI;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using System.Linq;
using CalamityMod.Items.Materials;

namespace CalRemix.NPCs
{
	public class LifeSlime : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Life Slime");
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.BlueSlime];
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<KamiFlu>()] = true;

            if (Main.dedServ)
                return;
            FannyManager.LoadFannyMessage(new FannyMessage("LifeBiome",
                "The Life Heart is a joint meeting area of the elements of fire, ice, and nature. With their powers combined, they make a powerful new bar which can be used to upgrade your items! Be careful of the dreaded Life Slimes that visciously guard the place though.",
                "Nuhuh",
                (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == Type)).AddItemDisplay(ModContent.ItemType<LifeAlloy>()));
        }
		public override void SetDefaults()
		{ 
			NPC.aiStyle = NPCAIStyleID.Slime;
			NPC.width = 48;
			NPC.height = 30;
			NPC.noTileCollide = false;
			NPC.noGravity = false;
			NPC.damage = 260;
			NPC.defense = 12;
			NPC.lifeMax = 2400;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.rarity = 2;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = Item.buyPrice(gold: 1, silver: 5);
			AnimationType = NPCID.BlueSlime;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<LifeBiome>().Type };
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new System.Collections.Generic.List<IBestiaryInfoElement>
			{
				new FlavorTextBestiaryInfoElement("When a slime and a life ore love each other very much, they produce this.")
			});
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (DownedBossSystem.downedRavager && spawnInfo.Player.InModBiome(ModContent.GetInstance<LifeBiome>()))
			{
				return SpawnCondition.Cavern.Chance * 8f;
            }
			else
			{
				return 0;
			}
		}
		public override void AI()
		{
			bool flag = false;
            var source = NPC.GetSource_FromAI();
            int projdam = 50;
            Player target = Main.player[NPC.target];

            if (NPC.life != NPC.lifeMax || target.InModBiome<LifeBiome>())
            {
                flag = true;
            }
            if (flag)
            {
                if (NPC.ai[0] == -2100)
                {
                    SoundEngine.PlaySound(SoundID.Item33, NPC.Center);
                    Projectile.NewProjectile(source, new Vector2(NPC.Center.X, NPC.Center.Y - NPC.height), Vector2.Normalize(target.Center - NPC.Center) * 6f, ModContent.ProjectileType<LifeMeteor>(), projdam, 0);
                }
                else if (NPC.ai[0] == -1020 || NPC.ai[0] == -1080)
                { 
                    SoundEngine.PlaySound(SoundID.Item33, NPC.Center);
                    Projectile.NewProjectile(source, new Vector2(target.Center.X + Main.rand.Next(-128, -63), target.Center.Y + 150), new Vector2(0, -40f), ModContent.ProjectileType<LifeThorn>(), projdam, 0, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(source, new Vector2(target.Center.X + Main.rand.Next(64, 129), target.Center.Y + 150), new Vector2(0, -40f), ModContent.ProjectileType<LifeThorn>(), projdam, 0, Main.myPlayer, 0f, 0f);
                }
                else if (NPC.ai[0] == -60 || NPC.ai[0] == -150)
                {
                    SoundEngine.PlaySound(SoundID.Item33, NPC.Center);
                    Projectile.NewProjectile(source, new Vector2(NPC.Center.X, NPC.Center.Y - NPC.height), Vector2.Normalize(target.Center - NPC.Center) * 12f, ModContent.ProjectileType<LifeIcicle>(), projdam, 0);
                    Projectile.NewProjectile(source, new Vector2(NPC.Center.X, NPC.Center.Y - NPC.height), Vector2.Normalize(target.Center - NPC.Center).RotatedBy(MathHelper.ToRadians(25f)) * 12f, ModContent.ProjectileType<LifeIcicle>(), projdam, 0);
                    Projectile.NewProjectile(source, new Vector2(NPC.Center.X, NPC.Center.Y - NPC.height), Vector2.Normalize(target.Center - NPC.Center).RotatedBy(MathHelper.ToRadians(25f)) * 12f, ModContent.ProjectileType<LifeIcicle>(), projdam, 0);
                    for (int i = 0; i < 3; i++)
                    {
                        Projectile.NewProjectile(source, new Vector2(NPC.Center.X, NPC.Center.Y - NPC.height), Vector2.Normalize(target.Center - NPC.Center).RotatedByRandom(MathHelper.ToRadians(45f)) * 12f, ModContent.ProjectileType<LifeIcicle>(), projdam, 0);
                    }
                }
            }
            NPC.ai[0] += 1f;
            if (NPC.ai[2] > 1f)
            {
                NPC.ai[2] -= 1f;
            }
            if (NPC.wet)
            {
                if (NPC.collideY)
                {
                    NPC.velocity.Y = -2f;
                }
                if (NPC.velocity.Y < 0f && NPC.ai[3] == NPC.position.X)
                {
                    NPC.direction *= -1;
                    NPC.ai[2] = 200f;
                }
                if (NPC.velocity.Y > 0f)
                {
                    NPC.ai[3] = NPC.position.X;
                }
                if (NPC.velocity.Y > 2f)
                {
                    NPC.velocity.Y *= 0.9f;
                }
                NPC.velocity.Y -= 0.5f;
                if (NPC.velocity.Y < -4f)
                {
                    NPC.velocity.Y = -4f;
                }
                if (NPC.ai[2] == 1f && flag)
                {
                    NPC.TargetClosest();
                }
            }
            if (NPC.ai[2] == 0f)
            {
                NPC.ai[0] = -100f;
                NPC.ai[2] = 1f;
                NPC.TargetClosest();
            }
            if (NPC.velocity.Y == 0f)
            {
                if (NPC.collideY && NPC.oldVelocity.Y != 0f && Collision.SolidCollision(NPC.position, NPC.width, NPC.height))
                {
                    NPC.position.X -= NPC.velocity.X + (float)NPC.direction;
                }
                if (NPC.ai[3] == NPC.position.X)
                {
                    NPC.direction *= -1;
                    NPC.ai[2] = 200f;
                }
                NPC.ai[3] = 0f;
                NPC.velocity.X *= 0.8f;
                if ((double)NPC.velocity.X > -0.1 && (double)NPC.velocity.X < 0.1)
                {
                    NPC.velocity.X = 0f;
                }
                float num32 = -1000f;
                int num33 = 0;
                if (NPC.ai[0] >= 0f)
                {
                    num33 = 1;
                }
                if (NPC.ai[0] >= num32 && NPC.ai[0] <= num32 * 0.5f)
                {
                    num33 = 2;
                }
                if (NPC.ai[0] >= num32 * 2f && NPC.ai[0] <= num32 * 1.5f)
                {
                    num33 = 3;
                }
                if (num33 > 0)
                {
                    NPC.netUpdate = true;
                    if (flag && NPC.ai[2] == 1f)
                    {
                        NPC.TargetClosest();
                    }
                    if (num33 == 3)
                    {
                        NPC.velocity.Y = -8f;
                        NPC.velocity.X += 3 * NPC.direction;
                        NPC.ai[0] = -200f;
                        NPC.ai[3] = NPC.position.X;
                    }
                    else
                    {
                        NPC.velocity.Y = -6f;
                        NPC.velocity.X += 2 * NPC.direction;
                        if (NPC.type == 59)
                        {
                            NPC.velocity.X += 2 * NPC.direction;
                        }
                        NPC.ai[0] = -120f;
                        if (num33 == 1)
                        {
                            NPC.ai[0] += num32;
                        }
                        else
                        {
                            NPC.ai[0] += num32 * 2f;
                        }
                    }
                }
            }
            else if (NPC.target < 255 && ((NPC.direction == 1 && NPC.velocity.X < 3f) || (NPC.direction == -1 && NPC.velocity.X > -3f)))
            {
                if (NPC.collideX && Math.Abs(NPC.velocity.X) == 0.2f)
                {
                    NPC.position.X -= 1.4f * (float)NPC.direction;
                }
                if (NPC.collideY && NPC.oldVelocity.Y != 0f && Collision.SolidCollision(NPC.position, NPC.width, NPC.height))
                {
                    NPC.position.X -= NPC.velocity.X + (float)NPC.direction;
                }
                if ((NPC.direction == -1 && (double)NPC.velocity.X < 0.01) || (NPC.direction == 1 && (double)NPC.velocity.X > -0.01))
                {
                    NPC.velocity.X += 0.2f * (float)NPC.direction;
                }
                else
                {
                    NPC.velocity.X *= 0.93f;
                }
            }

        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<LifeOre>(), 1, 10, 26);
        }
    }
}