using CalamityMod;
using CalRemix.Content.Items.Armor;
using CalRemix.Content.Items.Potions;
using CalRemix.Core.Biomes;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Subworlds.Sealed
{
    public class Observer : ModNPC
    {
        public override void SetDefaults()
        {
            NPC.aiStyle = NPCAIStyleID.FaceClosestPlayer;
            NPC.width = 60;
            NPC.height = 134;
            NPC.lifeMax = 1000;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.noGravity = false;
            NPC.HitSound = null;
            NPC.DeathSound = null;
            NPC.knockBackResist = 0f;
            NPC.noTileCollide = false;
            NPC.scale = 1;
            SpawnModBiomes = new int[] { ModContent.GetInstance<TurnipBiome>().Type, ModContent.GetInstance<SealedDimensionBiome>().Type };
        }
        public override void AI()
        {
            NPC.TargetClosest();
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<ObserverMask>(), 5);
            npcLoot.Add(ModContent.ItemType<ObserverEye>(), 1, 3, 6);
            npcLoot.Add(ItemID.CopperPickaxe, 10);
        }
    }
}
