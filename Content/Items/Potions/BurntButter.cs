using CalamityMod;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions
{
    public class BurntButter : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 5;
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 26;
            Item.rare = ItemRarityID.Cyan;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.value = Item.sellPrice(silver: 2);
            Item.UseSound = BetterSoundID.ItemEat;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.useTurn = true;
            Item.buffType = BuffID.WellFed3;
            Item.buffTime = CalamityUtils.SecondsToFrames(300);
        }

        public override bool? UseItem(Player player)
        {
            player.AddBuff(BuffID.ObsidianSkin, Item.buffTime);
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<Butter>()).
                AddIngredient(ModContent.ItemType<FireApple>()).
                AddTile(TileID.CookingPots).
                Register();
        }
    }
}
