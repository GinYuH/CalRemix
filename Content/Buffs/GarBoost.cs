using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs
{
    public class GarBoost : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gar Mana Boost");
            Description.SetDefault("Temporary unlimited mana");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.manaCost = 0;
        }
    }
}
