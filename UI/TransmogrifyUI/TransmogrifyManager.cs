using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalRemix.UI.TransmogrifyUI
{
    public static class TransmogrifyManager
    {
        public static List<TransmogrifyRecipe> TransmogrifyRecipes = new List<TransmogrifyRecipe>
        {
            new TransmogrifyRecipe(ModContent.ItemType<CalamityMod.Items.Weapons.Ranged.AstralBlaster>(),
                ModContent.ItemType<CalamityMod.Items.Placeables.SeaPrism>(), 10, 400,
                ModContent.ItemType<CalamityMod.Items.Weapons.Ranged.ClockGatlignum>()),
            new TransmogrifyRecipe(ModContent.ItemType<CalamityMod.Items.Armor.Tarragon.TarragonBreastplate>(),
                ModContent.ItemType<CalamityMod.Items.Placeables.FurnitureCosmilite.CosmiliteBathtub>(), 10, 1200, // bathtub because no slag
                ModContent.ItemType<CalamityMod.Items.Armor.Prismatic.PrismaticGreaves>()),
            new TransmogrifyRecipe(ModContent.ItemType<CalamityMod.Items.Weapons.Melee.Oracle>(),
                ItemID.FallenStar, 40, 600,
                ModContent.ItemType<CalamityMod.Items.Weapons.Melee.TheMicrowave>()),
            new TransmogrifyRecipe(ModContent.ItemType<CalamityMod.Items.Weapons.DraedonsArsenal.PoleWarper>(),
                ModContent.ItemType<CalamityMod.Items.Materials.EndothermicEnergy>(), 5, 1200,
                ItemID.NorthPole),
            new TransmogrifyRecipe(ModContent.ItemType<CalamityMod.Items.Pets.RottingEyeball>(),
                ModContent.ItemType<CalamityMod.Items.Materials.RottenMatter>(), 22, 600,
                ModContent.ItemType<CalamityMod.Items.Pets.BloodyVein>()),
            new TransmogrifyRecipe(ModContent.ItemType<CalamityMod.Items.Accessories.CryoStone>(),
                ModContent.ItemType<CalamityMod.Items.Materials.EssenceofSunlight>(), 1, 0,
                ItemID.WaterBucket),
            new TransmogrifyRecipe(ModContent.ItemType<CalamityMod.Items.Weapons.Rogue.ShardofAntumbra>(),
                ItemID.SoulofLight, 5, 1200,
                ModContent.ItemType<CalamityMod.Items.Weapons.Magic.LightGodsBrilliance>()),
            #region The Yharim Bar Ones
            new TransmogrifyRecipe(ModContent.ItemType<Items.Materials.YharimBar>(),
                ModContent.ItemType<CalamityMod.Items.Materials.UnholyEssence>(), 20, 1200,
                ModContent.ItemType<CalamityMod.Items.Materials.DivineGeode>(), 20),
            new TransmogrifyRecipe(ModContent.ItemType<Items.Materials.YharimBar>(),
                ModContent.ItemType<CalamityMod.Items.Materials.Polterplasm>(), 10, 3600,
                ModContent.ItemType<CalamityMod.Items.Materials.RuinousSoul>(), 10),
            new TransmogrifyRecipe(ModContent.ItemType<Items.Materials.YharimBar>(),
                ModContent.ItemType<CalamityMod.Items.Materials.EndothermicEnergy>(), 20, 1200,
                ModContent.ItemType<CalamityMod.Items.Materials.NightmareFuel>(), 20),
            new TransmogrifyRecipe(ModContent.ItemType<Items.Materials.YharimBar>(),
                ModContent.ItemType<CalamityMod.Items.Materials.NightmareFuel>(), 20, 1200,
                ModContent.ItemType<CalamityMod.Items.Materials.EndothermicEnergy>(), 21),
            new TransmogrifyRecipe(ModContent.ItemType<Items.Materials.YharimBar>(),
                ItemID.SoulofNight, 20, 1200,
                ModContent.ItemType<CalamityMod.Items.Materials.DarksunFragment>(), 20),
            new TransmogrifyRecipe(ModContent.ItemType<Items.Materials.YharimBar>(),
                ModContent.ItemType<CalamityMod.Items.Placeables.Ores.AuricOre>(), 5, 6000,
                ModContent.ItemType<CalamityMod.Items.Materials.YharonSoulFragment>(), 5),
            new TransmogrifyRecipe(ModContent.ItemType<Items.Materials.YharimBar>(),
                ModContent.ItemType<CalamityMod.Items.Materials.AshesofAnnihilation>(), 5, 12000,
                ModContent.ItemType<CalamityMod.Items.Materials.ExoPrism>(), 5),
            new TransmogrifyRecipe(ModContent.ItemType<Items.Materials.YharimBar>(),
                ModContent.ItemType<CalamityMod.Items.Materials.ExoPrism>(), 5, 12000,
                ModContent.ItemType<CalamityMod.Items.Materials.AshesofAnnihilation>(), 5),
            #endregion
        };

        public static bool CanTransmogrify(this int item)
        {
            foreach(TransmogrifyRecipe recipe in TransmogrifyRecipes)
            {
                if (recipe.ingredient == item) return true;
            }
            return false;
        }

        public static bool CanServeAsCatalystFor(this int item, int item2)
        {
            foreach (TransmogrifyRecipe recipe in TransmogrifyRecipes)
            {
                if (recipe.catalyst == item && recipe.ingredient == item2) return true;
            }
            return false;
        }

        public static TransmogrifyRecipe GetRecipe(int item, int item2)
        {
            foreach (TransmogrifyRecipe recipe in TransmogrifyRecipes)
            {
                if (recipe.ingredient == item && recipe.catalyst == item2) return recipe;
            }
            throw new ApplicationException("Your recipe was: not found.");
        }
    }
}
