using static Terraria.ModLoader.ModContent;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Items.Accessories;
using CalamityMod.Items.Armor.Empyrean;
using CalamityMod.Items.Armor.Plaguebringer;
using CalamityMod.Items.Placeables.Furniture;
using CalamityMod.Items.Fishing.AstralCatches;
using CalRemix.Content.Items.Potions;
using CalamityMod.Items.Potions;
using CalamityMod.Items.Placeables.Ores;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Items.Placeables;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Potions.Alcohol;
using CalamityMod.Items.PermanentBoosters;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Accessories.Wings;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.SummonItems;
using CalamityMod.Items.Fishing.FishingRods;
using CalamityMod.Items.Placeables.Furniture.CraftingStations;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Rogue;
using CalRemix.Content.Items.Weapons;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Items.Armor.PlagueReaper;
using CalamityMod.Items.Armor.Prismatic;
using CalamityMod.Items.Armor.Silva;
using CalamityMod.Items.Armor.Fearmonger;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CalamityMod.Items.Armor;
using CalamityMod.Items.Placeables.FurnitureStratus;
using CalamityMod.Items.LoreItems;
using CalamityMod;
using CalRemix.Content.NPCs.TownNPCs;
using CalRemix.Core.World;
using CalRemix.Content.Items.Critters;
using CalRemix.Content.Items.Lore;
using CalRemix.Content.Items.Ammo;
using CalRemix.Content.Items.Placeables.MusicBoxes;
using CalamityMod.Items.Placeables;

namespace CalRemix
{
    public partial class Recipes : ModSystem
    {
        public static RecipeGroup Blinkchid, Daychid, Moonchid, Deathchid, Waterchid, Firechid, Shiverchid;
        public static RecipeGroup GreaterEvil, EvilBar, T4Bar, HMT1Bar;
        public static RecipeGroup AnyButterfly;
        public override void Unload()
        {
            Blinkchid = null;
            Daychid = null;
            Moonchid = null;
            Deathchid = null;
            Waterchid = null;
            Firechid = null;
            Shiverchid = null;
            GreaterEvil = null;
            EvilBar = null;
            T4Bar = null;
            HMT1Bar = null;
            AnyButterfly = null;
        }
        public override void AddRecipeGroups()
        {
            CosmichidGroup(Blinkchid, "Blinkchid", ItemID.Blinkroot);
            CosmichidGroup(Daychid, "Daychid", ItemID.Daybloom);
            CosmichidGroup(Moonchid, "Moonchid", ItemID.Moonglow);
            CosmichidGroup(Deathchid, "Deathchid", ItemID.Deathweed);
            CosmichidGroup(Waterchid, "Waterchid", ItemID.Waterleaf);
            CosmichidGroup(Firechid, "Firechid", ItemID.Fireblossom);
            CosmichidGroup(Shiverchid, "Shiverchid", ItemID.Shiverthorn);

            GreaterEvil = new RecipeGroup(() => "Any Greater Evil Flesh", ItemType<RottenMatter>(), ItemType<BloodSample>());
            RecipeGroup.RegisterGroup("CalRemix:GreaterEvil", GreaterEvil);
            EvilBar = new RecipeGroup(() => "Any Evil Bar", ItemID.DemoniteBar, ItemID.CrimtaneBar);
            RecipeGroup.RegisterGroup("CalRemix:EvilBar", EvilBar);
            T4Bar = new RecipeGroup(() => "Any Tier 4 Bar", ItemID.GoldBar, ItemID.PlatinumBar);
            RecipeGroup.RegisterGroup("CalRemix:T4Bar", T4Bar);
            HMT1Bar = new RecipeGroup(() => "Any Tier 1 Hardmode Bar", ItemID.CobaltBar, ItemID.PalladiumBar);
            RecipeGroup.RegisterGroup("CalRemix:HMT1Bar", HMT1Bar);

            AnyButterfly = new RecipeGroup(() => "Any Normal Butterfly", ItemID.MonarchButterfly, ItemID.SulphurButterfly, ItemID.ZebraSwallowtailButterfly, ItemID.UlyssesButterfly, ItemID.JuliaButterfly, ItemID.RedAdmiralButterfly, ItemID.PurpleEmperorButterfly, ItemID.TreeNymphButterfly);
            RecipeGroup.RegisterGroup("CalRemix:AnyButterfly", HMT1Bar);
        }
        public override void AddRecipes()
        {
            Mod music = CalRemix.CalMusic;
            {
                Recipe box = Recipe.Create(music.Find<ModItem>("Interlude1MusicBox").Type);
                box.AddIngredient(ItemID.MusicBox)
                .AddIngredient<ConquestFragment>(30)
                .AddCondition(new Condition("After Calamitas Clone is defeated", () => DownedBossSystem.downedCalamitasClone))
                .AddTile(TileID.TinkerersWorkbench)
                .Register();

                Recipe box2 = Recipe.Create(music.Find<ModItem>("Interlude2MusicBox").Type);
                box2.AddIngredient(ItemID.MusicBox)
                .AddIngredient<ConquestFragment>(30)
                .AddCondition(new Condition("After Moon Lord is defeated", () => NPC.downedMoonlord))
                .AddTile(TileID.TinkerersWorkbench)
                .Register();

                Recipe box3 = Recipe.Create(music.Find<ModItem>("DevourerofGodsEulogyMusicBox").Type);
                box3.AddIngredient(ItemID.MusicBox)
                .AddIngredient<ConquestFragment>(30)
                .AddCondition(new Condition("After the Devourer of Gods is defeated", () => DownedBossSystem.downedDoG))
                .AddTile(TileID.TinkerersWorkbench)
                .Register();

                Recipe box4 = Recipe.Create(music.Find<ModItem>("Interlude3MusicBox").Type);
                box4.AddIngredient(ItemID.MusicBox)
                .AddIngredient<ConquestFragment>(30)
                .AddCondition(new Condition("After Yharon is defeated", () => DownedBossSystem.downedYharon))
                .AddTile(TileID.TinkerersWorkbench)
                .Register();

                Recipe box5 = Recipe.Create(music.Find<ModItem>("AcidRainTier1MusicBox").Type);
                box5.AddIngredient<AcidRainTier2MusicBox>()
                .AddIngredient(music.Find<ModItem>("SulphurousSeaDayMusicBox").Type)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();

                Recipe box6 = Recipe.Create(music.Find<ModItem>("AcidRainTier1MusicBox").Type);
                box6.AddIngredient<AcidRainTier2MusicBox>()
                .AddIngredient(music.Find<ModItem>("SulphurousSeaNightMusicBox").Type)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
            }
            {
                Recipe slumbering = Recipe.Create(ItemType<LoreAwakening>());
                slumbering.AddIngredient<Slumbering>()
                .Register();
            }
            {
                Recipe alloy = Recipe.Create(ItemType<LifeAlloy>());
                alloy.AddIngredient<LifeOre>(5)
                .AddCondition(new Condition("While the Anomaly 109 \'life_ore\' setting is enabled", () => CalRemixWorld.alloyBars))
                .AddTile(TileID.AdamantiteForge)
                .Register();
            }
            {
                Recipe coin = Recipe.Create(ItemID.PlatinumCoin, 100);
                coin.AddIngredient<CosmiliteCoin>(1);
                coin.Register();
            }
            {
                Recipe cell = Recipe.Create(ItemType<PlagueCellCanister>(), 1);
                cell.AddRecipeGroup(RecipeGroupID.IronBar);
                cell.AddCondition(new Condition("While the Anomaly 109 \'coyote_venom\' setting is enabled", () => CalRemixWorld.wolfvenom));
                cell.Register();
            }
            {
                Recipe cell = Recipe.Create(ItemType<DubiousPlating>(), 4);
                cell.AddIngredient(ItemType<ArsenicOre>(), 8);
                cell.AddIngredient(ItemID.Bone);
                cell.AddRecipeGroup("AnyGoldBar");
                cell.AddTile(TileID.Furnaces);
                cell.Register();
            }
            {
                Recipe bar = Recipe.Create(ItemType<CosmiliteBar>(), 1);
                bar.AddIngredient<CosmiliteSlag>(5);
                bar.AddTile(TileID.LunarCraftingStation)
                .AddCondition(new Condition("While the Anomaly 109 \'cosmilite_slag\' setting is enabled", () => CalRemixWorld.cosmislag));
                bar.Register();
            }
            {
                Recipe bar = Recipe.Create(ItemType<MidasPrime>(), 1);
                bar.AddIngredient<MinnowsPrimeItem>();
                bar.AddTile(TileID.CookingPots);
                bar.Register();
            }
            {
                Recipe halibut = Recipe.Create(ItemType<HalibutCannon>());
                halibut.AddIngredient<FlounderMortar>();
                halibut.AddIngredient<ReaperTooth>(20);
                halibut.AddIngredient<Lumenyl>(20);
                halibut.AddIngredient<SubnauticalPlate>(5)
                .AddTile<CalamityMod.Tiles.Furniture.CraftingStations.CosmicAnvil>()
                .Register();
            }
            {
                Recipe bar = Recipe.Create(ItemID.CookedFish, 1);
                bar.AddIngredient<CrocodileHerringItem>();
                bar.AddTile(TileID.CookingPots);
                bar.Register();
            }
            {
                Recipe ash = Recipe.Create(ItemType<CirrusDress>(), 1);
                ash.AddIngredient(ItemID.Silk, 6);
                ash.AddIngredient(ItemID.AshBlock, 10);
                ash.AddTile(TileID.Loom);
                ash.Register();
            }
            {
                Recipe recipe = Recipe.Create(ItemType<CryoKey>(), 1)
                .AddIngredient(ItemID.ShadowKey)
                .AddIngredient(ItemType<CryoKeyMold>())
                .AddIngredient(ItemID.SoulofLight, 5)
                .AddIngredient(ItemID.SoulofNight, 5)
                .AddIngredient(ItemID.SoulofFlight, 5)
                .AddCondition(new Condition("While the Anomaly 109 \'primal_aspids\' setting is enabled", () => CalRemixWorld.aspids))
                .Register();
            }
            {
                Recipe recipe = Recipe.Create(ItemID.WaterBucket, 1);
                recipe.AddIngredient(ItemID.EmptyBucket);
                recipe.AddIngredient(ItemType<SoulofHydrogen>(), 2);
                recipe.AddIngredient(ItemType<SoulofOxygen>());
                recipe.DisableDecraft();
                recipe.Register();
            }

            #region DP stuff
            // Alcohol...
            AlcoholRecipe(ItemType<CaribbeanRum>(), ItemType<Vodka>(), ItemID.BambooBlock, ItemType<LivingShard>(), 20, 20);
            AlcoholRecipe(ItemType<CinnamonRoll>(), ItemType<Everclear>(), ItemType<Whiskey>(), ItemID.BeetleHusk, 5, 1);
            AlcoholRecipe(ItemType<Everclear>(), ItemType<Margarita>(), ItemID.Hay, ItemType<AureusCell>(), 10, 1);
            AlcoholRecipe(ItemType<EvergreenGin>(), ItemType<Vodka>(), ItemID.PineTreeBlock, ItemType<LivingShard>(), 20, 2);
            AlcoholRecipe(ItemType<Fireball>(), ItemID.Ale, ItemType<BloodOrange>(), ItemID.UnicornHorn, 40, 1);
            AlcoholRecipe(ItemType<GrapeBeer>(), ItemID.Ale, ItemID.GrapeJuice, ItemID.UnicornHorn, 40, 1);
            AlcoholRecipe(ItemType<Margarita>(), ItemType<Vodka>(), ItemID.Starfruit, ItemType<LivingShard>(), 20, 1);
            AlcoholRecipe(ItemType<Moonshine>(), ItemType<Everclear>(), ItemID.Ale, ItemID.BeetleHusk, 5, 1);
            AlcoholRecipe(ItemType<MoscowMule>(), ItemType<Everclear>(), ItemType<Vodka>(), ItemID.BeetleHusk, 5, 1);
            AlcoholRecipe(ItemType<RedWine>(), ItemID.Ale, ItemID.Grapes, ItemID.UnicornHorn, 40, 1);
            AlcoholRecipe(ItemType<Rum>(), ItemID.Ale, ItemID.BambooBlock, ItemID.UnicornHorn, 40, 20);
            AlcoholRecipe(ItemType<Screwdriver>(), ItemType<NotFabsolVodka>(), ItemType<BloodOrange>(), ItemType<HallowedOre>(), 30, 1);
            AlcoholRecipe(ItemType<StarBeamRye>(), ItemType<Margarita>(), ItemID.Hay, ItemType<AureusCell>(), 10, 20);
            AlcoholRecipe(ItemType<Tequila>(), ItemID.Ale, ItemID.Hay, ItemID.UnicornHorn, 40, 20);
            AlcoholRecipe(ItemType<TequilaSunrise>(), ItemType<Everclear>(), ItemType<Tequila>(), ItemID.BeetleHusk, 5, 1);
            AlcoholRecipe(ItemType<Vodka>(), ItemType<NotFabsolVodka>(), ItemID.Hay, ItemType<HallowedOre>(), 30, 40);
            AlcoholRecipe(ItemType<Whiskey>(), ItemID.Ale, ItemID.BottledWater, ItemID.UnicornHorn, 40, 1);
            AlcoholRecipe(ItemType<WhiteWine>(), ItemType<NotFabsolVodka>(), ItemID.Grapes, ItemType<HallowedOre>(), 30, 1);
            // Candles
            CandleRecipe(ItemType<ResilientCandle>(), ItemID.SoulofNight, 445, ItemType<EssenceofBabil>(), 444);
            CandleRecipe(ItemType<SpitefulCandle>(), ItemType<EssenceofSunlight>(), 1098, ItemType<EssenceofHavoc>(), 987);
            CandleRecipe(ItemType<VigorousCandle>(), ItemID.SoulofLight, 277, ItemType<EssenceofSunlight>(), 128);
            CandleRecipe(ItemType<WeightlessCandle>(), ItemID.SoulofFlight, 3422, ItemType<EssenceofEleum>(), 357);
            // Bloody Mary exception
            {
                Recipe recipe = Recipe.Create(ItemType<BloodyMary>(), 5);
                recipe.AddIngredient<Margarita>(5);
                recipe.AddIngredient<BloodOrb>(30);
                recipe.AddIngredient<AureusCell>(1);
                recipe.AddTile(TileID.AlchemyTable);
                recipe.Register();
            }
            #endregion
        }
        public override void PostAddRecipes()
        {
            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe recipe = Main.recipe[i];

                CosmichidChange(recipe, "Blinkchid", ItemID.Blinkroot);
                CosmichidChange(recipe, "Daychid", ItemID.Daybloom);
                CosmichidChange(recipe, "Moonchid", ItemID.Moonglow);
                CosmichidChange(recipe, "Deathchid", ItemID.Deathweed);
                CosmichidChange(recipe, "Waterchid", ItemID.Waterleaf);
                CosmichidChange(recipe, "Firechid", ItemID.Fireblossom);
                CosmichidChange(recipe, "Shiverchid", ItemID.Shiverthorn);

                if (recipe.HasResult(ItemType<FabsolsVodka>()))
                {
                    recipe.DisableRecipe();
                }
                if (recipe.HasIngredient(ItemType<GrandScale>()))
                {
                    recipe.DisableRecipe();
                }
                if (recipe.HasResult(ItemType<CryonicBrick>()) && recipe.HasIngredient(ItemType<CryonicOre>()))
                {
                    recipe.DisableDecraft();
                }
                if (recipe.HasResult(ItemType<ShadowspecBar>()))
                {
                    recipe.AddIngredient<SubnauticalPlate>();
                }
                if (recipe.HasResult(ItemType<AstralChunk>()))
                {
                    recipe.RemoveIngredient(ItemID.FallenStar);
                    recipe.AddRecipeGroup(RecipeGroupID.IronBar, 8);
                }
                if (recipe.HasResult(ItemType<ProfanedShard>()))
                {
                    recipe.AddCondition(new Condition("Locked recipe. Drops from Yggdrasil Ents in Hallow and Hell.", () => false));
                }
                if (recipe.HasResult(ItemType<AcesHigh>()))
                {
                    recipe.AddIngredient<AcesLow>();
                }
                if (recipe.HasResult(ItemType<CosmicImmaterializer>()))
                {
                    recipe.RemoveIngredient(ItemType<SanctifiedSpark>());
                    recipe.AddIngredient(ItemType<DarkEnergyStaff>());
                }
                if (recipe.HasResult(ItemType<Supernova>()))
                {
                    recipe.RemoveIngredient(ItemType<SealedSingularity>());
                    recipe.AddIngredient(ItemType<UnsealedSingularity>());
                    recipe.AddIngredient(ItemType<ProfanedNucleus>());
                }
                if (recipe.HasResult(ItemType<Apotheosis>()))
                {
                    recipe.RemoveIngredient(ItemID.SpellTome);
                    recipe.AddIngredient(ItemType<WrathoftheCosmos>());
                }
                if (recipe.HasResult(ItemType<Voidragon>()))
                {
                    recipe.RemoveIngredient(ItemType<Seadragon>());
                    recipe.AddIngredient(ItemType<Megaskeet>());
                }
                bool shard = (recipe.HasResult(ItemType<SeaRemains>()) || recipe.HasResult(ItemType<MonstrousKnives>()) || recipe.HasResult(ItemType<FirestormCannon>()) );
                if (recipe.HasIngredient(ItemType<PearlShard>()) && shard)
		        {
                    recipe.RemoveIngredient(ItemType<PearlShard>());
                    recipe.AddIngredient(ItemType<ParchedScale>());
                }
                bool shard2 = recipe.HasResult(ItemType<NavyFishingRod>()) || recipe.HasResult(ItemType<EutrophicShelf>()) || recipe.HasResult(ItemType<AquamarineStaff>()) || recipe.HasResult(ItemType<Riptide>()) || recipe.HasResult(ItemType<SeashineSword>()) || recipe.HasResult(ItemType<StormSurge>()) || recipe.HasResult(ItemType<SeafoamBomb>());
                if (recipe.HasIngredient(ItemType<PearlShard>()) && shard2)
                {
                    recipe.RemoveIngredient(ItemType<PearlShard>());
                }
                if (recipe.TryGetIngredient(ItemType<PearlShard>(), out Item pShard) && !(shard || shard))
                {
                    pShard.type = ItemType<ConquestFragment>();
                }
                if (recipe.HasResult(ItemType<TearsofHeaven>()))
                {
                    recipe.RemoveIngredient(ItemID.WaterBolt);
                    recipe.RemoveIngredient(ItemType<CoreofSunlight>());
                    recipe.AddIngredient(ItemType<PlasmaflashBolt>());
                    recipe.AddIngredient(ItemType<SaltWaterBolt>());
                }
                if (recipe.HasResult(ItemType<Elderberry>()))
                {
                    recipe.AddCondition(new Condition("Locked recipe. Trade with the Dye Trader Post-Providence.", () => !CalRemixWorld.permanenthealth));
                }
                if (recipe.HasResult(ItemType<MiracleFruit>()))
                {
                    recipe.AddCondition(new Condition("Locked recipe. Find in post-Golem Jungle Planetoids.", () => !CalRemixWorld.permanenthealth));
                }
                if (recipe.HasResult(ItemType<Dragonfruit>()))
                {
                    recipe.AddCondition(new Condition("Locked recipe. Open an Azure Crate while a Wyvern is alive post-Yharon", () => !CalRemixWorld.permanenthealth));
                }
                if (recipe.HasResult(ItemType<BloodOrange>()))
                {
                    recipe.AddCondition(new Condition("Locked recipe. Throw an Apple in water during a Blood Moon post-mechs.", () => !CalRemixWorld.permanenthealth));
                }
                if (recipe.HasResult(ItemType<CometShard>()))
                {
                    recipe.AddCondition(new Condition("Locked recipe. Get a Nova to collide with Astral Ore.", () => !CalRemixWorld.permanenthealth));
                }
                if (recipe.HasResult(ItemType<EtherealCore>()))
                {
                    recipe.AddCondition(new Condition("Locked recipe. Insert a Bloody Vein at the Astral Beacon.", () => !CalRemixWorld.permanenthealth));
                }
                if (recipe.HasResult(ItemType<PhantomHeart>()))
                {
                    recipe.AddCondition(new Condition("Locked recipe. Spawns in the Post-Polterghast Dungeon.", () => !CalRemixWorld.permanenthealth));
                }
                if (recipe.HasResult(ItemType<DesertMedallion>()))
                {
                    recipe.AddCondition(new Condition("Locked recipe. Drops from Cnidrions after defeating the Wulfrum Excavator.", () => false));
                }
                if (recipe.HasResult(ItemType<CryoKey>()))
                {
                    recipe.AddCondition(new Condition("Locked recipe. Drops from Primal Aspids in the snow biome at night.", () => !CalRemixWorld.aspids));
                }
                if (recipe.HasResult(ItemType<EyeofDesolation>()))
                {
                    recipe.AddCondition(new Condition("Locked recipe. Drops from Clamitas in the Brimstone Crags.", () => !CalRemixWorld.clamitas));
                }
                if (recipe.HasResult(ItemType<GalacticaSingularity>()))
                {
                    recipe.AddCondition(new Condition("Locked recipe. Fish Side Gars from Godseeker Mode space.", () => !CalRemixWorld.sidegar));
                }
                if (recipe.HasResult(ItemType<FearmongerGreathelm>()))
                {
                    recipe.AddCondition(new Condition("Locked recipe. Obtain by making an enemy walk on Grimesand.", () => !CalRemixWorld.fearmonger));
                    recipe.DisableDecraft();
                }
                if (recipe.HasResult(ItemType<FearmongerPlateMail>()))
                {
                    recipe.AddCondition(new Condition("Locked recipe. Obtain by making an enemy walk on Grimesand.", () => !CalRemixWorld.fearmonger));
                    recipe.DisableDecraft();
                }
                if (recipe.HasResult(ItemType<FearmongerGreaves>()))
                {
                    recipe.AddCondition(new Condition("Locked recipe. Obtain by making an enemy walk on Grimesand.", () => !CalRemixWorld.fearmonger));
                    recipe.DisableDecraft();
                }
                if (recipe.HasResult(ItemType<Seafood>()))
                {
                    recipe.AddCondition(new Condition("Locked recipe. Make the other Seafood.", () => !CalRemixWorld.seafood));
                }
                if (recipe.HasResult(ItemType<StratusBricks>()) && recipe.HasIngredient(ItemType<RuinousSoul>()))
                {
                    recipe.DisableDecraft();
                }
                if (recipe.HasIngredient(ItemType<StratusBricks>()) && !recipe.HasResult(ItemType<StratusPlatform>()))
                {
                    recipe.DisableDecraft();
                }
                if (recipe.HasResult(ItemType<GravityNormalizerPotion>()))
                {
                    recipe.DisableDecraft();
                }
                if (recipe.HasResult(ItemType<AstralInjection>()))
                {
                    recipe.DisableDecraft();
                }
                #region Accessory edits
                if (recipe.HasResult(ItemType<GrandGelatin>()))
                {
                    recipe.AddIngredient<MirageJellyItem>();
                }
                if (recipe.HasResult(ItemType<TheAbsorber>()))
                {
                    recipe.AddIngredient<Regenator>();
                }
                if (recipe.HasResult(ItemType<TheSponge>()))
                {
                    recipe.AddIngredient<TheAbsorber>();
                    recipe.AddIngredient<AquaticHeart>();
                    recipe.AddIngredient<FlameLickedShell>();
                    recipe.AddIngredient<TrinketofChi>();
                    recipe.AddIngredient<AmidiasSpark>();
                    recipe.AddIngredient<UrsaSergeant>();
                    recipe.AddIngredient<PermafrostsConcoction>();
                }
                if (recipe.HasResult(ItemType<RampartofDeities>()))
                {
                    recipe.AddIngredient<RustyMedallion>();
                    recipe.AddIngredient<AmidiasPendant>();
                }
                if (recipe.HasResult(ItemType<TracersElysian>()))
                {
                    recipe.AddIngredient<GravistarSabaton>();
                    recipe.AddIngredient<Microxodonta>();
                }
                if (recipe.HasResult(ItemType<AmbrosialAmpoule>()))
                {
                    recipe.AddIngredient<ArchaicPowder>();
                    recipe.AddIngredient<HoneyDew>();
                }
                if (recipe.HasResult(ItemType<AbyssalDivingGear>()))
                {
                    recipe.AddIngredient<OceanCrest>();
                }
                if (recipe.HasResult(ItemType<AbyssalDivingSuit>()))
                {
                    recipe.AddIngredient<AquaticEmblem>();
                    recipe.AddIngredient<SpelunkersAmulet>();
                    recipe.AddIngredient<AlluringBait>();
                    recipe.AddIngredient<LumenousAmulet>();
                }
                if (recipe.HasResult(ItemType<TheAmalgam>()))
                {
                    recipe.AddIngredient<GiantPearl>();
                    recipe.AddIngredient<ManaPolarizer>();
                    recipe.AddIngredient<FrostFlare>();
                    recipe.AddIngredient<CorrosiveSpine>();
                    recipe.AddIngredient<VoidofExtinction>();
                    recipe.AddIngredient<LeviathanAmbergris>();
                    recipe.AddIngredient(ItemID.SporeSac);
                    recipe.AddIngredient<TheCamper>();
                    recipe.AddIngredient<AlchemicalFlask>();
                    recipe.AddIngredient<ToxicHeart>();
                    recipe.AddIngredient<Radiance>();
                    recipe.AddIngredient<DynamoStemCells>();
                    recipe.AddIngredient<BlazingCore>();
                    recipe.AddIngredient<TheEvolution>();
                    recipe.AddIngredient<Affliction>();
                    recipe.AddIngredient<OldDukeScales>();
                }
                if (recipe.HasResult(ItemType<PhantomicArtifact>()))
                {
                    recipe.RemoveIngredient(ItemType<HallowedRune>());
                    recipe.RemoveIngredient(ItemType<RuinousSoul>());
                    recipe.RemoveIngredient(ItemType<BloodOrb>());
                    recipe.RemoveIngredient(ItemType<ExodiumCluster>());
                    recipe.RemoveTile(TileID.LunarCraftingStation);
                    recipe.AddIngredient(ItemType<CalamityMod.Items.Placeables.Plates.Navyplate>(), 25);
                    recipe.AddIngredient(ItemType<RuinousSoul>(), 5);
                    recipe.AddIngredient(ItemType<ExodiumCluster>(), 25);
                    recipe.AddTile(TileID.DemonAltar);
                }
                #endregion
                #region Babil
                if (recipe.HasResult(ItemType<StormfrontRazor>()))
                {
                    recipe.AddIngredient(ItemType<EssenceofBabil>(), 4);
                }
                if (recipe.HasResult(ItemType<CoreofEleum>()) || recipe.HasResult(ItemType<CoreofHavoc>()) || recipe.HasResult(ItemType<CoreofSunlight>()))
                {
                    recipe.RemoveIngredient(ItemID.Ectoplasm);
                    recipe.AddIngredient(ItemID.HallowedBar);
                }
                if (recipe.HasResult(ItemType<CoreofCalamity>()))
                {
                    recipe.RemoveIngredient(ItemType<AshesofCalamity>());
                    recipe.AddIngredient(ItemType<CoreofBabil>(), 3);
                    recipe.AddIngredient(ItemType<UnholyCore>());
                }
                if (recipe.HasResult(ItemType<AngelicShotgun>()))
                {
                    recipe.RemoveIngredient(ItemType<CoreofSunlight>());
                    recipe.AddIngredient(ItemType<CoreofBabil>(), 7);
                }
                if (recipe.HasResult(ItemType<TwistingThunder>()))
                {
                    recipe.RemoveIngredient(ItemType<CoreofSunlight>());
                    recipe.AddIngredient(ItemType<CoreofBabil>(), 5);
                }
                if (recipe.HasResult(ItemType<EmpyreanMask>()))
                {
                    recipe.AddIngredient(ItemType<CoreofBabil>(), 2);
                }
                if (recipe.HasResult(ItemType<EmpyreanCloak>()))
                {
                    recipe.AddIngredient(ItemType<CoreofBabil>(), 5);
                }
                if (recipe.HasResult(ItemType<EmpyreanCuisses>()))
                {
                    recipe.AddIngredient(ItemType<CoreofBabil>(), 3);
                }
                #endregion
                #region Coyote Venom
                if (recipe.HasResult(ItemType<PlaguebringerPistons>()) || recipe.HasResult(ItemType<PlaguebringerVisor>()))
                {
                    recipe.RemoveIngredient(ItemType<PlagueCellCanister>());
                }
                if (recipe.HasResult(ItemType<PlaguebringerCarapace>()))
                {
                    recipe.RemoveIngredient(ItemType<PlagueCellCanister>());
                }
                #endregion
                #region Alloy Bar Recipes
                if (recipe.HasResult(ItemID.NightsEdge))
                {
                    recipe.AddIngredient(ItemType<TaintedBlade>());
                    recipe.RemoveIngredient(ItemType<PurifiedGel>());
                }
                if (recipe.HasResult(ItemType<DefiledGreatsword>()))
                {
                    recipe.RemoveIngredient(ItemType<UelibloomBar>());
                    recipe.AddIngredient(ItemType<DeliciousMeat>(), 800);
                }
                if (recipe.HasResult(ItemType<FracturedArk>()) && recipe.HasIngredient(ItemID.Terragrim))
                {
                    recipe.DisableRecipe();
                }
                if (recipe.HasResult(ItemType<FracturedArk>()) && !recipe.HasIngredient(ItemID.Terragrim))
                {
                    recipe.RemoveRecipeGroup(RecipeGroup.recipeGroups[RecipeGroup.recipeGroupIDs["AnyCopperBar"]].RegisteredId);
                    if (recipe.HasIngredient(ItemID.CopperBar))
                        recipe.RemoveIngredient(ItemID.CopperBar);
                    if (recipe.HasIngredient(ItemID.TinBar))
                        recipe.RemoveIngredient(ItemID.TinBar);
                    recipe.RemoveIngredient(ItemType<PurifiedGel>());
                    recipe.AddIngredient(ItemID.Starfury);
                    recipe.AddIngredient(ItemID.Gel, 5);
                    recipe.AddIngredient(ItemID.Diamond, 10);
                }
                if (recipe.HasResult(ItemID.MoneyTrough))
                {
                    recipe.RemoveIngredient(ItemID.PiggyBank);
                    recipe.RemoveIngredient(ItemID.GoldCoin);
                    recipe.RemoveIngredient(ItemID.Feather);
                    recipe.RemoveIngredient(ItemType<BloodOrb>());
                    recipe.AddIngredient(ItemType<DeliciousMeat>(), 100);
                }
                #endregion
                #region Essences
                // Zot
                if (recipe.HasResult(ItemType<AstrealDefeat>()))
                {
                    recipe.AddIngredient(ItemType<ContinentalGreatbow>());
                    recipe.AddIngredient(ItemType<DarkechoGreatbow>());
                    recipe.AddIngredient(ItemType<BladedgeRailbow>());
                    recipe.RemoveIngredient(ItemType<AshesofCalamity>());
                }
                //Law
                if (recipe.HasResult(ItemType<SpiritGlyph>()))
                {
                    recipe.RemoveIngredient(ItemID.Diamond);
                    recipe.RemoveIngredient(ItemID.Obsidian);
                    recipe.AddRecipeGroup(RecipeGroupID.IronBar, 15);
                }
                // Myst
                if (recipe.HasResult(ItemType<EpidemicShredder>()))
                {
                    recipe.RemoveIngredient(ItemID.Nanites);
                }
                if (recipe.HasResult(ItemType<PlantationStaff>()))
                {
                    recipe.RemoveIngredient(ItemType<EyeOfNight>());
                }
                if (recipe.HasResult(ItemType<PlagueReaperMask>()))
                {
                    recipe.RemoveIngredient(ItemID.Nanites);
                }
                if (recipe.HasResult(ItemType<PlagueReaperStriders>()))
                {
                    recipe.RemoveIngredient(ItemID.Nanites);
                }
                if (recipe.HasResult(ItemType<PlagueReaperVest>()))
                {
                    recipe.RemoveIngredient(ItemID.Nanites);
                }
                #endregion
                #region Delicious Meat
                if (recipe.HasResult(ItemType<ZenPotion>()))
                {
                    recipe.AddIngredient(ItemType<DeliciousMeat>(), 256);
                }
                if (recipe.HasResult(ItemType<ZergPotion>()))
                {
                    recipe.AddIngredient(ItemType<DeliciousMeat>(), 256);
                }
                if (recipe.HasResult(ItemType<SupremeHealingPotion>()))
                {
                    recipe.AddIngredient(ItemType<DeliciousMeat>(), 8192);
                }
                if (recipe.HasResult(ItemType<DraedonsForge>()))
                {
                    recipe.AddIngredient(ItemType<DeliciousMeat>(), 65536);
                }
                if (recipe.HasResult(ItemType<Abaddon>()))
                {
                    recipe.AddIngredient(ItemType<DeliciousMeat>(), 100);
                }
                if (recipe.HasResult(ItemType<RuinMedallion>()))
                {
                    recipe.AddIngredient(ItemType<DeliciousMeat>(), 50);
                }
                #endregion
                if (recipe.HasResult(ItemType<PrismaticHelmet>()))
                {
                    recipe.AddIngredient(ItemType<OrnateCloth>(), 8);
                }
                if (recipe.HasResult(ItemType<PrismaticRegalia>()))
                {
                    recipe.AddIngredient(ItemType<OrnateCloth>(), 8);
                }
                if (recipe.HasResult(ItemType<PrismaticGreaves>()))
                {
                    recipe.AddIngredient(ItemType<OrnateCloth>(), 8);
                }
                if (recipe.HasResult(ItemType<SilvaArmor>()))
                {
                    recipe.AddIngredient(ItemType<OrnateCloth>(), 12);
                }
                if (recipe.HasResult(ItemType<SilvaLeggings>()))
                {
                    recipe.AddIngredient(ItemType<OrnateCloth>(), 12);
                }
                if (recipe.HasResult(ItemType<SilvaHeadMagic>()))
                {
                    recipe.AddIngredient(ItemType<OrnateCloth>(), 12);
                }
                if (recipe.HasResult(ItemType<SilvaHeadSummon>()))
                {
                    recipe.AddIngredient(ItemType<OrnateCloth>(), 12);
                }
                if (recipe.HasResult(ItemType<SilvaWings>()))
                {
                    recipe.AddIngredient(ItemType<OrnateCloth>(), 12);
                }
                if (recipe.HasResult(ItemType<ClaretCannon>()))
                {
                    recipe.AddIngredient(ItemType<UnholyBloodCells>(), 25);
                }
                if (recipe.HasResult(ItemType<MidnightSunBeacon>()))
                {
                    recipe.AddIngredient(ItemType<CosmiliteSlag>(), 10);
                    recipe.AddIngredient(ItemType<UnholyEssence>(), 10);
                }
                if (recipe.HasResult(ItemType<Fabstaff>()))
                {
                    recipe.RemoveIngredient(ItemID.RainbowRod);
                    recipe.RemoveIngredient(ItemType<Necroplasm>());
                    recipe.AddIngredient(ItemType<BucketofCoal>());
                    recipe.AddIngredient(ItemID.MartianConduitPlating, 1000);
                }
                if (!recipe.HasResult(ItemType<HauntedBar>()) && recipe.TryGetIngredient(ItemType<RuinousSoul>(), out Item rSoul))
                {
                    recipe.RemoveIngredient(ItemType<RuinousSoul>());
                    recipe.AddIngredient(ItemType<HauntedBar>(), rSoul.stack);
                }
                if (!recipe.HasResult(ItemType<ElementalBar>()) && recipe.TryGetIngredient(ItemType<GalacticaSingularity>(), out Item ing))
                {
                    if (ing.stack % 5 == 0 && ing.stack > 1)
                    {
                        recipe.RemoveIngredient(ItemType<GalacticaSingularity>());
                        recipe.AddIngredient(ItemType<ElementalBar>(), ing.stack);
                    }
                }
            }
            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe recipe = Main.recipe[i];
                if (recipe.HasIngredient<CosmiliteBar>() && !recipe.HasIngredient<NightmareFuel>() && !recipe.HasIngredient<DarksunFragment>() && !recipe.HasIngredient<AscendantSpiritEssence>() && !recipe.HasIngredient<EndothermicEnergy>())
                {
                    if (recipe.createItem.damage > 1)
                    {
                        CalRemixItem.cosmicItems.Add(recipe.createItem.type);
                    }
                }
            }

            string wiz = NPCShopDatabase.GetShopNameFromVanillaIndex(7); // wizard index
            NPCShopDatabase.TryGetNPCShop(wiz, out AbstractNPCShop shope);
            NPCShop shopee = shope as NPCShop;

            NPCShop npcShop = new NPCShop(NPCType<IRON>(), "Surge");
            foreach (NPCShop.Entry entry in shopee.Entries)
            {
                if (entry.Item.type != ItemID.Harp)
                    npcShop.Add(entry);
            }
            npcShop.Register();
        }

        public static void MassModifyIngredient(bool condition, List<(int, int, int)> results)
        {
            if (condition)
            {
                MassRemoveIngredient(results);
            }
            else
            {
                MassAddIngredient(results);
            }
            Recipe.UpdateWhichItemsAreMaterials();
            Recipe.UpdateWhichItemsAreCrafted();
            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                foreach (Item item in Main.recipe[i].requiredItem)
                {
                    item.material = ItemID.Sets.IsAMaterial[item.type];
                }
                Main.recipe[i].createItem.material = ItemID.Sets.IsAMaterial[Main.recipe[i].createItem.type];
            }
            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe recipe = Main.recipe[i];
                if (recipe.acceptedGroups.Count <= 0)
                {
                    continue;
                }
                List<int> toRemove = new List<int>();
                foreach (int num in recipe.acceptedGroups)
                {
                    if (!RecipeGroup.recipeGroups[num].ValidItems.Intersect(recipe.requiredItem.Select((Item x) => x.type)).Any())
                    {
                        toRemove.Add(num);
                    }
                }
                foreach (int group in toRemove)
                {
                    recipe.acceptedGroups.Remove(group);
                }
            }
            MethodInfo info = typeof(Recipe).GetMethod("CreateRequiredItemQuickLookups", BindingFlags.Static | BindingFlags.NonPublic);
            info.Invoke(null, null); // FUCK YOU
            Terraria.GameContent.ShimmerTransforms.UpdateRecipeSets();
        }

        public static void MassRemoveIngredient(List<(int, int, int)> results)
        {
            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe recipe = Main.recipe[i];
                for (int j = 0; j < results.Count; j++)
                {
                    if (recipe.HasResult(results[j].Item1) && recipe.HasIngredient(results[j].Item2))
                    {
                        recipe.RemoveIngredient(results[j].Item2);
                    }
                    // you get special treatment
                    if (results == alloyBarCrafts)
                    {
                        if (recipe.HasIngredient(ItemType<AlloyBar>()))
                        {
                            if (recipe.HasIngredient(ItemType<CalamityMod.Items.Placeables.FurnitureStatigel.StatigelBlock>()) || recipe.HasResult(ItemType<CalamityMod.Items.Placeables.FurnitureStatigel.StatigelBlock>()))
                            {
                                recipe.RemoveIngredient(ItemType<AlloyBar>());
                            }
                        }
                    }
                }
            }
        }

        public static void MassAddIngredient(List<(int, int, int)> itemList)
        {
            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe recipe = Main.recipe[i];
                for (int j = 0; j < itemList.Count; j++)
                {
                    if (recipe.HasResult(itemList[j].Item1) && !recipe.HasIngredient(itemList[j].Item2))
                    {
                        recipe.AddIngredient(itemList[j].Item2, itemList[j].Item3);
                    }
                }
                // you get special treatment
                if (itemList == alloyBarCrafts)
                {
                    if (!recipe.HasIngredient(ItemType<AlloyBar>()))
                    {
                        if (recipe.HasIngredient(ItemType<CalamityMod.Items.Placeables.FurnitureStatigel.StatigelBlock>()) || recipe.HasResult(ItemType<CalamityMod.Items.Placeables.FurnitureStatigel.StatigelBlock>()))
                        {
                            recipe.AddIngredient(ItemType<AlloyBar>(), 5);
                        }
                    }
                }
            }
        }

        public void AlcoholRecipe(int result, int drinkingredient, int midgredient, int lastgredient, int blorbcount, int midnum = 5)
        {
            int lastnum = 1;
            if (lastgredient == ItemType<HallowedOre>())
            {
                lastnum = 5;
            }
            Recipe norm = Recipe.Create(result, 5);
            norm.AddIngredient(drinkingredient);
            norm.AddIngredient(midgredient, midnum);
            norm.AddIngredient(lastgredient, lastnum);
            norm.AddTile(TileID.Kegs);
            norm.Register();
            Recipe blood = Recipe.Create(result, 5);
            blood.AddIngredient(drinkingredient);
            blood.AddIngredient(ItemType<BloodOrb>(), blorbcount);
            blood.AddIngredient(lastgredient, lastnum);
            blood.AddTile(TileID.AlchemyTable);
            blood.Register();
        }
        public void CandleRecipe(int result, int soul, int soulnum, int essence, int essencenum)
        {
            Recipe recipe = Recipe.Create(result);
            recipe.AddIngredient(ItemID.PeaceCandle);
            recipe.AddIngredient(essence, essencenum);
            recipe.AddIngredient(soul, soulnum);
            recipe.AddTile(TileID.CrystalBall);
            recipe.Register();
        }
        public void CosmichidGroup(RecipeGroup group, string name, int herb)
        {
            group = new RecipeGroup(() => $"{Lang.GetItemNameValue(herb)} or Cosmichid",
            herb, ItemType<Cosmichid>());
            RecipeGroup.RegisterGroup("CalRemix:" + name, group);
        }
        public void CosmichidChange(Recipe recipe, string group, int herb)
        {
            if (recipe.TryGetIngredient(herb, out Item item))
            {
                recipe.AddRecipeGroup("CalRemix:" + group, item.stack);
                recipe.RemoveIngredient(item);
            }
        }
    }
}
