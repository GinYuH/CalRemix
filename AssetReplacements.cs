using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Graphics;

using ReLogic.Content;

using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix;

[Autoload(Side = ModSide.Client)]
public class AssetReplacements : ModSystem
{
    private delegate ref Asset<T> AssetProvider<T>() where T : class;

    private readonly struct Replacement<T> : IDisposable where T : class
    {
        private readonly AssetProvider<T> provider;
        private readonly Asset<T>         original;

        public Replacement(AssetProvider<T> provider, Asset<T> replacement)
        {
            this.provider = provider;
            original      = provider();
            provider()    = replacement;
        }

        public void Dispose()
        {
            provider() = original;
        }
    }

    private static readonly List<IDisposable> replacements = [];

    public override void Load()
    {
        base.Load();

        replacements.Add(
            new Replacement<Texture2D>(
                () => ref TextureAssets.Tile[TileID.Amethyst],
                ModContent.Request<Texture2D>("CalRemix/Assets/Textures/AmethystTile")
            )
        );
    }

    public override void Unload()
    {
        base.Unload();

        foreach (var replacement in replacements)
        {
            replacement.Dispose();
        }
    }
}