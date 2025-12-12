using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Bosses.RebornModPhoenixes.Vernix
{
    public class BlossomBlast : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;
            Item.damage = 98;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 20;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 7f;
            Item.value = 20000;
            Item.rare = ItemRarityID.Lime;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<PerennialFlowerMine>();
            Item.shootSpeed = 18f;
            Item.UseSound = SoundID.Item8;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int b = Projectile.NewProjectile(source, position, velocity, type, damage, knockback);
            Main.projectile[b].friendly = true;
            Main.projectile[b].hostile = false;
            return false;
        }
    }
}
