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
        public static void LoadFluxMessages()
        {
            #region debuff dialogue
            HelperMessage.New("FluxArmorCrunch", "It looks like your armor's broken. You might want to get that checked out.",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<ArmorCrunch>() && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxAstralInfection", "You might want to be careful around that Astral stuff.",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<AstralInfectionDebuff>() && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxBrainRot", "Are you alright? It looks like your head hurts... I can get you a Tylenol™ if you need.",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<BrainRot>() && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxBrimstoneFlames", "You might want to be careful... It looks like you're on fire...",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<BrimstoneFlames>() && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxBurningBlood", "You look uncomfortable, are you alright?",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<BurningBlood>() && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxClamity", "The clams look rather rambunctious today. Watch out.",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<Clamity>() && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);

            HelperMessage fluxCrushDepth1 = HelperMessage.New("FluxCrushDepth1", "'Warning: maximum depth reached! Hull damage imminent!' Haha, that's a line from Subnautica...",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<CrushDepth>() && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux).InitiateConversation(6);
            HelperMessage fluxCrushDepth2 = HelperMessage.New("FluxCrushDepth2", "I'm sorry. I won't say that again.",
                "FluxDefault", HelperMessage.AlwaysShow, 5)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux).EndConversation()
                .ChainAfter(fluxCrushDepth1, delay: 3f);

            HelperMessage.New("FluxDragonfire", "You might want to be careful... It looks like you're on fire...",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<Dragonfire>() && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            // not doing extreme gravity so the focus during dog is on fanny
            HelperMessage.New("FluxFishAlert", "Watch out! It looks like the sealife of the Abyss caught on to your tricks.",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<FishAlert>() && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxGlacialState", "Are you good in there? You look like a frozen caveman...",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<GlacialState>() && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            // also not doing godslayer inferno, ditto
            HelperMessage.New("FluxHolyFlames", "You might want to be careful... It looks like you're on fire...",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<HolyFlames>() && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxHolyInferno", "You should stick closer to the burning sun, it looks like staying away from it burns you.",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<HolyInferno>() && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            // nothing for icarus folly since thats persistent itd be weird
            HelperMessage.New("FluxIrradiated", "You might wanna get some cancer treatment after this...",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<Irradiated>() && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxMiracleBlight", "What's going on? Are you okay? It looks like you're all green! Like me! That isn't good!",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<MiracleBlight>() && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxNightwither", "Are you alright? You look so sickly... Is there anything I can do to help?",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<Nightwither>() && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxNOU", "Oh No! It looks like you're naked! That's terrible! Can you stay like that Haha",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<NOU>() && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, duration: 2)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxPlague", "Are you ill? Oh no... That isn't good... I'm sorry...",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<Plague>() && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            // nothing for riptide bcuz im lazy
            HelperMessage.New("FluxSulphuricPoisoning", "Don't drink the sulphur water. I don't think it's... Nevermind. You probably already knew that...",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<SulphuricPoisoning>() && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxVulnerabilityHex", "You're vulnerable... I'm sorry, I can't help...",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<VulnerabilityHex>() && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            // nothing for warped, see extreme gravity
            // nothing for weak petrification, ditto
            HelperMessage.New("FluxWhisperingDeath", "Oh no... Oh no...",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<WhisperingDeath>() && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);

            // vanilla

            HelperMessage.New("FluxBleeding", "Are you bleeding!? Oh no... Try and look for first aid?",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Bleeding) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxPoison", "Oh jeez. You might need medical attention.",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Poisoned) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxOnFire", "You might want to be careful... It looks like you're on fire...",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.OnFire) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxAcidVenom", "Did a spider bite you? Seek medical attention as fast as you can!",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Venom) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxDarkness", "It's so dark... What happened?",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Darkness) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxBlackout", "What...?",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Blackout) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxSilenced", " ",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Silenced) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            //HelperMessage.New("FluxCursed", "What happened? Why aren't you attacking?",
            //    "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Cursed) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 30, onlyPlayOnce: false).SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            // nothing for confused cuz i felt like it
            HelperMessage.New("FluxSlow", "You're as slow as a snail. That... Isn't good.",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Slow) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxOozed", "Eew... Yucky. You're all sticky and stuff... Yuckers.",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.OgreSpit) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxWeak", "Your muscles look atrophied... Are you good? Do you need a break?",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Weak) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            // nothing for withered debuffs
            // nothing for horrified
            //HelperMessage.New("FluxTheTongue", "Freaky",
            //    "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.TheTongue) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 30, onlyPlayOnce: false)
            //    .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxCursedInferno", "Foul demon fire... You should try to extinguish yourself.",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.CursedInferno) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxIchor", "Gross...",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Ichor) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxFrostburn", "It's pretty cold... You might want to invest in a sweater.",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Frostburn) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxChilled", "Is it getting cold here, or is it just me...?",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Chilled) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxWebbed", "Yuck- you've been rendered immobile by webs.",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Webbed) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxStoned", "Are... Are you okay? Are you good? Do you need any help?",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Stoned) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxDistorted", "What- what are you doing? Ughh, I feel like I'm gonna be sick...",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.VortexDebuff) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            // idk what to do for obstructed
            HelperMessage.New("FluxElectrified", "Quite shocking! Hahah... I'm sorry, that wasn't funny. You're clearly in pain... I'm sorry.",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Electrified) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxFeralBite", "You've gotten your rabies shot recently, right?... Right?",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Rabies) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxMoonBite", "What's he doing to you!? Get him to stop that! That can't be good for you...",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.MoonLeech) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 30, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            #endregion

            #region crit
            int chanceForCritDialogue = 130;

            // technically, more than one of these could play per crit
            // i dont care
            HelperMessage.New("FluxCrit1", "You hit their weakspot!",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.GetModPlayer<FluxPlayer>().hasCrit && Main.rand.NextBool(chanceForCritDialogue) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 5, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxCrit2", "You got the critical hit!",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.GetModPlayer<FluxPlayer>().hasCrit && Main.rand.NextBool(chanceForCritDialogue) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 5, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxCrit3", "I hope you can beat them with that hit...",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.GetModPlayer<FluxPlayer>().hasCrit && Main.rand.NextBool(chanceForCritDialogue) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 5, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxCrit4", "Keep trying to hit their weak spot!",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.GetModPlayer<FluxPlayer>().hasCrit && Main.rand.NextBool(chanceForCritDialogue) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 5, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxCrit5", "Yeah! Keep hitting there, they took more damage!",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.GetModPlayer<FluxPlayer>().hasCrit && Main.rand.NextBool(chanceForCritDialogue) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 5, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            #endregion

            #region hit
            int chanceForHitDialogue = 50;

            // technically, more than one of these could play per crit
            // i dont care
            HelperMessage.New("FluxHit1", "Are you alright??",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.GetModPlayer<FluxPlayer>().hasBeenHit && Main.rand.NextBool(chanceForHitDialogue) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 5, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxHit2", "Watch out for enemy attacks!",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.GetModPlayer<FluxPlayer>().hasBeenHit && Main.rand.NextBool(chanceForHitDialogue) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 5, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxHit3", "I hope you'll be fine after that...",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.GetModPlayer<FluxPlayer>().hasBeenHit && Main.rand.NextBool(chanceForHitDialogue) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 5, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxHit4", "You should heal up, stat!",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.GetModPlayer<FluxPlayer>().hasBeenHit && Main.rand.NextBool(chanceForHitDialogue) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 5, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxHit5", "Please be alright, please be alright...",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.GetModPlayer<FluxPlayer>().hasBeenHit && Main.rand.NextBool(chanceForHitDialogue) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 5, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            #endregion

            #region boss spawn/death
            HelperMessage.New("BossSpawn1", "Oh jeez, that's one big foe... I wish I could help you...",
                "FluxDefault", (ScreenHelperSceneMetrics metrics) => metrics.onscreenNPCs.Any((NPC n) => n.type != 4) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossIsPresent == true && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossSpawnEndMessageVariant == 1 && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 3, onlyPlayOnce: false)
                .AddStartEvent(SwitchBossActivity)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("BossSpawn2", "You're gonna wanna be careful. It looks like something big is coming...",
                "FluxDefault", (ScreenHelperSceneMetrics metrics) => metrics.onscreenNPCs.Any((NPC n) => n.type != 4) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossIsPresent == true && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossSpawnEndMessageVariant == 2 && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 3, onlyPlayOnce: false)
                .AddStartEvent(SwitchBossActivity)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("BossSpawn3", "Oh no... Something big's approaching, and fast. Are you gonna be alright?",
                "FluxDefault", (ScreenHelperSceneMetrics metrics) => metrics.onscreenNPCs.Any((NPC n) => n.type != 4) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossIsPresent == true && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossSpawnEndMessageVariant == 3 && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 3, onlyPlayOnce: false)
                .AddStartEvent(SwitchBossActivity)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("BossSpawn4", "Some sort of boss monster is coming... Watch out...",
                "FluxDefault", (ScreenHelperSceneMetrics metrics) => metrics.onscreenNPCs.Any((NPC n) => n.type != 4) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossIsPresent == true && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossSpawnEndMessageVariant == 4 && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 3, onlyPlayOnce: false)
                .AddStartEvent(SwitchBossActivity)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);

            HelperMessage.New("BossDie1", "Thank goodness, you're still alive! Good job...",
                "FluxDefault", (ScreenHelperSceneMetrics metrics) => metrics.onscreenNPCs.Any((NPC n) => n.type != 4) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossIsPresent == false && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossSpawnEndMessageVariant == 1 && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 3, onlyPlayOnce: false)
                .AddStartEvent(SwitchBossActivity)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("BossDie2", "Is that thing down? I wish i could have helped you... Atleast you made it out alive.",
                "FluxDefault", (ScreenHelperSceneMetrics metrics) => metrics.onscreenNPCs.Any((NPC n) => n.type != 4) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossIsPresent == false && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossSpawnEndMessageVariant == 2 && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 3, onlyPlayOnce: false)
                .AddStartEvent(SwitchBossActivity)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("BossDie3", "Glad to see that thing taken care of. Take some time to heal your wounds, please...",
                "FluxDefault", (ScreenHelperSceneMetrics metrics) => metrics.onscreenNPCs.Any((NPC n) => n.type != 4) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossIsPresent == false && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossSpawnEndMessageVariant == 3 && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 3, onlyPlayOnce: false)
                .AddStartEvent(SwitchBossActivity)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("BossDie4", "With that thing down, the world is just an inch safer. Let's hope it doesn't come back...",
                "FluxDefault", (ScreenHelperSceneMetrics metrics) => metrics.onscreenNPCs.Any((NPC n) => n.type != 4) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossIsPresent == false && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossSpawnEndMessageVariant == 4 && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake, cooldown: 3, onlyPlayOnce: false)
                .AddStartEvent(SwitchBossActivity)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            #endregion

            HelperMessage.New("WhiteRabbit", "Yikes! That thing looks really dangerous! Be careful!",
                "FluxDefault", (ScreenHelperSceneMetrics metrics) => metrics.onscreenNPCs.Any((NPC n) => n.type == NPCID.Bunny) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);

            HelperMessage.New("BlackRabbit", "Eek! I told you it was dangerous!",
                "FluxDefault", (ScreenHelperSceneMetrics metrics) => metrics.onscreenNPCs.Any((NPC n) => n.type == NPCID.CorruptBunny || n.type == NPCID.CrimsonBunny) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().isFluxAwake)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);

            HelperMessage ankhShield1 = HelperMessage.New("AnkhShield1", "Is that an Ankh item? With one of those, you won't get as many debuffs anymore...",
                "FluxDefault", (ScreenHelperSceneMetrics metrics) => Main.LocalPlayer.HasItem(ItemID.AnkhShield))
                .AddSelectionEvent(ForceWakeUpFlux)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage ankhShield2 = HelperMessage.New("AnkhShield2", "I'm sorry!! I'm so sorry!!! I'll find other things to call out, I promise!",
                "FluxDefault", (ScreenHelperSceneMetrics metrics) => Main.LocalPlayer.HasItem(ItemID.AnkhShield))
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux)
                .ChainAfter(ankhShield1);
        }

        public static void SwitchBossActivity()
        {
            Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossIsPresent = null;
        }

        public static void ForceWakeUpFlux()
        {
            Main.LocalPlayer.GetModPlayer<FluxPlayer>().timeUntilNextFluxAction = GetTimeUntilNextFluxStage();
            Main.LocalPlayer.GetModPlayer<FluxPlayer>().currentFluxMode = (int)FluxPlayer.FluxState.Awake;
        }

        public static int GetTimeUntilNextFluxStage()
        {
            float timeAsleepToWaking = 28;
            float timeWakingToAwake = 2;
            float timeAwakeToAsleep = 20;

            float timeToReturn = 0;
            
            switch (Main.LocalPlayer.GetModPlayer<FluxPlayer>().currentFluxMode)
            {
                case (int)FluxPlayer.FluxState.Asleep:
                    timeToReturn = timeAsleepToWaking;
                    break;
                case (int)FluxPlayer.FluxState.WakingUp:
                    timeToReturn = timeWakingToAwake;
                    break;
                case (int)FluxPlayer.FluxState.Awake:
                    timeToReturn = timeAwakeToAsleep;
                    break;
            }

            // turn int into minutes
            timeToReturn *= (float)Math.Pow(60, 2);
            // add layer of noise
            if (Main.LocalPlayer.GetModPlayer<FluxPlayer>().currentFluxMode != (int)FluxPlayer.FluxState.WakingUp)
            {
                // between -3 and 3 minutes
                timeToReturn += Main.rand.Next(-10800, 10801);
            }

            return (int)timeToReturn;
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
                        timeUntilNextFluxAction = ScreenHelperManager.GetTimeUntilNextFluxStage();
                        currentFluxMode = (int)FluxState.WakingUp;
                        break;
                    case (int)FluxState.WakingUp:
                        // if waking up, then fully wake up
                        timeUntilNextFluxAction = ScreenHelperManager.GetTimeUntilNextFluxStage();
                        currentFluxMode = (int)FluxState.Awake;
                        break;
                    case (int)FluxState.Awake:
                        // if awake, go to sleep
                        timeUntilNextFluxAction = ScreenHelperManager.GetTimeUntilNextFluxStage();
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