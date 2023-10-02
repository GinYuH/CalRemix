﻿using Microsoft.Xna.Framework;
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
using Terraria.Audio;

namespace CalRemix.UI
{
    public class Fanny : UIElement
    {
        public Vector2 BasePosition => GetDimensions().ToRectangle().Bottom();
        public static FannyTextbox SpeechBubble;

        private static int fanFrame;
        private static int fanFrameCounter;

        public float bounce;
        public float tickle;

        public static bool needsToShake;
        public static float shakeTime;

        //Small break between messages
        public static int talkCooldown;

        private static FannyMessage currentMessage;
        public static bool Speaking => currentMessage != null;

        public static FannyMessage UsedMessage {
            get => Speaking ? currentMessage : FannyManager.NoMessage;
            set {
                currentMessage = value;
                SpeechBubble.Recalculate();
            }
        }


        public static void StopTalking()
        {
            currentMessage = null;
            talkCooldown = 60;
        }


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
        // Here we draw our UI
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (Speaking && UsedMessage.ItemType != -22)
                DrawItem();

            AnimateFanny();

            // finally draw Fanny
            Texture2D fannySprite = UsedMessage.Portrait.Texture.Value;
            Rectangle frame = fannySprite.Frame(1, UsedMessage.Portrait.frameCount, 0, fanFrame);
            Vector2 position = BasePosition;

            if (shakeTime > 0)
            {
                position += Main.rand.NextVector2Circular(1f, 1f) * 5f * Utils.GetLerpValue(0.6f, 1f, shakeTime, true);

                float bounceTime = Utils.GetLerpValue(0.6f, 1f, shakeTime, true);
                position.Y -= Math.Abs(MathF.Sin(bounceTime * MathHelper.TwoPi)) * 62f * MathF.Pow(bounceTime, 0.6f);
            }

            else if (ContainsPoint(Main.MouseScreen) || bounce > 0)
            {
                if (bounce < 0)
                    bounce = 1;

                position.Y -= MathF.Pow(Math.Abs(MathF.Sin(bounce * MathHelper.Pi)), 0.6f) * 22f;
            }

            if (tickle >= 1)
                position += Main.rand.NextVector2Circular(3f, 3f) * tickle;


            spriteBatch.Draw(fannySprite, position, frame, Color.White * FannyUIState.fadeIn, 0, new Vector2(frame.Width / 2f, frame.Height), 1f, 0, 0);
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

        public void DrawItem()
        {
            Texture2D itemSprite = TextureAssets.Item[UsedMessage.ItemType].Value;
            int count = Main.itemAnimations[UsedMessage.ItemType] != null ? Main.itemAnimations[UsedMessage.ItemType].FrameCount : 1;
            Rectangle nframe = itemSprite.Frame(1, count, 0, 0);
            Vector2 origin = new Vector2((float)(itemSprite.Width / 2), (float)(itemSprite.Height / count / 2));
            Vector2 itemPosition = BasePosition + new Vector2(100, 30) + UsedMessage.ItemOffset + Vector2.UnitY * MathF.Sin(Main.GlobalTimeWrappedHourly * 2) * 4;

            Main.EntitySpriteDraw(itemSprite, itemPosition, nframe, Color.White * FannyUIState.fadeIn, 0f, origin, UsedMessage.ItemScale, SpriteEffects.None);
        }
        #endregion
    }

    public class FannyTextbox : UIElement
    {
        public Fanny ParentFanny;
        public int outlineThickness = 3;
        public int backgroundPadding = 10;

        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);

            //Set the timeleft of the message to 30
            if (Fanny.Speaking && FannyUIState.fadeIn == 1 && Fanny.UsedMessage.NeedsToBeClickedOff && Fanny.UsedMessage.TimeLeft > 30)
                Fanny.UsedMessage.TimeLeft = 30;
        }

        public override void Recalculate()
        {
            Vector2 basePosition = ParentFanny.BasePosition - new Vector2(50, 90);
            Vector2 textSize = FontAssets.MouseText.Value.MeasureString(Fanny.UsedMessage.Text);
            Vector2 cornerPosition = basePosition - textSize;

            //Account for padding
            textSize += Vector2.One * (backgroundPadding + outlineThickness) * 2;
            cornerPosition -= Vector2.One * (backgroundPadding + outlineThickness);

            //Fade out
            cornerPosition.Y -= MathF.Pow(Utils.GetLerpValue(30, 0, Fanny.UsedMessage.TimeLeft, true), 1.6f) * 30f;
            //Fade in
            cornerPosition.Y += MathF.Pow(1 - FannyUIState.fadeIn, 2f) * 40;

            Width.Set(textSize.X, 0);
            Height.Set(textSize.Y, 0);
            Left.Set(cornerPosition.X, 0);
            Top.Set(cornerPosition.Y, 0);
            base.Recalculate();
        }


        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (!Fanny.Speaking)
                return;


            // a shit ton of variables
            var font = FontAssets.MouseText.Value;
            string text = Fanny.UsedMessage.Text;

            Rectangle dimensions = GetDimensions().ToRectangle();

            Vector2 outlineDrawPosition = dimensions.TopLeft();
            Vector2 backgroundDrawPosition = outlineDrawPosition + Vector2.One * outlineThickness;
            Vector2 textDrawPosition = backgroundDrawPosition + Vector2.One * backgroundPadding;

            Vector2 outlineSize = dimensions.Size();
            Vector2 backgroundSize = outlineSize - Vector2.One * outlineThickness * 2;

            Texture2D squareTexture = TextureAssets.MagicPixel.Value;
            float opacity = FannyUIState.fadeIn * Utils.GetLerpValue(0, 30, Fanny.UsedMessage.TimeLeft, true);

            // draw the border as a large rectangle behind, and the inners as a slightly smaller rectangle infront
            Main.spriteBatch.Draw(squareTexture, outlineDrawPosition, null, Color.Magenta * opacity, 0, Vector2.Zero, outlineSize / squareTexture.Size(), 0, 0);
            Main.spriteBatch.Draw(squareTexture, backgroundDrawPosition, null, Color.SaddleBrown * opacity, 0, Vector2.Zero, backgroundSize / squareTexture.Size(), 0, 0);


            if (ContainsPoint(Main.MouseScreen) && FannyUIState.fadeIn == 1 && Fanny.UsedMessage.NeedsToBeClickedOff && Fanny.UsedMessage.TimeLeft > 30)
            {
                Main.spriteBatch.Draw(squareTexture, backgroundDrawPosition, null, Color.SaddleBrown with { A = 0 } * (0.4f + 0.2f * MathF.Sin(Main.GlobalTimeWrappedHourly * 4f)) * opacity, 0, Vector2.Zero, backgroundSize / squareTexture.Size(), 0, 0);
                Main.LocalPlayer.mouseInterface = true;
                Main.instance.MouseText("Thank you for the help, Fanny!");
            }

            // finally draw the text
            Utils.DrawBorderStringFourWay(Main.spriteBatch, font, text, textDrawPosition.X, textDrawPosition.Y, Color.Lime * (Main.mouseTextColor / 255f) * opacity, Color.DarkBlue * opacity, Vector2.Zero);
        }
    }

    public class FannyUIState : UIState 
    {
        public Fanny FannyTheFire;
        public static float fadeIn;

        public override void OnInitialize()
        {
            FannyTheFire = new Fanny();
            FannyTheFire.Left.Set(-80, 1);
            FannyTheFire.Top.Set(-160, 1);

            FannyTheFire.Height.Set(80, 0f);
            FannyTheFire.Width.Set(80, 0f);
            FannyTheFire.OnLeftClick += TickleTheRepugnantFuck;

            Append(FannyTheFire);

            FannyTextbox textbox = new FannyTextbox();

            textbox.Height.Set(0, 0f);
            textbox.Width.Set(0, 0f);
            textbox.ParentFanny = FannyTheFire;

            Append(textbox);
            Fanny.SpeechBubble = textbox;
        }

        private void TickleTheRepugnantFuck(UIMouseEvent evt, UIElement listeningElement)
        {
            Fanny fannyMyAmigo = (listeningElement as Fanny);
            fannyMyAmigo.tickle = Math.Max(fannyMyAmigo.tickle, 0) + 1;

            SoundEngine.PlaySound(SoundID.DD2_GoblinScream with { MaxInstances = 0 });
        }

        public override void Update(GameTime gameTime)
        {
            FannyTheFire.Left.Set(-(80 + 240 * MathF.Pow(fadeIn, 0.4f)), 1);
            FannyTheFire.Recalculate();
            
            if (!Main.playerInventory && !Fanny.UsedMessage.DisplayOutsideInventory)
            {
                fadeIn -= 0.05f;
                if (fadeIn < 0)
                    fadeIn = 0;
            }
            else
            {
                fadeIn += 0.05f;
                if (fadeIn > 1)
                    fadeIn = 1;
            }

            if (fadeIn == 1 && Fanny.needsToShake)
            {
                Fanny.needsToShake = false;

                if (Main.playerInventory)
                    Fanny.shakeTime = 1f;
            }
            if (Fanny.shakeTime > 0)
            {
                Fanny.shakeTime -= 1 / (60f * 1f);
            }

            FannyTheFire.bounce -= 1 / (60f * 0.4f);

            FannyTheFire.tickle -= 1 / (60f * 0.4f);
            if (FannyTheFire.tickle > 4)
                FannyTheFire.tickle -= 1 / (60f * 0.4f);
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            if (fadeIn > 0)
                base.Draw(spriteBatch);
        }
    }

    // This class will only be autoloaded/registered if we're not loading on a server
    [Autoload(Side = ModSide.Client)]
    internal class FannyUISystem : ModSystem
    {
        private UserInterface FannyInterface;
        internal FannyUIState UIState;

        public override void Load()
        {
            FannyInterface = new UserInterface();
            UIState = new FannyUIState();
            FannyInterface.SetState(UIState);
        }

        public override void UpdateUI(GameTime gameTime)
        {
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars")); 
            if (resourceBarIndex != -1)
            {
                layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                    "CalRemix: Fanny",
                    delegate {

                        FannyInterface?.Update(Main._drawInterfaceGameTime);
                        FannyInterface.Draw(Main.spriteBatch, Main._drawInterfaceGameTime);
                        return true;
                    },
                    InterfaceScaleType.None)
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

            NoMessage = new FannyMessage("", "Idle", displayOutsideInventory: false);
        }


        public override void PostUpdateEverything()
        {
            if (Main.dedServ)
                return;

            Fanny.talkCooldown--;

            /*
            if (Main.mouseRight && Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift))
            {
                Fanny.StopTalking();
                fannyMessages.Clear();
                LoadGeneralFannyMessages();
            }
            */

            //Tick down message times
            for (int i = 0; i < fannyMessages.Count; i++)
            {
                FannyMessage msg = fannyMessages[i];

                if (msg.CooldownTime > 100)
                    msg.CooldownTime = 100;


                //Tick down the cooldown
                if (msg.CooldownTime > 0)
                    msg.CooldownTime--;

                //Otherwise
                else if (msg.TimeLeft > 0)
                {
                    //Tick down only if either we don't need clicking off, or we were already clicked on (timeleft 30 and under)
                    if (!msg.NeedsToBeClickedOff || msg.TimeLeft <= 30)
                        msg.TimeLeft--;

                    //Message stays in stasis if it needs to be clicked off
                    if (msg.NeedsToBeClickedOff && msg.TimeLeft == 30)
                        msg.TimeLeft = 31;
                }

                else if (Fanny.UsedMessage == msg)
                {
                    Fanny.StopTalking();
                    msg.StartCooldown();
                }
            }

            //Don't even try looking for a new message if speaking
            if (Fanny.Speaking || Fanny.talkCooldown > 0)
                return;

            FannySceneMetrics scene = new FannySceneMetrics();

            //Precalculate screen NPCs to avoid repeated checks over all npcs everytime
            Rectangle screenRect = new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight);
            scene.onscreenNPCs = Main.npc.Where(n => n.active && n.Hitbox.Intersects(screenRect));

            

            foreach (FannyMessage message in fannyMessages)
            {
                if (message.CanPlayMessage() && message.Condition(scene))
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
            FannyPortrait.LoadPortrait("Nuhuh", 19);
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
                "Idle", FannyMessage.AlwaysShow, onlyPlayOnce:true, displayOutsideInventory: true));

            fannyMessages.Add(new FannyMessage("Na Na Na! The big robotic forge needs a lot of blue meat from the ads! It cannot work without it!",
                "Nuhuh", HasDraedonForgeMaterialsButNoMeat));

            fannyMessages.Add(new FannyMessage("Fear the Meld Gunk.", 
                "Idle", (FannySceneMetrics scene) => Main.hardMode && Main.LocalPlayer.InModBiome<UndergroundAstralBiome>(), cooldown: 120));

            fannyMessages.Add(new FannyMessage("Oooh! Delicious Meat! Collect as much as you can, it will save you a lot of time.", "Awooga",
                (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<DeliciousMeat>())));

            fannyMessages.Add(new FannyMessage("It appears you're approaching the Dungeon. Normally this place is guarded by viscious guardians, but I've disabled them for you my dear friend.", "Nuhuh",
                NearDungeonEntrance, onlyPlayOnce: true));

            //Add a condition to this one yuh, to pass the test of knowledge...
            fannyMessages.Add(new FannyMessage("I hope you know what you've gotten yourself into... Go kill some Cnidrions instead.", "Nuhuh"));
           
        }

        #region Conditions for general messages
        public static bool HasDraedonForgeMaterialsButNoMeat(FannySceneMetrics scene)
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

        public static bool NearDungeonEntrance(FannySceneMetrics scene)
        {
            return Main.LocalPlayer.WithinRange(new Vector2(Main.dungeonX * 16, Main.dungeonY * 16), 700);
        }
        #endregion
    }


    public class FannySceneMetrics
    {
        public IEnumerable<NPC> onscreenNPCs;
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

        public delegate bool FannyMessageCondition(FannySceneMetrics sceneMetrics);
        public static bool NeverShow(FannySceneMetrics sceneMetrics) => false;
        public static bool AlwaysShow(FannySceneMetrics sceneMetrics) => true;

        public FannyMessageCondition Condition { get; set; }
        #endregion

        public int CooldownTime { get; set; }
        private int cooldownDuration;

        public int TimeLeft { get; set; }
        private int messageDuration;

        private bool alreadySeen = false;

        public bool DisplayOutsideInventory { get; set; }
        public bool OnlyPlayOnce { get; set; }
        public bool NeedsToBeClickedOff { get; set; }

        public FannyPortrait Portrait { get; set; }

        public FannyMessage(string message, string portrait = "", FannyMessageCondition condition = null, float duration = 5, float cooldown = 60, bool displayOutsideInventory = true, bool onlyPlayOnce = true, bool needsToBeClickedOff = true)
        {
            Text = message;
            Condition = condition ?? NeverShow;

            //Duration and cooldown are inputted in seconds and then converted into frames by the constructor
            cooldownDuration = (int)(cooldown * 60);
            CooldownTime = 0;
            
            messageDuration = (int)(duration * 60);
            TimeLeft = 0;

            DisplayOutsideInventory = displayOutsideInventory;
            OnlyPlayOnce = onlyPlayOnce;
            NeedsToBeClickedOff = needsToBeClickedOff;

            if (portrait == "")
                portrait = "Idle";
            Portrait = FannyManager.Portraits[portrait];
        }

        //Technically the TimeLeft is not needed because when its active, no other message will try to play. But just in case
        public bool CanPlayMessage()
        {
            //Can't play message if on cooldown, if already playing, if already played once before and we only wanted it to play once, or if the inventory is closed
            return CooldownTime <= 0 && TimeLeft <= 0 && (!OnlyPlayOnce || !alreadySeen) && (DisplayOutsideInventory || Main.playerInventory);
        }

        public void PlayMessage()
        {
            TimeLeft = messageDuration;
            Fanny.UsedMessage = this;
            Fanny.needsToShake = true;
            alreadySeen = true;

            SoundEngine.PlaySound(SoundID.MenuOpen);
            SoundEngine.PlaySound(SoundID.Cockatiel with { MaxInstances = 0, Volume = 0.3f, Pitch = -0.8f});
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

            formattedText = formattedSetence;
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

    public class FannyPortrait
    {
        public Asset<Texture2D> Texture;
        public int frameCount;
        public int animationSpeed;

        public static void LoadPortrait(string portraitName, int frameCount, int animationSpeed = 12)
        {
            FannyPortrait portrait = new FannyPortrait(portraitName, frameCount, animationSpeed);
            //Load itself into the portrait list
            if (!FannyManager.Portraits.ContainsKey(portraitName))
                FannyManager.Portraits.Add(portraitName, portrait);
        }

        public FannyPortrait(string portrait, int frameCount, int animationSpeed = 12)
        {
            Texture = ModContent.Request<Texture2D>("CalRemix/UI/Fanny/Fanny" + portrait);
            this.frameCount = frameCount;
            this.animationSpeed = animationSpeed;
        }
    }
}