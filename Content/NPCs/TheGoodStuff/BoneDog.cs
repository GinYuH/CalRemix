using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using CalamityMod;
using CalamityMod.BiomeManagers;
using CalRemix.Content.Items.Placeables.Banners;
using Microsoft.Xna.Framework;
using System;
using CalamityMod.NPCs.NormalNPCs;

namespace CalRemix.Content.NPCs.TheGoodStuff
{
    public class BoneDog : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 5;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = NPCAIStyleID.Unicorn;
            NPC.damage = 50;
            NPC.width = 32;
            NPC.height = 50;
            NPC.defense = 8;
            NPC.lifeMax = 300;
            NPC.value = Item.buyPrice(silver: 1);
            NPC.HitSound = Rimehound.HitSound with { Pitch = 0.4f };
            NPC.DeathSound = Rimehound.GrowlSound with { Pitch = -0.4f };
            NPC.Calamity().VulnerableToHeat = false;
            NPC.Calamity().VulnerableToWater = true;
            NPC.Calamity().VulnerableToCold = true;
            NPC.Calamity().VulnerableToSickness = false;
            Banner = ModContent.NPCType<WalkingBird>();
            BannerItem = ModContent.ItemType<WalkingBirdBanner>();
            Banner = Type;
            BannerItem = ModContent.ItemType<BoneDogBanner>();
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon,
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
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
                    if (NPC.frame.Y > frameHeight * 4)
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
                spawnInfo.Player.Calamity().ZoneSunkenSea || !NPC.downedQueenBee)
            {
                return 0f;
            }
            if (spawnInfo.Player.Distance(new Vector2(Main.dungeonX, Main.dungeonY) * 16) < 700)
            {
                return SpawnCondition.OverworldNightMonster.Chance;
            }
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
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("BoneDog1").Type, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("BoneDog2").Type, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("BoneDog3").Type, NPC.scale);
                }
            }
        }
    }
}
