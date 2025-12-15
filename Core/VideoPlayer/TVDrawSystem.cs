using CalRemix.Content.Tiles.TVs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalRemix.Core.VideoPlayer;

public class TVDrawSystem : ModSystem
{
    private static List<(Rectangle ScreenArea, TVTileEntity Entity)> _tvsToRender = new();

    public static void RegisterTVForDrawing(Rectangle screenArea, TVTileEntity entity)
    {
        _tvsToRender.Add((screenArea, entity));
    }

    public override void UpdateUI(GameTime gameTime)
    {
        // Just collect the TV entities, don't calculate positions yet
        _tvsToRender.Clear();

        foreach (var kvp in TileEntity.ByID)
        {
            if (kvp.Value is TVTileEntity tvEntity && tvEntity.IsOn)
            {
                _tvsToRender.Add((default, tvEntity)); // Placeholder rectangle
            }
        }
    }

    public override void PostDrawTiles()
    {
        if (_tvsToRender.Count == 0)
            return;

        SpriteBatch spriteBatch = Main.spriteBatch;
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone);

        foreach (var (_, tvEntity) in _tvsToRender)
        {
            var (position, size) = CalculateScreenArea(tvEntity);
            tvEntity.DrawVideoContent(spriteBatch, position, size);
        }

        spriteBatch.End();

        _tvsToRender.Clear();
    }

    private static (Vector2 Position, Vector2 Size) CalculateScreenArea(TVTileEntity tvEntity)
    {
        int tileType = Main.tile[tvEntity.Position.X, tvEntity.Position.Y].TileType;

        if (!TVTileEntity.TileData.TryGetValue(tileType, out var tileInfo))
            return (Vector2.Zero, Vector2.Zero);

        Rectangle worldArea = new Rectangle(
            tvEntity.Position.X * 16,
            tvEntity.Position.Y * 16,
            tileInfo.TileSize.X * 16,
            tileInfo.TileSize.Y * 16
        );

        worldArea.X += tileInfo.ScreenOffsets.X;
        worldArea.Y += tileInfo.ScreenOffsets.Y;
        worldArea.Width -= tileInfo.ScreenOffsets.X;
        worldArea.Height -= tileInfo.ScreenOffsets.Y;
        worldArea.Width += tileInfo.ScreenOffsets.Width;
        worldArea.Height += tileInfo.ScreenOffsets.Height;

        Vector2 worldPos = new Vector2(worldArea.X, worldArea.Y);
        Vector2 screenPos = worldPos - Main.screenPosition;

        // Use the actual zoom from the GameViewMatrix that's being used for rendering
        float zoom = Main.GameViewMatrix.Zoom.X; // This is the actual current render zoom
        Vector2 screenCenter = new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f);

        Vector2 finalPos = (screenPos - screenCenter) * zoom + screenCenter;
        Vector2 finalSize = new Vector2(worldArea.Width, worldArea.Height) * zoom;

        return (finalPos, finalSize);
    }
}
