using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalRemix.Content.Tiles;

namespace CalRemix.Content.Items.Placeables
{
    [Autoload(false)]
    public class SubworldDoor : ModItem
    {
        protected override bool CloneNewInstances => true;
        public override string Texture => "CalRemix/Content/Items/Placeables/SubworldDoor";
        public override string Name => NameOverride;

        public string NameOverride;

        public int TileID;

        public int PlaceStyle;

        public SubworldDoor(ModTile tile, int placeStyle = -1)
        {
            TileID = tile.Type;
            PlaceStyle = placeStyle;
            NameOverride = tile.Name.Replace("Placed", "");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 32;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.createTile = TileID;
            Item.placeStyle = PlaceStyle == -1 ? Main.rand.Next(0, 3) : PlaceStyle;
        }
    }
}
