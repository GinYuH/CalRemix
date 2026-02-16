using CalamityMod;
using CalRemix.Content.Items.ZAccessories;
using CalRemix.Content.NPCs.Subworlds.Sealed;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
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
                if (talkedNPC.life < 0 || !talkedNPC.active || !NPCDIalogueUISystem.allDialogue.TryGetValue(talkedNPC.type, out NPCDialogueSet value))
                {
                    ResetDialogue();
                }
                else
                {
                    NPCDialogueSet dialogueInfo = value;
                    if (currentKey == "" || currentIndex == -1)
                    {
                        ResetDialogue();
                    }
                    else
                    {
                        if (!dialogueInfo.dialogue.ContainsKey(currentKey))
                        {
                            Main.NewText("Key " + currentKey + " for " + talkedNPC.ModNPC.Name + " not found", Color.Red);
                            ResetDialogue();
                            return;
                        }
                        Main.blockMouse = true;
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
                                    string key = talkedNPC.ModNPC.Name + "." + currentKey;
                                    if (!Main.LocalPlayer.GetModPlayer<DialoguePlayer>().readDialogue.TryAdd(key, true))
                                    {
                                        Main.LocalPlayer.GetModPlayer<DialoguePlayer>().readDialogue[key] = true;
                                    }
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

        /// <summary>
        /// Is this NPC currently writing text?
        /// </summary>
        /// <param name="npc"></param>
        /// <returns> False if not being talked to or the NPC has finished writing</returns>
        public static bool NotFinishedTalking(NPC npc)
        {
            if (!IsBeingTalkedTo(npc))
                return false;
            if (currentMaxCharacters > 0 && currentCharacter >= currentMaxCharacters)
            {
                return false;
            }
            return currentCharacter >= 0;
        }

        /// <summary>
        /// Is this NPC's dialogue UI currently up?
        /// </summary>
        /// <param name="npc"></param>
        /// <returns>False if the NPC is not being talked to</returns>
        public static bool IsBeingTalkedTo(NPC npc)
        {
            if (Main.LocalPlayer.Remix().talkedNPC <= -1)
                return false;
            if (Main.npc[Main.LocalPlayer.Remix().talkedNPC] != npc)
                return false;
            if (currentKey == "")
                return false;
            return true;
        }

        public static bool HasReadDialogue(Player p, string key)
        {
            DialoguePlayer dialP = p.GetModPlayer<DialoguePlayer>();
            if (dialP.readDialogue.TryGetValue(key, out bool value))
            {
                return value;
            }
            return false;
        }

        public static bool HasReadDialogue(Player p, string npc, string key)
        {
            return HasReadDialogue(p, npc + "." + key);
        }

        public static bool HasReadDialogue(Player p, NPC npc, string key)
        {
            return HasReadDialogue(p, npc.ModNPC.Name + "." + key);
        }

        public static bool HasReadDialogue(Player p, int index, string key)
        {
            return HasReadDialogue(p, Main.npc[index].ModNPC.Name + "." + key);
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
        }

        public override void PostAddRecipes()
        {
            RegisterNPC(new(ModContent.NPCType<BrightMind>(),
            ["Intro", "Gastrosludge", "GastroEye", "End",
            "Hurt1", "Hurt2", "Hurt3", "Hurt4", "Hurt5", "Hurt6", "Hurt7", "Hurt8", "Hurt9", "Hurt10"
            ],
            Color.DarkGoldenrod, Color.LightGoldenrodYellow));

            RegisterNPC(new(ModContent.NPCType<ShadeGreen>(),
            ["Intro1", "Intro2", "Intro3", "Intro4", "Mind1", "Mind2", "Cultist1", "Cultist2", "QuestEnd" ],
            Color.Black, new(34, 177, 76)));

            RegisterNPC(new(ModContent.NPCType<ShadeBlue>(),
            ["Intro1", "Intro2", "Intro3", "Mind1", "Cultist1", "QuestEnd" ],
            Color.Black, Color.Blue));

            RegisterNPC(new(ModContent.NPCType<ShadeYellow>(),
            ["Intro1", "Cultist1", "QuestEnd" ],
            Color.Black, Color.Yellow));

            RegisterNPC(new(ModContent.NPCType<DreadonFriendly>(),
            ["Intro", "FightIntro", "End"],
            Color.PaleGoldenrod, Color.Cyan));

            RegisterNPC(new(ModContent.NPCType<MonorianWarrior>(),
            ["Intro", "Enrage"],
            Color.Red, Color.Cyan));

            RegisterNPC(new(ModContent.NPCType<RubyWarrior>(),
            ["Intro", "Cookie", "Rapier", "End"
            ],
            Color.Pink, Color.DarkRed));

            RegisterNPC(new(ModContent.NPCType<VigorCloak>(),
            ["Intro", "Void", "Disilphia", "Oneguy", "Shades", "End" ],
            Color.MediumPurple, Color.Red));
        }

        public static void RegisterNPC(NPCDialogueSet set)
        {
            allDialogue.Add(set.npcType, set);
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
        public Dictionary<string, List<string>> dialogue = new();
        public Color bgColor;
        public Color borderColor;
        public int npcType;

        public NPCDialogueSet(int npcType, List<string> keys, Color bgColor, Color borderColor)
        {
            for (int j = 0; j < keys.Count; j++)
            {
                List<string> allText = new();
                string key = keys[j];
                for (int i = 0; i < 22; i++)
                {
                    string currentText = "Mods.CalRemix.NPCDialog." + ContentSamples.NpcsByNetId[npcType].ModNPC.Name + "." + key + "." + i;
                    if (Language.Exists(currentText))
                    {
                        allText.Add(Language.GetTextValue(currentText));
                    }
                    else
                    {
                        break;
                    }
                }
                dialogue.Add(key, allText);
            }
            this.npcType = npcType;
            this.bgColor = bgColor;
            this.borderColor = borderColor;
        }
    }

    public class DialoguePlayer : ModPlayer
    {
        public Dictionary<string, bool> readDialogue = new();

        public override void SaveData(TagCompound tag)
        {
            foreach (var v in readDialogue)
            {
                string key = v.Key;
                if (tag.TryGet(key, out bool _))
                {
                    tag[key] = v.Value;
                }
                else
                {
                    tag.Add(key, v.Value);
                }
            }
        }

        public override void LoadData(TagCompound tag)
        {
            foreach (var v in NPCDIalogueUISystem.allDialogue)
            {
                NPC n = ContentSamples.NpcsByNetId[v.Key];
                string name = n.ModNPC.Name;
                foreach (var pair in v.Value.dialogue)
                {
                    string registerKey = name + "." + pair.Key;
                    if (readDialogue.TryGetValue(registerKey, out bool value))
                    {
                        readDialogue[registerKey] = value;
                    }
                    else
                    {
                        readDialogue.Add(registerKey, tag.GetBool(registerKey));
                    }
                }
            }
        }
    }
}