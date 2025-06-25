using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;

namespace CalRemix.Content.Items.Accessories
{
    public class IrateJelly : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            // DisplayName.SetDefault("Irate Jelly");
            // Tooltip.SetDefault("Activating Rage will deal 50% of your health as damage");
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 20;
            Item.value = CalamityGlobalItem.RarityLightRedBuyPrice;
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalRemixPlayer modplayer = player.GetModPlayer<CalRemixPlayer>();
            modplayer.irategel = true;
        }
    }
}
