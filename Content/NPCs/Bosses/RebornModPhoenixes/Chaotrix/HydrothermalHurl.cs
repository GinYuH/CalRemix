using CalRemix.Content.NPCs.Bosses.RebornModPhoenixes.Vernix;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Bosses.RebornModPhoenixes.Chaotrix
{
    public class HydrothermalHurl : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;
            Item.damage = 78;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 40;
            Item.useTime = 50;
            Item.useAnimation = 50;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 7f;
            Item.value = 20000;
            Item.rare = ItemRarityID.Yellow;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<PerennialFlowerMine>();
            Item.shootSpeed = 13f;
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
