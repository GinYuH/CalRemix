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
using Terraria.Audio;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.Items.SummonItems;
using Steamworks;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.NPCs.HiveMind;
using CalamityMod.NPCs.Perforator;
using CalamityMod.NPCs.Yharon;
using CalamityMod.NPCs.DevourerofGods;
using CalamityMod.Items;
using CalamityMod.NPCs.ExoMechs;
using CalamityMod.NPCs.ExoMechs.Artemis;
using CalamityMod.NPCs.ExoMechs.Thanatos;
using CalamityMod.NPCs.ExoMechs.Ares;
using CalamityMod.NPCs;
using CalamityMod.NPCs.SupremeCalamitas;
using CalamityMod.Items.Pets;
using CalRemix.Items.Materials;
using CalamityMod.Items.Mounts;
using CalamityMod.Items.Placeables.Furniture;
using CalRemix.Items.Placeables;
using CalamityMod.NPCs.CalClone;
using CalamityMod.NPCs.AcidRain;
using CalamityMod.NPCs.TownNPCs;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.NPCs.AstrumDeus;
using CalamityMod.Items.Weapons.Rogue;

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
            tickle = Math.Max(tickle, 0) + 1;
            SoundEngine.PlaySound(SoundID.DD2_GoblinScream with { MaxInstances = 0 });
        }

        public override void Update(GameTime gameTime)
        {
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

        public override void OnInitialize()
        {
            LoadFanny(FannyTheFire, "Thank you for the help, Fanny!", false, true, SoundID.Cockatiel with { MaxInstances = 0, Volume = 0.3f, Pitch = -0.8f }, "Idle");
            LoadFanny(EvilFanny, "Get away, Evil Fanny!", true, false, SoundID.DD2_DrakinShot with { MaxInstances = 0, Volume = 0.3f, Pitch = 0.8f }, "EvilIdle", distanceFromEdge: 120,
                textboxPalette: new FannyTextboxPalette(Color.Black, Color.Red, Color.Indigo, Color.DeepPink, Color.Tomato));
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

    public partial class FannyManager : ModSystem
    {
        public static List<FannyMessage> fannyMessages = new List<FannyMessage>();
        public static Dictionary<string, FannyPortrait> Portraits = new Dictionary<string, FannyPortrait>();

        #region Loading
        public override void Load()
        {
            LoadFannyPortraits();
            LoadGeneralFannyMessages();
            LoadDogSpamMessages();
            LoadLoreComments();
        }


        public static void LoadFannyPortraits()
        {
            FannyPortrait.LoadPortrait("Idle", 8);
            FannyPortrait.LoadPortrait("Awooga", 4);
            FannyPortrait.LoadPortrait("Cryptid", 1);
            FannyPortrait.LoadPortrait("Sob", 4);
            FannyPortrait.LoadPortrait("Nuhuh", 19);

            FannyPortrait.LoadPortrait("EvilIdle", 1);
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
            fannyMessages.Add(new FannyMessage("Intro", "Hello there! I'm Fanny the Flame, your personal guide to assist you with traversing this dangerous world. I wish you good luck on your journey and a Fan-tastic time!",
                "Idle", FannyMessage.AlwaysShow, displayOutsideInventory: true));

            #region Passive
            fannyMessages.Add(new FannyMessage("MeldGunk", "Fear the Meld Gunk.",
                "Idle", (FannySceneMetrics scene) => Main.hardMode && Main.LocalPlayer.InModBiome<UndergroundAstralBiome>(), onlyPlayOnce: false, cooldown: 120));
            
            fannyMessages.Add(new FannyMessage("FungusGarden", "Careful when exploring the Shroom Garden. I hear some rather large crustaceans make their home there. Wouldn't want to be turned into Delicious Meat!",
    "Nuhuh", (FannySceneMetrics scene) => Main.rand.NextBool(2160000) && !DownedBossSystem.downedCrabulon, cooldown: 120));

            fannyMessages.Add(new FannyMessage("ProbablyYakuza", "One time, I saw someone being dragged into a car by three men. The men took around 10 minutes and 23 seconds to subdue their victim, and  2 more minutes to drive away. I did nothing to stop it.",
            "Nuhuh", (FannySceneMetrics scene) => Main.rand.NextBool(1500000)));

            fannyMessages.Add(new FannyMessage("Fuckyou", "You are now manually breathing.",
               "Nuhuh", (FannySceneMetrics scene) => Main.rand.NextBool(4000000)));

            fannyMessages.Add(new FannyMessage("Mount", "Do a barrel roll on that thing you're riding!",
               "Awooga", (FannySceneMetrics scene) => Main.rand.NextBool(1000) && Main.LocalPlayer.mount.Type != MountID.None));

            fannyMessages.Add(new FannyMessage("Creepy", Main.rand.Next(1000000) + " remaining...",
   "Cryptid", (FannySceneMetrics scene) => Main.rand.NextBool(100000000), duration: 60, needsToBeClickedOff: false));

            fannyMessages.Add(new FannyMessage("Mhage", "Be careful when using magic weapons. Drinking too many mana potions can drain your health, and leave you vulnerable to enemy attacks.",
               "Nuhuh", (FannySceneMetrics scene) => Main.rand.NextBool(2160000) && Main.LocalPlayer.HeldItem.DamageType == DamageClass.Magic, cooldown: 300, onlyPlayOnce: false));

            fannyMessages.Add(new FannyMessage("Thrust", "Did you know you can parry enemy attacks with your sword? Just right click the moment something is about to hit you, and you'll block it with ease!",
               "Idle", (FannySceneMetrics scene) => Main.rand.NextBool(2160000), cooldown: 300, onlyPlayOnce: false));


            #endregion

            #region Item
            fannyMessages.Add(new FannyMessage("Forge", "Na Na Na! The big robotic forge needs a lot of blue meat from the ads! It cannot work without it!",
                "Nuhuh", HasDraedonForgeMaterialsButNoMeat, onlyPlayOnce: false, cooldown: 120).AddItemDisplay(ModContent.ItemType<DeliciousMeat>()));

            fannyMessages.Add(new FannyMessage("DeliciousMeat", "Oooh! Delicious Meat! Collect as much as you can, it will save you a lot of time.", "Awooga",
                (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<DeliciousMeat>())).AddItemDisplay(ModContent.ItemType<DeliciousMeat>()));

            //Add a condition to this one YUH, to pass the test of knowledge...
            //YUH YUH YUH YUH YUH
            //IBAN IBAN IBAN IBAN IBAN
            fannyMessages.Add(new FannyMessage("DesertScourge", "I see you've gotten some mandibles. For some reason, people always try to make medallions out of them when the only way to get them is by killing Cnidrions after the destruction of the legendary Wulfrum Excavator. Strangely specific isn't it? Guess that's just how the cookie crumbles!", "Nuhuh", HasDesertMedallionMaterials).AddItemDisplay(ModContent.ItemType<DesertMedallion>()));
            
            fannyMessages.Add(new FannyMessage("VoodooDoll", "Cool doll you have! i think that it will be even cooler when in lava!",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ItemID.GuideVoodooDoll)));
            
            fannyMessages.Add(new FannyMessage("PortalGun", "Cave Johnson here. We're fresh out of combustible lemons, but let me tell you a little bit about this thing here. These portals are only designed to stick on planetoid rock and not much else. Hope you've got a test chamber lying around that's full of that stuff!",
    "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ItemID.PortalGun)));

            fannyMessages.Add(new FannyMessage("TwentyTwo", "I love 22. My banner now.",
    "Idle", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ItemID.HornetBanner)).AddItemDisplay(ItemID.HornetBanner).AddStartEvent(() => Main.LocalPlayer.ConsumeItem(ItemID.HornetBanner)).SetHoverTextOverride("Thanks Fanny! That was cluttering my inventory!"));

            fannyMessages.Add(new FannyMessage("Shadowspec", "Please throw this thing out, it will delete your world if you have it in inventory for too long!",
                "Sob", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<ShadowspecBar>())).AddItemDisplay(ModContent.ItemType<ShadowspecBar>()).SetHoverTextOverride("Thank you for the help Fanny! I will!"));

            fannyMessages.Add(new FannyMessage("Jump", "Did you know? You can press the \"space\" button to jump!",
                "Nuhuh", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ItemID.PlatinumCoin)).SetHoverTextOverride("Thanks Fanny! You're so helpful!"));
            
            fannyMessages.Add(new FannyMessage("TitanHeart", "You got a heart from a titan! Place it on the tower for a wacky light show!",
                "Idle", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<TitanHeart>())).AddItemDisplay(ModContent.ItemType<TitanHeart>()));

            fannyMessages.Add(new FannyMessage("BloodyVein", "The Bloody Vein is an item of utmost importance which can be inserted into various altars and machinery for wacky results. How about inserting one into one of those lab hologram box things?",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<BloodyVein>())).AddItemDisplay(ModContent.ItemType<BloodyVein>()));

            fannyMessages.Add(new FannyMessage("RottenEye", "The Rotting Eyeball is an item of zero importance. The Bloody Vein from the Crimson's Perforators is way better!",
                "Nuhuh", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<RottingEyeball>()) && !WorldGen.crimson).AddItemDisplay(ModContent.ItemType<RottingEyeball>()).SetHoverTextOverride("Thanks Fanny! I'll be sure to make a Crimson world next time."));

            fannyMessages.Add(new FannyMessage("AlloyBar", "Congratulations, you have obtained the final bar for this stage of your adventure. You should attempt making some Alloy Bars, a versatile material made of every available bar which can be used for powerful items.",
                "Idle", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<AlloyBar>())).AddItemDisplay(ModContent.ItemType<AlloyBar>()));

            fannyMessages.Add(new FannyMessage("Murasama", "Erm, holy crap? " + Main.LocalPlayer.name + "? Is that a reference to my FAVORITE game of all time, metal gear rising revengeance? Did you know that calamity adds a custom boss health boss bar and many othe-",
               "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<Murasama>())));

            fannyMessages.Add(new FannyMessage("Sponge", "Oh, is that a Sponge? Maybe avoid using it. I've heard something about the wielder dying, or something...",
               "Nuhuh", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<TheSponge>())).AddItemDisplay(ModContent.ItemType<TheSponge>()));

            fannyMessages.Add(new FannyMessage("Garbo", "Wowie! That scrap there is useless!",
               "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ItemID.OldShoe) || Main.LocalPlayer.HasItem(ItemID.FishingSeaweed) || Main.LocalPlayer.HasItem(ItemID.TinCan) || Main.LocalPlayer.HasItem(ItemID.JojaCola)).AddItemDisplay(ItemID.TrashCan).SetHoverTextOverride("Thanks Fanny! I already wanted to cook it."));

            /*fannyMessages.Add(new FannyMessage("Catharsis", "Don’t exhume Kaleidoscope! Catharsis is known to cause clinical depression in users.",
               "Nuhuh", (FannySceneMetrics scene) => ModLoader.HasMod("CatalystMod") && Main.LocalPlayer.HasItem(ItemID.RainbowWhip) && Main.LocalPlayer.talk == ModContent.NPCType<WITCH>()));*/
            #endregion

            #region Biome
            fannyMessages.Add(new FannyMessage("DungeonGuardian", "It appears you're approaching the Dungeon. Normally this place is guarded by viscious guardians, but I've disabled them for you my dear friend.", "Nuhuh",
                NearDungeonEntrance));

            fannyMessages.Add(new FannyMessage("ShimmerNothing", "You should consider throwing that item you're holding in Shimmer! You may get something powerful!",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.ZoneShimmer && !Main.LocalPlayer.HeldItem.CanShimmer(), onlyPlayOnce: false, cooldown: 600));

            fannyMessages.Add(new FannyMessage("DeepAbyss", "Tired of this pesky abyss drowning you? I have an idea! If you go into the underworld and poke a hole at the bottom, all the water will drain out! No more pesky pressure!",
                "Nuhuh", (FannySceneMetrics scene) => Main.LocalPlayer.Calamity().ZoneAbyssLayer3));

            fannyMessages.Add(new FannyMessage("RodAbyss", "It sure takes a while to get to the bottom of the Abyss... Maybe try using that teleporting thingamabob you have?",
                "Idle", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ItemID.RodofDiscord) || Main.LocalPlayer.HasItem(ModContent.ItemType<NormalityRelocator>())).AddItemDisplay(ItemID.RodofDiscord));
            
            fannyMessages.Add(new FannyMessage("Temple", "Aw man, there's so many booby traps in here! Try using that fancy gadget of yours to disable them!",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ItemID.WireCutter) || Main.LocalPlayer.HasItem(ItemID.MulticolorWrench) || Main.LocalPlayer.HasItem(3611)).AddItemDisplay(ItemID.WireCutter));
            
            fannyMessages.Add(new FannyMessage("Altars", "Smashing demon altars is no longer guaranteed to bless your world with ores. But it’s still worth a shot!",
               "Idle", (FannySceneMetrics scene) => (Main.LocalPlayer.ZoneCorrupt || Main.LocalPlayer.ZoneCrimson) && Main.hardMode && CalamityConfig.Instance.EarlyHardmodeProgressionRework && !Main.LocalPlayer.ZoneUnderworldHeight));

            fannyMessages.Add(new FannyMessage("StupidSword", "If you kill enough Meteor Heads, you might be able to get the Divine Intervention!",
                "Nuhuh", (FannySceneMetrics scene) => Main.LocalPlayer.ZoneMeteor && ModLoader.HasMod("CatalystMod")));
            
            fannyMessages.Add(new FannyMessage("DrowningAbyss", "Your air bubbles are disappearing at an alarming rate, you should set up an air pocket, and fast!",
                "Idle", (FannySceneMetrics scene) => Main.LocalPlayer.breath <= 0 && Main.LocalPlayer.Calamity().ZoneAbyss));
            
            fannyMessages.Add(new FannyMessage("Jungleabyss", "I’ve heard word that there’s incredible treasures in the mysterious depths of the ocean, the one past the jungle!",
   "Nuhuh", (FannySceneMetrics scene) => !NPC.downedBoss3 && Main.LocalPlayer.ZoneJungle && Main.rand.NextBool(600)));
            #endregion

            #region Shrine
            fannyMessages.Add(new FannyMessage("ShrineSnow", "Woah, is that a snow shrine? You better go loot it for its one-of-a-kind treasure! It gave you a really cool item that you'll use forever I think?",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.ZoneSnow && Main.LocalPlayer.ZoneRockLayerHeight && Main.rand.NextBool(116000)).AddItemDisplay(ModContent.ItemType<TundraLeash>()));

            fannyMessages.Add(new FannyMessage("ShrineDesert", "Woah, is that a desert shrine? You better go loot it for its one-of-a-kind treasure! It gave you a tile-matching game called Luxor I think?",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.ZoneUndergroundDesert && Main.rand.NextBool(116000)).AddItemDisplay(ModContent.ItemType<LuxorsGift>()));

            fannyMessages.Add(new FannyMessage("ShrineCorruption", "Woah, is that a corruption shrine? You better go loot it for its one-of-a-kind treasure! It caused pebbles to rain from the sky I think?",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.ZoneCorrupt && Main.rand.NextBool(116000)).AddItemDisplay(ModContent.ItemType<CorruptionEffigy>()));

            fannyMessages.Add(new FannyMessage("ShrineCrimson", "Woah, is that a crimson shrine? You better go loot it for its one-of-a-kind treasure! It caused pebbles to rain from the sky I think?",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.ZoneCrimson && Main.rand.NextBool(116000)).AddItemDisplay(ModContent.ItemType<CrimsonEffigy>()));

            fannyMessages.Add(new FannyMessage("ShrineUg", "Woah, is that an underground shrine? You better go loot it for its one-of-a-kind treasure! It caused you to gain defense while standing still I think?",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.ZoneNormalUnderground && Main.rand.NextBool(116000)).AddItemDisplay(ModContent.ItemType<TrinketofChi>()));

            fannyMessages.Add(new FannyMessage("ShrineHallow", "Woah, is that a hallow shrine? You better go loot it for its one-of-a-kind treasure! No seriously, it's the only thing exclusive to the Hallow!",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.ZoneHallow && Main.LocalPlayer.ZoneRockLayerHeight && Main.rand.NextBool(116000)).AddItemDisplay(ModContent.ItemType<HallowEffigy>()));

            fannyMessages.Add(new FannyMessage("ShrineAstral", "Woah, is that an astral shrine? You better go loot it for its one-of-a-kind treasure! It summoned a large mimic I think?",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.Calamity().ZoneAstral && Main.LocalPlayer.ZoneRockLayerHeight && Main.rand.NextBool(116000)).AddItemDisplay(ModContent.ItemType<AstralEffigy>()));

            fannyMessages.Add(new FannyMessage("ShrineGranite", "Woah, is that a granite shrine? You better go loot it for its one-of-a-kind treasure! It caused sparks to fly out of enemies when hit I think?",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.ZoneGranite && Main.rand.NextBool(116000)).AddItemDisplay(ModContent.ItemType<UnstableGraniteCore>()));

            fannyMessages.Add(new FannyMessage("ShrineMarble", "Woah, is that a marble shrine? You better go loot it for its one-of-a-kind treasure! It summoned cool orbital swords I think?",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.ZoneMarble && Main.rand.NextBool(116000)).AddItemDisplay(ModContent.ItemType<GladiatorsLocket>()));

            fannyMessages.Add(new FannyMessage("ShrineMushroom", "Woah, is that a mushroom shrine? You better go loot it for its one-of-a-kind treasure! It imbued true melee weapons with fungi I think?",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.ZoneGlowshroom && Main.rand.NextBool(116000)).AddItemDisplay(ModContent.ItemType<FungalSymbiote>()));


            #endregion

            #region NPC
            fannyMessages.Add(new FannyMessage("WulfrumPylone",
                "Woah, you hear that? No? Well it sounded like something big... we should get it's attention. A dose of some of that tower over there's energy in a special chest might be just the motivation it needs to come to the surface!",
                "Idle",
                (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<WulfrumAmplifier>() && !CalRemixWorld.downedExcavator), onlyPlayOnce: false, cooldown: 2400).AddItemDisplay(ItemID.LivingWoodChest));

            fannyMessages.Add(new FannyMessage("Cysts",
                "That pimple thing looks useless, but it drops a very useful material. Please kill it!",
                "Awooga",
                (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<HiveTumor>() || n.type == ModContent.NPCType<PerforatorCyst>())));
            
            fannyMessages.Add(new FannyMessage("Deimos", "That \"Deimos\" over there. She has some delicious Mars Bars, you should go buy some!",
    "Idle", (FannySceneMetrics scene) => CrossModBoss(scene, "EverquartzAdventure", "StarbornPrincess")).SetHoverTextOverride("Thanks Fanny! I'll buy you plenty of Mars Bars!"));

            fannyMessages.Add(new FannyMessage("Fairy", "That thing is hurting my eyes! Kill it, quick!",
                "Sob", (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == NPCID.EmpressButterfly)));

            fannyMessages.Add(new FannyMessage("Bloodworm", "Crush it under your boot.",
                "Idle", (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<BloodwormNormal>() || n.type == ModContent.NPCType<BloodwormFleeing>())));

            fannyMessages.Add(new FannyMessage("Wolf", "Aw look a cute wolf! You can extract valuable Coyote Venom from their lifeless corpses in order to make some neat ice items.",
    "Idle", (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == NPCID.Wolf)).AddItemDisplay(ModContent.ItemType<CoyoteVenom>()));


            #endregion

            #region Event
            fannyMessages.Add(new FannyMessage("InvasionDeath", "These guys are really giving us what for. It might be a good idea to step away for a bit in order to come up with a new strategy...",
                "Sob", (FannySceneMetrics scene) => Main.LocalPlayer.dead && Main.invasionType != InvasionID.None, cooldown: 1200));
            
            fannyMessages.Add(new FannyMessage("BloodMoon", "During a blood moon, strange critters can be found hiding under rocks. They can be used for blood moon fishing, but be careful, those teeth can really hurt.",
   "Idle", (FannySceneMetrics scene) => Main.bloodMoon));
            #endregion

            #region Boss
            fannyMessages.Add(new FannyMessage("KingSlime", "It looks like you're fighting.. what's his name? The giant blue thing.. King something. Slime! King Slime. I heard he's pretty tough! So in order to defeat him, you need to shoot flares at him! Or was that an animation? Alright, you gotta get a falling star to hit him! But that's rare. And at night. There's also other creatures that can spawn at night. Like the Eye of Cthulhu.  Wait, Cthulhu? Although you fight his organs, you never actually fight Cthulhu. Says here that the Dryads tore him apart or something. Pretty brutal, although this is Terraria lore. Does Terraria lore take place in Calamity? Likely not, as Draedon constructed the mechanical bosses. Mechanical bosses? Those are tough. You could use some help with those. So, I heard that some of them shoot lasers. Nevermind, turns out they all shoot lasers. Just dodge the lasers I guess. Lasers? Like.. the Moon Lord? Moon Lord is the stronger version of Cthulhu. He lives on the moon. In vanilla Terraria, the Moon Lord is the final boss. Once you defeat him, I guess you just think.. who was the first boss you fought? Desert Scourge? But that's from the Calamity Mod. And what if you fought another boss before Desert Scourge, like King Slime? Oh, right! You're fighting King Slime. Dodge him. That's all I have to say.",
              "Nuhuh", (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == NPCID.KingSlime), maxWidth: 470));

            fannyMessages.Add(new FannyMessage("Mutant", "Woah, how much HP does that guy have??",
    "Awooga", (FannySceneMetrics scene) => CrossModBoss(scene, "FargowiltasSouls", "MutantBoss")));

            fannyMessages.Add(new FannyMessage("Goozma", "Holy cow! It's THE Goozma! An easy way to defeat this slippery menace is to lead him into shimmer.",
                "Awooga", (FannySceneMetrics scene) => CrossModBoss(scene, "CalamityHunt", "Goozma")));

            fannyMessages.Add(new FannyMessage("Astrageldon", "Woah, this boss seems a little strong for you! Maybe come back after you’ve killed the Moon Lord!",
                "Nuhuh", (FannySceneMetrics scene) => CrossModBoss(scene, "CatalystMod", "Astrageldon")).SetHoverTextOverride("Thanks you Fanny! I'll go kill the Moon Lord first."));

            fannyMessages.Add(new FannyMessage("Calclone", "It is time. The Brimstone Witch, the one behind the Calamity in this world. You will now face Supreme Witch, Calamitas and end everything once and for all!",
               "Awooga", (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<CalamitasClone>())));

            fannyMessages.Add(new FannyMessage("Deus", "It appears that you are once again fighting a large serpentine creature. Therefore, it's advisable to do what you've done with them before and blast it with fast single target weaponry!",
               "Idle", (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<AstrumDeusHead>())));

            fannyMessages.Add(new FannyMessage("DeusSplitMod", "This is getting out of hand! Now there are two of them!",
               "Awooga", (FannySceneMetrics scene) => !Main.zenithWorld && NPC.CountNPCS(ModContent.NPCType<AstrumDeusHead>()) > 1));

            fannyMessages.Add(new FannyMessage("DeusSplitModGFB", "This is getting out of hand! Now there are ten of them!",
               "Awooga", (FannySceneMetrics scene) => Main.zenithWorld && NPC.CountNPCS(ModContent.NPCType<AstrumDeusHead>()) > 9));

            fannyMessages.Add(new FannyMessage("YharvelQuip", "Is it just me, or is it getting hot in here?",
                "Awooga", YharonPhase2));

            fannyMessages.Add(new FannyMessage("DraedonEnter", "Gee willikers! It's the real Draedon! He will soon present you with a difficult choice between three of your previous foes but with new attacks and increased difficulty. This appears to be somewhat of a common theme with this world dontcha think?",
               "Awooga", (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<Draedon>())));

            fannyMessages.Add(new FannyMessage("ExoMayhem", "Wow! What a mayhem! Don't panic though, if you focus on dodging, you will be less likely to get hit. A common strategy for these tin cans is to \" fall god \", which I believe means summoning other gods like the Slime God and killing them for extra health. You should also pay extra attention to Ares' red cannon, because sometimes it can sweep across the screen, ruining your dodge flow. As for the twins, keep a close eye on the right one, as it has increased fire rate. There is no saving you from Thanatos, it isn't synced and breaks the structure these guys are allegedly supposed to have. Like seriously, why do the twins and Ares hover to the sides and above you while that robo-snake just does whatever the heckle heckity heckering hecky heck he wants? It would be significantly more logical if it tried to like stay below you, but no. Anyways, good luck buddy! You're almost at the end, you can do this!",
                "Idle", (FannySceneMetrics scene) => CalamityGlobalNPC.draedonExoMechPrime != -1 && CalamityGlobalNPC.draedonExoMechTwinGreen != -1 && CalamityGlobalNPC.draedonExoMechWorm != -1, needsToBeClickedOff: false, duration: 22));

            #endregion

            #region BossDeath
            //fannyMessages.Add(new FannyMessage("Hardmode", "Now why'dya have to do that, champ? You've done did it now! If I were you, I'd get started on completely purifying " + Main.worldName + " before doing anything else!",
              //  "Sob", (FannySceneMetrics scene) => Main.hardMode));

            fannyMessages.Add(new FannyMessage("AbyssBegin", "Every 60 seconds in the Abyss a hour passes by, truly wonderful!",
               "Nuhuh", (FannySceneMetrics scene) => NPC.downedBoss3).SetHoverTextOverride("Very interesting Fanny!"));

            fannyMessages.Add(new FannyMessage("DraedonExit", "Good golly! You did it! Though I'd HATE to imagine the financial losses caused by the destruction of those machines.",
                "Awooga", (FannySceneMetrics scene) => DownedBossSystem.downedExoMechs));

            fannyMessages.Add(new FannyMessage("SCalDie", "That was exhilarating! Though that means the end of our adventure is upon us. What a Calamity as one may say!",
                "Awooga", (FannySceneMetrics scene) => DownedBossSystem.downedCalamitas));

            fannyMessages.Add(new FannyMessage("CalcloneDeath", "Oh it was just a clone.",
                "Sob", (FannySceneMetrics scene) => DownedBossSystem.downedCalamitasClone));

            fannyMessages.Add(new FannyMessage("Yharore", "Looks like the caverns have been laced with Auric Ore! The ore veins are pretty massive, so I’d say it’s best that you get up close and go hog wild!",
               "Idle", (FannySceneMetrics scene) => DownedBossSystem.downedYharon).AddItemDisplay(ModContent.ItemType<AuricOre>()));


            #endregion

            #region Misc
            fannyMessages.Add(new FannyMessage("LowHP", "It looks like you're low on health. If your health reaches 0, you'll die. To combat this, don't let your health reach 0!",
                "Nuhuh", (FannySceneMetrics scene) => Main.LocalPlayer.statLife < Main.LocalPlayer.statLifeMax2 * 0.25f, cooldown: 300, onlyPlayOnce: false).SetHoverTextOverride("Thanks Fanny! I'll heal."));

            #endregion      

            IntroducingEvilFanny();
            MoonLordTextDump();
        }

        #endregion

        private static void IntroducingEvilFanny()
        {

            FannyMessage introLore = new FannyMessage("IntroducingEvilFanny", "My friend, we've made it to Hardmode! Plenty of new opportunities have popped up and plenty of dangerous new foes now lurk about.",
                "Idle", (FannySceneMetrics scene) => Main.LocalPlayer.velocity == Vector2.Zero && Main.hardMode, 7, needsToBeClickedOff: false, onlyPlayOnce: true, displayOutsideInventory: true, persistsThroughSaves: false).AddDelay(0.4f);

            fannyMessages.Add(introLore);

            FannyMessage introEvilLore = new FannyMessage("IntroducingEvilFanny2", "'Sup",
                "EvilIdle", FannyMessage.AlwaysShow, 6, needsToBeClickedOff: false, onlyPlayOnce: true, displayOutsideInventory: true, persistsThroughSaves: false)
                .NeedsActivation(4f).SpokenByEvilFanny();

            introLore.AddStartEvent(() => introEvilLore.ActivateMessage());

            fannyMessages.Add(introEvilLore);

            FannyMessage introLore2 = new FannyMessage("IntroducingEvilFanny3", "E-evil Fanny!? I thought you moved away to the Yukon!",
                "Sob", FannyMessage.AlwaysShow, 8, needsToBeClickedOff: false, onlyPlayOnce: true, displayOutsideInventory: true, persistsThroughSaves: false)
                .NeedsActivation();
            introLore.AddEndEvent(() => introLore2.ActivateMessage());

            fannyMessages.Add(introLore2);

            FannyMessage introEvilLore2 = new FannyMessage("IntroducingEvilFanny4", "Yeah. Got cold.",
               "EvilIdle", FannyMessage.AlwaysShow, 5, needsToBeClickedOff: false, onlyPlayOnce: true, displayOutsideInventory: true, persistsThroughSaves: false)
               .NeedsActivation().SpokenByEvilFanny();

            introEvilLore.AddEndEvent(() => introEvilLore2.ActivateMessage());

            fannyMessages.Add(introEvilLore2);

            FannyMessage introLore3 = new FannyMessage("IntroducingEvilFanny5", Main.LocalPlayer.name + ", it seems my evil counterpart, Evil Fanny, has returned! Don't trust a thing they say, and hopefully they'll leave..",
               "Idle", FannyMessage.AlwaysShow, 8, onlyPlayOnce: true, displayOutsideInventory: true, persistsThroughSaves: false)
               .NeedsActivation();

            introLore2.AddEndEvent(() => introLore3.ActivateMessage());

            fannyMessages.Add(introLore3);
        }
        private static void MoonLordTextDump()
        {

            FannyMessage ml = new FannyMessage("ML1", "Blegh, I think I swallowed one of that thing's bones. Well, it's time for Godseeker Mode. You will face a sequence of challenges, each more difficult than the last with little to no breathing between encounters.",
                "Idle", (FannySceneMetrics scene) => NPC.downedMoonlord, 7, needsToBeClickedOff: true, onlyPlayOnce: true, displayOutsideInventory: true, persistsThroughSaves: false).AddDelay(0.4f);

            fannyMessages.Add(ml);

            FannyMessage ml2 = new FannyMessage("ML2", "Almost sounds like a boss rush or something.",
                "EvilIdle", FannyMessage.AlwaysShow, 6, needsToBeClickedOff: false, onlyPlayOnce: true, displayOutsideInventory: true, persistsThroughSaves: false)
                .NeedsActivation(4f).SpokenByEvilFanny();

            ml.AddStartEvent(() => ml2.ActivateMessage());

            fannyMessages.Add(ml2);

            FannyMessage ml3 = new FannyMessage("ML3", "A priority you should take care of immediately is harvesting Unholy Essence from some new, fearsome creatures that have appeared in the Underworld and Hallow. You can then use the essence to make the Rune of Kos and summon the Sentinels of the Devourer.",
                "Idle", FannyMessage.AlwaysShow, 6, needsToBeClickedOff: true, onlyPlayOnce: true, displayOutsideInventory: true, persistsThroughSaves: false).AddItemDisplay(ModContent.ItemType<RuneofKos>()).NeedsActivation();

            ml.AddEndEvent(() => ml3.ActivateMessage());
            fannyMessages.Add(ml3);

            FannyMessage ml4 = new FannyMessage("ML4", "You can find 3 different types of cosmic remains if you search the sky, one of them is the remains of the Moon guy you just defeated! Second one is some exotic clusters used for some artifacts! Third one is the distorted remains of Cosmos itself.",
                "Idle", FannyMessage.AlwaysShow, 6, needsToBeClickedOff: true, onlyPlayOnce: true, displayOutsideInventory: true, persistsThroughSaves: false).NeedsActivation();
            ml3.AddEndEvent(() => ml4.ActivateMessage());
            fannyMessages.Add(ml4);

            FannyMessage ml5 = new FannyMessage("ML5", "The Dungeon has also gotten an upgrade in power, with new spirit enemies that occasionally pop out of enemies when defeated which drop Phantoplasm, an important crafting material. I'd reccomend killing as many of those things as possible!",
                "Idle", FannyMessage.AlwaysShow, 6, needsToBeClickedOff: true, onlyPlayOnce: true, displayOutsideInventory: true, persistsThroughSaves: false).NeedsActivation().AddItemDisplay(ModContent.ItemType<Polterplasm>());
            ml4.AddEndEvent(() => ml5.ActivateMessage());
            fannyMessages.Add(ml5);

            FannyMessage ml6 = new FannyMessage("ML6", "I'm getting a bit of deja vu here.",
                "EvilIdle", FannyMessage.AlwaysShow, 6, needsToBeClickedOff: false, onlyPlayOnce: true, displayOutsideInventory: true, persistsThroughSaves: false)
                .NeedsActivation(4f).SpokenByEvilFanny();

            ml5.AddStartEvent(() => ml6.ActivateMessage());

            fannyMessages.Add(ml6);


            FannyMessage ml7 = new FannyMessage("ML7", "It appears that the red moon will start yielding bountiful harvests of Blood Orbs now! You should take advantage of this opportuntiy to craft lotsa potions! I'm personally a fan of the Inferno Potion myself.",
                "Nuhuh", FannyMessage.AlwaysShow, 6, needsToBeClickedOff: true, onlyPlayOnce: true, displayOutsideInventory: true, persistsThroughSaves: false).NeedsActivation().AddItemDisplay(ItemID.InfernoPotion);
            ml5.AddEndEvent(() => ml7.ActivateMessage());
            fannyMessages.Add(ml7);


            FannyMessage ml8 = new FannyMessage("ML8", "Are you feeling a little weak? It may be because of the dreaded \'Curse of the Eldritch\', a terrifying affliction inflicted upon those who slay eldritch beasts which permanently reduces your life regeneration!",
                "Sob", FannyMessage.AlwaysShow, 6, needsToBeClickedOff: true, onlyPlayOnce: true, displayOutsideInventory: true, persistsThroughSaves: false).NeedsActivation();
            ml7.AddEndEvent(() => ml8.ActivateMessage());
            fannyMessages.Add(ml8);


            FannyMessage ml9 = new FannyMessage("ML9", "I should also mention that if you have a certain thief in one of your towns, they'll start selling the flawless Celestial Reaper, which can be used to cut down herbs significantly faster than the normal Sickle.",
                "Idle", FannyMessage.AlwaysShow, 6, needsToBeClickedOff: true, onlyPlayOnce: true, displayOutsideInventory: true, persistsThroughSaves: false).NeedsActivation().AddItemDisplay(ModContent.ItemType<CelestialReaper>());
            ml8.AddEndEvent(() => ml9.ActivateMessage());
            fannyMessages.Add(ml9);

            FannyMessage ml10 = new FannyMessage("ML10", "Oh oh I should also mention-",
                "Idle", FannyMessage.AlwaysShow, 6, needsToBeClickedOff: false, onlyPlayOnce: true, displayOutsideInventory: true, persistsThroughSaves: false).NeedsActivation();
            ml9.AddEndEvent(() => ml10.ActivateMessage());
            fannyMessages.Add(ml10);

            FannyMessage ml11 = new FannyMessage("ML11", "Oh my god shut up already, how much can one boss unlock!?",
                "EvilIdle", FannyMessage.AlwaysShow, 6, needsToBeClickedOff: false, onlyPlayOnce: true, displayOutsideInventory: true, persistsThroughSaves: false)
                .NeedsActivation(2f).SpokenByEvilFanny();

            ml10.AddStartEvent(() => ml11.ActivateMessage());

            fannyMessages.Add(ml11);

            FannyMessage ml12 = new FannyMessage("ML12", "It appears this encounter is going to have to be cut short buddy, I need to go do something.",
                "Idle", FannyMessage.AlwaysShow, 6, needsToBeClickedOff: false, onlyPlayOnce: true, displayOutsideInventory: true, persistsThroughSaves: false).NeedsActivation();
            ml11.AddStartEvent(() => ml12.ActivateMessage());
            fannyMessages.Add(ml12);

            ml12.AddEndEvent(() => Violence());

        }

        private static void Violence()
        {
            Main.LocalPlayer.Calamity().GeneralScreenShakePower = 800;
            SoundEngine.PlaySound(CalamityMod.NPCs.ExoMechs.Ares.AresGaussNuke.NukeExplosionSound, Main.LocalPlayer.Center);
            SoundEngine.PlaySound(SoundID.DD2_GoblinScream);
            SoundEngine.PlaySound(SoundID.DD2_KoboldExplosion);
        }

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

        public override void SaveWorldData(TagCompound tag)
        {
            //Save all the ones already seen
            for (int i = 0; i < fannyMessages.Count; i++)
            {
                FannyMessage msg = fannyMessages[i];
                if (msg.alreadySeen && msg.PersistsThroughSaves)
                    tag["FannyDialogue" + msg.Identifier] = true;
            }

            tag["FannyReadThroughDogDialogue"] = ReadAllDogTips;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            //Debug konami code to prevent loading
            if (Main.mouseRight && Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift) && Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space))
            {
                SoundEngine.PlaySound(SoundID.Cockatiel);
                SoundEngine.PlaySound(SoundID.DD2_GoblinScream);
                SoundEngine.PlaySound(SoundID.DD2_KoboldExplosion);
                return;
            }

            for (int i = 0; i < fannyMessages.Count; i++)
            {
                FannyMessage msg = fannyMessages[i];
                if (tag.ContainsKey("FannyDialogue" + msg.Identifier))
                    msg.alreadySeen = true;
            }

            if (tag.TryGet<bool>("FannyReadThroughDogDialogue", out bool readDog))
                ReadAllDogTips = readDog;
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
            SoundEngine.PlaySound(speakingFanny.speakingSound);
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