﻿using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.CalPlayer;
using CalamityMod.Items;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Accessories.Wings;
using CalamityMod.Items.Ammo;
using CalamityMod.Items.Armor.Fearmonger;
using CalamityMod.Items.Fishing.AstralCatches;
using CalamityMod.Items.Materials;
using CalamityMod.Items.PermanentBoosters;
using CalamityMod.Items.Placeables;
using CalamityMod.Items.Placeables.Furniture;
using CalamityMod.Items.Placeables.FurnitureAbyss;
using CalamityMod.Items.Potions;
using CalamityMod.Items.Potions.Alcohol;
using CalamityMod.Items.SummonItems;
using CalamityMod.Items.TreasureBags;
using CalamityMod.Items.TreasureBags.MiscGrabBags;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.NPCs.HiveMind;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.NPCs.Other;
using CalamityMod.NPCs.Perforator;
using CalamityMod.Rarities;
using CalamityMod.World;
using CalRemix.Content.Buffs;
using CalRemix.Content.Cooldowns;
using CalRemix.Content.DamageClasses;
using CalRemix.Content.Items.Accessories;
using CalRemix.Content.Items.Ammo;
using CalRemix.Content.Items.Armor;
using CalRemix.Content.Items.Lore;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Items.Placeables;
using CalRemix.Content.Items.Potions;
using CalRemix.Content.Items.Weapons;
using CalRemix.Content.Items.Weapons.Stormbow;
using CalRemix.Content.NPCs;
using CalRemix.Content.NPCs.Bosses.Pyrogen;
using CalRemix.Content.NPCs.Minibosses;
using CalRemix.Content.Projectiles;
using CalRemix.Content.Projectiles.Accessories;
using CalRemix.Content.Projectiles.Weapons;
using CalRemix.Content.Tiles;
using CalRemix.Core.World;
using CalRemix.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Tile_Entities;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace CalRemix
{
    public class CalRemixItem : GlobalItem
    {
        public override bool InstancePerEntity => true;
        internal string devItem = string.Empty;
        public bool Scoriad = false;
        public int NonScoria = -1;
        internal static List<int> Torch = new()
        {
            ItemID.RainbowTorch,
            ItemID.UltrabrightTorch,
            ItemID.IchorTorch,
            ItemID.BoneTorch,
            ItemID.CursedTorch,
            ItemID.DemonTorch,
            ItemID.IceTorch,
            ItemID.JungleTorch,
            ItemID.CrimsonTorch,
            ItemID.CorruptTorch,
            ItemID.HallowedTorch,
            ItemID.Torch,
            ItemType<AstralTorch>(),
            ItemType<SulphurousTorch>(),
            ItemType<GloomTorch>(),
            ItemType<AbyssTorch>(),
            ItemType<AlgalPrismTorch>(),
            ItemType<NavyPrismTorch>(),
            ItemType<RefractivePrismTorch>()
        };
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
            ItemType<SoulofPyrogen>(),
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
            else if (item.type == ItemType<EssenceofRend>())
            {
                ItemID.Sets.ShimmerTransformToItem[item.type] = ItemType<EssenceofSurt>();
            }
            else if (item.type == ItemType<EssenceofLaw>())
            {
                ItemID.Sets.ShimmerTransformToItem[item.type] = ItemType<EssenceofHavoc>();
            }
            else if (item.type == ItemType<EssenceofCrystal>())
            {
                ItemID.Sets.ShimmerTransformToItem[item.type] = ItemType<EssenceofEleum>();
            }
            else if (item.type == ItemType<EssenceofMyst>())
            {
                ItemID.Sets.ShimmerTransformToItem[item.type] = ItemType<EssenceofSunlight>();
            }
            else if (item.type == ItemType<EssenceofZot>())
            {
                ItemID.Sets.ShimmerTransformToItem[item.type] = ItemType<EssenceofBabil>();
            }
            else if (item.type == ItemType<EssenceofSurt>())
            {
                ItemID.Sets.ShimmerTransformToItem[item.type] = ItemType<EssenceofRend>();
            }
            else if (item.type == ItemType<TitanArm>())
            {
                ItemID.Sets.ShimmerTransformToItem[item.type] = ItemType<TitanFinger>();
            }
            else if (item.type == ItemType<FlashRound>())
            {
                ItemID.Sets.ShimmerTransformToItem[item.type] = ItemType<AncientFlashBullet>();
            }
            else if (item.type == ItemType<CosmiliteBar>())
            {
                item.rare = CalRemixWorld.cosmislag ? ItemRarityID.Purple : RarityType<DarkBlue>();
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
            if (CalRemixWorld.weaponReworks)
            {
                if (item.type == ItemType<ScourgeoftheDesert>())
                {
                    item.shoot = ProjectileType<ScourgeDesert>();
                    item.useStyle = ItemUseStyleID.Rapier; // Makes the player do the proper arm motion
                }
                if (item.type == ItemType<CalamityMod.Items.Weapons.Rogue.Turbulance>())
                {
                    item.shoot = ProjectileType<Content.Projectiles.Weapons.Turbulance>();
                    item.useStyle = ItemUseStyleID.Rapier; // Makes the player do the proper arm motion
                }
                if (item.type == ItemType<CalamityMod.Items.Weapons.Rogue.IchorSpear>())
                {
                    item.shoot = ProjectileType<Content.Projectiles.Weapons.IchorSpear>();
                    item.useStyle = ItemUseStyleID.Rapier; // Makes the player do the proper arm motion
                }
                if (item.type == ItemType<CalamityMod.Items.Weapons.Rogue.PalladiumJavelin>())
                {
                    item.shoot = ProjectileType<Content.Projectiles.Weapons.PalJav>();
                    item.useStyle = ItemUseStyleID.Rapier; // Makes the player do the proper arm motion
                }
                if (item.type == ItemType<CalamityMod.Items.Weapons.Rogue.CrystalPiercer>())
                {
                    item.shoot = ProjectileType<Content.Projectiles.Weapons.CrystalPiercer>();
                    item.useStyle = ItemUseStyleID.Rapier; // Makes the player do the proper arm motion
                }
                if (item.type == ItemType<CalamityMod.Items.Weapons.DraedonsArsenal.FrequencyManipulator>())
                {
                    item.shoot = ProjectileType<Content.Projectiles.Weapons.FreqManip>();
                    item.useStyle = ItemUseStyleID.Rapier; // Makes the player do the proper arm motion
                }
                if (item.type == ItemType<CalamityMod.Items.Weapons.Rogue.ScourgeoftheSeas>())
                {
                    item.shoot = ProjectileType<Content.Projectiles.Weapons.ScourgeSea>();
                    item.useStyle = ItemUseStyleID.Rapier; // Makes the player do the proper arm motion
                }
                if (item.type == ItemType<CalamityMod.Items.Weapons.Rogue.SpearofPaleolith>())
                {
                    item.shoot = ProjectileType<Content.Projectiles.Weapons.Paleolith>();
                    item.useStyle = ItemUseStyleID.Rapier; // Makes the player do the proper arm motion
                }
                if (item.type == ItemType<CalamityMod.Items.Weapons.Rogue.WaveSkipper>())
                {
                    item.shoot = ProjectileType<Content.Projectiles.Weapons.WaveSkipper>();
                    item.useStyle = ItemUseStyleID.Rapier; // Makes the player do the proper arm motion
                }
                if (item.type == ItemType<CalamityMod.Items.Weapons.Rogue.SpearofDestiny>())
                {
                    item.shoot = ProjectileType<Content.Projectiles.Weapons.SpearDestiny>();
                    item.useStyle = ItemUseStyleID.Rapier; // Makes the player do the proper arm motion
                }
                if (item.type == ItemType<CalamityMod.Items.Weapons.Rogue.PhantasmalRuin>())
                {
                    item.shoot = ProjectileType<Content.Projectiles.Weapons.PhantasmalRuin>();
                    item.useStyle = ItemUseStyleID.Rapier; // Makes the player do the proper arm motion
                }
                if (item.type == ItemType<CalamityMod.Items.Weapons.Rogue.ShardofAntumbra>())
                {
                    item.shoot = ProjectileType<Content.Projectiles.Weapons.Antumbra>();
                    item.useStyle = ItemUseStyleID.Rapier; // Makes the player do the proper arm motion
                }
                if (item.type == ItemType<CalamityMod.Items.Weapons.Rogue.ProfanedPartisan>())
                {
                    item.shoot = ProjectileType<Content.Projectiles.Weapons.ProfanedPartisan>();
                    item.useStyle = ItemUseStyleID.Rapier; // Makes the player do the proper arm motion
                }
                if (item.type == ItemType<CalamityMod.Items.Weapons.Rogue.RealityRupture>())
                {
                    item.shoot = ProjectileType<Content.Projectiles.Weapons.RealityRapture>();
                    item.useStyle = ItemUseStyleID.Rapier; // Makes the player do the proper arm motion
                }
                if (item.type == ItemType<CalamityMod.Items.Weapons.Rogue.NightsGaze>())
                {
                    item.shoot = ProjectileType<Content.Projectiles.Weapons.NightsGaze>();
                    item.useStyle = ItemUseStyleID.Rapier; // Makes the player do the proper arm motion
                }
                if (item.type == ItemType<CalamityMod.Items.Weapons.Rogue.EclipsesFall>())
                {
                    item.shoot = ProjectileType<Content.Projectiles.Weapons.EclipseFall>();
                    item.useStyle = ItemUseStyleID.Rapier; // Makes the player do the proper arm motion
                }
                if (item.type == ItemType<CalamityMod.Items.Weapons.Rogue.TheAtomSplitter>())
                {
                    item.shoot = ProjectileType<Content.Projectiles.Weapons.AtomSplitter>();
                    item.useStyle = ItemUseStyleID.Rapier; // Makes the player do the proper arm motion
                }
                /*if (item.type == ItemType<CalamityMod.Items.Weapons.Rogue.Wrathwing>()) he does not cooperate
                {
                    item.shoot = ProjectileType<Content.Projectiles.Weapons.Wrathwing>();
                    item.useStyle = ItemUseStyleID.Rapier; // Makes the player do the proper arm motion
                }*/
                if (item.type == ItemType<CalamityMod.Items.Weapons.Rogue.ScarletDevil>())
                {
                    item.shoot = ProjectileType<Content.Projectiles.Weapons.ScarletDevil>();
                    item.useStyle = ItemUseStyleID.Rapier; // Makes the player do the proper arm motion
                }
                if (item.type == ItemType<CalamityMod.Items.Weapons.Melee.ArkoftheCosmos>())
                {
                    item.shoot = ProjectileType<Ark>();
                }
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

        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (player.Remix().taintedArchery && item.useAmmo == AmmoID.Arrow)
            {
                damage = (int)(damage * 0.9f);
                velocity *= 0.8f;
            }
            if (player.Remix().taintedMagic && item.DamageType == DamageClass.Magic)
            {
                damage = (int)(damage * 0.7f);
                knockback *= 7;
            }
            if (player.Remix().taintedTitan)
            {
                knockback = 0;
            }
        }

        public override float UseTimeMultiplier(Item item, Player player)
        {
            if (player.Remix().taintedArchery && item.useAmmo == AmmoID.Arrow)
            {
                return 0.7f;
            }
            return 1f;
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
            if (item.type == ItemID.EnchantedSword && !(DownedBossSystem.downedPerforator || DownedBossSystem.downedHiveMind) && CalRemixWorld.weaponReworks)
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
        public override void UpdateInventory(Item item, Player player)
        {
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
            if (item.type == ItemID.EnchantedSword && !(DownedBossSystem.downedPerforator || DownedBossSystem.downedHiveMind) && CalRemixWorld.weaponReworks)
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
            if (item.type == ItemID.CellPhone || 
                item.type == ItemID.Shellphone ||
                item.type == ItemID.ShellphoneDummy ||
                item.type == ItemID.ShellphoneHell ||
                item.type == ItemID.ShellphoneOcean ||
                item.type == ItemID.ShellphoneSpawn)
            {
                if (!player.GetModPlayer<CalRemixPlayer>().gottenCellPhone)
                {
                    player.GetModPlayer<CalRemixPlayer>().gottenCellPhone = true;
                }
            }
            if (item.pick > 0)
            {
                if (player.Remix().taintedMining)
                    item.pick = ContentSamples.ItemsByType[item.type].pick + 22;
                else
                    item.pick = ContentSamples.ItemsByType[item.type].pick;
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
            if (modPlayer.stratusBeverage && item != null && !item.CountsAsClass<TrueMeleeDamageClass>())
            {
                for (int i = 0; i < 2; i++)
                {
                    Vector2 starvelocity = -Vector2.UnitY.RotatedByRandom(0.6152018) * Main.rand.NextFloat(2.5f, 4f);
                    Projectile.NewProjectile(source, position, starvelocity, ProjectileType<StratusStar>(), (int)(damage * 0.33f), player.whoAmI);
                }
            }
            if (modPlayer.taintedAmmo && item.useAmmo > 0 && Main.rand.NextBool(5))
            {
                player.PickAmmo(item, out int _, out float _, out int _, out float _, out int _);

                bool infMusk = item.useAmmo == AmmoID.Bullet && player.HasItem(ItemID.EndlessMusketPouch);
                bool infAr = item.useAmmo == AmmoID.Arrow && player.HasItem(ItemID.EndlessQuiver);
                    
                if (!infMusk && !infAr)
                    Projectile.NewProjectile(source, position, velocity.RotatedBy(-Main.rand.NextFloat(-0.022f, 0.022f)), type, damage, knockback, player.whoAmI);
            }
            if (item.type == ItemType<ArkoftheCosmos>() && CalRemixWorld.weaponReworks)
            {
                if (player.ownedProjectileCounts[ProjectileType<Ark>()] <= 0)
                Projectile.NewProjectile(source, position, velocity, ProjectileType<Ark>(), damage, knockback, player.whoAmI);
                return false;
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
            if (item.type == ItemID.EnchantedSword && !(DownedBossSystem.downedPerforator || DownedBossSystem.downedHiveMind) && CalRemixWorld.weaponReworks)
            {
                TransformItem(ref item, ItemType<DisenchantedSword>());
            }
            if (item.type == ItemType<CryoKey>() && item.lavaWet)
            {
                if (!NPC.AnyNPCs(NPCType<Pyrogen>()))
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 spawnAt = item.Center + new Vector2(1500f, (float)item.height / 2f);
                        int n = NPC.NewNPC(item.GetSource_FromThis(), (int)spawnAt.X, (int)spawnAt.Y, NPCType<Pyrogen>());
                        NPC blug = Main.npc[n];
                        blug.ModNPC<Pyrogen>().enrageCounter = 2222222;
                        blug.ModNPC<Pyrogen>().ultraEnraged = true;
                        SoundStyle sound = new SoundStyle("CalRemix/Assets/Sounds/GenBosses/PyrogenPissed");
                        SoundEngine.PlaySound(sound, blug.Center);
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
            else if (item.type == ItemID.DungeonFishingCrate || item.type == ItemID.DungeonFishingCrateHard && Main.rand.NextBool(4))
            {
                itemLoot.Add(ItemType<BundleBones>(), 4, 10, 25);
                itemLoot.Add(ItemType<Watercooler>(), 20);
            }
            else if (item.type == ItemID.CorruptFishingCrate || item.type == ItemID.CorruptFishingCrateHard)
            {
                itemLoot.Add(ItemType<Grimesand>(), 1, 10, 30);
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
                itemLoot.Add(ItemType<Anomaly109>());
                itemLoot.AddIf(() => Main.netMode != NetmodeID.MultiplayerClient, ItemType<TheInsacredTexts>());
            }
            if (CalRemixAddon.CalVal != null)
            {
                if (item.type == CalRemixAddon.CalVal.Find<ModItem>("MysteryPainting").Type)
                {
                    itemLoot.Add(ItemType<MovieSign>(), 22);
                }
            }

            // boss bags
            // phm
            if (item.type == ItemID.KingSlimeBossBag)
            {

            }
            else if (item.type == ItemType<DesertScourgeBag>())
            {
                itemLoot.Add(ItemType<Duststorm>(), 1 / 3);
                itemLoot.Add(ItemType<ParchedScale>(), 1, 30, 40);
                itemLoot.RemoveWhere((rule) => rule is CommonDrop e && e.itemId == ItemType<PearlShard>());
            }
            else if (item.type == ItemID.EyeOfCthulhuBossBag)
            {

            }
            else if (item.type == ItemType<CrabulonBag>())
            {
                itemLoot.Add(ItemType<DeliciousMeat>(), 1, 4, 7);
                itemLoot.Add(ItemType<CrabLeaves>(), 1, 4, 7);
                itemLoot.Add(ItemType<OddMushroom>(), 3);
            }
            else if (item.type == ItemID.EaterOfWorldsBossBag)
            {

            }
            else if (item.type == ItemID.BrainOfCthulhuBossBag)
            {

            }
            else if (item.type == ItemType<HiveMindBag>())
            {

            }
            else if (item.type == ItemType<PerforatorBag>())
            {

            }
            else if (item.type == ItemID.QueenBeeBossBag)
            {

            }
            else if (item.type == ItemID.DeerclopsBossBag)
            {
                itemLoot.Add(ItemType<DeerdalusStormclops>(), 20);
            }
            else if (item.type == ItemID.SkeletronBossBag)
            {

            }
            else if (item.type == ItemType<SlimeGodBag>())
            {
                itemLoot.Add(ItemType<ToxicTome>(), 33);
                itemLoot.Add(ItemType<ChlorislimeStaff>(), 33);
            }
            else if (item.type == ItemID.WallOfFleshBossBag)
            {

            }
            
            // hm
            else if (item.type == ItemID.QueenSlimeBossBag)
            {

            }
            else if (item.type == ItemType<CryogenBag>())
            {
                itemLoot.Add(ItemType<FrostedFractals>(), 1 / 3);
            }
            else if (item.type == ItemID.TwinsBossBag)
            {

            }
            else if (item.type == ItemType<AquaticScourgeBag>())
            {
                itemLoot.Add(ItemType<Rainstorm>(), 1 / 3);
            }
            else if (item.type == ItemID.DestroyerBossBag)
            {

            }
            else if (item.type == ItemType<BrimstoneWaifuBag>())
            {

            }
            else if (item.type == ItemID.SkeletronPrimeBossBag)
            {

            }
            else if (item.type == ItemType<CalamitasCloneBag>())
            {
                itemLoot.Add(ItemType<RisingFire>(), 1 / 3);
                itemLoot.Add(ItemType<CalamityRing>());
            }
            else if (item.type == ItemID.PlanteraBossBag)
            {
                itemLoot.Add(ItemType<EssenceofBabil>(), 1, 5, 9);
            }
            else if (item.type == ItemType<LeviathanBag>())
            {
                itemLoot.Add(ItemType<CrocodileScale>(), 1, 20, 30);
            }
            else if (item.type == ItemType<AstrumAureusBag>())
            {
                itemLoot.Add(ItemType<SoulofBright>(), 1, 10, 12);
            }
            else if (item.type == ItemID.GolemBossBag)
            {

            }
            else if (item.type == ItemID.FishronBossBag)
            {
                itemLoot.Add(ItemType<DeliciousMeat>(), 2, 45, 92);
            }
            else if (item.type == ItemType<PlaguebringerGoliathBag>())
            {
                itemLoot.Add(ItemType<Alchemists3rdTrumpet>(), 1 / 3);
            }
            else if (item.type == ItemID.FairyQueenBossBag)
            {

            }
            else if (item.type == ItemType<RavagerBag>())
            {

            }
            else if (item.type == ItemID.CultistBossBag) // lol
            {

            }
            else if (item.type == ItemType<AstrumDeusBag>())
            {

            }
            else if (item.type == ItemID.MoonLordBossBag)
            {

            }

            // pml
            else if (item.type == ItemType<DragonfollyBag>())
            {
                itemLoot.Add(ItemType<DisgustingMeat>(), new Fraction(55, 100), 236, 650);
            }
            else if (item.type == ItemType<ProvidenceBag>())
            {
                itemLoot.Add(ItemType<ProfanedNucleus>());
                itemLoot.Add(ItemType<TorrefiedTephra>(), 1, 200, 222);
            }
            else if (item.type == ItemType<SignusBag>())
            {

            }
            else if (item.type == ItemType<StormWeaverBag>())
            {

            }
            else if (item.type == ItemType<CeaselessVoidBag>())
            {

            }
            else if (item.type == ItemType<PolterghastBag>())
            {

            }
            else if (item.type == ItemType<DevourerofGodsBag>())
            {
                itemLoot.Add(ItemType<Lean>(), 1, 6, 8);
                itemLoot.AddIf(() => CalamityWorld.revenge, ItemType<YharimBar>(), 1, 10, 30);
                itemLoot.RemoveWhere((rule) => rule is CommonDrop e && e.itemId == ItemType<CosmiliteBar>());
                itemLoot.AddIf(() => !CalRemixWorld.cosmislag, ItemType<CosmiliteBar>(), 1, 55, 65);
            }
            else if (item.type == ItemType<YharonBag>())
            {
                LeadingConditionRule yhar = itemLoot.DefineConditionalDropSet(() => CalamityWorld.revenge);
                yhar.Add(ItemType<YharimBar>(), 1, 10, 30, hideLootReport: !CalamityWorld.revenge);
                yhar.AddFail(ItemType<YharimBar>(), 1, 60, 80, hideLootReport: CalamityWorld.revenge);
                itemLoot.Add(yhar);
                itemLoot.Add(ItemType<MovieSign>(), 100);
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
                yhar.Add(ItemType<YharimBar>(), 1, 90, 110, hideLootReport: !CalamityWorld.revenge);
                yhar.AddFail(ItemType<YharimBar>(), 1, 70, 90, hideLootReport: CalamityWorld.revenge);
                itemLoot.Add(yhar);
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
                modplayer.elastigel = true;
                modplayer.irategel = true;
                modplayer.invigel = true;
            }
            if (item.type == ItemType<TheAbsorber>())
            {
                if (!hideVisual)
                {
                    calplayer.regenator = true;
                }

                modplayer.elastigel = true;
                modplayer.invigel = true;
                modplayer.irategel = true;
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

                modplayer.elastigel = true;
                modplayer.invigel = true;
                modplayer.irategel = true;
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
                GetModItem(ItemType<TheEvolution>()).UpdateAccessory(player, hideVisual);
                GetModItem(ItemType<Affliction>()).UpdateAccessory(player, hideVisual);
                GetModItem(ItemType<CorrosiveSpine>()).UpdateAccessory(player, hideVisual);
                GetModItem(ItemType<LeviathanAmbergris>()).UpdateAccessory(player, hideVisual);
                if (!hideVisual)
                GetModItem(ItemType<OldDukeScales>()).UpdateAccessory(player, hideVisual);
                player.sporeSac = true;
                GetModItem(ItemType<Abaddon>()).UpdateAccessory(player, hideVisual);
                player.magmaStone = true;
                GetModItem(ItemType<DynamoStemCells>()).UpdateAccessory(player, hideVisual);
                GetModItem(ItemType<BlazingCore>()).UpdateAccessory(player, hideVisual);
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

        public override bool ConsumeItem(Item item, Player player)
        {
            // Infinite white blocks that arent ores
            if (player.Remix().taintedBuilder)
            {
                if (item.rare == ItemRarityID.White)
                {
                    if (item.createTile > -1 || item.createWall > 0)
                    {
                        if (!(item.createTile > -1 && TileID.Sets.Ore[item.createTile]) && Main.rand.NextBool(5))
                            return false;
                    }
                }
            }
            return true;
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

            if (!Main.dedServ)
                ScreenHelperManager.sceneMetrics.onscreenItems.Add(item);
        }

        public override bool CanEquipAccessory(Item item, Player player, int slot, bool modded)
        {
            if (item.type == ItemType<SoulofCryogen>())
            {
                return slot == GetInstance<SoulSlot>().Type;
            }
            else
                return true;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            string key = "Items.Tooltips.";
            if (item.DamageType == GetInstance<StormbowDamageClass>() && item.damage < 30 && item.rare <= ItemRarityID.Blue)
            {
                if (tooltips.Exists((TooltipLine t) => t.Name.Equals("Tooltip0")))
                {
                    TooltipLine line = tooltips.Find((TooltipLine t) => t.Name.Equals("Tooltip0"));
                    if (string.IsNullOrWhiteSpace(line.Text))
                    {
                        TooltipLine tip = new(Mod, "CalRemix:Stormbow", CalRemixHelper.LocalText($"{key}StormbowTip").Value);
                        tooltips.Add(tip);
                    }
                }
                else
                {
                    TooltipLine tip = new(Mod, "CalRemix:Stormbow", CalRemixHelper.LocalText($"{key}StormbowTip").Value);
                    tooltips.Add(tip);
                }
            }
            if (devItem != string.Empty)
            {
                string text = CalamityUtils.ColorMessage($"- {CalRemixHelper.LocalText($"{key}Lightmix")} ", Color.Crimson);
                text += CalamityUtils.ColorMessage((devItem.Equals("Remix")) ? " " : $": {devItem} ", Color.Gold);
                text += CalamityUtils.ColorMessage("-", Color.Crimson);
                TooltipLine tip = new(Mod, "CalRemix:Dev", text);
                tooltips.Add(tip);
            }
            if (CalRemixWorld.aspids)
            {
                if (item.type == ItemType<CryoKey>())
                {
                    var line = new TooltipLine(Mod, "CryoKeyRemix", CalRemixHelper.LocalText($"{key}CryoKeyRemix").Value);
                    tooltips.Add(line);
                }
            }
            if (CalRemixWorld.clamitas)
            {
                if (item.type == ItemType<EyeofDesolation>())
                {
                    var line = new TooltipLine(Mod, "EyeofDesolationRemix", CalRemixHelper.LocalText($"{key}EyeofDesolationRemix").Value);
                    tooltips.Add(line);
                }
            }
            if (CalRemixWorld.plaguetoggle)
            {
                if (item.type == ItemType<Abombination>())
                {
                    tooltips.FindAndReplace(CalRemixHelper.LocalText($"{key}AbombinationOld").Value, CalRemixHelper.LocalText($"{key}AbombinationNew").Value);
                }
            }
            if (CalRemixWorld.fearmonger)
            {
                if (item.type == ItemType<FearmongerGreathelm>())
                {
                    tooltips.FindAndReplace("+60 max mana and ", "");
                    tooltips.FindAndReplace("20% increased summon damage and +2 max minions", "+1 max minions");
                    for (int i = 0; i < tooltips.Count; i++)
                    {
                        if (tooltips[i].Text.Contains("Pumpkin"))
                        {
                            tooltips.RemoveAt(i);
                            break;
                        }
                    }
                    tooltips.Add(new TooltipLine(Mod, "FearmongerRemix", "+Set bonus: +1 max minions\nThe minion damage nerf while wielding weaponry is reduced\nAll minion attacks grant regeneration"));
                }
                if (item.type == ItemType<FearmongerPlateMail>())
                {
                    tooltips.FindAndReplace("+100 max life and ", "");
                    for (int i = 0; i < tooltips.Count; i++)
                    {
                        if (tooltips[i].Text.Contains("Pumpkin"))
                        {
                            tooltips.RemoveAt(i);
                        }
                    }
                    tooltips.Add(new TooltipLine(Mod, "FearmongerRemix", "+Set bonus: 1 max minions\nThe minion damage nerf while wielding weaponry is reduced\nAll minion attacks grant regeneration"));
                }
                if (item.type == ItemType<FearmongerGreaves>())
                {
                    for (int i = 0; i < tooltips.Count; i++)
                    {
                        if (tooltips[i].Text.Contains("Pumpkin"))
                        {
                            tooltips.RemoveAt(i);
                            break;
                        }
                    }
                    tooltips.Add(new TooltipLine(Mod, "FearmongerRemix", "+Set bonus: +1 max minions\nThe minion damage nerf while wielding weaponry is reduced\nAll minion attacks grant regeneration"));
                }
            }
            if (Torch.Contains(item.type))
            {
                var line = new TooltipLine(Mod, "TorchRemix", CalRemixHelper.LocalText($"{key}TorchRemix").Value);
                line.OverrideColor = Color.OrangeRed;
                tooltips.Add(line);
            }
            if (item.type == ItemType<PhantomicArtifact>())
            {
                var line = new TooltipLine(Mod, "PhantomicSoulArtifact", CalRemixHelper.LocalText($"{key}PhantomicSoulArtifact").Value);
                tooltips.Add(line);
            }
            if (item.type == ItemType<GrandGelatin>())
            {
                var line = new TooltipLine(Mod, "GrandGelatinRemix", CalRemixHelper.LocalText($"{key}GrandGelatinRemix").Value);
                tooltips.Add(line);
            }
            if (item.type == ItemType<TheAbsorber>())
            {
                var line = new TooltipLine(Mod, "AbsorberRemix", CalRemixHelper.LocalText($"{key}AbsorberRemix").Value);
                tooltips.Add(line);
            }
            if (item.type == ItemType<TheSponge>())
            {
                var line = new TooltipLine(Mod, "SpongeRemix", CalRemixHelper.LocalText($"{key}SpongeRemix").Value);
                tooltips.Add(line);
            }
            if (item.type == ItemType<AmbrosialAmpoule>())
            {
                var line = new TooltipLine(Mod, "AmbrosiaRemix", CalRemixHelper.LocalText($"{key}AmbrosiaRemix").Value);
                tooltips.Add(line);
            }
            if (item.type == ItemType<AbyssalDivingGear>())
            {
                var line = new TooltipLine(Mod, "DivingGearRemix", CalRemixHelper.LocalText($"{key}DivingGearRemix").Value);
                tooltips.Add(line);
            }
            if (item.type == ItemType<AbyssalDivingSuit>())
            {
                var line = new TooltipLine(Mod, "DivingSuitRemix", CalRemixHelper.LocalText($"{key}DivingSuitRemix").Value);
                tooltips.Add(line);
            }
            if (item.type == ItemType<TheAmalgam>())
            {
                var line = new TooltipLine(Mod, "AmalgamRemix", CalRemixHelper.LocalText($"{key}AmalgamRemix").Value);
                tooltips.Add(line);
            }
            if (item.type == ItemType<DesertMedallion>())
            {
                var line = new TooltipLine(Mod, "MedallionRemix", CalRemixHelper.LocalText($"{key}MedallionRemix").Value);
                tooltips.Add(line);
            }
            if (item.type == ItemType<HadalStew>())
            {
                var line = new TooltipLine(Mod, "HadalStewRemix", CalRemixHelper.LocalText($"{key}HadalStewRemix").Value);
                tooltips.Add(line);
            }
            if (item.type == ItemType<SoulofCryogen>())
            {
                var line = new TooltipLine(Mod, "SoulofCryogenRemix", CalamityUtils.ColorMessage(CalRemixHelper.LocalText($"{key}SoulofCryogenRemix").Value, Color.LightSkyBlue));
                tooltips.Add(line);
            }
            if (item.type == ItemType<MetalMonstrosity>())
            {
                int idx = -1;
                for (int i = 0; i < tooltips.Count; i++)
                {
                    if (tooltips[i].Name == "Tooltip0")
                    {
                        idx = i;
                    }
                    if (tooltips[i].Text.Contains("hurt"))
                    {
                        tooltips.RemoveAt(i);
                    }
                }

                var line = new TooltipLine(Mod, "MetalMonstrosityRemix", CalRemixHelper.LocalText($"{key}MetalMonstrosityRemix").Value);
                tooltips.Insert(idx, line);
            }
            if (CalRemixPlayer.dyeStats.ContainsKey(item.type) && CalRemixWorld.dyeStats)
            {
                DyeStats stats = CalRemixPlayer.dyeStats[item.type];
                string ret = "";
                if (stats.red != 0)
                    ret += $"[c/ff0000:{CalRemixHelper.LocalText($"Items.DyeStats.Red").Format(WhichIncrement(stats.red), Math.Abs(stats.red))}]\n";
                if (stats.orange != 0)
                    ret += $"[c/ffa200:{CalRemixHelper.LocalText($"Items.DyeStats.Orange").Format(WhichIncrement(stats.orange), Math.Abs(stats.orange))}]\n";
                if (stats.yellow != 0)
                    ret += $"[c/ffff00:{CalRemixHelper.LocalText($"Items.DyeStats.Yellow").Format(WhichIncrement(stats.yellow), Math.Abs(stats.yellow))}]\n";
                if (stats.lime != 0)
                    ret += $"[c/a2ff00:{CalRemixHelper.LocalText($"Items.DyeStats.Lime").Format(WhichIncrement(stats.lime), Math.Abs(stats.lime))}]\n";
                if (stats.green != 0)
                    ret += $"[c/00ff00:{CalRemixHelper.LocalText($"Items.DyeStats.Green").Format(WhichIncrement(stats.green), Math.Abs(stats.green))}]\n";
                if (stats.cyan != 0)
                    ret += $"[c/00ffff:{CalRemixHelper.LocalText($"Items.DyeStats.Cyan").Format(WhichIncrement(stats.cyan), Math.Abs(stats.cyan))}]\n";
                if (stats.teal != 0)
                    ret += $"[c/008080:{CalRemixHelper.LocalText($"Items.DyeStats.Teal").Format(WhichIncrement(stats.teal), Math.Abs(stats.teal))}]\n";
                if (stats.skyblue != 0)
                    ret += $"[c/66a3ff:{CalRemixHelper.LocalText($"Items.DyeStats.SkyBlue").Format(WhichIncrement(stats.skyblue), Math.Abs(stats.skyblue) * 10)}]\n";
                if (stats.blue != 0)
                    ret += $"[c/0000ff:{CalRemixHelper.LocalText($"Items.DyeStats.Blue").Format(WhichIncrement(stats.blue), Math.Abs(stats.blue))}]\n";
                if (stats.purple != 0)
                    ret += $"[c/9400cf:{CalRemixHelper.LocalText($"Items.DyeStats.Purple").Format(WhichIncrement(stats.purple), Math.Abs(stats.purple))}]\n";
                if (stats.violet != 0)
                    ret += $"[c/ff00b7:{CalRemixHelper.LocalText($"Items.DyeStats.Violet").Format(WhichIncrement(stats.violet), Math.Abs(stats.violet))}]\n";
                if (stats.pink != 0)
                    ret += $"[c/ff45a2:{CalRemixHelper.LocalText($"Items.DyeStats.Pink").Format(WhichIncrement(stats.pink), Math.Abs(stats.pink))}]\n";
                if (stats.brown != 0)
                    ret += $"[c/7a4b00:{CalRemixHelper.LocalText($"Items.DyeStats.Brown").Format(WhichIncrement(stats.brown), Math.Abs(stats.brown))}]\n";
                if (stats.silver != 0)
                    ret += $"[c/ffffff:{CalRemixHelper.LocalText($"Items.DyeStats.Silver").Format(WhichIncrement(stats.silver), Math.Abs(stats.silver))}]\n";
                if (stats.black != 0)
                    ret += $"[c/000000:{CalRemixHelper.LocalText($"Items.DyeStats.Black").Format(WhichIncrement(stats.black), Math.Abs(stats.black))}]\n";
                tooltips.Add(new TooltipLine(Mod, "DyeStats", ret));
            }
        }
        public static string WhichIncrement(int stat)
        {
            if (stat > 0)
                return CalRemixHelper.LocalText("Items.DyeStats.Increment").Value;
            else if (stat < 0)
                return CalRemixHelper.LocalText("Items.DyeStats.Decrement").Value;
            else
                return CalRemixHelper.LocalText("Items.DyeStats.NoChange").Value;
        }
    }
}
