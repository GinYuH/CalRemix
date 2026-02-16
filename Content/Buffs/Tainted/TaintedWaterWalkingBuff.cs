using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs.Tainted
{
    public class TaintedWaterWalkingBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.WaterWalking;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Air Walking");
            // Description.SetDefault("Magic glass");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(BuffID.WaterWalking))
            {
                player.ClearBuff(Type);
                return;
            }
            player.Remix().taintedWater = true;
        }
    }
}
