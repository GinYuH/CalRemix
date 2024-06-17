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
    public partial class ScreenHelperManager : ModSystem
    {
        public static void LoadMoonLordDeath()
        {
            HelperMessage startMessage = HelperMessage.New("ML1", "Blegh, I think I swallowed one of that thing's bones. Well, it's time for Godseeker Mode. You will face a sequence of challenges, each more difficult than the last with little to no breathing between encounters.",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => NPC.downedMoonlord, 7).AddDelay(0.4f);


            HelperMessage.New("ML2", "Almost sounds like a boss rush or something.",
                "EvilFannyIdle", HelperMessage.AlwaysShow, 6, cantBeClickedOff: true)
                .ChainAfter(delay: 4f, startTimerOnMessageSpoken: true).SpokenByEvilFanny();


            HelperMessage.New("ML3", "A priority you should take care of immediately is harvesting Unholy Essence from some new, fearsome creatures that have appeared in the Underworld and Hallow. You can then use the essence to make the Rune of Kos and summon the Sentinels of the Devourer.",
                "FannyIdle", HelperMessage.AlwaysShow, 6).AddItemDisplay(ModContent.ItemType<RuneofKos>()).ChainAfter(startMessage);


            HelperMessage.New("ML4", "You can find 3 different types of cosmic remains if you search the sky, one of them is the remains of the Moon guy you just defeated! Second one is some exotic clusters used for some artifacts! Third one is the distorted remains of Cosmos itself.",
                "FannyIdle", HelperMessage.AlwaysShow, 6).ChainAfter();

            HelperMessage ml5 = HelperMessage.New("ML5", "The Dungeon has also gotten an upgrade in power, with new spirit enemies that occasionally pop out of enemies when defeated which drop Phantoplasm, an important crafting material. I'd reccomend killing as many of those things as possible!",
                "FannyIdle", HelperMessage.AlwaysShow, 6).AddItemDisplay(ModContent.ItemType<Necroplasm>()).ChainAfter();

            HelperMessage.New("ML6", "I'm getting a bit of deja vu here.",
                "EvilFannyIdle", HelperMessage.AlwaysShow, 6, cantBeClickedOff: true)
                .ChainAfter(delay: 4f, startTimerOnMessageSpoken: true).SpokenByEvilFanny();


             HelperMessage.New("ML7", "It appears that the red moon will start yielding bountiful harvests of Blood Orbs now! You should take advantage of this opportunity to craft lotsa potions! I'm personally a fan of the Inferno Potion myself.",
                "FannyNuhuh", HelperMessage.AlwaysShow, 6).AddItemDisplay(ItemID.InfernoPotion).ChainAfter(ml5);


            HelperMessage.New("ML8", "Are you feeling a little weak? It may be because of the dreaded \'Curse of the Eldritch\', a terrifying affliction inflicted upon those who slay eldritch beasts which permanently reduces your life regeneration!",
                "FannySob", HelperMessage.AlwaysShow, 6).ChainAfter();

            HelperMessage.New("ML9", "I should also mention that if you have a certain thief in one of your towns, they'll start selling the flawless Celestial Reaper, which can be used to cut down herbs significantly faster than the normal Sickle.",
                "FannyIdle", HelperMessage.AlwaysShow, 6).ChainAfter().AddItemDisplay(ModContent.ItemType<CelestialReaper>());

            HelperMessage.New("ML10", "Oh oh I should also mention-",
                "FannyIdle", HelperMessage.AlwaysShow, 6, cantBeClickedOff: true).ChainAfter();

            HelperMessage.New("ML11", "Oh my god shut up already, how much can one boss unlock!?",
                "EvilFannyIdle", HelperMessage.AlwaysShow, 6, cantBeClickedOff: true)
                .ChainAfter(delay: 2f, startTimerOnMessageSpoken: true).SpokenByEvilFanny();

            HelperMessage.New("ML12", "It appears this encounter is going to have to be cut short buddy, I need to go do something.",
                "FannyIdle", HelperMessage.AlwaysShow, 6, cantBeClickedOff: true)
                .ChainAfter(startTimerOnMessageSpoken : true).AddEndEvent(Violence);

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
