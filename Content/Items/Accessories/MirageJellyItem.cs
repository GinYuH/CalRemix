using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;

namespace CalRemix.Content.Items.Accessories
{
    public class MirageJellyItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("Mirage Jelly");
            Tooltip.SetDefault("Lowers the cost of stealth strikes by 5%");
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            item.height = 20;
            Item.value = CalamityGlobalItem.RarityLimeBuyPrice;
            Item.rare = ItemRarityID.Lime;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalRemixPlayer modplayer = player.GetModPlayer<CalRemixPlayer>();
            modplayer.miragel = true;
        }
    }
}
