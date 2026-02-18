using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using CalamityMod.Items;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalRemix.Content.Items.Materials;
using CalamityMod.Items.Placeables;

namespace CalRemix.Content.Items.Weapons
{
    public class WrathoftheGods : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Wrath of the Gods");
            // Tooltip.SetDefault("Summons a godly pulse at the cursor that periodically summon a ring of godly fireballs and occasionally explodes into a massive star of projectile and fires a deathray to annihilate your foes\n"+"[c/FF3333:The End..?]");
        }
        public override void SetDefaults()
        {
            Item.damage = 999;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 75;
            Item.width = 28;
            Item.height = 30;
            Item.useTime = 56;
            Item.useAnimation = 56;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 7f;
            Item.value = CalamityGlobalItem.RarityHotPinkBuyPrice;
            Item.rare = ModContent.RarityType<LightmixOrange>();
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<RockBullet>();
            Item.shootSpeed = 12f;
            Item.Remix().devItem = "KitsuLilly";
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, damage, knockback, player.whoAmI, 0f, 0f);
            return false;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine tooltipLine in tooltips)
            {
                if (tooltipLine.Mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.OverrideColor = Color.Lerp(new Color(194, 39, 39), new Color(125, 12, 12), 0.22f); //change the color accordingly to above
                }
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<WrathoftheDragons>(1).
                AddIngredient(ModContent.ItemType<EventHorizon>()).
                AddIngredient(ModContent.ItemType<AshesofAnnihilation>(), 5).
                AddIngredient(ModContent.ItemType<AscendantSpiritEssence>(), 10).
                AddIngredient(ModContent.ItemType<Rock>()).
                AddIngredient(ModContent.ItemType<LightmixBar>(), 5).
                AddTile(ModContent.TileType<DraedonsForge>()).
                Register();
        }
    }
}
