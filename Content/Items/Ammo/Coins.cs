using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ReLogic.Content;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Ammo;

public abstract class Coin : ModItem
{
    private class HiMom : ILoadable
    {
        public void Load(Mod mod)
        {
            On_Main.DrawItem_GetBasics += (On_Main.orig_DrawItem_GetBasics orig, Main self, Item item, int slot, out Texture2D texture, out Rectangle frame, out Rectangle glowmaskFrame) =>
            {
                if (item.ModItem is Coin coinItem)
                {
                    texture = ModContent.Request<Texture2D>(coinItem.Texture + "_Dropped", AssetRequestMode.ImmediateLoad).Value;
                    AnimateSlot(slot, coinItem.Animation.TicksPerFrame, coinItem.Animation.FrameCount);
                    frame = glowmaskFrame = coinItem.Animation.GetFrame(texture, Main.itemFrameCounter[slot]);
                    return;
                }

                orig(self, item, slot, out texture, out frame, out glowmaskFrame);

                return;

                static void AnimateSlot(int slot, int gameFramesPerSpriteFrame, int spriteFramesAmount)
                {
                    if (++Main.itemFrameCounter[slot] >= gameFramesPerSpriteFrame * spriteFramesAmount)
                    {
                        Main.itemFrameCounter[slot] = 0;
                    }
                }
            };
        }

        public void Unload() { }
    }

    public virtual DrawAnimation Animation { get; } = new DrawAnimationVertical(6, 8);

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
}

public class Klepticoin : Coin
{
    public override DrawAnimation Animation { get; } = new DrawAnimationVertical(6, 12);

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        Item.ResearchUnlockCount = 1;
        DisplayName.SetDefault("Klepticoin");
        Tooltip.SetDefault("The change of the gods");
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
}