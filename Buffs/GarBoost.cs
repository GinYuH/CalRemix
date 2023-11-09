using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Buffs
{
    public class GarBoost : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gar Mana Boost");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.manaCost = 0;
        }
    }
}
