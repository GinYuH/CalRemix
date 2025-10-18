using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs
{
    public class StratusBeverageBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Stratus Beverage");
            // Description.SetDefault("Seeing stars");
            Main.debuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage<GenericDamageClass>() -= 0.33f;
            player.GetModPlayer<CalRemixPlayer>().stratusBeverage = true;
        }
    }
}
