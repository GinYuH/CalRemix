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

            #region crit
            // technically, more than one of these could play per crit
            // i dont care
            HelperMessage.New("FluxCrit1", "You hit their weakspot!",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.GetModPlayer<FluxPlayer>().hasCrit && Main.rand.NextBool(75), cooldown: 5, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxCrit2", "You got the critical hit!",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.GetModPlayer<FluxPlayer>().hasCrit && Main.rand.NextBool(75), cooldown: 5, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxCrit3", "I hope you can beat them with that hit...",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.GetModPlayer<FluxPlayer>().hasCrit && Main.rand.NextBool(75), cooldown: 5, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxCrit4", "Keep trying to hit their weak spot!",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.GetModPlayer<FluxPlayer>().hasCrit && Main.rand.NextBool(75), cooldown: 5, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("FluxCrit5", "Yeah! Keep hitting there, they took more damage!",
                "FluxDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.GetModPlayer<FluxPlayer>().hasCrit && Main.rand.NextBool(75), cooldown: 5, onlyPlayOnce: false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            #endregion

            #region boss spawn/death
            HelperMessage.New("BossSpawn1", "Oh jeez, that's one big foe... I wish I could help you...",
                "FluxDefault", (ScreenHelperSceneMetrics metrics) => metrics.onscreenNPCs.Any((NPC n) => n.type != 4) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossIsPresent == true && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossSpawnEndMessageVariant == 1, cooldown: 3, onlyPlayOnce: false)
                .AddStartEvent(SwitchBossActivity)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("BossSpawn2", "You're gonna wanna be careful. It looks like something big is coming...",
                "FluxDefault", (ScreenHelperSceneMetrics metrics) => metrics.onscreenNPCs.Any((NPC n) => n.type != 4) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossIsPresent == true && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossSpawnEndMessageVariant == 2, cooldown: 3, onlyPlayOnce: false)
                .AddStartEvent(SwitchBossActivity)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("BossSpawn3", "Oh no... Soemthing big's approaching, and fast. Are you gonna be alright?",
                "FluxDefault", (ScreenHelperSceneMetrics metrics) => metrics.onscreenNPCs.Any((NPC n) => n.type != 4) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossIsPresent == true && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossSpawnEndMessageVariant == 3, cooldown: 3, onlyPlayOnce: false)
                .AddStartEvent(SwitchBossActivity)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("BossSpawn4", "Some sort of boss monster is coming... Watch out...",
                "FluxDefault", (ScreenHelperSceneMetrics metrics) => metrics.onscreenNPCs.Any((NPC n) => n.type != 4) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossIsPresent == true && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossSpawnEndMessageVariant == 4, cooldown: 3, onlyPlayOnce: false)
                .AddStartEvent(SwitchBossActivity)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);

            HelperMessage.New("BossDie1", "Thank goodness, you're still alive! Good job...",
                "FluxDefault", (ScreenHelperSceneMetrics metrics) => metrics.onscreenNPCs.Any((NPC n) => n.type != 4) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossIsPresent == false && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossSpawnEndMessageVariant == 1, cooldown: 3, onlyPlayOnce: false)
                .AddStartEvent(SwitchBossActivity)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("BossDie2", "Is that thing down? I wish i could have helped you... Atleast you made it out alive.",
                "FluxDefault", (ScreenHelperSceneMetrics metrics) => metrics.onscreenNPCs.Any((NPC n) => n.type != 4) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossIsPresent == false && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossSpawnEndMessageVariant == 2, cooldown: 3, onlyPlayOnce: false)
                .AddStartEvent(SwitchBossActivity)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("BossDie3", "Glad to see that thing taken care of. Take some time to heal your wounds, please...",
                "FluxDefault", (ScreenHelperSceneMetrics metrics) => metrics.onscreenNPCs.Any((NPC n) => n.type != 4) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossIsPresent == false && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossSpawnEndMessageVariant == 3, cooldown: 3, onlyPlayOnce: false)
                .AddStartEvent(SwitchBossActivity)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            HelperMessage.New("BossDie4", "With that thing down, the world is just an inch safer. Let's hope it doesn't come back...",
                "FluxDefault", (ScreenHelperSceneMetrics metrics) => metrics.onscreenNPCs.Any((NPC n) => n.type != 4) && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossIsPresent == false && Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossSpawnEndMessageVariant == 4, cooldown: 3, onlyPlayOnce: false)
                .AddStartEvent(SwitchBossActivity)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Flux);
            #endregion
        }

        public static void SwitchBossActivity()
        {
            Main.LocalPlayer.GetModPlayer<FluxPlayer>().bossIsPresent = null;
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

        public bool? bossIsPresent;
        public int bossSpawnEndMessageVariant;
        
        public override void PreUpdate()
        {
            hasCrit = false;
        }
    }

    public class FluxNPC : GlobalNPC
    {
        private int chanceToFail = 5;
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