using CalamityMod;
using CalRemix.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs.Tainted
{
    public class TaintedInfernoBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Inferno;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Ablaze");
            // Description.SetDefault("Everybody burns");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(BuffID.Inferno))
            {
                player.ClearBuff(Type);
                return;
            }
            player.Remix().taintedInferno = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.Remix().taintedInferno < npc.buffTime[buffIndex])
                npc.Remix().taintedInferno = npc.buffTime[buffIndex];
            npc.DelBuff(buffIndex);
            buffIndex--;
        }
    }
}
