using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace CalRemix.UI.Games.TrapperQuest
{
    public class TrapperQuestUIState : UIState
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
            if (!(Main.LocalPlayer.TryGetModPlayer(out CalRemixPlayer p) && p.tqactive))
            {
                return;
            }
            if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                Main.blockInput = false;
                Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().tqactive = false;
                initialized = false;
                TQHandler.Unload();
                LevelEditor.scrollNew = 0;
                LevelEditor.scrollOld = 0;
                return;
            }
            Main.blockInput = true;
            if (!initialized)
            {
                TQHandler.Load();
                initialized = true;
            }
            TQHandler.Run();

            base.Draw(spriteBatch);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            TQHandler.Draw(spriteBatch);
        }
    }

    [Autoload(Side = ModSide.Client)]
    internal class TrapperQuestUISystem : ModSystem
    {
        private UserInterface TrapperQuestUserInterface;

        internal TrapperQuestUIState TrapperQuestui;

        public override void Load()
        {
            TrapperQuestui = new();
            TrapperQuestUserInterface = new();
            TrapperQuestUserInterface.SetState(TrapperQuestui);
        }

        public override void AddRecipes()
        {
            LevelEditor.LoadTypes();
            LevelEditor.LoadOptions();

            int roomTypes = 4;
            string path = "UI/Games/TrapperQuest/Rooms/Room";
            for (int i = 0; i < roomTypes; i++)
            {
                TQRoomPopulator.LoadedRooms.Add(TQRoomPopulator.LoadRoom(path + i + ".txt"));
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            TrapperQuestUserInterface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
            if (resourceBarIndex != -1)
            {
                layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                    "CalRemix:TQInterface",
                    delegate
                    {
                        TrapperQuestUserInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}