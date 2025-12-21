using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Armor.RajahChampion.Carrot
{
    public class CBoost1 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Champion Boost");
            // Description.SetDefault("Increased stats");
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            CalRemixPlayer mplayer = player.GetModPlayer<CalRemixPlayer>();

            player.manaRegenBonus += 25;
            player.GetDamage(DamageClass.Generic) += 0.18f * mplayer.CarrotBuff;
            player.lifeRegen += 12 * mplayer.CarrotBuff;

            if (player.buffTime[buffIndex] == 2)
            {
				player.DelBuff(buffIndex);
				buffIndex--;
            }
        }
    }
}