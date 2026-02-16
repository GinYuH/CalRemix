using CalRemix.Content.Buffs.Tainted;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions.Tainted
{
    public class TaintedSwiftnessPotion : TaintedPotion
    {
        public override int BuffType => ModContent.BuffType<TaintedSwiftnessBuff>();
        public override int BuffTime => ContentSamples.ItemsByType[PotionType].buffTime;
        public override int PotionType => ItemID.SwiftnessPotion;
        public override int MeatAmount => 5;
        public override string PotionName => "Swiftness";
    }
}