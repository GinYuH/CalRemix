using CalamityMod.Items.Materials;
using CalRemix.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions
{
    public class Coke : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ashes of the Sunken Sea");
            Tooltip.SetDefault("These fine crystals feel very powerful");
        }


        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item2;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(gold: 50);
        }
        public override bool CanUseItem(Player player)
        {
            if (player.HasBuff(ModContent.BuffType<Euphoria>()))
            {
                Item.buffType = ModContent.BuffType<Anxiety>();
                Item.buffTime = 72000;
            }
            else if (!player.HasBuff(ModContent.BuffType<Euphoria>()))
            {
                Item.buffType = ModContent.BuffType<Euphoria>();
                Item.buffTime = 36000;
            }
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.GlowingMushroom, 10).
                AddIngredient<DemonicBoneAsh>(2).
                AddIngredient(ItemID.Gel, 20).
                AddTile(TileID.AlchemyTable).
                Register();
        }
    }
}