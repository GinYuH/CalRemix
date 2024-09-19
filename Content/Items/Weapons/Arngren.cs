using CalamityMod.Items;
using CalamityMod.Items.Weapons.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Placeables;

namespace CalRemix.Content.Items.Weapons
{
    public class Arngren : ModItem
	{
		public override void SetStaticDefaults() 
		{
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("Arngren");
            Tooltip.SetDefault("80% chance to not consume ammo");
		}

		public override void SetDefaults() 
		{
			Item.width = 50;
			Item.height = 50;
			Item.rare = ItemRarityID.Cyan;
			Item.value = CalamityGlobalItem.RarityCyanBuyPrice;
            Item.useTime = 3; 
			Item.useAnimation = 3;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item41;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 50;
			Item.knockBack = 0f; 
			Item.noMelee = true;
			Item.crit = 4;
			Item.shoot = ProjectileID.PurificationPowder;
			Item.shootSpeed = 26f;
			Item.useAmmo = AmmoID.Bullet;
        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return Main.rand.NextFloat() >= 0.80f;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(45));
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ClockGatlignum>(1).
                AddIngredient(ItemID.FragmentStardust, 10).
                AddIngredient<AbyssGravel>(64).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
