using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.NPCs.Cryogen;
using CalamityMod.NPCs.OldDuke;
using CalRemix.Content.Items.Placeables.MusicBoxes;
using CalRemix.Content.NPCs;
using CalRemix.Content.NPCs.Bosses.Acideye;
using CalRemix.Content.NPCs.Bosses.Carcinogen;
using CalRemix.Content.NPCs.Bosses.Hydrogen;
using CalRemix.Content.NPCs.Bosses.Hypnos;
using CalRemix.Content.NPCs.Bosses.Ionogen;
using CalRemix.Content.NPCs.Bosses.Origen;
using CalRemix.Content.NPCs.Bosses.Oxygen;
using CalRemix.Content.NPCs.Bosses.Pathogen;
using CalRemix.Content.NPCs.Bosses.Pyrogen;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static CalRemix.UI.HelperMessage;

namespace CalRemix.UI
{
    public class MoodTracker : InfoDisplay
	{
        public static bool unlocked = false;
        public static FannyMood lingeringMood = null;
        public static float moodLingerTimer = 0f;

        public override bool Active() => Main.LocalPlayer.GetModPlayer<ScreenHelperSavePlayer>().unlockedMoodTracker;
        public override string Texture => "CalRemix/UI/Fanny/HelperDisplay";
        public static int DisplayID;

        public static HelperMessage firstBlockDisableMessage;
        public static HelperMessage secondBlockDisableMessage;
        public static FannyMood secondBlockMood;
        public static FannyMood remorsefulMood;


        public static List<FannyMood> moodList = new List<FannyMood>();
        public static List<FannyMood> npcDependentMoodList = new List<FannyMood>();

        public enum PriorityClass
        {
            /// <summary>
            /// Used for the "Chummy" by default, and other really baseline things, like "nostalgic" when the retroman is equipped
            /// </summary>
            Baseline = 1,
            /// <summary>
            /// Used by the "damp" feeling when in water
            /// </summary>
            EnvironmentalMood = 2,
            /// <summary>
            /// Used by the "AFRAID" feeling when in an eclipse
            /// </summary>
            EventMood = 3,
            /// <summary>
            /// Used for the moods linked to misc npcs being on screen
            /// </summary>
            OnscreenNPCsMood = 4,
            /// <summary>
            /// Used for the moods linked to other helpers talking
            /// </summary>
            OtherHelperMood = 6,
        }


        public static void Reset()
        {
            NPC.downedDeerclops = false;
            Main.LocalPlayer.GetModPlayer<ScreenHelperSavePlayer>().unlockedMoodTracker = false;
            foreach (HelperMessage msg in ScreenHelperManager.screenHelperMessages)
                if (msg.Identifier == "MoodDisplayUnlock")
                    msg.alreadySeen = false;
            secondBlockDisableMessage.alreadySeen = true;
            firstBlockDisableMessage.alreadySeen = false;
        }

        public override void Load()
        {
            moodList = new List<FannyMood>();
        }

        public override void SetStaticDefaults()
        {
            if (Main.dedServ)
                return;

            DisplayID = Type;

            HelperMessage.New("MoodDisplayUnlock",
                "I feel like we've became such good pals, but I don't know if you truly understand how i feel sometimes... Fret not, for i've upgraded your interface with a new indicator that should inform you about my mood at anytime! Be sure to keep an eye out for any \"special\" moods that indicate rare loot!",
                "FannyIdle", CanUnlockMoodTracker).
                AddStartEvent(() => Main.LocalPlayer.GetModPlayer<ScreenHelperSavePlayer>().unlockedMoodTracker = true);

            firstBlockDisableMessage = HelperMessage.New("MoodDisplayBlockDisable",
                "I'm sorry my friend, but I can't let you do that! Bonds are forged through mutual understanding, and this moodtracker is vital to taking our friendship to the next level! Always remember the three big rules of healthy communication: Wear your heart on your sleeve, communicate problems, and don't try disabling the mood tracker!",
                "FannyNuhuh", PlayerHidTracker).AddDelay().
                SetHoverTextOverride("Ohhmigosh, that was a mislick! I appreciate you correcting that for me, Fanny!").
                AddStartEvent(UnhideTrackerForTheFirstTime);

            secondBlockDisableMessage = HelperMessage.New("MoodDisplayBlockDisableTwo",
                "Okay buddy, come on. Fuck you. I'm trying to be a good friend and you keep kicking me down. Is that who you really are inside? You are a deeply sick individual.",
                "FannyCryptid", null, 15, cantBeClickedOff:true).NeedsActivation().AddStartEvent(() => secondBlockMood.Activate());
            secondBlockDisableMessage.alreadySeen = true;

            FannyMood.New("Educated!", (ScreenHelperSceneMetrics metrics) => ScreenHelpersUIState.FannyTheFire.Speaking, 0.5f, 5f, true);

            FannyMood.New("Cheerful!", (ScreenHelperSceneMetrics metrics) => Main.LocalPlayer.HasBuff(BuffID.OnFire) ||
                Main.LocalPlayer.HasBuff(BuffID.CursedInferno) ||
                Main.LocalPlayer.HasBuff(BuffID.Frostburn) ||
                Main.LocalPlayer.HasBuff(BuffID.Burning) ||
                Main.LocalPlayer.HasBuff(BuffID.ShadowFlame) ||
                Main.LocalPlayer.HasBuff(BuffID.OnFire3) ||
                Main.LocalPlayer.HasBuff(BuffID.Frostburn2) ||
                Main.LocalPlayer.HasBuff(ModContent.BuffType<Nightwither>()) ||
                Main.LocalPlayer.HasBuff(ModContent.BuffType<AstralInfectionDebuff>()) ||
                Main.LocalPlayer.HasBuff(ModContent.BuffType<BrimstoneFlames>()) ||
                Main.LocalPlayer.HasBuff(ModContent.BuffType<Dragonfire>()) ||
                Main.LocalPlayer.HasBuff(ModContent.BuffType<HolyFlames>()) ||
                Main.LocalPlayer.HasBuff(ModContent.BuffType<VulnerabilityHex>()) ||
                Main.LocalPlayer.HasBuff(ModContent.BuffType<GodSlayerInferno>()) ||
                Main.LocalPlayer.HasBuff(ModContent.BuffType<BurningBlood>()) ||
                Main.LocalPlayer.HasBuff(ModContent.BuffType<HolyInferno>()) ||
                Main.LocalPlayer.HasBuff(ModContent.BuffType<MiracleBlight>()) ||
                Main.LocalPlayer.HasBuff(ModContent.BuffType<SearingLava>()) ||
                Main.LocalPlayer.HasBuff(ModContent.BuffType<ManaBurn>()),
                PriorityClass.EnvironmentalMood, 3f);

            FannyMood.New("Damp...", (ScreenHelperSceneMetrics metrics) => Main.LocalPlayer.wet || Main.LocalPlayer.HasBuff(BuffID.Wet) || Main.LocalPlayer.HasBuff(BuffID.Slimed),
                PriorityClass.EnvironmentalMood, 0f, true);
            //Boss about to spawn
            FannyMood.New("Agitated...", (ScreenHelperSceneMetrics metrics) => WorldGen.spawnEye || WorldGen.spawnHardBoss > 0,
                PriorityClass.EventMood, 0f, true);


            //"Good" or "Bad" content is onscreen. Needs to be filled out
            FannyMood.New("Ecstatic!!!", null, PriorityClass.OnscreenNPCsMood, 0f, true).
                AddRequiredNPC(ModContent.NPCType<Hypnos>(), ModContent.NPCType<AcidEye>());
            FannyMood.New("Impoverished...", null, PriorityClass.OnscreenNPCsMood, 0f, true).
                AddRequiredNPC(ModContent.NPCType<OgsculianBurrower>(), ModContent.NPCType<OldDuke>());

            FannyMood.New("GENuine fear!", null, 4.5f, 0f, false).
                AddRequiredNPC(ModContent.NPCType<Cryogen>(), 
                ModContent.NPCType<Pyrogen>(), 
                ModContent.NPCType<Hydrogen>(),
                ModContent.NPCType<Oxygen>(),
                ModContent.NPCType<Carcinogen>(), 
                ModContent.NPCType<Pathogen>(),
                ModContent.NPCType<Ionogen>(),
                ModContent.NPCType<Origen>(),
                NPCID.SkeletronHead,
                NPCID.SkeletronPrime);


            //Ticklish
            FannyMood.New("HARASSED!!!!!", (ScreenHelperSceneMetrics metrics) => ScreenHelpersUIState.FannyTheFire.tickle > 4f,
               10f, 20f, false);

            //RETRO!
            FannyMood.New("Nostalgic!", (ScreenHelperSceneMetrics metrics) => Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().retroman,
               PriorityClass.Baseline, 0f, true);

            FannyMood.New("AFRAID!", (ScreenHelperSceneMetrics metrics) => Main.eclipse,
               PriorityClass.EventMood, 0f, true);

            //Wotg
            if (ModLoader.HasMod("NoxusBoss"))
            {
                if (ModLoader.GetMod("NoxusBoss").TryFind("AvatarRift", out ModNPC rift))
                {
                    FannyMood.New("Rustled!...", null, 10f, 0f, false).
                        AddRequiredNPC(rift.Type);
                }
                if (ModLoader.GetMod("NoxusBoss").TryFind("AvatarOfEmptiness", out ModNPC avatar))
                {
                    FannyMood.New("Numb!", null, 10f, 0f, false).
                        AddRequiredNPC(avatar.Type);
                }
                if (ModLoader.GetMod("NoxusBoss").TryFind("namelessID", out ModNPC god))
                {
                    FannyMood.New("Nostalgic!", null, 10f, 0f, false).
                        AddRequiredNPC(god.Type);
                }
            }



            //Helper moods, reverse order so that rarer helpers take precedence
            FannyMood.New("100% Electric!", (ScreenHelperSceneMetrics metrics) => ScreenHelpersUIState.Renault5.Speaking,
               PriorityClass.OtherHelperMood, 0f, false, Color.Black, Color.White);
            FannyMood.New("Wonderful!", (ScreenHelperSceneMetrics metrics) => ScreenHelpersUIState.WonderFlower.Speaking,
               PriorityClass.OtherHelperMood, 0f, false);
            FannyMood.New("Awake!", (ScreenHelperSceneMetrics metrics) => Main.LocalPlayer.GetModPlayer<FluxPlayer>().currentFluxMode == (int)FluxPlayer.FluxState.WakingUp,
               PriorityClass.OtherHelperMood, 0f, false, Color.Chartreuse, Color.DarkOliveGreen);
            FannyMood.New("Kawaii!", (ScreenHelperSceneMetrics metrics) => ScreenHelpersUIState.TrapperBulbChan.Speaking,
               PriorityClass.OtherHelperMood, 0f, false);
            FannyMood.New("Fresh!", (ScreenHelperSceneMetrics metrics) => ScreenHelpersUIState.CrimSon.Speaking,
               PriorityClass.OtherHelperMood, 0f, false);
            FannyMood.New("Aggravated!!!", (ScreenHelperSceneMetrics metrics) => ScreenHelpersUIState.MiracleBoy.Speaking,
               PriorityClass.OtherHelperMood, 0f, false, Color.OrangeRed, Color.White);
            FannyMood.New("Bothered!", (ScreenHelperSceneMetrics metrics) => ScreenHelpersUIState.EvilFanny.Speaking,
               PriorityClass.OtherHelperMood, 0f, false, Color.Black, Color.Red);

            secondBlockMood = FannyMood.New("Miserable!", null,
               60f, 60f, false, Color.Red, Color.Black).DisableNaturalSelection();

            remorsefulMood = FannyMood.New("Remorseful!", null,
               7f, 10f, false).DisableNaturalSelection();
        }

        public bool CanUnlockMoodTracker(ScreenHelperSceneMetrics metrics) => NPC.downedDeerclops || NPC.downedQueenBee;
        public bool PlayerHidTracker(ScreenHelperSceneMetrics metrics) => Main.LocalPlayer.hideInfo[Type];

        public void UnhideTrackerForTheFirstTime()
        {
            Main.LocalPlayer.hideInfo[Type] = false;
            //Enables the second message (which by default is treated as "already seen"
            secondBlockDisableMessage.alreadySeen = false;
        }

		public override string DisplayValue(ref Color displayColor, ref Color displayShadowColor)
		{
            if (!ScreenHelperManager.screenHelpersEnabled)
            {
                displayColor = InactiveInfoTextColor;
                return "I'm feeling: Betrayed...";
            }

            string moodText = "Chummy!";
            displayColor = Color.Lime;
            displayShadowColor = Color.DarkBlue;

            FannyMood chosenMood = null;
            if (lingeringMood != null)
                chosenMood = lingeringMood;

            bool foundNewMood = false;

            //Moodlist waas sorted in reverse priority order so we start with the most important first
            foreach (FannyMood mood in moodList)
            {
                //Ignore moods that can be overriden if theres a lingering mood if theres already a lingering mood
                if (lingeringMood != null && mood.canBeOverridenByLingeringMood)
                    continue;

                //NPCLess moods just have this always set to true
                if (!mood.validConditionMet)
                    continue;

                //Get the first highest priority mood
                if (mood.condition(ScreenHelperManager.sceneMetrics))
                {
                    chosenMood = mood;
                    foundNewMood = true;
                    break;
                }
            }

            //NOTE : Plastic oracle in IonCubePlaced file sets the mood to "Trustful!"

            //Save the c urrent mood as a "lingering" mood
            if (foundNewMood && chosenMood.Lingering)
            {
                lingeringMood = chosenMood;
                moodLingerTimer = chosenMood.lingerTime;
            }

            if (chosenMood != null)
            {
                moodText = chosenMood.moodText;
                displayColor = chosenMood.textColor;
                displayShadowColor = chosenMood.outlineColor;
            }

            return "I'm feeling: " + moodText;
		}
	}

    public class FannyMood
    {
        public ScreenHelperMessageCondition condition;

        public string moodText;
        public float priority;
        public bool Lingering => lingerTime > 0f;
        public float lingerTime;
        public bool canBeOverridenByLingeringMood;

        public Color textColor;
        public Color outlineColor;

        public int[] requiredNPCs = new int[0];
        public bool validConditionMet = true;

        private FannyMood(string text, ScreenHelperMessageCondition condition, float priority, float lingeringTime, bool canBeOverridenByLingeringMood , Color textColor, Color outlineColor)
        {
            moodText = text;
            this.condition = condition ?? HelperMessage.AlwaysShow;
            this.priority = priority;
            this.lingerTime = lingeringTime;
            this.canBeOverridenByLingeringMood = canBeOverridenByLingeringMood;

            this.textColor = textColor;
            this.outlineColor = outlineColor;
        }

        public static FannyMood New(string text, ScreenHelperMessageCondition condition, float priority, float lingeringTime = 0f, bool canBeOverridenByLingeringMood = false, Color? textColor = null, Color? outlineColor = null)
        {
            FannyMood mood = new FannyMood(text, condition, priority, lingeringTime, canBeOverridenByLingeringMood, textColor.HasValue ? textColor.Value : Color.Lime, outlineColor.HasValue ? outlineColor.Value : Color.DarkBlue);
            MoodTracker.moodList.Add(mood);
            return mood;
        }

        public static FannyMood New(string text, ScreenHelperMessageCondition condition, MoodTracker.PriorityClass priority, float lingeringTime = 0f, bool canBeOverridenByLingeringMood = false, Color? textColor = null, Color? outlineColor = null)
        {
            FannyMood mood = new FannyMood(text, condition, (float)priority, lingeringTime, canBeOverridenByLingeringMood, textColor.HasValue ? textColor.Value : Color.Lime, outlineColor.HasValue ? outlineColor.Value : Color.DarkBlue);
            MoodTracker.moodList.Add(mood);
            return mood;
        }

        public FannyMood AddRequiredNPC(params int[] args)
        {
            requiredNPCs = args;
            validConditionMet = false;
            MoodTracker.npcDependentMoodList.Add(this);
            return this;
        }

        public FannyMood DisableNaturalSelection()
        {
            validConditionMet = false;
            return this;
        }


        public void Activate(bool forceReplace = false)
        {
            if (forceReplace || MoodTracker.lingeringMood == null || MoodTracker.lingeringMood.priority <= priority)
            {
                MoodTracker.lingeringMood = this;
                MoodTracker.moodLingerTimer = lingerTime;
            }
        }

    }
}
