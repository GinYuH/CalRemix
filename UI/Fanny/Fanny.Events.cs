using CalamityMod;
using CalamityMod.Events;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class FannyManager : ModSystem
    {
        public static void LoadEventMessages()
        {

            fannyMessages.Add(new FannyMessage("InvasionDeath", "These guys are really giving us what for. It might be a good idea to step away for a bit in order to come up with a new strategy...",
                "Sob", (FannySceneMetrics scene) => Main.LocalPlayer.dead && Main.invasionType != InvasionID.None, cooldown: 1200));

            fannyMessages.Add(new FannyMessage("Raining", "It's raining! It's pouring! The man on the moon is snoring! Wait, who is the man on the moon!?",
                "Idle", (FannySceneMetrics scene) => Main.raining).SetHoverTextOverride("That's a good question Fanny!"));

            fannyMessages.Add(new FannyMessage("Winding", "Ah the weather is so nice out today! You should go fly a kite! That's something a lot of people were interested in right?",
                "Idle", (FannySceneMetrics scene) => Main._shouldUseWindyDayMusic));
            
            fannyMessages.Add(new FannyMessage("EvilSnowLegion", "You know... This reminds me back when I had a boyfriend. He was real sweet, if I'll be honest. I even had my first kiss with him. Though he was a snowman, and I was a flame... You kinda get where this is going.",
                "EvilIdle", (FannySceneMetrics scene) => Main.hardMode && Main.invasionType == InvasionID.SnowLegion).SpokenByEvilFanny());

            fannyMessages.Add(new FannyMessage("OOA", "Just so we're clear, none of this is canon, got it?",
                "Nuhuh", (FannySceneMetrics scene) => Main.invasionType == InvasionID.CachedOldOnesArmy).SetHoverTextOverride("Gotcha Fanny!"));

            fannyMessages.Add(new FannyMessage("BloodMoon", "During a blood moon, strange critters can be found hiding under rocks. They can be used for blood moon fishing, but be careful, those teeth can really hurt.",
                "Idle", (FannySceneMetrics scene) => Main.bloodMoon));

            fannyMessages.Add(new FannyMessage("BloodMoonStare", "",
                "Stare", (FannySceneMetrics scene) => Main.rand.NextBool(1500000) && Main.bloodMoon));

            fannyMessages.Add(new FannyMessage("Eclipxe", "It's dark.",
                "Sob", (FannySceneMetrics scene) => Main.eclipse && !DownedBossSystem.downedDoG).SetHoverTextOverride("It is."));

            fannyMessages.Add(new FannyMessage("Holloween", "Happy Halloween my friend! Looks like everyone is getting their spook game on. Get ready for a monster mash!",
                "Idle", (FannySceneMetrics scene) => Main.pumpkinMoon && !DownedBossSystem.downedDoG));

            fannyMessages.Add(new FannyMessage("Frostmas", "IT'S CHRISTMAS!!! You don't need to get me a gift, just having you around is the most fan-tastic gift a flame like me could ask for!",
                "Idle", (FannySceneMetrics scene) => Main.snowMoon && !DownedBossSystem.downedDoG).SetHoverTextOverride("Awe, thanks Fanny, you're great to have around too!"));

            fannyMessages.Add(new FannyMessage("BREvilkys", "Kill yourself NOW!",
                "EvilIdle", (FannySceneMetrics scene) => Main.hardMode && Main.LocalPlayer.statLife >= Main.LocalPlayer.statLifeMax2 * 0.75f && BossRushEvent.BossRushActive && BossRushEvent.CurrentTier >= 5).SetHoverTextOverride("I'm busy right now, Evil Fanny!").SpokenByEvilFanny());

            fannyMessages.Add(new FannyMessage("Nite", "Nighttime is when the real party starts! But watch out for those nocturnal nasties, they're like uninvited guests who never leave. Keep a torch handy, it's like bringing a flashlight to a ghost story.",
              "Idle", (FannySceneMetrics scene) => !Main.dayTime));

            fannyMessages.Add(new FannyMessage("Slimerain", "Ooh, a slime rain! It's like a colorful meteor shower, except instead of making wishes, you're dodging slimy projectiles! Better grab an umbrella, or at least a slicker. Don't want to end up looking like a walking slime ball! wink wink",
               "Awooga", (FannySceneMetrics scene) => Main.slimeRain));

            fannyMessages.Add(new FannyMessage("Towers", "Ah, the lunar events! It's like hosting a cosmic tea party, except instead of sipping tea, you're dodging death rays from space! Just remember to RSVP with your best battle gear and a side of moon cheese.",
               "Idle", (FannySceneMetrics scene) => NPC.downedAncientCultist));
        }
    }
}