using CalamityMod.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Projectiles.Weapons;

namespace CalRemix.Content.Items.Weapons
{
    public class TrueMonorianGem : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 1;
            Item.height = 1;
            Item.rare = ItemRarityID.Red;
            Item.value = CalamityGlobalItem.RarityCalamityRedBuyPrice;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.autoReuse = true;
            Item.UseSound = BetterSoundID.ItemBow;
            Item.DamageType = DamageClass.Magic;
            Item.damage = 76;
            Item.knockBack = 10f;
            Item.mana = 12;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<TrueMonorianGemProj>();
            Item.shootSpeed = 40f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<MonorianGem>()).
                AddIngredient(ModContent.ItemType<BrokenHeroGem>()).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
