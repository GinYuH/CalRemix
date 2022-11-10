using CalRemix.Projectiles;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Buffs
{
    public class EarthElementalBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Earth Elemental");
            Description.SetDefault("The earth elemental will protect you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();

            if (!modPlayer.earthele)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
            else
            {
                player.buffTime[buffIndex] = 18000;
            }
        }
    }
}
