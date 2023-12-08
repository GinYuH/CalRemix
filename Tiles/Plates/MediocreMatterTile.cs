using CalamityMod;
using CalRemix.Items.Placeables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace CalRemix.Tiles.Plates
{
    public class MediocreMatterTile : ModTile
    {
        public static readonly SoundStyle MinePlatingSound = new SoundStyle("CalamityMod/Sounds/Custom/PlatingMine", 3, SoundType.Sound);

        public static Texture2D GlowTexture;
        public static Texture2D PulseTexture;
        public static Color[] PulseColors;

        public override void SetStaticDefaults()
        {
            if (!Main.dedServ)
            {
                PulseTexture = Request<Texture2D>("CalRemix/Tiles/Plates/MediocreMatterTilePulse", (AssetRequestMode)1).Value;
                PulseColors = (Color[])(object)new Color[PulseTexture.Width];
                Main.QueueMainThreadAction((Action)delegate
                {
                    PulseTexture.GetData<Color>(PulseColors);
                });
                GlowTexture = Request<Texture2D>("CalRemix/Tiles/Plates/MediocreMatterTileGlow", (AssetRequestMode)1).Value;
            }
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            CalamityUtils.MergeWithGeneral(Type);
            HitSound = MinePlatingSound;
            MineResist = 1f;
            AddMapEntry(new Color(77, 57, 70), null);
        }

        public override bool CreateDust(int i, int j, ref int type)
        {
            Dust.NewDust(new Vector2(i, j) * 16f, 16, 16, 6, 0f, 0f, 1, new Color(255, 255, 255), 1f);
            Dust.NewDust(new Vector2(i, j) * 16f, 16, 16, 1, 0f, 0f, 1, new Color(51, 12, 8), 1f);
            return false;
        }

        public override void RandomUpdate(int i, int j)
        {
            Dust.NewDust(new Vector2(i, j) * 16f, 16, 16, 6, 0f, 0f, 1, new Color(255, 255, 255), 1f);
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if (GlowTexture != null && PulseTexture != null)
            {
                Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
                Vector2 drawOffset = new Vector2((i * 16) - Main.screenPosition.X, (j * 16) - Main.screenPosition.Y) + zero;
                int factor = (int)Main.GameUpdateCount % PulseTexture.Width;
                float brightness = PulseColors[factor].R / 255f;
                int drawBrightness = (int)(40f * brightness) + 10;
                Color drawColour = GetDrawColour(i, j, new Color(drawBrightness, drawBrightness, drawBrightness, drawBrightness));
                CalRemixTile.SlopedGlowmask(i, j, 0, GlowTexture, drawOffset, null, GetDrawColour(i, j, drawColour), default);
            }
        }

        private Color GetDrawColour(int i, int j, Color colour)
        {
            Tile tile = Main.tile[i, j];
            int colType = tile.TileColor;
            Color paintCol = WorldGen.paintColor(colType);
            if (colType >= 13 && colType <= 24)
            {
                colour.R = (byte)(paintCol.R / 255f * colour.R);
                colour.G = (byte)(paintCol.G / 255f * colour.G);
                colour.B = (byte)(paintCol.B / 255f * colour.B);
            }
            return colour;
        }
    }
}