using Microsoft.Xna.Framework;
using System.Diagnostics;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRemix.Buffs
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
