using CalamityMod.Buffs.StatBuffs;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Potions;
using CalamityMod.Rarities;
using CalRemix.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Potions
{
    public class Heroin : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magical Slag Ashes");
            Tooltip.SetDefault("You can feel the power of a fallen hero in these ashes");
        }


        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useTurn = true;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.rare = ModContent.RarityType<Violet>();
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.UseSound = SoundID.Item2;
            Item.value = Item.sellPrice(0, 0, 70);
            Item.buffType = ModContent.BuffType<BrimstoneMadness>();
            Item.buffTime = 60 * 60 * 20;
        }
        public override void AddRecipes()
        {
            CreateRecipe(2).
                AddCondition(Condition.PlayerCarriesItem(ModContent.ItemType<BrimstoneLocus>())).
                AddIngredient<FlaskOfBrimstone>().
                AddIngredient<BloodOrb>(25).
                AddTile(TileID.AlchemyTable).
                Register();
            CreateRecipe(5).
                AddCondition(Condition.PlayerCarriesItem(ModContent.ItemType<BrimstoneLocus>())).
                AddIngredient(ItemID.BrokenHeroSword).
                AddTile(TileID.AlchemyTable).
                Register();
        }
    }
}