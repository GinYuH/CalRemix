using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.CalPlayer;
using CalamityMod.CalPlayer.Dashes;
using CalamityMod.NPCs.SlimeGod;
using CalamityMod.Items;
using static Terraria.ModLoader.ModContent;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Items.Accessories;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod;
using Microsoft.Xna.Framework;
using CalamityMod.Projectiles.Summon;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Projectiles.Typeless;
using Terraria;
using System.Collections.Generic;
using CalRemix.Projectiles.Accessories;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Accessories
{
    [AutoloadEquip(EquipType.Wings)]
    public class TheVerbotenOne : ModItem
    {

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("The Verboten One");
            Tooltip.SetDefault("Ultimatum\n"+
            "Provides all effects of its ingredients"); 
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(500, 14f, 3.2f);
        }

        public override void SetDefaults()
        {
            Item.defense = 20;
            Item.width = 20;
            Item.height = 20;
            Item.value = CalamityGlobalItem.RarityHotPinkBuyPrice;
            Item.accessory = true;
            Item.rare = ModContent.RarityType<HotPink>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer calPlayer = player.Calamity();
            var source = player.GetSource_Accessory(Item);
            calPlayer.absorber = true;
            calPlayer.sponge = true;
            player.statManaMax2 += 30;
            CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();
            calPlayer.amalgam = true;
            calPlayer.transformer = true;
            calPlayer.aSpark = true;
            calPlayer.hideOfDeus = true;
            calPlayer.nCore = true;
            modPlayer.godfather = true;
            modPlayer.tvo = true;
            modPlayer.arcanumHands = true;
            modPlayer.brimPortal = true;
            modPlayer.roguebox = true;
            if (hideVisual)
                modPlayer.tvohide = true;
            else
                modPlayer.tvohide = false;
            calPlayer.nanotech = true;
            calPlayer.draedonsHeart = true;
            calPlayer.blazingCursorDamage = true;
            calPlayer.blazingCursorVisuals = true;
            player.autoJump = true;
            player.jumpSpeedBoost += 0.6f;
            player.noFallDmg = true;
            player.blackBelt = true;
            calPlayer.DashID = StatisVoidSashDash.ID;
            player.dashType = 0;
            player.spikedBoots = 2;
            calPlayer.aAmpoule = true;
            calPlayer.laudanum = true;
            calPlayer.heartOfDarkness = true;
            calPlayer.stressPills = true;
            calPlayer.chaliceOfTheBloodGod = true;
            calPlayer.fleshTotem = true;
            calPlayer.healingPotionMultiplier += 0.25f;
            calPlayer.raiderTalisman = true;
            calPlayer.electricianGlove = true;
            calPlayer.filthyGlove = true;
            calPlayer.bloodyGlove = true;
            player.autoJump = true;
            player.jumpSpeedBoost += 0.6f;
            player.noFallDmg = true;
            player.blackBelt = true;
            player.spikedBoots = 2;
            player.maxMinions += 20;
            player.Calamity().infiniteFlight = true;

            player.GetDamage<GenericDamageClass>() += 0.15f;
            player.GetArmorPenetration<GenericDamageClass>() += 150;

            if (player.immune)
            {
                if (player.miscCounter % 6 == 0)
                {
                    if (player.whoAmI == Main.myPlayer)
                    {
                        int damage = (int)player.GetBestClassDamage().ApplyTo(300);
                        Projectile rain = CalamityUtils.ProjectileRain(source, player.Center, 400f, 100f, 500f, 800f, 22f, ProjectileType<AuraRain>(), damage, 2f, player.whoAmI);
                        if (rain.whoAmI.WithinBounds(Main.maxProjectiles))
                        {
                            rain.DamageType = DamageClass.Generic;
                            rain.tileCollide = false;
                            rain.penetrate = 1;
                        }
                        Projectile star = CalamityUtils.ProjectileRain(source, player.Center, 400f, 100f, 500f, 800f, 22f, ProjectileType<AstralStarMagic>(), damage, 2f, player.whoAmI);
                        if (star.whoAmI.WithinBounds(Main.maxProjectiles))
                        {
                            star.DamageType = DamageClass.Generic;
                            star.tileCollide = false;
                            star.penetrate = 1;
                        }
                        Projectile ball = CalamityUtils.ProjectileRain(source, player.Center, 400f, 100f, 500f, 800f, 22f, ProjectileType<CalamityMod.Projectiles.Typeless.SkyFlareFriendly>(), damage, 2f, player.whoAmI);
                        if (star.whoAmI.WithinBounds(Main.maxProjectiles))
                        {
                            star.DamageType = DamageClass.Generic;
                            star.tileCollide = false;
                            star.penetrate = 1;
                        }
                        int microbeDamage = (int)player.GetBestClassDamage().ApplyTo(20);
                        int p = Projectile.NewProjectile(source, player.Center.X, player.Center.Y, 0f, 0f, ProjectileID.TruffleSpore, microbeDamage, 0f, player.whoAmI, 0f, 0f);
                        if (p.WithinBounds(Main.maxProjectiles))
                        {
                            Main.projectile[p].DamageType = DamageClass.Generic;
                            Main.projectile[p].usesLocalNPCImmunity = true;
                            Main.projectile[p].localNPCHitCooldown = 10;
                            Main.projectile[p].originalDamage = microbeDamage;
                        }
                    }
                }
                modPlayer.nuclegel = true;
                calPlayer.royalGel = true;
                modPlayer.amalgel = true;
                if (!CalamityMod.Events.BossRushEvent.BossRushActive)
                {
                    player.npcTypeNoAggro[NPCType<CrimsonSlimeSpawn>()] = true;
                    player.npcTypeNoAggro[NPCType<CrimsonSlimeSpawn2>()] = true;
                    player.npcTypeNoAggro[NPCType<CorruptSlimeSpawn>()] = true;
                    player.npcTypeNoAggro[NPCType<CorruptSlimeSpawn2>()] = true;
                    player.npcTypeNoAggro[NPCType<AeroSlime>()] = true;
                    player.npcTypeNoAggro[NPCType<BloomSlime>()] = true;
                    player.npcTypeNoAggro[NPCType<CalamityMod.NPCs.Crags.InfernalCongealment>()] = true;
                    player.npcTypeNoAggro[NPCType<CalamityMod.NPCs.Astral.AstralSlime>()] = true;
                    player.npcTypeNoAggro[NPCType<CalamityMod.NPCs.PlagueEnemies.PestilentSlime>()] = true;
                    player.npcTypeNoAggro[NPCType<CryoSlime>()] = true;
                    player.npcTypeNoAggro[NPCType<PerennialSlime>()] = true;
                    player.npcTypeNoAggro[NPCType<NPCs.AuricSlime>()] = true;
                    player.npcTypeNoAggro[NPCID.SlimeSpiked] = true;
                    player.npcTypeNoAggro[NPCID.QueenSlimeMinionBlue] = true;
                    player.npcTypeNoAggro[NPCID.QueenSlimeMinionPink] = true;
                    player.npcTypeNoAggro[NPCID.QueenSlimeMinionPurple] = true;
                    player.npcTypeNoAggro[1] = true;
                    player.npcTypeNoAggro[16] = true;
                    player.npcTypeNoAggro[59] = true;
                    player.npcTypeNoAggro[71] = true;
                    player.npcTypeNoAggro[81] = true;
                    player.npcTypeNoAggro[138] = true;
                    player.npcTypeNoAggro[121] = true;
                    player.npcTypeNoAggro[122] = true;
                    player.npcTypeNoAggro[141] = true;
                    player.npcTypeNoAggro[147] = true;
                    player.npcTypeNoAggro[183] = true;
                    player.npcTypeNoAggro[184] = true;
                    player.npcTypeNoAggro[204] = true;
                    player.npcTypeNoAggro[225] = true;
                    player.npcTypeNoAggro[244] = true;
                    player.npcTypeNoAggro[302] = true;
                    player.npcTypeNoAggro[333] = true;
                    player.npcTypeNoAggro[335] = true;
                    player.npcTypeNoAggro[334] = true;
                    player.npcTypeNoAggro[336] = true;
                    player.npcTypeNoAggro[537] = true;
                }
            }
            int brimmy = ProjectileType<CriticalSlimeCore>();

            if (player.whoAmI == Main.myPlayer)
            {
                int baseDamage = 600;
                int swordDmg = (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(baseDamage);
                if (player.ownedProjectileCounts[brimmy] < 1 && !hideVisual)
                {
                    var sword = Projectile.NewProjectileDirect(source, player.Center, Vector2.Zero, brimmy, swordDmg, 2f, Main.myPlayer);
                    sword.originalDamage = baseDamage;
                }
            }
            player.noKnockback = true;
            player.fireWalk = true;
            player.statLifeMax2 += 40;
            player.lifeRegen++;
            for (int i = 0; i < BuffLoader.BuffCount; i++) 
            {
                if (i != BuffID.PotionSickness && Main.debuff[i])
                player.buffImmune[i] = true;
            }
            if (Collision.DrownCollision(player.position, player.width, player.height, player.gravDir))
            { player.statDefense += 20; }
            if (player.wingTime < 500)
            {
                player.wingTime = 500;
            }
            player.noFallDmg = true;
            calPlayer.eTalisman = true;
            player.GetModPlayer<CalRemixPlayer>().marnite = true;

            player.manaMagnet = true;

            player.statManaMax2 += 250;
            player.GetDamage<MagicDamageClass>() += 0.3f;
            player.manaCost *= 0f;
            player.GetCritChance<MagicDamageClass>() += 25;


            if (Main.LocalPlayer.HasBuff(BuffID.ManaSickness))
            {
                if (!Main.LocalPlayer.HasCooldown(MagnaCoreCooldown.ID) && player.active)
                {
                    float variance = MathHelper.TwoPi / 8;
                    for (int i = 0; i < 8; i++)
                    {
                        Vector2 velocitye = new Vector2(0f, 10f);
                        velocitye = velocitye.RotatedBy(variance * i);
                        int p = Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center, velocitye, ModContent.ProjectileType<RainbowComet>(), 20000, 0, player.whoAmI);
                        if (Main.projectile.IndexInRange(p))
                            Main.projectile[p].originalDamage = 20000;
                        Main.projectile[p].GetGlobalProjectile<CalRemixProjectile>().tvoproj = true;
                    }
                    Main.LocalPlayer.AddCooldown(MagnaCoreCooldown.ID, CalamityUtils.SecondsToFrames(10));
                }
            }
            player.magicQuiver = true;
            calPlayer.deadshotBrooch = true;
            calPlayer.rangedAmmoCost *= 0f;
            calPlayer.spiritOrigin = true;
            player.GetCritChance<RangedDamageClass>() += 25;
            modPlayer.brimPortal = true;
            if (player.whoAmI == Main.myPlayer)
            {
                if (player.ownedProjectileCounts[ModContent.ProjectileType<BrimstoneGenerator>()] < 4 && !hideVisual)
                {
                    for (int v = 0; v < 4; v++)
                    {
                        Projectile.NewProjectileDirect(source, player.Center, Vector2.Zero, ModContent.ProjectileType<BrimstoneGenerator>(), 0, 0f, Main.myPlayer, v);
                    }
                }
            }
            calPlayer.stealthGenStandstill += 0.25f;
            calPlayer.rogueStealthMax += 0.5f;
            calPlayer.eclipseMirror = true;
            calPlayer.stealthStrikeHalfCost = true;
            calPlayer.rogueStealth = calPlayer.rogueStealthMax;
            player.GetCritChance<ThrowingDamageClass>() += 6;
            player.GetDamage<ThrowingDamageClass>() += 0.06f;
            player.aggro -= 800;
            player.luck += 0.2f;
            if (!hideVisual)
            player.Calamity().thiefsDime = true;
            player.GetModPlayer<CalRemixPlayer>().roguebox = true;
            calPlayer.eGauntlet = true;
            player.kbGlove = true;
            player.autoReuseGlove = true;
            player.meleeScaleGlove = true;
            player.yoyoGlove = true;
            player.yoyoString = true;
            player.counterWeight = 1;


            calPlayer.shadowMinions = true;
            calPlayer.holyMinions = true;
            calPlayer.voltaicJelly = true;
            calPlayer.starTaintedGenerator = true;
            calPlayer.nucleogenesis = true;

            calPlayer.howlTrio = true;
            calPlayer.howlsHeart = true;
            calPlayer.brimstoneWaifu = true;
            calPlayer.sirenWaifu = true;
            calPlayer.sandWaifu = true;
            calPlayer.sandBoobWaifu = true;
            calPlayer.cloudWaifu = true;
            calPlayer.MutatedTruffleBool = true;
            calPlayer.miniOldDuke = true;
            calPlayer.allWaifus = true;
            calPlayer.elementalHeart = true;
            calPlayer.virili = true;

            int brimmye = ProjectileType<BrimstoneElementalMinion>();
            int siren = ProjectileType<WaterElementalMinion>();
            int healer = ProjectileType<SandElementalHealer>();
            int sandy = ProjectileType<SandElementalMinion>();
            int cloudy = ProjectileType<CloudElementalMinion>();
            int thomas = ProjectileType<PlaguePrincess>();
            int yd = ProjectileType<MutatedTruffleMinion>();
            calPlayer.gladiatorSword = true;

            Vector2 velocity = new Vector2(0f, -1f);
            int elementalDmg = (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(290);
            float kBack = 2f + player.GetKnockback<SummonDamageClass>().Additive;

            if (modPlayer.tvohide && CalamityUtils.CountProjectiles(ProjectileType<HowlsHeartCalcifer>()) > 0)
            {
                List<int> MinionList = new List<int>
                {
                    ModContent.ProjectileType<HowlsHeartCalcifer>(),
                    ModContent.ProjectileType<HowlsHeartHowl>(),
                    ModContent.ProjectileType<HowlsHeartTurnipHead>(),
                    ModContent.ProjectileType<SandElementalHealer>(),
                    ModContent.ProjectileType<BrimstoneElementalMinion>(),
                    ModContent.ProjectileType<CloudElementalMinion>(),
                    ModContent.ProjectileType<SandElementalMinion>(),
                    ModContent.ProjectileType<WaterElementalMinion>(),
                    ModContent.ProjectileType<PlaguePrincess>(),
                    ModContent.ProjectileType<MutatedTruffleMinion>(),
                    ModContent.ProjectileType<CryonicShield>()
                };
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile projectile = Main.projectile[i];
                    if (MinionList.Contains(projectile.type))
                    {
                        projectile.active = false;
                    }
                }
            }
            if (player.whoAmI == Main.myPlayer && !hideVisual)
            {
                int baseDamage = 290;
                int swordDmg = (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(baseDamage);
                if (player.ownedProjectileCounts[brimmye] < 1)
                {
                    int p = Projectile.NewProjectile(source, player.Center, velocity, brimmye, elementalDmg, kBack, player.whoAmI);
                    if (Main.projectile.IndexInRange(p))
                        Main.projectile[p].originalDamage = 20;
                }
                if (player.ownedProjectileCounts[siren] < 1)
                {
                    int p = Projectile.NewProjectile(source, player.Center, velocity, siren, elementalDmg, kBack, player.whoAmI);
                    if (Main.projectile.IndexInRange(p))
                        Main.projectile[p].originalDamage = 290;
                }
                if (player.ownedProjectileCounts[healer] < 1)
                {
                    int p = Projectile.NewProjectile(source, player.Center, velocity, healer, elementalDmg, kBack, player.whoAmI);
                    if (Main.projectile.IndexInRange(p))
                        Main.projectile[p].originalDamage = 290;
                }
                if (player.ownedProjectileCounts[sandy] < 1)
                {
                    int p = Projectile.NewProjectile(source, player.Center, velocity, sandy, elementalDmg, kBack, player.whoAmI);
                    if (Main.projectile.IndexInRange(p))
                        Main.projectile[p].originalDamage = 290;
                }
                if (player.ownedProjectileCounts[cloudy] < 1)
                {
                    int p = Projectile.NewProjectile(source, player.Center, velocity, cloudy, elementalDmg, kBack, player.whoAmI);
                    if (Main.projectile.IndexInRange(p))
                        Main.projectile[p].originalDamage = 290;
                }
                if (player.ownedProjectileCounts[thomas] < 1)
                {
                    int p = Projectile.NewProjectile(source, player.Center, velocity, thomas, elementalDmg, kBack, player.whoAmI);
                    if (Main.projectile.IndexInRange(p))
                        Main.projectile[p].originalDamage = 290;
                }
                if (player.ownedProjectileCounts[ProjectileType<HowlsHeartHowl>()] < 1)
                {
                    int damage = (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(290);
                    Projectile howl = Projectile.NewProjectileDirect(source, player.Center, -Vector2.UnitY, ProjectileType<HowlsHeartHowl>(), damage, 1f, player.whoAmI, 0f, 1f);
                    howl.originalDamage = damage;
                }
                if (player.ownedProjectileCounts[ProjectileType<HowlsHeartCalcifer>()] < 1)
                {
                    Projectile.NewProjectile(source, player.Center, -Vector2.UnitY, ProjectileType<HowlsHeartCalcifer>(), 0, 0f, player.whoAmI, 0f, 0f);
                }
                if (player.ownedProjectileCounts[ProjectileType<HowlsHeartTurnipHead>()] < 1)
                {
                    Projectile.NewProjectile(source, player.Center, -Vector2.UnitY, ProjectileType<HowlsHeartTurnipHead>(), 0, 0f, player.whoAmI, 0f, 0f);
                }
                if (player.ownedProjectileCounts[ProjectileType<MutatedTruffleMinion>()] < 1)
                {
                    int damage = (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(290);
                    Projectile dud = Projectile.NewProjectileDirect(source, player.Center, -Vector2.UnitY, ProjectileType<MutatedTruffleMinion>(), damage, 1f, player.whoAmI, 0f, 1f);
                    dud.originalDamage = damage;
                }
            }


            if (!hideVisual)
            calPlayer.CryoStone = true;
            player.wingTimeMax = (int)(player.wingTimeMax * 1.5f);
            if (!hideVisual)
                calPlayer.ChaosStone = true;
            modPlayer.crystalconflict = true;
            int brimmyt = ProjectileType<CosmicConflict>();

            if (player.whoAmI == Main.myPlayer && !hideVisual)
            {
                int baseDamage = 4002;
                int swordDmg = (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(baseDamage);
                if (player.ownedProjectileCounts[brimmyt] < 1)
                {
                    var sword = Projectile.NewProjectileDirect(source, player.Center, Vector2.Zero, brimmyt, swordDmg, 2f, Main.myPlayer);
                    sword.originalDamage = baseDamage;
                }
            }

            // Provide life benefits if the player is standing on ground and has typical gravity.
            int x = (int)player.Center.X / 16;
            int y = (int)(player.Bottom.Y - 1f) / 16;
            Tile groundTile = CalamityMod.CalamityUtils.ParanoidTileRetrieval(x, y + 1);
            bool groundTileIsSolid = groundTile.HasUnactuatedTile && (Main.tileSolid[groundTile.TileType] || Main.tileSolidTop[groundTile.TileType]);
            if (groundTileIsSolid && player.gravDir == 1f)
                calPlayer.BloomStoneRegen = true;

            // Grow chid
            if (player.whoAmI == Main.myPlayer && player.velocity.Y == 0f && player.grappling[0] == -1)
            {
                Tile walkTile = CalamityMod.CalamityUtils.ParanoidTileRetrieval(x, y);
                if (!walkTile.HasTile && walkTile.LiquidAmount == 0 && groundTile != null && WorldGen.SolidTile(groundTile) && Main.rand.NextBool(2048))
                {
                    if (groundTile.TileType == TileID.Stone || groundTile.TileType == TileID.Grass)
                    {
                        WorldGen.PlaceObject(x, y, (ushort)ModContent.TileType<Tiles.CosmichidPlant>());
                    }
                }
            }
        }
        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 1f;
            ascentWhenRising = 0.175f;
            maxCanAscendMultiplier = 1.2f;
            maxAscentMultiplier = 3.25f;
            constantAscend = 0.15f;
        }

        public override void AddRecipes()
        {
            { 
            CreateRecipe().
                AddIngredient<TheGodfather>().
                AddIngredient<ThrowersGauntlet>().
                AddIngredient<QuiverofMadness>().
                AddIngredient<MagnaCore>().
                AddIngredient<ZenithArcanum>().
                AddIngredient<CalamityMod.Items.Accessories.Nanotech>().
                AddIngredient<RoguesLootbox>().
                AddIngredient<StatisVoidSash>().
                AddIngredient<AsgardianAegis>().
                AddIngredient<ReaperToothNecklace>().
                AddIngredient<ChaliceOfTheBloodGod>().
                AddIngredient<RampartofDeities>().
                AddIngredient<UniversalStone>().
                AddIngredient<Calamity>().
                AddIngredient<DraedonsHeart>().
                AddIngredient<YharimsGift>().
                AddIngredient<OccultSkullCrown>().
                AddIngredient<HolyMantle>().
                AddIngredient<CalamityMod.Items.Accessories.Wings.TracersSeraph>().
                AddIngredient<TheCommunity>().
                AddIngredient<CalamitousSoulArtifact>().
                AddIngredient<AmbrosialAmpoule>().
                AddIngredient(ItemID.CelestialShell).
                AddIngredient<ShadowspecBar>(50).
                AddTile<DraedonsForge>().
                Register();
            }
            { 
            CreateRecipe().
                AddIngredient<TheGodfather>().
                AddIngredient<ThrowersGauntlet>().
                AddIngredient<QuiverofMadness>().
                AddIngredient<MagnaCore>().
                AddIngredient<ZenithArcanum>().
                AddIngredient<CalamityMod.Items.Accessories.Nanotech>().
                AddIngredient<RoguesLootbox>().
                AddIngredient<StatisVoidSash>().
                AddIngredient<AsgardianAegis>().
                AddIngredient<ReaperToothNecklace>().
                AddIngredient<ChaliceOfTheBloodGod>().
                AddIngredient<RampartofDeities>().
                AddIngredient<UniversalStone>().
                AddIngredient<Calamity>().
                AddIngredient<DraedonsHeart>().
                AddIngredient<YharimsGift>().
                AddIngredient<OccultSkullCrown>().
                AddIngredient<HolyMantle>().
                AddIngredient<CalamityMod.Items.Accessories.Wings.TracersSeraph>().
                AddIngredient<ShatteredCommunity>().
                AddIngredient<CalamitousSoulArtifact>().
                AddIngredient<AmbrosialAmpoule>().
                AddIngredient(ItemID.CelestialShell).
                AddIngredient<ShadowspecBar>(50).
                AddTile<DraedonsForge>().
                Register();
            }
        }
    }
}
