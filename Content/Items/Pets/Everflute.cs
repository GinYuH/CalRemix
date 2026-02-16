using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalRemix.Content.Projectiles;
using CalRemix.Content.Buffs;
using CalamityMod.Rarities;

namespace CalRemix.Content.Items.Pets
{
    public class Everflute : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.DefaultToVanitypet(ModContent.ProjectileType<WeirdBird>(), ModContent.BuffType<WeirdBirdBuff>());
            Item.width = 28;
            Item.height = 20;
            Item.rare = ModContent.RarityType<DarkBlue>();
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

