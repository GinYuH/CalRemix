using CalRemix.Content.Buffs.Tainted;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions.Tainted
{
    public class TaintedArcheryPotion : TaintedPotion
    {
        public override int BuffType => ModContent.BuffType<TaintedArcheryBuff>();
        public override int BuffTime => ContentSamples.ItemsByType[PotionType].buffTime;
        public override int PotionType => ItemID.ArcheryPotion;
        public override int MeatAmount => 1;
        public override string PotionName => "Archery";
    }
}