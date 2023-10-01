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
using ReLogic.Graphics;
using ReLogic.Content;
using System.Linq;
using CalamityMod.BiomeManagers;
using CalamityMod.Items.Potions;
using CalamityMod.Items.Placeables.Furniture.CraftingStations;
using CalamityMod.Items.Materials;

namespace CalRemix.UI
{
    internal class Fanny : UIState
    {
        public Vector2 BasePosition => new Vector2(Main.screenWidth - 500, Main.screenHeight - 100);


        /// <summary>
        /// Fanny's current frame
        /// </summary>
        private int fanFrame;
        /// <summary>
        /// Fanny's frame counter
        /// </summary>
        private int fanFrameCounter;

        private static FannyMessage currentMessage;
        public static bool Speaking => currentMessage != null;

        public static FannyMessage UsedMessage {
            get => Speaking ? currentMessage : FannyManager.NoMessage;
            set => currentMessage = value;
        }


        public static void StopTalking() => currentMessage = null;


        /// <summary>
        /// Plays the desired message, ignoring any condition the message may have
        /// </summary>
        public static void TalkAbout(FannyMessage message)
        {
            if (Speaking || !message.CanPlayMessage())
                return;

            message.PlayMessage();
        }

        #region Drawing
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Main.playerInventory && !UsedMessage.DisplayOutsideInventory)
                return;
            base.Draw(spriteBatch);
        }

        // Here we draw our UI
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (Speaking)
            {
                // draw Fanny's dialogue box and if necessary, the item they're holding
                DrawText();
                if (UsedMessage.ItemType != -22)
                    DrawItem();
            }

            AnimateFanny();

            // finally draw Fanny
            Texture2D fannySprite = UsedMessage.Portrait.Texture.Value;
            Rectangle frame = fannySprite.Frame(1, UsedMessage.Portrait.frameCount, 0, fanFrame);
            spriteBatch.Draw(fannySprite, BasePosition, frame, Color.White, 0, new Vector2(fannySprite.Width / 2f, fannySprite.Height), 1f, 0, 0);
        }

        public void AnimateFanny()
        {
            fanFrameCounter++;
            if (fanFrameCounter > UsedMessage.Portrait.animationSpeed)
            {
                fanFrame++;
                fanFrameCounter = 0;
            }

            if (fanFrame >= UsedMessage.Portrait.frameCount)
                fanFrame = 0;
        }

        public void DrawText()
        {
            // a shit ton of variables
            var font = FontAssets.MouseText.Value;
            string text = UsedMessage.Text;

            Vector2 basePosition = BasePosition;
            Vector2 textSize = font.MeasureString(text) + new Vector2(16, 35);

            int outlineThickness = 3;
            int bgPadding = 10;

            Vector2 textDrawPowition = basePosition - textSize;
            Vector2 backgroundDrawPosition = textDrawPowition - Vector2.One * bgPadding;
            Vector2 outlineDrawPosition = backgroundDrawPosition - Vector2.One * outlineThickness;

            Vector2 backgroundSize = textSize + Vector2.One * bgPadding * 2;
            Vector2 outlineSize = backgroundSize + Vector2.One * outlineThickness * 2;

            Texture2D squareTexture = TextureAssets.MagicPixel.Value;

            // draw the border as a large rectangle behind, and the inners as a slightly smaller rectangle infront
            Main.spriteBatch.Draw(squareTexture, outlineDrawPosition, null, Color.Magenta, 0, Vector2.Zero, outlineSize / squareTexture.Size(), 0, 0);
            Main.spriteBatch.Draw(squareTexture, backgroundDrawPosition, null, Color.SaddleBrown, 0, Vector2.Zero, backgroundSize / squareTexture.Size(), 0, 0);
            // finally draw the text
            Utils.DrawBorderStringFourWay(Main.spriteBatch, font, text, basePosition.X - textSize.X, basePosition.Y - textSize.Y, Color.Lime * (Main.mouseTextColor / 255f), Color.DarkBlue, Vector2.Zero);
        }

        public void DrawItem()
        {
            Texture2D itemSprite = TextureAssets.Item[UsedMessage.ItemType].Value;
            int count = Main.itemAnimations[UsedMessage.ItemType] != null ? Main.itemAnimations[UsedMessage.ItemType].FrameCount : 1;
            Rectangle nframe = itemSprite.Frame(1, count, 0, 0);
            Vector2 origin = new Vector2((float)(itemSprite.Width / 2), (float)(itemSprite.Height / count / 2));
            Vector2 itemPosition = BasePosition + new Vector2(100, 30) + UsedMessage.ItemOffset + Vector2.UnitY * MathF.Sin(Main.GlobalTimeWrappedHourly * 2) * 4;

            Main.EntitySpriteDraw(itemSprite, itemPosition, nframe, Color.White, 0f, origin, UsedMessage.ItemScale, SpriteEffects.None);
        }
        #endregion
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
        public static List<FannyMessage> fannyMessages = new List<FannyMessage>();
        public static Dictionary<string, FannyPortrait> Portraits = new Dictionary<string, FannyPortrait>();

        public static FannyMessage NoMessage;
        

        public override void Load()
        {
            LoadFannyPortraits();
            LoadGeneralFannyMessages();

            NoMessage = new FannyMessage("", "Idle");
        }


        public override void PostUpdateEverything()
        {
            if (Main.dedServ)
                return;

            //Tick down message times
            for (int i = 0; i < fannyMessages.Count; i++)
            {
                FannyMessage msg = fannyMessages[i];

                //Tick down the cooldown
                if (msg.CooldownTime > 0)
                    msg.CooldownTime--;

                //Otherwise
                else if (msg.TimeLeft > 0)
                    msg.TimeLeft--;
                else
                {
                    Fanny.StopTalking();
                    msg.StartCooldown();
                }
            }

            //Don't even try looking for a new message if speaking
            if (Fanny.Speaking)
                return;

            Rectangle screenRect = new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight);
            var onscreenNPCs = Main.npc.Where(n => n.active && n.Hitbox.Intersects(screenRect));

            foreach (FannyMessage message in fannyMessages)
            {
                if (message.CanPlayMessage() && message.Condition(onscreenNPCs))
                {
                    message.PlayMessage();
                    break;
                }
            }
        }

        public static void LoadFannyPortraits()
        {
            FannyPortrait.LoadPortrait("Idle", 8);
            FannyPortrait.LoadPortrait("Awooga", 4);
            FannyPortrait.LoadPortrait("Cryptid", 1);
            FannyPortrait.LoadPortrait("Sob", 4);
            FannyPortrait.LoadPortrait("NuhUh", 19);
        }

        /// <summary>
        /// Registers a message for fanny to speak
        /// You can either provide a condition to the message, in which case the message will automatically play when the condition is met <br/>
        /// Alternatively, you could cache the message, and play it yourself when needed using <see cref="Fanny.TalkAbout(FannyMessage)"/>
        /// </summary>
        public static FannyMessage LoadFannyMessage(FannyMessage message)
        {
            fannyMessages.Add(message);
            return message;
        }


        //Loads fanny messages that aren't associated with anything else in particular
        private static void LoadGeneralFannyMessages()
        {
            fannyMessages.Add(new FannyMessage("Hello there! I'm Fanny the Flame, your personal guide to assist you with traversing this dangerous world. I wish you good luck on your journey and a Fan-tastic time!",
                "Idle", FannyMessage.AlwaysShow, -1, displayOutsideInventory: true));

            fannyMessages.Add(new FannyMessage("Na Na Na! The big robotic forge needs a lot of blue meat from the ads! It cannot work without it!",
                "NuhUh", HasDraedonForgeMaterialsButNoMeat));

            fannyMessages.Add(new FannyMessage("Fear the Meld Gunk.", 
                "Idle", (IEnumerable<NPC> npcs) => Main.hardMode && Main.LocalPlayer.InModBiome<UndergroundAstralBiome>(), cooldown: 120));

            fannyMessages.Add(new FannyMessage("Oooh! Delicious Meat! Collect as much as you can, it will save you a lot of time.", "Awooga",
                (IEnumerable<NPC> npcs) => Main.LocalPlayer.HasItem(ModContent.ItemType<DeliciousMeat>())));

            fannyMessages.Add(new FannyMessage("It appears you're approaching the Dungeon. Normally this place is guarded by viscious guardians, but I've disabled them for you my dear friend.", "NuhUh",
                (IEnumerable<NPC> npcs) => Main.LocalPlayer.WithinRange(new Vector2(Main.dungeonX, Main.dungeonY), 700), onlyPlayOnce: true));

            //Add a condition to this one yuh, to pass the test of knowledge...
            fannyMessages.Add(new FannyMessage("I hope you know what you've gotten yourself into... Go kill some Cnidrions instead.", "NuhUh"));
           
        }

        #region Conditions for general messages
        public static bool HasDraedonForgeMaterialsButNoMeat(IEnumerable<NPC> npcs)
        {
            if (Main.LocalPlayer.HasItem(ModContent.ItemType<DeliciousMeat>()))
                return false;

            List<int> materials = new List<int>();
            {
                materials.Add(ModContent.ItemType<CosmicAnvilItem>());
                materials.Add(ItemID.LunarCraftingStation);
                materials.Add(ModContent.ItemType<AuricBar>());
                materials.Add(ModContent.ItemType<ExoPrism>());
                materials.Add(ModContent.ItemType<AscendantSpiritEssence>());
            }

            return Main.LocalPlayer.HasItems(materials);
        }
        #endregion
    }


    public class FannyPortrait
    {
        public Asset<Texture2D> Texture;
        public int frameCount;
        public int animationSpeed;

        public static void LoadPortrait(string portraitName, int frameCount, int animationSpeed = 12)
        {
            FannyPortrait portrait = new FannyPortrait(portraitName, frameCount, animationSpeed);
            //Load itself into the portrait list
            if (FannyManager.Portraits.ContainsKey(portraitName))
                FannyManager.Portraits.Add(portraitName, portrait);
        }

        public FannyPortrait(string portrait, int frameCount, int animationSpeed = 12)
        {
            Texture = ModContent.Request<Texture2D>("CalRemix/UI/Fanny/Fanny" + portrait);
            this.frameCount = frameCount;
            this.animationSpeed = animationSpeed;
        }
    }


    public class FannyMessage
    {
        private string originalText;
        private string formattedText;

        public string Text {
            get => formattedText;
            set {
                //Cache the original text, and format it
                originalText = value;
                FormatText(FontAssets.MouseText.Value);
            }
        }

        #region Message Condition bases
        public delegate bool FannyMessageCondition(IEnumerable<NPC> onscreenNPCs);
        public static bool NeverShow(IEnumerable<NPC> onscreenNPCs) => false;
        public static bool AlwaysShow(IEnumerable<NPC> onscreenNPCs) => true;

        public FannyMessageCondition Condition { get; set; }
        #endregion

        public int CooldownTime { get; set; }
        private int cooldownDuration;

        public int TimeLeft { get; set; }
        private int messageDuration;

        private bool alreadySeen = false;

        public bool DisplayOutsideInventory { get; set; }
        public bool OnlyPlayOnce { get; set; }
        public FannyPortrait Portrait { get; set; }

        public FannyMessage(string message, string portrait = "", FannyMessageCondition condition = null, float duration = 5, float cooldown = 60, bool displayOutsideInventory = false, bool onlyPlayOnce = false)
        {
            Text = message;
            Condition = condition ?? NeverShow;

            //Duration and cooldown are inputted in seconds and then converted into frames by the constructor
            cooldownDuration = (int)(cooldown * 60);
            CooldownTime = 0;
            
            messageDuration = (int)(duration * 60);
            TimeLeft = 0;

            DisplayOutsideInventory = displayOutsideInventory;

            if (portrait == "")
                portrait = "Idle";
            Portrait = FannyManager.Portraits[portrait];
        }

        //Technically the TimeLeft is not needed because when its active, no other message will try to play. But just in case
        public bool CanPlayMessage()
        {
            return CooldownTime <= 0 && TimeLeft <= 0 && (!OnlyPlayOnce || !alreadySeen);
        }

        public void PlayMessage()
        {
            TimeLeft = messageDuration;
            Fanny.UsedMessage = this;
            alreadySeen = true;
        }

        public void StartCooldown() => CooldownTime = cooldownDuration;

        //Item display stuff
        public int ItemType { get; set; } = -22;
        public float ItemScale { get; set; } = 1f;
        public Vector2 ItemOffset { get; set; } = Vector2.Zero;
        public FannyMessage AddItemDisplay(int itemType, float itemScale = 1f, Vector2? itemOffset = null)
        {
            ItemType = itemType;
            ItemScale = itemScale;
            ItemOffset = itemOffset ?? Vector2.Zero;
            return this;
        }

        #region Text formatting
        public void FormatText(DynamicSpriteFont font, float maxLineWidth = 380)
        {
            if (Main.dedServ)
                return;

            string currentWord = "";
            float currentLineLenght = 0;
            float spaceWidth = font.MeasureString(" ").X;

            //This is the setence as we are building it
            string formattedSetence = "";


            for (int i = 0; i < originalText.Length; i++)
            {
                //When theres a space, check if the word is short enough to not go over our width limit
                if (originalText[i] == ' ')
                {
                    CheckWord(currentWord, maxLineWidth, font, ref formattedSetence, ref currentLineLenght, spaceWidth);
                    currentWord = "";
                }

                //Continue to build the word
                else
                    currentWord += originalText[i];
            }
            //Check the final word
            if (currentWord != "")
                CheckWord(currentWord, maxLineWidth, font, ref formattedSetence, ref currentLineLenght, spaceWidth);

            Text = formattedSetence;
        }

        /// <summary>
        /// Checks if a word can be fitted into the current text line, and if not adds a linebreak
        /// </summary>
        /// <param name="word"></param>
        private void CheckWord(string word, float maxLineWidth, DynamicSpriteFont font, ref string formattedSetence, ref float currentLineLenght, float spaceWidth)
        {
            //Get the lenght of the word
            float wordLenght = font.MeasureString(word).X;

            //If adding the word doesn't make the line go over the max width, simply add the lenght of our last word (and the space) to the current line
            if (wordLenght + currentLineLenght <= maxLineWidth)
                currentLineLenght += wordLenght + spaceWidth;

            //If adding the word goes over the max width
            else
            {
                //Reset the line lenght to a new line (so only the word and the space
                currentLineLenght = wordLenght + spaceWidth;
                //Add a linebreak
                formattedSetence += "\n";
            }

            //Add the word and space to the end of the setence
            formattedSetence += word;
            formattedSetence += " ";
        }
        #endregion
    }
}