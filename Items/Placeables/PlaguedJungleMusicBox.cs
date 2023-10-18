using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Utilities;
using CalRemix;
using Terraria;

namespace CalRemix.Items.Placeables
{
	public class PlaguedJungleMusicBox : ModItem
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Music Box (Plagued Jungle)");
            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Sounds/Music/PlaguedJungle"), Type, ModContent.TileType<Tiles.PlaguedJungleMusicBox>());
        }

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.PlaguedJungleMusicBox>();
			Item.width = 24;
			Item.height = 24;
			Item.rare = ItemRarityID.LightRed;
			Item.value = 100000;
			Item.accessory = true;
		}

		public override bool? PrefixChance(int pre, UnifiedRandom rand)
		{
			if (this != null)
			{
				return false;
			}
			else
			{
				return true;
			}
		}
	}
}