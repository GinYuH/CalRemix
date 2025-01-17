using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using static CalRemix.UI.Games.TrapperQuest.TQHandler;

namespace CalRemix.UI.Games.TrapperQuest
{
    /// <summary>
    /// Dialogue
    /// </summary>
    public class TQMessage
    {
        public string Text;

        public int Speaker;

        public bool MessageCompleted => TQDialogueSystem.DialogueTimer >= Text.Length;

        public string Child = null;

        public TQMessage(string text, int speaker = 0, string child = null)
        {
            Text = text;
            Speaker = speaker;
            Child = child;
        }

        public static void PlayMessage(TQMessage message)
        {
            TQDialogueSystem.DialogueTimer = 0;
            TQDialogueSystem.ActiveMessage = message;
        }
        public static void PlayMessage(string key)
        {
            TQMessage message = TQDialogueSystem.messages[key];
            TQDialogueSystem.DialogueTimer = 0;
            TQDialogueSystem.ActiveMessage = message;
        }
    }

    public class TQDialogueSystem : ModSystem
    {
        public static Dictionary<string, TQMessage> messages = new Dictionary<string, TQMessage>();

        public static bool DialogueActive => ActiveMessage != null;

        public static int DialogueTimer = 0;

        public static TQMessage ActiveMessage;

        public override void Load()
        {
            messages.Add("Morning", new TQMessage("What a wonderful day in the world of Calamity", 1, "Rock"));
            messages.Add("Rock", new TQMessage("A simple rock", 0));
        }

        public override void PostUpdateEverything()
        {
            if (DialogueActive)
            {
                DialogueTimer++;
                // Progress dialogue when clicking
                if (Main.mouseLeftRelease && Main.mouseLeft)
                {
                    // If a message is done, check if the message has a child message
                    // If it does, play that message, otherwise quit
                    if (ActiveMessage.MessageCompleted)
                    {
                        if (ActiveMessage.Child != null)
                        {
                            ActiveMessage = messages[ActiveMessage.Child];
                        }
                        else
                        {
                            ActiveMessage = null;
                        }
                        DialogueTimer = 0;
                    }
                    // If a message isn't done, make it instantly complete
                    else
                    {
                        DialogueTimer = ActiveMessage.Text.Length;
                    }
                }
            }
            else
                DialogueTimer = 0;
        }

        public static void DrawMessage(SpriteBatch sb, Vector2 position, Rectangle rect)
        {
            Texture2D box = ModContent.Request<Texture2D>("CalRemix/UI/Games/TrapperQuest/DialogueBox").Value;
            if (DialogueActive)
            {
                float scale = rect.Width / (float)box.Width * 0.9f;
                Vector2 finalPos = position + Vector2.UnitX * (rect.Width - box.Width * scale) / 2 + Vector2.UnitY * 20;
                sb.Draw(box, finalPos, null, Color.White * 0.8f, 0, Vector2.Zero, new Vector2(scale, scale * 0.9f), 0, 0);
                string finalText = ActiveMessage.Text.Remove((int)MathHelper.Min(DialogueTimer, ActiveMessage.Text.Length));
                Vector2 textPos = finalPos + new Vector2(170, 32);
                Utils.DrawBorderString(sb, finalText, textPos, Color.White);
            }
        }
    }
}