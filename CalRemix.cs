using CalamityMod;
using CalamityMod.Items.Accessories;
using CalRemix.CrossCompatibility.OutboundCompatibility;
using CalRemix.NPCs;
using CalRemix.NPCs.Minibosses;
using CalRemix.NPCs.Bosses.Wulfwyrm;
using CalRemix.Items.Accessories;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ID;
using Microsoft.Xna.Framework;
using CalRemix.Backgrounds.Plague;
using Terraria.Graphics.Effects;
using CalamityMod.NPCs.HiveMind;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using CalRemix.Scenes;
using Terraria.Graphics.Shaders;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;
using CalRemix.Subworlds;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.Audio;
using System.IO;
using CalamityMod.NPCs.OldDuke;
using CalRemix.UI.Title;
using CalRemix.NPCs.Bosses.Carcinogen;
using System.Linq;
using CalRemix.NPCs.PandemicPanic;
using CalRemix.NPCs.Bosses.Hypnos;

namespace CalRemix
{
    enum HypnosMessageType
    {
        HypnosSummoned
    }
    public class CalRemix : Mod
    {
        public static CalRemix instance;
        public static Mod CalVal;

        public static int CosmiliteCoinCurrencyId;
        public static int KlepticoinCurrencyId;

        internal static Effect SlendermanShader;
        internal static Effect ShieldShader;
        internal static Effect LeanShader;

        public static Asset<Texture2D> sunOG = null;
        public static Asset<Texture2D> sunReal = null;
        public static Asset<Texture2D> sunCreepy = null;
        public static Asset<Texture2D> sunOxy = null;
        public static Asset<Texture2D> sunOxy2 = null;

        public static Type calvalFanny = null;
        public static Type calvalFannyBox = null;

        public static readonly SoundStyle Silence = new($"{nameof(CalRemix)}/Sounds/EmptySound");

        public static readonly List<string> CalamityAddons = new List<string>()
        {
            "ApothTestMod",
            "Bloopsitems",
            "CalamityHunt",
            "CalamityLootSwap",
            "CalamityMod",
            "CalamityModMusic",
            "CalRemix",
            "CalValEX",
            "CJMOD",
            "Clamity",
            "CatalystMod",
            "InfernumMode",
            "NoxusBoss",
            "UnCalamityModMusic"
        };
        public static List<ModItem> CalamityAddonItems = new List<ModItem>();

        public static List<int> oreList = new List<int>
        {
            TileID.Copper,
            TileID.Tin,
            TileID.Iron,
            TileID.Lead,
            TileID.Silver,
            TileID.Tungsten,
            TileID.Gold,
            TileID.Platinum
        };
        // Defer mod call handling to the extraneous mod call manager.
        public override object Call(params object[] args) => ModCallManager.Call(args);
        private static void RegisterSceneFilter(ScreenShaderData passReg, string registrationName, EffectPriority priority = EffectPriority.High)
        {
            string prefixedRegistrationName = "CalRemix:" + registrationName;
            Terraria.Graphics.Effects.Filters.Scene[prefixedRegistrationName] = new Filter(passReg, priority);
            Terraria.Graphics.Effects.Filters.Scene[prefixedRegistrationName].Load();
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

        public override void Load()
        {
            instance = this;
            ModLoader.TryGetMod("CalValEX", out CalVal);


            if (CalVal != null)
            {
                Type[] r = CalVal.Code.GetTypes();
                foreach (Type mn in r)
                {
                    if (mn.Name == "Fanny")
                    {
                        calvalFanny = mn;
                    }
                    if (mn.Name == "FannyTextbox")
                    {
                        calvalFannyBox = mn;
                    }
                    if (calvalFannyBox !=null && calvalFanny != null)
                    {
                        break;
                    }
                }
            }

            PlagueGlobalNPC.PlagueHelper = new PlagueJungleHelper();

            CosmiliteCoinCurrencyId = CustomCurrencyManager.RegisterCurrency(new Items.Ammo.CosmiliteCoinCurrency(ModContent.ItemType<Items.Ammo.CosmiliteCoin>(), 100L, "Mods.CalRemix.Currencies.CosmiliteCoinCurrency"));
            KlepticoinCurrencyId = CustomCurrencyManager.RegisterCurrency(new Items.Ammo.KlepticoinCurrency(ModContent.ItemType<Items.Ammo.Klepticoin>(), 100L, "Mods.CalRemix.Currencies.Klepticoin"));

            if (!Main.dedServ)
            {
                Terraria.Graphics.Effects.Filters.Scene["CalRemix:PlagueBiome"] = new Filter(new PlagueSkyData("FilterMiniTower").UseColor(Color.Green).UseOpacity(0.15f), EffectPriority.VeryHigh);
                SkyManager.Instance["CalRemix:PlagueBiome"] = new PlagueSky();
                Terraria.Graphics.Effects.Filters.Scene["CalRemix:CalamitySky"] = new Filter(new ScreenShaderData("FilterMoonLord"), EffectPriority.High);
                SkyManager.Instance["CalRemix:CalamitySky"] = new CalamitySky();
                Terraria.Graphics.Effects.Filters.Scene["CalRemix:Exosphere"] = new Filter(new ExosphereScreenShaderData("FilterMiniTower").UseColor(ExosphereSky.DrawColor).UseOpacity(0.25f), EffectPriority.VeryHigh);
                SkyManager.Instance["CalRemix:Exosphere"] = new ExosphereSky();
                Terraria.Graphics.Effects.Filters.Scene["CalRemix:Fanny"] = new Filter(new FannyScreenShaderData("FilterMiniTower").UseColor(FannySky.DrawColor).UseOpacity(0.25f), EffectPriority.VeryHigh);
                SkyManager.Instance["CalRemix:Fanny"] = new FannySky();

                Terraria.Graphics.Effects.Filters.Scene["CalRemix:Slenderman"] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(0f, 0f, 0f).UseOpacity(0f), EffectPriority.VeryHigh);
                Terraria.Graphics.Effects.Filters.Scene["CalRemix:Asbestos"] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(Color.Gray).UseOpacity(0.5f), EffectPriority.VeryHigh);
                SkyManager.Instance["CalRemix:Asbestos"] = new CarcinogenSky();
                Terraria.Graphics.Effects.Filters.Scene["CalRemix:AcidSight"] = new Filter(new ScreenShaderData(ModContent.Request<Effect>("CalRemix/Effects/AcidSight", AssetRequestMode.ImmediateLoad), "AcidPass"), EffectPriority.VeryHigh);
                Terraria.Graphics.Effects.Filters.Scene["CalRemix:LeanVision"] = new Filter(new ScreenShaderData(ModContent.Request<Effect>("CalRemix/Effects/LeanVision", AssetRequestMode.ImmediateLoad), "LeanPass"), EffectPriority.VeryHigh);
                Terraria.Graphics.Effects.Filters.Scene["CalRemix:PandemicPanic"] = new Filter(new PandemicPanicScreenShaderData("FilterMiniTower").UseColor(ExosphereSky.DrawColor).UseOpacity(0f), EffectPriority.VeryHigh);
                SkyManager.Instance["CalRemix:PandemicPanic"] = new PandemicSky();
            }

            AssetRepository calAss = instance.Assets;
            Effect LoadShader(string path) => calAss.Request<Effect>("Effects/" + path, AssetRequestMode.ImmediateLoad).Value;
            SlendermanShader = LoadShader("SlendermanStatic");
            RegisterMiscShader(SlendermanShader, "StaticPass", "SlendermanStaticShader");
            ShieldShader = LoadShader("HoloShield");
            RegisterScreenShader(ShieldShader, "ShieldPass", "HoloShieldShader");

            sunOG = TextureAssets.Sun3;
            sunReal = TextureAssets.Sun;
            sunCreepy = ModContent.Request<Texture2D>("CalRemix/ExtraTextures/Eclipse");
            sunOxy = ModContent.Request<Texture2D>("CalRemix/ExtraTextures/Oxysun");
            sunOxy2 = ModContent.Request<Texture2D>("CalRemix/ExtraTextures/Oxysun2");
        }

        public override void Unload()
        {
            PlagueGlobalNPC.PlagueHelper = null;
            instance = null;
            CalVal = null;
        }
        public override void PostSetupContent()
        {
            // Calamity Calls
            Mod cal = ModLoader.GetMod("CalamityMod");
            cal.Call("DeclareMiniboss", ModContent.NPCType<LifeSlime>());
            cal.Call("DeclareMiniboss", ModContent.NPCType<Clamitas>());
            cal.Call("DeclareMiniboss", ModContent.NPCType<OnyxKinsman>());
            cal.Call("DeclareMiniboss", ModContent.NPCType<CyberDraedon>());
            cal.Call("DeclareMiniboss", ModContent.NPCType<PlagueEmperor>());
            cal.Call("DeclareMiniboss", ModContent.NPCType<YggdrasilEnt>());
            cal.Call("DeclareMiniboss", ModContent.NPCType<KingMinnowsPrime>());
            cal.Call("DeclareMiniboss", ModContent.NPCType<Dendritiator>());
            cal.Call("MakeItemExhumable", ModContent.ItemType<YharimsGift>(), ModContent.ItemType<YharimsCurse>());
            //cal.Call("DeclareOneToManyRelationshipForHealthBar", ModContent.NPCType<DerellectBoss>(), ModContent.NPCType<SignalDrone>());
            //cal.Call("DeclareOneToManyRelationshipForHealthBar", ModContent.NPCType<DerellectBoss>(), ModContent.NPCType<DerellectPlug>());

            AddEnchantments(cal);
            LoadBossRushEntries(cal);

            if (Main.netMode != NetmodeID.Server)
            {
                Main.QueueMainThreadAction(() =>
                {
                    cal.Call("LoadParticleInstances", instance);
                });
            }
            AddHiveBestiary(ModContent.NPCType<DankCreeper>(), "When threatened by outside forces, chunks of the Hive Mind knocked loose in combat will animate in attempt to subdue their attacker. Each Creeper knocked loose shrinks the brain ever so slightly- though this is an inherently selfdestructive self defense mechanism, any survivors will rejoin with the main body should the threat pass.");
            AddHiveBestiary(ModContent.NPCType<HiveBlob>(), "Clustering globs ejected from the Hive Mind. The very nature of these balls of matter act as a common example of the convergent properties that the Corruption's microorganisms possess.");
            AddHiveBestiary(ModContent.NPCType<DarkHeart>(), "Flying sacs filled with large amounts of caustic liquid. The Hive Mind possesses a seemingly large amount of these hearts, adding to its strange biology.");
            RefreshBestiary();

            try
            {
                // Yes I am nullchecking all of these
                if (typeof(MenuLoader).GetMethod("OffsetModMenu", BindingFlags.Static | BindingFlags.NonPublic) is null)
                    return;
                if (typeof(MenuLoader).GetField("LastSelectedModMenu", BindingFlags.Static | BindingFlags.NonPublic) is null)
                    return;
                ModMenu menu = Main.rand.NextBool(4) ? CalRemixMenu2.Instance : CalRemixMenu.Instance;
                if (menu is null)
                    return;
                MenuStuff(menu);
            }
            catch (Exception e)
            {
                Console.WriteLine("\n\n\n\n\n\n\n\n\n\n");
                Console.WriteLine("CalRemixMenu");
                Console.WriteLine(e.ToString());
                Console.WriteLine("\n\n\n\n\n\n\n\n\n\n");
            }
            for (int i = 0; i < ItemLoader.ItemCount; i++)
            {
                if (ItemLoader.GetItem(i) is null)
                    continue;
                ModItem item = ItemLoader.GetItem(i);
                if (!CalamityAddons.Contains(item.Mod.Name) || Main.itemAnimations[item.Type] != null)
                    continue;
                CalamityAddonItems.Add(item);
            }
        }
        private void MenuStuff(ModMenu menu)
        {
            if (menu.FullName is null)
                return;
            typeof(MenuLoader).GetMethod("OffsetModMenu", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { Main.rand.Next(-2, 3) });
            typeof(MenuLoader).GetField("LastSelectedModMenu", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, menu.FullName);

            if ((ModMenu)typeof(MenuLoader).GetField("switchToMenu", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) is null || menu is null)
                return;
            if (((ModMenu)typeof(MenuLoader).GetField("switchToMenu", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null)).FullName is null || menu.FullName is null)
                return;
        }
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            HypnosMessageType msgType = (HypnosMessageType)reader.ReadByte();
            switch (msgType)
            {
                case HypnosMessageType.HypnosSummoned:
                    int player = reader.ReadByte();

                    Hypnos.SummonDraedon(Main.player[player]);
                    break;
            }
        }

        internal void AddEnchantments(Mod cal)
        {
            LocalizedText defiant = Language.GetOrRegister($"Mods.{nameof(CalRemix)}.Enchantments.Defiant.Name");
            LocalizedText defiantDesc = Language.GetOrRegister($"Mods.{nameof(CalRemix)}.Enchantments.Defiant.Description");
            cal.Call("CreateEnchantment", defiant, defiantDesc, 150, new Predicate<Item>(DefiantEnchantable), "CalRemix/ExtraTextures/Enchantments/EnchantmentRuneDefiant", delegate (Player player)
            {
                player.GetModPlayer<CalRemixPlayer>().earthEnchant = true;
            });

            LocalizedText fallacious = Language.GetOrRegister($"Mods.{nameof(CalRemix)}.Enchantments.Fallacious.Name");
            LocalizedText fallaciousDesc = Language.GetOrRegister($"Mods.{nameof(CalRemix)}.Enchantments.Fallacious.Description");
            cal.Call("CreateEnchantment", fallacious, fallaciousDesc, 156, new Predicate<Item>(FallaciousEnchantable), "CalRemix/ExtraTextures/Enchantments/EnchantmentRuneFallacious", delegate (Player player)
            {
                player.GetModPlayer<CalRemixPlayer>().amongusEnchant = true;
            });
        }
        internal static bool DefiantEnchantable(Item item)
        {
            return item.IsEnchantable() && item.damage > 0;
        }
        internal static bool FallaciousEnchantable(Item item)
        {
            return item.IsEnchantable() && item.damage > 0 && !item.CountsAsClass<SummonDamageClass>() && !item.IsWhip();
        }
        internal static void LoadBossRushEntries(Mod cal)
        {
            List<(int, int, Action<int>, int, bool, float, int[], int[])> brEntries = (List<(int, int, Action<int>, int, bool, float, int[], int[])>)cal.Call("GetBossRushEntries");
            int[] excIDs = { ModContent.NPCType<WulfwyrmBody>(), ModContent.NPCType<WulfwyrmTail>() };
            int[] headID = { ModContent.NPCType<WulfwyrmHead>() };
            Action<int> pr = delegate (int npc)
            {
                NPC.SpawnOnPlayer(CalamityMod.Events.BossRushEvent.ClosestPlayerToWorldCenter, ModContent.NPCType<WulfwyrmHead>());
            };
            brEntries.Insert(0, (ModContent.NPCType<WulfwyrmHead>(), -1, pr, 180, false, 0f, excIDs, headID));
            foreach (var entry in brEntries)
            {
                if (entry.Item1 == ModContent.NPCType<OldDuke>())
                {
                    brEntries.Remove(entry);
                    break;
                }
            }
            int[] excIDs2 = { ModContent.NPCType<AergiaNeuron>(), ModContent.NPCType<HypnosPlug>() };
            int[] headID2 = { ModContent.NPCType<Hypnos>() };
            Action<int> pr2 = delegate (int npc)
            {
                NPC.SpawnOnPlayer(CalamityMod.Events.BossRushEvent.ClosestPlayerToWorldCenter, ModContent.NPCType<Hypnos>());
            };
            brEntries.Insert(brEntries.Count() - 2, (ModContent.NPCType<Hypnos>(), -1, pr2, 180, false, 0f, excIDs2, headID2));
            cal.Call("SetBossRushEntries", brEntries);
        }

        public static void AddHiveBestiary(int id, string entryText)
        {
            NPCID.Sets.NPCBestiaryDrawModifiers modifiers = new NPCID.Sets.NPCBestiaryDrawModifiers();
            modifiers.Hide = false;
            if (id == ModContent.NPCType<DankCreeper>())
            {
                modifiers.CustomTexturePath = "CalRemix/Retheme/HiveMind/DankCreeper";
            }
            if (id == ModContent.NPCType<HiveBlob>())
            {
                modifiers.CustomTexturePath = "CalRemix/Retheme/HiveMind/HiveBlob";
            }
            if (id == ModContent.NPCType<DarkHeart>())
            {
                modifiers.PortraitPositionXOverride = 10;
                modifiers.PortraitPositionYOverride = 20;
            }

            NPCID.Sets.NPCBestiaryDrawOffset[id] = modifiers;
            BestiaryEntry b = new BestiaryEntry();
            b.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCorruption,
        new FlavorTextBestiaryInfoElement(entryText)
            });
            int associatedNPCType = ModContent.NPCType<HiveMind>();
            b.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[associatedNPCType], quickUnlock: true);
            ContentSamples.NpcsByNetId[id].ModNPC?.SetBestiary(Main.BestiaryDB, b);
        }

        public static void RefreshBestiary()
        {
            ContentSamples.RebuildBestiarySortingIDsByBestiaryDatabaseContents(Main.BestiaryDB);
            ItemDropDatabase itemDropDatabase = new ItemDropDatabase();
            itemDropDatabase.Populate();
            Main.ItemDropsDB = itemDropDatabase;
            Main.BestiaryDB.Merge(Main.ItemDropsDB);
            if (!Main.dedServ)
            {
                Main.BestiaryUI = new Terraria.GameContent.UI.States.UIBestiaryTest(Main.BestiaryDB);
            }
        }
        internal static void AddToShop(int type, int price, ref Chest shop, ref int nextSlot, bool condition = true, int specialMoney = 0)
        {
            if (!condition || shop is null) return;
            shop.item[nextSlot].SetDefaults(type);
            shop.item[nextSlot].shopCustomPrice = price > 0 ? price : shop.item[nextSlot].value;
            if (specialMoney == 1) shop.item[nextSlot].shopSpecialCurrency = CosmiliteCoinCurrencyId;
            else if (specialMoney == 2) shop.item[nextSlot].shopSpecialCurrency = KlepticoinCurrencyId;
            nextSlot++;
        }
    }
}