using CalamityMod;
using CalamityMod.Items.Potions;
using CalRemix.Content.Buffs.Tainted;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions.Tainted
{
    public class TaintedLovePotion : TaintedPotion
    {
        public override int BuffType => ModContent.BuffType<TaintedLoveBuff>();
        public override int BuffTime => CalamityUtils.SecondsToFrames(300);
        public override int PotionType => ItemID.LovePotion;
        public override int MeatAmount => 4;
        public override string PotionName => "Love";
    }
}