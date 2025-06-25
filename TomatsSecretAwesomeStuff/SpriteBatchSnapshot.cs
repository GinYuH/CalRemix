using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CalRemix;

internal struct SpriteBatchSnapshot(SpriteBatch spriteBatch)
{
    public SpriteSortMode SortMode { get; set; } = spriteBatch.sortMode;

    public BlendState BlendState { get; set; } = spriteBatch.blendState;

    public SamplerState SamplerState { get; set; } = spriteBatch.samplerState;

    public DepthStencilState DepthStencilState { get; set; } = spriteBatch.depthStencilState;

    public RasterizerState RasterizerState { get; set; } = spriteBatch.rasterizerState;

    public Effect? Effect { get; set; } = spriteBatch.customEffect;

    public Matrix Matrix { get; set; } = spriteBatch.transformMatrix;
}

internal static class SpriteBatchSnapshotExtensions
{
    public static void End(this SpriteBatch sb, out SpriteBatchSnapshot snapshot)
    {
        snapshot = new SpriteBatchSnapshot(sb);
        sb.End();
    }

    public static void Begin(this SpriteBatch sb, in SpriteBatchSnapshot snapshot)
    {
        sb.Begin(
            snapshot.SortMode,
            snapshot.BlendState,
            snapshot.SamplerState,
            snapshot.DepthStencilState,
            snapshot.RasterizerState,
            snapshot.Effect,
            snapshot.Matrix
        );
    }
}