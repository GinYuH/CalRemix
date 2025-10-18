﻿using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework;
using CalamityMod.Items.Materials;

namespace CalRemix.Content.NPCs
{
    public class GildedSpirit : ModNPC
    {
        public Player Target => Main.player[NPC.target];
        public ref float Mag => ref NPC.ai[0];
        public override void SetStaticDefaults()
        {
            NPCID.Sets.ImmuneToAllBuffs[Type] = true;
        }
        public override void SetDefaults()
        {
            NPC.width = 40;
            NPC.height = 40;
            NPC.lifeMax = 500;
            NPC.damage = 100;
            NPC.defense = 0;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(silver: 25, copper: 10);
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit36;
            NPC.DeathSound = SoundID.NPCDeath39;
        }
        public override void AI()
        {
            NPC.TargetClosest();
            NPC.rotation = NPC.velocity.ToRotation() - MathHelper.PiOver2;
            if (Target != null)
            {
                if (Mag < 9)
                    Mag += 0.02f;
                NPC.velocity = NPC.DirectionFrom(Target.Center) * Mag;
            }
        }
        public override void DrawEffects(ref Color drawColor)
        {
            if (Main.rand.NextBool())
            {
                Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.GoldCoin, Scale: 2.5f);
                dust.noGravity = true;
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon,
		        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<Necroplasm>(), 1, 13, 17);
            npcLoot.Add(ItemID.GoldCoin, 1, 40, 50);
            npcLoot.Add(ItemID.PlatinumCoin, 1, 10, 15);
        }
        public override Color? GetAlpha(Color drawColor) => new Color(255, 255, 255, 100);
    }
}
