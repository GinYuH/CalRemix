using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria;
using Terraria.ModLoader;
using CalRemix.Content.NPCs.Bosses.Carcinogen;
using CalRemix.Core.Backgrounds.Plague;
using CalRemix.Core.Scenes;
using CalRemix.Core.Subworlds;
using Terraria.GameContent;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace CalRemix
{
    public class CalRemixAsset : ModSystem
    {
        internal static Effect SlendermanShader;
        internal static Effect ShieldShader;

        public static Asset<Texture2D> sunOG = null;
        public static Asset<Texture2D> sunReal = null;
        public static Asset<Texture2D> sunCreepy = null;
        public static Asset<Texture2D> sunOxy = null;
        public static Asset<Texture2D> sunOxy2 = null;
        public static Asset<Texture2D> brian = null;
        public override void Load()
        {
            if (!Main.dedServ)
            {
                Filters.Scene["CalRemix:AcidSight"] = new Filter(new ScreenShaderData(Request<Effect>("CalRemix/Assets/Effects/AcidSight"), "AcidPass"), EffectPriority.VeryHigh);
                Filters.Scene["CalRemix:LeanVision"] = new Filter(new ScreenShaderData(Request<Effect>("CalRemix/Assets/Effects/LeanVision"), "LeanPass"), EffectPriority.VeryHigh);
                Filters.Scene["CalRemix:PyrogenHeat"] = new Filter(new ScreenShaderData(Request<Effect>("CalRemix/Assets/Effects/PyrogenHeat"), "PyroPass"), EffectPriority.VeryHigh);
                Filters.Scene["CalRemix:PandemicPanic"] = new Filter(new PandemicPanicScreenShaderData("FilterMiniTower").UseColor(ExosphereSky.DrawColor).UseOpacity(0f), EffectPriority.VeryHigh);
                Filters.Scene["CalRemix:Slenderman"] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(0f, 0f, 0f).UseOpacity(0f), EffectPriority.VeryHigh);

                Filters.Scene["CalRemix:PlagueBiome"] = new Filter(new PlagueSkyData("FilterMiniTower").UseColor(Color.Green).UseOpacity(0.15f), EffectPriority.VeryHigh);
                SkyManager.Instance["CalRemix:PlagueBiome"] = new PlagueSky();
                Filters.Scene["CalRemix:CalamitySky"] = new Filter(new ScreenShaderData("FilterMoonLord"), EffectPriority.High);
                SkyManager.Instance["CalRemix:CalamitySky"] = new CalamitySky();
                Filters.Scene["CalRemix:Exosphere"] = new Filter(new ExosphereScreenShaderData("FilterMiniTower").UseColor(ExosphereSky.DrawColor).UseOpacity(0.25f), EffectPriority.VeryHigh);
                SkyManager.Instance["CalRemix:Exosphere"] = new ExosphereSky();
                Filters.Scene["CalRemix:Fanny"] = new Filter(new FannyScreenShaderData("FilterMiniTower").UseColor(FannySky.DrawColor).UseOpacity(0.25f), EffectPriority.VeryHigh);
                SkyManager.Instance["CalRemix:Fanny"] = new FannySky();
                Filters.Scene["CalRemix:Asbestos"] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(Color.Gray).UseOpacity(0.5f), EffectPriority.VeryHigh);
                SkyManager.Instance["CalRemix:Asbestos"] = new CarcinogenSky();
                SkyManager.Instance["CalRemix:PandemicPanic"] = new PandemicSky();
                SkyManager.Instance["CalRemix:TrueStory"] = new StorySky();
            }

            AssetRepository remixAsset = Mod.Assets;
            Effect LoadShader(string path) => remixAsset.Request<Effect>("Assets/Effects/" + path, AssetRequestMode.ImmediateLoad).Value;
            SlendermanShader = LoadShader("SlendermanStatic");
            RegisterMiscShader(SlendermanShader, "StaticPass", "SlendermanStaticShader");
            ShieldShader = LoadShader("HoloShield");
            RegisterScreenShader(ShieldShader, "ShieldPass", "HoloShieldShader");

            sunOG = TextureAssets.Sun3;
            sunReal = TextureAssets.Sun;
            brian = TextureAssets.Npc[NPCID.BrainofCthulhu];
            sunCreepy = Request<Texture2D>("CalRemix/Assets/ExtraTextures/Eclipse");
            sunOxy = Request<Texture2D>("CalRemix/Assets/ExtraTextures/Oxysun");
            sunOxy2 = Request<Texture2D>("CalRemix/Assets/ExtraTextures/Oxysun2");
        }
        private static void RegisterSceneFilter(ScreenShaderData passReg, string registrationName, EffectPriority priority = EffectPriority.High)
        {
            string prefixedRegistrationName = "CalRemix:" + registrationName;
            Filters.Scene[prefixedRegistrationName] = new Filter(passReg, priority);
            Filters.Scene[prefixedRegistrationName].Load();
        }
        private static void RegisterScreenShader(Effect shader, string passName, string registrationName, EffectPriority priority = EffectPriority.High)
        {
            Ref<Effect> shaderPointer = new(shader);
            ScreenShaderData passParamRegistration = new(shaderPointer, passName);
            RegisterSceneFilter(passParamRegistration, registrationName, priority);
        }
        private static void RegisterMiscShader(Effect shader, string passName, string registrationName)
        {
            Ref<Effect> shaderPointer = new(shader);
            MiscShaderData passParamRegistration = new(shaderPointer, passName);
            GameShaders.Misc["CalRemix/" + registrationName] = passParamRegistration;
        }
    }
    public static class CalRemixMusic
    {
        private static readonly string Path = "Assets/Music/";

        // Subworlds
        public static readonly int Exosphere = Set("CanAnybodyHearMe");

        // Biomes
        public static readonly int AsbestosCaves = Set("FibrousWhisper");
        public static readonly int PlaguedJungle = Set("PlaguedJungle");
        public static readonly int BaronStrait = Set("TheEndZone");
        public static readonly int VernalPass = Set("VajrabhairavasRest");
        public static readonly int FrozenStronghold = Set("LockedAway");
        public static readonly int ProfanedDesert = Set("EmpireofAsh");

        // Events
        public static readonly int AcidRainTier2 = Set("TropicofCancer");
        public static readonly int PandemicPanic = Set("PandemicPanic");

        // NPCs
        public static readonly int CryoSlime = Set("AntarcticReinsertion");

        // Minibosses
        public static readonly int LaRuga = Set("LaRuga");
        public static readonly int DemonChase = Set("DemonChase");

        // Bosses
        public static readonly int TheCalamity = Set("StainedSmearedCalamity");
        public static readonly int WulfrumExcavator = Set("ScourgeoftheScrapyard");
        public static readonly int Origen = Set("AntarcticReinforcementStructuredMix");
        public static readonly int Acidsighter = Set("Opticatalysis");
        public static readonly int Carcinogen = Set("OncologicReinforcement");

        public static readonly int Derellect = Set("SignalInterruption");
        public static readonly int Polyphemalus = Set("EyesofFlame");
        public static readonly int PolyphemalusAlt = Set("TheEyesareAnger");
        public static readonly int Phytogen = Set("BotanicReinforcement");
        public static readonly int Hydrogen = Set("AtomicReinforcement");
        public static readonly int Oxygen = Set("AerobicReinforcement");
        public static readonly int Ionogen = Set("IonicReinforcement");
        public static readonly int Pathogen = Set("MicrobicReinforcement");
        public static readonly int EmpressofLightDay = Set("Gegenschein");

        public static readonly int Pyrogen = Set("VolcanicReinforcement");
        public static readonly int Cryogen = Set("NotVolcanicReinforcement");

        public static readonly int DevourerofGods = Set("DoGRemix");
        public static readonly int DevourerofGodsFinalForm = Set("DoGRemix2");
        public static readonly int Hypnos = Set("CerebralAugmentations");

        public static readonly int ExoMechs = Set("Exos/XO");
        public static readonly int ExoTwins = Set("Exos/Apingas");
        public static readonly int Ares = Set("Exos/Larry");
        public static readonly int Thanatos = Set("Exos/Thanos");
        public static readonly int ExoTwinsThanatos = Set("Exos/ApingasThanos");
        public static readonly int ExoTwinsAres = Set("Exos/ApingasLarry");
        public static readonly int ThanatosAres = Set("Exos/ThanosLarry");

        // Misc
        public static readonly int Menu = Set("Menu");
        public static readonly int Menu2 = Set("CrazyLaPaint");
        public static readonly int TrueStory = Set("TrueStory");
        public static readonly int PlasticOracle = Set("OhmnOmens");

        private static int Set(string name) => MusicLoader.GetMusicSlot(CalRemix.instance, $"{Path}{name}");
    }
}
