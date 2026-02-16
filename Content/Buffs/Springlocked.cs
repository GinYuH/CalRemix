using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs
{
    public class Springlocked : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<CalRemixPlayer>().springlocked = true;
        }
    }
}
