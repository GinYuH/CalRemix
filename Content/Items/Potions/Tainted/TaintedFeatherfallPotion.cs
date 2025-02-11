using CalamityMod;
using CalamityMod.Items.Potions;
using CalRemix.Content.Buffs.Tainted;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions.Tainted
{
    public class TaintedFeatherfallPotion : TaintedPotion
    {
        public override int BuffType => ModContent.BuffType<TaintedFeatherfallBuff>();
        public override int BuffTime => ContentSamples.ItemsByType[PotionType].buffTime;
        public override int PotionType => ItemID.FeatherfallPotion;
        public override int MeatAmount => 3;
        public override string PotionName => "Featherfall";
    }
}