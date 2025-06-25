using CalamityMod;
using CalRemix.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs.Tainted
{
    public class TaintedObsidianSkinBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.ObsidianSkin;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Lethal Lava");
            // Description.SetDefault("Better not touch it!");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(BuffID.ObsidianSkin))
            {
                player.ClearBuff(Type);
                return;
            }
            player.Remix().taintedObsidian = true;
        }
    }
}
