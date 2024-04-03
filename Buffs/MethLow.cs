using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Buffs
{
    public class MethLow : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sunken Withdrawal");
            // Description.SetDefault("You feel like garbage");
        }



        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage<GenericDamageClass>() += 0.1f;
            player.statDefense -= 20;
            player.moveSpeed += 0.2f;
            player.accRunSpeed += 0.2f;
            player.GetCritChance<GenericDamageClass>() -= 0.5f;
            // it's supposed to apply a shader but i am not of sound enough mind at 5am to write one
            // or ever
            if (player.HasBuff(ModContent.BuffType<MethHigh>()))
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}
