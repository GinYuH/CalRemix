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
    public partial class FannyManager : ModSystem
    {
        public static void LoadMoonLordDeath()
        {
            FannyMessage ml = new FannyMessage("ML1", "Blegh, I think I swallowed one of that thing's bones. Well, it's time for Godseeker Mode. You will face a sequence of challenges, each more difficult than the last with little to no breathing between encounters.",
                "Idle", (FannySceneMetrics scene) => NPC.downedMoonlord, 7, needsToBeClickedOff: true, onlyPlayOnce: true, displayOutsideInventory: true).AddDelay(0.4f);

            fannyMessages.Add(ml);

            FannyMessage ml2 = new FannyMessage("ML2", "Almost sounds like a boss rush or something.",
                "EvilIdle", FannyMessage.AlwaysShow, 6, needsToBeClickedOff: false, onlyPlayOnce: true, displayOutsideInventory: true)
                .NeedsActivation(4f).SpokenByEvilFanny();

            ml.AddStartEvent(() => ml2.ActivateMessage());

            fannyMessages.Add(ml2);

            FannyMessage ml3 = new FannyMessage("ML3", "A priority you should take care of immediately is harvesting Unholy Essence from some new, fearsome creatures that have appeared in the Underworld and Hallow. You can then use the essence to make the Rune of Kos and summon the Sentinels of the Devourer.",
                "Idle", FannyMessage.AlwaysShow, 6, needsToBeClickedOff: true, onlyPlayOnce: true, displayOutsideInventory: true).AddItemDisplay(ModContent.ItemType<RuneofKos>()).NeedsActivation();

            ml.AddEndEvent(() => ml3.ActivateMessage());
            fannyMessages.Add(ml3);

            FannyMessage ml4 = new FannyMessage("ML4", "You can find 3 different types of cosmic remains if you search the sky, one of them is the remains of the Moon guy you just defeated! Second one is some exotic clusters used for some artifacts! Third one is the distorted remains of Cosmos itself.",
                "Idle", FannyMessage.AlwaysShow, 6, needsToBeClickedOff: true, onlyPlayOnce: true, displayOutsideInventory: true).NeedsActivation();
            ml3.AddEndEvent(() => ml4.ActivateMessage());
            fannyMessages.Add(ml4);

            FannyMessage ml5 = new FannyMessage("ML5", "The Dungeon has also gotten an upgrade in power, with new spirit enemies that occasionally pop out of enemies when defeated which drop Phantoplasm, an important crafting material. I'd reccomend killing as many of those things as possible!",
                "Idle", FannyMessage.AlwaysShow, 6, needsToBeClickedOff: true, onlyPlayOnce: true, displayOutsideInventory: true).NeedsActivation().AddItemDisplay(ModContent.ItemType<Polterplasm>());
            ml4.AddEndEvent(() => ml5.ActivateMessage());
            fannyMessages.Add(ml5);

            FannyMessage ml6 = new FannyMessage("ML6", "I'm getting a bit of deja vu here.",
                "EvilIdle", FannyMessage.AlwaysShow, 6, needsToBeClickedOff: false, onlyPlayOnce: true, displayOutsideInventory: true)
                .NeedsActivation(4f).SpokenByEvilFanny();

            ml5.AddStartEvent(() => ml6.ActivateMessage());

            fannyMessages.Add(ml6);


            FannyMessage ml7 = new FannyMessage("ML7", "It appears that the red moon will start yielding bountiful harvests of Blood Orbs now! You should take advantage of this opportunity to craft lotsa potions! I'm personally a fan of the Inferno Potion myself.",
                "Nuhuh", FannyMessage.AlwaysShow, 6, needsToBeClickedOff: true, onlyPlayOnce: true, displayOutsideInventory: true).NeedsActivation().AddItemDisplay(ItemID.InfernoPotion);
            ml5.AddEndEvent(() => ml7.ActivateMessage());
            fannyMessages.Add(ml7);


            FannyMessage ml8 = new FannyMessage("ML8", "Are you feeling a little weak? It may be because of the dreaded \'Curse of the Eldritch\', a terrifying affliction inflicted upon those who slay eldritch beasts which permanently reduces your life regeneration!",
                "Sob", FannyMessage.AlwaysShow, 6, needsToBeClickedOff: true, onlyPlayOnce: true, displayOutsideInventory: true).NeedsActivation();
            ml7.AddEndEvent(() => ml8.ActivateMessage());
            fannyMessages.Add(ml8);


            FannyMessage ml9 = new FannyMessage("ML9", "I should also mention that if you have a certain thief in one of your towns, they'll start selling the flawless Celestial Reaper, which can be used to cut down herbs significantly faster than the normal Sickle.",
                "Idle", FannyMessage.AlwaysShow, 6, needsToBeClickedOff: true, onlyPlayOnce: true, displayOutsideInventory: true).NeedsActivation().AddItemDisplay(ModContent.ItemType<CelestialReaper>());
            ml8.AddEndEvent(() => ml9.ActivateMessage());
            fannyMessages.Add(ml9);

            FannyMessage ml10 = new FannyMessage("ML10", "Oh oh I should also mention-",
                "Idle", FannyMessage.AlwaysShow, 6, needsToBeClickedOff: false, onlyPlayOnce: true, displayOutsideInventory: true).NeedsActivation();
            ml9.AddEndEvent(() => ml10.ActivateMessage());
            fannyMessages.Add(ml10);

            FannyMessage ml11 = new FannyMessage("ML11", "Oh my god shut up already, how much can one boss unlock!?",
                "EvilIdle", FannyMessage.AlwaysShow, 6, needsToBeClickedOff: false, onlyPlayOnce: true, displayOutsideInventory: true)
                .NeedsActivation(2f).SpokenByEvilFanny();

            ml10.AddStartEvent(() => ml11.ActivateMessage());

            fannyMessages.Add(ml11);

            FannyMessage ml12 = new FannyMessage("ML12", "It appears this encounter is going to have to be cut short buddy, I need to go do something.",
                "Idle", FannyMessage.AlwaysShow, 6, needsToBeClickedOff: false, onlyPlayOnce: true, displayOutsideInventory: true).NeedsActivation();
            ml11.AddStartEvent(() => ml12.ActivateMessage());
            fannyMessages.Add(ml12);

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
