using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Rarities;
using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons
{
    public class SignoftheOperator : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToWhip(ModContent.ProjectileType<OperatorWhipEnd>(), 1029, 2, 3, 40);
            Item.shoot = ModContent.ProjectileType<OperatorWhipPage>();
            Item.rare = ModContent.RarityType<DarkBlue>();
            Item.channel = true;
            Item.autoReuse = true;
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.useStyle = ItemUseStyleID.Shoot;
        }

        // Makes the whip receive melee prefixes
        public override bool MeleePrefix()
        {
            return true;
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;

        public override void HoldItem(Player player) => player.Calamity().mouseWorldListener = true;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return true;
        }
    }
}