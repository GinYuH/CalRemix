using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Terraria.ModLoader.Utilities;
using System.IO;
using CalamityMod.NPCs.CalamityAIs.CalamityRegularEnemyAIs;

namespace CalRemix.Content.NPCs
{
    public class Lizard : ModNPC
    {
        public Player Target => Main.player[NPC.target];
        public ref float Mag => ref NPC.ai[0];
        private bool detected;
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 5;
        }
        public override void SetDefaults()
        {
            NPC.width = 42;
            NPC.height = 16;
            NPC.lifeMax = 20;
            NPC.damage = 10;
            NPC.defense = 0;
            NPC.knockBackResist = 0.2f;
            NPC.value = Item.buyPrice(silver: 1);
            NPC.HitSound = SoundID.NPCHit33;
            NPC.DeathSound = SoundID.NPCDeath36;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToCold = true;
            NPC.Calamity().VulnerableToSickness = true;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(detected);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            detected = reader.ReadBoolean();
        }
        public override void FindFrame(int frameHeight)
        {
            if (!detected)
            {
                NPC.frame.Y = frameHeight * 4;
                NPC.frameCounter = 0.0;
                return;
            }
            NPC.spriteDirection = -NPC.direction;
            NPC.frameCounter += NPC.velocity.Length() / 8f;
            if (NPC.frameCounter > 2.0)
            {
                NPC.frame.Y = NPC.frame.Y + frameHeight;
                NPC.frameCounter = 0.0;
            }
            if (NPC.frame.Y >= frameHeight * 3)
                NPC.frame.Y = 0;
        }
        public override void AI()
        {
            NPC.TargetClosest();
            if (!detected && (NPC.Distance(Target.Center) < 100 || NPC.life < NPC.lifeMax))
                detected = true;
            if (detected)
                CalamityRegularEnemyAI.GemCrawlerAI(NPC, Mod, 6f, 0.18f);
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemID.StoneBlock, 1, 3, 4);
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneNormalUnderground)
                return SpawnCondition.Underground.Chance;
            else if (spawnInfo.Player.ZoneNormalCaverns)
                return SpawnCondition.Cavern.Chance * 0.2f;
            return 0f;
        }
    }
}
