using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CalamityMod.Items;
using CalRemix.Projectiles.Weapons;
using CalRemix.Projectiles;
using Terraria.Audio;
using CalamityMod.Sounds;

namespace CalRemix.Items.Weapons
{
    public class WaraxeReloaded : ModItem
    {
        int currentDir = 1;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Waraxe: Reloaded");
            Tooltip.SetDefault("Swings an axe several times while performing combos\nBreaks in Hardmode");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 60;
            Item.height = 66;
            Item.damage = 145;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 60;
            Item.useTime = 60;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.knockBack = 7f;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.value = CalamityGlobalItem.RarityBlueBuyPrice;
            Item.rare = ItemRarityID.Blue;
            Item.shoot = ModContent.ProjectileType<WaraxeReloadedHoldout>();
            Item.shootSpeed = 60f;
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0 && !Main.hardMode;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (currentDir == 0)
                currentDir = 1;
            int aivar = currentDir == 5 ? 3 : currentDir == 1 || currentDir == 3 ? 1 : 2;
            int p = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, ai2: aivar);
            if (aivar == 3)
            {
                Main.projectile[p].timeLeft = 120;
                SoundEngine.PlaySound(CommonCalamitySounds.MeatySlashSound with { Pitch = -0.2f }, player.Center);
            }
            else
            {
                SoundEngine.PlaySound(CommonCalamitySounds.SwiftSliceSound with { Pitch = -0.4f }, player.Center);
            }
            currentDir++;
            if (currentDir >= 6)
                currentDir = 1;

            return false;
        }
    }
}
