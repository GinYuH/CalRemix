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
            NPC.width = 60;
            NPC.height = 72;
            NPC.lavaImmune = true;
            NPC.HitSound = CommonCalamitySounds.ExoHitSound;
            NPC.DeathSound = CommonCalamitySounds.ExoDeathSound with { Pitch = -1 };
            NPC.lifeMax = 5000;
            NPC.defense = 100;
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
            npcLoot.Add(ModContent.ItemType<Mercury>(), 1, 1, 2);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }
    }
}