﻿using CalamityMod;
using CalamityMod.Items.Potions;
using CalRemix.Content.Buffs.Tainted;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions.Tainted
{
    public class TaintedMiningPotion : TaintedPotion
    {
        public override int BuffType => ModContent.BuffType<TaintedMiningBuff>();
        public override int BuffTime => ContentSamples.ItemsByType[PotionType].buffTime;
        public override int PotionType => ItemID.MiningPotion;
        public override int MeatAmount => 2;
        public override string PotionName => "Mining";
    }
}