using CalamityMod;
using CalRemix.Content.Items.Placeables.Subworlds.Pinnacles;
using CalRemix.Content.Items.Potions;
using CalRemix.Core.Biomes;
using CalRemix.Core.Biomes.Subworlds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace CalRemix.Content.NPCs.Subworlds.Pinnacles
{
    public class Vinisunik : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.CantTakeLunchMoney[Type] = true;
            Main.npcFrameCount[Type] = 2;
        }

        public override void SetDefaults()
        {
            NPC.width = 100;
            NPC.height = 124;
            NPC.HitSound = BetterSoundID.ItemCrystalSerpentImpact with { Pitch = 0.3f };
            NPC.DeathSound = BetterSoundID.ItemSolarEruption with { Pitch = -1 };
            NPC.lifeMax = 200;
            NPC.defense = 0;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToSickness = true;
            NPC.Calamity().VulnerableToCold = false;
            NPC.Calamity().VulnerableToWater = true;
            SpawnModBiomes = [ModContent.GetInstance<PinnaclesBiome>().Type];
            NPC.scale = 0.5f;
        }

        public override void AI()
        {
            if (NPC.direction == 0)
            {
                NPC.direction = Main.rand.NextBool().ToDirectionInt();
            }
            if (NPC.ai[0] == 0)
            {
                NPC.velocity = new Vector2(NPC.direction * 4, Main.rand.NextFloat(-8, -4));
                NPC.ai[0]++;
                SoundEngine.PlaySound(BetterSoundID.ItemSnowballHit with { Pitch = -1, MaxInstances = 0 }, NPC.Center);
                NPC.noTileCollide = true;
                NPC.ai[1]++;
            }

            if (NPC.ai[1] > 0)
            {
                NPC.ai[1]++;
                if (NPC.ai[1] > 5)
                {
                    NPC.noTileCollide = false;
                    NPC.ai[1] = 0;
                }
            }

            if (NPC.ai[0] > 0)
            {
                if (NPC.velocity.Y == 0)
                {
                    NPC.velocity.X *= 0.9f;
                    NPC.ai[0]++;
                    if (NPC.ai[0] > 22)
                    {
                        NPC.ai[0] = 0;
                    }

                    if (Main.rand.NextBool(200))
                    {
                        NPC.direction *= -1;
                    }
                }
            }
            NPC.spriteDirection = NPC.direction;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[Type], quickUnlock: true);
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.velocity.Y != 0)
            {
                NPC.frame.Y = frameHeight;
            }
            else
            {
                NPC.frame.Y = 0;
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Slush, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Slush, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            return true;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemType<PowderedAsh>(), 1, 10, 30);
        }
    }
}