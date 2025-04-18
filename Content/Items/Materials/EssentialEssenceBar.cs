using CalamityMod.Items.Materials;
using CalRemix.UI;
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
			// DisplayName.SetDefault("Essential Essence Bar");
      		// Tooltip.SetDefault("Flowing with pure energy");
			Item.ResearchUnlockCount = 5;
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(8, 4));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;

            if (!Main.dedServ)
            {
                HelperMessage.New("EssentialEssenceDisbelief", "...Did you seriously go through the pain of making your first essential essence bar? Are you really gonna repeat this pain grinding the materials needed for the items you're gonna make??? There's a damn good reason why that starter bag you opened a long while ago gave you that strange red pickaxe. Place that down, and figure the codes out on your own.", "EvilFannyMiffed",
                    (ScreenHelperSceneMetrics m) => Main.LocalPlayer.HasItem(Type))
                    .SpokenByAnotherHelper(ScreenHelpersUIState.EvilFanny).SetHoverTextOverride("Thanks, uh, for the \"evil\" tip, Evil Fanny...?");
            }
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
                AddIngredient<PerennialBar>().
                AddIngredient<CryonicBar>().
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
