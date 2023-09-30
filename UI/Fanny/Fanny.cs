using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent;
using System.Collections.Generic;
using Terraria.Social.Base;
using Terraria.ModLoader.IO;
using System.IO;
using CalRemix.Projectiles.Weapons;
using System;
using CalamityMod;

namespace CalRemix.UI
{
    internal class Fanny : UIState
    {    /// <summary>
         /// The horizontal position of the text box relative to Fanny
         /// </summary>
        public static int textX;
        /// <summary>
        /// The vertical position of the text box relative to Fanny
        /// </summary>
        public static int textY;
        /// <summary>
        /// unused
        /// </summary>
        public static int overrideX;
        /// <summary>
        /// Fanny's current dialogue
        /// </summary>
        public static string displayText;
        /// <summary>
        /// How much time left Fanny's dialogue will last
        /// </summary>
        public static int textDuration;
        /// <summary>
        /// Fanny's current frame
        /// </summary>
        private int fanFrame;
        /// <summary>
        /// Fanny's frame counter
        /// </summary>
        private int fanFrameCounter;
        /// <summary>
        /// The maximum amount of frames Fanny's current animation uses
        /// </summary>
        public static int fannyFrameMax;
        /// <summary>
        /// The string path to where Fanny's current animation sprite is stored
        /// </summary>
        public static string fannyPath;
        /// <summary>
        /// How much time left before Fanny disappears while not in the inventory
        /// </summary>
        public static int persistCountdown;
        /// <summary>
        /// What item Fanny should display. Set to 22 for nothing to display
        /// </summary>
        public static int itemDisplay;
        /// <summary>
        /// Fanny's item X offset
        /// </summary>
        public static int itemX;
        /// <summary>
        /// Fanny's item X offset
        /// </summary>
        public static int itemY;
        /// <summary>
        /// Fanny's item scale
        /// </summary>
        public static float itemScal;

        public override void OnInitialize()
        {
            fannyFrameMax = 7;
            textX = 1400;
            textY = 800;
            textDuration = 300;
            displayText = "";
            overrideX = -1;
            itemDisplay = 22;
        }

        /// <summary>
        /// 1 = Idle
        /// 2 = Nuhuh
        /// 3 = Awooga
        /// 4 = Sob
        /// </summary>
        public enum FannyAnimation
        {
            Idle = 0,
            Nuhuh = 1,
            Awooga = 2,
            Sob = 3
        };

        public override void Draw(SpriteBatch spriteBatch)
        {
            // This prevents drawing unless we are using an ExampleCustomResourceWeapon
            if (!Main.playerInventory && persistCountdown <= 0)
                return;

            base.Draw(spriteBatch);
        }

        // Here we draw our UI
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            // initialize Fanny's default animation stats if they dont exist yet
            if (fannyPath == null)
            {
                fannyPath = "CalRemix/UI/Fanny/FannyIdle";
                fannyFrameMax = 8;
                itemDisplay = 22;
            }
            // click down the persistence counter
            if (persistCountdown > 0 && Main.hasFocus)
            {
                persistCountdown--;
            }
            if (textDuration <= 0)
            {
                displayText = "";
            }
            // draw positioning
            int baseX = 1400;
            int baseY = 800;
            // draw Fanny's dialogue box
            DrawText();
            // animation
            fanFrameCounter++;
            if (fanFrame > fannyFrameMax)
            {
                fanFrame = 0;
            }
            if (fanFrameCounter % 12 == 0)
            {
                fanFrame++;
                if (fanFrame > fannyFrameMax - 1)
                {
                    fanFrame = 0;
                }
            }
            // finally draw Fanny
            Texture2D face = ModContent.Request<Texture2D>(fannyPath).Value;
            Rectangle nframe = face.Frame(1, fannyFrameMax, 0, (int)fanFrame);
            if (itemDisplay != 22)
            {
                DrawItem();
            }
            spriteBatch.Draw(face, new Rectangle(baseX, baseY, face.Width, (face.Height / fannyFrameMax)), nframe, Color.White);
        }

        /// <summary>
        /// Causes Fanny to talk to you. 
        /// text is the dialogue itself. 
        /// face is the id of the animation (see the FannyAnimation enum). 
        /// time is how long the message should display. 
        /// baseX and baseY are the draw positions relative to Fanny
        /// xOverride is how long Fanny should persist if the player exits their inventory
        /// </summary>
        public static void Dialogue(string text, int face = 0, int time = 300, int baseX = 1400, int baseY = 800, int xOverride = -1)
        {
            // set Fanny's variables
            textDuration = time;
            displayText = text;
            textX = baseX;
            textY = baseY;
            persistCountdown = xOverride;
            string fanPath = "CalRemix/UI/Fanny/Fanny";
            // go through a switch case to decide which animation and frame count Fanny currently needs to use
            switch (face)
            {
                case (int)FannyAnimation.Idle:
                    fannyFrameMax = 8;
                    fanPath += "Idle";
                    break;
                case (int)FannyAnimation.Nuhuh:
                    fannyFrameMax = 19;
                    fanPath += "Nuhuh";
                    break;
                case (int)FannyAnimation.Awooga:
                    fannyFrameMax = 4;
                    fanPath += "Awooga";
                    break;
                case (int)FannyAnimation.Sob:
                    fannyFrameMax = 4;
                    fanPath += "Sob";
                    break;
                default:
                    fannyFrameMax = 8;
                    fanPath += "Idle";
                    break;
            }
            fannyPath = fanPath;
        }
        public static void Item(int itemID = 22, int xPos = 0, int yPos = 0, float scale = 1f)
        {
            itemDisplay = itemID;
            itemX = xPos;
            itemY = yPos;
            itemScal = scale;
        }

        /// <summary>
        /// The function which draws Fanny's text
        /// </summary>
        public void DrawText()
        {
            // if Fanny doesn't have anything to say, reset him to idle
            if (displayText == "" || textDuration <= 0)
            {
                fannyPath = "CalRemix/UI/Fanny/FannyIdle";
                fannyFrameMax = 8;
                return;
            }
            if (Main.hasFocus)
            {
                textDuration--;
            }
            // a shit ton of variables
            string text = displayText;
            int maxCharsPerLine = 35;
            bool dueForBreak = false;
            // go through the dialogue and chop it up into lines
            for (int i = 0; i < text.Length; i++)
            {
                // if the maximum characters have been reached attempt to insert a newline
                if (i % maxCharsPerLine == 0 && i > 0 && !dueForBreak)
                {
                    if (text[i] == ' ')
                    {
                        text = text.Remove(i, 1);
                        text = text.Insert(i, "\n");
                    }
                    else
                    {
                        // if it can't find a space, let Fanny's code know it needs to insert a newline asap
                        dueForBreak = true;
                    }
                }
                if (text[i] == ' ' && dueForBreak)
                {
                    text = text.Remove(i, 1);
                    text = text.Insert(i, "\n");
                    dueForBreak = false;
                }
            }
            string[] lineList = text.Split('\n');
            string longestLine = lineList[0];
            for (int i = 0; i < lineList.Length; i++)
            {
                if (lineList[i].Length > longestLine.Length)
                {
                    longestLine = lineList[i];
                }
            }
            int baseX = textX;
            int baseY = textY;
            int textLength = text.Length;
            int lineAmount = textLength / maxCharsPerLine;
            int textWidth = (int)FontAssets.MouseText.Value.MeasureString(longestLine).X + 16;
            int textHeight = (int)(lineAmount * (FontAssets.MouseText.Value.MeasureString(longestLine).Y)) + 35;
            int textOffsetX = textWidth;
            int textOffsetY = textHeight;
            int borderWith = 3;
            int bgPadding = 10;
            // draw the border as a large rectangle behind, and the inners as a slightly smaller rectangle infront
            Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(baseX - textOffsetX - borderWith - bgPadding, baseY - textOffsetY - borderWith - bgPadding, textWidth + borderWith * 2 + bgPadding / 2, textHeight + borderWith * 2 + bgPadding / 2), Color.Magenta);
            Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(baseX - textOffsetX - bgPadding, baseY - textOffsetY - bgPadding, textWidth + bgPadding / 2, textHeight + bgPadding / 2), Color.SaddleBrown);
            // finally draw the text
            Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, text, baseX - textOffsetX, baseY - textOffsetY, Color.Lime * (Main.mouseTextColor / 255f), Color.DarkBlue, Vector2.Zero);
        }

        public void DrawItem()
        {
            if (itemDisplay == 22)
            {
                return;
            }
            Texture2D itemSprite = TextureAssets.Item[itemDisplay].Value;
            int count = Main.itemAnimations[itemDisplay] != null ? Main.itemAnimations[itemDisplay].FrameCount : 1;
            Rectangle nframe = itemSprite.Frame(1, count, 0, 0);
            Vector2 origin = new Vector2((float)(itemSprite.Width / 2), (float)(itemSprite.Height / count / 2));
            Main.EntitySpriteDraw(itemSprite, new Vector2(1510 + itemX, 830 + itemY + (float)System.Math.Sin(Main.GlobalTimeWrappedHourly * 2) * 4), nframe, Color.White, 0f, origin, itemScal, SpriteEffects.None);
        }
    }

    // This class will only be autoloaded/registered if we're not loading on a server
    [Autoload(Side = ModSide.Client)]
    internal class FannyUISystem : ModSystem
    {
        private UserInterface FannyInterface;

        internal Fanny FannyTheFlame;

        public override void Load()
        {
            FannyTheFlame = new();
            FannyInterface = new();
            FannyInterface.SetState(FannyTheFlame);
        }

        public override void UpdateUI(GameTime gameTime)
        {
            FannyInterface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
            if (resourceBarIndex != -1)
            {
                layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                    "CalRemix: Fanny",
                    delegate
                    {
                        FannyInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }

    public class FannyBools : ModSystem
    {
        public static bool start = false;
        public static bool desertmed = false;
        public static bool aspid = false;
        public static bool ogsworm = false;
        public static bool dungeon = false;
        public static bool delicious = false;
        public static bool draeforge = false;
        public static bool meld = false;
        public static List<bool> fannybools = new List<bool>();

        public override void OnWorldLoad()
        {
            fannybools.Add(start);
            fannybools.Add(desertmed);
            fannybools.Add(aspid);
            fannybools.Add(ogsworm);
            fannybools.Add(dungeon);
            fannybools.Add(delicious);
            fannybools.Add(draeforge);
            fannybools.Add(meld);

            start = false;
        }
        public override void OnWorldUnload()
        {
            start = false;
        }
        public override void SaveWorldData(TagCompound tag)
        {
            tag["fanstart"] = start;
        }
        public override void LoadWorldData(TagCompound tag)
        {
            start = tag.Get<bool>("fanstart");
        }
        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(start);
        }
        public override void NetReceive(BinaryReader reader)
        {
             start = reader.ReadBoolean();
        }

        public class FannyMessageID
        {
            public static string start = "Hello there! I'm Fanny the Flame, your personal guide to assist you with traversing this dangerous world. I wish you good luck on your journey and a Fan-tastic time!";
            public static string desertmed = "I hope you know what you've gotten yourself into... Go kill some Cnidrions instead.";
            public static string aspid = "Uh oh! A Primal Aspid! Best be wary around those buggers as killing too many may subject you to ancient ice spells!";
            public static string ogsworm = "That Ogsculian Burrower over there. A dangerous foe. The best course of action here is to jump over them to dodge their laser of doom.";
            public static string dungeon = "It appears you're approaching the Dungeon. Normally this place is guarded by viscious guardians, but I've disabled them for you my dear friend.";
            public static string delicious = "Oooh! Delicious Meat! Collect as much as you can, it will save you a lot of time.";
            public static string draeforge = "Na Na Na! The big robotic forge needs a lot of blue meat from the ads! It cannot work without it!";
            public static string meld = "Fear the Meld Gunk.";      
        }
    }
}