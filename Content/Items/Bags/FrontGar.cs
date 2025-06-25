using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using CalamityMod.Rarities;
using CalamityMod.Items.Materials;

namespace CalRemix.Content.Items.Bags
{
    public class FrontGar : ModItem
    {

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Front Gar");
            // Tooltip.SetDefault("Right click to extract reaper teeth");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.width = 24;
            Item.height = 24;
            Item.rare = ModContent.RarityType<PureGreen>();
            Item.value = Item.sellPrice(gold: 2);
        }

        public override bool CanRightClick()
        {
            return true;
        }
        public override void RightClick(Player player)
        {
            player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<ReaperTooth>(), Main.rand.Next(5, 16));
        }
    }
}
