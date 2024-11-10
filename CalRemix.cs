using static Terraria.ModLoader.ModContent;
using CalamityMod;
using CalamityMod.Items.Accessories;
using CalRemix.Core.OutboundCompatibility;
using CalRemix.Content.NPCs;
using CalRemix.Content.NPCs.Minibosses;
using CalRemix.Content.NPCs.Bosses.Wulfwyrm;
using CalRemix.Content.Items.Accessories;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;
using System.Reflection;
using System.IO;
using CalamityMod.NPCs.OldDuke;
using CalRemix.UI.Title;
using CalRemix.Content.NPCs.Bosses.Carcinogen;
using CalRemix.Content.NPCs.PandemicPanic;
using CalRemix.Content.NPCs.Bosses.Hypnos;
using CalRemix.Content.NPCs.Bosses.Phytogen;
using CalRemix.Content.NPCs.Bosses.Origen;
using CalRemix.Content.NPCs.Bosses.Ionogen;
using CalamityMod.NPCs.CalClone;
using CalRemix.Content.NPCs.Bosses.Oxygen;
using CalRemix.Content.NPCs.Bosses.Hydrogen;
using CalamityMod.NPCs.PlaguebringerGoliath;
using CalRemix.Content.NPCs.Bosses.Pathogen;
using CalamityMod.NPCs.Crabulon;
using CalRemix.Content.NPCs.Bosses.Acideye;
using CalamityMod.NPCs.Leviathan;
using CalRemix.Content.NPCs.Bosses.Poly;
using CalamityMod.NPCs.ExoMechs;
using CalRemix.Content.Items.Ammo;
using CalamityMod.Items.Materials;
using CalRemix.Content.Items.ZAccessories;

namespace CalRemix
{
    enum RemixMessageType
    {
        HypnosSummoned,
        PandemicPanicStart
    }
    public class CalRemix : Mod
    {
        internal static Mod CalMod = ModLoader.GetMod("CalamityMod");
        internal static Mod CalMusic = ModLoader.GetMod("CalamityModMusic");
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
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            RemixMessageType msgType = (RemixMessageType)reader.ReadByte();
            switch (msgType)
            {
                case RemixMessageType.HypnosSummoned:
                    {
                        int player = reader.ReadByte();

                        Hypnos.SummonDraedon(Main.player[player]);
                        break;
                    }
                case RemixMessageType.PandemicPanicStart:
                    {
                        int player = reader.ReadByte();

                        PandemicPanic.StartEvent(Main.player[player]);
                        break;
                    }
            }
        }
        public override void Load()
        {
            instance = this;
            PlagueGlobalNPC.PlagueHelper = new PlagueJungleHelper();

            CosmiliteCoinCurrencyId = CustomCurrencyManager.RegisterCurrency(new CosmiliteCoinCurrency(ItemType<CosmiliteCoin>(), 100L, "Mods.CalRemix.Currencies.CosmiliteCoinCurrency"));
            KlepticoinCurrencyId = CustomCurrencyManager.RegisterCurrency(new KlepticoinCurrency(ItemType<Klepticoin>(), 100L, "Mods.CalRemix.Currencies.Klepticoin"));
        }

        public override void Unload()
        {
            instance = null;
            PlagueGlobalNPC.PlagueHelper = null;
        }
        public override void PostSetupContent()
        {
            // Calamity Calls
            Mod cal = CalMod;
            cal.Call("DeclareMiniboss", NPCType<LifeSlime>());
            cal.Call("DeclareMiniboss", NPCType<Clamitas>());
            cal.Call("DeclareMiniboss", NPCType<OnyxKinsman>());
            cal.Call("DeclareMiniboss", NPCType<CyberDraedon>());
            cal.Call("DeclareMiniboss", NPCType<PlagueEmperor>());
            cal.Call("DeclareMiniboss", NPCType<YggdrasilEnt>());
            cal.Call("DeclareMiniboss", NPCType<KingMinnowsPrime>());
            cal.Call("DeclareMiniboss", NPCType<Dendritiator>());
            cal.Call("MakeItemExhumable", ItemType<YharimsGift>(), ItemType<YharimsCurse>());
            cal.Call("DeclareOneToManyRelationshipForHealthBar", NPCType<Hypnos>(), NPCType<HypnosPlug>());
            cal.Call("DeclareOneToManyRelationshipForHealthBar", NPCType<Phytogen>(), NPCType<PineappleFrond>());
            //cal.Call("DeclareOneToManyRelationshipForHealthBar", NPCType<DerellectBoss>(), NPCType<SignalDrone>());
            //cal.Call("DeclareOneToManyRelationshipForHealthBar", NPCType<DerellectBoss>(), NPCType<DerellectPlug>());
            AddEnchantments(cal);
            if (!ModLoader.HasMod("InfernumMode"))
                LoadBossRushEntries(cal);
            RefreshBestiary();

            for (int i = 0; i < ItemLoader.ItemCount; i++)
            {
                if (ItemLoader.GetItem(i) is null)
                    continue;
                ModItem item = ItemLoader.GetItem(i);
                if (item.Type == ItemType<WulfrumMetalScrap>())
                    continue;
                if (!CalRemixAddon.Names.Contains(item.Mod.Name) || Main.itemAnimations[item.Type] != null || item is DebuffStone)
                    continue;
                CalRemixAddon.Items.Add(item);
            }

            // Menu
            if (GetInstance<CalRemixConfig>().forcedMenu)
            {
                try
                {
                    CalRemixConfig config = GetInstance<CalRemixConfig>();
                    if (typeof(MenuLoader).GetMethod("OffsetModMenu", BindingFlags.Static | BindingFlags.NonPublic) is null)
                        return;
                    if (typeof(MenuLoader).GetField("LastSelectedModMenu", BindingFlags.Static | BindingFlags.NonPublic) is null)
                        return;
                    ModMenu menu = ((Main.rand.NextBool(4) && config.randomMenu) || config.useSecondMenu) ? CalRemixMenu2.Instance : CalRemixMenu.Instance;
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
            }
        }
        private void MenuStuff(ModMenu menu)
        {
            if (menu.FullName is null)
                return;
            typeof(MenuLoader).GetMethod("OffsetModMenu", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, [Main.rand.Next(-2, 3)]);
            typeof(MenuLoader).GetField("LastSelectedModMenu", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, menu.FullName);

            if ((ModMenu)typeof(MenuLoader).GetField("switchToMenu", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) is null || menu is null)
                return;
            if (((ModMenu)typeof(MenuLoader).GetField("switchToMenu", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null)).FullName is null || menu.FullName is null)
                return;
        }

        internal void AddEnchantments(Mod cal)
        {
            LocalizedText defiant = Language.GetOrRegister($"Mods.{nameof(CalRemix)}.Enchantments.Defiant.Name");
            LocalizedText defiantDesc = Language.GetOrRegister($"Mods.{nameof(CalRemix)}.Enchantments.Defiant.Description");
            cal.Call("CreateEnchantment", defiant, defiantDesc, 150, new Predicate<Item>(DefiantEnchantable), "CalRemix/Assets/ExtraTextures/Enchantments/EnchantmentRuneDefiant", delegate (Player player)
            {
                player.GetModPlayer<CalRemixPlayer>().earthEnchant = true;
            });

            LocalizedText fallacious = Language.GetOrRegister($"Mods.{nameof(CalRemix)}.Enchantments.Fallacious.Name");
            LocalizedText fallaciousDesc = Language.GetOrRegister($"Mods.{nameof(CalRemix)}.Enchantments.Fallacious.Description");
            cal.Call("CreateEnchantment", fallacious, fallaciousDesc, 156, new Predicate<Item>(FallaciousEnchantable), "CalRemix/Assets/ExtraTextures/Enchantments/EnchantmentRuneFallacious", delegate (Player player)
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

            AddToBossRush(ref brEntries, NPCID.KingSlime, NPCType<WulfwyrmHead>(), [NPCType<WulfwyrmBody>(), NPCType<WulfwyrmTail>()]);
            AddToBossRush(ref brEntries, NPCID.KingSlime, NPCType<Origen>(), [NPCType<OrigenCore>()], [NPCType<OrigenCore>()]);
            AddToBossRush(ref brEntries, NPCType<Crabulon>(), NPCType<Acideye>(), [NPCType<MutatedEye>()]);
            AddToBossRush(ref brEntries, NPCID.Deerclops, NPCType<Carcinogen>(), [NPCType<CarcinogenShield>()]);
            AddToBossRush(ref brEntries, NPCType<CalamitasClone>(), NPCType<Ionogen>(), [NPCType<IonogenShield>()]);
            AddToBossRush(ref brEntries, NPCID.Plantera, NPCType<Oxygen>(), [NPCType<OxygenShield>()]);
            AddToBossRush(ref brEntries, NPCType<Anahita>(), NPCType<Polyphemalus>(), [NPCType<Astigmageddon>(), NPCType<Exotrexia>(), NPCType<Conjunctivirus>(), NPCType<Cataractacomb>()], [NPCType<Astigmageddon>(), NPCType<Exotrexia>(), NPCType<Conjunctivirus>(), NPCType<Cataractacomb>()]);
            AddToBossRush(ref brEntries, NPCID.Golem, NPCType<Phytogen>(), [NPCType<PhytogenShield>(), NPCType<PineappleFrond>()]);
            AddToBossRush(ref brEntries, NPCType<PlaguebringerGoliath>(), NPCType<Hydrogen>(), [NPCType<HydrogenShield>()]);
            AddToBossRush(ref brEntries, NPCID.CultistBoss, NPCType<Pathogen>(), [NPCType<PathogenShield>()]);
            AddToBossRush(ref brEntries, NPCType<Draedon>(), NPCType<Hypnos>(), [NPCType<AergiaNeuron>(), NPCType<HypnosPlug>()]);
            //AddToBossRush(ref brEntries, NPCType<SupremeCalamitas>(), NPCType<Losbaf>(), [NPCType<Losbaf>()]);
            foreach (var entry in brEntries)
            {
                if (entry.Item1 == NPCType<OldDuke>())
                {
                    brEntries.Remove(entry);
                    break;
                }
            }
            cal.Call("SetBossRushEntries", brEntries);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="brEntries">Just pass in the boss rush list, cmon you can do it!</param>
        /// <param name="beforeBossType">The boss that is fought directly after the boss you want to insert</param>
        /// <param name="NPCType">The boss you want to insert</param>
        /// <param name="extraNPCs">Minions/parts of the boss</param>
        /// <param name="needsNight">Sets time to night if true</param>
        public static void AddToBossRush(ref List<(int, int, Action<int>, int, bool, float, int[], int[])> brEntries, int beforeBossType, int NPCType, int[] extraNPCs, int[] needsDead = default, bool needsNight = false, Action<int> customAction = default)
        {
            int bossidx = -1;
            for (int i = 0; i < brEntries.Count; i++)
            {
                if (brEntries[i].Item1 == beforeBossType)
                {
                    bossidx = i;
                    break;
                }
            }
            int[] headID = [NPCType];
            if (needsDead != default)
            {
                headID = needsDead;
            }
            Action<int> pr2 = delegate (int npc)
            {
                NPC.SpawnOnPlayer(CalamityMod.Events.BossRushEvent.ClosestPlayerToWorldCenter, NPCType);
            };
            if (customAction != default)
            {
                pr2 = customAction;
            }
            brEntries.Insert(bossidx, (NPCType, -1, pr2, 45, needsNight, 0f, extraNPCs, headID));
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