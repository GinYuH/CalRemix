using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;
using CalRemix.Content.Projectiles.Weapons;
using CalamityMod.Rarities;

namespace CalRemix.Content.Items.Weapons
{
    public class TheFirestorm : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Rapidly fires molten globs which follow your cursor");
        }
        public override void SetDefaults()
        {
            Item.width = 66;
            Item.height = 66;
            Item.damage = 125;
            Item.DamageType = DamageClass.Magic;
            Item.channel = true;
            Item.mana = 18;
            Item.useTime = 26;
            Item.useAnimation = 26;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 3;
            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
            Item.rare = ModContent.RarityType<Turquoise>();
            Item.UseSound = BetterSoundID.ItemInfernoFork;
            Item.shoot = ModContent.ProjectileType<FirestormLava>();
            Item.shootSpeed = 9f;
            Item.autoReuse = true;
        }
    }
}
