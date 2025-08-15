using CalamityMod;
using CalRemix.Content.Items.Accessories;
using CalRemix.UI;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System.Linq;
using Microsoft.Xna.Framework;
using CalamityMod.BiomeManagers;
//using CalamityMod.CalPlayer;

namespace CalRemix.Content.NPCs.TheGoodStuff
{
    public class PenguinCommander : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 3;
        }

        public override void SetDefaults()
        {
            NPC.width = 312;
            NPC.height = 196;
            NPC.CloneDefaults(NPCID.GoblinScout);
            NPC.damage = 40;
            NPC.lavaImmune = false;
            AIType = NPCID.GoblinScout;
            NPC.aiStyle = NPCAIStyleID.Fighter;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.lifeMax = 300;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToSickness = true;
            NPC.Calamity().VulnerableToWater = false;
            NPC.Calamity().VulnerableToCold = false;
        }

        public override void SetBestiary(Terraria.GameContent.Bestiary.BestiaryDatabase database, Terraria.GameContent.Bestiary.BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new Terraria.GameContent.Bestiary.IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow,
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.spriteDirection = NPC.velocity.X.DirectionalSign();
            if (NPC.velocity.Y != 0)
            {
                NPC.frame.Y = frameHeight * 2;
                return;
            }
            NPC.frameCounter++;
            if (NPC.frameCounter % 8 == 0)
            {
                NPC.frame.Y += frameHeight;
            }
            if (NPC.frame.Y > frameHeight * 2)
            {
                NPC.frame.Y = 0;
            }
        }


        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.PlayerSafe || spawnInfo.Player.InModBiome<AstralInfectionBiome>() || spawnInfo.Player.Calamity().ZoneAbyss ||
                spawnInfo.Player.Calamity().ZoneSunkenSea || !spawnInfo.Player.ZoneSnow || !NPC.downedBoss3)
                return 0;
            return Terraria.ModLoader.Utilities.SpawnCondition.OverworldDaySnowCritter.Chance * 0.2f;
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Bone, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Bone, hit.HitDirection, -1f, 0, default, 1f);
                }
                if (Main.netMode != NetmodeID.Server)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("PenguinCommander1").Type, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("PenguinCommander2").Type, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("PenguinCommander3").Type, NPC.scale);
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemID.TragicUmbrella, 10);
        }
    }
}