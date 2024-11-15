using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using System.Linq;
using Terraria.Audio;
using System.Collections.Generic;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Potions;
using Terraria.DataStructures;
using CalamityMod.Items.PermanentBoosters;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Accessories.Vanity;
using CalamityMod.Items.Pets;
using CalamityMod.NPCs.CalClone;
using Microsoft.Build.Tasks.Deployment.ManifestUtilities;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.NPCs.HiveMind;
using CalamityMod.NPCs.Perforator;
using CalamityMod.Items.DraedonMisc;

namespace CalRemix.UI
{
    public partial class ScreenHelperManager : ModSystem
    {
        public static List<int> medicalItems =
        [
            ItemID.Vitamins,
            ModContent.ItemType<StressPills>(),
            ModContent.ItemType<Laudanum>(),
            ItemID.LesserHealingPotion,
            ItemID.HealingPotion,
            ItemID.GreaterHealingPotion,
            ItemID.SuperHealingPotion,
            ModContent.ItemType<OmegaHealingPotion>(),
            ModContent.ItemType<SupremeHealingPotion>()
        ];

        public static void LoadCrimSon()
        {
            HelperMessage crimtro1 = HelperMessage.New("CrimSonIntro1", "It is Dangerous to Go Alone. Take This.", "CrimSonDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().gottenCellPhone, 6, cantBeClickedOff: true)
                .SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon).AddEndEvent(() => Main.LocalPlayer.ConsumeItem(ItemID.CellPhone));

            HelperMessage.New("CrimSonIntro2", "Woah! Hey there buddy! How are you d-",
                "FannyIdle", HelperMessage.AlwaysShow, 5, cantBeClickedOff: true).ChainAfter(crimtro1, delay: 3, startTimerOnMessageSpoken: true);

            HelperMessage crimtro3 = HelperMessage.New("CrimSonIntro3", "Hnnk, you shut lil flame boy ueehhe.",
                "CrimSonDefault", HelperMessage.AlwaysShow).SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon).ChainAfter(crimtro1);

            HelperMessage.New("CrimSonIntro4", "Well that's not nice.",
                "FannySob", HelperMessage.AlwaysShow).ChainAfter(crimtro3, delay: 3, startTimerOnMessageSpoken: true).SetHoverTextOverride("Indeed it isn't Fanny!");

            HelperMessage.New("CrimPizza", "I like my cheese drippy bruh.",
                "CrimSonDefault", (ScreenHelperSceneMetrics metrics) => Main.LocalPlayer.HasItem(ItemID.Pizza)).SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon);

            HelperMessage.New("CrimFood", "I’m hungry, give me that.",
                "CrimSonDefault", (ScreenHelperSceneMetrics metrics) => !Main.LocalPlayer.HasItem(ItemID.Pizza) && Main.LocalPlayer.inventory.Any((Item i) => ItemID.Sets.IsFood[i.type]) && Main.rand.NextBool(36000), onlyPlayOnce: false).AddStartEvent(EatFood).SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon);

            HelperMessage.New("CrimMedicalAid", "Give me Painkillers.",
                "CrimSonDefault", (ScreenHelperSceneMetrics metrics) => Main.LocalPlayer.inventory.Any((Item i) => medicalItems.Contains(i.type)) && Main.rand.NextBool(36000), onlyPlayOnce: false).AddStartEvent(GetMedicalHelp).SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon);
            
            HelperMessage.New("CrimBandage", "Change my bandages.",
                "CrimSonDefault", (ScreenHelperSceneMetrics metrics) => Main.LocalPlayer.HasItem(ItemID.Cobweb)).AddStartEvent(GetBandage).SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon);

            HelperMessage.New("StillWater", "Still water? Those who know.",
                "CrimSonDefault", (ScreenHelperSceneMetrics metrics) => Main.LocalPlayer.wet && Main.rand.NextBool(36000), onlyPlayOnce: false).SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon);

            HelperMessage.New("BuildThang", "Block tuah build on that thang",
                "CrimSonDefault", (ScreenHelperSceneMetrics metrics) => Main.LocalPlayer.HeldItem.createTile > TileID.Dirt).SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon);

            HelperMessage goon1 = HelperMessage.New("Calgyatt1", "erm, what the sigma? mirror mirror on the wall, who has the biggest GYATT of them all?? calamitas spotted, gooning mode: ACTIVATED",
                "CrimSonDefault", (ScreenHelperSceneMetrics metrics) => metrics.onscreenNPCs.Any((NPC n) => n.type == ModContent.NPCType<CalamitasClone>()) && Main.rand.NextBool(600), 6, cantBeClickedOff: true).SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon);

            HelperMessage.New("Calgyatt2", "Go die in a ditch.",
                "EvilFannyIdle", HelperMessage.AlwaysShow).SpokenByEvilFanny().ChainAfter(goon1, delay: 4, startTimerOnMessageSpoken: true);

            HelperMessage.New("Calgyatt3", "Phonk music and galaxy gas lil chuddy",
                "CrimSonDefault", HelperMessage.AlwaysShow).SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon).ChainAfter(goon1);

            HelperMessage.New("Mutantsacep", "This video is sponsored by MutantScaped",
                "CrimSonDefault", (ScreenHelperSceneMetrics metrics) => metrics.onscreenNPCs.Any((NPC n) => ModLoader.TryGetMod("FargowiltasSouls", out Mod farto) && n.ModNPC != null && n.ModNPC.Mod == farto && n.boss)).SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon);

            HelperMessage.New("RedHealth", "Nah fr your hearts looked better red /srs. MAKE HEALTH GREAT AGAIN!!!",
                "CrimSonDefault", (ScreenHelperSceneMetrics metrics) => Main.LocalPlayer.ConsumedLifeFruit > 0).SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon).AddStartEvent(() => Main.LocalPlayer.ConsumedLifeFruit = 0);

            HelperMessage.New("Calpaintwoo", "Hey dude did u watch calamity paint? It's like skibidi toilet kinda but like with that calamity rizz, y'know? You have gyatt to check it out!",
                "CrimSonDefault", (ScreenHelperSceneMetrics metrics) => Main.rand.NextBool(216000) || metrics.onscreenNPCs.Any((NPC n) => n.type == ModContent.NPCType<HiveMind>() || n.type == ModContent.NPCType<PerforatorHive>())).SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon).AddEndEvent(() => Utils.OpenToURL("https://www.youtube.com/watch?v=pEL0wMug4-Q&list=PLtiUOCdXKkxGh36KxHUyKy832in1hE3o-&index=1"));

            HelperMessage.New("Draecell", "Ayo u gonna share bruh? I stg gimme my fanum tax",
                "CrimSonDefault", (ScreenHelperSceneMetrics metrics) => Main.LocalPlayer.HasItem(ModContent.ItemType<DraedonPowerCell>())).SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon).AddEndEvent(EatCell);
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
                if (medicalItems.Contains(i.type))
                {
                    SoundEngine.PlaySound(BetterSoundID.ItemEat);
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
    }
}