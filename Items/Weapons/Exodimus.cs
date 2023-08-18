using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;
using CalamityMod.Rarities;

namespace CalRemix.Items.Weapons;

public class Exodimus : ModItem
{
    public override void SetStaticDefaults()
    {

        DisplayName.SetDefault("Exodimus");
        Tooltip.SetDefault("Throws out a dagger aggressively dart towards an enemy The dagger will then explode into a cross of laser beams. \n" + "Stealth strikes double the amount of beams on dissipation into an 8 pointed star");
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
    }

    public override void SetDefaults()
    {
        Mod calamityMod = ModLoader.GetMod("CalamityMod");

        Item.CloneDefaults(calamityMod.Find<ModItem>("Seraphim").Type);
        Item.damage = 426;
        Item.width = 40;
        Item.height = 40;
        Item.useTime = 10;
        Item.useAnimation = 10;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.noUseGraphic = true;
        Item.knockBack = 0;
        Item.value = Item.buyPrice(0, 52, 0, 50);
        Item.rare = ModContent.RarityType<Violet>();
        Item.value = CalamityGlobalItem.Rarity15BuyPrice;
        Item.UseSound = SoundID.Item1;
        Item.autoReuse = true;
        Item.shoot = Mod.Find<ModProjectile>("ExodimusPROJ").Type;
        Item.shootSpeed = 64;
       
       
    }

  
    public override void AddRecipes()
    {
        Mod calamityMod = ModLoader.GetMod("CalamityMod");
        Recipe recipe = CreateRecipe();

        recipe.AddIngredient(calamityMod.Find<ModItem>("MiracleMatter").Type);
        recipe.AddIngredient(calamityMod.Find<ModItem>("Seraphim").Type);
        recipe.AddIngredient(calamityMod.Find<ModItem>("JawsOfOblivion").Type);
        recipe.AddIngredient(calamityMod.Find<ModItem>("StellarKnife").Type);
        recipe.AddIngredient(calamityMod.Find<ModItem>("ShinobiBlade").Type);
        recipe.AddTile(calamityMod.Find<ModTile>("DraedonsForge").Type);
        recipe.Register();
    }


}
