using CalamityMod;
using CalRemix.Content.Items.Accessories;
using CalRemix.Core.Retheme;
using CalRemix.Core.World;
using System;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public class SneakerSlot : ModAccessorySlot
    {
        public override bool IsEnabled()
        {
            return CalRemixWorld.sneakerheadMode;
        }

        public override bool IsHidden() => IsEmpty && !IsEnabled();

        public override bool CanAcceptItem(Item checkItem, AccessorySlotType context)
        {
            return SneakersRetheme.SneakerList[checkItem.type];
        }

        public override string FunctionalTexture => "CalRemix/UI/SneakerSlot";

        public override void ApplyEquipEffects()
        {
            base.ApplyEquipEffects();
            if (FunctionalItem != null)
            {
                SneakersRetheme.sneakerIntroMessage.ActivateMessage();
                Player.GetModPlayer<NetWorthPlayer>().netWorthCap += NetWorthPlayer.netWorthCapPerSneaker[FunctionalItem.type];
                Player.GetModPlayer<NetWorthPlayer>().netWorthSpeed += (int)Math.Pow(FunctionalItem.rare, 23);
            }
        }

        public override bool ModifyDefaultSwapSlot(Item item, int accSlotToSwapTo)
        {
            return SneakersRetheme.SneakerList[item.type];
        }
        public override bool DrawVanitySlot => false;

        public override void OnMouseHover(AccessorySlotType context)
        {
            switch (context)
            {
                case AccessorySlotType.FunctionalSlot:
                    Main.hoverItemName = CalRemixHelper.LocalText("UI.SneakerSlot").Value;
                    break;
            }
        }
    }

    public class SneakerSlot2 : SneakerSlot
    {
        public override string Name => "ExpertSneakerSlot";
        public override bool IsEnabled() =>  Player.extraAccessory && CalRemixWorld.sneakerheadMode;
    }
}