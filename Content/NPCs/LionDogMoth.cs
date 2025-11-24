using CalamityMod;
using Terraria;
using Terraria.Enums;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using static Terraria.ModLoader.ModContent;

namespace CalRemix.Content.NPCs
{
    public class LionDogMoth : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 2;
            Main.npcCatchable[NPC.type] = true;
            NPCID.Sets.CantTakeLunchMoney[Type] = true;
            NPCID.Sets.CountsAsCritter[NPC.type] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 30;
            NPC.height = 24;
            NPC.CloneDefaults(NPCID.Butterfly);
            NPC.catchItem = (short)ItemType<Items.Critters.LionDogMoth>();
            NPC.lavaImmune = false;
            AIType = NPCID.Butterfly;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.lifeMax = 5;
            NPC.defense = 0;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToSickness = true;
            NPC.Calamity().VulnerableToCold = false;
        }

        public override void AI()
        {
            NPC.spriteDirection = NPC.direction;
            foreach (NPC n in Main.ActiveNPCs)
            {
                if (n.aiStyle == NPCAIStyleID.Butterfly && n.type != NPC.type)
                {
                    NPC.velocity = NPC.DirectionTo(n.Center) * 2;
                    if (n.getRect().Intersects(NPC.getRect()))
                    {
                        n.StrikeInstantKill();
                    }
                    break;
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.075f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[Type], quickUnlock: true);
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow,
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!spawnInfo.Player.ZoneSnow || Main.moonPhase != (int)MoonPhase.Full)
                return 0f;

            return SpawnCondition.OverworldDaySnowCritter.Chance * 4f;
        }
    }
}