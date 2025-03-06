using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CalRemix.Content.Items;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using CalRemix.Content.Items.Placeables.Relics;

namespace CalRemix.Content.Tiles.Relics
{
    public class NoxusRelicTile : ModTile
    {
        public const int FrameWidth = 18 * 3;

        public const int FrameHeight = 18 * 4;

        public Asset<Texture2D> RelicTexture;

        public virtual string RelicTextureName => "CalRemix/Content/Tiles/Relics/NoxusRelicTile";

        public override string Texture => $"Terraria/Images/Tiles_{TileID.MasterTrophyBase}";

        public override void Load()
        {
            if (!Main.dedServ)
            {
                // Cache the extra texture displayed on the pedestal.
                RelicTexture = ModContent.Request<Texture2D>(RelicTextureName);
            }
        }

        public override void Unload()
        {
            // Unload the extra texture displayed on the pedestal.
            RelicTexture = null;
        }

        public override void SetStaticDefaults()
        {
            Main.tileShine[Type] = 400; // Responsible for golden particles.
            Main.tileFrameImportant[Type] = true; // Any multitile requires this.
            TileID.Sets.InteractibleByNPCs[Type] = true; // Town NPCs will palm their hand at this tile.

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4); // Relics are 3x4.
            TileObjectData.newTile.LavaDeath = false; // Does not break when lava touches it.
            TileObjectData.newTile.DrawYOffset = 2; // So the tile sinks into the ground.
            TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft; // Player faces to the left.
            TileObjectData.newTile.StyleHorizontal = false; // Based on how the alternate sprites are positioned on the sprite (by default, true).

            TileObjectData.newTile.StyleWrapLimitVisualOverride = 2;
            TileObjectData.newTile.StyleMultiplier = 2;
            TileObjectData.newTile.StyleWrapLimit = 2;
            TileObjectData.newTile.styleLineSkipVisualOverride = 0;

            // Register an alternate tile data with flipped direction.
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
            TileObjectData.addAlternate(1);

            // Register the tile data itself
            TileObjectData.addTile(Type);

            // Register map name and color
            // "MapObject.Relic" refers to the translation key for the vanilla "Relic" text.
            AddMapEntry(new Color(233, 207, 94), Language.GetText("MapObject.Relic"));

            RegisterItemDrop(ModContent.ItemType<NoxusRelic>());
        }

        public override bool CreateDust(int i, int j, ref int type) => false;

        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
        {
            tileFrameX %= FrameWidth;
            tileFrameY %= FrameHeight * 2;
        }

        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {
            // Since this tile does not have the hovering part on its sheet, it must be animated manually.
            // Therefore we register the top-left of the tile as a "special point".
            // This allows us to draw things in the SpecialDraw hook.
            if (drawData.tileFrameX % FrameWidth == 0 && drawData.tileFrameY % FrameHeight == 0)
                Main.instance.TilesRenderer.AddSpecialLegacyPoint(i, j);
        }

        public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Vector2 offScreen = new(Main.offScreenRange);
            if (Main.drawToScreen)
                offScreen = Vector2.Zero;

            // Take the tile, check if it actually exists.
            Point p = new(i, j);
            Tile tile = Main.tile[p.X, p.Y];
            if (tile == null || !tile.HasTile)
                return;

            // Get the initial draw parameters
            Texture2D texture = RelicTexture.Value;

            int frameY = tile.TileFrameX / FrameWidth; // Picks the frame on the sheet based on the placeStyle of the item.
            Rectangle frame = texture.Frame(1, 1, 0, frameY);

            Vector2 origin = frame.Size() / 2f;
            Vector2 worldPos = p.ToWorldCoordinates(24f, 64f);

            Color color = Lighting.GetColor(p.X, p.Y);

            bool direction = tile.TileFrameY / FrameHeight != 0; // This is related to the alternate tile data we registered before.
            SpriteEffects effects = direction ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            // Some math magic to make it smoothly move up and down over time.
            float offset = (float)Math.Sin(Main.GlobalTimeWrappedHourly * MathHelper.TwoPi / 5f);
            Vector2 drawPos = worldPos + offScreen - Main.screenPosition + new Vector2(0f, -40f) + new Vector2(0f, offset * 4f);

            // Draw the main texture.
            spriteBatch.Draw(texture, drawPos, frame, color, 0f, origin, 1f, effects, 0f);

            // Draw the periodic glow effect.
            float scale = MathF.Sin(Main.GlobalTimeWrappedHourly * MathHelper.TwoPi / 2f) * 0.3f + 0.7f;
            Color effectColor = color;
            effectColor.A = 0;
            effectColor = effectColor * 0.1f * scale;
            for (float offsetAngle = 0f; offsetAngle < 1f; offsetAngle += 0.1666f)
                spriteBatch.Draw(texture, drawPos + (MathHelper.TwoPi * offsetAngle).ToRotationVector2() * (6f + offset * 2f), frame, effectColor, 0f, origin, 1f, effects, 0f);
        }
    }
}
