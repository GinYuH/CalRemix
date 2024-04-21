using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalRemix.Projectiles.Weapons;
using CalamityMod;

namespace CalRemix.Items.Weapons
{
    public class Snowgrave : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Snowgrave");
            Tooltip.SetDefault("Memories of a snowy grave...\nSummons a large deadly fountain of ice and snow below your cursor that instantly kills normal enemies\nAfter usage, the player won't be able to use the weapon again for 1 minute");
        }
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Yellow;
            Item.value = CalamityGlobalItem.RarityYellowBuyPrice;
            Item.useTime = 1480;
            Item.useAnimation = 1480;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.UseSound = SoundID.NPCHit5 with { Pitch = -1.8f };
            Item.DamageType = DamageClass.Magic;
            Item.damage = 2002;
            Item.knockBack = 0f;
            Item.mana = 200;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<SnowgraveSigil>();
            Item.shootSpeed = 12f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, Main.MouseWorld + Vector2.UnitY * 100, Vector2.Zero, type, damage, 0f, player.whoAmI);
            return false;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] <= 0 && !player.HasCooldown(SnowgraveCooldown.ID);
        }
    }
}
