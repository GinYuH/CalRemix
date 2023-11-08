using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using CalRemix.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Items.Placeables.Ores;

namespace CalRemix.Items
{
    public class TarGar : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tar Gar");
            Tooltip.SetDefault("Right click to extract slime rain");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.width = 24;
            Item.height = 24;
            Item.rare = ModContent.RarityType<Violet>();
        }

        public override bool CanRightClick()
        {
            return !Main.slimeRain;
        }
        public override void RightClick(Player player)
        {
            Main.StartSlimeRain();
        }
    }
}
