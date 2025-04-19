using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs
{
    public class GarBoost : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.manaCost = 0;
        }
    }
}
