using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalRemix.Content.Projectiles;
using CalRemix.Content.Buffs;

namespace CalRemix.Content.Items.Pets
{
    public class OblivionRattle : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.DefaultToVanitypet(ModContent.ProjectileType<BabyNoxus>(), ModContent.BuffType<BabyNoxusBuff>());
            Item.width = 28;
            Item.height = 20;
            Item.rare = ItemRarityID.Master;
            Item.master = true;
            Item.value = Item.sellPrice(0, 5);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // The item applies the buff, the buff spawns the projectile.
            player.AddBuff(Item.buffType, 2);
            return false;
        }
    }
}

