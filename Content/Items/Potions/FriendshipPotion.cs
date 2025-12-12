using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Rarities;
using CalRemix.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions
{
    public class FriendshipPotion : ModItem
    {

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
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.buyPrice(gold: 1);
            Item.buffType = ModContent.BuffType<FriendshipBuff>();
            Item.buffTime = CalamityUtils.SecondsToFrames(60);

        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.BottledWater).
                AddIngredient(ModContent.ItemType<GalacticaSingularity>()).
                AddIngredient(ModContent.ItemType<MeldBlob>(), 1).
                AddTile(TileID.AlchemyTable).
                Register();
        }
    }
}