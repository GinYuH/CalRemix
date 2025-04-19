using CalamityMod.Items.Accessories;
using CalamityMod.Rarities;
using CalamityMod.Items;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Accessories
{
    public class YharimsCurse : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            // DisplayName.SetDefault("Yharim's Curse");
            /* Tooltip.SetDefault("This unholy abomination feels like it's deteriorating\n"+
            "90% more damage taken\n" +
            "50% more damage dealt\n" + // yes i buffed it FUCK you
            "Why would anyone use this"); */
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemType<YharimsGift>();
        }

        public override void SetDefaults()
        {
            Item.width = 62;
            Item.height = 70;
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.rare = RarityType<Violet>();
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage<GenericDamageClass>() += 0.5f;
            player.GetModPlayer<CalRemixPlayer>().cursed = true;

        }
    }
}
