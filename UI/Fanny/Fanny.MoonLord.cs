using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Items.SummonItems;
using CalamityMod.Items.Weapons.Rogue;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class ScreenHelperMessageManager : ModSystem
    {
        public static void LoadMoonLordDeath()
        {
            HelperMessage ml = new HelperMessage("ML1", "Blegh, I think I swallowed one of that thing's bones. Well, it's time for Godseeker Mode. You will face a sequence of challenges, each more difficult than the last with little to no breathing between encounters.",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => NPC.downedMoonlord, 7, needsToBeClickedOff: true, onlyPlayOnce: true, displayOutsideInventory: true).AddDelay(0.4f);

            screenHelperMessages.Add(ml);

            HelperMessage ml2 = new HelperMessage("ML2", "Almost sounds like a boss rush or something.",
                "EvilFannyIdle", HelperMessage.AlwaysShow, 6, needsToBeClickedOff: false, onlyPlayOnce: true, displayOutsideInventory: true)
                .NeedsActivation(4f).SpokenByEvilFanny();

            ml.AddStartEvent(() => ml2.ActivateMessage());

            screenHelperMessages.Add(ml2);

            HelperMessage ml3 = new HelperMessage("ML3", "A priority you should take care of immediately is harvesting Unholy Essence from some new, fearsome creatures that have appeared in the Underworld and Hallow. You can then use the essence to make the Rune of Kos and summon the Sentinels of the Devourer.",
                "FannyIdle", HelperMessage.AlwaysShow, 6, needsToBeClickedOff: true, onlyPlayOnce: true, displayOutsideInventory: true).AddItemDisplay(ModContent.ItemType<RuneofKos>()).NeedsActivation();

            ml.AddEndEvent(() => ml3.ActivateMessage());
            screenHelperMessages.Add(ml3);

            HelperMessage ml4 = new HelperMessage("ML4", "You can find 3 different types of cosmic remains if you search the sky, one of them is the remains of the Moon guy you just defeated! Second one is some exotic clusters used for some artifacts! Third one is the distorted remains of Cosmos itself.",
                "FannyIdle", HelperMessage.AlwaysShow, 6, needsToBeClickedOff: true, onlyPlayOnce: true, displayOutsideInventory: true).NeedsActivation();
            ml3.AddEndEvent(() => ml4.ActivateMessage());
            screenHelperMessages.Add(ml4);

            HelperMessage ml5 = new HelperMessage("ML5", "The Dungeon has also gotten an upgrade in power, with new spirit enemies that occasionally pop out of enemies when defeated which drop Phantoplasm, an important crafting material. I'd reccomend killing as many of those things as possible!",
                "FannyIdle", HelperMessage.AlwaysShow, 6, needsToBeClickedOff: true, onlyPlayOnce: true, displayOutsideInventory: true).NeedsActivation().AddItemDisplay(ModContent.ItemType<Necroplasm>());
            ml4.AddEndEvent(() => ml5.ActivateMessage());
            screenHelperMessages.Add(ml5);

            HelperMessage ml6 = new HelperMessage("ML6", "I'm getting a bit of deja vu here.",
                "EvilFannyIdle", HelperMessage.AlwaysShow, 6, needsToBeClickedOff: false, onlyPlayOnce: true, displayOutsideInventory: true)
                .NeedsActivation(4f).SpokenByEvilFanny();

            ml5.AddStartEvent(() => ml6.ActivateMessage());

            screenHelperMessages.Add(ml6);


            HelperMessage ml7 = new HelperMessage("ML7", "It appears that the red moon will start yielding bountiful harvests of Blood Orbs now! You should take advantage of this opportunity to craft lotsa potions! I'm personally a fan of the Inferno Potion myself.",
                "FannyNuhuh", HelperMessage.AlwaysShow, 6, needsToBeClickedOff: true, onlyPlayOnce: true, displayOutsideInventory: true).NeedsActivation().AddItemDisplay(ItemID.InfernoPotion);
            ml5.AddEndEvent(() => ml7.ActivateMessage());
            screenHelperMessages.Add(ml7);


            HelperMessage ml8 = new HelperMessage("ML8", "Are you feeling a little weak? It may be because of the dreaded \'Curse of the Eldritch\', a terrifying affliction inflicted upon those who slay eldritch beasts which permanently reduces your life regeneration!",
                "FannySob", HelperMessage.AlwaysShow, 6, needsToBeClickedOff: true, onlyPlayOnce: true, displayOutsideInventory: true).NeedsActivation();
            ml7.AddEndEvent(() => ml8.ActivateMessage());
            screenHelperMessages.Add(ml8);


            HelperMessage ml9 = new HelperMessage("ML9", "I should also mention that if you have a certain thief in one of your towns, they'll start selling the flawless Celestial Reaper, which can be used to cut down herbs significantly faster than the normal Sickle.",
                "FannyIdle", HelperMessage.AlwaysShow, 6, needsToBeClickedOff: true, onlyPlayOnce: true, displayOutsideInventory: true).NeedsActivation().AddItemDisplay(ModContent.ItemType<CelestialReaper>());
            ml8.AddEndEvent(() => ml9.ActivateMessage());
            screenHelperMessages.Add(ml9);

            HelperMessage ml10 = new HelperMessage("ML10", "Oh oh I should also mention-",
                "FannyIdle", HelperMessage.AlwaysShow, 6, needsToBeClickedOff: false, onlyPlayOnce: true, displayOutsideInventory: true).NeedsActivation();
            ml9.AddEndEvent(() => ml10.ActivateMessage());
            screenHelperMessages.Add(ml10);

            HelperMessage ml11 = new HelperMessage("ML11", "Oh my god shut up already, how much can one boss unlock!?",
                "EvilFannyIdle", HelperMessage.AlwaysShow, 6, needsToBeClickedOff: false, onlyPlayOnce: true, displayOutsideInventory: true)
                .NeedsActivation(2f).SpokenByEvilFanny();

            ml10.AddStartEvent(() => ml11.ActivateMessage());

            screenHelperMessages.Add(ml11);

            HelperMessage ml12 = new HelperMessage("ML12", "It appears this encounter is going to have to be cut short buddy, I need to go do something.",
                "FannyIdle", HelperMessage.AlwaysShow, 6, needsToBeClickedOff: false, onlyPlayOnce: true, displayOutsideInventory: true).NeedsActivation();
            ml11.AddStartEvent(() => ml12.ActivateMessage());
            screenHelperMessages.Add(ml12);

            ml12.AddEndEvent(() => Violence());
        }

        private static void Violence()
        {
            Main.LocalPlayer.Calamity().GeneralScreenShakePower = 800;
            SoundEngine.PlaySound(CalamityMod.NPCs.ExoMechs.Ares.AresGaussNuke.NukeExplosionSound, Main.LocalPlayer.Center);
            SoundEngine.PlaySound(SoundID.DD2_GoblinScream);
            SoundEngine.PlaySound(SoundID.DD2_KoboldExplosion);
        }
    }
}
