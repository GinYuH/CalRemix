using CalamityMod;
using CalRemix.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs.Tainted
{
    public class TaintedDangersenseBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Dangersense;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Danger");
            // Description.SetDefault("Everything is out to kill you!");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(BuffID.Dangersense))
            {
                player.ClearBuff(Type);
                return;
            }
            player.Remix().taintedDanger = true;
        }
    }
}
