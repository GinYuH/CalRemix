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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Threading;
using Terraria;
using Terraria.DataStructures;
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

        public static void LoadQoCMessages()
        {
            HelperMessage.New("QoCTest", "It looks like your armor's broken. You might want to get that checked out.It looks like your armor's broken. You might want to get that checked out.It looks like your armor's broken. You might want to get that checked out.It looks like your armor's broken. You might want to get that checked out.It looks like your armor's broken. You might want to get that checked out.It looks like your armor's broken. You might want to get that checked out.",
                "QueenOfClubsIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<ArmorCrunch>(), cooldown: 3, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.QueenOfClubs);
            HelperMessage.New("QoCTest2", "It",
                "QueenOfClubsIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<Crumbling>(), cooldown: 3, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.QueenOfClubs);
        }

        /// <summary>
        /// Tells the game to wake up the Queen of Clubs. If you want them to be present for dialogue, use this when you call a message
        /// they're involved in!
        /// </summary>
        public static void ForceWakeUpQoC()
        {
            Main.LocalPlayer.GetModPlayer<QoCPlayer>().timeUntilNextQoCAction_Heavy = GetTimeUntilNextHelperStage(QoC_timeAsleepToWaking, QoC_timeAwakeToAsleep);
            Main.LocalPlayer.GetModPlayer<QoCPlayer>().currentQoCMode = (int)QoCPlayer.QoCState.Awake_Idle;
        }

        //TODO: make fully modular, move to a more general area, update flux accordingly
        /// <summary>
        /// Returns a new randomly-generated value for the Queen of Clubs to use as a timer before the next major Queen of Clubs stage.
        /// This is used to determine when she's awake or asleep.
        /// </summary>
        /// <param name="timeAsleepToAwake"> The time to return if switching a helper from asleep to awake, in minutes. </param>
        /// <param name="timeAwakeToAsleep"> The time to return if switching a helper from awake to asleep, in minutes. </param>
        /// <param name="noiseSubtract"> The maximum amount of time which could be subtracted from the end result. Defaults to 3 minutes. </param>
        /// <param name="noiseAdd"> The maximum amount of time which could be added to the end result. Defaults to 3 minutes. </param>
        public static int GetTimeUntilNextHelperStage(float timeAsleepToAwake, float timeAwakeToAsleep, float noiseSubtract = 3, float noiseAdd = 3)
        {
            float timeToReturn = 0;

            switch (Main.LocalPlayer.GetModPlayer<QoCPlayer>().currentQoCMode)
            {
                case (int)QoCPlayer.QoCState.Asleep:
                    timeToReturn = timeAsleepToAwake;
                    break;
                case (int)QoCPlayer.QoCState.Awake_Idle:
                    timeToReturn = timeAwakeToAsleep;
                    break;
            }

            // turn int into minutes
            timeToReturn *= (float)Math.Pow(60, 2);
            // add layer of noise
            int noiseSubtractFrames = (int)(-noiseSubtract * Math.Pow(60, 2));
            int noiseAddFrames = (int)(noiseAdd * Math.Pow(60, 2) + 1);
            timeToReturn += Main.rand.Next(noiseSubtractFrames, noiseAddFrames);

            return (int)timeToReturn;
        }
    }

    public class QoCPlayer : ModPlayer
    {
        public enum QoCState
        {
            Asleep = 0,
            Awake_Idle = 1,
            Awake_TurnLoop = 2
        }
        /// <summary>
        /// The length between the Queen of Clubs' next major action. This is used as a timer between being awake or asleep. 
        /// </summary>
        public int timeUntilNextQoCAction_Heavy;
        /// <summary>
        /// The length between the Queen of Clubs' next minor action. This is used as a timer between being flipping, not flipping, ect.
        /// </summary>
        public int? timeUntilNextQoCAction_Light;
        /// <summary>
        /// The current action the Queen of Clubs is performing
        /// </summary>
        public int currentQoCMode = (int)QoCState.Asleep;

        public bool isQoCAwake => currentQoCMode >= (int)QoCState.Awake_Idle;
        public bool isQoCUnlocked => Player.GetModPlayer<CalRemixPlayer>().fifteenMinutesSinceHardmode <= 0;

        public override void PreUpdate()
        {
            if (isQoCUnlocked)
            {
                #region Minor Action Timer
                if (isQoCAwake && timeUntilNextQoCAction_Light <= 0)
                {

                }
                timeUntilNextQoCAction_Light--;
                #endregion

                #region Awake/Asleep Timer
                if (timeUntilNextQoCAction_Heavy <= 0)
                {
                    if (currentQoCMode >= (int)QoCState.Awake_Idle)
                    {
                        // if awake, go to sleep
                        timeUntilNextQoCAction_Heavy = ScreenHelperManager.GetTimeUntilNextHelperStage(ScreenHelperManager.QoC_timeAsleepToWaking, ScreenHelperManager.QoC_timeAwakeToAsleep);
                        currentQoCMode = (int)QoCState.Asleep;
                    }
                    else
                    {
                        // if asleep, start waking up
                        timeUntilNextQoCAction_Heavy = ScreenHelperManager.GetTimeUntilNextHelperStage(ScreenHelperManager.QoC_timeAsleepToWaking, ScreenHelperManager.QoC_timeAwakeToAsleep);
                        currentQoCMode = (int)QoCState.Awake_Idle;
                    }
                }
                #endregion
            }

            timeUntilNextQoCAction_Heavy--;
        }

        #region Data Save/Load
        public override void SaveData(TagCompound tag)
        {
            tag["TimeUntilNextQoCAction_Heavy"] = timeUntilNextQoCAction_Heavy;
            tag["TimeUntilNextQoCAction_Light"] = timeUntilNextQoCAction_Light;
            tag["CurrentQoCMode"] = currentQoCMode;
        }

        public override void LoadData(TagCompound tag)
        {
            timeUntilNextQoCAction_Heavy = tag.GetInt("TimeUntilNextQoCAction_Heavy");
            timeUntilNextQoCAction_Light = tag.GetInt("TimeUntilNextQoCAction_Light");
            currentQoCMode = tag.GetInt("CurrentQoCMode");
        }
        #endregion
    }

    public class QueenOfClubsCard : UIElement
    {
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Texture2D testSprite = ModContent.Request<Texture2D>("CalRemix/UI/Fanny/HelperQueenOfClubsIdle").Value;
            if ((Main.GlobalTimeWrappedHourly + MathHelper.PiOver2) % MathHelper.TwoPi >= Math.PI)
                testSprite = ModContent.Request<Texture2D>("CalRemix/UI/Fanny/HelperQueenOfClubsBack").Value;
            Rectangle testFrame = testSprite.Frame(1, 1, 0, 0);

            Matrix rotation = Matrix.CreateRotationY(Main.GlobalTimeWrappedHourly) * Matrix.CreateRotationZ((float)Math.Sin(Main.GlobalTimeWrappedHourly * 0.5f) * 0.1f);
            Matrix translation = Matrix.CreateTranslation(new Vector3(Main.screenWidth - 175, 100, 0));
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
                effect.Parameters["opacity"].SetValue(Main.LocalPlayer.GetModPlayer<QoCPlayer>().isQoCAwake.ToInt());
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
            QueenOfClubs = new QueenOfClubsCard();
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