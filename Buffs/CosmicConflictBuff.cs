using CalRemix.Projectiles;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Buffs
{
    public class CosmicConflictBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cosmic Conflict Construct");
            Description.SetDefault("The cosmic conflict construct will protect you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();

            if (!modPlayer.crystalconflict)
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
