using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using CalamityMod;
using CalRemix.Content.Items.Potions;

namespace CalRemix.Content.NPCs
{
    public class BoarCat : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Boar Cat");
            Main.npcFrameCount[NPC.type] = 1;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = NPCAIStyleID.Herpling;
            NPC.damage = 150;
            NPC.width = 32;
            NPC.height = 36;
            NPC.defense = 8;
            NPC.lifeMax = 30;
            NPC.knockBackResist = 0.9f;
            NPC.value = Item.buyPrice(silver: 2);
            NPC.noGravity = false;
            NPC.HitSound = CalamityMod.NPCs.NormalNPCs.Rimehound.HitSound with { Pitch = 1 };
            NPC.DeathSound = SoundID.NPCDeath27 with { Pitch = 1 };
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToSickness = false;
            NPC.Calamity().VulnerableToCold = true;
        }

        public override void AI()
        {
            NPC.spriteDirection = -NPC.velocity.X.DirectionalSign();
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (SpawnCondition.SurfaceJungle.Active)

                return SpawnCondition.SurfaceJungle.Chance * 0.05f;
            return 0;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<DisgustingMeat>());
        }
    }
}
