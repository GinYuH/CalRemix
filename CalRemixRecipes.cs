using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Items;
using CalRemix.Items.Accessories;
using CalamityMod.Items.Armor.Empyrean;
using CalamityMod.Items.Armor.Plaguebringer;
using CalamityMod.Items.Placeables.Furniture;
using CalamityMod.Items.Fishing.AstralCatches;
using CalRemix.Items.Potions;
using CalamityMod.Items.Potions;
using CalamityMod.Items.Placeables.Ores;
using CalRemix.Items.Materials;
using CalRemix.Items.Placeables;
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
using CalRemix.Items.Weapons;
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

namespace CalRemix
{
    public partial class Recipes : ModSystem
    {
        public static RecipeGroup Blinkchid, Daychid, Moonchid, Deathchid, Waterchid, Firechid, Shiverchid;
        public static RecipeGroup GreaterEvil, EvilBar, T4Bar, HMT1Bar;
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

            GreaterEvil = new RecipeGroup(() => "Any Greater Evil Flesh", ModContent.ItemType<RottenMatter>(), ModContent.ItemType<BloodSample>());
            RecipeGroup.RegisterGroup("CalRemix:GreaterEvil", GreaterEvil);
            EvilBar = new RecipeGroup(() => "Any Evil Bar", ItemID.DemoniteBar, ItemID.CrimtaneBar);
            RecipeGroup.RegisterGroup("CalRemix:EvilBar", EvilBar);
            T4Bar = new RecipeGroup(() => "Any Tier 4 Bar", ItemID.GoldBar, ItemID.PlatinumBar);
            RecipeGroup.RegisterGroup("CalRemix:T4Bar", T4Bar);
            HMT1Bar = new RecipeGroup(() => "Any Tier 1 Hardmode Bar", ItemID.CobaltBar, ItemID.PalladiumBar);
            RecipeGroup.RegisterGroup("CalRemix:HMT1Bar", HMT1Bar);
        }
        public override void AddRecipes()
        {
            {
                Recipe slumbering = Recipe.Create(ModContent.ItemType<LoreAwakening>());
                slumbering.AddIngredient<Slumbering>()
                .Register();
            }
            {
                Recipe alloy = Recipe.Create(ModContent.ItemType<LifeAlloy>());
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
                Recipe cell = Recipe.Create(ModContent.ItemType<PlagueCellCanister>(), 1);
                cell.AddRecipeGroup(RecipeGroupID.IronBar);
                cell.AddCondition(new Condition("While the Anomaly 109 \'coyote_venom\' setting is enabled", () => CalRemixWorld.wolfvenom));
                cell.Register();
            }
            {
                Recipe cell = Recipe.Create(ModContent.ItemType<DubiousPlating>(), 4);
                cell.AddIngredient(ModContent.ItemType<ArsenicOre>(), 8);
                cell.AddIngredient(ItemID.Bone);
                cell.AddRecipeGroup("AnyGoldBar");
                cell.AddTile(TileID.Furnaces);
                cell.Register();
            }
            {
                Recipe bar = Recipe.Create(ModContent.ItemType<CosmiliteBar>(), 1);
                bar.AddIngredient<CosmiliteSlag>(5);
                bar.AddTile(TileID.LunarCraftingStation)
                .AddCondition(new Condition("While the Anomaly 109 \'cosmilite_slag\' setting is enabled", () => CalRemixWorld.cosmislag));
                bar.Register();
            }
            {
                Recipe bar = Recipe.Create(ModContent.ItemType<MidasPrime>(), 1);
                bar.AddIngredient<MinnowsPrimeItem>();
                bar.AddTile(TileID.CookingPots);
                bar.Register();
            }
            {
                Recipe halibut = Recipe.Create(ModContent.ItemType<HalibutCannon>());
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
                Recipe ash = Recipe.Create(ModContent.ItemType<CirrusDress>(), 1);
                ash.AddIngredient(ItemID.Silk, 6);
                ash.AddIngredient(ItemID.AshBlock, 10);
                ash.AddTile(TileID.Loom);
                ash.Register();
            }
            {
                Recipe recipe = Recipe.Create(ModContent.ItemType<CryoKey>(), 1)
                .AddIngredient(ItemID.ShadowKey)
                .AddIngredient(ModContent.ItemType<CryoKeyMold>())
                .AddIngredient(ItemID.SoulofLight, 5)
                .AddIngredient(ItemID.SoulofNight, 5)
                .AddIngredient(ItemID.SoulofFlight, 5)
                .AddCondition(new Condition("While the Anomaly 109 \'primal_aspids\' setting is enabled", () => CalRemixWorld.aspids))
                .Register();
            }

            #region DP stuff
            // Alcohol...
            AlcoholRecipe(ModContent.ItemType<CaribbeanRum>(), ModContent.ItemType<Vodka>(), ItemID.BambooBlock, ModContent.ItemType<LivingShard>(), 20, 20);
            AlcoholRecipe(ModContent.ItemType<CinnamonRoll>(), ModContent.ItemType<Everclear>(), ModContent.ItemType<Whiskey>(), ItemID.BeetleHusk, 5, 1);
            AlcoholRecipe(ModContent.ItemType<Everclear>(), ModContent.ItemType<Margarita>(), ItemID.Hay, ModContent.ItemType<AureusCell>(), 10, 1);
            AlcoholRecipe(ModContent.ItemType<EvergreenGin>(), ModContent.ItemType<Vodka>(), ItemID.PineTreeBlock, ModContent.ItemType<LivingShard>(), 20, 2);
            AlcoholRecipe(ModContent.ItemType<Fireball>(), ItemID.Ale, ModContent.ItemType<BloodOrange>(), ItemID.UnicornHorn, 40, 1);
            AlcoholRecipe(ModContent.ItemType<GrapeBeer>(), ItemID.Ale, ItemID.GrapeJuice, ItemID.UnicornHorn, 40, 1);
            AlcoholRecipe(ModContent.ItemType<Margarita>(), ModContent.ItemType<Vodka>(), ItemID.Starfruit, ModContent.ItemType<LivingShard>(), 20, 1);
            AlcoholRecipe(ModContent.ItemType<Moonshine>(), ModContent.ItemType<Everclear>(), ItemID.Ale, ItemID.BeetleHusk, 5, 1);
            AlcoholRecipe(ModContent.ItemType<MoscowMule>(), ModContent.ItemType<Everclear>(), ModContent.ItemType<Vodka>(), ItemID.BeetleHusk, 5, 1);
            AlcoholRecipe(ModContent.ItemType<RedWine>(), ItemID.Ale, ItemID.Grapes, ItemID.UnicornHorn, 40, 1);
            AlcoholRecipe(ModContent.ItemType<Rum>(), ItemID.Ale, ItemID.BambooBlock, ItemID.UnicornHorn, 40, 20);
            AlcoholRecipe(ModContent.ItemType<Screwdriver>(), ModContent.ItemType<NotFabsolVodka>(), ModContent.ItemType<BloodOrange>(), ModContent.ItemType<HallowedOre>(), 30, 1);
            AlcoholRecipe(ModContent.ItemType<StarBeamRye>(), ModContent.ItemType<Margarita>(), ItemID.Hay, ModContent.ItemType<AureusCell>(), 10, 20);
            AlcoholRecipe(ModContent.ItemType<Tequila>(), ItemID.Ale, ItemID.Hay, ItemID.UnicornHorn, 40, 20);
            AlcoholRecipe(ModContent.ItemType<TequilaSunrise>(), ModContent.ItemType<Everclear>(), ModContent.ItemType<Tequila>(), ItemID.BeetleHusk, 5, 1);
            AlcoholRecipe(ModContent.ItemType<Vodka>(), ModContent.ItemType<NotFabsolVodka>(), ItemID.Hay, ModContent.ItemType<HallowedOre>(), 30, 40);
            AlcoholRecipe(ModContent.ItemType<Whiskey>(), ItemID.Ale, ItemID.BottledWater, ItemID.UnicornHorn, 40, 1);
            AlcoholRecipe(ModContent.ItemType<WhiteWine>(), ModContent.ItemType<NotFabsolVodka>(), ItemID.Grapes, ModContent.ItemType<HallowedOre>(), 30, 1);
            // Candles
            CandleRecipe(ModContent.ItemType<ResilientCandle>(), ItemID.SoulofNight, 445, ModContent.ItemType<EssenceofBabil>(), 444);
            CandleRecipe(ModContent.ItemType<SpitefulCandle>(), ModContent.ItemType<EssenceofSunlight>(), 1098, ModContent.ItemType<EssenceofHavoc>(), 987);
            CandleRecipe(ModContent.ItemType<VigorousCandle>(), ItemID.SoulofLight, 277, ModContent.ItemType<EssenceofSunlight>(), 128);
            CandleRecipe(ModContent.ItemType<WeightlessCandle>(), ItemID.SoulofFlight, 3422, ModContent.ItemType<EssenceofEleum>(), 357);
            // Bloody Mary exception
            {
                Recipe recipe = Recipe.Create(ModContent.ItemType<BloodyMary>(), 5);
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

                if (recipe.HasResult(ModContent.ItemType<FabsolsVodka>()))
                {
                    recipe.DisableRecipe();
                }
                if (recipe.HasIngredient(ModContent.ItemType<GrandScale>()))
                {
                    recipe.DisableRecipe();
                }
                if (recipe.HasResult(ModContent.ItemType<ShadowspecBar>()))
                {
                    recipe.AddIngredient<SubnauticalPlate>();
                }
                if (recipe.HasResult(ModContent.ItemType<AstralChunk>()))
                {
                    recipe.RemoveIngredient(ItemID.FallenStar);
                    recipe.AddRecipeGroup(RecipeGroupID.IronBar, 8);
                }
                if (recipe.HasResult(ModContent.ItemType<ProfanedShard>()))
                {
                    recipe.AddCondition(new Condition("Locked recipe. Drops from Yggdrasil Ents in Hallow and Hell.", () => false));
                }
                if (recipe.HasResult(ModContent.ItemType<ExoticPheromones>()))
                {
                    recipe.RemoveIngredient(ModContent.ItemType<LifeAlloy>());
                    recipe.RemoveIngredient(ItemID.FragmentSolar);
                    recipe.RemoveTile(TileID.LunarCraftingStation);
                    recipe.AddRecipeGroup("CalRemix:EvilBar", 15);
                    recipe.AddRecipeGroup("CalRemix:T4Bar", 10);
                    recipe.AddIngredient(ItemID.Feather, 7);
                    recipe.AddTile(TileID.MythrilAnvil);
                }
                if (recipe.HasResult(ModContent.ItemType<AcesHigh>()))
                {
                    recipe.AddIngredient<AcesLow>();
                }
                if (recipe.HasResult(ModContent.ItemType<CosmicImmaterializer>()))
                {
                    recipe.RemoveIngredient(ModContent.ItemType<SanctifiedSpark>());
                    recipe.AddIngredient(ModContent.ItemType<DarkEnergyStaff>());
                }
                if (recipe.HasResult(ModContent.ItemType<Supernova>()))
                {
                    recipe.RemoveIngredient(ModContent.ItemType<SealedSingularity>());
                    recipe.AddIngredient(ModContent.ItemType<UnsealedSingularity>());
                    recipe.AddIngredient(ModContent.ItemType<ProfanedNucleus>());
                }
                if (recipe.HasResult(ModContent.ItemType<Apotheosis>()))
                {
                    recipe.RemoveIngredient(ItemID.SpellTome);
                    recipe.AddIngredient(ModContent.ItemType<WrathoftheCosmos>());
                }
                if (recipe.HasResult(ModContent.ItemType<Voidragon>()))
                {
                    recipe.RemoveIngredient(ModContent.ItemType<Seadragon>());
                    recipe.AddIngredient(ModContent.ItemType<Megaskeet>());
                }
                bool shard = (recipe.HasResult(ModContent.ItemType<SeaRemains>()) || recipe.HasResult(ModContent.ItemType<MonstrousKnives>()) || recipe.HasResult(ModContent.ItemType<FirestormCannon>()) );
                if (recipe.HasIngredient(ModContent.ItemType<PearlShard>()) && shard)
		        {
                    recipe.RemoveIngredient(ModContent.ItemType<PearlShard>());
                    recipe.AddIngredient(ModContent.ItemType<ParchedScale>());
                }
                bool shard2 = recipe.HasResult(ModContent.ItemType<NavyFishingRod>()) || recipe.HasResult(ModContent.ItemType<EutrophicShelf>()) || recipe.HasResult(ModContent.ItemType<AquamarineStaff>()) || recipe.HasResult(ModContent.ItemType<Riptide>()) || recipe.HasResult(ModContent.ItemType<SeashineSword>()) || recipe.HasResult(ModContent.ItemType<StormSurge>()) || recipe.HasResult(ModContent.ItemType<SeafoamBomb>());
                if (recipe.HasIngredient(ModContent.ItemType<PearlShard>()) && shard2)
                {
                    recipe.RemoveIngredient(ModContent.ItemType<PearlShard>());
                }
                if (recipe.TryGetIngredient(ModContent.ItemType<PearlShard>(), out Item pShard) && !(shard || shard))
                {
                    pShard.type = ModContent.ItemType<ConquestFragment>();
                }
                if (recipe.HasResult(ModContent.ItemType<TearsofHeaven>()))
                {
                    recipe.RemoveIngredient(ItemID.WaterBolt);
                    recipe.RemoveIngredient(ModContent.ItemType<CoreofSunlight>());
                    recipe.AddIngredient(ModContent.ItemType<PlasmaflashBolt>());
                    recipe.AddIngredient(ModContent.ItemType<SaltWaterBolt>());
                }
                if (recipe.HasResult(ModContent.ItemType<Elderberry>()))
                {
                    recipe.AddCondition(new Condition("Locked recipe. Trade with the Dye Trader Post-Providence.", () => !CalRemixWorld.permanenthealth));
                }
                if (recipe.HasResult(ModContent.ItemType<MiracleFruit>()))
                {
                    recipe.AddCondition(new Condition("Locked recipe. Find in post-Golem Jungle Planetoids.", () => !CalRemixWorld.permanenthealth));
                }
                if (recipe.HasResult(ModContent.ItemType<Dragonfruit>()))
                {
                    recipe.AddCondition(new Condition("Locked recipe. Open an Azure Crate while a Wyvern is alive post-Yharon", () => !CalRemixWorld.permanenthealth));
                }
                if (recipe.HasResult(ModContent.ItemType<BloodOrange>()))
                {
                    recipe.AddCondition(new Condition("Locked recipe. Throw an Apple in water during a Blood Moon post-mechs.", () => !CalRemixWorld.permanenthealth));
                }
                if (recipe.HasResult(ModContent.ItemType<CometShard>()))
                {
                    recipe.AddCondition(new Condition("Locked recipe. Get a Nova to collide with Astral Ore.", () => !CalRemixWorld.permanenthealth));
                }
                if (recipe.HasResult(ModContent.ItemType<EtherealCore>()))
                {
                    recipe.AddCondition(new Condition("Locked recipe. Insert a Bloody Vein at the Astral Beacon.", () => !CalRemixWorld.permanenthealth));
                }
                if (recipe.HasResult(ModContent.ItemType<PhantomHeart>()))
                {
                    recipe.AddCondition(new Condition("Locked recipe. Spawns in the Post-Polterghast Dungeon.", () => !CalRemixWorld.permanenthealth));
                }
                if (recipe.HasResult(ModContent.ItemType<DesertMedallion>()))
                {
                    recipe.AddCondition(new Condition("Locked recipe. Drops from Cnidrions after defeating the Wulfrum Excavator.", () => false));
                }
                if (recipe.HasResult(ModContent.ItemType<CryoKey>()))
                {
                    recipe.AddCondition(new Condition("Locked recipe. Drops from Primal Aspids in the snow biome at night.", () => !CalRemixWorld.aspids));
                }
                if (recipe.HasResult(ModContent.ItemType<EyeofDesolation>()))
                {
                    recipe.AddCondition(new Condition("Locked recipe. Drops from Clamitas in the Brimstone Crags.", () => !CalRemixWorld.clamitas));
                }
                if (recipe.HasResult(ModContent.ItemType<GalacticaSingularity>()))
                {
                    recipe.AddCondition(new Condition("Locked recipe. Fish Side Gars from Godseeker Mode space.", () => !CalRemixWorld.sidegar));
                }
                if (recipe.HasResult(ModContent.ItemType<FearmongerGreathelm>()))
                {
                    recipe.AddCondition(new Condition("Locked recipe. Obtain by making an enemy walk on Grimesand.", () => !CalRemixWorld.fearmonger));
                    recipe.DisableDecraft();
                }
                if (recipe.HasResult(ModContent.ItemType<FearmongerPlateMail>()))
                {
                    recipe.AddCondition(new Condition("Locked recipe. Obtain by making an enemy walk on Grimesand.", () => !CalRemixWorld.fearmonger));
                    recipe.DisableDecraft();
                }
                if (recipe.HasResult(ModContent.ItemType<FearmongerGreaves>()))
                {
                    recipe.AddCondition(new Condition("Locked recipe. Obtain by making an enemy walk on Grimesand.", () => !CalRemixWorld.fearmonger));
                    recipe.DisableDecraft();
                }
                if (recipe.HasResult(ModContent.ItemType<Seafood>()))
                {
                    recipe.AddCondition(new Condition("Locked recipe. Make the other Seafood.", () => !CalRemixWorld.seafood));
                }
                if (recipe.HasResult(ModContent.ItemType<StratusBricks>()) && recipe.HasIngredient(ModContent.ItemType<RuinousSoul>()))
                {
                    recipe.DisableDecraft();
                }
                if (recipe.HasIngredient(ModContent.ItemType<StratusBricks>()) && !recipe.HasResult(ModContent.ItemType<StratusPlatform>()))
                {
                    recipe.DisableDecraft();
                }
                #region Accessory edits
                if (recipe.HasResult(ModContent.ItemType<GrandGelatin>()))
                {
                    recipe.AddIngredient<MirageJellyItem>();
                }
                if (recipe.HasResult(ModContent.ItemType<TheAbsorber>()))
                {
                    recipe.AddIngredient<Regenator>();
                }
                if (recipe.HasResult(ModContent.ItemType<TheSponge>()))
                {
                    recipe.AddIngredient<TheAbsorber>();
                    recipe.AddIngredient<AquaticHeart>();
                    recipe.AddIngredient<FlameLickedShell>();
                    recipe.AddIngredient<TrinketofChi>();
                    recipe.AddIngredient<AmidiasSpark>();
                    recipe.AddIngredient<UrsaSergeant>();
                    recipe.AddIngredient<PermafrostsConcoction>();
                }
                if (recipe.HasResult(ModContent.ItemType<RampartofDeities>()))
                {
                    recipe.AddIngredient<RustyMedallion>();
                    recipe.AddIngredient<AmidiasPendant>();
                }
                if (recipe.HasResult(ModContent.ItemType<TracersElysian>()))
                {
                    recipe.AddIngredient<GravistarSabaton>();
                    recipe.AddIngredient<Microxodonta>();
                }
                if (recipe.HasResult(ModContent.ItemType<AmbrosialAmpoule>()))
                {
                    recipe.AddIngredient<ArchaicPowder>();
                    recipe.AddIngredient<HoneyDew>();
                }
                if (recipe.HasResult(ModContent.ItemType<AbyssalDivingGear>()))
                {
                    recipe.AddIngredient<OceanCrest>();
                }
                if (recipe.HasResult(ModContent.ItemType<AbyssalDivingSuit>()))
                {
                    recipe.AddIngredient<AquaticEmblem>();
                    recipe.AddIngredient<SpelunkersAmulet>();
                    recipe.AddIngredient<AlluringBait>();
                    recipe.AddIngredient<LumenousAmulet>();
                }
                if (recipe.HasResult(ModContent.ItemType<TheAmalgam>()))
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
                if (recipe.HasResult(ModContent.ItemType<PhantomicArtifact>()))
                {
                    recipe.RemoveIngredient(ModContent.ItemType<HallowedRune>());
                    recipe.RemoveIngredient(ModContent.ItemType<RuinousSoul>());
                    recipe.RemoveIngredient(ModContent.ItemType<BloodOrb>());
                    recipe.RemoveIngredient(ModContent.ItemType<ExodiumCluster>());
                    recipe.RemoveTile(TileID.LunarCraftingStation);
                    recipe.AddIngredient(ModContent.ItemType<CalamityMod.Items.Placeables.Plates.Navyplate>(), 25);
                    recipe.AddIngredient(ModContent.ItemType<RuinousSoul>(), 5);
                    recipe.AddIngredient(ModContent.ItemType<ExodiumCluster>(), 25);
                    recipe.AddTile(TileID.DemonAltar);
                }
                #endregion
                #region Babil
                if (recipe.HasResult(ModContent.ItemType<StormfrontRazor>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<EssenceofBabil>(), 4);
                }
                if (recipe.HasResult(ModContent.ItemType<CoreofEleum>()) || recipe.HasResult(ModContent.ItemType<CoreofHavoc>()) || recipe.HasResult(ModContent.ItemType<CoreofSunlight>()))
                {
                    recipe.RemoveIngredient(ItemID.Ectoplasm);
                    recipe.AddIngredient(ItemID.HallowedBar);
                }
                if (recipe.HasResult(ModContent.ItemType<CoreofCalamity>()))
                {
                    recipe.RemoveIngredient(ModContent.ItemType<AshesofCalamity>());
                    recipe.AddIngredient(ModContent.ItemType<CoreofBabil>(), 3);
                    recipe.AddIngredient(ModContent.ItemType<UnholyCore>());
                }
                if (recipe.HasResult(ModContent.ItemType<AngelicShotgun>()))
                {
                    recipe.RemoveIngredient(ModContent.ItemType<CoreofSunlight>());
                    recipe.AddIngredient(ModContent.ItemType<CoreofBabil>(), 7);
                }
                if (recipe.HasResult(ModContent.ItemType<TwistingThunder>()))
                {
                    recipe.RemoveIngredient(ModContent.ItemType<CoreofSunlight>());
                    recipe.AddIngredient(ModContent.ItemType<CoreofBabil>(), 5);
                }
                if (recipe.HasResult(ModContent.ItemType<EmpyreanMask>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<CoreofBabil>(), 2);
                }
                if (recipe.HasResult(ModContent.ItemType<EmpyreanCloak>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<CoreofBabil>(), 5);
                }
                if (recipe.HasResult(ModContent.ItemType<EmpyreanCuisses>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<CoreofBabil>(), 3);
                }
                #endregion
                #region Coyote Venom
                if (recipe.HasResult(ModContent.ItemType<PlaguebringerPistons>()) || recipe.HasResult(ModContent.ItemType<PlaguebringerVisor>()))
                {
                    recipe.RemoveIngredient(ModContent.ItemType<PlagueCellCanister>());
                }
                if (recipe.HasResult(ModContent.ItemType<PlaguebringerCarapace>()))
                {
                    recipe.RemoveIngredient(ModContent.ItemType<PlagueCellCanister>());
                }
                #endregion
                #region Alloy Bar Recipes
                if (recipe.HasResult(ItemID.NightsEdge))
                {
                    recipe.AddIngredient(ModContent.ItemType<TaintedBlade>());
                    recipe.RemoveIngredient(ModContent.ItemType<PurifiedGel>());
                }
                if (recipe.HasResult(ModContent.ItemType<DefiledGreatsword>()))
                {
                    recipe.RemoveIngredient(ModContent.ItemType<UelibloomBar>());
                    recipe.AddIngredient(ModContent.ItemType<DeliciousMeat>(), 800);
                }
                if (recipe.HasResult(ModContent.ItemType<FracturedArk>()) && recipe.HasIngredient(ItemID.Terragrim))
                {
                    recipe.DisableRecipe();
                }
                if (recipe.HasResult(ModContent.ItemType<FracturedArk>()) && !recipe.HasIngredient(ItemID.Terragrim))
                {
                    recipe.RemoveRecipeGroup(RecipeGroup.recipeGroups[RecipeGroup.recipeGroupIDs["AnyCopperBar"]].RegisteredId);
                    if (recipe.HasIngredient(ItemID.CopperBar))
                        recipe.RemoveIngredient(ItemID.CopperBar);
                    if (recipe.HasIngredient(ItemID.TinBar))
                        recipe.RemoveIngredient(ItemID.TinBar);
                    recipe.RemoveIngredient(ModContent.ItemType<PurifiedGel>());
                    recipe.AddIngredient(ItemID.Starfury);
                    recipe.AddIngredient(ItemID.Gel, 5);
                    recipe.AddIngredient(ItemID.Diamond, 10);
                }
                if (recipe.HasResult(ItemID.MoneyTrough))
                {
                    recipe.RemoveIngredient(ItemID.PiggyBank);
                    recipe.RemoveIngredient(ItemID.GoldCoin);
                    recipe.RemoveIngredient(ItemID.Feather);
                    recipe.RemoveIngredient(ModContent.ItemType<BloodOrb>());
                    recipe.AddIngredient(ModContent.ItemType<DeliciousMeat>(), 100);
                }
                #endregion
                #region Essences
                // Zot
                if (recipe.HasResult(ModContent.ItemType<AstrealDefeat>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<ContinentalGreatbow>());
                    recipe.AddIngredient(ModContent.ItemType<DarkechoGreatbow>());
                    recipe.AddIngredient(ModContent.ItemType<BladedgeRailbow>());
                    recipe.RemoveIngredient(ModContent.ItemType<AshesofCalamity>());
                }
                //Law
                if (recipe.HasResult(ModContent.ItemType<SpiritGlyph>()))
                {
                    recipe.RemoveIngredient(ItemID.Diamond);
                    recipe.RemoveIngredient(ItemID.Obsidian);
                    recipe.AddRecipeGroup(RecipeGroupID.IronBar, 15);
                }
                // Myst
                if (recipe.HasResult(ModContent.ItemType<EpidemicShredder>()))
                {
                    recipe.RemoveIngredient(ItemID.Nanites);
                }
                if (recipe.HasResult(ModContent.ItemType<PlantationStaff>()))
                {
                    recipe.RemoveIngredient(ModContent.ItemType<EyeOfNight>());
                }
                if (recipe.HasResult(ModContent.ItemType<PlagueReaperMask>()))
                {
                    recipe.RemoveIngredient(ItemID.Nanites);
                }
                if (recipe.HasResult(ModContent.ItemType<PlagueReaperStriders>()))
                {
                    recipe.RemoveIngredient(ItemID.Nanites);
                }
                if (recipe.HasResult(ModContent.ItemType<PlagueReaperVest>()))
                {
                    recipe.RemoveIngredient(ItemID.Nanites);
                }
                #endregion
                #region Delicious Meat
                if (recipe.HasResult(ModContent.ItemType<ZenPotion>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<DeliciousMeat>(), 256);
                }
                if (recipe.HasResult(ModContent.ItemType<ZergPotion>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<DeliciousMeat>(), 256);
                }
                if (recipe.HasResult(ModContent.ItemType<SupremeHealingPotion>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<DeliciousMeat>(), 8192);
                }
                if (recipe.HasResult(ModContent.ItemType<DraedonsForge>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<DeliciousMeat>(), 65536);
                }
                if (recipe.HasResult(ModContent.ItemType<Abaddon>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<DeliciousMeat>(), 100);
                }
                if (recipe.HasResult(ModContent.ItemType<RuinMedallion>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<DeliciousMeat>(), 50);
                }
                #endregion
                if (recipe.HasResult(ModContent.ItemType<PrismaticHelmet>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<OrnateCloth>(), 8);
                }
                if (recipe.HasResult(ModContent.ItemType<PrismaticRegalia>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<OrnateCloth>(), 8);
                }
                if (recipe.HasResult(ModContent.ItemType<PrismaticGreaves>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<OrnateCloth>(), 8);
                }
                if (recipe.HasResult(ModContent.ItemType<SilvaArmor>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<OrnateCloth>(), 12);
                }
                if (recipe.HasResult(ModContent.ItemType<SilvaLeggings>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<OrnateCloth>(), 12);
                }
                if (recipe.HasResult(ModContent.ItemType<SilvaHeadMagic>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<OrnateCloth>(), 12);
                }
                if (recipe.HasResult(ModContent.ItemType<SilvaHeadSummon>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<OrnateCloth>(), 12);
                }
                if (recipe.HasResult(ModContent.ItemType<SilvaWings>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<OrnateCloth>(), 12);
                }
                if (recipe.HasResult(ModContent.ItemType<ClaretCannon>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<UnholyBloodCells>(), 25);
                }
                if (recipe.HasResult(ModContent.ItemType<MidnightSunBeacon>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<CosmiliteSlag>(), 10);
                    recipe.AddIngredient(ModContent.ItemType<UnholyEssence>(), 10);
                }
                if (recipe.HasResult(ModContent.ItemType<Fabstaff>()))
                {
                    recipe.RemoveIngredient(ItemID.RainbowRod);
                    recipe.RemoveIngredient(ModContent.ItemType<Necroplasm>());
                    recipe.AddIngredient(ModContent.ItemType<BucketofCoal>());
                    recipe.AddIngredient(ItemID.MartianConduitPlating, 1000);
                }
                if (!recipe.HasResult(ModContent.ItemType<HauntedBar>()) && recipe.TryGetIngredient(ModContent.ItemType<RuinousSoul>(), out Item rSoul))
                {
                    recipe.RemoveIngredient(ModContent.ItemType<RuinousSoul>());
                    recipe.AddIngredient(ModContent.ItemType<HauntedBar>(), rSoul.stack);
                }
                if (!recipe.HasResult(ModContent.ItemType<ElementalBar>()) && recipe.TryGetIngredient(ModContent.ItemType<GalacticaSingularity>(), out Item ing))
                {
                    if (ing.stack % 5 == 0 && ing.stack > 1)
                    {
                        recipe.RemoveIngredient(ModContent.ItemType<GalacticaSingularity>());
                        recipe.AddIngredient(ModContent.ItemType<ElementalBar>(), ing.stack);
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
                        if (recipe.HasIngredient(ModContent.ItemType<AlloyBar>()))
                        {
                            if (recipe.HasIngredient(ModContent.ItemType<CalamityMod.Items.Placeables.FurnitureStatigel.StatigelBlock>()) || recipe.HasResult(ModContent.ItemType<CalamityMod.Items.Placeables.FurnitureStatigel.StatigelBlock>()))
                            {
                                recipe.RemoveIngredient(ModContent.ItemType<AlloyBar>());
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
                    if (!recipe.HasIngredient(ModContent.ItemType<AlloyBar>()))
                    {
                        if (recipe.HasIngredient(ModContent.ItemType<CalamityMod.Items.Placeables.FurnitureStatigel.StatigelBlock>()) || recipe.HasResult(ModContent.ItemType<CalamityMod.Items.Placeables.FurnitureStatigel.StatigelBlock>()))
                        {
                            recipe.AddIngredient(ModContent.ItemType<AlloyBar>(), 5);
                        }
                    }
                }
            }
        }

        public void AlcoholRecipe(int result, int drinkingredient, int midgredient, int lastgredient, int blorbcount, int midnum = 5)
        {
            int lastnum = 1;
            if (lastgredient == ModContent.ItemType<HallowedOre>())
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
            blood.AddIngredient(ModContent.ItemType<BloodOrb>(), blorbcount);
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
            herb, ModContent.ItemType<Cosmichid>());
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
