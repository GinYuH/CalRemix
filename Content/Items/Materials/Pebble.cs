using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityMod.Items.Placeables.Furniture.Trophies;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Items.Potions;
using CalRemix.Content.Items.Placeables;
using CalRemix.Content.Items.Placeables.Subworlds.Sealed;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Materials
{
    public class Pebble : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
        }
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.sellPrice(silver: 5);
            Item.maxStack = 9999;
        }


        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<PebbleFlake>(), 6)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class PebbleBrimstone : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
        }
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.sellPrice(silver: 5);
            Item.maxStack = 9999;
        }


        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<PebbleShardBrimstone>(), 6)
                .AddIngredient(ModContent.ItemType<CalamitasCloneTrophy>())
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class PebbleAstral : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
        }
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.sellPrice(silver: 5);
            Item.maxStack = 9999;
        }


        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<PebbleShardAstral>(), 6)
                .AddIngredient(ItemID.MoonLordTrophy)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class PebbleSealed : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
        }
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.sellPrice(silver: 5);
            Item.maxStack = 9999;
        }


        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<PebbleShardSealed>(), 6)
                .AddIngredient(ModContent.ItemType<FrozenSealedTear>(), 6)
                //.AddIngredient(ModContent.ItemType<DraedonTrophy>())
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    public class PebbleCarnelian : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
        }
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.sellPrice(silver: 5);
            Item.maxStack = 9999;
        }


        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<PebbleShardCarnelian>(), 6)
                .AddIngredient(ModContent.ItemType<RefinedCarnelianite>(), 6)
                //.AddIngredient(ModContent.ItemType<WinterWitchTrophy>()) ??? idk unsure
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class PebbleFlake : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
        }
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.sellPrice(silver: 5);
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            CreateRecipe(6)
                .AddIngredient(ModContent.ItemType<Pebble>())
                .AddTile(TileID.MythrilAnvil)
                .Register();
            CreateRecipe(12)
                .AddIngredient(ItemID.MoonLordTrophy)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    public class PebbleShardBrimstone : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
        }
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.sellPrice(silver: 5);
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<PebbleFlake>())
                .AddIngredient(ModContent.ItemType<AshesofCalamity>(), 12)
                .AddIngredient(ModContent.ItemType<BrimstoneSlag>(), 12)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    public class PebbleShardAstral : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
        }
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.sellPrice(silver: 5);
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<PebbleFlake>())
                .AddIngredient(ModContent.ItemType<AstralBar>(), 12)
                .AddIngredient(ModContent.ItemType<AstralStone>(), 12)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    public class PebbleShardCarnelian : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
        }
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.sellPrice(silver: 5);
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<PebbleFlake>())
                .AddIngredient(ModContent.ItemType<RefinedCarnelianite>(), 12)
                .AddIngredient(ModContent.ItemType<CarnelianStone>(), 12)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    public class PebbleShardSealed : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
        }
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.sellPrice(silver: 5);
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<PebbleFlake>())
                .AddIngredient(ModContent.ItemType<FrozenSealedTear>(), 12)
                .AddIngredient(ModContent.ItemType<SealedStone>(), 12)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
