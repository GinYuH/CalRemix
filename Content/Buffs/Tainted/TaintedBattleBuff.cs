using CalamityMod;
using CalRemix.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs.Tainted
{
    public class TaintedBattleBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Battle;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Apocalypse Scenario");
            Description.SetDefault("Those who remain are the richest");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(BuffID.Battle))
            {
                player.ClearBuff(Type);
                return;
            }
            player.Remix().taintedBattle = true;
        }
    }
}
