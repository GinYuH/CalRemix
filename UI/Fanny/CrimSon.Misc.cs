﻿using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using System.Linq;
using Terraria.Audio;
using Terraria.DataStructures;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Pets;
using CalamityMod.NPCs.CalClone;
using CalamityMod.NPCs.HiveMind;
using CalamityMod.NPCs.Perforator;
using CalamityMod.Items.DraedonMisc;
using CalamityMod.Items.Weapons.Summon;

namespace CalRemix.UI
{
    public partial class ScreenHelperManager : ModSystem
    {
        public static readonly SoundStyle ThoseWhoNose = new("CalRemix/Assets/Sounds/Helpers/ThoseWhoNose");

        public static void LoadCrimSon()
        {
            HelperMessage.New("CrimCutMeOut", "You didnt have to cut me off     Nekalakininahappenenawiwanatin...",
                "CrimSonDefault", (ScreenHelperSceneMetrics scene) => Main.rand.NextBool(2160000)).SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon);

            HelperMessage.New("CrimMango", "MANGO MANGO MANGO",
                "CrimSonDefault", (ScreenHelperSceneMetrics metrics) => Main.LocalPlayer.HasItem(ItemID.Mango)).SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon);

            HelperMessage.New("CrimPizza", "I like my cheese drippy bruh.",
                "CrimSonDefault", (ScreenHelperSceneMetrics metrics) => Main.LocalPlayer.HasItem(ItemID.Pizza)).SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon);

            HelperMessage.New("CrimFood", "Im hungry, give me that.",
                "CrimSonDefault", (ScreenHelperSceneMetrics metrics) => !Main.LocalPlayer.HasItem(ItemID.Pizza) && !Main.LocalPlayer.HasItem(ItemID.Mango) && Main.LocalPlayer.inventory.Any((Item i) => ItemID.Sets.IsFood[i.type]) && Main.rand.NextBool(360000), onlyPlayOnce: false).AddStartEvent(EatFood).SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon);

            HelperMessage.New("CrimMedicalAid", "Give me Painkillers.",
                "CrimSonDefault", (ScreenHelperSceneMetrics metrics) => Main.LocalPlayer.inventory.Any((Item i) => i.healLife > 0) && Main.rand.NextBool(360000), onlyPlayOnce: false).AddStartEvent(GetMedicalHelp).SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon);
            
            HelperMessage.New("CrimBandage", "Change my bandages.",
                "CrimSonDefault", (ScreenHelperSceneMetrics metrics) => Main.LocalPlayer.HasItem(ItemID.Cobweb)).AddStartEvent(GetBandage).SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon);

            HelperMessage.New("StillWater", "Still water? Those who know.",
                "CrimSonDefault", (ScreenHelperSceneMetrics metrics) => Main.LocalPlayer.wet && Main.rand.NextBool(360000), onlyPlayOnce: false).SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon);

            HelperMessage.New("BuildThang", "Block tuah build on that thang",
                "CrimSonDefault", (ScreenHelperSceneMetrics metrics) => Main.LocalPlayer.HeldItem.createTile > TileID.Dirt).SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon);

            HelperMessage.New("BlackHawkTuah", "More like black hawk tuah bruh",
                "CrimSonDefault", (ScreenHelperSceneMetrics metrics) => Main.LocalPlayer.HasItem(ModContent.ItemType<BlackHawkRemote>())).SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon).InitiateConversation();
            HelperMessage.New("BlackHawkTuah2", "I think this guy needs to be euthanized.",
                "EvilFannyDisgusted", HelperMessage.AlwaysShow).SpokenByEvilFanny().ChainAfter(delay: 2, startTimerOnMessageSpoken: true).EndConversation();

            HelperMessage goon1 = HelperMessage.New("Calgyatt1", "erm, what the sigma? mirror mirror on the wall, who has the biggest GYATT of them all?? calamitas spotted, gooning mode: ACTIVATED",
                "CrimSonDefault", (ScreenHelperSceneMetrics metrics) => metrics.onscreenNPCs.Any((NPC n) => n.type == ModContent.NPCType<CalamitasClone>()) && Main.rand.NextBool(600), 6, cantBeClickedOff: true).SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon).InitiateConversation();
            HelperMessage.New("Calgyatt2", "Go die in a ditch.",
                "EvilFannyMiffed", HelperMessage.AlwaysShow).SpokenByEvilFanny().ChainAfter(goon1, delay: 4, startTimerOnMessageSpoken: true);
            HelperMessage.New("Calgyatt3", "Phonk music and galaxy gas lil chuddy",
                "CrimSonDefault", HelperMessage.AlwaysShow).SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon).ChainAfter(goon1).EndConversation();

            HelperMessage.New("Mutantsacep", "This video is sponsored by MutantScaped",
                "CrimSonDefault", (ScreenHelperSceneMetrics metrics) => metrics.onscreenNPCs.Any((NPC n) => ModLoader.TryGetMod("FargowiltasSouls", out Mod farto) && n.ModNPC != null && n.ModNPC.Mod == farto && n.boss)).SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon);

            HelperMessage.New("RedHealth", "Nah fr your hearts looked better red /srs. MAKE HEALTH GREAT AGAIN!!!",
                "CrimSonDefault", (ScreenHelperSceneMetrics metrics) => Main.LocalPlayer.ConsumedLifeFruit > 0).SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon).AddStartEvent(() => Main.LocalPlayer.ConsumedLifeFruit = 0);

            HelperMessage.New("Calpaintwoo", "Hey dude did u watch calamity paint? It's like skibidi toilet kinda but like with that calamity rizz, y'know? You have gyatt to check it out!",
                "CrimSonDefault", (ScreenHelperSceneMetrics metrics) => Main.rand.NextBool(216000) || metrics.onscreenNPCs.Any((NPC n) => n.type == ModContent.NPCType<HiveMind>() || n.type == ModContent.NPCType<PerforatorHive>())).SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon).AddEndEvent(() => Utils.OpenToURL("https://www.youtube.com/watch?v=pEL0wMug4-Q&list=PLtiUOCdXKkxGh36KxHUyKy832in1hE3o-&index=1"));

            HelperMessage.New("Draecell", "Ayo u gonna share bruh? I stg gimme my fanum tax",
                "CrimSonDefault", (ScreenHelperSceneMetrics metrics) => Main.LocalPlayer.HasItem(ModContent.ItemType<DraedonPowerCell>())).SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon).AddEndEvent(EatCell);

            HelperMessage.New("BrickedUp", "Bricked up rn",
                "CrimSonDefault", (ScreenHelperSceneMetrics metrics) => Main.LocalPlayer.inventory.Any(i => i.Name.Contains("Brick"))).SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon);

            HelperMessage.New("HomoTwins", "Bro has homophobia",
                "CrimSonDefault", (ScreenHelperSceneMetrics metrics) => metrics.onscreenNPCs.Any((NPC n) => n.type == NPCID.Retinazer || n.type == NPCID.Spazmatism)).SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon);

            HelperMessage.New("dementia", "Can you beat Calamity Mod with Dementia?", 
                "CrimSonDefault", (ScreenHelperSceneMetrics metrics) => metrics.onscreenNPCs.Any((NPC n) => n.boss && n.life <= n.lifeMax * 1 / 10) && Main.rand.NextBool(108000), 6, cantBeClickedOff: true, cooldown: 108000, onlyPlayOnce: false).AddStartEvent(Dementia).SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon);
        }

        private static void EatFood()
        {
            foreach (Item i in Main.LocalPlayer.inventory)
            {
                if (ItemID.Sets.IsFood[i.type])
                {
                    SoundEngine.PlaySound(BetterSoundID.ItemEat);
                    Main.LocalPlayer.ConsumeItem(i.type);
                    break;
                }
            }
        }

        private static void GetMedicalHelp()
        {
            foreach (Item i in Main.LocalPlayer.inventory)
            {
                if (i.healLife > 0)
                {
                    SoundEngine.PlaySound(BetterSoundID.ItemDrink);
                    Main.LocalPlayer.ConsumeItem(i.type);
                    Main.LocalPlayer.QuickSpawnItem(new EntitySource_Misc(""), ModContent.ItemType<BloodOrb>());
                    break;
                }
            }
        }

        private static void GetBandage()
        {
            SoundEngine.PlaySound(BetterSoundID.ItemEat);
            Main.LocalPlayer.ConsumeItem(ItemID.Cobweb);
            Main.LocalPlayer.QuickSpawnItem(new EntitySource_Misc(""), ItemID.AncientCloth, 382);
            Main.LocalPlayer.QuickSpawnItem(new EntitySource_Misc(""), ModContent.ItemType<BloodyVein>());            
        }

        private static void EatCell()
        {
            SoundEngine.PlaySound(BetterSoundID.ItemEat);
            for (int i = 0; i < 200; i++)
                Main.LocalPlayer.ConsumeItem(ModContent.ItemType<DraedonPowerCell>());
        }

        private static void Dementia()
        {
            for (int q = 0; q < Terraria.Main.npc.Length - 1; q++)
            {
                if (Terraria.Main.npc[q].boss)
                {
                    Terraria.Main.npc[q].active = false;
                }
            }
        }

    }
}