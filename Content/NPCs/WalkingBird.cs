using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using CalamityMod;
using CalRemix.Content.Items.Placeables;
using CalRemix.Core.Biomes;
using CalamityMod.BiomeManagers;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Items.Placeables.Banners;
using Microsoft.Xna.Framework;
using System;

namespace CalRemix.Content.NPCs
{
    public class WalkingBird : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 10;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = NPCAIStyleID.Unicorn;
            NPC.damage = 30;
            NPC.width = 32;
            NPC.height = 50;
            NPC.defense = 2;
            NPC.lifeMax = 200;
            NPC.value = Item.buyPrice(silver: 5);
            NPC.lavaImmune = false;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit51 with { Pitch = 0.4f };
            NPC.DeathSound = SoundID.NPCDeath46 with { Pitch = 0.4f };
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToWater = true;
            NPC.Calamity().VulnerableToCold = true;
            NPC.Calamity().VulnerableToSickness = true;
            NPC.Calamity().VulnerableToElectricity = true;
            Banner = ModContent.NPCType<WalkingBird>();
            BannerItem = ModContent.ItemType<WalkingBirdBanner>();
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }
        public override void AI()
        {
            //CalamityMod.NPCs.VanillaNPCAIOverrides.RegularEnemies.RevengeanceAndDeathAI.BuffedUnicornAI(NPC, Mod);
            //NPC.spriteDirection = -NPC.velocity.X.DirectionalSign();
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.spriteDirection = NPC.velocity.X.DirectionalSign();
            if (NPC.velocity.Y == 0)
            {
                if (NPC.velocity.X != 0)
                {
                    NPC.frameCounter++;
                    if (NPC.frameCounter > MathHelper.Lerp(12, 1, Utils.GetLerpValue(0, 5, Math.Abs(NPC.velocity.X), true)))
                    {
                        NPC.frame.Y += frameHeight;
                        NPC.frameCounter = 0;
                    }
                    if (NPC.frame.Y > frameHeight * 9)
                        NPC.frame.Y = 0;
                }
                else
                {
                    NPC.frame.X = 0;
                }
            }
            else
            {
                NPC.frame.Y = frameHeight;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.PlayerSafe || spawnInfo.Player.InModBiome<AstralInfectionBiome>() || spawnInfo.Player.Calamity().ZoneAbyss ||
                spawnInfo.Player.Calamity().ZoneSunkenSea || !NPC.downedBoss1)
            {
                return 0f;
            }
            return SpawnCondition.OverworldDay.Chance * 0.05f;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
                }
                int startID = NPC.type == ModContent.NPCType<WalkingBirdRed>() ? 5 : NPC.type == ModContent.NPCType<WalkingBirdBlue>() ? 3 : 1;
                if (Main.netMode != NetmodeID.Server)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("WalkingBird" + startID).Type, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("WalkingBird" + (startID + 1)).Type, NPC.scale);
                }
            }
        }
    }

    public class WalkingBirdBlue : WalkingBird
    {
    }
    public class WalkingBirdRed : WalkingBird
    {
    }
}
