using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Furniture.CraftingStations;
using CalamityMod.Items.Potions;
using CalamityMod.NPCs.Yharon;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using SubworldLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace CalRemix.UI
{
    /// <summary>
    /// Represents the character / UI eleemnt of the character itself
    /// </summary>
    public class ScreenHelper : UIElement
    {
        public Vector2 BasePosition => GetDimensions().ToRectangle().Bottom();
        public ScreenHelperTextbox SpeechBubble;

        public float fadeIn;
        public bool flipped;
        public bool idlesInInventory;

        public float distanceFromEdge = 240;


        private int helperFrame;
        private int helperFrameCounter;

        //Timer that ticks down after a message has been spoken. Lasts one second
        public int talkCooldown;

        //Default placeholder message used when not speaking
        public HelperMessage NoMessage;

        //Non-default message currently being spoken
        private HelperMessage currentMessage;

        /// <summary>
        /// Checks if currently speaking a message. Ignores messages that the helper is already "speaking" but haven't showed yet due to a delay time
        /// </summary>
        public bool Speaking => currentMessage != null && !currentMessage.InDelayPeriod;

        /// <summary>
        /// The screen helper's currently used message. If not speaking, returns the default empty message
        /// </summary>
        public HelperMessage UsedMessage
        {
            get => Speaking ? currentMessage : NoMessage;
            set
            {
                currentMessage = value;
                SpeechBubble.Recalculate();
            }
        }

        /// <summary>
        /// Basic checks to know if the helper can even talk, irregardless of what the message is<br/>
        /// Checks that they're currently not on cooldown, and that theyre not already talking
        /// </summary>
        public bool CanSpeak => currentMessage == null && talkCooldown <= 0;

        /// <summary>
        /// Plays the desired message, ignoring any extra condition the message may have <br/>
        /// It still checks for if the fanny can speak all, and for the generic play conditions of the message (cooldown, if its already being played, etc)
        /// </summary>
        public void TalkAbout(HelperMessage message)
        {
            if (!CanSpeak || !message.CanPlayMessage())
                return;

            message.PlayMessage(this);
        }

        /// <summary>
        /// Stops talking about the current message and goes on cooldown for one second until another message can be played
        /// </summary>
        public void StopTalking()
        {
            currentMessage = null;
            talkCooldown = 60;
        }


        #region Style
        /// <summary>
        /// Speaking sound for the helper's messages. Can be overriden by <see cref="HelperMessage.voiceOverride"/>
        /// </summary>
        public SoundStyle speakingSound;
        /// <summary>
        /// Hover text for the helper's textboxes. Can be overriden by <see cref="HelperMessage.hoverTextOverride"/>
        /// </summary>
        public string textboxHoverText;
        /// <summary>
        /// Palette for the helper's textboxes. Can be overriden by <see cref="HelperMessage.paletteOverride"/>
        /// </summary>
        public HelperTextboxPalette textboxPalette;
        /// <summary>
        /// Sound played when the helper is tickled by clicking on them. Doesnt play if <see cref="canBeTickled"/> is set to false <br/>
        /// Doesn't play any sounds if null
        /// </summary>
        public SoundStyle? tickleSound = null;

        /// <summary>
        /// The palette currently used by the helper. This uses the palette of the message's override if one is present
        /// </summary>
        public HelperTextboxPalette UsedPalette => (currentMessage != null && currentMessage.paletteOverride.HasValue) ? currentMessage.paletteOverride.Value : textboxPalette;

        /// <summary>
        /// Sets the voices of the helper
        /// </summary>
        public ScreenHelper SetVoiceStyle(SoundStyle speakingSound, SoundStyle? tickleSound = null)
        {
            this.speakingSound = speakingSound;
            if (tickleSound.HasValue)
                this.tickleSound = tickleSound.Value with { MaxInstances = 0 };
            return this;
        }

        /// <summary>
        /// Sets the default hover text and palette of the character's textbox
        /// </summary>
        public ScreenHelper SetTextboxStyle(string textboxHoverText, HelperTextboxPalette? palette = null)
        {
            this.textboxHoverText = textboxHoverText;

            if (palette.HasValue)
                this.textboxPalette = palette.Value;
            else
                textboxPalette = new HelperTextboxPalette();
            return this;
        }
        #endregion

        #region Bounce, tickle, and shake anims
        //Bounce is the aniamtion that plays when the character is hovered
        //Tickle is the animation that plays when the character is clicked
        //Shake is the animation that plays when the character plays a message that was already visible in the inventory
        public float bounce;
        public float tickle;
        public bool needsToShake;
        public float shakeTime;

        public bool canBeTickled = true;
        public bool canBounce = true;
        public bool canShake = true;

        /// <summary>
        /// Sets if the screen helper can be tickled on click, if it bounces on hover, and if it shakes when speaking and the inventory was already open
        /// </summary>
        public ScreenHelper SetExtraAnimations(bool bouncesOnHover = true, bool ticklesOnClick = true, bool shakesWhenSpeaking = true)
        {
            canBounce = bouncesOnHover;
            canBeTickled = ticklesOnClick;
            canShake = shakesWhenSpeaking;
            return this;
        }
        #endregion

        #region Specific availability conditions
        public delegate bool CharacterCondition();
        private CharacterCondition unlockCondition;
        /// <summary>
        /// Checks if the character is available to speak or not: For example, Evil Fanny only appears after hardmode<br/>
        /// Set with <see cref="SetAvailabilityCondition(CharacterCondition)"/>. Can be ignored by specific messages with <see cref="HelperMessage.IgnoreSpeakerSpecificCondition"/>
        /// </summary>
        public bool CharacterAvailableCondition => unlockCondition == null || unlockCondition();

        /// <summary>
        /// Sets a specific condition that needs to be fulfilled for the character to speak: For example, Evil Fanny only appears after hardmode<br/>
        ///  Can be ignored by specific messages with <see cref="HelperMessage.IgnoreSpeakerSpecificCondition"/>
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public ScreenHelper SetAvailabilityCondition(CharacterCondition condition)
        {
            unlockCondition = condition;
            return this;
        }
        #endregion

        public override void OnInitialize()
        {
            OnLeftClick += OnClickHelper;
        }

        private void OnClickHelper(UIMouseEvent evt, UIElement listeningElement)
        {
            if (!ScreenHelperMessageManager.fannyEnabled)
                return;



            if (canBeTickled && tickleSound.HasValue)
            {
                tickle = Math.Max(tickle, 0) + 1;
                SoundEngine.PlaySound(tickleSound.Value);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (!ScreenHelperMessageManager.fannyEnabled)
            {
                StopTalking();
            }
            //Tick down the talk cooldown
            talkCooldown--;

            //Slide
            if (!flipped)
                Left.Set(-(80 + distanceFromEdge * MathF.Pow(fadeIn, 0.4f)), 1);
            else
                Left.Set(distanceFromEdge * MathF.Pow(fadeIn, 0.4f), 0);

            Recalculate();


            //Show show if there's currently a message (if it's a message only shown in the inventory, only show it there)
            //Additionally, always show with the inventory open as long as its not hidden by default
            bool shouldShow = (Main.playerInventory && idlesInInventory) ||
                (currentMessage != null && (Main.playerInventory || UsedMessage.DisplayOutsideInventory) && !currentMessage.InDelayPeriod);


            //Slides in and out
            if (!shouldShow)
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


            //Shake if fanny choses a new message while already out there in the inventory
            if (needsToShake)
            {
                needsToShake = false;
                if (Main.playerInventory && fadeIn == 1 && canShake)
                    shakeTime = 1f;
            }
            if (shakeTime > 0)
                shakeTime -= 1 / (60f * 1f);

            //Tick down bounce & tickle anims
            bounce -= 1 / (60f * 0.4f);

            tickle -= 1 / (60f * 0.4f);
            if (tickle > 4)
                tickle -= 1 / (60f * 0.4f);

            base.Update(gameTime);
        }

        #region Drawing
        // Here we draw our UI
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (!ScreenHelperMessageManager.fannyEnabled)
            {
                return;
            }
            // find the player's latest discord chat while the game isn't opened. doesn't work in multiplayer for sanity reasons
            if (NPC.downedBoss1)
            {
                if (Main.netMode == NetmodeID.SinglePlayer && !(ScreenHelperMessageManager.discord1.alreadySeen || ScreenHelperMessageManager.discord2.alreadySeen) && !Main.hasFocus)
                {
                    ScreenHelperMessageManager.GetDiscord();
                }
            }
            AnimateFanny();

            // finally draw Fanny
            Texture2D fannySprite = UsedMessage.Portrait.Texture.Value;
            Rectangle frame = fannySprite.Frame(1, UsedMessage.Portrait.frameCount, 0, helperFrame);
            Vector2 position = BasePosition;

            //Shake motion when fanny picks a message to play when he was already showing in the inventory
            if (shakeTime > 0)
            {
                position += Main.rand.NextVector2Circular(1f, 1f) * 5f * Utils.GetLerpValue(0.6f, 1f, shakeTime, true);

                float bounceTime = Utils.GetLerpValue(0.6f, 1f, shakeTime, true);
                position.Y -= Math.Abs(MathF.Sin(bounceTime * MathHelper.TwoPi)) * 62f * MathF.Pow(bounceTime, 0.6f);
            }

            //Bouncing up and down when hovered over
            else if (canBounce && (ContainsPoint(Main.MouseScreen) || bounce > 0))
            {
                if (bounce < 0)
                    bounce = 1;

                position.Y -= MathF.Pow(Math.Abs(MathF.Sin(bounce * MathHelper.Pi)), 0.6f) * 22f;
            }
            

            //Stupid fuck
            if (tickle >= 1)
                position += Main.rand.NextVector2Circular(3f, 3f) * tickle;


            spriteBatch.Draw(fannySprite, position, frame, Color.White * fadeIn, 0, new Vector2(frame.Width / 2f, frame.Height), 1f, 0, 0);
            if (Speaking && UsedMessage.ItemType != -22)
                DrawItem();
        }

        public void AnimateFanny()
        {
            helperFrameCounter++;
            if (helperFrameCounter > UsedMessage.Portrait.animationSpeed)
            {
                helperFrame++;
                helperFrameCounter = 0;
            }

            if (helperFrame >= UsedMessage.Portrait.frameCount)
                helperFrame = 0;
        }

        public void DrawItem()
        {
            Main.instance.LoadItem(UsedMessage.ItemType);
            Texture2D itemSprite = TextureAssets.Item[UsedMessage.ItemType].Value;
            int count = Main.itemAnimations[UsedMessage.ItemType] != null ? Main.itemAnimations[UsedMessage.ItemType].FrameCount : 1;
            Rectangle nframe = itemSprite.Frame(1, count, 0, 0);
            Vector2 origin = nframe.Size() / 2f;
            Vector2 itemPosition = BasePosition + new Vector2(60 * (flipped ? -1 : 1), -60) + UsedMessage.ItemOffset + Vector2.UnitY * MathF.Sin(Main.GlobalTimeWrappedHourly * 2) * 4;

            Main.EntitySpriteDraw(itemSprite, itemPosition, nframe, Color.White * fadeIn, MathF.Sin(Main.GlobalTimeWrappedHourly * 2 + MathHelper.PiOver2) * 0.2f, origin, UsedMessage.ItemScale, SpriteEffects.None);
        }
        #endregion
    }

    public class ScreenHelperTextbox : UIElement
    {
        public ScreenHelper ParentSpeaker;
        public int outlineThickness = 3;
        public int backgroundPadding = 10;

        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);

            //Set the timeleft of the message to 30
            if (ParentSpeaker.Speaking && ParentSpeaker.fadeIn == 1 && ParentSpeaker.UsedMessage.NeedsToBeClickedOff && ParentSpeaker.UsedMessage.TimeLeft > 30)
                ParentSpeaker.UsedMessage.TimeLeft = 30;
        }

        public override void Recalculate()
        {
            Vector2 offset = new Vector2(50, 90);
            if (ParentSpeaker.flipped)
                offset.X *= -1;

            Vector2 basePosition = ParentSpeaker.BasePosition - offset;
            Vector2 textSize = FontAssets.MouseText.Value.MeasureString(ParentSpeaker.UsedMessage.Text) * ParentSpeaker.UsedMessage.textSize;
            Vector2 cornerPosition = basePosition - textSize;

            if (ParentSpeaker.flipped)
                cornerPosition = basePosition - Vector2.UnitY * textSize.Y;

            //Account for padding
            textSize += Vector2.One * (backgroundPadding + outlineThickness) * 2;
            cornerPosition -= Vector2.One * (backgroundPadding + outlineThickness);

            //Fade out
            cornerPosition.Y -= MathF.Pow(Utils.GetLerpValue(30, 0, ParentSpeaker.UsedMessage.TimeLeft, true), 1.6f) * 30f;
            //Fade in
            cornerPosition.Y += MathF.Pow(1 - ParentSpeaker.fadeIn, 2f) * 40;

            Width.Set(textSize.X, 0);
            Height.Set(textSize.Y, 0);
            Left.Set(cornerPosition.X, 0);
            Top.Set(cornerPosition.Y, 0);
            base.Recalculate();
        }


        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (!ParentSpeaker.Speaking)
                return;

            HelperTextboxPalette palette = ParentSpeaker.UsedPalette;

            // a shit ton of variables
            var font = FontAssets.MouseText.Value;
            string text = ParentSpeaker.UsedMessage.Text;

            Rectangle dimensions = GetDimensions().ToRectangle();

            Vector2 outlineDrawPosition = dimensions.TopLeft();
            Vector2 backgroundDrawPosition = outlineDrawPosition + Vector2.One * outlineThickness;
            Vector2 textDrawPosition = backgroundDrawPosition + Vector2.One * backgroundPadding;

            Vector2 outlineSize = dimensions.Size();
            Vector2 backgroundSize = outlineSize - Vector2.One * outlineThickness * 2;

            Texture2D squareTexture = TextureAssets.MagicPixel.Value;
            float opacity = ParentSpeaker.fadeIn * Utils.GetLerpValue(0, 30, ParentSpeaker.UsedMessage.TimeLeft, true);

            // draw the border as a large rectangle behind, and the inners as a slightly smaller rectangle infront
            Main.spriteBatch.Draw(squareTexture, outlineDrawPosition, null, palette.outline * opacity, 0, Vector2.Zero, outlineSize / squareTexture.Size(), 0, 0);
            Main.spriteBatch.Draw(squareTexture, backgroundDrawPosition, null, palette.background * opacity, 0, Vector2.Zero, backgroundSize / squareTexture.Size(), 0, 0);


            if (ContainsPoint(Main.MouseScreen) && ParentSpeaker.fadeIn == 1 && ParentSpeaker.UsedMessage.NeedsToBeClickedOff && ParentSpeaker.UsedMessage.TimeLeft > 30)
            {
                Main.spriteBatch.Draw(squareTexture, backgroundDrawPosition, null, palette.backgroundHover with { A = 0 } * (0.4f + 0.2f * MathF.Sin(Main.GlobalTimeWrappedHourly * 4f)) * opacity, 0, Vector2.Zero, backgroundSize / squareTexture.Size(), 0, 0);
                Main.LocalPlayer.mouseInterface = true;

                string hoverText = ParentSpeaker.textboxHoverText;
                if (ParentSpeaker.UsedMessage.hoverTextOverride != "")
                    hoverText = ParentSpeaker.UsedMessage.hoverTextOverride;
                Main.instance.MouseText(hoverText);
            }

            // finally draw the text
            Utils.DrawBorderStringFourWay(Main.spriteBatch, font, text, textDrawPosition.X, textDrawPosition.Y, palette.text * (Main.mouseTextColor / 255f) * opacity, palette.textOutline * opacity, Vector2.Zero, ParentSpeaker.UsedMessage.textSize);
        }
    }

    public class ScreenHelpersUIState : UIState
    {
        public static ScreenHelper FannyTheFire = new ScreenHelper();
        public static ScreenHelper EvilFanny = new ScreenHelper();
        public static ScreenHelper WonderFlower = new ScreenHelper();
        public static ScreenHelper GonerFanny = new ScreenHelper();

        public override void OnInitialize()
        {
            LoadScreenHelper(FannyTheFire, false, true, "FannyIdle")
                .SetVoiceStyle(SoundID.Cockatiel with { MaxInstances = 0, Volume = 0.3f, Pitch = -0.8f }, SoundID.DD2_GoblinScream)
                .SetTextboxStyle("Thank you for the help, Fanny!");
            
            LoadScreenHelper(EvilFanny, true, false, "EvilFannyIdle", distanceFromEdge: 120)
                .SetVoiceStyle(SoundID.DD2_DrakinShot with { MaxInstances = 0, Volume = 0.3f, Pitch = 0.8f }, SoundID.DD2_GoblinScream)
                .SetTextboxStyle("Get away, Evil Fanny!", new HelperTextboxPalette(Color.Black, Color.Red, Color.Indigo, Color.DeepPink, Color.Tomato));

            LoadScreenHelper(WonderFlower,  false, false, "TalkingFlower", verticalOffset: 0.3f, distanceFromEdge: 240)
                .SetVoiceStyle(ScreenHelperMessageManager.WonderFannyVoice, SoundID.DD2_GoblinScream)
                .SetTextboxStyle("Oooh! So exciting!", new HelperTextboxPalette(Color.Black, Color.Transparent, new Color(250, 250, 250), Color.White, Color.Black * 0.4f));

            LoadScreenHelper(GonerFanny, false, false, "FannyGoner", verticalOffset: 0.275f, distanceFromEdge: 840)
                .SetVoiceStyle(ScreenHelperMessageManager.HappySFX with { MaxInstances = 0 })
                .SetTextboxStyle("     ", new HelperTextboxPalette(Color.Gray, Color.Gray, Color.Gray, Color.Gray, Color.Black))
                .SetExtraAnimations(false, false, false);

        }

        /// <summary>
        /// Loads a screen helper
        /// </summary>
        /// <param name="helper">The empty UI object to load the values into</param>
        /// <param name="flipped">If the screen helper slides from the left instead of from the right of the screen</param>
        /// <param name="idlesInInventory">If the screen helper shows with the inventory open, even when not speaking</param>
        /// <param name="emptyMessagePortrait">The default portrait used when not speaking</param>
        /// <param name="verticalOffset">How high up on the screen this fanny is</param>
        /// <param name="distanceFromEdge">How far away from the edge of the screen this fanny is</param>
        /// <returns></returns>
        public ScreenHelper LoadScreenHelper(ScreenHelper helper, bool flipped, bool idlesInInventory, string emptyMessagePortrait, float verticalOffset = 0f, float distanceFromEdge = 240f)
        {
            helper.Left.Set(-80, 1);
            helper.Top.Set(-160, 1 - verticalOffset);
            helper.Height.Set(80, 0f);
            helper.Width.Set(80, 0f);

            Append(helper);

            ScreenHelperTextbox textbox = new ScreenHelperTextbox();
            textbox.Height.Set(0, 0f);
            textbox.Width.Set(0, 0f);
            textbox.ParentSpeaker = helper;
            Append(textbox);
            helper.SpeechBubble = textbox;

            helper.flipped = flipped;
            helper.idlesInInventory = idlesInInventory;
            helper.NoMessage = new HelperMessage("", "", emptyMessagePortrait, displayOutsideInventory: false);
            helper.distanceFromEdge = distanceFromEdge;

            return helper;
        }

        public void StopAllDialogue()
        {
            foreach (UIElement element in Elements)
            {
                if (element is ScreenHelper speaker)
                    speaker.StopTalking();
            }
        }

        public bool AnyAvailableScreenHelper()
        {
            return Elements.Any(ui => ui is ScreenHelper helper && helper.CanSpeak);
        }
    }

    // This class will only be autoloaded/registered if we're not loading on a server
    [Autoload(Side = ModSide.Client)]
    internal class ScreenHelperUISystem : ModSystem
    {
        private UserInterface FannyInterface;
        internal static ScreenHelpersUIState UIState;

        public override void Load()
        {
            FannyInterface = new UserInterface();
            UIState = new ScreenHelpersUIState();
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
                    InterfaceScaleType.UI)
                );
            }
        }
    }

    public class ScreenHelperSavePlayer : ModPlayer
    {
        public bool[] readMessages;

        public bool readTroughFannyDogDialogue = false;

        public override void SaveData(TagCompound tag)
        {
            for (int i = 0; i < ScreenHelperMessageManager.screenHelperMessages.Count; i++)
            {
                HelperMessage msg = ScreenHelperMessageManager.screenHelperMessages[i];

                if (msg.alreadySeen && msg.PersistsThroughSaves)
                    tag["FannyDialogue" + msg.Identifier] = true;

                //update readmessages array, so that when the player enters a world without loading data, it is up to date
                //nullcheck cuz savedata happens at the start when presenting players
                if (readMessages != null && msg.alreadySeen)
                    readMessages[i] = true;
            }

            tag["FannyReadThroughDogDialogue"] = ScreenHelperMessageManager.ReadAllDogTips;
        }


        public override void LoadData(TagCompound tag)
        {
            readMessages = new bool[ScreenHelperMessageManager.screenHelperMessages.Count];
            for (int i = 0; i < ScreenHelperMessageManager.screenHelperMessages.Count; i++)
            {
                HelperMessage msg = ScreenHelperMessageManager.screenHelperMessages[i];
                readMessages[i] = tag.ContainsKey("FannyDialogue" + msg.Identifier);
            }

            if (tag.TryGet<bool>("FannyReadThroughDogDialogue", out bool readDog))
                readTroughFannyDogDialogue = readDog;
        }

        public override void OnEnterWorld()
        {
            if (Main.mouseRight && Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift) && Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space))
            {
                SoundEngine.PlaySound(SoundID.Cockatiel);
                SoundEngine.PlaySound(SoundID.DD2_GoblinScream);
                SoundEngine.PlaySound(SoundID.DD2_KoboldExplosion);
                return;
            }

            if (readMessages is null || readMessages.Length < ScreenHelperMessageManager.screenHelperMessages.Count)
            {

                for (int i = 0; i < ScreenHelperMessageManager.screenHelperMessages.Count; i++)
                {
                    HelperMessage msg = ScreenHelperMessageManager.screenHelperMessages[i];
                    msg.alreadySeen = false;
                }
                return;
            }


            for (int i = 0; i < ScreenHelperMessageManager.screenHelperMessages.Count; i++)
            {
                HelperMessage msg = ScreenHelperMessageManager.screenHelperMessages[i];
                msg.alreadySeen = readMessages[i];
            }

            ScreenHelperMessageManager.ReadAllDogTips = readTroughFannyDogDialogue;
        }
    }

    [Autoload(Side = ModSide.Client)]
    public partial class ScreenHelperMessageManager : ModSystem
    {
        /// <summary>
        /// List that contains all the messages spoken by screen helpers, in the order they're loaded
        /// </summary>
        public static List<HelperMessage> screenHelperMessages = new List<HelperMessage>();
        /// <summary>
        /// Contains the screen helpers messages grouped by speaker. Created after all messages have been loaded
        /// </summary>
        public static IEnumerable<IGrouping<ScreenHelper, HelperMessage>> screenHelperMessageGroups; 
        /// <summary>
        /// Dictionnary of screen helper portraits
        /// </summary>
        public static Dictionary<string, ScreenHelperPortrait> Portraits = new Dictionary<string, ScreenHelperPortrait>();

        public static bool fannyEnabled = true;
        public static int fannyTimesFrozen = 0;
        public static ScreenHelperSceneMetrics sceneMetrics;
        public static Rectangle screenRect;

        #region Loading
        public override void Load()
        {
            LoadFannyPortraits();
            LoadGeneralFannyMessages();
            LoadDogSpamMessages();
            LoadLoreComments();
            LoadIntroMessages();
            LoadPassiveMessages();
            LoadEventMessages();
            LoadItemMessages();
            LoadBiomeMessages();
            LoadShrineMessages();
            LoadNPCMessages();
            LoadBossMessages();
            LoadBossDeathMessages();
            LoadEvilGFBMessages();
            LoadEvilFannyIntro();
            LoadMoonLordDeath();
            LoadBabil();
            //LoadPityParty();
            LoadWonderFlowerMessages();

            fannyEnabled = true;
            fannyTimesFrozen = 0;
            sceneMetrics = new ScreenHelperSceneMetrics();
        }

        public static void LoadFannyPortraits()
        {
            //Fanny
            ScreenHelperPortrait.LoadPortrait("FannyIdle", 8);
            ScreenHelperPortrait.LoadPortrait("FannyAwooga", 4);
            ScreenHelperPortrait.LoadPortrait("FannyCryptid", 1);
            ScreenHelperPortrait.LoadPortrait("FannySob", 4);
            ScreenHelperPortrait.LoadPortrait("FannyNuhuh", 19);
            ScreenHelperPortrait.LoadPortrait("FannyStare", 1);
            ScreenHelperPortrait.LoadPortrait("FannyGoner", 1);

            //Evil Fanny
            ScreenHelperPortrait.LoadPortrait("EvilFannyIdle", 1);

            //Talking Flower
            ScreenHelperPortrait.LoadPortrait("TalkingFlower", 11, 5);
        }

        /// <summary>
        /// Registers a message for a screen helper to speak
        /// You can either provide a condition to the message, in which case the message will automatically play when the condition is met <br/>
        /// Alternatively, you could cache the message, and play it yourself when needed using <see cref="ScreenHelper.TalkAbout(HelperMessage)"/>
        /// </summary>
        public static HelperMessage LoadFannyMessage(HelperMessage message)
        {
            screenHelperMessages.Add(message);
            return message;
        }

        public override void PostSetupContent()
        {
            //Now that the messages have been created, sort them by speaker 
            screenHelperMessageGroups = screenHelperMessages.GroupBy(m => m.DesiredSpeaker);
        }
        #endregion

        public override void PreUpdateTime()
        {
            //Clears out the scene metrics and set the screen rect
            sceneMetrics.Clear();
            screenRect = new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight);
        }

        public override void PostUpdateEverything()
        {
            if (Main.dedServ)
                return;

            UpdateLoreCommentTracking();
            UpdateMessages();

            //Don't even try looking for a new message if everyone is speaking already / On speak cooldown
            if (ScreenHelperUISystem.UIState.AnyAvailableScreenHelper())
            {
                //Go through the messages grouped per helper
                foreach (var messageGroup in screenHelperMessageGroups)
                {
                    ScreenHelper speaker = messageGroup.First().DesiredSpeaker;

                    //Check if the helper in question can speak at all
                    if (!speaker.CanSpeak)
                        continue;

                    //Looks at all the messages
                    foreach (HelperMessage message in messageGroup)
                    {
                        if (message.CanPlayMessage() && message.CheckExtraConditions(sceneMetrics))
                        {
                            message.PlayMessage(speaker);
                            break;
                        }
                    }
                }
            }

            previousHoveredItem = Main.HoverItem.type;
        }

        public void UpdateMessages()
        {
            //Go through every message
            for (int i = 0; i < screenHelperMessages.Count; i++)
            {
                HelperMessage msg = screenHelperMessages[i];

                //Fallback
                if (msg.DesiredSpeaker == null)
                    msg.DesiredSpeaker = ScreenHelpersUIState.FannyTheFire;

                //Tick up messages that are queued to play with a timer
                if (msg.timerToPlay > 0)
                {
                    msg.timerToPlay++;

                    //if the message spent 10 seconds without playing after it should have, we stop trying to play it
                    //this is to avoid scenarios where the message's condition isnt met and it ends up hanging in the air forever until it is met
                    if (msg.timeToWaitBeforePlaying + 600 <= msg.timerToPlay)
                        msg.timerToPlay = 0;
                }

                //Tick down the cooldown before a message can repeat
                if (msg.CooldownTime > 0)
                    msg.CooldownTime--;

                //Otherwise tick down the timer
                else if (msg.TimeLeft > 0)
                {
                    msg.TimeLeft--;

                    //Message stays in stasis if it needs to be clicked off
                    if (msg.NeedsToBeClickedOff && msg.TimeLeft == 30)
                        msg.TimeLeft = 31;
                }

                //Stop talking
                else if (msg.currentSpeaker != null)
                {
                    msg.currentSpeaker.StopTalking();
                    msg.EndMessage();
                }

                //Call the start effects if the message has a delay (messages without a delay do the start effects when its played)
                if (msg.delayTime > 0 && msg.TimeLeft == msg.MessageDuration)
                    msg.OnMessageStart();
            }
        }

        #region Conditions for general messages
        public static bool HasDraedonForgeMaterialsButNoMeat(ScreenHelperSceneMetrics scene)
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

        public static bool NearDungeonEntrance(ScreenHelperSceneMetrics scene)
        {
            return Main.LocalPlayer.WithinRange(new Vector2(Main.dungeonX * 16, Main.dungeonY * 16), 700);
        }

        public static bool HasDesertMedallionMaterials(ScreenHelperSceneMetrics scene)
        {
            List<int> materials = new List<int>();
            {
                materials.Add(ItemID.AntlionMandible);
                materials.Add(ItemID.SandBlock);
                materials.Add(ModContent.ItemType<StormlionMandible>());
            }

            return Main.LocalPlayer.HasItems(materials) && !DownedBossSystem.downedDesertScourge;
        }
        public static bool HasRoxcaliburMaterials(ScreenHelperSceneMetrics scene)
        {
            List<int> materials = new List<int>();
            {
                materials.Add(ItemID.HellstoneBar);
                materials.Add(ItemID.SoulofNight);
                materials.Add(ModContent.ItemType<EssenceofHavoc>());
                materials.Add(ItemID.Obsidian);
                materials.Add(ItemID.StoneBlock);
                materials.Add(ItemID.Amethyst);
            }

            return Main.LocalPlayer.HasItems(materials) && !Main.hardMode;
        }

        public static bool CrossModNPC(ScreenHelperSceneMetrics scene, string ModName, string NPCName)
        {
            if (ModLoader.HasMod(ModName))
            {
                if (scene.onscreenNPCs.Any(n => n.type == ModLoader.GetMod(ModName).Find<ModNPC>(NPCName).Type))
                {
                    return true;
                }
                return false;
            }
            return false;
        }
        #endregion

        #region Saving and Loading data
        public override void ClearWorld()
        {
            //Stop all fannies talking, and close every dialogue
            ScreenHelperUISystem.UIState.StopAllDialogue();
            for (int i = 0; i < screenHelperMessages.Count; i++)
            {
                HelperMessage msg = screenHelperMessages[i];
                msg.timerToPlay = 0;
                msg.currentSpeaker = null;
                msg.TimeLeft = 0;
            }
        }
        #endregion
    }

    public class ScreenHelperSceneMetrics
    {
        public List<NPC> onscreenNPCs;
        //public List<Projectile> onscreenProjectiles;
        //public List<Item> onscreenItems;

        public ScreenHelperSceneMetrics()
        {
            onscreenNPCs = new List<NPC>(Main.maxNPCs);
            //onscreenProjectiles = new List<Projectile>(Main.maxProjectiles);
            //onscreenItems = new List<Item>(Main.maxItems);
        }

        public void Clear()
        {
            //Clear the NPCs
            onscreenNPCs.Clear();
            //Clear the Projectiles
            //onscreenProjectiles.Clear();
            //Clear the Items
            //onscreenItems.Clear();
        }
    }

    public class HelperMessage
    {
        private string originalText;
        private string formattedText;
        internal int maxTextWidth;
        public float textSize;

        public string Text
        {
            get => formattedText;
            set
            {
                //Cache the original text, and format it
                originalText = value;
                FormatText(FontAssets.MouseText.Value, maxTextWidth);
            }
        }

        //A unique identifier for the message that is used to save in the player data if the message was read or not
        public string Identifier;

        //We need to keep track of this to avoid playing the same message twice. This variable is set when the player loads into a world, based on saved player data
        public bool alreadySeen = false;


        public int CooldownTime { get; set; } //Current cooldown time for this message
        private int cooldownDuration; //How long the cooldown for this message lasts

        public int TimeLeft { get; set; } //Current timeleft for this message
        internal int messageDuration; //How long the message stayts on screen
        public int MessageDuration => messageDuration;


        /// <summary>
        /// The desired screen helper to play this message
        /// </summary>
        public ScreenHelper DesiredSpeaker;

        //The fanny actively speaking the message. For cases where we want one fanny to say what the other fanny says
        public ScreenHelper currentSpeaker;


        public bool DisplayOutsideInventory { get; set; } //Defaults to true
        public bool OnlyPlayOnce { get; set; } //Defaults to true
        public bool NeedsToBeClickedOff { get; set; } //Defaults to true
        public bool PersistsThroughSaves { get; set; } //Defaults to true
        public bool IgnoreSpeakerSpecificCondition { get; set; } //Defaults to false

        public ScreenHelperPortrait Portrait { get; set; } //The portrait used by the message

        public delegate string DynamicHelperMessageSegments();
        public static string GetPlayerName() => Main.LocalPlayer.name;
        public static string GetWorldName() => Main.worldName;

        public List<DynamicHelperMessageSegments> textSegments = new List<DynamicHelperMessageSegments>();


        public HelperMessage(string identifier, string message, string portrait = "", ScreenHelperMessageCondition condition = null, float duration = 5, float cooldown = 60, bool displayOutsideInventory = true, bool onlyPlayOnce = true, bool needsToBeClickedOff = true, bool persistsThroughSaves = true, int maxWidth = 380, float fontSize = 1f)
        {
            //Unique identifier for saving data
            Identifier = identifier;
            textSize = fontSize;

            maxTextWidth = maxWidth;
            Text = message;
            Conditions += condition ?? AlwaysShow;

            //Duration and cooldown are inputted in seconds and then converted into frames by the constructor
            cooldownDuration = (int)(cooldown * 60);
            CooldownTime = 0;

            messageDuration = (int)(duration * 60);
            TimeLeft = 0;

            DisplayOutsideInventory = displayOutsideInventory;
            OnlyPlayOnce = onlyPlayOnce;
            NeedsToBeClickedOff = needsToBeClickedOff;
            PersistsThroughSaves = persistsThroughSaves;

            if (portrait == "")
                portrait = "FannyIdle";
            Portrait = ScreenHelperMessageManager.Portraits[portrait];

            //default
            DesiredSpeaker = ScreenHelpersUIState.FannyTheFire;
        }

        /// <summary>
        /// Makes the message be spoken by evil fanny
        /// </summary>
        public HelperMessage SpokenByEvilFanny(bool ignoreHardmodeUnlockCondition = false)
        {
            DesiredSpeaker = ScreenHelpersUIState.EvilFanny;
            IgnoreSpeakerSpecificCondition = ignoreHardmodeUnlockCondition;
            return this;
        }

        /// <summary>
        /// Setups the message to be played by the specified screen helper
        /// </summary>
        /// <param name="speakingHelper">The helper who should play this message</param>
        /// <param name="ignoreHelperCondition">Ignores any extra conditions that would prevent a helped from playing (ie, Evil Fanny unlocking in Hardmode)</param>
        /// <returns></returns>
        public HelperMessage SpokenByAnotherHelper(ScreenHelper speakingHelper, bool ignoreHelperCondition = false)
        {
            DesiredSpeaker = speakingHelper;
            IgnoreSpeakerSpecificCondition = ignoreHelperCondition;
            return this;
        }

        #region Stylistic overrides
        public HelperTextboxPalette? paletteOverride = null;
        public string hoverTextOverride = "";
        public SoundStyle? voiceOverride = null;

        /// <summary>
        /// Adds a custom textbox palette override for this message
        /// </summary>
        public HelperMessage SetPalette(HelperTextboxPalette palette)
        {
            paletteOverride = palette;
            return this;
        }

        /// <summary>
        /// Adds a custom hover text for this message
        /// </summary>
        public HelperMessage SetHoverTextOverride(string hoverTextOverride)
        {
            this.hoverTextOverride = hoverTextOverride;
            return this;
        }

        /// <summary>
        /// Adds a custom sound for this message
        /// </summary>
        public HelperMessage SetSoundOverride(SoundStyle soundStyleOverride)
        {
            this.voiceOverride = soundStyleOverride;
            return this;
        }
        #endregion

        #region Message Conditions

        public delegate bool ScreenHelperMessageCondition(ScreenHelperSceneMetrics sceneMetrics);
        public static bool AlwaysShow(ScreenHelperSceneMetrics sceneMetrics) => true; //Default conditions

        public event ScreenHelperMessageCondition Conditions; //The list of conditions used for fanny's message to be played

        /// <summary>
        /// Checks if the extra conditions for the message to be played are met
        /// </summary>
        public bool CheckExtraConditions(ScreenHelperSceneMetrics sceneMetrics)
        {
            //Check every condition. If a condition isn't met, return false
            foreach (ScreenHelperMessageCondition condition in Conditions.GetInvocationList())
            {
                if (!condition(sceneMetrics))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Adds the condition that the message needs the player to be in a subworld for it to play
        /// </summary>
        /// <param name="subworld">The subworld the message is meant to play in. If null, the message only plays in the main world</param>
        public HelperMessage OnlyPlayInSubworld(Subworld subworld)
        {
            Conditions += (ScreenHelperSceneMetrics metrics) => subworld == SubworldSystem.Current;
            return this;
        }
        #endregion

        #region Message Activation & Chaining

        /// <summary>
        /// Timer used for messages that need activation. It's set to 1 by <see cref="ActivateMessage"/>, from which it starts counting up <br/>
        /// When the timer reaches <see cref="timeToWaitBeforePlaying"/>, the message can play
        /// </summary>
        public int timerToPlay = 0;
        /// <summary>
        /// Used by messages that need activation, set by <see cref="NeedsActivation()"/> and <see cref="ChainAfter(HelperMessage, float)"/> <br/>
        /// This is how long after activation the message plays. If no delay is specified, it is 1 frame
        /// </summary>
        public int timeToWaitBeforePlaying = 0;

        /// <summary>
        /// Makes it so that the message will never play on its own, and needs both its condition to be met, and <see cref="ActivateMessage"/> to be called for it to be read
        /// If the condition for the message isn't met, the message won't play even if activated
        /// </summary>
        public HelperMessage NeedsActivation()
        {
            timeToWaitBeforePlaying = 1;
            return this;
        }

        /// <summary>
        /// Makes it so that the message doesn't play on its own, and needs to be manually called by another message or event to play, using <see cref="ActivateMessage"/><br/>
        /// If the condition for the message isn't met, the message won't play even if activated
        /// </summary>
        /// <param name="timer">Delay period after <see cref="ActivateMessage"/> is called where the message waits to play, in seconds</param>
        public HelperMessage NeedsActivation(float delay = 1)
        {
            timeToWaitBeforePlaying = Math.Max(1, (int)(delay * 60));
            return this;
        }

        /// <summary>
        /// Manually activates a message, making it able to be played if its condition is met
        /// To make a message need manual activation, use <see cref="NeedsActivation"/>
        /// </summary>
        public void ActivateMessage()
        {
            //Increases the timer by 1, which makes it start counting up
            timerToPlay++;
        }

        /// <summary>
        /// Sets a message to be played after another message plays
        /// </summary>
        /// <param name="chainFrom">The message that this message is spoken after</param>
        /// <param name="delay">The delay between the first message being spoken, and this one appearing</param>
        /// <param name="startTimerOnMessageSpoken">By default, the delay timer starts when the original message is clicked off. Set this to true so the timer starts as soon as the first message appears</param>
        /// <returns></returns>
        public HelperMessage ChainAfter(HelperMessage chainFrom, float delay = 1f, bool startTimerOnMessageSpoken = false)
        {
            //Sets the message to need the activation of the first message
            NeedsActivation(delay);

            //Sets the first message to activate this one after tis played
            if (startTimerOnMessageSpoken)
                chainFrom.AddStartEvent(this.ActivateMessage);
            else
                chainFrom.AddEndEvent(this.ActivateMessage);

            return this;
        }
        #endregion

        #region Message Delay

        public int delayTime = 0;
        public bool InDelayPeriod => TimeLeft > messageDuration;

        /// <summary>
        /// Adds a delay time before the message starts playing, when its activation condition is met<br/>
        /// No other message can play while the delay period is started
        /// </summary>
        /// <param name="delay"></param>
        /// <returns></returns>
        public HelperMessage AddDelay(float delay = 1)
        {
            delayTime = (int)(delay * 60);
            return this;
        }
        #endregion

        #region Playing messages
        public event Action OnStart;
        public event Action OnEnd;

        /// <summary>
        /// Adds an action that happens when the message is being read
        /// </summary>
        public HelperMessage AddStartEvent(Action action)
        {
            OnStart += action;
            return this;
        }

        /// <summary>
        /// Adds an action that happens when the message goes away
        /// </summary>
        public HelperMessage AddEndEvent(Action action)
        {
            OnEnd += action;
            return this;
        }

        /// <summary>
        /// Check for generic shared conditions on messages, such as cooldown, if its already been played, if it only displays in inventory etc...
        /// </summary>
        public bool CanPlayMessage()
        {
            return CooldownTime <= 0 &&                                                             //Can't play messages on cooldown
                   TimeLeft <= 0 &&                                                                 //Can't play messages that are already playing
                   (!OnlyPlayOnce || !alreadySeen) &&                                               //Can't play messages that are only played once, more than once
                   (DisplayOutsideInventory || Main.playerInventory) &&                             //Can't play messages that only display in the inventory outside of the inventory
                   timeToWaitBeforePlaying <= timerToPlay &&                                        //Can't play messages with a timer before the timer is reached
                   (DesiredSpeaker.CharacterAvailableCondition || IgnoreSpeakerSpecificCondition);  //Can't play messages if the speaker isn't avaialble

            //Technically the TimeLeft is not needed because when its active, no other message will try to play. But just in case
        }

        /// <summary>
        /// Makes the desired <see cref="ScreenHelper"/> speak this message <br/>
        /// Sets the message duration, checks it off as seen, and calls the <see cref="OnMessageStart"/> events
        /// </summary>
        public void PlayMessage(ScreenHelper fanny)
        {
            TimeLeft = messageDuration + delayTime;
            fanny.UsedMessage = this;
            currentSpeaker = fanny;
            alreadySeen = true;

            //Recalculate the text as its played if we have dynamic text segments
            if (textSegments.Count > 0)
                FormatText(FontAssets.MouseText.Value, maxTextWidth);

            //Immediately play message start effects if the message doesnt have a delay
            if (delayTime == 0)
                OnMessageStart();
        }

        public void OnMessageStart()
        {
            if (currentSpeaker is null)
                return;

            currentSpeaker.needsToShake = true;
            SoundEngine.PlaySound(SoundID.MenuOpen);
            SoundEngine.PlaySound(voiceOverride ?? currentSpeaker.speakingSound);
            OnStart?.Invoke();
        }

        public void EndMessage()
        {
            OnEnd?.Invoke();
            CooldownTime = cooldownDuration;
            currentSpeaker = null;

            //Reset timer to play if we want to play it again later
            timerToPlay = 0;
        }
        #endregion



        #region Item display
        public int ItemType { get; set; } = -22;
        public float ItemScale { get; set; } = 1f;
        public Vector2 ItemOffset { get; set; } = Vector2.Zero;
        public HelperMessage AddItemDisplay(int itemType, float itemScale = 1f, Vector2? itemOffset = null)
        {
            ItemType = itemType;
            ItemScale = itemScale;
            ItemOffset = itemOffset ?? Vector2.Zero;
            return this;
        }
        #endregion

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
            string baseText = originalText;
            if (textSegments.Count > 0)
            {
                int i = 0;
                foreach (DynamicHelperMessageSegments dynamicText in textSegments)
                {
                    baseText = baseText.Replace("$" + i.ToString(), dynamicText());
                    i++;
                }
            }


            for (int i = 0; i < baseText.Length; i++)
            {
                //When theres a space, check if the word is short enough to not go over our width limit
                if (baseText[i] == ' ')
                {
                    CheckWord(currentWord, maxLineWidth, font, ref formattedSetence, ref currentLineLenght, spaceWidth);
                    currentWord = "";
                }

                //Continue to build the word
                else
                    currentWord += baseText[i];
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
            float wordLenght = font.MeasureString(word).X * textSize;

            //If adding the word doesn't make the line go over the max width, simply add the lenght of our last word (and the space) to the current line
            if (wordLenght + currentLineLenght <= maxLineWidth)
                currentLineLenght += wordLenght * textSize + spaceWidth;

            //If adding the word goes over the max width
            else
            {
                //Reset the line lenght to a new line (so only the word and the space
                currentLineLenght = wordLenght * textSize + spaceWidth;
                //Add a linebreak
                formattedSetence += "\n";
            }

            //Add the word and space to the end of the setence
            formattedSetence += word;
            formattedSetence += " ";
        }

        /// <summary>
        /// Tells fanny that the message spoken has a dynamic text element that is calculated on the fly. The dynamic text replaces the matching $ key. So if you have two dynamic text elements, itll replace $0 and $1
        /// </summary>
        /// <param name="customText"></param>
        /// <returns></returns>
        public HelperMessage AddDynamicText(DynamicHelperMessageSegments customText)
        {
            if (originalText.Contains("$" + textSegments.Count.ToString()))
                textSegments.Add(customText);
            return this;
        }
        #endregion
    }

    public class ScreenHelperPortrait
    {
        public Asset<Texture2D> Texture;
        public int frameCount;
        public int animationSpeed;

        /// <summary>
        /// Registers a portrait that's used by a screen helper
        /// </summary>
        /// <param name="portraitName">The filename. The texture name will use this , prefixed with "Helper" when loading it</param>
        /// <param name="frameCount">How many frames this portrait has</param>
        /// <param name="animationSpeed">How fast the animation plays</param>
        public static void LoadPortrait(string portraitName, int frameCount, int animationSpeed = 11)
        {
            ScreenHelperPortrait portrait = new ScreenHelperPortrait(portraitName, frameCount, animationSpeed);
            //Load itself into the portrait list
            if (!ScreenHelperMessageManager.Portraits.ContainsKey(portraitName))
                ScreenHelperMessageManager.Portraits.Add(portraitName, portrait);
        }

        public ScreenHelperPortrait(string portrait, int frameCount, int animationSpeed = 11)
        {
            Texture = ModContent.Request<Texture2D>("CalRemix/UI/Fanny/Helper" + portrait);
            this.frameCount = frameCount;
            this.animationSpeed = animationSpeed;
        }
    }

    public struct HelperTextboxPalette
    {
        public Color background = Color.SaddleBrown;
        public Color backgroundHover = Color.SaddleBrown;
        public Color outline = Color.Magenta;
        public Color text = Color.Lime;
        public Color textOutline = Color.DarkBlue;

        public HelperTextboxPalette() { }

        public HelperTextboxPalette(Color text, Color textOutline, Color background, Color backgroundHover, Color outline)
        {
            this.text = text;
            this.textOutline = textOutline;
            this.background = background;
            this.backgroundHover = backgroundHover;
            this.outline = outline;
        }
    }
}