using CalamityMod;
using CalRemix.Content.Items.Accessories;
using CalRemix.Core.World;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public class SoulSlot : ModAccessorySlot
    {
        public override bool IsEnabled()
        {
            // GetModPlayer will throw an index error in this step of the loading process for whatever reason
            // We prematurely stop it from getting to that point
            if (!Player.active)
                return false;

            return RemixDowned.downedOrigen || RemixDowned.downedCarcinogen || DownedBossSystem.downedCryogen || RemixDowned.downedIonogen
                || RemixDowned.downedPathogen || RemixDowned.downedOxygen || RemixDowned.downedPhytogen || RemixDowned.downedHydrogen
                || RemixDowned.downedPyrogen;
        }
        public override bool IsHidden() => IsEmpty && !IsEnabled();

        public override bool CanAcceptItem(Item checkItem, AccessorySlotType context)
        {
            if (CalRemixItem.genSouls.Contains(checkItem.type) || checkItem.type == ModContent.ItemType<SoulofOrigen>())
            {
                return true;
            }
            return false;
        }

        public override string FunctionalTexture => "CalRemix/UI/SoulSlot";

        public override bool ModifyDefaultSwapSlot(Item item, int accSlotToSwapTo)
        {
            return CalRemixItem.genSouls.Contains(item.type) || item.type == ModContent.ItemType<SoulofOrigen>();
        }
        public override bool DrawVanitySlot => false;

        public override void OnMouseHover(AccessorySlotType context)
        {
            switch (context)
            {
                case AccessorySlotType.FunctionalSlot:
                    Main.hoverItemName = CalRemixHelper.LocalText("UI.SoulSlot").Value;
                    break;
            }
        }
    }
}