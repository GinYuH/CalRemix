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
using CalamityMod.Items.Ammo;
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
using CalamityMod.Items.Weapons.Typeless;
using CalamityMod.Items.Armor.Auric;
using CalRemix.Items.Weapons;
using CalamityMod.Items.Mounts;
using CalamityMod.Items.Weapons.DraedonsArsenal;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Items.Placeables.DraedonStructures;
using CalamityMod.Items.Tools;
using CalamityMod.Items.Armor.Victide;
using CalamityMod.Items.Armor.Statigel;
using CalamityMod.Items.Armor.PlagueReaper;
using CalamityMod.Items.Placeables;
using CalamityMod.Items.DraedonMisc;
using CalamityMod.Items.Tools.ClimateChange;

namespace CalRemix
{
    public class Recipes : ModSystem
    {
        public static RecipeGroup Blinkchid, Daychid, Moonchid, Deathchid, Waterchid, Firechid, Shiverchid;

        public override void Unload()
        {
            Blinkchid = null;
            Daychid = null;
            Moonchid = null;
            Deathchid = null;
            Waterchid = null;
            Firechid = null;
            Shiverchid = null;
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
        }
        public override void AddRecipes() 
        {
            /*{
                Recipe feather = Recipe.Create(ModContent.ItemType<EffulgentFeather>(), 3);
                feather.AddIngredient<DesertFeather>(3)
                .AddIngredient<LifeAlloy>()
                .AddTile(TileID.LunarCraftingStation)
                .Register();
            }*/
            {
                Recipe alloy = Recipe.Create(ModContent.ItemType<LifeAlloy>());
                alloy.AddIngredient<LifeOre>(5)
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
                cell.AddIngredient<CoyoteVenom>(1);
                cell.Register();
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
                /*if (recipe.HasResult(ModContent.ItemType<GrandScale>()))
                {
                    recipe.DisableRecipe();
                }*/
                if (recipe.HasResult(ModContent.ItemType<ShadowspecBar>()))
                {
                    recipe.AddIngredient<SubnauticalPlate>();
                }
                if (recipe.HasResult(ModContent.ItemType<AcesHigh>()))
                {
                    recipe.AddIngredient<AcesLow>();
                }
                if (recipe.HasResult(ModContent.ItemType<CosmicImmaterializer>()))
                {
                    recipe.AddIngredient<DarkEnergyStaff>();
                    recipe.RemoveIngredient(ModContent.ItemType<SanctifiedSpark>());
                }
                if (recipe.HasResult(ModContent.ItemType<Supernova>()))
                {
                    recipe.AddIngredient<UnsealedSingularity>();
                    recipe.AddIngredient<ProfanedNucleus>();
                    recipe.RemoveIngredient(ModContent.ItemType<SealedSingularity>());
                }
                if (recipe.HasResult(ModContent.ItemType<TearsofHeaven>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<AeroBolt>());
                    recipe.AddIngredient(ModContent.ItemType<ThunderBolt>());
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
                if (recipe.HasIngredient(ModContent.ItemType<PearlShard>()) && (recipe.HasResult(ModContent.ItemType<SeaRemains>()) || recipe.HasResult(ModContent.ItemType<MonstrousKnives>()) || recipe.HasResult(ModContent.ItemType<FirestormCannon>()) || recipe.HasResult(ModContent.ItemType<SuperballBullet>())))
		        {
                    recipe.RemoveIngredient(ModContent.ItemType<PearlShard>());
                    recipe.AddIngredient(ModContent.ItemType<ParchedScale>());
                }
                if (recipe.HasIngredient(ModContent.ItemType<PearlShard>()) && recipe.HasResult(ModContent.ItemType<NavyFishingRod>()) || recipe.HasResult(ModContent.ItemType<EutrophicShelf>()) || recipe.HasResult(ModContent.ItemType<AquamarineStaff>()) || recipe.HasResult(ModContent.ItemType<Riptide>()) || recipe.HasResult(ModContent.ItemType<SeashineSword>()) || recipe.HasResult(ModContent.ItemType<StormSurge>()) || recipe.HasResult(ModContent.ItemType<SeafoamBomb>()))
                {
                    recipe.RemoveIngredient(ModContent.ItemType<PearlShard>());
                }
                if (recipe.HasResult(ModContent.ItemType<Elderberry>()))
                {
                    recipe.DisableRecipe();
                }
                if (recipe.HasResult(ModContent.ItemType<MiracleFruit>()))
                {
                    recipe.DisableRecipe();
                }
                if (recipe.HasResult(ModContent.ItemType<Dragonfruit>()))
                {
                    recipe.DisableRecipe();
                }
                if (recipe.HasResult(ModContent.ItemType<BloodOrange>()))
                {
                    recipe.DisableRecipe();
                }
                if (recipe.HasResult(ModContent.ItemType<CometShard>()))
                {
                    recipe.DisableRecipe();
                }
                if (recipe.HasResult(ModContent.ItemType<EtherealCore>()))
                {
                    recipe.DisableRecipe();
                }
                if (recipe.HasResult(ModContent.ItemType<PhantomHeart>()))
                {
                    recipe.DisableRecipe();
                }
                if (recipe.HasResult(ModContent.ItemType<DesertMedallion>()))
                {
                    recipe.DisableRecipe();
                }
                if (recipe.HasResult(ModContent.ItemType<CryoKey>()))
                {
                    recipe.DisableRecipe();
                }
                if (recipe.HasResult(ModContent.ItemType<EyeofDesolation>()))
                {
                    recipe.DisableRecipe();
                }
                /*if (recipe.HasResult(ModContent.ItemType<ExoticPheromones>()))
                {
                    recipe.RemoveIngredient(ModContent.ItemType<LifeAlloy>());
                    recipe.RemoveIngredient(ItemID.FragmentSolar);
                    recipe.RemoveTile(TileID.LunarCraftingStation);
                    recipe.AddIngredient(ModContent.ItemType<UnholyCore>(), 5);
                    recipe.AddIngredient(ItemID.SoulofLight, 5);
                    recipe.AddIngredient(ItemID.SoulofNight, 5);
                    recipe.AddIngredient(ItemID.PinkPricklyPear);
                    recipe.AddTile(TileID.MythrilAnvil);
                }*/
                #region Accessory edits
                if (recipe.HasResult(ModContent.ItemType<GrandGelatin>()))
                {
                    recipe.AddIngredient<MirageJellyItem>();
                    recipe.AddIngredient<AlloyBar>();
                }
                if (recipe.HasResult(ModContent.ItemType<TheAbsorber>()))
                {
                    recipe.AddIngredient<Regenator>();
                    recipe.AddIngredient(ModContent.ItemType<EssentialEssenceBar>(), 40);
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
                    recipe.AddIngredient<EssentialEssenceBar>(40);
                }
                if (recipe.HasResult(ModContent.ItemType<AbyssalDivingGear>()))
                {
                    recipe.AddIngredient<OceanCrest>();
                    recipe.AddIngredient<EssentialEssenceBar>(40);
                }
                if (recipe.HasResult(ModContent.ItemType<AbyssalDivingSuit>()))
                {
                    recipe.AddIngredient<AquaticEmblem>();
                    recipe.AddIngredient<SpelunkersAmulet>();
                    recipe.AddIngredient<AlluringBait>();
                    recipe.AddIngredient<LumenousAmulet>();
                    recipe.AddIngredient<EssentialEssenceBar>(40);
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
                    recipe.AddIngredient<PlagueHive>();
                    recipe.AddIngredient<Purity>();
                    recipe.AddIngredient<DynamoStemCells>();
                    recipe.AddIngredient<BlazingCore>();
                    recipe.AddIngredient<TheEvolution>();
                    recipe.AddIngredient<Affliction>();
                    recipe.AddIngredient<OldDukeScales>();
                }
                if (recipe.HasResult(ModContent.ItemType<PlagueHive>()))
                {
                    recipe.AddIngredient<ToxicHeart>();
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
                #region Yharim Bar Recipes
                if (recipe.HasResult(ModContent.ItemType<AsgardianAegis>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<YharimBar>(), 4);
                }
                if (recipe.HasResult(ModContent.ItemType<AuricBar>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<YharimBar>());
                }
                if (recipe.HasResult(ModContent.ItemType<AuricTeslaBodyArmor>()) || recipe.HasResult(ModContent.ItemType<AuricTeslaCuisses>()) || recipe.HasResult(ModContent.ItemType<AuricTeslaHoodedFacemask>()) || recipe.HasResult(ModContent.ItemType<AuricTeslaPlumedHelm>()) || recipe.HasResult(ModContent.ItemType<AuricTeslaRoyalHelm>()) || recipe.HasResult(ModContent.ItemType<AuricTeslaSpaceHelmet>()) || recipe.HasResult(ModContent.ItemType<AuricTeslaWireHemmedVisage>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<YharimBar>(), 35);
                }
                if (recipe.HasResult(ModContent.ItemType<CoreOfTheBloodGod>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<YharimBar>(), 4);
                }
                if (recipe.HasResult(ModContent.ItemType<EclipseMirror>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<YharimBar>(), 4);
                }
                if (recipe.HasResult(ModContent.ItemType<EcologicalCollapse>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<YharimBar>(), 5);
                }
                if (recipe.HasResult(ModContent.ItemType<EidolonStaff>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<YharimBar>(), 17);
                }
                if (recipe.HasResult(ModContent.ItemType<ElementalQuiver>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<YharimBar>(), 4);
                }
                if (recipe.HasResult(ModContent.ItemType<TracersElysian>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<YharimBar>(), 4);
                }

                if (recipe.HasResult(ModContent.ItemType<GazeOfCrysthamyr>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<YharimBar>(), 50);
                }
                if (recipe.HasResult(ModContent.ItemType<GodSlayerSlug>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<YharimBar>(), 4);
                }
                if (recipe.HasResult(ModContent.ItemType<GrandReef>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<YharimBar>(), 30);
                }
                if (recipe.HasResult(ModContent.ItemType<HadopelagicEcho>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<YharimBar>(), 2);
                }
                if (recipe.HasResult(ModContent.ItemType<HalibutCannon>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<YharimBar>(), 99);
                }
                if (recipe.HasResult(ModContent.ItemType<HolyMantle>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<YharimBar>());
                }

                if (recipe.HasResult(ModContent.ItemType<MagnaCore>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<YharimBar>(), 49);
                }
                if (recipe.HasResult(ModContent.ItemType<Megaskeet>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<YharimBar>(), 99);
                }
                if (recipe.HasResult(ModContent.ItemType<Nanotech>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<YharimBar>(), 4);
                }

                if (recipe.HasResult(ModContent.ItemType<Nucleogenesis>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<YharimBar>(), 4);
                }
                if (recipe.HasResult(ModContent.ItemType<OmegaHealingPotion>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<YharimBar>(), 90);
                }

                if (recipe.HasResult(ModContent.ItemType<PlasmaGrenade>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<YharimBar>());
                }
                if (recipe.HasResult(ModContent.ItemType<QuiverofMadness>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<YharimBar>(), 4);
                }
                if (recipe.HasResult(ModContent.ItemType<RampartofDeities>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<YharimBar>(), 4);
                }
                if (recipe.HasResult(ModContent.ItemType<RoguesLootbox>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<YharimBar>(), 22);
                }
                if (recipe.HasResult(ModContent.ItemType<ScorchedEarth>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<YharimBar>(), 99);
                }
                if (recipe.HasResult(ModContent.ItemType<SearedPan>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<YharimBar>(), 99);
                }
                if (recipe.HasResult(ModContent.ItemType<Slimelgamation>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<YharimBar>(), 4);
                }
                if (recipe.HasResult(ModContent.ItemType<TheAmalgam>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<YharimBar>(), 99);
                }
                if (recipe.HasResult(ModContent.ItemType<TheDevourerofCods>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<YharimBar>(), 10);
                }
                /*
                if (recipe.HasResult(ModContent.ItemType<TheDreamingGhost>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<YharimBar>(), 4);
                }
                */
                if (recipe.HasResult(ModContent.ItemType<TheSponge>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<YharimBar>(), 100);
                }
                if (recipe.HasResult(ModContent.ItemType<ThrowersGauntlet>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<YharimBar>());
                }
                /*
                if (recipe.HasResult(ModContent.ItemType<TyrantShield>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<YharimBar>(), 124);
                }
                */
                if (recipe.HasResult(ModContent.ItemType<UniversalStone>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<YharimBar>(), 100);
                }

                if (recipe.HasResult(ItemID.Zenith))
                {
                    recipe.AddIngredient(ModContent.ItemType<YharimBar>(), 193);
                }

                if (recipe.HasResult(ModContent.ItemType<ZenithArcanum>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<YharimBar>(), 99);
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
                    recipe.AddIngredient(ModContent.ItemType<EssentialEssenceBar>(), 2);
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
                if (recipe.HasResult(ModContent.ItemType<EnhancedNanoRound>()))
                {
                    recipe.RemoveIngredient(ModContent.ItemType<EssenceofEleum>());
                    recipe.AddIngredient(ModContent.ItemType<CoyoteVenom>(), 1);
                }
                if (recipe.HasResult(ModContent.ItemType<StarlightWings>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<CoyoteVenom>(), 1);
                }
                if (recipe.HasResult(ModContent.ItemType<PlaguebringerPistons>()) || recipe.HasResult(ModContent.ItemType<PlaguebringerVisor>()))
                {
                    recipe.RemoveIngredient(ModContent.ItemType<PlagueCellCanister>());
                    recipe.AddIngredient(ModContent.ItemType<CoyoteVenom>(), 1);
                }
                if (recipe.HasResult(ModContent.ItemType<PlaguebringerCarapace>()))
                {
                    recipe.RemoveIngredient(ModContent.ItemType<PlagueCellCanister>());
                    recipe.AddIngredient(ModContent.ItemType<CoyoteVenom>(), 2);
                }
                #endregion
                #region Alloy Bar Recipes
                if (recipe.HasResult(ItemID.NightsEdge))
                {
                    recipe.AddIngredient(ModContent.ItemType<TaintedBlade>());
                    recipe.RemoveIngredient(ModContent.ItemType<PurifiedGel>());
                    recipe.AddIngredient(ModContent.ItemType<AlloyBar>(), 30);
                }
                if (recipe.HasResult(ModContent.ItemType<OverloadedSludge>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<AlloyBar>(), 2);
                }
                if (recipe.HasResult(ModContent.ItemType<BlightedCleaver>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<AlloyBar>());
                }
                if (recipe.HasResult(ModContent.ItemType<DefiledGreatsword>()))
                {
                    recipe.RemoveIngredient(ModContent.ItemType<UelibloomBar>());
                    recipe.AddIngredient(ModContent.ItemType<DeliciousMeat>(), 800);
                    recipe.AddIngredient(ModContent.ItemType<AlloyBar>(), 340);
                }
                if (recipe.HasResult(ModContent.ItemType<Aestheticus>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<AlloyBar>());
                }
                if (recipe.HasResult(ModContent.ItemType<AgedLaboratoryElectricPanelItem>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<AlloyBar>());
                }
                if (recipe.HasResult(ModContent.ItemType<FracturedArk>()))
                {
                    recipe.DisableRecipe();
                    recipe.AddIngredient(ItemID.Starfury);
                    recipe.AddIngredient(ItemID.EnchantedSword);
                    recipe.AddIngredient(ItemID.Gel, 5);
                    recipe.AddIngredient(ItemID.Diamond, 10);
                    recipe.AddIngredient(ModContent.ItemType<AlloyBar>(), 92);
                }
                if (recipe.HasResult(ModContent.ItemType<JellyChargedBattery>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<AlloyBar>(), 3);
                }
                if (recipe.HasResult(ItemID.LifeCrystal))
                {
                    recipe.AddIngredient(ModContent.ItemType<AlloyBar>());
                }
                if (recipe.HasResult(ItemID.MagicMirror))
                {
                    recipe.AddIngredient(ModContent.ItemType<AlloyBar>(), 2);
                }
                if (recipe.HasResult(ItemID.MoneyTrough))
                {
                    recipe.DisableRecipe();
                    recipe.AddIngredient(ModContent.ItemType<DeliciousMeat>(), 100);
                    recipe.AddIngredient(ModContent.ItemType<AlloyBar>(), 2);
                }
                if (recipe.HasResult(ItemID.Muramasa))
                {
                    recipe.AddIngredient(ModContent.ItemType<AlloyBar>(), 22);
                }
                if (recipe.HasResult(ModContent.ItemType<Roxcalibur>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<AlloyBar>(), 21);
                }
                if (recipe.HasResult(ItemID.ShadowKey))
                {
                    recipe.AddIngredient(ModContent.ItemType<AlloyBar>(), 22);
                }
                if (recipe.HasResult(ModContent.ItemType<SunSpiritStaff>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<AlloyBar>(), 5);
                    recipe.AddIngredient(ModContent.ItemType<EssenceofZot>(), 2);
                }
                if (recipe.HasResult(ItemID.UltrabrightTorch))
                {
                    recipe.AddIngredient(ModContent.ItemType<AlloyBar>(), 4);
                }
                if (recipe.HasResult(ModContent.ItemType<VictideBreastplate>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<AlloyBar>());
                }
                if (recipe.HasResult(ModContent.ItemType<GeliticBlade>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<AlloyBar>(), 5);
                }
                if (recipe.HasResult(ModContent.ItemType<Gelpick>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<AlloyBar>(), 5);
                }
                if (recipe.HasResult(ModContent.ItemType<Goobow>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<AlloyBar>(), 5);
                }
                if (recipe.HasResult(ModContent.ItemType<GunkShot>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<AlloyBar>(), 5);
                }
                if (recipe.HasResult(ModContent.ItemType<StatigelArmor>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<AlloyBar>(), 5);
                }
                if (recipe.HasResult(ModContent.ItemType<TheGodsGambit>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<AlloyBar>(), 5);
                }
                if (recipe.HasResult(ModContent.ItemType<StatigelGreaves>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<AlloyBar>(), 5);
                }
                if (recipe.HasResult(ModContent.ItemType<StatigelHeadMagic>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<AlloyBar>(), 5);
                }
                if (recipe.HasResult(ModContent.ItemType<StatigelHeadSummon>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<AlloyBar>(), 5);
                }
                if (recipe.HasResult(ModContent.ItemType<StatigelHeadRogue>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<AlloyBar>(), 5);
                }
                if (recipe.HasResult(ModContent.ItemType<StatigelHeadRanged>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<AlloyBar>(), 5);
                }
                if (recipe.HasResult(ModContent.ItemType<StatigelHeadMelee>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<AlloyBar>(), 5);
                }
                if (recipe.HasIngredient(ModContent.ItemType<CalamityMod.Items.Placeables.FurnitureStatigel.StatigelBlock>()) || recipe.HasResult(ModContent.ItemType<CalamityMod.Items.Placeables.FurnitureStatigel.StatigelBlock>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<AlloyBar>(), 5);
                }
                #endregion
                #region Essences
                // Zot
                if (recipe.HasResult(ModContent.ItemType<AstrealDefeat>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<ContinentalGreatbow>());
                    recipe.AddIngredient(ModContent.ItemType<DarkechoGreatbow>());
                    recipe.AddIngredient(ModContent.ItemType<BladedgeGreatbow>());
                    recipe.RemoveIngredient(ModContent.ItemType<AshesofCalamity>());
                    recipe.AddIngredient(ModContent.ItemType<EssenceofZot>(), 5);
                }
                if (recipe.HasResult(ModContent.ItemType<CosmicAnvilItem>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<EssenceofZot>(), 10);
                    recipe.AddIngredient(ModContent.ItemType<EssentialEssenceBar>(), 40);
                }
                if (recipe.HasResult(ModContent.ItemType<CosmicWorm>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<EssenceofZot>(), 30);
                    recipe.AddIngredient(ModContent.ItemType<EssentialEssenceBar>(), 40);
                }
                if (recipe.HasResult(ModContent.ItemType<DaawnlightSpiritOrigin>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<EssenceofZot>(), 4);
                }
                // Crystal
                if (recipe.HasResult(ModContent.ItemType<Crystalline>()))
                {
                    recipe.RemoveIngredient(ModContent.ItemType<WulfrumKnife>());
                    recipe.AddIngredient(ModContent.ItemType<EssenceofCrystal>(), 5);
                }
                if (recipe.HasResult(ModContent.ItemType<LunicEye>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<EssenceofCrystal>(), 15);
                }
                if (recipe.HasResult(ModContent.ItemType<StormSurge>()))
                {
                    recipe.RemoveIngredient(ModContent.ItemType<CalamityMod.Items.Placeables.SeaPrism>());
                    recipe.AddIngredient(ModContent.ItemType<EssenceofZot>(), 7);
                }
                //Law
                if (recipe.HasResult(ModContent.ItemType<AccelerationRound>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<EssenceofLaw>());
                }
                if (recipe.HasResult(ModContent.ItemType<EnchantedAxe>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<EssenceofLaw>(), 5);
                }
                if (recipe.HasResult(ItemID.EnchantedSword))
                {
                    recipe.AddIngredient(ModContent.ItemType<EssenceofLaw>(), 5);
                }
                if (recipe.HasResult(ModContent.ItemType<SpiritGlyph>()))
                {
                    recipe.DisableRecipe();
                    recipe.AddRecipeGroup(RecipeGroupID.IronBar, 15);
                    recipe.AddIngredient(ModContent.ItemType<EssenceofLaw>(), 5);
                }
                if (recipe.HasResult(ItemID.Starfury))
                {
                    recipe.AddIngredient(ModContent.ItemType<EssenceofLaw>(), 3);
                }
                // Myst
                if (recipe.HasResult(ModContent.ItemType<EpidemicShredder>()))
                {
                    recipe.RemoveIngredient(ItemID.Nanites);
                    recipe.AddIngredient(ModContent.ItemType<EssenceofMyst>(), 105);
                }
                if (recipe.HasResult(ModContent.ItemType<PlantationStaff>()))
                {
                    if (recipe.HasIngredient(ModContent.ItemType<FleshOfInfidelity>()))
                    {
                        recipe.DisableRecipe();
                    }
                    if (recipe.HasIngredient(ModContent.ItemType<EyeOfNight>()))
                    {
                        recipe.RemoveIngredient(ModContent.ItemType<EyeOfNight>());
                    }
                    recipe.AddIngredient(ModContent.ItemType<EssenceofMyst>(), 10);
                }
                if (recipe.HasResult(ModContent.ItemType<PlagueReaperMask>()))
                {
                    recipe.RemoveIngredient(ItemID.Nanites);
                    recipe.AddIngredient(ModContent.ItemType<EssenceofMyst>(), 11);
                }
                if (recipe.HasResult(ModContent.ItemType<PlagueReaperStriders>()))
                {
                    recipe.RemoveIngredient(ItemID.Nanites);
                    recipe.AddIngredient(ModContent.ItemType<EssenceofMyst>(), 17);
                }
                if (recipe.HasResult(ModContent.ItemType<PlagueReaperVest>()))
                {
                    recipe.RemoveIngredient(ItemID.Nanites);
                    recipe.AddIngredient(ModContent.ItemType<EssenceofMyst>(), 19);
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
                if (recipe.HasResult(ModContent.ItemType<ClaretCannon>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<UnholyBloodCells>(), 25);
                }
                if (recipe.HasResult(ModContent.ItemType<AstralBeaconItem>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<EssentialEssenceBar>(), 40);
                }
                if (recipe.HasResult(ModContent.ItemType<AstrealDefeat>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<EssentialEssenceBar>(), 40);
                }
                if (recipe.HasResult(ModContent.ItemType<ClockworkBow>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<EssentialEssenceBar>(), 40);
                }
                if (recipe.HasResult(ModContent.ItemType<CosmicRainbow>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<EssentialEssenceBar>(), 40);
                }
                if (recipe.HasResult(ModContent.ItemType<Abombination>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<EssentialEssenceBar>(), 40);
                }
                if (recipe.HasResult(ModContent.ItemType<AbyssBlade>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<EssentialEssenceBar>(), 40);
                }
                if (recipe.HasResult(ModContent.ItemType<AbyssalMirror>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<EssentialEssenceBar>(), 40);
                }
                if (recipe.HasResult(ModContent.ItemType<AdvancedDisplay>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<EssentialEssenceBar>(), 40);
                }
                if (recipe.HasResult(ModContent.ItemType<AridArtifact>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<EssentialEssenceBar>(), 40);
                }
                if (recipe.HasResult(ModContent.ItemType<TrueArkoftheAncients>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<EssentialEssenceBar>(), 40);
                }
                if (recipe.HasResult(ModContent.ItemType<AsgardsValor>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<EssentialEssenceBar>(), 40);
                }
                if (recipe.HasResult(ModContent.ItemType<AstralChunk>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<EssentialEssenceBar>(), 40);
                }
                if (recipe.HasResult(ModContent.ItemType<TrueBiomeBlade>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<EssentialEssenceBar>(), 40);
                }
                if (recipe.HasResult(ModContent.ItemType<BarracudaGun>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<EssentialEssenceBar>(), 40);
                }
                if (recipe.HasResult(ModContent.ItemType<BladedgeGreatbow>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<EssentialEssenceBar>(), 40);
                }
                if (recipe.HasResult(ModContent.ItemType<BrimstoneSword>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<EssentialEssenceBar>(), 40);
                }
                if (recipe.HasResult(ModContent.ItemType<CatastropheClaymore>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<EssentialEssenceBar>(), 40);
                }
                if (recipe.HasResult(ModContent.ItemType<DarklightGreatsword>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<EssentialEssenceBar>(), 40);
                }
                if (recipe.HasResult(ModContent.ItemType<EnhancedNanoRound>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<EssentialEssenceBar>(), 40);
                }
                if (recipe.HasResult(ModContent.ItemType<ExoticPheromones>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<EssentialEssenceBar>(), 40);
                }
                if (recipe.HasResult(ModContent.ItemType<FlarefrostBlade>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<EssentialEssenceBar>(), 40);
                }
                if (recipe.HasResult(ModContent.ItemType<Seafood>()))
                {
                    recipe.DisableRecipe();
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
