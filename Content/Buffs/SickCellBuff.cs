using CalRemix.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs
{
    public class SickCellBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Sick Cell");
            // Description.SetDefault("Sick 'em, boys!");

            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<SickCell>()] > 0)
                player.GetModPlayer<CalRemixPlayer>().sickcell = true;
            if (!player.GetModPlayer<CalRemixPlayer>().sickcell)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
            else
                player.buffTime[buffIndex] = 18000;
        }
    }
}