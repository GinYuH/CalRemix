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

    private static Mod?  theMod;
    private static bool  isCurrentlyHandlingOurMod;
    private static float hoverProgress;

    private static readonly List<DrawCall> pending_calls = [];

    public static void Load(Mod mod)
    {
        theMod = mod;

        var uiModItem = typeof(UIModItem);

        MonoModHooks.Add(
            GetMethod(nameof(UIModItem.Draw)),
            Draw
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

        hoverProgress += (self.IsMouseHovering ? 1f : -1f) / 45f;
        hoverProgress =  Math.Clamp(hoverProgress, 0f, 1f);

        orig(self, spriteBatch);

        var modIconDims = uiModItem._modIcon.GetDimensions();

        const int fanny_frames = 8;

        var fannyImage = theMod.Assets.Request<Texture2D>("UI/Fanny/HelperFannyIdleAdjustedForModList");
        var fannyFrame = fannyImage.Frame(
            1,
            fanny_frames,
            frameY: (int)(Main.GlobalTimeWrappedHourly * 7f % fanny_frames)
        );

        var fannyPosition = modIconDims.Position();
        fannyPosition.X += modIconDims.Width  / 2f;
        fannyPosition.Y += modIconDims.Height / 3f;

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
                clipRectangle
            )
        );
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