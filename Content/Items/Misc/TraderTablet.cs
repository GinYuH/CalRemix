using CalamityMod.Items;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.CalPlayer;
using CalamityMod.NPCs.SlimeGod;
using CalamityMod.Items.Materials;
using CalRemix.Content.Projectiles.Accessories;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using Terraria.ModLoader;
using CalRemix.Core;
using Terraria.DataStructures;

namespace CalRemix.Content.Items.Misc
{
    public class TraderTablet : ModItem
    {
        public override string Texture => "CalRemix/icon_small";

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.value = 0;
            Item.rare = ItemRarityID.LightRed;
            Item.maxStack = 1;
            Item.useTime = 15;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }
        public override bool CanUseItem(Player player) { return true; }
        public override bool? UseItem(Player player)
        {
            Main.NewText(StockMarketSystem.GetStockPrices());
            if(player.GetModPlayer<CalRemixPlayer>().HasStock("CAL")) Main.NewText("You have " + player.GetModPlayer<CalRemixPlayer>().stocks["CAL"] + " shares of CAL");
            return true;
        }
    }
}
