using CalamityMod.Rarities;
using CalRemix.UI.Arsenal;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Misc
{
    public class DraePhone15 : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = 0;
            Item.rare = ModContent.RarityType<DarkOrange>();
            Item.useAnimation = 1;
            Item.useTime = 1;
            Item.useStyle = ItemUseStyleID.Thrust;
            Item.consumable = false;
        }
        public override bool? UseItem(Player player)
        {
            ArsenalSystem system = ModContent.GetInstance<ArsenalSystem>();
            if (!system.uiOpen)
                system.OpenArsenalUI();
            else
                system.CloseArsenalUI();

            return base.UseItem(player);
        }
    }
}
