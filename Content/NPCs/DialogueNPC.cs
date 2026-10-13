using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using CalRemix.UI;
using System.IO;
using Terraria;

namespace CalRemix.Content.NPCs
{
    public abstract class DialogueNPC : ModNPC
    {
        public virtual bool CanBeTalkedTo => true;

        public bool JustFinishedTalking = false;

        public bool IsTalking = false;

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(IsTalking);
            writer.Write(JustFinishedTalking);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            IsTalking = reader.ReadBoolean();
            JustFinishedTalking = reader.ReadBoolean();
        }

        /// <summary>
        /// Make stuff happen when the NPC is talked to.
        /// </summary>
        /// <param name="key">The current dialogue key being spoken</param>
        public virtual void OnTalk(string key)
        {

        }

        /// <summary>
        /// Determines what dialogue line the NPC will say when talked to
        /// </summary>
        /// <returns>The key for the dialogue line</returns>
        public virtual string GetDialogue()
        {
            return "";
        }

        /// <summary>
        /// Make stuff happen when the NPC is done being talked to.
        /// </summary>
        /// <param name="key">The current dialogue key being spoken</param>
        public virtual void OnEnd(string key)
        {

        }

        /// <summary>
        /// The sound that plays when the NPC is talking. Set to default for no sound.
        /// </summary>
        public virtual SoundStyle TextSound => default;

        /// <summary>
        /// The rate at which the text sound is played
        /// </summary>
        public virtual int TextSpeed => 30;


        public override void AI()
        {
            if (CanBeTalkedTo)
            {
                Rectangle maus = Utils.CenteredRectangle(Main.MouseWorld, Vector2.One * 10);

                if (maus.Intersects(NPC.getRect()))
                {
                    if (Main.LocalPlayer.controlUseTile && Main.LocalPlayer.Remix().talkedNPC == -1 && Main.LocalPlayer.InInteractionRange(maus.X / 16, maus.Y / 16, Terraria.DataStructures.TileReachCheckSettings.Simple))
                    {
                        string key = GetDialogue();
                        OnTalk(key);
                        NPCDialogueUI.StartDialogue(NPC.whoAmI, key);
                        for (int i = 0; i < 50; i++)
                            Main.BestiaryTracker.Kills.RegisterKill(NPC);
                    }
                }
            }
            if (NPCDialogueUI.IsBeingTalkedTo(NPC))
            {
                if (!IsTalking)
                {
                    IsTalking = true;
                    NPC.netUpdate = true;
                }
                if (NPCDialogueUI.NotFinishedTalking(NPC))
                {
                    if (TextSound != default)
                    {
                        if (Main.LocalPlayer.miscCounter % TextSpeed == 0)
                        {
                            SoundEngine.PlaySound(TextSound, NPC.Center);
                        }
                    }
                }
            }
            else
            {
                if (IsTalking)
                {
                    IsTalking = false;
                    JustFinishedTalking = true;
                    NPC.netUpdate = true;
                }
                else
                {
                    if (JustFinishedTalking)
                    {
                        JustFinishedTalking = false;
                        NPC.netUpdate = true;
                    }
                }
            }
        }
    }
}
