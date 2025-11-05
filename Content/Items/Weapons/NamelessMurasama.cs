using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Projectiles.Weapons;
using CalamityMod.Items.Weapons.Melee;
using CalRemix.Content.Items.Materials;

namespace CalRemix.Content.Items.Weapons
{
    public class NamelessMurasama : ModItem
    {
        public override string Texture => "CalamityMod/Items/Weapons/UHFMurasama";

        public override void SetDefaults()
        {
            Item.CloneDefaults(ModContent.ItemType<Murasama>());
            Item.damage = 200;
            Item.value = Item.sellPrice(gold: 50);
            Item.shoot = ModContent.ProjectileType<NamelessMurasamaSlash>();
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
