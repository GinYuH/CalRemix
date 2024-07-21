using CalRemix.Projectiles.Weapons;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Buffs
{
    public class AergiaNeuronBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aergia Neuron");

            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<AergiaNeuronCore>()] > 0)
                player.GetModPlayer<CalRemixPlayer>().neuron = true;
            if (!player.GetModPlayer<CalRemixPlayer>().neuron)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
            else
                player.buffTime[buffIndex] = 18000;
        }
    }
}