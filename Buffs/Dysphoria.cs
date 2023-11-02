using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Buffs
{
    public class Dysphoria : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Calamitous Dysphoria");
            Main.debuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed -= 0.1f;
            player.GetDamage<GenericDamageClass>() -= 0.2f;
            player.GetCritChance<GenericDamageClass>() -= 0.2f;
            player.pickSpeed -= 0.4f;
            player.statDefense -= 10;
            if (player.HasBuff(ModContent.BuffType<Euphoria>()) || player.HasBuff(ModContent.BuffType<Anxiety>()))
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}
