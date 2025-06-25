using Terraria.ModLoader;
using Terraria.ID;
using CalRemix.Content.Tiles;
using CalRemix;

namespace CalRemix.Content.Items.Placeables
{
    public class TrapperQuestMachine : ModItem
    {
        public override string Texture => "CalRemix/Content/Items/Placeables/BoiMachine";
        public override void SetDefaults()
        {
            Item.useStyle = 1;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<TrapperQuestMachinePlaced>();
            Item.maxStack = 99;
            Item.width = 48;
            Item.height = 32;
            Item.rare = ItemRarityID.LightRed;
        }
    }
}