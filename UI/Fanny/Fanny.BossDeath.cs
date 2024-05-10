using CalamityMod;
using CalamityMod.Items.Placeables.Ores;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class FannyManager : ModSystem
    {
        public static void LoadBossDeathMessages()
        {

            fannyMessages.Add(new FannyMessage("AbyssBegin", "Every 60 seconds in the Abyss a hour passes by, truly wonderful!",
               "Nuhuh", (FannySceneMetrics scene) => !Main.zenithWorld && NPC.downedBoss3).SetHoverTextOverride("Very interesting Fanny!"));

            fannyMessages.Add(new FannyMessage("Cryodeath", "Ha! Snow's over, Cryogen! Wasn't that pretty cool?",
               "Idle", (FannySceneMetrics scene) => !Main.zenithWorld && DownedBossSystem.downedCryogen));

            fannyMessages.Add(new FannyMessage("FollyDeath", "You finally beat the Dragonfolly! Though, I don't know why you'd need pheromones though. I mean, you're already a handsome fellow...",
               "Nuhuh", (FannySceneMetrics scene) => !Main.zenithWorld && DownedBossSystem.downedDragonfolly).SetHoverTextOverride("Uhm.... Thanks for the help, Fanny...?"));

            fannyMessages.Add(new FannyMessage("CalcloneDeath", "Oh it was just a clone.",
                "Sob", (FannySceneMetrics scene) => !Main.zenithWorld && DownedBossSystem.downedCalamitasClone));

            fannyMessages.Add(new FannyMessage("Yharore", "Looks like the caverns have been laced with Auric Ore! The ore veins are pretty massive, so I’d say it’s best that you get up close and go hog wild!",
               "Idle", (FannySceneMetrics scene) => !Main.zenithWorld && DownedBossSystem.downedYharon).AddItemDisplay(ModContent.ItemType<AuricOre>()));

            fannyMessages.Add(new FannyMessage("DraedonExit", "Good golly! You did it! Though I'd HATE to imagine the financial losses caused by the destruction of those machines.",
                "Awooga", (FannySceneMetrics scene) => !Main.zenithWorld && DownedBossSystem.downedExoMechs));

            fannyMessages.Add(new FannyMessage("SCalDie", "That was exhilarating! Though that means the end of our adventure is upon us. What a Calamity as one may say!",
                "Awooga", (FannySceneMetrics scene) => !Main.zenithWorld && DownedBossSystem.downedCalamitas));

            fannyMessages.Add(new FannyMessage("Mechs", "Congrats on taking down those clanky contraptions! It's like defeating a bunch of oversized kitchen appliances. Just remember, don't get too cocky or they might just hit you with their spatulas of doom!",
               "Idle", (FannySceneMetrics scene) => NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3));

            FannyMessage plant = new FannyMessage("Plantoreum1", "Hey, have you seen my precious pink flower that I've been growing for 15 years? I left her around here. She's been my best friend for years now, and I could never fathom what I'd do if I had lost h",
               "Idle", (FannySceneMetrics scene) => NPC.downedPlantBoss);

            fannyMessages.Add(plant);
            FannyMessage plant2 = new FannyMessage("Plantoreum2", "Oh.",
               "Sob", FannyMessage.AlwaysShow, needsToBeClickedOff: false, duration: 30)
                .NeedsActivation();

            plant.AddStartEvent(() => plant2.ActivateMessage());

            fannyMessages.Add(plant2);

            fannyMessages.Add(new FannyMessage("Golem", "Good job defeating that pile o' bricks! You sure.. cough cough wow, the air sure is du- cough cough",
               "Nuhuh", (FannySceneMetrics scene) => NPC.downedGolemBoss));
        }
    }
}
