using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalRemix.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.Audio;

namespace CalRemix.Items.Weapons
{
	public class Exosphear : ModItem
	{
		public override void SetStaticDefaults() 
		{
            SacrificeTotal = 1;
            DisplayName.SetDefault("Gravitonomy Pike");
            Tooltip.SetDefault("Fires exo pike beams that split into more beams\n" +
                "Hitting enemies with the pike sucks in all nearby enemies\n" +
                "Ignores immunity frames");
            ItemID.Sets.SkipsInitialUseSound[Item.type] = true;
        }
		public override void SetDefaults() 
		{
			Item.width = 82;
			Item.height = 88;
			Item.rare = ModContent.RarityType<Violet>();
			Item.value = CalamityGlobalItem.Rarity15BuyPrice;
            Item.useTime = 18; 
			Item.useAnimation = 18;
			Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
			Item.UseSound = SoundID.DD2_GhastlyGlaivePierce;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.noMelee = true;
            Item.damage = 257;
			Item.knockBack = 9.5f;
			Item.shoot = ModContent.ProjectileType<ExosphearSpear>();
			Item.shootSpeed = 12f;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }
        public override bool? UseItem(Player player)
        {
            if (!Main.dedServ && Item.UseSound.HasValue)
            {
                SoundEngine.PlaySound(Item.UseSound.Value, player.Center);
            }
            return null;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position + Vector2.Normalize(velocity) * 120f, velocity, ModContent.ProjectileType<ExosphearBeam>(), damage, knockback, player.whoAmI);
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ElementalLance>(1).
                AddIngredient<StreamGouge>(1).
                AddIngredient<Nadir>(1).
                AddIngredient<BansheeHook>(1).
                AddIngredient<InsidiousImpaler>(1).
                AddIngredient<MiracleMatter>(1).
                AddTile<DraedonsForge>().
                Register();
        }
    }
}
