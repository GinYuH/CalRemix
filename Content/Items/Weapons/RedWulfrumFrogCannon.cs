using CalamityMod.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Sounds;
using CalamityMod.Items.Materials;
using CalRemix.Content.Projectiles.Weapons;

namespace CalRemix.Content.Items.Weapons
{
    public class RedWulfrumFrogCannon : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;
            Item.rare = ItemRarityID.Blue;
            Item.value = CalamityGlobalItem.RarityBlueBuyPrice;
            Item.useTime = 42;
            Item.useAnimation = 42;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.UseSound = CommonCalamitySounds.LargeWeaponFireSound;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 78;
            Item.knockBack = 2f;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<WulfrumFrogProj>();
            Item.shootSpeed = 16;
            Item.useAmmo = ItemID.Frog;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<WulfrumMetalScrap>(16).
                AddIngredient(ItemID.Frog, 10).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
