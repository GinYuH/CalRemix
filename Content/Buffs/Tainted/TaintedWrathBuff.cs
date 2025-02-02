using CalamityMod;
using CalRemix.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs.Tainted
{
    public class TaintedWrathBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Wrath;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pacifist");
            Description.SetDefault("As harmless as a fly");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(BuffID.Wrath))
            {
                player.ClearBuff(Type);
                return;
            }
            player.Remix().taintedWrath = true;
        }
    }
}
