using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;
using CalRemix.Content.Projectiles.Weapons;
using CalRemix.Content.Items.Materials;

namespace CalRemix.Content.Items.Weapons
{
    public class Umbren : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 66;
            Item.height = 66;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.damage = 131;
            Item.knockBack = 4f;
            Item.useAnimation = 25;
            Item.useTime = 5;
            Item.autoReuse = false;
            Item.useStyle = ItemUseStyleID.Shoot;

            Item.value = CalamityGlobalItem.RarityRedBuyPrice;
            Item.rare = ItemRarityID.Red;

            Item.shoot = ModContent.ProjectileType<UmbrenSwing>();
            Item.shootSpeed = 24f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<Mikado>()).
                AddIngredient(ModContent.ItemType<VoidSingularity>()).
                AddIngredient(ModContent.ItemType<RotPearl>()).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
