using CalamityMod;
using CalamityMod.Items.Potions;
using CalRemix.Content.Buffs.Tainted;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions.Tainted
{
    public class TaintedWrathPotion : TaintedPotion
    {
        public override int BuffType => ModContent.BuffType<TaintedWrathBuff>();
        public override int BuffTime => ContentSamples.ItemsByType[PotionType].buffTime;
        public override int PotionType => ItemID.WrathPotion;
        public override int MeatAmount => 6;
        public override string PotionName => "Wrath";
    }
}