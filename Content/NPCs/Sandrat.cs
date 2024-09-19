using CalamityMod;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using static Terraria.ModLoader.ModContent;
//using CalamityMod.CalPlayer;

namespace CalRemix.Content.NPCs
{
    public class Sandrat : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Minor Sandrat");
            Main.npcFrameCount[NPC.type] = 12;
            Main.npcCatchable[NPC.type] = true;
            NPCID.Sets.CantTakeLunchMoney[Type] = true;
            NPCID.Sets.CountsAsCritter[NPC.type] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 52;
            NPC.height = 38;
            NPC.CloneDefaults(NPCID.Mouse);
            NPC.catchItem = (short)ItemType<Items.Critters.Sandrat>();
            NPC.lavaImmune = false;
            AIType = NPCID.Mouse;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.lifeMax = 20;
            NPC.defense = 1;
            NPC.chaseable = false;
            NPC.Calamity().VulnerableToHeat = false;
            NPC.Calamity().VulnerableToSickness = true;
            NPC.Calamity().VulnerableToCold = true;
        }
        public override bool? CanBeHitByItem(Player player, Item item) => null;

        public override bool? CanBeHitByProjectile(Projectile projectile) => null;

        public override void AI()
        {
            Main.npcFrameCount[NPC.type] = 12;
            NPC.spriteDirection = NPC.direction;

        }
        public override void FindFrame(int frameHeight)
        {
            if (NPC.velocity.Y < 2)
            {
                if (NPC.velocity.X != 0)
                {
                    NPC.frameCounter += 1;
                    if (NPC.frameCounter > 6)
                    {
                        NPC.frame.Y += frameHeight;
                        NPC.frameCounter = 0;
                    }
                    if (NPC.frame.Y >= frameHeight * 11 || NPC.frame.Y < frameHeight * 6)
                    {
                        NPC.frame.Y = frameHeight * 6;
                    }
                }
                else if (NPC.velocity.X == 0)
                {
                    NPC.frameCounter += 1;
                    if (NPC.frameCounter > 6)
                    {
                        NPC.frame.Y += frameHeight;
                        NPC.frameCounter = 0;
                    }
                    if (NPC.frame.Y >= frameHeight * 5)
                    {
                        NPC.frame.Y = 0;
                    }
                }
            }
            else
            {
                NPC.frame.Y = 0;
            }
        }

        public override void SetBestiary(Terraria.GameContent.Bestiary.BestiaryDatabase database, Terraria.GameContent.Bestiary.BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.UIInfoProvider = new Terraria.GameContent.Bestiary.CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[Type], quickUnlock: true);
            bestiaryEntry.Info.AddRange(new Terraria.GameContent.Bestiary.IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert, BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert,
                new Terraria.GameContent.Bestiary.FlavorTextBestiaryInfoElement("Why would you hurt her you sicko"),
            });
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneUndergroundDesert)
                return SpawnCondition.DesertCave.Chance * 0.5f;
            if (spawnInfo.Player.ZoneDesert)
                return SpawnCondition.OverworldDayDesert.Chance * 2f;
            return 0;
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Sandrat1").Type, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Sandrat2").Type, NPC.scale);
                }
            }
        }
    }
}