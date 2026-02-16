using CalRemix.Content.Buffs.Tainted;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions.Tainted
{
    public class TaintedAmmoReservationPotion : TaintedPotion
    {
        public override int BuffType => ModContent.BuffType<TaintedAmmoBuff>();
        public override int BuffTime => ContentSamples.ItemsByType[PotionType].buffTime;
        public override int PotionType => ItemID.AmmoReservationPotion;
        public override int MeatAmount => 5;
        public override string PotionName => "Ammo Reservation";
    }
}