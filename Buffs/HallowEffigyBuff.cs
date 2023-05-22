using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Buffs
{
    public class HallowEffigyBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Hallow Effigy");
            // Description.SetDefault("The hallow empowers you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<CalRemixPlayer>().halEffigy = true;
        }
    }
}
