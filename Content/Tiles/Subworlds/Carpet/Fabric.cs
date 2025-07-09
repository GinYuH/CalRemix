using CalamityMod;
using CalamityMod.Items;
using CalRemix.Content.Items.Placeables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace CalRemix.Content.Tiles.Subworlds.Carpet
{
    public class GreenFabricPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(21, 158, 96), name);
            HitSound = BetterSoundID.ItemIceHit with { Pitch = -0.6f };
            DustType = DustID.DungeonGreen;
        }
    }

    public class GreenFabric : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 14;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<GreenFabricPlaced>();
            Item.width = 12;
            Item.height = 12;
            Item.value = CalamityGlobalItem.RarityWhiteBuyPrice;
            Item.rare = ItemRarityID.White;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<GreenFabricWall>(4)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
