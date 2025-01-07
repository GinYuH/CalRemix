using CalRemix.UI.Games.Boi;
using CalRemix.Content.Items.Placeables;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CalRemix.Content.Tiles
{
    public class BoiMachinePlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Terraria.ID.TileID.Sets.DisableSmartCursor[Type] = true;
            Main.tileLavaDeath[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.Width = 3;
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(0, 118, 49));
        }

        public override bool RightClick(int i, int j)
        {
            CalRemixPlayer modPlayer = Main.LocalPlayer.GetModPlayer<CalRemixPlayer>();
            //if (modPlayer.boiactive == false)
            {
                //modPlayer.boihealth = 3;
                modPlayer.boiactive = true;
            }
            return true;
        }

        public override void MouseOver(int i, int j)
        {
            Player localPlayer = Main.LocalPlayer;
            localPlayer.noThrow = 2;
            localPlayer.cursorItemIconEnabled = true;
            localPlayer.cursorItemIconID = ModContent.ItemType<BoiMachine>();
        }
    }
}