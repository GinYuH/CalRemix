using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs
{
    public class HayFeaver : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hay Fever");
            Description.SetDefault("You're sneezing out blood!");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Terraria.ID.BuffID.Sets.LongerExpertDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<CalRemixPlayer>().hayFever = true;
        }
    }
}
