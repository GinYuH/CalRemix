using CalRemix.NPCs.Bosses.BossScule;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Buffs
{
    public class Calamitized : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Calamitized");
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
        }
    }
}
