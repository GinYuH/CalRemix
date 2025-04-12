using CalamityMod;
using CalamityMod.NPCs;
using CalamityMod.NPCs.AstrumDeus;
using CalamityMod.NPCs.CalClone;
using CalamityMod.NPCs.DevourerofGods;
using CalamityMod.NPCs.ExoMechs;
using CalamityMod.NPCs.ExoMechs.Ares;
using CalamityMod.NPCs.Providence;
using CalamityMod.NPCs.ProfanedGuardians;
using CalamityMod.NPCs.SupremeCalamitas;
using CalamityMod.NPCs.Yharon;
using CalamityMod.World;
using CalamityMod.Tiles.Ores;
using CalRemix.Core.World;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CalRemix.UI
{
    public partial class ScreenHelperManager : ModSystem
    {
        public static void LoadBossMessages()
        {
            HelperMessage.New("TorchGod", "A fellow being of the flames! It seems you played with a bit too much fire and now you are facing the wrath of the almighty Torch God! Don't worry though, he's impervious to damage, so you won't be able to hurt him.",
              "FannyAwooga", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == NPCID.TorchGod));

            HelperMessage.New("Wof", "EEYIKES, that thing is scary! Better use your Magic Mirror to get away!",
              "FannySob", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == NPCID.WallofFlesh) && Main.LocalPlayer.HasBuff(BuffID.Horrified));

            HelperMessage.New("QSFly", "THEY FLY NOW??",
              "FannyAwooga", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == NPCID.QueenSlimeBoss && n.life <= n.lifeMax / 2)).SetHoverTextOverride("THEY FLY NOW!!");

            HelperMessage.New("SkelePrime", "Is that THE Skeletron Prime?! Goodness, he sure is a whole lot scarier up close! Fear not my friend, if you wait this night out, he might just run his batteries dry and tip right over like the bucket of bolts he is!",
              "FannyNuhuh", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == NPCID.SkeletronPrime) && (!Main.masterMode || !CalamityWorld.death));
            
            HelperMessage.New("OblivionPrime", "Hey, is it just me, or does this seem obliviously familiar to me? I sense a hint of inspiration here, someone was quite blahsphemous with the design of this bucket of bolts!",
              "FannyNuhuh", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == NPCID.SkeletronPrime) && Main.masterMode && CalamityWorld.death);

            HelperMessage.New("Calclone", "It is time. The Brimstone Witch, the one behind the Calamity in this world. You will now face Supreme Witch, Calamitas and end everything once and for all!",
               "FannyAwooga", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<CalamitasClone>()));

            HelperMessage.New("Pumpking1", "Wh- Ahh! AAAAAAAAAAAAH!!",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == NPCID.Pumpking), 3, cantBeClickedOff: true).SetHoverTextOverride("What's wrong, Fanny?").InitiateConversation();

            HelperMessage.New("Pumpking2", "I told Fanny as a joke that jack-o-lanterns get their lights by eating flames. Don't tell him, though. It's funnier this way.",
                "EvilFannyPoint").ChainAfter(delay: 2).SetHoverTextOverride("Sure? I might tell Fanny later...").SpokenByEvilFanny().EndConversation();

            HelperMessage.New("EoL1", "So, there's a second boss for the Hallow... Then where's the second boss for the other biomes? Did they just like this one more than the others?",
                "EvilFannyIdle", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == NPCID.HallowBoss), 8, cantBeClickedOff: true).SpokenByEvilFanny().InitiateConversation();

            HelperMessage.New("EoL2", "... then again, the other boss is a recolor. Maybe for the best.",
                "EvilFannyIdle", HelperMessage.AlwaysShow, 6, cantBeClickedOff: true).SpokenByEvilFanny().ChainAfter();

            HelperMessage.New("EoL3", "Just like you!",
                "FannyNuhuh", HelperMessage.AlwaysShow, 5, cantBeClickedOff: true).ChainAfter(delay: 5, startTimerOnMessageSpoken: true);

            HelperMessage.New("EoL4", "...",
                "EvilFannyMiffed", HelperMessage.AlwaysShow, 5, cantBeClickedOff: true).SpokenByEvilFanny().ChainAfter(delay: 3, startTimerOnMessageSpoken: true);

            HelperMessage.New("EoL5", "Bitch.",
                "EvilFannyMiffed", HelperMessage.AlwaysShow, 5, onlyPlayOnce: false).SpokenByEvilFanny().ChainAfter(delay: 0, startTimerOnMessageSpoken: true).EndConversation();

            HelperMessage.New("Abomination1", "Chat, one of the CEO of Remix decided to commit a delulu and shame me for flicker-gooning to 56 Giant Illumina Woman gyatt image tab, how tf do I doxx the CEO of this mod bruh... I wanna make him mic up against my giga-sigma phonk master rap skill, bruh.",
                "CrimSonDefault", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == NPCID.HallowBoss && n.life <= n.lifeMax * 1 / 10)).SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon).InitiateConversation();

            HelperMessage.New("Abomination2", "I want nothing more than to punt you off of a cliff.",
                "EvilFannyDisgusted", HelperMessage.AlwaysShow, 5, cantBeClickedOff: true).SpokenByEvilFanny().ChainAfter(delay: 5, startTimerOnMessageSpoken: true).EndConversation();

            HelperMessage.New("Suffer", "Only god can save you now.",
                "EvilFannyIdle", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == NPCID.HallowBoss && n.AI_120_HallowBoss_IsGenuinelyEnraged())).SpokenByEvilFanny();

            HelperMessage.New("Deus", "It appears that you are once again fighting a large serpentine creature. Therefore, it's advisable to do what you've done with them before and blast it with fast single target weaponry!",
               "FannyIdle", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<AstrumDeusHead>()));

            HelperMessage.New("DeusSplitMod", "This is getting out of hand! Now there are two of them!",
               "FannyAwooga", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && NPC.CountNPCS(ModContent.NPCType<AstrumDeusHead>()) > 1);

            HelperMessage.New("PGuardians", "It seems like these mischievous scoundrels are up to no good, and plan to burn all the delicious meat! We gotta go put an end to their plan of calamity!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<ProfanedGuardianCommander>()));

            HelperMessage.New("ProviRefight", "Hang on, didn't we already do this? You should probably move onto a new boss instead of refighting this one.",
                "EvilFannyIdle", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && NPC.downedEmpressOfLight && scene.onscreenNPCs.Any (n=> n.type == ModContent.NPCType<Providence>() && n.life <= n.lifeMax * 4 / 5), 8).AddStartEvent(ProviSkip).SetHoverTextOverride("That's a good point, actually...").SpokenByEvilFanny();

            HelperMessage.New("NoArmorDog", "Woah there, $0! Seems like you forgot to put on your favorite set of armor before fighting this boss! We don't want you to pull a Cheeseboy, do we?",
                "FannySob", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<DevourerofGodsHead>()) && NoArmor()).AddDynamicText(HelperMessage.GetPlayerName);

            HelperMessage.New("NewYork", "Oh, I saw that sky somewhere in my dreams! the place was called uhhh... New Yuck... Nu Yok.... New Yok.... yea something like that!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && !CalRemixWorld.npcChanges && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<Yharon>())).SetHoverTextOverride("It's called New York, Fanny! I'll take you there one day.");

            HelperMessage.New("YharRebirth", "Good job friend, you've almost gotten him to half health!",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<Yharon>() && n.life <= n.lifeMax * 2/3) && ModLoader.HasMod("YharonRebirth"));

            HelperMessage.New("YharRebirth2", "Oh.",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<Yharon>() && n.ai[0] == 17f) && ModLoader.HasMod("YharonRebirth"));

            HelperMessage.New("YharvelQuip", "Is it just me, or is it getting hot in here?",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<Yharon>() && n.ai[0] == 17f) && !ModLoader.HasMod("YharonRebirth"));

            HelperMessage.New("DraedonEnter", "Gee willikers! It's the real Draedon! He will soon present you with a difficult choice between three of your previous foes but with new attacks and increased difficulty. This appears to be somewhat of a common theme with this world dontcha think?",
               "FannyIdle", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<Draedon>()));

            HelperMessage.New("AresGlue", "You ever noticed XF-09 Ares and myself are the only two characters that are glued onto on area of your screen beside yourself? That must make us glue-buddies!",
               "FannyAwooga", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && CalRemixAddon.Infernum == null && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<AresBody>() && n.life < (int)(n.lifeMax * 0.4f))).InitiateConversation();

            HelperMessage.New("AresGlue2", "So do I just like, not exist to you during battles, or something? I'm pretty sure I've been giving my commentary just as often as you have.",
               "EvilFannyIdle").SpokenByEvilFanny().ChainAfter(delay: 6, startTimerOnMessageSpoken: true);

            HelperMessage.New("AresGlue3", "Mein friends! I see you've forgotten about me, have you? No worries; I will ensure this does not happen again, with a whimsical fable from my past!",
               "MiracleBoyIdle", duration: 6, cantBeClickedOff: true).SpokenByAnotherHelper(ScreenHelpersUIState.MiracleBoy).ChainAfter(delay: 4, startTimerOnMessageSpoken: true);

            HelperMessage.New("AresGlue4", "Oho, this should prove quite comedic indeed! When I was a boy, many years ago- I had gotten stuck in un stone! You are all probably wondering how this happened, yes? Well, once upon a time, I was just a cloud! It must be hard to believe this, as I am your truest friend of all, but it is true! It was much a magical moment when I began to walk and talk; I took a trip to the deepest caverns! I found myself within a large city; the biggest I have ever seen. Previously, the record was zero! It is very hard to believe, but there was many people, and I began to help by giving them information about everything I knew, which is many things, yes! They very much appreciated this, and I was quickly appointed to community service due to my incredible many facts, which were very well recieved by all who listened to them! It is hard to believe that I had such a position, but the story does not end there! Once I did my part, the mayor himself, a yellow person, decided to promote me further, suggesting I assist the earth itself! With his help, I was pushed into the ground, where I began to tell magical tales to all who would listen, which was very many people- then you freed me, and we became inseparable allies! And that, my friends, is how we met! It was a truly whimsical journey, yes? I hope all of you have enjoyed it!",
               "MiracleBoyIdle", duration: 30, cantBeClickedOff: true).SpokenByAnotherHelper(ScreenHelpersUIState.MiracleBoy).ChainAfter();

            HelperMessage.New("AresGlue5", "... You're trying too hard, man.",
               "EvilFannyIdle").SpokenByEvilFanny().ChainAfter(delay: 26, startTimerOnMessageSpoken: true).EndConversation();

            HelperMessage.New("ExoMayhem", "Wow! What a mayhem! Don't panic though, if you focus on dodging, you will be less likely to get hit. A common strategy for these tin cans is to \" fall god \", which I believe means summoning other gods like the Slime God and killing them for extra health. You should also pay extra attention to Ares' red cannon, because sometimes it can sweep across the screen, ruining your dodge flow. As for the twins, keep a close eye on the right one, as it has increased fire rate. There is no saving you from Thanatos, it isn't synced and breaks the structure these guys are allegedly supposed to have. Like seriously, why do the twins and Ares hover to the sides and above you while that robo-snake just does whatever the heckle heckity heckering hecky heck he wants? It would be significantly more logical if it tried to like stay below you, but no. Anyways, good luck buddy! You're almost at the end, you can do this!",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && CalamityGlobalNPC.draedonExoMechPrime != -1 && CalamityGlobalNPC.draedonExoMechTwinGreen != -1 && CalamityGlobalNPC.draedonExoMechWorm != -1, cantBeClickedOff: true, duration: 22);

            HelperMessage.New("Scaldie", "Are you hurt? That was calamitous!",
                "FannySob", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<SupremeCalamitas>()) && Main.LocalPlayer.dead);

            #region CrossMod

            HelperMessage.New("BereftVassal", "Woah! That Argus guy has challenged you to a duel! Keep on your toes buddy, because they are sure as hell agile on their feet! Oh yeah, beware of his pet shark too..!",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => CrossModNPC(scene, "InfernumMode", "BereftVassal"));

            HelperMessage.New("Goozma", "Holy cow! It's THE Goozma! An easy way to defeat this slippery menace is to lead him into shimmer.",
                "FannyAwe", (ScreenHelperSceneMetrics scene) => CrossModNPC(scene, "CalamityHunt", "Goozma"));

            HelperMessage.New("Astrageldon", "Woah, this boss seems a little strong for you! Maybe come back after you’ve killed the Moon Lord!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => CrossModNPC(scene, "CatalystMod", "Astrageldon")).SetHoverTextOverride("Thanks you Fanny! I'll go kill the Moon Lord first.");

            HelperMessage.New("Mutant", "Woah, how much HP does that guy have??",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => CrossModNPC(scene, "FargowiltasSouls", "MutantBoss") && EnragedMutant());

            HelperMessage.New("ThoriumPrimordials", "WHOA! I didn't think that any Pre-Mordials were still alive! You're in for a tough fight! Killing them may awaken the legendary Dying Reality, a terrifying being that threatens our world! ",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => CrossModNPC(scene, "ThoriumMod", "SlagFury") || CrossModNPC(scene, "ThoriumMod", "Aquaius") || CrossModNPC(scene, "ThoriumMod", "Omnicide"));

            HelperMessage.New("AvatarRiftAppear", "n  meRE }plant{ can sTop t e inEvItaeb|e",
                "BizarroFannyIdle", (ScreenHelperSceneMetrics scene) => CrossModNPC(scene, "NoxusBoss", "AvatarRift"), 12, cantBeClickedOff: true).SpokenByAnotherHelper(ScreenHelpersUIState.BizarroFanny);

            HelperMessage.New("AoEAppear", "TTTh the (timE) HA5 ** come     FOR us 222]]2222 be] FULfilEd}}",
                "BizarroFannyIdle", (ScreenHelperSceneMetrics scene) => CrossModNPC(scene, "NoxusBoss", "AvatarOfEmptiness"), 12, cantBeClickedOff: true).SpokenByAnotherHelper(ScreenHelpersUIState.BizarroFanny);

            #endregion
        }
        private static bool NoArmor() => Main.LocalPlayer.armor[0].type == ItemID.None && Main.LocalPlayer.armor[1].type == ItemID.None && Main.LocalPlayer.armor[2].type == ItemID.None;
        private static bool EnragedMutant()
        {
            if (CalRemixAddon.FargoSoulsAngryMutant != null)
                return (bool)CalRemixAddon.FargoSoulsAngryMutant.GetValue(null);
            return false;
        }
        private static void ProviSkip()
        {

            for (int q = 0; q < Terraria.Main.npc.Length - 1; q++)
            {
                if (Terraria.Main.npc[q].type == ModContent.NPCType<Providence>())
                {
                    Terraria.Main.npc[q].active = false;
                }
            }
            CalamityUtils.DisplayLocalizedText("Providence, the Profaned Goddess has been defeated!", new Color(175, 75, 255));
            string key2 = "Mods.CalamityMod.Status.Progression.ProfanedBossText3";
            Color messageColor2 = Color.Orange;
            CalamityUtils.DisplayLocalizedText(key2, messageColor2);
            if (!CalRemixWorld.reargar) { 
                string key3 = "Mods.CalamityMod.Status.Progression.TreeOreText";
                Color messageColor3 = Color.LightGreen;
                CalamityUtils.SpawnOre(ModContent.TileType<UelibloomOre>(), 17E-05, 0.55f, 0.9f, 8, 14, TileID.Mud);
                CalamityUtils.DisplayLocalizedText(key3, messageColor3);

            }

            DownedBossSystem.downedProvidence = true;
            CalamityNetcode.SyncWorld();
        }
    }
}
