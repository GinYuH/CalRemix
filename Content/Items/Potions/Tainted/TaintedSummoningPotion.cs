using CalRemix.Content.Buffs.Tainted;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions.Tainted
{
    public class TaintedSummoningPotion : TaintedPotion
    {
        public override int BuffType => ModContent.BuffType<TaintedSummoningBuff>();
        public override int BuffTime => ContentSamples.ItemsByType[PotionType].buffTime;
        public override int PotionType => ItemID.SummoningPotion;
        public override int MeatAmount => 5;
        public override string PotionName => "Summoning";
    }
}