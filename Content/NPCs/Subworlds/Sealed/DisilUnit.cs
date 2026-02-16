using CalamityMod;
using CalamityMod.Sounds;
using CalRemix.Content.Items.Materials;
using CalRemix.Core.Biomes;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
//using CalamityMod.CalPlayer;

namespace CalRemix.Content.NPCs.Subworlds.Sealed
{
    public class DisilUnit : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 2;
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Mouse);
            NPC.width = 60;
            NPC.height = 72;
            NPC.lavaImmune = false;
            AIType = NPCID.Mouse;
            NPC.HitSound = CommonCalamitySounds.ExoHitSound;
            NPC.DeathSound = CommonCalamitySounds.ExoDeathSound with { Pitch = 1 };
            NPC.lifeMax = 5000;
            NPC.defense = 100;
            NPC.chaseable = false;
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
        public override void FindFrame(int frameHeight)
        {
            if (NPC.velocity.Y != 0)
            {
                NPC.frame.Y = frameHeight;
            }
            else if (NPC.velocity.X != 0)
            {
                NPC.frameCounter += 0.2f;
                NPC.frameCounter %= Main.npcFrameCount[NPC.type];
                int frame = (int)NPC.frameCounter;
                NPC.frame.Y = frame * frameHeight;
            }
            else
            {
                NPC.frame.Y = 0;
            }
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