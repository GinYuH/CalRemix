using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs
{
    public class Leaves : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Leaves Power");
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage<GenericDamageClass>() += 0.06f;
            player.GetCritChance<GenericDamageClass>() += 0.02f;
            player.pickSpeed += 0.05f;
            player.GetKnockback<GenericDamageClass>() += 1f; 
            player.moveSpeed += 0.15f;
            player.endurance += 0.06f;
            player.statDefense += 8;
        }
    }
}
