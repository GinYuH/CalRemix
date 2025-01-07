using CalRemix.UI.Anomaly109;
using CalRemix.UI.Games.Boi.BaseClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace CalRemix.UI.Games.Boi
{
    public class BoiUIState : UIState
    {
        bool initialized = false;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Main.gameMenu)
                return;
            if (!(Main.LocalPlayer.TryGetModPlayer(out CalRemixPlayer p) && p.boiactive))
            {
                return;
            }
            if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                Main.blockInput = false;
                Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().boiactive = false;
                Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().boistage = 0;
                Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().boihealth = 0;
                initialized = false;
                BoiHandler.Unload();
                return;
            }
            Main.blockInput = true;
            if (!initialized)
            {
                BoiHandler.Initialize();
                initialized = true;
            }
            BoiHandler.Run();

            base.Draw(spriteBatch);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            BoiHandler.Draw(spriteBatch);
        }
    }

    [Autoload(Side = ModSide.Client)]
    internal class BoiUISystem : ModSystem
    {
        private UserInterface BoiUserInterface;

        internal BoiUIState Boiui;

        public override void Load()
        {
            Boiui = new();
            BoiUserInterface = new();
            BoiUserInterface.SetState(Boiui);
        }

        public override void UpdateUI(GameTime gameTime)
        {
            BoiUserInterface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
            if (resourceBarIndex != -1)
            {
                layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                    "CalRemix:BoiInterface",
                    delegate
                    {
                        BoiUserInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}