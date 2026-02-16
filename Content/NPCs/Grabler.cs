using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Terraria.GameContent.ItemDropRules;
using CalRemix.Content.Items.Materials;
using Terraria.ModLoader.Utilities;
using CalRemix.Content.Items.Weapons;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;
using Terraria.Localization;

namespace CalRemix.Content.NPCs
{
    public class Grabler : ModNPC
    {
        public Player Target => Main.player[NPC.target];
        public override void SetDefaults()
        {
            Main.npcFrameCount[Type] = 5;
            NPC.noGravity = true;
            NPC.width = 52;
            NPC.height = 64;
            NPC.damage = 0;
            NPC.defense = 6;
            NPC.lavaImmune = false;
            NPC.HitSound = SoundID.NPCHit20;
            NPC.DeathSound = SoundID.NPCHit8;
            NPC.lifeMax = 130;
            NPC.rarity = 2;
            NPC.aiStyle = NPCAIStyleID.HoveringFighter;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToSickness = false;
            NPC.Calamity().VulnerableToWater = false;
            NPC.knockBackResist = 0.6f;
        }

        public int dashCooldown = 90;
        public bool dashValid = false;
        public int timeSinceDashPrep = 0;
        public override void FindFrame(int frameheight)
        {
            float frameSpeed = 12f;
            if (NPC.velocity.X > 0) NPC.frameCounter += 1.5f * NPC.velocity.X;
            else NPC.frameCounter += 1.5f * -NPC.velocity.X;
            if (NPC.frameCounter >= frameSpeed)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameheight;
                if (NPC.frame.Y >= frameheight * Main.npcFrameCount[Type]) NPC.frame.Y = 0; // reset to 0 once frames are done
            }
        }
        public override void AI()
        {
            NPC.TargetClosest();
            NPC.Calamity().newAI[0]++;
            if (Main.player[NPC.target].Distance(NPC.Center) < 600 && NPC.Calamity().newAI[0] % dashCooldown == 0 && Main.rand.Next(1, 3) == 1)
            {
                //dash preparation
                SoundEngine.PlaySound(SoundID.NPCHit44, NPC.Center);
                dashValid = true;
                NPC.velocity.Y -= 6;
                for (int d = 0; d < 50; d++)
                {
                    Dust dust = Dust.NewDustDirect(NPC.Center, NPC.width * 2, NPC.height * 2, DustID.Grass);
                    dust.position = NPC.Center;
                    dust.scale = 2f;
                    dust.velocity = (NPC.velocity * Main.rand.Next(3, 5) * 3f).RotatedBy(MathHelper.ToRadians(Main.rand.Next(0, 360)));
                    dust.noGravity = true;
                }
            }
            if (dashValid) timeSinceDashPrep ++;
            if (NPC.Calamity().newAI[0] == 30) NPC.damage = 0;

            if (dashValid && timeSinceDashPrep >= 30)
            {
                //dash
                NPC.velocity = NPC.DirectionFrom(Main.player[NPC.target].Center) * -16;
                NPC.Calamity().newAI[0] = 0;
                NPC.damage = 30;
                timeSinceDashPrep = 0;
                dashValid = false;
                SoundEngine.PlaySound( SoundID.Roar with { Pitch = 1f - NPC.life / (float)NPC.lifeMax }, (NPC.Center));
            }

        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!spawnInfo.PlayerSafe && spawnInfo.Player.ZoneForest && spawnInfo.Player.ZoneOverworldHeight)
            {
                return SpawnCondition.OverworldNight.Chance * 0.06f;
            }
            return 0f;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Poisoned, 60);
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    for (int d = 0; d < 40; d++)
                    {
                        Dust dust = Dust.NewDustDirect(NPC.Center, NPC.width * 2, NPC.height * 2, DustID.Grass);
                        dust.position = NPC.Center;
                        dust.scale = 2f;
                        dust.velocity = (NPC.velocity * Main.rand.Next(3, 9) * 3f).RotatedBy(MathHelper.ToRadians(Main.rand.Next(0, 360)));
                        dust.noGravity = false;
                    }
                    for (int i = 0; i < 4; i++)
                        Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity * 0.5f, Mod.Find<ModGore>("Grabler" + (i + 1)).Type, NPC.scale);
                }
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.Grabler").Value)
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemID.Lens, 1);
            npcLoot.Add(ItemID.DirtBlock, Main.rand.Next(2,18));
            npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<Grablerscalibur>(), 4, 3));
        }
    }
}
