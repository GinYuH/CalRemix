using CalRemix.Content.Buffs.Tainted;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions.Tainted
{
    public class TaintedGravitationPotion : TaintedPotion
    {
        public override int BuffType => ModContent.BuffType<TaintedGravityBuff>();
        public override int BuffTime => ContentSamples.ItemsByType[PotionType].buffTime;
        public override int PotionType => ItemID.GravitationPotion;
        public override int MeatAmount => 1;
        public override string PotionName => "Gravitation";
    }
}