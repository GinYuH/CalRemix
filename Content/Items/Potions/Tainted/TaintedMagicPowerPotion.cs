using CalamityMod;
using CalamityMod.Items.Potions;
using CalRemix.Content.Buffs.Tainted;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions.Tainted
{
    public class TaintedMagicPowerPotion : TaintedPotion
    {
        public override int BuffType => ModContent.BuffType<TaintedMagicPowerBuff>();
        public override int BuffTime => ContentSamples.ItemsByType[PotionType].buffTime;
        public override int PotionType => ItemID.MagicPowerPotion;
        public override int MeatAmount => 1;
        public override string PotionName => "Magic Power";
    }
}