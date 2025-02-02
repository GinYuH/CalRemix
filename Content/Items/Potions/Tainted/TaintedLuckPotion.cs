using CalamityMod;
using CalamityMod.Items.Potions;
using CalRemix.Content.Buffs.Tainted;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions.Tainted
{
    public class TaintedLuckPotion : TaintedPotion
    {
        public override int BuffType => ModContent.BuffType<TaintedLuckBuff>();
        public override int BuffTime => ContentSamples.ItemsByType[PotionType].buffTime;
        public override int PotionType => ItemID.LuckPotion;
        public override int MeatAmount => 60;
        public override string PotionName => "Luck";
    }
}