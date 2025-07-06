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
using CalRemix.Core.World.Subworld;

namespace CalRemix.Core.Subworlds
{
    public class BeautifulWinterWorldSubworld : Subworld, IDisableOcean
    {
        public override int Height => 2222;
        public override int Width => 2222;

        public override List<GenPass> Tasks => new List<GenPass>()
        {
            new PassLegacy("Winter Wonderland", (progress, config) =>
                {
                    progress.Message = "Creating a winter wonderland";
                    BeautifulWinterWorld.GenerateWinterWorld();
                })
        };

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void Update()
        {
            Main.LocalPlayer.ZoneBeach = false;
        }

        public override void DrawMenu(GameTime gameTime)
        {
            base.DrawMenu(gameTime);
            string str = "Wintering rn, please wait";
            Vector2 size = FontAssets.MouseText.Value.MeasureString(str) * 2;
            Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, Vector2.Zero, new Rectangle(0, 0, Main.screenWidth * 2, Main.screenHeight * 2), Color.Cyan, 0, Vector2.Zero, 1, 0, 0);
            Utils.DrawBorderString(Main.spriteBatch,
                str,
                Main.ScreenSize.ToVector2() * 0.5f - size * 0.5f, Color.White, 2);

        }
    }
}