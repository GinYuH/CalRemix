using CalamityMod;
using CalamityMod.Items.Potions;
using CalRemix.Content.Buffs.Tainted;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions.Tainted
{
    public class TaintedManaRegenerationPotion : TaintedPotion
    {
        public override int BuffType => ModContent.BuffType<TaintedManaBuff>();
        public override int BuffTime => ContentSamples.ItemsByType[PotionType].buffTime;
        public override int PotionType => ItemID.ManaRegenerationPotion;
        public override int MeatAmount => 4;
        public override string PotionName => "Mana Regeneration";
    }
}