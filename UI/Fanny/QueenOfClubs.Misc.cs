using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatBuffs;
using CalamityMod.Buffs.StatDebuffs;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Threading;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using static Terraria.Graphics.Effects.Filters;

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
        public enum QoCState
        {
            Asleep = 0,
            Awake_Idle = 1,
            Awake_TurnLoop = 2,
            Awake_TurnLoopReverse = 3,
            Awake_TurnOnce = 4
        }
        public enum QoCFace
        {
            Idle = 0,
            Test1 = 1,
            Test2 = 2
        }
        public Dictionary<QoCFace, string> QoCFaceDict = new Dictionary<QoCFace, string>
        {
            [QoCFace.Idle] = "Idle",
            [QoCFace.Test1] = "Test1",
            [QoCFace.Test2] = "Test2"
        };

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
                        currentFaceBuffer = (QoCFace)Main.rand.Next(1, QoCFaceDict.Count);

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
                        currentQoCState = Main.rand.Next(1, 3 + 1);
                        // reset face
                        timeUntilNextQoCAction_Light = HelperHelpers.GetTimeUntilNextStage(ScreenHelperManager.QoC_timeNextFaceIdleBaseline, ScreenHelperManager.QoC_timeNextFaceIdleNoiseMin, ScreenHelperManager.QoC_timeNextFaceIdleNoiseMax);
                        currentFace = QoCFace.Idle;
                        // add a big spin on wake-up
                        rateOfSpinExtra = 0.25f * spinReverse;
                        // fuck it add some bounce too
                        bounce = 1;
                        // make some noise 
                        SoundEngine.PlaySound(new SoundStyle("CalRemix/Assets/Sounds/Helpers/QueenOfClubsLaugh") with { MaxInstances = 1 }, Main.LocalPlayer.position);
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
                SoundEngine.PlaySound(new SoundStyle("CalRemix/Assets/Sounds/Helpers/QueenOfClubsLaugh") with { PitchRange = (-0.75f, 0.75f), MaxInstances = 1 }, Main.LocalPlayer.position);
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = GetDimensions();
            Rectangle dimensionsRect = dimensions.ToRectangle();
            //Utils.DrawInvBG(spriteBatch, dimensionsRect);

            Texture2D testSprite = ModContent.Request<Texture2D>("CalRemix/UI/Fanny/HelperQueenOfClubs" + Main.LocalPlayer.GetModPlayer<QoCPlayer>().QoCFaceDict[Main.LocalPlayer.GetModPlayer<QoCPlayer>().currentFace]).Value;
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
}