using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;
using CalRemix.Content.Projectiles.Weapons;

namespace CalRemix.Content.Items.Weapons
{
    public class Aerospray : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aerospray");
            Tooltip.SetDefault("Releases pressurized mist that increases in size but decreases in damage as it travels");
        }
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.rare = RarityHelper.Oxygen;
            Item.value = CalamityGlobalItem.RarityLimeBuyPrice;
            Item.useTime = 27;
            Item.useAnimation = 27;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.UseSound = BetterSoundID.ItemFlamethrower;
            Item.DamageType = DamageClass.Magic;
            Item.damage = 89;
            Item.knockBack = 1f;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<AerosprayMist>();
            Item.shootSpeed = 8f;
            Item.mana = 6;
        }
    }
}
