using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;
using CalRemix.UI;
using System.Linq;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Items.Potions;
using CalamityMod.Items.Weapons.Summon;
using CalRemix.Core.World;
using CalamityMod.Items.Potions;

namespace CalRemix.Content.NPCs
{
    public class FrostHog : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frost Hog");
            Main.npcFrameCount[NPC.type] = 1;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 150;
            NPC.width = 42;
            NPC.height = 38;
            NPC.defense = 8;
            NPC.lifeMax = 30;
            NPC.knockBackResist = 0.9f;
            NPC.value = Item.buyPrice(silver: 2);
            NPC.noGravity = false;
            NPC.HitSound = CalamityMod.NPCs.NormalNPCs.Rimehound.HitSound with { Pitch = 1 };
            NPC.DeathSound = SoundID.NPCDeath27 with { Pitch = 1 };
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToSickness = true;
            NPC.Calamity().VulnerableToCold = false;
        }

        public override void AI()
        {
           CalamityMod.NPCs.VanillaNPCAIOverrides.RegularEnemies.RevengeanceAndDeathAI.BuffedHerplingAI(NPC, Mod);
            NPC.spriteDirection = -NPC.velocity.X.DirectionalSign();
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow,
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (SpawnCondition.OverworldDaySnowCritter.Active)

            return SpawnCondition.OverworldDaySnowCritter.Chance * 0.05f;
            return 0;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<DeliciousMeat>());
        }
    }
}
