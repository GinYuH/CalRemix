using CalRemix.Core.Subworlds;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SubworldLibrary;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Capture;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace CalRemix.Core.Biomes.Subworlds
{
    public class ClownWorldClownBiome : ModBiome
    {
        //public override bool IsPrimaryBiome =>false; // Allows this biome to impact NPC prices
        //public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.Find<ModSurfaceBackgroundStyle>("CalRemix/ProfanedDesertBackground");
        public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;

        // Populate the Bestiary Filter
        public override string BestiaryIcon => "CalRemix/Core/Biomes/AsbestosIcon";
        public override string BackgroundPath => "Terraria/Images/MapBG11";
        public override Color? BackgroundColor => Color.Orange;

        public override string MapBackground => BackgroundPath;

        public override void MapBackgroundColor(ref Color color) => color = BackgroundColor.Value;

        // Use SetStaticDefaults to assign the display name
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Asbestos Caves");
        }

        // Calculate when the biome is active.
        public override bool IsBiomeActive(Player player)
        {
            return SubworldSystem.IsActive<ClownWorldSubworld>();
        }

        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;

        public override int Music => CalRemixMusic.ClownWorld;
    }

    public class ClownWorldSky : CustomSky
    {
        public float BackgroundIntensity;
        public static bool CanSkyBeActive
        {
            get
            {
                return SubworldSystem.IsActive<ClownWorldSubworld>();
            }
        }

        public static readonly Color DrawColor = Color.White;
        public int offset = 0;

        public override void Update(GameTime gameTime)
        {
            if (!CanSkyBeActive)
            {
                BackgroundIntensity = MathHelper.Clamp(BackgroundIntensity - 0.08f, 0f, 1f);
                Deactivate(Array.Empty<object>());
                return;
            }

            Opacity = 1;
            offset++;
            if (offset > Main.screenWidth / 10)
                offset = 0;
        }
        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            Texture2D face = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Subworlds/ScreamingMummifiedFace").Value;
            //if (maxDepth >= float.MaxValue)
            {
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, Vector2.Zero, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
                bool flip = false;
                int height = 0;
                int xPos = 0;
                for (int i = 0; i < 201; i++)
                {
                    spriteBatch.Draw(face, Vector2.Zero + new Vector2(xPos + offset * flip.ToDirectionInt() - 100, height), null, Color.White * 0.6f, 0, face.Size() / 2, 1, flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                    for (int j = 0; j < 4; j++)
                    {
                        Vector2 dp = (Vector2.Zero + new Vector2(xPos + offset * flip.ToDirectionInt() - 100, height)) + Vector2.UnitY.RotatedBy(MathHelper.Lerp(0, MathHelper.TwoPi, j / 4f)) * 8 * MathF.Sin(Main.GlobalTimeWrappedHourly);
                        spriteBatch.Draw(face, dp, null, Color.White * 0.1f, 0, face.Size() / 2, 1, flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                    }
                    xPos += Main.screenWidth / 10;
                    if (i % 15 == 0 && i > 0)
                    {
                        flip = !flip;
                        height += Main.screenHeight / 5;
                        xPos = 0;
                    }
                }
            }
        }

        public override float GetCloudAlpha() => 0f;

        public override void Reset() { }

        public override void Activate(Vector2 position, params object[] args) { }

        public override void Deactivate(params object[] args) { }

        public override bool IsActive() => CanSkyBeActive && !Main.gameMenu;
    }
}
