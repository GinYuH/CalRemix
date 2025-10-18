using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs.Tainted
{
    public class TaintedShineBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Shine;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Pitch Black");
            // Description.SetDefault("True homing");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(BuffID.Shine))
            {
                player.ClearBuff(Type);
                return;
            }
            player.Remix().taintedShine = true;
        }
    }
}
