using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Furniture.CraftingStations;
using CalamityMod.Items.Potions;
using CalRemix.Assets.Fonts;
using CalRemix.Core.Retheme;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using SubworldLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace CalRemix.UI
{
    /// <summary>
    /// Represents the character / UI eleemnt of the character itself
    /// </summary>
    public class ScreenHelper(string name) : UIElement
    {
        public string Name { get; } = name;

        public Vector2 BasePosition => GetDimensions().ToRectangle().Bottom();
        public ScreenHelperTextbox SpeechBubble;

        public float fadeIn;
        public bool idlesInInventory;
        public bool renderOverBackground;

        private int helperFrame;
        private int helperFrameCounter;

        #region Main Speaking / Stopping code
        //Default placeholder message used when not speaking
        public HelperMessage NoMessage;

        //Non-default message currently being spoken
        private HelperMessage currentMessage;

        //Timer that ticks down after a message has been spoken. Lasts one second
        public int talkCooldown;

        //universal on start
        public Action OnStartUniversal;

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
        /// Wether or not the helper has a message that's not displayed yet due to being in a delay period
        /// </summary>
        public bool HasMessageInDelay => currentMessage != null && currentMessage.InDelayPeriod;

        /// <summary>
        /// You probably don't need to use this. This is the helper's current message wether or not it is being shown or if it is in a delay waiting period. Use <see cref="UsedMessage"/> for the messagte thats actually seen
        /// </summary>
        public HelperMessage StoredMessage => currentMessage ;

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
            if (currentMessage == null || !currentMessage.NoCooldownAfterSpeaking)
                talkCooldown = 60;
            currentMessage = null;
        }
        #endregion


        #region Position data
        public HelperPositionData CharacterPositionData;
        public ScreenHelper SetPositionData(HelperPositionData data)
        {
            CharacterPositionData = data;
            CharacterPositionData.SetPosition(this);
            return this;
        }

        /// <summary>
        /// Gives the character a basic position data, like the one used by fanny and friends
        /// </summary>
        /// <param name="fromTheLeft">If the character comes from the left of the screen or not</param>
        /// <param name="distanceFromEdge">How far away from the edge is the character</param>
        /// <param name="screenHeightPercent">Percentage of the screen from which the character is located</param>
        /// <returns></returns>
        public ScreenHelper SetPositionData(bool fromTheLeft, float distanceFromEdge, float screenHeightPercent = 0.074f)
        {
            CharacterPositionData = new HelperPositionData(
                new Vector2(fromTheLeft ? 0 : 1, 1 - screenHeightPercent),           //Percent anchor
                new Vector2(fromTheLeft ? 0 : -80, -80),                             //Pixel anchor
                new Vector2(fromTheLeft ? distanceFromEdge : -distanceFromEdge, 0),  //Offset when appearing
                new Vector2(fromTheLeft ? 50 : -50, -90),                             //Textbox offset
                new Vector2(fromTheLeft ? 0 : 1f, 1f),                               //Textbox text size offset
                new Vector2(0, -30f),                                                //Textbox fade out offset
                new Vector2(0f, 40f),                                                //Textbox fade in offset
                new Vector2(60 * (fromTheLeft ? -1 : 1), -60));                      //item hold offset

            //update the position
            CharacterPositionData.SetPosition(this);
            return this;
        }
        #endregion

        #region Style
        /// <summary>
        /// Speaking sound for the helper's messages. Can be overriden by <see cref="HelperMessage.voiceOverride"/>
        /// </summary>
        public SoundStyle speakingSound;
        /// <summary>
        /// Sound played when the helper is tickled by clicking on them. Doesnt play if <see cref="canBeTickled"/> is set to false <br/>
        /// Doesn't play any sounds if null
        /// </summary>
        public SoundStyle? tickleSound = null;



        /// <summary>
        /// Hover text for the helper's textboxes. Can be overriden by <see cref="HelperMessage.hoverTextOverride"/>
        /// </summary>
        public string textboxHoverText;
        /// <summary>
        /// Palette for the helper's textboxes. Can be overriden by <see cref="HelperMessage.paletteOverride"/>
        /// </summary>
        public HelperTextboxPalette textboxPalette;
        /// <summary>
        /// The palette currently used by the helper. This uses the palette of the message's override if one is present
        /// </summary>
        public HelperTextboxPalette UsedPalette => (currentMessage != null && currentMessage.paletteOverride != null) ? currentMessage.paletteOverride : textboxPalette;


        /// <summary>
        /// The visual theme of the textbox, made of textures. Contains a 9slice border, and a (potentially animated) background texture
        /// </summary>
        public HelperTextboxTheme textboxTheme;
        /// <summary>
        /// The formatting of the textbox. Defines the minimum and max size, and the default font size
        /// </summary>
        public HelperTextboxFormatting textboxFormatting;
        /// <summary>
        /// The font the textbox uses.
        /// </summary>
        public DynamicSpriteFont textboxFont;


        #region Setters
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
        public ScreenHelper SetTextboxStyle(string textboxHoverText, HelperTextboxPalette palette = null, DynamicSpriteFont font = null)
        {
            this.textboxHoverText = textboxHoverText;

            if (palette != null)
                textboxPalette = palette;
            else
                textboxPalette = new HelperTextboxPalette();

            if (font != null)
                textboxFont = font;
            else
                textboxFont = FontAssets.MouseText.Value;

            return this;
        }

        /// <summary>
        /// Sets the texture theme of the textbox
        /// </summary>
        public ScreenHelper SetTextboxTheme(HelperTextboxTheme theme)
        {
            textboxTheme = theme;
            return this;
        }

        /// <summary>
        /// Sets the text formatting of the textbox
        /// </summary>
        public ScreenHelper SetTextboxFormatting(HelperTextboxFormatting formatting, int outlineThickness = 3, int textBorderPadding = 10)
        {
            textboxFormatting = formatting;
            SpeechBubble.outlineThickness = outlineThickness;
            SpeechBubble.backgroundPadding = textBorderPadding;

            return this;
        }

        public ScreenHelper SetOnStartUniversal(Action action)
        {
            OnStartUniversal += action;
            return this;
        }
        #endregion
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

        #region On click stuff
        public event OnClickHelperDelegate OnClick;
        public delegate void OnClickHelperDelegate(ScreenHelper helper);

        public ScreenHelper AddOnClickEffect(OnClickHelperDelegate onClick)
        {
            OnClick += onClick;
            return this;
        }
        private void OnClickHelper(UIMouseEvent evt, UIElement listeningElement)
        {
            if (!ScreenHelperManager.screenHelpersEnabled || fadeIn < 1f)
                return;

            OnClick?.Invoke(this);

            if (canBeTickled && tickleSound.HasValue)
            {
                tickle = Math.Max(tickle, 0) + 1;
                SoundEngine.PlaySound(tickleSound.Value);
            }
        }
        #endregion

        public override void OnInitialize()
        {
            OnLeftClick += OnClickHelper;
        }

        public override void Update(GameTime gameTime)
        {
            if (!ScreenHelperManager.screenHelpersEnabled)
                return;

            //Tick down the cooldown between messages
            talkCooldown--;

            //Show show if there's currently a message (if it's a message only shown in the inventory, only show it there)
            //Additionally, always show with the inventory open as long as its not hidden by default
            bool shouldShow = (Main.playerInventory && idlesInInventory) ||
                (currentMessage != null && (Main.playerInventory || UsedMessage.DisplayOutsideInventory) && !currentMessage.InDelayPeriod);

            float previousFadeIn = fadeIn;

            //Fades in and out
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

            //Update the position if it changed
            if (fadeIn != previousFadeIn)
            {
                CharacterPositionData.SetPosition(this);
            }

            #region Misc shake, bounce, and tickle animations
            //Shake if fanny choses a new message while already out there in the inventory
            if (needsToShake)
            {
                needsToShake = false;
                if (Main.playerInventory && fadeIn == 1 && canShake)
                    shakeTime = 1f;
            }

            //Tick down bounce, shake & tickle anims

            if (shakeTime > 0)
                shakeTime -= 1 / (60f * 1f);

            bounce -= 1 / (60f * 0.4f);

            tickle -= 1 / (60f * 0.4f);
            if (tickle > 4)
                tickle -= 1 / (60f * 0.4f);
            #endregion

            //Play animation for the textbox's background
            if (textboxTheme != null && textboxTheme.backgroundFill != null && textboxTheme.backgroundFrameCount != new Point(1, 1))
                textboxTheme.Animate();

            base.Update(gameTime);
        }

        #region Drawing
        // Here we draw our UI
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (!ScreenHelperManager.screenHelpersEnabled)
            {
                return;
            }
            // find the player's latest discord chat while the game isn't opened. doesn't work in multiplayer for sanity reasons
            if (NPC.downedBoss1)
            {
                if (Main.netMode == NetmodeID.SinglePlayer && !(ScreenHelperManager.discord1.alreadySeen || ScreenHelperManager.discord2.alreadySeen) && !Main.hasFocus)
                {
                    ScreenHelperManager.GetDiscord();
                }
            }
            AnimateFanny();

            // finally draw Fanny
            Texture2D fannySprite = UsedMessage.Portrait.Texture.Value;
            Rectangle frame;

            //Regular sheet
            if (UsedMessage.Portrait.rows == UsedMessage.Portrait.frameCount)
                frame = fannySprite.Frame(1, UsedMessage.Portrait.frameCount, 0, helperFrame);
            else
            {
                int horizontalFrames = (int)MathF.Ceiling(UsedMessage.Portrait.frameCount / UsedMessage.Portrait.rows);
                int frameY = helperFrame % UsedMessage.Portrait.rows;
                int frameX = helperFrame / horizontalFrames;

                frame = fannySprite.Frame(horizontalFrames, UsedMessage.Portrait.rows, frameX, frameY);
            }

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

            float opacity = fadeIn;
            

            //fades with the textbox if the textbox doesnt have a fade offset
            if (CharacterPositionData.textboxFadeOutOffset == Vector2.Zero)
            {
                if (Speaking)
                    opacity *= 1 - MathF.Pow(Utils.GetLerpValue(30, 0, UsedMessage.TimeLeft, true), 1.6f);
                else
                    opacity = 0;
            }

            spriteBatch.Draw(fannySprite, position, frame, Color.White * opacity, 0, new Vector2(frame.Width / 2f, frame.Height), 1f, 0, 0);
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
                helperFrame = UsedMessage.Portrait.loopPoint;
        }

        public void DrawItem()
        {
            Main.instance.LoadItem(UsedMessage.ItemType);
            Texture2D itemSprite = TextureAssets.Item[UsedMessage.ItemType].Value;
            int count = Main.itemAnimations[UsedMessage.ItemType] != null ? Main.itemAnimations[UsedMessage.ItemType].FrameCount : 1;
            Rectangle nframe = itemSprite.Frame(1, count, 0, 0);
            Vector2 origin = nframe.Size() / 2f;
            Vector2 itemPosition = BasePosition + CharacterPositionData.itemOffset + UsedMessage.ItemOffset + Vector2.UnitY * MathF.Sin(Main.GlobalTimeWrappedHourly * 2) * 4;

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
            if (ParentSpeaker.Speaking && ParentSpeaker.fadeIn == 1 && !ParentSpeaker.UsedMessage.CantBeClickedOff && ParentSpeaker.UsedMessage.TimeLeft > 30)
                ParentSpeaker.UsedMessage.TimeLeft = 30;
        }

        public override void Recalculate()
        {
            if (ParentSpeaker != null && ParentSpeaker.CharacterPositionData != null)
            {
                ParentSpeaker.CharacterPositionData.SetTextboxPosition(this);
                base.Recalculate();
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (!ParentSpeaker.Speaking)
                return;
            HelperTextboxPalette palette = ParentSpeaker.UsedPalette;

            // a shit ton of variables
            var font = ParentSpeaker.textboxFont;
            string text = ParentSpeaker.UsedMessage.Text;

            Rectangle dimensions = GetDimensions().ToRectangle();

            Vector2 outlineDrawPosition = dimensions.TopLeft();
            Vector2 backgroundDrawPosition = outlineDrawPosition + Vector2.One * outlineThickness;
            Vector2 textDrawPosition = backgroundDrawPosition + Vector2.One * backgroundPadding + ParentSpeaker.CharacterPositionData.textboxTextOffset;

            Vector2 outlineSize = dimensions.Size();
            Vector2 backgroundSize = outlineSize - Vector2.One * outlineThickness * 2;

            Texture2D squareTexture = TextureAssets.MagicPixel.Value;
            float opacity = ParentSpeaker.fadeIn * Utils.GetLerpValue(0, 30, ParentSpeaker.UsedMessage.TimeLeft, true);

            // draw the border as a large rectangle behind, and the inners as a slightly smaller rectangle infront
            Main.spriteBatch.Draw(squareTexture, outlineDrawPosition, null, palette.outline * opacity, 0, Vector2.Zero, outlineSize / squareTexture.Size(), 0, 0);
            Main.spriteBatch.Draw(squareTexture, backgroundDrawPosition, null, palette.background * opacity, 0, Vector2.Zero, backgroundSize / squareTexture.Size(), 0, 0);

            //Hover text and hover thing
            if (ContainsPoint(Main.MouseScreen) && ParentSpeaker.fadeIn == 1 && !ParentSpeaker.UsedMessage.CantBeClickedOff && ParentSpeaker.UsedMessage.TimeLeft > 30)
            {
                Main.spriteBatch.Draw(squareTexture, backgroundDrawPosition, null, palette.backgroundHover with { A = 0 } * (0.4f + 0.2f * MathF.Sin(Main.GlobalTimeWrappedHourly * 4f)) * opacity, 0, Vector2.Zero, backgroundSize / squareTexture.Size(), 0, 0);
                Main.LocalPlayer.mouseInterface = true;

                string hoverText = ParentSpeaker.textboxHoverText;
                if (ParentSpeaker.UsedMessage.hoverTextOverride != "")
                    hoverText = ParentSpeaker.UsedMessage.hoverTextOverride;
                Main.instance.MouseText(hoverText);
            }

            //texture theme
            if (ParentSpeaker.textboxTheme != null)
            {
                //Draw the background texture
                if (ParentSpeaker.textboxTheme.backgroundFill  != null)
                {
                    Texture2D tex = ParentSpeaker.textboxTheme.backgroundFill.Value;

                    Vector2 bgTextureCorner = outlineDrawPosition;
                    bgTextureCorner += ParentSpeaker.textboxTheme.backgroundPadding;
                    Vector2 bgTextureFillSize = outlineSize;
                    bgTextureFillSize -= ParentSpeaker.textboxTheme.backgroundPadding * 2;

                    Rectangle bgFrame = ParentSpeaker.textboxTheme.GetFrame();

                    Main.spriteBatch.Draw(tex, bgTextureCorner, bgFrame, Color.White * opacity, 0, Vector2.Zero, bgTextureFillSize / bgFrame.Size(), 0, 0);
                }

                //Draw the 9 slice border
                if (ParentSpeaker.textboxTheme.border9Slice != null)
                {
                    Vector2 insideSize = outlineSize;
                    insideSize -= ParentSpeaker.textboxTheme.border9SliceCornerSize * 2;

                    Texture2D tex = ParentSpeaker.textboxTheme.border9Slice.Value;
                    Vector2 cornerSize = ParentSpeaker.textboxTheme.border9SliceCornerSize;

                    Vector2 middlePartSize = tex.Size() - cornerSize * 2;


                    //Draw corners
                    for (int i = 0; i < 2; i++)
                        for (int j = 0; j < 2; j++)
                        {
                            Vector2 origin = cornerSize;
                            origin.X *= i;
                            origin.Y *= j;

                            Vector2 drawPosition = outlineDrawPosition;
                            drawPosition.X += outlineSize.X * i;
                            drawPosition.Y += outlineSize.Y * j;

                            Rectangle cornerFrame = new Rectangle((int)((cornerSize.X + middlePartSize.X) * i), (int)((cornerSize.Y + middlePartSize.Y) * j), (int)cornerSize.X, (int)cornerSize.Y);

                            Main.spriteBatch.Draw(tex, drawPosition, cornerFrame, Color.White * opacity, 0, origin, Vector2.One, 0, 0);
                        }

                    //Draw fillings

                    //up and down
                    for (int i = 0; i < 2; i++)
                    {
                        Vector2 origin = Vector2.UnitY * cornerSize.Y * i;

                        Vector2 drawPosition = outlineDrawPosition + Vector2.UnitX * cornerSize.X;
                        drawPosition.Y += outlineSize.Y * i;

                        Rectangle fillingFrame = new Rectangle((int)cornerSize.X + 1, (int)((cornerSize.Y + middlePartSize.Y) * i), (int)middlePartSize.X - 2, (int)cornerSize.Y);

                        Main.spriteBatch.Draw(tex, drawPosition, fillingFrame, Color.White * opacity, 0, origin, new Vector2(insideSize.X / (float)fillingFrame.Width, 1f), 0, 0);
                    }

                    //left and right
                    for (int i = 0; i < 2; i++)
                    {
                        Vector2 origin = Vector2.UnitX * cornerSize.X * i;

                        Vector2 drawPosition = outlineDrawPosition + Vector2.UnitY * cornerSize.Y;
                        drawPosition.X += outlineSize.X * i;

                        Rectangle fillingFrame = new Rectangle((int)((cornerSize.X + middlePartSize.X) * i), (int)cornerSize.Y + 1, (int)cornerSize.X, (int)middlePartSize.Y - 2);

                        Main.spriteBatch.Draw(tex, drawPosition, fillingFrame, Color.White * opacity, 0, origin, new Vector2(1f, insideSize.Y / (float)fillingFrame.Height), 0, 0);
                    }
                }
            }

            // finally draw the text
            if (ParentSpeaker.Name == "QueenOfClubs")
                Utils.DrawBorderStringFourWay(Main.spriteBatch, ParentSpeaker.textboxFont, text, textDrawPosition.X, textDrawPosition.Y, palette.text * 10, palette.textOutline * opacity, Vector2.Zero, ParentSpeaker.UsedMessage.textSize);
            else
                Utils.DrawBorderStringFourWay(Main.spriteBatch, ParentSpeaker.textboxFont, text, textDrawPosition.X, textDrawPosition.Y, palette.text * (Main.mouseTextColor / 255f) * opacity, palette.textOutline * opacity, Vector2.Zero, ParentSpeaker.UsedMessage.textSize);

            // not final actually draw the guy over the box if desired
            if (ParentSpeaker.renderOverBackground)
            {
                ParentSpeaker.Draw(Main.spriteBatch);
            }
        }
    }

    public class ScreenHelpersUIState : UIState
    {
        public static ScreenHelper FannyTheFire = new("Fanny");
        public static ScreenHelper EvilFanny = new("EvilFanny");
        public static ScreenHelper WonderFlower = new("WonderFlower");
        public static ScreenHelper BizarroFanny = new("BizarroFanny");
        public static ScreenHelper Renault5 = new("Renault5");
        public static ScreenHelper CrimSon = new("CrimSon");
        public static ScreenHelper TrapperBulbChan = new("TrapperBulbChan");
        public static ScreenHelper MiracleBoy = new("MiracleBoy");
        public static ScreenHelper MovieCygn = new("MovieCygn");
        public static ScreenHelper Solyn = new("Solyn");
        public static ScreenHelper Flux = new("Flux");
        public static ScreenHelper QueenOfClubs = new("QueenOfClubs");

        public static ScreenHelper AltMetalFanny = new("AltMetalFanny");
        public static ScreenHelper ThePinkFlame = new("ThePinkFlame");
        public static ScreenHelper CrimFather = new("CrimFather");
        public static ScreenHelper ExampleHelper = new("ExampleHelper");

        public override void OnInitialize()
        {
            LoadScreenHelper(FannyTheFire,"FannyIdle", true)
                .SetVoiceStyle(SoundID.Cockatiel with { MaxInstances = 0, Volume = 0.3f, Pitch = -0.8f }, SoundID.DD2_GoblinScream)
                .SetTextboxStyle("Thank you for the help, Fanny!")
                .SetPositionData(false, 240);
            
            LoadScreenHelper(EvilFanny, "EvilFannyIdle")
                .SetVoiceStyle(SoundID.DD2_DrakinShot with { MaxInstances = 0, Volume = 0.3f, Pitch = 0.8f }, SoundID.NPCHit40)
                .SetTextboxStyle("Get away, Evil Fanny!", new HelperTextboxPalette(Color.Black, Color.Red, Color.Indigo, Color.DeepPink, Color.Tomato))
                .SetAvailabilityCondition(() => Main.hardMode)
                .SetPositionData(true, 120);

            LoadScreenHelper(MiracleBoy, "MiracleBoyIdle")
                .SetVoiceStyle(SoundID.BloodZombie with { MaxInstances = 0 })
                .SetTextboxStyle("uh uh..?", new HelperTextboxPalette(Color.OrangeRed, Color.White, Color.Transparent, Color.Transparent, Color.Transparent))
                .SetTextboxTheme(new HelperTextboxTheme("MiracleBoy_9Slice", new Vector2(16, 16), "MiracleBoy_Background", new Vector2(16, 16)))
                .SetTextboxFormatting(null, 0, 16)
                .SetPositionData(new HelperPositionData(
                    new Vector2(0f, 0.42f), //Anchored to the left side at 42% of the way down the screen
                    new Vector2(0f, -80f),
                    new Vector2(120, 0f),   //Slides right when spawning
                    new Vector2(100, -90),  //Offset from bottom center of portrait to the top left of the textbox
                    Vector2.Zero,
                    Vector2.UnitY * -30f,
                    Vector2.UnitY * 40f
                    ))
                .SetAvailabilityCondition(() => Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().miracleUnlocked || (!CalRemixWorld.postGenUpdate && NPC.downedMoonlord));
                
            LoadScreenHelper(CrimSon, "CrimSonDefault")
                .SetVoiceStyle(SoundID.DD2_KoboldFlyerChargeScream with { MaxInstances = 0 })
                .SetAvailabilityCondition(() => (Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().gottenCellPhone || NPC.downedGolemBoss || !CalRemixWorld.postGenUpdate) && Main.hardMode && (DateTime.Today.DayOfYear != 18))
                .SetTextboxStyle("Wretched abomination agaisnt god", new HelperTextboxPalette(Color.White, Color.Black, Color.Transparent, Color.Transparent, Color.Transparent))
                .SetTextboxTheme(new HelperTextboxTheme("CrimSon_9Slice", new Vector2(22, 19), "CrimSon_Background", Vector2.Zero, new Point(6, 6), 6)).
                SetTextboxFormatting(new HelperTextboxFormatting(new Vector2(135, 300), 135)).
                SetPositionData(new HelperPositionData(
                    new Vector2(0.44f, 1f), //Anchored to bottom middle of screen
                    new Vector2(0f, 0f),   //Offset so we're centered
                    new Vector2(0f, -160f),   //Slides up when spawning
                    new Vector2(60, -298),   //Offset from b ottom of portrait to center of 
                    Vector2.Zero,
                    Vector2.UnitY * -40f,
                    Vector2.UnitY * 40f
                    ));

            LoadScreenHelper(TrapperBulbChan, "TrapperIdle", false, new Vector2(96, 96))
                .SetVoiceStyle(SoundID.LucyTheAxeTalk with { MaxInstances = 0 })
                .SetTextboxStyle("Sugoi!!! Arigato Gozaimas!", new HelperTextboxPalette(Color.White, Color.Black * 0.2f, Color.Transparent, Color.Transparent, Color.Transparent))
                .SetTextboxTheme(new HelperTextboxTheme(null, Vector2.Zero, "Trapper_Background", Vector2.Zero)).
                SetExtraAnimations(false, false, false). //shes locked in her textbox
                SetAvailabilityCondition(() => NPC.downedPlantBoss).
                SetTextboxFormatting(new HelperTextboxFormatting(new Vector2(640, 158), 465), 0, 0).
                SetPositionData(new HelperPositionData(
                    new Vector2(0.5f, 0.0f), //Anchored to top middle of screen
                    new Vector2(-288, 0f),   //Offset so we're centered
                    new Vector2(0f, 58f),   //Slides down when spawning
                    new Vector2(-80, -128),   //Offset from b ottom of portrait to center of 
                    Vector2.Zero,
                    Vector2.Zero,
                    Vector2.Zero,
                    null,
                    new Vector2(149, 12)
                    ));


            LoadScreenHelper(Flux, "FluxIdle", false, new Vector2(150, 160), true)
                .SetVoiceStyle(SoundID.Item178 with { MaxInstances = 0 })
                .SetTextboxStyle("I mean, alright Flux", new HelperTextboxPalette(Color.White, Color.Black * 0.2f, Color.Transparent, Color.Transparent, Color.Transparent))
                .SetTextboxTheme(new HelperTextboxTheme(null, Vector2.Zero, "Flux_Background", Vector2.Zero))
                .SetExtraAnimations(false, false, false) //shes locked in her textbox
                .SetAvailabilityCondition(() => Main.LocalPlayer.Remix().fifteenMinutesSinceHardmode <= 0 && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake)
                .SetTextboxFormatting(new HelperTextboxFormatting(new Vector2(670, 172), 462), 0, 0)
                .SetPositionData(new HelperPositionData(
                    new Vector2(0.1f, 0.6f), // anchored to bottom middle, a little shifted to the left
                    new Vector2(0, 0f),   //Offset so we're centered
                    new Vector2(100f, 0f),   // slide from left
                    new Vector2(-94, -154),   //Offset from b ottom of portrait to center of 
                    Vector2.Zero,
                    Vector2.Zero,
                    Vector2.Zero,
                    null,
                    new Vector2(189, 25)
                    ));

            LoadScreenHelper(QueenOfClubs, "QueenOfClubsEmpty", false, new Vector2(136, 174), true)
                .SetVoiceStyle(SoundID.Item178 with { MaxInstances = 0 })
                .SetTextboxStyle("test", new HelperTextboxPalette(Color.White, Color.Black * 0.2f, Color.Black, Color.Black, Color.LightSlateGray), FontRegistry.Instance.WorkbenchDelicateText)
                .SetExtraAnimations(false, false, false)
                .SetOnStartUniversal(ScreenHelperManager.SpinQoC)
                .SetAvailabilityCondition(() => Main.LocalPlayer.GetModPlayer<QoCPlayer>().isQoCUnlocked && Main.LocalPlayer.GetModPlayer<QoCPlayer>().isQoCAwake)
                .SetPositionData(new HelperPositionData(
                    new Vector2(1, 0), // anchored to bottom middle, a little shifted to the left
                    new Vector2(-175 - 65, 100 - 83),   //top right. arbitrary values - half of width/height
                    new Vector2(0f, 0f),   // just fade in
                    new Vector2(-480, -100),   //left 
                    Vector2.Zero,
                    Vector2.Zero,
                    Vector2.Zero
                    ));

            /* LoadScreenHelper(MovieCygn, "Moviecygn", false, new Vector2(495, 595))
                 .SetVoiceStyle(SoundID.Drown with { MaxInstances = 0, Volume = 0.3f, Pitch = -0.8f }, SoundID.DD2_GoblinScream)
                 .SetTextboxStyle("Join my fandom", new HelperTextboxPalette(Color.White, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent))
                 .SetTextboxTheme(new HelperTextboxTheme("Moviecygn_9Slice", new Vector2(38, 38), "Moviecygn_Background", new Vector2(38, 38)))
                 .SetTextboxFormatting(new HelperTextboxFormatting(new Vector2(184, 300), 184), 0, 30)
                 .SetExtraAnimations(false, false, false)
                 .SetPositionData(new HelperPositionData(
                     new Vector2(0.0f, 0.0f), //Anchored to top left of the screen
                     new Vector2(-20, 0f),
                     new Vector2(20f, 0f),   //Slides from the left when spawning
                     new Vector2(290, -435),   //Offset from b ottom of portrait to center of 
                     Vector2.Zero,
                     -Vector2.UnitX * 20f,
                     Vector2.Zero,
                     null,
                     new Vector2(0, 0)
                     ));
             */


            LoadScreenHelper(WonderFlower, "TalkingFlower")
                .SetVoiceStyle(ScreenHelperManager.WonderFannyVoice, SoundID.Grass)
                .SetTextboxStyle("Oooh! So exciting!", new HelperTextboxPalette(Color.Black, Color.Transparent, new Color(250, 250, 250), Color.White, Color.Black * 0.4f))
                .SetPositionData(false, 240, 0.37f)
                .SetAvailabilityCondition(() => Main.hardMode);

            LoadScreenHelper(BizarroFanny, "BizarroFannyIdle")
                .SetVoiceStyle(ScreenHelperManager.BizarroFannyTalk with { MaxInstances = 0 })
                .SetTextboxStyle("???", new HelperTextboxPalette(Color.White, Color.Black, Color.Transparent, Color.Transparent, Color.Transparent))
                .SetExtraAnimations(false, false, false)
                .SetPositionData(false, 840, 0.65f)
                .SetAvailabilityCondition(() => DownedBossSystem.downedCeaselessVoid);

            LoadScreenHelper(Renault5, "Renault5")
                .SetVoiceStyle(ScreenHelperManager.VroomVroom with { MaxInstances = 0 })
                .SetAvailabilityCondition(() => NPC.downedMechBossAny)
                .SetTextboxStyle("TRUE", new HelperTextboxPalette(Color.Black, Color.White, new Color(238, 217, 14), Color.White, Color.Black))
                .SetTextboxTheme(new HelperTextboxTheme("Renault5_9Slice", new Vector2(77, 15)))
                .SetExtraAnimations(true, false, true).
                AddOnClickEffect(ScreenHelperManager.RenaultAdvertisment)
                .SetPositionData(false, 220, 0.42f);

            LoadScreenHelper(AltMetalFanny, "FannyIdle", false)
                .SetVoiceStyle(SoundID.Cockatiel with { MaxInstances = 0, Volume = 0.3f, Pitch = -0.8f }, SoundID.DD2_GoblinScream)
                .SetTextboxStyle("Thank you for the help, Fanny!")
                .SetPositionData(false, 240, 0.17f);

            LoadScreenHelper(Solyn, "Solyn")
                .SetVoiceStyle(BetterSoundID.ItemManaCrystal with { MaxInstances = 0, Volume = 0.3f, Pitch = 0.4f }, BetterSoundID.ItemMagicStar)
                .SetTextboxStyle("Thank you for the help, Solyn!", new HelperTextboxPalette(Color.HotPink, Color.Gold, Color.Purple, Color.MediumPurple, Color.PaleGoldenrod))
                .SetAvailabilityCondition(() => CalRemixAddon.Wrath != null && Main.LocalPlayer.Remix().solynUnlocked)
                .SetPositionData(false, 240);

            LoadScreenHelper(ThePinkFlame, "ThePinkFlame")
                .SetVoiceStyle(SoundID.Cockatiel with { MaxInstances = 0, Volume = 2 }, SoundID.DD2_GoblinScream)
                .SetTextboxStyle("Thank you for the help, The Pink Flame!", new HelperTextboxPalette(Color.AliceBlue, Color.DarkOrchid, Color.HotPink, Color.DarkOrchid, Color.DarkOrchid), FontRegistry.Instance.TimesNewRomanText)
                .SetPositionData(false, 240);

            LoadScreenHelper(CrimFather, "CrimFather")
                .SetVoiceStyle(SoundID.DD2_OgreRoar with { MaxInstances = 0, Volume = 0.3f, Pitch = -0.3f })
                .SetTextboxStyle("Anytime, Crim Father.", new HelperTextboxPalette(Color.Red, Color.Yellow, Color.DarkRed, Color.Black, Color.Black))
                .SetAvailabilityCondition(() => DownedBossSystem.downedYharon && (Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().gottenCellPhone || NPC.downedGolemBoss) && CalRemixWorld.timeSinceYharonMurdered >= 36000)
                .SetPositionData(false, 360);

            // This is how a helper is registered
            // First make a static ScreenHelper instance, then assign the base texture to it. This texture will have the word "Helper" in front of it in the actual files, so in this example, the file is called HelperExampleHelper.
            // SetVoiceStyle sets the sound that plays when the helper appears.
            // SetTextboxStyle allows you to put the default response to this helper's dialogue as well as the colors for its text box
            // SetPositoinData sets where the helper will appear on screen. This helper appears in the same place as Fanny.
            LoadScreenHelper(ExampleHelper, "ExampleHelper", false)
                .SetVoiceStyle(SoundID.MenuOpen)
                .SetTextboxStyle("This is an example response.", new HelperTextboxPalette(Color.White, Color.Black, Color.White, Color.LightGray, Color.Black))
                .SetPositionData(false, 240, 0.17f);
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
        public ScreenHelper LoadScreenHelper(ScreenHelper helper, string emptyMessagePortrait, bool idlesInInventory = false, Vector2? size = null, bool renderOverBackground = false)
        {
            helper.Height.Set(80, 0f);
            helper.Width.Set(80, 0f);

            if (size != null)
            {
                helper.Height.Set(size.Value.Y, 0f);
                helper.Width.Set(size.Value.X, 0f);
            }

            helper.idlesInInventory = idlesInInventory;
            helper.renderOverBackground = renderOverBackground;
            helper.NoMessage = new HelperMessage("", "", emptyMessagePortrait, displayOutsideInventory: false);

            Append(helper);

            ScreenHelperTextbox textbox = new ScreenHelperTextbox();
            textbox.Height.Set(0, 0f);
            textbox.Width.Set(0, 0f);
            textbox.ParentSpeaker = helper;
            Append(textbox);
            helper.SpeechBubble = textbox;



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

        public List<ScreenHelper> ScreenHelpers
        {
            get {
                List<ScreenHelper> helpers = new();
                foreach (UIElement element in Elements)
                {
                    if (element is ScreenHelper speaker)
                        helpers.Add(speaker);
                }

                return helpers;
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
            int resourceBarIndex = layers.FindLastIndex(layer => layer.Name.Equals("Rage and Adrenaline UI")) + 1;
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
        public bool unlockedMoodTracker = false;

        public override void SaveData(TagCompound tag)
        {
            for (int i = 0; i < ScreenHelperManager.screenHelperMessages.Count; i++)
            {
                HelperMessage msg = ScreenHelperManager.screenHelperMessages[i];

                if (msg.alreadySeen && msg.PersistsThroughSaves)
                    tag["FannyDialogue" + msg.Identifier] = true;

                //update readmessages array, so that when the player enters a world without loading data, it is up to date
                //nullcheck cuz savedata happens at the start when presenting players
                if (readMessages != null && msg.alreadySeen)
                    readMessages[i] = true;
            }

            tag["FannyReadThroughDogDialogue"] = ScreenHelperManager.ReadAllDogTips;
            tag["fannyMoodTracker"] = unlockedMoodTracker;
        }


        public override void LoadData(TagCompound tag)
        {
            readMessages = new bool[ScreenHelperManager.screenHelperMessages.Count];
            for (int i = 0; i < ScreenHelperManager.screenHelperMessages.Count; i++)
            {
                HelperMessage msg = ScreenHelperManager.screenHelperMessages[i];
                readMessages[i] = tag.ContainsKey("FannyDialogue" + msg.Identifier);
            }

            if (tag.TryGet("FannyReadThroughDogDialogue", out bool readDog))
                readTroughFannyDogDialogue = readDog;

            unlockedMoodTracker = false;
            if (tag.TryGet("fannyMoodTracker", out bool unlockedMoodDisplay))
                unlockedMoodTracker = unlockedMoodDisplay;
        }

        public override void OnEnterWorld()
        {
            if (Main.mouseRight && Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift) && Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space))
            {
                SoundEngine.PlaySound(SoundID.Cockatiel);
                SoundEngine.PlaySound(SoundID.DD2_GoblinScream);
                SoundEngine.PlaySound(SoundID.DD2_KoboldExplosion);

                for (int i = 0; i < ScreenHelperManager.screenHelperMessages.Count; i++)
                {
                    HelperMessage msg = ScreenHelperManager.screenHelperMessages[i];
                    msg.alreadySeen = false;
                    msg.CooldownTime = 0;
                }

                return;
            }

            if (readMessages is null || readMessages.Length < ScreenHelperManager.screenHelperMessages.Count)
            {

                for (int i = 0; i < ScreenHelperManager.screenHelperMessages.Count; i++)
                {
                    HelperMessage msg = ScreenHelperManager.screenHelperMessages[i];
                    msg.alreadySeen = false;
                }
                return;
            }


            for (int i = 0; i < ScreenHelperManager.screenHelperMessages.Count; i++)
            {
                HelperMessage msg = ScreenHelperManager.screenHelperMessages[i];
                msg.alreadySeen = readMessages[i];
            }

            ScreenHelperManager.ReadAllDogTips = readTroughFannyDogDialogue;
        }
    }

    [Autoload(Side = ModSide.Client)]
    public partial class ScreenHelperManager : ModSystem
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

        /// <summary>
        /// List that contains all the messages spoken by screen helpers, in the order they're loaded
        /// </summary>
        public static Dictionary<string, int> messageIndexByID = new();

        public static bool screenHelpersEnabled = true;
        public static int fannyTimesFrozen = 0;
        public static ScreenHelperSceneMetrics sceneMetrics;
        public static Rectangle screenRect;

        public static bool ongoingConversation = false;
        public static int ongoingConversationTimer = 0;

        #region Loading

        public override void Load()
        {
            LoadFannyPortraits();

            screenHelpersEnabled = true;
            fannyTimesFrozen = 0;
            sceneMetrics = new ScreenHelperSceneMetrics();
        }

        public override void AddRecipes()
        {
            LoadIntroMessages();
            LoadMoonLordDeath();

            LoadGeneralFannyMessages();
            LoadBabil();
            LoadDogSpamMessages();
            LoadLoreComments();
            LoadPassiveMessages();
            LoadEventMessages();
            LoadItemMessages();
            LoadBiomeMessages();
            LoadShrineMessages();
            LoadNPCMessages();
            LoadBossMessages();
            LoadBossDeathMessages();

            //LoadPityParty();
            LoadEvilGFBMessages();
            LoadMetalDiscoveryMessages();
            LoadWonderFlowerMessages();
            LoadCrimSon();
            LoadRenault5();
            LoadTrapperBulbChan();
            LoadSolynMessages();
            LoadFluxMessages();
            LoadQoCMessages();
            LoadMiracleBoyMessages();
            LoadPinkFlameMessage();
            LoadCrimFatherMessages();
            LoadExampleHelperMessage();
            SneakersRetheme.LoadHelperMessages();
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
            ScreenHelperPortrait.LoadPortrait("FannyDisturbed", 4);
            ScreenHelperPortrait.LoadPortrait("FannyBigGrin", 1);
            ScreenHelperPortrait.LoadPortrait("FannyAwe", 2);
            ScreenHelperPortrait.LoadPortrait("FannyEepy", 2);

            ScreenHelperPortrait.LoadPortrait("FannyCarbonized", 1);
            ScreenHelperPortrait.LoadPortrait("FannyCorporate", 1);
            ScreenHelperPortrait.LoadPortrait("FannyCosplay", 1);
            ScreenHelperPortrait.LoadPortrait("FannyHD", 1);
            ScreenHelperPortrait.LoadPortrait("FannyInfiniteFun", 1);
            ScreenHelperPortrait.LoadPortrait("FannyInfiniteFunScary", 1);
            ScreenHelperPortrait.LoadPortrait("FannyApocalypse", 4);
            ScreenHelperPortrait.LoadPortrait("FannyRadiant", 1);
            ScreenHelperPortrait.LoadPortrait("FannyRetro", 1);
            ScreenHelperPortrait.LoadPortrait("FannySneakers", 1);
            ScreenHelperPortrait.LoadPortrait("FannyBarefoot", 1);
            ScreenHelperPortrait.LoadPortrait("FannyIdleFrame", 1);

            //Metal Fanny
            ScreenHelperPortrait.LoadPortrait("FannyMetalCopper", 8);
            ScreenHelperPortrait.LoadPortrait("FannyMetalTin", 8);
            ScreenHelperPortrait.LoadPortrait("FannyMetalIron", 8);
            ScreenHelperPortrait.LoadPortrait("FannyMetalLead", 8);
            ScreenHelperPortrait.LoadPortrait("FannyMetalSilver", 8);
            ScreenHelperPortrait.LoadPortrait("FannyMetalTungsten", 8);
            ScreenHelperPortrait.LoadPortrait("FannyMetalGold", 8);
            ScreenHelperPortrait.LoadPortrait("FannyMetalPlatinum", 8);

            ScreenHelperPortrait.LoadPortrait("FannyMetalCobalt", 8);
            ScreenHelperPortrait.LoadPortrait("FannyMetalPalladium", 8);
            ScreenHelperPortrait.LoadPortrait("FannyMetalMythril", 8);
            ScreenHelperPortrait.LoadPortrait("FannyMetalOrichalcum", 8);
            ScreenHelperPortrait.LoadPortrait("FannyMetalAdamantite", 8);
            ScreenHelperPortrait.LoadPortrait("FannyMetalTitanium", 8);
            ScreenHelperPortrait.LoadPortrait("FannyMetalChlorophyte", 8);
            ScreenHelperPortrait.LoadPortrait("FannyMetalChlorium", 8);
            ScreenHelperPortrait.LoadPortrait("FannyMetalHallowed", 8);

            ScreenHelperPortrait.LoadPortrait("FannyMetalMiracle", 8);

            //Fazbear
            ScreenHelperPortrait.LoadPortrait("FannyFazebear", 1);
            ScreenHelperPortrait.LoadPortrait("FannyFazebearEndo", 1);
            ScreenHelperPortrait.LoadPortrait("FannyFazebearWithered", 1);

            //Evil Fanny
            ScreenHelperPortrait.LoadPortrait("EvilFannyIdle", 1);
            ScreenHelperPortrait.LoadPortrait("EvilFannyBabilplate", 1);
            ScreenHelperPortrait.LoadPortrait("EvilFannyBabilWrath", 1);
            ScreenHelperPortrait.LoadPortrait("EvilFannyCosplay", 1);
            ScreenHelperPortrait.LoadPortrait("EvilFannyCrisped", 1);
            ScreenHelperPortrait.LoadPortrait("EvilFannyDisgusted", 1);
            ScreenHelperPortrait.LoadPortrait("EvilFannyHD", 1);
            ScreenHelperPortrait.LoadPortrait("EvilFannyKYS", 1);
            ScreenHelperPortrait.LoadPortrait("EvilFannyMiffed", 1);
            ScreenHelperPortrait.LoadPortrait("EvilFannyPissed", 1);
            ScreenHelperPortrait.LoadPortrait("EvilFannyPoint", 1);
            ScreenHelperPortrait.LoadPortrait("EvilFannyWrathful", 1);

            //Bizarro Fanny
            ScreenHelperPortrait.LoadPortrait("BizarroFannyIdle", 4);
            ScreenHelperPortrait.LoadPortrait("BizarroFannyGoner", 1);

            //Miracle Boy
            ScreenHelperPortrait.LoadPortrait("MiracleBoyIdle", 2, 6);
            ScreenHelperPortrait.LoadPortrait("MiracleBoyIceCream", 10, 6, 5,  6);
            ScreenHelperPortrait.LoadPortrait("MiracleBoyRead", 2, 6);
            ScreenHelperPortrait.LoadPortrait("MiracleBoyGnaw", 2, 6);
            ScreenHelperPortrait.LoadPortrait("MiracleBoySob", 2, 6);
            ScreenHelperPortrait.LoadPortrait("MiracleBoyRadiant", 2, 6);
            ScreenHelperPortrait.LoadPortrait("MiracleBoyItWasMeee", 2, 6);
            ScreenHelperPortrait.LoadPortrait("MiracleBoyCry", 2, 6);
            ScreenHelperPortrait.LoadPortrait("MiracleBoyGrinchBaby", 2, 6);
            ScreenHelperPortrait.LoadPortrait("MiracleBoyUpset", 2, 6);
            ScreenHelperPortrait.LoadPortrait("MiracleBoySweat", 2, 6);

            //Dead miracle boy
            ScreenHelperPortrait.LoadPortrait("MiracleBoyDisemboweled", 1);
            ScreenHelperPortrait.LoadPortrait("MiracleBoyMortis", 1);
            ScreenHelperPortrait.LoadPortrait("MiracleBoyWithered", 1);
            ScreenHelperPortrait.LoadPortrait("MiracleBoyPhantom", 1);
            ScreenHelperPortrait.LoadPortrait("MiracleBoyNightmare", 1);
            ScreenHelperPortrait.LoadPortrait("MiracleBoyPurple", 1);

            //Crim Son
            ScreenHelperPortrait.LoadPortrait("CrimSonDefault", 42, 6, 6);
            ScreenHelperPortrait.LoadPortrait("CrimSonCorrupson", 42, 6, 6);
            ScreenHelperPortrait.LoadPortrait("CrimSonHeadless", 42, 6, 6);
            ScreenHelperPortrait.LoadPortrait("CrimSonLostSoul", 42, 6, 6);
            ScreenHelperPortrait.LoadPortrait("CrimSonNose", 42, 6, 6);

            //Trapper bulb chan
            ScreenHelperPortrait.LoadPortrait("TrapperIdle", 1);
            ScreenHelperPortrait.LoadPortrait("TrapperUwaa", 1);
            ScreenHelperPortrait.LoadPortrait("TrapperHappy", 1);
            ScreenHelperPortrait.LoadPortrait("TrapperWTF", 1);
            ScreenHelperPortrait.LoadPortrait("TrapperHuh", 1);
            ScreenHelperPortrait.LoadPortrait("TrapperDisgust", 1);

            //Flux
            ScreenHelperPortrait.LoadPortrait("FluxIdle", 1);

            //Queen of Clubs
            ScreenHelperPortrait.LoadPortrait("QueenOfClubsEmpty", 1);

            //Talking Flower
            ScreenHelperPortrait.LoadPortrait("TalkingFlower", 11, 5);

            //Renault 5
            ScreenHelperPortrait.LoadPortrait("Renault5", 1);

            //Solyn
            ScreenHelperPortrait.LoadPortrait("Solyn", 1);

            //The Pink Flame
            ScreenHelperPortrait.LoadPortrait("ThePinkFlame", 1);

            //Crim Father
            ScreenHelperPortrait.LoadPortrait("CrimFather", 1);

            //Example Helper
            ScreenHelperPortrait.LoadPortrait("ExampleHelper", 1);

            //Moviecygn
            ScreenHelperPortrait.LoadPortrait("Moviecygn", 3, 30);
        }

        public override void PostSetupContent()
        {
            //Now that the messages have been created, sort them by speaker 
            screenHelperMessageGroups = screenHelperMessages.GroupBy(m => m.DesiredSpeaker);
            MoodTracker.moodList = MoodTracker.moodList.OrderByDescending(m => m.priority).ToList();
        }
        #endregion

        public override void PostUpdateEverything()
        {
            if (Main.dedServ)
                return;

            Task.Run(UpdateLoreCommentTracking);
            Task.Run(UpdateMessages);
            Task.Run(UpdateFannyMood);

            //Dont try speaking messages if fanny is not enabled
            if (!screenHelpersEnabled)
                return;

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

                    HelperMessage messageToPlay = null;

                    //Looks at all the messages
                    foreach (HelperMessage message in messageGroup)
                    {
                        //Can only play activated messages during conversations
                        if (ongoingConversation && !message._needsActivation)
                            continue;

                        //If we already found a message, we only take in messages of increased priority
                        if (messageToPlay != null && messageToPlay.Priority >= message.Priority)
                            continue;

                        //If we already found a message to play, we only consider messages that have been activated because those are more important and take priority
                        //if (messageToPlay != null && !message._needsActivation)
                        //    continue;

                        if (message.CanPlayMessage() && message.CheckExtraConditions(sceneMetrics))
                        {
                            messageToPlay = message;
                            //message.Text = Language.GetOrRegister("Mods.CalRemix.Fanny." + message.Identifier).Value;
                            break;
                        }
                    }

                    if (messageToPlay != null)
                        messageToPlay.PlayMessage(speaker);
                }
            }

            previousHoveredItem = Main.HoverItem.type;
        }

        public override void PreUpdateNPCs()
        {
            //Clears out the scene metrics and set the screen rect
            sceneMetrics.Clear();
            screenRect = new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight);
            foreach (FannyMood mood in MoodTracker.npcDependentMoodList)
                mood.validConditionMet = false;
        }

        public void UpdateMessages()
        {
            //Tick down ongoing conversation
            ongoingConversationTimer--;
            if (ongoingConversationTimer <= 0)
            {
                ongoingConversationTimer = 0;
                ongoingConversation = false;
            }


            //Go through every message
            for (int i = 0; i < screenHelperMessages.Count; i++)
            {
                HelperMessage msg = screenHelperMessages[i];

                //Fallback
                if (msg.DesiredSpeaker == null)
                    msg.DesiredSpeaker = ScreenHelpersUIState.FannyTheFire;

                //Tick up messages that are queued to play with a timer
                if (msg.activationTimer > 0)
                {
                    msg.activationTimer++;

                    //if the activated message spent 10 seconds without playing, we stop trying to play it
                    //this is to avoid scenarios where the message's condition isnt met and it ends up hanging in the air forever until it is met
                    //DONT disable activated messages with start/end events, they may be important helper unlock ones
                    if (msg.activationTimer >= 600 && !msg.HasAnyEndEvents && !msg.HasAnyStartEvents)
                        msg.activationTimer = 0;
                }

                //Tick down the cooldown before a message can repeat
                if (msg.CooldownTime > 0)
                    msg.CooldownTime--;

                //Otherwise tick down the timer
                else if (msg.TimeLeft > 0)
                {
                    msg.TimeLeft--;

                    //Message stays in stasis if it needs to be clicked off
                    if (!msg.CantBeClickedOff && msg.TimeLeft == 30)
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

        #region Moods

        public void UpdateFannyMood()
        {
            //if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.NumPad0) && Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.NumPad7))
            //    MoodTracker.Reset();



            //Tick down lingering mood
            if (MoodTracker.moodLingerTimer > 0f)
                MoodTracker.moodLingerTimer -= 1 / 60f;
            if (MoodTracker.moodLingerTimer <= 0)
                MoodTracker.lingeringMood = null;

            if (Main.LocalPlayer.hideInfo[MoodTracker.DisplayID] && MoodTracker.firstBlockDisableMessage.alreadySeen && !MoodTracker.firstBlockDisableMessage.InDelayPeriod)
            {
                if (!MoodTracker.secondBlockDisableMessage.alreadySeen)
                {
                    //End the already seen one
                    if (ScreenHelpersUIState.FannyTheFire.UsedMessage == MoodTracker.firstBlockDisableMessage)
                    {
                        ScreenHelpersUIState.FannyTheFire.UsedMessage.EndMessage();
                        ScreenHelpersUIState.FannyTheFire.StopTalking();
                    }

                    MoodTracker.secondBlockDisableMessage.PlayMessage(ScreenHelpersUIState.FannyTheFire);
                }

                Main.LocalPlayer.hideInfo[MoodTracker.DisplayID] = false;
            }
        }

        #endregion

        #region Hooks
        public delegate void MessageActionDelegate(HelperMessage message);
        public static event MessageActionDelegate OnMessageEnd;
        public static event MessageActionDelegate OnMessageStart;

        public static void OnMessageEndCall(HelperMessage message) => OnMessageEnd?.Invoke(message);
        public static void OnMessageStartCall(HelperMessage message) => OnMessageStart?.Invoke(message);
        #endregion

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
        public static bool NearPosition(int positionX, int positionY, int distance) => Main.LocalPlayer.WithinRange(new Vector2(positionX * 16, positionY * 16), distance);
        public static bool NearPosition(Vector2 position, int distance) => Main.LocalPlayer.WithinRange(position, distance);
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
        public static bool CrossModBiome(string ModName, string BiomeName)
        {
            if (ModLoader.HasMod(ModName))
            {
                if (Main.LocalPlayer.InModBiome(ModLoader.GetMod(ModName).Find<ModBiome>(BiomeName)))
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
                msg.activationTimer = 0;
                msg.currentSpeaker = null;
                msg.TimeLeft = 0;
            }

            ongoingConversation = false;
            ongoingConversationTimer = 0;
        }
        #endregion
    }

    public class ScreenHelperSceneMetrics
    {
        public List<NPC> onscreenNPCs;
        public List<Projectile> onscreenProjectiles;
        public List<Item> onscreenItems;

        public ScreenHelperSceneMetrics()
        {
            onscreenNPCs = new List<NPC>(Main.maxNPCs);
            onscreenProjectiles = new List<Projectile>(Main.maxProjectiles);
            onscreenItems = new List<Item>(Main.maxItems);
        }

        public void Clear()
        {
            //Clear the NPCs
            onscreenNPCs.Clear();
            //Clear the Projectiles
            onscreenProjectiles.Clear();
            //Clear the Items
            onscreenItems.Clear();
        }
    }

    public class HelperMessage
    {
        private string originalText;
        private string formattedText;
        internal int maxTextWidth;
        public float textSize;


        public static HelperMessage ByID(string identifier)
        {
            if (ScreenHelperManager.messageIndexByID.TryGetValue(identifier, out int index))
            {
                return ScreenHelperManager.screenHelperMessages[index];
            }
            return null;
        }

        public string Text
        {
            get => formattedText;
            set
            {
                //Cache the original text, and format it
                originalText = value;
                // this doesnt account for helper font. i hope that doesnt break anything ahahaha
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
        /// Messages with higher priority are selected with.. priority. Use this so important messages like evil fanny's introduction doesn't appear after another unimportant evil fanny message
        /// </summary>
        public float Priority = 0;

        /// <summary>
        /// The desired screen helper to play this message
        /// </summary>
        public ScreenHelper DesiredSpeaker;

        //The fanny actively speaking the message. For cases where we want one fanny to say what the other fanny says
        public ScreenHelper currentSpeaker;


        public bool DisplayOutsideInventory { get; set; } //Defaults to true
        public bool OnlyPlayOnce { get; set; } //Defaults to true
        public bool CantBeClickedOff { get; set; } //Defaults to false
        public bool PersistsThroughSaves { get; set; } //Defaults to true
        public bool IgnoreSpeakerSpecificCondition { get; set; } //Defaults to false
        public bool NoCooldownAfterSpeaking { get; set; } //Defaults to false

        public ScreenHelperPortrait Portrait { get; set; } //The portrait used by the message

        public delegate string DynamicHelperMessageSegments();
        public static string GetPlayerName() => Main.LocalPlayer.name;
        public static string GetWorldName() => Main.worldName;

        public List<DynamicHelperMessageSegments> textSegments = new List<DynamicHelperMessageSegments>();

        /// <summary>
        /// DONT USE THIS. It doesnt automatically load the message!
        /// </summary>
        internal HelperMessage(string identifier, string message, string portrait = "", ScreenHelperMessageCondition condition = null, float duration = 5, float cooldown = 60, bool displayOutsideInventory = true, bool onlyPlayOnce = true, bool cantBeClickedOff = false, bool persistsThroughSaves = true, int maxWidth = 380, float fontSize = 1f)
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
            CantBeClickedOff = cantBeClickedOff;
            PersistsThroughSaves = persistsThroughSaves;

            if (portrait == "")
                portrait = "FannyIdle";
            Portrait = ScreenHelperManager.Portraits[portrait];

            //default
            DesiredSpeaker = ScreenHelpersUIState.FannyTheFire;


        }

        /// <summary>
        /// Creates a new helper message and registers it in <see cref="ScreenHelperManager.screenHelperMessages"/>
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="message"></param>
        /// <param name="portrait"></param>
        /// <param name="condition"></param>
        /// <param name="duration"></param>
        /// <param name="cooldown"></param>
        /// <param name="displayOutsideInventory"></param>
        /// <param name="onlyPlayOnce"></param>
        /// <param name="cantBeClickedOff"></param>
        /// <param name="persistsThroughSaves"></param>
        /// <param name="maxWidth"></param>
        /// <param name="fontSize"></param>
        /// <returns></returns>
        public static HelperMessage New(string identifier, string message, string portrait = "", ScreenHelperMessageCondition condition = null, float duration = 5, float cooldown = 60, bool displayOutsideInventory = true, bool onlyPlayOnce = true, bool cantBeClickedOff = false, bool persistsThroughSaves = true, int maxWidth = 380, float fontSize = 1f)
        {
            string txt = Language.GetOrRegister("Mods.CalRemix.Fanny." + identifier, () => message).Value;

            HelperMessage msg = new HelperMessage(identifier, txt, portrait, condition, duration, cooldown, displayOutsideInventory, onlyPlayOnce, cantBeClickedOff, persistsThroughSaves, maxWidth, fontSize);

            //Adds the message to the list
            ScreenHelperManager.screenHelperMessages.Add(msg);
            ScreenHelperManager.messageIndexByID.Add(identifier, ScreenHelperManager.screenHelperMessages.Count - 1);
            return msg;
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

        public HelperMessage NoCooldown()
        {
            NoCooldownAfterSpeaking = true;
            return this;
        }

        public HelperMessage SetPriority(float priority)
        {
            Priority = priority;
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
        /// Used by messages that need activation, set by <see cref="NeedsActivation()"/> and <see cref="ChainAfter(HelperMessage, float)"/> 
        /// </summary>
        internal bool _needsActivation = false;

        /// <summary>
        /// Timer used for messages that need activation. It's set to 1 by <see cref="ActivateMessage"/>, indicating its been activated, from which it starts counting up <br/>
        /// If the timer reaches 10 seconds, the message becomes deactivated
        /// </summary>
        public int activationTimer = 0;

        /// <summary>
        /// Makes it so that the message doesn't play on its own, and needs to be manually called by another message or event to play, using <see cref="ActivateMessage"/><br/>
        /// If the condition for the message isn't met, the message won't play even if activated
        /// </summary>
        public HelperMessage NeedsActivation()
        {
            _needsActivation = true;
            return this;
        }

        /// <summary>
        /// Manually activates a message, making it able to be played if its condition is met <br/>
        /// To make a message need manual activation, use <see cref="NeedsActivation"/>
        /// </summary>
        public void ActivateMessage()
        {
            //Increases the timer by 1, which makes it start counting up
            activationTimer++;
        }

        /// <summary>
        /// Sets a message to be played after another message ends (By default, uses the last created message)<br/>
        /// Can be configured to chain it after the other message begins playing instead
        /// </summary>
        /// <param name="chainFrom">The message that this message is spoken after. If null, uses the last loaded message</param>
        /// <param name="delay">The delay between the first message being spoken, and this one appearing</param>
        /// <param name="startTimerOnMessageSpoken">By default, the delay timer starts when the original message is clicked off. Set this to true so the timer starts as soon as the first message appears</param>
        /// <returns></returns>
        public HelperMessage ChainAfter(HelperMessage chainFrom = null, float delay = 1f, bool startTimerOnMessageSpoken = false)
        {
            //Sets the message to need the activation of the first message
            NeedsActivation();

            if (delay > 0)
                AddDelay(delay);

            //Uses the last registered message if no message is provided
            if (chainFrom == null)
                chainFrom = ScreenHelperManager.screenHelperMessages[^2];

            //Sets the first message to activate this one after tis played
            if (startTimerOnMessageSpoken)
                chainFrom.AddStartEvent(this.ActivateMessage);
            else
                chainFrom.AddEndEvent(this.ActivateMessage);

            return this;
        }

        /// <summary>
        /// Sets the <see cref="ScreenHelperManager.ongoingConversation"/> bool to true, which prevents any more helper messages from appearing when this message is spoken
        /// This is useful for moments when the helpers are intended to speak between one another without being interrupted by the wrong textbox
        /// </summary>
        /// <param name="conversationDurationInSeconds">Timer used to clear the convo lock after a certain amount of time has passed. Conversations can also be manually ended, this is just a failsave</param>
        /// <returns></returns>
        public HelperMessage InitiateConversation(float conversationDurationInSeconds = 120, float messagePriority = 100f)
        {
            Priority = messagePriority;
            AddSelectionEvent(() => InitiateConversationEvent(this, conversationDurationInSeconds));
            return this;
        }

        /// <summary>
        /// Sets the <see cref="ScreenHelperManager.ongoingConversation"/> bool to false, letting non-activated messages from appearing
        /// Use this at the end of conversations
        /// </summary>
        /// <param name="deadAirInSeconds">Timer used to delay the end of the conversation, to make it more natural (avoids another message being spoken RIGHT after te conversation</param>
        /// <returns></returns>
        public HelperMessage EndConversation(float deadAirInSeconds = 5)
        {
            AddEndEvent(() => EndConversationEvent(deadAirInSeconds));
            return this;
        }

        private void InitiateConversationEvent(HelperMessage conversationStarter, float conversationDurationInSecond)
        {
            ScreenHelperManager.ongoingConversation = true;
            ScreenHelperManager.ongoingConversationTimer = (int)(conversationDurationInSecond * 60);

            var helpers = ScreenHelperUISystem.UIState.ScreenHelpers;
            foreach (ScreenHelper helper in helpers)
            {
                //Ignore the message that we spoke about
                if (helper.StoredMessage == conversationStarter)
                    continue;

                //Hide messages that were in delay
                if (helper.HasMessageInDelay)
                {
                    helper.StoredMessage.alreadySeen = false;
                    helper.StopTalking();
                    continue;
                }

                if (helper.Speaking)
                {
                    HelperMessage spokenMsg = helper.UsedMessage;
                    //If the message was on screen for only half a second, hide it and pretend it wasnt already seen
                    if (spokenMsg.TimeLeft >= spokenMsg.messageDuration - 30)
                        spokenMsg.alreadySeen = false;

                    spokenMsg.EndMessage();
                    helper.StopTalking();
                }
            }
        }


        private void EndConversationEvent(float deadAirInSeconds)
        {
            if (!ScreenHelperManager.ongoingConversation)
                return;

            //Just end the lock
            if (deadAirInSeconds == 0)
            {
                ScreenHelperManager.ongoingConversation = false;
                return;
            }

            //Set the dead air time
            ScreenHelperManager.ongoingConversationTimer = (int)(deadAirInSeconds * 60);
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
        public event Action OnSelected;

        public bool HasAnyEndEvents => OnEnd != null;
        public bool HasAnyStartEvents => OnStart != null;
        public bool HasAnySelectedEvents => OnSelected != null;
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
        /// Adds an action that happens when the message gets selected to be played (called immediately, even when the message isn't immediately visible due to a delay)
        /// </summary>
        public HelperMessage AddSelectionEvent(Action action)
        {
            OnSelected += action;
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
                   (!_needsActivation || activationTimer > 0) &&                                    //Can't play messages that need activation that havent been activated yet
                   (DesiredSpeaker.CharacterAvailableCondition || IgnoreSpeakerSpecificCondition);  //Can't play messages if the speaker isn't avaialble

            //Technically the TimeLeft is not needed because when its active, no other message will try to play. But just in case
        }

        /// <summary>
        /// Makes the desired <see cref="ScreenHelper"/> speak this message <br/>
        /// Sets the message duration, checks it off as seen, and calls the <see cref="OnMessageStart"/> events
        /// </summary>
        public void PlayMessage(ScreenHelper speaker)
        {
            // Once the character has been seen, take note.  Make sure the
            // character is available and isn't just being forced to speak.
            if (speaker.CharacterAvailableCondition)
            {
                PersistentData.UnlockHelper(speaker.Name);
            }
            
            TimeLeft = messageDuration + delayTime;
            speaker.UsedMessage = this;
            currentSpeaker = speaker;
            alreadySeen = true;

            OnSelected?.Invoke();

            //if the helper has custom text, we change the formatting to match
            if (speaker.textboxFormatting != null)
            {
                if (textSize != speaker.textboxFormatting.defaultTextSize)
                {
                    textSize = speaker.textboxFormatting.defaultTextSize;
                }
                if (speaker.textboxFormatting.maximumWidth != 0 && speaker.textboxFormatting.maximumWidth != maxTextWidth)
                {
                    maxTextWidth = (int)speaker.textboxFormatting.maximumWidth;
                }
            }

            //Recalculate the text as its played if we have dynamic text segments, or if the speaker's custom formatting changed
            FormatText(speaker.textboxFont, maxTextWidth);

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
            currentSpeaker.OnStartUniversal?.Invoke();
            OnStart?.Invoke();

            //Hook
            ScreenHelperManager.OnMessageStartCall(this);
        }

        public void EndMessage()
        {
            OnEnd?.Invoke();
            CooldownTime = cooldownDuration;
            currentSpeaker = null;

            //Reset timer to play if we want to play it again later
            activationTimer = 0;

            //Hook
            ScreenHelperManager.OnMessageEndCall(this);
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
            //if the word is a linebreak, we just line break
            if (word == "\n")
            {
                currentLineLenght = 0;
                formattedSetence += "\n";
                return;
            }


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

        public override string ToString()
        {
            return Identifier + ": " + Text;
        }
    }

    public class ScreenHelperPortrait
    {
        public Asset<Texture2D> Texture;
        public int frameCount;
        public int animationSpeed;
        public int rows;
        public int loopPoint = 0;

        /// <summary>
        /// Registers a portrait that's used by a screen helper
        /// </summary>
        /// <param name="portraitName">The filename. The texture name will use this , prefixed with "Helper" when loading it</param>
        /// <param name="frameCount">How many frames this portrait has</param>
        /// <param name="animationSpeed">How fast the animation plays</param>
        public static void LoadPortrait(string portraitName, int frameCount, int animationSpeed = 11, int rows = 0, int loopPoint = 0)
        {
            ScreenHelperPortrait portrait = new ScreenHelperPortrait(portraitName, frameCount, animationSpeed, rows, loopPoint);
            //Load itself into the portrait list
            if (!ScreenHelperManager.Portraits.ContainsKey(portraitName))
                ScreenHelperManager.Portraits.Add(portraitName, portrait);
        }

        public ScreenHelperPortrait(string portrait, int frameCount, int animationSpeed = 11, int rows = 0, int loopPoint = 0)
        {
            Texture = ModContent.Request<Texture2D>("CalRemix/UI/Fanny/Helper" + portrait);
            this.frameCount = frameCount;
            this.animationSpeed = animationSpeed;

            this.rows = rows == 0 ? frameCount : rows;
            this.loopPoint = loopPoint;
        }
    }

    #region Data classes
    public class HelperPositionData
    {
        public Vector2 percentAnchor;
        public Vector2 pixelAnchor;
        public Vector2 pixelOffset;

        //Textbox stuff
        public Vector2 textboxOffsetFromCharacter;
        public Vector2 textSizeOffset;
        public Vector2 textboxFadeInOffset;
        public Vector2 textboxFadeOutOffset;
        public Vector2 textboxTextOffset;

        public Vector2 itemOffset;

        /// <param name="percentAnchor">Top left position as a percentage of the screen</param>
        /// <param name="pixelAnchor">Top left position as pixel coordinates (Added ontop of the percent anchor)</param>
        /// <param name="appearOffset">Offset applied to the position when the helper appears (to make them slide in and out)</param>
        /// <param name="textboxOffset">Offset from the bottom middle of the helper to the top left corner of their textbox</param>
        /// <param name="offsetByTextSize">Offset to the top left corner of the textbox, based on the size of the text. Set it to X = 1 if the helper is to the right of their textbox and you need the textbox's left corner to be pushed further to the left when the helper's textbox gets wider</param>
        /// <param name="textboxFadeInOffset">Offset applied to the textbox before it is fully faded in</param>
        /// <param name="textboxFadeOutOffset">Offset applied to the textbox as it fades out</param>
        /// <param name="itemOffset"></param>
        /// <param name="textboxTextOffset">Offset between the top left corner of the textbox and the start of the actual text</param>
        public HelperPositionData(Vector2 percentAnchor, Vector2 pixelAnchor, Vector2 appearOffset,
            Vector2 textboxOffset, Vector2 offsetByTextSize, Vector2 textboxFadeInOffset, Vector2 textboxFadeOutOffset,
            Vector2? itemOffset = null, Vector2? textboxTextOffset = null)
        {
            this.percentAnchor = percentAnchor;
            this.pixelAnchor = pixelAnchor;
            this.pixelOffset = appearOffset;

            textboxOffsetFromCharacter = textboxOffset;
            textSizeOffset = offsetByTextSize;
            this.textboxFadeInOffset = textboxFadeInOffset;
            this.textboxFadeOutOffset = textboxFadeOutOffset;

            this.itemOffset = itemOffset ?? new Vector2(0, -90);
            this.textboxTextOffset = textboxTextOffset ?? Vector2.Zero;
        }

        public void SetPosition(ScreenHelper helper)
        {
            Vector2 pixelPosition = pixelAnchor + pixelOffset * MathF.Pow(helper.fadeIn, 0.4f);

            helper.Left.Set(pixelPosition.X, percentAnchor.X);
            helper.Top.Set(pixelPosition.Y, percentAnchor.Y);

            helper.Recalculate();
        }


        public void SetTextboxPosition(ScreenHelperTextbox textbox)
        {
            ScreenHelper speaker = textbox.ParentSpeaker;

            //Base position is obtained from the parent speakers position + some offset
            Vector2 basePosition = speaker.BasePosition + textboxOffsetFromCharacter;

            //Measure the text
            Vector2 textSize = speaker.textboxFont.MeasureString(speaker.UsedMessage.Text) * speaker.UsedMessage.textSize;

            //Get the corner position by offsetting the corner by the text's size
            //Can be modified so the text size affects or not the position
            Vector2 cornerPosition = basePosition - textSize * textSizeOffset;

            //Account for padding
            cornerPosition -= Vector2.One * (textbox.backgroundPadding + textbox.outlineThickness);
            //Fade out when the message is about to end
            cornerPosition += textboxFadeOutOffset * MathF.Pow(Utils.GetLerpValue(30, 0, speaker.UsedMessage.TimeLeft, true), 1.6f);
            //Fade in when the speaker appears
            cornerPosition += textboxFadeInOffset * MathF.Pow(1 - speaker.fadeIn, 2f);

            //Calculate the size of the final box
            Vector2 boxSize = textSize + Vector2.One * (textbox.backgroundPadding + textbox.outlineThickness) * 2;
            if (speaker.textboxFormatting != null)
            {
                boxSize.X = Math.Max(boxSize.X, speaker.textboxFormatting.minimumDimensions.X);
                boxSize.Y = Math.Max(boxSize.Y, speaker.textboxFormatting.minimumDimensions.Y);
            }

            textbox.Width.Set(boxSize.X, 0);
            textbox.Height.Set(boxSize.Y, 0);
            textbox.Left.Set(cornerPosition.X, 0);
            textbox.Top.Set(cornerPosition.Y, 0);
        }
    }

    /// <summary>
    /// Palette for the textbox's default appearance, drawn below textures from <see cref="HelperTextboxTheme"/>
    /// </summary>
    public class HelperTextboxPalette
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

    /// <summary>
    /// Texture theme for the textbox, including a 9 slice edge, and a background texture that can be animated
    /// </summary>
    public class HelperTextboxTheme
    {
        public Asset<Texture2D> border9Slice;
        public Asset<Texture2D> backgroundFill;


        public Vector2 border9SliceCornerSize;
        public Vector2 backgroundPadding;

        public Point backgroundFrameCount;
        public int backgroundFrameTime;

        private int bgAnimationFrameTotal;
        private int bgAnimationFrame;
        private int bgAnimationFrameCounter;

        public HelperTextboxTheme(string border9SliceTexture, Vector2 border9SliceSize, string backgroundFill = null, Vector2? backgroundPadding = null, Point? backgroundFrameCount = null, int backgroundFrameTime = 1)
        {
            if (border9SliceTexture != null)
            {
                border9Slice = ModContent.Request<Texture2D>("CalRemix/UI/Fanny/Helper" + border9SliceTexture);
                border9SliceCornerSize = border9SliceSize;
            }

            if (backgroundFill != null)
            {
                this.backgroundFill = ModContent.Request<Texture2D>("CalRemix/UI/Fanny/Helper" + backgroundFill);
                this.backgroundFrameCount = backgroundFrameCount ?? new Point(1, 1);
                this.backgroundFrameTime = backgroundFrameTime;
                this.backgroundPadding = backgroundPadding ?? Vector2.Zero;

                bgAnimationFrameTotal = this.backgroundFrameCount.X * this.backgroundFrameCount.Y;
            }
        }

        public void Animate()
        {
            bgAnimationFrameCounter++;
            if (bgAnimationFrameCounter >= backgroundFrameTime)
            {
                bgAnimationFrameCounter = 0;
                bgAnimationFrame++;
                if (bgAnimationFrame >= bgAnimationFrameTotal)
                    bgAnimationFrame = 0;
            }
        }

        public Rectangle GetFrame()
        {
            int frameX = bgAnimationFrame % backgroundFrameCount.X;
            int frameY = bgAnimationFrame / backgroundFrameCount.Y;

            return backgroundFill.Value.Frame(backgroundFrameCount.X, backgroundFrameCount.Y, frameX, frameY);
        }
    }

    /// <summary>
    /// Formatting information for the textbox, including default font size and minimum & maximum dimensions
    /// </summary>
    public class HelperTextboxFormatting
    {
        public Vector2 minimumDimensions;
        public int maximumWidth;
        public float defaultTextSize;


        public HelperTextboxFormatting(Vector2 minimumDimensions, int maximumWidth, float defaultTextSize = 1f)
        {
            this.minimumDimensions = minimumDimensions;
            this.maximumWidth = maximumWidth;
            this.defaultTextSize = defaultTextSize;
        }
    }

    /// <summary>
    /// Helper methods for assorted Helpers.
    /// </summary>
    public class HelperHelpers
    {
        /// <summary>
        /// Returns the the time in frames with a level of noise applied. For use with "Mediumweight" Helpers, like Flux or the Queen of Clubs.
        /// </summary>
        /// <param name="timeInput"> The baseline value of time, in minutes. </param>
        /// <param name="noiseSubtract"> The maximum amount of time which could be subtracted from the end result. Defaults to 3 minutes. </param>
        /// <param name="noiseAdd"> The maximum amount of time which could be added to the end result. Defaults to 3 minutes. </param>
        public static int GetTimeUntilNextStage(float timeInput, float noiseSubtract = 3, float noiseAdd = 3)
        {
            float timeToReturn = timeInput;

            // turn int into minutes
            timeToReturn *= (float)Math.Pow(60, 2);
            // add layer of noise
            int noiseSubtractFrames = (int)(-noiseSubtract * Math.Pow(60, 2));
            int noiseAddFrames = (int)(noiseAdd * Math.Pow(60, 2) + 1);
            timeToReturn += Main.rand.Next(noiseSubtractFrames, noiseAddFrames);

            //timeToReturn = 120;
            return (int)timeToReturn;
        }
    }

    #endregion
}