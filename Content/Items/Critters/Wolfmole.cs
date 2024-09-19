using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Critters
{
    public class Wolfmole : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Legendary Wolfmole");
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 24;
            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.White;
            Item.makeNPC = (short)ModContent.NPCType<NPCs.Wolfmole>();
        }
    }
}