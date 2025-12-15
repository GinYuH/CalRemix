using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CalRemix.Content.Tiles.TVs;

/// <summary>
/// Modern flatscreen TV - 8x5 tiles.
/// </summary>
public class FlatscreenTVTile : BaseTVTile
{
    public override Point GetTVDimensions() => new(8, 5);

    public override string GetTVStyleName() => "Flatscreen TV";

    public override void SetStaticDefaults()
    {
        Point dimensions = GetTVDimensions();

        Main.tileFrameImportant[Type] = true;
        Main.tileNoAttach[Type] = true;
        Main.tileLavaDeath[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
        TileObjectData.newTile.Width = dimensions.X;
        TileObjectData.newTile.Height = dimensions.Y;
        TileObjectData.newTile.CoordinateHeights = new int[dimensions.Y];
        for (int i = 0; i < dimensions.Y; i++)
        {
            TileObjectData.newTile.CoordinateHeights[i] = 16;
        }
        TileObjectData.newTile.CoordinateWidth = 16;
        TileObjectData.newTile.CoordinatePadding = 0;
        TileObjectData.newTile.Origin = new Point16(0, dimensions.Y - 1);
        TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
        TileObjectData.newTile.UsesCustomCanPlace = true;
        TileObjectData.newTile.LavaDeath = true;

        ModTileEntity te = ModContent.GetInstance<TVTileEntity>();
        TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(te.Hook_AfterPlacement, -1, 0, true);

        TileObjectData.addTile(Type);

        AddMapEntry(new Color(100, 100, 100), CreateMapEntryName());

        DustType = DustID.Iron;

        TVTileEntity.TileData[Type] = (GetTVDimensions(), new Rectangle(4, 4, -4, -14));
    }   
}

public class FlatscreenTVItem : ModItem
{
    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 1;
    }

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
        Item.value = Item.buyPrice(1, 0, 0, 0);
        Item.rare = ItemRarityID.Green;
        Item.createTile = ModContent.TileType<FlatscreenTVTile>();
    }
}
