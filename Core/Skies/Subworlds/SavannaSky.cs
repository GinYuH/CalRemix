using System;
using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SubworldLibrary;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using static Terraria.Main;

namespace CalRemix.Core.Subworlds
{
    public class SavannaSky : CustomSky
    {
        public static Asset<Texture2D> bg = null;

        public override void OnLoad()
        {
            if (!Main.dedServ)
                bg = ModContent.Request<Texture2D>("CalRemix/Core/Backgrounds/Subworlds/Savanna");
        }

        public static bool CanSkyBeActive
        {
            get
            {
                return SubworldSystem.Current == ModContent.GetInstance<SavannaSubworld>();
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (!CanSkyBeActive)
            {
                Deactivate(Array.Empty<object>());
                return;
            }

            Opacity = 1;

        }
        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            Vector2 max = new Vector2(maxTilesX * 16, maxTilesY * 16);
            Vector2 lowered = max * new Vector2(0.7f, 0.4f);
            spriteBatch.Draw(bg.Value, Vector2.Zero - screenPosition + (max - lowered) / 2f - Vector2.UnitY * 20, null, Color.White, 0, Vector2.Zero, lowered / bg.Value.Size(), 0, 0);
        }

        public override Color OnTileColor(Color inColor) => inColor;

        public override float GetCloudAlpha() => 0f;

        public override void Reset() { }

        public override void Activate(Vector2 position, params object[] args) { }

        public override void Deactivate(params object[] args) { }

        public override bool IsActive() => CanSkyBeActive && !Main.gameMenu;
    }
}
