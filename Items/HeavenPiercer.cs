using CalRemix.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace CalRemix.Items
{
    public class HeavenPiercer : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Heaven Piercer");
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }
        public override void SetDefaults()
        {
            Item.damage = 280;
            Item.knockBack = 1f;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.pick = 275;
            Item.axe = 180 / 5;
            Item.DamageType = DamageClass.Melee;
            Item.width = 36;
            Item.height = 18;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(gold: 20);
            Item.rare = ModContent.RarityType<DarkBlue>();
            Item.UseSound = SoundID.Item23;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<HeavenPiercerProj>();
            Item.shootSpeed = 40f;
		}
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
                Item.UseSound = SoundID.Item33;
            else
                Item.UseSound = SoundID.Item23;
            return true;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                Projectile.NewProjectile(source, position + velocity / 10f, velocity / 10f, ModContent.ProjectileType<HeavenPiercerLaser>(), damage / 2, knockback, player.whoAmI);
                return false;
            }
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<CosmiliteBar>(13).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
