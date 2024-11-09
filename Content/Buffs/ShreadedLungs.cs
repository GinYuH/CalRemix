using CalamityMod;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs
{
    public class ShreadedLungs : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shreaded Lungs");
            Description.SetDefault("No that isn't a typo!");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.Remix().shreadedLungs < npc.buffTime[buffIndex])
                npc.Remix().shreadedLungs = npc.buffTime[buffIndex];
            npc.DelBuff(buffIndex);
            buffIndex--;
        }
    }
}
