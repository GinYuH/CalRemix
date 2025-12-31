using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.Items;
using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework.Audio;
using Terraria.Audio;

namespace CalRemix.Content.Items.Weapons
{
    public class Glockarina : ModItem, ILocalizedModType
    {
        public static SoundStyle Occarina = new SoundStyle("CalRemix/Assets/Sounds/Ocarina");

        public override void SetDefaults()
        {
            Item.width = 56;
            Item.height = 34;
            Item.damage = 725;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 9f;
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.rare = ModContent.RarityType<CosmicPurple>();
            Item.UseSound = null;
            Item.autoReuse = true;
            Item.shootSpeed = 30;
            Item.shoot = ModContent.ProjectileType<ZalGlock>();
            if (CalRemixAddon.Thorium != null)
            {
                Item.DamageType = CalRemixAddon.Thorium.Find<DamageClass>("BardDamage");
            }
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(10, 0);
        }
    }
}
