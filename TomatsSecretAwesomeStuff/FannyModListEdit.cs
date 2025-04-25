using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoMod.Cil;

using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace CalRemix;

public static class FannyModListEdit
{
    private readonly record struct DrawCall(
        Texture2D Texture,
        Rectangle SourceRectangle,
        Vector2 Position,
        Color Color,
        float Rotation,
        Vector2 Origin,
        Vector2 Scale,
        SpriteEffects Effects,
        Rectangle? ClipRectangle
    );

    private readonly record struct FrameTime(
        int Fps,
        int[] Frames
    )
    {
        private readonly int totalFrames = Frames.Sum();

        public int GetFrame()
        {
            var totalTimeInSeconds = Main.GlobalTimeWrappedHourly;
            var totalTimeInFrames = (int)(totalTimeInSeconds * Fps);
            var currentFrame = totalTimeInFrames % totalFrames;

            var frame = 0;
            var frameTime = 0;

            for (var i = 0; i < Frames.Length; i++)
            {
                frameTime += Frames[i];

                if (currentFrame >= frameTime)
                {
                    continue;
                }

                frame = i;
                break;
            }

            return frame;
        }
    }

    private readonly record struct HelperTexture(
        string Texture,
        FrameTime Timing
    );

    private readonly record struct Helper(
        HelperTexture Idle,
        HelperTexture Sob,
        HelperTexture Stare,
        HelperTexture Awe
    );

    private static readonly Dictionary<string, Helper> helpers = new()
    {
        {
            "Fanny", new Helper(
                Idle: new HelperTexture(
                    Texture: "UI/Fanny/ModList_Fanny_Idle",
                    Timing: new FrameTime(Fps: 6, [10, 1, 1, 1])
                ),
                Sob: new HelperTexture(
                    Texture: "UI/Fanny/ModList_Fanny_Cry",
                    Timing: new FrameTime(Fps: 6, [1, 1])
                ),
                Stare: new HelperTexture(
                    Texture: "UI/Fanny/ModList_Fanny_Stare",
                    Timing: new FrameTime(Fps: 6, [1])
                ),
                Awe: new HelperTexture(
                    Texture: "UI/Fanny/ModList_Fanny_Awe",
                    Timing: new FrameTime(Fps: 6, [10, 1, 1, 1])
                )
            )
        },
        {
            "EvilFanny", new Helper(
                Idle: new HelperTexture(
                    Texture: "UI/Fanny/ModList_EvilFanny_Idle",
                    Timing: new FrameTime(Fps: 6, [10, 1, 1, 1])
                ),
                Sob: new HelperTexture(
                    Texture: "UI/Fanny/ModList_EvilFanny_Cry",
                    Timing: new FrameTime(Fps: 6, [1, 1])
                ),
                Stare: new HelperTexture(
                    Texture: "UI/Fanny/ModList_EvilFanny_Stare",
                    Timing: new FrameTime(Fps: 6, [1])
                ),
                Awe: new HelperTexture(
                    Texture: "UI/Fanny/ModList_EvilFanny_Awe",
                    Timing: new FrameTime(Fps: 6, [10, 1, 1, 1])
                )
            )
        },
        {
            "TrapperBulbChan", new Helper(
                Idle: new HelperTexture(
                    Texture: "UI/Fanny/ModList_TrapperBulbChan_Idle",
                    Timing: new FrameTime(Fps: 6, [10, 1])
                ),
                Sob: new HelperTexture(
                    Texture: "UI/Fanny/ModList_TrapperBulbChan_Cry",
                    Timing: new FrameTime(Fps: 6, [1, 1])
                ),
                Stare: new HelperTexture(
                    Texture: "UI/Fanny/ModList_TrapperBulbChan_Stare",
                    Timing: new FrameTime(Fps: 6, [1])
                ),
                Awe: new HelperTexture(
                    Texture: "UI/Fanny/ModList_TrapperBulbChan_Awe",
                    Timing: new FrameTime(Fps: 6, [10, 1])
                )
            )
        },
    };

    private enum HelperState
    {
        Idle,
        Sob,
        Stare,
        Awe,
    }

    private static string? currentHelper;

    private static Helper? CurrentHelper
    {
        get
        {
            if (currentHelper is null)
            {
                return null;
            }

            return helpers.TryGetValue(currentHelper, out var helper) ? helper : null;
        }
    }

    private static HelperState helperState = HelperState.Idle;

    private static Mod? theMod;
    private static bool isCurrentlyHandlingOurMod;
    private static float hoverProgress;

    private static bool enabledWhenHoveredOver;

    private static readonly List<DrawCall> pending_calls = [];

    private static Func<List<string>>? theHelpersProvider;

    public static void Load(Mod mod, Func<List<string>> helpersProvider)
    {
        theMod = mod;

        theHelpersProvider = helpersProvider;

        MonoModHooks.Add(
            GetMethod(nameof(UIModItem.Draw)),
            Draw
        );

        MonoModHooks.Add(
            GetMethod(nameof(UIModItem.OnInitialize)),
            OnInitialize
        );

        MonoModHooks.Add(
            typeof(UIPanel).GetMethod("DrawSelf", BindingFlags.NonPublic | BindingFlags.Instance),
            OverrideRegularPanelDrawing
        );

        IL_Main.DrawMenu += il =>
        {
            var c = new ILCursor(il);

            c.GotoNext(MoveType.Before, x => x.MatchCallvirt<UserInterface>(nameof(UserInterface.Draw)));
            c.Remove();
            c.EmitDelegate(UserInterfaceDraw);
        };

        foreach (var (_, helper) in helpers)
        {
            theMod.Assets.Request<Texture2D>(helper.Awe.Texture);
            theMod.Assets.Request<Texture2D>(helper.Idle.Texture);
            theMod.Assets.Request<Texture2D>(helper.Stare.Texture);
            theMod.Assets.Request<Texture2D>(helper.Sob.Texture);
        }

        return;

        static MethodInfo GetMethod(string name)
        {
            return typeof(UIModItem).GetMethod(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)!;
        }
    }

    private static void Draw(
        Action<UIModItem, SpriteBatch> orig,
        UIModItem self,
        SpriteBatch spriteBatch
    )
    {
        Debug.Assert(theMod is not null);

        if (self._mod.Name != theMod.Name)
        {
            orig(self, spriteBatch);
            return;
        }

        isCurrentlyHandlingOurMod = true;
        orig(self, spriteBatch);
        isCurrentlyHandlingOurMod = false;
    }

    private static void OnInitialize(
        Action<UIModItem> orig,
        UIModItem self
    )
    {
        Debug.Assert(theMod is not null);

        if (self._mod.Name != theMod.Name)
        {
            orig(self);
            return;
        }

        orig(self);

        var verifiedHelpers = (theHelpersProvider?.Invoke() ?? [])
                             .Where(x => helpers.ContainsKey(x))
                             .ToArray();

        if (verifiedHelpers.Length > 0)
        {
            currentHelper = verifiedHelpers[Main.rand.Next(verifiedHelpers.Length)];
        }

        self._uiModStateText.OnMouseOver += (_, _) =>
        {
            enabledWhenHoveredOver = self._uiModStateText._enabled;

            helperState = self._uiModStateText._enabled ? HelperState.Sob : HelperState.Awe;
        };

        self._uiModStateText.OnMouseOut += (_, _) =>
        {
            helperState = self._uiModStateText._enabled ? HelperState.Idle : HelperState.Stare;
        };

        self._uiModStateText.OnLeftClick += (_, _) =>
        {
            var nowEnabled = self._uiModStateText._enabled;

            if (!nowEnabled && enabledWhenHoveredOver)
            {
                helperState = HelperState.Stare;
            }
            else if (nowEnabled && !enabledWhenHoveredOver)
            {
                helperState = HelperState.Idle;
            }

            enabledWhenHoveredOver = nowEnabled;
        };
    }

    private static void OverrideRegularPanelDrawing(
        Action<UIPanel, SpriteBatch> orig,
        UIPanel self,
        SpriteBatch spriteBatch
    )
    {
        Debug.Assert(theMod is not null);

        if (!isCurrentlyHandlingOurMod || self is not UIModItem uiModItem)
        {
            orig(self, spriteBatch);
            return;
        }

        orig(self, spriteBatch);

        if (!CurrentHelper.HasValue)
        {
            return;
        }

        var helperTexture = GetHelperTexture(CurrentHelper.Value);

        hoverProgress += (self.IsMouseHovering || helperState == HelperState.Stare ? 1f : -1f) / 45f;
        hoverProgress = Math.Clamp(hoverProgress, 0f, 1f);

        var modIconDims = uiModItem._modIcon.GetDimensions();

        var helperImage = theMod.Assets.Request<Texture2D>(helperTexture.Texture);
        var helperFrame = helperImage.Frame(
            1,
            helperTexture.Timing.Frames.Length,
            frameY: helperTexture.Timing.GetFrame()
        );

        var helperPosition = modIconDims.Position();
        helperPosition.X += modIconDims.Width / 2f;
        helperPosition.Y += 2f;
        // helperPosition.Y += modIconDims.Height / 3f;

        var helperScale = Ease(hoverProgress);

        var helperOrigin = helperFrame.Size() / 2f;
        helperOrigin.Y += helperFrame.Height / 2f;

        pending_calls.Add(
            new DrawCall(
                helperImage.Value,
                helperFrame,
                helperPosition,
                Color.White,
                0f,
                helperOrigin,
                new Vector2(helperScale),
                SpriteEffects.None,
                // clipRectangle
                null
            )
        );

        return;

        static HelperTexture GetHelperTexture(Helper helper)
        {
            return helperState switch
            {
                HelperState.Idle => helper.Idle,
                HelperState.Sob => helper.Sob,
                HelperState.Stare => helper.Stare,
                HelperState.Awe => helper.Awe,
                _ => helper.Idle,
            };
        }
    }

    private static void UserInterfaceDraw(
        UserInterface ui,
        SpriteBatch spriteBatch,
        GameTime gameTime
    )
    {
        ui.Draw(spriteBatch, gameTime);

        foreach (var call in pending_calls)
        {
            if (call.ClipRectangle.HasValue)
            {
                spriteBatch.End(out var snapshot);
                spriteBatch.GraphicsDevice.ScissorRectangle = call.ClipRectangle.Value;
                spriteBatch.Begin(
                    snapshot with
                    {
                        RasterizerState = new RasterizerState { ScissorTestEnable = true },
                    }
                );

                spriteBatch.Draw(
                    call.Texture,
                    call.Position,
                    call.SourceRectangle,
                    call.Color,
                    call.Rotation,
                    call.Origin,
                    call.Scale,
                    call.Effects,
                    0f
                );

                spriteBatch.End();
                spriteBatch.GraphicsDevice.ScissorRectangle = Rectangle.Empty;
                spriteBatch.Begin(in snapshot);
            }
            else
            {
                spriteBatch.Draw(
                    call.Texture,
                    call.Position,
                    call.SourceRectangle,
                    call.Color,
                    call.Rotation,
                    call.Origin,
                    call.Scale,
                    call.Effects,
                    0f
                );
            }
        }

        pending_calls.Clear();
    }

    private static float Ease(float progress)
    {
        /*const float c4 = 2f * MathF.PI / 3f;

        return progress switch
        {
            <= 0f => 0f,
            >= 1f => 1f,
            _     => MathF.Pow(2f, -10f * progress) + MathF.Sin((progress * 10f - 0.75f) * c4) + 1f,
        };*/

        const float c1 = 1.70158f;
        const float c3 = c1 + 1f;

        return 1f + c3 * MathF.Pow(progress - 1f, 3f) + c1 * MathF.Pow(progress - 1f, 2f);
    }
}