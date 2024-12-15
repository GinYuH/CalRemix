using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalRemix.Content.Tiles.Trophies;
using static Terraria.ModLoader.ModContent;

namespace CalRemix.Content.Items.Placeables.Trophies
{
    public class IonogenTrophy : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemType<OldIonogenTrophy>();
        }
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.maxStack = 99;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<IonogenTrophyPlaced>();
            Item.width = 12;
            Item.height = 12;
            Item.rare = ItemRarityID.Blue;
        }
    }
}