using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs
{
    public class Anxiety : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Calamitous Anxiety");
            Main.debuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed += 0.15f;
            player.statDefense += 5;
            player.GetDamage<GenericDamageClass>() -= 0.5f;
            player.GetCritChance<GenericDamageClass>() -= 0.9f;
        }
    }
}
