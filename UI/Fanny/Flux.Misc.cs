using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatBuffs;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.NPCs.Polterghast;
using CalRemix.Content.Items.Placeables;
using CalRemix.Content.Items.Weapons.Stormbow;
using System.Linq;
using System.Threading;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class ScreenHelperManager : ModSystem
    {
        public static void LoadFluxMessages()
        {
            #region debuff dialogue
            HelperMessage.New("FluxArmorCrunch", "It looks like your armor's broken. You might want to get that checked out.",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<ArmorCrunch>(), cooldown: 1200, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxAstralInfection", "You might want to be careful around that Astral stuff.",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<AstralInfectionDebuff>(), cooldown: 1200, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxBrainRot", "Are you alright? It looks like your head hurts... I can get you a Tylenol™ if you need.",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<BrainRot>(), cooldown: 1200, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxBrimstoneFlames", "You might want to be careful... It looks like you're on fire...",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<BrimstoneFlames>(), cooldown: 1200, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxBurningBlood", "You look uncomfortable, are you alright?",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<BurningBlood>(), cooldown: 1200, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxClamity", "The clams look rather rambunctious today. Watch out.",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<Clamity>(), cooldown: 4800, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);

            HelperMessage fluxCrushDepth1 = HelperMessage.New("FluxCrushDepth1", "'Warning: maximum depth reached! Hull damage imminent!' Haha, that's a line from Subnautica...",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<CrushDepth>())
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux).InitiateConversation(6);
            HelperMessage fluxCrushDepth2 = HelperMessage.New("FluxCrushDepth2", "I'm sorry. I won't say that again.",
                "FluxDefault", HelperMessage.AlwaysShow, 5)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux).EndConversation()
                .ChainAfter(fluxCrushDepth1, delay: 3f);

            HelperMessage.New("FluxDragonfire", "You might want to be careful... It looks like you're on fire...",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<Dragonfire>(), cooldown: 1200, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            // not doing extreme gravity so the focus during dog is on fanny
            HelperMessage.New("FluxFishAlert", "Watch out! It looks like the sealife of the Abyss caught on to your tricks.",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<FishAlert>(), cooldown: 1200, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxGlacialState", "Are you good in there? You look like a frozen caveman...",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<GlacialState>(), cooldown: 1200, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            // also not doing godslayer inferno, ditto
            HelperMessage.New("FluxHolyFlames", "You might want to be careful... It looks like you're on fire...",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<HolyFlames>(), cooldown: 1200, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxHolyInferno", "You should stick closer to the burning sun, it looks like staying away from it burns you.",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<HolyInferno>(), cooldown: 1200, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            // nothing for icarus folly since thats persistent itd be weird
            HelperMessage.New("FluxIrradiated", "You might wanna get some cancer treatment after this...",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<Irradiated>(), cooldown: 1200, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxMiracleBlight", "What's going on? Are you okay? It looks like you're all green! Like me! That isn't good!",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<MiracleBlight>(), cooldown: 1200, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxNightwither", "Are you alright? You look so sickly... Is there anything I can do to help?",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<Nightwither>(), cooldown: 1200, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxNOU", "Oh No! It looks like you're naked! That's terrible! Can you stay like that Haha",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<NOU>(), duration: 2)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxPlague", "Are you ill? Oh no... That isn't good... I'm sorry...",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<Plague>(), cooldown: 1200, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            // nothing for riptide bcuz im lazy
            HelperMessage.New("FluxSulphuricPoisoning", "Don't drink the sulphur water. I don't think it's... Nevermind. You probably already knew that...",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<SulphuricPoisoning>(), cooldown: 1200, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxVulnerabilityHex", "You're vulnerable... I'm sorry, I can't help...",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<VulnerabilityHex>(), cooldown: 1200, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            // nothing for warped, see extreme gravity
            // nothing for weak petrification, ditto
            HelperMessage.New("FluxWhisperingDeath", "Oh no... Oh no...",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff<WhisperingDeath>(), cooldown: 1200, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);

            // vanilla

            HelperMessage.New("FluxBleeding", "Are you bleeding!? Oh no... Try and look for first aid?",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Bleeding), cooldown: 1200, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxPoison", "Oh jeez. You might need medical attention.",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Poisoned), cooldown: 1200, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxOnFire", "You might want to be careful... It looks like you're on fire...",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.OnFire), cooldown: 1200, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxAcidVenom", "Did a spider bite you? Seek medical attention as fast as you can!",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Venom), cooldown: 1200, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxDarkness", "It's so dark... What happened?",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Darkness), cooldown: 1200, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxBlackout", "What...?",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Blackout), cooldown: 1200, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxSilenced", " ",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Silenced), cooldown: 1200, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            //HelperMessage.New("FluxCursed", "What happened? Why aren't you attacking?",
            //    "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Cursed), cooldown: 1200, onlyPlayOnce: false).SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            // nothing for confused cuz i felt like it
            HelperMessage.New("FluxSlow", "You're as slow as a snail. That... Isn't good.",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Slow), cooldown: 1200, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxOozed", "Eew... Yucky. You're all sticky and stuff... Yuckers.",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.OgreSpit), cooldown: 1200, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxWeak", "Your muscles look atrophied... Are you good? Do you need a break?",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Weak), cooldown: 1200, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            // nothing for withered debuffs
            // nothing for horrified
            HelperMessage.New("FluxTheTongue", "Freaky",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.TheTongue), cooldown: 1200, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxCursedInferno", "Foul demon fire... You should try to extinguish yourself.",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.CursedInferno), cooldown: 1200, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxIchor", "Gross...",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Ichor), cooldown: 1200, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxFrostburn", "It's pretty cold... You might want to invest in a sweater.",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Frostburn), cooldown: 1200, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxChilled", "Is it getting cold here, or is it just me...?",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Chilled), cooldown: 1200, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxWebbed", "Yuck- you've been rendered immobile by webs.",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Webbed), cooldown: 1200, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxStoned", "Are... Are you okay? Are you good? Do you need any help?",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Stoned), cooldown: 1200, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxDistorted", "What- what are you doing? Ughh, I feel like I'm gonna be sick...",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.VortexDebuff), cooldown: 1200, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            // idk what to do for obstructed
            HelperMessage.New("FluxElectrified", "Quite shocking! Hahah... I'm sorry, that wasn't funny. You're clearly in pain... I'm sorry.",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Electrified), cooldown: 1200, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxFeralBite", "You've gotten your rabies shot recently, right?... Right?",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.Rabies), cooldown: 1200, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxMoonBite", "What's he doing to you!? Get him to stop that! That can't be good for you...",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.MoonLeech), cooldown: 1200, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            #endregion

            HelperMessage.New("FluxCrit1", "Crit",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.MoonLeech), cooldown: 1, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
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
        
        public override void PreUpdate()
        {
            hasCrit = false;
        }
    }
}