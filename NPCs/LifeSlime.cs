using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using CalRemix.Items.Placeables;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace CalRemix.NPCs
{
	public class LifeSlime : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Life Slime");
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.BlueSlime];
            NPCID.Sets.DebuffImmunitySets.Add(Type, new NPCDebuffImmunityData
            {
                SpecificallyImmuneTo = new int[] 
				{
					ModContent.BuffType<KamiFlu>(),
                    BuffID.Frostburn,
                    BuffID.Poisoned
				}
            });
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
			NPC.DeathSound = SoundID.NPCDeath1;
			AnimationType = NPCID.BlueSlime;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<Biomes.LifeBiome>().Type };
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
			if (CalamityMod.DownedBossSystem.downedRavager && spawnInfo.Player.InModBiome(ModContent.GetInstance<Biomes.LifeBiome>()))
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

		}
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<LifeOre>(), 1, 10, 26);
        }
    }
}