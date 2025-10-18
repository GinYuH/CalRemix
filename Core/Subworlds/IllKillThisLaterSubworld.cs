using Microsoft.Xna.Framework;
using SubworldLibrary;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using static CalRemix.Core.Subworlds.SubworldHelpers;

namespace CalRemix.Core.Subworlds
{
    public class IllKillThisLaterSubworld : Subworld, IDisableOcean
    {
        public override int Height => 2222;
        public override int Width => 2222;

        public override List<GenPass> Tasks => new List<GenPass>()
        {
            new PassLegacy("Winter Wonderland", (progress, config) =>
                {
                    progress.Message = "Creating a winter wonderland";
                    IllKillThisLater.GenerateWinterWorld();
                })
        };

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void Update()
        {

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

    public class IllKillThisLater : ModSystem
    {
        public static void GenerateWinterWorld()
        {
            // heres a more stripped example, no grass-related stuff and auto turned into an ice biome
            // we gen everything first, then replace everything with snow and ice ns hit
            GenerateBasicTerrain_Recoded();
            Tunnels();
            DirtWallBackgrounds();
            // riight after the mega barebones world shaping youd wanna do other dirt-stone-based stuff
            // as well as other world-shaping things
            RocksInDirt();
            DirtInRocks();
            /*DirtLayerCaves();
            RockLayerCaves();*/
            CleanUpDirt();
            // super basic stuff done genning, dirt is pretty much finalized except sloping
            SmoothWorld();
            QuickCleanup();
            SpawnPoint();
            // foliage stuff next
            // by now the worlds major stuff is genned entirely, now we just place flavor around
            // final cleanup; never run anything after here unless ur dumb 
            TileCleanup();
            FinalCleanup();
        }
    }
}