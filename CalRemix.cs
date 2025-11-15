using CalamityMod;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.NPCs.CalClone;
using CalamityMod.NPCs.Crabulon;
using CalamityMod.NPCs.ExoMechs;
using CalamityMod.NPCs.Leviathan;
using CalamityMod.NPCs.OldDuke;
using CalamityMod.NPCs.PlaguebringerGoliath;
using CalamityMod.NPCs.CeaselessVoid;
using CalRemix.Content.Items.Accessories;
using CalRemix.Content.Items.ZAccessories;
using CalRemix.Content.NPCs;
using CalRemix.Content.NPCs.Bosses.Acideye;
using CalRemix.Content.NPCs.Bosses.Carcinogen;
using CalRemix.Content.NPCs.Bosses.Hydrogen;
using CalRemix.Content.NPCs.Bosses.Hypnos;
using CalRemix.Content.NPCs.Bosses.Ionogen;
using CalRemix.Content.NPCs.Bosses.Origen;
using CalRemix.Content.NPCs.Bosses.Oxygen;
using CalRemix.Content.NPCs.Bosses.Pathogen;
using CalRemix.Content.NPCs.Bosses.Phytogen;
using CalRemix.Content.NPCs.Bosses.Poly;
using CalRemix.Content.NPCs.Bosses.Pyrogen;
using CalRemix.Content.NPCs.Bosses.Wulfwyrm;
using CalRemix.Content.NPCs.Minibosses;
using CalRemix.Content.NPCs.PandemicPanic;
using CalRemix.Content.Tiles;
using CalRemix.Core.OutboundCompatibility;
using CalRemix.Core.World;
using CalRemix.UI.Title;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using CalRemix.UI.Anomaly109;
using CalRemix.Content.Items.Weapons;
using CalRemix.UI;
using CalRemix.Content.Buffs;

namespace CalRemix
{
    enum RemixMessageType
    {
        Anomaly109Help,
        Anomaly109Sync,
        Anomaly109Unlock,
        ToggleHelpers,
        SyncIonmaster,
        IonQuestLevel,
        OxydayTime,
        TrueStory,
        StartPandemicPanic,
        EndPandemicPanic,
        KillDefender,
        KillInvader,
        ShadeQuestIncrement
    }
    public class CalRemix : Mod
    {
        internal static Mod CalMod = ModLoader.GetMod("CalamityMod");
        internal static Mod CalMusic = ModLoader.GetMod("CalamityModMusic");
        public static CalRemix instance;

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
                case RemixMessageType.Anomaly109Help:
                    {
                        Anomaly109Manager.helpUnlocked = true;
                        CalRemixWorld.UpdateWorldBool();
                        break;
                    }
                case RemixMessageType.Anomaly109Sync:
                    {
                        int optionIndex = reader.ReadInt32();
                        Anomaly109Manager.options[optionIndex].toggle();
                        CalRemixWorld.UpdateWorldBool();
                        break;
                    }
                case RemixMessageType.Anomaly109Unlock:
                    {
                        int optionIndex = reader.ReadInt32(); 
                        Anomaly109Manager.options[optionIndex].unlocked = true;
                        CalRemixWorld.UpdateWorldBool();
                        break;
                    }
                case RemixMessageType.ToggleHelpers:
                    {
                        ScreenHelperManager.screenHelpersEnabled = !ScreenHelperManager.screenHelpersEnabled;
                        CalRemixWorld.UpdateWorldBool();
                        break;
                    }
                case RemixMessageType.SyncIonmaster:
                    {
                        int kennyID = reader.ReadByte();
                        float posX = reader.ReadSingle();
                        float posY = reader.ReadSingle();
                        float desiredX = reader.ReadSingle();
                        float desiredY = reader.ReadSingle();
                        string text = reader.ReadString();
                        float textLife = reader.ReadSingle();
                        int lookedItem = reader.ReadInt32();
                        int itemTimer = reader.ReadInt32();
                        float rotation = reader.ReadSingle();
                        float desRotation = reader.ReadSingle();

                        if (TileEntity.ByID.TryGetValue(kennyID, out TileEntity t))
                        {
                            if (t is IonCubeTE kendrick)
                            {
                                kendrick.positionX = posX;
                                kendrick.positionY = posY;
                                kendrick.desiredX = desiredX;
                                kendrick.desiredY = desiredY;
                                kendrick.rotation = rotation;
                                kendrick.desiredRotation = desRotation;
                                kendrick.lookedAtItem = lookedItem;
                                kendrick.lookingAtItem = itemTimer;
                                kendrick.displayText = text;
                                kendrick.textLifeTime = textLife;
                            }
                        }

                        break;
                    }
                case RemixMessageType.IonQuestLevel:
                    {
                        int level = reader.ReadByte();
                        CalRemixWorld.ionQuestLevel = level;
                        break;
                    }
                case RemixMessageType.OxydayTime:
                    {
                        int oxygenTime = reader.ReadByte();
                        CalRemixWorld.oxydayTime = oxygenTime;
                        break;
                    }
                case RemixMessageType.TrueStory:
                    {
                        string uuid = reader.ReadString();
                        CalRemixWorld.playerSawTrueStory.Add(uuid);
                        break;
                    }
                case RemixMessageType.StartPandemicPanic:
                    {
                        PandemicPanic.IsActive = true;
                        PandemicPanic.DefendersKilled = 0;
                        PandemicPanic.InvadersKilled = 0;
                        CalRemixWorld.UpdateWorldBool();
                        break;
                    }
                case RemixMessageType.EndPandemicPanic:
                    {
                        PandemicPanic.IsActive = false;
                        PandemicPanic.DefendersKilled = 0;
                        PandemicPanic.InvadersKilled = 0;
                        PandemicPanic.LockedFinalSide = 0;
                        PandemicPanic.SummonedPathogen = false;
                        CalRemixWorld.UpdateWorldBool();
                        break;
                    }
                case RemixMessageType.KillDefender:
                    {
                        int killCount = reader.ReadInt32();
                        PandemicPanic.DefendersKilled = killCount;
                        CalRemixWorld.UpdateWorldBool();
                        break;
                    }
                case RemixMessageType.KillInvader:
                    {
                        int killCount = reader.ReadInt32();
                        PandemicPanic.InvadersKilled = killCount;
                        CalRemixWorld.UpdateWorldBool();
                        break;
                    }
                case RemixMessageType.ShadeQuestIncrement:
                    {
                        int count = reader.ReadInt32();
                        CalRemixWorld.shadeQuestLevel = count;
                        CalRemixWorld.UpdateWorldBool();
                        break;
                    }
            }
        }
        public override void Load()
        {
            instance = this;
            PlagueGlobalNPC.PlagueHelper = new PlagueJungleHelper();
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
            if (!ModLoader.HasMod("InfernumMode") && !ModLoader.HasMod("FargowiltasCrossmod"))
                LoadBossRushEntries(cal);
            RefreshBestiary();

            for (int i = 0; i < ItemLoader.ItemCount; i++)
            {
                if (ItemLoader.GetItem(i) is null)
                    continue;
                ModItem item = ItemLoader.GetItem(i);
                if (item.Type == ItemType<WulfrumMetalScrap>())
                    continue;
                if (!CalRemixAddon.Names.Contains(item.Mod.Name) || Main.itemAnimations[item.Type] != null || item is DebuffStone || item is BouncyRogue || item is StickyRogue || item is AutoloadedLegendPortraitItem || item is LegendMemorialItem || item is LegendPromoItem )
                    continue;
                CalRemixAddon.Items.Add(item);
            }

            // Menu
            if (GetInstance<CalRemixConfig>().forcedMenu)
            {
                try
                {
                    CalRemixConfig config = GetInstance<CalRemixConfig>();
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

            // all for the eight seconds its all worth it
            cal.Call("RegisterDebuff", "CalRemix/Content/Buffs/Bleeding", (NPC npc) => npc.HasBuff(BuffID.Bleeding));

            cal.Call("RegisterDebuff", "CalRemix/Content/Buffs/RealityBearerForClopsBuff", (NPC npc) => npc.HasBuff<RealityBearerForClopsBuff>());

            CalRemixPlayer.LoadDyeStats();
        }
        private static void MenuStuff(ModMenu menu)
        {
            if (menu.FullName is null)
                return;
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
            AddToBossRush(ref brEntries, NPCType<Crabulon>(), NPCType<AcidEye>(), [NPCType<MutatedEye>()], needsNight: true);
            AddToBossRush(ref brEntries, NPCID.Deerclops, NPCType<Carcinogen>(), [NPCType<CarcinogenShield>()]);
            AddToBossRush(ref brEntries, NPCType<CalamitasClone>(), NPCType<Ionogen>(), [NPCType<IonogenShield>()]);
            AddToBossRush(ref brEntries, NPCID.Plantera, NPCType<Oxygen>(), [NPCType<OxygenShield>()]);
            AddToBossRush(ref brEntries, NPCType<Anahita>(), NPCType<Polyphemalus>(), [NPCType<Astigmageddon>(), NPCType<Exotrexia>(), NPCType<Conjunctivirus>(), NPCType<Cataractacomb>()], [NPCType<Astigmageddon>(), NPCType<Exotrexia>(), NPCType<Conjunctivirus>(), NPCType<Cataractacomb>()], true);
            AddToBossRush(ref brEntries, NPCID.Golem, NPCType<Phytogen>(), [NPCType<PhytogenShield>(), NPCType<PineappleFrond>()]);
            AddToBossRush(ref brEntries, NPCType<PlaguebringerGoliath>(), NPCType<Hydrogen>(), [NPCType<HydrogenShield>()]);
            AddToBossRush(ref brEntries, NPCID.CultistBoss, NPCType<Pathogen>(), [NPCType<PathogenShield>()]);
            AddToBossRush(ref brEntries, NPCType<CeaselessVoid>(), NPCType<Pyrogen>(), [NPCType<PyrogenShield>()]);

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
            if (bossidx == -1)
                return;
            int[] headID = [NPCType];
            if (needsDead != default)
            {
                headID = needsDead;
            }
            Action<int> pr2 = delegate (int npc)
            {
                CalRemixHelper.SpawnNPCOnPlayer(CalamityMod.Events.BossRushEvent.ClosestPlayerToWorldCenter, NPCType);
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
    }
}