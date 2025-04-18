using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;
using CalRemix.Content.Projectiles;

namespace CalRemix.Content.Items.Weapons
{
    public class Warstaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("War Staff");
            // Tooltip.SetDefault("Casts a ball of gray energy that breaks enemy defense on hit");
            Item.staff[Type] = true;
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.rare = ItemRarityID.Blue;
            Item.value = CalamityGlobalItem.RarityBlueBuyPrice;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.UseSound = BetterSoundID.ItemMagicStaff;
            Item.DamageType = DamageClass.Magic;
            Item.damage = 12;
            Item.knockBack = 2.5f;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<WarBolt>();
            Item.shootSpeed = 11f;
            Item.mana = 10;
        }
    }
}
