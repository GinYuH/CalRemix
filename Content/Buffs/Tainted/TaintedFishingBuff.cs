using CalamityMod;
using CalRemix.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs.Tainted
{
    public class TaintedFishingBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Fishing;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fishing Curse");
            Description.SetDefault("No food, only fights");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(BuffID.Fishing))
            {
                player.ClearBuff(Type);
                return;
            }
            player.Remix().taintedFish = true;
        }
    }
}
