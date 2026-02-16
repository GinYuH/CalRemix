using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs.Tainted
{
    public class TaintedSonarBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Sonar;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Restless Fish");
            // Description.SetDefault("Your fishing lines shake rapidly");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(BuffID.Sonar))
            {
                player.ClearBuff(Type);
                return;
            }
            player.Remix().taintedSonar = true;
        }
    }
}
