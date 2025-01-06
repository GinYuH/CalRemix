using CalRemix.Content.Projectiles.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalRemix.Content.DamageClasses;
using CalamityMod.Rarities;
using CalamityMod.Items;
using CalRemix.Content.Projectiles.Weapons.Stormbow;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.Items.Materials;

namespace CalRemix.Content.Items.Weapons.Stormbow
{
    public class TheSimpstring : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 4;
            Item.DamageType = ModContent.GetInstance<StormbowDamageClass>();
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.rare = ModContent.RarityType<DarkBlue>();
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<RisingFire>().
                AddIngredient(ItemID.Cobweb, 15).
                AddIngredient(ItemID.YellowDye, 5).
                AddIngredient<NightmareFuel>(2).
                AddTile<CosmicAnvil>().
                Register();
        }
        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[ModContent.ProjectileType<TheSimpstringHoldout>()] < 1;
    }
}
