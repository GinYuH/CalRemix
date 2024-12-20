using CalamityMod.Items.Materials;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Materials
{
    public class EssentialEssenceBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Essential Essence Bar");
      		Tooltip.SetDefault("Flowing with pure energy");
			Item.ResearchUnlockCount = 5;
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(8, 4));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }
		public override void SetDefaults()
        {
            Item.rare = ItemRarityID.LightPurple;
            Item.sellPrice(silver: 80);
            Item.maxStack = 9999;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<Theswordisinsidethecore>().
                AddIngredient<UnholyBloodCells>(5).
                AddIngredient<SoulFlux>(5).
                AddIngredient<GrandioseGland>().
                AddIngredient<GreenDemonHead>().
                AddIngredient(ItemID.CobaltBar).
                AddIngredient(ItemID.PalladiumBar).
                AddIngredient(ItemID.MythrilBar).
                AddIngredient(ItemID.OrichalcumBar).
                AddIngredient(ItemID.AdamantiteBar).
                AddIngredient(ItemID.TitaniumBar).
                AddIngredient(ItemID.HallowedBar).
                AddIngredient(ItemID.ChlorophyteBar).
                AddIngredient(ItemID.ShroomiteBar).
                AddIngredient(ItemID.SpectreBar).
                AddIngredient<ScoriaBar>().
                AddIngredient<CryonicBar>().
                AddIngredient<AstralBar>().
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
