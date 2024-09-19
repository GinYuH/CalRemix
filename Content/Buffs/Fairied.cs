using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs
{
    public class Fairied : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fairy Float");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.vortexDebuff = true;
            player.GetDamage<GenericDamageClass>() -= 0.07f;
            player.statDefense += 14;
            player.endurance += 0.07f;
        }
    }
}
