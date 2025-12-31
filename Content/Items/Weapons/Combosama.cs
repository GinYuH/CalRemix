using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Projectiles.Weapons;
using CalamityMod.Items.Weapons.Melee;
using CalRemix.Content.Items.Materials;
using CalamityMod;
using CalRemix.Content.Cooldowns;
using Terraria.Audio;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using System.Collections.Generic;
using Terraria.DataStructures;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;

namespace CalRemix.Content.Items.Weapons
{
    public class Combosama : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(base.Item.type, new DrawAnimationVertical(6, 8));
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ModContent.ItemType<Murasama>());
            Item.damage = 3000;
            Item.value = Item.sellPrice(gold: 5);
            Item.shoot = ModContent.ProjectileType<CombosamaSlash>();
            Item.rare = ModContent.RarityType<BurnishedAuric>();
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse != 2 && player.ownedProjectileCounts[base.Item.shoot] > 0)
            {
                return false;
            }
            if (player.altFunctionUse == 2 && player.ownedProjectileCounts[ModContent.ProjectileType<CombosamaSpin>()] > 0)
            {
                return false;
            }
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse != 2)
                return true;
            else
            {
                SoundEngine.PlaySound(NamelessMurasama.MurasamaTornado with { PitchVariance = 0.2f }, player.Center);
                Projectile.NewProjectile(source, player.Center, velocity, ModContent.ProjectileType<CombosamaSpin>(), damage, knockback, player.whoAmI);
                return false;
            }
        }
        public override void ModifyTooltips(List<TooltipLine> list) => list.IntegrateHotkey(CalamityKeybinds.NormalityRelocatorHotKey);

        public override void UpdateInventory(Player player)
        {
            player.Remix().murablink = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<Murasama>().
                AddIngredient<NamelessMurasama>().
                AddIngredient<ParadiseInfusedMurasama>().
                AddIngredient<ExoPrism>(10).
                AddTile<DraedonsForge>().
                Register();
        }
    }
}
