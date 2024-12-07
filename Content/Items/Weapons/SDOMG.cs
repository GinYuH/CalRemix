using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.Items;
using CalamityMod.Items.Weapons.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Terraria.DataStructures;
using CalRemix.Content.Projectiles.Weapons;
using CalamityMod.Items.Weapons.DraedonsArsenal;

namespace CalRemix.Content.Items.Weapons
{
	public class SDOMG : ModItem
	{
        public override void SetStaticDefaults() 
		{
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
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
			Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
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
            if (player.altFunctionUse == 2)
                return false;
            return Main.rand.NextFloat() >= 0.75f;
        }
        public override bool AltFunctionUse(Player player) => true;
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                if (player.HasItem(ModContent.ItemType<PlasmaGrenade>()) && player.velocity.Length() == 0)
                {
                    Item.UseSound = OpalStriker.Charge;
                    player.ConsumeItem(ModContent.ItemType<PlasmaGrenade>(), true);
                    return true;
                }
                return false;
            }
            else
            {
                Item.UseSound = SoundID.Item40;
                return true;
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                if (player.ownedProjectileCounts[Item.shoot] < 1)
                    Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<SDOMGRay>(), damage / 2, knockback, player.whoAmI);
                return false;
            }
            return true;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (player.altFunctionUse != 2)
                velocity = velocity.RotatedByRandom(MathHelper.ToRadians(75-(player.GetModPlayer<CalRemixPlayer>().commonItemHoldTimer/10)));
        }
        public override Vector2? HoldoutOffset() => new Vector2(-2f, -2f);
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
