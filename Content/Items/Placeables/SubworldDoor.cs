using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalRemix.Content.Tiles;
using CalRemix.Content.Items.Placeables.Plates;

namespace CalRemix.Content.Items.Placeables
{
    public class SubworldDoor : ModItem
    {
        public override string Texture => "CalRemix/Assets/ExtraTextures/SludgeCannon";

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 32;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<ExosphereDoor>();
        }
    }
}
