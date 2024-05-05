﻿using CalamityMod.Rarities;
using CalamityMod.UI;
using CalamityMod.UI.DraedonLogs;
using CalRemix.UI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items
{
    public class FannyLogSpace : ModItem, ILocalizedModType
    {
        public override string Texture => "CalamityMod/Items/DraedonMisc/DraedonsLogJungle";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Trouble on Planetoid X-42 (Entry 2)");
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.rare = ModContent.RarityType<DarkOrange>();
            Item.useAnimation = Item.useTime = 20;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }

        public override bool? UseItem(Player player)
        {
            if (Main.myPlayer == player.whoAmI)
                PopupGUIManager.FlipActivityOfGUIWithType(typeof(FannyLog2));
            return true;
        }
    }
}
