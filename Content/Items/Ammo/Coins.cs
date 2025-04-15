using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Ammo;

public abstract class Coin : ModItem
{
    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        ItemID.Sets.NeverAppearsAsNewInInventory[Type] = true;
        ItemID.Sets.CommonCoin[Type] = true;
        ItemID.Sets.IsAMaterial[Type] = false;
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Item.maxStack = Item.CommonMaxStack;
        Item.ammo = AmmoID.Coin;
        Item.notAmmo = true;
        Item.DamageType = DamageClass.Ranged;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useTurn = true;
        Item.useAnimation = 15;
        Item.useTime = 10;
        Item.autoReuse = true;
        Item.consumable = true;
        Item.noMelee = true;
    }
}

public class CosmiliteCoin : Coin
{
    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        Item.ResearchUnlockCount = 1;
        DisplayName.SetDefault("Cosmilite Coin");
        Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(8, 8));
        ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        
        ItemID.Sets.CoinLuckValue[Type] = 100000000;
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Item.width = 14;
        Item.height = 20;

        Item.value = Item.sellPrice(platinum: 100);
        Item.damage = 400;
        Item.shoot = 0; // todo
        Item.shootSpeed = 5f;
        Item.createTile = -1; // todo
    }

    public override void AddRecipes()
    {
        CreateRecipe().AddIngredient(ItemID.PlatinumCoin, 100).Register();
        CreateRecipe(100).AddIngredient<Klepticoin>(1).Register();
    }

    public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
    {
        Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(8, 8));
        return true;
    }
}

public class Klepticoin : Coin
{
    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        Item.ResearchUnlockCount = 1;
        DisplayName.SetDefault("Klepticoin");
        Tooltip.SetDefault("The change of the gods");
        Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(12, 12));
        ItemID.Sets.AnimatesAsSoul[Item.type] = true;

        ItemID.Sets.CoinLuckValue[Type] = int.MaxValue; //10000000000;
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Item.width = 28;
        Item.height = 34;

        // Item.value = Item.sellPrice(platinum: 10000); // too much!!!
        Item.value = int.MaxValue;
        Item.damage = 800;
        Item.shoot = 0; // todo
        Item.shootSpeed = 6f;
        Item.createTile = -1; // todo
    }

    public override void AddRecipes()
    {
        CreateRecipe().AddIngredient<CosmiliteCoin>(100).Register();
    }

    public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
    {
        Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(12, 12));
        return true;
    }
}