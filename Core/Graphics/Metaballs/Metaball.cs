using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;

namespace CalRemix.Core.Graphics
{
    public abstract class Metaball
    {
        internal List<ManagedRenderTarget> LayerTargets = new();

        public abstract List<Asset<Texture2D>> Layers
        {
            get;
        }

        public abstract MetaballDrawLayerType DrawContext
        {
            get;
        }

        public abstract Color EdgeColor
        {
            get;
        }

        public Metaball()
        {
            Main.QueueMainThreadAction(() =>
            {
                for (int i = 0; i < Layers.Count; i++)
                    LayerTargets.Add(new(true, RenderTargetManager.CreateScreenSizedTarget));
            });
        }

        public void Dispose()
        {
            for (int i = 0; i < LayerTargets.Count; i++)
                LayerTargets[i]?.Dispose();
        }

        public virtual void Update() { }

        public virtual bool PrepareSpriteBatch(SpriteBatch spriteBatch) => false;

        public abstract void DrawInstances();
    }
}
