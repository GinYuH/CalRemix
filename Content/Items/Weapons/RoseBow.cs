using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;
using CalamityMod.Rarities;
using CalRemix.Content.Projectiles.Weapons;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Materials;
using CalRemix.Content.Items.Materials;

namespace CalRemix.Content.Items.Weapons
{
    public class RoseBow : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.rare = ItemRarityID.Red;
            Item.value = CalamityGlobalItem.RarityRedBuyPrice;
            Item.useTime = 6;
            Item.useAnimation = 6;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item109;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 98;
            Item.knockBack = 5f;
            Item.noMelee = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 23f;
            Item.useAmmo = AmmoID.Arrow;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (type == ProjectileID.WoodenArrowFriendly)
                type = ModContent.ProjectileType<RoseArrow>();
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(12.5f));
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.PearlwoodBow).
                AddIngredient<ArchAmaryllis>().
                AddIngredient<MercuryCoatedSubcinium>(5).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
