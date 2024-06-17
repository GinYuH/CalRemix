using CalamityMod;
using CalamityMod.Items.Placeables.Ores;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class ScreenHelperMessageManager : ModSystem
    {
        public static void LoadBossDeathMessages()
        {

            screenHelperMessages.Add(new HelperMessage("AbyssBegin", "Every 60 seconds in the Abyss a hour passes by, truly wonderful!",
               "FannyNuhuh", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && NPC.downedBoss3).SetHoverTextOverride("Very interesting Fanny!"));

            screenHelperMessages.Add(new HelperMessage("Cryodeath", "Ha! Snow's over, Cryogen! Wasn't that pretty cool?",
               "FannyIdle", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && DownedBossSystem.downedCryogen));

            screenHelperMessages.Add(new HelperMessage("FollyDeath", "You finally beat the Dragonfolly! Though, I don't know why you'd need pheromones though. I mean, you're already a handsome fellow...",
               "FannyNuhuh", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && DownedBossSystem.downedDragonfolly).SetHoverTextOverride("Uhm.... Thanks for the help, Fanny...?"));

            screenHelperMessages.Add(new HelperMessage("CalcloneDeath", "Oh it was just a clone.",
                "FannySob", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && DownedBossSystem.downedCalamitasClone));

            screenHelperMessages.Add(new HelperMessage("Yharore", "Looks like the caverns have been laced with Auric Ore! The ore veins are pretty massive, so I’d say it’s best that you get up close and go hog wild!",
               "FannyIdle", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && DownedBossSystem.downedYharon).AddItemDisplay(ModContent.ItemType<AuricOre>()));

            screenHelperMessages.Add(new HelperMessage("DraedonExit", "Good golly! You did it! Though I'd HATE to imagine the financial losses caused by the destruction of those machines.",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && DownedBossSystem.downedExoMechs));

            screenHelperMessages.Add(new HelperMessage("SCalDie", "That was exhilarating! Though that means the end of our adventure is upon us. What a Calamity as one may say!",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && DownedBossSystem.downedCalamitas));

            screenHelperMessages.Add(new HelperMessage("Mechs", "Congrats on taking down those clanky contraptions! It's like defeating a bunch of oversized kitchen appliances. Just remember, don't get too cocky or they might just hit you with their spatulas of doom!",
               "FannyIdle", (ScreenHelperSceneMetrics scene) => NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3));

            HelperMessage plant = new HelperMessage("Plantoreum1", "Hey, have you seen my precious pink flower that I've been growing for 15 years? I left her around here. She's been my best friend for years now, and I could never fathom what I'd do if I had lost h",
               "FannyIdle", (ScreenHelperSceneMetrics scene) => NPC.downedPlantBoss);

            screenHelperMessages.Add(plant);
            HelperMessage plant2 = new HelperMessage("Plantoreum2", "Oh.",
               "FannySob", HelperMessage.AlwaysShow, needsToBeClickedOff: false, duration: 30)
                .NeedsActivation();

            plant.AddStartEvent(() => plant2.ActivateMessage());

            screenHelperMessages.Add(plant2);

            screenHelperMessages.Add(new HelperMessage("Golem", "Good job defeating that pile o' bricks! You sure.. cough cough wow, the air sure is du- cough cough",
               "FannyNuhuh", (ScreenHelperSceneMetrics scene) => NPC.downedGolemBoss));
        }
    }
}
