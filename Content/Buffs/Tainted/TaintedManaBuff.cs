using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs.Tainted
{
    public class TaintedManaBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.ManaRegeneration;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Mana Acceleration");
            // Description.SetDefault("Beat the stars out of them!");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(BuffID.ManaRegeneration))
            {
                player.ClearBuff(Type);
                return;
            }
            player.Remix().taintedMana = true;
        }
    }
}
