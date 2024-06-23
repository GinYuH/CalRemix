using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using CalamityMod.Rarities;
using CalamityMod.Items.Placeables.Ores;

namespace CalRemix.Items.Bags
{
    public class RearGar : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rear Gar");
            Tooltip.SetDefault("Right click to extract tarragon");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.width = 24;
            Item.height = 24;
            Item.rare = ModContent.RarityType<Turquoise>();
            Item.value = Item.sellPrice(gold: 1);
        }

        public override bool CanRightClick()
        {
            return true;
        }
        public override void RightClick(Player player)
        {
            player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<UelibloomOre>(), Main.rand.Next(5, 16));
        }
    }
}
