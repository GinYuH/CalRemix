using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs
{
    public class Euphoria : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Calamitous Euphoria");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage<GenericDamageClass>() += 0.5f;
            player.GetCritChance<GenericDamageClass>() -= 0.3f;
            player.statDefense -= 10;
            if (player.buffTime[buffIndex] == 0)
                player.AddBuff(ModContent.BuffType<Dysphoria>(), 162000);
            if (player.HasBuff(ModContent.BuffType<Anxiety>()))
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}
