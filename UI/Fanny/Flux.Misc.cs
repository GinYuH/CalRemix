using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatBuffs;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.NPCs.Polterghast;
using CalRemix.Content.Items.Placeables;
using CalRemix.Content.Items.Weapons.Stormbow;
using CalRemix.Content.NPCs.Bosses.Hypnos;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CalRemix.UI
{
    public partial class ScreenHelperManager : ModSystem
    {
        public const float Flux_timeAsleepToWaking = 28;
        public const float Flux_timeWakingToAwake = 2;
        public const float Flux_timeAwakeToAsleep = 20;

        public static void LoadFluxMessages()
        {
            #region debuff dialogue
            HelperMessage.New("FluxArmorCrunch", "It looks like your armor's broken. You might want to get that checked out.",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<ArmorCrunch>(), cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxAstralInfection", "You might want to be careful around that Astral stuff.",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<AstralInfectionDebuff>(), cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxBrainRot", "Are you alright? It looks like your head hurts... I can get you a Tylenol™ if you need.",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<BrainRot>(), cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxBrimstoneFlames", "You might want to be careful... It looks like you're on fire...",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<BrimstoneFlames>(), cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxBurningBlood", "You look uncomfortable, are you alright?",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<BurningBlood>(), cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxClamity", "The clams look rather rambunctious today. Watch out.",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<Clamity>(), cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);

            HelperMessage fluxCrushDepth1 = HelperMessage.New("FluxCrushDepth1", "'Warning: maximum depth reached! Hull damage imminent!' Haha, that's a line from Subnautica...",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<CrushDepth>())
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux).InitiateConversation(6);
            HelperMessage fluxCrushDepth2 = HelperMessage.New("FluxCrushDepth2", "I'm sorry. I won't say that again.",
                "FluxIdle", HelperMessage.AlwaysShow, 5)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux).EndConversation()
                .ChainAfter(fluxCrushDepth1, delay: 3f);

            HelperMessage.New("FluxDragonfire", "You might want to be careful... It looks like you're on fire...",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<Dragonfire>(), cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            // not doing extreme gravity so the focus during dog is on fanny
            HelperMessage.New("FluxFishAlert", "Watch out! It looks like the sealife of the Abyss caught on to your tricks.",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<FishAlert>(), cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxGlacialState", "Are you good in there? You look like a frozen caveman...",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<GlacialState>(), cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            // also not doing godslayer inferno, ditto
            HelperMessage.New("FluxHolyFlames", "You might want to be careful... It looks like you're on fire...",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<HolyFlames>(), cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxHolyInferno", "You should stick closer to the burning sun, it looks like staying away from it burns you.",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<HolyInferno>(), cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            // nothing for icarus folly since thats persistent itd be weird
            HelperMessage.New("FluxIrradiated", "You might wanna get some cancer treatment after this...",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<Irradiated>(), cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxMiracleBlight", "What's going on? Are you okay? It looks like you're all green! Like me! That isn't good!",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<MiracleBlight>(), cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxNightwither", "Are you alright? You look so sickly... Is there anything I can do to help?",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<Nightwither>(), cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxNOU", "Oh No! It looks like you're naked! That's terrible! Can you stay like that Haha",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<NOU>(), duration: 2)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxPlague", "Are you ill? Oh no... That isn't good... I'm sorry...",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<Plague>(), cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            // nothing for riptide bcuz im lazy
            HelperMessage.New("FluxSulphuricPoisoning", "Don't drink the sulphur water. I don't think it's... Nevermind. You probably already knew that...",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<SulphuricPoisoning>(), cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxVulnerabilityHex", "You're vulnerable... I'm sorry, I can't help...",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<VulnerabilityHex>(), cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            // nothing for warped, see extreme gravity
            // nothing for weak petrification, ditto
            HelperMessage.New("FluxWhisperingDeath", "Oh no... Oh no...",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<WhisperingDeath>(), cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);

            // vanilla

            HelperMessage.New("FluxBleeding", "Are you bleeding!? Oh no... Try and look for first aid?",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Bleeding), cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxPoison", "Oh jeez. You might need medical attention.",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Poisoned), cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxOnFire", "You might want to be careful... It looks like you're on fire...",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.OnFire), cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxAcidVenom", "Did a spider bite you? Seek medical attention as fast as you can!",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Venom), cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxDarkness", "It's so dark... What happened?",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Darkness), cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxBlackout", "What...?",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Blackout), cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxSilenced", " ",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Silenced), cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            //HelperMessage.New("FluxCursed", "What happened? Why aren't you attacking?",
            //    "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Cursed), cooldown: 30, onlyPlayOnce: false).SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            // nothing for confused cuz i felt like it
            HelperMessage.New("FluxSlow", "You're as slow as a snail. That... Isn't good.",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Slow), cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxOozed", "Eew... Yucky. You're all sticky and stuff... Yuckers.",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.OgreSpit), cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxWeak", "Your muscles look atrophied... Are you good? Do you need a break?",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Weak), cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            // nothing for withered debuffs
            // nothing for horrified
            //HelperMessage.New("FluxTheTongue", "Freaky",
            //    "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.TheTongue), cooldown: 30, onlyPlayOnce: false)
            //    .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxCursedInferno", "Foul demon fire... You should try to extinguish yourself.",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.CursedInferno), cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxIchor", "Gross...",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Ichor), cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxFrostburn", "It's pretty cold... You might want to invest in a sweater.",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Frostburn), cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxChilled", "Is it getting cold here, or is it just me...?",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Chilled), cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxWebbed", "Yuck- you've been rendered immobile by webs.",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Webbed), cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxStoned", "Are... Are you okay? Are you good? Do you need any help?",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Stoned), cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxDistorted", "What- what are you doing? Ughh, I feel like I'm gonna be sick...",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.VortexDebuff), cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            // idk what to do for obstructed
            HelperMessage.New("FluxElectrified", "Quite shocking! Hahah... I'm sorry, that wasn't funny. You're clearly in pain... I'm sorry.",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Electrified), cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxFeralBite", "You've gotten your rabies shot recently, right?... Right?",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Rabies), cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxMoonBite", "What's he doing to you!? Get him to stop that! That can't be good for you...",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.MoonLeech), cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            #endregion

            #region crit
            int chanceForCritDialogue = 130;

            // technically, more than one of these could play per crit
            // i dont care
            HelperMessage.New("FluxCrit1", "You hit their weakspot!",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.GetModPlayer<FluxPlayer>().hasCrit && Main.rand.NextBool(chanceForCritDialogue), cooldown: 5, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxCrit2", "You got the critical hit!",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.GetModPlayer<FluxPlayer>().hasCrit && Main.rand.NextBool(chanceForCritDialogue), cooldown: 5, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxCrit3", "I hope you can beat them with that hit...",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.GetModPlayer<FluxPlayer>().hasCrit && Main.rand.NextBool(chanceForCritDialogue), cooldown: 5, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxCrit4", "Keep trying to hit their weak spot!",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.GetModPlayer<FluxPlayer>().hasCrit && Main.rand.NextBool(chanceForCritDialogue), cooldown: 5, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxCrit5", "Yeah! Keep hitting there, they took more damage!",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.GetModPlayer<FluxPlayer>().hasCrit && Main.rand.NextBool(chanceForCritDialogue), cooldown: 5, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            #endregion

            #region hit
            int chanceForHitDialogue = 50;

            // technically, more than one of these could play per crit
            // i dont care
            HelperMessage.New("FluxHit1", "Are you alright??",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.GetModPlayer<FluxPlayer>().hasBeenHit && Main.rand.NextBool(chanceForHitDialogue), cooldown: 5, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxHit2", "Watch out for enemy attacks!",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.GetModPlayer<FluxPlayer>().hasBeenHit && Main.rand.NextBool(chanceForHitDialogue), cooldown: 5, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxHit3", "I hope you'll be fine after that...",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.GetModPlayer<FluxPlayer>().hasBeenHit && Main.rand.NextBool(chanceForHitDialogue), cooldown: 5, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxHit4", "You should heal up, stat!",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.GetModPlayer<FluxPlayer>().hasBeenHit && Main.rand.NextBool(chanceForHitDialogue), cooldown: 5, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxHit5", "Please be alright, please be alright...",
                "FluxIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.GetModPlayer<FluxPlayer>().hasBeenHit && Main.rand.NextBool(chanceForHitDialogue), cooldown: 5, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            #endregion

            #region boss spawn/death
            HelperMessage.New("BossSpawn1", "Oh jeez, that's one big foe... I wish I could help you...",
                "FluxIdle", (ScreenHelperSceneMetrics metrics) => metrics.onscreenNPCs.Any((NPC n) => n.type != 4) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossIsPresent == true && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossSpawnEndMessageVariant == 1, cooldown: 3, onlyPlayOnce: false)
                .AddStartEvent(SwitchBossActivity)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("BossSpawn2", "You're gonna wanna be careful. It looks like something big is coming...",
                "FluxIdle", (ScreenHelperSceneMetrics metrics) => metrics.onscreenNPCs.Any((NPC n) => n.type != 4) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossIsPresent == true && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossSpawnEndMessageVariant == 2, cooldown: 3, onlyPlayOnce: false)
                .AddStartEvent(SwitchBossActivity)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("BossSpawn3", "Oh no... Something big's approaching, and fast. Are you gonna be alright?",
                "FluxIdle", (ScreenHelperSceneMetrics metrics) => metrics.onscreenNPCs.Any((NPC n) => n.type != 4) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossIsPresent == true && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossSpawnEndMessageVariant == 3, cooldown: 3, onlyPlayOnce: false)
                .AddStartEvent(SwitchBossActivity)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("BossSpawn4", "Some sort of boss monster is coming... Watch out...",
                "FluxIdle", (ScreenHelperSceneMetrics metrics) => metrics.onscreenNPCs.Any((NPC n) => n.type != 4) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossIsPresent == true && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossSpawnEndMessageVariant == 4, cooldown: 3, onlyPlayOnce: false)
                .AddStartEvent(SwitchBossActivity)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);

            HelperMessage.New("BossDie1", "Thank goodness, you're still alive! Good job...",
                "FluxIdle", (ScreenHelperSceneMetrics metrics) => metrics.onscreenNPCs.Any((NPC n) => n.type != 4) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossIsPresent == false && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossSpawnEndMessageVariant == 1, cooldown: 3, onlyPlayOnce: false)
                .AddStartEvent(SwitchBossActivity)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("BossDie2", "Is that thing down? I wish i could have helped you... Atleast you made it out alive.",
                "FluxIdle", (ScreenHelperSceneMetrics metrics) => metrics.onscreenNPCs.Any((NPC n) => n.type != 4) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossIsPresent == false && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossSpawnEndMessageVariant == 2, cooldown: 3, onlyPlayOnce: false)
                .AddStartEvent(SwitchBossActivity)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("BossDie3", "Glad to see that thing taken care of. Take some time to heal your wounds, please...",
                "FluxIdle", (ScreenHelperSceneMetrics metrics) => metrics.onscreenNPCs.Any((NPC n) => n.type != 4) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossIsPresent == false && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossSpawnEndMessageVariant == 3, cooldown: 3, onlyPlayOnce: false)
                .AddStartEvent(SwitchBossActivity)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("BossDie4", "With that thing down, the world is just an inch safer. Let's hope it doesn't come back...",
                "FluxIdle", (ScreenHelperSceneMetrics metrics) => metrics.onscreenNPCs.Any((NPC n) => n.type != 4) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossIsPresent == false && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossSpawnEndMessageVariant == 4, cooldown: 3, onlyPlayOnce: false)
                .AddStartEvent(SwitchBossActivity)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            #endregion

            HelperMessage.New("WhiteRabbit", "Yikes! That thing looks really dangerous! Be careful!",
                "FluxIdle", (ScreenHelperSceneMetrics metrics) => metrics.onscreenNPCs.Any((NPC n) => n.type == NPCID.Bunny))
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);

            HelperMessage.New("BlackRabbit", "Eek! I told you it was dangerous!",
                "FluxIdle", (ScreenHelperSceneMetrics metrics) => metrics.onscreenNPCs.Any((NPC n) => n.type == NPCID.CorruptBunny || n.type == NPCID.CrimsonBunny))
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);

            HelperMessage ankhShield1 = HelperMessage.New("AnkhShield1", "Is that an Ankh item? With one of those, you won't get as many debuffs anymore...",
                "FluxIdle", (ScreenHelperSceneMetrics metrics) => Main.LocalPlayer.HasItem(ItemID.AnkhShield))
                .AddSelectionEvent(ForceWakeUpFlux)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage ankhShield2 = HelperMessage.New("AnkhShield2", "I'm sorry!! I'm so sorry!!! I'll find other things to call out, I promise!",
                "FluxIdle", (ScreenHelperSceneMetrics metrics) => Main.LocalPlayer.HasItem(ItemID.AnkhShield))
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux)
                .ChainAfter(ankhShield1);
        }

        public static void SwitchBossActivity()
        {
            Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossIsPresent = null;
        }

        public static void ForceWakeUpFlux()
        {
            Main.LocalPlayer.GetModPlayer<FluxPlayer>().timeUntilNextFluxAction = HelperHelpers.GetTimeUntilNextStage(Flux_timeAwakeToAsleep);
            Main.LocalPlayer.GetModPlayer<FluxPlayer>().currentFluxMode = (int)FluxPlayer.FluxState.Awake;
        }
    }

    public class FluxProjectile : GlobalProjectile
    {
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit && projectile.owner == Main.myPlayer)
            {
                Main.LocalPlayer.GetModPlayer<FluxPlayer>().hasCrit = true;
            }
        }
    }

    public class FluxPlayer : ModPlayer
    {
        public bool hasCrit;
        public bool hasBeenHit;

        public bool? bossIsPresent;
        public int bossSpawnEndMessageVariant;

        public enum FluxState
        {
            Asleep = 0,
            WakingUp = 1,
            Awake = 2
        }
        public int timeUntilNextFluxAction;
        public int currentFluxMode = (int)FluxState.Asleep;
        public bool isFluxAwake => currentFluxMode == (int)FluxState.Awake;

        public override void PreUpdate()
        {
            hasCrit = false;
            hasBeenHit = false;

            if (timeUntilNextFluxAction <= 0 && Player.GetModPlayer<CalRemixPlayer>().fifteenMinutesSinceHardmode <= 0)
            {
                switch (currentFluxMode)
                {
                    case (int)FluxState.Asleep:
                        // if asleep, start waking up
                        timeUntilNextFluxAction = HelperHelpers.GetTimeUntilNextStage(ScreenHelperManager.Flux_timeWakingToAwake);
                        currentFluxMode = (int)FluxState.WakingUp;
                        break; 
                    case (int)FluxState.WakingUp:
                        // if waking up, then fully wake up
                        timeUntilNextFluxAction = HelperHelpers.GetTimeUntilNextStage(ScreenHelperManager.Flux_timeAwakeToAsleep);
                        currentFluxMode = (int)FluxState.Awake;
                        break;
                    case (int)FluxState.Awake:
                        // if awake, go to sleep
                        timeUntilNextFluxAction = HelperHelpers.GetTimeUntilNextStage(ScreenHelperManager.Flux_timeAsleepToWaking);
                        currentFluxMode = (int)FluxState.Asleep;
                        break;
                }
            }
            timeUntilNextFluxAction--;

            // in case flux is a naughty girl and somehow gets activated before hardmode, put her back to bed
            if ((currentFluxMode == (int)FluxState.Awake || currentFluxMode == (int)FluxState.WakingUp) && Player.GetModPlayer<CalRemixPlayer>().fifteenMinutesSinceHardmode > 0)
            {
                currentFluxMode = (int)FluxState.Asleep;
            }
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            hasBeenHit = true;
        }

        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            hasBeenHit = true;
        }

        #region Data Save/Load
        public override void SaveData(TagCompound tag)
        {
            tag["TimeUntilNextFluxAction"] = timeUntilNextFluxAction;
            tag["CurrentFluxMode"] = currentFluxMode;
        }

        public override void LoadData(TagCompound tag)
        {
            timeUntilNextFluxAction = tag.GetInt("TimeUntilNextFluxAction");
            currentFluxMode = tag.GetInt("CurrentFluxMode");
        }
        #endregion
    }

    public class FluxNPC : GlobalNPC
    {
        private static int chanceToFail = 6;
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (npc.boss)
            {
                Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossIsPresent = true;
                Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossSpawnEndMessageVariant = Main.rand.Next(4 + chanceToFail + 1) - chanceToFail;
            }
        }

        public override void OnKill(NPC npc)
        {
            if (npc.boss)
            {
                Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossIsPresent = false;
                Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossSpawnEndMessageVariant = Main.rand.Next(4 + chanceToFail + 1) - chanceToFail;
            }
        }
    }
}