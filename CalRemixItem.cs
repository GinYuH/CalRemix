using static Terraria.ModLoader.ModContent;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using CalamityMod.CalPlayer;
using CalRemix.Content.Items.Accessories;
using CalamityMod.Items.Accessories;
using Microsoft.Xna.Framework;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.PermanentBoosters;
using CalamityMod;
using CalRemix.Content.Items.Weapons;
using CalamityMod.Items.Materials;
using System.Collections.Generic;
using CalamityMod.Items.TreasureBags;
using CalRemix.Content.Items.Materials;
using CalamityMod.World;
using CalamityMod.Items.Potions;
using CalamityMod.Items.SummonItems;
using CalRemix.Content.Items.Potions;
using CalamityMod.Items.Weapons.Melee;
using Terraria.GameContent.ItemDropRules;
using CalRemix.Content.NPCs.Minibosses;
using CalRemix.Content.Buffs;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using CalRemix.Content.Projectiles.Accessories;
using Terraria.Audio;
using CalamityMod.Items.Armor.Fearmonger;
using CalRemix.Content.Tiles;
using CalamityMod.NPCs.Perforator;
using CalamityMod.NPCs.HiveMind;
using CalRemix.Content.Items.Placeables;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.TreasureBags.MiscGrabBags;
using CalamityMod.Rarities;
using CalRemix.Content.NPCs;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.NPCs.Other;
using CalamityMod.Items;
using CalamityMod.Items.Placeables;
using CalamityMod.Items.Fishing.AstralCatches;
using CalRemix.Content.Items.Ammo;
using CalRemix.Content.Projectiles;
using CalRemix.Core.World;
using CalRemix.Content.Items.Lore;
using CalamityMod.Items.Accessories.Wings;
using CalRemix.Content.Items.Armor;
using CalRemix.Content.Cooldowns;
using CalamityMod.Items.Potions.Alcohol;
using System;
using CalRemix.Content.NPCs.Bosses.Pyrogen;

namespace CalRemix
{
    public class CalRemixItem : GlobalItem
    {
        public override bool InstancePerEntity => true;
        internal string devItem = string.Empty;
        public bool Scoriad = false;
        public int NonScoria = -1;
        private static readonly Dictionary<int, int> GemCrawl = new()
        {
            { ItemID.Ruby, NPCType<CrawlerRuby>() },
            { ItemID.Amber, NPCType<CrawlerAmber>() },
            { ItemID.Topaz, NPCType<CrawlerTopaz>() },
            { ItemID.Emerald, NPCType<CrawlerEmerald>() },
            { ItemID.Sapphire, NPCType<CrawlerSapphire>() },
            { ItemID.Amethyst, NPCType<CrawlerAmethyst>() },
            { ItemID.Diamond, NPCType<CrawlerDiamond>() },
            { ItemID.CrystalShard, NPCType<CrawlerCrystal>() }
        };

        public static List<int> genSouls = new List<int>()
        {
            ItemType<SoulofPhytogen>(),
            ItemType<SoulofPathogen>(),
            ItemType<SoulofOxygen>(),
            ItemType<SoulofIonogen>(),
            ItemType<SoulofHydrogen>(),
            ItemType<SoulofCryogen>(),
            ItemType<SoulofCarcinogen>(),
        };

        public override void SetDefaults(Item item)
        {
            if (item.type == ItemType<EssenceofHavoc>())
            {
                ItemID.Sets.ShimmerTransformToItem[item.type] = ItemType<EssenceofLaw>();
            }
            else if (item.type == ItemType<EssenceofEleum>())
            {
                ItemID.Sets.ShimmerTransformToItem[item.type] = ItemType<EssenceofCrystal>();
            }
            else if (item.type == ItemType<EssenceofSunlight>())
            {
                ItemID.Sets.ShimmerTransformToItem[item.type] = ItemType<EssenceofMyst>();
            }
            else if (item.type == ItemType<EssenceofBabil>())
            {
                ItemID.Sets.ShimmerTransformToItem[item.type] = ItemType<EssenceofZot>();
            }
            else if (item.type == ItemType<TitanArm>())
            {
                ItemID.Sets.ShimmerTransformToItem[item.type] = ItemType<TitanFinger>();
            }
            if (CalRemixWorld.fearmonger)
            {
                if (item.type == ItemType<FearmongerGreathelm>())
                {
                    item.defense = 2;
                    item.value = Item.sellPrice(silver: 15);
                    item.rare = ItemRarityID.Blue;
                }
                else if (item.type == ItemType<FearmongerPlateMail>())
                {
                    item.defense = 8;
                    item.value = Item.sellPrice(silver: 12);
                    item.rare = ItemRarityID.Blue;
                }
                else if (item.type == ItemType<FearmongerGreaves>())
                {
                    item.defense = 6;
                    item.value = Item.sellPrice(silver: 9);
                    item.rare = ItemRarityID.Blue;
                }
            }
            if (cosmicItems.Contains(item.type))
            {
                item.rare = ItemRarityID.Purple;
            }
            else
            {
                // undo the curve flattening
                // done with content samples so that reforges are ignored
                if (ContentSamples.ItemsByType.ContainsKey(item.type))
                {
                    if (ContentSamples.ItemsByType[item.type].rare == RarityType<Turquoise>())
                    {
                        item.damage = (int)(item.damage * 1.33f);
                    }
                    if (ContentSamples.ItemsByType[item.type].rare == RarityType<PureGreen>())
                    {
                        item.damage = (int)(item.damage * 1.67f);
                    }
                    if (ContentSamples.ItemsByType[item.type].rare == RarityType<DarkBlue>())
                    {
                        item.damage = (int)(item.damage * 2.5f);
                    }
                    if (ContentSamples.ItemsByType[item.type].rare == RarityType<Violet>())
                    {
                        item.damage = (int)(item.damage * 3.33f);
                    }
                    if (ContentSamples.ItemsByType[item.type].rare == RarityType<HotPink>())
                    {
                        item.damage = (int)(item.damage * 5f);
                    }
                }
            }
            if (item.type == ItemType<Navystone>())
            {
                item.createTile = TileType<NavystoneSafe>();
            }
            if (item.type == ItemType<EutrophicSand>())
            {
                item.createTile = TileType<EutrophicSandSafe>();
            }
            if (item.type == ItemType<HardenedEutrophicSand>())
            {
                item.createTile = TileType<HardenedEutrophicSandSafe>();
            }
            if (item.type == ItemType<SeaPrism>())
            {
                item.createTile = TileType<SeaPrismSafe>();
            }
            if (item.type == ItemType<TheBurningSky>())
            {
                item.DamageType = DamageClass.SummonMeleeSpeed;
            }
        }
        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            if (CalRemixWorld.cosmislag)
            {
                if (cosmicItems.Contains(item.type))
                {
                    if (item.damage > 0)
                    {
                        damage *= 0.7f;
                    }
                }
            }
        }
        public override void HoldItem(Item item, Player player)
        {
            if (player.HasBuff<BrimstoneMadness>() && item != null && !item.IsAir && item.damage > 0)
            {   
                if (item.CountsAsClass<SummonDamageClass>() && !item.IsWhip())
                    player.Calamity().cursedSummonsEnchant = true;
                if (!item.CountsAsClass<SummonDamageClass>() && !item.IsWhip())
                {
                    player.Calamity().flamingItemEnchant = true;
                    player.AddBuff(BuffType<WeakBrimstoneFlames>(), 10);
                }
                if (item.CountsAsClass<MagicDamageClass>() && item.mana > 0 && item.type != ItemType<Eternity>())
                    player.Calamity().lifeManaEnchant = true;
                if (item.shoot > ProjectileID.None && !item.IsTrueMelee() && item.type != ItemType<TheFinalDawn>())
                    player.Calamity().farProximityRewardEnchant = true;
                if (item.shoot > ProjectileID.None && !item.IsTrueMelee() && item.type != ItemType<TheFinalDawn>())
                    player.Calamity().closeProximityRewardEnchant = true;
                if (!item.CountsAsClass<SummonDamageClass>() && !item.CountsAsClass<RogueDamageClass>() && !item.channel && item.type != ItemType<HeavenlyGale>())
                {
                    player.Calamity().dischargingItemEnchant = true;
                    item.Calamity().DischargeEnchantExhaustion = CalamityGlobalItem.DischargeEnchantExhaustionCap;
                }
                if (item.CountsAsClass<SummonDamageClass>() && !item.IsWhip())
                    player.Calamity().explosiveMinionsEnchant = true;
                if (item.CountsAsClass<MagicDamageClass>() && item.mana > 0)
                    player.Calamity().manaMonsterEnchant = true;
                if (!item.CountsAsClass<SummonDamageClass>())
                    player.Calamity().witheringWeaponEnchant = true;
                if (item.shoot > ProjectileID.None)
                    player.Calamity().persecutedEnchant = true;
                if (item.shoot > ProjectileID.None && !item.CountsAsClass<SummonDamageClass>() && !item.IsTrueMelee())
                {
                    if (!Main.gameMenu)
                    {
                        player.Calamity().lecherousOrbEnchant = true;
                        bool flag = false;
                        int num = NPCType<LecherousOrb>();
                        ActiveEntityIterator<NPC>.Enumerator enumerator = Main.ActiveNPCs.GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            NPC current = enumerator.Current;
                            if (current.type == num && current.target == player.whoAmI)
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (Main.myPlayer == player.whoAmI && !flag && !player.Calamity().awaitingLecherousOrbSpawn)
                        {
                            player.Calamity().awaitingLecherousOrbSpawn = true;
                            CalamityNetcode.NewNPC_ClientSide(player.Center, num, player);
                        }
                    }
                }
            }
            if (item.type == ItemType<FabsolsVodka>())
            {
                TransformItem(ref item, ItemType<NotFabsolVodka>());
            }
            if (CalRemixWorld.seafood)
            {
                if (item.type == ItemType<Seafood>())
                {
                    TransformItem(ref item, ItemType<SeafoodFood>());
                }
            }
            if (item.type == ItemID.EnchantedSword && !(DownedBossSystem.downedPerforator || DownedBossSystem.downedHiveMind))
            {
                TransformItem(ref item, ItemType<DisenchantedSword>());
            }
        }
        public override bool? UseItem(Item item, Player player)
        {
            if (item.Calamity().AppliedEnchantment != null) 
            {
                if (player.ItemAnimationJustStarted && player.Calamity().dischargingItemEnchant && item.Calamity().AppliedEnchantment.Value.Name != CalamityUtils.GetText("UI.Ephemeral.DisplayName"))
                    item.Calamity().DischargeEnchantExhaustion--;
            }
            return null;
        }
        public override bool PreDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (CalRemixWorld.laruga)
            {
                if (Scoriad)
                {
                    int frameCount = Main.itemAnimations[item.type] != null ? Main.itemAnimations[item.type].FrameCount : 1;
                    Vector2 rand = new Vector2(Main.rand.Next(-4, 5), 0);
                    Color col = item.type == ItemType<HornetRound>() ? Color.Yellow : Color.Red;
                    Main.EntitySpriteDraw(TextureAssets.Item[item.type].Value, position - new Vector2(TextureAssets.Item[item.type].Value.Width * 0.02f, TextureAssets.Item[item.type].Value.Height * 0.1f / frameCount) + rand, frame, col, 0, origin, scale * 1.4f, SpriteEffects.None);
                }
            }
            return true;
        }
        public override bool CanConsumeAmmo(Item weapon, Item ammo, Player player)
        {
            if (player.GetModPlayer<CalRemixPlayer>().clockBar)
                return Main.rand.NextFloat() >= 0.66f;
            return true;
        }
        public override void UpdateInventory(Item item, Player player)
        {
            if (item.type == ItemType<ClockGatlignum>())
            {
                player.GetModPlayer<CalRemixPlayer>().clockBar = true;
            }
            if (CalRemixWorld.permanenthealth)
            {
                if (item.type == ItemType<Elderberry>() && item.stack > 1)
                {
                    item.stack = 1;
                }
            }
            if (player.GetModPlayer<CalRemixPlayer>().amongusEnchant)
            {
                item.crit /= 3;
            }
            if (item.type == ItemType<FabsolsVodka>())
            {
                TransformItem(ref item, ItemType<NotFabsolVodka>());
            }
            if (CalRemixWorld.seafood)
            {
                if (item.type == ItemType<Seafood>())
                {
                    TransformItem(ref item, ItemType<SeafoodFood>());
                }
            }
            if (item.type == ItemID.EnchantedSword && !(DownedBossSystem.downedPerforator || DownedBossSystem.downedHiveMind))
            {
                TransformItem(ref item, ItemType<DisenchantedSword>());
            }
            if (CalRemixWorld.laruga)
            {
                if (Scoriad)
                {
                    if (!NPC.AnyNPCs(NPCType<LaRuga>()) && !player.HasBuff(BuffType<Scorinfestation>()))
                    {
                        int stacke = item.stack;
                        item.SetDefaults(NonScoria);
                        item.stack = stacke;
                        Scoriad = false;
                    }
                }
            }
            if (item.type == ItemID.CellPhone)
            {
                if (!player.GetModPlayer<CalRemixPlayer>().gottenCellPhone)
                {
                    player.GetModPlayer<CalRemixPlayer>().gottenCellPhone = true;
                }
            }
        }
        public static List<int> cosmicItems = new List<int>();
        public static void TransformItem(ref Item item, int transformType)
        {
            int stack = item.stack;
            item.SetDefaults(transformType);
            item.stack = stack;
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();
            CalamityPlayer calPlayer = player.GetModPlayer<CalamityPlayer>();
            if (modPlayer.roguebox && item.CountsAsClass<RogueDamageClass>())
            {
                int p = Projectile.NewProjectile(source, new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y - 400), new Vector2(0, 20), type, (int)(damage * 0.33f), knockback, player.whoAmI);
                {
                    Main.projectile[p].GetGlobalProjectile<CalRemixProjectile>().rogueclone = true;
                    if (p.WithinBounds(Main.maxProjectiles))
                        Main.projectile[p].originalDamage = (int)(damage * 0.33f);
                    if (modPlayer.tvo && calPlayer.StealthStrikeAvailable())
                    {
                        Main.projectile[p].GetGlobalProjectile<CalamityMod.Projectiles.CalamityGlobalProjectile>().stealthStrike = true;
                    }
                }
            }
            if (modPlayer.blaze && item.DamageType == DamageClass.Ranged)
            {
                if (modPlayer.blazeCount < 1)
                    modPlayer.blazeCount = 1;
                else
                {
                    Projectile.NewProjectile(source, position, velocity * 0.75f, ProjectileType<AstralFireball>(), 25, 0f, player.whoAmI);
                    modPlayer.blazeCount = 0;
                }
            }
            return true;
        }
        public override void Update(Item item, ref float gravity, ref float maxFallSpeed)
        {
            if (CalRemixWorld.permanenthealth)
            {
                if (item.type == ItemID.Apple)
                {
                    if (item.wet && !item.lavaWet && Main.bloodMoon && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
                    {
                        item.SetDefaults(ItemType<BloodOrange>());
                        item.stack++;
                    }
                }
                if (item.type == ItemType<Elderberry>() && item.stack > 1)
                {
                    item.stack = 1;
                }
            }
            if (item.type == ItemID.ShadowScale || item.type == ItemID.TissueSample)
            {
                if (!CalamityPlayer.areThereAnyDamnBosses)
                {

                    if (Main.tile[(int)item.Bottom.X / 16, (int)item.Bottom.Y / 16].TileType == TileType<GrimesandPlaced>() || Main.tile[(int)item.Bottom.X / 16, (int)item.Bottom.Y / 16 + 1].TileType == TileType<GrimesandPlaced>())
                    {
                        if (item.type == ItemID.ShadowScale)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Vector2 spawnAt = item.Center + new Vector2(0f, (float)item.height / 2f);
                                int n = NPC.NewNPC(item.GetSource_FromThis(), (int)spawnAt.X, (int)spawnAt.Y, NPCType<HiveMind>());
                                NPC blug = Main.npc[n];
                                for (int i = 0; i < 30; i++)
                                {
                                    Dust.NewDust(blug.position, blug.width, blug.height, DustID.Corruption);
                                }
                            }
                        }
                        else if (item.type == ItemID.TissueSample)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Vector2 spawnAt = item.Center + new Vector2(0f, (float)item.height / 2f);
                                int n = NPC.NewNPC(item.GetSource_FromThis(), (int)spawnAt.X, (int)spawnAt.Y, NPCType<PerforatorHive>());
                                NPC blug = Main.npc[n];
                                for (int i = 0; i < 30; i++)
                                {
                                    Dust.NewDust(blug.position, blug.width, blug.height, DustID.Blood);
                                }
                            }
                        }
                        item.active = false;
                    }
                }
            }
            if (item.type == ItemID.EnchantedSword && !(DownedBossSystem.downedPerforator || DownedBossSystem.downedHiveMind))
            {
                TransformItem(ref item, ItemType<DisenchantedSword>());
            }
            if (item.type == ItemType<CryoKey>() && item.lavaWet)
            {
                if (!NPC.AnyNPCs(NPCType<Pyrogen>()))
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 spawnAt = item.Center + new Vector2(0f, (float)item.height / 2f);
                        int n = NPC.NewNPC(item.GetSource_FromThis(), (int)spawnAt.X, (int)spawnAt.Y, NPCType<Pyrogen>());
                        NPC blug = Main.npc[n];
                        // todo: use blug to enrage him
                    }
                    item.active = false;
                }
            }
        }
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if (item.type == ItemID.FloatingIslandFishingCrate || item.type == ItemID.FloatingIslandFishingCrateHard)
            {
                itemLoot.AddIf(() => NPC.AnyNPCs(NPCID.WyvernHead) && CalamityMod.DownedBossSystem.downedYharon && !Main.LocalPlayer.Calamity().dFruit && CalRemixWorld.permanenthealth, ItemType<Dragonfruit>(), 1);
                itemLoot.AddIf(() => NPC.AnyNPCs(NPCID.WyvernHead) && CalamityMod.DownedBossSystem.downedYharon && Main.LocalPlayer.Calamity().dFruit && CalRemixWorld.permanenthealth, ItemType<Dragonfruit>(), 20);
            }
            if (item.type == ItemID.DungeonFishingCrate || item.type == ItemID.DungeonFishingCrateHard && Main.rand.NextBool(4))
            {
                itemLoot.Add(ItemType<BundleBones>(), 4, 10, 25);
            }
            if (item.type == ItemType<DesertScourgeBag>())
            {
                itemLoot.Add(ItemType<ParchedScale>(), 1, 30, 40);
                itemLoot.RemoveWhere((rule) => rule is CommonDrop e && e.itemId == ItemType<PearlShard>());
            }
            else if (item.type == ItemID.PlanteraBossBag)
            {
                itemLoot.Add(ItemType<EssenceofBabil>(), 1, 5, 9);
            }
            else if (item.type == ItemID.FishronBossBag)
            {
                itemLoot.Add(ItemType<DeliciousMeat>(), 2, 45, 92);
            }
            else if (item.type == ItemType<AstrumAureusBag>())
            {
                itemLoot.Add(ItemType<SoulofBright>(), 1, 10, 12);
            }
            else if (item.type == ItemType<ProvidenceBag>())
            {
                itemLoot.Add(ItemType<ProfanedNucleus>());
            }
            else if (item.type == ItemType<DevourerofGodsBag>())
            {
                itemLoot.Add(ItemType<Lean>(), 1, 6, 8);
                itemLoot.AddIf(() => CalamityWorld.revenge, ItemType<YharimBar>(), 1, 1, 3);
                itemLoot.RemoveWhere((rule) => rule is CommonDrop e && e.itemId == ItemType<CosmiliteBar>());
                itemLoot.AddIf(()=> !CalRemixWorld.cosmislag, ItemType<CosmiliteBar>(), 1, 55, 65);
            }
            else if (item.type == ItemType<YharonBag>())
            {
                LeadingConditionRule yhar = itemLoot.DefineConditionalDropSet(() => CalamityWorld.revenge);
                yhar.Add(ItemType<YharimBar>(), 1, 1, 3, hideLootReport: !CalamityWorld.revenge);
                yhar.AddFail(ItemType<YharimBar>(), 1, 6, 8, hideLootReport: CalamityWorld.revenge);
                itemLoot.Add(yhar);
                itemLoot.Add(ItemType<MovieSign>(), 100);
            }
            else if (item.type == ItemType<CrabulonBag>())
            {
                itemLoot.Add(ItemType<DeliciousMeat>(), 1, 4, 7);
                itemLoot.Add(ItemType<CrabLeaves>(), 1, 4, 7);
                itemLoot.Add(ItemType<OddMushroom>(), 3);
            }
            else if (item.type == ItemID.FishronBossBag)
            {
                itemLoot.Add(ItemType<DeliciousMeat>(), 2, 45, 92);
            }
            else if (item.type == ItemType<LeviathanBag>())
            {
                itemLoot.Add(ItemType<CrocodileScale>(), 1, 20, 30);
            }
            else if (item.type == ItemID.CorruptFishingCrate || item.type == ItemID.CorruptFishingCrateHard)
            {
                itemLoot.Add(ItemType<Grimesand>(), 1, 10, 30);
            }
            else if (item.type == ItemType<DraedonBag>())
            {
                itemLoot.Add(ItemType<DeliciousMeat>(), 1, 6000, 8000);
                itemLoot.AddIf(() => RemixDowned.downedHypnos, ItemType<AergianTechnistaff>());
                itemLoot.AddIf(() => RemixDowned.downedHypnos, ItemType<Neuraze>());
                itemLoot.AddIf(() => RemixDowned.downedHypnos, ItemType<HypnosMask>(), new Fraction(2, 7));
            }
            else if (item.type == ItemType<CalamitasCoffer>() || item.type == ItemType<DraedonBag>())
            {
                LeadingConditionRule yhar = itemLoot.DefineConditionalDropSet(() => CalamityWorld.revenge);
                yhar.Add(ItemType<YharimBar>(), 1, 9, 11, hideLootReport: !CalamityWorld.revenge);
                yhar.AddFail(ItemType<YharimBar>(), 1, 7, 9, hideLootReport: CalamityWorld.revenge);
                itemLoot.Add(yhar);
            }
            else if (item.type == ItemType<StarterBag>())
            {
                List<IItemDropRule> starterBagLoot = itemLoot.Get();
                for (int i = 0; i < starterBagLoot.Count; i++)
                {
                    if (starterBagLoot[i] is LeadingConditionRule lead)
                    {
                        for (int j = 0; j < lead.ChainedRules.Count; j++)
                        {
                            if (lead.ChainedRules[j] is Chains.TryIfSucceeded c)
                            {
                                if (c.RuleToChain is CommonDrop fuck) 
                                {
                                    if (fuck.itemId == ItemID.AmethystStaff || fuck.itemId == ItemID.TopazStaff)
                                    {
                                        lead.ChainedRules.RemoveAt(j);
                                    }
                                }
                            }
                            else if (lead.ChainedRules[j] is Chains.TryIfFailedRandomRoll c2)
                            {
                                if (c2.RuleToChain is CommonDrop fuck)
                                {
                                    if (fuck.itemId == ItemID.AmethystStaff || fuck.itemId == ItemID.TopazStaff)
                                    {
                                        lead.ChainedRules.RemoveAt(j);
                                    }
                                }

                            }
                        }
                    }
                }
                itemLoot.Add(ItemType<SaltBooklet>(), 1);
                itemLoot.AddIf(()=> Main.netMode != NetmodeID.MultiplayerClient, ItemType<Anomaly109>());
                itemLoot.AddIf(() => Main.netMode != NetmodeID.MultiplayerClient, ItemType<TheInsacredTexts>());
            }
            if (CalRemixAddon.CalVal != null)
            {
                if (item.type == CalRemixAddon.CalVal.Find<ModItem>("MysteryPainting").Type)
                {
                    itemLoot.Add(ItemType<MovieSign>(), 22);
                }
            }
        }



        public override void UpdateEquip(Item item, Player player)
        {
            if (CalRemixWorld.fearmonger)
            {
                if (item.type == ItemType<FearmongerGreathelm>())
                {
                    player.statManaMax2 -= 60;
                }
                if (item.type == ItemType<FearmongerPlateMail>())
                {
                    player.statLifeMax2 -= 100;
                }
                if (item.type == ItemType<FearmongerGreaves>())
                {
                    player.panic = false;
                }
            }
        }

        public override void UpdateArmorSet(Player player, string sns)
        {
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            CalamityPlayer calplayer = player.GetModPlayer<CalamityPlayer>();
            CalRemixPlayer modplayer = player.GetModPlayer<CalRemixPlayer>();
            if (item.type == ItemType<GrandGelatin>())
            {
                modplayer.miragel = true;
            }
            if (item.type == ItemType<TheAbsorber>())
            {
                if (!hideVisual)
                {
                    calplayer.regenator = true;
                }
            }
            if (item.type == ItemType<TheSponge>() || item.type == ItemType<TheGodfather>() || item.type == ItemType<TheVerbotenOne>())
            {
                GetModItem(ItemType<TheAbsorber>()).UpdateAccessory(player, hideVisual);
                if (item.type != ItemType<TheSponge>())
                    GetModItem(ItemType<TheSponge>()).UpdateAccessory(player, hideVisual);
                if (!hideVisual)
                    GetModItem(ItemType<Regenator>()).UpdateAccessory(player, hideVisual);
                GetModItem(ItemType<UrsaSergeant>()).UpdateAccessory(player, hideVisual);
                GetModItem(ItemType<TrinketofChi>()).UpdateAccessory(player, hideVisual);
                GetModItem(ItemType<AmidiasSpark>()).UpdateAccessory(player, hideVisual);
                GetModItem(ItemType<FlameLickedShell>()).UpdateAccessory(player, hideVisual);
                GetModItem(ItemType<PermafrostsConcoction>()).UpdateAccessory(player, hideVisual);
                GetModItem(ItemType<AquaticHeart>()).UpdateAccessory(player, hideVisual);
            }
            if (item.type == ItemType<AmbrosialAmpoule>() || item.type == ItemType<TheVerbotenOne>())
            {
                GetModItem(ItemType<ArchaicPowder>()).UpdateAccessory(player, hideVisual);
                GetModItem(ItemType<HoneyDew>()).UpdateAccessory(player, hideVisual);
            }
            if (item.type == ItemType<AbyssalDivingSuit>() || item.type == ItemType<TheGodfather>() || item.type == ItemType<TheVerbotenOne>())
            {
                GetModItem(ItemType<LumenousAmulet>()).UpdateAccessory(player, hideVisual);
                GetModItem(ItemType<AquaticEmblem>()).UpdateAccessory(player, hideVisual);
                GetModItem(ItemType<AlluringBait>()).UpdateAccessory(player, hideVisual);
                if (!hideVisual)
                GetModItem(ItemType<SpelunkersAmulet>()).UpdateAccessory(player, hideVisual);
                GetModItem(ItemType<OceanCrest>()).UpdateAccessory(player, hideVisual);
                GetModItem(ItemType<AquaticEmblem>()).UpdateAccessory(player, hideVisual);
            }
            if (item.type == ItemType<AbyssalDivingGear>())
            {
                GetModItem(ItemType<OceanCrest>()).UpdateAccessory(player, hideVisual);
            }
            if (item.type == ItemType<TheAmalgam>() || item.type == ItemType<Slimelgamation>() || item.type == ItemType<TheGodfather>() || item.type == ItemType<TheVerbotenOne>())
            {
                GetModItem(ItemType<GiantPearl>()).UpdateAccessory(player, hideVisual);
                if (!hideVisual)
                    GetModItem(ItemType<ManaPolarizer>()).UpdateAccessory(player, hideVisual);
                GetModItem(ItemType<FrostFlare>()).UpdateAccessory(player, hideVisual);
                GetModItem(ItemType<VoidofExtinction>()).UpdateAccessory(player, hideVisual);
                GetModItem(ItemType<VoidofCalamity>()).UpdateAccessory(player, hideVisual);
                GetModItem(ItemType<ToxicHeart>()).UpdateAccessory(player, hideVisual);
                GetModItem(ItemType<AlchemicalFlask>()).UpdateAccessory(player, hideVisual);
                GetModItem(ItemType<Radiance>()).UpdateAccessory(player, hideVisual);
                GetModItem(ItemType<TheEvolution>()).UpdateAccessory(player, hideVisual);
                GetModItem(ItemType<Affliction>()).UpdateAccessory(player, hideVisual);
                GetModItem(ItemType<CorrosiveSpine>()).UpdateAccessory(player, hideVisual);
                GetModItem(ItemType<LeviathanAmbergris>()).UpdateAccessory(player, hideVisual);
                GetModItem(ItemType<OldDukeScales>()).UpdateAccessory(player, hideVisual);
                player.sporeSac = true;
                GetModItem(ItemType<Abaddon>()).UpdateAccessory(player, hideVisual);
                player.magmaStone = true;
                GetModItem(ItemType<DynamoStemCells>()).UpdateAccessory(player, hideVisual);
                GetModItem(ItemType<BlazingCore>()).UpdateAccessory(player, hideVisual);
            }
            // Doing it here means any accessories equipped in higher slots won't be affected by the boost
            // This is intentional
            //
            // I hate this so much
            if (modplayer.origenSoul)
            {
                if (genSouls.Contains(item.type))
                {
                    player.statDefense += NPC.downedMoonlord ? 40 : Main.hardMode ? 8 : 4;
                }
            }
        }

        public override void OnConsumeItem(Item item, Player player)
        {
            if (player.GetModPlayer<CalRemixPlayer>().bananaClown && !player.HasCooldown(BananaClownCooldown.ID))
            {
                if (IsFruit(item))
                {
                    for (int i = 0; i < Main.rand.Next(2, 6); i++)
                        SoundEngine.PlaySound(CalamityMod.Projectiles.Magic.AcidicReed.SaxSound with { MaxInstances = 0 });
                    for (int num502 = 0; num502 < 36; num502++)
                    {
                        int dust = Dust.NewDust(new Vector2(player.position.X, player.position.Y + 16f), player.width, player.height - 16, DustID.Confetti_Yellow, 0f, 0f, 0, default, 1f);
                        Main.dust[dust].velocity *= 3f;
                        Main.dust[dust].scale *= 1.15f;
                    }
                    int num226 = 36;
                    for (int num227 = 0; num227 < num226; num227++)
                    {
                        Vector2 vector6 = Vector2.Normalize(player.velocity) * new Vector2((float)player.width / 2f, (float)player.height) * 0.75f;
                        vector6 = vector6.RotatedBy((double)((float)(num227 - (num226 / 2 - 1)) * MathHelper.TwoPi / (float)num226), default) + player.Center;
                        Vector2 vector7 = vector6 - player.Center;
                        int num228 = Dust.NewDust(vector6 + vector7, 0, 0, DustID.Confetti_Yellow, vector7.X * 1.5f, vector7.Y * 1.5f, 100, default, 1.4f);
                        Main.dust[num228].noGravity = true;
                        Main.dust[num228].noLight = true;
                        Main.dust[num228].velocity = vector7;
                    }
                    player.AddCooldown(BananaClownCooldown.ID, 4200);
                }
            }
            if (player.GetModPlayer<CalRemixPlayer>().phytogenSoul && !player.HasBuff(BuffID.PotionSickness))
            {
                if (IsFruit(item))
                {
                    player.Heal(player.statLifeMax2 / 3);
                    player.AddBuff(BuffID.PotionSickness, CalamityUtils.SecondsToFrames(45));
                }
            }
            if (item.type == ItemType<HadalStew>())
            {
                player.AddBuff(BuffID.Wrath, CalamityUtils.SecondsToFrames(60));
                player.AddBuff(BuffID.Rage, CalamityUtils.SecondsToFrames(60));
                player.AddBuff(BuffID.Endurance, CalamityUtils.SecondsToFrames(60));
                player.AddBuff(BuffID.Swiftness, CalamityUtils.SecondsToFrames(60));
                player.AddBuff(BuffID.Ironskin, CalamityUtils.SecondsToFrames(60));
                player.AddBuff(BuffID.Regeneration, CalamityUtils.SecondsToFrames(60));
                player.AddBuff(BuffID.Titan, CalamityUtils.SecondsToFrames(60));
            }
        }

        public bool IsFruit(Item item)
        {
            return item.type == ItemID.Apple || item.type == ItemID.Apricot || item.type == ItemID.Grapefruit || item.type == ItemID.Lemon || item.type == ItemID.Peach
                    || item.type == ItemID.Cherry || item.type == ItemID.Plum || item.type == ItemID.BlackCurrant || item.type == ItemID.Elderberry
                    || item.type == ItemID.BloodOrange || item.type == ItemID.Rambutan || item.type == ItemID.Mango || item.type == ItemID.Pineapple
                    || item.type == ItemID.Banana || item.type == ItemID.Coconut || item.type == ItemID.Dragonfruit || item.type == ItemID.Starfruit
                    || item.type == ItemID.Pomegranate || item.type == ItemID.SpicyPepper;
        }

        public override void PostUpdate(Item item)
        {
            int value = NPCType<Lizard>();
            foreach (NPC npc in Main.npc)
            {
                if (item.Hitbox.Intersects(npc.Hitbox) && npc.type == NPCType<Lizard>() && GemCrawl.TryGetValue(item.type, out value))
                {
                    NPC.NewNPCDirect(NPC.GetSource_None(), npc.Center, value);
                    npc.life = 0;
                    item.active = false;
                }
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (devItem != string.Empty)
            {
                string text = CalamityUtils.ColorMessage("- Lightmix Dedicated Item ", Color.Crimson);
                text += CalamityUtils.ColorMessage((devItem.Equals("Remix")) ? " " : $": {devItem} ", Color.Gold);
                text += CalamityUtils.ColorMessage("-", Color.Crimson);
                TooltipLine tip = new(Mod, "CalRemix:Dev", text);
                tooltips.Add(tip);
            }
            if (CalRemixPlayer.dyeStats.ContainsKey(item.type) && CalRemixWorld.dyeStats)
            {
                DyeStats stats = CalRemixPlayer.dyeStats[item.type];
                string ret = "";
                if (stats.red != 0)
                    ret += $"[c/ff0000:Damage " + WhichIncrement(stats.red) + " by " + Math.Abs(stats.red) + "%]\n";
                if (stats.orange != 0)
                    ret += $"[c/ffa200:Weapon speed " + WhichIncrement(stats.orange) + " by " + Math.Abs(stats.orange) + "%]\n";
                if (stats.yellow != 0)
                    ret += $"[c/ffff00:Movement speed " + WhichIncrement(stats.yellow) + " by " + Math.Abs(stats.yellow) + "%]\n";
                if (stats.lime != 0)
                    ret += $"[c/a2ff00:Luck " + WhichIncrement(stats.lime) + " by " + Math.Abs(stats.lime) + "]\n";
                if (stats.green != 0)
                    ret += $"[c/00ff00:Jump speed " + WhichIncrement(stats.green) + " by " + Math.Abs(stats.green) + "%]\n";
                if (stats.cyan != 0)
                    ret += $"[c/00ffff:Critical strike chance " + WhichIncrement(stats.cyan) + " by " + Math.Abs(stats.cyan) + "%]\n";
                if (stats.teal != 0)
                    ret += $"[c/008080:Damage reduction " + WhichIncrement(stats.teal) + " by " + Math.Abs(stats.teal) + "%]\n";
                if (stats.skyblue != 0)
                    ret += $"[c/66a3ff:Flight time " + WhichIncrement(stats.skyblue) + " by " + Math.Abs(stats.skyblue) * 10 + "]\n";
                if (stats.blue != 0)
                    ret += $"[c/0000ff:Defense " + WhichIncrement(stats.blue) + " by " + Math.Abs(stats.blue) + "]\n";
                if (stats.purple != 0)
                    ret += $"[c/9400cf:Weapon knockback " + WhichIncrement(stats.purple) + " by " + Math.Abs(stats.purple) + "%]\n";
                if (stats.violet != 0)
                    ret += $"[c/ff00b7:Enemy aggro " + WhichIncrement(stats.violet) + " by " + Math.Abs(stats.violet) + "]\n";
                if (stats.pink != 0)
                    ret += $"[c/ff45a2:Life regeneration " + WhichIncrement(stats.pink) + " by " + Math.Abs(stats.pink) + "]\n";
                if (stats.brown != 0)
                    ret += $"[c/7a4b00:Building range " + WhichIncrement(stats.brown) + " by " + Math.Abs(stats.brown) + "]\n";
                if (stats.silver != 0)
                    ret += $"[c/ffffff:Charisma " + WhichIncrement(stats.silver) + " by " + Math.Abs(stats.silver) + "]\n";
                if (stats.black != 0)
                    ret += $"[c/000000:Evil " + WhichIncrement(stats.black) + " by " + Math.Abs(stats.black) + "]\n";
                tooltips.Add(new TooltipLine(Mod, "DyeStats", ret));
            }
        }

        public string WhichIncrement(int stat)
        {
            if (stat > 0)
                return "increased";
            else if (stat < 0)
                return "decreased";
            else
                return "not changed";
        }
    }
}
