using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalRemix.Content.Projectiles;
using CalRemix.Content.Items.Materials;

namespace CalRemix.Content.Items.Tools.Chlorium
{
    public class ChloriumChainsaw : ModItem
    {
        public override void SetStaticDefaults()
        {
            // As mentioned in the documentation, IsDrill and IsChainsaw automatically reduce useTime and useAnimation to 60% of what is set in SetDefaults and decrease tileBoost by 1, but only for vanilla items.
            // We set it here despite it doing nothing because it is likely to be used by other mods to provide special effects to drill or chainsaw items globally.
            ItemID.Sets.IsChainsaw[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ChlorophyteChainsaw);
            Item.shoot = ModContent.ProjectileType<ChloriumChainsawProjectile>();
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
