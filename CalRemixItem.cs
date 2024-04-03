﻿using Terraria;
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

namespace CalRemix
{
    public class CalRemixItem : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public bool Scoriad = false;
        public int NonScoria = -1;
        public List<int> BossBags = new()
        {
            ItemID.KingSlimeBossBag,
            ItemID.EyeOfCthulhuBossBag,
            ItemID.EaterOfWorldsBossBag,
            ItemID.BrainOfCthulhuBossBag,
            ItemID.QueenBeeBossBag,
            ItemID.DeerclopsBossBag,
            ItemID.SkeletronBossBag,
            ItemID.WallOfFleshBossBag,
            ItemID.QueenSlimeBossBag,
            ItemID.DestroyerBossBag,
            ItemID.TwinsBossBag,
            ItemID.SkeletronPrimeBossBag,
            ItemID.PlanteraBossBag,
            ItemID.GolemBossBag,
            ItemID.FishronBossBag,
            ItemID.FairyQueenBossBag,
            ItemID.CultistBossBag,
            ItemID.MoonLordBossBag,
            ModContent.ItemType<DesertScourgeBag>(),
            ModContent.ItemType<CrabulonBag>(),
            ModContent.ItemType<HiveMindBag>(),
            ModContent.ItemType<PerforatorBag>(),
            ModContent.ItemType<SlimeGodBag>(),
            ModContent.ItemType<CryogenBag>(),
            ModContent.ItemType<AquaticScourgeBag>(),
            ModContent.ItemType<BrimstoneWaifuBag>(),
            ModContent.ItemType<CalamitasCloneBag>(),
            ModContent.ItemType<LeviathanBag>(),
            ModContent.ItemType<AstrumAureusBag>(),
            ModContent.ItemType<PlaguebringerGoliathBag>(),
            ModContent.ItemType<RavagerBag>(),
            ModContent.ItemType<AstrumDeusBag>(),
            ModContent.ItemType<DragonfollyBag>(),
            ModContent.ItemType<ProvidenceBag>(),
            ModContent.ItemType<CeaselessVoidBag>(),
            ModContent.ItemType<StormWeaverBag>(),
            ModContent.ItemType<SignusBag>(),
            ModContent.ItemType<PolterghastBag>(),
            ModContent.ItemType<OldDukeBag>(),
            ModContent.ItemType<DevourerofGodsBag>(),
            ModContent.ItemType<YharonBag>(),
            ModContent.ItemType<CalamitasCoffer>(),
            ModContent.ItemType<DraedonBag>(),
        };
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ModContent.ItemType<LoreAwakening>())
            {
                var line = new TooltipLine(Mod, "AwakeningRemix", "[c/FF0000:Right click to begin the trial] \n[c/FF0000:Although you won't die, please do not get hit]");
                tooltips.Add(line);
            }
        }
        public override void SetDefaults(Item item)
        {
            if (item.type == ModContent.ItemType<GildedProboscis>())
            {
                item.damage = item.damage / 9;
                item.rare = ItemRarityID.LightRed;
            }
            else if (item.type == ModContent.ItemType<GoldenEagle>() || item.type == ModContent.ItemType<RougeSlash>())
            {
                item.damage = item.damage / 5;
                item.rare = ItemRarityID.LightRed;
            }
            else if (item.type == ModContent.ItemType<Swordsplosion>())
            {
                item.damage = item.damage / 3;
                item.rare = ItemRarityID.LightRed;
            }
            else if (item.type == ModContent.ItemType<EssenceofHavoc>())
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
        }
        public override bool CanRightClick(Item item)
        {
            if (item.type == ModContent.ItemType<LoreAwakening>())
                return true;
            return false;
        }
        public override void RightClick(Item item, Player player)
        {
            if (player.whoAmI == Main.myPlayer && item.type == ModContent.ItemType<LoreAwakening>())
            {
                player.QuickSpawnItem(Entity.GetSource_None(), item);
                int type = ModContent.NPCType<TheCalamity>();

                if (Main.netMode != NetmodeID.MultiplayerClient)
                    NPC.SpawnOnPlayer(player.whoAmI, type);
                else
                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: type);
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
       
        public override void MeleeEffects(Item item, Player Player, Rectangle hitbox)
        {
            if (item.CountsAsClass<MeleeDamageClass>() && item.shoot == ProjectileID.None && Player.GetModPlayer<CalRemixPlayer>().godfather && Main.rand.NextBool(20))
            {
                var source = Player.GetSource_ItemUse(item);
                if (Player.whoAmI == Main.myPlayer)
                {
                    double damageMult = item.useTime / 30D;
                    if (damageMult < 0.35)
                        damageMult = 0.35;

                    int newDamage = (int)(item.damage * 2 * damageMult);

                    int projtype = Main.rand.Next(4);
                    switch (projtype)
                    {
                        case 0:
                            projtype = ModContent.ProjectileType<BlazingSun>();
                            break;
                        case 1:
                            projtype = ModContent.ProjectileType<MiniatureFolly>();
                            break;
                        case 2:
                            projtype = ProjectileID.Mushroom;
                            break;
                        default:
                            projtype = ModContent.ProjectileType<LuxorsGiftMagic>();
                            break;
                    }
                    Vector2 mousedir = Main.MouseWorld - Player.Center;
                    mousedir.Normalize();
                    Projectile.NewProjectile(source, Player.Center, mousedir * 6, projtype, newDamage, 0f, Player.whoAmI, 0f, 0f);
                    
                }
            }
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
            if (modPlayer.godfather)
            {
                if (Main.rand.NextBool(20) && !item.channel)
                {
                    double damageMult = item.useTime / 30D;
                    if (damageMult < 0.35)
                        damageMult = 0.35;

                    int newDamage = (int)(damage * 2 * damageMult);

                    int projtype = Main.rand.Next(4);
                    switch (projtype)
                    {
                        case 0:
                            projtype = ModContent.ProjectileType<BlazingSun>();
                            break;
                        case 1:
                            projtype = ModContent.ProjectileType<MiniatureFolly>();
                            break;
                        case 2:
                            projtype = ProjectileID.Mushroom;
                            break;
                        default:
                            projtype = ModContent.ProjectileType<LuxorsGiftMagic>();
                            break;
                    }

                    if (player.whoAmI == Main.myPlayer)
                    {
                        int projectile = Projectile.NewProjectile(source, position, velocity * 1.25f, projtype, newDamage, 2f, player.whoAmI);
                        if (projectile.WithinBounds(Main.maxProjectiles))
                            Main.projectile[projectile].DamageType = DamageClass.Generic;
                    }
                }

            }
            else if (calPlayer.amalgam)
            {
                if (Main.rand.NextBool(20) && !item.channel)
                {
                    double damageMult = item.useTime / 30D;
                    if (damageMult < 0.35)
                        damageMult = 0.35;

                    int newDamage = (int)(damage * 2 * damageMult);

                    int projtype = Main.rand.NextBool(2) ? ModContent.ProjectileType<BlazingSun>() : ModContent.ProjectileType<MiniatureFolly>();

                    if (player.whoAmI == Main.myPlayer)
                    {
                        int projectile = Projectile.NewProjectile(source, position, velocity * 1.25f, projtype, newDamage, 2f, player.whoAmI);
                        if (projectile.WithinBounds(Main.maxProjectiles))
                            Main.projectile[projectile].DamageType = DamageClass.Generic;
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

                    if (Main.tile[(int)item.Bottom.X / 16, (int)item.Bottom.Y / 16].TileType == ModContent.TileType<GrimesandPlaced>())
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
            else if (item.type == ModContent.ItemType<DragonfollyBag>())
            {
                itemLoot.Add(ModContent.ItemType<DesertFeather>(), 1, 25, 30);
                itemLoot.RemoveWhere((rule) => rule is CommonDrop e && e.itemId == ModContent.ItemType<EffulgentFeather>());
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
                itemLoot.AddIf(()=> Main.netMode != NetmodeID.MultiplayerClient, ModContent.ItemType<Anomaly109>());
                itemLoot.AddIf(() => Main.netMode != NetmodeID.MultiplayerClient, ModContent.ItemType<TheInsacredTexts>());
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
            OldDukeScalesPlayer dukePlayer = player.GetModPlayer<OldDukeScalesPlayer>();
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
                calplayer.absorber = true;
                calplayer.spongeShieldVisible = !hideVisual;
                calplayer.baroclaw = true; // cringe you
                calplayer.lifejelly = true;
                calplayer.cleansingjelly = true;
                calplayer.regenator = true;
                calplayer.ursaSergeant = true;
                calplayer.trinketOfChi = true;
                calplayer.aSpark = true;
                calplayer.flameLickedShell = true;
                calplayer.permafrostsConcoction = true;
                calplayer.aquaticHeart = true;
                calplayer.aquaticHeartHide = hideVisual;
                dukePlayer.OldDukeScalesOn = true;
                if (calplayer.SpongeShieldDurability > 0)
                {
                    player.statDefense += 30;
                    player.endurance += 0.3f;
                }
            }
            if (item.type == ModContent.ItemType<AmbrosialAmpoule>() || item.type == ModContent.ItemType<TheVerbotenOne>())
            {
                if (player.ZoneJungle)
                {
                    player.lifeRegen += 1;
                    player.statDefense += 9;
                    player.endurance += 0.05f;
                }

                player.buffImmune[BuffID.Venom] = true;
                player.buffImmune[BuffID.Poisoned] = true;

                if (!player.honey && player.lifeRegen < 0)
                {
                    player.lifeRegen += 2;
                    if (player.lifeRegen > 0)
                        player.lifeRegen = 0;
                }

                player.lifeRegenTime += 1;
                player.lifeRegen += 2;

                if (player.ZoneDirtLayerHeight || player.ZoneRockLayerHeight || player.ZoneUnderworldHeight)
                {
                    player.statDefense += 10;
                    player.endurance += 0.05f;
                    player.pickSpeed -= 0.2f;
                }
            }
            if (item.type == ModContent.ItemType<AbyssalDivingSuit>() || item.type == ModContent.ItemType<TheGodfather>() || item.type == ModContent.ItemType<TheVerbotenOne>())
            {
                calplayer.lumenousAmulet = true;
                calplayer.abyssalAmulet = true;
                calplayer.aquaticEmblem = true;
                calplayer.alluringBait = true;
                player.pickSpeed -= 0.15f;
                if (!hideVisual)
                {
                    player.findTreasure = true;
                }
                player.npcTypeNoAggro[NPCID.Shark] = true;
                player.npcTypeNoAggro[NPCID.SeaSnail] = true;
                player.npcTypeNoAggro[NPCID.PinkJellyfish] = true;
                player.npcTypeNoAggro[NPCID.Crab] = true;
                player.npcTypeNoAggro[NPCID.Squid] = true;
            }
            if (item.type == ModContent.ItemType<AbyssalDivingGear>())
            {
                player.npcTypeNoAggro[NPCID.Shark] = true;
                player.npcTypeNoAggro[NPCID.SeaSnail] = true;
                player.npcTypeNoAggro[NPCID.PinkJellyfish] = true;
                player.npcTypeNoAggro[NPCID.Crab] = true;
                player.npcTypeNoAggro[NPCID.Squid] = true;
            }
            if (item.type == ModContent.ItemType<TheAmalgam>() || item.type == ModContent.ItemType<Slimelgamation>() || item.type == ModContent.ItemType<TheGodfather>() || item.type == ModContent.ItemType<TheVerbotenOne>())
            {
                calplayer.giantPearl = true;
                if (!hideVisual)
                calplayer.manaOverloader = true;
                calplayer.frostFlare = true;
                calplayer.voidOfExtinction = true;
                player.strongBees = true;
                calplayer.uberBees = true;
                calplayer.alchFlask = true;
                calplayer.purity = true;
                calplayer.evolution = true;
                calplayer.affliction = true;
                //calplayer.oldDukeScales = true;
                var source = player.GetSource_Accessory(item);
                if (Main.rand.NextBool(3))
                {
                    if (player.whoAmI == Main.myPlayer)
                    {
                        int microbeDamage = (int)player.GetBestClassDamage().ApplyTo(500);
                        int choice = Main.rand.Next(4);
                        switch (choice)
                        {
                            case 0:
                                choice = ModContent.ProjectileType<PoisonousSeawater>();
                                break;
                            case 1:
                                choice = ModContent.ProjectileType<PlagueBeeSmall>();
                                break;
                            case 2:
                                choice = ModContent.ProjectileType<Corrocloud1>();
                                break;
                            default:
                                choice = ProjectileID.SporeCloud;
                                break;
                        }
                        int p = Projectile.NewProjectile(source, player.Center.X, player.Center.Y, 0f, 0f, choice, microbeDamage, 0f, player.whoAmI, 0f, 0f);
                        if (p.WithinBounds(Main.maxProjectiles))
                        {
                            Main.projectile[p].DamageType = DamageClass.Generic;
                            Main.projectile[p].usesLocalNPCImmunity = true;
                            Main.projectile[p].localNPCHitCooldown = 10;
                            Main.projectile[p].originalDamage = microbeDamage;
                        }
                    }
                }
                CalamityPlayer modPlayer = player.Calamity();
                modPlayer.voidOfCalamity = true;
                modPlayer.abaddon = true;
                player.buffImmune[ModContent.BuffType<BrimstoneFlames>()] = true;
                player.magmaStone = true;
                player.buffImmune[BuffID.OnFire] = true;
                player.fireWalk = true;
                player.lavaRose = true;
                if (player.immune)
                {
                    if (player.miscCounter % 10 == 0)
                    {
                        if (player.whoAmI == Main.myPlayer)
                        {
                            int damage = (int)player.GetBestClassDamage().ApplyTo(300);
                            Projectile fire = CalamityUtils.ProjectileRain(source, player.Center, 400f, 100f, 500f, 800f, 22f, ModContent.ProjectileType<StandingFire>(), damage, 5f, player.whoAmI);
                            if (fire.whoAmI.WithinBounds(Main.maxProjectiles))
                            {
                                fire.usesLocalNPCImmunity = true;
                                fire.localNPCHitCooldown = 60;
                            }
                        }
                    }
                }
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
    }
}
