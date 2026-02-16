using CalRemix.Content.NPCs.Subworlds.Sealed;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Map;
using Terraria.ModLoader;
using Terraria.UI;

namespace CalRemix.Core
{
    public class QuestNPCLayer : ModMapLayer
    {
        public static List<int> validNPCs = new()
        {
            ModContent.NPCType<RubyWarrior>(),
            ModContent.NPCType<BrightMind>(),
            ModContent.NPCType<ShadeGreen>(),
            ModContent.NPCType<DreadonFriendly>(),
            ModContent.NPCType<VigorCloak>()
        };

        public override void Draw(ref MapOverlayDrawContext context, ref string text)
        {
            foreach (NPC n in Main.ActiveNPCs)
            {
                if (validNPCs.Contains(n.type))
                {
                    Texture2D head = ModContent.Request<Texture2D>(NPCLoader.GetNPC(n.type).HeadTexture).Value;
                    //if (context.Draw(head, n.Center / 16, Alignment.Center).IsMouseOver)
                     //   text = n.GivenOrTypeName;


                    Vector2 position = (n.Center / 16 - context.MapPosition) * context.MapScale + context.MapOffset;
                    NPCHeadDrawRenderTargetContent targ = new();
                    targ.SetTexture(head);
                    NPCHeadDrawRenderTargetContent nPCHeadDrawRenderTargetContent = targ;
                    nPCHeadDrawRenderTargetContent.PrepareRenderTarget(Main.graphics.GraphicsDevice, Main.spriteBatch);
                    if (nPCHeadDrawRenderTargetContent.IsReady)
                    {
                        RenderTarget2D target = nPCHeadDrawRenderTargetContent.GetTarget();
                        Main.spriteBatch.Draw(target, position, null, Color.White, 0, target.Size() / 2f, 1, 0, 0f);
                    }
                    else
                    {
                        nPCHeadDrawRenderTargetContent.Request();
                    }

                }
            }
        }
    }
}