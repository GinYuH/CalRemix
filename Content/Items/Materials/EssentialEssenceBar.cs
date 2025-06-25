using CalamityMod.Items.Materials;
using CalRemix.Content.Tiles;
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
			Item.ResearchUnlockCount = 25;
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(8, 4));
            ItemID.Sets.AnimatesAsSoul[Type] = true;

            if (!Main.dedServ)
            {
                HelperMessage.New("EssentialEssenceDisbelief", "You better have gotten that from shimmering cores, otherwise that's genuinely fucking embarassing, man. Did you really waste your time getting every single component? You probably had most of this stuff lying around, but even then. You can't even do jack shit with ONE bar! How many more of these are you gonna craft?? You like, genuinely repulse me. I feel like I'm going to barf in my own mouth just from looking at you. NOBODY is impressed by the fact you're willing to waste your time resource gathering. The starter bag came with a red pickaxe for a reason, get to learning the codes.", "EvilFannyMiffed",
                    (ScreenHelperSceneMetrics m) => Main.LocalPlayer.HasItem(Type))
                    .SpokenByAnotherHelper(ScreenHelpersUIState.EvilFanny).SetHoverTextOverride("Thanks, uh, for the \"evil\" tip, Evil Fanny...?");
            }
        }
		public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<EssentialEssenceBarPlant>());
            Item.rare = ItemRarityID.LightPurple;
            Item.sellPrice(silver: 80);
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
