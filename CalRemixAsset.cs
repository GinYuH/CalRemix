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
using CalRemix.Core.Biomes.Subworlds;

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
                Filters.Scene["CalRemix:Slenderman"] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(0f, 0f, 0f).UseOpacity(0f), EffectPriority.VeryHigh);
                Filters.Scene["CalRemix:NormalDraw"] = new Filter(new ScreenShaderData(Request<Effect>("CalRemix/Assets/Effects/NormalDraw"), "NormalDrawPass"), EffectPriority.VeryHigh);
                Filters.Scene["CalRemix:VoidColors"] = new Filter(new ScreenShaderData(Request<Effect>("CalRemix/Assets/Effects/VoidColors"), "VoidPass"), EffectPriority.VeryHigh);

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
                Filters.Scene["CalRemix:PandemicPanic"] = new Filter(new PandemicPanicScreenShaderData("FilterMiniTower").UseColor(ExosphereSky.DrawColor).UseOpacity(0f), EffectPriority.VeryHigh);
                SkyManager.Instance["CalRemix:PandemicPanic"] = new PandemicSky();
                Filters.Scene["CalRemix:ScreamingFaceSky"] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(ScreamingFaceSky.DrawColor).UseOpacity(0.25f), EffectPriority.VeryHigh);
                SkyManager.Instance["CalRemix:ScreamingFaceSky"] = new ScreamingFaceSky();
                Filters.Scene["CalRemix:ClownWorldSky"] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(ClownWorldSky.DrawColor).UseOpacity(0.25f), EffectPriority.VeryHigh);
                SkyManager.Instance["CalRemix:ClownWorldSky"] = new ClownWorldSky();
                Filters.Scene["CalRemix:EaterSky"] = new Filter(new ScreenShaderData("FilterMiniTower").UseOpacity(0f), EffectPriority.VeryHigh);
                SkyManager.Instance["CalRemix:EaterSky"] = new EaterSky();


                Filters.Scene["CalRemix:Sealed"] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(Color.White).UseOpacity(0f), EffectPriority.Medium);
                SkyManager.Instance["CalRemix:Sealed"] = new SealedSky();
                Filters.Scene["CalRemix:Disilphia"] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(Color.White).UseOpacity(0f), EffectPriority.Medium);
                SkyManager.Instance["CalRemix:Disilphia"] = new DisilphiaSky();
                Filters.Scene["CalRemix:HorizonSky"] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(Color.White).UseOpacity(0f), EffectPriority.Medium);
                SkyManager.Instance["CalRemix:HorizonSky"] = new HorizonSky();
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
        public static readonly int Exosphere = Set("Biomes/Subworlds/Exosphere");
        public static readonly int ClownWorld = Set("Biomes/Subworlds/ClownWorld");

        // Biomes
        public static readonly int AsbestosCaves = Set("Biomes/AsbestosCaves");
        public static readonly int PlaguedJungle = Set("Biomes/PlaguedJungle");
        public static readonly int BaronStrait = Set("Biomes/BaronStrait");
        public static readonly int VernalPass = Set("Biomes/VernalPass");
        public static readonly int FrozenStronghold = Set("Biomes/FrozenStronghold");
        public static readonly int ProfanedDesert = Set("Biomes/ProfanedDesert");

        // Events
        public static readonly int AcidRainTier2 = Set("Events/AcidRainTier2");
        public static readonly int PandemicPanic = Set("Events/PandemicPanic");
        public static readonly int GaleforceDay = Set("Events/GaleforceDay");

        // NPCs
        public static readonly int CryoSlime = Set("CryoSlime");

        // Minibosses
        public static readonly int LaRuga = Set("Bosses/Minibosses/LaRuga");
        public static readonly int DemonChase = Set("DemonChase");

        // Bosses
        public static readonly int TheCalamity = Set("Bosses/TheCalamity");
        public static readonly int WulfrumExcavator = Set("Bosses/WulfrumExcavator");
        public static readonly int Origen = Set("Bosses/Origen");
        public static readonly int Acidsighter = Set("Bosses/Acidsighter");
        public static readonly int Carcinogen = Set("Bosses/Carcinogen");

        public static readonly int Derellect = Set("Bosses/Derellect");
        public static readonly int Polyphemalus = Set("Bosses/Polyphemalus");
        public static readonly int PolyphemalusAlt = Set("Bosses/PolyphemalusAlt");
        public static readonly int Phytogen = Set("Bosses/Phytogen");
        public static readonly int Hydrogen = Set("Bosses/Hydrogen");
        public static readonly int Oxygen = Set("Bosses/Oxygen");
        public static readonly int Ionogen = Set("Bosses/Ionogen");
        public static readonly int Pathogen = Set("Bosses/Pathogen");
        public static readonly int EmpressofLightDay = Set("Bosses/EmpressOfLightDay");
        public static readonly int SealedOnePhase1 = Set("Bosses/SealedOnePhase1");
        public static readonly int SealedOnePhase2 = Set("Bosses/SealedOnePhase2");

        public static readonly int Pyrogen = Set("Bosses/Pyrogen");
        public static readonly int Cryogen = Set("Bosses/PyrogenFTB");
        public static readonly int DevourerofGods = Set("Bosses/DevourerOfGods");
        public static readonly int DevourerofGodsFinalForm = Set("Bosses/DevourerOfGodsFinalForm");
        public static readonly int Hypnos = Set("Bosses/Hypnos");

        public static readonly int ExoMechs = Set("Bosses/Exos/XO");
        public static readonly int ExoTwins = Set("Bosses/Exos/Apingas");
        public static readonly int Ares = Set("Bosses/Exos/Larry");
        public static readonly int Thanatos = Set("Bosses/Exos/Thanos");
        public static readonly int ExoTwinsThanatos = Set("Bosses/Exos/ApingasThanos");
        public static readonly int ExoTwinsAres = Set("Bosses/Exos/ApingasLarry");
        public static readonly int ThanatosAres = Set("Bosses/Exos/ThanosLarry");

        public static readonly int RenoxPhase2 = Set("Stelliferous");
        public static readonly int RenoxPhase3 = Set("Degenerate");

        // Vanilla Replacement 
        public static readonly int BloodMoonRemix = Set("Misc/VanillaRemix/BloodMoon");
        public static readonly int DesertRemix = Set("Misc/VanillaRemix/Desert");
        public static readonly int DesertRemixles = Set("Misc/VanillaRemix/DesertVanilla");
        public static readonly int QueenSlimeRemix = Set("Misc/VanillaRemix/QueenSlime");
        public static readonly int ShimmerRemix = Set("Misc/VanillaRemix/Shimmer");
        public static readonly int SulphSeaDayRemix = Set("Misc/VanillaRemix/probably-not-wasteland-by-dm-dokuro-unlisted-on-my-channel-for-my-playlists-and-such-128-ytshorts.savetube.me");

        // Misc
        public static readonly int Menu = Set("Misc/Menu");
        public static readonly int Menu2 = Set("Misc/Menu2");
        public static readonly int TrueStory = Set("Misc/TrueStory");
        public static readonly int PlasticOracle = Set("Misc/PlasticOracle");
        public static readonly int Generator = Set("Misc/Generator");

        private static int Set(string name) => MusicLoader.GetMusicSlot(CalRemix.instance, $"{Path}{name}");
    }
}
