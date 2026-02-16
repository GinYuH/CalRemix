using CalRemix.Content.NPCs.Bosses.RebornModPhoenixes.Vernix;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Bosses.RebornModPhoenixes.Cryonix
{
    public class FlashFreeze : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;
            Item.damage = 100;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 20;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 7f;
            Item.value = 20000;
            Item.rare = ItemRarityID.Pink;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<CryoIcicle>();
            Item.shootSpeed = 13f;
            Item.UseSound = SoundID.Item8;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int b = Projectile.NewProjectile(source, position, new Vector2(0, 25), type, damage, knockback);
            Main.projectile[b].friendly = true;
            Main.projectile[b].hostile = false;
            return false;
        }
    }
}
