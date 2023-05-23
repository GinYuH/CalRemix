using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Tiles;

namespace CalRemix.Items.Placeables
{
    public class DerellectMusicBox : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("Music Box (Signal Interruption)");
            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Sounds/Music/signal interruption"), ModContent.ItemType<DerellectMusicBox>(), ModContent.TileType<DerellectMusicBoxPlaced>());
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<DerellectMusicBoxPlaced>();
            Item.width = 24;
            Item.height = 24;
            Item.rare = ItemRarityID.LightRed;
            Item.value = 100000;
            Item.accessory = true;
        }
    }
}
