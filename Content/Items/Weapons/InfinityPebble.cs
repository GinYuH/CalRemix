using CalamityMod.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Projectiles.Weapons;
using CalamityMod.Sounds;

namespace CalRemix.Content.Items.Weapons
{
    public class InfinityPebble : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 1;
            Item.height = 1;
            Item.rare = ItemRarityID.Purple;
            Item.value = CalamityGlobalItem.RarityPurpleBuyPrice;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.autoReuse = true;
            Item.UseSound = CommonCalamitySounds.LargeWeaponFireSound;
            Item.DamageType = DamageClass.Magic;
            Item.damage = 168;
            Item.knockBack = 6f;
            Item.mana = 5;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<InfinityBeam>();
            Item.shootSpeed = 20f;
            Item.channel = true;
        }
        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<TrueMonorianGem>()).
                AddIngredient(ModContent.ItemType<PebbleAstral>()).
                AddIngredient(ModContent.ItemType<PebbleBrimstone>()).
                AddIngredient(ModContent.ItemType<PebbleSealed>()).
                AddIngredient(ModContent.ItemType<PebbleCarnelian>()).
                AddRecipeGroup(Recipes.AnyEnemyStatue, 3).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
