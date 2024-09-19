using CalamityMod;
using CalamityMod.Items.Materials;
using CalRemix.Content.Projectiles.Weapons;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons
{
    public class WulfrumLeechDagger : ModItem
    {
        public static readonly SoundStyle LeechDaggerSwing = new("CalRemix/Assets/Sounds/LeechDaggerSwing")
        {
            PitchVariance = 0.4f
        };

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }

        public override void SetDefaults()
        {
            Item.damage = 1;
            Item.knockBack = 0f;
            Item.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();

            Item.useStyle = ItemUseStyleID.Rapier;
            Item.useAnimation = 56;
            Item.useTime = 56;
            Item.autoReuse = true;
            Item.UseSound = LeechDaggerSwing;

            Item.width = 34;
            Item.height = 34;

            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.maxStack = 999;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 10, 0);

            Item.shoot = ModContent.ProjectileType<WulfrumLeechDaggerSwordProjectile>();
            Item.shootSpeed = 1.6f;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<WulfrumMetalScrap>(4)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void ModifyWeaponCrit(Player player, ref float crit)
        {
            crit = 0f;
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            damage *= 0;
            damage.Flat = 1;
        }
    }
}