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
using CalRemix.Items;
using CalamityMod.NPCs.Polterghast;
using CalamityMod.Items.PermanentBoosters;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Magic;
using CalRemix.Items.Weapons;
using CalamityMod.NPCs.DesertScourge;
using CalamityMod.NPCs.Crabulon;
using CalamityMod.NPCs.SlimeGod;
using CalamityMod.NPCs.Cryogen;
using CalamityMod.NPCs.AquaticScourge;
using CalamityMod.NPCs.Leviathan;
using CalamityMod.NPCs.PlaguebringerGoliath;
using CalamityMod.NPCs.Ravager;
using CalamityMod.NPCs.ProfanedGuardians;
using CalamityMod.NPCs.Bumblebirb;
using CalamityMod.NPCs.Providence;
using CalamityMod.NPCs.CeaselessVoid;
using CalamityMod.NPCs.Signus;
using CalamityMod.NPCs.StormWeaver;
using CalamityMod.NPCs.OldDuke;
using CalamityMod.NPCs.Other;
using CalamityMod.NPCs.AstrumAureus;
using CalamityMod.NPCs.BrimstoneElemental;
using CalamityMod.NPCs.ExoMechs.Apollo;

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
                "Idle", (FannySceneMetrics scene) => !Main.zenithWorld, displayOutsideInventory: true));

            fannyMessages.Add(new FannyMessage("GfbintroEvil", "WELCOME TO HELL!",
    "EvilIdle", (FannySceneMetrics scene) => Main.zenithWorld).SpokenByEvilFanny());

            fannyMessages.Add(new FannyMessage("Gfbintro", "This is different that its supposed to be... Oh! You made a getfixedboi world. This world presents new, unfamiliar challenges so always be on your toes.",
                "Idle", (FannySceneMetrics scene) => Main.zenithWorld));

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
               "Idle", (FannySceneMetrics scene) => Main.rand.NextBool(2160000) && Main.LocalPlayer.HeldItem.DamageType == ModContent.GetInstance<TrueMeleeDamageClass>(), cooldown: 300, onlyPlayOnce: false));


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

            fannyMessages.Add(new FannyMessage("Murasama", "Erm, holy crap? $0? Is that a reference to my FAVORITE game of all time, metal gear rising revengeance? Did you know that calamity adds a custom boss health boss bar and many othe-",
               "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<Murasama>())).AddDynamicText(FannyMessage.GetPlayerName));

            fannyMessages.Add(new FannyMessage("Sponge", "Oh, is that a Sponge? Maybe avoid using it. I've heard something about the wielder dying, or something...",
               "Nuhuh", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<TheSponge>())).AddItemDisplay(ModContent.ItemType<TheSponge>()));

            fannyMessages.Add(new FannyMessage("Garbo", "Wowie! That scrap there is useless!",
               "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ItemID.OldShoe) || Main.LocalPlayer.HasItem(ItemID.FishingSeaweed) || Main.LocalPlayer.HasItem(ItemID.TinCan) || Main.LocalPlayer.HasItem(ItemID.JojaCola)).AddItemDisplay(ItemID.TrashCan).SetHoverTextOverride("Thanks Fanny! I already wanted to cook it."));

            fannyMessages.Add(new FannyMessage("Nightfuel", "Nightmare Fuel, huh? ...you know, maybe if you can harvest enough of it, maybe those Pumpkings will stop terorrizing our inhabitants and they'll be permanently more happy!",
   "Idle", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<NightmareFuel>())).AddItemDisplay(ModContent.ItemType<NightmareFuel>()));

            fannyMessages.Add(new FannyMessage("Endenergy", "Ooh, is that Endothermic Energy? If we can get a decent supply of it, I think those Ice Queens will fear us and our residents might be forever grateful with us!",
   "Idle", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<EndothermicEnergy>())).AddItemDisplay(ModContent.ItemType<EndothermicEnergy>()));

            fannyMessages.Add(new FannyMessage("Darksunfrag", "What's that? Darksun Fragment? Do you think with enough of it, our world will be permanently lit up like a lemon-scented candle flame?",
   "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<DarksunFragment>())).AddItemDisplay(ModContent.ItemType<DarksunFragment>()));

            fannyMessages.Add(new FannyMessage("Onion", "I'd be weary about eating that strange plant. You can only get one, so it might be useful to hang on to it for later.",
   "Nuhuh", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<CelestialOnion>())).AddItemDisplay(ModContent.ItemType<CelestialOnion>()));

            fannyMessages.Add(new FannyMessage("Ultrakill", "Oh EM GEE! A gun from the hit first-person shooter game, \'MURDERDEATH\'!? Try throwing out some coins and hitting them with a Titanium Railgun to pull a sick railcoin maneuver!",
   "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<MidasPrime>()) || Main.LocalPlayer.HasItem(ModContent.ItemType<CrackshotColt>())).AddItemDisplay(ModContent.ItemType<MidasPrime>()));

            fannyMessages.Add(new FannyMessage("Tofu", "Uh oh! Looks like one of your items is a reference to a smelly old game franchise known as Touhou! Do your ol\' pal Fanny a good deed and put it away.",
   "Nuhuh", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<ScarletDevil>()) || Main.LocalPlayer.HasItem(ModContent.ItemType<GlacialEmbrace>()) || Main.LocalPlayer.HasItem(ModContent.ItemType<RecitationoftheBeast>()) || Main.LocalPlayer.HasItem(ModContent.ItemType<EventHorizon>()) || Main.LocalPlayer.HasItem(ModContent.ItemType<HermitsBoxofOneHundredMedicines>()) || Main.LocalPlayer.HasItem(ModContent.ItemType<PristineFury>()) || Main.LocalPlayer.HasItem(ModContent.ItemType<DarkSpark>()) || Main.LocalPlayer.HasItem(ModContent.ItemType<ResurrectionButterfly>()) || Main.LocalPlayer.HasItem(ModContent.ItemType<FantasyTalisman>()) || Main.LocalPlayer.HasItem(ModContent.ItemType<HellsSun>()) || Main.LocalPlayer.HasItem(ModContent.ItemType<TheDreamingGhost>())).SetHoverTextOverride("Anything for you Fanny!"));

            /*fannyMessages.Add(new FannyMessage("Catharsis", "Don’t exhume Kaleidoscope! Catharsis is known to cause clinical depression in users.",
               "Nuhuh", (FannySceneMetrics scene) => ModLoader.HasMod("CatalystMod") && Main.LocalPlayer.HasItem(ItemID.RainbowWhip) && Main.LocalPlayer.talk == ModContent.NPCType<WITCH>()));*/
            #endregion

            #region Biome
            fannyMessages.Add(new FannyMessage("DungeonGuardian", "It appears you're approaching the Dungeon. Normally this place is guarded by viscious guardians, but I've disabled them for you my dear friend.", "Nuhuh",
                NearDungeonEntrance));

            fannyMessages.Add(new FannyMessage("Snowbr", "It's quite chilly here, maybe you should invest some time in gathering some cold-protective gear before you freeze to death!", "Idle",
                (FannySceneMetrics scene) => Main.LocalPlayer.ZoneSnow));

            fannyMessages.Add(new FannyMessage("Cavern", "It's quite dark down here. You should go get some more torches before further exploration or you may fall into a pit full of lice!", "Idle",
                (FannySceneMetrics scene) => Main.LocalPlayer.ZoneRockLayerHeight).AddItemDisplay(ItemID.Torch));

            fannyMessages.Add(new FannyMessage("Granite", "Woah, this place looks so cool and futuristic! It's almost like an entirely different dimension here!",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.ZoneGranite));

            fannyMessages.Add(new FannyMessage("Marble", "Marble? I LOVE playing with marbles! A few hundred years ago I was an avid marble collector, collecting marbles of various shapes, colors, and sizes. But, one day, I lost my marbles.",
                "Idle", (FannySceneMetrics scene) => Main.LocalPlayer.ZoneMarble));

            fannyMessages.Add(new FannyMessage("FungalGrowths", "I know a quick get rich quick scheme. See those Glowing Mushrooms? They sell for a lot! Go destroy that ecosystem for some quick cash!",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.ZoneGlowshroom).AddItemDisplay(ItemID.GlowingMushroom));

            fannyMessages.Add(new FannyMessage("GemCave", "So many gemstones! Make sure to keep some Emeralds handy. Apparently a lot of people like to search for them to make crates for some reason!",
                "Nuhuh", (FannySceneMetrics scene) => Main.LocalPlayer.ZoneGemCave).AddItemDisplay(ItemID.Emerald));

            fannyMessages.Add(new FannyMessage("SunkySea", "Did you know that the oldest animal ever identified was a clam? Unfortunately, the people who caught it accidentally froze it to death. Maybe you can find an older clam here in this Sunken Sea!",
                "Nuhuh", (FannySceneMetrics scene) => Main.LocalPlayer.Calamity().ZoneSunkenSea));

            fannyMessages.Add(new FannyMessage("Hell", "Welcome to hell! This place is flaming hot just like me, so you better get some gear to protect you aganist the heat!", "Nuhuh",
                (FannySceneMetrics scene) => Main.LocalPlayer.ZoneUnderworldHeight));

            fannyMessages.Add(new FannyMessage("ShimmerNothing", "You should consider throwing that item you're holding in Shimmer! You may get something powerful!",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.ZoneShimmer && !Main.LocalPlayer.HeldItem.CanShimmer(), onlyPlayOnce: false, cooldown: 600));

            fannyMessages.Add(new FannyMessage("Meteore", "A Fallen Star!",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.ZoneMeteor).AddItemDisplay(ItemID.FallenStar));

            fannyMessages.Add(new FannyMessage("DeepAbyss", "Tired of this pesky abyss drowning you? I have an idea! If you go into the underworld and poke a hole at the bottom, all the water will drain out! No more pesky pressure!",
                "Nuhuh", (FannySceneMetrics scene) => Main.LocalPlayer.Calamity().ZoneAbyssLayer3));

            fannyMessages.Add(new FannyMessage("RodAbyss", "It sure takes a while to get to the bottom of the Abyss... Maybe try using that teleporting thingamabob you have?",
                "Idle", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ItemID.RodofDiscord) || Main.LocalPlayer.HasItem(ModContent.ItemType<NormalityRelocator>())).AddItemDisplay(ItemID.RodofDiscord));

            fannyMessages.Add(new FannyMessage("Temple", "I love house invasion!",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.ZoneLihzhardTemple).SetHoverTextOverride("Me too Fanny!"));

            fannyMessages.Add(new FannyMessage("TempleWires", "Aw man, there's so many booby traps in here! Try using that fancy gadget of yours to disable them!",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.ZoneLihzhardTemple && (Main.LocalPlayer.HasItem(ItemID.WireCutter) || Main.LocalPlayer.HasItem(ItemID.MulticolorWrench) || Main.LocalPlayer.HasItem(3611))).AddItemDisplay(ItemID.WireCutter));

            fannyMessages.Add(new FannyMessage("Altars", "Smashing demon altars is no longer guaranteed to bless your world with ores. But it’s still worth a shot!",
               "Idle", (FannySceneMetrics scene) => (Main.LocalPlayer.ZoneCorrupt || Main.LocalPlayer.ZoneCrimson) && Main.hardMode && CalamityConfig.Instance.EarlyHardmodeProgressionRework && !Main.LocalPlayer.ZoneUnderworldHeight));

            fannyMessages.Add(new FannyMessage("StupidSword", "If you kill enough Meteor Heads, you might be able to get the Divine Intervention!",
                "Nuhuh", (FannySceneMetrics scene) => Main.LocalPlayer.ZoneMeteor && ModLoader.HasMod("CatalystMod")));

            fannyMessages.Add(new FannyMessage("DrowningAbyss", "Your air bubbles are disappearing at an alarming rate, you should set up an air pocket, and fast!",
                "Idle", (FannySceneMetrics scene) => Main.LocalPlayer.breath <= 0 && Main.LocalPlayer.Calamity().ZoneAbyss));

            fannyMessages.Add(new FannyMessage("Jungleabyss", "I’ve heard word that there’s incredible treasures in the mysterious depths of the ocean, the one past the jungle!",
   "Nuhuh", (FannySceneMetrics scene) => !NPC.downedBoss3 && Main.LocalPlayer.ZoneJungle && Main.rand.NextBool(600)));

            fannyMessages.Add(new FannyMessage("Sulph", "Ah the good ol' Sulphurous Sea. Just take a breathe of the fresh air here! If you see any tiny light green lights, you should use a Bug Net on it to get a fancy light pet.",
  "Idle", (FannySceneMetrics scene) => Main.LocalPlayer.Calamity().ZoneSulphur).AddItemDisplay(ModContent.ItemType<DisgustingSeawater>()));

            fannyMessages.Add(new FannyMessage("Starbuster", "Trying to get a Starbuster Core? Lately those culex things have been hardening up! The only way to force their cores out of them is by running a Unicorn into them!",
  "Nuhuh", (FannySceneMetrics scene) => Main.LocalPlayer.Calamity().ZoneAstral && DownedBossSystem.downedAstrumAureus && Main.LocalPlayer.slotsMinions > 2 && (Main.LocalPlayer.ZoneDirtLayerHeight || Main.LocalPlayer.ZoneRockLayerHeight)).AddItemDisplay(ModContent.ItemType<StarbusterCore>()));

            fannyMessages.Add(new FannyMessage("NotBlessedApple", "A smart one ey? Unfortunately, only hostile Unicorns are able to break those astral batties open.",
  "Nuhuh", (FannySceneMetrics scene) => Main.LocalPlayer.Calamity().ZoneAstral && DownedBossSystem.downedAstrumAureus && Main.LocalPlayer.slotsMinions > 2 && (Main.LocalPlayer.ZoneDirtLayerHeight || Main.LocalPlayer.ZoneRockLayerHeight) && Main.LocalPlayer.mount.Type == MountID.Unicorn));

            fannyMessages.Add(new FannyMessage("SideGar", "Have you ever heard of gars? They're a neat fish group that you can rip open for valuable loot. One species of gar is the Side Gar, which can be fished up in sky lakes!",
  "Nuhuh", (FannySceneMetrics scene) => Main.LocalPlayer.ZoneSkyHeight && NPC.downedMoonlord).AddItemDisplay(ModContent.ItemType<SideGar>()));

            fannyMessages.Add(new FannyMessage("RearGar", "Fossilized tree bark!? In the Jungle's mud!? That sounds disgusting! I'll send over some gars to clean it up for you my friend. But, if you ever want some of that stuff for whatever reason, just go fish for some gars in the Jungle!",
  "Nuhuh", (FannySceneMetrics scene) => DownedBossSystem.downedProvidence).AddItemDisplay(ModContent.ItemType<RearGar>()).SetHoverTextOverride("Thank you so much Fanny! Tree bark is disgusting!"));

            fannyMessages.Add(new FannyMessage("FrontGar", "Now why did that ghost thing cause the ocean to go all crazy? Who knows! But what I do know is that the gars in the Abyss have started mutating. You should try fishing up some gars from the Sulphurous Sea and see if you can extract them for something useful.",
  "Nuhuh", (FannySceneMetrics scene) => Main.LocalPlayer.Calamity().ZoneSulphur && DownedBossSystem.downedPolterghast).AddItemDisplay(ModContent.ItemType<FrontGar>()));
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

            fannyMessages.Add(new FannyMessage("Anauwu", "I sense an ominous presence. I think the best course of action here would be to kill everything you see. If something is dead it can't hurt you!",
                "Idle", (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<LeviathanStart>())));

            fannyMessages.Add(new FannyMessage("Fairy", "That thing is hurting my eyes! Kill it, quick!",
                "Sob", (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == NPCID.EmpressButterfly)));

            fannyMessages.Add(new FannyMessage("Cultists", "Looks like some blue robe-wearing hooligans are worshiping a coin! Try not to interrupt them, they seem to be having a good time.",
                "Nuhuh", (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == NPCID.CultistDevote)));

            fannyMessages.Add(new FannyMessage("AncientDom", "Who is this guy???",
                "Sob", (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == NPCID.AncientCultistSquidhead)));

            fannyMessages.Add(new FannyMessage("Bloodworm", "Crush it under your boot.",
                "Idle", (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<BloodwormNormal>() || n.type == ModContent.NPCType<BloodwormFleeing>())));

            fannyMessages.Add(new FannyMessage("Wolf", "Aw look a cute wolf! You can extract valuable Coyote Venom from their lifeless corpses in order to make some neat ice items.",
    "Idle", (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == NPCID.Wolf)).AddItemDisplay(ModContent.ItemType<CoyoteVenom>()));

            fannyMessages.Add(new FannyMessage("Dungeondie", "Oh, it appears my hack didn't work.",
    "Sob", (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == NPCID.DungeonGuardian) && Main.LocalPlayer.dead));

            #endregion

            #region Event
            fannyMessages.Add(new FannyMessage("Raining", "It's raining! It's pooring! The man on the moon is snoring! Wait, who is the man on the moon!?",
   "Idle", (FannySceneMetrics scene) => Main.raining).SetHoverTextOverride("That's a good question Fanny!"));

            fannyMessages.Add(new FannyMessage("Winding", "Ah the weather is so nice out today! You should go fly a kite! That's something a lot of people were interested in right?",
   "Idle", (FannySceneMetrics scene) => Main._shouldUseWindyDayMusic));

            fannyMessages.Add(new FannyMessage("OOA", "Just so we're clear, none of this is canon, got it?",
   "Nuhuh", (FannySceneMetrics scene) => Main.invasionType == InvasionID.CachedOldOnesArmy).SetHoverTextOverride("Gotcha Fanny!"));

            fannyMessages.Add(new FannyMessage("InvasionDeath", "These guys are really giving us what for. It might be a good idea to step away for a bit in order to come up with a new strategy...",
                "Sob", (FannySceneMetrics scene) => Main.LocalPlayer.dead && Main.invasionType != InvasionID.None, cooldown: 1200));

            fannyMessages.Add(new FannyMessage("BloodMoon", "During a blood moon, strange critters can be found hiding under rocks. They can be used for blood moon fishing, but be careful, those teeth can really hurt.",
   "Idle", (FannySceneMetrics scene) => Main.bloodMoon));

            fannyMessages.Add(new FannyMessage("Eclipxe", "It's dark.",
   "Sob", (FannySceneMetrics scene) => Main.eclipse && !DownedBossSystem.downedDoG).SetHoverTextOverride("It is."));

            fannyMessages.Add(new FannyMessage("Holloween", "Happy Halloween my friend! Looks like everyone is getting their spook game on. Get ready for a monster mash!",
   "Idle", (FannySceneMetrics scene) => Main.pumpkinMoon && !DownedBossSystem.downedDoG));

            fannyMessages.Add(new FannyMessage("Frostmas", "IT'S CHRISTMAS!!! You don't need to get me a gift, just having you around is the most fan-tastic gift a flame like me could ask for!",
   "Idle", (FannySceneMetrics scene) => Main.snowMoon && !DownedBossSystem.downedDoG).SetHoverTextOverride("Awe, thanks Fanny, you're great to have around too!"));

            #endregion

            #region Boss
            fannyMessages.Add(new FannyMessage("Mutant", "Woah, how much HP does that guy have??",
    "Awooga", (FannySceneMetrics scene) => CrossModBoss(scene, "FargowiltasSouls", "MutantBoss")));

            fannyMessages.Add(new FannyMessage("TorchGod", "A fellow being of the flames! It seems you played with a bit too much fire and now you are facing the wrath of the almighty Torch God! Don't worry though, he's impervious to damage, so you won't be able to hurt him.",
              "Awooga", (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == NPCID.TorchGod)));

            fannyMessages.Add(new FannyMessage("Mutant", "Woah, how much HP does that guy have??",
    "Awooga", (FannySceneMetrics scene) => CrossModBoss(scene, "FargowiltasSouls", "MutantBoss")));

            fannyMessages.Add(new FannyMessage("Goozma", "Holy cow! It's THE Goozma! An easy way to defeat this slippery menace is to lead him into shimmer.",
                "Awooga", (FannySceneMetrics scene) => CrossModBoss(scene, "CalamityHunt", "Goozma")));

            fannyMessages.Add(new FannyMessage("Astrageldon", "Woah, this boss seems a little strong for you! Maybe come back after you’ve killed the Moon Lord!",
                "Nuhuh", (FannySceneMetrics scene) => CrossModBoss(scene, "CatalystMod", "Astrageldon")).SetHoverTextOverride("Thanks you Fanny! I'll go kill the Moon Lord first."));

            fannyMessages.Add(new FannyMessage("Calclone", "It is time. The Brimstone Witch, the one behind the Calamity in this world. You will now face Supreme Witch, Calamitas and end everything once and for all!",
               "Awooga", (FannySceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<CalamitasClone>())));

            fannyMessages.Add(new FannyMessage("Deus", "It appears that you are once again fighting a large serpentine creature. Therefore, it's advisable to do what you've done with them before and blast it with fast single target weaponry!",
               "Idle", (FannySceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<AstrumDeusHead>())));

            fannyMessages.Add(new FannyMessage("DeusSplitMod", "This is getting out of hand! Now there are two of them!",
               "Awooga", (FannySceneMetrics scene) => !Main.zenithWorld && NPC.CountNPCS(ModContent.NPCType<AstrumDeusHead>()) > 1));

            fannyMessages.Add(new FannyMessage("YharvelQuip", "Is it just me, or is it getting hot in here?",
                "Awooga", YharonPhase2));

            fannyMessages.Add(new FannyMessage("DraedonEnter", "Gee willikers! It's the real Draedon! He will soon present you with a difficult choice between three of your previous foes but with new attacks and increased difficulty. This appears to be somewhat of a common theme with this world dontcha think?",
               "Awooga", (FannySceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<Draedon>())));

            fannyMessages.Add(new FannyMessage("ExoMayhem", "Wow! What a mayhem! Don't panic though, if you focus on dodging, you will be less likely to get hit. A common strategy for these tin cans is to \" fall god \", which I believe means summoning other gods like the Slime God and killing them for extra health. You should also pay extra attention to Ares' red cannon, because sometimes it can sweep across the screen, ruining your dodge flow. As for the twins, keep a close eye on the right one, as it has increased fire rate. There is no saving you from Thanatos, it isn't synced and breaks the structure these guys are allegedly supposed to have. Like seriously, why do the twins and Ares hover to the sides and above you while that robo-snake just does whatever the heckle heckity heckering hecky heck he wants? It would be significantly more logical if it tried to like stay below you, but no. Anyways, good luck buddy! You're almost at the end, you can do this!",
                "Idle", (FannySceneMetrics scene) => !Main.zenithWorld && CalamityGlobalNPC.draedonExoMechPrime != -1 && CalamityGlobalNPC.draedonExoMechTwinGreen != -1 && CalamityGlobalNPC.draedonExoMechWorm != -1, needsToBeClickedOff: false, duration: 22));
            #endregion

            #region BossDeath

            fannyMessages.Add(new FannyMessage("AbyssBegin", "Every 60 seconds in the Abyss a hour passes by, truly wonderful!",
               "Nuhuh", (FannySceneMetrics scene) => !Main.zenithWorld && NPC.downedBoss3).SetHoverTextOverride("Very interesting Fanny!"));

            fannyMessages.Add(new FannyMessage("Cryodeath", "Ha! Snow's over, Cryogen! Wasn't that pretty cool?",
               "Idle", (FannySceneMetrics scene) => !Main.zenithWorld && DownedBossSystem.downedCryogen));

            fannyMessages.Add(new FannyMessage("DraedonExit", "Good golly! You did it! Though I'd HATE to imagine the financial losses caused by the destruction of those machines.",
                "Awooga", (FannySceneMetrics scene) => !Main.zenithWorld && DownedBossSystem.downedExoMechs));

            fannyMessages.Add(new FannyMessage("SCalDie", "That was exhilarating! Though that means the end of our adventure is upon us. What a Calamity as one may say!",
                "Awooga", (FannySceneMetrics scene) => !Main.zenithWorld && DownedBossSystem.downedCalamitas));

            fannyMessages.Add(new FannyMessage("CalcloneDeath", "Oh it was just a clone.",
                "Sob", (FannySceneMetrics scene) => !Main.zenithWorld && DownedBossSystem.downedCalamitasClone));

            fannyMessages.Add(new FannyMessage("Yharore", "Looks like the caverns have been laced with Auric Ore! The ore veins are pretty massive, so I’d say it’s best that you get up close and go hog wild!",
               "Idle", (FannySceneMetrics scene) => !Main.zenithWorld && DownedBossSystem.downedYharon).AddItemDisplay(ModContent.ItemType<AuricOre>()));

            #endregion

            #region GFB tips

            fannyMessages.Add(new FannyMessage("GFBDesert", "The sand worm is looking a bit blue isn't he? You may find your pitiful projectiles useless, but I'll reveal a wicked trick to seize the upper hand. As you know, this monstrous beast thrives on devouring your puny attacks and converting them into life-sustaining water. How delightfully diabolical! To subdue this insatiable leviathan, you must cunningly employ your trusty melee weapons.",
               "EvilIdle", (FannySceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<DesertScourgeHead>())).SpokenByEvilFanny());

            fannyMessages.Add(new FannyMessage("GFBCrab", "This guy is making me a bit dizzy, how rude. I don't have much to say about ol' crabson here, but you may find great profits once you bash its stupid green shell in.",
               "EvilIdle", (FannySceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<Crabulon>())).SpokenByEvilFanny());

            fannyMessages.Add(new FannyMessage("GfbHive", "Ohohoh you're gonna have fun with this one. The real kicker with the Hive Mind here is it can summon a cyst, a real game over trap, and if you're foolish enough to shatter it, you're in for a double dose of agony because another Hive Mind will spawn. So, I guess the real tip here is, don't be the idiot who rushes in without a plan, or you'll be in for a never-ending nightmare of your own incompetence.",
               "EvilIdle", (FannySceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<HiveMind>())).SpokenByEvilFanny());

            fannyMessages.Add(new FannyMessage("GfbPerf", "Brace yourself for a bloodcurdling challenge. This monstrous abomination will summon three nasty worms when you cut one down. The first worm, oh how annoying, burrows into its vile hive, replenishing its sorry health. The second, the pesky splitter, will break into gruesome blobs that yearn for your destruction. And the last, the sadistic laser lord, conjures deadly walls of scorching beams to trap you.",
               "EvilIdle", (FannySceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<PerforatorHive>())).SpokenByEvilFanny());

            fannyMessages.Add(new FannyMessage("GfbSlimeGod", "Ever felt that boss minions were too insignificant as you are? Well! In this world they will absolutely shower overwhelm you with dozens upon dozens of gel balls. My advice? Well, honestly, you're in for a world of hurt, but if you want to survive, focus on obliterating those puny slimes and evading their slimy detonations. Don't expect any mercy from this vile creation; it's only here to show you how insignificant you truly are.",
               "EvilIdle", (FannySceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<SlimeGodCore>())).SpokenByEvilFanny());

            fannyMessages.Add(new FannyMessage("GfbCryogen", "Wow, what a joke of a boss! You expected a frozen snowflake, but you get this pathetic excuse for a fire-themed one instead. It's as uninspired as it sounds! I hope you like homing fireballs!",
               "EvilIdle", (FannySceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<Cryogen>())).SpokenByEvilFanny());

            fannyMessages.Add(new FannyMessage("GfbAqua", "You're in for a real treat, you hapless fool, because this maggot's party trick is a nightmare! From the moment it slithers into your miserable existence, it'll wrap around you like a constricting nightmare, pelting you with projectiles aimed right at your feeble core. If you dare try to escape its lethal embrace, you'll be generously rewarded with a dose of crippling poison. So here's your precious tip: Accept your wretched fate and relish in the poison's sweet caress! Dance with this serpent of doom, dodging its toxic barrage with pinpoint precision, and strike back when it's momentarily vulnerable. Or flee, like the coward you are, and let the poison slowly consume you while it mocks your pathetic attempts to escape. The choice is yours, weakling. Enjoy your futile struggle!",
               "EvilIdle", (FannySceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<AquaticScourgeHead>())).SpokenByEvilFanny());

            fannyMessages.Add(new FannyMessage("GfbBrimmy", "Well, isn't this a surprise? Our fiery friend here usually sticks to playing with her little flames, but today she's feeling a bit more generous, unleashing her inner multi-elemental diva. How utterly inconvenient for you! You better brace yourselves, because in addition to her fiery tantrums, she's now throwing all kinds of elements your way. Sand, clouds, water—you name it, she's got it. Here's a tip, even though you probably don't deserve it: Pay attention to the boss's body language and you'll see what element she's currently focusing.",
               "EvilIdle", (FannySceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<BrimstoneElemental>())).SpokenByEvilFanny());

            fannyMessages.Add(new FannyMessage("GfbCalclone", "You pitiful shrimp! As if facing a mechanical eye that hurls fireballs and spawns its wretched siblings charging at you isn't challenging enough, now you find yourselves confined to a pitiful arena, your movement mercilessly restricted. Ha! Your usual tactics won't save you here. Show some wits for once, or burn like the fools you truly are!",
               "EvilIdle", (FannySceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<CalamitasClone>())).SpokenByEvilFanny());

            fannyMessages.Add(new FannyMessage("GfbAureus", "This laughable arachnid thinks it can pose a challenge, but its so-called 'strategy' is nothing more than a circus act. When it starts flailing around like a fish dropped in the desert, be ready for the pathetic display of generic projectile patterns! The only thing intimidating here is the fact that it actually believes it can defeat you. Mock it mercilessly, dodge its sad projectiles, and rain down destruction upon its pitiful mechanical frame. The only question is: Can you crush this pitiable pest before it embarrasses itself further? Show this metallic mockery what true power looks like!",
              "EvilIdle", (FannySceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<AstrumAureus>())).SpokenByEvilFanny());

            fannyMessages.Add(new FannyMessage("GfbFishies", "Well, well, it looks like our roles have been cruelly reversed this time, doesn't it? You'll be facing off against a gargantuan mermaid, and the so-called 'Leviathan' has been downsized to a mere annoyance. As always, remember that every challenge has a solution, but I won't be the one to hand it to you. You'll have to figure it out on your own, and you'd better do it fast, or you'll find yourself singing a sad tune at the bottom of the ocean. Good luck, or should I say, 'good riddance'!",
             "EvilIdle", (FannySceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<Leviathan>())).SpokenByEvilFanny());

            fannyMessages.Add(new FannyMessage("Gfbplague", "The Tyrant's lovely favorite bee seems to be a bit more sick than usual. Those rockets of hers? Oh, they won't just explode; they'll leave a pestilent mist lingering around to ruin your day. But, hold on, sometimes she tosses peanuts at you - yeah, you heard that right! And if you're 'lucky,' you might even see a gauss nuke or two. So, don't just stand there with your jaws agape; stay alert and dodge her garbage attacks. This boss is as unpredictable as it gets, but you better figure her out if you want any chance of survival.",
             "EvilIdle", (FannySceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<PlaguebringerGoliath>())).SpokenByEvilFanny());

            fannyMessages.Add(new FannyMessage("GfbRavager", "Ah, what a pathetic attempt at originality! This walking corpse fortress may think it's clever, but it's nothing more than a hackneyed copycat. It shamelessly rips off Sans from Undertale, summoning those ridiculously uninspired skulls that shoot lasers at you. If you're dim-witted enough to get hit by these feeble attacks, you don't even deserve to play this game. But if you want to survive this sorry excuse for a boss battle, just dodge the lackluster lasers, and focus your attacks on the so-called 'fortress' itself. Don't be fooled by this uninspired imitation, and put it out of its misery before it realizes how utterly unoriginal it is!",
             "EvilIdle", (FannySceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<RavagerBody>())).SpokenByEvilFanny());

            fannyMessages.Add(new FannyMessage("GfbAstrumDeus", "Welcome, feeble player, to a cosmic encounter with a space worm beyond your meager comprehension. There's a chance you've faced it before, and it cunningly split into two to vex you. Now, in a delicious twist, it dares to multiply once again, though it'll remain a delightful secret, so I won't bother to spell it out for your primitive intellect. You'll have to guess how many offspring you're up against. Can you even count that high, imbecile? Just remember, victory may require more than your pathetic usual strategy. But don't expect me to guide you any further, it's more amusing to watch you flounder.",
             "EvilIdle", (FannySceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<AstrumDeusHead>())).SpokenByEvilFanny());

            fannyMessages.Add(new FannyMessage("GfbDonuts", "Ah, what an utter disappointment these three pitiful constructs are! You'd think they'd be as unique as they are grotesque, but alas, you'll barely find any interesting twists here, just more mediocrity. As they clumsily lumber around with their molten bodies and clichéd flaming wings, your best bet is to stick to the basics.",
             "EvilIdle", (FannySceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<ProfanedGuardianCommander>())).SpokenByEvilFanny());

            fannyMessages.Add(new FannyMessage("GfbBirb", "Oh, look who decided to show up in this pathetic game! Our little draconic offshoot has had a makeover, or should I say a 'downgrade.' Now, it shrieks like a demented parakeet on steroids and thinks it's cute to summon its wimpy babies only for them to EXPLODE into lightning. What a low tier boss.",
             "EvilIdle", (FannySceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<Bumblefuck>())).SpokenByEvilFanny());

            fannyMessages.Add(new FannyMessage("GfbProvidence", "Only god can help you here.",
             "EvilIdle", (FannySceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<Providence>())).SpokenByEvilFanny());

            fannyMessages.Add(new FannyMessage("GfbCeaseless", "Prepare for a rollercoaster of frustration, you miserable little adventurer. This malevolent monstrosity will gleefully toy with your feeble senses by warping the world into a nightmarish kaleidoscope of black and rainbow hues. Don't even bother trying to make sense of it; it's all just a taunting charade. To survive this despicable encounter, you'll need to navigate the disorienting chaos with the grace of a drunken clown.",
             "EvilIdle", (FannySceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<CeaselessVoid>())).SpokenByEvilFanny());

            fannyMessages.Add(new FannyMessage("GfbSignus", "This spectral jokester loves to play hide and seek, but he's got a little trick up his ethereal sleeve. When he stands still and goes all see-through, he's not just taking a break for a ghostly snack, he's charging up an attack that'll make you wish you never created a world with this seed. So, here's a tip for you, if your feeble mind can handle it: Keep a close eye on this translucent loser, and when he starts to shimmer and glow, run for cover like the coward you are. If you don't, you'll feel the full force of his otherworldly wrath, and there's no saving you from that.",
             "EvilIdle", (FannySceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<Signus>())).SpokenByEvilFanny());

            fannyMessages.Add(new FannyMessage("GfbStormWeaver", "You there, feeble player, prepare to be humiliated by the wrath of this insufferable sky serpent. This time, it's even longer than your list of shortcomings! Not much else to say here, just use piercing weapons or explosions.",
             "EvilIdle", (FannySceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<StormWeaverHead>())).SpokenByEvilFanny());

            fannyMessages.Add(new FannyMessage("GfbPolter", "Return at 3 am for a surprise.",
                "EvilIdle", (FannySceneMetrics scene) => !(Main.time >= 27000 && Main.time < 30600 && Main.dayTime == false) && Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<Polterghast>())).SpokenByEvilFanny());

            fannyMessages.Add(new FannyMessage("GfbOverdose", "It's time for the laziest, most shameless, uninspired pile of pixelated junk you've ever seen, a sorry excuse for a boss who couldn't even come up with its own look! Meet this 'brown recolor' ripoff, a pitiful imitation of an already goofy boss. And guess what? This time, they've taken to flatulence as their 'unique' attack! Get ready to hold your nose and your laughter because it's about to get 'stinky' in here. Just dodge the gas clouds, seriously, how hard can it be? Pummel this pathetic carbon copy and watch it dissolve into the pixelated void where it belongs. If you thought the original was a challenge, prepare to be underwhelmed by this trash!",
              "EvilIdle", (FannySceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<OldDuke>())).SpokenByEvilFanny());

            fannyMessages.Add(new FannyMessage("GfbYharon", "Oh, look who's trying to play hero, facing the chicken with its fancy fiery bullet hell attacks! But here's a little evil twist for you, you pathetic weakling: this dragon can heal back all its health, just to spite you. You think you're so clever? Well, you're not. To stand a chance, you'll have to do more than just dodge its relentless onslaught – you'll need to focus on interrupting its little healing charade. Attack like your life depends on it, because it does, you fool! Don't let this dragon laugh at your misery as it regenerates its health, or you'll be nothing more than a stain on its claws.",
              "EvilIdle", (FannySceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<Yharon>())).SpokenByEvilFanny());

            fannyMessages.Add(new FannyMessage("GfbExos", "As you stumble into this chaotic clash of metal monstrosities, let's address the real challenge here: the spineless nerd who's finally decided to get off his lazy behind and join the fray. The colossal skeletal abomination, the sinister fighter plane 'eyes,' and the writhing serpent spewing laser turrets are nothing compared to the pitiful attacks from their mastermind. You'll need to multitask your feeble brain, dodging the obvious and pathetic shots from the so-called 'genius' while taking down the hulking mechs. Focus on systematically disabling each colossal creation one at a time while smirking at the thought of that wannabe inventor's pathetic attempts to hinder you. Prove to them that they're nothing more than a sideshow in this battle, and leave them to wallow in their own mediocrity as you claim your victory with sheer malevolence.",
              "EvilIdle", (FannySceneMetrics scene) => Main.zenithWorld && (scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<AresBody>() && n.ModNPC is CalamityMod.NPCs.ExoMechs.Ares.AresBody head && !head.exoMechdusa) || scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<ThanatosHead>() && n.ModNPC is CalamityMod.NPCs.ExoMechs.Thanatos.ThanatosHead head && !head.exoMechdusa) || scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<Apollo>() && n.ModNPC is CalamityMod.NPCs.ExoMechs.Apollo.Apollo head && !head.exoMechdusa))).SpokenByEvilFanny());

            fannyMessages.Add(new FannyMessage("Hekate", "Behold! The most wretched amalgamation of monstrosities you've ever witnessed! The fabricational quartet has merged into a chaotic nightmare. What's this, you ask? The skeleton's got two puny probes now, shooting lasers? How quaint! To defeat this unholy amalgam, you'll need to exploit their combined idiocy. When these dimwits can't coordinate, they'll crumble under their own incompetence. Embrace the chaos and savor their impending doom!",
              "EvilIdle", (FannySceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<AresBody>() && n.ModNPC is CalamityMod.NPCs.ExoMechs.Ares.AresBody head && head.exoMechdusa)).SpokenByEvilFanny());

            fannyMessages.Add(new FannyMessage("GfbScal", "Well, well, you've finally reached the pinnacle of your pathetic little adventure. Meet the grand finale, the wicked witch of scarlet brimstone flames and necromancy. She's not one to follow the rules, so here's a dirty secret to make her life more miserable. When she summons that ghastly centipede made of human leftovers, don't expect it to kick the bucket easily this time. You'll need to work your sorry behind off and kill it yourself! Oh, and remember her cozy square arena? Surprise, surprise, it's all random now, like popcorn popping in hell. But here's the icing on the cake – her projectiles? All swapped out, so you can't rely on muscle memory. Have fun stumbling through this chaotic nightmare, because I'm not here to hold your hand, hero!",
              "EvilIdle", (FannySceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<SupremeCalamitas>())).SpokenByEvilFanny());

            //fannyMessages.Add(new FannyMessage("CirrusBoss", "Well, congratulations, you've stumbled upon the pinnacle of this abhorrent seed - a self-indulgent, self-insert boss who's nothing more than a snobby, drunken princess. The developer's inflated ego oozes from every pixel of this farce. When this wretched spectacle begins, prepare for a relentless barrage of recycled, inferior boss fights. Yes, she'll pull out all the stops by summoning those laughable earlier bosses to do her dirty work. And as if that weren't enough, she'll pelt you with purple beams and alcohol bottles, probably echoing her own drunken stupor. But don't fret, you can still prevail. Keep your wits about you, dodge the recycled nonsense, and strike back at this embarrassing monstrosity. Let's face it, beating her is the only satisfaction this game offers.",
            //  "EvilIdle", (FannySceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<SupremeCalamitas>() && n.ModNPC is CalamityMod.NPCs.SupremeCalamitas.SupremeCalamitas head && !head.cirrus)).SpokenByEvilFanny());

            fannyMessages.Add(new FannyMessage("TheLorde", "Hey there, you clueless wannabe gamer! You've stumbled upon the most infuriating easter egg boss in gaming history. This abomination of an AI can't make up its damn mind and switches between the traits of every boss and enemy you've ever faced, just to make sure you can't predict a thing. So, don't even bother thinking you've got it figured out. When this wretched abomination is on its last legs, it pulls the ultimate cheat move - going invincible for a whole agonizing minute before you can finally put it out of its misery. And yeah, you'll be pulling your hair out, but remember, even this unholy mess can't escape its inevitable doom. So, gear up, slug through that excruciating minute, and claim your hollow victory, loser!",
              "EvilIdle", (FannySceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<THELORDE>())).SpokenByEvilFanny());

            #endregion

            #region Misc
            fannyMessages.Add(new FannyMessage("LowHP", "It looks like you're low on health. If your health reaches 0, you'll die. To combat this, don't let your health reach 0!",
                "Nuhuh", (FannySceneMetrics scene) => Main.LocalPlayer.statLife < Main.LocalPlayer.statLifeMax2 * 0.25f, cooldown: 300, onlyPlayOnce: false).SetHoverTextOverride("Thanks Fanny! I'll heal."));

            fannyMessages.Add(new FannyMessage("Invisible", "Where did you go?",
                "Sob", (FannySceneMetrics scene) => Main.LocalPlayer.invis || Main.LocalPlayer.shroomiteStealth || Main.LocalPlayer.vortexStealthActive || Main.LocalPlayer.Calamity().rogueStealth >= Main.LocalPlayer.Calamity().rogueStealthMax, onlyPlayOnce: true).SetHoverTextOverride("I'm still here Fanny!"));

            #endregion      

            IntroducingEvilFanny();
            MoonLordTextDump();
            Babil();
        }

        #endregion

        private static void IntroducingEvilFanny()
        {

            FannyMessage introLore = new FannyMessage("IntroducingEvilFanny", "My friend, we've made it to Hardmode! Plenty of new opportunities have popped up and plenty of dangerous new foes now lurk about.",
                "Idle", (FannySceneMetrics scene) => Main.hardMode, 8, needsToBeClickedOff: false, onlyPlayOnce: true, displayOutsideInventory: true).AddDelay(5);

            fannyMessages.Add(introLore);

            FannyMessage introEvilLore = new FannyMessage("IntroducingEvilFanny2", "'Sup",
                "EvilIdle", FannyMessage.AlwaysShow, 6, needsToBeClickedOff: false, onlyPlayOnce: true, displayOutsideInventory: true)
                .NeedsActivation(4f).SpokenByEvilFanny();

            introLore.AddStartEvent(() => introEvilLore.ActivateMessage());

            fannyMessages.Add(introEvilLore);

            FannyMessage introLore2 = new FannyMessage("IntroducingEvilFanny3", "E-evil Fanny!? I thought you moved away to the Yukon!",
                "Sob", FannyMessage.AlwaysShow, 8, needsToBeClickedOff: false, onlyPlayOnce: true, displayOutsideInventory: true)
                .NeedsActivation();
            introLore.AddEndEvent(() => introLore2.ActivateMessage());

            fannyMessages.Add(introLore2);

            FannyMessage introEvilLore2 = new FannyMessage("IntroducingEvilFanny4", "Yeah. Got cold.",
               "EvilIdle", FannyMessage.AlwaysShow, 5, needsToBeClickedOff: false, onlyPlayOnce: true, displayOutsideInventory: true)
               .NeedsActivation().SpokenByEvilFanny();

            introEvilLore.AddEndEvent(() => introEvilLore2.ActivateMessage());

            fannyMessages.Add(introEvilLore2);

            FannyMessage introLore3 = new FannyMessage("IntroducingEvilFanny5", "$0, it seems my evil counterpart, Evil Fanny, has returned! Don't trust a thing they say, and hopefully they'll leave..",
               "Idle", FannyMessage.AlwaysShow, 8, onlyPlayOnce: true, displayOutsideInventory: true)
               .NeedsActivation().AddDynamicText(FannyMessage.GetPlayerName);

            introLore2.AddEndEvent(() => introLore3.ActivateMessage());

            fannyMessages.Add(introLore3);
        }
        private static void MoonLordTextDump()
        {

            FannyMessage ml = new FannyMessage("ML1", "Blegh, I think I swallowed one of that thing's bones. Well, it's time for Godseeker Mode. You will face a sequence of challenges, each more difficult than the last with little to no breathing between encounters.",
                "Idle", (FannySceneMetrics scene) => NPC.downedMoonlord, 7, needsToBeClickedOff: true, onlyPlayOnce: true, displayOutsideInventory: true).AddDelay(0.4f);

            fannyMessages.Add(ml);

            FannyMessage ml2 = new FannyMessage("ML2", "Almost sounds like a boss rush or something.",
                "EvilIdle", FannyMessage.AlwaysShow, 6, needsToBeClickedOff: false, onlyPlayOnce: true, displayOutsideInventory: true)
                .NeedsActivation(4f).SpokenByEvilFanny();

            ml.AddStartEvent(() => ml2.ActivateMessage());

            fannyMessages.Add(ml2);

            FannyMessage ml3 = new FannyMessage("ML3", "A priority you should take care of immediately is harvesting Unholy Essence from some new, fearsome creatures that have appeared in the Underworld and Hallow. You can then use the essence to make the Rune of Kos and summon the Sentinels of the Devourer.",
                "Idle", FannyMessage.AlwaysShow, 6, needsToBeClickedOff: true, onlyPlayOnce: true, displayOutsideInventory: true).AddItemDisplay(ModContent.ItemType<RuneofKos>()).NeedsActivation();

            ml.AddEndEvent(() => ml3.ActivateMessage());
            fannyMessages.Add(ml3);

            FannyMessage ml4 = new FannyMessage("ML4", "You can find 3 different types of cosmic remains if you search the sky, one of them is the remains of the Moon guy you just defeated! Second one is some exotic clusters used for some artifacts! Third one is the distorted remains of Cosmos itself.",
                "Idle", FannyMessage.AlwaysShow, 6, needsToBeClickedOff: true, onlyPlayOnce: true, displayOutsideInventory: true).NeedsActivation();
            ml3.AddEndEvent(() => ml4.ActivateMessage());
            fannyMessages.Add(ml4);

            FannyMessage ml5 = new FannyMessage("ML5", "The Dungeon has also gotten an upgrade in power, with new spirit enemies that occasionally pop out of enemies when defeated which drop Phantoplasm, an important crafting material. I'd reccomend killing as many of those things as possible!",
                "Idle", FannyMessage.AlwaysShow, 6, needsToBeClickedOff: true, onlyPlayOnce: true, displayOutsideInventory: true).NeedsActivation().AddItemDisplay(ModContent.ItemType<Polterplasm>());
            ml4.AddEndEvent(() => ml5.ActivateMessage());
            fannyMessages.Add(ml5);

            FannyMessage ml6 = new FannyMessage("ML6", "I'm getting a bit of deja vu here.",
                "EvilIdle", FannyMessage.AlwaysShow, 6, needsToBeClickedOff: false, onlyPlayOnce: true, displayOutsideInventory: true)
                .NeedsActivation(4f).SpokenByEvilFanny();

            ml5.AddStartEvent(() => ml6.ActivateMessage());

            fannyMessages.Add(ml6);


            FannyMessage ml7 = new FannyMessage("ML7", "It appears that the red moon will start yielding bountiful harvests of Blood Orbs now! You should take advantage of this opportuntiy to craft lotsa potions! I'm personally a fan of the Inferno Potion myself.",
                "Nuhuh", FannyMessage.AlwaysShow, 6, needsToBeClickedOff: true, onlyPlayOnce: true, displayOutsideInventory: true).NeedsActivation().AddItemDisplay(ItemID.InfernoPotion);
            ml5.AddEndEvent(() => ml7.ActivateMessage());
            fannyMessages.Add(ml7);


            FannyMessage ml8 = new FannyMessage("ML8", "Are you feeling a little weak? It may be because of the dreaded \'Curse of the Eldritch\', a terrifying affliction inflicted upon those who slay eldritch beasts which permanently reduces your life regeneration!",
                "Sob", FannyMessage.AlwaysShow, 6, needsToBeClickedOff: true, onlyPlayOnce: true, displayOutsideInventory: true).NeedsActivation();
            ml7.AddEndEvent(() => ml8.ActivateMessage());
            fannyMessages.Add(ml8);


            FannyMessage ml9 = new FannyMessage("ML9", "I should also mention that if you have a certain thief in one of your towns, they'll start selling the flawless Celestial Reaper, which can be used to cut down herbs significantly faster than the normal Sickle.",
                "Idle", FannyMessage.AlwaysShow, 6, needsToBeClickedOff: true, onlyPlayOnce: true, displayOutsideInventory: true).NeedsActivation().AddItemDisplay(ModContent.ItemType<CelestialReaper>());
            ml8.AddEndEvent(() => ml9.ActivateMessage());
            fannyMessages.Add(ml9);

            FannyMessage ml10 = new FannyMessage("ML10", "Oh oh I should also mention-",
                "Idle", FannyMessage.AlwaysShow, 6, needsToBeClickedOff: false, onlyPlayOnce: true, displayOutsideInventory: true).NeedsActivation();
            ml9.AddEndEvent(() => ml10.ActivateMessage());
            fannyMessages.Add(ml10);

            FannyMessage ml11 = new FannyMessage("ML11", "Oh my god shut up already, how much can one boss unlock!?",
                "EvilIdle", FannyMessage.AlwaysShow, 6, needsToBeClickedOff: false, onlyPlayOnce: true, displayOutsideInventory: true)
                .NeedsActivation(2f).SpokenByEvilFanny();

            ml10.AddStartEvent(() => ml11.ActivateMessage());

            fannyMessages.Add(ml11);

            FannyMessage ml12 = new FannyMessage("ML12", "It appears this encounter is going to have to be cut short buddy, I need to go do something.",
                "Idle", FannyMessage.AlwaysShow, 6, needsToBeClickedOff: false, onlyPlayOnce: true, displayOutsideInventory: true).NeedsActivation();
            ml11.AddStartEvent(() => ml12.ActivateMessage());
            fannyMessages.Add(ml12);

            ml12.AddEndEvent(() => Violence());

        }

        private static void Babil()
        {
            FannyMessage babil1 = new FannyMessage("Babil1", "Hey there, adventurer! Have you heard about the Essence of Babil? It's this amazing crafting material that drops from certain jungle creatures. Let me give you some tips on how to find it!",
                "Nuhuh", (FannySceneMetrics scene) => Main.hardMode && Main.LocalPlayer.ZoneJungle).SetHoverTextOverride("Umm, Essence of Babil?");

            fannyMessages.Add(babil1);

            FannyMessage babil2 = new FannyMessage("Babil2", "Oh you sweet summer child! The Essence of Babil is this incredible, mystical substance you can gather from jungle enemies. It's a key ingredient for crafting some seriously awesome gear. You should definitely try to collect it!",
                "Nuhuh", FannyMessage.AlwaysShow).SetHoverTextOverride("Huh, okay. So, where do I find it?").AddDelay(2).NeedsActivation().AddItemDisplay(ModContent.ItemType<EssenceofBabil>());

            babil1.AddEndEvent(() => babil2.ActivateMessage());

            fannyMessages.Add(babil2);

            FannyMessage babil3 = new FannyMessage("Babil3", "Seriously? I just told you, it drops from jungle creatures. You know, those critters lurking around in the jungle biome? Go hunt them down, and you might get your hands on some Essence of Babil!",
                "Nuhuh", FannyMessage.AlwaysShow).SetHoverTextOverride("Jungle creatures... got it!").NeedsActivation();

            babil2.AddEndEvent(() => babil3.ActivateMessage());

            fannyMessages.Add(babil3);

            FannyMessage babil4 = new FannyMessage("Babil4", "The Essence of Babil used for crafting powerful items. You can create some fantastic air-themed equipment with it. Seriously, just check the crafting menu, it's all there!",
                "Nuhuh", FannyMessage.AlwaysShow).SetHoverTextOverride("I'm not sure I understand...").AddDelay(5).NeedsActivation().AddItemDisplay(ModContent.ItemType<EssenceofBabil>());

            babil3.AddEndEvent(() => babil4.ActivateMessage());

            fannyMessages.Add(babil4);

            FannyMessage babil5 = new FannyMessage("Babil5", "Okay, let me spell it out for you one more time. Essence of Babil is a crafting material. You find it in the jungle. You use it to make cool stuff. Got it now? Good!",
                "Nuhuh", FannyMessage.AlwaysShow).SetHoverTextOverride("Uh, thanks, Fanny. I think I get it now.").NeedsActivation().AddItemDisplay(ModContent.ItemType<EssenceofBabil>());

            babil4.AddEndEvent(() => babil5.ActivateMessage());

            fannyMessages.Add(babil5);

            FannyMessage babil6 = new FannyMessage("Babil6", "Hey there, adventurer... Have you heard about the Essence of Babil? It's this... remarkable crafting material that drops from such unworthy jungle creatures. Let me grace you with some information, whether you appreciate it or not.",
                "EvilIdle", (FannySceneMetrics scene) => Main.hardMode && Main.LocalPlayer.ZoneJungle).SetHoverTextOverride("Umm, Essence of Babil? What's that?").AddDelay(3).SpokenByEvilFanny();

            fannyMessages.Add(babil6);

            FannyMessage babil7 = new FannyMessage("Babil7", "Oh, how utterly clueless. The Essence of Babil is this incredibly mundane substance you can get from jungle enemies. You might even consider it somewhat important for crafting marginally useful gear. But, hey, who cares, right?",
                "EvilIdle", FannyMessage.AlwaysShow).SetHoverTextOverride("Huh, okay. So, where do I find it?").NeedsActivation().SpokenByEvilFanny();

            babil6.AddEndEvent(() => babil7.ActivateMessage());

            fannyMessages.Add(babil7);

            FannyMessage babil8 = new FannyMessage("Babil8", "Seriously? I can't believe I have to repeat myself. It drops from those jungle creatures, assuming you can manage to defeat them, of course. Go ahead, give it a shot. Not like it matters.",
                "EvilIdle", FannyMessage.AlwaysShow).SetHoverTextOverride("Jungle creatures... got it. But what can I make with it?").NeedsActivation().SpokenByEvilFanny().AddDelay(5);

            babil7.AddEndEvent(() => babil8.ActivateMessage());

            fannyMessages.Add(babil8);

            FannyMessage babil9 = new FannyMessage("Babil9", "You're really pushing your limits here, aren't you? It's used to craft... well, whatever. You can create some totally average air-themed equipment. But, honestly, who cares about that, right?",
                "EvilIdle", FannyMessage.AlwaysShow).SetHoverTextOverride("I'm not sure I understand...").NeedsActivation().SpokenByEvilFanny();

            babil8.AddEndEvent(() => babil9.ActivateMessage());

            fannyMessages.Add(babil9);

            FannyMessage babil10 = new FannyMessage("Babil10", "Of course, you don't!!! Why would I expect any different? Essence of Babil is just a crafting material. You find it in the jungle. You use it to make \"cool\" stuff, if you're into that sort of thing. But, frankly, I couldn't care less.",
                "EvilIdle", FannyMessage.AlwaysShow).SetHoverTextOverride("Uh, thanks, Evil Fanny. I think I get it now.").NeedsActivation().SpokenByEvilFanny().AddDelay(2);

            babil9.AddEndEvent(() => babil10.ActivateMessage());

            fannyMessages.Add(babil10);

            FannyMessage babil11 = new FannyMessage("Babil11", "You think you \"get it\"? You're beyond hopeless! There, now you're truly enlightened. Enjoy your essence... of oblivion!",
                "EvilIdle", FannyMessage.AlwaysShow, duration: 20).NeedsActivation().SpokenByEvilFanny().AddStartEvent(EssenceOfOblivionEvilFanny);

            babil10.AddEndEvent(() => babil11.ActivateMessage());

            fannyMessages.Add(babil11);
        }

        private static void EssenceOfOblivionEvilFanny()
        {
            Main.LocalPlayer.AddBuff(ModContent.BuffType<VulnerabilityHex>(), 216000);
            Main.LocalPlayer.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 216000);
            Main.LocalPlayer.AddBuff(ModContent.BuffType<BurningBlood>(), 216000);
            Main.LocalPlayer.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 216000);
            Main.LocalPlayer.AddBuff(ModContent.BuffType<HolyFlames>(), 216000);
            Main.LocalPlayer.AddBuff(ModContent.BuffType<MiracleBlight>(), 216000);
            Main.LocalPlayer.statLife = Math.Max(Main.LocalPlayer.statLife, Main.LocalPlayer.statLifeMax2 / 4);

            for (int i = 0; i < 5; i++)
                SoundEngine.PlaySound(SoundID.Thunder with { MaxInstances = 0 });
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