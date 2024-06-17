using CalamityMod.NPCs;
using CalamityMod.NPCs.AstrumDeus;
using CalamityMod.NPCs.CalClone;
using CalamityMod.NPCs.DevourerofGods;
using CalamityMod.NPCs.ExoMechs;
using CalamityMod.NPCs.ExoMechs.Ares;
using CalamityMod.NPCs.ProfanedGuardians;
using CalamityMod.NPCs.SupremeCalamitas;
using CalamityMod.NPCs.Yharon;
using CalamityMod.World;
using System;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class ScreenHelperManager : ModSystem
    {
        public static void LoadBossMessages()
        {
            screenHelperMessages.Add(new HelperMessage("TorchGod", "A fellow being of the flames! It seems you played with a bit too much fire and now you are facing the wrath of the almighty Torch God! Don't worry though, he's impervious to damage, so you won't be able to hurt him.",
              "FannyAwooga", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == NPCID.TorchGod)));

            screenHelperMessages.Add(new HelperMessage("Wof", "EEYIKES, that thing is scary! Better use your Magic Mirror to get away!",
              "FannySob", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == NPCID.WallofFlesh) && Main.LocalPlayer.HasBuff(BuffID.Horrified)));

            screenHelperMessages.Add(new HelperMessage("QSFly", "THEY FLY NOW??",
              "FannyAwooga", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == NPCID.QueenSlimeBoss && n.life <= n.lifeMax / 2)).SetHoverTextOverride("THEY FLY NOW!!"));

            screenHelperMessages.Add(new HelperMessage("SkelePrime", "Is that THE Skeletron Prime?! Goodness, he sure is a whole lot scarier up close! Fear not my friend, if you wait this night out, he might just run his batteries dry and tip right over like the bucket of bolts he is!",
              "FannyNuhuh", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == NPCID.SkeletronPrime) && (!Main.masterMode || !CalamityWorld.death)));
            
            screenHelperMessages.Add(new HelperMessage("OblivionPrime", "Hey, is it just me, or does this seem obliviously familiar to me? I sense a hint of inspiration here, someone was quite blahsphemous with the design of this bucket of bolts!",
              "FannyNuhuh", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == NPCID.SkeletronPrime) && Main.masterMode && CalamityWorld.death));

            screenHelperMessages.Add(new HelperMessage("Calclone", "It is time. The Brimstone Witch, the one behind the Calamity in this world. You will now face Supreme Witch, Calamitas and end everything once and for all!",
               "FannyAwooga", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<CalamitasClone>())));

            screenHelperMessages.Add(new HelperMessage("Pumpking1", "Wh- Ahh! AAAAAAAAAAAAH!!",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == NPCID.Pumpking), 3, needsToBeClickedOff: false).SetHoverTextOverride("What?"));

            screenHelperMessages.Add(new HelperMessage("Pumpking2", "I told Fanny as a joke that jack-o-lanterns get their lights by eating flames. Don't tell him, though. It's funnier this way.",
                "EvilFannyIdle", HelperMessage.AlwaysShow).ChainAfter(delay: 2).SetHoverTextOverride("Sure? I might tell Fanny later...").SpokenByEvilFanny());

            // Empress
            screenHelperMessages.Add(new HelperMessage("EoL1", "So, there's a second boss for the Hallow... Then where's the second boss for the other biomes? Did they just like this one more than the others?",
                "EvilFannyIdle", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == NPCID.HallowBoss), 8, needsToBeClickedOff: false, displayOutsideInventory: true).SpokenByEvilFanny());

            screenHelperMessages.Add(new HelperMessage("EoL2", "... then again, the other boss is a recolor. Maybe for the best.",
                "EvilFannyIdle", HelperMessage.AlwaysShow, 6, needsToBeClickedOff: false, displayOutsideInventory: true).SpokenByEvilFanny().ChainAfter());

            screenHelperMessages.Add(new HelperMessage("EoL3", "Just like you!",
                "FannyNuhuh", HelperMessage.AlwaysShow, 5, needsToBeClickedOff: false, displayOutsideInventory: true).ChainAfter());

            screenHelperMessages.Add(new HelperMessage("EoL4", "...",
                "EvilFannyIdle", HelperMessage.AlwaysShow, 5, needsToBeClickedOff: false, displayOutsideInventory: true).SpokenByEvilFanny().ChainAfter());

            screenHelperMessages.Add(new HelperMessage("EoL5", "Bitch.",
                "EvilFannyIdle", HelperMessage.AlwaysShow, 5, onlyPlayOnce: false, displayOutsideInventory: true).SpokenByEvilFanny().ChainAfter());


            screenHelperMessages.Add(new HelperMessage("Deus", "It appears that you are once again fighting a large serpentine creature. Therefore, it's advisable to do what you've done with them before and blast it with fast single target weaponry!",
               "FannyIdle", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<AstrumDeusHead>())));

            screenHelperMessages.Add(new HelperMessage("DeusSplitMod", "This is getting out of hand! Now there are two of them!",
               "FannyAwooga", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && NPC.CountNPCS(ModContent.NPCType<AstrumDeusHead>()) > 1));

            screenHelperMessages.Add(new HelperMessage("PGuardians", "It seems like these mischievous scoundrels are up to no good, and plan to burn all the delicious meat! We gotta go put an end to their plan of calamity!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<ProfanedGuardianCommander>())));

            screenHelperMessages.Add(new HelperMessage("NoArmorDog", "Woah there, $0! Seems like you forgot to put on your favorite set of armor before fighting this boss! We don't want you to pull a Cheeseboy, do we?",
                "FannySob", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<DevourerofGodsHead>()) && NoArmor()).AddDynamicText(HelperMessage.GetPlayerName));

            screenHelperMessages.Add(new HelperMessage("NewYork", "Oh, I saw that sky somewhere in my dreams! the place was called uhhh... New Yuck... Nu Yok.... New Yok.... yea something like that!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<Yharon>())).SetHoverTextOverride("It's called New York, Fanny! I'll take you there one day."));

            screenHelperMessages.Add(new HelperMessage("YharRebirth", "Good job friend you almost got him to half health!",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<Yharon>() && n.life <= n.lifeMax * 2/3) && ModLoader.HasMod("YharonRebirth")));

            screenHelperMessages.Add(new HelperMessage("YharRebirth2", "Oh.",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<Yharon>() && n.ai[0] == 17f) && ModLoader.HasMod("YharonRebirth")));

            screenHelperMessages.Add(new HelperMessage("YharvelQuip", "Is it just me, or is it getting hot in here?",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<Yharon>() && n.ai[0] == 17f) && !ModLoader.HasMod("YharonRebirth")));

            screenHelperMessages.Add(new HelperMessage("DraedonEnter", "Gee willikers! It's the real Draedon! He will soon present you with a difficult choice between three of your previous foes but with new attacks and increased difficulty. This appears to be somewhat of a common theme with this world dontcha think?",
               "FannyIdle", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<Draedon>())));

            screenHelperMessages.Add(new HelperMessage("AresGlue", "You ever noticed XF-09 Ares and myself are the only two characters that are glued onto on area of your screen beside yourself? That must make us glue-buddies!",
               "FannyAwooga", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && !ModLoader.HasMod("InfernumMode") && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<AresBody>())));

            screenHelperMessages.Add(new HelperMessage("ExoMayhem", "Wow! What a mayhem! Don't panic though, if you focus on dodging, you will be less likely to get hit. A common strategy for these tin cans is to \" fall god \", which I believe means summoning other gods like the Slime God and killing them for extra health. You should also pay extra attention to Ares' red cannon, because sometimes it can sweep across the screen, ruining your dodge flow. As for the twins, keep a close eye on the right one, as it has increased fire rate. There is no saving you from Thanatos, it isn't synced and breaks the structure these guys are allegedly supposed to have. Like seriously, why do the twins and Ares hover to the sides and above you while that robo-snake just does whatever the heckle heckity heckering hecky heck he wants? It would be significantly more logical if it tried to like stay below you, but no. Anyways, good luck buddy! You're almost at the end, you can do this!",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && CalamityGlobalNPC.draedonExoMechPrime != -1 && CalamityGlobalNPC.draedonExoMechTwinGreen != -1 && CalamityGlobalNPC.draedonExoMechWorm != -1, needsToBeClickedOff: false, duration: 22));

            screenHelperMessages.Add(new HelperMessage("Scaldie", "Are you hurt? That was calamitous!",
                "FannySob", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<SupremeCalamitas>()) && Main.LocalPlayer.dead));

            #region CrossMod

            screenHelperMessages.Add(new HelperMessage("BereftVassal", "Woah! That Argus guy has challenged you to a duel! Keep on your toes buddy, because they are sure as hell agile on their feet! Oh yeah, beware of his pet shark too..!",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => CrossModNPC(scene, "InfernumMode", "BereftVassal")));

            screenHelperMessages.Add(new HelperMessage("Goozma", "Holy cow! It's THE Goozma! An easy way to defeat this slippery menace is to lead him into shimmer.",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => CrossModNPC(scene, "CalamityHunt", "Goozma")));

            screenHelperMessages.Add(new HelperMessage("Astrageldon", "Woah, this boss seems a little strong for you! Maybe come back after you’ve killed the Moon Lord!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => CrossModNPC(scene, "CatalystMod", "Astrageldon")).SetHoverTextOverride("Thanks you Fanny! I'll go kill the Moon Lord first."));

            screenHelperMessages.Add(new HelperMessage("Mutant", "Woah, how much HP does that guy have??",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => CrossModNPC(scene, "FargowiltasSouls", "MutantBoss") && EnragedMutant()));

            screenHelperMessages.Add(new HelperMessage("ThoriumPrimordials", "WHOA! I didn't think that any Pre-Mordials were still alive! You're in for a tough fight! Killing them may awaken the legendary Dying Reality, a terrifying being that threatens our world! ",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => CrossModNPC(scene, "ThoriumMod", "SlagFury") || CrossModNPC(scene, "ThoriumMod", "Aquaius") || CrossModNPC(scene, "ThoriumMod", "Omnicide")));

            #endregion
        }
        private static bool NoArmor()
        {
            return Main.LocalPlayer.armor[0].type == ItemID.None && Main.LocalPlayer.armor[1].type == ItemID.None && Main.LocalPlayer.armor[2].type == ItemID.None;
        }
        private static bool EnragedMutant()
        {
            if (ModLoader.TryGetMod("FargowiltasSouls", out Mod f))
            {
                Type worldSystem = null;
                Assembly fargoAssembly = f.GetType().Assembly;
                bool enraged = false;
                foreach (Type t in fargoAssembly.GetTypes())
                {
                    if (t.Name == "WorldSavingSystem")
                        worldSystem = t;
                }
                if (worldSystem != null)
                {
                    PropertyInfo angryProperty = worldSystem.GetProperty("AngryMutant", BindingFlags.Public | BindingFlags.Static);
                    enraged = (bool)angryProperty.GetValue(null);
                }
                if (enraged)
                    return true;
            }
            return false;
        }
    }
}
