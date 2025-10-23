using CalamityMod;
using CalamityMod.Sounds;
using CalRemix.Content.Items.Materials;
using CalRemix.Core.Biomes;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Subworlds.Sealed
{
    public class Disilphia : ModNPC
    {
        public override void SetDefaults()
        {
            NPC.width = 1150;
            NPC.height = 1150;
            NPC.lavaImmune = true;
            NPC.HitSound = CommonCalamitySounds.ExoHitSound;
            NPC.DeathSound = CommonCalamitySounds.ExoDeathSound with { Pitch = -1 };
            NPC.lifeMax = 300000;
            NPC.defense = 200;
            NPC.Calamity().VulnerableToHeat = false;
            NPC.Calamity().VulnerableToSickness = false;
            NPC.Calamity().VulnerableToCold = false;
            NPC.Calamity().VulnerableToWater = true;
            NPC.Calamity().VulnerableToElectricity = true;
            SpawnModBiomes = [ModContent.GetInstance<VolcanicFieldBiome>().Type];
        }

        public override void AI()
        {
            NPC.spriteDirection = NPC.direction;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<MercuryCoatedSubcinium>(), 1, 16, 30);
            npcLoot.Add(ModContent.ItemType<Mercury>(), 1, 25, 40);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }
    }
}