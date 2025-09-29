
using CalamityMod;
using CalamityMod.Items.Accessories;
using CalamityMod.NPCs.Crags;
using CalamityMod.Particles;
using CalRemix.Content.Items.Critters;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Items.Misc;
using CalRemix.Content.Items.Placeables.Subworlds.Sealed;
using CalRemix.Content.Items.Potions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Utilities;

namespace CalRemix.Content.Tiles
{
    public class BabySealedPuppetPlaced : ModTile
    {
        public override string Texture => "CalRemix/Content/Items/Materials/BabySealedPuppet";

        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Terraria.ID.TileID.Sets.DisableSmartCursor[Type] = true;
            Main.tileObsidianKill[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.Height = 1;
            TileObjectData.newTile.AnchorBottom = new AnchorData(Terraria.Enums.AnchorType.AlternateTile, TileObjectData.newTile.Width, 0);
            List<int> heightArray = new List<int>(0);
            for (int i = 0; i < 1; i++)
            {
                heightArray.Add(16);
            }
            TileObjectData.newTile.CoordinateHeights = [.. heightArray];
            TileObjectData.addTile(Type);
            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(144, 148, 144), name);
            DustType = DustID.GreenBlood;
        }

        public override void PostSetupTileMerge()
        {
            // Allow plushies to be placed on any sleepable bed
            List<int> beds = new List<int>();
            for (int i = 0; i < TileLoader.TileCount; i++)
            {
                if (TileID.Sets.CanBeSleptIn[i])
                {
                    beds.Add(i);
                }
            }
            int[] bedArray = [.. beds];
            TileObjectData data = TileObjectData.GetTileData(Type, 0);
            data.AnchorAlternateTiles = bedArray;
        }

        public override void MouseOver(int i, int j)
        {
            Player localPlayer = Main.LocalPlayer;
            localPlayer.noThrow = 2;
            localPlayer.cursorItemIconEnabled = true;
            localPlayer.cursorItemIconID = ModContent.ItemType<BabySealedPuppet>();
        }
        public override bool RightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;
            SoundEngine.PlaySound(BetterSoundID.ItemDartPistol, new Vector2(i * 16, j * 16));
            return true;
        }

        public override void HitWire(int i, int j)
        {
            int x = i - Main.tile[i, j].TileFrameX / 18 % 1;
            int y = j - Main.tile[i, j].TileFrameY / 18 % 1;
            NetMessage.SendTileSquare(-1, x + 1, y + 1, 1);
            SoundEngine.PlaySound(BetterSoundID.ItemDartPistol, new Vector2(i * 16, j * 16));
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile t = Framing.GetTileSafely(i, j);
            // Get the top left tile
            int tileX = t.TileFrameX / 18;
            while (tileX >= 4)
            {
                tileX -= 4;
            }
            tileX = i - tileX;
            int tileY = t.TileFrameY / 18;
            while (tileY >= 4)
            {
                tileY -= 4;
            }
            tileY = j - tileY;

            // If the tile beneath can be slept in (aka a bed most of the time), run
            if (Main.tile[tileX, tileY + 1].HasTile && TileID.Sets.CanBeSleptIn[Main.tile[tileX, tileY + 1].TileType])
            {
                // Don't draw for tiles besides the top left
                if (t.TileFrameX == 0 && t.TileFrameY == 0)
                {
                    Texture2D tex = TextureAssets.Item[ModContent.ItemType<BabySealedPuppet>()].Value;
                    spriteBatch.Draw(tex, new Vector2(i, j) * 16 - Main.screenPosition + new Vector2(16, 16 * 1 + 16) + (Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange)), null, Lighting.GetColor(i, j), MathHelper.PiOver4, new Vector2(tex.Width / 2, tex.Height), 1, 0, 0);
                }
                // Regardless of the above, cancel drawing
                return false;
            }
            // If there's no bed, draw normally
            return true;
        }
    }
}