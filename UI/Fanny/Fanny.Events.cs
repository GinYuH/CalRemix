using CalamityMod;
using CalamityMod.Events;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class ScreenHelperMessageManager : ModSystem
    {
        public static void LoadEventMessages()
        {

            screenHelperMessages.Add(new HelperMessage("InvasionDeath", "These guys are really giving us what for. It might be a good idea to step away for a bit in order to come up with a new strategy...",
                "FannySob", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.dead && Main.invasionType != InvasionID.None, cooldown: 1200));

            screenHelperMessages.Add(new HelperMessage("Raining", "It's raining! It's pouring! The man on the moon is snoring! Wait, who is the man on the moon!?",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.raining).SetHoverTextOverride("That's a good question Fanny!"));

            screenHelperMessages.Add(new HelperMessage("Winding", "Ah the weather is so nice out today! You should go fly a kite! That's something a lot of people were interested in right?",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main._shouldUseWindyDayMusic));
            
            screenHelperMessages.Add(new HelperMessage("EvilSnowLegion", "You know... This reminds me back when I had a boyfriend. He was real sweet, if I'll be honest. I even had my first kiss with him. Though he was a snowman, and I was a flame... You kinda get where this is going.",
                "EvilFannyIdle", (ScreenHelperSceneMetrics scene) => Main.invasionType == InvasionID.SnowLegion).SpokenByEvilFanny());

            screenHelperMessages.Add(new HelperMessage("OOA", "Just so we're clear, none of this is canon, got it?",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.invasionType == InvasionID.CachedOldOnesArmy).SetHoverTextOverride("Gotcha Fanny!"));

            screenHelperMessages.Add(new HelperMessage("BloodMoon", "During a blood moon, strange critters can be found hiding under rocks. They can be used for blood moon fishing, but be careful, those teeth can really hurt.",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.bloodMoon));

            screenHelperMessages.Add(new HelperMessage("BloodMoonStare", "",
                "FannyStare", (ScreenHelperSceneMetrics scene) => Main.rand.NextBool(1500000) && Main.bloodMoon));

            screenHelperMessages.Add(new HelperMessage("Eclipxe", "It's dark.",
                "FannySob", (ScreenHelperSceneMetrics scene) => Main.eclipse && !DownedBossSystem.downedDoG).SetHoverTextOverride("It is."));

            screenHelperMessages.Add(new HelperMessage("Holloween", "Happy Halloween my friend! Looks like everyone is getting their spook game on. Get ready for a monster mash!",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.pumpkinMoon && !DownedBossSystem.downedDoG));

            screenHelperMessages.Add(new HelperMessage("Frostmas", "IT'S CHRISTMAS!!! You don't need to get me a gift, just having you around is the most fan-tastic gift a flame like me could ask for!",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.snowMoon && !DownedBossSystem.downedDoG).SetHoverTextOverride("Awe, thanks Fanny, you're great to have around too!"));

            screenHelperMessages.Add(new HelperMessage("BREvilkys", "Kill yourself NOW!",
                "EvilFannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.statLife >= Main.LocalPlayer.statLifeMax2 * 0.75f && BossRushEvent.BossRushActive && BossRushEvent.CurrentTier >= 5).SetHoverTextOverride("I'm busy right now, Evil Fanny!").SpokenByEvilFanny());

            screenHelperMessages.Add(new HelperMessage("Nite", "Nighttime is when the real party starts! But watch out for those nocturnal nasties, they're like uninvited guests who never leave. Keep a torch handy, it's like bringing a flashlight to a ghost story.",
              "FannyIdle", (ScreenHelperSceneMetrics scene) => !Main.dayTime));

            screenHelperMessages.Add(new HelperMessage("Slimerain", "Ooh, a slime rain! It's like a colorful meteor shower, except instead of making wishes, you're dodging slimy projectiles! Better grab an umbrella, or at least a slicker. Don't want to end up looking like a walking slime ball! wink wink",
               "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.slimeRain));

            screenHelperMessages.Add(new HelperMessage("Towers", "Ah, the lunar events! It's like hosting a cosmic tea party, except instead of sipping tea, you're dodging death rays from space! Just remember to RSVP with your best battle gear and a side of moon cheese.",
               "FannyIdle", (ScreenHelperSceneMetrics scene) => NPC.downedAncientCultist));
        }
    }
}