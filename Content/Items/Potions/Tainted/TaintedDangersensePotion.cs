using CalamityMod;
using CalamityMod.Items.Potions;
using CalRemix.Content.Buffs.Tainted;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions.Tainted
{
    public class TaintedDangersensePotion : TaintedPotion
    {
        public override int BuffType => ModContent.BuffType<TaintedDangersenseBuff>();
        public override int BuffTime => ContentSamples.ItemsByType[PotionType].buffTime;
        public override int PotionType => ItemID.TrapsightPotion;
        public override int MeatAmount => 4;
        public override string PotionName => "Dangersense";
    }
}