using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs
{
    public class AstralEffigyBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Astral Effigy");
            // Description.SetDefault("The astral infection empowers you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<CalRemixPlayer>().astEffigy = true;
        }
    }
}
