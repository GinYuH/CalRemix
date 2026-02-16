using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;
using CalRemix.Content.Projectiles.Weapons;

namespace CalRemix.Content.Items.Weapons
{
    public class Mikado : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 66;
            Item.height = 66;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.damage = 57;
            Item.knockBack = 4f;
            Item.useAnimation = 25;
            Item.useTime = 5;
            Item.autoReuse = false;
            Item.useStyle = ItemUseStyleID.Shoot;

            Item.value = CalamityGlobalItem.RarityCyanBuyPrice;
            Item.rare = ItemRarityID.Cyan;

            Item.shoot = ModContent.ProjectileType<MikadoSwing>();
            Item.shootSpeed = 24f;
        }
    }
}
