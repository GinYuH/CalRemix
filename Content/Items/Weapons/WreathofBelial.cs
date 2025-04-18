using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.CalPlayer;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalRemix.Content.Projectiles.Weapons;
using CalamityMod.Items;
using CalamityMod.Items.Weapons.Rogue;

namespace CalRemix.Content.Items.Weapons
{
    public class WreathofBelial: RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            // DisplayName.SetDefault("Wreath of Belial");
            // Tooltip.SetDefault("Throws a festive wreath that grows in size until exploding into icicle shards\nStealth strikes cause the wreath to suck in nearby enemies");
        }
        public override void SetDefaults()
        {
            Item.width = 1;
            Item.height = 1;
            Item.rare = ItemRarityID.Yellow;
            Item.value = CalamityGlobalItem.RarityYellowBuyPrice;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.damage = 97;
            Item.knockBack = 1f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<WreathProjectile>();
            Item.shootSpeed = 4;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CalamityPlayer calamityPlayer = player.Calamity();
            if (calamityPlayer.StealthStrikeAvailable())
            {
                int num = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                if (num.WithinBounds(1000))
                {
                    Main.projectile[num].Calamity().stealthStrike = true;
                }

                return false;
            }
            else
                return true;
        }
    }
}
