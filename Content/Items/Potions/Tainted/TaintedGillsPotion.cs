using CalRemix.Content.Buffs.Tainted;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions.Tainted
{
    public class TaintedGillsPotion : TaintedPotion
    {
        public override int BuffType => ModContent.BuffType<TaintedGillsBuff>();
        public override int BuffTime => ContentSamples.ItemsByType[PotionType].buffTime;
        public override int PotionType => ItemID.GillsPotion;
        public override int MeatAmount => 7;
        public override string PotionName => "Gills";
    }
}