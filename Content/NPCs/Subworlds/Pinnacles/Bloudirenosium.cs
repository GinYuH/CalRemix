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
    public class Bloudirenosium : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.CantTakeLunchMoney[Type] = true;
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Penguin);
            NPC.width = 100;
            NPC.height = 268;
            NPC.lavaImmune = false;
            AIType = NPCID.Penguin;
            NPC.HitSound = BetterSoundID.ItemPoopSquish;
            NPC.DeathSound = BetterSoundID.ItemSolarEruption with { Pitch = -1 };
            NPC.lifeMax = 10000;
            NPC.defense = 50;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToSickness = true;
            NPC.Calamity().VulnerableToCold = false;
            NPC.Calamity().VulnerableToWater = true;
            SpawnModBiomes = [ModContent.GetInstance<PinnaclesBiome>().Type];
        }

        public override void AI()
        {
            NPC.spriteDirection = NPC.direction;

            if (NPC.velocity.X != 0)
                NPC.Remix().GreenAI[2]++;
            else
            {
                NPC.Remix().GreenAI[2] = 0;
                NPC.Remix().GreenAI[3] = 0;
                NPC.Remix().GreenAI[4] = 0;
            }

            if (NPC.Remix().GreenAI[2] % 20 == 0 && NPC.Remix().GreenAI[2] > 0)
            {
                NPC.Remix().GreenAI[0] = 0.1f * MathF.Sin(NPC.Remix().GreenAI[2]);
                NPC.Remix().GreenAI[1] = 0.1f * MathF.Sin(NPC.Remix().GreenAI[2]);
                NPC.Remix().GreenAI[3] = 0.1f * NPC.Remix().GreenAI[2];
                NPC.Remix().GreenAI[4] = 0.1f * NPC.Remix().GreenAI[2] + 22;
            }

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
            Texture2D legs = ModContent.Request<Texture2D>(Texture + "_Legs").Value;
            Texture2D head = ModContent.Request<Texture2D>(Texture + "_Head").Value;
            Texture2D tail = ModContent.Request<Texture2D>(Texture + "_Tail").Value;

            int legBaseY = 40;

            DrawPart(spriteBatch, screenPos, drawColor, legs, 0, legs.Size() / 2, new Vector2(0, legBaseY) + Vector2.UnitY.RotatedBy(-NPC.direction * NPC.Remix().GreenAI[3]) * 20);
            spriteBatch.Draw(tex, NPC.Center - screenPos, null, NPC.GetAlpha(drawColor), NPC.rotation, tex.Size() / 2, NPC.scale, NPC.FlippedEffects(), 0);
            DrawPart(spriteBatch, screenPos, drawColor, legs, 0, legs.Size() / 2, new Vector2(-40, legBaseY) + Vector2.UnitY.RotatedBy(-NPC.direction * NPC.Remix().GreenAI[4]) * 20);
            DrawPart(spriteBatch, screenPos, drawColor, head, NPC.Remix().GreenAI[0], new Vector2(190, 56), new Vector2(-160, 0));
            DrawPart(spriteBatch, screenPos, drawColor, tail, NPC.Remix().GreenAI[1], new Vector2(22, 96), new Vector2(180, 0));

            return false;
        }

        public void DrawPart(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor, Texture2D tex, float rotation, Vector2 origin, Vector2 offset)
        {
            spriteBatch.Draw(tex, NPC.Center - screenPos + new Vector2(offset.X * -NPC.direction, offset.Y), null, NPC.GetAlpha(drawColor), rotation, new Vector2((NPC.direction == 1) ? tex.Width - origin.X : origin.X, origin.Y), NPC.scale, NPC.FlippedEffects(), 0);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemType<Rhyolite>(), 1, 50, 100);
        }
    }
}