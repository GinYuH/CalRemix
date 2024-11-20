using CalamityMod.Items;
using CalRemix.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons
{
    public class Quadnoculars : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Quadnoculars");
            Item.staff[Type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.useTime = 120;
            Item.useAnimation = 120;
            Item.damage = 4;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Generic;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = CalamityGlobalItem.RarityYellowBuyPrice;
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = ModContent.ProjectileType<QuadLaser>();
            Item.shootSpeed = 44;
        }
        public override void HoldItem(Player player)
        {
            player.scope = true;
        }
    }
}
