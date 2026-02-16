using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Projectiles.Weapons;
using CalRemix.Content.Items.Materials;

namespace CalRemix.Content.Items.Weapons
{
    public class ChloriumPike : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.Spears[Item.type] = true;
        }
        public override void SetDefaults() 
		{
            Item.CloneDefaults(ItemID.AdamantiteGlaive);
            Item.rare = ItemRarityID.Lime;
            Item.shoot = ModContent.ProjectileType<ChloriumPikeProjectile>();
        }
        public override bool CanUseItem(Player player)
        {
            // Ensures no more than one spear can be thrown out, use this when using autoReuse
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ChloriumBar>(12).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
