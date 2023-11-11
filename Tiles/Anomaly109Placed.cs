using CalRemix.Buffs;
using CalRemix.Items.Placeables;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CalRemix.Tiles
{
    public class Anomaly109Placed : ModTile
    {
        public const int Width = 3;
        public const int Height = 3;
        public const int OriginOffsetX = 1;
        public const int OriginOffsetY = 1;
        public const int SheetSquare = 18;
        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;

            TileObjectData.newTile.Width = 3;
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.Origin = new Point16(0, 3);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, 2, 0); 
            ModTileEntity te = ModContent.GetInstance<Anomaly109TileEntity>();
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(te.Hook_AfterPlacement, -1, 0, true);
            TileObjectData.addTile(Type);
            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(255, 0, 0), name);
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Tile t = Main.tile[i, j];
            int left = i - t.TileFrameX % (Width * SheetSquare) / SheetSquare;
            int top = j - t.TileFrameY % (Height * SheetSquare) / SheetSquare;

            Anomaly109TileEntity charger = CalamityMod.CalamityUtils.FindTileEntity<Anomaly109TileEntity>(i, j, Width, Height, SheetSquare);

            charger?.Kill(left, top);
        }
    }
}
