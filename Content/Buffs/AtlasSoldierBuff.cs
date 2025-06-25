﻿using CalRemix.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs
{
    public class AtlasSoldierBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<AtlasSoldier>()] > 0)
            {
                player.GetModPlayer<CalRemixPlayer>().soldier = true;
            }
            if (!player.GetModPlayer<CalRemixPlayer>().soldier)
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
