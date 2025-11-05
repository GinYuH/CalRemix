using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Projectiles.Weapons;
using CalamityMod.Items.Weapons.Melee;
using CalRemix.Content.Items.Materials;

namespace CalRemix.Content.Items.Weapons
{
    public class ParadiseInfusedMurasama : ModItem
    {

        public override void SetDefaults()
        {
            Item.CloneDefaults(ModContent.ItemType<Murasama>());
            Item.damage = 100;
            Item.value = Item.sellPrice(gold: 5);
            Item.shoot = ModContent.ProjectileType<ParadiseMurasamaSlash>();
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Muramasa).
                AddIngredient<ParadiseBlade>().
                AddIngredient<GildedShard>(5).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
