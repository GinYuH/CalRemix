﻿using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs
{
    public class ElementalAffinity : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.jumpSpeedBoost -= 0.05f;
            player.moveSpeed -= 0.55f;
            player.statDefense -= 5;
        }
    }
}
