using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;
using CalamityMod.NPCs.PlaguebringerGoliath;
using Terraria.Audio;

namespace CalRemix.Content.Tiles.PlaguedJungle
{
    public class PlaguedLarva : ModTile
    {
        public const int Width = 3;
        public const int Height = 4;
        public const int OriginOffsetX = 1;
        public const int OriginOffsetY = 1;
        public const int SheetSquare = 18;
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileCut[Type] = true;
            Main.tileSolid[Type] = false;
            Main.tileBlockLight[Type] = false;
            Main.tileLighted[Type] = false;

            TileObjectData.newTile.Width = 3;
            TileObjectData.newTile.Height = 4;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.Origin = new Point16(1, 2);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, 2, 0);
            TileObjectData.addTile(Type);
            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(41, 119, 55), name);
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            SoundEngine.PlaySound(new SoundStyle("CalamityMod/Sounds/Custom/PlagueSounds/PBGNukeWarning"), Main.LocalPlayer.Center);
            NPC.SpawnOnPlayer(Main.myPlayer, NPCType<PlaguebringerGoliath>());
        }
    }
}
