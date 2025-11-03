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

namespace CalRemix.Content.NPCs
{
    public abstract class QuestNPC : ModNPC
    {
        public virtual bool CanBeTalkedTo => true;

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
                        string key = "";
                        for (int i = 0; i < ItemQuestSystem.itemQuests[Type].Count; i++)
                        {
                            ItemQuest quest = ItemQuestSystem.itemQuests[Type][i];
                            bool completedQuest = false;
                            if (quest.IsActive.Invoke())
                            {
                                if (quest.CustomCondition != null)
                                {
                                    if (quest.CustomCondition.Invoke())
                                    {
                                        quest.CompletionEvent.Invoke();
                                        completedQuest = true;
                                    }
                                }
                                else if (Main.LocalPlayer.HasItem(quest.RequiredItem))
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
                                if (i == ItemQuestSystem.itemQuests[Type].Count - 1)
                                {
                                    key = "End";
                                }
                                else
                                {
                                    key = ItemQuestSystem.itemQuests[Type][i + 1].AssociatedDialogue;
                                }
                            }
                        }
                        if (key == "")
                            key = "End";
                        NPCDialogueUI.StartDialogue(NPC.whoAmI, key);
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

        public ItemQuest(int requiredItem, Func<bool> isActive, Action completionEvent, string associatedDialogue, bool consume = false, Func<bool> customCondition = null)
        {
            RequiredItem = requiredItem;
            IsActive = isActive;
            AssociatedDialogue = associatedDialogue;
            this.consume = consume;
            CompletionEvent = completionEvent;
            CustomCondition = customCondition;
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
                new(ModContent.ItemType<Butter>(), () => rubyLevel == 0, () => rubyLevel = 1, "Intro", true),
                new(ItemID.ChocolateChipCookie, () => rubyLevel == 1, () => rubyLevel = 2, "Cookie", true),
                new(ModContent.ItemType<ArcticRapier>(), () => rubyLevel == 2, () => rubyLevel = 3, "Rapier")
            });

            itemQuests.Add(ModContent.NPCType<DreadonFriendly>(), new() {
                new(ModContent.ItemType<TurnipSprout>(), () => draedonLevel == 0, () => draedonLevel = 1, "Intro", true),
                new(ItemID.HornetBanner, () => draedonLevel == 1, () => draedonLevel = 2, "FightIntro", true, () => RemixDowned.downedDraedon),
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