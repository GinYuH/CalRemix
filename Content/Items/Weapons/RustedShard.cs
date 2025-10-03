using CalRemix.Content.Projectiles.Weapons;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.Items.Materials;
using Terraria.Audio;
using CalRemix.Content.Items.Placeables.Subworlds.Sealed;
using CalRemix.Content.Items.Materials;

namespace CalRemix.Content.Items.Weapons
{
    public class RustedShard : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 34;
            Item.knockBack = 3f;
            Item.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();

            Item.useStyle = ItemUseStyleID.Rapier;
            Item.useAnimation = 14;
            Item.useTime = 14;
            Item.autoReuse = true;
            Item.UseSound = BetterSoundID.ItemGolfClubSwing;

            Item.width = 24;
            Item.height = 24;

            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.buyPrice(0, 36, 0, 0);

            Item.shoot = ModContent.ProjectileType<RustedShardProjectile>();
            Item.shootSpeed = 1.6f;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<SealToken>(5)
                .AddIngredient<SealedStone>(30)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
