using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        Texture2D     Texture,
        Rectangle     SourceRectangle,
        Vector2       Position,
        Color         Color,
        float         Rotation,
        Vector2       Origin,
        Vector2       Scale,
        SpriteEffects Effects,
        Rectangle?    ClipRectangle
    );

    private readonly record struct FannyTexture(
        string Texture,
        int    FrameCount,
        float  SpeedMultiplier
    );

    private enum FannyState
    {
        Idle,
        Sob,
        Stare,
        Awe,
    }

    private static readonly FannyTexture fanny_idle = new(
        Texture: "UI/Fanny/ModList_FannyIdle",
        FrameCount: 13,
        SpeedMultiplier: 7f
    );

    private static readonly FannyTexture fanny_cry = new(
        Texture: "UI/Fanny/ModList_FannyCry",
        FrameCount: 2,
        SpeedMultiplier: 3.5f
    );

    private static readonly FannyTexture fanny_stare = new(
        Texture: "UI/Fanny/ModList_FannyStare",
        FrameCount: 1,
        SpeedMultiplier: 0f
    );

    private static readonly FannyTexture fanny_awe = new(
        Texture: "UI/Fanny/ModList_FannyAwe",
        FrameCount: 4,
        SpeedMultiplier: 5f
    );

    private static FannyState fannyState = FannyState.Idle;

    private static Mod?  theMod;
    private static bool  isCurrentlyHandlingOurMod;
    private static float hoverProgress;

    private static bool enabledWhenHoveredOver;

    private static readonly List<DrawCall> pending_calls = [];

    public static void Load(Mod mod)
    {
        theMod = mod;

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

        return;

        static MethodInfo GetMethod(string name)
        {
            return typeof(UIModItem).GetMethod(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)!;
        }
    }

    private static void Draw(
        Action<UIModItem, SpriteBatch> orig,
        UIModItem                      self,
        SpriteBatch                    spriteBatch
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
        UIModItem         self
    )
    {
        Debug.Assert(theMod is not null);

        if (self._mod.Name != theMod.Name)
        {
            orig(self);
            return;
        }

        orig(self);

        // pre-request assets
        theMod.Assets.Request<Texture2D>(fanny_idle.Texture);
        theMod.Assets.Request<Texture2D>(fanny_cry.Texture);
        theMod.Assets.Request<Texture2D>(fanny_stare.Texture);
        theMod.Assets.Request<Texture2D>(fanny_awe.Texture);

        self._uiModStateText.OnMouseOver += (e, _) =>
        {
            enabledWhenHoveredOver = self._uiModStateText._enabled;

            if (self._uiModStateText._enabled)
            {
                fannyState = FannyState.Sob;
            }
            else
            {
                fannyState = FannyState.Awe;
            }
        };

        self._uiModStateText.OnMouseOut += (e, _) =>
        {
            fannyState = self._uiModStateText._enabled ? FannyState.Idle : FannyState.Stare;
        };

        self._uiModStateText.OnLeftClick += (e, _) =>
        {
            var nowEnabled = self._uiModStateText._enabled;

            if (!nowEnabled && enabledWhenHoveredOver)
            {
                fannyState = FannyState.Stare;
            }
            else if (nowEnabled && !enabledWhenHoveredOver)
            {
                fannyState = FannyState.Idle;
            }
            /*else
            {
                fannyState = FannyState.Sob;
            }*/

            enabledWhenHoveredOver = nowEnabled;
        };
    }

    private static void OverrideRegularPanelDrawing(
        Action<UIPanel, SpriteBatch> orig,
        UIPanel                      self,
        SpriteBatch                  spriteBatch
    )
    {
        Debug.Assert(theMod is not null);

        if (!isCurrentlyHandlingOurMod || self is not UIModItem uiModItem)
        {
            orig(self, spriteBatch);
            return;
        }

        var fannyTexture = GetFannyTexture(uiModItem);

        hoverProgress += (self.IsMouseHovering || fannyState == FannyState.Stare ? 1f : -1f) / 45f;
        hoverProgress =  Math.Clamp(hoverProgress, 0f, 1f);

        orig(self, spriteBatch);

        var modIconDims = uiModItem._modIcon.GetDimensions();

        var fannyImage = theMod.Assets.Request<Texture2D>(fannyTexture.Texture);
        var fannyFrame = fannyImage.Frame(
            1,
            fannyTexture.FrameCount,
            frameY: (int)(Main.GlobalTimeWrappedHourly * fannyTexture.SpeedMultiplier % fannyTexture.FrameCount)
        );

        var fannyPosition = modIconDims.Position();
        fannyPosition.X += modIconDims.Width / 2f;
        fannyPosition.Y += 2f;
        // fannyPosition.Y += modIconDims.Height / 3f;

        var fannyScale = Ease(hoverProgress);

        var fannyOrigin = fannyFrame.Size() / 2f;
        fannyOrigin.Y += fannyFrame.Height / 2f;

        var clipRectangle = new Rectangle(
            0,
            0,
            Main.screenWidth,
            (int)modIconDims.Y
        );

        pending_calls.Add(
            new DrawCall(
                fannyImage.Value,
                fannyFrame,
                fannyPosition,
                Color.White,
                0f,
                fannyOrigin,
                new Vector2(fannyScale),
                SpriteEffects.None,
                // clipRectangle
                null
            )
        );

        return;

        static FannyTexture GetFannyTexture(UIModItem ui)
        {
            return fannyState switch
            {
                FannyState.Idle  => fanny_idle,
                FannyState.Sob   => fanny_cry,
                FannyState.Stare => fanny_stare,
                FannyState.Awe   => fanny_awe,
                _                => fanny_idle,
            };
        }
    }

    private static void UserInterfaceDraw(
        UserInterface ui,
        SpriteBatch   spriteBatch,
        GameTime      gameTime
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