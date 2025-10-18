using CalRemix.Content.Buffs.Tainted;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions.Tainted
{
    public class TaintedSpelunkerPotion : TaintedPotion
    {
        public override int BuffType => ModContent.BuffType<TaintedSpelunkerBuff>();
        public override int BuffTime => ContentSamples.ItemsByType[PotionType].buffTime;
        public override int PotionType => ItemID.SpelunkerPotion;
        public override int MeatAmount => 6;
        public override string PotionName => "Spelunker";
    }
}