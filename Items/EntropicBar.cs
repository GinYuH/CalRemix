using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using CalamityMod.Rarities;

namespace CalRemix.Items
{
    public class EntropicBar : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Entropic Bar");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.width = 24;
            Item.height = 24;
            Item.rare = ModContent.RarityType<Violet>();
            Item.value = 0;
        }
    }
}
