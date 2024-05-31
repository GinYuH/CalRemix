using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using CalamityMod.CalPlayer;
using CalRemix.Items.Accessories;
using CalamityMod.Items.Accessories;
using CalamityMod.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Projectiles.Typeless;
using CalamityMod.Projectiles.Summon;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Projectiles.Rogue;
using CalamityMod.Items.PermanentBoosters;
using CalamityMod;
using CalRemix.Items.Weapons;
using CalamityMod.Items.Materials;
using System.Collections.Generic;
using CalamityMod.Items.TreasureBags;
using CalRemix.Items.Materials;
using CalamityMod.World;
using CalamityMod.Items.Potions;
using CalamityMod.Items.SummonItems;
using CalRemix.Items.Potions;
using CalamityMod.Items.Weapons.Melee;
using Terraria.GameContent.ItemDropRules;
using CalRemix.NPCs.Minibosses;
using CalRemix.Buffs;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using CalRemix.Items;
using CalRemix.Projectiles.Accessories;
using Terraria.Audio;
using CalamityMod.Items.Armor.Fearmonger;
using CalRemix.Tiles;
using CalamityMod.NPCs.Perforator;
using CalamityMod.NPCs.HiveMind;
using CalRemix.Items.Placeables;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.TreasureBags.MiscGrabBags;
using CalamityMod.Items.LoreItems;
using CalRemix.NPCs.Bosses.BossScule;
using CalamityMod.Rarities;
using CalRemix.NPCs;
using CalamityMod.NPCs.NormalNPCs;
using System.Linq;
using CalamityMod.UI.CalamitasEnchants;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.NPCs.Other;
using CalamityMod.Items;
using CalamityMod.Items.Placeables;
using CalamityMod.Items.Fishing.AstralCatches;

namespace CalRemix
{
    public class CalRemixItem : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public bool Scoriad = false;
        public int NonScoria = -1;
        private static readonly Dictionary<int, int> GemCrawl = new()
        {
            { ItemID.Ruby, ModContent.NPCType<CrawlerRuby>() },
            { ItemID.Amber, ModContent.NPCType<CrawlerAmber>() },
            { ItemID.Topaz, ModContent.NPCType<CrawlerTopaz>() },
            { ItemID.Emerald, ModContent.NPCType<CrawlerEmerald>() },
            { ItemID.Sapphire, ModContent.NPCType<CrawlerSapphire>() },
            { ItemID.Amethyst, ModContent.NPCType<CrawlerAmethyst>() },
            { ItemID.Diamond, ModContent.NPCType<CrawlerDiamond>() },
            { ItemID.CrystalShard, ModContent.NPCType<CrawlerCrystal>() }
        };
        public override void SetDefaults(Item item)
        {
            if (item.type == ModContent.ItemType<EssenceofHavoc>())
            {
                ItemID.Sets.ShimmerTransformToItem[item.type] = ModContent.ItemType<EssenceofLaw>();
            }
            else if (item.type == ModContent.ItemType<EssenceofEleum>())
            {
                ItemID.Sets.ShimmerTransformToItem[item.type] = ModContent.ItemType<EssenceofCrystal>();
            }
            else if (item.type == ModContent.ItemType<EssenceofSunlight>())
            {
                ItemID.Sets.ShimmerTransformToItem[item.type] = ModContent.ItemType<EssenceofMyst>();
            }
            else if (item.type == ModContent.ItemType<EssenceofBabil>())
            {
                ItemID.Sets.ShimmerTransformToItem[item.type] = ModContent.ItemType<EssenceofZot>();
            }
            else if (item.type == ModContent.ItemType<TitanArm>())
            {
                ItemID.Sets.ShimmerTransformToItem[item.type] = ModContent.ItemType<TitanFinger>();
            }
            if (CalRemixWorld.fearmonger)
            {
                if (item.type == ModContent.ItemType<FearmongerGreathelm>())
                {
                    item.defense = 2;
                    item.value = Item.sellPrice(silver: 15);
                    item.rare = ItemRarityID.Blue;
                }
                else if (item.type == ModContent.ItemType<FearmongerPlateMail>())
                {
                    item.defense = 8;
                    item.value = Item.sellPrice(silver: 12);
                    item.rare = ItemRarityID.Blue;
                }
                else if (item.type == ModContent.ItemType<FearmongerGreaves>())
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
            // undo the curve flattening
            // done with content samples so that reforges are ignored
            if (ContentSamples.ItemsByType.ContainsKey(item.type))
            {
                if (ContentSamples.ItemsByType[item.type].rare == ModContent.RarityType<Turquoise>())
                {
                    item.damage = (int)(item.damage * 1.33f);
                }
                if (ContentSamples.ItemsByType[item.type].rare == ModContent.RarityType<PureGreen>())
                {
                    item.damage = (int)(item.damage * 1.67f);
                }
                if (ContentSamples.ItemsByType[item.type].rare == ModContent.RarityType<DarkBlue>())
                {
                    item.damage = (int)(item.damage * 2.5f);
                }
                if (ContentSamples.ItemsByType[item.type].rare == ModContent.RarityType<Violet>())
                {
                    item.damage = (int)(item.damage * 3.33f);
                }
                if (ContentSamples.ItemsByType[item.type].rare == ModContent.RarityType<HotPink>())
                {
                    item.damage = (int)(item.damage * 5f);
                }
            }
            if (item.type == ModContent.ItemType<Navystone>())
            {
                item.createTile = ModContent.TileType<NavystoneSafe>();
            }
            if (item.type == ModContent.ItemType<EutrophicSand>())
            {
                item.createTile = ModContent.TileType<EutrophicSandSafe>();
            }
            if (item.type == ModContent.ItemType<HardenedEutrophicSand>())
            {
                item.createTile = ModContent.TileType<HardenedEutrophicSandSafe>();
            }
            if (item.type == ModContent.ItemType<SeaPrism>())
            {
                item.createTile = ModContent.TileType<SeaPrismSafe>();
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
                    player.AddBuff(ModContent.BuffType<WeakBrimstoneFlames>(), 10);
                }
                if (item.CountsAsClass<MagicDamageClass>() && item.mana > 0 && item.type != ModContent.ItemType<Eternity>())
                    player.Calamity().lifeManaEnchant = true;
                if (item.shoot > ProjectileID.None && !item.IsTrueMelee() && item.type != ModContent.ItemType<TheFinalDawn>())
                    player.Calamity().farProximityRewardEnchant = true;
                if (item.shoot > ProjectileID.None && !item.IsTrueMelee() && item.type != ModContent.ItemType<TheFinalDawn>())
                    player.Calamity().closeProximityRewardEnchant = true;
                if (!item.CountsAsClass<SummonDamageClass>() && !item.CountsAsClass<RogueDamageClass>() && !item.channel && item.type != ModContent.ItemType<HeavenlyGale>())
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
                        int num = ModContent.NPCType<LecherousOrb>();
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
            if (item.type == ModContent.ItemType<CalamityMod.Items.Potions.Alcohol.FabsolsVodka>())
            {
                TransformItem(ref item, ModContent.ItemType<Items.Potions.NotFabsolVodka>());
            }
            if (CalRemixWorld.seafood)
            {
                if (item.type == ModContent.ItemType<Seafood>())
                {
                    TransformItem(ref item, ModContent.ItemType<SeafoodFood>());
                }
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
                    Color col = item.type == ModContent.ItemType<HornetRound>() ? Color.Yellow : Color.Red;
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
            if (item.type == ModContent.ItemType<ClockGatlignum>())
            {
                player.GetModPlayer<CalRemixPlayer>().clockBar = true;
            }
            if (CalRemixWorld.permanenthealth)
            {
                if (item.type == ModContent.ItemType<Elderberry>() && item.stack > 1)
                {
                    item.stack = 1;
                }
            }
            if (player.GetModPlayer<CalRemixPlayer>().amongusEnchant)
            {
                item.crit /= 3;
            }
            if (item.type == ModContent.ItemType<CalamityMod.Items.Potions.Alcohol.FabsolsVodka>())
            {
                TransformItem(ref item, ModContent.ItemType<Items.Potions.NotFabsolVodka>());
            }
            if (CalRemixWorld.seafood)
            {
                if (item.type == ModContent.ItemType<Seafood>())
                {
                    TransformItem(ref item, ModContent.ItemType<SeafoodFood>());
                }
            }
            if (CalRemixWorld.laruga)
            {
                if (Scoriad)
                {
                    if (!NPC.AnyNPCs(ModContent.NPCType<LaRuga>()) && !player.HasBuff(ModContent.BuffType<Scorinfestation>()))
                    {
                        int stacke = item.stack;
                        item.SetDefaults(NonScoria);
                        item.stack = stacke;
                        Scoriad = false;
                    }
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
                    Projectile.NewProjectile(source, position, velocity * 0.75f, ModContent.ProjectileType<AstralFireball>(), 25, 0f, player.whoAmI);
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
                        item.SetDefaults(ModContent.ItemType<BloodOrange>());
                        item.stack++;
                    }
                }
                if (item.type == ModContent.ItemType<Elderberry>() && item.stack > 1)
                {
                    item.stack = 1;
                }
            }
            if (item.type == ItemID.ShadowScale || item.type == ItemID.TissueSample)
            {
                if (!CalamityPlayer.areThereAnyDamnBosses)
                {

                    if (Main.tile[(int)item.Bottom.X / 16, (int)item.Bottom.Y / 16].TileType == ModContent.TileType<GrimesandPlaced>() || Main.tile[(int)item.Bottom.X / 16, (int)item.Bottom.Y / 16 + 1].TileType == ModContent.TileType<GrimesandPlaced>())
                    {
                        if (item.type == ItemID.ShadowScale)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Vector2 spawnAt = item.Center + new Vector2(0f, (float)item.height / 2f);
                                int n = NPC.NewNPC(item.GetSource_FromThis(), (int)spawnAt.X, (int)spawnAt.Y, ModContent.NPCType<HiveMind>());
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
                                int n = NPC.NewNPC(item.GetSource_FromThis(), (int)spawnAt.X, (int)spawnAt.Y, ModContent.NPCType<PerforatorHive>());
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
        }
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if (item.type == ItemID.FloatingIslandFishingCrate || item.type == ItemID.FloatingIslandFishingCrateHard)
            {
                itemLoot.AddIf(() => NPC.AnyNPCs(NPCID.WyvernHead) && CalamityMod.DownedBossSystem.downedYharon && !Main.LocalPlayer.Calamity().dFruit && CalRemixWorld.permanenthealth, ModContent.ItemType<Dragonfruit>(), 1);
                itemLoot.AddIf(() => NPC.AnyNPCs(NPCID.WyvernHead) && CalamityMod.DownedBossSystem.downedYharon && Main.LocalPlayer.Calamity().dFruit && CalRemixWorld.permanenthealth, ModContent.ItemType<Dragonfruit>(), 20);
            }
            if (item.type == ItemID.DungeonFishingCrate || item.type == ItemID.DungeonFishingCrateHard && Main.rand.NextBool(4))
            {
                itemLoot.Add(ModContent.ItemType<BundleBones>(), 4, 10, 25);
            }
            if (item.type == ModContent.ItemType<DesertScourgeBag>())
            {
                itemLoot.Add(ModContent.ItemType<ParchedScale>(), 1, 30, 40);
                itemLoot.RemoveWhere((rule) => rule is CommonDrop e && e.itemId == ModContent.ItemType<PearlShard>());
            }
            else if (item.type == ItemID.PlanteraBossBag)
            {
                itemLoot.Add(ModContent.ItemType<EssenceofBabil>(), 1, 5, 9);
            }
            else if (item.type == ModContent.ItemType<ProvidenceBag>())
            {
                itemLoot.Add(ModContent.ItemType<ProfanedNucleus>());
            }
            else if (item.type == ModContent.ItemType<DevourerofGodsBag>())
            {
                itemLoot.Add(ModContent.ItemType<Lean>(), 1, 6, 8);
                itemLoot.AddIf(() => CalamityWorld.revenge, ModContent.ItemType<YharimBar>(), 1, 1, 3);
                itemLoot.RemoveWhere((rule) => rule is CommonDrop e && e.itemId == ModContent.ItemType<CosmiliteBar>());
                itemLoot.AddIf(()=> !CalRemixWorld.cosmislag, ModContent.ItemType<CosmiliteBar>(), 1, 55, 65);

            }
            else if (item.type == ModContent.ItemType<YharonBag>())
            {
                LeadingConditionRule yhar = itemLoot.DefineConditionalDropSet(() => CalamityWorld.revenge);
                yhar.Add(ModContent.ItemType<YharimBar>(), 1, 1, 3, hideLootReport: !CalamityWorld.revenge);
                yhar.AddFail(ModContent.ItemType<YharimBar>(), 1, 6, 8, hideLootReport: CalamityWorld.revenge);
            }
            else if (item.type == ModContent.ItemType<CrabulonBag>())
            {
                itemLoot.Add(ModContent.ItemType<DeliciousMeat>(), 1, 4, 7);
                itemLoot.Add(ModContent.ItemType<CrabLeaves>(), 1, 4, 7);
            }
            else if (item.type == ItemID.FishronBossBag)
            {
                itemLoot.Add(ModContent.ItemType<DeliciousMeat>(), 2, 45, 92);
            }
            else if (item.type == ModContent.ItemType<LeviathanBag>())
            {
                itemLoot.Add(ModContent.ItemType<CrocodileScale>(), 1, 20, 30);
            }
            else if (item.type == ItemID.CorruptFishingCrate || item.type == ItemID.CorruptFishingCrateHard)
            {
                itemLoot.Add(ModContent.ItemType<Grimesand>(), 1, 10, 30);
            }
            else if (item.type == ModContent.ItemType<DraedonBag>())
            {
                itemLoot.Add(ModContent.ItemType<DeliciousMeat>(), 1, 6000, 8000);
            }
            else if (item.type == ModContent.ItemType<CalamitasCoffer>() || item.type == ModContent.ItemType<DraedonBag>())
            {
                LeadingConditionRule yhar = itemLoot.DefineConditionalDropSet(() => CalamityWorld.revenge);
                yhar.Add(ModContent.ItemType<YharimBar>(), 1, 9, 11, hideLootReport: !CalamityWorld.revenge);
                yhar.AddFail(ModContent.ItemType<YharimBar>(), 1, 7, 9, hideLootReport: CalamityWorld.revenge);
            }
            else if (item.type == ModContent.ItemType<StarterBag>())
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
                itemLoot.Add(ModContent.ItemType<SaltBooklet>(), 1);
                itemLoot.AddIf(()=> Main.netMode != NetmodeID.MultiplayerClient, ModContent.ItemType<Anomaly109>());
                itemLoot.AddIf(() => Main.netMode != NetmodeID.MultiplayerClient, ModContent.ItemType<TheInsacredTexts>());
            }
            if (ModLoader.HasMod("CalValEX"))
            {
                if (item.type == ModLoader.GetMod("CalValEX").Find<ModItem>("MysteryPainting").Type)
                {
                    itemLoot.Add(ModContent.ItemType<MovieSign>(), 22);
                }
            }
        }



        public override void UpdateEquip(Item item, Player player)
        {
            if (CalRemixWorld.fearmonger)
            {
                if (item.type == ModContent.ItemType<FearmongerGreathelm>())
                {
                    player.statManaMax2 -= 60;
                }
                if (item.type == ModContent.ItemType<FearmongerPlateMail>())
                {
                    player.statLifeMax2 -= 100;
                }
                if (item.type == ModContent.ItemType<FearmongerGreaves>())
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
            if (item.type == ModContent.ItemType<GrandGelatin>())
            {
                modplayer.miragel = true;
            }
            if (item.type == ModContent.ItemType<TheAbsorber>())
            {
                if (!hideVisual)
                {
                    calplayer.regenator = true;
                }
            }
            if (item.type == ModContent.ItemType<TheSponge>() || item.type == ModContent.ItemType<TheGodfather>() || item.type == ModContent.ItemType<TheVerbotenOne>())
            {
                ModContent.GetModItem(ModContent.ItemType<TheAbsorber>()).UpdateAccessory(player, hideVisual);
                if (item.type != ModContent.ItemType<TheSponge>())
                    ModContent.GetModItem(ModContent.ItemType<TheSponge>()).UpdateAccessory(player, hideVisual);
                if (!hideVisual)
                    ModContent.GetModItem(ModContent.ItemType<Regenator>()).UpdateAccessory(player, hideVisual);
                ModContent.GetModItem(ModContent.ItemType<UrsaSergeant>()).UpdateAccessory(player, hideVisual);
                ModContent.GetModItem(ModContent.ItemType<TrinketofChi>()).UpdateAccessory(player, hideVisual);
                ModContent.GetModItem(ModContent.ItemType<AmidiasSpark>()).UpdateAccessory(player, hideVisual);
                ModContent.GetModItem(ModContent.ItemType<FlameLickedShell>()).UpdateAccessory(player, hideVisual);
                ModContent.GetModItem(ModContent.ItemType<PermafrostsConcoction>()).UpdateAccessory(player, hideVisual);
                ModContent.GetModItem(ModContent.ItemType<AquaticHeart>()).UpdateAccessory(player, hideVisual);
                ModContent.GetModItem(ModContent.ItemType<OldDukeScales>()).UpdateAccessory(player, hideVisual);
            }
            if (item.type == ModContent.ItemType<AmbrosialAmpoule>() || item.type == ModContent.ItemType<TheVerbotenOne>())
            {
                ModContent.GetModItem(ModContent.ItemType<ArchaicPowder>()).UpdateAccessory(player, hideVisual);
                ModContent.GetModItem(ModContent.ItemType<HoneyDew>()).UpdateAccessory(player, hideVisual);
            }
            if (item.type == ModContent.ItemType<AbyssalDivingSuit>() || item.type == ModContent.ItemType<TheGodfather>() || item.type == ModContent.ItemType<TheVerbotenOne>())
            {
                ModContent.GetModItem(ModContent.ItemType<LumenousAmulet>()).UpdateAccessory(player, hideVisual);
                ModContent.GetModItem(ModContent.ItemType<AquaticEmblem>()).UpdateAccessory(player, hideVisual);
                ModContent.GetModItem(ModContent.ItemType<AlluringBait>()).UpdateAccessory(player, hideVisual);
                if (!hideVisual)
                ModContent.GetModItem(ModContent.ItemType<SpelunkersAmulet>()).UpdateAccessory(player, hideVisual);
                ModContent.GetModItem(ModContent.ItemType<OceanCrest>()).UpdateAccessory(player, hideVisual);
                ModContent.GetModItem(ModContent.ItemType<AquaticEmblem>()).UpdateAccessory(player, hideVisual);
            }
            if (item.type == ModContent.ItemType<AbyssalDivingGear>())
            {
                ModContent.GetModItem(ModContent.ItemType<OceanCrest>()).UpdateAccessory(player, hideVisual);
            }
            if (item.type == ModContent.ItemType<TheAmalgam>() || item.type == ModContent.ItemType<Slimelgamation>() || item.type == ModContent.ItemType<TheGodfather>() || item.type == ModContent.ItemType<TheVerbotenOne>())
            {
                ModContent.GetModItem(ModContent.ItemType<GiantPearl>()).UpdateAccessory(player, hideVisual);
                if (!hideVisual)
                    ModContent.GetModItem(ModContent.ItemType<ManaPolarizer>()).UpdateAccessory(player, hideVisual);
                ModContent.GetModItem(ModContent.ItemType<FrostFlare>()).UpdateAccessory(player, hideVisual);
                ModContent.GetModItem(ModContent.ItemType<VoidofExtinction>()).UpdateAccessory(player, hideVisual);
                ModContent.GetModItem(ModContent.ItemType<VoidofCalamity>()).UpdateAccessory(player, hideVisual);
                ModContent.GetModItem(ModContent.ItemType<ToxicHeart>()).UpdateAccessory(player, hideVisual);
                ModContent.GetModItem(ModContent.ItemType<AlchemicalFlask>()).UpdateAccessory(player, hideVisual);
                ModContent.GetModItem(ModContent.ItemType<Radiance>()).UpdateAccessory(player, hideVisual);
                ModContent.GetModItem(ModContent.ItemType<TheEvolution>()).UpdateAccessory(player, hideVisual);
                ModContent.GetModItem(ModContent.ItemType<Affliction>()).UpdateAccessory(player, hideVisual);
                ModContent.GetModItem(ModContent.ItemType<CorrosiveSpine>()).UpdateAccessory(player, hideVisual);
                ModContent.GetModItem(ModContent.ItemType<LeviathanAmbergris>()).UpdateAccessory(player, hideVisual);
                player.sporeSac = true;
                ModContent.GetModItem(ModContent.ItemType<Abaddon>()).UpdateAccessory(player, hideVisual);
                player.magmaStone = true;
                ModContent.GetModItem(ModContent.ItemType<DynamoStemCells>()).UpdateAccessory(player, hideVisual);
                ModContent.GetModItem(ModContent.ItemType<BlazingCore>()).UpdateAccessory(player, hideVisual);
            }
        }

        public override void OnConsumeItem(Item item, Player player)
        {
            if (player.GetModPlayer<CalRemixPlayer>().bananaClown && !player.HasCooldown(BananaClownCooldown.ID))
            {
                if (item.type == ItemID.Apple || item.type == ItemID.Apricot || item.type == ItemID.Grapefruit || item.type == ItemID.Lemon || item.type == ItemID.Peach
                    || item.type == ItemID.Cherry || item.type == ItemID.Plum || item.type == ItemID.BlackCurrant || item.type == ItemID.Elderberry
                    || item.type == ItemID.BloodOrange || item.type == ItemID.Rambutan || item.type == ItemID.Mango || item.type == ItemID.Pineapple
                    || item.type == ItemID.Banana || item.type == ItemID.Coconut || item.type == ItemID.Dragonfruit || item.type == ItemID.Starfruit
                    || item.type == ItemID.Pomegranate || item.type == ItemID.SpicyPepper)
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
        }
        public override void PostUpdate(Item item)
        {
            int value = ModContent.NPCType<Lizard>();
            foreach (NPC npc in Main.npc)
            {
                if (item.Hitbox.Intersects(npc.Hitbox) && npc.type == ModContent.NPCType<Lizard>() && GemCrawl.TryGetValue(item.type, out value))
                {
                    NPC.NewNPCDirect(NPC.GetSource_None(), npc.Center, value);
                    npc.life = 0;
                    item.active = false;
                }
            }
        }
    }
}
