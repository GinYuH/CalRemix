using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalRemix.Core.Biomes;
using CalamityMod.NPCs;
using CalRemix.Content.Tiles;
using CalamityMod.Tiles;

namespace CalRemix.Content.NPCs
{
    public class EnchantedSkull : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Enchanted Skull");
            Main.npcFrameCount[NPC.type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = NPCAIStyleID.CursedSkull;
            //AIType = NPCID.CursedSkull;
            NPC.damage = 30;
            NPC.width = 28;
            NPC.height = 28;
            NPC.defense = 3;
            NPC.lifeMax = 200;
            NPC.knockBackResist = 0f;
            NPC.value = 10;
            NPC.lavaImmune = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit2;
            NPC.DeathSound = SoundID.NPCDeath2;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToCold = false;
            NPC.Calamity().VulnerableToSickness = false;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<FrozenStrongholdBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement("Many have tried to breach the stronghold's walls and many have failed to leave a dent on them. However, in the afterlife, they got their wish and now patrol the fort's inner sanctums.")
            });
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter > 6)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;
            }
            if (NPC.frame.Y > frameHeight * 3)
                NPC.frame.Y = 0;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!spawnInfo.Player.InModBiome<FrozenStrongholdBiome>() || spawnInfo.Player.Calamity().ZoneAbyss ||
                spawnInfo.Player.Calamity().ZoneSunkenSea || CalamityGlobalNPC.AnyEvents(spawnInfo.Player))
            {
                return 0f;
            }
            if (spawnInfo.SpawnTileType != ModContent.TileType<FrostflakeBrickPlaced>() && spawnInfo.SpawnTileType != ModContent.TileType<CryonicBrick>() && spawnInfo.SpawnTileType != TileID.Platforms)
                return 0f;
            return 0.7f;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.IceTorch, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.IceTorch, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }
    }
}
