using CalRemix.Content.NPCs.Subworlds.Sealed;
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
                    if (context.Draw(head, n.Center / 16, Alignment.Center).IsMouseOver)
                        text = n.GivenOrTypeName;
                }
            }
        }
    }
}