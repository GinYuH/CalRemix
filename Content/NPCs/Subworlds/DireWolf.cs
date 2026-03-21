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
using Terraria.Enums;
using CalamityMod.NPCs.NormalNPCs;
using Microsoft.Xna.Framework.Graphics;
using CalRemix.Content.Items.Placeables.Subworlds.Wolf;
using Terraria.GameContent;

namespace CalRemix.Content.NPCs.Subworlds
{
    public class DireWolf : ModNPC
    {
        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.GoblinScout);
            NPC.aiStyle = NPCAIStyleID.Fighter;
            AIType = NPCID.GoblinScout;
            NPC.damage = 80;
            NPC.width = 82;
            NPC.height = 60;
            NPC.defense = 16;
            NPC.lifeMax = 8000;
            NPC.value = Item.buyPrice(silver: 50);
            NPC.HitSound = Rimehound.HitSound with { Pitch = -0.4f };
            NPC.DeathSound = Rimehound.GrowlSound with { Pitch = -0.6f };
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToCold = false;
            NPC.Calamity().VulnerableToSickness = true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }
        public override void AI()
        {
            float legAngle = MathHelper.ToRadians(30);
            NPC.spriteDirection = NPC.velocity.X.DirectionalSign();
            float animSpeed = 0.03f * MathHelper.Max(NPC.Calamity().newAI[2], 1);
            if (NPC.Calamity().newAI[0] >= legAngle && NPC.Calamity().newAI[3] == 0)
            {
                NPC.Calamity().newAI[3] = 1;
            }
            else if (NPC.Calamity().newAI[0] <= -legAngle && NPC.Calamity().newAI[3] == 1)
            {
                NPC.Calamity().newAI[3] = 0;
            }

            if (NPC.velocity.Y >= 0)
            {
                NPC.Calamity().newAI[0] += (NPC.Calamity().newAI[3] == 0).ToDirectionInt() * animSpeed;
            }
            else
            {
                NPC.Calamity().newAI[0] = 0;
            }
            NPC.Calamity().newAI[0] = MathHelper.Clamp(NPC.Calamity().newAI[0], -legAngle, legAngle);
            if (NPC.justHit)
            {
                NPC.Calamity().newAI[2] = 30;
            }
            NPC.Calamity().newAI[2]--;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = TextureAssets.Npc[Type].Value;
            Texture2D leg = ModContent.Request<Texture2D>(Texture + "_Leg").Value;
            Texture2D leg2 = ModContent.Request<Texture2D>(Texture + "_Leg2").Value;
            Vector2 drawPos = NPC.Bottom - screenPos + NPC.gfxOffY * Vector2.UnitY - Vector2.UnitY * 30;
            if (NPC.Calamity().newAI[2] > 15)
                drawColor = Color.Red;
            spriteBatch.Draw(leg2, drawPos + new Vector2(42 * -NPC.spriteDirection, -20), null, NPC.GetAlpha(drawColor), -NPC.Calamity().newAI[0], new Vector2(leg2.Width / 2, 0), NPC.scale, NPC.FlippedEffects(), 0);
            spriteBatch.Draw(leg2, drawPos + new Vector2(-6 * -NPC.spriteDirection, -20), null, NPC.GetAlpha(drawColor), NPC.Calamity().newAI[0], new Vector2(leg2.Width / 2, 0), NPC.scale, NPC.FlippedEffects(), 0);
            spriteBatch.Draw(leg, drawPos + new Vector2(42 * -NPC.spriteDirection, -20), null, NPC.GetAlpha(drawColor), NPC.Calamity().newAI[0], new Vector2(leg.Width / 2, 0), NPC.scale, NPC.FlippedEffects(), 0);
            spriteBatch.Draw(leg, drawPos + new Vector2(-6 * -NPC.spriteDirection, -20), null, NPC.GetAlpha(drawColor), -NPC.Calamity().newAI[0], new Vector2(leg.Width / 2, 0), NPC.scale, NPC.FlippedEffects(), 0);
            spriteBatch.Draw(tex, drawPos, null, NPC.GetAlpha(drawColor), NPC.rotation, new Vector2(tex.Width / 2, tex.Height), NPC.scale, NPC.FlippedEffects(), 0);
            return false;
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
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<WolfEarth>(), 1, 10, 24);
            npcLoot.Add(ModContent.ItemType<WolfSnow>(), 1, 10, 24);
            npcLoot.Add(ModContent.ItemType<WolfTree>(), 1, 10, 24);
        }
    }
}
