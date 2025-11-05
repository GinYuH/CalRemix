using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Projectiles.Weapons;
using CalamityMod.Items.Weapons.Melee;
using CalRemix.Content.Items.Materials;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using CalRemix.Content.Projectiles.Hostile;
using System.Collections.Generic;
using CalamityMod;

namespace CalRemix.Content.Items.Weapons
{
    public class NamelessMurasama : ModItem
    {
        public override string Texture => "CalamityMod/Items/Weapons/UHFMurasama";

        public static SoundStyle LongCoolSlash = new SoundStyle("CalRemix/Assets/Sounds/LongCoolSlash");
        public static SoundStyle MurasamaCut = new SoundStyle("CalRemix/Assets/Sounds/SwiftSlice", 2);
        public static SoundStyle MurasamaTornado = new SoundStyle("CalRemix/Assets/Sounds/ReverbSliceQuick");


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
            if (player.altFunctionUse == 2 && player.ownedProjectileCounts[ModContent.ProjectileType<NamelessMurasamaSpin>()] > 0)
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
                SoundEngine.PlaySound(MurasamaTornado with { PitchVariance = 0.2f }, player.Center);
                Projectile.NewProjectile(source, player.Center, velocity, ModContent.ProjectileType<NamelessMurasamaSpin>(), damage, knockback, player.whoAmI);
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
                AddIngredient(ItemID.Muramasa).
                AddIngredient<KrakenTooth>(2).
                AddIngredient<VoidSingularity>(3).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
