using CalamityMod;
using CalRemix.Content.Items.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CalRemix.Content.Tiles
{
    public class AltaroftheAblessedPlaced : ModTile
    {
        public const int Width = 2;
        public const int Height = 3;
        public const int OriginOffsetX = 1;
        public const int OriginOffsetY = 1;
        public const int SheetSquare = 18;
        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.PreventsTileRemovalIfOnTopOfIt[Type] = true;
            TileID.Sets.PreventsTileReplaceIfOnTopOfIt[Type] = true;
            TileID.Sets.PreventsSandfall[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;

            TileObjectData.newTile.Width = 2;
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.Origin = new Point16(1, 2);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, 2, 0);
            TileObjectData.addTile(Type);
            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(255, 0, 0), name);
        }
        public override bool HasSmartInteract(int i, int j, Terraria.GameContent.ObjectInteractions.SmartInteractScanSettings settings) => true;

        public override bool RightClick(int i, int j)
        {
            if (false)
            {
                return true;
            }
            return false;
        }
        public override void MouseOver(int i, int j)
        {
            Player localPlayer = Main.LocalPlayer;
            localPlayer.noThrow = 2;
            localPlayer.cursorItemIconEnabled = true;
            localPlayer.cursorItemIconID = ModContent.ItemType<FocusedConvergence>();            
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            frameCounter++;
            if (frameCounter > 4)
            {
                frameCounter = 0;
                frame++;
            }
            if (frame > 27)
                frame = 0;
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D rune = ModContent.Request<Texture2D>("CalRemix/Content/Tiles/AltaroftheAblessedRune").Value;
            Vector2 posOffset = new Vector2(16, MathF.Sin(Main.GlobalTimeWrappedHourly * 3) * 5 + 6);
            if (Main.tile[i, j].TileFrameX == 0 && Main.tile[i, j].TileFrameY == 0)
                spriteBatch.Draw(rune, new Vector2(i * 16, j * 16) - Main.screenPosition + CalamityUtils.TileDrawOffset + posOffset, rune.Frame(1, 28, 0, Main.tileFrame[Type]), Color.White, 0, new Vector2(rune.Width / 2, rune.Height / 28), 1f, SpriteEffects.None, 0);
            return true;
        }

        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return DownedBossSystem.downedCryogen;
        }

        public override bool CanExplode(int i, int j)
        {
            return DownedBossSystem.downedCryogen;
        }
    }
}
