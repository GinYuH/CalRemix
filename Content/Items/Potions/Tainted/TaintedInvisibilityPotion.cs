using CalRemix.Content.Buffs.Tainted;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions.Tainted
{
    public class TaintedInvisibilityPotion : TaintedPotion
    {
        public override int BuffType => ModContent.BuffType<TaintedInvisibilityBuff>();
        public override int BuffTime => ContentSamples.ItemsByType[PotionType].buffTime;
        public override int PotionType => ItemID.InvisibilityPotion;
        public override int MeatAmount => 6;
        public override string PotionName => "Invisibility";
    }
}