using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.Items.Materials;

namespace CalRemix.Content.NPCs
{
    public class WulfrumFrog : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 13;
            this.HideFromBestiary();
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Frog);
            AIType = NPCID.Frog;
            AnimationType = NPCID.Frog;
            NPC.catchItem = 0;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToSickness = false;
            NPC.Calamity().VulnerableToElectricity = true;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.t_Martian, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.t_Martian, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<WulfrumMetalScrap>(), 1, 1, 2);
        }
    }
}
