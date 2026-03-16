using CalamityMod;
using CalRemix.Content.Items.Placeables.Subworlds.Pinnacles;
using CalRemix.Content.Items.Potions;
using CalRemix.Core.Biomes;
using CalRemix.Core.Biomes.Subworlds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace CalRemix.Content.NPCs.Subworlds.Pinnacles
{
    public class Gerasimekon : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.CantTakeLunchMoney[Type] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 100;
            NPC.height = 100;
            NPC.HitSound = SoundID.NPCDeath43;
            NPC.DeathSound = BetterSoundID.ItemSolarEruption with { Pitch = -1 };
            NPC.lifeMax = 10000;
            NPC.defense = 50;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToSickness = true;
            NPC.Calamity().VulnerableToCold = false;
            NPC.Calamity().VulnerableToWater = true;
            SpawnModBiomes = [ModContent.GetInstance<PinnaclesBiome>().Type];
            NPC.noGravity = true;
        }

        public override void AI()
        {
            if (NPC.ai[0] == 0)
            {
                NPC.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
                NPC.direction = NPC.spriteDirection = Main.rand.NextBool().ToDirectionInt();
                NPC.ai[1] = Main.rand.NextBool().ToDirectionInt() * Main.rand.NextFloat(0.3f, 0.6f);
            }
            else
            {
                if (NPC.ai[0] % 20 == 0)
                NPC.rotation += NPC.ai[1];
            }
            NPC.velocity *= 0.98f;
            NPC.ai[0]++;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[Type], quickUnlock: true);
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
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
            Texture2D tex = TextureAssets.Npc[Type].Value;
            spriteBatch.Draw(tex, NPC.Center - screenPos, null, NPC.GetAlpha(drawColor), NPC.rotation, tex.Size() / 2, NPC.scale, NPC.FlippedEffects(), 0);
            return false;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemType<Rhyolite>(), 1, 10, 30);
        }
    }
}