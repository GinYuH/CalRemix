using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalRemix.Core.Biomes;
using CalamityMod.NPCs;
using Terraria.Audio;
using CalRemix.Content.Tiles;
using CalamityMod.Tiles;

namespace CalRemix.Content.NPCs
{
    public class FrozenMummy : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Frozen Mummy");
            Main.npcFrameCount[NPC.type] = 13;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = NPCAIStyleID.Slime;
            AIType = NPCID.ToxicSludge;
            NPC.damage = 60;
            NPC.width = 20;
            NPC.height = 46;
            NPC.defense = 22;
            NPC.lifeMax = 400;
            NPC.value = 1000;
            NPC.lavaImmune = false;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath3;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToCold = false;
            NPC.Calamity().VulnerableToSickness = false;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<FrozenStrongholdBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
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
            if (NPC.frame.Y > frameHeight * 12)
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
            return 0.5f;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.BlueTorch, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.BlueTorch, hit.HitDirection, -1f, 0, default, 1f);
                }
                if (Main.rand.NextBool(3))
                {
                    SoundEngine.PlaySound(BetterSoundID.ItemSentrySummonStrong, NPC.Center);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                        NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<EnchantedSkull>());
                }
            }
        }
    }
}
