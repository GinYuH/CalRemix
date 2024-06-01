﻿using CalamityMod.NPCs;
using CalamityMod.NPCs.AstrumDeus;
using CalamityMod.NPCs.CalClone;
using CalamityMod.NPCs.ExoMechs;
using CalamityMod.NPCs.ExoMechs.Ares;
using CalamityMod.NPCs.ProfanedGuardians;
using CalamityMod.NPCs.SupremeCalamitas;
using CalamityMod.NPCs.Yharon;
using CalamityMod.World;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class FannyManager : ModSystem
    {
        public static void LoadBossMessages()
        {
            fannyMessages.Add(new FannyMessage("TorchGod", "A fellow being of the flames! It seems you played with a bit too much fire and now you are facing the wrath of the almighty Torch God! Don't worry though, he's impervious to damage, so you won't be able to hurt him.",
              "Awooga", (FannySceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == NPCID.TorchGod)));

            fannyMessages.Add(new FannyMessage("Wof", "EEYIKES, that thing is scary! Better use your Magic Mirror to get away!",
              "Sob", (FannySceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == NPCID.WallofFlesh) && Main.LocalPlayer.HasBuff(BuffID.Horrified)));

            fannyMessages.Add(new FannyMessage("QSFly", "THEY FLY NOW??",
              "Awooga", (FannySceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == NPCID.QueenSlimeBoss && n.life <= n.lifeMax / 2)).SetHoverTextOverride("THEY FLY NOW!!"));

            fannyMessages.Add(new FannyMessage("SkelePrime", "Is that THE Skeletron Prime?! Goodness, he sure is a whole lot scarier up close! Fear not my friend, if you wait this night out, he might just run his batteries dry and tip right over like the bucket of bolts he is!",
              "Nuhuh", (FannySceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == NPCID.SkeletronPrime) && !Main.masterMode || !CalamityWorld.death));
            
            fannyMessages.Add(new FannyMessage("OblivionPrime", "Hey, is it just me, or does this seem obliviously familiar to me? I sense a hint of inspiration here, someone was quite blahsphemous with the design of this bucket of bolts!",
              "Nuhuh", (FannySceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == NPCID.SkeletronPrime) && Main.masterMode && CalamityWorld.death));

            fannyMessages.Add(new FannyMessage("Calclone", "It is time. The Brimstone Witch, the one behind the Calamity in this world. You will now face Supreme Witch, Calamitas and end everything once and for all!",
               "Awooga", (FannySceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<CalamitasClone>())));

            FannyMessage pumpking1 = new FannyMessage("Pumpking1", "Wh- Ahh! AAAAAAAAAAAAH!!",
                "Awooga", (FannySceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == NPCID.Pumpking), 3, needsToBeClickedOff: false).SetHoverTextOverride("What?");

            fannyMessages.Add(pumpking1);

            FannyMessage pumpking2 = new FannyMessage("Pumpking2", "I told Fanny as a joke that jack-o-lanterns get their lights by eating flames. Don't tell him, though. It's funnier this way.",
                "EvilIdle", FannyMessage.AlwaysShow).AddDelay(2).NeedsActivation().SetHoverTextOverride("Sure? I might tell Fanny later...").SpokenByEvilFanny();

            pumpking1.AddEndEvent(() => pumpking2.ActivateMessage());

            fannyMessages.Add(pumpking2);

            // Empress
            FannyMessage eol1 = new FannyMessage("EoL1", "So, there's a second boss for the Hallow... Then where's the second boss for the other biomes? Did they just like this one more than the others?",
                "EvilIdle", (FannySceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == NPCID.HallowBoss), 8, needsToBeClickedOff: false, displayOutsideInventory: true).SpokenByEvilFanny();

            fannyMessages.Add(eol1);

            FannyMessage eol2 = new FannyMessage("EoL2", "... then again, the other boss is a recolor. Maybe for the best.",
                "EvilIdle", FannyMessage.AlwaysShow, 6, needsToBeClickedOff: false, displayOutsideInventory: true).SpokenByEvilFanny().NeedsActivation();

            eol1.AddEndEvent(() => eol2.ActivateMessage());

            fannyMessages.Add(eol2);

            FannyMessage eol3 = new FannyMessage("EoL3", "Just like you!",
                "Nuhuh", FannyMessage.AlwaysShow, 5, needsToBeClickedOff: false, displayOutsideInventory: true).NeedsActivation();

            eol2.AddEndEvent(() => eol3.ActivateMessage());

            fannyMessages.Add(eol3);

            FannyMessage eol4 = new FannyMessage("EoL4", "...",
                "EvilIdle", FannyMessage.AlwaysShow, 5, needsToBeClickedOff: false, displayOutsideInventory: true).SpokenByEvilFanny().NeedsActivation();

            eol3.AddEndEvent(() => eol4.ActivateMessage());

            fannyMessages.Add(eol4);

            FannyMessage eol5 = new FannyMessage("EoL5", "Bitch.",
                "EvilIdle", FannyMessage.AlwaysShow, 5, onlyPlayOnce: false, displayOutsideInventory: true).SpokenByEvilFanny().NeedsActivation();

            eol4.AddEndEvent(() => eol5.ActivateMessage());

            fannyMessages.Add(eol5);

            fannyMessages.Add(new FannyMessage("Deus", "It appears that you are once again fighting a large serpentine creature. Therefore, it's advisable to do what you've done with them before and blast it with fast single target weaponry!",
               "Idle", (FannySceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<AstrumDeusHead>())));

            fannyMessages.Add(new FannyMessage("DeusSplitMod", "This is getting out of hand! Now there are two of them!",
               "Awooga", (FannySceneMetrics scene) => !Main.zenithWorld && NPC.CountNPCS(ModContent.NPCType<AstrumDeusHead>()) > 1));

            fannyMessages.Add(new FannyMessage("PGuardians", "It seems like these mischievous scoundrels are up to no good, and plan to burn all the delicious meat! We gotta go put an end to their plan of calamity!",
                "Nuhuh", (FannySceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<ProfanedGuardianCommander>())));

            fannyMessages.Add(new FannyMessage("NewYork", "Oh, I saw that sky somewhere in my dreams! the place was called uhhh... New Yuck... Nu Yok.... New Yok.... yea something like that!",
                "Nuhuh", (FannySceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<Yharon>())).SetHoverTextOverride("It's called New York, Fanny! I'll take you there one day."));

            fannyMessages.Add(new FannyMessage("YharRebirth", "Good job friend you almost got him to half health!",
                "Awooga", (FannySceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<Yharon>() && n.life <= n.lifeMax * 2/3) && ModLoader.HasMod("YharonRebirth")));

            fannyMessages.Add(new FannyMessage("YharRebirth2", "Oh.",
                "Idle", (FannySceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<Yharon>() && n.ai[0] == 17f) && ModLoader.HasMod("YharonRebirth")));

            fannyMessages.Add(new FannyMessage("YharvelQuip", "Is it just me, or is it getting hot in here?",
                "Awooga", (FannySceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<Yharon>() && n.ai[0] == 17f) && !ModLoader.HasMod("YharonRebirth")));

            fannyMessages.Add(new FannyMessage("DraedonEnter", "Gee willikers! It's the real Draedon! He will soon present you with a difficult choice between three of your previous foes but with new attacks and increased difficulty. This appears to be somewhat of a common theme with this world dontcha think?",
               "Idle", (FannySceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<Draedon>())));

            fannyMessages.Add(new FannyMessage("AresGlue", "You ever noticed XF-09 Ares and myself are the only two characters that are glued onto on area of your screen beside yourself? That must make us glue-buddies!",
               "Awooga", (FannySceneMetrics scene) => !Main.zenithWorld && !ModLoader.HasMod("InfernumMode") && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<AresBody>())));

            fannyMessages.Add(new FannyMessage("ExoMayhem", "Wow! What a mayhem! Don't panic though, if you focus on dodging, you will be less likely to get hit. A common strategy for these tin cans is to \" fall god \", which I believe means summoning other gods like the Slime God and killing them for extra health. You should also pay extra attention to Ares' red cannon, because sometimes it can sweep across the screen, ruining your dodge flow. As for the twins, keep a close eye on the right one, as it has increased fire rate. There is no saving you from Thanatos, it isn't synced and breaks the structure these guys are allegedly supposed to have. Like seriously, why do the twins and Ares hover to the sides and above you while that robo-snake just does whatever the heckle heckity heckering hecky heck he wants? It would be significantly more logical if it tried to like stay below you, but no. Anyways, good luck buddy! You're almost at the end, you can do this!",
                "Idle", (FannySceneMetrics scene) => !Main.zenithWorld && CalamityGlobalNPC.draedonExoMechPrime != -1 && CalamityGlobalNPC.draedonExoMechTwinGreen != -1 && CalamityGlobalNPC.draedonExoMechWorm != -1, needsToBeClickedOff: false, duration: 22));

            fannyMessages.Add(new FannyMessage("Scaldie", "Are you hurt? That was calamitous!",
                "Sob", (FannySceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<SupremeCalamitas>()) && Main.LocalPlayer.dead));

            #region CrossMod

            fannyMessages.Add(new FannyMessage("BereftVassal", "Woah! That Argus guy has challenged you to a duel! Keep on your toes buddy, because they are sure as hell agile on their feet! Oh yeah, beware of his pet shark too..!",
                "Awooga", (FannySceneMetrics scene) => CrossModNPC(scene, "InfernumMode", "BereftVassal")));

            fannyMessages.Add(new FannyMessage("Goozma", "Holy cow! It's THE Goozma! An easy way to defeat this slippery menace is to lead him into shimmer.",
                "Awooga", (FannySceneMetrics scene) => CrossModNPC(scene, "CalamityHunt", "Goozma")));

            fannyMessages.Add(new FannyMessage("Astrageldon", "Woah, this boss seems a little strong for you! Maybe come back after you’ve killed the Moon Lord!",
                "Nuhuh", (FannySceneMetrics scene) => CrossModNPC(scene, "CatalystMod", "Astrageldon")).SetHoverTextOverride("Thanks you Fanny! I'll go kill the Moon Lord first."));

            fannyMessages.Add(new FannyMessage("Mutant", "Woah, how much HP does that guy have??",
                "Awooga", (FannySceneMetrics scene) => CrossModNPC(scene, "FargowiltasSouls", "MutantBoss")));

            fannyMessages.Add(new FannyMessage("ThoriumPrimordials", "WHOA! I didn't think that any Pre-Mordials were still alive! You're in for a tough fight! Killing them may awaken the legendary Dying Reality, a terrifying being that threatens our world! ",
                "Awooga", (FannySceneMetrics scene) => CrossModNPC(scene, "ThoriumMod", "SlagFury") || CrossModNPC(scene, "ThoriumMod", "Aquaius") || CrossModNPC(scene, "ThoriumMod", "Omnicide")));

            #endregion
        }
    }
}
