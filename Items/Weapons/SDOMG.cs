using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.Sounds;
using CalamityMod.Items;
using CalamityMod.Items.Weapons.Ranged;
using CalRemix.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CalamityMod.Projectiles.Ranged;
using CalamityMod.Projectiles.Summon;
using CalamityMod;

namespace CalRemix.Items.Weapons
{
	public class SDOMG : ModItem
	{
        private int spread = 0;
        public override void SetStaticDefaults() 
		{
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("SDOMG");
            Tooltip.SetDefault("75% chance to not consume ammo\n" +
                "Rapidly fires a spread of bullets that close in over time\n" +
                "Right clicking while standing still consumes a Plasma Grenade to fire a large deathray\n" +
                "While the deathray is active, you are afflicted with slow, on fire, and oiled"); 
		}

		public override void SetDefaults() 
		{
			Item.width = 10;
			Item.height = 10;
			Item.rare = ModContent.RarityType<DarkBlue>();
			Item.value = CalamityGlobalItem.Rarity14BuyPrice;
            Item.Calamity().donorItem = true;
            Item.useTime = 5; 
			Item.useAnimation = 5;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;
            Item.UseSound = SoundID.Item40;
            Item.DamageType = DamageClass.Ranged;
			Item.damage = 443;
			Item.knockBack = 5f; 
			Item.noMelee = true;
            Item.channel = true;
			Item.shoot = ProjectileID.PurificationPowder;
			Item.shootSpeed = 18f;
			Item.useAmmo = AmmoID.Bullet;
        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return Main.rand.NextFloat() >= 0.75f;
        }
        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.useTime = 30;
                Item.useAnimation = 30;
            }
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (!player.channel)
            {
                spread = 0;
            }
            else if (spread < 600 && player.channel)
            {
                spread++;
            }
            return true;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(60-(spread/10)));
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SDFMG>(1).
                AddIngredient(ItemID.SDMG).
                AddIngredient<Starmada>(1).
                AddIngredient<CosmiliteBar>(15).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
