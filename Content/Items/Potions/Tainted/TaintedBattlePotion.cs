using CalamityMod;
using CalamityMod.Items.Potions;
using CalRemix.Content.Buffs.Tainted;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions.Tainted
{
    public class TaintedBattlePotion : TaintedPotion
    {
        public override int BuffType => ModContent.BuffType<TaintedBattleBuff>();
        public override int BuffTime => ContentSamples.ItemsByType[PotionType].buffTime;
        public override int PotionType => ItemID.BattlePotion;
        public override int MeatAmount => 2;
        public override string PotionName => "Battle";
    }
}