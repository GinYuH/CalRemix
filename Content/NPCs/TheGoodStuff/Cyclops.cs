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
using System;
//using CalamityMod.CalPlayer;

namespace CalRemix.Content.NPCs.TheGoodStuff
{
    public class Cyclops : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 14;
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
            NPC.lifeMax = 800;
            NPC.chaseable = true;
            NPC.Calamity().VulnerableToHeat = false;
            NPC.Calamity().VulnerableToSickness = true;
            NPC.Calamity().VulnerableToCold = true;
        }

        public override void SetBestiary(Terraria.GameContent.Bestiary.BestiaryDatabase database, Terraria.GameContent.Bestiary.BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new Terraria.GameContent.Bestiary.IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.spriteDirection = NPC.velocity.X.DirectionalSign();
            if (NPC.velocity.Y != 0)
            {
                NPC.frame.Y = frameHeight * 13;
                return;
            }
            NPC.frameCounter++;
            if (NPC.frameCounter % 8 == 0)
            {
                NPC.frame.Y += frameHeight;
            }
            if (NPC.frame.Y > frameHeight * 13)
            {
                NPC.frame.Y = 0;
            }
        }


        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            float cord = (spawnInfo.Player.position.X) / (float)Main.maxTilesX;
            if (spawnInfo.PlayerSafe || spawnInfo.Player.InModBiome<AstralInfectionBiome>() || spawnInfo.Player.Calamity().ZoneAbyss ||
                spawnInfo.Player.Calamity().ZoneSunkenSea || !NPC.downedBoss3)
                return 0;
            if (cord < 0.16f || cord > 0.84f)
                return Terraria.ModLoader.Utilities.SpawnCondition.OverworldDaySnowCritter.Chance * 0.2f;
            return 0;
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
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Cyclops1").Type, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Cyclops2").Type, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Cyclops3").Type, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Cyclops4").Type, NPC.scale);
                }
            }
        }
    }
}