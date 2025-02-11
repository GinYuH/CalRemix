using CalamityMod;
using CalRemix.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs.Tainted
{
    public class TaintedLifeBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Lifeforce;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mana Blood");
            Description.SetDefault("Your health is being converted into mana");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(BuffID.Lifeforce))
            {
                player.ClearBuff(Type);
                return;
            }
            player.Remix().taintedLife = true;
        }
    }
}
