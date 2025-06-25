using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using CalamityMod.Items.Materials;

namespace CalRemix.Content.Items.Bags
{
    public class SideGar : ModItem
    {

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Side Gar");
            // Tooltip.SetDefault("Right click to extract galactica singularities");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.width = 24;
            Item.height = 24;
            Item.rare = ItemRarityID.Red;
            Item.value = Item.sellPrice(gold: 1);
        }

        public override bool CanRightClick()
        {
            return true;
        }
        public override void RightClick(Player player)
        {
            player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<GalacticaSingularity>(), Main.rand.Next(5, 16));
        }
    }
}
