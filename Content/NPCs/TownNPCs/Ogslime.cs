using CalRemix.UI;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.Player;
using CalRemix.Content.Items.ZAccessories;
using Terraria.GameContent.Bestiary;

namespace CalRemix.Content.NPCs.TownNPCs
{
    [AutoloadHead]
    public class Ogslime : ModNPC
    {
        public static readonly short MaxPhrases = 34;
        public static double spawnTime = double.MaxValue;
        public override void Load()
        {
            Terraria.On_Player.ItemCheck_ApplyHoldStyle_Inner += PetSlime;
        }
        public static void PetSlime(On_Player.orig_ItemCheck_ApplyHoldStyle_Inner orig, Player p, float mountOffset, Item sItem, Rectangle heldItemFrame)
        {
            if (p.isPettingAnimal && p.TalkNPC?.type == ModContent.NPCType<Ogslime>())
            {
                int counter = p.miscCounter % 14 / 7;
                CompositeArmStretchAmount stretch = CompositeArmStretchAmount.ThreeQuarters;
                if (counter == 1)
                {
                    stretch = CompositeArmStretchAmount.Full;
                }
                p.SetCompositeArmBack(enabled: true, stretch, (float)Math.PI * -2f * 0.2f * (float)p.direction);
            }
            else
            {
                orig(p, mountOffset, sItem, heldItemFrame);
            }
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 14;
            NPCID.Sets.ExtraFramesCount[Type] = 0;
            NPCID.Sets.AttackFrameCount[Type] = 0;
            NPCID.Sets.ExtraTextureCount[Type] = 0;
            NPCID.Sets.HatOffsetY[Type] = NPCID.Sets.HatOffsetY[NPCID.TownSlimeBlue];
            NPCID.Sets.NPCFramingGroup[Type] = NPCID.Sets.NPCFramingGroup[NPCID.TownSlimeBlue];
            NPCID.Sets.IsTownPet[Type] = true;
            NPCID.Sets.IsTownSlime[Type] = true;
            if (Main.dedServ)
                return;
            HelperMessage.New("OgslimeAwakening",
                "O-Ogslime!?!?",
                "FannyAwooga",
                (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == Type));
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.TownSlimeBlue);

            AIType = NPCID.TownSlimeBlue;
            AnimationType = NPCID.TownSlimeBlue;
            NPC.lifeMax = 1000;
            DrawOffsetY = 5;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override string GetChat()
        {
            Main.player[Main.myPlayer].currentShoppingSettings.HappinessReport = "";
            // a list of common words and phrases
            List<string> list = [];
            for (int i = 1; i < MaxPhrases; i++)
            {
                list.Add(CalRemixHelper.LocalText($"NPCs.{Name}.Phrase{i}").Value);
            }
            // inflate the list with a bunch of "ands" since that allows for it to appear more often and create more comical results
            for (int i = 0; i < list.Count; i++)
            {
                if (i % 3 == 0)
                {
                    list.Add(CalRemixHelper.LocalText($"NPCs.{Name}.And").Value);
                }
            }
            List<string> itemNames = new List<string>();

            // add every remix item to a list
            for (int i = ItemID.Count; i < ContentSamples.ItemsByType.Count; i++)
            {
                Item item = ContentSamples.ItemsByType[i];
                if (item.ModItem != null)
                {
                    if (item.ModItem.Mod == Mod && item.ModItem is not DebuffStone)
                    {
                        itemNames.Add(item.Name);
                    }
                }
            }
            // add every remix npc to the list
            for (int i = NPCID.Count; i < ContentSamples.NpcsByNetId.Count; i++)
            {
                if (ContentSamples.NpcsByNetId.ContainsKey(i))
                {
                    NPC item = ContentSamples.NpcsByNetId[i];
                    if (item.ModNPC != null)
                    {
                        if (item.ModNPC.Mod == Mod)
                        {
                            itemNames.Add(item.FullName);
                        }
                    }
                }
            }
            // add your steam name to the list
            itemNames.Add(Steamworks.SteamFriends.GetPersonaName());

            // smash everything together
            string chat = itemNames[Main.rand.Next(0, itemNames.Count - 1)] + " " + list[Main.rand.Next(0, list.Count - 1)] + " " + itemNames[Main.rand.Next(0, itemNames.Count - 1)] + " " + list[Main.rand.Next(0, list.Count - 1)] + " " + itemNames[Main.rand.Next(0, itemNames.Count - 1)];
            // there is a 1 in 3+iteration chance for another word and name to be attatched
            for (int i = 0; i < 7; i++)
            {
                if (Main.rand.NextBool(3 + i))
                {
                    chat += " " + list[Main.rand.Next(0, list.Count - 1)] + " " + itemNames[Main.rand.Next(0, itemNames.Count - 1)];
                }
            }
            // period
            chat += ".";
            return chat;
        }
        public static List<string> PossibleNames = new()
        {
            "Sloomeburn", "Schlawsma", "Salvation", "Slumb", "Slaughe", "Sluagombloid", "Soil", "Scluner", "Slogs", "Sclerotina", "Scordyceps", "Subrum"
        };
        public override List<string> SetNPCNameList() => PossibleNames;

        public override void SetChatButtons(ref string button, ref string button2)
        {
            //button = "";
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
        }

        public override bool CanTownNPCSpawn(int numTownNPCs)/* tModPorter Suggestion: Copy the implementation of NPC.SpawnAllowed_Merchant in vanilla if you to count money, and be sure to set a flag when unlocked, so you don't count every tick. */
        {
            return CalRemixWorld.ogslime;
        }

        public override void AI()
        {
            NPC.position.X = MathHelper.Clamp(NPC.position.X, 150f, Main.maxTilesX * 16f - 150f);
            NPC.position.Y = MathHelper.Clamp(NPC.position.Y, 150f, Main.maxTilesY * 16f - 150f);
            if (!CalRemixWorld.ogslime)
            {
                CalRemixWorld.ogslime = true;
            }
        }
    }
}