using CalamityMod;
using CalRemix.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs.Tainted
{
    public class TaintedFeatherfallBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Featherfall;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Friendly Gravity");
            // Description.SetDefault("Why couldn't they take fall damage before?");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(BuffID.Featherfall))
            {
                player.ClearBuff(Type);
                return;
            }
            player.Remix().taintedFeather = true;
        }
    }
}
