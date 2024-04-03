﻿using CalamityMod;
using CalamityMod.Items.Pets;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Summon;
using CalRemix.Items.Weapons;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class FannyManager : ModSystem
    {
        public static void LoadWeaponMessages()
        {
            #region Weapon
            fannyMessages.Add(new FannyMessage("Murasama", "Erm, holy crap? $0? Is that a reference to my FAVORITE game of all time, metal gear rising revengeance? Did you know that calamity adds a custom boss health boss bar and many othe-",
               "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<Murasama>())).AddDynamicText(FannyMessage.GetPlayerName));

            fannyMessages.Add(new FannyMessage("Ultrakill", "Oh EM GEE! A gun from the hit first-person shooter game, \'MURDERDEATH\'!? Try throwing out some coins and hitting them with a Titanium Railgun to pull a sick railcoin maneuver!",
   "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<MidasPrime>()) || Main.LocalPlayer.HasItem(ModContent.ItemType<CrackshotColt>())).AddItemDisplay(ModContent.ItemType<MidasPrime>()));

            fannyMessages.Add(new FannyMessage("MurasamaBig", "You. Yeah, you. I know you downloaded this mod just so you could have your disgustingly sized Murasama slash back! After all of Fanny's incessant, inaccurate drivel, are you satisfied? Was it worth it?",
   "EvilIdle", (FannySceneMetrics scene) => Main.LocalPlayer.controlUseItem && Main.LocalPlayer.HeldItem.type == ModContent.ItemType<Murasama>() && DownedBossSystem.downedDoG && fannyTimesFrozen <= 0).SpokenByEvilFanny());

            fannyMessages.Add(new FannyMessage("Tofu", "Uh oh! Looks like one of your items is a reference to a smelly old game franchise known as Touhou! Do your ol\' pal Fanny a good deed and put it away.",
   "Nuhuh", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<ScarletDevil>()) || Main.LocalPlayer.HasItem(ModContent.ItemType<GlacialEmbrace>()) || Main.LocalPlayer.HasItem(ModContent.ItemType<RecitationoftheBeast>()) || Main.LocalPlayer.HasItem(ModContent.ItemType<EventHorizon>()) || Main.LocalPlayer.HasItem(ModContent.ItemType<HermitsBoxofOneHundredMedicines>()) || Main.LocalPlayer.HasItem(ModContent.ItemType<PristineFury>()) || Main.LocalPlayer.HasItem(ModContent.ItemType<DarkSpark>()) || Main.LocalPlayer.HasItem(ModContent.ItemType<ResurrectionButterfly>()) || Main.LocalPlayer.HasItem(ModContent.ItemType<FantasyTalisman>()) || Main.LocalPlayer.HasItem(ModContent.ItemType<HellsSun>()) || Main.LocalPlayer.HasItem(ModContent.ItemType<TheDreamingGhost>())).SetHoverTextOverride("Anything for you Fanny!"));
            #endregion
        }
    }
}