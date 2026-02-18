using CalamityMod.Rarities;
using CalRemix.Content.Items.Placeables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Accessories
{
    public class XenoxApparatus : ModItem
    {
        public override void SetDefaults()
        {
            Item.value = Item.sellPrice(gold: 11, silver: 55);
            Item.rare = ModContent.RarityType<PureGreen>();
            Item.expert = true;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<CalRemixPlayer>().xApparatus = true;
        }
    }
}
