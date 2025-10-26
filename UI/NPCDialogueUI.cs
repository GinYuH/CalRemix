using CalamityMod;
using CalRemix.Content.Items.ZAccessories;
using CalRemix.Content.NPCs.Subworlds.Sealed;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace CalRemix.UI
{
    public class NPCDialogueUI : UIState
    {
        public static string currentKey = "";
        public static int currentIndex = -1;

        public static int currentMaxCharacters => NPCDIalogueUISystem.allDialogue[Main.npc[Main.LocalPlayer.Remix().talkedNPC].type].dialogue[currentKey][currentIndex].Length;
        public static int currentCharacter = 0;
        public static float CurrentCompletion => currentCharacter / (float)currentMaxCharacters;

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Main.LocalPlayer.Remix().talkedNPC > -1)
            {
                NPC talkedNPC = Main.npc[Main.LocalPlayer.Remix().talkedNPC];
                if (talkedNPC.life < 0 || !talkedNPC.active || !NPCDIalogueUISystem.allDialogue.ContainsKey(talkedNPC.type))
                {
                    ResetDialogue();
                }
                else
                {
                    NPCDialogueSet dialogueInfo = NPCDIalogueUISystem.allDialogue[talkedNPC.type];
                    if (currentKey == "" || currentIndex == -1)
                    {
                        ResetDialogue();
                    }
                    else
                    {
                        string currentLine = dialogueInfo.dialogue[currentKey][currentIndex];


                        string txt = currentLine;
                        if (currentCharacter < currentMaxCharacters)
                            txt = currentLine.Substring(0, currentCharacter);

                        int textWidth = (int)FontAssets.MouseText.Value.MeasureString(currentLine).X;
                        int textHeight = (int)FontAssets.MouseText.Value.MeasureString(currentLine).Y;

                        Rectangle rect = new Rectangle(0, 0, textWidth + 16, textHeight + 16);
                        Rectangle rect2 = new Rectangle(0, 0, textWidth + 24, textHeight + 24);

                        Vector2 boxPos = talkedNPC.Top - Main.screenPosition - Vector2.UnitY * rect2.Height;

                        spriteBatch.Draw(TextureAssets.MagicPixel.Value, boxPos, rect2, dialogueInfo.borderColor, 0, new Vector2(rect2.Width / 2f, rect2.Height / 2f), 1, 0, 0);
                        spriteBatch.Draw(TextureAssets.MagicPixel.Value, boxPos, rect, dialogueInfo.bgColor, 0, new Vector2(rect.Width / 2f, rect.Height / 2f), 1, 0, 0);
                        Utils.DrawBorderString(spriteBatch, txt, boxPos - new Vector2(textWidth, textHeight) * 0.5f, Color.White);

                        if (Main.LocalPlayer.miscCounter % 3 == 0)
                            currentCharacter++;

                        if (Main.mouseLeft && Main.mouseLeftRelease)
                        {
                            if (CurrentCompletion < 1f)
                            {
                                currentCharacter = currentMaxCharacters;
                            }
                            else
                            {
                                currentIndex++;
                                currentCharacter = 0;
                                if (currentIndex > dialogueInfo.dialogue[currentKey].Count - 1)
                                {
                                    ResetDialogue();
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                ResetDialogue();
            }
        }

        public static void StartDialogue(int npcIndex, string key)
        {
            currentKey = key;
            Main.LocalPlayer.Remix().talkedNPC = npcIndex;
            currentCharacter = 0;
            currentIndex = 0;
        }

        public static void ResetDialogue()
        {
            currentCharacter = -1;
            currentIndex = -1;
            currentKey = "";
            Main.LocalPlayer.Remix().talkedNPC = -1;
        }

        public static bool IsTalking(NPC npc)
        {
            if (currentKey == "")
                return false;
            if (currentMaxCharacters > 0 && currentCharacter >= currentMaxCharacters)
            {
                return false;
            }
            return currentCharacter >= 0;
        }

        public static bool IsBeingTalkedTo(NPC npc)
        {
            if (currentKey == "")
                return false;
            return true;
        }
    }

    [Autoload(Side = ModSide.Client)]
    internal class NPCDIalogueUISystem : ModSystem
    {
        private UserInterface UserInter;

        internal NPCDialogueUI SUI;

        public static Dictionary<int, NPCDialogueSet> allDialogue = new();

        public override void Load()
        {
            SUI = new();
            UserInter = new();
            UserInter.SetState(SUI);

            NPCDialogueSet brightMindDialogue = new NPCDialogueSet(
            new() { { "Intro", new()
            {
                "OOOOHOOO! bobobobobo!",
                "Babonya! bo oooaaa oaoaa!",
                "...",
                "FETCH ME !   THE    SHARDS !\nMonorian Gem Shards !!!!"
            }
            } }, Color.DarkGoldenrod, Color.LightGoldenrodYellow, "CalRemix/Content/NPCs/Subworlds/Sealed/BrightMind_Head");

            allDialogue.Add(ModContent.NPCType<BrightMind>(), brightMindDialogue);

        }

        public override void UpdateUI(GameTime gameTime)
        {
            UserInter?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
            if (resourceBarIndex != -1)
            {
                layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                    "CalRemix:NPCDialogueInterface",
                    delegate
                    {
                        UserInter.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.Game)
                );
            }
        }
    }

    public class NPCDialogueSet
    {
        public Dictionary<string, List<string>> dialogue;
        public Color bgColor;
        public Color borderColor;
        public string texture;

        public NPCDialogueSet(Dictionary<string, List<string>> dialogue, Color bgColor, Color borderColor, string texture)
        {
            this.dialogue = dialogue;
            this.bgColor = bgColor;
            this.borderColor = borderColor;
            this.texture = texture;
        }
    }
}