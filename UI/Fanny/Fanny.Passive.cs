using CalamityMod;
using CalamityMod.Tiles.DraedonStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class FannyManager : ModSystem
    {
        public static void LoadPassiveMessages()
        {
            fannyMessages.Add(new FannyMessage("MeldGunk", "In a remote location underground, there is a second strain of Astral Infection. If left unattended for too long, it can start spreading and dealing irreversible damage! Stay safe and happy hunting!",
                "Nuhuh", (FannySceneMetrics scene) => CalRemixWorld.meldCountdown <= 3600 && Main.hardMode));

            fannyMessages.Add(new FannyMessage("FungusGarden", "Careful when exploring the Shroom Garden. I hear some rather large crustaceans make their home there. Wouldn't want to be turned into Delicious Meat!",
    "Nuhuh", (FannySceneMetrics scene) => Main.rand.NextBool(2160000) && !DownedBossSystem.downedCrabulon, cooldown: 120));

            fannyMessages.Add(new FannyMessage("ProbablyYakuza", "One time, I saw someone being dragged into a car by three men. The men took around 10 minutes and 23 seconds to subdue their victim, and 2 more minutes to drive away. I did nothing to stop it.",
            "Nuhuh", (FannySceneMetrics scene) => Main.rand.NextBool(1500000)));

            fannyMessages.Add(new FannyMessage("Fuckyou", "You are now manually breathing.",
               "Nuhuh", (FannySceneMetrics scene) => Main.rand.NextBool(4000000)));

            fannyMessages.Add(new FannyMessage("Mount", "Do a barrel roll on that thing you're riding!",
               "Awooga", (FannySceneMetrics scene) => Main.rand.NextBool(1000) && Main.LocalPlayer.mount.Type != MountID.None));

            fannyMessages.Add(new FannyMessage("LookingForPlating", "Are you trying to find some Dubious Plating? I'm afraid that the stocks for them have plummeted and all existing plating was turned into scrap metal to be dumped in the Dungeon, so try looking there!",
               "Nuhuh", (FannySceneMetrics scene) => Main.LocalPlayer.chest != -1 && (Main.tile[Main.chest[Main.LocalPlayer.chest].x, Main.chest[Main.LocalPlayer.chest].y].TileType == ModContent.TileType<SecurityChestTile>() || Main.tile[Main.chest[Main.LocalPlayer.chest].x, Main.chest[Main.LocalPlayer.chest].y].TileType == ModContent.TileType<AgedSecurityChestTile>())));

            fannyMessages.Add(new FannyMessage("Creepy", Main.rand.Next(1000000) + " remaining...",
   "Cryptid", (FannySceneMetrics scene) => Main.rand.NextBool(100000000), duration: 60, needsToBeClickedOff: false));

            fannyMessages.Add(new FannyMessage("Mhage", "Be careful when using magic weapons. Drinking too many mana potions can drain your health, and leave you vulnerable to enemy attacks.",
               "Nuhuh", (FannySceneMetrics scene) => Main.rand.NextBool(2160000) && Main.LocalPlayer.HeldItem.DamageType == DamageClass.Magic, cooldown: 300, onlyPlayOnce: false));

            fannyMessages.Add(new FannyMessage("Thrust", "Did you know you can parry enemy attacks with your sword? Just right click the moment something is about to hit you, and you'll block it with ease!",
               "Idle", (FannySceneMetrics scene) => Main.rand.NextBool(2160000) && Main.LocalPlayer.HeldItem.DamageType == ModContent.GetInstance<TrueMeleeDamageClass>(), cooldown: 300, onlyPlayOnce: false));

            fannyMessages.Add(new FannyMessage("Frozen1", "I'm back! It was quite chilly in there, but luckily, I was able to thaw myself out! Hopefully it doesn't happen again!",
               "Idle", (FannySceneMetrics scene) => fannyTimesFrozen == 1).SetHoverTextOverride("..."));

            fannyMessages.Add(new FannyMessage("Frozen2", "I-cee you're having some trouble. Don't worry! I broke out of the ice cube I was stuck in again!",
               "Nuhuh", (FannySceneMetrics scene) => fannyTimesFrozen == 2));

            fannyMessages.Add(new FannyMessage("Frozen3", "Wouldja believe it? I somehow managed to get trapped in another ice cube! Whoever keeps doing that is sure getting on thin ice.",
               "Nuhuh", (FannySceneMetrics scene) => fannyTimesFrozen == 3));

            fannyMessages.Add(new FannyMessage("Frozen4", "This is a bit embarassing, but I got myself caught in yet another ice cube! This shtick is getting cold at this point, or should I say warm?",
               "Idle", (FannySceneMetrics scene) => fannyTimesFrozen == 4));

            fannyMessages.Add(new FannyMessage("Frozen5", "At this point me and ice have gotten to know each other quite well, a true dance of the elements. I won't weigh you down anymore with updates on my frigid situation, have fun!",
               "Idle", (FannySceneMetrics scene) => fannyTimesFrozen == 5));

            fannyMessages.Add(new FannyMessage("Frozen6", "Oh wait wait wait, this time I found a small crumb inside the ice. It was disgusting!",
               "Idle", (FannySceneMetrics scene) => fannyTimesFrozen == 6));
        }
    }
}