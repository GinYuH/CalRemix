using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CalamityMod.Items;
using CalRemix.Content.Projectiles.Weapons;
using CalRemix.Content.Projectiles.Hostile;
using CalamityMod.Items.Weapons.Ranged;
using CalRemix.Content.Items.Materials;

namespace CalRemix.Content.Items.Weapons
{
    public class Hermes : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.rare = ItemRarityID.Red;
            Item.value = CalamityGlobalItem.RarityRedBuyPrice;
            Item.useTime = 72;
            Item.useAnimation = 72;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.UseSound = ScorchedEarth.ShootSound with { Pitch = -0.4f };
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 340;
            Item.knockBack = 12f;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<MercuryRocketFriendly>();
            Item.shootSpeed = 20f;
            Item.useAmmo = AmmoID.Rocket;
        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<MercuryCoatedSubcinium>(), 12)
                .AddIngredient(ModContent.ItemType<Mercury>(), 20)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
