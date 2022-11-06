using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using CalRemix.Items;
using CalRemix.Items.Accessories;
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

namespace CalRemix
{
    public class Recipes : ModSystem
    {
        public override void AddRecipes() 
        {
            {
                Recipe feather = Recipe.Create(ModContent.ItemType<EffulgentFeather>(), 3);
                feather.AddIngredient<DesertFeather>(3)
                .AddIngredient<LifeAlloy>()
                .AddTile(TileID.LunarCraftingStation)
                .Register();
            }
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
            CandleRecipe(ModContent.ItemType<ResilientCandle>(), ItemID.SoulofNight, 445, ModContent.ItemType<EssenceofSunlight>(), 444);
            CandleRecipe(ModContent.ItemType<SpitefulCandle>(), ModContent.ItemType<EssenceofSunlight>(), 1098, ModContent.ItemType<EssenceofChaos>(), 987);
            CandleRecipe(ModContent.ItemType<VigorousCandle>(), ItemID.SoulofLight, 277, ModContent.ItemType<EssenceofSunlight>(), 128);
            CandleRecipe(ModContent.ItemType<VigorousCandle>(), ItemID.SoulofFlight, 3422, ModContent.ItemType<EssenceofEleum>(), 357);
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
                
                if (recipe.HasResult(ModContent.ItemType<FabsolsVodka>()))
                {
                    recipe.DisableRecipe();
                }
                if (recipe.HasResult(ModContent.ItemType<GrandScale>()))
                {
                    recipe.DisableRecipe();
                }
                if (recipe.HasResult(ModContent.ItemType<ShadowspecBar>()))
                {
                    recipe.AddIngredient<SubnauticalPlate>();
                }
                if (recipe.TryGetIngredient(ModContent.ItemType<PearlShard>(), out Item shard) && recipe.HasResult(ModContent.ItemType<SeaRemains>()) || recipe.HasResult(ModContent.ItemType<MonstrousKnives>()) || recipe.HasResult(ModContent.ItemType<FirestormCannon>()) || recipe.HasResult(ModContent.ItemType<SuperballBullet>()))
		        {
			        shard.type = ModContent.ItemType<ParchedScale>();
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
                if (recipe.HasResult(ModContent.ItemType<ExoticPheromones>()))
                {
                    recipe.RemoveIngredient(ModContent.ItemType<LifeAlloy>());
                    recipe.RemoveIngredient(ItemID.FragmentSolar);
                    recipe.RemoveTile(TileID.LunarCraftingStation);
                    recipe.AddIngredient(ModContent.ItemType<UnholyCore>(), 5);
                    recipe.AddIngredient(ItemID.SoulofLight, 5);
                    recipe.AddIngredient(ItemID.SoulofNight, 5);
                    recipe.AddIngredient(ItemID.PinkPricklyPear);
                    recipe.AddTile(TileID.MythrilAnvil);
                }
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
                if (recipe.HasResult(ModContent.ItemType<ElysianTracers>()))
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
                    recipe.AddIngredient<PlagueHive>();
                    recipe.AddIngredient<AstralArcanum>();
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
    }
}
