using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using CalamityMod.Rarities;

namespace CalRemix.Content.Items.Materials
{
    public class EntropicBar : ModItem
    {

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Entropic Bar");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.width = 24;
            Item.height = 24;
            Item.rare = ModContent.RarityType<BurnishedAuric>();
            Item.value = 0;
        }
    }
}
