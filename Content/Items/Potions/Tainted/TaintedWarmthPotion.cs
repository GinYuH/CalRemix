using CalRemix.Content.Buffs.Tainted;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions.Tainted
{
    public class TaintedWarmthPotion : TaintedPotion
    {
        public override int BuffType => ModContent.BuffType<TaintedWarmthBuff>();
        public override int BuffTime => ContentSamples.ItemsByType[PotionType].buffTime;
        public override int PotionType => ItemID.WarmthPotion;
        public override int MeatAmount => 1;
        public override string PotionName => "Warmth";
    }
}