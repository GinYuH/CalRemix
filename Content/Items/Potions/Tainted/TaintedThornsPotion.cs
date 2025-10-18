using CalRemix.Content.Buffs.Tainted;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions.Tainted
{
    public class TaintedThornsPotion : TaintedPotion
    {
        public override int BuffType => ModContent.BuffType<TaintedThornsBuff>();
        public override int BuffTime => ContentSamples.ItemsByType[PotionType].buffTime;
        public override int PotionType => ItemID.ThornsPotion;
        public override int MeatAmount => 1;
        public override string PotionName => "Thorns";
    }
}