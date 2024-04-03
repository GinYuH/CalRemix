using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using CalamityMod.Items;
using Terraria.ModLoader;
using CalRemix.Projectiles.Weapons;
using CalamityMod.Projectiles.Melee;

namespace CalRemix.Items.Weapons
{
    public class ChristmasCarol : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Christmas Carol");
            Tooltip.SetDefault("Summons a carol bell to support you\nHitting the bell with a whip causes it to unleash a damaging sound pulse");
        }

        public override void SetDefaults()
        {
            Item.damage = 205;
            Item.mana = 10;
            Item.width = 40;
            Item.height = 42;
            Item.useTime = Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true; 
            Item.sentry = true;
            Item.knockBack = 5f;
            Item.value = CalamityGlobalItem.Rarity8BuyPrice;
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = PwnagehammerProj.UseSoundFunny;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<CarolBell>();
            Item.shootSpeed = 10f;
            Item.DamageType = DamageClass.Summon;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int p = Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, damage, knockback, player.whoAmI);
            if (Main.projectile.IndexInRange(p))
                Main.projectile[p].originalDamage = Item.damage;
            player.UpdateMaxTurrets();
            return false;
        }
    }
}
