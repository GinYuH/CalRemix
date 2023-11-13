using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Furniture.CraftingStations;
using CalamityMod.Items.Potions;
using CalamityMod.NPCs.Yharon;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
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
    public class Fanny : UIElement
    {
        public Vector2 BasePosition => GetDimensions().ToRectangle().Bottom();
        public FannyTextbox SpeechBubble;
        public SoundStyle speakingSound;
        public FannyTextboxPalette textboxPalette;

        public string textboxHoverText;

        public float fadeIn;
        public bool flipped;
        public bool idlesInInventory;

        public float distanceFromEdge = 240;

        private int fanFrame;
        private int fanFrameCounter;

        //Shake wwhen saying a new message in the inventory
        public bool needsToShake;
        public float shakeTime;

        //Bounce and tickle anims when hovered / clicked
        public float bounce;
        public float tickle;


        //Small break between messages
        public int talkCooldown;

        //Default placeholder message used when not speaking
        public FannyMessage NoMessage = new FannyMessage("None", "", "Idle", displayOutsideInventory: false);

        //Message currently being spoken
        private FannyMessage currentMessage;

        public bool Speaking => currentMessage != null && !currentMessage.InDelayPeriod;

        public FannyMessage UsedMessage
        {
            get => Speaking ? currentMessage : NoMessage;
            set
            {
                currentMessage = value;
                SpeechBubble.Recalculate();
            }
        }

        public bool CanSpeak => currentMessage == null && talkCooldown <= 0;

        /// <summary>
        /// Plays the desired message, ignoring any condition the message may have
        /// </summary>
        public void TalkAbout(FannyMessage message)
        {
            if (!CanSpeak || !message.CanPlayMessage())
                return;

            message.PlayMessage(this);
        }

        /// <summary>
        /// Stops talking about the current message and goes on cooldown for that message
        /// </summary>
        public void StopTalking()
        {
            currentMessage = null;
            talkCooldown = 60;
        }

        public FannyTextboxPalette UsedPalette => (currentMessage != null && currentMessage.paletteOverride.HasValue) ? currentMessage.paletteOverride.Value : textboxPalette;

        public override void OnInitialize()
        {
            OnLeftClick += TickleTheRepugnantFuck;
        }

        private void TickleTheRepugnantFuck(UIMouseEvent evt, UIElement listeningElement)
        {
            if (!FannyManager.fannyEnabled)
                return;
            tickle = Math.Max(tickle, 0) + 1;
            SoundEngine.PlaySound(SoundID.DD2_GoblinScream with { MaxInstances = 0 });
        }

        public override void Update(GameTime gameTime)
        {
            if (!FannyManager.fannyEnabled)
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
                if (Main.playerInventory && fadeIn == 1)
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
            if (!FannyManager.fannyEnabled)
            {
                return;
            }
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


            spriteBatch.Draw(fannySprite, position, frame, Color.White * fadeIn, 0, new Vector2(frame.Width / 2f, frame.Height), 1f, 0, 0);
            if (Speaking && UsedMessage.ItemType != -22)
                DrawItem();
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

    public class FannyTextbox : UIElement
    {
        public Fanny ParentFanny;
        public int outlineThickness = 3;
        public int backgroundPadding = 10;

        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);

            //Set the timeleft of the message to 30
            if (ParentFanny.Speaking && ParentFanny.fadeIn == 1 && ParentFanny.UsedMessage.NeedsToBeClickedOff && ParentFanny.UsedMessage.TimeLeft > 30)
                ParentFanny.UsedMessage.TimeLeft = 30;
        }

        public override void Recalculate()
        {
            Vector2 offset = new Vector2(50, 90);
            if (ParentFanny.flipped)
                offset.X *= -1;

            Vector2 basePosition = ParentFanny.BasePosition - offset;
            Vector2 textSize = FontAssets.MouseText.Value.MeasureString(ParentFanny.UsedMessage.Text) * ParentFanny.UsedMessage.textSize;
            Vector2 cornerPosition = basePosition - textSize;

            if (ParentFanny.flipped)
                cornerPosition = basePosition - Vector2.UnitY * textSize.Y;

            //Account for padding
            textSize += Vector2.One * (backgroundPadding + outlineThickness) * 2;
            cornerPosition -= Vector2.One * (backgroundPadding + outlineThickness);

            //Fade out
            cornerPosition.Y -= MathF.Pow(Utils.GetLerpValue(30, 0, ParentFanny.UsedMessage.TimeLeft, true), 1.6f) * 30f;
            //Fade in
            cornerPosition.Y += MathF.Pow(1 - ParentFanny.fadeIn, 2f) * 40;

            Width.Set(textSize.X, 0);
            Height.Set(textSize.Y, 0);
            Left.Set(cornerPosition.X, 0);
            Top.Set(cornerPosition.Y, 0);
            base.Recalculate();
        }


        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (!ParentFanny.Speaking)
                return;

            FannyTextboxPalette palette = ParentFanny.UsedPalette;

            // a shit ton of variables
            var font = FontAssets.MouseText.Value;
            string text = ParentFanny.UsedMessage.Text;

            Rectangle dimensions = GetDimensions().ToRectangle();

            Vector2 outlineDrawPosition = dimensions.TopLeft();
            Vector2 backgroundDrawPosition = outlineDrawPosition + Vector2.One * outlineThickness;
            Vector2 textDrawPosition = backgroundDrawPosition + Vector2.One * backgroundPadding;

            Vector2 outlineSize = dimensions.Size();
            Vector2 backgroundSize = outlineSize - Vector2.One * outlineThickness * 2;

            Texture2D squareTexture = TextureAssets.MagicPixel.Value;
            float opacity = ParentFanny.fadeIn * Utils.GetLerpValue(0, 30, ParentFanny.UsedMessage.TimeLeft, true);

            // draw the border as a large rectangle behind, and the inners as a slightly smaller rectangle infront
            Main.spriteBatch.Draw(squareTexture, outlineDrawPosition, null, palette.outline * opacity, 0, Vector2.Zero, outlineSize / squareTexture.Size(), 0, 0);
            Main.spriteBatch.Draw(squareTexture, backgroundDrawPosition, null, palette.background * opacity, 0, Vector2.Zero, backgroundSize / squareTexture.Size(), 0, 0);


            if (ContainsPoint(Main.MouseScreen) && ParentFanny.fadeIn == 1 && ParentFanny.UsedMessage.NeedsToBeClickedOff && ParentFanny.UsedMessage.TimeLeft > 30)
            {
                Main.spriteBatch.Draw(squareTexture, backgroundDrawPosition, null, palette.backgroundHover with { A = 0 } * (0.4f + 0.2f * MathF.Sin(Main.GlobalTimeWrappedHourly * 4f)) * opacity, 0, Vector2.Zero, backgroundSize / squareTexture.Size(), 0, 0);
                Main.LocalPlayer.mouseInterface = true;

                string hoverText = ParentFanny.textboxHoverText;
                if (ParentFanny.UsedMessage.hoverTextOverride != "")
                    hoverText = ParentFanny.UsedMessage.hoverTextOverride;
                Main.instance.MouseText(hoverText);
            }

            // finally draw the text
            Utils.DrawBorderStringFourWay(Main.spriteBatch, font, text, textDrawPosition.X, textDrawPosition.Y, palette.text * (Main.mouseTextColor / 255f) * opacity, palette.textOutline * opacity, Vector2.Zero, ParentFanny.UsedMessage.textSize);
        }
    }

    public class FannyUIState : UIState
    {
        public static Fanny FannyTheFire = new Fanny();
        public static Fanny EvilFanny = new Fanny();
        public static Fanny WonderFlower = new Fanny();

        public override void OnInitialize()
        {
            LoadFanny(FannyTheFire, "Thank you for the help, Fanny!", false, true, SoundID.Cockatiel with { MaxInstances = 0, Volume = 0.3f, Pitch = -0.8f }, "Idle");
            LoadFanny(EvilFanny, "Get away, Evil Fanny!", true, false, SoundID.DD2_DrakinShot with { MaxInstances = 0, Volume = 0.3f, Pitch = 0.8f }, "EvilIdle", distanceFromEdge: 120,
                textboxPalette: new FannyTextboxPalette(Color.Black, Color.Red, Color.Indigo, Color.DeepPink, Color.Tomato));
            LoadFanny(WonderFlower, "Oooh! So exciting!", false, false, FannyManager.WonderFannyVoice, "TalkingFlower", verticalOffset: 0.3f, distanceFromEdge: 240,
               textboxPalette: new FannyTextboxPalette(Color.Black, Color.Transparent, new Color(250, 250, 250), Color.White, Color.Black * 0.4f));
        }

        public Fanny LoadFanny(Fanny fanny, string hoverText, bool flipped, bool idlesInInventory, SoundStyle voice, string emptyMessagePortrait, float verticalOffset = 0f, float distanceFromEdge = 240f, FannyTextboxPalette? textboxPalette = null)
        {
            fanny.Left.Set(-80, 1);
            fanny.Top.Set(-160, 1 - verticalOffset);
            fanny.Height.Set(80, 0f);
            fanny.Width.Set(80, 0f);

            Append(fanny);

            FannyTextbox textbox = new FannyTextbox();

            textbox.Height.Set(0, 0f);
            textbox.Width.Set(0, 0f);
            textbox.ParentFanny = fanny;
            Append(textbox);
            fanny.SpeechBubble = textbox;

            fanny.textboxHoverText = hoverText;
            fanny.flipped = flipped;
            fanny.idlesInInventory = idlesInInventory;
            fanny.speakingSound = voice;
            fanny.NoMessage = new FannyMessage("", "", emptyMessagePortrait, displayOutsideInventory: false);
            fanny.distanceFromEdge = distanceFromEdge;

            if (textboxPalette.HasValue)
                fanny.textboxPalette = textboxPalette.Value;
            else
                fanny.textboxPalette = new FannyTextboxPalette();

            return fanny;
        }

        public void StopAllDialogue()
        {
            foreach (UIElement element in Elements)
            {
                if (element is Fanny fanny)
                    fanny.StopTalking();
            }
        }

        public bool AnyAvailableFanny()
        {
            return Elements.Any(ui => ui is Fanny fanny && fanny.CanSpeak);
        }
    }

    // This class will only be autoloaded/registered if we're not loading on a server
    [Autoload(Side = ModSide.Client)]
    internal class FannyUISystem : ModSystem
    {
        private UserInterface FannyInterface;
        internal static FannyUIState UIState;

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
                    InterfaceScaleType.UI)
                );
            }
        }
    }

    public class FannySavePlayer : ModPlayer
    {
        public bool[] readMessages;

        public bool readTroughFannyDogDialogue = false;


        public override void SaveData(TagCompound tag)
        {
            for (int i = 0; i < FannyManager.fannyMessages.Count; i++)
            {
                FannyMessage msg = FannyManager.fannyMessages[i];
                if (msg.alreadySeen && msg.PersistsThroughSaves)
                    tag["FannyDialogue" + msg.Identifier] = true;
            }

            tag["FannyReadThroughDogDialogue"] = FannyManager.ReadAllDogTips;
        }

        public override void LoadData(TagCompound tag)
        {
            readMessages = new bool[FannyManager.fannyMessages.Count];
            for (int i = 0; i < FannyManager.fannyMessages.Count; i++)
            {
                FannyMessage msg = FannyManager.fannyMessages[i];
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

            if (readMessages is null || readMessages.Length < FannyManager.fannyMessages.Count)
            {

                for (int i = 0; i < FannyManager.fannyMessages.Count; i++)
                {
                    FannyMessage msg = FannyManager.fannyMessages[i];
                    msg.alreadySeen = false;
                }
                return;
            }


            for (int i = 0; i < FannyManager.fannyMessages.Count; i++)
            {
                FannyMessage msg = FannyManager.fannyMessages[i];
                msg.alreadySeen = readMessages[i];
            }

            FannyManager.ReadAllDogTips = readTroughFannyDogDialogue;
        }
    }

    [Autoload(Side = ModSide.Client)]
    public partial class FannyManager : ModSystem
    {
        public static List<FannyMessage> fannyMessages = new List<FannyMessage>();
        public static Dictionary<string, FannyPortrait> Portraits = new Dictionary<string, FannyPortrait>();
        public static bool fannyEnabled = true;
        public static int fannyTimesFrozen = 0;

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
        }


        public static void LoadFannyPortraits()
        {
            FannyPortrait.LoadPortrait("Idle", 8);
            FannyPortrait.LoadPortrait("Awooga", 4);
            FannyPortrait.LoadPortrait("Cryptid", 1);
            FannyPortrait.LoadPortrait("Sob", 4);
            FannyPortrait.LoadPortrait("Nuhuh", 19);

            FannyPortrait.LoadPortrait("EvilIdle", 1);

            LoadWonderFlowerPortraits();
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
            fannyMessages.Add(new FannyMessage("LowHP", "It looks like you're low on health. If your health reaches 0, you'll die. To combat this, don't let your health reach 0!",
                "Nuhuh", (FannySceneMetrics scene) => Main.LocalPlayer.statLife < Main.LocalPlayer.statLifeMax2 * 0.25f, cooldown: 1200, onlyPlayOnce: false).SetHoverTextOverride("Thanks Fanny! I'll heal."));

            fannyMessages.Add(new FannyMessage("Invisible", "Where did you go?",
                "Sob", (FannySceneMetrics scene) => Main.LocalPlayer.invis || Main.LocalPlayer.shroomiteStealth || Main.LocalPlayer.vortexStealthActive || (Main.LocalPlayer.Calamity().rogueStealth >= Main.LocalPlayer.Calamity().rogueStealthMax && Main.LocalPlayer.Calamity().rogueStealthMax > 0), onlyPlayOnce: true).SetHoverTextOverride("I'm still here Fanny!"));
        }
        #endregion

        public override void PostUpdateEverything()
        {
            if (Main.dedServ)
                return;

            UpdateLoreCommentTracking();

            //Tick down message times
            for (int i = 0; i < fannyMessages.Count; i++)
            {
                FannyMessage msg = fannyMessages[i];

                //Fallback
                if (msg.DesiredFanny == null)
                    msg.DesiredFanny = FannyUIState.FannyTheFire;

                if (msg.timerToPlay > 0 && msg.timeToWaitBeforePlaying > msg.timerToPlay)
                    msg.timerToPlay++;

                //Tick down the cooldown
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
                else if (msg.speakingFanny != null)
                {
                    msg.speakingFanny.StopTalking();
                    msg.StartCooldown();
                }

                //Do the start effects with a delay
                if (msg.delayTime > 0 && msg.TimeLeft == msg.MessageDuration)
                    msg.OnMessageStart();
            }

            //Don't even try looking for a new message if speaking already / On speak cooldown
            if (FannyUISystem.UIState.AnyAvailableFanny())
            {

                FannySceneMetrics scene = new FannySceneMetrics();
                //Precalculate screen NPCs to avoid repeated checks over all npcs everytime
                Rectangle screenRect = new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight);
                scene.onscreenNPCs = Main.npc.Where(n => n.active && n.Hitbox.Intersects(screenRect));

                //Split the messages per fanny speaking
                var messageGroups = fannyMessages.GroupBy(m => m.DesiredFanny);
                foreach (var messageGroup in messageGroups)
                {
                    Fanny speakingFanny = messageGroup.First().DesiredFanny;

                    //Check if the fanny in question can speak
                    if (!speakingFanny.CanSpeak)
                        continue;

                    foreach (FannyMessage message in messageGroup)
                    {
                        if (message.CanPlayMessage() && message.Condition(scene))
                        {
                            message.PlayMessage(speakingFanny);
                            break;
                        }
                    }
                }
            }

            previousHoveredItem = Main.HoverItem.type;
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

        public static bool HasDesertMedallionMaterials(FannySceneMetrics scene)
        {
            List<int> materials = new List<int>();
            {
                materials.Add(ItemID.AntlionMandible);
                materials.Add(ItemID.SandBlock);
                materials.Add(ModContent.ItemType<StormlionMandible>());
            }

            return Main.LocalPlayer.HasItems(materials) && !DownedBossSystem.downedDesertScourge;
        }

        public static bool CrossModBoss(FannySceneMetrics scene, string ModName, string NPCName)
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

        public static bool YharonPhase2(FannySceneMetrics scene)
        {
            if (scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<Yharon>() && n.ai[0] == 17f))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Saving and Loading data
        public override void ClearWorld()
        {
            //Stop all fannies talking, and close every dialogue
            FannyUISystem.UIState.StopAllDialogue();
            for (int i = 0; i < fannyMessages.Count; i++)
            {
                FannyMessage msg = fannyMessages[i];
                msg.timerToPlay = 0;
                msg.speakingFanny = null;
                msg.TimeLeft = 0;
                msg.CooldownTime = 0;
                msg.alreadySeen = false;
            }
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

        public string Identifier;

        public int CooldownTime { get; set; }
        private int cooldownDuration;

        public int TimeLeft { get; set; }
        internal int messageDuration;
        public int MessageDuration => messageDuration;

        //Which fanny the message wants to be spoken by
        public Fanny DesiredFanny;

        //The fanny actively speaking the message. For cases where we want one fanny to say what the other fanny says
        public Fanny speakingFanny;


        public bool alreadySeen = false;

        public bool DisplayOutsideInventory { get; set; }
        public bool OnlyPlayOnce { get; set; }
        public bool NeedsToBeClickedOff { get; set; }
        public bool PersistsThroughSaves { get; set; }

        public FannyPortrait Portrait { get; set; }

        public FannyTextboxPalette? paletteOverride = null;
        public string hoverTextOverride = "";
        public SoundStyle? voiceOverride = null;

        public delegate string DynamicFannyTextSegment();
        public static string GetPlayerName() => Main.LocalPlayer.name;
        public static string GetWorldName() => Main.worldName;

        public List<DynamicFannyTextSegment> textSegments = new List<DynamicFannyTextSegment>();


        public FannyMessage(string identifier, string message, string portrait = "", FannyMessageCondition condition = null, float duration = 5, float cooldown = 60, bool displayOutsideInventory = true, bool onlyPlayOnce = true, bool needsToBeClickedOff = true, bool persistsThroughSaves = true, int maxWidth = 380, float fontSize = 1f)
        {
            //Unique identifier for saving data
            Identifier = identifier;
            textSize = fontSize;

            maxTextWidth = maxWidth;
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
            PersistsThroughSaves = persistsThroughSaves;

            if (portrait == "")
                portrait = "Idle";
            Portrait = FannyManager.Portraits[portrait];

            //default
            DesiredFanny = FannyUIState.FannyTheFire;
        }

        /// <summary>
        /// Makes the message be spoken by evil fanny
        /// </summary>
        public FannyMessage SpokenByEvilFanny()
        {
            DesiredFanny = FannyUIState.EvilFanny;
            return this;
        }

        /// <summary>
        /// Makes the message get spoken by the specified fanny
        /// </summary>
        public FannyMessage SpokenByAnotherFanny(Fanny speakingFanny)
        {
            DesiredFanny = speakingFanny;
            return this;
        }

        /// <summary>
        /// Adds a custom textbox palette override for this message
        /// </summary>
        public FannyMessage SetPalette(FannyTextboxPalette palette)
        {
            paletteOverride = palette;
            return this;
        }

        public FannyMessage SetHoverTextOverride(string hoverTextOverride)
        {
            this.hoverTextOverride = hoverTextOverride;
            return this;
        }

        public FannyMessage SetSoundOverride(SoundStyle soundStyleOverride)
        {
            this.voiceOverride = soundStyleOverride;
            return this;
        }

        #region Message Condition stuff

        public delegate bool FannyMessageCondition(FannySceneMetrics sceneMetrics);
        public static bool NeverShow(FannySceneMetrics sceneMetrics) => false;
        public static bool AlwaysShow(FannySceneMetrics sceneMetrics) => true;

        public FannyMessageCondition Condition { get; set; }

        public int timerToPlay = 0;
        public int timeToWaitBeforePlaying = 0;

        /// <summary>
        /// Makes it so that the message will never play on its own, and needs both its condition to be met, and <see cref="ActivateMessage"/> to be called for it to be read
        /// If the condition for the message isn't met, the message won't play even if activated
        /// </summary>
        public FannyMessage NeedsActivation()
        {
            timeToWaitBeforePlaying = 1;
            return this;
        }

        /// <summary>
        /// Makes it so that the message doesn't play on its own, and needs to be manually called by another message or event to play, using <see cref="ActivateMessage"/><br/>
        /// If the condition for the message isn't met, the message won't play even if activated
        /// </summary>
        /// <param name="timer">Delay period after <see cref="ActivateMessage"/> is called where the message waits to play, in seconds</param>
        public FannyMessage NeedsActivation(float delay = 1)
        {
            timeToWaitBeforePlaying = (int)(delay * 60);
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

        public int delayTime = 0;
        public bool InDelayPeriod => TimeLeft > messageDuration;

        /// <summary>
        /// Adds a delay time before the message starts playing, when its activation condition is met<br/>
        /// No other message can play while the delay period is started
        /// </summary>
        /// <param name="delay"></param>
        /// <returns></returns>
        public FannyMessage AddDelay(float delay = 1)
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
        public FannyMessage AddStartEvent(Action action)
        {
            OnStart += action;
            return this;
        }

        /// <summary>
        /// Adds an action that happens when the message goes away
        /// </summary>
        public FannyMessage AddEndEvent(Action action)
        {
            OnEnd += action;
            return this;
        }


        //Technically the TimeLeft is not needed because when its active, no other message will try to play. But just in case
        public bool CanPlayMessage()
        {
            return CooldownTime <= 0 &&                                     //Can't play messages on cooldown
                   TimeLeft <= 0 &&                                         //Can't play messages that are already playing
                   (!OnlyPlayOnce || !alreadySeen) &&                       //Can't play messages that are only played once, more than once
                   (DisplayOutsideInventory || Main.playerInventory) &&     //Can't play messages that only display in the inventory outside of the inventory
                   timeToWaitBeforePlaying <= timerToPlay;                  //Can't play messages with a timer before the timer is reached
        }

        public void PlayMessage(Fanny fanny)
        {
            TimeLeft = messageDuration + delayTime;
            fanny.UsedMessage = this;
            speakingFanny = fanny;
            alreadySeen = true;

            //Recalculate the text as its played if we have dynamic text segments
            if (textSegments.Count > 0)
                FormatText(FontAssets.MouseText.Value, maxTextWidth);

            //Immediately play message start effects
            if (delayTime == 0)
                OnMessageStart();
        }

        public void OnMessageStart()
        {
            if (speakingFanny is null)
                return;

            speakingFanny.needsToShake = true;
            SoundEngine.PlaySound(SoundID.MenuOpen);
            SoundEngine.PlaySound(voiceOverride ?? speakingFanny.speakingSound);
            OnStart?.Invoke();
        }

        public void StartCooldown()
        {
            OnEnd?.Invoke();
            CooldownTime = cooldownDuration;
            speakingFanny = null;

            //Reset timer to play if we want to play it again later
            timerToPlay = 0;
        }
        #endregion

        #region Item display
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
                foreach (DynamicFannyTextSegment dynamicText in textSegments)
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
        public FannyMessage AddDynamicText(DynamicFannyTextSegment customText)
        {
            if (originalText.Contains("$" + textSegments.Count.ToString()))
                textSegments.Add(customText);
            return this;
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

    public struct FannyTextboxPalette
    {
        public Color background = Color.SaddleBrown;
        public Color backgroundHover = Color.SaddleBrown;
        public Color outline = Color.Magenta;
        public Color text = Color.Lime;
        public Color textOutline = Color.DarkBlue;

        public FannyTextboxPalette() { }

        public FannyTextboxPalette(Color text, Color textOutline, Color background, Color backgroundHover, Color outline)
        {
            this.text = text;
            this.textOutline = textOutline;
            this.background = background;
            this.backgroundHover = backgroundHover;
            this.outline = outline;
        }
    }
}