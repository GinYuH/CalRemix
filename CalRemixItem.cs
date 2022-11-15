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
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Events;
using CalamityMod.NPCs.Bumblebirb;
using CalamityMod.Items.SummonItems;
using CalamityMod.Items.Materials;
using System.Collections.Generic;

namespace CalRemix
{
	public class CalRemixItem : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.type == ModContent.ItemType<GildedProboscis>())
            {
                item.damage = item.damage / 4;
                item.rare = ItemRarityID.LightRed;
            }
            else if (item.type == ModContent.ItemType<GoldenEagle>() || item.type == ModContent.ItemType<RougeSlash>() || item.type == ModContent.ItemType<Swordsplosion>())
            {
                item.damage = item.damage / 2;
                item.rare = ItemRarityID.LightRed;
            }
            else if (item.type == ModContent.ItemType<PearlShard>())
            {
                item.SetNameOverride("Conquest Fragment");
                item.rare = ItemRarityID.Orange;
            }
            else if (item.type == ModContent.ItemType<PhantomicArtifact>())
            {
                item.SetNameOverride("Phantomic Soul Artifact");
            }

        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ModContent.ItemType<PearlShard>())
            {
                var line = new TooltipLine(Mod, "ConquestFragment", "\'Victory is yours!\'");
                tooltips.Add(line);
            }
            if (item.type == ModContent.ItemType<PhantomicArtifact>())
            {
                var line = new TooltipLine(Mod, "PhantomicSoulArtifact", "Judgement");
                tooltips.Add(line);
            }
        }
        public override bool CanUseItem(Item item, Player player)
        {
            if (item.type == ModContent.ItemType<ExoticPheromones>())
            {
                return player.ZoneDesert && !NPC.AnyNPCs(ModContent.NPCType<Bumblefuck>()) && !BossRushEvent.BossRushActive;
            }
            return true;
        }
        public override void UpdateInventory(Item item, Player player)
        {
            if (item.type == ModContent.ItemType<Elderberry>() && item.stack > 1)
            {
                item.stack = 1;
            }
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
            
            return true;
        }

        public override void Update(Item item, ref float gravity, ref float maxFallSpeed)
        {
            if (item.type == ItemID.Apple)
            {
                if (item.wet && !item.lavaWet && Main.bloodMoon && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
                {
                    item.type = ModContent.ItemType<BloodOrange>();
                }
            }
            if (item.type == ModContent.ItemType<Elderberry>() && item.stack > 1)
            {
                item.stack = 1;
            }
        }
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if (item.type == ItemID.FloatingIslandFishingCrate || item.type == ItemID.FloatingIslandFishingCrateHard)
            {
                itemLoot.AddIf(() => NPC.AnyNPCs(NPCID.WyvernHead) && CalamityMod.DownedBossSystem.downedYharon && !Main.LocalPlayer.Calamity().dFruit, ModContent.ItemType<Dragonfruit>(), 1);
                itemLoot.AddIf(() => NPC.AnyNPCs(NPCID.WyvernHead) && CalamityMod.DownedBossSystem.downedYharon && Main.LocalPlayer.Calamity().dFruit, ModContent.ItemType<Dragonfruit>(), 20);
            }
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
            if (item.type == ModContent.ItemType<TheSponge>() || item.type == ModContent.ItemType<TheGodfather>())
            {
                calplayer.regenator = true;
                calplayer.ursaSergeant = true;
                calplayer.trinketOfChi = true;
                calplayer.aSpark = true;
                calplayer.flameLickedShell = true;
                calplayer.permafrostsConcoction = true;
                calplayer.aquaticHeart = true;
                calplayer.roverDrive = true;
            }
            if (item.type == ModContent.ItemType<AmbrosialAmpoule>())
            {
                calplayer.beeResist = true;

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
            if (item.type == ModContent.ItemType<AbyssalDivingSuit>() || item.type == ModContent.ItemType<TheGodfather>())
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
            if (item.type == ModContent.ItemType<TheAmalgam>() || item.type == ModContent.ItemType<Slimelgamation>() || item.type == ModContent.ItemType<TheGodfather>())
            {
                calplayer.giantPearl = true;
                if (!hideVisual)
                calplayer.manaOverloader = true;
                calplayer.frostFlare = true;
                calplayer.voidOfExtinction = true;
                player.strongBees = true;
                calplayer.uberBees = true;
                calplayer.alchFlask = true;
                calplayer.astralArcanum = true;
                calplayer.projRefRare = true;
                calplayer.affliction = true;
                calplayer.oldDukeScales = true;
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
    }
}
