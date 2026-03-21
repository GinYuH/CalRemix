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
            NPC.width = 32;
            NPC.height = 50;
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
            //CalamityMod.NPCs.VanillaNPCAIOverrides.RegularEnemies.RevengeanceAndDeathAI.BuffedUnicornAI(NPC, Mod);
            //NPC.spriteDirection = -NPC.velocity.X.DirectionalSign();
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
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
