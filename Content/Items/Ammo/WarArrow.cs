using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Projectiles;

namespace CalRemix.Content.Items.Ammo
{
    public class WarArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("War Arrow");
            // Tooltip.SetDefault("Inflicts Armor Crunch");
            Item.ResearchUnlockCount = 99;
        }

        public override void SetDefaults()
        {
            Item.damage = 9;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 8;
            Item.height = 8;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.knockBack = 1.5f;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(copper: 12);
            Item.shoot = ModContent.ProjectileType<WarrowProjectile>();
            Item.shootSpeed = 13f;
            Item.ammo = AmmoID.Arrow;
        }
    }
}
