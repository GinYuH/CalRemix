using CalamityMod;
using CalamityMod.Items.Placeables.Ores;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class ScreenHelperManager : ModSystem
    {
        public static void LoadBossDeathMessages()
        {

            HelperMessage.New("AbyssBegin", "Every 60 seconds in the Abyss a hour passes by, truly wonderful!",
               "FannyNuhuh", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && NPC.downedBoss3).SetHoverTextOverride("Very interesting Fanny!");

            HelperMessage.New("Cryodeath", "Ha! Snow's over, Cryogen! Wasn't that pretty cool?",
               "FannyIdle", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && DownedBossSystem.downedCryogen);

            HelperMessage.New("FollyDeath", "You finally beat the Dragonfolly! Though, I don't know why you'd need pheromones though. I mean, you're already a handsome fellow...",
               "FannyNuhuh", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && DownedBossSystem.downedDragonfolly).SetHoverTextOverride("Uhm.... Thanks for the help, Fanny...?");

            HelperMessage.New("CalcloneDeath", "Oh it was just a clone.",
                "FannySob", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && DownedBossSystem.downedCalamitasClone);

            HelperMessage.New("Yharore", "Looks like the caverns have been laced with Auric Ore! The ore veins are pretty massive, so I’d say it’s best that you get up close and go hog wild!",
               "FannyIdle", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && DownedBossSystem.downedYharon).AddItemDisplay(ModContent.ItemType<AuricOre>());

            HelperMessage.New("DraedonExit", "Good golly! You did it! Though I'd HATE to imagine the financial losses caused by the destruction of those machines.",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && DownedBossSystem.downedExoMechs);

            HelperMessage.New("SCalDie", "That was exhilarating! Though that means the end of our adventure is upon us. What a Calamity as one may say!",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && DownedBossSystem.downedCalamitas);

            HelperMessage.New("Mechs", "Congrats on taking down those clanky contraptions! It's like defeating a bunch of oversized kitchen appliances. Just remember, don't get too cocky or they might just hit you with their spatulas of doom!",
               "FannyIdle", (ScreenHelperSceneMetrics scene) => NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3);

            HelperMessage.New("Golem", "Good job defeating that pile o' bricks! You sure.. cough cough wow, the air sure is du- cough cough",
               "FannyNuhuh", (ScreenHelperSceneMetrics scene) => NPC.downedGolemBoss);
        }
    }
}
