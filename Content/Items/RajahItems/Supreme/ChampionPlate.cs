using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.RajahItems.Supreme
{
    public class ChampionPlate : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Champion Plate");
            // Tooltip.SetDefault("");
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
			Item.maxStack = 99;
            Item.rare = ItemRarityID.Purple;
        }

        public override void ModifyTooltips(System.Collections.Generic.List<TooltipLine> list)
        {
            foreach (TooltipLine line2 in list)
            {
                if (line2.Mod == "Terraria" && line2.Name == "ItemName")
                {
                    line2.OverrideColor = new Color(255, 22, 0);
                }
            }
        }

    }
}
