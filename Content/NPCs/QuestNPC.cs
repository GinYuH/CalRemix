using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using CalRemix.Core.Biomes;
using CalRemix.Content.Projectiles.Weapons;
using CalRemix.UI;
using CalRemix.Content.Particles;
using Terraria.ID;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CalRemix.Content.Items.Weapons;
using CalRemix.Content.Items.Materials;
using CalRemix.Core.World;
using Microsoft.Xna.Framework.Input;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Sounds;
using CalRemix.Content.Items.Misc;
using System.Collections.Generic;
using CalRemix.Content.NPCs.Subworlds.Sealed;
using CalRemix.Content.Items.Potions;
using CalRemix.Content.Items.SummonItems;
using Terraria.ModLoader.IO;
using System.IO;
using SubworldLibrary;
using CalRemix.Content.Items.Placeables.Subworlds.Sealed;

namespace CalRemix.Content.NPCs
{
    public abstract class QuestNPC : ModNPC
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


        public override void AI()
        {
            if (CanBeTalkedTo)
            {
                if (!ItemQuestSystem.itemQuests.ContainsKey(Type))
                    return; 
                Rectangle maus = Utils.CenteredRectangle(Main.MouseWorld, Vector2.One * 10);

                if (maus.Intersects(NPC.getRect()))
                {
                    if (Main.LocalPlayer.controlUseTile && Main.LocalPlayer.Remix().talkedNPC == -1 && Main.LocalPlayer.Distance(NPC.Center) < 600)
                    {
                        if (NPC.type == ModContent.NPCType<VigorCloak>())
                        {
                            if (CalRemixWorld.shadeQuestLevel == 2)
                            {
                                NPCDialogueUI.StartDialogue(NPC.whoAmI, "Shades");
                                return;
                            }
                        }

                        string key = "";
                        for (int i = 0; i < ItemQuestSystem.itemQuests[Type].Count; i++)
                        {
                            ItemQuest quest = ItemQuestSystem.itemQuests[Type][i];
                            bool completedQuest = false;
                            if (quest.IsActive.Invoke())
                            {
                                bool cusQuest = false;
                                if (quest.CustomCondition != null)
                                {
                                    if (quest.CustomCondition.Invoke())
                                    {
                                        cusQuest = true;
                                        quest.CompletionEvent.Invoke();
                                        completedQuest = true;
                                    }
                                }
                                if (!cusQuest && Main.LocalPlayer.HasItem(quest.RequiredItem))
                                {
                                    if (quest.consume)
                                    {
                                        Main.LocalPlayer.ConsumeItem(quest.RequiredItem);
                                    }
                                    quest.CompletionEvent.Invoke();
                                    completedQuest = true;
                                }
                                else
                                {
                                    key = quest.AssociatedDialogue;
                                }
                            }
                            if (completedQuest)
                            {
                                if (quest.reward != null)
                                    Main.LocalPlayer.QuickSpawnItem(NPC.GetSource_FromThis(), quest.reward);
                                if (i == ItemQuestSystem.itemQuests[Type].Count - 1)
                                {
                                    key = "End";
                                }
                                else
                                {
                                    key = ItemQuestSystem.itemQuests[Type][i + 1].AssociatedDialogue;
                                }
                                break;
                            }
                        }
                        if (key == "")
                            key = "End";
                        NPCDialogueUI.StartDialogue(NPC.whoAmI, key);
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

    public class ItemQuest
    {
        public int RequiredItem;
        public Func<bool> CustomCondition = null;
        public Func<bool> IsActive;
        public Action CompletionEvent;
        public bool IsFinished = false;
        public string AssociatedDialogue = "";
        public bool consume = false;
        public Item reward;

        public ItemQuest(int requiredItem, Func<bool> isActive, Action completionEvent, string associatedDialogue, bool consume = false, Func<bool> customCondition = null, Item reward = null)
        {
            RequiredItem = requiredItem;
            IsActive = isActive;
            AssociatedDialogue = associatedDialogue;
            this.consume = consume;
            CompletionEvent = completionEvent;
            CustomCondition = customCondition;
            this.reward = reward;
        }

        public ItemQuest(Func<bool> customCondition, Func<bool> isActive, Action completionEvent, string associatedDialogue, bool consume = false, Item reward = null)
        {
            RequiredItem = -1;
            IsActive = isActive;
            AssociatedDialogue = associatedDialogue;
            this.consume = consume;
            CompletionEvent = completionEvent;
            CustomCondition = customCondition;
            this.reward = reward;
        }
    }

    public class ItemQuestSystem : ModSystem
    {
        public static Dictionary<int, List<ItemQuest>> itemQuests = new();

        public static int brainLevel = 0;

        public static int rubyLevel = 0;

        public static int draedonLevel = 0;

        public static int cultistLevel = 0;

        public override void PostSetupContent()
        {
            itemQuests.Add(ModContent.NPCType<BrightMind>(), new() { 
                new(ModContent.ItemType<MonorianGemShards>(), () => brainLevel == 0, () => brainLevel = 1, "Intro"),
                new(ModContent.ItemType<GastropodHide>(), () => brainLevel == 1, () => brainLevel = 2, "Gastrosludge"),
                new(ModContent.ItemType<GastropodEye>(), () => brainLevel == 2, () => brainLevel = 3, "GastroEye")
            });

            itemQuests.Add(ModContent.NPCType<RubyWarrior>(), new() {
                new(ModContent.ItemType<Butter>(), () => rubyLevel == 0, () => rubyLevel = 1, "Intro", true, reward: new Item(ModContent.ItemType<SealToken>(), 10)),
                new(ItemID.ChocolateChipCookie, () => rubyLevel == 1, () => rubyLevel = 2, "Cookie", true, reward: new Item(ModContent.ItemType<SealToken>(), 20)),
                new(ModContent.ItemType<ArcticRapier>(), () => rubyLevel == 2, () => rubyLevel = 3, "Rapier", reward: new Item(ModContent.ItemType<SealToken>(), 50))
            });

            itemQuests.Add(ModContent.NPCType<DreadonFriendly>(), new() {
                new(ModContent.ItemType<TurnipSprout>(), () => draedonLevel == 0, () => draedonLevel = 1, "Intro", true),
                new(() => RemixDowned.downedDraedon, () => draedonLevel == 1, () => draedonLevel = 2, "FightIntro", true),
            });

            itemQuests.Add(ModContent.NPCType<VigorCloak>(), new() {
                new(() => RemixDowned.downedVoid, () => cultistLevel == 0, () => cultistLevel = 1, "Intro", true, reward: new Item(ModContent.ItemType<GroundFleshBlock>(), 9999)),
                new(() => RemixDowned.downedDisil, () => cultistLevel == 1, () => cultistLevel = 2, "Disilphia", true, reward: new Item(ModContent.ItemType<GroundFleshBlock>(), 9999)),
                new(() => RemixDowned.downedOneguy, () => cultistLevel == 2, () => cultistLevel = 3, "Oneguy", true, reward: new Item(ModContent.ItemType<SkullKarrver>())),
            });
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(brainLevel);
            writer.Write(rubyLevel);
            writer.Write(draedonLevel);
            writer.Write(cultistLevel);
        }

        public override void NetReceive(BinaryReader reader)
        {
            brainLevel = reader.ReadInt32();
            rubyLevel = reader.ReadInt32();
            draedonLevel = reader.ReadInt32();
            cultistLevel = reader.ReadInt32();
        }

        public override void SaveWorldData(TagCompound tag)
        {
            tag.Add("CultistLevel", cultistLevel);
            tag.Add("BrainLevel", brainLevel);
            tag.Add("DraedonLevel", draedonLevel);
            tag.Add("RubyLevel", rubyLevel);
        }

        public override void LoadWorldData(TagCompound tag)
        {
            cultistLevel = tag.GetInt("CultistLevel");
            brainLevel = tag.GetInt("BrainLevel");
            draedonLevel = tag.GetInt("DraedonLevel");
            rubyLevel = tag.GetInt("RubyLevel");
        }

        public override void OnWorldLoad()
        {
            ResetBools();
        }

        public override void OnWorldUnload()
        {
            ResetBools();
        }


        public static void ResetBools()
        {
            if (SubworldSystem.AnyActive())
            {
                return;
            }
            rubyLevel = 0;
            cultistLevel = 0;
            draedonLevel = 0;
            rubyLevel = 0;
        }
    }
}