using CalamityMod;
using CalamityMod.Items.Potions;
using CalRemix.Content.Buffs.Tainted;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions.Tainted
{
    public class TaintedShinePotion : TaintedPotion
    {
        public override int BuffType => ModContent.BuffType<TaintedShineBuff>();
        public override int BuffTime => ContentSamples.ItemsByType[PotionType].buffTime;
        public override int PotionType => ItemID.ShinePotion;
        public override int MeatAmount => 10;
        public override string PotionName => "Shine";
    }
}