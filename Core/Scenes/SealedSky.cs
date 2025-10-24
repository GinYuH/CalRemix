using System;
using CalRemix.Content.NPCs.Subworlds.Sealed;
using CalRemix.Core.Biomes;
using CalRemix.Core.Subworlds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SubworldLibrary;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Bosses.Carcinogen
{
    public class SealedSky : CustomSky
    {
        public float BackgroundIntensity;
        public static bool CanSkyBeActive
        {
            get
            {
                return SubworldSystem.IsActive<SealedSubworld>();
            }
        }

        public static Color ChooseSealedColor(Player p)
        {
            if (Filters.Scene["CalRemix:VoidColors"].IsActive())
                return Color.Black;
            if (NPC.AnyNPCs(ModContent.NPCType<Disilphia>()))
                return Color.Brown;
            if (p.InModBiome<UnsealedSeaBiome>())
            {
                return Color.Gray;
            }
            else if (p.InModBiome<DarnwoodSwampBiome>())
            {
                return Color.DarkOliveGreen;
            }
            else if (p.InModBiome<VoidForestBiome>())
            {
                return Color.White with { A = 0 };
            }
            if (p.InModBiome<TurnipBiome>())
            {
                return Color.MediumPurple;
            }
            else if (p.InModBiome<VolcanicFieldBiome>())
            {
                return Color.OrangeRed;
            }
            else if (p.InModBiome<CarnelianForestBiome>())
            {
                return Color.Red;
            }
            else if (p.InModBiome<BadlandsBiome>())
            {
                return Color.MediumPurple;
            }
            else if (p.InModBiome<BarrensBiome>())
            {
                return Color.Gray;
            }
            else if (p.InModBiome<SealedFieldsBiome>())
            {
                return Color.Purple;
            }
            else
                return Color.Purple;
        }

        public static Color current = new();

        public override void Update(GameTime gameTime)
        {
            if (!CanSkyBeActive)
            {
                BackgroundIntensity = MathHelper.Clamp(BackgroundIntensity - 0.08f, 0f, 1f);
                Deactivate(Array.Empty<object>());
                return;
            }

            BackgroundIntensity = MathHelper.Clamp(BackgroundIntensity + 0.01f, 0f, 1f);

            Opacity = BackgroundIntensity;

            current = Color.Lerp(current, ChooseSealedColor(Main.LocalPlayer), 0.01f);
            if (Main.LocalPlayer.InModBiome<BarrensBiome>() && !Main.LocalPlayer.InModBiome<VolcanicFieldBiome>() && !Main.LocalPlayer.InModBiome<BadlandsBiome>())
            {
                current = Color.Gray;
            }
        }
        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (minDepth < 0)
            {
                float mult = NPC.AnyNPCs(ModContent.NPCType<Disilphia>()) ? 1.5f : 0.6f;
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(0, 0, (int)Main.ScreenSize.X, (int)Main.ScreenSize.Y), current * mult);
            }
        }

        public override Color OnTileColor(Color inColor) => inColor;

        public override float GetCloudAlpha() => 0f;

        public override void Reset() { }

        public override void Activate(Vector2 position, params object[] args) { }

        public override void Deactivate(params object[] args) { }

        public override bool IsActive() => CanSkyBeActive && !Main.gameMenu;
    }
}
