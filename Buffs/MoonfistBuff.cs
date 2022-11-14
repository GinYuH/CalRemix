using CalamityMod;
using CalamityMod.CalPlayer;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Buffs
{
    public class MoonfistBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Moon Empowerment");
            Description.SetDefault("Hit me all you want, you can't un51 the recipe");
        }

        public override void Update(Player player, ref int buffIndex)
        {
            CalamityPlayer modPlayer = player.Calamity();
            player.GetDamage<TrueMeleeDamageClass>() += 0.6f;
        }
    }
}
