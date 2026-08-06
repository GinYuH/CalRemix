using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CalamityMod.Items;
using CalRemix.Content.Projectiles.Weapons;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Weapons.Rogue;

namespace CalRemix.Content.Items.Weapons
{
    public class WisdomTeethFragments : RogueWeapon
    {
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.rare = RarityHelper.Hardmode;
            Item.value = CalamityGlobalItem.RarityLightRedBuyPrice;
            Item.useTime = 9;
            Item.useAnimation = 9;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.UseSound = BetterSoundID.HitDrippler;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.damage = 25;
            Item.knockBack = 4f;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<WisdomTeeth>();
            Item.shootSpeed = 14f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CalamityPlayer calamityPlayer = player.Calamity();
            int ct = calamityPlayer.StealthStrikeAvailable() ? 14 : 1;
            for (int i = 0; i < ct; i++)
            {
                int num = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                Main.projectile[num].frame = Main.rand.Next(0, 14);
                if (calamityPlayer.StealthStrikeAvailable())
                {
                    Main.projectile[num].frame = i;
                    Main.projectile[num].Calamity().stealthStrike = true;
                }
                Main.projectile[num].netUpdate = true;
            }

            return false;
        }
    }
}
