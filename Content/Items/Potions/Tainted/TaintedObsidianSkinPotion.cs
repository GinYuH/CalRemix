﻿using CalamityMod;
using CalamityMod.Items.Potions;
using CalRemix.Content.Buffs.Tainted;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions.Tainted
{
    public class TaintedObsidianSkinPotion : TaintedPotion
    {
        public override int BuffType => ModContent.BuffType<TaintedObsidianSkinBuff>();
        public override int BuffTime => ContentSamples.ItemsByType[PotionType].buffTime;
        public override int PotionType => ItemID.ObsidianSkinPotion;
        public override int MeatAmount => 8;
        public override string PotionName => "Obsidian Skin";
    }
}