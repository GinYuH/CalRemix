using CalRemix.Content.Buffs.Tainted;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions.Tainted
{
    public class TaintedHeartreachPotion : TaintedPotion
    {
        public override int BuffType => ModContent.BuffType<TaintedHeartBuff>();
        public override int BuffTime => ContentSamples.ItemsByType[PotionType].buffTime;
        public override int PotionType => ItemID.HeartreachPotion;
        public override int MeatAmount => 2;
        public override string PotionName => "Heartreach";
    }
}