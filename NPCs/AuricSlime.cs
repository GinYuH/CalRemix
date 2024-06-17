using CalamityMod.Items.Placeables.Ores;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using CalamityMod;
using CalRemix.Items;
using System.Linq;
using CalRemix.UI;

namespace CalRemix.NPCs
{
    public class AuricSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Auric Slime");
            Main.npcFrameCount[NPC.type] = 2;

            if (Main.dedServ)
                return;
            ScreenHelperManager.LoadMessage(new HelperMessage("AuricSlime",
                "Look! A rare Auric Slime! It's fascinating how fascinated that fascinating humans are with these fascinating creatures. Such is the inner-machinations of the natural animalistic instinct to complete a collection.",
                "FannyAwooga",
                (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == Type)));
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = NPCAIStyleID.Slime;
            AIType = NPCID.ToxicSludge;
            NPC.damage = 160;
            NPC.width = 68;
            NPC.height = 46;
            NPC.defense = 60;
            NPC.lifeMax = 14000;
            NPC.knockBackResist = 0f;
            AnimationType = NPCID.CorruptSlime;
            NPC.value = 0;
            NPC.alpha = 50;
            NPC.lavaImmune = false;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToSickness = false;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
		new FlavorTextBestiaryInfoElement("A slime that has successfully fused with godly matter. It is a wonder that these have taken so long to evolve considering how easily they can absorb other ores.")
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.PlayerSafe || !DownedBossSystem.downedYharon || spawnInfo.Player.Calamity().ZoneAbyss ||
                spawnInfo.Player.Calamity().ZoneSunkenSea)
            {
                return 0f;
            }
            return SpawnCondition.Cavern.Chance * 0.04f;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, 234, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, 234, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<AuricOre>(), 1, 10, 26);
            npcLoot.Add(ModContent.ItemType<CosmiliteCoin>(), 1, 1, 1);
        }
    }
}
