using CalamityMod.BiomeManagers;
using CalRemix.Core.World;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using CalRemix.Content.Items.Placeables.Banners;
using Terraria.GameContent.Bestiary;
namespace CalRemix.Content.NPCs
{
    public class LabRoach : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Lab Roach");
            Main.npcFrameCount[NPC.type] = 4;
            NPCID.Sets.CantTakeLunchMoney[Type] = true;
            NPCID.Sets.CountsAsCritter[NPC.type] = true;
            Main.npcCatchable[NPC.type] = true;
        }
        public override void SetDefaults()
        {
            NPC.width = 22;
            NPC.height = 22;
            NPC.aiStyle = 7;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.lifeMax = 5;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.lavaImmune = false;
            Banner = Type;
            BannerItem = ItemType<LabRoachBanner>();
            NPC.catchItem = (short)ItemType<Items.Critters.LabRoach>();
            AIType = NPCID.Mouse;
            AnimationType = NPCID.Grubby;
            NPC.npcSlots = 0.05f;
            NPC.dontTakeDamage = true;
            NPC.chaseable = false;
            SpawnModBiomes = new int[] { GetInstance<ArsenalLabBiome>().Type };
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[Type], quickUnlock: true);
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }
        public override void AI()
        {
            NPC.ai[2]--;
            if (Main.rand.NextBool(300) && NPC.ai[2] <= 0)
            {
                NPC.velocity.Y = -Main.rand.NextFloat(3, 8);
                if (Main.rand.NextBool(22))
                    NPC.velocity.Y = -Main.rand.NextFloat(8, 16);
                NPC.ai[2] = 120;
            }
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (CalRemixWorld.roachDuration > 0)
            {
                return 22f;
            }
            return 0f;
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("LabRoachGore").Type, NPC.scale);
                }
            }
        }
    }
}