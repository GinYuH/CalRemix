using CalamityMod;
using CalamityMod.Events;
using CalamityMod.Items.Pets;
using CalRemix.Content.NPCs.PandemicPanic;
using CalRemix.Core.World;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class ScreenHelperManager : ModSystem
    {
        public static void LoadEventMessages()
        {

            HelperMessage.New("InvasionDeath", "These guys are really giving us what for. It might be a good idea to step away for a bit in order to come up with a new strategy...",
                "FannySob", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.dead && Main.invasionType != InvasionID.None, cooldown: 1200);

            HelperMessage.New("Raining", "It's raining! It's pouring! The man on the moon is snoring! Wait, who is the man on the moon!?",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.raining).SetHoverTextOverride("That's a good question Fanny!");

            HelperMessage.New("Winding", "Ah the weather is so nice out today! You should go fly a kite! That's something a lot of people were interested in right?",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main._shouldUseWindyDayMusic);
            
            HelperMessage.New("SnowLegion", "You know... This reminds me back when I had a boyfriend. He was real sweet, if I'll be honest. I even had my first kiss with him. Though he was a snowman, and I was a flame... You kinda get where this is going.",
                "EvilFannyIdle", (ScreenHelperSceneMetrics scene) => Main.invasionType == InvasionID.SnowLegion).SpokenByEvilFanny().InitiateConversation();

            HelperMessage.New("SnowLegion2", "Where'd all these gooners come from?",
                "CrimSonDefault", HelperMessage.AlwaysShow).SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon).ChainAfter(delay: 6, startTimerOnMessageSpoken: true);

            HelperMessage.New("SnowLegion3", "They're called goons!",
                "FannyNuhuh", HelperMessage.AlwaysShow).ChainAfter(delay: 4, startTimerOnMessageSpoken: true).EndConversation();

            HelperMessage.New("OOA", "Just so we're clear, none of this is canon, got it?",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.invasionType == InvasionID.CachedOldOnesArmy).SetHoverTextOverride("Gotcha Fanny!");

            HelperMessage.New("BloodMoon", "During a blood moon, strange critters can be found hiding under rocks. They can be used for blood moon fishing, but be careful, those teeth can really hurt.",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.bloodMoon);

            HelperMessage.New("BloodMoonStare", "",
                "FannyStare", (ScreenHelperSceneMetrics scene) => Main.rand.NextBool(1500000) && Main.bloodMoon);

            HelperMessage.New("Eclipxe", "It's dark.",
                "FannySob", (ScreenHelperSceneMetrics scene) => Main.eclipse && !DownedBossSystem.downedDoG).SetHoverTextOverride("It is.");

            HelperMessage.New("Holloween", "Happy Halloween my friend! Looks like everyone is getting their spook game on. Get ready for a monster mash!",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.pumpkinMoon && !DownedBossSystem.downedDoG);

            HelperMessage.New("Frostmas", "IT'S CHRISTMAS!!! You don't need to get me a gift, just having you around is the most fan-tastic gift a flame like me could ask for!",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.snowMoon && !DownedBossSystem.downedDoG).SetHoverTextOverride("Awe, thanks Fanny, you're great to have around too!");

            HelperMessage.New("BREvilkys", "Kill yourself NOW!",
                "EvilFannyKYS", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.statLife >= Main.LocalPlayer.statLifeMax2 * 0.75f && BossRushEvent.BossRushActive && BossRushEvent.CurrentTier >= 5).SetHoverTextOverride("I'm busy right now, Evil Fanny!").SpokenByEvilFanny();

            HelperMessage.New("Nite", "Nighttime is when the real party starts! But watch out for those nocturnal nasties, they're like uninvited guests who never leave. Keep a torch handy, it's like bringing a flashlight to a ghost story.",
              "FannyIdle", (ScreenHelperSceneMetrics scene) => !Main.dayTime);

            HelperMessage.New("Slimerain", "Ooh, a slime rain! It's like a colorful meteor shower, except instead of making wishes, you're dodging slimy projectiles! Better grab an umbrella, or at least a slicker. Don't want to end up looking like a walking slime ball! wink wink",
               "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.slimeRain);

            HelperMessage.New("Towers", "Ah, the lunar events! It's like hosting a cosmic tea party, except instead of sipping tea, you're dodging death rays from space! Just remember to RSVP with your best battle gear and a side of moon cheese.",
               "FannyIdle", (ScreenHelperSceneMetrics scene) => NPC.downedAncientCultist);

            HelperMessage.New("PanicHint", "You should always collect water bills from your civilians for money! If you don't, try poisoning their water supply! Putting a Bloody Vein into a sink should do it!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => DownedBossSystem.downedPerforator && Main.bloodMoon).AddItemDisplay(ModContent.ItemType<BloodyVein>());

            HelperMessage.New("Oxydazy", "With the wind blowing like crazy, your projectiles are getting carried away faster than a bad joke. But here’s a hole-in-one tip: if you hit a golf ball into space, you may be able to hit one of the ships of the legendary Archwitch, Oxy!",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => CalRemixWorld.oxydayTime > 1).AddItemDisplay(ItemID.GolfClubIron);

            HelperMessage pandemic1 = HelperMessage.New("PandemicPanic", "An invasion has begun with giant immune system cells duking it out with invading microbes! You can side with the immune cells or the microbes by taking out more of the other side. Just remember, no matter who you choose, it’s going to be a cell-ebration of epic proportions! Stay sharp and choose wisely!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => PandemicPanic.IsActive).InitiateConversation();
            HelperMessage pandemic2 = HelperMessage.New("CrimsonPandemic1", "Yo king go mog on them immune system and show them your Jamaican smile + Brazilian phonk + Balkan squat + Pinoy resilience + Vietnamese crawling technique + Still water + Noradrenaline + Those who know :skull:",
                "CrimSonDefault", (ScreenHelperSceneMetrics scene) => PandemicPanic.IsActive).SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon).ChainAfter(pandemic1, startTimerOnMessageSpoken: true, delay: 10);
            HelperMessage pandemic3 = HelperMessage.New("CrimsonPandemic2", "Ooiii, Crim Soooon~!!! You do realize that these virulent critters are no good for our body, nya~! >m< These lil goobers protect our bodies like millions of microscopic magical girls, desu~! OwO I learned that from my club's biology studies~!",
                "TrapperHappy", (ScreenHelperSceneMetrics scene) => PandemicPanic.IsActive).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan).ChainAfter(pandemic2, startTimerOnMessageSpoken: true, delay: 4).EndConversation();

            HelperMessage.New("MeteorStarboard", "Wow! You sure are getting lots of stars tonight! You must have said something really funny to get all those. After all, every star you get is a community contribution!", "FannyAwooga", (ScreenHelperSceneMetrics metrics) => Terraria.Star.starfallBoost >= 3);
        }
    }
}