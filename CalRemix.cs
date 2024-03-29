using CalamityMod;
using CalamityMod.Items.Accessories;
using CalamityMod.UI.CalamitasEnchants;
using CalRemix.CrossCompatibility.OutboundCompatibility;
using CalRemix.NPCs;
using CalRemix.NPCs.Minibosses;
using CalRemix.NPCs.Bosses;
using CalRemix.NPCs.Bosses.Wulfwyrm;
using CalRemix.Items.Accessories;
using System;
using System.IO;
using System.Linq;
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

namespace CalRemix
{
    public class CalRemix : Mod
    {
        public static CalRemix instance;

        public static int CosmiliteCoinCurrencyId;
        public static int KlepticoinCurrencyId;

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
        public override void Load()
        {
            instance = this;

            PlagueGlobalNPC.PlagueHelper = new PlagueJungleHelper();

            CosmiliteCoinCurrencyId = CustomCurrencyManager.RegisterCurrency(new Items.CosmiliteCoinCurrency(ModContent.ItemType<Items.CosmiliteCoin>(), 100L, "Mods.CalRemix.Currencies.CosmiliteCoinCurrency"));
            KlepticoinCurrencyId = CustomCurrencyManager.RegisterCurrency(new Items.KlepticoinCurrency(ModContent.ItemType<Items.Klepticoin>(), 100L, "Mods.CalRemix.Currencies.Klepticoin"));

            if (!Main.dedServ)
            {
                Terraria.Graphics.Effects.Filters.Scene["CalRemix:PlagueBiome"] = new Filter(new PlagueSkyData("FilterMiniTower").UseColor(Color.Green).UseOpacity(0.15f), EffectPriority.VeryHigh);
                SkyManager.Instance["CalRemix:PlagueBiome"] = new PlagueSky();
                Terraria.Graphics.Effects.Filters.Scene["CalRemix:CalamitySky"] = new Filter(new ScreenShaderData("FilterMoonLord"), EffectPriority.High);
                SkyManager.Instance["CalRemix:CalamitySky"] = new CalamitySky();
            }
        }
        public override void Unload()
        {
            PlagueGlobalNPC.PlagueHelper = null;
            instance = null;
        }
        public override void PostSetupContent()
        {
            // Calamity Calls
            Mod cal = ModLoader.GetMod("CalamityMod");
            cal.Call("RegisterModPopupGUIs", this);
            cal.Call("RegisterModCooldowns", this);
            cal.Call("DeclareMiniboss", ModContent.NPCType<LifeSlime>());
            cal.Call("DeclareMiniboss", ModContent.NPCType<Clamitas>());
            cal.Call("DeclareMiniboss", ModContent.NPCType<OnyxKinsman>());
            cal.Call("DeclareMiniboss", ModContent.NPCType<CyberDraedon>());
            cal.Call("DeclareMiniboss", ModContent.NPCType<PlagueEmperor>());
            cal.Call("DeclareMiniboss", ModContent.NPCType<YggdrasilEnt>());
            cal.Call("DeclareMiniboss", ModContent.NPCType<KingMinnowsPrime>());
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