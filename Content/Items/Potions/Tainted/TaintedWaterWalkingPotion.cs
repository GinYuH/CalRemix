using CalRemix.Content.Buffs.Tainted;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions.Tainted
{
    public class TaintedWaterWalkingPotion : TaintedPotion
    {
        public override int BuffType => ModContent.BuffType<TaintedWaterWalkingBuff>();
        public override int BuffTime => ContentSamples.ItemsByType[PotionType].buffTime;
        public override int PotionType => ItemID.WaterWalkingPotion;
        public override int MeatAmount => 9;
        public override string PotionName => "Water Walking";
    }
}