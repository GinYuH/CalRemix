using CalRemix.Content.NPCs.Bosses.BossScule;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs
{
    public class Calamitized : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (!NPC.AnyNPCs(ModContent.NPCType<TheCalamity>()))
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
            if (buffIndex <= 0 && NPC.AnyNPCs(ModContent.NPCType<TheCalamity>()))
            {
                player.Remix().calamitizedCounter = TheCalamity.CalamitySetCounter();
            }
        }
    }
}
