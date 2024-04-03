using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Buffs
{
    public class MethHigh : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sunken Power");
            // Description.SetDefault("You feel euphoric and ready for anything");
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
            if (player.buffTime[buffIndex] == 0);
            {
            player.AddBuff(ModContent.BuffType<Buffs.MethLow>(), 216000);
            }
        }
    }
}
