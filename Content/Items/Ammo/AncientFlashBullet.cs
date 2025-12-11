using CalamityMod.Items.Ammo;
using CalRemix.Content.Projectiles.Weapons;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Ammo
{
    public class AncientFlashBullet : ModItem
    {
        public override void Load()
        {
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<FlashRound>();
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Ancient Flash Round");
            // Tooltip.SetDefault("Gives off a concussive blast that confuses enemies in a large area for a short time");
        }

        public override void SetDefaults()
        {
            Item.damage = 7;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 8;
            Item.height = 8;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.knockBack = 1.15f;
            Item.value = 250;
            Item.rare = ItemRarityID.Blue;
            Item.shoot = ModContent.ProjectileType<AncientFlashBulletProj>();
            Item.shootSpeed = 12f;
            Item.ammo = 97;
            Item.maxStack = 9999;
        }
    }
}