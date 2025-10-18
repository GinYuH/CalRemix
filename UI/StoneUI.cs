using CalamityMod;
using CalRemix.Content.Items.ZAccessories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace CalRemix.UI
{
    public class StoneUI : UIState
    {

        public static Dictionary<int, Color> stoneColors = new();
        public override void Draw(SpriteBatch spriteBatch)
        {
            return;
            if (stoneColors.Count <= 0)
            {
                foreach (Item e in ContentSamples.ItemsByType.Values)
                {
                    if (e.ModItem == null)
                        continue;
                    if (e.ModItem is DebuffStone stohne)
                    {
                        Texture2D debuff = TextureAssets.Buff[stohne.debuffType].Value;
                        Color[,] color = debuff.GetColorsFromTexture();
                        Color mid = color[16, 16];
                        int atts = 5;
                        // if the color is too dark, attempt finding a brighter one
                        for (int i = -atts; i < atts; i++)
                        {
                            if ((mid.R + mid.G + mid.B) < 255f)
                            {
                                mid = color[16 + i * 2, 16];
                            }
                            else
                            {
                                break;
                            }
                        }
                        stohne.debuffColor = mid;
                        stoneColors.Add(e.type, mid);
                    }
                }
            }


            int rouge = Main.LocalPlayer.CountItem(ItemID.RedBanner);
            int green = Main.LocalPlayer.CountItem(ItemID.GreenBanner);
            int blue = Main.LocalPlayer.CountItem(ItemID.BlueBanner);


            float xBase = Main.screenWidth * 0.4f;
            float yBase = Main.screenHeight * 0.4f;
            float xPos = xBase;
            float yPos = yBase;

            Utils.DrawBorderString(spriteBatch, "CURRENT COLORS: R:" + rouge + ", G:" + green + ", B:" + blue, new Vector2(xPos, yPos - 30), Color.White);
            foreach (var v in stoneColors)
            {
                int redThresh = v.Value.R;
                int greenThresh = v.Value.G;
                int blueThresh = v.Value.B;
                if (rouge == redThresh && green == greenThresh && blue == blueThresh)
                {
                    string name = ContentSamples.ItemsByType[v.Key].Name + "(R:" + redThresh + ", G:" + greenThresh + ", B:" + blueThresh + ") |||";
                    name = name.Replace("Stone", "");
                    Utils.DrawBorderString(spriteBatch, name, new Vector2(xPos, yPos), Color.White);
                    xPos += FontAssets.MouseText.Value.MeasureString(name).X + 6;
                    if (xPos > Main.screenWidth - xBase)
                    {
                        xPos = xBase;
                        yPos += FontAssets.MouseText.Value.MeasureString("Mis nuevos los gatos").Y + 4;
                    }
                }
            }
        }
    }

    [Autoload(Side = ModSide.Client)]
    internal class StoneUISystem : ModSystem
    {
        private UserInterface UserInter;

        internal StoneUI SUI;

        public override void Load()
        {
            SUI = new();
            UserInter = new();
            UserInter.SetState(SUI);
        }

        public override void UpdateUI(GameTime gameTime)
        {
            UserInter?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
            if (resourceBarIndex != -1)
            {
                layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                    "CalRemix:StoneAlchemyInterface",
                    delegate
                    {
                        UserInter.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}