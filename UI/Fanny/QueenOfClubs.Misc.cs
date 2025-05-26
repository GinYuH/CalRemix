using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatBuffs;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.NPCs.Polterghast;
using CalRemix.Content.Items.Placeables;
using CalRemix.Content.Items.Weapons.Stormbow;
using CalRemix.Content.NPCs.Bosses.Hypnos;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using ReLogic.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Threading;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.ResourceSets;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using static Terraria.Graphics.Effects.Filters;
using static Terraria.ModLoader.ModContent;

namespace CalRemix.UI
{
    public partial class ScreenHelperManager : ModSystem
    {
        public const float QoC_timeAsleepToWaking = 25;
        public const float QoC_timeAwakeToAsleep = 30;

        public const float QoC_timeNextLightIdleBaseline = 0.5f;
        public const float QoC_timeNextLightIdleNoiseMin = 0;
        public const float QoC_timeNextLightIdleNoiseMax = 1;

        public const float QoC_timeNextFaceIdleBaseline = 0.5f;
        public const float QoC_timeNextFaceIdleNoiseMin = 0.25f;
        public const float QoC_timeNextFaceIdleNoiseMax = 1.5f;

        public static void LoadQoCMessages()
        {
            HelperMessage.New("QoCTest", "It looks like your armor's broken. You might want to get that checked out.It looks like your armor's broken. You might want to get that checked out.It looks like your armor's broken. You might want to get that checked out.It looks like your armor's broken. You might want to get that checked out.It looks like your armor's broken. You might want to get that checked out.It looks like your armor's broken. You might want to get that checked out.",
                "QueenOfClubsEmpty", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<ArmorCrunch>(), cooldown: 3, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.QueenOfClubs);
            HelperMessage.New("QoCTest2", "It",
                "QueenOfClubsEmpty", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<Crumbling>(), cooldown: 3, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.QueenOfClubs);
        }

        /// <summary>
        /// Tells the game to wake up the Queen of Clubs. If you want them to be present for dialogue, use this when you call a message
        /// they're involved in!
        /// </summary>
        public static void ForceWakeUpQoC()
        {
            Main.LocalPlayer.GetModPlayer<QoCPlayer>().timeUntilNextQoCAction_Heavy = HelperHelpers.GetTimeUntilNextStage(QoC_timeAwakeToAsleep);
            Main.LocalPlayer.GetModPlayer<QoCPlayer>().currentQoCState = (int)QoCPlayer.QoCState.Awake_Idle;
        }
    }

    public class QoCPlayer : ModPlayer
    {
        #region Data
        /// <summary>
        /// All of the states the Queen of Clubs can be in.
        /// </summary>
        public enum QoCState
        {
            Asleep = 0,
            Awake_Idle = 1,
            Awake_TurnLoop = 2,
            Awake_TurnLoopReverse = 3,
            Awake_TurnOnce = 4
        }
        /// <summary>
        /// All of the faces the Queen of Clubs can make. Make sure the name matches the last part of the png name!
        /// </summary>
        public enum QoCFace
        {
            Idle = 0,
            Test1 = 1,
            Test2 = 2
        }

        /// <summary>
        /// The length between the Queen of Clubs' next major action. This is used as a timer between being awake or asleep. 
        /// </summary>
        public int timeUntilNextQoCAction_Heavy;
        /// <summary>
        /// The length between the Queen of Clubs' next minor action. This is used as a timer between being flipping, not flipping, ect.
        /// </summary>
        public int timeUntilNextQoCAction_Light;
        /// <summary>
        /// The length between the Queen of Clubs' next face action. This is used as a timer between being changing face.
        /// </summary>
        public int timeUntilNextQoCAction_Face;
        /// <summary>
        /// The current action the Queen of Clubs is performing.
        /// </summary>
        public int currentQoCState = (int)QoCState.Asleep;
        /// <summary>
        /// The current face the Queen of Clubs is making.
        /// </summary>
        public QoCFace currentFace = 0;
        /// <summary>
        /// A buffer for the next face in line. When possible, the Queen of Clubs will switch to it.
        /// </summary>
        public QoCFace currentFaceBuffer = 0;

        /// <summary>
        /// Opacity at which the Queen of Clubs should render.
        /// </summary>
        public float fadeInOutSmoothing = 0;
        /// <summary>
        /// Set to 1 (or higher i guess) to make the Queen of Clubs bounce!
        /// </summary>
        public float bounce = 0;
        /// <summary>
        /// A vertical offset used to determine where the Queen of Clubs should be mid-bounce.
        /// </summary>
        public float bouncePositionOffset;

        #region Spinning values
        public float currentSpinRadians;
        public int spinReverse = 1;
        public float rateOfSpin;
        public float rateOfSpinIntended;
        public float rateOfSpinExtra;

        public const float rateOfSpin_Fast = 0.025f;
        public const float rateOfSpin_Normal = 0.015f;
        public const float rateOfSpin_Slow = 0.0075f;
        #endregion

        /// <summary>
        /// Whether or not the Queen of Clubs is currently awake.
        /// </summary>
        public bool isQoCAwake => currentQoCState >= (int)QoCState.Awake_Idle;
        //TODO: finalize unlock method
        /// <summary>
        /// Whether or not the Queen of Clubs is unlocked.
        /// </summary>
        public bool isQoCUnlocked => Player.GetModPlayer<CalRemixPlayer>().fifteenMinutesSinceHardmode <= 0;
        /// <summary>
        /// Whether or not the back of the card is facing the camera.
        /// </summary>
        public bool cardFacingBack => (currentSpinRadians + MathHelper.PiOver2) % MathHelper.TwoPi >= Math.PI;
        /// <summary>
        /// Whether or not the front of the card is facing the camera.
        /// </summary>
        public bool cardFacingFront => !cardFacingBack;
        /// <summary>
        /// The highest state. THIS IS NOT THE AMOUNT OF STATES! THIS IS THE VALUE OF THE HIGHEST STATE!
        /// </summary>
        public int stateMax => (int)Enum.GetValues(typeof(QoCState)).Cast<QoCState>().Max();
        /// <summary>
        /// The highest face.THIS IS NOT THE AMOUNT OF FACES! THIS IS THE VALUE OF THE HIGHEST FACE!
        /// </summary>
        public int faceMax => (int)Enum.GetValues(typeof(QoCFace)).Cast<QoCFace>().Max();

        public SlotId LaughSoundSlot;
        #endregion

        public override void PreUpdate()
        {
            if (isQoCUnlocked)
            {
                #region Opacity
                if (isQoCAwake)
                    fadeInOutSmoothing += 0.1f;
                else
                    fadeInOutSmoothing -= 0.1f;
                fadeInOutSmoothing = Math.Clamp(fadeInOutSmoothing, 0, 1);
                #endregion

                #region Minor Action Timer
                if (isQoCAwake && currentQoCState >= (int)QoCState.Awake_Idle && timeUntilNextQoCAction_Light <= 0)
                {
                    timeUntilNextQoCAction_Light = HelperHelpers.GetTimeUntilNextStage(ScreenHelperManager.QoC_timeNextLightIdleBaseline, ScreenHelperManager.QoC_timeNextLightIdleNoiseMin, ScreenHelperManager.QoC_timeNextLightIdleNoiseMax);
                    currentQoCState = Main.rand.Next(1, 3 + 1);
                }
                timeUntilNextQoCAction_Light--;
                #endregion

                #region Update Behavior
                if (currentQoCState == (int)QoCState.Awake_Idle)
                {
                    if (currentSpinRadians >= -0.05 && currentSpinRadians <= 0.05 && rateOfSpinExtra < 0.01f && rateOfSpinExtra > -0.01f)
                    {
                        rateOfSpin = 0;
                        rateOfSpinExtra = 0;
                    }
                    else
                    {
                        rateOfSpinIntended = rateOfSpin_Normal * spinReverse;
                        rateOfSpin = rateOfSpinIntended;
                    }
                }
                else if (currentQoCState == (int)QoCState.Awake_TurnLoop)
                {
                    rateOfSpinIntended = rateOfSpin_Normal;
                    spinReverse = 1;
                }
                else if (currentQoCState == (int)QoCState.Awake_TurnLoopReverse)
                {
                    rateOfSpinIntended = rateOfSpin_Normal;
                    spinReverse = -1;
                }
                else if (currentQoCState == (int)QoCState.Awake_TurnOnce)
                {
                    rateOfSpinIntended = rateOfSpin_Fast;

                    if (cardFacingBack)
                    {
                        currentQoCState = (int)QoCState.Awake_Idle;
                    }
                }
                #endregion

                #region Bounce
                if (bounce > 0)
                {
                    if (bounce > 0)
                        bounce -= 1 / (60f * 0.4f);

                    bouncePositionOffset = -MathF.Pow(Math.Abs(MathF.Sin(bounce * MathHelper.Pi)), 0.6f) * 22f;
                }
                else
                {
                    bounce = 0;
                    bouncePositionOffset = 0;
                }
                #endregion

                #region Face Swap Timer + Buffer
                if (isQoCAwake && timeUntilNextQoCAction_Face <= 0)
                {
                    timeUntilNextQoCAction_Face = HelperHelpers.GetTimeUntilNextStage(ScreenHelperManager.QoC_timeNextFaceIdleBaseline, ScreenHelperManager.QoC_timeNextFaceIdleNoiseMin, ScreenHelperManager.QoC_timeNextFaceIdleNoiseMax);

                    // if not idle, return to idle. if idle, new face
                    if (currentFace != 0)
                        currentFaceBuffer = 0;
                    else
                        currentFaceBuffer = (QoCFace)Main.rand.Next(1, faceMax + 1);

                    // flip to update face if needed
                    if (currentQoCState == (int)QoCState.Awake_Idle)
                        currentQoCState = (int)QoCState.Awake_TurnOnce;
                }
                timeUntilNextQoCAction_Face--;

                if (cardFacingBack)
                    currentFace = currentFaceBuffer;
                #endregion

               #region Awake/Asleep Timer
                if (timeUntilNextQoCAction_Heavy <= 0)
                {
                    if (currentQoCState >= (int)QoCState.Awake_Idle)
                    {
                        // if awake, go to sleep
                        timeUntilNextQoCAction_Heavy = HelperHelpers.GetTimeUntilNextStage(ScreenHelperManager.QoC_timeAwakeToAsleep);
                        currentQoCState = (int)QoCState.Asleep;
                    }
                    else
                    {
                        // if asleep, start waking up
                        timeUntilNextQoCAction_Heavy = HelperHelpers.GetTimeUntilNextStage(ScreenHelperManager.QoC_timeAsleepToWaking);
                        currentQoCState = (int)QoCState.Awake_Idle;
                        // randomize starting mode
                        currentSpinRadians = 0;
                        timeUntilNextQoCAction_Light = HelperHelpers.GetTimeUntilNextStage(ScreenHelperManager.QoC_timeNextLightIdleBaseline, ScreenHelperManager.QoC_timeNextLightIdleNoiseMin, ScreenHelperManager.QoC_timeNextLightIdleNoiseMax);
                        currentQoCState = Main.rand.Next(1, stateMax); // normally we would add one, but theres a 4th state we dont wanna roll into
                        // reset face
                        timeUntilNextQoCAction_Light = HelperHelpers.GetTimeUntilNextStage(ScreenHelperManager.QoC_timeNextFaceIdleBaseline, ScreenHelperManager.QoC_timeNextFaceIdleNoiseMin, ScreenHelperManager.QoC_timeNextFaceIdleNoiseMax);
                        currentFace = QoCFace.Idle;
                        // add a big spin on wake-up
                        rateOfSpinExtra = 0.25f * spinReverse;
                        // fuck it add some bounce too
                        bounce = 1;
                        // make some noise 
                        LaughSoundSlot = SoundEngine.PlaySound(new SoundStyle("CalRemix/Assets/Sounds/Helpers/QueenOfClubsLaugh") with { MaxInstances = 1, Volume = 3 }, Main.LocalPlayer.position);
                    }
                }
                #endregion
            }

            #region Spinning
            // adjust the rateOfSpin value to smoothly transition between current and new spin rate
            if (currentQoCState != (int)QoCState.Awake_Idle)
                rateOfSpin = MathHelper.Lerp(rateOfSpin, rateOfSpinIntended * spinReverse, 0.01f);

            // add the rate of spinning to the radians value, and wrap everything properly
            currentSpinRadians += rateOfSpin + rateOfSpinExtra;
            if (currentSpinRadians > MathHelper.PiOver2 * 4)
                currentSpinRadians -= MathHelper.PiOver2 * 4;
            else if (currentSpinRadians < 0)
                currentSpinRadians += MathHelper.PiOver2 * 4;

            // reduce the extra spin over time
            rateOfSpinExtra *= 0.98f;
            #endregion

            if (SoundEngine.TryGetActiveSound(LaughSoundSlot, out var drinkSound) && drinkSound.IsPlaying)
                drinkSound.Position = Player.Center;

            // kept outside of the awake/asleep timer region so when shes unlocked she immediately appears
            timeUntilNextQoCAction_Heavy--;
        }

        #region Data Save/Load
        public override void SaveData(TagCompound tag)
        {
            tag["TimeUntilNextQoCAction_Heavy"] = timeUntilNextQoCAction_Heavy;
            tag["TimeUntilNextQoCAction_Light"] = timeUntilNextQoCAction_Light;
            tag["TimeUntilNextQoCAction_Face"] = timeUntilNextQoCAction_Face;
            tag["currentQoCState"] = currentQoCState;
        }

        public override void LoadData(TagCompound tag)
        {
            timeUntilNextQoCAction_Heavy = tag.GetInt("TimeUntilNextQoCAction_Heavy");
            timeUntilNextQoCAction_Light = tag.GetInt("TimeUntilNextQoCAction_Light");
            timeUntilNextQoCAction_Face = tag.GetInt("TimeUntilNextQoCAction_Face");
            currentQoCState = tag.GetInt("currentQoCState");
        }
        #endregion
    }

    public class QueenOfClubsCard : UIElement
    {
        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);

            if (Main.LocalPlayer.GetModPlayer<QoCPlayer>().isQoCAwake && Main.LocalPlayer.GetModPlayer<QoCPlayer>().isQoCUnlocked)
            {
                Main.LocalPlayer.GetModPlayer<QoCPlayer>().rateOfSpinExtra = 0.25f * Main.LocalPlayer.GetModPlayer<QoCPlayer>().spinReverse;
                Main.LocalPlayer.GetModPlayer<QoCPlayer>().bounce = 1;
                Main.LocalPlayer.GetModPlayer<QoCPlayer>().LaughSoundSlot = SoundEngine.PlaySound(new SoundStyle("CalRemix/Assets/Sounds/Helpers/QueenOfClubsLaugh") with { PitchRange = (-0.75f, 0.75f), MaxInstances = 1, Volume = 3 }, Main.LocalPlayer.position);
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = GetDimensions();
            Rectangle dimensionsRect = dimensions.ToRectangle();
            //Utils.DrawInvBG(spriteBatch, dimensionsRect);

            Texture2D testSprite = ModContent.Request<Texture2D>("CalRemix/UI/Fanny/HelperQueenOfClubs" + Main.LocalPlayer.GetModPlayer<QoCPlayer>().currentFace.ToString()).Value;
            if (Main.LocalPlayer.GetModPlayer<QoCPlayer>().cardFacingBack)
                testSprite = ModContent.Request<Texture2D>("CalRemix/UI/Fanny/HelperQueenOfClubsBack").Value;
            Rectangle testFrame = testSprite.Frame(1, 1, 0, 0);

            Matrix rotation = Matrix.CreateRotationY(Main.LocalPlayer.GetModPlayer<QoCPlayer>().currentSpinRadians) * Matrix.CreateRotationZ((float)Math.Sin(Main.GlobalTimeWrappedHourly * 0.5f) * 0.1f);
            Matrix translation = Matrix.CreateTranslation(new Vector3(Main.screenWidth - 175, 100 + Main.LocalPlayer.GetModPlayer<QoCPlayer>().bouncePositionOffset, 0));
            //translation = Matrix.CreateTranslation(new Vector3(Main.screenWidth / 2, Main.screenHeight / 2, 0));
            Matrix view = Main.GameViewMatrix.TransformationMatrix;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, -220, 220);
            Matrix renderMatrix = rotation * translation * view * projection;
            Effect effect = Scene["CalRemix:NormalDraw"].GetShader().Shader;

            short[] indices = [0, 1, 2, 1, 3, 2];
            Vector3 center = new Vector3(0, 0, 0);

            Vector3 topLeft = new Vector3(center.X - (testFrame.Width / 2), center.Y - (testFrame.Height / 2), 0);
            Vector3 topRight = new Vector3(center.X + (testFrame.Width / 2), center.Y - (testFrame.Height / 2), 0);
            Vector3 botLeft = new Vector3(center.X - (testFrame.Width / 2), center.Y + (testFrame.Height / 2), 0);
            Vector3 botRight = new Vector3(center.X + (testFrame.Width / 2), center.Y + (testFrame.Height / 2), 0);

            VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[4];
            vertices[0] = new(topLeft, Color.White, new Vector2(0, 0));
            vertices[1] = new(topRight, Color.White, new Vector2(1, 0));
            vertices[2] = new(botLeft, Color.White, new Vector2(0, 1));
            vertices[3] = new(botRight, Color.White, new Vector2(1, 1));

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                effect.Parameters["textureResolution"].SetValue(testSprite.Size());
                effect.Parameters["sampleTexture"].SetValue(testSprite);
                effect.Parameters["frame"].SetValue(new Vector4(testFrame.X, testFrame.Y, testFrame.Width, testFrame.Height));
                effect.Parameters["uWorldViewProjection"].SetValue(renderMatrix);
                effect.Parameters["opacity"].SetValue(Main.LocalPlayer.GetModPlayer<QoCPlayer>().fadeInOutSmoothing);
                pass.Apply();

                Main.instance.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
                Main.instance.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, 4, indices, 0, 2);
            }
        }
    }

    #region Appending UI stuff together
    public class QueenOfClubsUIState : UIState
    {
        public QueenOfClubsCard QueenOfClubs;

        public override void OnInitialize()
        {
            //TODO: rn this and the translation matrix use duped values. centralize them
            Texture2D testSprite = ModContent.Request<Texture2D>("CalRemix/UI/Fanny/HelperQueenOfClubsIdle", AssetRequestMode.ImmediateLoad).Value;
            QueenOfClubs = new QueenOfClubsCard();
            QueenOfClubs.Width.Pixels = testSprite.Width;
            QueenOfClubs.Height.Pixels = testSprite.Height;
            QueenOfClubs.Top.Pixels = 100 - (testSprite.Height / 2);
            QueenOfClubs.Left.Pixels = -175 - (testSprite.Width / 2);
            QueenOfClubs.Left.Percent = 1;
            Append(QueenOfClubs);
        }
    }

    // This class will only be autoloaded/registered if we're not loading on a server
    [Autoload(Side = ModSide.Client)]
    internal class QueenOfClubsUISystem : ModSystem
    {
        private UserInterface QoCInterface;
        internal static QueenOfClubsUIState UIState;

        public override void Load()
        {
            QoCInterface = new UserInterface();
            UIState = new QueenOfClubsUIState();
            QoCInterface.SetState(UIState);
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
                    "CalRemix: QueenOfClubs",
                    delegate {

                        QoCInterface?.Update(Main._drawInterfaceGameTime);
                        QoCInterface.Draw(Main.spriteBatch, Main._drawInterfaceGameTime);
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
    #endregion

    #region Changing Healthbar Text, Color
    public class QueenOfClubsAsset : ModSystem
    {
        private const string FilePath = "CalRemix/Assets/ExtraTextures/QoCHealthbars/";

        private static FieldInfo setsfield = typeof(PlayerResourceSetsManager).GetField("_sets", BindingFlags.NonPublic | BindingFlags.Instance);
        private static Dictionary<string, IPlayerResourcesDisplaySet> _sets = (Dictionary<string, IPlayerResourcesDisplaySet>)setsfield.GetValue(Main.ResourceSetsManager);

        #region New
        public static Asset<Texture2D> Heart = null;
        public static Asset<Texture2D> Heart2 = null;

        public static Asset<Texture2D> Heart_Fill = null;
        public static Asset<Texture2D> Heart_Fill_B = null;
        public static Asset<Texture2D> Heart_Left = null;
        public static Asset<Texture2D> Heart_Middle = null;
        public static Asset<Texture2D> Heart_Right = null;
        public static Asset<Texture2D> Heart_Right_Fancy = null;
        public static Asset<Texture2D> Heart_Single_Fancy = null;

        public static Asset<Texture2D> HP_Fill = null;
        public static Asset<Texture2D> HP_Fill_Honey = null;
        public static Asset<Texture2D> HP_Panel_Middle = null; 
        public static Asset<Texture2D> HP_Panel_Right = null;
        #endregion

        #region Old
        public static Asset<Texture2D> Heart_OG = null;
        public static Asset<Texture2D> Heart2_OG = null;

        public static Asset<Texture2D> Heart_Fill_OG = null;
        public static Asset<Texture2D> Heart_Fill_B_OG = null;
        public static Asset<Texture2D> Heart_Left_OG = null;
        public static Asset<Texture2D> Heart_Middle_OG = null;
        public static Asset<Texture2D> Heart_Right_OG = null;
        public static Asset<Texture2D> Heart_Right_Fancy_OG = null;
        public static Asset<Texture2D> Heart_Single_Fancy_OG = null;

        public static Asset<Texture2D> HP_Fill_OG = null;
        public static Asset<Texture2D> HP_Fill_Honey_OG = null;
        public static Asset<Texture2D> HP_Panel_Middle_OG = null;
        public static Asset<Texture2D> HP_Panel_Right_OG = null;
        #endregion

        #region FieldInfo
        public static FieldInfo Classic_Heart = null;
        public static FieldInfo Classic_Heart2 = null;

        public static FieldInfo FancyClassic_Heart_Fill = null;
        public static FieldInfo FancyClassic_Heart_Fill_B = null;
        public static FieldInfo FancyClassic_Heart_Left = null;
        public static FieldInfo FancyClassic_Heart_Middle = null;
        public static FieldInfo FancyClassic_Heart_Right = null;
        public static FieldInfo FancyClassic_Heart_Right_Fancy = null;
        public static FieldInfo FancyClassic_Heart_Single_Fancy = null;

        public static FieldInfo HorizontalBars_HP_Fill = null;
        public static FieldInfo HorizontalBars_HP_Fill_Honey = null;
        public static FieldInfo HorizontalBars_HP_Panel_Middle = null;
        public static FieldInfo HorizontalBars_HP_Panel_Right = null;
        #endregion

        public override void Load()
        {
            #region Load Assets
            Heart = Request<Texture2D>(FilePath + "Heart");
            Heart2 = Request<Texture2D>(FilePath + "Heart2");
            Heart_OG = TextureAssets.Heart;
            Heart2_OG = TextureAssets.Heart2;

            //TODO: automate this
            string fancyClassic = "FancyClassic/";
            Heart_Left = Request<Texture2D>(FilePath + fancyClassic + "Heart_Left");
            Heart_Middle = Request<Texture2D>(FilePath + fancyClassic + "Heart_Middle");
            Heart_Right = Request<Texture2D>(FilePath + fancyClassic + "Heart_Right");
            Heart_Right_Fancy = Request<Texture2D>(FilePath + fancyClassic + "Heart_Right_Fancy");
            Heart_Fill = Request<Texture2D>(FilePath + fancyClassic + "Heart_Fill");
            Heart_Fill_B = Request<Texture2D>(FilePath + fancyClassic + "Heart_Fill");
            Heart_Single_Fancy = Request<Texture2D>(FilePath + fancyClassic + "Heart_Single_Fancy");
            FancyClassic_Heart_Left = typeof(FancyClassicPlayerResourcesDisplaySet).GetField("_heartLeft", BindingFlags.NonPublic | BindingFlags.Instance);
            FancyClassic_Heart_Middle = typeof(FancyClassicPlayerResourcesDisplaySet).GetField("_heartMiddle", BindingFlags.NonPublic | BindingFlags.Instance);
            FancyClassic_Heart_Right = typeof(FancyClassicPlayerResourcesDisplaySet).GetField("_heartRight", BindingFlags.NonPublic | BindingFlags.Instance);
            FancyClassic_Heart_Right_Fancy = typeof(FancyClassicPlayerResourcesDisplaySet).GetField("_heartRightFancy", BindingFlags.NonPublic | BindingFlags.Instance);
            FancyClassic_Heart_Fill = typeof(FancyClassicPlayerResourcesDisplaySet).GetField("_heartFill", BindingFlags.NonPublic | BindingFlags.Instance);
            FancyClassic_Heart_Fill_B = typeof(FancyClassicPlayerResourcesDisplaySet).GetField("_heartFillHoney", BindingFlags.NonPublic | BindingFlags.Instance);
            FancyClassic_Heart_Single_Fancy = typeof(FancyClassicPlayerResourcesDisplaySet).GetField("_heartSingleFancy", BindingFlags.NonPublic | BindingFlags.Instance);
            Heart_Left_OG = (Asset<Texture2D>)FancyClassic_Heart_Left.GetValue(_sets["New"]);
            Heart_Middle_OG = (Asset<Texture2D>)FancyClassic_Heart_Middle.GetValue(_sets["New"]);
            Heart_Right_OG = (Asset<Texture2D>)FancyClassic_Heart_Right.GetValue(_sets["New"]);
            Heart_Right_Fancy_OG = (Asset<Texture2D>)FancyClassic_Heart_Right_Fancy.GetValue(_sets["New"]);
            Heart_Fill_OG = (Asset<Texture2D>)FancyClassic_Heart_Fill.GetValue(_sets["New"]);
            Heart_Fill_B_OG = (Asset<Texture2D>)FancyClassic_Heart_Fill_B.GetValue(_sets["New"]);
            Heart_Single_Fancy_OG = (Asset<Texture2D>)FancyClassic_Heart_Single_Fancy.GetValue(_sets["New"]);

            string horizontalBars = "HorizontalBars/";
            HP_Fill = Request<Texture2D>(FilePath + horizontalBars + "HP_Fill");
            HP_Fill_Honey = Request<Texture2D>(FilePath + horizontalBars + "HP_Fill");
            HP_Panel_Middle = Request<Texture2D>(FilePath + horizontalBars + "HP_Panel_Middle");
            HP_Panel_Right = Request<Texture2D>(FilePath + horizontalBars + "HP_Panel_Right");
            HorizontalBars_HP_Fill = typeof(HorizontalBarsPlayerResourcesDisplaySet).GetField("_hpFill", BindingFlags.NonPublic | BindingFlags.Instance);
            HorizontalBars_HP_Fill_Honey = typeof(HorizontalBarsPlayerResourcesDisplaySet).GetField("_hpFillHoney", BindingFlags.NonPublic | BindingFlags.Instance);
            HorizontalBars_HP_Panel_Middle = typeof(HorizontalBarsPlayerResourcesDisplaySet).GetField("_panelMiddleHP", BindingFlags.NonPublic | BindingFlags.Instance);
            HorizontalBars_HP_Panel_Right = typeof(HorizontalBarsPlayerResourcesDisplaySet).GetField("_panelRightHP", BindingFlags.NonPublic | BindingFlags.Instance);
            HP_Fill_OG = (Asset<Texture2D>)HorizontalBars_HP_Fill.GetValue(_sets["HorizontalBarsWithFullText"]);
            HP_Fill_Honey_OG = (Asset<Texture2D>)HorizontalBars_HP_Fill_Honey.GetValue(_sets["HorizontalBarsWithFullText"]);
            HP_Panel_Middle_OG = (Asset<Texture2D>)HorizontalBars_HP_Panel_Middle.GetValue(_sets["HorizontalBarsWithFullText"]);
            HP_Panel_Right_OG = (Asset<Texture2D>)HorizontalBars_HP_Panel_Right.GetValue(_sets["HorizontalBarsWithFullText"]);
            #endregion

            #region Hooks
            On_FancyClassicPlayerResourcesDisplaySet.DrawLifeBarText += RedrawText_FancyClassic;
            On_HorizontalBarsPlayerResourcesDisplaySet.DrawLifeBarText += RedrawText_HorizontalBars;
            On_ClassicPlayerResourcesDisplaySet.DrawLife += RedrawText_Classic;
            #endregion
        }

        public override void UpdateUI(GameTime gameTime)
        {
            #region Great and Terrible Evil
            if (Main.LocalPlayer.GetModPlayer<QoCPlayer>().isQoCAwake)
            {
                TextureAssets.Heart = Heart;
                TextureAssets.Heart2 = Heart2;

                FancyClassic_Heart_Right.SetValue(_sets["New"], Heart_Right);
                FancyClassic_Heart_Middle.SetValue(_sets["New"], Heart_Middle);
                FancyClassic_Heart_Left.SetValue(_sets["New"], Heart_Left);
                FancyClassic_Heart_Right_Fancy.SetValue(_sets["New"], Heart_Right_Fancy);
                FancyClassic_Heart_Fill.SetValue(_sets["New"], Heart_Fill);
                FancyClassic_Heart_Fill_B.SetValue(_sets["New"], Heart_Fill);
                FancyClassic_Heart_Single_Fancy.SetValue(_sets["New"], Heart_Single_Fancy);

                //

                HorizontalBars_HP_Fill.SetValue(_sets["HorizontalBarsWithFullText"], HP_Fill);
                HorizontalBars_HP_Fill_Honey.SetValue(_sets["HorizontalBarsWithFullText"], HP_Fill);
                HorizontalBars_HP_Panel_Middle.SetValue(_sets["HorizontalBarsWithFullText"], HP_Panel_Middle);
                HorizontalBars_HP_Panel_Right.SetValue(_sets["HorizontalBarsWithFullText"], HP_Panel_Right);

                HorizontalBars_HP_Fill.SetValue(_sets["HorizontalBarsWithText"], HP_Fill);
                HorizontalBars_HP_Fill_Honey.SetValue(_sets["HorizontalBarsWithText"], HP_Fill);
                HorizontalBars_HP_Panel_Middle.SetValue(_sets["HorizontalBarsWithText"], HP_Panel_Middle);
                HorizontalBars_HP_Panel_Right.SetValue(_sets["HorizontalBarsWithText"], HP_Panel_Right);

                HorizontalBars_HP_Fill.SetValue(_sets["HorizontalBars"], HP_Fill);
                HorizontalBars_HP_Fill_Honey.SetValue(_sets["HorizontalBars"], HP_Fill);
                HorizontalBars_HP_Panel_Middle.SetValue(_sets["HorizontalBars"], HP_Panel_Middle);
                HorizontalBars_HP_Panel_Right.SetValue(_sets["HorizontalBars"], HP_Panel_Right);

                FancyClassic_Heart_Right.SetValue(_sets["NewWithText"], Heart_Right);
                FancyClassic_Heart_Middle.SetValue(_sets["NewWithText"], Heart_Middle);
                FancyClassic_Heart_Left.SetValue(_sets["NewWithText"], Heart_Left);
                FancyClassic_Heart_Right_Fancy.SetValue(_sets["NewWithText"], Heart_Right_Fancy);
                FancyClassic_Heart_Fill.SetValue(_sets["NewWithText"], Heart_Fill);
                FancyClassic_Heart_Fill_B.SetValue(_sets["NewWithText"], Heart_Fill_B);
                FancyClassic_Heart_Single_Fancy.SetValue(_sets["NewWithText"], Heart_Single_Fancy);
            }
            else
            {
                TextureAssets.Heart = Heart_OG;
                TextureAssets.Heart2 = Heart2_OG;

                FancyClassic_Heart_Right.SetValue(_sets["New"], Heart_Right_OG);
                FancyClassic_Heart_Middle.SetValue(_sets["New"], Heart_Middle_OG);
                FancyClassic_Heart_Left.SetValue(_sets["New"], Heart_Left_OG);
                FancyClassic_Heart_Right_Fancy.SetValue(_sets["New"], Heart_Right_Fancy_OG);
                FancyClassic_Heart_Fill.SetValue(_sets["New"], Heart_Fill_OG);
                FancyClassic_Heart_Fill_B.SetValue(_sets["New"], Heart_Fill_OG);
                FancyClassic_Heart_Single_Fancy.SetValue(_sets["New"], Heart_Single_Fancy_OG);

                //

                HorizontalBars_HP_Fill.SetValue(_sets["HorizontalBarsWithFullText"], HP_Fill_OG);
                HorizontalBars_HP_Fill_Honey.SetValue(_sets["HorizontalBarsWithFullText"], HP_Fill_OG);
                HorizontalBars_HP_Panel_Middle.SetValue(_sets["HorizontalBarsWithFullText"], HP_Panel_Middle_OG);
                HorizontalBars_HP_Panel_Right.SetValue(_sets["HorizontalBarsWithFullText"], HP_Panel_Right_OG);

                HorizontalBars_HP_Fill.SetValue(_sets["HorizontalBarsWithText"], HP_Fill_OG);
                HorizontalBars_HP_Fill_Honey.SetValue(_sets["HorizontalBarsWithText"], HP_Fill_OG);
                HorizontalBars_HP_Panel_Middle.SetValue(_sets["HorizontalBarsWithText"], HP_Panel_Middle_OG);
                HorizontalBars_HP_Panel_Right.SetValue(_sets["HorizontalBarsWithText"], HP_Panel_Right_OG);

                HorizontalBars_HP_Fill.SetValue(_sets["HorizontalBars"], HP_Fill_OG);
                HorizontalBars_HP_Fill_Honey.SetValue(_sets["HorizontalBars"], HP_Fill_OG);
                HorizontalBars_HP_Panel_Middle.SetValue(_sets["HorizontalBars"], HP_Panel_Middle_OG);
                HorizontalBars_HP_Panel_Right.SetValue(_sets["HorizontalBars"], HP_Panel_Right_OG);

                FancyClassic_Heart_Right.SetValue(_sets["NewWithText"], Heart_Right_OG);
                FancyClassic_Heart_Middle.SetValue(_sets["NewWithText"], Heart_Middle_OG);
                FancyClassic_Heart_Left.SetValue(_sets["NewWithText"], Heart_Left_OG);
                FancyClassic_Heart_Right_Fancy.SetValue(_sets["NewWithText"], Heart_Right_Fancy_OG);
                FancyClassic_Heart_Fill.SetValue(_sets["NewWithText"], Heart_Fill_OG);
                FancyClassic_Heart_Fill_B.SetValue(_sets["NewWithText"], Heart_Fill_B_OG);
                FancyClassic_Heart_Single_Fancy.SetValue(_sets["NewWithText"], Heart_Single_Fancy_OG);
            }

            setsfield.SetValue(Main.ResourceSetsManager, _sets);
            #endregion
        }

        #region Hook Methods
        private static void RedrawText_FancyClassic(On_FancyClassicPlayerResourcesDisplaySet.orig_DrawLifeBarText orig, SpriteBatch spriteBatch, Vector2 topLeftAnchor)
        {
            if (Main.LocalPlayer.GetModPlayer<QoCPlayer>().isQoCAwake)
            {
                Vector2 vector = topLeftAnchor + new Vector2(130f, -24f);
                Player localPlayer = Main.LocalPlayer;
                Color color = new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor);
                string text = Lang.inter[0].Value + " " + "???" + "/" + localPlayer.statLifeMax2;
                Vector2 vector2 = FontAssets.MouseText.Value.MeasureString(text);
                spriteBatch.DrawString(FontAssets.MouseText.Value, Lang.inter[0].Value, vector + new Vector2((0f - vector2.X) * 0.5f, 0f), color, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(FontAssets.MouseText.Value, "???" + "/" + localPlayer.statLifeMax2, vector + new Vector2(vector2.X * 0.5f, 0f), color, 0f, new Vector2(FontAssets.MouseText.Value.MeasureString("???" + "/" + localPlayer.statLifeMax2).X, 0f), 1f, SpriteEffects.None, 0f);
            }
            else
                orig(spriteBatch, topLeftAnchor);
        }

        private static void RedrawText_HorizontalBars(On_HorizontalBarsPlayerResourcesDisplaySet.orig_DrawLifeBarText orig, SpriteBatch spriteBatch, Vector2 topLeftAnchor)
        {
            if (Main.LocalPlayer.GetModPlayer<QoCPlayer>().isQoCAwake)
            {
                Vector2 vector = topLeftAnchor + new Vector2(130f, -20f);
                Player localPlayer = Main.LocalPlayer;
                Color color = new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor);
                string text = Lang.inter[0].Value + " " + "???" + "/" + localPlayer.statLifeMax2;
                Vector2 vector2 = FontAssets.MouseText.Value.MeasureString(text);
                spriteBatch.DrawString(FontAssets.MouseText.Value, Lang.inter[0].Value, vector + new Vector2((0f - vector2.X) * 0.5f, 0f), color, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(FontAssets.MouseText.Value, "???" + "/" + localPlayer.statLifeMax2, vector + new Vector2(vector2.X * 0.5f, 0f), color, 0f, new Vector2(FontAssets.MouseText.Value.MeasureString("???" + "/" + localPlayer.statLifeMax2).X, 0f), 1f, SpriteEffects.None, 0f);
            }
            else
                orig(spriteBatch, topLeftAnchor);
        }

        // warning: evil
        private static void RedrawText_Classic(On_ClassicPlayerResourcesDisplaySet.orig_DrawLife orig, ClassicPlayerResourcesDisplaySet self)
        {
            if (Main.LocalPlayer.GetModPlayer<QoCPlayer>().isQoCAwake)
            {
                FieldInfo UIDisplay_LifePerHeart = typeof(ClassicPlayerResourcesDisplaySet).GetField("UIDisplay_LifePerHeart", BindingFlags.NonPublic | BindingFlags.Instance);
                FieldInfo UI_ScreenAnchorX = typeof(ClassicPlayerResourcesDisplaySet).GetField("UI_ScreenAnchorX", BindingFlags.NonPublic | BindingFlags.Instance);

                Player localPlayer = Main.LocalPlayer;
                SpriteBatch spriteBatch = Main.spriteBatch;
                Color color = new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor);
                //UIDisplay_LifePerHeart = 20f;
                UIDisplay_LifePerHeart.SetValue(_sets["Default"], 20f);
                PlayerStatsSnapshot snapshot = new PlayerStatsSnapshot(localPlayer);
                if (localPlayer.ghost || localPlayer.statLifeMax2 <= 0 || snapshot.AmountOfLifeHearts <= 0)
                {
                    return;
                }
                //UIDisplay_LifePerHeart = snapshot.LifePerSegment;
                UIDisplay_LifePerHeart.SetValue(_sets["Default"], snapshot.LifePerSegment);
                int num2 = snapshot.LifeFruitCount;
                bool drawText;
                bool drawHearts = ResourceOverlayLoader.PreDrawResourceDisplay(snapshot, self, drawingLife: true, ref color, out drawText);
                if (drawText)
                {
                    //int num3 = (int)((float)localPlayer.statLifeMax2 / UIDisplay_LifePerHeart);
                    int num3 = (int)((float)localPlayer.statLifeMax2 / (float)UIDisplay_LifePerHeart.GetValue(_sets["Default"]));
                    if (num3 >= 10)
                    {
                        num3 = 10;
                    }
                    string text = Lang.inter[0].Value + " " + "???" + "/" + localPlayer.statLifeMax2;
                    Vector2 vector = FontAssets.MouseText.Value.MeasureString(text);
                    if (!localPlayer.ghost)
                    {
                        //spriteBatch.DrawString(FontAssets.MouseText.Value, Lang.inter[0].Value, new Vector2((float)(500 + 13 * num3) - vector.X * 0.5f + (float)UI_ScreenAnchorX, 6f), color, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(FontAssets.MouseText.Value, Lang.inter[0].Value, new Vector2((float)(500 + 13 * num3) - vector.X * 0.5f + (float)(int)UI_ScreenAnchorX.GetValue(_sets["Default"]), 6f), color, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
                        //spriteBatch.DrawString(FontAssets.MouseText.Value, "???" + "/" + localPlayer.statLifeMax2, new Vector2((float)(500 + 13 * num3) + vector.X * 0.5f + (float)UI_ScreenAnchorX, 6f), color, 0f, new Vector2(FontAssets.MouseText.Value.MeasureString(localPlayer.statLife + "/" + localPlayer.statLifeMax2).X, 0f), 1f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(FontAssets.MouseText.Value, "???" + "/" + localPlayer.statLifeMax2, new Vector2((float)(500 + 13 * num3) + vector.X * 0.5f + (float)(int)UI_ScreenAnchorX.GetValue(_sets["Default"]), 6f), color, 0f, new Vector2(FontAssets.MouseText.Value.MeasureString("???" + "/" + localPlayer.statLifeMax2).X, 0f), 1f, SpriteEffects.None, 0f);
                    }
                }
                if (drawHearts)
                {
                    //for (int i = 1; i < (int)((float)localPlayer.statLifeMax2 / UIDisplay_LifePerHeart) + 1; i++)
                    for (int i = 1; i < (int)((float)localPlayer.statLifeMax2 / (float)UIDisplay_LifePerHeart.GetValue(_sets["Default"])) + 1; i++)
                    {
                        int num4 = 255;
                        float num5 = 1f;
                        bool flag = false;
                        //if ((float)localPlayer.statLife >= (float)i * UIDisplay_LifePerHeart)
                        if ((float)localPlayer.statLife >= (float)i * (float)UIDisplay_LifePerHeart.GetValue(_sets["Default"]))
                        {
                            num4 = 255;
                            //if ((float)localPlayer.statLife == (float)i * UIDisplay_LifePerHeart)
                            if ((float)localPlayer.statLife == (float)i * (float)UIDisplay_LifePerHeart.GetValue(_sets["Default"]))
                            {
                                flag = true;
                            }
                        }
                        else
                        {
                            //float num6 = ((float)localPlayer.statLife - (float)(i - 1) * UIDisplay_LifePerHeart) / UIDisplay_LifePerHeart;
                            float num6 = ((float)localPlayer.statLife - (float)(i - 1) * (float)UIDisplay_LifePerHeart.GetValue(_sets["Default"])) / (float)UIDisplay_LifePerHeart.GetValue(_sets["Default"]);
                            num4 = (int)(30f + 225f * num6);
                            if (num4 < 30)
                            {
                                num4 = 30;
                            }
                            num5 = num6 / 4f + 0.75f;
                            if ((double)num5 < 0.75)
                            {
                                num5 = 0.75f;
                            }
                            if (num6 > 0f)
                            {
                                flag = true;
                            }
                        }
                        if (flag)
                        {
                            num5 += Main.cursorScale - 1f;
                        }
                        int num7 = 0;
                        int num8 = 0;
                        if (i > 10)
                        {
                            num7 -= 260;
                            num8 += 26;
                        }
                        int a = (int)((double)num4 * 0.9);
                        if (!localPlayer.ghost)
                        {
                            Asset<Texture2D> heartTexture = ((num2 > 0) ? TextureAssets.Heart2 : TextureAssets.Heart);
                            if (num2 > 0)
                            {
                                num2--;
                            }
                            //Vector2 position = new Vector2(500 + 26 * (i - 1) + num7 + UI_ScreenAnchorX + heartTexture.Width() / 2, 32f + (float)heartTexture.Height() * (1f - num5) / 2f + (float)num8 + (float)(heartTexture.Height() / 2));
                            Vector2 position = new Vector2(500 + 26 * (i - 1) + num7 + (int)UI_ScreenAnchorX.GetValue(_sets["Default"]) + heartTexture.Width() / 2, 32f + (float)heartTexture.Height() * (1f - num5) / 2f + (float)num8 + (float)(heartTexture.Height() / 2));
                            ResourceOverlayDrawContext drawContext = new ResourceOverlayDrawContext(snapshot, self, i - 1, heartTexture);
                            drawContext.position = position;
                            drawContext.color = new Color(num4, num4, num4, a);
                            drawContext.origin = heartTexture.Size() / 2f;
                            drawContext.scale = new Vector2(num5);
                            ResourceOverlayLoader.DrawResource(drawContext);
                        }
                    }
                }
                ResourceOverlayLoader.PostDrawResourceDisplay(snapshot, self, drawingLife: true, color, drawText);
            }
            else
                orig(self);
        }
        #endregion
    }
    #endregion
}