using Terraria;
using Terraria.ModLoader;
using CalRemix.Content.Items.Mounts;

namespace CalRemix.Content.Buffs
{
    public class DepthGliderBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.mount.SetMount(ModContent.MountType<DepthGlider>(), player);
            player.buffTime[buffIndex] = 10;
        }
    }
}