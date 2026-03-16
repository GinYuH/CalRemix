using CalamityMod.CalPlayer;
using CalamityMod.Items.Accessories;
using CalRemix.Content.Items.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public class AcroToggle : BuilderToggle
    {
        public static LocalizedText OnText { get; private set; }
        public static LocalizedText OffText { get; private set; }

        public override string Texture => "CalRemix/UI/AcroToggle";
        public override string HoverTexture => Texture;

        public override bool Active() => Main.LocalPlayer.Remix().wapUnlocked;
        public override int NumberOfStates => 2;
        public override void SetStaticDefaults()
        {
            OnText = this.GetLocalization(nameof(OnText));
            OffText = this.GetLocalization(nameof(OffText));
        }

        public override string DisplayValue()
        {
            return CurrentState == 0 ? OnText.Value : OffText.Value;
        }
        public override bool DrawHover(SpriteBatch spriteBatch, ref BuilderToggleDrawParams drawParams)
        {
            int column = CurrentState == 0 ? 1 : 0;
            drawParams.Frame = drawParams.Texture.Frame(2, 2, column, 1);
            return true;
        }

        public override bool Draw(SpriteBatch spriteBatch, ref BuilderToggleDrawParams drawParams)
        {
            int column = CurrentState == 0 ? 1 : 0;
            drawParams.Frame = drawParams.Texture.Frame(2, 2, column, 0);
            return true;
        }
    }
}