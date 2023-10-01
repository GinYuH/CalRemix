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
using static System.Net.Mime.MediaTypeNames;
using Terraria.ID;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CalRemix.UI
{
    internal class Fanny : UIState
    { 
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

        public static int currentMessageID;

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

        public static FannyMessage CurrentMessage => FannyManager.fannyMessages[currentMessageID];        

        public override void Draw(SpriteBatch spriteBatch)
        {
            // This prevents drawing unless we are using an ExampleCustomResourceWeapon
            if (!Main.playerInventory && !CurrentMessage.persistent)
                return;

            base.Draw(spriteBatch);
        }

        // Here we draw our UI
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            // if the current message's lifetime is up KICK IT OUT
            if (CurrentMessage.duration <= 0)
            {
                CurrentMessage.cooldown = CurrentMessage.cooldownMax;
                currentMessageID = FannyMessageID.none;
            }
            // initialize Fanny's default animation stats if they dont exist yet
            if (fannyPath == null)
            {
                fannyPath = "CalRemix/UI/Fanny/FannyIdle";
                fannyFrameMax = 8;
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
            if (CurrentMessage.itemID != -22)
            {
                DrawItem();
            }
            spriteBatch.Draw(face, new Rectangle(baseX, baseY, face.Width, (face.Height / fannyFrameMax)), nframe, Color.White);
        }

        /// <summary>
        /// Loads the desired message into the Fanny system. Check FannyMessageID for ids.
        /// </summary>
        public static void Dialogue(int id)
        {
            if (currentMessageID != FannyMessageID.none)
            {
                return;
            }
            if (currentMessageID == id && CurrentMessage.cooldown > 0)
            {
                return;
            }
            if (FannyManager.fannyMessages[id].cooldown > 0)
            {
                return;
            }
            currentMessageID = id;
            CurrentMessage.duration = CurrentMessage.durationMax;
        }

        /// <summary>
        /// The function which draws Fanny's text
        /// </summary>
        public void DrawText()
        {
            // if Fanny doesn't have anything to say, reset him to idle
            if (currentMessageID == 0)
            {
                fannyPath = "CalRemix/UI/Fanny/FannyIdle";
                fannyFrameMax = 8;
                return;
            }
            // a shit ton of variables
            string text = CurrentMessage.message;
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
            int baseX = CurrentMessage.positionX;
            int baseY = CurrentMessage.positionY;
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
            if (CurrentMessage.itemID == -22)
            {
                return;
            }
            Texture2D itemSprite = TextureAssets.Item[CurrentMessage.itemID].Value;
            int count = Main.itemAnimations[CurrentMessage.itemID] != null ? Main.itemAnimations[CurrentMessage.itemID].FrameCount : 1;
            Rectangle nframe = itemSprite.Frame(1, count, 0, 0);
            Vector2 origin = new Vector2((float)(itemSprite.Width / 2), (float)(itemSprite.Height / count / 2));
            Main.EntitySpriteDraw(itemSprite, new Vector2(1510 + CurrentMessage.itemX, 830 + CurrentMessage.itemY + (float)System.Math.Sin(Main.GlobalTimeWrappedHourly * 2) * 4), nframe, Color.White, 0f, origin, CurrentMessage().itemScale, SpriteEffects.None);
        }
        public void DetermineAnimation()
        {
            string fanPath = "CalRemix/UI/Fanny/Fanny";
            // go through a switch case to decide which animation and frame count Fanny currently needs to use
            switch (CurrentMessage.animation)
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

    public class FannyManager : ModSystem
    {
        public static bool start = false;
        public static List<bool> fannybools = new List<bool>();
        public static List<FannyMessage> fannyMessages = new List<FannyMessage>();

        public override void OnWorldLoad()
        {
            LoadFannyMessages();
            fannybools.Add(start);

            start = false;
        }
        public override void PostUpdatePlayers()
        {
            fannyMessages[0].duration = fannyMessages[0].cooldown = 22;
            // constantly tick down the cooldown and lifetime of every message if above 0
            for (int i = 0; i < fannyMessages.Count; i++)
            {
                FannyMessage msg = fannyMessages[i];
                // cooldown only applies if it's not the currently displayed message
                if (Fanny.currentMessageID != i)
                {
                    if (msg.cooldown <= 0 || i == 0)
                    {
                        continue;
                    }
                    else
                    {
                        msg.cooldown--;
                    }
                }

                if (msg.duration <= 0)
                {
                    continue;
                }
                else
                {
                    msg.duration--;
                }
            }
        }
        public override void OnWorldUnload()
        {
            //start = false;
        }
        public override void SaveWorldData(TagCompound tag)
        {
            //tag["fanstart"] = start;
        }
        public override void LoadWorldData(TagCompound tag)
        {
            //start = tag.Get<bool>("fanstart");
        }
        public override void NetSend(BinaryWriter writer)
        {
            //writer.Write(start);
        }
        public override void NetReceive(BinaryReader reader)
        {
             //start = reader.ReadBoolean();
        }
        public static void LoadFannyMessages()
        {
            fannyMessages.Add(new FannyMessage(""));
            fannyMessages.Add(new FannyMessage(FannyMessageList.start, -1, 300, true));
            fannyMessages.Add(new FannyMessage(FannyMessageList.desertmed));
            fannyMessages.Add(new FannyMessage(FannyMessageList.aspid, 180));
            fannyMessages.Add(new FannyMessage(FannyMessageList.ogsworm));
            fannyMessages.Add(new FannyMessage(FannyMessageList.dungeon));
            fannyMessages.Add(new FannyMessage(FannyMessageList.delicious));
            fannyMessages.Add(new FannyMessage(FannyMessageList.draeforge));
            fannyMessages.Add(new FannyMessage(FannyMessageList.meld, 7200));
        }
    }

    public class FannyMessageID
    {
        public static int none = 0;
        public static int start = 1;
        public static int desertmed = 2;
        public static int aspid = 3;
        public static int ogsworm = 4;
        public static int dungeon = 5;
        public static int delicious = 6;
        public static int draeforge = 7;
        public static int meld = 8;
    }

    public class FannyMessageList
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

    public class FannyMessage
    {
        public string message { get; set; }
        public int cooldownMax { get; set; }
        public int duration { get; set; }
        public int durationMax { get; set; }
        public bool persistent { get; set; }
        public int animation { get; set; }
        public int positionX { get; set; }
        public int positionY { get; set; }
        public int itemID { get; set; }
        public float itemScale { get; set; }
        public int itemX { get; set; }
        public int itemY { get; set; }
        public int cooldown { get; set; }

        public FannyMessage(string message, int cooldown = 3600, int duration = 300, bool persistent = false, int animation = 0, int posX = 1400, int posY = 800, int itemID = -22, float itemScale = 1f, int itemX = 0, int itemY = 0)
        {
            this.message = message;
            this.cooldown = cooldown;
            cooldownMax = cooldown;
            this.duration = duration;
            durationMax = duration;
            this.persistent = persistent;
            this.animation = animation;
            positionX = posX;
            positionY = posY;
            this.itemID = itemID;
            this.itemScale = itemScale;
            this.itemX = itemX;
            this.itemY = itemY;
        }
    }
}