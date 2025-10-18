using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs.Tainted
{
    public class TaintedIronskinBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Ironskin;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Battered");
            // Description.SetDefault("Beaten up, but still determined");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(BuffID.Ironskin))
            {
                player.ClearBuff(Type);
                return;
            }
            player.Remix().taintedIron = true;
        }
    }
}
