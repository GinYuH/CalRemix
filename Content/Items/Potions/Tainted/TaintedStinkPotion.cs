using CalamityMod;
using CalamityMod.Items.Potions;
using CalRemix.Content.Buffs.Tainted;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions.Tainted
{
    public class TaintedStinkPotion : TaintedPotion
    {
        public override int BuffType => ModContent.BuffType<TaintedStinkBuff>();
        public override int BuffTime => CalamityUtils.SecondsToFrames(300);
        public override int PotionType => ItemID.StinkPotion;
        public override int MeatAmount => 7;
        public override string PotionName => "Stink";
    }
}