using CalamityMod;
using CalRemix.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions
{
    public class SharkPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 5;
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 26;
            Item.rare = ItemRarityID.Blue;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.UseSound = SoundID.Item3;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useTurn = true;
            Item.buffType = ModContent.BuffType<SharkRain>();
            Item.buffTime = CalamityUtils.MinutesToFrames(5);
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe().
                AddIngredient(ItemID.BottledWater).
                AddIngredient(ItemID.SharkBait).
                AddTile(TileID.Bottles);
            if (ModLoader.TryGetMod("SpiritReforged", out Mod spir))
            {
                r.AddIngredient(spir.Find<ModItem>("Cloudstalk").Type);
            }
            r.Register();
        }
    }
}
