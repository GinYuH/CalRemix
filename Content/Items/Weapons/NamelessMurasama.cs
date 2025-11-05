using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Projectiles.Weapons;
using CalamityMod.Items.Weapons.Melee;
using CalRemix.Content.Items.Materials;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace CalRemix.Content.Items.Weapons
{
    public class NamelessMurasama : ModItem
    {
        public override string Texture => "CalamityMod/Items/Weapons/UHFMurasama";

        public override void SetDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
            Item.CloneDefaults(ModContent.ItemType<Murasama>());
            Item.damage = 200;
            Item.value = Item.sellPrice(gold: 50);
            Item.shoot = ModContent.ProjectileType<NamelessMurasamaSlash>();
            Item.rare = ItemRarityID.Purple;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse !=2 && player.ownedProjectileCounts[base.Item.shoot] > 0)
            {
                return false;
            }
            if (player.altFunctionUse == 2)
            {
                Item.useTime = Item.useAnimation = 40;
                Item.autoReuse = true;
            }
            else
            {
                Item.useAnimation = 25;
                Item.useTime = 5;
                Item.autoReuse = true;
            }
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse != 2)
            return true;
            else
            {
                Projectile.NewProjectile(source, player.Center, velocity, ModContent.ProjectileType<NamelessVortex>(), damage, knockback, player.whoAmI);
                return false;
            }
        }

        public override void UpdateInventory(Player player)
        {
            player.Remix().murablink = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Muramasa).
                AddIngredient<KrakenTooth>(2).
                AddIngredient<VoidSingularity>(3).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
