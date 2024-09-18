using CalamityMod;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class ScreenHelperManager : ModSystem
    {
        //Loads fanny messages that aren't associated with anything else in particular
        public static void LoadGeneralFannyMessages()
        {

            HelperMessage.New("RemixJump", "Hey there friend! I noticed your jumps were a little too weak, so I added a bit of my Fanny-spice and now you can jump TWO times! I hope you enjoy this!",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().remixJumpCount >= 20).SetHoverTextOverride("Thanks Fanny! I will enjoy my new jumps!");

            HelperMessage.New("LowHP", "It looks like you're low on health. If your health reaches 0, you'll die. To combat this, don't let your health reach 0!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.statLife < Main.LocalPlayer.statLifeMax2 * 0.25f, cooldown: 1200, onlyPlayOnce: false).SetHoverTextOverride("Thanks Fanny! I'll heal.");

            HelperMessage.New("Invisible", "Where did you go?",
                "FannySob", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.invis || Main.LocalPlayer.shroomiteStealth || Main.LocalPlayer.vortexStealthActive || (Main.LocalPlayer.Calamity().rogueStealth >= Main.LocalPlayer.Calamity().rogueStealthMax && Main.LocalPlayer.Calamity().rogueStealthMax > 0)).SetHoverTextOverride("I'm still here Fanny!");
            
            HelperMessage.New("DarkArea", "Fun fact. The human head can still be conscious after decapitation for the average of 20 seconds.",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => DarkArea() && CalRemixWorld.worldFullyStarted);
            
            HelperMessage.New("ConstantDeath", "Is that someone behind you...?",
                "FannySob", (ScreenHelperSceneMetrics scene) => DontStarveDarknessDamageDealer.darknessTimer >= 300 && !Main.LocalPlayer.DeadOrGhost);
            
            HelperMessage.New("Cursed", "Looks like you've been cursed! If you spam Left Click, you'll be able to use items again sooner!",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.cursed);

            HelperMessage.New("OctFebruary", "Did you know? 31 in Octagonal is the same as 25 in Decimal! That means OCT 31 is the same as DEC 25! Happy Halloween and Merry Christmas!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => System.DateTime.Now.Month == 4 && System.DateTime.Now.Day == 14).SetHoverTextOverride("Thanks Fanny! I'll heal.");
            
            HelperMessage.New("DungeonGuardian", "It appears you're approaching the Dungeon. Normally this place is guarded by viscious guardians, but I've disabled them for you my dear friend.",
                "FannyNuhuh", NearDungeonEntrance);

            HelperMessage.New("MeldGunk", "In a remote location underground, there is a second strain of Astral Infection. If left unattended for too long, it can start spreading and dealing irreversible damage! Stay safe and happy hunting!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => CalRemixWorld.meldCountdown <= 3600 && Main.hardMode);

            HelperMessage.New("MeldHeart", "Look at all that gunk! I'm pretty sure it's impossible to break it, so the best solution I can give is to assure it doesn't spread further by digging around it.",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => CalRemixWorld.MeldTiles > 22 && !ModLoader.HasMod("NoxusBoss"));

            HelperMessage.New("MeldHeartNoxus", "Look at all that gunk! I'm pretty sure it's impossible to break it, well, maybe if you got some powerful spray bottle, but that might take a while, so the best solution I can give is to assure it doesn't spread further by digging around it.",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => CalRemixWorld.MeldTiles > 22 && ModLoader.HasMod("NoxusBoss"));

            HelperMessage.New("EvilMinions", "Oh, joy, another player reveling in their summoned minions like they've won the pixelated lottery. Just remember, those minions are as loyal as your Wi-Fi signal during a storm—here one minute, gone the next. Enjoy your fleeting companionship, I guess.",
                "EvilFannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.numMinions >= 10).SpokenByEvilFanny();

            HelperMessage.New("EvilTerraBlade", "Oh, congratulations, you managed to get a Terra Blade. I'm sure you're feeling all proud and accomplished now. But hey, don't strain yourself patting your own back too hard. It's just a sword, after all. Now, go on, swing it around like the hero you think you are.",
                "EvilFannyIdle", (ScreenHelperSceneMetrics scene) =>Main.LocalPlayer.HasItem(ItemID.TerraBlade)).SpokenByEvilFanny();

            HelperMessage.New("IonGuy", "Hey there, beachcomber! Looks like you've found a talking panel in that trash pile! It's wired up and ready to chat, but beware—it’s probably going to ask you for items. Remember, not all that glitters is gold, and not all talking panels are trustworthy. You might end up giving away your favorite pair of socks!",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().ionDialogue > 0);

            HelperMessage.New("Multiplayer", "The atmosphere seems a lot more social than what I'm used to. Be wary of ruptures in reality!",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.netMode != NetmodeID.SinglePlayer);

            HelperMessage.New("Norcheese", "Something, something, you dirty cheater.",
                "EvilFannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.Calamity().NorfleetCounter >= 4).SpokenByEvilFanny();

        }
        private static bool DarkArea()
        {
            Vector3 vector = Lighting.GetColor((int)Main.LocalPlayer.Center.X / 16, (int)Main.LocalPlayer.Center.Y / 16).ToVector3();
            return vector.Length() >= 0.15f;
        }
    }
}
