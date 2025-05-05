using System.Collections.Generic;
using Terraria;
using SubworldLibrary;
using Terraria.WorldBuilding;
using Terraria.IO;
using Terraria.ModLoader;
using CalamityMod.Tiles.FurnitureExo;
using System;
using CalamityMod.Tiles.Ores;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Terraria.GameContent.Generation;
using CalRemix.Core.World;

namespace CalRemix.Core.Subworlds
{
    public class NormalSubworld : Subworld
    {
        public override int Height => 2222;
        public override int Width => 8222;
        public override List<GenPass> Tasks => new List<GenPass>()
        {
            new PassLegacy("NormalGen", (progress, config) =>
            {
                GenerationProgress cache = WorldGenerator.CurrentGenerationProgress;
                WorldGen.GenerateWorld(Main.ActiveWorldFileData.Seed);
                WorldGenerator.CurrentGenerationProgress = cache;
            }),
        };

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void DrawMenu(GameTime gameTime)
        {
            base.DrawMenu(gameTime);
            if (WorldGenerator.CurrentGenerationProgress == null)
                return;
            string str = "Generating world rn, please wait, this will take a very, very, very long time. Progress: " + WorldGenerator.CurrentGenerationProgress.Message + " " + WorldGenerator.CurrentGenerationProgress.Value + " / " + WorldGenerator.CurrentGenerationProgress.Value;
            Vector2 size = FontAssets.MouseText.Value.MeasureString(str) * 2;
            Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, Vector2.Zero, new Rectangle(0, 0, Main.screenWidth * 2, Main.screenHeight * 2), Color.Cyan, 0, Vector2.Zero, 1, 0, 0);
            Utils.DrawBorderString(Main.spriteBatch,
                str,
                Main.ScreenSize.ToVector2() * 0.5f - size * 0.5f, Color.White, 2);

        }
    }
}