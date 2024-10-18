using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using ReLogic.Content;
using Terraria;
using System.Diagnostics.CodeAnalysis;

namespace CalRemix.Core
{
    public class CustomGen(int texture, Color color, bool glow, bool visible, int texture2, Color color2, bool glow2, bool visible2)
    {
        public int CoreTexture { get; set; } = texture;
        public Color CoreColor { get; set; } = color;
        public bool CoreGlow { get; set; } = glow;
        public bool CoreVisible { get; set; } = visible;
        public int ShieldTexture { get; set; } = texture2;
        public Color ShieldColor { get; set; } = color2;
        public bool ShieldGlow { get; set; } = glow2;
        public bool ShieldVisible { get; set; } = visible2;

        private static readonly string Path = "CalRemix/Assets/ExtraTextures/GenParts/";
        internal static List<string> TextureList =
        [
            "Origen",
            "Carcinogen",
            "Phytogen",
            "Hydrogen",
            "Oxygen",
            "Ionogen",
            "Pathogen",
            "Pyrogen"
        ];

        public override bool Equals([NotNullWhen(true)] object obj)
        {
            CustomGen o = (CustomGen)obj;
            return CoreTexture == o.CoreTexture && CoreColor == o.CoreColor && CoreGlow == o.CoreGlow && CoreVisible == o.CoreVisible && ShieldTexture == o.ShieldTexture && ShieldColor == o.ShieldColor && ShieldGlow == o.ShieldGlow && ShieldVisible == o.ShieldVisible;
        }
        public override int GetHashCode()
        {
            throw new System.NotImplementedException();
        }
        public static Texture2D GetTexture2D(string type, int texture, bool glow)
        {
            string path = Path;
            if (texture < 0 || texture >= TextureList.Count)
                path += TextureList[0];
            else
                path += TextureList[texture];
            path += type;
            Texture2D baseTexture = Request<Texture2D>(path).Value;
            if (!glow)
                return baseTexture;
            string glowPath = path + (glow ? "Glow" : string.Empty);
            return RequestIfExists(glowPath, out Asset<Texture2D> t) ? t.Value : Request<Texture2D>("CalRemix/Assets/ExtraTextures/Blank").Value;
        }
        public static void Copy(CustomGen from, CustomGen to)
        {
            to.CoreTexture = from.CoreTexture;
            to.CoreColor = from.CoreColor;
            to.CoreGlow = from.CoreGlow;
            to.CoreVisible = from.CoreVisible;

            to.ShieldTexture = from.ShieldTexture;
            to.ShieldColor = from.ShieldColor;
            to.ShieldGlow = from.ShieldGlow;
            to.ShieldVisible = from.ShieldVisible;
        }
        public static void DrawGenToSpritebatch(SpriteBatch spritebatch, CustomGen gen, Vector2 position)
        {
            float rotation = MathHelper.TwoPi / 2 * (Main.GlobalTimeWrappedHourly % 2);

            Texture2D shield = GetTexture2D("Shield", gen.ShieldTexture, false);
            Texture2D core = GetTexture2D(string.Empty, gen.CoreTexture, false);
            Texture2D shieldGlow = GetTexture2D("Shield", gen.ShieldTexture, true);
            Texture2D coreGlow = GetTexture2D(string.Empty, gen.CoreTexture, true);

            if (gen.ShieldVisible)
            {
                spritebatch.Draw(shield, position, null, gen.ShieldColor, rotation, shield.Size() * 0.5f, 1f, SpriteEffects.None, 0);
                if (gen.ShieldGlow)
                    spritebatch.Draw(shieldGlow, position, null, Color.White, rotation, shield.Size() * 0.5f, 1f, SpriteEffects.None, 0);
            }
            if (gen.CoreVisible)
            {
                spritebatch.Draw(core, position, null, gen.CoreColor, 0f, core.Size() * 0.5f, 1f, SpriteEffects.None, 0);
                if (gen.CoreGlow)
                    spritebatch.Draw(coreGlow, position, null, Color.White, 0f, core.Size() * 0.5f, 1f, SpriteEffects.None, 0);
            }
        }
    }
}
