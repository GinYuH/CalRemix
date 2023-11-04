using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Items.PermanentBoosters;
using CalamityMod.Items.Pets;
using CalamityMod.Items.Potions;
using CalamityMod.Items.SummonItems;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Summon;
using CalRemix.Items.Materials;
using CalRemix.Items.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class FannyManager : ModSystem
    {
        public static void LoadItemMessages()
        {
            #region Item
            fannyMessages.Add(new FannyMessage("Forge", "Na Na Na! The big robotic forge needs a lot of blue meat from the ads! It cannot work without it!",
                "Nuhuh", HasDraedonForgeMaterialsButNoMeat, onlyPlayOnce: false, cooldown: 120).AddItemDisplay(ModContent.ItemType<DeliciousMeat>()));

            fannyMessages.Add(new FannyMessage("DeliciousMeat", "Oooh! Delicious Meat! Collect as much as you can, it will save you a lot of time.", "Awooga",
                (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<DeliciousMeat>())).AddItemDisplay(ModContent.ItemType<DeliciousMeat>()));

            //Add a condition to this one YUH, to pass the test of knowledge...
            //YUH YUH YUH YUH YUH
            //IBAN IBAN IBAN IBAN IBAN
            fannyMessages.Add(new FannyMessage("DesertScourge", "I see you've gotten some mandibles. For some reason, people always try to make medallions out of them when the only way to get them is by killing Cnidrions after the destruction of the legendary Wulfrum Excavator. Strangely specific isn't it? Guess that's just how the cookie crumbles!", "Nuhuh", HasDesertMedallionMaterials).AddItemDisplay(ModContent.ItemType<DesertMedallion>()));

            fannyMessages.Add(new FannyMessage("VoodooDoll", "Cool doll you have! i think that it will be even cooler when in lava!",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ItemID.GuideVoodooDoll)));

            fannyMessages.Add(new FannyMessage("PortalGun", "Cave Johnson here. We're fresh out of combustible lemons, but let me tell you a little bit about this thing here. These portals are only designed to stick on planetoid rock and not much else. Hope you've got a test chamber lying around that's full of that stuff!",
    "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ItemID.PortalGun)));

            fannyMessages.Add(new FannyMessage("TwentyTwo", "I love 22. My banner now.",
    "Idle", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ItemID.HornetBanner)).AddItemDisplay(ItemID.HornetBanner).AddStartEvent(() => Main.LocalPlayer.ConsumeItem(ItemID.HornetBanner)).SetHoverTextOverride("Thanks Fanny! That was cluttering my inventory!"));

            fannyMessages.Add(new FannyMessage("Shadowspec", "Please throw this thing out, it will delete your world if you have it in inventory for too long!",
                "Sob", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<ShadowspecBar>())).AddItemDisplay(ModContent.ItemType<ShadowspecBar>()).SetHoverTextOverride("Thank you for the help Fanny! I will!"));

            fannyMessages.Add(new FannyMessage("Jump", "Did you know? You can press the \"space\" button to jump!",
                "Nuhuh", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ItemID.PlatinumCoin)).SetHoverTextOverride("Thanks Fanny! You're so helpful!"));

            fannyMessages.Add(new FannyMessage("TitanHeart", "You got a heart from a titan! Place it on the tower for a wacky light show!",
                "Idle", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<TitanHeart>())).AddItemDisplay(ModContent.ItemType<TitanHeart>()));

            fannyMessages.Add(new FannyMessage("BloodyVein", "The Bloody Vein is an item of utmost importance which can be inserted into various altars and machinery for wacky results. How about inserting one into one of those lab hologram box things?",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<BloodyVein>())).AddItemDisplay(ModContent.ItemType<BloodyVein>()));

            fannyMessages.Add(new FannyMessage("RottenEye", "The Rotting Eyeball is an item of zero importance. The Bloody Vein from the Crimson's Perforators is way better!",
                "Nuhuh", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<RottingEyeball>()) && !WorldGen.crimson).AddItemDisplay(ModContent.ItemType<RottingEyeball>()).SetHoverTextOverride("Thanks Fanny! I'll be sure to make a Crimson world next time."));

            fannyMessages.Add(new FannyMessage("AlloyBar", "Congratulations, you have obtained the final bar for this stage of your adventure. You should attempt making some Alloy Bars, a versatile material made of every available bar which can be used for powerful items.",
                "Idle", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<AlloyBar>())).AddItemDisplay(ModContent.ItemType<AlloyBar>()));

            fannyMessages.Add(new FannyMessage("Murasama", "Erm, holy crap? $0? Is that a reference to my FAVORITE game of all time, metal gear rising revengeance? Did you know that calamity adds a custom boss health boss bar and many othe-",
               "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<Murasama>())).AddDynamicText(FannyMessage.GetPlayerName));

            fannyMessages.Add(new FannyMessage("Sponge", "Oh, is that a Sponge? Maybe avoid using it. I've heard something about the wielder dying, or something...",
               "Nuhuh", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<TheSponge>())).AddItemDisplay(ModContent.ItemType<TheSponge>()));

            fannyMessages.Add(new FannyMessage("Garbo", "Wowie! That scrap there is useless!",
               "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ItemID.OldShoe) || Main.LocalPlayer.HasItem(ItemID.FishingSeaweed) || Main.LocalPlayer.HasItem(ItemID.TinCan) || Main.LocalPlayer.HasItem(ItemID.JojaCola)).AddItemDisplay(ItemID.TrashCan).SetHoverTextOverride("Thanks Fanny! I already wanted to cook it."));

            fannyMessages.Add(new FannyMessage("Nightfuel", "Nightmare Fuel, huh? ...you know, maybe if you can harvest enough of it, maybe those Pumpkings will stop terorrizing our inhabitants and they'll be permanently more happy!",
   "Idle", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<NightmareFuel>())).AddItemDisplay(ModContent.ItemType<NightmareFuel>()));

            fannyMessages.Add(new FannyMessage("Endenergy", "Ooh, is that Endothermic Energy? If we can get a decent supply of it, I think those Ice Queens will fear us and our residents might be forever grateful with us!",
   "Idle", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<EndothermicEnergy>())).AddItemDisplay(ModContent.ItemType<EndothermicEnergy>()));

            fannyMessages.Add(new FannyMessage("Darksunfrag", "What's that? Darksun Fragment? Do you think with enough of it, our world will be permanently lit up like a lemon-scented candle flame?",
   "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<DarksunFragment>())).AddItemDisplay(ModContent.ItemType<DarksunFragment>()));

            fannyMessages.Add(new FannyMessage("Onion", "I'd be weary about eating that strange plant. You can only get one, so it might be useful to hang on to it for later.",
   "Nuhuh", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<CelestialOnion>())).AddItemDisplay(ModContent.ItemType<CelestialOnion>()));

            fannyMessages.Add(new FannyMessage("Ultrakill", "Oh EM GEE! A gun from the hit first-person shooter game, \'MURDERDEATH\'!? Try throwing out some coins and hitting them with a Titanium Railgun to pull a sick railcoin maneuver!",
   "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<MidasPrime>()) || Main.LocalPlayer.HasItem(ModContent.ItemType<CrackshotColt>())).AddItemDisplay(ModContent.ItemType<MidasPrime>()));

            fannyMessages.Add(new FannyMessage("Tofu", "Uh oh! Looks like one of your items is a reference to a smelly old game franchise known as Touhou! Do your ol\' pal Fanny a good deed and put it away.",
   "Nuhuh", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<ScarletDevil>()) || Main.LocalPlayer.HasItem(ModContent.ItemType<GlacialEmbrace>()) || Main.LocalPlayer.HasItem(ModContent.ItemType<RecitationoftheBeast>()) || Main.LocalPlayer.HasItem(ModContent.ItemType<EventHorizon>()) || Main.LocalPlayer.HasItem(ModContent.ItemType<HermitsBoxofOneHundredMedicines>()) || Main.LocalPlayer.HasItem(ModContent.ItemType<PristineFury>()) || Main.LocalPlayer.HasItem(ModContent.ItemType<DarkSpark>()) || Main.LocalPlayer.HasItem(ModContent.ItemType<ResurrectionButterfly>()) || Main.LocalPlayer.HasItem(ModContent.ItemType<FantasyTalisman>()) || Main.LocalPlayer.HasItem(ModContent.ItemType<HellsSun>()) || Main.LocalPlayer.HasItem(ModContent.ItemType<TheDreamingGhost>())).SetHoverTextOverride("Anything for you Fanny!"));

            /*fannyMessages.Add(new FannyMessage("Catharsis", "Don’t exhume Kaleidoscope! Catharsis is known to cause clinical depression in users.",
               "Nuhuh", (FannySceneMetrics scene) => ModLoader.HasMod("CatalystMod") && Main.LocalPlayer.HasItem(ItemID.RainbowWhip) && Main.LocalPlayer.talk == ModContent.NPCType<WITCH>()));*/
            #endregion
        }
    }
}